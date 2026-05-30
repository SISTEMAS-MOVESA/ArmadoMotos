DECLARE @nmodelo nvarchar(30);
set @nmodelo = (select [Name] from movesa..[@AMODELO] where Code = @modelo)

DECLARE @StartDate date = DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 2, 0);
DECLARE @EndDate   date = CAST(GETDATE() AS date);

;WITH Movs AS (
    -- Facturas: usa el WhsCode físico de la línea (sin remapeo por DICCIONARIO)
    SELECT
        t1.WhsCode COLLATE Modern_Spanish_CI_AS AS whscode,
        CAST(t1.Quantity AS decimal(19,6)) AS Monto
    FROM MOVESA..OINV t0 WITH (NOLOCK)
    JOIN MOVESA..INV1 t1 WITH (NOLOCK) ON t0.DocEntry=t1.DocEntry
    JOIN MOVESA..OITM t2 WITH (NOLOCK) ON t1.ItemCode=t2.ItemCode
    WHERE t0.DocDate >= @StartDate AND t0.DocDate <= @EndDate
      AND t2.ItemType='I'
      AND t2.ItmsGrpCod=154
      AND (t2.U_AMODELO=@modelo OR t2.U_MODELO=@modelo)
      AND t0.slpcode NOT IN (117,332)
      AND t0.CANCELED = 'N'

    UNION ALL

    -- Notas de crédito (negativo): mismo criterio
    SELECT
        t1.WhsCode COLLATE Modern_Spanish_CI_AS AS whscode,
        CAST(-t1.Quantity AS decimal(19,6)) AS Monto
    FROM MOVESA..ORIN t0 WITH (NOLOCK)
    JOIN MOVESA..RIN1 t1 WITH (NOLOCK) ON t0.DocEntry=t1.DocEntry
    JOIN MOVESA..OITM t2 WITH (NOLOCK) ON t1.ItemCode=t2.ItemCode
    WHERE t0.DocDate >= @StartDate AND t0.DocDate <= @EndDate
      AND t2.ItemType='I'
      AND t2.ItmsGrpCod=154
      AND (t2.U_AMODELO=@modelo OR t2.U_MODELO=@modelo)
      AND t0.slpcode NOT IN (117,332)
      AND t0.CANCELED = 'N'
),
Agg AS (
    SELECT whscode, SUM(Monto) AS Monto
    FROM Movs
    GROUP BY whscode
),
Rnk AS (
    SELECT
        ROW_NUMBER() OVER (ORDER BY Monto DESC) AS [Row],
        whscode,
        Monto,
        SUM(Monto) OVER () AS TotalMonto,
        SUM(Monto) OVER (ORDER BY Monto DESC
                         ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS RunningMonto
    FROM Agg
)
SELECT
    [Row],
    whscode,
    CAST(Monto AS decimal(19,6)) AS Monto,
    CAST(RunningMonto / NULLIF(TotalMonto,0) AS decimal(19,6)) AS ParetoPercent
INTO #MyTempTable
FROM Rnk
ORDER BY [Row];

SELECT *
    ,(SELECT whsname FROM movesa..owhs WITH(NOLOCK)
        WHERE WhsCode = #MyTempTable.whscode COLLATE Modern_Spanish_CI_AS) [Nombre Almacen]
    ,ISNULL((SELECT [MAXIMO] FROM [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV]
                WHERE [WHSCODE] = #MyTempTable.whscode COLLATE Modern_Spanish_CI_AS
                  AND [MODELO] COLLATE Modern_Spanish_CI_AS = @nmodelo), 0) [CB]
    ,ISNULL((SELECT CONVERT(INT,SUM(t11.Quantity))
                FROM [MOVESA].[dbo].[ORDR] t10 WITH(NOLOCK)
                INNER JOIN [MOVESA].[dbo].RDR1 t11 WITH(NOLOCK) ON t10.DocEntry=t11.DocEntry
                INNER JOIN [MOVESA].[dbo].OITM t12 WITH(NOLOCK) ON t11.ItemCode=t12.ItemCode
                INNER JOIN [MOVESA].[dbo].[@AMODELO] t13 WITH(NOLOCK) ON t12.U_AMODELO=t13.Code
                WHERE t12.ItmsGrpCod=154
                  AND t10.CANCELED='N'
                  AND t11.LineStatus='O'
                  AND t11.WhsCode = #MyTempTable.whscode COLLATE Modern_Spanish_CI_AS
                  AND t13.Code = @modelo
                GROUP BY t11.WhsCode, t13.Name), 0) AS [Comprometido]
    ,ISNULL((SELECT CONVERT(INT,SUM(t11.Quantity))
                FROM [MOVESA].[dbo].[OWTQ] t10 WITH(NOLOCK)
                INNER JOIN [MOVESA].[dbo].WTQ1 t11 WITH(NOLOCK) ON t10.DocEntry=t11.DocEntry
                INNER JOIN [MOVESA].[dbo].OITM t12 WITH(NOLOCK) ON t11.ItemCode=t12.ItemCode
                INNER JOIN [MOVESA].[dbo].[@AMODELO] t13 WITH(NOLOCK) ON t12.U_AMODELO=t13.Code
                WHERE t12.ItmsGrpCod=154
                  AND t10.CANCELED='N'
                  AND t11.LineStatus='O'
                  AND t11.WhsCode = 'T' + RTRIM(#MyTempTable.whscode COLLATE Modern_Spanish_CI_AS)
                  AND t13.Code = @modelo
                GROUP BY t11.WhsCode, t13.Name), 0) AS [Solicitado]
    ,ISNULL((SELECT CONVERT(INT,SUM(t100.ONHAND))
                FROM MOVESA..OITW t100 WITH(NOLOCK)
                JOIN MOVESA..OITM t101 WITH(NOLOCK) ON t100.ItemCode=t101.ItemCode
                WHERE (t101.U_AMODELO = @modelo OR t101.U_MODELO = @modelo)
                  AND t101.ItmsGrpCod=154
                  AND t100.WhsCode = 'T' + RTRIM(#MyTempTable.whscode COLLATE Modern_Spanish_CI_AS)
            ), 0) AS [Transito]
    ,(SELECT CONVERT(INT,SUM(t100.ONHAND))
                FROM MOVESA..OITW t100 WITH(NOLOCK)
                JOIN MOVESA..OITM t101 WITH(NOLOCK) ON t100.ItemCode=t101.ItemCode
                WHERE (t101.U_AMODELO = @modelo OR t101.U_MODELO = @modelo)
                  AND t101.ItmsGrpCod=154
                  AND t100.WhsCode = #MyTempTable.whscode COLLATE Modern_Spanish_CI_AS
    ) AS [Fisico]
FROM #MyTempTable ORDER BY [Row];

DROP TABLE #MyTempTable;
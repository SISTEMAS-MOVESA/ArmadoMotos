USE [ArmadoMotos]
GO

/****** Object:  StoredProcedure [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS]    Script Date: 5/12/2026 1:11:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		ROGER JOVEL
-- Create date: 11/12/2020
-- Description:	PRE CARGA EL PARETO PARA LUEGO MOSTRAR EL CUADRO BASICO POR SUCURSAL
-- =============================================
CREATE PROCEDURE [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS]
@WHSCODE NVARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;



	DECLARE @CD NVARCHAR(6)
SET @CD = (SELECT CODIGOCD FROM ARMADOMOTOS..DICCIONARIO WHERE WhsCode = @WHSCODE)
DECLARE @greatSum decimal(19,6)

-- Limpiar tabla de resultados
DELETE FROM armadomotos..PARETOBY_WHSCODE

-- Procesar períodos de 3, 6 y 12 meses
DECLARE @FechaBase DATE = EOMONTH(DATEADD(MONTH, -1, GETDATE()));

-- Procesar períodos de 3, 6 y 12 meses
DECLARE @i INT = 3;
WHILE (@i <= 12)
BEGIN
    -- Calcular fechas del período
    DECLARE @F1 DATE = DATEADD(MONTH, -(@i-1), DATEFROMPARTS(YEAR(@FechaBase), MONTH(@FechaBase), 1));
    DECLARE @F2 DATE = getdate();
    --DECLARE @F2 DATE = @FechaBase;
    
    -- Mostrar las fechas usadas para verificación (puedes eliminar esto en producción)
    PRINT 'Período de ' + CAST(@i AS VARCHAR) + ' meses:';
    PRINT '  Desde: ' + CONVERT(VARCHAR, @F1, 103);
    PRINT '  Hasta: ' + CONVERT(VARCHAR, @F2, 103);
	--DECLARE @i INT = 3;
--WHILE (@i <= 12)
--BEGIN
--    -- Definir fechas del período
--    DECLARE @F1 DATE = DATEFROMPARTS(YEAR(DATEADD(MONTH, -@i, GETDATE())), MONTH(DATEADD(MONTH, -@i, GETDATE())), 1);
--    DECLARE @F2 DATE = EOMONTH(@F1);

    -- Crear tabla temporal con datos consolidados de ventas y devoluciones
    SELECT 
        ROW_NUMBER() OVER(ORDER BY SUM(t.CANTIDAD) DESC, SUM(t.Monto) DESC) AS RowNum,
        t.Code,
        t.MODELO,
        SUM(t.CANTIDAD) AS Cantidad,
        SUM(t.Monto) AS Monto,
        CONVERT(DECIMAL(19,6), 0) AS ParetoPercent
    INTO #TempPareto
    FROM 
    (
        -- Ventas
        SELECT 
            T5.CODE,
            T5.Name AS MODELO,
            SUM(T1.Quantity) AS CANTIDAD,
            SUM(T1.linetotal) AS Monto
        FROM MOVESA..OINV T0 WITH (NOLOCK)
        INNER JOIN MOVESA..INV1 T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry
        INNER JOIN MOVESA..OITM T2 WITH (NOLOCK) ON T2.ItemCode = T1.ItemCode
        LEFT JOIN MOVESA..[@AMODELO] T5 WITH (NOLOCK) ON T2.U_MODELO = T5.Code
        INNER JOIN MOVESA..OWHS T6 WITH (NOLOCK) ON T1.WhsCode = T6.WhsCode
        INNER JOIN MOVESA..OSLP T7 WITH (NOLOCK) ON T0.SlpCode = T7.SlpCode
        WHERE 
            T0.DocDate BETWEEN @F1 AND @F2
            AND T2.ItmsGrpCod = '154'
            AND T0.CANCELED = 'N'
            AND T6.WhsCode = @WHSCODE
            --AND T7.SlpName LIKE @CD
            AND ISNULL(T5.u_lg5, '-') = 'Y'
            --AND T6.U_TYPE = 'PRO'
        GROUP BY T5.CODE, T5.Name

        UNION ALL

        -- Devoluciones
        SELECT 
            T5.CODE,
            T5.Name AS MODELO,
            -SUM(T1.Quantity) AS CANTIDAD,
            -SUM(T1.linetotal) AS Monto
        FROM MOVESA..ORIN T0 WITH (NOLOCK)
        INNER JOIN MOVESA..RIN1 T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry
        INNER JOIN MOVESA..OITM T2 WITH (NOLOCK) ON T2.ItemCode = T1.ItemCode
        LEFT JOIN MOVESA..[@AMODELO] T5 WITH (NOLOCK) ON T2.U_MODELO = T5.Code
        INNER JOIN MOVESA..OWHS T6 WITH (NOLOCK) ON T1.WhsCode = T6.WhsCode
        INNER JOIN MOVESA..OSLP T7 WITH (NOLOCK) ON T0.SlpCode = T7.SlpCode
        WHERE 
            T0.DocDate BETWEEN @F1 AND @F2
            AND T2.ItmsGrpCod = '154'
            AND T0.CANCELED = 'N'
            AND T6.WhsCode = @WHSCODE
            --AND T7.SlpName LIKE @CD
            AND ISNULL(T5.u_lg5, '-') = 'Y'
            --AND T6.U_TYPE = 'PRO'
        GROUP BY T5.CODE, T5.Name
    ) AS t
    GROUP BY t.CODE, t.MODELO;

    -- Calcular suma total para Pareto
    SET @greatSum = (SELECT SUM(Cantidad) FROM #TempPareto WITH (NOLOCK));

    -- Actualizar porcentajes de Pareto
    UPDATE #TempPareto
    SET ParetoPercent = (
        SELECT SUM(t2.Cantidad) / @greatSum
        FROM #TempPareto t2
        WHERE t2.RowNum <= #TempPareto.RowNum
    );

    -- Insertar resultados en tabla permanente
    INSERT INTO armadomotos..PARETOBY_WHSCODE
    SELECT 
        RowNum,
        Code,
        MODELO,
        Cantidad AS UNDS,
        Monto,
        ParetoPercent AS PARETO,
        CASE 
            WHEN ParetoPercent < 0.9 THEN 'A'
            WHEN ParetoPercent >= 0.9 AND ParetoPercent < 1 THEN 'B'
            ELSE 'C'			
        END AS Categoria,
        @i AS Meses
    FROM #TempPareto WITH (NOLOCK)
    ORDER BY RowNum;

    DROP TABLE #TempPareto;

    -- Avanzar al siguiente período (3, 6, 12 meses)
    SET @i = @i * 2;
END

-- Generar reporte final (Heatmap)
SELECT 
    t2.whscode, t2.whsname,
    ISNULL(t1.ROWID, 99) AS ROWID,
    T0.CODE,
    T0.NAME AS MODELO,
    ISNULL(T1.[3m], 0) AS [3m],
    ISNULL(t1.[6m], 0) AS [6m],
    ISNULL(t1.[12m], 0) AS [12m],
    ISNULL(t1.[3mp], 0) AS [3mp],
    ISNULL(t1.[6mp], 0) AS [6mp],
    ISNULL(t1.[12mp], 0) AS [12mp],
    (SELECT CONVERT(INT, SUM(t100.ONHAND))
     FROM movesa..OITW t100 WITH (NOLOCK)
     JOIN movesa..OITM T101 WITH (NOLOCK) ON t100.ItemCode = t101.ItemCode
     WHERE T101.U_MODELO = t0.CODE AND T100.WhsCode = 'DCM00' AND T101.ItmsGrpCod = 154) AS [DCM00],
    ISNULL(ISNULL(t1.[CB], (SELECT [MAXIMO] FROM [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] 
                           WHERE WHSCODE = @whscode AND MODELO COLLATE SQL_Latin1_General_CP850_CI_AS = T0.Name)), 0) AS [CB],
    ISNULL(t1.Comprometido, 0) AS [Comprometido],
    ISNULL(t1.Solicitado, 0) AS [Solicitado],
    ISNULL(t1.Transito, 0) AS [Transito],
    ISNULL(t1.Fisico, (SELECT CONVERT(INT, SUM(t100.ONHAND))
                      FROM movesa..OITW t100 WITH (NOLOCK)
                      JOIN movesa..OITM T101 WITH (NOLOCK) ON t100.ItemCode = t101.ItemCode
                      WHERE T101.U_MODELO = t0.CODE AND T100.WhsCode = @WHSCODE AND T101.ItmsGrpCod = 154)) AS [Fisico],
    ISNULL(t1.Faltante, 0) AS [Faltante],
    ISNULL(t1.Actual, 0) AS [Actual],
    ISNULL(t1.A, 0) AS [A],
    ISNULL(t1.B, 0) AS [B],
    ISNULL(t1.C, 0) AS [C],
    ISNULL(t1.Actual, 0) + ISNULL(t1.A, 0) + ISNULL(t1.B, 0) + ISNULL(t1.C, 0) AS [Total],
    (SELECT TOP 1 CONVERT(CHAR, t10.DocDate, 103)
     FROM [MOVESA].[dbo].[oinv] t10 WITH (NOLOCK) 
     JOIN [MOVESA].[dbo].INV1 T11 WITH (NOLOCK) ON T10.DocEntry = T11.DOCENTRY
     JOIN [MOVESA].[dbo].OITM T12 WITH (NOLOCK) ON T11.ItemCode = T12.ItemCode
     JOIN [MOVESA].[dbo].[@AMODELO] T13 WITH (NOLOCK) ON T12.U_MODELO = T13.Code 
     JOIN MOVESA..OSLP T14 WITH (NOLOCK) ON T10.SLPCODE = T14.SLPCODE
     WHERE T12.ItmsGrpCod = 154
       AND T10.CANCELED = 'N'
       AND t11.WHSCODE = @WHSCODE 
       AND t13.Code = T0.Code
     ORDER BY t10.DocEntry DESC) AS [UltVenta],
    ISNULL((SELECT SUM(QTYLOGISTICA) FROM ArmadoMotos..MACROINSERT 
            WHERE [ESTADO] = 'T' AND MODELO = T0.NAME COLLATE Modern_Spanish_CI_AS AND ALMDESTINO = @WHSCODE), 0) AS [Logistica],
    (SELECT TOP 1 t00.price 
     FROM MOVESA..ITM1 t00 WITH (NOLOCK) 
     JOIN MOVESA..oitm t11 WITH (NOLOCK) ON t00.itemcode = t11.itemcode 
     WHERE pricelist = 1 AND T11.U_MODELO = T0.CODE AND T11.ItmsGrpCod = 154) AS [PRECIOV],
    (SELECT TOP 1 ROW_NUM FROM RANKING WHERE code = T0.CODE AND WHSCODE = @WHSCODE) AS [Ranking],
    ISNULL((SELECT SUM([CANTIDAD]) FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] 
            WHERE ALMDESTINO = @WHSCODE AND [MODELO] = T0.NAME COLLATE Modern_Spanish_CI_AS 
            AND CODIGOESTADO IN ('DESPACHO ABIERTO', 'CAMION ABIERTO')), 0) AS [Despacho]
    ,(select top 1 t42.[Name]
        from	(select itemcode,ItemName,U_MODELO,u_tipo_moto,ItmsGrpCod from movesa..oitm with(nolock) where ItmsGrpCod=154) as t40  
		inner join movesa..[@amodelo] t41 on t40.U_MODELO=t41.code 
		inner join movesa..[@TIPOMOTOCICLETA] t42 with(nolock) on t40.u_tipo_moto = t42.Code
        where t41.[Code] = t0.Code)[TipoMoto]
FROM MOVESA..[@AMODELO] T0 
LEFT JOIN (
    SELECT 
        T.ROWID,
        T.CODE,
        T.MODELO,
        T.[3m],
        T.[6m],
        T.[12m],
        T.[3mp],
        T.[6mp],
        T.[12mp],
        T.DCM00,
        T.CB,
        T.Comprometido,
        T.Solicitado,
        T.Transito,
        T.Fisico,
        (T.CB + T.Comprometido - T.Solicitado - T.Transito - T.Fisico) AS [Faltante],
        ISNULL((SELECT CONVERT(INT, SUM(t11.Quantity))
               FROM [MOVESA].[dbo].[oinv] t10 WITH (NOLOCK) 
               JOIN [MOVESA].[dbo].INV1 T11 WITH (NOLOCK) ON T10.DocEntry = T11.DOCENTRY
               JOIN [MOVESA].[dbo].OITM T12 WITH (NOLOCK) ON T11.ItemCode = T12.ItemCode 
               JOIN [MOVESA].[dbo].[@AMODELO] T13 WITH (NOLOCK) ON T12.U_MODELO = T13.Code 
               JOIN MOVESA..OSLP T14 WITH (NOLOCK) ON T10.SLPCODE = T14.SLPCODE
               WHERE T12.ItmsGrpCod = 154
                 AND DATEPART(MONTH, T10.DocDate) = MONTH(GETDATE())
                 AND DATEPART(YEAR, T10.DocDate) = DATEPART(YEAR, GETDATE())
                 AND T10.CANCELED = 'N'
                 AND T11.WhsCode=@WHSCODE
				 --AND T14.SlpName LIKE @CD 
                 AND t13.Code = T.CODE), 0) AS [Actual],
        ISNULL((SELECT CONVERT(INT, SUM(t11.Quantity))
               FROM [MOVESA].[dbo].[oinv] t10 WITH (NOLOCK) 
               JOIN [MOVESA].[dbo].INV1 T11 WITH (NOLOCK) ON T10.DocEntry = T11.DOCENTRY
               JOIN [MOVESA].[dbo].OITM T12 WITH (NOLOCK) ON T11.ItemCode = T12.ItemCode 
               JOIN [MOVESA].[dbo].[@AMODELO] T13 WITH (NOLOCK) ON T12.U_MODELO = T13.Code 
               JOIN MOVESA..OSLP T14 WITH (NOLOCK) ON T10.SLPCODE = T14.SLPCODE
               WHERE T12.ItmsGrpCod = 154
                 AND DATEPART(MONTH, T10.DocDate) = DATEPART(MONTH, DATEADD(MONTH, -1, GETDATE()))
                 AND DATEPART(YEAR, T10.DocDate) = DATEPART(YEAR, DATEADD(MONTH, -1, GETDATE()))
                 AND T10.CANCELED = 'N'
                 AND T11.WhsCode=@WHSCODE
				 --AND T14.SlpName LIKE @CD 
                 AND t13.Code = T.CODE), 0) AS [A],
        ISNULL((SELECT CONVERT(INT, SUM(t11.Quantity))
               FROM [MOVESA].[dbo].[oinv] t10 WITH (NOLOCK) 
               JOIN [MOVESA].[dbo].INV1 T11 WITH (NOLOCK) ON T10.DocEntry = T11.DOCENTRY
               JOIN [MOVESA].[dbo].OITM T12 WITH (NOLOCK) ON T11.ItemCode = T12.ItemCode 
               JOIN [MOVESA].[dbo].[@AMODELO] T13 WITH (NOLOCK) ON T12.U_MODELO = T13.Code 
               JOIN MOVESA..OSLP T14 WITH (NOLOCK) ON T10.SLPCODE = T14.SLPCODE
               WHERE T12.ItmsGrpCod = 154
                 AND DATEPART(MONTH, T10.DocDate) = MONTH(DATEADD(MONTH, -2, GETDATE()))
                 AND DATEPART(YEAR, T10.DocDate) = DATEPART(YEAR, DATEADD(MONTH, -2, GETDATE()))
                 AND T10.CANCELED = 'N'
                 AND T11.WhsCode=@WHSCODE
				 --AND T14.SlpName LIKE @CD 
                 AND t13.Code = T.CODE), 0) AS [B],
        ISNULL((SELECT CONVERT(INT, SUM(t11.Quantity))
               FROM [MOVESA].[dbo].[oinv] t10 WITH (NOLOCK) 
               JOIN [MOVESA].[dbo].INV1 T11 WITH (NOLOCK) ON T10.DocEntry = T11.DOCENTRY
               JOIN [MOVESA].[dbo].OITM T12 WITH (NOLOCK) ON T11.ItemCode = T12.ItemCode 
               JOIN [MOVESA].[dbo].[@AMODELO] T13 WITH (NOLOCK) ON T12.U_MODELO = T13.Code 
               JOIN MOVESA..OSLP T14 WITH (NOLOCK) ON T10.SLPCODE = T14.SLPCODE
               WHERE T12.ItmsGrpCod = 154
                 AND DATEPART(MONTH, T10.DocDate) = MONTH(DATEADD(MONTH, -3, GETDATE()))
                 AND DATEPART(YEAR, T10.DocDate) = DATEPART(YEAR, DATEADD(MONTH, -3, GETDATE()))
                 AND T10.CANCELED = 'N'
                 AND T11.WhsCode=@WHSCODE
				 --AND T14.SlpName LIKE @CD 
                 AND t13.Code = T.CODE), 0) AS [C]
    FROM (
        SELECT 
            t0.[ROWID],
            t0.[CODE],
            t0.[MODELO],
            (SELECT [PARETOBY_WHSCODE].UNDS FROM [ArmadoMotos].[dbo].[PARETOBY_WHSCODE] WHERE MESES = 3 AND MODELO = t0.MODELO) AS [3m],
            (SELECT [PARETOBY_WHSCODE].UNDS FROM [ArmadoMotos].[dbo].[PARETOBY_WHSCODE] WHERE MESES = 6 AND MODELO = t0.MODELO) AS [6m],
            (SELECT [PARETOBY_WHSCODE].UNDS FROM [ArmadoMotos].[dbo].[PARETOBY_WHSCODE] WHERE MESES = 12 AND MODELO = t0.MODELO) AS [12m],
            (SELECT [PARETOBY_WHSCODE].PARETO FROM [ArmadoMotos].[dbo].[PARETOBY_WHSCODE] WHERE MESES = 3 AND MODELO = t0.MODELO) AS [3mp],
            (SELECT [PARETOBY_WHSCODE].PARETO FROM [ArmadoMotos].[dbo].[PARETOBY_WHSCODE] WHERE MESES = 6 AND MODELO = t0.MODELO) AS [6mp],
            (SELECT [PARETOBY_WHSCODE].PARETO FROM [ArmadoMotos].[dbo].[PARETOBY_WHSCODE] WHERE MESES = 12 AND MODELO = t0.MODELO) AS [12mp],
            (SELECT CONVERT(INT, SUM(t100.ONHAND))
             FROM movesa..OITW t100 WITH (NOLOCK)
             JOIN movesa..OITM T101 WITH (NOLOCK) ON t100.ItemCode = t101.ItemCode
             JOIN movesa..[@AMODELO] t102 WITH (NOLOCK) ON t101.U_MODELO = t102.Code 
             WHERE T102.Code = t0.CODE AND T100.WhsCode = 'DCM00' AND T101.ItmsGrpCod = 154) AS [DCM00],
            ISNULL((SELECT [MAXIMO] FROM [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] 
                   WHERE [WHSCODE] = @WHSCODE AND [MODELO] COLLATE Modern_Spanish_CI_AS = T0.MODELO), 0) AS [CB],
            ISNULL((SELECT CONVERT(INT, SUM(t11.Quantity))
                   FROM [MOVESA].[dbo].[ORDR] t10 WITH (NOLOCK) 
                   JOIN [MOVESA].[dbo].RDR1 T11 WITH (NOLOCK) ON T10.DocEntry = T11.DOCENTRY
                   JOIN [MOVESA].[dbo].OITM T12 WITH (NOLOCK) ON T11.ItemCode = T12.ItemCode 
                   JOIN [MOVESA].[dbo].[@AMODELO] T13 WITH (NOLOCK) ON T12.U_MODELO = T13.Code
                   WHERE T12.ItmsGrpCod = 154
                     AND T10.CANCELED = 'N'
                     AND T11.LineStatus = 'O'
                     AND T12.ItmsGrpCod = 154
                     AND T11.WhsCode = @WHSCODE
                     AND t13.Code = t0.CODE 
                   GROUP BY t11.WhsCode, T13.Name), 0) AS [Comprometido],
            ISNULL((SELECT CONVERT(INT, SUM(t11.Quantity))
                   FROM [MOVESA].[dbo].[OWTQ] t10 WITH (NOLOCK) 
                   JOIN [MOVESA].[dbo].WTQ1 T11 WITH (NOLOCK) ON T10.DocEntry = T11.DOCENTRY
                   JOIN [MOVESA].[dbo].OITM T12 WITH (NOLOCK) ON T11.ItemCode = T12.ItemCode 
                   JOIN [MOVESA].[dbo].[@AMODELO] T13 WITH (NOLOCK) ON T12.U_MODELO = T13.Code
                   WHERE T12.ItmsGrpCod = 154
                     AND T10.CANCELED = 'N'
                     AND T11.LineStatus = 'O'
                     AND T12.ItmsGrpCod = 154
                     AND T11.WhsCode = 'T' + RTRIM(@WHSCODE)
                     AND t13.Code = t0.CODE 
                   GROUP BY t11.WhsCode, T13.Name), 0) AS [Solicitado],
            ISNULL((SELECT CONVERT(INT, SUM(t100.ONHAND))
                   FROM MOVESA..OITW t100 WITH (NOLOCK)
                   JOIN MOVESA..OITM T101 WITH (NOLOCK) ON t100.ItemCode = t101.ItemCode
                   JOIN MOVESA..[@AMODELO] t102 WITH (NOLOCK) ON t101.U_MODELO = t102.Code 
                   WHERE T102.Code = T0.CODE AND T101.ItmsGrpCod = 154 AND t100.WhsCode = 'T' + RTRIM(@WHSCODE)), 0) AS [Transito],
            (SELECT CONVERT(INT, SUM(t100.ONHAND))
             FROM movesa..OITW t100 WITH (NOLOCK)
             JOIN movesa..OITM T101 WITH (NOLOCK) ON t100.ItemCode = t101.ItemCode
             JOIN movesa..[@AMODELO] t102 WITH (NOLOCK) ON t101.U_MODELO = t102.Code 
             WHERE T102.Code = t0.CODE AND T100.WhsCode = @WHSCODE AND T101.ItmsGrpCod = 154) AS 'Fisico'
			,isnull((SELECT sum([CANTIDAD])  FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] WHERE ALMDESTINO = @WHSCODE and [MODELO] = t0.MODELO collate Modern_Spanish_CI_AS AND CODIGOESTADO IN ('DESPACHO ABIERTO','CAMION ABIERTO') ),0)[Despacho]

        FROM [ArmadoMotos].[dbo].[PARETOBY_WHSCODE] t0 
        WHERE t0.MESES = 3
    ) AS T
) t1 ON T0.CODE = t1.CODE
inner join movesa..owhs as t2 on t2.whscode = @WHSCODE
WHERE ISNULL(T0.u_lg5, '-') = 'Y'
ORDER BY [ROWID];

------------------------fin--------------------------

END

GO


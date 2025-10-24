DECLARE @nmodelo nvarchar(30);

set @nmodelo = (select [Name] from movesa..[@AMODELO] where Code = @modelo)

DECLARE @StartDate date = DATEADD(month, DATEDIFF(month, 0, DATEADD(month, -6, GETDATE())), 0);
DECLARE @EndDate   date = CAST(GETDATE() AS date);  -- hoy

;WITH Movs AS (
    -- Facturas
    SELECT
        CASE t3.U_Type WHEN 'PRO'
            THEN (SELECT WHSCODE FROM ArmadoMotos.dbo.DICCIONARIO
                  WHERE LEFT(CODIGOCD,5) COLLATE Modern_Spanish_CI_AS = LEFT(t4.SlpName,5))
            ELSE t3.WhsCode COLLATE Modern_Spanish_CI_AS
        END AS whscode,
        CAST(t1.Quantity AS decimal(19,6)) AS Monto
    FROM MOVESA..OINV t0 WITH (NOLOCK)
    JOIN MOVESA..INV1 t1 WITH (NOLOCK) ON t0.DocEntry=t1.DocEntry
    JOIN MOVESA..OITM t2 WITH (NOLOCK) ON t1.ItemCode=t2.ItemCode
    JOIN MOVESA..OWHS t3 WITH (NOLOCK) ON t1.WhsCode=t3.WhsCode
    JOIN (SELECT SlpCode, SlpName FROM MOVESA..OSLP WITH (NOLOCK)) t4 ON t0.SlpCode=t4.SlpCode
    WHERE t0.DocDate >= @StartDate AND t0.DocDate <= @EndDate
      AND t2.ItemType='I' AND t2.ItmsGrpCod=154 AND t2.U_AMODELO=@modelo
      AND t0.slpcode not in (117,332)
    UNION ALL

    -- Notas de crédito (negativo)
    SELECT
        CASE t3.U_Type WHEN 'PRO'
            THEN (SELECT WHSCODE FROM ArmadoMotos.dbo.DICCIONARIO
                  WHERE LEFT(CODIGOCD,5) COLLATE Modern_Spanish_CI_AS = LEFT(t4.SlpName,5))
            ELSE t3.WhsCode COLLATE Modern_Spanish_CI_AS
        END AS whscode,
        CAST(-t1.Quantity AS decimal(19,6)) AS Monto
    FROM MOVESA..ORIN t0 WITH (NOLOCK)
    JOIN MOVESA..RIN1 t1 WITH (NOLOCK) ON t0.DocEntry=t1.DocEntry
    JOIN MOVESA..OITM t2 WITH (NOLOCK) ON t1.ItemCode=t2.ItemCode
    JOIN MOVESA..OWHS t3 WITH (NOLOCK) ON t1.WhsCode=t3.WhsCode
    JOIN (SELECT SlpCode, SlpName FROM MOVESA..OSLP WITH (NOLOCK)) t4 ON t0.SlpCode=t4.SlpCode
    WHERE t0.DocDate >= @StartDate AND t0.DocDate <= @EndDate
      AND t2.ItemType='I' AND t2.ItmsGrpCod=154 AND t2.U_AMODELO=@modelo
      AND t0.slpcode not in (117,332)
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
        ,(select whsname from movesa..owhs with(nolock) where WhsCode = #MyTempTable.whscode collate Modern_Spanish_CI_AS )[Nombre Almacen]  

		,ISNULL((SELECT [MAXIMO] FROM [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] 
					where [WHSCODE] = #MyTempTable.whscode collate Modern_Spanish_CI_AS and [MODELO] collate Modern_Spanish_CI_AS = @nmodelo ),0) [CB]
		,isnull((select	CONVERT(INT,SUM(t11.Quantity))
									from [MOVESA].[dbo].[ORDR] t10 with(Nolock) INNER JOIN [MOVESA].[dbo].RDR1 T11 WITH(NOLOCK) ON T10.DocEntry=T11.DOCENTRY
										inner join [MOVESA].[dbo].OITM T12 WITH(NOLOCK) ON T11.ItemCode=T12.ItemCode INNER JOIN
										[MOVESA].[dbo].[@AMODELO] T13 WITH(nOLOCK) ON T12.U_AMODELO=T13.Code
								WHERE	T12.ItmsGrpCod=154
										AND T10.CANCELED='N'
										AND T11.LineStatus='O'
										AND T12.ItmsGrpCod=154
										AND T11.WhsCode = #MyTempTable.whscode collate Modern_Spanish_CI_AS 
										and t13.Code= @modelo 
								GROUP BY  t11.WhsCode,T13.Name),0) as [Comprometido]

		,isnull((select	CONVERT(INT,SUM(t11.Quantity))
									from [MOVESA].[dbo].[OWTQ] t10 with(Nolock) INNER JOIN [MOVESA].[dbo].WTQ1 T11 WITH(NOLOCK) ON T10.DocEntry=T11.DOCENTRY
										inner join [MOVESA].[dbo].OITM T12 WITH(NOLOCK) ON T11.ItemCode=T12.ItemCode INNER JOIN
										[MOVESA].[dbo].[@AMODELO] T13 WITH(nOLOCK) ON T12.U_AMODELO=T13.Code
								WHERE	T12.ItmsGrpCod=154
										AND T10.CANCELED='N'
										AND T11.LineStatus='O'
										AND T12.ItmsGrpCod=154
										AND T11.WhsCode ='T' + rtrim(#MyTempTable.whscode collate Modern_Spanish_CI_AS )
										and t13.Code = @modelo 
								GROUP BY  t11.WhsCode,T13.Name),0) as [Solicitado]

								,isnull((SELECT CONVERT(INT,SUM(t100.ONHAND))
								from MOVESA..OITW t100 with(nolock)
								full join
								MOVESA..OITM T101 with(nolock)
								on t100.ItemCode=t101.ItemCode
								full join
								MOVESA..[@AMODELO] t102 with(Nolock)
								on t101.U_AMODELO=t102.Code 
								WHERE T102.Code = @modelo AND T101.ItmsGrpCod=154 and t100.WhsCode='T' + rtrim(#MyTempTable.whscode collate Modern_Spanish_CI_AS )),0)
								as [Transito]
		, (SELECT convert(int,SUM(t100.ONHAND))
				from movesa..OITW t100 with(nolock)
				full join
				movesa..OITM T101 with(nolock)
				on t100.ItemCode=t101.ItemCode
				full join
				movesa..[@AMODELO] t102 with(Nolock)
				on t101.U_AMODELO=t102.Code 
				WHERE T102.Code = @modelo AND T100.WhsCode= #MyTempTable.whscode collate Modern_Spanish_CI_AS  AND T101.ItmsGrpCod=154) 'Fisico'    
FROM #MyTempTable ORDER BY [Row];
DROP TABLE #MyTempTable;

--select Posicion,Codigo,[Nombre Almacen],[Vta 3M],CB,Comprometido,Solicitado,Transito,Fisico,(CB-Comprometido+Solicitado+Transito-Fisico)[Faltante]

--from (
--SELECT [ROW_NUM] [Posicion], [WHSCODE] [Codigo]  
--,(select whsname from movesa..owhs with(nolock) where WhsCode=[RANKING].[WHSCODE] )[Nombre Almacen]  
--,convert(int,[CANTIDAD])[Vta 3M]   
--,ISNULL((SELECT [MAXIMO] FROM [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] where [WHSCODE]=[RANKING].[WHSCODE]  collate SQL_Latin1_General_CP1_CI_AS and [MODELO] =@p1 ),0) [CB]
--,isnull((select	CONVERT(INT,SUM(t11.Quantity))
--							from [MOVESA].[dbo].[ORDR] t10 with(Nolock) INNER JOIN [MOVESA].[dbo].RDR1 T11 WITH(NOLOCK) ON T10.DocEntry=T11.DOCENTRY
--								inner join [MOVESA].[dbo].OITM T12 WITH(NOLOCK) ON T11.ItemCode=T12.ItemCode INNER JOIN
--								[MOVESA].[dbo].[@AMODELO] T13 WITH(nOLOCK) ON T12.U_AMODELO=T13.Code
--						WHERE	T12.ItmsGrpCod=154
--								AND T10.CANCELED='N'
--								AND T11.LineStatus='O'
--								AND T12.ItmsGrpCod=154
--								AND T11.WhsCode =[RANKING].[WHSCODE]
--								and t13.name=@p1 
--						GROUP BY  t11.WhsCode,T13.Name),0) as [Comprometido]

--,isnull((select	CONVERT(INT,SUM(t11.Quantity))
--							from [MOVESA].[dbo].[OWTQ] t10 with(Nolock) INNER JOIN [MOVESA].[dbo].WTQ1 T11 WITH(NOLOCK) ON T10.DocEntry=T11.DOCENTRY
--								inner join [MOVESA].[dbo].OITM T12 WITH(NOLOCK) ON T11.ItemCode=T12.ItemCode INNER JOIN
--								[MOVESA].[dbo].[@AMODELO] T13 WITH(nOLOCK) ON T12.U_AMODELO=T13.Code
--						WHERE	T12.ItmsGrpCod=154
--								AND T10.CANCELED='N'
--								AND T11.LineStatus='O'
--								AND T12.ItmsGrpCod=154
--								AND T11.WhsCode ='T' + rtrim([RANKING].[WHSCODE])
--								and t13.name=@p1 
--						GROUP BY  t11.WhsCode,T13.Name),0) as [Solicitado]

--						,isnull((SELECT CONVERT(INT,SUM(t100.ONHAND))
--						from MOVESA..OITW t100 with(nolock)
--						full join
--						MOVESA..OITM T101 with(nolock)
--						on t100.ItemCode=t101.ItemCode
--						full join
--						MOVESA..[@AMODELO] t102 with(Nolock)
--						on t101.U_AMODELO=t102.Code 
--						WHERE T102.Name=@p1 AND T101.ItmsGrpCod=154 and t100.WhsCode='T' + rtrim([RANKING].[WHSCODE])),0)
--						as [Transito]
--, (SELECT convert(int,SUM(t100.ONHAND))
--		from movesa..OITW t100 with(nolock)
--		full join
--		movesa..OITM T101 with(nolock)
--		on t100.ItemCode=t101.ItemCode
--		full join
--		movesa..[@AMODELO] t102 with(Nolock)
--		on t101.U_AMODELO=t102.Code 
--		WHERE T102.Name=@p1 AND T100.WhsCode=[RANKING].[WHSCODE] AND T101.ItmsGrpCod=154) 'Fisico' 

--FROM [ArmadoMotos].[dbo].[RANKING] WHERE [MODELO] = @p1 ) as tempRanking

--order by Posicion




--SELECT [ROW_NUM] [Posicion], [WHSCODE] [Codigo]  
--,(select whsname from movesa..owhs with(nolock) where WhsCode=[RANKING].[WHSCODE] )[Nombre Almacen]  
--,convert(int,[CANTIDAD])[Vta 3M]   
--,CONVERT(INT,(select MAXIMO from movesaweb..LOGISTICA_MAX_INV where WHSCODE=[RANKING].[WHSCODE] collate SQL_Latin1_General_CP1_CI_AS and MODELO=@p1)) [CB]  
--, (SELECT convert(int,SUM(t100.ONHAND))
--from movesa..OITW t100 with(nolock)
--full join
--movesa..OITM T101 with(nolock)
--on t100.ItemCode=t101.ItemCode
--full join
--movesa..[@AMODELO] t102 with(Nolock)
--on t101.U_AMODELO=t102.Code 
--WHERE T102.Name=@p1 AND T100.WhsCode=[RANKING].[WHSCODE] AND T101.ItmsGrpCod=154) 'Fisico' 

--,(	CONVERT(INT,(select MAXIMO from movesaweb..LOGISTICA_MAX_INV where WHSCODE=[RANKING].[WHSCODE] collate SQL_Latin1_General_CP1_CI_AS and MODELO=@p1))   
---  
--(SELECT convert(int,SUM(t100.ONHAND))
--from movesa..OITW t100 with(nolock)
--full join
--movesa..OITM T101 with(nolock)
--on t100.ItemCode=t101.ItemCode
--full join
--movesa..[@AMODELO] t102 with(Nolock)
--on t101.U_AMODELO=t102.Code 
--WHERE T102.Name=@p1 AND T100.WhsCode=[RANKING].[WHSCODE] AND T101.ItmsGrpCod=154)) [Falt]  
--FROM [ArmadoMotos].[dbo].[RANKING] WHERE [MODELO] = @p1 ORDER BY [ROW_NUM] 
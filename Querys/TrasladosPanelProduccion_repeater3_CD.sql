DECLARE @COLS NVARCHAR(MAX) = '', @COLS_ISNULL NVARCHAR(MAX) = '', @SQL NVARCHAR(MAX) = ''
DECLARE @F1 VARCHAR(50) = FORMAT(DATEADD(WEEK, -6, GETDATE()), 'yyyyMMdd')
DECLARE @F2 VARCHAR(50) = FORMAT(GETDATE(), 'yyyyMMdd')

SELECT @F1 = FORMAT(MIN(FECHA),'yyyyMMdd'), @F2 = FORMAT(MAX(FECHA),'yyyyMMdd')
FROM MOVESAWEB..CALENDARIO WHERE  CONCAT(YEAR(FECHA),'-',DATEPART(WEEK, FECHA)) IN (
	SELECT DISTINCT CONCAT(YEAR(FECHA),'-',DATEPART(WEEK, FECHA)) AS SEMANA 
	FROM MOVESAWEB..Calendario WHERE FECHA BETWEEN @F1 AND @F2
)

-- Construir columnas pivot
SELECT @COLS = STUFF((
	SELECT DISTINCT ', ' + QUOTENAME(DATEPART(WK, FECHA))
	FROM MOVESAWEB..CALENDARIO 
	WHERE FECHA BETWEEN @F1 AND @F2
	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')

-- Construir columnas con ISNULL
SELECT @COLS_ISNULL = STUFF((
	SELECT DISTINCT ', ISNULL(' + QUOTENAME(DATEPART(WK, FECHA)) + ', 0) AS ' + QUOTENAME(DATEPART(WK, FECHA))
	FROM MOVESAWEB..CALENDARIO 
	WHERE FECHA BETWEEN @F1 AND @F2
	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')

-- Armar SQL dinámico
SET @SQL = '
SELECT ZONA, CANAL, ' + @COLS_ISNULL + '
FROM (
    SELECT 
        T1.Zona AS ZONA, T0.CANAL, 
        DATEPART(WK, DOCDATE) AS WK, 
        SUM(QUANTITY) AS Quantity 
    FROM MOVESA..PT_VW_GENERAL_VENTAS_MOTOS AS T0
    INNER JOIN MOVESA..PT_VW_USUARIOS AS T1 
        ON T1.[USER] = T0.VENDEDOR COLLATE SQL_Latin1_General_CP850_CI_AS
    WHERE DOCDATE BETWEEN ''' + @F1 + ''' AND ''' + @F2 + ''' 
        AND CANAL = ''CD''
    GROUP BY T1.ZONA, T0.CANAL, DATEPART(WK, DOCDATE)

    UNION ALL

    SELECT 
        T1.U_ZONA AS ZONA, T0.CANAL, 
        DATEPART(WK, DOCDATE) AS WK, 
        SUM(QUANTITY) AS Quantity 
    FROM MOVESA..PT_VW_GENERAL_VENTAS_MOTOS AS T0
    INNER JOIN MOVESA..PT_VW_USUARIOS AS T1 
        ON T1.[USER] = T0.VENDEDOR COLLATE SQL_Latin1_General_CP850_CI_AS
    WHERE DOCDATE BETWEEN ''' + @F1 + ''' AND ''' + @F2 + ''' 
        AND CANAL = ''CI''
    GROUP BY T1.U_ZONA, T0.CANAL, DATEPART(WK, DOCDATE)
) AS SRC
PIVOT (
    SUM(Quantity) FOR WK IN (' + @COLS + ')
) AS PVT
ORDER BY CANAL, ZONA;
'

-- Ejecutar
EXEC sp_executesql @SQL;

--DECLARE @COLS NVARCHAR(MAX) = '', @COLS_ISNULL NVARCHAR(MAX) = '', @SQL NVARCHAR(MAX) = ''
--DECLARE @F1 VARCHAR(50) = FORMAT(DATEADD(WEEK, -6, GETDATE()), 'yyyyMMdd')
--DECLARE @F2 VARCHAR(50) = FORMAT(GETDATE(), 'yyyyMMdd')

---- Construir columnas pivot
--SELECT @COLS = STUFF((
--	SELECT DISTINCT ', ' + QUOTENAME(DATEPART(WK, FECHA))
--	FROM MOVESAWEB..CALENDARIO 
--	WHERE FECHA BETWEEN @F1 AND @F2
--	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')

---- Construir columnas con ISNULL
--SELECT @COLS_ISNULL = STUFF((
--	SELECT DISTINCT ', ISNULL(' + QUOTENAME(DATEPART(WK, FECHA)) + ', 0) AS ' + QUOTENAME(DATEPART(WK, FECHA))
--	FROM MOVESAWEB..CALENDARIO 
--	WHERE FECHA BETWEEN @F1 AND @F2
--	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')

---- Armar SQL dinámico
--SET @SQL = '
--SELECT ZONA, CANAL, ' + @COLS_ISNULL + '
--FROM (
--    SELECT 
--        T1.Zona AS ZONA, T0.CANAL, 
--        DATEPART(WK, DOCDATE) AS WK, 
--        SUM(QUANTITY) AS Quantity 
--    FROM MOVESA..PT_VW_GENERAL_VENTAS_MOTOS AS T0
--    INNER JOIN MOVESA..PT_VW_USUARIOS AS T1 
--        ON T1.[USER] = T0.VENDEDOR COLLATE SQL_Latin1_General_CP850_CI_AS
--    WHERE DOCDATE BETWEEN ''' + @F1 + ''' AND ''' + @F2 + ''' 
--        AND CANAL = ''CD''
--    GROUP BY T1.ZONA, T0.CANAL, DATEPART(WK, DOCDATE)

--    UNION ALL

--    SELECT 
--        T1.U_ZONA AS ZONA, T0.CANAL, 
--        DATEPART(WK, DOCDATE) AS WK, 
--        SUM(QUANTITY) AS Quantity 
--    FROM MOVESA..PT_VW_GENERAL_VENTAS_MOTOS AS T0
--    INNER JOIN MOVESA..PT_VW_USUARIOS AS T1 
--        ON T1.[USER] = T0.VENDEDOR COLLATE SQL_Latin1_General_CP850_CI_AS
--    WHERE DOCDATE BETWEEN ''' + @F1 + ''' AND ''' + @F2 + ''' 
--        AND CANAL = ''CI''
--    GROUP BY T1.U_ZONA, T0.CANAL, DATEPART(WK, DOCDATE)
--) AS SRC
--PIVOT (
--    SUM(Quantity) FOR WK IN (' + @COLS + ')
--) AS PVT
--ORDER BY CANAL, ZONA;
--'

---- Ejecutar
--EXEC sp_executesql @SQL;



----DECLARE @COLS NVARCHAR(MAX) = '', @SQL NVARCHAR(MAX) = ''
----DECLARE @F1 VARCHAR(50) = FORMAT(DATEADD(WEEK, -6, GETDATE()),'yyyyMMdd'),  @F2 VARCHAR(50) = FORMAT(GETDATE(),'yyyyMMdd')

----SET @COLS = STUFF((
----	SELECT DISTINCT ', ' + QUOTENAME(DATEPART(WK, FECHA))
----	FROM MOVESAWEB..CALENDARIO WHERE FECHA BETWEEN @F1 AND @F2
----FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '');

----SET @sql = '
----    SELECT * FROM (
----		SELECT 
----			T1.Zona, T0.CANAL, 
----			DATEPART(WK,DOCDATE) AS WK, SUM(QUANTITY) AS Quantity 
----		FROM MOVESA..PT_VW_GENERAL_VENTAS_MOTOS AS T0
----		INNER JOIN MOVESA..PT_VW_USUARIOS AS T1 ON T1.[USER] = T0.VENDEDOR COLLATE SQL_Latin1_General_CP850_CI_AS
----		WHERE DOCDATE BETWEEN '''+@F1+''' AND  '''+@F2+''' AND CANAL = ''CD''
----		GROUP BY T1.ZONA, T0.CANAL, DATEPART(WK,DOCDATE)
----		UNION ALL
----		SELECT 
----			T1.U_ZONA, T0.CANAL, 
----			DATEPART(WK,DOCDATE) AS WK, SUM(QUANTITY) AS Quantity 
----		FROM MOVESA..PT_VW_GENERAL_VENTAS_MOTOS AS T0
----		INNER JOIN MOVESA..PT_VW_USUARIOS AS T1 ON T1.[USER] = T0.VENDEDOR COLLATE SQL_Latin1_General_CP850_CI_AS
----		WHERE DOCDATE BETWEEN '''+@F1+''' AND  '''+@F2+''' AND CANAL = ''CI''
----		GROUP BY T1.U_ZONA, T0.CANAL, DATEPART(WK,DOCDATE)
----	) AS T0
----	PIVOT (SUM(Quantity) FOR WK IN ('+@COLS+')) AS PT
----	ORDER BY CANAL, ZONA
----';

----EXEC sp_executesql @sql;




----DECLARE @year INT = DATEPART(YEAR, GETDATE());
----DECLARE @week INT = DATEPART(ISOWK, GETDATE());

------ Obtener columnas dinámicas (una por semana)
----DECLARE @cols NVARCHAR(MAX);

----SELECT @cols = STUFF((
----    SELECT DISTINCT ',[' + CAST(DATEPART(ISOWK, t0.DocDate) AS VARCHAR) + ']'
----    FROM oinv t0
----    INNER JOIN inv1 t1 ON t0.DocEntry = t1.DocEntry
----    INNER JOIN owhs alm ON t1.WhsCode = alm.WhsCode
----    INNER JOIN oitm t2 ON t2.ItemCode = t1.ItemCode
----    WHERE 
----        DATEPART(YEAR, t0.DocDate) = @year AND
----        DATEPART(WEEK, t0.DocDate) >= @week - 6 AND
----        alm.WhsCode NOT IN ('EST001') AND
----        CASE alm.U_Type WHEN 'PRO' THEN 'CD' ELSE 'CI' END = 'CD' AND
----        t2.ItmsGrpCod = 154 AND 
----        t0.CANCELED = 'N' AND 
----        t1.TargetType <> 14
----    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '');

------ Crear SQL dinámico con PIVOT
----DECLARE @sql NVARCHAR(MAX) = '
----SELECT Zona, ' + @cols + '
----FROM (
----    SELECT 
----        alm.U_ZONA AS Zona,
----        DATEPART(ISOWK, t0.DocDate) AS WK,
----        t1.Quantity
----    FROM oinv t0 
----    INNER JOIN inv1 t1 ON t0.DocEntry = t1.DocEntry
----    INNER JOIN owhs alm ON t1.WhsCode = alm.WhsCode
----    INNER JOIN oitm t2 ON t2.ItemCode = t1.ItemCode
----    WHERE 
----        DATEPART(YEAR, t0.DocDate) = ' + CAST(@year AS VARCHAR) + ' AND
----        DATEPART(WEEK, t0.DocDate) >= ' + CAST(@week - 6 AS VARCHAR) + ' AND
----        alm.WhsCode NOT IN (''EST001'') AND
----        CASE alm.U_Type WHEN ''PRO'' THEN ''CD'' ELSE ''CI'' END = ''CD'' AND
----        t2.ItmsGrpCod = 154 AND 
----        t0.CANCELED = ''N'' AND 
----        t1.TargetType <> 14
----) AS Datos
----PIVOT (
----    SUM(Quantity)
----    FOR WK IN (' + @cols + ')
----) AS pvt
----ORDER BY Zona;
----';

------ Ejecutar el query dinámico
----EXEC sp_executesql @sql;

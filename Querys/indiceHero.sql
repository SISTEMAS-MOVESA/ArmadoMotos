SET NOCOUNT ON;


 
-- Paso 1: Tabla temporal de almacenes a excluir (sin duplicados)
IF OBJECT_ID('tempdb..#WhsToExclude') IS NOT NULL DROP TABLE #WhsToExclude;
CREATE TABLE #WhsToExclude (WhsCode VARCHAR(50) PRIMARY KEY);

INSERT INTO #WhsToExclude (WhsCode) VALUES
('BPD01'), ('CDM00'), ('CDR00'), ('CDR01'), ('CONCA01'), ('Consumir'),
('DCM00'), ('DCR00'), ('DCR01'), ('DEV01'), ('DEV02'), ('EST001'), ('FCE01'), ('MAY01'), 
('MLCB01'), ('MLCB02'), ('MOB01'), ('MSPS01'), ('MSPS02'), ('MSPS03'), ('MSPS04'),
('MSUC01'), ('MSUC02'), ('MSUC03'), ('MSUC04'), ('MSUC05'), ('MSUC07'), ('MTGA01'),
('MTRANS00'), ('RLCB02'), ('RLCB03'), ('RSPS02'), ('RSPS03'), ('RSPS04'), ('RSPS05'),
('RSPS06'), ('RSPS07'), ('RTGA01'), ('RTGA0102'), ('SUC0102'), ('SUC0201'), ('SUC03'),
('SUC0301'), ('SUC0401'), ('SUC0701'), ('SUC0801'), ('SUM001'), ('DIU-10'), ('GMG122'),
('mym 09'), ('con'), ('TSUC03'), ('CON 334'), ('CON01');

-- Paso 2: Fechas cabales
DECLARE @FechaBase DATE = EOMONTH(DATEADD(MONTH, -1, GETDATE()));
DECLARE @F1 DATE = DATEADD(MONTH, -(3-1), DATEFROMPARTS(YEAR(@FechaBase), MONTH(@FechaBase), 1));
DECLARE @F2 DATE = GETDATE();

-- Paso 3: Almacenes filtrados
IF OBJECT_ID('tempdb..#AlmacenesFiltrados') IS NOT NULL DROP TABLE #AlmacenesFiltrados;
SELECT WhsCode, U_Type
INTO #AlmacenesFiltrados
FROM MOVESA..OWHS WITH (NOLOCK)
WHERE WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS NOT IN (SELECT WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS FROM #WhsToExclude)
  AND LEFT(WhsCode, 1) NOT IN ('T', 'H');

-- Limpia la tabla destino antes de los CTEs
DELETE FROM TempParetoAllWhscodes_Hero;

-- CTEs de cálculo principales
WITH VentasDevs AS (
    SELECT 
        T5.Code,
        T5.Name AS Modelo,
        T1.WhsCode,
        SUM(CASE WHEN T0.ObjType = 13 THEN T1.Quantity ELSE -T1.Quantity END) AS Cantidad,
        SUM(CASE WHEN T0.ObjType = 13 THEN T1.linetotal ELSE -T1.linetotal END) AS Monto
    FROM (
        SELECT 13 AS ObjType, DocEntry, DocDate, CANCELED FROM MOVESA..OINV WITH (NOLOCK)
        UNION ALL
        SELECT 14 AS ObjType, DocEntry, DocDate, CANCELED FROM MOVESA..ORIN WITH (NOLOCK)
    ) T0
    INNER JOIN (
        SELECT DocEntry, ItemCode, WhsCode, Quantity, linetotal FROM MOVESA..INV1 WITH (NOLOCK)
        UNION ALL
        SELECT DocEntry, ItemCode, WhsCode, Quantity, linetotal FROM MOVESA..RIN1 WITH (NOLOCK)
    ) T1 ON T0.DocEntry = T1.DocEntry
    INNER JOIN MOVESA..OITM T2 WITH (NOLOCK) ON T2.ItemCode = T1.ItemCode
    LEFT JOIN MOVESA..[@AMODELO] T5 WITH (NOLOCK) 
        ON T2.U_AMODELO COLLATE SQL_Latin1_General_CP1_CI_AS = T5.Code COLLATE SQL_Latin1_General_CP1_CI_AS
    WHERE T0.DocDate >= @F1 AND T0.CANCELED = 'N' AND T2.ItmsGrpCod = 154 AND T2.U_AMARCA='21'
          AND T1.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS IN (SELECT WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS FROM #AlmacenesFiltrados)
    GROUP BY T5.Code, T5.Name, T1.WhsCode
),
Totales AS (
    SELECT WhsCode, SUM(Cantidad) AS TotalCantidad
    FROM VentasDevs
    GROUP BY WhsCode
),
ParetoRanking AS (
    SELECT 
        AF.WhsCode,
        AF.U_Type,
        V.Code,
        V.Modelo,
        V.Cantidad,
        V.Monto,
        ROW_NUMBER() OVER(PARTITION BY V.WhsCode ORDER BY V.Cantidad DESC, V.Monto DESC) AS [Row],
        SUM(V.Cantidad) OVER(PARTITION BY V.WhsCode ORDER BY V.Cantidad DESC, V.Monto DESC 
                ROWS UNBOUNDED PRECEDING) * 1.0 / NULLIF(T.TotalCantidad,0) AS ParetoPercent
    FROM VentasDevs V
    INNER JOIN Totales T ON V.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS = T.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS
    INNER JOIN #AlmacenesFiltrados AF ON V.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS = AF.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS
),
CB AS (
    SELECT [WHSCODE], [MODELO],
    MAX([MAXIMO]) AS [CB]
    FROM MOVESAWeb.dbo.LOGISTICA_MAX_INV
    GROUP BY [WHSCODE], [MODELO]
),
DespachosAbiertos AS (
    SELECT
        rtrim(ALMDESTINO) AS ALMDESTINO,
        MODELO,
        SUM(CANTIDAD) AS Despacho
    FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS]
    WHERE DELETED <> 'Y' 
      AND CODIGOESTADO IN ('DESPACHO ABIERTO','CAMION ABIERTO')
    GROUP BY rtrim(ALMDESTINO), MODELO
),
Comprometido AS (
    SELECT
        T11.WhsCode,
        T13.Code AS Modelo,
        SUM(T11.Quantity) AS Comprometido
    FROM MOVESA.dbo.ORDR T10 WITH(NOLOCK)
    INNER JOIN MOVESA.dbo.RDR1 T11 WITH(NOLOCK) ON T10.DocEntry = T11.DOCENTRY
    INNER JOIN MOVESA.dbo.OITM T12 WITH(NOLOCK) ON T11.ItemCode = T12.ItemCode
    INNER JOIN MOVESA.dbo.[@AMODELO] T13 WITH(NOLOCK) ON T12.U_AMODELO = T13.Code
    WHERE
        T10.DocDate >= DATEADD(day,-3,GETDATE())
        AND T12.ItmsGrpCod = 154
        AND T10.CANCELED = 'N'
        AND T11.LineStatus = 'O'
    GROUP BY T11.WhsCode, T13.Code
),
Solicitado AS (
    SELECT
        T11.WhsCode,
        T13.Code AS Modelo,
        SUM(T11.Quantity) AS Solicitado
    FROM MOVESA.dbo.OWTQ T10 WITH(NOLOCK)
    INNER JOIN MOVESA.dbo.WTQ1 T11 WITH(NOLOCK) ON T10.DocEntry = T11.DOCENTRY
    INNER JOIN MOVESA.dbo.OITM T12 WITH(NOLOCK) ON T11.ItemCode = T12.ItemCode
    INNER JOIN MOVESA.dbo.[@AMODELO] T13 WITH(NOLOCK) ON T12.U_AMODELO = T13.Code
    WHERE
        T12.ItmsGrpCod = 154
        AND T10.CANCELED = 'N'
        AND T11.LineStatus = 'O'
    GROUP BY T11.WhsCode, T13.Code
),
Transito AS (
    SELECT
        t100.WhsCode,
        t102.Code AS Modelo,
        SUM(t100.ONHAND) AS Transito
    FROM MOVESA.dbo.OITW t100 WITH(NOLOCK)
    FULL JOIN MOVESA.dbo.OITM t101 WITH(NOLOCK) ON t100.ItemCode = t101.ItemCode
    FULL JOIN MOVESA.dbo.[@AMODELO] t102 WITH(NOLOCK) ON t101.U_AMODELO = t102.Code
    WHERE t101.ItmsGrpCod = 154
    GROUP BY t100.WhsCode, t102.Code
),
Fisico AS (
    SELECT
        t100.WhsCode,
        t102.Code AS Modelo,
        SUM(t100.ONHAND) AS Fisico
    FROM MOVESA.dbo.OITW t100 WITH(NOLOCK)
    FULL JOIN MOVESA.dbo.OITM t101 WITH(NOLOCK) ON t100.ItemCode = t101.ItemCode
    FULL JOIN MOVESA.dbo.[@AMODELO] t102 WITH(NOLOCK) ON t101.U_AMODELO = t102.Code
    WHERE t101.ItmsGrpCod = 154
    GROUP BY t100.WhsCode, t102.Code
),
UltMov AS (
    SELECT
        warehouse,
        MAX(DOCDATE) AS UltMov
    FROM movesa..oinm WITH(NOLOCK)
    WHERE transtype = 67 AND REF2 = 'DCM00'
    GROUP BY warehouse
)

INSERT INTO TempParetoAllWhscodes_Hero (
    [ROW], [WHSCODE], [CODE], [MODELO], [CANTIDAD], [MONTO], [PARETOPERCENT],
    CB, DE, CO, SO, TR, FI, FA, ULTRANSFER, [FIAJ], [FAIN]
)
SELECT
    P.[Row], 
    P.WhsCode, 
    P.Code, 
    P.Modelo, 
    P.Cantidad, 
    P.Monto, 
    P.ParetoPercent,
    ISNULL(CB.CB,0) AS CB,
    ISNULL(DA.Despacho,0) AS DE,
    ISNULL(CO.Comprometido,0) AS CO,
    ISNULL(SO.Solicitado,0) AS SO,
    CASE WHEN P.U_Type = 'PRO'
         THEN ISNULL(TT.Transito,0)
         ELSE 0
    END AS TR,
    ISNULL(FI.Fisico,0) AS FI

	,ISNULL(CB.CB,0) + ISNULL(CO.Comprometido,0) - ISNULL(SO.Solicitado,0) 
			- CASE WHEN P.U_Type = 'PRO' THEN ISNULL(TT.Transito,0) ELSE 0 END - ISNULL(FI.Fisico,0) AS FA

    ,UM.UltMov AS ULTRANSFER
	, CASE WHEN ISNULL(FI.Fisico,0) > ISNULL(CB.CB,0) THEN ISNULL(CB.CB,0) ELSE ISNULL(FI.Fisico,0) END [FIAJ]

	,CASE WHEN 	ISNULL(CB.CB,0) + ISNULL(CO.Comprometido,0) - ISNULL(SO.Solicitado,0) 
			- CASE WHEN P.U_Type = 'PRO' THEN ISNULL(TT.Transito,0) ELSE 0 END -
				(CASE WHEN ISNULL(FI.Fisico,0) > ISNULL(CB.CB,0) THEN ISNULL(CB.CB,0) ELSE ISNULL(FI.Fisico,0) END ) <0 THEN
				0
				ELSE
					ISNULL(CB.CB,0) + ISNULL(CO.Comprometido,0) - ISNULL(SO.Solicitado,0) 
					- CASE WHEN P.U_Type = 'PRO' THEN ISNULL(TT.Transito,0) ELSE 0 END -
					(CASE WHEN ISNULL(FI.Fisico,0) > ISNULL(CB.CB,0) THEN ISNULL(CB.CB,0) ELSE ISNULL(FI.Fisico,0) END ) 


				END [FAIN]

FROM ParetoRanking P
LEFT JOIN CB 
  ON CB.WHSCODE COLLATE SQL_Latin1_General_CP1_CI_AS = P.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS
 AND CB.MODELO COLLATE SQL_Latin1_General_CP1_CI_AS = P.Modelo COLLATE SQL_Latin1_General_CP1_CI_AS
LEFT JOIN DespachosAbiertos DA
  ON RTRIM(P.WhsCode) COLLATE SQL_Latin1_General_CP1_CI_AS = DA.ALMDESTINO COLLATE SQL_Latin1_General_CP1_CI_AS
 AND DA.MODELO COLLATE SQL_Latin1_General_CP1_CI_AS = P.Modelo COLLATE SQL_Latin1_General_CP1_CI_AS
LEFT JOIN Comprometido CO 
  ON CO.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS = P.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS
 AND CO.Modelo COLLATE SQL_Latin1_General_CP1_CI_AS = P.Code COLLATE SQL_Latin1_General_CP1_CI_AS
LEFT JOIN Solicitado SO
  ON SO.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS = 
      (CASE WHEN P.U_Type = 'PRO' THEN 'T' + P.WhsCode ELSE P.WhsCode END) COLLATE SQL_Latin1_General_CP1_CI_AS
 AND SO.Modelo COLLATE SQL_Latin1_General_CP1_CI_AS = P.Code COLLATE SQL_Latin1_General_CP1_CI_AS
LEFT JOIN Transito TT 
  ON TT.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS = 
      (CASE WHEN P.U_Type = 'PRO' THEN 'T' + P.WhsCode ELSE P.WhsCode END) COLLATE SQL_Latin1_General_CP1_CI_AS
 AND TT.Modelo COLLATE SQL_Latin1_General_CP1_CI_AS = P.Code COLLATE SQL_Latin1_General_CP1_CI_AS
LEFT JOIN Fisico FI 
  ON FI.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS = P.WhsCode COLLATE SQL_Latin1_General_CP1_CI_AS
 AND FI.Modelo COLLATE SQL_Latin1_General_CP1_CI_AS = P.Code COLLATE SQL_Latin1_General_CP1_CI_AS
LEFT JOIN UltMov UM 
  ON UM.warehouse COLLATE SQL_Latin1_General_CP1_CI_AS = 
      (CASE WHEN P.U_Type = 'PRO' THEN 'T' + P.WhsCode ELSE P.WhsCode END) COLLATE SQL_Latin1_General_CP1_CI_AS


-- Consulta final
--SELECT * FROM TempParetoAllWhscodes_Hero ORDER BY WHSCODE,ROW;


-- Limpieza final
IF OBJECT_ID('tempdb..#WhsToExclude') IS NOT NULL DROP TABLE #WhsToExclude;
IF OBJECT_ID('tempdb..#AlmacenesFiltrados') IS NOT NULL DROP TABLE #AlmacenesFiltrados;



;WITH CanalAlmacen AS (
    SELECT
        (SELECT name FROM movesa..[@CRUTA]  
         WHERE code = (SELECT U_Ruta FROM movesa..OWHS WHERE WhsCode = t.WHSCODE COLLATE Modern_Spanish_CI_AS)
        ) AS [Ruta],

        (SELECT CASE u_type WHEN 'PRO' THEN 'CD' ELSE 'CI' END FROM movesa..OWHS 
                WHERE WhsCode = t.WHSCODE COLLATE Modern_Spanish_CI_AS) AS [Canal],

        t.WHSCODE,
        (SELECT WhsName FROM movesa..OWHS 
            WHERE WhsCode = t.WHSCODE COLLATE Modern_Spanish_CI_AS) AS [Nombre_Almacen],

        SUM(CANTIDAD) AS SUM_CANTIDAD,
         ISNULL((SELECT CONVERT(decimal(19,4),SUM(CB))
            FROM TempParetoAllWhscodes_Hero sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_CB,
        SUM(DE) AS SUM_DE,
        SUM(CO) AS SUM_CO,
        SUM(SO) AS SUM_SO,
        SUM(TR) AS SUM_TR,
        ISNULL((SELECT CONVERT(decimal(19,4),SUM(FI))
            FROM TempParetoAllWhscodes_Hero sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_FI,
        ISNULL((SELECT CONVERT(decimal(19,4),SUM(FA))
            FROM TempParetoAllWhscodes_Hero sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_FA,

        ISNULL((
            SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
            FROM TempParetoAllWhscodes_Hero sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS IND_GRAL,

        ISNULL((
            SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
            FROM TempParetoAllWhscodes_Hero sub
            WHERE sub.PARETOPERCENT <= 0.40 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS IND_1_40,

        ISNULL((
            SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
            FROM TempParetoAllWhscodes_Hero sub
            WHERE sub.PARETOPERCENT BETWEEN 0.41 AND 0.70 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS IND_41_70,
        ISNULL((SELECT CONVERT(decimal(19,4),SUM(FIAJ))
            FROM TempParetoAllWhscodes_Hero sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_FIAJ,

		  ISNULL((SELECT CONVERT(decimal(19,4),SUM(FAIN))
            FROM TempParetoAllWhscodes_Hero sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_FAIN,

        MAX(ULTRANSFER) AS ULTRANSFER,
         ISNULL((
            SELECT CONVERT(decimal(19,4),(SUM(FAIN)-SUM(DE))) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
            FROM TempParetoAllWhscodes_Hero sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS  [Indice_Proyectado]

    FROM TempParetoAllWhscodes_Hero t
    GROUP BY t.WHSCODE
)
, CanalRanking AS (
    SELECT *,
        ROW_NUMBER() OVER (PARTITION BY Canal ORDER BY SUM_CANTIDAD DESC) AS RANK_CANAL
    FROM CanalAlmacen
)
SELECT *,
       CASE 
            WHEN IND_GRAL >= 0.70 THEN 1
            WHEN IND_GRAL >= 0.50 THEN 2
            WHEN IND_GRAL >= 0.40 THEN 3
            ELSE 4
       END AS Categoria_Indice,
       CASE 
            WHEN RANK_CANAL BETWEEN 1 AND 15 THEN '1 al 15'
            WHEN RANK_CANAL BETWEEN 16 AND 30 THEN '16 al 30'
            WHEN RANK_CANAL BETWEEN 31 AND 45 THEN '31 al 45'
            WHEN RANK_CANAL BETWEEN 46 AND 60 THEN '46 al 60'
            WHEN RANK_CANAL BETWEEN 61 AND 75 THEN '61 al 75'
            ELSE '+76'	
       END AS [Leyenda],
       CASE 
            WHEN RANK_CANAL BETWEEN 1 AND 15 THEN 1
            WHEN RANK_CANAL BETWEEN 16 AND 30 THEN 2
            WHEN RANK_CANAL BETWEEN 31 AND 45 THEN 3
            WHEN RANK_CANAL BETWEEN 46 AND 60 THEN 4
            WHEN RANK_CANAL BETWEEN 61 AND 75 THEN 5
            ELSE 6
       END AS Categoria_Ranking
FROM CanalRanking
ORDER BY Ruta, Canal, RANK_CANAL;
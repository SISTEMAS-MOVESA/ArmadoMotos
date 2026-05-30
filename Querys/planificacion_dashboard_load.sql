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
            FROM TempParetoAllWhscodes sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_CB,
        SUM(DE) AS SUM_DE,
        SUM(CO) AS SUM_CO,
        SUM(SO) AS SUM_SO,
        SUM(TR) AS SUM_TR,
        ISNULL((SELECT CONVERT(decimal(19,4),SUM(FI))
            FROM TempParetoAllWhscodes sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_FI,
        ISNULL((SELECT CONVERT(decimal(19,4),SUM(FA))ok
            FROM TempParetoAllWhscodes sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_FA,

        -- IND_GRAL con límites 0-1
        CASE 
            WHEN ISNULL((
                SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
            ),0.0000) < 0 THEN 0
            WHEN ISNULL((
                SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
            ),0.0000) > 1 THEN 1
            ELSE ISNULL((
                SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
            ),0.0000)
        END AS IND_GRAL,

        -- IND_1_40 con límites 0-1
        CASE 
            WHEN ISNULL((
                SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT <= 0.40 AND sub.WHSCODE = t.WHSCODE
            ),0.0000) < 0 THEN 0
            WHEN ISNULL((
                SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT <= 0.40 AND sub.WHSCODE = t.WHSCODE
            ),0.0000) > 1 THEN 1
            ELSE ISNULL((
                SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT <= 0.40 AND sub.WHSCODE = t.WHSCODE
            ),0.0000)
        END AS IND_1_40,

        -- IND_41_70 con límites 0-1
        CASE 
            WHEN ISNULL((
                SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT BETWEEN 0.41 AND 0.70 AND sub.WHSCODE = t.WHSCODE
            ),0.0000) < 0 THEN 0
            WHEN ISNULL((
                SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT BETWEEN 0.41 AND 0.70 AND sub.WHSCODE = t.WHSCODE
            ),0.0000) > 1 THEN 1
            ELSE ISNULL((
                SELECT CONVERT(decimal(19,4),SUM(FAIN)) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT BETWEEN 0.41 AND 0.70 AND sub.WHSCODE = t.WHSCODE
            ),0.0000)
        END AS IND_41_70,

        ISNULL((SELECT CONVERT(decimal(19,4),SUM(FIAJ))
            FROM TempParetoAllWhscodes sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_FIAJ,

        ISNULL((SELECT CONVERT(decimal(19,4),SUM(FAIN))
            FROM TempParetoAllWhscodes sub
            WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
        ),0.0000) AS SUM_FAIN,

        MAX(ULTRANSFER) AS ULTRANSFER,

        -- Indice_Proyectado con límites 0-1
        CASE 
            WHEN ISNULL((
                SELECT CONVERT(decimal(19,4),(SUM(FAIN)-SUM(DE))) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
            ),0.0000) < 0 THEN 0
            WHEN ISNULL((
                SELECT CONVERT(decimal(19,4),(SUM(FAIN)-SUM(DE))) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
            ),0.0000) > 1 THEN 1
            ELSE ISNULL((
                SELECT CONVERT(decimal(19,4),(SUM(FAIN)-SUM(DE))) / NULLIF(CONVERT(decimal(19,4),SUM(CB)), 0.0000)
                FROM TempParetoAllWhscodes sub
                WHERE sub.PARETOPERCENT <= 0.80 AND sub.WHSCODE = t.WHSCODE
            ),0.0000)
        END AS [Indice_Proyectado]

    FROM TempParetoAllWhscodes t
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
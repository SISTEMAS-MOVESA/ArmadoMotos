CREATE TABLE #TempSeries
(
    ITEMCODE NVARCHAR(255) COLLATE SQL_Latin1_General_CP850_CI_AS,
    SERIE NVARCHAR(255) COLLATE SQL_Latin1_General_CP850_CI_AS
);
CREATE TABLE #TempSeries2
(
    ITEMCODE NVARCHAR(255) COLLATE Modern_Spanish_CI_AS,
    SERIE NVARCHAR(255) COLLATE Modern_Spanish_CI_AS,
    almacen NVARCHAR(255) COLLATE Modern_Spanish_CI_AS,
    estatus NVARCHAR(255) COLLATE Modern_Spanish_CI_AS
);
INSERT INTO #TempSeries (ITEMCODE,SERIE)
SELECT T0.ITEMCODE, t0.[SERIE]
FROM [dbo].[ARMADOMOTOS] t0 WITH (NOLOCK) 
WHERE ESTATUS <> 'ERROR' AND CANCELED = 'N' AND LIQUIDACIONID = 0;
INSERT INTO #TempSeries2
SELECT ITEMCODE, SERIE, (SELECT TOP 1 whscode 
                   FROM movesa..osri WITH (NOLOCK) 
                   WHERE ItemCode = #TempSeries.ITEMCODE AND SuppSerial = #TempSeries.SERIE)[WHSCODE],
				   (SELECT TOP 1 [Status] 
                   FROM movesa..osri WITH (NOLOCK)
				   WHERE ItemCode = #TempSeries.ITEMCODE AND SuppSerial = #TempSeries.SERIE)[ESTATUS]
FROM #TempSeries;
SELECT t0.[ID],t0.[SERIE],t0.[MODELO],t0.[COLOR],t0.[DATECREATED],t0.[ESTATUS]  
, (select [MECANICONAME] from [dbo].[MECANICOS] with(nolock) where id=t0.MECANICOASIGNADO) [MECANICOASIGNADO],    
t0.[FIRSTUPDATEDATE], t0.[FECHACC1], t0.SECONDUPDATEDATE, t0.DATEDETALLE     
, (select USERNAME from USUARIOS with(nolock) where USERCODE=t0.CCALIDAD1USER) [Usuario]     
, (select #TempSeries2.almacen from #TempSeries2 with(nolock) where #TempSeries2.ITEMCODE=t0.itemcode and #TempSeries2.SERIE=t0.[SERIE])[Almacen]    
, (select #TempSeries2.estatus from #TempSeries2 with(nolock) where #TempSeries2.ITEMCODE=t0.itemcode and #TempSeries2.SERIE=t0.[SERIE])[Estado]    
,DATEDIFF(DAY,SECONDUPDATEDATE,GETDATE())[DIAS] 
From [dbo].[ARMADOMOTOS] t0 With(nolock) where ESTATUS in ('Pintura', 'Pintura en Proceso')  and CANCELED='N' and LIQUIDACIONID=0 
DROP TABLE #TempSeries
DROP TABLE #TempSeries2
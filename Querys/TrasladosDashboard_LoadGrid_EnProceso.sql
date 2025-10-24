DELETE FROM PRESOLICITUD_HEADER WHERE DATEDIFF(DAY,DATECREATED,GETDATE())>10;


SELECT	t0.ID
		,t0.ORIGEN
		,t0.DESTINO
		,t0.RUTA
		,t0.ESTADO
		,t0.DATECREATED
		,t0.USERCREATED
		,t0.DOCENTRYSAP
		,t0.SUPERVISOR
		,[DATECREATED][FECHA] 
		,(select [Name] from movesa..[@CRESPONSABLEOWHS] where code=(select U_Categorizacion from movesa..owhs where WhsCode=[DESTINO] collate Modern_Spanish_CI_AS))[SUPERVISORASIGNADO]  
		,(select whsname from movesa..owhs where WhsCode=[DESTINO] collate Modern_Spanish_CI_AS)[NALMDESTINO] 
		,CASE ESTADO WHEN '1' THEN 'SUGERIDO' WHEN '2' THEN 'SUCURSAL' WHEN '3' THEN 'LOGISTICA' WHEN '4' THEN 'CARGA'  
		WHEN '5' THEN 'SOLICITUD' WHEN '6' THEN 'TRANSFER' WHEN '7' THEN 'Despachado' end [ETAPA] 
		,convert(char,t0.farmado,103) [FechaArmado]
		,convert(char,t0.FDESPACHO,103) [FechaEntrega]
FROM [ArmadoMotos].[dbo].[MACROINSERT_HEADER] t0 WHERE ID>32 AND DATEDIFF(DAY,[DATECREATED],GETDATE())<=15 and [ESTATUS]='A' and ESTADO>=3

ORDER BY DATECREATED DESC

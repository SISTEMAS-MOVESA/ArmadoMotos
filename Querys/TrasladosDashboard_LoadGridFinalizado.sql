
SELECT	t0.ID
		,t0.ORIGEN
		,t0.DESTINO
		,t0.RUTA
		,t0.ESTADO
		,t0.DATECREATED [FECHA]
		,t0.USERCREATED
		,t0.DOCENTRYSAP [DOCENTRYSAP]
		,t0.SUPERVISOR [SUPERVISORASIGNADO]
		,(select [Name] from movesa..[@CRESPONSABLEOWHS] where code=(select U_Categorizacion from movesa..owhs where WhsCode=[DESTINO] collate Modern_Spanish_CI_AS))[Supervisor]
		,(select whsname from movesa..owhs where WhsCode=[DESTINO] collate Modern_Spanish_CI_AS)[NALMDESTINO] 
		,CASE ESTADO WHEN '1' THEN 'SUGERIDO' WHEN '2' THEN 'SUCURSAL' WHEN '3' THEN 'LOGISTICA' WHEN '4' THEN 'CARGA'  
		WHEN '5' THEN 'SOLICITUD' WHEN '6' THEN 'TRANSFER' WHEN '7' THEN 'Despachado' end [ETAPA] 
		,convert(char,t0.farmado,103) [FechaArmado]
		,convert(char,t0.FDESPACHO,103) [FechaEntrega]
FROM [ArmadoMotos].[dbo].[MACROINSERT_HEADER] t0 WHERE ID>32 AND DATEDIFF(DAY,[DATECREATED],GETDATE())<=15 and [ESTATUS]='C'

UNION ALL
SELECT distinct t0.[ID]
		,t0.ALMORIGEN COLLATE Modern_Spanish_CI_AS
		,t0.[ALMDESTINO] COLLATE Modern_Spanish_CI_AS[DESTINO]
		,t4.Name COLLATE Modern_Spanish_CI_AS[Ruta]
		,2 [Estado]
		,[DATECREATED] [FECHA]
		,t0.USERCREATED
		,NULL [DOCENTRYSAP]
		,NULL [SUPERVISORASIGNADO]
		,t3.Name COLLATE Modern_Spanish_CI_AS [Supervisor]
		,t2.WhsName [NALMDESTINO]
		,'PRE SOLICITUD CI' [ETAPA]
	    ,(select top 1 convert(char,FECHAARMADO,103) from SOLCITUD_HEADER where MACROID = t0.[ID])[FechaArmado]
		,(select top 1 convert(char,FECHAENTREGA,103) from SOLCITUD_HEADER where MACROID = t0.[ID])[FechaEntrega]
  FROM [ArmadoMotos].[dbo].[PRESOLICITUD_HEADER] t0
		inner join (
						SELECT WhsName,WhsCode,U_Categorizacion,U_Ruta FROM MOVESA..OWHS WITH(NOLOCK) WHERE U_TYPE<>'PRO'
						AND WhsCode NOT IN ('BPD01','CDM00','CDR00','CDR01','CONCA01','Consumir','DCM00','DCR00',    
						'DCR01','DEV01','DEV02','EST001','FCE01','MAY01','MLCB01','MLCB02','MOB01','MSPS01','MSPS02',    
						'MSPS03','MSPS04','MSUC01','MSUC02','MSUC03','MSUC04','MSUC05','MSUC07','MTGA01','MTRANS00',    
						'RLCB02','RLCB03','RSPS02','RSPS03','RSPS04','RSPS05','RSPS06','RSPS07','RTGA01',    
						'RTGA0102','SUC0102','SUC0201','SUC03','SUC0301','SUC0401','SUC0701','SUC0801','SUM001','DIU-10','GMG122','mym 09','con') 
					) t2
		on t0.ALMDESTINO collate Modern_Spanish_CI_AS=t2.WhsCode
		inner join (
					select Code,Name from movesa..[@CRESPONSABLEOWHS] with(nolock)	
					) t3
		on t2.U_Categorizacion=t3.Code
		inner join (
					select code, name from movesa..[@CRUTA] with(nolock)
					) t4
		on t2.U_Ruta = t4.Code
where  ISNULL(ESTADO,'-')='C'
UNION ALL

SELECT distinct t0.[ID]
      ,t0.ALMORIGEN
	  ,t0.[ALMDESTINO]
	  ,t4.Name [Ruta]
      ,2 [Estado]
	  ,[DATECREATED][FECHA]
	  ,t0.USERCREATED
	  ,NULL [DOCENTRYSAP]
	  ,NULL [SUPERVISORASIGNADO]
	  ,t3.Name [Supervisor]
	  ,t2.WhsName
	  ,'PRE SOLICITUD CD' [ETAPA]
	    ,(select top 1 convert(char,FECHAARMADO,103) from SOLCITUD_HEADER where MACROID = t0.[ID])[FechaArmado]
		,(select top 1 convert(char,FECHAENTREGA,103) from SOLCITUD_HEADER where MACROID = t0.[ID])[FechaEntrega]
  FROM [ArmadoMotos].[dbo].[PRESOLICITUD_HEADER] t0
		inner join (
						SELECT WhsName,WhsCode,U_Categorizacion,U_Ruta FROM MOVESA..OWHS WITH(NOLOCK) WHERE U_TYPE='PRO'
						AND WhsCode NOT IN ('BPD01','CDM00','CDR00','CDR01','CONCA01','Consumir','DCM00','DCR00',    
						'DCR01','DEV01','DEV02','EST001','FCE01','MAY01','MLCB01','MLCB02','MOB01','MSPS01','MSPS02',    
						'MSPS03','MSPS04','MSUC01','MSUC02','MSUC03','MSUC04','MSUC05','MSUC07','MTGA01','MTRANS00',    
						'RLCB02','RLCB03','RSPS02','RSPS03','RSPS04','RSPS05','RSPS06','RSPS07','RTGA01',    
						'RTGA0102','SUC0102','SUC0201','SUC03','SUC0301','SUC0401','SUC0701','SUC0801','SUM001','DIU-10','GMG122','mym 09','con') 
					) t2
		on t0.ALMDESTINO collate Modern_Spanish_CI_AS=t2.WhsCode
		inner join (
					select Code,Name from movesa..[@CRESPONSABLEOWHS] with(nolock)	
					) t3
		on t2.U_Categorizacion=t3.Code
		inner join (
					select code, name from movesa..[@CRUTA] with(nolock)
					) t4
		on t2.U_Ruta = t4.Code
where ISNULL(ESTADO,'-')='C'

ORDER BY DATECREATED DESC

--SELECT distinct t0.[ID]
--		,t0.ALMORIGEN
--		,t0.[ALMDESTINO] [DESTINO]
--		,t4.Name [Ruta]
--		,2 [Estado]
--		,[DATECREATED] [FECHA]
--		,t0.USERCREATED
--		,NULL [DOCENTRYSAP]
--		,NULL [SUPERVISORASIGNADO]
--		,[DATECREATED][SUPERVISORASIGNADO]
--		,t3.Name [Supervisor]
--		,t2.WhsName [NALMDESTINO]
--		,'PRE SOLICITUD CI' [ETAPA]
--	    ,(select top 1 convert(char,FECHAARMADO,103) from SOLCITUD_HEADER where MACROID = t0.[ID])[FechaArmado]
--		,(select top 1 convert(char,FECHAENTREGA,103) from SOLCITUD_HEADER where MACROID = t0.[ID])[FechaEntrega]
--  FROM [ArmadoMotos].[dbo].[PRESOLICITUD_HEADER] t0
--		inner join (
--						SELECT WhsName,WhsCode,U_Categorizacion,U_Ruta FROM MOVESA..OWHS WITH(NOLOCK) WHERE U_TYPE<>'PRO'
--						AND WhsCode NOT IN ('BPD01','CDM00','CDR00','CDR01','CONCA01','Consumir','DCM00','DCR00',    
--						'DCR01','DEV01','DEV02','EST001','FCE01','MAY01','MLCB01','MLCB02','MOB01','MSPS01','MSPS02',    
--						'MSPS03','MSPS04','MSUC01','MSUC02','MSUC03','MSUC04','MSUC05','MSUC07','MTGA01','MTRANS00',    
--						'RLCB02','RLCB03','RSPS02','RSPS03','RSPS04','RSPS05','RSPS06','RSPS07','RTGA01',    
--						'RTGA0102','SUC0102','SUC0201','SUC03','SUC0301','SUC0401','SUC0701','SUC0801','SUM001','DIU-10','GMG122','mym 09','con') 
--					) t2
--		on t0.ALMDESTINO collate Modern_Spanish_CI_AS=t2.WhsCode
--		inner join (
--					select Code,Name from movesa..[@CRESPONSABLEOWHS] with(nolock)	
--					) t3
--		on t2.U_Categorizacion=t3.Code
--		inner join (
--					select code, name from movesa..[@CRUTA] with(nolock)
--					) t4
--		on t2.U_Ruta = t4.Code
--where  ISNULL(ESTADO,'-')='C'
--UNION ALL

--SELECT distinct t0.[ID]
--      ,t0.ALMORIGEN
--	  ,t0.[ALMDESTINO]
--	  ,t4.Name [Ruta]
--      ,2 [Estado]
--	  ,[DATECREATED]
--	  ,t0.USERCREATED
--	  ,NULL [DOCENTRYSAP]
--	  ,NULL [SUPERVISORASIGNADO]
--	  ,[DATECREATED][FECHA]
--	  ,t3.Name [Supervisor]
--	  ,t2.WhsName
--	  ,'PRE SOLICITUD CD' [ETAPA]
--	    ,(select top 1 convert(char,FECHAARMADO,103) from SOLCITUD_HEADER where MACROID = t0.[ID])[FechaArmado]
--		,(select top 1 convert(char,FECHAENTREGA,103) from SOLCITUD_HEADER where MACROID = t0.[ID])[FechaEntrega]
--  FROM [ArmadoMotos].[dbo].[PRESOLICITUD_HEADER] t0
--		inner join (
--						SELECT WhsName,WhsCode,U_Categorizacion,U_Ruta FROM MOVESA..OWHS WITH(NOLOCK) WHERE U_TYPE='PRO'
--						AND WhsCode NOT IN ('BPD01','CDM00','CDR00','CDR01','CONCA01','Consumir','DCM00','DCR00',    
--						'DCR01','DEV01','DEV02','EST001','FCE01','MAY01','MLCB01','MLCB02','MOB01','MSPS01','MSPS02',    
--						'MSPS03','MSPS04','MSUC01','MSUC02','MSUC03','MSUC04','MSUC05','MSUC07','MTGA01','MTRANS00',    
--						'RLCB02','RLCB03','RSPS02','RSPS03','RSPS04','RSPS05','RSPS06','RSPS07','RTGA01',    
--						'RTGA0102','SUC0102','SUC0201','SUC03','SUC0301','SUC0401','SUC0701','SUC0801','SUM001','DIU-10','GMG122','mym 09','con') 
--					) t2
--		on t0.ALMDESTINO collate Modern_Spanish_CI_AS=t2.WhsCode
--		inner join (
--					select Code,Name from movesa..[@CRESPONSABLEOWHS] with(nolock)	
--					) t3
--		on t2.U_Categorizacion=t3.Code
--		inner join (
--					select code, name from movesa..[@CRUTA] with(nolock)
--					) t4
--		on t2.U_Ruta = t4.Code
--where ISNULL(ESTADO,'-')='C'

--ORDER BY DATECREATED DESC

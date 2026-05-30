--DELETE FROM PRESOLICITUD_HEADER WHERE DATEDIFF(DAY,DATECREATED,GETDATE())>10;


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
FROM [ArmadoMotos].[dbo].[MACROINSERT_HEADER] t0 WHERE ID>32 and [ESTATUS]='A' and ESTADO<=2

UNION ALL


SELECT	DISTINCT T0.DocNum
		,t0.Filler COLLATE Modern_Spanish_CI_AS
		,t0.ToWhsCode COLLATE Modern_Spanish_CI_AS
		,t0.JrnlMemo COLLATE Modern_Spanish_CI_AS
		,t0.U_Espacios
		,CONVERT(CHAR,t0.DocDate,103)
		,t0.Ref1
		,t0.DocEntry
		,NULL
		,T0.DocDate
		,(select [Name] from movesa..[@CRESPONSABLEOWHS] where code=(select U_Categorizacion from movesa..owhs where WhsCode = T0.TOWHSCODE  collate Modern_Spanish_CI_AS))
		,(SELECT WHSNAME FROM MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE = T0.TOWHSCODE ),'SOLICITUD TRASLADO'
		,(select top 1 convert(char,FECHAARMADO,103) from SOLCITUD_HEADER where MACROID= T0.DocNum )[FechaArmado]
		,(select top 1 convert(char,FECHAENTREGA,103) from SOLCITUD_HEADER where MACROID=T0.DocNum)[FechaEntrega]

FROM MOVESA..OWTQ T0 WITH(NOLOCK) INNER JOIN MOVESA..WTQ1 T1 WITH(NOLOCK) ON T0.DocEntry=T1.DOCENTRY
WHERE T0.DocStatus = 'O' AND T0.U_Envio='PP_ST' AND DATEDIFF(DAY,t0.DocDate,GETDATE())<7


UNION ALL

SELECT distinct t0.[ID]
		,t0.ALMORIGEN
		,t0.[ALMDESTINO]
		,t4.Name [Ruta]
		,2 [Estado]
		,[DATECREATED]
		,t0.USERCREATED
		,NULL [DOCENTRYSAP]
		,NULL [SUPERVISORASIGNADO]
		,[DATECREATED][FECHA]
		,t3.Name [Supervisor]
		,t2.WhsName
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
where DATEDIFF(DAY,[DATECREATED],GETDATE())<7 and ISNULL(ESTADO,'-')<>'C'
UNION ALL

SELECT distinct t0.[ID]
      ,t0.ALMORIGEN
	  ,t0.[ALMDESTINO]
	  ,t4.Name [Ruta]
      ,2 [Estado]
	  ,[DATECREATED]
	  ,t0.USERCREATED
	  ,NULL [DOCENTRYSAP]
	  ,NULL [SUPERVISORASIGNADO]
	  ,[DATECREATED][FECHA]
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
where DATEDIFF(DAY,[DATECREATED],GETDATE())<7 and ISNULL(ESTADO,'-')<>'C'

ORDER BY DATECREATED DESC

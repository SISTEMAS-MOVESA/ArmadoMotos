SELECT	ROW_NUMBER() over(order by t1.itemcode) as no 
		,t3.Name [Modelo]  
		,t5.Name [Marca]
		,t4.Name [Color]
		,t6.Name [Cilindros]
		,t0.BatchId [Year]
		,T1.ItemName [Descripcion]
		,T0.WhsCode [Cod.Almacen]
		,T2.WhsName [NombreAlmacen] 
		,(select U_CardCode from owhs with(nolock) where whscode=T0.WhsCode)[CodCliente]
		,T0.SuppSerial [SerieChasis]
		,T0.IntrSerial [SerieMotor]  
		,(SELECT TOP 1 DATEDIFF(DAY,T6.DocDate,GETDATE()) FROM SRI1 T6 WITH(NOLOCK)
			WHERE T0.SysSerial=T6.SysSerial AND T0.ItemCode=T6.ItemCode  ORDER BY T6.DocDate DESC)[Dias en Almacen]  
		,(SELECT TOP 1 DATEDIFF(DAY,T6.DocDate,GETDATE()) FROM SRI1 T6 WITH(NOLOCK) 
			WHERE T0.SysSerial=T6.SysSerial AND T0.ItemCode=T6.ItemCode  ORDER BY T6.DocDate ASC)[Dia en Movesa]   

FROM	OSRI T0 WITH(NOLOCK)  
		inner join 
		OITM T1  WITH(NOLOCK) 
		on t0.ItemCode=T1.ItemCode  
		inner join 
		OWHS t2  WITH(NOLOCK) 
		on T0.WhsCode=T2.WhsCode  
		left join 
		[@AMODELO] t3  WITH(NOLOCK) 
		on t1.U_AMODELO=t3.Code  
		left join
		[@SCOLOR]  t4 with(nolock)
		on t1.U_ACOLOR=t4.Code
		left join 
		[@AMARCA] t5 with(nolock)
		on t1.U_AMARCA=t5.code
		left join
		[@ACILINDROS] t6 with(nolock)
		on t1.U_ACILINDROS=t6.Code
WHERE t0.whscode not in ('TCEDI','DCM00','DEV01', 'CDM00','MSPS01') and T0.ITEMCODE<>'PLA001' AND  T0.Status=0 AND T1.ItmsGrpCod=154 and t1.ManSerNum='Y'  
and t3.Name = @modelo
order by [no]
SELECT T1.NAME [Tipo]
,ISNULL((SELECT ESPACIOS FROM [ArmadoMotos].[dbo].[CBLOGICO] WHERE TIPOMOTO collate Modern_Spanish_CI_AS = T1.Name AND WHSCODE  = @whscode ),0) [Espacios]
,ISNULL((SELECT CBLOGICO FROM [ArmadoMotos].[dbo].[CBLOGICO] WHERE TIPOMOTO collate Modern_Spanish_CI_AS = T1.Name AND WHSCODE  = @whscode ),0) [Cantidad]
FROM MOVESA..[@TIPOMOTOCICLETA] t1  where t1.Name not in ('CHOPPER')



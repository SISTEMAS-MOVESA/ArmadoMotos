Imports Microsoft.VisualBasic
Imports SAPbobsCOM

Public Class GlobalConnecttoSAP
    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public xCompany As SAPbobsCOM.Company
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public oGoodsRcpt As SAPbobsCOM.Documents

    ' Variable estática para mantener la instancia
    Private Shared _instancia As GlobalConnecttoSAP

    Public Shared Function Connect() As Integer
        ' Crear una nueva instancia
        _instancia = New GlobalConnecttoSAP()

        _instancia.SCompany = New SAPbobsCOM.Company
        _instancia.SCompany.Server = "server"
        _instancia.SCompany.CompanyDB = "MOVESA"
        _instancia.SCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
        _instancia.SCompany.DbUserName = "sa"
        _instancia.SCompany.DbPassword = "pass"
        _instancia.SCompany.UserName = "user"
        _instancia.SCompany.Password = "pass"
        _instancia.SCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
        _instancia.SCompany.SLDServer = "lsservice"

        _instancia.lRetCode = _instancia.SCompany.Connect

        ' Obtener el error si existe
        If _instancia.lRetCode <> 0 Then
            _instancia.SCompany.GetLastError(_instancia.lErrCode, _instancia.sErrMsg)
        End If

        Return _instancia.lRetCode
    End Function

    ' Propiedad para acceder a la instancia
    Public Shared ReadOnly Property Instancia As GlobalConnecttoSAP
        Get
            Return _instancia
        End Get
    End Property
End Class
<%@ WebHandler Language="VB" Class="UpdateCuadroBasicoV2Handler" %>

Imports System
Imports System.Web
Imports System.Web.SessionState
Imports System.Data
Imports System.Data.SqlClient

Public Class UpdateCuadroBasicoV2Handler
    Implements IHttpHandler, IRequiresSessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/plain"
        Try
            Dim maximo As Integer = Convert.ToInt32(context.Request("maximo"))
            Dim whsCode As String = context.Request("whscode")
            Dim modelo As String = context.Request("modelo")
            Dim cbAnterior As Integer = SafeInt(context.Request("cb_anterior"))
            Dim cbSugerido As Integer = SafeInt(context.Request("cb_sugerido"))
            Dim ventaDiaria As Decimal = SafeDec(context.Request("venta_diaria"))
            Dim diasCobObj As Object = ParseCob(context.Request("dias_cobertura"))
            Dim motivo As String = context.Request("motivo")
            Dim origen As String = If(String.IsNullOrEmpty(context.Request("origen")), "INDIVIDUAL", context.Request("origen"))
            Dim usuario As String = If(context.Session("Name") IsNot Nothing, context.Session("Name").ToString(), "anon")
            Dim ip As String = context.Request.UserHostAddress

            CuadroBasicoHelper.ActualizarCB(whsCode, modelo, cbAnterior, maximo,
                                            cbSugerido, ventaDiaria, diasCobObj,
                                            usuario, ip, motivo, origen)

            context.Response.Write("OK · " & whsCode & " · " & modelo & " · CB " & cbAnterior & " → " & maximo)
        Catch ex As Exception
            context.Response.StatusCode = 500
            context.Response.Write("ERR: " & ex.Message)
        End Try
    End Sub

    Private Shared Function SafeInt(v As String) As Integer
        Dim n As Integer
        If Integer.TryParse(v, n) Then Return n
        Return 0
    End Function

    Private Shared Function SafeDec(v As String) As Decimal
        Dim d As Decimal
        If Decimal.TryParse(v, d) Then Return d
        Return 0D
    End Function

    Private Shared Function ParseCob(v As String) As Object
        If String.IsNullOrEmpty(v) OrElse v = "—" OrElse v.ToLower() = "nan" Then Return DBNull.Value
        Dim d As Decimal
        If Decimal.TryParse(v, d) Then Return d
        Return DBNull.Value
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
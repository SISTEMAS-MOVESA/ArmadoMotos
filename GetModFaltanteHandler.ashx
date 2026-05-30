<%@ WebHandler Language="VB" Class="GetModFaltanteHandler" %>

Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text

Public Class GetModFaltanteHandler
    Implements IHttpHandler

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.Charset = "utf-8"

        Dim whscode As String = If(context.Request.QueryString("whscode"), "").Trim()
        Dim rango   As String = If(context.Request.QueryString("rango"),   "").Trim()

        If whscode = "" Then
            context.Response.Write("[]")
            Return
        End If

        Dim wherePareto As String
        Select Case rango
            Case "1_40"  : wherePareto = "PARETOPERCENT <= 0.40"
            Case "41_70" : wherePareto = "PARETOPERCENT > 0.40 AND PARETOPERCENT <= 0.70"
            Case Else    : wherePareto = "PARETOPERCENT <= 0.80"
        End Select

        Dim sql As String =
            "SELECT MODELO, CAST(FAIN AS INT) AS FAIN, CAST(CB AS INT) AS CB, CAST(FI AS INT) AS FI " &
            "FROM TempParetoAllWhscodes WITH(NOLOCK) " &
            "WHERE WHSCODE = @whs AND " & wherePareto & " AND FAIN > 0 " &
            "ORDER BY FAIN DESC"

        Dim sb As New StringBuilder("[")
        Try
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@whs", whscode)})

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim r As DataRow = dt.Rows(i)
                Dim modelo As String = r("MODELO").ToString().Replace("""", "'")
                Dim fain   As Integer = Convert.ToInt32(r("FAIN"))
                Dim cb     As Integer = Convert.ToInt32(r("CB"))
                Dim fi     As Integer = Convert.ToInt32(r("FI"))
                If i > 0 Then sb.Append(",")
                sb.AppendFormat("{{""m"":""{0}"",""f"":{1},""cb"":{2},""fi"":{3}}}", modelo, fain, cb, fi)
            Next
        Catch ex As Exception
            context.Response.Write("[]")
            Return
        End Try

        sb.Append("]")
        context.Response.Write(sb.ToString())
    End Sub

    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class

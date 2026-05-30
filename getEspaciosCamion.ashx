<%@ WebHandler Language="VB" Class="getEspaciosCamion" %>
Imports System.Web
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Public Class getEspaciosCamion : Implements IHttpHandler
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"

        Dim placa As String = context.Request.QueryString("placa")
        Dim resultado As New List(Of Dictionary(Of String, Object))

        If String.IsNullOrEmpty(placa) Then
            context.Response.Write("{""error"":""Falta Numero de Placa""}")
            Return
        End If

        Dim connStr As String = sCon2
        Using conn As New SqlConnection(connStr)
            Dim cmd As New SqlCommand("SELECT distinct [ESPACIOS] FROM [ArmadoMotos].[dbo].[TRANSPORTISTAS] where PLACA = @placa", conn)
            cmd.Parameters.AddWithValue("@placa", placa)

            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()
                Dim fila As New Dictionary(Of String, Object)
                For i As Integer = 0 To reader.FieldCount - 1
                    fila.Add(reader.GetName(i), reader(i))
                Next
                resultado.Add(fila)
            End While
            reader.Close()
        End Using

        Dim json As String = New JavaScriptSerializer().Serialize(resultado)
        context.Response.Write(json)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class

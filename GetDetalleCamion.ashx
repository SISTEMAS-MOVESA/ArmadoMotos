<%@ WebHandler Language="VB" Class="GetDetalleCamion" %>
Imports System.Web
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Public Class GetDetalleCamion : Implements IHttpHandler
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"

        Dim despachoid As String = context.Request.QueryString("despachoid")
        Dim resultado As New List(Of Dictionary(Of String, Object))

        If String.IsNullOrEmpty(despachoid) Then
            context.Response.Write("{""error"":""Falta el parámetro despachoid""}")
            Return
        End If

        Dim connStr As String = sCon2
        Using conn As New SqlConnection(connStr)
            Dim cmd As New SqlCommand("SELECT DOCNUM, LINENUM, ROAD, LINETYPE, TO_WHSCODE, TO_WHSNAME, ITEMCODE, ITEMNAME, SERIAL_NUMBER, 'N' AS SELECTED, SLOT " &
                                        "FROM ArmadoMotos..VW_DESPACHOS_DETALLE WHERE DOCNUM = @despachoid AND PICKED = 'Y' AND IN_TRUCK = 'N' " &
                                        "ORDER BY TO_WHSCODE, LINETYPE, ITEMCODE", conn)
            cmd.Parameters.AddWithValue("@despachoid", despachoid)

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

<%@ WebHandler Language="VB" Class="CerrarDespachoHandler" %>

Imports System.Web
Imports System.Web.SessionState ' Importar para usar Session
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient

Public Class CerrarDespachoHandler
    Implements IHttpHandler, IRequiresSessionState ' Habilitar sesión

    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public idPlan As Integer
    Public despachoID As Integer
    Public usuario As String

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        Dim serializer As New JavaScriptSerializer()
        Try
            Dim input As String = New IO.StreamReader(context.Request.InputStream).ReadToEnd()
            Dim data As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(input)
            idPlan = Convert.ToInt32(data("id"))
            despachoID = Convert.ToInt32(data("despachoId"))

            ' Obtener el usuario desde la sesión
            usuario = context.Session("User").ToString()

            Using conn As New SqlConnection(sCon2)
                conn.Open()
                Dim sql As String = " UPDATE [ArmadoMotos].[dbo].[DEPACHOS_HEADER] SET [ESTADO] = 'CERRADO', [USUARIOMOFICACION] = @usuario, [FECHAMODIFICACION] = GETDATE() WHERE PLANID = @id AND ID = @despachoID; " &
                                    " UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO = 'CERRADO' WHERE PLANID = @id AND DESPACHOID= @despachoID;" &
                                    " DELETE FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE PLANID = @id AND HEADERID = @despachoID;" &
                                    " UPDATE MOVESAWEB..TBL_REPORTES_VENTAS_DISTRIBUIDORES_DETALLE SET DISPATCH_ID = NULL, DISPATCH_DATE = NULL WHERE DISPATCH_ID = @despachoID"

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@usuario", usuario)
                    cmd.Parameters.AddWithValue("@id", idPlan)
                    cmd.Parameters.AddWithValue("despachoID", despachoID)
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        context.Response.Write(serializer.Serialize(New With {.success = True}))
                    Else
                        context.Response.Write(serializer.Serialize(New With {.success = False, .message = "No se encontró la planificación."}))
                    End If
                End Using
                conn.Close()
            End Using

        Catch ex As Exception
            context.Response.Write(serializer.Serialize(New With {.success = False, .message = ex.Message, .idPlan = idPlan, .username = usuario}))
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class

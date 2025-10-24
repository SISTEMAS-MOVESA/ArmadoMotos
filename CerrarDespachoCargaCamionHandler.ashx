<%@ WebHandler Language="VB" Class="CerrarDespachoCrearCamion" %>

Imports System.Web
Imports System.Web.SessionState
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient

Public Class CerrarDespachoCrearCamion
    Implements IHttpHandler, IRequiresSessionState
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public idPlan As Integer
    Public despachoID As Integer
    Public usuario As String

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        Dim serializer As New JavaScriptSerializer()

        Try
            ' Leer JSON de entrada
            Dim input As String = New IO.StreamReader(context.Request.InputStream).ReadToEnd()
            Dim data As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(input)

            If Not data.ContainsKey("id") OrElse Not data.ContainsKey("despachoId") Then
                context.Response.Write(serializer.Serialize(New With {.success = False, .message = "Parámetros incompletos."}))
                Return
            End If

            idPlan = Convert.ToInt32(data("id"))
            despachoID = Convert.ToInt32(data("despachoId"))

            ' Validar sesión
            If context.Session("User") Is Nothing Then
                context.Response.Write(serializer.Serialize(New With {.success = False, .message = "Sesión expirada."}))
                Return
            End If

            usuario = context.Session("User").ToString()

            Using conn As New SqlConnection(sCon2)
                conn.Open()

                Dim sql As String =
                    " UPDATE [ArmadoMotos].[dbo].[DESPACHOS_HEADER] SET [ESTADO] = 'CERRADO', [USUARIOMODIFICACION] = @usuario, [FECHAMODIFICACION] = GETDATE() WHERE PLANID = @id AND ID = @despachoID; " &
                    " UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO = 'CAMION DESPACHADO' WHERE PLANID = @id AND DESPACHOID = @despachoID; " &
                    " UPDATE [ArmadoMotos].[dbo].[MACROINSERT] SET ESTADO='F' WHERE PLANID = @id AND HEADERID = @despachoID; "

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@usuario", usuario)
                    cmd.Parameters.AddWithValue("@id", idPlan)
                    cmd.Parameters.AddWithValue("@despachoID", despachoID)

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    context.Response.Write(serializer.Serialize(New With {
                        .success = rowsAffected > 0,
                        .message = If(rowsAffected > 0, "Despacho cerrado correctamente.", "No se encontró la planificación.")
                    }))
                End Using
            End Using

        Catch ex As Exception
            context.Response.Write(serializer.Serialize(New With {
                .success = False,
                .message = ex.Message,
                .idPlan = idPlan,
                .usuario = usuario
            }))
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class

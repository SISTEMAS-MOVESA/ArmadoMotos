<%@ WebHandler Language="VB" Class="PlanificacionUpdateAfterCrearHandler" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports System.Web.SessionState ' Importar para usar Session

Public Class PlanificacionUpdateAfterCrearHandler : Implements IHttpHandler, IRequiresSessionState ' Habilitar sesión
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"

        Try
            ' Leer los datos enviados desde AJAX
            Dim jsonData As String = New System.IO.StreamReader(context.Request.InputStream).ReadToEnd()
            Dim serializer As New JavaScriptSerializer()
            Dim planData As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(jsonData)

            ' Extraer valores
            Dim ID As Integer = planData("id")
            ' Conexión a la base de datos (ajusta la cadena de conexión)
            Using conn As New SqlConnection(sCon2)
                conn.Open()
                Dim sel As String = " update [ArmadoMotos].[dbo].[PLANIFICACIONES_DETALLE]  set [HEADERID] = @p1, [ESTADOHEADER] = 'PLANIFICACION' " &
                                    " where [ESTADOHEADER] = 'TEMPORAL' and [ESTADOLINEA] = 'ABIERTO' and [HEADERID] = 0 "

                Using cmd As New SqlCommand(sel, conn)
                    cmd.Parameters.AddWithValue("@p1", ID)
                    cmd.ExecuteNonQuery()
                    Dim responseObj = New With {.success = True, .message = "Planificación Actualizada correctamente.", .id = ID}
                    context.Response.Write(serializer.Serialize(responseObj))
                End Using
                conn.Close()
            End Using

        Catch ex As Exception
            ' Manejo de errores
            Dim errorObj = New With {.success = False, .message = "Error: " & ex.Message}
            context.Response.Write(New JavaScriptSerializer().Serialize(errorObj))
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
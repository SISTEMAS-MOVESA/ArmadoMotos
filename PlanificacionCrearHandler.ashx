<%@ WebHandler Language="VB" Class="PlanificacionCrearHandler" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports System.Web.SessionState ' Importar para usar Session

Public Class PlanificacionCrearHandler : Implements IHttpHandler, IRequiresSessionState ' Habilitar sesión
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"

        Try
            ' Leer los datos enviados desde AJAX
            Dim jsonData As String = New System.IO.StreamReader(context.Request.InputStream).ReadToEnd()
            Dim serializer As New JavaScriptSerializer()
            Dim planData As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(jsonData)

            ' Extraer valores
            Dim FECHAINICIO As DateTime = Convert.ToDateTime(planData("fechaInicio")).ToString("yyyy-MM-dd")
            Dim FECHAVENCIMIENTO As DateTime = Convert.ToDateTime(planData("fechaVencimiento")).ToString("yyyy-MM-dd")
            Dim FALTANTE As Integer = If(IsNumeric(planData("cantidadMotos")), Convert.ToInt32(planData("cantidadMotos")), 0)

            'Dim FECHAINICIO As String = planData("fechaInicio").ToString("yyyy-MM-dd")
            'Dim FECHAVENCIMIENTO As String = planData("fechaVencimiento").ToString("yyyy-MM-dd")
            'Dim FALTANTE As String = planData("cantidadMotos").ToString()
            Dim ESTADO As String = "ABIERTO"
            Dim USUARIO As String = If(context.Session("User"), "ADMIN")
            Dim FECHACREACION As DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

            ' Conexión a la base de datos (ajusta la cadena de conexión)
            Using conn As New SqlConnection(sCon2)
                conn.Open()
                Dim sel As String = " INSERT INTO [dbo].[PLANIFICACIONES]([FECHAINICIO],[FECHAVENCIMIENTO]," &
                                    " [FALTANTE],[ESTADO],[USUARIO],[FECHACREACION])" &
                                    " VALUES (@p1,@p2,@p3,@p4,@p5,@p6)" &
                                    " SELECT SCOPE_IDENTITY();"

                Using cmd As New SqlCommand(sel, conn)


                    cmd.Parameters.AddWithValue("@p1", FECHAINICIO)
                    cmd.Parameters.AddWithValue("@p2", FECHAVENCIMIENTO)
                    cmd.Parameters.AddWithValue("@p3", FALTANTE)
                    cmd.Parameters.AddWithValue("@p4", ESTADO)
                    cmd.Parameters.AddWithValue("@p5", USUARIO)
                    cmd.Parameters.AddWithValue("@p6", FECHACREACION)

                    ' Ejecutar y obtener el ID insertado
                    Dim idPlanificacion As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    ' Responder con el ID generado
                    Dim responseObj = New With {.success = True, .message = "Planificación creada correctamente.", .id = idPlanificacion}
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
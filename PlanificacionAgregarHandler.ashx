<%@ WebHandler Language="VB" Class="PlanificacionHandler" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports System.Web.SessionState ' Importar para usar Session

Public Class PlanificacionHandler : Implements IHttpHandler, IRequiresSessionState ' Habilitar sesión
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"

        Try
            ' Leer los datos enviados desde AJAX
            Dim jsonData As String = New System.IO.StreamReader(context.Request.InputStream).ReadToEnd()
            Dim serializer As New JavaScriptSerializer()
            Dim planData As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(jsonData)

            ' Extraer valores
            Dim fechaInicio As String = planData("fechaInicio").ToString()
            Dim fechaVencimiento As String = planData("fechaVencimiento").ToString()
            Dim estado As String = "ABIERTO"

            ' Obtener usuario desde sesión (ya no dará error)
            Dim usuario As String = If(context.Session("User"), "ADMIN")

            Dim fechaCreacion As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            Dim jsonDetalles As String = serializer.Serialize(planData("detalles"))

            ' Conexión a la base de datos (ajusta la cadena de conexión)
            Using conn As New SqlConnection(sCon2)
                conn.Open()
                Dim sel As String = "INSERT INTO dbo.PLANIFICACIONES (FECHAINICIO, FECHAVENCIMIENTO, ESTADO, USUARIO, FECHACREACION, JSON) VALUES (@p1, @p2, @p3, @p4, @p5, @p6); SELECT SCOPE_IDENTITY();"

                Using cmd As New SqlCommand(sel, conn)
                    cmd.Parameters.AddWithValue("@p1", fechaInicio)
                    cmd.Parameters.AddWithValue("@p2", fechaVencimiento)
                    cmd.Parameters.AddWithValue("@p3", estado)
                    cmd.Parameters.AddWithValue("@p4", usuario)
                    cmd.Parameters.AddWithValue("@p5", fechaCreacion)
                    cmd.Parameters.AddWithValue("@p6", jsonDetalles)

                    ' Ejecutar y obtener el ID insertado
                    Dim idPlanificacion As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    ' Responder con el ID generado
                    Dim responseObj = New With {.success = True, .message = "Planificación creada correctamente.", .id = idPlanificacion}
                    context.Response.Write(serializer.Serialize(responseObj))
                End Using
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

'Imports System
'Imports System.Web
'Imports System.Data.SqlClient
'Imports System.Web.Script.Serialization

'Public Class PlanificacionHandler : Implements IHttpHandler
'    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

'    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
'        context.Response.ContentType = "application/json"

'        Try
'            ' Leer los datos enviados desde AJAX
'            Dim jsonData As String = New System.IO.StreamReader(context.Request.InputStream).ReadToEnd()
'            Dim serializer As New JavaScriptSerializer()
'            Dim planData As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(jsonData)

'            ' Extraer valores
'            Dim fechaInicio As String = planData("fechaInicio").ToString()
'            Dim fechaVencimiento As String = planData("fechaVencimiento").ToString()
'            Dim estado As String = "ABIERTO"
'            Dim usuario As String = context.Session("User") ' Obtener usuario de sesión
'            Dim fechaCreacion As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
'            Dim jsonDetalles As String = serializer.Serialize(planData("detalles"))

'            ' Conexión a la base de datos (ajusta la cadena de conexión)
'            Using conn As New SqlConnection(sCon2)
'                conn.Open()
'                Dim sel As String = "INSERT INTO dbo.PLANIFICACIONES (FECHAINICIO, FECHAVENCIMIENTO, ESTADO, USUARIO, FECHACREACION, JSON) VALUES (@p1, @p2, @p3, @p4, @p5, @p6); SELECT SCOPE_IDENTITY();"

'                Using cmd As New SqlCommand(sel, conn)
'                    cmd.Parameters.AddWithValue("@p1", fechaInicio)
'                    cmd.Parameters.AddWithValue("@p2", fechaVencimiento)
'                    cmd.Parameters.AddWithValue("@p3", estado)
'                    cmd.Parameters.AddWithValue("@p4", usuario)
'                    cmd.Parameters.AddWithValue("@p5", fechaCreacion)
'                    cmd.Parameters.AddWithValue("@p6", jsonDetalles)

'                    ' Ejecutar y obtener el ID insertado
'                    Dim idPlanificacion As Integer = Convert.ToInt32(cmd.ExecuteScalar())

'                    ' Responder con el ID generado
'                    Dim responseObj = New With {.success = True, .message = "Planificación creada correctamente.", .id = idPlanificacion}
'                    context.Response.Write(serializer.Serialize(responseObj))
'                End Using
'            End Using

'        Catch ex As Exception
'            ' Manejo de errores
'            Dim errorObj = New With {.success = False, .message = "Error: " & ex.Message}
'            context.Response.Write(New JavaScriptSerializer().Serialize(errorObj))
'        End Try
'    End Sub

'    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
'        Get
'            Return False
'        End Get
'    End Property

'End Class

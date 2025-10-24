<%@ WebHandler Language="VB" Class="UpdateOrdenamiento" %>
Imports System.Web
Imports System.Web.Services
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports System.Configuration
Imports System.Collections.Generic
Imports System.IO
Imports System.Data

Public Class UpdateOrdenamiento
    Implements System.Web.IHttpHandler

    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        Dim serializer As New JavaScriptSerializer()

        Try
            ' Leer el JSON recibido
            Dim jsonString As String
            Using reader As New StreamReader(context.Request.InputStream)
                jsonString = reader.ReadToEnd()
            End Using

            Dim input As CrearCamionInput = serializer.Deserialize(Of CrearCamionInput)(jsonString)

            If input Is Nothing OrElse input.lineas Is Nothing OrElse input.lineas.Count = 0 Then
                context.Response.StatusCode = 400
                context.Response.Write(serializer.Serialize(New With {
                    .success = False,
                    .message = "JSON inválido o incompleto",
                    .data = jsonString
                }))
                Return
            End If

            ' Convertir la fecha y obtener usuario
            Dim fechaSalida As DateTime = DateTime.ParseExact(input.fechaSalida, "yyyy-MM-dd", Nothing)
            Dim usuario As String = "PORTAL" 'context.Session("User").ToString()
            If String.IsNullOrWhiteSpace(usuario) Then usuario = "portal"

            ' Crear camión y obtener ID
            Dim camionID As Integer = crearCamion(fechaSalida, usuario)
            If camionID <= 0 Then
                context.Response.StatusCode = 400
                context.Response.Write(serializer.Serialize(New With {
                    .success = False,
                    .message = "Ah ocurrido un error al crear el camion",
                    .data = 0
                }))
                Return
            End If

            ' Insertar líneas usando transacción
            Using conn As New SqlConnection(sCon2)
                conn.Open()
                Using tran As SqlTransaction = conn.BeginTransaction()
                    Try
                        For Each lineaID In input.lineas
                            Using cmd As New SqlCommand("EXEC ARMADOMOTOS..[SP_PORTAL_CAMIONES] @FN = 'AGREGAR LINEA CAMION', @DOCNUM = @CAMION_ID, @DISPATCH_LINENUM = @LINEA_ID", conn, tran)
                                cmd.Parameters.AddWithValue("@CAMION_ID", camionID)
                                cmd.Parameters.AddWithValue("@LINEA_ID", lineaID)
                                cmd.ExecuteNonQuery()
                            End Using
                        Next

                        tran.Commit()

                        context.Response.Write(serializer.Serialize(New With {
                            .success = True,
                            .message = "Camion # " & camionID & " Creado con Éxito!",
                            .data = camionID
                        }))
                    Catch ex As SqlException
                        tran.Rollback()

                        context.Response.StatusCode = 500
                        context.Response.Write(serializer.Serialize(New With {
                            .success = False,
                            .message = ex.Message,
                            .data = 0
                        }))
                    End Try
                End Using
            End Using

        Catch ex As Exception
            context.Response.StatusCode = 500
            context.Response.Write(New JavaScriptSerializer().Serialize(New With {
                .success = False,
                .message = ex.Message
            }))
        End Try
    End Sub

    Public Function crearCamion(docDueDate As DateTime, userName As String) As Integer
        Dim resultado As Integer = -1

        Using conn As New SqlConnection(sCon2)
            Using cmd As New SqlCommand("[ArmadoMotos]..[SP_PORTAL_CAMIONES]", conn)
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.AddWithValue("@FN", "CREAR CAMION")
                cmd.Parameters.AddWithValue("@DOCDUEDATE", docDueDate)
                cmd.Parameters.AddWithValue("@USER", userName)

                conn.Open()

                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() AndAlso Not IsDBNull(reader("DOCNUM")) Then
                    resultado = Convert.ToInt32(reader("DOCNUM"))
                End If
                reader.Close()
            End Using
        End Using

        Return resultado
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    ' Estructura del JSON recibido
    Public Class CrearCamionInput
        Public Property fechaSalida As String
        Public Property lineas As List(Of Integer)
    End Class
End Class

<%@ WebHandler Language="VB" Class="InventarioHandlerReingreso" %>

Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Configuration

Public Class InventarioHandlerReingreso : Implements IHttpHandler
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"

        ' Leer el JSON recibido desde la solicitud
        Dim json As String
        Using reader As New System.IO.StreamReader(context.Request.InputStream)
            json = reader.ReadToEnd()
        End Using

        ' Deserializar el JSON en un diccionario
        Dim serializer As New JavaScriptSerializer()
        Dim data As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(json)

        ' Obtener la cadena de conexión
        Dim sCon As String = sCon2

        Try
            Using con As New SqlConnection(sCon)
                Dim query As String = "INSERT INTO InventarioVehiculosReingreso (Serie, ItemCode, ItemName, SerieMotor, Marca, Modelo, " &
                                      "Cilindros, Color, Almacen, EstatusSerie, YearMoto, DiasArmado, Ageing, Comentarios, " &
                                      "FechaInsercion, EstadoGeneralChasis, EstadoPintura, EstadoAsiento, CondicionRuedasDelantera, " &
                                      "CondicionRuedasTrasera, EstadoGuardafangos, Motor, EstadoAceite, SistemaTransmision, " &
                                      "SistemaRefrigeracion, SistemaEscape, LucesDelanteras, LucesTraseras, LucesDireccionales, " &
                                      "LucesFrenos, Bateria, ClaxonPito, EspejosRetrovisores, Frenos, Pedales, " &
                                      "FuncionamientoMotor, FuncionamientoTransmision, FuncionamientoFrenos, " &
                                      "EstabilidadSuspension, EstadoFinalVehiculo) " &
                                      "VALUES (@Serie, @ItemCode, @ItemName, @SerieMotor, @Marca, @Modelo, @Cilindros, @Color, " &
                                      "@Almacen, @EstatusSerie, @YearMoto, @DiasArmado, @Ageing, @Comentarios, GETDATE(), " &
                                      "@EstadoGeneralChasis, @EstadoPintura, @EstadoAsiento, @CondicionRuedasDelantera, " &
                                      "@CondicionRuedasTrasera, @EstadoGuardafangos, @Motor, @EstadoAceite, @SistemaTransmision, " &
                                      "@SistemaRefrigeracion, @SistemaEscape, @LucesDelanteras, @LucesTraseras, " &
                                      "@LucesDireccionales, @LucesFrenos, @Bateria, @ClaxonPito, @EspejosRetrovisores, " &
                                      "@Frenos, @Pedales, @FuncionamientoMotor, @FuncionamientoTransmision, @FuncionamientoFrenos, " &
                                      "@EstabilidadSuspension, @EstadoFinalVehiculo)"

                Using cmd As New SqlCommand(query, con)
                    ' Parámetros principales con verificación de clave
                    cmd.Parameters.AddWithValue("@Serie", If(data.ContainsKey("serie"), data("serie").ToString(), ""))
                    cmd.Parameters.AddWithValue("@ItemCode", If(data.ContainsKey("itemcode"), data("itemcode").ToString(), ""))
                    cmd.Parameters.AddWithValue("@ItemName", If(data.ContainsKey("itemname"), data("itemname").ToString(), ""))
                    cmd.Parameters.AddWithValue("@SerieMotor", If(data.ContainsKey("seriemotor"), data("seriemotor").ToString(), ""))
                    cmd.Parameters.AddWithValue("@Marca", If(data.ContainsKey("marca"), data("marca").ToString(), ""))
                    cmd.Parameters.AddWithValue("@Modelo", If(data.ContainsKey("modelo"), data("modelo").ToString(), ""))
                    cmd.Parameters.AddWithValue("@Cilindros", If(data.ContainsKey("cilindros"), data("cilindros").ToString(), ""))
                    cmd.Parameters.AddWithValue("@Color", If(data.ContainsKey("color"), data("color").ToString(), ""))
                    cmd.Parameters.AddWithValue("@Almacen", If(data.ContainsKey("almacen"), data("almacen").ToString(), ""))
                    cmd.Parameters.AddWithValue("@EstatusSerie", If(data.ContainsKey("estatus_serie"), data("estatus_serie").ToString(), ""))
                    cmd.Parameters.AddWithValue("@YearMoto", If(data.ContainsKey("yearmoto"), data("yearmoto").ToString(), ""))
                    cmd.Parameters.AddWithValue("@DiasArmado", If(data.ContainsKey("dias_armado"), Convert.ToInt32(data("dias_armado")), 0))
                    cmd.Parameters.AddWithValue("@Ageing", If(data.ContainsKey("ageing"), Convert.ToInt32(data("ageing")), 0))
                    cmd.Parameters.AddWithValue("@Comentarios", If(data.ContainsKey("comentarios"), data("comentarios").ToString(), ""))

                    ' Verificación de subobjeto "data" del JSON con los nombres originales
                    If data.ContainsKey("data") Then
                        Dim jsonData As Dictionary(Of String, Object) = DirectCast(data("data"), Dictionary(Of String, Object))
                        cmd.Parameters.AddWithValue("@EstadoGeneralChasis", If(jsonData.ContainsKey("Estado General del Chasis"), jsonData("Estado General del Chasis").ToString(), ""))
                        cmd.Parameters.AddWithValue("@EstadoPintura", If(jsonData.ContainsKey("Estado de La Pintura"), jsonData("Estado de La Pintura").ToString(), ""))
                        cmd.Parameters.AddWithValue("@EstadoAsiento", If(jsonData.ContainsKey("Estado del Asiento"), jsonData("Estado del Asiento").ToString(), ""))
                        cmd.Parameters.AddWithValue("@CondicionRuedasDelantera", If(jsonData.ContainsKey("Condición de las Ruedas Delantera"), jsonData("Condición de las Ruedas Delantera").ToString(), ""))
                        cmd.Parameters.AddWithValue("@CondicionRuedasTrasera", If(jsonData.ContainsKey("Condición de las Ruedas Trasera"), jsonData("Condición de las Ruedas Trasera").ToString(), ""))
                        cmd.Parameters.AddWithValue("@EstadoGuardafangos", If(jsonData.ContainsKey("Estado de los Guardafangos"), jsonData("Estado de los Guardafangos").ToString(), ""))
                        cmd.Parameters.AddWithValue("@Motor", If(jsonData.ContainsKey("Motor"), jsonData("Motor").ToString(), ""))
                        cmd.Parameters.AddWithValue("@EstadoAceite", If(jsonData.ContainsKey("Estado del Aceite"), jsonData("Estado del Aceite").ToString(), ""))
                        cmd.Parameters.AddWithValue("@SistemaTransmision", If(jsonData.ContainsKey("Sistema de Transmisión"), jsonData("Sistema de Transmisión").ToString(), ""))
                        cmd.Parameters.AddWithValue("@SistemaRefrigeracion", If(jsonData.ContainsKey("Sistema Refrigeracion"), jsonData("Sistema Refrigeracion").ToString(), ""))
                        cmd.Parameters.AddWithValue("@SistemaEscape", If(jsonData.ContainsKey("Sistema de Escape"), jsonData("Sistema de Escape").ToString(), ""))
                        cmd.Parameters.AddWithValue("@LucesDelanteras", If(jsonData.ContainsKey("Luces Delanteras"), jsonData("Luces Delanteras").ToString(), ""))
                        cmd.Parameters.AddWithValue("@LucesTraseras", If(jsonData.ContainsKey("Luces Traseras"), jsonData("Luces Traseras").ToString(), ""))
                        cmd.Parameters.AddWithValue("@LucesDireccionales", If(jsonData.ContainsKey("Luces Direccionales"), jsonData("Luces Direccionales").ToString(), ""))
                        cmd.Parameters.AddWithValue("@LucesFrenos", If(jsonData.ContainsKey("Luces de Frenos"), jsonData("Luces de Frenos").ToString(), ""))
                        cmd.Parameters.AddWithValue("@Bateria", If(jsonData.ContainsKey("Bateria"), jsonData("Bateria").ToString(), ""))
                        cmd.Parameters.AddWithValue("@ClaxonPito", If(jsonData.ContainsKey("Claxon / Pito"), jsonData("Claxon / Pito").ToString(), ""))
                        cmd.Parameters.AddWithValue("@EspejosRetrovisores", If(jsonData.ContainsKey("Espejos Retrovisores"), jsonData("Espejos Retrovisores").ToString(), ""))
                        cmd.Parameters.AddWithValue("@Frenos", If(jsonData.ContainsKey("Frenos"), jsonData("Frenos").ToString(), ""))
                        cmd.Parameters.AddWithValue("@Pedales", If(jsonData.ContainsKey("Pedales"), jsonData("Pedales").ToString(), ""))
                        cmd.Parameters.AddWithValue("@FuncionamientoMotor", If(jsonData.ContainsKey("Funcionamiento Motor"), jsonData("Funcionamiento Motor").ToString(), ""))
                        cmd.Parameters.AddWithValue("@FuncionamientoTransmision", If(jsonData.ContainsKey("Funcionamiento Transmision"), jsonData("Funcionamiento Transmision").ToString(), ""))
                        cmd.Parameters.AddWithValue("@FuncionamientoFrenos", If(jsonData.ContainsKey("Funcionamiento Frenos"), jsonData("Funcionamiento Frenos").ToString(), ""))
                        cmd.Parameters.AddWithValue("@EstabilidadSuspension", If(jsonData.ContainsKey("Estabilidad y Suspension"), jsonData("Estabilidad y Suspension").ToString(), ""))
                        cmd.Parameters.AddWithValue("@EstadoFinalVehiculo", If(jsonData.ContainsKey("Estado Final Vehiculo"), jsonData("Estado Final Vehiculo").ToString(), ""))
                    End If

                    ' Ejecutar el comando de inserción
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using

            ' Responder con éxito
            context.Response.StatusCode = 200
            context.Response.Write("{""status"":""success""}")

        Catch ex As KeyNotFoundException
            ' Responder con mensaje de error específico para clave faltante
            context.Response.StatusCode = 500
            context.Response.Write("{""status"":""error"", ""message"":""Clave faltante en el diccionario: " & ex.Message & """}")
        Catch ex As Exception
            ' Responder con mensaje de error general
            context.Response.StatusCode = 500
            context.Response.Write("{""status"":""error"", ""message"":""" & ex.Message & """}")
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class

 
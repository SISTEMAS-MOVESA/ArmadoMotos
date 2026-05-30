Imports System.Data
Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Net.Mail

Partial Class Test
    Inherits System.Web.UI.Page

    Public Shared sCon1 As String = "server=192.168.1.3;database=ARMADOMOTOS;uid=sa;password=M*l!n3r0s2k12"

    Private Sub Test_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub btnCargarSugerido_Click(sender As Object, e As EventArgs) Handles btnCargarSugerido.Click
        Try
            Dim server As New SmtpClient
            Dim mensaje As New MailMessage
            server.Host = "mail.grupomovesa.com"
            server.Port = "587"
            server.Credentials = New System.Net.NetworkCredential("notificaciones@grupomovesa.com", "VYgd}T!XcVy}")
            mensaje.From = New MailAddress("notificaciones@grupomovesa.com")

            mensaje.To.Add("rjovel@grupomovesa.com")
            mensaje.Subject = "MOVESA - PORTAL CREACION Produccion de Motos"
            mensaje.Body = "Se Ha Creado un Nuevo Sugerido de Motos" &
                            "<BR /> " &
                            "PORTAL CREACION CLIENTES By RJ"

            mensaje.IsBodyHtml = True
            mensaje.Priority = MailPriority.High
            server.Send(mensaje)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
End Class

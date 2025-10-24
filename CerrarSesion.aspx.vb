
Imports CrystalDecisions.[Shared].Json

Partial Class CerrarSesion
    Inherits System.Web.UI.Page

    Private Sub CerrarSesion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Session("Name") = vbNullString ' Limpia la variable de sesión "Name"
            Session.Clear()            ' Limpia todos los valores de sesión
            Session.Abandon()          ' Destruye la sesión por completo
            Response.Redirect("default.aspx") ' Redirecciona al login o página de inicio
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class


Partial Class Menu
    Inherits System.Web.UI.Page

    Protected Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
    End Sub
    
    Protected Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        Response.Redirect("MenuDacion.aspx")
    End Sub
    
    Protected Sub ImageButton3_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton3.Click
        Response.Redirect("ListadoMotos.aspx")
    End Sub
    
    Protected Sub ImageButton4_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton4.Click
        Response.Redirect("Task.aspx")
    End Sub

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles Me.Load
        'GESTORC
        If Session("Position").ToString = "GESTORC" Then
            pnlSubMenuOrdenRecuperacion.Visible = False
            pnlSubMenuConfirmacionVenta.Visible = False
            pnlSubMenuPrecios.Visible = False
            pnlSubMenuReconciliaciones.Visible = False
        End If
        'SKA1001
        If Session("Position").ToString = "ADMIN" Then
            pnlSubMenuConfirmacion.Visible = False
            pnlSubMenuConfirmacionVenta.Visible = False
            pnlSubMenuPrecios.Visible = False
            pnlSubMenuReconciliaciones.Visible = False
        End If
        'VENDEDOR
        If Session("Position").ToString = "VENDEDOR" Then
            pnlSubMenuConfirmacion.Visible = False
            pnlSubMenuOrdenRecuperacion.Visible = False
            pnlSubMenuConfirmacionVenta.Visible = True
            pnlSubMenuPrecios.Visible = False
            pnlSubMenuReconciliaciones.Visible = False
        End If
        lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
    End Sub
End Class

Imports System.Data
Imports System.Data.SqlClient
Partial Class DashboardAuditoria
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Private Sub DashboardAuditoria_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else

                End If
            End If
        Catch ex As Exception
            Response.Write("DashboardAuditoria " & ex.Message)
        End Try
    End Sub
End Class

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Partial Class TrasladoTareaCargaCamion
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Sub SeriesPreparadas()
        Try
            If Not Me.IsPostBack Then
                Dim constr As String = sCon2 'ConfigurationManager.ConnectionStrings("constr").ConnectionString
                Using con As SqlConnection = New SqlConnection(constr)
                    Using cmd As SqlCommand = New SqlCommand("SELECT [HEADERID],[SERIE] FROM [ArmadoMotos].[dbo].[SOLCITUD_LINES] WHERE ESTADO='PROCESADO'")
                        cmd.CommandType = CommandType.Text
                        cmd.Connection = con
                        con.Open()
                        lstSeriesPreparadas.DataSource = cmd.ExecuteReader()
                        lstSeriesPreparadas.DataTextField = "SERIE"
                        lstSeriesPreparadas.DataValueField = "HEADERID"
                        lstSeriesPreparadas.DataBind()
                        con.Close()
                    End Using
                End Using

            End If
        Catch ex As Exception
            Response.Write("TrabajosAdicionales " & ex.Message)
        End Try
    End Sub

    Private Sub TrasladoTareaCargaCamion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    SeriesPreparadas()
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub
End Class

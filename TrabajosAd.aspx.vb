Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Partial Class ModelosMotos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"


    Private Sub ModelosMotos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGrid()
                    Select Case Session("Position").ToString()
                        Case "Iniciador"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Supervisor Armado"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Calidad"
                            Response.Redirect("MainDashBoard.aspx")
                    End Select
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub


    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Modificar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                pnlUpdate.Visible = True
                pnlGridUsuarios.Visible = False
                txtUpdateIDTA.Text = GridView1.Rows(index).Cells(1).Text
                txtUpdateDescripcionTA.Text = GridView1.Rows(index).Cells(2).Text
                txtUpdatePrecioTA.Text = GridView1.Rows(index).Cells(3).Text
                'txtUpdateUSerEmail.Text = GridView1.Rows(index).Cells(4).Text
                'End Select
            End If
        Catch ex As Exception
            ' ADD_LOG("GridView1_RowCommand", ex.Message)
        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[TRABAJO],[PRECIO],[STATUS]FROM [dbo].[TRABAJOS] with(nolock)")
                    'Using cmd As New SqlCommand("SELECT [ID], [MODELCODE], [MODELORDER], [MODELNAME], [MODELESTATUS] FROM [ArmadoMotos].[DBO].[MODELOS] with(nolock)")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            'ADD_LOG("CargarTareasPendientes BindGrid ", ex.Message)
            Response.Write("CargarTareasPendientes BindGrid " & ex.Message)
        End Try
    End Sub
    Protected Sub btnIngreso_Click(sender As Object, e As EventArgs) Handles btnIngreso.Click
        pnlGridUsuarios.Visible = False
        pnlCrearUsuario.Visible = True
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("TrabajosAD.aspx")

    End Sub
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("TrabajosAD.aspx")
    End Sub
End Class

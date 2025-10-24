Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Partial Class TrasladosForklift
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
    Private TimerEnabled As Boolean = False

    Private Sub BindGridForklift()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[MODELO],[DESCRIPCION],[QTYSOLICITADA] " &
                                            " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] WHERE FORKLIFT='PENDIENTE'")

                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridForklift.DataSource = dt
                            GridForklift.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GridForklift.UseAccessibleHeader = True
            GridForklift.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub ButtonStartPause_Click(sender As Object, e As EventArgs)
        TimerEnabled = Not TimerEnabled
        If TimerEnabled Then
            RefreshTimer.Interval = Convert.ToInt32(ComboBoxInterval.SelectedValue)
            RefreshTimer.Enabled = True
            ButtonStartPause.Text = "Pausar"
        Else
            RefreshTimer.Enabled = False
            ButtonStartPause.Text = "Iniciar"
        End If
    End Sub

    Protected Sub RefreshTimer_Tick(sender As Object, e As EventArgs)
        BindGridForklift()

    End Sub
    Private Sub GridForklift_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridForklift.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridForklift.Rows(index)
                ActualizarForklift("ARMADO", Date.Now, GridForklift.Rows(index).Cells(0).Text)
                BindGridForklift()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ActualizarForklift(ESTATUS As String, FDATE As DateTime, ID As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[SOLCITUD_HEADER] set [FORKLIFT]=@p1,[FORKLIFTDATE]=@p2 where [ID]=@p3"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ESTATUS)
                cmd.Parameters.AddWithValue("@p2", FDATE)
                cmd.Parameters.AddWithValue("@p3", ID)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Moto Eliminada con Exito!<hr> EliminarLinea');", True)
            End Using
        Catch ex As Exception
            Response.Write("EliminarLinea " & ex.Message)
        End Try
    End Sub

    Private Sub TrasladosForklift_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGridForklift()
                    RefreshTimer.Enabled = False
                    TimerEnabled = False
                End If
            End If
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Ocurrio Un Error, Revise Consola!');", True)
            'Response.Write("<script>console.log('" & ex.Message & "')</script>")
            'Response.Write("MainDashBoard_Load " & ex.Message)
        End Try
    End Sub
End Class

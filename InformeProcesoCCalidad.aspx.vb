Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Partial Class InformeProcesoCCalidad
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[SERIE],[MODELO],[COLOR],[DATECREATED],[ESTATUS],(select [MECANICONAME] from [dbo].[MECANICOS] with(nolock) where id=MECANICOASIGNADO) [MECANICOASIGNADO],[FIRSTUPDATEDATE] " &
                                            " ,[FECHACC1] ,SECONDUPDATEDATE" &
                                            ", (select USERNAME from USUARIOS with(nolock) where USERCODE=ArmadoMotos.CCALIDAD1USER) [Usuario] " &
                                            " From [dbo].[ARMADOMOTOS] with(nolock) where ESTATUS<>'ERROR' and CANCELED='N' and LIQUIDACIONID=0 ")
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
            Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub

    Private Sub InformeProcesoCCalidad_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGrid()
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class

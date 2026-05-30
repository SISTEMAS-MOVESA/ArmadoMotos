Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Class TrasladosCrearSolicitudTS
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
    Private Sub TrasladosCrearSolicitudTS_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGridSolicitudesSeries(Request.QueryString("idcamion"))
                End If
            End If
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Ocurrio Un Error, Revise Consola!');", True)
            'Response.Write("<script>console.log('" & ex.Message & "')</script>")
            'Response.Write("MainDashBoard_Load " & ex.Message)
        End Try
    End Sub
    Private Sub BindGridSolicitudesSeries(camion As String)
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT distinct  [WHSCODE],[WHSNAME],[CODIGOCLIENTE] " &
                                            " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] where [ESTATUS]='PROCESADO' and [CAMION]=" & camion & "")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPresolicitudes.DataSource = dt
                            gridPresolicitudes.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridPresolicitudes.UseAccessibleHeader = True
            gridPresolicitudes.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGridSolicitudesSeries " & ex.Message)
        End Try
    End Sub
    Private Sub BindGridSolicitudesSeriesDetalle(camion As String, whscode_origen As String, whscode_destino As String)
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT distinct  [WHSCODE],[WHSNAME],[CODIGOCLIENTE] " &
                                            " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] where [ESTATUS]='PROCESADO' and [CAMION]=" & camion & "")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPresolicitudes.DataSource = dt
                            gridPresolicitudes.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridPresolicitudes.UseAccessibleHeader = True
            gridPresolicitudes.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGridSolicitudesSeries " & ex.Message)
        End Try
    End Sub
End Class

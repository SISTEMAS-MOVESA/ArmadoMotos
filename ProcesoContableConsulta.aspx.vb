
Imports System.Data.SqlClient
Imports System.Data

Partial Class ProcesoContableConsulta
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
    Public Sub _CargarContratistas()
        Try
            Dim dt As New DataTable()
            Using conn As New SqlConnection(sCon1)
                Dim query As String = "SELECT [GRUPORESPONSABLE] ,[BPCODE] FROM [ArmadoMotos].[dbo].[GRUPOS]"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpContratista.Dispose()
                drpContratista.DataTextField = "GRUPORESPONSABLE"
                drpContratista.DataValueField = "BPCODE"
                drpContratista.DataSource = dt
                drpContratista.DataBind()
                conn.Close()
            End Using
        Catch ex As Exception
            'ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub ProcesoContableConsulta_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    _CargarContratistas()
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
    Private Sub BindGrid(_BPCODE As String, desde As Date, hasta As Date)
        Try
            GridView1.DataSource = Nothing
            GridView1.Dispose()
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [LIQUIDACIONID],[IDCALIDAD],[SERIE],[MODELO],[COLOR],[MECANICOID],[CPROVEEDOR],[PRECIOARMADO],convert(char,FECHACREACION,103)[FECHACREACION] " &
                                            " FROM [ArmadoMotos].[dbo].[MOTOSLIQ] WHERE [CPROVEEDOR]='" & _BPCODE & "' AND [FECHACREACION] BETWEEN '" & desde & "' AND '" & hasta & "'")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            GridView1.UseAccessibleHeader = True
                            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
        Catch ex As Exception
            'Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        BindGrid(drpContratista.SelectedValue.ToString, txtDesde.Text, txtHasta.Text)
    End Sub
End Class

Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web

Partial Class InformeMotosDisponibles
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[ITEMCODE],[ITENMANE],[SERIE],[MODELO],[COLOR],[DATECREATED],[ESTATUS] " &
                                            " , (select [MECANICONAME] from [dbo].[MECANICOS] with(nolock) where id=MECANICOASIGNADO) [MECANICOASIGNADO] " &
                                            " ,[DATECREATED],[FIRSTUPDATEDATE],[SECONDUPDATEDATE],[INICIOCC],[FINCC]," &
                                            " rtrim(SUBSTRING(OBSERVACIONES1, CHARINDEX(':', OBSERVACIONES1)+2,100))[OBSERVACIONES1]" &
                                            " ,(select USERNAME from USUARIOS with(nolock) where USERCODE=ArmadoMotos.CCALIDAD1USER) [Usuario],PINTOR " &
                                            " From [dbo].[ARMADOMOTOS] with(nolock) where ESTATUS<>'ERROR' and CANCELED='N'" &
                                            " and datepart(year,SECONDUPDATEDATE)=datepart(year,getdate()) and datepart(month,SECONDUPDATEDATE)=datepart(month,getdate())")
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

    Private Sub InformeMotosDisponibles_Load(sender As Object, e As EventArgs) Handles Me.Load
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
    Private Sub ExportGridToExcel()
        Response.Clear()
        Response.Buffer = True
        Response.ClearContent()
        Response.ClearHeaders()
        Response.Charset = ""
        Dim FileName As String = "MotosDisponibles " & DateTime.Now & ".xls"
        Dim strwritter As StringWriter = New StringWriter()
        Dim htmltextwrtter As HtmlTextWriter = New HtmlTextWriter(strwritter)
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & FileName)
        GridView1.GridLines = GridLines.Both
        GridView1.HeaderStyle.Font.Bold = True
        GridView1.RenderControl(htmltextwrtter)
        Response.Write(strwritter.ToString())
        Response.[End]()
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Expediente" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                Session("SerieExpediente") = GridView1.Rows(index).Cells(4).Text
                Response.Redirect("ExpedienteVehiculo.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            BindGridByDate(txtDesde.Text, txtHasta.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindGridByDate(desde As DateTime, hasta As DateTime)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[ITEMCODE],[ITENMANE],[SERIE],[MODELO],[COLOR],[DATECREATED],[ESTATUS], " &
                                            "(SELECT [MECANICONAME] FROM [dbo].[MECANICOS] WITH(NOLOCK) WHERE ID = MECANICOASIGNADO) AS [MECANICOASIGNADO], " &
                                            "[DATECREATED],[FIRSTUPDATEDATE],[SECONDUPDATEDATE],[INICIOCC],[FINCC] , " &
                                            "(SELECT USERNAME FROM USUARIOS WITH(NOLOCK) WHERE USERCODE = ArmadoMotos.CCALIDAD1USER) AS [Usuario],PINTOR, " &
                                             " rtrim(SUBSTRING(OBSERVACIONES1, CHARINDEX(':', OBSERVACIONES1)+2,100))[OBSERVACIONES1] " &
                                             "FROM [dbo].[ARMADOMOTOS] WITH(NOLOCK) " &
                                            "WHERE ESTATUS <> 'ERROR' AND CANCELED = 'N' " &
                                            "AND SECONDUPDATEDATE BETWEEN @d1 AND @d2", con)
                    ' Agregar los parámetros a la consulta
                    cmd.Parameters.AddWithValue("@d1", desde)
                    cmd.Parameters.AddWithValue("@d2", hasta)

                    Using sda As New SqlDataAdapter(cmd)
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using

            ' Configurar el GridView
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGridByDate " & ex.Message)
        End Try
    End Sub
End Class

Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports ClosedXML.Excel

Partial Class InformeEnProcesoCalidad
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)

                ' Leer el contenido del archivo .sql
                Dim SQL_string As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/InformeCC_BindGrid_En_Proceso_Calidad.sql"))

                Using cmd As New SqlCommand(SQL_string, con)

                    Using sda As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataSource = dt
                        GridView1.DataBind()
                    End Using
                End Using
            End Using
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub
    Private Sub InformeCC_Load(sender As Object, e As EventArgs) Handles Me.Load
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
        Dim FileName As String = "ControlCalidad " & DateTime.Now & ".xls"
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
    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Select Case e.Row.Cells(4).Text
                    Case "Calidad"
                        e.Row.Cells(4).Enabled = False
                        e.Row.Cells(4).BackColor = System.Drawing.Color.Red
                    Case "Calidad Procesando"
                        e.Row.Cells(4).BackColor = System.Drawing.Color.Orange
                        e.Row.Cells(4).ForeColor = System.Drawing.Color.White
                    Case "Armada"
                        e.Row.Cells(4).BackColor = System.Drawing.Color.Green
                        e.Row.Cells(4).ForeColor = System.Drawing.Color.White
                End Select

            End If
        Catch ex As Exception
            Response.Write("GridView1_RowDataBound " & ex.Message)
        End Try
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Iniciar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                UpdateInicioCalidad(Date.Now, GridView1.Rows(index).Cells(1).Text)
            End If
            If e.CommandName = "Terminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                Session("SerieCalidad") = GridView1.Rows(index).Cells(1).Text
                Response.Redirect("ListadoCCalidad.aspx")
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub UpdateInicioCalidad(_fechaInicioControlcalidad As Date, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " Set [INICIOCC] = @p1 " &
                    " ,[ESTATUS] = 'Calidad Procesando'" &
                    " WHERE [SERIE] = @p2"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _fechaInicioControlcalidad)
                cmd.Parameters.AddWithValue("@p2", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("UpdateInicioCalidad " & ex.Message)
        End Try
    End Sub

    Protected Sub ExportExcel()
        Dim constr As String = sCon2
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("Select t0.[ID], t0.[SERIE], t0.[MODELO], t0.[COLOR], t0.[DATECREATED], t0.[ESTATUS] " &
                                            " , (select [MECANICONAME] from [dbo].[MECANICOS] with(nolock) where id=t0.MECANICOASIGNADO) [MECANICOASIGNADO],   " &
                                            " t0.[FIRSTUPDATEDATE], t0.[FECHACC1], t0.SECONDUPDATEDATE    " &
                                            " , (Select USERNAME from USUARIOS With(nolock) where USERCODE=t0.CCALIDAD1USER) [Usuario]    " &
                                            " , (Select top 1 whscode from movesa..osri With(nolock) where ItemCode=t0.ITEMCODE collate Modern_Spanish_CI_AS " &
                                            " and SuppSerial=t0.[SERIE]  collate Modern_Spanish_CI_AS order by id desc)[Almacen]   " &
                                            " , (Select top 1 Case Status When '1' Then 'NO DISPONIBLE' ELSE 'DISPONIBLE'  END from movesa..osri with(nolock)  " &
                                            " where ItemCode=t0.ITEMCODE collate Modern_Spanish_CI_AS and SuppSerial = t0.[SERIE] collate Modern_Spanish_CI_AS order by id desc)[Estado] " &
                                            " ,DATEDIFF(DAY,SECONDUPDATEDATE,GETDATE())[DIAS]" &
                                            " From [dbo].[ARMADOMOTOS] t0 With(nolock) where ESTATUS in ('Calidad','Calidad Procesando') and CANCELED='N' and LIQUIDACIONID=0 ")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        Using wb As New XLWorkbook()
                            wb.Worksheets.Add(dt, "Produccion")

                            Response.Clear()
                            Response.Buffer = True
                            Response.Charset = ""
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            Response.AddHeader("content-disposition", "attachment;filename=Produccion.xlsx")
                            Using MyMemoryStream As New MemoryStream()
                                wb.SaveAs(MyMemoryStream)
                                MyMemoryStream.WriteTo(Response.OutputStream)
                                Response.Flush()
                                Response.End()
                                con.Close()
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

End Class

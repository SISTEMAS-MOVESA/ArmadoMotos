Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports ClosedXML.Excel
Partial Class InformeCCFinArmado
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Public Sub KardexVehiculo(DATELOG As Date, SERIE As String, MARCA As String, MODELO As String,
                              COLOR As String, ACCION As String, COMENTARIOS As String, USUARIO As String, MECANICO As String, CALIDAD As String)
        Try

            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[KARDEXVEHICULO] ([DATELOG],[SERIE],[MARCA],[MODELO],[COLOR],[ACCION],[COMENTARIOS],[USUARIO],[MECANICO],[CALIDAD],[MECANICOASIGNADO],[AUDITORASIGNADO]) " &
                    " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,0,0,@p9,@p10)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", DATELOG)
                cmd.Parameters.AddWithValue("@p2", SERIE)
                cmd.Parameters.AddWithValue("@p3", MARCA)
                cmd.Parameters.AddWithValue("@p4", MODELO)
                cmd.Parameters.AddWithValue("@p5", COLOR)
                cmd.Parameters.AddWithValue("@p6", ACCION)
                cmd.Parameters.AddWithValue("@p7", COMENTARIOS)
                cmd.Parameters.AddWithValue("@p8", USUARIO)
                cmd.Parameters.AddWithValue("@p9", MECANICO)
                cmd.Parameters.AddWithValue("@p10", CALIDAD)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("KardexVehiculo " & ex.Message)
        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)

                ' Leer el contenido del archivo .sql
                Dim SQL_string As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/InformeCC_BindGrid_Pendiente_Control.sql"))

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

    Private Sub InformeCCFinArmado_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGrid()
                    _CargarControlCalidad()
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub _CargarControlCalidad()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon2)
                Dim query As String = "	SELECT [ID],[USERNAME] FROM [ArmadoMotos].[dbo].[USUARIOS] with(nolock) where USERROL='CALIDAD'"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpCalidadUsers.Dispose()
                drpCalidadUsers.DataTextField = "USERNAME"
                drpCalidadUsers.DataValueField = "ID"
                drpCalidadUsers.DataSource = dt
                drpCalidadUsers.DataBind()
                conn.Close()
            End Using
        Catch ex As Exception
            'ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write("_CargarControlCalidad " & ex.Message)
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
                Select Case e.Row.Cells(7).Text
                    Case "Reproceso"
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Red
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                        e.Row.Cells(0).Enabled = False
                    Case "Asignada"
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Coral
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                        e.Row.Cells(0).Enabled = False
                    Case "Proceso"
                        e.Row.Cells(0).Enabled = False
                        e.Row.Cells(9).Enabled = False
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Gray
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                    Case "Calidad"
                        e.Row.Cells(9).Enabled = False
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Red
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                    Case "Calidad Procesando"
                        e.Row.Cells(0).Enabled = False
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Orange
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                    Case "Armada"
                        e.Row.Cells(0).Enabled = False
                        e.Row.Cells(9).Enabled = False
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Green
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                        e.Row.Cells(0).Enabled = False
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


                txtFlyMiniSerial.Text = GridView1.Rows(index).Cells(2).Text
                txtFlyMiniModelo.Text = GridView1.Rows(index).Cells(3).Text
                txtFlyMiniColor.Text = GridView1.Rows(index).Cells(4).Text
                txtFlyMiniMecanico.Text = Page.Server.HtmlDecode(GridView1.Rows(index).Cells(8).Text)
                Dim Flyscript As String = "<script>$('#IniciarPintura').flyout('toggle');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", Flyscript, False)


                Dim script As String = "iziToast.warning({" &
                                       "title: 'Adevrtencia'," &
                                       "message: 'Seleccione Pintor'," &
                                       "position: 'topRight'," &
                                       "timeout: 5000" &
                                       "});"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowToast", script, True)

            End If

        Catch ex As Exception
            Response.Write("GridView1_RowCommand " & ex.Message)
        End Try
    End Sub
    Public Function GetMake(_serie As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "select t1.Name from	oitm t0 with(nolock) inner join [@AMARCA] t1 with(nolock) " &
        " on t0.u_amarca=t1.code where t0.ItemCode=(select ItemCode from osrn with(nolock) where osrn.mnfserial=@p1)"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", _serie)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetMake: " & t & "');</script>")
        End Using
    End Function
    Public Sub UpdateInicioCalidad(_fechaInicioControlcalidad As Date, _Serie As String, AUDITORCCASIGNADO As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " Set [INICIOCC] = @p1 " &
                    " ,[ESTATUS] = 'Calidad Procesando'" &
                    " ,[AUDITORCCASIGNADO] = @p3 " &
                    " WHERE [SERIE] = @p2 "

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _fechaInicioControlcalidad)
                cmd.Parameters.AddWithValue("@p2", _Serie)
                cmd.Parameters.AddWithValue("@p3", AUDITORCCASIGNADO)
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
                                            " From [dbo].[ARMADOMOTOS] t0 With(nolock) where ESTATUS<>'ERROR' and CANCELED='N' and LIQUIDACIONID=0 ")
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

    Protected Sub btnInicioCCalidad_Click(sender As Object, e As EventArgs) Handles btnInicioCCalidad.Click
        Try
            UpdateInicioCalidad(Date.Now, txtFlyMiniSerial.Text, drpCalidadUsers.SelectedItem.Text)

            KardexVehiculo(Date.Now,
                           txtFlyMiniSerial.Text,
                           GetMake(txtFlyMiniSerial.Text),
                           txtFlyMiniModelo.Text,
                           txtFlyMiniColor.Text,
                           "Inicio Control Calidad",
                           "Inicio Control Calidad",
                           Session("UserCode").ToString,
                           txtFlyMiniMecanico.Text,
                           drpCalidadUsers.SelectedItem.Text)

            BindGrid()
        Catch ex As Exception
            Response.Write("btnInicioCCalidad_Click " & ex.Message)
        End Try
    End Sub
    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
        Dim NewStr As String
        sName = Replace(sName, "/", sChr)
        sName = Replace(sName, "\", sChr)
        sName = Replace(sName, ":", sChr)
        sName = Replace(sName, "?", sChr)
        sName = Replace(sName, Chr(34), sChr)
        sName = Replace(sName, "<", sChr)
        sName = Replace(sName, ">", sChr)
        sName = Replace(sName, "|", sChr)
        sName = Replace(sName, "&", sChr)
        sName = Replace(sName, "%", sChr)
        sName = Replace(sName, "*", sChr)
        sName = Replace(sName, "'", sChr)
        sName = Replace(sName, "{", sChr)
        sName = Replace(sName, "[", sChr)
        sName = Replace(sName, "]", sChr)
        sName = Replace(sName, "}", sChr)
        sName = Replace(sName, "!", sChr)
        sName = Replace(sName, ",", sChr)
        NewStr = sName
        Return NewStr
    End Function

    Private Sub btnCancelarInicio_Click(sender As Object, e As EventArgs) Handles btnCancelarInicio.Click
        Try
            BindGrid()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class

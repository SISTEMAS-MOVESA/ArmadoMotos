Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports DocumentFormat.OpenXml.Spreadsheet
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Net.Mail
Partial Class TrasladosConfirmLogistica
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String
    Public SQL_STRING As String

    Private totalEspacios As Decimal = 0
    Private totalQtylog As Decimal = 0
    Private totalQtySucursal As Decimal = 0
    Public flag_reload As Boolean = False

    Private Sub TrasladosConfirmLogistica_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else

                    Response.Write("<script>console.log('VisorLogistica_Load: " & ReplaceCharsForFileName("Mscroid" & Request.QueryString("id"), " ") & "');</script>")
                    Response.Write("<script>console.log('VisorLogistica_Load: " & ReplaceCharsForFileName("Almacen" & Request.QueryString("destino"), " ") & "');</script>")
                    BindgridSugerido(Request.QueryString("id"), Request.QueryString("destino"))
                    lblIdMacro.Text = Request.QueryString("id")
                    lblAlmacenDestino.Text = GetWhsName(Request.QueryString("destino"))
                    lblRuta.Text = Request.QueryString("ruta")

                    'txtFechaArme.Text = Convert.ToDateTime(GetFechaArmado(Request.QueryString("id"))).ToString("yyyy-MM-dd")
                    'txtFechaEnvio.Text = Convert.ToDateTime(GetFechaDespacho(Request.QueryString("id"))).ToString("yyyy-MM-dd")


                    'BindgridSugeridoHistorial(Request.QueryString("id"))
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('VisorLogistica_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindgridSugerido(macro_id As String, destino As String)
        'Private Sub BindgridSugerido(destino As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/TrasladosConfirmLogistica_BindgridSugerido.sql"))
                Using cmd As New SqlCommand(SQL_STRING)
                    cmd.Parameters.AddWithValue("@macroId", macro_id)
                    cmd.Parameters.AddWithValue("@almDestino", destino)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPedidoTemp.DataSource = dt
                            gridPedidoTemp.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridPedidoTemp.UseAccessibleHeader = True
            gridPedidoTemp.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            btnCrearSolicitud.Visible = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Warning", "iziToast.warning({title: 'Advertencia', message: 'Esta Solicitud ya fue Confirmada!!!', position: 'topRight'});", True)

            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub UpdateMacroInsertHeaderId(headerid As String)
        Try
            Dim query As String = String.Empty
            query &= "update [ArmadoMotos].[dbo].[MACROINSERT_HEADER] set [ESTADO]=3 WHERE ID=@P1"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", headerid)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('UpdateMacroInsertHeaderId: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub UpdateFechasArmadoDespacho(headerid As String, fechaarmado As Date, fechadespacho As Date)
        Try
            Dim query As String = String.Empty
            query &= "update [ArmadoMotos].[dbo].[PRESOLICITUD_HEADER] set [FECHAARMADO] = @p2 ,[FECHAENTREGA] = @p3 where id = @p1"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", headerid)
                        .Parameters.AddWithValue("@p2", fechaarmado)
                        .Parameters.AddWithValue("@p3", fechadespacho)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('UpdateFechasArmadoDespacho: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub gridPedidoTemp_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        Try
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    Dim macroID As Integer = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "MACROID"))
            '    If macroID = 0 Then
            '        flag_reload = True
            '        Dim rowID As Integer = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "LID"))
            '        Dim newMacroID As Integer = Convert.ToInt32(Request.QueryString("id"))

            '        Dim constr As String = sCon2
            '        Using con As New SqlConnection(constr)
            '            Using cmd As New SqlCommand("UPDATE [ArmadoMotos].[dbo].[PRESOLICITUD_LINES] SET MACROID = @NewMacroID WHERE ID = @RowID " &
            '                                        " AND [ALMDESTINO] = @almDestino ", con)
            '                cmd.Parameters.AddWithValue("@NewMacroID", newMacroID)
            '                cmd.Parameters.AddWithValue("@RowID", rowID)
            '                cmd.Parameters.AddWithValue("@almDestino", "T" & Request.QueryString("destino"))
            '                con.Open()
            '                cmd.ExecuteNonQuery()
            '                con.Close()
            '            End Using
            '        End Using
            '    End If
            'End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim espacios As Decimal
                Dim qtyLog As Decimal
                Dim qtySucursal As Decimal

                ' Try to parse values safely to avoid format errors
                If Decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "ESPACIOS").ToString(), espacios) AndAlso
                   Decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "QTYLOG").ToString(), qtyLog) AndAlso
                   Decimal.TryParse(DataBinder.Eval(e.Row.DataItem, "QTYSUC").ToString(), qtySucursal) Then

                    totalEspacios += espacios
                    totalQtylog += qtyLog
                    totalQtySucursal += qtySucursal

                    ' Set background color based on conditions
                    If qtyLog = qtySucursal Then
                        e.Row.Cells(10).BackColor = System.Drawing.Color.LightGreen
                    ElseIf qtyLog > qtySucursal Then
                        e.Row.Cells(10).BackColor = System.Drawing.Color.Coral
                    ElseIf qtyLog < qtySucursal Then
                        e.Row.Cells(4).BackColor = System.Drawing.Color.Yellow
                        e.Row.Cells(10).BackColor = System.Drawing.Color.Yellow
                    End If
                End If
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(8).Text = "Totales"
                e.Row.Cells(9).Text = totalEspacios.ToString("N0")
                e.Row.Cells(10).Text = totalQtylog.ToString("N0")
                e.Row.Cells(11).Text = totalQtySucursal.ToString("N0")

                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(8).Font.Bold = True
                e.Row.Cells(8).Font.Size = FontUnit.Point(16)
                e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(9).Font.Bold = True
                e.Row.Cells(9).Font.Size = FontUnit.Point(16)
                e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(10).Font.Bold = True
                e.Row.Cells(10).Font.Size = FontUnit.Point(16)
                e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(11).Font.Bold = True
                e.Row.Cells(11).Font.Size = FontUnit.Point(16)
            End If

            If flag_reload = True Then
                Dim script As String = "<script type='text/javascript'>window.location.href = window.location.pathname + window.location.search;</script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "ReloadPage", script, False)
            End If

        Catch ex As Exception
            Response.Write("<script>console.log('gridPedidoTemp_RowDataBound: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function GetWhsName(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT whsname from owhs with(nolock) where whscode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
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
    Private Sub gridPedidoTemp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPedidoTemp.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridPedidoTemp.Rows(index)
                EliminarLinea(gridPedidoTemp.Rows(index).Cells(3).Text)
                BindgridSugerido(Request.QueryString("id"), "T" & Request.QueryString("destino"))

            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPedidoTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub EliminarLinea(idlinea As Integer)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "DELETE FROM [PRESOLICITUD_LINES] where [ID] = @p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Moto Eliminada con Exito!<hr> EliminarLinea');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub btnPlus_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnPlus As Button = DirectCast(sender, Button)
        Dim row As GridViewRow = DirectCast(btnPlus.NamingContainer, GridViewRow)
        Dim qtyTextBox As TextBox = DirectCast(row.FindControl("qty"), TextBox)

        ' If qty is null or empty, set to zero
        Dim qty As Integer
        If Not Integer.TryParse(qtyTextBox.Text, qty) Then
            qty = 0
        End If

        qty += 1
        qtyTextBox.Text = qty.ToString()
        'BindgridSugerido(Request.QueryString("id"), "T" & Request.QueryString("destino"))
    End Sub
    Protected Sub btnMinus_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnMinus As Button = DirectCast(sender, Button)
        Dim row As GridViewRow = DirectCast(btnMinus.NamingContainer, GridViewRow)
        Dim qtyTextBox As TextBox = DirectCast(row.FindControl("qty"), TextBox)

        ' If qty is null or empty, set to zero
        Dim qty As Integer
        If Not Integer.TryParse(qtyTextBox.Text, qty) Then
            qty = 0
        End If

        ' Decrement qty if greater than zero
        If qty > 0 Then
            qty -= 1
        End If

        qtyTextBox.Text = qty.ToString()
        'BindgridSugerido(Request.QueryString("id"), "T" & Request.QueryString("destino"))

    End Sub
    Public Sub SOLICITUD_HEADER(WHSCODEORIGEN As String, WHSNAMEORIGEN As String, WHSCODE As String, WHSNAME As String, CODIGOCLIENTE As String,
                                CODEMODELO As String, MODELO As String, QTYSOLICITADA As Integer,
                                QTYPREPARADA As Integer, OBSERVACIONES As String, USERCREATED As String, DATECREATED As DateTime,
                                ESTATUS As String, FECHA_ARMADO As DateTime, FECHA_ENTREGA As DateTime, DESCRIPCION As String,
                                PREIDHEADER As String, PRELIDHEADER As String, ITEMCODE As String,
                                QTYLOG As String, QTYSUC As String, QTYFINAL As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = " INSERT INTO [dbo].[SOLCITUD_HEADER]([WHSCODEORIGEN],[WHSNAMEORIGEN],[WHSCODE],[WHSNAME],[CODIGOCLIENTE]," &
                  " [CODEMODELO],[MODELO],[QTYSOLICITADA],[QTYPREPARADA],[OBSERVACIONES],[USERCREATED],[DATECREATED]," &
                  " [ESTATUS],[FECHAARMADO],[FECHAENTREGA],[DESCRIPCION],[PREIDHEADER],[PRELIDHEADER],[MACROID],[ITEMCODE] " &
                  " ,[QTYLOG],[QTYSUC],[QTYFINAL])" &
                    " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)

                cmd.Parameters.AddWithValue("@p1", WHSCODEORIGEN)
                cmd.Parameters.AddWithValue("@p2", WHSNAMEORIGEN)
                cmd.Parameters.AddWithValue("@p3", WHSCODE)
                cmd.Parameters.AddWithValue("@p4", WHSNAME)
                cmd.Parameters.AddWithValue("@p5", CODIGOCLIENTE)
                cmd.Parameters.AddWithValue("@p6", CODEMODELO)
                cmd.Parameters.AddWithValue("@p7", MODELO)
                cmd.Parameters.AddWithValue("@p8", QTYSOLICITADA)
                cmd.Parameters.AddWithValue("@p9", QTYPREPARADA)
                cmd.Parameters.AddWithValue("@p10", OBSERVACIONES)
                cmd.Parameters.AddWithValue("@p11", USERCREATED)
                cmd.Parameters.AddWithValue("@p12", DATECREATED)
                cmd.Parameters.AddWithValue("@p13", ESTATUS)
                cmd.Parameters.AddWithValue("@p14", FECHA_ARMADO)
                cmd.Parameters.AddWithValue("@p15", FECHA_ENTREGA)
                cmd.Parameters.AddWithValue("@p16", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p17", PREIDHEADER)
                cmd.Parameters.AddWithValue("@p18", PRELIDHEADER)
                cmd.Parameters.AddWithValue("@p19", Request.QueryString("id"))
                cmd.Parameters.AddWithValue("@p20", ITEMCODE)
                cmd.Parameters.AddWithValue("@p21", QTYLOG)
                cmd.Parameters.AddWithValue("@p22", QTYSUC)
                cmd.Parameters.AddWithValue("@p23", QTYFINAL)

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Solicitud Guardada con Exito!<hr> SOLICITUD_HEADER');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('SOLICITUD_HEADER: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub btnCrearSolicitud_Click(sender As Object, e As EventArgs) Handles btnCrearSolicitud.Click
        Try
            If txtFechaArme.Text = "" Or txtFechaEnvio.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Warning", "iziToast.warning({title: 'Advertencia', message: 'Falta Fecha de Armado o Fecha Entrega.', position: 'topRight'});", True)
                Exit Sub
            End If

            For Each row As GridViewRow In gridPedidoTemp.Rows
                Dim qtyTextBox As TextBox = TryCast(row.FindControl("qty"), System.Web.UI.WebControls.TextBox)
                If qtyTextBox IsNot Nothing Then
                    Dim str As String = qtyTextBox.Text
                    If String.IsNullOrEmpty(str) Then
                        qtyTextBox.BorderStyle = BorderStyle.Double
                        qtyTextBox.BorderColor = System.Drawing.Color.Red
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Warning", String.Format("iziToast.warning({{title: 'Advertencia', message: 'La línea {0} no debe estar vacía o tener valor cero.', position: 'topRight'}});", row.RowIndex + 1), True)
                        Exit Sub
                    End If
                End If
            Next

            For Each row As GridViewRow In gridPedidoTemp.Rows
                Dim str As String = TryCast(row.FindControl("qty"), System.Web.UI.WebControls.TextBox).Text
                Response.Write("<script>console.log('===============================');</script>")
                Response.Write("<script>console.log('" & row.Cells(0).Text & "');</script>")
                Response.Write("<script>console.log('" & str & "');</script>")

                If str > 0 Then
                    SOLICITUD_HEADER("DCM00",
                                     "Distribucion Central de Motos",
                                     row.Cells(3).Text,
                                     lblAlmacenDestino.Text,
                                     row.Cells(14).Text,
                                     GetCodigoModelo(row.Cells(7).Text),
                                     row.Cells(7).Text,
                                     str,
                                     0,
                                     row.Cells(4).Text,
                                     Session("UserCode"),
                                     Date.Now,
                                     "SOLICITADO",
                                     txtFechaArme.Text,
                                     txtFechaEnvio.Text,
                                     row.Cells(8).Text,
                                     row.Cells(1).Text,
                                     row.Cells(2).Text,
                                     row.Cells(6).Text,
                                     row.Cells(10).Text,
                                     row.Cells(11).Text,
                                     str)
                End If
                UpdateMacroInsertHeaderId(Request.QueryString("id"))
            Next
            Dim nextcamion As String = GetNextCamion()
            UpdateNextCamion(nextcamion, Session("UserCode"))

            For Each row As GridViewRow In gridPedidoTemp.Rows
                UpdatePreSolicitud(lblIdMacro.Text, nextcamion, Session("UserCode"), row.Cells(2).Text)
            Next

            UpdateFechasArmadoDespacho(lblIdMacro.Text, txtFechaArme.Text, txtFechaEnvio.Text)

            Response.Redirect("VisorLogistica.aspx?id=" & Request.QueryString("id") & "&destino=" & lblAlmacenDestino.Text & "&ruta=" & lblRuta.Text & "")
        Catch ex As Exception
            Response.Write("<script>console.log('SOLICITUD_HEADER: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub UpdateNextCamion(camion As String, usuario As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[SOLCITUD_HEADER] Set CAMION=@p1, [ESTATUS] ='CAMION' where [ESTATUS] = 'SOLICITADO' AND [USERCREATED]=@p2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", camion)
                cmd.Parameters.AddWithValue("@p2", usuario)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('UpdateNextCamion : " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub UpdatePreSolicitud(headerid As String, camion As String, usuario As String, lineheaderid As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "update [dbo].[PRESOLICITUD_HEADER] set ESTATUS='PROCESADO', CAMION=@p2, USERUPDATED=@p3,DATEUPDATED=getdate() where ID=@p1;" &
                "update [dbo].[PRESOLICITUD_LINES] set ESTADO='PROCESADO', USERUPDATED=@p3 where ID=@p4"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", headerid)
                cmd.Parameters.AddWithValue("@p2", camion)
                cmd.Parameters.AddWithValue("@p3", usuario)
                cmd.Parameters.AddWithValue("@p4", lineheaderid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('UpdatePreSolicitud : " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Function GetFechaArmado(macroid As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT FARMADO from MACROINSERT_HEADER where id = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", macroid)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Public Function GetFechaDespacho(macroid As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT FDESPACHO from MACROINSERT_HEADER where id = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", macroid)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Public Function GetNextCamion() As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT ISNULL(MAX([CAMION]),0)+1[Camion] FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER]"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Public Function GetCodigoModelo(modelo As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "select code from movesa..[@AMODELO] where [Name]='" & modelo & "'"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Try
            Response.Redirect("TrasladosDashboard.aspx")
        Catch ex As Exception
            Response.Write("<script>console.log('btnRegresar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub EnviarNotificacionEmail(almdestino As String, headerid As String)
        Try
            Dim server As New SmtpClient
            Using mensaje As New MailMessage
                server.Host = "mail.grupomovesa.com"
                server.Port = "587"
                server.Credentials = New System.Net.NetworkCredential("notificaciones@grupomovesa.com", "VYgd}T!XcVy}")
                mensaje.From = New MailAddress("notificaciones@grupomovesa.com")

                Dim EMAIL As String = GetEmailSuc(Session("ALMTRANSIT").ToString)

                Dim constr As String = sCon2
                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("select t0.ID,t0.ALMDESTINO,(SELECT WHSNAME FROM MOVESA..OWHS WITH(NOLOCK) WHERE WhsCode=t0.ALMDESTINO COLLATE Modern_Spanish_CI_AS)[NAMLACEN], " &
                                                        "t1.RUTA,t1.ARTICULO,t1.MODELO,t1.DESCRIPCION,t1.CANTIDAD,t1.ESPACIOS,'____________'[Ult Digitos] " &
                                                        " from PRESOLICITUD_HEADER t0 with(nolock) inner join PRESOLICITUD_LINES t1 with(nolock) " &
                                                        " on t0.id=t1.headerid " &
                                                        " where t0.[ALMDESTINO]='" & almdestino & "' and t0.id=" & headerid & "")

                        Using sda As New SqlDataAdapter()
                            cmd.Connection = con
                            sda.SelectCommand = cmd
                            Using dt As New DataTable()
                                sda.Fill(dt)
                                'gridCuadroBasico.DataSource = dt
                                'gridCuadroBasico.DataBind()
                                Dim pdfBytes As Byte() = ConvertDataTableToPdf(dt)
                                Dim pdfStream As New MemoryStream(pdfBytes)
                                mensaje.Attachments.Add(New Attachment(pdfStream, Session("ALMTRANSIT").ToString & ".pdf"))
                                con.Close()
                            End Using
                        End Using
                    End Using
                End Using

                mensaje.To.Add("logistica05@grupomovesa.com")
                mensaje.To.Add("nromero@grupomovesa.com")
                mensaje.CC.Add(Session("USEREMAIL").ToString)
                mensaje.CC.Add("pbustillo@grupomovesa.com")
                mensaje.CC.Add("analistainventario@grupomovesa.com")
                mensaje.CC.Add("rjovel@grupomovesa.com")
                mensaje.Subject = "Confirmacion Final de Produccion Logistica " & headerid & " " & almdestino & " MOVESA - Portal Produccion de Motos"
                mensaje.Body = "" & Session("Name").ToString & ", Ha Confirmado el Sugerido de Motos" &
                                    "<BR /> " &
                                    "PORTAL Produccion de Motos By RJ"

                mensaje.IsBodyHtml = True
                mensaje.Priority = MailPriority.High
                server.Send(mensaje)
            End Using

            Response.Write("<script>console.log('EnviarNotificacionEmail Correo Enviado');</script>")

        Catch ex As Exception
            Response.Write("<script>console.log('EnviarNotificacionEmail " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function ConvertDataTableToPdf(dataTable As DataTable) As Byte()
        ' Crear el documento PDF en tamaño carta y formato horizontal
        Dim document As New Document(PageSize.LETTER.Rotate())
        Dim memoryStream As New MemoryStream()
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)
        document.Open()

        ' Crear una tabla en el documento PDF
        Dim table As New PdfPTable(dataTable.Columns.Count)
        table.WidthPercentage = 100

        ' Establecer el tamaño de letra y otros estilos de la celda
        Dim font As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8)
        Dim cellStyle As New PdfPCell()
        cellStyle.HorizontalAlignment = Element.ALIGN_LEFT
        cellStyle.VerticalAlignment = Element.ALIGN_MIDDLE
        cellStyle.Padding = 5

        ' Agregar los encabezados de columna a la tabla
        For i As Integer = 0 To dataTable.Columns.Count - 1
            Dim phrase As New Phrase(dataTable.Columns(i).ColumnName, font)
            Dim headerCell As New PdfPCell(phrase)
            headerCell.BackgroundColor = New BaseColor(230, 230, 230)
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE
            headerCell.Padding = 5
            table.AddCell(headerCell)
        Next

        ' Agregar los datos del DataTable a la tabla
        For i As Integer = 0 To dataTable.Rows.Count - 1
            For j As Integer = 0 To dataTable.Columns.Count - 1
                Dim phrase As New Phrase(dataTable.Rows(i)(j).ToString(), font)
                Dim dataCell As New PdfPCell(phrase)
                dataCell.HorizontalAlignment = Element.ALIGN_LEFT
                dataCell.VerticalAlignment = Element.ALIGN_MIDDLE
                dataCell.Padding = 5
                'dataCell.NoWrap = True ' Evitar el wrap de texto
                table.AddCell(dataCell)
            Next
        Next

        ' Agregar la tabla al documento
        document.Add(table)
        document.Close()

        ' Convertir el documento PDF a un arreglo de bytes
        Dim pdfBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()

        Return pdfBytes
    End Function
    Public Function GetEmailSuc(whscode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT [USEREMAIL] FROM [ArmadoMotos].[dbo].[USUARIOS] where SUCURSAL=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            'Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
End Class

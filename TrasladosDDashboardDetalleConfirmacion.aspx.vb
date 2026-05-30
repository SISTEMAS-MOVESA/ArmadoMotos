Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail

Partial Class TrasladosDDashboardDetalleConfirmacion
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public nuevoDespachoId As Integer = 0
    Private Sub TrasladosDDashboardDetalleConfirmacion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindgridPortalPedidos()
                    BindgridSugerido(Request.QueryString("planid"), Session("User"))

                    'BindGridMotosPortal()
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub
    Private Sub BindgridSugerido(idPlan As String, usuario As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SqlQry As String = "SELECT RUTA,ID,ALMDESTINO,ARTICULO,MODELO,DESCRIPCION,ESPACIOS,CANTIDAD,QTYLOGISTICA, " &
                                        " OBSERVACIONES FROM ArmadoMotos.dbo.MACROINSERT " &
                                        " WHERE [ESTADO]='T' AND PLANID = @planId and USUARIO = @usuario"
                Using cmd As New SqlCommand(SqlQry)
                    cmd.Parameters.AddWithValue("@planId", idPlan)
                    cmd.Parameters.AddWithValue("@usuario", usuario)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPedidoTemp.DataSource = dt
                            gridPedidoTemp.DataBind()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridPedidoTemp.UseAccessibleHeader = True
            gridPedidoTemp.HeaderRow.TableSection = TableRowSection.TableHeader

            Dim unidades As Integer = 0
            Dim espacios As Integer = 0
            For Each row As GridViewRow In gridPedidoTemp.Rows
                espacios = espacios + CInt(row.Cells(6).Text)
                unidades = unidades + CInt(row.Cells(7).Text)
            Next

            lblEspacios.Text = "Espacios Asignados: " & espacios
            lblUnidades.Text = "Unidades Asignadas: " & unidades
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindgridPortalPedidos()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)

                Dim SqlQry As String = "EXEC MOVESA..SP_PORTAL_DISTRIBUIDORES 'PEDIDOS DISPONIBLE DESPACHO'"

                'Dim SqlQry As String = "SELECT RUTA,DOCNUM, convert(char,DOCDATE,103) DOCDATE, DOCSUBTYPE, " &
                '                    " CARDCODE, WHSCODE,WHSNAME,T0.ITEMCODE, T1.MODELO, T1.COLOR, T0.SERIAL_NUM, isnull(ESPACIOS,0) ESPACIOS, LINENUM " &
                '                                " FROM MOVESA..PT_VW_PEDIDOS_DISTRIBUIDORES AS T0 " &
                '                                " INNER JOIN MOVESA..PT_VW_MODELOS AS T1 ON T1.ITEMCODE = T0.ITEMCODE COLLATE SQL_Latin1_General_CP1_CI_AS " &
                '                                " WHERE CANCELED = 'N' AND DISPATCH_ID IS NULL"

                Using cmd As New SqlCommand(SqlQry)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridMotosPortalPedidos.DataSource = dt
                            gridMotosPortalPedidos.DataBind()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridMotosPortalPedidos.UseAccessibleHeader = True
            gridMotosPortalPedidos.HeaderRow.TableSection = TableRowSection.TableHeader


        Catch ex As Exception
            Response.Write("<script>console.log('BindgridPortalPedidos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Try
            Response.Redirect("TrasladosDDashboardDetalle.aspx?planid=" & Request.QueryString("planid"))
        Catch ex As Exception
            Response.Write("<script>console.log('btnRegresar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    'Private Sub btnCrearDespacho_Click(sender As Object, e As EventArgs) Handles btnCrearDespacho.Click

    'End Sub
    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Try

            If txtFechaDespacho.Text = "" Then
                ClientScript.RegisterStartupScript(Me.GetType(), "alert", "Swal.fire('Error', 'No se pudo crear el despacho, se requiere una fecha.', 'error');", True)
            End If

            ' Datos fijos (se pueden modificar según la lógica)
            Dim planid As String = Request.QueryString("planid")
            Dim fechacreacion As DateTime = DateTime.Now
            Dim estado As String = "ABIERTO"
            Dim usuario As String = Session("User")

            Dim nuevoDespachoId As Integer = CrearNuevoDespacho(planid, fechacreacion, estado, fechacreacion, txtFechaDespacho.Text, usuario)

            If nuevoDespachoId > 0 Then

                actualizarDespachoDetalleMotos(nuevoDespachoId, Request.QueryString("planid"), Session("User"))
                actualizarMacroInsert(nuevoDespachoId, txtFechaDespacho.Text, Request.QueryString("planid"))
                InsertarMotosPortalPedidos(nuevoDespachoId, planid)

                ClientScript.RegisterStartupScript(Me.GetType(), "alert",
                                                                        "Swal.fire({" &
                                                                        "  title: 'Éxito'," &
                                                                        "  text: 'Despacho creado correctamente.'," &
                                                                        "  icon: 'success'" &
                                                                        "}).then((result) => {" &
                                                                        "  window.location.href = 'TrasladosDDashboard.aspx';" &
                                                                        "});", True)

                ClientScript.RegisterStartupScript(Me.GetType(), "alert", "Swal.fire('Éxito', 'Despacho creado correctamente.', 'success');", True)

            Else
                ClientScript.RegisterStartupScript(Me.GetType(), "alert", "Swal.fire('Error', 'No se pudo crear el despacho.', 'error');", True)
            End If

        Catch ex As Exception
            Response.Write("<script>console.log('btnCrearDespacho_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function CrearNuevoDespacho(planid As Integer, fechacreacion As DateTime, estado As String, fechainicio As DateTime, fechavence As DateTime, usuario As String) As Integer
        Try
            Dim sCon As String = sCon2
            Dim sel As String = "INSERT INTO [dbo].[DEPACHOS_HEADER] ([PLANID],[FECHACREACION],[ESTADO],[FECHAINICIO],[FECHAVENCE],[USUARIO]) " &
                            "VALUES (@p1, @p2, @p3, @p4, @p5, @p6); " &
                            "SELECT @@IDENTITY;"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.Add("@p1", SqlDbType.Int).Value = planid
                cmd.Parameters.Add("@p2", SqlDbType.DateTime).Value = fechacreacion
                cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = estado
                cmd.Parameters.Add("@p4", SqlDbType.Date).Value = fechainicio
                cmd.Parameters.Add("@p5", SqlDbType.Date).Value = fechavence
                cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50).Value = usuario

                con.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar())
                con.Close()
            End Using
        Catch ex As Exception
            ' Retorna 0 si ocurre un error
            Return 0
        End Try
    End Function
    Public Sub InsertarMotosPortalPedidos(despachoId As String, planId As String)
        Try
            Dim sCon As String = sCon2
            Using con As New SqlConnection(sCon)
                con.Open()

                For Each row As GridViewRow In gridMotosPortalPedidos.Rows
                    Dim cbSeleccionado As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)

                    If cbSeleccionado IsNot Nothing AndAlso cbSeleccionado.Checked Then

                        Using cmd As New SqlCommand("EXEC MOVESA..SP_PORTAL_DISTRIBUIDORES " &
                                                        " @FUNCTION = 'INSERTAR LINEA PICKING', @PARAMETER_1 = @PEDIDOID,  @PARAMETER_2 = @LINEAID, " &
                                                        " @PARAMETER_3 = @DESPACHOID, @PARAMETER_4 = @PLANID, @USUARIO = @USUARIO ", con)
                            Response.Write("<script>console.log('Error en InsertarMotosPortalPedidos: " & ReplaceCharsForFileName(despachoId & " " & planId & " " & row.Cells(2).Text & " " & row.Cells(13).Text, " ") & "');</script>")

                            cmd.Parameters.AddWithValue("@PEDIDOID", row.Cells(2).Text)
                            cmd.Parameters.AddWithValue("@LINEAID", row.Cells(13).Text)
                            cmd.Parameters.AddWithValue("@DESPACHOID", despachoId)
                            cmd.Parameters.AddWithValue("@PLANID", planId)
                            cmd.Parameters.AddWithValue("@USUARIO", Session("User"))
                            cmd.ExecuteNonQuery()
                        End Using
                    End If
                Next
                con.Close()
            End Using
            'ActualizarEstadoLineaDespachoMotosDetalle(planId)

        Catch ex As Exception
            Response.Write("<pre>" & Server.HtmlEncode(ex.ToString()) & "</pre>")

            If ex.InnerException IsNot Nothing Then
                Response.Write("<br><b>InnerException:</b><br><pre>" & Server.HtmlEncode(ex.InnerException.ToString()) & "</pre>")
            End If

            Response.Write("<script>console.log('Error en InsertarMotosPortalPedidos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub actualizarDespachoDetalleMotos(headerid As String, planid As String, usuario As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String = "update [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] " &
                " set [HEADERID] = @headerId, [ESTADO] = 'P', [DESPACHOID] = @headerId, [CODIGOESTADO] = 'DESPACHO ABIERTO' " &
                " where PLANID = @planId And [USUARIO] = @usuario And [ESTADO] = 'T'"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@headerId", headerid)
                cmd.Parameters.AddWithValue("@planId", planid)
                cmd.Parameters.AddWithValue("@usuario", usuario)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Public Sub actualizarMacroInsert(headerid As String, fdespacho As DateTime, planid As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String = "update [ArmadoMotos].[dbo].[MACROINSERT] set [HEADERID] = @headerId, " &
            " [ESTADO] = 'P', [FDESPACHO] = @fdespacho where PLANID = @planId"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@headerId", headerid)
                cmd.Parameters.AddWithValue("@fdespacho", fdespacho)
                cmd.Parameters.AddWithValue("@planId", planid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Public Function getModeloCode(itemcode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT u_amodelo from oitm with(nolock) where itemcode = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", itemcode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getModeloCode: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
        Dim NewStr As String

        sName = Replace(sName, "/", sChr)
        sName = Replace(sName, "\", sChr)
        sName = Replace(sName, ":", sChr)
        sName = Replace(sName, "?", sChr)
        sName = Replace(sName, Chr(34), sChr) ' Comillas dobles
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
                EliminarLinea(gridPedidoTemp.Rows(index).Cells(2).Text, gridPedidoTemp.Rows(index).Cells(4).Text, Request.QueryString("planid"))

                BindgridPortalPedidos()
                BindgridSugerido(Request.QueryString("planid"), Session("User"))

                Dim script As String = "<script>iziToast.warning({title: 'OK!', message: 'Moto Eliminada Con Exito!!!',position: 'topRight',timeout: 10000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPedidoTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub EliminarLinea(idlinea As Integer, itemcode As String, planid As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "DELETE FROM [MACROINSERT] where [ID] = @p1;" &
                " DELETE FROM [DESPACHOS_DETALLE_MOTOS] WHERE PLANID = @planid AND [ARTICULO] = @itemcode"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                cmd.Parameters.AddWithValue("@itemcode", idlinea)
                cmd.Parameters.AddWithValue("@planid", planid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    'Public Sub ActualizarEstadoLineaDespachoMotosDetalle(planid As String)
    '    Try
    '        Dim sCon As String = sCon2
    '        Dim sel As String
    '        sel = "update [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] set [CODIGOESTADO] = 'DESPACHO ABIERTO' WHERE PLANID = @planid"
    '        Using con As New SqlConnection(sCon)
    '            Dim cmd As New SqlCommand(sel, con)
    '            cmd.Parameters.AddWithValue("@planid", planid)
    '            con.Open()
    '            cmd.ExecuteNonQuery()
    '            con.Close()
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub
    'Public Sub EnviarNotificacionEmail()
    '    Try
    '        If Session("User") <> "RJOVEL" Then
    '            Dim server As New SmtpClient
    '            Dim mensaje As New MailMessage
    '            server.Host = "mail.grupomovesa.com"
    '            server.Port = "587"
    '            server.Credentials = New System.Net.NetworkCredential("notificaciones@grupomovesa.com", "VYgd}T!XcVy}")
    '            mensaje.From = New MailAddress("notificaciones@grupomovesa.com")

    '            Dim EMAIL As String = GetEmailSuc(drpSucursales.SelectedValue.ToString.Trim)
    '            Dim constr As String = sCon2
    '            Using con As New SqlConnection(constr)
    '                Using cmd As New SqlCommand("SELECT [ALMDESTINO],[ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS]," &
    '                                                    " [QTYLOGISTICA],[OBSERVACIONES] FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE [ALMDESTINO]='" & drpSucursales.SelectedValue.ToString & "' " &
    '                                                    " AND HEADERID=" & header_macroinsert & "")

    '                    Using sda As New SqlDataAdapter()
    '                        cmd.Connection = con
    '                        sda.SelectCommand = cmd
    '                        Using dt As New DataTable()
    '                            sda.Fill(dt)
    '                            Dim pdfBytes As Byte() = ConvertDataTableToPdf(dt)
    '                            Dim pdfStream As New MemoryStream(pdfBytes)
    '                            mensaje.Attachments.Add(New Net.Mail.Attachment(pdfStream, drpSucursales.SelectedValue.ToString & ".pdf"))
    '                            con.Close()
    '                        End Using
    '                    End Using
    '                End Using
    '            End Using

    '            mensaje.To.Add(EMAIL)
    '            mensaje.CC.Add("nromero@grupomovesa.com")
    '            mensaje.CC.Add("logistica05@grupomovesa.com")
    '            mensaje.CC.Add("pbustillo@grupomovesa.com")
    '            mensaje.CC.Add("analistainventario@grupomovesa.com")
    '            mensaje.CC.Add("rjovel@grupomovesa.com")
    '            mensaje.Subject = "Sugerido Motos #" & header_macroinsert & " Almacen - " & drpSucursales.SelectedValue.ToString & " MOVESA - Portal Produccion de Motos"
    '            mensaje.Body = "" & Session("Name") & ", Ha Creado un Nuevo Sugerido de Motos" & "<BR /> " &
    '                                    "Fecha Propuesta de Armado " & txtFechaArmado.Text & "<BR /> " &
    '                                    "Fecha Propuesta de Despacho " & txtFechaDespacho.Text & "<BR /> " &
    '                                    "<BR /> " &
    '                                    "PORTAL Produccion de Motos By RJ"
    '            mensaje.IsBodyHtml = True
    '            mensaje.Priority = MailPriority.High
    '            server.Send(mensaje)

    '            Response.Write("<script>console.log('Correo Enviado');</script>")
    '        End If
    '    Catch ex As Exception
    '        'lblError.Text = ex.Message
    '    End Try
    'End Sub
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
            Response.Write("<script>console.log('GetEmailSuc: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function

End Class

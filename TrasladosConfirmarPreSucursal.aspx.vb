Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports DocumentFormat.OpenXml.Drawing.Spreadsheet
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class TrasladosConfirmarPreSucursal
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Dim MyTable As New DataTable

    Private Sub TrasladosConfirmarPreSucursal_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub
    Private Sub BindgridSugerido(headerid As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SqlQry As String = " Select ID, HEADERID, RUTA, ALMDESTINO, ARTICULO, MODELO, DESCRIPCION, ESPACIOS, CANTIDAD, QTYLOGISTICA, OBSERVACIONES  " &
                                        " ,CB,FISICO,FALTANTE,VTAA" &
                                        " From ArmadoMotos.dbo.MACROINSERT Where [ESTADO] ='S' AND HEADERID=@HEADERID "

                Using cmd As New SqlCommand(SqlQry)
                    cmd.Parameters.AddWithValue("@HEADERID", headerid)
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

            Dim espacios As Integer = 0
            For Each row As GridViewRow In gridPedidoTemp.Rows
                row.Cells(9).BackColor = System.Drawing.Color.LightSeaGreen
                espacios = espacios + CInt(row.Cells(7).Text)
            Next
            txtEspaciosAsignados.Text = espacios

        Catch ex As Exception
            Response.Write("<script>console.log('" & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Shared Function GetData(query As String) As DataTable
        Dim strConnString As String = sCon2
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand()
                cmd.CommandText = query
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using ds As New DataSet()
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
            con.Close()
        End Using
    End Function
    Private Sub btnGuardarPlanificacion_Click(sender As Object, e As EventArgs) Handles btnGuardarPlanificacion.Click
        Try
            ' Verificar si ya se ejecutó el guardado
            If ViewState("IsPlanificacionSaved") IsNot Nothing AndAlso CBool(ViewState("IsPlanificacionSaved")) = True Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Warning", "iziToast.warning({title: 'Advertencia', message: 'El resurtido ya fue guardado previamente.', position: 'topRight'});", True)
                Exit Sub
            End If

            ' Validar si hay valores null o vacíos
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

            ' Esperar 5 segundos
            'Threading.Thread.Sleep(5000)
            Dim id_header As Integer
            'id_header = Presolicitud_Header("DCM00", "T" & Session("ALMTRANSIT"),
            '                            GetWhsCustomer(Session("ALMTRANSIT")),
            '                            "RESURTIDO",
            '                            0,
            '                            Session("UserCode"),
            '                            Date.Now,
            '                            "PRESOLICITUD",
            '                            Date.Now)

            'If id_header <> 0 Then

            For Each row As GridViewRow In gridPedidoTemp.Rows
                    Dim str As String = TryCast(row.FindControl("qty"), System.Web.UI.WebControls.TextBox).Text
                    Response.Write("<script>console.log('===============================');</script>")
                    Response.Write(String.Format("<script>console.log('{0}');</script>", row.Cells(0).Text))
                    Response.Write(String.Format("<script>console.log('{0}');</script>", row.Cells(4).Text))
                    Response.Write(String.Format("<script>console.log('{0}');</script>", row.Cells(5).Text))
                    Response.Write(String.Format("<script>console.log('{0}');</script>", str))

                'If id_header <> 0 Then
                '    UpdateLineState(row.Cells(0).Text, str)
                '    Presolicitud_Lines(row.Cells(4).Text,
                '                   str,
                '                   GetEspacios(row.Cells(4).Text),
                '                   "PRESOLICITUD",
                '                   "DCM00",
                '                   "T" & Session("ALMTRANSIT"),
                '                   Session("UserCode"),
                '                   row.Cells(6).Text,
                '                   row.Cells(2).Text,
                '                   id_header,
                '                   row.Cells(5).Text, row.Cells(1).Text)
                'End If

                UpdateMacroInsertHeaderId(row.Cells(1).Text)
                UpdateMacroInserLine_QtyResponse(row.Cells(0).Text, str)

            Next

            'For Each row As GridViewRow In gridPedidoTemp.Rows
            '    Dim str As String = TryCast(row.FindControl("qty"), System.Web.UI.WebControls.TextBox).Text
            '    Response.Write("<script>console.log('===============================');</script>")
            '    Response.Write(String.Format("<script>console.log('{0}');</script>", row.Cells(0).Text))
            '    Response.Write(String.Format("<script>console.log('{0}');</script>", row.Cells(4).Text))
            '    Response.Write(String.Format("<script>console.log('{0}');</script>", row.Cells(5).Text))
            '    Response.Write(String.Format("<script>console.log('{0}');</script>", str))

            '    If id_header <> 0 Then
            '        UpdateLineState(row.Cells(0).Text, str)
            '        Presolicitud_Lines(row.Cells(4).Text,
            '                       str,
            '                       GetEspacios(row.Cells(4).Text),
            '                       "PRESOLICITUD",
            '                       "DCM00",
            '                       "T" & Session("ALMTRANSIT"),
            '                       Session("UserCode"),
            '                       row.Cells(6).Text,
            '                       row.Cells(2).Text,
            '                       id_header,
            '                       row.Cells(5).Text, row.Cells(1).Text)
            '    End If
            '    UpdateMacroInsertHeaderId(row.Cells(1).Text)
            'Next

            ' Marcar como guardado en el ViewState
            ViewState("IsPlanificacionSaved") = True
                EnviarNotificacionEmail("T" & Session("ALMTRANSIT"), id_header)

            ' Redirigir para evitar duplicación en caso de refresh
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Redirect", "window.location.href = window.location.href;", True)
            'Else
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Ocurrio Un Error al Intentar Grabar Pre-Solicitud!!!');", True)
            'End If

        Catch ex As Exception
            Response.Write("btnGuardarPlanificacion_Click: " & ex.Message)
            btnGuardarPlanificacion.Visible = True
            Response.Write("<script>console.log('btnGuardarPlanificacion_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub UpdateMacroInsertHeaderId(headerid As String)
        Try
            Dim query As String = String.Empty
            query &= "update [ArmadoMotos].[dbo].[MACROINSERT_HEADER] set [ESTADO]=2 WHERE ID=@P1"
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
    Public Sub UpdateMacroInserLine_QtyResponse(lineid As String, qty As Integer)
        Try
            Dim query As String = String.Empty
            query &= "UPDATE [ArmadoMotos].[dbo].[MACROINSERT]  SET [QTYSUCURSAL] = @p1 WHERE ID = @p2"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", qty)
                        .Parameters.AddWithValue("@p2", lineid)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('UpdateMacroInserLine_QtyResponse: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function GetWhsCustomer(whsname As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "select t0.U_CardCode from OWHS t0 With(nolock) where t0.whscode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whsname)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetWhsCustomer: " & t & "');</script>")
        End Using
    End Function
    Public Function GetEspacios(articulo As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT [U_COLUMNA] FROM [MOVESA].[dbo].[OITM] where [ITEMCODE]=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", articulo)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
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
                                Dim pdfBytes As Byte() = ConvertDataTableToPdf(dt)
                                Dim pdfStream As New MemoryStream(pdfBytes)
                                mensaje.Attachments.Add(New Attachment(pdfStream, Session("ALMTRANSIT").ToString & ".pdf"))
                                con.Close()
                            End Using
                        End Using
                    End Using
                End Using

                mensaje.CC.Add(Session("USEREMAIL").ToString)
                mensaje.To.Add("nromero@grupomovesa.com")
                mensaje.To.Add("logistica05@grupomovesa.com")
                mensaje.CC.Add("pbustillo@grupomovesa.com")
                mensaje.CC.Add("analistainventario@grupomovesa.com")
                mensaje.CC.Add("rjovel@grupomovesa.com")
                mensaje.Subject = "Confirmacion Sugerido de Produccion " & headerid & " " & almdestino & " MOVESA - Portal Produccion de Motos"
                mensaje.Body = "" & Session("Name").ToString & ", Ha Confirmado el Sugerido de Motos" &
                                    "<BR /> " &
                                    "PORTAL Produccion de Motos By RJ"

                mensaje.IsBodyHtml = True
                mensaje.Priority = MailPriority.High
                server.Send(mensaje)
            End Using
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "success", "iziToast.warning({title: 'OK', message: 'Correo Enviado!!', position: 'topRight'});", True)
            BindgridSugerido(txtNumeroSugerido.Text)
            Response.Write("<script>console.log('Correo Enviado');</script>")

        Catch ex As Exception
            Response.Write("<script>console.log('Exception " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
        Dim font As New Font(Font.FontFamily.HELVETICA, 8)
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
    Public Sub UpdateLineState(rowid As String, cantidad As String)
        Try
            Dim query As String = String.Empty
            query &= "update [MACROINSERT] set [QTYSUCURSAL]=@p2, [ESTADO]='L' where id=@p1"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", rowid)
                        .Parameters.AddWithValue("@p2", cantidad)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
            Response.Write("<script>console.log('Linea Actualizada" & rowid & "');</script>")

        Catch ex As Exception
            Response.Write("UpdateLineState " & ex.Message)
        End Try
    End Sub
    Public Sub DeleteLineZero(rowid As String)
        Try
            Dim query As String = String.Empty
            query &= "delete from [MACROINSERT] where id=@p1"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", rowid)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
            Response.Write("<script>console.log('Linea Eliminaa" & rowid & "');</script>")

        Catch ex As Exception
            Response.Write("DeleteLineZero " & ex.Message)
        End Try
    End Sub

    Public Function Presolicitud_Header(ALMORIGEN As String, ALMDESTINO As String, CODCLIALMDESTINO As String,
                                           OBSERACIONES As String, MOTORISTA As String, USERCREATED As String, DATECREATED As DateTime,
                                           ESTATUS As String, FECHADESEADA As DateTime) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[PRESOLICITUD_HEADER] ([ALMORIGEN],[ALMDESTINO],[CODCLIALMDESTINO],[OBSERACIONES]," &
                    " [MOTORISTA],[USERCREATED],[DATECREATED],[ESTATUS],[FECHADESEADA],[ESTADO])" &
                    " VALUES(@p1,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11); SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ALMORIGEN)
                cmd.Parameters.AddWithValue("@p3", ALMDESTINO)
                cmd.Parameters.AddWithValue("@p4", CODCLIALMDESTINO)
                cmd.Parameters.AddWithValue("@p5", OBSERACIONES)
                cmd.Parameters.AddWithValue("@p6", MOTORISTA)
                cmd.Parameters.AddWithValue("@p7", USERCREATED)
                cmd.Parameters.AddWithValue("@p8", DATECREATED)
                cmd.Parameters.AddWithValue("@p9", ESTATUS)
                cmd.Parameters.AddWithValue("@p10", FECHADESEADA)
                cmd.Parameters.AddWithValue("@p11", "A")
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("Presolicitud_Header " & ex.Message)
        End Try
    End Function
    Public Function Presolicitud_Lines(ARTICULO As String, CANTIDAD As String, ESPACIOS As String, ESTADO As String, ALMORIGEN As String,
                                           ALMDESTINO As String, USERCREATED As String, DESCRIPCION As String, RUTA As String, HEADERID As String,
                                           MODELO As String, MACROID As String) As Integer
        Try
            Dim retrnvalue As Integer
            Dim impuestoRetro As Decimal = 0
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[PRESOLICITUD_LINES] ([ARTICULO],[CANTIDAD],[ESPACIOS],[ESTADO],[ALMORIGEN], " &
                " [ALMDESTINO],[USERCREATED],[DESCRIPCION],[RUTA],[HEADERID],[MODELO],[MACROID],[ESTATUS])" &
                " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13); SELECT SCOPE_IDENTITY(); " &
                " UPDATE PRESOLICITUD_HEADER SET MACROID = @p12 where id = @p10;"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ARTICULO)
                cmd.Parameters.AddWithValue("@p2", CANTIDAD)
                cmd.Parameters.AddWithValue("@p3", ESPACIOS)
                cmd.Parameters.AddWithValue("@p4", ESTADO)
                cmd.Parameters.AddWithValue("@p5", ALMORIGEN)
                cmd.Parameters.AddWithValue("@p6", ALMDESTINO)
                cmd.Parameters.AddWithValue("@p7", USERCREATED)
                cmd.Parameters.AddWithValue("@p8", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p9", RUTA)
                cmd.Parameters.AddWithValue("@p10", HEADERID)
                cmd.Parameters.AddWithValue("@p11", MODELO)
                cmd.Parameters.AddWithValue("@p12", MACROID)
                cmd.Parameters.AddWithValue("@p13", "A")
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("Presolicitud_Lines " & ex.Message)
        End Try
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

    Private Sub btnBuscarSugerido_Click(sender As Object, e As EventArgs) Handles btnBuscarSugerido.Click
        Try
            BindgridSugerido(txtNumeroSugerido.Text)
        Catch ex As Exception
            Response.Write("<script>console.log('Linea Actualizada" & ex.Message & "');</script>")

        End Try
    End Sub
End Class

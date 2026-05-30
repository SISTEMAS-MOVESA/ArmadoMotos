
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Partial Class VisorSugerido
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String
    Public SQL_STRING As String
    Private Sub VisorSugerido_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindgridSugerido(Request.QueryString("id"))
                    lblIdMacro.Text = Request.QueryString("id")
                    lblAlmacenDestino.Text = GetWhsName(Request.QueryString("destino"))
                    lblRuta.Text = Request.QueryString("ruta")
                End If
            Else
            End If


        Catch ex As Exception
            Response.Write("<script>console.log('VisorSugerido_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindgridSugerido(headerid As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[RUTA],[ALMDESTINO],[ARTICULO],[MODELO],[DESCRIPCION]," &
                                            "[ESPACIOS],[CANTIDAD],[QTYLOGISTICA],[OBSERVACIONES],[HEADERID] " &
                                            " FROM [dbo].[MACROINSERT] WHERE [HEADERID] =" & headerid & "")
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
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Try
            Response.Redirect("TrasladosDashboard.aspx")
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnReenviarNotificacion_Click(sender As Object, e As EventArgs) Handles btnReenviarNotificacion.Click
        Try
            Threading.Thread.Sleep(5000)
            Dim server As New SmtpClient
            Dim mensaje As New MailMessage
            server.Host = "mail.grupomovesa.com"
            server.Port = "587"
            server.Credentials = New System.Net.NetworkCredential("notificaciones@grupomovesa.com", "VYgd}T!XcVy}")
            mensaje.From = New MailAddress("notificaciones@grupomovesa.com")

            Dim EMAIL As String = GetEmailSuc(Request.QueryString("destino"))
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ALMDESTINO],[ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS]," &
                                            " [QTYLOGISTICA],[OBSERVACIONES] FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE [ALMDESTINO]='" & Request.QueryString("destino") & "' " &
                                            " AND HEADERID=" & Request.QueryString("id") & "")


                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            Dim pdfBytes As Byte() = ConvertDataTableToPdf(dt)
                            Dim pdfStream As New MemoryStream(pdfBytes)
                            mensaje.Attachments.Add(New Attachment(pdfStream, Request.QueryString("destino") & ".pdf"))
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using

            mensaje.To.Add(EMAIL)
            mensaje.CC.Add("nromero@grupomovesa.com")
            mensaje.CC.Add("logistica05@grupomovesa.com")
            mensaje.CC.Add("rjovel@grupomovesa.com")
            mensaje.Subject = "Sugerido Motos #" & Request.QueryString("id") & " Almacen - " & Request.QueryString("destino") & " - Portal Produccion de Motos"
            mensaje.Body = "" & Session("Name") & ", Ha Creado un Nuevo Sugerido de Motos" &
                            "<BR /> " &
                            "PORTAL Produccion de Motos By RJ"
            mensaje.IsBodyHtml = True
            mensaje.Priority = MailPriority.High
            server.Send(mensaje)
            Response.Write("<script>console.log('Correo Enviado');</script>")

            Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Correo Reenviado!',timeout: 10000})</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)

        Catch ex As Exception
            Response.Write("<script>console.log('Exception " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function ConvertDataTableToPdf(dataTable As DataTable) As Byte()
        Dim document As New Document(PageSize.LETTER.Rotate())
        Dim memoryStream As New MemoryStream()
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)
        document.Open()

        Dim table As New PdfPTable(dataTable.Columns.Count)
        table.WidthPercentage = 100

        Dim font As New Font(Font.FontFamily.HELVETICA, 8)
        Dim cellStyle As New PdfPCell()
        cellStyle.HorizontalAlignment = Element.ALIGN_LEFT
        cellStyle.VerticalAlignment = Element.ALIGN_MIDDLE
        cellStyle.Padding = 5

        For i As Integer = 0 To dataTable.Columns.Count - 1
            Dim phrase As New Phrase(dataTable.Columns(i).ColumnName, font)
            Dim headerCell As New PdfPCell(phrase)
            headerCell.BackgroundColor = New BaseColor(230, 230, 230)
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE
            headerCell.Padding = 5
            table.AddCell(headerCell)
        Next

        For i As Integer = 0 To dataTable.Rows.Count - 1
            For j As Integer = 0 To dataTable.Columns.Count - 1
                Dim phrase As New Phrase(dataTable.Rows(i)(j).ToString(), font)
                Dim dataCell As New PdfPCell(phrase)
                dataCell.HorizontalAlignment = Element.ALIGN_LEFT
                dataCell.VerticalAlignment = Element.ALIGN_MIDDLE
                dataCell.Padding = 5
                table.AddCell(dataCell)
            Next
        Next

        document.Add(table)
        document.Close()
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
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
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
        NewStr = sName
        Return NewStr

    End Function
End Class

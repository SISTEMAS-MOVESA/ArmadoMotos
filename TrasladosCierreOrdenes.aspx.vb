Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports DocumentFormat.OpenXml.Spreadsheet
Imports SAPbobsCOM
Partial Class TrasladosCierreOrdenes
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public almacen As String

    'VARIABLES DE USO PARA SAP
    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public sErrMsg As String
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public OTS As SAPbobsCOM.StockTransfer
    Public OTSR As SAPbobsCOM.StockTransfer
    Public oTransferDraft As SAPbobsCOM.StockTransfer
    Public oOrder As SAPbobsCOM.Documents

    Private Sub TrasladosCierreOrdenes_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                BindGridDespachosAbiertos()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub BindGridDespachosAbiertos()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String

                SQL_STRING = "SELECT DISTINCT t0.docentry, t0.docnum, t0.docdate, t0.cardcode, t0.cardname, " &
                                  "DATEDIFF(DAY, t0.DocDate, GETDATE()) AS dias " &
                                  "FROM ORDR t0 " &
                                  "INNER JOIN RDR1 t1 ON t0.docentry = t1.docentry " &
                                  "INNER JOIN OITM t2 ON t1.ItemCode = t2.ItemCode " &
                                  "WHERE DATEDIFF(DAY, t0.DocDate, GETDATE()) > 2 AND t0.DocStatus = 'O' AND t2.ManSerNum = 'Y'"


                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPedidosAbiertosSap.DataSource = dt
                            gridPedidosAbiertosSap.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridPedidosAbiertosSap.UseAccessibleHeader = True
            gridPedidosAbiertosSap.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDespachosAbiertos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub ADD_LOG_ORDER_CLOSED(DOCENTRY As String, DOCNUM As String, DOCDATE As Date, CARDCODE As String, CARDNAME As String,
                                    CREATEDATE As Date, DIAS As String, TIPODOCUMENTO As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String

            sel = " INSERT INTO movesaweb.[dbo].[EntregaPlacasService_LOG_ORDER_CLOSED] " &
                " ([DOCENTRY],[DOCNUM],[DOCDATE],[CARDCODE],[CARDNAME],[CREATEDATE],[DIAS],[TIPODOCUMENTO]) " &
                " VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", DOCENTRY)
                cmd.Parameters.AddWithValue("@p2", DOCNUM)
                cmd.Parameters.AddWithValue("@p3", DOCDATE)
                cmd.Parameters.AddWithValue("@p4", CARDCODE)
                cmd.Parameters.AddWithValue("@p5", CARDNAME)
                cmd.Parameters.AddWithValue("@p6", CREATEDATE)
                cmd.Parameters.AddWithValue("@p7", DIAS)
                cmd.Parameters.AddWithValue("@p8", TIPODOCUMENTO)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('ADD_LOG_ORDER_CLOSED: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function GlobalConnecttoSAP() As Integer
        SCompany = New SAPbobsCOM.Company
        SCompany.Server = "192.168.1.3"
        SCompany.CompanyDB = "MOVESA"
        SCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
        SCompany.DbUserName = "sa"
        SCompany.DbPassword = "M*l!n3r0s2k12"
        SCompany.UserName = "it"
        SCompany.Password = "polar"
        SCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
        SCompany.SLDServer = "192.168.1.9:40000"
        lRetCode = SCompany.Connect
        Return lRetCode
    End Function
    Protected Sub CloseOrder()
        On Error Resume Next
        'Try

        ' Conexión a SAP
        'GlobalConnecttoSAP()
        'If lRetCode <> 0 Then
        '    Response.Write("<script>console.log(""Error SAP: " & ReplaceCharsForFileName(SCompany.GetLastErrorDescription, " ") & """);</script>")
        '    Exit Sub
        'End If

        'Dim oOrder As SAPbobsCOM.Documents = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)

        For Each row As GridViewRow In gridPedidosAbiertosSap.Rows
            Dim cbSeleccionado As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)

            If cbSeleccionado IsNot Nothing AndAlso cbSeleccionado.Checked Then
                'Dim docentry As Integer = Convert.ToInt32(row.Cells(1).Text)
                Response.Write(row.Cells(1).Text)
                'Response.Write("<script>console.log(""Error SAP: " & ReplaceCharsForFileName(row.Cells(1).Text, " ") & """);</script>")

                'If oOrder.GetByKey(docentry) Then
                '    For lc As Integer = 0 To oOrder.Lines.Count - 1
                '        oOrder.Lines.SetCurrentLine(lc)
                '        If oOrder.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Close Then
                '            ADD_LOG("Estado de Linea OV", oOrder.Lines.LineStatus.ToString(), docentry)
                '            oOrder.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                '        End If
                '    Next

                '    Dim intStatus As Integer = oOrder.Update()
                '    If intStatus <> 0 Then
                '        Dim errCode As Integer
                '        Dim errMsg As String = String.Empty
                '        SCompany.GetLastError(errCode, errMsg)
                '        ADD_LOG("Error al actualizar la orden de venta", "Error: " & errMsg, docentry)
                '    Else
                '        ADD_LOG_ORDER_CLOSED(row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text, row.Cells(4).Text, row.Cells(5).Text, Date.Now, row.Cells(6).Text, "OV")
                '    End If
                'End If

                'Runtime.InteropServices.Marshal.ReleaseComObject(oOrder)
                'oOrder = Nothing
            End If
        Next

        '    ' Confirmación con SweetAlert2
        '    Dim confirmScript As String = "Swal.fire({" &
        '    "icon: 'success'," &
        '    "title: 'Órdenes cerradas correctamente'," &
        '    "showConfirmButton: true," &
        '    "confirmButtonText: 'Aceptar'" &
        '    "});"
        '    ClientScript.RegisterStartupScript(Me.GetType(), "showSuccess", "<script>" & confirmScript & "</script>", False)

        'Catch ex As Exception
        '    ADD_LOG("Error al Cerrar Orden de Venta", ex.Message, 0)
        '    Dim errorScript As String = "Swal.fire({" &
        '    "icon: 'error'," &
        '    "title: 'Error'," &
        '    "text: 'Ocurrió un error al cerrar las órdenes. Revisa el log.'," &
        '    "confirmButtonText: 'Aceptar'" &
        '    "});"
        '    ClientScript.RegisterStartupScript(Me.GetType(), "showError", "<script>" & errorScript & "</script>", False)
        'End Try
    End Sub
    'Protected Sub CloseOrder()
    '    Try
    '        ' Mostrar loader con SweetAlert2
    '        Dim loaderScript As String = "Swal.fire({" &
    '            "title: 'Cerrando órdenes...'," &
    '            "html: 'Por favor espere unos segundos'," &
    '            "allowOutsideClick: false," &
    '            "didOpen: () => {" &
    '            "Swal.showLoading()" &
    '            "}" &
    '            "});"
    '        ClientScript.RegisterStartupScript(Me.GetType(), "showLoader", "<script>" & loaderScript & "</script>", False)

    '        ' Conexión a SAP
    '        GlobalConnecttoSAP()
    '        If lRetCode <> 0 Then
    '            Response.Write("<script>console.log(""Error SAP: " & ReplaceCharsForFileName(SCompany.GetLastErrorDescription, " ") & """);</script>")
    '            Exit Sub
    '        End If

    '        Dim oOrder As SAPbobsCOM.Documents = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)

    '        For Each row As GridViewRow In gridPedidosAbiertosSap.Rows
    '            Dim cbSeleccionado As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)

    '            If cbSeleccionado IsNot Nothing AndAlso cbSeleccionado.Checked Then
    '                Dim docentry As Integer = Convert.ToInt32(row.Cells(1).Text)

    '                If oOrder.GetByKey(docentry) Then
    '                    For lc As Integer = 0 To oOrder.Lines.Count - 1
    '                        oOrder.Lines.SetCurrentLine(lc)
    '                        If oOrder.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Close Then
    '                            ADD_LOG("Estado de Linea OV", oOrder.Lines.LineStatus.ToString(), docentry)
    '                            oOrder.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
    '                        End If
    '                    Next

    '                    Dim intStatus As Integer = oOrder.Update()
    '                    If intStatus <> 0 Then
    '                        Dim errCode As Integer
    '                        Dim errMsg As String = String.Empty
    '                        SCompany.GetLastError(errCode, errMsg)
    '                        ADD_LOG("Error al actualizar la orden de venta", "Error: " & errMsg, docentry)
    '                    Else
    '                        ADD_LOG_ORDER_CLOSED(row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text, row.Cells(4).Text, row.Cells(5).Text, Date.Now, row.Cells(6).Text, "OV")
    '                    End If
    '                End If

    '                Runtime.InteropServices.Marshal.ReleaseComObject(oOrder)
    '                oOrder = Nothing
    '            End If
    '        Next

    '        ' Confirmación con SweetAlert2
    '        Dim confirmScript As String = "Swal.fire({" &
    '            "icon: 'success'," &
    '            "title: 'Órdenes cerradas correctamente'," &
    '            "showConfirmButton: true," &
    '            "confirmButtonText: 'Aceptar'" &
    '            "});"
    '        ClientScript.RegisterStartupScript(Me.GetType(), "showSuccess", "<script>" & confirmScript & "</script>", False)

    '    Catch ex As Exception
    '        ADD_LOG("Error al Cerrar Orden de Venta", ex.Message, 0)
    '        Dim errorScript As String = "Swal.fire({" &
    '            "icon: 'error'," &
    '            "title: 'Error'," &
    '            "text: 'Ocurrió un error al cerrar las órdenes. Revisa el log.'," &
    '            "confirmButtonText: 'Aceptar'" &
    '            "});"
    '        ClientScript.RegisterStartupScript(Me.GetType(), "showError", "<script>" & errorScript & "</script>", False)
    '    End Try
    'End Sub

    'Protected Sub CloseOrder()
    '    Try
    '        ' Conexión a SAP
    '        GlobalConnecttoSAP()
    '        If lRetCode <> 0 Then
    '            Response.Write("<script>console.log('ADD_LOG_ORDER_CLOSED: " & ReplaceCharsForFileName(SCompany.GetLastErrorDescription, " ") & "');</script>")
    '            Exit Sub
    '        End If

    '        Dim oOrder As SAPbobsCOM.Documents = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
    '        For Each row As GridViewRow In gridPedidosAbiertosSap.Rows
    '            Dim cbSeleccionado As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)

    '            If cbSeleccionado IsNot Nothing AndAlso cbSeleccionado.Checked Then

    '                Dim docentry As Integer = row.Cells(1).Text

    '                If oOrder.GetByKey(docEntry) Then
    '                    For lc As Integer = 0 To oOrder.Lines.Count - 1
    '                        oOrder.Lines.SetCurrentLine(lc)
    '                        If oOrder.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Close Then
    '                            ADD_LOG("Estado de Linea OV", oOrder.Lines.LineStatus.ToString(), docEntry)
    '                            oOrder.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
    '                        End If
    '                    Next

    '                    Dim intStatus As Integer = oOrder.Update()
    '                    If intStatus <> 0 Then
    '                        Dim errCode As Integer
    '                        Dim errMsg As String = String.Empty
    '                        SCompany.GetLastError(errCode, errMsg)
    '                        ADD_LOG("Error al actualizar la orden de venta", "Error: " & errMsg, docentry)
    '                    Else
    '                        ADD_LOG_ORDER_CLOSED(row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text, row.Cells(4).Text, row.Cells(5).Text, Date.Now, row.Cells(6).Text, "OV")
    '                    End If
    '                End If

    '                Runtime.InteropServices.Marshal.ReleaseComObject(oOrder)
    '                oOrder = Nothing

    '            End If
    '        Next

    '    Catch ex As Exception
    '        ADD_LOG("Error al Cerrar Orden de Venta", ex.Message, 0)
    '    End Try
    'End Sub
    Public Sub ADD_LOG(_PROCESO As String, _ERROR As String, _TOKEN As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String

            sel = " INSERT INTO [movesaweb].[dbo].[EntregaPlacasService] ([FECHA],[PROCESO],[ERROR],[TOKEN]) " &
                    " VALUES (@p1,@p2,@p3,@p4) "

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", Date.Now)
                cmd.Parameters.AddWithValue("@p2", _PROCESO)
                cmd.Parameters.AddWithValue("@p3", _ERROR)
                cmd.Parameters.AddWithValue("@p4", _TOKEN)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('ADD_LOG_ORDER_CLOSED: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

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

    'Private Sub btnCerrarOrdenes_Click(sender As Object, e As EventArgs) Handles btnCerrarOrdenes.Click
    '    Try
    '        '' Mostrar loader con SweetAlert2
    '        'Dim loaderScript As String = "Swal.fire({" &
    '        '"title: 'Cerrando órdenes...'," &
    '        '"html: 'Por favor espere unos segundos'," &
    '        '"allowOutsideClick: false," &
    '        '"didOpen: () => {" &
    '        '"Swal.showLoading()" &
    '        '"}" &
    '        '"});"
    '        'ClientScript.RegisterStartupScript(Me.GetType(), "showLoader", "<script>" & loaderScript & "</script>", False)
    '        Response.Write("<script>console.log('Hola');</script>")
    '        Response.Write("Hola")

    '        'CloseOrder()
    '        For Each row As GridViewRow In gridPedidosAbiertosSap.Rows
    '            Dim cbSeleccionado As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)

    '            If cbSeleccionado IsNot Nothing AndAlso cbSeleccionado.Checked Then
    '                'Dim docentry As Integer = Convert.ToInt32(row.Cells(1).Text)
    '                'Response.Write(row.Cells(1).Text)
    '                Response.Write("<script>console.log('BindGridDespachosAbiertos: " & ReplaceCharsForFileName(row.Cells(1).Text, " ") & "');</script>")
    '                Response.Write("Hola")

    '                'If oOrder.GetByKey(docentry) Then
    '                '    For lc As Integer = 0 To oOrder.Lines.Count - 1
    '                '        oOrder.Lines.SetCurrentLine(lc)
    '                '        If oOrder.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Close Then
    '                '            ADD_LOG("Estado de Linea OV", oOrder.Lines.LineStatus.ToString(), docentry)
    '                '            oOrder.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
    '                '        End If
    '                '    Next

    '                '    Dim intStatus As Integer = oOrder.Update()
    '                '    If intStatus <> 0 Then
    '                '        Dim errCode As Integer
    '                '        Dim errMsg As String = String.Empty
    '                '        SCompany.GetLastError(errCode, errMsg)
    '                '        ADD_LOG("Error al actualizar la orden de venta", "Error: " & errMsg, docentry)
    '                '    Else
    '                '        ADD_LOG_ORDER_CLOSED(row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text, row.Cells(4).Text, row.Cells(5).Text, Date.Now, row.Cells(6).Text, "OV")
    '                '    End If
    '                'End If

    '                'Runtime.InteropServices.Marshal.ReleaseComObject(oOrder)
    '                'oOrder = Nothing
    '            End If
    '        Next


    '    Catch ex As Exception
    '        Response.Write("<script>console.log('btnCerrarOrdenes_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub

    Protected Sub btnCerrarOrdenes_Click(sender As Object, e As EventArgs) Handles btnCerrarOrdenes.Click
        On Error Resume Next

        'Conexión a SAP
        GlobalConnecttoSAP()
        If lRetCode <> 0 Then
            Response.Write("<script>console.log('ADD_LOG_ORDER_CLOSED: " & ReplaceCharsForFileName(SCompany.GetLastErrorDescription, " ") & "');</script>")
            Exit Sub
        End If


        Dim oOrder As SAPbobsCOM.Documents = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
        For Each row As GridViewRow In gridPedidosAbiertosSap.Rows
            Dim cbSeleccionado As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)

            If cbSeleccionado IsNot Nothing AndAlso cbSeleccionado.Checked Then

                Dim docentry As Integer = row.Cells(1).Text

                If oOrder.GetByKey(docentry) Then
                    'For lc As Integer = 0 To oOrder.Lines.Count - 1
                    '    oOrder.Lines.SetCurrentLine(lc)
                    '    If oOrder.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Close Then
                    '        'ADD_LOG("Cierre OV Web", oOrder.Lines.LineStatus.ToString(), docentry)
                    '        oOrder.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                    '    End If
                    'Next

                    'Dim intStatus As Integer = oOrder.Update()
                    Dim intStatus As Integer = oOrder.Cancel()

                    If intStatus <> 0 Then
                        ADD_LOG("Error al actualizar la orden de venta", "Error: " & SCompany.GetLastErrorDescription, docentry)
                    Else
                        ADD_LOG_ORDER_CLOSED(row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text, row.Cells(4).Text, row.Cells(5).Text, Date.Now, row.Cells(6).Text, "Cierre OV Web")
                    End If
                End If

                Runtime.InteropServices.Marshal.ReleaseComObject(oOrder)
                oOrder = Nothing

            End If
        Next
        BindGridDespachosAbiertos()
        ' Confirmación con SweetAlert2
        Dim confirmScript As String = "Swal.fire({" &
        "icon: 'success'," &
        "title: 'Órdenes cerradas correctamente'," &
        "showConfirmButton: true," &
        "confirmButtonText: 'Aceptar'" &
        "});"
        ClientScript.RegisterStartupScript(Me.GetType(), "showSuccess", "<script>" & confirmScript & "</script>", False)

        'For Each row As GridViewRow In gridPedidosAbiertosSap.Rows
        '    If row.RowType = DataControlRowType.DataRow Then
        '        Dim cbSeleccionado As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)

        '        If cbSeleccionado IsNot Nothing AndAlso cbSeleccionado.Checked Then
        '            ' Obtener valores de la fila
        '            Dim docEntry As String = row.Cells(1).Text
        '            Dim docNum As String = row.Cells(2).Text

        '            ' Aquí puedes realizar tu lógica, por ejemplo:
        '            Response.Write("<script>console.log('Seleccionado: " & docEntry & " - " & docNum & "');</script>")
        '        End If
        '    End If
        'Next
    End Sub

End Class

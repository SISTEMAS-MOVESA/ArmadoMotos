Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Hosting
Imports ClosedXML.Excel
Imports DocumentFormat.OpenXml.ExtendedProperties
Imports SAPbobsCOM



Partial Class CrearFreservaPlacas
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"


    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public xCompany As SAPbobsCOM.Company
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public oGoodsRcpt As SAPbobsCOM.Documents

    Public _docentry As String
    Public _docnum As String
    Public _docdate As DateTime
    Public _cardcode As String
    Public _carndname As String
    Public _createdate As DateTime
    Public _msg As String


    Protected Sub ImportExcel(sender As Object, e As EventArgs)
        'Save the uploaded Excel file.
        Dim filePath As String = HostingEnvironment.ApplicationPhysicalPath & Path.GetFileName(fu1.PostedFile.FileName)
        fu1.SaveAs(filePath)

        'Open the Excel file using ClosedXML.
        Using workBook As New XLWorkbook(filePath)
            'Read the first Sheet from Excel file.
            Dim workSheet As IXLWorksheet = workBook.Worksheet(1)

            'Create a new DataTable.
            Dim dt As New DataTable()

            'Loop through the Worksheet rows.
            Dim firstRow As Boolean = True
            For Each row As IXLRow In workSheet.Rows()
                'Use the first row to add columns to DataTable.
                If firstRow Then
                    For Each cell As IXLCell In row.Cells()
                        dt.Columns.Add(cell.Value.ToString())
                    Next
                    firstRow = False
                Else
                    'Check for blank cells before adding rows to DataTable.
                    If row.CellsUsed().Any(Function(c) Not String.IsNullOrEmpty(c.Value.ToString())) Then
                        dt.Rows.Add()
                        Dim i As Integer = 0
                        For Each cell As IXLCell In row.Cells()
                            dt.Rows(dt.Rows.Count - 1)(i) = cell.Value.ToString()
                            i += 1
                        Next
                    End If
                End If
            Next

            gridReservas.DataSource = dt
            gridReservas.DataBind()
            gridReservas.UseAccessibleHeader = True
            gridReservas.HeaderRow.TableSection = TableRowSection.TableHeader
        End Using
    End Sub

    'Protected Sub ImportExcel(sender As Object, e As EventArgs)
    '    'Save the uploaded Excel file.
    '    Dim filePath As String = HostingEnvironment.ApplicationPhysicalPath & Path.GetFileName(fu1.PostedFile.FileName)
    '    fu1.SaveAs(filePath)

    '    'Open the Excel file using ClosedXML.
    '    Using workBook As New XLWorkbook(filePath)
    '        'Read the first Sheet from Excel file.
    '        Dim workSheet As IXLWorksheet = workBook.Worksheet(1)

    '        'Create a new DataTable.
    '        Dim dt As New DataTable()

    '        'Loop through the Worksheet rows.
    '        Dim firstRow As Boolean = True
    '        For Each row As IXLRow In workSheet.Rows()
    '            'Use the first row to add columns to DataTable.
    '            If firstRow Then
    '                For Each cell As IXLCell In row.Cells()
    '                    dt.Columns.Add(cell.Value.ToString())
    '                Next
    '                firstRow = False
    '            Else
    '                'Add rows to DataTable.
    '                dt.Rows.Add()
    '                Dim i As Integer = 0
    '                For Each cell As IXLCell In row.Cells()
    '                    dt.Rows(dt.Rows.Count - 1)(i) = cell.Value.ToString()
    '                    i += 1
    '                Next
    '            End If

    '            gridReservas.DataSource = dt
    '            gridReservas.DataBind()
    '        Next
    '        gridReservas.UseAccessibleHeader = True
    '        gridReservas.HeaderRow.TableSection = TableRowSection.TableHeader

    '    End Using
    'End Sub

    'MOVESA_TSET
    'MOVESA

    Public Function GlobalConnecttoSAP() As Integer
        SCompany = New SAPbobsCOM.Company
        SCompany.Server = "192.168.1.3"
        SCompany.CompanyDB = "MOVESA"
        SCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
        SCompany.DbUserName = "sa"
        SCompany.DbPassword = "M*l!n3r0s2k12"
        SCompany.UserName = "itpro"
        SCompany.Password = "eeal96"
        SCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
        SCompany.SLDServer = "192.168.1.9:40000"
        lRetCode = SCompany.Connect
        Return lRetCode
    End Function

    Protected Function GenerateSAPPO(DOCSERIES As Integer, DOCNUM As String, CARDCODE As String, CARDNAME As String, SERIALNUM As String,
                                MONTO As Decimal, ITEMCODE As String, WHSCODE As String, SLPCODE As Integer, CODIGOSKCLIENTE As String,
                                NOMBRECLIENTEFINAL As String) As String


        Try

            Dim ReserveInvoice As SAPbobsCOM.Documents = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
            ReserveInvoice.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
            ReserveInvoice.CardCode = Left(CARDCODE, 15)
            ReserveInvoice.CardName = Left(CARDNAME, 100)
            ReserveInvoice.Series = DOCSERIES
            ReserveInvoice.DocDate = Today
            ReserveInvoice.Reference2 = "FRPP"
            ReserveInvoice.Comments = "Usuario:" & Session("Name").ToString
            ReserveInvoice.NumAtCard = DOCNUM
            ReserveInvoice.SalesPersonCode = SLPCODE
            ReserveInvoice.GroupNumber = 48


            'lines
            ReserveInvoice.Lines.ItemCode = ITEMCODE
            ReserveInvoice.Lines.WarehouseCode = WHSCODE
            ReserveInvoice.Lines.SerialNum = SERIALNUM
            ReserveInvoice.Lines.Quantity = 1
            ReserveInvoice.Lines.Price = MONTO
            ReserveInvoice.Lines.TaxCode = "EXE"

            ReserveInvoice.Lines.UserFields.Fields.Item("U_N_Factura").Value = Left(DOCNUM, 20)
            ReserveInvoice.Lines.UserFields.Fields.Item("U_Cardcode").Value = Left(CODIGOSKCLIENTE, 10)
            ReserveInvoice.Lines.UserFields.Fields.Item("U_Cardname").Value = Left(NOMBRECLIENTEFINAL, 100)
            ReserveInvoice.Lines.Add()

            lRetCode = ReserveInvoice.Add
            If lRetCode <> 0 Then
                Response.Write("<script>console.log('Error al Intentar Crear Documeto: " & ReplaceCharsForFileName(SCompany.GetLastErrorDescription, " ") & "');</script>")
                Return "Error al Intentar Crear Documeto: " & ReplaceCharsForFileName(SCompany.GetLastErrorDescription, " ")
            Else
                Response.Write("<script>console.log('Documento Creado, Docentry: " & SCompany.GetNewObjectKey() & "');</script>")
                Return "Documento Creado, Docentry: " & SCompany.GetNewObjectKey()
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('Error General: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            Return "Error General: " & ReplaceCharsForFileName(ex.Message, " ")
        End Try
    End Function
    Protected Function CreateCreditemo(docentry As Integer, docnum As String) As String
        Try
            Dim oInvoice As SAPbobsCOM.Documents = CType(SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices), SAPbobsCOM.Documents)
            If oInvoice.GetByKey(docentry) Then
                Dim vCreditMemo As SAPbobsCOM.Documents = CType(SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes), SAPbobsCOM.Documents)

                _docentry = oInvoice.DocEntry
                _docnum = oInvoice.DocNum
                _docdate = oInvoice.DocDate
                _cardcode = oInvoice.CardCode
                _carndname = oInvoice.CardName
                _createdate = Date.Now


                vCreditMemo.CardCode = oInvoice.CardCode
                vCreditMemo.DocDate = oInvoice.DocDate
                vCreditMemo.DocDueDate = oInvoice.DocDueDate
                vCreditMemo.TaxDate = oInvoice.TaxDate
                vCreditMemo.Comments = "Factura Reversada #" & docnum & " Usuario:" & Session("Name").ToString
                vCreditMemo.NumAtCard = docnum
                vCreditMemo.UserFields.Fields.Item("U_DocBase").Value = "13"
                vCreditMemo.UserFields.Fields.Item("U_FacturaBase").Value = docnum
                vCreditMemo.UserFields.Fields.Item("U_NameSN").Value = oInvoice.CardName
                vCreditMemo.UserFields.Fields.Item("U_RTNSN").Value = GetIdentidad(oInvoice.CardCode)
                vCreditMemo.Reference2 = "RVRPP"

                Dim line As Integer = 0
                For i As Integer = 0 To oInvoice.Lines.Count - 1
                    oInvoice.Lines.SetCurrentLine(i)
                    If oInvoice.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Open Then
                        vCreditMemo.Lines.BaseEntry = oInvoice.DocEntry
                        vCreditMemo.Lines.BaseType = CInt(SAPbobsCOM.BoObjectTypes.oInvoices)
                        vCreditMemo.Lines.BaseLine = oInvoice.Lines.LineNum
                        vCreditMemo.Lines.Add()
                        line = line + 1
                    End If
                Next

                lRetCode = vCreditMemo.Add()
            Else
                lRetCode = -1
            End If
            If lRetCode <> 0 Then
                _msg = "Error al Intentar Crear Documeto: " & ReplaceCharsForFileName(SCompany.GetLastErrorDescription, " ")
                Response.Write("<script>console.log('Error al Intentar Crear Documeto: " & ReplaceCharsForFileName(SCompany.GetLastErrorDescription, " ") & "');</script>")
                Return "Error al Intentar Crear Documeto: " & ReplaceCharsForFileName(SCompany.GetLastErrorDescription, " ")
            Else
                _msg = "Documento Creado, Docentry: " & SCompany.GetNewObjectKey()
                ADD_LOG_ORDER_CLOSED(_docentry, _docnum, _docdate, _cardcode, _carndname, _createdate, 0, "RESERVA")
                Response.Write("<script>console.log('Documento Creado, Docentry: " & SCompany.GetNewObjectKey() & "');</script>")
                Return "Documento Creado, Docentry: " & SCompany.GetNewObjectKey()
            End If

        Catch ex As Exception
            '_msg = "Erorr General: " & ReplaceCharsForFileName(ex.Message, " ")
            ADD_LOG_ORDER_CLOSED(_docentry, _docnum, _docdate, _cardcode, _carndname, _createdate, 0, "RESERVA")
            Response.Write("<script>console.log('Error General: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            Return "Error General: " & ReplaceCharsForFileName(ex.Message, " ")
        End Try

    End Function
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
    Protected Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        On Error Resume Next

        GlobalConnecttoSAP()

        If GlobalConnecttoSAP() <> 0 Then
            Response.Write("<script>console.log('Error al Intentar Conectarse a SAP');</script>")
            Exit Sub
        End If

        For Each fila As GridViewRow In gridReservas.Rows

            If VeifyIfExists(fila.Cells(1).Text) = "False" Then
                fila.Cells(11).Text = GenerateSAPPO(fila.Cells(0).Text,
                        fila.Cells(1).Text,
                        fila.Cells(2).Text,
                        Page.Server.HtmlDecode(fila.Cells(3).Text),
                        fila.Cells(4).Text,
                        fila.Cells(5).Text,
                        fila.Cells(6).Text,
                        fila.Cells(7).Text,
                        fila.Cells(8).Text,
                        fila.Cells(9).Text,
                        Page.Server.HtmlDecode(fila.Cells(10).Text))
            Else
                fila.Cells(11).Text = "Documento Ya Existe"
            End If
        Next
        SCompany.Disconnect()
        Runtime.InteropServices.Marshal.ReleaseComObject(SCompany)
        GC.Collect()
        GC.WaitForPendingFinalizers()
        SCompany = Nothing

        gridReservas.UseAccessibleHeader = True
        gridReservas.HeaderRow.TableSection = TableRowSection.TableHeader

    End Sub
    'Public Sub InsertLogCreacionDocumentos(p1 As String, p2 As String, p3 As String, p4 As String, p5 As String, p6 As String, p7 As String, p8 As String,
    '                                       p9 As String, p10 As String, p11 As String)
    '    Try
    '        Dim sCon As String = sCon1
    '        Dim sel As String
    '        sel = " INSERT INTO [dbo].[MOTOSLIQ]([LIQUIDACIONID],[IDCALIDAD],[SERIE],[MODELO],[COLOR],[MECANICOID] " &
    '            " ,[CPROVEEDOR],[NPROVEEDOR],[CUENTAC],[PRECIOARMADO],[FECHACREACION],[USUARIOCREACION]) " &
    '            " VALUES (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12)"
    '        Using con As New SqlConnection(sCon)
    '            Dim cmd As New SqlCommand(sel, con)
    '            cmd.Parameters.AddWithValue("@p1", _LIQUIDACIONID)
    '            cmd.Parameters.AddWithValue("@p2", _IDCALIDAD)
    '            cmd.Parameters.AddWithValue("@p3", _SERIE)
    '            cmd.Parameters.AddWithValue("@p4", _MODELO)
    '            cmd.Parameters.AddWithValue("@p5", _COLOR)
    '            cmd.Parameters.AddWithValue("@p6", _MECANICOID)
    '            cmd.Parameters.AddWithValue("@p7", _CPROVEEDOR)
    '            cmd.Parameters.AddWithValue("@p8", _NPROVEEDOR)
    '            cmd.Parameters.AddWithValue("@p9", _CUENTAC)
    '            cmd.Parameters.AddWithValue("@p10", _PRECIOARMADO)
    '            cmd.Parameters.AddWithValue("@p11", _FECHACREACION)
    '            cmd.Parameters.AddWithValue("@p12", _USUARIOCREACION)
    '            con.Open()
    '            cmd.ExecuteNonQuery()
    '            con.Close()
    '        End Using

    '        If _CPROVEEDOR = "PL0000000" Then
    '            CloseLocalLiquidation(_LIQUIDACIONID)
    '        End If
    '    Catch ex As Exception
    '        Response.Write("CreateNewLiquidationDetalle " & ex.Message)
    '    End Try
    'End Sub
    Protected Sub btnReverseInvoice_Click(sender As Object, e As EventArgs) Handles btnReverseInvoice.Click
        On Error Resume Next

        GlobalConnecttoSAP()

        If GlobalConnecttoSAP() <> 0 Then
            Response.Write("<script>console.log('Error al Intentar Conectarse a SAP');</script>")
            Exit Sub
        End If

        For Each fila As GridViewRow In gridReservas.Rows
            fila.Cells(2).Text = CreateCreditemo(fila.Cells(0).Text, fila.Cells(1).Text)
        Next
        SCompany.Disconnect()
        Runtime.InteropServices.Marshal.ReleaseComObject(SCompany)
        GC.Collect()
        GC.WaitForPendingFinalizers()
        SCompany = Nothing

        gridReservas.UseAccessibleHeader = True
        gridReservas.HeaderRow.TableSection = TableRowSection.TableHeader
    End Sub

    Public Function GetIdentidad(cardcode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "select ISNULL(VatIdUnCmp,CardFName) from ocrd with(nolock) where CardCode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", cardcode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetIdentidad: " & t & "');</script>")
        End Using
    End Function

    Public Function VeifyIfExists(docnum As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT " &
                    " CASE  " &
                    "     WHEN EXISTS ( " &
                    "         SELECT * " &
                    "         FROM inv1 t1 WITH (NOLOCK) " &
                    "         WHERE t1.U_N_Factura = @p1 " &
                    "     ) THEN 'True' " &
                    "     ELSE 'False' " &
                    " END AS ExistsFlag"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", docnum)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('VeifyIfExists: " & t & "');</script>")
        End Using
    End Function
    Private Sub CrearFreservaPlacas_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")

                    Else
                    End If
                End If
            End If

            If Session("User") = "RJOVEL" Then
                btnReverseInvoice.Visible = True
            End If

            Response.Write("<script>console.log('cargarPlanificacionesAbiertas: " & ReplaceCharsForFileName(Session("User"), " ") & "');</script>")

        Catch ex As Exception
            Response.Write("<script>console.log('cargarPlanificacionesAbiertas: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
End Class

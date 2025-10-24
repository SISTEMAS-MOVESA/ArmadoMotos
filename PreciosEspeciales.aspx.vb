Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System
Imports System.IO
Imports System.Web.Hosting
Imports Microsoft.VisualBasic.FileIO
Imports SAPbobsCOM
Imports DocumentFormat.OpenXml.Spreadsheet

Partial Class PreciosEspeciales
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


    Protected Sub ImportExcel_ospp(sender As Object, e As EventArgs)
        Try
            Dim filePath As String = HostingEnvironment.ApplicationPhysicalPath & Path.GetFileName(FU_OSPP.PostedFile.FileName)
            FU_OSPP.SaveAs(filePath)
            Dim dt As New DataTable()
            Using parser As New TextFieldParser(filePath)
                parser.TextFieldType = FieldType.Delimited
                parser.SetDelimiters({",", ";"})
                Dim fields As String() = parser.ReadFields()
                For Each field As String In fields
                    dt.Columns.Add(field)
                Next
                While Not parser.EndOfData
                    Dim rowData As String() = parser.ReadFields()
                    dt.Rows.Add(rowData)
                End While
            End Using
            grid_ospp.DataSource = dt
            grid_ospp.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('ImportExcel_ospp: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub ImportExcel_spp1(sender As Object, e As EventArgs)
        Try
            Dim filePath As String = HostingEnvironment.ApplicationPhysicalPath & Path.GetFileName(FU_SPP1.PostedFile.FileName)
            FU_SPP1.SaveAs(filePath)
            Dim dt As New DataTable()
            Using parser As New TextFieldParser(filePath)
                parser.TextFieldType = FieldType.Delimited
                parser.SetDelimiters({",", ";"})
                Dim fields As String() = parser.ReadFields()
                For Each field As String In fields
                    dt.Columns.Add(field)
                Next
                While Not parser.EndOfData
                    Dim rowData As String() = parser.ReadFields()
                    dt.Rows.Add(rowData)
                End While
            End Using
            grid_spp1.DataSource = dt
            grid_spp1.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('ImportExcel_spp1: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
        NewStr = sName
        Return NewStr

    End Function
    Protected Sub CreateUpdateSpecialPriceList(sender As Object, e As EventArgs)
        Try
            Response.Write("<script>console.log('CreateUpdateSpecialPriceList: Iniciando Conexion: " & Date.Now & "');</script>")

            If GlobalConnecttoSAP() <> 0 Then
                Response.Write("<script>alert('Error al Intentar Conectarse a SAP');</script>")
                Exit Sub
            End If

            Response.Write("<script>console.log('CreateUpdateSpecialPriceList: Conectado a SAP: " & Date.Now & "');</script>")

            Dim oSpecialPrices As SAPbobsCOM.SpecialPrices
            Dim oSpecialPricesData As SAPbobsCOM.SpecialPricesDataAreas
            Dim SpecialPricesDataAreas As SAPbobsCOM.SpecialPricesQuantityAreas
            Dim _items As Integer = 0
            Dim _itemsoff As Integer = 0
            Dim bRetVal As Boolean

            For Each row As GridViewRow In grid_ospp.Rows
                Dim accessType As String = row.Cells(3).Text
                ' Realizar operaciones con el valor de la celda, por ejemplo, imprimirlo
                ''Response.Write("AccessType: " & accessType & "<br>")
                Response.Write("<script>console.log('CreateUpdateSpecialPriceList: " & row.Cells(0).Text & "');</script>")


                Dim sp As SAPbobsCOM.SpecialPrices
                sp = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSpecialPrices)
                bRetVal = sp.GetByKey(row.Cells(0).Text, "*" & CStr(row.Cells(5).Text))
                If bRetVal = True Then
                    sp.Remove()
                End If
                'ospp
                oSpecialPrices = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSpecialPrices)
                oSpecialPrices.ItemCode = row.Cells(0).Text
                oSpecialPrices.PriceListNum = row.Cells(5).Text
                'oSpecialPrices.CardCode = "*" & CStr(row.Cells("Lista").Value.ToString.Trim)
                'spp1
                For Each row_ssp1 As GridViewRow In grid_spp1.Rows
                    If (row.Cells(0).Text = row_ssp1.Cells(0).Text) Then

                        oSpecialPricesData = oSpecialPrices.SpecialPricesDataAreas
                        oSpecialPricesData.PriceListNo = row.Cells(5).Text
                        oSpecialPricesData.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tYES
                        oSpecialPricesData.DateFrom = CDate(txtDate_from.Text)
                        oSpecialPricesData.Dateto = CDate(txtDate_to.Text)
                        oSpecialPricesData.Discount = CDbl(row_ssp1.Cells(6).Text)
                        'oSpecialPrices.SpecialPricesDataAreas.SpecialPrice = row.Cells("SpecialPrice").Value
                        'oSpecialPricesData.SpecialPricesDataAreas.Add()

                    End If
                Next
                'spp2
                'For Each row_ssp1 As GridViewRow In grid_spp1.Rows
                'If (row.Cells(0).Text = row_ssp1.Cells(0).Text) Then
                'SpecialPricesDataAreas = oSpecialPricesData.SpecialPricesQuantityAreas
                'SpecialPricesDataAreas.Discountin = CDbl(row_ssp1.Cells(6).Text)
                'SpecialPricesDataAreas.Quantity = CDbl(rows("Qty").ToString())
                'SpecialPricesDataAreas.SpecialPrice = CDbl(rows("Price").ToString())
                'oSpecialPrices.SpecialPricesDataAreas.SpecialPricesQuantityAreas.Add()
                lRetCode = oSpecialPrices.Add()

                If lRetCode <> 0 Then
                    SCompany.GetLastError(lErrCode, sErrMsg)
                    'Response.Write(sErrMsg)
                    Response.Write("<script>console.log('Error al Intentar Grabar Precio Especial: " & ReplaceCharsForFileName(sErrMsg, " ") & "');</script>")
                Else
                    Response.Write("<script>console.log('Precio Especial Agregado!!!');</script>")
                End If
            Next
            SCompany.Disconnect()
            Response.Write("<script>console.log('Fin de Proceso" & Date.Now & "');</script>")
            'Runtime.InteropServices.Marshal.ReleaseComObject(SpecialPricesDataAreas)
            Runtime.InteropServices.Marshal.ReleaseComObject(oSpecialPricesData)
            Runtime.InteropServices.Marshal.ReleaseComObject(oSpecialPrices)
            Runtime.InteropServices.Marshal.ReleaseComObject(SCompany)
            GC.Collect()
            GC.WaitForPendingFinalizers()
            'SpecialPricesDataAreas = Nothing
            oSpecialPricesData = Nothing
            oSpecialPrices = Nothing
            SCompany = Nothing
        Catch ex As Exception
            'Response.Write("CreateUpdateSpecialPriceList: " & ex.Message)
            Response.Write("<script>console.log('CreateUpdateSpecialPriceList: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
End Class

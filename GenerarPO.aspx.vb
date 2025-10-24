Imports System.IO
Imports System.Linq
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports SAPbobsCOM
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class GenerarPO
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

    Protected Sub GenerateSAPPO(_liquidacionid As String)
        Try
            If GlobalConnecttoSAP() <> 0 Then
                Response.Write("<script>alert('Error al Intentar Conectarse a SAP');</script>")
                Exit Sub
            End If

            Dim purchaseOrder As SAPbobsCOM.Documents = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders)
            purchaseOrder.CardCode = txtBPCode.Text
            purchaseOrder.Series = 220
            purchaseOrder.DocDate = Today
            purchaseOrder.DocDueDate = Today
            purchaseOrder.TaxDate = Today
            purchaseOrder.DocType = BoDocumentTypes.dDocument_Service
            purchaseOrder.Reference2 = txtNumeroLiquidacion.Text
            purchaseOrder.Comments = "Liquidacio #" & txtNumeroLiquidacion.Text & " Orden de Compra Creada por: " & Session("Name").ToString
            purchaseOrder.NumAtCard = txtRefProveedor.Text

            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT * FROM [ArmadoMotos].[dbo].[MOTOSLIQ] with(nolock) where liquidacionid=" & _liquidacionid & " ")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            For Each row As DataRow In dt.Rows
                                purchaseOrder.Lines.ItemDescription = row.Item("modelo").ToString() & " " & row.Item("color").ToString()
                                purchaseOrder.Lines.AccountCode = row.Item("cuentac").ToString()
                                purchaseOrder.Lines.UserFields.Fields.Item("U_QTY").Value = 1
                                purchaseOrder.Lines.UserFields.Fields.Item("U_MSERIE").Value = row.Item("serie").ToString()
                                purchaseOrder.Lines.UserFields.Fields.Item("U_pi_number").Value = row.Item("liquidacionid").ToString()
                                purchaseOrder.Lines.LineTotal = row.Item("precioarmado").ToString()
                                purchaseOrder.Lines.TaxCode = "EXE"
                                purchaseOrder.Lines.Add()
                            Next
                        End Using
                    End Using
                End Using
                con.Close()
            End Using

            lRetCode = purchaseOrder.Add
            If lRetCode <> 0 Then
                Response.Write(SCompany.GetLastErrorDescription)
                SCompany.Disconnect()
                Runtime.InteropServices.Marshal.ReleaseComObject(purchaseOrder)
                Runtime.InteropServices.Marshal.ReleaseComObject(SCompany)
                GC.Collect()
                GC.WaitForPendingFinalizers()
                purchaseOrder = Nothing
                SCompany = Nothing
            Else
                UpdateLIQUIDACIONES(SCompany.GetNewObjectKey.ToString, Session("UserCode").ToString(), Date.Now, _liquidacionid)
                UpdateLMOTOSLIQ(SCompany.GetNewObjectKey.ToString, Session("UserCode").ToString(), Date.Now, _liquidacionid)
                SCompany.Disconnect()
                Runtime.InteropServices.Marshal.ReleaseComObject(purchaseOrder)
                Runtime.InteropServices.Marshal.ReleaseComObject(SCompany)
                GC.Collect()
                GC.WaitForPendingFinalizers()
                purchaseOrder = Nothing
                SCompany = Nothing
                Response.Redirect("GenerarPO.aspx")
            End If
        Catch ex As Exception
            Response.Write("btnProcesarOrden_Click " & ex.Message)
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

    Private Sub BindGrid()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT T0.[ID] " &
                                                ", (SELECT TOP 1 MOTOSLIQ.CPROVEEDOR FROM [ArmadoMotos].[dbo].MOTOSLIQ WHERE LIQUIDACIONID=T0.ID) [BPCODE]  " &
                                                ", T0.[FECHACREACION] " &
                                                ",convert(decimal(19,0),T0.CANTIDAD) [CANTIDAD] " &
                                                ",T0.[MONTO] " &
                                                ",T0.[USERCODE]  " &
                                                ",(SELECT DOCNUM FROM MOVESA..OPOR WHERE DocEntry=T0.DOCENTRYPO) [OCSAP] " &
                                                ",(SELECT DocDate FROM MOVESA..OPOR WHERE DocEntry=T0.DOCENTRYPO) [FOSAP] " &
                                                ",case isnull((SELECT DOCNUM FROM MOVESA..OPCH WITH(NOLOCK) WHERE DOCENTRY=(Select top 1 DocEntry from movesa..pch1 With(nolock) where BaseEntry=t0.DOCENTRYPO And BaseType=22 )),0)" &
                                                " WHEN 0 THEN CASE T0.DOCENTRYPO WHEN 0 THEN 'LIQ' ELSE 'OCS' END ELSE 'FPS' END [ESTATUS] " &
                                                ", isnull((SELECT DOCNUM FROM MOVESA..OPCH WITH(NOLOCK) WHERE DOCENTRY=(Select top 1 DocEntry from movesa..pch1 With(nolock) where BaseEntry=t0.DOCENTRYPO And BaseType=22 )),0)[FPSAP]" &
                                                ", isnull((SELECT DOCDATE FROM MOVESA..OPCH WITH(NOLOCK) WHERE DOCENTRY=(Select top 1 DocEntry from movesa..pch1 With(nolock) where BaseEntry=t0.DOCENTRYPO And BaseType=22 )),0)[FFPSAP]" &
                                                "From [ArmadoMotos].[dbo].[LIQUIDACIONES] T0 Where T0.CANCELED= 'N'")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            con.Close()
                            Dim cantidad As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("CANTIDAD"))
                            GridView1.FooterRow.Cells(4).Text = "Total Motos: " & cantidad.ToString("N2")
                            GridView1.FooterRow.Cells(4).Font.Bold = True
                            GridView1.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right

                            Dim total As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("MONTO"))
                            GridView1.FooterRow.Cells(5).Text = "Monto a Pagar: " & total.ToString("N2")
                            GridView1.FooterRow.Cells(5).Font.Bold = True
                            GridView1.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                            GridView1.UseAccessibleHeader = True
                            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
                        End Using
                    End Using
                End Using
            End Using
        Catch ex As Exception
            'Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub

    Private Sub GenerarPO_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Position") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGrid()
                    Select Case Session("Position").ToString()
                        Case "Iniciador"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Supervisor Armado"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Calidad"
                            Response.Redirect("MainDashBoard.aspx")
                    End Select
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Modificar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                pnlCreatePO.Visible = True
                pnlGrid.Visible = False
                txtNumeroLiquidacion.Text = GridView1.Rows(index).Cells(2).Text
                txtBPCode.Text = GridView1.Rows(index).Cells(7).Text
            End If
            If e.CommandName = "Imprimir" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                ExportToPdf("EXECUTE [Armadomotos].[dbo].[CRYSTAL_LIQUIDACION]  " & GridView1.Rows(index).Cells(2).Text & "", "./LIQUIDACION.rpt", "Liquidacion")
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub ExportToPdf(ByVal StringProcedure As String, ByVal path As String, ByVal name As String)
        Try
            Dim conn = New SqlConnection(sCon1)
            Dim sqlstring As String = StringProcedure
            Using comando = New SqlCommand(sqlstring, conn)
                Using adaptador = New SqlDataAdapter(comando)
                    Dim ds = New DataSet()
                    adaptador.Fill(ds)
                    Dim reporte = New ReportDocument()
                    reporte.Load(Server.MapPath(path))
                    reporte.SetDataSource(ds.Tables(0))
                    Response.Buffer = False
                    Response.ClearContent()
                    Response.ClearHeaders()
                    reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, name & "_" & String.Format("{0:ddMMyyyyHHmm}", DateTime.Now))
                    reporte.Close()
                    reporte.Dispose()
                End Using
            End Using
        Catch ex As Exception
            Response.Write(name & " " & ex.Message)
        End Try
    End Sub
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("GenerarPO.aspx")
    End Sub
    Protected Sub btnCrearPOSAP_Click(sender As Object, e As EventArgs) Handles btnCrearPOSAP.Click
        GenerateSAPPO(txtNumeroLiquidacion.Text)
    End Sub
    Public Sub UpdateLIQUIDACIONES(_docentry As String, _userupdate As String, _fechaupdate As Date, _id As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "	update [ArmadoMotos].[dbo].[LIQUIDACIONES] " &
                    " set DOCENTRYPO=@p1 " &
                    " ,USERUPDATE=@p2 " &
                    " ,FECHAUPDATE=@p3 " &
                    " where id=@p4"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _docentry)
                cmd.Parameters.AddWithValue("@p2", _userupdate)
                cmd.Parameters.AddWithValue("@p3", _fechaupdate)
                cmd.Parameters.AddWithValue("@p4", _id)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("UpdateLIQUIDACIONES " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateLMOTOSLIQ(_docentry As String, _userupdate As String, _fechaupdate As Date, _id As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " update [ArmadoMotos].[dbo].[MOTOSLIQ] " &
                    " set	USUARIOCREACION=@p1 " &
                    " ,FECHACREACIONPO=@p2 " &
                    " ,DOCENTRYPO=@p3  " &
                    " where LIQUIDACIONID=@p4"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _userupdate)
                cmd.Parameters.AddWithValue("@p2", _fechaupdate)
                cmd.Parameters.AddWithValue("@p3", _docentry)
                cmd.Parameters.AddWithValue("@p4", _id)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("UpdateLMOTOSLIQ " & ex.Message)
        End Try
    End Sub
    'Private Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim exc As Exception = Server.GetLastError()
    '    If TypeOf exc Is HttpUnhandledException Then
    '        Response.Redirect("Default.aspx", True)
    '    End If
    'End Sub
    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Select Case e.Row.Cells(12).Text
                    Case "LIQ"
                        e.Row.Cells(2).BackColor = System.Drawing.Color.Purple
                        e.Row.Cells(2).ForeColor = System.Drawing.Color.White
                    Case "OCS"
                        e.Row.Cells(2).BackColor = System.Drawing.Color.Orange
                        e.Row.Cells(2).ForeColor = System.Drawing.Color.White
                        e.Row.Cells(0).Enabled = False
                    Case "FPS"
                        e.Row.Cells(2).BackColor = System.Drawing.Color.Green
                        e.Row.Cells(2).ForeColor = System.Drawing.Color.White
                        e.Row.Cells(0).Enabled = False
                End Select
            End If
        Catch ex As Exception
            Response.Write("GridView1_RowDataBound " & ex.Message)
        End Try
    End Sub
End Class

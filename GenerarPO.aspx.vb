Imports System.IO
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports SAPbobsCOM
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class GenerarPO
    Inherits System.Web.UI.Page

    ' SAP COM objects — used by GenerateSAPPO / GlobalConnecttoSAP
    Public SCompany As SAPbobsCOM.Company
    Public lRetCode As Integer

    ' ── Page Load ─────────────────────────────────────────────────────────────

    Private Sub GenerarPO_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Position") Is vbNullString Then Response.Redirect("Default.aspx")
                Select Case Session("Position").ToString()
                    Case "Iniciador", "Supervisor Armado", "Calidad"
                        Response.Redirect("MainDashBoard.aspx")
                End Select
                lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString()
                Dim desde As New Date(Date.Now.Year, Date.Now.Month, 1)
                Dim hasta As Date = Date.Now
                txtDesde.Text = desde.ToString("yyyy-MM-dd")
                txtHasta.Text = hasta.ToString("yyyy-MM-dd")
                BindGrid(desde, hasta)
            End If
        Catch ex As Exception
            Response.Write("Load: " & ex.Message)
        End Try
    End Sub

    ' ── Filter helpers ────────────────────────────────────────────────────────

    Private Function GetFechas() As Tuple(Of Date, Date)
        Dim desde As New Date(Date.Now.Year, Date.Now.Month, 1)
        Dim hasta As Date = Date.Now
        If Not String.IsNullOrEmpty(txtDesde.Text) Then Date.TryParse(txtDesde.Text, desde)
        If Not String.IsNullOrEmpty(txtHasta.Text) Then Date.TryParse(txtHasta.Text, hasta)
        Return Tuple.Create(desde, hasta)
    End Function

    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        Dim fechas As Tuple(Of Date, Date) = GetFechas()
        BindGrid(fechas.Item1, fechas.Item2)
    End Sub

    ' ── BindGrid ──────────────────────────────────────────────────────────────

    Private Sub BindGrid(desde As Date, hasta As Date)
        Try
            Dim sql As String =
                "SELECT T0.[ID]," &
                "(SELECT TOP 1 MOTOSLIQ.CPROVEEDOR FROM [ArmadoMotos].[dbo].MOTOSLIQ WHERE LIQUIDACIONID=T0.ID) [BPCODE]," &
                "T0.[FECHACREACION]," &
                "CONVERT(DECIMAL(19,0),T0.CANTIDAD) [CANTIDAD]," &
                "T0.[MONTO]," &
                "T0.[USERCODE]," &
                "(SELECT DOCNUM FROM MOVESA..OPOR WHERE DocEntry=T0.DOCENTRYPO) [OCSAP]," &
                "(SELECT DocDate FROM MOVESA..OPOR WHERE DocEntry=T0.DOCENTRYPO) [FOSAP]," &
                "CASE ISNULL((SELECT DOCNUM FROM MOVESA..OPCH WITH(NOLOCK) WHERE DOCENTRY=" &
                "  (SELECT TOP 1 DocEntry FROM movesa..pch1 WITH(NOLOCK) WHERE BaseEntry=T0.DOCENTRYPO AND BaseType=22)),0)" &
                "  WHEN 0 THEN CASE T0.DOCENTRYPO WHEN 0 THEN 'LIQ' ELSE 'OCS' END ELSE 'FPS' END [ESTATUS]," &
                "ISNULL((SELECT DOCNUM FROM MOVESA..OPCH WITH(NOLOCK) WHERE DOCENTRY=" &
                "  (SELECT TOP 1 DocEntry FROM movesa..pch1 WITH(NOLOCK) WHERE BaseEntry=T0.DOCENTRYPO AND BaseType=22)),0) [FPSAP]," &
                "ISNULL((SELECT DOCDATE FROM MOVESA..OPCH WITH(NOLOCK) WHERE DOCENTRY=" &
                "  (SELECT TOP 1 DocEntry FROM movesa..pch1 WITH(NOLOCK) WHERE BaseEntry=T0.DOCENTRYPO AND BaseType=22)),0) [FFPSAP] " &
                "FROM [ArmadoMotos].[dbo].[LIQUIDACIONES] T0 " &
                "WHERE T0.CANCELED='N' " &
                "AND T0.FECHACREACION >= @desde AND T0.FECHACREACION < DATEADD(DAY,1,@hasta) " &
                "ORDER BY T0.ID DESC"

            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@desde", desde.Date),
                    New SqlParameter("@hasta", hasta.Date)})

            ' KPIs
            Dim totalLiq As Integer = dt.Rows.Count
            Dim totalMotos As Decimal = If(totalLiq > 0, dt.AsEnumerable().Sum(Function(r) r.Field(Of Decimal)("CANTIDAD")), 0D)
            Dim totalMonto As Decimal = If(totalLiq > 0, dt.AsEnumerable().Sum(Function(r) r.Field(Of Decimal)("MONTO")), 0D)
            lblKpiLiquidaciones.Text = totalLiq.ToString()
            lblKpiMotos.Text = totalMotos.ToString("N0")
            lblKpiMonto.Text = "L " & totalMonto.ToString("N2")
            lblRango.Text = "Del " & desde.ToString("dd/MM/yyyy") & " al " & hasta.ToString("dd/MM/yyyy")

            GridView1.DataSource = dt
            GridView1.DataBind()
            GridView1.UseAccessibleHeader = True
            If GridView1.HeaderRow IsNot Nothing Then
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
            If GridView1.FooterRow IsNot Nothing Then
                GridView1.FooterRow.Cells(4).Text = "Total: " & totalMotos.ToString("N0")
                GridView1.FooterRow.Cells(4).Font.Bold = True
                GridView1.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                GridView1.FooterRow.Cells(5).Text = "L " & totalMonto.ToString("N2")
                GridView1.FooterRow.Cells(5).Font.Bold = True
                GridView1.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ex.Message.Replace("'", " ").Replace(Chr(34), " ") & "');</script>")
        End Try
    End Sub

    ' ── GridView events ───────────────────────────────────────────────────────

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Modificar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                pnlCreatePO.Visible = True
                pnlGrid.Visible = False
                txtNumeroLiquidacion.Text = GridView1.Rows(index).Cells(2).Text
                txtBPCode.Text = GridView1.Rows(index).Cells(7).Text
            End If
            If e.CommandName = "Imprimir" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                ExportToPdf("EXECUTE [Armadomotos].[dbo].[CRYSTAL_LIQUIDACION] " & GridView1.Rows(index).Cells(2).Text, "./LIQUIDACION.rpt", "Liquidacion")
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Select Case e.Row.Cells(12).Text
                    Case "LIQ"
                        e.Row.CssClass = "row-liq"
                    Case "OCS"
                        e.Row.CssClass = "row-ocs"
                        Dim btn As ImageButton = TryCast(e.Row.Cells(0).Controls(0), ImageButton)
                        If btn IsNot Nothing Then btn.Enabled = False
                    Case "FPS"
                        e.Row.CssClass = "row-fps"
                        Dim btn As ImageButton = TryCast(e.Row.Cells(0).Controls(0), ImageButton)
                        If btn IsNot Nothing Then btn.Enabled = False
                End Select
            End If
        Catch ex As Exception
            Response.Write("GridView1_RowDataBound: " & ex.Message)
        End Try
    End Sub

    ' ── SAP PO creation ───────────────────────────────────────────────────────

    Protected Sub btnCrearPOSAP_Click(sender As Object, e As EventArgs) Handles btnCrearPOSAP.Click
        GenerateSAPPO(txtNumeroLiquidacion.Text)
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("GenerarPO.aspx")
    End Sub

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

            Dim dtMotos As DataTable = DbConfig.GetDataTable(
                "SELECT * FROM [ArmadoMotos].[dbo].[MOTOSLIQ] WITH(NOLOCK) WHERE liquidacionid=@id",
                DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@id", _liquidacionid)})

            For Each row As DataRow In dtMotos.Rows
                purchaseOrder.Lines.ItemDescription = row("modelo").ToString() & " " & row("color").ToString()
                purchaseOrder.Lines.AccountCode = row("cuentac").ToString()
                purchaseOrder.Lines.UserFields.Fields.Item("U_QTY").Value = 1
                purchaseOrder.Lines.UserFields.Fields.Item("U_MSERIE").Value = row("serie").ToString()
                purchaseOrder.Lines.UserFields.Fields.Item("U_pi_number").Value = row("liquidacionid").ToString()
                purchaseOrder.Lines.LineTotal = row("precioarmado").ToString()
                purchaseOrder.Lines.TaxCode = "EXE"
                purchaseOrder.Lines.Add()
            Next

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
            Response.Write("GenerateSAPPO: " & ex.Message)
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

    Protected Sub ExportToPdf(ByVal StringProcedure As String, ByVal path As String, ByVal name As String)
        Try
            Dim dt As DataTable = DbConfig.GetDataTable(StringProcedure, DbConfig.DBServer.ARMADOMOTOS)
            Dim ds As New DataSet()
            ds.Tables.Add(dt.Copy())
            Dim reporte = New ReportDocument()
            reporte.Load(Server.MapPath(path))
            reporte.SetDataSource(ds.Tables(0))
            Response.Buffer = False
            Response.ClearContent()
            Response.ClearHeaders()
            reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, name & "_" & String.Format("{0:ddMMyyyyHHmm}", DateTime.Now))
            reporte.Close()
            reporte.Dispose()
        Catch ex As Exception
            Response.Write(name & " " & ex.Message)
        End Try
    End Sub

    ' ── DB updates ────────────────────────────────────────────────────────────

    Public Sub UpdateLIQUIDACIONES(_docentry As String, _userupdate As String, _fechaupdate As Date, _id As String)
        Try
            DbConfig.ExecuteNonQuery(
                "UPDATE [ArmadoMotos].[dbo].[LIQUIDACIONES] SET DOCENTRYPO=@p1,USERUPDATE=@p2,FECHAUPDATE=@p3 WHERE id=@p4",
                DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@p1", _docentry),
                    New SqlParameter("@p2", _userupdate),
                    New SqlParameter("@p3", _fechaupdate),
                    New SqlParameter("@p4", _id)})
        Catch ex As Exception
            Response.Write("UpdateLIQUIDACIONES: " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateLMOTOSLIQ(_docentry As String, _userupdate As String, _fechaupdate As Date, _id As String)
        Try
            DbConfig.ExecuteNonQuery(
                "UPDATE [ArmadoMotos].[dbo].[MOTOSLIQ] SET USUARIOCREACION=@p1,FECHACREACIONPO=@p2,DOCENTRYPO=@p3 WHERE LIQUIDACIONID=@p4",
                DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@p1", _userupdate),
                    New SqlParameter("@p2", _fechaupdate),
                    New SqlParameter("@p3", _docentry),
                    New SqlParameter("@p4", _id)})
        Catch ex As Exception
            Response.Write("UpdateLMOTOSLIQ: " & ex.Message)
        End Try
    End Sub

End Class

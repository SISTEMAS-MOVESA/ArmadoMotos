Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class OCRProveedor
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Private Sub OCRProveedor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Position") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
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
            If Not IsPostBack Then
                GridView1.DataSource = GetData("SELECT [ITEMCODE],[ITEMNAME],SUM([QTY])[TOTAL], " &
                                                "(select CONVERT(INT, onhand) from movesa..OITW With(nolock) where whscode='DCR00' AND ITEMCODE=[RETIROREPUESTOS].ITEMCODE collate Modern_Spanish_CI_AS)[DCR],[LIQUIDACIONID]  " &
                                                " From [ArmadoMotos].[dbo].[RETIROREPUESTOS] with(nolock) where [ESTATUS]=1 and [LIQUIDACIONID]=0 GROUP BY [ITEMCODE],[ITEMNAME],[LIQUIDACIONID]")
                GridView1.DataBind()
                GridView1.UseAccessibleHeader = True
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
                GridView1.Caption = GridView1.Rows.Count - 1
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
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

    'Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim CODPROV As String = gvCustomers.DataKeys(e.Row.RowIndex).Value.ToString()
    '        Dim gvOrders As GridView = TryCast(e.Row.FindControl("gvOrders"), GridView)
    '        gvOrders.DataSource = GetData("SELECT [ITEMCODE],[ITEMNAME],sum([QTY])[Cant] " &
    '                                                "FROM [ArmadoMotos].[dbo].[RETIROREPUESTOS] WHERE [ESTATUS]='1' and ISNULL((select CARDCODE from movesa..OITM with(nolock) where ITEMCODE=[RETIROREPUESTOS].ITEMCODE collate Modern_Spanish_CI_AS),'Sin Proveedor')='" & CODPROV & "'" &
    '                                                "Group by [ITEMCODE],[ITEMNAME] ")
    '        gvOrders.DataBind()
    '    End If
    'End Sub

    Private Sub ExportGridToExcel()
        Response.Clear()
        Response.Buffer = True
        Response.ClearContent()
        Response.ClearHeaders()
        Response.Charset = ""
        Dim FileName As String = "Resumen Repuestos a Comprar " & DateTime.Now & ".xls"
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
    Protected Sub btnIngreso_Click(sender As Object, e As EventArgs) Handles btnIngreso.Click
        ExportGridToExcel()
        'ExportExcelWorkbook("EXECUTE [Armadomotos].[dbo].[CRYSTAL_REPUESTOSPROVEEDORES]", "./PedidoRepuestos.rpt", "PedidoRepuestosProveedores")

    End Sub
    Protected Sub ExportExcelWorkbook(ByVal StringProcedure As String, ByVal path As String, ByVal name As String)
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
                    reporte.ExportToHttpResponse(ExportFormatType.Excel, Response, True, name & "_" & String.Format("{0:ddMMyyyyHHmm}", DateTime.Now))
                    reporte.Close()
                    reporte.Dispose()
                End Using
            End Using
            conn.Close()
        Catch ex As Exception
            Response.Write(name & " " & ex.Message)
        End Try
    End Sub
    Protected Sub btnOrdenCompraGEN_Click(sender As Object, e As EventArgs) Handles btnOrdenCompraGEN.Click
        GridView1.DataSource = GetData("SELECT [ITEMCODE],[ITEMNAME],SUM([QTY])[TOTAL], " &
                                "(select CONVERT(INT, onhand) from movesa..OITW With(nolock) where whscode='DCR00' AND ITEMCODE=[RETIROREPUESTOS].ITEMCODE collate Modern_Spanish_CI_AS)[DCR] ,[LIQUIDACIONID] " &
                                " From [ArmadoMotos].[dbo].[RETIROREPUESTOS] with(nolock) where [ESTATUS]=1 and [ITEMNAME] NOT LIKE '%OEM%'  and [LIQUIDACIONID]=0 GROUP BY [ITEMCODE],[ITEMNAME],[LIQUIDACIONID]")
        GridView1.DataBind()
        GridView1.UseAccessibleHeader = True
        GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        GridView1.Caption = GridView1.Rows.Count - 1
    End Sub
    Protected Sub btnOrdenCompraOEM_Click(sender As Object, e As EventArgs) Handles btnOrdenCompraOEM.Click
        GridView1.DataSource = GetData("SELECT [ITEMCODE],[ITEMNAME],SUM([QTY])[TOTAL], " &
                                "(select CONVERT(INT, onhand) from movesa..OITW With(nolock) where whscode='DCR00' AND ITEMCODE=[RETIROREPUESTOS].ITEMCODE collate Modern_Spanish_CI_AS)[DCR]  ,[LIQUIDACIONID]" &
                                " From [ArmadoMotos].[dbo].[RETIROREPUESTOS] with(nolock) where [ESTATUS]=1 and [ITEMNAME] LIKE '%OEM%'  and [LIQUIDACIONID]=0 GROUP BY [ITEMCODE],[ITEMNAME],[LIQUIDACIONID]")
        GridView1.DataBind()
        GridView1.UseAccessibleHeader = True
        GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        GridView1.Caption = GridView1.Rows.Count - 1
    End Sub
    Public Function CreateNewLiquidation(_fecha As Date, _cantidad As Integer, _usercode As String) As Integer
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "INSERT INTO [dbo].[LIQUIDACIONES_REPUESTOS] ([DATECREATED],[CANTIDAD],[USUARIO]) " &
                " VALUES (@p1,@p2,@p3);Select CAST(scope_identity() As int)"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", _fecha)
            cmd.Parameters.AddWithValue("@p2", _cantidad)
            cmd.Parameters.AddWithValue("@p3", _usercode)
            con.Open()
            Dim t As Integer = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Protected Sub btnCrearLiquidacion_Click(sender As Object, e As EventArgs) Handles btnCrearLiquidacion.Click
        Dim NumeroLiquidacionRuntime As Integer
        NumeroLiquidacionRuntime = CreateNewLiquidation(Date.Now, GridView1.Caption.ToString(), Session("UserCode").ToString())

        For Each row As GridViewRow In GridView1.Rows
            'Dim accessType As String = row.Cells(1).Text
            UpdateItemsLiquidationId(NumeroLiquidacionRuntime, row.Cells(0).Text)
        Next
    End Sub
    Public Sub UpdateItemsLiquidationId(_liquidacion As Integer, _itemcode As String)
        Dim sCon As String = sCon2
        Dim sel As String
        sel = " UPDATE [ArmadoMotos].[dbo].[RETIROREPUESTOS]  SET [LIQUIDACIONID]=@P1 where ITEMCODE=@P2 AND liquidacionid=0 and [ESTATUS]=1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", _liquidacion)
            cmd.Parameters.AddWithValue("@p2", _itemcode)
            con.Open()
            Dim t As Integer = cmd.ExecuteScalar()
            con.Close()
        End Using
    End Sub

    Private Sub btnEstadoCompra_Click(sender As Object, e As EventArgs) Handles btnEstadoCompra.Click
        'GridView2
        pnlGridResumenRepuestos.Visible = False
        pnlGridArriboCompra.Visible = True

        GridView2.DataSource = GetData("SELECT	[ITEMCODE],[ITEMNAME],DOCUMENTOSAP,(select DocDueDate from movesa..OPOR WITH(NOLOCK) WHERE DocNum=DOCUMENTOSAP)[FechaArribo] " &
                                        " From [ArmadoMotos].[dbo].[RETIROREPUESTOS] With(nolock) where [ESTATUS]=1 And SOLICITADO=1 GROUP BY [ITEMCODE],[ITEMNAME],DOCUMENTOSAP")
        GridView2.DataBind()
        GridView2.UseAccessibleHeader = True
        GridView2.HeaderRow.TableSection = TableRowSection.TableHeader
        GridView2.Caption = GridView1.Rows.Count - 1
    End Sub
End Class
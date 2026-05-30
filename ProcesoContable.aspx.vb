Imports System.IO
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class ProcesoContable
    Inherits System.Web.UI.Page

    ' ── Page Load ─────────────────────────────────────────────────────────────

    Private Sub ProcesoContable_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Name") Is vbNullString Then Response.Redirect("Default.aspx")
                Select Case Session("Position").ToString()
                    Case "Iniciador", "Supervisor Armado", "Calidad"
                        Response.Redirect("MainDashBoard.aspx")
                End Select
                lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString()
                IssueToken()
                _CargarContratistas()
            End If
        Catch ex As Exception
            Response.Write("Load: " & ex.Message)
        End Try
    End Sub

    Private Sub IssueToken()
        Dim token As String = Guid.NewGuid().ToString("N")
        Session("LiqToken") = token
        hfToken.Value = token
    End Sub

    Private Function ConsumeToken() As Boolean
        Dim stored As String = If(Session("LiqToken") IsNot Nothing, Session("LiqToken").ToString(), "")
        If String.IsNullOrEmpty(stored) OrElse stored <> hfToken.Value Then Return False
        Session.Remove("LiqToken")
        Return True
    End Function

    ' ── Contratistas dropdown ──────────────────────────────────────────────────

    Public Sub _CargarContratistas()
        Try
            Dim dt As DataTable = DbConfig.GetDataTable(
                "SELECT [GRUPORESPONSABLE], [ID] [BPCODE] FROM [ArmadoMotos].[dbo].[GRUPOS]",
                DbConfig.DBServer.ARMADOMOTOS)
            drpContratista.DataTextField = "GRUPORESPONSABLE"
            drpContratista.DataValueField = "BPCODE"
            drpContratista.DataSource = dt
            drpContratista.DataBind()
            BindGrid(drpContratista.SelectedValue.ToString())
        Catch ex As Exception
            Response.Write("_CargarContratistas: " & ex.Message)
        End Try
    End Sub

    Private Sub drpContratista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpContratista.SelectedIndexChanged
        gridPrecedentes.DataSource = Nothing
        gridPrecedentes.Visible = False
        BindGrid(drpContratista.SelectedValue.ToString())
    End Sub

    ' ── Grid: Pendientes ──────────────────────────────────────────────────────

    Private Sub BindGrid(_BPCODE As String)
        Try
            Dim sql As String =
                "SELECT t0.[ID], t0.[SERIE], t0.[MODELO], t0.[COLOR], t1.[ID] [Mecanico], t2.BPCODE, " &
                "T2.GRUPORESPONSABLE, '_SYS00000004238' [Cuenta], " &
                "(SELECT CONVERT(DECIMAL(19,2),U_Precio) FROM [movesa].[dbo].[@amodelo] " &
                " WHERE name=t0.[modelo] COLLATE Modern_Spanish_CI_AS) [Precio] " &
                "FROM [ArmadoMotos].[dbo].[ARMADOMOTOS] T0 " &
                "INNER JOIN [ArmadoMotos].[dbo].[mecanicos] T1 ON t0.MECANICOASIGNADO=t1.ID " &
                "INNER JOIN [ArmadoMotos].[dbo].[GRUPOS] T2 ON t1.GRUPOID=t2.ID " &
                "WHERE LIQUIDACIONID=0 AND T0.[CONTROL1]='ARMADA' AND T0.[CONTROL2]='PP' AND T2.[ID]=@bpcode " &
                "ORDER BY t0.[MODELO]"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@bpcode", _BPCODE)})

            ' KPI calculations
            Dim totalMotos As Integer = dt.Rows.Count
            Dim montoTotal As Decimal = 0
            Dim sinPrecio As Integer = 0
            For Each dr As DataRow In dt.Rows
                If IsDBNull(dr("Precio")) Then
                    sinPrecio += 1
                Else
                    montoTotal += Convert.ToDecimal(dr("Precio"))
                End If
            Next
            lblKpiTotal.Text = totalMotos.ToString()
            lblKpiMonto.Text = "L " & montoTotal.ToString("N2")
            lblKpiSinPrecio.Text = sinPrecio.ToString()
            hfSinPrecio.Value = sinPrecio.ToString()

            GridView1.DataSource = dt
            GridView1.DataBind()
            GridView1.UseAccessibleHeader = True
            If GridView1.HeaderRow IsNot Nothing Then
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
            If GridView1.FooterRow IsNot Nothing Then
                GridView1.FooterRow.Cells(1).Text = "Total: " & totalMotos.ToString()
                GridView1.FooterRow.Cells(1).Font.Bold = True
                GridView1.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                GridView1.FooterRow.Cells(7).Text = montoTotal.ToString("N2")
                GridView1.FooterRow.Cells(7).Font.Bold = True
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceChars(ex.Message) & "');</script>")
        End Try
    End Sub

    ' ── Grid: Precedentes ─────────────────────────────────────────────────────

    Protected Sub btnVerPrecendentes_Click(sender As Object, e As EventArgs) Handles btnVerPrecendentes.Click
        gridPrecedentes.Visible = True
        GridView1.Visible = False
        btnCrearLiquidacion.Visible = False
        drpContratista.Visible = False
        btnBack.Visible = True
        BindGridLiquidacionesPrecedentes(drpContratista.SelectedValue.ToString())
    End Sub

    Private Sub BindGridLiquidacionesPrecedentes(_BPCODE As String)
        Try
            Dim sql As String =
                "SELECT CONVERT(CHAR,t0.DATECREATED,103) [DATECREATED], t0.LIQUIDACIONID, t0.[SERIE], " &
                "t0.[MODELO], t0.[COLOR], T2.GRUPORESPONSABLE, t1.MECANICONAME " &
                "FROM [ArmadoMotos].[dbo].[ARMADOMOTOS] T0 " &
                "INNER JOIN [ArmadoMotos].[dbo].[mecanicos] T1 ON t0.MECANICOASIGNADO=t1.ID " &
                "INNER JOIN [ArmadoMotos].[dbo].[GRUPOS] T2 ON t1.GRUPOID=t2.ID " &
                "WHERE LIQUIDACIONID<>0 AND T2.[ID]=@bpcode " &
                "ORDER BY t0.LIQUIDACIONID, t0.[MODELO]"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@bpcode", _BPCODE)})
            gridPrecedentes.DataSource = dt
            gridPrecedentes.DataBind()
            gridPrecedentes.UseAccessibleHeader = True
            If gridPrecedentes.HeaderRow IsNot Nothing Then
                gridPrecedentes.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("BindGridLiquidacionesPrecedentes: " & ex.Message)
        End Try
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("ProcesoContable.aspx")
    End Sub

    ' ── Crear Liquidacion ─────────────────────────────────────────────────────

    Protected Sub btnCrearLiquidacion_Click(sender As Object, e As EventArgs) Handles btnCrearLiquidacion.Click
        Try
            ' Anti-duplicate: reject stale/duplicate submissions (F5 refresh or double-click)
            If Not ConsumeToken() Then Exit Sub

            If GridView1.Rows.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "warn",
                    "<script>Swal.fire({icon:'warning',title:'Sin motos',text:'No hay motos pendientes para liquidar.',confirmButtonText:'OK'});</script>", False)
                IssueToken()
                Exit Sub
            End If

            ' Bloqueo servidor: verificar que ninguna moto tenga precio vacío (NULL en BD)
            Dim sinPrecioSrv As Integer = 0
            For Each row As GridViewRow In GridView1.Rows
                If String.IsNullOrWhiteSpace(row.Cells(7).Text) OrElse row.Cells(7).Text = "&nbsp;" Then
                    sinPrecioSrv += 1
                End If
            Next
            If sinPrecioSrv > 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errPrecio",
                    "<script>Swal.fire({icon:'error',title:'No se puede crear la liquidacion',html:'<strong>" & sinPrecioSrv & "</strong> moto(s) sin precio de armado. Configure los precios antes de continuar.',confirmButtonText:'Entendido',confirmButtonColor:'#dc3545'});</script>", False)
                IssueToken()
                Exit Sub
            End If

            Dim montototal As Decimal = 0
            Dim cantidadtotal As Integer = 0
            For Each row As GridViewRow In GridView1.Rows
                Dim precioStr As String = row.Cells(7).Text.Replace(",", "").Trim()
                Dim precio As Decimal
                If Decimal.TryParse(precioStr, precio) Then montototal += precio
                cantidadtotal += 1
            Next

            Dim liquidacionId As Integer = CreateNewLiquidation(Date.Now, cantidadtotal, montototal, Session("UserCode").ToString())

            For Each row As GridViewRow In GridView1.Rows
                Dim precioStr As String = row.Cells(7).Text.Replace(",", "").Trim()
                UpdateOsrnSAP(row.Cells(1).Text, "04", "02")
                CreateNewLiquidationDetalle(liquidacionId,
                    row.Cells(0).Text, row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text,
                    row.Cells(4).Text, row.Cells(5).Text, drpContratista.SelectedItem.Text,
                    row.Cells(6).Text, precioStr, Date.Now, Session("UserCode").ToString())
                UpdateMaintableLiquidacion(liquidacionId, Date.Now, row.Cells(1).Text)
            Next

            Response.Redirect("GenerarPO.aspx")
        Catch ex As Exception
            IssueToken()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errLiq",
                "<script>Swal.fire({icon:'error',title:'Error al crear liquidacion',text:'" & ReplaceChars(ex.Message) & "',confirmButtonText:'OK'});</script>", False)
        End Try
    End Sub

    ' ── DB operations ─────────────────────────────────────────────────────────

    Public Function CreateNewLiquidation(_fecha As Date, _cantidad As Integer, _monto As Decimal, _usercode As String) As Integer
        Dim result As Object = DbConfig.ExecuteScalar(
            "INSERT INTO [dbo].[LIQUIDACIONES]([FECHACREACION],[CANTIDAD],[MONTO],[USERCODE]) " &
            "VALUES (@p1,@p2,@p3,@p4); SELECT CAST(SCOPE_IDENTITY() AS INT)",
            DbConfig.DBServer.ARMADOMOTOS,
            New List(Of SqlParameter) From {
                New SqlParameter("@p1", _fecha),
                New SqlParameter("@p2", _cantidad),
                New SqlParameter("@p3", _monto),
                New SqlParameter("@p4", _usercode)
            })
        Return Convert.ToInt32(result)
    End Function

    Public Sub CreateNewLiquidationDetalle(_LIQUIDACIONID As Integer, _IDCALIDAD As String, _SERIE As String,
                                           _MODELO As String, _COLOR As String, _MECANICOID As String,
                                           _CPROVEEDOR As String, _NPROVEEDOR As String, _CUENTAC As String,
                                           _PRECIOARMADO As String, _FECHACREACION As Date, _USUARIOCREACION As String)
        Try
            Dim precio As Decimal = 0
            Decimal.TryParse(_PRECIOARMADO, precio)
            DbConfig.ExecuteNonQuery(
                "INSERT INTO [dbo].[MOTOSLIQ]([LIQUIDACIONID],[IDCALIDAD],[SERIE],[MODELO],[COLOR],[MECANICOID]," &
                "[CPROVEEDOR],[NPROVEEDOR],[CUENTAC],[PRECIOARMADO],[FECHACREACION],[USUARIOCREACION]) " &
                "VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12)",
                DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@p1",  _LIQUIDACIONID),
                    New SqlParameter("@p2",  _IDCALIDAD),
                    New SqlParameter("@p3",  _SERIE),
                    New SqlParameter("@p4",  _MODELO),
                    New SqlParameter("@p5",  _COLOR),
                    New SqlParameter("@p6",  _MECANICOID),
                    New SqlParameter("@p7",  _CPROVEEDOR),
                    New SqlParameter("@p8",  _NPROVEEDOR),
                    New SqlParameter("@p9",  _CUENTAC),
                    New SqlParameter("@p10", precio),
                    New SqlParameter("@p11", _FECHACREACION),
                    New SqlParameter("@p12", _USUARIOCREACION)
                })
            If _CPROVEEDOR = "PL0000000" Then CloseLocalLiquidation(_LIQUIDACIONID.ToString())
        Catch ex As Exception
            Response.Write("CreateNewLiquidationDetalle: " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateMaintableLiquidacion(_LIQUIDACIONID As Integer, _FECHALIQUIDACION As Date, _SERIE As String)
        Try
            DbConfig.ExecuteNonQuery(
                "UPDATE [dbo].[ARMADOMOTOS] SET [LIQUIDACIONID]=@p1,[FECHALIQUIDACION]=@p2 WHERE [SERIE]=@p3",
                DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@p1", _LIQUIDACIONID),
                    New SqlParameter("@p2", _FECHALIQUIDACION),
                    New SqlParameter("@p3", _SERIE)
                })
        Catch ex As Exception
            Response.Write("UpdateMaintableLiquidacion: " & ex.Message)
        End Try
    End Sub

    Public Sub CloseLocalLiquidation(_LIQUIDACIONID As String)
        Try
            DbConfig.ExecuteNonQuery(
                "UPDATE LIQUIDACIONES SET CANCELED='Y' WHERE ID=@p1",
                DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@p1", _LIQUIDACIONID)})
        Catch ex As Exception
            Response.Write("CloseLocalLiquidation: " & ex.Message)
        End Try
    End Sub

    Public Sub UpdateOsrnSAP(_SERIE As String, _estadoproduccion As String, _estadopago As String)
        Try
            DbConfig.ExecuteNonQuery(
                "UPDATE OSRN SET U_Estado_Produccion=@p2, U_Estado_Contabilidad=@p3 WHERE MnfSerial=@p1",
                DbConfig.DBServer.MOVESA,
                New List(Of SqlParameter) From {
                    New SqlParameter("@p1", _SERIE),
                    New SqlParameter("@p2", _estadoproduccion),
                    New SqlParameter("@p3", _estadopago)
                })
        Catch ex As Exception
            Response.Write("UpdateOsrnSAP: " & ex.Message)
        End Try
    End Sub

    ' ── Excel export ──────────────────────────────────────────────────────────

    Private Sub ExportGridToExcel()
        Response.Clear()
        Response.Buffer = True
        Response.ClearContent()
        Response.ClearHeaders()
        Response.Charset = ""
        Dim fileName As String = "Proforma_" & drpContratista.SelectedItem.ToString() & "_" & DateTime.Now.ToString("yyyyMMdd") & ".xls"
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & fileName)
        GridView1.GridLines = GridLines.Both
        GridView1.HeaderStyle.Font.Bold = True
        GridView1.RenderControl(hw)
        Response.Write(sw.ToString())
        Response.[End]()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

    ' ── Utility ───────────────────────────────────────────────────────────────

    Private Function ReplaceChars(s As String) As String
        Dim chars() As String = {"/", "\", ":", "?", Chr(34), "<", ">", "|", "&", "%", "*", "'", "{", "[", "]", "}", "!", ","}
        For Each c As String In chars
            s = Replace(s, c, " ")
        Next
        Return s
    End Function

End Class

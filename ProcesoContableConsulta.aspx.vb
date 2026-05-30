Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class ProcesoContableConsulta
    Inherits System.Web.UI.Page

    ' ── Page Load ─────────────────────────────────────────────────────────────

    Private Sub ProcesoContableConsulta_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Name") Is vbNullString Then Response.Redirect("Default.aspx")
                Select Case Session("Position").ToString()
                    Case "Iniciador", "Supervisor Armado", "Calidad"
                        Response.Redirect("MainDashBoard.aspx")
                End Select
                lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString()
                _CargarContratistas()
                Dim desde As New Date(Date.Now.Year, Date.Now.Month, 1)
                Dim hasta As Date = Date.Now
                txtDesde.Text = desde.ToString("yyyy-MM-dd")
                txtHasta.Text = hasta.ToString("yyyy-MM-dd")
                BindGrid(drpContratista.SelectedValue.ToString(), desde, hasta)
            End If
        Catch ex As Exception
            Response.Write("Load: " & ex.Message)
        End Try
    End Sub

    ' ── Contratistas dropdown ──────────────────────────────────────────────────

    Public Sub _CargarContratistas()
        Try
            Dim dt As DataTable = DbConfig.GetDataTable(
                "SELECT [GRUPORESPONSABLE], [BPCODE] FROM [ArmadoMotos].[dbo].[GRUPOS] ORDER BY [GRUPORESPONSABLE]",
                DbConfig.DBServer.ARMADOMOTOS)
            drpContratista.DataTextField = "GRUPORESPONSABLE"
            drpContratista.DataValueField = "BPCODE"
            drpContratista.DataSource = dt
            drpContratista.DataBind()
            drpContratista.Items.Insert(0, New ListItem("(Todos los contratistas)", ""))
        Catch ex As Exception
            Response.Write("_CargarContratistas: " & ex.Message)
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

    Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        Dim fechas As Tuple(Of Date, Date) = GetFechas()
        BindGrid(drpContratista.SelectedValue.ToString(), fechas.Item1, fechas.Item2)
    End Sub

    ' ── BindGrid ──────────────────────────────────────────────────────────────

    Private Sub BindGrid(_BPCODE As String, desde As Date, hasta As Date)
        Try
            Dim sql As String =
                "SELECT [LIQUIDACIONID],[IDCALIDAD],[SERIE],[MODELO],[COLOR],[MECANICOID],[CPROVEEDOR]," &
                "[PRECIOARMADO], CONVERT(CHAR,FECHACREACION,103) [FECHACREACION] " &
                "FROM [ArmadoMotos].[dbo].[MOTOSLIQ] " &
                "WHERE FECHACREACION >= @desde AND FECHACREACION < DATEADD(DAY,1,@hasta) " &
                "AND (@bpcode = '' OR CPROVEEDOR = @bpcode) " &
                "ORDER BY FECHACREACION DESC, LIQUIDACIONID"

            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@desde",  desde.Date),
                    New SqlParameter("@hasta",  hasta.Date),
                    New SqlParameter("@bpcode", _BPCODE)})

            ' KPIs
            Dim totalMotos As Integer = dt.Rows.Count
            Dim totalMonto As Decimal = If(totalMotos > 0,
                dt.AsEnumerable().Sum(Function(r) If(IsDBNull(r("PRECIOARMADO")), 0D, Convert.ToDecimal(r("PRECIOARMADO")))), 0D)
            lblKpiMotos.Text = totalMotos.ToString()
            lblKpiMonto.Text = "L " & totalMonto.ToString("N2")
            lblRango.Text = "Del " & desde.ToString("dd/MM/yyyy") & " al " & hasta.ToString("dd/MM/yyyy")

            GridView1.DataSource = dt
            GridView1.DataBind()
            GridView1.UseAccessibleHeader = True
            If GridView1.HeaderRow IsNot Nothing Then
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
            If GridView1.FooterRow IsNot Nothing Then
                GridView1.FooterRow.Cells(1).Text = "Total: " & totalMotos.ToString()
                GridView1.FooterRow.Cells(1).Font.Bold = True
                GridView1.FooterRow.Cells(8).Text = totalMonto.ToString("N2")
                GridView1.FooterRow.Cells(8).Font.Bold = True
                GridView1.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ex.Message.Replace("'", " ").Replace(Chr(34), " ") & "');</script>")
        End Try
    End Sub

End Class

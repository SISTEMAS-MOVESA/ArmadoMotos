Imports System.Data
Imports System.Data.SqlClient

Partial Class VentasU7DKpi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Defecto: inicio y fin del mes actual
            Dim hoy As Date = Date.Today
            txtDesde.Text = New Date(hoy.Year, hoy.Month, 1).ToString("yyyy-MM-dd")
            txtHasta.Text = New Date(hoy.Year, hoy.Month, Date.DaysInMonth(hoy.Year, hoy.Month)).ToString("yyyy-MM-dd")
            CargarTodo()
        End If
    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs)
        CargarTodo()
    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs)
        CargarTodo()
    End Sub

    Private Function GetFechas() As Tuple(Of Date, Date)
        Dim hoy As Date = Date.Today
        Dim desde As Date = New Date(hoy.Year, hoy.Month, 1)
        Dim hasta As Date = New Date(hoy.Year, hoy.Month, Date.DaysInMonth(hoy.Year, hoy.Month))
        If Not Date.TryParse(txtDesde.Text, desde) Then desde = New Date(hoy.Year, hoy.Month, 1)
        If Not Date.TryParse(txtHasta.Text, hasta) Then hasta = New Date(hoy.Year, hoy.Month, Date.DaysInMonth(hoy.Year, hoy.Month))
        If desde > hasta Then desde = hasta
        Return Tuple.Create(desde, hasta)
    End Function

    Private Sub CargarTodo()
        Dim rng As Tuple(Of Date, Date) = GetFechas()
        Dim desde As Date = rng.Item1
        Dim hasta As Date = rng.Item2
        ' Actualiza labels de rango
        lblRango.Text = desde.ToString("dd/MM/yyyy") & " al " & hasta.ToString("dd/MM/yyyy")
        CargarKPIs(desde, hasta)
        CargarPorNota(desde, hasta)
        CargarSinNota(desde, hasta)
        CargarDetallePendientes(desde, hasta)
        CargarResumenPareto(desde, hasta)
        lblActualizado.Text = "Actualizado: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
    End Sub

    ' -----------------------------------------------------------------------
    ' KPI Summary
    ' -----------------------------------------------------------------------
    Private Sub CargarKPIs(desde As Date, hasta As Date)
        Dim sql As String =
            "SELECT" &
            " COUNT(*) AS Total," &
            " ISNULL(SUM(CASE WHEN ESTADO COLLATE DATABASE_DEFAULT='RESUELTO' THEN 1 ELSE 0 END), 0) AS Resueltas," &
            " ISNULL(SUM(CASE WHEN ISNULL(ESTADO COLLATE DATABASE_DEFAULT,'PENDIENTE')<>'RESUELTO' THEN 1 ELSE 0 END), 0) AS Pendientes," &
            " ISNULL(SUM(CASE WHEN ISNULL(ESTADO COLLATE DATABASE_DEFAULT,'PENDIENTE')<>'RESUELTO' AND NULLIF(RTRIM(NOTA),'') IS NULL THEN 1 ELSE 0 END), 0) AS SinNota," &
            " COUNT(DISTINCT NULLIF(RTRIM(NOTA),'')) AS NotasUsadas" &
            " FROM ArmadoMotos.dbo.VENTAS_U7D_CONTROL" &
            " WHERE CAST(FECHA_REGISTRO AS DATE) BETWEEN @desde AND @hasta"

        Dim params As New List(Of SqlParameter) From {
            New SqlParameter("@desde", desde),
            New SqlParameter("@hasta", hasta)
        }
        Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA, params)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return

        Dim row As DataRow = dt.Rows(0)
        Dim total As Integer = CInt(row("Total"))
        Dim res As Integer = CInt(row("Resueltas"))
        Dim pend As Integer = CInt(row("Pendientes"))
        Dim sinNota As Integer = CInt(row("SinNota"))
        Dim notasUsadas As Integer = CInt(row("NotasUsadas"))
        Dim pct As Double = If(total > 0, Math.Round(res * 100.0 / total, 1), 0)

        lblTotal.Text = total.ToString()
        lblResueltas.Text = res.ToString()
        lblPendientes.Text = pend.ToString()
        lblCobertura.Text = pct.ToString("N1")
        lblNotasUsadas.Text = notasUsadas.ToString()
        lblSinNota.Text = sinNota.ToString()
        lblBadgePend.Text = pend.ToString()
        lblBadgeSinNota.Text = sinNota.ToString()

        progCobertura.Style("width") = pct.ToString("N0") & "%"
        If pct >= 80 Then
            progCobertura.Style("background-color") = "#27ae60"
        ElseIf pct >= 50 Then
            progCobertura.Style("background-color") = "#f39c12"
        Else
            progCobertura.Style("background-color") = "#e74c3c"
        End If
    End Sub

    ' -----------------------------------------------------------------------
    ' Por Nota
    ' -----------------------------------------------------------------------
    Private Sub CargarPorNota(desde As Date, hasta As Date)
        Dim sql As String =
            "SELECT" &
            " ISNULL(NULLIF(RTRIM(v.NOTA),''), '(Sin nota)') AS Nota," &
            " COUNT(*) AS Total," &
            " SUM(CASE WHEN v.ESTADO COLLATE DATABASE_DEFAULT='RESUELTO' THEN 1 ELSE 0 END) AS Resueltas," &
            " SUM(CASE WHEN ISNULL(v.ESTADO COLLATE DATABASE_DEFAULT,'PENDIENTE')<>'RESUELTO' THEN 1 ELSE 0 END) AS Pendientes," &
            " CAST(ROUND(" &
            "   SUM(CASE WHEN v.ESTADO COLLATE DATABASE_DEFAULT='RESUELTO' THEN 1.0 ELSE 0 END) / NULLIF(COUNT(*),0) * 100," &
            "   1) AS DECIMAL(5,1)) AS PctOk" &
            " FROM ArmadoMotos.dbo.VENTAS_U7D_CONTROL v" &
            " WHERE CAST(v.FECHA_REGISTRO AS DATE) BETWEEN @desde AND @hasta" &
            " GROUP BY NULLIF(RTRIM(v.NOTA),'')" &
            " ORDER BY PctOk DESC, Total DESC"

        Dim params As New List(Of SqlParameter) From {
            New SqlParameter("@desde", desde),
            New SqlParameter("@hasta", hasta)
        }
        gvNota.DataSource = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA, params)
        gvNota.DataBind()
    End Sub

    Protected Sub gvNota_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then Return
        Dim row As DataRowView = DirectCast(e.Row.DataItem, DataRowView)

        Dim nota As String = row("Nota").ToString()
        If nota = "(Sin nota)" Then e.Row.Cells(0).CssClass = "nota-sin"

        Dim pend As Integer = Convert.ToInt32(row("Pendientes"))
        If pend > 0 Then e.Row.Cells(3).CssClass = "pend-hi"

        Dim pct As Double = Convert.ToDouble(row("PctOk"))
        If pct >= 80 Then
            e.Row.Cells(4).CssClass = "pct-ok"
        ElseIf pct >= 50 Then
            e.Row.Cells(4).CssClass = "pct-warn"
        Else
            e.Row.Cells(4).CssClass = "pct-bad"
        End If

        Dim bar As System.Web.UI.HtmlControls.HtmlGenericControl =
            DirectCast(e.Row.FindControl("barNota"), System.Web.UI.HtmlControls.HtmlGenericControl)
        If bar IsNot Nothing Then
            bar.Style("width") = pct.ToString("N0") & "%"
            If pct >= 80 Then
                bar.Style("background-color") = "#27ae60"
            ElseIf pct >= 50 Then
                bar.Style("background-color") = "#f39c12"
            Else
                bar.Style("background-color") = "#e74c3c"
            End If
        End If
    End Sub

    ' -----------------------------------------------------------------------
    ' Sin Nota (pendientes sin nota)
    ' -----------------------------------------------------------------------
    Private Sub CargarSinNota(desde As Date, hasta As Date)
        Dim sql As String =
            "SELECT" &
            " v.DOCNUM AS Factura," &
            " ISNULL(w.WhsName, v.WHSCODE) AS Almacen," &
            " ISNULL(m.Name, v.COD_MODELO) AS Modelo," &
            " ISNULL(CONVERT(VARCHAR(10), v.FECHA_FACTURA, 103), '') AS FechaVenta," &
            " DATEDIFF(DAY, v.FECHA_REGISTRO, GETDATE()) AS DiasAbierto," &
            " ISNULL((" &
            "   SELECT CONVERT(INT, SUM(w2.OnHand))" &
            "   FROM MOVESA..OITW w2 WITH(NOLOCK)" &
            "   JOIN MOVESA..OITM i2 WITH(NOLOCK) ON w2.ItemCode = i2.ItemCode" &
            "   WHERE i2.U_MODELO = v.COD_MODELO COLLATE DATABASE_DEFAULT AND i2.ItmsGrpCod = 154 AND w2.WhsCode = 'DCM00'" &
            " ), 0) AS DCM00," &
            " ISNULL((" &
            "   SELECT CONVERT(INT, SUM(w2.OnHand))" &
            "   FROM MOVESA..OITW w2 WITH(NOLOCK)" &
            "   JOIN MOVESA..OITM i2 WITH(NOLOCK) ON w2.ItemCode = i2.ItemCode" &
            "   WHERE i2.U_MODELO = v.COD_MODELO COLLATE DATABASE_DEFAULT AND i2.ItmsGrpCod = 154 AND w2.WhsCode = v.WHSCODE COLLATE DATABASE_DEFAULT" &
            " ), 0) AS StockSuc" &
            " FROM ArmadoMotos.dbo.VENTAS_U7D_CONTROL v" &
            " LEFT JOIN MOVESA..OWHS w WITH(NOLOCK) ON w.WhsCode = v.WHSCODE COLLATE DATABASE_DEFAULT" &
            " LEFT JOIN MOVESA..[@AMODELO] m WITH(NOLOCK) ON m.Code = v.COD_MODELO COLLATE DATABASE_DEFAULT" &
            " WHERE ISNULL(v.ESTADO COLLATE DATABASE_DEFAULT,'PENDIENTE') <> 'RESUELTO'" &
            "   AND NULLIF(RTRIM(v.NOTA),'') IS NULL" &
            "   AND CAST(v.FECHA_REGISTRO AS DATE) BETWEEN @desde AND @hasta" &
            " ORDER BY v.FECHA_FACTURA ASC, m.Name"

        Dim params As New List(Of SqlParameter) From {
            New SqlParameter("@desde", desde),
            New SqlParameter("@hasta", hasta)
        }
        gvSinNota.DataSource = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA, params)
        gvSinNota.DataBind()
    End Sub

    Protected Sub gvSinNota_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then Return
        Dim row As DataRowView = DirectCast(e.Row.DataItem, DataRowView)

        Dim dias As Integer = Convert.ToInt32(row("DiasAbierto"))
        If dias >= 2 Then
            e.Row.CssClass = "dias-2"
        ElseIf dias = 1 Then
            e.Row.CssClass = "dias-1"
        End If

        Dim dcm As Integer = Convert.ToInt32(row("DCM00"))
        e.Row.Cells(5).CssClass = If(dcm > 0, "dcm-ok", "dcm-zero")
    End Sub

    ' -----------------------------------------------------------------------
    ' Detalle Pendientes
    ' -----------------------------------------------------------------------
    Private Sub CargarDetallePendientes(desde As Date, hasta As Date)
        Dim sql As String =
            "SELECT" &
            " v.DOCNUM AS Factura," &
            " ISNULL(w.WhsName, v.WHSCODE) AS Almacen," &
            " ISNULL(m.Name, v.COD_MODELO) AS Modelo," &
            " ISNULL(CONVERT(VARCHAR(10), v.FECHA_FACTURA, 103), '') AS FechaVenta," &
            " DATEDIFF(DAY, v.FECHA_REGISTRO, GETDATE()) AS DiasAbierto," &
            " ISNULL((" &
            "   SELECT CONVERT(INT, SUM(w2.OnHand))" &
            "   FROM MOVESA..OITW w2 WITH(NOLOCK)" &
            "   JOIN MOVESA..OITM i2 WITH(NOLOCK) ON w2.ItemCode = i2.ItemCode" &
            "   WHERE i2.U_MODELO = v.COD_MODELO COLLATE DATABASE_DEFAULT AND i2.ItmsGrpCod = 154 AND w2.WhsCode = 'DCM00'" &
            " ), 0) AS DCM00," &
            " ISNULL((" &
            "   SELECT CONVERT(INT, SUM(w2.OnHand))" &
            "   FROM MOVESA..OITW w2 WITH(NOLOCK)" &
            "   JOIN MOVESA..OITM i2 WITH(NOLOCK) ON w2.ItemCode = i2.ItemCode" &
            "   WHERE i2.U_MODELO = v.COD_MODELO COLLATE DATABASE_DEFAULT AND i2.ItmsGrpCod = 154 AND w2.WhsCode = v.WHSCODE COLLATE DATABASE_DEFAULT" &
            " ), 0) AS StockSuc," &
            " ISNULL((" &
            "   SELECT CONVERT(INT, SUM(w2.OnHand))" &
            "   FROM MOVESA..OITW w2 WITH(NOLOCK)" &
            "   JOIN MOVESA..OITM i2 WITH(NOLOCK) ON w2.ItemCode = i2.ItemCode" &
            "   WHERE i2.U_MODELO = v.COD_MODELO COLLATE DATABASE_DEFAULT AND i2.ItmsGrpCod = 154" &
            "     AND w2.WhsCode = 'T' + RTRIM(v.WHSCODE COLLATE DATABASE_DEFAULT)" &
            " ), 0) AS Transito," &
            " ISNULL(v.NOTA, '') AS Nota" &
            " FROM ArmadoMotos.dbo.VENTAS_U7D_CONTROL v" &
            " LEFT JOIN MOVESA..OWHS w WITH(NOLOCK) ON w.WhsCode = v.WHSCODE COLLATE DATABASE_DEFAULT" &
            " LEFT JOIN MOVESA..[@AMODELO] m WITH(NOLOCK) ON m.Code = v.COD_MODELO COLLATE DATABASE_DEFAULT" &
            " WHERE ISNULL(v.ESTADO COLLATE DATABASE_DEFAULT,'PENDIENTE') <> 'RESUELTO'" &
            "   AND CAST(v.FECHA_REGISTRO AS DATE) BETWEEN @desde AND @hasta" &
            " ORDER BY NULLIF(RTRIM(v.NOTA),'') DESC, v.FECHA_FACTURA ASC"

        Dim params As New List(Of SqlParameter) From {
            New SqlParameter("@desde", desde),
            New SqlParameter("@hasta", hasta)
        }
        gvDetalle.DataSource = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA, params)
        gvDetalle.DataBind()
    End Sub

    Protected Sub gvDetalle_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then Return
        Dim row As DataRowView = DirectCast(e.Row.DataItem, DataRowView)

        Dim dias As Integer = Convert.ToInt32(row("DiasAbierto"))
        If dias >= 2 Then
            e.Row.CssClass = "dias-2"
        ElseIf dias = 1 Then
            e.Row.CssClass = "dias-1"
        End If

        Dim nota As String = row("Nota").ToString().Trim()
        If nota = "" Then e.Row.Cells(8).CssClass = "nota-sin"

        Dim dcm As Integer = Convert.ToInt32(row("DCM00"))
        e.Row.Cells(5).CssClass = If(dcm > 0, "dcm-ok", "dcm-zero")
    End Sub

    ' -----------------------------------------------------------------------
    ' Resumen por Rango Pareto
    ' -----------------------------------------------------------------------
    Private Sub CargarResumenPareto(desde As Date, hasta As Date)
        ' Lee PARETO_PCT guardado en VENTAS_U7D_CONTROL.
        ' Incluye 0% al final; excluye NULL (registros sin pareto capturado).
        Dim sql As String =
            "SELECT" &
            "  CASE" &
            "    WHEN PARETO_PCT = 0    THEN '0%'" &
            "    WHEN PARETO_PCT <= 40  THEN '1-40%'" &
            "    WHEN PARETO_PCT <= 60  THEN '41-60%'" &
            "    WHEN PARETO_PCT <= 80  THEN '61-80%'" &
            "    ELSE                       '81-99%'" &
            "  END AS Rango," &
            "  COUNT(*) AS Cantidad," &
            "  SUM(CASE WHEN ISNULL(ESTADO,'') = 'RESUELTO' THEN 1 ELSE 0 END) AS Resueltas," &
            "  SUM(CASE WHEN ISNULL(ESTADO,'PENDIENTE') <> 'RESUELTO' THEN 1 ELSE 0 END) AS Pendientes," &
            "  CAST(COUNT(*) * 100.0 / NULLIF(SUM(COUNT(*)) OVER(), 0) AS DECIMAL(5,1)) AS PctTotal," &
            "  CASE" &
            "    WHEN MIN(PARETO_PCT) = 0    THEN 5" &
            "    WHEN MIN(PARETO_PCT) <= 40  THEN 1" &
            "    WHEN MIN(PARETO_PCT) <= 60  THEN 2" &
            "    WHEN MIN(PARETO_PCT) <= 80  THEN 3" &
            "    ELSE 4" &
            "  END AS Orden" &
            " FROM VENTAS_U7D_CONTROL" &
            " WHERE CAST(FECHA_REGISTRO AS DATE) BETWEEN @desde AND @hasta" &
            "   AND PARETO_PCT IS NOT NULL" &
            " GROUP BY" &
            "  CASE WHEN PARETO_PCT=0 THEN '0%' WHEN PARETO_PCT<=40 THEN '1-40%' WHEN PARETO_PCT<=60 THEN '41-60%' WHEN PARETO_PCT<=80 THEN '61-80%' ELSE '81-99%' END" &
            " ORDER BY" &
            "  CASE WHEN MIN(PARETO_PCT)=0 THEN 5 WHEN MIN(PARETO_PCT)<=40 THEN 1 WHEN MIN(PARETO_PCT)<=60 THEN 2 WHEN MIN(PARETO_PCT)<=80 THEN 3 ELSE 4 END"

        Dim params As New List(Of SqlParameter) From {
            New SqlParameter("@desde", desde),
            New SqlParameter("@hasta", hasta)
        }
        gvPareto.DataSource = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS, params)
        gvPareto.DataBind()
    End Sub

    Protected Sub gvPareto_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then Return
        Dim row As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Dim orden As Integer = Convert.ToInt32(row("Orden"))
        Dim cssClass As String
        Select Case orden
            Case 1 : cssClass = "pareto-1"
            Case 2 : cssClass = "pareto-2"
            Case 3 : cssClass = "pareto-3"
            Case 4 : cssClass = "pareto-4"
            Case Else : cssClass = "pareto-0"
        End Select
        e.Row.Cells(0).CssClass = cssClass
        Dim pend As Integer = Convert.ToInt32(row("Pendientes"))
        If pend > 0 Then e.Row.Cells(3).CssClass = "pct-bad"
    End Sub

End Class

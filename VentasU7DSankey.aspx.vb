Imports System.Data
Imports System.Text

Partial Class VentasU7DSankey
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDesde.Text = New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("yyyy-MM-dd")
            txtHasta.Text = DateTime.Today.ToString("yyyy-MM-dd")
        End If
        CargarDatos()
        lblActualizado.Text = "Actualizado: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub CargarDatos()
        Dim dDesde As DateTime, dHasta As DateTime
        If Not DateTime.TryParse(txtDesde.Text.Trim(), dDesde) Then
            dDesde = New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
            txtDesde.Text = dDesde.ToString("yyyy-MM-dd")
        End If
        If Not DateTime.TryParse(txtHasta.Text.Trim(), dHasta) Then
            dHasta = DateTime.Today
            txtHasta.Text = dHasta.ToString("yyyy-MM-dd")
        End If

        lblRango.Text = dDesde.ToString("dd/MM/yyyy") & " &#8211; " & dHasta.ToString("dd/MM/yyyy")

        Dim filtroFecha As String =
            " v.FECHA_FACTURA >= '" & dDesde.ToString("yyyy-MM-dd") & "'" &
            " AND v.FECHA_FACTURA <= '" & dHasta.ToString("yyyy-MM-dd") & "'"

        Dim sb As New StringBuilder()
        sb.AppendLine("<script type=""text/javascript"">")

        ' --- Por Modelo ---
        Dim sqlModelo As String =
            "WITH Base AS (" &
            " SELECT ISNULL(m.Name, v.COD_MODELO) AS Modelo," &
            "  ISNULL(NULLIF(RTRIM(v.NOTA),''), '(Sin nota)') AS Nota," &
            "  COUNT(*) AS Cantidad" &
            " FROM ArmadoMotos.dbo.VENTAS_U7D_CONTROL v" &
            " LEFT JOIN MOVESA..[@AMODELO] m WITH(NOLOCK) ON m.Code = v.COD_MODELO COLLATE DATABASE_DEFAULT" &
            " WHERE " & filtroFecha & " {WHERE_ESTADO}" &
            " GROUP BY v.COD_MODELO, m.Name, NULLIF(RTRIM(v.NOTA),'')" &
            " HAVING COUNT(*) > 0" &
            ")," &
            " MT AS (SELECT Modelo, SUM(Cantidad) AS Tot FROM Base GROUP BY Modelo)," &
            " NT AS (SELECT Nota,   SUM(Cantidad) AS Tot FROM Base GROUP BY Nota)" &
            " SELECT b.Modelo, b.Nota, b.Cantidad FROM Base b" &
            " JOIN MT ON MT.Modelo = b.Modelo JOIN NT ON NT.Nota = b.Nota" &
            " ORDER BY NT.Tot DESC, MT.Tot DESC"

        sb.AppendLine("window._sankey = {")
        sb.Append("  todos: ")      : sb.AppendLine(EjecutarYSerializar(sqlModelo, "") & ",")
        sb.Append("  resueltos: ")  : sb.AppendLine(EjecutarYSerializar(sqlModelo, "AND v.ESTADO COLLATE DATABASE_DEFAULT = 'RESUELTO'") & ",")
        sb.Append("  pendientes: ") : sb.AppendLine(EjecutarYSerializar(sqlModelo, "AND ISNULL(v.ESTADO COLLATE DATABASE_DEFAULT,'PENDIENTE') <> 'RESUELTO'"))
        sb.AppendLine("};")

        ' --- Por Almacen ---
        Dim sqlAlm As String =
            "WITH Base AS (" &
            " SELECT ISNULL(w.WhsName, v.WHSCODE) AS Modelo," &
            "  ISNULL(NULLIF(RTRIM(v.NOTA),''), '(Sin nota)') AS Nota," &
            "  COUNT(*) AS Cantidad" &
            " FROM ArmadoMotos.dbo.VENTAS_U7D_CONTROL v" &
            " LEFT JOIN MOVESA..OWHS w WITH(NOLOCK) ON w.WhsCode = v.WHSCODE COLLATE DATABASE_DEFAULT" &
            " WHERE " & filtroFecha & " {WHERE_ESTADO}" &
            " GROUP BY v.WHSCODE, w.WhsName, NULLIF(RTRIM(v.NOTA),'')" &
            " HAVING COUNT(*) > 0" &
            ")," &
            " MT AS (SELECT Modelo, SUM(Cantidad) AS Tot FROM Base GROUP BY Modelo)," &
            " NT AS (SELECT Nota,   SUM(Cantidad) AS Tot FROM Base GROUP BY Nota)" &
            " SELECT b.Modelo, b.Nota, b.Cantidad FROM Base b" &
            " JOIN MT ON MT.Modelo = b.Modelo JOIN NT ON NT.Nota = b.Nota" &
            " ORDER BY NT.Tot DESC, MT.Tot DESC"

        sb.AppendLine("window._sankeyAlm = {")
        sb.Append("  todos: ")      : sb.AppendLine(EjecutarYSerializar(sqlAlm, "") & ",")
        sb.Append("  resueltos: ")  : sb.AppendLine(EjecutarYSerializar(sqlAlm, "AND v.ESTADO COLLATE DATABASE_DEFAULT = 'RESUELTO'") & ",")
        sb.Append("  pendientes: ") : sb.AppendLine(EjecutarYSerializar(sqlAlm, "AND ISNULL(v.ESTADO COLLATE DATABASE_DEFAULT,'PENDIENTE') <> 'RESUELTO'"))
        sb.AppendLine("};")

        sb.AppendLine("</script>")
        litData.Text = sb.ToString()
    End Sub

    Private Function EjecutarYSerializar(sqlBase As String, estadoFiltro As String) As String
        Dim sql As String = sqlBase.Replace("{WHERE_ESTADO}", estadoFiltro)
        Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA)
        Return SerializarFilas(dt)
    End Function

    Private Function SerializarFilas(dt As DataTable) As String
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return "[]"
        Dim sb As New StringBuilder()
        sb.Append("[")
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim row As DataRow = dt.Rows(i)
            Dim modelo As String = EscapeJs(row("Modelo").ToString())
            Dim nota   As String = EscapeJs(row("Nota").ToString())
            Dim cant   As Integer = Convert.ToInt32(row("Cantidad"))
            If i > 0 Then sb.Append(",")
            sb.AppendFormat("[""{0}"",""{1}"",{2}]", modelo, nota, cant)
        Next
        sb.Append("]")
        Return sb.ToString()
    End Function

    Private Function EscapeJs(s As String) As String
        Return s.Replace("\", "\\").Replace("""", "\""").Replace(vbCr, "").Replace(vbLf, " ")
    End Function

End Class

Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq

' Motor CBv3 — Algoritmo de 27 pasos según CB_Optimo_v3_SX1_SUC02.xlsx
Partial Class TrasladosCuadroBasico
    Inherits System.Web.UI.Page

    ' ═══════════════════════════════════════════════════════
    ' Parámetros CBv3 (CB_Optimo_v3_SX1_SUC02.xlsx — Hoja Variables)
    ' ═══════════════════════════════════════════════════════
    Private Const W1          As Decimal = 2.0D
    Private Const W2          As Decimal = 1.5D
    Private Const W3          As Decimal = 1.0D
    Private Const LT_DAYS     As Integer = 3
    Private Const GROWTH_TH   As Decimal = 1.2D
    Private Const DECLINE_TH  As Decimal = 0.8D
    Private Const GROWTH_ADJ  As Decimal = 1.1D
    Private Const DECLINE_ADJ As Decimal = 0.9D
    Private Const SEASON_FLR  As Decimal = 1.0D

    Private Const PARETO_ORO    As Decimal = 0.60D
    Private Const PARETO_PLATA  As Decimal = 0.80D
    Private Const PARETO_BRONCE As Decimal = 0.95D

    Private ReadOnly ZByTier As New Dictionary(Of String, Decimal)(StringComparer.OrdinalIgnoreCase) From {
        {"ORO", 1.645D}, {"PLATA", 1.28D}, {"BRONCE", 1.04D}, {"BAJO", 0.84D}
    }
    Private ReadOnly TFByTier As New Dictionary(Of String, Decimal)(StringComparer.OrdinalIgnoreCase) From {
        {"ORO", 1.0D}, {"PLATA", 1.0D}, {"BRONCE", 0.85D}, {"BAJO", 0.5D}
    }
    Private ReadOnly CycleByStore As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase) From {
        {"A", 5}, {"B", 7}, {"C", 10}
    }
    Private ReadOnly CbMinMap As New Dictionary(Of String, Dictionary(Of String, Integer))(StringComparer.OrdinalIgnoreCase) From {
        {"A", New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase) From {{"ORO", 2}, {"PLATA", 1}, {"BRONCE", 1}, {"BAJO", 0}}},
        {"B", New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase) From {{"ORO", 1}, {"PLATA", 1}, {"BRONCE", 0}, {"BAJO", 0}}},
        {"C", New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase) From {{"ORO", 1}, {"PLATA", 0}, {"BRONCE", 0}, {"BAJO", 0}}}
    }

    ' ═══════════════════════════════════════════════════════
    ' Par modelo-demanda para cálculo Pareto
    ' ═══════════════════════════════════════════════════════
    Private Structure ModelDemanda
        Dim Modelo  As String
        Dim Demanda As Decimal
        Sub New(m As String, d As Decimal)
            Modelo = m : Demanda = d
        End Sub
    End Structure

    ' ═══════════════════════════════════════════════════════
    ' Estructura resultado CBv3 (27 pasos)
    ' ═══════════════════════════════════════════════════════
    Private Structure CBv3Result
        Dim Prom1a4     As Decimal
        Dim Prom5a8     As Decimal
        Dim Prom9a12    As Decimal
        Dim DPS         As Decimal
        Dim VtaDiaBase  As Decimal
        Dim IdxTend     As Decimal
        Dim TrendDir    As String
        Dim AjTend      As Decimal
        Dim VtaDiaTend  As Decimal
        Dim FEstFinal   As Decimal
        Dim VtaDiaFinal As Decimal
        Dim Sigma       As Decimal
        Dim DCiclo      As Decimal
        Dim DLT         As Decimal
        Dim SS          As Decimal
        Dim CBBase      As Decimal
        Dim CBxTier     As Decimal
        Dim CB_Propuesto As Integer
        Dim Deficit     As Integer
        Dim CobDias     As Decimal
        Dim Accion      As String
        Dim Estado      As String
    End Structure

    ' ═══════════════════════════════════════════════════════
    ' Page_Load
    ' ═══════════════════════════════════════════════════════
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Session("Name") Is Nothing OrElse Session("Name").ToString() = "" Then
                Response.Redirect("Default.aspx") : Exit Sub
            End If
            If Not IsPostBack Then
                CargarSucursales()
                Dim whs As String = Request.QueryString("whscode")
                If Not String.IsNullOrEmpty(whs) Then
                    drpSucursales.SelectedValue = whs
                    BindGrid(whs)
                End If
            End If
        Catch ex As Exception
            Response.Write("<script>console.error(" & Q(ex.Message) & ");</script>")
        End Try
    End Sub

    Private Sub CargarSucursales()
        Dim q As String =
            "SELECT WhsName, WhsCode FROM OWHS WITH(NOLOCK) " &
            "WHERE WhsCode NOT LIKE 'TSUC%' " &
            "AND WhsCode NOT IN ('BPD01','CDM00','CDR00','CDR01','CONCA01','Consumir','DCM00','DCR00'," &
            "'DCR01','DEV01','DEV02','EST001','FCE01','MAY01','MLCB01','MLCB02','MOB01','MSPS01','MSPS02'," &
            "'MSPS03','MSPS04','MSUC01','MSUC02','MSUC03','MSUC04','MSUC05','MSUC07','MTGA01','MTRANS00'," &
            "'RLCB02','RLCB03','RSPS02','RSPS03','RSPS04','RSPS05','RSPS06','RSPS07','RTGA01'," &
            "'RTGA0102','SUC0102','SUC0201','SUC03','SUC0301','SUC0401','SUC0701','SUC0801','SUM001') " &
            "ORDER BY WhsName"
        Dim dt As DataTable = DbConfig.GetDataTable(q, DbConfig.DBServer.MOVESA)
        drpSucursales.DataTextField = "WhsName"
        drpSucursales.DataValueField = "WhsCode"
        drpSucursales.DataSource = dt
        drpSucursales.DataBind()
    End Sub

    ' ═══════════════════════════════════════════════════════
    ' Orquestación principal
    ' ═══════════════════════════════════════════════════════
    Private Sub BindGrid(whscode As String)
        Try
            Dim storeTier As String = ObtenerStoreTier(whscode)
            Dim paramsSP As New List(Of SqlParameter) From {
                New SqlParameter("@WHSCODE", whscode),
                New SqlParameter("@COBERTURA_DIAS", 7),
                New SqlParameter("@MUERTO_DIAS", 90)
            }
            Dim dtBase As DataTable = DbConfig.ExecuteStoredProcedure(
                "[dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS_V2]",
                DbConfig.DBServer.ARMADOMOTOS, paramsSP)
            Dim dtSemanas As DataTable = CargarVentasSemanales(whscode)
            Dim dt As DataTable = BuildResultTable(dtBase, dtSemanas, storeTier)
            gridV3.DataSource = dt
            gridV3.DataBind()
            ActualizarMetricas(dt)
        Catch ex As Exception
            Response.Write("<script>console.error(" & Q(ex.Message) & ");</script>")
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════
    ' Ventas semanales S1..S12 — inline SQL
    ' ═══════════════════════════════════════════════════════
    Private Function CargarVentasSemanales(whscode As String) As DataTable
        Dim hoy     As Date    = Date.Today
        Dim dow     As Integer = (CInt(hoy.DayOfWeek) + 6) Mod 7
        Dim s1Start As Date    = hoy.AddDays(-dow - 7)
        Dim s1End   As Date    = s1Start.AddDays(6)
        Dim s12Start As Date   = s1Start.AddDays(-11 * 7)

        Dim sb As New System.Text.StringBuilder()
        sb.Append("SELECT x.MODELO")
        Dim p As New List(Of SqlParameter)
        p.Add(New SqlParameter("@whs", whscode))
        p.Add(New SqlParameter("@FI", s12Start))
        p.Add(New SqlParameter("@FF", s1End))

        For i As Integer = 1 To 12
            Dim ws As Date = s1Start.AddDays(-(i - 1) * 7)
            Dim we As Date = ws.AddDays(6)
            sb.Append(", SUM(CASE WHEN x.DocDate BETWEEN @S" & i & "s AND @S" & i & "e THEN x.qty ELSE 0 END) AS S" & i)
            p.Add(New SqlParameter("@S" & i & "s", ws))
            p.Add(New SqlParameter("@S" & i & "e", we))
        Next

        sb.Append(" FROM (")
        sb.Append("  SELECT T5.Name AS MODELO, T0.DocDate, T1.Quantity AS qty")
        sb.Append("  FROM MOVESA..OINV T0 WITH(NOLOCK)")
        sb.Append("  INNER JOIN MOVESA..INV1 T1 WITH(NOLOCK) ON T0.DocEntry=T1.DocEntry")
        sb.Append("  INNER JOIN MOVESA..OITM T2 WITH(NOLOCK) ON T2.ItemCode=T1.ItemCode")
        sb.Append("  LEFT  JOIN MOVESA..[@AMODELO] T5 WITH(NOLOCK) ON T2.U_MODELO=T5.Code")
        sb.Append("  WHERE T0.DocDate BETWEEN @FI AND @FF")
        sb.Append("    AND T2.ItmsGrpCod=154 AND T0.CANCELED='N' AND T1.WhsCode=@whs")
        sb.Append("    AND ISNULL(T5.u_lg5,'-')='Y'")
        sb.Append("  UNION ALL")
        sb.Append("  SELECT T5.Name, T0.DocDate, -T1.Quantity")
        sb.Append("  FROM MOVESA..ORIN T0 WITH(NOLOCK)")
        sb.Append("  INNER JOIN MOVESA..RIN1 T1 WITH(NOLOCK) ON T0.DocEntry=T1.DocEntry")
        sb.Append("  INNER JOIN MOVESA..OITM T2 WITH(NOLOCK) ON T2.ItemCode=T1.ItemCode")
        sb.Append("  LEFT  JOIN MOVESA..[@AMODELO] T5 WITH(NOLOCK) ON T2.U_MODELO=T5.Code")
        sb.Append("  WHERE T0.DocDate BETWEEN @FI AND @FF")
        sb.Append("    AND T2.ItmsGrpCod=154 AND T0.CANCELED='N' AND T1.WhsCode=@whs")
        sb.Append("    AND ISNULL(T5.u_lg5,'-')='Y'")
        sb.Append(" ) x GROUP BY x.MODELO")

        Try
            Return DbConfig.GetDataTable(sb.ToString(), DbConfig.DBServer.MOVESA, p)
        Catch
            Return New DataTable()
        End Try
    End Function

    ' ═══════════════════════════════════════════════════════
    ' Construir DataTable final con CBv3 calculado
    ' ═══════════════════════════════════════════════════════
    Private Function BuildResultTable(dtBase As DataTable, dtSemanas As DataTable, storeTier As String) As DataTable
        Dim semLookup As New Dictionary(Of String, DataRow)(StringComparer.OrdinalIgnoreCase)
        For Each r As DataRow In dtSemanas.Rows
            semLookup(r("MODELO").ToString()) = r
        Next

        ' Demanda ponderada por modelo para cálculo Pareto
        Dim demands As New List(Of ModelDemanda)
        Dim totalD  As Decimal = 0
        For Each row As DataRow In dtBase.Rows
            Dim m As String = row("MODELO").ToString()
            Dim s(11) As Decimal
            LeerSemanas(semLookup, m, s)
            Dim p1 As Decimal = (s(0)+s(1)+s(2)+s(3))   / 4D
            Dim p2 As Decimal = (s(4)+s(5)+s(6)+s(7))   / 4D
            Dim p3 As Decimal = (s(8)+s(9)+s(10)+s(11)) / 4D
            Dim d  As Decimal = (p1*W1 + p2*W2 + p3*W3) / (W1+W2+W3)
            demands.Add(New ModelDemanda(m, d))
            totalD += d
        Next

        ' Asignar tier por Pareto
        demands.Sort(Function(a, b) b.Demanda.CompareTo(a.Demanda))
        Dim tierMap As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        Dim acum As Decimal = 0
        For Each item As ModelDemanda In demands
            acum += item.Demanda
            Dim pct As Decimal = If(totalD > 0, acum / totalD, 1D)
            tierMap(item.Modelo) = If(pct <= PARETO_ORO, "ORO",
                                   If(pct <= PARETO_PLATA, "PLATA",
                                   If(pct <= PARETO_BRONCE, "BRONCE", "BAJO")))
        Next

        ' Esquema DataTable
        Dim dt As New DataTable()
        Dim strCols() As String = {"MODELO","CODE","ModelTier","StoreTier","TrendDir","Accion","Estado","UltVenta","UltTraslado"}
        Dim intCols() As String = {"Ranking","RankingGlobal","VtaMes","Fisico","Despacho","Comprometido","Transito",
                                   "InvAjustado","CB_Actual","MinExhib","CB_Propuesto","Deficit","DiasSinTraslado"}
        Dim decCols() As String = {"Prom1a4","Prom5a8","Prom9a12","DPS","VtaDiaBase","IdxTend","AjTend",
                                   "VtaDiaTend","FEstFinal","VtaDiaFinal","Sigma","DCiclo","DLT","SS",
                                   "CBBase","CBxTier","CobDias","VtaDiaria","SeasonalIndex"}
        For Each c In strCols : dt.Columns.Add(c, GetType(String)) : Next
        For Each c In intCols : dt.Columns.Add(c, GetType(Integer)) : Next
        For i As Integer = 1 To 12 : dt.Columns.Add("S" & i, GetType(Decimal)) : Next
        For Each c In decCols : dt.Columns.Add(c, GetType(Decimal)) : Next

        ' Calcular CBv3 por fila
        For Each baseRow As DataRow In dtBase.Rows
            Dim m As String = baseRow("MODELO").ToString()
            Dim s(11) As Decimal
            LeerSemanas(semLookup, m, s)

            Dim fisico As Integer = SafeInt(baseRow("Fisico"))
            Dim desp   As Integer = SafeInt(baseRow("Despacho"))
            Dim trans  As Integer = SafeInt(baseRow("Transito"))
            Dim comp   As Integer = SafeInt(baseRow("Comprometido"))
            Dim invAdj As Integer = fisico + desp + trans - comp
            Dim cbAct  As Integer = SafeInt(baseRow("CB"))
            Dim modelTier As String = If(tierMap.ContainsKey(m), tierMap(m), "BAJO")
            Dim cbr As CBv3Result = CalcularCBv3(s, storeTier, modelTier, SEASON_FLR, invAdj, 0)

            Dim dr As DataRow = dt.NewRow()
            dr("MODELO")          = m
            dr("CODE")            = baseRow("CODE").ToString()
            dr("ModelTier")       = modelTier
            dr("StoreTier")       = storeTier
            dr("TrendDir")        = cbr.TrendDir
            dr("Accion")          = cbr.Accion
            dr("Estado")          = cbr.Estado
            dr("UltVenta")        = If(IsDBNull(baseRow("UltVenta")), "", baseRow("UltVenta").ToString())
            dr("UltTraslado")     = If(IsDBNull(baseRow("UltTraslado")), "", baseRow("UltTraslado").ToString())
            dr("Ranking")         = SafeInt(baseRow("Ranking"))
            dr("RankingGlobal")   = SafeInt(baseRow("RankingGlobal"))
            dr("VtaMes")          = SafeInt(baseRow("VtaMes"))
            dr("Fisico")          = fisico
            dr("Despacho")        = desp
            dr("Comprometido")    = comp
            dr("Transito")        = trans
            dr("InvAjustado")     = invAdj
            dr("CB_Actual")       = cbAct
            dr("MinExhib")        = 0
            dr("CB_Propuesto")    = cbr.CB_Propuesto
            dr("Deficit")         = cbr.Deficit
            dr("DiasSinTraslado") = SafeInt(baseRow("DiasSinTraslado"))
            For i As Integer = 0 To 11 : dr("S" & (i+1)) = s(i) : Next
            dr("Prom1a4")      = cbr.Prom1a4
            dr("Prom5a8")      = cbr.Prom5a8
            dr("Prom9a12")     = cbr.Prom9a12
            dr("DPS")          = cbr.DPS
            dr("VtaDiaBase")   = cbr.VtaDiaBase
            dr("IdxTend")      = cbr.IdxTend
            dr("AjTend")       = cbr.AjTend
            dr("VtaDiaTend")   = cbr.VtaDiaTend
            dr("FEstFinal")    = cbr.FEstFinal
            dr("VtaDiaFinal")  = cbr.VtaDiaFinal
            dr("Sigma")        = cbr.Sigma
            dr("DCiclo")       = cbr.DCiclo
            dr("DLT")          = cbr.DLT
            dr("SS")           = cbr.SS
            dr("CBBase")       = cbr.CBBase
            dr("CBxTier")      = cbr.CBxTier
            dr("CobDias")      = cbr.CobDias
            dr("VtaDiaria")    = cbr.VtaDiaFinal
            dr("SeasonalIndex") = SEASON_FLR
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function

    ' ═══════════════════════════════════════════════════════
    ' Motor CBv3 — 27 pasos completos
    ' ═══════════════════════════════════════════════════════
    Private Function CalcularCBv3(s() As Decimal, storeTier As String, modelTier As String,
                                   seasonalIdx As Decimal, invAdj As Integer, minExhib As Integer) As CBv3Result
        Dim r As CBv3Result
        r.TrendDir  = "ESTABLE" : r.AjTend = 1D : r.FEstFinal = 1D
        r.Accion    = "SIN_CB"  : r.Estado = "Muerto"

        ' Paso 1: Validar — mínimo 4 semanas con venta
        Dim totalV As Decimal = s.Sum()
        Dim semsOk As Integer = s.Count(Function(v) v > 0)
        If totalV = 0 OrElse semsOk < 4 Then
            r.CB_Propuesto = 0
            r.Deficit  = Math.Max(0, -invAdj)
            r.CobDias  = If(invAdj > 0, 999D, 0D)
            r.Accion   = If(totalV = 0, "RETIRAR", "SIN_CB")
            r.Estado   = "Muerto"
            Return r
        End If

        ' Pasos 2-4: Promedios por grupo de 4 semanas
        r.Prom1a4  = (s(0)+s(1)+s(2)+s(3))   / 4D
        r.Prom5a8  = (s(4)+s(5)+s(6)+s(7))   / 4D
        r.Prom9a12 = (s(8)+s(9)+s(10)+s(11)) / 4D

        ' Paso 5: Demanda Ponderada Semanal
        r.DPS = (r.Prom1a4*W1 + r.Prom5a8*W2 + r.Prom9a12*W3) / (W1+W2+W3)

        ' Paso 6: Venta diaria base
        r.VtaDiaBase = r.DPS / 7D

        ' Pasos 7-9: Tendencia
        r.IdxTend = If(r.Prom9a12 > 0, r.Prom1a4 / r.Prom9a12, If(r.Prom1a4 > 0, 9.99D, 1D))
        If r.IdxTend > GROWTH_TH Then
            r.TrendDir = "CRECIENDO"   : r.AjTend = GROWTH_ADJ
        ElseIf r.IdxTend < DECLINE_TH Then
            r.TrendDir = "DECRECIENDO" : r.AjTend = DECLINE_ADJ
        Else
            r.TrendDir = "ESTABLE"     : r.AjTend = 1.0D
        End If
        r.VtaDiaTend = r.VtaDiaBase * r.AjTend

        ' Pasos 10-15: Estacionalidad — piso=1.0 nunca reduce demanda
        r.FEstFinal   = Math.Max(seasonalIdx, SEASON_FLR)
        r.VtaDiaFinal = r.VtaDiaTend * r.FEstFinal

        ' Paso 18: σ diaria = DESVEST.M(semanas) / 7
        Dim mean   As Decimal = totalV / 12D
        Dim sumSq  As Decimal = s.Select(Function(v) (v - mean) ^ 2D).Sum()
        Dim sigmaW As Decimal = CDec(Math.Sqrt(CDbl(sumSq / 11D)))
        r.Sigma = sigmaW / 7D

        ' Pasos 19-24: Componentes CB
        Dim cycle    As Integer = If(CycleByStore.ContainsKey(storeTier), CycleByStore(storeTier), 7)
        Dim z        As Decimal = If(ZByTier.ContainsKey(modelTier), ZByTier(modelTier), 1.28D)
        Dim tf       As Decimal = If(TFByTier.ContainsKey(modelTier), TFByTier(modelTier), 1.0D)
        Dim cbMinVal As Integer = 0
        If CbMinMap.ContainsKey(storeTier) AndAlso CbMinMap(storeTier).ContainsKey(modelTier) Then
            cbMinVal = CbMinMap(storeTier)(modelTier)
        End If

        r.DCiclo      = r.VtaDiaFinal * cycle
        r.DLT         = r.VtaDiaFinal * LT_DAYS
        r.SS          = z * r.Sigma * CDec(Math.Sqrt(cycle + LT_DAYS))
        r.CBBase      = r.DCiclo + r.DLT + r.SS
        r.CBxTier     = r.CBBase * tf
        r.CB_Propuesto = Math.Max(CInt(Math.Ceiling(CDbl(r.CBxTier))), Math.Max(minExhib, cbMinVal))

        ' Pasos 25-27: Resultados
        r.Deficit = Math.Max(0, r.CB_Propuesto - invAdj)
        r.CobDias = If(r.VtaDiaFinal > 0, CDec(invAdj) / r.VtaDiaFinal,
                       If(invAdj > 0, 999D, 0D))

        If r.Deficit > 0 Then
            r.Accion = "RESURTIR"
            r.Estado = If(invAdj <= 0, "Critico", "Bajo")
        ElseIf invAdj > r.CB_Propuesto Then
            r.Accion = "REDUCIR"
            r.Estado = "SobreStock"
        Else
            r.Accion = "MANTENER"
            r.Estado = "OK"
        End If
        Return r
    End Function

    ' ═══════════════════════════════════════════════════════
    ' Métricas del dashboard
    ' ═══════════════════════════════════════════════════════
    Private Sub ActualizarMetricas(dt As DataTable)
        Dim total As Integer = 0, critico As Integer = 0, bajo As Integer = 0
        Dim ok As Integer = 0, sobre As Integer = 0, muerto As Integer = 0
        Dim res As Integer = 0, man As Integer = 0, red As Integer = 0
        Dim nOro As Integer = 0, nPla As Integer = 0, nBro As Integer = 0, nBaj As Integer = 0
        Dim totalDef As Integer = 0

        For Each row As DataRow In dt.Rows
            Dim est  As String = row("Estado").ToString()
            Dim acc  As String = row("Accion").ToString()
            Dim tier As String = row("ModelTier").ToString()
            totalDef += SafeInt(row("Deficit"))
            Select Case tier
                Case "ORO"    : nOro += 1
                Case "PLATA"  : nPla += 1
                Case "BRONCE" : nBro += 1
                Case "BAJO"   : nBaj += 1
            End Select
            If est = "Muerto" Then
                muerto += 1
            Else
                total += 1
                Select Case est
                    Case "Critico"    : critico += 1
                    Case "Bajo"       : bajo += 1
                    Case "OK"         : ok += 1
                    Case "SobreStock" : sobre += 1
                End Select
            End If
            Select Case acc
                Case "RESURTIR"          : res += 1
                Case "MANTENER"          : man += 1
                Case "REDUCIR", "RETIRAR": red += 1
            End Select
        Next

        litTotal.Text      = total.ToString()
        litOro.Text        = nOro.ToString()
        litPlata.Text      = nPla.ToString()
        litBronce.Text     = nBro.ToString()
        litBajoT.Text      = nBaj.ToString()
        litCritico.Text    = critico.ToString()
        litBajo.Text       = bajo.ToString()
        litOk.Text         = ok.ToString()
        litSobreStock.Text = sobre.ToString()
        litMuerto.Text     = muerto.ToString()
        litResurtir.Text   = res.ToString()
        litMantener.Text   = man.ToString()
        litReducir.Text    = red.ToString()
        litDeficit.Text    = totalDef.ToString()
    End Sub

    ' ═══════════════════════════════════════════════════════
    ' Eventos de botones
    ' ═══════════════════════════════════════════════════════
    Protected Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
        BindGrid(drpSucursales.SelectedValue.ToString())
    End Sub

    Protected Sub btnAplicarSugerido_Click(sender As Object, e As EventArgs) Handles btnAplicarSugerido.Click
        Try
            Dim whscode   As String = drpSucursales.SelectedValue.ToString()
            Dim usuario   As String = If(Session("Name") IsNot Nothing, Session("Name").ToString(), "anon")
            Dim ip        As String = Request.UserHostAddress
            Dim storeTier As String = ObtenerStoreTier(whscode)

            Dim paramsSP As New List(Of SqlParameter) From {
                New SqlParameter("@WHSCODE", whscode),
                New SqlParameter("@COBERTURA_DIAS", 7),
                New SqlParameter("@MUERTO_DIAS", 90)
            }
            Dim dtBase As DataTable = DbConfig.ExecuteStoredProcedure(
                "[dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS_V2]",
                DbConfig.DBServer.ARMADOMOTOS, paramsSP)
            Dim dtSem  As DataTable = CargarVentasSemanales(whscode)
            Dim dt     As DataTable = BuildResultTable(dtBase, dtSem, storeTier)

            Dim aplicados As Integer = 0
            For Each row As DataRow In dt.Rows
                Dim cbAct As Integer = SafeInt(row("CB_Actual"))
                Dim cbSug As Integer = SafeInt(row("CB_Propuesto"))
                If cbSug <> cbAct Then
                    CuadroBasicoHelper.ActualizarCB(whscode, row("MODELO").ToString(),
                        cbAct, cbSug, cbSug, SafeDec(row("VtaDiaria")), row("CobDias"),
                        usuario, ip, "Aplicar CB v3 en lote", "MASIVO_V3")
                    aplicados += 1
                End If
            Next

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ok",
                "Swal.fire('Listo','" & aplicados & " modelos actualizados con CB v3','success').then(function(){location.reload();});", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "err",
                "Swal.fire('Error'," & Q(ex.Message) & ",'error');", True)
        End Try
    End Sub

    Protected Sub btnHistorial_Click(sender As Object, e As EventArgs) Handles btnHistorial.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hist",
            "Swal.fire('Historial','Pantalla de historial pendiente.','info');", True)
    End Sub

    Protected Sub gridV3_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then Exit Sub
        Try
            Dim dias As Integer = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DiasSinTraslado"))
            Dim cell As TableCell = e.Row.Cells(e.Row.Cells.Count - 2)
            If dias <= 7 Then
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#16a34a")
                cell.ForeColor = System.Drawing.Color.White
            ElseIf dias <= 14 Then
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#f97316")
                cell.ForeColor = System.Drawing.Color.White
            Else
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#dc2626")
                cell.ForeColor = System.Drawing.Color.White
            End If
        Catch
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════
    ' Helpers protegidos — accesibles desde ASPX markup
    ' ═══════════════════════════════════════════════════════

    Protected Function GetCBv3Attr(p1 As Object, p2 As Object, p3 As Object,
                                    vdb As Object, tend As Object, ajtend As Object,
                                    vdf As Object, sigma As Object,
                                    dciclo As Object, dlt As Object, ss As Object,
                                    cbbase As Object, cbtier As Object) As String
        Return String.Join("|", New Object() {
            CInt(SafeDec(p1)*10000), CInt(SafeDec(p2)*10000), CInt(SafeDec(p3)*10000),
            CInt(SafeDec(vdb)*10000), tend.ToString(),
            CInt(SafeDec(ajtend)*10000), CInt(SafeDec(vdf)*10000),
            CInt(SafeDec(sigma)*10000), CInt(SafeDec(dciclo)*10000),
            CInt(SafeDec(dlt)*10000), CInt(SafeDec(ss)*10000),
            CInt(SafeDec(cbbase)*10000), CInt(SafeDec(cbtier)*10000)
        })
    End Function

    Protected Function GetDeltaHtml(cbAct As Object, cbProp As Object) As String
        Dim a As Integer = SafeInt(cbAct)
        Dim p As Integer = SafeInt(cbProp)
        If p > a Then Return "<span class='delta-up'>&#9650;" & (p-a) & "</span>"
        If p < a Then Return "<span class='delta-down'>&#9660;" & (a-p) & "</span>"
        Return "<span class='delta-eq'>=</span>"
    End Function

    Protected Function GetZInt(tier As Object) As Integer
        Select Case tier.ToString().ToUpper()
            Case "ORO"    : Return 16450
            Case "PLATA"  : Return 12800
            Case "BRONCE" : Return 10400
            Case Else     : Return 8400
        End Select
    End Function

    Protected Function GetTFInt(tier As Object) As Integer
        Select Case tier.ToString().ToUpper()
            Case "BRONCE" : Return 8500
            Case "BAJO"   : Return 5000
            Case Else     : Return 10000
        End Select
    End Function

    Protected Function GetCycleInt(storeTier As Object) As Integer
        Select Case storeTier.ToString().ToUpper()
            Case "A" : Return 5
            Case "C" : Return 10
            Case Else : Return 7
        End Select
    End Function

    Protected Function ToI4(v As Object) As Integer
        Try : Return CInt(Convert.ToDecimal(v) * 10000) : Catch : Return 0 : End Try
    End Function

    Protected Function SafeInt2(v As Object) As Integer
        Return SafeInt(v)
    End Function

    Protected Function FmtCob(v As Object) As String
        Dim d As Decimal = SafeDec(v)
        If d >= 999 Then Return "&#8734;"
        If d <= 0   Then Return "0.0"
        Return d.ToString("0.0")
    End Function

    Protected Function TendIcon(tend As Object) As String
        Select Case tend.ToString()
            Case "CRECIENDO"   : Return "<span class='tend-CRE' title='Tendencia creciente'>&#8593;</span>"
            Case "DECRECIENDO" : Return "<span class='tend-DEC' title='Tendencia decreciente'>&#8595;</span>"
            Case Else          : Return "<span class='tend-EST' title='Tendencia estable'>&#8594;</span>"
        End Select
    End Function

    ' ═══════════════════════════════════════════════════════
    ' Wrapper público (compatibilidad externa)
    ' ═══════════════════════════════════════════════════════
    Public Shared Function ActualizarCB(whscode As String, modelo As String,
                                        cbAnterior As Integer, cbNuevo As Integer,
                                        cbSugerido As Integer, ventaDiaria As Decimal,
                                        diasCobertura As Object, usuario As String,
                                        ip As String, motivo As String, origen As String) As Integer
        Return CuadroBasicoHelper.ActualizarCB(whscode, modelo, cbAnterior, cbNuevo,
                                               cbSugerido, ventaDiaria, diasCobertura,
                                               usuario, ip, motivo, origen)
    End Function

    ' ═══════════════════════════════════════════════════════
    ' Utilidades internas
    ' ═══════════════════════════════════════════════════════
    Private Sub LeerSemanas(lookup As Dictionary(Of String, DataRow), modelo As String, ByRef s() As Decimal)
        If lookup.ContainsKey(modelo) Then
            Dim sr As DataRow = lookup(modelo)
            For i As Integer = 0 To 11
                s(i) = Math.Max(0D, SafeDec(sr("S" & (i+1))))
            Next
        End If
    End Sub

    Private Function ObtenerStoreTier(whscode As String) As String
        Return "B"  ' Default 7 días. Configurar en system_configs cuando esté disponible.
    End Function

    Private Shared Function SafeInt(v As Object) As Integer
        If v Is Nothing OrElse v Is DBNull.Value Then Return 0
        Try : Return Convert.ToInt32(v) : Catch : Return 0 : End Try
    End Function

    Private Shared Function SafeDec(v As Object) As Decimal
        If v Is Nothing OrElse v Is DBNull.Value Then Return 0D
        Try : Return Convert.ToDecimal(v) : Catch : Return 0D : End Try
    End Function

    Private Shared Function Q(s As String) As String
        If s Is Nothing Then Return "''"
        Return "'" & s.Replace("'", " ").Replace("""", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "'"
    End Function

End Class

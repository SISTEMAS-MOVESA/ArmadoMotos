
Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosPanelProduccion
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTableZero As New DataTable
    Public MyTable As New DataTable
    Private Sub TrasladosPanelProduccion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'loadRepeaterZero()
            'loadRepeater()
            'loadRepeaterZonas_CD()
        Catch ex As Exception
            Response.Write("<script>console.log('TrasladosPanelProduccion_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    'Sub loadRepeater()
    '    Try
    '        Dim strSQL = "  DECLARE @year INT = DATEPART(YEAR, GETDATE()); " &
    '            " DECLARE @week INT = DATEPART(WEEK, GETDATE()); " &
    '            " DECLARE @F1 VARCHAR(50), @F2 VARCHAR(50) " &
    '            " SELECT @F1 = FORMAT(MIN(FECHA),'yyyyMMdd'), @F2 = FORMAT(MAX(FECHA),'yyyyMMdd') " &
    '            " FROM MOVESAWEB..CALENDARIO WHERE CONCAT(YEAR(FECHA),'-',DATEPART(WEEK, FECHA)) = CONCAT(@year,'-',@week) " &
    '            " ;WITH VENTAS AS ( " &
    '            " SELECT CONVERT(DATE, DOCDATE) AS DOCDATE, CANAL, SUM(QUANTITY) AS QUANTITY FROM MOVESA..PT_VW_GENERAL_VENTAS_MOTOS AS GVM " &
    '            " WHERE CANAL IN ('CD','CI') AND DOCDATE BETWEEN @F1 AND @F2 " &
    '            " GROUP BY CANAL, CONVERT(DATE, DOCDATE) " &
    '            " ) " &
    '            " SELECT * FROM ( " &
    '            " SELECT CAL.Nom_Dia AS NombreDia, CAN.CANAL, isnull(SUM(GVM.QUANTITY),0) AS VENTAS FROM MOVESAWEB..CALENDARIO AS CAL " &
    '            " INNER JOIN (SELECT 'CD' AS CANAL UNION ALL SELECT 'CI') AS CAN ON 1 = 1 " &
    '            " LEFT JOIN VENTAS AS GVM ON GVM.CANAL = CAN.CANAL AND GVM.DOCDATE = CAL.FECHA " &
    '            " WHERE FECHA BETWEEN @F1 AND @F2 " &
    '            " GROUP BY CAL.Nom_Dia, CAN.CANAL " &
    '            " ) AS SourceTable " &
    '            " PIVOT ( SUM(VENTAS) FOR NombreDia IN ([lunes], [martes], [miércoles], [jueves], [viernes], [sábado])) AS PivotTable;"

    '        Dim MyTable As New DataTable()
    '        Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon1))
    '            cmdSQL.Connection.Open()
    '            MyTable.Load(cmdSQL.ExecuteReader)
    '            cmdSQL.Connection.Close()
    '        End Using

    '        ' Calcular totales por día
    '        Dim totalLunes As Decimal = If(MyTable.Columns.Contains("Lunes"), MyTable.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Lunes")), 0D, Convert.ToDecimal(r("Lunes")))), 0D)
    '        Dim totalMartes As Decimal = If(MyTable.Columns.Contains("Martes"), MyTable.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Martes")), 0D, Convert.ToDecimal(r("Martes")))), 0D)
    '        Dim totalMiercoles As Decimal = If(MyTable.Columns.Contains("Miércoles"), MyTable.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Miércoles")), 0D, Convert.ToDecimal(r("Miércoles")))), 0D)
    '        Dim totalJueves As Decimal = If(MyTable.Columns.Contains("Jueves"), MyTable.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Jueves")), 0D, Convert.ToDecimal(r("Jueves")))), 0D)
    '        Dim totalViernes As Decimal = If(MyTable.Columns.Contains("Viernes"), MyTable.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Viernes")), 0D, Convert.ToDecimal(r("Viernes")))), 0D)
    '        Dim totalSabado As Decimal = If(MyTable.Columns.Contains("Sábado"), MyTable.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Sábado")), 0D, Convert.ToDecimal(r("Sábado")))), 0D)

    '        ' Agregar columnas de porcentaje por día
    '        MyTable.Columns.Add("PorcLunes", GetType(Decimal))
    '        MyTable.Columns.Add("PorcMartes", GetType(Decimal))
    '        MyTable.Columns.Add("PorcMiércoles", GetType(Decimal))
    '        MyTable.Columns.Add("PorcJueves", GetType(Decimal))
    '        MyTable.Columns.Add("PorcViernes", GetType(Decimal))
    '        MyTable.Columns.Add("PorcSabado", GetType(Decimal))

    '        ' Agregar columnas de total por canal y porcentaje por canal
    '        MyTable.Columns.Add("TotalCanal", GetType(Decimal))
    '        MyTable.Columns.Add("PorcCanal", GetType(Decimal))

    '        ' Calcular gran total de la semana
    '        Dim granTotalSemana As Decimal = 0D

    '        For Each row As DataRow In MyTable.Rows
    '            Dim vLunes As Decimal = If(IsDBNull(row("Lunes")), 0D, Convert.ToDecimal(row("Lunes")))
    '            Dim vMartes As Decimal = If(IsDBNull(row("Martes")), 0D, Convert.ToDecimal(row("Martes")))
    '            Dim vMiercoles As Decimal = If(IsDBNull(row("Miércoles")), 0D, Convert.ToDecimal(row("Miércoles")))
    '            Dim vJueves As Decimal = If(IsDBNull(row("Jueves")), 0D, Convert.ToDecimal(row("Jueves")))
    '            Dim vViernes As Decimal = If(IsDBNull(row("Viernes")), 0D, Convert.ToDecimal(row("Viernes")))
    '            Dim vSabado As Decimal = If(IsDBNull(row("Sábado")), 0D, Convert.ToDecimal(row("Sábado")))

    '            row("PorcLunes") = If(totalLunes = 0, 0, vLunes / totalLunes * 100)
    '            row("PorcMartes") = If(totalMartes = 0, 0, vMartes / totalMartes * 100)
    '            row("PorcMiércoles") = If(totalMiercoles = 0, 0, vMiercoles / totalMiercoles * 100)
    '            row("PorcJueves") = If(totalJueves = 0, 0, vJueves / totalJueves * 100)
    '            row("PorcViernes") = If(totalViernes = 0, 0, vViernes / totalViernes * 100)
    '            row("PorcSabado") = If(totalSabado = 0, 0, vSabado / totalSabado * 100)

    '            Dim totalCanal As Decimal = vLunes + vMartes + vMiercoles + vJueves + vViernes + vSabado
    '            row("TotalCanal") = totalCanal
    '            granTotalSemana += totalCanal
    '        Next

    '        ' Ahora puedes calcular el % que representa cada canal respecto al total de la semana
    '        For Each row As DataRow In MyTable.Rows
    '            Dim totalCanal As Decimal = Convert.ToDecimal(row("TotalCanal"))
    '            row("PorcCanal") = If(granTotalSemana = 0, 0, totalCanal / granTotalSemana * 100)
    '        Next

    '        ' Enlazar repeater
    '        repeater1.DataSource = MyTable
    '        repeater1.DataBind()

    '        ' Asignar los totales a los Literals
    '        litTotalLunes.Text = totalLunes.ToString("N0")
    '        litTotalMartes.Text = totalMartes.ToString("N0")
    '        litTotalMiercoles.Text = totalMiercoles.ToString("N0")
    '        litTotalJueves.Text = totalJueves.ToString("N0")
    '        litTotalViernes.Text = totalViernes.ToString("N0")
    '        litTotalSabado.Text = totalSabado.ToString("N0")
    '        litTotalSemana.Text = granTotalSemana.ToString("N0")
    '        litPorcTotalSemana.Text = "100.00%" ' Siempre será 100% del total

    '        '--------------------------- generador dinamico tabla resumen de toda la semana---------------------------
    '        Dim canales As String() = {"CD", "CI"}
    '        Dim resumenCanal As New List(Of Tuple(Of String, Decimal))

    '        For Each canal In canales
    '            Dim filtro = MyTable.Select("Canal = '" & canal & "'")
    '            Dim sumLunes As Decimal = filtro.Sum(Function(r) If(IsDBNull(r("Lunes")), 0D, Convert.ToDecimal(r("Lunes"))))
    '            Dim sumMartes As Decimal = filtro.Sum(Function(r) If(IsDBNull(r("Martes")), 0D, Convert.ToDecimal(r("Martes"))))
    '            Dim sumMiercoles As Decimal = filtro.Sum(Function(r) If(IsDBNull(r("Miércoles")), 0D, Convert.ToDecimal(r("Miércoles"))))
    '            Dim sumJueves As Decimal = filtro.Sum(Function(r) If(IsDBNull(r("Jueves")), 0D, Convert.ToDecimal(r("Jueves"))))
    '            Dim sumViernes As Decimal = filtro.Sum(Function(r) If(IsDBNull(r("Viernes")), 0D, Convert.ToDecimal(r("Viernes"))))
    '            Dim sumSabado As Decimal = filtro.Sum(Function(r) If(IsDBNull(r("Sábado")), 0D, Convert.ToDecimal(r("Sábado"))))
    '            Dim totalCanal As Decimal = sumLunes + sumMartes + sumMiercoles + sumJueves + sumViernes + sumSabado

    '            resumenCanal.Add(New Tuple(Of String, Decimal)(canal, totalCanal))
    '        Next

    '        Dim totalGeneral As Decimal = resumenCanal.Sum(Function(d) d.Item2)

    '        Dim porcIncremento As Decimal = 0
    '        Decimal.TryParse(txtIncremento.Text, porcIncremento)

    '        Dim sbResumen As New Text.StringBuilder()
    '        sbResumen.AppendLine("<table id='resumenGralCanal' class='table table-bordered text-center w-50' style='margin-bottom:1rem'>")
    '        sbResumen.AppendLine("  <thead style='background-color: black;color: #fff;'><tr>")
    '        sbResumen.AppendLine("    <th>Canal</th><th>Total</th><th>% Total</th><th>Meta Calculada</th><th>Meta Asignada</th><th>% MA</th>")
    '        sbResumen.AppendLine("    <th>Despacho Asignado</th><th>Cargado</th><th>% Logro</th><th>% Proyeccion</th>")
    '        sbResumen.AppendLine("  </tr></thead>")
    '        sbResumen.AppendLine("  <tbody>")

    '        For Each tupla In resumenCanal
    '            Dim metaCalculada As Decimal = tupla.Item2 * (1 + porcIncremento / 100)
    '            sbResumen.AppendLine("    <tr>")
    '            sbResumen.AppendLine("      <td>" & tupla.Item1 & "</td>")
    '            sbResumen.AppendLine("      <td>" & tupla.Item2.ToString("N0") & "</td>")
    '            sbResumen.AppendLine("      <td>" & If(totalGeneral > 0, (tupla.Item2 / totalGeneral * 100).ToString("0.00") & "%", "0.00%") & "</td>")
    '            sbResumen.AppendLine("      <td>" & metaCalculada.ToString("N0") & "</td>")
    '            sbResumen.AppendLine("      <td><input class='form-control-sm text-center' txtCalculada ></td>")
    '            sbResumen.AppendLine("      <td><input class='form-control-sm text-center' txtPMetaPropuesta></td>")
    '            sbResumen.AppendLine("      <td>0</td>")
    '            sbResumen.AppendLine("      <td>0</td>")
    '            sbResumen.AppendLine("      <td>0.00</td>")
    '            sbResumen.AppendLine("      <td>0.00</td>")
    '            sbResumen.AppendLine("    </tr>")
    '        Next

    '        ' Fila total general
    '        Dim metaTotal As Decimal = totalGeneral * (1 + porcIncremento / 100)
    '        sbResumen.AppendLine("    <tr style='background-color: red;color: #fff; font-weight:bold'>")
    '        sbResumen.AppendLine("      <td>Totales</td>")
    '        sbResumen.AppendLine("      <td>" & totalGeneral.ToString("N0") & "</td>")
    '        sbResumen.AppendLine("      <td>100.00%</td>")
    '        sbResumen.AppendLine("      <td>" & metaTotal.ToString("N0") & "</td>")
    '        sbResumen.AppendLine("      <td><input class='form-control-sm text-center' txtSumCalculada ></td>")
    '        sbResumen.AppendLine("      <td><input class='form-control-sm text-center' txtPMetaPropuesta></td>")
    '        sbResumen.AppendLine("      <td>0.00%</td>")
    '        sbResumen.AppendLine("      <td>0.00%</td>")
    '        sbResumen.AppendLine("      <td>0.00%</td>")
    '        sbResumen.AppendLine("      <td>0.00%</td>")
    '        sbResumen.AppendLine("    </tr>")

    '        sbResumen.AppendLine("  </tbody>")
    '        sbResumen.AppendLine("</table>")

    '        litResumenCanal.Text = sbResumen.ToString()
    '    Catch ex As Exception
    '        Response.Write("loadRepeater " & ex.Message)
    '        Response.Write("<script>console.log('loadRepeater: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub
    'Sub loadRepeaterZero()
    '    Try
    '        Dim strSQL = "  DECLARE @year INT = DATEPART(YEAR, GETDATE()); " &
    '                 " DECLARE @week INT = DATEPART(ISOWK, GETDATE()); " &
    '                 " WITH Ventas AS ( " &
    '                 " SELECT " &
    '                 " CASE alm.U_Type WHEN 'PRO' THEN 'CD' ELSE 'CI' END AS Canal, " &
    '                 " DATENAME(WEEKDAY, t0.DocDate) AS NombreDia, " &
    '                 " SUM(t1.Quantity) AS Cantidad " &
    '                 " FROM OINV t0  " &
    '                 " INNER JOIN INV1 t1 ON t0.DocEntry = t1.DocEntry " &
    '                 " INNER JOIN OWHS alm WITH(NOLOCK) ON t1.WhsCode = alm.WhsCode " &
    '                 " INNER JOIN OITM t2 ON t2.ItemCode = t1.ItemCode " &
    '                 " LEFT JOIN [@AMODELO] t3 ON t2.U_AMODELO = t3.Code " &
    '                 " WHERE DATEPART(YEAR, t0.DocDate) = @year " &
    '                 " AND DATEPART(ISOWK, t0.DocDate) = @week - 1 " &
    '                 " AND alm.WhsCode NOT IN ('EST001') " &
    '                 " AND t2.ItmsGrpCod = 154 " &
    '                 " AND t0.CANCELED = 'N' " &
    '                 " AND t1.TargetType <> 14 " &
    '                 " GROUP BY  " &
    '                 " CASE alm.U_Type WHEN 'PRO' THEN 'CD' ELSE 'CI' END, " &
    '                 " DATENAME(WEEKDAY, t0.DocDate) " &
    '                 " ) " &
    '                 " SELECT * " &
    '                 " FROM ( " &
    '                 " SELECT Canal, NombreDia, Cantidad " &
    '                 " FROM Ventas " &
    '                 " ) AS SourceTable " &
    '                 " PIVOT ( " &
    '                 " SUM(Cantidad) " &
    '                 " FOR NombreDia IN ([Lunes], [Martes], [Miércoles], [Jueves], [Viernes], [Sábado]) " &
    '                 " ) AS PivotTable;"

    '        Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon1))
    '            cmdSQL.Connection.Open()
    '            MyTableZero.Load(cmdSQL.ExecuteReader)
    '            cmdSQL.Connection.Close()
    '        End Using

    '        ' Calcular totales por día
    '        Dim totalLunes As Decimal = If(MyTableZero.Columns.Contains("Lunes"), MyTableZero.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Lunes")), 0D, Convert.ToDecimal(r("Lunes")))), 0D)
    '        Dim totalMartes As Decimal = MyTableZero.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Martes")), 0D, Convert.ToDecimal(r("Martes"))))
    '        Dim totalMiercoles As Decimal = MyTableZero.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Miércoles")), 0D, Convert.ToDecimal(r("Miércoles"))))
    '        Dim totalJueves As Decimal = MyTableZero.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Jueves")), 0D, Convert.ToDecimal(r("Jueves"))))
    '        Dim totalViernes As Decimal = MyTableZero.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Viernes")), 0D, Convert.ToDecimal(r("Viernes"))))
    '        Dim totalSabado As Decimal = MyTableZero.AsEnumerable().Sum(Function(r) If(IsDBNull(r("Sábado")), 0D, Convert.ToDecimal(r("Sábado"))))

    '        ' Guardar totales en ViewState
    '        ViewState("Totales") = New Decimal() {totalLunes, totalMartes, totalMiercoles, totalJueves, totalViernes, totalSabado}

    '        ' Agregar columnas de porcentaje
    '        MyTableZero.Columns.Add("PorcLunes", GetType(Decimal))
    '        MyTableZero.Columns.Add("PorcMartes", GetType(Decimal))
    '        MyTableZero.Columns.Add("PorcMiércoles", GetType(Decimal))
    '        MyTableZero.Columns.Add("PorcJueves", GetType(Decimal))
    '        MyTableZero.Columns.Add("PorcViernes", GetType(Decimal))
    '        MyTableZero.Columns.Add("PorcSabado", GetType(Decimal))

    '        For Each row As DataRow In MyTableZero.Rows
    '            row("PorcLunes") = If(totalLunes = 0, 0, Convert.ToDecimal(row("Lunes")) / totalLunes * 100)
    '            row("PorcMartes") = If(totalMartes = 0, 0, Convert.ToDecimal(row("Martes")) / totalMartes * 100)
    '            row("PorcMiércoles") = If(totalMiercoles = 0, 0, Convert.ToDecimal(row("Miércoles")) / totalMiercoles * 100)
    '            row("PorcJueves") = If(totalJueves = 0, 0, Convert.ToDecimal(row("Jueves")) / totalJueves * 100)
    '            row("PorcViernes") = If(totalViernes = 0, 0, Convert.ToDecimal(row("Viernes")) / totalViernes * 100)
    '            row("PorcSabado") = If(totalSabado = 0, 0, Convert.ToDecimal(row("Sábado")) / totalSabado * 100)
    '        Next

    '        ' Enlazar
    '        repeater2.DataSource = MyTableZero
    '        repeater2.DataBind()

    '    Catch ex As Exception
    '        Response.Write("loadRepeater " & ex.Message)
    '        Response.Write("<script>console.log('loadRepeater: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub
    'Sub loadRepeaterZonas_CD()
    '    Try
    '        Dim pathSQL As String = "C:\inetpub\wwwroot\ArmadoMotos\Querys\TrasladosPanelProduccion_repeater3_CD.sql"
    '        Dim strSQL As String = System.IO.File.ReadAllText(pathSQL)

    '        ' Ejecutar SQL
    '        Dim ds As New DataSet()
    '        Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon1))
    '            cmdSQL.Connection.Open()
    '            Dim da As New SqlDataAdapter(cmdSQL)
    '            ds.EnforceConstraints = False
    '            da.Fill(ds)
    '            cmdSQL.Connection.Close()
    '        End Using

    '        ' Copiar tabla limpia
    '        MyTableZero = New DataTable()
    '        MyTableZero = ds.Tables(0).Copy()

    '        ' Detectar semanas dinámicas
    '        Dim columnasSemana As New List(Of String)
    '        For Each col As DataColumn In MyTableZero.Columns
    '            If IsNumeric(col.ColumnName) Then
    '                columnasSemana.Add(col.ColumnName)
    '            End If
    '        Next

    '        ' Calcular totales por semana
    '        Dim totales As New Dictionary(Of String, Decimal)
    '        For Each semana In columnasSemana
    '            Dim total As Decimal = MyTableZero.AsEnumerable().Sum(Function(r) If(IsDBNull(r(semana)), 0D, Convert.ToDecimal(r(semana))))
    '            totales(semana) = total
    '        Next

    '        ' Agregar columnas de porcentaje por semana
    '        For Each semana In columnasSemana
    '            Dim colPorc As String = "Porc_" & semana
    '            If Not MyTableZero.Columns.Contains(colPorc) Then
    '                MyTableZero.Columns.Add(colPorc, GetType(Decimal))
    '            End If
    '        Next

    '        ' Calcular porcentajes por fila
    '        For Each row As DataRow In MyTableZero.Rows
    '            For Each semana In columnasSemana
    '                Dim val As Decimal = If(IsDBNull(row(semana)), 0D, Convert.ToDecimal(row(semana)))
    '                Dim total As Decimal = totales(semana)
    '                row("Porc_" & semana) = If(total = 0, 0, (val / total) * 100)
    '            Next
    '        Next

    '        ' Generar HTML de tabla
    '        Dim html As New StringBuilder()
    '        html.Append("<table id='tablaCanalesCD' class='table table-bordered text-center w-75'>")
    '        html.Append("<thead class='thead-dark'><tr>")
    '        html.Append("<th>Canal</th>")
    '        html.Append("<th>Ruta</th>")

    '        For Each semana In columnasSemana
    '            html.Append("<th>Semana " & semana & "</th><th>%</th>")
    '        Next

    '        html.Append("</tr></thead><tbody>")

    '        ' Cuerpo de la tabla
    '        For Each row As DataRow In MyTableZero.Rows
    '            html.Append("<tr>")
    '            html.Append("<td>" & row("CANAL") & "</td>")
    '            html.Append("<td>" & row("Zona") & "</td>")
    '            For Each semana In columnasSemana
    '                html.Append("<td>" & FormatNumber(row(semana), 0) & "</td>")
    '                html.Append("<td>" & FormatNumber(row("Porc_" & semana), 2) & "%</td>")
    '            Next
    '            html.Append("</tr>")
    '        Next

    '        ' Pie con totales
    '        html.Append("<tfoot><tr><th colspan='2'>Totales</th>")
    '        For Each semana In columnasSemana
    '            html.Append("<td>" & FormatNumber(totales(semana), 0) & "</td><td>100%</td>")
    '        Next
    '        html.Append("</tr></tfoot>")

    '        html.Append("</tbody></table>")

    '        ' Renderizar
    '        litTablaCD.Text = html.ToString()

    '    Catch ex As Exception
    '        Response.Write("loadRepeaterZonas_CD " & ex.Message)
    '        Response.Write("<script>console.log('loadRepeaterZonas_CD: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub



    'Sub loadRepeaterZonas_CD()
    '    Try
    '        ' Leer el script SQL desde archivo
    '        Dim pathSQL As String = "C:\inetpub\wwwroot\ArmadoMotos\Querys\TrasladosPanelProduccion_repeater3_CD.sql"
    '        Dim strSQL As String = System.IO.File.ReadAllText(pathSQL)

    '        Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon1))
    '            cmdSQL.Connection.Open()
    '            MyTableZero.Load(cmdSQL.ExecuteReader)
    '            cmdSQL.Connection.Close()
    '        End Using

    '        ' Enlazar datos al repeater
    '        repeater2.DataSource = MyTableZero
    '        repeater2.DataBind()

    '    Catch ex As Exception
    '        Response.Write("loadRepeater " & ex.Message)
    '        Response.Write("<script>console.log('loadRepeater: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub

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

    'Private Sub repeater1_DataBinding(sender As Object, e As EventArgs) Handles repeater1.DataBinding
    '    Try
    '        Dim totales As Decimal() = CType(ViewState("Totales"), Decimal())

    '        litTotalLunes.Text = totales(0).ToString("0.00")
    '        litTotalMartes.Text = totales(1).ToString("0.00")
    '        litTotalMiercoles.Text = totales(2).ToString("0.00")
    '        litTotalJueves.Text = totales(3).ToString("0.00")
    '        litTotalViernes.Text = totales(4).ToString("0.00")
    '        litTotalSabado.Text = totales(5).ToString("0.00")

    '    Catch ex As Exception
    '        Response.Write("repeater1_DataBinding " & ex.Message)
    '        Response.Write("<script>console.log('repeater1_DataBinding: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub

    'Private Sub repeater2_DataBinding(sender As Object, e As EventArgs) Handles repeater2.DataBinding
    '    Try
    '        Dim totales As Decimal() = CType(ViewState("Totales"), Decimal())

    '        litZeroTotalLunes.Text = totales(0).ToString("0.00")
    '        litZeroTotalMartes.Text = totales(1).ToString("0.00")
    '        litZeroTotalMiercoles.Text = totales(2).ToString("0.00")
    '        litZeroTotalJueves.Text = totales(3).ToString("0.00")
    '        litZeroTotalViernes.Text = totales(4).ToString("0.00")
    '        litZeroTotalSabado.Text = totales(5).ToString("0.00")

    '    Catch ex As Exception
    '        Response.Write("repeater1_DataBinding " & ex.Message)
    '        Response.Write("<script>console.log('repeater1_DataBinding: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub
End Class

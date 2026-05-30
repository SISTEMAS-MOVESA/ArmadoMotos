Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Partial Class TrasladosDDashboardDetalle
    Inherits System.Web.UI.Page
    Public nuevoDespachoId As Integer = 0

    Private Sub TrasladosDDashboardDetalle_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            EnsureControlTable()
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGridDetallePlan(Request.QueryString("planid"))
                    BindgridSugerido(Request.QueryString("planid"), Session("User"))
                    RestoreLastAlmacen(Request.QueryString("planid"), Session("User").ToString())
                    getResumenPedidosCI()
                    BindgridPortalPedidos()
                End If
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub

    ' Crea la tabla de control si no existe; agrega columna FECHA_FACTURA si falta
    Private Sub EnsureControlTable()
        Try
            Dim sqlCreate As String =
                "IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id=OBJECT_ID(N'[dbo].[VENTAS_U7D_CONTROL]') AND type='U') " &
                "CREATE TABLE [dbo].[VENTAS_U7D_CONTROL] (" &
                " ID INT IDENTITY(1,1) PRIMARY KEY," &
                " DOCNUM INT NOT NULL," &
                " WHSCODE NVARCHAR(20) NOT NULL," &
                " COD_MODELO NVARCHAR(50) NULL," &
                " NOTA NVARCHAR(500) NULL," &
                " ESTADO NVARCHAR(20) NOT NULL DEFAULT 'PENDIENTE'," &
                " FECHA_FACTURA DATE NULL," &
                " FECHA_REGISTRO DATETIME NOT NULL DEFAULT GETDATE()," &
                " USUARIO NVARCHAR(100) NULL," &
                " CONSTRAINT UQ_VU7D_DOC_WHS UNIQUE (DOCNUM, WHSCODE)" &
                ")"
            DbConfig.ExecuteNonQuery(sqlCreate, DbConfig.DBServer.ARMADOMOTOS)
            ' Migración: agrega FECHA_FACTURA si la tabla ya existía sin ella
            Dim sqlMig As String =
                "IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'[dbo].[VENTAS_U7D_CONTROL]') AND name='FECHA_FACTURA') " &
                "ALTER TABLE [dbo].[VENTAS_U7D_CONTROL] ADD FECHA_FACTURA DATE NULL"
            DbConfig.ExecuteNonQuery(sqlMig, DbConfig.DBServer.ARMADOMOTOS)
            ' Migración: agrega PARETO_PCT si la tabla ya existía sin ella
            Dim sqlMig2 As String =
                "IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'[dbo].[VENTAS_U7D_CONTROL]') AND name='PARETO_PCT') " &
                "ALTER TABLE [dbo].[VENTAS_U7D_CONTROL] ADD PARETO_PCT DECIMAL(5,2) NULL"
            DbConfig.ExecuteNonQuery(sqlMig2, DbConfig.DBServer.ARMADOMOTOS)
        Catch : End Try
    End Sub

    ' Al cargar la página restaura automáticamente el último almacén trabajado (útil tras refresh o vencimiento de sesión)
    Private Sub RestoreLastAlmacen(planid As String, usuario As String)
        Try
            If String.IsNullOrEmpty(planid) OrElse String.IsNullOrEmpty(usuario) Then Return

            ' 1. Último almacén con ítems en borrador para este plan y usuario
            Dim sqlLast As String =
                "SELECT TOP 1 ALMDESTINO FROM ArmadoMotos.dbo.MACROINSERT " &
                "WHERE PLANID=@planid AND USUARIO=@usuario AND ESTADO='B' " &
                "ORDER BY ID DESC"
            Dim dtLast As DataTable = DbConfig.GetDataTable(sqlLast, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@planid", planid),
                    New SqlParameter("@usuario", usuario)
                })
            If dtLast.Rows.Count = 0 Then Return

            Dim lastWhs As String = dtLast.Rows(0)("ALMDESTINO").ToString().Trim()
            If String.IsNullOrEmpty(lastWhs) Then Return

            ' 2. Datos del almacén en PLANIFICACIONES_DETALLE para mostrar Ruta y Nombre
            Dim sqlAlm As String =
                "SELECT TOP 1 Ruta, Codigo, Almacen FROM ArmadoMotos.dbo.PLANIFICACIONES_DETALLE " &
                "WHERE HEADERID=@planid AND Codigo=@whs"
            Dim dtAlm As DataTable = DbConfig.GetDataTable(sqlAlm, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@planid", planid),
                    New SqlParameter("@whs", lastWhs)
                })
            If dtAlm.Rows.Count > 0 Then
                lblRuta.Text         = dtAlm.Rows(0)("Ruta").ToString()
                lblNombreAlmacen.Text = dtAlm.Rows(0)("Almacen").ToString()
            End If
            lblAlmActual.Text        = lastWhs
            Session("almacenDestino") = lastWhs

            ' 3. Marcar la fila activa en el panel izquierdo
            For Each gvRow As GridViewRow In gridAlmacenesDespachos.Rows
                If gvRow.Cells(1).Text.Trim().Equals(lastWhs, StringComparison.OrdinalIgnoreCase) Then
                    gvRow.CssClass = "selected-alm"
                    Exit For
                End If
            Next

            ' 4. Cargar gridCuadroBasico, hfCurrentWhs, gridUltimas6 y colores
            BindGrid(lastWhs)
        Catch ex As Exception
            Response.Write("<script>console.log('RestoreLastAlmacen: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub TraerEstadoCliente(cardcode As String)
        Try
            Dim sqlString As String = "select frozenFor,Balance,CreditLine from movesa..ocrd where CardCode = @p1"
            Dim params As New List(Of SqlParameter) From {New SqlParameter("@p1", cardcode)}
            Dim dt As DataTable = DbConfig.GetDataTable(sqlString, DbConfig.DBServer.MOVESA, params)
            If dt.Rows.Count > 0 Then
                Dim drow As DataRow = dt.Rows(0)
                If drow("frozenFor").ToString = "Y" Then
                    txtEstatusCliente.Text = "BLOQUEADO"
                    txtEstatusCliente.Style("border") = "2px solid red"
                Else
                    txtEstatusCliente.Text = "ACTIVO"
                    txtEstatusCliente.Style("border") = "2px solid green"
                End If
                txtLimiteCredito.Text = FormatNumber(drow("CreditLine").ToString, 2, TriState.True)
                txtSaldoCuenta.Text = FormatNumber(drow("Balance").ToString, 2, TriState.True)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('TraerEstadoCliente: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub SaldoConsignacion(cardcode As String)
        Try
            Dim Consulta As String =
                "select isnull(SUM(CONVERT(numeric(18,2),T3.Price)),0) [Total]" &
                " From OWHS t0 inner join OSRI t1 on t0.WhsCode = t1.WhsCode" &
                " inner Join OCRD t2 on t0.U_CardCode = t2.CardCode" &
                " INNER JOIN OSPP T3 ON T3.ItemCode = T1.ItemCode AND T3.CardCode = T2.CardCode" &
                " INNER Join OITM T4 ON T4.ItemCode = T1.ItemCode" &
                " LEFT JOIN [@AMODELO] T5 ON T4.U_MODELO = T5.Code" &
                " Left Join(SELECT distinct t1.ItemCode, t0.DocDate, t0.BaseNum, t1.MnfSerial, t1.SysNumber" &
                " FROM SRI1 t0 inner Join OSRN t1 on t0.SysSerial=t1.SysNumber And t0.ItemCode = t1.ItemCode And t0.BaseType = 67" &
                " WHERE t0.LineNum = (select max(LineNum) from SRI1 tt0 inner Join OSRN tt1 on tt0.SysSerial=tt1.SysNumber And tt0.BaseType = 67" &
                " where tt0.ItemCode = t0.ItemCode and tt1.SysNumber = t1.SysNumber)) t12 on t12.ItemCode = t1.ItemCode and t12.MnfSerial = t1.SuppSerial" &
                " where t2.CardCode = @p1 And T1.[Status] = 0"
            Dim params As New List(Of SqlParameter) From {New SqlParameter("@p1", cardcode)}
            Dim dt As DataTable = DbConfig.GetDataTable(Consulta, DbConfig.DBServer.MOVESA, params)
            If dt.Rows.Count > 0 Then
                txtSaldoConsignacion.Text = FormatNumber(dt.Rows(0)("Total").ToString, 2, TriState.True)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('SaldoConsignacion: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Function ToIntOrZeroSafe(ByVal input As String) As Integer
        If input Is Nothing Then Return 0
        Dim s As String = System.Web.HttpUtility.HtmlDecode(input).Replace(ChrW(160), " ").Trim()
        If s = "" Then Return 0
        Dim n As Integer
        If Integer.TryParse(s, NumberStyles.Integer, CultureInfo.CurrentCulture, n) Then Return n
        Dim d As Decimal
        If Decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, d) Then Return CInt(Math.Truncate(d))
        If Integer.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, n) Then Return n
        If Decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, d) Then Return CInt(Math.Truncate(d))
        s = s.Replace(",", "").Replace(".", "")
        If Integer.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, n) Then Return n
        Return 0
    End Function

    Private Sub BindGridDetallePlan(idPlan As String)
        Try
            Dim SQL_STRING As String =
                "SELECT *" &
                " ,isnull((SELECT sum([CANTIDAD]) FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS]" &
                " WHERE ALMDESTINO = [PLANIFICACIONES_DETALLE].Codigo And CODIGOESTADO IN ('DESPACHO ABIERTO','CAMION ABIERTO')),0)[Despacho]" &
                " ,0 AS [Indice_Proyectado]" &
                " FROM [ArmadoMotos].[dbo].[PLANIFICACIONES_DETALLE]" &
                " where [HEADERID] = @p1 order by 1,2"
            Dim params As New List(Of SqlParameter) From {New SqlParameter("@p1", idPlan)}
            Dim dt As DataTable = DbConfig.GetDataTable(SQL_STRING, DbConfig.DBServer.ARMADOMOTOS, params)
            gridAlmacenesDespachos.DataSource = dt
            gridAlmacenesDespachos.DataBind()
            gridAlmacenesDespachos.UseAccessibleHeader = True
            gridAlmacenesDespachos.HeaderRow.TableSection = TableRowSection.TableHeader
            For Each row As GridViewRow In gridAlmacenesDespachos.Rows
                Dim resultadoFinal As Decimal = 0
                Try
                    Dim celda8, celda9, celda14 As Decimal
                    If Decimal.TryParse(row.Cells(8).Text, celda8) AndAlso
                       Decimal.TryParse(row.Cells(9).Text, celda9) AndAlso
                       Decimal.TryParse(row.Cells(14).Text, celda14) AndAlso celda8 <> 0 Then
                        Dim r As Decimal = ((celda14 - celda9) / celda8) * 100D
                        resultadoFinal = If(r < 0, 0D, If(r > 100, 100D, Math.Round(r, 2)))
                    End If
                Catch : resultadoFinal = 0D : End Try
                row.Cells(7).Text = resultadoFinal.ToString("N2")
            Next
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDetallePlan: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
        sName = Replace(sName, "/", sChr) : sName = Replace(sName, "\", sChr)
        sName = Replace(sName, ":", sChr) : sName = Replace(sName, "?", sChr)
        sName = Replace(sName, Chr(34), sChr) : sName = Replace(sName, "<", sChr)
        sName = Replace(sName, ">", sChr) : sName = Replace(sName, "|", sChr)
        sName = Replace(sName, "&", sChr) : sName = Replace(sName, "%", sChr)
        sName = Replace(sName, "*", sChr) : sName = Replace(sName, "'", sChr)
        sName = Replace(sName, "{", sChr) : sName = Replace(sName, "[", sChr)
        sName = Replace(sName, "]", sChr) : sName = Replace(sName, "}", sChr)
        sName = Replace(sName, "!", sChr) : sName = Replace(sName, ",", sChr)
        Return sName
    End Function

    Public Sub AGREGAR_ALM_MANUAL(planid As String, whscode As String)
        Try
            Dim sql As String =
                "INSERT INTO [dbo].[PLANIFICACIONES_DETALLE]" &
                " ([CANAL],[RANKING],[RUTA],[CODIGO],[ALMACEN],[CUADRO],[COMP],[SOL],[TRANSITO],[FISICO]" &
                " ,[FALTANTE],[UNDS],[INDICE],[CINDICE],[CRANKING],[LEYENDA],[HEADERID],[ESTADOHEADER],[ESTADOLINEA]" &
                " ,[DESPACHOID],[DEPACHOFECHAINICIO],[DESPACHOFECHAVENCE],[USUARIODESPACHO],[FECHAACTUALIZACIONDESPACHO],[DESPACHOHEADER])" &
                " select TOP 1 [CANAL],[RANKING]" &
                " ,(SELECT [NAME] FROM MOVESA..[@CRUTA] WHERE CODE=(SELECT U_Ruta FROM MOVESA..OWHS WHERE WHSCODE=@whscode))[RUTA]" &
                " ,@whscode [CODIGO],(SELECT WHSNAME FROM MOVESA..OWHS WHERE WHSCODE=@whscode)[ALMACEN]" &
                " ,0,0,0,0,0,0,0,100,100,0,[LEYENDA],[HEADERID],[ESTADOHEADER],[ESTADOLINEA],[DESPACHOID]" &
                " ,[DEPACHOFECHAINICIO],[DESPACHOFECHAVENCE],[USUARIODESPACHO],[FECHAACTUALIZACIONDESPACHO],[DESPACHOHEADER]" &
                " from [dbo].[PLANIFICACIONES_DETALLE] where HEADERID=@planid"
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@whscode", whscode), New SqlParameter("@planid", planid)
            }
            DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS, params)
            BindGridDetallePlan(Request.QueryString("planid"))
            BindgridSugerido(Request.QueryString("planid"), Session("User"))
        Catch ex As Exception
            Response.Write("<script>console.log('AGREGAR_ALM_MANUAL: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridAlmacenesDespachos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridAlmacenesDespachos.RowCommand
        Try
            If e.CommandName = "Sugerir" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridAlmacenesDespachos.Rows(index)
                lblRuta.Text = gvRow.Cells(0).Text
                Session("rutaAlmacen") = gvRow.Cells(0).Text
                lblAlmActual.Text = gvRow.Cells(1).Text
                Session("almacenDestino") = gvRow.Cells(1).Text
                lblNombreAlmacen.Text = gvRow.Cells(2).Text
                BindGrid(gvRow.Cells(1).Text)
                BindgridSugerido(Request.QueryString("planid"), Session("User"))
                BindGridDetallePlan(Request.QueryString("planid"))
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridAlmacenesDespachos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub BindGrid(whscode As String)
        Try
            Dim sqlOrd As String = "SELECT MODELO FROM ORDENAMIENTO WHERE ACTIVOCD = 1"
            Dim dtOrd As DataTable = DbConfig.GetDataTable(sqlOrd, DbConfig.DBServer.ARMADOMOTOS)
            Dim modelosOrdenados As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            For Each r As DataRow In dtOrd.Rows
                modelosOrdenados.Add(r("MODELO").ToString().Trim())
            Next

            Dim sql_string As String = "EXEC [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS] @whscode"
            Dim params As New List(Of SqlParameter) From {New SqlParameter("@whscode", whscode)}
            Dim dt As DataTable = DbConfig.GetDataTable(sql_string, DbConfig.DBServer.ARMADOMOTOS, params)

            dt.Columns.Add("_OrdenPrioridad", GetType(Integer))
            For Each r As DataRow In dt.Rows
                r("_OrdenPrioridad") = If(modelosOrdenados.Contains(r("MODELO").ToString().Trim()), 0, 1)
            Next

            ' AGG_TOTAL = valor Logistica del SP (ya calculado, misma WHERE, sin query extra)
            ' AGG_RUV / AGG_RCB = misma cláusula que Logistica: MODELO+ALMDESTINO, sin filtro de plan
            dt.Columns.Add("AGG_RUV",   GetType(Integer))
            dt.Columns.Add("AGG_RCB",   GetType(Integer))
            dt.Columns.Add("AGG_TOTAL", GetType(Integer))
            For Each r As DataRow In dt.Rows
                Dim logVal As Integer = 0
                Integer.TryParse(If(r("Logistica") Is DBNull.Value, "0", r("Logistica").ToString()), logVal)
                r("AGG_TOTAL") = logVal
            Next
            Dim sqlAgg As String =
                "SELECT MODELO COLLATE Modern_Spanish_CI_AS AS MODELO," &
                " ISNULL(SUM(CASE WHEN ISNULL(RUV,0)=1 THEN ISNULL(QTYLOGISTICA,1) ELSE 0 END),0) AS SUM_RUV," &
                " ISNULL(SUM(CASE WHEN ISNULL(RCB,0)=1 THEN ISNULL(QTYLOGISTICA,1) ELSE 0 END),0) AS SUM_RCB" &
                " FROM ArmadoMotos.dbo.MACROINSERT" &
                " WHERE ESTADO='B' AND ALMDESTINO=@whscode" &
                " GROUP BY MODELO COLLATE Modern_Spanish_CI_AS"
            Dim paramsAgg As New List(Of SqlParameter) From {New SqlParameter("@whscode", whscode)}
            Dim dtAgg As DataTable = DbConfig.GetDataTable(sqlAgg, DbConfig.DBServer.ARMADOMOTOS, paramsAgg)
            Dim aggDict As New Dictionary(Of String, Integer())(StringComparer.OrdinalIgnoreCase)
            For Each agg As DataRow In dtAgg.Rows
                aggDict(agg("MODELO").ToString().Trim()) = New Integer() {
                    Convert.ToInt32(agg("SUM_RUV")),
                    Convert.ToInt32(agg("SUM_RCB"))
                }
            Next
            For Each r As DataRow In dt.Rows
                Dim modelo As String = r("MODELO").ToString().Trim()
                If aggDict.ContainsKey(modelo) Then
                    r("AGG_RUV") = aggDict(modelo)(0)
                    r("AGG_RCB") = aggDict(modelo)(1)
                End If
            Next

            Dim dv As New DataView(dt)
            dv.Sort = "_OrdenPrioridad ASC, ROWID ASC"
            gridCuadroBasico.DataSource = dv
            gridCuadroBasico.DataBind()

            If gridCuadroBasico.Rows.Count > 0 AndAlso gridCuadroBasico.HeaderRow IsNot Nothing Then
                gridCuadroBasico.UseAccessibleHeader = True
                gridCuadroBasico.HeaderRow.TableSection = TableRowSection.TableHeader
            End If

            ' Índices gridCuadroBasico:
            ' 0=Chk  1=ROWID  2=CODE  3=TemplateModelo  4=3m  5=6m  6=12m  7=%3m  8=%6m  9=%12m
            ' 10=DCM00  11=CB  12=Desp  13=Comp  14=Sol  15=Trans  16=Fisico  17=Faltante
            ' 18=VtaA  19=-30  20=-60  21=-90  22=Total4M  23=UltVenta  24=Tipo
            ' 25=AGG_RUV  26=AGG_RCB  27=AGG_TOTAL  28=Sugerir

            For Each row As GridViewRow In gridCuadroBasico.Rows
                Dim lblM As Label = TryCast(row.FindControl("lblModelo"), Label)
                If lblM IsNot Nothing AndAlso modelosOrdenados.Contains(lblM.Text.Trim()) Then
                    row.Cells(1).Text = "0"
                    row.Cells(1).BackColor = System.Drawing.Color.GreenYellow
                End If
            Next

            For Each row As GridViewRow In gridCuadroBasico.Rows
                row.Cells(17).Text = (ToIntOrZero(row.Cells(11).Text) -
                                      ToIntOrZero(row.Cells(12).Text) +
                                      ToIntOrZero(row.Cells(13).Text) -
                                      ToIntOrZero(row.Cells(14).Text) -
                                      ToIntOrZero(row.Cells(15).Text) -
                                      ToIntOrZero(row.Cells(16).Text)).ToString()
                If ToIntOrZero(row.Cells(17).Text) > 0 Then row.Cells(17).BackColor = System.Drawing.Color.Orange
                If ToIntOrZero(row.Cells(17).Text) < 0 Then
                    row.Cells(17).ToolTip = "Sugerido Mayor al Cuadro Basico!!"
                    row.Cells(17).BackColor = System.Drawing.Color.Red
                End If
                If ToIntOrZero(row.Cells(17).Text) > ToIntOrZero(row.Cells(11).Text) Then
                    row.Cells(11).ToolTip = "Cuadro Basico Menor a la Venta Actual!!"
                    row.Cells(11).BackColor = System.Drawing.Color.Red
                    row.Cells(17).BackColor = System.Drawing.Color.Red
                End If
                If ToIntOrZero(row.Cells(10).Text) > 0 Then
                    row.Cells(10).BackColor = System.Drawing.Color.LightBlue
                    row.Cells(10).Font.Bold = True
                End If
                ' Col 27=AGG_TOTAL
                If ToIntOrZero(row.Cells(27).Text) > 0 Then row.Cells(27).BackColor = System.Drawing.Color.LightGreen

                ' Pareto acumulado cols 7/8/9: SP devuelve fracción 0-1, mostramos × 100
                Dim par3 As Double = 0, par6 As Double = 0, par12 As Double = 0
                Double.TryParse(row.Cells(7).Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, par3)
                Double.TryParse(row.Cells(8).Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, par6)
                Double.TryParse(row.Cells(9).Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, par12)
                row.Cells(7).Text = (par3 * 100).ToString("N2") & "%"
                row.Cells(8).Text = (par6 * 100).ToString("N2") & "%"
                row.Cells(9).Text = (par12 * 100).ToString("N2") & "%"
                If par3 <= 0.5 Then row.Cells(7).BackColor = System.Drawing.Color.LightGreen Else If par3 <= 0.9 Then row.Cells(7).BackColor = System.Drawing.Color.LightYellow
                If par6 <= 0.5 Then row.Cells(8).BackColor = System.Drawing.Color.LightGreen Else If par6 <= 0.9 Then row.Cells(8).BackColor = System.Drawing.Color.LightYellow
                If par12 <= 0.5 Then row.Cells(9).BackColor = System.Drawing.Color.LightGreen Else If par12 <= 0.9 Then row.Cells(9).BackColor = System.Drawing.Color.LightYellow
            Next

            hfCurrentWhs.Value = whscode
            BindGridUltimas6(whscode)
            LoadColoresJSON(whscode)

        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    ' Precarga colores de todos los modelos del cuadro básico como JSON para JS
    Private Sub LoadColoresJSON(whscode As String)
        Try
            Dim codigos As New List(Of String)()
            For Each r As GridViewRow In gridCuadroBasico.Rows
                Dim cod As String = r.Cells(2).Text.Trim()
                If Not String.IsNullOrEmpty(cod) AndAlso Not codigos.Contains(cod) Then codigos.Add(cod)
            Next
            If codigos.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "coloresData",
                    "window._coloresData={};window._coloresCardCode='';", True)
                Return
            End If

            Dim paramNames As New List(Of String)()
            Dim pList As New List(Of SqlParameter)()
            pList.Add(New SqlParameter("@whs", whscode))
            For i As Integer = 0 To codigos.Count - 1
                paramNames.Add("@m" & i)
                pList.Add(New SqlParameter("@m" & i, codigos(i)))
            Next

            Dim sql As String =
                "SELECT t0.U_MODELO [CodModelo], t0.itemcode, t0.itemname," &
                " t1.Name [Modelo], ISNULL(CONVERT(int,t0.u_columna),0) [Espacios]," &
                " ISNULL((SELECT CONVERT(int,onhand) FROM movesa..OITW WITH(NOLOCK) WHERE WhsCode='DCM00' AND ItemCode=t0.ItemCode),0) [CEDIS]," &
                " ISNULL((SELECT CONVERT(int,onhand) FROM movesa..OITW WITH(NOLOCK) WHERE WhsCode=@whs   AND ItemCode=t0.ItemCode),0) [SUCURSAL]" &
                " FROM movesa..oitm t0 WITH(NOLOCK)" &
                " INNER JOIN movesa..[@AMODELO] t1 WITH(NOLOCK) ON t0.U_MODELO=t1.Code" &
                " WHERE t0.U_MODELO IN (" & String.Join(",", paramNames) & ")" &
                " AND t0.ItmsGrpCod=154 AND t0.frozenFor='N' AND t0.onhand>0" &
                " ORDER BY t0.U_MODELO, t0.itemcode"

            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA, pList)

            Dim dict As New Dictionary(Of String, List(Of String))(StringComparer.OrdinalIgnoreCase)
            For Each r As DataRow In dt.Rows
                Dim cod As String = r("CodModelo").ToString()
                If Not dict.ContainsKey(cod) Then dict(cod) = New List(Of String)()
                dict(cod).Add(String.Format(
                    "{{""code"":""{0}"",""name"":""{1}"",""modelo"":""{2}"",""esp"":{3},""cedis"":{4},""suc"":{5}}}",
                    JSEnc(r("itemcode").ToString()), JSEnc(r("itemname").ToString()),
                    JSEnc(r("Modelo").ToString()), r("Espacios"), r("CEDIS"), r("SUCURSAL")))
            Next

            Dim sb As New System.Text.StringBuilder("{")
            Dim first As Boolean = True
            For Each kv In dict
                If Not first Then sb.Append(",")
                sb.Append("""").Append(JSEnc(kv.Key)).Append(""":[").Append(String.Join(",", kv.Value)).Append("]")
                first = False
            Next
            sb.Append("}")

            Dim cc As String = JSEnc(getCardcode(whscode))
            Dim whsJs As String = JSEnc(whscode)
            Dim script As String =
                "window._coloresData=" & sb.ToString() & ";" &
                "window._coloresCardCode='" & cc & "';" &
                "try{sessionStorage.setItem('_colData_" & whsJs & "',JSON.stringify(window._coloresData));" &
                "sessionStorage.setItem('_colCC_" & whsJs & "','" & cc & "');}catch(e){}"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "coloresData", script, True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "coloresData",
                "window._coloresData={};window._coloresCardCode='';", True)
        End Try
    End Sub

    Private Function JSEnc(s As String) As String
        Return s.Replace("\", "\\").Replace("""", "\""").Replace("'", "\'").
                 Replace(vbCr, "").Replace(vbLf, "").Replace(vbTab, " ")
    End Function

    ' ══════════════════════════════════════════════════════════════════════════════════════
    ' VENTAS ÚLTIMOS 7 DÍAS — 1 fila por venta individual
    '
    ' columnas gridUltimas6:
    '   0=Expand(tmpl)  1=TaskID  2=Chk  3=Factura  4=Modelo  5=FechaVenta  6=UltTraslado
    '   7=Pareto  8=RankSuc  9=Mercadeo  10=StockSuc  11=StockDCM
    '   12=Desp(tmpl)  13=Trans  14=CB  15=Faltante  16=EnPedido  17=Nota
    ' ══════════════════════════════════════════════════════════════════════════════════════
    Private Sub BindGridUltimas6(whscode As String)
        Try
            Dim planId As String = If(String.IsNullOrEmpty(Request.QueryString("planid")), "0", Request.QueryString("planid"))
            Dim usuario As String = If(Session("User") IsNot Nothing, Session("User").ToString(), "")
            Dim canal As String = getWhsCanal(whscode)
            Dim diasVentas As Integer = If(canal = "CD", 15, 30)
            lblU6Titulo.Text = "Ventas &uacute;ltimos " & diasVentas & " d&iacute;as (" & canal & ") &mdash; Demanda Inmediata (1 fila por venta)"

            ' ── 1. Diccionarios desde gridCuadroBasico (ya ligado)
            '    hfROWID preserva el ROWID antes de que BindGrid lo sobreescriba con "0"
            Dim cbDict As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
            Dim rowIdDict As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
            For Each cbRow As GridViewRow In gridCuadroBasico.Rows
                Dim lblM As Label = TryCast(cbRow.FindControl("lblModelo"), Label)
                If lblM IsNot Nothing AndAlso Not String.IsNullOrEmpty(lblM.Text.Trim()) Then
                    Dim modelKey As String = lblM.Text.Trim()
                    cbDict(modelKey) = ToIntOrZero(cbRow.Cells(11).Text)
                    Dim hfRowId As HiddenField = TryCast(cbRow.FindControl("hfROWID"), HiddenField)
                    rowIdDict(modelKey) = If(hfRowId IsNot Nothing, ToIntOrZero(hfRowId.Value), ToIntOrZero(cbRow.Cells(1).Text))
                End If
            Next

            ' ── 2. SQL: 1 fila por venta individual, últimos 7 días
            Dim sql As String =
            "SELECT " &
            "  ROW_NUMBER() OVER (ORDER BY t0.DocDate DESC, t0.DocNum DESC) AS TaskID, " &
            "  t0.DocNum, " &
            "  ISNULL(t1.U_MSERIE,'') AS Serie, " &
            "  ISNULL(t1.U_MCOLOR,'') AS Color, " &
            "  t4.U_MODELO AS CodModelo, " &
            "  t5.Name     AS Modelo, " &
            "  t0.DocDate  AS FechaVenta, " &
            "  CAST(ISNULL(pb.PARETO * 100, 0) AS DECIMAL(5,2)) AS Pareto, " &
            "  CASE WHEN EXISTS ( " &
            "    SELECT 1 FROM ArmadoMotos.dbo.ORDENAMIENTO " &
            "    WHERE CONVERT(varchar(100),MODELO) COLLATE DATABASE_DEFAULT = CONVERT(varchar(100),t4.U_MODELO) COLLATE DATABASE_DEFAULT " &
            "      AND ACTIVOCD=1 " &
            "  ) THEN 'SI' ELSE 'NO' END AS Mercadeo, " &
            "  ISNULL(( " &
            "    SELECT SUM(CONVERT(int,w.OnHand)) FROM OITW w WITH(NOLOCK) " &
            "    INNER JOIN OITM i WITH(NOLOCK) ON w.ItemCode=i.ItemCode " &
            "    WHERE w.WhsCode=@whscode AND i.U_MODELO=t4.U_MODELO AND i.ItmsGrpCod=154 " &
            "  ),0) AS StockSuc, " &
            "  ISNULL(( " &
            "    SELECT SUM(CONVERT(int,w.OnHand)) FROM OITW w WITH(NOLOCK) " &
            "    INNER JOIN OITM i WITH(NOLOCK) ON w.ItemCode=i.ItemCode " &
            "    WHERE w.WhsCode='DCM00' AND i.U_MODELO=t4.U_MODELO AND i.ItmsGrpCod=154 " &
            "  ),0) AS StockDCM, " &
            "  ISNULL(( " &
            "    SELECT TOP 1 " &
            "      CONVERT(varchar(10),m.FECHA,103) + ' (' + CONVERT(varchar,m.CANTIDAD) + 'u)' " &
            "    FROM ArmadoMotos.dbo.MACROINSERT m " &
            "    WHERE m.ALMDESTINO COLLATE DATABASE_DEFAULT = @whscode " &
            "      AND CONVERT(varchar(100),m.MODELO) COLLATE DATABASE_DEFAULT = CONVERT(varchar(100),t5.Name) COLLATE DATABASE_DEFAULT " &
            "      AND m.ESTADO COLLATE DATABASE_DEFAULT <> 'B' " &
            "    ORDER BY m.FECHA DESC " &
            "  ),'—') AS UltTraslado, " &
            "  ISNULL(( " &
            "    SELECT SUM(CANTIDAD) FROM ArmadoMotos.dbo.DESPACHOS_DETALLE_MOTOS " &
            "    WHERE ALMDESTINO   COLLATE DATABASE_DEFAULT = @whscode " &
            "      AND CODMODELO = t4.U_MODELO " &
            "      AND CODIGOESTADO COLLATE DATABASE_DEFAULT IN ('DESPACHO ABIERTO','CAMION ABIERTO') " &
            "  ),0) AS Desp, " &
            "  ISNULL(( " &
            "    SELECT SUM(CONVERT(int,w.OnHand)) FROM MOVESA..OITW w WITH(NOLOCK) " &
            "    JOIN MOVESA..OITM i WITH(NOLOCK) ON i.ItemCode=w.ItemCode " &
            "    WHERE i.ItmsGrpCod=154 AND i.U_MODELO=t4.U_MODELO " &
            "      AND w.WhsCode='T'+RTRIM(@whscode) " &
            "  ),0) AS Trans, " &
            "  ISNULL(( " &
            "    SELECT TOP 1 'SI' FROM ArmadoMotos.dbo.MACROINSERT m " &
            "    WHERE CONVERT(varchar(100),m.MODELO) COLLATE DATABASE_DEFAULT = CONVERT(varchar(100),t5.Name) COLLATE DATABASE_DEFAULT " &
            "      AND m.ALMDESTINO COLLATE DATABASE_DEFAULT = @whscode " &
            "      AND m.ESTADO     COLLATE DATABASE_DEFAULT IN ('B','T') " &
            "      AND m.PLANID     = @planid " &
            "      AND m.USUARIO    COLLATE DATABASE_DEFAULT = @usuario COLLATE DATABASE_DEFAULT " &
            "  ),'NO') AS EnPedido " &
            "FROM OINV  t0 WITH(NOLOCK) " &
            "INNER JOIN INV1       t1 WITH(NOLOCK) ON t0.DocEntry=t1.DocEntry " &
            "INNER JOIN OITM       t4 WITH(NOLOCK) ON t1.ItemCode=t4.ItemCode " &
            "INNER JOIN [@AMODELO] t5 WITH(NOLOCK) ON t4.U_MODELO=t5.Code " &
            "LEFT  JOIN ArmadoMotos.dbo.PARETOBY_WHSCODE pb ON pb.MODELO COLLATE DATABASE_DEFAULT = t5.Name AND pb.MESES = 3 " &
            "WHERE t0.CANCELED='N' " &
            "  AND t1.WhsCode    = @whscode " &
            "  AND t4.ItmsGrpCod = 154 " &
            "  AND t0.DocDate   >= DATEADD(DAY,-(@dias-1), CAST(GETDATE() AS DATE)) " &
            "  AND NOT EXISTS ( " &
            "    SELECT 1 FROM ArmadoMotos.dbo.VENTAS_U7D_CONTROL ctrl " &
            "    WHERE ctrl.DOCNUM = t0.DocNum " &
            "      AND ctrl.WHSCODE COLLATE DATABASE_DEFAULT = @whscode " &
            "      AND ctrl.ESTADO  COLLATE DATABASE_DEFAULT = 'RESUELTO' " &
            "  ) " &
            "ORDER BY t0.DocDate DESC, t0.DocNum DESC"

            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@whscode", whscode),
                New SqlParameter("@planid", planId),
                New SqlParameter("@usuario", usuario),
                New SqlParameter("@dias", diasVentas)
            }
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA, params)

            ' ── 3. Agregar columnas CB, RankSuc, Faltante
            dt.Columns.Add("CB",       GetType(Integer))
            dt.Columns.Add("RankSuc",  GetType(Integer))
            dt.Columns.Add("Faltante", GetType(Integer))

            For Each drow As DataRow In dt.Rows
                Dim modelName As String = drow("Modelo").ToString().Trim()
                Dim cbV  As Integer = If(cbDict.ContainsKey(modelName), cbDict(modelName), 0)
                Dim stkV As Integer = Convert.ToInt32(drow("StockSuc"))
                Dim dspV As Integer = Convert.ToInt32(drow("Desp"))
                Dim trnV As Integer = Convert.ToInt32(drow("Trans"))
                drow("CB")       = cbV
                drow("RankSuc")  = If(rowIdDict.ContainsKey(modelName), rowIdDict(modelName), 0)
                drow("Faltante") = Math.Max(0, cbV - (stkV + dspV + trnV))
            Next

            gridUltimas6.DataSource = dt
            gridUltimas6.DataBind()

            If gridUltimas6.Rows.Count > 0 AndAlso gridUltimas6.HeaderRow IsNot Nothing Then
                gridUltimas6.UseAccessibleHeader = True
                gridUltimas6.HeaderRow.TableSection = TableRowSection.TableHeader
            End If

            ' ── 4. Tooltip de despachos: detalle por modelo para hover en col Despacho
            Dim sqlDespTip As String =
                "SELECT MODELO COLLATE Modern_Spanish_CI_AS AS MODELO," &
                " ISNULL(DESPACHOID,0) AS DESPACHOID, CODIGOESTADO," &
                " SUM(CANTIDAD) AS CANTIDAD," &
                " CONVERT(varchar(10),MAX(FECHA),103) AS FECHA" &
                " FROM ArmadoMotos.dbo.DESPACHOS_DETALLE_MOTOS" &
                " WHERE ALMDESTINO=@whscode" &
                "   AND CODIGOESTADO IN ('DESPACHO ABIERTO','CAMION ABIERTO')" &
                " GROUP BY MODELO COLLATE Modern_Spanish_CI_AS, ISNULL(DESPACHOID,0), CODIGOESTADO" &
                " ORDER BY MODELO COLLATE Modern_Spanish_CI_AS, MAX(FECHA) DESC"
            Dim dtDespTip As DataTable = DbConfig.GetDataTable(sqlDespTip, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@whscode", whscode)})
            Dim despTipDict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            For Each dr As DataRow In dtDespTip.Rows
                Dim mKey As String = dr("MODELO").ToString().Trim()
                Dim linea As String = String.Format("&#8226; #{0} &middot; {1} &middot; {2}u &middot; {3}",
                    dr("DESPACHOID"), dr("CODIGOESTADO"), dr("CANTIDAD"), dr("FECHA"))
                despTipDict(mKey) = If(despTipDict.ContainsKey(mKey), despTipDict(mKey) & "<br>" & linea, linea)
            Next

            ' ── 5. Post-binding: colores y badges
            ' 0=Expand  1=TaskID  2=Chk  3=Factura  4=Modelo  5=FechaVenta  6=UltTraslado
            ' 7=Pareto  8=RankSuc  9=Mercadeo  10=StockSuc  11=StockDCM
            ' 12=Desp(tmpl)  13=Trans  14=CB  15=Faltante  16=EnPedido  17=Nota
            Dim clrVerde As Drawing.Color = Drawing.Color.FromArgb(198, 239, 206)
            Dim clrNaranja As Drawing.Color = Drawing.Color.FromArgb(255, 235, 156)
            Dim clrRojo As Drawing.Color = Drawing.Color.FromArgb(255, 199, 206)
            Dim clrDCM As Drawing.Color = Drawing.Color.FromArgb(204, 229, 255)

            Dim cubiertas As Integer = 0
            For Each row As GridViewRow In gridUltimas6.Rows
                Dim pareto As String = row.Cells(7).Text.Trim()
                Dim rankVal As Integer = ToIntOrZero(row.Cells(8).Text)
                Dim mercadeo As String = row.Cells(9).Text.Trim()
                Dim stockSuc As Integer = ToIntOrZero(row.Cells(10).Text)
                Dim stockDCM As Integer = ToIntOrZero(row.Cells(11).Text)
                Dim trans As Integer = ToIntOrZero(row.Cells(13).Text)
                Dim cbVal As Integer = ToIntOrZero(row.Cells(14).Text)
                Dim enPedido As String = row.Cells(16).Text.Trim()

                Dim lblDespCtrl As Label = TryCast(row.FindControl("lblDespU6"), Label)
                Dim desp As Integer = If(lblDespCtrl IsNot Nothing, ToIntOrZero(lblDespCtrl.Text), 0)

                ' Tooltip de detalle en la etiqueta de despacho
                If lblDespCtrl IsNot Nothing AndAlso desp > 0 Then
                    Dim modeloRow As String = row.Cells(4).Text.Trim()
                    If despTipDict.ContainsKey(modeloRow) Then
                        Dim tipHtml As String =
                            "<b style='font-size:11px;'>Despachos abiertos</b><br>" & despTipDict(modeloRow)
                        lblDespCtrl.Attributes("data-toggle") = "tooltip"
                        lblDespCtrl.Attributes("data-html")   = "true"
                        lblDespCtrl.Attributes("data-placement") = "auto"
                        lblDespCtrl.Attributes("title")        = tipHtml
                        lblDespCtrl.CssClass = "desp-has-tip"
                    End If
                End If

                Dim available As Integer = stockSuc + desp + trans

                ' RankSuc: #N o "—"
                row.Cells(8).Text = If(rankVal > 0, "#" & rankVal.ToString(), "—")

                ' EnPedido badge
                If enPedido = "SI" Then
                    row.Cells(16).Text = "&#10004; SI"
                    cubiertas += 1
                Else
                    row.Cells(16).Text = "&#9888; NO"
                End If

                ' Color de fila
                If enPedido = "SI" Then
                    row.BackColor = clrVerde
                ElseIf available = 0 Then
                    row.BackColor = clrRojo
                    row.ForeColor = Drawing.Color.DarkRed
                ElseIf cbVal = 0 OrElse available >= cbVal Then
                    row.BackColor = clrVerde
                Else
                    row.BackColor = clrNaranja
                End If

                ' Badge Pareto — % acumulado → rango
                Dim paretoPct As Double = 0
                Double.TryParse(pareto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, paretoPct)
                Dim paretoLabel As String
                Dim paretoBg As String
                If paretoPct = 0 Then
                    paretoLabel = "—" : paretoBg = "#aaa"
                ElseIf paretoPct <= 40 Then
                    paretoLabel = "1-40%" : paretoBg = "#27ae60"
                ElseIf paretoPct <= 60 Then
                    paretoLabel = "41-60%" : paretoBg = "#2980b9"
                ElseIf paretoPct <= 80 Then
                    paretoLabel = "61-80%" : paretoBg = "#f39c12"
                Else
                    paretoLabel = "81-99%" : paretoBg = "#e74c3c"
                End If
                row.Cells(7).Text = "<span style=""background:" & paretoBg & ";color:#fff;padding:2px 7px;border-radius:10px;font-size:11px;font-weight:700;"" title=""Rango: " & paretoLabel & """>" & If(paretoPct > 0, paretoPct.ToString("N2") & "%", "—") & "</span>"

                ' Badge Mercadeo
                If mercadeo = "SI" Then
                    row.Cells(9).Text = "<span class=""badge-mercadeo"">&#9733; Mercadeo</span>"
                Else
                    row.Cells(9).Text = "<span style=""color:#aaa;font-size:11px;"">No</span>"
                End If

                ' Stock Suc
                If stockSuc = 0 Then
                    row.Cells(10).BackColor = clrRojo : row.Cells(10).ForeColor = Drawing.Color.DarkRed
                Else
                    row.Cells(10).BackColor = clrVerde : row.Cells(10).ForeColor = Drawing.Color.DarkGreen
                End If

                ' Stock DCM
                If stockDCM > 0 Then
                    row.Cells(11).BackColor = clrDCM : row.Cells(11).Font.Bold = True
                Else
                    row.Cells(11).BackColor = clrRojo : row.Cells(11).ForeColor = Drawing.Color.DarkRed
                End If

                If desp > 0 Then row.Cells(12).BackColor = clrNaranja
                If trans > 0 Then row.Cells(13).BackColor = clrVerde

                ' Col 15=Faltante
                Dim faltante As Integer = ToIntOrZero(row.Cells(15).Text)
                If faltante > 0 Then
                    row.Cells(15).BackColor = clrNaranja
                    row.Cells(15).ForeColor = Drawing.Color.DarkRed
                    row.Cells(15).Font.Bold = True
                End If
            Next

            lblResumen6.Text = cubiertas.ToString() & "/" & gridUltimas6.Rows.Count.ToString() & " cubiertas"

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridUltimas6: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub btnAbrirU6Modal_Click(sender As Object, e As EventArgs) Handles btnAbrirU6Modal.Click
        Try
            Dim codModelo As String = hfU6CodModelo.Value.Trim()
            If String.IsNullOrEmpty(codModelo) OrElse String.IsNullOrEmpty(lblAlmActual.Text) OrElse lblAlmActual.Text = "—" Then Return
            lblModeloCode.Text = codModelo : lblModeMoto.Text = codModelo
            lblSugerido.Text = "0" : lblFALTANTE.Text = "0"
            lblVTAA.Text = "0" : lblCB.Text = "0" : lblFISICO.Text = "0"
            lblrutaWhs.Text = lblRuta.Text
            lblAlmDestino.Text = lblAlmActual.Text
            lblcardcode.Text = getCardcode(lblAlmActual.Text)
            lblplanId.Text = Request.QueryString("planid")
            BindgridColoresMoto(codModelo, lblAlmActual.Text)
            ' Masivo → mover divInlineDetail al modal
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openModalU6", "showColoresInModal();", True)
        Catch ex As Exception
            Response.Write("<script>console.log('btnAbrirU6Modal: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Function ToIntOrZero(value As Object) As Integer
        If value Is Nothing OrElse String.IsNullOrWhiteSpace(value.ToString()) Then Return 0
        Dim result As Integer
        Return If(Integer.TryParse(value.ToString(), result), result, 0)
    End Function

    Public Function GetOrdenamiento(modelo As String) As String
        Dim sel As String = "IF EXISTS (SELECT 1 FROM ORDENAMIENTO WHERE MODELO=@p1 and ACTIVOCD=1) SELECT 'True'; ELSE SELECT 'False';"
        Dim params As New List(Of SqlParameter) From {New SqlParameter("@p1", modelo)}
        Dim result As Object = DbConfig.ExecuteScalar(sel, DbConfig.DBServer.ARMADOMOTOS, params)
        Return If(result IsNot Nothing, result.ToString(), "False")
    End Function

    ' ── gridCuadroBasico RowCommand
    ' "Sugerir" ahora es JS puro (expandInline) — sin postback
    ' "BI"      → abre ranking en nueva ventana
    Private Sub gridCuadroBasico_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCuadroBasico.RowCommand
        Try
            If e.CommandName = "BI" Then
                Dim modelo As String = e.CommandArgument.ToString().Trim()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenNewWindow",
                    "window.open('TrasladosCargaMacroBI.aspx?modelo=" & Server.UrlEncode(modelo) & "', '_blank');", True)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridCuadroBasico_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    ' ── gridUltimas6 RowCommand
    ' "ExpandU6" ahora es JS puro (expandInline) — sin postback
    Private Sub gridUltimas6_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridUltimas6.RowCommand
        ' Nada que manejar aquí — expand/colapso de colores es 100% JS
    End Sub

    Public Function getCardcode(whscode As String) As String
        Dim sel As String = "SELECT u_cardcode from owhs with(nolock) where whscode = @p1"
        Dim params As New List(Of SqlParameter) From {New SqlParameter("@p1", whscode)}
        Dim result As Object = DbConfig.ExecuteScalar(sel, DbConfig.DBServer.MOVESA, params)
        Return If(result IsNot Nothing, result.ToString(), "")
    End Function

    Private Sub BindgridColoresMoto(MODELO As String, WHSCODE As String)
        Try
            Dim sql As String =
                "select t0.itemcode, t0.itemname, t1.Name [Modelo], t0.u_columna [Espacios]" &
                " ,(select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode='DCM00' AND ItemCode=T0.ItemCode)[CEDIS]" &
                " ,(select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode=@whscode AND ItemCode=T0.ItemCode)[SUCURSAL]" &
                " From movesa..oitm t0 with(nolock) inner Join movesa..[@AMODELO] t1 with(nolock) On t0.U_MODELO=t1.Code" &
                " where t0.U_MODELO=@modelo and ItmsGrpCod=154 and t0.frozenFor='N' and t0.onhand>0"
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@whscode", WHSCODE), New SqlParameter("@modelo", MODELO)
            }
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA, params)
            gridColoresMoto.DataSource = dt
            gridColoresMoto.DataBind()
            gridColoresMoto.UseAccessibleHeader = True
            gridColoresMoto.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridColoresMoto: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function MacroInsert_Header(ORIGEN As String, DESTINO As String, RUTA As String, ESTADO As String,
                                       DATECREATED As DateTime, USERCREATED As String, FARMADO As Date, FDESPACHO As Date) As Integer
        Try
            Dim planid As Integer = 0
            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then planid = Request.QueryString("planId")
            Dim sel As String =
                "INSERT INTO [dbo].[MACROINSERT_HEADER]([ORIGEN],[DESTINO],[RUTA],[ESTADO],[DATECREATED],[USERCREATED],[FARMADO],[FDESPACHO],[ESTATUS],[PLANID])" &
                " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10); SELECT SCOPE_IDENTITY()"
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@p1", ORIGEN), New SqlParameter("@p2", DESTINO), New SqlParameter("@p3", RUTA), New SqlParameter("@p4", ESTADO),
                New SqlParameter("@p5", DATECREATED), New SqlParameter("@p6", USERCREATED), New SqlParameter("@p7", FARMADO), New SqlParameter("@p8", FDESPACHO),
                New SqlParameter("@p9", "A"), New SqlParameter("@p10", planid)
            }
            Return CInt(DbConfig.ExecuteScalar(sel, DbConfig.DBServer.ARMADOMOTOS, params))
        Catch : Return 0 : End Try
    End Function

    Public Function getWhsType(whscode As String) As String
        Dim sel As String = "SELECT u_type from owhs with(nolock) where whscode = @p1"
        Dim params As New List(Of SqlParameter) From {New SqlParameter("@p1", whscode)}
        Dim result As Object = DbConfig.ExecuteScalar(sel, DbConfig.DBServer.MOVESA, params)
        Return If(result IsNot Nothing, result.ToString(), "")
    End Function

    ' Maneja "Agregar" desde panel inline (JS, lee hfColorSelections) y desde modal masivo (gridColoresMoto)
    Private Sub btnModalColoresMotos_Click(sender As Object, e As EventArgs) Handles btnModalColoresMotos.Click
        Try
            Dim selectionsJson As String = hfColorSelections.Value
            hfColorSelections.Value = ""

            If Not String.IsNullOrEmpty(selectionsJson) AndAlso selectionsJson.StartsWith("{") Then
                ' ── Flujo JS individual: leer del JSON enviado por el cliente
                Dim jss As New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim payload As Dictionary(Of String, Object) = jss.Deserialize(Of Dictionary(Of String, Object))(selectionsJson)

                ' G: lee clave del payload con null-safe (JavaScriptSerializer puede retornar Nothing)
                Dim G = Function(k As String, def As String) _
                    If(payload.ContainsKey(k) AndAlso payload(k) IsNot Nothing, payload(k).ToString(), def)

                Dim ruta    As String  = G("ruta",       lblRuta.Text)
                Dim almOrig As String  = G("almOrigen",  "DCM00")
                Dim almDest As String  = G("almDestino", lblAlmActual.Text)
                Dim cc      As String  = G("cardcode",   "")
                Dim cbVal   As String  = G("cb",         "0")
                Dim fisVal  As String  = G("fisico",     "0")
                Dim falVal  As String  = G("faltante",   "0")
                Dim vtaVal  As String  = G("vtaa",       "0")
                Dim modCode As String  = G("modeloCode", "")
                Dim src     As String  = G("source",     "cb")
                Dim ruv As Integer = If(src = "u6", 1, 0)
                Dim rcb As Integer = If(src = "cb", 1, 0)

                ' JavaScriptSerializer deserializa arrays JSON como ArrayList, NO como Object()
                If payload.ContainsKey("items") AndAlso payload("items") IsNot Nothing Then
                    Dim rawItems As System.Collections.IEnumerable =
                        TryCast(payload("items"), System.Collections.IEnumerable)
                    If rawItems IsNot Nothing Then
                        For Each itemObj As Object In rawItems
                            Dim item As Dictionary(Of String, Object) =
                                TryCast(itemObj, Dictionary(Of String, Object))
                            If item Is Nothing Then Continue For
                            Dim I = Function(k As String, def As String) _
                                If(item.ContainsKey(k) AndAlso item(k) IsNot Nothing, item(k).ToString(), def)
                            Dim icode As String  = I("code",   "")
                            Dim iname As String  = I("name",   "")
                            Dim imod  As String  = I("modelo", "")
                            Dim iesp  As String  = I("esp",    "0")
                            Dim iqty  As Integer = 0
                            If item.ContainsKey("qty") AndAlso item("qty") IsNot Nothing Then
                                Integer.TryParse(item("qty").ToString(), iqty)
                            End If
                            For idx As Integer = 1 To iqty
                                InsertSugeridoSucursales(ruta, almOrig, almDest, cc, icode, imod, iname, iesp, 1, 1, 1, "TRASLADO", Date.Now, Session("User"), cbVal, fisVal, falVal, vtaVal, ruv, rcb)
                                If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                                    InsertSugeridoSucursalesDespacho(ruta, almOrig, almDest, cc, icode, imod, iname, iesp, 1, 1, 1, "TRASLADO", Date.Now, Session("User"), cbVal, fisVal, falVal, vtaVal, modCode)
                                End If
                            Next
                        Next
                    End If
                End If
            Else
                ' ── Flujo masivo U6: RUV=1, RCB=0 (siempre desde últimas ventas)
                For Each row As GridViewRow In gridColoresMoto.Rows
                    Dim qtyTextBox As TextBox = TryCast(row.FindControl("qty"), TextBox)
                    If qtyTextBox IsNot Nothing AndAlso Not String.IsNullOrEmpty(qtyTextBox.Text) Then
                        Dim cantidad As Integer
                        If Integer.TryParse(qtyTextBox.Text, cantidad) AndAlso cantidad > 0 Then
                            For i As Integer = 1 To cantidad
                                InsertSugeridoSucursales(
                                    lblRuta.Text, lblAlmOrigen.Text, lblAlmDestino.Text, lblcardcode.Text,
                                    row.Cells(0).Text, row.Cells(2).Text, row.Cells(1).Text,
                                    row.Cells(3).Text * 1, 1, 1, 1, "TRASLADO", Date.Now, Session("User"),
                                    lblCB.Text, lblFISICO.Text, lblFALTANTE.Text, lblVTAA.Text, RUV:=1, RCB:=0)
                                If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                                    InsertSugeridoSucursalesDespacho(
                                        lblRuta.Text, lblAlmOrigen.Text, lblAlmDestino.Text, lblcardcode.Text,
                                        row.Cells(0).Text, row.Cells(2).Text, row.Cells(1).Text,
                                        row.Cells(3).Text * 1, 1, 1, 1, "TRASLADO", Date.Now, Session("User"),
                                        lblCB.Text, lblFISICO.Text, lblFALTANTE.Text, lblVTAA.Text, lblModeloCode.Text)
                                End If
                            Next
                        End If
                    End If
                Next

                ' Continuar cola masivo U6 si hay pendientes
                Dim queue As String = hfU6Queue.Value
                If Not String.IsNullOrEmpty(queue) Then
                    Dim items() As String = queue.Split(","c)
                    Dim nextCod As String = items(0).Trim()
                    hfU6Queue.Value = If(items.Length > 1, String.Join(",", items.Skip(1).Select(Function(x) x.Trim()).ToArray()), "")
                    hfU6CodModelo.Value = nextCod
                    lblModeloCode.Text = nextCod : lblModeMoto.Text = nextCod
                    lblSugerido.Text = "0" : lblFALTANTE.Text = "0"
                    lblVTAA.Text = "0" : lblCB.Text = "0" : lblFISICO.Text = "0"
                    lblrutaWhs.Text = lblRuta.Text
                    lblAlmDestino.Text = lblAlmActual.Text
                    lblcardcode.Text = getCardcode(lblAlmActual.Text)
                    lblplanId.Text = Request.QueryString("planid")
                    BindgridColoresMoto(nextCod, lblAlmActual.Text)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openNextModal",
                        "setTimeout(function(){ showColoresInModal(); }, 300);", True)
                End If
            End If

            BindGrid(lblAlmActual.Text)
            BindGridDetallePlan(Request.QueryString("planid"))
            BindgridSugerido(Request.QueryString("planid"), Session("User"))
            getResumenPedidosCI()

        Catch ex As Exception
            Response.Write("<script>console.log('btnModalColoresMotos_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function InsertSugeridoSucursales(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String,
                                             ARTICULO As String, MODELO As String, DESCRIPCION As String, ESPACIOS As String,
                                             CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                             OBSERVACIONES As String, FECHA As Date, USUARIO As String,
                                             CB As String, FISICO As String, FALTANTE As String, VTAA As String,
                                             Optional RUV As Integer = 0, Optional RCB As Integer = 0) As Integer
        Try
            Dim planId As Integer = 0
            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then planId = Request.QueryString("planId")
            Dim SQL_QRY As String =
                "BEGIN INSERT INTO [dbo].[MACROINSERT]" &
                " ([RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],[ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS]," &
                " [CANTIDAD],[QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[ESTADO],[TIPO],[FECHA]," &
                " [USUARIO],[CB],[FISICO],[FALTANTE],[VTAA],[PLANID],[RUV],[RCB])" &
                " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23);" &
                " SELECT SCOPE_IDENTITY() AS InsertedID; END;"
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@p1", RUTA), New SqlParameter("@p2", ALMORIGEN), New SqlParameter("@p3", ALMDESTINO), New SqlParameter("@p4", CARDCODE),
                New SqlParameter("@p5", ARTICULO), New SqlParameter("@p6", MODELO), New SqlParameter("@p7", DESCRIPCION), New SqlParameter("@p8", ESPACIOS),
                New SqlParameter("@p9", CANTIDAD), New SqlParameter("@p10", QTYLOGISTICA), New SqlParameter("@p11", QTYSUCURSAL), New SqlParameter("@p12", OBSERVACIONES),
                New SqlParameter("@p13", "B"), New SqlParameter("@p14", getWhsCanal(ALMDESTINO)), New SqlParameter("@p15", FECHA), New SqlParameter("@p16", USUARIO),
                New SqlParameter("@p17", CB), New SqlParameter("@p18", FISICO), New SqlParameter("@p19", FALTANTE), New SqlParameter("@p20", VTAA),
                New SqlParameter("@p21", planId), New SqlParameter("@p22", RUV), New SqlParameter("@p23", RCB)
            }
            Return CInt(DbConfig.ExecuteScalar(SQL_QRY, DbConfig.DBServer.ARMADOMOTOS, params))
        Catch ex As Exception
            Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            Return 0
        End Try
    End Function

    Public Function InsertSugeridoSucursalesDespacho(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String,
                                                     ARTICULO As String, MODELO As String, DESCRIPCION As String, ESPACIOS As String,
                                                     CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                                     OBSERVACIONES As String, FECHA As Date, USUARIO As String,
                                                     CB As String, FISICO As String, FALTANTE As String, VTAA As String, CODMODELO As String) As Integer
        Try
            Dim planId As Integer = 0 : Dim headerId As Integer = 0
            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planId = Request.QueryString("planId") : headerId = Request.QueryString("headerId")
            End If
            Dim SQL_QRY As String =
                "BEGIN INSERT INTO [dbo].[DESPACHOS_DETALLE_MOTOS]" &
                " ([RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],[ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS]," &
                " [CANTIDAD],[QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[ESTADO],[TIPO],[FECHA]," &
                " [USUARIO],[CB],[FISICO],[FALTANTE],[VTAA],[PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO])" &
                " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23,@p24);" &
                " SELECT SCOPE_IDENTITY() AS InsertedID; END;"
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@p1", RUTA), New SqlParameter("@p2", ALMORIGEN), New SqlParameter("@p3", ALMDESTINO), New SqlParameter("@p4", CARDCODE),
                New SqlParameter("@p5", ARTICULO), New SqlParameter("@p6", MODELO), New SqlParameter("@p7", DESCRIPCION), New SqlParameter("@p8", ESPACIOS),
                New SqlParameter("@p9", CANTIDAD), New SqlParameter("@p10", QTYLOGISTICA), New SqlParameter("@p11", QTYSUCURSAL), New SqlParameter("@p12", OBSERVACIONES),
                New SqlParameter("@p13", "B"), New SqlParameter("@p14", getWhsCanal(ALMDESTINO)), New SqlParameter("@p15", FECHA), New SqlParameter("@p16", USUARIO),
                New SqlParameter("@p17", CB), New SqlParameter("@p18", FISICO), New SqlParameter("@p19", FALTANTE), New SqlParameter("@p20", VTAA),
                New SqlParameter("@p21", planId), New SqlParameter("@p22", headerId), New SqlParameter("@p23", "ABIERTO"), New SqlParameter("@p24", CODMODELO)
            }
            Return CInt(DbConfig.ExecuteScalar(SQL_QRY, DbConfig.DBServer.ARMADOMOTOS, params))
        Catch ex As Exception
            Response.Write("<script>console.log('InsertSugeridoSucursalesDespacho: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            Return 0
        End Try
    End Function

    Public Function getWhsCanal(whscode As String) As String
        Dim sel As String = "select case u_type when 'PRO' THEN 'CD' ELSE 'CI' END [CANAL] from movesa..owhs where whscode=@p1"
        Dim params As New List(Of SqlParameter) From {New SqlParameter("@p1", whscode)}
        Dim result As Object = DbConfig.ExecuteScalar(sel, DbConfig.DBServer.ARMADOMOTOS, params)
        Return If(result IsNot Nothing, result.ToString(), "")
    End Function

    Public Function getCustomerCode(whscode As String) As String
        Dim sel As String = "SELECT U_CARDCODE FROM MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE = @p1"
        Dim params As New List(Of SqlParameter) From {New SqlParameter("@p1", whscode)}
        Dim result As Object = DbConfig.ExecuteScalar(sel, DbConfig.DBServer.MOVESA, params)
        Return If(result IsNot Nothing, result.ToString(), "")
    End Function

    Private Sub BindgridSugerido(idPlan As String, usuario As String)
        Try
            Dim SqlQry As String =
                "SELECT ID,ALMDESTINO,ARTICULO,MODELO,DESCRIPCION,ESPACIOS,CANTIDAD,QTYLOGISTICA,OBSERVACIONES,ISNULL(RUV,0) RUV,ISNULL(RCB,0) RCB" &
                " FROM ArmadoMotos.dbo.MACROINSERT WHERE [ESTADO]='B' AND PLANID=@planId AND usuario=@usuario"
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@planId", idPlan), New SqlParameter("@usuario", usuario)
            }
            Dim dt As DataTable = DbConfig.GetDataTable(SqlQry, DbConfig.DBServer.ARMADOMOTOS, params)
            gridPedidoTemp.DataSource = dt
            gridPedidoTemp.DataBind()
            If gridPedidoTemp.Rows.Count > 0 AndAlso gridPedidoTemp.HeaderRow IsNot Nothing Then
                gridPedidoTemp.UseAccessibleHeader = True
                gridPedidoTemp.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
            Dim unidades As Integer = 0 : Dim espacios As Integer = 0
            For Each row As GridViewRow In gridPedidoTemp.Rows
                espacios += CInt(row.Cells(4).Text)
                unidades += CInt(row.Cells(5).Text)
            Next
            lblEspacios.Text = "Espacios Asignados: " & espacios
            lblUnidades.Text = "Unidades Asignadas: " & unidades
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnCancelColoresMotos_Click(sender As Object, e As EventArgs) Handles btnCancelColoresMotos.Click
        Try
            hfU6Queue.Value = ""
            BindGridDetallePlan(Request.QueryString("planid"))
            BindgridSugerido(Request.QueryString("planid"), Session("User"))
        Catch ex As Exception
        End Try
    End Sub

    Public Sub EliminarLinea(idlinea As Integer, itemcode As String, planid As String, almacen As String)
        Try
            Dim sel As String =
                "DELETE FROM [MACROINSERT] where [ID]=@p1;" &
                " DELETE FROM [DESPACHOS_DETALLE_MOTOS] WHERE PLANID=@planid AND [ARTICULO]=@itemcode" &
                " AND [ALMDESTINO]=@almacen AND [ESTADO]=@estado AND [USUARIO]=@usuario"
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@p1", idlinea), New SqlParameter("@itemcode", itemcode),
                New SqlParameter("@planid", planid), New SqlParameter("@almacen", almacen),
                New SqlParameter("@estado", "B"), New SqlParameter("@usuario", Session("User"))
            }
            DbConfig.ExecuteNonQuery(sel, DbConfig.DBServer.ARMADOMOTOS, params)
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridPedidoTemp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPedidoTemp.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridPedidoTemp.Rows(index)
                EliminarLinea(gvRow.Cells(0).Text, gvRow.Cells(2).Text, Request.QueryString("planid"), gvRow.Cells(1).Text)
                BindGrid(lblAlmActual.Text)
                BindGridDetallePlan(Request.QueryString("planid"))
                BindgridSugerido(Request.QueryString("planid"), Session("User"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript",
                    "<script>iziToast.warning({title:'OK!',message:'Moto Eliminada Con Exito!!!',position:'topRight',timeout:10000})</script>", False)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPedidoTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Try : Response.Redirect("TrasladosDDashboard.aspx") : Catch ex As Exception : End Try
    End Sub

    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Try : Response.Redirect("TrasladosDDashboardDetalleConfirmacion.aspx?planid=" & Request.QueryString("planid")) : Catch ex As Exception : End Try
    End Sub

    Public Sub actualizarDespachoDetalleMotos(headerid As String, planid As String)
        Try
            Dim sel As String = "update [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] set [HEADERID]=@headerId,[ESTADO]='T',[DESPACHOID]=@headerId where PLANID=@planId"
            Dim params As New List(Of SqlParameter) From {New SqlParameter("@headerId", headerid), New SqlParameter("@planId", planid)}
            DbConfig.ExecuteNonQuery(sel, DbConfig.DBServer.ARMADOMOTOS, params)
        Catch ex As Exception : End Try
    End Sub

    Public Sub actualizarMacroInsert(headerid As String, fdespacho As DateTime, planid As String)
        Try
            Dim sel As String =
                "update [ArmadoMotos].[dbo].[MACROINSERT] set [HEADERID]=@headerId," &
                " [ESTADO]='P',[FDESPACHO]=@fdespacho where PLANID=@planId AND USUARIO=@usuario"
            Dim params As New List(Of SqlParameter) From {
                New SqlParameter("@headerId", headerid), New SqlParameter("@fdespacho", fdespacho),
                New SqlParameter("@planId", planid), New SqlParameter("@usuario", Session("User"))
            }
            DbConfig.ExecuteNonQuery(sel, DbConfig.DBServer.ARMADOMOTOS, params)
        Catch ex As Exception
            Response.Write("<script>console.log('actualizarMacroInsert: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub getResumenPedidosCI()
        Try
            Dim sel As String = "EXEC MOVESA..SP_PORTAL_DISTRIBUIDORES @FUNCTION = 'PEDIDOS DISPONIBLE DESPACHO'"
            Dim dt As DataTable = DbConfig.GetDataTable(sel, DbConfig.DBServer.ARMADOMOTOS)
            lblUnidadesCI.Text = "Gen Unidades: " & dt.Rows.Count.ToString()
        Catch ex As Exception
            Response.Write("<script>console.log('getResumenPedidosCI: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnAgregarAlmacenManual_Click(sender As Object, e As EventArgs) Handles btnAgregarAlmacenManual.Click
        Try : AGREGAR_ALM_MANUAL(Request.QueryString("planid"), txtCodigoAlmacen.Text) : Catch ex As Exception : End Try
    End Sub

    Private Sub BindgridPortalPedidos()
        Try
            Dim SqlQry As String = "EXEC MOVESA..SP_PORTAL_DISTRIBUIDORES 'PEDIDOS DISPONIBLE DESPACHO'"
            Dim dt As DataTable = DbConfig.GetDataTable(SqlQry, DbConfig.DBServer.ARMADOMOTOS)
            gridMotosPortalPedidos.DataSource = dt
            gridMotosPortalPedidos.DataBind()
            gridMotosPortalPedidos.UseAccessibleHeader = True
            gridMotosPortalPedidos.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridPortalPedidos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

End Class

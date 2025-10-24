Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Partial Class TrasladosDDashboardDetalle
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public nuevoDespachoId As Integer = 0
    Private Sub TrasladosDDashboardDetalle_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else

                    BindGridDetallePlan(Request.QueryString("planid"))
                    BindgridSugerido(Request.QueryString("planid"), Session("User"))
                    getResumenPedidosCI()
                    BindgridPortalPedidos()
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub
    Public Sub TraerEstadoCliente(cardcode As String)
        Try
            Dim dt As New DataTable
            Dim conn As New SqlConnection(sCon1)
            Dim sqlString As String = "select frozenFor,Balance,CreditLine from movesa..ocrd where CardCode = @p1"

            Dim command As New SqlCommand(sqlString, conn)
            command.Parameters.AddWithValue("@p1", cardcode)
            Dim drd As SqlDataReader
            conn.Open()
            drd = command.ExecuteReader()
            If drd.Read() Then

                If drd.Item("frozenFor").ToString = "Y" Then
                    txtEstatusCliente.Text = "BLOQUEADO"
                    txtEstatusCliente.Style("border") = "2px solid red"
                    'btnCrearSolicitud.Enabled = False
                Else
                    txtEstatusCliente.Text = "ACTIVO"
                    txtEstatusCliente.Style("border") = "2px solid green"
                End If

                txtLimiteCredito.Text = FormatNumber(drd.Item("CreditLine").ToString, 2, TriState.True)
                txtSaldoCuenta.Text = FormatNumber(drd.Item("Balance").ToString, 2, TriState.True)
            End If
            conn.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('TraerEstadoCliente: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub SaldoConsignacion(cardcode As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select isnull(SUM(CONVERT(numeric(18,2),T3.Price)),0) [Total]" &
                                            " From OWHS t0 " &
                                            " inner join OSRI t1 on t0.WhsCode = t1.WhsCode" &
                                            " inner Join OCRD t2 on t0.U_CardCode = t2.CardCode" &
                                            " INNER JOIN OSPP T3 ON T3.ItemCode = T1.ItemCode AND T3.CardCode = T2.CardCode" &
                                            " INNER Join OITM T4 ON T4.ItemCode = T1.ItemCode " &
                                            " LEFT JOIN [@AMODELO] T5 ON T4.U_AMODELO = T5.Code" &
                                            " Left Join(SELECT  distinct t1.ItemCode, t0.DocDate, t0.BaseNum, t1.MnfSerial, t1.SysNumber" &
                                            " FROM SRI1  t0 " &
                                            " inner Join OSRN t1 on t0.SysSerial=t1.SysNumber And t0.ItemCode = t1.ItemCode And t0.BaseType = 67" &
                                            " WHERE  t0.LineNum = (select  max(LineNum) from SRI1  tt0 " &
                                            " inner Join OSRN tt1 on tt0.SysSerial=tt1.SysNumber And tt0.BaseType = 67 " &
                                            " where tt0.ItemCode = t0.ItemCode and tt1.SysNumber = t1.SysNumber)  ) t12 on t12.ItemCode = t1.ItemCode and  t12.MnfSerial = t1.SuppSerial" &
                                            " where t2.CardCode = @p1" &
                                            " And T1.[Status] = 0"

            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@p1", cardcode)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then
                txtSaldoConsignacion.Text = FormatNumber(drd.Item("Total").ToString, 2, TriState.True)
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('TraerEstadoCliente: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Private Sub BindgridCapacidadPiso(whscode As String)
        Try
            ' Limpiar y bind principal
            gridCapacidadPiso.DataSource = Nothing
            gridCapacidadPiso.DataBind()

            Using con As New SqlConnection(sCon2)
                Dim SqlQry As String = "EXEC ARMADOMOTOS..[SP_PORTAL_CAPACIDAD_TIENDAS] @FN = 'DESGLOCE ABASTECIMIENTO PISO TIENDAS', @WHSCODE = @whscode"
                Using cmd As New SqlCommand(SqlQry, con)
                    cmd.Parameters.AddWithValue("@whscode", whscode)
                    Using sda As New SqlDataAdapter(cmd)
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCapacidadPiso.DataSource = dt
                            gridCapacidadPiso.DataBind()
                        End Using
                    End Using
                End Using
            End Using

            ' Configurar header accesible
            If gridCapacidadPiso.Rows.Count > 0 AndAlso gridCapacidadPiso.HeaderRow IsNot Nothing Then
                gridCapacidadPiso.UseAccessibleHeader = True
                gridCapacidadPiso.HeaderRow.TableSection = TableRowSection.TableHeader
            End If

            ' Activar tab "third"
            Dim script As String = "<script>" & vbCrLf &
                               "  $(document).ready(function () {" & vbCrLf &
                               "    $('.menu .item[data-tab=""third""]').click();" & vbCrLf &
                               "    $('.ui.tab.segment').removeClass('active');" & vbCrLf &
                               "    $('.ui.tab.segment[data-tab=""third""]').addClass('active');" & vbCrLf &
                               "  });" & vbCrLf &
                               "</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "activarTabThird", script)

            '' ====== Segunda búsqueda: CPVENTAS (SEGMENTO, CAPACIDAD) por WHSCODE ======
            'Dim capBySeg As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
            'Using con2 As New SqlConnection(sCon2)
            '    Using cmd2 As New SqlCommand("SELECT SEGMENTO, CAPACIDAD FROM CPVENTAS WHERE WHSCODE = @whs", con2)
            '        cmd2.Parameters.AddWithValue("@whs", whscode)
            '        con2.Open()
            '        Using rd As SqlDataReader = cmd2.ExecuteReader()
            '            While rd.Read()
            '                Dim seg As String = Convert.ToString(rd("SEGMENTO")).Trim()
            '                Dim cap As Integer = 0
            '                If Not Convert.IsDBNull(rd("CAPACIDAD")) Then
            '                    cap = ToIntOrZero(Convert.ToString(rd("CAPACIDAD")))
            '                End If
            '                If seg <> "" Then capBySeg(seg) = cap
            '            End While
            '        End Using
            '    End Using
            'End Using

            '' Aplicar capacidad al grid: SEGMENTO (col 0) -> "Cap Aut" (col 1)
            'For Each r As GridViewRow In gridCapacidadPiso.Rows
            '    If r.RowType = DataControlRowType.DataRow Then
            '        Dim segRow As String = Server.HtmlDecode(r.Cells(0).Text).Trim()
            '        Dim cap As Integer
            '        If capBySeg.TryGetValue(segRow, cap) Then
            '            r.Cells(1).Text = cap.ToString()
            '            'r.Cells(6).Text = r.Cells(1).Text - r.Cells(2).Text - r.Cells(3).Text - r.Cells(4).Text - r.Cells(5).Text
            '        End If
            '    End If
            'Next

            ' === Crear diccionario para acumular las unidades por tipo ===
            Dim totalPorTipo As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)

            For Each row As GridViewRow In gridCuadroBasico.Rows
                Dim tipo As String = row.Cells(24).Text ' Columna TipoMoto (índice según orden real)
                Dim logistica As Integer = ToIntOrZero(row.Cells(23).Text) ' Columna Logistica

                If totalPorTipo.ContainsKey(tipo) Then
                    totalPorTipo(tipo) += logistica
                Else
                    totalPorTipo(tipo) = logistica
                End If
            Next

            ' === Asignar el total a gridCapacidadPiso ===
            For Each row As GridViewRow In gridCapacidadPiso.Rows
                Dim segmento As String = row.Cells(0).Text ' Columna SEGMENTO
                Dim lblSug As Label = CType(row.FindControl("lblSugerido"), Label)

                If totalPorTipo.ContainsKey(segmento) Then
                    lblSug.Text = totalPorTipo(segmento).ToString()
                Else
                    lblSug.Text = "0"
                End If
            Next

            ' Aplicar capacidad al grid: SEGMENTO (col 0) -> "Cap Aut" (col 1) y calcular Proyectado (col 6)
            For Each r As GridViewRow In gridCapacidadPiso.Rows
                If r.RowType <> DataControlRowType.DataRow Then Continue For

                Dim segRow As String = System.Web.HttpUtility.HtmlDecode(r.Cells(0).Text).Trim()

                '' Setear Cap Aut desde el diccionario si existe
                'Dim cap As Integer
                'If capBySeg.TryGetValue(segRow, cap) Then
                '    r.Cells(1).Text = cap.ToString()
                'End If

                ' Leer valores numéricos con seguridad
                Dim capAut As Integer = ToIntOrZeroSafe(r.Cells(1).Text)
                Dim invPiso As Integer = ToIntOrZeroSafe(r.Cells(2).Text)
                Dim transito As Integer = ToIntOrZeroSafe(r.Cells(3).Text)
                Dim despachos As Integer = ToIntOrZeroSafe(r.Cells(4).Text)

                Dim lblSug As Label = TryCast(r.FindControl("lblSugerido"), Label)
                Dim sugerido As Integer = If(lblSug IsNot Nothing, ToIntOrZeroSafe(lblSug.Text), 0)

                ' Calcula Inv Proyectado (o el campo que corresponda en tu col 6)
                Dim proyectado As Integer = invPiso + transito + despachos + sugerido
                Dim ExcesoFaltante As Integer = capAut - invPiso - transito - despachos - sugerido

                'If proyectado < 0 Then
                'r.Cells(6).Text = 0 ' Asegurar que no sea negativo
                'Else
                r.Cells(6).Text = proyectado.ToString()
                'End If


                r.Cells(7).Text = ExcesoFaltante.ToString()

                If proyectado > capAut Then
                    r.Cells(6).BackColor = Drawing.Color.Red   ' Sugerido = 5ta col (índice 4)
                    r.Cells(6).ForeColor = Drawing.Color.White
                Else
                    r.Cells(6).BackColor = Drawing.Color.LimeGreen   ' Sugerido = 5ta col (índice 4)
                    r.Cells(6).ForeColor = Drawing.Color.Black

                End If


            Next

            ' ====== Totales del footer (después de aplicar CAPACIDAD) ======
            Dim totCapAut As Integer = 0
            Dim totInvPiso As Integer = 0
            Dim totTransito As Integer = 0
            Dim totDesp As Integer = 0
            Dim totSugerido As Integer = 0
            Dim totProy As Integer = 0
            Dim totRestante As Integer = 0
            Dim totAbast As Integer = 0

            For Each r As GridViewRow In gridCapacidadPiso.Rows
                If r.RowType = DataControlRowType.DataRow Then
                    totCapAut += ToIntOrZero(Server.HtmlDecode(r.Cells(1).Text))
                    totInvPiso += ToIntOrZero(Server.HtmlDecode(r.Cells(2).Text))
                    totTransito += ToIntOrZero(Server.HtmlDecode(r.Cells(3).Text))
                    totDesp += ToIntOrZero(Server.HtmlDecode(r.Cells(4).Text))

                    Dim lbl As Label = TryCast(r.FindControl("lblSugerido"), Label)
                    If lbl IsNot Nothing Then totSugerido += ToIntOrZero(lbl.Text)

                    totProy += ToIntOrZero(Server.HtmlDecode(r.Cells(6).Text))
                    totRestante += ToIntOrZero(Server.HtmlDecode(r.Cells(7).Text))
                    totAbast += ToIntOrZero(Server.HtmlDecode(r.Cells(8).Text))
                End If
            Next

            If gridCapacidadPiso.ShowFooter AndAlso gridCapacidadPiso.FooterRow IsNot Nothing Then
                With gridCapacidadPiso.FooterRow
                    .Cells(0).Text = "Totales"
                    .Cells(1).Text = totCapAut.ToString()
                    .Cells(2).Text = totInvPiso.ToString()
                    .Cells(3).Text = totTransito.ToString()
                    .Cells(4).Text = totDesp.ToString()
                    .Cells(5).Text = totSugerido.ToString()
                    .Cells(6).Text = totProy.ToString()
                    .Cells(7).Text = totRestante.ToString()
                    .Cells(8).Text = totAbast.ToString()
                    .Font.Bold = True
                End With

                lblPromedio1.Text = FormatNumber(((totInvPiso / totCapAut) * 100), 2, TriState.True)
                lblPromedio2.Text = FormatNumber(((totProy / totCapAut) * 100), 2, TriState.True)

                ' Alinear footer al centro y marcar como <tfoot>
                For Each cell As TableCell In gridCapacidadPiso.FooterRow.Cells
                    cell.HorizontalAlign = HorizontalAlign.Center
                Next
                gridCapacidadPiso.FooterRow.TableSection = TableRowSection.TableFooter

                ' Mantener el color skyblue en "Sugerido" (col 5)
                gridCapacidadPiso.FooterRow.CssClass = "table-secondary"
                gridCapacidadPiso.FooterRow.Cells(5).Style.Add("background-color", "skyblue")
                gridCapacidadPiso.FooterRow.Cells(5).Style.Add("color", "black")
            End If

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridCapacidadPiso: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    ' Convierte texto (incluye &nbsp;) a entero; vacío = 0
    Private Function ToIntOrZeroSafe(ByVal input As String) As Integer
        If input Is Nothing Then Return 0

        ' Decodifica HTML y limpia NBSP (160)
        Dim s As String = System.Web.HttpUtility.HtmlDecode(input).Replace(ChrW(160), " ").Trim()
        If s = "" Then Return 0

        ' 1) Int con cultura actual
        Dim n As Integer
        If Integer.TryParse(s, NumberStyles.Integer, CultureInfo.CurrentCulture, n) Then Return n

        ' 2) Decimal con cultura actual (por si viene "123.00" / "123,00")
        Dim d As Decimal
        If Decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, d) Then Return CInt(Math.Truncate(d))

        ' 3) Int con cultura invariante
        If Integer.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, n) Then Return n

        ' 4) Decimal con cultura invariante
        If Decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, d) Then Return CInt(Math.Truncate(d))

        ' 5) Fallback: quita separadores comunes y reintenta
        s = s.Replace(",", "").Replace(".", "")
        If Integer.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, n) Then Return n

        Return 0
    End Function
    Private Sub BindGridDetallePlan(idPlan As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = "SELECT * " &
                        " ,isnull((SELECT sum([CANTIDAD])  FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS]  " &
                        " WHERE ALMDESTINO = [PLANIFICACIONES_DETALLE].Codigo And CODIGOESTADO IN ('DESPACHO ABIERTO','CAMION ABIERTO')),0)[Despacho] " &
                        ",0 AS [Indice_Proyectado]  " &
                        " FROM [ArmadoMotos].[dbo].[PLANIFICACIONES_DETALLE] " &
                        " where [HEADERID] = @p1 order by 1,2"

                Using cmd As New SqlCommand(SQL_STRING)
                    cmd.Parameters.AddWithValue("@p1", idPlan)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridAlmacenesDespachos.DataSource = dt
                            gridAlmacenesDespachos.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridAlmacenesDespachos.UseAccessibleHeader = True
            gridAlmacenesDespachos.HeaderRow.TableSection = TableRowSection.TableHeader

            For Each row As GridViewRow In gridAlmacenesDespachos.Rows
                Dim resultadoFinal As Decimal = 0 ' Valor por defecto si hay error

                Try
                    ' Verificar que las celdas no estén vacías
                    If Not String.IsNullOrEmpty(row.Cells(8).Text) AndAlso
                   Not String.IsNullOrEmpty(row.Cells(9).Text) AndAlso
                   Not String.IsNullOrEmpty(row.Cells(14).Text) Then

                        ' Intentar convertir los valores a decimal
                        Dim celda8 As Decimal
                        Dim celda9 As Decimal
                        Dim celda14 As Decimal

                        If Decimal.TryParse(row.Cells(8).Text, celda8) AndAlso
                       Decimal.TryParse(row.Cells(9).Text, celda9) AndAlso
                       Decimal.TryParse(row.Cells(14).Text, celda14) Then

                            ' Evitar división por cero
                            If celda8 <> 0 Then
                                ' Calcular el resultado
                                Dim resultadoCalculado As Decimal = ((celda14 - celda9) / celda8) * 100D

                                ' Aplicar límites
                                If resultadoCalculado < 0 Then
                                    resultadoFinal = 0D
                                ElseIf resultadoCalculado > 100 Then
                                    resultadoFinal = 100D
                                Else
                                    resultadoFinal = Math.Round(resultadoCalculado, 2) ' Redondear a 2 decimales
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    resultadoFinal = 0D ' Si ocurre cualquier error, establecer a cero
                End Try

                ' Asignar el resultado formateado a la celda
                row.Cells(7).Text = resultadoFinal.ToString("N2") ' Formato con 2 decimales
            Next

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDetallePlan: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function ExisteDespacho(codigo As String, planId As String) As Boolean
        Dim existe As Boolean = False

        Try
            Dim sCon As String = sCon2
            Using con As New SqlConnection(sCon)
                Dim query As String = "SELECT COUNT(*) FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE] WHERE CODIGO = @p1 AND PLANID = @p2 and ESTADOLINEA = 'Pendiente'"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@p1", codigo)
                    cmd.Parameters.AddWithValue("@p2", planId)

                    con.Open()
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    If count > 0 Then
                        existe = True
                    End If
                End Using
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('Error en ExisteDespacho: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try

        Return existe
    End Function


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
        sName = Replace(sName, ",", sChr)

        NewStr = sName
        Return NewStr
    End Function
    Public Function CrearNuevoDespacho(planid As Integer, fechacreacion As DateTime, estado As String, fechainicio As DateTime, fechavence As DateTime, usuario As String) As Integer
        Try
            Dim sCon As String = sCon2
            Dim sel As String = "INSERT INTO [dbo].[DEPACHOS_HEADER] ([PLANID],[FECHACREACION],[ESTADO],[FECHAINICIO],[FECHAVENCE],[USUARIO]) " &
                                "VALUES (@p1, @p2, @p3, @p4, @p5, @p6); " &
                                "SELECT SCOPE_IDENTITY();"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.Add("@p1", SqlDbType.Int).Value = planid
                cmd.Parameters.Add("@p2", SqlDbType.DateTime).Value = fechacreacion
                cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = estado
                cmd.Parameters.Add("@p4", SqlDbType.Date).Value = fechainicio
                cmd.Parameters.Add("@p5", SqlDbType.Date).Value = fechavence
                cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50).Value = usuario

                con.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar())
                con.Close()
            End Using
        Catch ex As Exception
            ' Retorna 0 si ocurre un error
            Return 0
        End Try
    End Function
    Public Sub AGREGAR_ALM_MANUAL(planid As String, whscode As String)
        Try
            Dim sCon As String = sCon2
            Using con As New SqlConnection(sCon)
                con.Open()

                Using cmd As New SqlCommand("INSERT INTO [dbo].[PLANIFICACIONES_DETALLE] " &
                        " ([CANAL],[RANKING],[RUTA],[CODIGO],[ALMACEN] " &
                        " ,[CUADRO],[COMP],[SOL],[TRANSITO],[FISICO] " &
                        " ,[FALTANTE],[UNDS],[INDICE],[CINDICE],[CRANKING] " &
                        " ,[LEYENDA],[HEADERID],[ESTADOHEADER],[ESTADOLINEA] " &
                        " ,[DESPACHOID],[DEPACHOFECHAINICIO],[DESPACHOFECHAVENCE] " &
                        " ,[USUARIODESPACHO],[FECHAACTUALIZACIONDESPACHO],[DESPACHOHEADER]) " &
                        " select  TOP 1 [CANAL],[RANKING] " &
                        " ,(SELECT [NAME] FROM MOVESA..[@CRUTA] WHERE CODE =(SELECT U_Ruta FROM MOVESA..OWHS WHERE WHSCODE=@whscode))[RUTA] " &
                        " ,@whscode [CODIGO],(SELECT WHSNAME FROM MOVESA..OWHS WHERE WHSCODE=@whscode)[ALMACEN] " &
                        " ,0,0,0,0,0,0,0,100,100,0,[LEYENDA],[HEADERID],[ESTADOHEADER],[ESTADOLINEA],[DESPACHOID] " &
                        " ,[DEPACHOFECHAINICIO],[DESPACHOFECHAVENCE],[USUARIODESPACHO],[FECHAACTUALIZACIONDESPACHO] " &
                        " ,[DESPACHOHEADER] " &
                        " from [dbo].[PLANIFICACIONES_DETALLE] where HEADERID=@planid " &
                        " ", con)
                    cmd.Parameters.AddWithValue("@whscode", whscode)
                    cmd.Parameters.AddWithValue("@planid", planid)
                    cmd.ExecuteNonQuery()
                End Using
                con.Close()
            End Using
            BindGridDetallePlan(Request.QueryString("planid"))
            BindgridSugerido(Request.QueryString("planid"), Session("User"))
        Catch ex As Exception
            Response.Write("<script>console.log('Error en InsertarDespachosSeleccionados: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function EliminarOnError(despachoid As String) As Boolean
        Try
            Dim sCon As String = sCon2
            Dim sel As String = "DELETE FROM [dbo].[DEPACHOS_HEADER] WHERE ID = @p1); "
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.Add("@p1", SqlDbType.Int).Value = despachoid
                con.Open()
                cmd.ExecuteScalar()
                con.Close()
                Return True
            End Using
        Catch ex As Exception
            ' Retorna 0 si ocurre un error
            Return False
        End Try
    End Function

    Private Sub gridAlmacenesDespachos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridAlmacenesDespachos.RowCommand
        Try
            If e.CommandName = "Sugerir" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridAlmacenesDespachos.Rows(index)

                ' Aquí lees lo que ya tenías
                lblRuta.Text = gvRow.Cells(0).Text
                Session("rutaAlmacen") = gvRow.Cells(0).Text
                lblAlmActual.Text = gvRow.Cells(4).Text
                Session("almacenDestino") = gvRow.Cells(4).Text
                lblNombreAlmacen.Text = gvRow.Cells(5).Text

                lblWhsCardcode.Text = getCustomerCode(gvRow.Cells(4).Text)

                BindGrid(gvRow.Cells(4).Text)
                BindgridSugerido(Request.QueryString("planid"), Session("User"))
                BindGridDetallePlan(Request.QueryString("planid"))
                'BindgridCapacidadPiso(gvRow.Cells(4).Text)

                lblCurrentWHscode.Text = "Almacen Actual: " & gvRow.Cells(4).Text & " - " & gvRow.Cells(5).Text

            End If

        Catch ex As Exception
            Response.Write("<script>console.log('gridAlmacenesDespachos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGrid(whscode As String)
        Try
            Dim tipoalmacen As String = getWhsType(whscode)

            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String

                sql_string = "EXEC [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS] @whscode "

                Using cmd As New SqlCommand(sql_string)
                    cmd.Parameters.AddWithValue("@whscode", whscode)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCuadroBasico.DataSource = dt
                            gridCuadroBasico.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridCuadroBasico.UseAccessibleHeader = True
            gridCuadroBasico.HeaderRow.TableSection = TableRowSection.TableHeader

            For Each row As GridViewRow In gridCuadroBasico.Rows
                Dim lblModelo As Label = TryCast(row.FindControl("lblModelo"), Label)

                If GetOrdenamiento(lblModelo.Text.ToString.Trim) = "True" Then
                    row.Cells(0).Text = 0
                    row.Cells(0).BackColor = System.Drawing.Color.GreenYellow
                End If
            Next

            For Each row As GridViewRow In gridCuadroBasico.Rows

                row.Cells(16).Text = (ToIntOrZero(row.Cells(10).Text) -
                          ToIntOrZero(row.Cells(11).Text) +
                          ToIntOrZero(row.Cells(12).Text) -
                          ToIntOrZero(row.Cells(13).Text) -
                          ToIntOrZero(row.Cells(14).Text) -
                          ToIntOrZero(row.Cells(15).Text)).ToString()


                If row.Cells(16).Text > 0 Then
                    row.Cells(16).BackColor = System.Drawing.Color.Orange

                ElseIf row.Cells(16).Text < 0 Then
                    row.Cells(16).ToolTip = "Sugerido Mayor al Cuadro Basico!!"
                    row.Cells(16).BackColor = System.Drawing.Color.Red
                End If

                If row.Cells(16).Text > row.Cells(10).Text Then
                    row.Cells(10).ToolTip = "Cuadro Basico Menor a la Venta Actual!!"
                    row.Cells(10).BackColor = System.Drawing.Color.Red
                    row.Cells(16).BackColor = System.Drawing.Color.Red
                End If
                If row.Cells(23).Text > 0 Then
                    row.Cells(23).BackColor = System.Drawing.Color.LightGreen
                End If
            Next

            BindgridCapacidadPiso(whscode)

            '' === Crear diccionario para acumular las unidades por tipo ===
            'Dim totalPorTipo As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)

            'For Each row As GridViewRow In gridCuadroBasico.Rows
            '    Dim tipo As String = row.Cells(24).Text ' Columna TipoMoto (índice según orden real)
            '    Dim logistica As Integer = ToIntOrZero(row.Cells(23).Text) ' Columna Logistica

            '    If totalPorTipo.ContainsKey(tipo) Then
            '        totalPorTipo(tipo) += logistica
            '    Else
            '        totalPorTipo(tipo) = logistica
            '    End If
            'Next

            '' === Asignar el total a gridCapacidadPiso ===
            'For Each row As GridViewRow In gridCapacidadPiso.Rows
            '    Dim segmento As String = row.Cells(0).Text ' Columna SEGMENTO
            '    Dim lblSug As Label = CType(row.FindControl("lblSugerido"), Label)

            '    If totalPorTipo.ContainsKey(segmento) Then
            '        lblSug.Text = totalPorTipo(segmento).ToString()
            '    Else
            '        lblSug.Text = "0"
            '    End If
            'Next

            '' ====== Avisos con SweetAlert2 cuando Sugerido > Cap Aut (secuencial) ======
            'Dim sb As New StringBuilder()
            'sb.AppendLine("(function(){")
            'sb.AppendLine("  async function fireAlerts(){")
            'sb.AppendLine("    var alerts = [];")

            'For Each row As GridViewRow In gridCapacidadPiso.Rows
            '    If row.RowType = DataControlRowType.DataRow Then
            '        Dim segmento As String = Server.HtmlDecode(row.Cells(0).Text)       ' SEGMENTO
            '        Dim capAut As Integer = ToIntOrZero(Server.HtmlDecode(row.Cells(1).Text)) ' Cap Aut
            '        Dim lblSug As Label = TryCast(row.FindControl("lblSugerido"), Label)
            '        Dim sugerido As Integer = If(lblSug IsNot Nothing, ToIntOrZero(lblSug.Text), 0)
            '        Dim sugeridofinal = sugerido + ToIntOrZero(Server.HtmlDecode(row.Cells(2).Text)) + ToIntOrZero(Server.HtmlDecode(row.Cells(3).Text)) + ToIntOrZero(Server.HtmlDecode(row.Cells(4).Text))


            '        If sugeridofinal > capAut Then
            '            ' Pintar la celda Sugerido en rojo (visual)
            '            row.Cells(5).BackColor = Drawing.Color.Red   ' Sugerido = 5ta col (índice 4)
            '            row.Cells(5).ForeColor = Drawing.Color.White

            '            ' Agregar mensaje a la cola
            '            Dim msg As String = segmento & ": Sugerido " & sugerido & " > Cap. Autorizada " & capAut
            '            Dim jsMsg As String = HttpUtility.JavaScriptStringEncode(msg)
            '            sb.AppendLine("    alerts.push('" & jsMsg & "');")
            '        End If
            '    End If
            'Next

            'sb.AppendLine("    for (let i = 0; i < alerts.length; i++) {")
            'sb.AppendLine("      await Swal.fire({")
            'sb.AppendLine("        icon: 'warning',")
            'sb.AppendLine("        title: 'Capacidad excedida',")
            'sb.AppendLine("        text: alerts[i],")
            'sb.AppendLine("        confirmButtonText: 'Entendido'")
            'sb.AppendLine("      });")
            'sb.AppendLine("    }")
            'sb.AppendLine("  }")

            '' Cargar SweetAlert2 si hace falta y luego ejecutar la cola
            'sb.AppendLine("  if (window.Swal) { fireAlerts(); } else {")
            'sb.AppendLine("    var s = document.createElement('script');")
            'sb.AppendLine("    s.src = 'https://cdn.jsdelivr.net/npm/sweetalert2@11';")
            'sb.AppendLine("    s.onload = fireAlerts;")
            'sb.AppendLine("    document.head.appendChild(s);")
            'sb.AppendLine("  }")
            'sb.AppendLine("})();")

            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "excesoCapQueue", sb.ToString(), True)
        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Function ToIntOrZero(value As Object) As Integer
        If value Is Nothing OrElse String.IsNullOrWhiteSpace(value.ToString()) Then
            Return 0
        End If

        Dim result As Integer
        If Integer.TryParse(value.ToString(), result) Then
            Return result
        Else
            Return 0
        End If
    End Function
    Public Function GetOrdenamiento(modelo As String) As String
        Dim sCon As String = sCon2
        Dim sel As String

        sel = " IF EXISTS (SELECT 1 FROM ORDENAMIENTO WHERE MODELO = @p1 and ACTIVOCD=1) " &
                  "       SELECT 'True'; " &
                  "  ELSE " &
                  "      SELECT 'False';"

        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", modelo)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Private Sub gridCuadroBasico_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCuadroBasico.RowCommand
        Try
            If e.CommandName = "BI" Then

                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCuadroBasico.Rows(index - 1)
                Dim lblModelo As Label = CType(gvRow.FindControl("lblModelo"), Label)
                Dim modelo As String = ""

                If lblModelo IsNot Nothing Then
                    modelo = lblModelo.Text.Trim()
                End If

                Dim url As String = "TrasladosCargaMacroBI.aspx?modelo=" & modelo
                Dim script As String = "window.open('" & url & "', '_blank');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenNewWindow", script, True)
            End If

            If e.CommandName = "Sugerir" Then

                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCuadroBasico.Rows(index)
                Dim lblModelo As Label = CType(gvRow.FindControl("lblModelo"), Label)
                Dim modelo As String = ""

                lblModeloCode.Text = gvRow.Cells(1).Text
                lblModeMoto.Text = lblModelo.Text
                lblSugerido.Text = gvRow.Cells(10).Text
                lblFALTANTE.Text = gvRow.Cells(10).Text
                lblVTAA.Text = gvRow.Cells(11).Text
                lblrutaWhs.Text = lblRuta.Text
                lblAlmDestino.Text = lblAlmActual.Text
                lblCB.Text = gvRow.Cells(4).Text
                lblplanId.Text = lblplanId.Text
                lblFISICO.Text = IIf(String.IsNullOrEmpty(gvRow.Cells(9).Text), 0, gvRow.Cells(9).Text)
                lblcardcode.Text = getCardcode(lblAlmActual.Text)
                lblplanId.Text = Request.QueryString("planid")

                BindgridColoresMoto(gvRow.Cells(1).Text, lblAlmActual.Text)
                ModalPopupColoresMotos.Show()

            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridCuadroBasico_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function getCardcode(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT u_cardcode from owhs with(nolock) where whscode = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getCardcode: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Private Sub BindgridColoresMoto(MODELO As String, WHSCODE As String)
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("select	t0.itemcode, t0.itemname, t1.Name [Modelo], t0.u_columna [Espacios] " &
                        " , (select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode='DCM00' AND ItemCode=T0.ItemCode)[CEDIS] " &
                        " ,(select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode = @whscode AND ItemCode=T0.ItemCode)[SUCURSAL] " &
                        " From movesa..oitm t0 with(nolock)  inner Join movesa..[@AMODELO] t1 with(nolock) On t0.U_AMODELO=t1.Code " &
                        " where t0.U_AMODELO = @modelo and ItmsGrpCod=154 and t0.frozenFor='N' and t0.onhand>0")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@whscode", WHSCODE)
                        cmd.Parameters.AddWithValue("@modelo", MODELO)
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridColoresMoto.DataSource = dt
                            gridColoresMoto.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridColoresMoto.UseAccessibleHeader = True
            gridColoresMoto.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridColoresMoto: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function MacroInsert_Header(ORIGEN As String, DESTINO As String, RUTA As String, ESTADO As String,
                                       DATECREATED As DateTime, USERCREATED As String,
                                       FARMADO As Date, FDESPACHO As Date) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            Dim planid As Integer = 0

            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planid = Request.QueryString("planId")
            End If


            sel = "INSERT INTO [dbo].[MACROINSERT_HEADER]([ORIGEN],[DESTINO],[RUTA],[ESTADO],[DATECREATED],[USERCREATED], [FARMADO], [FDESPACHO],[ESTATUS],[PLANID])" &
                    " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10); SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ORIGEN)
                cmd.Parameters.AddWithValue("@p2", DESTINO)
                cmd.Parameters.AddWithValue("@p3", RUTA)
                cmd.Parameters.AddWithValue("@p4", ESTADO)
                cmd.Parameters.AddWithValue("@p5", DATECREATED)
                cmd.Parameters.AddWithValue("@p6", USERCREATED)
                cmd.Parameters.AddWithValue("@p7", FARMADO)
                cmd.Parameters.AddWithValue("@p8", FDESPACHO)
                cmd.Parameters.AddWithValue("@p9", "A")
                cmd.Parameters.AddWithValue("@p10", planid)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Return 0
            Response.Write("<script>console.log('MacroInsert_Header: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Function getWhsType(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT u_type from owhs with(nolock) where whscode = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getCardcode: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Private Sub btnModalColoresMotos_Click(sender As Object, e As EventArgs) Handles btnModalColoresMotos.Click
        Try
            For Each row As GridViewRow In gridColoresMoto.Rows
                Dim qtyTextBox As TextBox = TryCast(row.FindControl("qty"), TextBox)
                If qtyTextBox IsNot Nothing AndAlso Not String.IsNullOrEmpty(qtyTextBox.Text) Then
                    Dim cantidad As Integer
                    If Integer.TryParse(qtyTextBox.Text, cantidad) AndAlso cantidad > 0 Then
                        For i As Integer = 1 To cantidad
                            'tabla macroinsert
                            InsertSugeridoSucursales(
                            lblRuta.Text,
                            lblAlmOrigen.Text,
                            lblAlmDestino.Text,
                            lblcardcode.Text,
                            row.Cells(0).Text,
                            row.Cells(2).Text,
                            row.Cells(1).Text,
                            row.Cells(3).Text * 1,
                            1,
                            1,
                            1,
                            "TRASLADO",
                            Date.Now,
                            Session("User"),
                            lblCB.Text,
                            lblFISICO.Text,
                            lblFALTANTE.Text,
                            lblVTAA.Text)

                            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                                'tabla de control lineas de despacho
                                InsertSugeridoSucursalesDespacho(
                                lblRuta.Text,
                                lblAlmOrigen.Text,
                                lblAlmDestino.Text,
                                lblcardcode.Text,
                                row.Cells(0).Text,
                                row.Cells(2).Text,
                                row.Cells(1).Text,
                                row.Cells(3).Text * 1,
                                1,
                                1,
                                1,
                                "TRASLADO",
                                Date.Now,
                                Session("User"),
                                lblCB.Text,
                                lblFISICO.Text,
                                lblFALTANTE.Text,
                                lblVTAA.Text,
                                lblModeloCode.Text)
                            End If
                        Next
                    End If
                End If
            Next

            BindGrid(lblAlmActual.Text)
            BindGridDetallePlan(Request.QueryString("planid"))
            BindgridSugerido(Request.QueryString("planid"), Session("User"))
            getResumenPedidosCI()
        Catch ex As Exception
            Response.Write("<script>console.log('btnModalColoresMotos_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function InsertSugeridoSucursales(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String, ARTICULO As String,
                                                MODELO As String, DESCRIPCION As String, ESPACIOS As String,
                                                CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                                OBSERVACIONES As String, FECHA As Date, USUARIO As String,
                                                CB As String, FISICO As String, FALTANTE As String, VTAA As String) As Integer
        Try

            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim planId As Integer = 0

            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planId = Request.QueryString("planId")
            End If

            Dim SQL_QRY As String = " BEGIN " &
                    " INSERT INTO [dbo].[MACROINSERT] " &
                    " ([RUTA], [ALMORIGEN], [ALMDESTINO], [CARDCODE], [ARTICULO], [MODELO], [DESCRIPCION], [ESPACIOS],  " &
                    " [CANTIDAD], [QTYLOGISTICA], [QTYSUCURSAL], [OBSERVACIONES], [ESTADO], [TIPO], [FECHA],  " &
                    " [USUARIO], [CB], [FISICO], [FALTANTE], [VTAA],[PLANID])  " &
                    " VALUES  " &
                    " (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21); " &
                    " SELECT SCOPE_IDENTITY() AS InsertedID; " &
                    " END;"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(SQL_QRY, con)
                cmd.Parameters.AddWithValue("@p1", RUTA)
                cmd.Parameters.AddWithValue("@p2", ALMORIGEN)
                cmd.Parameters.AddWithValue("@p3", ALMDESTINO)
                cmd.Parameters.AddWithValue("@p4", CARDCODE)
                cmd.Parameters.AddWithValue("@p5", ARTICULO)
                cmd.Parameters.AddWithValue("@p6", MODELO)
                cmd.Parameters.AddWithValue("@p7", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p8", ESPACIOS)
                cmd.Parameters.AddWithValue("@p9", CANTIDAD)
                cmd.Parameters.AddWithValue("@p10", QTYLOGISTICA)
                cmd.Parameters.AddWithValue("@p11", QTYSUCURSAL)
                cmd.Parameters.AddWithValue("@p12", OBSERVACIONES)
                cmd.Parameters.AddWithValue("@p13", "T")
                cmd.Parameters.AddWithValue("@p14", getWhsCanal(ALMDESTINO))
                cmd.Parameters.AddWithValue("@p15", FECHA)
                cmd.Parameters.AddWithValue("@p16", USUARIO)
                cmd.Parameters.AddWithValue("@p17", CB)
                cmd.Parameters.AddWithValue("@p18", FISICO)
                cmd.Parameters.AddWithValue("@p19", FALTANTE)
                cmd.Parameters.AddWithValue("@p20", VTAA)
                cmd.Parameters.AddWithValue("@p21", planId)

                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Function InsertSugeridoSucursalesDespacho(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String, ARTICULO As String,
                                                MODELO As String, DESCRIPCION As String, ESPACIOS As String,
                                                CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                                OBSERVACIONES As String, FECHA As Date, USUARIO As String,
                                                CB As String, FISICO As String, FALTANTE As String, VTAA As String, CODMODELO As String) As Integer
        Try

            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim planId As Integer = 0
            Dim headerId As Integer = 0

            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planId = Request.QueryString("planId")
                headerId = Request.QueryString("headerId")
            End If

            Dim SQL_QRY As String = " BEGIN " &
                    " INSERT INTO [dbo].[DESPACHOS_DETALLE_MOTOS] " &
                    " ([RUTA], [ALMORIGEN], [ALMDESTINO], [CARDCODE], [ARTICULO], [MODELO], [DESCRIPCION], [ESPACIOS],  " &
                    " [CANTIDAD], [QTYLOGISTICA], [QTYSUCURSAL], [OBSERVACIONES], [ESTADO], [TIPO], [FECHA],  " &
                    " [USUARIO], [CB], [FISICO], [FALTANTE], [VTAA],[PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO])  " &
                    " VALUES  " &
                    " (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24); " &
                    " SELECT SCOPE_IDENTITY() AS InsertedID; " &
                    " END;"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(SQL_QRY, con)
                cmd.Parameters.AddWithValue("@p1", RUTA)
                cmd.Parameters.AddWithValue("@p2", ALMORIGEN)
                cmd.Parameters.AddWithValue("@p3", ALMDESTINO)
                cmd.Parameters.AddWithValue("@p4", CARDCODE)
                cmd.Parameters.AddWithValue("@p5", ARTICULO)
                cmd.Parameters.AddWithValue("@p6", MODELO)
                cmd.Parameters.AddWithValue("@p7", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p8", ESPACIOS)
                cmd.Parameters.AddWithValue("@p9", CANTIDAD)
                cmd.Parameters.AddWithValue("@p10", QTYLOGISTICA)
                cmd.Parameters.AddWithValue("@p11", QTYSUCURSAL)
                cmd.Parameters.AddWithValue("@p12", OBSERVACIONES)
                cmd.Parameters.AddWithValue("@p13", "T")
                cmd.Parameters.AddWithValue("@p14", getWhsCanal(ALMDESTINO))
                cmd.Parameters.AddWithValue("@p15", FECHA)
                cmd.Parameters.AddWithValue("@p16", USUARIO)
                cmd.Parameters.AddWithValue("@p17", CB)
                cmd.Parameters.AddWithValue("@p18", FISICO)
                cmd.Parameters.AddWithValue("@p19", FALTANTE)
                cmd.Parameters.AddWithValue("@p20", VTAA)
                cmd.Parameters.AddWithValue("@p21", planId)
                cmd.Parameters.AddWithValue("@p22", headerId)
                cmd.Parameters.AddWithValue("@p23", "ABIERTO")
                cmd.Parameters.AddWithValue("@p24", CODMODELO)

                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Function getCustomerCode(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT U_CARDCODE FROM MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getCustomerCode: " & t & "');</script>")
        End Using
    End Function
    Public Function getWhsCanal(whscode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "select case u_type when 'PRO' THEN 'CD' ELSE 'CI' END [CANAL] from movesa..owhs where whscode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getWhsCanal: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Private Sub BindgridSugerido(idPlan As String, usuario As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SqlQry As String = "SELECT ID,ALMDESTINO,ARTICULO,MODELO,DESCRIPCION,ESPACIOS,CANTIDAD,QTYLOGISTICA, " &
                                            " OBSERVACIONES FROM ArmadoMotos.dbo.MACROINSERT " &
                                            " WHERE [ESTADO]='T' AND PLANID = @planId and usuario = @usuario"
                Using cmd As New SqlCommand(SqlQry)
                    cmd.Parameters.AddWithValue("@planId", idPlan)
                    cmd.Parameters.AddWithValue("@usuario", usuario)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPedidoTemp.DataSource = dt
                            gridPedidoTemp.DataBind()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridPedidoTemp.UseAccessibleHeader = True
            gridPedidoTemp.HeaderRow.TableSection = TableRowSection.TableHeader

            Dim unidades As Integer = 0
            Dim espacios As Integer = 0
            For Each row As GridViewRow In gridPedidoTemp.Rows
                espacios = espacios + CInt(row.Cells(4).Text)
                unidades = unidades + CInt(row.Cells(5).Text)
            Next

            lblEspacios.Text = "Espacios Asignados: " & espacios
            lblUnidades.Text = "Unidades Asignadas: " & unidades
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub btnCancelColoresMotos_Click(sender As Object, e As EventArgs) Handles btnCancelColoresMotos.Click
        Try
            'BindGrid(lblAlmActual.Text)
            BindGridDetallePlan(Request.QueryString("planid"))
            BindgridSugerido(Request.QueryString("planid"), Session("User"))
        Catch ex As Exception

        End Try
    End Sub
    Public Sub EliminarLinea(idlinea As Integer, itemcode As String, planid As String, almacen As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "DELETE FROM [MACROINSERT] where [ID] = @p1;" &
                " DELETE FROM [DESPACHOS_DETALLE_MOTOS] WHERE PLANID = @planid And [ARTICULO] = @itemcode And [ALMDESTINO] = @almacen  " &
                " And [ESTADO] = @estado  " &
                " and [USUARIO] = @usuario"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                cmd.Parameters.AddWithValue("@itemcode", itemcode)
                cmd.Parameters.AddWithValue("@planid", planid)
                cmd.Parameters.AddWithValue("@almacen", almacen)
                cmd.Parameters.AddWithValue("@estado", "T")
                cmd.Parameters.AddWithValue("@usuario", Session("User"))
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                Dim script As String = "<script>iziToast.warning({title: 'OK!', message: 'Moto Eliminada Con Exito!!!',position: 'topRight',timeout: 10000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            End Using
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

                Dim script As String = "<script>iziToast.warning({title: 'OK!', message: 'Moto Eliminada Con Exito!!!',position: 'topRight',timeout: 10000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPedidoTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Try
            Response.Redirect("TrasladosDDashboard.aspx")
        Catch ex As Exception
            Response.Write("<script>console.log('btnRegresar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub

    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Try
            Response.Redirect("TrasladosDDashboardDetalleConfirmacion.aspx?planid=" & Request.QueryString("planid"))
        Catch ex As Exception
            Response.Write("<script>console.log('btnContinuar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub actualizarDespachoDetalleMotos(headerid As String, planid As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String = "update [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] set [HEADERID] = @headerId, [ESTADO] = 'T', [DESPACHOID] = @headerId where PLANID = @planId"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@headerId", headerid)
                cmd.Parameters.AddWithValue("@planId", planid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Public Sub actualizarMacroInsert(headerid As String, fdespacho As DateTime, planid As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String = "update [ArmadoMotos].[dbo].[MACROINSERT] set [HEADERID] = @headerId, " &
            " [ESTADO] = 'P', [FDESPACHO] = @fdespacho where PLANID = @planId AND USUARIO = @usuario"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@headerId", headerid)
                cmd.Parameters.AddWithValue("@fdespacho", fdespacho)
                cmd.Parameters.AddWithValue("@planId", planid)
                cmd.Parameters.AddWithValue("@usuario", Session("User"))
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('btnRegresar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub getResumenPedidosCI()
        Try
            Dim sCon As String = sCon2
            Dim sel As String = "EXEC MOVESA..SP_PORTAL_DISTRIBUIDORES @FUNCTION = 'PEDIDOS DISPONIBLE DESPACHO'"
            Dim totalFilas As Integer = 0
            Dim sumaEspacios As Integer = 0

            ' Lista temporal para guardar almacenes y espacios
            Dim listaPedidos As New List(Of Tuple(Of String, Integer))

            ' Ejecutar query y llenar lista + totales generales
            Using con As New SqlConnection(sCon)
                Using cmd As New SqlCommand(sel, con)
                    con.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            totalFilas += 1

                            Dim alm As String = ""
                            Dim esp As Integer = 0

                            If Not IsDBNull(reader("almacen")) Then
                                alm = reader("almacen").ToString().Trim()
                            End If

                            If Not IsDBNull(reader("espacios")) Then
                                esp = Convert.ToInt32(reader("espacios"))
                                sumaEspacios += esp
                            End If

                            listaPedidos.Add(New Tuple(Of String, Integer)(alm, esp))
                        End While
                    End Using
                End Using
            End Using

            ' Mostrar resultados generales
            lblUnidadesCI.Text = "Gen Unidades: " & totalFilas.ToString()
            lblEspaciosCI.Text = "Gen Espacios: " & sumaEspacios.ToString()
        Catch ex As Exception
            Response.Write("<script>console.log('getResumenPedidosCI: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub btnAgregarAlmacenManual_Click(sender As Object, e As EventArgs) Handles btnAgregarAlmacenManual.Click
        Try
            AGREGAR_ALM_MANUAL(Request.QueryString("planid"), txtCodigoAlmacen.Text)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BindgridPortalPedidos()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)

                Dim SqlQry As String = "EXEC MOVESA..SP_PORTAL_DISTRIBUIDORES 'PEDIDOS DISPONIBLE DESPACHO'"
                Using cmd As New SqlCommand(SqlQry)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridMotosPortalPedidos.DataSource = dt
                            gridMotosPortalPedidos.DataBind()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridMotosPortalPedidos.UseAccessibleHeader = True
            gridMotosPortalPedidos.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridPortalPedidos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

End Class

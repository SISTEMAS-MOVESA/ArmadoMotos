Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDashboardPlanner
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public almacen As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGrid()
                    BindGridgridPlanificacionTemp()
                    cargarPlanificacionesAbiertas()

                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
                    Else
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write("TrasladosDashboard_Load " & ex.Message)
        End Try
    End Sub
    Public Sub cargarPlanificacionesAbiertas()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "SELECT [ID] FROM [ArmadoMotos].[dbo].[PLANIFICACIONES] WHERE ESTADO='ABIERTO'"

                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using
            drpPlanificacionesAbiertas.Dispose()
            drpPlanificacionesAbiertas.DataTextField = "ID"
            drpPlanificacionesAbiertas.DataValueField = "ID"
            drpPlanificacionesAbiertas.DataSource = dt
            drpPlanificacionesAbiertas.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('cargarPlanificacionesAbiertas: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)

                ' Leer el contenido del archivo .sql
                Dim SQL_string As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/planificacion_dashboard_load.sql"))
                Using cmd As New SqlCommand(SQL_string, con)

                    Using sda As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        gridIndiceD.DataSource = dt
                        gridIndiceD.DataBind()
                    End Using
                End Using
            End Using
            gridIndiceD.UseAccessibleHeader = True
            gridIndiceD.HeaderRow.TableSection = TableRowSection.TableHeader

            Dim tempSumatoria As Integer = 0
            For Each row As GridViewRow In gridIndiceD.Rows
                tempSumatoria = CInt(row.Cells(12).Text) - CInt(row.Cells(13).Text) - CInt(row.Cells(20).Text)

                ' Corrige las celdas de porcentaje: si es negativo, pon 0.00%
                For i As Integer = 8 To 10
                    Dim valor As Decimal
                    If Decimal.TryParse(row.Cells(i).Text.Replace("%", "").Trim, valor) Then
                        If valor < 0 Then
                            row.Cells(i).Text = "0.00 %"
                        Else
                            row.Cells(i).Text = FormatNumber(valor, 2) & " %"
                        End If
                    End If
                Next
            Next


        Catch ex As Exception
            Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub
    Protected Sub gridPlanificacionTemp_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim valorPlanificacion As String = e.Row.Cells(5).Text.Trim()
                For Each rowIndiceD As GridViewRow In gridIndiceD.Rows
                    Dim valorIndice As String = rowIndiceD.Cells(6).Text.Trim()
                    Dim cb As CheckBox = CType(rowIndiceD.FindControl("cbDocument"), CheckBox)

                    If valorPlanificacion.Trim = valorIndice.Trim Then
                        cb.Visible = False
                        Dim checkLabel As New Label()
                        checkLabel.Text = "<i class='fa-solid fa-check fa-2x text-success text-center'></i>"
                        rowIndiceD.Cells(23).Controls.Add(checkLabel)
                    End If
                Next
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGridgridPlanificacionTemp()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = " SELECT * FROM [ArmadoMotos].[dbo].[PLANIFICACIONES_DETALLE] where headerid=0 "

                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPlanificacionTemp.DataSource = dt
                            gridPlanificacionTemp.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridPlanificacionTemp.UseAccessibleHeader = True
            gridPlanificacionTemp.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub gridIndiceD_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.Add("data-merge", "true")
            e.Row.Cells(1).Attributes.Add("data-merge", "true")
            e.Row.Cells(2).Attributes.Add("data-merge", "true")
            e.Row.Cells(3).Attributes.Add("data-merge", "true")
        End If
    End Sub
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

    Private Sub MergeRows(grid As GridView, column As String)
        Dim columnIndex As Integer = GetColumnIndexByName(grid, column)
        If columnIndex = -1 Then Exit Sub

        Dim previousCell As TableCell = Nothing
        Dim rowSpanCount As Integer = 1

        For rowIndex As Integer = 0 To grid.Rows.Count - 1
            Dim currentRow As GridViewRow = grid.Rows(rowIndex)
            Dim currentCell As TableCell = currentRow.Cells(columnIndex)

            If previousCell IsNot Nothing AndAlso currentCell.Text = previousCell.Text Then
                rowSpanCount += 1
                previousCell.RowSpan = rowSpanCount
                currentCell.Visible = False
            Else
                previousCell = currentCell
                rowSpanCount = 1
            End If
        Next
    End Sub
    Private Function GetColumnIndexByName(grid As GridView, columnName As String) As Integer
        For i As Integer = 0 To grid.Columns.Count - 1
            If grid.Columns(i).HeaderText = columnName Then
                Return i
            End If
        Next
        Return -1
    End Function
    Private Sub gridPlanificacionTemp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPlanificacionTemp.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridPlanificacionTemp.Rows(index)
                EliminarLinea(gridPlanificacionTemp.Rows(index).Cells(1).Text)

                BindGrid()
                BindGridgridPlanificacionTemp()

                Dim script As String = "<script>Swal.fire({title: 'Éxito!', text: 'Linea Eliminada Exitosamente!', icon: 'success', confirmButtonText: 'OK'});</script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "swal", script)

            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPlanificacionTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub EliminarLinea(idlinea As Integer)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "DELETE FROM [PLANIFICACIONES_DETALLE] where [ID] = @p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("EliminarLinea " & ex.Message)
        End Try
    End Sub

    Private Sub btnCrearPlan_Click(sender As Object, e As EventArgs) Handles btnCrearPlan.Click
        Try
            Dim lineas As Integer = 0
            For Each row As GridViewRow In gridIndiceD.Rows
                Dim cb As CheckBox = CType(row.FindControl("cbDocument"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked Then
                    lineas += 1
                    Dim connectionString As String = sCon2
                    Using con As New SqlConnection(connectionString)
                        con.Open()
                        Dim ruta As String = row.Cells(0).Text
                        Dim canal As String = row.Cells(1).Text
                        Dim cindice As String = row.Cells(2).Text
                        Dim leyenda As String = row.Cells(3).Text
                        Dim ranking As String = row.Cells(4).Text
                        Dim crank As String = row.Cells(5).Text
                        Dim codigo As String = row.Cells(6).Text
                        almacen = row.Cells(7).Text
                        Dim indice As String = row.Cells(10).Text.Replace(" %", "")
                        Dim indiceProyectado As String = row.Cells(11).Text.Replace("%", "")
                        Dim cuadro As String = row.Cells(12).Text
                        Dim comprometido As String = row.Cells(14).Text
                        Dim solicitado As String = row.Cells(15).Text
                        Dim transito As String = row.Cells(16).Text
                        Dim fisico As String = row.Cells(20).Text
                        Dim faltante As String = row.Cells(21).Text
                        Dim unds As String = row.Cells(19).Text
                        Dim headerID As Integer = 0


                        Response.Write("<script>console.log('RUTA: " & ruta & "');</script>")
                        Response.Write("<script>console.log('CANAL: " & canal & "');</script>")
                        Response.Write("<script>console.log('CINDICE: " & cindice & "');</script>")
                        Response.Write("<script>console.log('LEYENDA: " & leyenda & "');</script>")
                        Response.Write("<script>console.log('RANKING: " & ranking & "');</script>")
                        Response.Write("<script>console.log('CRANK: " & crank & "');</script>")
                        Response.Write("<script>console.log('CODIGO: " & codigo & "');</script>")
                        Response.Write("<script>console.log('ALMACEN: " & almacen & "');</script>")
                        Response.Write("<script>console.log('INDICE: " & indice & "');</script>")
                        Response.Write("<script>console.log('INDICE PROYECTADO: " & indiceProyectado & "');</script>")
                        Response.Write("<script>console.log('CUADRO: " & cuadro & "');</script>")
                        Response.Write("<script>console.log('COMPROMETIDO: " & comprometido & "');</script>")
                        Response.Write("<script>console.log('SOLICITADO: " & solicitado & "');</script>")
                        Response.Write("<script>console.log('TRANSITO: " & transito & "');</script>")
                        Response.Write("<script>console.log('FISICO: " & fisico & "');</script>")
                        Response.Write("<script>console.log('FALTANTE: " & faltante & "');</script>")
                        Response.Write("<script>console.log('UNDS: " & unds & "');</script>")
                        Response.Write("<script>console.log('HEADERID: " & headerID & "');</script>")




                        Dim query As String = "INSERT INTO [dbo].[PLANIFICACIONES_DETALLE] " &
                                                  "([CANAL], [RANKING], [RUTA], [CODIGO], [ALMACEN], [CUADRO], [COMP], [SOL], [TRANSITO], " &
                                                  " [FISICO], [FALTANTE], [UNDS], [INDICE], [CINDICE], [CRANKING], [LEYENDA], [HEADERID], " &
                                                  " [ESTADOHEADER], [ESTADOLINEA],[INDICEP]) " &
                                                  " VALUES (@CANAL, @RANKING, @RUTA, @CODIGO, @ALMACEN, @CUADRO, @COMP, @SOL, @TRANSITO, @FISICO, " &
                                                  " @FALTANTE, @UNDS, @INDICE, @CINDICE, @CRANKING, @LEYENDA, @HEADERID, @ESTADOHEADER, @ESTADOLINEA, " &
                                                  " @INDICEP)"

                        Using cmd As New SqlCommand(query, con)
                            cmd.Parameters.AddWithValue("@CANAL", canal)
                            cmd.Parameters.AddWithValue("@RANKING", ranking)
                            cmd.Parameters.AddWithValue("@RUTA", ruta)
                            cmd.Parameters.AddWithValue("@CODIGO", codigo)
                            cmd.Parameters.AddWithValue("@ALMACEN", almacen)
                            cmd.Parameters.AddWithValue("@CUADRO", cuadro)
                            cmd.Parameters.AddWithValue("@COMP", comprometido)
                            cmd.Parameters.AddWithValue("@SOL", solicitado)
                            cmd.Parameters.AddWithValue("@TRANSITO", transito)
                            cmd.Parameters.AddWithValue("@FISICO", fisico)
                            cmd.Parameters.AddWithValue("@FALTANTE", faltante)
                            cmd.Parameters.AddWithValue("@UNDS", unds)
                            cmd.Parameters.AddWithValue("@INDICE", indice)
                            cmd.Parameters.AddWithValue("@CINDICE", cindice)
                            cmd.Parameters.AddWithValue("@CRANKING", crank)
                            cmd.Parameters.AddWithValue("@LEYENDA", leyenda)
                            cmd.Parameters.AddWithValue("@HEADERID", headerID)
                            cmd.Parameters.AddWithValue("@ESTADOHEADER", "TEMPORAL")
                            cmd.Parameters.AddWithValue("@ESTADOLINEA", "ABIERTO")
                            cmd.Parameters.AddWithValue("@INDICEP", indiceProyectado)

                            cmd.ExecuteNonQuery()
                        End Using

                        con.Close()
                    End Using
                End If
            Next
            BindGrid()
            BindGridgridPlanificacionTemp()

            Dim script As String = "<script>Swal.fire({title: 'Éxito!', text: '" & ReplaceCharsForFileName(lineas, " ") & " Lineas Agregadas Exitosamente!', icon: 'success', confirmButtonText: 'OK'});</script>"

            ClientScript.RegisterStartupScript(Me.GetType(), "swal", script)
        Catch ex As Exception
            Response.Write("<script>console.log('btnCrearPlan_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub btnEliminarLineas_Click(sender As Object, e As EventArgs) Handles btnEliminarLineas.Click
        If Request("__EVENTARGUMENT") = "EliminarConfirmado" Then
            ' Ejecutar la eliminación después de la confirmación
            EliminarLineasConfirmadas()
        Else
            ' Mostrar el Swal2 de confirmación antes de eliminar
            Dim scriptConfirm As String = "<script>" &
                "Swal.fire({" &
                "title: '¿Está seguro?', " &
                "text: 'Esta acción eliminará todas las líneas de forma permanente.', " &
                "icon: 'warning', " &
                "showCancelButton: true, " &
                "confirmButtonColor: '#3085d6', " &
                "cancelButtonColor: '#d33', " &
                "confirmButtonText: 'Sí, eliminar', " &
                "cancelButtonText: 'Cancelar'" &
                "}).then((result) => {" &
                "if (result.isConfirmed) {" &
                "__doPostBack('" & btnEliminarLineas.UniqueID & "', 'EliminarConfirmado');" &
                "}" &
                "});" &
                "</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "swalConfirm", scriptConfirm)
        End If
    End Sub

    Private Sub EliminarLineasConfirmadas()
        Try
            Dim lineas As Integer = 0
            For Each row As GridViewRow In gridPlanificacionTemp.Rows
                Dim connectionString As String = sCon2
                Using con As New SqlConnection(connectionString)
                    con.Open()
                    lineas += 1 ' Incrementar el contador de líneas
                    Dim lineId As String = row.Cells(1).Text
                    Dim query As String = "DELETE FROM [dbo].[PLANIFICACIONES_DETALLE] WHERE id = @lineId"
                    Using cmd As New SqlCommand(query, con)
                        cmd.Parameters.AddWithValue("@lineId", lineId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
            Next

            ' Refrescar los grids
            BindGrid()
            BindGridgridPlanificacionTemp()

            ' Mostrar mensaje de éxito
            Dim scriptSuccess As String = "<script>" &
                "Swal.fire({" &
                "title: 'Éxito!'," &
                "text: '" & lineas & " Líneas eliminadas exitosamente!'," &
                "icon: 'success'," &
                "confirmButtonText: 'OK'" &
                "});" &
                "</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "swalSuccess", scriptSuccess)
        Catch ex As Exception
            ' Mostrar error en consola del navegador
            Dim scriptError As String = "<script>console.log('" &
                Replace(Replace(Replace(ex.Message, ",", ""), "'", ""), ";", "") &
                "');</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "swalError", scriptError)
        End Try
    End Sub

    ' ✅ Sobrescribir el método Render para registrar el argumento de postback
    Protected Overrides Sub Render(writer As HtmlTextWriter)
        ClientScript.RegisterForEventValidation(btnEliminarLineas.UniqueID, "EliminarConfirmado")
        MyBase.Render(writer)
    End Sub

    Private Sub btnAgregarPlan_Click(sender As Object, e As EventArgs) Handles btnAgregarPlan.Click
        Try
            Dim lineas As Integer = 0
            For Each row As GridViewRow In gridPlanificacionTemp.Rows
                lineas += 1
                Dim connectionString As String = sCon2
                Using con As New SqlConnection(connectionString)
                    con.Open()
                    Dim query As String = "UPDATE [dbo].[PLANIFICACIONES_DETALLE] " &
                                                  "SET [HEADERID] = @p1, [ESTADOHEADER] = @p2 WHERE ID = @p3"

                    Using cmd As New SqlCommand(query, con)
                        cmd.Parameters.AddWithValue("@p1", drpPlanificacionesAbiertas.SelectedValue.ToString())
                        cmd.Parameters.AddWithValue("@p2", "PLANIFICACION")
                        cmd.Parameters.AddWithValue("@p3", row.Cells(1).Text.ToString())
                        cmd.ExecuteNonQuery()
                    End Using

                    con.Close()
                End Using
            Next
            BindGrid()
            BindGridgridPlanificacionTemp()

            Dim script As String = "<script>Swal.fire({title: 'Éxito!', text: '" & ReplaceCharsForFileName(lineas, " ") & " Lineas Agregadas Exitosamente!', icon: 'success', confirmButtonText: 'OK'});</script>"

            ClientScript.RegisterStartupScript(Me.GetType(), "swal", script)
        Catch ex As Exception
            Response.Write("<script>console.log('btnAgregarPlan_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnCalendario_Click(sender As Object, e As EventArgs) Handles btnCalendario.Click
        Try
            Response.Redirect("TrasladosDespachosCalendario.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub btnCuadroBasico_Click(sender As Object, e As EventArgs) Handles btnCuadroBasico.Click
        Try
            Response.Redirect("TrasladosCuadroBasico.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class

Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDDashboard
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Private Sub TrasladosDDashboard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGrid()
                    BindGridDespachosAbiertos()
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = "SELECT [ID],convert(char,FECHACREACION,103)[FECHACREACION],convert(char,FECHAINICIO,103)[FECHAINICIO],convert(char,FECHAVENCIMIENTO,103)[FECHAVENCIMIENTO],[FALTANTE],[ESTADO], " &
                             " [USUARIO],[FECHACREACION] FROM [ArmadoMotos].[dbo].[PLANIFICACIONES] WHERE ESTADO='ABIERTO'"

                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridDespachos.DataSource = dt
                            gridDespachos.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridDespachos.UseAccessibleHeader = True
            gridDespachos.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGridDespachosAbiertos()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = "select DISTINCT T0.ID [Id Planificacion],T0.PLANID,CONVERT(CHAR,FECHACREACION,103)[Fecha Creacion], " &
                " CONVERT(CHAR,FECHAINICIO,103)[Fecha Inicio], CONVERT(CHAR,FECHAVENCE,103)[Fecha Vencimiento],T0.ESTADO [Estado], " &
                " (select count(codigo) from DESPACHOS_DETALLE where PLANID=T0.PLANID And CODIGOESTADO='DESPACHO ABIERTO')[Suma Almacenes], " &
                " (select sum(faltante) from DESPACHOS_DETALLE where PLANID=T0.PLANID And CODIGOESTADO='DESPACHO ABIERTO')[Suma Fantalte] " &
                " from [DEPACHOS_HEADER]  T0 WITH(NOLOCK) INNER JOIN DESPACHOS_DETALLE_MOTOS T1 WITH(nOLOCK) ON T0.ID = T1.HEADERID " &
                " WHERE T0.ESTADO='ABIERTO' AND T1.CODIGOESTADO='DESPACHO ABIERTO'"

                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridDespachosAbiertos.DataSource = dt
                            gridDespachosAbiertos.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridDespachosAbiertos.UseAccessibleHeader = True
            gridDespachosAbiertos.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDespachosAbiertos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGridDetallePlan(idPlan As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = "SELECT * " &
                    " ,isnull((SELECT sum([CANTIDAD])  FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS]  " &
                    " WHERE ALMDESTINO = [PLANIFICACIONES_DETALLE].Codigo And CODIGOESTADO <> 'CERRADO' ),0)[Despacho] " &
                    " ,isnull(convert(decimal(19,2),CONVERT(DECIMAL(19,4), ([Faltante]-isnull((SELECT sum([CANTIDAD])   " &
                    " FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] WHERE ALMDESTINO = [PLANIFICACIONES_DETALLE].codigo  " &
                    " AND CODIGOESTADO <> 'CERRADO' ),0))) / NULLIF(CONVERT(DECIMAL(19,4), [Cuadro]), 0) * 100),0) AS [Indice_Proyectado] " &
                    " FROM [ArmadoMotos].[dbo].[PLANIFICACIONES_DETALLE] " &
                    " where [HEADERID] = @p1"

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

            Dim tempSumatoria As Integer = 0
            For Each row As GridViewRow In gridAlmacenesDespachos.Rows
                tempSumatoria = CInt(row.Cells(10).Text) + CInt(row.Cells(11).Text) - CInt(row.Cells(12).Text - CInt(row.Cells(13).Text) - CInt(row.Cells(13).Text) - CInt(row.Cells(14).Text) - CInt(row.Cells(15).Text))
                row.Cells(16).Text = tempSumatoria
            Next

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDetallePlan: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub gridIndiceD_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Agregar atributo data-merge a las celdas que deben fusionarse
            e.Row.Cells(0).Attributes.Add("data-merge", "true") ' Columna 1: Ruta
            e.Row.Cells(1).Attributes.Add("data-merge", "true") ' Columna 2: Canal
            e.Row.Cells(2).Attributes.Add("data-merge", "true") ' Columna 3: Range Ranking
            e.Row.Cells(3).Attributes.Add("data-merge", "true") ' Columna 4: Range Desabasto

            ' Obtener valores de CODIGO (columna 6) y PLANID (columna 16)
            Dim codigo As String = e.Row.Cells(6).Text
            Dim planId As String = e.Row.Cells(16).Text

            ' Buscar en la base de datos si existe el registro
            If ExisteDespacho(codigo, planId) Then
                ' Ocultar CheckBox
                Dim cb As CheckBox = TryCast(e.Row.FindControl("cbDocument"), CheckBox)
                If cb IsNot Nothing Then
                    cb.Visible = False
                End If

                ' Agregar el ícono de check en la celda 16
                Dim checkLabel As New Label()
                checkLabel.Text = "<i class='fa-solid fa-check fa-2x text-success text-center'></i>"
                e.Row.Cells(17).Controls.Add(checkLabel)
            End If
        End If
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
        sName = Replace(sName, Chr(34), sChr) ' Comillas dobles
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


    Private Sub gridDespachos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDespachos.RowCommand
        Try
            If e.CommandName = "Ver" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridDespachos.Rows(index)
                EliminarLinea(Session("Name"))
                Response.Redirect("TrasladosDDashboardDetalle.aspx?planid=" & gridDespachos.Rows(index).Cells(0).Text)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridDespachos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub btnRedirect_Command(sender As Object, e As CommandEventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim row As GridViewRow = CType(btn.NamingContainer, GridViewRow)
            Dim connectionString As String = sCon2


            BindGrid()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub EliminarLinea(usuario As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "DELETE FROM [ArmadoMotos].[dbo].[MACROINSERT] where ESTADO='T' AND USUARIO = @usuario;" &
                " DELETE FROM [ArmadoMotos].[dbo].[CARGA_CAMION_DETALLE] WHERE ESTADO='T' AND USUARIO = @usuario;"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@usuario", usuario)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
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
    Public Function InsertarDespachosSeleccionados(planid As String, numerodespacho As String) As Integer
        Dim registrosInsertados As Integer = 0

        Try
            Dim sCon As String = sCon2
            Using con As New SqlConnection(sCon)
                con.Open()

                For Each row As GridViewRow In gridAlmacenesDespachos.Rows
                    Dim cbSeleccionado As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)

                    If cbSeleccionado IsNot Nothing AndAlso cbSeleccionado.Checked Then
                        Using cmd As New SqlCommand("INSERT INTO [dbo].[DESPACHOS_DETALLE] " &
                        " ([CANAL], [RANKING], [RUTA], [CODIGO], [ALMACEN], [CUADRO], [COMP], [SOL], " &
                        " [TRANSITO], [FISICO], [FALTANTE], [UNDS], [INDICE], [CINDICE], [CRANKING],  " &
                        " [LEYENDA], [HEADERID], [ESTADOHEADER], [ESTADOLINEA], [DESPACHOID], [PLANID],  " &
                        " [DEPACHOFECHAINICIO], [DESPACHOFECHAVENCE], [USUARIODESPACHO],  " &
                        " [FECHAACTUALIZACIONDESPACHO], [DESPACHOHEADER])  " &
                        " VALUES (@CANAL, @RANKING, @RUTA, @CODIGO, @ALMACEN, @CUADRO, @COMP, @SOL, " &
                        " @TRANSITO, @FISICO, @FALTANTE, @UNDS, @INDICE, @CINDICE, @CRANKING, @LEYENDA,  " &
                        " @HEADERID, @ESTADOHEADER, @ESTADOLINEA, @DESPACHOID, @PLANID,  " &
                        " @DEPACHOFECHAINICIO, @DESPACHOFECHAVENCE, @USUARIODESPACHO,  " &
                        " @FECHAACTUALIZACIONDESPACHO, @DESPACHOHEADER);", con)

                            cmd.Parameters.AddWithValue("@CANAL", row.Cells(1).Text)
                            cmd.Parameters.AddWithValue("@RANKING", row.Cells(2).Text)
                            cmd.Parameters.AddWithValue("@RUTA", row.Cells(0).Text)
                            cmd.Parameters.AddWithValue("@CODIGO", row.Cells(6).Text)
                            cmd.Parameters.AddWithValue("@ALMACEN", row.Cells(7).Text)
                            cmd.Parameters.AddWithValue("@CUADRO", row.Cells(9).Text)
                            cmd.Parameters.AddWithValue("@COMP", row.Cells(10).Text)
                            cmd.Parameters.AddWithValue("@SOL", row.Cells(11).Text)
                            cmd.Parameters.AddWithValue("@TRANSITO", row.Cells(12).Text)
                            cmd.Parameters.AddWithValue("@FISICO", row.Cells(13).Text)
                            cmd.Parameters.AddWithValue("@FALTANTE", row.Cells(14).Text)
                            cmd.Parameters.AddWithValue("@UNDS", row.Cells(15).Text)
                            cmd.Parameters.AddWithValue("@INDICE", row.Cells(8).Text)
                            cmd.Parameters.AddWithValue("@CINDICE", row.Cells(3).Text)
                            cmd.Parameters.AddWithValue("@CRANKING", row.Cells(5).Text)
                            cmd.Parameters.AddWithValue("@LEYENDA", row.Cells(4).Text)
                            cmd.Parameters.AddWithValue("@HEADERID", row.Cells(16).Text)
                            cmd.Parameters.AddWithValue("@ESTADOHEADER", "Pendiente")
                            cmd.Parameters.AddWithValue("@ESTADOLINEA", "Pendiente")
                            cmd.Parameters.AddWithValue("@DESPACHOID", row.Cells(15).Text) ' (Modificar según lógica)
                            cmd.Parameters.AddWithValue("@PLANID", planid) ' (Modificar según lógica)
                            cmd.Parameters.AddWithValue("@DEPACHOFECHAINICIO", DateTime.Now.Date)
                            cmd.Parameters.AddWithValue("@DESPACHOFECHAVENCE", DateTime.Now.AddDays(5).Date)
                            cmd.Parameters.AddWithValue("@USUARIODESPACHO", Session("User"))
                            cmd.Parameters.AddWithValue("@FECHAACTUALIZACIONDESPACHO", DateTime.Now)
                            cmd.Parameters.AddWithValue("@DESPACHOHEADER", numerodespacho)

                            registrosInsertados += cmd.ExecuteNonQuery()
                        End Using
                    End If
                Next
                con.Close()
            End Using


            ClientScript.RegisterStartupScript(Me.GetType(), "alert", "Swal.fire('Éxito', 'Almacenes agregados correctamente.', 'success');", True)
            BindGrid()
            BindGridDespachosAbiertos()
            BindGridDetallePlan(lblPlanId.Text)

        Catch ex As Exception
            Response.Write("<script>console.log('Error en InsertarDespachosSeleccionados: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            registrosInsertados = 0
        End Try

        Return registrosInsertados
    End Function
End Class

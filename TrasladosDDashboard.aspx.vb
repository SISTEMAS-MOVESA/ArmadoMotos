Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDDashboard
    Inherits System.Web.UI.Page

    Private Sub TrasladosDDashboard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                End If
                Dim hoy As Date = Date.Today
                txtDesde.Text = New Date(hoy.Year, hoy.Month, 1).ToString("yyyy-MM-dd")
                txtHasta.Text = New Date(hoy.Year, hoy.Month, Date.DaysInMonth(hoy.Year, hoy.Month)).ToString("yyyy-MM-dd")
                CargarTodo()
            End If
        Catch ex As Exception
            Response.Write("TrasladosDDashboard_Load " & ex.Message)
        End Try
    End Sub

    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs)
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
        Dim rng = GetFechas()
        lblRango.Text = rng.Item1.ToString("dd/MM/yyyy") & " al " & rng.Item2.ToString("dd/MM/yyyy")
        BindGrid(rng.Item1, rng.Item2)
        BindGridDespachosAbiertos(rng.Item1, rng.Item2)
    End Sub

    Private Sub BindGrid(desde As Date, hasta As Date)
        Try
            Dim sql As String =
                "SELECT [ID], CONVERT(CHAR,FECHACREACION,103) AS FECHACREACION, " &
                "CONVERT(CHAR,FECHAINICIO,103) AS FECHAINICIO, " &
                "CONVERT(CHAR,FECHAVENCIMIENTO,103) AS FECHAVENCIMIENTO, " &
                "[FALTANTE],[ESTADO],[USUARIO] " &
                "FROM [dbo].[PLANIFICACIONES] " &
                "WHERE ESTADO='ABIERTO' " &
                "  AND FECHACREACION >= @desde AND FECHACREACION < DATEADD(DAY,1,@hasta) " &
                "ORDER BY FECHACREACION DESC"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@desde", desde),
                    New SqlParameter("@hasta", hasta)
                })
            gridDespachos.DataSource = dt
            gridDespachos.DataBind()
            gridDespachos.UseAccessibleHeader = True
            If gridDespachos.HeaderRow IsNot Nothing Then
                gridDespachos.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub BindGridDespachosAbiertos(desde As Date, hasta As Date)
        Try
            Dim sql As String =
                "SELECT DISTINCT T0.ID AS [Id Despacho], T0.PLANID, " &
                "CONVERT(CHAR,T0.FECHACREACION,103) AS [Fecha Creacion], " &
                "CONVERT(CHAR,T0.FECHAINICIO,103) AS [Fecha Inicio], " &
                "CONVERT(CHAR,T0.FECHAVENCE,103) AS [Fecha Vencimiento], " &
                "T0.ESTADO, " &
                "(SELECT COUNT(codigo) FROM DESPACHOS_DETALLE WHERE PLANID=T0.PLANID AND CODIGOESTADO='DESPACHO ABIERTO') AS [Almacenes], " &
                "(SELECT ISNULL(SUM(faltante),0) FROM DESPACHOS_DETALLE WHERE PLANID=T0.PLANID AND CODIGOESTADO='DESPACHO ABIERTO') AS [Tot Faltante] " &
                "FROM [DEPACHOS_HEADER] T0 WITH(NOLOCK) " &
                "INNER JOIN DESPACHOS_DETALLE_MOTOS T1 WITH(NOLOCK) ON T0.ID = T1.HEADERID " &
                "WHERE T0.ESTADO='ABIERTO' AND T1.CODIGOESTADO='DESPACHO ABIERTO' " &
                "  AND T0.FECHACREACION >= @desde AND T0.FECHACREACION < DATEADD(DAY,1,@hasta)"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@desde", desde),
                    New SqlParameter("@hasta", hasta)
                })
            gridDespachosAbiertos.DataSource = dt
            gridDespachosAbiertos.DataBind()
            gridDespachosAbiertos.UseAccessibleHeader = True
            If gridDespachosAbiertos.HeaderRow IsNot Nothing Then
                gridDespachosAbiertos.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDespachosAbiertos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub BindGridDetallePlan(idPlan As String)
        Try
            Dim sql As String =
                "SELECT *, " &
                "ISNULL((SELECT SUM([CANTIDAD]) FROM [dbo].[DESPACHOS_DETALLE_MOTOS] " &
                "  WHERE ALMDESTINO=[PLANIFICACIONES_DETALLE].Codigo AND CODIGOESTADO<>'CERRADO'),0) AS Despacho, " &
                "ISNULL(CONVERT(DECIMAL(19,2), CONVERT(DECIMAL(19,4), " &
                "  ([Faltante]-ISNULL((SELECT SUM([CANTIDAD]) FROM [dbo].[DESPACHOS_DETALLE_MOTOS] " &
                "    WHERE ALMDESTINO=[PLANIFICACIONES_DETALLE].Codigo AND CODIGOESTADO<>'CERRADO'),0))) " &
                "  / NULLIF(CONVERT(DECIMAL(19,4),[Cuadro]),0)*100),0) AS Indice_Proyectado " &
                "FROM [dbo].[PLANIFICACIONES_DETALLE] WHERE [HEADERID]=@p1"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@p1", idPlan)})
            gridAlmacenesDespachos.DataSource = dt
            gridAlmacenesDespachos.DataBind()
            gridAlmacenesDespachos.UseAccessibleHeader = True
            If gridAlmacenesDespachos.HeaderRow IsNot Nothing Then
                gridAlmacenesDespachos.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDetallePlan: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Protected Sub gridIndiceD_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.Add("data-merge", "true")
            e.Row.Cells(1).Attributes.Add("data-merge", "true")
            e.Row.Cells(2).Attributes.Add("data-merge", "true")
            e.Row.Cells(3).Attributes.Add("data-merge", "true")
            Dim codigo As String = e.Row.Cells(6).Text
            Dim planId As String = e.Row.Cells(18).Text
            If ExisteDespacho(codigo, planId) Then
                Dim cb As CheckBox = TryCast(e.Row.FindControl("cbDocument"), CheckBox)
                If cb IsNot Nothing Then cb.Visible = False
                Dim lbl As New Label()
                lbl.Text = "<i class='fa fa-check fa-2x text-success'></i>"
                e.Row.Cells(19).Controls.Add(lbl)
            End If
        End If
    End Sub

    Public Function ExisteDespacho(codigo As String, planId As String) As Boolean
        Try
            Dim sql As String =
                "SELECT COUNT(*) FROM [dbo].[DESPACHOS_DETALLE] " &
                "WHERE CODIGO=@p1 AND PLANID=@p2 AND ESTADOLINEA='Pendiente'"
            Dim result As Object = DbConfig.ExecuteScalar(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@p1", codigo),
                    New SqlParameter("@p2", planId)
                })
            Return Convert.ToInt32(result) > 0
        Catch
            Return False
        End Try
    End Function

    Private Sub gridDespachos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDespachos.RowCommand
        Try
            If e.CommandName = "Ver" Then
                EliminarLinea(Session("Name").ToString())
                Dim idx As Integer = Convert.ToInt32(e.CommandArgument)
                Response.Redirect("TrasladosDDashboardDetalle.aspx?planid=" & gridDespachos.Rows(idx).Cells(0).Text)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridDespachos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub EliminarLinea(usuario As String)
        Try
            Dim sql As String =
                "DELETE FROM [dbo].[MACROINSERT] WHERE ESTADO='T' AND USUARIO=@u; " &
                "DELETE FROM [dbo].[CARGA_CAMION_DETALLE] WHERE ESTADO='T' AND USUARIO=@u;"
            DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@u", usuario)})
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
        Dim chars() As String = {"/", "\", ":", "?", Chr(34), "<", ">", "|", "&", "%", "*", "'", "{", "[", "]", "}", "!", ","}
        For Each c As String In chars
            sName = Replace(sName, c, sChr)
        Next
        Return sName
    End Function

End Class

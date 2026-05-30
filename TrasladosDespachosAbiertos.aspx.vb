Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDespachosAbiertos
    Inherits System.Web.UI.Page

    Private Sub TrasladosDespachosAbiertos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Name") Is vbNullString Then Response.Redirect("Default.aspx")
                If Session("USERROL") = "Jefe Tienda" Then Response.Redirect("MaindashboardSucursales.aspx")
                Dim hoy As Date = Date.Today
                txtDesde.Text = New Date(hoy.Year, hoy.Month, 1).ToString("yyyy-MM-dd")
                txtHasta.Text = New Date(hoy.Year, hoy.Month, Date.DaysInMonth(hoy.Year, hoy.Month)).ToString("yyyy-MM-dd")
                CargarTodo()
                cargarPlacas()
                cargarMotorista()
            End If
        Catch ex As Exception
            Response.Write("TrasladosDespachosAbiertos_Load: " & ex.Message)
        End Try
    End Sub

    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
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
        BindGridDespachosAbiertos(rng.Item1, rng.Item2)
        BindGridDespachosAbiertosDetalleMotos(rng.Item1, rng.Item2)
        BindGridCamionesAbiertos()
        BindGridCamionesAbiertosDetalle()
    End Sub

    ' ── Tab 1: Despachos Abiertos ─────────────────────────────────────────────

    Private Sub BindGridDespachosAbiertos(desde As Date, hasta As Date)
        Try
            Dim sql As String =
                "SELECT H.DOCNUM [DespachoId], H.BASENUM [PlanId], FORMAT(H.DOCDUEDATE,'yyyy/MM/dd') [FechaDespacho], " &
                "DATEDIFF(DAY,GETDATE(),H.DOCDUEDATE) [DiasRestantes], D.CANAL [Canal], " &
                "COUNT(DISTINCT D.TO_WHSCODE) [TotalAlm], 0 [TotalFalt], COUNT(D.LINENUM) [Unds], " &
                "SUM(D.SLOT) [TotalEspacios], " &
                "SUM(CASE WHEN D.PICKED='Y' THEN 1 ELSE 0 END) [Preparado], " &
                "CONVERT(DECIMAL(18,2),SUM(CASE WHEN D.PICKED='Y' THEN 1 ELSE 0 END)) " &
                "/ NULLIF(CONVERT(DECIMAL(18,2),COUNT(D.LINENUM)),0) * 100 [Porcentaje], " &
                "STUFF((SELECT DISTINCT ', '+UPPER(ALMDESTINO) FROM ArmadoMotos..DESPACHOS_DETALLE_MOTOS " &
                "WHERE DESPACHOID=H.DOCNUM AND TIPO=D.CANAL " &
                "FOR XML PATH(''),TYPE).value('.','NVARCHAR(MAX)'),1,2,'') [Almacenes], " &
                "STUFF((SELECT DISTINCT ', '+UPPER(RUTA) FROM ArmadoMotos..DESPACHOS_DETALLE_MOTOS " &
                "WHERE DESPACHOID=H.DOCNUM AND TIPO=D.CANAL " &
                "FOR XML PATH(''),TYPE).value('.','NVARCHAR(MAX)'),1,2,'') [Ruta] " &
                "FROM ArmadoMotos..VW_DESPACHOS AS H " &
                "INNER JOIN ARMADOMOTOS..VW_DESPACHOS_DETALLE AS D ON D.DOCNUM=H.DOCNUM " &
                "WHERE DOCSTATUS IN ('PICK_EN_PROCESO','SERIES_PICKEADAS') " &
                "  AND H.DOCDUEDATE >= @desde AND H.DOCDUEDATE < DATEADD(DAY,1,@hasta) " &
                "GROUP BY H.DOCNUM, H.BASENUM, H.DOCDUEDATE, D.CANAL " &
                "ORDER BY H.DOCDUEDATE DESC"
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

    Protected Sub gridDespachosAbiertos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridDespachosAbiertos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Cells(5) = DiasRestantes
            Dim diasRestantes As Integer
            If Integer.TryParse(e.Row.Cells(5).Text, diasRestantes) Then
                Dim maxAbs As Integer = 7
                Dim clamped As Integer = Math.Max(-maxAbs, Math.Min(maxAbs, diasRestantes))
                Dim normalized As Double = (clamped + maxAbs) / (2.0 * maxAbs)
                Dim red As Integer = CInt(255 * (1 - normalized))
                Dim green As Integer = CInt(255 * normalized)
                e.Row.Cells(5).BackColor = Drawing.Color.FromArgb(red, green, 0)
                e.Row.Cells(5).ForeColor = Drawing.Color.White
                e.Row.Cells(5).Attributes.Add("style", "text-shadow:1px 1px 2px black; font-weight:bold; text-align:center;")
            End If
        End If
    End Sub

    Private Sub gridDespachosAbiertos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDespachosAbiertos.RowCommand
        ' Cells: [0]=Pick%,[1]=chk,[2]=DespachoId,[3]=PlanId,[4]=FechaDespacho,[5]=Dias,
        '        [6]=Canal,[7]=Ruta,[8]=Almacenes,[9]=TotalAlm,[10]=TotalFalt,[11]=Unds,
        '        [12]=TotalEspacios,[13]=Preparado,[14]=Eliminar
        If e.CommandName = "Eliminar" Then
            Try
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridDespachosAbiertos.Rows(index)
                Dim preparado As Integer = 0
                Integer.TryParse(gvRow.Cells(13).Text, preparado)
                If preparado > 0 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertSeries",
                        "<script>Swal.fire({icon:'error',title:'No se puede cerrar',text:'Ya hay series asignadas. Vaya a la seccion Detalle.',confirmButtonText:'Entendido'});</script>", False)
                    Exit Sub
                End If
                Dim despachoID As String = gvRow.Cells(2).Text
                Dim planId As String = gvRow.Cells(3).Text
                Dim usuario As String = If(Session("User") IsNot Nothing, Session("User").ToString(), "ANONIMO")
                Dim sql As String =
                    "UPDATE [ArmadoMotos].[dbo].[DEPACHOS_HEADER] SET [ESTADO]='CERRADO',[USUARIOMOFICACION]=@usuario,[FECHAMODIFICACION]=GETDATE() WHERE PLANID=@planid AND ID=@did;" &
                    "UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO='CERRADO' WHERE PLANID=@planid AND DESPACHOID=@did;" &
                    "DELETE FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE PLANID=@planid AND HEADERID=@did;" &
                    "UPDATE MOVESAWEB..TBL_REPORTES_VENTAS_DISTRIBUIDORES_DETALLE SET DISPATCH_ID=NULL,DISPATCH_DATE=NULL WHERE DISPATCH_ID=@did;"
                Try
                    DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                        New List(Of SqlParameter) From {
                            New SqlParameter("@usuario", usuario),
                            New SqlParameter("@planid", planId),
                            New SqlParameter("@did", despachoID)
                        })
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "okCierre",
                        "<script>Swal.fire({icon:'success',title:'Despacho cerrado correctamente',confirmButtonText:'Aceptar'}).then(()=>{window.location.href=window.location.href;});</script>", False)
                Catch exInner As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errCierre",
                        "<script>Swal.fire({icon:'error',title:'Error al cerrar',text:'" & ReplaceCharsForFileName(exInner.Message, " ") & "',confirmButtonText:'Aceptar'});</script>", False)
                End Try
            Catch ex As Exception
                Response.Write("<script>console.log('gridDespachosAbiertos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            End Try
        End If
    End Sub

    ' ── Tab 2: Detalle Motos ──────────────────────────────────────────────────

    Private Sub BindGridDespachosAbiertosDetalleMotos(desde As Date, hasta As Date)
        Try
            Dim sql As String =
                "SELECT DISTINCT t2.ID, t0.ID [DespachoID], T2.PLANID, T2.TIPO, t2.RUTA, " &
                "CONVERT(CHAR,t0.FECHAVENCE,103) [Fecha Despacho], " &
                "DATEDIFF(DAY,GETDATE(),t0.FECHAVENCE) [Dias Restantes], " &
                "t2.ALMDESTINO, t2.MODELO, t2.ARTICULO, t2.DESCRIPCION, t2.CANTIDAD, t2.SERIEASIGNADA, t2.OBSERVACIONES " &
                "FROM [DEPACHOS_HEADER] T0 WITH(NOLOCK) " &
                "INNER JOIN [DESPACHOS_DETALLE_MOTOS] T2 WITH(NOLOCK) ON T0.ID=T2.HEADERID " &
                "WHERE T0.ESTADO='ABIERTO' AND t2.CODIGOESTADO='DESPACHO ABIERTO' " &
                "  AND t0.FECHAVENCE >= @desde AND t0.FECHAVENCE < DATEADD(DAY,1,@hasta)"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@desde", desde),
                    New SqlParameter("@hasta", hasta)
                })
            gridDespachosAbiertosMotos.DataSource = dt
            gridDespachosAbiertosMotos.DataBind()
            gridDespachosAbiertosMotos.UseAccessibleHeader = True
            If gridDespachosAbiertosMotos.HeaderRow IsNot Nothing Then
                gridDespachosAbiertosMotos.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDespachosAbiertosDetalleMotos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Protected Sub gridDespachosAbiertosMotos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridDespachosAbiertosMotos.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' Cells(7) = Dias Restantes (after button at [0])
                Dim diasRestantes As Integer
                If Integer.TryParse(e.Row.Cells(7).Text, diasRestantes) Then
                    Dim maxAbs As Integer = 7
                    Dim clamped As Integer = Math.Max(-maxAbs, Math.Min(maxAbs, diasRestantes))
                    Dim normalized As Double = (clamped + maxAbs) / (2.0 * maxAbs)
                    Dim red As Integer = CInt(255 * (1 - normalized))
                    Dim green As Integer = CInt(255 * normalized)
                    e.Row.Cells(7).BackColor = Drawing.Color.FromArgb(red, green, 0)
                    e.Row.Cells(7).ForeColor = Drawing.Color.White
                    e.Row.Cells(7).Attributes.Add("style", "text-shadow:1px 1px 2px black; font-weight:bold; text-align:center;")
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub gridDespachosAbiertosMotos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDespachosAbiertosMotos.RowCommand
        ' Cells: [0]=btn,[1]=ID,[2]=DespachoID,[3]=PLANID,[4]=TIPO,[5]=RUTA,
        '        [6]=FechaDespacho,[7]=DiasRestantes,[8]=ALMDESTINO,[9]=MODELO,
        '        [10]=ARTICULO,[11]=DESCRIPCION,[12]=CANTIDAD,[13]=SERIEASIGNADA,[14]=OBSERVACIONES
        If e.CommandName = "Eliminar" Then
            Try
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridDespachosAbiertosMotos.Rows(index)
                Dim serie As String = gvRow.Cells(13).Text.Trim()
                If Not String.IsNullOrEmpty(serie) AndAlso serie <> "&nbsp;" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertSeries",
                        "<script>Swal.fire({icon:'error',title:'No se puede eliminar',text:'Ya hay series asignadas. Proceda via Devolucion.',confirmButtonText:'Entendido'});</script>", False)
                    Exit Sub
                End If
                Dim rowid As String = gvRow.Cells(1).Text
                Dim planid As String = gvRow.Cells(3).Text
                Dim despachoID As String = gvRow.Cells(2).Text
                Dim almdestino As String = gvRow.Cells(8).Text
                Dim articulo As String = gvRow.Cells(10).Text
                Dim sql As String =
                    "UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO='CERRADO' WHERE id=@id;" &
                    "DELETE FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE PLANID=@planid AND HEADERID=@did AND ALMDESTINO=@alm AND ARTICULO=@art;"
                Try
                    DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                        New List(Of SqlParameter) From {
                            New SqlParameter("@id", rowid),
                            New SqlParameter("@planid", planid),
                            New SqlParameter("@did", despachoID),
                            New SqlParameter("@alm", almdestino),
                            New SqlParameter("@art", articulo)
                        })
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "okElim",
                        "<script>Swal.fire({icon:'success',title:'Linea eliminada correctamente',confirmButtonText:'Aceptar'}).then(()=>{window.location.href=window.location.href;});</script>", False)
                Catch exInner As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errElim",
                        "<script>Swal.fire({icon:'error',title:'Error',text:'" & ReplaceCharsForFileName(exInner.Message, " ") & "',confirmButtonText:'Aceptar'});</script>", False)
                End Try
            Catch ex As Exception
                Response.Write("<script>console.log('gridDespachosAbiertosMotos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            End Try
        End If
    End Sub

    ' ── Tab 3: Camiones Abiertos ──────────────────────────────────────────────

    Private Sub BindGridCamionesAbiertos()
        Try
            Dim sql As String =
                "SELECT ID, ISNULL((SELECT TOP 1 DESPACHOID FROM CARGA_CAMION_DETALLE WHERE IDCAMION=[CARGA_CAMION_HEADER].[ID]),0) [DespachoID], " &
                "MOTORISTA, FECHASALIDA, ESTADO, FECHACREACION, USUARIO " &
                "FROM CARGA_CAMION_HEADER ORDER BY ID DESC"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
            gridCamiones.DataSource = dt
            gridCamiones.DataBind()
            gridCamiones.UseAccessibleHeader = True
            If gridCamiones.HeaderRow IsNot Nothing Then
                gridCamiones.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridCamionesAbiertos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridCamiones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCamiones.RowCommand
        ' Cells: [0]=Ver,[1]=Imprimir,[2]=Eliminar,[3]=ID,[4]=DespachoID,[5]=MOTORISTA,...
        Try
            If e.CommandName = "Ver" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim url As String = "TrasladosDetallesCamion.aspx?idcamion=" & gridCamiones.Rows(index).Cells(3).Text
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTab", "window.open('" & url & "','_blank');", True)
            End If
            If e.CommandName = "Imprimir" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCamiones.Rows(index)
                Dim url As String = "DespachosCaratula.aspx?idcamion=" & gvRow.Cells(3).Text & "&motorista=" & gvRow.Cells(5).Text
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTab", "window.open('" & url & "','_blank');", True)
            End If
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim camionId As String = gridCamiones.Rows(index).Cells(3).Text
                Dim ok As Boolean = EliminarCamion(camionId)
                If ok Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "okCamion",
                        "<script>Swal.fire({icon:'success',title:'Camion eliminado correctamente',confirmButtonText:'Aceptar'});</script>", False)
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errCamion",
                        "<script>Swal.fire({icon:'error',title:'Error al eliminar el camion',confirmButtonText:'Aceptar'});</script>", False)
                End If
            End If
            CargarTodo()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "TabChangeScript",
                "$('#mainTabs a[href=""#tab3""]').tab('show');", True)
        Catch ex As Exception
            Response.Write("<script>console.log('gridCamiones_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    ' ── Tab 4: Detalle Camiones ───────────────────────────────────────────────

    Private Sub BindGridCamionesAbiertosDetalle()
        Try
            Dim sql As String =
                "SELECT ID, ALMDESTINO, ARTICULO, DESCRIPCION, CANTIDAD, TIPO, " &
                "ESPACIOS, OBSERVACIONES, PLANID, DESPACHOID, SERIEASIGNADA, ESCANEADA, FECHAESCANEO " &
                "FROM CARGA_CAMION_DETALLE WHERE CODIGOESTADO='ABIERTO'"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
            gridCamionesDetalle.DataSource = dt
            gridCamionesDetalle.DataBind()
            gridCamionesDetalle.UseAccessibleHeader = True
            If gridCamionesDetalle.HeaderRow IsNot Nothing Then
                gridCamionesDetalle.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridCamionesAbiertosDetalle: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    ' ── Dropdowns ─────────────────────────────────────────────────────────────

    Public Sub cargarPlacas()
        Try
            Dim dt As DataTable = DbConfig.GetDataTable(
                "SELECT [ID],[PLACA] FROM [ArmadoMotos].[dbo].[TRANSPORTISTAS]",
                DbConfig.DBServer.ARMADOMOTOS)
            drpPlacas.DataTextField = "PLACA"
            drpPlacas.DataValueField = "ID"
            drpPlacas.DataSource = dt
            drpPlacas.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('cargarPlacas: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub cargarMotorista()
        Try
            Dim dt As DataTable = DbConfig.GetDataTable(
                "SELECT [ID],[MOTORISTA] FROM [ArmadoMotos].[dbo].[TRANSPORTISTAS]",
                DbConfig.DBServer.ARMADOMOTOS)
            drpMotorista.DataTextField = "MOTORISTA"
            drpMotorista.DataValueField = "ID"
            drpMotorista.DataSource = dt
            drpMotorista.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('cargarMotorista: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    ' ── Helper operations ─────────────────────────────────────────────────────

    Public Sub EliminarLinea(almdestino As String, articulo As String, planid As String)
        Try
            DbConfig.ExecuteNonQuery(
                "DELETE FROM DESPACHOS_DETALLE_MOTOS WHERE ALMDESTINO=@alm AND ARTICULO=@art AND PLANID=@planid;" &
                "DELETE FROM [MACROINSERT] WHERE ALMDESTINO=@alm AND ARTICULO=@art AND PLANID=@planid;",
                DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@alm", almdestino),
                    New SqlParameter("@art", articulo),
                    New SqlParameter("@planid", planid)
                })
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "TabChangeScript",
                "$('#mainTabs a[href=""#tab2""]').tab('show');", True)
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function EliminarCamion(camionid As String) As Boolean
        Try
            DbConfig.ExecuteNonQuery(
                "DELETE FROM CARGA_CAMION_HEADER WHERE ID=@id",
                DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@id", camionid)})
            Return True
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarCamion: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            Return False
        End Try
    End Function

    ' ── Button handlers ───────────────────────────────────────────────────────

    Private Sub btnAgregarCamion_Click(sender As Object, e As EventArgs) Handles btnAgregarCamion.Click
    End Sub

    Private Sub btnCrearCamion_Click(sender As Object, e As EventArgs) Handles btnCrearCamion.Click
        ' Never fires — OnClientClick shows Bootstrap modal and returns false
    End Sub

    Private Sub btnCamionCrear_Click(sender As Object, e As EventArgs) Handles btnCamionCrear.Click
        Try
            If String.IsNullOrEmpty(txtFechaCamion.Text.Trim()) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errFecha",
                    "<script>Swal.fire({icon:'error',title:'Fecha requerida',text:'Agrega una fecha de salida antes de continuar.',confirmButtonText:'OK'});</script>", False)
                Exit Sub
            End If
            Dim usuario As String = If(Session("User") IsNot Nothing, Session("User").ToString(), "ANONIMO")
            Dim placa As String = drpPlacas.SelectedItem.Text
            Dim motorista As String = drpMotorista.SelectedItem.Text
            Dim nuevoId As Integer = Convert.ToInt32(
                DbConfig.ExecuteScalar(
                    "INSERT INTO [dbo].[CARGA_CAMION_HEADER]([FECHASALIDA],[ESTADO],[FECHACREACION],[USUARIO],[PLACA],[MOTORISTA]) " &
                    "VALUES (@fecha,'ABIERTO',GETDATE(),@usuario,@placa,@motorista); SELECT SCOPE_IDENTITY();",
                    DbConfig.DBServer.ARMADOMOTOS,
                    New List(Of SqlParameter) From {
                        New SqlParameter("@fecha", CDate(txtFechaCamion.Text)),
                        New SqlParameter("@usuario", usuario),
                        New SqlParameter("@placa", placa),
                        New SqlParameter("@motorista", motorista)
                    }))
            Dim detailSql As String =
                "IF NOT EXISTS (SELECT 1 FROM CARGA_CAMION_DETALLE WHERE PLANID=@planid AND HEADERID=@did) BEGIN " &
                "INSERT INTO [dbo].[CARGA_CAMION_DETALLE] " &
                "([IDCAMION],[RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],[ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS],[CANTIDAD]," &
                "[QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[HEADERID],[ESTADO],[TIPO],[SUPERVISOR],[FECHA],[USUARIO],[CB]," &
                "[FISICO],[FALTANTE],[VTAA],[FARMADO],[FDESPACHO],[PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO]," &
                "[SERIEASIGNADA],[SERIESYS],[FECHASERIE],[PEDIDOID],[PEDIDOLINEA],[DESPACHOROWID]) " &
                "SELECT @idcamion,[RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],[ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS],[CANTIDAD]," &
                "[QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[HEADERID],[ESTADO],[TIPO],[SUPERVISOR],[FECHA],[USUARIO],[CB]," &
                "[FISICO],[FALTANTE],[VTAA],[FARMADO],[FDESPACHO],[PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO]," &
                "[SERIEASIGNADA],[SERIESYS],[FECHASERIE],[PEDIDOID],[PEDIDOLINEA],[ID] " &
                "FROM DESPACHOS_DETALLE_MOTOS WHERE PLANID=@planid AND HEADERID=@did AND DELETED='N'; " &
                "UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO='CAMION ABIERTO',CAMIONID=@idcamion WHERE DESPACHOID=@did; " &
                "UPDATE [ArmadoMotos].[dbo].[MACROINSERT] SET ESTADO='F' WHERE HEADERID=@did AND PLANID=@planid; END"
            For Each row As GridViewRow In gridDespachosAbiertos.Rows
                Dim cb As CheckBox = CType(row.FindControl("cbDocument"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked Then
                    Dim planid As String = row.Cells(3).Text.Trim()
                    Dim despachoId As String = row.Cells(2).Text.Trim()
                    DbConfig.ExecuteNonQuery(detailSql, DbConfig.DBServer.ARMADOMOTOS,
                        New List(Of SqlParameter) From {
                            New SqlParameter("@idcamion", nuevoId),
                            New SqlParameter("@planid", planid),
                            New SqlParameter("@did", despachoId)
                        })
                End If
            Next
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "okCamion",
                "<script>Swal.fire({icon:'success',title:'Camion creado',text:'ID: " & nuevoId & "',confirmButtonText:'OK'})" &
                ".then(()=>{window.location.href='DetalleCargaCamion.aspx?idcamion=" & nuevoId & "&motorista=" & motorista & "';});</script>", False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errCamion",
                "<script>Swal.fire({icon:'error',title:'Error',text:'" & ReplaceCharsForFileName(ex.Message, " ") & "',confirmButtonText:'OK'});</script>", False)
        End Try
    End Sub

    ' ── Utilities ─────────────────────────────────────────────────────────────

    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
        Dim chars() As String = {"/", "\", ":", "?", Chr(34), "<", ">", "|", "&", "%", "*", "'", "{", "[", "]", "}", "!", ","}
        For Each c As String In chars
            sName = Replace(sName, c, sChr)
        Next
        Return sName
    End Function

End Class

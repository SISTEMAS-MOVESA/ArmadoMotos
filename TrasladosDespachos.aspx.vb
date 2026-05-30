Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDespachos
    Inherits System.Web.UI.Page

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerEventos() As Object
        Dim eventos As New List(Of Object)
        Dim sql As String =
            "SELECT t0.ID, t0.PLANID, t0.FECHAVENCE, t1.ruta, t1.almdestino, " &
            "(SELECT whsname FROM movesa..owhs WHERE whscode=T1.ALMDESTINO COLLATE Modern_Spanish_CI_AS) [Nalmacen], " &
            "SUM(t1.ESPACIOS) AS TotalEspacios, SUM(t1.CANTIDAD) AS TotalCantidad " &
            "FROM [ArmadoMotos].[dbo].[DEPACHOS_HEADER] t0 WITH(NOLOCK) " &
            "INNER JOIN [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] t1 WITH(NOLOCK) ON t0.ID=t1.HEADERID " &
            "GROUP BY t0.ID, t0.PLANID, t0.FECHAVENCE, t1.ruta, t1.almdestino"
        Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
        For Each row As DataRow In dt.Rows
            eventos.Add(New With {
                .title = row("ruta").ToString() & " - " & row("almdestino").ToString() & "  " & row("Nalmacen").ToString() &
                         " (" & row("TotalCantidad").ToString() & " unidades, " & row("TotalEspacios").ToString() & " espacios), Planid: " & row("PLANID").ToString(),
                .start = Convert.ToDateTime(row("FECHAVENCE")).ToString("yyyy-MM-dd"),
                .allDay = True,
                .planid = row("PLANID")
            })
        Next
        Return eventos
    End Function

    Private Sub TrasladosDespachos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Name") Is vbNullString Then Response.Redirect("Default.aspx")
                If Session("USERROL") = "Jefe Tienda" Then Response.Redirect("MaindashboardSucursales.aspx")
                Dim hoy As Date = Date.Today
                Dim mesProx As Date = hoy.AddMonths(1)
                txtDesde.Text = New Date(hoy.Year, hoy.Month, 1).ToString("yyyy-MM-dd")
                txtHasta.Text = New Date(mesProx.Year, mesProx.Month, Date.DaysInMonth(mesProx.Year, mesProx.Month)).ToString("yyyy-MM-dd")
                CargarTodo()
            End If
        Catch ex As Exception
            Response.Write("TrasladosDespachos_Load: " & ex.Message)
        End Try
    End Sub

    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        CargarTodo()
    End Sub

    Private Function GetFechas() As Tuple(Of Date, Date)
        Dim hoy As Date = Date.Today
        Dim mesProx As Date = hoy.AddMonths(1)
        Dim desde As Date = New Date(hoy.Year, hoy.Month, 1)
        Dim hasta As Date = New Date(mesProx.Year, mesProx.Month, Date.DaysInMonth(mesProx.Year, mesProx.Month))
        If Not Date.TryParse(txtDesde.Text, desde) Then desde = New Date(hoy.Year, hoy.Month, 1)
        If Not Date.TryParse(txtHasta.Text, hasta) Then hasta = New Date(mesProx.Year, mesProx.Month, Date.DaysInMonth(mesProx.Year, mesProx.Month))
        If desde > hasta Then desde = hasta
        Return Tuple.Create(desde, hasta)
    End Function

    Private Sub CargarTodo()
        Dim rng = GetFechas()
        lblRango.Text = rng.Item1.ToString("dd/MM/yyyy") & " al " & rng.Item2.ToString("dd/MM/yyyy")
        BindGridDespachosAbiertos(rng.Item1, rng.Item2)
    End Sub

    Private Sub BindGridDespachosAbiertos(desde As Date, hasta As Date)
        Try
            Dim sql As String =
                "SELECT H.DOCNUM [DespachoId], H.BASENUM [PlanId], FORMAT(H.DOCDUEDATE,'yyyy/MM/dd') [FechaDespacho], " &
                "DATEDIFF(DAY,GETDATE(),H.DOCDUEDATE) [DiasRestantes], " &
                "D.CANAL, COUNT(DISTINCT D.FROM_WHSCODE) [TotalAlm], 0 [TotalFalt], COUNT(D.LINENUM) [Unds], SUM(D.SLOT) [TotalEspacios], " &
                "SUM(CASE WHEN D.PICKED='Y' THEN 1 ELSE 0 END) [Preparado], " &
                "SUM(CASE WHEN D.IN_TRUCK='Y' THEN 1 ELSE 0 END) [Cargado], " &
                "CONVERT(DECIMAL(18,2),SUM(CASE WHEN D.PICKED='Y' THEN 1 ELSE 0 END))/NULLIF(CONVERT(DECIMAL(18,2),COUNT(D.LINENUM)),0)*100 [PorcentajePick], " &
                "CONVERT(DECIMAL(18,2),SUM(CASE WHEN D.IN_TRUCK='Y' THEN 1 ELSE 0 END))/NULLIF(CONVERT(DECIMAL(18,2),COUNT(D.LINENUM)),0)*100 [PorcentajeCarga], " &
                "STUFF((SELECT DISTINCT ', '+TO_WHSCODE FROM ArmadoMotos..VW_DESPACHOS_DETALLE WHERE DOCNUM=H.DOCNUM FOR XML PATH(''),TYPE).value('.','NVARCHAR(MAX)'),1,2,'') [Almacenes], " &
                "STUFF((SELECT DISTINCT ', '+ROAD FROM ArmadoMotos..VW_DESPACHOS_DETALLE WHERE DOCNUM=H.DOCNUM FOR XML PATH(''),TYPE).value('.','NVARCHAR(MAX)'),1,2,'') [Ruta] " &
                "FROM ARMADOMOTOS..VW_DESPACHOS AS H " &
                "INNER JOIN VW_DESPACHOS_DETALLE AS D ON D.DOCNUM=H.DOCNUM " &
                "WHERE H.DOCSTATUS IN ('PICK_EN_PROCESO','SERIES_PICKEADAS') " &
                "  AND H.DOCDUEDATE >= @desde AND H.DOCDUEDATE < DATEADD(DAY,1,@hasta) " &
                "GROUP BY H.DOCNUM, H.BASENUM, H.DOCDUEDATE, D.CANAL " &
                "ORDER BY H.DOCDUEDATE"
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
            ' Cells(7) = DiasRestantes
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
    End Sub

    Private Sub BindGridgridAlmacenesDespacho(despachoid As String)
        Try
            Dim sql As String =
                "SELECT T0.LINENUM [ID], T0.ROAD [RUTA], T0.TO_WHSCODE [ALMDESTINO], T0.TO_WHSNAME [Nalmacen], " &
                "T0.TO_CARDCDE [CARDCODE], T0.ITEMCODE [ARTICULO], T0.MODEL_NAME [MODELO], T0.ITEMNAME [DESCRIPCION], " &
                "T0.SLOT [ESPACIOS], 1 [CANTIDAD], T0.LINETYPE [OBSERVACIONES], T0.CREATED_BY [USUARIO], " &
                "T0.BASE_DOCNUM [PLANID], DOCNUM [DESPACHOID], ISNULL(SERIAL_NUMBER,'N/A') [SERIEASIGNADA] " &
                "FROM ARMADOMOTOS..VW_DESPACHOS_DETALLE AS T0 WHERE DOCNUM=@p2 AND IN_TRUCK='N'"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@p2", despachoid)})
            gridAlmacenesDespachos.DataSource = dt
            gridAlmacenesDespachos.DataBind()
            gridAlmacenesDespachos.UseAccessibleHeader = True
            If gridAlmacenesDespachos.HeaderRow IsNot Nothing Then
                gridAlmacenesDespachos.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridgridAlmacenesDespacho: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridDespachosAbiertos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDespachosAbiertos.RowCommand
        Try
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim gvRow As GridViewRow = gridDespachosAbiertos.Rows(index)
            ' Cells: 0=Ver,1=Cerrar,2=PickTemplate,3=CargaTemplate,4=DespachoId,5=PlanId,
            '        6=FechaDespacho,7=DiasRestantes,8=Canal,9=Ruta,...,15=Preparado

            If e.CommandName = "Ver" Then
                Dim despachoId As String = gvRow.Cells(4).Text
                Dim planId As String = gvRow.Cells(5).Text
                lblCurrentDespacho.Text = despachoId
                lblPlanId.Text = despachoId
                lblIdPlan.Text = planId
                lblRuta.Text = gvRow.Cells(9).Text
                lblDespachoActivo.Text = despachoId
                lblPlanActivo.Text = planId
                BindGridgridAlmacenesDespacho(despachoId)
                CargarTodo()
            End If

            If e.CommandName = "Cerrar" Then
                Dim despachoId As String = gvRow.Cells(4).Text
                Dim preparado As Integer = 0
                Integer.TryParse(gvRow.Cells(15).Text, preparado)
                If preparado > 0 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertSeries",
                        "<script>Swal.fire({icon:'error',title:'No se puede cerrar',text:'Ya hay series asignadas. Vaya a la sección Detalle.',confirmButtonText:'Entendido'});</script>", False)
                    Exit Sub
                End If
                Try
                    DbConfig.ExecuteNonQuery(
                        "EXEC ARMADOMOTOS..SP_PORTAL_DESPACHOS @FN='CANCELAR DESPACHO',@DOCNUM=@id",
                        DbConfig.DBServer.ARMADOMOTOS,
                        New List(Of SqlParameter) From {New SqlParameter("@id", despachoId)})
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "okCierre",
                        "<script>Swal.fire({icon:'success',title:'Despacho cerrado correctamente',confirmButtonText:'Aceptar'}).then(()=>{window.location.href=window.location.href;});</script>", False)
                Catch exInner As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errCierre",
                        "<script>Swal.fire({icon:'error',title:'Error al cerrar',text:'" & exInner.Message.Replace("'", "") & "',confirmButtonText:'Aceptar'});</script>", False)
                End Try
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridDespachosAbiertos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridAlmacenesDespachos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridAlmacenesDespachos.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridAlmacenesDespachos.Rows(index)
                ' Cells(0)=Eliminar btn, Cells(1)=ID, ..., Cells(15)=SERIEASIGNADA
                If gvRow.Cells(15).Text <> "N/A" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertSeries",
                        "<script>Swal.fire({icon:'error',title:'No se puede eliminar',text:'Ya hay series asignadas. Proceda vía Devolución.',confirmButtonText:'Entendido'});</script>", False)
                    Exit Sub
                End If
                Dim rowid As String = gvRow.Cells(1).Text
                Try
                    DbConfig.ExecuteNonQuery(
                        "EXEC ARMADOMOTOS..SP_PORTAL_DESPACHOS @FN='ELIMINAR LINEA DESPACHO',@LINENUM=@id",
                        DbConfig.DBServer.ARMADOMOTOS,
                        New List(Of SqlParameter) From {New SqlParameter("@id", rowid)})
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "okElim",
                        "<script>Swal.fire({icon:'success',title:'Línea eliminada correctamente',confirmButtonText:'Aceptar'}).then(()=>{window.location.href=window.location.href;});</script>", False)
                Catch exInner As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errElim",
                        "<script>Swal.fire({icon:'error',title:'Error',text:'" & exInner.Message.Replace("'", "") & "',confirmButtonText:'Aceptar'});</script>", False)
                End Try
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridAlmacenesDespachos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnVistaCalendario_Click(sender As Object, e As EventArgs) Handles btnVistaCalendario.Click
        Response.Redirect("TrasladosDespachosCalendario.aspx")
    End Sub

    Public Sub ActualizarDocumento(lineId As String, whscode As String, despachoheader As String)
        Try
            DbConfig.ExecuteNonQuery(
                "UPDATE [PLANIFICACIONES_DETALLE] SET [ESTADOLINEA]='DESPACHO PENDIENTE',[DESPACHOHEADER]=@p3 WHERE [HEADERID]=@p1 AND [CODIGO]=@p2",
                DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@p1", lineId),
                    New SqlParameter("@p2", whscode),
                    New SqlParameter("@p3", despachoheader)
                })
        Catch ex As Exception
            Response.Write("<script>console.log('ActualizarDocumento: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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

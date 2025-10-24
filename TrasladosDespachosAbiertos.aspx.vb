Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDespachosAbiertos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public almacen As String
    Private Sub TrasladosDespachosAbiertos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'BindGrid()
                    BindGridDespachosAbiertos()
                    BindGridDespachosAbiertosDetalleMotos()
                    BindGridCamionesAbiertos()
                    cargarPlacas()
                    cargarMotorista()
                    BindGridCamionesAbiertosDetalle()

                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
                    Else

                        'If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                        '    BindGridgridAlmacenesDespacho(Request.QueryString("planId"))
                        'End If
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindGridDespachosAbiertos()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                'SQL_STRING = "select DISTINCT T0.ID [DespachoId]  " &
                '    " ,T0.PLANID [PlanId] " &
                '    " ,CONVERT(CHAR,t0.FECHAVENCE,103)[FechaDespacho] " &
                '    " ,datediff(day,getdate(),t0.FECHAVENCE)[DiasRestantes] " &
                '    " ,T1.TIPO [Canal],T1.RUTA [Ruta]" &
                '    " ,ISNULL((select count(distinct almdestino) from [DESPACHOS_DETALLE_MOTOS] where PLANID=t0.PLANID And TIPO=T1.TIPO),0)[TotalAlm] " &
                '    " ,ISNULL((select sum(faltante) from [DESPACHOS_DETALLE_MOTOS] where PLANID=t0.PLANID And TIPO=T1.TIPO),0)[TotalFalt] " &
                '    " ,ISNULL((SELECT sum(CANTIDAD) FROM [dbo].[DESPACHOS_DETALLE_MOTOS] where PLANID=t0.PLANID And RUTA=t1.RUTA And TIPO=T1.TIPO),0)[Unds] " &
                '    " ,ISNULL((SELECT sum(ESPACIOS) FROM [dbo].[DESPACHOS_DETALLE_MOTOS] where PLANID=t0.PLANID And RUTA=t1.RUTA And TIPO=T1.TIPO),0)[TotalEspacios] " &
                '    " ,ISNULL((SELECT count(SERIEASIGNADA) FROM [dbo].[DESPACHOS_DETALLE_MOTOS] where PLANID=t0.PLANID And RUTA=t1.RUTA And TIPO=T1.TIPO),0)[Preparado] " &
                '    "  ,(convert(decimal(19,2),ISNULL((SELECT count(SERIEASIGNADA) FROM [dbo].[DESPACHOS_DETALLE_MOTOS] where PLANID=t0.PLANID And RUTA=t1.RUTA And TIPO=T1.TIPO),0)) " &
                '    "  /convert(decimal(19,2),ISNULL((SELECT sum(CANTIDAD) FROM [dbo].[DESPACHOS_DETALLE_MOTOS] where PLANID=t0.PLANID And RUTA=t1.RUTA And TIPO=T1.TIPO),0))) " &
                '    "  * 100 [Porcentaje] " &
                '    " ,(SELECT STUFF((SELECT DISTINCT ', ' + [almdestino] FROM [DESPACHOS_DETALLE_MOTOS] WHERE planid = t0.planid and tipo=t1.tipo FOR XML PATH('')), 1, 2, '') )AS [Almacenes] " &
                '    " ,(SELECT STUFF((SELECT DISTINCT ', ' + [RUTA] FROM [DESPACHOS_DETALLE_MOTOS] WHERE planid =t0.planid FOR XML PATH('')), 1, 2, '') )AS [Rutas] " &
                '    " from [DEPACHOS_HEADER] T0 WITH(NOLOCK) INNER JOIN [DESPACHOS_DETALLE_MOTOS] T1 WITH(NOLOCK) ON T0.ID=T1.HEADERID " &
                '    " WHERE T0.ESTADO='ABIERTO'"

                'SQL_STRING = "select T0.ID [DespachoId] " &
                '    " ,T0.PLANID [PlanId] " &
                '    " ,CONVERT(CHAR,t0.FECHAVENCE,103)[FechaDespacho] " &
                '    " ,datediff(day,getdate(),t0.FECHAVENCE)[DiasRestantes] " &
                '    " ,T1.TIPO [Canal],T1.RUTA [Ruta] " &
                '    " ,count(distinct almdestino) [TotalAlm] " &
                '    " ,sum(faltante)[TotalFalt] " &
                '    " ,sum(CANTIDAD)[Unds] " &
                '    " ,sum(t1.ESPACIOS)[TotalEspacios] " &
                '    " ,count(SERIEASIGNADA)[Preparado] " &
                '    " ,(convert(decimal(18,2),count(SERIEASIGNADA))/convert(decimal(18,2),sum(CANTIDAD)))*100 [Porcentaje] " &
                '    " ,t1.USUARIO " &
                '    " ,(SELECT STUFF((SELECT DISTINCT ', ' + [almdestino] FROM [DESPACHOS_DETALLE_MOTOS] WHERE planid = t0.planid and tipo=t1.tipo and RUTA=t1.RUTA and USUARIO=t1.USUARIO FOR XML PATH('')), 1, 2, '') )AS [Almacenes] " &
                '    " ,(SELECT STUFF((SELECT DISTINCT ', ' + [RUTA] FROM [DESPACHOS_DETALLE_MOTOS] WHERE planid =t0.planid and RUTA=t1.RUTA  and USUARIO=t1.USUARIO FOR XML PATH('')), 1, 2, '') )AS [Rutas] " &
                '    " from [DEPACHOS_HEADER] T0 WITH(NOLOCK) INNER JOIN [DESPACHOS_DETALLE_MOTOS] T1 WITH(NOLOCK) ON T0.ID=T1.HEADERID " &
                '    " WHERE T0.ESTADO='ABIERTO' and t1.CODIGOESTADO='DESPACHO ABIERTO' " &
                '    " group by T0.ID ,T0.PLANID ,CONVERT(CHAR,t0.FECHAVENCE,103),datediff(day,getdate(),t0.FECHAVENCE),T1.TIPO,T1.RUTA ,t1.USUARIO"

                'SQL_STRING = "select T0.ID [DespachoId] " &
                '    " ,T0.PLANID [PlanId],CONVERT(CHAR,t0.FECHAVENCE,103)[FechaDespacho],datediff(day,getdate(),t0.FECHAVENCE)[DiasRestantes] " &
                '    " ,T1.TIPO [Canal],count(distinct almdestino) [TotalAlm],sum(faltante)[TotalFalt],sum(CANTIDAD)[Unds],sum(t1.ESPACIOS)[TotalEspacios] " &
                '    " ,count(SERIEASIGNADA)[Preparado],(convert(decimal(18,2),count(SERIEASIGNADA))/convert(decimal(18,2),sum(CANTIDAD)))*100 [Porcentaje] " &
                '    " ,(SELECT STUFF((SELECT DISTINCT ', ' + [almdestino] FROM [DESPACHOS_DETALLE_MOTOS] WHERE HEADERID=t0.ID and tipo=t1.tipo FOR XML PATH('')), 1, 2, '') )AS [Almacenes] " &
                '    " ,(SELECT STUFF((SELECT DISTINCT ', ' + [RUTA] FROM [DESPACHOS_DETALLE_MOTOS] WHERE HEADERID=t0.ID and tipo=t1.tipo FOR XML PATH('')), 1, 2, '') )AS [Ruta] " &
                '    " from [DEPACHOS_HEADER] T0 WITH(NOLOCK) INNER JOIN [DESPACHOS_DETALLE_MOTOS] T1 WITH(NOLOCK) ON T0.ID=T1.HEADERID " &
                '    " WHERE T0.ESTADO='ABIERTO' and t1.CODIGOESTADO='DESPACHO ABIERTO' " &
                '    " group by T0.ID ,T0.PLANID ,CONVERT(CHAR,t0.FECHAVENCE,103),datediff(day,getdate(),t0.FECHAVENCE),T1.TIPO"

                SQL_STRING = "SELECT H.DOCNUM [DespachoId],  H.BASENUM [PlanId], CONVERT(CHAR,H.DOCDUEDATE,103) [FechaDespacho], " &
                " DATEDIFF(DAY,GETDATE(),H.DOCDUEDATE) [DiasRestantes],D.CANAL [Canal], COUNT(DISTINCT D.TO_WHSCODE) [TotalAlm],  " &
                " 0 [TotalFalt], COUNT(D.LINENUM) [Unds], SUM(D.SLOT) [TotalEspacios], SUM(CASE WHEN D.PICKED = 'Y' THEN 1 ELSE 0 END) [Preparado], " &
                " CONVERT(DECIMAL(18,2), SUM(CASE WHEN D.PICKED = 'Y' THEN 1 ELSE 0 END)) / CONVERT(DECIMAL(18,2), COUNT(D.LINENUM)) * 100 [Porcentaje], " &
                " STUFF(( " &
                " SELECT DISTINCT ', ' + UPPER(ALMDESTINO) FROM ArmadoMotos..DESPACHOS_DETALLE_MOTOS  " &
                " WHERE DESPACHOID = H.DOCNUM AND TIPO = D.CANAL " &
                " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS [Almacenes], " &
                " STUFF(( " &
                " SELECT DISTINCT ', ' + UPPER(RUTA) FROM ArmadoMotos..DESPACHOS_DETALLE_MOTOS  " &
                " WHERE DESPACHOID = H.DOCNUM AND TIPO = D.CANAL " &
                " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS [Ruta] " &
                " FROM ArmadoMotos..VW_DESPACHOS AS H " &
                " INNER JOIN ARMADOMOTOS..VW_DESPACHOS_DETALLE AS D ON D.DOCNUM = H.DOCNUM " &
                " WHERE DOCSTATUS IN ('PICK_EN_PROCESO','SERIES_PICKEADAS') " &
                " GROUP BY H.DOCNUM, H.BASENUM, H.DOCDUEDATE, D.CANAL " &
                " ORDER BY H.DOCDUEDATE DESC"


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
    Protected Sub gridDespachosAbiertos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridDespachosAbiertos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim diasRestantesText As String = e.Row.Cells(5).Text
            Dim diasRestantes As Integer

            If Integer.TryParse(diasRestantesText, diasRestantes) Then
                Dim maxAbsValue As Integer = 7 ' Ajusta al rango real

                If diasRestantes < -maxAbsValue Then diasRestantes = -maxAbsValue
                If diasRestantes > maxAbsValue Then diasRestantes = maxAbsValue

                Dim normalized As Double = (diasRestantes + maxAbsValue) / (2 * maxAbsValue)

                Dim red As Integer = CInt(255 * (1 - normalized))
                Dim green As Integer = CInt(255 * normalized)
                Dim blue As Integer = 0

                ' Color de fondo
                e.Row.Cells(5).BackColor = Drawing.Color.FromArgb(red, green, blue)

                ' Estilo de fuente: más grande, negra con sombra blanca
                e.Row.Cells(5).Font.Size = FontUnit.XXLarge
                e.Row.Cells(5).ForeColor = Drawing.Color.White
                e.Row.Cells(5).Attributes.Add("style", "text-shadow: 1px 1px 2px black; font-weight: bold; text-align: center;")
            End If
        End If
    End Sub
    Private Sub BindGridDespachosAbiertosDetalleMotos()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = "select distinct t2.ID,t0.ID[DespachoID],T2.PLANID,T2.TIPO,t2.RUTA " &
                    " ,CONVERT(CHAR,t0.FECHAVENCE,103)[Fecha Despacho] " &
                    " ,datediff(day,getdate(),t0.FECHAVENCE)[Dias Restantes] " &
                    " ,t2.ALMDESTINO,t2.MODELO,t2.ALMDESTINO,t2.ARTICULO,t2.DESCRIPCION,t2.CANTIDAD,t2.SERIEASIGNADA, t2.OBSERVACIONES " &
                    " from [DEPACHOS_HEADER] T0 WITH(NOLOCK) INNER JOIN [DESPACHOS_DETALLE_MOTOS] T2 WITH(NOLOCK) ON T0.ID=T2.HEADERID " &
                    " where T0.ESTADO='ABIERTO' and t2.CODIGOESTADO='DESPACHO ABIERTO'"
                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridDespachosAbiertosMotos.DataSource = dt
                            gridDespachosAbiertosMotos.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridDespachosAbiertosMotos.UseAccessibleHeader = True
            gridDespachosAbiertosMotos.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDespachosAbiertosDetalleMotos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub gridDespachosAbiertosMotos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDespachosAbiertosMotos.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim gvRow As GridViewRow = gridDespachosAbiertosMotos.Rows(index)

            If Not String.IsNullOrEmpty(gvRow.Cells(13).Text) Then
                Dim sb As New StringBuilder()
                sb.AppendLine("<script>")
                sb.AppendLine("Swal.fire({")
                sb.AppendLine("  icon: 'error',")
                sb.AppendLine("  title: 'No se puede cerrar la linea seleccionada',")
                sb.AppendLine("  text: 'Ya hay series asignadas. Proceda Via Devolucion.',")
                sb.AppendLine("  confirmButtonText: 'Entendido'")
                sb.AppendLine("});")
                sb.AppendLine("</script>")

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertSeriesAsignadas", sb.ToString(), False)
                Exit Sub
            Else
                Dim rowid As Integer = gvRow.Cells(1).Text
                Dim idPlan As Integer = gvRow.Cells(3).Text
                Dim despachoID As Integer = gvRow.Cells(2).Text
                Dim usuario As String = Context.Session("User").ToString()
                Dim almdestino As String = gvRow.Cells(8).Text
                Dim articulo As String = gvRow.Cells(11).Text
                Try
                    Using conn As New SqlConnection(sCon2)
                        conn.Open()

                        Dim sql As String = "UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO = 'CERRADO' WHERE id = @id;" &
                                    "DELETE FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE PLANID = @id AND HEADERID = @despachoID and ALMDESTINO = @almdestino and ARTICULO = @articulo;"

                        Using cmd As New SqlCommand(sql, conn)
                            cmd.Parameters.AddWithValue("@id", rowid)
                            cmd.Parameters.AddWithValue("@usuario", usuario)
                            cmd.Parameters.AddWithValue("@id", idPlan)
                            cmd.Parameters.AddWithValue("@despachoID", despachoID)
                            cmd.Parameters.AddWithValue("@almdestino", almdestino)
                            cmd.Parameters.AddWithValue("@articulo", articulo)

                            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                            Dim sb As New StringBuilder()
                            sb.AppendLine("<script>")
                            If rowsAffected > 0 Then
                                sb.AppendLine("Swal.fire({")
                                sb.AppendLine("  icon: 'success',")
                                sb.AppendLine("  title: 'Despacho cerrado correctamente',")
                                sb.AppendLine("  confirmButtonText: 'Aceptar'")
                                sb.AppendLine("}).then(() => { window.location.href = window.location.href; });")
                            Else
                                sb.AppendLine("Swal.fire({")
                                sb.AppendLine("  icon: 'warning',")
                                sb.AppendLine("  title: 'No se encontró la planificación',")
                                sb.AppendLine("  confirmButtonText: 'Aceptar'")
                                sb.AppendLine("});")
                            End If
                            sb.AppendLine("</script>")

                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "resultadoCierre", sb.ToString(), False)
                        End Using
                    End Using
                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("<script>")
                    sb.AppendLine("Swal.fire({")
                    sb.AppendLine("  icon: 'error',")
                    sb.AppendLine("  title: 'Error al cerrar el despacho',")
                    sb.AppendLine("  text: '" & ex.Message.Replace("'", "\'") & "',")
                    sb.AppendLine("  confirmButtonText: 'Aceptar'")
                    sb.AppendLine("});")
                    sb.AppendLine("</script>")

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorCierre", sb.ToString(), False)
                End Try
                'EliminarLinea(gridDespachosAbiertosMotos.Rows(index).Cells(6).Text,
                '              gridDespachosAbiertosMotos.Rows(index).Cells(10).Text,
                '              gridDespachosAbiertosMotos.Rows(index).Cells(2).Text)

                'Dim script As String = "<script>iziToast.warning({title: 'OK!', message: 'Moto Eliminada Con Exito!!!',position: 'topRight',timeout: 10000})</script>"
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                'BindGridDespachosAbiertos()
                'BindGridDespachosAbiertosDetalleMotos()


            End If
        End If
    End Sub
    Private Sub BindGridCamionesAbiertos()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = "select ID, isnull((select  top 1 DESPACHOID from CARGA_CAMION_DETALLE where IDCAMION=[CARGA_CAMION_HEADER].[ID]),0)[DespachoID], " &
                    " MOTORISTA, FECHASALIDA, ESTADO, FECHACREACION, USUARIO from CARGA_CAMION_HEADER" &
                    " order by id desc"

                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCamiones.DataSource = dt
                            gridCamiones.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridCamiones.UseAccessibleHeader = True
            gridCamiones.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridCamionesAbiertos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGridCamionesAbiertosDetalle()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = "select ID,ALMDESTINO,ARTICULO, DESCRIPCION, CANTIDAD,TIPO, " &
                " ESPACIOS,OBSERVACIONES,PLANID,DESPACHOID,SERIEASIGNADA,ESCANEADA,FECHAESCANEO from CARGA_CAMION_DETALLE " &
                " WHERE CODIGOESTADO='ABIERTO'"

                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCamionesDetalle.DataSource = dt
                            gridCamionesDetalle.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridCamionesDetalle.UseAccessibleHeader = True
            gridCamionesDetalle.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridCamionesAbiertosDetalle: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub EliminarLinea(almdestino As String, articulo As String, planid As String)
        Try
            Response.Write("<script>console.log('Variables: " & almdestino & "," & articulo & "," & planid & ",');</script>")

            Dim sCon As String = sCon2
            Dim sel As String
            sel = " delete from DESPACHOS_DETALLE_MOTOS WHERE ALMDESTINO = @almDestino AND ARTICULO = @Articulo AND PLANID = @planId;" &
                  " delete from  [MACROINSERT] WHERE ALMDESTINO = @almDestino AND ARTICULO = @Articulo AND PLANID = @planId;"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@almDestino", almdestino)
                cmd.Parameters.AddWithValue("@Articulo ", articulo)
                cmd.Parameters.AddWithValue("@planId", planid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using

            ' Script para cambiar de tab
            Dim tabScript As String = "document.querySelector('.ui.tabular.menu .item.active').classList.remove('active'); " &
                                     "document.querySelector('.ui.tabular.menu .item[data-tab=""second""]').classList.add('active'); " &
                                     "document.querySelector('.ui.tab.active').classList.remove('active'); " &
                                     "document.querySelector('.ui.tab[data-tab=""second""]').classList.add('active');"

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "TabChangeScript", tabScript, True)
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function EliminarCamion(camionid As String) As Boolean
        Try
            Dim return_Value As Boolean = False
            Dim sCon As String = sCon2
            Dim sel As String
            sel = " delete from CARGA_CAMION_HEADER WHERE ID = @idCamion;"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@idCamion", camionid)
                con.Open()
                cmd.ExecuteNonQuery()
                return_Value = True
                con.Close()
            End Using
            Return return_Value
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarCamion: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            Return False
        End Try
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

    Private Sub gridDespachosAbiertosMotos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridDespachosAbiertosMotos.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim diasRestantesText As String = e.Row.Cells(7).Text
                Dim diasRestantes As Integer

                If Integer.TryParse(diasRestantesText, diasRestantes) Then
                    Dim maxAbsValue As Integer = 7 ' Ajusta al rango real

                    If diasRestantes < -maxAbsValue Then diasRestantes = -maxAbsValue
                    If diasRestantes > maxAbsValue Then diasRestantes = maxAbsValue

                    Dim normalized As Double = (diasRestantes + maxAbsValue) / (2 * maxAbsValue)

                    Dim red As Integer = CInt(255 * (1 - normalized))
                    Dim green As Integer = CInt(255 * normalized)
                    Dim blue As Integer = 0

                    ' Color de fondo
                    e.Row.Cells(7).BackColor = Drawing.Color.FromArgb(red, green, blue)

                    ' Estilo de fuente: más grande, negra con sombra blanca
                    e.Row.Cells(7).Font.Size = FontUnit.XXLarge
                    e.Row.Cells(7).ForeColor = Drawing.Color.White
                    e.Row.Cells(7).Attributes.Add("style", "text-shadow: 1px 1px 2px black; font-weight: bold; text-align: center;")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub btnAgregarCamion_Click(sender As Object, e As EventArgs) Handles btnAgregarCamion.Click

    End Sub
    Private Sub btnCrearCamion_Click(sender As Object, e As EventArgs) Handles btnCrearCamion.Click
        Try
            'ModalPopupColoresMotos.Show()

            Dim scriptBuilder As New Text.StringBuilder()
            scriptBuilder.AppendLine("<script>")
            scriptBuilder.AppendLine("const arreglo = [];")

            For Each row As GridViewRow In gridDespachosAbiertos.Rows
                Dim cb As CheckBox = CType(row.FindControl("cbDocument"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked Then
                    Dim row_linea As String = row.Cells(3).Text.Trim()
                    Dim despachoId As String = row.Cells(2).Text.Trim()

                    scriptBuilder.AppendLine("if (!arreglo.some(e => e.linea === " & row_linea & " && e.despachoid === " & despachoId & ")) {")
                    scriptBuilder.AppendLine("    arreglo.push({ linea: " & row_linea & ", despachoid: " & despachoId & " });")
                    scriptBuilder.AppendLine("}")
                End If
            Next


            scriptBuilder.AppendLine("const json = btoa(JSON.stringify(arreglo));")
            scriptBuilder.AppendLine("window.location.href = 'TrasladosDespachosAbiertosConfirmacion.aspx?datos=' + json;")
            scriptBuilder.AppendLine("</script>")

            ClientScript.RegisterStartupScript(Me.GetType(), "redirectScript", scriptBuilder.ToString())


        Catch ex As Exception
            Response.Write("<script>console.log('btnCrearCamion_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub cargarPlacas()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "SELECT [ID],[PLACA] FROM [ArmadoMotos].[dbo].[TRANSPORTISTAS]"

                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using
            drpPlacas.Dispose()
            drpPlacas.DataTextField = "PLACA"
            drpPlacas.DataValueField = "ID"
            drpPlacas.DataSource = dt
            drpPlacas.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('CargarSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub cargarMotorista()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "SELECT [ID],[MOTORISTA] FROM [ArmadoMotos].[dbo].[TRANSPORTISTAS]"

                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using
            drpMotorista.Dispose()
            drpMotorista.DataTextField = "MOTORISTA"
            drpMotorista.DataValueField = "ID"
            drpMotorista.DataSource = dt
            drpMotorista.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('CargarSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub gridCamiones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCamiones.RowCommand
        Try
            If e.CommandName = "Ver" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCamiones.Rows(index)
                Dim url As String = "TrasladosDetallesCamion.aspx?idcamion=" & gvRow.Cells(3).Text
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openNewTab", "window.open('" & url & "', '_blank');", True)
            End If
            If e.CommandName = "Imprimir" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCamiones.Rows(index)
                Dim url As String = "DespachosCaratula.aspx?idcamion=" & gvRow.Cells(3).Text & "&motorista=" & gvRow.Cells(5).Text
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openNewTab", "window.open('" & url & "', '_blank');", True)
            End If

            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCamiones.Rows(index)
                Dim return_value As Boolean = EliminarCamion(gvRow.Cells(2).Text)

                If return_value = True Then
                    Dim script As String = "<script>iziToast.warning({title: 'OK!', message: 'Camión Eliminado Con Éxito!!!', position: 'topRight', timeout: 10000});</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                Else
                    Dim scriptError As String = "<script>iziToast.error({title: 'Error!', message: 'Ocurrió un Error, Revise Consola!!!', position: 'topRight', timeout: 10000});</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorScript", scriptError, False)
                End If
            End If

            ' Script para cambiar de tab
            Dim tabScript As String = "document.querySelector('.ui.tabular.menu .item.active').classList.remove('active'); " &
                                     "document.querySelector('.ui.tabular.menu .item[data-tab=""third""]').classList.add('active'); " &
                                     "document.querySelector('.ui.tab.active').classList.remove('active'); " &
                                     "document.querySelector('.ui.tab[data-tab=""third""]').classList.add('active');"

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "TabChangeScript", tabScript, True)

            ' Recargar datos
            BindGridDespachosAbiertos()
            BindGridDespachosAbiertosDetalleMotos()
            BindGridCamionesAbiertos()

        Catch ex As Exception
            Dim errorScript As String = "<script>console.log('gridCamiones_RowCommand: " & ex.Message.Replace("'", "\'") & "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorConsoleScript", errorScript, False)
        End Try
    End Sub

    Private Sub btnCamionCrear_Click(sender As Object, e As EventArgs) Handles btnCamionCrear.Click
        Try
            ' Validación de fecha
            If String.IsNullOrEmpty(txtFechaCamion.Text.Trim()) Then
                Dim errorScript As String = "<script>" &
                                          "Swal.fire({" &
                                          "    title: 'Error!', " &
                                          "    text: 'Debes agregar una fecha de salida del camión antes de continuar', " &
                                          "    icon: 'error', " &
                                          "    confirmButtonText: 'OK'" &
                                          "});" &
                                          "</script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "swal", errorScript)
                Exit Sub
            End If

            Dim estado As String = "ABIERTO"
            Dim fechaSalida As DateTime = CDate(txtFechaCamion.Text)
            Dim fechaCreacion As DateTime = DateTime.Now
            Dim usuario As String = If(Session("User") IsNot Nothing, Session("User").ToString(), "ANONIMO")
            Dim placa As String = drpPlacas.SelectedItem.Text
            Dim motorista As String = drpMotorista.SelectedItem.Text

            ' Insertar en CABECERA
            Dim queryHeader As String = "INSERT INTO [dbo].[CARGA_CAMION_HEADER]([FECHASALIDA],[ESTADO],[FECHACREACION],[USUARIO],[PLACA],[MOTORISTA]) " &
                                      "VALUES (@FECHASALIDA,@ESTADO,@FECHACREACION,@USUARIO,@PLACA,@MOTORISTA); " &
                                      "SELECT SCOPE_IDENTITY();"

            Dim nuevoId As Integer = 0

            Using connection As New SqlConnection(sCon2)
                ' Insertar cabecera
                Using command As New SqlCommand(queryHeader, connection)
                    command.Parameters.AddWithValue("@FECHASALIDA", fechaSalida)
                    command.Parameters.AddWithValue("@ESTADO", estado)
                    command.Parameters.AddWithValue("@FECHACREACION", fechaCreacion)
                    command.Parameters.AddWithValue("@USUARIO", usuario)
                    command.Parameters.AddWithValue("@PLACA", placa)
                    command.Parameters.AddWithValue("@MOTORISTA", motorista)

                    connection.Open()
                    nuevoId = Convert.ToInt32(command.ExecuteScalar())

                    ' Insertar detalles para cada fila seleccionada
                    For Each row As GridViewRow In gridDespachosAbiertos.Rows
                        Dim cb As CheckBox = CType(row.FindControl("cbDocument"), CheckBox)
                        If cb IsNot Nothing AndAlso cb.Checked Then
                            ' Obtener el PLANID (cuarta columna)
                            Dim row_linea As String = row.Cells(3).Text ' Índice 3 para la cuarta columna (0-based)
                            Dim despachoId As String = row.Cells(2).Text

                            Dim qry_insert As String = "IF NOT EXISTS ( " &
                            " SELECT 1 FROM CARGA_CAMION_DETALLE WHERE PLANID = @planid AND HEADERID = @despachoId)" &
                            " BEGIN" &
                            " INSERT INTO [dbo].[CARGA_CAMION_DETALLE]  (" &
                            " [IDCAMION],[RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],  " &
                            " [ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS],[CANTIDAD],  " &
                            " [QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[HEADERID],  " &
                            " [ESTADO],[TIPO],[SUPERVISOR],[FECHA],[USUARIO],[CB],  " &
                            " [FISICO],[FALTANTE],[VTAA],[FARMADO],[FDESPACHO],  " &
                            " [PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO],  " &
                            " [SERIEASIGNADA],[SERIESYS],[FECHASERIE],[PEDIDOID],[PEDIDOLINEA],[DESPACHOROWID]" &
                            " )" &
                            " SELECT " &
                            " @idcamion,[RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],  " &
                            " [ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS],[CANTIDAD],  " &
                            " [QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[HEADERID],  " &
                            " [ESTADO],[TIPO],[SUPERVISOR],[FECHA],[USUARIO],[CB]," &
                            " [FISICO],[FALTANTE],[VTAA],[FARMADO],[FDESPACHO],  " &
                            " [PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO],  " &
                            " [SERIEASIGNADA],[SERIESYS],[FECHASERIE],[PEDIDOID],[PEDIDOLINEA],[ID]" &
                            " FROM DESPACHOS_DETALLE_MOTOS " &
                            " WHERE PLANID = @planid AND HEADERID = @despachoId and DELETED='N';" &
                            " UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO = 'CAMION ABIERTO', CAMIONID = @idcamion WHERE DESPACHOID = @despachoId;" &
                            " UPDATE [ArmadoMotos].[dbo].[MACROINSERT] SET ESTADO='F' WHERE HEADERID = @despachoId  AND PLANID = @planid;" &
                            " END"

                            Using cmdDetalle As New SqlCommand(qry_insert, connection)
                                cmdDetalle.Parameters.AddWithValue("@idcamion", nuevoId)
                                cmdDetalle.Parameters.AddWithValue("@planid", row_linea)
                                cmdDetalle.Parameters.AddWithValue("@despachoId", despachoId)
                                cmdDetalle.ExecuteNonQuery()
                            End Using
                        End If
                    Next

                    ' Mensaje de éxito
                    Dim successScript As String = "<script>" &
                           "Swal.fire({" &
                           "    title: 'Éxito!', " &
                           "    text: 'Carga de camión creada exitosamente. ID: " & nuevoId & "', " &
                           "    icon: 'success', " &
                           "    confirmButtonText: 'OK'" &
                           "}).then(() => {" &
                           "    window.location.href = 'DetalleCargaCamion.aspx?idcamion=" & nuevoId & "&motorista=" & motorista & "';" &
                           "});" &
                           "</script>"
                    ClientScript.RegisterStartupScript(Me.GetType(), "swal", successScript)
                    connection.Close()
                End Using
            End Using

        Catch ex As Exception
            Dim errorScript As String = "<script>" &
                                      "Swal.fire({" &
                                      "    title: 'Error!', " &
                                      "    text: '" & ReplaceCharsForFileName(ex.Message, " ") & "', " &
                                      "    icon: 'error', " &
                                      "    confirmButtonText: 'OK'" &
                                      "});" &
                                      "console.log('btnCrearCamion_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');" &
                                      "</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "swal", errorScript)
        End Try
    End Sub

    Private Sub gridDespachosAbiertos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDespachosAbiertos.RowCommand
        If e.CommandName = "Cerrar" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim gvRow As GridViewRow = gridDespachosAbiertos.Rows(index)

            If CInt(gvRow.Cells(13).Text) > 0 Then
                Dim sb As New StringBuilder()
                sb.AppendLine("<script>")
                sb.AppendLine("Swal.fire({")
                sb.AppendLine("  icon: 'error',")
                sb.AppendLine("  title: 'No se puede cerrar el despacho general',")
                sb.AppendLine("  text: 'Ya hay series asignadas. Vaya a la sección Detalle.',")
                sb.AppendLine("  confirmButtonText: 'Entendido'")
                sb.AppendLine("});")
                sb.AppendLine("</script>")

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertSeriesAsignadas", sb.ToString(), False)
                Exit Sub
            Else
                Dim idPlan As Integer = gvRow.Cells(4).Text
                Dim despachoID As Integer = gvRow.Cells(3).Text
                Dim usuario As String = Context.Session("User").ToString()

                Try
                    Using conn As New SqlConnection(sCon2)
                        conn.Open()

                        Dim sql As String = "UPDATE [ArmadoMotos].[dbo].[DEPACHOS_HEADER] SET [ESTADO] = 'CERRADO', [USUARIOMOFICACION] = @usuario, [FECHAMODIFICACION] = GETDATE() WHERE PLANID = @id AND ID = @despachoID;" &
                                "UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO = 'CERRADO' WHERE PLANID = @id AND DESPACHOID = @despachoID;" &
                                "DELETE FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE PLANID = @id AND HEADERID = @despachoID;" &
                                "UPDATE MOVESAWEB..TBL_REPORTES_VENTAS_DISTRIBUIDORES_DETALLE SET DISPATCH_ID = NULL, DISPATCH_DATE = NULL WHERE DISPATCH_ID = @despachoID;"

                        Using cmd As New SqlCommand(sql, conn)
                            cmd.Parameters.AddWithValue("@usuario", usuario)
                            cmd.Parameters.AddWithValue("@id", idPlan)
                            cmd.Parameters.AddWithValue("@despachoID", despachoID)

                            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                            Dim sb As New StringBuilder()
                            sb.AppendLine("<script>")
                            If rowsAffected > 0 Then
                                sb.AppendLine("Swal.fire({")
                                sb.AppendLine("  icon: 'success',")
                                sb.AppendLine("  title: 'Despacho cerrado correctamente',")
                                sb.AppendLine("  confirmButtonText: 'Aceptar'")
                                sb.AppendLine("}).then(() => { window.location.href = window.location.href; });")
                            Else
                                sb.AppendLine("Swal.fire({")
                                sb.AppendLine("  icon: 'warning',")
                                sb.AppendLine("  title: 'No se encontró la planificación',")
                                sb.AppendLine("  confirmButtonText: 'Aceptar'")
                                sb.AppendLine("});")
                            End If
                            sb.AppendLine("</script>")

                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "resultadoCierre", sb.ToString(), False)
                        End Using
                    End Using
                Catch ex As Exception
                    Dim sb As New StringBuilder()
                    sb.AppendLine("<script>")
                    sb.AppendLine("Swal.fire({")
                    sb.AppendLine("  icon: 'error',")
                    sb.AppendLine("  title: 'Error al cerrar el despacho',")
                    sb.AppendLine("  text: '" & ex.Message.Replace("'", "\'") & "',")
                    sb.AppendLine("  confirmButtonText: 'Aceptar'")
                    sb.AppendLine("});")
                    sb.AppendLine("</script>")

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorCierre", sb.ToString(), False)
                End Try
            End If
        End If
    End Sub
    'Private Sub gridCamiones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCamiones.RowCommand
    '    Try
    '        If e.CommandName = "Eliminar" Then
    '            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '            Dim gvRow As GridViewRow = gridCamiones.Rows(index)
    '            Dim return_alue As Boolean = EliminarCamion(gvRow.Cells(2).Text)

    '            If return_alue = True Then
    '                Dim script As String = "<script>iziToast.warning({title: 'OK!', message: 'Camion Eliminado Con Exito!!!',position: 'topRight',timeout: 10000})</script>"
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
    '            Else
    '                Dim scriptError As String = "<script>iziToast.error({title: 'Error!', message: 'Ocurrio un Error, Revise Consola!!!',position: 'topRight',timeout: 10000})</script>"
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", scriptError, False)
    '            End If

    '        End If

    '        Dim tabScript As String = " document.querySelector('.ui.tabular.menu .item.active').classList.remove('active'); " &
    '        " document.querySelector('.ui.tabular.menu .item[data-tab="third"]').classList.add('active'); " &
    '        " document.querySelector('.ui.tab.active').classList.remove('active'); " &
    '        " document.querySelector('.ui.tab[data-tab="third"]').classList.add('active'); "
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", tabScript, False)

    '        BindGridDespachosAbiertos()
    '        BindGridDespachosAbiertosDetalleMotos()
    '        BindGridCamionesAbiertos()
    '    Catch ex As Exception
    '        Response.Write("<script>console.log('gridCamiones_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try

    'End Sub
End Class

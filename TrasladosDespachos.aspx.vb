Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDespachos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public almacen As String

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerEventos() As Object
        Dim eventos As New List(Of Object)
        Dim connStr As String = sCon2

        Using con As New SqlConnection(connStr)
            Dim query As String = "SELECT t0.ID,t0.PLANID,t0.FECHAVENCE,t1.ruta,t1.almdestino, " &
                " (select whsname from movesa..owhs where whscode= T1.ALMDESTINO collate Modern_Spanish_CI_AS) [Nalmacen]" &
                " ,SUM(t1.ESPACIOS) AS TotalEspacios, " &
                " SUM(t1.CANTIDAD) AS TotalCantidad " &
                " FROM [ArmadoMotos].[dbo].[DEPACHOS_HEADER] t0 WITH(NOLOCK) " &
                " INNER JOIN [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] t1 WITH(NOLOCK) " &
                " ON t0.ID = t1.HEADERID GROUP BY " &
                " t0.ID, t0.PLANID, t0.FECHAVENCE, t1.ruta, t1.almdestino"

            Using cmd As New SqlCommand(query, con)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        eventos.Add(New With {
                        .title = "" & reader("ruta") & " - " & reader("almdestino") & "  " & reader("Nalmacen") &
                                 " (" & reader("TotalCantidad") & " unidades, " & reader("TotalEspacios") & " espacios ) , Planid: " & reader("PLANID") & " ",
                        .start = Convert.ToDateTime(reader("FECHAVENCE")).ToString("yyyy-MM-dd"),
                        .allDay = True,
                        .planid = reader("PLANID")
                    })
                    End While
                End Using
            End Using
        End Using

        Return eventos
    End Function

    Private Sub TrasladosDespachos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGridDespachosAbiertos()
                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
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

                'SQL_STRING = "select T0.ID [DespachoId] " &
                '    " ,T0.PLANID [PlanId],CONVERT(CHAR,t0.FECHAVENCE,103)[FechaDespacho],datediff(day,getdate(),t0.FECHAVENCE)[DiasRestantes] " &
                '    " ,T1.TIPO [Canal],count(distinct almdestino) [TotalAlm],sum(faltante)[TotalFalt],sum(CANTIDAD)[Unds],sum(t1.ESPACIOS)[TotalEspacios] " &
                '    " ,count(SERIEASIGNADA)[Preparado],(convert(decimal(18,2),count(SERIEASIGNADA))/convert(decimal(18,2),sum(CANTIDAD)))*100 [Porcentaje] " &
                '    " ,(SELECT STUFF((SELECT DISTINCT ', ' + [almdestino] FROM [DESPACHOS_DETALLE_MOTOS] WHERE HEADERID=t0.ID and tipo=t1.tipo FOR XML PATH('')), 1, 2, '') )AS [Almacenes] " &
                '    " ,(SELECT STUFF((SELECT DISTINCT ', ' + [RUTA] FROM [DESPACHOS_DETALLE_MOTOS] WHERE HEADERID=t0.ID and tipo=t1.tipo FOR XML PATH('')), 1, 2, '') )AS [Ruta] " &
                '    " from [DEPACHOS_HEADER] T0 WITH(NOLOCK) INNER JOIN [DESPACHOS_DETALLE_MOTOS] T1 WITH(NOLOCK) ON T0.ID=T1.HEADERID " &
                '    " WHERE T0.ESTADO='ABIERTO' and t1.CODIGOESTADO='DESPACHO ABIERTO' " &
                '    " group by T0.ID ,T0.PLANID ,CONVERT(CHAR,t0.FECHAVENCE,103),datediff(day,getdate(),t0.FECHAVENCE),T1.TIPO"

                SQL_STRING = "SELECT H.DOCNUM [DespachoId], H.BASENUM [PlanId], FORMAT(H.DOCDUEDATE,'yyyy/MM/dd') [FechaDespacho], " &
                    " DATEDIFF(DAY, GETDATE(),H.DOCDUEDATE) [DiasRestantes], " &
                    " D.CANAL, COUNT(DISTINCT D.FROM_WHSCODE) [TotalAlm], 0 [TotalFalt], COUNT(D.LINENUM) [Unds], SUM(D.SLOT) [TotalEspacios], " &
                    " SUM(CASE WHEN D.PICKED = 'Y' THEN 1 ELSE 0 END) [Preparado],  " &
                    " SUM(CASE WHEN D.IN_TRUCK = 'Y' THEN 1 ELSE 0 END)[Cargado], " &
                    " CONVERT(DECIMAL(18,2), SUM(CASE WHEN D.PICKED = 'Y' THEN 1 ELSE 0 END)) / CONVERT(DECIMAL(18,2),COUNT(D.LINENUM)) * 100 [PorcentajePick], " &
                    " CONVERT(DECIMAL(18,2), SUM(CASE WHEN D.IN_TRUCK = 'Y' THEN 1 ELSE 0 END)) / CONVERT(DECIMAL(18,2),COUNT(D.LINENUM)) * 100 [PorcentajeCarga], " &
                    " STUFF(( " &
                    " SELECT DISTINCT ', ' + TO_WHSCODE FROM ArmadoMotos..VW_DESPACHOS_DETALLE WHERE DOCNUM = H.DOCNUM " &
                    " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') [Almacenes], " &
                    " STUFF(( " &
                    " SELECT DISTINCT ', ' + ROAD FROM ArmadoMotos..VW_DESPACHOS_DETALLE WHERE DOCNUM = H.DOCNUM " &
                    " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') [Ruta] " &
                    " FROM ARMADOMOTOS..VW_DESPACHOS AS H " &
                    " INNER JOIN VW_DESPACHOS_DETALLE AS D ON D.DOCNUM = H.DOCNUM " &
                    " WHERE H.DOCSTATUS IN ('PICK_EN_PROCESO','SERIES_PICKEADAS') " &
                    " GROUP BY H.DOCNUM, H.BASENUM, H.DOCDUEDATE, D.CANAL " &
                    " ORDER BY H.DOCDUEDATE"

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
            Dim diasRestantesText As String = e.Row.Cells(6).Text
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
                e.Row.Cells(6).BackColor = Drawing.Color.FromArgb(red, green, blue)

                ' Estilo de fuente: más grande, negra con sombra blanca
                e.Row.Cells(6).Font.Size = FontUnit.XXLarge
                e.Row.Cells(6).ForeColor = Drawing.Color.White
                e.Row.Cells(6).Attributes.Add("style", "text-shadow: 1px 1px 2px black; font-weight: bold; text-align: center;")
            End If
        End If
    End Sub
    Private Sub BindGridgridAlmacenesDespacho(plan_Id As String, despachoid As String, ruta As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                'SQL_STRING = "SELECT [ID],[RUTA],[ALMDESTINO] " &
                '    ",(SELECT WhsName FROM MOVESA..OWHS WHERE WHSCODE=[ALMDESTINO] COLLATE Modern_Spanish_CI_AS)[Nalmacen]" &
                '    " ,[CARDCODE],[ARTICULO],[MODELO] " &
                '    " ,[DESCRIPCION],[ESPACIOS],[CANTIDAD],[OBSERVACIONES]" &
                '    " ,[USUARIO],[PLANID],[DESPACHOID]" &
                '    " ,[SERIEASIGNADA]" &
                '    " FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] where ESTADO='P' " &
                '    " and PLANID = @p1 And despachoid = @p2 And CODIGOESTADO='DESPACHO ABIERTO'" &
                '    " and RUTA= @ruta"

                'SQL_STRING = "SELECT [ID],[RUTA],[ALMDESTINO] " &
                '    ",(SELECT WhsName FROM MOVESA..OWHS WHERE WHSCODE=[ALMDESTINO] COLLATE Modern_Spanish_CI_AS)[Nalmacen]" &
                '    " ,[CARDCODE],[ARTICULO],[MODELO] " &
                '    " ,[DESCRIPCION],[ESPACIOS],[CANTIDAD],[OBSERVACIONES]" &
                '    " ,[USUARIO],[PLANID],[DESPACHOID]" &
                '    " ,isnull([SERIEASIGNADA], 'N/A') [SERIEASIGNADA] " &
                '    " FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] where ESTADO='P' " &
                '    " And despachoid = @p2 And CODIGOESTADO='DESPACHO ABIERTO'"

                SQL_STRING = "SELECT T0.LINENUM [ID], T0.ROAD [RUTA], T0.TO_WHSCODE [ALMDESTINO], T0.TO_WHSNAME [Nalmacen], " &
                    " T0.TO_CARDCDE [CARDCODE], T0.ITEMCODE [ARTICULO], T0.MODEL_NAME [MODELO], T0.ITEMNAME [DESCRIPCION], " &
                    " T0.SLOT [ESPACIOS], 1 [CANTIDAD], T0.LINETYPE [OBSERVACIONES], T0.CREATED_BY [USUARIO],  " &
                    " T0.BASE_DOCNUM [PLANID], DOCNUM [DESPACHOID], ISNULL(SERIAL_NUMBER, 'N/A') [SERIEASIGNADA] " &
                    " FROM ARMADOMOTOS..VW_DESPACHOS_DETALLE AS T0 WHERE DOCNUM = @p2 and IN_TRUCK ='N'"


                Using cmd As New SqlCommand(SQL_STRING)
                    'cmd.Parameters.AddWithValue("@p1", plan_Id)
                    cmd.Parameters.AddWithValue("@p2", despachoid)
                    'cmd.Parameters.AddWithValue("@ruta", ruta)
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

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridgridAlmacenesDespacho: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub gridIndiceD_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Agregar atributo data-merge a las celdas que deben fusionarse
            e.Row.Cells(0).Attributes.Add("data-merge", "true") ' Columna 1: Ruta
            e.Row.Cells(1).Attributes.Add("data-merge", "true") ' Columna 2: Canal
            e.Row.Cells(2).Attributes.Add("data-merge", "true") ' Columna 3: Range Ranking
            e.Row.Cells(3).Attributes.Add("data-merge", "true") ' Columna 4: Range Desabasto
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
    Public Sub ActualizarDocumento(lineId As String, whscode As String, despachoheader As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String = ""
            sel = "UPDATE  [PLANIFICACIONES_DETALLE] set [ESTADOLINEA]='DESPACHO PENDIENTE', [DESPACHOHEADER] = @p3 WHERE [HEADERID] = @p1 and [CODIGO]= @p2 "
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", lineId)
                cmd.Parameters.AddWithValue("@p2", whscode)
                cmd.Parameters.AddWithValue("@p3", despachoheader)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('ActualizarDocumento: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function CrearNuevoDespacho(fechacreacion As DateTime, estado As String, fechainicio As DateTime, fechavence As DateTime,
                                       usuario As String) As Integer
        Try
            Dim sCon As String = sCon2
            Dim sel As String = ""
            sel = "INSERT INTO [dbo].[DEPACHOS_HEADER] ([FECHACREACION]" &
                                  " ,[ESTADO],[FECHAINICIO],[FECHAVENCE],[USUARIO])" &
                                  " VALUES (@p1,@p2,@p3,@p4,@p5)" &
                                  " SELECT SCOPE_IDENTITY();"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", fechacreacion)
                cmd.Parameters.AddWithValue("@p2", estado)
                cmd.Parameters.AddWithValue("@p3", fechainicio)
                cmd.Parameters.AddWithValue("@p4", fechavence)
                cmd.Parameters.AddWithValue("@p5", usuario)
                con.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        Catch ex As Exception
            ' Retorna 0 si ocurre un error
            Return 0
        End Try
    End Function

    Private Sub gridDespachosAbiertos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDespachosAbiertos.RowCommand
        Try
            If e.CommandName = "Ver" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridDespachosAbiertos.Rows(index)
                lblPlanId.Text = gvRow.Cells(4).Text
                lblCurrentDespacho.Text = gvRow.Cells(3).Text
                lblIdPlan.Text = gvRow.Cells(4).Text
                lblRuta.Text = gvRow.Cells(8).Text
                BindGridgridAlmacenesDespacho(gridDespachosAbiertos.Rows(index).Cells(5).Text,
                                              gridDespachosAbiertos.Rows(index).Cells(4).Text,
                                              gridDespachosAbiertos.Rows(index).Cells(9).Text)
            End If

            If e.CommandName = "Cerrar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridDespachosAbiertos.Rows(index)

                If CInt(gvRow.Cells(14).Text) > 0 Then
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

                            'Dim sql As String = "UPDATE [ArmadoMotos].[dbo].[DEPACHOS_HEADER] SET [ESTADO] = 'CERRADO', [USUARIOMOFICACION] = @usuario, [FECHAMODIFICACION] = GETDATE() WHERE PLANID = @id AND ID = @despachoID;" &
                            '        "UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO = 'CERRADO' WHERE PLANID = @id AND DESPACHOID = @despachoID;" &
                            '        "DELETE FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE PLANID = @id AND HEADERID = @despachoID;" &
                            '        "UPDATE MOVESAWEB..TBL_REPORTES_VENTAS_DISTRIBUIDORES_DETALLE SET DISPATCH_ID = NULL, DISPATCH_DATE = NULL WHERE DISPATCH_ID = @despachoID;"

                            Dim sql As String = "EXEC ARMADOMOTOS..SP_PORTAL_DESPACHOS @FN = 'CANCELAR DESPACHO', @DOCNUM = @despachoID;"

                            Using cmd As New SqlCommand(sql, conn)
                                'cmd.Parameters.AddWithValue("@usuario", usuario)
                                'cmd.Parameters.AddWithValue("@id", idPlan)
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
        Catch ex As Exception
            Response.Write("<script>console.log('gridDespachos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function ExisteDespacho(codigo As String, planId As String) As Boolean
        Dim existe As Boolean = False

        Try
            Dim sCon As String = sCon2
            Using con As New SqlConnection(sCon)
                Dim query As String = "select count(*) from [DESPACHOS_DETALLE_MOTOS] where planid = @p1 and ALMDESTINO = @p2"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@p1", planId)
                    cmd.Parameters.AddWithValue("@p2", codigo)

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
    Private Sub btnVistaCalendario_Click(sender As Object, e As EventArgs) Handles btnVistaCalendario.Click
        Try
            Response.Redirect("TrasladosDespachosCalendario.aspx")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub gridAlmacenesDespachos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridAlmacenesDespachos.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridAlmacenesDespachos.Rows(index)

                Response.Write("<script>console.log('Serie: " & gvRow.Cells(15).Text & "');</script>")

                If gvRow.Cells(15).Text <> "N/A" Then
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
                    Dim idPlan As Integer = lblIdPlan.Text
                    Dim despachoID As Integer = lblCurrentDespacho.Text
                    Dim usuario As String = Context.Session("User").ToString()
                    Dim almdestino As String = gvRow.Cells(3).Text
                    Dim articulo As String = gvRow.Cells(6).Text
                    Try
                        Using conn As New SqlConnection(sCon2)
                            conn.Open()

                            'Dim sql As String = "UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO = 'CERRADO' WHERE id = @id;" &
                            '        "DELETE FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE PLANID = @idplan AND HEADERID = @despachoID and ALMDESTINO = @almdestino and ARTICULO = @articulo;"
                            Dim sql As String = "EXEC ARMADOMOTOS..SP_PORTAL_DESPACHOS @FN = 'ELIMINAR LINEA DESPACHO', @LINENUM = @id;"

                            Using cmd As New SqlCommand(sql, conn)
                                cmd.Parameters.AddWithValue("@id", rowid)
                                'cmd.Parameters.AddWithValue("@usuario", usuario)
                                'cmd.Parameters.AddWithValue("@idplan", idPlan)
                                'cmd.Parameters.AddWithValue("@despachoID", despachoID)
                                'cmd.Parameters.AddWithValue("@almdestino", almdestino)
                                'cmd.Parameters.AddWithValue("@articulo", articulo)

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

                'EliminarLinea(gvRow.Cells(2).Text)

                'BindGridgridAlmacenesDespacho(lblIdPlan.Text, lblCurrentDespacho.Text, lblRuta.Text)
                ''Response.Redirect("TrasladosDespachos.aspx")

                ''Dim script As String = "<script>Swal.fire({title: 'Éxito!', text: 'Linea Eliminada Exitosamente!', icon: 'success', confirmButtonText: 'OK'});</script>"
                ''ClientScript.RegisterStartupScript(Me.GetType(), "swal", script)

            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPlanificacionTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    'Public Sub EliminarLinea(idlinea As Integer)
    '    Try
    '        Dim sCon As String = sCon2
    '        Dim sel As String
    '        sel = "update DESPACHOS_DETALLE_MOTOS set CODIGOESTADO='CERRADO' where [ID] = @p1"
    '        Using con As New SqlConnection(sCon)
    '            Dim cmd As New SqlCommand(sel, con)
    '            cmd.Parameters.AddWithValue("@p1", idlinea)
    '            con.Open()
    '            cmd.ExecuteNonQuery()
    '            con.Close()
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("EliminarLinea " & ex.Message)
    '    End Try
    'End Sub
    'Public Function CrearNuevoDespacho(fechacreacion As DateTime, estado As String, fechainicio As DateTime, fechavence As DateTime,
    '                                   usuario As String) As Integer
    '    Dim sCon As String = sCon2
    '    Dim sel As String = ""
    '    sel = " INSERT INTO [dbo].[DEPACHOS_HEADER] ([FECHACREACION]" &
    '                            " ,[ESTADO],[FECHAINICIO],[FECHAVENCE],[USUARIO])" &
    '                            " VALUES (@p1,@p2,@p3,@p4,@p5)" &
    '                            " SELECT SCOPE_IDENTITY();"
    '    Using con As New SqlConnection(sCon)
    '        Dim cmd As New SqlCommand(sel, con)
    '        cmd.Parameters.AddWithValue("@p1", fechacreacion)
    '        cmd.Parameters.AddWithValue("@p2", estado)
    '        cmd.Parameters.AddWithValue("@p3", fechainicio)
    '        cmd.Parameters.AddWithValue("@p4", fechavence)
    '        cmd.Parameters.AddWithValue("@p5", usuario)
    '        con.Open()
    '        Return cmd.ExecuteScalar()
    '        con.Close()
    '    End Using
    'End Function
End Class

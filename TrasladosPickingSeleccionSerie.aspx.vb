Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosPickingSeleccionSerie
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public almacen As String
    Private Sub TrasladosPickingSeleccionSerie(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    bindGridSeriesDisponibles(Request.QueryString("itemcode"))

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

    Private Sub bindGridSeriesDisponibles(itemcode As String)
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = "select datediff(day,PrdDate,getdate())[Dias],SuppSerial[Serie],IntrSerial[Motor],ItemCode[Codigo] " &
                            " ,(select itemname from oitm with(nolock) where itemcode=osri.itemcode)[Descripcion] " &
                            " ,BatchId[Año],WhsCode[Almacen] " &
                            ",SysSerial[SerieSys] from osri where itemcode = @itemcode " &
                            " And status = 0 And whscode='DCM00' order by 1 desc"

                Using cmd As New SqlCommand(SQL_STRING)
                    cmd.Parameters.AddWithValue("@itemcode", itemcode)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridSeriesDisponibles.DataSource = dt
                            gridSeriesDisponibles.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridSeriesDisponibles.UseAccessibleHeader = True
            gridSeriesDisponibles.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGridDespachosAbiertosDetalleMotos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
                cmd.Parameters.AddWithValue("@almDestino ", almdestino)
                cmd.Parameters.AddWithValue("@Articulo ", articulo)
                cmd.Parameters.AddWithValue("@planId", planid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
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

    Private Sub gridSeriesDisponibles_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridSeriesDisponibles.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridSeriesDisponibles.Rows(index)

                SeleccionarSerie(gvRow.Cells(2).Text, DateTime.Now, Request.QueryString("id"), Request.QueryString("itemcode"), gvRow.Cells(8).Text)

                Dim script As String = "<script>" & vbCrLf &
                "iziToast.success({" & vbCrLf &
                "    title: 'OK!'," & vbCrLf &
                "    message: 'Serie Agregada con Éxito!!!'," & vbCrLf &
                "    position: 'topRight'," & vbCrLf &
                "    timeout: 2000," & vbCrLf &
                "    onClosed: function () {" & vbCrLf &
                "        window.location.href = 'TrasladosPiking.aspx';" & vbCrLf &
                "    }" & vbCrLf &
                "});" & vbCrLf &
                "</script>"

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridDespachosAbiertosMotos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub SeleccionarSerie(serie_asignada As String, fecha_asignacion As DateTime, row_id As String, itemcode As String, serie_sys As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String = "UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] " &
                                "SET [SERIEASIGNADA] = @p1, [FECHASERIE] = @p2, [SERIESYS] = @p5 " &
                                "WHERE id = @p3 AND ARTICULO = @p4"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", serie_asignada)
                cmd.Parameters.AddWithValue("@p2", fecha_asignacion)
                cmd.Parameters.AddWithValue("@p3", row_id)
                cmd.Parameters.AddWithValue("@p4", itemcode)
                cmd.Parameters.AddWithValue("@p5", serie_sys)
                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('Serie Insertada: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

End Class

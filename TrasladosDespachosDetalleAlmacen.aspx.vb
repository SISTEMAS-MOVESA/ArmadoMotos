Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDespachosDetalleAlmacen
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public almacen As String

    Private Sub TrasladosDespachosDetalleAlmacen_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGridDespachosAbiertos(Request.QueryString("whscode"))
                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindGridDespachosAbiertos(whscode As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String

                SQL_STRING = "select ID,RUTA,ALMDESTINO,ARTICULO,MODELO,DESCRIPCION,CANTIDAD, " &
                    " ESPACIOS,OBSERVACIONES,FECHA,PLANID,DESPACHOID,SERIEASIGNADA,FECHASERIE, " &
                    " USUARIO,CODIGOESTADO from ArmadoMotos..DESPACHOS_DETALLE_MOTOS  " &
                    " where ALMDESTINO = @whscode  AND CODIGOESTADO IN ('DESPACHO ABIERTO','CAMION ABIERTO')"

                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@whscode", whscode)
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
    Private Sub gridDespachosAbiertos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDespachosAbiertos.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim gvRow As GridViewRow = gridDespachosAbiertos.Rows(index)
            Dim rowid As Integer = gvRow.Cells(1).Text
            Try
                Using conn As New SqlConnection(sCon2)
                    conn.Open()
                    Dim sql As String = "update DESPACHOS_DETALLE_MOTOS set CODIGOESTADO='CERRADO', DELETED='Y' where [ID] = @id;"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@id", rowid)
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
End Class

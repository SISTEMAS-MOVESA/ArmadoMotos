Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosPiking
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public almacen As String
    Private Sub TrasladosPiking_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGridDespachosAbiertosDetalleMotos()

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
    Private Sub BindGridDespachosAbiertosDetalleMotos()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String
                SQL_STRING = "select distinct t2.ID,T2.PLANID " &
                    " ,t2.ALMDESTINO,t2.CODMODELO,t2.MODELO,t2.ARTICULO,t2.DESCRIPCION,t2.CANTIDAD   " &
                    " from [DESPACHOS_DETALLE_MOTOS] T2 WHERE [CODIGOESTADO]='ABIERTO' and [SERIEASIGNADA] is null"
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
        Try
            If e.CommandName = "Seleccionar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridDespachosAbiertosMotos.Rows(index)

                Response.Redirect("TrasladosPickingSeleccionSerie.aspx?id=" & gridDespachosAbiertosMotos.Rows(index).Cells(1).Text & "&whscode=" & gridDespachosAbiertosMotos.Rows(index).Cells(3).Text & "&itemcode=" & gridDespachosAbiertosMotos.Rows(index).Cells(6).Text & "")
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridDespachosAbiertosMotos_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
End Class

Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDDashboardDetalleConfirmacion
    Inherits System.Web.UI.Page

    Private Sub TrasladosDDashboardDetalleConfirmacion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                End If
                BindgridPortalPedidos()
                BindgridSugerido(Request.QueryString("planid"), Session("User"))
            End If
        Catch ex As Exception
            Response.Write("TrasladosDDashboardDetalleConfirmacion_Load " & ex.Message)
        End Try
    End Sub

    Private Sub BindgridSugerido(idPlan As String, usuario As String)
        Try
            Dim sql As String =
                "SELECT RUTA,ID,ALMDESTINO,ARTICULO,MODELO,DESCRIPCION,ESPACIOS,CANTIDAD,QTYLOGISTICA,OBSERVACIONES " &
                "FROM ArmadoMotos.dbo.MACROINSERT " &
                "WHERE [ESTADO]='B' AND PLANID=@planId AND USUARIO=@usuario"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@planId", idPlan),
                    New SqlParameter("@usuario", usuario)
                })
            gridPedidoTemp.DataSource = dt
            gridPedidoTemp.DataBind()
            gridPedidoTemp.UseAccessibleHeader = True
            If gridPedidoTemp.HeaderRow IsNot Nothing Then
                gridPedidoTemp.HeaderRow.TableSection = TableRowSection.TableHeader
            End If

            Dim unidades As Integer = 0
            Dim espacios As Integer = 0
            For Each row As GridViewRow In gridPedidoTemp.Rows
                espacios += CInt(row.Cells(6).Text)
                unidades += CInt(row.Cells(7).Text)
            Next
            lblEspacios.Text = "Espacios: " & espacios
            lblUnidades.Text = "Unidades: " & unidades
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub BindgridPortalPedidos()
        Try
            Dim sql As String = "EXEC MOVESA..SP_PORTAL_DISTRIBUIDORES 'PEDIDOS DISPONIBLE DESPACHO'"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
            gridMotosPortalPedidos.DataSource = dt
            gridMotosPortalPedidos.DataBind()
            gridMotosPortalPedidos.UseAccessibleHeader = True
            If gridMotosPortalPedidos.HeaderRow IsNot Nothing Then
                gridMotosPortalPedidos.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridPortalPedidos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Try
            Response.Redirect("TrasladosDDashboardDetalle.aspx?planid=" & Request.QueryString("planid"))
        Catch ex As Exception
            Response.Write("<script>console.log('btnRegresar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Try
            If txtFechaDespacho.Text = "" Then
                ClientScript.RegisterStartupScript(Me.GetType(), "alert",
                    "Swal.fire('Error','Se requiere una fecha de despacho.','error');", True)
                Return
            End If

            Dim planid As String = Request.QueryString("planid")
            Dim nuevoDespachoId As Integer = CrearNuevoDespacho(
                CInt(planid), DateTime.Now, "ABIERTO", DateTime.Now, txtFechaDespacho.Text, Session("User").ToString())

            If nuevoDespachoId > 0 Then
                actualizarDespachoDetalleMotos(nuevoDespachoId.ToString(), planid, Session("User").ToString())
                actualizarMacroInsert(nuevoDespachoId.ToString(), txtFechaDespacho.Text, planid)
                InsertarMotosPortalPedidos(nuevoDespachoId.ToString(), planid)
                ClientScript.RegisterStartupScript(Me.GetType(), "alert",
                    "Swal.fire({title:'Despacho Creado',html:'Despacho <strong>#" & nuevoDespachoId &
                    "</strong> creado correctamente.',icon:'success'}).then(function(){window.location.href='TrasladosDDashboard.aspx';});", True)
            Else
                ClientScript.RegisterStartupScript(Me.GetType(), "alert",
                    "Swal.fire('Error','No se pudo crear el despacho.','error');", True)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('btnContinuar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function CrearNuevoDespacho(planid As Integer, fechacreacion As DateTime, estado As String,
                                       fechainicio As DateTime, fechavence As DateTime, usuario As String) As Integer
        Try
            Dim sql As String =
                "INSERT INTO [dbo].[DEPACHOS_HEADER] ([PLANID],[FECHACREACION],[ESTADO],[FECHAINICIO],[FECHAVENCE],[USUARIO]) " &
                "VALUES (@p1,@p2,@p3,@p4,@p5,@p6); SELECT @@IDENTITY;"
            Dim result As Object = DbConfig.ExecuteScalar(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@p1", planid),
                    New SqlParameter("@p2", fechacreacion),
                    New SqlParameter("@p3", estado),
                    New SqlParameter("@p4", fechainicio),
                    New SqlParameter("@p5", fechavence),
                    New SqlParameter("@p6", usuario)
                })
            Return If(result IsNot Nothing, Convert.ToInt32(result), 0)
        Catch
            Return 0
        End Try
    End Function

    Public Sub InsertarMotosPortalPedidos(despachoId As String, planId As String)
        Try
            For Each row As GridViewRow In gridMotosPortalPedidos.Rows
                Dim cb As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked Then
                    Dim sql As String =
                        "EXEC MOVESA..SP_PORTAL_DISTRIBUIDORES " &
                        "@FUNCTION='INSERTAR LINEA PICKING',@PARAMETER_1=@p1,@PARAMETER_2=@p2," &
                        "@PARAMETER_3=@p3,@PARAMETER_4=@p4,@USUARIO=@usr"
                    DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                        New List(Of SqlParameter) From {
                            New SqlParameter("@p1", row.Cells(2).Text),
                            New SqlParameter("@p2", row.Cells(13).Text),
                            New SqlParameter("@p3", despachoId),
                            New SqlParameter("@p4", planId),
                            New SqlParameter("@usr", Session("User"))
                        })
                End If
            Next
        Catch ex As Exception
            Response.Write("<script>console.log('InsertarMotosPortalPedidos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub actualizarDespachoDetalleMotos(headerid As String, planid As String, usuario As String)
        Try
            Dim sql As String =
                "UPDATE [dbo].[DESPACHOS_DETALLE_MOTOS] " &
                "SET [HEADERID]=@h,[ESTADO]='P',[DESPACHOID]=@h,[CODIGOESTADO]='DESPACHO ABIERTO' " &
                "WHERE PLANID=@p AND [USUARIO]=@u AND [ESTADO]='B'"
            DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@h", headerid),
                    New SqlParameter("@p", planid),
                    New SqlParameter("@u", usuario)
                })
        Catch ex As Exception
            Response.Write("<script>console.log('actualizarDespachoDetalleMotos: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub actualizarMacroInsert(headerid As String, fdespacho As DateTime, planid As String)
        Try
            Dim sql As String =
                "UPDATE [dbo].[MACROINSERT] SET [HEADERID]=@h,[ESTADO]='T',[FDESPACHO]=@f WHERE PLANID=@p"
            DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@h", headerid),
                    New SqlParameter("@f", fdespacho),
                    New SqlParameter("@p", planid)
                })
        Catch ex As Exception
            Response.Write("<script>console.log('actualizarMacroInsert: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridPedidoTemp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPedidoTemp.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim idx As Integer = Convert.ToInt32(e.CommandArgument)
                EliminarLinea(gridPedidoTemp.Rows(idx).Cells(2).Text,
                              gridPedidoTemp.Rows(idx).Cells(4).Text,
                              Request.QueryString("planid"))
                BindgridPortalPedidos()
                BindgridSugerido(Request.QueryString("planid"), Session("User"))
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPedidoTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub EliminarLinea(idlinea As String, itemcode As String, planid As String)
        Try
            Dim sql As String =
                "DELETE FROM [MACROINSERT] WHERE [ID]=@p1; " &
                "DELETE FROM [DESPACHOS_DETALLE_MOTOS] WHERE PLANID=@planid AND [ARTICULO]=@itemcode"
            DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {
                    New SqlParameter("@p1", idlinea),
                    New SqlParameter("@itemcode", itemcode),
                    New SqlParameter("@planid", planid)
                })
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function GetEmailSuc(whscode As String) As String
        Try
            Dim sql As String = "SELECT [USEREMAIL] FROM [dbo].[USUARIOS] WHERE SUCURSAL=@p1"
            Dim result As Object = DbConfig.ExecuteScalar(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@p1", whscode)})
            Return If(result IsNot Nothing, result.ToString(), "")
        Catch
            Return ""
        End Try
    End Function

    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
        Dim chars() As String = {"/", "\", ":", "?", Chr(34), "<", ">", "|", "&", "%", "*", "'", "{", "[", "]", "}", "!", ","}
        For Each c As String In chars
            sName = Replace(sName, c, sChr)
        Next
        Return sName
    End Function

End Class

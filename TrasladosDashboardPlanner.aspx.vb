Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDashboardPlanner
    Inherits System.Web.UI.Page

    Public almacen As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
                    End If
                    BindGrid()
                    BindGridgridPlanificacionTemp()
                    cargarPlanificacionesAbiertas()
                End If
            End If
        Catch ex As Exception
            Response.Write("TrasladosDashboard_Load " & ex.Message)
        End Try
    End Sub

    Public Sub cargarPlanificacionesAbiertas()
        Try
            Dim sql As String = "SELECT [ID] FROM [dbo].[PLANIFICACIONES] WHERE ESTADO='ABIERTO'"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
            drpPlanificacionesAbiertas.DataTextField = "ID"
            drpPlanificacionesAbiertas.DataValueField = "ID"
            drpPlanificacionesAbiertas.DataSource = dt
            drpPlanificacionesAbiertas.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('cargarPlanificacionesAbiertas: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub BindGrid()
        Try
            Dim SQL_string As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/planificacion_dashboard_load.sql"))
            Dim dt As DataTable = DbConfig.GetDataTable(SQL_string, DbConfig.DBServer.ARMADOMOTOS)
            gridIndiceD.DataSource = dt
            gridIndiceD.DataBind()
            gridIndiceD.UseAccessibleHeader = True
            gridIndiceD.HeaderRow.TableSection = TableRowSection.TableHeader

            For Each row As GridViewRow In gridIndiceD.Rows
                For i As Integer = 8 To 10
                    Dim valor As Decimal
                    If Decimal.TryParse(row.Cells(i).Text.Replace("%", "").Trim(), valor) Then
                        row.Cells(i).Text = If(valor < 0, "0.00 %", FormatNumber(valor, 2) & " %")
                    End If
                Next
            Next
        Catch ex As Exception
            Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub

    Private Sub BindGridgridPlanificacionTemp()
        Try
            Dim sql As String = "SELECT * FROM [dbo].[PLANIFICACIONES_DETALLE] WHERE headerid=0"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
            gridPlanificacionTemp.DataSource = dt
            gridPlanificacionTemp.DataBind()
            gridPlanificacionTemp.UseAccessibleHeader = True
            If gridPlanificacionTemp.HeaderRow IsNot Nothing Then
                gridPlanificacionTemp.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridgridPlanificacionTemp: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Protected Sub gridIndiceD_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.Add("data-merge", "true")
            e.Row.Cells(1).Attributes.Add("data-merge", "true")
            e.Row.Cells(2).Attributes.Add("data-merge", "true")
            e.Row.Cells(3).Attributes.Add("data-merge", "true")
        End If
    End Sub

    Protected Sub gridPlanificacionTemp_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim valorPlanificacion As String = e.Row.Cells(5).Text.Trim()
                For Each rowIndiceD As GridViewRow In gridIndiceD.Rows
                    Dim valorIndice As String = rowIndiceD.Cells(6).Text.Trim()
                    Dim cb As CheckBox = CType(rowIndiceD.FindControl("cbDocument"), CheckBox)
                    If valorPlanificacion.Trim() = valorIndice.Trim() Then
                        cb.Visible = False
                        Dim checkLabel As New Label()
                        checkLabel.Text = "<i class='fa-solid fa-check fa-2x text-success text-center'></i>"
                        rowIndiceD.Cells(23).Controls.Add(checkLabel)
                    End If
                Next
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPlanificacionTemp_RowDataBound: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridPlanificacionTemp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPlanificacionTemp.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                EliminarLinea(gridPlanificacionTemp.Rows(index).Cells(1).Text)
                BindGrid()
                BindGridgridPlanificacionTemp()
                ClientScript.RegisterStartupScript(Me.GetType(), "swal",
                    "<script>Swal.fire({title:'Éxito!',text:'Linea Eliminada Exitosamente!',icon:'success',confirmButtonText:'OK'});</script>")
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPlanificacionTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Sub EliminarLinea(idlinea As Integer)
        Try
            Dim sql As String = "DELETE FROM [dbo].[PLANIFICACIONES_DETALLE] WHERE [ID] = @p1"
            DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                New List(Of SqlParameter) From {New SqlParameter("@p1", idlinea)})
        Catch ex As Exception
            Response.Write("EliminarLinea " & ex.Message)
        End Try
    End Sub

    Private Sub btnCrearPlan_Click(sender As Object, e As EventArgs) Handles btnCrearPlan.Click
        Try
            Dim lineas As Integer = 0
            For Each row As GridViewRow In gridIndiceD.Rows
                Dim cb As CheckBox = CType(row.FindControl("cbDocument"), CheckBox)
                If cb IsNot Nothing AndAlso cb.Checked Then
                    lineas += 1
                    Dim ruta As String = row.Cells(0).Text
                    Dim canal As String = row.Cells(1).Text
                    Dim cindice As String = row.Cells(2).Text
                    Dim leyenda As String = row.Cells(3).Text
                    Dim ranking As String = row.Cells(4).Text
                    Dim crank As String = row.Cells(5).Text
                    Dim codigo As String = row.Cells(6).Text
                    almacen = row.Cells(7).Text
                    Dim indice As String = row.Cells(10).Text.Replace(" %", "")
                    Dim indiceProyectado As String = row.Cells(11).Text.Replace("%", "")
                    Dim cuadro As String = row.Cells(12).Text
                    Dim comprometido As String = row.Cells(14).Text
                    Dim solicitado As String = row.Cells(15).Text
                    Dim transito As String = row.Cells(16).Text
                    Dim fisico As String = row.Cells(20).Text
                    Dim faltante As String = row.Cells(21).Text
                    Dim unds As String = row.Cells(19).Text

                    Dim query As String =
                        "INSERT INTO [dbo].[PLANIFICACIONES_DETALLE] " &
                        "([CANAL],[RANKING],[RUTA],[CODIGO],[ALMACEN],[CUADRO],[COMP],[SOL],[TRANSITO]," &
                        " [FISICO],[FALTANTE],[UNDS],[INDICE],[CINDICE],[CRANKING],[LEYENDA],[HEADERID]," &
                        " [ESTADOHEADER],[ESTADOLINEA],[INDICEP]) " &
                        "VALUES (@CANAL,@RANKING,@RUTA,@CODIGO,@ALMACEN,@CUADRO,@COMP,@SOL,@TRANSITO," &
                        " @FISICO,@FALTANTE,@UNDS,@INDICE,@CINDICE,@CRANKING,@LEYENDA,0," &
                        " 'TEMPORAL','ABIERTO',@INDICEP)"

                    Dim params As New List(Of SqlParameter) From {
                        New SqlParameter("@CANAL", canal),
                        New SqlParameter("@RANKING", ranking),
                        New SqlParameter("@RUTA", ruta),
                        New SqlParameter("@CODIGO", codigo),
                        New SqlParameter("@ALMACEN", almacen),
                        New SqlParameter("@CUADRO", cuadro),
                        New SqlParameter("@COMP", comprometido),
                        New SqlParameter("@SOL", solicitado),
                        New SqlParameter("@TRANSITO", transito),
                        New SqlParameter("@FISICO", fisico),
                        New SqlParameter("@FALTANTE", faltante),
                        New SqlParameter("@UNDS", unds),
                        New SqlParameter("@INDICE", indice),
                        New SqlParameter("@CINDICE", cindice),
                        New SqlParameter("@CRANKING", crank),
                        New SqlParameter("@LEYENDA", leyenda),
                        New SqlParameter("@INDICEP", indiceProyectado)
                    }
                    DbConfig.ExecuteNonQuery(query, DbConfig.DBServer.ARMADOMOTOS, params)
                End If
            Next
            BindGrid()
            BindGridgridPlanificacionTemp()
            ClientScript.RegisterStartupScript(Me.GetType(), "swal",
                "<script>Swal.fire({title:'Éxito!',text:'" & lineas & " Lineas Agregadas Exitosamente!',icon:'success',confirmButtonText:'OK'});</script>")
        Catch ex As Exception
            Response.Write("<script>console.log('btnCrearPlan_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Protected Sub btnEliminarLineas_Click(sender As Object, e As EventArgs) Handles btnEliminarLineas.Click
        If Request("__EVENTARGUMENT") = "EliminarConfirmado" Then
            EliminarLineasConfirmadas()
        Else
            Dim scriptConfirm As String = "<script>" &
                "Swal.fire({title:'¿Está seguro?',text:'Esta acción eliminará todas las líneas de forma permanente.'," &
                "icon:'warning',showCancelButton:true,confirmButtonColor:'#3085d6',cancelButtonColor:'#d33'," &
                "confirmButtonText:'Sí, eliminar',cancelButtonText:'Cancelar'}).then((result)=>{" &
                "if(result.isConfirmed){__doPostBack('" & btnEliminarLineas.UniqueID & "','EliminarConfirmado');}" &
                "});</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "swalConfirm", scriptConfirm)
        End If
    End Sub

    Private Sub EliminarLineasConfirmadas()
        Try
            Dim lineas As Integer = 0
            For Each row As GridViewRow In gridPlanificacionTemp.Rows
                lineas += 1
                Dim sql As String = "DELETE FROM [dbo].[PLANIFICACIONES_DETALLE] WHERE id = @lineId"
                DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                    New List(Of SqlParameter) From {New SqlParameter("@lineId", row.Cells(1).Text)})
            Next
            BindGrid()
            BindGridgridPlanificacionTemp()
            ClientScript.RegisterStartupScript(Me.GetType(), "swalSuccess",
                "<script>Swal.fire({title:'Éxito!',text:'" & lineas & " Líneas eliminadas exitosamente!',icon:'success',confirmButtonText:'OK'});</script>")
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "swalError",
                "<script>console.log('" & Replace(Replace(Replace(ex.Message, ",", ""), "'", ""), ";", "") & "');</script>")
        End Try
    End Sub

    Private Sub btnAgregarPlan_Click(sender As Object, e As EventArgs) Handles btnAgregarPlan.Click
        Try
            Dim lineas As Integer = 0
            For Each row As GridViewRow In gridPlanificacionTemp.Rows
                lineas += 1
                Dim sql As String = "UPDATE [dbo].[PLANIFICACIONES_DETALLE] SET [HEADERID]=@p1,[ESTADOHEADER]=@p2 WHERE ID=@p3"
                DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS,
                    New List(Of SqlParameter) From {
                        New SqlParameter("@p1", drpPlanificacionesAbiertas.SelectedValue),
                        New SqlParameter("@p2", "PLANIFICACION"),
                        New SqlParameter("@p3", row.Cells(1).Text)
                    })
            Next
            BindGrid()
            BindGridgridPlanificacionTemp()
            ClientScript.RegisterStartupScript(Me.GetType(), "swal",
                "<script>Swal.fire({title:'Éxito!',text:'" & lineas & " Lineas Agregadas Exitosamente!',icon:'success',confirmButtonText:'OK'});</script>")
        Catch ex As Exception
            Response.Write("<script>console.log('btnAgregarPlan_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnCalendario_Click(sender As Object, e As EventArgs) Handles btnCalendario.Click
        Response.Redirect("TrasladosDespachosCalendario.aspx")
    End Sub

    Private Sub btnCuadroBasico_Click(sender As Object, e As EventArgs) Handles btnCuadroBasico.Click
        Response.Redirect("TrasladosCuadroBasico.aspx")
    End Sub

    Protected Overrides Sub Render(writer As HtmlTextWriter)
        ClientScript.RegisterForEventValidation(btnEliminarLineas.UniqueID, "EliminarConfirmado")
        MyBase.Render(writer)
    End Sub

    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
        Dim chars() As String = {"/", "\", ":", "?", Chr(34), "<", ">", "|", "&", "%", "*", "'", "{", "[", "]", "}", "!", ","}
        For Each c As String In chars
            sName = Replace(sName, c, sChr)
        Next
        Return sName
    End Function

End Class

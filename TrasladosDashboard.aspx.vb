Imports System.Data
Imports System.Web.Services
Imports System.Data.SqlClient
Imports Newtonsoft.Json

Partial Class TrasladosDashboard
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public MyTable_Enpoceso As New DataTable
    Public MyTable_Finalizado As New DataTable
    Private Sub TrasladosDashboard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Response.Redirect("TrasladosDashboardPlanner.aspx")

            'If IsPostBack = False Then
            '    If Session("Name") Is vbNullString Then
            '        Response.Redirect("Default.aspx")
            '    Else

            '        If Session("USERROL") = "Jefe Tienda" Then
            '            Response.Redirect("MaindashboardSucursales.aspx")
            '        Else
            '            LoadGrid()
            '            LoadGrid_EnProceso()
            '            LoadGridFinalizado()
            '        End If
            '    End If
            'End If
            lblDiunsa.Text = obtenerPedidosPendientesDiunsa()
        Catch ex As Exception
            Response.Write("TrasladosDashboard_Load " & ex.Message)
        End Try
    End Sub
    Sub LoadGrid()
        Try
            Dim strSQL As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/TrasladosDashboard_LoadGrid.sql"))
            Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon2))
                cmdSQL.Connection.Open()
                MyTable.Load(cmdSQL.ExecuteReader)
                repeater1.DataSource = MyTable
                repeater1.DataBind()
                cmdSQL.Connection.Close()
            End Using
        Catch ex As Exception
            Response.Write("LoadGrid " & ex.Message)
        End Try
    End Sub
    Sub LoadGrid_EnProceso()
        Try
            Dim strSQL As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/TrasladosDashboard_LoadGrid_EnProceso.sql"))
            Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon2))
                cmdSQL.Connection.Open()
                MyTable_Enpoceso.Load(cmdSQL.ExecuteReader)
                repeater3.DataSource = MyTable_Enpoceso
                repeater3.DataBind()
                cmdSQL.Connection.Close()
            End Using
        Catch ex As Exception
            Response.Write("LoadGrid_EnProceso " & ex.Message)
            Response.Write("<script>console.log('VisorLogistica_Load: " & ReplaceCharsForFileName("Mscroid" & ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Sub LoadGridFinalizado()
        Try
            Dim strSQL As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/TrasladosDashboard_LoadGridFinalizado.sql"))
            Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon2))
                cmdSQL.Connection.Open()
                MyTable_Finalizado.Load(cmdSQL.ExecuteReader)
                repeater2.DataSource = MyTable_Finalizado
                repeater2.DataBind()
                cmdSQL.Connection.Close()
            End Using
        Catch ex As Exception
            Response.Write("LoadGridFinalizado " & ex.Message)
        End Try
    End Sub
    'Protected Sub btnEliminar_Click(sender As Object, e As EventArgs)
    '    Try
    '        Dim id As Integer = Convert.ToInt32(TryCast(sender, Button).CommandArgument)
    '        Dim result As String = EliminarRegistro(id)

    '        Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Registro Eliminado!!',position: 'topRight',timeout: 10000})</script>"
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
    '        Threading.Thread.Sleep(5000)
    '        LoadGrid()
    '        LoadGridFinalizado()

    '    Catch ex As Exception
    '        Response.Write("btnEliminar_Click " & ex.Message)
    '    End Try
    'End Sub
    Protected Sub btnFinalizar_Click(sender As Object, e As EventArgs)
        Try
            Dim id As Integer = Convert.ToInt32(TryCast(sender, Button).CommandArgument)
            Dim result As String = FinalizarRegistro(id)

            Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Documento Finalizado!!',position: 'topRight',timeout: 10000})</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            Threading.Thread.Sleep(5000)
            LoadGrid()
            LoadGridFinalizado()
        Catch ex As Exception
            Response.Write("btnFinalizar_Click " & ex.Message)
        End Try
    End Sub
    Protected Sub btnActivar_Click(sender As Object, e As EventArgs)
        Try
            Dim id As Integer = Convert.ToInt32(TryCast(sender, Button).CommandArgument)
            Dim result As String = ActivarRegistro(id)

            Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Documento Activado!!',position: 'topRight',timeout: 10000})</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            Threading.Thread.Sleep(5000)
            LoadGrid()
            LoadGridFinalizado()
        Catch ex As Exception
            Response.Write("btnActivar_Click " & ex.Message)
        End Try
    End Sub
    Protected Sub btnAbrirFly_Click(sender As Object, e As EventArgs)
        Try
            Dim id As Integer = Convert.ToInt32(TryCast(sender, Button).CommandArgument)
            txtFlyMiniId.Text = id

            Dim Flyscript As String = "<script>$('#IniciarPintura').flyout('toggle');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", Flyscript, False)
        Catch ex As Exception
            Response.Write("btnActivar_Click " & ex.Message)
        End Try
    End Sub
    <WebMethod()>
    Public Shared Function EliminarRegistro(id As Integer) As String
        Dim connectionString As String = sCon2
        Using con As New SqlConnection(connectionString)
            con.Open()
            Dim query As String = " " &
                " DELETE FROM MACROINSERT WHERE HEADERID= @p1 " &
                " ;DELETE FROM MACROINSERT_HEADER WHERE ID= @p1 " &
                " ;DELETE FROM PRESOLICITUD_HEADER  WHERE MACROID=@p1 " &
                " ;DELETE FROM PRESOLICITUD_LINES WHERE MACROID=@p1 " &
                " ;DELETE FROM SOLCITUD_HEADER WHERE MACROID=@p1 " &
                " ;DELETE FROM SOLCITUD_LINES WHERE MACROID=@p1" &
                " ;DELETE FROM PRESOLICITUD_HEADER WHERE ID=@p1 " &
                " ;DELETE FROM PRESOLICITUD_LINES WHERE HEADERID=@p1 "
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@p1", id)
                cmd.ExecuteNonQuery()
                Return "Registro eliminado correctamente."
            End Using
            con.Close()
        End Using
        Return "Error al eliminar el registro."
    End Function
    <WebMethod()>
    Public Shared Function ActivarRegistro(id As Integer) As String
        Dim connectionString As String = sCon2
        Using con As New SqlConnection(connectionString)
            con.Open()
            Dim query As String = "UPDATE PRESOLICITUD_HEADER SET ESTADO = 'A' WHERE ID=@p1 "
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@p1", id)
                cmd.ExecuteNonQuery()
                Return "Registro eliminado correctamente."
            End Using
            con.Close()
        End Using
        Return "Error al eliminar el registro."
    End Function
    <WebMethod()>
    Public Shared Function FinalizarRegistro(id As Integer) As String
        Dim connectionString As String = sCon2
        Using con As New SqlConnection(connectionString)
            con.Open()
            Dim query As String = "UPDATE PRESOLICITUD_HEADER SET ESTADO = 'C' WHERE ID = @p1; " &
            " UPDATE [ArmadoMotos].[dbo].[MACROINSERT_HEADER] SET [ESTATUS]='C' WHERE ID = @p1 "
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@p1", id)
                cmd.ExecuteNonQuery()
                Return "Registro eliminado correctamente."
            End Using
            con.Close()
        End Using
        Return "Error al eliminar el registro."
    End Function

    Private Sub btnProcederEliminar_Click(sender As Object, e As EventArgs) Handles btnProcederEliminar.Click
        Try
            EliminarRegistro(txtFlyMiniId.Text)
            Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Registro Eliminado!!',position: 'topRight',timeout: 10000})</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            LoadGrid()
            LoadGridFinalizado()
        Catch ex As Exception
            Response.Write(ex.Message)
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
    Public Function obtenerPedidosPendientesDiunsa() As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT COUNT(T0.DOCNUM) AS PEDIDOS_PENDIENTES FROM MOVESAWEB..TBL_REPORTES_VENTAS_DISTRIBUIDORES AS T0 " &
              "LEFT JOIN MOVESAWEB..TBL_REPORTES_VENTAS_DISTRIBUIDORES_DETALLE AS T1 ON T1.DOCNUM = T0.DOCNUM " &
              "WHERE DOCTYPE IN ('EN','OP') AND T0.SERIAL_NUM IS NULL AND T1.SERIAL_NUM IS NULL"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()

            Return t
        End Using
    End Function
End Class

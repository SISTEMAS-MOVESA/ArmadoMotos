Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Partial Class Task
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"

    Private Sub Task_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'GESTORC
            If Session("Position").ToString = "GESTORC" Then
                pnlSubMenuOrdenRecuperacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = False
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            'SKA1001
            If Session("Position").ToString = "ADMIN" Then
                pnlSubMenuConfirmacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = False
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            'VENDEDOR
            If Session("Position").ToString = "VENDEDOR" Then
                pnlSubMenuConfirmacion.Visible = False
                pnlSubMenuOrdenRecuperacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = True
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            'CargarTareasPendientes()
            If Not Me.IsPostBack Then
                Me.BindGrid()
            End If
            lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString

        Catch ex As Exception
            ADD_LOG("Page_Load", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub BindGrid()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand(" SELECT [ID],DATEDIFF(DAY,[CREATEDATE],GETDATE()) [DIAS],[DOCNUM],[CARDCODE],[CARDNAME],[IDENTIDAD],[RTN],[ITEMCODE] " &
                            ", [MODELO], [Year], [ITEMNAME], [MARCA], [CILINDROS], [COLOR], [ADUANA]" &
                            ", [POLIZA], [SERIE], [SERIEM],[GESTOR]" &
                            " FROM [MOVESAWeb].[dbo].[ENTRADAOPSKG] WHERE ESTADO='Recuperar'")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                        End Using
                    End Using
                End Using
            End Using
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            ADD_LOG("CargarTareasPendientes", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CargarTareasPendientes()
        Try
            Dim constr As String = sCon1
            Using conn As SqlConnection = New SqlConnection(constr)
                Using sda As SqlDataAdapter = New SqlDataAdapter(" SELECT [ID],[DOCNUM],[CARDCODE],[CARDNAME],[IDENTIDAD],[RTN],[ITEMCODE] " &
                            ", [MODELO], [Year], [ITEMNAME], [MARCA], [CILINDROS], [COLOR], [ADUANA]" &
                            ", [POLIZA], [ITEM], [SERIE], [SERIEM]" &
                            " FROM [MOVESAWeb].[dbo].[ENTRADAOPSKG]", conn)
                    Dim dt As DataTable = New DataTable()
                    sda.Fill(dt)
                    GridView1.DataSource = dt
                    GridView1.DataBind()
                End Using
            End Using
        Catch ex As Exception
            ADD_LOG("CargarTareasPendientes", ex.Message)
        End Try
    End Sub

    Public Sub ADD_LOG(_PROCESO As String, _ERROR As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "INSERT INTO [MovesaWeb].[dbo].[PortalSKG] ([FECHA],[PROCESO],[ERROR])" &
                   " VALUES (@p1,@p2,@p3)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", Date.Now)
                cmd.Parameters.AddWithValue("@p2", _PROCESO)
                cmd.Parameters.AddWithValue("@p3", _ERROR)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Agregar" Then
                Select Case Session("Position").ToString
                    Case "GESTORC"
                        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                        Dim gvRow As GridViewRow = GridView1.Rows(index)
                        Session("TaskNumber") = GridView1.Rows(index).Cells(2).Text
                        Response.Redirect("ConfirmacionDacion.aspx")
                    Case "VENDEDOR"
                        Response.Redirect("Task.aspx")
                    Case "ADMIN"
                        Response.Redirect("Task.aspx")
                End Select
            End If
            If e.CommandName = "Modificar" Then

                Select Case Session("Position").ToString
                    Case "GESTORC"
                        Response.Redirect("Task.aspx")
                    Case "VENDEDOR"
                        Response.Redirect("Task.aspx")
                    Case "ADMIN"
                        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                        Dim gvRow As GridViewRow = GridView1.Rows(index)
                        txtOrder.Text = GridView1.Rows(index).Cells(2).Text
                        pnlUpdate.Visible = True
                        pnlGrid.Visible = False
                End Select
            End If
        Catch ex As Exception
            ADD_LOG("GridView1_RowCommand", ex.Message)
        End Try
    End Sub


    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("Task.aspx")
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        UpdateOrderStatus(txtOrder.Text, drpOpciones.SelectedValue)
        Me.BindGrid()
        pnlUpdate.Visible = False
        pnlGrid.Visible = True
    End Sub

    Public Sub UpdateOrderStatus(_order As String, _estatus As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " UPDATE [MOVESAWEB].[dbo].[ENTRADAOPSKG]  SET [ESTADO] = @P2 WHERE [ID]= @P2 "
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@P1", _estatus)
                cmd.Parameters.AddWithValue("@P2", _order)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Audit(Date.Now, "UpdateOrderStatus", Session("User").ToString, "Dacion: #" & _order & " Estado :" & _estatus)
        Catch ex As Exception
            ADD_LOG("UpdateBikePictures", ex.Message)
        End Try
    End Sub

    Public Sub Audit(_fecha As String, _proceso As String, _usuario As String, _observaciones As String)
        Try
            Dim query As String = String.Empty
            query &= " INSERT INTO [MovesaWeb].[dbo].[ENTRADAOPSKG_AUDIT] ([FECHA],[PROCESO],[USUARIO],[OBSERVACIONES]) " &
                      " VALUES (@p1,@p2,@p3,@p4) "
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _fecha)
                        .Parameters.AddWithValue("@p2", _proceso)
                        .Parameters.AddWithValue("@p3", _usuario)
                        .Parameters.AddWithValue("@p4", _observaciones)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ADD_LOG("Audit", ex.Message)
        End Try
    End Sub

    Private Sub ExportGridToExcel()
        Response.Clear()
        Response.Buffer = True
        Response.ClearContent()
        Response.ClearHeaders()
        Response.Charset = ""
        Dim FileName As String = "Tareas " & DateTime.Now & ".xls"
        Dim strwritter As StringWriter = New StringWriter()
        Dim htmltextwrtter As HtmlTextWriter = New HtmlTextWriter(strwritter)
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & FileName)
        GridView1.GridLines = GridLines.Both
        GridView1.HeaderStyle.Font.Bold = True
        GridView1.RenderControl(htmltextwrtter)
        Response.Write(strwritter.ToString())
        Response.[End]()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

    Protected Sub btnIngreso_Click(sender As Object, e As EventArgs) Handles btnIngreso.Click
        ExportGridToExcel()
    End Sub
End Class

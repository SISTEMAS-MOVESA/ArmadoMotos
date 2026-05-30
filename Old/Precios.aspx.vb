Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Partial Class Precios
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.60;database=PRUEBAS;uid=sa;password=S@pB1Sql"

    Private Sub Precios_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'GESTORC
            If Session("Position").ToString = "GESTORC" Then
                Response.Redirect("Menu.aspx")
            End If
            'SKA1001
            If Session("Position").ToString = "ADMIN" Then
                Response.Redirect("Menu.aspx")
            End If
            'VENDEDOR
            If Session("Position").ToString = "VENDEDOR" Then
                Response.Redirect("Menu.aspx")
            End If
            lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
            'CargarTareasPendientes()
            If Not Me.IsPostBack Then
                Me.BindGrid()
            End If
        Catch ex As Exception
            ADD_LOG("Page_Load", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand(" select convert(char,InDate,103) [Fecha],itemcode [Articulo],ItemName [Descripcion] " &
                                                " , SuppSerial [Serie], IntrSerial [SerieM], WhsCode [Almacen], Replace(isnull(U_SCOLOR,'-'),' ','-') [Color] " &
                                                " , isnull(Convert(Decimal(19, 2), U_PrecioMat), 0.00) [Matricula], isnull(Convert(Decimal(19, 2), U_PrecioVenta), 0.00) [Precio]  " &
                                                " From [PRUEBAS].[DBO].[osri] with(nolock) Where Status = 0 ")
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
            ADD_LOG("BindGrid Cargar Tareas Pendientes", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub ADD_LOG(_PROCESO As String, _ERROR As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "INSERT INTO [MovesaWeb].[dbo].[PortalSKG] ([FECHA], [PROCESO], [Error])" &
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

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try 
            If e.CommandName = "Agregar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                txtSerie.Text = GridView1.Rows(index).Cells(3).Text
                pnlUpdate.Visible = True
                pnlGrid.Visible = False
            End If
            'If e.CommandName = "Compras" Then
            'End If
        Catch ex As Exception
            ADD_LOG("GridView1_RowCommand", ex.Message)
        End Try
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        UpdateSerialPrice(txtSerie.Text, txtPrecio.Text)
        Me.BindGrid()
        pnlGrid.Visible = True
        pnlUpdate.Visible = False
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlGrid.Visible = True
        pnlUpdate.Visible = False
    End Sub

    Public Sub UpdateSerialPrice(_Serie As String, _Precio As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = " update [PRUEBAS].[dbo].[OSRN] SET U_PrecioVenta=@P1 WHERE MnfSerial=@P2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@P1", _Precio)
                cmd.Parameters.AddWithValue("@P2", _Serie)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Audit(Date.Now, "UpdateSerialPrice", Session("User").ToString, "Dacion: #" & _Serie & " Precio :" & _Precio)
        Catch ex As Exception
            ADD_LOG("UpdateBikePictures", ex.Message)
        End Try
    End Sub
End Class
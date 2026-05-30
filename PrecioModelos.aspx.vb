Imports System.Data
Imports System.Data.SqlClient
Partial Class PrecioModelos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
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
                cmd.ExecuteNonQuery()
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
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            ADD_LOG("Audit", ex.Message)
        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Dim sqlqry_string As String = "SELECT CODE,NAME,isnull(U_LG5,'-') U_LG5, isnull(U_LG6,0)[U_LG6], " &
                                            "isnull(U_LG7,'Sin Asignar') [U_LG7], isnull(U_Orden,0) U_Orden,U_Precio " &
                                            "FROM [MOVESA].[DBO].[@AMODELO] WITH(NOLOCK) WHERE U_LG5='Y'"

                'Dim sqlqry_string As String = "SELECT CODE,NAME,isnull(U_LG5,'-') U_LG5, isnull(U_LG6,0)[U_LG6], " &
                '                            "isnull(U_LG7,'Sin Asignar') [U_LG7], isnull(U_Orden,0) U_Orden,U_Precio " &
                '                            "FROM [MOVESA].[DBO].[@AMODELO] WITH(NOLOCK) where isnull(U_LG7,'Sin Asignar')='" & _TIPO & "' " &
                '                            "and U_LG5='Y'"

                Using cmd As New SqlCommand(sqlqry_string)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            'ADD_LOG("CargarTareasPendientes BindGrid ", ex.Message)
            Response.Write("CargarPreciosModelo BindGrid " & ex.Message)
        End Try
    End Sub
    Public Sub _CargarModelosPrecios()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "SELECT distinct isnull(U_LG7,'Sin Asignar') [U_LG7] FROM [movesa].[dbo].[@AMODELO] WHERE U_LG5='Y'"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using
            'drpTipoMoto.Dispose()
            'drpTipoMoto.DataTextField = "U_LG7"
            'drpTipoMoto.DataValueField = "U_LG7"
            'drpTipoMoto.DataSource = dt
            'drpTipoMoto.DataBind()

            drpCreateTipoMoto.Dispose()
            drpCreateTipoMoto.DataTextField = "U_LG7"
            drpCreateTipoMoto.DataValueField = "U_LG7"
            drpCreateTipoMoto.DataSource = dt
            drpCreateTipoMoto.DataBind()

            drpUpdateTipoMoto.Dispose()
            drpUpdateTipoMoto.DataTextField = "U_LG7"
            drpUpdateTipoMoto.DataValueField = "U_LG7"
            drpUpdateTipoMoto.DataSource = dt
            drpUpdateTipoMoto.DataBind()
        Catch ex As Exception
            'ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub PrecioModelos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    _CargarModelosPrecios()
                    BindGrid()
                    Select Case Session("Position").ToString()
                        Case "Iniciador"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Supervisor Armado"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Calidad"
                            Response.Redirect("MainDashBoard.aspx")
                    End Select
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    'Private Sub drpTipoMoto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpTipoMoto.SelectedIndexChanged
    '    BindGrid(drpTipoMoto.SelectedValue.ToString())
    'End Sub
    Protected Sub btnIngreso_Click(sender As Object, e As EventArgs) Handles btnIngreso.Click
        FINDNEXTCODE()
        pnlCrearUsuario.Visible = True
        pnlGridUsuarios.Visible = False
    End Sub
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("PrecioModelos.aspx")
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("PrecioModelos.aspx")
    End Sub
    Public Sub FINDNEXTCODE()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "SELECT max(100000+code)+1 [correlativo] FROM [MOVESA].[dbo].[@amodelo] "

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtCreateNextCode.Text = drd.Item("correlativo").ToString - 100000

            End If
        Catch ex As Exception
            Response.Write("FINDNEXTCODE " & ex.Message)
        End Try
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Modificar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                pnlUpdate.Visible = True
                pnlCrearUsuario.Visible = False
                pnlGridUsuarios.Visible = False

                txtUpdateCodeModelo.Text = GridView1.Rows(index).Cells(1).Text
                txtUpdateNombreModelo.Text = GridView1.Rows(index).Cells(2).Text
                txtUpdateInvReserva.Text = GridView1.Rows(index).Cells(4).Text
                txtUpdatePrecioArmado.Text = GridView1.Rows(index).Cells(7).Text
                drpUpdateTipoMoto.SelectedItem.Text = GridView1.Rows(index).Cells(5).Text
            End If
        Catch ex As Exception
            Response.Write("GridView1_RowCommand " & ex.Message)
        End Try
    End Sub
    Protected Sub btnCreateModelo_Click(sender As Object, e As EventArgs) Handles btnCreateModelo.Click

    End Sub
    Public Sub ADD_LOG(_Codigo As String, _nombreModelo As String, _habilitado As String, _invReserva As String, _tipoMoto As String, _precioarmado As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " INSERT INTO [dbo].[@AMODELO] " &
                  " ([Code] " &
                  " ,[Name] " &
                  " ,[U_LG5] --habilitado " &
                  " ,[U_LG6] --inventario reserva " &
                  " ,[U_LG7] --tipo de moto " &
                  " ,[U_Precio] --precio armado) " &
                  " VALUES (@p1,@p2,@p3,@p4,@p5,@p6) "
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _Codigo)
                cmd.Parameters.AddWithValue("@p2", _nombreModelo)
                cmd.Parameters.AddWithValue("@p3", _habilitado)
                cmd.Parameters.AddWithValue("@p4", _invReserva)
                cmd.Parameters.AddWithValue("@p5", +_tipoMoto)
                cmd.Parameters.AddWithValue("@p6", _precioarmado)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub btnUpdateModelo_Click(sender As Object, e As EventArgs) Handles btnUpdateModelo.Click
        UodateModelo(txtUpdateNombreModelo.Text, drpUpdateDisponibleCB.SelectedValue.ToString, txtUpdateInvReserva.Text, drpUpdateTipoMoto.SelectedItem.Text, txtUpdatePrecioArmado.Text, txtUpdateCodeModelo.Text)
    End Sub
    Public Sub UodateModelo(_nombremodelo As String, _habilitadoCB As String, _invreserva As String, _tipomoto As String, _precioarmado As String, _codigomodelo As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[@AMODELO] " &
                    " SET [Name] = @p1" &
                    " ,[U_LG5] = @p2" &
                    " ,[U_LG6] = @p3" &
                    " ,[U_LG7] = @p4" &
                    " ,[U_Precio] = @p5" &
                    " WHERE [Code]=@p6"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _nombremodelo)
                cmd.Parameters.AddWithValue("@p2", _habilitadoCB)
                cmd.Parameters.AddWithValue("@p3", _invreserva)
                cmd.Parameters.AddWithValue("@p4", _tipomoto)
                cmd.Parameters.AddWithValue("@p5", _precioarmado)
                cmd.Parameters.AddWithValue("@p6", _codigomodelo)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Response.Redirect("PrecioModelos.aspx")
        Catch ex As Exception
            Response.Write("UpdatMecanico " & ex.Message)
        End Try
    End Sub
End Class
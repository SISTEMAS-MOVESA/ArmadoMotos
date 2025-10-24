Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Partial Class PedidoSupervisor
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public OTHEADERID As Integer

    <WebMethod()>
    Public Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1 'ConfigurationManager.ConnectionStrings("constr").ConnectionString
            Using cmd As SqlCommand = New SqlCommand()

                cmd.CommandText = " Select whsname from owhs With(nolock) where whsname Like '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("whsname").ToString())
                    End While
                End Using
                conn.Close()

                Return customers
            End Using
        End Using

    End Function
    Public Sub BuscarSerie(nalmacen As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = " select t0.WhsCode,t0.WhsName,(select name from [@CRUTA] with(nolock) where code=t0.U_Ruta)[Ruta] from owhs t0 with(nolock) where WhsName='" & nalmacen & "' "
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtCodigoAlmacen.Text = drd.Item("WhsCode").ToString
                txtNombreAlmacen.Text = drd.Item("WhsName").ToString
                txtRutaAlmacen.Text = drd.Item("Ruta").ToString
            End If
            _con.Close()
        Catch ex As Exception
            txtCodigoAlmacen.Text = ""
            txtNombreAlmacen.Text = ""
            txtRutaAlmacen.Text = ""
            'Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub _CargarModelos()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "select [CODE],[NAME] from [@AMODELO] with(nolock) WHERE U_LG5='Y' order by [Name]" '&
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpModelo.Dispose()
                drpModelo.DataTextField = "NAME"
                drpModelo.DataValueField = "CODE"
                drpModelo.DataSource = dt
                drpModelo.DataBind()
                conn.Close()
            End Using

        Catch ex As Exception
            'ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write("_CargarModelos " & ex.Message)
        End Try
    End Sub
    Private Sub drpModelo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpModelo.SelectedIndexChanged
        _CargarCodigosMotos(drpModelo.SelectedValue.ToString)
    End Sub
    Public Sub _CargarCodigosMotos(_codigoModelo As String)
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "select itemcode, itemname from OITM with(nolock) WHERE U_AMODELO='" & _codigoModelo & "' and ItmsGrpCod=154 order by itemname" '&
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpItemcodeMoto.Dispose()
                drpItemcodeMoto.DataTextField = "itemname"
                drpItemcodeMoto.DataValueField = "itemcode"
                drpItemcodeMoto.DataSource = dt
                drpItemcodeMoto.DataBind()
                conn.Close()
            End Using

        Catch ex As Exception
            'ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write("_CargarRutas " & ex.Message)
        End Try
    End Sub
    Public Sub CrearOTHeader(RUTA As String, CODEALM As String, ALMACEN As String, USUARIO As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = " INSERT INTO [dbo].[OTHEADER]([DATECREATED],[RUTA],[CODEALM],[ALMACEN],[USUARIO],[Preference])" &
                " VALUES (@p1,@p2,@p3,@p4,@p5,(select max(id)+1  from [dbo].[OTHEADER] with(nolock))) ​select scope_identity()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", Date.Now)
                cmd.Parameters.AddWithValue("@p2", RUTA)
                cmd.Parameters.AddWithValue("@p3", CODEALM)
                cmd.Parameters.AddWithValue("@p4", ALMACEN)
                cmd.Parameters.AddWithValue("@p5", USUARIO)
                con.Open()
                OTHEADERID = Convert.ToInt32(cmd.ExecuteScalar())
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("CrearOTHeader " & ex.Message)
        End Try
    End Sub
    Public Sub CrearOTLines(HID As String, CODIGOALMACEN As String, NOMBREALMACEN As String, MODELO As String, CODIGO As String, DESCRIPCION As String, CANTIDAD As Integer, ESPACIOS As Integer, USUARIO As String, Preference As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[OTLINES]([HID],[CALMACEN],[NALMACEN],[MODELO],[CODIGO],[DESCRIPCION],[CANTIDAD],[ESPACIOS],[USUARIO],[Preference])" &
                " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", HID)
                cmd.Parameters.AddWithValue("@p2", CODIGOALMACEN)
                cmd.Parameters.AddWithValue("@p3", NOMBREALMACEN)
                cmd.Parameters.AddWithValue("@p4", MODELO)
                cmd.Parameters.AddWithValue("@p5", CODIGO)
                cmd.Parameters.AddWithValue("@p6", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p7", CANTIDAD)
                cmd.Parameters.AddWithValue("@p8", ESPACIOS)
                cmd.Parameters.AddWithValue("@p9", USUARIO)
                cmd.Parameters.AddWithValue("@p10", Preference)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("CrearOTLines " & ex.Message)
        End Try
    End Sub

    Private Sub PedidoSupervisor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString

                    'Dim i As Integer
                    'drpCamion.Items.Insert(0, "Seleccione")
                    'drpCamion.SelectedIndex = 0
                    'For i = 1 To 200
                    '    drpCamion.Items.Insert(i, "Camion: " & i.ToString())
                    'Next


                    'drpPrioridad.Items.Insert(0, "Urgente")
                    'drpPrioridad.SelectedIndex = 0
                    'For i = 1 To 4
                    '    drpPrioridad.Items.Insert(i, "Prioridad " & i.ToString())
                    'Next

                    '_CargarRutas()
                    _CargarModelos()
                    _CargarCodigosMotos(drpModelo.SelectedValue.ToString)
                    LoadGridOTAbiertas()
                    'LoadGrid()
                End If
            Else
                BuscarSerie(txtFilterAlmacen.Text)
                LoadGridOTAbiertasLines(lblNumeroOrden.Text)
            End If
        Catch ex As Exception
            Response.Write("PedidoSupervisor_Load " & ex.Message)
        End Try
    End Sub
    Public Sub LoadGridOTAbiertas()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT * FROM [ArmadoMotos].[dbo].[OTHEADER] with(nolock) where ESTATUS='ABIERTO' order by preference")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridOrdenes.DataSource = dt
                            gridOrdenes.DataBind()

                            'Dim totalMotos As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("CANTIDAD"))
                            'Dim totalEspacios As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("ESPACIOS"))
                            'gvLocations.FooterRow.Cells(6).Text = "Motos: " & totalMotos.ToString("N2")
                            'gvLocations.FooterRow.Cells(6).Font.Bold = True
                            'gvLocations.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Left

                            'gvLocations.FooterRow.Cells(7).Text = "Espacios: " & totalEspacios.ToString("N2")
                            'gvLocations.FooterRow.Cells(7).Font.Bold = True
                            'gvLocations.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Left

                            gridOrdenes.UseAccessibleHeader = True
                            gridOrdenes.HeaderRow.TableSection = TableRowSection.TableHeader
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("LoadGridOTAbiertas " & ex.Message)
        End Try
    End Sub

    Public Sub LoadGridOTAbiertasLines(HID As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[MODELO],[CODIGO],[DESCRIPCION],[CANTIDAD],[ESPACIOS],[Preference] FROM [ArmadoMotos].[dbo].[OTLINES] where [HID]=" & HID & "")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridOrdenesLines.DataSource = dt
                            gridOrdenesLines.DataBind()

                            'Dim totalMotos As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("CANTIDAD"))
                            'Dim totalEspacios As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("ESPACIOS"))
                            'gvLocations.FooterRow.Cells(6).Text = "Motos: " & totalMotos.ToString("N2")
                            'gvLocations.FooterRow.Cells(6).Font.Bold = True
                            'gvLocations.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Left

                            'gvLocations.FooterRow.Cells(7).Text = "Espacios: " & totalEspacios.ToString("N2")
                            'gvLocations.FooterRow.Cells(7).Font.Bold = True
                            'gvLocations.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Left

                            gridOrdenesLines.UseAccessibleHeader = True
                            gridOrdenesLines.HeaderRow.TableSection = TableRowSection.TableHeader
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            'Response.Write("LoadGridOTAbiertasLines " & ex.Message)
        End Try
    End Sub
    Private Sub gridOrdenes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridOrdenes.RowCommand
        Try
            If e.CommandName = "Agregar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridOrdenes.Rows(index)
                txtFilterAlmacen.Text = gridOrdenes.Rows(index).Cells(6).Text
                BuscarSerie(gridOrdenes.Rows(index).Cells(6).Text)
                lblNumeroOrden.Text = gridOrdenes.Rows(index).Cells(2).Text
                LoadGridOTAbiertasLines(gridOrdenes.Rows(index).Cells(2).Text)

            End If
            If e.CommandName = "Buscar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridOrdenes.Rows(index)
                txtFilterAlmacen.Text = gridOrdenes.Rows(index).Cells(6).Text
                lblNumeroOrden.Text = gridOrdenes.Rows(index).Cells(2).Text
                BuscarSerie(gridOrdenes.Rows(index).Cells(6).Text)
                LoadGridOTAbiertasLines(gridOrdenes.Rows(index).Cells(2).Text)
            End If
        Catch ex As Exception
            Response.Write("gridOrdenes_RowCommand " & ex.Message)
        End Try
    End Sub
    Private Sub gridOrdenesLines_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridOrdenesLines.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridOrdenesLines.Rows(index)

                Dim strSQL As String = "delete from [dbo].[OTLINES] where id=" & gridOrdenesLines.Rows(index).Cells(1).Text & ";"
                Using conn As New SqlConnection(sCon2)
                    Using cmdSQL As New SqlCommand(strSQL, conn)
                        conn.Open()
                        cmdSQL.ExecuteNonQuery()
                        conn.Close()
                    End Using
                End Using

            End If
            'txtFilterAlmacen.Text = gridOrdenes.Rows(Index).Cells(6).Text
            'lblNumeroOrden.Text = gridOrdenes.Rows(Index).Cells(2).Text
            'BuscarSerie(gridOrdenes.Rows(Index).Cells(6).Text)
            LoadGridOTAbiertasLines(lblNumeroOrden.Text)
        Catch ex As Exception
            Response.Write("gridOrdenesLines_RowCommand " & ex.Message)
        End Try
    End Sub
End Class

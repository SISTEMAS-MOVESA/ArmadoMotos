Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Partial Class Contratistas
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public selContratistaUpdate As String
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
                Using cmd As New SqlCommand("SELECT [ID],[GRUPONAME],[GRUPORESPONSABLE],[GRUPOEMAILRESPONSABLE],[GRUPOSTATUS],[BPCODE] FROM [dbo].[GRUPOS] with(nolock)")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GRID_1.DataSource = dt
                            GRID_1.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GRID_1.UseAccessibleHeader = True
            GRID_1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            'ADD_LOG("CargarTareasPendientes BindGrid ", ex.Message)
            Response.Write("CargarTareasPendientes BindGrid " & ex.Message)
        End Try
    End Sub
    Private Sub BindGridMecanicos(_grupoid As String)
        Try
            BindGrid()
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT ID, GRUPOID, MECANICONAME, MECANICOEMAIL, MECANICOESTATUS FROM [ArmadoMotos].[dbo].[MECANICOS] with(nolock) where GRUPOID=" & _grupoid & "")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GRID_2.DataSource = dt
                            GRID_2.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GRID_2.UseAccessibleHeader = True
            GRID_2.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGridMecanicos BindGrid " & ex.Message)
        End Try
    End Sub
    'Private Function GetData() As DataTable
    '    Dim constr As String = sCon1 'ConfigurationManager.ConnectionStrings("constr").ConnectionString
    '    Using con As New SqlConnection(constr)
    '        Using cmd As New SqlCommand("select [ID],[USERCODE],[USERNAME],[USEREMAIL],[USERROL],[USERESTATUS] from [ArmadoMotos].[DBO].[USUARIOS] with(nolock) ")
    '            Using sda As New SqlDataAdapter()
    '                cmd.Connection = con
    '                sda.SelectCommand = cmd
    '                Using dt As New DataTable()
    '                    sda.Fill(dt)
    '                    Return dt
    '                End Using
    '            End Using
    '        End Using
    '    End Using
    'End Function

    Private Sub Contratistas_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
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

    Private Sub gvCustomers_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GRID_1.RowCommand
        If e.CommandName = "Buscar" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim gvRow As GridViewRow = GRID_1.Rows(index)
            pnlMecanicosDetalle.Visible = True
            BindGridMecanicos(GRID_1.Rows(index).Cells(2).Text)
        End If
        If e.CommandName = "Editar" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim gvRow As GridViewRow = GRID_1.Rows(index)
            pnlContratistasMain.Visible = False
            pnlMecanicosDetalle.Visible = False
            pnlCrearContratista.Visible = False
            pnlModificarContratista.Visible = True
            pnlCrearMecanico.Visible = False
            pnlModificarMecanico.Visible = False
            txtUpdateContratistaID.Text = GRID_1.Rows(index).Cells(2).Text
            txtUpdateContratistaCode.Text = GRID_1.Rows(index).Cells(3).Text
            'pnlMecanicosDetalle.Visible = True
            'BindGridMecanicos(gvCustomers.Rows(index).Cells(1).Text)
        End If
    End Sub
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("Contratistas.aspx")
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("Contratistas.aspx")
    End Sub
    Protected Sub btnAgregarContratista_Click(sender As Object, e As EventArgs) Handles btnAgregarContratista.Click
        pnlContratistasMain.Visible = False
        pnlMecanicosDetalle.Visible = False
        pnlCrearContratista.Visible = True
        pnlModificarContratista.Visible = False
        pnlCrearMecanico.Visible = False
        pnlModificarMecanico.Visible = False
    End Sub
    Protected Sub btnAgregarMeca_Click(sender As Object, e As EventArgs) Handles btnAgregarMeca.Click
        pnlContratistasMain.Visible = False
        pnlMecanicosDetalle.Visible = False
        pnlCrearContratista.Visible = False
        pnlModificarContratista.Visible = False
        pnlCrearMecanico.Visible = True
        pnlModificarMecanico.Visible = False
        _CargarModelosPrecios()

    End Sub
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Response.Redirect("Contratistas.aspx")
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CreateContratista(txtCodigoContratista.Text, txtResponsableContratista.Text, txtEmailContratista.Text, "True", Date.Now, Session("UserCode").ToString)
    End Sub
    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles btnRegresarContratista.Click
        Response.Redirect("Contratistas.aspx")
    End Sub

    Private Sub GridMecanicosDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GRID_2.RowCommand
        Try
            If e.CommandName = "Editar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GRID_2.Rows(index)
                pnlContratistasMain.Visible = False
                pnlMecanicosDetalle.Visible = False
                pnlCrearContratista.Visible = False
                pnlModificarContratista.Visible = False
                pnlCrearMecanico.Visible = False
                pnlModificarMecanico.Visible = True
                txtUpdateMecanicoID.Text = GRID_2.Rows(index).Cells(1).Text
                txtUpdateMecanicoGroupCode.Text = GRID_2.Rows(index).Cells(2).Text
                txtUpdateMecaName.Text = GRID_2.Rows(index).Cells(3).Text
                txtUpdateMecaEmail.Text = GRID_2.Rows(index).Cells(4).Text
                _CargarModelosPrecios()
                drpUpdateGrupoMeca.Items.FindByValue(GRID_2.Rows(index).Cells(2).Text).Selected = True
            End If
        Catch ex As Exception
            Response.Write("GridMecanicosDetalle_RowCommand " & ex.Message)
        End Try

    End Sub
    Public Sub _CargarModelosPrecios()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "SELECT [ID],[GRUPONAME] from [dbo].[GRUPOS] with(nolock)"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                drpGrupoContratistas.Dispose()
                drpGrupoContratistas.DataTextField = "GRUPONAME"
                drpGrupoContratistas.DataValueField = "ID"
                drpGrupoContratistas.DataSource = dt
                drpGrupoContratistas.DataBind()

                drpUpdateGrupoMeca.Dispose()
                drpUpdateGrupoMeca.DataTextField = "GRUPONAME"
                drpUpdateGrupoMeca.DataValueField = "ID"
                drpUpdateGrupoMeca.DataSource = dt
                drpUpdateGrupoMeca.DataBind()
                conn.Close()
            End Using

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub CreateContratista(_gruponame As String, _gruporesponsable As String, _grupoemail As String, _grupostatus As String, _datecreated As Date, _codecreated As String)
        Try
            Dim query As String = String.Empty
            query &= " INSERT INTO [ArmadoMotos].[dbo].[GRUPOS] ([GRUPONAME],[GRUPORESPONSABLE],[GRUPOEMAILRESPONSABLE],[GRUPOSTATUS],[DATECREATED],[CODECREATED]) " &
                        " VALUES (@p1,@p2,@p3,@p4,@p5,@p6) "
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _gruponame)
                        .Parameters.AddWithValue("@p2", _gruporesponsable)
                        .Parameters.AddWithValue("@p3", _grupoemail)
                        .Parameters.AddWithValue("@p4", _grupostatus)
                        .Parameters.AddWithValue("@p5", _datecreated)
                        .Parameters.AddWithValue("@p6", _codecreated)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
            Response.Redirect("Contratistas.aspx")
        Catch ex As Exception
            Response.Write("CreateContratista " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateContratista(_responsablegrupo As String, _emailresponsable As String, _grupostatus As String, _lastupdate As Date, _lastuserupdate As String, _idgrupo As String, _gruponame As String)
        Try
            Dim sCon As String = sCon1
            'Dim selContratistaUpdate As String

            If txtUpdateContratistaName.Text.Length = 0 Then
                selContratistaUpdate = " UPDATE [dbo].[GRUPOS] " &
                    " SET [GRUPOEMAILRESPONSABLE] = @p2 " &
                    "    ,[GRUPOSTATUS] = @p3 " &
                    "    ,[LASTUPDATE] = @p4 " &
                    "    ,[LASTUSERUPDATE] = @p5 " &
                    " WHERE [ID]=@p6 and [GRUPONAME]=@p7 "
            End If

            If txtUpdateContratistaEmail.Text.Length = 0 Then
                selContratistaUpdate = " UPDATE [dbo].[GRUPOS] " &
                        " SET [GRUPORESPONSABLE] = @p1 " &
                        "    ,[GRUPOSTATUS] = @p3 " &
                        "    ,[LASTUPDATE] = @p4 " &
                        "    ,[LASTUSERUPDATE] = @p5 " &
                        " WHERE [ID]=@p6 and [GRUPONAME]=@p7 "
            End If
            If txtUpdateContratistaName.Text.Length = 0 And txtUpdateContratistaEmail.Text.Length = 0 Then
                selContratistaUpdate = " UPDATE [dbo].[GRUPOS] " &
                    " SET [GRUPOSTATUS] = @p3 " &
                    "    ,[LASTUPDATE] = @p4 " &
                    "    ,[LASTUSERUPDATE] = @p5 " &
                    " WHERE [ID]=@p6 and [GRUPONAME]=@p7 "
            End If

            If txtUpdateContratistaName.Text.Length > 0 And txtUpdateContratistaEmail.Text.Length > 0 Then
                selContratistaUpdate = " UPDATE [dbo].[GRUPOS] " &
                    " SET [GRUPORESPONSABLE] = @p1,GRUPORESPONSABLE" &
                    "    ,[GRUPOEMAILRESPONSABLE] = @p2 " &
                    "    ,[GRUPOSTATUS] = @p3 " &
                    "    ,[LASTUPDATE] = @p4 " &
                    "    ,[LASTUSERUPDATE] = @p5 " &
                    " WHERE [ID]=@p6 and [GRUPONAME]=@p7 "
            End If

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(selContratistaUpdate, con)
                cmd.Parameters.AddWithValue("@p1", _responsablegrupo)
                cmd.Parameters.AddWithValue("@p2", _emailresponsable)
                cmd.Parameters.AddWithValue("@p3", _grupostatus)
                cmd.Parameters.AddWithValue("@p4", _lastupdate)
                cmd.Parameters.AddWithValue("@p5", _lastuserupdate)
                cmd.Parameters.AddWithValue("@p6", _idgrupo)
                cmd.Parameters.AddWithValue("@p7", _gruponame)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Response.Redirect("Contratistas.aspx")
        Catch ex As Exception
            Response.Write("UpdateContratista " & ex.Message)

        End Try
    End Sub
    Public Sub CreateMecacnico(_mecanicogrupoid As String, _mecaniconame As String, _emailmecanico As String, _estatusmecanico As String, _datecreated As Date, _codecreated As String)
        Try
            Dim query As String = String.Empty
            query &= " INSERT INTO [dbo].[MECANICOS] ([GRUPOID],[MECANICONAME],[MECANICOEMAIL],[MECANICOESTATUS],[DATECREATED],[CODECREATED]) " &
                     " VALUES (@p1,@p2,@p3,@p4,@p5,@p6) "
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _mecanicogrupoid)
                        .Parameters.AddWithValue("@p2", _mecaniconame)
                        .Parameters.AddWithValue("@p3", _emailmecanico)
                        .Parameters.AddWithValue("@p4", _estatusmecanico)
                        .Parameters.AddWithValue("@p5", _datecreated)
                        .Parameters.AddWithValue("@p6", _codecreated)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
            Response.Redirect("Contratistas.aspx")
        Catch ex As Exception
            Response.Write("CreateMecacnico " & ex.Message)
        End Try
    End Sub
    Public Sub UpdatMecanico(_mecagrupoid As String, _mecaname As String, _mecaemail As String, _mecastatus As String, _lastupdate As Date, _lastuserupdate As String, _mecaid As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "UPDATE [ArmadoMotos].[dbo].[MECANICOS] " &
                    " Set [GRUPOID] = @p1 " &
                    " ,[MECANICONAME] = @p2 " &
                    " ,[MECANICOEMAIL] = @p3 " &
                    " ,[MECANICOESTATUS] = @p4 " &
                    " ,[LASTUPDATE] = @p5 " &
                    " ,[LASTUSERUPDATE] = @p6 " &
                    " WHERE [ID] =@p7"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _mecagrupoid)
                cmd.Parameters.AddWithValue("@p2", _mecaname)
                cmd.Parameters.AddWithValue("@p3", _mecaemail)
                cmd.Parameters.AddWithValue("@p4", _mecastatus)
                cmd.Parameters.AddWithValue("@p5", _lastupdate)
                cmd.Parameters.AddWithValue("@p6", _lastuserupdate)
                cmd.Parameters.AddWithValue("@p7", _mecaid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Response.Redirect("Contratistas.aspx")
        Catch ex As Exception
            Response.Write("UpdatMecanico " & ex.Message)
        End Try
    End Sub

    Protected Sub btnActualizarContratista_Click(sender As Object, e As EventArgs) Handles btnActualizarContratista.Click
        UpdateContratista(txtUpdateContratistaName.Text, txtUpdateContratistaEmail.Text, drpUpdateEstadoContratista.SelectedValue.ToString, Date.Now, Session("UserCode").ToString, txtUpdateContratistaID.Text, txtUpdateContratistaCode.text)
        'Session("UserCode").ToString
    End Sub
    Protected Sub btnAGegarMecanico_Click(sender As Object, e As EventArgs) Handles btnAGegarMecanico.Click
        CreateMecacnico(drpGrupoContratistas.SelectedValue.ToString, txtNMecanico.Text, txtEmailMeca.Text, "True", Date.Now, Session("UserCode").ToString)
    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        'ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Estas Seguro mi Dog???');", True)
        UpdatMecanico(drpUpdateGrupoMeca.SelectedValue.ToString, txtUpdateMecaName.Text, txtUpdateMecaEmail.Text, drpUpdateEstadoMeca.SelectedValue.ToString, Date.Now, Session("UserCode").ToString, txtUpdateMecanicoID.Text)
    End Sub
End Class

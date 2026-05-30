Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Partial Class Usuarios
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
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
                    Case "Administrador"
                        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                        Dim gvRow As GridViewRow = GridView1.Rows(index)
                        pnlUpdate.Visible = True
                        pnlGridUsuarios.Visible = False
                        txtUpdateUserID.Text = GridView1.Rows(index).Cells(1).Text
                        txtUpdateUserCode.Text = GridView1.Rows(index).Cells(2).Text
                        txtUpdateUserName.Text = GridView1.Rows(index).Cells(3).Text
                        txtUpdateUSerEmail.Text = GridView1.Rows(index).Cells(4).Text
                End Select
            End If
        Catch ex As Exception
            ' ADD_LOG("GridView1_RowCommand", ex.Message)
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
                Using cmd As New SqlCommand("select [ID],[USERCODE],[USERNAME],[USEREMAIL],[USERROL],[USERESTATUS] from [ArmadoMotos].[DBO].[USUARIOS] with(nolock) ")
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
            'Response.Write("CargarTareasPendientes BindGrid " & ex.Message)
        End Try
    End Sub

    Private Sub Usuarios_Load(sender As Object, e As EventArgs) Handles Me.Load
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
    Protected Sub btnIngreso_Click(sender As Object, e As EventArgs) Handles btnIngreso.Click
        pnlGridUsuarios.Visible = False
        pnlCrearUsuario.Visible = True
    End Sub
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("Usuarios.aspx")
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CreateUser(txtUserCode.Text, txtUserName.Text, txtEmail.Text, drpRolUsuario.SelectedItem.ToString, "True", Date.Now, Session("UserCode").ToString, txtUserPass.Text)
        Response.Redirect("Usuarios.aspx")
    End Sub
    Public Sub CreateUser(_usercode As String, _username As String, _usermail As String, _userrol As String, _userstatus As String, _datecreated As String, _codecreated As String, _userpass As String)
        Try
            Dim query As String = String.Empty
            query &= " INSERT INTO [dbo].[USUARIOS]([USERCODE],[USERNAME],[USEREMAIL],[USERROL],[USERESTATUS],[DATECREATED],[CODECREATED],[USERPASS])" &
                     " VALUES (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8) "
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _usercode)
                        .Parameters.AddWithValue("@p2", _username)
                        .Parameters.AddWithValue("@p3", _usermail)
                        .Parameters.AddWithValue("@p4", _userrol)
                        .Parameters.AddWithValue("@p5", _userstatus)
                        .Parameters.AddWithValue("@p6", _datecreated)
                        .Parameters.AddWithValue("@p7", _codecreated)
                        .Parameters.AddWithValue("@p8", _userpass)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("Usuarios.aspx")
    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        UpdateUsuario(txtUpdateUserPass.Text, txtUpdateUserName.Text, txtUpdateUSerEmail.Text, Date.Now, Session("UserCode").ToString, txtUpdateUserID.Text)
    End Sub
    Public Sub UpdateUsuario(_userpass As String, _username As String, _useremail As String, _lastupdate As Date, _lastuserupdate As String, _userid As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "UPDATE [dbo].[USUARIOS] " &
                   " SET [USERPASS] = @p1 " &
                   " ,[USERNAME] = @p2 " &
                   " ,[USEREMAIL] = @p3 " &
                   " ,[LASTUPDATE] = @p4 " &
                   " ,[LASTUSERUPDATE] = @p5 " &
                   " WHERE [ID]=@p6"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _userpass)
                cmd.Parameters.AddWithValue("@p2", _username)
                cmd.Parameters.AddWithValue("@p3", _useremail)
                cmd.Parameters.AddWithValue("@p4", _lastupdate)
                cmd.Parameters.AddWithValue("@p5", _lastuserupdate)
                cmd.Parameters.AddWithValue("@p6", _userid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Response.Redirect("Usuarios.aspx")
        Catch ex As Exception
            Response.Write("UpdateUsuario " & ex.Message)
        End Try
    End Sub
End Class

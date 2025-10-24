Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web

Partial Class MenuParametrizaciones
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Private Sub MenuParametrizaciones_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    CantidadUsuariosLabel()
                    CantidadTrabajosADLabel()
                    CantidadModelosPrecioLabel()
                    CantidadModelosGrupos()
                    CantidadMecanicos()

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
    Public Sub CantidadUsuariosLabel()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select Count(1) [Cantidad] from [ArmadoMotos].[DBO].[USUARIOS] with(nolock) "

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblCantUsuarios.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub CantidadTrabajosADLabel()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select Count(1) [Cantidad] from [ArmadoMotos].[DBO].[TRABAJOS]"

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblCantTrabajosAD.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    'Public Sub CantidadModelosLabel()
    '    Try
    '        Dim dt As New DataTable
    '        Dim _con As New SqlConnection(sCon1)
    '        Dim Consulta As String = "select Count(1) [Cantidad] from [ArmadoMotos].[DBO].[MODELOS] with(nolock) "

    '        Dim Comando As New SqlCommand(Consulta, _con)
    '        Dim drd As SqlDataReader
    '        _con.Open()
    '        drd = Comando.ExecuteReader()
    '        If drd.Read() Then
    '            lblCantModelos.Text = drd.Item("Cantidad").ToString
    '        End If
    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try
    'End Sub
    Public Sub CantidadModelosPrecioLabel()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select Count(1) [Cantidad] from [ArmadoMotos].[DBO].[PRECIOS] with(nolock) "

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblModeloPrecio.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CantidadModelosGrupos()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select Count(1) [Cantidad] from [ArmadoMotos].[DBO].[GRUPOS] with(nolock) "

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblContratistas.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CantidadMecanicos()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select Count(1) [Cantidad] from [ArmadoMotos].[DBO].[Mecanicos] with(nolock) "

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblMecanicos.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class

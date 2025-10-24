Imports System.Data
Imports System.Data.SqlClient
Partial Class MainDashBoard
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Private Sub MainDashBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else

                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
                    End If

                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    CantidadModelosPrecioLabel()
                    CantidadMotosDesarmadas()
                    CantidadControlCalidad()
                    CantidadMotosDisponibles()
                    CantidadMotosNoDisponibles()
                    GetData(DatePart(DateInterval.Year, Date.Now), DatePart(DateInterval.Month, Date.Now))
                End If
            End If
        Catch ex As Exception
            Response.Write("MainDashBoard_Load " & ex.Message)
        End Try
    End Sub

    Public Sub CantidadModelosPrecioLabel()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = "select Count(1) [Cantidad] from [DBO].[ARMADOMOTOS] with(nolock) where CANCELED='N' AND [ESTATUS] IN ('Asignada','Proceso','Reproceso')"

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblMotosEnProceso.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub CantidadMotosDesarmadas()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select count(1) as Cantidad from osri with(nolock) where isnull(U_VIN,'-')<>'N' and U_Estado_Produccion='01' AND WHSCODE='DCM00' and Status=0"

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblMotosEnCaja.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CantidadControlCalidad()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = "select Count(1) [Cantidad] from [DBO].[ARMADOMOTOS] with(nolock) where [ESTATUS] IN ('Calidad')"

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblControlCalidad.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CantidadMotosDisponibles()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select count(1) as Cantidad from osri with(nolock) where U_Estado_Produccion='04' and Status=0 and WhsCode='DCM00'"

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblMotosDisponibles.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CantidadMotosNoDisponibles()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select count(1) as Cantidad from osri with(nolock) where U_Estado_Produccion='05'"

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtMotosNoDisponibles.Text = drd.Item("Cantidad").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim exc As Exception = Server.GetLastError()

        If TypeOf exc Is HttpUnhandledException Then
            Response.Redirect("Default.aspx", True)
        End If
    End Sub
    Private Sub GetData(year As Integer, month As Integer)
        Try
            Dim table As DataTable = New DataTable()

            Using conn As SqlConnection = New SqlConnection(sCon2)
                Dim sql As String = "EXEC PIVOT_ARMADO_MENSUAL " & year & "," & month & ""

                Using cmd As SqlCommand = New SqlCommand(sql, conn)

                    Using ad As SqlDataAdapter = New SqlDataAdapter(cmd)
                        ad.Fill(table)
                    End Using
                End Using
                conn.Close()
            End Using

            GridView1.DataSource = table
            GridView1.DataBind()
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception

        End Try
    End Sub
End Class

Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports DocumentFormat.OpenXml.Wordprocessing

Partial Class InformesProduccion
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
    'Public Sub _CargarModelosPrecios()
    '    Try
    '        Dim dt As DataTable = New DataTable()
    '        Using conn As SqlConnection = New SqlConnection(sCon1)
    '            Dim query As String = "set language  'Spanish' " &
    '            " select distinct rtrim(convert(char,datepart(year,SECONDUPDATEDATE))) + ',' + rtrim(convert(char,DATEPART(month,SECONDUPDATEDATE))) [DateString] " &
    '            ", rtrim(Convert(Char, datepart(year,SECONDUPDATEDATE))) + ' - ' + rtrim(convert(char,datename(month,SECONDUPDATEDATE))) [Periodo] " &
    '            " from armadomotos " &
    '            "where DATEPART(month,SECONDUPDATEDATE) is not null"
    '            Dim cmd As SqlCommand = New SqlCommand(query, conn)
    '            Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
    '            da.Fill(dt)
    '        End Using
    '        drpDesde.Dispose()
    '        drpDesde.DataTextField = "Periodo"
    '        drpDesde.DataValueField = "DateString"
    '        drpDesde.DataSource = dt
    '        drpDesde.DataBind()

    '        drpHasta.Dispose()
    '        drpHasta.DataTextField = "Periodo"
    '        drpHasta.DataValueField = "DateString"
    '        drpHasta.DataSource = dt
    '        drpHasta.DataBind()

    '    Catch ex As Exception
    '        'ADD_LOG("_CargarAlmacenes", ex.Message)
    '        Response.Write(ex.Message)
    '    End Try
    'End Sub

    Private Sub InformesProduccion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    '_CargarModelosPrecios()
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    'Private Sub GetData(dRangeOne As String, dRangeTwo As String)
    '    Try
    '        Dim table As DataTable = New DataTable()

    '        Using conn As SqlConnection = New SqlConnection(sCon1)
    '            Dim sql As String = "EXEC [PIVOT_ARMADO_CONSULTA] '" & dRangeOne & "','" & dRangeTwo & "'"

    '            Using cmd As SqlCommand = New SqlCommand(sql, conn)

    '                Using ad As SqlDataAdapter = New SqlDataAdapter(cmd)
    '                    ad.Fill(table)
    '                End Using
    '            End Using

    '            GridView1.DataSource = table
    '            GridView1.DataBind()
    '            GridView1.UseAccessibleHeader = True
    '            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
    '            conn.Close()
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("GetData " & ex.Message)
    '    End Try
    'End Sub
    'Private Sub GetDataCalidad(dRangeOne As String, dRangeTwo As String)
    '    Try
    '        Dim table As DataTable = New DataTable()

    '        Using conn As SqlConnection = New SqlConnection(sCon1)
    '            Dim sql As String = "EXEC [PIVOT_ARMADO_CONSULTA_CALIDAD] '" & dRangeOne & "','" & dRangeTwo & "'"

    '            Using cmd As SqlCommand = New SqlCommand(sql, conn)

    '                Using ad As SqlDataAdapter = New SqlDataAdapter(cmd)
    '                    ad.Fill(table)
    '                End Using
    '            End Using

    '            GridView1.DataSource = table
    '            GridView1.DataBind()
    '            GridView1.UseAccessibleHeader = True
    '            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
    '            conn.Close()
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("GetData " & ex.Message)
    '    End Try
    'End Sub

    'Private Sub GetDataDetalle(dRangeOne As String, dRangeTwo As String)
    '    Try
    '        Dim table As DataTable = New DataTable()

    '        Using conn As SqlConnection = New SqlConnection(sCon1)
    '            Dim sql As String = "EXEC [PIVOT_ARMADO_CONSULTA_DETALLE] '" & dRangeOne & "','" & dRangeTwo & "'"

    '            Using cmd As SqlCommand = New SqlCommand(sql, conn)

    '                Using ad As SqlDataAdapter = New SqlDataAdapter(cmd)
    '                    ad.Fill(table)
    '                End Using
    '            End Using

    '            GridView1.DataSource = table
    '            GridView1.DataBind()
    '            GridView1.UseAccessibleHeader = True
    '            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
    '            conn.Close()
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("GetData " & ex.Message)
    '    End Try
    'End Sub
    'Private Sub GetDataCalidadDetalle(dRangeOne As String, dRangeTwo As String)
    '    Try
    '        Dim table As DataTable = New DataTable()

    '        Using conn As SqlConnection = New SqlConnection(sCon1)
    '            Dim sql As String = "EXEC [PIVOT_ARMADO_CONSULTA_CALIDAD_DETALLE] '" & dRangeOne & "','" & dRangeTwo & "'"

    '            Using cmd As SqlCommand = New SqlCommand(sql, conn)

    '                Using ad As SqlDataAdapter = New SqlDataAdapter(cmd)
    '                    ad.Fill(table)
    '                End Using
    '            End Using

    '            GridView1.DataSource = table
    '            GridView1.DataBind()
    '            GridView1.UseAccessibleHeader = True
    '            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
    '            conn.Close()
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("GetData " & ex.Message)
    '    End Try
    'End Sub
    'Protected Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
    '    GetData(txtDesde.Text, txtHasta.Text)
    'End Sub

    'Private Sub btnCargarCalidad_Click(sender As Object, e As EventArgs) Handles btnCargarCalidad.Click
    '    GetDataCalidad(txtDesde.Text, txtHasta.Text)
    'End Sub

    'Private Sub btnDetalleProduccion_Click(sender As Object, e As EventArgs) Handles btnDetalleProduccion.Click
    '    GetDataDetalle(txtDesde.Text, txtHasta.Text)
    'End Sub

    'Private Sub btnDetalleCalidad_Click(sender As Object, e As EventArgs) Handles btnDetalleCalidad.Click
    '    GetDataCalidadDetalle(txtDesde.Text, txtHasta.Text)
    'End Sub
End Class

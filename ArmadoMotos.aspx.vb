
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System.Configuration
Imports System.Web.Services
Partial Class ArmadoMotos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    'Protected Sub btnCargarDatos_Click(sender As Object, e As EventArgs) Handles btnCargarDatos.Click
    '    BuscarSerie(Session("IdMecanico").ToString)
    'End Sub
    Public Sub BuscarSerie(_Seriemoto As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " SELECT SERIE,SERIEM,ITEMCODE,ITENMANE,MARCA,MODELO,MOTOR " &
                                      " ,YEAR,CILINDROS,COLOR,ESTATUS" &
                                      " FROM dbo.ARMADOMOTOS with(nolock) where SERIE='" & _Seriemoto & "' "
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                'lblItemcode.Text = drd.Item("ITEMCODE").ToString
                'lblItemname.Text = drd.Item("ITENMANE").ToString
                'lblModelo.Text = drd.Item("MODELO").ToString
                'lblYear.Text = drd.Item("YEAR").ToString
                'lblColor.Text = drd.Item("COLOR").ToString
                'lblMarca.Text = drd.Item("MARCA").ToString
                'lblCilindros.Text = drd.Item("CILINDROS").ToString
                'lblSerie.Text = drd.Item("SERIE").ToString
                'lblSerieMotor.Text = drd.Item("SERIEM").ToString
                'lblMotorM.Text = drd.Item("MOTOR").ToString

                'lblestatusMoto.Text = drd.Item("ESTATUS").ToString

                'If drd.Item("ESTATUS").ToString = "Proceso" Then
                '    btnIniciarArmado.Enabled = False
                'End If

                'Select Case drd.Item("ESTATUS").ToString
                '    Case "Asignada"
                '        btnIniciarArmado.Enabled = True
                '        btnTerminarArmado.Enabled = False
                '    Case "Proceso"
                '        btnIniciarArmado.Enabled = False
                '        btnTerminarArmado.Enabled = True

                'End Select
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub BuscarMOtoAsignadaOne()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " SELECT top 1 [ID],[SERIE],[MODELO],[COLOR],[DATECREATED],[ESTATUS],(select [MECANICOCODE] from [dbo].[MECANICOS] where id=MECANICOASIGNADO) [MECANICOASIGNADO],[FIRSTUPDATEDATE] " &
                                        " ,[CANCELED] FROM [dbo].[ARMADOMOTOS] where [ESTATUS] IN ('Proceso','Asignada','Reproceso') and [MECANICOASIGNADO]=" & Session("IdMecanico").ToString & " ORDER BY ID DESC"
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblSerie01.Text = drd.Item("SERIE").ToString
                lblModelo01.Text = drd.Item("MODELO").ToString
                lblColor01.Text = drd.Item("COLOR").ToString

                Select Case drd.Item("ESTATUS").ToString
                    Case "Proceso"
                        btnIniciar01.Visible = False
                        btnStop01.Visible = True
                    Case "Asignada"
                        btnIniciar01.Visible = True
                        btnStop01.Visible = False
                    Case "Reproceso"
                        btnIniciar01.Visible = True
                        btnStop01.Visible = False
                End Select
            End If
            _con.Close()
        Catch ex As Exception
        End Try
    End Sub

    Public Sub BuscarMOtoAsignadaTwo()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " SELECT top 1 [ID],[SERIE],[MODELO],[COLOR],[DATECREATED],[ESTATUS],(select [MECANICOCODE] from [dbo].[MECANICOS] where id=MECANICOASIGNADO) [MECANICOASIGNADO],[FIRSTUPDATEDATE] " &
                                        " ,[CANCELED] FROM [dbo].[ARMADOMOTOS] where [ESTATUS] IN ('Proceso','Asignada','Reproceso') and SERIE not in ('" & lblSerie01.Text & "') and [MECANICOASIGNADO]=" & Session("IdMecanico").ToString & " ORDER BY ID DESC"
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblSerie02.Text = drd.Item("SERIE").ToString
                lblModelo02.Text = drd.Item("MODELO").ToString
                lblColor02.Text = drd.Item("COLOR").ToString

                Select Case drd.Item("ESTATUS").ToString
                    Case "Proceso"
                        btnIniciar02.Visible = False
                        btnStop02.Visible = True
                    Case "Asignada"
                        btnIniciar02.Visible = True
                        btnStop02.Visible = False
                    Case "Reproceso"
                        btnIniciar02.Visible = True
                        btnStop02.Visible = False
                End Select
            End If
            _con.Close()
        Catch ex As Exception
        End Try
    End Sub

    Public Sub BuscarMOtoAsignadaThree()
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " SELECT top 1 [ID],[SERIE],[MODELO],[COLOR],[DATECREATED],[ESTATUS],(select [MECANICOCODE] from [dbo].[MECANICOS] where id=MECANICOASIGNADO) [MECANICOASIGNADO],[FIRSTUPDATEDATE] " &
                                        " ,[CANCELED] FROM [dbo].[ARMADOMOTOS] where [ESTATUS] IN ('Proceso','Asignada','Reproceso') and SERIE not in ('" & lblSerie01.Text & "','" & lblSerie02.Text & "') and [MECANICOASIGNADO]=" & Session("IdMecanico").ToString & " ORDER BY ID DESC"
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblSerie03.Text = drd.Item("SERIE").ToString
                lblModelo03.Text = drd.Item("MODELO").ToString
                lblColor03.Text = drd.Item("COLOR").ToString

                Select Case drd.Item("ESTATUS").ToString
                    Case "Proceso"
                        btnIniciar03.Visible = False
                        btnStop03.Visible = True
                    Case "Asignada"
                        btnIniciar03.Visible = True
                        btnStop03.Visible = False
                    Case "Reproceso"
                        btnIniciar03.Visible = True
                        btnStop03.Visible = False
                End Select
            End If
            _con.Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ArmadoMotos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    GetDataCount()
                    Select Case Session("Position").ToString()
                        Case "Iniciador"
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
    Public Sub UpdateUsuario(_estado As String, _idmecanico As String, _timestamp As Date, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " SET [ESTATUS]=@p1" &
                    " ,[FIRSTUPDATEUSER]=@p2" &
                    " ,[FIRSTUPDATEDATE]=@p3" &
                    " WHERE [SERIE]=@p4"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _estado)
                cmd.Parameters.AddWithValue("@p2", _idmecanico)
                cmd.Parameters.AddWithValue("@p3", _timestamp)
                cmd.Parameters.AddWithValue("@p4", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Response.Redirect("ArmadoMotos.aspx")
        Catch ex As Exception
            Response.Write("UpdateUsuario " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateFinArmado(_estadoarmado As String, _usuariofinarmado As String, _fechafinarmado As Date, _campocontrol1 As String, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " SET [ESTATUS]=@p1 " &
                    " ,[SECONDUPDATEUSER]=@p2 " &
                    " ,[SECONDUPDATEDATE]=@p3 " &
                    " ,[CONTROL1]=@p4 " &
                    " WHERE [SERIE]=@p5"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _estadoarmado)
                cmd.Parameters.AddWithValue("@p2", _usuariofinarmado)
                cmd.Parameters.AddWithValue("@p3", _fechafinarmado)
                cmd.Parameters.AddWithValue("@p4", _campocontrol1)
                cmd.Parameters.AddWithValue("@p5", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Response.Redirect("ArmadoMotos.aspx")
        Catch ex As Exception
            Response.Write("UpdateFinArmado " & ex.Message)
        End Try
    End Sub

    Private Sub GetDataCount()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT top 3 [ID],[SERIE],[MODELO],[COLOR],[DATECREATED],[ESTATUS],(select [MECANICOCODE] from [dbo].[MECANICOS] where id=MECANICOASIGNADO) [MECANICOASIGNADO],[FIRSTUPDATEDATE] " &
                                                " ,[CANCELED] FROM [dbo].[ARMADOMOTOS] where [ESTATUS] IN ('Proceso','Asignada','Reproceso') and [MECANICOASIGNADO]=" & Session("IdMecanico").ToString & " ")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            Select Case dt.Rows.Count.ToString
                                Case "1"
                                    pnlMoto01.Visible = True
                                    BUscarMOtoAsignadaOne()
                                Case "2"
                                    pnlMoto01.Visible = True
                                    pnlMoto02.Visible = True
                                    BUscarMOtoAsignadaOne()
                                    BUscarMOtoAsignadaTwo()

                                Case "3"
                                    pnlMoto01.Visible = True
                                    pnlMoto02.Visible = True
                                    pnlMoto03.Visible = True
                                    BuscarMOtoAsignadaOne()
                                    BuscarMOtoAsignadaTwo()
                                    BuscarMOtoAsignadaThree()
                            End Select
                        End Using
                        cmd.Connection.Close()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Response.Write("GetDataCount " & ex.Message)
        End Try

    End Sub

    'Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
    '    Try
    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            Select Case e.Row.Cells(7).Text
    '                Case "Reproceso"
    '                    e.Row.Cells(7).BackColor = System.Drawing.Color.Red
    '                    e.Row.Cells(7).ForeColor = System.Drawing.Color.White
    '                Case "Asignada"
    '                    e.Row.Cells(7).BackColor = System.Drawing.Color.Coral
    '                    e.Row.Cells(7).ForeColor = System.Drawing.Color.White
    '                Case "Proceso"
    '                    e.Row.Cells(7).BackColor = System.Drawing.Color.Orange
    '                    e.Row.Cells(7).ForeColor = System.Drawing.Color.White
    '                Case "Calidad"
    '                    e.Row.Cells(7).BackColor = System.Drawing.Color.Green
    '                    e.Row.Cells(7).ForeColor = System.Drawing.Color.White
    '            End Select
    '            '    If e.Row.Cells(7).Text = "Reproceso" Then
    '            '        e.Row.Cells(7).BackColor = System.Drawing.Color.Red
    '            '        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
    '            '        'ElseIf e.Row.Cells(0).Text = "close" Then
    '            '        '    e.Row.Cells(0).ForeColor = System.Drawing.Color.Black
    '            '        'Else
    '            '        '    e.Row.Cells(0).ForeColor = System.Drawing.Color.Green
    '            '    End If
    '        End If
    '    Catch ex As Exception
    '        Response.Write("GridView1_RowDataBound " & ex.Message)
    '    End Try
    'End Sub

    'Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
    '    Try
    '        If e.CommandName = "Modificar" Then
    '            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '            Dim gvRow As GridViewRow = GridView1.Rows(index)
    '            pnlTareaIndividual.Visible = True
    '            pnlGridUsuarios.Visible = False
    '            BuscarSerie(GridView1.Rows(index).Cells(2).Text)
    '            'CreateCCHistory(GridView1.Rows(index).Cells(3).Text, Date.Now, GridView1.Rows(index).Cells(4).Text, GridView1.Rows(index).Cells(10).Text, "1", "0", "0", Session("User").ToString, "Control de Calidad Pasado", "0", "0")
    '            'UpdateMotoEstatusMainTable("Armada", "ARMADA", "PP", Session("User").ToString, Date.Now, "Control de Calidad Pasado", GridView1.Rows(index).Cells(4).Text)
    '            'Response.Write("<script>alert('Infromacion Almacenada');</script>")
    '        End If
    '        'If e.CommandName = "No" Then
    '        '    pnlUpdate.Visible = True
    '        '    pnlGridControlCalidad.Visible = False
    '        '    Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '        '    Dim gvRow As GridViewRow = GridView1.Rows(index)
    '        '    txtUpdateIdArmado.Text = GridView1.Rows(index).Cells(3).Text
    '        '    txtUpdateSerial.Text = GridView1.Rows(index).Cells(4).Text
    '        '    txtUpdateDefecto.Text = "Reproceso"
    '        '    txtUpdateMecanicoCode.Text = GridView1.Rows(index).Cells(10).Text
    '        '    drpUpdateTipoProblema.Visible = False
    '        '    Session("Reproceso") = "SI"
    '        '    Session("Defectuoso") = "NO"

    '        'End If
    '        'If e.CommandName = "Defectuoso" Then
    '        '    pnlUpdate.Visible = True
    '        '    pnlGridControlCalidad.Visible = False
    '        '    Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '        '    Dim gvRow As GridViewRow = GridView1.Rows(index)
    '        '    txtUpdateIdArmado.Text = GridView1.Rows(index).Cells(3).Text
    '        '    txtUpdateSerial.Text = GridView1.Rows(index).Cells(4).Text
    '        '    txtUpdateDefecto.Text = "Desperfecto"
    '        '    txtUpdateMecanicoCode.Text = GridView1.Rows(index).Cells(10).Text
    '        '    Session("Reproceso") = "NO"
    '        '    Session("Defectuoso") = "SI"
    '        'End If
    '    Catch ex As Exception
    '        Response.Write("GridView1_RowCommand " & ex.Message)
    '    End Try
    'End Sub

    'Public Sub btnMOtoASignada1_ServerClick(sender As Object, e As EventArgs) Handles btnMOtoASignada1.ServerClick
    '    Response.Write("<script>alert('MOto 1');</script>")
    'End Sub
    'Public Sub btnMOtoASignada2_ServerClick(sender As Object, e As EventArgs) Handles btnMOtoASignada2.ServerClick
    '    Response.Write("<script>alert('Moto 2');</script>")
    'End Sub
    'Public Sub btnMOtoASignada3_ServerClick(sender As Object, e As EventArgs) Handles btnMOtoASignada3.ServerClick
    '    Response.Write("<script>alert('Moto 3');</script>")
    'End Sub

    Protected Sub btnIniciar01_Click(sender As Object, e As ImageClickEventArgs) Handles btnIniciar01.Click
        UpdateUsuario("Proceso", Session("IdMecanico").ToString, Date.Now, lblSerie01.Text)
    End Sub
    Protected Sub btnStop01_Click(sender As Object, e As ImageClickEventArgs) Handles btnStop01.Click
        UpdateFinArmado("Calidad", Session("IdMecanico").ToString, Date.Now, "ARMADA", lblSerie01.Text)
    End Sub
    Protected Sub btnIniciar02_Click(sender As Object, e As ImageClickEventArgs) Handles btnIniciar02.Click
        UpdateUsuario("Proceso", Session("IdMecanico").ToString, Date.Now, lblSerie02.Text)
    End Sub
    Protected Sub btnStop02_Click(sender As Object, e As ImageClickEventArgs) Handles btnStop02.Click
        UpdateFinArmado("Calidad", Session("IdMecanico").ToString, Date.Now, "ARMADA", lblSerie02.Text)
    End Sub
    Protected Sub btnIniciar03_Click(sender As Object, e As ImageClickEventArgs) Handles btnIniciar03.Click
        UpdateUsuario("Proceso", Session("IdMecanico").ToString, Date.Now, lblSerie03.Text)
    End Sub
    Protected Sub btnStop03_Click(sender As Object, e As ImageClickEventArgs) Handles btnStop03.Click
        UpdateFinArmado("Calidad", Session("IdMecanico").ToString, Date.Now, "ARMADA", lblSerie03.Text)
    End Sub
End Class

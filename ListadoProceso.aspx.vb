Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web

Partial Class ListadoProceso
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Private Sub ListadoProceso_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGrid()
                    Select Case Session("Position").ToString()
                        Case "Iniciador"
                    'Response.Redirect("MainDashBoard.aspx")
                        Case "Calidad"
                            'Response.Redirect("MainDashBoard.aspx")
                    End Select
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.BindGrid()
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[SERIE],[MODELO],[COLOR],[DATECREATED],[ESTATUS],(select [MECANICONAME] from [dbo].[MECANICOS] where id=MECANICOASIGNADO) [MECANICOASIGNADO],[FIRSTUPDATEDATE] " &
                                            " ,[CANCELED],[REPROCESOINICIO],[OBSERVACIONES3] FROM [dbo].[ARMADOMOTOS] where CANCELED='N' and  [ESTATUS] IN ('Proceso','Asignada','Reproceso')")
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
            'Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub
    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Select Case e.Row.Cells(7).Text
                    Case "Reproceso"
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Red
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                    Case "Asignada"
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Coral
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                        e.Row.Cells(9).Enabled = False
                    Case "Proceso"
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Orange
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                        e.Row.Cells(0).Enabled = False
                    Case "Calidad"
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Green
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                End Select

            End If
        Catch ex As Exception
            Response.Write("GridView1_RowDataBound " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateInicioArmado(_estado As String, _idmecanico As String, _timestamp As Date, _Serie As String)
        Try
            UpdateOsrnSAPArmado(_Serie, "02")

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
            Response.Redirect("ListadoProceso.aspx")
        Catch ex As Exception
            Response.Write("UpdateInicioArmado " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateInicioReproceso(_estado As String, _idmecanico As String, _timestamp As Date, _Serie As String)
        Try
            UpdateOsrnSAPArmado(_Serie, "02")

            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " SET [ESTATUS]=@p1" &
                    " ,[MECANICOREPROCESO]=@p2" &
                    " ,[REPROCESOINICIO]=@p3" &
                    ", OBSERVACIONES3='Reproceso'" &
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
            Response.Redirect("ListadoProceso.aspx")
        Catch ex As Exception
            Response.Write("UpdateInicioArmado " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateFinArmado(_estadoarmado As String, _usuariofinarmado As String, _fechafinarmado As Date, _campocontrol1 As String, _Serie As String)
        Try
            UpdateOsrnSAPArmado(_Serie, "03")

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
            Response.Redirect("ListadoProceso.aspx")
        Catch ex As Exception
            Response.Write("UpdateFinArmado " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateFinArmadoReproceso(_estadoarmado As String, _usuariofinarmado As String, _fechafinarmado As Date, _campocontrol1 As String, _Serie As String)
        Try
            UpdateOsrnSAPArmado(_Serie, "03")

            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS] " &
                    " SET [ESTATUS]=@p1 " &
                    " ,[SECONDUPDATEUSER]=@p2 " &
                    " ,[REPROCESOFIN]=@p3 " &
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
            Response.Redirect("ListadoProceso.aspx")
        Catch ex As Exception
            Response.Write("UpdateFinArmado " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateOsrnSAPArmado(_SERIE As String, _estado As String)
        Try
            Dim query As String = String.Empty
            query &= "UPDATE OSRN SET U_Estado_Produccion=@p2,U_Estado_Contabilidad='01' WHERE MnfSerial=@p1"
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _SERIE)
                        .Parameters.AddWithValue("@p2", _estado)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("UpdateOsrnSAP " & ex.Message)
        End Try
    End Sub
    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Iniciar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                KardexVehiculo(Date.Now, GridView1.Rows(index).Cells(2).Text, GetMarca(GridView1.Rows(index).Cells(2).Text), GridView1.Rows(index).Cells(3).Text, GridView1.Rows(index).Cells(4).Text, "Inicio Armado", "Inicio Armado", Session("UserCode").ToString, GetMecanicoId(GridView1.Rows(index).Cells(8).Text), 0)

                If GridView1.Rows(index).Cells(7).Text = "Reproceso" Then
                    UpdateInicioReproceso("Proceso", Session("UserCode").ToString, Date.Now, GridView1.Rows(index).Cells(2).Text)
                Else
                    UpdateInicioArmado("Proceso", Session("UserCode").ToString, Date.Now, GridView1.Rows(index).Cells(2).Text)
                End If
            End If
            If e.CommandName = "Terminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                HARMADO(GridView1.Rows(index).Cells(2).Text, Date.Now, Session("UserCode").ToString)
                KardexVehiculo(Date.Now, GridView1.Rows(index).Cells(2).Text, GetMarca(GridView1.Rows(index).Cells(2).Text), GridView1.Rows(index).Cells(3).Text, GridView1.Rows(index).Cells(4).Text, "Fin Armado", "Fin Armado", Session("UserCode").ToString, GetMecanicoId(GridView1.Rows(index).Cells(8).Text), 0)

                If GridView1.Rows(index).Cells(10).Text = "Reproceso" Then
                    UpdateFinArmadoReproceso("Calidad", Session("UserCode").ToString, Date.Now, "ARMADA", GridView1.Rows(index).Cells(2).Text)
                Else
                    UpdateFinArmado("Calidad", Session("UserCode").ToString, Date.Now, "ARMADA", GridView1.Rows(index).Cells(2).Text)
                End If

            End If
        Catch ex As Exception
            Response.Write("HARMADO " & ex.Message)
        End Try
    End Sub
    Public Sub KardexVehiculo(DATELOG As Date, SERIE As String, MARCA As String, MODELO As String, COLOR As String, ACCION As String, COMENTARIOS As String, USUARIO As String, MECANICO As String, CALIDAD As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[KARDEXVEHICULO] ([DATELOG],[SERIE],[MARCA],[MODELO],[COLOR],[ACCION],[COMENTARIOS],[USUARIO],[MECANICO],[CALIDAD]) " &
                    " VALUES(@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", DATELOG)
                cmd.Parameters.AddWithValue("@p2", SERIE)
                cmd.Parameters.AddWithValue("@p3", MARCA)
                cmd.Parameters.AddWithValue("@p4", MODELO)
                cmd.Parameters.AddWithValue("@p5", COLOR)
                cmd.Parameters.AddWithValue("@p6", ACCION)
                cmd.Parameters.AddWithValue("@p7", COMENTARIOS)
                cmd.Parameters.AddWithValue("@p8", USUARIO)
                cmd.Parameters.AddWithValue("@p9", MECANICO)
                cmd.Parameters.AddWithValue("@p10", CALIDAD)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("RetiroDePiezas " & ex.Message)
        End Try
    End Sub
    Public Sub HARMADO(_Serie As String, _datecreated As Date, _usuario As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[HARMADO] ([SERIE],[FECHAINGRESO],[USUARIO]) " &
                    " VALUES(@P1,@P2,@P3)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _Serie)
                cmd.Parameters.AddWithValue("@p2", _datecreated)
                cmd.Parameters.AddWithValue("@p3", _usuario)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("HARMADO " & ex.Message)
        End Try
    End Sub
    Public Function GetMecanicoId(_mecanicoCode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT [ID] FROM [ArmadoMotos].[dbo].[MECANICOS] where [MECANICONAME]=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", _mecanicoCode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Public Function GetMarca(_Serial As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "Select T5.Name [Marca]  " &
                " FROM OITM T0 With(nolock) INNER JOIN OSRI T1 With(nolock) On T0.ItemCode = T1.ItemCode   " &
                " INNER Join [@AMODELO] T2 With(NOLOCK) On T0.U_AMODELO=T2.CODE    " &
                " INNER Join [@AMOTOR] T3 With(NOLOCK) On T0.U_AMOTOR=T3.Code   " &
                " INNER Join [@ACILINDROS] T4 With(NOLOCK) On T0.U_ACILINDROS=T4.Code   " &
                " INNER Join [@AMARCA] T5 With(NOLOCK) On T0.U_AMARCA=T5.CODE   " &
                " INNER Join [@SCOLOR] T6 With(NOLOCK) On T0.U_ACOLOR=T6.CODE   " &
                " WHERE T1.[SuppSerial]=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", _Serial)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
End Class

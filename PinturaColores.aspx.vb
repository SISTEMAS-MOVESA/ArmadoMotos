Imports System.Data
Imports System.Data.SqlClient

Partial Class PinturaColores
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"

    Private Sub PinturaColores_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGrid()
                    BindGridGalones()
                    _CargarColoresPintura()
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
            Response.Write("<script>console.log('PinturaColores_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Dim sqlqry_string As String = "SELECT [ID],[DESCRIPCION],[DATECREATED],[USERCREATED] FROM [ArmadoMotos].[dbo].[PINTURA_COLORES]"

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
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub BindGridGalones()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Dim sqlqry_string As String = "SELECT [ID],[DESCRIPCION],convert(char,FECHACREACIO,103)[FECHACREACIO],[FECHACONSUMO],[DATECREATED],[USERCREATED] FROM [ArmadoMotos].[dbo].[PINTURA_GALONES]"

                Using cmd As New SqlCommand(sqlqry_string)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridGalones.DataSource = dt
                            gridGalones.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridGalones.UseAccessibleHeader = True
            gridGalones.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridGalones: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnCrearColor_Click(sender As Object, e As EventArgs) Handles btnCrearColor.Click
        Try
            CreateColor(txtNuevoColor.Text)
        Catch ex As Exception
            Response.Write("<script>console.log('CreateColor: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub CreateColor(_color As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " INSERT INTO [dbo].[PINTURA_COLORES] ([DESCRIPCION],[DATECREATED],[USERCREATED])" &
                  " VALUES (@p1,@p2,@p3) "
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _color)
                cmd.Parameters.AddWithValue("@p2", Date.Now)
                cmd.Parameters.AddWithValue("@p3", Session("UserCode"))
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Elemento Agregado Con Exito!!!',position: 'topRight',timeout: 5000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                BindGrid()
                BindGridGalones()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('CreateColor: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub _CargarColoresPintura()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "SELECT [ID],[DESCRIPCION] FROM [ArmadoMotos].[dbo].[PINTURA_COLORES]"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using

            drpColorPintura.Dispose()
            drpColorPintura.DataTextField = "DESCRIPCION"
            drpColorPintura.DataValueField = "ID"
            drpColorPintura.DataSource = dt
            drpColorPintura.DataBind()

        Catch ex As Exception
            'ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write("<script>console.log('_CargarColoresPintura: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function ReplaceCharsForFileName(sName As String, sChr As String) As String
        Dim NewStr As String
        sName = Replace(sName, "/", sChr)
        sName = Replace(sName, "\", sChr)
        sName = Replace(sName, ":", sChr)
        sName = Replace(sName, "?", sChr)
        sName = Replace(sName, Chr(34), sChr)
        sName = Replace(sName, "<", sChr)
        sName = Replace(sName, ">", sChr)
        sName = Replace(sName, "|", sChr)
        sName = Replace(sName, "&", sChr)
        sName = Replace(sName, "%", sChr)
        sName = Replace(sName, "*", sChr)
        sName = Replace(sName, "'", sChr)
        sName = Replace(sName, "{", sChr)
        sName = Replace(sName, "[", sChr)
        sName = Replace(sName, "]", sChr)
        sName = Replace(sName, "}", sChr)
        sName = Replace(sName, "!", sChr)
        NewStr = sName
        Return NewStr

    End Function

    Private Sub btnCrearGalon_Click(sender As Object, e As EventArgs) Handles btnCrearGalon.Click
        Try
            CreateGalon(drpColorPintura.SelectedItem.Text, txtFechaCreacion.Text)
        Catch ex As Exception
            Response.Write("<script>console.log('btnCrearGalon_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub CreateGalon(_color As String, _fechacreacion As DateTime)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " INSERT INTO [dbo].[PINTURA_GALONES] ([DESCRIPCION],[FECHACREACIO],[DATECREATED],[USERCREATED])" &
                  " VALUES (@p1,@p2,@p3,@p4) "
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _color)
                cmd.Parameters.AddWithValue("@p2", _fechacreacion)
                cmd.Parameters.AddWithValue("@p3", Date.Now)
                cmd.Parameters.AddWithValue("@p4", Session("UserCode"))
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Elemento Agregado Con Exito!!!',position: 'topRight',timeout: 5000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                BindGrid()
                BindGridGalones()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('CreateGalon: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
End Class

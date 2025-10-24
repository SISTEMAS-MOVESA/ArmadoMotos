Imports System.Data
Imports System.Data.SqlClient
Partial Class Ordenamiento
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"

    Private Sub Ordenamiento_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGrid()
                    cargarModeloMotos()

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
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Dim sqlqry_string As String = "SELECT ID, MODELO, ACTIVOCD, ACTIVOCI, OFERTA, convert(char,DESDE,103) DESDE, convert(char,HASTA,103) HASTA FROM [ArmadoMotos].[dbo].[ORDENAMIENTO] "
                Using cmd As New SqlCommand(sqlqry_string)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridOrdenamiento.DataSource = dt
                            gridOrdenamiento.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridOrdenamiento.UseAccessibleHeader = True
            gridOrdenamiento.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
        sName = Replace(sName, ",", sChr)
        NewStr = sName
        Return NewStr
    End Function
    Public Sub gridOrdenamiento_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridOrdenamiento.RowCommand
        Try
            If e.CommandName = "Modificar" Then
                BindGrid()
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridOrdenamiento.Rows(index)
                txtModalId.Text = gridOrdenamiento.Rows(index).Cells(2).Text
                drpModalModelo.SelectedItem.Text = gridOrdenamiento.Rows(index).Cells(3).Text

                If gridOrdenamiento.Rows(index).Cells(4).Text = "True" Then
                    chkActivoCD.Checked = True
                End If

                If gridOrdenamiento.Rows(index).Cells(5).Text = "True" Then
                    chkActivoCI.Checked = True
                End If

                If gridOrdenamiento.Rows(index).Cells(6).Text = "True" Then
                    chkOferta.Checked = True
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Popup", "$('#ordenamientoModal').modal('show');", True)
            End If
            If e.CommandName = "Eliminar" Then
                BindGrid()
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridOrdenamiento.Rows(index)

                lblIdEliminar.Text = gridOrdenamiento.Rows(index).Cells(2).Text
                lblModelo.Text = gridOrdenamiento.Rows(index).Cells(3).Text
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Popup", "$('#confirmDeleteModal').modal('show');", True)

            End If
        Catch ex As Exception
            Response.Write("gridOrdenamiento_RowCommand " & ex.Message)
        End Try
    End Sub
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        ' Aquí va la lógica para Insertar o Actualizar
    End Sub

    Private Sub btnGuardarModal_Click(sender As Object, e As EventArgs) Handles btnGuardarModal.Click
        Try
            InsertUpdateModeloOrdenamiento(txtModalId.Text, drpModalModelo.SelectedItem.Text,
                                           chkActivoCD.Checked.ToString, chkActivoCI.Checked.ToString, chkOferta.Checked.ToString,
                                           txtModalDesde.Text, txtModalHasta.Text, Session("User"))
        Catch ex As Exception
            Response.Write("<script>console.log('btnGuardarModal_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnAgregarModelo_Click(sender As Object, e As EventArgs) Handles btnAgregarModelo.Click
        Try
            BindGrid()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Popup", "$('#ordenamientoModal').modal('show');", True)

        Catch ex As Exception
            Response.Write("<script>console.log('btnAgregarModelo_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub cargarModeloMotos()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "SELECT Code,Name FROM [movesa].[dbo].[@AMODELO] WHERE U_LG5='Y' order by Name"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using
            drpModalModelo.Dispose()
            drpModalModelo.DataTextField = "Name"
            drpModalModelo.DataValueField = "Code"
            drpModalModelo.DataSource = dt
            drpModalModelo.DataBind()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub InsertUpdateModeloOrdenamiento(headerId As Integer, modelo As String, activoCD As Boolean, activoCI As Boolean, oferta As Boolean, desde As String, hasta As String, usuario As String)
        Try
            Dim sCon As String = sCon1
            Dim SQL_Query As String

            If txtModalId.Text = "0" Then
                SQL_Query = "INSERT INTO [dbo].[ORDENAMIENTO] ([MODELO],[ACTIVOCD],[ACTIVOCI],[OFERTA],[DESDE],[HASTA],[USUARIO],[FECHACREACION]) " &
                        "VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,GETDATE())"
            Else
                SQL_Query = "UPDATE [dbo].[ORDENAMIENTO] SET [MODELO] = @p1, [ACTIVOCD] = @p2, [ACTIVOCI] = @p3, " &
                        "[OFERTA] = @p4, [DESDE] = @p5, [HASTA] = @p6, [USUARIOACT] = @p7, [FECHAMODIFICACION] = GETDATE() " &
                        "WHERE ID = @p8"
            End If

            Using con As New SqlConnection(sCon)
                Using cmd As New SqlCommand(SQL_Query, con)
                    cmd.Parameters.AddWithValue("@p1", modelo)
                    cmd.Parameters.AddWithValue("@p2", activoCD)
                    cmd.Parameters.AddWithValue("@p3", activoCI)
                    cmd.Parameters.AddWithValue("@p4", oferta)

                    If String.IsNullOrEmpty(desde) Then
                        cmd.Parameters.AddWithValue("@p5", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("@p5", Convert.ToDateTime(desde))
                    End If

                    If String.IsNullOrEmpty(hasta) Then
                        cmd.Parameters.AddWithValue("@p6", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("@p6", Convert.ToDateTime(hasta))
                    End If

                    cmd.Parameters.AddWithValue("@p7", usuario)

                    If txtModalId.Text <> "0" Then
                        cmd.Parameters.AddWithValue("@p8", Convert.ToInt32(txtModalId.Text))
                    End If

                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using

            BindGrid()

        Catch ex As Exception
            Response.Write("<script>console.log('Error en InsertUpdateModeloOrdenamiento: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnConfirmDelete_Click(sender As Object, e As EventArgs) Handles btnConfirmDelete.Click
        Try
            Dim sCon As String = sCon1
            Dim SQL_Query As String

            SQL_Query = " DELETE FROM [dbo].[ORDENAMIENTO] where ID = @p1 "

            Using con As New SqlConnection(sCon)
                Using cmd As New SqlCommand(SQL_Query, con)
                    cmd.Parameters.AddWithValue("@p1", lblIdEliminar.Text)
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
            BindGrid()
        Catch ex As Exception
            Response.Write("<script>console.log('btnConfirmDelete_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
End Class

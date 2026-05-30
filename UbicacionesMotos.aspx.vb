Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Class UbicacionesMotos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
    <WebMethod()>
    Public Shared Function InsertarUbicacion(ByVal ubicacion As String) As String
        Try
            Dim usuario As String = ""
            If HttpContext.Current.Session("User") IsNot Nothing Then
                usuario = HttpContext.Current.Session("User").ToString()
            Else
                Return "error: Sesión expirada o usuario no definido."
            End If

            Dim connectionString As String = sCon2
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim cmd As New SqlCommand("INSERT INTO [ArmadoMotos].[dbo].[UBICACIONES] ([UBICACION],[USERCREATED],[DATECREATED]) VALUES (@UBICACION, @USERCREATED, GETDATE())", conn)
                cmd.Parameters.AddWithValue("@UBICACION", ubicacion)
                cmd.Parameters.AddWithValue("@USERCREATED", usuario)
                cmd.ExecuteNonQuery()
            End Using
            Return "ok"
        Catch ex As Exception
            Return "error: " & ex.Message
        End Try
    End Function
    Private Sub UbicacionesMotos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
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
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Dim sqlqry_string As String = "SELECT [ID],[UBICACION],[USERCREATED], " &
                                            " [DATECREATED],[USERUPDATED],[DATEUPDATED] FROM [ArmadoMotos].[dbo].[UBICACIONES]"
                Using cmd As New SqlCommand(sqlqry_string)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridUbicaciones.DataSource = dt
                            gridUbicaciones.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridUbicaciones.UseAccessibleHeader = True
            gridUbicaciones.HeaderRow.TableSection = TableRowSection.TableHeader
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
End Class

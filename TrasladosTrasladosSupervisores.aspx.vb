Imports System.Data
Imports System.Data.SqlClient
Partial Class TrasladosTrasladosSupervisores
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"

    Private Sub TrasladosTrasladosSupervisores_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BindGrid(docentry As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT DocEntry,DocNum,ObjType,Filler,ToWhsCode,CardCode,Comments,GroupNum FROM OWTQ WITH(NOLOCK)  WHERE DocEntry=" & docentry & "")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridEncabezadoTransfer.DataSource = dt
                            gridEncabezadoTransfer.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridEncabezadoTransfer.UseAccessibleHeader = True
            gridEncabezadoTransfer.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>alert('BindGrid : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Protected Function EscapeJavaScriptString(ByVal str As String) As String
        Return str.Replace("'", "\'").Replace("""", "\""")
    End Function
End Class

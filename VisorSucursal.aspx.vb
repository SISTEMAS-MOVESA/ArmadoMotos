Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Partial Class VisorSucursal
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String
    Public SQL_STRING As String

    Private Sub VisorSucursal_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            BindgridSugerido(Request.QueryString("id"))
            lblIdMacro.Text = Request.QueryString("id")
            lblAlmacenDestino.Text = GetWhsName(Request.QueryString("destino"))
            lblRuta.Text = Request.QueryString("ruta")
        Catch ex As Exception
            Response.Write("<script>console.log('VisorSucursal_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindgridSugerido(headerid As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[RUTA],[ALMDESTINO],[ARTICULO],[MODELO],[DESCRIPCION]," &
                                            " [ESPACIOS],[CANTIDAD],[QTYLOGISTICA],[QTYSUCURSAL],([QTYLOGISTICA]-[QTYSUCURSAL])[DIFERENCIA]" &
                                            ",[HEADERID] FROM [dbo].[MACROINSERT] WHERE [HEADERID] =" & headerid & "")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPedidoTemp.DataSource = dt
                            gridPedidoTemp.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridPedidoTemp.UseAccessibleHeader = True
            gridPedidoTemp.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Try
            Response.Redirect("TrasladosDashboard.aspx")
        Catch ex As Exception
            Response.Write("<script>console.log('btnRegresar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Public Function GetWhsName(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT whsname from owhs with(nolock) where whscode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
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
End Class

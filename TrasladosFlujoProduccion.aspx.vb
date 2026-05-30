Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Class TrasladosFlujoProduccion
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
    Private Sub BindGridForklift()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String = " SELECT PRIORIDAD,convert(char,datecreated,103)[FECHA],CAMION,WHSCODEORIGEN," &
                " WHSCODE,WHSNAME,CODIGOCLIENTE,SUM(QTYSOLICITADA)[QTYSOLICITADA],SUM(QTYPREPARADA)[QTYPREPARADA]" &
                " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] where ESTATUS='CAMION' " &
                " GROUP BY PRIORIDAD,convert(char,datecreated,103),CAMION,WHSCODEORIGEN,WHSCODE,WHSNAME,CODIGOCLIENTE"

                Using cmd As New SqlCommand(sql_string)



                    'Using cmd As New SqlCommand("SELECT PREIDHEADER,PRIORIDAD,CONVERT(CHAR,DATECREATED,103)DATECREATED,[WHSCODEORIGEN] " &
                    '    " ,[WHSCODE],[WHSNAME],[CODIGOCLIENTE],SUM([QTYSOLICITADA])[QTYSOLICITADA],SUM([QTYPREPARADA])[QTYPREPARADA] " &
                    '    " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] WHERE ESTATUS='CAMION' " &
                    '    " GROUP BY PREIDHEADER,PRIORIDAD,DATECREATED,[WHSCODEORIGEN],[WHSCODE],[WHSNAME],[CODIGOCLIENTE] " &
                    '    " ORDER BY DATECREATED")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridForklift.DataSource = dt
                            GridForklift.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GridForklift.UseAccessibleHeader = True
            GridForklift.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('BindGridForklift: " & ReplaceCharsForFileName(ex.Message, " ") & "')</script>")
        End Try
    End Sub
    Private Sub GridForklift_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridForklift.RowCommand
        Try
            If e.CommandName = "Seleccionar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridForklift.Rows(index)
                ActualizarForklift("ARMADO", Date.Now, GridForklift.Rows(index).Cells(0).Text)
                BindGridForklift()
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('GridForklift_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "')</script>")
        End Try
    End Sub
    Public Sub ActualizarForklift(ESTATUS As String, FDATE As DateTime, ID As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[SOLCITUD_HEADER] set [FORKLIFT]=@p1,[FORKLIFTDATE]=@p2 where [ID]=@p3"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ESTATUS)
                cmd.Parameters.AddWithValue("@p2", FDATE)
                cmd.Parameters.AddWithValue("@p3", ID)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Moto Eliminada con Exito!<hr> EliminarLinea');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('ActualizarForklift: " & ReplaceCharsForFileName(ex.Message, " ") & "')</script>")
        End Try
    End Sub

    Private Sub TrasladosFlujoProduccion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGridForklift()
                End If
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('TrasladosFlujoProduccion_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "')</script>")
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

    Private Sub brnRegresar_Click(sender As Object, e As EventArgs) Handles brnRegresar.Click
        Try
            Response.Redirect("TrasladosDashboard.aspx")
        Catch ex As Exception
            Response.Write("<script>console.log('brnRegresar_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "')</script>")

        End Try
    End Sub
End Class

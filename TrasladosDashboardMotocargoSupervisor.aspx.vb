Imports System.Data
Imports System.Data.SqlClient
Partial Class TrasladosDashboardMotocargoSupervisor
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public SQL_STRING As String
    Public sql_consulta As String

    Private Sub TrasladosDashboardMotocargoSupervisor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindgridSugerido()
                End If
            Else
            End If

        Catch ex As Exception
            Response.Write("<script>console.log('VisorLogistica_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindgridSugerido()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)


                SQL_STRING = "SELECT ROW_NUMBER() OVER(ORDER BY SUM(CANTIDAD) DESC, SUM(Monto) DESC) AS Ranking " &
                                " ,WHSCODE [Codigo] " &
                                " ,(select whsname from movesa..OWHS with(nolock) where WhsCode=[PARETO_MOTOCARGO].WHSCODE collate Modern_Spanish_CI_AS) [Almacen] " &
                                " ,CASE  " &
                                " WHEN (SUM([CB]) + SUM([CO]) - SUM([SO]) - SUM([TR]) - SUM([FI])) / NULLIF(SUM([CB]), 0) < 0 THEN 0 " &
                                " ELSE ISNULL(CONVERT(DECIMAL(19, 4),  " &
                                " (SUM([CB]) + SUM([CO]) - SUM([SO]) - SUM([TR]) - SUM([FI])) * 1.0 / NULLIF(SUM([CB]), 0) " &
                                " ), 0) * 100 " &
                                " END [70] " &
                                " ,SUM(CANTIDAD) [Unds] " &
                                " ,SUM(MONTO) [Monto] " &
                                " ,SUM([CB]) [Cuadro] " &
                                " ,SUM([CO]) [Comprometido] " &
                                " ,SUM([SO]) [Solicitado] " &
                                " ,SUM([TR]) [Transito] " &
                                " ,SUM([FI]) [Fisico] " &
                                " ,(SUM([CB])+SUM([CO])-SUM([SO])-SUM([TR])-SUM([FI]))[Faltante] " &
                                " ,convert(char,max(ULTRANSFER),103)[Ultransfer] " &
                                " FROM [PARETO_MOTOCARGO] " &
                                " GROUP BY WHSCODE"

                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridIndiceD.DataSource = dt
                            gridIndiceD.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridIndiceD.UseAccessibleHeader = True
            gridIndiceD.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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

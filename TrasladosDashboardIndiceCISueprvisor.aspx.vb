Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json
Partial Class TrasladosDashboardIndiceCISueprvisor
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public SQL_STRING As String
    Public sql_consulta As String

    Private Sub TrasladosDashboardIndiceCISueprvisor_Load(sender As Object, e As EventArgs) Handles Me.Load
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


                SQL_STRING = "SELECT " &
                             "  ROW_NUMBER() OVER (ORDER BY t0.[Unds] desc) AS Ranking, " &
                             "    t0.[Codigo], " &
                             "    t0.[Almacen], " &
                             "    t0.[Cuadro], " &
                             "    t0.[Comprometido], " &
                             "    t0.[Solicitado], " &
                             "    t0.[Transito], " &
                             "    t0.[Fisico], " &
                             "    t0.[Faltante], " &
                             "    t0.[Unds], " &
                             "    convert(char,t0.[Ultransfer],103) [Ultransfer], " &
                             "    (SELECT U_ZONA   " &
                             "    FROM movesa..OWHS    " &
                             "    WHERE WhsCode = t0.Codigo COLLATE Modern_Spanish_CI_AS) AS [Zona], " &
                             "    (SELECT U_CardCode  " &
                             "    FROM movesa..OWHS  " &
                             "    WHERE WhsCode = t0.Codigo COLLATE Modern_Spanish_CI_AS) As [Cardcode], " &
                             "    (Select U_SLPNAME  " &
                             "    FROM movesa..[@CRESPONSABLEOWHS] With (NOLOCK) " &
                             "    WHERE code = (Select U_Categorizacion  " &
                             "    FROM movesa..OWHS  " &
                             "    WHERE WhsCode = t0.Codigo COLLATE Modern_Spanish_CI_AS)) As [Sup], " &
                             "    (Select Name  " &
                             "     FROM movesa..[@CRUTA] With (NOLOCK) " &
                             "     WHERE code = (Select U_Ruta  " &
                             "                   FROM movesa..OWHS  " &
                             "                   WHERE WhsCode = t0.Codigo COLLATE Modern_Spanish_CI_AS)) As [Ruta] " &
                             "     ,isnull(Convert(Decimal(19, 2), Convert(Decimal(19, 4), [Faltante]) / NULLIF(Convert(Decimal(19, 4), [Cuadro]), 0) * 100),0) As [70] " &
                             " FROM  " &
                             "    [ArmadoMotos].[dbo].[RESUMEN_I_ALLWHS] t0 With (NOLOCK); "

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
    Protected Sub btnRedirect_Command(sender As Object, e As CommandEventArgs)
        Dim almacen As String = e.CommandArgument.ToString()
        Dim url As String = "TrasladosCargaMacroIndirectoSupervisores.aspx?almacen=" & almacen
        ' Abrir la URL en una nueva pestaña
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenNewTab", "window.open('" & url & "', '_blank');", True)
        BindgridSugerido()
    End Sub
End Class

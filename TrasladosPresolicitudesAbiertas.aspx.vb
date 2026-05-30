Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class TrasladosPresolicitudesAbiertas
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String
    'Public Sub CargarSucursales()
    '    Try
    '        Dim dt As DataTable = New DataTable()
    '        Using conn As SqlConnection = New SqlConnection(sCon1)
    '            Dim query As String = "SELECT WhsName,WhsCode FROM OWHS WITH(NOLOCK) WHERE U_TYPE='PRO' AND WhsCode NOT LIKE 'T%' ORDER BY [WhsName] "

    '            'Dim query As String = "SELECT distinct [ALMDESTINO]," &
    '            '    " (select whsname from movesa..OWHS with(nolock) where whscode=[ALMDESTINO] collate Modern_Spanish_CI_AS)" &
    '            '    " [NOMBRE] FROM [ArmadoMotos].[dbo].[MACROINSERT] order by [NOMBRE]"
    '            Dim cmd As SqlCommand = New SqlCommand(query, conn)
    '            Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
    '            da.Fill(dt)
    '        End Using
    '        drpSucursales.Dispose()
    '        drpSucursales.DataTextField = "WhsName"
    '        drpSucursales.DataValueField = "WhsCode"
    '        drpSucursales.DataSource = dt
    '        drpSucursales.DataBind()
    '    Catch ex As Exception
    '        Response.Write("CargarSucursales " & ex.Message)
    '    End Try
    'End Sub

    Private Sub TrasladosPresolicitudesAbiertas_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGrid()
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('TrasladosPresolicitudesAbiertas_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_STRING As String = "SELECT * ,(SELECT WHSNAME FROM MOVESA..OWHS WITH(NOLOCK) WHERE WhsCode=[ALMDESTINO] COLLATE Modern_Spanish_CI_AS)[NALMACEN]" &
                                           " FROM [ArmadoMotos].[dbo].[PRESOLICITUD_HEADER] with(nolock) where ESTATUS='PRESOLICITUD' order by id desc"
                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCuadroBasico.DataSource = dt
                            gridCuadroBasico.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridCuadroBasico.UseAccessibleHeader = True
            gridCuadroBasico.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub

    Private Sub gridCuadroBasico_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCuadroBasico.RowCommand
        Try
            If e.CommandName = "Ver" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCuadroBasico.Rows(index)

                Response.Redirect("TrasladosPresolicitudesAbiertasDetalle.aspx?id=" & gridCuadroBasico.Rows(index).Cells(0).Text)

            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPedidoTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
End Class

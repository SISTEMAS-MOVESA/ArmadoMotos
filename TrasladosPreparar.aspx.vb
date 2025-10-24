Imports System.Activities
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Partial Class TrasladosPreparar
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Dim MyTable As New DataTable
    'Sub LoadGrid()
    '    Try
    '        Dim strSQL = "SELECT CAMION,WHSCODE,WHSNAME,sum(qtysolicitada)[Total] , convert(char,max(FECHAARMADO),103)[FechaPrep]" &
    '                     " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] where [ESTATUS]='CAMION' group by [camion],[WHSCODE],WHSNAME "

    '        Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon2))

    '            cmdSQL.Connection.Open()
    '            MyTable.Load(cmdSQL.ExecuteReader)
    '            repeater1.DataSource = MyTable
    '            repeater1.DataBind()
    '            cmdSQL.Connection.Close()
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("LoadGrid " & ex.Message)
    '    End Try
    'End Sub

    Private Sub TrasladosPreparar_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    'LoadGrid()
                    BindgridCargaCamion()

                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub
    Protected Sub btnAll_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Dim argument As String = TryCast(repeater1.FindControl("btnLogin"), System.Web.UI.WebControls.Button).CommandArgument.ToString()

        Dim argumento As String = (CType(sender, Button)).CommandArgument
        'Dim Cantidad As String = (CType(sender, Button)).Text

        Response.Redirect("TrasladosConfirmarSerie.aspx?camion=" & argumento)

        'For i As Integer = 1 To CInt(argumento)
        '    'Response.Write("<script>console.log('" & argumento & " / " & Cantidad & " ');</script>")
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ModalSolicitud').modal('show');</script>", False)
        'Next

        'Replace(argumento, "Aceptar Completo: ", "")

        'UpdateCamionCompleto(id, argumento, Session("UserCode"), Date.Now)

        'Dim argument = (CType(sender, Button)).CommandArgument.ToString()
        'Response.Write("<script>console.log('aqui');</script>")
        'Response.Write("<script>console.log('" & id & " ');</script>")
        'Response.Write("<script>console.log('" & argumento & " / " & Cantidad & " ');</script>")
    End Sub


    Private Sub BindgridCargaCamion()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim strSQL = "SELECT a.CAMION,a.WHSCODE,a.WHSNAME,sum(a.qtysolicitada)[Total] , convert(char,max(FECHAARMADO),103)[FechaPrep]" &
                    " ,(select [name] from movesa..[@CRESPONSABLEOWHS] with(nolock) where code=(SELECT U_Categorizacion FROM MOVESA..OWHS WITH(nolock) WHERE WHSCODE collate modern_Spanish_CI_AS=a.WHSCODE))[Supervisor]" &
                         " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] a where a.[ESTATUS]='CAMION' group by a.[camion],a.[WHSCODE],a.WHSNAME order by a.CAMION desc"

                Using cmd As New SqlCommand(strSQL)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCargaCamion.DataSource = dt
                            gridCargaCamion.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridCargaCamion.UseAccessibleHeader = True
            gridCargaCamion.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridCargaCamion_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCargaCamion.RowCommand
        Try
            If e.CommandName = "Series" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCargaCamion.Rows(index)
                'EliminarLinea(gridCargaCamion.Rows(index).Cells(0).Text)
                Response.Redirect("TrasladosConfirmarSerie.aspx?camion=" & gridCargaCamion.Rows(index).Cells(0).Text)
            End If
            BindgridCargaCamion()
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

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Partial Class OrdendeTrabajo
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Dim MyTable As New DataTable



    Private Sub OrdendeTrabajo_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    LoadGrid()

                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub
    Sub LoadGrid()
        Try
            Dim strSQL = "select distinct (select id from OTHEADER with(nolock) where RUTA=t.RUTA and CAMION=t.CAMION)[Id],t.CAMION, T.RUTA from  " &
                                                " (SELECT distinct ROWID [CAMION],RUTA " &
                                                    " ,(SELECT [NAME] FROM MOVESA..[@CRESPONSABLEOWHS] WHERE CODE=(SELECT U_CATEGORIZACION   " &
                                                    " FROM MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE=CODALM COLLATE Modern_Spanish_CI_AS ))[RESPONSABLE]   " &
                                                    " FROM [dbo].[PLAN_ARMADO] With(nolock) where APROBADOL='N' AND APROBADOC='N' ) as t "

            Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon2))

                cmdSQL.Connection.Open()
                MyTable.Load(cmdSQL.ExecuteReader)
                repeater1.DataSource = MyTable
                repeater1.DataBind()
                cmdSQL.Connection.Close()
            End Using
        Catch ex As Exception
            Response.Write("LoadGrid " & ex.Message)
        End Try
    End Sub
    Private Shared Function GetData(query As String) As DataTable
        Dim strConnString As String = sCon2
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand()
                cmd.CommandText = query
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using ds As New DataSet()
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
            con.Close()
        End Using
    End Function

    Private Sub repeater1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles repeater1.ItemDataBound
        Try
            Dim RepRow As Integer = e.Item.ItemIndex
            Dim PKId As Integer = MyTable.Rows(RepRow).Item("CAMION")
            Dim gvOrders As GridView = e.Item.FindControl("gvOrders")
            gvOrders.DataSource = GetData(String.Format("SELECT * FROM [ArmadoMotos].[dbo].[PLAN_ARMADO] where [ROWID]='{0}'", PKId))
            gvOrders.DataBind()
            gvOrders.ShowHeader = True
        Catch ex As Exception
            Response.Write("repeater1_ItemDataBound " & ex.Message)
        End Try
    End Sub
End Class

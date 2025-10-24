
Imports System.Data
Imports System.Data.SqlClient

Partial Class MaindashboardSucursales
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    'Sub LoadGrid()
    '    Try
    '        Dim strSQL = "SELECT *,CONVERT(CHAR,[DATECREATED],103)[FECHA]" &
    '        ",(select whsname from movesa..owhs where WhsCode=[DESTINO] collate Modern_Spanish_CI_AS)[NALMDESTINO]" &
    '        ",CASE ESTADO WHEN '1' THEN 'SUGERIDO' WHEN '2' THEN 'SUCURSAL' WHEN '3' THEN 'LOGISTICA' WHEN '4' THEN 'CARGA' " &
    '        " WHEN '5' THEN 'SOLICITUD' WHEN '6' THEN 'TRANSFER' WHEN '7' THEN 'Despachado' end [ETAPA]" &
    '        " FROM [ArmadoMotos].[dbo].[MACROINSERT_HEADER] WHERE ID>32 and [DESTINO]='" & Session("ALMTRANSIT") & "'"

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

    Private Sub MaindashboardSucursales_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'LoadGrid()
                End If
            End If
        Catch ex As Exception
            Response.Write("TrasladosDashboard_Load " & ex.Message)
        End Try
    End Sub
End Class

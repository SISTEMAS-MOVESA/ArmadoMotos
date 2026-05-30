Imports System.Data
Imports System.Web.Services
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Partial Class TrabajosAdicionalesDashboard
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable

    Private Sub TrabajosAdicionalesDashboard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else

                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
                    Else
                        LoadGrid()
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write("TrabajosAdicionalesDashboard_Load " & ex.Message)
        End Try
    End Sub

    <WebMethod()>
    Public Shared Function GetChartEjecutivosPieChart() As List(Of Object)
        Dim query As String = " SELECT top 15 [ALMDESTINO],count([ALMDESTINO])[Cuenta] " &
                                " FROM [ArmadoMotos].[dbo].[MACROINSERT] group by [ALMDESTINO] order by count([ALMDESTINO]) desc "


        Dim constr As String = sCon1
        Dim chartData As New List(Of Object)()
        chartData.Add(New Object() {"ALMDESTINO", "Cuenta"})
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(query)
                cmd.CommandType = CommandType.Text
                cmd.Connection = con
                con.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        chartData.Add(New Object() {sdr("ALMDESTINO"), sdr("Cuenta")})
                    End While
                End Using
                con.Close()
                Return chartData
            End Using
        End Using
    End Function
    'RADAR DE ETAPAS
    <WebMethod()>
    Public Shared Function GetRadarChartDataEtapas() As String
        Dim connectionString As String = sCon2
        Dim query As String = "SELECT T1.[TRABAJO][Trabajo],Count(1)[Total] FROM [ArmadoMotos].[dbo].[TRABAJOSADICIONALES] T0 INNER JOIN " &
                                " [ArmadoMotos].[dbo].TRABAJOS T1 ON T0.IDTRABAJO = T1.ID where T0.ESTADO='P' and datepart(year,T0.[DATECREATED])=2023 group by T1.[TRABAJO]"

        'Dim query As String = " SELECT CASE ESTADO WHEN '1' THEN 'SUGERIDO' WHEN '2' THEN 'SUCURSAL' WHEN '3' THEN 'LOGISTICA' WHEN '4' THEN 'CARGA'   " &
        '                " WHEN '5' THEN 'SOLICITUD' WHEN '6' THEN 'TRANSFER' WHEN '7' THEN 'Despachado' end [ETAPA] " &
        '                " ,count(ESTADO)[Total] FROM [ArmadoMotos].[dbo].[MACROINSERT_HEADER] WHERE ID>32 and ESTADO NOT IN (6,7) group by ESTADO"

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim data As New List(Of Object)()
                    While reader.Read()
                        data.Add(New With {
                            .ShippingLine = reader("Trabajo").ToString() & " - " & reader("Total"),
                            .Conteo = Convert.ToInt32(reader("Total"))
                        })
                    End While
                    Return Newtonsoft.Json.JsonConvert.SerializeObject(data)
                End Using
            End Using
            connection.Close()
        End Using
    End Function
    Sub LoadGrid()
        Try
            Dim strSQL = "SELECT	T0.[ID],T0.[SERIE],T2.ITENMANE,T0.[IDTRABAJO],T1.[TRABAJO],T0.[DATECREATED] " &
                        " FROM	[ArmadoMotos].[dbo].[TRABAJOSADICIONALES] T0 " &
                        " INNER JOIN [ArmadoMotos].[dbo].TRABAJOS T1 " &
                        " ON T0.IDTRABAJO = T1.ID INNER JOIN [ArmadoMotos].DBO.ARMADOMOTOS T2 " &
                        " ON T0.SERIE=T2.SERIE " &
                        " where T0.ESTADO='P' and datepart(year,T0.[DATECREATED])=2023  " &
                        " ORDER BY T0.ID "

            'Dim strSQL = "SELECT *,CONVERT(CHAR,[DATECREATED],103)[FECHA]" &
            '",(select [Name] from movesa..[@CRESPONSABLEOWHS] where code=(select U_Categorizacion from movesa..owhs where WhsCode=[DESTINO] collate Modern_Spanish_CI_AS))[SUPERVISORASIGNADO] " &
            '",(select whsname from movesa..owhs where WhsCode=[DESTINO] collate Modern_Spanish_CI_AS)[NALMDESTINO]" &
            '",CASE ESTADO WHEN '1' THEN 'SUGERIDO' WHEN '2' THEN 'SUCURSAL' WHEN '3' THEN 'LOGISTICA' WHEN '4' THEN 'CARGA' " &
            '" WHEN '5' THEN 'SOLICITUD' WHEN '6' THEN 'TRANSFER' WHEN '7' THEN 'Despachado' end [ETAPA]" &
            '" FROM [ArmadoMotos].[dbo].[MACROINSERT_HEADER] WHERE ID>32 "


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
    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs)
        Try
            Dim id As Integer = Convert.ToInt32(TryCast(sender, Button).CommandArgument)
            Dim result As String = EliminarRegistro(id)

            Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Registro Eliminado!!',position: 'topRight',timeout: 10000})</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            Threading.Thread.Sleep(5000)
            LoadGrid()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        ' Puedes hacer algo con el resultado, como mostrarlo en una etiqueta o en una alerta JavaScript
        ' Ejemplo: lblResultado.Text = result
    End Sub
    <WebMethod()>
    Public Shared Function EliminarRegistro(id As Integer) As String
        Dim connectionString As String = sCon2
        Using con As New SqlConnection(connectionString)
            con.Open()
            Dim query As String = "DELETE FROM [ArmadoMotos].[dbo].[MACROINSERT_HEADER] WHERE id = @p1"
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@p1", id)
                cmd.ExecuteNonQuery()
                Return "Registro eliminado correctamente."
            End Using
        End Using
        Return "Error al eliminar el registro."
    End Function
End Class

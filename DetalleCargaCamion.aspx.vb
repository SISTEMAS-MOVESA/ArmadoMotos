
Imports System.Data
Imports System.Data.SqlClient

Partial Class DetalleCargaCamion
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public id_camion As String

    Private Sub DetalleCargaCamion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            id_camion = Request.QueryString("idcamion")
            TrabajosAdicionales(id_camion)

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub TrabajosAdicionales(idCamion As String)
        Try
            If Not Me.IsPostBack Then
                Dim constr As String = sCon2
                Using con As SqlConnection = New SqlConnection(constr)
                    Using cmd As SqlCommand = New SqlCommand("SELECT DISTINCT isnull(SERIEASIGNADA,'N/A') [Serie], RUTA COLLATE Modern_Spanish_CI_AS  " &
                                                             " + ' ' + OBSERVACIONES COLLATE Modern_Spanish_CI_AS  " &
                                                             " + ' ' + ALMDESTINO COLLATE Modern_Spanish_CI_AS  " &
                                                             " + ' ' + (SELECT WhsName FROM MOVESA..OWHS WHERE WHSCODE=ALMDESTINO COLLATE Modern_Spanish_CI_AS)  " &
                                                             " + ' ' + DESCRIPCION COLLATE Modern_Spanish_CI_AS  " &
                                                             " + ' ' + isnull(SERIEASIGNADA,'N/A') COLLATE Modern_Spanish_CI_AS [Detalle] FROM CARGA_CAMION_DETALLE  " &
                                                             " WHERE IDCAMION = @idCamion")
                        cmd.Parameters.AddWithValue("@idCamion", idCamion)
                        cmd.CommandType = CommandType.Text
                        cmd.Connection = con
                        con.Open()
                        lstMotosySeries.DataSource = cmd.ExecuteReader()
                        lstMotosySeries.DataTextField = "Detalle"
                        lstMotosySeries.DataValueField = "Serie"
                        lstMotosySeries.DataBind()
                        con.Close()
                    End Using
                End Using

            End If
        Catch ex As Exception
            Response.Write("TrabajosAdicionales " & ex.Message)
        End Try
    End Sub

End Class

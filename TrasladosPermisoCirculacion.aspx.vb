Imports System.Data
Imports System.Data.SqlClient
Partial Class TrasladosPermisoCirculacion
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public Sub BringDetallesMoto(SERIEMOTO As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim consulta As String = " " &
            " select	t0.ItemCode,t2.Name [Modelo],t3.Name [Tipo],t4.Name [Color],t0.MnfSerial,t0.DistNumber,t0.LotNumber,t5.Name[Marca]  " &
            " from	OSRN t0 with(nolock) inner join oitm t1 with(nolock) on t0.ItemCode=t1.ItemCode" &
            " inner join [@AMODELO] t2 on t1.U_AMODELO=t2.Code inner join [@SUBGRUPO] t3 with(nolock)" &
            " on t1.U_SubGrupo=t3.Code inner join [@SCOLOR] t4 on t1.U_ACOLOR=t4.Code INNER JOIN [@AMARCA] t5 on t1.U_AMARCA=t5.Code" &
            " where	t0.MnfSerial='" & SERIEMOTO & "'"

            Dim Comando As New SqlCommand(consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblMarca.Text = drd.Item("Marca").ToString
                lblModelo.Text = drd.Item("Modelo").ToString
                lblTipo.Text = drd.Item("Tipo").ToString
                lblChasis.Text = drd.Item("MnfSerial").ToString
                lblMotor.Text = drd.Item("DistNumber").ToString
                lblVin.Text = drd.Item("MnfSerial").ToString
                lblColor.Text = drd.Item("Color").ToString
                lblYear.Text = drd.Item("LotNumber").ToString
            End If
        Catch ex As Exception
            Response.Write("TraerDetallesHuesped " & ex.Message)
        End Try
    End Sub

    Private Sub TrasladosPermisoCirculacion_Load(sender As Object, e As EventArgs) Handles Me.Load
        BringDetallesMoto(Request.QueryString("serie"))
    End Sub
End Class

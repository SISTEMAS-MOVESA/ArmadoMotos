Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Class TrasladosCargaCamion
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"

    <WebMethod()>
    Public Shared Function SearchSerial(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1
            Using cmd As SqlCommand = New SqlCommand()

                cmd.CommandText = " SELECT [SERIE] FROM [ArmadoMotos].[dbo].[SOLCITUD_LINES] WHERE [SERIE] LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("SERIE").ToString())
                    End While
                End Using
                conn.Close()

                Return customers
            End Using
        End Using
    End Function
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        BuscarSerie(txtbuscar.Text)
    End Sub
    Public Sub BuscarSerie(_SERIE As String)
        Try
            btnGuardar.Visible = True

            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = "select t0.WHSCODEORIGEN,t0.WHSNAMEORIGEN,t0.WHSCODE,t0.WHSNAME,t0.CODIGOCLIENTE,t0.MODELO, " &
                                    " t1.ARTICULO, t1.DESCRIPCION,t1.SERIE from [ArmadoMotos].[dbo].[SOLCITUD_HEADER] t0  " &
                                    " Inner join [ArmadoMotos].[dbo].[SOLCITUD_LINES] t1 " &
                                    " On t0.id=t1.headerid where t1.SERIE='" & _SERIE & "'"
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtItemcode.Text = drd.Item("ARTICULO").ToString
                txtItemname.Text = drd.Item("DESCRIPCION").ToString
                txtModelo.Text = drd.Item("MODELO").ToString

                txtAlmOrigenCodigo.Text = drd.Item("WHSCODEORIGEN").ToString
                txtAlmOrigenNombre.Text = drd.Item("WHSNAMEORIGEN").ToString

                txtAlmacenDestinoCode.Text = drd.Item("WHSCODE").ToString
                txtAlmacenDestinoNombre.Text = drd.Item("WHSNAME").ToString
                txtCodigoCliente.Text = drd.Item("CODIGOCLIENTE").ToString
                'txtStatusSerie.Text = drd.Item("Estatus").ToString
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class

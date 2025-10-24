
Imports System.Data
Imports System.Data.SqlClient

Partial Class DespachosCaratula
    Inherits System.Web.UI.Page
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable

    Private Sub DespachosCaratula_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            InformacionCamion(getPlaca(Request.QueryString("idcamion")))
            InformacionMotorista(Request.QueryString("motorista"))
            LoadGrid(Request.QueryString("idcamion"))
        Catch ex As Exception

        End Try
    End Sub
    Public Sub InformacionCamion(placa As String)
        Try
            Dim dt As New DataTable
            Dim conn As New SqlConnection(sCon2)
            Dim sqlString As String = "SELECT [EMPRESA],[RTN],[MARCAMODELO],[PLACA] FROM [ArmadoMotos].[dbo].[TRANSPORTISTAS] where PLACA= @placa"

            Dim command As New SqlCommand(sqlString, conn)
            command.Parameters.AddWithValue("@placa", placa)
            Dim drd As SqlDataReader
            conn.Open()
            drd = command.ExecuteReader()
            If drd.Read() Then

                txtEmpresa.Text = drd.Item("EMPRESA").ToString()
                txtMarca.Text = drd.Item("MARCAMODELO").ToString()
                txtPlaca.Text = drd.Item("PLACA").ToString()
                txtRTN.Text = drd.Item("RTN").ToString()

            End If
            conn.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('TraerEstadoCliente: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub

    Public Sub InformacionMotorista(motorista As String)
        Try
            Dim dt As New DataTable
            Dim conn As New SqlConnection(sCon2)
            Dim sqlString As String = "SELECT MOTORISTA,LICENCIA,isnull(CELULAR,'N/A') [CELULAR] FROM [ArmadoMotos].[dbo].[TRANSPORTISTAS] where MOTORISTA= @motorista"

            Dim command As New SqlCommand(sqlString, conn)
            command.Parameters.AddWithValue("@motorista", motorista)
            Dim drd As SqlDataReader
            conn.Open()
            drd = command.ExecuteReader()
            If drd.Read() Then

                txtMotorista.Text = drd.Item("MOTORISTA").ToString()
                txtLicencia.Text = drd.Item("LICENCIA").ToString()
                txtCelular.Text = drd.Item("CELULAR").ToString()

            End If
            conn.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('TraerEstadoCliente: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function getPlaca(camion_id As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "select PLACA from CARGA_CAMION_HEADER where id = @id"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@id", camion_id)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getPlaca: " & t & "');</script>")
        End Using
    End Function
    Sub LoadGrid(idcamion As String)
        Try
            Dim strSQL = "select distinct IDCAMION,ALMDESTINO, " &
            " (select whsname from movesa..OWHS where WhsCode=ALMDESTINO collate Modern_Spanish_CI_AS)[WhsName] " &
            " ,RUTA from CARGA_CAMION_DETALLE where idcamion = @idcamion AND [CODIGOESTADO]<>'CERRADO'"

            Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon2))
                cmdSQL.Parameters.AddWithValue("@idcamion", idcamion)
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
            Dim PKId As Integer = MyTable.Rows(RepRow).Item("IDCAMION")
            Dim PkAlm As String = MyTable.Rows(RepRow).Item("ALMDESTINO")
            Dim gvOrders As GridView = e.Item.FindControl("gvOrders")
            gvOrders.DataSource = GetData(String.Format("select MODELO,ARTICULO,DESCRIPCION,SERIEASIGNADA " &
                                                        " ,(select DistNumber from movesa..OSRN where MnfSerial=SERIEASIGNADA collate Modern_Spanish_CI_AS)[SerieMotor] " &
                                                        " from CARGA_CAMION_DETALLE where [IDCAMION]='{0}' and [ALMDESTINO]='{1}'", PKId, PkAlm))
            gvOrders.DataBind()
            gvOrders.ShowHeader = True
        Catch ex As Exception
            Response.Write("repeater1_ItemDataBound " & ex.Message)
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

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing
Partial Class ActualizarFotosRecepcion
    Inherits System.Web.UI.Page
    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public xCompany As SAPbobsCOM.Company
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public oGoodsRcpt As SAPbobsCOM.Documents
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"

    Protected Sub btnBuscarMoto_Click(sender As Object, e As EventArgs) Handles btnBuscarMoto.Click
        Try
            CLIENTE_SKG2(txtRecordId.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CLIENTE_SKG2(_SERIAL As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "SELECT t0.*,(select itemname from movesa..oitm with(nolock) where itemcode collate SQL_Latin1_General_CP1_CI_AS=t0.ITEMCODE) [ItemName] FROM [MOVESAWEB].[dbo].[ENTRADAOPSKG] t0 WHERE t0.SERIE='" & _SERIAL & "'"

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtSerie.Text = drd.Item("SERIE").ToString
                txtSerieM.Text = drd.Item("SERIEM").ToString
                txtModelo.Text = drd.Item("MODELO").ToString
            End If
            Audit(Date.Now, "CLIENTE_SKG2", Session("User").ToString, "Buscar: " & _SERIAL)
        Catch ex As Exception
            ADD_LOG("CLIENTE_SKG2", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub Audit(_fecha As String, _proceso As String, _usuario As String, _observaciones As String)
        Try
            Dim query As String = String.Empty
            query &= " INSERT INTO [MovesaWeb].[dbo].[ENTRADAOPSKG_AUDIT] ([FECHA],[PROCESO],[USUARIO],[OBSERVACIONES]) " &
                      " VALUES (@p1,@p2,@p3,@p4) "
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _fecha)
                        .Parameters.AddWithValue("@p2", _proceso)
                        .Parameters.AddWithValue("@p3", _usuario)
                        .Parameters.AddWithValue("@p4", _observaciones)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ADD_LOG("Audit", ex.Message)
        End Try
    End Sub
    Public Sub ADD_LOG(_PROCESO As String, _ERROR As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "INSERT INTO [MovesaWeb].[dbo].[PortalSKG] ([FECHA],[PROCESO],[ERROR])" &
                   " VALUES (@p1,@p2,@p3)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", Date.Now)
                cmd.Parameters.AddWithValue("@p2", _PROCESO)
                cmd.Parameters.AddWithValue("@p3", _ERROR)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ActualizarFotosRecepcion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'GESTORC
            If Session("Position").ToString = "GESTORC" Then
                pnlSubMenuOrdenRecuperacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = False
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            'SKA1001
            If Session("Position").ToString = "ADMIN" Then
                pnlSubMenuConfirmacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = False
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            'VENDEDOR
            If Session("Position").ToString = "VENDEDOR" Then
                pnlSubMenuConfirmacion.Visible = False
                pnlSubMenuOrdenRecuperacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = True
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            If Session("SerialNumberPhotosIniciales").ToString = "NULL" Then
            Else
                CLIENTE_SKG2(Session("SerialNumberPhotosIniciales").ToString)
                txtRecordId.Text = Session("SerialNumberPhotosIniciales").ToString
            End If
            lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
        Catch ex As Exception
            ADD_LOG("Page_Load", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub
End Class

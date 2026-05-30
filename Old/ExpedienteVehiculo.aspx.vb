Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing
Partial Class ExpedienteVehiculo
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
    
    <System.Web.Script.Services.ScriptMethod(), System.Web.Services.WebMethod()>
    Public Shared Function SearchCustomersByCode(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = sCon1
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = "select top 10 MnfSerial from oinv t0  with (nolock) " &
                          "  inner join inv1 t1 With (nolock) On t0.DocEntry = t1.DocEntry " &
                          " inner Join OSRN t2 With (nolock) On t1.U_MSERIE = t2.MnfSerial inner " &
                          " join oitm t3 With (nolock) On t3.ItemCode = t1.ItemCode " &
                          " inner Join ocrd t4 With (nolock) On t0.cardcode = t4.cardcode " &
                          " where t4.GroupCode In (116, 102) And t3.ItmsGrpCod = '154' " &
                          " And t1.TargetType <> 14  And t0.CANCELED = 'N' " &
                          " And t2.MnfSerial LIKE '%' + @SearchText + '%'"

        Dim value1 As String = prefixText
        cmd.Parameters.AddWithValue("@SearchText", value1)
        cmd.Connection = conn
        conn.Open()
        Dim customers As List(Of String) = New List(Of String)
        Dim sdr As SqlDataReader = cmd.ExecuteReader
        While sdr.Read
            customers.Add(sdr("MnfSerial").ToString)
        End While
        conn.Close()
        Return customers
    End Function

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
                lblNumCAI.Text = drd.Item("DOCNUMCAI").ToString
                lblDocnum.Text = drd.Item("DOCNUM").ToString
                lblDocdate.Text = drd.Item("DOCDATE").ToString
                lblCardcode.Text = drd.Item("CARDCODE").ToString
                lblCardname.Text = drd.Item("CARDNAME").ToString
                lblIdentidad.Text = drd.Item("IDENTIDAD").ToString
                lblRTN.Text = drd.Item("RTN").ToString
                lblItemcode.Text = drd.Item("ITEMCODE").ToString
                lblItemname.Text = drd.Item("ItemName").ToString
                lblModelo.Text = drd.Item("MODELO").ToString
                lblYear.Text = drd.Item("YEAR").ToString
                lblColor.Text = drd.Item("COLOR").ToString
                lblAduana.Text = drd.Item("ADUANA").ToString
                LblItem.Text = drd.Item("ITEM").ToString
                lblPoliza.Text = drd.Item("POLIZA").ToString
                lblMarca.Text = drd.Item("MARCA").ToString
                lblCilindros.Text = drd.Item("CILINDROS").ToString
                lblDesembolso.Text = drd.Item("DESEMBOLSO").ToString
                lblSerie.Text = drd.Item("SERIE").ToString
                lblMotor.Text = drd.Item("SERIEM").ToString
                lblFpago.Text = drd.Item("FPAGOADUANA").ToString
                lblMotorM.Text = drd.Item("MOTORM").ToString
                txtMontoMatricula.Text = drd.Item("MONTOMATRICULA").ToString
                txtNumPlaca.Text = drd.Item("NUMEROPLACA").ToString
                lblMontoDacion.Text = drd.Item("MONTODACION").ToString
                txtGestor.Text = drd.Item("GESTOR").ToString
                txtAlmacen.Text = drd.Item("WHSNAME").ToString
                txtComments1.Text = drd.Item("ESTADOMOTO").ToString
                CargarSetOne(_SERIAL)
                CargarSetTwo(_SERIAL)
                ComentariosFichaVenta(_SERIAL)
            End If
            Audit(Date.Now, "CLIENTE_SKG2", Session("User").ToString, "Buscar: " & _SERIAL)
        Catch ex As Exception
            ADD_LOG("CLIENTE_SKG2", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub ComentariosFichaVenta(_SERIAL As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "SELECT [COMENTARIOSFINALES] FROM [MOVESAWeb].[dbo].[ENTRADAOPSKG_PHOTOS_TWO] t0 WHERE t0.SERIE='" & _SERIAL & "'"

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtComments2.Text = drd.Item("COMENTARIOSFINALES").ToString
            End If
            Audit(Date.Now, "CLIENTE_SKG2", Session("User").ToString, "Buscar: " & _SERIAL)
        Catch ex As Exception
            ADD_LOG("CLIENTE_SKG2", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub CargarSetOne(_Serial As String)
        Try
            Dim constr As String = sCon1
            Using conn As SqlConnection = New SqlConnection(constr)
                Using sda As SqlDataAdapter = New SqlDataAdapter("Select ID, ITEMCODE, ITEMNAME, MARCA, MODELO, SERIE, IMG1, IMG2, IMG3, IMG4, IMG5, IMG6, IMG7 FROM MOVESAWeb.dbo.ENTRADAOPSKG where SERIE='" & _Serial & "'  order by ID desc", conn)
                    Dim dt As DataTable = New DataTable()
                    sda.Fill(dt)
                    gridSetFotos1.DataSource = dt
                    gridSetFotos1.DataBind()
                    gridSetFotos1.UseAccessibleHeader = True
                    gridSetFotos1.HeaderRow.TableSection = TableRowSection.TableHeader
                End Using
            End Using
        Catch ex As Exception
            ADD_LOG("CargarSetOne", ex.Message)
        End Try
    End Sub
    
    Public Sub CargarSetTwo(_Serial As String)
        Try
            Dim constr As String = sCon1
            Using conn As SqlConnection = New SqlConnection(constr)
                Using sda As SqlDataAdapter = New SqlDataAdapter("SELECT * FROM [MOVESAWeb].[dbo].[ENTRADAOPSKG_PHOTOS_TWO] where SERIE='" & _Serial & "'", conn)
                    Dim dt As DataTable = New DataTable()
                    sda.Fill(dt)
                    gridSetFotos2.DataSource = dt
                    gridSetFotos2.DataBind()
                    gridSetFotos2.UseAccessibleHeader = True
                    gridSetFotos2.HeaderRow.TableSection = TableRowSection.TableHeader

                End Using
            End Using
        Catch ex As Exception
            ADD_LOG("CargarSetTwo", ex.Message)
        End Try
    End Sub
    
    Protected Sub btnBuscarMoto_Click(sender As Object, e As EventArgs) Handles btnBuscarMoto.Click
        CLIENTE_SKG2(txtSnMoto.Text)
    End Sub
    
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("ExpedienteVehiculo.aspx")
    End Sub

    Private Sub ExpedienteVehiculo_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
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

            If Session("SerialNumberPhotos").ToString() = "NULL" Then
            Else
                CLIENTE_SKG2(Session("SerialNumberPhotos").ToString)
                txtSnMoto.Text = Session("SerialNumberPhotos").ToString
            End If

        Catch ex As Exception
            ADD_LOG("Page_Load", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub btnSetTwo_Click(sender As Object, e As ImageClickEventArgs) Handles btnSetTwo.Click
        Response.Redirect("ConfirmacionVenta.aspx")
    End Sub
    
    Protected Sub btnSetOne_Click(sender As Object, e As ImageClickEventArgs) Handles btnSetOne.Click
        Session("SerialNumberPhotosIniciales") = lblSerie.Text
        Response.Redirect("ActualizarFotosRecepcion.aspx")
    End Sub
End Class

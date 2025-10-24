Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing

Partial Class GoodsRcpt
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
            'Session("User")
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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'GESTORC
            If Session("Position").ToString = "GESTORC" Then
                Response.Redirect("Menu.aspx")
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
                Response.Redirect("Menu.aspx")
            End If

            If String.IsNullOrEmpty(Session("Name").ToString()) Then
                Response.Redirect("Default.aspx")
            End If
            lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
        Catch ex As Exception
            ADD_LOG("Page_Load", ex.Message)
            Response.Write(ex.Message)
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

    Public Sub CLIENTE_SKG2(CARDCODESKG As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select t0.DocNum,t0.u_numdei,convert(char,t0.DocDate,103) [DocDate],t0.CardCode,t0.CardName, " &
                                        " (Select VatIdUnCmp from ocrd with(nolock) where CardCode=t0.CardCode) [Identidad], " &
                                        " t2.itemcode, t2.itemname, t0.U_RTNSN, t16.MnfSerial [Serie], t16.DistNumber [Motor], t16.U_NPoliza [Poliza], " &
                                        " t16.LotNumber [Año],t16.U_NAduana [Aduana],t16.U_NITEM [Item],t16.LotNumber [Año], t11.Name [MotorM] , " &
                                        " convert(char,t16.U_FPago,103) [fpago],t3.Name [Marca], t4.Name [Modelo],t9.Name [Color],t10.[Name] [Cilindros], t17.[PRENUMERO] " &
                                        " From OINV t0 inner join inv1 t1  with(nolock) on t0.DocEntry = t1.DocEntry  " &
                                        " inner join oitm t2 with(nolock) on t2.ItemCode = t1.ItemCode " &
                                        " left join [@AMARCA] t3 with(nolock) on t2.U_AMARCA = t3.Code " &
                                        " left join [@AMODELO] t4 with(nolock) on t2.U_AMODELO = t4.Code " &
                                        " Left join [@SCOLOR] t9  With(nolock) On t9.Code = t2.U_ACOLOR " &
                                        " left join [@ACILINDROS] t10 with(nolock) on t10.Code = t2.U_ACILINDROS " &
                                        " left join [@AMOTOR] t11  with(nolock) on t2.U_AMOTOR = t11.Code " &
                                        " left join [@AFRENOSD] t12  with(nolock) on t12.Code = t2.U_FrenoD " &
                                        " left join [@AFRENOST] t13  with(nolock) on t13.Code = t2.U_FrenoT " &
                                        " left join [@ENFRIAMIENTO] t14  with(nolock) on t14.Code = t2.U_Enfriamiento " &
                                        " left join [@ATIPO] t15  with(nolock) on t15.Code = t2.U_ATIPO " &
                                        " inner join osrn t16  with(nolock) on t16.MnfSerial = t1.u_mserie and t16.ItemCode = t1.ItemCode " &
                                        " left join ( seleCT CARDCODE , [DESEMBOLSO] , [prenumero] from [DESEMBOLSOS_SIFCO] where CrMoTrxCod IS NULL )  t17 on t0.CardCode = t17.CARDCODE COLLATE SQL_Latin1_General_CP850_CI_AS " &
                                        " where t16.MnfSerial ='" & CARDCODESKG & "' "

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblNumCAI.Text = drd.Item("u_numdei").ToString
                lblDocnum.Text = drd.Item("DocNum").ToString
                lblDocdate.Text = drd.Item("DocDate").ToString
                lblCardcode.Text = drd.Item("Cardcode").ToString
                lblCardname.Text = drd.Item("Cardname").ToString
                lblIdentidad.Text = drd.Item("Identidad").ToString
                lblRTN.Text = Replace(Replace(Replace(drd.Item("U_RTNSN").ToString, "RTN", ""), ":", ""), "-", "")
                lblItemcode.Text = drd.Item("itemcode").ToString
                lblItemname.Text = drd.Item("itemname").ToString
                lblModelo.Text = drd.Item("Modelo").ToString
                lblYear.Text = drd.Item("Año").ToString
                lblColor.Text = drd.Item("Color").ToString
                lblAduana.Text = drd.Item("Aduana").ToString
                LblItem.Text = drd.Item("Item").ToString
                lblPoliza.Text = drd.Item("Poliza").ToString
                lblMarca.Text = drd.Item("Marca").ToString
                lblCilindros.Text = drd.Item("Cilindros").ToString
                lblDesembolso.Text = drd.Item("PRENUMERO").ToString
                lblSerie.Text = drd.Item("serie").ToString
                lblMotor.Text = drd.Item("motor").ToString
                lblFpago.Text = drd.Item("fpago").ToString
                lblMotorM.Text = drd.Item("MotorM").ToString
            End If
            Audit(Date.Now, "CLIENTE_SKG2", Session("User").ToString, "Buscar: " & CARDCODESKG)
        Catch ex As Exception
            ADD_LOG("CLIENTE_SKG2", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub _CargarAlmacenes()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection("Data Source=192.168.1.60;Initial Catalog=DBSKG;Persist Security Info=True;User ID=sa;Password=S@pB1Sql")
                Dim query As String = "select whscode, whscode + ' - ' + whsname [whsname] from owhs with(nolock)  WHERE WhsCode<>'01'"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
            drpAlmacenes.Dispose()
            drpAlmacenes.DataTextField = "whsname"
            drpAlmacenes.DataValueField = "whscode"
            drpAlmacenes.DataSource = dt
            drpAlmacenes.DataBind()
        Catch ex As Exception
            ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
        End Try
    End Sub

    Protected Sub btnBuscarMoto_Click(sender As Object, e As EventArgs) Handles btnBuscarMoto.Click
        CLIENTE_SKG2(txtSnMoto.Text)
        _CargarAlmacenes()
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("GoodsRcpt.aspx")
    End Sub

    Protected Sub btnIngreso_Click(sender As Object, e As EventArgs) Handles btnIngreso.Click
        ADD_RECEPCION_MOTO()
    End Sub

    Public Sub ADD_RECEPCION_MOTO()
        Try

            If drpGestores.SelectedItem.ToString = "Seleccione" Then
                Response.Write("<script>alert('Debe Seeccionar un Gestor de Cobro, Antes de Continuar');</script>")
                Exit Sub
            End If

            If lblMontoDacion.Text = "" Then
                Response.Write("<script>alert('Debe Colocar un Monto de Dacion, Antes de Continuar');</script>")
                Exit Sub
            End If

            If txtMontoMatricula.Text = "" Then
                Response.Write("<script>alert('Debe Colocar el Valor de la Matricula, Antes de Continuar');</script>")
                Exit Sub
            End If

            If txtNumPlaca.Text = "" Then
                Response.Write("<script>alert('Debe Colocar el Numero de Placa, Antes de Continuar');</script>")
                Exit Sub
            End If

            Dim sel As String
            sel = "SELECT * FROM [MOVESAWeb].[dbo].[ENTRADAOPSKG] with(nolock)"
            Dim da As New SqlDataAdapter(sel, sCon1)
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            Dim cb As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)

            Dim dr As DataRow = dt.NewRow
            dr("CREATEDATE") = Date.Now
            dr("DOCNUM") = lblDocnum.Text.ToString.Trim
            dr("DOCDATE") = CDate(lblDocdate.Text.ToString.Trim)
            dr("DOCNUMCAI") = lblNumCAI.Text.ToString.Trim
            dr("DESEMBOLSO") = lblDesembolso.Text.ToString.Trim
            dr("CARDCODE") = lblCardcode.Text.ToString.Trim
            dr("CARDNAME") = lblCardname.Text.ToString.Trim
            dr("IDENTIDAD") = lblIdentidad.Text.ToString.Trim
            dr("RTN") = lblRTN.Text.ToString.Trim
            dr("ITEMCODE") = lblItemcode.Text.ToString.Trim
            dr("MODELO") = lblModelo.Text.ToString.Trim
            dr("YEAR") = lblYear.Text.ToString.Trim
            dr("ITEMNAME") = lblItemname.Text.ToString.Trim
            dr("MARCA") = lblMarca.Text.ToString.Trim
            dr("MOTORM") = lblMotorM.Text.ToString.Trim
            dr("CILINDROS") = lblCilindros.Text.ToString.Trim
            dr("COLOR") = lblColor.Text.ToString.Trim
            dr("ADUANA") = lblAduana.Text.ToString.Trim
            dr("POLIZA") = lblPoliza.Text.ToString.Trim
            dr("ITEM") = LblItem.Text.ToString.Trim
            dr("SERIE") = lblSerie.Text.ToString.Trim
            dr("SERIEM") = lblMotor.Text.ToString.Trim
            dr("FPAGOADUANA") = CDate(lblFpago.Text.ToString.Trim)
            dr("NUMEROPLACA") = txtNumPlaca.Text.ToString.Trim
            dr("WHSCODE") = drpAlmacenes.SelectedValue
            dr("WHSNAME") = drpAlmacenes.SelectedItem
            dr("GESTOR") = drpGestores.SelectedItem
            dr("MONTOMATRICULA") = IIf(txtMontoMatricula.Text.ToString.Trim <> "", txtMontoMatricula.Text.ToString.Trim, 0.00)
            dr("MONTODACION") = IIf(lblMontoDacion.Text.ToString.Trim <> "", lblMontoDacion.Text.ToString.Trim, 0.00)
            dr("ESTADO") = "Recuperar"
            dr("ESTATUSINGRESO") = "Pendiente"
            dt.Rows.Add(dr)
            da.Update(dt)
            Audit(Date.Now, "ADD_RECEPCION_MOTO", Session("User").ToString, "Ingresar en Tabla Serie: " & lblSerie.Text.ToString.Trim)
            Response.Redirect("GoodsRcpt.aspx")

        Catch ex As Exception
            ADD_LOG("ADD_RECEPCION_MOTO", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Function ResizeImage(ByVal fuImage As FileUpload) As Byte()
        Try
            Dim imageToBeResized As System.Drawing.Image = System.Drawing.Image.FromStream(fuImage.PostedFile.InputStream)
            Dim imageHeight As Integer = imageToBeResized.Height
            Dim imageWidth As Integer = imageToBeResized.Width
            Dim maxHeight As Integer = 480
            Dim maxWidth As Integer = 640
            imageHeight = (imageHeight * maxWidth) / imageWidth
            imageWidth = maxWidth

            If imageHeight > maxHeight Then
                imageWidth = (imageWidth * maxHeight) / imageHeight
                imageHeight = maxHeight
            End If

            Dim bitmap As New Bitmap(imageToBeResized, imageWidth, imageHeight)
            Dim stream As System.IO.MemoryStream = New MemoryStream()
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg)
            stream.Position = 0
            Dim image As Byte() = New Byte(stream.Length) {}
            stream.Read(image, 0, image.Length)
            Return image
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function ConvertImageToByteArray(ByVal fuImage As FileUpload) As Byte()
        Dim ImageByteArray As Byte()
        Try
            Dim ms As MemoryStream = New MemoryStream(fuImage.FileBytes)
            ImageByteArray = ms.ToArray()
            Return ImageByteArray
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class

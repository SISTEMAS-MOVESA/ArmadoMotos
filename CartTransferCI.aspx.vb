Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports DocumentFormat.OpenXml.Spreadsheet

Partial Class CartTransferCI
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
    Dim MyTable As New DataTable
    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public xCompany As SAPbobsCOM.Company
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public argumento As String
    Public IdentidadMotorista As String
    Public MACRO_ID As String
    Public SelectedRecords As New List(Of Dictionary(Of String, String))()

    Public Function GlobalConnecttoSAP() As Integer
        SCompany = New SAPbobsCOM.Company
        SCompany = New SAPbobsCOM.Company
        SCompany.Server = "192.168.1.3"
        SCompany.CompanyDB = "MOVESA"
        SCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
        SCompany.DbUserName = "sa"
        SCompany.DbPassword = "M*l!n3r0s2k12"
        SCompany.UserName = "itpro"
        SCompany.Password = "eeal96"
        SCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
        SCompany.SLDServer = "192.168.1.9:40000"
        lRetCode = SCompany.Connect
        Return lRetCode
    End Function

    <WebMethod()>
    Public Shared Function SearchWhsname(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon2
            Using cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = " select whsname from OWHS with(nolock)   where whsname LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("whsname").ToString())
                    End While
                End Using
                conn.Close()
                Return customers
            End Using
        End Using
    End Function
    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'BindGrid(Session("User"))
                    Dim cartCount As Integer = ObtenerConteoLineasUsuario(Session("User"))
                    'lblCartCount.Text = cartCount.ToString()

                    CargarMotivoTraslado()
                    CargarMotoristas()
                End If
            End If
        Catch ex As Exception

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
        sName = Replace(sName, ",", sChr)
        NewStr = sName
        Return NewStr

    End Function
    Private Function ObtenerConteoLineasUsuario(usuario As String) As Integer
        Dim conteo As Integer = 0
        Try
            Dim connectionString As String = sCon1
            Dim query As String = "SELECT COUNT(*) FROM [dbo].[SOLICITU_TRANSFER_TEMP] WHERE [USUARIO] = @USUARIO"

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@USUARIO", usuario)
                    conteo = Convert.ToInt32(cmd.ExecuteScalar())
                End Using
                conn.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('ObtenerConteoLineasUsuario: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try

        Return conteo
    End Function

    Private Sub btnAlmDestino_Click(sender As Object, e As EventArgs) Handles btnAlmDestino.Click
        Try
            BuscarDatosAlmacenDestino(txtNalmacenDestino.Text)
            TraerEstadoClienteDestino(txtCodigoClienteDestino.Text)
            SaldoConsignacionDestino(txtCodigoClienteDestino.Text)
            If (CDec(txtSaldoClienteAD.Text) + CDec(txtSaldoConsignaClienteAD.Text)) > CDec(txtLimiteClienteAD.Text) Then
                txtLimiteClienteAD.Style("border") = "2px solid red"
                txtSaldoClienteAD.Style("border") = "2px solid red"
                txtSaldoConsignaClienteAD.Style("border") = "2px solid red"
                lstSeriesDisponiblesAlmacen.Visible = False
                Dim script As String = "<script>iziToast.error({title: 'Error!', message: 'No procede la Solicitud, por favor comuniquese con Creditos y Logistica!',position: 'topRight',timeout: 5000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                Response.Write("<script>console.log('No puede proceder con la solicitud, por favor comuniquese con Creditos y Logistica!');</script>")

            End If
        Catch ex As Exception
            Response.Write("<script>console.log('btnAlmDestino_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnAlmOrigen_Click(sender As Object, e As EventArgs) Handles btnAlmOrigen.Click
        Try
            BuscarDatosAlmacenOrigen(txtNalmacenOrigen.Text)
            SaldoConsignacion(txtCodeAlmacenOrigen.Text)
            TraerEstadoCliente(txtCodigoClienteOrigen.Text)
            CargarSeries(txtCodeAlmacenOrigen.Text)

            If (CDec(txtSaldoCuenta.Text) + CDec(txtSaldoConsignacion.Text)) > CDec(txtLimiteCredito.Text) Then
                txtLimiteCredito.Style("border") = "2px solid red"
                txtSaldoCuenta.Style("border") = "2px solid red"
                txtSaldoConsignacion.Style("border") = "2px solid red"
                lstSeriesDisponiblesAlmacen.Visible = False
                Dim script As String = "<script>iziToast.error({title: 'Error!', message: 'No puede proceder con la solicitud, por favor comuniquese con Creditos y Logistica!',position: 'topRight',timeout: 5000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                Response.Write("<script>console.log('No procede la Solicitud, por favor comuniquese con Creditos y Logistica!');</script>")

            End If

        Catch ex As Exception
            Response.Write("<script>console.log('btnAlmOrigen_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub CargarSeries(whscode As String)
        Try
            Dim constr As String = sCon2
            Using con As SqlConnection = New SqlConnection(constr)
                Dim SQL_string As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/CartTransferCI_CargarSeries.sql"))
                Using cmd As SqlCommand = New SqlCommand(SQL_string)
                    cmd.Parameters.AddWithValue("@whscode", whscode)
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    con.Open()
                    lstSeriesDisponiblesAlmacen.DataSource = cmd.ExecuteReader()
                    lstSeriesDisponiblesAlmacen.DataTextField = "Descripcion"
                    lstSeriesDisponiblesAlmacen.DataValueField = "SerieSYS"
                    lstSeriesDisponiblesAlmacen.DataBind()
                    con.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("CargarSeries " & ex.Message)
            Response.Write("<script>console.log('CargarSeries: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub BuscarDatosAlmacenOrigen(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " select t0.WhsCode,t0.WhsName,t0.U_CardCode,t1.Name [Ruta], t2.Name [Responsable] " &
                                    " from OWHS t0 With(nolock) full join [@CRUTA] t1 With(nolock) On t0.U_Ruta=t1.Code " &
                                    " full join [@CRESPONSABLEOWHS] t2 with(nolock) on t0.U_Categorizacion=t2.Code " &
                                    " where t0.whsname = @whsname "


            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@whsname", whsname)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then
                txtCodeAlmacenOrigen.Text = drd.Item("WhsCode").ToString
                txtNalmacenOrigen.Text = drd.Item("WhsName").ToString
                txtRutaAlmacenOrigen.Text = drd.Item("Ruta").ToString
                txtEncargadoOrigen.Text = drd.Item("Responsable").ToString
                txtCodigoClienteOrigen.Text = drd.Item("U_CardCode").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('BuscarDatosAlmacenOrigen: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub BuscarDatosAlmacenDestino(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " select t0.WhsCode,t0.WhsName,t0.U_CardCode,t1.Name [Ruta], t2.Name [Responsable] " &
                                    " from OWHS t0 With(nolock) full join [@CRUTA] t1 With(nolock) On t0.U_Ruta=t1.Code " &
                                    " full join [@CRESPONSABLEOWHS] t2 with(nolock) on t0.U_Categorizacion=t2.Code " &
                                    " where t0.whsname = @whsname "


            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@whsname", whsname)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then
                txtCodeAlmDestino.Text = drd.Item("WhsCode").ToString
                txtNalmacenDestino.Text = drd.Item("WhsName").ToString
                txtRutaAlmacenDestino.Text = drd.Item("Ruta").ToString
                txtEncargadoDestino.Text = drd.Item("Responsable").ToString
                txtCodigoClienteDestino.Text = drd.Item("U_CardCode").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('BuscarDatosAlmacenOrigen: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub TraerEstadoCliente(cardcode As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = "select frozenFor,Balance,CreditLine  from movesa..ocrd where CardCode = @cardcode"


            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@cardcode", cardcode)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then

                If drd.Item("frozenFor").ToString = "Y" Then
                    txtEstatusCliente.Text = "BLOQUEADO"
                    txtEstatusCliente.Style("border") = "2px solid red"
                    lstSeriesDisponiblesAlmacen.Visible = False
                    Dim script As String = "<script>iziToast.error({title: 'Error!', message: 'No puede proceder con la solicitud, por favor comuniquese con Creditos y Logistica!',position: 'topRight',timeout: 5000})</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                Else
                    txtEstatusCliente.Text = "ACTIVO"
                End If

                txtLimiteCredito.Text = FormatNumber(drd.Item("CreditLine").ToString, 2, TriState.True)
                txtSaldoCuenta.Text = FormatNumber(drd.Item("Balance").ToString, 2, TriState.True)
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('TraerEstadoCliente: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub TraerEstadoClienteDestino(cardcode As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = "select frozenFor,Balance,CreditLine  from movesa..ocrd where CardCode = @cardcode"


            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@cardcode", cardcode)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then

                If drd.Item("frozenFor").ToString = "Y" Then
                    txtEstadoClienteAD.Text = "BLOQUEADO"
                    txtEstadoClienteAD.Style("border") = "2px solid red"
                    lstSeriesDisponiblesAlmacen.Visible = False
                    Dim script As String = "<script>iziToast.error({title: 'Error!', message: 'No puede proceder con la solicitud, por favor comuniquese con Creditos y Logistica!',position: 'topRight',timeout: 5000})</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                Else
                    txtEstadoClienteAD.Text = "ACTIVO"
                End If
                txtLimiteClienteAD.Text = FormatNumber(drd.Item("CreditLine").ToString, 2, TriState.True)
                txtSaldoClienteAD.Text = FormatNumber(drd.Item("Balance").ToString, 2, TriState.True)
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('TraerEstadoClienteDestino: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub SaldoConsignacion(cardcode As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = "select isnull(SUM(CONVERT(numeric(18,2),T3.Price)),0) [Total]" &
                                        " From OWHS t0 " &
                                        " inner join OSRI t1 on t0.WhsCode = t1.WhsCode" &
                                        " inner Join OCRD t2 on t0.U_CardCode = t2.CardCode" &
                                        " INNER JOIN OSPP T3 ON T3.ItemCode = T1.ItemCode AND T3.CardCode = T2.CardCode" &
                                        " INNER Join OITM T4 ON T4.ItemCode = T1.ItemCode " &
                                        " LEFT JOIN [@AMODELO] T5 ON T4.U_AMODELO = T5.Code" &
                                        " Left Join(SELECT  distinct t1.ItemCode, t0.DocDate, t0.BaseNum, t1.MnfSerial, t1.SysNumber" &
                                        " FROM SRI1  t0 " &
                                        " inner Join OSRN t1 on t0.SysSerial=t1.SysNumber And t0.ItemCode = t1.ItemCode And t0.BaseType = 67" &
                                        " WHERE  t0.LineNum = (select  max(LineNum) from SRI1  tt0 " &
                                        " inner Join OSRN tt1 on tt0.SysSerial=tt1.SysNumber And tt0.BaseType = 67 " &
                                        " where tt0.ItemCode = t0.ItemCode and tt1.SysNumber = t1.SysNumber)  ) t12 on t12.ItemCode = t1.ItemCode and  t12.MnfSerial = t1.SuppSerial" &
                                        " where t2.CardCode = @cardcode " &
                                        " And T1.[Status] = 0"

            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@cardcode", cardcode)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then
                txtSaldoConsignacion.Text = FormatNumber(drd.Item("Total").ToString, 2, TriState.True)
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('SaldoConsignacion: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub SaldoConsignacionDestino(cardcode As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = "select isnull(SUM(CONVERT(numeric(18,2),T3.Price)),0) [Total]" &
                                        " From OWHS t0 " &
                                        " inner join OSRI t1 on t0.WhsCode = t1.WhsCode" &
                                        " inner Join OCRD t2 on t0.U_CardCode = t2.CardCode" &
                                        " INNER JOIN OSPP T3 ON T3.ItemCode = T1.ItemCode AND T3.CardCode = T2.CardCode" &
                                        " INNER Join OITM T4 ON T4.ItemCode = T1.ItemCode " &
                                        " LEFT JOIN [@AMODELO] T5 ON T4.U_AMODELO = T5.Code" &
                                        " Left Join(SELECT  distinct t1.ItemCode, t0.DocDate, t0.BaseNum, t1.MnfSerial, t1.SysNumber" &
                                        " FROM SRI1  t0 " &
                                        " inner Join OSRN t1 on t0.SysSerial=t1.SysNumber And t0.ItemCode = t1.ItemCode And t0.BaseType = 67" &
                                        " WHERE  t0.LineNum = (select  max(LineNum) from SRI1  tt0 " &
                                        " inner Join OSRN tt1 on tt0.SysSerial=tt1.SysNumber And tt0.BaseType = 67 " &
                                        " where tt0.ItemCode = t0.ItemCode and tt1.SysNumber = t1.SysNumber)  ) t12 on t12.ItemCode = t1.ItemCode and  t12.MnfSerial = t1.SuppSerial" &
                                        " where t2.CardCode = @cardcode " &
                                        " And T1.[Status] = 0"



            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@cardcode", cardcode)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then
                txtSaldoConsignaClienteAD.Text = FormatNumber(drd.Item("Total").ToString, 2, TriState.True)
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('SaldoConsignacionDestino: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub btnLimpiarFormulario_Click(sender As Object, e As EventArgs) Handles btnLimpiarFormulario.Click
        Response.Redirect("CartTransferCI.aspx")
    End Sub
    Public Sub CargarMotoristas()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon2)
                Dim query As String = "select LICENCIA,LICENCIA + ' ' + RTN + ' ' + EMPRESA + ' ' + PLACA [Empresa] from movesaweb..LOGISTICA_PLACAS_TRASLADOS order by EMPRESA"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
            drpMotoristas.Dispose()
            drpMotoristas.DataTextField = "Empresa"
            drpMotoristas.DataValueField = "LICENCIA"
            drpMotoristas.DataSource = dt
            drpMotoristas.DataBind()

        Catch ex As Exception
            Response.Write(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Public Sub CargarMotivoTraslado()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon2)
                Dim query As String = "SELECT [Code],[Name]  FROM [MOVESA].[dbo].[@MOTIVOSTRASLADO] order by Name"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
            drpMotivoTraslado.Dispose()
            drpMotivoTraslado.DataTextField = "Name"
            drpMotivoTraslado.DataValueField = "Code"
            drpMotivoTraslado.DataSource = dt
            drpMotivoTraslado.DataBind()

        Catch ex As Exception
            Response.Write(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnCrearSOlicitud_Click(sender As Object, e As EventArgs) Handles btnCrearSOlicitud.Click
        Try
            ' Conexión a SAP
            If GlobalConnecttoSAP() <> 0 Then
                Response.Write("<script>alert('Error al Intentar Conectarse a SAP');</script>")
                Exit Sub
            End If

            ' Crear solicitud de transferencia
            Dim oStockTransferRequest As SAPbobsCOM.StockTransfer = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryTransferRequest)

            ' Asignar la serie en función del almacén de destino
            oStockTransferRequest.Series = If(GetTypeWhsCode(txtCodeAlmDestino.Text) <> "PRO", 968, 243)

            ' Configurar datos del encabezado
            oStockTransferRequest.DocDate = DateTime.Now
            oStockTransferRequest.FromWarehouse = txtCodeAlmacenOrigen.Text
            oStockTransferRequest.ToWarehouse = txtCodeAlmDestino.Text
            oStockTransferRequest.CardCode = txtCodigoClienteDestino.Text
            oStockTransferRequest.Comments = txtComments.Text & " Solicitud de Traslado Via Portal Produccion de Motos " & Session("User") & " " & Session("Name")
            oStockTransferRequest.JournalMemo = GetRuta(txtCodeAlmDestino.Text)
            oStockTransferRequest.Reference1 = Session("User")
            oStockTransferRequest.PriceList = -2

            ' Informacion del transportista
            oStockTransferRequest.UserFields.Fields.Item("U_IdentConductor").Value = drpMotoristas.SelectedValue.ToString
            oStockTransferRequest.UserFields.Fields.Item("U_Mottras").Value = drpMotivoTraslado.SelectedValue.ToString
            oStockTransferRequest.UserFields.Fields.Item("U_TipoTransporte").Value = drpTipoTransporte.SelectedValue.ToString
            oStockTransferRequest.UserFields.Fields.Item("U_Envio").Value = "PP_ST"
            oStockTransferRequest.UserFields.Fields.Item("U_Espacios").Value = "2"

            ' Obtener datos del transportista y asignarlos
            Dim datos As String(,) = obtenerDatos(drpMotoristas.SelectedValue.ToString)
            If datos.GetLength(1) > 0 Then
                oStockTransferRequest.UserFields.Fields.Item("U_Transportista").Value = datos(0, 0)
                oStockTransferRequest.UserFields.Fields.Item("U_Conductor").Value = datos(1, 0)
                oStockTransferRequest.UserFields.Fields.Item("U_LicenciaConductor").Value = datos(2, 0)
                oStockTransferRequest.UserFields.Fields.Item("U_MarcaModelo").Value = datos(3, 0)
                oStockTransferRequest.UserFields.Fields.Item("U_placaTransporte").Value = datos(4, 0)
                oStockTransferRequest.UserFields.Fields.Item("U_RTNTransportista").Value = datos(5, 0)
            End If

            ' Recorrer los ítems seleccionados en la lista
            For Each item As ListItem In lstSeriesDisponiblesAlmacen.Items
                If item.Selected Then
                    Dim rawText As String = item.Text
                    Dim keywords As String() = {"Serie", "Descripcion", "Year", "Dias Alm", "Dias Movesa"}
                    Dim cleanedText As String = keywords.Aggregate(rawText, Function(current, keyword) current.Replace(keyword, "").Trim())
                    Dim values As String() = cleanedText.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

                    If values.Length >= 2 Then
                        ' Agregar línea al documento
                        oStockTransferRequest.Lines.ItemCode = Getitemcode(values(1))
                        oStockTransferRequest.Lines.SerialNumber = values(1)
                        oStockTransferRequest.Lines.UserFields.Fields.Item("U_MSERIE").Value = values(1)
                        oStockTransferRequest.Lines.UserFields.Fields.Item("U_MVin").Value = item.Value
                        oStockTransferRequest.Lines.Quantity = 1

                        Dim datosMoto As String(,) = obtenerDatosMoto(values(1))
                        If datos.GetLength(1) > 0 Then
                            oStockTransferRequest.Lines.UserFields.Fields.Item("U_MMOTOR").Value = datosMoto(0, 0)
                            oStockTransferRequest.Lines.UserFields.Fields.Item("U_MColor").Value = datosMoto(1, 0)
                            oStockTransferRequest.Lines.UserFields.Fields.Item("U_MAno").Value = datosMoto(2, 0)
                            oStockTransferRequest.Lines.UserFields.Fields.Item("U_MModelo").Value = datosMoto(3, 0)
                            oStockTransferRequest.Lines.UserFields.Fields.Item("U_MCilindros").Value = datosMoto(4, 0)
                            oStockTransferRequest.Lines.UserFields.Fields.Item("U_MMarca").Value = datosMoto(5, 0)
                        End If

                        oStockTransferRequest.Lines.Add()
                    Else
                        Response.Write("<script>console.warn('Datos insuficientes para procesar el registro');</script>")
                        Exit Sub
                    End If
                End If
            Next

            ' Intentar añadir el documento
            Dim iError As Integer = oStockTransferRequest.Add()
            If iError <> 0 Then
                Dim sErrMsg As String = SCompany.GetLastErrorDescription()
                Throw New Exception(sErrMsg)
            Else
                Dim sDocNum As String = SCompany.GetNewObjectKey()
                Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Documento Creado!!! # " &
                                    GetDocumentoCreado(sDocNum) & "',position: 'topRight',timeout: 10000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            End If
        Catch ex As Exception
            ' Manejo de errores
            Response.Write("CreateStockTransferRequest: " & ex.Message)
            Response.Write("<script>console.log('CreateStockTransferRequest: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function GetRuta(whscode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = " select t1.Name [Ruta] from OWHS t0 With(nolock) full join [@CRUTA] t1 With(nolock) On t0.U_Ruta=t1.Code where t0.whscode = @p1 "
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetRuta: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Public Function Getitemcode(whscode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT itemcode FROM [MOVESA].[dbo].[OSRN] where MnfSerial=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('Getitemcode: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Public Function GetTypeWhsCode(whscode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT isnull([u_type],'PRO')[u_type] FROM [MOVESA].[dbo].[OWHS] where WHSCODE=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetTypeWhsCode: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Public Function GetDocumentoCreado(docentry As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "select docnum from owtq with(nolock) where docentry=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", docentry)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetDocumentoCreado: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Public Function GetCardCodeByWHS(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "select u_Cardcode from owhs with(nolock) where WhsCode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetCardCodeByWHS: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Public Sub UpdateDocentrySAP(docentrysap As String, fecha As DateTime, camion As String, MACROID As String)
        Try
            Response.Write("<script>console.log('Parametros: " & ReplaceCharsForFileName(docentrysap & " " & fecha & " " & camion & " " & MACROID, " ") & "');</script>")

            Dim sCon As String = sCon2
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[SOLCITUD_HEADER] Set [DOCENTRYSAP]=@p1 ,[DATEPROCESEDSAP]=@p2,[ESTATUS] = 'PROCESADOSAP' where [CAMION]=@p3;"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", docentrysap)
                cmd.Parameters.AddWithValue("@p2", fecha)
                cmd.Parameters.AddWithValue("@p3", camion)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Camion Creado con Exito!');", True)
            End Using
        Catch ex As Exception
            Response.Write("UpdateDocentrySAP " & ex.Message)
            Response.Write("<script>console.log('UpdateDocentrySAP: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Function obtenerDatos(RTN As String) As String(,)
        'Conectar a la base de datos
        Dim conn As New SqlConnection(sCon2)
        conn.Open()

        Dim sql As String = "select EMPRESA,MOTORISTA,LICENCIA,MARCAMODELO,PLACA,RTN from movesaweb..LOGISTICA_PLACAS_TRASLADOS WHERE LICENCIA= @p1"
        Dim cmd As New SqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("@p1", RTN)
        Dim reader As SqlDataReader = cmd.ExecuteReader()
        Dim numResultados As Integer = 0
        While reader.Read()
            numResultados += 1
        End While
        reader.Close()
        reader = cmd.ExecuteReader()

        Dim datos(,) As String = New String(6, numResultados - 1) {}
        Dim fila As Integer = 0
        While reader.Read()
            datos(0, fila) = reader.GetString(0)
            datos(1, fila) = reader.GetString(1)
            datos(2, fila) = reader.GetString(2)
            datos(3, fila) = reader.GetString(3)
            datos(4, fila) = reader.GetString(4)
            datos(5, fila) = reader.GetString(5)
            fila += 1
        End While
        reader.Close()
        conn.Close()
        Return datos
    End Function
    Function obtenerDatosMoto(SERIE As String) As String(,)
        'Conectar a la base de datos
        Dim conn As New SqlConnection(sCon2)
        conn.Open()

        Dim sql As String = "SELECT	T0.DistNumber,t3.Name, t0.LotNumber, T2.Name, t4.Name,t5.Name " &
            " FROM " &
            " OSRN T0 WITH(NOLOCK) INNER JOIN OITM T1 WITH(NOLOCK) ON T0.ItemCode=T1.ItemCode " &
            " INNER JOIN [@AMODELO] T2 WITH(NOLOCK) ON T1.U_AMODELO=T2.Code  " &
            " inner join [@SCOLOR] t3 with(nolock) on t1.u_acolor=t3.Code  " &
            " inner join [@ACILINDROS] t4 with(nolock) on t1.U_ACILINDROS = t4.Code " &
            " inner join [@AMARCA] t5 with(nolock) on t1.U_AMARCA = t5.Code " &
            " WHERE T0.MnfSerial=@p1"

        Dim cmd As New SqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("@p1", SERIE)
        Dim reader As SqlDataReader = cmd.ExecuteReader()
        Dim numResultados As Integer = 0
        While reader.Read()
            numResultados += 1
        End While
        reader.Close()
        reader = cmd.ExecuteReader()

        Dim datos(,) As String = New String(6, numResultados - 1) {}
        Dim fila As Integer = 0
        While reader.Read()
            datos(0, fila) = reader.GetString(0)
            datos(1, fila) = reader.GetString(1)
            datos(2, fila) = reader.GetString(2)
            datos(3, fila) = reader.GetString(3)
            datos(4, fila) = reader.GetString(4)
            datos(5, fila) = reader.GetString(5)
            fila += 1
        End While
        reader.Close()
        conn.Close()
        Return datos
    End Function
End Class

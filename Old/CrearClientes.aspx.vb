Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Partial Class CrearClientes
    Inherits System.Web.UI.Page

    Public xCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public _DataSet As DataSet
    Public _CmdText As String
    Public _Command As SqlClient.SqlCommand
    Public _CommandTimeOut As Integer = 1200
    Public _Adapter As SqlClient.SqlDataAdapter
    Public _Table As DataTable
    Public MOVESA As String = "server=192.168.1.3;database=MOVESA;uid=RJOVEL;password=Jovelo32"
    Public _ConnDrop As String = "Data Source=192.168.1.3;Initial Catalog=MOVESA;Persist Security Info=True;User ID=rjovel;Password=Jovelo32"
    Public SKG As String = ("Data Source=192.168.1.60;Initial Catalog=SKG_BP;User ID=sa;Password=S@pB1Sql")
    Public Con As New SqlClient.SqlConnection
    Private Sub CrearClientes_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then

                If String.IsNullOrEmpty(Session("Name").ToString()) Then
                    Response.Redirect("Default.aspx")
                End If
                _CargarGrupoCliente()
                _CargarVendedores()
                _CargarDepartamento()
                _CargarSucursal()
                txtCreadoPor.Text = Session("Name").ToString()
                txtCreadoPor.ReadOnly = True
                drpVendedores.SelectedIndex = drpVendedores.Items.IndexOf(drpVendedores.Items.FindByText(Session("User").ToString()))
            End If

            For Each ctrl As TextBox In form1.Controls.OfType(Of TextBox)()
                ctrl.Enabled = False
            Next

            For Each drpCtrl As DropDownList In form1.Controls.OfType(Of DropDownList)()
                drpCtrl.Enabled = False
            Next

            If drpSucursal.SelectedValue = "" And Session("User").ToString() <> "SK0000" Then
                txtCodigoSKG.Enabled = False
                btn2.Visible = False
                btn3.Visible = False
                Button1.Visible = False
            Else
                txtCodigoSKG.Enabled = True
            End If
        Catch ex As Exception
            Response.Redirect("Default.aspx")
        End Try
    End Sub
    Protected Sub btn1_Click(sender As Object, e As EventArgs) Handles btn1.Click
        Response.Redirect("Default.aspx")
    End Sub
    Protected Sub btn2_Click(sender As Object, e As EventArgs) Handles btn2.Click
        For Each ctrl As TextBox In form1.Controls.OfType(Of TextBox)()
            ctrl.Enabled = True
            ctrl.Text = ""
        Next
        For Each drpCtrl As DropDownList In form1.Controls.OfType(Of DropDownList)()
            drpCtrl.Enabled = True
        Next
        btn3.Enabled = True
    End Sub
    Protected Sub btn3_Click(sender As Object, e As EventArgs) Handles btn3.Click
        Try
            For Each ctrl As TextBox In form1.Controls.OfType(Of TextBox)()
                ctrl.Enabled = False
            Next
            For Each drpCtrl As DropDownList In form1.Controls.OfType(Of DropDownList)()
                drpCtrl.Enabled = False
            Next
            btn2.Enabled = False
            btn3.Enabled = False
            Button1.Enabled = False
            btn2.Visible = False
            btn3.Visible = False
            Button1.Visible = False
            login_sp()
            BusinessPartner(UCase(txtCodigoSKG.Text))
        Catch ex As Exception
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
            txtComentarios.Text = ex.Message
        End Try
    End Sub
    Public Sub login_sp()
        Try
            xCompany = New SAPbobsCOM.Company
            xCompany.Server = "192.168.1.3"
            xCompany.CompanyDB = "MOVESA"
            xCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
            xCompany.DbUserName = "sa"
            xCompany.DbPassword = "M*l!n3r0s2k12"
            xCompany.UserName = "it"
            xCompany.Password = "polar"
            xCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
            xCompany.SLDServer = "192.168.1.9:40000"
            lRetCode = xCompany.Connect

            If lRetCode <> 0 Then
                xCompany.GetLastError(lErrCode, sErrMsg)
                Response.Write("<script>alert('" & xCompany.GetLastErrorDescription & " ');</script>")
            End If
        Catch ex As Exception
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
        End Try
    End Sub
    Public Sub BusinessPartner(ByVal _CardCode As String)
        Dim xBusinessPartners As SAPbobsCOM.BusinessPartners
        xBusinessPartners = xCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
        If xBusinessPartners.GetByKey(_CardCode) = True Then

            With xBusinessPartners
                .CardCode = _CardCode
                .CardName = txtNcliente.Text
                .CardForeignName = txtIdentidad.Text
                .Currency = "LPS"
                .GroupCode = 116
                .FederalTaxID = "000000000000"
                .AdditionalID = txtIdentidad.Text
                .UnifiedFederalTaxID = txtRTN.Text
                .Phone1 = txtTelefono1.Text.Trim
                .EmailAddress = txtCorreo.Text
                .PayTermsGrpCode = 4
                .PriceListNum = 1
                If Session("User").ToString() <> "SK0000" Then
                    .SalesPersonCode = drpVendedores.SelectedValue
                    .UserFields.Fields.Item("U_Canal").Value() = "01"
                Else
                    .SalesPersonCode = "-1"
                    .UserFields.Fields.Item("U_Canal").Value() = "02"
                End If
                If Session("User").ToString() <> "SK0000" Then
                    .Properties(drpSucursal.SelectedValue) = SAPbobsCOM.BoYesNoEnum.tYES
                End If
                .Valid = SAPbobsCOM.BoYesNoEnum.tNO
                .Frozen = SAPbobsCOM.BoYesNoEnum.tYES
                .FrozenFrom = DateAndTime.DateAdd(DateInterval.Day, 1, DateTime.Now)
                .FatherCard = "SK100000"

                .Addresses.SetCurrentLine(0)
                .Addresses.AddressName = "BILL TO"
                .Addresses.Street = txtCalle.Text
                .Addresses.Block = Left(txtColonia.Text, 100)
                .Addresses.City = drpMunicipio.Text
                .Addresses.County = drpDepartamento.SelectedItem.Text
                .Addresses.State = drpDepartamento.Text
                .Addresses.Country = "HN"
                .Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                .Addresses.AddressName3 = txtCasa.Text
                .Addresses.Add()

                .Addresses.SetCurrentLine(1)
                .Addresses.AddressName = "SHIP TO"
                .Addresses.Street = txtCalle.Text
                .Addresses.Block = Left(txtColonia.Text, 100)
                .Addresses.City = drpMunicipio.Text
                .Addresses.County = drpDepartamento.SelectedItem.Text
                .Addresses.State = drpDepartamento.Text
                .Addresses.Country = "HN"
                .Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                .Addresses.AddressName3 = txtCasa.Text
                .Addresses.TaxCode = "ISV"
                .Addresses.Add()

                'CAMPOS DE USUARIO
                .UserFields.Fields.Item("U_Empresa").Value() = "2"

            End With
            If xBusinessPartners.Update <> 0 Then
                Response.Write("<script>alert('Update: " & xCompany.GetLastErrorDescription & " ');</script>")
                Runtime.InteropServices.Marshal.ReleaseComObject(xBusinessPartners)
                Runtime.InteropServices.Marshal.ReleaseComObject(xCompany)
                GC.Collect()
                GC.WaitForPendingFinalizers()
                xBusinessPartners = Nothing
                xCompany = Nothing
            Else
                Response.Write("<script>alert('Update:  " & _CardCode & " Actualizado Con Exito');</script>")
            End If
        Else
            With xBusinessPartners
                .CardCode = _CardCode
                .CardName = txtNcliente.Text
                .CardForeignName = txtIdentidad.Text
                .Currency = "LPS"
                .GroupCode = 116
                .FederalTaxID = "000000000000"
                .AdditionalID = txtIdentidad.Text
                .UnifiedFederalTaxID = txtRTN.Text
                .Phone1 = txtTelefono1.Text.Trim
                .EmailAddress = txtCorreo.Text
                .Notes = txtCreadoPor.Text + " " + DateTime.Now.ToString
                .CardType = SAPbobsCOM.BoCardTypes.cCustomer
                .PayTermsGrpCode = 4
                .PriceListNum = 1
                If Session("User").ToString() <> "SK0000" Then
                    .SalesPersonCode = drpVendedores.SelectedValue
                    .UserFields.Fields.Item("U_Canal").Value() = "01"
                Else
                    .SalesPersonCode = "-1"
                    .UserFields.Fields.Item("U_Canal").Value() = "02"
                End If
                If Session("User").ToString() <> "SK0000" Then
                    .Properties(drpSucursal.SelectedValue) = SAPbobsCOM.BoYesNoEnum.tYES
                End If
                .Valid = SAPbobsCOM.BoYesNoEnum.tNO
                .Frozen = SAPbobsCOM.BoYesNoEnum.tYES
                .FrozenFrom = DateAndTime.DateAdd(DateInterval.Day, 1, DateTime.Now)
                .FatherCard = "SK100000"

                'Persona de Contacto
                .ContactEmployees.Add()
                .ContactEmployees.SetCurrentLine(0)
                .Addresses.AddressName = "BILL TO"
                .Addresses.Street = txtCalle.Text
                .Addresses.Block = Left(txtColonia.Text, 100)
                .Addresses.City = drpMunicipio.Text
                .Addresses.County = drpDepartamento.SelectedItem.Text
                .Addresses.State = drpDepartamento.SelectedValue
                .Addresses.Country = "HN"
                .Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                .Addresses.AddressName3 = txtCasa.Text
                .Addresses.Add()

                .Addresses.AddressName = "SHIP TO"
                .Addresses.Street = txtCalle.Text
                .Addresses.Block = Left(txtColonia.Text, 100)
                .Addresses.City = drpMunicipio.Text
                .Addresses.County = drpDepartamento.SelectedItem.Text
                .Addresses.State = drpDepartamento.SelectedValue
                .Addresses.Country = "HN"
                .Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                .Addresses.AddressName3 = txtCasa.Text
                .Addresses.TaxCode = "ISV"
                .Addresses.Add()

                'CAMPOS DE USUARIO
                .UserFields.Fields.Item("U_Empresa").Value() = "2"

            End With

            If xBusinessPartners.Add <> 0 Then
                Response.Write("<script>alert('Crear: " & xCompany.GetLastErrorDescription & " ');</script>")
                Runtime.InteropServices.Marshal.ReleaseComObject(xBusinessPartners)
                Runtime.InteropServices.Marshal.ReleaseComObject(xCompany)
                GC.Collect()
                GC.WaitForPendingFinalizers()
                xBusinessPartners = Nothing
                xCompany = Nothing
                txtComentarios.Text = xCompany.GetLastErrorDescription
            Else
                Response.Write("<script>alert('Crear: " & _CardCode & " Creado Con Exito');</script>")
                SendEmail(_CardCode, txtNcliente.Text, Session("Name").ToString(), "rjovel@grupomovesa.com")
                Runtime.InteropServices.Marshal.ReleaseComObject(xBusinessPartners)
                Runtime.InteropServices.Marshal.ReleaseComObject(xCompany)
                GC.Collect()
                GC.WaitForPendingFinalizers()
                xBusinessPartners = Nothing
                xCompany = Nothing
            End If
        End If
    End Sub
    Public Sub JustUpdateBusinessPartner(ByVal _CardCode As String)
        Dim xBusinessPartners As SAPbobsCOM.BusinessPartners
        xBusinessPartners = xCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
        If xBusinessPartners.GetByKey(_CardCode) = True Then

            With xBusinessPartners
                .CardCode = _CardCode
                .CardName = txtNcliente.Text
                .CardForeignName = txtIdentidad.Text
                .Currency = "LPS"
                .FederalTaxID = "000000000000"
                .AdditionalID = txtIdentidad.Text
                .UnifiedFederalTaxID = txtRTN.Text
                .Phone1 = txtTelefono1.Text.Trim
                .EmailAddress = txtCorreo.Text
                .PayTermsGrpCode = 4
                .PriceListNum = 1
                If Session("User").ToString() <> "SK0000" Then
                    .SalesPersonCode = drpVendedores.SelectedValue
                Else
                    .SalesPersonCode = "-1"
                End If
                If Session("User").ToString() <> "SK0000" Then
                    .Properties(drpSucursal.SelectedValue) = SAPbobsCOM.BoYesNoEnum.tYES
                End If

                .Addresses.SetCurrentLine(0)
                .Addresses.AddressName = "BILL TO"
                .Addresses.Street = txtCalle.Text
                .Addresses.Block = txtColonia.Text
                .Addresses.City = drpMunicipio.Text
                .Addresses.County = drpDepartamento.SelectedItem.Text
                .Addresses.State = drpDepartamento.Text
                .Addresses.Country = "HN"
                .Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                .Addresses.AddressName3 = txtCasa.Text
                .Addresses.Add()

                .Addresses.SetCurrentLine(1)
                .Addresses.AddressName = "SHIP TO"
                .Addresses.Street = txtCalle.Text
                .Addresses.Block = txtColonia.Text
                .Addresses.City = drpMunicipio.Text
                .Addresses.County = drpDepartamento.SelectedItem.Text
                .Addresses.State = drpDepartamento.Text
                .Addresses.Country = "HN"
                .Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                .Addresses.AddressName3 = txtCasa.Text
                .Addresses.TaxCode = "ISV"
                .Addresses.Add()
                'CAMPOS DE USUARIO
                .UserFields.Fields.Item("U_Empresa").Value() = "2"
                .UserFields.Fields.Item("U_Tipo").Value() = "3"
            End With
            If xBusinessPartners.Update <> 0 Then
                Response.Write("<script>alert('Update: " & xCompany.GetLastErrorDescription & " ');</script>")
                Runtime.InteropServices.Marshal.ReleaseComObject(xBusinessPartners)
                Runtime.InteropServices.Marshal.ReleaseComObject(xCompany)
                GC.Collect()
                GC.WaitForPendingFinalizers()
                xBusinessPartners = Nothing
                xCompany = Nothing
            Else
                Response.Write("<script>alert('Update:  " & _CardCode & " Actualizado Con Exito');</script>")
            End If
        Else
            Exit Sub
        End If
    End Sub
    Public Sub _CargarGrupoCliente()
        Try
            Dim dt As DataTable = New DataTable()

            Using conn As SqlConnection = New SqlConnection(_ConnDrop)
                Dim query As String = "select GroupCode,GroupName from OCRG where GroupType='C' and groupcode=116"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using

            drpGrupoClientes.DataTextField = "GroupName"
            drpGrupoClientes.DataValueField = "GroupCode"
            drpGrupoClientes.DataSource = dt
            drpGrupoClientes.DataBind()
        Catch ex As Exception
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
        End Try
    End Sub
    Public Sub _CargarVendedores()
        Try
            Dim dt As DataTable = New DataTable()

            Using conn As SqlConnection = New SqlConnection(_ConnDrop)
                Dim query As String = "select SlpCode,SlpName from oslp with(nolock)"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using

            drpVendedores.DataTextField = "SlpName"
            drpVendedores.DataValueField = "SlpCode"
            drpVendedores.DataSource = dt
            drpVendedores.DataBind()
        Catch ex As Exception
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
        End Try
    End Sub
    Public Sub _CargarDepartamento()
        Try
            Dim dt As DataTable = New DataTable()

            Using conn As SqlConnection = New SqlConnection(_ConnDrop)
                Dim query As String = "select CODIGO_SAP AS  CODE, DEPARTAMENTO  AS [Depto] from [192.168.1.70].[MovesaWeb].[dbo].[DEPARTAMENTOS] order by [Depto]"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using

            drpDepartamento.DataTextField = "Depto"
            drpDepartamento.DataValueField = "CODE"
            drpDepartamento.DataSource = dt
            drpDepartamento.DataBind()

            _CargarMunicipio("D04")
        Catch ex As Exception
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
        End Try
    End Sub
    Public Sub _CargarMunicipio(_CodeDepto As String)
        Try
            Dim dt As DataTable = New DataTable()

            Using conn As SqlConnection = New SqlConnection("Data Source=192.168.1.3;Initial Catalog=MOVESA;Persist Security Info=True;User ID=rjovel;Password=Jovelo32")
                Dim query As String = "select MUNICIPIO from [192.168.1.70].[MovesaWeb].[dbo].[Municipios] WHERE CODIGO_SAP ='" & _CodeDepto & "' order by MUNICIPIO"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using

            drpMunicipio.DataTextField = "MUNICIPIO"
            drpMunicipio.DataValueField = "MUNICIPIO"
            drpMunicipio.DataSource = dt
            drpMunicipio.DataBind()
        Catch ex As Exception
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
        End Try
    End Sub
    Protected Sub drpDepartamento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpDepartamento.SelectedIndexChanged
        _CargarMunicipio(drpDepartamento.SelectedValue.ToString.Trim())
    End Sub
    Public Sub _CargarSucursal()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection("Data Source=192.168.1.3;Initial Catalog=MOVESA;Persist Security Info=True;User ID=rjovel;Password=Jovelo32")
                Dim query As String = "select GroupCode, GroupName from OCQG where LEFT(GroupName,6)  in  ((select WhsCode from owhs where WhsCode='" & Branch(Session("User").ToString()) & "'))"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using

            drpSucursal.DataTextField = "GroupName"
            drpSucursal.DataValueField = "GroupCode"
            drpSucursal.DataSource = dt
            drpSucursal.DataBind()
        Catch ex As Exception
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
        End Try
    End Sub
    Private Function Branch(ByVal _SlpCode As String) As String
        Dim StrReturnValue As String
        Dim strConnection As String = "server=192.168.1.3;database=MOVESA;uid=RJOVEL;pwd=Jovelo32;"
        Dim sqlConnection As SqlConnection = New SqlConnection(strConnection)
        Dim SQLQuery As String = "  select	t0.SlpCode,t0.SlpName,t2.Name from oslp t0 with(nolock) " &
                                 "  inner join " &
                                 "  OHEM t1 with(nolock) " &
                                 "  on t0.SlpCode=t1.salesPrson " &
                                 "  inner join " &
                                 "  OUBR t2 with(nolock) " &
                                 "  on t1.branch=t2.Code " &
                                 "  where t0.SlpName='" & _SlpCode & "'"

        Dim command As SqlCommand = New SqlCommand(SQLQuery, sqlConnection)
        Dim Dr As SqlDataReader
        sqlConnection.Open()
        Dr = command.ExecuteReader()
        While Dr.Read()
            StrReturnValue = Dr("Name").ToString.Trim
            Dr.Close()
            Return StrReturnValue
        End While
        If String.IsNullOrEmpty(StrReturnValue) Then
            StrReturnValue = "*"
            Return StrReturnValue
        End If
    End Function
    Public Sub SendEmail(ByVal _CARDCODE As String, ByVal _CARDNAME As String, ByVal _USUARIO As String, ByVal _CORREO As String)
        Try
            Dim server As New SmtpClient
            Dim mensaje As New MailMessage
            server.Host = "mail.grupomovesa.com"
            server.Port = "587"
            server.Credentials = New System.Net.NetworkCredential("notificaciones@grupomovesa.com", "VYgd}T!XcVy}")
            mensaje.From = New MailAddress("notificaciones@grupomovesa.com")

            mensaje.To.Add(_CORREO) '& ",")
            mensaje.Subject = "MOVESA - PORTAL CREACION USUARIOS"
            mensaje.Body = " El usuario: " & _USUARIO &
                            "<BR /> " &
                            "Codigo de CL: " & _CARDCODE &
                            "<BR /> " &
                            "Nombre CL: " & _CARDNAME &
                            "<BR /> " &
                            "<BR /> " &
                            "PORTAL CREACION CLIENTES By RJ"

            mensaje.IsBodyHtml = True
            mensaje.Priority = MailPriority.High
            server.Send(mensaje)
        Catch ex As Exception
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
        End Try
    End Sub
    Public Sub CLIENTE_SKG2(CARDCODESKG As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(SKG)
            Dim Consulta As String = "SELECT ClReferencia 'Referencia',T0.ClCliCod 'CodCLiente' ,ClNumID 'Identidad' ,ClIdeTrib 'RTN' , ClNomSocio 'NombreCliente',TelNumero 'Telefono' ,DirDireccion 'Direccion',Municipio 'Municipio',CodDeptoSAP 'Departamento','HN' 'Pais'" &
            " FROM [SKG_BP].[SIFCO].[ClClientes] t0 with (nolock) left join (select DISTINCT " &
            "t0.ClCliCod,t0.DirDireccion,t1.ApRegDes [Depto], " &
                            "Case t1.ApRegCod When 2 Then 'D04' " &
                            "when 7 then 'D14' " &
                            "when 3 then 'D09' " &
                            "when 4 then 'D05' " &
                            "when 5 then 'D12' " &
                            "when 6 then 'D02' " &
                            "when 8 then 'D10' " &
                            "when 1 then 'D03' " &
                            "when 9 then 'D19' " &
                            "when 10 then 'D11' " &
                            "when 11 then 'D18' " &
                            "when 12 then 'D15' " &
                            "when 13 then 'D07' " &
                            "when 18 then 'D16' " &
                            "when 14 then 'D06' " &
                            "when 15 then 'D01' " &
                            "when 16 then 'D13' " &
                            "when 17 then 'D08' " &
                            "end [CodDeptoSAP], " &
                            "t2.ApDepDes [Municipio],t3.ApMunDes [Col/Residencia] " &
            "from   [SKG_BP].[SIFCO].[ClDirecciones] t0   " &
            "left join [SKG_BP].[SIFCO].[ApRegiones] t1 on t0.DirRegCod = t1.ApRegCod " &
            "left join [SKG_BP].[SIFCO].[ApDepartamento] t2 on t0.DirDepCod = t2.ApDepCod and t1.ApRegCod = t2.ApRegCod " &
            "left join  [SKG_BP].[SIFCO].[ApMunicipio] t3 on  t1.ApRegCod = t3.ApRegCod and t0.DirMunCod = t3.ApMunCod and T0.DIRDEPCOD = t3.ApDepCod  " &
            "WHERE T0.DirTipo = 1   ) t1 on t0.ClCliCod = t1.ClCliCod " &
            "left join (select distinct t0.ClCliCod,t0.TelNumero FROM [SKG_BP].[SIFCO].[ClTelefonos] t0 WHERE TelTipo = '3') t2 on t0.ClCliCod = t2.ClCliCod " &
            "where t0.[ClReferencia] ='" & CARDCODESKG & "' "

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtNcliente.Text = drd.Item("NombreCliente").ToString()
                txtIdentidad.Text = drd.Item("Identidad").ToString()
                txtRTN.Text = drd.Item("RTN").ToString()
                txtTelefono1.Text = drd.Item("Telefono").ToString()
                txtTelefono2.Text = drd.Item("Telefono").ToString()
                txtCelular.Text = drd.Item("Telefono").ToString()
                txtColonia.Text = drd.Item("Direccion").ToString()
                drpDepartamento.SelectedIndex = drpDepartamento.Items.IndexOf(drpDepartamento.Items.FindByValue(drd.Item("Departamento").ToString()))
                _CargarMunicipio(drpDepartamento.SelectedValue)
                drpMunicipio.SelectedIndex = drpMunicipio.Items.IndexOf(drpMunicipio.Items.FindByText(Replace(drd.Item("Municipio").ToString(), "Tegucigalpa", "Distrito Central")))
                drpVendedores.SelectedIndex = drpVendedores.Items.IndexOf(drpVendedores.Items.FindByText(Session("User").ToString()))
                txtCorreo.Text = "A@A.com"
            End If
        Catch ex As Exception
            Response.Write("<script>alert('" & ex.Message & " ');</script>")
        End Try
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            CLIENTE_SKG2(txtCodigoSKG.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class

Imports System.Data
Imports System.Data.SqlClient
Imports DocumentFormat.OpenXml.ExtendedProperties
Imports DocumentFormat.OpenXml.Spreadsheet
Imports SAPbobsCOM

Partial Class SapNewItems
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public xCompany As SAPbobsCOM.Company
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public oGoodsRcpt As SAPbobsCOM.Documents
    'Public oItem As SAPbobsCOM.Items

    Private Sub SapNewItems_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    _cargarGrupoArticulos()
                    _cargarProveedores()
                    _cargarModelos()
                    _cargarAranceles()

                    Select Case Session("Position").ToString()
                        Case "Iniciador"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Supervisor Armado"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Calidad"
                            Response.Redirect("MainDashBoard.aspx")
                    End Select
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub _cargarGrupoArticulos()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "select itmsgrpcod,itmsgrpnam from oitb with(nolock) where itmsgrpcod not in (528,526,527)"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using

            drpGrupoArticulos.Dispose()
            drpGrupoArticulos.DataTextField = "itmsgrpnam"
            drpGrupoArticulos.DataValueField = "itmsgrpcod"
            drpGrupoArticulos.DataSource = dt
            drpGrupoArticulos.DataBind()
        Catch ex As Exception

            Response.Write(ex.Message)
        End Try
    End Sub
    'Public Sub _cargarSubGrupoArticulos()
    '    Try
    '        Dim dt As DataTable = New DataTable()
    '        Using conn As SqlConnection = New SqlConnection(sCon1)
    '            Dim query As String = "select code,name from [@SUBGRUPO] WITH(NOLOCK)"
    '            Dim cmd As SqlCommand = New SqlCommand(query, conn)
    '            Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
    '            da.Fill(dt)
    '            conn.Close()
    '        End Using

    '        drpSubGrupo.Dispose()
    '        drpSubGrupo.DataTextField = "name"
    '        drpSubGrupo.DataValueField = "code"
    '        drpSubGrupo.DataSource = dt
    '        drpSubGrupo.DataBind()
    '    Catch ex As Exception

    '        Response.Write(ex.Message)
    '    End Try
    'End Sub
    Public Sub _cargarProveedores()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "Select CardCode, CardName  from ocrd With(nolock) where CardType='S' and CardCode in (select CardCode from oitm with(nolock))"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using

            drpProveedores.Dispose()
            drpProveedores.DataTextField = "CardName"
            drpProveedores.DataValueField = "CardCode"
            drpProveedores.DataSource = dt
            drpProveedores.DataBind()
        Catch ex As Exception

            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub _cargarModelos()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "select Code,Name  from [@AMODELO] with(nolock) ORDER BY Name"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using

            drpModelo.Dispose()
            drpModelo.DataTextField = "Name"
            drpModelo.DataValueField = "Code"
            drpModelo.DataSource = dt
            drpModelo.DataBind()
        Catch ex As Exception

            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub _cargarAranceles()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "select CstGrpCode, CstGrpName from OARG with(nolock)"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using

            drpArancel.Dispose()
            drpArancel.DataTextField = "CstGrpName"
            drpArancel.DataValueField = "CstGrpCode"
            drpArancel.DataSource = dt
            drpArancel.DataBind()
        Catch ex As Exception

            Response.Write(ex.Message)
        End Try
    End Sub

    Public Function GlobalConnecttoSAP() As Integer
        SCompany = New SAPbobsCOM.Company
        SCompany.Server = "192.168.1.3"
        SCompany.CompanyDB = "MOVESA"
        SCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
        SCompany.DbUserName = "sa"
        SCompany.DbPassword = "M*l!n3r0s2k12"
        SCompany.UserName = "it"
        SCompany.Password = "polar"
        SCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
        SCompany.SLDServer = "192.168.1.9:40000"
        lRetCode = SCompany.Connect
        Return lRetCode
    End Function

    Sub CrearItemSAP()
        Try
            If GlobalConnecttoSAP() <> 0 Then
                Response.Write("<script>alert('Error al Intentar Conectarse a SAP');</script>")
                Exit Sub
            End If

            Dim oItem As SAPbobsCOM.Items = SCompany.GetBusinessObject(BoObjectTypes.oItems)
            oItem.ItemCode = txtItemcode.Text
            oItem.BarCode = txtCodeBars.Text
            oItem.ItemName = txtItenmane.Text
            oItem.SupplierCatalogNo = Left(txtItenmane.Text, 50)
            oItem.ForeignName = txtFrgnName.Text
            oItem.CustomsGroupCode = drpArancel.SelectedValue.ToString
            oItem.Mainsupplier = drpProveedores.SelectedValue.ToString
            oItem.UserFields.Fields.Item("U_AMODELO").Value = drpModelo.SelectedValue.ToString
            oItem.UserFields.Fields.Item("U_SubGrupoRpt").Value = txtSubGrupo.Text
            oItem.UserFields.Fields.Item("U_Compras").Value = txtOEM.Text
            oItem.ItemType = ItemTypeEnum.itItems
            oItem.InventoryItem = BoYesNoEnum.tYES
            oItem.PurchaseItem = BoYesNoEnum.tYES
            oItem.SalesItem = BoYesNoEnum.tYES
            oItem.ItemsGroupCode = drpGrupoArticulos.SelectedValue.ToString
            oItem.ManageStockByWarehouse = BoYesNoEnum.tYES
            oItem.GLMethod = BoGLMethods.glm_WH
            oItem.User_Text = "Articulo creado por: " & Session("Name").ToString & " Fecha y Hora: " & Date.Now

            If checkProperty59.Checked = True Then
                oItem.Properties(59) = SAPbobsCOM.BoYesNoEnum.tYES
            End If

            If checkProperty64.Checked = True Then
                oItem.Properties(64) = SAPbobsCOM.BoYesNoEnum.tYES
            End If

            Dim lRetCode As Integer = oItem.Add()
            If lRetCode <> 0 Then
                SCompany.GetLastError(lErrCode, sErrMsg)

                Dim script As String = "<script>iziToast.error({title: 'Error!', message: 'Error al Intentar Crear Item Nuevo!',timeout: 10000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                Response.Write("CrearItemSAP Error al crear el ítem: " & lErrCode & " - " & sErrMsg)
            Else
                Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Item Nuevo Creado Exitosamente!',timeout: 10000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            End If
            SCompany.Disconnect()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Sub ActualizarItemSAP()
        Try
            If GlobalConnecttoSAP() <> 0 Then
                Response.Write("<script>alert('Error al Intentar Conectarse a SAP');</script>")
                Exit Sub
            End If

            Dim oItem As SAPbobsCOM.Items = SCompany.GetBusinessObject(BoObjectTypes.oItems)
            If oItem.GetByKey(txtItemcode.Text) Then
                oItem.BarCode = txtCodeBars.Text
                oItem.ItemName = txtItenmane.Text
                oItem.SupplierCatalogNo = txtSuppCatNum.Text
                oItem.ForeignName = txtFrgnName.Text
                oItem.CustomsGroupCode = drpArancel.SelectedValue.ToString
                oItem.Mainsupplier = drpProveedores.SelectedValue.ToString
                oItem.UserFields.Fields.Item("U_AMODELO").Value = drpModelo.SelectedValue.ToString
                oItem.UserFields.Fields.Item("U_SubGrupoRpt").Value = txtSubGrupo.Text
                oItem.UserFields.Fields.Item("U_Compras").Value = txtOEM.Text
                oItem.ItemType = ItemTypeEnum.itItems
                oItem.InventoryItem = BoYesNoEnum.tYES
                oItem.PurchaseItem = BoYesNoEnum.tYES
                oItem.SalesItem = BoYesNoEnum.tYES
                oItem.ItemsGroupCode = drpGrupoArticulos.SelectedValue.ToString
                oItem.ManageStockByWarehouse = BoYesNoEnum.tYES
                oItem.GLMethod = BoGLMethods.glm_WH
                oItem.User_Text = "Articulo actualizado por: " & Session("Name").ToString & " Fecha y Hora: " & Date.Now
                oItem.Properties(59) = If(checkProperty59.Checked, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO)
                oItem.Properties(64) = If(checkProperty64.Checked, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO)

                Dim lRetCode As Integer = oItem.Update()
                If lRetCode <> 0 Then
                    Dim lErrCode As Integer
                    Dim sErrMsg As String
                    SCompany.GetLastError(lErrCode, sErrMsg)

                    Dim script As String = "<script>iziToast.error({title: 'Error!', message: 'Error al Intentar Actualizar el Ítem!',timeout: 10000})</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorScript", script, False)
                    Response.Write("ActualizarItemSAP Error al actualizar el ítem: " & lErrCode & " - " & sErrMsg)
                Else
                    Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Ítem Actualizado Exitosamente!',timeout: 10000})</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                End If
            Else
                Response.Write("<script>alert('El ítem no existe.');</script>")
            End If
            SCompany.Disconnect()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub btnCrearItem_Click(sender As Object, e As EventArgs) Handles btnCrearItem.Click
        Try
            txtItemcode.ReadOnly = True
            CrearItemSAP()
            txtItemcode.Text = ""
            txtItemcode.ReadOnly = False
            txtCodeBars.Text = ""
            txtSuppCatNum.Text = ""
            txtItenmane.Text = ""
            txtFrgnName.Text = ""
            txtSubGrupo.Text = ""
            txtOEM.Text = ""
            _cargarGrupoArticulos()
            _cargarProveedores()
            _cargarModelos()
            _cargarAranceles()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub GetItemData(itemCode As String)
        Dim connectionString As String = sCon1
        Dim query As String = "SELECT itemcode, CodeBars, SuppCatNum, ItemName, FrgnName, CardCode, ItmsGrpCod, CstGrpCode, U_SubGrupoRpt, U_AMODELO, U_Compras " &
                          "FROM oitm WITH(NOLOCK) WHERE ItemCode = @itemcode"
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@itemcode", itemCode)
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            txtItemcode.Text = reader("itemcode").ToString()
                            txtItemcode.ReadOnly = True
                            txtCodeBars.Text = reader("CodeBars").ToString()
                            txtSuppCatNum.Text = reader("SuppCatNum").ToString()
                            txtItenmane.Text = reader("ItemName").ToString()
                            txtFrgnName.Text = reader("FrgnName").ToString()
                            txtSubGrupo.Text = reader("U_SubGrupoRpt").ToString()
                            txtOEM.Text = reader("U_Compras").ToString()
                            drpGrupoArticulos.SelectedValue = reader("ItmsGrpCod").ToString()
                            drpProveedores.SelectedValue = reader("CardCode").ToString()
                            drpArancel.SelectedValue = reader("CstGrpCode").ToString()
                            drpModelo.SelectedValue = reader("U_AMODELO").ToString()
                        End While
                    Else
                        Dim script As String = "<script>iziToast.error({title: 'Error!', message: 'No se encontraron registros para el código de artículo proporcionado.!',timeout: 10000})</script>"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                    End If
                End Using
                conn.Close()
            End Using
        End Using
    End Sub

    Private Sub btnLimpiarFormulario_Click(sender As Object, e As EventArgs) Handles btnLimpiarFormulario.Click
        Try
            Response.Redirect("SapNewItems.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub btnVerificarExiste_Click(sender As Object, e As EventArgs) Handles btnVerificarExiste.Click
        Try
            GetItemData(txtItemcode.Text)
            Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Codigo Si Existe!',timeout: 10000})</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Try
            ActualizarItemSAP()
            txtItemcode.Text = ""
            txtItemcode.ReadOnly = False
            txtCodeBars.Text = ""
            txtSuppCatNum.Text = ""
            txtItenmane.Text = ""
            txtFrgnName.Text = ""
            txtSubGrupo.Text = ""
            txtOEM.Text = ""
            _cargarGrupoArticulos()
            _cargarProveedores()
            _cargarModelos()
            _cargarAranceles()

        Catch ex As Exception
            Dim script As String = "<script>iziToast.error({title: 'Error!', message: 'Ocurrio un Error, revisar consola!',timeout: 10000})</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            Response.Write("<script>console.log('" & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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

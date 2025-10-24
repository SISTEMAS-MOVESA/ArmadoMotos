Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports SAPbobsCOM
Partial Class AuditoriaCobroRepuestos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    <WebMethod()>
    Public Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        prefixText = prefixText.Replace(" ", "%")
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1
            Using cmd As SqlCommand = New SqlCommand()

                cmd.CommandText = "select top 10 CardName from movesa..ocrd with(nolock) where cardtype='C' AND CardName LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("CardName").ToString())
                    End While
                End Using
                conn.Close()
                Return customers
            End Using
        End Using
    End Function

    <WebMethod()>
    Public Shared Function SearchItems(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        prefixText = prefixText.Replace(" ", "%")
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1
            Using cmd As SqlCommand = New SqlCommand()

                cmd.CommandText = "select top 10 Itemcode + ', ' + ItemName + ', Stock DCR00: ' + convert(nvarchar,(select convert(int,onhand) from oitw with(nolock) where itemcode=OITM.itemcode and whscode='DCR00')) [ItemName] from movesa..OITM with(nolock) where ItmsGrpCod<>54 AND ItemName LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()

                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("ItemName").ToString())
                    End While
                End Using

                conn.Close()
                Return customers
            End Using
        End Using
    End Function
    Private Sub AuditoriaCobroRepuestos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGrid(txtcardcode.Text, Session("User").ToString.Trim)
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub btnBuscarCliente_Click(sender As Object, e As EventArgs) Handles btnBuscarCliente.Click
        Try
            buscarCLiente(txtNombreCliente.Text)
            BindGrid(txtcardcode.Text, Session("User").ToString.Trim)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub buscarCLiente(_cardname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = " select CardCode,CardName, VatIdUnCmp from movesa..ocrd with(nolock) where CardName = @p1"
            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@p1", _cardname)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then
                txtcardcode.Text = drd.Item("CardCode").ToString
                txtCardName.Text = drd.Item("CardName").ToString
                txtRTN.Text = drd.Item("VatIdUnCmp").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("buscarCLiente: " & ex.Message)
        End Try
    End Sub
    Public Sub buscarItem(_itemcode As String, _cardcode As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select t0.ItemCode,t0.ItemName " &
            " ,(select price from itm1 with(nolock) where ItemCode=t0.ItemCode  " &
            " and PriceList=(select ListNum from ocrd with(nolock) where cardcode = @p2))[Precio]  " &
            " ,(select taxcode from CRD1 with(nolock) where cardcode = @p2) [Impuesto] " &
            " from movesa..oitm t0  where ItemCode=@p1"
            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@p1", _itemcode)
            cmd.Parameters.AddWithValue("@p2", _cardcode)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then
                txtItemcode.Text = drd.Item("ItemCode").ToString
                txtItemname.Text = drd.Item("ItemName").ToString
                txtPrecio.Text = drd.Item("Precio").ToString
                txtImpuesto.Text = drd.Item("Impuesto").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("buscarItem Error: " & ex.Message)
        End Try
    End Sub

    Public Sub InsertAuditoriaCobros(ByVal fecha As DateTime, ByVal cardcode As String, ByVal cardname As String, ByVal rtn As String,
                                 ByVal itemcode As String, ByVal itemname As String, ByVal price As Decimal, ByVal cantidad As Integer,
                                 ByVal impuesto As String, ByVal totalLinea As Decimal, ByVal usuario As String, ByVal estatus As String)
        Try
            Dim sCon As String = sCon2
            Using con As New SqlConnection(sCon)
                Dim query As String = "INSERT INTO [dbo].[AUDITORIA_COBROS] (FECHA, CARDCODE, CARDNAME, RTN, " &
                                  " ITEMCODE, ITENMANE, PRICE, CANTIDAD, IMPUESTO, TOTALLINEA, USUARIO, ESTATUS) " &
                                  " VALUES (@FECHA, @CARDCODE, @CARDNAME, @RTN, @ITEMCODE, @ITENMANE, @PRICE, @CANTIDAD, " &
                                  " @IMPUESTO, @TOTALLINEA, @USUARIO, @ESTATUS)"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@FECHA", fecha)
                    cmd.Parameters.AddWithValue("@CARDCODE", cardcode)
                    cmd.Parameters.AddWithValue("@CARDNAME", cardname)
                    cmd.Parameters.AddWithValue("@RTN", rtn)
                    cmd.Parameters.AddWithValue("@ITEMCODE", itemcode)
                    cmd.Parameters.AddWithValue("@ITENMANE", itemname)
                    cmd.Parameters.AddWithValue("@PRICE", price)
                    cmd.Parameters.AddWithValue("@CANTIDAD", cantidad)
                    cmd.Parameters.AddWithValue("@IMPUESTO", impuesto)
                    cmd.Parameters.AddWithValue("@TOTALLINEA", totalLinea)
                    cmd.Parameters.AddWithValue("@USUARIO", usuario)
                    cmd.Parameters.AddWithValue("@ESTATUS", estatus)
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("InsertAuditoriaCobros Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAgregarItem_Click(sender As Object, e As EventArgs) Handles btnAgregarItem.Click
        Try
            InsertAuditoriaCobros(Date.Now, txtcardcode.Text, txtCardName.Text, txtRTN.Text, txtItemcode.Text, txtItemname.Text, txtPrecio.Text,
                                    txtCantidad.Text, txtImpuesto.Text, txtCantidad.Text * txtPrecio.Text, Session("User").ToString.Trim, "ACTIVO")
        Catch ex As Exception
            Response.Write("btnAgregarItem_Click Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnBuscarInfoItem_Click(sender As Object, e As EventArgs) Handles btnBuscarInfoItem.Click
        Try
            If txtcardcode.Text = "" Then
                Dim script As String = "iziToast.warning({title: 'Campo vacío', message: 'Seleccione un Cliente Antes de Continuar!!', position: 'topRight'});"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showWarning", script, True)
                Exit Sub
            End If
            Dim descripcion As String = txtDescripcionRepuesto.Text.Split(","c)(0).Trim()
            buscarItem(descripcion, txtcardcode.Text)
        Catch ex As Exception
            Response.Write("btnBuscarInfoItem_Click Error: " & ex.Message)
        End Try
    End Sub
    Private Sub BindGrid(_cardcode As String, _usuario As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[ITEMCODE],[ITENMANE],[PRICE],[CANTIDAD], " &
                                            " [IMPUESTO],[TOTALLINEA] , CASE IMPUESTO WHEN 'ISV' THEN TOTALLINEA*1.15 ELSE TOTALLINEA END [TOTAL] " &
                                            " FROM [ArmadoMotos].[dbo].[AUDITORIA_COBROS]  " &
                                            " with(nolock) where [ESTATUS] = 'ACTIVO'  " &
                                            " and [CARDCODE] = @p1 and [USUARIO] = @p2")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@p1", _cardcode)
                        cmd.Parameters.AddWithValue("@p2", _usuario)
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridOrdenTemporal.DataSource = dt
                            gridOrdenTemporal.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridOrdenTemporal.UseAccessibleHeader = True
            gridOrdenTemporal.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Dim script As String = "console.log('Grid Sin Datos " & ex.Message & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showWarning", script, True)
        End Try
    End Sub
    Protected Sub gridOrdenTemporal_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gridOrdenTemporal.RowDataBound
        Static totalGeneral As Decimal = 0

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim totalValue As Decimal = 0
            If Decimal.TryParse(e.Row.Cells(7).Text, totalValue) Then
                totalGeneral += totalValue
            End If
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "Total General:"
            e.Row.Cells(7).Text = totalGeneral.ToString("C")
            e.Row.Cells(6).Font.Size = FontUnit.XLarge
            e.Row.Cells(7).Font.Size = FontUnit.XLarge
            e.Row.Cells(7).Font.Bold = True
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Center

        End If
    End Sub
    Public Sub CrearoOrdersServicio()
        Dim company As Company = New Company()
        Try
            company = New SAPbobsCOM.Company
            company.Server = "192.168.1.3"
            company.CompanyDB = "MOVESA"
            company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
            company.DbUserName = "sa"
            company.DbPassword = "M*l!n3r0s2k12"
            company.UserName = "itpro"
            company.Password = "eeal96"
            company.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
            company.SLDServer = "192.168.1.9:40000"

            If company.Connect() <> 0 Then
                Dim script As String = String.Format("console.log('Error al Conectarse a SAP: {0}');", company.GetLastErrorDescription().Replace("'", "\'"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showWarning", script, True)

                Dim strToast As String = String.Format("iziToast.error({{title: 'Error', message: '{0}', position: 'topRight'}});", "Ourrio un Error " & company.GetLastErrorDescription().Replace("'", "\'"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showError", strToast, True)

            End If

            Dim oOrders As Documents = company.GetBusinessObject(BoObjectTypes.oOrders)
            oOrders.Series = 205
            oOrders.CardCode = txtcardcode.Text
            oOrders.CardName = txtCardName.Text
            oOrders.DocDate = DateTime.Now
            oOrders.DocDueDate = DateTime.Now.AddDays(7)
            oOrders.TaxDate = DateTime.Now
            oOrders.DocType = BoDocumentTypes.dDocument_Service
            oOrders.Comments = txtComments.Text

            For Each row As GridViewRow In gridOrdenTemporal.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    oOrders.Lines.ItemDescription = row.Cells(1).Text & " - " & row.Cells(2).Text
                    oOrders.Lines.AccountCode = "_SYS00000005196"

                    Dim cantidad As Integer
                    Dim totalLinea As Decimal
                    Integer.TryParse(row.Cells(4).Text, cantidad)
                    Decimal.TryParse(row.Cells(6).Text, totalLinea)

                    oOrders.Lines.Quantity = cantidad
                    oOrders.Lines.UnitPrice = totalLinea
                    oOrders.Lines.LineTotal = totalLinea
                    oOrders.Lines.UserFields.Fields.Item("U_MSERIE").Value = txtSerieMoto.Text
                    oOrders.Lines.Add()
                End If
            Next

            If oOrders.Add() <> 0 Then
                Dim script As String = String.Format("console.log('Ocurrio un Error al Enviar Documento a SAP: {0}');", company.GetLastErrorDescription().Replace("'", "\'"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showWarning", script, True)

                Dim strToast As String = String.Format("iziToast.error({{title: 'Error', message: '{0}', position: 'topRight'}});", "Ourrio un Error " & company.GetLastErrorDescription().Replace("'", "\'"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showError", strToast, True)

            Else
                Dim docEntry As String = company.GetNewObjectKey()
                UpdateDocentry(txtcardcode.Text, docEntry, Date.Now, "CREADO")

                BindGrid(txtcardcode.Text, Session("User").ToString.Trim)

                Dim script As String = String.Format("console.log('Documento Creado Exitosamente Docentry: {0}');", docEntry.Replace("'", "\'"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showWarning", script, True)

                Dim strToast As String = String.Format("iziToast.success({{title: 'OK', message: '{0}', position: 'topRight'}});", "Documento Creado Exitosamente " & docEntry.Replace("'", "\'"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showError", strToast, True)

            End If

        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        Finally
            If company.Connected Then
                company.Disconnect()
            End If
        End Try
    End Sub

    Private Sub btnEnviarSAP_Click(sender As Object, e As EventArgs) Handles btnEnviarSAP.Click
        Try
            If txtComments.Text = "" Then
                Dim strToast As String = String.Format("iziToast.warning({{title: 'Error', message: '{0}', position: 'topRight'}});", "Faltan Los Comentarios")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showError", strToast, True)
                Exit Sub
            End If

            If txtSerieMoto.Text = "" Then
                Dim strToast As String = String.Format("iziToast.warning({{title: 'Error', message: '{0}', position: 'topRight'}});", "Falta El Nmero de Serie de la Moto")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showError", strToast, True)
                Exit Sub
            End If

            CrearoOrdersServicio()
        Catch ex As Exception
            Dim script As String = "console.log('Grid Sin Datos " & ex.Message & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showWarning", script, True)
        End Try
    End Sub

    Private Sub gridOrdenTemporal_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridOrdenTemporal.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridOrdenTemporal.Rows(index)
                EliminarLinea(gridOrdenTemporal.Rows(index).Cells(0).Text)
                BindGrid(txtcardcode.Text, Session("User").ToString.Trim)
            End If
        Catch ex As Exception
            Response.Write("gridOrdenTemporal_RowCommand Error: " & ex.Message)
        End Try
    End Sub
    Public Sub EliminarLinea(idlinea As Integer)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "DELETE FROM [ArmadoMotos].[dbo].[AUDITORIA_COBROS] where [ID] = @p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                Dim strToast As String = String.Format("iziToast.warning({{title: 'Warning', message: '{0}', position: 'topRight'}});", "Linea Eliminada con Exito")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showError", strToast, True)
            End Using
        Catch ex As Exception
            Response.Write("EliminarLinea Error: " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateDocentry(cardcode As String, docentry As String, docdate As DateTime, estado As String)
        Try
            Dim query As String = String.Empty
            query &= "update AUDITORIA_COBROS set DOCENTRYSAP=@p1, ENVIOSAP=@p2, ESTATUS=@p3 where CARDCODE=@p4 and ESTATUS='ACTIVO'"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", docentry)
                        .Parameters.AddWithValue("@p2", docdate)
                        .Parameters.AddWithValue("@p3", estado)
                        .Parameters.AddWithValue("@p4", cardcode)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("UpdateDocentry Cobro:  " & ex.Message)
        End Try
    End Sub
End Class

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System.Configuration
Imports System.Web.Services
Imports SAPbobsCOM
Partial Class ExpedienteVehiculoGarantia
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public SCompany As SAPbobsCOM.Company
    Public oSBObob As SAPbobsCOM.SBObob
    Public oRecordSet As SAPbobsCOM.Recordset
    Public xCompany As SAPbobsCOM.Company
    Public lErrCode As Integer
    Public lRetCode As Integer
    Public sErrMsg As String
    Public oGoodsRcpt As SAPbobsCOM.Documents


    <WebMethod()>
    Public Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1
            Using cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = " SELECT top 10 T1.MnfSerial FROM OSRN T1 with(nolock) INNER JOIN OITM T0 WITH(NOLOCK) ON T1.ITEMCODE=T0.ITEMCODE WHERE T0.ItmsGrpCod='154' AND T1.[MnfSerial] LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("MnfSerial").ToString())
                    End While
                End Using
                conn.Close()
                Return customers
            End Using
        End Using
    End Function

    <WebMethod()>
    Public Shared Function SearchByItemCode(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1
            Using cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = " SELECT top 10 itemcode from oitm with(nolock) where itemcode LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("itemcode").ToString())
                    End While
                End Using
                conn.Close()
                Return customers
            End Using
        End Using
    End Function

    <WebMethod()>
    Public Shared Function SearchByItemName(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1
            Using cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = " SELECT top 20 itemname from oitm with(nolock) where itemname LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("itemname").ToString())
                    End While
                End Using
                conn.Close()
                Return customers
            End Using
        End Using
    End Function

    Public Sub BuscarSerie(_SERIE As String)
        Try
            If _SERIE = "NULL" Then
                Exit Sub
            Else
                Dim dt As New DataTable
                Dim _con As New SqlConnection(sCon1)
                Dim Consulta As String = " Select T0.[ItemCode],T0.[ItemName],T1.[SuppSerial],T1.[IntrSerial],t1.BatchId, " &
                                        " T5.Name [Marca],T4.NAME [Cilindros], T3.Name [Motor],T2.Name [Modelo],T1.[WhsCode],T6.Name [Color], " &
                                        " case t1.Status when 1 THEN 'No Disponible' WHEN 0 THEN 'Disponible' END [Estado],(Select Owhs.[Whsname] FROM Owhs With(nolock) WHERE Owhs.[whscode]=T1.[WhsCode] ) [Almacen],t1.U_Estado_Produccion " &
                                        " FROM OITM T0 With(nolock) INNER JOIN OSRI T1 With(nolock) On T0.ItemCode = T1.ItemCode " &
                                        " FULL JOIN [@AMODELO] T2 With(NOLOCK) On T0.U_AMODELO=T2.CODE  " &
                                        " FULL JOIN [@AMOTOR] T3 With(NOLOCK) On T0.U_AMOTOR=T3.Code " &
                                        " FULL JOIN [@ACILINDROS] T4 With(NOLOCK) On T0.U_ACILINDROS=T4.Code " &
                                        " FULL JOIN [@AMARCA] T5 With(NOLOCK) On T0.U_AMARCA=T5.CODE " &
                                        " FULL JOIN [@SCOLOR] T6 With(NOLOCK) On T0.U_ACOLOR=T6.CODE " &
                                        " WHERE ISNULL(u_vin,'-')<>'N' AND T1.[SuppSerial]='" & _SERIE & "' "
                'Response.Write("<script>console.log('" & Replace(Consulta, "'", "") & "')</script>")

                Dim Comando As New SqlCommand(Consulta, _con)
                Dim drd As SqlDataReader
                _con.Open()
                drd = Comando.ExecuteReader()
                If drd.Read() Then
                    lblItemcode.Text = drd.Item("ItemCode").ToString
                    lblItemname.Text = drd.Item("itemname").ToString
                    lblModelo.Text = drd.Item("Modelo").ToString
                    lblYear.Text = drd.Item("BatchId").ToString
                    lblColor.Text = drd.Item("Color").ToString
                    lblMarca.Text = drd.Item("Marca").ToString
                    lblCilindros.Text = drd.Item("Cilindros").ToString
                    lblMotorM.Text = drd.Item("Motor").ToString
                    txtEstadpSN.Text = drd.Item("Estado").ToString
                    BindGrid(txtSnMoto.Text)
                    BindGridRepuestosResueltos(txtSnMoto.Text)
                    BindGridKardex(txtSnMoto.Text)
                    If drd.Item("U_Estado_Produccion").ToString = "05" Then
                        btnDesHailitar.Visible = False
                    End If
                Else
                    lblItemcode.Text = ""
                    lblItemname.Text = ""
                    lblModelo.Text = ""
                    lblYear.Text = ""
                    lblColor.Text = ""
                    lblMarca.Text = ""
                    lblCilindros.Text = ""
                    lblMotorM.Text = ""
                End If
                _con.Close()
            End If
        Catch ex As Exception
            Response.Write("BuscarSerie " & ex.Message)
        End Try
    End Sub

    Public Sub BuscarSerieExpediente(_SERIE As String)
        Try
            If _SERIE = "NULL" Then
                Exit Sub
            Else
                Dim dt As New DataTable
                Dim _con As New SqlConnection(sCon1)
                Dim Consulta As String = " select count(1)[Existe] from armadomotos..kardexvehiculo with(nolock) where serie='" & _SERIE & "' "
                Dim Comando As New SqlCommand(Consulta, _con)
                Dim drd As SqlDataReader
                _con.Open()
                drd = Comando.ExecuteReader()
                If drd.Read() Then

                    If drd.Item("Existe").ToString = "0" Then
                        txtExpediente.Text = "Sin Expediente"
                        btnHabilitar.Visible = False
                        btnExtraerRepuestos.Visible = False
                        btnArmarRepuestos.Visible = False
                    Else
                        txtExpediente.Text = "Con Expediente"
                    End If
                End If
                _con.Close()
            End If
        Catch ex As Exception
            Response.Write("BuscarSerieExpediente: " & ex.Message)
        End Try
    End Sub
    Public Sub BuscarInfoRepuesto(_STRINGBUSQUEDA As String, _TIPOBUSQUEDA As String)
        Try
            If _TIPOBUSQUEDA = "ITEMCODE" Then
                Dim dt As New DataTable
                Dim _con As New SqlConnection(sCon1)
                Dim Consulta As String = "SELECT ITEMNAME FROM OITM WITH(NOLOCK) WHERE ITEMCODE='" & _STRINGBUSQUEDA & "'"
                Dim Comando As New SqlCommand(Consulta, _con)
                Dim drd As SqlDataReader
                _con.Open()
                drd = Comando.ExecuteReader()
                If drd.Read() Then
                    txtItemName.Text = drd.Item("ITEMNAME").ToString
                    txtQtyRep.Text = "1"
                End If
                _con.Close()
            End If

            If _TIPOBUSQUEDA = "ITEMNAME" Then
                Dim dt As New DataTable
                Dim _con As New SqlConnection(sCon1)
                Dim Consulta As String = "SELECT ITEMCODE FROM OITM WITH(NOLOCK) WHERE ITEMNAME='" & _STRINGBUSQUEDA & "'"
                Dim Comando As New SqlCommand(Consulta, _con)
                Dim drd As SqlDataReader
                _con.Open()
                drd = Comando.ExecuteReader()
                If drd.Read() Then
                    txtItemcode.Text = drd.Item("ITEMCODE").ToString
                    txtQtyRep.Text = "1"
                End If
                _con.Close()
            End If
        Catch ex As Exception
            Response.Write("BuscarInfoRepuesto " & ex.Message)
        End Try
    End Sub
    Private Sub BindGrid(_SERIE As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("Select t0.[DATECREATED],t0.[ITEMCODE],t0.[ITEMNAME],t0.[QTY],t0.[SERIEORIGEN],t0.[ESTATUS],t0.[COMENTARIOS]," &
                                               " (select convert(int,onhand) from movesa..oitw with(nolock) where ItemCode  COLLATE Modern_Spanish_CI_AS =t0.ITEMCODE And WhsCode='DCR00')[DCR]" &
                                               " From [ArmadoMotos].[dbo].[RETIROREPUESTOS] t0 Where [ESTATUS] ='1' and [SERIEORIGEN]='" & _SERIE.ToString.Trim & "' ")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                        End Using
                        con.Close()
                    End Using
                End Using
            End Using
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            ' Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindGridRepuestosResueltos(_SERIE As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [DATEUPDATED],[ITEMCODE],[ITEMNAME],[QTY],[SERIEORIGEN],[ESTATUS],[COMENTARIOSRETIRO] " &
                                            " FROM [ArmadoMotos].[dbo].[RETIROREPUESTOS] WHERE [ESTATUS]='0' and [SERIEORIGEN]='" & _SERIE.ToString.Trim & "' ")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView3.DataSource = dt
                            GridView3.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GridView3.UseAccessibleHeader = True
            GridView3.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception

        End Try
    End Sub
    Public Sub RetiroDePiezas(_datecreated As Date, _itemcode As String, _itemname As String, _qty As String, _serieorigen As String, _idusuario As String, _estatus As String, _comentarios As String, _seriedestino As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[RETIROREPUESTOS]([DATECREATED],[ITEMCODE],[ITEMNAME],[QTY],[SERIEORIGEN],[IDUSUARIO],[ESTATUS],[COMENTARIOS],[SERIEDESTINO]) " &
                    " VALUES(@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _datecreated)
                cmd.Parameters.AddWithValue("@p2", _itemcode)
                cmd.Parameters.AddWithValue("@p3", _itemname)
                cmd.Parameters.AddWithValue("@p4", _qty)
                cmd.Parameters.AddWithValue("@p5", _serieorigen)
                cmd.Parameters.AddWithValue("@p6", _idusuario)
                cmd.Parameters.AddWithValue("@p7", _estatus)
                cmd.Parameters.AddWithValue("@p8", _comentarios)
                cmd.Parameters.AddWithValue("@p9", _seriedestino)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("RetiroDePiezas " & ex.Message)
        End Try
    End Sub
    Protected Sub btnBuscarMoto_Click(sender As Object, e As EventArgs) Handles btnBuscarMoto.Click
        Session("SerieExpediente") = txtSnMoto.Text
        BuscarSerie(txtSnMoto.Text)
        BuscarSerieExpediente(txtSnMoto.Text)
    End Sub
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Session("SerieExpediente") = "NULL"
        Response.Redirect("ExpedienteVehiculoGarantia.aspx")
    End Sub
    Protected Sub btnBuscarItemcode_Click(sender As Object, e As EventArgs) Handles btnBuscarItemcode.Click
        BuscarInfoRepuesto(txtItemcode.Text, "ITEMCODE")
    End Sub
    Protected Sub btnBuscarItemname_Click(sender As Object, e As EventArgs) Handles btnBuscarItemname.Click
        BuscarInfoRepuesto(txtItemName.Text, "ITEMNAME")
    End Sub
    Protected Sub btnAgregarRepuesto_Click(sender As Object, e As EventArgs) Handles btnAgregarRepuesto.Click
        If txtItemcode.Text = "" Then
            Response.Write("<script language=javascript>alert('FALTA CODIGO DE ARTICULO');</script>")
            Exit Sub
        End If

        If txtItemName.Text = "" Then
            Response.Write("<script language=javascript>alert('FALTA DESCRIPCION ARTICULO');</script>")
            Exit Sub
        End If

        If txtQtyRep.Text = "" Then
            Response.Write("<script language=javascript>alert('FALTA CANTIDAD REPEUSTO SOLICITADO');</script>")
            Exit Sub
        End If

        If txtSerieReemplazo.Text = "" Then
            Response.Write("<script language=javascript>alert('FALTA SERIE MOTO DE DESTINO O ESCRIBA RETIRO SIN SERIE');</script>")
            Exit Sub
        End If
        If txtComentarios.Text = "" Then
            Response.Write("<script language=javascript>alert('FALTA COMENTARIO DEL RETIRO');</script>")
            Exit Sub
        Else
            RetiroDePiezas(Date.Now, txtItemcode.Text, txtItemName.Text, txtQtyRep.Text, txtSnMoto.Text, Session("UserCode").ToString, "1", txtComentarios.Text, txtSerieReemplazo.Text)
        End If
        Response.Redirect("ExpedienteVehiculoGarantia.aspx")
    End Sub
    Private Sub ExpedienteVehiculoGarantia_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            If Session("Position").ToString = "Garantias" Then
                pnlBotonesDeshabilitar.Visible = True
            End If
            If Session("Position").ToString = "Administrador" Then
                pnlBotonesDeshabilitar.Visible = True
            End If
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    If Session("SerieExpediente").ToString <> "NULL" Then
                        BuscarSerie(Session("SerieExpediente").ToString)
                        txtSnMoto.Text = Session("SerieExpediente").ToString
                        BuscarSerieExpediente(txtSnMoto.Text)
                        BindGrid(txtSnMoto.Text)
                        BindGridKardex(txtSnMoto.Text)
                        BindGridRepuestosResueltos(txtSnMoto.Text)
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub btnExtraerRepuestos_Click(sender As Object, e As EventArgs) Handles btnExtraerRepuestos.Click
        txtSerieReemplazo.Visible = True
        pnlRetirodePiezas.Visible = True
        pnlBotonesDeshabilitar.Visible = False
        BuscarSerieExpediente(txtSnMoto.Text)
        BindGrid(txtSnMoto.Text)
        BindGridKardex(txtSnMoto.Text)
        BindGridRepuestosResueltos(txtSnMoto.Text)
    End Sub
    Public Sub RearmarRepuestos(_serie As String)
        Try
            Dim constr As String = sCon2
            Using con As SqlConnection = New SqlConnection(constr)
                Using cmd As SqlCommand = New SqlCommand("SELECT [ID],[ITEMNAME] FROM [ArmadoMotos].[dbo].[RETIROREPUESTOS] WITH(NOLOCK) WHERE ESTATUS='1' and SERIEORIGEN='" & _serie & "'")
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    con.Open()
                    lstArmarRepuestos.DataSource = cmd.ExecuteReader()
                    lstArmarRepuestos.DataTextField = "ITEMNAME"
                    lstArmarRepuestos.DataValueField = "ID"
                    lstArmarRepuestos.DataBind()
                    con.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("RearmarRepuestos " & ex.Message)
        End Try
    End Sub
    Protected Sub btnArmarRepuestos_Click(sender As Object, e As EventArgs) Handles btnArmarRepuestos.Click
        RearmarRepuestos(txtSnMoto.Text)
        pnlArmadoPiezas.Visible = True
        pnlBotonesDeshabilitar.Visible = False
        pnlGridRepuestos.Visible = False
    End Sub
    Protected Sub btnUpdateArmarRepuesto_Click(sender As Object, e As EventArgs) Handles btnUpdateArmarRepuesto.Click
        Dim list As New List(Of String)()
        For Each item As ListItem In lstArmarRepuestos.Items
            If item.Selected = True Then
                RearmadoPiezas(txtCommentsRearmado.Text, Session("UserCode").ToString, item.Value, txtSnMoto.Text)
            End If
        Next
        Response.Redirect("ExpedienteVehiculoGarantia.aspx")
    End Sub

    Public Sub RearmadoPiezas(_comentarioretiro As String, _usuarioretiro As String, _idrepuesto As String, _serieorigen As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = " UPDATE [dbo].[RETIROREPUESTOS] " &
                  " SET [ESTATUS] = @p1 " &
                  " ,[DATEUPDATED] = @p2 " &
                  " ,[COMENTARIOSRETIRO] = @p3 " &
                  " ,[USUARIORETIRO] = @p4 " &
                  " WHERE [ID]=@p5 and SERIEORIGEN = @p6"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", 0)
                cmd.Parameters.AddWithValue("@p2", Date.Now)
                cmd.Parameters.AddWithValue("@p3", _comentarioretiro)
                cmd.Parameters.AddWithValue("@p4", _usuarioretiro)
                cmd.Parameters.AddWithValue("@p5", _idrepuesto)
                cmd.Parameters.AddWithValue("@p6", _serieorigen)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("RearmadoPiezas " & ex.Message)
        End Try
    End Sub
    Protected Sub btnDesHailitar_Click(sender As Object, e As EventArgs) Handles btnDesHailitar.Click
        Try
            Session("AccionHabilitarDeshabilitar") = "Deshabilitar"
            txtComentariosAccion.Attributes.Add("placeholder", "Comentarios Sobre La Accion / " & Session("AccionHabilitarDeshabilitar"))
            btnConfirmarAccion.Attributes.Add("class", "form-control btn btn-danger rounded submit px-3")
            pnlHabilitarDeshabilitar.Visible = True
            pnlArmadoPiezas.Visible = False
            pnlBotonesDeshabilitar.Visible = False
            pnlGridRepuestos.Visible = False
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnHabilitar_Click(sender As Object, e As EventArgs) Handles btnHabilitar.Click
        Try
            Session("AccionHabilitarDeshabilitar") = "Habilitar"
            txtComentariosAccion.Attributes.Add("placeholder", "Comentarios Sobre La Accion / " & Session("AccionHabilitarDeshabilitar"))
            pnlHabilitarDeshabilitar.Visible = True
            pnlArmadoPiezas.Visible = False
            pnlBotonesDeshabilitar.Visible = False
            pnlGridRepuestos.Visible = False
        Catch ex As Exception

        End Try
    End Sub

    Public Sub KardexVehiculo(DATELOG As Date, SERIE As String, MARCA As String, MODELO As String, COLOR As String, ACCION As String, COMENTARIOS As String, USUARIO As String, MECANICO As String, CALIDAD As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[KARDEXVEHICULO] ([DATELOG],[SERIE],[MARCA],[MODELO],[COLOR],[ACCION],[COMENTARIOS],[USUARIO],[MECANICO],[CALIDAD]) " &
                    " VALUES(@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", DATELOG)
                cmd.Parameters.AddWithValue("@p2", SERIE)
                cmd.Parameters.AddWithValue("@p3", MARCA)
                cmd.Parameters.AddWithValue("@p4", MODELO)
                cmd.Parameters.AddWithValue("@p5", COLOR)
                cmd.Parameters.AddWithValue("@p6", ACCION)
                cmd.Parameters.AddWithValue("@p7", COMENTARIOS)
                cmd.Parameters.AddWithValue("@p8", USUARIO)
                cmd.Parameters.AddWithValue("@p9", MECANICO)
                cmd.Parameters.AddWithValue("@p10", CALIDAD)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("RetiroDePiezas " & ex.Message)
        End Try
    End Sub

    Protected Sub btnConfirmarAccion_Click(sender As Object, e As EventArgs) Handles btnConfirmarAccion.Click
        If Session("AccionHabilitarDeshabilitar") = "Habilitar" Then
            KardexVehiculo(Date.Now, txtSnMoto.Text, lblMarca.Text, lblModelo.Text, lblColor.Text, Session("AccionHabilitarDeshabilitar"), txtComentariosAccion.Text, Session("UserCode").ToString, 0, 0)
            UpdateOsrnSAP(txtSnMoto.Text, "04")
        End If
        If Session("AccionHabilitarDeshabilitar") = "Deshabilitar" Then
            KardexVehiculo(Date.Now, txtSnMoto.Text, lblMarca.Text, lblModelo.Text, lblColor.Text, Session("AccionHabilitarDeshabilitar"), txtComentariosAccion.Text, Session("UserCode").ToString, 0, 0)
            UpdateOsrnSAP(txtSnMoto.Text, "05")
        End If
        Response.Redirect("ExpedienteVehiculoGarantia.aspx")
    End Sub
    Public Sub UpdateOsrnSAP(_SERIE As String, _estado As String)
        Try
            Dim query As String = String.Empty
            query &= "UPDATE OSRN SET U_Estado_Produccion=@p2 WHERE MnfSerial=@p1"
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _SERIE)
                        .Parameters.AddWithValue("@p2", _estado)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("UpdateOsrnSAP " & ex.Message)
        End Try
    End Sub
    Private Sub BindGridKardex(_SERIE As String)
        Try
            pnlKardex.Visible = True
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("	SELECT	T0.[MnfSerial],t1.Quantity, " &
                                                " case t2.DocType   " &
                                                " when 20 then 'Ingreso Mercaderia'   " &
                                                " when 67 then 'Transferencia Inventario'   " &
                                                " when 15 then 'Entrega de Ventas'   " &
                                                " when 13 then 'Factura Cliente'   " &
                                                " else convert(nvarchar,t2.doctype) end [Documento]  " &
                                                ", t2.Docnum, Format(t2.DocDate, 'dd/MM/yyyy') [DocDate],t2.LocCode,(select whsname from owhs with(nolock) where whscode=t2.LocCode)[Almacen]   " &
                                                " FROM	OSRN T0 With(nolock) " &
                                                " inner join    " &
                                                " ITL1 T1 with(nolock) " &
                                                " on T1.SysNumber = T0.SysNumber and T1.ItemCode = T0.ItemCode    " &
                                                " inner join    " &
                                                " OITL T2 with(nolock) " &
                                                " on T1.LogEntry = T2.LogEntry " &
                                                " where	t0.MnfSerial='" & _SERIE & "'  AND t2.DocType=20 " &
                                                " UNION ALL " &
                                                " SELECT  " &
                                                " [SERIE] COLLATE Modern_Spanish_CI_AS " &
                                                ", 0 " &
                                                " ,[ACCION] COLLATE Modern_Spanish_CI_AS " &
                                                ", [ID] " &
                                                " ,[DATELOG] " &
                                                " ,'DCM00' " &
                                                " ,case ACCION WHEN 'Deshabilitar' THEN [COMENTARIOS] COLLATE Modern_Spanish_CI_AS ELSE  'Distribucion Central de Motos'  END " &
                                                " FROM [ArmadoMotos].[dbo].[KARDEXVEHICULO] where serie='" & _SERIE & "' " &
                                                " UNION ALL " &
                                                " SELECT	T0.[MnfSerial],t1.Quantity, " &
                                                " case t2.DocType   " &
                                                " when 20 then 'Ingreso Mercaderia'   " &
                                                " when 67 then 'Transferencia Inventario'   " &
                                                " when 15 then 'Entrega de Ventas'   " &
                                                " when 13 then 'Factura Cliente'   " &
                                                " else convert(nvarchar,t2.doctype) end [Documento] " &
                                                " ,t2.Docnum,FORMAT (t2.DocDate, 'dd/MM/yyyy') [DocDate],t2.LocCode,(select whsname from owhs with(nolock) where whscode=t2.LocCode)[Almacen] " &
                                                " FROM	OSRN T0 with(nolock)    " &
                                                " inner join    " &
                                                " ITL1 T1 with(nolock) " &
                                                " on T1.SysNumber = T0.SysNumber And T1.ItemCode = T0.ItemCode    " &
                                                " inner join    " &
                                                " OITL T2 with(nolock) " &
                                                " on T1.LogEntry = T2.LogEntry    " &
                                                " where	t0.MnfSerial='" & _SERIE & "'  And t2.DocType<>20")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView2.DataSource = dt
                            GridView2.DataBind()
                        End Using
                        con.Close()
                    End Using
                End Using
            End Using
            GridView2.UseAccessibleHeader = True
            GridView2.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnPedirSAP_Click(sender As Object, e As EventArgs) Handles btnPedirSAP.Click

    End Sub
    Protected Sub GenerateSAPPO(_liquidacionid As String)
        Try
            '    If GlobalConnecttoSAP() <> 0 Then
            '        Response.Write("<script>alert('Error al Intentar Conectarse a SAP');</script>")
            '        Exit Sub
            '    End If

            '    Dim purchaseOrder As SAPbobsCOM.Documents = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders)
            '    purchaseOrder.CardCode = txtBPCode.Text
            '    purchaseOrder.Series = 220
            '    purchaseOrder.DocDate = Today
            '    purchaseOrder.DocDueDate = Today
            '    purchaseOrder.TaxDate = Today
            '    purchaseOrder.DocType = BoDocumentTypes.dDocument_Service
            '    purchaseOrder.Reference2 = txtNumeroLiquidacion.Text
            '    purchaseOrder.Comments = "Liquidacio #" & txtNumeroLiquidacion.Text & " Orden de Compra Creada por: " & Session("Name").ToString
            '    purchaseOrder.NumAtCard = txtRefProveedor.Text

            '    Dim constr As String = sCon1
            '    Using con As New SqlConnection(constr)
            '        Using cmd As New SqlCommand("SELECT * FROM [ArmadoMotos].[dbo].[MOTOSLIQ] with(nolock) where liquidacionid=" & _liquidacionid & " ")
            '            Using sda As New SqlDataAdapter()
            '                cmd.Connection = con
            '                sda.SelectCommand = cmd
            '                Using dt As New DataTable()
            '                    sda.Fill(dt)
            '                    For Each row As DataRow In dt.Rows
            '                        purchaseOrder.Lines.ItemDescription = row.Item("modelo").ToString() & " " & row.Item("color").ToString()
            '                        purchaseOrder.Lines.AccountCode = row.Item("cuentac").ToString()
            '                        purchaseOrder.Lines.UserFields.Fields.Item("U_QTY").Value = 1
            '                        purchaseOrder.Lines.UserFields.Fields.Item("U_MSERIE").Value = row.Item("serie").ToString()
            '                        purchaseOrder.Lines.UserFields.Fields.Item("U_pi_number").Value = row.Item("liquidacionid").ToString()
            '                        purchaseOrder.Lines.LineTotal = row.Item("precioarmado").ToString()
            '                        purchaseOrder.Lines.TaxCode = "EXE"
            '                        purchaseOrder.Lines.Add()
            '                    Next
            '                End Using
            '            End Using
            '        End Using
            '    End Using

            '    lRetCode = purchaseOrder.Add
            '    If lRetCode <> 0 Then
            '        Response.Write(SCompany.GetLastErrorDescription)
            '        SCompany.Disconnect()
            '        Runtime.InteropServices.Marshal.ReleaseComObject(purchaseOrder)
            '        Runtime.InteropServices.Marshal.ReleaseComObject(SCompany)
            '        GC.Collect()
            '        GC.WaitForPendingFinalizers()
            '        purchaseOrder = Nothing
            '        SCompany = Nothing
            '    Else
            '        UpdateLIQUIDACIONES(SCompany.GetNewObjectKey.ToString, Session("UserCode").ToString(), Date.Now, _liquidacionid)
            '        UpdateLMOTOSLIQ(SCompany.GetNewObjectKey.ToString, Session("UserCode").ToString(), Date.Now, _liquidacionid)
            '        SCompany.Disconnect()
            '        Runtime.InteropServices.Marshal.ReleaseComObject(purchaseOrder)
            '        Runtime.InteropServices.Marshal.ReleaseComObject(SCompany)
            '        GC.Collect()
            '        GC.WaitForPendingFinalizers()
            '        purchaseOrder = Nothing
            '        SCompany = Nothing
            '        Response.Redirect("GenerarPO.aspx")
            '    End If
        Catch ex As Exception
            Response.Write("btnProcesarOrden_Click " & ex.Message)
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
End Class

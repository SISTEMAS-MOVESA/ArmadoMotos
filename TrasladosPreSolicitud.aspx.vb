Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Web.Services
Imports System.Web.UI.WebControls.Expressions
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class TrasladosPreSolicitud
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=Movesa;uid=sa;password=M*l!n3r0s2k12"
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
    Public Sub cargarModelosMotos()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon2)
                Dim query As String = "select itemcode, itemname from oitm t0 with(nolock)  " &
                "inner join [@amodelo] t1 with(nolock) on t0.U_AMODELO=t1.Code where t0.itmsgrpcod =154 " &
                " And t1.U_LG5='Y' and t0.onhand>0 and t0.frozenfor='N' ORDER BY T0.ItemName"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
            drpModelosMoto.Dispose()
            drpModelosMoto.DataTextField = "itemname"
            drpModelosMoto.DataValueField = "itemcode"
            drpModelosMoto.DataSource = dt
            drpModelosMoto.DataBind()

        Catch ex As Exception
            Response.Write(Replace(ex.Message, "'", ""))
        End Try
    End Sub

    'Public Sub CargarMotoristas()
    '    Try
    '        Dim dt As DataTable = New DataTable()
    '        Using conn As SqlConnection = New SqlConnection(sCon2)
    '            Dim query As String = "select ID,RTN+ ' ' + EMPRESA + ' ' + PLACA [Empresa] from movesaweb..LOGISTICA_PLACAS_TRASLADOS order by EMPRESA"
    '            Dim cmd As SqlCommand = New SqlCommand(query, conn)
    '            Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
    '            da.Fill(dt)
    '        End Using
    '        drpMotoristas.Dispose()
    '        drpMotoristas.DataTextField = "Empresa"
    '        drpMotoristas.DataValueField = "ID"
    '        drpMotoristas.DataSource = dt
    '        drpMotoristas.DataBind()

    '    Catch ex As Exception
    '        Response.Write(Replace(ex.Message, "'", ""))
    '    End Try
    'End Sub

    Private Sub TrasladosPreSolicitud_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    cargarModelosMotos()
                    'CargarMotoristas()
                    BindGridSolicitudesAbiertas()

                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("TrasladosPreSolicitudSucursal.aspx")
                        'BuscarDatosDestinoLoad("T" & Session("ALMTRANSIT"))
                        'txtNalmacenDestino.Text = Session("USERROL")
                        'ocultarmenujefetienda.Visible = False
                    End If

                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    'Private Sub BindGridSolicitudesAbiertas()
    '    Try
    '        Dim constr As String = sCon2
    '        Using con As New SqlConnection(constr)
    '            Using cmd As New SqlCommand("SELECT [ID],[WHSCODE],[WHSNAME],[MODELO],[QTYSOLICITADA], " &
    '                                        "[QTYPREPARADA],[OBSERVACIONES],[DATECREATED],[DATEUPDATED] " &
    '                                        "FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] WHERE ESTATUS='PRESOLICITADO'")

    '                Using sda As New SqlDataAdapter()
    '                    cmd.Connection = con
    '                    sda.SelectCommand = cmd
    '                    Using dt As New DataTable()
    '                        sda.Fill(dt)
    '                        gridSolicitudAbierta.DataSource = dt
    '                        gridSolicitudAbierta.DataBind()
    '                        con.Close()
    '                    End Using
    '                End Using
    '            End Using
    '        End Using
    '        gridSolicitudAbierta.UseAccessibleHeader = True
    '        gridSolicitudAbierta.HeaderRow.TableSection = TableRowSection.TableHeader
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Public Function Presolicitud_Header(ALMORIGEN As String, ALMDESTINO As String, CODCLIALMDESTINO As String,
                                        OBSERACIONES As String, MOTORISTA As String, USERCREATED As String, DATECREATED As DateTime,
                                        ESTATUS As String, FECHADESEADA As DateTime) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "INSERT INTO [dbo].[PRESOLICITUD_HEADER] ([ALMORIGEN],[ALMDESTINO],[CODCLIALMDESTINO],[OBSERACIONES]," &
                " [MOTORISTA],[USERCREATED],[DATECREATED],[ESTATUS],[FECHADESEADA])" &
                " VALUES(@p1,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10); SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ALMORIGEN)
                cmd.Parameters.AddWithValue("@p3", ALMDESTINO)
                cmd.Parameters.AddWithValue("@p4", CODCLIALMDESTINO)
                cmd.Parameters.AddWithValue("@p5", OBSERACIONES)
                cmd.Parameters.AddWithValue("@p6", MOTORISTA)
                cmd.Parameters.AddWithValue("@p7", USERCREATED)
                cmd.Parameters.AddWithValue("@p8", DATECREATED)
                cmd.Parameters.AddWithValue("@p9", ESTATUS)
                cmd.Parameters.AddWithValue("@p10", FECHADESEADA)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("Presolicitud_Header " & ex.Message)
        End Try
    End Function
    Public Function Presolicitud_Lines(ARTICULO As String, CANTIDAD As String, ESPACIOS As String, ESTADO As String, ALMORIGEN As String,
                                       ALMDESTINO As String, USERCREATED As String, DESCRIPCION As String, RUTA As String, MODELO As String) As Integer
        Try
            Dim retrnvalue As Integer
            Dim impuestoRetro As Decimal = 0
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "INSERT INTO [dbo].[PRESOLICITUD_LINES] ([ARTICULO],[CANTIDAD],[ESPACIOS],[ESTADO],[ALMORIGEN],[ALMDESTINO],[USERCREATED],[DESCRIPCION],[RUTA],[MODELO])" &
                  " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10); SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ARTICULO)
                cmd.Parameters.AddWithValue("@p2", CANTIDAD)
                cmd.Parameters.AddWithValue("@p3", ESPACIOS)
                cmd.Parameters.AddWithValue("@p4", ESTADO)
                cmd.Parameters.AddWithValue("@p5", ALMORIGEN)
                cmd.Parameters.AddWithValue("@p6", ALMDESTINO)
                cmd.Parameters.AddWithValue("@p7", USERCREATED)
                cmd.Parameters.AddWithValue("@p8", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p9", RUTA)
                cmd.Parameters.AddWithValue("@p10", MODELO)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("Presolicitud_Lines " & ex.Message)
        End Try
    End Function
    Protected Sub btnAlmOrigen_Click(sender As Object, e As EventArgs) Handles btnAlmOrigen.Click
        BuscarDatosAlmacenOrigen(txtNalmacenOrigen.Text)
        BindGridSolicitudesAbiertas()
    End Sub
    Protected Sub btnAlmDestino_Click(sender As Object, e As EventArgs) Handles btnAlmDestino.Click
        BuscarDatosAlmacenDestino(txtNalmacenDestino.Text)
        BindGridSolicitudesAbiertas()
    End Sub
    Public Sub BuscarDatosAlmacenOrigen(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " select t0.WhsCode,t0.WhsName,t0.U_CardCode,t1.Name [Ruta], t2.Name [Responsable] " &
                                    " from OWHS t0 With(nolock) full join [@CRUTA] t1 With(nolock) On t0.U_Ruta=t1.Code " &
                                    " full join [@CRESPONSABLEOWHS] t2 with(nolock) on t0.U_Categorizacion=t2.Code " &
                                    " where t0.whsname ='" & whsname & "' "


            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtCodeAlmacenOrigen.Text = drd.Item("WhsCode").ToString
                txtNalmacenOrigen.Text = drd.Item("WhsName").ToString
                txtRutaAlmacenOrigen.Text = drd.Item("Ruta").ToString
                txtEncargadoOrigen.Text = drd.Item("Responsable").ToString
                'txtAlmacenCustomer.Text = drd.Item("U_CardCode").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub BuscarDatosDestinoLoad(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " select t0.WhsCode,t0.WhsName,t0.U_CardCode,t1.Name [Ruta], t2.Name [Responsable] " &
                                    " from OWHS t0 With(nolock) full join [@CRUTA] t1 With(nolock) On t0.U_Ruta=t1.Code " &
                                    " full join [@CRESPONSABLEOWHS] t2 with(nolock) on t0.U_Categorizacion=t2.Code " &
                                    " where t0.whscode ='" & whsname & "' "


            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtNalmacenDestino.ReadOnly = True
                txtCodeAlmDestino.Text = drd.Item("WhsCode").ToString
                txtNalmacenDestino.Text = drd.Item("WhsName").ToString
                txtRutaAlmacenDestino.Text = drd.Item("Ruta").ToString
                txtEncargadoDestino.Text = drd.Item("Responsable").ToString
                txtAlmacenCustomerDestino.Text = drd.Item("U_CardCode").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub BuscarDatosAlmacenDestino(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " select t0.WhsCode,t0.WhsName,t0.U_CardCode,t1.Name [Ruta], t2.Name [Responsable] " &
                                    " from OWHS t0 With(nolock) full join [@CRUTA] t1 With(nolock) On t0.U_Ruta=t1.Code " &
                                    " full join [@CRESPONSABLEOWHS] t2 with(nolock) on t0.U_Categorizacion=t2.Code " &
                                    " where whsname ='" & whsname & "' "
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtCodeAlmDestino.Text = drd.Item("WhsCode").ToString
                txtNalmacenDestino.Text = drd.Item("WhsName").ToString
                txtAlmacenCustomerDestino.Text = drd.Item("U_CardCode").ToString
                txtRutaAlmacenDestino.Text = drd.Item("Ruta").ToString
                txtEncargadoDestino.Text = drd.Item("Responsable").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub btnCrearSolicitud_Click(sender As Object, e As EventArgs) Handles btnCrearSolicitud.Click
        Try
            If drpMotivoTraslado.SelectedItem.Text = "Seleccione Un Motivo" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Debe Seleccionar un Motivo del Traslado! <hr> btnCrearSolicitud_Click');", True)
                Exit Sub
            End If

            If txtNalmacenDestino.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Primero Selccione Almacen de Transito, Destino! <hr> btnCrearSolicitud_Click');", True)
                Exit Sub
            End If

            If txtCantidad.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','La Cantidad Debe Ser Mayor a Cero! <hr> btnCrearSolicitud_Click');", True)
                Exit Sub
            End If
            Dim confirmacion As Integer = Presolicitud_Lines(drpModelosMoto.SelectedValue.ToString, txtCantidad.Text,
                                                            GetEspacios(drpModelosMoto.SelectedValue.ToString), "PRESOLICITUDTEMP",
                                                             txtCodeAlmacenOrigen.Text, txtCodeAlmDestino.Text, Session("UserCode"),
                                                             drpModelosMoto.SelectedItem.Text, txtRutaAlmacenDestino.Text, GetModeloMoto(drpModelosMoto.SelectedValue.ToString))
            If confirmacion > 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Datos Guardados Exitosamente! <hr> btnCrearSolicitud_Click');", True)
            End If
            BindGridSolicitudesAbiertas()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Function GetEspacios(articulo As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT [U_COLUMNA] FROM [MOVESA].[dbo].[OITM] where [ITEMCODE]=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", articulo)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
    Public Function GetModeloMoto(articulo As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "select t1.Name from oitm t0 with(nolock) inner join [@amodelo] t1 with(nolock) on t0.u_amodelo=t1.code where t0.itemcode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", articulo)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
    Private Sub BindGridSolicitudesAbiertas()
        Try

            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[ARTICULO],convert(decimal,[CANTIDAD])[CANTIDAD],convert(decimal,[ESPACIOS])[ESPACIOS],[ALMORIGEN],[DESCRIPCION] " &
                                            ", (SELECT WHSNAME FROM  MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE=[ALMORIGEN] COLLATE Modern_Spanish_CI_AS)[NALMACENO]" &
                                            " ,[ALMDESTINO]" &
                                            " ,(SELECT WHSNAME FROM  MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE=[ALMDESTINO] COLLATE Modern_Spanish_CI_AS)[NALMACEND]" &
                                            " FROM [ArmadoMotos].[dbo].[PRESOLICITUD_LINES] where [ESTADO] ='PRESOLICITUDTEMP' " &
                                            " and USERCREATED='" & Session("UserCode") & "'")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPreSolicitudAbierta.DataSource = dt
                            gridPreSolicitudAbierta.DataBind()
                            con.Close()
                            Dim totalMotos As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("CANTIDAD"))
                            Dim totalEspacios As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("ESPACIOS"))

                            gridPreSolicitudAbierta.FooterRow.Cells(3).Text = "Total Motos: " & totalMotos.ToString("N2")
                            gridPreSolicitudAbierta.FooterRow.Cells(3).Font.Bold = True
                            gridPreSolicitudAbierta.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Center

                            gridPreSolicitudAbierta.FooterRow.Cells(4).Text = "Total Espacios: " & totalEspacios.ToString("N2")
                            gridPreSolicitudAbierta.FooterRow.Cells(4).Font.Bold = True
                            gridPreSolicitudAbierta.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Center

                            gridPreSolicitudAbierta.UseAccessibleHeader = True
                            gridPreSolicitudAbierta.HeaderRow.TableSection = TableRowSection.TableHeader
                        End Using
                    End Using
                End Using
            End Using


            'Dim constr As String = sCon2
            'Using con As New SqlConnection(constr)
            '    Using cmd As New SqlCommand("SELECT [ID],[ARTICULO],[CANTIDAD],[ESPACIOS],[ALMORIGEN],[DESCRIPCION] " &
            '                                ", (SELECT WHSNAME FROM  MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE=[ALMORIGEN] COLLATE Modern_Spanish_CI_AS)[NALMACENO]" &
            '                                " ,[ALMDESTINO]" &
            '                                " ,(SELECT WHSNAME FROM  MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE=[ALMDESTINO] COLLATE Modern_Spanish_CI_AS)[NALMACEND]" &
            '                                " FROM [ArmadoMotos].[dbo].[PRESOLICITUD_LINES] where [ESTADO] ='PRESOLICITUDTEMP' " &
            '                                " and USERCREATED='" & Session("UserCode") & "'")

            '        Using sda As New SqlDataAdapter()
            '            cmd.Connection = con
            '            sda.SelectCommand = cmd
            '            Using dt As New DataTable()
            '                sda.Fill(dt)
            '                gridPreSolicitudAbierta.DataSource = dt
            '                gridPreSolicitudAbierta.DataBind()
            '                con.Close()
            '            End Using
            '        End Using
            '    End Using
            'End Using
            'Dim total As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("Precio"))
            'GridView1.FooterRow.Cells(1).Text = "Total Motos: " & dt.Rows.Count
            'GridView1.FooterRow.Cells(1).Font.Bold = True
            'GridView1.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
            'GridView1.FooterRow.Cells(7).Text = "Monto a Pagar: " & total.ToString("N2")
            'GridView1.FooterRow.Cells(7).Font.Bold = True

            'gridPreSolicitudAbierta.UseAccessibleHeader = True
            'gridPreSolicitudAbierta.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            'Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub gridPreSolicitudAbierta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPreSolicitudAbierta.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridPreSolicitudAbierta.Rows(index)
                EliminarLinea(gridPreSolicitudAbierta.Rows(index).Cells(0).Text)
                BindGridSolicitudesAbiertas()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub EliminarLinea(idlinea As Integer)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "DELETE FROM [PRESOLICITUD_LINES] where [ID] = @p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Moto Eliminada con Exito!<hr> EliminarLinea');", True)
            End Using
        Catch ex As Exception
            Response.Write("EliminarLinea " & ex.Message)
        End Try
    End Sub
    Protected Sub btnCrearCamion_Click(sender As Object, e As EventArgs) Handles btnCrearCamion.Click
        Try
            Dim id_header As Integer

            If gridPreSolicitudAbierta.Rows.Count > 0 Then

                If txtNalmacenOrigen.Text = "" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Seleccione Almacen de Origen...');", True)
                    Exit Sub
                End If
                If txtNalmacenDestino.Text = "" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Seleccione Almacen de Destino...');", True)
                    Exit Sub
                End If

                If drpMotivoTraslado.SelectedItem.Text = "Seleccione Un Motivo" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Seleccione Un Motivo de Traslado...');", True)
                    Exit Sub
                End If

                If txtFechaDeseada.Text = "" Then
                    txtFechaDeseada.Focus()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Verifique la Fecha Deseada <br> Fecha Deseada Incorrecta...');", True)
                    Exit Sub
                End If

                id_header = Presolicitud_Header(txtCodeAlmacenOrigen.Text, txtCodeAlmDestino.Text, txtAlmacenCustomerDestino.Text, drpMotivoTraslado.SelectedItem.Text, 0, Session("UserCode"), Date.Now, "PRESOLICITUD", txtFechaDeseada.Text)

                If id_header <> 0 Then
                    For Each row As GridViewRow In gridPreSolicitudAbierta.Rows
                        ActualziarLineasDetalle(id_header, Session("UserCode"), row.Cells(5).Text, row.Cells(7).Text, row.Cells(0).Text)
                    Next

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Solicitud Agregada Exitosamente!!');", True)
                    EnviarNotificacionEmail(Session("ALMTRANSIT").ToString.Trim, id_header)
                    BindGridSolicitudesAbiertas()
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Ocurrio un Error al Intentar Crear el Documento!!');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Debe agregar motos antes de crear la solicitud');", True)
                Exit Sub
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub EnviarNotificacionEmail(almdestino As String, headerid As String)
        Try
            Dim server As New SmtpClient
            Dim mensaje As New MailMessage
            server.Host = "mail.grupomovesa.com"
            server.Port = "587"
            server.Credentials = New System.Net.NetworkCredential("notificaciones@grupomovesa.com", "VYgd}T!XcVy}")
            mensaje.From = New MailAddress("notificaciones@grupomovesa.com")

            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("select t0.ID,t0.ALMDESTINO,t1.RUTA,t1.ARTICULO,t1.MODELO,t1.DESCRIPCION,t1.CANTIDAD,t1.ESPACIOS " &
                                                " from PRESOLICITUD_HEADER t0 with(nolock) inner join PRESOLICITUD_LINES t1 with(nolock) " &
                                                " on t0.id=t1.headerid " &
                                                " where t0.[ALMDESTINO]='" & almdestino & "' and t0.id=" & headerid & "")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            'gridCuadroBasico.DataSource = dt
                            'gridCuadroBasico.DataBind()
                            Dim pdfBytes As Byte() = ConvertDataTableToPdf(dt)
                            Dim pdfStream As New MemoryStream(pdfBytes)
                            mensaje.Attachments.Add(New Attachment(pdfStream, Session("ALMTRANSIT").ToString & ".pdf"))
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using

            mensaje.To.Add("jlogistica@grupomovesa.com")
            mensaje.CC.Add("nromero@grupomovesa.com")
            mensaje.Bcc.Add("pbustillo@grupomovesa.com")
            mensaje.Subject = headerid & " " & almdestino & " MOVESA - Portal Produccion de Motos"
            mensaje.Body = "" & Session("Name").ToString & ", Ha Confirmado el Sugerido de Motos" &
                            "<BR /> " &
                            "PORTAL Produccion de Motos By RJ"

            mensaje.IsBodyHtml = True
            mensaje.Priority = MailPriority.High
            server.Send(mensaje)
            Response.Write("<script>console.log('Correo Enviado');</script>")

        Catch ex As Exception
            Response.Write("<script>console.log('Exception " & Replace(Replace(Replace(ex.Message, ",", ""), "'", ""), ";", "") & "');</script>")
        End Try
    End Sub
    Public Function ConvertDataTableToPdf(dataTable As DataTable) As Byte()
        ' Crear el documento PDF en tamaño carta y formato horizontal
        Dim document As New Document(PageSize.LETTER.Rotate())
        Dim memoryStream As New MemoryStream()
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)
        document.Open()

        ' Crear una tabla en el documento PDF
        Dim table As New PdfPTable(dataTable.Columns.Count)
        table.WidthPercentage = 100

        ' Establecer el tamaño de letra y otros estilos de la celda
        Dim font As New Font(font.FontFamily.COURIER, 8)
        Dim cellStyle As New PdfPCell()
        cellStyle.HorizontalAlignment = Element.ALIGN_LEFT
        cellStyle.VerticalAlignment = Element.ALIGN_MIDDLE
        cellStyle.Padding = 5

        ' Agregar los encabezados de columna a la tabla
        For i As Integer = 0 To dataTable.Columns.Count - 1
            Dim phrase As New Phrase(dataTable.Columns(i).ColumnName, font)
            Dim headerCell As New PdfPCell(phrase)
            headerCell.BackgroundColor = New BaseColor(230, 230, 230)
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE
            headerCell.Padding = 5
            table.AddCell(headerCell)
        Next

        ' Agregar los datos del DataTable a la tabla
        For i As Integer = 0 To dataTable.Rows.Count - 1
            For j As Integer = 0 To dataTable.Columns.Count - 1
                Dim phrase As New Phrase(dataTable.Rows(i)(j).ToString(), font)
                Dim dataCell As New PdfPCell(phrase)
                dataCell.HorizontalAlignment = Element.ALIGN_LEFT
                dataCell.VerticalAlignment = Element.ALIGN_MIDDLE
                dataCell.Padding = 5
                'dataCell.NoWrap = True ' Evitar el wrap de texto
                table.AddCell(dataCell)
            Next
        Next

        ' Agregar la tabla al documento
        document.Add(table)
        document.Close()

        ' Convertir el documento PDF a un arreglo de bytes
        Dim pdfBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()

        Return pdfBytes
    End Function
    Public Sub ActualziarLineasDetalle(idheader As Integer, USUARIO As String, ALMORIGEN As String,
                                       ALMDESTINO As String, IDDETALLE As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[PRESOLICITUD_LINES] set headerid=@p1,[ESTADO]='PRESOLICITUD' " &
                    " where [ESTADO]='PRESOLICITUDTEMP' AND USERCREATED=@p2 AND ALMORIGEN=@p3 AND ALMDESTINO=@p4 AND [ID]=@p5"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idheader)
                cmd.Parameters.AddWithValue("@p2", USUARIO)
                cmd.Parameters.AddWithValue("@p3", ALMORIGEN)
                cmd.Parameters.AddWithValue("@p4", ALMDESTINO)
                cmd.Parameters.AddWithValue("@p5", IDDETALLE)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Pre-Solicitud Creada Con Exito!<hr> ActualziarLineasDetalle');", True)
            End Using
        Catch ex As Exception
            Response.Write("EliminarLinea " & ex.Message)
        End Try
    End Sub
    Public Sub gridPreSolicitudAbierta_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gridPreSolicitudAbierta.DataBound
        'Dim totalCol3 As Decimal = 0
        'Dim totalCol4 As Decimal = 0

        'For i As Integer = 0 To gridPreSolicitudAbierta.Rows.Count - 1
        '    totalCol3 += Convert.ToDecimal(gridPreSolicitudAbierta.Rows(i).Cells(3).Text)
        '    totalCol4 += Convert.ToDecimal(gridPreSolicitudAbierta.Rows(i).Cells(4).Text)
        'Next
        'littotal

        'litTotalCol3.Text = "Total columna 3: " & totalCol3.ToString()
        'litTotalCol4.Text = "Total columna 4: " & totalCol4.ToString()
    End Sub

End Class

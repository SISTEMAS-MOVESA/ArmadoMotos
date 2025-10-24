Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports DocumentFormat.OpenXml.ExtendedProperties
Imports SAPbobsCOM

Partial Class TrasladosConfirmarSerie
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
    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("TrasladosDashboard.aspx")
    End Sub

    <WebMethod()>
    Public Shared Function SearchSerie(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1
            Using cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = "select SERIE from armadomotos where SERIE LIKE '%' + @SearchText + '%'"
                cmd.Parameters.AddWithValue("@SearchText", prefixText)
                cmd.Connection = conn
                conn.Open()
                Dim customers As List(Of String) = New List(Of String)()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        customers.Add(sdr("SERIE").ToString())
                    End While
                End Using
                conn.Close()
                Return customers
            End Using
        End Using
    End Function
    Private Sub TrasladosConfirmarSerie_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGridSolicitudesAbiertas(Request.QueryString("macroid"))
                    BindGridSeriesConfirmadas(Request.QueryString("macroid"))
                    CargarMotoristas()
                    CargarMotivoTraslado()
                End If
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('TrasladosConfirmarSerie_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGridSolicitudesAbiertas(_macroid As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[WHSCODE],[WHSNAME],[MODELO],[DESCRIPCION],[QTYSOLICITADA], " &
                                            "[QTYPREPARADA],[OBSERVACIONES],[DATECREATED],[DATEUPDATED] " &
                                            "FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] WHERE [QTYSOLICITADA]<>[QTYPREPARADA] AND macroid = @p1 ")

                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@p1", _macroid)
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridSolicitudAbierta.DataSource = dt
                            gridSolicitudAbierta.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridSolicitudAbierta.UseAccessibleHeader = True
            gridSolicitudAbierta.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BindGridSeriesConfirmadas(_macroid As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[ARTICULO],[DESCRIPCION],[SERIE],[USERCREATED],[HEADERID],[CAMIONID] FROM [ArmadoMotos].[dbo].[SOLCITUD_LINES] WHERE [MACROID] = @p1 ")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@p1", _macroid)
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridConfirmacion.DataSource = dt
                            gridConfirmacion.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridConfirmacion.UseAccessibleHeader = True
            gridConfirmacion.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('v: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridSolicitudAbierta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridSolicitudAbierta.RowCommand
        Try
            If e.CommandName = "Confirmar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridSolicitudAbierta.Rows(index)
                txtModalIdLinea.Text = gridSolicitudAbierta.Rows(index).Cells(0).Text
                txtModalCamionId.Text = Request.QueryString("macroid")
                txtModalModeloMoto.Text = gridSolicitudAbierta.Rows(index).Cells(3).Text
                txtModalAlmDestino.Text = gridSolicitudAbierta.Rows(index).Cells(1).Text

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ModalSolicitud').modal('show');</script>", False)

                BindGridSolicitudesAbiertas(Request.QueryString("macroid"))
                BindGridSeriesConfirmadas(Request.QueryString("macroid"))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnModalConfirmarSerie_Click(sender As Object, e As EventArgs) Handles btnModalConfirmarSerie.Click
        Try
            If VerificarSerieExistente(txtModalSerie.Text) = "True" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Esa Serie ya fue Utilizada en una Solicitud Anterior, por favor verificar... <hr> btnModalConfirmarSerie_Click');", True)
                txtModalSerie.Text = ""
            Else

                If txtModalModeloMoto.Text.Trim = GetModeloBySerie(txtModalSerie.Text.Trim).Trim Then
                    SOLICITUD_LINES(Session("UserCode"), Date.Now, txtModalIdLinea.Text, txtModalCamionId.Text, txtModalSerie.Text, txtModalAlmDestino.Text)
                    BindGridSolicitudesAbiertas(Request.QueryString("macroid"))
                    BindGridSeriesConfirmadas(Request.QueryString("macroid"))
                    txtModalSerie.Text = ""
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','La Serie no Concuerda con el Modelo Solicitado, Por Favor Verificar Nuevamente... <hr> btnModalConfirmarSerie_Click');", True)
                    txtModalSerie.Text = ""
                End If
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Function GetModeloBySerie(serialnumber As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "select modelo from ArmadoMotos with(nolock) where serie = @p1 "
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", serialnumber)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Public Function VerificarSerieExistente(serialnumber As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = " if exists (select * from dbo.SOLCITUD_LINES where SERIE = @p1) " &
                " begin " &
                "Select 'True' [Response] " &
                "end " &
                "else " &
                "begin  " &
                "Select 'False' [Response] " &
                "end"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", serialnumber)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Public Sub SOLICITUD_LINES(USERCREATED As String, DATECREATED As DateTime, HEADERID As String, CAMIONID As String, SERIE As String, WHSCODE As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "INSERT INTO [dbo].[SOLCITUD_LINES]([ARTICULO],[DESCRIPCION],[SERIE],[USERCREATED],[DATECREATED],[HEADERID],[CAMIONID],[WHSCODE],[MACROID])" &
                    " select [ITEMCODE], [ITENMANE],[SERIE],@p1,@p2,@p3,@p4,@p6,@p4 from ArmadoMotos where SERIE=@p5; " &
                    "update [ArmadoMotos].[dbo].[SOLCITUD_HEADER] Set [ESTATUS] ='PROCESADO',[QTYPREPARADA]=[QTYPREPARADA]+1 where [ID]=@p3"

            'Response.Write(sel)
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", USERCREATED)
                cmd.Parameters.AddWithValue("@p2", DATECREATED)
                cmd.Parameters.AddWithValue("@p3", HEADERID)
                cmd.Parameters.AddWithValue("@p4", CAMIONID)
                cmd.Parameters.AddWithValue("@p5", SERIE)
                cmd.Parameters.AddWithValue("@p6", WHSCODE)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ActualizarEstado(HEADERID, CAMIONID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Solicitud Guardada con Exito!<hr> SOLICITUD_LINES');", True)
            End Using

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Ocurrio Un Error al Intentar Agregar Modelo <hr> SOLICITUD_LINES');", True)
            Response.Write("SOLICITUD_LINES " & ex.Message)
        End Try
    End Sub

    Private Sub gridConfirmacion_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridConfirmacion.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridConfirmacion.Rows(index)
                EliminarLinea(gridConfirmacion.Rows(index).Cells(0).Text, gridConfirmacion.Rows(index).Cells(4).Text)
            End If
            BindGridSolicitudesAbiertas(Request.QueryString("macroid"))
            BindGridSeriesConfirmadas(Request.QueryString("macroid"))
        Catch ex As Exception

        End Try
    End Sub
    Public Sub EliminarLinea(idlinea As Integer, idsolicitud As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "DELETE FROM [dbo].[SOLCITUD_LINES] where [ID] = @p1;" &
                "update [ArmadoMotos].[dbo].[SOLCITUD_HEADER] set [ESTATUS]='CAMION', [QTYPREPARADA]=[QTYPREPARADA]-1 where [ID] = @p2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                cmd.Parameters.AddWithValue("@p2", idsolicitud)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Moto Eliminada con Exito!<hr> EliminarLinea');", True)
            End Using
        Catch ex As Exception
            Response.Write("EliminarLinea " & ex.Message)
        End Try
    End Sub
    Public Sub ActualizarEstado(HEADERID As String, CAMIONID As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "UPDATE SOLCITUD_LINES SET ESTADO='PREPARADO' WHERE HEADERID = @p1 AND CAMIONID = @p2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", HEADERID)
                cmd.Parameters.AddWithValue("@p2", CAMIONID)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("ActualizarEstado " & ex.Message)
        End Try
    End Sub
    Public Sub ActualizarCantidadesHeader(HEADERID As String, CAMIONID As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "UPDATE SOLCITUD_LINES SET ESTADO='PREPARADO' WHERE HEADERID=@p1 AND CAMIONID=@p2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", HEADERID)
                cmd.Parameters.AddWithValue("@p2", CAMIONID)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("ActualizarEstado " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateMacroInsertHeaderId(headerid As String)
        Try
            Dim query As String = String.Empty
            query &= "update [ArmadoMotos].[dbo].[MACROINSERT_HEADER] set [ESTADO]=4 WHERE ID = @p1"
            Using conn As New SqlConnection(sCon2)
                Using cmd As New SqlCommand()
                    With cmd
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", headerid)
                    End With
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('UpdateMacroInsertHeaderId: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
    Private Sub btnConfirmarSolicitudTraslado_Click(sender As Object, e As EventArgs) Handles btnConfirmarSolicitudTraslado.Click
        Try
            IdentidadMotorista = drpMotoristas.SelectedValue.ToString
            CreateStockTransferRequest(Request.QueryString("macroid"))
        Catch ex As Exception
            Response.Write("btnConfirmarSolicitudTraslado_Click " & ex.Message)
        End Try
    End Sub
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
    Public Sub CreateStockTransferRequest(_macroid As String)

        Try
            If chkCasco.Checked = True Then
                If txtCodigoCasco.Text = "" Then
                    Response.Write("<script>alert('Falta Codigo de Casco');</script>")
                    Exit Sub
                End If
                If txtQtyCasco.Text = "" Then
                    Response.Write("<script>alert('Falta Cantidad de Cascos');</script>")
                    Exit Sub
                End If
            End If
            If GlobalConnecttoSAP() <> 0 Then
                Response.Write("<script>alert('Error al Intentar Conectarse a SAP');</script>")
                Exit Sub
            End If
            Dim macro_id As String
            Dim oStockTransferRequest As SAPbobsCOM.StockTransfer = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryTransferRequest)
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT Top 1  [WHSCODEORIGEN],[WHSCODE],[CODIGOCLIENTE],[OBSERVACIONES]," &
                                        " [CAMION],[MACROID] FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] where macroid = @p1 ")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@p1", _macroid)
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            For Each row As DataRow In dt.Rows
                                macro_id = row.Item("MACROID").ToString()
                                'si el almacen no es propio
                                Response.Write("<script>console.log('UpdateDocentrySAP: " & ReplaceCharsForFileName(GetTypeWhsCode(row.Item("WHSCODE").ToString()), " ") & "');</script>")

                                If GetTypeWhsCode(row.Item("WHSCODE").ToString()) <> "PRO" Then
                                    oStockTransferRequest.Series = 968
                                Else
                                    oStockTransferRequest.Series = 243
                                End If
                                oStockTransferRequest.DocDate = DateTime.Now
                                oStockTransferRequest.FromWarehouse = row.Item("WHSCODEORIGEN").ToString()

                                If GetTypeWhsCode(row.Item("WHSCODE").ToString()) = "PRO" AndAlso Left(row.Item("WHSCODE").ToString(), 1) <> "T" Then
                                    oStockTransferRequest.ToWarehouse = "T" & row.Item("WHSCODE").ToString()
                                Else
                                    oStockTransferRequest.ToWarehouse = row.Item("WHSCODE").ToString()

                                End If

                                oStockTransferRequest.CardCode = GetCardCodeByWHS(row.Item("WHSCODE").ToString())
                                oStockTransferRequest.Comments = "Sugerido #" & _macroid
                                oStockTransferRequest.JournalMemo = "Portal_Produccion"
                                oStockTransferRequest.PriceList = -2
                                oStockTransferRequest.UserFields.Fields.Item("U_P_MODELO").Value = row.Item("CAMION").ToString()
                                oStockTransferRequest.UserFields.Fields.Item("U_N_Cliente").Value = row.Item("OBSERVACIONES").ToString()
                                'Informacion del transportista
                                oStockTransferRequest.UserFields.Fields.Item("U_IdentConductor").Value = IdentidadMotorista
                                oStockTransferRequest.UserFields.Fields.Item("U_Mottras").Value = drpMotivoTraslado.SelectedValue.ToString
                                oStockTransferRequest.UserFields.Fields.Item("U_TipoTransporte").Value = drpTipoTransporte.SelectedValue.ToString
                                oStockTransferRequest.UserFields.Fields.Item("U_Envio").Value = _macroid

                                Dim datos As String(,) = obtenerDatos(IdentidadMotorista)

                                ' Recorrer los datos y mostrarlos en la consola
                                For i As Integer = 0 To datos.GetLength(1) - 1
                                    oStockTransferRequest.UserFields.Fields.Item("U_Transportista").Value = datos(0, i)
                                    oStockTransferRequest.UserFields.Fields.Item("U_Conductor").Value = datos(1, i)
                                    oStockTransferRequest.UserFields.Fields.Item("U_LicenciaConductor").Value = datos(2, i)
                                    oStockTransferRequest.UserFields.Fields.Item("U_MarcaModelo").Value = datos(3, i)
                                    oStockTransferRequest.UserFields.Fields.Item("U_placaTransporte").Value = datos(4, i)
                                    oStockTransferRequest.UserFields.Fields.Item("U_RTNTransportista").Value = datos(5, i)
                                Next

                                Dim constr2 As String = sCon1
                                Using con2 As New SqlConnection(constr2)
                                    Using cmd2 As New SqlCommand("select * from SOLCITUD_LINES where macroid = @p1 and WHSCODE = @p2 ")
                                        Using sda2 As New SqlDataAdapter()
                                            cmd2.Connection = con2
                                            cmd2.Parameters.AddWithValue("@p1", _macroid)
                                            cmd2.Parameters.AddWithValue("@p2", row.Item("WHSCODE").ToString())
                                            sda2.SelectCommand = cmd2
                                            Using dt2 As New DataTable()
                                                sda2.Fill(dt2)
                                                For Each row2 As DataRow In dt2.Rows
                                                    ' Add the item lines
                                                    oStockTransferRequest.Lines.ItemCode = row2.Item("ARTICULO").ToString()
                                                    oStockTransferRequest.Lines.SerialNumber = Trim(row2.Item("SERIE").ToString())
                                                    oStockTransferRequest.Lines.UserFields.Fields.Item("U_MSERIE").Value = Trim(row2.Item("SERIE").ToString())
                                                    oStockTransferRequest.Lines.UserFields.Fields.Item("U_MVin").Value = GetSerialSysNumber(Trim(row2.Item("SERIE").ToString()))

                                                    oStockTransferRequest.Lines.Quantity = 1
                                                    oStockTransferRequest.Lines.Add()
                                                    Response.Write("<script>window.open('TrasladosPermisoCirculacion.aspx?serie=" & Trim(row2.Item("SERIE").ToString()) & "','_blank');</script>")
                                                Next
                                            End Using
                                        End Using
                                    End Using
                                    con2.Close()
                                End Using
                                If chkCasco.Checked = True Then
                                    oStockTransferRequest.Lines.ItemCode = txtCodigoCasco.Text
                                    oStockTransferRequest.Lines.Quantity = CInt(txtQtyCasco.Text)
                                    oStockTransferRequest.Lines.Add()
                                End If
                                Dim iError As Integer = oStockTransferRequest.Add()
                                If iError <> 0 Then
                                    Dim sErrMsg As String = SCompany.GetLastErrorDescription()
                                    Throw New Exception(sErrMsg)
                                Else
                                    Dim sDocNum As String = SCompany.GetNewObjectKey()
                                    UpdateDocentrySAP(sDocNum, Date.Now, _macroid)
                                    'Response.Write("<script>alert('Stock transfer request created successfully. DocNum: " & sDocNum & "');</script>")
                                    Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Documento Creado!!! # " &
                                        GetDocumentoCreado(sDocNum) & "',position: 'topRight',timeout: 10000})</script>"
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                                    UpdateMacroInsertHeaderId(Request.QueryString("macroid"))
                                    FinalizarRegistro(Request.QueryString("macroid"))
                                End If
                            Next

                        End Using
                    End Using
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("CreateStockTransferRequest: " & ex.Message)
            Response.Write("<script>console.log('CreateStockTransferRequest: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    <WebMethod()>
    Public Shared Function FinalizarRegistro(id As Integer) As String
        Dim connectionString As String = sCon1
        Using con As New SqlConnection(connectionString)
            con.Open()
            Dim query As String = "UPDATE PRESOLICITUD_HEADER SET ESTADO = 'C' WHERE ID = @p1; " &
            " UPDATE [ArmadoMotos].[dbo].[MACROINSERT_HEADER] SET [ESTATUS]='C' WHERE ID = @p1 "
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@p1", id)
                cmd.ExecuteNonQuery()
                Return "Registro eliminado correctamente."
            End Using
            con.Close()
        End Using
        Return "Error al eliminar el registro."
    End Function
    Public Sub UpdateDocentrySAP(docentrysap As String, fecha As DateTime, macroid As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[SOLCITUD_HEADER] Set [DOCENTRYSAP] = @p1 ,[DATEPROCESEDSAP] = @p2,[ESTATUS] = 'PROCESADOSAP' where [MACROID] = @p3;"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", docentrysap)
                cmd.Parameters.AddWithValue("@p2", fecha)
                cmd.Parameters.AddWithValue("@p3", macroid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("UpdateDocentrySAP " & ex.Message)
            Response.Write("<script>console.log('UpdateDocentrySAP: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
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
    Public Function GetSerialSysNumber(serialNumber As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "select SysNumber from OSRN where MnfSerial = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", serialNumber)
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
        Dim sCon As String = sCon2
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
    Protected Sub btnCerrarDocumento_Click(sender As Object, e As EventArgs) Handles btnCerrarDocumento.Click
        Try
            lblCamion.Text = (CType(sender, Button)).CommandArgument
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ModalMotorista').modal('show');</script>", False)

            'If gridConfirmacion.Rows.Count > 0 Then
            '    lblCamion.Text = (CType(sender, Button)).CommandArgument
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ModalMotorista').modal('show');</script>", False)

            '    'UpdateMacroInsertHeaderId(Request.QueryString("macroid"))
            'Else
            '    Dim script As String = "<script>iziToast.warning({title: 'Advertencia!', message: 'Debe agregar Series Antes de Cerrar el Documento!!',position: 'topRight',timeout: 10000})</script>"
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            'End If
        Catch ex As Exception
            Response.Write("<script>console.log('btnCerrarDocumento_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
End Class

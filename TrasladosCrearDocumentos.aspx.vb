Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.UI.DataVisualization.Charting
Imports DocumentFormat.OpenXml.ExtendedProperties
Imports DocumentFormat.OpenXml.Spreadsheet
Imports SAPbobsCOM

Partial Class TrasladosCrearDocumentos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
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
    'Sub LoadGrid()
    '    Try
    '        Dim strSQL = "SELECT [CAMION],[PREIDHEADER],[WHSCODE],[DOCENTRYSAP],sum(qtysolicitada)[Solicitado],sum(qtypreparada)[Preparado], " &
    '                     " convert(decimal(19,2),sum(qtypreparada))/convert(decimal(19,2),sum(qtysolicitada))*100  [Porcentaje] " &
    '                     " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] " &
    '                     " group by [CAMION],[PREIDHEADER],[WHSCODE],[DOCENTRYSAP]"

    '        Using cmdSQL As New SqlCommand(strSQL, New SqlConnection(sCon2))

    '            cmdSQL.Connection.Open()
    '            MyTable.Load(cmdSQL.ExecuteReader)
    '            repeater1.DataSource = MyTable
    '            repeater1.DataBind()
    '            cmdSQL.Connection.Close()
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("LoadGrid " & ex.Message)
    '    End Try
    'End Sub
    Private Sub BindgridCargaCamion()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim strSQL = "SELECT [CAMION],[PREIDHEADER],[WHSCODE],[DOCENTRYSAP],sum(qtysolicitada)[Solicitado],sum(qtypreparada)[Preparado], " &
                         " convert(decimal(19,2),sum(qtypreparada))/convert(decimal(19,2),sum(qtysolicitada))*100  [Porcentaje] " &
                         " FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] " &
                         " group by [CAMION],[PREIDHEADER],[WHSCODE],[DOCENTRYSAP]"

                Using cmd As New SqlCommand(strSQL)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCargaCamion.DataSource = dt
                            gridCargaCamion.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridCargaCamion.UseAccessibleHeader = True
            gridCargaCamion.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub TrasladosCrearDocumentos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    CargarMotoristas()
                    CargarMotivoTraslado()
                    'LoadGrid()
                    BindgridCargaCamion()
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try

    End Sub
    Protected Sub btnAll_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Dim argument As String = TryCast(repeater1.FindControl("btnLogin"), System.Web.UI.WebControls.Button).CommandArgument.ToString()
        'argumento = (CType(sender, Button)).CommandArgument
        lblCamion.Text = (CType(sender, Button)).CommandArgument
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ModalMotorista').modal('show');</script>", False)


        'Dim
        'CreateStockTransferRequest(argumento)
        'Response.Redirect("TrasladosCargaCamion.aspx?idcamion=" & argumento & "")

        'Dim Cantidad As String = (CType(sender, Button)).Text
        'Response.Redirect("TrasladosConfirmarSerie.aspx?camion=" & argumento)
        'Response.Write("<script>console.log('" & argumento & " ');</script>")
        'For i As Integer = 1 To CInt(argumento)
        '    'Response.Write("<script>console.log('" & argumento & " / " & Cantidad & " ');</script>")
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ModalSolicitud').modal('show');</script>", False)
        'Next
        'Replace(argumento, "Aceptar Completo: ", "")
        'UpdateCamionCompleto(id, argumento, Session("UserCode"), Date.Now)
        'Dim argument = (CType(sender, Button)).CommandArgument.ToString()
        'Response.Write("<script>console.log('aqui');</script>")
        'Response.Write("<script>console.log('" & id & " ');</script>")
        'Response.Write("<script>console.log('" & argumento & " / " & Cantidad & " ');</script>")
    End Sub
    Protected Sub btn_Transfer(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim docentry As String = (CType(sender, Button)).CommandArgument
            Response.Redirect("TrasladosTransferencia.aspx?id=" & docentry)

        Catch ex As Exception

        End Try
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
    Public Sub CreateStockTransferRequest(CAMIONID As String)

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
                                        " [CAMION],[MACROID] FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] where CAMION=" & CAMIONID & " ")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
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
                                oStockTransferRequest.ToWarehouse = row.Item("WHSCODE").ToString()
                                oStockTransferRequest.CardCode = GetCardCodeByWHS(row.Item("WHSCODE").ToString())
                                oStockTransferRequest.Comments = "CAMION: " & row.Item("CAMION").ToString() & " / " & row.Item("OBSERVACIONES").ToString()
                                oStockTransferRequest.JournalMemo = "Portal_Produccion"
                                oStockTransferRequest.PriceList = -2
                                oStockTransferRequest.UserFields.Fields.Item("U_P_MODELO").Value = row.Item("CAMION").ToString()
                                oStockTransferRequest.UserFields.Fields.Item("U_N_Cliente").Value = row.Item("OBSERVACIONES").ToString()
                                'Informacion del transportista
                                oStockTransferRequest.UserFields.Fields.Item("U_IdentConductor").Value = IdentidadMotorista
                                oStockTransferRequest.UserFields.Fields.Item("U_Mottras").Value = drpMotivoTraslado.SelectedValue.ToString
                                oStockTransferRequest.UserFields.Fields.Item("U_TipoTransporte").Value = drpTipoTransporte.SelectedValue.ToString

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

                                Dim constr2 As String = sCon2
                                Using con2 As New SqlConnection(constr2)
                                    Using cmd2 As New SqlCommand("select * from SOLCITUD_LINES where CAMIONID=" & CAMIONID & "  and WHSCODE='" & row.Item("WHSCODE").ToString() & "' ")
                                        Using sda2 As New SqlDataAdapter()
                                            cmd2.Connection = con2
                                            sda2.SelectCommand = cmd2
                                            Using dt2 As New DataTable()
                                                sda2.Fill(dt2)
                                                For Each row2 As DataRow In dt2.Rows
                                                    ' Add the item lines
                                                    oStockTransferRequest.Lines.ItemCode = row2.Item("ARTICULO").ToString()
                                                    oStockTransferRequest.Lines.SerialNumber = Trim(row2.Item("SERIE").ToString())
                                                    oStockTransferRequest.Lines.UserFields.Fields.Item("U_MSERIE").Value = Trim(row2.Item("SERIE").ToString())
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
                                    UpdateDocentrySAP(sDocNum, Date.Now, CAMIONID, macro_id)
                                    'Response.Write("<script>alert('Stock transfer request created successfully. DocNum: " & sDocNum & "');</script>")
                                    Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Documento Creado!!! # " &
                                        GetDocumentoCreado(sDocNum) & "',position: 'topRight',timeout: 10000})</script>"
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                                End If
                            Next

                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            BindgridCargaCamion()
        Catch ex As Exception
            Response.Write("CreateStockTransferRequest: " & ex.Message)
            Response.Write("<script>console.log('CreateStockTransferRequest: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
    Public Function GetDocumentoCreado(docentry As String) As String
        Dim sCon As String = sCon1
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
            '"update [ArmadoMotos].[dbo].[MACROINSERT_HEADER] set ESTADO=5 WHERE ID=" & MACROID & ""
            'Response.Write("<script>console.log('UpdateDocentrySAP: " & ReplaceCharsForFileName(sel, " ") & "');</script>")
            'Response.Write("<script>console.log('Parametros: " & ReplaceCharsForFileName(docentrysap & " " & fecha & " " & camion & " " & MACROID, " ") & "');</script>")


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
    Private Sub btnConfirmarSolicitudTraslado_Click(sender As Object, e As EventArgs) Handles btnConfirmarSolicitudTraslado.Click
        Try
            IdentidadMotorista = drpMotoristas.SelectedValue.ToString
            CreateStockTransferRequest(lblCamion.Text)
        Catch ex As Exception
            Response.Write("btnConfirmarSolicitudTraslado_Click " & ex.Message)
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

    Private Sub gridCargaCamion_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCargaCamion.RowCommand
        Try
            If e.CommandName = "Series" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCargaCamion.Rows(index)
                'EliminarLinea(gridCargaCamion.Rows(index).Cells(0).Text)
                'Response.Redirect("TrasladosConfirmarSerie.aspx?camion=" & gridCargaCamion.Rows(index).Cells(0).Text)
                lblCamion.Text = gridCargaCamion.Rows(index).Cells(0).Text
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ModalMotorista').modal('show');</script>", False)
            End If
            BindgridCargaCamion()
        Catch ex As Exception
            Response.Write("<script>console.log('gridPedidoTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
End Class

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System
Imports System.Web.Script.Services
Imports System.Web.Services
Imports SAPbobsCOM

Partial Class ConfirmacionDacion
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
    Public Shared sCon2 As String = "server=192.168.1.60;database=PRUEBAS;uid=sa;password=S@pB1Sql"

    ''' <summary>
    ''' 'campos para actualizar la serie en runtime
    ''' </summary>
    Public RuntimeSerie As String
    Public RuntimeSCOLOR As String
    Public RuntimeSAno As String
    Public RuntimeNPoliza As String
    Public RuntimeNAduana As String
    Public RuntimeNItem As String
    Public RuntimeFPago As DateTime
    Public RuntimePrecioMat As String
    Public RuntimeItemcode As String
    ''' <summary>
    ''' fin de campos
    ''' </summary>

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

    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        Dim base64 As String = Request.Form(hfImageData.UniqueID).Split(",")(1)
        Dim bytes() As Byte = Convert.FromBase64String(base64)
        Dim filePath As String = String.Format("~/ImagenesRecuperacion/{0}.png", txtSerie.Text) 'Path.GetRandomFileName)
        Session("Img8") = filePath
        File.WriteAllBytes(Server.MapPath(filePath), bytes)
        pnlFirma.Visible = False
    End Sub

    Private Sub ConfirmacionDacion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'GESTORC
            If Session("Position").ToString = "GESTORC" Then
                pnlSubMenuOrdenRecuperacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = False
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            'SKA1001
            If Session("Position").ToString = "ADMIN" Then
                Response.Redirect("Menu.aspx")
            End If
            'VENDEDOR
            If Session("Position").ToString = "VENDEDOR" Then
                Response.Redirect("Menu.aspx")
            End If

            If Session("TaskNumber").ToString() = "NULL" Then
            Else
                _CargarAlmacenes()
                txtRecordId.Text = Session("TaskNumber").ToString()
                CLIENTE_SKG2(Session("TaskNumber").ToString())
            End If
            lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
        Catch ex As Exception
            ADD_LOG("Page_Load", ex.Message)
            Response.Write(ex.Message)
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

    Public Sub INSERT_ENTRADAOPSKG_PHOTOS_ONE(_CREATEDATE As DateTime, _SERIE As String, _IMG1 As String, _IMG2 As String, _IMG3 As String, _IMG4 As String, _IMG5 As String, _IMG6 As String, _IMG7 As String, _FIRMARECIBIDO As String, _USUARIO As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " INSERT INTO [MovesaWeb].[dbo].[ENTRADAOPSKG_PHOTOS_ONE] " &
                    " ([CREATEDATE],[SERIE],[IMG1],[IMG2],[IMG3],[IMG4],[IMG5],[IMG6],[IMG7],[FIRMARECIBIDO],[USUARIO])" &
                    " VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _CREATEDATE)
                cmd.Parameters.AddWithValue("@p2", _SERIE)
                cmd.Parameters.AddWithValue("@p3", _IMG1)
                cmd.Parameters.AddWithValue("@p4", _IMG2)
                cmd.Parameters.AddWithValue("@p5", _IMG3)
                cmd.Parameters.AddWithValue("@p6", _IMG4)
                cmd.Parameters.AddWithValue("@p7", _IMG5)
                cmd.Parameters.AddWithValue("@p8", _IMG6)
                cmd.Parameters.AddWithValue("@p9", _IMG7)
                cmd.Parameters.AddWithValue("@p10", _FIRMARECIBIDO)
                cmd.Parameters.AddWithValue("@p11", _USUARIO)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            ADD_LOG("INSERT_ENTRADAOPSKG_PHOTOS_ONE", ex.Message)
        End Try
    End Sub

    Public Sub INSERT_ENTRADAOPSKG_PHOTOS_TWO(_CREATEDATE As DateTime, _SERIE As String, _IMG1 As String, _IMG2 As String, _IMG3 As String, _IMG4 As String, _IMG5 As String, _IMG6 As String, _IMG7 As String, _FIRMARECIBIDO As String, _USUARIO As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " INSERT INTO [MovesaWeb].[dbo].[ENTRADAOPSKG_PHOTOS_ONE] " &
                    " ([CREATEDATE],[SERIE],[IMG1],[IMG2],[IMG3],[IMG4],[IMG5],[IMG6],[IMG7],[FIRMARECIBIDO],[USUARIO])" &
                    " VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _CREATEDATE)
                cmd.Parameters.AddWithValue("@p2", _SERIE)
                cmd.Parameters.AddWithValue("@p3", _IMG1)
                cmd.Parameters.AddWithValue("@p4", _IMG2)
                cmd.Parameters.AddWithValue("@p5", _IMG3)
                cmd.Parameters.AddWithValue("@p6", _IMG4)
                cmd.Parameters.AddWithValue("@p7", _IMG5)
                cmd.Parameters.AddWithValue("@p8", _IMG6)
                cmd.Parameters.AddWithValue("@p9", _IMG7)
                cmd.Parameters.AddWithValue("@p10", _FIRMARECIBIDO)
                cmd.Parameters.AddWithValue("@p11", _USUARIO)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            ADD_LOG("Page_Load", ex.Message)
        End Try
    End Sub

    Public Sub CLIENTE_SKG2(_ID As Integer)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "SELECT ID,CARDNAME,IDENTIDAD,MODELO,COLOR,SERIE,SERIEM " &
                                       " FROM [movesaweb].[dbo].[ENTRADAOPSKG] WITH(NOLOCK) WHERE ID=" & _ID & " "

            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtCardname.Text = drd.Item("cardname").ToString
                txtIdentidad.Text = drd.Item("Identidad").ToString
                txtModelo.Text = drd.Item("Modelo").ToString
                txtColor.Text = drd.Item("Color").ToString
                txtSerie.Text = drd.Item("Serie").ToString
                txtSerieM.Text = drd.Item("SerieM").ToString
            End If
            Audit(Date.Now, "CLIENTE_SKG2", Session("User").ToString, "Buscar: " & _ID)

        Catch ex As Exception
            ADD_LOG("CLIENTE_SKG2", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btnBuscarMoto_Click(sender As Object, e As EventArgs) Handles btnBuscarMoto.Click
        _CargarAlmacenes()
        CLIENTE_SKG2(txtRecordId.Text.ToString.Trim)
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("ConfirmacionDacion.aspx")
    End Sub

    Public Sub Cargar_Imagen_Moto()
        If fuImage1.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage1.FileName)
                fuImage1.SaveAs(Server.MapPath("~/ImagenesRecuperacion/") & txtSerie.Text & "_FRENTE_" & System.IO.Path.GetExtension(fuImage1.FileName))
                Session("Img1") = "~/ImagenesRecuperacion/" & txtSerie.Text & "_FRENTE_" & System.IO.Path.GetExtension(fuImage1.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage1", ex.Message)
            End Try
        End If

        If fuImage2.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage2.FileName)
                fuImage2.SaveAs(Server.MapPath("~/ImagenesRecuperacion/") & txtSerie.Text & "_ATRAS_" & System.IO.Path.GetExtension(fuImage2.FileName))
                Session("Img2") = "~/ImagenesRecuperacion/" & txtSerie.Text & "_ATRAS_" & System.IO.Path.GetExtension(fuImage2.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage2", ex.Message)
            End Try
        End If

        If fuImage3.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage3.FileName)
                fuImage3.SaveAs(Server.MapPath("~/ImagenesRecuperacion/") & txtSerie.Text & "_DERECHA_" & System.IO.Path.GetExtension(fuImage3.FileName))
                Session("Img3") = "~/ImagenesRecuperacion/" & txtSerie.Text & "_DERECHA_" & System.IO.Path.GetExtension(fuImage3.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage3", ex.Message)
            End Try
        End If

        If fuImage4.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage4.FileName)
                fuImage4.SaveAs(Server.MapPath("~/ImagenesRecuperacion/") & txtSerie.Text & "_IZQUIERDA_" & System.IO.Path.GetExtension(fuImage4.FileName))
                Session("Img4") = "~/ImagenesRecuperacion/" & txtSerie.Text & "_IZQUIERDA_" & System.IO.Path.GetExtension(fuImage4.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage4", ex.Message)
            End Try
        End If

        If fuImage5.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage5.FileName)
                fuImage5.SaveAs(Server.MapPath("~/ImagenesRecuperacion/") & txtSerie.Text & "_TACOMETRO_" & System.IO.Path.GetExtension(fuImage5.FileName))
                Session("Img5") = "~/ImagenesRecuperacion/" & txtSerie.Text & "_TACOMETRO_" & System.IO.Path.GetExtension(fuImage5.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage5", ex.Message)
            End Try
        End If

        If fuImage6.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage6.FileName)
                fuImage6.SaveAs(Server.MapPath("~/ImagenesRecuperacion/") & txtSerie.Text & "_ARRIBA_" & System.IO.Path.GetExtension(fuImage6.FileName))
                Session("Img6") = "~/ImagenesRecuperacion/" & txtSerie.Text & "_ARRIBA_" & System.IO.Path.GetExtension(fuImage6.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage6", ex.Message)
            End Try
        End If

        If fuImage7.HasFile Then
            Try
                Dim filename As String = Path.GetFileName(fuImage7.FileName)
                fuImage7.SaveAs(Server.MapPath("~/ImagenesRecuperacion/") & txtSerie.Text & "_SERIE_" & System.IO.Path.GetExtension(fuImage7.FileName))
                Session("Img7") = "~/ImagenesRecuperacion/" & txtSerie.Text & "_SERIE_" & System.IO.Path.GetExtension(fuImage7.FileName)
            Catch ex As Exception
                Response.Write("Ocurrio un error al intentar subir la foto" + ex.Message)
                ADD_LOG("fuImage7", ex.Message)
            End Try
        End If
    End Sub

    Public Sub UpdateBikePictures(_ID As Integer, _name1 As String, _name2 As String, _name3 As String, _name4 As String, _name5 As String _
                                  , _name6 As String, _name7 As String, _name8 As String, _placa As String, _nivelcombustible As String _
                                  , _whscode As String, _whsname As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " UPDATE [MOVESAWEB].[dbo].[ENTRADAOPSKG] " &
                    "    SET [ESTADOMOTO] = @Estado " &
                    "       ,[IMG1] = @img1 " &
                    "       ,[IMG2] = @img2 " &
                    "       ,[IMG3] = @img3 " &
                    "       ,[IMG4] = @img4 " &
                    "       ,[IMG5] = @img5 " &
                    "       ,[IMG6] = @img6 " &
                    "       ,[IMG7] = @img7 " &
                    "       ,[FIRMARECIBIDO] = @img8 " &
                    "       ,[ESTADO] = 'Recibida' " &
                    "       ,[PLACA]=@p9 " &
                    "       ,[NIVELCOMBUSTIBLE]=@p10 " &
                    "       ,[WHSCODE]=@p11 " &
                    "       ,[WHSNAME]=@p12 " &
                    "  WHERE [ID]= @id "

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@Estado", txtEstadoMoto.Text)
                cmd.Parameters.AddWithValue("@img1", _name1)
                cmd.Parameters.AddWithValue("@img2", _name2)
                cmd.Parameters.AddWithValue("@img3", _name3)
                cmd.Parameters.AddWithValue("@img4", _name4)
                cmd.Parameters.AddWithValue("@img5", _name5)
                cmd.Parameters.AddWithValue("@img6", _name6)
                cmd.Parameters.AddWithValue("@img7", _name7)
                cmd.Parameters.AddWithValue("@img8", _name8)
                cmd.Parameters.AddWithValue("@id", _ID)
                cmd.Parameters.AddWithValue("@p9", _placa)
                cmd.Parameters.AddWithValue("@p10", _nivelcombustible)
                cmd.Parameters.AddWithValue("@p11", _whscode)
                cmd.Parameters.AddWithValue("@p12", _whsname)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Audit(Date.Now, "UpdateBikePictures", Session("User").ToString, "Dacion: #" & _ID)
        Catch ex As Exception
            ADD_LOG("UpdateBikePictures", ex.Message)
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Cargar_Imagen_Moto()
        pnlFotosMoto.Visible = False
        pnlFirma.Visible = True
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try

            If drpCombustible.SelectedItem.ToString = "Seleccione Nivel" Then
                Response.Write("<script>alert('Debe Seeccionar Nivel de Combustible, Antes de Continuar');</script>")
                Exit Sub
            End If

            If drpPlaca.SelectedItem.ToString = "Seleccione Opcion" Then
                Response.Write("<script>alert('Debe Seeccionar Si Trae Placa, Si o No!, Antes de Continuar');</script>")
                Exit Sub
            End If

            If txtEstadoMoto.Text = "" Then
                Response.Write("<script>alert('Debe Agregar los Comentarios de Recepcion de la Moto!!!, Si o No!, Antes de Continuar');</script>")
                Exit Sub
            End If

            UpdateBikePictures(txtRecordId.Text.ToString.Trim, Session("Img1").ToString.Trim, Session("Img2").ToString.Trim, Session("Img3").ToString.Trim, Session("Img4").ToString.Trim, Session("Img5").ToString.Trim, Session("Img6").ToString.Trim, Session("Img7").ToString.Trim, Session("Img8").ToString.Trim, drpPlaca.SelectedValue, drpCombustible.SelectedItem.ToString, drpAlmacenes.SelectedValue, drpAlmacenes.SelectedItem.ToString)
            INSERT_ENTRADAOPSKG_PHOTOS_ONE(Date.Now, txtSerie.Text, Session("Img1").ToString.Trim, Session("Img2").ToString.Trim, Session("Img3").ToString.Trim, Session("Img4").ToString.Trim, Session("Img5").ToString.Trim, Session("Img6").ToString.Trim, Session("Img7").ToString.Trim, Session("Img8").ToString.Trim, Session("User").ToString)
            GenerarIngresoMda(txtRecordId.Text.ToString.Trim)
        Catch ex As Exception
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

    Public Sub GenerarIngresoMda(_oORDER As Integer)
        GlobalConnecttoSAP()
        Dim cnSN As New SqlConnection
        cnSN.ConnectionString = sCon1
        cnSN.Open()
        Dim tablaSN As New DataTable
        Dim comSN As SqlDataAdapter
        comSN = New SqlDataAdapter("SELECT [ID],[DESEMBOLSO],[CARDCODE],[CARDNAME],[IDENTIDAD],[RTN],[ITEMCODE],[WHSCODE],[SERIE],[SERIEM],[YEAR] " &
                                    " ,[POLIZA],[ADUANA],[ITEM],[FPAGOADUANA],isnull(MONTOMATRICULA,0.00) [MONTOMATRICULA],[MONTODACION] " &
                                    " ,[MODELO],[MARCA],[CILINDROS],[COLOR],[MOTORM] " &
                                    " FROM [MOVESAWeb].[dbo].[ENTRADAOPSKG] with(nolock) where [ID]=" & _oORDER & "", cnSN)

        tablaSN.Clear()
        comSN.Fill(tablaSN)
        For Each rowSN As DataRow In tablaSN.Rows

            'Entrada de Mercaderia.
            oGoodsRcpt = SCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes)
            'ENCABEZADO
            oGoodsRcpt.CardCode = "PL1000"
            oGoodsRcpt.DocDate = Date.Now
            oGoodsRcpt.DocDueDate = Date.Now
            oGoodsRcpt.NumAtCard = _oORDER
            oGoodsRcpt.Comments = "Ingreso DACION de Pago #: " & _oORDER
            oGoodsRcpt.Reference2 = "P-DACION"
            oGoodsRcpt.UserFields.Fields.Item("U_Desembolso").Value = rowSN("DESEMBOLSO").ToString()
            oGoodsRcpt.UserFields.Fields.Item("U_DACION").Value = rowSN("ID").ToString()
            oGoodsRcpt.UserFields.Fields.Item("U_CODCLI").Value = rowSN("CARDCODE").ToString()
            oGoodsRcpt.UserFields.Fields.Item("U_NCLI").Value = rowSN("CARDNAME").ToString()
            oGoodsRcpt.UserFields.Fields.Item("U_IDCLI").Value = rowSN("IDENTIDAD").ToString()
            oGoodsRcpt.UserFields.Fields.Item("U_RTNCLI").Value = rowSN("RTN").ToString()
            'DETALLE
            oGoodsRcpt.Lines.ItemCode = rowSN("ITEMCODE").ToString()
            oGoodsRcpt.Lines.Quantity = 1
            oGoodsRcpt.Lines.Price = rowSN("MONTODACION").ToString()
            oGoodsRcpt.Lines.WarehouseCode = rowSN("WHSCODE").ToString()
            oGoodsRcpt.Lines.UserFields.Fields.Item("U_Serie").Value = rowSN("SERIE").ToString()
            RuntimeSerie = rowSN("SERIE").ToString()
            oGoodsRcpt.Lines.UserFields.Fields.Item("U_Modelo").Value = rowSN("MODELO").ToString()
            oGoodsRcpt.Lines.UserFields.Fields.Item("U_MOTOR").Value = rowSN("MOTORM").ToString()
            oGoodsRcpt.Lines.UserFields.Fields.Item("U_Color").Value = rowSN("COLOR").ToString()
            oGoodsRcpt.Lines.UserFields.Fields.Item("U_Cilindros").Value = rowSN("CILINDROS").ToString()
            oGoodsRcpt.Lines.UserFields.Fields.Item("U_MAno").Value = rowSN("YEAR").ToString()
            'SERIE
            oGoodsRcpt.Lines.SerialNumbers.ManufacturerSerialNumber = rowSN("SERIE").ToString()
            oGoodsRcpt.Lines.SerialNumbers.InternalSerialNumber = rowSN("SERIEM").ToString()
            oGoodsRcpt.Lines.SerialNumbers.BatchID = rowSN("YEAR").ToString()
            oGoodsRcpt.Lines.SerialNumbers.ReceptionDate = Date.Now
            RuntimeSCOLOR = rowSN("COLOR").ToString()
            RuntimeSAno = rowSN("YEAR").ToString()
            RuntimeNPoliza = rowSN("POLIZA").ToString()
            Runtimenaduana = rowSN("ADUANA").ToString()
            RuntimeNItem = rowSN("ITEM").ToString()
            RuntimeFPago = rowSN("FPAGOADUANA").ToString()
            RuntimePrecioMat = rowSN("MONTOMATRICULA").ToString()
            oGoodsRcpt.Lines.SerialNumbers.Add()
            oGoodsRcpt.Lines.Add()
        Next
        lRetCode = oGoodsRcpt.Add

        If lRetCode <> 0 Then
            ADD_LOG("Generar Ingreso", "Referencia #" & _oORDER & " - " & SCompany.GetLastErrorDescription)
            SCompany.Disconnect()
        Else
            UpdateOSRN_UserFields(RuntimeSerie, RuntimeSCOLOR, RuntimeSAno, RuntimeNPoliza, RuntimeNAduana, RuntimeNItem, RuntimeFPago, RuntimePrecioMat)
            oRecordSet = Nothing
            oRecordSet = SCompany.GetBusinessObject(BoObjectTypes.BoRecordset)
            UpdateConfirmacionIngresoSAP(SCompany.GetNewObjectKey, Date.Now, RuntimeSerie)
            oRecordSet.DoQuery("select t0.DocNum from opdn t0 with(nolock) where t0.DocEntry= " & SCompany.GetNewObjectKey & "")
            ADD_LOG("Generar Ingreso", "El Ingreso se Creo Exitosamente, No.: " & oRecordSet.Fields.Item("Docnum").Value)
            SCompany.Disconnect()
            Audit(Date.Now, "GenerarIngresoMda", Session("User").ToString, "Dacion: #" & _oORDER)
        End If
    End Sub

    Public Sub UpdateOSRN_UserFields(_Serie As String, _U_SCOLOR As String, _U_SAno As String, _U_NPoliza As String, _U_NAduana As String, _U_NItem As String, _U_FPago As DateTime, _U_PrecioMat As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = " UPDATE [PRUEBAS].[dbo].[OSRN] " &
                    " SET U_SCOLOR =@P1 " &
                    " ,U_SAno =@P2 " &
                    " ,U_NPoliza =@P3 " &
                    " ,U_NItem =@P4 " &
                    " ,U_NAduana =@P5 " &
                    " ,U_FPago =@P6 " &
                    " ,U_PrecioMat =convert(decimal(19,2),@P7) " &
                    " WHERE MnfSerial =@P8 "

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@P1", _U_SCOLOR)
                cmd.Parameters.AddWithValue("@P2", _U_SAno)
                cmd.Parameters.AddWithValue("@P3", _U_NPoliza)
                cmd.Parameters.AddWithValue("@P4", _U_NItem)
                cmd.Parameters.AddWithValue("@P5", _U_NAduana)
                cmd.Parameters.AddWithValue("@P6", _U_FPago)
                cmd.Parameters.AddWithValue("@P7", _U_PrecioMat)
                cmd.Parameters.AddWithValue("@P8", _Serie)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Audit(Date.Now, "UpdateOSRN_UserFields", Session("User").ToString, "Serie: " & _Serie)

        Catch ex As Exception
            ADD_LOG("Update OSRN User Fields", ex.Message)
        End Try
    End Sub

    Public Sub UpdateConfirmacionIngresoSAP(_docentry As Integer, _docdate As DateTime, _serie As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " update MOVESAWeb.dbo.ENTRADAOPSKG " &
                            " set docentryingresosap=@P1" &
                            " ,DOCATEINGRESOSAP=@P2" &
                            " ,ESTATUSINGRESO='Terminado'" &
                            " WHERE SERIE=@P3"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@P1", _docentry)
                cmd.Parameters.AddWithValue("@P2", _docdate)
                cmd.Parameters.AddWithValue("@P3", _serie)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
            Audit(Date.Now, "UpdateConfirmacionIngresoSAP", Session("User").ToString, "Serie: " & _serie)
        Catch ex As Exception
            ADD_LOG("UpdateConfirmacionIngresoSAP", ex.Message)
        End Try
    End Sub

    Public Function GlobalConnecttoSAP() As Integer
        SCompany = New SAPbobsCOM.Company
        SCompany.Server = "192.168.1.60"
        SCompany.CompanyDB = "PRUEBAS" '"MOVESA"
        SCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014
        SCompany.DbUserName = "sa"
        SCompany.DbPassword = "S@pB1Sql"
        SCompany.UserName = "it"
        SCompany.Password = "1234"
        SCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish_La
        SCompany.SLDServer = "192.168.1.60:40000"
        lRetCode = SCompany.Connect
        Return lRetCode
    End Function
End Class

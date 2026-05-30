Imports System.Data
Imports System.Data.SqlClient

Partial Class SeleccionarMoto
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String
    Private Sub SeleccionarMoto_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                lblModeloCode.Text = Request.QueryString("codmodelo")
                lblModeMoto.Text = Request.QueryString("modelo")
                lblSugerido.Text = Request.QueryString("sugerido")
                lblFALTANTE.Text = Request.QueryString("faltante")
                lblVTAA.Text = Request.QueryString("vta")
                lblruta.Text = Session("rutaAlmacen")
                lblAlmDestino.Text = Session("almacenDestino")
                lblCB.Text = Request.QueryString("cb")
                lblplanId.Text = Request.QueryString("planId")
                lblFISICO.Text = IIf(String.IsNullOrEmpty(Request.QueryString("fisico")), 0, Request.QueryString("fisico"))
                lblcardcode.Text = getCardcode(Session("almacenDestino"))
                Response.Write("<script>console.log('BindgridColoresMoto: " & ReplaceCharsForFileName(Request.QueryString("codmodelo"), " ") & "');</script>")
                Response.Write("<script>console.log('BindgridColoresMoto: " & ReplaceCharsForFileName(Session("almacenDestino"), " ") & "');</script>")
                BindgridColoresMoto(Request.QueryString("codmodelo"), Session("almacenDestino"))
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub BindgridColoresMoto(MODELO As String, WHSCODE As String)
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("select	t0.itemcode, t0.itemname, t1.Name [Modelo], t0.u_columna [Espacios] " &
                        " , (select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode='DCM00' AND ItemCode=T0.ItemCode)[CEDIS] " &
                        " ,(select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode = @whscode AND ItemCode=T0.ItemCode)[SUCURSAL] " &
                        " From movesa..oitm t0 with(nolock)  inner Join movesa..[@AMODELO] t1 with(nolock) On t0.U_AMODELO=t1.Code " &
                        " where t0.U_AMODELO = @modelo and ItmsGrpCod=154 and t0.frozenFor='N' and t0.onhand>0")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@whscode", WHSCODE)
                        cmd.Parameters.AddWithValue("@modelo", MODELO)
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridColoresMoto.DataSource = dt
                            gridColoresMoto.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridColoresMoto.UseAccessibleHeader = True
            gridColoresMoto.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridColoresMoto: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub btnModalColoresMotos_Click(sender As Object, e As EventArgs) Handles btnModalColoresMotos.Click
        Try
            For Each row As GridViewRow In gridColoresMoto.Rows
                Dim str As String = TryCast(row.FindControl("qty"), System.Web.UI.WebControls.TextBox).Text
                If str > 0 Then
                    'tabla macroinsert
                    InsertSugeridoSucursales(
                                lblruta.Text,
                                lblAlmOrigen.Text,
                                lblAlmDestino.Text,
                                lblcardcode.Text,
                                row.Cells(0).Text,
                                row.Cells(2).Text,
                                row.Cells(1).Text,
                                row.Cells(3).Text * str,
                                str,
                                str,
                                0,
                                lblSugerido.Text,
                                Date.Now,
                                Session("User"),
                                lblCB.Text,
                                lblFISICO.Text,
                                lblFALTANTE.Text,
                                lblVTAA.Text)

                    If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                        'tabla de control lineas de despacho
                        InsertSugeridoSucursalesDespacho(lblruta.Text,
                                lblAlmOrigen.Text,
                                lblAlmDestino.Text,
                                lblcardcode.Text,
                                row.Cells(0).Text,
                                row.Cells(2).Text,
                                row.Cells(1).Text,
                                row.Cells(3).Text * str,
                                str,
                                str,
                                0,
                                lblSugerido.Text,
                                Date.Now,
                                Session("User"),
                                lblCB.Text,
                                lblFISICO.Text,
                                lblFALTANTE.Text,
                                lblVTAA.Text,
                                lblModeloCode.Text)
                    End If
                End If
            Next
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CerrarVentana", "window.close();", True)
            'Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Moto Agregada Con Exito!!!',position: 'topRight',timeout: 10000})</script>"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)

            btnModalColoresMotos.Visible = False
            maincontainer.Visible = False
        Catch ex As Exception
            Response.Write("<script>console.log('btnModalColoresMotos_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function InsertSugeridoSucursales(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String, ARTICULO As String,
                                                MODELO As String, DESCRIPCION As String, ESPACIOS As String,
                                                CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                                OBSERVACIONES As String, FECHA As Date, USUARIO As String,
                                                CB As String, FISICO As String, FALTANTE As String, VTAA As String) As Integer
        Try

            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim planId As Integer = 0

            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planId = Request.QueryString("planId")
            End If

            Dim SQL_QRY As String = "IF EXISTS (SELECT 1 FROM [MACROINSERT] WHERE [ALMDESTINO] = @p3 " &
                                    " And [ARTICULO] = @p5 And isnull(HEADERID,0)=0 and isnull([ESTADO],'-')='T')" &
                    " BEGIN " &
                    " UPDATE [MACROINSERT] " &
                    " SET CANTIDAD = CANTIDAD + @p9 " &
                    " ,QTYLOGISTICA = CANTIDAD + @p10 " &
                    " WHERE [ALMDESTINO] = @p3 And [ARTICULO] = @p5; " &
                    " END " &
                    " ELSE " &
                    " BEGIN " &
                    " INSERT INTO [dbo].[MACROINSERT] " &
                    " ([RUTA], [ALMORIGEN], [ALMDESTINO], [CARDCODE], [ARTICULO], [MODELO], [DESCRIPCION], [ESPACIOS],  " &
                    " [CANTIDAD], [QTYLOGISTICA], [QTYSUCURSAL], [OBSERVACIONES], [ESTADO], [TIPO], [FECHA],  " &
                    " [USUARIO], [CB], [FISICO], [FALTANTE], [VTAA],[PLANID])  " &
                    " VALUES  " &
                    " (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21); " &
                    " SELECT SCOPE_IDENTITY() AS InsertedID; " &
                    " END;"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(SQL_QRY, con)
                cmd.Parameters.AddWithValue("@p1", RUTA)
                cmd.Parameters.AddWithValue("@p2", ALMORIGEN)
                cmd.Parameters.AddWithValue("@p3", ALMDESTINO)
                cmd.Parameters.AddWithValue("@p4", CARDCODE)
                cmd.Parameters.AddWithValue("@p5", ARTICULO)
                cmd.Parameters.AddWithValue("@p6", MODELO)
                cmd.Parameters.AddWithValue("@p7", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p8", ESPACIOS)
                cmd.Parameters.AddWithValue("@p9", CANTIDAD)
                cmd.Parameters.AddWithValue("@p10", QTYLOGISTICA)
                cmd.Parameters.AddWithValue("@p11", QTYSUCURSAL)
                cmd.Parameters.AddWithValue("@p12", OBSERVACIONES)
                cmd.Parameters.AddWithValue("@p13", "T")
                cmd.Parameters.AddWithValue("@p14", "CD")
                cmd.Parameters.AddWithValue("@p15", FECHA)
                cmd.Parameters.AddWithValue("@p16", USUARIO)
                cmd.Parameters.AddWithValue("@p17", CB)
                cmd.Parameters.AddWithValue("@p18", FISICO)
                cmd.Parameters.AddWithValue("@p19", FALTANTE)
                cmd.Parameters.AddWithValue("@p20", VTAA)
                cmd.Parameters.AddWithValue("@p21", planId)

                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Function InsertSugeridoSucursalesDespacho(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String, ARTICULO As String,
                                                MODELO As String, DESCRIPCION As String, ESPACIOS As String,
                                                CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                                OBSERVACIONES As String, FECHA As Date, USUARIO As String,
                                                CB As String, FISICO As String, FALTANTE As String, VTAA As String, CODMODELO As String) As Integer
        Try

            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim planId As Integer = 0
            Dim headerId As Integer = 0

            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planId = Request.QueryString("planId")
                headerId = Request.QueryString("headerId")
            End If

            Dim SQL_QRY As String = "IF EXISTS (SELECT 1 FROM [DESPACHOS_DETALLE_MOTOS] WHERE [ALMDESTINO] = @p3 " &
                                    " And [ARTICULO] = @p5 And isnull(HEADERID,0)=0 and isnull([ESTADO],'-')='T')" &
                    " BEGIN " &
                    " UPDATE [DESPACHOS_DETALLE_MOTOS] " &
                    " SET CANTIDAD = CANTIDAD + @p9 " &
                    " ,QTYLOGISTICA = CANTIDAD + @p10 " &
                    " WHERE [ALMDESTINO] = @p3 And [ARTICULO] = @p5; " &
                    " END " &
                    " ELSE " &
                    " BEGIN " &
                    " INSERT INTO [dbo].[DESPACHOS_DETALLE_MOTOS] " &
                    " ([RUTA], [ALMORIGEN], [ALMDESTINO], [CARDCODE], [ARTICULO], [MODELO], [DESCRIPCION], [ESPACIOS],  " &
                    " [CANTIDAD], [QTYLOGISTICA], [QTYSUCURSAL], [OBSERVACIONES], [ESTADO], [TIPO], [FECHA],  " &
                    " [USUARIO], [CB], [FISICO], [FALTANTE], [VTAA],[PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO])  " &
                    " VALUES  " &
                    " (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24); " &
                    " SELECT SCOPE_IDENTITY() AS InsertedID; " &
                    " END;"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(SQL_QRY, con)
                cmd.Parameters.AddWithValue("@p1", RUTA)
                cmd.Parameters.AddWithValue("@p2", ALMORIGEN)
                cmd.Parameters.AddWithValue("@p3", ALMDESTINO)
                cmd.Parameters.AddWithValue("@p4", CARDCODE)
                cmd.Parameters.AddWithValue("@p5", ARTICULO)
                cmd.Parameters.AddWithValue("@p6", MODELO)
                cmd.Parameters.AddWithValue("@p7", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p8", ESPACIOS)
                cmd.Parameters.AddWithValue("@p9", CANTIDAD)
                cmd.Parameters.AddWithValue("@p10", QTYLOGISTICA)
                cmd.Parameters.AddWithValue("@p11", QTYSUCURSAL)
                cmd.Parameters.AddWithValue("@p12", OBSERVACIONES)
                cmd.Parameters.AddWithValue("@p13", "T")
                cmd.Parameters.AddWithValue("@p14", "CD")
                cmd.Parameters.AddWithValue("@p15", FECHA)
                cmd.Parameters.AddWithValue("@p16", USUARIO)
                cmd.Parameters.AddWithValue("@p17", CB)
                cmd.Parameters.AddWithValue("@p18", FISICO)
                cmd.Parameters.AddWithValue("@p19", FALTANTE)
                cmd.Parameters.AddWithValue("@p20", VTAA)
                cmd.Parameters.AddWithValue("@p21", planId)
                cmd.Parameters.AddWithValue("@p22", headerId)
                cmd.Parameters.AddWithValue("@p23", "ABIERTO")
                cmd.Parameters.AddWithValue("@p24", CODMODELO)

                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Function getCardcode(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT u_cardcode from owhs with(nolock) where whscode = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getCardcode: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
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
        sName = Replace(sName, ",", sChr)
        NewStr = sName
        Return NewStr

    End Function
End Class

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports SAPbobsCOM

Partial Class TrasladosCargaMacro
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String
    Private Sub TrasladosCargaMacro_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    CargarSucursales()

                    If Not String.IsNullOrEmpty(Request.QueryString("almacen")) Then
                        Dim Suc As String = Request.QueryString("almacen")
                        drpSucursales.SelectedValue = Suc
                        btnCargarSugerido_Click(btnCargarSugerido, New EventArgs())
                        Response.Write("<script>console.log('Almacen Presolicitud " & Suc & "');</script>")
                        Response.Write("<script>console.log('Carga Presolicitud " & ID & "');</script>")
                    End If
                    If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                        ' El parámetro "id" existe y no está vacío
                        Dim id As String = Request.QueryString("id")
                        Dim Suc As String = Replace(Request.QueryString("destino"), "T", "")
                        InsertFromQueryToMacroInsert(Request.QueryString("id"))
                        drpSucursales.SelectedValue = Suc
                        btnCargarSugerido_Click(btnCargarSugerido, New EventArgs())
                        Response.Write("<script>console.log('Almacen Presolicitud " & Suc & "');</script>")
                        Response.Write("<script>console.log('Carga Presolicitud " & id & "');</script>")
                    Else
                        Response.Write("<script>console.log('Carga Directa');</script>")
                    End If

                    If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                        lblRutaPlan.Text = Request.QueryString("planId")
                        lblAlmacen.Text = Request.QueryString("almacen")
                        lblPlanId.Text = Request.QueryString("ruta")

                    End If
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('TrasladosCargaMacro_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub CargarSucursales()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = " SELECT WhsName,WhsCode FROM OWHS WITH(NOLOCK) WHERE U_TYPE='PRO' AND WhsCode NOT LIKE 'T%'   " &
                                                        " AND WhsCode NOT IN ('BPD01','CDM00','CDR00','CDR01','CONCA01','Consumir','DCM00','DCR00',   " &
                                                        " 'DCR01','DEV01','DEV02','EST001','FCE01','MAY01','MLCB01','MLCB02','MOB01','MSPS01','MSPS02',   " &
                                                        " 'MSPS03','MSPS04','MSUC01','MSUC02','MSUC03','MSUC04','MSUC05','MSUC07','MTGA01','MTRANS00',   " &
                                                        " 'RLCB02','RLCB03','RSPS02','RSPS03','RSPS04','RSPS05','RSPS06','RSPS07','RTGA01',   " &
                                                        " 'RTGA0102','SUC0102','SUC0201','SUC03','SUC0301','SUC0401','SUC0701','SUC0801','SUM001','DIU-10',  " &
                                                        " 'GMG122','mym 09','con','CON 334','CON349') "

                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using
            drpSucursales.Dispose()
            drpSucursales.DataTextField = "WhsName"
            drpSucursales.DataValueField = "WhsCode"
            drpSucursales.DataSource = dt
            drpSucursales.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('CargarSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Shared Function GetData(query As String) As DataTable
        Dim strConnString As String = sCon2
        Using con As New SqlConnection(strConnString)
            Using cmd As New SqlCommand()
                cmd.CommandText = query
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using ds As New DataSet()
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
            con.Close()
        End Using
    End Function
    Public Sub UpdateLineState(rowid As String, cantidad As String)
        Try
            Dim query As String = String.Empty
            query &= "update [MACROINSERT] set qtylogistica=@p2 where id=@p1"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", rowid)
                        .Parameters.AddWithValue("@p2", cantidad)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
            Response.Write("<script>console.log('Linea Actualizada" & rowid & "');</script>")

        Catch ex As Exception
            Response.Write("<script>console.log('UpdateLineState: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub EliminarPresolicitudOrigen(headerid As String)
        Try
            Dim query As String = String.Empty
            query &= "delete from PRESOLICITUD_HEADER where id=@p1; delete from PRESOLICITUD_LINES where HEADERID=@p1;"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", headerid)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
            Response.Write("<script>console.log('Documento de Origen Eliminado " & headerid & "');</script>")

        Catch ex As Exception
            Response.Write("<script>console.log('EliminarPresolicitudOrigen: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnCargarSugerido_Click(sender As Object, e As EventArgs) Handles btnCargarSugerido.Click
        Try
            If Session("Name") Is vbNullString Then
                Response.Redirect("Default.aspx")
            Else

                BindGrid(drpSucursales.SelectedValue.ToString)
                BuscarDatosAlmacenDestino(drpSucursales.SelectedValue.ToString)
                BindgridSugerido(drpSucursales.SelectedValue.ToString)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('btnCargarSugerido_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub BuscarDatosAlmacenDestino(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = " select t0.WhsCode,t0.WhsName,t0.U_CardCode,t1.Name [Ruta], t2.Name [Responsable] " &
                                        " from OWHS t0 With(nolock) full join [@CRUTA] t1 With(nolock) On t0.U_Ruta=t1.Code " &
                                        " full join [@CRESPONSABLEOWHS] t2 with(nolock) on t0.U_Categorizacion=t2.Code " &
                                        " where whscode ='" & whsname & "' "
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtRuta.Text = drd.Item("Ruta").ToString
                txtCodigoCliente.Text = drd.Item("U_CardCode").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('BuscarDatosAlmacenDestino: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindgridColoresMoto(MODELO As String, WHSCODE As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("select	t0.itemcode, t0.itemname, t1.Name [Modelo], t0.u_columna [Espacios] " &
                        " , (select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode='DCM00' AND ItemCode=T0.ItemCode)[CEDIS] " &
                        " ,(select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode = @whscode AND ItemCode=T0.ItemCode)[SUCURSAL] " &
                        " From movesa..oitm t0 with(nolock)  inner Join movesa..[@AMODELO] t1 with(nolock) On t0.U_AMODELO=t1.Code " &
                        " where t1.Name = @modelo and ItmsGrpCod=154 and t0.frozenFor='N' and t0.onhand>0")
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
    Private Sub BindgridSugerido(WHSCODE As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SqlQry As String = "SELECT ID,ARTICULO,MODELO,DESCRIPCION,ESPACIOS,CANTIDAD,QTYLOGISTICA, " &
                                            " OBSERVACIONES FROM ArmadoMotos.dbo.MACROINSERT " &
                                            " WHERE [ESTADO]='T' AND ALMDESTINO = @almDestino"
                Using cmd As New SqlCommand(SqlQry)
                    cmd.Parameters.AddWithValue("@almDestino", WHSCODE)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPedidoTemp.DataSource = dt
                            gridPedidoTemp.DataBind()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridPedidoTemp.UseAccessibleHeader = True
            gridPedidoTemp.HeaderRow.TableSection = TableRowSection.TableHeader

            Dim espacios As Integer = 0
            For Each row As GridViewRow In gridPedidoTemp.Rows
                espacios = espacios + CInt(row.Cells(4).Text)
            Next
            txtEspaciosAsignados.Text = espacios

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGrid(whscode As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String

                sql_string = "EXEC [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS] @whscode "
                Using cmd As New SqlCommand(sql_string)
                    cmd.Parameters.AddWithValue("@whscode", whscode)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCuadroBasico.DataSource = dt
                            gridCuadroBasico.DataBind()
                        End Using
                    End Using
                    con.Close()
                End Using
            End Using
            gridCuadroBasico.UseAccessibleHeader = True
            gridCuadroBasico.HeaderRow.TableSection = TableRowSection.TableHeader

            For Each row As GridViewRow In gridCuadroBasico.Rows
                Dim lblModelo As Label = TryCast(row.FindControl("lblModelo"), Label)

                If GetOrdenamiento(lblModelo.Text.ToString.Trim) = "True" Then
                    row.Cells(0).Text = 0
                    row.Cells(0).BackColor = System.Drawing.Color.GreenYellow
                End If
            Next

            For Each row As GridViewRow In gridCuadroBasico.Rows

                row.Cells(16).Text = row.Cells(16).Text - row.Cells(11).Text

                row.Cells(21).Text = CInt(row.Cells(17).Text) + CInt(row.Cells(18).Text) + CInt(row.Cells(19).Text + CInt(row.Cells(20).Text))

                If row.Cells(16).Text > 0 Then
                    row.Cells(16).BackColor = System.Drawing.Color.Orange

                ElseIf row.Cells(16).Text < 0 Then
                    row.Cells(16).ToolTip = "Sugerido Mayor al Cuadro Basico!!"
                    row.Cells(16).BackColor = System.Drawing.Color.Red
                End If

                If row.Cells(16).Text > row.Cells(10).Text Then
                    row.Cells(10).ToolTip = "Cuadro Basico Menor a la Venta Actual!!"
                    row.Cells(10).BackColor = System.Drawing.Color.Red
                    row.Cells(16).BackColor = System.Drawing.Color.Red
                End If
                If row.Cells(23).Text > 0 Then
                    row.Cells(23).BackColor = System.Drawing.Color.LightGreen
                End If
            Next

        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridCuadroBasico_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCuadroBasico.RowCommand
        Try
            If e.CommandName = "BI" Then

                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCuadroBasico.Rows(index - 1)
                Dim lblModelo As Label = CType(gvRow.FindControl("lblModelo"), Label)
                Dim modelo As String = ""
                If lblModelo IsNot Nothing Then
                    modelo = lblModelo.Text.Trim()
                End If
                BindGrid(drpSucursales.SelectedValue.ToString())
                BindgridColoresMoto(modelo, drpSucursales.SelectedValue.ToString())

                Dim url As String = "TrasladosCargaMacroBI.aspx?modelo=" & modelo
                Dim script As String = "window.open('" & url & "', '_blank');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenNewWindow", script, True)
            End If

            If e.CommandName = "Sugerir" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCuadroBasico.Rows(index)
                Dim lblModelo As Label = CType(gvRow.FindControl("lblModelo"), Label)
                Dim modelo As String = ""

                lblModeMoto.Text = lblModelo.Text.Trim()
                lblModeloCode.Text = gridCuadroBasico.Rows(index).Cells(1).Text


                If lblModelo IsNot Nothing Then
                    modelo = lblModelo.Text.Trim()
                End If

                BindGrid(drpSucursales.SelectedValue.ToString())
                BindgridColoresMoto(modelo, drpSucursales.SelectedValue.ToString())
                lblruta.Text = txtRuta.Text
                lblcardcode.Text = txtCodigoCliente.Text
                lblAlmDestino.Text = drpSucursales.SelectedValue.ToString()

                lblCB.Text = gridCuadroBasico.Rows(index).Cells(10).Text
                lblFISICO.Text = gridCuadroBasico.Rows(index).Cells(14).Text
                lblFALTANTE.Text = gridCuadroBasico.Rows(index).Cells(15).Text
                lblVTAA.Text = gridCuadroBasico.Rows(index).Cells(16).Text
                Dim script As String = "<script>" &
                           "$('#ColoresMotos').flyout('toggle'); " &
                           "</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", script, False)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridCuadroBasico_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub btnCancelColoresMotos_Click(sender As Object, e As EventArgs) Handles btnCancelColoresMotos.Click
        BindGrid(drpSucursales.SelectedValue.ToString)
        BindgridSugerido(drpSucursales.SelectedValue.ToString)
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
            BindGrid(drpSucursales.SelectedValue.ToString)
            BindgridSugerido(drpSucursales.SelectedValue.ToString)
            Dim script As String = "<script>iziToast.success({title: 'OK!', message: 'Moto Agregada Con Exito!!!',position: 'topRight',timeout: 10000})</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)

        Catch ex As Exception
            Response.Write("<script>console.log('btnModalColoresMotos_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub InsertFromQueryToMacroInsert(headerId As Integer)
        Try
            Dim sCon As String = sCon2
            Dim SQL_Query As String = "SELECT t1.RUTA, t1.ALMORIGEN, REPLACE(T1.ALMDESTINO,'T','')[ALMDESTINO], t0.CODCLIALMDESTINO, t1.ARTICULO, t1.MODELO, t1.DESCRIPCION, " &
                                  "t1.ESPACIOS, t1.CANTIDAD, t1.CANTIDAD, t1.CANTIDAD, T0.OBSERACIONES, 'T' AS ESTADO, " &
                                  "'CD' AS TIPO, GETDATE() AS FECHA, T0.USERCREATED, 0 AS CB, 0 AS FISICO, 0 AS FALTANTE, 0 AS VTAA " &
                                  "FROM PRESOLICITUD_HEADER t0 " &
                                  "INNER JOIN PRESOLICITUD_LINES t1 ON t0.ID = t1.HEADERID " &
                                  "WHERE T0.ID = @HeaderID and t1.CANTIDAD > 0"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(SQL_Query, con)
                cmd.Parameters.AddWithValue("@HeaderID", headerId)

                con.Open()

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim RUTA As String = reader("RUTA").ToString()
                        Dim ALMORIGEN As String = reader("ALMORIGEN").ToString()
                        Dim ALMDESTINO As String = reader("ALMDESTINO").ToString()
                        Dim CARDCODE As String = reader("CODCLIALMDESTINO").ToString()
                        Dim ARTICULO As String = reader("ARTICULO").ToString()
                        Dim MODELO As String = reader("MODELO").ToString()
                        Dim DESCRIPCION As String = reader("DESCRIPCION").ToString()
                        Dim ESPACIOS As String = reader("ESPACIOS").ToString()
                        Dim CANTIDAD As String = reader("CANTIDAD").ToString()
                        Dim QTYLOGISTICA As String = reader("CANTIDAD").ToString()
                        Dim QTYSUCURSAL As String = reader("CANTIDAD").ToString()
                        Dim OBSERVACIONES As String = reader("OBSERACIONES").ToString()
                        Dim FECHA As Date = DateTime.Now
                        Dim USUARIO As String = reader("USERCREATED").ToString()
                        Dim CB As String = "0"
                        Dim FISICO As String = "0"
                        Dim FALTANTE As String = "0"
                        Dim VTAA As String = "0"

                        InsertSugeridoSucursalesFromQuery(RUTA, ALMORIGEN, ALMDESTINO, CARDCODE, ARTICULO, MODELO, DESCRIPCION,
                                             ESPACIOS, CANTIDAD, QTYLOGISTICA, QTYSUCURSAL, OBSERVACIONES & " SEGUN PRESOLICITUD: " & headerId,
                                             FECHA, USUARIO, CB, FISICO, FALTANTE, VTAA)
                    End While
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('InsertFromQueryToMacroInsert: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
    Public Function InsertSugeridoSucursalesFromQuery(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String, ARTICULO As String,
                                                MODELO As String, DESCRIPCION As String, ESPACIOS As String,
                                                CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                                OBSERVACIONES As String, FECHA As Date, USUARIO As String,
                                                CB As String, FISICO As String, FALTANTE As String, VTAA As String) As Integer
        Try

            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim SQL_QRY As String = "IF EXISTS (SELECT 1 FROM [MACROINSERT] WHERE [ALMDESTINO] = @p3 " &
                                    " And [ARTICULO] = @p5 And isnull(HEADERID,0)=0 and isnull([ESTADO],'-')='T')" &
                    " BEGIN " &
                    " DELETE FROM [MACROINSERT] WHERE [ALMDESTINO] = @p3 And [ARTICULO] = @p5 And isnull(HEADERID,0)=0 and isnull([ESTADO],'-')='T';" &
                     " INSERT INTO [dbo].[MACROINSERT] " &
                    " ([RUTA], [ALMORIGEN], [ALMDESTINO], [CARDCODE], [ARTICULO], [MODELO], [DESCRIPCION], [ESPACIOS],  " &
                    " [CANTIDAD], [QTYLOGISTICA], [QTYSUCURSAL], [OBSERVACIONES], [ESTADO], [TIPO], [FECHA],  " &
                    " [USUARIO], [CB], [FISICO], [FALTANTE], [VTAA])  " &
                    " VALUES  " &
                    " (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20); " &
                    " SELECT SCOPE_IDENTITY() AS InsertedID; " &
                    " END " &
                    " ELSE " &
                    " BEGIN " &
                    " INSERT INTO [dbo].[MACROINSERT] " &
                    " ([RUTA], [ALMORIGEN], [ALMDESTINO], [CARDCODE], [ARTICULO], [MODELO], [DESCRIPCION], [ESPACIOS],  " &
                    " [CANTIDAD], [QTYLOGISTICA], [QTYSUCURSAL], [OBSERVACIONES], [ESTADO], [TIPO], [FECHA],  " &
                    " [USUARIO], [CB], [FISICO], [FALTANTE], [VTAA])  " &
                    " VALUES  " &
                    " (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20); " &
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

                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Sub EliminarLinea(idlinea As Integer)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "DELETE FROM [MACROINSERT] where [ID] = @p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Moto Eliminada con Exito!<hr> EliminarLinea');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('EliminarLinea: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridPedidoTemp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPedidoTemp.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridPedidoTemp.Rows(index)
                EliminarLinea(gridPedidoTemp.Rows(index).Cells(0).Text)
                BindGrid(drpSucursales.SelectedValue.ToString)
                BindgridSugerido(drpSucursales.SelectedValue.ToString)
                Dim script As String = "<script>iziToast.warning({title: 'OK!', message: 'Moto Eliminada Con Exito!!!',position: 'topRight',timeout: 10000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridPedidoTemp_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub


    Public Sub EnviarNotificacionEmail()
        Try
            Dim server As New SmtpClient
            Dim mensaje As New MailMessage
            server.Host = "mail.grupomovesa.com"
            server.Port = "587"
            server.Credentials = New System.Net.NetworkCredential("notificaciones@grupomovesa.com", "VYgd}T!XcVy}")
            mensaje.From = New MailAddress("notificaciones@grupomovesa.com")

            mensaje.To.Add(GetEmailSuc(drpSucursales.SelectedValue.ToString.Trim))
            mensaje.CC.Add("nromero@grupomovesa.com")
            mensaje.CC.Add("logistica05@grupomovesa.com")
            mensaje.CC.Add("pbustillo@grupomovesa.com")
            mensaje.CC.Add("analistainventario@grupomovesa.com")
            mensaje.CC.Add("rjovel@grupomovesa.com")
            mensaje.Subject = "MOVESA - Portal Produccion de Motos"
            mensaje.Body = Session("Name") & ", Creado un Nuevo Sugerido de Motos" &
                                "<BR /> " &
                                "PORTAL Produccion de Motos By RJ"

            mensaje.IsBodyHtml = True
            mensaje.Priority = MailPriority.High
            server.Send(mensaje)
        Catch ex As Exception
            'lblError.Text = ex.Message
        End Try
    End Sub
    Public Function GetEmailSuc(whscode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT [USEREMAIL] FROM [ArmadoMotos].[dbo].[USUARIOS] where SUCURSAL=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEmailSuc: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
    Public Function GetOrdenamiento(modelo As String) As String
        Dim sCon As String = sCon2
        Dim sel As String

        sel = " IF EXISTS (SELECT 1 FROM ORDENAMIENTO WHERE MODELO = @p1 and ACTIVOCD=1) " &
              "       SELECT 'True'; " &
              "  ELSE " &
              "      SELECT 'False';"

        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", modelo)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function
    Private Sub ExportarDataTableACSV(dt As DataTable, filePath As String)
        ' Crear el archivo CSV
        Using writer As New StreamWriter(filePath)
            ' Escribir las cabeceras de las columnas
            For Each column As DataColumn In dt.Columns
                writer.Write(column.ColumnName)
                writer.Write(",")
            Next
            writer.WriteLine()

            ' Escribir los datos de cada fila
            For Each row As DataRow In dt.Rows
                For i As Integer = 0 To dt.Columns.Count - 1
                    writer.Write(EscapeCSVValue(row(i).ToString()))
                    writer.Write(",")
                Next
                writer.WriteLine()
            Next
        End Using
    End Sub
    Private Function EscapeCSVValue(value As String) As String
        ' Verificar si el valor contiene comas o comillas dobles
        If value.Contains(",") OrElse value.Contains("""") Then
            ' Si contiene comas o comillas dobles, encerrar el valor entre comillas dobles y duplicar las comillas dobles internas
            Return """" & value.Replace("""", """""") & """"
        Else
            ' Si no contiene comas ni comillas dobles, devolver el valor sin modificar
            Return value
        End If
    End Function
    Private Sub btnNotificar_Click(sender As Object, e As EventArgs) Handles btnNotificar.Click
        Try
            If Session("Name") Is vbNullString Then
                Response.Redirect("Default.aspx")
            End If

            ' Verificar si los campos están vacíos
            If String.IsNullOrWhiteSpace(txtFechaArmado.Text) Or String.IsNullOrWhiteSpace(txtFechaDespacho.Text) Then
                Dim scriptFechas As String = "<script>iziToast.error({title: 'Error!', message: 'Faltas Fechas de Armado o Despacho, Por Favor Revise!!!',timeout: 10000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", scriptFechas, False)
                Exit Sub
            End If

            Threading.Thread.Sleep(5000)
            Dim header_macroinsert As Integer
            header_macroinsert = MacroInsert_Header("DCM00", drpSucursales.SelectedValue.ToString, txtRuta.Text, "1", Date.Now,
                                                    Session("UserCode"), txtFechaArmado.Text, txtFechaDespacho.Text)

            If header_macroinsert <> 0 Then
                UpdateHeaderId(header_macroinsert, drpSucursales.SelectedValue.ToString, txtFechaArmado.Text, txtFechaDespacho.Text)

                If Session("User") <> "RJOVEL" Then
                    Dim server As New SmtpClient
                    Dim mensaje As New MailMessage
                    server.Host = "mail.grupomovesa.com"
                    server.Port = "587"
                    server.Credentials = New System.Net.NetworkCredential("notificaciones@grupomovesa.com", "VYgd}T!XcVy}")
                    mensaje.From = New MailAddress("notificaciones@grupomovesa.com")

                    Dim EMAIL As String = GetEmailSuc(drpSucursales.SelectedValue.ToString.Trim)
                    Dim constr As String = sCon2
                    Using con As New SqlConnection(constr)
                        Using cmd As New SqlCommand("SELECT [ALMDESTINO],[ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS]," &
                                                        " [QTYLOGISTICA],[OBSERVACIONES] FROM [ArmadoMotos].[dbo].[MACROINSERT] WHERE [ALMDESTINO]='" & drpSucursales.SelectedValue.ToString & "' " &
                                                        " AND HEADERID=" & header_macroinsert & "")

                            Using sda As New SqlDataAdapter()
                                cmd.Connection = con
                                sda.SelectCommand = cmd
                                Using dt As New DataTable()
                                    sda.Fill(dt)
                                    Dim pdfBytes As Byte() = ConvertDataTableToPdf(dt)
                                    Dim pdfStream As New MemoryStream(pdfBytes)
                                    mensaje.Attachments.Add(New Net.Mail.Attachment(pdfStream, drpSucursales.SelectedValue.ToString & ".pdf"))
                                    con.Close()
                                End Using
                            End Using
                        End Using
                    End Using

                    mensaje.To.Add(EMAIL)
                    mensaje.CC.Add("nromero@grupomovesa.com")
                    mensaje.CC.Add("logistica05@grupomovesa.com")
                    mensaje.CC.Add("pbustillo@grupomovesa.com")
                    mensaje.CC.Add("analistainventario@grupomovesa.com")
                    mensaje.CC.Add("rjovel@grupomovesa.com")
                    mensaje.Subject = "Sugerido Motos #" & header_macroinsert & " Almacen - " & drpSucursales.SelectedValue.ToString & " MOVESA - Portal Produccion de Motos"
                    mensaje.Body = "" & Session("Name") & ", Ha Creado un Nuevo Sugerido de Motos" & "<BR /> " &
                                        "Fecha Propuesta de Armado " & txtFechaArmado.Text & "<BR /> " &
                                        "Fecha Propuesta de Despacho " & txtFechaDespacho.Text & "<BR /> " &
                                        "<BR /> " &
                                        "PORTAL Produccion de Motos By RJ"
                    mensaje.IsBodyHtml = True
                    mensaje.Priority = MailPriority.High
                    server.Send(mensaje)

                    Response.Write("<script>console.log('Correo Enviado');</script>")
                End If

                If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                    EliminarPresolicitudOrigen(Request.QueryString("id"))
                End If

                If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                    Response.Redirect("TrasladosDespachos.aspx?planid=" & Request.QueryString("planId"))
                End If

                BindGrid(drpSucursales.SelectedValue.ToString)
                BindgridSugerido(drpSucursales.SelectedValue.ToString)
                lblDocumento.Text = header_macroinsert
                Dim Flyscript As String = "<script>$('.ui.basic.modal').modal('show');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", Flyscript, False)
            Else
                Dim script As String = "<script>iziToast.error({title: 'Error!', message: 'Ocurrio un Error, Consulte con Sistemas!',timeout: 20000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", script, False)
                Exit Sub
            End If

        Catch ex As Exception
            Response.Write("<script>console.log('Exception " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function MacroInsert_Header(ORIGEN As String, DESTINO As String, RUTA As String, ESTADO As String,
                                       DATECREATED As DateTime, USERCREATED As String,
                                       FARMADO As Date, FDESPACHO As Date) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            Dim planid As Integer = 0

            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planid = Request.QueryString("planId")
            End If


            sel = "INSERT INTO [dbo].[MACROINSERT_HEADER]([ORIGEN],[DESTINO],[RUTA],[ESTADO],[DATECREATED],[USERCREATED], [FARMADO], [FDESPACHO],[ESTATUS],[PLANID])" &
                    " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10); SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ORIGEN)
                cmd.Parameters.AddWithValue("@p2", DESTINO)
                cmd.Parameters.AddWithValue("@p3", RUTA)
                cmd.Parameters.AddWithValue("@p4", ESTADO)
                cmd.Parameters.AddWithValue("@p5", DATECREATED)
                cmd.Parameters.AddWithValue("@p6", USERCREATED)
                cmd.Parameters.AddWithValue("@p7", FARMADO)
                cmd.Parameters.AddWithValue("@p8", FDESPACHO)
                cmd.Parameters.AddWithValue("@p9", "A")
                cmd.Parameters.AddWithValue("@p10", planid)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Return 0
            Response.Write("<script>console.log('MacroInsert_Header: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Sub UpdateHeaderId(headerid As String, almdestino As String, farmado As Date, fdespacho As Date)
        Try
            Dim query As String = String.Empty
            query &= "UPDATE [ArmadoMotos].[dbo].[MACROINSERT] SET [HEADERID]=@P1,[ESTADO]='S', [FARMADO] = @p3, [FDESPACHO] = @p4 " &
                        " where [ESTADO]='T' and [ALMDESTINO]=@P2"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", headerid)
                        .Parameters.AddWithValue("@p2", almdestino)
                        .Parameters.AddWithValue("@p3", farmado)
                        .Parameters.AddWithValue("@p4", fdespacho)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('UpdateHeaderId: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function ConvertDataTableToPdf(dataTable As DataTable) As Byte()
        Dim document As New Document(PageSize.LETTER.Rotate())
        Dim memoryStream As New MemoryStream()
        Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)
        document.Open()

        Dim table As New PdfPTable(dataTable.Columns.Count)
        table.WidthPercentage = 100
        Dim font As New Font(Font.FontFamily.HELVETICA, 8)
        Dim cellStyle As New PdfPCell()
        cellStyle.HorizontalAlignment = Element.ALIGN_LEFT
        cellStyle.VerticalAlignment = Element.ALIGN_MIDDLE
        cellStyle.Padding = 5

        For i As Integer = 0 To dataTable.Columns.Count - 1
            Dim phrase As New Phrase(dataTable.Columns(i).ColumnName, font)
            Dim headerCell As New PdfPCell(phrase)
            headerCell.BackgroundColor = New BaseColor(230, 230, 230)
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE
            headerCell.Padding = 5
            table.AddCell(headerCell)
        Next

        For i As Integer = 0 To dataTable.Rows.Count - 1
            For j As Integer = 0 To dataTable.Columns.Count - 1
                Dim phrase As New Phrase(dataTable.Rows(i)(j).ToString(), font)
                Dim dataCell As New PdfPCell(phrase)
                dataCell.HorizontalAlignment = Element.ALIGN_LEFT
                dataCell.VerticalAlignment = Element.ALIGN_MIDDLE
                dataCell.Padding = 5
                table.AddCell(dataCell)
            Next
        Next

        document.Add(table)
        document.Close()

        Dim pdfBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()

        Return pdfBytes
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

    Private Sub btnEliminarPedidoTemp_Click(sender As Object, e As EventArgs) Handles btnEliminarPedidoTemp.Click
        Try
            For Each row As GridViewRow In gridPedidoTemp.Rows
                EliminarLinea(row.Cells(0).Text)
            Next
            BindGrid(drpSucursales.SelectedValue.ToString)
            BindgridSugerido(drpSucursales.SelectedValue.ToString)
        Catch ex As Exception
            Response.Write("<script>console.log('btnEliminarPedidoTemp_Click " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
End Class


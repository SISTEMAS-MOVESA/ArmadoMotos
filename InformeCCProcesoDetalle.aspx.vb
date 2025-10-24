Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Partial Class InformeCCProcesoDetalle
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim SQL_string As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/InformeCC_BindGrid_En_Proceso_Detalle.sql"))
                Using cmd As New SqlCommand(SQL_string, con)

                    Using sda As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        GridView1.DataSource = dt
                        GridView1.DataBind()
                    End Using
                End Using
                con.Close()
            End Using
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            'Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub

    Private Sub InformeCCProcesoDetalle_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    BindGrid()
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Select Case e.Row.Cells(7).Text
                    Case "Pintura"
                        e.Row.Cells(0).Enabled = True
                        e.Row.Cells(12).Enabled = False
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Red
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                    Case "Pintura en Proceso"
                        e.Row.Cells(0).Enabled = False
                        e.Row.Cells(12).Enabled = True
                        e.Row.Cells(7).BackColor = System.Drawing.Color.Orange
                        e.Row.Cells(7).ForeColor = System.Drawing.Color.White
                End Select
            End If
        Catch ex As Exception
            Response.Write("GridView1_RowDataBound " & ex.Message)
        End Try
    End Sub
    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            If e.CommandName = "Iniciar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)
                txtFlyMiniSerial.Text = GridView1.Rows(index).Cells(2).Text
                BindGrid()

                Dim Flyscript As String = "<script>$('#IniciarPintura').flyout('toggle');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", Flyscript, False)


                Dim script As String = "iziToast.warning({" &
                                       "title: 'Adevrtencia'," &
                                       "message: 'Seleccione Pintor'," &
                                       "position: 'topRight'," &
                                       "timeout: 5000" &
                                       "});"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowToast", script, True)

            End If
            If e.CommandName = "Terminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = GridView1.Rows(index)

                UpdateFinPintura(Date.Now, Session("Name").ToString, GridView1.Rows(index).Cells(2).Text)

                'BuscarSerie(GridView1.Rows(index).Cells(2).Text)
                BindGrid()
                'Dim script As String = "iziToast.warning({" &
                '                       "title: 'Adevrtencia'," &
                '                       "message: 'Esta Funcion aun no esta Lista'," &
                '                       "position: 'topRight'," &
                '                       "timeout: 5000" &
                '                       "});"
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowToast", script, True)

                'Dim Flyscript As String = "<script>$('#UbicarMoto').flyout('toggle');</script>"
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", Flyscript, False)

            End If

        Catch ex As Exception
            Response.Write("GridView1_RowCommand " & ex.Message)
        End Try
    End Sub
    Public Sub BuscarSerie(_SERIE As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = " SELECT	T0.[ItemCode],T0.[ItemName],T1.[SuppSerial],T1.[IntrSerial],t1.BatchId, " &
                                        " T5.Name [Marca],T4.NAME [Cilindros], T3.Name [Motor],T2.Name [Modelo],T1.[WhsCode],T6.Name [Color], " &
                                        " (SELECT top 1 Owhs.[Whsname] FROM Owhs with(nolock) WHERE Owhs.[whscode]=T1.[WhsCode] ) [Almacen] " &
                                        " ,isnull((select top 1 datediff(day, DATECREATED, getdate()) from ArmadoMotos..ARMADOMOTOS where SERIE=@p1),0) [DiasArmado] " &
                                        " ,isnull((SELECT top 1 datediff(day,t2.DocDate,getdate()) FROM OSRN T0 With(nolock) inner join ITL1 T1 with(nolock) " &
                                        " On T1.SysNumber=T0.SysNumber And T1.ItemCode = T0.ItemCode inner join " &
                                        " OITL T2 with(nolock) on T1.LogEntry = T2.LogEntry where t0.MnfSerial =@p1  And t2.DocType=20),0) [Dias] " &
                                        " ,(select whscode from movesa..osri where status=0 and suppserial=@p1) + ' ' + (select whsname from movesa..owhs with(nolock) where  status=0 and whscode=(select whscode from movesa..osri where  status=0 and suppserial=@p1)) [AlmacenSerie] " &
                                        " ,(select CASE Status WHEN 0 then 'Disponible' ELSE 'No Disponible' END from movesa..osri where  status=0 and suppserial=@p1)[EstatusSAP] " &
                                        " FROM OITM T0 With(nolock) INNER JOIN OSRI T1 With(nolock) On T0.ItemCode = T1.ItemCode " &
                                        " FULL JOIN [@AMODELO] T2 With(NOLOCK) On T0.U_AMODELO=T2.CODE  " &
                                        " FULL JOIN [@AMOTOR] T3 With(NOLOCK) On T0.U_AMOTOR=T3.Code " &
                                        " FULL JOIN [@ACILINDROS] T4 With(NOLOCK) On T0.U_ACILINDROS=T4.Code " &
                                        " FULL JOIN [@AMARCA] T5 With(NOLOCK) On T0.U_AMARCA=T5.CODE " &
                                        " INNER JOIN [@SCOLOR] T6 With(NOLOCK) On T0.U_ACOLOR=T6.CODE " &
                                        " WHERE T1.[SuppSerial]= @p1 and T1.itemcode<>'PLA001'"
            Dim Comando As New SqlCommand(Consulta, _con)
            Comando.Parameters.AddWithValue("@p1", _SERIE)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtFlyItemcode.Text = drd.Item("ItemCode").ToString
                txtFlyItemName.Text = drd.Item("ItemName").ToString
                txtFlyModelo.Text = drd.Item("Modelo").ToString
                txtFlyMarca.Text = drd.Item("Marca").ToString
                txtFlyCilindros.Text = drd.Item("Cilindros").ToString
                txtFlySerie.Text = drd.Item("SuppSerial").ToString
                txtFlySerieMotor.Text = drd.Item("IntrSerial").ToString
                txtFlyColor.Text = drd.Item("Color").ToString
                txtFlyYYYY.Text = drd.Item("BatchId").ToString
                txtFlyDiasArmado.Text = drd.Item("DiasArmado").ToString
                txtFlyDiasIngreso.Text = drd.Item("Dias").ToString
                txtFlyAlmacen.Text = drd.Item("AlmacenSerie").ToString
                txtFlyEstatusSerieSAP.Text = drd.Item("EstatusSAP").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub UpdateInicioPintura(_fecha As Date, _Pintor As String, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS]  SET [ESTATUS]='Pintura en Proceso', [DATEINICIODETALLE] = @p1, [PINTOR] = @p2 WHERE [SERIE] = @p3"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _fecha)
                cmd.Parameters.AddWithValue("@p2", _Pintor)
                cmd.Parameters.AddWithValue("@p3", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using

            BindGrid()
            Dim script As String = "iziToast.success({" &
                                   "title: 'OK!'," &
                                   "message: 'Inicio Pintura Exitoso!!!'," &
                                   "position: 'topRight'," &
                                   "timeout: 5000" &
                                   "});"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowToast", script, True)

        Catch ex As Exception
            Response.Write("UpdateInicioPintura " & ex.Message)
        End Try
    End Sub
    Public Sub UpdateFinPintura(_fecha As Date, _usuario As String, _Serie As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "UPDATE [dbo].[ARMADOMOTOS]  SET [ESTATUS]='Armada', [DATEFINDETALLE] = @p1, CONTROLDETALLE = @p2 WHERE [SERIE] = @p3"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _fecha)
                cmd.Parameters.AddWithValue("@p2", _usuario)
                cmd.Parameters.AddWithValue("@p3", _Serie)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using

            BindGrid()
            Dim script As String = "iziToast.success({" &
                                   "title: 'OK!'," &
                                   "message: 'Fin de Pintura Exitoso!!!'," &
                                   "position: 'topRight'," &
                                   "timeout: 5000" &
                                   "});"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowToast", script, True)

        Catch ex As Exception
            Response.Write("UpdateInicioPintura " & ex.Message)
        End Try
    End Sub

    Private Sub btnFlyActualizarUbicacion_Click(sender As Object, e As EventArgs) Handles btnFlyActualizarUbicacion.Click
        Try
            CREATEARMADO(drpEstadoVeh.SelectedValue.ToString,
                            drpOrigenDestino.SelectedValue.ToString,
                            drpPasillo.SelectedValue.ToString,
                            drpSegmento.SelectedValue.ToString,
                            txtFlyItemcode.Text,
                            txtFlyItemName.Text,
                            txtFlySerie.Text,
                            txtFlySerieMotor.Text,
                            txtFlyMarca.Text,
                            txtFlyModelo.Text,
                            txtFlyCilindros.Text,
                            txtFlyColor.Text,
                            txtFlyYYYY.Text,
                            txtFlyDiasArmado.Text,
                            txtFlyDiasIngreso.Text,
                            txtFlyObservaciones.Text,
                            Session("Name").ToString,
                            txtFlyEstatusSerieSAP.Text,
                            txtFlyAlmacen.Text)

            UpdateFinPintura(Date.Now, Session("Name").ToString, txtFlySerie.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub CREATEARMADO(ESTADOVEH As String, ORIGENDESTINO As String, PASILLO As String, SEGMENTO As String, CODIGO As String,
                  DESCRIPCION As String, SERIEMOTO As String, SERIEMOTOR As String, MARCA As String, MODELO As String,
                  CILINDROS As String, COLOR As String, YEAR As String, DIASARMADO As Integer, DIASINGRESO As Integer,
                  OBSERVACIONES As String, USUARIO As String, ESTADOSAP As String, ALMACENSAP As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            Dim exists As Boolean = False

            Using con As New SqlConnection(sCon)
                Dim checkQuery As String = "SELECT COUNT(1) FROM dbo.InventarioVehiculos WHERE SerieMoto = @SERIE"
                Dim checkCmd As New SqlCommand(checkQuery, con)
                checkCmd.Parameters.AddWithValue("@SERIE", SERIEMOTO)
                con.Open()
                exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0
                con.Close()
            End Using

            If exists Then
                sel = "UPDATE dbo.InventarioVehiculos SET EstadoVeh = @P1, OrigenDestino = @P2, Pasillo = @P3, Segmento = @P4, " &
              " Codigo = @P5, Descripcion = @P6, SerieMotor = @P8, Marca = @P9, Modelo = @P10, Cilindros = @P11, " &
              " Color = @P12, YYYY = @P13, DiasArmado = @P14, DiasIngreso = @P15, Observaciones = @P16, Usuario = @P17, " &
              " LIMPIEZA = @P19, AUDITORIA = @P20, PINTURA = @P21, ESTADOSAP = @P22, ALMACENSAP = @P23, ESTADOLOGISTICA = 'ACTIVO' " &
              " WHERE SerieMoto = @P7"
            Else
                sel = "INSERT INTO dbo.InventarioVehiculos (EstadoVeh, OrigenDestino, Pasillo, Segmento, Codigo, Descripcion, SerieMoto, " &
              " SerieMotor, Marca, Modelo, Cilindros, Color, YYYY, DiasArmado, DiasIngreso, Observaciones, Usuario, DateCreated, " &
              " LIMPIEZA, AUDITORIA, PINTURA, ESTADOSAP, ALMACENSAP, ESTADOLOGISTICA) " &
              " VALUES (@P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10, @P11, @P12, @P13, @P14, @P15, @P16, @P17, @P18, @P19, @P20, @P21, @P22, @P23, 'ACTIVO')"
            End If

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@P1", ESTADOVEH)
                cmd.Parameters.AddWithValue("@P2", ORIGENDESTINO)
                cmd.Parameters.AddWithValue("@P3", PASILLO)
                cmd.Parameters.AddWithValue("@P4", SEGMENTO)
                cmd.Parameters.AddWithValue("@P5", CODIGO)
                cmd.Parameters.AddWithValue("@P6", DESCRIPCION)
                cmd.Parameters.AddWithValue("@P7", SERIEMOTO)
                cmd.Parameters.AddWithValue("@P8", SERIEMOTOR)
                cmd.Parameters.AddWithValue("@P9", MARCA)
                cmd.Parameters.AddWithValue("@P10", MODELO)
                cmd.Parameters.AddWithValue("@P11", CILINDROS)
                cmd.Parameters.AddWithValue("@P12", COLOR)
                cmd.Parameters.AddWithValue("@P13", YEAR)
                cmd.Parameters.AddWithValue("@P14", DIASARMADO)
                cmd.Parameters.AddWithValue("@P15", DIASINGRESO)
                cmd.Parameters.AddWithValue("@P16", OBSERVACIONES)
                cmd.Parameters.AddWithValue("@P17", USUARIO)
                If Not exists Then
                    cmd.Parameters.AddWithValue("@P18", DateTime.Now)
                End If
                cmd.Parameters.AddWithValue("@P19", False)
                cmd.Parameters.AddWithValue("@P20", False)
                cmd.Parameters.AddWithValue("@P21", False)
                cmd.Parameters.AddWithValue("@P22", ESTADOSAP)
                cmd.Parameters.AddWithValue("@P23", ALMACENSAP)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using

        Catch ex As Exception
            Response.Write("CREATEARMADO " & ex.Message)
        End Try
    End Sub

    Private Sub btnInicioPintura_Click(sender As Object, e As EventArgs) Handles btnInicioPintura.Click
        Try
            If drpFlyMiniPintores.SelectedValue = "Seleccione" Then
                Dim script As String = "iziToast.error({" &
                                   "title: 'Error!'," &
                                   "message: 'Seleccione un Pintor!!!'," &
                                   "position: 'topRight'," &
                                   "timeout: 5000" &
                                   "});"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowToast", script, True)
                Exit Sub
            Else
                UpdateInicioPintura(Date.Now, drpFlyMiniPintores.SelectedValue.ToString, txtFlyMiniSerial.Text)
            End If
        Catch ex As Exception
            Response.Write("btnInicioPintura_Click " & ex.Message)
        End Try
    End Sub

    Private Sub btnFlyCancelarUbicar_Click(sender As Object, e As EventArgs) Handles btnFlyCancelarUbicar.Click
        Try
            BindGrid()
        Catch ex As Exception
            Response.Write("btnFlyCancelarUbicar_Click " & ex.Message)
        End Try
    End Sub
End Class

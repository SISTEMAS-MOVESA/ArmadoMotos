Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Class AuditoriaInventarioArmadas
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    <WebMethod()>
    Public Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1
            Using cmd As SqlCommand = New SqlCommand()

                cmd.CommandText = " SELECT top 10 T1.MnfSerial FROM OSRN T1 with(nolock) INNER JOIN OITM T0 WITH(NOLOCK) " &
                    " ON T1.ITEMCODE=T0.ITEMCODE WHERE T1.ITEMCODE<>'PLA001' AND T1.[MnfSerial] LIKE '%' + @SearchText + '%'"
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
                txtCodigo.Text = drd.Item("ItemCode").ToString
                txtDescripcion.Text = drd.Item("ItemName").ToString
                txtModelo.Text = drd.Item("Modelo").ToString
                txtMarca.Text = drd.Item("Marca").ToString
                txtCilindros.Text = drd.Item("Cilindros").ToString
                txtSeriemoto.Text = drd.Item("SuppSerial").ToString
                txtSeriemotor.Text = drd.Item("IntrSerial").ToString
                txtColor.Text = drd.Item("Color").ToString
                txtYear.Text = drd.Item("BatchId").ToString
                txtArmado.Text = drd.Item("DiasArmado").ToString
                txtAgeing.Text = drd.Item("Dias").ToString
                txtALmacenSAP.Text = drd.Item("AlmacenSerie").ToString
                txtEstatusSerieSAP.Text = drd.Item("EstatusSAP").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CREATEARMADO(ESTADOVEH As String, ORIGENDESTINO As String, PASILLO As String, SEGMENTO As String, CODIGO As String,
                  DESCRIPCION As String, SERIEMOTO As String, SERIEMOTOR As String, MARCA As String, MODELO As String,
                  CILINDROS As String, COLOR As String, YEAR As String, DIASARMADO As Integer, DIASINGRESO As Integer,
                  OBSERVACIONES As String, USUARIO As String, LIMPIEZA As String, AUDITORIA As String, PINTURA As String,
                  ESTADOSAP As String, ALMACENSAP As String)
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
                cmd.Parameters.AddWithValue("@P19", LIMPIEZA)
                cmd.Parameters.AddWithValue("@P20", AUDITORIA)
                cmd.Parameters.AddWithValue("@P21", PINTURA)
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
    Public Sub UpdateOsrnSAP(_SERIE As String, _estado As String, _mecanicoid As String)
        Try
            Dim query As String = String.Empty
            query &= "UPDATE OSRN SET u_estado_produccion=@p2,U_mecanico=@p3 WHERE MnfSerial=@p1"
            Using conn As New SqlConnection(sCon1)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", _SERIE)
                        .Parameters.AddWithValue("@p2", _estado)
                        .Parameters.AddWithValue("@p3", _mecanicoid)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("UpdateOsrnSAPpAGO " & ex.Message)
        End Try
    End Sub

    Protected Sub btnBusarSerie_Click(sender As Object, e As EventArgs) Handles btnBusarSerie.Click
        Try
            BuscarSerie(txtSnMoto.Text)
            bindGridKardex(txtSnMoto.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub bindGridKardex(_SERIE As String)
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("	SELECT	T0.[MnfSerial],t1.Quantity, " &
                                                " case t2.DocType   " &
                                                " when 20 then 'Ingreso Mercaderia'   " &
                                                " when 67 then 'Transferencia Inventario'   " &
                                                " when 15 then 'Entrega de Ventas'   " &
                                                " when 13 then 'Factura Cliente'   " &
                                                " else convert(nvarchar,t2.doctype) end [Documento]  " &
                                                ", t2.Docnum, convert(char,t2.DocDate,103) [DocDate],t2.LocCode,(select whsname from owhs with(nolock) where whscode=t2.LocCode)[Almacen]   " &
                                                " FROM	OSRN T0 With(nolock) " &
                                                " inner join    " &
                                                " ITL1 T1 with(nolock) " &
                                                " on T1.SysNumber = T0.SysNumber and T1.ItemCode = T0.ItemCode    " &
                                                " inner join    " &
                                                " OITL T2 with(nolock) " &
                                                " on T1.LogEntry = T2.LogEntry " &
                                                " where	t0.MnfSerial =@p1  AND t2.DocType=20 " &
                                                " UNION ALL " &
                                                " SELECT  " &
                                                " [SERIE] COLLATE Modern_Spanish_CI_AS " &
                                                ", 0 " &
                                                " ,[ACCION] COLLATE Modern_Spanish_CI_AS " &
                                                ", [ID] " &
                                                " ,[DATELOG] " &
                                                " ,'DCM00' " &
                                                " ,'Distribucion Central de Motos' " &
                                                " FROM [ArmadoMotos].[dbo].[KARDEXVEHICULO] where serie = @p1 " &
                                                " UNION ALL " &
                                                " SELECT	T0.[MnfSerial],t1.Quantity, " &
                                                " case t2.DocType   " &
                                                " when 20 then 'Ingreso Mercaderia'   " &
                                                " when 67 then 'Transferencia Inventario'   " &
                                                " when 15 then 'Entrega de Ventas'   " &
                                                " when 13 then 'Factura Cliente'   " &
                                                " else convert(nvarchar,t2.doctype) end [Documento] " &
                                                " ,t2.Docnum,convert(char,t2.DocDate,103) [DocDate],t2.LocCode, " &
                                                " (select whsname from owhs with(nolock) where whscode=t2.LocCode)[Almacen] " &
                                                " FROM	OSRN T0 with(nolock)    " &
                                                " inner join    " &
                                                " ITL1 T1 with(nolock) " &
                                                " on T1.SysNumber = T0.SysNumber And T1.ItemCode = T0.ItemCode    " &
                                                " inner join    " &
                                                " OITL T2 with(nolock) " &
                                                " on T1.LogEntry = T2.LogEntry    " &
                                                " where	t0.MnfSerial = @p1  And t2.DocType<>20")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@p1", _SERIE)
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridKardex.DataSource = dt
                            gridKardex.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridKardex.UseAccessibleHeader = True
            gridKardex.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("bindGridKardex " & ex.Message)
        End Try
    End Sub

    Private Sub btnGuardarInfo_Click(sender As Object, e As EventArgs) Handles btnGuardarInfo.Click
        Try
            CREATEARMADO(drpEstadoVeh.SelectedValue.ToString,
                            drpOrigenDestino.SelectedValue.ToString,
                            drpPasillo.SelectedValue.ToString,
                            drpSegmento.SelectedValue.ToString,
                            txtCodigo.Text,
                            txtDescripcion.Text,
                            txtSeriemoto.Text,
                            txtSeriemotor.Text,
                            txtMarca.Text,
                            txtModelo.Text,
                            txtCilindros.Text,
                            txtColor.Text,
                            txtYear.Text,
                            txtArmado.Text,
                            txtAgeing.Text,
                            txtObservaciones.Text,
                            Session("Name").ToString,
                            chkLimpieza.Checked.ToString,
                            chkAuditoria.Checked.ToString,
                            chkPintura.Checked.ToString,
                            txtEstatusSerieSAP.Text,
                            txtALmacenSAP.Text)
            Response.Redirect("AuditoriaInventarioArmadas.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub AuditoriaInventarioArmadas_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class

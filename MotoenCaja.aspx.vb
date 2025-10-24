Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Partial Class MotoenCaja
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
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    <WebMethod()>
    Public Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon1 'ConfigurationManager.ConnectionStrings("constr").ConnectionString
            Using cmd As SqlCommand = New SqlCommand()

                cmd.CommandText = " SELECT top 10 T1.MnfSerial FROM OSRN T1 with(nolock) INNER JOIN OITM T0 WITH(NOLOCK) ON T1.ITEMCODE=T0.ITEMCODE WHERE isnull(T1.U_VIN,'-')<>'N' AND T1.[MnfSerial] LIKE '%' + @SearchText + '%'" 'and U_Estado='02' 

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
    Public Sub _CargarMecanicos()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon2)
                Dim query As String = "SELECT ID,MECANICONAME FROM [dbo].[MECANICOS] with(nolock) " '&
                '" where ID NOT IN (SELECT MECANICOASIGNADO FROM [dbo].[ARMADOMOTOS] WITH (NOLOCK) WHERE [ESTATUS] IN ('Asignada','Proceso'))"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
                drpMecanicos.Dispose()
                drpMecanicos.DataTextField = "MECANICONAME"
                drpMecanicos.DataValueField = "ID"
                drpMecanicos.DataSource = dt
                drpMecanicos.DataBind()
                conn.Close()
            End Using

        Catch ex As Exception
            'ADD_LOG("_CargarAlmacenes", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub BuscarSerie(_SERIE As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = " SELECT	T0.[ItemCode],T0.[ItemName],T1.[SuppSerial],T1.[IntrSerial],t1.BatchId, " &
                                        " T5.Name [Marca],T4.NAME [Cilindros], T3.Name [Motor],T2.Name [Modelo],T1.[WhsCode],T6.Name [Color], " &
                                        " (SELECT Owhs.[Whsname] FROM Owhs with(nolock) WHERE Owhs.[whscode]=T1.[WhsCode] ) [Almacen] " &
                                        " FROM OITM T0 with(nolock) INNER JOIN OSRI T1 with(nolock) ON T0.ItemCode = T1.ItemCode " &
                                        " FULL JOIN [@AMODELO] T2 WITH(NOLOCK) ON T0.U_AMODELO=T2.CODE  " &
                                        " FULL JOIN [@AMOTOR] T3 WITH(NOLOCK) ON T0.U_AMOTOR=T3.Code " &
                                        " FULL JOIN [@ACILINDROS] T4 WITH(NOLOCK) ON T0.U_ACILINDROS=T4.Code " &
                                        " FULL JOIN [@AMARCA] T5 WITH(NOLOCK) ON T0.U_AMARCA=T5.CODE " &
                                        " INNER JOIN [@SCOLOR] T6 WITH(NOLOCK) ON T0.U_ACOLOR=T6.CODE " &
                                        " WHERE isnull(T1.U_VIN,'-')<>'N' AND T1.[SuppSerial]='" & _SERIE & "' "
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                lblItemcode.Text = drd.Item("ItemCode").ToString
                lblItemname.Text = drd.Item("ItemName").ToString
                lblModelo.Text = drd.Item("Modelo").ToString
                lblYear.Text = drd.Item("BatchId").ToString
                lblColor.Text = drd.Item("Color").ToString
                lblMarca.Text = drd.Item("Marca").ToString
                lblCilindros.Text = drd.Item("Cilindros").ToString
                lblSerie.Text = drd.Item("SuppSerial").ToString
                lblSerieMotor.Text = drd.Item("IntrSerial").ToString
                lblMotorM.Text = drd.Item("Motor").ToString
            End If
            _con.Close()
            _CargarMecanicos()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CREATEARMADO(_Serie As String, _SerieM As String, _itemcode As String, _itenmane As String, _marca As String, _modelo As String, _motor As String, _year As String, _cilindros As String, _color As String, _asignador As String, _datecreated As Date, _mecanicoasignada As String, _estatus As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO dbo.ARMADOMOTOS (SERIE,SERIEM,ITEMCODE,ITENMANE,MARCA,MODELO,MOTOR,[YEAR],CILINDROS,COLOR,ASIGNADORCODE,DATECREATED,MECANICOASIGNADO,ESTATUS) " &
                    " VALUES(@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12,@P13,@P14)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _Serie)
                cmd.Parameters.AddWithValue("@p2", _SerieM)
                cmd.Parameters.AddWithValue("@p3", _itemcode)
                cmd.Parameters.AddWithValue("@p4", _itenmane)
                cmd.Parameters.AddWithValue("@p5", _marca)
                cmd.Parameters.AddWithValue("@p6", _modelo)
                cmd.Parameters.AddWithValue("@p7", _motor)
                cmd.Parameters.AddWithValue("@p8", _year)
                cmd.Parameters.AddWithValue("@p9", _cilindros)
                cmd.Parameters.AddWithValue("@p10", _color)
                cmd.Parameters.AddWithValue("@p11", _asignador)
                cmd.Parameters.AddWithValue("@p12", _datecreated)
                cmd.Parameters.AddWithValue("@p13", _mecanicoasignada)
                cmd.Parameters.AddWithValue("@p14", _estatus)
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
    Protected Sub btnBuscarMoto_Click(sender As Object, e As EventArgs) Handles btnBuscarMoto.Click
        If GetIfExistSerial(txtSnMoto.Text.Trim) = txtSnMoto.Text.Trim Then
            Response.Write("<script>alert('ERROR || Esa Serie Ya Fue Ingresada');</script>")
            txtSnMoto.Text = ""
            Exit Sub
        End If
        BuscarSerie(txtSnMoto.Text)
    End Sub
    Protected Sub btnIngreso_Click(sender As Object, e As EventArgs) Handles btnIngreso.Click
        Try
            If txtSnMoto.Text <> "" Then
                UpdateOsrnSAP(lblSerie.Text, "01", drpMecanicos.SelectedValue.ToString)
                KardexVehiculo(Date.Now, lblSerie.Text, lblMarca.Text, lblModelo.Text, lblColor.Text, "Asignada", "Asignada Para Armado", Session("UserCode").ToString, 0, 0)
                CREATEARMADO(lblSerie.Text, lblSerieMotor.Text, lblItemcode.Text, lblItemname.Text, lblMarca.Text, lblModelo.Text, lblMotorM.Text, lblYear.Text, lblCilindros.Text, lblColor.Text, Session("UserCode").ToString, Date.Now, drpMecanicos.SelectedValue.ToString, "Asignada")
                HASIGNA(lblSerie.Text, Date.Now, Session("UserCode").ToString)
                _CargarMecanicos()
                lblItemcode.Text = ""
                lblItemname.Text = ""
                lblModelo.Text = ""
                lblYear.Text = ""
                lblColor.Text = ""
                lblMarca.Text = ""
                lblCilindros.Text = ""
                lblSerie.Text = ""
                lblSerieMotor.Text = ""
                lblMotorM.Text = ""
                txtSnMoto.Text = ""
            Else
                Response.Write("<script>alert('ERROR || Debe Seleccionar un Numero de Serie');</script>")
            End If
            Response.Redirect("MotoenCaja.aspx")
        Catch ex As Exception
            Response.Write("btnIngreso_Click " & ex.Message)
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
    Public Sub HASIGNA(_Serie As String, _datecreated As Date, _usuario As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[HASIGNA]([SERIE],[FECHAINGRESO],[USUARIO]) " &
                    " VALUES(@P1,@P2,@P3)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _Serie)
                cmd.Parameters.AddWithValue("@p2", _datecreated)
                cmd.Parameters.AddWithValue("@p3", _usuario)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("HASIGNA " & ex.Message)
        End Try
    End Sub
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("MotoenCaja.aspx")
    End Sub

    Private Sub MotoenCaja_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    Select Case Session("Position").ToString()
                        Case "Supervisor Armado"
                            Response.Redirect("MainDashBoard.aspx")
                        Case "Calidad"
                            'Response.Redirect("MainDashBoard.aspx")
                    End Select
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Function GetIfExistSerial(_SERIE As String) As String
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "select SERIE from ArmadoMotos WITH(NOLOCK) where serie=@p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", _SERIE)
                con.Open()
                Dim t As String = cmd.ExecuteScalar()
                con.Close()
                Return t
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("GetIfExistSerial " & ex.Message)
        End Try
    End Function
End Class

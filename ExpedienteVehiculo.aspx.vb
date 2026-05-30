Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System.Configuration
Imports System.Web.Services
Partial Class ExpedienteVehiculo
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    <WebMethod()>
    Public Shared Function SearchCustomers(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Using conn As SqlConnection = New SqlConnection()
            conn.ConnectionString = sCon2 'ConfigurationManager.ConnectionStrings("constr").ConnectionString
            Using cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = " SELECT top 10 [SERIE] FROM [dbo].[ARMADOMOTOS] T1 with(nolock) WHERE T1.[ESTATUS] ='Armada' and T1.[SERIE] LIKE '%' + @SearchText + '%'"
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

    Protected Sub btnBuscarMoto_Click(sender As Object, e As EventArgs) Handles btnBuscarMoto.Click
        BuscarSerie(txtSnMoto.Text)
        EstadoSerieSAP(txtSnMoto.Text)
    End Sub
    Public Sub BuscarSerie(_SERIE As String)
        Try
            If _SERIE = "NULL" Then
                Exit Sub
            Else
                Dim dt As New DataTable
                Dim _con As New SqlConnection(sCon2)
                Dim Consulta As String = " SELECT *,substring(OBSERVACIONES1,PATINDEX('%:%',OBSERVACIONES1)+1,len(OBSERVACIONES1))[CC] FROM [dbo].[ARMADOMOTOS] T1 with(nolock) inner join MECANICOS t2 with(nolock) on t1.MECANICOASIGNADO=t2.ID WHERE T1.[SERIE]='" & _SERIE & "'"
                Dim Comando As New SqlCommand(Consulta, _con)
                Dim drd As SqlDataReader
                _con.Open()
                drd = Comando.ExecuteReader()
                If drd.Read() Then
                    lblItemcode.Text = drd.Item("ItemCode").ToString
                    lblItemname.Text = drd.Item("ITENMANE").ToString
                    lblModelo.Text = drd.Item("Modelo").ToString
                    lblYear.Text = drd.Item("YEAR").ToString
                    lblColor.Text = drd.Item("Color").ToString
                    lblMarca.Text = drd.Item("Marca").ToString
                    lblCilindros.Text = drd.Item("Cilindros").ToString
                    txtEstadoProduccion.Text = drd.Item("ESTATUS").ToString
                    lblMotorM.Text = drd.Item("Motor").ToString
                    txtMecanico.Text = drd.Item("MecanicoName").ToString
                    txtControlCalidad.Text = drd.Item("CC").ToString
                    BindGrid(_SERIE)
                Else
                    'Response.Write("<script>alert('ERROR || La Serie ya Paso por el Proceso de Calidad, Verifique los datos');</script>")
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

    'Public Sub HistorialPortal(_SERIE As String)
    '    Try
    '        If _SERIE = "NULL" Then
    '            Exit Sub
    '        Else
    '            Dim dt As New DataTable
    '            Dim _con As New SqlConnection(sCon2)
    '            Dim Consulta As String = "SELECT FORMAT(FIRSTUPDATEDATE, 'dd/MM/yyyy')[FIRSTUPDATEDATE],FORMAT(SECONDUPDATEDATE, 'dd/MM/yyyy')[SECONDUPDATEDATE],FORMAT(INICIOCC, 'dd/MM/yyyy')[INICIOCC],FORMAT(FINCC, 'dd/MM/yyyy')[FINCC] FROM [ArmadoMotos].[dbo].[ARMADOMOTOS] with(nolock) where SERIE='" & _SERIE & "'"
    '            Dim Comando As New SqlCommand(Consulta, _con)
    '            Dim drd As SqlDataReader
    '            _con.Open()
    '            drd = Comando.ExecuteReader()
    '            If drd.Read() Then
    '                txtInicioArmado.Text = drd.Item("FIRSTUPDATEDATE").ToString
    '                txtFinArmado.Text = drd.Item("SECONDUPDATEDATE").ToString
    '                txtInicioCalidad.Text = drd.Item("INICIOCC").ToString
    '                txtFinCalidad.Text = drd.Item("FINCC").ToString
    '            Else
    '                txtInicioArmado.Text = ""
    '                txtFinArmado.Text = ""
    '                txtInicioCalidad.Text = ""
    '                txtFinCalidad.Text = ""

    '            End If
    '        End If

    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try
    'End Sub

    Public Sub EstadoSerieSAP(_SERIE As String)
        Try
            If _SERIE = "NULL" Then
                Exit Sub
            Else
                Dim dt As New DataTable
                Dim _con As New SqlConnection(sCon1)
                Dim Consulta As String = "SELECT CASE [Status] WHEN 0 THEN 'Disponible' ELSE 'No Disponible' END [EMoto],case [U_Estado_Moto] when '01' THEN 'Nueva' ELSE 'Usada' END [ESerie] " &
                                            " FROM OSRI With(NOLOCK) WHERE SuppSerial='" & _SERIE & "'"
                Dim Comando As New SqlCommand(Consulta, _con)
                Dim drd As SqlDataReader
                _con.Open()
                drd = Comando.ExecuteReader()
                If drd.Read() Then
                    txtEstadpSN.Text = drd.Item("ESerie").ToString
                    txtEstadoMoto.Text = drd.Item("EMoto").ToString
                Else
                    txtEstadpSN.Text = ""
                    txtEstadoMoto.Text = ""
                End If
                _con.Close()
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindGrid(_SERIE As String)
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
                                                " ,'Distribucion Central de Motos' " &
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
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub
    Private Sub ExpedienteVehiculo_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    If Session("SerieExpediente").ToString <> "NULL" Then
                        BuscarSerie(Session("SerieExpediente").ToString)
                        EstadoSerieSAP(Session("SerieExpediente").ToString)
                        txtSnMoto.Text = Session("SerieExpediente").ToString
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Session("SerieExpediente") = "NULL"
        Response.Redirect("ExpedienteVehiculo.aspx")
    End Sub
End Class

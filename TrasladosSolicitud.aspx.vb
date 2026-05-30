Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Globalization

Partial Class TrasladosSolicitud
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
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "SELECT T1.Name,T0.ItemName " &
                                        " From [MOVESA].[dbo].[OITM] T0 WITH(NOLOCK) INNER Join [movesa].[dbo].[@AMODELO] T1 WITH(NOLOCK)  " &
                                        " ON T0.U_AMODELO=T1.Code Where U_LG5 ='Y' AND T0.ITMSGRPCOD=154 ORDER BY T0.ItemName"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
            drpModelosMoto.Dispose()
            drpModelosMoto.DataTextField = "ItemName"
            drpModelosMoto.DataValueField = "Name"
            drpModelosMoto.DataSource = dt
            drpModelosMoto.DataBind()

        Catch ex As Exception
            Response.Write("<script>alert('cargarModelosMotos : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub CargarMotoristas()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon2)
                Dim query As String = "select ID,RTN+ ' ' + EMPRESA + ' ' + PLACA [Empresa] from movesaweb..LOGISTICA_PLACAS_TRASLADOS order by EMPRESA"
                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
            drpMotoristas.Dispose()
            drpMotoristas.DataTextField = "Empresa"
            drpMotoristas.DataValueField = "ID"
            drpMotoristas.DataSource = dt
            drpMotoristas.DataBind()

        Catch ex As Exception
            Response.Write("<script>alert('CargarMotoristas : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Function AddBusinessDays(ByVal startDate As DateTime, ByVal businessDays As Integer) As DateTime
        Dim count As Integer = 0
        Dim currentDate As DateTime = startDate

        While count < businessDays
            currentDate = currentDate.AddDays(1)

            ' Verificar si el día actual es laborable
            If IsBusinessDay(currentDate) Then
                count += 1
            End If
        End While

        Return currentDate
    End Function

    Public Function IsBusinessDay(ByVal dateToCheck As DateTime) As Boolean
        ' Verificar si el día actual es sábado o domingo
        If dateToCheck.DayOfWeek = DayOfWeek.Saturday OrElse dateToCheck.DayOfWeek = DayOfWeek.Sunday Then
            Return False
        End If

        ' También puedes agregar otras reglas específicas de días no laborables
        ' como feriados, días festivos, etc.

        Return True
    End Function
    Private Sub TrasladosSolicitud_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    Dim startDate As DateTime = DateTime.Now
                    Dim businessDaysToAdd As Integer = 2

                    Dim newDate As DateTime = AddBusinessDays(startDate, businessDaysToAdd)
                    txtFechaArmado.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Now)
                    txtFechaEntrega.Text = String.Format("{0:yyyy-MM-dd}", newDate)
                    'lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    cargarModelosMotos()
                    CargarMotoristas()
                    'BindGridPreSolicitudesAbiertas()
                    BindGridSolicitudesAbiertas()
                    'MultiSelectRutas()
                End If
            End If
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Ocurrio Un Error, Revise Consola!');", True)
            'Response.Write("<script>console.log('" & ex.Message & "')</script>")
            'Response.Write("MainDashBoard_Load " & ex.Message)
        End Try
    End Sub

    Private Sub btnBuscarAlmacen_Click(sender As Object, e As EventArgs) Handles btnBuscarAlmacen.Click
        Try
            BuscarDatosAlmacen(txtAlmacenName.Text)
        Catch ex As Exception
            Response.Write("<script>alert('btnBuscarAlmacen_Click : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub BuscarDatosAlmacen(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " select WhsCode,WhsName,U_CardCode from OWHS with(nolock) where whsname ='" & whsname & "' "
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtAlmacenCode.Text = drd.Item("WhsCode").ToString
                txtAlmacenName.Text = drd.Item("WhsName").ToString
                txtAlmacenCustomer.Text = drd.Item("U_CardCode").ToString
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>alert('BuscarDatosAlmacen : " & EscapeJavaScriptString(ex.Message) & "');</script>")
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','***** Ocurrio Un Error al Intentar Buscar los Datos ***** <hr> BuscarDatosAlmacen');", True)
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub SOLICITUD_HEADER(WHSCODEORIGEN As String, WHSNAMEORIGEN As String, WHSCODE As String, WHSNAME As String, CODIGOCLIENTE As String,
                                CODEMODELO As String, MODELO As String, QTYSOLICITADA As Integer,
                                QTYPREPARADA As Integer, OBSERVACIONES As String, USERCREATED As String, DATECREATED As DateTime,
                                ESTATUS As String, FECHA_ARMADO As DateTime, FECHA_ENTREGA As DateTime, DESCRIPCION As String,
                                PREIDHEADER As String, PRELIDHEADER As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = " INSERT INTO [dbo].[SOLCITUD_HEADER]([WHSCODEORIGEN],[WHSNAMEORIGEN],[WHSCODE],[WHSNAME],[CODIGOCLIENTE]," &
                  " [CODEMODELO],[MODELO],[QTYSOLICITADA],[QTYPREPARADA],[OBSERVACIONES],[USERCREATED],[DATECREATED]," &
                  " [ESTATUS],[FECHAARMADO],[FECHAENTREGA],[DESCRIPCION],[PREIDHEADER],[PRELIDHEADER])" &
                    " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", WHSCODEORIGEN)
                cmd.Parameters.AddWithValue("@p2", WHSNAMEORIGEN)
                cmd.Parameters.AddWithValue("@p3", WHSCODE)
                cmd.Parameters.AddWithValue("@p4", WHSNAME)
                cmd.Parameters.AddWithValue("@p5", CODIGOCLIENTE)
                cmd.Parameters.AddWithValue("@p6", CODEMODELO)
                cmd.Parameters.AddWithValue("@p7", MODELO)
                cmd.Parameters.AddWithValue("@p8", QTYSOLICITADA)
                cmd.Parameters.AddWithValue("@p9", QTYPREPARADA)
                cmd.Parameters.AddWithValue("@p10", OBSERVACIONES)
                cmd.Parameters.AddWithValue("@p11", USERCREATED)
                cmd.Parameters.AddWithValue("@p12", DATECREATED)
                cmd.Parameters.AddWithValue("@p13", ESTATUS)
                cmd.Parameters.AddWithValue("@p14", FECHA_ARMADO)
                cmd.Parameters.AddWithValue("@p15", FECHA_ENTREGA)
                cmd.Parameters.AddWithValue("@p16", DESCRIPCION)
                cmd.Parameters.AddWithValue("@p17", PREIDHEADER)
                cmd.Parameters.AddWithValue("@p18", PRELIDHEADER)

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Solicitud Guardada con Exito!<hr> SOLICITUD_HEADER');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('RecorrerConsulta : " & EscapeJavaScriptString(ex.Message) & "');</script>")
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','Ocurrio Un Error al Intentar Agregar Modelo <hr> SOLICITUD_HEADER');", True)
            Response.Write("SOLICITUD_HEADER " & ex.Message)
        End Try
    End Sub

    Private Sub btnLimpiarFormulario_Click(sender As Object, e As EventArgs) Handles btnLimpiarFormulario.Click
        Response.Redirect("TrasladosSolicitud.aspx")
    End Sub

    Private Sub btnCrearSolicitud_Click(sender As Object, e As EventArgs) Handles btnCrearSolicitud.Click
        Try
            If Session("Name") Is vbNullString Then
                Response.Redirect("Default.aspx")
            Else
                SOLICITUD_HEADER(txtCodeAlmacenOrigen.Text, txtNalmacenOrigen.Text, txtAlmacenCode.Text, txtAlmacenName.Text, txtAlmacenCustomer.Text,
                             GetModeloId(drpModelosMoto.SelectedValue.ToString), drpModelosMoto.SelectedValue.ToString, txtCantidad.Text, 0,
                             drpMotivoTraslado.SelectedItem.Text, Session("UserCode"), Date.Now, "SOLICITADO",
                             txtFechaArmado.Text, txtFechaEntrega.Text, drpModelosMoto.SelectedItem.Text, 0, 0)
                BindGridSolicitudesAbiertas()
            End If
        Catch ex As Exception
            Response.Write("<script>alert('btnCrearSolicitud_Click : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Private Sub BindGridSolicitudesAbiertas()
        Try


            'Dim constr As String = sCon1
            'Using con As New SqlConnection(constr)
            '    Using cmd As New SqlCommand("SELECT [ID],[WHSCODE],[WHSNAME],[MODELO],[QTYSOLICITADA], " &
            '                                "[QTYPREPARADA],[OBSERVACIONES],[DATECREATED],[DATEUPDATED],[PREIDHEADER],[PRELIDHEADER] " &
            '                                "FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] WHERE ESTATUS='SOLICITADO'")
            '        Using sda As New SqlDataAdapter()
            '            cmd.Connection = con
            '            sda.SelectCommand = cmd
            '            Using dt As New DataTable()
            '                sda.Fill(dt)
            '                gridSolicitudAbierta.DataSource = dt
            '                gridSolicitudAbierta.DataBind()
            '                con.Close()
            '                Dim totalMotos As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("CANTIDAD"))
            '                Dim totalEspacios As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("ESPACIOS"))

            '                gridSolicitudAbierta.FooterRow.Cells(3).Text = "Total Motos: " & totalMotos.ToString("N2")
            '                gridSolicitudAbierta.FooterRow.Cells(3).Font.Bold = True
            '                gridSolicitudAbierta.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Center

            '                gridSolicitudAbierta.FooterRow.Cells(4).Text = "Total Espacios: " & totalEspacios.ToString("N2")
            '                gridSolicitudAbierta.FooterRow.Cells(4).Font.Bold = True
            '                gridSolicitudAbierta.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Center

            '                gridSolicitudAbierta.UseAccessibleHeader = True
            '                gridSolicitudAbierta.HeaderRow.TableSection = TableRowSection.TableHeader
            '            End Using
            '        End Using
            '    End Using
            'End Using



            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [ID],[WHSCODE],[WHSNAME],[MODELO],[QTYSOLICITADA], " &
                                            "[QTYPREPARADA],[OBSERVACIONES],[DATECREATED],[DATEUPDATED],[PREIDHEADER],[PRELIDHEADER] " &
                                            "FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER] WHERE ESTATUS='SOLICITADO'")

                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
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

    'Private Sub BindGridPreSolicitudesAbiertas()
    '    Try
    '        Dim constr As String = sCon1
    '        Using con As New SqlConnection(constr)
    '            Using cmd As New SqlCommand("select	t0.ID,t0.ALMORIGEN,t0.ALMDESTINO,t1.ARTICULO," &
    '                                        "(select t4.[Name] from movesa..oitm t3 with(nolock) inner join movesa..[@amodelo] t4 with(nolock) on t3.U_AMODELO=t4.code where itemcode=t1.ARTICULO collate Modern_Spanish_CI_AS)" &
    '                                        " [MODELO],t1.CANTIDAD,t1.ESPACIOS,t0.OBSERACIONES,CONVERT(CHAR,T0.FECHADESEADA,103) [FECHADESEADA]  " &
    '                                        " from	[dbo].[PRESOLICITUD_HEADER] t0 With(nolock) " &
    '                                        " inner Join " &
    '                                        " [dbo].[PRESOLICITUD_LINES] t1 with(nolock) " &
    '                                        " On t0.id=t1.headerid And t0.almorigen=t1.almorigen And t0.almdestino=t1.almdestino " &
    '                                        " where t0.ESTATUS ='PRESOLICITUD' order by t0.OBSERACIONES desc")

    '                Using sda As New SqlDataAdapter()
    '                    cmd.Connection = con
    '                    sda.SelectCommand = cmd
    '                    Using dt As New DataTable()
    '                        sda.Fill(dt)
    '                        gridPresolicitudes.DataSource = dt
    '                        gridPresolicitudes.DataBind()
    '                        con.Close()
    '                    End Using
    '                End Using
    '            End Using
    '        End Using
    '        gridPresolicitudes.UseAccessibleHeader = True
    '        gridPresolicitudes.HeaderRow.TableSection = TableRowSection.TableHeader
    '    Catch ex As Exception
    '        'Response.Write(ex.Message)
    '    End Try
    'End Sub

    Private Sub BindGridgridTaskFlow()
        Try
            Dim constr As String = sCon1
            Using con As New SqlConnection(constr)
                Dim rutas As New List(Of String)()


                'For Each listItem As ListItem In lstMultiSelectRutas.Items
                For Each listItem As ListItem In lstRutasDisponibles.Items
                    If listItem.Selected Then
                        rutas.Add(listItem.Text)
                    End If
                Next

                'Using cmd As New SqlCommand("select	t0.ID, Convert(Char,T0.FECHADESEADA,103) [FECHADESEADA] , t0.OBSERACIONES " &
                '                " , t1.ARTICULO, T1.DESCRIPCION, T1.[RUTA]   " &
                '                " , (select t4.[Name] from movesa..oitm t3 with(nolock) inner join movesa..[@amodelo] t4 With(nolock) " &
                '                " On t3.U_AMODELO=t4.code where itemcode=t1.ARTICULO collate Modern_Spanish_CI_AS) [MODELO]   " &
                '                " , t1.CANTIDAD, t1.ESPACIOS,t0.ALMORIGEN " &
                '                " ,t0.ALMDESTINO " &
                '                " ,t0.CODCLIALMDESTINO, T1.ID" &
                '                " from	[dbo].[PRESOLICITUD_HEADER] t0 With(nolock)   " &
                '                " inner Join  [dbo].[PRESOLICITUD_LINES] t1 With(nolock) " &
                '                " On t0.id=t1.headerid And t0.almorigen=t1.almorigen And t0.almdestino=t1.almdestino  where t0.ESTATUS ='PRESOLICITUD' " &
                '                " And ruta In (" & String.Join(", ", rutas.Select(Function(x) "'" & x & "'")) & ") order by t0.OBSERACIONES desc", con)



                Using cmd As New SqlCommand("select	t0.ID, Convert(Char,T0.FECHADESEADA,103) [FECHADESEADA] , t0.OBSERACIONES " &
            " , t1.ARTICULO, T1.DESCRIPCION, T1.[RUTA]   " &
            " , (select t4.[Name] from movesa..oitm t3 with(nolock) inner join movesa..[@amodelo] t4 With(nolock) " &
            " On t3.U_AMODELO=t4.code where itemcode=t1.ARTICULO collate Modern_Spanish_CI_AS) [MODELO]   " &
            " , t1.CANTIDAD, t1.ESPACIOS,t0.ALMORIGEN " &
            " , (SELECT WHSNAME FROM MOVESA..OWHS WITH(NOLOCK) WHERE WhsCode=t0.ALMORIGEN COLLATE Modern_Spanish_CI_AS)[NORIGEN]   " &
            " ,t0.ALMDESTINO " &
            " , (SELECT WHSNAME FROM MOVESA..OWHS WITH(NOLOCK) WHERE WhsCode=t0.ALMDESTINO COLLATE Modern_Spanish_CI_AS)[NDESTINO]   " &
            " ,t0.CODCLIALMDESTINO, T1.ID [IDLINEA]" &
            " from	[dbo].[PRESOLICITUD_HEADER] t0 With(nolock)   " &
            " inner Join  [dbo].[PRESOLICITUD_LINES] t1 With(nolock) " &
            " On t0.id=t1.headerid And t0.almorigen=t1.almorigen And t0.almdestino=t1.almdestino  where t1.[ESTADO]='PRESOLICITUD' " &
            " And ruta In (" & String.Join(", ", rutas.Select(Function(x) "'" & x & "'")) & ") order by t0.OBSERACIONES desc", con)


                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridTaskFlow.DataSource = dt
                            gridTaskFlow.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
                gridTaskFlow.UseAccessibleHeader = True
                gridTaskFlow.HeaderRow.TableSection = TableRowSection.TableHeader

                'Using cmd As New SqlCommand("select	t0.ID,t0.ALMORIGEN,t0.ALMDESTINO,t1.ARTICULO,T1.DESCRIPCION,T1.[RUTA] " &
                '                            ", (select t4.[Name] from movesa..oitm t3 with(nolock) inner join movesa..[@amodelo] t4 With(nolock)  " &
                '                            " On t3.U_AMODELO=t4.code where itemcode=t1.ARTICULO collate Modern_Spanish_CI_AS) [MODELO] " &
                '                            ", t1.CANTIDAD, t1.ESPACIOS, t0.OBSERACIONES, Convert(Char,T0.FECHADESEADA,103) [FECHADESEADA] " &
                '                            ", (SELECT WHSNAME FROM MOVESA..OWHS WITH(NOLOCK) WHERE WhsCode=t0.ALMORIGEN COLLATE Modern_Spanish_CI_AS)[NORIGEN] " &
                '                            ", (SELECT WHSNAME FROM MOVESA..OWHS WITH(NOLOCK) WHERE WhsCode=t0.ALMDESTINO COLLATE Modern_Spanish_CI_AS)[NDESTINO] " &
                '                            " from	[dbo].[PRESOLICITUD_HEADER] t0 With(nolock) " &
                '                            " inner Join " &
                '                            " [dbo].[PRESOLICITUD_LINES] t1 With(nolock) " &
                '                            " On t0.id=t1.headerid And t0.almorigen=t1.almorigen And t0.almdestino=t1.almdestino " &
                '                            " where t0.ESTATUS ='PRESOLICITUD' and ruta in (@rutas) order by t0.OBSERACIONES desc")

                '    Dim parametro As New SqlParameter("@rutas", SqlDbType.VarChar)
                '    For Each listItem As ListItem In lstMultiSelectRutas.Items
                '        If listItem.Selected Then
                '            parametro.Value = lstMultiSelectRutas.SelectedItem.ToString()
                '            cmd.Parameters.Add(parametro)
                '        End If
                '    Next
                '    Using sda As New SqlDataAdapter()
                '        cmd.Connection = con
                '        sda.SelectCommand = cmd
                '        Using dt As New DataTable()
                '            sda.Fill(dt)
                '            gridTaskFlow.DataSource = dt
                '            gridTaskFlow.DataBind()
                '            con.Close()
                '        End Using
                '    End Using
                'End Using
            End Using
        Catch ex As Exception
            'Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub gridSolicitudAbierta_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridSolicitudAbierta.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridSolicitudAbierta.Rows(index)
                EliminarLinea(gridSolicitudAbierta.Rows(index).Cells(0).Text)
                BindGridSolicitudesAbiertas()
            End If
        Catch ex As Exception
            Response.Write("<script>alert('gridSolicitudAbierta_RowCommand : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub EliminarLinea(idlinea As Integer)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "DELETE FROM SOLCITUD_HEADER where [ID] = @p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Moto Eliminada con Exito!<hr> EliminarLinea');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('EliminarLinea : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Private Sub btnCrearCamion_Click(sender As Object, e As EventArgs) Handles btnCrearCamion.Click
        'aqui
        Try
            Dim nextcamion As String = GetNextCamion()
            UpdateNextCamion(nextcamion, Session("UserCode"))

            For Each row As GridViewRow In gridSolicitudAbierta.Rows
                UpdatePreSolicitud(row.Cells(7).Text, nextcamion, Session("UserCode"), row.Cells(8).Text)
            Next
            BindGridSolicitudesAbiertas()
        Catch ex As Exception
            Response.Write("<script>alert('btnCrearCamion_Click : " & EscapeJavaScriptString(ex.Message) & "');</script>")

        End Try
    End Sub
    Public Sub UpdateNextCamion(camion As String, usuario As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "update [ArmadoMotos].[dbo].[SOLCITUD_HEADER] Set CAMION=@p1, [ESTATUS] ='CAMION' where [ESTATUS] = 'SOLICITADO' AND [USERCREATED]=@p2"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", camion)
                cmd.Parameters.AddWithValue("@p2", usuario)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Camion Creado con Exito!');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('UpdateNextCamion : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub UpdatePreSolicitud(headerid As String, camion As String, usuario As String, lineheaderid As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "update [dbo].[PRESOLICITUD_HEADER] set ESTATUS='PROCESADO', CAMION=@p2, USERUPDATED=@p3,DATEUPDATED=getdate() where ID=@p1;" &
                "update [dbo].[PRESOLICITUD_LINES] set ESTADO='PROCESADO', USERUPDATED=@p3 where ID=@p4"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", headerid)
                cmd.Parameters.AddWithValue("@p2", camion)
                cmd.Parameters.AddWithValue("@p3", usuario)
                cmd.Parameters.AddWithValue("@p4", lineheaderid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Camion Creado con Exito!');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('UpdatePreSolicitud : " & EscapeJavaScriptString(ex.Message) & "');</script>")

        End Try
    End Sub
    Public Function GetNextCamion() As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT ISNULL(MAX([CAMION]),0)+1[Camion] FROM [ArmadoMotos].[dbo].[SOLCITUD_HEADER]"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            'cmd.Parameters.AddWithValue("@p1", COTIZAID)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            'Response.Write("<script>console.log('GetNextCamion: " & t & "');</script>")
        End Using
    End Function
    Protected Sub btnAlmOrigen_Click(sender As Object, e As EventArgs) Handles btnAlmOrigen.Click
        BuscarDatosAlmacenOrigen(txtNalmacenOrigen.Text)
    End Sub
    Public Sub BuscarDatosAlmacenOrigen(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " select WhsCode,WhsName,U_CardCode from OWHS with(nolock) where whsname ='" & whsname & "' "
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtCodeAlmacenOrigen.Text = drd.Item("WhsCode").ToString
                txtNalmacenOrigen.Text = drd.Item("WhsName").ToString
                'txtAlmacenCustomer.Text = drd.Item("U_CardCode").ToString
            End If
            _con.Close()
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','***** Ocurrio Un Error al Intentar Buscar los Datos ***** <hr> BuscarDatosAlmacenOrigen');", True)
            Response.Write("<script>alert('BuscarDatosAlmacenOrigen : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub BuscarDatosAlmacenOrigenByCode(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " select WhsCode,WhsName,U_CardCode from OWHS with(nolock) where whscode ='" & whsname & "' "
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtCodeAlmacenOrigen.Text = drd.Item("WhsCode").ToString
                txtNalmacenOrigen.Text = drd.Item("WhsName").ToString
                'txtAlmacenCustomerDestino.Text = drd.Item("U_CardCode").ToString
            End If
            _con.Close()
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','***** Ocurrio Un Error al Intentar Buscar los Datos ***** <hr> BuscarDatosAlmacenDestino');", True)
            Response.Write("<script>alert('BuscarDatosAlmacenOrigenByCode : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub BuscarDatosAlmacenDestinoByCode(whsname As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = " select WhsCode,WhsName,U_CardCode from OWHS with(nolock) where whscode ='" & whsname & "' "
            Dim Comando As New SqlCommand(Consulta, _con)
            Dim drd As SqlDataReader
            _con.Open()
            drd = Comando.ExecuteReader()
            If drd.Read() Then
                txtAlmacenCode.Text = drd.Item("WhsCode").ToString
                txtAlmacenName.Text = drd.Item("WhsName").ToString
                txtAlmacenCustomer.Text = drd.Item("U_CardCode").ToString
            End If
            _con.Close()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','***** Ocurrio Un Error al Intentar Buscar los Datos ***** <hr> BuscarDatosAlmacenDestino');", True)
            Response.Write("<script>alert('BuscarDatosAlmacenDestinoByCode : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Private Sub gridPresolicitudes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPresolicitudes.RowCommand
        Try
            If e.CommandName = "Agregar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridPresolicitudes.Rows(index)
                gridPresolicitudes.Rows(index).Cells(0).BackColor = Drawing.Color.Coral
                BuscarDatosAlmacenOrigenByCode(gridPresolicitudes.Rows(index).Cells(1).Text)
                BuscarDatosAlmacenDestinoByCode(gridPresolicitudes.Rows(index).Cells(2).Text)
                drpModelosMoto.SelectedIndex = drpModelosMoto.Items.IndexOf(drpModelosMoto.Items.FindByText(gridPresolicitudes.Rows(index).Cells(3).Text.Trim))
                drpMotivoTraslado.SelectedIndex = drpMotivoTraslado.Items.IndexOf(drpMotivoTraslado.Items.FindByText(gridPresolicitudes.Rows(index).Cells(6).Text.Trim))
                txtCantidad.Text = gridPresolicitudes.Rows(index).Cells(4).Text
                'EliminarLinea(gridPresolicitudes.Rows(index).Cells(0).Text)
                'BindGridSolicitudesAbiertas()
            End If
        Catch ex As Exception
            Response.Write("<script>alert('gridPresolicitudes_RowCommand : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub UpdatePresolicitud(rowid As String, usuario As String, updatedate As DateTime, camion As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "  update [ArmadoMotos].[dbo].[PRESOLICITUD_HEADER] set ESTATUS=@p1, USERUPDATED=@P2,DATEUPDATED=@p3,CAMION=@p4 where id=@p5;" &
                "update [ArmadoMotos].[dbo].[PRESOLICITUD_LINES] set ESTADO=@p1,USERUPDATED=@p2 where id=@p5"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", "SOLICITUD CREADA")
                cmd.Parameters.AddWithValue("@p2", usuario)
                cmd.Parameters.AddWithValue("@p3", updatedate)
                cmd.Parameters.AddWithValue("@p4", camion)
                cmd.Parameters.AddWithValue("@p5", rowid)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                BindGridSolicitudesAbiertas()

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Presolicitud Actualizada con Exito!');", True)
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('UpdatePresolicitud : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    'Public Sub MultiSelectRutas()
    '    Try
    '        Dim dt As DataTable = New DataTable()
    '        Using conn As SqlConnection = New SqlConnection(sCon1)
    '            Dim query As String = "select [CODE],[NAME] from MOVESA..[@CRUTA] order by [NAME]"
    '            Dim cmd As SqlCommand = New SqlCommand(query, conn)
    '            Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
    '            da.Fill(dt)
    '        End Using
    '        lstMultiSelectRutas.Dispose()
    '        lstMultiSelectRutas.DataTextField = "NAME"
    '        lstMultiSelectRutas.DataValueField = "CODE"
    '        lstMultiSelectRutas.DataSource = dt
    '        lstMultiSelectRutas.DataBind()

    '    Catch ex As Exception
    '        'ADD_LOG("_CargarAlmacenes", ex.Message)
    '        'Response.Write(ex.Message)
    '    End Try
    'End Sub

    'Private Sub btnPPresolicitud_Click(sender As Object, e As EventArgs) Handles btnPPresolicitud.Click
    '    BindGridgridTaskFlow()
    '    mpe.Show()
    '    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostrarModal", "<script>var modal = document.getElementById('myModal'); modal.style.display = 'block';</script>", False)
    '    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostrarModal", "<script>$('#myModal').modal('show');</script>", False)

    '    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#fsModal').modal('show');</script>", False)
    'End Sub
    Protected Sub repeaterTaskFlow_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim card As LiteralControl = New LiteralControl()
            card.Text = "<div class='card mb-3'>" & "<div class='card-body'>" & "<h5 class='card-title'>" & row("ID").ToString() & "</h5>" & "<p class='card-text'>" + row("ALMORIGEN").ToString() & "</p>" & "<p class='card-text'>" + row("ALMDESTINO").ToString() & "</p>" & "<p class='card-text'>" + row("MODELO").ToString() & "</p>" & "<p class='card-text'>" + row("CANTIDAD").ToString() & "</p>" & "<p class='card-text'>" + row("FECHADESEADA").ToString() & "</p>" & "<p class='card-text'>" + row("OBSERACIONES").ToString() & "</p>" & "<asp:Button ID='btnAgregar' runat='server' Text='Agregar' CommandName='Agregar' CssClass='btn btn-primary' />" & "</div>" & "</div>"
            e.Item.Controls.Add(card)
        End If
    End Sub

    Private Sub gridTaskFlow_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridTaskFlow.RowDataBound
        Try
            For Each row As GridViewRow In gridTaskFlow.Rows

                Select Case row.Cells(3).Text.Trim
                    Case "Venta Inmediata"
                        row.Cells(1).BackColor = Drawing.Color.Red
                    Case "Inventario"
                        row.Cells(1).BackColor = Drawing.Color.Orange
                End Select
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gridTaskFlow_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridTaskFlow.RowCommand
        If e.CommandName = "OcultarFila" Then
            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            gridTaskFlow.Rows(rowIndex).Visible = False
        End If
    End Sub
    Protected Sub CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        For Each Row As GridViewRow In gridTaskFlow.Rows
            Dim cb As CheckBox = TryCast(Row.FindControl("cbDocument"), CheckBox)
            If cb.Checked Then
                Response.Write(Row.Cells(1).Text) '("<script>console.log('" & Row.Cells(1).Text & "');</script>")
            End If
        Next
    End Sub

    Private Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click
        Try
            If txtFechaArme.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','No Se cargaron las motos... <br> Verifique la Fecha de Armado');", True)
                Exit Sub
            End If

            If txtFechaEnvio.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('error','No Se cargaron las motos... <br> Verifique la Fecha de Armado');", True)
                Exit Sub
            End If

            txtFechaArmado.Text = txtFechaArme.Text
            txtFechaEntrega.Text = txtFechaEnvio.Text

            For Each Row As GridViewRow In gridTaskFlow.Rows
                Dim cb As CheckBox = TryCast(Row.FindControl("cbDocument"), CheckBox)
                If cb.Checked Then
                    For i = 1 To CLng(Row.Cells(8).Text)
                        Response.Write("<script>console.log('Linea:" & Row.Cells(1).Text &
                                       " Articulo:" & Row.Cells(4).Text &
                                       " Descripcion:" & Row.Cells(5).Text &
                                       " Almacen Origen:" & Row.Cells(10).Text &
                                       " Nombre Almacen Origen:" & Row.Cells(11).Text &
                                       " Almacen Destino:" & Row.Cells(12).Text &
                                       " Nombre Almacen Destino:" & Row.Cells(13).Text &
                                       " Fecha Armado:" & txtFechaArme.Text &
                                       " Fecha Envio:" & txtFechaEnvio.Text &
                                       "');</script>")

                        SOLICITUD_HEADER(Row.Cells(10).Text, Row.Cells(11).Text, Row.Cells(12).Text, Row.Cells(13).Text, Row.Cells(14).Text,
                                            GetModeloId(Row.Cells(7).Text.Trim), Row.Cells(7).Text, 1, 0, Row.Cells(3).Text, Session("UserCode"), Date.Now, "SOLICITADO",
                                           txtFechaArme.Text, txtFechaEnvio.Text, Row.Cells(5).Text, Row.Cells(1).Text, Row.Cells(15).Text)
                    Next
                End If
            Next
            BindGridSolicitudesAbiertas()
        Catch ex As Exception
            Response.Write("<script>alert('btnProcesar_Click : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Function GetModeloId(modelo As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT CODE FROM [@AMODELO] WHERE [NAME]=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", modelo)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
        End Using
    End Function

    Protected Sub btnMOdalCargarRutas_Click(sender As Object, e As EventArgs) Handles btnMOdalCargarRutas.Click
        CargarRutas()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#modalRutas').modal('show');</script>", False)
    End Sub
    Public Sub CargarRutas()
        Try
            Dim constr As String = sCon2 'ConfigurationManager.ConnectionStrings("constr").ConnectionString
            Using con As SqlConnection = New SqlConnection(constr)
                Using cmd As SqlCommand = New SqlCommand("select code, name from [@CRUTA]")
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    con.Open()
                    lstRutasDisponibles.DataSource = cmd.ExecuteReader()
                    lstRutasDisponibles.DataTextField = "name"
                    lstRutasDisponibles.DataValueField = "code"
                    lstRutasDisponibles.DataBind()
                    con.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("<script>alert('CargarRutas : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub

    Private Sub btnGuardarSolicitud_Click(sender As Object, e As EventArgs) Handles btnGuardarSolicitud.Click
        BindGridgridTaskFlow()
        mpe.Show()

    End Sub

    Protected Sub btnBuscarSolicitud_Click(sender As Object, e As EventArgs) Handles btnBuscarSolicitud.Click
        Try
            RecorrerConsulta(txtPresolicitud.Text)
        Catch ex As Exception
            Response.Write("<script>alert('btnBuscarSolicitud : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Public Sub RecorrerConsulta(headerid As String)
        Try
            Dim almacen_presolicitud As String
            'BuscarDatosAlmacen("T" & Session("ALMTRANSIT"))
            Dim connectionString As String = sCon1
            Dim query As String = "SELECT t0.almorigen, " &
                                  "       (SELECT whsname FROM movesa..owhs WITH(NOLOCK) WHERE whscode = t0.almorigen COLLATE Modern_Spanish_CI_AS) AS [NALMACENORIGEN], " &
                                  "       T0.ALMDESTINO, " &
                                  "       (SELECT whsname FROM movesa..owhs WITH(NOLOCK) WHERE whscode = T0.ALMDESTINO COLLATE Modern_Spanish_CI_AS) AS [NALMACENDESTINO], " &
                                  "       T0.ALMDESTINO, T0.CODCLIALMDESTINO, T1.ARTICULO, " &
                                  "       (SELECT CODE FROM MOVESA..[@AMODELO] WHERE NAME = T1.MODELO COLLATE Modern_Spanish_CI_AS) AS [CODEMODELO], " &
                                  "       T1.MODELO, T1.DESCRIPCION, T1.CANTIDAD, T0.OBSERACIONES, T0.ID [HID], T1.ID [LID]" &
                                  "FROM PRESOLICITUD_HEADER t0 WITH(NOLOCK) " &
                                  "INNER JOIN PRESOLICITUD_LINES t1 WITH(NOLOCK) ON t0.id = t1.headerid " &
                                  "WHERE t0.id = " & headerid & ""

            Using connection As New SqlConnection(connectionString)
                Dim command As New SqlCommand(query, connection)
                Dim dataTable As New DataTable()

                connection.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()

                ' Cargar los datos en el DataTable
                dataTable.Load(reader)

                ' Recorrer el contenido del DataTable
                For Each row As DataRow In dataTable.Rows
                    ' Acceder a los valores de las columnas
                    Dim almorigen As String = row("almorigen").ToString()
                    Dim nalmacenorigen As String = row("NALMACENORIGEN").ToString()
                    Dim almdestino As String = row("ALMDESTINO").ToString()
                    Dim nalmacendestino As String = row("NALMACENDESTINO").ToString()
                    Dim codclialmdestino As String = row("CODCLIALMDESTINO").ToString()
                    Dim articulo As String = row("ARTICULO").ToString()
                    Dim codmodelo As String = row("CODEMODELO").ToString()
                    Dim modelo As String = row("MODELO").ToString()
                    Dim descripcion As String = row("DESCRIPCION").ToString()
                    Dim cantidad As Integer = Convert.ToInt32(row("CANTIDAD"))
                    Dim observaciones As String = row("OBSERACIONES").ToString()
                    Dim id As Integer = Convert.ToInt32(row("HID"))
                    Dim lineId As Integer = Convert.ToInt32(row("LID"))

                    almacen_presolicitud = nalmacendestino

                    'SOLICITUD_HEADER(row.Cells(10).Text, row.Cells(11).Text, row.Cells(12).Text, row.Cells(13).Text, row.Cells(14).Text,
                    '                    GetModeloId(row.Cells(7).Text.Trim), row.Cells(7).Text, 1, 0, row.Cells(3).Text, Session("UserCode"), Date.Now, "SOLICITADO",
                    '                   txtFechaArme.Text, txtFechaEnvio.Text, row.Cells(5).Text, row.Cells(1).Text, row.Cells(15).Text)

                    SOLICITUD_HEADER(almorigen, nalmacenorigen, almdestino, nalmacendestino, codclialmdestino, codmodelo, modelo, cantidad, 0, observaciones,
                                     Session("UserCode"), Date.Now, "SOLICITADO", Date.Now, Date.Now, descripcion, id, lineId)


                    'Response.Write("<script>alert('RecorrerConsulta : ================================');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & almorigen & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & nalmacenorigen & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & almdestino & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & nalmacendestino & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & codclialmdestino & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & articulo & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & codmodelo & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & modelo & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & descripcion & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & cantidad & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & observaciones & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & id & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : " & lineId & "');</script>")
                    'Response.Write("<script>alert('RecorrerConsulta : ================================');</script>")

                    '' Hacer algo con los valores obtenidos
                    '' Por ejemplo, imprimirlos en la consola
                    'Console.WriteLine("almorigen: " & almorigen)
                    'Console.WriteLine("NALMACENORIGEN: " & nalmacenorigen)
                    'Console.WriteLine("ALMDESTINO: " & almdestino)
                    'Console.WriteLine("NALMACENDESTINO: " & nalmacendestino)
                    'Console.WriteLine("CODCLIALMDESTINO: " & codclialmdestino)
                    'Console.WriteLine()
                    'Console.WriteLine("ARTICULO: " & articulo)
                    'Console.WriteLine("CODEMODELO: " & codmodelo)
                    'Console.WriteLine("MODELO: " & modelo)
                    'Console.WriteLine("DESCRIPCION: " & descripcion)
                    'Console.WriteLine("CANTIDAD: " & cantidad)
                    'Console.WriteLine("OBSERVACIONES: " & observaciones)
                    'Console.WriteLine("ID: " & id)
                    'Console.WriteLine("LINE ID: " & lineId)

                    ' Realizar otras operaciones o lógica con los datos

                    'Console.WriteLine("--------------------------------------")
                Next
                reader.Close()
            End Using
            BuscarDatosAlmacen(almacen_presolicitud)
            BindGridSolicitudesAbiertas()
        Catch ex As Exception
            Response.Write("<script>alert('RecorrerConsulta : " & EscapeJavaScriptString(ex.Message) & "');</script>")
        End Try
    End Sub
    Protected Function EscapeJavaScriptString(ByVal str As String) As String
        Return str.Replace("'", "\'").Replace("""", "\""")
    End Function
End Class

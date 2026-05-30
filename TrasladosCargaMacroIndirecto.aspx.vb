Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.ServiceModel.Description
Imports DocumentFormat.OpenXml.Spreadsheet
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class TrasladosCargaMacroIndirecto
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Private Sub TrasladosCargaMacroIndirecto_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else

                    CargarSucursales()
                    CargarRutaLogica()


                    If Not String.IsNullOrEmpty(Request.QueryString("almacen")) Then
                        Dim Suc As String = Request.QueryString("almacen")
                        drpOptions.SelectedValue = getEncargadoWhs(Suc)
                        btnCargaGeneral_Click(btnCargaGeneral, New EventArgs())
                        CARGA_INDIRECTA(Suc)

                        'Response.Write("<script>console.log('Almacen Presolicitud " & Suc & "');</script>")
                        'Response.Write("<script>console.log('Carga Presolicitud " & ID & "');</script>")
                    End If
                    If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                        ' El parámetro "id" existe y no está vacío
                        Dim id As String = Request.QueryString("id")
                        Dim Suc As String = Request.QueryString("destino")
                        drpOptions.SelectedValue = getEncargadoWhs(Suc)
                        InsertFromQueryToMacroInsert(Request.QueryString("id"))

                        'InsertFromQueryToMacroInsert(Request.QueryString("id"))
                        'drpSucursales.SelectedValue = Suc
                        btnCargaGeneral_Click(btnCargaGeneral, New EventArgs())
                        CARGA_INDIRECTA(Suc)

                        'Response.Write("<script>console.log('Almacen Presolicitud " & Suc & "');</script>")
                        'Response.Write("<script>console.log('Carga Presolicitud " & id & "');</script>")
                    Else
                        'Response.Write("<script>console.log('Carga Directa');</script>")
                    End If

                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('TrasladosCargaMacroIndirecto_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Sub CargarRutaLogica()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim SqlQry As String = "SELECT Code, Name FROM [@CRUTA] WHERE (Code >= 37)"
                Dim cmd As SqlCommand = New SqlCommand(SqlQry, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using
            drpRutaLogica.Dispose()
            drpRutaLogica.DataTextField = "Name"
            drpRutaLogica.DataValueField = "Code"
            drpRutaLogica.DataSource = dt
            drpRutaLogica.DataBind()
        Catch ex As Exception
            Response.Write("CargarRutaLogica " & ex.Message)
        End Try
    End Sub
    Public Sub CargarSucursales()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim SqlQry As String = "select distinct u_categorizacion [CODE], T1.Name + ' ' + t1.U_SLPNAME [SUPERVISOR] from	OWHS T0 " &
                        " INNER JOIN [@CRESPONSABLEOWHS] T1 WITH(NOLOCK) ON T0.U_Categorizacion=T1.CODE " &
                        " WHERE	T1.Name LIKE '%SUPERVISOR%'"

                Dim cmd As SqlCommand = New SqlCommand(SqlQry, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using
            drpOptions.Dispose()
            drpOptions.DataTextField = "SUPERVISOR"
            drpOptions.DataValueField = "CODE"
            drpOptions.DataSource = dt
            drpOptions.DataBind()
        Catch ex As Exception
            Response.Write("CargarSucursales " & ex.Message)
        End Try
    End Sub
    Private Sub BindGridClientesPareto(Canal As String, Supervisor As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String

                sql_string = "EXEC [dbo].[SP_LOGISTICA_PARETO_CANAL_I]  @p1, @p2"
                Using cmd As New SqlCommand(sql_string)
                    cmd.Parameters.AddWithValue("@p1", Canal)
                    cmd.Parameters.AddWithValue("@p2", Supervisor)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridParetoClientes.DataSource = dt
                            gridParetoClientes.DataBind()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridParetoClientes.UseAccessibleHeader = True
            gridParetoClientes.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Private Sub BindGridClientesParetoAll(Supervisor As String)

        'Response.Write("<script>console.log('Supervisor: " & ReplaceCharsForFileName(Supervisor, " ") & "');</script>")

        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String

                sql_string = "EXEC [dbo].[SP_LOGISTICA_PARETO_CANAL_I_ALL] @p1"
                Using cmd As New SqlCommand(sql_string)
                    cmd.Parameters.AddWithValue("@p1", Supervisor)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridParetoClientes.DataSource = dt
                            gridParetoClientes.DataBind()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridParetoClientes.UseAccessibleHeader = True
            gridParetoClientes.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGridClientesParetoAllRuta(ruta As String)
        'Response.Write("<script>console.log('Supervisor: " & ReplaceCharsForFileName(ruta, " ") & "');</script>")
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String

                sql_string = "EXEC [dbo].[SP_LOGISTICA_PARETO_CANAL_I_ALL_RUTA] @p1"
                Using cmd As New SqlCommand(sql_string)
                    cmd.Parameters.AddWithValue("@p1", ruta)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridParetoClientes.DataSource = dt
                            gridParetoClientes.DataBind()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridParetoClientes.UseAccessibleHeader = True
            gridParetoClientes.HeaderRow.TableSection = TableRowSection.TableHeader


        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

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
        NewStr = sName
        Return NewStr

    End Function

    Private Sub btnCadena_Click(sender As Object, e As EventArgs) Handles btnCadena.Click
        Try
            lblCanal.Text = "CADENA"
            lblAlmacen.Text = drpOptions.SelectedValue.ToString
            lblSupervisor.Text = drpOptions.SelectedItem.Text
            BindGridClientesPareto("CADENA", drpOptions.SelectedValue.ToString)

            gridCuadroBasico.DataSource = Nothing
            gridCuadroBasico.DataBind()

            gridPedidoTemp.DataSource = Nothing
            gridPedidoTemp.DataBind()

            lblCorreo.Text = getEmail(drpOptions.SelectedValue.ToString)

        Catch ex As Exception
            Response.Write("<script>console.log('btnCadena_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnConsigna_Click(sender As Object, e As EventArgs) Handles btnConsigna.Click
        Try
            lblCanal.Text = "CONS"
            lblAlmacen.Text = drpOptions.SelectedValue.ToString
            lblSupervisor.Text = drpOptions.SelectedItem.Text
            BindGridClientesPareto("CONS", drpOptions.SelectedValue.ToString)

            gridCuadroBasico.DataSource = Nothing
            gridCuadroBasico.DataBind()

            gridPedidoTemp.DataSource = Nothing
            gridPedidoTemp.DataBind()

            lblCorreo.Text = getEmail(drpOptions.SelectedValue.ToString)

        Catch ex As Exception
            Response.Write("<script>console.log('btnConsigna_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub gridParetoClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridParetoClientes.RowCommand
        Try
            If e.CommandName = "Cargar" Then
                txtLimiteCredito.Style("border") = "2px solid green"

                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridParetoClientes.Rows(index)
                lblWhscode.Text = gridParetoClientes.Rows(index).Cells(1).Text
                lblNombreAlmacen.Text = getWhsName(gridParetoClientes.Rows(index).Cells(1).Text)


                'aqui el importante
                Session("CurrIndex") = index
                lblRowIndex.Text = index

                Select Case lblCanal.Text
                    Case "ALL"
                        BindGridClientesParetoAll(lblAlmacen.Text)

                    Case "ALL RUTA"
                        BindGridClientesParetoAllRuta(lblAlmacen.Text)
                    Case Else
                        BindGridClientesPareto(lblCanal.Text, lblAlmacen.Text)

                End Select

                If lblCanal.Text = "ALL" Then
                Else
                End If

                BindGrid(gridParetoClientes.Rows(index).Cells(1).Text)

                BindgridSugerido(lblWhscode.Text, drpOptions.SelectedValue.ToString)
                BuscarSerie(lblWhscode.Text)

                lblNombreAlmacenHeader.Text = gridParetoClientes.Rows(index).Cells(1).Text + " - " & lblNombreAlmacen.Text
                lblRutaWhs.Text = GetRutaAlmacen(lblWhscode.Text)

                For Each row As GridViewRow In gridParetoClientes.Rows
                    If row.Cells(1).Text = lblWhscode.Text Then
                        row.Cells(0).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(1).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(2).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(3).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(4).BackColor = System.Drawing.Color.LightGreen
                    End If
                Next
                btnNotificar.Visible = True

                TraerEstadoCliente(getCustomerCode(gridParetoClientes.Rows(index).Cells(1).Text))
                SaldoConsignacion(getCustomerCode(gridParetoClientes.Rows(index).Cells(1).Text))

                If (CDec(txtSaldoCuenta.Text) + CDec(txtSaldoConsignacion.Text)) > CDec(txtLimiteCredito.Text) Then
                    txtLimiteCredito.Style("border") = "2px solid red"
                End If

            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Sub CARGA_INDIRECTA(WHSCODE As String)
        Try
            'Response.Write("<script>console.log('CARGA_INDIRECTA: " & ReplaceCharsForFileName(WHSCODE, " ") & "');</script>")


            txtLimiteCredito.Style("border") = "2px solid green"

            lblWhscode.Text = WHSCODE
            lblNombreAlmacen.Text = getWhsName(WHSCODE)

            Select Case lblCanal.Text
                Case "ALL"
                    BindGridClientesParetoAll(lblAlmacen.Text)

                Case "ALL RUTA"
                    BindGridClientesParetoAllRuta(lblAlmacen.Text)
                Case Else
                    BindGridClientesPareto(lblCanal.Text, lblAlmacen.Text)

            End Select

            BindGrid(WHSCODE)

            BindgridSugerido(WHSCODE, drpOptions.SelectedValue.ToString)
            BuscarSerie(lblWhscode.Text)

            lblNombreAlmacenHeader.Text = WHSCODE + " - " & lblNombreAlmacen.Text
            lblRutaWhs.Text = GetRutaAlmacen(lblWhscode.Text)

            For Each row As GridViewRow In gridParetoClientes.Rows
                If row.Cells(1).Text = lblWhscode.Text Then
                    row.Cells(0).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(1).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(2).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(3).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(4).BackColor = System.Drawing.Color.LightGreen
                    lblRowIndex.Text = row.Cells(0).Text
                End If
            Next

            Session("CurrIndex") = lblRowIndex.Text
            btnNotificar.Visible = True

            TraerEstadoCliente(getCustomerCode(WHSCODE))
            SaldoConsignacion(getCustomerCode(WHSCODE))

            If (CDec(txtSaldoCuenta.Text) + CDec(txtSaldoConsignacion.Text)) > CDec(txtLimiteCredito.Text) Then
                txtLimiteCredito.Style("border") = "2px solid red"
            End If

        Catch ex As Exception
            Response.Write("CARGA_INDIRECTA " & ex.Message)
        End Try
    End Sub
    Private Sub BindGrid(whscode As String)
        Try
            'Response.Write("<script>console.log('BindGrid Canal: " & ReplaceCharsForFileName(whscode, " ") & "');</script>")

            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String
                sql_string = "EXEC [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS_I] @p1"
                Using cmd As New SqlCommand(sql_string)
                    cmd.Parameters.AddWithValue("@p1", whscode)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCuadroBasico.DataSource = dt
                            gridCuadroBasico.DataBind()
                        End Using
                    End Using
                End Using
                con.Close()
            End Using
            gridCuadroBasico.UseAccessibleHeader = True
            gridCuadroBasico.HeaderRow.TableSection = TableRowSection.TableHeader

            For Each row As GridViewRow In gridCuadroBasico.Rows

                If GetOrdenamiento(row.Cells(2).Text.ToString.Trim) = "True" Then
                    row.Cells(0).Text = 0
                    row.Cells(0).BackColor = System.Drawing.Color.GreenYellow
                End If



                row.Cells(22).Text = CInt(row.Cells(18).Text) + CInt(row.Cells(19).Text) + CInt(row.Cells(20).Text) + CInt(row.Cells(21).Text)

                row.Cells(17).Text = CInt(row.Cells(10).Text) - CInt(row.Cells(11).Text) + CInt(row.Cells(12).Text) - CInt(row.Cells(13).Text) - CInt(row.Cells(14).Text) - CInt(row.Cells(15).Text - CInt(row.Cells(16).Text))

                If row.Cells(17).Text > 0 Then
                    row.Cells(17).BackColor = System.Drawing.Color.Orange
                End If


                If row.Cells(16).Text > 0 Then
                    row.Cells(16).BackColor = System.Drawing.Color.Yellow
                End If

                If row.Cells(17).Text < 0 Then
                    row.Cells(17).ToolTip = "Sugerido Mayor al Cuadro Basico!!"
                    row.Cells(17).BackColor = System.Drawing.Color.Red
                End If

                If row.Cells(17).Text > row.Cells(10).Text Then
                    row.Cells(10).ToolTip = "Cuadro Basico Menor a la Venta Actual!!"
                    row.Cells(10).BackColor = System.Drawing.Color.Red
                    row.Cells(17).BackColor = System.Drawing.Color.Red
                End If

                If row.Cells(24).Text > 0 Then
                    row.Cells(24).BackColor = System.Drawing.Color.LightGreen
                End If

                'If row.Cells(10).Text > row.Cells(16).Text And row.Cells(15).Text = 0 Then
                '    row.Cells(15).Text = CInt(row.Cells(10).Text) - row.Cells(14).Text
                '    row.Cells(15).BackColor = System.Drawing.Color.Yellow
                'End If
                'If row.Cells(14).Text > row.Cells(16).Text And row.Cells(15).Text = 0 Then
                '    row.Cells(15).Text = CInt(row.Cells(10).Text) - row.Cells(14).Text
                '    row.Cells(15).BackColor = System.Drawing.Color.Yellow
                'End If
            Next

        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid gridCuadroBasico: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function getEncargadoWhs(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "select u_categorizacion from OWHS where whscode = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getEncargadoWhs: " & t & "');</script>")
        End Using
    End Function
    Public Function getWhsName(whscode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT WHSNAME FROM MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getWhsName: " & t & "');</script>")
        End Using
    End Function
    Public Function getCustomerCode(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT U_CARDCODE FROM MOVESA..OWHS WITH(NOLOCK) WHERE WHSCODE = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getCustomerCode: " & t & "');</script>")
        End Using
    End Function

    Public Function getEmail(code As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT [USEREMAIL] FROM [ArmadoMotos].[dbo].[USUARIOS] where [CODIGOSUP] = @p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", code)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getEmail: " & t & "');</script>")
        End Using
    End Function

    Private Sub btnCargaGeneral_Click(sender As Object, e As EventArgs) Handles btnCargaGeneral.Click
        Try
            lblCanal.Text = "ALL"
            lblAlmacen.Text = drpOptions.SelectedValue.ToString
            lblSupervisor.Text = drpOptions.SelectedItem.Text
            BindGridClientesParetoAll(drpOptions.SelectedValue.ToString)

            gridCuadroBasico.DataSource = Nothing
            gridCuadroBasico.DataBind()

            gridPedidoTemp.DataSource = Nothing
            gridPedidoTemp.DataBind()

            lblCorreo.Text = getEmail(drpOptions.SelectedValue.ToString)

        Catch ex As Exception
            Response.Write("<script>console.log('btnCargaGeneral_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub gridCuadroBasico_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCuadroBasico.RowCommand
        Try
            If e.CommandName = "Sugerir" Then
                lblRowIndex.Text = Session("CurrIndex")
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridCuadroBasico.Rows(index)
                BindgridColoresMoto(gridCuadroBasico.Rows(index).Cells(2).Text, lblWhscode.Text)
                Dim script As String = "$(document).ready(function(){$('#ColoresMotos').modal('show');});"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostrarModalColoresMotos", script, True)

                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#ColoresMotos').modal('show')</script>", False)

                If lblCanal.Text = "ALL" Then
                    BindGridClientesParetoAll(lblAlmacen.Text)
                Else
                    BindGridClientesPareto(lblCanal.Text, lblAlmacen.Text)
                End If

                lblAlmDestino.Text = lblWhscode.Text
                lblModeloCode.Text = gridCuadroBasico.Rows(index).Cells(1).Text
                lblModeloMoto.Text = gridCuadroBasico.Rows(index).Cells(2).Text

                lblruta.Text = GetRuta(lblWhscode.Text)
                lblcardcode.Text = GetCardcode(lblWhscode.Text)

                BindGrid(lblWhscode.Text)
                'BindGrid(gridParetoClientes.Rows(index).Cells(1).Text)

                For Each row As GridViewRow In gridParetoClientes.Rows
                    If row.Cells(1).Text = lblWhscode.Text Then
                        row.Cells(0).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(1).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(2).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(3).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(4).BackColor = System.Drawing.Color.LightGreen
                        lblRowIndex.Text = row.Cells(0).Text
                    End If
                Next


            End If

        Catch ex As Exception
            Response.Write("<script>console.log('gridCuadroBasico_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub btnCancelColoresMotos_Click(sender As Object, e As EventArgs) Handles btnCancelColoresMotos.Click
        Try
            If lblCanal.Text = "ALL" Then
                BindGridClientesParetoAll(lblAlmacen.Text)
            Else
                BindGridClientesPareto(lblCanal.Text, lblAlmacen.Text)
            End If
            BindGrid(lblWhscode.Text)

            For Each row As GridViewRow In gridParetoClientes.Rows
                If row.Cells(1).Text = lblWhscode.Text Then
                    row.Cells(0).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(1).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(2).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(3).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(4).BackColor = System.Drawing.Color.LightGreen
                    lblRowIndex.Text = row.Cells(0).Text
                End If
            Next

        Catch ex As Exception
            Response.Write("<script>console.log('btnCancelColoresMotos_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnModalColoresMotos_Click(sender As Object, e As EventArgs) Handles btnModalColoresMotos.Click
        Try
            'sigue por aqui
            For Each row As GridViewRow In gridColoresMoto.Rows
                Dim str As String = TryCast(row.FindControl("qty"), System.Web.UI.WebControls.TextBox).Text
                If str > 0 Then
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
                                drpOptions.SelectedValue.ToString,
                                Date.Now
                        )
                    If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
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
                                drpOptions.SelectedValue.ToString,
                                Date.Now,
                                lblModeloCode.Text)
                    End If

                End If
            Next

            If lblCanal.Text = "ALL" Then
                BindGridClientesParetoAll(lblAlmacen.Text)
            Else
                BindGridClientesPareto(lblCanal.Text, lblAlmacen.Text)
            End If
            BindGrid(lblWhscode.Text)

            BindgridSugerido(lblWhscode.Text, drpOptions.SelectedValue.ToString)
            BuscarSerie(lblWhscode.Text)


            For Each row As GridViewRow In gridParetoClientes.Rows
                If row.Cells(1).Text = lblWhscode.Text Then
                    row.Cells(0).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(1).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(2).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(3).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(4).BackColor = System.Drawing.Color.LightGreen
                    lblRowIndex.Text = row.Cells(0).Text
                End If
            Next


        Catch ex As Exception
            Response.Write("<script>console.log('btnModalColoresMotos_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub InsertFromQueryToMacroInsert(headerId As Integer)
        Try
            Dim sCon As String = sCon2
            Dim SQL_Query As String = "SELECT t1.RUTA, t1.ALMORIGEN, T1.ALMDESTINO, t0.CODCLIALMDESTINO, t1.ARTICULO, t1.MODELO, t1.DESCRIPCION, " &
                                      "t1.ESPACIOS, t1.CANTIDAD, t1.CANTIDAD, t1.CANTIDAD, T0.OBSERACIONES, 'T' AS ESTADO, " &
                                      "'CI' AS TIPO, GETDATE() AS FECHA, T0.USERCREATED, 0 AS CB, 0 AS FISICO, 0 AS FALTANTE, 0 AS VTAA " &
                                      "FROM PRESOLICITUD_HEADER t0 " &
                                      "INNER JOIN PRESOLICITUD_LINES t1 ON t0.ID = t1.HEADERID " &
                                      "WHERE T0.ID = @HeaderID"

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
                        Dim USER As String = drpOptions.SelectedValue.ToString

                        'Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ARTICULO, " ") & "');</script>")

                        InsertSugeridoSucursalesFromQuery(RUTA, ALMORIGEN, ALMDESTINO, CARDCODE, ARTICULO, MODELO, DESCRIPCION,
                                                 ESPACIOS, CANTIDAD, QTYLOGISTICA, QTYSUCURSAL, OBSERVACIONES & " SEGUN PRESOLICITUD: " & headerId,
                                                 USER, FECHA)
                    End While

                End Using

                con.Close()
            End Using
            BindgridSugerido(lblWhscode.Text, lblAlmacen.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
            Response.Write("<script>console.log('InsertFromQueryToMacroInsert: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function InsertSugeridoSucursales(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String, ARTICULO As String,
             MODELO As String, DESCRIPCION As String, ESPACIOS As String, CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                                 OBSERVACIONES As String, SUPERVISOR As String, FECHA As DateTime) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            Dim planId As Integer = 0

            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planId = Request.QueryString("planId")
            End If

            sel = "INSERT INTO [dbo].[MACROINSERT]([RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],[ARTICULO],[MODELO] " &
                    " ,[DESCRIPCION],[ESPACIOS],[CANTIDAD],[QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[ESTADO],[TIPO] " &
                    " ,[SUPERVISOR],[FECHA],[PLANID])" &
                    " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17); SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
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
                cmd.Parameters.AddWithValue("@p14", "CI")
                cmd.Parameters.AddWithValue("@p15", SUPERVISOR)
                cmd.Parameters.AddWithValue("@p16", FECHA)
                cmd.Parameters.AddWithValue("@p17", planId)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.WriteFile(ex.Message)
            Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Function InsertSugeridoSucursalesDespacho(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String, ARTICULO As String,
             MODELO As String, DESCRIPCION As String, ESPACIOS As String, CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                                 OBSERVACIONES As String, SUPERVISOR As String, FECHA As DateTime, CODMODELO As String) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            Dim planId As Integer = 0
            Dim headerId As Integer = 0

            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planId = Request.QueryString("planId")
                headerId = Request.QueryString("headerId")
            End If

            sel = "INSERT INTO [dbo].[DESPACHOS_DETALLE_MOTOS]([RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],[ARTICULO],[MODELO] " &
                    " ,[DESCRIPCION],[ESPACIOS],[CANTIDAD],[QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[ESTADO],[TIPO] " &
                    " ,[SUPERVISOR],[FECHA],[PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO])" &
                    " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20); SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
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
                cmd.Parameters.AddWithValue("@p14", "CI")
                cmd.Parameters.AddWithValue("@p15", SUPERVISOR)
                cmd.Parameters.AddWithValue("@p16", FECHA)
                cmd.Parameters.AddWithValue("@p17", planId)
                cmd.Parameters.AddWithValue("@p18", headerId)
                cmd.Parameters.AddWithValue("@p19", "ABIERTO")
                cmd.Parameters.AddWithValue("@p20", CODMODELO)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.WriteFile(ex.Message)
            Response.Write("<script>console.log('InsertSugeridoSucursalesDespacho: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Function InsertSugeridoSucursalesFromQuery(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String, ARTICULO As String,
            MODELO As String, DESCRIPCION As String, ESPACIOS As String, CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String,
                                                OBSERVACIONES As String, SUPERVISOR As String, FECHA As DateTime) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String

            sel = " " &
                    " if exists (SELECT * FROM MACROINSERT WITH(NOLOCK) WHERE ESTADO = 'T' AND ALMDESTINO = @p3 AND SUPERVISOR = @p15 AND ARTICULO = @p5) " &
                    "   BEGIN " &
                    "       DELETE FROM ArmadoMotos.dbo.MACROINSERT WHERE ESTADO = 'T' AND ALMDESTINO = @p3 AND SUPERVISOR = @p15 AND ARTICULO = @p5; " &
                    "       INSERT INTO [dbo].[MACROINSERT]([RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],[ARTICULO],[MODELO] " &
                    "       ,[DESCRIPCION],[ESPACIOS],[CANTIDAD],[QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[ESTADO],[TIPO] " &
                    "       ,[SUPERVISOR],[FECHA])" &
                    "       VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16); SELECT SCOPE_IDENTITY() " &
                    "   END " &
                    " ELSE " &
                    "   BEGIN " &
                    "       INSERT INTO [dbo].[MACROINSERT]([RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],[ARTICULO],[MODELO] " &
                    "       ,[DESCRIPCION],[ESPACIOS],[CANTIDAD],[QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[ESTADO],[TIPO] " &
                    "       ,[SUPERVISOR],[FECHA])" &
                    "       VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16); SELECT SCOPE_IDENTITY() " &
                    "   END"

            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
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
                cmd.Parameters.AddWithValue("@p14", "CI")
                cmd.Parameters.AddWithValue("@p15", SUPERVISOR)
                cmd.Parameters.AddWithValue("@p16", FECHA)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.WriteFile(ex.Message)
            Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function

    Private Sub BindgridColoresMoto(MODELO As String, WHSCODE As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("select	t0.itemcode, t0.itemname, t1.Name [Modelo], t0.u_columna [Espacios] " &
                        " , (select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode='DCM00' AND ItemCode=T0.ItemCode)[CEDIS] " &
                        " ,(select convert(int,onhand) from movesa..OITW with(nolock) where WhsCode = @p2 AND ItemCode=T0.ItemCode)[SUCURSAL] " &
                        " From movesa..oitm t0 with(nolock)  inner Join movesa..[@AMODELO] t1 with(nolock) On t0.U_AMODELO=t1.Code " &
                        " where t1.Name = @p1 and ItmsGrpCod=154 and t0.frozenFor='N' and t0.onhand>0")
                    cmd.Parameters.AddWithValue("@p1", MODELO)
                    cmd.Parameters.AddWithValue("@p2", WHSCODE)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridColoresMoto.DataSource = dt
                            gridColoresMoto.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridColoresMoto.UseAccessibleHeader = True
            gridColoresMoto.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridColoresMoto: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function GetRuta(whscode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "select t1.Name from [MOVESA].[dbo].owhs t0 inner join [MOVESA].[dbo].[@CRUTA] t1  on t0.u_ruta=t1.code where t0.WhsCode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
    Public Function GetCardcode(whscode As String) As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "select t0.u_Cardcode from [MOVESA].[dbo].owhs t0 where t0.WhsCode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
    Public Function GetRutaAlmacen(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "select t1.Name from owhs t0 with(nolock) inner join [@CRUTA] t1 on t0.U_Ruta=t1.Code where t0.WhsCode=@p1"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('GetEspacios: " & t & "');</script>")
        End Using
    End Function
    Public Function GetOrdenamiento(modelo As String) As String

        'Response.Write("<script>console.log('GetOrdenamiento: " & modelo & "');</script>")

        Dim sCon As String = sCon2
        Dim sel As String

        sel = " IF EXISTS (SELECT 1 FROM ORDENAMIENTO WHERE MODELO = @p1 and ACTIVOCI=1) " &
                  "       SELECT 'True'; " &
                  "  ELSE " &
                  "      SELECT 'False';"

        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@p1", modelo)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            'Response.Write("<script>console.log('GetOrdenamiento: " & t & "');</script>")

            Return t
            'Response.Write("<script>console.log('GetOrdenamiento: " & t & "');</script>")
        End Using
    End Function
    Private Sub BindgridSugerido(WHSCODE As String, SUPERVISOR As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT ID,ARTICULO,MODELO,DESCRIPCION,ESPACIOS,CANTIDAD,QTYLOGISTICA,OBSERVACIONES " &
                                                " FROM ArmadoMotos.dbo.MACROINSERT WHERE [ESTADO]='T' AND ALMDESTINO = @p1" &
                                                " And SUPERVISOR = @p2")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        cmd.Parameters.AddWithValue("@p1", WHSCODE)
                        cmd.Parameters.AddWithValue("@p2", SUPERVISOR)
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPedidoTemp.DataSource = dt
                            gridPedidoTemp.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridPedidoTemp.UseAccessibleHeader = True
            gridPedidoTemp.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido gridPedidoTemp : " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub gridPedidoTemp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPedidoTemp.RowCommand
        Try
            If e.CommandName = "Eliminar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridPedidoTemp.Rows(index)

                EliminarLinea(gridPedidoTemp.Rows(index).Cells(0).Text)

                If lblCanal.Text = "ALL" Then
                    BindGridClientesParetoAll(lblAlmacen.Text)
                Else
                    BindGridClientesPareto(lblCanal.Text, lblAlmacen.Text)
                End If

                BindgridSugerido(lblWhscode.Text, lblAlmacen.Text)
                BindGrid(lblWhscode.Text)

                For Each row As GridViewRow In gridParetoClientes.Rows
                    If row.Cells(1).Text = lblWhscode.Text Then
                        row.Cells(0).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(1).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(2).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(3).BackColor = System.Drawing.Color.LightGreen
                        row.Cells(4).BackColor = System.Drawing.Color.LightGreen
                        lblRowIndex.Text = row.Cells(0).Text
                    End If
                Next

            End If
        Catch ex As Exception
            Response.Write("<script>console.log('" & Replace(Replace(Replace(ex.Message, ",", ""), "'", ""), ";", "") & "');</script>")
        End Try
    End Sub
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMyFunction", "showContent('success','Moto Eliminada con Exito!');", True)
            End Using
        Catch ex As Exception
            Response.Write("EliminarLinea " & ex.Message)
        End Try
    End Sub
    Public Sub BuscarSerie(WHSCODE As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon2)
            Dim Consulta As String = "SP_LOGISTICA_PARETOMOTOS_BYWHS_I_ROTACION @p1"
            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@p1", WHSCODE)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then

                lbls00.Text = drd.Item("S00").ToString
                lbls30.Text = drd.Item("S30").ToString
                lbls60.Text = drd.Item("S60").ToString
                lbls90.Text = drd.Item("S90").ToString
                lblv00.Text = drd.Item("V00").ToString
                lblv30.Text = drd.Item("V30").ToString
                lblv60.Text = drd.Item("V60").ToString
                lblv90.Text = drd.Item("V90").ToString

                If drd.Item("V00").ToString = "0" Then
                    lblr00.Text = 0.00
                Else
                    lblr00.Text = FormatNumber(CDec(drd.Item("S00").ToString / drd.Item("V00").ToString), 2, TriState.True)
                End If
                If drd.Item("V30").ToString = "0" Then
                    lblr30.Text = 0.00
                Else
                    lblr30.Text = FormatNumber(CDec(drd.Item("S30").ToString / drd.Item("V30").ToString), 2, TriState.True)
                End If
                If drd.Item("V60").ToString = "0" Then
                    lblr60.Text = 0.00
                Else
                    lblr60.Text = FormatNumber(CDec(drd.Item("S60").ToString / drd.Item("V60").ToString), 2, TriState.True)
                End If
                If drd.Item("V90").ToString = "0" Then
                    lblr90.Text = 0.00
                Else
                    lblr90.Text = FormatNumber(CDec(drd.Item("S90").ToString / drd.Item("V90").ToString), 2, TriState.True)
                End If
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('BuscarSerie: " & Replace(Replace(Replace(ex.Message, ",", ""), "'", ""), ";", "") & "');</script>")

        End Try
    End Sub

    Private Sub btnNotificar_Click(sender As Object, e As EventArgs) Handles btnNotificar.Click
        Try
            ' Verificar si los campos están vacíos
            If String.IsNullOrWhiteSpace(txtFechaArmado.Text) Or String.IsNullOrWhiteSpace(txtFechaDespacho.Text) Then
                Dim scriptFechas As String = "<script>iziToast.error({title: 'Error!', message: 'Fantas Fechas de Armado o Despacho, Por Favor Revise!!!',timeout: 10000})</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessScript", scriptFechas, False)
                Exit Sub
            End If

            Dim header_macroinsert As Integer = MacroInsert_Header("DCM00", lblWhscode.Text, lblRutaWhs.Text, "1",
                                                                        Date.Now, IIf(Session("UserCode") Is vbNullString, "1038", Session("UserCode")),
                                                                        txtFechaArmado.Text, txtFechaDespacho.Text)
            If header_macroinsert <> 0 Then
                UpdateHeaderId(header_macroinsert, lblWhscode.Text)
                'fin
                If Session("User") <> "RJOVEL" Then
                    Dim server As New SmtpClient
                    Dim mensaje As New MailMessage
                    server.Host = "mail.grupomovesa.com"
                    server.Port = "587"
                    server.Credentials = New System.Net.NetworkCredential("notificaciones@grupomovesa.com", "VYgd}T!XcVy}")
                    mensaje.From = New MailAddress("notificaciones@grupomovesa.com")
                    Dim EMAIL As String = lblCorreo.Text
                    'Response.Write("<script>console.log('GetEspacios: " & EMAIL & "');</script>")

                    Dim constr As String = sCon2
                    Using con As New SqlConnection(constr)
                        Using cmd As New SqlCommand("SELECT [ALMDESTINO],[ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS]," &
                                                    " [QTYLOGISTICA],[OBSERVACIONES] FROM [ArmadoMotos].[dbo].[MACROINSERT] " &
                                                    " WHERE [ALMDESTINO] = @p1 AND HEADERID = @p2")

                            Using sda As New SqlDataAdapter()
                                cmd.Connection = con
                                cmd.Parameters.AddWithValue("@p1", lblWhscode.Text)
                                cmd.Parameters.AddWithValue("@p2", header_macroinsert)
                                sda.SelectCommand = cmd
                                Using dt As New DataTable()
                                    sda.Fill(dt)
                                    Dim pdfBytes As Byte() = ConvertDataTableToPdf(dt)
                                    Dim pdfStream As New MemoryStream(pdfBytes)
                                    mensaje.Attachments.Add(New Attachment(pdfStream, header_macroinsert & " " & lblWhscode.Text & "-" & drpOptions.SelectedItem.Text & ".pdf"))
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

                    mensaje.Subject = "Confirmacion Sugerido # " & header_macroinsert & " MOVESA - Portal Produccion de Motos"
                    mensaje.Body = "" & Session("Name") & ", Ha Creado un Nuevo Sugerido de Motos" &
                                    "<BR /> " &
                                    "PORTAL Produccion de Motos By RJ"

                    mensaje.IsBodyHtml = True
                    mensaje.Priority = MailPriority.High
                    server.Send(mensaje)
                    'Response.Write("<script>console.log('Correo Enviado');</script>")
                End If

                If lblCanal.Text = "ALL" Then
                    BindGridClientesParetoAll(lblAlmacen.Text)
                Else
                    BindGridClientesPareto(lblCanal.Text, lblAlmacen.Text)
                End If


                UpdateHeaderId(header_macroinsert, lblAlmacen.Text, txtFechaArmado.Text, txtFechaDespacho.Text)


                If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                    Response.Redirect("TrasladosDespachos.aspx?planId=" & Request.QueryString("planId"))
                End If

                BindGrid(lblWhscode.Text)
                BindgridSugerido(lblWhscode.Text, drpOptions.SelectedValue.ToString)
                EliminarPresolicitudOrigen(Request.QueryString("id"))

                lblNuevoDocto.Text = header_macroinsert.ToString()

                ' Código para mostrar el modal
                Dim script As String = "<script type='text/javascript'>" &
                           "$(document).ready(function(){" &
                           "$('#confirmacionDocto').modal('show');" &
                           "});</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostrarModalconfirmacionDocto", script, False)
            End If

            If lblCanal.Text = "ALL" Then
                BindGridClientesParetoAll(lblAlmacen.Text)
            Else
                BindGridClientesPareto(lblCanal.Text, lblAlmacen.Text)
            End If
            BindGrid(lblWhscode.Text)

            BindgridSugerido(lblWhscode.Text, drpOptions.SelectedValue.ToString)
            BuscarSerie(lblWhscode.Text)

            For Each row As GridViewRow In gridParetoClientes.Rows
                If row.Cells(1).Text = lblWhscode.Text Then
                    row.Cells(0).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(1).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(2).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(3).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(4).BackColor = System.Drawing.Color.LightGreen
                    lblRowIndex.Text = row.Cells(0).Text
                End If
            Next

        Catch ex As Exception
            Response.Write("<script>console.log('btnNotificar_Click " & Replace(Replace(Replace(ex.Message, ",", ""), "'", ""), ";", "") & "');</script>")
        End Try
    End Sub
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
            Response.Write("UpdateHeaderId " & ex.Message)
        End Try
    End Sub
    Public Sub EliminarPresolicitudOrigen(headerid As String)
        Try
            Dim query As String = String.Empty
            query &= "delete from PRESOLICITUD_HEADER where id = @p1; delete from PRESOLICITUD_LINES where HEADERID = @p1;"
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
            'Response.Write("<script>console.log('Documento de Origen Eliminado " & headerid & "');</script>")

        Catch ex As Exception
            Response.Write("EliminarPresolicitudOrigen " & ex.Message)
        End Try
    End Sub
    Public Function MacroInsert_Header(ORIGEN As String, DESTINO As String, RUTA As String, ESTADO As String,
                                           DATECREATED As DateTime, USERCREATED As String, FARMADO As DateTime, FDESPACHO As DateTime) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            Dim planid As Integer = 0

            If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                planid = Request.QueryString("planId")
            End If

            sel = "INSERT INTO [dbo].[MACROINSERT_HEADER]([ORIGEN],[DESTINO],[RUTA],[ESTADO],[DATECREATED],[USERCREATED],[ESTATUS],[FARMADO],[FDESPACHO],[PLANID])" &
                    " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10); SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ORIGEN)
                cmd.Parameters.AddWithValue("@p2", DESTINO)
                cmd.Parameters.AddWithValue("@p3", RUTA)
                cmd.Parameters.AddWithValue("@p4", ESTADO)
                cmd.Parameters.AddWithValue("@p5", DATECREATED)
                cmd.Parameters.AddWithValue("@p6", USERCREATED)
                cmd.Parameters.AddWithValue("@p7", "A")
                cmd.Parameters.AddWithValue("@p8", FARMADO)
                cmd.Parameters.AddWithValue("@p9", FDESPACHO)
                cmd.Parameters.AddWithValue("@p10", planid)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('MacroInsert_Header: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Sub UpdateHeaderId(headerid As String, almdestino As String)
        Try
            Dim query As String = String.Empty
            query &= "UPDATE [ArmadoMotos].[dbo].[MACROINSERT] SET [HEADERID] = @P1,[ESTADO] = 'S' where [ESTADO] = 'T' and [ALMDESTINO] = @P2"
            Using conn As New SqlConnection(sCon2)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@p1", headerid)
                        .Parameters.AddWithValue("@p2", almdestino)
                    End With
                    conn.Open()
                    comm.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            Response.Write("UpdateHeaderId " & ex.Message)
        End Try
    End Sub
    Public Function ConvertDataTableToPdf(dataTable As DataTable) As Byte()
        ' Crear el documento PDF en tamaño carta y formato horizontal
        Dim document As New Document(PageSize.LETTER.Rotate())
        Dim memoryStream As New MemoryStream()

        Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)
        document.Open()

        ' Crear una tabla en el documento PDF
        Dim table As New PdfPTable(dataTable.Columns.Count)
        table.WidthPercentage = 100

        ' Establecer el tamaño de letra y otros estilos de la celda
        Dim font As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8)
        Dim cellStyle As New PdfPCell()
        cellStyle.HorizontalAlignment = Element.ALIGN_LEFT
        cellStyle.VerticalAlignment = Element.ALIGN_MIDDLE
        cellStyle.Padding = 5

        ' Agregar los encabezados de columna a la tabla
        For i As Integer = 0 To dataTable.Columns.Count - 1
            Dim phrase As New Phrase(dataTable.Columns(i).ColumnName, font)
            Dim headerCell As New PdfPCell(phrase)
            headerCell.BackgroundColor = New BaseColor(230, 230, 230)
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE
            headerCell.Padding = 5
            table.AddCell(headerCell)
        Next

        ' Agregar los datos del DataTable a la tabla
        For i As Integer = 0 To dataTable.Rows.Count - 1
            For j As Integer = 0 To dataTable.Columns.Count - 1
                Dim phrase As New Phrase(dataTable.Rows(i)(j).ToString(), font)
                Dim dataCell As New PdfPCell(phrase)
                dataCell.HorizontalAlignment = Element.ALIGN_LEFT
                dataCell.VerticalAlignment = Element.ALIGN_MIDDLE
                dataCell.Padding = 5
                'dataCell.NoWrap = True ' Evitar el wrap de texto
                table.AddCell(dataCell)
            Next
        Next

        ' Agregar la tabla al documento
        document.Add(table)
        document.Close()

        ' Convertir el documento PDF a un arreglo de bytes
        Dim pdfBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()

        Return pdfBytes
    End Function

    Public Sub TraerEstadoCliente(cardcode As String)
        Try
            Dim dt As New DataTable
            Dim conn As New SqlConnection(sCon1)
            Dim sqlString As String = "select frozenFor,Balance,CreditLine from movesa..ocrd where CardCode = @p1"

            Dim command As New SqlCommand(sqlString, conn)
            command.Parameters.AddWithValue("@p1", cardcode)
            Dim drd As SqlDataReader
            conn.Open()
            drd = command.ExecuteReader()
            If drd.Read() Then

                If drd.Item("frozenFor").ToString = "Y" Then
                    txtEstatusCliente.Text = "BLOQUEADO"
                    txtEstatusCliente.Style("border") = "2px solid red"
                    'btnCrearSolicitud.Enabled = False
                Else
                    txtEstatusCliente.Text = "ACTIVO"
                    txtEstatusCliente.Style("border") = "2px solid green"
                End If

                txtLimiteCredito.Text = FormatNumber(drd.Item("CreditLine").ToString, 2, TriState.True)
                txtSaldoCuenta.Text = FormatNumber(drd.Item("Balance").ToString, 2, TriState.True)
            End If
            conn.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('TraerEstadoCliente: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub SaldoConsignacion(cardcode As String)
        Try
            Dim dt As New DataTable
            Dim _con As New SqlConnection(sCon1)
            Dim Consulta As String = "select isnull(SUM(CONVERT(numeric(18,2),T3.Price)),0) [Total]" &
                                            " From OWHS t0 " &
                                            " inner join OSRI t1 on t0.WhsCode = t1.WhsCode" &
                                            " inner Join OCRD t2 on t0.U_CardCode = t2.CardCode" &
                                            " INNER JOIN OSPP T3 ON T3.ItemCode = T1.ItemCode AND T3.CardCode = T2.CardCode" &
                                            " INNER Join OITM T4 ON T4.ItemCode = T1.ItemCode " &
                                            " LEFT JOIN [@AMODELO] T5 ON T4.U_AMODELO = T5.Code" &
                                            " Left Join(SELECT  distinct t1.ItemCode, t0.DocDate, t0.BaseNum, t1.MnfSerial, t1.SysNumber" &
                                            " FROM SRI1  t0 " &
                                            " inner Join OSRN t1 on t0.SysSerial=t1.SysNumber And t0.ItemCode = t1.ItemCode And t0.BaseType = 67" &
                                            " WHERE  t0.LineNum = (select  max(LineNum) from SRI1  tt0 " &
                                            " inner Join OSRN tt1 on tt0.SysSerial=tt1.SysNumber And tt0.BaseType = 67 " &
                                            " where tt0.ItemCode = t0.ItemCode and tt1.SysNumber = t1.SysNumber)  ) t12 on t12.ItemCode = t1.ItemCode and  t12.MnfSerial = t1.SuppSerial" &
                                            " where t2.CardCode = @p1" &
                                            " And T1.[Status] = 0"

            Dim cmd As New SqlCommand(Consulta, _con)
            cmd.Parameters.AddWithValue("@p1", cardcode)
            Dim drd As SqlDataReader
            _con.Open()
            drd = cmd.ExecuteReader()
            If drd.Read() Then
                txtSaldoConsignacion.Text = FormatNumber(drd.Item("Total").ToString, 2, TriState.True)
            End If
            _con.Close()
        Catch ex As Exception
            Response.Write("<script>console.log('TraerEstadoCliente: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub

    Private Sub btnCargaGeneralRuta_Click(sender As Object, e As EventArgs) Handles btnCargaGeneralRuta.Click
        Try
            lblCanal.Text = "ALL RUTA"
            lblAlmacen.Text = drpRutaLogica.SelectedValue.ToString
            lblSupervisor.Text = drpRutaLogica.SelectedItem.Text
            BindGridClientesParetoAllRuta(drpRutaLogica.SelectedValue.ToString)

            gridCuadroBasico.DataSource = Nothing
            gridCuadroBasico.DataBind()

            gridPedidoTemp.DataSource = Nothing
            gridPedidoTemp.DataBind()

            lblCorreo.Text = getEmail(drpOptions.SelectedValue.ToString)

        Catch ex As Exception
            Response.Write("<script>console.log('btnCargaGeneral_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try

    End Sub
    Private Sub btnEliminarPedidoTemp_Click(sender As Object, e As EventArgs) Handles btnEliminarPedidoTemp.Click
        Try
            For Each row As GridViewRow In gridPedidoTemp.Rows
                EliminarLinea(row.Cells(0).Text)
            Next
            BindGrid(lblWhscode.Text)
            BindgridSugerido(lblWhscode.Text, drpOptions.SelectedValue.ToString)
        Catch ex As Exception
            Response.Write("<script>console.log('btnEliminarPedidoTemp_Click " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    Private Sub btnCerrarModal_Click(sender As Object, e As EventArgs) Handles btnCerrarModal.Click
        Try
            If lblCanal.Text = "ALL" Then
                BindGridClientesParetoAll(lblAlmacen.Text)
            Else
                BindGridClientesPareto(lblCanal.Text, lblAlmacen.Text)
            End If
            BindGrid(lblWhscode.Text)

            BindgridSugerido(lblWhscode.Text, drpOptions.SelectedValue.ToString)
            BuscarSerie(lblWhscode.Text)


            For Each row As GridViewRow In gridParetoClientes.Rows
                If row.Cells(1).Text = lblWhscode.Text Then
                    row.Cells(0).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(1).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(2).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(3).BackColor = System.Drawing.Color.LightGreen
                    row.Cells(4).BackColor = System.Drawing.Color.LightGreen
                    lblRowIndex.Text = row.Cells(0).Text
                End If
            Next
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub btnPlanidManual_Click(sender As Object, e As EventArgs) Handles btnPlanidManual.Click
        Try
            Dim planid As String = txtPlanidManual.Text.Trim()

            ' Usar la URL actual con parámetros (puedes ajustarla si es una URL base fija)
            Dim urlCompleto As String = Request.RawUrl

            ' Si ya tiene parámetros, usar "&", de lo contrario usar "?"
            Dim separador As String = If(urlCompleto.Contains("?"), "&", "?")

            ' Redireccionar con el parámetro planid agregado
            Response.Redirect(urlCompleto & separador & "planId=" & Server.UrlEncode(planid))
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class

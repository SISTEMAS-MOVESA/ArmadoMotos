Imports System.Data
Imports System.Data.SqlClient
Partial Class SupervisoresCuadroBasico
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String

    Private Sub SupervisoresCuadroBasico_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    CargarSucursales()
                    Dim whscode As String = Request.QueryString("whscode")

                    If Not String.IsNullOrEmpty(whscode) Then
                        drpSucursales.SelectedValue = whscode
                        BindGrid(drpSucursales.SelectedValue.ToString)
                    Else

                    End If

                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('TrasladosCuadroBasico_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Sub CargarSucursales()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = " SELECT WhsName,WhsCode FROM OWHS WITH(NOLOCK) WHERE WhsCode NOT LIKE 'TSUC%'   " &
                                        " AND WhsCode NOT IN ('BPD01','CDM00','CDR00','CDR01','CONCA01','Consumir','DCM00','DCR00',   " &
                                        " 'DCR01','DEV01','DEV02','EST001','FCE01','MAY01','MLCB01','MLCB02','MOB01','MSPS01','MSPS02',   " &
                                        " 'MSPS03','MSPS04','MSUC01','MSUC02','MSUC03','MSUC04','MSUC05','MSUC07','MTGA01','MTRANS00',   " &
                                        " 'RLCB02','RLCB03','RSPS02','RSPS03','RSPS04','RSPS05','RSPS06','RSPS07','RTGA01',   " &
                                        " 'RTGA0102','SUC0102','SUC0201','SUC03','SUC0301','SUC0401','SUC0701','SUC0801','SUM001') "

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

    Private Sub btnCargarSugerido_Click(sender As Object, e As EventArgs) Handles btnCargarSugerido.Click
        Try
            If Session("Name") Is vbNullString Then
                Response.Redirect("Default.aspx")
            Else
                BindGrid(drpSucursales.SelectedValue.ToString)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('btnCargarSugerido_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Private Sub BindGrid(whscode As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String

                Dim sucType As String = getWhscodeType(drpSucursales.SelectedValue.ToString())
                If sucType = "PRO" Then
                    sql_string = "EXEC [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS] '" & drpSucursales.SelectedValue.ToString & "'"
                Else sucType = "S"
                    sql_string = "EXEC [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS_I] '" & drpSucursales.SelectedValue.ToString & "'"
                End If

                Using cmd As New SqlCommand(sql_string)
                    'Using cmd As New SqlCommand("EXEC [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS] '" & drpSucursales.SelectedValue.ToString & "'")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridCuadroBasico.DataSource = dt
                            gridCuadroBasico.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridCuadroBasico.UseAccessibleHeader = True
            gridCuadroBasico.HeaderRow.TableSection = TableRowSection.TableHeader


            For Each row As GridViewRow In gridCuadroBasico.Rows

                row.Cells(17).Text = CInt(row.Cells(16).Text) + CInt(row.Cells(15).Text) + CInt(row.Cells(14).Text) + CInt(row.Cells(13).Text)

                If row.Cells(12).Text > row.Cells(11).Text Then
                    row.Cells(12).ToolTip = "Venta Mayor al Cuadro Basico!!"
                    row.Cells(12).BackColor = System.Drawing.Color.Orange

                ElseIf row.Cells(12).Text < row.Cells(11).Text Then
                    row.Cells(12).ToolTip = "Venta Menor al Cuadro Basico!!"
                    row.Cells(12).BackColor = System.Drawing.Color.Red

                End If

                If row.Cells(10).Text < row.Cells(11).Text Then
                    row.Cells(10).ToolTip = "Fisico Menor a CB!!"
                    row.Cells(10).BackColor = System.Drawing.Color.Yellow
                    row.Cells(10).BackColor = System.Drawing.Color.Yellow
                End If
            Next

        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

        End Try
    End Sub
    Public Function getWhscodeType(whscode As String) As String
        Dim sCon As String = sCon1
        Dim sel As String
        sel = "SELECT [u_type] FROM OWHS where WHSCODE = @whscode"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            cmd.Parameters.AddWithValue("@whscode", whscode)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()
            Return t
            Response.Write("<script>console.log('getWhscodeType: " & ReplaceCharsForFileName(t, " ") & "');</script>")
        End Using
    End Function
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
                Dim url As String = "TrasladosCargaMacroBI.aspx?modelo=" & modelo
                Dim script As String = "window.open('" & url & "', '_blank');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenNewWindow", script, True)
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('gridCuadroBasico_RowCommand: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function InsertSugeridoSucursales(RUTA As String, ALMORIGEN As String, ALMDESTINO As String, CARDCODE As String, ARTICULO As String,
        MODELO As String, DESCRIPCION As String, ESPACIOS As String, CANTIDAD As String, QTYLOGISTICA As String, QTYSUCURSAL As String, OBSERVACIONES As String) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[MACROINSERT]([RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],[ARTICULO],[MODELO]" &
                ",[DESCRIPCION],[ESPACIOS],[CANTIDAD],[QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[ESTADO])" &
                " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13); SELECT SCOPE_IDENTITY()"
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
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('InsertSugeridoSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
            Return 0
        End Try
    End Function

    Public Function MacroInsert_Header(ORIGEN As String, DESTINO As String, RUTA As String, ESTADO As String, DATECREATED As DateTime, USERCREATED As String) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "INSERT INTO [dbo].[MACROINSERT_HEADER]([ORIGEN],[DESTINO],[RUTA],[ESTADO],[DATECREATED],[USERCREATED])" &
                " VALUES(@p1,@p2,@p3,@p4,@p5,@p6); SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", ORIGEN)
                cmd.Parameters.AddWithValue("@p2", DESTINO)
                cmd.Parameters.AddWithValue("@p3", RUTA)
                cmd.Parameters.AddWithValue("@p4", ESTADO)
                cmd.Parameters.AddWithValue("@p5", DATECREATED)
                cmd.Parameters.AddWithValue("@p6", USERCREATED)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('MacroInsert_Header: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
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

    'Private Sub btnNuevaSucursal_Click(sender As Object, e As EventArgs) Handles btnNuevaSucursal.Click
    '    Try
    '        txtCodidoAlm.Text = ""
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#modalAdd').modal('show');</script>", False)
    '    Catch ex As Exception
    '        Response.Write("<script>console.log('btnNuevaSucursal_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub
    Public Function CrearAlmacenNuevo(whscode As String) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "insert into [LOGISTICA_MAX_INV](WHSCODE,MODELO,MAXIMO) " &
                    "SELECT distinct @p1,[MODELO],1 FROM [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV]; SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", whscode)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('MacroInsert_Header: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    'Private Sub btnNuevoModelo_Click(sender As Object, e As EventArgs) Handles btnNuevoModelo.Click
    '    Try
    '        txtModelo.Text = ""
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ModalView", "<script>$('#modalAdd_modelo').modal('show');</script>", False)
    '    Catch ex As Exception
    '        Response.Write("<script>console.log('btnNuevaSucursal_Click: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub
    Public Function CrearModeloNuevo(modelo As String) As Integer
        Try
            Dim retrnvalue As Integer
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "insert into [LOGISTICA_MAX_INV](WHSCODE,MODELO,MAXIMO) " &
                    "SELECT distinct WHSCODE,@p1,0 from logistica_max_inv; SELECT SCOPE_IDENTITY()"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", modelo)
                con.Open()
                retrnvalue = CInt(cmd.ExecuteScalar())
                con.Close()
                Return retrnvalue
            End Using
        Catch ex As Exception
            Response.Write("<script>console.log('MacroInsert_Header: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function

    Private Sub btnIndiceGeneral_Click(sender As Object, e As EventArgs) Handles btnIndiceGeneral.Click
        Response.Redirect("SupervisoresIndiceGeneral.aspx")
    End Sub

    Private Sub btnCalendario_Click(sender As Object, e As EventArgs) Handles btnCalendario.Click
        Try
            Response.Redirect("SupervisoresCalendarioDespachos.aspx")
        Catch ex As Exception

        End Try
    End Sub
End Class

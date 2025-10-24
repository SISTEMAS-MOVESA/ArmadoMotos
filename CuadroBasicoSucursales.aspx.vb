Imports System.Data
Imports System.Data.SqlClient

Partial Class CuadroBasicoSucursales
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public csvFilePath As String

    Private Sub CuadroBasicoSucursales_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'CargarSucursales()
                    BindGrid(Session("ALMTRANSIT"))
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("<script>console.log('TrasladosCuadroBasico_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

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
    Private Sub BindGrid(whscode As String)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Dim sql_string As String
                sql_string = "EXEC [dbo].[SP_LOGISTICA_PARETOMOTOS_BYWHS_I] '" & whscode & "'"
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

                row.Cells(20).Text = CInt(row.Cells(10).Text) + CInt(row.Cells(11).Text) + CInt(row.Cells(18).Text) + CInt(row.Cells(19).Text)

                If row.Cells(15).Text > 0 Then
                    row.Cells(15).BackColor = System.Drawing.Color.Orange
                    row.Cells(15).ToolTip = "Cantidad a Pedir!!"

                ElseIf row.Cells(15).Text < 0 Then
                    row.Cells(15).ToolTip = "Sugerido Mayor al Cuadro Basico!!"
                    row.Cells(15).BackColor = System.Drawing.Color.Red
                End If

                If row.Cells(16).Text > row.Cells(10).Text Then
                    row.Cells(10).ToolTip = "Cuadro Basico Menor a la Venta Actual!!"
                    row.Cells(16).ToolTip = "Solicite Aumento al Cuadro Basico!!"
                    row.Cells(10).BackColor = System.Drawing.Color.Red
                    row.Cells(16).BackColor = System.Drawing.Color.Red
                End If
                'If row.Cells(22).Text > 0 Then
                '    row.Cells(22).BackColor = System.Drawing.Color.LightGreen
                'End If
            Next

        Catch ex As Exception
            Response.Write("<script>console.log('BindGrid: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")

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
            Response.Write("gridCuadroBasico_RowCommand " & ex.Message)
        End Try
    End Sub
End Class

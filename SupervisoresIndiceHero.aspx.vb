Imports System.Data
Imports System.Data.SqlClient
Partial Class SupervisoresIndiceHero
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public almacen As String

    Private Sub SupervisoresIndiceHero_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGrid()
                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
                    Else
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write("TrasladosDashboard_Load " & ex.Message)
        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)

                ' Leer el contenido del archivo .sql
                Dim SQL_string As String = System.IO.File.ReadAllText(Server.MapPath("~/Querys/indiceHero.sql"))
                Using cmd As New SqlCommand(SQL_string, con)

                    Using sda As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        gridIndiceD.DataSource = dt
                        gridIndiceD.DataBind()
                    End Using
                End Using
            End Using
            gridIndiceD.UseAccessibleHeader = True
            gridIndiceD.HeaderRow.TableSection = TableRowSection.TableHeader

            Dim tempSumatoria As Integer = 0
            For Each row As GridViewRow In gridIndiceD.Rows
                tempSumatoria = CInt(row.Cells(12).Text) - CInt(row.Cells(13).Text) - CInt(row.Cells(20).Text)

                ' Corrige las celdas de porcentaje: si es negativo, pon 0.00%
                For i As Integer = 8 To 10
                    Dim valor As Decimal
                    If Decimal.TryParse(row.Cells(i).Text.Replace("%", "").Trim, valor) Then
                        If valor < 0 Then
                            row.Cells(i).Text = "0.00 %"
                        Else
                            row.Cells(i).Text = FormatNumber(valor, 2) & " %"
                        End If
                    End If
                Next

                'If CDec(row.Cells(12).Text) <> 0 Then
                '    row.Cells(11).Text = FormatPercent(IIf((CDec(tempSumatoria) / CDec(row.Cells(12).Text)) < 0, 0, (CDec(tempSumatoria) / CDec(row.Cells(12).Text))), 2, TriState.True)
                'Else
                '    row.Cells(11).Text = FormatPercent(0, 2, TriState.True)
                'End If
            Next


        Catch ex As Exception
            Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub
    Protected Sub gridPlanificacionTemp_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim valorPlanificacion As String = e.Row.Cells(5).Text.Trim()
                For Each rowIndiceD As GridViewRow In gridIndiceD.Rows
                    Dim valorIndice As String = rowIndiceD.Cells(6).Text.Trim()
                    Dim cb As CheckBox = CType(rowIndiceD.FindControl("cbDocument"), CheckBox)




                    If valorPlanificacion.Trim = valorIndice.Trim Then

                        cb.Visible = False
                        Dim checkLabel As New Label()
                        checkLabel.Text = "<i class='fa-solid fa-check fa-2x text-success text-center'></i>"
                        rowIndiceD.Cells(23).Controls.Add(checkLabel)

                        'Dim btnRedirect As Button = CType(rowIndiceD.FindControl("btnRedirect"), Button)



                        'If btnRedirect IsNot Nothing Then
                        '    btnRedirect.Visible = False
                        '    Dim checkLabel As New Label()
                        '    checkLabel.Text = "<i class='fa-solid fa-check fa-2x text-success text-center'></i>"
                        '    rowIndiceD.Cells(16).Controls.Add(checkLabel)
                        'End If
                    End If
                Next
            End If
        Catch ex As Exception
            'Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Protected Sub gridIndiceD_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.Add("data-merge", "true")
            e.Row.Cells(1).Attributes.Add("data-merge", "true")
            e.Row.Cells(2).Attributes.Add("data-merge", "true")
            e.Row.Cells(3).Attributes.Add("data-merge", "true")
        End If
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
        sName = Replace(sName, ",", sChr)
        NewStr = sName
        Return NewStr

    End Function

    Private Sub MergeRows(grid As GridView, column As String)
        Dim columnIndex As Integer = GetColumnIndexByName(grid, column)
        If columnIndex = -1 Then Exit Sub

        Dim previousCell As TableCell = Nothing
        Dim rowSpanCount As Integer = 1

        For rowIndex As Integer = 0 To grid.Rows.Count - 1
            Dim currentRow As GridViewRow = grid.Rows(rowIndex)
            Dim currentCell As TableCell = currentRow.Cells(columnIndex)

            If previousCell IsNot Nothing AndAlso currentCell.Text = previousCell.Text Then
                rowSpanCount += 1
                previousCell.RowSpan = rowSpanCount
                currentCell.Visible = False
            Else
                previousCell = currentCell
                rowSpanCount = 1
            End If
        Next
    End Sub
    Private Function GetColumnIndexByName(grid As GridView, columnName As String) As Integer
        For i As Integer = 0 To grid.Columns.Count - 1
            If grid.Columns(i).HeaderText = columnName Then
                Return i
            End If
        Next
        Return -1
    End Function
    Public Sub EliminarLinea(idlinea As Integer)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "DELETE FROM [PLANIFICACIONES_DETALLE] where [ID] = @p1"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", idlinea)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("EliminarLinea " & ex.Message)
        End Try
    End Sub
    Private Sub btnCalendario_Click(sender As Object, e As EventArgs) Handles btnCalendario.Click
        Try
            Response.Redirect("SupervisoresCalendarioDespachos.aspx")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnCuadroBasico_Click(sender As Object, e As EventArgs) Handles btnCuadroBasico.Click
        Try
            Response.Redirect("SupervisoresCuadroBasico.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub btnIndiceHero_Click(sender As Object, e As EventArgs) Handles btnIndiceHero.Click
        Try
            Response.Redirect("SupervisoresIndiceHero.aspx")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnIndiceGeneral_Click(sender As Object, e As EventArgs) Handles btnIndiceGeneral.Click
        Try
            Response.Redirect("SupervisoresIndiceGeneral.aspx")
        Catch ex As Exception

        End Try
    End Sub
End Class

Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System
Imports System.IO
Imports System.Data.OleDb
Imports System.Web.Hosting
Imports System.Collections.Generic
Imports ClosedXML.Excel
Partial Class AutorizacionCreditos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public Sub LoadGridAUtorizacionCreditos()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)


                Using cmd As New SqlCommand("SELECT [ID],[CODIGO],[NOMBRE] FROM [ArmadoMotos].[dbo].[BLACKLIST]")

                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridPlanificacion.DataSource = dt
                            gridPlanificacion.DataBind()
                            gridPlanificacion.UseAccessibleHeader = True
                            gridPlanificacion.HeaderRow.TableSection = TableRowSection.TableHeader
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
        Catch ex As Exception
            'Response.Write("LoadGridPlanificacion " & ex.Message)
        End Try
    End Sub

    Private Sub AutorizacionCreditos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
                    LoadGridAUtorizacionCreditos()
                End If
            Else
            End If
        Catch ex As Exception
            Response.Write("OrdendeTrabajo_Load " & ex.Message)
        End Try
    End Sub

    Private Sub gridPlanificacion_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPlanificacion.RowCommand
        Try
            If e.CommandName = "Autorizar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gridPlanificacion.Rows(index)

                Dim strSQL As String = "update [dbo].[PLAN_ARMADO] SET APROBADOC='Y' where CODIGOCLIENTE='" & gridPlanificacion.Rows(index).Cells(2).Text & "';"
                Using conn As New SqlConnection(sCon2)
                    Using cmdSQL As New SqlCommand(strSQL, conn)
                        conn.Open()
                        cmdSQL.ExecuteNonQuery()
                        LoadGridAUtorizacionCreditos()
                        conn.Close()
                    End Using
                End Using

            End If
        Catch ex As Exception
            Response.Write("gridPlanificacion_RowCommand " & ex.Message)
        End Try
    End Sub
    Protected Sub ImportExcel(sender As Object, e As EventArgs)
        'Save the uploaded Excel file.
        Dim filePath As String = HostingEnvironment.ApplicationPhysicalPath & Path.GetFileName(FileUpload1.PostedFile.FileName)
        FileUpload1.SaveAs(filePath)

        'Open the Excel file using ClosedXML.
        Using workBook As New XLWorkbook(filePath)
            'Read the first Sheet from Excel file.
            Dim workSheet As IXLWorksheet = workBook.Worksheet(1)

            'Create a new DataTable.
            Dim dt As New DataTable()

            'Loop through the Worksheet rows.
            Dim firstRow As Boolean = True
            For Each row As IXLRow In workSheet.Rows()
                'Use the first row to add columns to DataTable.
                If firstRow Then
                    For Each cell As IXLCell In row.Cells()
                        dt.Columns.Add(cell.Value.ToString())
                    Next
                    firstRow = False
                Else
                    'Add rows to DataTable.
                    dt.Rows.Add()
                    Dim i As Integer = 0
                    For Each cell As IXLCell In row.Cells()
                        dt.Rows(dt.Rows.Count - 1)(i) = cell.Value.ToString()
                        i += 1
                    Next
                End If

                GridView1.DataSource = dt
                GridView1.DataBind()
            Next
        End Using
    End Sub
    Public Sub CreateBlackList(CODIGOCLIENTE As String, NOMBRECLIENTE As String)
        Try
            Dim sCon As String = sCon2
            Dim sel As String
            sel = "delete from [dbo].[BLACKLIST] where [codigo]=@p1; INSERT INTO [dbo].[BLACKLIST]([CODIGO],[NOMBRE]) VALUES (@p1,@p2);"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", CODIGOCLIENTE)
                cmd.Parameters.AddWithValue("@p2", NOMBRECLIENTE)
                con.Open()
                cmd.ExecuteScalar()
                con.Close()
            End Using
        Catch ex As Exception
            Response.Write("CreateBlackList " & ex.Message)
        End Try
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Try
            For Each row As GridViewRow In GridView1.Rows
                CreateBlackList(row.Cells(0).Text, row.Cells(1).Text)
                row.Cells(0).BackColor = System.Drawing.Color.Green
            Next
        Catch ex As Exception
            Response.Write("btnActualizar_Click " & ex.Message)
        End Try

    End Sub
End Class

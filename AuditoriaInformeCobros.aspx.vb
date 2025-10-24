Imports System.Data
Imports System.Data.SqlClient
Partial Class AuditoriaInformeCobros
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Private Sub AuditoriaInformeCobros_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindGrid()
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindGrid()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("select FECHA[Fecha],CARDCODE[Codigo],CARDNAME[Cliente],ITEMCODE[Articulo],ITENMANE[Desripcion],TOTALLINEA[Total Linea], " &
                                            " DOCENTRYSAP[Entry Sap],ENVIOSAP[Fecha Creacion SAP],ESTATUS[Estatus] from AUDITORIA_COBROS")
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
            'Response.Write("BindGrid " & ex.Message)
        End Try
    End Sub
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            BindGridByDate(txtDesde.Text, txtHasta.Text)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindGridByDate(desde As DateTime, hasta As DateTime)
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("select FECHA[Fecha],CARDCODE[Codigo],CARDNAME[Cliente],ITEMCODE[Articulo],ITENMANE[Desripcion],TOTALLINEA[Total Linea], " &
                                            " DOCENTRYSAP[Entry Sap],ENVIOSAP[Fecha Creacion SAP],ESTATUS[Estatus] from AUDITORIA_COBROS where FECHA BETWEEN @d1 AND @d2", con)
                    ' Agregar los parámetros a la consulta
                    cmd.Parameters.AddWithValue("@d1", desde)
                    cmd.Parameters.AddWithValue("@d2", hasta)

                    Using sda As New SqlDataAdapter(cmd)
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            GridView1.DataSource = dt
                            GridView1.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using

            ' Configurar el GridView
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            'Response.Write("BindGridByDate " & ex.Message)
        End Try
    End Sub


    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Try
            Response.Redirect("AuditoriaInformeCobros.aspx")
        Catch ex As Exception
            Response.Write("btnReset_Click " & ex.Message)

        End Try
    End Sub
End Class

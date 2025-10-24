Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Partial Class ListadoMotos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.60;database=PRUEBAS;uid=sa;password=S@pB1Sql"

    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
    End Sub

    Public Sub ADD_LOG(_PROCESO As String, _ERROR As String)
        Try
            Dim sCon As String = sCon1
            Dim sel As String
            sel = "INSERT INTO [MovesaWeb].[dbo].[PortalSKG] ([FECHA],[PROCESO],[ERROR])" &
                   " VALUES (@p1,@p2,@p3)"
            Using con As New SqlConnection(sCon)
                Dim cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", Date.Now)
                cmd.Parameters.AddWithValue("@p2", _PROCESO)
                cmd.Parameters.AddWithValue("@p3", _ERROR)
                con.Open()
                Dim t As Integer = cmd.ExecuteNonQuery()
                con.Close()
            End Using
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ListadoMotos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'GESTORC
            If Session("Position").ToString = "GESTORC" Then
                pnlSubMenuOrdenRecuperacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = False
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            'SKA1001
            If Session("Position").ToString = "ADMIN" Then
                pnlSubMenuConfirmacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = False
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If
            'VENDEDOR
            If Session("Position").ToString = "VENDEDOR" Then
                pnlSubMenuConfirmacion.Visible = False
                pnlSubMenuOrdenRecuperacion.Visible = False
                pnlSubMenuConfirmacionVenta.Visible = True
                pnlSubMenuPrecios.Visible = False
                pnlSubMenuReconciliaciones.Visible = False
            End If

            If String.IsNullOrEmpty(Session("Name").ToString()) Then
                Response.Redirect("Default.aspx")
            End If

            If Not Me.IsPostBack Then
                Dim constr As String = sCon2
                Using conn As SqlConnection = New SqlConnection(constr)
                    Using sda As SqlDataAdapter = New SqlDataAdapter("select distinct t0.ItemCode,t0.ItemName,t0.SuppSerial,t0.WhsCode,t0.U_PrecioMat,t0.U_PrecioVenta " &
                            ", t1.U_SCOLOR, t1.U_SAno, t1.U_NPoliza, t1.U_NItem, t1.U_NAduana, t1.U_FPago " &
                            ", FORMAT(t1.U_PrecioMat, 'C', 'es-HN')[U_PrecioMat],FORMAT(t1.U_PrecioVenta, 'C', 'es-HN')[U_PrecioVenta] " &
                            " from [OSRI] t0 With(nolock) " &
                            " inner join  " &
                            " [OSRN] t1 With(nolock) " &
                            " on t0.SuppSerial=t1.MnfSerial " &
                            " where t0.Status=0", conn)
                        Dim dt As DataTable = New DataTable()
                        sda.Fill(dt)

                        gvImages.DataSource = dt
                        gvImages.DataBind()
                        gvImages.UseAccessibleHeader = True
                        gvImages.HeaderRow.TableSection = TableRowSection.TableHeader
                    End Using
                End Using
            End If
            lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString
        Catch ex As Exception
            ADD_LOG("ListadoMotos_Load", ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub gvImages_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvImages.RowCommand
        Try 
            If e.CommandName = "Agregar" Then
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim gvRow As GridViewRow = gvImages.Rows(index)
                Session("SerialNumberPhotos") = gvImages.Rows(index).Cells(3).Text
                Response.Redirect("ExpedienteVehiculo.aspx")
            End If
            'If e.CommandName = "Compras" Then 
            'End If
        Catch ex As Exception
            ADD_LOG("gvImages_RowCommand", ex.Message)
        End Try
    End Sub

    Public Sub CargarMotoUnica(_Serie As String)
        Dim constr As String = sCon2
        Using conn As SqlConnection = New SqlConnection(constr)
            Using sda As SqlDataAdapter = New SqlDataAdapter("select distinct t0.ItemCode, t0.ItemName, t0.SuppSerial, t0.WhsCode, t0.U_PrecioMat, t0.U_PrecioVenta " &
                    ", t1.U_SCOLOR, t1.U_SAno, t1.U_NPoliza, t1.U_NItem, t1.U_NAduana, t1.U_FPago " &
                    ", Format(t1.U_PrecioMat, 'C', 'es-HN')[U_PrecioMat],FORMAT(t1.U_PrecioVenta, 'C', 'es-HN')[U_PrecioVenta] " &
                    " from [OSRI] t0 With(nolock) " &
                    " inner join  " &
                    " [OSRN] t1 With(nolock) " &
                    " on t0.SuppSerial=t1.MnfSerial " &
                    " where t0.SuppSerial='" & _Serie & "'", conn)
                Dim dt As DataTable = New DataTable()
                sda.Fill(dt)

                gvImages.DataSource = dt
                gvImages.DataBind()
                gvImages.UseAccessibleHeader = True
                gvImages.HeaderRow.TableSection = TableRowSection.TableHeader
            End Using
        End Using
    End Sub
End Class

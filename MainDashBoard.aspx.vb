Imports System.Data
Imports System.Data.SqlClient

Partial Class MainDashBoard
    Inherits System.Web.UI.Page

    Private Sub MainDashBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
                    End If
                    lblNombreUsuario.Text = "Conectado como: " & Session("Name").ToString()
                    CantidadModelosPrecioLabel()
                    CantidadMotosDesarmadas()
                    CantidadControlCalidad()
                    CantidadMotosDisponibles()
                    CantidadMotosNoDisponibles()
                    GetData(DatePart(DateInterval.Year, Date.Now), DatePart(DateInterval.Month, Date.Now))
                End If
            End If
        Catch ex As Exception
            Response.Write("MainDashBoard_Load " & ex.Message)
        End Try
    End Sub

    Public Sub CantidadModelosPrecioLabel()
        Try
            Dim sql As String = "SELECT COUNT(1) AS Cantidad FROM [dbo].[ARMADOMOTOS] WITH(NOLOCK) WHERE CANCELED='N' AND [ESTATUS] IN ('Asignada','Proceso','Reproceso')"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                lblMotosEnProceso.Text = dt.Rows(0)("Cantidad").ToString()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub CantidadMotosDesarmadas()
        Try
            Dim sql As String = "SELECT COUNT(1) AS Cantidad FROM osri WITH(NOLOCK) WHERE ISNULL(U_VIN,'-')<>'N' AND U_Estado_Produccion='01' AND WHSCODE='DCM00' AND Status=0"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                lblMotosEnCaja.Text = dt.Rows(0)("Cantidad").ToString()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub CantidadControlCalidad()
        Try
            Dim sql As String = "SELECT COUNT(1) AS Cantidad FROM [dbo].[ARMADOMOTOS] WITH(NOLOCK) WHERE [ESTATUS] IN ('Calidad')"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                lblControlCalidad.Text = dt.Rows(0)("Cantidad").ToString()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub CantidadMotosDisponibles()
        Try
            Dim sql As String = "SELECT COUNT(1) AS Cantidad FROM osri WITH(NOLOCK) WHERE U_Estado_Produccion='04' AND Status=0 AND WhsCode='DCM00'"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                lblMotosDisponibles.Text = dt.Rows(0)("Cantidad").ToString()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub CantidadMotosNoDisponibles()
        Try
            Dim sql As String = "SELECT COUNT(1) AS Cantidad FROM osri WITH(NOLOCK) WHERE U_Estado_Produccion='05'"
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.MOVESA)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                txtMotosNoDisponibles.Text = dt.Rows(0)("Cantidad").ToString()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub GetData(year As Integer, month As Integer)
        Try
            Dim sql As String = "EXEC PIVOT_ARMADO_MENSUAL " & year & "," & month
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
            If dt Is Nothing Then Return
            GridView1.DataSource = dt
            GridView1.DataBind()
            GridView1.UseAccessibleHeader = True
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
        End Try
    End Sub

End Class

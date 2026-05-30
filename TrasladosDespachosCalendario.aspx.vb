Imports System.Data
Imports System.Data.SqlClient

Partial Class TrasladosDespachosCalendario
    Inherits System.Web.UI.Page

    Private Sub TrasladosDespachosCalendario_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("Name") Is vbNullString Then Response.Redirect("Default.aspx")
                If Session("USERROL") = "Jefe Tienda" Then Response.Redirect("MaindashboardSucursales.aspx")
            End If
        Catch ex As Exception
            Response.Write("TrasladosDespachosCalendario_Load: " & ex.Message)
        End Try
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerEventos() As Object
        Dim eventos As New List(Of Object)
        Dim sql As String =
            "SELECT t0.ID, t0.PLANID, t0.FECHAVENCE, t1.RUTA, t1.ALMDESTINO, " &
            "(SELECT whsname FROM movesa..owhs WHERE whscode=T1.ALMDESTINO COLLATE Modern_Spanish_CI_AS) [Nalmacen], " &
            "SUM(t1.ESPACIOS) AS TotalEspacios, SUM(t1.CANTIDAD) AS TotalCantidad " &
            "FROM [ArmadoMotos].[dbo].[DEPACHOS_HEADER] t0 WITH(NOLOCK) " &
            "INNER JOIN [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] t1 WITH(NOLOCK) ON t0.ID=t1.HEADERID " &
            "WHERE t0.ESTADO='ABIERTO' " &
            "GROUP BY t0.ID, t0.PLANID, t0.FECHAVENCE, t1.RUTA, t1.ALMDESTINO " &
            "ORDER BY t0.FECHAVENCE"
        Try
            Dim dt As DataTable = DbConfig.GetDataTable(sql, DbConfig.DBServer.ARMADOMOTOS)
            For Each row As DataRow In dt.Rows
                Dim ruta As String = If(IsDBNull(row("RUTA")), "", row("RUTA").ToString().Trim())
                Dim alm As String = If(IsDBNull(row("ALMDESTINO")), "", row("ALMDESTINO").ToString().Trim())
                Dim nAlm As String = If(IsDBNull(row("Nalmacen")), "", row("Nalmacen").ToString().Trim())
                Dim cant As Integer = If(IsDBNull(row("TotalCantidad")), 0, Convert.ToInt32(row("TotalCantidad")))
                Dim esp As Integer = If(IsDBNull(row("TotalEspacios")), 0, Convert.ToInt32(row("TotalEspacios")))
                Dim planid As String = row("PLANID").ToString()
                Dim id As String = row("ID").ToString()
                eventos.Add(New With {
                    .title = ruta & " - " & alm & " (" & cant & " unds)",
                    .start = Convert.ToDateTime(row("FECHAVENCE")).ToString("yyyy-MM-dd"),
                    .allDay = True,
                    .id = id,
                    .planid = planid,
                    .ruta = ruta,
                    .almdestino = alm,
                    .nalmacen = nAlm,
                    .totalcantidad = cant,
                    .totalespacios = esp
                })
            Next
        Catch ex As Exception
        End Try
        Return eventos
    End Function

End Class

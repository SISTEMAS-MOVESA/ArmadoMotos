Imports System.Data
Imports System.Data.SqlClient
Partial Class SupervisoresCalendarioDespachos
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public almacen As String
    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerEventos() As Object
        Dim eventos As New List(Of Object)
        Dim connStr As String = sCon2

        Using con As New SqlConnection(connStr)
            Dim query As String = "SELECT t0.id, t1.despachoid [DespachoID],t0.PLANID,t0.FECHAVENCE,t1.ruta,t1.almdestino, " &
                " (select whsname from movesa..owhs where whscode= T1.ALMDESTINO collate Modern_Spanish_CI_AS) [Nalmacen]" &
                " ,SUM(t1.ESPACIOS) AS TotalEspacios, " &
                " SUM(t1.CANTIDAD) AS TotalCantidad " &
                " FROM [ArmadoMotos].[dbo].[DEPACHOS_HEADER] t0 WITH(NOLOCK) " &
                " INNER JOIN [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] t1 WITH(NOLOCK) " &
                " ON t0.ID = t1.HEADERID GROUP BY " &
                " t0.id, t1.despachoid, t0.PLANID, t0.FECHAVENCE, t1.ruta, t1.almdestino"

            Using cmd As New SqlCommand(query, con)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        eventos.Add(New With {
                        .title = "" & reader("ruta") & " - " & reader("almdestino") & "  " & reader("Nalmacen") &
                                 " (" & reader("TotalCantidad") & " unidades, " & reader("TotalEspacios") & " espacios ) , Planid: " & reader("PLANID") & " , DespachoID: " & reader("DespachoID") & " ",
                        .start = Convert.ToDateTime(reader("FECHAVENCE")).ToString("yyyy-MM-dd"),
                        .allDay = True,
                        .DespachoID = reader("DespachoID")
                    })
                    End While
                End Using
            End Using
        End Using

        Return eventos
    End Function
    Private Sub SupervisoresCalendarioDespachos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    'BindGrid()
                    'BindGridDespachosAbiertos()
                    'bindGridSugeridosAbiertos()

                    If Session("USERROL") = "Jefe Tienda" Then
                        Response.Redirect("MaindashboardSucursales.aspx")
                    Else

                        'If Not String.IsNullOrEmpty(Request.QueryString("planId")) Then
                        '    BindGridgridAlmacenesDespacho(Request.QueryString("planId"))
                        'End If
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub btnCuadroBasico_Click(sender As Object, e As EventArgs) Handles btnCuadroBasico.Click
        Try
            Response.Redirect("SupervisoresCuadroBasico.aspx")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnCalendario_Click(sender As Object, e As EventArgs) Handles btnCalendario.Click
        Try
            Response.Redirect("SupervisoresCalendarioDespachos.aspx")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnIndiceGeneral_Click(sender As Object, e As EventArgs) Handles btnIndiceGeneral.Click
        Try
            Response.Redirect("SupervisoresIndiceGeneral.aspx")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnIndiceHero_Click(sender As Object, e As EventArgs) Handles btnIndiceHero.Click
        Try
            Response.Redirect("SupervisoresIndiceHero.aspx")
        Catch ex As Exception

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
        sName = Replace(sName, ",", sChr)
        NewStr = sName
        Return NewStr

    End Function
End Class

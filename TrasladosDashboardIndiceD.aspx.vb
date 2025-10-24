Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json

Partial Class TrasladosDashboardIndiceD
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public SQL_STRING As String
    Public sql_consulta As String

    Private Sub TrasladosDashboardIndiceD_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If Session("Name") Is vbNullString Then
                    Response.Redirect("Default.aspx")
                Else
                    BindgridSugerido()
                End If
            Else
            End If
            lblDiunsa.Text = obtenerPedidosPendientesDiunsa()

        Catch ex As Exception
            Response.Write("<script>console.log('VisorLogistica_Load: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub
    Public Function obtenerPedidosPendientesDiunsa() As String
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "SELECT COUNT(DOCNUM)AS PEDIDOS_PENDIENTES FROM MOVESAWEB..TBL_REPORTES_VENTAS_DISTRIBUIDORES AS T0 WHERE T0.CARDCODE = 'CL104628' AND DOCTYPE = 'OP' AND T0.DOCSTATUS IN ('O')"
        Using con As New SqlConnection(sCon)
            Dim cmd As New SqlCommand(sel, con)
            con.Open()
            Dim t As String = cmd.ExecuteScalar()
            con.Close()

            Return t
        End Using
    End Function
    Private Sub BindgridSugerido()
        Try
            Dim constr As String = sCon2
            Using con As New SqlConnection(constr)

                SQL_STRING = "SELECT ROW_NUMBER() OVER (ORDER BY [Unds] desc) AS Ranking " &
                             " ,(SELECT U_CardCode FROM movesa..OWHS WHERE WhsCode = [RESUMEN_ALLWHS].Codigo COLLATE Modern_Spanish_CI_AS) AS [CodCli] " &
                             " ,(SELECT U_ZONA FROM movesa..OWHS WHERE WhsCode = [RESUMEN_ALLWHS].Codigo COLLATE Modern_Spanish_CI_AS) AS [Zona] " &
                             " ,(Select U_SLPNAME FROM movesa..[@CRESPONSABLEOWHS] With (NOLOCK) WHERE code = (Select U_Categorizacion FROM movesa..OWHS " &
                             " WHERE WhsCode = [RESUMEN_ALLWHS].Codigo COLLATE Modern_Spanish_CI_AS)) As [Sup] " &
                             " ,(select name from movesa..[@cruta] where code=(SELECT U_Ruta FROM movesa..OWHS WHERE WhsCode = [RESUMEN_ALLWHS].Codigo COLLATE Modern_Spanish_CI_AS)) AS [Ruta] " &
                             " ,[Codigo],[Almacen],[Cuadro],[Comprometido],[Solicitado],[Transito],[Fisico],[Faltante],[Unds] " &
                             " ,convert(decimal(19,2),CONVERT(DECIMAL(19,4), [Faltante]) / NULLIF(CONVERT(DECIMAL(19,4), [Cuadro]), 0) * 100) AS [70] " &
                             " ,isnull((select  convert(decimal(19,2),CONVERT(DECIMAL(19,4), [Faltante]) / NULLIF(CONVERT(DECIMAL(19,4), [Cuadro]), 0) * 100) " &
                             " FROM [ArmadoMotos].[dbo].[RESUMEN_ALLWHS_III] where Codigo=[ArmadoMotos].[dbo].[RESUMEN_ALLWHS].codigo),0)[1~40] " &
                             " ,isnull((select  convert(decimal(19,2),CONVERT(DECIMAL(19,4), [Faltante]) / NULLIF(CONVERT(DECIMAL(19,4), [Cuadro]), 0) * 100) " &
                             " FROM	[ArmadoMotos].[dbo].[RESUMEN_ALLWHS_II] where	Codigo=[ArmadoMotos].[dbo].[RESUMEN_ALLWHS].codigo),0)[41~70] " &
                             " ,isnull((SELECT sum([CANTIDAD])  FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] WHERE ALMDESTINO = [RESUMEN_ALLWHS].codigo AND CODIGOESTADO <> 'CERRADO' ),0)[Despacho] " &
                             " ,convert(decimal(19,2),CONVERT(DECIMAL(19,4), ([Faltante]-isnull((SELECT sum([CANTIDAD]) " &
                             " FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] WHERE ALMDESTINO = [RESUMEN_ALLWHS].codigo AND CODIGOESTADO <> 'CERRADO' ),0))) " &
                             " / NULLIF(CONVERT(DECIMAL(19,4), [Cuadro]), 0) * 100) AS [Indice_Proyectado] " &
                             " ,convert(char,Ultransfer,103) [Ultransfer] " &
                             " FROM [ArmadoMotos].[dbo].[RESUMEN_ALLWHS]  "


                'SQL_STRING = " SELECT ROW_NUMBER() OVER (ORDER BY [Unds] desc) AS Ranking,  " &
                '            " (SELECT U_CardCode FROM movesa..OWHS WHERE WhsCode = [RESUMEN_ALLWHS].Codigo COLLATE Modern_Spanish_CI_AS) AS [CodCli], " &
                '            " (SELECT U_ZONA FROM movesa..OWHS WHERE WhsCode = [RESUMEN_ALLWHS].Codigo COLLATE Modern_Spanish_CI_AS) AS [Zona],  " &
                '            " (Select U_SLPNAME FROM movesa..[@CRESPONSABLEOWHS] With (NOLOCK) WHERE code = (Select U_Categorizacion FROM movesa..OWHS " &
                '            " WHERE WhsCode = [RESUMEN_ALLWHS].Codigo COLLATE Modern_Spanish_CI_AS)) As [Sup],   " &
                '            " (select name from movesa..[@cruta] where code=(SELECT U_Ruta FROM movesa..OWHS WHERE WhsCode = [RESUMEN_ALLWHS].Codigo COLLATE Modern_Spanish_CI_AS)) AS [Ruta], " &
                '            " [Codigo],[Almacen],[Cuadro],[Comprometido],[Solicitado],[Transito],[Fisico],[Faltante],[Unds],    " &
                '            " convert(decimal(19,2),CONVERT(DECIMAL(19,4), [Faltante]) / NULLIF(CONVERT(DECIMAL(19,4), [Cuadro]), 0) * 100) AS [70] " &
                '            " ,isnull((select  convert(decimal(19,2),CONVERT(DECIMAL(19,4), [Faltante]) / NULLIF(CONVERT(DECIMAL(19,4), [Cuadro]), 0) * 100) " &
                '            " FROM [ArmadoMotos].[dbo].[RESUMEN_ALLWHS_II] where Codigo=[ArmadoMotos].[dbo].[RESUMEN_ALLWHS].codigo),0)[41~70] " &
                '            " ,isnull((select  convert(decimal(19,2),CONVERT(DECIMAL(19,4), [Faltante]) / NULLIF(CONVERT(DECIMAL(19,4), [Cuadro]), 0) * 100) " &
                '            " FROM [ArmadoMotos].[dbo].[RESUMEN_ALLWHS_III] where Codigo=[ArmadoMotos].[dbo].[RESUMEN_ALLWHS].codigo),0)[1~40] " &
                '            " ,convert(char,Ultransfer,103) [Ultransfer]   " &
                '            " FROM [ArmadoMotos].[dbo].[RESUMEN_ALLWHS]  "

                Using cmd As New SqlCommand(SQL_STRING)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            gridIndiceD.DataSource = dt
                            gridIndiceD.DataBind()
                            con.Close()
                        End Using
                    End Using
                End Using
            End Using
            gridIndiceD.UseAccessibleHeader = True
            gridIndiceD.HeaderRow.TableSection = TableRowSection.TableHeader

        Catch ex As Exception
            Response.Write("<script>console.log('BindgridSugerido: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
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
    Protected Sub btnRedirect_Command(sender As Object, e As CommandEventArgs)
        Dim almacen As String = e.CommandArgument.ToString()
        Dim url As String = "TrasladosCargaMacro.aspx?almacen=" & almacen
        ' Abrir la URL en una nueva pestaña
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenNewTab", "window.open('" & url & "', '_blank');", True)
        BindgridSugerido()
    End Sub
    Public Function GetNewPlanificacion(FECHAINICIO As DateTime, FECHAVENCIMIENTO As DateTime, ESTADO As String, USUARIO As String,
                                        FECHACREACION As DateTime, JSONSTRING As String) As Integer
        Dim sCon As String = sCon2
        Dim sel As String
        sel = "INSERT INTO dbo.PLANIFICACIONES (FECHAINICIO, FECHAVENCIMIENTO, ESTADO, USUARIO, FECHACREACION, JSON) VALUES (@p1, @p2, @p3, @p4, @p5, @p6); SELECT SCOPE_IDENTITY();"

        Using con As New SqlConnection(sCon)
            Using cmd As New SqlCommand(sel, con)
                cmd.Parameters.AddWithValue("@p1", FECHAINICIO)
                cmd.Parameters.AddWithValue("@p2", FECHAVENCIMIENTO)
                cmd.Parameters.AddWithValue("@p3", ESTADO)
                cmd.Parameters.AddWithValue("@p4", USUARIO)
                cmd.Parameters.AddWithValue("@p5", FECHACREACION)
                cmd.Parameters.AddWithValue("@p6", JSONSTRING)

                con.Open()
                Dim newIndex As Object = cmd.ExecuteScalar()
                con.Close()

                If newIndex IsNot Nothing AndAlso IsNumeric(newIndex) Then
                    Return Convert.ToInt32(newIndex)
                Else
                    Return -1 ' Indica un error si no se obtiene un ID válido.
                End If
            End Using
        End Using
    End Function

End Class

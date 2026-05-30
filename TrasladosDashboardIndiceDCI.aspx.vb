Imports System.Data
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Partial Class TrasladosDashboardIndiceDCI
    Inherits System.Web.UI.Page
    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public MyTable As New DataTable
    Public SQL_STRING As String
    Public sql_consulta As String

    Private Sub TrasladosDashboardIndiceDCI_Load(sender As Object, e As EventArgs) Handles Me.Load
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

                SQL_STRING = "SELECT " &
                    " ROW_NUMBER() OVER (ORDER BY t0.[Unds] desc) AS Ranking, " &
                    " (SELECT U_CardCode FROM movesa..OWHS WHERE WhsCode = t0.Codigo COLLATE Modern_Spanish_CI_AS) As [Cardcode], " &
                    " (SELECT U_ZONA FROM movesa..OWHS WHERE WhsCode = t0.Codigo COLLATE Modern_Spanish_CI_AS) AS [Zona], " &
                    " (Select U_SLPNAME FROM movesa..[@CRESPONSABLEOWHS] With (NOLOCK) WHERE code =  " &
                    " (Select U_Categorizacion FROM movesa..OWHS WHERE WhsCode = t0.Codigo COLLATE Modern_Spanish_CI_AS)) As [Sup], " &
                    " (Select [Name] FROM movesa..[@CRUTA] With (NOLOCK) WHERE code = (Select U_Ruta FROM movesa..OWHS  " &
                    " WHERE WhsCode = t0.Codigo COLLATE Modern_Spanish_CI_AS)) As [Ruta],  " &
                    " t0.[Codigo],t0.[Almacen],t0.[Cuadro],t0.[Comprometido],t0.[Solicitado],t0.[Transito],t0.[Fisico],t0.[Faltante],t0.[Unds] " &
                    " ,isnull(Convert(Decimal(19, 2), Convert(Decimal(19, 4), [Faltante]) / NULLIF(Convert(Decimal(19, 4), [Cuadro]), 0) * 100),0) As [70] " &
                    " ,isnull((SELECT sum([CANTIDAD])  FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS]  " &
                    " WHERE ALMDESTINO = t0.Codigo AND CODIGOESTADO <> 'CERRADO' ),0)[Despacho] " &
                    " ,isnull(convert(decimal(19,2),CONVERT(DECIMAL(19,4), ([Faltante]-isnull((SELECT sum([CANTIDAD])   " &
                    " FROM [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] WHERE ALMDESTINO = t0.codigo AND CODIGOESTADO <> 'CERRADO' ),0))) " &
                    " / NULLIF(CONVERT(DECIMAL(19,4), [Cuadro]), 0) * 100),0) AS [Indice_Proyectado] " &
                    " ,convert(char,t0.[Ultransfer],103) [Ultransfer] " &
                    " FROM [ArmadoMotos].[dbo].[RESUMEN_I_ALLWHS] t0 With (NOLOCK); "

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
        Dim url As String = "TrasladosCargaMacroIndirecto.aspx?almacen=" & almacen
        ' Abrir la URL en una nueva pestaña
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenNewTab", "window.open('" & url & "', '_blank');", True)
        BindgridSugerido()
    End Sub

    'Private Sub btnExportarJson_Click(sender As Object, e As EventArgs) Handles btnExportarJson.Click
    '    Try
    '        ' Lista para almacenar los datos de las filas seleccionadas
    '        Dim listaDatos As New List(Of Dictionary(Of String, Object))

    '        ' Recorrer las filas del GridView
    '        For Each row As GridViewRow In gridIndiceD.Rows
    '            ' Verificar si el CheckBox está activo
    '            Dim cbDocument As CheckBox = TryCast(row.FindControl("cbDocument"), CheckBox)
    '            If cbDocument IsNot Nothing AndAlso cbDocument.Checked Then
    '                ' Diccionario para almacenar los datos de la fila actual
    '                Dim filaData As New Dictionary(Of String, Object)

    '                ' Recorrer las columnas (excluyendo las últimas dos TemplateField)
    '                For i As Integer = 0 To gridIndiceD.Columns.Count - 3
    '                    ' Obtener el nombre de la columna
    '                    Dim nombreColumna As String = gridIndiceD.Columns(i).HeaderText

    '                    ' Obtener el valor de la celda y eliminar espacios en blanco
    '                    Dim valorCelda As String = HttpUtility.HtmlDecode(row.Cells(i).Text.Trim())

    '                    ' Agregar al diccionario
    '                    filaData(nombreColumna) = valorCelda
    '                Next

    '                ' Agregar la fila a la lista
    '                listaDatos.Add(filaData)
    '            End If
    '        Next

    '        ' Convertir la lista a JSON
    '        Dim jsonString As String = JsonConvert.SerializeObject(listaDatos, Formatting.None)

    '        ' Escapar caracteres problemáticos para JavaScript
    '        jsonString = jsonString.Replace("\", "\\").Replace("'", "\'").Replace(vbCrLf, "").Replace(vbLf, "")

    '        ' Enviar el JSON a la consola del navegador
    '        Dim script As String = "<script>console.log(JSON.parse('" & jsonString & "'));</script>"
    '        Response.Write(script)

    '        Session("jsonCI") = jsonString


    '        Response.Redirect("TrasladosDashboardPlanner.aspx")
    '        'BindgridSugerido()

    '    Catch ex As Exception
    '        ' Manejar errores mostrando un mensaje en la consola del navegador
    '        Dim errorScript As String = "<script>console.error('Error: " & ex.Message.Replace("'", "\'") & "');</script>"
    '        Response.Write(errorScript)
    '    End Try
    'End Sub
End Class

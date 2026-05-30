Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports System.Text
Imports SAPbobsCOM


Partial Class TrasladosDespachosAbiertosConfirmacion
    Inherits System.Web.UI.Page

    Public Shared sCon1 As String = "server=192.168.1.3;database=MOVESA;uid=sa;password=M*l!n3r0s2k12"
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    Public id_camion As String
    Public nuevoId As Integer = 0
    Public nuevoDetailId As Integer = 0
    Public ordenamiento As Integer = 0

    Private Sub TrasladosDespachosAbiertosConfirmacion_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'cargarMotorista()
            cargarPlacas()
            'Dim json As String = Request.QueryString("datos")
            'If Not String.IsNullOrEmpty(json) Then
            '    cargarJson(json)
            'End If
        End If
    End Sub
    Public Sub cargarPlacas()
        Try
            Dim dt As DataTable = New DataTable()
            Using conn As SqlConnection = New SqlConnection(sCon1)
                Dim query As String = "SELECT [ID],[PLACA] FROM [ArmadoMotos].[dbo].[TRANSPORTISTAS]"

                Dim cmd As SqlCommand = New SqlCommand(query, conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(dt)
                conn.Close()
            End Using
            drpPlacas.Dispose()
            drpPlacas.DataTextField = "PLACA"
            drpPlacas.DataValueField = "ID"
            drpPlacas.DataSource = dt
            drpPlacas.DataBind()
        Catch ex As Exception
            Response.Write("<script>console.log('CargarSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Sub

    'Public Sub cargarMotorista()
    '    Try
    '        Dim dt As DataTable = New DataTable()
    '        Using conn As SqlConnection = New SqlConnection(sCon1)
    '            Dim query As String = "SELECT [ID],[MOTORISTA] FROM [ArmadoMotos].[dbo].[TRANSPORTISTAS]"

    '            Dim cmd As SqlCommand = New SqlCommand(query, conn)
    '            Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
    '            da.Fill(dt)
    '            conn.Close()
    '        End Using
    '        drpMotorista.Dispose()
    '        drpMotorista.DataTextField = "MOTORISTA"
    '        drpMotorista.DataValueField = "ID"
    '        drpMotorista.DataSource = dt
    '        drpMotorista.DataBind()
    '    Catch ex As Exception
    '        Response.Write("<script>console.log('CargarSucursales: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub
    'Public Sub cargarJson(jsonastring As String)
    '    Try
    '        If Not Me.IsPostBack Then
    '            ' Paso 1: Decodificar y deserializar el JSON
    '            Dim jsonDecoded As String = HttpUtility.UrlDecode(jsonastring)
    '            Dim js As New JavaScriptSerializer()
    '            Dim lista = js.Deserialize(Of List(Of Dictionary(Of String, Integer)))(jsonDecoded)

    '            ' Paso 2: Extraer los despachoid únicos
    '            Dim ids As New List(Of Integer)
    '            For Each item In lista
    '                Response.Write("<script>console.log('ids: " & ReplaceCharsForFileName(item("despachoid"), " ") & "');</script>")
    '                Dim id As Integer = item("despachoid")
    '                If Not ids.Contains(id) Then
    '                    ids.Add(id)
    '                End If
    '            Next

    '            If ids.Count = 0 Then Exit Sub

    '            ' Paso 3: Crear la cláusula IN con parámetros
    '            Dim parametrosIn As New List(Of String)
    '            For i As Integer = 0 To ids.Count - 1
    '                parametrosIn.Add("@id" & i)
    '            Next

    '            Dim sql As New StringBuilder()
    '            sql.AppendLine("SELECT DISTINCT ISNULL(SERIEASIGNADA,'N/A') [Serie],")
    '            sql.AppendLine("       OBSERVACIONES COLLATE Modern_Spanish_CI_AS + ' ' +")
    '            sql.AppendLine("       RTRIM(CONVERT(NVARCHAR,DESPACHOID))  + ' ' +")
    '            sql.AppendLine("       RUTA COLLATE Modern_Spanish_CI_AS + ' ' +")
    '            sql.AppendLine("       ALMDESTINO COLLATE Modern_Spanish_CI_AS + ' ' +")
    '            sql.AppendLine("       (SELECT WhsName FROM MOVESA..OWHS WHERE WHSCODE=ALMDESTINO COLLATE Modern_Spanish_CI_AS) + ' ' +")
    '            sql.AppendLine("       DESCRIPCION COLLATE Modern_Spanish_CI_AS + ' ' +")
    '            sql.AppendLine("       ISNULL(SERIEASIGNADA,'N/A') COLLATE Modern_Spanish_CI_AS AS [Detalle]")
    '            sql.AppendLine("FROM DESPACHOS_DETALLE_MOTOS")
    '            sql.AppendLine("WHERE despachoid IN (" & String.Join(",", parametrosIn) & ")")

    '            ' Paso 4: Ejecutar
    '            Dim constr As String = sCon2
    '            Using con As New SqlConnection(constr)
    '                Using cmd As New SqlCommand(sql.ToString(), con)
    '                    ' Agregar parámetros
    '                    For i As Integer = 0 To ids.Count - 1
    '                        cmd.Parameters.AddWithValue("@id" & i, ids(i))
    '                    Next

    '                    cmd.CommandType = CommandType.Text
    '                    con.Open()
    '                    lstMotosySeries.DataSource = cmd.ExecuteReader()
    '                    lstMotosySeries.DataTextField = "Detalle"
    '                    lstMotosySeries.DataValueField = "Serie"
    '                    lstMotosySeries.DataBind()
    '                    con.Close()
    '                End Using
    '            End Using
    '        End If
    '    Catch ex As Exception
    '        Response.Write("cargarJson " & ex.Message)
    '        Response.Write("<script>console.log('cargarJson: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
    '    End Try
    'End Sub
    Public Function crearCamionHeader(estado As String, fecha_salida As DateTime, fecha_creacion As DateTime,
                                      usuario As String, placa As String, motorista As String) As Integer
        Try
            ' Insertar en CABECERA
            Dim queryHeader As String = "INSERT INTO [dbo].[CARGA_CAMION_HEADER]([FECHASALIDA],[ESTADO],[FECHACREACION],[USUARIO],[PLACA],[MOTORISTA]) " &
                                      "VALUES (@FECHASALIDA,@ESTADO,@FECHACREACION,@USUARIO,@PLACA,@MOTORISTA); " &
                                      "SELECT SCOPE_IDENTITY();"

            Using connection As New SqlConnection(sCon2)
                ' Insertar cabecera
                Using cmd As New SqlCommand(queryHeader, connection)
                    cmd.Parameters.AddWithValue("@FECHASALIDA", fecha_salida)
                    cmd.Parameters.AddWithValue("@ESTADO", estado)
                    cmd.Parameters.AddWithValue("@FECHACREACION", fecha_creacion)
                    cmd.Parameters.AddWithValue("@USUARIO", usuario)
                    cmd.Parameters.AddWithValue("@PLACA", placa)
                    cmd.Parameters.AddWithValue("@MOTORISTA", motorista)
                    connection.Open()
                    nuevoId = Convert.ToInt32(cmd.ExecuteScalar())
                    connection.Close()
                End Using
            End Using
            ' Guardar el ID del camión en la variable de sesión
            Return nuevoId
        Catch ex As Exception
            Return nuevoId
            Response.Write("<script>console.log('crearCamionHeader: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Function crearCamionDetail(header_id As String, row_linea As String, despacho_id As String, serie_asignada As String) As Integer
        Try
            ' Insertar detalles para cada fila seleccionada
            Dim qry_insert As String = "IF NOT EXISTS ( " &
                    " SELECT 1 FROM CARGA_CAMION_DETALLE WHERE PLANID = @planid AND HEADERID = @despachoId)" &
                    " BEGIN" &
                    " INSERT INTO [dbo].[CARGA_CAMION_DETALLE]  (" &
                    " [IDCAMION],[RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],  " &
                    " [ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS],[CANTIDAD],  " &
                    " [QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[HEADERID],  " &
                    " [ESTADO],[TIPO],[SUPERVISOR],[FECHA],[USUARIO],[CB],  " &
                    " [FISICO],[FALTANTE],[VTAA],[FARMADO],[FDESPACHO],  " &
                    " [PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO],  " &
                    " [SERIEASIGNADA],[SERIESYS],[FECHASERIE],[PEDIDOID],[PEDIDOLINEA],[DESPACHOROWID]" &
                    " )" &
                    " SELECT " &
                    " @idcamion,[RUTA],[ALMORIGEN],[ALMDESTINO],[CARDCODE],  " &
                    " [ARTICULO],[MODELO],[DESCRIPCION],[ESPACIOS],[CANTIDAD],  " &
                    " [QTYLOGISTICA],[QTYSUCURSAL],[OBSERVACIONES],[HEADERID],  " &
                    " [ESTADO],[TIPO],[SUPERVISOR],getdate(),[USUARIO],[CB]," &
                    " [FISICO],[FALTANTE],[VTAA],[FARMADO],[FDESPACHO],  " &
                    " [PLANID],[DESPACHOID],[CODIGOESTADO],[CODMODELO],  " &
                    " [SERIEASIGNADA],[SERIESYS],[FECHASERIE],[PEDIDOID],[PEDIDOLINEA],[ID]" &
                    " FROM DESPACHOS_DETALLE_MOTOS " &
                    " WHERE PLANID = @planid AND HEADERID = @despachoId and DELETED='N' and [SERIEASIGNADA] SERIEASIGNADA = @serie;" &
                    " declare @itemcode as nvarchar(10) = (select [ARTICULO] from DESPACHOS_DETALLE_MOTOS where = @serie)" &
                    " UPDATE [ArmadoMotos].[dbo].[DESPACHOS_DETALLE_MOTOS] SET CODIGOESTADO = 'CAMION ABIERTO', CAMIONID = @idcamion WHERE DESPACHOID = @despachoId and [SERIEASIGNADA] = @serie;" &
                    " UPDATE [ArmadoMotos].[dbo].[MACROINSERT] SET ESTADO='F' WHERE HEADERID = @despachoId AND PLANID = @planid and [ARTICULO] = @itemcode;" &
                    " END"

            Using connection As New SqlConnection(sCon2)
                Using cmd As New SqlCommand(qry_insert, connection)
                    cmd.Parameters.AddWithValue("@idcamion", header_id)
                    cmd.Parameters.AddWithValue("@planid", row_linea)
                    cmd.Parameters.AddWithValue("@despachoId", despacho_id)
                    cmd.Parameters.AddWithValue("@serie", serie_asignada)
                    nuevoDetailId = cmd.ExecuteNonQuery()
                End Using
            End Using
            Return nuevoDetailId
        Catch ex As Exception
            Return nuevoDetailId
            Response.Write("<script>console.log('crearCamionDetail: " & ReplaceCharsForFileName(ex.Message, " ") & "');</script>")
        End Try
    End Function
    Public Function actualizarOrdenamientoCamion(serial_number As String, item_order As String) As Integer
        Try
            Using conn As New SqlConnection(sCon2)
                conn.Open()

                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        ' Actualizar cada item en la base de datos
                        Using cmd As New SqlCommand("UPDATE [ArmadoMotos].[dbo].[CARGA_CAMION_DETALLE] SET ORDENAMIENTO = @orden WHERE SERIEASIGNADA = @serie", conn, transaction)
                            cmd.Parameters.AddWithValue("@orden", item_order)
                            cmd.Parameters.AddWithValue("@serie", serial_number)
                            ordenamiento = cmd.ExecuteNonQuery()
                        End Using

                        transaction.Commit()

                    Catch ex As System.Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
        Catch ex As Exception

        End Try
    End Function
    'Public Sub TrabajosAdicionales(jsonastring As String)
    '    Try
    '        If Not Me.IsPostBack Then
    '            Dim constr As String = sCon2
    '            Using con As SqlConnection = New SqlConnection(constr)
    '                Using cmd As SqlCommand = New SqlCommand("SELECT DISTINCT isnull(SERIEASIGNADA,'N/A') [Serie], RUTA COLLATE Modern_Spanish_CI_AS  " &
    '                                                         " + ' ' + OBSERVACIONES COLLATE Modern_Spanish_CI_AS  " &
    '                                                         " + ' ' + ALMDESTINO COLLATE Modern_Spanish_CI_AS  " &
    '                                                         " + ' ' + (SELECT WhsName FROM MOVESA..OWHS WHERE WHSCODE=ALMDESTINO COLLATE Modern_Spanish_CI_AS)  " &
    '                                                         " + ' ' + DESCRIPCION COLLATE Modern_Spanish_CI_AS  " &
    '                                                         " + ' ' + isnull(SERIEASIGNADA,'N/A') COLLATE Modern_Spanish_CI_AS [Detalle] FROM CARGA_CAMION_DETALLE  " &
    '                                                         " WHERE IDCAMION = @idCamion")
    '                    cmd.Parameters.AddWithValue("@idCamion", idCamion)
    '                    cmd.CommandType = CommandType.Text
    '                    cmd.Connection = con
    '                    con.Open()
    '                    lstMotosySeries.DataSource = cmd.ExecuteReader()
    '                    lstMotosySeries.DataTextField = "Detalle"
    '                    lstMotosySeries.DataValueField = "Serie"
    '                    lstMotosySeries.DataBind()
    '                    con.Close()
    '                End Using
    '            End Using

    '        End If
    '    Catch ex As Exception
    '        Response.Write("TrabajosAdicionales " & ex.Message)
    '    End Try
    'End Sub
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

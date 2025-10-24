Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json

Partial Class Grafico
    Inherits System.Web.UI.Page
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"
    <WebMethod()>
    Public Shared Function GetData(ByVal anio As Integer, ByVal mes As Integer, ByVal almacen As String) As String
        Dim data As New List(Of Object)()
        Dim connString As String = sCon2
        Dim query As String = "EXEC spRecorrerDiasDelMes @Anio, @Mes, @Whscode"

        Try
            Using conn As New SqlConnection(connString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Anio", anio)
                    cmd.Parameters.AddWithValue("@Mes", mes)
                    cmd.Parameters.AddWithValue("@Whscode", almacen)
                    cmd.CommandTimeout = 120 ' Aumentar timeout a 120 segundos

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            data.Add(New With {
                                .Dia = reader("DIA"),
                                .InvInicial = reader("INVINICIAL"),
                                .Marca = reader("MARCA"),
                                .Modelo = reader("MODELO")
                            })
                        End While
                    End Using
                End Using
            End Using

            ' Serializar con Newtonsoft.Json sin `MaxJsonLength`
            Return JsonConvert.SerializeObject(data, Formatting.None)

        Catch ex As SqlException When ex.Number = -2
            Return "{""error"":""Tiempo de espera agotado en la base de datos. Inténtalo nuevamente.""}"
        Catch ex As Exception
            Return "{""error"":""" & ex.Message & """}"
        End Try
    End Function
    '<WebMethod()>
    'Public Shared Function GetData(ByVal anio As Integer, ByVal mes As Integer, ByVal almacen As String) As String
    '    Dim data As New List(Of Object)()
    '    Dim connString As String = sCon2
    '    Dim query As String = "EXEC spRecorrerDiasDelMes @Anio, @Mes, @Whscode"

    '    Try
    '        Using conn As New SqlConnection(connString)
    '            Using cmd As New SqlCommand(query, conn)
    '                cmd.Parameters.AddWithValue("@Anio", anio)
    '                cmd.Parameters.AddWithValue("@Mes", mes)
    '                cmd.Parameters.AddWithValue("@Whscode", almacen)
    '                ' ⏳ Aumentar el timeout a 120 segundos (2 minutos)
    '                cmd.CommandTimeout = 120
    '                conn.Open()

    '                Using reader As SqlDataReader = cmd.ExecuteReader()
    '                    While reader.Read()
    '                        data.Add(New With {
    '                        .Dia = reader("DIA"),
    '                        .InvInicial = reader("INVINICIAL"),
    '                        .Marca = reader("MARCA"),
    '                        .Modelo = reader("MODELO")
    '                    })
    '                    End While
    '                End Using
    '            End Using
    '        End Using

    '        ' ✅ Aumentar el tamaño máximo del JSON serializado
    '        Dim js As New JavaScriptSerializer()
    '        js.MaxJsonLength = Integer.MaxValue ' Permite tamaños grandes

    '        Return js.Serialize(data)

    '    Catch ex As SqlException When ex.Number = -2
    '        ' Manejar específicamente timeout
    '        Return "{""error"":""Tiempo de espera agotado en la base de datos. Inténtalo nuevamente.""}"
    '    Catch ex As Exception
    '        ' Manejo general de errores
    '        Return "{""error"":""" & ex.Message & """}"
    '    End Try
    'End Function

    '<WebMethod()>
    'Public Shared Function GetData(ByVal anio As Integer, ByVal mes As Integer, ByVal almacen As String) As String
    '    Dim data As New List(Of Object)()
    '    Dim connString As String = sCon2
    '    Dim query As String = "EXEC spRecorrerDiasDelMes @Anio, @Mes, @Whscode"

    '    Using conn As New SqlConnection(connString)
    '        Using cmd As New SqlCommand(query, conn)
    '            cmd.Parameters.AddWithValue("@Anio", anio)
    '            cmd.Parameters.AddWithValue("@Mes", mes)
    '            cmd.Parameters.AddWithValue("@Whscode", almacen)
    '            ' ⏳ Aumentar el timeout a 60 segundos
    '            cmd.CommandTimeout = 120
    '            conn.Open()
    '            Using reader As SqlDataReader = cmd.ExecuteReader()
    '                While reader.Read()
    '                    data.Add(New With {
    '                        .Dia = reader("DIA"),
    '                        .InvInicial = reader("INVINICIAL"),
    '                        .Marca = reader("MARCA"),
    '                        .Modelo = reader("MODELO")
    '                    })
    '                End While
    '            End Using
    '        End Using
    '    End Using

    '    Dim js As New JavaScriptSerializer()
    '    Return js.Serialize(data)
    'End Function

    <WebMethod()>
    Public Shared Function GetWHS() As String
        Dim data As New List(Of Object)()
        Dim connString As String = sCon2
        Dim query As String = "Select whscode, whsname from movesa..owhs with(nolock) where u_type='PRO' and left(whscode,1)<>'T'" &
                                " AND WhsCode NOT IN ('BPD01','CDM00','CDR00','CDR01','CONCA01','Consumir','DCM00','DCR00',   " &
                                " 'DCR01','DEV01','DEV02','EST001','FCE01','MAY01','MLCB01','MLCB02','MOB01','MSPS01','MSPS02',   " &
                                " 'MSPS03','MSPS04','MSUC01','MSUC02','MSUC03','MSUC04','MSUC05','MSUC07','MTGA01','MTRANS00',   " &
                                " 'RLCB02','RLCB03','RSPS02','RSPS03','RSPS04','RSPS05','RSPS06','RSPS07','RTGA01',   " &
                                " 'RTGA0102','SUC0102','SUC0201','SUC03','SUC0301','SUC0401','SUC0701','SUC0801','SUM001','DIU-10','GMG122','mym 09','con', 'CON 334') "

        Using conn As New SqlConnection(connString)
            Using cmd As New SqlCommand(query, conn)
                ' ⏳ Aumentar el timeout a 60 segundos
                cmd.CommandTimeout = 60
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        data.Add(New With {
                            .Whscode = reader("whscode"),
                            .Whsname = reader("whsname")
                        })
                    End While
                End Using
            End Using
        End Using

        Dim js As New JavaScriptSerializer()
        Return js.Serialize(data)
    End Function
End Class

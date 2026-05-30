<%@ WebHandler Language="VB" Class="InventarioHandler" %>

Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Configuration

Public Class InventarioHandler
    Implements IHttpHandler
    Public Shared sCon2 As String = "server=192.168.1.3;database=ArmadoMotos;uid=sa;password=M*l!n3r0s2k12"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"

        Dim result As New List(Of Object)
        Dim query As String = "SELECT [Id], [EstadoVeh], [OrigenDestino], [Pasillo], [Segmento], [Codigo], " &
                " [Descripcion], [SerieMoto], [SerieMotor], [Marca], [Modelo], [Cilindros], [Color],  " &
                " [YYYY], " &
                " ,(SELECT DATEDIFF(DAY,FINCC,GETDATE()) FROM ARMADOMOTOS..ARMADOMOTOS WHERE SERIE=T00.SerieMoto) [DiasArmado] " &
                " , isnull((SELECT top 1 datediff(day,t2.DocDate,getdate()) FROM MOVESA..OSRN T0 With(nolock) inner join MOVESA..ITL1 T1 with(nolock) " &
                " On T1.SysNumber=T0.SysNumber And T1.ItemCode = T0.ItemCode inner join " &
                " MOVESA..OITL T2 with(nolock) on T1.LogEntry = T2.LogEntry where t0.MnfSerial =T00.SerieMoto  COLLATE Modern_Spanish_CI_AS And t2.DocType=20),0) [DiasIngreso] " &
                " , [Observaciones], [Usuario], [DateCreated],  " &
                " [LIMPIEZA], [AUDITORIA], [PINTURA] " &
                " FROM [ArmadoMotos].[dbo].[InventarioVehiculos] WHERE ESTADOLOGISTICA='ACTIVO'"

        Using con As New SqlConnection(sCon2)
            Dim cmd As New SqlCommand(query, con)
            con.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                Dim item As New Dictionary(Of String, Object) From {
                    {"Id", reader("Id")},
                    {"EstadoVeh", reader("EstadoVeh").ToString()},
                    {"OrigenDestino", reader("OrigenDestino").ToString()},
                    {"Pasillo", reader("Pasillo").ToString()},
                    {"Segmento", reader("Segmento").ToString()},
                    {"Codigo", reader("Codigo").ToString()},
                    {"Descripcion", reader("Descripcion").ToString()},
                    {"SerieMoto", reader("SerieMoto").ToString()},
                    {"SerieMotor", reader("SerieMotor").ToString()},
                    {"Marca", reader("Marca").ToString()},
                    {"Modelo", reader("Modelo").ToString()},
                    {"Cilindros", reader("Cilindros").ToString()},
                    {"Color", reader("Color").ToString()},
                    {"YYYY", reader("YYYY").ToString()},
                    {"DiasArmado", reader("DiasArmado").ToString()},
                    {"DiasIngreso", reader("DiasIngreso").ToString()},
                    {"Observaciones", reader("Observaciones").ToString()},
                    {"Usuario", reader("Usuario").ToString()},
                    {"DateCreated", reader("DateCreated").ToString()},
                    {"LIMPIEZA", reader("LIMPIEZA").ToString()},
                    {"AUDITORIA", reader("AUDITORIA").ToString()},
                    {"PINTURA", reader("PINTURA").ToString()}
                }

                result.Add(item)
            End While
        End Using

        Dim json As String = New JavaScriptSerializer().Serialize(result)
        context.Response.Write(json)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class

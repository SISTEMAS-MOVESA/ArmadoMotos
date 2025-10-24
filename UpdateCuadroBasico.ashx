<%@ WebHandler Language="VB" Class="UpdateMaximoHandler" %>

Imports System.Web
Imports System.Data.SqlClient

Public Class UpdateMaximoHandler : Implements IHttpHandler
    Public Shared sCon1 As String = "server=192.168.1.3;database=MovesaWeb;uid=sa;password=M*l!n3r0s2k12"


    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim maximo As Integer = Convert.ToInt32(context.Request("maximo"))
        Dim whsCode As String = context.Request("whscode")
        Dim modelo As String = context.Request("modelo")

        Dim connectionString As String = sCon1
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "IF EXISTS (SELECT 1 FROM [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] WHERE WHSCODE=@p2 AND MODELO=@p3) " &
                                    "BEGIN " &
                                    "UPDATE [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] SET MAXIMO=@p1 WHERE WHSCODE=@p2 AND MODELO=@p3 " &
                                    "END " &
                                    "ELSE " &
                                    "BEGIN " &
                                    "INSERT INTO [MOVESAWeb].[dbo].[LOGISTICA_MAX_INV] (WHSCODE, MODELO, MAXIMO) VALUES (@p2, @p3, @p1) " &
                                    "END"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@p1", maximo)
                cmd.Parameters.AddWithValue("@p2", whsCode)
                cmd.Parameters.AddWithValue("@p3", modelo)

                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using

        context.Response.ContentType = "text/plain"
        context.Response.Write("Almacen :" & whsCode & " Modelo :" & modelo & " CB :" & maximo)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class

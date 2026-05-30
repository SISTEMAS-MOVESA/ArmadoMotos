<%@ WebHandler Language="VB" Class="GuardarNotasU7DHandler" %>

Imports System
Imports System.Web
Imports System.Web.SessionState
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Class GuardarNotasU7DHandler
    Implements IHttpHandler, IRequiresSessionState

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        Try
            Dim body As String = ""
            Using reader As New System.IO.StreamReader(context.Request.InputStream)
                body = reader.ReadToEnd()
            End Using

            If String.IsNullOrEmpty(body) Then
                context.Response.Write("{""ok"":false,""error"":""body vacio""}")
                Return
            End If

            Dim jss As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim payload As Dictionary(Of String, Object) = jss.Deserialize(Of Dictionary(Of String, Object))(body)

            Dim whscode As String = If(payload.ContainsKey("whscode") AndAlso payload("whscode") IsNot Nothing, payload("whscode").ToString(), "")
            Dim usuario  As String = If(payload.ContainsKey("usuario") AndAlso payload("usuario") IsNot Nothing, payload("usuario").ToString(), "anon")

            If String.IsNullOrEmpty(whscode) Then
                context.Response.Write("{""ok"":false,""error"":""whscode requerido""}")
                Return
            End If

            Dim saved As Integer = 0

            If payload.ContainsKey("notas") AndAlso payload("notas") IsNot Nothing Then
                Dim notas As System.Collections.IEnumerable = TryCast(payload("notas"), System.Collections.IEnumerable)
                If notas IsNot Nothing Then
                    For Each itemObj As Object In notas
                        Dim item As Dictionary(Of String, Object) = TryCast(itemObj, Dictionary(Of String, Object))
                        If item Is Nothing Then Continue For

                        Dim docnum As Integer = 0
                        If item.ContainsKey("docnum") AndAlso item("docnum") IsNot Nothing Then
                            Integer.TryParse(item("docnum").ToString(), docnum)
                        End If
                        If docnum = 0 Then Continue For

                        Dim nota As String = If(item.ContainsKey("nota") AndAlso item("nota") IsNot Nothing, item("nota").ToString(), "")
                        If String.IsNullOrEmpty(nota) Then Continue For

                        Dim codmodelo    As String = If(item.ContainsKey("codmodelo") AndAlso item("codmodelo") IsNot Nothing, item("codmodelo").ToString(), "")
                        Dim fechaStr     As String = If(item.ContainsKey("fechafactura") AndAlso item("fechafactura") IsNot Nothing, item("fechafactura").ToString(), "")
                        Dim fechaParam   As Object = DBNull.Value
                        Dim fechaParsed  As DateTime
                        If Not String.IsNullOrEmpty(fechaStr) AndAlso DateTime.TryParse(fechaStr, fechaParsed) Then
                            fechaParam = fechaParsed.Date
                        End If
                        Dim paretoPct As Decimal = 0
                        If item.ContainsKey("pareto") AndAlso item("pareto") IsNot Nothing Then
                            Decimal.TryParse(item("pareto").ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, paretoPct)
                        End If

                        Dim sql As String =
                            "IF EXISTS (SELECT 1 FROM [ArmadoMotos].[dbo].[VENTAS_U7D_CONTROL] WHERE DOCNUM=@dn AND WHSCODE=@whs) " &
                            " UPDATE [ArmadoMotos].[dbo].[VENTAS_U7D_CONTROL]" &
                            "   SET NOTA=@nota, ESTADO='RESUELTO', FECHA_REGISTRO=GETDATE(), USUARIO=@usr, FECHA_FACTURA=@ff, PARETO_PCT=@pareto" &
                            "   WHERE DOCNUM=@dn AND WHSCODE=@whs " &
                            "ELSE " &
                            " INSERT INTO [ArmadoMotos].[dbo].[VENTAS_U7D_CONTROL] (DOCNUM,WHSCODE,COD_MODELO,NOTA,ESTADO,USUARIO,FECHA_FACTURA,PARETO_PCT)" &
                            " VALUES (@dn,@whs,@mod,@nota,'RESUELTO',@usr,@ff,@pareto)"

                        Dim params As New List(Of SqlParameter) From {
                            New SqlParameter("@dn",     docnum),
                            New SqlParameter("@whs",    whscode),
                            New SqlParameter("@mod",    codmodelo),
                            New SqlParameter("@nota",   nota),
                            New SqlParameter("@usr",    usuario),
                            New SqlParameter("@ff",     If(fechaParam Is DBNull.Value, CObj(DBNull.Value), fechaParam)),
                            New SqlParameter("@pareto", paretoPct)
                        }
                        DbConfig.ExecuteNonQuery(sql, DbConfig.DBServer.ARMADOMOTOS, params)
                        saved += 1
                    Next
                End If
            End If

            context.Response.Write("{""ok"":true,""saved"":" & saved & "}")

        Catch ex As Exception
            Dim msg As String = ex.Message.Replace("""", "'").Replace(vbCr, " ").Replace(vbLf, " ")
            context.Response.Write("{""ok"":false,""error"":""" & msg & """}")
        End Try
    End Sub

    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class

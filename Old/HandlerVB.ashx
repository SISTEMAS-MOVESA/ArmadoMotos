<%@ WebHandler Language="VB" Class="HandlerVB" %>

Imports System
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Web.Script.Serialization

Public Class HandlerVB : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        'Check if Request is to Upload the File.
        If context.Request.Files.Count > 0 Then

            'Fetch the Uploaded File.
            Dim postedFile As HttpPostedFile = context.Request.Files(0)

            'Set the Folder Path.
            Dim folderPath As String = context.Server.MapPath("ImagenesVideo/")

            'Set the File Name.
            Dim fileName As String = Path.GetFileName(postedFile.FileName)

            'Save the File in Folder.
            postedFile.SaveAs(folderPath + fileName)
            'Send File details in a JSON Response.
            Dim json As String = New JavaScriptSerializer().Serialize(New With {
                .name = fileName
            })
            context.Response.StatusCode = CInt(HttpStatusCode.OK)
            context.Response.ContentType = "text/json"
            context.Response.Write(json)
            context.Response.End()
        End If
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
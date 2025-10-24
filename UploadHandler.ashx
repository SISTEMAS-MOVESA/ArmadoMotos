<%@ WebHandler Language="VB" Class="UploadHandler" %>

Imports System.Web
Imports System.IO

Public Class UploadHandler : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/plain"
        Try
            If context.Request.Files.Count > 0 Then
                Dim file As HttpPostedFile = context.Request.Files(0)
                Dim serieMoto As String = context.Request.Form("serieMoto")
                Dim uniqueId As String = Guid.NewGuid().ToString()
                Dim fileName As String = serieMoto & "_" & uniqueId & Path.GetExtension(file.FileName)
                Dim savePath As String = context.Server.MapPath("~/Imagenes/Auditoria/ConteoFisico/") & fileName

                file.SaveAs(savePath)
                context.Response.Write("Archivo subido exitosamente: " & fileName)
            Else
                context.Response.Write("No se ha recibido ningún archivo.")
            End If
        Catch ex As Exception
            context.Response.Write("Error al subir el archivo: " & ex.Message)
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class

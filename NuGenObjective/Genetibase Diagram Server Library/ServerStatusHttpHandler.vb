Option Strict On
Option Explicit On

Imports System.Web
Imports System.Reflection
Imports System.IO


Public Class ServerStatusHttpHandler
    Implements IHttpHandler

    Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
        Dim result As New ServerStatus
        result.Version = Assembly.GetExecutingAssembly.GetName.Version.ToString
        result.Secure = False ' This will change later

        Dim diagramRoot As New DirectoryInfo(context.Request.PhysicalApplicationPath)
        Dim diagramFiles() As DirectoryInfo = diagramRoot.GetDirectories("diagrams\*")

        For Each f As DirectoryInfo In diagramFiles
            If Not ( _
                (f.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden _
                    OrElse _
                (f.Attributes And FileAttributes.System) = FileAttributes.System _
                ) Then

                Dim fileUri As New UriBuilder(context.Request.Url)
                With fileUri
                    .Path = .Path.Remove(.Path.LastIndexOf("/"c))
                    .Path &= "/" & f.Name & ".ocidml"
                End With

                result.Files.Add(New ServerStatusFile(f.Name, fileUri.ToString))
            End If
        Next

        Dim xs As New Xml.Serialization.XmlSerializer(GetType(ServerStatus))


        With context.Response
            .AddHeader("content-type", "text/xml")

            xs.Serialize(.OutputStream, result)
            .Flush()
        End With
    End Sub
End Class

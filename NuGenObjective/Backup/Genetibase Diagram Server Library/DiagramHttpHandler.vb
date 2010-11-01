Option Strict On
Option Explicit On

Imports System.Xml
Imports System.Web
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports Genetibase.NuGenObjective
Imports Genetibase.NuGenObjective.DiffPatch
Imports Genetibase.NuGenObjective.Drawing
Imports System.IO
Imports System.Text
Imports System.Runtime.Serialization.Formatters.Binary

Public Class DiagramHttpHandler
    Implements IHttpHandler

    Private Const originalFileName As String = "original.ocidml"
    Private Const currentFileName As String = "current.ocidml"
    Private Const lockFileName As String = "lock.ocidmllock"

#Region " Utility methods "
    Private Function ReaderSettings() As XmlReaderSettings
        Dim result As New XmlReaderSettings
        With result
            .IgnoreWhitespace = True
            .IgnoreProcessingInstructions = True
            .IgnoreComments = True
        End With
        Return result
    End Function

    Private Function WriterSettings() As XmlWriterSettings
        Dim result As New XmlWriterSettings
        With result
            .Indent = False
            .Encoding = Encoding.UTF8
        End With
        Return result
    End Function

    Private Sub SetResponseStatus(ByVal context As System.Web.HttpContext, ByVal statusCode As Integer)
        With context.Response
            .StatusCode = statusCode
            .End()
        End With
    End Sub

    Private Function GetDiagramPath(ByVal filePath As String) As String
        Dim directory As String = Path.GetDirectoryName(filePath)
        Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)
        directory = Path.Combine(directory, "diagrams")
        fileName = Path.Combine(directory, fileName)
        Return fileName
    End Function

    Private Function GetCurrentRevision(ByVal currentFilePath As String) As Integer
        Dim currentRevision As Integer
        ' Read the current revision number
        Dim xrs As XmlReaderSettings = ReaderSettings()
        Dim xr As XmlReader = XmlReader.Create(currentFilePath, xrs)
        With xr
            .MoveToContent()
            .Read() ' Read the diagram tag
            currentRevision = .ReadElementContentAsInt("Revision", Diagram.RootNameSpace)
            .Close()
        End With
        Return currentRevision
    End Function

    Private Function GetCurrentLock(ByVal filePath As String) As Hashtable
        Dim result As New Hashtable
        Dim lockPath As String = Path.Combine(filePath, lockFileName)

        If File.Exists(lockPath) Then
            Try
                Dim bf As New BinaryFormatter
                Dim fs As New FileStream(lockPath, FileMode.Open, FileAccess.Read)
                result = DirectCast(bf.Deserialize(fs), Hashtable)
                fs.Close()
            Catch

            End Try
        End If

        Return result
    End Function

    Private Function CreateLock(ByVal filePath As String) As Hashtable
        Dim result As New Hashtable
        Dim lockPath As String = Path.Combine(filePath, "lock.ocidmllock")

        Dim fs As New FileStream(lockPath, FileMode.Create, FileAccess.Write)
        Dim bf As New BinaryFormatter
        result.Add("LockId", Guid.NewGuid.ToString("", Globalization.CultureInfo.InvariantCulture))
        ' TODO: Change the following to 20 minutes
        result.Add("TimeOutAt", Now.AddMinutes(5))
        bf.Serialize(fs, result)
        fs.Flush()
        fs.Close()

        Return result
    End Function

    Private Sub DeleteLock(ByVal filePath As String, ByVal lockCode As String)
        Dim currLock As Hashtable = GetCurrentLock(filePath)
        If currLock.Count = 0 Then
            ' No lock present. Nothing to do.
            Exit Sub
        End If
        If DirectCast(currLock("LockId"), String) = lockCode Then
            File.Delete(Path.Combine(filePath, lockFileName))
        End If
    End Sub
#End Region

#Region " GET "
    Private Sub ShowLatestDiagram(ByVal context As System.Web.HttpContext, ByVal pageKey As String, ByVal filePath As String)
        Dim currentFilePath As String = Path.Combine(filePath, currentFileName)
        If Not File.Exists(currentFilePath) Then
            SetResponseStatus(context, 404)
            Exit Sub
        End If

        Dim xrs As XmlReaderSettings = ReaderSettings()

        Dim xr As XmlReader = XmlReader.Create(currentFilePath, xrs)

        Dim latestDiagram As Diagram = Diagram.Open(xr)

        Dim pageToRender As Page = Nothing

        If pageKey <> "" Then
            If latestDiagram.Pages.Contains(pageKey) Then
                pageToRender = latestDiagram.Pages(pageKey)
            Else
                SetResponseStatus(context, 404)
                Exit Sub
            End If
        Else
            pageToRender = latestDiagram.Pages(0)
        End If


        Dim imageToRender As Bitmap

        With DrawingHelper.PageSize(pageToRender)
            imageToRender = New Bitmap(.Width, .Height, Imaging.PixelFormat.Format32bppPArgb Or Imaging.PixelFormat.Gdi)
        End With

        Dim g As Graphics = Graphics.FromImage(imageToRender)
        g.FillRectangle(Brushes.White, g.ClipBounds())

        Dim diagramDivisionsPen As New Pen(Color.SlateGray, 1.5F)
        diagramDivisionsPen.DashStyle = DashStyle.Dot

        Dim stateLinePen As New Pen(Color.SlateGray, 3.0!)

        Dim objectLinePen As New Pen(Color.FromArgb(64, Color.SlateGray), 5.0F)
        objectLinePen.EndCap = LineCap.ArrowAnchor


        Dim drawingPage As New DrawingPage(pageToRender)

        drawingPage.Draw( _
                    Nothing, _
                    g, _
                    0, _
                    0, _
                    1.0!, _
                    diagramDivisionsPen, _
                    stateLinePen, _
                    stateLinePen, _
                    objectLinePen, _
                    objectLinePen _
            )

        context.Response.ContentType = "image/jpeg"
        imageToRender.Save(context.Response.OutputStream, Imaging.ImageFormat.Jpeg)
        g.Dispose()
    End Sub

    Private Sub GetLatestDiagram(ByVal context As HttpContext, ByVal filePath As String)
        Dim currentFilePath As String = Path.Combine(filePath, currentFileName)
        If Not File.Exists(currentFilePath) Then
            SetResponseStatus(context, 404)
        Else
            With context.Response
                .WriteFile(currentFilePath)
                .Flush()
            End With
        End If
    End Sub


    Private Sub GetStatus(ByVal context As HttpContext, ByVal filePath As String)
        Dim currentRevision As Integer = 0
        Dim currentFilePath As String = Path.Combine(filePath, currentFileName)
        If Not File.Exists(currentFilePath) Then
            SetResponseStatus(context, 404)
            Exit Sub
        End If

        Try
            currentRevision = GetCurrentRevision(currentFilePath)
        Catch
            SetResponseStatus(context, 500)
            Exit Sub
        End Try

        Dim xws As XmlWriterSettings = WriterSettings()

        With context.Response
            .ContentType = "text/xml"
            .ContentEncoding = Encoding.UTF8
            Dim xw As XmlWriter = XmlWriter.Create(.OutputStream, xws)
            With xw
                .WriteStartDocument()
                .WriteStartElement("DiagramStatus")
                .WriteAttributeString("CurrentRevision", currentRevision.ToString(Globalization.CultureInfo.InvariantCulture))
                .WriteAttributeString("Locked", "False")
                .WriteEndElement()
                .WriteEndDocument()
            End With
            xw.Flush()
        End With
    End Sub

    Private Sub ProcessGetRequest(ByVal context As System.Web.HttpContext, ByVal pageKey As String, ByVal filePath As String, ByVal commandString As String)
        If Not Directory.Exists(filePath) Then
            SetResponseStatus(context, 404)
            Exit Sub
        End If

        Try
            Select Case LCase(commandString)
                Case "status"
                    GetStatus(context, filePath)
                Case "checkout", "get"
                    GetLatestDiagram(context, filePath)
                Case Else
                    ShowLatestDiagram(context, pageKey, filePath)
            End Select
        Catch ex As Exception
            SetResponseStatus(context, 500)
        End Try
    End Sub
#End Region

#Region " POST "
    Public Sub LockDiagram(ByVal context As HttpContext, ByVal filePath As String)
        If Not Directory.Exists(filePath) Then
            SetResponseStatus(context, 404)
            Exit Sub
        End If
        Dim currentLock As Hashtable = GetCurrentLock(filePath)
        If currentLock.Count <> 0 Then
            ' If the old lock is alive, error out
            If Now < DirectCast(currentLock("TimeOutAt"), Date) Then
                SetResponseStatus(context, 500)
                Exit Sub
            End If
        End If
        currentLock = CreateLock(filePath)
        If currentLock.Count > 0 Then
            ' Write the lock back
            With context.Response
                .ContentType = "text/txt"
                .ContentEncoding = Encoding.UTF8
                .Write(currentLock("LockId"))
                .Flush()
            End With
        End If
    End Sub

    Private Sub UnlockDiagram(ByVal context As HttpContext, ByVal filePath As String)
        If Not Directory.Exists(filePath) Then
            SetResponseStatus(context, 404)
            Exit Sub
        End If
        Try
            Dim lockCode As String = context.Request.QueryString("lockcode")
            Dim currentLock As Hashtable = GetCurrentLock(filePath)
            If currentLock.Count <> 0 Then
                If DirectCast(currentLock("LockId"), String) = lockCode Then
                    DeleteLock(filePath, lockCode)
                Else
                    SetResponseStatus(context, 500)
                    Exit Sub
                End If
            End If
        Catch
            SetResponseStatus(context, 500)
            Exit Sub
        End Try
        With context.Response
            .ContentType = "text/txt"
            .ContentEncoding = Encoding.UTF8
            .Write("OK")
            .Flush()
        End With
    End Sub

    Public Sub UpdateDiagram(ByVal context As HttpContext, ByVal filePath As String)
        If Not Directory.Exists(filePath) Then
            SetResponseStatus(context, 404)
            Exit Sub
        End If

        Dim lockCode As String = context.Request.QueryString("lockcode")
        Dim currentLock As Hashtable = GetCurrentLock(filePath)
        If currentLock.Count = 0 Then
            ' If no lock, error out
            SetResponseStatus(context, 500)
            Exit Sub
        End If
        If DirectCast(currentLock("LockId"), String) <> lockCode Then
            ' If invalid lock code, error out
            SetResponseStatus(context, 500)
            Exit Sub
        End If

        ' Read the incoming patch
        Dim xrs As XmlReaderSettings = ReaderSettings()
        Dim xr As XmlReader = XmlReader.Create(context.Request.InputStream, xrs)
        Dim df As DiagramPatch = DiagramPatch.Open(xr)
        xr.Close()

        ' Open the current diagram
        Dim currentFilePath As String = Path.Combine(filePath, currentFileName)
        xr = XmlReader.Create(currentFilePath, xrs)
        Dim currentDiagram As Diagram = Diagram.Open(xr)
        xr.Close()

        ' Match the revisions
        If df.Revision <> currentDiagram.Revision Then
            ' If revisions don't match, error out
            SetResponseStatus(context, 500)
            Exit Sub
        End If

        ' Save the patch
        Dim patchFilename As String = String.Format("{0}.dpatch", df.Revision)
        Dim xws As XmlWriterSettings = WriterSettings()
        Dim xw As XmlWriter = XmlWriter.Create(Path.Combine(filePath, patchFilename), xws)
        df.Save(xw)
        xw.Close()

        ' Apply the patch
        Dim newDiagram As Diagram = DiagramPatch.ApplyPatch(currentDiagram, df)

        ' Save the new diagram
        xw = XmlWriter.Create(currentFilePath, xws)
        newDiagram.Save(xw)
        xw.Close()

        ' TODO: Return something 
        context.Response.Flush()
    End Sub

    Private Sub ProcessPostRequest(ByVal context As System.Web.HttpContext, ByVal filePath As String, ByVal commandString As String)
        If Not Directory.Exists(filePath) Then
            SetResponseStatus(context, 404)
            Exit Sub
        End If

        Try
            Select Case LCase(commandString)
                Case "lock"
                    LockDiagram(context, filePath)
                Case "unlock"
                    UnlockDiagram(context, filePath)
                Case Else
                    UpdateDiagram(context, filePath)
            End Select
        Catch Ex As Exception
            SetResponseStatus(context, 500)
        End Try
    End Sub
#End Region

#Region " PUT "
    Private Sub StoreNewDiagram(ByVal context As HttpContext, ByVal filePath As String)
        If Directory.Exists(filePath) Then
            SetResponseStatus(context, 500)
            Exit Sub
        End If
        Try
            ' Read the incoming diagram
            Dim xrs As XmlReaderSettings = ReaderSettings()
            Dim rd As XmlReader = XmlReader.Create(context.Request.InputStream, xrs)
            Dim dg As Diagram = Diagram.Open(rd)

            Dim uploadLocation As String = Path.Combine(filePath, "")
            ' Create a directory
            Directory.CreateDirectory(uploadLocation)

            Dim originalFilePath As String = Path.Combine(uploadLocation, originalFileName)
            Dim currentFilePath As String = Path.Combine(uploadLocation, currentFileName)

            ' Save the base
            Dim xws As XmlWriterSettings = WriterSettings()
            Dim w As XmlWriter = XmlWriter.Create(originalFilePath, xws)
            dg.Save(w)
            w.Close()

            ' Apply the "zero" patch
            dg = DiffPatch.DiagramPatch.ApplyZeroPatch(dg)

            ' Save the "current" copy
            w = XmlWriter.Create(currentFilePath, xws)
            dg.Save(w)
            w.Close()

        Catch
            SetResponseStatus(context, 500)
        End Try
    End Sub
#End Region

    Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest

        Dim diagramPath As String = context.Request.Url.LocalPath
        Dim pageKey As String = context.Request.QueryString("page")
        Dim filePath As String = context.Server.MapPath(diagramPath)
        Dim commandString As String = context.Request.QueryString("cmd")


        filePath = GetDiagramPath(filePath)

        With context.Request
            Select Case .RequestType
                Case "PUT"      ' New diagram
                    StoreNewDiagram(context, filePath)
                Case "POST"     ' Lock/Update diagram
                    ProcessPostRequest(context, filePath, commandString)
                Case "GET"      ' Get/show diagram
                    ProcessGetRequest(context, pageKey, filePath, commandString)
                Case "DELETE"   ' Delete diagram

            End Select
        End With
    End Sub
End Class

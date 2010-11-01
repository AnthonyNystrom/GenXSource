Option Strict On
Option Explicit On

Imports System.Xml
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Windows.Forms
Imports Genetibase.NuGenObjective
Imports Genetibase.NuGenObjective.DiffPatch

Public Class ServerDiagram
    Inherits ClientDiagram

    Public Const DiagramExtension As String = ".serverdiagram"

    Public Enum OpenMode
        ForEditing
        ForPosting
        ReceivedFromServer
    End Enum

    Private Class ServerStatus
        Public Revision As Integer
        Public Lock As String
    End Class

    Private m_serverUrl As String
    Private m_baseDiagram As Diagram
    Private m_openMode As OpenMode
    Private m_lockCode As String

    Public Const RootNamespace As String = "http://genetibase.com/schemas/ServerDiagramSchema.xsd"

    Private Function GetCurrentStatus(ByVal wc As WebClient, ByVal serverURL As String) As ServerStatus
        Dim result As New ServerStatus
        Dim finalURLBuilder As New UriBuilder(serverURL)
        finalURLBuilder.Query = "cmd=status"

        Dim webStream As Stream = wc.OpenRead(finalURLBuilder.Uri)

        Dim xrs As XmlReaderSettings = ReaderSettings()
        Dim xr As XmlReader = XmlReader.Create(webStream, xrs)
        With xr
            .MoveToContent()
            .MoveToFirstAttribute()
            .ReadAttributeValue()
            result.Revision = .ReadContentAsInt
            .MoveToNextAttribute()
            .ReadAttributeValue()
            result.Lock = .ReadContentAsString
            .Close()
        End With
        Return result
    End Function

    Private Sub OpenBaseDiagram()
        Dim xrs As New XmlReaderSettings
        With xrs
            .IgnoreWhitespace = True
            .IgnoreComments = True
        End With
        Dim xr As XmlReader = XmlReader.Create(FileName, xrs)
        With xr
            .MoveToContent()
            .Read() ' Skip over the ServerDiagram
            .Skip() ' Skip over the ServerUrl
            .Read() ' Skip over the base
            m_baseDiagram = Diagram.Open(xr)
            xr.Close()
        End With
    End Sub

    Public ReadOnly Property BaseDiagram() As Diagram
        Get
            Return m_baseDiagram
        End Get
    End Property

    Public ReadOnly Property Mode() As OpenMode
        Get
            Return m_openMode
        End Get
    End Property

    Public Overrides Property ServerUrl() As String
        Get
            Return m_serverUrl
        End Get
        Set(ByVal value As String)
            m_serverUrl = value
        End Set
    End Property

    Public ReadOnly Property Locked() As Boolean
        Get
            Return m_lockCode <> ""
        End Get
    End Property

    Public Overrides Sub Save(ByVal filePath As String)
        If m_openMode = OpenMode.ForEditing Then
            OpenBaseDiagram()
        End If

        Dim xws As New XmlWriterSettings
        With xws
            .Indent = False
            .Encoding = Encoding.UTF8
        End With
        Dim xw As XmlWriter = XmlWriter.Create(filePath)
        With xw
            .WriteStartDocument()
            .WriteStartElement("ServerDiagram", RootNamespace)
            .WriteElementString("ServerURL", RootNamespace, m_serverUrl)

            .WriteStartElement("Base", RootNamespace)
            m_baseDiagram.Save(xw)
            .WriteEndElement()

            .WriteStartElement("Current", RootNamespace)
            CurrentDiagram.Save(xw)
            .WriteEndElement()

            .WriteEndElement()
            .Flush()
            .Close()
        End With

        If m_openMode = OpenMode.ReceivedFromServer OrElse m_openMode = OpenMode.ForEditing Then
            m_baseDiagram = Nothing
            m_openMode = OpenMode.ForEditing
        End If
        FileName = filePath
        Dirty = False
    End Sub

    Public Overrides Sub Publish()
        If FileName = "" OrElse Dirty Then
            RaiseMessage("The server diagram cannot be published, because it is not saved locally. Please save the diagram, and then Publish again.", _
                        MessageBoxButtons.OK, _
                        MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' Open a copy
        Dim sd As ServerDiagram
        sd = ServerDiagram.Open(FileName, ServerDiagram.OpenMode.ForPosting)

        ' Calculate diff
        Dim df As DiagramPatch = DiagramPatch.CreatePatch(sd.BaseDiagram, sd.CurrentDiagram)
        If df.DifferenceCount < 1 Then
            RaiseMessage("There have been no changes to the diagram. There is nothing to publish.", _
                        MessageBoxButtons.OK, _
                        MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' Check for a lock
        If Not Locked Then
            RaiseMessage("The server diagram is not locked. Please lock the diagram, and then Publish again.", _
                        MessageBoxButtons.OK, _
                        MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim wc As New WebClient

        ' Verify version
        Dim status As ServerStatus = GetCurrentStatus(wc, ServerUrl)
        If sd.BaseDiagram.Revision < status.Revision Then
            wc.Dispose()
            RaiseMessage("There is a new version of the diagram on the server. Your copy cannot be published.", _
                        MessageBoxButtons.OK, _
                        MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Publish
        Dim serverUri As New UriBuilder(sd.ServerUrl)
        serverUri.Query = "cmd=Update&lockcode=" & m_lockCode
        Dim webStream As Stream = wc.OpenWrite(serverUri.Uri, "POST")
        Dim xws As XmlWriterSettings = WriterSettings()
        xws.CloseOutput = True
        Dim xw As XmlWriter = XmlWriter.Create(webStream, xws)
        df.Save(xw)
        xw.Close()

        ' Try to get the current status
        status = GetCurrentStatus(wc, ServerUrl)
        With status
            If .Revision = sd.BaseDiagram.Revision + 1 Then
                m_openMode = OpenMode.ForPosting
                CurrentDiagram = DiagramPatch.ApplyPatch(sd.BaseDiagram, df)
                m_baseDiagram = CurrentDiagram.Clone
                Save(FileName)
                m_baseDiagram = Nothing
                m_openMode = OpenMode.ForEditing
                Dirty = False
                RaiseMessage("The diagram has been published.", MessageBoxButtons.OK, MessageBoxIcon.None)
            Else
                'Throw New InvalidOperationException("Could not publish.")
                RaiseMessage("Could not publish the diagram. Please contact your Administrator.", _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Error)
                Exit Sub
            End If
        End With

        wc.Dispose()
    End Sub

    Public Sub Lock()
        Dim result As String = ""
        Dim wc As New WebClient

        If GetCurrentStatus(wc, ServerUrl).Revision > CurrentDiagram.Revision Then
            RaiseMessage("There is a new version of the diagram on the server. Please update your copy, and the try locking again.", _
                        MessageBoxButtons.OK, _
                        MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim serverUri As New UriBuilder(ServerUrl)
        serverUri.Query = "cmd=Lock"

        result = wc.UploadString(serverUri.Uri, "")

        wc.Dispose()
        m_lockCode = result
    End Sub

    Public Sub Unlock()
        If Not Locked Then
            Exit Sub
        End If
        Dim wc As New WebClient
        Dim serverUri As New UriBuilder(ServerUrl)
        serverUri.Query = "cmd=Unlock&lockcode=" & m_lockCode
        Dim result As String = wc.UploadString(serverUri.Uri, "")
        If result = "OK" Then
            m_lockCode = ""
        End If
    End Sub

    Public Sub UpdateFromServer()
        If GetCurrentStatus(New WebClient, ServerUrl).Revision = CurrentDiagram.Revision Then
            RaiseMessage("The diagram is current.", _
                        MessageBoxButtons.OK, _
                        MessageBoxIcon.Information)
            Exit Sub
        End If
        If Locked Then
            OpenBaseDiagram()
            Dim df As DiagramPatch = DiagramPatch.CreatePatch(m_baseDiagram, CurrentDiagram)
            If df.DifferenceCount > 0 Then
                If RaiseMessage( _
                            "If you update, you will lose the modifications since you locked the diagram." & _
                            "Click Yes to discard the modifications and update." & vbLf & _
                            "Click No to keep your modifications, but not update.", _
                            MessageBoxButtons.YesNo, _
                            MessageBoxIcon.Question _
                        ) <> DialogResult.Yes Then
                    m_baseDiagram = Nothing
                    Exit Sub
                Else
                    Unlock()
                End If
            End If
        End If
        Dim sd As ServerDiagram = ServerDiagram.CheckOut(ServerUrl)
        m_openMode = OpenMode.ReceivedFromServer
        m_baseDiagram = sd.BaseDiagram
        CurrentDiagram = sd.CurrentDiagram
        Save(FileName)
        sd = Nothing
        m_baseDiagram = Nothing
        m_openMode = OpenMode.ForEditing
        Dirty = False
        RaiseMessage("The diagram has been updated.", MessageBoxButtons.OK, MessageBoxIcon.None)
    End Sub

    Public Shared Function Open(ByVal filePath As String, ByVal mode As OpenMode) As ServerDiagram
        Dim result As ServerDiagram
        Dim xrs As New XmlReaderSettings
        With xrs
            .IgnoreWhitespace = True
            .IgnoreComments = True
        End With
        Dim xr As XmlReader = XmlReader.Create(filePath, xrs)
        With xr
            .MoveToContent()
            .Read() ' Skip over the ServerDiagram tag

            Dim serverUrl As String = .ReadElementString("ServerURL", RootNamespace)
            result = New ServerDiagram(mode, serverUrl, filePath)

            If mode = OpenMode.ForEditing Then
                .Skip()
            Else
                .Read() ' Skip over the Base tag
                result.m_baseDiagram = Diagram.Open(xr)
                .Read() ' Skip over the end Base tag
            End If

            .Read() ' Skip over the Current tag
            result.CurrentDiagram = Diagram.Open(xr)

            ' Set the filename
            result.FileName = filePath
            result.Dirty = False

            .Close()

        End With

        Return result
    End Function

    Public Shared Function CheckOut(ByVal serverURL As String) As ServerDiagram
        Dim currentDiagram As Diagram
        Dim result As ServerDiagram

        Dim serverURI As New UriBuilder(serverURL)
        With serverURI
            Dim oldQueryString As String = ""
            If .Query.Length > 1 Then
                oldQueryString = .Query.Substring(1) & "&"
            End If
            .Query = oldQueryString & "cmd=checkout"
        End With


        Dim wc As New WebClient
        Dim webStream As Stream = wc.OpenRead(serverURI.Uri)

        Dim xrs As XmlReaderSettings = ReaderSettings()
        Dim xr As XmlReader = XmlReader.Create(webStream, xrs)
        With xr
            currentDiagram = Diagram.Open(xr)
            .Close()
        End With
        wc.Dispose()

        result = New ServerDiagram(currentDiagram, serverURL)
        result.Dirty = True

        Return result
    End Function

    Private Sub New(ByVal baseDiagram As Diagram, ByVal serverUrl As String)
        m_baseDiagram = baseDiagram
        CurrentDiagram = baseDiagram.Clone()
        m_serverUrl = serverUrl
        m_openMode = OpenMode.ReceivedFromServer
    End Sub

    Private Sub New(ByVal mode As OpenMode, ByVal serverUrl As String, ByVal originalPath As String)
        m_serverUrl = serverUrl
        m_openMode = mode
        FileName = originalPath
    End Sub
End Class

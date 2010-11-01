Option Strict On
Option Explicit On

Imports System.Xml
Imports System.Text
Imports System.Windows.Forms
Imports Genetibase.NuGenObjective

Public MustInherit Class ClientDiagram
    Private m_fileName As String = ""
    Private m_dirty As Boolean
    Private m_currentDiagram As Diagram

    Public Event FileNameChanged As EventHandler
    Public Event DirtyStateChanged As EventHandler
    Public Event Message As EventHandler(Of DiagramMessageEventArgs)

    Private Sub DirtyHandler(ByVal sender As Object, ByVal e As EventArgs)
        Dirty = True
    End Sub

    Private Sub GenericDirtyEventHandler(Of T As EventArgs)(ByVal sender As Object, ByVal e As T)
        Dirty = True
    End Sub

    Private Sub CollectionDirtyHandler(Of T)(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of T))
        Dirty = True
    End Sub

    Private Sub AddDirtyHandler(Of T As Element)(ByVal collection As ElementCollection(Of T))
        With collection
            AddHandler .Added, AddressOf CollectionDirtyHandler(Of T)
            AddHandler .Removed, AddressOf CollectionDirtyHandler(Of T)
            AddHandler .Changed, AddressOf CollectionDirtyHandler(Of T)
            AddHandler .ElementChanged, AddressOf DirtyHandler
            AddHandler .ElementRenamed, AddressOf GenericDirtyEventHandler(Of ElementRenamedEventArgs)
        End With
    End Sub

    Private Sub RemoveDirtyHandler(Of T As Element)(ByVal collection As ElementCollection(Of T))
        With collection
            RemoveHandler .Added, AddressOf CollectionDirtyHandler(Of T)
            RemoveHandler .Removed, AddressOf CollectionDirtyHandler(Of T)
            RemoveHandler .Changed, AddressOf CollectionDirtyHandler(Of T)
            RemoveHandler .ElementChanged, AddressOf DirtyHandler
            RemoveHandler .ElementRenamed, AddressOf GenericDirtyEventHandler(Of ElementRenamedEventArgs)
        End With
    End Sub

    Private Sub AddDirtyHandlers()
        If m_currentDiagram Is Nothing Then
            Exit Sub
        End If

        AddDirtyHandler(m_currentDiagram.Pages)

        AddDirtyHandler(m_currentDiagram.System.Roles)

        AddDirtyHandler(m_currentDiagram.System.Actions)

        AddDirtyHandler(m_currentDiagram.System.States)

        AddDirtyHandler(m_currentDiagram.System.Objects)
    End Sub

    Private Sub RemoveDirtyHandlers()
        If m_currentDiagram Is Nothing Then
            Exit Sub
        End If

        RemoveDirtyHandler(m_currentDiagram.Pages)

        RemoveDirtyHandler(m_currentDiagram.System.Roles)

        RemoveDirtyHandler(m_currentDiagram.System.Actions)

        RemoveDirtyHandler(m_currentDiagram.System.States)

        RemoveDirtyHandler(m_currentDiagram.System.Objects)
    End Sub

    Protected Shared Function ReaderSettings() As XmlReaderSettings
        Dim result As New XmlReaderSettings
        With result
            .IgnoreWhitespace = True
            .IgnoreProcessingInstructions = True
            .IgnoreComments = True
        End With
        Return result
    End Function

    Protected Shared Function WriterSettings() As XmlWriterSettings
        Dim result As New XmlWriterSettings
        With result
            .Indent = False
            .Encoding = Encoding.UTF8
        End With
        Return result
    End Function

    Protected Function RaiseMessage( _
                    ByVal message As String, _
                    ByVal buttons As MessageBoxButtons, _
                    ByVal icon As MessageBoxIcon _
                    ) As DialogResult
        Dim e As New DiagramMessageEventArgs
        With e
            .Message = message
            .MessageButtons = buttons
            .MessageIcon = icon
        End With
        RaiseEvent Message(Me, e)
        Return e.MessageResult
    End Function

    Public MustOverride Property ServerUrl() As String

    Public Property FileName() As String
        Get
            Return m_fileName
        End Get
        Protected Set(ByVal value As String)
            If value <> m_fileName Then
                m_fileName = value
                RaiseEvent FileNameChanged(Me, EventArgs.Empty)
            End If
        End Set
    End Property

    Public Property Dirty() As Boolean
        Get
            Return m_dirty
        End Get
        Protected Set(ByVal value As Boolean)
            If m_dirty <> value Then
                m_dirty = value
                RaiseEvent DirtyStateChanged(Me, EventArgs.Empty)
            End If
        End Set
    End Property

    Public Property CurrentDiagram() As Diagram
        Get
            Return m_currentDiagram
        End Get
        Protected Set(ByVal value As Diagram)
            If Not value Is m_currentDiagram Then
                RemoveDirtyHandlers()
            End If
            m_currentDiagram = value
            If value IsNot Nothing Then
                AddDirtyHandlers()
            End If
        End Set
    End Property

    Public MustOverride Sub Publish()

    Public MustOverride Sub Save(ByVal filePath As String)
End Class

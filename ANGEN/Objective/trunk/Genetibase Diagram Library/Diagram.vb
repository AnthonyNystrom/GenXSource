Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.IO
Imports System.Text

Public Class Diagram
    Private WithEvents m_system As New ModelSystem
    Private WithEvents m_pages As New ElementCollection(Of Page)(m_system)
    Private m_revision As Integer = -1

    Public Const RootNameSpace As String = "http://genetibase.com/Schemas/DiagramSchema.xsd"

    Private Sub SetCurrentRevision(ByVal e As Element)
        If e.Revision = -1 AndAlso m_revision <> -1 Then
            e.SetRevision(m_revision)
        End If
    End Sub

#Region " Collection and element events "
    Private Sub m_pages_Added(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Page)) Handles m_pages.Added, m_pages.Changed
        With e.Element
            If .ParentDiagram Is Nothing Then
                .SetDiagram(Me)
            End If
            SetCurrentRevision(e.Element)
        End With
    End Sub

    Private Sub m_pages_Changed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Page)) Handles m_pages.Changed
        SetCurrentRevision(e.Element)
    End Sub

    Private Sub m_pages_ElementChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_pages.ElementChanged
        Dim changedPage As Element = DirectCast(sender, Element)
        If m_revision <> (-1) Then
            changedPage.SetRevision(m_revision)
        End If
    End Sub

    Private Sub elementRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_system.ActionRenamed, m_system.InteractionRenamed, m_system.ObjectRenamed, m_system.RoleRenamed, m_system.StateRenamed
        Dim renamedElement As Element = DirectCast(sender, Element)
        If m_revision <> (-1) Then
            renamedElement.SetRevision(m_revision)
        End If
    End Sub

    Private Sub elementChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_system.ActionChanged, m_system.InteractionChanged, m_system.ObjectChanged, m_system.RoleChanged, m_system.StateChanged
        Dim changedElement As Element = DirectCast(sender, Element)
        If m_revision <> (-1) Then
            changedElement.SetRevision(m_revision)
        End If
    End Sub

    Private Sub m_system_ActionAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Action)) Handles m_system.ActionAdded, m_system.ActionReplaced
        SetCurrentRevision(e.Element)
    End Sub

    Private Sub m_system_InteractionAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction)) Handles m_system.InteractionAdded, m_system.InteractionReplaced
        SetCurrentRevision(e.Element)
    End Sub

    Private Sub m_system_ObjectAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of ModelObject)) Handles m_system.ObjectAdded, m_system.ObjectReplaced
        SetCurrentRevision(e.Element)
    End Sub

    Private Sub m_system_RoleAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Role)) Handles m_system.RoleAdded, m_system.RoleReplaced
        SetCurrentRevision(e.Element)
    End Sub

    Private Sub m_system_StateAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of State)) Handles m_system.StateAdded, m_system.StateReplaced
        SetCurrentRevision(e.Element)
    End Sub
#End Region

    Friend Sub IncrementRevision()
        m_revision += 1
    End Sub

    Public ReadOnly Property System() As ModelSystem
        Get
            Return m_system
        End Get
    End Property

    Public ReadOnly Property Pages() As ElementCollection(Of Page)
        Get
            Return m_pages
        End Get
    End Property

    Public ReadOnly Property Revision() As Integer
        Get
            Return m_revision
        End Get
    End Property

    Public Function NewPage() As Page
        Dim result As New Page(Me)
        result.Name = ModelSystem.GenerateUniqueName(Pages, "Page", "p_")
        Return result
    End Function

    Public Function AddNewPage() As Page
        Dim result As Page = NewPage()
        m_pages.Add(result)
        Return result
    End Function

    Public Sub Save(ByVal writer As XmlWriter)
        With writer
            '.WriteStartDocument()
            .WriteStartElement("Diagram", RootNameSpace)
            If m_revision > -1 Then
                .WriteElementString("Revision", RootNameSpace, m_revision.ToString(Globalization.CultureInfo.InvariantCulture))
            End If

            .WriteStartElement("ModelSystem")
            m_system.WriteXML(writer)
            .WriteEndElement()

            .WriteStartElement("Pages")
            For Each page As Page In Pages
                .WriteStartElement("Page")
                page.WriteXML(writer)
                .WriteEndElement()
            Next
            .WriteEndElement()

            .WriteEndElement()

            .Flush()
            ' Diagram should not close the stream
            ' .Close()
        End With
    End Sub

    Public Function Clone() As Diagram
        Dim result As Diagram
        Dim tempStorage As New StringBuilder
        Dim xw As XmlWriter = XmlWriter.Create(tempStorage)
        Save(xw)
        xw.Close()
        xw = Nothing

        Dim tempString As String = tempStorage.ToString
        Using sr As New StringReader(tempString)
            Dim xrs As New XmlReaderSettings
            xrs.IgnoreComments = True
            xrs.IgnoreWhitespace = True
            Dim xr As XmlReader = XmlReader.Create(sr, xrs)
            result = Diagram.Open(xr)
            xr.Close()
            xr = Nothing
        End Using

        tempString = ""
        tempStorage = Nothing
        Return result
    End Function



    Public Shared Function Open(ByVal reader As XmlReader) As Diagram
        Dim result As New Diagram
        With reader
            .MoveToContent()

            .ReadStartElement("Diagram", RootNameSpace)
            If .LocalName = "Revision" Then
                result.m_revision = .ReadElementContentAsInt("Revision", RootNameSpace)
            End If
            .ReadStartElement("ModelSystem", RootNameSpace)
            result.m_system = New ModelSystem()
            result.m_system.ReadXML(reader)
            .ReadEndElement()


            result.m_pages = New ElementCollection(Of Page)(result.m_system)

            If .IsEmptyElement AndAlso .Name = "Pages" Then
                .Read()
            Else
                .ReadStartElement("Pages")
                If Not .IsEmptyElement Then
                    Do While Not .EOF
                        If .Name = "Pages" Then
                            .Read() ' Skip over the Pages end tag
                            Exit Do
                        Else
                            .ReadStartElement("Page")
                            Dim currentPage As Page = result.NewPage()
                            currentPage.ReadXML(reader)
                            .ReadEndElement()
                            result.Pages.Add(currentPage)
                        End If
                    Loop
                End If
            End If
            .ReadEndElement()
        End With
        Return result
    End Function


End Class

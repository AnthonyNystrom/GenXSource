Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Xml

Namespace DiffPatch
    Public Class DiagramPatch

        Private m_revision As Integer

        Private m_roleDifferences As DiffCollection(Of Role)
        Private m_actionDifferences As DiffCollection(Of Action)
        Private m_stateDifferences As DiffCollection(Of State)
        Private m_objectDifferences As DiffCollection(Of ModelObject)
        Private m_pageDifferences As DiffCollection(Of Page)

        Public Const RootNameSpace As String = "http://genetibase.com/Schemas/PatchSchema.xsd"

#Region " Patch creation "
        Private Shared Function GetDiffs(Of T As Element)( _
                                    ByVal base As Collection(Of T), _
                                    ByVal modified As Collection(Of T) _
                                ) As DiffCollection(Of T)

            Dim diffs As UnixDiff1Algorithm(Of T)
            diffs = New UnixDiff1Algorithm(Of T)( _
                                                base, _
                                                modified, _
                                                False _
                                            )
            Return diffs.Differences
        End Function

        Public Shared Function CreatePatch( _
                                        ByVal baseDiagram As Diagram, _
                                        ByVal modifiedDiagram As Diagram _
                                    ) As DiagramPatch

            ' A patch always has the same revision as the 
            ' diagram it is patching
            Dim result As New DiagramPatch(baseDiagram.Revision)

            result.RoleDifferences = GetDiffs( _
                                        baseDiagram.System.Roles, _
                                        modifiedDiagram.System.Roles _
                                    )

            result.ActionDifferences = GetDiffs( _
                            baseDiagram.System.Actions, _
                            modifiedDiagram.System.Actions _
                        )


            result.StateDifferences = GetDiffs( _
                baseDiagram.System.States, _
                modifiedDiagram.System.States _
            )

            result.ObjectDifferences = GetDiffs( _
                baseDiagram.System.Objects, _
                modifiedDiagram.System.Objects _
            )

            result.PageDifferences = GetDiffs( _
                baseDiagram.Pages, _
                modifiedDiagram.Pages _
            )

            Return result
        End Function
#End Region

        Private Shared Sub ApplySimplePatchOnCollection(Of T As Element)( _
                        ByVal diffCollection As DiffCollection(Of T), _
                        ByVal elementCollection As ElementCollection(Of T) _
                    )

            For Each de As DiffEdit(Of T) In diffCollection
                Dim endPoint As Integer = de.StartOffset + (de.ChangeLength - 1)
                Select Case de.DiffOperation
                    Case DiffType.Delete
                        For i As Integer = de.StartOffset To endPoint
                            elementCollection.RemoveAt(de.StartOffset)
                        Next
                    Case DiffType.Insert
                        For i As Integer = de.StartOffset To endPoint
                            elementCollection.Insert(i, de.ChangedElements(i - de.StartOffset))
                        Next
                End Select
            Next
        End Sub

        Public Shared Function ApplyPatch(ByVal baseDiagram As Diagram, ByVal patch As DiagramPatch) As Diagram
            Dim result As Diagram = baseDiagram.Clone

            ApplySimplePatchOnCollection(patch.RoleDifferences, result.System.Roles)
            ApplySimplePatchOnCollection(patch.ActionDifferences, result.System.Actions)
            ApplySimplePatchOnCollection(patch.StateDifferences, result.System.States)
            ApplySimplePatchOnCollection(patch.ObjectDifferences, result.System.Objects)
            ApplySimplePatchOnCollection(patch.PageDifferences, result.Pages)

            ' Increment revision
            result.IncrementRevision()

            Return result
        End Function

        Public Shared Function ApplyZeroPatch(ByVal baseDiagram As Diagram) As Diagram
            Dim result As Diagram = baseDiagram.Clone()
            If result.Revision = -1 Then
                result.IncrementRevision()
                result = result.Clone
                result.IncrementRevision()
            End If
            Return result
        End Function

        Public ReadOnly Property Revision() As Integer
            Get
                Return m_revision
            End Get
        End Property

        Public Property RoleDifferences() As DiffCollection(Of Role)
            Get
                Return m_roleDifferences
            End Get
            Friend Set(ByVal value As DiffCollection(Of Role))
                m_roleDifferences = value
            End Set
        End Property

        Public Property ActionDifferences() As DiffCollection(Of Action)
            Get
                Return m_actionDifferences
            End Get
            Friend Set(ByVal value As DiffCollection(Of Action))
                m_actionDifferences = value
            End Set
        End Property

        Public Property StateDifferences() As DiffCollection(Of State)
            Get
                Return m_stateDifferences
            End Get
            Friend Set(ByVal value As DiffCollection(Of State))
                m_stateDifferences = value
            End Set
        End Property

        Public Property ObjectDifferences() As DiffCollection(Of ModelObject)
            Get
                Return m_objectDifferences
            End Get
            Friend Set(ByVal value As DiffCollection(Of ModelObject))
                m_objectDifferences = value
            End Set
        End Property

        Public Property PageDifferences() As DiffCollection(Of Page)
            Get
                Return m_pageDifferences
            End Get
            Friend Set(ByVal value As DiffCollection(Of Page))
                m_pageDifferences = value
            End Set
        End Property

        Public ReadOnly Property DifferenceCount() As Integer
            Get
                Return m_roleDifferences.Count + _
                        m_actionDifferences.Count + _
                        m_stateDifferences.Count + _
                        m_objectDifferences.Count + _
                        m_pageDifferences.Count

            End Get
        End Property

#Region " Save "
        Private Shared Sub WriteDifferences(Of T As Element)( _
                            ByVal writer As XmlWriter, _
                            ByVal diffs As DiffCollection(Of T), _
                            ByVal nameOfOuterElement As String _
                        )
            With writer
                .WriteStartElement(nameOfOuterElement, RootNameSpace)
                For Each d As DiffEdit(Of T) In diffs

                    .WriteStartElement("Change", RootNameSpace)
                    .WriteAttributeString("operation", d.DiffOperation.ToString)
                    .WriteElementString("Offset", RootNameSpace, d.StartOffset.ToString(Globalization.CultureInfo.InvariantCulture))
                    .WriteElementString("Length", RootNameSpace, d.ChangeLength.ToString(Globalization.CultureInfo.InvariantCulture))
                    .WriteStartElement("ChangedElements", RootNameSpace)
                    For Each e As Element In d.ChangedElements
                        ' TODO: The next line will change when element names are introduced
                        .WriteStartElement(TypeName(e), e.NameSpace)
                        e.WriteXML(writer)
                        .WriteEndElement()
                    Next
                    .WriteEndElement()
                    .WriteEndElement()

                Next
                .WriteEndElement()
            End With
        End Sub

        Public Sub Save(ByVal writer As XmlWriter)
            With writer
                .WriteStartDocument()
                .WriteStartElement("DiagramPatch", RootNameSpace)
                .WriteAttributeString("xmlns", "d", Nothing, Diagram.RootNameSpace)
                .WriteElementString("Revision", RootNameSpace, m_revision.ToString(Globalization.CultureInfo.InvariantCulture))
                WriteDifferences(writer, m_roleDifferences, "RoleChanges")
                WriteDifferences(writer, m_actionDifferences, "ActionChanges")
                WriteDifferences(writer, m_stateDifferences, "StateChanges")
                WriteDifferences(writer, m_objectDifferences, "ObjectChanges")
                WriteDifferences(writer, m_pageDifferences, "PageChanges")
                .WriteEndElement()
                .WriteEndDocument()
            End With
        End Sub
#End Region

#Region " Open "
        Private Shared Sub ReadDifferences(Of T As Element)( _
                            ByVal reader As XmlReader, _
                            ByVal diffs As DiffCollection(Of T), _
                            ByVal nameOfOuterElement As String _
                        )

            With reader
                If .IsEmptyElement Then
                    .Read() ' Read past the empty element
                    Exit Sub
                Else
                    ' Read the starting "collection" element
                    .ReadStartElement(nameOfOuterElement, RootNameSpace)
                End If
                Do While .Name = "Change"
                    If .IsEmptyElement Then
                        .Read() ' Read past an empty Change element
                    Else
                        ' Get the operation attribute
                        .MoveToAttribute("operation")
                        Dim diff As New DiffEdit(Of T)
                        diff.DiffOperation = DirectCast(DiffType.Parse(GetType(DiffType), .Value, True), DiffType)
                        .MoveToElement()
                        .Read() ' Read past the opening Change element
                        diff.StartOffset = .ReadElementContentAsInt("Offset", RootNameSpace)
                        diff.ChangeLength = .ReadElementContentAsInt("Length", RootNameSpace)

                        If .IsEmptyElement() Then
                            .Read()   ' Read past an empty ChangedElements element
                        Else
                            .ReadStartElement("ChangedElements", RootNameSpace)
                            Do Until (.LocalName = "ChangedElements" AndAlso .NamespaceURI = RootNameSpace)
                                Dim changedElement As Element = Nothing
                                Select Case .LocalName
                                    Case "Role"
                                        changedElement = New Role
                                    Case "Action"
                                        changedElement = New Action
                                    Case "State"
                                        changedElement = New State
                                    Case "ModelObject"
                                        changedElement = New ModelObject
                                    Case "Page"
                                        changedElement = New Page
                                    Case Else
                                        ' Extensibility here
                                End Select
                                If changedElement IsNot Nothing Then
                                    .Read() ' Read the starting element
                                    changedElement.ReadXML(reader)
                                    .Read() ' Read the ending element
                                    diff.ChangedElements.Add(DirectCast(changedElement, T))
                                End If
                            Loop
                            .Read() ' Read the ending ChangedElements element
                        End If
                        .ReadEndElement() ' Read the end Change element
                        diffs.Add(diff)
                    End If
                Loop
                ' Read the end "collection" element
                .ReadEndElement()
            End With
        End Sub

        Public Shared Function Open(ByVal reader As XmlReader) As DiagramPatch
            Dim result As New DiagramPatch()

            With reader
                .MoveToContent()
                .ReadStartElement("DiagramPatch", RootNameSpace)
                result.m_revision = .ReadElementContentAsInt("Revision", RootNameSpace)
                ' result.RoleDifferences = New DiffCollection(Of Role)
                ReadDifferences(reader, result.RoleDifferences, "RoleChanges")
                'result.ActionDifferences = New DiffCollection(Of Action)
                ReadDifferences(reader, result.ActionDifferences, "ActionChanges")
                'result.StateDifferences = New DiffCollection(Of State)
                ReadDifferences(reader, result.StateDifferences, "StateChanges")
                'result.ObjectDifferences = New DiffCollection(Of ModelObject)
                ReadDifferences(reader, result.ObjectDifferences, "ObjectChanges")
                'result.PageDifferences = New DiffCollection(Of Page)
                ReadDifferences(reader, result.PageDifferences, "PageChanges")
                .ReadEndElement()
            End With

            Return result
        End Function
#End Region

        Private Sub New(ByVal revisionNumber As Integer)
            m_revision = revisionNumber
        End Sub

        Public Sub New()
            m_revision = -1
            m_roleDifferences = New DiffCollection(Of Role)
            m_actionDifferences = New DiffCollection(Of Action)
            m_stateDifferences = New DiffCollection(Of State)
            m_objectDifferences = New DiffCollection(Of ModelObject)
            m_pageDifferences = New DiffCollection(Of Page)
        End Sub
    End Class
End Namespace


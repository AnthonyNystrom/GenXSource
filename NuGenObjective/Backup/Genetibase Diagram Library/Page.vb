Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.ComponentModel

''' <summary>
''' The Page class represents a page in the diagram.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class Page
    Inherits Element

    Private m_parentDiagram As Diagram
    Private WithEvents m_system As ModelSystem

    Private m_objects As New Collection(Of String)
    Private m_states As New Collection(Of String)
    Private m_roles As New Collection(Of String)
    Private m_actions As New Collection(Of String)
    Private m_interactions As New Collection(Of String)

    Public Event PageElementsChanged As EventHandler(Of PageElementsChangedEventArgs)

    Private Sub RaiseAddEvent(ByVal elementsChanged() As Element)
        Dim e As New PageElementsChangedEventArgs(elementsChanged, PageElementsChangedEventArgs.TypeOfChange.Added)
        RaiseEvent PageElementsChanged(Me, e)
        IndicateChange()
    End Sub

    Private Sub RaiseRemoveEvent(ByVal elementsChanged() As Element)
        Dim e As New PageElementsChangedEventArgs(elementsChanged, PageElementsChangedEventArgs.TypeOfChange.Removed)
        RaiseEvent PageElementsChanged(Me, e)
        IndicateChange()
    End Sub

    Private Shared Function InteractionAffectedByElement( _
                    ByVal interaction As Interaction, _
                    ByVal element As Element _
                ) As Boolean
        If TypeOf element Is State Then
            Return interaction.State Is element
        ElseIf TypeOf element Is Role Then
            Return interaction.Role Is element
        ElseIf TypeOf element Is Action Then
            Return interaction.Action Is element
        Else
            Return False
        End If
    End Function

#Region " Adding existing elements "
    Private Function AddExistingObject(ByVal key As String) As Boolean
        Dim result As Boolean = False
        If Not m_objects.Contains(key) Then
            m_objects.Add(key)
            result = True
        End If
        Return result
    End Function

    Private Function AddExistingRole(ByVal key As String) As Boolean
        Dim result As Boolean = False
        If Not m_roles.Contains(key) Then
            m_roles.Add(key)
            result = True
        End If
        Return result
    End Function

    Private Function AddExistingState(ByVal key As String) As Boolean
        Dim result As Boolean = False
        If Not m_states.Contains(key) Then
            m_states.Add(key)
            result = True
        End If
        Return result
    End Function

    Private Function AddExistingAction(ByVal key As String) As Boolean
        Dim result As Boolean = False
        If Not m_actions.Contains(key) Then
            m_actions.Add(key)
            result = True
        End If
        Return result
    End Function

    Private Function AddExistingInteraction(ByVal key As String) As Boolean
        Dim result As Boolean = False
        If Not m_interactions.Contains(key) Then
            m_interactions.Add(key)
            result = True
        End If
        Return result
    End Function
#End Region

#Region " Rename elements "
    Private Sub RenamePageElement( _
                ByVal collection As Collection(Of String), _
                ByVal oldKey As String, _
                ByVal element As Element _
                    )

        Dim position As Integer = collection.IndexOf(oldKey)
        If position <> -1 Then
            collection(position) = element.Key
            Dim changedElements() As Element = {element}
            RaiseEvent PageElementsChanged( _
                            Me, _
                            New PageElementsChangedEventArgs( _
                                    changedElements, _
                                    PageElementsChangedEventArgs.TypeOfChange.NameChanged _
                            ) _
                        )
            IndicateChange()
        End If
    End Sub



    Private Sub m_system_ActionRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_system.ActionRenamed
        RenamePageElement(m_actions, e.OldKey, DirectCast(sender, Action))
    End Sub

    Private Sub m_system_InteractionRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_system.InterActionRenamed
        RenamePageElement(m_interactions, e.OldKey, DirectCast(sender, Interaction))
    End Sub

    Private Sub m_system_ObjectRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_system.ObjectRenamed
        RenamePageElement(m_objects, e.OldKey, DirectCast(sender, ModelObject))
    End Sub

    Private Sub m_system_RoleRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_system.RoleRenamed
        RenamePageElement(m_roles, e.OldKey, DirectCast(sender, Role))
    End Sub

    Private Sub m_system_StateRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_system.StateRenamed
        RenamePageElement(m_states, e.OldKey, DirectCast(sender, State))
    End Sub
#End Region

#Region "Delete elements when they are removed from diagram"
    Private Sub m_system_ActionRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Action)) Handles m_system.ActionRemoved
        If m_actions.Contains(e.Element.Key) Then
            RemoveElement(e.Element)
        End If
    End Sub

    Private Sub m_system_InteractionRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction)) Handles m_system.InteractionRemoved
        If m_interactions.Contains(e.Element.Key) Then
            RemoveElement(e.Element)
        End If
    End Sub

    Private Sub m_system_ObjectRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of ModelObject)) Handles m_system.ObjectRemoved
        If m_objects.Contains(e.Element.Key) Then
            RemoveElement(e.Element)
        End If
    End Sub

    Private Sub m_system_RoleRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Role)) Handles m_system.RoleRemoved
        If m_roles.Contains(e.Element.Key) Then
            RemoveElement(e.Element)
        End If
    End Sub

    Private Sub m_system_StateRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of State)) Handles m_system.StateRemoved
        If m_states.Contains(e.Element.Key) Then
            RemoveElement(e.Element)
        End If
    End Sub
#End Region

#Region " Persistence "
    Private Shared Sub WriteCollection( _
                ByVal writer As XmlWriter, _
                ByVal collection As Collection(Of String), _
                ByVal nameOfEnclosingElement As String, _
                ByVal nameOfEachElement As String _
                )

        writer.WriteStartElement(nameOfEnclosingElement, Diagram.RootNameSpace)
        For Each item As String In collection
            writer.WriteElementString(nameOfEachElement, Diagram.RootNameSpace, item)
        Next
        writer.WriteEndElement()
    End Sub

    Private Shared Sub ReadCollection( _
                    ByVal reader As System.Xml.XmlReader, _
                    ByVal collection As Collection(Of String), _
                    ByVal nameOfEnclosingElement As String, _
                    ByVal nameOfEachElement As String _
                    )

        With reader
            If .IsEmptyElement AndAlso .LocalName = nameOfEnclosingElement Then
                .Read()
                Exit Sub
            End If

            .ReadStartElement(nameOfEnclosingElement, Diagram.RootNameSpace)
            Do While Not .EOF
                If .LocalName = nameOfEnclosingElement Then
                    Exit Do
                Else
                    Dim key As String = reader.ReadElementContentAsString(nameOfEachElement, Diagram.RootNameSpace)
                    collection.Add(key)
                End If
            Loop
            .ReadEndElement()
        End With
    End Sub

    Protected Overrides Sub OnSave(ByVal writer As System.Xml.XmlWriter)
        WriteCollection(writer, Roles, "RoleRefs", "RoleRef")
        WriteCollection(writer, Actions, "ActionRefs", "ActionRef")
        WriteCollection(writer, States, "StateRefs", "StateRef")
        WriteCollection(writer, Objects, "ObjectRefs", "ObjectRef")
        WriteCollection(writer, Interactions, "InteractionRefs", "InteractionRef")
    End Sub

    Protected Overrides Sub OnOpen(ByVal reader As System.Xml.XmlReader)
        ReadCollection(reader, Roles, "RoleRefs", "RoleRef")
        ReadCollection(reader, Actions, "ActionRefs", "ActionRef")
        ReadCollection(reader, States, "StateRefs", "StateRef")
        ReadCollection(reader, Objects, "ObjectRefs", "ObjectRef")
        ReadCollection(reader, Interactions, "InteractionRefs", "InteractionRef")
    End Sub
#End Region

    Friend Sub SetDiagram(ByVal diagram As Diagram)
        m_parentDiagram = diagram
    End Sub

    Private Shared Function CollectionEqualsTo(ByVal firstCollection As Collection(Of String), ByVal secondCollection As Collection(Of String)) As Boolean
        Dim result As Boolean = True
        ' If the count of the two collections are different
        ' the collections are different
        If firstCollection.Count <> secondCollection.Count Then
            result = False
        Else
            ' If anything in one collection does not exist
            ' in the other, there is no difference
            For Each s As String In firstCollection
                If Not secondCollection.Contains(s) Then
                    result = False
                    Exit For
                End If
            Next
        End If
        Return result
    End Function

    Public Overrides Function EqualsTo(ByVal other As Element) As Boolean
        Dim result As Boolean = MyBase.EqualsTo(other)
        Dim otherPage As Page = DirectCast(other, Page)

        If result Then result = CollectionEqualsTo(m_roles, otherPage.Roles)
        If result Then result = CollectionEqualsTo(m_actions, otherPage.Actions)
        If result Then result = CollectionEqualsTo(m_states, otherPage.States)
        If result Then result = CollectionEqualsTo(m_objects, otherPage.Objects)
        If result Then result = CollectionEqualsTo(m_interactions, otherPage.Interactions)

        Return result
    End Function

    Public Overrides ReadOnly Property Key() As String
        Get
            Return "p_" & MyBase.Name
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Objects() As Collection(Of String)
        Get
            Return m_objects
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Actions() As Collection(Of String)
        Get
            Return m_actions
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Roles() As Collection(Of String)
        Get
            Return m_roles
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property States() As Collection(Of String)
        Get
            Return m_states
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Interactions() As Collection(Of String)
        Get
            Return m_interactions
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ParentDiagram() As Diagram
        Get
            Return m_parentDiagram
        End Get
    End Property

    Public Sub AddNewAction()
        Dim newObject As Action = m_parentDiagram.System.AddNewAction()
        AddExistingElement(newObject)
    End Sub

    Public Sub AddNewState()
        Dim newObject As State = m_parentDiagram.System.AddNewState()
        AddExistingElement(newObject)
    End Sub

    Public Sub AddNewRole()
        Dim newObject As Role = m_parentDiagram.System.AddNewRole()
        AddExistingElement(newObject)
    End Sub

    Public Sub AddNewObject()
        Dim newObject As ModelObject = m_parentDiagram.System.AddNewObject()
        AddExistingElement(newObject)
    End Sub

    Public Overloads Sub AddExistingElement(ByVal modelObject As ModelObject)
        Dim addedElements As New List(Of Element)
        If AddExistingObject(modelObject.Key) Then addedElements.Add(modelObject)
        For Each interaction As Interaction In modelObject.Interactions
            AddExistingElement(interaction)
        Next

        RaiseAddEvent(addedElements.ToArray())
    End Sub

    Public Overloads Sub AddExistingElement(ByVal interaction As Interaction)
        Dim addedElements As New List(Of Element)
        If Not m_objects.Contains(interaction.[Object].Key) Then
            If AddExistingObject(interaction.[Object].Key) Then addedElements.Add(interaction.[Object])
        End If
        If Not m_states.Contains(interaction.State.Key) Then
            If AddExistingState(interaction.State.Key) Then addedElements.Add(interaction.State)
        End If
        If Not m_roles.Contains(interaction.Role.Key) Then
            If AddExistingRole(interaction.Role.Key) Then addedElements.Add(interaction.Role)
        End If
        If Not m_actions.Contains(interaction.Action.Key) Then
            If AddExistingAction(interaction.Action.Key) Then addedElements.Add(interaction.Action)
        End If
        If AddExistingInteraction(interaction.Key) Then addedElements.Add(interaction)

        RaiseAddEvent(addedElements.ToArray())
    End Sub

    Public Overloads Sub AddExistingElement(ByVal element As Element)
        If element Is Nothing Then
            Exit Sub
        End If

        Dim addedElement() As Element = {}

        Dim result As Boolean = False
        If TypeOf element Is State Then
            result = AddExistingState(element.Key)
        ElseIf TypeOf element Is Role Then
            result = AddExistingRole(element.Key)
        ElseIf TypeOf element Is Action Then
            result = AddExistingAction(element.Key)
        End If
        If result Then
            addedElement = New Element() {element}
        End If

        RaiseAddEvent(addedElement)
    End Sub

    Public Overloads Sub RemoveElement(ByVal interaction As Interaction)
        Dim elementsChanged(0) As Element
        m_interactions.Remove(interaction.Key)
        elementsChanged(0) = interaction
        RaiseRemoveEvent(elementsChanged)
    End Sub

    Public Overloads Sub RemoveElement(ByVal modelObject As ModelObject)
        Dim elementsChanged As New List(Of Element)
        For Each interaction As Interaction In modelObject.Interactions
            RemoveElement(interaction)
            elementsChanged.Add(interaction)
        Next
        m_objects.Remove(modelObject.Key)
        elementsChanged.Add(modelObject)
        RaiseRemoveEvent(elementsChanged.ToArray())
    End Sub

    Public Overloads Sub RemoveElement(ByVal element As Element)
        Dim elementsChanged As New List(Of Element)
        ' Remove interactions
        Dim deletionList As New List(Of String)
        For Each interactionKey As String In m_interactions
            Dim interaction As Interaction = ParentDiagram.System.GetInteraction(interactionKey)
            If interaction IsNot Nothing Then
                If InteractionAffectedByElement(interaction, element) Then
                    deletionList.Add(interactionKey)
                    elementsChanged.Add(interaction)
                End If
            End If
        Next

        For Each i As String In deletionList
            m_interactions.Remove(i)
        Next

        ' Remove element itself
        If TypeOf element Is State Then
            m_states.Remove(element.Key)
        ElseIf TypeOf element Is Role Then
            m_roles.Remove(element.Key)
        ElseIf TypeOf element Is Action Then
            m_actions.Remove(element.Key)
        End If

        elementsChanged.Add(element)
        RaiseRemoveEvent(elementsChanged.ToArray())
    End Sub

    Friend Sub New(ByVal parent As Diagram)
        MyBase.New(parent.System)
        m_parentDiagram = parent
        m_system = parent.System
    End Sub

    Public Sub New()

    End Sub

End Class

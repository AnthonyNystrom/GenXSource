Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.Text
Imports System.Globalization

''' <summary>
''' The ModelSystem class represents a real-life system that is being modelled.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class ModelSystem
    Private m_name As String

    Private WithEvents m_objects As New ElementCollection(Of ModelObject)(Me)
    Private WithEvents m_states As New ElementCollection(Of State)(Me)
    Private WithEvents m_roles As New ElementCollection(Of Role)(Me)
    Private WithEvents m_actions As New ElementCollection(Of Action)(Me)

    Public Event ObjectAdded As EventHandler(Of ElementCollectionEventArgs(Of ModelObject))
    Public Event ObjectRemoved As EventHandler(Of ElementCollectionEventArgs(Of ModelObject))
    Public Event ObjectReplaced As EventHandler(Of ElementCollectionEventArgs(Of ModelObject))
    Public Event ObjectRenamed As EventHandler(Of ElementRenamedEventArgs)
    Public Event ObjectChanged As EventHandler

    Public Event StateAdded As EventHandler(Of ElementCollectionEventArgs(Of State))
    Public Event StateRemoved As EventHandler(Of ElementCollectionEventArgs(Of State))
    Public Event StateReplaced As EventHandler(Of ElementCollectionEventArgs(Of State))
    Public Event StateRenamed As EventHandler(Of ElementRenamedEventArgs)
    Public Event StateChanged As EventHandler

    Public Event RoleAdded As EventHandler(Of ElementCollectionEventArgs(Of Role))
    Public Event RoleRemoved As EventHandler(Of ElementCollectionEventArgs(Of Role))
    Public Event RoleReplaced As EventHandler(Of ElementCollectionEventArgs(Of Role))
    Public Event RoleRenamed As EventHandler(Of ElementRenamedEventArgs)
    Public Event RoleChanged As EventHandler

    Public Event ActionAdded As EventHandler(Of ElementCollectionEventArgs(Of Action))
    Public Event ActionRemoved As EventHandler(Of ElementCollectionEventArgs(Of Action))
    Public Event ActionReplaced As EventHandler(Of ElementCollectionEventArgs(Of Action))
    Public Event ActionRenamed As EventHandler(Of ElementRenamedEventArgs)
    Public Event ActionChanged As EventHandler

    Public Event InteractionAdded As EventHandler(Of ElementCollectionEventArgs(Of Interaction))
    Public Event InteractionRemoved As EventHandler(Of ElementCollectionEventArgs(Of Interaction))
    Public Event InteractionReplaced As EventHandler(Of ElementCollectionEventArgs(Of Interaction))
    Public Event InteractionRenamed As EventHandler(Of ElementRenamedEventArgs)
    Public Event InteractionChanged As EventHandler

    Friend Shared Function GenerateUniqueName(Of T As Element)(ByVal dictionary As ElementCollection(Of T), ByVal rootName As String, ByVal keyRoot As String) As String
        Dim template As String = "{0}{1}"
        Dim result As String = ""
        Dim key As String = ""
        For i As Integer = 1 To 255
            result = String.Format(CultureInfo.InvariantCulture, template, rootName, i)
            key = String.Format(CultureInfo.InvariantCulture, template, keyRoot, result)
            If Not dictionary.Contains(key) Then
                Exit For
            Else
                result = ""
            End If
        Next
        If result = "" Then
            Throw New ArgumentOutOfRangeException("You have >255 generically named elements. That is NOT good diagramming practice.")
        Else
            Return result
        End If
    End Function

#Region " Collection Event Handlers "
    Private Sub m_objects_Added(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of ModelObject)) Handles m_objects.Added
        With e.Element
            AddHandler .InteractionAdded, AddressOf ObjectInteractionAdded
            AddHandler .InteractionReplaced, AddressOf ObjectInteractionReplaced
            AddHandler .InteractionRemoved, AddressOf ObjectInteractionRemoved
            AddHandler .InteractionRenamed, AddressOf ObjectInteractionRenamed
            AddHandler .InteractionChanged, AddressOf ObjectInteractionChanged
        End With
        RaiseEvent ObjectAdded(Me, e)
    End Sub

    Private Sub m_objects_Changed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of ModelObject)) Handles m_objects.Changed
        RaiseEvent ObjectReplaced(Me, e)
    End Sub

    Private Sub m_objects_ElementChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_objects.ElementChanged
        RaiseEvent ObjectChanged(sender, e)
    End Sub

    Private Sub m_objects_ElementRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_objects.ElementRenamed
        RaiseEvent ObjectRenamed(sender, e)
    End Sub

    Private Sub m_objects_Removed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of ModelObject)) Handles m_objects.Removed
        With e.Element
            RemoveHandler .InteractionAdded, AddressOf ObjectInteractionAdded
            RemoveHandler .InteractionReplaced, AddressOf ObjectInteractionReplaced
            RemoveHandler .InteractionRemoved, AddressOf ObjectInteractionRemoved
            RemoveHandler .InteractionRenamed, AddressOf ObjectInteractionRenamed
            AddHandler .InteractionChanged, AddressOf ObjectInteractionChanged
        End With
        RaiseEvent ObjectRemoved(Me, e)
    End Sub

    Private Sub m_actions_Added(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Action)) Handles m_actions.Added
        RaiseEvent ActionAdded(Me, e)
    End Sub

    Private Sub m_actions_Changed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Action)) Handles m_actions.Changed
        RaiseEvent ActionReplaced(Me, e)
    End Sub

    Private Sub m_actions_ElementChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_actions.ElementChanged
        RaiseEvent ActionChanged(sender, e)
    End Sub

    Private Sub m_actions_ElementRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_actions.ElementRenamed
        RaiseEvent ActionRenamed(sender, e)
    End Sub

    Private Sub m_actions_Removed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Action)) Handles m_actions.Removed
        RaiseEvent ActionRemoved(Me, e)
    End Sub

    Private Sub m_roles_Added(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Role)) Handles m_roles.Added
        RaiseEvent RoleAdded(Me, e)
    End Sub

    Private Sub m_roles_Changed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Role)) Handles m_roles.Changed
        RaiseEvent RoleReplaced(Me, e)
    End Sub

    Private Sub m_roles_ElementChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_roles.ElementChanged
        RaiseEvent RoleChanged(sender, e)
    End Sub

    Private Sub m_roles_ElementRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_roles.ElementRenamed
        RaiseEvent RoleRenamed(sender, e)
    End Sub

    Private Sub m_roles_Removed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Role)) Handles m_roles.Removed
        RaiseEvent RoleRemoved(Me, e)
    End Sub

    Private Sub m_states_Added(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of State)) Handles m_states.Added
        RaiseEvent StateAdded(Me, e)
    End Sub

    Private Sub m_states_Changed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of State)) Handles m_states.Changed
        RaiseEvent StateReplaced(Me, e)
    End Sub

    Private Sub m_states_ElementChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_states.ElementChanged
        RaiseEvent StateChanged(sender, e)
    End Sub

    Private Sub m_states_ElementRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_states.ElementRenamed
        RaiseEvent StateRenamed(sender, e)
    End Sub

    Private Sub m_states_Removed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of State)) Handles m_states.Removed
        RaiseEvent StateRemoved(Me, e)
    End Sub

    Private Sub ObjectInteractionAdded(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction))
        RaiseEvent InteractionAdded(Me, e)
    End Sub

    Private Sub ObjectInteractionReplaced(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction))
        RaiseEvent InteractionReplaced(Me, e)
    End Sub

    Private Sub ObjectInteractionRemoved(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction))
        RaiseEvent InteractionRemoved(Me, e)
    End Sub

    Private Sub ObjectInteractionRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs)
        RaiseEvent InteractionRenamed(sender, e)
    End Sub

    Private Sub ObjectInteractionChanged(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent InteractionChanged(sender, e)
    End Sub
#End Region

#Region " Persistence "
    Private Shared Sub WriteCollection(Of T As Element)( _
                ByVal writer As XmlWriter, _
                ByVal collection As Collection(Of T), _
                ByVal nameOfEnclosingElement As String _
                    )
        With writer
            ' Write the enclosing "collection element"
            .WriteStartElement(nameOfEnclosingElement)

            ' Write each element, using the type name
            ' and the NameSpace property of the element
            For Each item As T In collection
                .WriteStartElement(TypeName(item), item.NameSpace)
                item.WriteXML(writer)
                .WriteEndElement()
            Next
            .WriteEndElement()
        End With
    End Sub

    Private Shared Sub ReadCollection(Of T As Element)( _
                ByVal reader As XmlReader, _
                ByVal collection As Collection(Of T), _
                ByVal nameOfContainingElement As String _
                )
        With reader
            ' Handle the possibility that the collection being
            ' read is empty
            If .IsEmptyElement() AndAlso .Name = nameOfContainingElement Then
                .Read()
                Exit Sub
            End If
            ' Read the starting "collection element"
            .ReadStartElement(nameOfContainingElement)
            Do While Not .EOF
                ' Stop reading when the end "collection element"
                ' is read
                If .Name = nameOfContainingElement Then
                    Exit Do
                Else
                    Dim newItem As Element
                    'Dim newItemTypeName As String = GetType(T).Name

                    'If newItemTypeName = "Role" Then
                    '    newItem = DirectCast(Me.AddNewRole(), Element)
                    'ElseIf newItemTypeName = "State" Then
                    '    newItem = DirectCast(Me.AddNewState(), Element)
                    'ElseIf newItemTypeName = "Action" Then
                    '    newItem = DirectCast(Me.AddNewAction(), Element)
                    'ElseIf newItemTypeName = "ModelObject" Then
                    '    newItem = DirectCast(Me.AddNewObject(), Element)
                    'End If

                    'If newItem IsNot Nothing Then
                    '.ReadStartElement()
                    ' TODO: This needs to change to diagram-supplied
                    newItem = New ElementFactoryBase().CreateElement(.LocalName, .NamespaceURI)
                    .Read()
                    newItem.ReadXML(reader)
                    .ReadEndElement()
                    'End If

                    collection.Add(TryCast(newItem, T))
                End If
            Loop
            .ReadEndElement()
        End With
    End Sub

    Friend Sub WriteXML(ByVal writer As Xml.XmlWriter)
        With writer
            .WriteElementString("Name", Diagram.RootNameSpace, m_name)
            WriteCollection(writer, Roles, "Roles")
            WriteCollection(writer, Actions, "Actions")
            WriteCollection(writer, States, "States")
            WriteCollection(writer, Objects, "Objects")
        End With
    End Sub

    Friend Sub ReadXML(ByVal reader As XmlReader)
        m_name = reader.ReadElementContentAsString("Name", Diagram.RootNameSpace)
        ReadCollection(reader, Roles, "Roles")
        ReadCollection(reader, Actions, "Actions")
        ReadCollection(reader, States, "States")
        ReadCollection(reader, Objects, "Objects")
    End Sub
#End Region

    Public Function GetInteraction(ByVal key As String) As Interaction
        Dim splitStrings() As String = {"_i_"}
        Dim names() As String = key.Split(splitStrings, StringSplitOptions.None)
        Dim modelObject As ModelObject = m_objects(names(0))
        For Each interaction As Interaction In modelObject.Interactions
            If interaction.Key = key Then
                Return interaction
            End If
        Next
        Return Nothing
    End Function

    <Browsable(True), _
    ParenthesizePropertyName(True)> _
    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property

    Public ReadOnly Property Objects() As ElementCollection(Of ModelObject)
        Get
            Return m_objects
        End Get
    End Property

    Public ReadOnly Property States() As ElementCollection(Of State)
        Get
            Return m_states
        End Get
    End Property

    Public ReadOnly Property Roles() As ElementCollection(Of Role)
        Get
            Return m_roles
        End Get
    End Property

    Public ReadOnly Property Actions() As ElementCollection(Of Action)
        Get
            Return m_actions
        End Get
    End Property

    Public Overloads Function AddNewObject(ByVal name As String) As ModelObject
        Dim result As New ModelObject(Me)
        result.Name = name
        m_objects.Add(result)
        Return result
    End Function

    Public Overloads Function AddNewObject() As ModelObject
        Dim name As String = GenerateUniqueName(m_objects, "Object", "o_")
        Return AddNewObject(name)
    End Function

    Public Function AddNewState(ByVal name As String) As State
        Dim result As New State(Me)
        result.Name = name
        m_states.Add(result)
        Return result
    End Function

    Public Function AddNewState() As State
        Dim name As String = GenerateUniqueName(m_states, "State", "s_")
        Return AddNewState(name)
    End Function

    Public Function AddNewRole(ByVal name As String) As Role
        Dim result As New Role(Me)
        result.Name = name
        m_roles.Add(result)
        Return result
    End Function

    Public Function AddNewRole() As Role
        Dim name As String = GenerateUniqueName(m_roles, "Role", "r_")
        Return AddNewRole(name)
    End Function

    Public Function AddNewAction(ByVal name As String) As Action
        Dim result As New Action(Me)
        result.Name = name
        m_actions.Add(result)
        Return result
    End Function

    Public Function AddNewAction() As Action
        Dim name As String = GenerateUniqueName(m_actions, "Action", "a_")
        Return AddNewAction(name)
    End Function

    Public Function CopyElementToXML(ByVal element As Element) As String
        Dim result As New StringBuilder
        If element Is Nothing Then
            Throw New ArgumentNullException
        Else
            If TypeOf element Is Interaction Then
                ' TODO: figure out a way to copy interations
                Throw New NotImplementedException("Cannot cut/copy/paste interactions.")
            ElseIf element.System Is Me Then

                Dim xWriter As XmlWriter = XmlWriter.Create(result)
                With xWriter
                    '.Settings.Indent = True
                    .WriteStartElement(TypeName(element), element.NameSpace)
                    element.WriteXML(xWriter)
                    .WriteEndElement()
                    .Flush()
                    .Close()
                End With
            Else
                Throw New ArgumentException("The requested element is not a member of the system.")
            End If
        End If
        Return result.ToString()
    End Function

    Public Function CutElementToXML(ByVal element As Element) As String
        Dim result As String = CopyElementToXML(element)

        Dim modelObject As ModelObject = TryCast(element, ModelObject)
        If modelObject IsNot Nothing Then
            Objects.Remove(modelObject)
        Else
            Dim state As State = TryCast(element, State)
            If state IsNot Nothing Then
                States.Remove(state)
            Else
                Dim role As Role = TryCast(element, Role)
                If role IsNot Nothing Then
                    Roles.Remove(role)
                Else
                    Dim action As Action = TryCast(element, Action)
                    If action IsNot Nothing Then
                        Actions.Remove(action)
                    End If
                End If
            End If
        End If

        Return result
    End Function

    Public Function PasteElementFromXML(ByVal elementXML As String) As Element
        Dim xs As New XmlReaderSettings
        xs.IgnoreComments = True
        xs.IgnoreWhitespace = True

        Dim xtr As New IO.StringReader(elementXML)

        Dim xr As XmlReader = XmlReader.Create(xtr, xs)
        Dim element As Element

        With xr

            .MoveToContent()

            ' TODO: This also needs to change to diagram-supplied
            element = New ElementFactoryBase().CreateElement(.LocalName, .NamespaceURI)
            .Read()
            element.ReadXML(xr)
            .Close()
        End With

        Dim modelObject As ModelObject = TryCast(element, ModelObject)
        If modelObject IsNot Nothing Then
            Objects.Paste(modelObject)
        Else
            Dim state As State = TryCast(element, State)
            If state IsNot Nothing Then
                States.Paste(state)
            Else
                Dim role As Role = TryCast(element, Role)
                If role IsNot Nothing Then
                    Roles.Paste(role)
                Else
                    Dim action As Action = TryCast(element, Action)
                    If action IsNot Nothing Then
                        Actions.Paste(action)
                    Else
                        Throw New NotImplementedException("Cannot cut/copy/paste interactions.")
                    End If
                End If
            End If
        End If

        Return element
    End Function

End Class

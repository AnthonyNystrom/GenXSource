Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Xml

''' <summary>
''' The ModelObject class represents any object in the system being modelled.
''' A ModelObject contains zero or more Interactions. 
''' </summary>
''' <remarks></remarks>
Public Class ModelObject
    Inherits Element

    Private WithEvents m_interactions As ElementCollection(Of Interaction)
    Private m_interactionKeys As Dictionary(Of Interaction, String)
    Public Event InteractionAdded As EventHandler(Of ElementCollectionEventArgs(Of Interaction))
    Public Event InteractionRemoved As EventHandler(Of ElementCollectionEventArgs(Of Interaction))
    Public Event InteractionReplaced As EventHandler(Of ElementCollectionEventArgs(Of Interaction))
    Public Event InteractionRenamed As EventHandler(Of ElementRenamedEventArgs)
    Public Event InteractionChanged As EventHandler

    Private Sub m_interactions_Added(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction)) Handles m_interactions.Added
        RaiseEvent InteractionAdded(Me, e)
        IndicateChange()
    End Sub

    Private Sub m_interactions_Changed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction)) Handles m_interactions.Changed
        RaiseEvent InteractionReplaced(Me, e)
        IndicateChange()
    End Sub

    Private Sub m_interactions_ElementChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_interactions.ElementChanged
        RaiseEvent InteractionChanged(sender, e)
        IndicateChange()
    End Sub

    Private Sub m_interactions_ElementRenamed(ByVal sender As Object, ByVal e As ElementRenamedEventArgs) Handles m_interactions.ElementRenamed
        RaiseEvent InteractionRenamed(sender, e)
        IndicateChange()
    End Sub

    Private Sub m_interactions_Removed(ByVal sender As Object, ByVal e As ElementCollectionEventArgs(Of Interaction)) Handles m_interactions.Removed
        RaiseEvent InteractionRemoved(Me, e)
        IndicateChange()
    End Sub

    Private Overloads Function AddNewInteraction( _
        ByVal role As Role, _
        ByVal action As Action, _
        ByVal state As State) As Interaction

        Return AddNewInteraction(role, action, state, "")
    End Function

    Friend Sub InteractionTriggerDelete(ByVal interactionKey As String)
        If m_interactions.Contains(interactionKey) Then
            m_interactions.Remove(interactionKey)
        End If
    End Sub

    Protected Overrides Sub OnRenaming(ByVal newName As String)
        ' Store old interaction keys
        m_interactionKeys = New Dictionary(Of Interaction, String)
        For Each interaction As Interaction In m_interactions
            m_interactionKeys.Add(interaction, interaction.Key)
        Next
    End Sub

    Protected Overrides Sub OnRename(ByVal oldKey As String)
        ' Trigger interaction rename
        For Each interaction As Interaction In m_interactions
            interaction.ObjectTriggerRename( _
                            m_interactionKeys(interaction) _
                            )
        Next
        ' Rename the object
        MyBase.OnRename(oldKey)
        m_interactionKeys = Nothing
    End Sub

    Protected Overrides Sub OnSave(ByVal writer As System.Xml.XmlWriter)
        writer.WriteStartElement("Interactions", Diagram.RootNameSpace)
        For Each interaction As Interaction In Interactions
            writer.WriteStartElement(TypeName(interaction), interaction.NameSpace)
            interaction.WriteXML(writer)
            writer.WriteEndElement()
        Next
        writer.WriteEndElement()
    End Sub

    Protected Overrides Sub OnOpen(ByVal reader As System.Xml.XmlReader)

        If reader.IsEmptyElement() AndAlso reader.LocalName = "Interactions" Then
            reader.Read()
            Exit Sub
        End If

        reader.ReadStartElement("Interactions", Diagram.RootNameSpace)

        Do While Not reader.EOF
            If reader.LocalName = "Interactions" Then
                Exit Do
            Else
                ' reader.ReadStartElement("Interaction", Diagram.RootNameSpace)
                Dim i As Interaction
                ' TODO: Change this to be system-supplied
                i = DirectCast( _
                        New ElementFactoryBase().CreateElement(reader.LocalName, reader.NamespaceURI), _
                        Interaction _
                    )

                '= Me.AddNewInteraction(Nothing, Nothing, Nothing)
                'i.SetSystem(Me.System)
                i.SetObject(Me)
                reader.Read()
                i.ReadXML(reader)
                reader.ReadEndElement()
                m_interactions.Add(i)
            End If
        Loop
        reader.ReadEndElement()
    End Sub

    Protected Overrides Sub OnSetSystem(ByVal system As ModelSystem)
        MyBase.OnSetSystem(system)
        m_interactions.SetSystem(system)
        ' In case there are interactions already present
        If m_interactions.Count > 0 Then
            For Each i As Interaction In m_interactions
                i.SetSystem(system)
            Next
        End If
    End Sub

    '<Browsable(False)> _
    Public Overrides ReadOnly Property Key() As String
        Get
            Return "o_" & MyBase.Name
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Interactions() As ElementCollection(Of Interaction)
        Get
            Return m_interactions
        End Get
    End Property

    Public Function NewInteraction( _
                                    ByVal role As Role, _
                                    ByVal action As Action, _
                                    ByVal state As State, _
                                    ByVal name As String _
                            ) As Interaction
        Dim result As New Interaction( _
                                System, _
                                Me, _
                                role, _
                                action, _
                                state _
                            )
        result.Name = name
        Return result
    End Function

    Public Overloads Function AddNewInteraction( _
                                    ByVal role As Role, _
                                    ByVal action As Action, _
                                    ByVal state As State, _
                                    ByVal name As String _
                            ) As Interaction

        Dim result As Interaction = NewInteraction( _
                                role, _
                                action, _
                                state, _
                                name _
                            )
        m_interactions.Add(result)
        Return result
    End Function

    Public Overrides Function EqualsTo(ByVal other As Element) As Boolean
        Dim result As Boolean = MyBase.EqualsTo(other)
        Dim otherObject As ModelObject = DirectCast(other, ModelObject)

        If result Then
            ' If the number of interactions in the two objects is different
            ' the two objects are different
            If m_interactions.Count <> otherObject.Interactions.Count Then
                result = False
            Else
                For Each i As Interaction In m_interactions
                    With otherObject.Interactions
                        If .Contains(i.Key) Then
                            result = i.EqualsTo(.Item(i.Key))
                        Else
                            result = False
                        End If
                    End With
                    If Not result Then Exit For
                Next
            End If
        End If

        Return result
    End Function

    Protected Friend Sub New(ByVal system As ModelSystem)
        MyBase.New(system)

        m_interactions = New ElementCollection(Of Interaction)(system)
    End Sub

    Public Sub New()
        MyBase.New()

        m_interactions = New ElementCollection(Of Interaction)(Nothing)
    End Sub
End Class

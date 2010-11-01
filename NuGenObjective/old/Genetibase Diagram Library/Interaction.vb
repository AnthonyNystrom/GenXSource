Option Strict On
Option Explicit On

Imports System.ComponentModel

''' <summary>
''' An Interaction represents an operation that can be performed on
''' a ModelObject. It can be invoked by a Role, involves an Action,
''' and causes the ModelObject to transition to a State.
''' </summary>
''' <remarks></remarks>
Public Class Interaction
    Inherits Element

    Private m_object As ModelObject
    Private m_roleKey As String
    Private WithEvents m_role As Role
    Private m_actionKey As String
    Private WithEvents m_action As Action
    Private m_stateKey As String
    Private WithEvents m_state As State

    Private Sub RoleActionStateDeleted(ByVal sender As Object, ByVal e As EventArgs) Handles m_role.Deleted, m_action.Deleted, m_state.Deleted
        If m_object IsNot Nothing Then
            ' call delete
            m_object.InteractionTriggerDelete(Key)
        End If
    End Sub

    Friend Sub ObjectTriggerRename(ByVal oldKey As String)
        OnRename(oldKey)
    End Sub

    Protected Overrides Sub OnSetSystem(ByVal system As ModelSystem)
        MyBase.OnSetSystem(system)
        If system IsNot Nothing Then
            If m_role Is Nothing AndAlso m_roleKey <> "" Then
                m_role = system.Roles(m_roleKey)
            End If
            If m_action Is Nothing AndAlso m_actionKey <> "" Then
                m_action = system.Actions(m_actionKey)
            End If
            If m_state Is Nothing AndAlso m_stateKey <> "" Then
                m_state = system.States(m_stateKey)
            End If
        End If
    End Sub

    Protected Overrides Sub OnSave(ByVal writer As System.Xml.XmlWriter)
        Dim roleRef As String
        Dim actionRef As String
        Dim stateRef As String
        If m_role Is Nothing Then roleRef = m_roleKey Else roleRef = m_role.Key
        If m_action Is Nothing Then actionRef = m_actionKey Else actionRef = m_action.Key
        If m_state Is Nothing Then stateRef = m_stateKey Else stateRef = m_state.Key
        writer.WriteElementString("RoleRef", Diagram.RootNameSpace, roleRef)
        writer.WriteElementString("ActionRef", Diagram.RootNameSpace, actionRef)
        writer.WriteElementString("StateRef", Diagram.RootNameSpace, stateRef)
    End Sub

    Protected Overrides Sub OnOpen(ByVal reader As System.Xml.XmlReader)
        m_roleKey = reader.ReadElementContentAsString("RoleRef", Diagram.RootNameSpace)
        If System IsNot Nothing Then
            m_role = System.Roles(m_roleKey)
        End If

        m_actionKey = reader.ReadElementContentAsString("ActionRef", Diagram.RootNameSpace)
        If System IsNot Nothing Then
            m_action = m_object.System.Actions(m_actionKey)
        End If

        m_stateKey = reader.ReadElementContentAsString("StateRef", Diagram.RootNameSpace)
        If System IsNot Nothing Then
            m_state = m_object.System.States(m_stateKey)
        End If
    End Sub

    Public Overrides Function EqualsTo(ByVal other As Element) As Boolean
        Dim result As Boolean = MyBase.EqualsTo(other)
        Dim otherInteraction As Interaction = DirectCast(other, Interaction)
        If result Then result = m_role.EqualsTo(otherInteraction.Role)
        If result Then result = m_action.EqualsTo(otherInteraction.Action)
        If result Then result = m_state.EqualsTo(otherInteraction.State)
        Return result
    End Function

    ''' <summary>
    ''' The ModelObject that this Interaction is associated with.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A ModelObject.</returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property [Object]() As ModelObject
        Get
            Return m_object
        End Get
    End Property

    Friend Sub SetObject(ByVal objectToSet As ModelObject)
        m_object = objectToSet
    End Sub

    ''' <summary>
    ''' The Role that invokes this Interaction.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A Role.</returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property Role() As Role
        Get
            Return m_role
        End Get
    End Property

    ''' <summary>
    ''' The Action that is caused as a result of this Interaction.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property Action() As Action
        Get
            Return m_action
        End Get
    End Property

    ''' <summary>
    ''' The State that this Interaction's ModelObject transitions 
    ''' to as a result of this Interaction.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property State() As State
        Get
            Return m_state
        End Get
    End Property

    Public NotOverridable Overrides ReadOnly Property Key() As String
        Get
            Return m_object.Key & "_i_" & MyBase.Name
        End Get
    End Property

    Protected Friend Sub New( _
        ByVal system As ModelSystem, _
        ByVal modelObject As ModelObject, _
        ByVal role As Role, _
        ByVal action As Action, _
        ByVal state As State _
    )
        MyBase.New(system)

        m_object = modelObject
        m_role = role
        m_action = action
        m_state = state
    End Sub
End Class

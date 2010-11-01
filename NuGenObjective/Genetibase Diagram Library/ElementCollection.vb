Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel

''' <summary>
''' The ElementCollection(Of T) genetic class represents collections of elements.
''' It can be used like an indexed list or a keyed dictionary.
''' </summary>
''' <typeparam name="T">This can be any Element-derived class</typeparam>
''' <remarks></remarks>
Public NotInheritable Class ElementCollection(Of T As Element)
    Inherits KeyedCollection(Of String, T)

    Private m_system As ModelSystem

    Public Event Added As EventHandler(Of ElementCollectionEventArgs(Of T))
    Public Event Removed As EventHandler(Of ElementCollectionEventArgs(Of T))
    Public Event Changed As EventHandler(Of ElementCollectionEventArgs(Of T))
    Public Event ElementRenamed As EventHandler(Of ElementRenamedEventArgs)
    Public Event ElementChanged As EventHandler

    Private Sub ElementRenamedHandler(ByVal sender As Object, ByVal e As ElementRenamedEventArgs)
        RaiseEvent ElementRenamed(sender, e)
    End Sub

    Private Sub ElementChangedHandler(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent ElementChanged(sender, e)
    End Sub

    Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As T)
        item.SetSystem(m_system)
        MyBase.InsertItem(index, item)
        AddHandler item.Renamed, AddressOf ElementRenamedHandler
        AddHandler item.Changed, AddressOf ElementChangedHandler
        Dim eventArgs As New ElementCollectionEventArgs(Of T)(item)
        RaiseEvent Added(Me, eventArgs)
    End Sub

    Protected Overrides Sub RemoveItem(ByVal index As Integer)
        Dim removedItem As T = MyBase.Items(index)
        removedItem.SetSystem(Nothing)
        RemoveHandler removedItem.Renamed, AddressOf ElementRenamedHandler
        RemoveHandler removedItem.Changed, AddressOf ElementChangedHandler
        MyBase.RemoveItem(index)
        Dim eventArgs As New ElementCollectionEventArgs(Of T)(removedItem)
        RaiseEvent Removed(Me, eventArgs)
        removedItem.OnDeleted()
    End Sub

    Protected Overrides Sub SetItem(ByVal index As Integer, ByVal item As T)
        item.SetSystem(m_system)
        MyBase.SetItem(index, item)
        AddHandler item.Renamed, AddressOf ElementRenamedHandler
        AddHandler item.Changed, AddressOf ElementChangedHandler
        Dim eventArgs As New ElementCollectionEventArgs(Of T)(item)
        RaiseEvent Changed(Me, eventArgs)
    End Sub

    Protected Overrides Function GetKeyForItem(ByVal item As T) As String
        Return item.Key
    End Function

    Friend Sub SetSystem(ByVal system As ModelSystem)
        m_system = system
    End Sub

    Friend Sub Paste(ByVal item As T)
        Dim i As Integer = 0
        Dim originalName As String = item.Name
        Do While Contains(item.Key)
            item.Name = originalName & i.ToString(Globalization.CultureInfo.InvariantCulture)
            If i = Integer.MaxValue Then
                Throw New InvalidOperationException("Too many elements with the same name pattern.")
            Else
                i += 1
            End If
        Loop
        Add(item)
    End Sub

    Friend Sub New(ByVal modelSystem As ModelSystem)
        MyBase.New(Nothing, -1)
        m_system = modelSystem
    End Sub
End Class

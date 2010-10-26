Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Collections.ObjectModel

''' <summary>
''' An Action represents a well-known operation in a system. Performing an Action
''' causes an Object to transition to a new State.
''' </summary>
''' <remarks></remarks>
Public Class Action
    Inherits Element

    <Browsable(False)> _
    Public Overrides ReadOnly Property Key() As String
        Get
            Return "a_" & MyBase.Name
        End Get
    End Property

    Protected Overrides Sub OnSave(ByVal writer As System.Xml.XmlWriter)
        ' The default Action has no additional properties
    End Sub

    Protected Overrides Sub OnOpen(ByVal reader As System.Xml.XmlReader)
        ' The default Action has no additional properties
    End Sub

    Friend Sub New(ByVal system As ModelSystem)
        MyBase.New(system)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub
End Class

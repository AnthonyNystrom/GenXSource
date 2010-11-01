Option Strict On
Option Explicit On

Imports System.ComponentModel

''' <summary>
''' A State is just that: a state. ModelObjects transition from
''' state to state as a result of Interactions.
''' </summary>
''' <remarks></remarks>
Public Class State
    Inherits Element

    <Browsable(False)> _
    Public Overrides ReadOnly Property Key() As String
        Get
            Return "s_" & MyBase.Name
        End Get
    End Property

    Protected Overrides Sub OnSave(ByVal writer As System.Xml.XmlWriter)
        ' The default State has no additional properties
    End Sub

    Protected Overrides Sub OnOpen(ByVal reader As System.Xml.XmlReader)
        ' The default State has no additional properties
    End Sub

    Friend Sub New(ByVal system As ModelSystem)
        MyBase.New(system)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub
End Class

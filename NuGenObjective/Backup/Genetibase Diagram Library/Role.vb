Option Strict On
Option Explicit On

Imports System.ComponentModel

''' <summary>
''' A Role represents a person, group or process. A Role can 
''' invoke an Action that causes an Object to transition to
''' a new State.
''' </summary>
''' <remarks></remarks>
Public Class Role
    Inherits Element

    <Browsable(False)> _
    Public Overrides ReadOnly Property Key() As String
        Get
            Return "r_" & Name
        End Get
    End Property

    Protected Overrides Sub OnSave(ByVal writer As System.Xml.XmlWriter)
        ' The default Role has no additional properties
    End Sub

    Protected Overrides Sub OnOpen(ByVal reader As System.Xml.XmlReader)
        ' The default Role has no additional properties
    End Sub

    Friend Sub New(ByVal system As ModelSystem)
        MyBase.New(system)
    End Sub

    Friend Sub New()

    End Sub
End Class

Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel

Namespace DiffPatch
    Public MustInherit Class DiffAlgorithm(Of T)
        Private m_differences As DiffCollection(Of T)

        Public ReadOnly Property Differences() As DiffCollection(Of T)
            Get
                Return m_differences
            End Get
        End Property

        Protected Sub New()
            m_differences = New DiffCollection(Of T)
        End Sub
    End Class
End Namespace

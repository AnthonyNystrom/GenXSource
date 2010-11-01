Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel

Namespace DiffPatch
    Public Class DiffEdit(Of T)
        Private m_diffOperation As DiffType
        Private m_startOffset As Integer = 0
        Private m_changeLength As Integer = 0
        Private m_changedElements As New Collection(Of T)

        Public Property DiffOperation() As DiffType
            Get
                Return m_diffOperation
            End Get
            Set(ByVal value As DiffType)
                m_diffOperation = value
            End Set
        End Property

        Public Property StartOffset() As Integer
            Get
                Return m_startOffset
            End Get
            Set(ByVal value As Integer)
                m_startOffset = value
            End Set
        End Property

        Public Property ChangeLength() As Integer
            Get
                Return m_changeLength
            End Get
            Set(ByVal value As Integer)
                m_changeLength = value
            End Set
        End Property

        Public ReadOnly Property ChangedElements() As Collection(Of T)
            Get
                Return m_changedElements
            End Get
        End Property
    End Class
End Namespace

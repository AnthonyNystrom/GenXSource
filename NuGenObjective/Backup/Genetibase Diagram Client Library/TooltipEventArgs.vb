Option Strict On
Option Explicit On

Imports Genetibase.NuGenObjective

Public Class TooltipEventArgs
    Inherits EventArgs

    Private m_title As String
    Private m_text As String
    Private m_element As Element
    Private m_cancel As Boolean

    Public Property Title() As String
        Get
            Return m_title
        End Get
        Set(ByVal value As String)
            m_title = value
        End Set
    End Property

    Public Property Text() As String
        Get
            Return m_text
        End Get
        Set(ByVal value As String)
            m_text = value
        End Set
    End Property

    Public ReadOnly Property Element() As Element
        Get
            Return m_element
        End Get
    End Property

    Public Property Cancel() As Boolean
        Get
            Return m_cancel
        End Get
        Set(ByVal value As Boolean)
            m_cancel = value
        End Set
    End Property

    Public Sub New(ByVal tipElement As Element)
        m_element = tipElement
    End Sub
End Class

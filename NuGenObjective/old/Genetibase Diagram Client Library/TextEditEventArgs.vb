Option Strict On
Option Explicit On

Imports System.Windows.Forms

' This eventArgs class is used for most
' text editing events. It enables us to pass
' the internal text box to consumers, and receive
' a cancel notification if need be.
Public Class TextEditEventArgs
    Inherits EventArgs

    Private m_textBox As TextBox
    Private m_cancel As Boolean

    Public Property TextBox() As TextBox
        Get
            Return m_textBox
        End Get
        Set(ByVal value As TextBox)
            m_textBox = value
        End Set
    End Property

    Public Property Cancel() As Boolean
        Get
            Return m_cancel
        End Get
        Set(ByVal value As Boolean)
            m_cancel = value
        End Set
    End Property

End Class

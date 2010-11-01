Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Class DiagramMessageEventArgs
    Inherits EventArgs

    Private m_message As String
    Private m_messageButtons As MessageBoxButtons = MessageBoxButtons.OK
    Private m_messageIcon As MessageBoxIcon = MessageBoxIcon.None
    Private m_messageResult As DialogResult = DialogResult.Cancel

    Public Property Message() As String
        Get
            Return m_message
        End Get
        Set(ByVal value As String)
            m_message = value
        End Set
    End Property

    Public Property MessageButtons() As MessageBoxButtons
        Get
            Return m_messageButtons
        End Get
        Set(ByVal value As MessageBoxButtons)
            m_messageButtons = value
        End Set
    End Property

    Public Property MessageIcon() As MessageBoxIcon
        Get
            Return m_messageIcon
        End Get
        Set(ByVal value As MessageBoxIcon)
            m_messageIcon = value
        End Set
    End Property

    Public Property MessageResult() As DialogResult
        Get
            Return m_messageResult
        End Get
        Set(ByVal value As DialogResult)
            m_messageResult = value
        End Set
    End Property

End Class

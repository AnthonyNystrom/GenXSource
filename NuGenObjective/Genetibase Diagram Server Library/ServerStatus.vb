Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel

Public Class ServerStatus
    Private m_version As String
    Private m_secure As Boolean
    Private m_files As Collection(Of ServerStatusFile)

    Public Property Version() As String
        Get
            Return m_version
        End Get
        Set(ByVal value As String)
            m_version = value
        End Set
    End Property

    Public Property Secure() As Boolean
        Get
            Return m_secure
        End Get
        Set(ByVal value As Boolean)
            m_secure = value
        End Set
    End Property

    Public Property Files() As Collection(Of ServerStatusFile)
        Get
            Return m_files
        End Get
        Set(ByVal value As Collection(Of ServerStatusFile))
            m_files = value
        End Set
    End Property

    Public Sub New()
        m_files = New Collection(Of ServerStatusFile)
    End Sub
End Class

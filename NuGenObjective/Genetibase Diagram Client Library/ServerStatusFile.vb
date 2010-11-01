Option Strict On
Option Explicit On

Public Class ServerStatusFile
    Private m_filename As String
    Private m_fileURL As String

    Public Property Filename() As String
        Get
            Return m_filename
        End Get
        Set(ByVal value As String)
            m_filename = value
        End Set
    End Property

    Public Property FileURL() As String
        Get
            Return m_fileURL
        End Get
        Set(ByVal value As String)
            m_fileURL = value
        End Set
    End Property


    Public Sub New(ByVal fileName As String, ByVal fileUrl As String)
        m_filename = fileName
        m_fileURL = fileUrl
    End Sub

    Public Sub New()

    End Sub
End Class

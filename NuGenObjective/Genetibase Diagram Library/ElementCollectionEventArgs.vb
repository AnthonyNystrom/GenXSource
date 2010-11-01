Option Strict On
Option Explicit On

Public Class ElementCollectionEventArgs(Of T)
    Inherits EventArgs

    Private m_element As T

    Public ReadOnly Property Element() As T
        Get
            Return m_element
        End Get
    End Property

    Friend Sub New(ByVal elementChanged As T)
        m_element = elementChanged
    End Sub
End Class

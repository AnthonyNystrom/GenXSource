Option Strict On
Option Explicit On

Public Class PageElementsChangedEventArgs
    Inherits EventArgs

    Public Enum TypeOfChange
        Added
        Removed
        NameChanged
        Changed
    End Enum

    Private m_elements() As Element
    Private m_changeType As TypeOfChange

    Public Function GetChangedElements() As Element()
        Return m_elements
    End Function

    Public ReadOnly Property ChangeType() As TypeOfChange
        Get
            Return m_changeType
        End Get
    End Property

    Friend Sub New(ByVal elementsChanged() As Element, ByVal typeOfChange As TypeOfChange)
        m_elements = elementsChanged
        m_changeType = typeOfChange
    End Sub
End Class

Option Strict On
Option Explicit On

''' <summary>
''' The ElementRenamedEventArgs class provides the parameters for
''' the Renamed event of the Element class.
''' </summary>
''' <remarks></remarks>
Public Class ElementRenamedEventArgs
    Inherits EventArgs

    Private m_oldKey As String

    ''' <summary>
    ''' The Oldkey  property contains the old Key of the Element
    ''' that has just been renamed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property OldKey() As String
        Get
            Return m_oldKey
        End Get
    End Property

    Friend Sub New(ByVal keyBeforeRename As String)
        m_oldKey = keyBeforeRename
    End Sub
End Class

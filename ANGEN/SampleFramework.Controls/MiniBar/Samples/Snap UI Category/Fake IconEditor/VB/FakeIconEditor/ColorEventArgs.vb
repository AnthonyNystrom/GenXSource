Friend NotInheritable Class ColorEventArgs
    Inherits EventArgs
    Private _color As Color

    Public Property Color() As Color
        Get
            Return _color
        End Get
        Set(ByVal value As Color)
            _color = value
        End Set
    End Property

    Sub New(ByVal color As Color)
        _color = color
    End Sub
End Class

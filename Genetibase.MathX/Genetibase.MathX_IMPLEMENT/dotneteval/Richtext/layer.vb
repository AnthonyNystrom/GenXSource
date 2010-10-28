Public Class layer
    Inherits div

    Friend Overrides Sub SetRectangle(ByVal g As System.Drawing.Graphics, ByVal rectangle As System.Drawing.Rectangle)
        MyBase.SetRectangle(g, rectangle)
    End Sub

    Sub New(ByVal richText As RichText)
        MyBase.New(richText)
    End Sub

End Class

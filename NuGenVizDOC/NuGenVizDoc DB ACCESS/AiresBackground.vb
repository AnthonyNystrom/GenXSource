Imports System.Drawing.Drawing2D

#Region " AiresBackground "

Public Class AiresBackground
    Inherits System.Windows.Forms.Panel

#Region " Variables "

    Public Enum AiresBackgroundTypes As Integer
        AllBorders = 0
        NoBorder = 1
    End Enum

    Private m_BackType As AiresBackgroundTypes = AiresBackgroundTypes.AllBorders

#End Region

#Region " Properties "

    Public Property BackgroundType() As AiresBackgroundTypes
        Get
            Return m_BackType
        End Get
        Set(ByVal value As AiresBackgroundTypes)
            m_BackType = value
            Me.Refresh()
        End Set
    End Property

#End Region

#Region " Component Code "

    Private Sub AiresBackground_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        e.Graphics.FillRectangle(Brushes.White, 0, 0, Me.Width, Me.Height)
        If m_BackType = AiresBackgroundTypes.AllBorders Then
            e.Graphics.DrawRectangle(New Pen(Color.FromArgb(150, Color.Black)), 0, 0, Me.Width - 1, Me.Height - 1)
        End If
    End Sub

#End Region

End Class

#End Region

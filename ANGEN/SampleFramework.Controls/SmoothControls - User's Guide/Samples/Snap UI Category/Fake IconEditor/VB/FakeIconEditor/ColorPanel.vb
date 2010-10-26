Imports System.ComponentModel

<System.ComponentModel.DesignerCategory("Code")> _
Friend NotInheritable Class ColorPanel
    <DefaultValue(GetType(Color), "Window")> _
    Public Overrides Property BackColor() As System.Drawing.Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            MyBase.BackColor = value
        End Set
    End Property

    Private _selectedColor As Color

    Public Property SelectedColor() As Color
        Get
            If (Me._selectedColor = Color.Empty) Then
                Return Color.Transparent
            End If
            Return Me._selectedColor
        End Get
        Set(ByVal value As Color)
            If (Me._selectedColor <> value) Then
                Me._selectedColor = value
                MyBase.Invalidate()
            End If
        End Set
    End Property

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        If Me.Width > 0 AndAlso Me.Height > 0 Then
            Using sb As New SolidBrush(Me.SelectedColor)
                e.Graphics.FillRectangle(sb, Me.ClientRectangle)
            End Using
        End If
    End Sub

    Sub New()
        Me.InitializeComponent()

        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.UserPaint, True)

        Me.BackColor = SystemColors.Window
    End Sub
End Class

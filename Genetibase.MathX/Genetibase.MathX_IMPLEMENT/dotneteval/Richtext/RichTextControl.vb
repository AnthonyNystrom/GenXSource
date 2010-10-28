Public Class RichTextControl
    Inherits System.Windows.Forms.UserControl

    Public Sub New()
        MyBase.New()
        mRichText = New RichText
        Me.SetStyle(Windows.Forms.ControlStyles.ResizeRedraw, True)
        InitializeComponent()
    End Sub

    Private mXML As String
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Private components As System.ComponentModel.IContainer
    Private WithEvents mRichText As RichText

    Public Property XML() As String
        Get
            Return mXML
        End Get
        Set(ByVal Value As String)
            If Not mXML = Value Then
                mXML = Value
                mRichText.XML = Value
            End If
        End Set
    End Property

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        mRichText.Paint(e.Graphics, New System.Drawing.Rectangle(0, 0, Me.Width, Me.Height), e.ClipRectangle)
    End Sub

    Private Sub mRichText_RepaintNeeded() Handles mRichText.RepaintNeeded
        Me.Invalidate()
    End Sub



    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 5000
        '
        'RichTextControl
        '
        Me.Name = "RichTextControl"

    End Sub

    Private mPreviousToolTip As String

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        mx = e.X
        my = e.Y
        Dim newTooltip As String
        Dim elt As visualElement = mRichText.GetElementAt(New Drawing.Point(e.X, e.Y))
        If elt Is Nothing Then
            newTooltip = ""
        Else
            newTooltip = elt.Tooltip
        End If
        ' If Not newTooltip = mPreviousToolTip Then
        ToolTip1.SetToolTip(Me, newTooltip)
        mPreviousToolTip = newTooltip
        ' End If
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim pt As Drawing.Point = Windows.Forms.Control.MousePosition()
        pt = Me.PointToClient(pt)
        If pt.X = mx And pt.Y = my Then
            ToolTip1.SetToolTip(Me, Now.ToString)
        End If
    End Sub

    Private mx, my As Integer

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)
        ToolTip1.ShowAlways = False
        ToolTip1.InitialDelay = 1000
    End Sub
End Class

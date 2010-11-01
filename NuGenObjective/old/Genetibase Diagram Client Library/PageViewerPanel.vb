Option Strict On
Option Explicit On

Imports Genetibase.NuGenObjective
Imports Genetibase.NuGenObjective.Drawing
Imports System
Imports System.Drawing
Imports System.Drawing.Design
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles

Public Class PageViewerPanel
    Inherits Panel

#Region "Tooltip"
    Private Class ToolTipWindow
        Inherits Form

        Private m_title As String
        Private m_text As String
        Private m_titleRectangle As String
        Private m_textRectangle As String

        Protected Overrides ReadOnly Property ShowWithoutActivation() As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            Dim shadowRect As Rectangle = DisplayRectangle
            shadowRect.Offset(5, 3)
            ' shadowRect.Size -= New Size(2, 2)

            Dim tiprect As Rectangle = DisplayRectangle
            tiprect.Size -= New Size(3, 3)

            With e.Graphics
                .SmoothingMode = SmoothingMode.AntiAlias

                ' Draw shadow
                Using brush As New SolidBrush(Color.FromArgb(48, 0, 0, 0))
                    .FillRectangle( _
                        brush, _
                        New Rectangle( _
                                        shadowRect.X + 1, _
                                        shadowRect.Y + 1, _
                                        shadowRect.Width, _
                                        shadowRect.Height _
                                    ) _
                    )
                End Using


                Using brush As New SolidBrush(Color.FromArgb(32, 0, 0, 0))
                    .FillRectangle( _
                        brush, _
                        New Rectangle( _
                                        shadowRect.X + 2, _
                                        shadowRect.Y + 2, _
                                        shadowRect.Width, _
                                        shadowRect.Height _
                                    ) _
                    )
                End Using


                Using brush As New SolidBrush(Color.FromArgb(16, 0, 0, 0))
                    .FillRectangle( _
                        brush, _
                        New Rectangle( _
                                        shadowRect.X + 3, _
                                        shadowRect.Y + 3, _
                                        shadowRect.Width, _
                                        shadowRect.Height _
                                    ) _
                    )
                End Using

                ' Draw background
                Dim gBr As New LinearGradientBrush(tiprect, Color.FromArgb(227, 239, 255), Color.FromArgb(121, 161, 220), LinearGradientMode.ForwardDiagonal)
                'Dim gBr As New LinearGradientBrush(tiprect, Color.FromArgb(220235254), Color.FromArgb(129169226), LinearGradientMode.Horizontal)
                .FillRectangle(gBr, tiprect)
                ' Draw title
                Dim titleFont As Font = New Font(Font, FontStyle.Bold)
                Dim titleSize As SizeF = .MeasureString(m_title, titleFont, New PointF(2, 2), StringFormat.GenericTypographic)
                Dim titleRectangle As New Rectangle(New Point(2, 2), titleSize.ToSize)
                .DrawString(m_title, titleFont, Brushes.Chocolate, 2, 2)
                ' Draw text
                Dim textRectangle As New Rectangle(2, CInt(titleSize.Height) + 4, DisplayRectangle.Width - 4, CInt(DisplayRectangle.Height - (titleSize.Height + 6)))
                .DrawString(m_text, Font, Brushes.Brown, textRectangle)
            End With
        End Sub

        Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
            Close()
        End Sub

        Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
            Close()
        End Sub

        Sub New(ByVal startLocation As Point, ByVal startSize As Size, ByVal title As String, ByVal text As String)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            ShowInTaskbar = False
            StartPosition = FormStartPosition.Manual
            Location = startLocation
            Size = startSize
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            TransparencyKey = BackColor

            m_text = text
            m_title = title
        End Sub
    End Class

    Public Event ToolTip As EventHandler(Of TooltipEventArgs)
#End Region

    Private m_originOffsetX As Single = 0
    Private m_originOffsetY As Single = 0

    Private m_scaleFactor As Single = 1.0!

    Private WithEvents m_page As Page
    Private m_drawingPage As DrawingPage

    Private diagramDivisionsPen As New Pen(Color.SlateGray, 1.5F)
    Private stateLinePen As New Pen(Color.SlateGray, 3.0!)
    Private stateLineSelectedPen As New Pen(Color.Bisque, 3.0!)
    Private objectLinePen As New Pen(Color.FromArgb(64, Color.SlateGray), 5.0F)
    Private objectLineSelectedPen As New Pen(Color.Bisque, 5.0F)

    Private m_selectedElement As Element

    Private m_bottomRight As PointF

    Private m_dragX As Integer
    Private m_dragY As Integer
    Private m_dragBoxFromMouseDown As Rectangle
    Private m_dragging As Boolean = False

    ' Private m_tooltip As ToolTip
    ' Private m_hoverTime As Date
    ' Private Const HOVERDURATION As Integer = 500

    Public Event SelectionChanged As EventHandler
    Public Event ElementsChanged As EventHandler(Of PageElementsChangedEventArgs)

#Region " Scroll Buttons "
    Private Sub InitializeScrollButtons()
        ' Scroll buttons
        ' The buttons are 16x16 pixels, and we want to
        ' leave a 4-pixel margin
        With lblUp
            .Left = ClientRectangle.Right - 16
            .Top = ClientRectangle.Top + 4
            .Anchor = AnchorStyles.Right Or AnchorStyles.Top
            .Visible = False
        End With
        With lblDown
            .Left = ClientRectangle.Right - 16
            .Top = ClientRectangle.Bottom - 32
            .Anchor = AnchorStyles.Right Or AnchorStyles.Bottom
            .Visible = False
        End With
        With lblLeft
            .Left = ClientRectangle.Left + 4
            .Top = ClientRectangle.Bottom - 18
            .Anchor = AnchorStyles.Left Or AnchorStyles.Bottom
            .Visible = False
        End With
        With lblRight
            .Left = ClientRectangle.Right - 32
            .Top = ClientRectangle.Bottom - 18
            .Anchor = AnchorStyles.Right Or AnchorStyles.Bottom
            .Visible = False
        End With

        With Controls
            .Add(lblUp)
            .Add(lblDown)
            .Add(lblLeft)
            .Add(lblRight)
        End With
    End Sub

    Private Sub ManageScrollButtons()
        Dim bottomRight As PointF

        ' Calculate the bottom right point
        With DrawingHelper.PageSize(m_page)
            bottomRight = New Point(.Width, .Height)
        End With
        With bottomRight
            .X += m_originOffsetX
            .Y += m_originOffsetY
            .X *= m_scaleFactor
            .Y *= m_scaleFactor
        End With

        If bottomRight.Y > ClientRectangle.Bottom Then
            lblDown.Visible = True
        Else
            lblDown.Visible = False
        End If

        If m_originOffsetY < 0 Then
            lblUp.Visible = True
        Else
            lblUp.Visible = False
        End If

        If bottomRight.X > ClientRectangle.Right Then
            lblRight.Visible = True
        Else
            lblRight.Visible = False
        End If

        If m_originOffsetX < 0 Then
            lblLeft.Visible = True
        Else
            lblLeft.Visible = False
        End If

    End Sub

    Private Sub lblUp_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblUp.MouseDown
        If m_originOffsetY < 0 Then
            m_originOffsetY += 16
        Else
            m_originOffsetY = 0
        End If
        Invalidate()
    End Sub

    Private Sub lblDown_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblDown.MouseDown
        m_originOffsetY -= 16
        Invalidate()
    End Sub

    Private Sub lblLeft_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblLeft.MouseDown
        If m_originOffsetX < 0 Then
            m_originOffsetX += 16
        Else
            m_originOffsetX = 0
        End If
        Invalidate()
    End Sub

    Private Sub lblRight_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblRight.MouseDown
        m_originOffsetX -= 16
        Invalidate()
    End Sub
#End Region

#Region "Text Editing"

    ' Events added for Text Editing
    ' Raised when text editing is starting. Can be cancelled.
    Public Event TextEditStarting As EventHandler(Of TextEditEventArgs)

    ' Raised when ENTER key has been pressed in text editor. Can be cancelled.
    Public Event TextEditComplete As EventHandler(Of TextEditEventArgs)

    ' Raised when ESCAPE key has been pressed in text editor. Cannot be cancelled.
    Public Event TextEditCancelled As EventHandler(Of TextEditEventArgs)

    ' Some status fields
    ' This will be true if normal text edit processing is done.
    Private m_textEditHandled As Boolean = False
    ' This will hold the scale fator at the time of starting the 
    ' edit.
    Private m_oldScaleFactor As Single = 1.0F
    ' These will hold the old co-ordinates
    Private m_oldOriginOffsetX, m_oldOriginOffsetY As Single

    ' This method will start a text edit operation on the selected
    ' element
    Public Sub StartEdit()

        ' If there is an edit happenning,
        ' cancel and stop it
        If (txtEdit.Visible) Then
            CancelEdit()
            StopEdit()
        End If

        ' if there is a selected element
        If m_selectedElement IsNot Nothing Then
            Dim elementRectangle As Rectangle = m_drawingPage.GetRectangleForElement(m_selectedElement)
            ' Fire the TextEditStarting event here
            Dim e As New TextEditEventArgs
            e.TextBox = txtEdit
            e.Cancel = False

            RaiseEvent TextEditStarting(Me, e)

            ' If the TextEditStarting event has not been cancelled
            If Not e.Cancel Then

                ' If the scale is not 100%, temprarily set it to 
                ' 100%
                If (m_scaleFactor <> 1.0!) Then

                    m_oldScaleFactor = m_scaleFactor
                    m_scaleFactor = 1.0!

                    ' Also, move the current element to the center
                    m_oldOriginOffsetX = m_originOffsetX
                    m_oldOriginOffsetY = m_originOffsetY

                    m_originOffsetX = -(elementRectangle.X - 16)
                    m_originOffsetY = -(elementRectangle.Y - 16)

                    Invalidate()
                    Application.DoEvents()

                End If

                ' Place the editing text box as follows:
                ' Position it 16 pixels to the left of the left
                ' corner of the shape (with the x offset),
                ' and at the top of the shape (with the y
                ' offset), and set the width to the width of 
                ' the shape + 32 pixels
                txtEdit.Left = CInt(elementRectangle.X - 16 + m_originOffsetX)
                txtEdit.Top = CInt((elementRectangle.Height - txtEdit.Height) / 2) + elementRectangle.Top + CInt(m_originOffsetY)
                txtEdit.Width = elementRectangle.Width + 32

                ' Finally, make the editing text box visible and
                ' set the keyboard focus.
                txtEdit.Visible = True
                txtEdit.Focus()
                m_textEditHandled = False
            End If
        End If
    End Sub

    ' This will complete the text edit, and
    ' attempt to commit any changes.
    Public Sub CompleteEdit()

        Dim eArg As New TextEditEventArgs
        eArg.TextBox = txtEdit
        eArg.Cancel = False

        RaiseEvent TextEditComplete(Me, eArg)

        ' If the finalization is not cancelled,
        ' finish the edit.
        If Not eArg.Cancel Then

            m_textEditHandled = True
            StopEdit()
        End If
    End Sub


    Public Sub CancelEdit()

        Dim eArg As New TextEditEventArgs
        eArg.TextBox = txtEdit
        eArg.Cancel = False

        RaiseEvent TextEditCancelled(Me, eArg)

        m_textEditHandled = True
    End Sub


    Public Sub StopEdit()
        txtEdit.Text = ""
        txtEdit.Visible = False

        ' If the scale factor had to be changed
        ' for text editing, reset it
        If m_oldScaleFactor <> 1.0! Then
            m_scaleFactor = m_oldScaleFactor
            m_oldScaleFactor = 1.0!
            m_originOffsetX = m_oldOriginOffsetX
            m_originOffsetY = m_oldOriginOffsetY
        End If

        Invalidate()
    End Sub

    Private Sub txtEdit_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtEdit.KeyPress

        ' If the ENTER key is pressed, attempt to finalize.
        ' If ESCAPE is pressed, cancel.
        ' For any other key, do normal text editing.
        Select Case (e.KeyChar)
            Case ChrW(Keys.Enter)
                e.Handled = True
                CompleteEdit()
            Case ChrW(Keys.Escape)
                e.Handled = True
                CancelEdit()
                StopEdit()
            Case Else
                e.Handled = False
        End Select
    End Sub

    Private Sub txtEdit_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtEdit.Leave
        ' If the focus moves from the text editor 
        ' and the normal processing has not been 
        ' done, treat it as cancelled.
        If Not m_textEditHandled Then
            ' If you want to save text editing
            ' changes instead of cancelling, change
            ' the next line to
            ' CompleteEdit()
            CancelEdit()
            StopEdit()
        Else
            m_textEditHandled = False
        End If
    End Sub

#End Region

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        ' Manage selection
        Dim newSelectedElement As Element = m_drawingPage.GetElementAtPosition(e.X, e.Y, m_originOffsetX, m_originOffsetY, m_scaleFactor)

        If newSelectedElement IsNot m_selectedElement Then
            m_selectedElement = newSelectedElement
            Invalidate()
            RaiseEvent SelectionChanged(Me, EventArgs.Empty)
        End If

        ' Manage drag start
        ' If the Page was selected
        If m_selectedElement Is m_page Then
            ' Save current co-ordinates
            m_dragX = CInt(e.X - m_originOffsetX)
            m_dragY = CInt(e.Y - m_originOffsetY)

            ' Remember the point where the mouse down occurred. The DragSize indicates
            ' the size that the mouse can move before a drag event should be started.                
            Dim dragSize As Size = SystemInformation.DragSize

            ' Create a rectangle using the DragSize, with the mouse position being
            ' at the center of the rectangle.
            m_dragBoxFromMouseDown = New Rectangle(New Point(e.X - CInt(dragSize.Width / 2), _
                                                            e.Y - CInt(dragSize.Height / 2)), dragSize)
            'm_dragging = True
        Else
            ' Reset the rectangle if the mouse is over an element.
            m_dragBoxFromMouseDown = Rectangle.Empty
            m_dragX = 0
            m_dragY = 0
            m_dragging = False
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        If (e.Button And System.Windows.Forms.MouseButtons.Left) = System.Windows.Forms.MouseButtons.Left Then
            ' If the mouse has left the dragging rectangle then
            If m_dragBoxFromMouseDown <> Rectangle.Empty _
                    AndAlso _
                Not m_dragBoxFromMouseDown.Contains(e.X, e.Y) Then
                m_dragging = True
                Cursor = Cursors.NoMove2D
            End If

            ' If dragging is currently on
            If m_dragging Then
                m_originOffsetX = e.X - m_dragX
                m_originOffsetY = e.Y - m_dragY

                Invalidate()
            End If
        End If

        MyBase.OnMouseMove(e)
    End Sub

    'Protected Overrides Sub OnMouseHover(ByVal e As System.EventArgs)
    '    Dim hoverPosition As Point = PointToClient(MousePosition)
    '    If m_hoverTime = DateTime.MinValue Then
    '        m_hoverTime = Now
    '    Else
    '        If Now.Subtract(m_hoverTime).Milliseconds >= HOVERDURATION Then
    '            m_hoverTime = DateTime.MinValue
    '            m_tooltip.Hide(Me)
    '            m_tooltip.RemoveAll()
    '        End If
    '    End If

    '    If m_hoverTime <> DateTime.MinValue Then
    '        Dim he As Element = m_drawingPage.GetElementAtPosition(hoverPosition.X, hoverPosition.Y, m_originOffsetX, m_originOffsetY, m_scaleFactor)
    '        If he IsNot Nothing Then
    '            Dim te As New TooltipEventArgs(he)
    '            RaiseEvent ToolTip(Me, te)
    '            If Not te.Cancel Then
    '                Dim f As New ToolTipWindow(Point.Subtract(MousePosition, New Size(10, 10)), New Size(300, 100), te.Title, te.Text)
    '                f.Show(Me)
    '            End If
    '        End If
    '        m_hoverTime = DateTime.MinValue
    '    End If
    '    MyBase.OnMouseHover(e)
    'End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        If m_dragging Then
            m_dragX = e.X
            m_dragY = e.Y
            m_dragBoxFromMouseDown = Rectangle.Empty
            m_dragging = False
            Cursor = Cursors.Arrow
        End If

        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnMouseWheel(ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim numberOfTextLinesToMove As Integer = CInt(e.Delta * SystemInformation.MouseWheelScrollLines / 120) ' WHEEL_DATA
        Dim numberOfPixelsToMove As Integer = numberOfTextLinesToMove * 64

        If numberOfPixelsToMove <> 0 Then
            m_originOffsetY += numberOfPixelsToMove
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnDoubleClick(ByVal e As System.EventArgs)
        MyBase.OnDoubleClick(e)
        Dim hoverPosition As Point = PointToClient(MousePosition)

        Dim he As Element = m_drawingPage.GetElementAtPosition(hoverPosition.X, hoverPosition.Y, m_originOffsetX, m_originOffsetY, m_scaleFactor)
        If he IsNot Nothing Then
            Dim te As New TooltipEventArgs(he)
            RaiseEvent ToolTip(Me, te)
            If Not te.Cancel Then
                Dim f As New ToolTipWindow(Point.Subtract(MousePosition, New Size(10, 10)), New Size(300, 100), te.Title, te.Text)
                f.Show(Me)
            End If
        End If
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        If m_drawingPage IsNot Nothing Then
            m_drawingPage.Draw( _
                        m_selectedElement, _
                        e.Graphics, _
                        m_originOffsetX, _
                        m_originOffsetY, _
                        m_scaleFactor, _
                        diagramDivisionsPen, _
                        stateLinePen, _
                        stateLineSelectedPen, _
                        objectLinePen, _
                        objectLineSelectedPen _
                )
            ManageScrollButtons()
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal eventargs As System.EventArgs)
        MyBase.OnResize(eventargs)
        ManageScrollButtons()
    End Sub

    Private Sub m_page_ElementChanged(ByVal sender As Object, ByVal e As PageElementsChangedEventArgs) Handles m_page.PageElementsChanged
        RaiseEvent ElementsChanged(sender, e)
        Invalidate()
    End Sub

    <Browsable(False)> _
    Public Property Page() As Page
        Get
            Return m_page
        End Get
        Set(ByVal value As Page)
            If Not value Is m_page Then
                m_page = value
                m_drawingPage = New DrawingPage(m_page)
                If m_page IsNot Nothing Then
                    m_selectedElement = m_page
                Else
                    m_selectedElement = Nothing
                End If
                RaiseEvent SelectionChanged(Me, EventArgs.Empty)
                Invalidate()
            End If
        End Set
    End Property

    <Browsable(False)> _
    Public Property SelectedElement() As Element
        Get
            Return m_selectedElement
        End Get
        Set(ByVal value As Element)
            m_selectedElement = value
            Invalidate()
            RaiseEvent SelectionChanged(Me, EventArgs.Empty)
        End Set
    End Property

    Public Sub SetScale(ByVal scaleFactor As Single)
        m_scaleFactor = scaleFactor
        m_originOffsetX = 0
        m_originOffsetY = 0
        Invalidate()
    End Sub

    Public Sub Cut()
        If SelectedElement IsNot Nothing _
            AndAlso _
        (Not TypeOf SelectedElement Is Page) Then

            Dim nugenObjectiveCopyFormat As DataFormats.Format = _
                DataFormats.GetFormat("NuGenObjectiveExchange")

            Clipboard.SetData( _
                nugenObjectiveCopyFormat.Name, _
                SelectedElement.System.CutElementToXML(SelectedElement) _
            )
        End If
    End Sub

    Public Sub Copy()
        If SelectedElement IsNot Nothing _
            AndAlso _
        (Not TypeOf SelectedElement Is Page) Then

            Dim nugenObjectiveCopyFormat As DataFormats.Format = _
                DataFormats.GetFormat("NuGenObjectiveExchange")

            Clipboard.SetData( _
                nugenObjectiveCopyFormat.Name, _
                SelectedElement.System.CopyElementToXML(SelectedElement) _
            )
        End If
    End Sub

    Public Sub Paste()
        Dim nugenObjectiveCopyFormat As DataFormats.Format = _
            DataFormats.GetFormat("NuGenObjectiveExchange")

        If Clipboard.ContainsData(nugenObjectiveCopyFormat.Name) Then

            Dim elementText As String = DirectCast(Clipboard.GetData(nugenObjectiveCopyFormat.Name), String)

            If elementText IsNot Nothing AndAlso elementText.Length > 0 Then
                Dim element As Element = Page.ParentDiagram.System.PasteElementFromXML(elementText)
                Page.AddExistingElement(element)
            End If
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)

        diagramDivisionsPen.DashStyle = DashStyle.Dot
        objectLinePen.EndCap = LineCap.ArrowAnchor
        objectLineSelectedPen.EndCap = LineCap.ArrowAnchor

        InitializeScrollButtons()

        'm_tooltip = New ToolTip
    End Sub
End Class
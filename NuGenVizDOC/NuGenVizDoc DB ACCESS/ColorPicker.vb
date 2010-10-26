Public Class ColorPicker
    Inherits System.Windows.Forms.UserControl

    Private m_CustomColors(48) As Color
    Private m_CustomColorsPos(48) As Rectangle

    Public SelectedColor As Color = Color.Empty

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        InitCustomColors()
        InitOtherColors()
    End Sub

    'UserControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As system.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents tabControl1 As system.Windows.Forms.TabControl
    Friend WithEvents customTab As System.Windows.Forms.TabPage
    Friend WithEvents webTab As system.Windows.Forms.TabPage
    Friend WithEvents systemTab As system.Windows.Forms.TabPage
    Friend WithEvents listWeb As system.Windows.Forms.ListBox
    Friend WithEvents listSystem As system.Windows.Forms.ListBox
    Friend WithEvents colorPanel As System.Windows.Forms.Panel
    <system.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tabControl1 = New System.Windows.Forms.TabControl()
        Me.customTab = New System.Windows.Forms.TabPage()
        Me.webTab = New System.Windows.Forms.TabPage()
        Me.listWeb = New System.Windows.Forms.ListBox()
        Me.systemTab = New System.Windows.Forms.TabPage()
        Me.listSystem = New System.Windows.Forms.ListBox()
        Me.colorPanel = New System.Windows.Forms.Panel()
        Me.tabControl1.SuspendLayout()
        Me.customTab.SuspendLayout()
        Me.webTab.SuspendLayout()
        Me.systemTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabControl1
        '
        Me.tabControl1.Controls.AddRange(New System.Windows.Forms.Control() {Me.customTab, Me.webTab, Me.systemTab})
        Me.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(207, 182)
        Me.tabControl1.TabIndex = 0
        '
        'customTab
        '
        Me.customTab.Controls.AddRange(New System.Windows.Forms.Control() {Me.colorPanel})
        Me.customTab.Location = New System.Drawing.Point(4, 22)
        Me.customTab.Name = "customTab"
        Me.customTab.Size = New System.Drawing.Size(199, 156)
        Me.customTab.TabIndex = 0
        Me.customTab.Text = "Custom"
        '
        'webTab
        '
        Me.webTab.Controls.AddRange(New System.Windows.Forms.Control() {Me.listWeb})
        Me.webTab.Location = New System.Drawing.Point(4, 22)
        Me.webTab.Name = "webTab"
        Me.webTab.Size = New System.Drawing.Size(199, 156)
        Me.webTab.TabIndex = 1
        Me.webTab.Text = "Web"
        '
        'listWeb
        '
        Me.listWeb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.listWeb.IntegralHeight = False
        Me.listWeb.Location = New System.Drawing.Point(5, 4)
        Me.listWeb.Name = "listWeb"
        Me.listWeb.Size = New System.Drawing.Size(190, 147)
        Me.listWeb.TabIndex = 0
        '
        'systemTab
        '
        Me.systemTab.Controls.AddRange(New System.Windows.Forms.Control() {Me.listSystem})
        Me.systemTab.Location = New System.Drawing.Point(4, 22)
        Me.systemTab.Name = "systemTab"
        Me.systemTab.Size = New System.Drawing.Size(199, 156)
        Me.systemTab.TabIndex = 2
        Me.systemTab.Text = "System"
        '
        'listSystem
        '
        Me.listSystem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.listSystem.IntegralHeight = False
        Me.listSystem.Location = New System.Drawing.Point(4, 5)
        Me.listSystem.Name = "listSystem"
        Me.listSystem.Size = New System.Drawing.Size(190, 147)
        Me.listSystem.TabIndex = 1
        '
        'colorPanel
        '
        Me.colorPanel.Anchor = (((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right)
        Me.colorPanel.Location = New System.Drawing.Point(4, 5)
        Me.colorPanel.Name = "colorPanel"
        Me.colorPanel.Size = New System.Drawing.Size(190, 146)
        Me.colorPanel.TabIndex = 1
        '
        'ColorPicker
        '
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.tabControl1})
        Me.Name = "ColorPicker"
        Me.Size = New System.Drawing.Size(207, 182)
        Me.tabControl1.ResumeLayout(False)
        Me.customTab.ResumeLayout(False)
        Me.webTab.ResumeLayout(False)
        Me.systemTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub InitCustomColors()
        m_CustomColors(0) = Color.FromArgb(255, 255, 255)
        m_CustomColors(1) = Color.FromArgb(255, 195, 198)
        m_CustomColors(2) = Color.FromArgb(255, 227, 198)
        m_CustomColors(3) = Color.FromArgb(255, 255, 198)
        m_CustomColors(4) = Color.FromArgb(198, 255, 198)
        m_CustomColors(5) = Color.FromArgb(198, 255, 255)
        m_CustomColors(6) = Color.FromArgb(198, 195, 255)
        m_CustomColors(7) = Color.FromArgb(255, 195, 255)

        m_CustomColors(8) = Color.FromArgb(231, 227, 231)
        m_CustomColors(9) = Color.FromArgb(255, 130, 132)
        m_CustomColors(10) = Color.FromArgb(255, 195, 132)
        m_CustomColors(11) = Color.FromArgb(255, 255, 132)
        m_CustomColors(12) = Color.FromArgb(132, 255, 132)
        m_CustomColors(13) = Color.FromArgb(132, 255, 255)
        m_CustomColors(14) = Color.FromArgb(132, 130, 255)
        m_CustomColors(15) = Color.FromArgb(255, 130, 255)

        m_CustomColors(16) = Color.FromArgb(198, 195, 198)
        m_CustomColors(17) = Color.FromArgb(255, 0, 0)
        m_CustomColors(18) = Color.FromArgb(255, 130, 0)
        m_CustomColors(19) = Color.FromArgb(255, 255, 0)
        m_CustomColors(20) = Color.FromArgb(0, 255, 0)
        m_CustomColors(21) = Color.FromArgb(0, 255, 255)
        m_CustomColors(22) = Color.FromArgb(0, 0, 255)
        m_CustomColors(23) = Color.FromArgb(255, 0, 255)

        m_CustomColors(24) = Color.FromArgb(132, 130, 132)
        m_CustomColors(25) = Color.FromArgb(198, 0, 0)
        m_CustomColors(26) = Color.FromArgb(198, 65, 0)
        m_CustomColors(27) = Color.FromArgb(198, 195, 0)
        m_CustomColors(28) = Color.FromArgb(0, 195, 0)
        m_CustomColors(29) = Color.FromArgb(0, 195, 198)
        m_CustomColors(30) = Color.FromArgb(0, 0, 198)
        m_CustomColors(31) = Color.FromArgb(198, 0, 198)

        m_CustomColors(32) = Color.FromArgb(66, 65, 66)
        m_CustomColors(33) = Color.FromArgb(132, 0, 0)
        m_CustomColors(34) = Color.FromArgb(132, 65, 0)
        m_CustomColors(35) = Color.FromArgb(132, 130, 0)
        m_CustomColors(36) = Color.FromArgb(0, 130, 0)
        m_CustomColors(37) = Color.FromArgb(0, 130, 132)
        m_CustomColors(38) = Color.FromArgb(0, 0, 132)
        m_CustomColors(39) = Color.FromArgb(132, 0, 132)

        m_CustomColors(40) = Color.FromArgb(0, 0, 0)
        m_CustomColors(41) = Color.FromArgb(66, 0, 0)
        m_CustomColors(42) = Color.FromArgb(132, 65, 66)
        m_CustomColors(43) = Color.FromArgb(66, 65, 0)
        m_CustomColors(44) = Color.FromArgb(0, 65, 0)
        m_CustomColors(45) = Color.FromArgb(0, 65, 66)
        m_CustomColors(46) = Color.FromArgb(0, 0, 66)
        m_CustomColors(47) = Color.FromArgb(66, 0, 66)
    End Sub

    Private Sub InitOtherColors()
        Dim type As Type
        Dim fields() As System.Reflection.PropertyInfo
        Dim clr As Color
        Dim pi As System.Reflection.PropertyInfo
        listWeb.BeginUpdate()
        listWeb.Items.Clear()

        Type = GetType(Color)
        fields = type.GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static)
        clr = New Color()
        For Each pi In fields
            listWeb.Items.Add(pi.GetValue(clr, Nothing))
        Next
        listWeb.EndUpdate()

        listSystem.BeginUpdate()
        listSystem.Items.Clear()
        type = GetType(SystemColors)
        fields = type.GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static)
        For Each pi In fields
            listSystem.Items.Add(pi.GetValue(clr, Nothing))
        Next
        listSystem.EndUpdate()
    End Sub

    Private Sub listWeb_DrawItem(ByVal sender As Object, ByVal e As system.Windows.Forms.DrawItemEventArgs) Handles listWeb.DrawItem
        Dim r As RectangleF = New RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height)
        Dim rClr As Rectangle = New Rectangle(r.X + 1, r.Y + 2, 24, r.Height - 4)
        Dim textbrush As Brush = SystemBrushes.ControlText
        If (e.State And DrawItemState.Selected) <> 0 Then
            textbrush = SystemBrushes.HighlightText
            e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds)
        Else
            e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds)
        End If

        Dim clr As Color = CType(listWeb.Items(e.Index), Color)
        e.Graphics.FillRectangle(New SolidBrush(clr), rClr)
        e.Graphics.DrawRectangle(SystemPens.ControlText, rClr)
        r.Offset(30, 0)
        r.Width -= 30
        e.Graphics.DrawString(clr.Name, listWeb.Font, textbrush, r, StringFormat.GenericTypographic)
    End Sub

    Private Sub listWeb_SelectedIndexChanged(ByVal sender As Object, ByVal e As system.EventArgs) Handles listWeb.SelectedIndexChanged
        If Not listWeb.SelectedItem Is Nothing Then
            SelectedColor = CType(listWeb.SelectedItem, Color)
            ColorSelected()
        End If
    End Sub

    Private Sub colorPanel_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles colorPanel.Paint
        Dim r As Rectangle = Rectangle.Empty
        Dim x As Integer, y As Integer
        Dim g As Graphics = e.Graphics
        Dim side As Border3DSide = (Border3DSide.Left Or Border3DSide.Right Or Border3DSide.Top Or Border3DSide.Bottom)
        Dim width As Integer = Me.tabControl1.TabPages(0).ClientRectangle.Width
        Dim iIndex As Integer = 0
        Dim clr As Color

        For Each clr In m_CustomColors
            r = New Rectangle(x, y, 21, 21)
            If r.Right > width Then
                y += 25
                x = 0
                r.X = x
                r.Y = y
            End If

            ControlPaint.DrawBorder3D(g, x, y, 21, 21, Border3DStyle.Sunken, side)
            r.Inflate(-2, -2)
            g.FillRectangle(New SolidBrush(clr), r)

            m_CustomColorsPos(iIndex) = r
            iIndex = iIndex + 1
            x += 24
        Next
    End Sub

    Private Sub listSystem_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles listSystem.DrawItem
        Dim r As RectangleF = New RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height)
        Dim rClr As Rectangle = New Rectangle(r.X + 1, r.Y + 2, 24, r.Height - 4)

        Dim textbrush As Brush = SystemBrushes.ControlText
        If (e.State And DrawItemState.Selected) <> 0 Then
            textbrush = SystemBrushes.HighlightText
            e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds)
        Else
            e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds)

            Dim clr As Color = CType(listSystem.Items(e.Index), Color)
            e.Graphics.FillRectangle(New SolidBrush(clr), rClr)
            e.Graphics.DrawRectangle(SystemPens.ControlText, rClr)
            r.Offset(30, 0)
            r.Width -= 30
            e.Graphics.DrawString(clr.Name, listWeb.Font, textbrush, r, StringFormat.GenericTypographic)
        End If
    End Sub

    Private Sub colorPanel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles colorPanel.MouseUp
        Dim i As Integer
        For i = 0 To 47
            If m_CustomColorsPos(i).Contains(e.X, e.Y) Then
                SelectedColor = m_CustomColors(i)
                ColorSelected()
                Exit For
            End If
        Next
    End Sub

    Private Sub ColorSelected()
        Dim ctrl As DevComponents.DotNetBar.PopupContainerControl = CType(Me.Parent, DevComponents.DotNetBar.PopupContainerControl)
        If ctrl Is Nothing Then Exit Sub
        ctrl.ParentItem.Expanded = False
    End Sub

    Private Sub listSystem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles listSystem.SelectedIndexChanged
        If Not listSystem.SelectedItem Is Nothing Then
            SelectedColor = CType(listSystem.SelectedItem, Color)
            ColorSelected()
        End If
    End Sub
End Class

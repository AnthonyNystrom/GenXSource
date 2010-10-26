<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me._horzProgressBar2 = New Genetibase.SmoothControls.NuGenSmoothProgressBar
        Me._horzProgressBar = New Genetibase.SmoothControls.NuGenSmoothProgressBar
        Me._scrollBar = New Genetibase.SmoothControls.NuGenSmoothScrollBar
        Me._vertProgressBar = New Genetibase.SmoothControls.NuGenSmoothProgressBar
        Me._timer = New System.Windows.Forms.Timer(Me.components)
        Me._marqueeProgressBar = New Genetibase.SmoothControls.NuGenSmoothProgressBar
        Me._horzTrackBar = New Genetibase.SmoothControls.NuGenSmoothTrackBar
        Me._goButton = New Genetibase.SmoothControls.NuGenSmoothButton
        Me.nuGenSmoothPanelEx1 = New Genetibase.SmoothControls.NuGenSmoothPanelEx
        Me.nuGenSmoothPanelEx1.SuspendLayout()
        Me.SuspendLayout()
        '
        '_horzProgressBar2
        '
        Me._horzProgressBar2.Location = New System.Drawing.Point(12, 43)
        Me._horzProgressBar2.Name = "_horzProgressBar2"
        Me._horzProgressBar2.Size = New System.Drawing.Size(180, 25)
        Me._horzProgressBar2.TabIndex = 3
        '
        '_horzProgressBar
        '
        Me._horzProgressBar.Location = New System.Drawing.Point(12, 12)
        Me._horzProgressBar.Name = "_horzProgressBar"
        Me._horzProgressBar.Size = New System.Drawing.Size(180, 25)
        Me._horzProgressBar.Style = Genetibase.[Shared].Controls.NuGenProgressBarStyle.Blocks
        Me._horzProgressBar.TabIndex = 3
        '
        '_scrollBar
        '
        Me._scrollBar.Location = New System.Drawing.Point(236, 12)
        Me._scrollBar.Maximum = 100
        Me._scrollBar.Name = "_scrollBar"
        Me._scrollBar.Orientation = Genetibase.[Shared].NuGenOrientationStyle.Vertical
        Me._scrollBar.Size = New System.Drawing.Size(25, 180)
        Me._scrollBar.TabIndex = 2
        Me._scrollBar.Value = 100
        '
        '_vertProgressBar
        '
        Me._vertProgressBar.Location = New System.Drawing.Point(205, 12)
        Me._vertProgressBar.Name = "_vertProgressBar"
        Me._vertProgressBar.Orientation = Genetibase.[Shared].NuGenOrientationStyle.Vertical
        Me._vertProgressBar.Size = New System.Drawing.Size(25, 180)
        Me._vertProgressBar.TabIndex = 4
        '
        '_timer
        '
        Me._timer.Interval = 500
        '
        '_marqueeProgressBar
        '
        Me._marqueeProgressBar.Location = New System.Drawing.Point(12, 167)
        Me._marqueeProgressBar.Name = "_marqueeProgressBar"
        Me._marqueeProgressBar.Size = New System.Drawing.Size(180, 25)
        Me._marqueeProgressBar.Style = Genetibase.[Shared].Controls.NuGenProgressBarStyle.Marquee
        Me._marqueeProgressBar.TabIndex = 5
        '
        '_horzTrackBar
        '
        Me._horzTrackBar.Location = New System.Drawing.Point(12, 74)
        Me._horzTrackBar.Minimum = 1
        Me._horzTrackBar.Name = "_horzTrackBar"
        Me._horzTrackBar.Size = New System.Drawing.Size(180, 40)
        Me._horzTrackBar.TabIndex = 1
        Me._horzTrackBar.Value = 1
        '
        '_goButton
        '
        Me._goButton.Location = New System.Drawing.Point(12, 120)
        Me._goButton.Name = "_goButton"
        Me._goButton.Size = New System.Drawing.Size(180, 41)
        Me._goButton.TabIndex = 0
        Me._goButton.Text = "&Start"
        Me._goButton.UseVisualStyleBackColor = False
        '
        'nuGenSmoothPanelEx1
        '
        Me.nuGenSmoothPanelEx1.Controls.Add(Me._marqueeProgressBar)
        Me.nuGenSmoothPanelEx1.Controls.Add(Me._vertProgressBar)
        Me.nuGenSmoothPanelEx1.Controls.Add(Me._horzProgressBar2)
        Me.nuGenSmoothPanelEx1.Controls.Add(Me._horzProgressBar)
        Me.nuGenSmoothPanelEx1.Controls.Add(Me._scrollBar)
        Me.nuGenSmoothPanelEx1.Controls.Add(Me._horzTrackBar)
        Me.nuGenSmoothPanelEx1.Controls.Add(Me._goButton)
        Me.nuGenSmoothPanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nuGenSmoothPanelEx1.Location = New System.Drawing.Point(0, 0)
        Me.nuGenSmoothPanelEx1.Name = "nuGenSmoothPanelEx1"
        Me.nuGenSmoothPanelEx1.Size = New System.Drawing.Size(277, 206)
        Me.nuGenSmoothPanelEx1.TabIndex = 2
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(277, 206)
        Me.Controls.Add(Me.nuGenSmoothPanelEx1)
        Me.Name = "MainForm"
        Me.Text = "Cycler"
        Me.nuGenSmoothPanelEx1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _horzProgressBar2 As Genetibase.SmoothControls.NuGenSmoothProgressBar
    Private WithEvents _horzProgressBar As Genetibase.SmoothControls.NuGenSmoothProgressBar
    Private WithEvents _scrollBar As Genetibase.SmoothControls.NuGenSmoothScrollBar
    Private WithEvents _vertProgressBar As Genetibase.SmoothControls.NuGenSmoothProgressBar
    Private WithEvents _timer As System.Windows.Forms.Timer
    Private WithEvents _marqueeProgressBar As Genetibase.SmoothControls.NuGenSmoothProgressBar
    Private WithEvents _horzTrackBar As Genetibase.SmoothControls.NuGenSmoothTrackBar
    Private WithEvents _goButton As Genetibase.SmoothControls.NuGenSmoothButton
    Private WithEvents nuGenSmoothPanelEx1 As Genetibase.SmoothControls.NuGenSmoothPanelEx

End Class

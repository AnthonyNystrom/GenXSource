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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me._saveButton = New Genetibase.[Shared].Controls.NuGenMiniBarButton
        Me.nuGenMiniBarSpace1 = New Genetibase.[Shared].Controls.NuGenMiniBarSpace
        Me._label = New Genetibase.[Shared].Controls.NuGenMiniBarLabel
        Me._openButton = New Genetibase.[Shared].Controls.NuGenMiniBarButton
        Me.nuGenGradientPanel1 = New Genetibase.[Shared].Controls.NuGenGradientPanel
        Me.nuGenMiniBar1 = New Genetibase.[Shared].Controls.NuGenMiniBar
        Me._newButton = New Genetibase.[Shared].Controls.NuGenMiniBarButton
        Me.nuGenGradientPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        '_saveButton
        '
        Me._saveButton.Glyph = CType(resources.GetObject("_saveButton.Glyph"), System.Drawing.Bitmap)
        Me._saveButton.Height = 13
        Me._saveButton.Size = New System.Drawing.Size(13, 13)
        Me._saveButton.Width = 13
        Me._saveButton.X = 26
        Me._saveButton.Y = 0
        '
        'nuGenMiniBarSpace1
        '
        Me.nuGenMiniBarSpace1.Height = 13
        Me.nuGenMiniBarSpace1.Size = New System.Drawing.Size(40, 13)
        Me.nuGenMiniBarSpace1.Width = 40
        Me.nuGenMiniBarSpace1.X = 39
        Me.nuGenMiniBarSpace1.Y = 0
        '
        '_label
        '
        Me._label.Height = 13
        Me._label.Size = New System.Drawing.Size(100, 13)
        Me._label.Text = "Genetibase, Inc."
        Me._label.Width = 100
        Me._label.X = 79
        Me._label.Y = 0
        '
        '_openButton
        '
        Me._openButton.Glyph = CType(resources.GetObject("_openButton.Glyph"), System.Drawing.Bitmap)
        Me._openButton.Height = 13
        Me._openButton.Size = New System.Drawing.Size(13, 13)
        Me._openButton.Width = 13
        Me._openButton.X = 13
        Me._openButton.Y = 0
        '
        'nuGenGradientPanel1
        '
        Me.nuGenGradientPanel1.Controls.Add(Me.nuGenMiniBar1)
        Me.nuGenGradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nuGenGradientPanel1.Location = New System.Drawing.Point(0, 0)
        Me.nuGenGradientPanel1.Name = "nuGenGradientPanel1"
        Me.nuGenGradientPanel1.Size = New System.Drawing.Size(177, 254)
        Me.nuGenGradientPanel1.Watermark = CType(resources.GetObject("nuGenGradientPanel1.Watermark"), System.Drawing.Image)
        Me.nuGenGradientPanel1.WatermarkSize = New System.Drawing.Size(100, 100)
        '
        'nuGenMiniBar1
        '
        Me.nuGenMiniBar1.BackColor = System.Drawing.Color.Transparent
        Me.nuGenMiniBar1.Buttons.AddRange(New Genetibase.[Shared].Controls.NuGenMiniBarControl() {Me._newButton, Me._openButton, Me._saveButton, Me.nuGenMiniBarSpace1, Me._label})
        Me.nuGenMiniBar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.nuGenMiniBar1.Location = New System.Drawing.Point(0, 0)
        Me.nuGenMiniBar1.Name = "nuGenMiniBar1"
        Me.nuGenMiniBar1.Size = New System.Drawing.Size(177, 29)
        Me.nuGenMiniBar1.TabIndex = 0
        '
        '_newButton
        '
        Me._newButton.Glyph = CType(resources.GetObject("_newButton.Glyph"), System.Drawing.Bitmap)
        Me._newButton.Height = 13
        Me._newButton.Size = New System.Drawing.Size(13, 13)
        Me._newButton.Width = 13
        Me._newButton.X = 0
        Me._newButton.Y = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(177, 254)
        Me.Controls.Add(Me.nuGenGradientPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "MainForm"
        Me.Text = "MiniBar"
        Me.nuGenGradientPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _saveButton As Genetibase.Shared.Controls.NuGenMiniBarButton
    Private WithEvents nuGenMiniBarSpace1 As Genetibase.Shared.Controls.NuGenMiniBarSpace
    Private WithEvents _label As Genetibase.Shared.Controls.NuGenMiniBarLabel
    Private WithEvents _openButton As Genetibase.Shared.Controls.NuGenMiniBarButton
    Private WithEvents nuGenGradientPanel1 As Genetibase.Shared.Controls.NuGenGradientPanel
    Private WithEvents nuGenMiniBar1 As Genetibase.Shared.Controls.NuGenMiniBar
    Private WithEvents _newButton As Genetibase.Shared.Controls.NuGenMiniBarButton

End Class

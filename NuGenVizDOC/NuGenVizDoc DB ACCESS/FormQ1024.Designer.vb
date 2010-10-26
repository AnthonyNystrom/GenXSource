<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormQ1024
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.AiresBackground1 = New AiresBackground
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton

        Me.AiresBackground1.SuspendLayout()
        Me.SuspendLayout()
        '
        'AiresBackground1
        '
        Me.AiresBackground1.BackgroundType = AiresBackground.AiresBackgroundTypes.AllBorders
        Me.AiresBackground1.Controls.Add(Me.SimpleButton2)
        Me.AiresBackground1.Controls.Add(Me.SimpleButton1)

        Me.AiresBackground1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AiresBackground1.Location = New System.Drawing.Point(0, 0)
        Me.AiresBackground1.Name = "AiresBackground1"
        Me.AiresBackground1.Size = New System.Drawing.Size(1024, 768)
        Me.AiresBackground1.TabIndex = 0
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Location = New System.Drawing.Point(433, 3)
        Me.SimpleButton2.LookAndFeel.SkinName = "Money Twins"
        Me.SimpleButton2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin
        Me.SimpleButton2.LookAndFeel.UseDefaultLookAndFeel = False
        Me.SimpleButton2.LookAndFeel.UseWindowsXPTheme = False
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(78, 45)
        Me.SimpleButton2.TabIndex = 5
        Me.SimpleButton2.Text = "Propagate"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Location = New System.Drawing.Point(943, 3)
        Me.SimpleButton1.LookAndFeel.SkinName = "Money Twins"
        Me.SimpleButton1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin
        Me.SimpleButton1.LookAndFeel.UseDefaultLookAndFeel = False
        Me.SimpleButton1.LookAndFeel.UseWindowsXPTheme = False
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(78, 45)
        Me.SimpleButton1.TabIndex = 4
        Me.SimpleButton1.Text = "Close"
        '
        'NuGenMediImage4
        '

        '
        'NuGenMediImage3
        '

        '
        'NuGenMediImage2

        '
        'NuGenMediImage1
        '

        '
        'FormQ1024
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1024, 768)
        Me.Controls.Add(Me.AiresBackground1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FormQ1024"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "NuGenMediImage DICOM Window"
        Me.AiresBackground1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AiresBackground1 As AiresBackground

    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormQ
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

        Me.AiresBackground1.SuspendLayout()
        Me.SuspendLayout()
        '
        'AiresBackground1
        '
        Me.AiresBackground1.BackgroundType = AiresBackground.AiresBackgroundTypes.AllBorders

        Me.AiresBackground1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AiresBackground1.Location = New System.Drawing.Point(0, 0)
        Me.AiresBackground1.Name = "AiresBackground1"
        Me.AiresBackground1.Size = New System.Drawing.Size(800, 600)
        Me.AiresBackground1.TabIndex = 0
        '
        'NuGenMediImage1
        '

        '
        'NuGenMediImage2
        '

        '
        'NuGenMediImage3
        '

        '
        'NuGenMediImage4
        '

        '
        'FormQ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.AiresBackground1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FormQ"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "NuGenMediImage DICOM Window"
        Me.AiresBackground1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AiresBackground1 As AiresBackground

End Class

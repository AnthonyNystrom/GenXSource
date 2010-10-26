<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BlendForm
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
        Me._snapUI = New Genetibase.[Shared].Controls.NuGenUISnap
        Me.nuGenBlendSelector1 = New Genetibase.[Shared].Controls.NuGenBlendSelector
        Me.SuspendLayout()
        '
        '_snapUI
        '
        Me._snapUI.HostForm = Me
        '
        'nuGenBlendSelector1
        '
        Me.nuGenBlendSelector1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nuGenBlendSelector1.Location = New System.Drawing.Point(0, 0)
        Me.nuGenBlendSelector1.LowerColor = System.Drawing.Color.White
        Me.nuGenBlendSelector1.Name = "nuGenBlendSelector1"
        Me.nuGenBlendSelector1.Size = New System.Drawing.Size(267, 59)
        Me.nuGenBlendSelector1.TabIndex = 1
        Me.nuGenBlendSelector1.UpperColor = System.Drawing.Color.Black
        Me.nuGenBlendSelector1.Value = 0.3!
        '
        'BlendForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(267, 59)
        Me.Controls.Add(Me.nuGenBlendSelector1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "BlendForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "BlendForm"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _snapUI As Genetibase.Shared.Controls.NuGenUISnap
    Private WithEvents nuGenBlendSelector1 As Genetibase.Shared.Controls.NuGenBlendSelector
End Class

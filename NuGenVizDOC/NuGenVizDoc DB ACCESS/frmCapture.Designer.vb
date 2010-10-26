<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCapture
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        Application.DoEvents()
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.nuGenScreenCap1 = New Genetibase.UI.NuGenScreenCap
        Me.SuspendLayout()
        '
        'nuGenScreenCap1
        '
        Me.nuGenScreenCap1.AutoSizeParentForm = True
        Me.nuGenScreenCap1.CoordsColor = System.Drawing.Color.White
        Me.nuGenScreenCap1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nuGenScreenCap1.Location = New System.Drawing.Point(0, 0)
        Me.nuGenScreenCap1.Name = "nuGenScreenCap1"
        Me.nuGenScreenCap1.Size = New System.Drawing.Size(292, 266)
        Me.nuGenScreenCap1.TabIndex = 1
        '
        'frmCapture
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.ControlBox = False
        Me.Controls.Add(Me.nuGenScreenCap1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCapture"
        Me.ShowInTaskbar = False
        Me.Text = "frmCapture"
        Me.TopMost = True
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents nuGenScreenCap1 As Genetibase.UI.NuGenScreenCap
End Class

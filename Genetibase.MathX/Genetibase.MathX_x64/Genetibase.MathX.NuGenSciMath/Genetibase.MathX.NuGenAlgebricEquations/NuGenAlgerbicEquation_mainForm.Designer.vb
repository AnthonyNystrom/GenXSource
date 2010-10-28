<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenAlgerbicEquation_mainForm
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.AToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BisectionMethodsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ComplexMethodsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NumericUseOfSeriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PolynomialToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label14 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(501, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'AToolStripMenuItem
        '
        Me.AToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BisectionMethodsToolStripMenuItem, Me.ComplexMethodsToolStripMenuItem, Me.NumericUseOfSeriesToolStripMenuItem, Me.PolynomialToolStripMenuItem})
        Me.AToolStripMenuItem.Name = "AToolStripMenuItem"
        Me.AToolStripMenuItem.Size = New System.Drawing.Size(133, 20)
        Me.AToolStripMenuItem.Text = "&Algebric Equations"
        '
        'BisectionMethodsToolStripMenuItem
        '
        Me.BisectionMethodsToolStripMenuItem.Name = "BisectionMethodsToolStripMenuItem"
        Me.BisectionMethodsToolStripMenuItem.Size = New System.Drawing.Size(217, 22)
        Me.BisectionMethodsToolStripMenuItem.Text = "Bisection Methods"
        '
        'ComplexMethodsToolStripMenuItem
        '
        Me.ComplexMethodsToolStripMenuItem.Name = "ComplexMethodsToolStripMenuItem"
        Me.ComplexMethodsToolStripMenuItem.Size = New System.Drawing.Size(217, 22)
        Me.ComplexMethodsToolStripMenuItem.Text = "Complex Methods"
        '
        'NumericUseOfSeriesToolStripMenuItem
        '
        Me.NumericUseOfSeriesToolStripMenuItem.Name = "NumericUseOfSeriesToolStripMenuItem"
        Me.NumericUseOfSeriesToolStripMenuItem.Size = New System.Drawing.Size(217, 22)
        Me.NumericUseOfSeriesToolStripMenuItem.Text = "Numeric Use Of Series"
        '
        'PolynomialToolStripMenuItem
        '
        Me.PolynomialToolStripMenuItem.Name = "PolynomialToolStripMenuItem"
        Me.PolynomialToolStripMenuItem.Size = New System.Drawing.Size(217, 22)
        Me.PolynomialToolStripMenuItem.Text = "Polynomial"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.SystemColors.Window
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label14.Location = New System.Drawing.Point(143, 108)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(210, 25)
        Me.Label14.TabIndex = 7
        Me.Label14.Text = "Algerbic Equations"
        '
        'NuGenAlgerbicEquation_mainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(501, 266)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "NuGenAlgerbicEquation_mainForm"
        Me.Text = "NuGen Algerbic Equation Main"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents AToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BisectionMethodsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComplexMethodsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NumericUseOfSeriesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PolynomialToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class

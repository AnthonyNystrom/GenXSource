<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenPolynomial_MainForm
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ComplexMethodsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AllRootsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BairstowComplexRootToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ComplexRootToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AlgebricEquationMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Window
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(143, 115)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(237, 25)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Polynomial Functions"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ComplexMethodsToolStripMenuItem, Me.BackToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(562, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ComplexMethodsToolStripMenuItem
        '
        Me.ComplexMethodsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AllRootsToolStripMenuItem, Me.BairstowComplexRootToolStripMenuItem, Me.ComplexRootToolStripMenuItem})
        Me.ComplexMethodsToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComplexMethodsToolStripMenuItem.Name = "ComplexMethodsToolStripMenuItem"
        Me.ComplexMethodsToolStripMenuItem.Size = New System.Drawing.Size(148, 20)
        Me.ComplexMethodsToolStripMenuItem.Text = "&Polynomial Functions"
        '
        'AllRootsToolStripMenuItem
        '
        Me.AllRootsToolStripMenuItem.Name = "AllRootsToolStripMenuItem"
        Me.AllRootsToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.AllRootsToolStripMenuItem.Text = "Addition"
        '
        'BairstowComplexRootToolStripMenuItem
        '
        Me.BairstowComplexRootToolStripMenuItem.Name = "BairstowComplexRootToolStripMenuItem"
        Me.BairstowComplexRootToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.BairstowComplexRootToolStripMenuItem.Text = "Division"
        '
        'ComplexRootToolStripMenuItem
        '
        Me.ComplexRootToolStripMenuItem.Name = "ComplexRootToolStripMenuItem"
        Me.ComplexRootToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.ComplexRootToolStripMenuItem.Text = "Multiplication"
        '
        'BackToolStripMenuItem
        '
        Me.BackToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AlgebricEquationMenuToolStripMenuItem})
        Me.BackToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackToolStripMenuItem.Name = "BackToolStripMenuItem"
        Me.BackToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BackToolStripMenuItem.Text = "&Back"
        '
        'AlgebricEquationMenuToolStripMenuItem
        '
        Me.AlgebricEquationMenuToolStripMenuItem.Name = "AlgebricEquationMenuToolStripMenuItem"
        Me.AlgebricEquationMenuToolStripMenuItem.Size = New System.Drawing.Size(232, 22)
        Me.AlgebricEquationMenuToolStripMenuItem.Text = "Algebric Equation Menu"
        '
        'NuGenPolynomial_MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(562, 300)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Name = "NuGenPolynomial_MainForm"
        Me.Text = "NuGen Polynomial Main"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ComplexMethodsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AllRootsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BairstowComplexRootToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComplexRootToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AlgebricEquationMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class

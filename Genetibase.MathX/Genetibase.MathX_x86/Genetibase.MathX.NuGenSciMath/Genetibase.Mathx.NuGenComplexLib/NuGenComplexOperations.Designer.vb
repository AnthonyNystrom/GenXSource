<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenComplexOperations
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
        Me.ComplexOperationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AdditionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SubtractionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MultiplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DivisionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NumericalOperationMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Complex_mathematic_groupbox = New System.Windows.Forms.GroupBox
        Me.complex_clear = New System.Windows.Forms.Button
        Me.complex_mult = New System.Windows.Forms.Button
        Me.complex_div = New System.Windows.Forms.Button
        Me.complex_sub = New System.Windows.Forms.Button
        Me.Complex_add = New System.Windows.Forms.Button
        Me.complex_result = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.complex_second_no = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.complex_first_no = New System.Windows.Forms.TextBox
        Me.complex_label = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.Complex_mathematic_groupbox.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ComplexOperationToolStripMenuItem, Me.BackToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(573, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ComplexOperationToolStripMenuItem
        '
        Me.ComplexOperationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AdditionToolStripMenuItem, Me.SubtractionToolStripMenuItem, Me.MultiplicationToolStripMenuItem, Me.DivisionToolStripMenuItem})
        Me.ComplexOperationToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComplexOperationToolStripMenuItem.Name = "ComplexOperationToolStripMenuItem"
        Me.ComplexOperationToolStripMenuItem.Size = New System.Drawing.Size(136, 20)
        Me.ComplexOperationToolStripMenuItem.Text = "Complex Operation"
        '
        'AdditionToolStripMenuItem
        '
        Me.AdditionToolStripMenuItem.Name = "AdditionToolStripMenuItem"
        Me.AdditionToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.AdditionToolStripMenuItem.Text = "Addition"
        '
        'SubtractionToolStripMenuItem
        '
        Me.SubtractionToolStripMenuItem.Name = "SubtractionToolStripMenuItem"
        Me.SubtractionToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.SubtractionToolStripMenuItem.Text = "Subtraction"
        '
        'MultiplicationToolStripMenuItem
        '
        Me.MultiplicationToolStripMenuItem.Name = "MultiplicationToolStripMenuItem"
        Me.MultiplicationToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.MultiplicationToolStripMenuItem.Text = "Multiplication"
        '
        'DivisionToolStripMenuItem
        '
        Me.DivisionToolStripMenuItem.Name = "DivisionToolStripMenuItem"
        Me.DivisionToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.DivisionToolStripMenuItem.Text = "Division"
        '
        'BackToolStripMenuItem
        '
        Me.BackToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NumericalOperationMenuToolStripMenuItem})
        Me.BackToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackToolStripMenuItem.Name = "BackToolStripMenuItem"
        Me.BackToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BackToolStripMenuItem.Text = "Back"
        '
        'NumericalOperationMenuToolStripMenuItem
        '
        Me.NumericalOperationMenuToolStripMenuItem.Name = "NumericalOperationMenuToolStripMenuItem"
        Me.NumericalOperationMenuToolStripMenuItem.Size = New System.Drawing.Size(247, 22)
        Me.NumericalOperationMenuToolStripMenuItem.Text = "Numerical Operation Menu"
        '
        'Complex_mathematic_groupbox
        '
        Me.Complex_mathematic_groupbox.Controls.Add(Me.complex_clear)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.complex_mult)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.complex_div)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.complex_sub)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.Complex_add)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.complex_result)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.Label3)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.complex_second_no)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.Label2)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.complex_first_no)
        Me.Complex_mathematic_groupbox.Controls.Add(Me.complex_label)
        Me.Complex_mathematic_groupbox.Location = New System.Drawing.Point(27, 37)
        Me.Complex_mathematic_groupbox.Name = "Complex_mathematic_groupbox"
        Me.Complex_mathematic_groupbox.Size = New System.Drawing.Size(482, 212)
        Me.Complex_mathematic_groupbox.TabIndex = 1
        Me.Complex_mathematic_groupbox.TabStop = False
        '
        'complex_clear
        '
        Me.complex_clear.BackColor = System.Drawing.SystemColors.Window
        Me.complex_clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.complex_clear.ForeColor = System.Drawing.SystemColors.WindowText
        Me.complex_clear.Location = New System.Drawing.Point(317, 129)
        Me.complex_clear.Name = "complex_clear"
        Me.complex_clear.Size = New System.Drawing.Size(67, 38)
        Me.complex_clear.TabIndex = 7
        Me.complex_clear.Text = "Clear"
        Me.complex_clear.UseVisualStyleBackColor = False
        '
        'complex_mult
        '
        Me.complex_mult.BackColor = System.Drawing.SystemColors.Window
        Me.complex_mult.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.complex_mult.ForeColor = System.Drawing.SystemColors.WindowText
        Me.complex_mult.Location = New System.Drawing.Point(365, 23)
        Me.complex_mult.Name = "complex_mult"
        Me.complex_mult.Size = New System.Drawing.Size(63, 36)
        Me.complex_mult.TabIndex = 5
        Me.complex_mult.Text = "MULT"
        Me.complex_mult.UseVisualStyleBackColor = False
        '
        'complex_div
        '
        Me.complex_div.BackColor = System.Drawing.SystemColors.Window
        Me.complex_div.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.complex_div.ForeColor = System.Drawing.SystemColors.WindowText
        Me.complex_div.Location = New System.Drawing.Point(365, 74)
        Me.complex_div.Name = "complex_div"
        Me.complex_div.Size = New System.Drawing.Size(63, 38)
        Me.complex_div.TabIndex = 6
        Me.complex_div.Text = "DIV"
        Me.complex_div.UseVisualStyleBackColor = False
        '
        'complex_sub
        '
        Me.complex_sub.BackColor = System.Drawing.SystemColors.Window
        Me.complex_sub.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.complex_sub.ForeColor = System.Drawing.SystemColors.WindowText
        Me.complex_sub.Location = New System.Drawing.Point(280, 74)
        Me.complex_sub.Name = "complex_sub"
        Me.complex_sub.Size = New System.Drawing.Size(67, 38)
        Me.complex_sub.TabIndex = 4
        Me.complex_sub.Text = "SUB"
        Me.complex_sub.UseVisualStyleBackColor = False
        '
        'Complex_add
        '
        Me.Complex_add.BackColor = System.Drawing.SystemColors.Window
        Me.Complex_add.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Complex_add.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Complex_add.Location = New System.Drawing.Point(280, 23)
        Me.Complex_add.Name = "Complex_add"
        Me.Complex_add.Size = New System.Drawing.Size(67, 38)
        Me.Complex_add.TabIndex = 3
        Me.Complex_add.Text = "ADD"
        Me.Complex_add.UseVisualStyleBackColor = False
        '
        'complex_result
        '
        Me.complex_result.Enabled = False
        Me.complex_result.Location = New System.Drawing.Point(131, 110)
        Me.complex_result.Name = "complex_result"
        Me.complex_result.Size = New System.Drawing.Size(100, 20)
        Me.complex_result.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 110)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Result"
        '
        'complex_second_no
        '
        Me.complex_second_no.Location = New System.Drawing.Point(132, 68)
        Me.complex_second_no.Name = "complex_second_no"
        Me.complex_second_no.Size = New System.Drawing.Size(99, 20)
        Me.complex_second_no.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Enter Second Number"
        '
        'complex_first_no
        '
        Me.complex_first_no.Location = New System.Drawing.Point(131, 25)
        Me.complex_first_no.Name = "complex_first_no"
        Me.complex_first_no.Size = New System.Drawing.Size(100, 20)
        Me.complex_first_no.TabIndex = 1
        '
        'complex_label
        '
        Me.complex_label.AutoSize = True
        Me.complex_label.Location = New System.Drawing.Point(16, 25)
        Me.complex_label.Name = "complex_label"
        Me.complex_label.Size = New System.Drawing.Size(94, 13)
        Me.complex_label.TabIndex = 0
        Me.complex_label.Text = "Enter First Number"
        '
        'NuGenComplexOperations
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(573, 310)
        Me.Controls.Add(Me.Complex_mathematic_groupbox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.ForeColor = System.Drawing.SystemColors.Window
        Me.Location = New System.Drawing.Point(30, 30)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "NuGenComplexOperations"
        Me.Text = "NuGen Complex Operations"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Complex_mathematic_groupbox.ResumeLayout(False)
        Me.Complex_mathematic_groupbox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ComplexOperationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdditionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SubtractionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MultiplicationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DivisionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NumericalOperationMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Complex_mathematic_groupbox As System.Windows.Forms.GroupBox
    Friend WithEvents complex_result As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents complex_second_no As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents complex_first_no As System.Windows.Forms.TextBox
    Friend WithEvents complex_label As System.Windows.Forms.Label
    Friend WithEvents complex_mult As System.Windows.Forms.Button
    Friend WithEvents complex_div As System.Windows.Forms.Button
    Friend WithEvents complex_sub As System.Windows.Forms.Button
    Friend WithEvents Complex_add As System.Windows.Forms.Button
    Friend WithEvents complex_clear As System.Windows.Forms.Button
End Class

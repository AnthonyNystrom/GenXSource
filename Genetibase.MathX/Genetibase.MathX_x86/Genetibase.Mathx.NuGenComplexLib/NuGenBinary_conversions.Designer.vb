<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenBinary_conversions
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
        Me.BackToMainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToBinaryMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NuGenNumericMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Binary_Mathematical_operations = New System.Windows.Forms.GroupBox
        Me.binary_clear = New System.Windows.Forms.Button
        Me.binary_hexa_to_deci = New System.Windows.Forms.Button
        Me.binary_hexa_to_binary = New System.Windows.Forms.Button
        Me.binary_deci_to_hexa = New System.Windows.Forms.Button
        Me.binary_deci_to_binary = New System.Windows.Forms.Button
        Me.Binary_to_hexa = New System.Windows.Forms.Button
        Me.binary_to_deci = New System.Windows.Forms.Button
        Me.binary_p_to_n = New System.Windows.Forms.Button
        Me.binary_shift_right = New System.Windows.Forms.Button
        Me.binary_shift_left = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.result_oper = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.first_no = New System.Windows.Forms.TextBox
        Me.MenuStrip1.SuspendLayout()
        Me.Binary_Mathematical_operations.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackToMainMenuToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(652, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'BackToMainMenuToolStripMenuItem
        '
        Me.BackToMainMenuToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackToBinaryMenuToolStripMenuItem, Me.NuGenNumericMenuToolStripMenuItem, Me.BackToToolStripMenuItem})
        Me.BackToMainMenuToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackToMainMenuToolStripMenuItem.Name = "BackToMainMenuToolStripMenuItem"
        Me.BackToMainMenuToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BackToMainMenuToolStripMenuItem.Text = "&Back"
        '
        'BackToBinaryMenuToolStripMenuItem
        '
        Me.BackToBinaryMenuToolStripMenuItem.Name = "BackToBinaryMenuToolStripMenuItem"
        Me.BackToBinaryMenuToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.BackToBinaryMenuToolStripMenuItem.Text = "Binary Menu"
        '
        'NuGenNumericMenuToolStripMenuItem
        '
        Me.NuGenNumericMenuToolStripMenuItem.Name = "NuGenNumericMenuToolStripMenuItem"
        Me.NuGenNumericMenuToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.NuGenNumericMenuToolStripMenuItem.Text = "NuGen Numeric Menu"
        '
        'BackToToolStripMenuItem
        '
        Me.BackToToolStripMenuItem.Name = "BackToToolStripMenuItem"
        Me.BackToToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.BackToToolStripMenuItem.Text = "Main Menu"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'Binary_Mathematical_operations
        '
        Me.Binary_Mathematical_operations.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.Binary_Mathematical_operations.Controls.Add(Me.binary_clear)
        Me.Binary_Mathematical_operations.Controls.Add(Me.binary_hexa_to_deci)
        Me.Binary_Mathematical_operations.Controls.Add(Me.binary_hexa_to_binary)
        Me.Binary_Mathematical_operations.Controls.Add(Me.binary_deci_to_hexa)
        Me.Binary_Mathematical_operations.Controls.Add(Me.binary_deci_to_binary)
        Me.Binary_Mathematical_operations.Controls.Add(Me.Binary_to_hexa)
        Me.Binary_Mathematical_operations.Controls.Add(Me.binary_to_deci)
        Me.Binary_Mathematical_operations.Controls.Add(Me.binary_p_to_n)
        Me.Binary_Mathematical_operations.Controls.Add(Me.binary_shift_right)
        Me.Binary_Mathematical_operations.Controls.Add(Me.binary_shift_left)
        Me.Binary_Mathematical_operations.Controls.Add(Me.Label1)
        Me.Binary_Mathematical_operations.Controls.Add(Me.result_oper)
        Me.Binary_Mathematical_operations.Controls.Add(Me.Label8)
        Me.Binary_Mathematical_operations.Controls.Add(Me.first_no)
        Me.Binary_Mathematical_operations.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Binary_Mathematical_operations.Location = New System.Drawing.Point(51, 32)
        Me.Binary_Mathematical_operations.Name = "Binary_Mathematical_operations"
        Me.Binary_Mathematical_operations.Size = New System.Drawing.Size(545, 222)
        Me.Binary_Mathematical_operations.TabIndex = 20
        Me.Binary_Mathematical_operations.TabStop = False
        '
        'binary_clear
        '
        Me.binary_clear.BackColor = System.Drawing.SystemColors.Window
        Me.binary_clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binary_clear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.binary_clear.Location = New System.Drawing.Point(421, 172)
        Me.binary_clear.Name = "binary_clear"
        Me.binary_clear.Size = New System.Drawing.Size(120, 30)
        Me.binary_clear.TabIndex = 11
        Me.binary_clear.Text = "Clear"
        Me.binary_clear.UseVisualStyleBackColor = False
        '
        'binary_hexa_to_deci
        '
        Me.binary_hexa_to_deci.BackColor = System.Drawing.SystemColors.Window
        Me.binary_hexa_to_deci.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binary_hexa_to_deci.ForeColor = System.Drawing.SystemColors.ControlText
        Me.binary_hexa_to_deci.Location = New System.Drawing.Point(283, 172)
        Me.binary_hexa_to_deci.Name = "binary_hexa_to_deci"
        Me.binary_hexa_to_deci.Size = New System.Drawing.Size(120, 30)
        Me.binary_hexa_to_deci.TabIndex = 6
        Me.binary_hexa_to_deci.Text = "Hexa To Deci"
        Me.binary_hexa_to_deci.UseVisualStyleBackColor = False
        '
        'binary_hexa_to_binary
        '
        Me.binary_hexa_to_binary.BackColor = System.Drawing.SystemColors.Window
        Me.binary_hexa_to_binary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binary_hexa_to_binary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.binary_hexa_to_binary.Location = New System.Drawing.Point(282, 135)
        Me.binary_hexa_to_binary.Name = "binary_hexa_to_binary"
        Me.binary_hexa_to_binary.Size = New System.Drawing.Size(120, 30)
        Me.binary_hexa_to_binary.TabIndex = 5
        Me.binary_hexa_to_binary.Text = "Hexa To Binary"
        Me.binary_hexa_to_binary.UseVisualStyleBackColor = False
        '
        'binary_deci_to_hexa
        '
        Me.binary_deci_to_hexa.BackColor = System.Drawing.SystemColors.Window
        Me.binary_deci_to_hexa.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binary_deci_to_hexa.ForeColor = System.Drawing.SystemColors.ControlText
        Me.binary_deci_to_hexa.Location = New System.Drawing.Point(421, 134)
        Me.binary_deci_to_hexa.Name = "binary_deci_to_hexa"
        Me.binary_deci_to_hexa.Size = New System.Drawing.Size(120, 30)
        Me.binary_deci_to_hexa.TabIndex = 10
        Me.binary_deci_to_hexa.Text = "Deci To Hexa"
        Me.binary_deci_to_hexa.UseVisualStyleBackColor = False
        '
        'binary_deci_to_binary
        '
        Me.binary_deci_to_binary.BackColor = System.Drawing.SystemColors.Window
        Me.binary_deci_to_binary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binary_deci_to_binary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.binary_deci_to_binary.Location = New System.Drawing.Point(421, 98)
        Me.binary_deci_to_binary.Name = "binary_deci_to_binary"
        Me.binary_deci_to_binary.Size = New System.Drawing.Size(120, 30)
        Me.binary_deci_to_binary.TabIndex = 9
        Me.binary_deci_to_binary.Text = "Deci To Binary"
        Me.binary_deci_to_binary.UseVisualStyleBackColor = False
        '
        'Binary_to_hexa
        '
        Me.Binary_to_hexa.BackColor = System.Drawing.SystemColors.Window
        Me.Binary_to_hexa.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Binary_to_hexa.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Binary_to_hexa.Location = New System.Drawing.Point(421, 62)
        Me.Binary_to_hexa.Name = "Binary_to_hexa"
        Me.Binary_to_hexa.Size = New System.Drawing.Size(120, 30)
        Me.Binary_to_hexa.TabIndex = 8
        Me.Binary_to_hexa.Text = "Binary To Hexa"
        Me.Binary_to_hexa.UseVisualStyleBackColor = False
        '
        'binary_to_deci
        '
        Me.binary_to_deci.BackColor = System.Drawing.SystemColors.Window
        Me.binary_to_deci.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binary_to_deci.ForeColor = System.Drawing.SystemColors.ControlText
        Me.binary_to_deci.Location = New System.Drawing.Point(421, 26)
        Me.binary_to_deci.Name = "binary_to_deci"
        Me.binary_to_deci.Size = New System.Drawing.Size(120, 30)
        Me.binary_to_deci.TabIndex = 7
        Me.binary_to_deci.Text = "Binary To Deci"
        Me.binary_to_deci.UseVisualStyleBackColor = False
        '
        'binary_p_to_n
        '
        Me.binary_p_to_n.BackColor = System.Drawing.SystemColors.Window
        Me.binary_p_to_n.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binary_p_to_n.ForeColor = System.Drawing.SystemColors.ControlText
        Me.binary_p_to_n.Location = New System.Drawing.Point(282, 26)
        Me.binary_p_to_n.Name = "binary_p_to_n"
        Me.binary_p_to_n.Size = New System.Drawing.Size(120, 30)
        Me.binary_p_to_n.TabIndex = 2
        Me.binary_p_to_n.Text = "+ve To -ve"
        Me.binary_p_to_n.UseVisualStyleBackColor = False
        '
        'binary_shift_right
        '
        Me.binary_shift_right.BackColor = System.Drawing.SystemColors.Window
        Me.binary_shift_right.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binary_shift_right.ForeColor = System.Drawing.SystemColors.ControlText
        Me.binary_shift_right.Location = New System.Drawing.Point(282, 98)
        Me.binary_shift_right.Name = "binary_shift_right"
        Me.binary_shift_right.Size = New System.Drawing.Size(120, 30)
        Me.binary_shift_right.TabIndex = 4
        Me.binary_shift_right.Text = "Shift To Right"
        Me.binary_shift_right.UseVisualStyleBackColor = False
        '
        'binary_shift_left
        '
        Me.binary_shift_left.BackColor = System.Drawing.SystemColors.Window
        Me.binary_shift_left.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binary_shift_left.ForeColor = System.Drawing.SystemColors.ControlText
        Me.binary_shift_left.Location = New System.Drawing.Point(282, 62)
        Me.binary_shift_left.Name = "binary_shift_left"
        Me.binary_shift_left.Size = New System.Drawing.Size(120, 30)
        Me.binary_shift_left.TabIndex = 3
        Me.binary_shift_left.Text = "Shift To Left"
        Me.binary_shift_left.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label1.Location = New System.Drawing.Point(11, 133)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "The Result is :"
        '
        'result_oper
        '
        Me.result_oper.Enabled = False
        Me.result_oper.Location = New System.Drawing.Point(162, 126)
        Me.result_oper.Name = "result_oper"
        Me.result_oper.Size = New System.Drawing.Size(96, 20)
        Me.result_oper.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label8.Location = New System.Drawing.Point(6, 73)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(146, 13)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "Please Enter the number"
        '
        'first_no
        '
        Me.first_no.Location = New System.Drawing.Point(162, 66)
        Me.first_no.Name = "first_no"
        Me.first_no.Size = New System.Drawing.Size(96, 20)
        Me.first_no.TabIndex = 0
        '
        'NuGenBinary_conversions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(652, 266)
        Me.Controls.Add(Me.Binary_Mathematical_operations)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.Name = "NuGenBinary_conversions"
        Me.Text = "NuGen Binary Conversions"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Binary_Mathematical_operations.ResumeLayout(False)
        Me.Binary_Mathematical_operations.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents BackToMainMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToBinaryMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Binary_Mathematical_operations As System.Windows.Forms.GroupBox
    Friend WithEvents binary_to_deci As System.Windows.Forms.Button
    Friend WithEvents binary_p_to_n As System.Windows.Forms.Button
    Friend WithEvents binary_shift_right As System.Windows.Forms.Button
    Friend WithEvents binary_shift_left As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents result_oper As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents first_no As System.Windows.Forms.TextBox
    Friend WithEvents binary_hexa_to_deci As System.Windows.Forms.Button
    Friend WithEvents binary_hexa_to_binary As System.Windows.Forms.Button
    Friend WithEvents binary_deci_to_hexa As System.Windows.Forms.Button
    Friend WithEvents binary_deci_to_binary As System.Windows.Forms.Button
    Friend WithEvents Binary_to_hexa As System.Windows.Forms.Button
    Friend WithEvents binary_clear As System.Windows.Forms.Button
    Friend WithEvents NuGenNumericMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class

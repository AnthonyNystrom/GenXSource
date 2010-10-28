<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenFunctionApproximation_ParabolicLeastSquareForm
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.c_val = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.b_val = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.a_val = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.n_val = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.d_val = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.clear = New System.Windows.Forms.Button
        Me.linear_least_square = New System.Windows.Forms.Button
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.BToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SeriesMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AlgebricToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label5 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.c_val)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.b_val)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.a_val)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.n_val)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.d_val)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.clear)
        Me.GroupBox1.Controls.Add(Me.linear_least_square)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 55)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(573, 307)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.Window
        Me.Label1.Location = New System.Drawing.Point(55, 264)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Result Of C"
        '
        'c_val
        '
        Me.c_val.Enabled = False
        Me.c_val.Location = New System.Drawing.Point(177, 261)
        Me.c_val.Name = "c_val"
        Me.c_val.Size = New System.Drawing.Size(104, 20)
        Me.c_val.TabIndex = 23
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.Window
        Me.Label8.Location = New System.Drawing.Point(55, 229)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 13)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "Result Of B"
        '
        'b_val
        '
        Me.b_val.Enabled = False
        Me.b_val.Location = New System.Drawing.Point(177, 226)
        Me.b_val.Name = "b_val"
        Me.b_val.Size = New System.Drawing.Size(104, 20)
        Me.b_val.TabIndex = 21
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.Window
        Me.Label4.Location = New System.Drawing.Point(55, 193)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Result Of A"
        '
        'a_val
        '
        Me.a_val.Enabled = False
        Me.a_val.Location = New System.Drawing.Point(177, 190)
        Me.a_val.Name = "a_val"
        Me.a_val.Size = New System.Drawing.Size(104, 20)
        Me.a_val.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.Window
        Me.Label3.Location = New System.Drawing.Point(22, 42)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(114, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "No. Of Data Points"
        '
        'n_val
        '
        Me.n_val.Location = New System.Drawing.Point(177, 39)
        Me.n_val.Name = "n_val"
        Me.n_val.Size = New System.Drawing.Size(104, 20)
        Me.n_val.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.Window
        Me.Label6.Location = New System.Drawing.Point(55, 149)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(116, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Standard Deviation"
        '
        'd_val
        '
        Me.d_val.Enabled = False
        Me.d_val.Location = New System.Drawing.Point(177, 146)
        Me.d_val.Name = "d_val"
        Me.d_val.Size = New System.Drawing.Size(104, 20)
        Me.d_val.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.Window
        Me.Label2.Location = New System.Drawing.Point(6, 107)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Result Series"
        '
        'clear
        '
        Me.clear.BackColor = System.Drawing.SystemColors.Window
        Me.clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clear.Location = New System.Drawing.Point(357, 129)
        Me.clear.Name = "clear"
        Me.clear.Size = New System.Drawing.Size(155, 30)
        Me.clear.TabIndex = 2
        Me.clear.Text = "Clear"
        Me.clear.UseVisualStyleBackColor = False
        '
        'linear_least_square
        '
        Me.linear_least_square.BackColor = System.Drawing.SystemColors.Window
        Me.linear_least_square.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.linear_least_square.Location = New System.Drawing.Point(357, 80)
        Me.linear_least_square.Name = "linear_least_square"
        Me.linear_least_square.Size = New System.Drawing.Size(155, 29)
        Me.linear_least_square.TabIndex = 1
        Me.linear_least_square.Text = "Parabolic Least Squre"
        Me.linear_least_square.UseVisualStyleBackColor = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(614, 24)
        Me.MenuStrip1.TabIndex = 12
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'BToolStripMenuItem
        '
        Me.BToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SeriesMenuToolStripMenuItem, Me.AlgebricToolStripMenuItem, Me.MainMenuToolStripMenuItem})
        Me.BToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BToolStripMenuItem.Name = "BToolStripMenuItem"
        Me.BToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BToolStripMenuItem.Text = "&Back"
        '
        'SeriesMenuToolStripMenuItem
        '
        Me.SeriesMenuToolStripMenuItem.Name = "SeriesMenuToolStripMenuItem"
        Me.SeriesMenuToolStripMenuItem.Size = New System.Drawing.Size(272, 22)
        Me.SeriesMenuToolStripMenuItem.Text = "Matrix Operation Menu"
        '
        'AlgebricToolStripMenuItem
        '
        Me.AlgebricToolStripMenuItem.Name = "AlgebricToolStripMenuItem"
        Me.AlgebricToolStripMenuItem.Size = New System.Drawing.Size(272, 22)
        Me.AlgebricToolStripMenuItem.Text = "Function Approximation Menu"
        '
        'MainMenuToolStripMenuItem
        '
        Me.MainMenuToolStripMenuItem.Name = "MainMenuToolStripMenuItem"
        Me.MainMenuToolStripMenuItem.Size = New System.Drawing.Size(272, 22)
        Me.MainMenuToolStripMenuItem.Text = "Main Menu"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.Window
        Me.Label5.Location = New System.Drawing.Point(120, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(395, 20)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Parabolic Least  Square Function Approximation"
        '
        'NuGenFunctionApproximation_ParabolicLeastSquareForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(614, 371)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Name = "NuGenFunctionApproximation_ParabolicLeastSquareForm"
        Me.Text = "NuGenFunctionApproximation_ParabolicLeastSquareForm"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents c_val As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents b_val As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents a_val As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents n_val As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents d_val As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents clear As System.Windows.Forms.Button
    Friend WithEvents linear_least_square As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents BToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SeriesMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AlgebricToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class

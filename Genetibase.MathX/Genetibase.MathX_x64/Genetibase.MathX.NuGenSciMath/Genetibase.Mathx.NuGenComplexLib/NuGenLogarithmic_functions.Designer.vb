<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenLogarithmic_functions
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
        Me.LogirithmicFunctionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FactorialToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LogNToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NumericalMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Factorial = New System.Windows.Forms.GroupBox
        Me.Factorial_button = New System.Windows.Forms.Button
        Me.Second_no = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.First_no = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Logn_group = New System.Windows.Forms.GroupBox
        Me.logn_sec_no = New System.Windows.Forms.TextBox
        Me.sec_no = New System.Windows.Forms.Label
        Me.LogN = New System.Windows.Forms.Button
        Me.logn_result = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.logn_first_no = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Log_group = New System.Windows.Forms.GroupBox
        Me.Log_button = New System.Windows.Forms.Button
        Me.log_result = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.log_first_no = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.Factorial.SuspendLayout()
        Me.Logn_group.SuspendLayout()
        Me.Log_group.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LogirithmicFunctionToolStripMenuItem, Me.BackToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(421, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'LogirithmicFunctionToolStripMenuItem
        '
        Me.LogirithmicFunctionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FactorialToolStripMenuItem, Me.LogToolStripMenuItem, Me.LogNToolStripMenuItem})
        Me.LogirithmicFunctionToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogirithmicFunctionToolStripMenuItem.Name = "LogirithmicFunctionToolStripMenuItem"
        Me.LogirithmicFunctionToolStripMenuItem.Size = New System.Drawing.Size(144, 20)
        Me.LogirithmicFunctionToolStripMenuItem.Text = "Logirithmic Function"
        '
        'FactorialToolStripMenuItem
        '
        Me.FactorialToolStripMenuItem.Name = "FactorialToolStripMenuItem"
        Me.FactorialToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.FactorialToolStripMenuItem.Text = "Factorial"
        '
        'LogToolStripMenuItem
        '
        Me.LogToolStripMenuItem.Name = "LogToolStripMenuItem"
        Me.LogToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.LogToolStripMenuItem.Text = "Log"
        '
        'LogNToolStripMenuItem
        '
        Me.LogNToolStripMenuItem.Name = "LogNToolStripMenuItem"
        Me.LogNToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.LogNToolStripMenuItem.Text = "LogN"
        '
        'BackToolStripMenuItem
        '
        Me.BackToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NumericalMenuToolStripMenuItem})
        Me.BackToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackToolStripMenuItem.Name = "BackToolStripMenuItem"
        Me.BackToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BackToolStripMenuItem.Text = "Back"
        '
        'NumericalMenuToolStripMenuItem
        '
        Me.NumericalMenuToolStripMenuItem.Name = "NumericalMenuToolStripMenuItem"
        Me.NumericalMenuToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.NumericalMenuToolStripMenuItem.Text = "Numerical Menu"
        '
        'Factorial
        '
        Me.Factorial.Controls.Add(Me.Factorial_button)
        Me.Factorial.Controls.Add(Me.Second_no)
        Me.Factorial.Controls.Add(Me.Label2)
        Me.Factorial.Controls.Add(Me.First_no)
        Me.Factorial.Controls.Add(Me.Label1)
        Me.Factorial.Location = New System.Drawing.Point(0, 64)
        Me.Factorial.Name = "Factorial"
        Me.Factorial.Size = New System.Drawing.Size(340, 155)
        Me.Factorial.TabIndex = 1
        Me.Factorial.TabStop = False
        Me.Factorial.Visible = False
        '
        'Factorial_button
        '
        Me.Factorial_button.BackColor = System.Drawing.SystemColors.Window
        Me.Factorial_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Factorial_button.Location = New System.Drawing.Point(119, 114)
        Me.Factorial_button.Name = "Factorial_button"
        Me.Factorial_button.Size = New System.Drawing.Size(96, 35)
        Me.Factorial_button.TabIndex = 4
        Me.Factorial_button.Text = "Factorial"
        Me.Factorial_button.UseVisualStyleBackColor = False
        '
        'Second_no
        '
        Me.Second_no.Enabled = False
        Me.Second_no.Location = New System.Drawing.Point(163, 69)
        Me.Second_no.Name = "Second_no"
        Me.Second_no.Size = New System.Drawing.Size(145, 20)
        Me.Second_no.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Label2.Location = New System.Drawing.Point(24, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Result"
        '
        'First_no
        '
        Me.First_no.Location = New System.Drawing.Point(163, 24)
        Me.First_no.Name = "First_no"
        Me.First_no.Size = New System.Drawing.Size(145, 20)
        Me.First_no.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Label1.Location = New System.Drawing.Point(24, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Enter The Number"
        '
        'Logn_group
        '
        Me.Logn_group.Controls.Add(Me.logn_sec_no)
        Me.Logn_group.Controls.Add(Me.sec_no)
        Me.Logn_group.Controls.Add(Me.LogN)
        Me.Logn_group.Controls.Add(Me.logn_result)
        Me.Logn_group.Controls.Add(Me.Label3)
        Me.Logn_group.Controls.Add(Me.logn_first_no)
        Me.Logn_group.Controls.Add(Me.Label4)
        Me.Logn_group.Location = New System.Drawing.Point(12, 54)
        Me.Logn_group.Name = "Logn_group"
        Me.Logn_group.Size = New System.Drawing.Size(386, 204)
        Me.Logn_group.TabIndex = 2
        Me.Logn_group.TabStop = False
        Me.Logn_group.Visible = False
        '
        'logn_sec_no
        '
        Me.logn_sec_no.Location = New System.Drawing.Point(199, 63)
        Me.logn_sec_no.Name = "logn_sec_no"
        Me.logn_sec_no.Size = New System.Drawing.Size(145, 20)
        Me.logn_sec_no.TabIndex = 7
        '
        'sec_no
        '
        Me.sec_no.AutoSize = True
        Me.sec_no.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sec_no.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.sec_no.Location = New System.Drawing.Point(24, 67)
        Me.sec_no.Name = "sec_no"
        Me.sec_no.Size = New System.Drawing.Size(157, 13)
        Me.sec_no.TabIndex = 6
        Me.sec_no.Text = "Enter The Second Number"
        '
        'LogN
        '
        Me.LogN.BackColor = System.Drawing.SystemColors.Window
        Me.LogN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogN.Location = New System.Drawing.Point(141, 152)
        Me.LogN.Name = "LogN"
        Me.LogN.Size = New System.Drawing.Size(96, 35)
        Me.LogN.TabIndex = 5
        Me.LogN.Text = "LogN"
        Me.LogN.UseVisualStyleBackColor = False
        '
        'logn_result
        '
        Me.logn_result.Enabled = False
        Me.logn_result.Location = New System.Drawing.Point(199, 104)
        Me.logn_result.Name = "logn_result"
        Me.logn_result.Size = New System.Drawing.Size(145, 20)
        Me.logn_result.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label3.Location = New System.Drawing.Point(24, 108)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Result"
        '
        'logn_first_no
        '
        Me.logn_first_no.Location = New System.Drawing.Point(199, 23)
        Me.logn_first_no.Name = "logn_first_no"
        Me.logn_first_no.Size = New System.Drawing.Size(145, 20)
        Me.logn_first_no.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label4.Location = New System.Drawing.Point(24, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(138, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Enter The First Number"
        '
        'Log_group
        '
        Me.Log_group.Controls.Add(Me.Log_button)
        Me.Log_group.Controls.Add(Me.log_result)
        Me.Log_group.Controls.Add(Me.Label5)
        Me.Log_group.Controls.Add(Me.log_first_no)
        Me.Log_group.Controls.Add(Me.Label6)
        Me.Log_group.Location = New System.Drawing.Point(18, 27)
        Me.Log_group.Name = "Log_group"
        Me.Log_group.Size = New System.Drawing.Size(340, 155)
        Me.Log_group.TabIndex = 3
        Me.Log_group.TabStop = False
        Me.Log_group.Visible = False
        '
        'Log_button
        '
        Me.Log_button.BackColor = System.Drawing.SystemColors.Window
        Me.Log_button.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Log_button.Location = New System.Drawing.Point(119, 114)
        Me.Log_button.Name = "Log_button"
        Me.Log_button.Size = New System.Drawing.Size(96, 35)
        Me.Log_button.TabIndex = 4
        Me.Log_button.Text = "Log"
        Me.Log_button.UseVisualStyleBackColor = False
        '
        'log_result
        '
        Me.log_result.Enabled = False
        Me.log_result.Location = New System.Drawing.Point(163, 69)
        Me.log_result.Name = "log_result"
        Me.log_result.Size = New System.Drawing.Size(145, 20)
        Me.log_result.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Label5.Location = New System.Drawing.Point(24, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Result"
        '
        'log_first_no
        '
        Me.log_first_no.Location = New System.Drawing.Point(163, 24)
        Me.log_first_no.Name = "log_first_no"
        Me.log_first_no.Size = New System.Drawing.Size(145, 20)
        Me.log_first_no.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Label6.Location = New System.Drawing.Point(24, 27)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(110, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Enter The Number"
        '
        'NuGenLogarithmic_functions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(421, 299)
        Me.Controls.Add(Me.Logn_group)
        Me.Controls.Add(Me.Log_group)
        Me.Controls.Add(Me.Factorial)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "NuGenLogarithmic_functions"
        Me.Text = "Logirithmic Functions"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Factorial.ResumeLayout(False)
        Me.Factorial.PerformLayout()
        Me.Logn_group.ResumeLayout(False)
        Me.Logn_group.PerformLayout()
        Me.Log_group.ResumeLayout(False)
        Me.Log_group.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents LogirithmicFunctionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FactorialToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LogNToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NumericalMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Factorial As System.Windows.Forms.GroupBox
    Friend WithEvents First_no As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Second_no As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Factorial_button As System.Windows.Forms.Button
    Friend WithEvents Logn_group As System.Windows.Forms.GroupBox
    Friend WithEvents logn_sec_no As System.Windows.Forms.TextBox
    Friend WithEvents sec_no As System.Windows.Forms.Label
    Friend WithEvents LogN As System.Windows.Forms.Button
    Friend WithEvents logn_result As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents logn_first_no As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Log_group As System.Windows.Forms.GroupBox
    Friend WithEvents Log_button As System.Windows.Forms.Button
    Friend WithEvents log_result As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents log_first_no As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class

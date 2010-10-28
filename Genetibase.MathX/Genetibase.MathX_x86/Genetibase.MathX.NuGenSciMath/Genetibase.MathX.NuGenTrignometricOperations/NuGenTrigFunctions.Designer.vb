<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenTrigFunctions
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
        Me.SeriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ArcTangentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CosineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExponentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NaturalLogarithmicToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LogBaseTenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PowerOfTenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpposedAngleLengthToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TrignometricMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Trig_groupbox = New System.Windows.Forms.GroupBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.trig_second_no = New System.Windows.Forms.TextBox
        Me.trig_exponent = New System.Windows.Forms.Button
        Me.trig_cos = New System.Windows.Forms.Button
        Me.trig_sin = New System.Windows.Forms.Button
        Me.trig_pow_ten = New System.Windows.Forms.Button
        Me.trig_clear = New System.Windows.Forms.Button
        Me.trig_natural_log = New System.Windows.Forms.Button
        Me.trig_log_base_ten = New System.Windows.Forms.Button
        Me.trig_arctan = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.trig_result = New System.Windows.Forms.TextBox
        Me.trig_first_no = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.Trig_groupbox.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SeriesToolStripMenuItem, Me.BackToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(520, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SeriesToolStripMenuItem
        '
        Me.SeriesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArcTangentToolStripMenuItem, Me.CosineToolStripMenuItem, Me.ExponentToolStripMenuItem, Me.NaturalLogarithmicToolStripMenuItem, Me.LogBaseTenToolStripMenuItem, Me.SineToolStripMenuItem, Me.PowerOfTenToolStripMenuItem, Me.OpposedAngleLengthToolStripMenuItem})
        Me.SeriesToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SeriesToolStripMenuItem.Name = "SeriesToolStripMenuItem"
        Me.SeriesToolStripMenuItem.Size = New System.Drawing.Size(78, 20)
        Me.SeriesToolStripMenuItem.Text = "&Functions"
        '
        'ArcTangentToolStripMenuItem
        '
        Me.ArcTangentToolStripMenuItem.Name = "ArcTangentToolStripMenuItem"
        Me.ArcTangentToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.ArcTangentToolStripMenuItem.Text = "Hypotenuse -> Angle"
        Me.ArcTangentToolStripMenuItem.ToolTipText = "Angle From Hypotenuse Opposed"
        '
        'CosineToolStripMenuItem
        '
        Me.CosineToolStripMenuItem.Name = "CosineToolStripMenuItem"
        Me.CosineToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.CosineToolStripMenuItem.Text = "Length -> Angle"
        Me.CosineToolStripMenuItem.ToolTipText = "Angle From Lengths"
        '
        'ExponentToolStripMenuItem
        '
        Me.ExponentToolStripMenuItem.Name = "ExponentToolStripMenuItem"
        Me.ExponentToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.ExponentToolStripMenuItem.Text = "Length Adj. Angle -> Hypotenuse"
        Me.ExponentToolStripMenuItem.ToolTipText = "Hypotenuse From Length Adjacente Angle"
        '
        'NaturalLogarithmicToolStripMenuItem
        '
        Me.NaturalLogarithmicToolStripMenuItem.Name = "NaturalLogarithmicToolStripMenuItem"
        Me.NaturalLogarithmicToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.NaturalLogarithmicToolStripMenuItem.Text = "Length Opposed Angle -> Hypotenuse"
        Me.NaturalLogarithmicToolStripMenuItem.ToolTipText = "Hypotenuse From Length Opposed Angle"
        '
        'LogBaseTenToolStripMenuItem
        '
        Me.LogBaseTenToolStripMenuItem.Name = "LogBaseTenToolStripMenuItem"
        Me.LogBaseTenToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.LogBaseTenToolStripMenuItem.Text = "Hypotenuse Agjacent Angle -> Length"
        Me.LogBaseTenToolStripMenuItem.ToolTipText = "Length From Hypotenuse Agjacent Angle"
        '
        'SineToolStripMenuItem
        '
        Me.SineToolStripMenuItem.Name = "SineToolStripMenuItem"
        Me.SineToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.SineToolStripMenuItem.Text = "Hypotenuse Opposed Angle -> Length"
        Me.SineToolStripMenuItem.ToolTipText = "Length From Hypotenuse Opposed Angle"
        '
        'PowerOfTenToolStripMenuItem
        '
        Me.PowerOfTenToolStripMenuItem.Name = "PowerOfTenToolStripMenuItem"
        Me.PowerOfTenToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.PowerOfTenToolStripMenuItem.Text = "Opposed Adjacent Angle -> Length"
        Me.PowerOfTenToolStripMenuItem.ToolTipText = "Length From Opposed Adjacente Angle"
        '
        'OpposedAngleLengthToolStripMenuItem
        '
        Me.OpposedAngleLengthToolStripMenuItem.Name = "OpposedAngleLengthToolStripMenuItem"
        Me.OpposedAngleLengthToolStripMenuItem.Size = New System.Drawing.Size(325, 22)
        Me.OpposedAngleLengthToolStripMenuItem.Text = "Opposed Angle -> Length"
        Me.OpposedAngleLengthToolStripMenuItem.ToolTipText = "Length From Opposed Angle"
        '
        'BackToolStripMenuItem
        '
        Me.BackToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TrignometricMenuToolStripMenuItem})
        Me.BackToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackToolStripMenuItem.Name = "BackToolStripMenuItem"
        Me.BackToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BackToolStripMenuItem.Text = "&Back"
        '
        'TrignometricMenuToolStripMenuItem
        '
        Me.TrignometricMenuToolStripMenuItem.Name = "TrignometricMenuToolStripMenuItem"
        Me.TrignometricMenuToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.TrignometricMenuToolStripMenuItem.Text = "Trignometric Menu"
        '
        'Trig_groupbox
        '
        Me.Trig_groupbox.Controls.Add(Me.Button1)
        Me.Trig_groupbox.Controls.Add(Me.Label4)
        Me.Trig_groupbox.Controls.Add(Me.trig_second_no)
        Me.Trig_groupbox.Controls.Add(Me.trig_exponent)
        Me.Trig_groupbox.Controls.Add(Me.trig_cos)
        Me.Trig_groupbox.Controls.Add(Me.trig_sin)
        Me.Trig_groupbox.Controls.Add(Me.trig_pow_ten)
        Me.Trig_groupbox.Controls.Add(Me.trig_clear)
        Me.Trig_groupbox.Controls.Add(Me.trig_natural_log)
        Me.Trig_groupbox.Controls.Add(Me.trig_log_base_ten)
        Me.Trig_groupbox.Controls.Add(Me.trig_arctan)
        Me.Trig_groupbox.Controls.Add(Me.Label2)
        Me.Trig_groupbox.Controls.Add(Me.Label1)
        Me.Trig_groupbox.Controls.Add(Me.trig_result)
        Me.Trig_groupbox.Controls.Add(Me.trig_first_no)
        Me.Trig_groupbox.Location = New System.Drawing.Point(12, 55)
        Me.Trig_groupbox.Name = "Trig_groupbox"
        Me.Trig_groupbox.Size = New System.Drawing.Size(496, 231)
        Me.Trig_groupbox.TabIndex = 1
        Me.Trig_groupbox.TabStop = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Window
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(318, 194)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(105, 32)
        Me.Button1.TabIndex = 15
        Me.Button1.Text = "Clear"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.Window
        Me.Label4.Location = New System.Drawing.Point(6, 94)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(143, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Enter The 2nd Number  "
        '
        'trig_second_no
        '
        Me.trig_second_no.Location = New System.Drawing.Point(152, 91)
        Me.trig_second_no.Name = "trig_second_no"
        Me.trig_second_no.Size = New System.Drawing.Size(93, 20)
        Me.trig_second_no.TabIndex = 1
        '
        'trig_exponent
        '
        Me.trig_exponent.BackColor = System.Drawing.SystemColors.Window
        Me.trig_exponent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.trig_exponent.Location = New System.Drawing.Point(259, 62)
        Me.trig_exponent.Name = "trig_exponent"
        Me.trig_exponent.Size = New System.Drawing.Size(105, 32)
        Me.trig_exponent.TabIndex = 4
        Me.trig_exponent.Text = "Length -> Angle"
        Me.trig_exponent.UseVisualStyleBackColor = False
        '
        'trig_cos
        '
        Me.trig_cos.BackColor = System.Drawing.SystemColors.Window
        Me.trig_cos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.trig_cos.Location = New System.Drawing.Point(378, 19)
        Me.trig_cos.Name = "trig_cos"
        Me.trig_cos.Size = New System.Drawing.Size(104, 32)
        Me.trig_cos.TabIndex = 7
        Me.trig_cos.Text = "HypoAdj -> Len"
        Me.trig_cos.UseVisualStyleBackColor = False
        '
        'trig_sin
        '
        Me.trig_sin.BackColor = System.Drawing.SystemColors.Window
        Me.trig_sin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.trig_sin.Location = New System.Drawing.Point(378, 62)
        Me.trig_sin.Name = "trig_sin"
        Me.trig_sin.Size = New System.Drawing.Size(104, 32)
        Me.trig_sin.TabIndex = 8
        Me.trig_sin.Text = "Hypopos-> Len"
        Me.trig_sin.UseVisualStyleBackColor = False
        '
        'trig_pow_ten
        '
        Me.trig_pow_ten.BackColor = System.Drawing.SystemColors.Window
        Me.trig_pow_ten.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.trig_pow_ten.Location = New System.Drawing.Point(380, 108)
        Me.trig_pow_ten.Name = "trig_pow_ten"
        Me.trig_pow_ten.Size = New System.Drawing.Size(102, 32)
        Me.trig_pow_ten.TabIndex = 9
        Me.trig_pow_ten.Text = "Adj Angle->Len"
        Me.trig_pow_ten.UseVisualStyleBackColor = False
        '
        'trig_clear
        '
        Me.trig_clear.BackColor = System.Drawing.SystemColors.Window
        Me.trig_clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.trig_clear.Location = New System.Drawing.Point(378, 156)
        Me.trig_clear.Name = "trig_clear"
        Me.trig_clear.Size = New System.Drawing.Size(103, 32)
        Me.trig_clear.TabIndex = 10
        Me.trig_clear.Text = "Oppose->Len"
        Me.trig_clear.UseVisualStyleBackColor = False
        '
        'trig_natural_log
        '
        Me.trig_natural_log.BackColor = System.Drawing.SystemColors.Window
        Me.trig_natural_log.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.trig_natural_log.Location = New System.Drawing.Point(259, 108)
        Me.trig_natural_log.Name = "trig_natural_log"
        Me.trig_natural_log.Size = New System.Drawing.Size(105, 32)
        Me.trig_natural_log.TabIndex = 5
        Me.trig_natural_log.Text = "Len Adj ->Hypo"
        Me.trig_natural_log.UseVisualStyleBackColor = False
        '
        'trig_log_base_ten
        '
        Me.trig_log_base_ten.BackColor = System.Drawing.SystemColors.Window
        Me.trig_log_base_ten.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.trig_log_base_ten.Location = New System.Drawing.Point(259, 156)
        Me.trig_log_base_ten.Name = "trig_log_base_ten"
        Me.trig_log_base_ten.Size = New System.Drawing.Size(105, 32)
        Me.trig_log_base_ten.TabIndex = 6
        Me.trig_log_base_ten.Text = "Oppose->Hypo"
        Me.trig_log_base_ten.UseVisualStyleBackColor = False
        '
        'trig_arctan
        '
        Me.trig_arctan.BackColor = System.Drawing.SystemColors.Window
        Me.trig_arctan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.trig_arctan.Location = New System.Drawing.Point(259, 19)
        Me.trig_arctan.Name = "trig_arctan"
        Me.trig_arctan.Size = New System.Drawing.Size(105, 32)
        Me.trig_arctan.TabIndex = 3
        Me.trig_arctan.Text = "Hypo -> Angle"
        Me.trig_arctan.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.Window
        Me.Label2.Location = New System.Drawing.Point(7, 136)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Result"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.Window
        Me.Label1.Location = New System.Drawing.Point(6, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(143, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Enter The 1st  Number  "
        '
        'trig_result
        '
        Me.trig_result.Enabled = False
        Me.trig_result.Location = New System.Drawing.Point(152, 132)
        Me.trig_result.Name = "trig_result"
        Me.trig_result.Size = New System.Drawing.Size(93, 20)
        Me.trig_result.TabIndex = 2
        '
        'trig_first_no
        '
        Me.trig_first_no.Location = New System.Drawing.Point(152, 44)
        Me.trig_first_no.Name = "trig_first_no"
        Me.trig_first_no.Size = New System.Drawing.Size(95, 20)
        Me.trig_first_no.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.Window
        Me.Label3.Location = New System.Drawing.Point(160, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(192, 20)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Trignometric Functions"
        '
        'NuGenTrigFunctions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(520, 298)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Trig_groupbox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "NuGenTrigFunctions"
        Me.Text = "NuGen Trig Functions"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Trig_groupbox.ResumeLayout(False)
        Me.Trig_groupbox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents SeriesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ArcTangentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CosineToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExponentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NaturalLogarithmicToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LogBaseTenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SineToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PowerOfTenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TrignometricMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Trig_groupbox As System.Windows.Forms.GroupBox
    Friend WithEvents trig_arctan As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents trig_result As System.Windows.Forms.TextBox
    Friend WithEvents trig_first_no As System.Windows.Forms.TextBox
    Friend WithEvents trig_exponent As System.Windows.Forms.Button
    Friend WithEvents trig_cos As System.Windows.Forms.Button
    Friend WithEvents trig_sin As System.Windows.Forms.Button
    Friend WithEvents trig_pow_ten As System.Windows.Forms.Button
    Friend WithEvents trig_clear As System.Windows.Forms.Button
    Friend WithEvents trig_natural_log As System.Windows.Forms.Button
    Friend WithEvents trig_log_base_ten As System.Windows.Forms.Button
    Friend WithEvents OpposedAngleLengthToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents trig_second_no As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class

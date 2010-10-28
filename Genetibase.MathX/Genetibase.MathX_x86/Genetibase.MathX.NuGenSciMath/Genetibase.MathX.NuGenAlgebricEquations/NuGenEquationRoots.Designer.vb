<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenEquationRoots
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
        Me.EquationRootsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AITKNToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ASITERToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BISECTIONToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NEXTROOTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.REGULAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RSYNDIVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SECANTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ZNEWTONToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BACKToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.alg_equ_regula = New System.Windows.Forms.Button
        Me.alg_euqrsyndiv = New System.Windows.Forms.Button
        Me.alg_equ_asiter = New System.Windows.Forms.Button
        Me.alg_equ_secant = New System.Windows.Forms.Button
        Me.alg_equ_znewton = New System.Windows.Forms.Button
        Me.alg_equ_bisect = New System.Windows.Forms.Button
        Me.alg_equ_clear = New System.Windows.Forms.Button
        Me.alg_equ_aitkn = New System.Windows.Forms.Button
        Me.alg_equ_second_no = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.alg_equ_third_no = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.alg_equ_result_no = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.alg_equ_fourth_no = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.alg_equ_first_no = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EquationRootsToolStripMenuItem, Me.BACKToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(519, 24)
        Me.MenuStrip1.TabIndex = 12
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'EquationRootsToolStripMenuItem
        '
        Me.EquationRootsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AITKNToolStripMenuItem, Me.ASITERToolStripMenuItem, Me.BISECTIONToolStripMenuItem, Me.NEXTROOTToolStripMenuItem, Me.REGULAToolStripMenuItem, Me.RSYNDIVToolStripMenuItem, Me.SECANTToolStripMenuItem, Me.ZNEWTONToolStripMenuItem})
        Me.EquationRootsToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EquationRootsToolStripMenuItem.Name = "EquationRootsToolStripMenuItem"
        Me.EquationRootsToolStripMenuItem.Size = New System.Drawing.Size(115, 20)
        Me.EquationRootsToolStripMenuItem.Text = "&Equation Roots"
        '
        'AITKNToolStripMenuItem
        '
        Me.AITKNToolStripMenuItem.Name = "AITKNToolStripMenuItem"
        Me.AITKNToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.AITKNToolStripMenuItem.Text = "AITKN"
        '
        'ASITERToolStripMenuItem
        '
        Me.ASITERToolStripMenuItem.Name = "ASITERToolStripMenuItem"
        Me.ASITERToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.ASITERToolStripMenuItem.Text = "ASITER"
        '
        'BISECTIONToolStripMenuItem
        '
        Me.BISECTIONToolStripMenuItem.Name = "BISECTIONToolStripMenuItem"
        Me.BISECTIONToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.BISECTIONToolStripMenuItem.Text = "BISECTION"
        '
        'NEXTROOTToolStripMenuItem
        '
        Me.NEXTROOTToolStripMenuItem.Name = "NEXTROOTToolStripMenuItem"
        Me.NEXTROOTToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.NEXTROOTToolStripMenuItem.Text = "NEXT ROOT"
        '
        'REGULAToolStripMenuItem
        '
        Me.REGULAToolStripMenuItem.Name = "REGULAToolStripMenuItem"
        Me.REGULAToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.REGULAToolStripMenuItem.Text = "REGULA"
        '
        'RSYNDIVToolStripMenuItem
        '
        Me.RSYNDIVToolStripMenuItem.Name = "RSYNDIVToolStripMenuItem"
        Me.RSYNDIVToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.RSYNDIVToolStripMenuItem.Text = "RSYNDIV"
        '
        'SECANTToolStripMenuItem
        '
        Me.SECANTToolStripMenuItem.Name = "SECANTToolStripMenuItem"
        Me.SECANTToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.SECANTToolStripMenuItem.Text = "SECANT"
        '
        'ZNEWTONToolStripMenuItem
        '
        Me.ZNEWTONToolStripMenuItem.Name = "ZNEWTONToolStripMenuItem"
        Me.ZNEWTONToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.ZNEWTONToolStripMenuItem.Text = "Z-NEWTON"
        '
        'BACKToolStripMenuItem
        '
        Me.BACKToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1})
        Me.BACKToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BACKToolStripMenuItem.Name = "BACKToolStripMenuItem"
        Me.BACKToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BACKToolStripMenuItem.Text = "&Back"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(232, 22)
        Me.ToolStripMenuItem1.Text = "Algebric Equation Menu"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.alg_equ_regula)
        Me.GroupBox1.Controls.Add(Me.alg_euqrsyndiv)
        Me.GroupBox1.Controls.Add(Me.alg_equ_asiter)
        Me.GroupBox1.Controls.Add(Me.alg_equ_secant)
        Me.GroupBox1.Controls.Add(Me.alg_equ_znewton)
        Me.GroupBox1.Controls.Add(Me.alg_equ_bisect)
        Me.GroupBox1.Controls.Add(Me.alg_equ_clear)
        Me.GroupBox1.Controls.Add(Me.alg_equ_aitkn)
        Me.GroupBox1.Controls.Add(Me.alg_equ_second_no)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.alg_equ_third_no)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.alg_equ_result_no)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.alg_equ_fourth_no)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.alg_equ_first_no)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 71)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(476, 174)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'alg_equ_regula
        '
        Me.alg_equ_regula.BackColor = System.Drawing.SystemColors.Window
        Me.alg_equ_regula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.alg_equ_regula.Location = New System.Drawing.Point(375, 19)
        Me.alg_equ_regula.Name = "alg_equ_regula"
        Me.alg_equ_regula.Size = New System.Drawing.Size(69, 27)
        Me.alg_equ_regula.TabIndex = 8
        Me.alg_equ_regula.Text = "REGULA"
        Me.alg_equ_regula.UseVisualStyleBackColor = False
        '
        'alg_euqrsyndiv
        '
        Me.alg_euqrsyndiv.BackColor = System.Drawing.SystemColors.Window
        Me.alg_euqrsyndiv.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.alg_euqrsyndiv.Location = New System.Drawing.Point(375, 52)
        Me.alg_euqrsyndiv.Name = "alg_euqrsyndiv"
        Me.alg_euqrsyndiv.Size = New System.Drawing.Size(69, 27)
        Me.alg_euqrsyndiv.TabIndex = 9
        Me.alg_euqrsyndiv.Text = "RSYN DIV"
        Me.alg_euqrsyndiv.UseVisualStyleBackColor = False
        '
        'alg_equ_asiter
        '
        Me.alg_equ_asiter.BackColor = System.Drawing.SystemColors.Window
        Me.alg_equ_asiter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.alg_equ_asiter.Location = New System.Drawing.Point(287, 52)
        Me.alg_equ_asiter.Name = "alg_equ_asiter"
        Me.alg_equ_asiter.Size = New System.Drawing.Size(69, 27)
        Me.alg_equ_asiter.TabIndex = 5
        Me.alg_equ_asiter.Text = "ASITER"
        Me.alg_equ_asiter.UseVisualStyleBackColor = False
        '
        'alg_equ_secant
        '
        Me.alg_equ_secant.BackColor = System.Drawing.SystemColors.Window
        Me.alg_equ_secant.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.alg_equ_secant.Location = New System.Drawing.Point(375, 85)
        Me.alg_equ_secant.Name = "alg_equ_secant"
        Me.alg_equ_secant.Size = New System.Drawing.Size(69, 27)
        Me.alg_equ_secant.TabIndex = 10
        Me.alg_equ_secant.Text = "SECANT"
        Me.alg_equ_secant.UseVisualStyleBackColor = False
        '
        'alg_equ_znewton
        '
        Me.alg_equ_znewton.BackColor = System.Drawing.SystemColors.Window
        Me.alg_equ_znewton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.alg_equ_znewton.Location = New System.Drawing.Point(375, 125)
        Me.alg_equ_znewton.Name = "alg_equ_znewton"
        Me.alg_equ_znewton.Size = New System.Drawing.Size(69, 27)
        Me.alg_equ_znewton.TabIndex = 11
        Me.alg_equ_znewton.Text = "Z-NEWTON"
        Me.alg_equ_znewton.UseVisualStyleBackColor = False
        '
        'alg_equ_bisect
        '
        Me.alg_equ_bisect.BackColor = System.Drawing.SystemColors.Window
        Me.alg_equ_bisect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.alg_equ_bisect.Location = New System.Drawing.Point(287, 85)
        Me.alg_equ_bisect.Name = "alg_equ_bisect"
        Me.alg_equ_bisect.Size = New System.Drawing.Size(69, 27)
        Me.alg_equ_bisect.TabIndex = 6
        Me.alg_equ_bisect.Text = "BISECT"
        Me.alg_equ_bisect.UseVisualStyleBackColor = False
        '
        'alg_equ_clear
        '
        Me.alg_equ_clear.BackColor = System.Drawing.SystemColors.Window
        Me.alg_equ_clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.alg_equ_clear.Location = New System.Drawing.Point(287, 125)
        Me.alg_equ_clear.Name = "alg_equ_clear"
        Me.alg_equ_clear.Size = New System.Drawing.Size(69, 27)
        Me.alg_equ_clear.TabIndex = 7
        Me.alg_equ_clear.Text = "CLEAR"
        Me.alg_equ_clear.UseVisualStyleBackColor = False
        '
        'alg_equ_aitkn
        '
        Me.alg_equ_aitkn.BackColor = System.Drawing.SystemColors.Window
        Me.alg_equ_aitkn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.alg_equ_aitkn.Location = New System.Drawing.Point(287, 19)
        Me.alg_equ_aitkn.Name = "alg_equ_aitkn"
        Me.alg_equ_aitkn.Size = New System.Drawing.Size(69, 27)
        Me.alg_equ_aitkn.TabIndex = 4
        Me.alg_equ_aitkn.Text = "AITKN"
        Me.alg_equ_aitkn.UseVisualStyleBackColor = False
        '
        'alg_equ_second_no
        '
        Me.alg_equ_second_no.Location = New System.Drawing.Point(155, 48)
        Me.alg_equ_second_no.Name = "alg_equ_second_no"
        Me.alg_equ_second_no.Size = New System.Drawing.Size(105, 20)
        Me.alg_equ_second_no.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.Window
        Me.Label5.Location = New System.Drawing.Point(17, 51)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(86, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Enter 2nd No."
        '
        'alg_equ_third_no
        '
        Me.alg_equ_third_no.Location = New System.Drawing.Point(155, 77)
        Me.alg_equ_third_no.Name = "alg_equ_third_no"
        Me.alg_equ_third_no.Size = New System.Drawing.Size(105, 20)
        Me.alg_equ_third_no.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.Window
        Me.Label4.Location = New System.Drawing.Point(17, 80)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(83, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Enter 3rd No."
        '
        'alg_equ_result_no
        '
        Me.alg_equ_result_no.Enabled = False
        Me.alg_equ_result_no.Location = New System.Drawing.Point(155, 129)
        Me.alg_equ_result_no.Name = "alg_equ_result_no"
        Me.alg_equ_result_no.Size = New System.Drawing.Size(105, 20)
        Me.alg_equ_result_no.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.Window
        Me.Label3.Location = New System.Drawing.Point(17, 132)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(36, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Rsult"
        '
        'alg_equ_fourth_no
        '
        Me.alg_equ_fourth_no.Location = New System.Drawing.Point(155, 103)
        Me.alg_equ_fourth_no.Name = "alg_equ_fourth_no"
        Me.alg_equ_fourth_no.Size = New System.Drawing.Size(105, 20)
        Me.alg_equ_fourth_no.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.Window
        Me.Label2.Location = New System.Drawing.Point(17, 106)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Enter 4th  No."
        '
        'alg_equ_first_no
        '
        Me.alg_equ_first_no.Location = New System.Drawing.Point(155, 22)
        Me.alg_equ_first_no.Name = "alg_equ_first_no"
        Me.alg_equ_first_no.Size = New System.Drawing.Size(105, 20)
        Me.alg_equ_first_no.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.Window
        Me.Label1.Location = New System.Drawing.Point(17, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Enter 1st No."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.Window
        Me.Label6.Location = New System.Drawing.Point(97, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(306, 20)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Bisection Method To Calculate Roots"
        '
        'NuGenEquationRoots
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(519, 266)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "NuGenEquationRoots"
        Me.Text = "NuGen Equation Roots"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents EquationRootsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AITKNToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ASITERToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BISECTIONToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NEXTROOTToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents REGULAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RSYNDIVToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SECANTToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ZNEWTONToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BACKToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents alg_equ_second_no As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents alg_equ_third_no As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents alg_equ_result_no As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents alg_equ_fourth_no As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents alg_equ_first_no As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents alg_equ_regula As System.Windows.Forms.Button
    Friend WithEvents alg_euqrsyndiv As System.Windows.Forms.Button
    Friend WithEvents alg_equ_asiter As System.Windows.Forms.Button
    Friend WithEvents alg_equ_secant As System.Windows.Forms.Button
    Friend WithEvents alg_equ_znewton As System.Windows.Forms.Button
    Friend WithEvents alg_equ_bisect As System.Windows.Forms.Button
    Friend WithEvents alg_equ_clear As System.Windows.Forms.Button
    Friend WithEvents alg_equ_aitkn As System.Windows.Forms.Button
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class

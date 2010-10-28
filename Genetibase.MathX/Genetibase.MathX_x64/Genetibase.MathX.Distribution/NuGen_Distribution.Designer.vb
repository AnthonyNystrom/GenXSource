<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGen_Distribution
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
        Me.DistributionFunctionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BETAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BinomialToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CauchyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExponentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PermiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GammaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LinearToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NormalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PossionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WeibullToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.dist_three_para = New System.Windows.Forms.GroupBox
        Me.dist_linear = New System.Windows.Forms.Button
        Me.dist_normal = New System.Windows.Forms.Button
        Me.dist_weibull = New System.Windows.Forms.Button
        Me.dist_clear = New System.Windows.Forms.Button
        Me.dist_fermi = New System.Windows.Forms.Button
        Me.dist_binomial = New System.Windows.Forms.Button
        Me.dist_beta = New System.Windows.Forms.Button
        Me.dist_second_no = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.dist_result = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.dist_first_no = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.dist_two_para = New System.Windows.Forms.GroupBox
        Me.dist_exp = New System.Windows.Forms.Button
        Me.dist_poisson = New System.Windows.Forms.Button
        Me.dist_gamma = New System.Windows.Forms.Button
        Me.dist_2_clear = New System.Windows.Forms.Button
        Me.dist_2_result = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.dist_2_first_no = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.dist_cauchy = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.dist_three_para.SuspendLayout()
        Me.dist_two_para.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DistributionFunctionsToolStripMenuItem, Me.BackToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(525, 24)
        Me.MenuStrip1.TabIndex = 9
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'DistributionFunctionsToolStripMenuItem
        '
        Me.DistributionFunctionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BETAToolStripMenuItem, Me.BinomialToolStripMenuItem, Me.CauchyToolStripMenuItem, Me.ExponentToolStripMenuItem, Me.PermiToolStripMenuItem, Me.GammaToolStripMenuItem, Me.LinearToolStripMenuItem, Me.NormalToolStripMenuItem, Me.PossionToolStripMenuItem, Me.WeibullToolStripMenuItem})
        Me.DistributionFunctionsToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DistributionFunctionsToolStripMenuItem.Name = "DistributionFunctionsToolStripMenuItem"
        Me.DistributionFunctionsToolStripMenuItem.Size = New System.Drawing.Size(155, 20)
        Me.DistributionFunctionsToolStripMenuItem.Text = "&Distribution Functions"
        '
        'BETAToolStripMenuItem
        '
        Me.BETAToolStripMenuItem.Name = "BETAToolStripMenuItem"
        Me.BETAToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.BETAToolStripMenuItem.Text = "Beta"
        '
        'BinomialToolStripMenuItem
        '
        Me.BinomialToolStripMenuItem.Name = "BinomialToolStripMenuItem"
        Me.BinomialToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.BinomialToolStripMenuItem.Text = "Binomial"
        '
        'CauchyToolStripMenuItem
        '
        Me.CauchyToolStripMenuItem.Name = "CauchyToolStripMenuItem"
        Me.CauchyToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.CauchyToolStripMenuItem.Text = "Cauchy"
        '
        'ExponentToolStripMenuItem
        '
        Me.ExponentToolStripMenuItem.Name = "ExponentToolStripMenuItem"
        Me.ExponentToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.ExponentToolStripMenuItem.Text = "Exponent"
        '
        'PermiToolStripMenuItem
        '
        Me.PermiToolStripMenuItem.Name = "PermiToolStripMenuItem"
        Me.PermiToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.PermiToolStripMenuItem.Text = "Permi"
        '
        'GammaToolStripMenuItem
        '
        Me.GammaToolStripMenuItem.Name = "GammaToolStripMenuItem"
        Me.GammaToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.GammaToolStripMenuItem.Text = "Gamma"
        '
        'LinearToolStripMenuItem
        '
        Me.LinearToolStripMenuItem.Name = "LinearToolStripMenuItem"
        Me.LinearToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.LinearToolStripMenuItem.Text = "Linear"
        '
        'NormalToolStripMenuItem
        '
        Me.NormalToolStripMenuItem.Name = "NormalToolStripMenuItem"
        Me.NormalToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.NormalToolStripMenuItem.Text = "Normal"
        '
        'PossionToolStripMenuItem
        '
        Me.PossionToolStripMenuItem.Name = "PossionToolStripMenuItem"
        Me.PossionToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.PossionToolStripMenuItem.Text = "Possion"
        '
        'WeibullToolStripMenuItem
        '
        Me.WeibullToolStripMenuItem.Name = "WeibullToolStripMenuItem"
        Me.WeibullToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.WeibullToolStripMenuItem.Text = "Weibull"
        '
        'BackToolStripMenuItem
        '
        Me.BackToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MainMenuToolStripMenuItem})
        Me.BackToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackToolStripMenuItem.Name = "BackToolStripMenuItem"
        Me.BackToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BackToolStripMenuItem.Text = "&Back"
        '
        'MainMenuToolStripMenuItem
        '
        Me.MainMenuToolStripMenuItem.Name = "MainMenuToolStripMenuItem"
        Me.MainMenuToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.MainMenuToolStripMenuItem.Text = "Main Menu"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'dist_three_para
        '
        Me.dist_three_para.Controls.Add(Me.dist_linear)
        Me.dist_three_para.Controls.Add(Me.dist_normal)
        Me.dist_three_para.Controls.Add(Me.dist_weibull)
        Me.dist_three_para.Controls.Add(Me.dist_clear)
        Me.dist_three_para.Controls.Add(Me.dist_fermi)
        Me.dist_three_para.Controls.Add(Me.dist_binomial)
        Me.dist_three_para.Controls.Add(Me.dist_beta)
        Me.dist_three_para.Controls.Add(Me.dist_second_no)
        Me.dist_three_para.Controls.Add(Me.Label4)
        Me.dist_three_para.Controls.Add(Me.dist_result)
        Me.dist_three_para.Controls.Add(Me.Label2)
        Me.dist_three_para.Controls.Add(Me.dist_first_no)
        Me.dist_three_para.Controls.Add(Me.Label1)
        Me.dist_three_para.Location = New System.Drawing.Point(12, 106)
        Me.dist_three_para.Name = "dist_three_para"
        Me.dist_three_para.Size = New System.Drawing.Size(489, 195)
        Me.dist_three_para.TabIndex = 1
        Me.dist_three_para.TabStop = False
        Me.dist_three_para.Visible = False
        '
        'dist_linear
        '
        Me.dist_linear.BackColor = System.Drawing.SystemColors.Window
        Me.dist_linear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_linear.Location = New System.Drawing.Point(325, 108)
        Me.dist_linear.Name = "dist_linear"
        Me.dist_linear.Size = New System.Drawing.Size(81, 36)
        Me.dist_linear.TabIndex = 7
        Me.dist_linear.Text = "LINEAR"
        Me.dist_linear.UseVisualStyleBackColor = False
        '
        'dist_normal
        '
        Me.dist_normal.BackColor = System.Drawing.SystemColors.Window
        Me.dist_normal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_normal.Location = New System.Drawing.Point(325, 19)
        Me.dist_normal.Name = "dist_normal"
        Me.dist_normal.Size = New System.Drawing.Size(79, 36)
        Me.dist_normal.TabIndex = 5
        Me.dist_normal.Text = "NORMAL"
        Me.dist_normal.UseVisualStyleBackColor = False
        '
        'dist_weibull
        '
        Me.dist_weibull.BackColor = System.Drawing.SystemColors.Window
        Me.dist_weibull.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_weibull.Location = New System.Drawing.Point(325, 65)
        Me.dist_weibull.Name = "dist_weibull"
        Me.dist_weibull.Size = New System.Drawing.Size(81, 36)
        Me.dist_weibull.TabIndex = 6
        Me.dist_weibull.Text = "WEIBULL"
        Me.dist_weibull.UseVisualStyleBackColor = False
        '
        'dist_clear
        '
        Me.dist_clear.BackColor = System.Drawing.SystemColors.Window
        Me.dist_clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_clear.Location = New System.Drawing.Point(284, 153)
        Me.dist_clear.Name = "dist_clear"
        Me.dist_clear.Size = New System.Drawing.Size(80, 36)
        Me.dist_clear.TabIndex = 8
        Me.dist_clear.Text = "CLEAR"
        Me.dist_clear.UseVisualStyleBackColor = False
        '
        'dist_fermi
        '
        Me.dist_fermi.BackColor = System.Drawing.SystemColors.Window
        Me.dist_fermi.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_fermi.Location = New System.Drawing.Point(222, 107)
        Me.dist_fermi.Name = "dist_fermi"
        Me.dist_fermi.Size = New System.Drawing.Size(81, 36)
        Me.dist_fermi.TabIndex = 4
        Me.dist_fermi.Text = "FERMI"
        Me.dist_fermi.UseVisualStyleBackColor = False
        '
        'dist_binomial
        '
        Me.dist_binomial.BackColor = System.Drawing.SystemColors.Window
        Me.dist_binomial.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_binomial.Location = New System.Drawing.Point(222, 65)
        Me.dist_binomial.Name = "dist_binomial"
        Me.dist_binomial.Size = New System.Drawing.Size(81, 36)
        Me.dist_binomial.TabIndex = 3
        Me.dist_binomial.Text = "BINOMIAL"
        Me.dist_binomial.UseVisualStyleBackColor = False
        '
        'dist_beta
        '
        Me.dist_beta.BackColor = System.Drawing.SystemColors.Window
        Me.dist_beta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_beta.Location = New System.Drawing.Point(222, 19)
        Me.dist_beta.Name = "dist_beta"
        Me.dist_beta.Size = New System.Drawing.Size(78, 36)
        Me.dist_beta.TabIndex = 2
        Me.dist_beta.Text = "BETA"
        Me.dist_beta.UseVisualStyleBackColor = False
        '
        'dist_second_no
        '
        Me.dist_second_no.Location = New System.Drawing.Point(103, 68)
        Me.dist_second_no.Name = "dist_second_no"
        Me.dist_second_no.Size = New System.Drawing.Size(89, 20)
        Me.dist_second_no.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.Window
        Me.Label4.Location = New System.Drawing.Point(15, 71)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Enter 2nd No."
        '
        'dist_result
        '
        Me.dist_result.Enabled = False
        Me.dist_result.Location = New System.Drawing.Point(103, 109)
        Me.dist_result.Name = "dist_result"
        Me.dist_result.Size = New System.Drawing.Size(89, 20)
        Me.dist_result.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.Window
        Me.Label2.Location = New System.Drawing.Point(15, 112)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Result"
        '
        'dist_first_no
        '
        Me.dist_first_no.Location = New System.Drawing.Point(103, 26)
        Me.dist_first_no.Name = "dist_first_no"
        Me.dist_first_no.Size = New System.Drawing.Size(89, 20)
        Me.dist_first_no.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.Window
        Me.Label1.Location = New System.Drawing.Point(15, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Enter 1st No."
        '
        'dist_two_para
        '
        Me.dist_two_para.Controls.Add(Me.dist_exp)
        Me.dist_two_para.Controls.Add(Me.dist_poisson)
        Me.dist_two_para.Controls.Add(Me.dist_gamma)
        Me.dist_two_para.Controls.Add(Me.dist_2_clear)
        Me.dist_two_para.Controls.Add(Me.dist_2_result)
        Me.dist_two_para.Controls.Add(Me.Label6)
        Me.dist_two_para.Controls.Add(Me.dist_2_first_no)
        Me.dist_two_para.Controls.Add(Me.Label5)
        Me.dist_two_para.Controls.Add(Me.dist_cauchy)
        Me.dist_two_para.Location = New System.Drawing.Point(30, 81)
        Me.dist_two_para.Name = "dist_two_para"
        Me.dist_two_para.Size = New System.Drawing.Size(435, 168)
        Me.dist_two_para.TabIndex = 2
        Me.dist_two_para.TabStop = False
        Me.dist_two_para.Visible = False
        '
        'dist_exp
        '
        Me.dist_exp.BackColor = System.Drawing.SystemColors.Window
        Me.dist_exp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_exp.Location = New System.Drawing.Point(332, 20)
        Me.dist_exp.Name = "dist_exp"
        Me.dist_exp.Size = New System.Drawing.Size(82, 34)
        Me.dist_exp.TabIndex = 3
        Me.dist_exp.Text = "EXPONENT"
        Me.dist_exp.UseVisualStyleBackColor = False
        '
        'dist_poisson
        '
        Me.dist_poisson.BackColor = System.Drawing.SystemColors.Window
        Me.dist_poisson.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_poisson.Location = New System.Drawing.Point(334, 67)
        Me.dist_poisson.Name = "dist_poisson"
        Me.dist_poisson.Size = New System.Drawing.Size(80, 34)
        Me.dist_poisson.TabIndex = 4
        Me.dist_poisson.Text = "POISSON"
        Me.dist_poisson.UseVisualStyleBackColor = False
        '
        'dist_gamma
        '
        Me.dist_gamma.BackColor = System.Drawing.SystemColors.Window
        Me.dist_gamma.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_gamma.Location = New System.Drawing.Point(231, 67)
        Me.dist_gamma.Name = "dist_gamma"
        Me.dist_gamma.Size = New System.Drawing.Size(76, 34)
        Me.dist_gamma.TabIndex = 2
        Me.dist_gamma.Text = "GAMMA"
        Me.dist_gamma.UseVisualStyleBackColor = False
        '
        'dist_2_clear
        '
        Me.dist_2_clear.BackColor = System.Drawing.SystemColors.Window
        Me.dist_2_clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_2_clear.Location = New System.Drawing.Point(283, 117)
        Me.dist_2_clear.Name = "dist_2_clear"
        Me.dist_2_clear.Size = New System.Drawing.Size(76, 34)
        Me.dist_2_clear.TabIndex = 5
        Me.dist_2_clear.Text = "CLEAR"
        Me.dist_2_clear.UseVisualStyleBackColor = False
        '
        'dist_2_result
        '
        Me.dist_2_result.Enabled = False
        Me.dist_2_result.Location = New System.Drawing.Point(113, 92)
        Me.dist_2_result.Name = "dist_2_result"
        Me.dist_2_result.Size = New System.Drawing.Size(92, 20)
        Me.dist_2_result.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.Window
        Me.Label6.Location = New System.Drawing.Point(21, 92)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Result"
        '
        'dist_2_first_no
        '
        Me.dist_2_first_no.Location = New System.Drawing.Point(113, 35)
        Me.dist_2_first_no.Name = "dist_2_first_no"
        Me.dist_2_first_no.Size = New System.Drawing.Size(92, 20)
        Me.dist_2_first_no.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.Window
        Me.Label5.Location = New System.Drawing.Point(21, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Enter 1st No."
        '
        'dist_cauchy
        '
        Me.dist_cauchy.BackColor = System.Drawing.SystemColors.Window
        Me.dist_cauchy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dist_cauchy.Location = New System.Drawing.Point(231, 20)
        Me.dist_cauchy.Name = "dist_cauchy"
        Me.dist_cauchy.Size = New System.Drawing.Size(76, 34)
        Me.dist_cauchy.TabIndex = 1
        Me.dist_cauchy.Text = "CAUCHY"
        Me.dist_cauchy.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.Window
        Me.Label3.Location = New System.Drawing.Point(148, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(185, 20)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Distribution Functions"
        '
        'NuGen_Distribution
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(525, 322)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.dist_two_para)
        Me.Controls.Add(Me.dist_three_para)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "NuGen_Distribution"
        Me.Text = "NuGen Distribution"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.dist_three_para.ResumeLayout(False)
        Me.dist_three_para.PerformLayout()
        Me.dist_two_para.ResumeLayout(False)
        Me.dist_two_para.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents DistributionFunctionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BETAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BinomialToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CauchyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExponentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PermiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GammaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LinearToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NormalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PossionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WeibullToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dist_three_para As System.Windows.Forms.GroupBox
    Friend WithEvents dist_normal As System.Windows.Forms.Button
    Friend WithEvents dist_weibull As System.Windows.Forms.Button
    Friend WithEvents dist_clear As System.Windows.Forms.Button
    Friend WithEvents dist_fermi As System.Windows.Forms.Button
    Friend WithEvents dist_binomial As System.Windows.Forms.Button
    Friend WithEvents dist_beta As System.Windows.Forms.Button
    Friend WithEvents dist_second_no As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dist_result As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dist_first_no As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dist_two_para As System.Windows.Forms.GroupBox
    Friend WithEvents dist_2_first_no As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents dist_cauchy As System.Windows.Forms.Button
    Friend WithEvents dist_2_result As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dist_exp As System.Windows.Forms.Button
    Friend WithEvents dist_poisson As System.Windows.Forms.Button
    Friend WithEvents dist_gamma As System.Windows.Forms.Button
    Friend WithEvents dist_2_clear As System.Windows.Forms.Button
    Friend WithEvents dist_linear As System.Windows.Forms.Button
    Friend WithEvents MainMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class

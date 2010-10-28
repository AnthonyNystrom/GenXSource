<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenMainMenu1
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
        Me.menuStrip1 = New System.Windows.Forms.MenuStrip
        Me.functionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mathematicalOperationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Button_next = New System.Windows.Forms.Button
        Me.option_probability = New System.Windows.Forms.RadioButton
        Me.option_distribution = New System.Windows.Forms.RadioButton
        Me.option_matrix = New System.Windows.Forms.RadioButton
        Me.option_linear = New System.Windows.Forms.RadioButton
        Me.option_curves = New System.Windows.Forms.RadioButton
        Me.option_algebric = New System.Windows.Forms.RadioButton
        Me.option_structures = New System.Windows.Forms.RadioButton
        Me.option_numeric = New System.Windows.Forms.RadioButton
        Me.option_trig = New System.Windows.Forms.RadioButton
        Me.option_statistics = New System.Windows.Forms.RadioButton
        Me.option_derivatives = New System.Windows.Forms.RadioButton
        Me.menuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'menuStrip1
        '
        Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.functionsToolStripMenuItem, Me.exitToolStripMenuItem})
        Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip1.Name = "menuStrip1"
        Me.menuStrip1.Size = New System.Drawing.Size(584, 24)
        Me.menuStrip1.TabIndex = 36
        Me.menuStrip1.Text = "menuStrip1"
        '
        'functionsToolStripMenuItem
        '
        Me.functionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mathematicalOperationsToolStripMenuItem})
        Me.functionsToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.functionsToolStripMenuItem.Name = "functionsToolStripMenuItem"
        Me.functionsToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.functionsToolStripMenuItem.Text = "&Back"
        '
        'mathematicalOperationsToolStripMenuItem
        '
        Me.mathematicalOperationsToolStripMenuItem.Name = "mathematicalOperationsToolStripMenuItem"
        Me.mathematicalOperationsToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.mathematicalOperationsToolStripMenuItem.Text = "Main Menu"
        '
        'exitToolStripMenuItem
        '
        Me.exitToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.exitToolStripMenuItem.Name = "exitToolStripMenuItem"
        Me.exitToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.exitToolStripMenuItem.Text = "Exit"
        '
        'Button_next
        '
        Me.Button_next.BackColor = System.Drawing.SystemColors.Window
        Me.Button_next.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_next.Location = New System.Drawing.Point(302, 285)
        Me.Button_next.Name = "Button_next"
        Me.Button_next.Size = New System.Drawing.Size(64, 28)
        Me.Button_next.TabIndex = 43
        Me.Button_next.Text = "Next"
        Me.Button_next.UseVisualStyleBackColor = False
        '
        'option_probability
        '
        Me.option_probability.AutoSize = True
        Me.option_probability.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_probability.ForeColor = System.Drawing.SystemColors.Window
        Me.option_probability.Location = New System.Drawing.Point(33, 155)
        Me.option_probability.Name = "option_probability"
        Me.option_probability.Size = New System.Drawing.Size(141, 19)
        Me.option_probability.TabIndex = 42
        Me.option_probability.TabStop = True
        Me.option_probability.Text = "NuGen Probability"
        Me.option_probability.UseVisualStyleBackColor = True
        '
        'option_distribution
        '
        Me.option_distribution.AutoSize = True
        Me.option_distribution.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_distribution.ForeColor = System.Drawing.SystemColors.Window
        Me.option_distribution.Location = New System.Drawing.Point(33, 113)
        Me.option_distribution.Name = "option_distribution"
        Me.option_distribution.Size = New System.Drawing.Size(147, 19)
        Me.option_distribution.TabIndex = 41
        Me.option_distribution.TabStop = True
        Me.option_distribution.Text = "NuGen Distribution"
        Me.option_distribution.UseVisualStyleBackColor = True
        '
        'option_matrix
        '
        Me.option_matrix.AutoSize = True
        Me.option_matrix.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_matrix.ForeColor = System.Drawing.SystemColors.Window
        Me.option_matrix.Location = New System.Drawing.Point(33, 69)
        Me.option_matrix.Name = "option_matrix"
        Me.option_matrix.Size = New System.Drawing.Size(227, 19)
        Me.option_matrix.TabIndex = 40
        Me.option_matrix.TabStop = True
        Me.option_matrix.Text = "NuGen Matrix Operations Menu" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.option_matrix.UseVisualStyleBackColor = True
        '
        'option_linear
        '
        Me.option_linear.AutoSize = True
        Me.option_linear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_linear.ForeColor = System.Drawing.SystemColors.Window
        Me.option_linear.Location = New System.Drawing.Point(291, 155)
        Me.option_linear.Name = "option_linear"
        Me.option_linear.Size = New System.Drawing.Size(191, 19)
        Me.option_linear.TabIndex = 46
        Me.option_linear.TabStop = True
        Me.option_linear.Text = "NuGen Linear Regression"
        Me.option_linear.UseVisualStyleBackColor = True
        '
        'option_curves
        '
        Me.option_curves.AutoSize = True
        Me.option_curves.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_curves.ForeColor = System.Drawing.SystemColors.Window
        Me.option_curves.Location = New System.Drawing.Point(291, 113)
        Me.option_curves.Name = "option_curves"
        Me.option_curves.Size = New System.Drawing.Size(230, 19)
        Me.option_curves.TabIndex = 45
        Me.option_curves.TabStop = True
        Me.option_curves.Text = "NuGen Curves And Co-ordinates"
        Me.option_curves.UseVisualStyleBackColor = True
        '
        'option_algebric
        '
        Me.option_algebric.AutoSize = True
        Me.option_algebric.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_algebric.ForeColor = System.Drawing.SystemColors.Window
        Me.option_algebric.Location = New System.Drawing.Point(291, 69)
        Me.option_algebric.Name = "option_algebric"
        Me.option_algebric.Size = New System.Drawing.Size(193, 19)
        Me.option_algebric.TabIndex = 44
        Me.option_algebric.TabStop = True
        Me.option_algebric.Text = "NuGen Algebric Equations"
        Me.option_algebric.UseVisualStyleBackColor = True
        '
        'option_structures
        '
        Me.option_structures.AutoSize = True
        Me.option_structures.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_structures.ForeColor = System.Drawing.SystemColors.Window
        Me.option_structures.Location = New System.Drawing.Point(291, 239)
        Me.option_structures.Name = "option_structures"
        Me.option_structures.Size = New System.Drawing.Size(138, 19)
        Me.option_structures.TabIndex = 51
        Me.option_structures.TabStop = True
        Me.option_structures.Text = "NuGen Structures"
        Me.option_structures.UseVisualStyleBackColor = True
        '
        'option_numeric
        '
        Me.option_numeric.AutoSize = True
        Me.option_numeric.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_numeric.ForeColor = System.Drawing.SystemColors.Window
        Me.option_numeric.Location = New System.Drawing.Point(291, 195)
        Me.option_numeric.Name = "option_numeric"
        Me.option_numeric.Size = New System.Drawing.Size(201, 19)
        Me.option_numeric.TabIndex = 50
        Me.option_numeric.TabStop = True
        Me.option_numeric.Text = "NuGen Numeric Operations"
        Me.option_numeric.UseVisualStyleBackColor = True
        '
        'option_trig
        '
        Me.option_trig.AutoSize = True
        Me.option_trig.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_trig.ForeColor = System.Drawing.SystemColors.Window
        Me.option_trig.Location = New System.Drawing.Point(33, 281)
        Me.option_trig.Name = "option_trig"
        Me.option_trig.Size = New System.Drawing.Size(228, 19)
        Me.option_trig.TabIndex = 49
        Me.option_trig.TabStop = True
        Me.option_trig.Text = "NuGen Trignometric Operations"
        Me.option_trig.UseVisualStyleBackColor = True
        '
        'option_statistics
        '
        Me.option_statistics.AutoSize = True
        Me.option_statistics.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_statistics.ForeColor = System.Drawing.SystemColors.Window
        Me.option_statistics.Location = New System.Drawing.Point(33, 239)
        Me.option_statistics.Name = "option_statistics"
        Me.option_statistics.Size = New System.Drawing.Size(131, 19)
        Me.option_statistics.TabIndex = 48
        Me.option_statistics.TabStop = True
        Me.option_statistics.Text = "NuGen Statistics"
        Me.option_statistics.UseVisualStyleBackColor = True
        '
        'option_derivatives
        '
        Me.option_derivatives.AutoSize = True
        Me.option_derivatives.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_derivatives.ForeColor = System.Drawing.SystemColors.Window
        Me.option_derivatives.Location = New System.Drawing.Point(33, 195)
        Me.option_derivatives.Name = "option_derivatives"
        Me.option_derivatives.Size = New System.Drawing.Size(143, 19)
        Me.option_derivatives.TabIndex = 47
        Me.option_derivatives.TabStop = True
        Me.option_derivatives.Text = "NuGen Derivatives"
        Me.option_derivatives.UseVisualStyleBackColor = True
        '
        'NuGenMainMenu1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(584, 325)
        Me.Controls.Add(Me.option_structures)
        Me.Controls.Add(Me.option_numeric)
        Me.Controls.Add(Me.option_trig)
        Me.Controls.Add(Me.option_statistics)
        Me.Controls.Add(Me.option_derivatives)
        Me.Controls.Add(Me.option_linear)
        Me.Controls.Add(Me.option_curves)
        Me.Controls.Add(Me.option_algebric)
        Me.Controls.Add(Me.Button_next)
        Me.Controls.Add(Me.option_probability)
        Me.Controls.Add(Me.option_distribution)
        Me.Controls.Add(Me.option_matrix)
        Me.Controls.Add(Me.menuStrip1)
        Me.Name = "NuGenMainMenu1"
        Me.Text = "NuGen Scientific Calculator"
        Me.menuStrip1.ResumeLayout(False)
        Me.menuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents menuStrip1 As System.Windows.Forms.MenuStrip
    Private WithEvents functionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mathematicalOperationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button_next As System.Windows.Forms.Button
    Private WithEvents option_probability As System.Windows.Forms.RadioButton
    Private WithEvents option_distribution As System.Windows.Forms.RadioButton
    Private WithEvents option_matrix As System.Windows.Forms.RadioButton
    Private WithEvents option_linear As System.Windows.Forms.RadioButton
    Private WithEvents option_curves As System.Windows.Forms.RadioButton
    Private WithEvents option_algebric As System.Windows.Forms.RadioButton
    Private WithEvents option_structures As System.Windows.Forms.RadioButton
    Private WithEvents option_numeric As System.Windows.Forms.RadioButton
    Private WithEvents option_trig As System.Windows.Forms.RadioButton
    Private WithEvents option_statistics As System.Windows.Forms.RadioButton
    Private WithEvents option_derivatives As System.Windows.Forms.RadioButton
End Class

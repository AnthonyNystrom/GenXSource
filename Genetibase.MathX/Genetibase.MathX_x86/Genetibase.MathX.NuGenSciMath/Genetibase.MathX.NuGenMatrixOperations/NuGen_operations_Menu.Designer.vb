<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGen_operations_Menu
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
        Me.option_gauss = New System.Windows.Forms.RadioButton
        Me.option_Vectors = New System.Windows.Forms.RadioButton
        Me.option_linear = New System.Windows.Forms.RadioButton
        Me.Button_next = New System.Windows.Forms.Button
        Me.option_matrix = New System.Windows.Forms.RadioButton
        Me.option_function = New System.Windows.Forms.RadioButton
        Me.SuspendLayout()
        '
        'option_gauss
        '
        Me.option_gauss.AutoSize = True
        Me.option_gauss.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_gauss.ForeColor = System.Drawing.SystemColors.WindowText
        Me.option_gauss.Location = New System.Drawing.Point(44, 82)
        Me.option_gauss.Name = "option_gauss"
        Me.option_gauss.Size = New System.Drawing.Size(113, 19)
        Me.option_gauss.TabIndex = 2
        Me.option_gauss.TabStop = True
        Me.option_gauss.Text = "NuGen Gauss"
        Me.option_gauss.UseVisualStyleBackColor = True
        '
        'option_Vectors
        '
        Me.option_Vectors.AutoSize = True
        Me.option_Vectors.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_Vectors.ForeColor = System.Drawing.SystemColors.WindowText
        Me.option_Vectors.Location = New System.Drawing.Point(44, 168)
        Me.option_Vectors.Name = "option_Vectors"
        Me.option_Vectors.Size = New System.Drawing.Size(120, 19)
        Me.option_Vectors.TabIndex = 4
        Me.option_Vectors.TabStop = True
        Me.option_Vectors.Text = "NuGen Vectors"
        Me.option_Vectors.UseVisualStyleBackColor = True
        '
        'option_linear
        '
        Me.option_linear.AutoSize = True
        Me.option_linear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_linear.ForeColor = System.Drawing.SystemColors.WindowText
        Me.option_linear.Location = New System.Drawing.Point(44, 126)
        Me.option_linear.Name = "option_linear"
        Me.option_linear.Size = New System.Drawing.Size(208, 19)
        Me.option_linear.TabIndex = 3
        Me.option_linear.TabStop = True
        Me.option_linear.Text = "NuGen Linear System Solver"
        Me.option_linear.UseVisualStyleBackColor = True
        '
        'Button_next
        '
        Me.Button_next.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.Button_next.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_next.ForeColor = System.Drawing.SystemColors.Window
        Me.Button_next.Location = New System.Drawing.Point(54, 257)
        Me.Button_next.Name = "Button_next"
        Me.Button_next.Size = New System.Drawing.Size(64, 28)
        Me.Button_next.TabIndex = 6
        Me.Button_next.Text = "Next"
        Me.Button_next.UseVisualStyleBackColor = False
        '
        'option_matrix
        '
        Me.option_matrix.AutoSize = True
        Me.option_matrix.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_matrix.ForeColor = System.Drawing.SystemColors.WindowText
        Me.option_matrix.Location = New System.Drawing.Point(44, 40)
        Me.option_matrix.Name = "option_matrix"
        Me.option_matrix.Size = New System.Drawing.Size(145, 19)
        Me.option_matrix.TabIndex = 1
        Me.option_matrix.TabStop = True
        Me.option_matrix.Text = "NuGen Matrix Calc"
        Me.option_matrix.UseVisualStyleBackColor = True
        '
        'option_function
        '
        Me.option_function.AutoSize = True
        Me.option_function.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_function.ForeColor = System.Drawing.SystemColors.WindowText
        Me.option_function.Location = New System.Drawing.Point(44, 213)
        Me.option_function.Name = "option_function"
        Me.option_function.Size = New System.Drawing.Size(224, 19)
        Me.option_function.TabIndex = 5
        Me.option_function.TabStop = True
        Me.option_function.Text = "NuGen Function Apporximation"
        Me.option_function.UseVisualStyleBackColor = True
        '
        'NuGen_operations_Menu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(346, 299)
        Me.Controls.Add(Me.option_function)
        Me.Controls.Add(Me.option_matrix)
        Me.Controls.Add(Me.Button_next)
        Me.Controls.Add(Me.option_Vectors)
        Me.Controls.Add(Me.option_linear)
        Me.Controls.Add(Me.option_gauss)
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(30, 30)
        Me.Name = "NuGen_operations_Menu"
        Me.Text = "NuGen Operations Menu"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents option_gauss As System.Windows.Forms.RadioButton
    Private WithEvents option_Vectors As System.Windows.Forms.RadioButton
    Private WithEvents option_linear As System.Windows.Forms.RadioButton
    Friend WithEvents Button_next As System.Windows.Forms.Button
    Private WithEvents option_matrix As System.Windows.Forms.RadioButton
    Private WithEvents option_function As System.Windows.Forms.RadioButton
End Class

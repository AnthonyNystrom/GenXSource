<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGen_Vector_operations
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.vector_a = New System.Windows.Forms.TableLayoutPanel
        Me.Text_a_3 = New System.Windows.Forms.TextBox
        Me.Text_a_2 = New System.Windows.Forms.TextBox
        Me.Text_a_1 = New System.Windows.Forms.TextBox
        Me.vector_b = New System.Windows.Forms.TableLayoutPanel
        Me.Text_b_3 = New System.Windows.Forms.TextBox
        Me.Text_b_2 = New System.Windows.Forms.TextBox
        Me.Text_b_1 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Vector_out = New System.Windows.Forms.TableLayoutPanel
        Me.Text_3 = New System.Windows.Forms.TextBox
        Me.Text_2 = New System.Windows.Forms.TextBox
        Me.Text_1 = New System.Windows.Forms.TextBox
        Me.Button_add = New System.Windows.Forms.Button
        Me.Button_ok = New System.Windows.Forms.Button
        Me.Button_sub = New System.Windows.Forms.Button
        Me.Button_cross = New System.Windows.Forms.Button
        Me.Button_dot = New System.Windows.Forms.Button
        Me.Button_angle = New System.Windows.Forms.Button
        Me.Button_clear = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.menuStrip1.SuspendLayout()
        Me.vector_a.SuspendLayout()
        Me.vector_b.SuspendLayout()
        Me.Vector_out.SuspendLayout()
        Me.SuspendLayout()
        '
        'menuStrip1
        '
        Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.functionsToolStripMenuItem})
        Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip1.Name = "menuStrip1"
        Me.menuStrip1.Size = New System.Drawing.Size(540, 24)
        Me.menuStrip1.TabIndex = 19
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
        Me.mathematicalOperationsToolStripMenuItem.Size = New System.Drawing.Size(228, 22)
        Me.mathematicalOperationsToolStripMenuItem.Text = "NuGen Operation Menu"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.Window
        Me.Label1.Location = New System.Drawing.Point(29, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 16)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Vector A:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.Window
        Me.Label2.Location = New System.Drawing.Point(29, 125)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 16)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Vector B:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.Window
        Me.Label3.Location = New System.Drawing.Point(400, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 16)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "OutPut Vector:"
        '
        'vector_a
        '
        Me.vector_a.ColumnCount = 3
        Me.vector_a.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.vector_a.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.vector_a.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.vector_a.Controls.Add(Me.Text_a_3, 2, 0)
        Me.vector_a.Controls.Add(Me.Text_a_2, 1, 0)
        Me.vector_a.Controls.Add(Me.Text_a_1, 0, 0)
        Me.vector_a.Location = New System.Drawing.Point(125, 65)
        Me.vector_a.Name = "vector_a"
        Me.vector_a.RowCount = 1
        Me.vector_a.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.vector_a.Size = New System.Drawing.Size(144, 30)
        Me.vector_a.TabIndex = 23
        '
        'Text_a_3
        '
        Me.Text_a_3.Location = New System.Drawing.Point(97, 3)
        Me.Text_a_3.Name = "Text_a_3"
        Me.Text_a_3.Size = New System.Drawing.Size(39, 20)
        Me.Text_a_3.TabIndex = 2
        '
        'Text_a_2
        '
        Me.Text_a_2.Location = New System.Drawing.Point(50, 3)
        Me.Text_a_2.Name = "Text_a_2"
        Me.Text_a_2.Size = New System.Drawing.Size(39, 20)
        Me.Text_a_2.TabIndex = 1
        '
        'Text_a_1
        '
        Me.Text_a_1.Location = New System.Drawing.Point(3, 3)
        Me.Text_a_1.Name = "Text_a_1"
        Me.Text_a_1.Size = New System.Drawing.Size(39, 20)
        Me.Text_a_1.TabIndex = 0
        '
        'vector_b
        '
        Me.vector_b.ColumnCount = 3
        Me.vector_b.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.vector_b.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.vector_b.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.vector_b.Controls.Add(Me.Text_b_3, 2, 0)
        Me.vector_b.Controls.Add(Me.Text_b_2, 1, 0)
        Me.vector_b.Controls.Add(Me.Text_b_1, 0, 0)
        Me.vector_b.Location = New System.Drawing.Point(125, 125)
        Me.vector_b.Name = "vector_b"
        Me.vector_b.RowCount = 1
        Me.vector_b.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.vector_b.Size = New System.Drawing.Size(144, 30)
        Me.vector_b.TabIndex = 24
        '
        'Text_b_3
        '
        Me.Text_b_3.Location = New System.Drawing.Point(97, 3)
        Me.Text_b_3.Name = "Text_b_3"
        Me.Text_b_3.Size = New System.Drawing.Size(39, 20)
        Me.Text_b_3.TabIndex = 5
        '
        'Text_b_2
        '
        Me.Text_b_2.Location = New System.Drawing.Point(50, 3)
        Me.Text_b_2.Name = "Text_b_2"
        Me.Text_b_2.Size = New System.Drawing.Size(39, 20)
        Me.Text_b_2.TabIndex = 4
        '
        'Text_b_1
        '
        Me.Text_b_1.Location = New System.Drawing.Point(3, 3)
        Me.Text_b_1.Name = "Text_b_1"
        Me.Text_b_1.Size = New System.Drawing.Size(39, 20)
        Me.Text_b_1.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.Window
        Me.Label4.Location = New System.Drawing.Point(314, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(22, 24)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "="
        '
        'Vector_out
        '
        Me.Vector_out.ColumnCount = 3
        Me.Vector_out.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.Vector_out.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.Vector_out.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.Vector_out.Controls.Add(Me.Text_3, 2, 0)
        Me.Vector_out.Controls.Add(Me.Text_2, 1, 0)
        Me.Vector_out.Controls.Add(Me.Text_1, 0, 0)
        Me.Vector_out.Enabled = False
        Me.Vector_out.Location = New System.Drawing.Point(384, 100)
        Me.Vector_out.Name = "Vector_out"
        Me.Vector_out.RowCount = 1
        Me.Vector_out.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.Vector_out.Size = New System.Drawing.Size(144, 30)
        Me.Vector_out.TabIndex = 26
        '
        'Text_3
        '
        Me.Text_3.Enabled = False
        Me.Text_3.Location = New System.Drawing.Point(97, 3)
        Me.Text_3.Name = "Text_3"
        Me.Text_3.Size = New System.Drawing.Size(39, 20)
        Me.Text_3.TabIndex = 2
        '
        'Text_2
        '
        Me.Text_2.Enabled = False
        Me.Text_2.Location = New System.Drawing.Point(50, 3)
        Me.Text_2.Name = "Text_2"
        Me.Text_2.Size = New System.Drawing.Size(39, 20)
        Me.Text_2.TabIndex = 1
        '
        'Text_1
        '
        Me.Text_1.Enabled = False
        Me.Text_1.Location = New System.Drawing.Point(3, 3)
        Me.Text_1.Name = "Text_1"
        Me.Text_1.Size = New System.Drawing.Size(39, 20)
        Me.Text_1.TabIndex = 0
        '
        'Button_add
        '
        Me.Button_add.BackColor = System.Drawing.SystemColors.Window
        Me.Button_add.Enabled = False
        Me.Button_add.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_add.Location = New System.Drawing.Point(112, 202)
        Me.Button_add.Name = "Button_add"
        Me.Button_add.Size = New System.Drawing.Size(37, 33)
        Me.Button_add.TabIndex = 7
        Me.Button_add.Text = "+"
        Me.Button_add.UseVisualStyleBackColor = False
        '
        'Button_ok
        '
        Me.Button_ok.BackColor = System.Drawing.SystemColors.Window
        Me.Button_ok.Location = New System.Drawing.Point(37, 202)
        Me.Button_ok.Name = "Button_ok"
        Me.Button_ok.Size = New System.Drawing.Size(37, 33)
        Me.Button_ok.TabIndex = 6
        Me.Button_ok.Text = "OK"
        Me.Button_ok.UseVisualStyleBackColor = False
        '
        'Button_sub
        '
        Me.Button_sub.BackColor = System.Drawing.SystemColors.Window
        Me.Button_sub.Enabled = False
        Me.Button_sub.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_sub.Location = New System.Drawing.Point(184, 202)
        Me.Button_sub.Name = "Button_sub"
        Me.Button_sub.Size = New System.Drawing.Size(37, 33)
        Me.Button_sub.TabIndex = 8
        Me.Button_sub.Text = "-"
        Me.Button_sub.UseVisualStyleBackColor = False
        '
        'Button_cross
        '
        Me.Button_cross.BackColor = System.Drawing.SystemColors.Window
        Me.Button_cross.Enabled = False
        Me.Button_cross.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_cross.Location = New System.Drawing.Point(257, 202)
        Me.Button_cross.Name = "Button_cross"
        Me.Button_cross.Size = New System.Drawing.Size(37, 33)
        Me.Button_cross.TabIndex = 9
        Me.Button_cross.Text = "x"
        Me.Button_cross.UseVisualStyleBackColor = False
        '
        'Button_dot
        '
        Me.Button_dot.BackColor = System.Drawing.SystemColors.Window
        Me.Button_dot.Enabled = False
        Me.Button_dot.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_dot.Location = New System.Drawing.Point(325, 201)
        Me.Button_dot.Name = "Button_dot"
        Me.Button_dot.Size = New System.Drawing.Size(37, 33)
        Me.Button_dot.TabIndex = 10
        Me.Button_dot.Text = "*"
        Me.Button_dot.UseVisualStyleBackColor = False
        '
        'Button_angle
        '
        Me.Button_angle.BackColor = System.Drawing.SystemColors.Window
        Me.Button_angle.Enabled = False
        Me.Button_angle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_angle.Location = New System.Drawing.Point(390, 201)
        Me.Button_angle.Name = "Button_angle"
        Me.Button_angle.Size = New System.Drawing.Size(44, 33)
        Me.Button_angle.TabIndex = 11
        Me.Button_angle.Text = "Ang"
        Me.Button_angle.UseVisualStyleBackColor = False
        '
        'Button_clear
        '
        Me.Button_clear.BackColor = System.Drawing.SystemColors.Window
        Me.Button_clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_clear.Location = New System.Drawing.Point(461, 202)
        Me.Button_clear.Name = "Button_clear"
        Me.Button_clear.Size = New System.Drawing.Size(59, 33)
        Me.Button_clear.TabIndex = 12
        Me.Button_clear.Text = "Clear"
        Me.Button_clear.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.Window
        Me.Label5.Location = New System.Drawing.Point(371, 106)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(0, 15)
        Me.Label5.TabIndex = 27
        Me.Label5.Visible = False
        '
        'NuGen_Vector_operations
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(540, 266)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button_clear)
        Me.Controls.Add(Me.Button_angle)
        Me.Controls.Add(Me.Button_dot)
        Me.Controls.Add(Me.Button_cross)
        Me.Controls.Add(Me.Button_sub)
        Me.Controls.Add(Me.Button_ok)
        Me.Controls.Add(Me.Button_add)
        Me.Controls.Add(Me.Vector_out)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.vector_b)
        Me.Controls.Add(Me.vector_a)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.menuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.Name = "NuGen_Vector_operations"
        Me.Text = "NuGen Vector Operations"
        Me.menuStrip1.ResumeLayout(False)
        Me.menuStrip1.PerformLayout()
        Me.vector_a.ResumeLayout(False)
        Me.vector_a.PerformLayout()
        Me.vector_b.ResumeLayout(False)
        Me.vector_b.PerformLayout()
        Me.Vector_out.ResumeLayout(False)
        Me.Vector_out.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents menuStrip1 As System.Windows.Forms.MenuStrip
    Private WithEvents functionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mathematicalOperationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents vector_a As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Text_a_3 As System.Windows.Forms.TextBox
    Friend WithEvents Text_a_2 As System.Windows.Forms.TextBox
    Friend WithEvents Text_a_1 As System.Windows.Forms.TextBox
    Friend WithEvents vector_b As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Text_b_3 As System.Windows.Forms.TextBox
    Friend WithEvents Text_b_2 As System.Windows.Forms.TextBox
    Friend WithEvents Text_b_1 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Vector_out As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Text_3 As System.Windows.Forms.TextBox
    Friend WithEvents Text_2 As System.Windows.Forms.TextBox
    Friend WithEvents Text_1 As System.Windows.Forms.TextBox
    Friend WithEvents Button_add As System.Windows.Forms.Button
    Friend WithEvents Button_ok As System.Windows.Forms.Button
    Friend WithEvents Button_sub As System.Windows.Forms.Button
    Friend WithEvents Button_cross As System.Windows.Forms.Button
    Friend WithEvents Button_dot As System.Windows.Forms.Button
    Friend WithEvents Button_angle As System.Windows.Forms.Button
    Friend WithEvents Button_clear As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class

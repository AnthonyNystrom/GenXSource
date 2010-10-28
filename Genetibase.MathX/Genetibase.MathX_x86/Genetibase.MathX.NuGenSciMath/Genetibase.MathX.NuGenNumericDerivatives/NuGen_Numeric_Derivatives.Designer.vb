<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGen_Numeric_Derivatives
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
        Me.Button_steepda = New System.Windows.Forms.Button
        Me.Button_steepds = New System.Windows.Forms.Button
        Me.text_l = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.text_k = New System.Windows.Forms.TextBox
        Me.text_e = New System.Windows.Forms.TextBox
        Me.text_x = New System.Windows.Forms.TextBox
        Me.text_res = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.menuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'menuStrip1
        '
        Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.functionsToolStripMenuItem, Me.exitToolStripMenuItem})
        Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip1.Name = "menuStrip1"
        Me.menuStrip1.Size = New System.Drawing.Size(442, 24)
        Me.menuStrip1.TabIndex = 18
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
        'Button_steepda
        '
        Me.Button_steepda.BackColor = System.Drawing.SystemColors.Window
        Me.Button_steepda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_steepda.Location = New System.Drawing.Point(15, 283)
        Me.Button_steepda.Name = "Button_steepda"
        Me.Button_steepda.Size = New System.Drawing.Size(65, 33)
        Me.Button_steepda.TabIndex = 4
        Me.Button_steepda.Text = "STEEPDA"
        Me.Button_steepda.UseVisualStyleBackColor = False
        '
        'Button_steepds
        '
        Me.Button_steepds.BackColor = System.Drawing.SystemColors.Window
        Me.Button_steepds.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_steepds.Location = New System.Drawing.Point(115, 283)
        Me.Button_steepds.Name = "Button_steepds"
        Me.Button_steepds.Size = New System.Drawing.Size(65, 33)
        Me.Button_steepds.TabIndex = 5
        Me.Button_steepds.Text = "STEEPDS"
        Me.Button_steepds.UseVisualStyleBackColor = False
        '
        'text_l
        '
        Me.text_l.Location = New System.Drawing.Point(199, 69)
        Me.text_l.Name = "text_l"
        Me.text_l.Size = New System.Drawing.Size(120, 20)
        Me.text_l.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label1.Location = New System.Drawing.Point(12, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 18)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Value for L :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label2.Location = New System.Drawing.Point(12, 96)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(168, 13)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "(No of Dimentions of the function.)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label3.Location = New System.Drawing.Point(12, 126)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 18)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Multiplier  K :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label4.Location = New System.Drawing.Point(9, 173)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(109, 18)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "Error Limit E:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label5.Location = New System.Drawing.Point(9, 220)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(166, 18)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "Initial Value For X(I) :"
        '
        'text_k
        '
        Me.text_k.Location = New System.Drawing.Point(199, 124)
        Me.text_k.Name = "text_k"
        Me.text_k.Size = New System.Drawing.Size(120, 20)
        Me.text_k.TabIndex = 1
        '
        'text_e
        '
        Me.text_e.Location = New System.Drawing.Point(199, 173)
        Me.text_e.Name = "text_e"
        Me.text_e.Size = New System.Drawing.Size(120, 20)
        Me.text_e.TabIndex = 2
        '
        'text_x
        '
        Me.text_x.Location = New System.Drawing.Point(199, 221)
        Me.text_x.Name = "text_x"
        Me.text_x.Size = New System.Drawing.Size(120, 20)
        Me.text_x.TabIndex = 3
        '
        'text_res
        '
        Me.text_res.Location = New System.Drawing.Point(343, 290)
        Me.text_res.Name = "text_res"
        Me.text_res.Size = New System.Drawing.Size(87, 20)
        Me.text_res.TabIndex = 6
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label6.Location = New System.Drawing.Point(269, 290)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 18)
        Me.Label6.TabIndex = 31
        Me.Label6.Text = "Result:"
        '
        'NuGen_Numeric_Derivatives
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(442, 328)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.text_res)
        Me.Controls.Add(Me.text_x)
        Me.Controls.Add(Me.text_e)
        Me.Controls.Add(Me.text_k)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.text_l)
        Me.Controls.Add(Me.Button_steepds)
        Me.Controls.Add(Me.Button_steepda)
        Me.Controls.Add(Me.menuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.Name = "NuGen_Numeric_Derivatives"
        Me.Text = "NuGen_Numeric_Derivatives"
        Me.menuStrip1.ResumeLayout(False)
        Me.menuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents menuStrip1 As System.Windows.Forms.MenuStrip
    Private WithEvents functionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents mathematicalOperationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button_steepda As System.Windows.Forms.Button
    Friend WithEvents Button_steepds As System.Windows.Forms.Button
    Friend WithEvents text_l As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents text_k As System.Windows.Forms.TextBox
    Friend WithEvents text_e As System.Windows.Forms.TextBox
    Friend WithEvents text_x As System.Windows.Forms.TextBox
    Friend WithEvents text_res As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class

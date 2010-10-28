<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenNewtoneInterpolationForm
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
        Me.BackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InterpolationLagrangeMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.MainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.clear = New System.Windows.Forms.Button
        Me.y_1 = New System.Windows.Forms.TextBox
        Me.y_2 = New System.Windows.Forms.TextBox
        Me.x_2 = New System.Windows.Forms.TextBox
        Me.y_3 = New System.Windows.Forms.TextBox
        Me.x_3 = New System.Windows.Forms.TextBox
        Me.x_4 = New System.Windows.Forms.TextBox
        Me.y_4 = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.result = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.x_1 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.index_val = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.X2_val = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Result_show = New System.Windows.Forms.Button
        Me.x1_val = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(510, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'BackToolStripMenuItem
        '
        Me.BackToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InterpolationLagrangeMenuToolStripMenuItem, Me.ToolStripMenuItem1, Me.MainMenuToolStripMenuItem})
        Me.BackToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackToolStripMenuItem.Name = "BackToolStripMenuItem"
        Me.BackToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BackToolStripMenuItem.Text = "&Back"
        '
        'InterpolationLagrangeMenuToolStripMenuItem
        '
        Me.InterpolationLagrangeMenuToolStripMenuItem.Name = "InterpolationLagrangeMenuToolStripMenuItem"
        Me.InterpolationLagrangeMenuToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.InterpolationLagrangeMenuToolStripMenuItem.Text = "Interpolation Lagrange Menu"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(271, 22)
        Me.ToolStripMenuItem1.Text = "Curves And Co-ordinate Menu"
        '
        'MainMenuToolStripMenuItem
        '
        Me.MainMenuToolStripMenuItem.Name = "MainMenuToolStripMenuItem"
        Me.MainMenuToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.MainMenuToolStripMenuItem.Text = "Main Menu"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.clear)
        Me.GroupBox1.Controls.Add(Me.y_1)
        Me.GroupBox1.Controls.Add(Me.y_2)
        Me.GroupBox1.Controls.Add(Me.x_2)
        Me.GroupBox1.Controls.Add(Me.y_3)
        Me.GroupBox1.Controls.Add(Me.x_3)
        Me.GroupBox1.Controls.Add(Me.x_4)
        Me.GroupBox1.Controls.Add(Me.y_4)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.result)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.x_1)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.index_val)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.X2_val)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Result_show)
        Me.GroupBox1.Controls.Add(Me.x1_val)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 52)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(480, 250)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'clear
        '
        Me.clear.BackColor = System.Drawing.SystemColors.Window
        Me.clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clear.Location = New System.Drawing.Point(363, 109)
        Me.clear.Name = "clear"
        Me.clear.Size = New System.Drawing.Size(84, 29)
        Me.clear.TabIndex = 14
        Me.clear.Text = "Clear"
        Me.clear.UseVisualStyleBackColor = False
        '
        'y_1
        '
        Me.y_1.Location = New System.Drawing.Point(160, 147)
        Me.y_1.Name = "y_1"
        Me.y_1.Size = New System.Drawing.Size(34, 20)
        Me.y_1.TabIndex = 8
        '
        'y_2
        '
        Me.y_2.Location = New System.Drawing.Point(200, 147)
        Me.y_2.Name = "y_2"
        Me.y_2.Size = New System.Drawing.Size(34, 20)
        Me.y_2.TabIndex = 9
        '
        'x_2
        '
        Me.x_2.Location = New System.Drawing.Point(200, 117)
        Me.x_2.Name = "x_2"
        Me.x_2.Size = New System.Drawing.Size(34, 20)
        Me.x_2.TabIndex = 5
        '
        'y_3
        '
        Me.y_3.Location = New System.Drawing.Point(240, 147)
        Me.y_3.Name = "y_3"
        Me.y_3.Size = New System.Drawing.Size(34, 20)
        Me.y_3.TabIndex = 10
        '
        'x_3
        '
        Me.x_3.Location = New System.Drawing.Point(240, 117)
        Me.x_3.Name = "x_3"
        Me.x_3.Size = New System.Drawing.Size(34, 20)
        Me.x_3.TabIndex = 6
        '
        'x_4
        '
        Me.x_4.Location = New System.Drawing.Point(280, 117)
        Me.x_4.Name = "x_4"
        Me.x_4.Size = New System.Drawing.Size(34, 20)
        Me.x_4.TabIndex = 7
        '
        'y_4
        '
        Me.y_4.Location = New System.Drawing.Point(280, 147)
        Me.y_4.Name = "y_4"
        Me.y_4.Size = New System.Drawing.Size(34, 20)
        Me.y_4.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.Window
        Me.Label6.Location = New System.Drawing.Point(14, 150)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(140, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Enter Value For Y Array"
        '
        'result
        '
        Me.result.Enabled = False
        Me.result.Location = New System.Drawing.Point(159, 188)
        Me.result.Name = "result"
        Me.result.Size = New System.Drawing.Size(72, 20)
        Me.result.TabIndex = 12
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.Window
        Me.Label5.Location = New System.Drawing.Point(14, 188)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Result  "
        '
        'x_1
        '
        Me.x_1.Location = New System.Drawing.Point(159, 117)
        Me.x_1.Name = "x_1"
        Me.x_1.Size = New System.Drawing.Size(34, 20)
        Me.x_1.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.Window
        Me.Label4.Location = New System.Drawing.Point(15, 117)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(140, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Enter Value For X Array"
        '
        'index_val
        '
        Me.index_val.Location = New System.Drawing.Point(159, 84)
        Me.index_val.Name = "index_val"
        Me.index_val.Size = New System.Drawing.Size(155, 20)
        Me.index_val.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.Window
        Me.Label3.Location = New System.Drawing.Point(15, 84)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(130, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Enter Value For Index"
        '
        'X2_val
        '
        Me.X2_val.Location = New System.Drawing.Point(159, 55)
        Me.X2_val.Name = "X2_val"
        Me.X2_val.Size = New System.Drawing.Size(156, 20)
        Me.X2_val.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.Window
        Me.Label2.Location = New System.Drawing.Point(15, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Enter Value For YT"
        '
        'Result_show
        '
        Me.Result_show.BackColor = System.Drawing.SystemColors.Window
        Me.Result_show.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Result_show.Location = New System.Drawing.Point(363, 58)
        Me.Result_show.Name = "Result_show"
        Me.Result_show.Size = New System.Drawing.Size(84, 29)
        Me.Result_show.TabIndex = 13
        Me.Result_show.Text = "Calculate"
        Me.Result_show.UseVisualStyleBackColor = False
        '
        'x1_val
        '
        Me.x1_val.Location = New System.Drawing.Point(159, 22)
        Me.x1_val.Name = "x1_val"
        Me.x1_val.Size = New System.Drawing.Size(157, 20)
        Me.x1_val.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.Window
        Me.Label1.Location = New System.Drawing.Point(15, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Enter Value For XT"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.Window
        Me.Label7.Location = New System.Drawing.Point(124, 29)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(252, 20)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Newton InterPolation Function"
        '
        'NuGenNewtoneInterpolationForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(510, 314)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.Name = "NuGenNewtoneInterpolationForm"
        Me.Text = "NuGen Newtone Interpolation"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InterpolationLagrangeMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents clear As System.Windows.Forms.Button
    Friend WithEvents y_1 As System.Windows.Forms.TextBox
    Friend WithEvents y_2 As System.Windows.Forms.TextBox
    Friend WithEvents x_2 As System.Windows.Forms.TextBox
    Friend WithEvents y_3 As System.Windows.Forms.TextBox
    Friend WithEvents x_3 As System.Windows.Forms.TextBox
    Friend WithEvents x_4 As System.Windows.Forms.TextBox
    Friend WithEvents y_4 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents result As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents x_1 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents index_val As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents X2_val As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Result_show As System.Windows.Forms.Button
    Friend WithEvents x1_val As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
End Class

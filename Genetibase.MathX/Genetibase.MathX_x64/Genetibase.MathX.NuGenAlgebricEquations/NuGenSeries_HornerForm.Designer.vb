<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenSeries_HornerForm
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
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.b2 = New System.Windows.Forms.TextBox
        Me.b3 = New System.Windows.Forms.TextBox
        Me.b4 = New System.Windows.Forms.TextBox
        Me.b1 = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.a2 = New System.Windows.Forms.TextBox
        Me.a3 = New System.Windows.Forms.TextBox
        Me.a4 = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.x_val = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.clear = New System.Windows.Forms.Button
        Me.horner = New System.Windows.Forms.Button
        Me.a1 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.BToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SeriesMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AlgebricToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label14 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.b2)
        Me.GroupBox1.Controls.Add(Me.b3)
        Me.GroupBox1.Controls.Add(Me.b4)
        Me.GroupBox1.Controls.Add(Me.b1)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.a2)
        Me.GroupBox1.Controls.Add(Me.a3)
        Me.GroupBox1.Controls.Add(Me.a4)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.x_val)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.clear)
        Me.GroupBox1.Controls.Add(Me.horner)
        Me.GroupBox1.Controls.Add(Me.a1)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 49)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(679, 206)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.Window
        Me.Label9.Location = New System.Drawing.Point(464, 75)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(30, 13)
        Me.Label9.TabIndex = 29
        Me.Label9.Text = "B(2)"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.Window
        Me.Label10.Location = New System.Drawing.Point(464, 98)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(30, 13)
        Me.Label10.TabIndex = 28
        Me.Label10.Text = "B(3)"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.Window
        Me.Label11.Location = New System.Drawing.Point(464, 124)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(30, 13)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "B(4)"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.Window
        Me.Label12.Location = New System.Drawing.Point(464, 49)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(30, 13)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "B(1)"
        '
        'b2
        '
        Me.b2.Enabled = False
        Me.b2.Location = New System.Drawing.Point(524, 72)
        Me.b2.Name = "b2"
        Me.b2.Size = New System.Drawing.Size(104, 20)
        Me.b2.TabIndex = 8
        '
        'b3
        '
        Me.b3.Enabled = False
        Me.b3.Location = New System.Drawing.Point(523, 98)
        Me.b3.Name = "b3"
        Me.b3.Size = New System.Drawing.Size(104, 20)
        Me.b3.TabIndex = 9
        '
        'b4
        '
        Me.b4.Enabled = False
        Me.b4.Location = New System.Drawing.Point(523, 124)
        Me.b4.Name = "b4"
        Me.b4.Size = New System.Drawing.Size(104, 20)
        Me.b4.TabIndex = 10
        '
        'b1
        '
        Me.b1.Enabled = False
        Me.b1.Location = New System.Drawing.Point(524, 46)
        Me.b1.Name = "b1"
        Me.b1.Size = New System.Drawing.Size(104, 20)
        Me.b1.TabIndex = 7
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.Window
        Me.Label8.Location = New System.Drawing.Point(123, 72)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(30, 13)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "A(2)"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.Window
        Me.Label7.Location = New System.Drawing.Point(123, 95)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(30, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "A(3)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.Window
        Me.Label4.Location = New System.Drawing.Point(123, 121)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 13)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "A(4)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.Window
        Me.Label3.Location = New System.Drawing.Point(123, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 13)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "A(1)"
        '
        'a2
        '
        Me.a2.Location = New System.Drawing.Point(183, 69)
        Me.a2.Name = "a2"
        Me.a2.Size = New System.Drawing.Size(104, 20)
        Me.a2.TabIndex = 1
        '
        'a3
        '
        Me.a3.Location = New System.Drawing.Point(182, 95)
        Me.a3.Name = "a3"
        Me.a3.Size = New System.Drawing.Size(104, 20)
        Me.a3.TabIndex = 2
        '
        'a4
        '
        Me.a4.Location = New System.Drawing.Point(182, 121)
        Me.a4.Name = "a4"
        Me.a4.Size = New System.Drawing.Size(104, 20)
        Me.a4.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.Window
        Me.Label5.Location = New System.Drawing.Point(12, 157)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(159, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Enter Expansion Point (X0)"
        '
        'x_val
        '
        Me.x_val.Location = New System.Drawing.Point(185, 155)
        Me.x_val.Name = "x_val"
        Me.x_val.Size = New System.Drawing.Size(104, 20)
        Me.x_val.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.Window
        Me.Label2.Location = New System.Drawing.Point(413, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Result Series"
        '
        'clear
        '
        Me.clear.BackColor = System.Drawing.SystemColors.Window
        Me.clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clear.Location = New System.Drawing.Point(330, 113)
        Me.clear.Name = "clear"
        Me.clear.Size = New System.Drawing.Size(79, 36)
        Me.clear.TabIndex = 6
        Me.clear.Text = "Clear"
        Me.clear.UseVisualStyleBackColor = False
        '
        'horner
        '
        Me.horner.BackColor = System.Drawing.SystemColors.Window
        Me.horner.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.horner.Location = New System.Drawing.Point(330, 50)
        Me.horner.Name = "horner"
        Me.horner.Size = New System.Drawing.Size(79, 36)
        Me.horner.TabIndex = 5
        Me.horner.Text = "Horner's  Series"
        Me.horner.UseVisualStyleBackColor = False
        '
        'a1
        '
        Me.a1.Location = New System.Drawing.Point(183, 43)
        Me.a1.Name = "a1"
        Me.a1.Size = New System.Drawing.Size(104, 20)
        Me.a1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.Window
        Me.Label1.Location = New System.Drawing.Point(12, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(141, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Enter Coefficient Series"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(702, 24)
        Me.MenuStrip1.TabIndex = 11
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
        Me.SeriesMenuToolStripMenuItem.Size = New System.Drawing.Size(232, 22)
        Me.SeriesMenuToolStripMenuItem.Text = "Series Menu"
        '
        'AlgebricToolStripMenuItem
        '
        Me.AlgebricToolStripMenuItem.Name = "AlgebricToolStripMenuItem"
        Me.AlgebricToolStripMenuItem.Size = New System.Drawing.Size(232, 22)
        Me.AlgebricToolStripMenuItem.Text = "Algebric Equation Menu"
        '
        'MainMenuToolStripMenuItem
        '
        Me.MainMenuToolStripMenuItem.Name = "MainMenuToolStripMenuItem"
        Me.MainMenuToolStripMenuItem.Size = New System.Drawing.Size(232, 22)
        Me.MainMenuToolStripMenuItem.Text = "Main Menu"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.Window
        Me.Label14.Location = New System.Drawing.Point(243, 26)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(200, 20)
        Me.Label14.TabIndex = 12
        Me.Label14.Text = "Horner's Shifting Series"
        '
        'NuGenSeries_HornerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(702, 267)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Name = "NuGenSeries_HornerForm"
        Me.Text = "NuGen Series Horner"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents x_val As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents clear As System.Windows.Forms.Button
    Friend WithEvents horner As System.Windows.Forms.Button
    Friend WithEvents a1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents BToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SeriesMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AlgebricToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents a2 As System.Windows.Forms.TextBox
    Friend WithEvents a3 As System.Windows.Forms.TextBox
    Friend WithEvents a4 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents b2 As System.Windows.Forms.TextBox
    Friend WithEvents b3 As System.Windows.Forms.TextBox
    Friend WithEvents b4 As System.Windows.Forms.TextBox
    Friend WithEvents b1 As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
End Class

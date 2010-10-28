<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGen_Numeric_menu
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
        Me.MainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.option_complex = New System.Windows.Forms.RadioButton
        Me.option_log = New System.Windows.Forms.RadioButton
        Me.option_binary = New System.Windows.Forms.RadioButton
        Me.option_roots = New System.Windows.Forms.RadioButton
        Me.button_ok = New System.Windows.Forms.Button
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(393, 24)
        Me.MenuStrip1.TabIndex = 11
        Me.MenuStrip1.Text = "MenuStrip1"
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
        'option_complex
        '
        Me.option_complex.AutoSize = True
        Me.option_complex.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_complex.ForeColor = System.Drawing.SystemColors.InactiveCaption
        Me.option_complex.Location = New System.Drawing.Point(30, 131)
        Me.option_complex.Name = "option_complex"
        Me.option_complex.Size = New System.Drawing.Size(129, 19)
        Me.option_complex.TabIndex = 3
        Me.option_complex.TabStop = True
        Me.option_complex.Text = "NuGen Complex"
        Me.option_complex.UseVisualStyleBackColor = True
        '
        'option_log
        '
        Me.option_log.AutoSize = True
        Me.option_log.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_log.ForeColor = System.Drawing.SystemColors.InactiveCaption
        Me.option_log.Location = New System.Drawing.Point(30, 170)
        Me.option_log.Name = "option_log"
        Me.option_log.Size = New System.Drawing.Size(223, 19)
        Me.option_log.TabIndex = 4
        Me.option_log.TabStop = True
        Me.option_log.Text = "NuGen Logarithemic Functions"
        Me.option_log.UseVisualStyleBackColor = True
        '
        'option_binary
        '
        Me.option_binary.AutoSize = True
        Me.option_binary.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_binary.ForeColor = System.Drawing.SystemColors.InactiveCaption
        Me.option_binary.Location = New System.Drawing.Point(30, 92)
        Me.option_binary.Name = "option_binary"
        Me.option_binary.Size = New System.Drawing.Size(113, 19)
        Me.option_binary.TabIndex = 2
        Me.option_binary.TabStop = True
        Me.option_binary.Text = "NuGen Binary"
        Me.option_binary.UseVisualStyleBackColor = True
        '
        'option_roots
        '
        Me.option_roots.AutoSize = True
        Me.option_roots.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option_roots.ForeColor = System.Drawing.SystemColors.InactiveCaption
        Me.option_roots.Location = New System.Drawing.Point(30, 52)
        Me.option_roots.Name = "option_roots"
        Me.option_roots.Size = New System.Drawing.Size(110, 19)
        Me.option_roots.TabIndex = 1
        Me.option_roots.TabStop = True
        Me.option_roots.Text = "NuGen Roots"
        Me.option_roots.UseVisualStyleBackColor = True
        '
        'button_ok
        '
        Me.button_ok.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.button_ok.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.button_ok.ForeColor = System.Drawing.SystemColors.Window
        Me.button_ok.Location = New System.Drawing.Point(293, 152)
        Me.button_ok.Name = "button_ok"
        Me.button_ok.Size = New System.Drawing.Size(76, 37)
        Me.button_ok.TabIndex = 5
        Me.button_ok.Text = "OK"
        Me.button_ok.UseVisualStyleBackColor = False
        '
        'NuGen_Numeric_menu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(393, 216)
        Me.Controls.Add(Me.option_complex)
        Me.Controls.Add(Me.option_log)
        Me.Controls.Add(Me.option_binary)
        Me.Controls.Add(Me.option_roots)
        Me.Controls.Add(Me.button_ok)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.Name = "NuGen_Numeric_menu"
        Me.Text = "NuGen Numeric Menu"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents option_complex As System.Windows.Forms.RadioButton
    Private WithEvents option_log As System.Windows.Forms.RadioButton
    Private WithEvents option_binary As System.Windows.Forms.RadioButton
    Private WithEvents option_roots As System.Windows.Forms.RadioButton
    Private WithEvents button_ok As System.Windows.Forms.Button
End Class

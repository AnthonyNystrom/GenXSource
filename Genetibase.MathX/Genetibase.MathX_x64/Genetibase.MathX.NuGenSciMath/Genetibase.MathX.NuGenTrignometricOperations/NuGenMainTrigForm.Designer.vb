<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenMainTrigForm
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.BinaryOperationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BinaryAdditionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.BinaryNotToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.NuMericalOprationsMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label2.Location = New System.Drawing.Point(94, 142)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(166, 37)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Functions"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.label1.Location = New System.Drawing.Point(29, 71)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(346, 37)
        Me.label1.TabIndex = 4
        Me.label1.Text = "NuGen  Trignometry  "
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.MenuStrip1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BinaryOperationsToolStripMenuItem, Me.ToolStripMenuItem1})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(365, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'BinaryOperationsToolStripMenuItem
        '
        Me.BinaryOperationsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BinaryAdditionToolStripMenuItem, Me.ToolStripMenuItem2, Me.BinaryNotToolStripMenuItem})
        Me.BinaryOperationsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Desktop
        Me.BinaryOperationsToolStripMenuItem.Name = "BinaryOperationsToolStripMenuItem"
        Me.BinaryOperationsToolStripMenuItem.Size = New System.Drawing.Size(236, 20)
        Me.BinaryOperationsToolStripMenuItem.Text = "&Trignometry Operations Operations"
        '
        'BinaryAdditionToolStripMenuItem
        '
        Me.BinaryAdditionToolStripMenuItem.Name = "BinaryAdditionToolStripMenuItem"
        Me.BinaryAdditionToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.BinaryAdditionToolStripMenuItem.Text = "Trignometry With Arcs"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(251, 22)
        Me.ToolStripMenuItem2.Text = "Trignometry With Fun"
        '
        'BinaryNotToolStripMenuItem
        '
        Me.BinaryNotToolStripMenuItem.Name = "BinaryNotToolStripMenuItem"
        Me.BinaryNotToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.BinaryNotToolStripMenuItem.Text = "Trignometry Of Conversion"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NuMericalOprationsMenuToolStripMenuItem})
        Me.ToolStripMenuItem1.ForeColor = System.Drawing.SystemColors.Desktop
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(47, 20)
        Me.ToolStripMenuItem1.Text = "&Back"
        '
        'NuMericalOprationsMenuToolStripMenuItem
        '
        Me.NuMericalOprationsMenuToolStripMenuItem.Name = "NuMericalOprationsMenuToolStripMenuItem"
        Me.NuMericalOprationsMenuToolStripMenuItem.Size = New System.Drawing.Size(246, 22)
        Me.NuMericalOprationsMenuToolStripMenuItem.Text = "NuMerical Oprations Menu"
        '
        'NuGenMainTrigForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(365, 266)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.Name = "NuGenMainTrigForm"
        Me.Text = "NuGen Main Trignometry "
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents BinaryOperationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BinaryAdditionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BinaryNotToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NuMericalOprationsMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class

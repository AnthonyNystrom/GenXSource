<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenInterpolationMainForm
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
        Me.InterpolationForLagrangeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GeneralIntegrationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LagrangeDerivativeInterpolationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LargrangeInterpolationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewtoneInterpolationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InterpolationForLagrangeToolStripMenuItem, Me.BackToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(431, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'InterpolationForLagrangeToolStripMenuItem
        '
        Me.InterpolationForLagrangeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GeneralIntegrationToolStripMenuItem, Me.LagrangeDerivativeInterpolationToolStripMenuItem, Me.LargrangeInterpolationToolStripMenuItem, Me.NewtoneInterpolationToolStripMenuItem})
        Me.InterpolationForLagrangeToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InterpolationForLagrangeToolStripMenuItem.Name = "InterpolationForLagrangeToolStripMenuItem"
        Me.InterpolationForLagrangeToolStripMenuItem.Size = New System.Drawing.Size(185, 20)
        Me.InterpolationForLagrangeToolStripMenuItem.Text = "&Interpolation For Lagrange"
        '
        'GeneralIntegrationToolStripMenuItem
        '
        Me.GeneralIntegrationToolStripMenuItem.Name = "GeneralIntegrationToolStripMenuItem"
        Me.GeneralIntegrationToolStripMenuItem.Size = New System.Drawing.Size(294, 22)
        Me.GeneralIntegrationToolStripMenuItem.Text = "General Integration"
        '
        'LagrangeDerivativeInterpolationToolStripMenuItem
        '
        Me.LagrangeDerivativeInterpolationToolStripMenuItem.Name = "LagrangeDerivativeInterpolationToolStripMenuItem"
        Me.LagrangeDerivativeInterpolationToolStripMenuItem.Size = New System.Drawing.Size(294, 22)
        Me.LagrangeDerivativeInterpolationToolStripMenuItem.Text = "Lagrange Derivative Interpolation"
        '
        'LargrangeInterpolationToolStripMenuItem
        '
        Me.LargrangeInterpolationToolStripMenuItem.Name = "LargrangeInterpolationToolStripMenuItem"
        Me.LargrangeInterpolationToolStripMenuItem.Size = New System.Drawing.Size(294, 22)
        Me.LargrangeInterpolationToolStripMenuItem.Text = "Largrange Interpolation"
        '
        'NewtoneInterpolationToolStripMenuItem
        '
        Me.NewtoneInterpolationToolStripMenuItem.Name = "NewtoneInterpolationToolStripMenuItem"
        Me.NewtoneInterpolationToolStripMenuItem.Size = New System.Drawing.Size(294, 22)
        Me.NewtoneInterpolationToolStripMenuItem.Text = "Newtone Interpolation"
        '
        'BackToolStripMenuItem
        '
        Me.BackToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackToolStripMenuItem.Name = "BackToolStripMenuItem"
        Me.BackToolStripMenuItem.Size = New System.Drawing.Size(51, 20)
        Me.BackToolStripMenuItem.Text = " &Back"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(84, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(114, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(164, 24)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "InterPolation For"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(84, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(147, 105)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 24)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Largrange"
        '
        'NuGenInterpolationMainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(431, 205)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "NuGenInterpolationMainForm"
        Me.Text = "NuGenInterpolationMainForm"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents InterpolationForLagrangeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GeneralIntegrationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LagrangeDerivativeInterpolationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LargrangeInterpolationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewtoneInterpolationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class

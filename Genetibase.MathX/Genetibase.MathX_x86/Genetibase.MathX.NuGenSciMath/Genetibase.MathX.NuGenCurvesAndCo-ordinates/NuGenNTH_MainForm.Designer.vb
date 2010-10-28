<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenNTH_MainForm
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.CurvesAndCoordinatesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HyperbolicTrignometricFunctionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SystemOfCoordinatesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InterpolationUsingLagrangeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InterpolationUsingNTHToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.InterpolationUsingCordicToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(84, 110)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(203, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "InterPolation Using NTH"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CurvesAndCoordinatesToolStripMenuItem, Me.BackToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(405, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'CurvesAndCoordinatesToolStripMenuItem
        '
        Me.CurvesAndCoordinatesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HyperbolicTrignometricFunctionsToolStripMenuItem, Me.SystemOfCoordinatesToolStripMenuItem, Me.InterpolationUsingLagrangeToolStripMenuItem, Me.InterpolationUsingNTHToolStripMenuItem, Me.InterpolationUsingCordicToolStripMenuItem})
        Me.CurvesAndCoordinatesToolStripMenuItem.Name = "CurvesAndCoordinatesToolStripMenuItem"
        Me.CurvesAndCoordinatesToolStripMenuItem.Size = New System.Drawing.Size(152, 20)
        Me.CurvesAndCoordinatesToolStripMenuItem.Text = "InterPolation For NTH"
        '
        'HyperbolicTrignometricFunctionsToolStripMenuItem
        '
        Me.HyperbolicTrignometricFunctionsToolStripMenuItem.Name = "HyperbolicTrignometricFunctionsToolStripMenuItem"
        Me.HyperbolicTrignometricFunctionsToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.HyperbolicTrignometricFunctionsToolStripMenuItem.Text = "Integer Order Bessel"
        '
        'SystemOfCoordinatesToolStripMenuItem
        '
        Me.SystemOfCoordinatesToolStripMenuItem.Name = "SystemOfCoordinatesToolStripMenuItem"
        Me.SystemOfCoordinatesToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.SystemOfCoordinatesToolStripMenuItem.Text = "Cliptic"
        '
        'InterpolationUsingLagrangeToolStripMenuItem
        '
        Me.InterpolationUsingLagrangeToolStripMenuItem.Name = "InterpolationUsingLagrangeToolStripMenuItem"
        Me.InterpolationUsingLagrangeToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.InterpolationUsingLagrangeToolStripMenuItem.Text = "Forhermite"
        '
        'InterpolationUsingNTHToolStripMenuItem
        '
        Me.InterpolationUsingNTHToolStripMenuItem.Name = "InterpolationUsingNTHToolStripMenuItem"
        Me.InterpolationUsingNTHToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.InterpolationUsingNTHToolStripMenuItem.Text = "Arc SinX"
        '
        'InterpolationUsingCordicToolStripMenuItem
        '
        Me.InterpolationUsingCordicToolStripMenuItem.Name = "InterpolationUsingCordicToolStripMenuItem"
        Me.InterpolationUsingCordicToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.InterpolationUsingCordicToolStripMenuItem.Text = "Ataniter"
        '
        'BackToolStripMenuItem
        '
        Me.BackToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1})
        Me.BackToolStripMenuItem.Name = "BackToolStripMenuItem"
        Me.BackToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.BackToolStripMenuItem.Text = "&Back"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(271, 22)
        Me.ToolStripMenuItem1.Text = "Curves And Co-ordinate Menu"
        '
        'NuGenNTH_MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(405, 266)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.Name = "NuGenNTH_MainForm"
        Me.Text = "NuGen NTH Main"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents CurvesAndCoordinatesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HyperbolicTrignometricFunctionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SystemOfCoordinatesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InterpolationUsingLagrangeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InterpolationUsingNTHToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InterpolationUsingCordicToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
End Class

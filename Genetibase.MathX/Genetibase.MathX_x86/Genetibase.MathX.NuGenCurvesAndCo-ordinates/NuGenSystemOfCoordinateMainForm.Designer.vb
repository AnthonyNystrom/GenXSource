<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenSystemOfCoordinateMainForm
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.InterpolationForLagrangeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GeneralIntegrationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LagrangeDerivativeInterpolationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LargrangeInterpolationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewtoneInterpolationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PolarDivisionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PolarMultiplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PolarPowerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PolarRootToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RectangularComplexNoMultiplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RectangularComplexNoDivisionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RectangularComplexNoPowerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RectangularComplexNoRootToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CurvesAndCoordinateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(84, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(139, 133)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(129, 24)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Co-ordinates"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(84, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(106, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 24)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "System Of"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InterpolationForLagrangeToolStripMenuItem, Me.BackToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(412, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'InterpolationForLagrangeToolStripMenuItem
        '
        Me.InterpolationForLagrangeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GeneralIntegrationToolStripMenuItem, Me.LagrangeDerivativeInterpolationToolStripMenuItem, Me.LargrangeInterpolationToolStripMenuItem, Me.NewtoneInterpolationToolStripMenuItem, Me.PolarDivisionToolStripMenuItem, Me.PolarMultiplicationToolStripMenuItem, Me.PolarPowerToolStripMenuItem, Me.PolarRootToolStripMenuItem, Me.RectangularComplexNoMultiplicationToolStripMenuItem, Me.RectangularComplexNoDivisionToolStripMenuItem, Me.RectangularComplexNoPowerToolStripMenuItem, Me.RectangularComplexNoRootToolStripMenuItem})
        Me.InterpolationForLagrangeToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InterpolationForLagrangeToolStripMenuItem.Name = "InterpolationForLagrangeToolStripMenuItem"
        Me.InterpolationForLagrangeToolStripMenuItem.Size = New System.Drawing.Size(141, 20)
        Me.InterpolationForLagrangeToolStripMenuItem.Text = "&Co-ordinate System"
        '
        'GeneralIntegrationToolStripMenuItem
        '
        Me.GeneralIntegrationToolStripMenuItem.Name = "GeneralIntegrationToolStripMenuItem"
        Me.GeneralIntegrationToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.GeneralIntegrationToolStripMenuItem.Text = "Rectanglr To Sphere"
        '
        'LagrangeDerivativeInterpolationToolStripMenuItem
        '
        Me.LagrangeDerivativeInterpolationToolStripMenuItem.Name = "LagrangeDerivativeInterpolationToolStripMenuItem"
        Me.LagrangeDerivativeInterpolationToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.LagrangeDerivativeInterpolationToolStripMenuItem.Text = "Sphere To Rectangular"
        '
        'LargrangeInterpolationToolStripMenuItem
        '
        Me.LargrangeInterpolationToolStripMenuItem.Name = "LargrangeInterpolationToolStripMenuItem"
        Me.LargrangeInterpolationToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.LargrangeInterpolationToolStripMenuItem.Text = "Complex Addition"
        '
        'NewtoneInterpolationToolStripMenuItem
        '
        Me.NewtoneInterpolationToolStripMenuItem.Name = "NewtoneInterpolationToolStripMenuItem"
        Me.NewtoneInterpolationToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.NewtoneInterpolationToolStripMenuItem.Text = "Complex  Subtraction"
        '
        'PolarDivisionToolStripMenuItem
        '
        Me.PolarDivisionToolStripMenuItem.Name = "PolarDivisionToolStripMenuItem"
        Me.PolarDivisionToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.PolarDivisionToolStripMenuItem.Text = "Polar Division"
        '
        'PolarMultiplicationToolStripMenuItem
        '
        Me.PolarMultiplicationToolStripMenuItem.Name = "PolarMultiplicationToolStripMenuItem"
        Me.PolarMultiplicationToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.PolarMultiplicationToolStripMenuItem.Text = "Polar Multiplication"
        '
        'PolarPowerToolStripMenuItem
        '
        Me.PolarPowerToolStripMenuItem.Name = "PolarPowerToolStripMenuItem"
        Me.PolarPowerToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.PolarPowerToolStripMenuItem.Text = "Polar Power"
        '
        'PolarRootToolStripMenuItem
        '
        Me.PolarRootToolStripMenuItem.Name = "PolarRootToolStripMenuItem"
        Me.PolarRootToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.PolarRootToolStripMenuItem.Text = "Polar Root"
        '
        'RectangularComplexNoMultiplicationToolStripMenuItem
        '
        Me.RectangularComplexNoMultiplicationToolStripMenuItem.Name = "RectangularComplexNoMultiplicationToolStripMenuItem"
        Me.RectangularComplexNoMultiplicationToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.RectangularComplexNoMultiplicationToolStripMenuItem.Text = "Rectangular Complex No. Multiplication"
        '
        'RectangularComplexNoDivisionToolStripMenuItem
        '
        Me.RectangularComplexNoDivisionToolStripMenuItem.Name = "RectangularComplexNoDivisionToolStripMenuItem"
        Me.RectangularComplexNoDivisionToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.RectangularComplexNoDivisionToolStripMenuItem.Text = "Rectangular Complex No. Division"
        '
        'RectangularComplexNoPowerToolStripMenuItem
        '
        Me.RectangularComplexNoPowerToolStripMenuItem.Name = "RectangularComplexNoPowerToolStripMenuItem"
        Me.RectangularComplexNoPowerToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.RectangularComplexNoPowerToolStripMenuItem.Text = "Rectangular Complex No. Power"
        '
        'RectangularComplexNoRootToolStripMenuItem
        '
        Me.RectangularComplexNoRootToolStripMenuItem.Name = "RectangularComplexNoRootToolStripMenuItem"
        Me.RectangularComplexNoRootToolStripMenuItem.Size = New System.Drawing.Size(327, 22)
        Me.RectangularComplexNoRootToolStripMenuItem.Text = "Rectangular Complex No. Root"
        '
        'BackToolStripMenuItem
        '
        Me.BackToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CurvesAndCoordinateToolStripMenuItem, Me.MainMenuToolStripMenuItem})
        Me.BackToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackToolStripMenuItem.Name = "BackToolStripMenuItem"
        Me.BackToolStripMenuItem.Size = New System.Drawing.Size(51, 20)
        Me.BackToolStripMenuItem.Text = " &Back"
        '
        'CurvesAndCoordinateToolStripMenuItem
        '
        Me.CurvesAndCoordinateToolStripMenuItem.Name = "CurvesAndCoordinateToolStripMenuItem"
        Me.CurvesAndCoordinateToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.CurvesAndCoordinateToolStripMenuItem.Text = "Curves And Co-ordinate Menu"
        '
        'MainMenuToolStripMenuItem
        '
        Me.MainMenuToolStripMenuItem.Name = "MainMenuToolStripMenuItem"
        Me.MainMenuToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.MainMenuToolStripMenuItem.Text = " Main Menu"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'NuGenSystemOfCoordinateMainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(412, 266)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Location = New System.Drawing.Point(30, 30)
        Me.Name = "NuGenSystemOfCoordinateMainForm"
        Me.Text = "NuGen System Of Coordinate Main"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents InterpolationForLagrangeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GeneralIntegrationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LagrangeDerivativeInterpolationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LargrangeInterpolationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewtoneInterpolationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PolarDivisionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PolarMultiplicationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PolarPowerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PolarRootToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RectangularComplexNoMultiplicationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RectangularComplexNoDivisionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RectangularComplexNoPowerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RectangularComplexNoRootToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CurvesAndCoordinateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.explorerBar1 = New Janus.Windows.ExplorerBar.ExplorerBar
        Me.numericEditBox1 = New Janus.Windows.GridEX.EditControls.NumericEditBox
        Me.integerUpDown1 = New Janus.Windows.GridEX.EditControls.IntegerUpDown
        Me.valueListUpDown1 = New Janus.Windows.GridEX.EditControls.ValueListUpDown
        Me.uIPanelManager1 = New Janus.Windows.UI.Dock.UIPanelManager(Me.components)
        Me.uITab1 = New Janus.Windows.UI.Tab.UITab
        Me.uIComboBox1 = New Janus.Windows.EditControls.UIComboBox
        Me.UiCommandManager1 = New Janus.Windows.UI.CommandBars.UICommandManager(Me.components)
        Me.TopRebar1 = New Janus.Windows.UI.CommandBars.UIRebar
        Me.LeftRebar1 = New Janus.Windows.UI.CommandBars.UIRebar
        Me.RightRebar1 = New Janus.Windows.UI.CommandBars.UIRebar
        Me.uIRBottomRebar1 = New Janus.Windows.UI.CommandBars.UIRebar
        Me.uIColorPicker1 = New Janus.Windows.EditControls.UIColorPicker
        Me.uIButton1 = New Janus.Windows.EditControls.UIButton
        Me.uIColorButton1 = New Janus.Windows.EditControls.UIColorButton
        Me.uICheckBox1 = New Janus.Windows.EditControls.UICheckBox
        CType(Me.explorerBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uIPanelManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uITab1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UiCommandManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TopRebar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LeftRebar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RightRebar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uIRBottomRebar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'explorerBar1
        '
        Me.explorerBar1.Location = New System.Drawing.Point(25, 50)
        Me.explorerBar1.Name = "explorerBar1"
        Me.explorerBar1.Size = New System.Drawing.Size(75, 23)
        Me.explorerBar1.TabIndex = 0
        '
        'numericEditBox1
        '
        Me.numericEditBox1.Location = New System.Drawing.Point(-1, 137)
        Me.numericEditBox1.Name = "numericEditBox1"
        Me.numericEditBox1.Size = New System.Drawing.Size(100, 20)
        Me.numericEditBox1.TabIndex = 1
        '
        'integerUpDown1
        '
        Me.integerUpDown1.Location = New System.Drawing.Point(204, 247)
        Me.integerUpDown1.Name = "integerUpDown1"
        Me.integerUpDown1.Size = New System.Drawing.Size(100, 20)
        Me.integerUpDown1.TabIndex = 2
        '
        'valueListUpDown1
        '
        Me.valueListUpDown1.Location = New System.Drawing.Point(176, 184)
        Me.valueListUpDown1.Name = "valueListUpDown1"
        Me.valueListUpDown1.Size = New System.Drawing.Size(100, 20)
        Me.valueListUpDown1.TabIndex = 3
        '
        'uIPanelManager1
        '
        Me.uIPanelManager1.ContainerControl = Me
        '
        'uITab1
        '
        Me.uITab1.Location = New System.Drawing.Point(193, 422)
        Me.uITab1.Name = "uITab1"
        Me.uITab1.Size = New System.Drawing.Size(200, 100)
        Me.uITab1.TabIndex = 8
        '
        'uIComboBox1
        '
        Me.uIComboBox1.Location = New System.Drawing.Point(49, 376)
        Me.uIComboBox1.Name = "uIComboBox1"
        Me.uIComboBox1.Size = New System.Drawing.Size(103, 20)
        Me.uIComboBox1.TabIndex = 9
        Me.uIComboBox1.Text = "UiComboBox1"
        '
        'UiCommandManager1
        '
        Me.UiCommandManager1.BottomRebar = Me.uIRBottomRebar1
        Me.UiCommandManager1.ContainerControl = Me
        Me.UiCommandManager1.Id = New System.Guid("61eca76f-158a-4aa2-a24f-3ec7aee41d50")
        Me.UiCommandManager1.LeftRebar = Me.LeftRebar1
        Me.UiCommandManager1.RightRebar = Me.RightRebar1
        Me.UiCommandManager1.TopRebar = Me.TopRebar1
        '
        'TopRebar1
        '
        Me.TopRebar1.CommandManager = Me.UiCommandManager1
        Me.TopRebar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TopRebar1.Location = New System.Drawing.Point(0, 0)
        Me.TopRebar1.Name = "TopRebar1"
        Me.TopRebar1.Size = New System.Drawing.Size(605, 0)
        '
        'LeftRebar1
        '
        Me.LeftRebar1.CommandManager = Me.UiCommandManager1
        Me.LeftRebar1.Dock = System.Windows.Forms.DockStyle.Left
        Me.LeftRebar1.Location = New System.Drawing.Point(0, 0)
        Me.LeftRebar1.Name = "LeftRebar1"
        Me.LeftRebar1.Size = New System.Drawing.Size(0, 517)
        '
        'RightRebar1
        '
        Me.RightRebar1.CommandManager = Me.UiCommandManager1
        Me.RightRebar1.Dock = System.Windows.Forms.DockStyle.Right
        Me.RightRebar1.Location = New System.Drawing.Point(605, 0)
        Me.RightRebar1.Name = "RightRebar1"
        Me.RightRebar1.Size = New System.Drawing.Size(0, 517)
        '
        'uIRBottomRebar1
        '
        Me.uIRBottomRebar1.CommandManager = Me.UiCommandManager1
        Me.uIRBottomRebar1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.uIRBottomRebar1.Location = New System.Drawing.Point(0, 517)
        Me.uIRBottomRebar1.Name = "uIRBottomRebar1"
        Me.uIRBottomRebar1.Size = New System.Drawing.Size(605, 0)
        '
        'uIColorPicker1
        '
        Me.uIColorPicker1.Location = New System.Drawing.Point(324, 349)
        Me.uIColorPicker1.Name = "uIColorPicker1"
        Me.uIColorPicker1.Size = New System.Drawing.Size(152, 149)
        Me.uIColorPicker1.TabIndex = 14
        Me.uIColorPicker1.Text = "UiColorPicker1"
        '
        'uIButton1
        '
        Me.uIButton1.Location = New System.Drawing.Point(24, 461)
        Me.uIButton1.Name = "uIButton1"
        Me.uIButton1.Size = New System.Drawing.Size(75, 23)
        Me.uIButton1.TabIndex = 15
        Me.uIButton1.Text = "UiButton1"
        '
        'uIColorButton1
        '
        '
        '
        '
        Me.uIColorButton1.ColorPicker.Location = New System.Drawing.Point(0, 0)
        Me.uIColorButton1.ColorPicker.Name = ""
        Me.uIColorButton1.ColorPicker.Size = New System.Drawing.Size(100, 100)
        Me.uIColorButton1.ColorPicker.TabIndex = 0
        Me.uIColorButton1.Location = New System.Drawing.Point(531, 133)
        Me.uIColorButton1.Name = "uIColorButton1"
        Me.uIColorButton1.Size = New System.Drawing.Size(75, 23)
        Me.uIColorButton1.TabIndex = 16
        Me.uIColorButton1.Text = "UiColorButton1"
        '
        'uICheckBox1
        '
        Me.uICheckBox1.Location = New System.Drawing.Point(387, 237)
        Me.uICheckBox1.Name = "uICheckBox1"
        Me.uICheckBox1.Size = New System.Drawing.Size(104, 23)
        Me.uICheckBox1.TabIndex = 17
        Me.uICheckBox1.Text = "UiCheckBox1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(605, 517)
        Me.Controls.Add(Me.uICheckBox1)
        Me.Controls.Add(Me.uIColorButton1)
        Me.Controls.Add(Me.uIButton1)
        Me.Controls.Add(Me.uIColorPicker1)
        Me.Controls.Add(Me.uIComboBox1)
        Me.Controls.Add(Me.uITab1)
        Me.Controls.Add(Me.valueListUpDown1)
        Me.Controls.Add(Me.integerUpDown1)
        Me.Controls.Add(Me.numericEditBox1)
        Me.Controls.Add(Me.explorerBar1)
        Me.Controls.Add(Me.LeftRebar1)
        Me.Controls.Add(Me.RightRebar1)
        Me.Controls.Add(Me.TopRebar1)
        Me.Controls.Add(Me.uIRBottomRebar1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.explorerBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uIPanelManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uITab1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UiCommandManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TopRebar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LeftRebar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RightRebar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uIRBottomRebar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents explorerBar1 As Janus.Windows.ExplorerBar.ExplorerBar
    Friend WithEvents numericEditBox1 As Janus.Windows.GridEX.EditControls.NumericEditBox
    Friend WithEvents integerUpDown1 As Janus.Windows.GridEX.EditControls.IntegerUpDown
    Friend WithEvents valueListUpDown1 As Janus.Windows.GridEX.EditControls.ValueListUpDown
    Friend WithEvents uIPanelManager1 As Janus.Windows.UI.Dock.UIPanelManager
    Friend WithEvents uICheckBox1 As Janus.Windows.EditControls.UICheckBox
    Friend WithEvents uIColorButton1 As Janus.Windows.EditControls.UIColorButton
    Friend WithEvents uIButton1 As Janus.Windows.EditControls.UIButton
    Friend WithEvents uIColorPicker1 As Janus.Windows.EditControls.UIColorPicker
    Friend WithEvents uIComboBox1 As Janus.Windows.EditControls.UIComboBox
    Friend WithEvents uITab1 As Janus.Windows.UI.Tab.UITab
    Friend WithEvents LeftRebar1 As Janus.Windows.UI.CommandBars.UIRebar
    Friend WithEvents UiCommandManager1 As Janus.Windows.UI.CommandBars.UICommandManager
    Friend WithEvents uIRBottomRebar1 As Janus.Windows.UI.CommandBars.UIRebar
    Friend WithEvents RightRebar1 As Janus.Windows.UI.CommandBars.UIRebar
    Friend WithEvents TopRebar1 As Janus.Windows.UI.CommandBars.UIRebar
End Class

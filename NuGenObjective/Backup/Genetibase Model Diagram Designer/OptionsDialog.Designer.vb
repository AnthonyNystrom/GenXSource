<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsDialog
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
		Me.Tabs = New Genetibase.SmoothControls.NuGenSmoothTabControl
		Me.TabAddIns = New Genetibase.SmoothControls.NuGenSmoothTabPage
		Me.AddInsGrid = New System.Windows.Forms.DataGridView
		Me.NuGenSmoothPanel1 = New Genetibase.SmoothControls.NuGenSmoothPanel
		Me.Cancel_Button = New Genetibase.SmoothControls.NuGenSmoothButton
		Me.OK_Button = New Genetibase.SmoothControls.NuGenSmoothButton
		Me.Tabs.SuspendLayout()
		Me.TabAddIns.SuspendLayout()
		CType(Me.AddInsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.NuGenSmoothPanel1.SuspendLayout()
		Me.SuspendLayout()
		'
		'Tabs
		'
		Me.Tabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
					Or System.Windows.Forms.AnchorStyles.Left) _
					Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Tabs.CloseButtonOnTab = False
		Me.Tabs.Location = New System.Drawing.Point(12, 12)
		Me.Tabs.Name = "Tabs"
		Me.Tabs.Size = New System.Drawing.Size(495, 279)
		Me.Tabs.TabIndex = 1
		Me.Tabs.TabPages.Add(Me.TabAddIns)
		'
		'TabAddIns
		'
		Me.TabAddIns.Controls.Add(Me.AddInsGrid)
		Me.TabAddIns.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TabAddIns.Location = New System.Drawing.Point(1, 28)
		Me.TabAddIns.Name = "TabAddIns"
		Me.TabAddIns.Padding = New System.Windows.Forms.Padding(3)
		Me.TabAddIns.Size = New System.Drawing.Size(492, 249)
		Me.TabAddIns.TabIndex = 0
		Me.TabAddIns.Text = "Add-ins"
		'
		'AddInsGrid
		'
		Me.AddInsGrid.AllowUserToAddRows = False
		Me.AddInsGrid.AllowUserToDeleteRows = False
		Me.AddInsGrid.AllowUserToResizeRows = False
		Me.AddInsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
		Me.AddInsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.AddInsGrid.Dock = System.Windows.Forms.DockStyle.Fill
		Me.AddInsGrid.Location = New System.Drawing.Point(3, 3)
		Me.AddInsGrid.MultiSelect = False
		Me.AddInsGrid.Name = "AddInsGrid"
		Me.AddInsGrid.RowHeadersVisible = False
		Me.AddInsGrid.Size = New System.Drawing.Size(486, 243)
		Me.AddInsGrid.TabIndex = 0
		'
		'NuGenSmoothPanel1
		'
		Me.NuGenSmoothPanel1.Controls.Add(Me.Cancel_Button)
		Me.NuGenSmoothPanel1.Controls.Add(Me.OK_Button)
		Me.NuGenSmoothPanel1.Controls.Add(Me.Tabs)
		Me.NuGenSmoothPanel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.NuGenSmoothPanel1.Location = New System.Drawing.Point(0, 0)
		Me.NuGenSmoothPanel1.Name = "NuGenSmoothPanel1"
		Me.NuGenSmoothPanel1.Size = New System.Drawing.Size(519, 339)
        'Me.NuGenSmoothPanel1.TabIndex = 2
		'
		'Cancel_Button
		'
		Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.Cancel_Button.Location = New System.Drawing.Point(407, 297)
		Me.Cancel_Button.Name = "Cancel_Button"
		Me.Cancel_Button.Size = New System.Drawing.Size(100, 30)
		Me.Cancel_Button.TabIndex = 3
		Me.Cancel_Button.Text = "&Cancel"
		Me.Cancel_Button.UseVisualStyleBackColor = False
		'
		'OK_Button
		'
		Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.OK_Button.Location = New System.Drawing.Point(301, 297)
		Me.OK_Button.Name = "OK_Button"
		Me.OK_Button.Size = New System.Drawing.Size(100, 30)
		Me.OK_Button.TabIndex = 2
		Me.OK_Button.Text = "&Ok"
		Me.OK_Button.UseVisualStyleBackColor = False
		'
		'OptionsDialog
		'
		Me.AcceptButton = Me.OK_Button
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.Cancel_Button
		Me.ClientSize = New System.Drawing.Size(519, 339)
		Me.Controls.Add(Me.NuGenSmoothPanel1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "OptionsDialog"
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Options"
		Me.Tabs.ResumeLayout(False)
		Me.TabAddIns.ResumeLayout(False)
		CType(Me.AddInsGrid, System.ComponentModel.ISupportInitialize).EndInit()
		Me.NuGenSmoothPanel1.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents Tabs As Genetibase.SmoothControls.NuGenSmoothTabControl
	Friend WithEvents TabAddIns As Genetibase.SmoothControls.NuGenSmoothTabPage
	Friend WithEvents AddInsGrid As System.Windows.Forms.DataGridView
	Friend WithEvents NuGenSmoothPanel1 As Genetibase.SmoothControls.NuGenSmoothPanel
	Friend WithEvents Cancel_Button As Genetibase.SmoothControls.NuGenSmoothButton
	Friend WithEvents OK_Button As Genetibase.SmoothControls.NuGenSmoothButton

End Class

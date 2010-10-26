<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UrlInputDialog
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
		Me._backgroundPanel = New Genetibase.SmoothControls.NuGenSmoothPanel
		Me._urlCombo = New Genetibase.SmoothControls.NuGenSmoothComboBox
		Me._messageLabel = New Genetibase.[Shared].Controls.NuGenLabel
		Me._cancelButton = New Genetibase.SmoothControls.NuGenSmoothButton
		Me._okButton = New Genetibase.SmoothControls.NuGenSmoothButton
		Me._backgroundPanel.SuspendLayout()
		Me.SuspendLayout()
		'
		'_backgroundPanel
		'
		Me._backgroundPanel.Controls.Add(Me._urlCombo)
		Me._backgroundPanel.Controls.Add(Me._messageLabel)
		Me._backgroundPanel.Controls.Add(Me._cancelButton)
		Me._backgroundPanel.Controls.Add(Me._okButton)
		Me._backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill
		Me._backgroundPanel.Location = New System.Drawing.Point(0, 0)
		Me._backgroundPanel.Name = "_backgroundPanel"
		Me._backgroundPanel.Size = New System.Drawing.Size(401, 80)
		Me._backgroundPanel.TabIndex = 0
		'
		'_urlCombo
		'
		Me._urlCombo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
					Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me._urlCombo.FormattingEnabled = True
		Me._urlCombo.Location = New System.Drawing.Point(46, 12)
		Me._urlCombo.Name = "_urlCombo"
		Me._urlCombo.Size = New System.Drawing.Size(343, 21)
		Me._urlCombo.TabIndex = 3
		'
		'_messageLabel
		'
		Me._messageLabel.AutoSize = False
		Me._messageLabel.Location = New System.Drawing.Point(12, 12)
		Me._messageLabel.Name = "_messageLabel"
		Me._messageLabel.Size = New System.Drawing.Size(41, 21)
		Me._messageLabel.TabIndex = 2
		Me._messageLabel.Text = "URL:"
		'
		'_cancelButton
		'
		Me._cancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me._cancelButton.Location = New System.Drawing.Point(289, 39)
		Me._cancelButton.Name = "_cancelButton"
		Me._cancelButton.Size = New System.Drawing.Size(100, 30)
		Me._cancelButton.TabIndex = 1
		Me._cancelButton.Text = "&Cancel"
		Me._cancelButton.UseVisualStyleBackColor = False
		'
		'_okButton
		'
		Me._okButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me._okButton.DialogResult = System.Windows.Forms.DialogResult.OK
		Me._okButton.Location = New System.Drawing.Point(183, 39)
		Me._okButton.Name = "_okButton"
		Me._okButton.Size = New System.Drawing.Size(100, 30)
		Me._okButton.TabIndex = 0
		Me._okButton.Text = "&Ok"
		Me._okButton.UseVisualStyleBackColor = False
		'
		'UrlInputDialog
		'
		Me.AcceptButton = Me._okButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me._cancelButton
		Me.ClientSize = New System.Drawing.Size(401, 80)
		Me.Controls.Add(Me._backgroundPanel)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "UrlInputDialog"
		Me.ShowIcon = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me._backgroundPanel.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents _backgroundPanel As Genetibase.SmoothControls.NuGenSmoothPanel
	Friend WithEvents _messageLabel As Genetibase.Shared.Controls.NuGenLabel
	Friend WithEvents _cancelButton As Genetibase.SmoothControls.NuGenSmoothButton
	Friend WithEvents _okButton As Genetibase.SmoothControls.NuGenSmoothButton
	Friend WithEvents _urlCombo As Genetibase.SmoothControls.NuGenSmoothComboBox
End Class

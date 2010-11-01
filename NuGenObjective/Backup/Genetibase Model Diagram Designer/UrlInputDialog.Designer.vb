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
        Me._goButton = New Genetibase.SmoothControls.NuGenSmoothButton
        Me.dgFilesOnServer = New System.Windows.Forms.DataGrid
        Me.ServerStatusFilesTableStyle = New System.Windows.Forms.DataGridTableStyle
        Me.FilenameColumn = New System.Windows.Forms.DataGridTextBoxColumn
        Me._backgroundPanel.SuspendLayout()
        CType(Me.dgFilesOnServer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_backgroundPanel
        '
        Me._backgroundPanel.Controls.Add(Me._urlCombo)
        Me._backgroundPanel.Controls.Add(Me._messageLabel)
        Me._backgroundPanel.Controls.Add(Me._cancelButton)
        Me._backgroundPanel.Controls.Add(Me._okButton)
        Me._backgroundPanel.Controls.Add(Me._goButton)
        Me._backgroundPanel.Controls.Add(Me.dgFilesOnServer)
        Me._backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._backgroundPanel.Location = New System.Drawing.Point(0, 0)
        Me._backgroundPanel.Name = "_backgroundPanel"
        Me._backgroundPanel.Size = New System.Drawing.Size(423, 267)
        Me._backgroundPanel.TabIndex = 0
        '
        '_urlCombo
        '
        Me._urlCombo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._urlCombo.FormattingEnabled = True
        Me._urlCombo.Location = New System.Drawing.Point(64, 12)
        Me._urlCombo.Name = "_urlCombo"
        Me._urlCombo.Size = New System.Drawing.Size(280, 21)
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
        Me._cancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me._cancelButton.Location = New System.Drawing.Point(312, 230)
        Me._cancelButton.Name = "_cancelButton"
        Me._cancelButton.Size = New System.Drawing.Size(100, 30)
        Me._cancelButton.TabIndex = 1
        Me._cancelButton.Text = "&Cancel"
        Me._cancelButton.UseVisualStyleBackColor = False
        '
        '_okButton
        '
        Me._okButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._okButton.Location = New System.Drawing.Point(212, 230)
        Me._okButton.Name = "_okButton"
        Me._okButton.Size = New System.Drawing.Size(100, 30)
        Me._okButton.TabIndex = 0
        Me._okButton.Text = "&Ok"
        Me._okButton.UseVisualStyleBackColor = False
        '
        '_goButton
        '
        Me._goButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._goButton.Location = New System.Drawing.Point(362, 10)
        Me._goButton.Name = "_goButton"
        Me._goButton.Size = New System.Drawing.Size(50, 26)
        Me._goButton.TabIndex = 0
        Me._goButton.Text = "&Go"
        Me._goButton.UseVisualStyleBackColor = False
        '
        'dgFilesOnServer
        '
        Me.dgFilesOnServer.AllowNavigation = False
        Me.dgFilesOnServer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgFilesOnServer.CaptionText = "Diagrams on Server"
        Me.dgFilesOnServer.DataMember = ""
        Me.dgFilesOnServer.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dgFilesOnServer.Location = New System.Drawing.Point(12, 39)
        Me.dgFilesOnServer.Name = "dgFilesOnServer"
        Me.dgFilesOnServer.ReadOnly = True
        Me.dgFilesOnServer.RowHeadersVisible = False
        Me.dgFilesOnServer.Size = New System.Drawing.Size(399, 150)
        Me.dgFilesOnServer.TabIndex = 1
        Me.dgFilesOnServer.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.ServerStatusFilesTableStyle})
        '
        'ServerStatusFilesTableStyle
        '
        Me.ServerStatusFilesTableStyle.DataGrid = Me.dgFilesOnServer
        Me.ServerStatusFilesTableStyle.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.ServerStatusFilesTableStyle.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.FilenameColumn})
        Me.ServerStatusFilesTableStyle.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.ServerStatusFilesTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ServerStatusFilesTableStyle.MappingName = "ArrayList"
        Me.ServerStatusFilesTableStyle.ReadOnly = True
        Me.ServerStatusFilesTableStyle.RowHeadersVisible = False
        '
        'FilenameColumn
        '
        Me.FilenameColumn.Format = ""
        Me.FilenameColumn.FormatInfo = Nothing
        Me.FilenameColumn.HeaderText = "Diagram Name"
        Me.FilenameColumn.MappingName = "Filename"
        Me.FilenameColumn.ReadOnly = True
        Me.FilenameColumn.Width = 150
        '
        'UrlInputDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me._cancelButton
        Me.ClientSize = New System.Drawing.Size(423, 267)
        Me.Controls.Add(Me._backgroundPanel)
        Me.DoubleBuffered = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UrlInputDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me._backgroundPanel.ResumeLayout(False)
        CType(Me.dgFilesOnServer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _urlCombo As Genetibase.SmoothControls.NuGenSmoothComboBox
    Private WithEvents _backgroundPanel As Genetibase.SmoothControls.NuGenSmoothPanel
    Private WithEvents _messageLabel As Genetibase.Shared.Controls.NuGenLabel
    Private WithEvents _cancelButton As Genetibase.SmoothControls.NuGenSmoothButton
    Private WithEvents _okButton As Genetibase.SmoothControls.NuGenSmoothButton
    Private WithEvents _goButton As Genetibase.SmoothControls.NuGenSmoothButton
    Friend WithEvents dgFilesOnServer As System.Windows.Forms.DataGrid
    Friend WithEvents ServerStatusFilesTableStyle As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents FilenameColumn As System.Windows.Forms.DataGridTextBoxColumn
End Class

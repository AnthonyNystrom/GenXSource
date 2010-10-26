<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InteractionDialog
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
        Me.ObjectPanel = New System.Windows.Forms.Panel
        Me.lblObject = New System.Windows.Forms.Label
        Me.StatePanel = New System.Windows.Forms.Panel
        Me.cmbState = New System.Windows.Forms.ComboBox
        Me.lblState = New System.Windows.Forms.Label
        Me.RolePanel = New System.Windows.Forms.Panel
        Me.cmbRole = New System.Windows.Forms.ComboBox
        Me.lblRole = New System.Windows.Forms.Label
        Me.ActionPanel = New System.Windows.Forms.Panel
        Me.cmbAction = New System.Windows.Forms.ComboBox
        Me.lblAction = New System.Windows.Forms.Label
        Me.txtInteraction = New System.Windows.Forms.TextBox
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.ObjectPanel.SuspendLayout()
        Me.StatePanel.SuspendLayout()
        Me.RolePanel.SuspendLayout()
        Me.ActionPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'ObjectPanel
        '
        Me.ObjectPanel.Controls.Add(Me.lblObject)
        Me.ObjectPanel.Location = New System.Drawing.Point(170, 12)
        Me.ObjectPanel.Name = "ObjectPanel"
        Me.ObjectPanel.Size = New System.Drawing.Size(196, 44)
        Me.ObjectPanel.TabIndex = 6
        '
        'lblObject
        '
        Me.lblObject.AutoSize = True
        Me.lblObject.BackColor = System.Drawing.Color.Transparent
        Me.lblObject.Location = New System.Drawing.Point(8, 15)
        Me.lblObject.Name = "lblObject"
        Me.lblObject.Size = New System.Drawing.Size(38, 13)
        Me.lblObject.TabIndex = 2
        Me.lblObject.Text = "Object"
        Me.lblObject.Visible = False
        '
        'StatePanel
        '
        Me.StatePanel.Controls.Add(Me.cmbState)
        Me.StatePanel.Controls.Add(Me.lblState)
        Me.StatePanel.Location = New System.Drawing.Point(181, 74)
        Me.StatePanel.Name = "StatePanel"
        Me.StatePanel.Size = New System.Drawing.Size(170, 44)
        Me.StatePanel.TabIndex = 7
        '
        'cmbState
        '
        Me.cmbState.FormattingEnabled = True
        Me.cmbState.Location = New System.Drawing.Point(35, 10)
        Me.cmbState.Name = "cmbState"
        Me.cmbState.Size = New System.Drawing.Size(121, 21)
        Me.cmbState.TabIndex = 1
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.BackColor = System.Drawing.Color.Transparent
        Me.lblState.Location = New System.Drawing.Point(3, 13)
        Me.lblState.Name = "lblState"
        Me.lblState.Size = New System.Drawing.Size(32, 13)
        Me.lblState.TabIndex = 0
        Me.lblState.Text = "State"
        '
        'RolePanel
        '
        Me.RolePanel.Controls.Add(Me.cmbRole)
        Me.RolePanel.Controls.Add(Me.lblRole)
        Me.RolePanel.Location = New System.Drawing.Point(8, 154)
        Me.RolePanel.Name = "RolePanel"
        Me.RolePanel.Size = New System.Drawing.Size(131, 64)
        Me.RolePanel.TabIndex = 8
        '
        'cmbRole
        '
        Me.cmbRole.FormattingEnabled = True
        Me.cmbRole.Location = New System.Drawing.Point(5, 25)
        Me.cmbRole.Name = "cmbRole"
        Me.cmbRole.Size = New System.Drawing.Size(121, 21)
        Me.cmbRole.TabIndex = 5
        '
        'lblRole
        '
        Me.lblRole.AutoSize = True
        Me.lblRole.BackColor = System.Drawing.Color.Transparent
        Me.lblRole.Location = New System.Drawing.Point(52, 9)
        Me.lblRole.Name = "lblRole"
        Me.lblRole.Size = New System.Drawing.Size(29, 13)
        Me.lblRole.TabIndex = 4
        Me.lblRole.Text = "Role"
        '
        'ActionPanel
        '
        Me.ActionPanel.Controls.Add(Me.cmbAction)
        Me.ActionPanel.Controls.Add(Me.lblAction)
        Me.ActionPanel.Location = New System.Drawing.Point(382, 154)
        Me.ActionPanel.Name = "ActionPanel"
        Me.ActionPanel.Size = New System.Drawing.Size(137, 63)
        Me.ActionPanel.TabIndex = 9
        '
        'cmbAction
        '
        Me.cmbAction.FormattingEnabled = True
        Me.cmbAction.Location = New System.Drawing.Point(8, 25)
        Me.cmbAction.Name = "cmbAction"
        Me.cmbAction.Size = New System.Drawing.Size(121, 21)
        Me.cmbAction.TabIndex = 7
        '
        'lblAction
        '
        Me.lblAction.AutoSize = True
        Me.lblAction.BackColor = System.Drawing.Color.Transparent
        Me.lblAction.Location = New System.Drawing.Point(47, 9)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New System.Drawing.Size(37, 13)
        Me.lblAction.TabIndex = 6
        Me.lblAction.Text = "Action"
        '
        'txtInteraction
        '
        Me.txtInteraction.Location = New System.Drawing.Point(187, 179)
        Me.txtInteraction.Name = "txtInteraction"
        Me.txtInteraction.Size = New System.Drawing.Size(153, 20)
        Me.txtInteraction.TabIndex = 10
        '
        'cmdOk
        '
        Me.cmdOk.Location = New System.Drawing.Point(185, 278)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(80, 24)
        Me.cmdOk.TabIndex = 11
        Me.cmdOk.Text = "Ok"
        Me.cmdOk.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(271, 278)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 24)
        Me.cmdCancel.TabIndex = 12
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'InteractionDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(531, 319)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.txtInteraction)
        Me.Controls.Add(Me.ActionPanel)
        Me.Controls.Add(Me.RolePanel)
        Me.Controls.Add(Me.StatePanel)
        Me.Controls.Add(Me.ObjectPanel)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InteractionDialog"
        Me.Opacity = 0.9
        Me.Text = "Add Interaction"
        Me.ObjectPanel.ResumeLayout(False)
        Me.ObjectPanel.PerformLayout()
        Me.StatePanel.ResumeLayout(False)
        Me.StatePanel.PerformLayout()
        Me.RolePanel.ResumeLayout(False)
        Me.RolePanel.PerformLayout()
        Me.ActionPanel.ResumeLayout(False)
        Me.ActionPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ObjectPanel As System.Windows.Forms.Panel
    Friend WithEvents lblObject As System.Windows.Forms.Label
    Friend WithEvents StatePanel As System.Windows.Forms.Panel
    Friend WithEvents cmbState As System.Windows.Forms.ComboBox
    Friend WithEvents lblState As System.Windows.Forms.Label
    Friend WithEvents RolePanel As System.Windows.Forms.Panel
    Friend WithEvents cmbRole As System.Windows.Forms.ComboBox
    Friend WithEvents lblRole As System.Windows.Forms.Label
    Friend WithEvents ActionPanel As System.Windows.Forms.Panel
    Friend WithEvents cmbAction As System.Windows.Forms.ComboBox
    Friend WithEvents lblAction As System.Windows.Forms.Label
    Friend WithEvents txtInteraction As System.Windows.Forms.TextBox
    Friend WithEvents cmdOk As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
End Class

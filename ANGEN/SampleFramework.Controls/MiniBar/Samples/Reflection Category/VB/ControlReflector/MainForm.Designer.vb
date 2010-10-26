<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me._button = New System.Windows.Forms.Button
        Me._buttonReflector = New Genetibase.[Shared].Controls.NuGenControlReflector
        Me._checkBox = New System.Windows.Forms.CheckBox
        Me.NuGenControlReflector1 = New Genetibase.[Shared].Controls.NuGenControlReflector
        Me._linkLabel = New Genetibase.[Shared].Controls.NuGenLinkLabel
        Me._linkLabelReflector = New Genetibase.[Shared].Controls.NuGenControlReflector
        Me._propertyGrid = New Genetibase.[Shared].Controls.NuGenPropertyGrid
        Me.SuspendLayout()
        '
        '_button
        '
        Me._button.Location = New System.Drawing.Point(12, 12)
        Me._button.Name = "_button"
        Me._button.Size = New System.Drawing.Size(75, 23)
        Me._button.TabIndex = 0
        Me._button.Text = "Go"
        Me._button.UseVisualStyleBackColor = True
        '
        '_buttonReflector
        '
        Me._buttonReflector.ControlToReflect = Me._button
        Me._buttonReflector.Location = New System.Drawing.Point(12, 41)
        Me._buttonReflector.Name = "_buttonReflector"
        Me._buttonReflector.Size = New System.Drawing.Size(75, 46)
        '
        '_checkBox
        '
        Me._checkBox.AutoSize = True
        Me._checkBox.Location = New System.Drawing.Point(12, 93)
        Me._checkBox.Name = "_checkBox"
        Me._checkBox.Size = New System.Drawing.Size(75, 17)
        Me._checkBox.TabIndex = 2
        Me._checkBox.Text = "CheckBox"
        Me._checkBox.UseVisualStyleBackColor = True
        '
        'NuGenControlReflector1
        '
        Me.NuGenControlReflector1.ControlToReflect = Me._checkBox
        Me.NuGenControlReflector1.Location = New System.Drawing.Point(12, 116)
        Me.NuGenControlReflector1.Name = "NuGenControlReflector1"
        Me.NuGenControlReflector1.Size = New System.Drawing.Size(75, 48)
        '
        '_linkLabel
        '
        Me._linkLabel.Image = CType(resources.GetObject("_linkLabel.Image"), System.Drawing.Image)
        Me._linkLabel.Location = New System.Drawing.Point(12, 170)
        Me._linkLabel.Name = "_linkLabel"
        Me._linkLabel.Size = New System.Drawing.Size(112, 16)
        Me._linkLabel.TabIndex = 4
        Me._linkLabel.Target = "http://www.genetibase.com/"
        Me._linkLabel.Text = "Genetibase, Inc."
        '
        '_linkLabelReflector
        '
        Me._linkLabelReflector.ControlToReflect = Me._linkLabel
        Me._linkLabelReflector.Location = New System.Drawing.Point(12, 192)
        Me._linkLabelReflector.Name = "_linkLabelReflector"
        Me._linkLabelReflector.Size = New System.Drawing.Size(112, 62)
        '
        '_propertyGrid
        '
        Me._propertyGrid.Location = New System.Drawing.Point(130, 12)
        Me._propertyGrid.Name = "_propertyGrid"
        Me._propertyGrid.SelectedObject = Me._linkLabelReflector
        Me._propertyGrid.Size = New System.Drawing.Size(150, 242)
        Me._propertyGrid.TabIndex = 6
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me._propertyGrid)
        Me.Controls.Add(Me._linkLabelReflector)
        Me.Controls.Add(Me._linkLabel)
        Me.Controls.Add(Me.NuGenControlReflector1)
        Me.Controls.Add(Me._checkBox)
        Me.Controls.Add(Me._buttonReflector)
        Me.Controls.Add(Me._button)
        Me.Name = "MainForm"
        Me.Text = "MainForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents _button As System.Windows.Forms.Button
    Friend WithEvents _buttonReflector As Genetibase.Shared.Controls.NuGenControlReflector
    Friend WithEvents _checkBox As System.Windows.Forms.CheckBox
    Friend WithEvents NuGenControlReflector1 As Genetibase.Shared.Controls.NuGenControlReflector
    Friend WithEvents _linkLabel As Genetibase.Shared.Controls.NuGenLinkLabel
    Friend WithEvents _linkLabelReflector As Genetibase.Shared.Controls.NuGenControlReflector
    Friend WithEvents _propertyGrid As Genetibase.Shared.Controls.NuGenPropertyGrid
End Class

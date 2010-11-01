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
        Me.goButton = New System.Windows.Forms.Button
        Me.openFileSelector = New Genetibase.Controls.NuGenOpenFileSelector
        Me.SuspendLayout()
        '
        'goButton
        '
        Me.goButton.Location = New System.Drawing.Point(268, 12)
        Me.goButton.Name = "goButton"
        Me.goButton.Size = New System.Drawing.Size(75, 25)
        Me.goButton.TabIndex = 3
        Me.goButton.Text = "&Go"
        Me.goButton.UseVisualStyleBackColor = True
        '
        'openFileSelector
        '
        Me.openFileSelector.Filter = "JPEG| *.jpg;*.jpeg"
        Me.openFileSelector.FilterIndex = 1
        Me.openFileSelector.Guid = "941b5c54-6de3-4bbf-9b98-7e8cae791db2"
        Me.openFileSelector.Location = New System.Drawing.Point(12, 12)
        Me.openFileSelector.Name = "openFileSelector"
        Me.openFileSelector.Size = New System.Drawing.Size(250, 25)
        Me.openFileSelector.TabIndex = 2
        Me.openFileSelector.Title = ""
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(354, 47)
        Me.Controls.Add(Me.goButton)
        Me.Controls.Add(Me.openFileSelector)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents goButton As System.Windows.Forms.Button
    Private WithEvents openFileSelector As Genetibase.Controls.NuGenOpenFileSelector

End Class

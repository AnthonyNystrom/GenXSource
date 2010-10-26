<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PaletteForm
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
        Me._snapUI = New Genetibase.[Shared].Controls.NuGenUISnap
        Me._colorSelector = New Genetibase.[Shared].Controls.NuGenHSVPlainSelector
        Me._colorHistory = New Genetibase.[Shared].Controls.NuGenColorHistory
        Me.SuspendLayout()
        '
        '_snapUI
        '
        Me._snapUI.HostForm = Me
        '
        '_colorSelector
        '
        Me._colorSelector.Dock = System.Windows.Forms.DockStyle.Fill
        Me._colorSelector.Location = New System.Drawing.Point(0, 0)
        Me._colorSelector.Name = "_colorSelector"
        Me._colorSelector.SelectedColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me._colorSelector.Size = New System.Drawing.Size(192, 173)
        Me._colorSelector.TabIndex = 3
        Me._colorSelector.Text = "nuGenHSVPlainSelector1"
        '
        '_colorHistory
        '
        Me._colorHistory.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._colorHistory.Location = New System.Drawing.Point(0, 173)
        Me._colorHistory.Name = "_colorHistory"
        Me._colorHistory.Size = New System.Drawing.Size(192, 43)
        Me._colorHistory.TabIndex = 2
        Me._colorHistory.Text = "nuGenColorHistory1"
        '
        'PaletteForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(192, 216)
        Me.Controls.Add(Me._colorSelector)
        Me.Controls.Add(Me._colorHistory)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "PaletteForm"
        Me.Text = "PaletteForm"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _snapUI As Genetibase.Shared.Controls.NuGenUISnap
    Private WithEvents _colorSelector As Genetibase.Shared.Controls.NuGenHSVPlainSelector
    Private WithEvents _colorHistory As Genetibase.Shared.Controls.NuGenColorHistory
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class lic
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
        Me.panelEx1 = New DevComponents.DotNetBar.PanelEx
        Me.uIButton1 = New Janus.Windows.EditControls.UIButton
        Me.SuspendLayout()
        '
        'panelEx1
        '
        Me.panelEx1.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelEx1.Location = New System.Drawing.Point(-19, -45)
        Me.panelEx1.Name = "panelEx1"
        Me.panelEx1.Size = New System.Drawing.Size(200, 100)
        Me.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelEx1.Style.GradientAngle = 90
        Me.panelEx1.TabIndex = 0
        Me.panelEx1.Text = "PanelEx1"
        '
        'uIButton1
        '
        Me.uIButton1.Location = New System.Drawing.Point(129, 193)
        Me.uIButton1.Name = "uIButton1"
        Me.uIButton1.Size = New System.Drawing.Size(75, 23)
        Me.uIButton1.TabIndex = 1
        Me.uIButton1.Text = "UiButton1"
        '
        'lic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(540, 447)
        Me.Controls.Add(Me.uIButton1)
        Me.Controls.Add(Me.panelEx1)
        Me.Name = "lic"
        Me.Text = "lic"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents panelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents uIButton1 As Janus.Windows.EditControls.UIButton
End Class

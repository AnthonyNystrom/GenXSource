<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.NuGenRuler1 = New Genetibase.UI.NuGenRuler
        Me.SuspendLayout()
        '
        'NuGenRuler1
        '
        Me.NuGenRuler1.DivisionMarkFactor = 5
        Me.NuGenRuler1.Divisions = 10
        Me.NuGenRuler1.ForeColor = System.Drawing.Color.Black
        Me.NuGenRuler1.Location = New System.Drawing.Point(53, 177)
        Me.NuGenRuler1.MajorInterval = 1
        Me.NuGenRuler1.MiddleMarkFactor = 3
        Me.NuGenRuler1.MouseTrackingOn = True
        Me.NuGenRuler1.Name = "NuGenRuler1"
        Me.NuGenRuler1.Orientation = Genetibase.UI.enumOrientation.orHorizontal
        Me.NuGenRuler1.RulerAlignment = Genetibase.UI.enumRulerAlignment.raBottomOrRight
        Me.NuGenRuler1.ScaleMode = Genetibase.UI.enumScaleMode.smInches
        Me.NuGenRuler1.Size = New System.Drawing.Size(682, 23)
        Me.NuGenRuler1.StartValue = 0
        Me.NuGenRuler1.TabIndex = 0
        Me.NuGenRuler1.Text = "NuGenRuler1"
        Me.NuGenRuler1.VerticalNumbers = False
        Me.NuGenRuler1.ZoomFactor = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 626)
        Me.Controls.Add(Me.NuGenRuler1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NuGenRuler1 As Genetibase.UI.NuGenRuler

End Class

Imports System.Drawing
Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PageViewerPanel
    Inherits System.Windows.Forms.Panel

    'Control overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Control Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  Do not modify it
    ' using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtEdit = New System.Windows.Forms.TextBox
		Me.scrollButtonUp = New Genetibase.SmoothControls.NuGenSmoothScrollButton
		Me.scrollButtonDown = New Genetibase.SmoothControls.NuGenSmoothScrollButton
		Me.scrollButtonLeft = New Genetibase.SmoothControls.NuGenSmoothScrollButton
		Me.scrollButtonRight = New Genetibase.SmoothControls.NuGenSmoothScrollButton
		Me.SuspendLayout()
		'
		'txtEdit
		'
		Me.txtEdit.BackColor = System.Drawing.Color.Cyan
		Me.txtEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtEdit.Location = New System.Drawing.Point(0, 0)
		Me.txtEdit.Name = "txtEdit"
		Me.txtEdit.Size = New System.Drawing.Size(100, 20)
		Me.txtEdit.TabIndex = 0
		Me.txtEdit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me.txtEdit.Visible = False
		'
		'PageViewerPanel
		'
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.Controls.Add(Me.txtEdit)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents txtEdit As System.Windows.Forms.TextBox
	Friend WithEvents scrollButtonUp As Genetibase.SmoothControls.NuGenSmoothScrollButton
	Friend WithEvents scrollButtonDown As Genetibase.SmoothControls.NuGenSmoothScrollButton
	Friend WithEvents scrollButtonLeft As Genetibase.SmoothControls.NuGenSmoothScrollButton
	Friend WithEvents scrollButtonRight As Genetibase.SmoothControls.NuGenSmoothScrollButton

End Class


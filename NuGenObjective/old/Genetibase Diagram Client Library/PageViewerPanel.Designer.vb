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
        Me.lblUp = New System.Windows.Forms.Label
        Me.lblDown = New System.Windows.Forms.Label
        Me.lblLeft = New System.Windows.Forms.Label
        Me.lblRight = New System.Windows.Forms.Label
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
        'lblUp
        '
        Me.lblUp.BackColor = System.Drawing.Color.Transparent
        Me.lblUp.Font = New System.Drawing.Font("Wingdings", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.lblUp.Location = New System.Drawing.Point(0, 0)
        Me.lblUp.Name = "lblUp"
        Me.lblUp.Size = New System.Drawing.Size(16, 16)
        Me.lblUp.TabIndex = 0
        Me.lblUp.Text = "ñ"
        Me.lblUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDown
        '
        Me.lblDown.AutoSize = True
        Me.lblDown.BackColor = System.Drawing.Color.Transparent
        Me.lblDown.Font = New System.Drawing.Font("Wingdings", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.lblDown.Location = New System.Drawing.Point(0, 0)
        Me.lblDown.Name = "lblDown"
        Me.lblDown.Size = New System.Drawing.Size(16, 16)
        Me.lblDown.TabIndex = 0
        Me.lblDown.Text = "ò"
        Me.lblDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLeft
        '
        Me.lblLeft.BackColor = System.Drawing.Color.Transparent
        Me.lblLeft.Font = New System.Drawing.Font("Wingdings", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.lblLeft.Location = New System.Drawing.Point(0, 0)
        Me.lblLeft.Name = "lblLeft"
        Me.lblLeft.Size = New System.Drawing.Size(16, 16)
        Me.lblLeft.TabIndex = 0
        Me.lblLeft.Text = "ï"
        Me.lblLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRight
        '
        Me.lblRight.AutoSize = True
        Me.lblRight.BackColor = System.Drawing.Color.Transparent
        Me.lblRight.Font = New System.Drawing.Font("Wingdings", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.lblRight.Location = New System.Drawing.Point(0, 0)
        Me.lblRight.Name = "lblRight"
        Me.lblRight.Size = New System.Drawing.Size(16, 16)
        Me.lblRight.TabIndex = 0
        Me.lblRight.Text = "ð"
        Me.lblRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PageViewerPanel
        '
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.Controls.Add(Me.txtEdit)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtEdit As System.Windows.Forms.TextBox
    Friend WithEvents lblUp As System.Windows.Forms.Label
    Friend WithEvents lblDown As System.Windows.Forms.Label
    Friend WithEvents lblLeft As System.Windows.Forms.Label
    Friend WithEvents lblRight As System.Windows.Forms.Label

End Class


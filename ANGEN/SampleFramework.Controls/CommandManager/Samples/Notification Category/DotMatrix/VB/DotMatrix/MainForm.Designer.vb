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
        Me.components = New System.ComponentModel.Container
        Me._timer2 = New System.Windows.Forms.Timer(Me.components)
        Me._timer = New System.Windows.Forms.Timer(Me.components)
        Me._counterMatrix = New Genetibase.[Shared].Controls.NuGenMatrixLabel
        Me._dateMatrix = New Genetibase.[Shared].Controls.NuGenMatrixLabel
        Me._timeMatrix = New Genetibase.[Shared].Controls.NuGenMatrixLabel
        Me.SuspendLayout()
        '
        '_timer2
        '
        Me._timer2.Enabled = True
        Me._timer2.Interval = 50
        '
        '_timer
        '
        Me._timer.Interval = 1000
        '
        '_counterMatrix
        '
        Me._counterMatrix.AutoSize = True
        Me._counterMatrix.DotHeight = 10
        Me._counterMatrix.DotWidth = 10
        Me._counterMatrix.Location = New System.Drawing.Point(12, 89)
        Me._counterMatrix.Name = "_counterMatrix"
        Me._counterMatrix.Size = New System.Drawing.Size(60, 50)
        Me._counterMatrix.TabIndex = 5
        Me._counterMatrix.Text = " "
        '
        '_dateMatrix
        '
        Me._dateMatrix.AutoSize = True
        Me._dateMatrix.DotWidth = 3
        Me._dateMatrix.Location = New System.Drawing.Point(12, 58)
        Me._dateMatrix.Name = "_dateMatrix"
        Me._dateMatrix.OffColor = System.Drawing.Color.Navy
        Me._dateMatrix.OnColorShadow = System.Drawing.Color.Blue
        Me._dateMatrix.Size = New System.Drawing.Size(18, 25)
        Me._dateMatrix.TabIndex = 4
        Me._dateMatrix.Text = " "
        '
        '_timeMatrix
        '
        Me._timeMatrix.AutoSize = True
        Me._timeMatrix.DotHeight = 8
        Me._timeMatrix.DotWidth = 8
        Me._timeMatrix.Location = New System.Drawing.Point(12, 12)
        Me._timeMatrix.Name = "_timeMatrix"
        Me._timeMatrix.OnColor = System.Drawing.Color.Red
        Me._timeMatrix.OnColorShadow = System.Drawing.Color.Brown
        Me._timeMatrix.Size = New System.Drawing.Size(48, 40)
        Me._timeMatrix.TabIndex = 3
        Me._timeMatrix.Text = " "
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(582, 386)
        Me.Controls.Add(Me._counterMatrix)
        Me.Controls.Add(Me._dateMatrix)
        Me.Controls.Add(Me._timeMatrix)
        Me.Name = "MainForm"
        Me.Text = "DotMatrix"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _timer2 As System.Windows.Forms.Timer
    Private WithEvents _timer As System.Windows.Forms.Timer
    Private WithEvents _counterMatrix As Genetibase.Shared.Controls.NuGenMatrixLabel
    Private WithEvents _dateMatrix As Genetibase.Shared.Controls.NuGenMatrixLabel
    Private WithEvents _timeMatrix As Genetibase.Shared.Controls.NuGenMatrixLabel

End Class

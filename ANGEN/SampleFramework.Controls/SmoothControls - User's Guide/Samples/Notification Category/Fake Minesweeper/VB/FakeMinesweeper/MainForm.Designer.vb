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
        Me._goButton = New System.Windows.Forms.Button
        Me._segmentThree = New Genetibase.[Shared].Controls.NuGenSegDisplay
        Me._timer = New System.Windows.Forms.Timer(Me.components)
        Me._segmentTwo = New Genetibase.[Shared].Controls.NuGenSegDisplay
        Me._segmentOne = New Genetibase.[Shared].Controls.NuGenSegDisplay
        Me.SuspendLayout()
        '
        '_goButton
        '
        Me._goButton.Location = New System.Drawing.Point(65, 12)
        Me._goButton.Name = "_goButton"
        Me._goButton.Size = New System.Drawing.Size(35, 30)
        Me._goButton.TabIndex = 7
        Me._goButton.Text = "&Go"
        Me._goButton.UseVisualStyleBackColor = True
        '
        '_segmentThree
        '
        Me._segmentThree.Brightness = 3.0!
        Me._segmentThree.ColorLEDOff = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me._segmentThree.ColorLEDOn = System.Drawing.Color.Red
        Me._segmentThree.Location = New System.Drawing.Point(41, 12)
        Me._segmentThree.Name = "_segmentThree"
        Me._segmentThree.Size = New System.Drawing.Size(18, 30)
        Me._segmentThree.TabIndex = 6
        Me._segmentThree.Text = "nuGenSegDisplay3"
        Me._segmentThree.Value = 0
        '
        '_timer
        '
        '
        '_segmentTwo
        '
        Me._segmentTwo.Brightness = 3.0!
        Me._segmentTwo.ColorLEDOff = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me._segmentTwo.ColorLEDOn = System.Drawing.Color.Red
        Me._segmentTwo.Location = New System.Drawing.Point(25, 12)
        Me._segmentTwo.Name = "_segmentTwo"
        Me._segmentTwo.Size = New System.Drawing.Size(18, 30)
        Me._segmentTwo.TabIndex = 5
        Me._segmentTwo.Text = "nuGenSegDisplay2"
        Me._segmentTwo.Value = 0
        '
        '_segmentOne
        '
        Me._segmentOne.Brightness = 3.0!
        Me._segmentOne.ColorLEDOff = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me._segmentOne.ColorLEDOn = System.Drawing.Color.Red
        Me._segmentOne.Location = New System.Drawing.Point(9, 12)
        Me._segmentOne.Name = "_segmentOne"
        Me._segmentOne.Size = New System.Drawing.Size(18, 30)
        Me._segmentOne.TabIndex = 4
        Me._segmentOne.Text = "nuGenSegDisplay1"
        Me._segmentOne.Value = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me._goButton)
        Me.Controls.Add(Me._segmentThree)
        Me.Controls.Add(Me._segmentTwo)
        Me.Controls.Add(Me._segmentOne)
        Me.Name = "MainForm"
        Me.Text = "Fake Minesweeper"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _goButton As System.Windows.Forms.Button
    Private WithEvents _segmentThree As Genetibase.Shared.Controls.NuGenSegDisplay
    Private WithEvents _timer As System.Windows.Forms.Timer
    Private WithEvents _segmentTwo As Genetibase.Shared.Controls.NuGenSegDisplay
    Private WithEvents _segmentOne As Genetibase.Shared.Controls.NuGenSegDisplay

End Class

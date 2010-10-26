<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCapHis
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCapHis))
        Me.ScribbleBox1 = New Agilix.Ink.Scribble.ScribbleBox
        Me.SuspendLayout()
        '
        'ScribbleBox1
        '
        Me.ScribbleBox1.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.ScribbleBox1, "ScribbleBox1")
        Me.ScribbleBox1.ForceSwapFont = New System.Drawing.Font("Times New Roman", 10.0!)
        Me.ScribbleBox1.HighlightElementColor = System.Drawing.Color.FromArgb(CType(CType(189, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ScribbleBox1.Name = "ScribbleBox1"
        Me.ScribbleBox1.ShowActionButtons = False
        Me.ScribbleBox1.ShowEdit = False
        Me.ScribbleBox1.ShowFlags = False
        Me.ScribbleBox1.ShowFont = False
        Me.ScribbleBox1.ShowFormat = False
        Me.ScribbleBox1.ShowTabs = False
        Me.ScribbleBox1.ShowZoom = False
        Me.ScribbleBox1.TabColor = System.Drawing.SystemColors.ControlDark
        '
        'frmCapHis
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ControlBox = False
        Me.Controls.Add(Me.ScribbleBox1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmCapHis"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ScribbleBox1 As Agilix.Ink.Scribble.ScribbleBox
End Class

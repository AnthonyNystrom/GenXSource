<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDwnCnt
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        Application.DoEvents()
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.butCancel = New DevComponents.DotNetBar.ButtonX
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(294, 28)
        Me.Label1.TabIndex = 1
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label1.UseCompatibleTextRendering = True
        Me.Label1.UseWaitCursor = True
        '
        'butCancel
        '
        Me.butCancel.ColorScheme.DockSiteBackColorGradientAngle = 0
        Me.butCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.butCancel.Location = New System.Drawing.Point(218, 0)
        Me.butCancel.Name = "butCancel"
        Me.butCancel.Size = New System.Drawing.Size(75, 27)
        Me.butCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003
        Me.butCancel.TabIndex = 2
        Me.butCancel.Text = "Cancel"
        '
        'frmDwnCnt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(294, 28)
        Me.ControlBox = False
        Me.Controls.Add(Me.butCancel)
        Me.Controls.Add(Me.Label1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDwnCnt"
        Me.Opacity = 0.8
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Private _cntvalue As Integer

    Public Property cntvaluex() As Integer
        Get
            Return _cntvalue
        End Get
        Set(ByVal value As Integer)
            _cntvalue = value
        End Set
    End Property


    Public Sub New(ByVal cntvalue As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        cntvaluex = cntvalue

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Friend WithEvents butCancel As DevComponents.DotNetBar.ButtonX
End Class

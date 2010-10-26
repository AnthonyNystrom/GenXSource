Public Class BalloonSearch
    Inherits DevComponents.DotNetBar.Balloon

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents pictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents button1 As System.Windows.Forms.Button
    Friend WithEvents textBox1 As System.Windows.Forms.TextBox
    Friend WithEvents linkLabel1 As System.Windows.Forms.LinkLabel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(BalloonSearch))
        Me.pictureBox1 = New System.Windows.Forms.PictureBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.button1 = New System.Windows.Forms.Button()
        Me.textBox1 = New System.Windows.Forms.TextBox()
        Me.linkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.SuspendLayout()
        '
        'pictureBox1
        '
        Me.pictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.pictureBox1.Image = CType(resources.GetObject("pictureBox1.Image"), System.Drawing.Bitmap)
        Me.pictureBox1.Location = New System.Drawing.Point(8, 24)
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New System.Drawing.Size(64, 64)
        Me.pictureBox1.TabIndex = 7
        Me.pictureBox1.TabStop = False
        '
        'label1
        '
        Me.label1.BackColor = System.Drawing.Color.Transparent
        Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label1.Location = New System.Drawing.Point(72, 32)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(112, 16)
        Me.label1.TabIndex = 6
        Me.label1.Text = "Find what:"
        '
        'button1
        '
        Me.button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.button1.Location = New System.Drawing.Point(116, 72)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(72, 24)
        Me.button1.TabIndex = 5
        Me.button1.Text = "&Search"
        '
        'textBox1
        '
        Me.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.textBox1.Location = New System.Drawing.Point(72, 48)
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New System.Drawing.Size(160, 20)
        Me.textBox1.TabIndex = 4
        Me.textBox1.Text = ""
        '
        'linkLabel1
        '
        Me.linkLabel1.AutoSize = True
        Me.linkLabel1.BackColor = System.Drawing.Color.Transparent
        Me.linkLabel1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.linkLabel1.Location = New System.Drawing.Point(8, 88)
        Me.linkLabel1.Name = "linkLabel1"
        Me.linkLabel1.Size = New System.Drawing.Size(61, 11)
        Me.linkLabel1.TabIndex = 8
        Me.linkLabel1.TabStop = True
        Me.linkLabel1.Text = "Switch Style"
        Me.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue
        '
        'BalloonSearch
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(240, 104)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.linkLabel1, Me.pictureBox1, Me.label1, Me.button1, Me.textBox1})
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "BalloonSearch"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub BalloonSearch_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Me.Opacity = 1
    End Sub

    Private Sub BalloonSearch_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Deactivate
        Me.Opacity = 0.75
    End Sub

    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
        If Not Me.Owner Is Nothing Then
            If textBox1.Text = "" Then
                MessageBox.Show("Please enter search text.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim f As frmMain = CType(Me.Owner, frmMain)
                f.SearchActiveDocument(textBox1.Text)
            End If
        End If
    End Sub

    Private Sub linkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles linkLabel1.LinkClicked
        If Me.BackColor.Equals(SystemColors.Info) Then
            ' Switch to Office 2003 style colors...
            Dim scheme As DevComponents.DotNetBar.ColorScheme = New DevComponents.DotNetBar.ColorScheme(DevComponents.DotNetBar.eDotNetBarStyle.Office2003)
            Me.BackColor = scheme.ItemCheckedBackground
            Me.BackColor2 = scheme.ItemCheckedBackground2
            Me.BackColorGradientAngle = scheme.ItemCheckedBackgroundGradientAngle
            Me.Refresh()
        Else
        End If
    End Sub
End Class

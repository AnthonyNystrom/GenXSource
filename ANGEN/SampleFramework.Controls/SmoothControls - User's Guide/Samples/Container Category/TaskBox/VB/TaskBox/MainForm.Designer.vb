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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me._spacer = New Genetibase.[Shared].Controls.NuGenSpacer
        Me._taskBox = New Genetibase.SmoothControls.NuGenSmoothTaskBox
        Me._treeView = New Genetibase.[Shared].Controls.NuGenTreeView
        Me._taskBox2 = New Genetibase.SmoothControls.NuGenSmoothTaskBox
        Me._taskBox3 = New Genetibase.SmoothControls.NuGenSmoothTaskBox
        Me.nuGenSmoothPanel1 = New Genetibase.SmoothControls.NuGenSmoothPanel
        Me._spacer2 = New Genetibase.[Shared].Controls.NuGenSpacer
        Me._taskBox.SuspendLayout()
        Me.nuGenSmoothPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        '_spacer
        '
        Me._spacer.Dock = System.Windows.Forms.DockStyle.Top
        Me._spacer.Location = New System.Drawing.Point(0, 100)
        Me._spacer.Name = "_spacer"
        Me._spacer.Size = New System.Drawing.Size(231, 5)
        '
        '_taskBox
        '
        Me._taskBox.Controls.Add(Me._treeView)
        Me._taskBox.Dock = System.Windows.Forms.DockStyle.Top
        Me._taskBox.Image = CType(resources.GetObject("_taskBox.Image"), System.Drawing.Image)
        Me._taskBox.Location = New System.Drawing.Point(0, 0)
        Me._taskBox.Name = "_taskBox"
        Me._taskBox.Size = New System.Drawing.Size(231, 100)
        Me._taskBox.SmoothAnimation = True
        Me._taskBox.TabIndex = 0
        Me._taskBox.Text = "Accounts"
        '
        '_treeView
        '
        Me._treeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me._treeView.Location = New System.Drawing.Point(0, 0)
        Me._treeView.Name = "_treeView"
        Me._treeView.Size = New System.Drawing.Size(231, 79)
        Me._treeView.TabIndex = 1
        '
        '_taskBox2
        '
        Me._taskBox2.Dock = System.Windows.Forms.DockStyle.Top
        Me._taskBox2.Image = CType(resources.GetObject("_taskBox2.Image"), System.Drawing.Image)
        Me._taskBox2.Location = New System.Drawing.Point(0, 105)
        Me._taskBox2.Name = "_taskBox2"
        Me._taskBox2.Size = New System.Drawing.Size(231, 100)
        Me._taskBox2.SmoothAnimation = True
        Me._taskBox2.TabIndex = 2
        Me._taskBox2.Text = "Junk"
        '
        '_taskBox3
        '
        Me._taskBox3.Dock = System.Windows.Forms.DockStyle.Top
        Me._taskBox3.Image = CType(resources.GetObject("_taskBox3.Image"), System.Drawing.Image)
        Me._taskBox3.Location = New System.Drawing.Point(0, 210)
        Me._taskBox3.Name = "_taskBox3"
        Me._taskBox3.Size = New System.Drawing.Size(231, 100)
        Me._taskBox3.TabIndex = 5
        Me._taskBox3.Text = "RSS"
        '
        'nuGenSmoothPanel1
        '
        Me.nuGenSmoothPanel1.Controls.Add(Me._taskBox3)
        Me.nuGenSmoothPanel1.Controls.Add(Me._spacer2)
        Me.nuGenSmoothPanel1.Controls.Add(Me._taskBox2)
        Me.nuGenSmoothPanel1.Controls.Add(Me._spacer)
        Me.nuGenSmoothPanel1.Controls.Add(Me._taskBox)
        Me.nuGenSmoothPanel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.nuGenSmoothPanel1.Location = New System.Drawing.Point(0, 0)
        Me.nuGenSmoothPanel1.Name = "nuGenSmoothPanel1"
        Me.nuGenSmoothPanel1.Size = New System.Drawing.Size(231, 446)
        '
        '_spacer2
        '
        Me._spacer2.Dock = System.Windows.Forms.DockStyle.Top
        Me._spacer2.Location = New System.Drawing.Point(0, 205)
        Me._spacer2.Name = "_spacer2"
        Me._spacer2.Size = New System.Drawing.Size(231, 5)
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(632, 446)
        Me.Controls.Add(Me.nuGenSmoothPanel1)
        Me.Name = "MainForm"
        Me.Text = "TaskBox"
        Me._taskBox.ResumeLayout(False)
        Me.nuGenSmoothPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents _spacer As Genetibase.Shared.Controls.NuGenSpacer
    Private WithEvents _taskBox As Genetibase.SmoothControls.NuGenSmoothTaskBox
    Private WithEvents _treeView As Genetibase.Shared.Controls.NuGenTreeView
    Private WithEvents _taskBox2 As Genetibase.SmoothControls.NuGenSmoothTaskBox
    Private WithEvents _taskBox3 As Genetibase.SmoothControls.NuGenSmoothTaskBox
    Private WithEvents nuGenSmoothPanel1 As Genetibase.SmoothControls.NuGenSmoothPanel
    Private WithEvents _spacer2 As Genetibase.Shared.Controls.NuGenSpacer

End Class

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
        Me._tabControl = New Genetibase.[Shared].Controls.NuGenTabControl
        Me.nuGenTabPage1 = New Genetibase.[Shared].Controls.NuGenTabPage
        Me.nuGenTabPage3 = New Genetibase.[Shared].Controls.NuGenTabPage
        Me.nuGenTabPage2 = New Genetibase.[Shared].Controls.NuGenTabPage
        Me._addTabButton = New System.Windows.Forms.Button
        Me._splitContainer = New System.Windows.Forms.SplitContainer
        Me._tabControl2 = New Genetibase.[Shared].Controls.NuGenTabControl
        Me._splitContainer.Panel1.SuspendLayout()
        Me._splitContainer.Panel2.SuspendLayout()
        Me._splitContainer.SuspendLayout()
        Me._tabControl2.SuspendLayout()
        Me.SuspendLayout()
        '
        '_tabControl
        '
        Me._tabControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._tabControl.Location = New System.Drawing.Point(0, 43)
        Me._tabControl.Name = "_tabControl"
        Me._tabControl.Size = New System.Drawing.Size(362, 359)
        Me._tabControl.TabIndex = 0
        '
        'nuGenTabPage1
        '
        Me.nuGenTabPage1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nuGenTabPage1.Location = New System.Drawing.Point(1, 28)
        Me.nuGenTabPage1.Name = "nuGenTabPage1"
        Me.nuGenTabPage1.Padding = New System.Windows.Forms.Padding(0, 1, 0, 0)
        Me.nuGenTabPage1.Size = New System.Drawing.Size(338, 371)
        Me.nuGenTabPage1.TabIndex = 0
        Me.nuGenTabPage1.Text = "General"
        '
        'nuGenTabPage3
        '
        Me.nuGenTabPage3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nuGenTabPage3.Location = New System.Drawing.Point(1, 28)
        Me.nuGenTabPage3.Name = "nuGenTabPage3"
        Me.nuGenTabPage3.Padding = New System.Windows.Forms.Padding(0, 1, 0, 0)
        Me.nuGenTabPage3.Size = New System.Drawing.Size(338, 371)
        Me.nuGenTabPage3.TabIndex = 4
        Me.nuGenTabPage3.Text = "Network"
        '
        'nuGenTabPage2
        '
        Me.nuGenTabPage2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nuGenTabPage2.Location = New System.Drawing.Point(1, 28)
        Me.nuGenTabPage2.Name = "nuGenTabPage2"
        Me.nuGenTabPage2.Padding = New System.Windows.Forms.Padding(0, 1, 0, 0)
        Me.nuGenTabPage2.Size = New System.Drawing.Size(338, 371)
        Me.nuGenTabPage2.TabIndex = 2
        Me.nuGenTabPage2.Text = "Content"
        '
        '_addTabButton
        '
        Me._addTabButton.Location = New System.Drawing.Point(0, 0)
        Me._addTabButton.Name = "_addTabButton"
        Me._addTabButton.Size = New System.Drawing.Size(75, 23)
        Me._addTabButton.TabIndex = 1
        Me._addTabButton.Text = "&Add Tab"
        Me._addTabButton.UseVisualStyleBackColor = True
        '
        '_splitContainer
        '
        Me._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me._splitContainer.Location = New System.Drawing.Point(5, 5)
        Me._splitContainer.Name = "_splitContainer"
        '
        '_splitContainer.Panel1
        '
        Me._splitContainer.Panel1.Controls.Add(Me._tabControl)
        Me._splitContainer.Panel1.Controls.Add(Me._addTabButton)
        '
        '_splitContainer.Panel2
        '
        Me._splitContainer.Panel2.Controls.Add(Me._tabControl2)
        Me._splitContainer.Size = New System.Drawing.Size(708, 402)
        Me._splitContainer.SplitterDistance = 362
        Me._splitContainer.TabIndex = 4
        '
        '_tabControl2
        '
        Me._tabControl2.CloseButtonOnTab = False
        Me._tabControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me._tabControl2.Location = New System.Drawing.Point(0, 0)
        Me._tabControl2.Name = "_tabControl2"
        Me._tabControl2.Size = New System.Drawing.Size(342, 402)
        Me._tabControl2.TabIndex = 0
        Me._tabControl2.TabPages.Add(Me.nuGenTabPage1)
        Me._tabControl2.TabPages.Add(Me.nuGenTabPage2)
        Me._tabControl2.TabPages.Add(Me.nuGenTabPage3)
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(718, 412)
        Me.Controls.Add(Me._splitContainer)
        Me.Name = "MainForm"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.Text = "FirefoxTabControl"
        Me._splitContainer.Panel1.ResumeLayout(False)
        Me._splitContainer.Panel2.ResumeLayout(False)
        Me._splitContainer.ResumeLayout(False)
        Me._tabControl2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _tabControl As Genetibase.Shared.Controls.NuGenTabControl
    Private WithEvents nuGenTabPage1 As Genetibase.Shared.Controls.NuGenTabPage
    Private WithEvents nuGenTabPage3 As Genetibase.Shared.Controls.NuGenTabPage
    Private WithEvents nuGenTabPage2 As Genetibase.Shared.Controls.NuGenTabPage
    Friend WithEvents _addTabButton As System.Windows.Forms.Button
    Friend WithEvents _splitContainer As System.Windows.Forms.SplitContainer
    Private WithEvents _tabControl2 As Genetibase.Shared.Controls.NuGenTabControl

End Class

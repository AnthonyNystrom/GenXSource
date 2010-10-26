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
        Me.nuGenSmoothNavigationPane4 = New Genetibase.SmoothControls.NuGenSmoothNavigationPane
        Me.nuGenSmoothNavigationPane3 = New Genetibase.SmoothControls.NuGenSmoothNavigationPane
        Me.nuGenSmoothNavigationPane2 = New Genetibase.SmoothControls.NuGenSmoothNavigationPane
        Me._navigationBar = New Genetibase.SmoothControls.NuGenSmoothNavigationBar
        Me.nuGenSmoothNavigationPane1 = New Genetibase.SmoothControls.NuGenSmoothNavigationPane
        Me._splitContainer = New Genetibase.SmoothControls.NuGenSmoothSplitContainer
        Me._navigationBar.SuspendLayout()
        Me._splitContainer.Panel1.SuspendLayout()
        Me._splitContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'nuGenSmoothNavigationPane4
        '
        Me.nuGenSmoothNavigationPane4.Name = "nuGenSmoothNavigationPane4"
        Me.nuGenSmoothNavigationPane4.NavigationButtonImage = CType(resources.GetObject("nuGenSmoothNavigationPane4.NavigationButtonImage"), System.Drawing.Image)
        Me.nuGenSmoothNavigationPane4.TabIndex = 5
        Me.nuGenSmoothNavigationPane4.Text = "Advanced"
        '
        'nuGenSmoothNavigationPane3
        '
        Me.nuGenSmoothNavigationPane3.Name = "nuGenSmoothNavigationPane3"
        Me.nuGenSmoothNavigationPane3.NavigationButtonImage = CType(resources.GetObject("nuGenSmoothNavigationPane3.NavigationButtonImage"), System.Drawing.Image)
        Me.nuGenSmoothNavigationPane3.TabIndex = 4
        Me.nuGenSmoothNavigationPane3.Text = "Network"
        '
        'nuGenSmoothNavigationPane2
        '
        Me.nuGenSmoothNavigationPane2.Name = "nuGenSmoothNavigationPane2"
        Me.nuGenSmoothNavigationPane2.NavigationButtonImage = CType(resources.GetObject("nuGenSmoothNavigationPane2.NavigationButtonImage"), System.Drawing.Image)
        Me.nuGenSmoothNavigationPane2.TabIndex = 3
        Me.nuGenSmoothNavigationPane2.Text = "Cache"
        '
        '_navigationBar
        '
        Me._navigationBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me._navigationBar.GripDistance = 134
        Me._navigationBar.Location = New System.Drawing.Point(0, 0)
        Me._navigationBar.Name = "_navigationBar"
        Me._navigationBar.NavigationPanes.Add(Me.nuGenSmoothNavigationPane1)
        Me._navigationBar.NavigationPanes.Add(Me.nuGenSmoothNavigationPane2)
        Me._navigationBar.NavigationPanes.Add(Me.nuGenSmoothNavigationPane3)
        Me._navigationBar.NavigationPanes.Add(Me.nuGenSmoothNavigationPane4)
        Me._navigationBar.Size = New System.Drawing.Size(174, 266)
        Me._navigationBar.TabIndex = 0
        '
        'nuGenSmoothNavigationPane1
        '
        Me.nuGenSmoothNavigationPane1.Name = "nuGenSmoothNavigationPane1"
        Me.nuGenSmoothNavigationPane1.NavigationButtonImage = CType(resources.GetObject("nuGenSmoothNavigationPane1.NavigationButtonImage"), System.Drawing.Image)
        Me.nuGenSmoothNavigationPane1.TabIndex = 2
        Me.nuGenSmoothNavigationPane1.Text = "General"
        '
        '_splitContainer
        '
        Me._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me._splitContainer.Location = New System.Drawing.Point(0, 0)
        Me._splitContainer.Name = "_splitContainer"
        '
        '_splitContainer.Panel1
        '
        Me._splitContainer.Panel1.Controls.Add(Me._navigationBar)
        Me._splitContainer.Size = New System.Drawing.Size(522, 266)
        Me._splitContainer.SplitterDistance = 174
        Me._splitContainer.TabIndex = 2
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(522, 266)
        Me.Controls.Add(Me._splitContainer)
        Me.Name = "MainForm"
        Me.Text = "NavigationBar"
        Me._navigationBar.ResumeLayout(False)
        Me._splitContainer.Panel1.ResumeLayout(False)
        Me._splitContainer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents nuGenSmoothNavigationPane4 As Genetibase.SmoothControls.NuGenSmoothNavigationPane
    Private WithEvents nuGenSmoothNavigationPane3 As Genetibase.SmoothControls.NuGenSmoothNavigationPane
    Private WithEvents nuGenSmoothNavigationPane2 As Genetibase.SmoothControls.NuGenSmoothNavigationPane
    Private WithEvents _navigationBar As Genetibase.SmoothControls.NuGenSmoothNavigationBar
    Private WithEvents nuGenSmoothNavigationPane1 As Genetibase.SmoothControls.NuGenSmoothNavigationPane
    Private WithEvents _splitContainer As Genetibase.SmoothControls.NuGenSmoothSplitContainer

End Class

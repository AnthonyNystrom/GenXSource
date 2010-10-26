<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Firms
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Firms))
        Me.NavBarControl1 = New DevExpress.XtraNavBar.NavBarControl
        Me.NavBarGroup1 = New DevExpress.XtraNavBar.NavBarGroup
        Me.NavBarItem2 = New DevExpress.XtraNavBar.NavBarItem
        Me.NavBarItem5 = New DevExpress.XtraNavBar.NavBarItem
        Me.NavBarItem7 = New DevExpress.XtraNavBar.NavBarItem
        Me.NavBarItem4 = New DevExpress.XtraNavBar.NavBarItem
        Me.NavBarGroup2 = New DevExpress.XtraNavBar.NavBarGroup
        Me.NavBarItem3 = New DevExpress.XtraNavBar.NavBarItem
        Me.NavBarItem1 = New DevExpress.XtraNavBar.NavBarItem
        Me.NavBarGroup3 = New DevExpress.XtraNavBar.NavBarGroup
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.SpinEdit1 = New DevExpress.XtraEditors.SpinEdit
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        CType(Me.NavBarControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.SpinEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NavBarControl1
        '
        Me.NavBarControl1.ActiveGroup = Me.NavBarGroup1
        Me.NavBarControl1.AllowDrop = True
        resources.ApplyResources(Me.NavBarControl1, "NavBarControl1")
        Me.NavBarControl1.Groups.AddRange(New DevExpress.XtraNavBar.NavBarGroup() {Me.NavBarGroup1, Me.NavBarGroup2, Me.NavBarGroup3})
        Me.NavBarControl1.Items.AddRange(New DevExpress.XtraNavBar.NavBarItem() {Me.NavBarItem1, Me.NavBarItem2, Me.NavBarItem5, Me.NavBarItem3, Me.NavBarItem7, Me.NavBarItem4})
        Me.NavBarControl1.Name = "NavBarControl1"
        Me.NavBarControl1.View = New DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Money Twins")
        '
        'NavBarGroup1
        '
        resources.ApplyResources(Me.NavBarGroup1, "NavBarGroup1")
        Me.NavBarGroup1.Expanded = True
        Me.NavBarGroup1.ItemLinks.AddRange(New DevExpress.XtraNavBar.NavBarItemLink() {New DevExpress.XtraNavBar.NavBarItemLink(Me.NavBarItem2), New DevExpress.XtraNavBar.NavBarItemLink(Me.NavBarItem5), New DevExpress.XtraNavBar.NavBarItemLink(Me.NavBarItem7), New DevExpress.XtraNavBar.NavBarItemLink(Me.NavBarItem4)})
        Me.NavBarGroup1.Name = "NavBarGroup1"
        '
        'NavBarItem2
        '
        resources.ApplyResources(Me.NavBarItem2, "NavBarItem2")
        Me.NavBarItem2.Name = "NavBarItem2"
        '
        'NavBarItem5
        '
        resources.ApplyResources(Me.NavBarItem5, "NavBarItem5")
        Me.NavBarItem5.Name = "NavBarItem5"
        '
        'NavBarItem7
        '
        resources.ApplyResources(Me.NavBarItem7, "NavBarItem7")
        Me.NavBarItem7.Name = "NavBarItem7"
        '
        'NavBarItem4
        '
        resources.ApplyResources(Me.NavBarItem4, "NavBarItem4")
        Me.NavBarItem4.Name = "NavBarItem4"
        '
        'NavBarGroup2
        '
        resources.ApplyResources(Me.NavBarGroup2, "NavBarGroup2")
        Me.NavBarGroup2.ItemLinks.AddRange(New DevExpress.XtraNavBar.NavBarItemLink() {New DevExpress.XtraNavBar.NavBarItemLink(Me.NavBarItem3), New DevExpress.XtraNavBar.NavBarItemLink(Me.NavBarItem1)})
        Me.NavBarGroup2.Name = "NavBarGroup2"
        '
        'NavBarItem3
        '
        resources.ApplyResources(Me.NavBarItem3, "NavBarItem3")
        Me.NavBarItem3.Name = "NavBarItem3"
        '
        'NavBarItem1
        '
        resources.ApplyResources(Me.NavBarItem1, "NavBarItem1")
        Me.NavBarItem1.Name = "NavBarItem1"
        '
        'NavBarGroup3
        '
        resources.ApplyResources(Me.NavBarGroup3, "NavBarGroup3")
        Me.NavBarGroup3.Name = "NavBarGroup3"
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.SimpleButton1)
        Me.PanelControl1.Controls.Add(Me.Label2)
        Me.PanelControl1.Controls.Add(Me.Label1)
        Me.PanelControl1.Controls.Add(Me.SpinEdit1)
        resources.ApplyResources(Me.PanelControl1, "PanelControl1")
        Me.PanelControl1.LookAndFeel.SkinName = "Money Twins"
        Me.PanelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin
        Me.PanelControl1.LookAndFeel.UseDefaultLookAndFeel = False
        Me.PanelControl1.LookAndFeel.UseWindowsXPTheme = False
        Me.PanelControl1.Name = "PanelControl1"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        Me.Label2.UseCompatibleTextRendering = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        Me.Label1.UseCompatibleTextRendering = True
        '
        'SpinEdit1
        '
        resources.ApplyResources(Me.SpinEdit1, "SpinEdit1")
        Me.SpinEdit1.Name = "SpinEdit1"
        Me.SpinEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.SpinEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton})
        Me.SpinEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.SpinEdit1.Properties.IsFloatValue = False
        Me.SpinEdit1.Properties.LookAndFeel.SkinName = "Money Twins"
        Me.SpinEdit1.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin
        Me.SpinEdit1.Properties.LookAndFeel.UseDefaultLookAndFeel = False
        Me.SpinEdit1.Properties.LookAndFeel.UseWindowsXPTheme = False
        Me.SpinEdit1.Properties.Mask.EditMask = resources.GetString("SpinEdit1.Properties.Mask.EditMask")
        Me.SpinEdit1.Properties.MaxValue = New Decimal(New Integer() {500, 0, 0, 0})
        Me.SpinEdit1.Properties.MinValue = New Decimal(New Integer() {3, 0, 0, 0})
        Me.SpinEdit1.Properties.UseCtrlIncrement = False
        '
        'SimpleButton1
        '
        resources.ApplyResources(Me.SimpleButton1, "SimpleButton1")
        Me.SimpleButton1.LookAndFeel.SkinName = "Money Twins"
        Me.SimpleButton1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin
        Me.SimpleButton1.LookAndFeel.UseDefaultLookAndFeel = False
        Me.SimpleButton1.LookAndFeel.UseWindowsXPTheme = False
        Me.SimpleButton1.Name = "SimpleButton1"
        '
        'Form_Firms
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ControlBox = False
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.NavBarControl1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_Firms"
        Me.ShowInTaskbar = False
        CType(Me.NavBarControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.SpinEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NavBarControl1 As DevExpress.XtraNavBar.NavBarControl
    Friend WithEvents NavBarGroup1 As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents NavBarItem2 As DevExpress.XtraNavBar.NavBarItem
    Friend WithEvents NavBarGroup2 As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents NavBarItem1 As DevExpress.XtraNavBar.NavBarItem
    Friend WithEvents NavBarItem5 As DevExpress.XtraNavBar.NavBarItem
    Friend WithEvents NavBarItem3 As DevExpress.XtraNavBar.NavBarItem
    Friend WithEvents NavBarGroup3 As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents NavBarItem7 As DevExpress.XtraNavBar.NavBarItem
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents SpinEdit1 As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents NavBarItem4 As DevExpress.XtraNavBar.NavBarItem
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
End Class

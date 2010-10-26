Imports DevComponents.DotNetBar
Public Class frmMain
    Inherits System.Windows.Forms.Form
    Implements IDocumentUI

    Private m_Search As BalloonSearch

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
    Friend WithEvents barLeftDockSite As DevComponents.DotNetBar.DockSite
    Friend WithEvents barRightDockSite As DevComponents.DotNetBar.DockSite
    Friend WithEvents barTopDockSite As DevComponents.DotNetBar.DockSite
    Friend WithEvents barBottomDockSite As DevComponents.DotNetBar.DockSite
    Friend WithEvents dotNetBarManager1 As DevComponents.DotNetBar.DotNetBarManager
    Friend WithEvents imageList1 As System.Windows.Forms.ImageList
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents saveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tabStrip1 As DevComponents.DotNetBar.TabStrip
    Friend WithEvents labelStatus As DevComponents.DotNetBar.LabelItem
    Friend WithEvents labelPosition As DevComponents.DotNetBar.LabelItem
    Friend WithEvents itemProgressBar As DevComponents.DotNetBar.ProgressBarItem
    Private WithEvents barStatus As DevComponents.DotNetBar.Bar
    Friend WithEvents ribbonControl1 As DevComponents.DotNetBar.RibbonControl
    Friend WithEvents ribbonPanel1 As DevComponents.DotNetBar.RibbonPanel
    Friend WithEvents ribbonBar4 As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents ribbonBar3 As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents ribbonBar2 As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents ribbonBar1 As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents ribbonPanel2 As DevComponents.DotNetBar.RibbonPanel
    Friend WithEvents ribbonBar6 As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents ribbonPanel3 As DevComponents.DotNetBar.RibbonPanel
    Friend WithEvents ribbonBar5 As DevComponents.DotNetBar.RibbonBar
    Friend WithEvents buttonFind As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents itemContainer5 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents buttonReplace As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonGoto As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents itemContainer6 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents itemContainer7 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents buttonAlignLeft As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonAlignCenter As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonAlignRight As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonAlignJustify As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents itemContainer8 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents buttonItem2 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem3 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem4 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem5 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents itemContainer4 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents buttonItem6 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem7 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents itemContainer2 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents comboFont As DevComponents.DotNetBar.ComboBoxItem
    Friend WithEvents comboFontSize As DevComponents.DotNetBar.ComboBoxItem
    Friend WithEvents itemContainer3 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents buttonFontBold As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonFontItalic As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonFontUnderline As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonFontStrike As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonTextColor As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonPaste As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents itemContainer1 As DevComponents.DotNetBar.ItemContainer
    Friend WithEvents buttonCut As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonCopy As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem1 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem8 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem12 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonMargins As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem9 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem10 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonItem11 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonFile As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonFileOpen As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonFileSaveAs As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonExit As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonNew As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonSave As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonUndo As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ribbonTabItem1 As DevComponents.DotNetBar.RibbonTabItem
    Friend WithEvents ribbonTabItem3 As DevComponents.DotNetBar.RibbonTabItem
    Friend WithEvents ribbonTabContext As DevComponents.DotNetBar.RibbonTabItem
    Friend WithEvents buttonHelp As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonStyleOffice2003 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents buttonStyleOffice12 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents RibbonTabItemGroup1 As DevComponents.DotNetBar.RibbonTabItemGroup
    Friend WithEvents ComboItem1 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem2 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem3 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem4 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem5 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem6 As DevComponents.Editors.ComboItem
    Friend WithEvents ComboItem7 As DevComponents.Editors.ComboItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.dotNetBarManager1 = New DevComponents.DotNetBar.DotNetBarManager(Me.components)
        Me.barBottomDockSite = New DevComponents.DotNetBar.DockSite
        Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.barLeftDockSite = New DevComponents.DotNetBar.DockSite
        Me.barRightDockSite = New DevComponents.DotNetBar.DockSite
        Me.barTopDockSite = New DevComponents.DotNetBar.DockSite
        Me.tabStrip1 = New DevComponents.DotNetBar.TabStrip
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.saveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.barStatus = New DevComponents.DotNetBar.Bar
        Me.labelStatus = New DevComponents.DotNetBar.LabelItem
        Me.labelPosition = New DevComponents.DotNetBar.LabelItem
        Me.itemProgressBar = New DevComponents.DotNetBar.ProgressBarItem
        Me.ribbonControl1 = New DevComponents.DotNetBar.RibbonControl
        Me.ribbonPanel1 = New DevComponents.DotNetBar.RibbonPanel
        Me.ribbonBar4 = New DevComponents.DotNetBar.RibbonBar
        Me.buttonFind = New DevComponents.DotNetBar.ButtonItem
        Me.itemContainer5 = New DevComponents.DotNetBar.ItemContainer
        Me.buttonReplace = New DevComponents.DotNetBar.ButtonItem
        Me.buttonGoto = New DevComponents.DotNetBar.ButtonItem
        Me.ribbonBar3 = New DevComponents.DotNetBar.RibbonBar
        Me.itemContainer6 = New DevComponents.DotNetBar.ItemContainer
        Me.itemContainer7 = New DevComponents.DotNetBar.ItemContainer
        Me.buttonAlignLeft = New DevComponents.DotNetBar.ButtonItem
        Me.buttonAlignCenter = New DevComponents.DotNetBar.ButtonItem
        Me.buttonAlignRight = New DevComponents.DotNetBar.ButtonItem
        Me.buttonAlignJustify = New DevComponents.DotNetBar.ButtonItem
        Me.itemContainer8 = New DevComponents.DotNetBar.ItemContainer
        Me.buttonItem2 = New DevComponents.DotNetBar.ButtonItem
        Me.buttonItem3 = New DevComponents.DotNetBar.ButtonItem
        Me.buttonItem4 = New DevComponents.DotNetBar.ButtonItem
        Me.buttonItem5 = New DevComponents.DotNetBar.ButtonItem
        Me.itemContainer4 = New DevComponents.DotNetBar.ItemContainer
        Me.buttonItem6 = New DevComponents.DotNetBar.ButtonItem
        Me.buttonItem7 = New DevComponents.DotNetBar.ButtonItem
        Me.ribbonBar2 = New DevComponents.DotNetBar.RibbonBar
        Me.itemContainer2 = New DevComponents.DotNetBar.ItemContainer
        Me.comboFont = New DevComponents.DotNetBar.ComboBoxItem
        Me.comboFontSize = New DevComponents.DotNetBar.ComboBoxItem
        Me.ComboItem1 = New DevComponents.Editors.ComboItem
        Me.ComboItem2 = New DevComponents.Editors.ComboItem
        Me.ComboItem3 = New DevComponents.Editors.ComboItem
        Me.ComboItem4 = New DevComponents.Editors.ComboItem
        Me.ComboItem5 = New DevComponents.Editors.ComboItem
        Me.ComboItem6 = New DevComponents.Editors.ComboItem
        Me.ComboItem7 = New DevComponents.Editors.ComboItem
        Me.itemContainer3 = New DevComponents.DotNetBar.ItemContainer
        Me.buttonFontBold = New DevComponents.DotNetBar.ButtonItem
        Me.buttonFontItalic = New DevComponents.DotNetBar.ButtonItem
        Me.buttonFontUnderline = New DevComponents.DotNetBar.ButtonItem
        Me.buttonFontStrike = New DevComponents.DotNetBar.ButtonItem
        Me.buttonTextColor = New DevComponents.DotNetBar.ButtonItem
        Me.ribbonBar1 = New DevComponents.DotNetBar.RibbonBar
        Me.buttonPaste = New DevComponents.DotNetBar.ButtonItem
        Me.itemContainer1 = New DevComponents.DotNetBar.ItemContainer
        Me.buttonCut = New DevComponents.DotNetBar.ButtonItem
        Me.buttonCopy = New DevComponents.DotNetBar.ButtonItem
        Me.ribbonPanel3 = New DevComponents.DotNetBar.RibbonPanel
        Me.ribbonBar5 = New DevComponents.DotNetBar.RibbonBar
        Me.buttonMargins = New DevComponents.DotNetBar.ButtonItem
        Me.buttonItem9 = New DevComponents.DotNetBar.ButtonItem
        Me.buttonItem10 = New DevComponents.DotNetBar.ButtonItem
        Me.buttonItem11 = New DevComponents.DotNetBar.ButtonItem
        Me.ribbonPanel2 = New DevComponents.DotNetBar.RibbonPanel
        Me.buttonFile = New DevComponents.DotNetBar.ButtonItem
        Me.buttonFileOpen = New DevComponents.DotNetBar.ButtonItem
        Me.buttonFileSaveAs = New DevComponents.DotNetBar.ButtonItem
        Me.buttonExit = New DevComponents.DotNetBar.ButtonItem
        Me.buttonNew = New DevComponents.DotNetBar.ButtonItem
        Me.buttonSave = New DevComponents.DotNetBar.ButtonItem
        Me.buttonUndo = New DevComponents.DotNetBar.ButtonItem
        Me.ribbonTabItem1 = New DevComponents.DotNetBar.RibbonTabItem
        Me.ribbonTabItem3 = New DevComponents.DotNetBar.RibbonTabItem
        Me.ribbonTabContext = New DevComponents.DotNetBar.RibbonTabItem
        Me.RibbonTabItemGroup1 = New DevComponents.DotNetBar.RibbonTabItemGroup
        Me.buttonHelp = New DevComponents.DotNetBar.ButtonItem
        Me.buttonStyleOffice2003 = New DevComponents.DotNetBar.ButtonItem
        Me.buttonStyleOffice12 = New DevComponents.DotNetBar.ButtonItem
        Me.ribbonBar6 = New DevComponents.DotNetBar.RibbonBar
        Me.buttonItem1 = New DevComponents.DotNetBar.ButtonItem
        Me.buttonItem8 = New DevComponents.DotNetBar.ButtonItem
        Me.buttonItem12 = New DevComponents.DotNetBar.ButtonItem
        CType(Me.barStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ribbonControl1.SuspendLayout()
        Me.ribbonPanel1.SuspendLayout()
        Me.ribbonPanel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'dotNetBarManager1
        '
        Me.dotNetBarManager1.BottomDockSite = Me.barBottomDockSite
        Me.dotNetBarManager1.DefinitionName = ""
        Me.dotNetBarManager1.HideMdiSystemMenu = True
        Me.dotNetBarManager1.Images = Me.imageList1
        Me.dotNetBarManager1.LeftDockSite = Me.barLeftDockSite
        Me.dotNetBarManager1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.dotNetBarManager1.MdiSystemItemVisible = False
        Me.dotNetBarManager1.ParentForm = Me
        Me.dotNetBarManager1.RightDockSite = Me.barRightDockSite
        Me.dotNetBarManager1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003
        Me.dotNetBarManager1.ThemeAware = False
        Me.dotNetBarManager1.TopDockSite = Me.barTopDockSite
        '
        'barBottomDockSite
        '
        Me.barBottomDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.barBottomDockSite.BackgroundImageAlpha = CType(255, Byte)
        Me.barBottomDockSite.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barBottomDockSite.DocumentDockContainer = Nothing
        Me.barBottomDockSite.Location = New System.Drawing.Point(0, 715)
        Me.barBottomDockSite.Name = "barBottomDockSite"
        Me.barBottomDockSite.NeedsLayout = False
        Me.barBottomDockSite.Size = New System.Drawing.Size(1016, 0)
        Me.barBottomDockSite.TabIndex = 4
        Me.barBottomDockSite.TabStop = False
        '
        'imageList1
        '
        Me.imageList1.ImageStream = CType(resources.GetObject("imageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageList1.TransparentColor = System.Drawing.Color.Magenta
        Me.imageList1.Images.SetKeyName(0, "")
        Me.imageList1.Images.SetKeyName(1, "")
        Me.imageList1.Images.SetKeyName(2, "")
        Me.imageList1.Images.SetKeyName(3, "")
        Me.imageList1.Images.SetKeyName(4, "")
        Me.imageList1.Images.SetKeyName(5, "")
        Me.imageList1.Images.SetKeyName(6, "")
        Me.imageList1.Images.SetKeyName(7, "")
        Me.imageList1.Images.SetKeyName(8, "")
        Me.imageList1.Images.SetKeyName(9, "")
        Me.imageList1.Images.SetKeyName(10, "")
        Me.imageList1.Images.SetKeyName(11, "")
        Me.imageList1.Images.SetKeyName(12, "")
        Me.imageList1.Images.SetKeyName(13, "")
        Me.imageList1.Images.SetKeyName(14, "")
        Me.imageList1.Images.SetKeyName(15, "")
        Me.imageList1.Images.SetKeyName(16, "")
        Me.imageList1.Images.SetKeyName(17, "")
        Me.imageList1.Images.SetKeyName(18, "")
        Me.imageList1.Images.SetKeyName(19, "")
        Me.imageList1.Images.SetKeyName(20, "")
        Me.imageList1.Images.SetKeyName(21, "")
        Me.imageList1.Images.SetKeyName(22, "")
        Me.imageList1.Images.SetKeyName(23, "")
        '
        'barLeftDockSite
        '
        Me.barLeftDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.barLeftDockSite.BackgroundImageAlpha = CType(255, Byte)
        Me.barLeftDockSite.Dock = System.Windows.Forms.DockStyle.Left
        Me.barLeftDockSite.DocumentDockContainer = Nothing
        Me.barLeftDockSite.Location = New System.Drawing.Point(0, 0)
        Me.barLeftDockSite.Name = "barLeftDockSite"
        Me.barLeftDockSite.NeedsLayout = False
        Me.barLeftDockSite.Size = New System.Drawing.Size(0, 715)
        Me.barLeftDockSite.TabIndex = 1
        Me.barLeftDockSite.TabStop = False
        '
        'barRightDockSite
        '
        Me.barRightDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.barRightDockSite.BackgroundImageAlpha = CType(255, Byte)
        Me.barRightDockSite.Dock = System.Windows.Forms.DockStyle.Right
        Me.barRightDockSite.DocumentDockContainer = Nothing
        Me.barRightDockSite.Location = New System.Drawing.Point(1016, 0)
        Me.barRightDockSite.Name = "barRightDockSite"
        Me.barRightDockSite.NeedsLayout = False
        Me.barRightDockSite.Size = New System.Drawing.Size(0, 715)
        Me.barRightDockSite.TabIndex = 2
        Me.barRightDockSite.TabStop = False
        '
        'barTopDockSite
        '
        Me.barTopDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.barTopDockSite.BackgroundImageAlpha = CType(255, Byte)
        Me.barTopDockSite.Dock = System.Windows.Forms.DockStyle.Top
        Me.barTopDockSite.DocumentDockContainer = Nothing
        Me.barTopDockSite.Location = New System.Drawing.Point(0, 0)
        Me.barTopDockSite.Name = "barTopDockSite"
        Me.barTopDockSite.NeedsLayout = False
        Me.barTopDockSite.Size = New System.Drawing.Size(1016, 0)
        Me.barTopDockSite.TabIndex = 3
        Me.barTopDockSite.TabStop = False
        '
        'tabStrip1
        '
        Me.tabStrip1.CanReorderTabs = True
        Me.tabStrip1.CloseButtonVisible = True
        Me.dotNetBarManager1.SetContextMenuEx(Me.tabStrip1, "bTabContext")
        Me.tabStrip1.Dock = System.Windows.Forms.DockStyle.Top
        Me.tabStrip1.Location = New System.Drawing.Point(0, 108)
        Me.tabStrip1.MdiTabbedDocuments = True
        Me.tabStrip1.Name = "tabStrip1"
        Me.tabStrip1.SelectedTab = Nothing
        Me.tabStrip1.SelectedTabFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabStrip1.Size = New System.Drawing.Size(1016, 26)
        Me.tabStrip1.Style = DevComponents.DotNetBar.eTabStripStyle.OneNote
        Me.tabStrip1.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Top
        Me.tabStrip1.TabIndex = 7
        Me.tabStrip1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox
        Me.tabStrip1.Text = "tabStrip1"
        '
        'saveFileDialog1
        '
        Me.saveFileDialog1.FileName = "doc1"
        '
        'barStatus
        '
        Me.barStatus.AccessibleDescription = "DotNetBar Bar (barStatus)"
        Me.barStatus.AccessibleName = "DotNetBar Bar"
        Me.barStatus.AccessibleRole = System.Windows.Forms.AccessibleRole.StatusBar
        Me.barStatus.BackgroundImageAlpha = CType(255, Byte)
        Me.barStatus.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barStatus.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.ResizeHandle
        Me.barStatus.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.labelStatus, Me.labelPosition, Me.itemProgressBar})
        Me.barStatus.ItemSpacing = 2
        Me.barStatus.Location = New System.Drawing.Point(0, 715)
        Me.barStatus.Name = "barStatus"
        Me.barStatus.Size = New System.Drawing.Size(1016, 19)
        Me.barStatus.Stretch = True
        Me.barStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.barStatus.TabIndex = 9
        Me.barStatus.TabStop = False
        '
        'labelStatus
        '
        Me.labelStatus.BackColor = System.Drawing.Color.Empty
        Me.labelStatus.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.labelStatus.DividerStyle = False
        Me.labelStatus.Font = Nothing
        Me.labelStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.labelStatus.Name = "labelStatus"
        Me.labelStatus.PaddingBottom = 0
        Me.labelStatus.PaddingLeft = 2
        Me.labelStatus.PaddingRight = 2
        Me.labelStatus.PaddingTop = 0
        Me.labelStatus.SingleLineColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(97, Byte), Integer), CType(CType(156, Byte), Integer))
        Me.labelStatus.Stretch = True
        Me.labelStatus.TextAlignment = System.Drawing.StringAlignment.Near
        Me.labelStatus.TextLineAlignment = System.Drawing.StringAlignment.Center
        '
        'labelPosition
        '
        Me.labelPosition.BackColor = System.Drawing.Color.Empty
        Me.labelPosition.BorderType = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.labelPosition.DividerStyle = False
        Me.labelPosition.Font = Nothing
        Me.labelPosition.ForeColor = System.Drawing.SystemColors.ControlText
        Me.labelPosition.Name = "labelPosition"
        Me.labelPosition.PaddingBottom = 0
        Me.labelPosition.PaddingLeft = 2
        Me.labelPosition.PaddingRight = 2
        Me.labelPosition.PaddingTop = 0
        Me.labelPosition.SingleLineColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(97, Byte), Integer), CType(CType(156, Byte), Integer))
        Me.labelPosition.TextAlignment = System.Drawing.StringAlignment.Near
        Me.labelPosition.TextLineAlignment = System.Drawing.StringAlignment.Center
        Me.labelPosition.Width = 100
        '
        'itemProgressBar
        '
        Me.itemProgressBar.ChunkColor = System.Drawing.Color.FromArgb(CType(CType(183, Byte), Integer), CType(CType(214, Byte), Integer), CType(CType(253, Byte), Integer))
        Me.itemProgressBar.ChunkColor2 = System.Drawing.Color.FromArgb(CType(CType(209, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(253, Byte), Integer))
        Me.itemProgressBar.ChunkGradientAngle = 90.0!
        Me.itemProgressBar.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways
        Me.itemProgressBar.Name = "itemProgressBar"
        Me.itemProgressBar.RecentlyUsed = False
        Me.itemProgressBar.Text = "progressBarItem1"
        '
        'ribbonControl1
        '
        '
        '
        '
        Me.ribbonControl1.BackgroundStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.ribbonControl1.BackgroundStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(187, Byte), Integer), CType(CType(186, Byte), Integer), CType(CType(186, Byte), Integer))
        Me.ribbonControl1.Controls.Add(Me.ribbonPanel1)
        Me.ribbonControl1.Controls.Add(Me.ribbonPanel3)
        Me.ribbonControl1.Controls.Add(Me.ribbonPanel2)
        Me.ribbonControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.ribbonControl1.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonFile, Me.buttonNew, Me.buttonSave, Me.buttonUndo, Me.ribbonTabItem1, Me.ribbonTabItem3, Me.ribbonTabContext, Me.buttonHelp})
        Me.ribbonControl1.Location = New System.Drawing.Point(0, 0)
        Me.ribbonControl1.Name = "ribbonControl1"
        Me.ribbonControl1.Size = New System.Drawing.Size(1016, 108)
        Me.ribbonControl1.TabGroupHeight = 14
        Me.ribbonControl1.TabGroups.AddRange(New DevComponents.DotNetBar.RibbonTabItemGroup() {Me.RibbonTabItemGroup1})
        Me.ribbonControl1.TabGroupsVisible = True
        Me.ribbonControl1.TabIndex = 11
        '
        'ribbonPanel1
        '
        Me.ribbonPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.ribbonPanel1.Controls.Add(Me.ribbonBar4)
        Me.ribbonPanel1.Controls.Add(Me.ribbonBar3)
        Me.ribbonPanel1.Controls.Add(Me.ribbonBar2)
        Me.ribbonPanel1.Controls.Add(Me.ribbonBar1)
        Me.ribbonPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ribbonPanel1.Location = New System.Drawing.Point(0, 37)
        Me.ribbonPanel1.Name = "ribbonPanel1"
        Me.ribbonPanel1.Size = New System.Drawing.Size(1016, 71)
        Me.ribbonPanel1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.ribbonPanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ribbonPanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.ribbonPanel1.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile
        Me.ribbonPanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.ribbonPanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ribbonPanel1.Style.GradientAngle = 90
        Me.ribbonPanel1.TabIndex = 1
        '
        'ribbonBar4
        '
        Me.ribbonBar4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ribbonBar4.AntiAlias = True
        Me.ribbonBar4.AutoOverflowEnabled = True
        '
        '
        '
        Me.ribbonBar4.BackgroundStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.ribbonBar4.BackgroundStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(201, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.ribbonBar4.BackgroundStyle.BackColorGradientAngle = 90
        Me.ribbonBar4.DialogLauncherVisible = True
        Me.ribbonBar4.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonFind, Me.itemContainer5})
        Me.ribbonBar4.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.ribbonBar4.Location = New System.Drawing.Point(436, 0)
        Me.ribbonBar4.Name = "ribbonBar4"
        Me.ribbonBar4.Size = New System.Drawing.Size(580, 71)
        Me.ribbonBar4.TabIndex = 3
        Me.ribbonBar4.Text = "Find"
        '
        '
        '
        Me.ribbonBar4.TitleStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(106, Byte), Integer), CType(CType(112, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ribbonBar4.TitleStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(178, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ribbonBar4.TitleStyle.BackColorGradientAngle = 90
        Me.ribbonBar4.TitleStyle.PaddingBottom = 2
        Me.ribbonBar4.TitleStyle.PaddingLeft = 2
        Me.ribbonBar4.TitleStyle.PaddingRight = 2
        Me.ribbonBar4.TitleStyle.PaddingTop = 3
        Me.ribbonBar4.TitleStyle.TextColor = System.Drawing.Color.White
        Me.ribbonBar4.TitleStyle.TextShadowColor = System.Drawing.Color.Black
        Me.ribbonBar4.TitleStyle.TextShadowOffset = New System.Drawing.Point(1, 1)
        '
        'buttonFind
        '
        Me.buttonFind.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonFind.Enabled = False
        Me.buttonFind.Image = CType(resources.GetObject("buttonFind.Image"), System.Drawing.Image)
        Me.buttonFind.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.buttonFind.Name = "buttonFind"
        Me.buttonFind.Text = "Find"
        '
        'itemContainer5
        '
        Me.itemContainer5.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.itemContainer5.Name = "itemContainer5"
        Me.itemContainer5.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonReplace, Me.buttonGoto})
        '
        'buttonReplace
        '
        Me.buttonReplace.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonReplace.Enabled = False
        Me.buttonReplace.Image = CType(resources.GetObject("buttonReplace.Image"), System.Drawing.Image)
        Me.buttonReplace.Name = "buttonReplace"
        Me.buttonReplace.Text = "Replace"
        '
        'buttonGoto
        '
        Me.buttonGoto.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonGoto.Enabled = False
        Me.buttonGoto.Image = CType(resources.GetObject("buttonGoto.Image"), System.Drawing.Image)
        Me.buttonGoto.Name = "buttonGoto"
        Me.buttonGoto.Text = "Goto"
        '
        'ribbonBar3
        '
        Me.ribbonBar3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ribbonBar3.AntiAlias = True
        Me.ribbonBar3.AutoOverflowEnabled = True
        '
        '
        '
        Me.ribbonBar3.BackgroundStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.ribbonBar3.BackgroundStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(201, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.ribbonBar3.BackgroundStyle.BackColorGradientAngle = 90
        Me.ribbonBar3.DialogLauncherVisible = True
        Me.ribbonBar3.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.itemContainer6, Me.itemContainer4})
        Me.ribbonBar3.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.ribbonBar3.Location = New System.Drawing.Point(237, 0)
        Me.ribbonBar3.Name = "ribbonBar3"
        Me.ribbonBar3.Size = New System.Drawing.Size(197, 71)
        Me.ribbonBar3.TabIndex = 2
        Me.ribbonBar3.Text = "Paragraph"
        '
        '
        '
        Me.ribbonBar3.TitleStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(106, Byte), Integer), CType(CType(112, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ribbonBar3.TitleStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(178, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ribbonBar3.TitleStyle.BackColorGradientAngle = 90
        Me.ribbonBar3.TitleStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.ribbonBar3.TitleStyle.BorderBottomWidth = 1
        Me.ribbonBar3.TitleStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ribbonBar3.TitleStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.ribbonBar3.TitleStyle.BorderLeftWidth = 1
        Me.ribbonBar3.TitleStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.ribbonBar3.TitleStyle.BorderRightWidth = 1
        Me.ribbonBar3.TitleStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.ribbonBar3.TitleStyle.BorderTopWidth = 1
        Me.ribbonBar3.TitleStyle.PaddingBottom = 2
        Me.ribbonBar3.TitleStyle.PaddingLeft = 2
        Me.ribbonBar3.TitleStyle.PaddingRight = 2
        Me.ribbonBar3.TitleStyle.PaddingTop = 3
        Me.ribbonBar3.TitleStyle.TextColor = System.Drawing.Color.White
        Me.ribbonBar3.TitleStyle.TextShadowColor = System.Drawing.Color.Black
        Me.ribbonBar3.TitleStyle.TextShadowOffset = New System.Drawing.Point(1, 1)
        '
        'itemContainer6
        '
        Me.itemContainer6.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.itemContainer6.Name = "itemContainer6"
        Me.itemContainer6.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.itemContainer7, Me.itemContainer8})
        '
        'itemContainer7
        '
        Me.itemContainer7.Name = "itemContainer7"
        Me.itemContainer7.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonAlignLeft, Me.buttonAlignCenter, Me.buttonAlignRight, Me.buttonAlignJustify})
        '
        'buttonAlignLeft
        '
        Me.buttonAlignLeft.Enabled = False
        Me.buttonAlignLeft.Image = CType(resources.GetObject("buttonAlignLeft.Image"), System.Drawing.Image)
        Me.buttonAlignLeft.Name = "buttonAlignLeft"
        Me.buttonAlignLeft.Text = "Align Left"
        '
        'buttonAlignCenter
        '
        Me.buttonAlignCenter.Enabled = False
        Me.buttonAlignCenter.Image = CType(resources.GetObject("buttonAlignCenter.Image"), System.Drawing.Image)
        Me.buttonAlignCenter.Name = "buttonAlignCenter"
        Me.buttonAlignCenter.Text = "Align Center"
        '
        'buttonAlignRight
        '
        Me.buttonAlignRight.Enabled = False
        Me.buttonAlignRight.Image = CType(resources.GetObject("buttonAlignRight.Image"), System.Drawing.Image)
        Me.buttonAlignRight.Name = "buttonAlignRight"
        Me.buttonAlignRight.Text = "Align Right"
        '
        'buttonAlignJustify
        '
        Me.buttonAlignJustify.Enabled = False
        Me.buttonAlignJustify.Image = CType(resources.GetObject("buttonAlignJustify.Image"), System.Drawing.Image)
        Me.buttonAlignJustify.Name = "buttonAlignJustify"
        Me.buttonAlignJustify.Text = "Justify"
        '
        'itemContainer8
        '
        Me.itemContainer8.Name = "itemContainer8"
        Me.itemContainer8.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonItem2, Me.buttonItem3, Me.buttonItem4, Me.buttonItem5})
        '
        'buttonItem2
        '
        Me.buttonItem2.Image = CType(resources.GetObject("buttonItem2.Image"), System.Drawing.Image)
        Me.buttonItem2.Name = "buttonItem2"
        Me.buttonItem2.Text = "buttonItem2"
        '
        'buttonItem3
        '
        Me.buttonItem3.Image = CType(resources.GetObject("buttonItem3.Image"), System.Drawing.Image)
        Me.buttonItem3.Name = "buttonItem3"
        Me.buttonItem3.Text = "buttonItem3"
        '
        'buttonItem4
        '
        Me.buttonItem4.Image = CType(resources.GetObject("buttonItem4.Image"), System.Drawing.Image)
        Me.buttonItem4.Name = "buttonItem4"
        Me.buttonItem4.Text = "buttonItem4"
        '
        'buttonItem5
        '
        Me.buttonItem5.Image = CType(resources.GetObject("buttonItem5.Image"), System.Drawing.Image)
        Me.buttonItem5.Name = "buttonItem5"
        Me.buttonItem5.Text = "buttonItem5"
        '
        'itemContainer4
        '
        Me.itemContainer4.BeginGroup = True
        Me.itemContainer4.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.itemContainer4.Name = "itemContainer4"
        Me.itemContainer4.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonItem6, Me.buttonItem7})
        '
        'buttonItem6
        '
        Me.buttonItem6.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonItem6.Image = CType(resources.GetObject("buttonItem6.Image"), System.Drawing.Image)
        Me.buttonItem6.Name = "buttonItem6"
        Me.buttonItem6.Text = "Borders"
        '
        'buttonItem7
        '
        Me.buttonItem7.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonItem7.Image = CType(resources.GetObject("buttonItem7.Image"), System.Drawing.Image)
        Me.buttonItem7.Name = "buttonItem7"
        Me.buttonItem7.Text = "Shading"
        '
        'ribbonBar2
        '
        Me.ribbonBar2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ribbonBar2.AntiAlias = True
        Me.ribbonBar2.AutoOverflowEnabled = True
        '
        '
        '
        Me.ribbonBar2.BackgroundStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.ribbonBar2.BackgroundStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(201, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.ribbonBar2.BackgroundStyle.BackColorGradientAngle = 90
        Me.ribbonBar2.DialogLauncherVisible = True
        Me.ribbonBar2.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.itemContainer2, Me.itemContainer3})
        Me.ribbonBar2.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.ribbonBar2.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.ribbonBar2.Location = New System.Drawing.Point(74, 0)
        Me.ribbonBar2.Name = "ribbonBar2"
        Me.ribbonBar2.Size = New System.Drawing.Size(161, 71)
        Me.ribbonBar2.TabIndex = 1
        Me.ribbonBar2.Text = "Font"
        '
        '
        '
        Me.ribbonBar2.TitleStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(106, Byte), Integer), CType(CType(112, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ribbonBar2.TitleStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(178, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ribbonBar2.TitleStyle.BackColorGradientAngle = 90
        Me.ribbonBar2.TitleStyle.PaddingBottom = 2
        Me.ribbonBar2.TitleStyle.PaddingLeft = 2
        Me.ribbonBar2.TitleStyle.PaddingRight = 2
        Me.ribbonBar2.TitleStyle.PaddingTop = 3
        Me.ribbonBar2.TitleStyle.TextColor = System.Drawing.Color.White
        Me.ribbonBar2.TitleStyle.TextShadowColor = System.Drawing.Color.Black
        Me.ribbonBar2.TitleStyle.TextShadowOffset = New System.Drawing.Point(1, 1)
        '
        'itemContainer2
        '
        Me.itemContainer2.Name = "itemContainer2"
        Me.itemContainer2.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.comboFont, Me.comboFontSize})
        '
        'comboFont
        '
        Me.comboFont.ComboWidth = 96
        Me.comboFont.Enabled = False
        Me.comboFont.FontCombo = True
        Me.comboFont.ItemHeight = 12
        Me.comboFont.Name = "comboFont"
        '
        'comboFontSize
        '
        Me.comboFontSize.ComboWidth = 36
        Me.comboFontSize.Enabled = False
        Me.comboFontSize.ItemHeight = 12
        Me.comboFontSize.Items.AddRange(New Object() {Me.ComboItem1, Me.ComboItem2, Me.ComboItem3, Me.ComboItem4, Me.ComboItem5, Me.ComboItem6, Me.ComboItem7})
        Me.comboFontSize.Name = "comboFontSize"
        '
        'ComboItem1
        '
        Me.ComboItem1.Text = "6"
        '
        'ComboItem2
        '
        Me.ComboItem2.Text = "7"
        '
        'ComboItem3
        '
        Me.ComboItem3.Text = "8"
        '
        'ComboItem4
        '
        Me.ComboItem4.Text = "9"
        '
        'ComboItem5
        '
        Me.ComboItem5.Text = "10"
        '
        'ComboItem6
        '
        Me.ComboItem6.Text = "11"
        '
        'ComboItem7
        '
        Me.ComboItem7.Text = "12"
        '
        'itemContainer3
        '
        Me.itemContainer3.Name = "itemContainer3"
        Me.itemContainer3.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonFontBold, Me.buttonFontItalic, Me.buttonFontUnderline, Me.buttonFontStrike, Me.buttonTextColor})
        '
        'buttonFontBold
        '
        Me.buttonFontBold.Enabled = False
        Me.buttonFontBold.Image = CType(resources.GetObject("buttonFontBold.Image"), System.Drawing.Image)
        Me.buttonFontBold.Name = "buttonFontBold"
        Me.buttonFontBold.Text = "Bold"
        '
        'buttonFontItalic
        '
        Me.buttonFontItalic.Enabled = False
        Me.buttonFontItalic.Image = CType(resources.GetObject("buttonFontItalic.Image"), System.Drawing.Image)
        Me.buttonFontItalic.Name = "buttonFontItalic"
        Me.buttonFontItalic.Text = "Italic"
        '
        'buttonFontUnderline
        '
        Me.buttonFontUnderline.Enabled = False
        Me.buttonFontUnderline.Image = CType(resources.GetObject("buttonFontUnderline.Image"), System.Drawing.Image)
        Me.buttonFontUnderline.Name = "buttonFontUnderline"
        Me.buttonFontUnderline.Text = "Underline"
        '
        'buttonFontStrike
        '
        Me.buttonFontStrike.Enabled = False
        Me.buttonFontStrike.Image = CType(resources.GetObject("buttonFontStrike.Image"), System.Drawing.Image)
        Me.buttonFontStrike.Name = "buttonFontStrike"
        Me.buttonFontStrike.Text = "Strike"
        '
        'buttonTextColor
        '
        Me.buttonTextColor.Enabled = False
        Me.buttonTextColor.Image = CType(resources.GetObject("buttonTextColor.Image"), System.Drawing.Image)
        Me.buttonTextColor.Name = "buttonTextColor"
        Me.buttonTextColor.PopupType = DevComponents.DotNetBar.ePopupType.Container
        Me.buttonTextColor.Text = "Text Color"
        '
        'ribbonBar1
        '
        Me.ribbonBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ribbonBar1.AntiAlias = True
        Me.ribbonBar1.AutoOverflowEnabled = True
        '
        '
        '
        Me.ribbonBar1.BackgroundStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.ribbonBar1.BackgroundStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(201, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.ribbonBar1.BackgroundStyle.BackColorGradientAngle = 90
        Me.ribbonBar1.DialogLauncherVisible = True
        Me.ribbonBar1.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonPaste, Me.itemContainer1})
        Me.ribbonBar1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.ribbonBar1.Location = New System.Drawing.Point(0, 0)
        Me.ribbonBar1.Name = "ribbonBar1"
        Me.ribbonBar1.Size = New System.Drawing.Size(72, 71)
        Me.ribbonBar1.TabIndex = 0
        Me.ribbonBar1.Text = "Clipboard"
        '
        '
        '
        Me.ribbonBar1.TitleStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(106, Byte), Integer), CType(CType(112, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ribbonBar1.TitleStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(178, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ribbonBar1.TitleStyle.BackColorGradientAngle = 90
        Me.ribbonBar1.TitleStyle.PaddingBottom = 2
        Me.ribbonBar1.TitleStyle.PaddingLeft = 2
        Me.ribbonBar1.TitleStyle.PaddingRight = 2
        Me.ribbonBar1.TitleStyle.PaddingTop = 3
        Me.ribbonBar1.TitleStyle.TextColor = System.Drawing.Color.White
        Me.ribbonBar1.TitleStyle.TextShadowColor = System.Drawing.Color.Black
        Me.ribbonBar1.TitleStyle.TextShadowOffset = New System.Drawing.Point(1, 1)
        '
        'buttonPaste
        '
        Me.buttonPaste.Enabled = False
        Me.buttonPaste.Image = CType(resources.GetObject("buttonPaste.Image"), System.Drawing.Image)
        Me.buttonPaste.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.buttonPaste.Name = "buttonPaste"
        Me.buttonPaste.Text = "Paste"
        '
        'itemContainer1
        '
        Me.itemContainer1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
        Me.itemContainer1.Name = "itemContainer1"
        Me.itemContainer1.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonCut, Me.buttonCopy})
        '
        'buttonCut
        '
        Me.buttonCut.Enabled = False
        Me.buttonCut.Image = CType(resources.GetObject("buttonCut.Image"), System.Drawing.Image)
        Me.buttonCut.Name = "buttonCut"
        Me.buttonCut.Text = "Cut"
        '
        'buttonCopy
        '
        Me.buttonCopy.Enabled = False
        Me.buttonCopy.Image = CType(resources.GetObject("buttonCopy.Image"), System.Drawing.Image)
        Me.buttonCopy.Name = "buttonCopy"
        Me.buttonCopy.Text = "Copy"
        '
        'ribbonPanel3
        '
        Me.ribbonPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.ribbonPanel3.Controls.Add(Me.ribbonBar5)
        Me.ribbonPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ribbonPanel3.Location = New System.Drawing.Point(0, 0)
        Me.ribbonPanel3.Name = "ribbonPanel3"
        Me.ribbonPanel3.Size = New System.Drawing.Size(1016, 108)
        Me.ribbonPanel3.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.ribbonPanel3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ribbonPanel3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.ribbonPanel3.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile
        Me.ribbonPanel3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.ribbonPanel3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ribbonPanel3.Style.GradientAngle = 90
        Me.ribbonPanel3.TabIndex = 3
        Me.ribbonPanel3.Visible = False
        '
        'ribbonBar5
        '
        Me.ribbonBar5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ribbonBar5.AntiAlias = True
        Me.ribbonBar5.AutoOverflowEnabled = True
        '
        '
        '
        Me.ribbonBar5.BackgroundStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.ribbonBar5.BackgroundStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(201, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.ribbonBar5.BackgroundStyle.BackColorGradientAngle = 90
        Me.ribbonBar5.DialogLauncherVisible = True
        Me.ribbonBar5.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonMargins, Me.buttonItem9, Me.buttonItem10, Me.buttonItem11})
        Me.ribbonBar5.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.ribbonBar5.Location = New System.Drawing.Point(0, 0)
        Me.ribbonBar5.Name = "ribbonBar5"
        Me.ribbonBar5.Size = New System.Drawing.Size(1016, 108)
        Me.ribbonBar5.TabIndex = 1
        Me.ribbonBar5.Text = "Page Setup"
        '
        '
        '
        Me.ribbonBar5.TitleStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(106, Byte), Integer), CType(CType(112, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ribbonBar5.TitleStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(178, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ribbonBar5.TitleStyle.BackColorGradientAngle = 90
        Me.ribbonBar5.TitleStyle.PaddingBottom = 2
        Me.ribbonBar5.TitleStyle.PaddingLeft = 2
        Me.ribbonBar5.TitleStyle.PaddingRight = 2
        Me.ribbonBar5.TitleStyle.PaddingTop = 3
        Me.ribbonBar5.TitleStyle.TextColor = System.Drawing.Color.White
        Me.ribbonBar5.TitleStyle.TextShadowColor = System.Drawing.Color.Black
        Me.ribbonBar5.TitleStyle.TextShadowOffset = New System.Drawing.Point(1, 1)
        '
        'buttonMargins
        '
        Me.buttonMargins.Image = CType(resources.GetObject("buttonMargins.Image"), System.Drawing.Image)
        Me.buttonMargins.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.buttonMargins.Name = "buttonMargins"
        Me.buttonMargins.Text = "Margins"
        '
        'buttonItem9
        '
        Me.buttonItem9.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonItem9.Image = CType(resources.GetObject("buttonItem9.Image"), System.Drawing.Image)
        Me.buttonItem9.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.buttonItem9.Name = "buttonItem9"
        Me.buttonItem9.Text = "Orientation"
        '
        'buttonItem10
        '
        Me.buttonItem10.Image = CType(resources.GetObject("buttonItem10.Image"), System.Drawing.Image)
        Me.buttonItem10.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.buttonItem10.Name = "buttonItem10"
        Me.buttonItem10.Text = "Size"
        '
        'buttonItem11
        '
        Me.buttonItem11.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonItem11.Image = CType(resources.GetObject("buttonItem11.Image"), System.Drawing.Image)
        Me.buttonItem11.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.buttonItem11.Name = "buttonItem11"
        Me.buttonItem11.Text = "Print Area"
        '
        'ribbonPanel2
        '
        Me.ribbonPanel2.Location = New System.Drawing.Point(0, 0)
        Me.ribbonPanel2.Name = "ribbonPanel2"
        Me.ribbonPanel2.Size = New System.Drawing.Size(200, 100)
        Me.ribbonPanel2.TabIndex = 4
        Me.ribbonPanel2.Visible = False
        '
        'buttonFile
        '
        Me.buttonFile.AutoExpandOnClick = True
        Me.buttonFile.Name = "buttonFile"
        Me.buttonFile.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonFileOpen, Me.buttonFileSaveAs, Me.buttonExit})
        Me.buttonFile.Text = "File"
        '
        'buttonFileOpen
        '
        Me.buttonFileOpen.Image = CType(resources.GetObject("buttonFileOpen.Image"), System.Drawing.Image)
        Me.buttonFileOpen.Name = "buttonFileOpen"
        Me.buttonFileOpen.Text = "&Open..."
        '
        'buttonFileSaveAs
        '
        Me.buttonFileSaveAs.BeginGroup = True
        Me.buttonFileSaveAs.Enabled = False
        Me.buttonFileSaveAs.Name = "buttonFileSaveAs"
        Me.buttonFileSaveAs.Text = "Save As..."
        '
        'buttonExit
        '
        Me.buttonExit.BeginGroup = True
        Me.buttonExit.Name = "buttonExit"
        Me.buttonExit.Text = "&Exit"
        '
        'buttonNew
        '
        Me.buttonNew.BeginGroup = True
        Me.buttonNew.Image = CType(resources.GetObject("buttonNew.Image"), System.Drawing.Image)
        Me.buttonNew.Name = "buttonNew"
        Me.buttonNew.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlN)
        Me.buttonNew.Text = "New Document"
        '
        'buttonSave
        '
        Me.buttonSave.Enabled = False
        Me.buttonSave.Image = CType(resources.GetObject("buttonSave.Image"), System.Drawing.Image)
        Me.buttonSave.Name = "buttonSave"
        Me.buttonSave.Text = "buttonItem2"
        '
        'buttonUndo
        '
        Me.buttonUndo.Enabled = False
        Me.buttonUndo.Image = CType(resources.GetObject("buttonUndo.Image"), System.Drawing.Image)
        Me.buttonUndo.Name = "buttonUndo"
        Me.buttonUndo.Text = "Undo"
        '
        'ribbonTabItem1
        '
        Me.ribbonTabItem1.Checked = True
        Me.ribbonTabItem1.Name = "ribbonTabItem1"
        Me.ribbonTabItem1.Panel = Me.ribbonPanel1
        Me.ribbonTabItem1.Text = "Write"
        '
        'ribbonTabItem3
        '
        Me.ribbonTabItem3.Name = "ribbonTabItem3"
        Me.ribbonTabItem3.Panel = Me.ribbonPanel3
        Me.ribbonTabItem3.Text = "Page Layout"
        '
        'ribbonTabContext
        '
        Me.ribbonTabContext.Group = Me.RibbonTabItemGroup1
        Me.ribbonTabContext.Name = "ribbonTabContext"
        Me.ribbonTabContext.Panel = Me.ribbonPanel2
        Me.ribbonTabContext.Text = "Contex Tab"
        Me.ribbonTabContext.Visible = False
        '
        'RibbonTabItemGroup1
        '
        Me.RibbonTabItemGroup1.GroupTitle = "Tab Group"
        '
        '
        '
        Me.RibbonTabItemGroup1.Style.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(159, Byte), Integer))
        Me.RibbonTabItemGroup1.Style.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(225, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.RibbonTabItemGroup1.Style.BackColorGradientAngle = 90
        Me.RibbonTabItemGroup1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.RibbonTabItemGroup1.Style.BorderBottomWidth = 1
        Me.RibbonTabItemGroup1.Style.BorderColor = System.Drawing.Color.FromArgb(CType(CType(154, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(59, Byte), Integer))
        Me.RibbonTabItemGroup1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.RibbonTabItemGroup1.Style.BorderLeftWidth = 1
        Me.RibbonTabItemGroup1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.RibbonTabItemGroup1.Style.BorderRightWidth = 1
        Me.RibbonTabItemGroup1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.RibbonTabItemGroup1.Style.BorderTopWidth = 1
        Me.RibbonTabItemGroup1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.RibbonTabItemGroup1.Style.TextColor = System.Drawing.Color.Black
        Me.RibbonTabItemGroup1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        'buttonHelp
        '
        Me.buttonHelp.AutoExpandOnClick = True
        Me.buttonHelp.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far
        Me.buttonHelp.Name = "buttonHelp"
        Me.buttonHelp.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonStyleOffice2003, Me.buttonStyleOffice12})
        Me.buttonHelp.Text = "Style"
        '
        'buttonStyleOffice2003
        '
        Me.buttonStyleOffice2003.Name = "buttonStyleOffice2003"
        Me.buttonStyleOffice2003.OptionGroup = "style"
        Me.buttonStyleOffice2003.Text = "Office 2003"
        '
        'buttonStyleOffice12
        '
        Me.buttonStyleOffice12.Checked = True
        Me.buttonStyleOffice12.Name = "buttonStyleOffice12"
        Me.buttonStyleOffice12.OptionGroup = "style"
        Me.buttonStyleOffice12.Text = "Office 12"
        '
        'ribbonBar6
        '
        Me.ribbonBar6.AntiAlias = True
        Me.ribbonBar6.AutoOverflowEnabled = True
        '
        '
        '
        Me.ribbonBar6.BackgroundStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.ribbonBar6.BackgroundStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(201, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.ribbonBar6.BackgroundStyle.BackColorGradientAngle = 90
        Me.ribbonBar6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ribbonBar6.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.buttonItem1, Me.buttonItem8, Me.buttonItem12})
        Me.ribbonBar6.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.ribbonBar6.Location = New System.Drawing.Point(0, 0)
        Me.ribbonBar6.Name = "ribbonBar6"
        Me.ribbonBar6.Size = New System.Drawing.Size(1016, 108)
        Me.ribbonBar6.TabIndex = 0
        Me.ribbonBar6.Text = "Contextual Ribbon"
        '
        '
        '
        Me.ribbonBar6.TitleStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(106, Byte), Integer), CType(CType(112, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ribbonBar6.TitleStyle.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(178, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ribbonBar6.TitleStyle.BackColorGradientAngle = 90
        Me.ribbonBar6.TitleStyle.PaddingBottom = 2
        Me.ribbonBar6.TitleStyle.PaddingLeft = 2
        Me.ribbonBar6.TitleStyle.PaddingRight = 2
        Me.ribbonBar6.TitleStyle.PaddingTop = 3
        Me.ribbonBar6.TitleStyle.TextColor = System.Drawing.Color.White
        Me.ribbonBar6.TitleStyle.TextShadowColor = System.Drawing.Color.Black
        Me.ribbonBar6.TitleStyle.TextShadowOffset = New System.Drawing.Point(1, 1)
        '
        'buttonItem1
        '
        Me.buttonItem1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonItem1.Image = CType(resources.GetObject("buttonItem1.Image"), System.Drawing.Image)
        Me.buttonItem1.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.buttonItem1.Name = "buttonItem1"
        Me.buttonItem1.Text = "Command 1"
        '
        'buttonItem8
        '
        Me.buttonItem8.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonItem8.Image = CType(resources.GetObject("buttonItem8.Image"), System.Drawing.Image)
        Me.buttonItem8.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.buttonItem8.Name = "buttonItem8"
        Me.buttonItem8.Text = "Command 2"
        '
        'buttonItem12
        '
        Me.buttonItem12.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
        Me.buttonItem12.Image = CType(resources.GetObject("buttonItem12.Image"), System.Drawing.Image)
        Me.buttonItem12.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.buttonItem12.Name = "buttonItem12"
        Me.buttonItem12.Text = "Command 3"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1016, 734)
        Me.Controls.Add(Me.tabStrip1)
        Me.Controls.Add(Me.ribbonControl1)
        Me.Controls.Add(Me.barLeftDockSite)
        Me.Controls.Add(Me.barRightDockSite)
        Me.Controls.Add(Me.barTopDockSite)
        Me.Controls.Add(Me.barBottomDockSite)
        Me.Controls.Add(Me.barStatus)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.IsMdiContainer = True
        Me.Name = "frmMain"
        Me.Text = "RibbonPad Sample Project"
        CType(Me.barStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ribbonControl1.ResumeLayout(False)
        Me.ribbonPanel1.ResumeLayout(False)
        Me.ribbonPanel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub PopupContainerLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles ribbonBar2.PopupContainerLoad
        Dim item As ButtonItem = CType(sender, ButtonItem)

        If item Is Nothing Then Exit Sub

        If item.Name = "buttonTextColor" Then
            Dim container As PopupContainerControl = CType(item.PopupContainerControl, PopupContainerControl)
            Dim clr As ColorPicker = New ColorPicker()
            container.Controls.Add(clr)
            clr.Location = container.ClientRectangle.Location
            container.ClientSize = clr.Size
        End If

    End Sub

    Private Sub PopupContainerUnload(ByVal sender As Object, ByVal e As System.EventArgs) Handles ribbonBar2.PopupContainerUnload
        Dim item As ButtonItem = CType(sender, ButtonItem)
        Dim clr As ColorPicker
        Dim container As PopupContainerControl

        If item Is Nothing Then Exit Sub

        If item.Name = "buttonTextColor" Then
            container = CType(item.PopupContainerControl, PopupContainerControl)
            clr = CType(container.Controls(0), ColorPicker)
            If Not Color.Empty.Equals(clr.SelectedColor) Then
                Dim activedocument As frmDocument = CType(Me.ActiveMdiChild, frmDocument)
                'If Not activedocument Is Nothing Then
                '    activedocument.ExecuteCommand(item.Name, clr.SelectedColor)
                'End If
            End If
        End If
    End Sub

    Public Sub EditContextMenu()
        Dim objItem As ButtonItem = CType(dotNetBarManager1.ContextMenus("bEditPopup"), ButtonItem)
        objItem.Displayed = False
        objItem.PopupMenu(Control.MousePosition)
    End Sub

    Private Sub dotNetBarManager1_ItemClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dotNetBarManager1.ItemClick
        Dim objItem As BaseItem = CType(sender, BaseItem)
        Dim activedocument As frmDocument = CType(Me.ActiveMdiChild, frmDocument)

        Select Case objItem.Name
            ' File menu
        Case "bNew", "bNewWindow"
                CreateNewDocument()

            Case "bTaskNewDocument"
                CreateNewDocument()

            Case "bOpen"
                OpenDocument()

            Case "bTaskOpenDocument"
                OpenDocument()

            Case "bSave"
                SaveDocument()

            Case "bSaveAs"
                SaveDocumentAs()

            Case "bClose"
                If Not activedocument Is Nothing Then
                    activedocument.Close()
                End If

            Case "bPageSetup"
                MessageBox.Show("Not implemented yet.")

            Case "bPrintPreview"
                MessageBox.Show("Not implemented yet.")

            Case "bPrint"
                MessageBox.Show("Not implemented yet.")

            Case "bExit"
                Me.Close()

                ' Window menu
            Case "bArrangeAll"
                Me.LayoutMdi(MdiLayout.Cascade)

            Case "window_list"
                CType(objItem.Tag, Form).Activate()

                ' Style switching
            Case "bSwitchStyle"
                If objItem.Style = eDotNetBarStyle.Office2000 Then
                    dotNetBarManager1.Style = eDotNetBarStyle.OfficeXP
                Else
                    dotNetBarManager1.Style = eDotNetBarStyle.Office2000
                End If
            Case "bTaskNewFromExisting"
                MessageBox.Show("Not implemented.")

            Case "bTaskHelp"
                MessageBox.Show("Starts the help file.")

            Case "bTaskSampleDoc"
                MessageBox.Show("This is only a sample item that when clicked should open most recently used document.")
            Case "bThemes"
                EnableThemes(CType(objItem, ButtonItem))
            Case "bStyle2000"
                ChangeDotNetBarStyle(eDotNetBarStyle.Office2000)
            Case "bStyle2003"
                ChangeDotNetBarStyle(eDotNetBarStyle.Office2003)
            Case "bStyleXP"
                ChangeDotNetBarStyle(eDotNetBarStyle.OfficeXP)
            Case "bVS2005"
                ChangeDotNetBarStyle(eDotNetBarStyle.VS2005)
            Case "bFind"
                If m_Search Is Nothing OrElse m_Search.IsDisposed Then
                    m_Search = New BalloonSearch()
                    m_Search.Owner = Me
                    m_Search.Show(objItem, True)
                End If
            Case Else
                ' Pass them to the active document
                'If Not activedocument Is Nothing Then
                '    activedocument.ExecuteCommand(objItem.Name, Nothing)
                'End If
        End Select
    End Sub

    Private Sub CreateNewDocument()
        Dim doc As frmDocument = New frmDocument()
        doc.MdiParent = Me
        doc.WindowState = FormWindowState.Maximized
        doc.Show()
        doc.Update()
        doc.Text = "New Document " + Me.MdiChildren.Length.ToString()
    End Sub

    Private Sub EnableFileItems()
        ' Accessing items through the Items collection and setting the properties on them
        ' will propagate certain properties to all items with the same name...
        If Me.ActiveMdiChild Is Nothing Then
            dotNetBarManager1.Items("bSave").Enabled = False
            dotNetBarManager1.Items("bSaveAs").Enabled = False
            dotNetBarManager1.Items("bPrint").Enabled = False
            dotNetBarManager1.Items("bPrintPreview").Enabled = False
            dotNetBarManager1.Items("bPageSetup").Enabled = False
        Else
            dotNetBarManager1.Items("bSave").Enabled = True
            dotNetBarManager1.Items("bSaveAs").Enabled = True
            dotNetBarManager1.Items("bPrint").Enabled = True
            dotNetBarManager1.Items("bPrintPreview").Enabled = True
            dotNetBarManager1.Items("bPageSetup").Enabled = True
        End If
    End Sub


    Private Sub frmMain_MdiChildActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MdiChildActivate
        EnableFileItems()
    End Sub

    Private Sub OpenDocument()
        openFileDialog1.FileName = ""
        openFileDialog1.ShowDialog()
        If openFileDialog1.FileName = "" Then Exit Sub
        Dim doc As frmDocument = New frmDocument()
        doc.Text = openFileDialog1.FileName
        doc.MdiParent = Me
        doc.Show()
        'doc.OpenFile(openFileDialog1.FileName)
    End Sub

    Public Sub SaveDocument()
        If Me.ActiveMdiChild Is Nothing Then Exit Sub
        Dim doc As frmDocument = CType(Me.ActiveMdiChild, frmDocument)

        If doc Is Nothing Then Exit Sub
        If Not doc.DocumentChanged Then Exit Sub
        'SaveDocument(doc)
    End Sub

    'Public Sub SaveDocument(ByVal doc As frmDocument)
    '    If doc.FileName = "" Then
    '        Dim dr As DialogResult = saveFileDialog1.ShowDialog()
    '        If saveFileDialog1.FileName = "" Then Exit Sub
    '        If dr = DialogResult.OK Then doc.FileName = saveFileDialog1.FileName
    '    End If
    '    If doc.FileName <> "" Then doc.SaveFile()
    'End Sub

    Private Sub SaveDocumentAs()
        If Me.ActiveMdiChild Is Nothing Then Exit Sub
        Dim doc As frmDocument = CType(Me.ActiveMdiChild, frmDocument)

        If doc Is Nothing Then Exit Sub
        If Not doc.DocumentChanged Then Exit Sub
        If doc.FileName = "" Then
            'SaveDocument(doc)
            Exit Sub
        End If

        saveFileDialog1.ShowDialog()

        If saveFileDialog1.FileName = "" Then Exit Sub

        doc.FileName = saveFileDialog1.FileName
        'doc.SaveFile()
    End Sub


    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ChangeDotNetBarStyle(eDotNetBarStyle.Office2007)

        ' Neccessary for Automatic Tab-Strip Mdi support
        tabStrip1.MdiForm = Me

        Dim c As Control
        For Each c In Me.Controls
            If TypeOf (c) Is MdiClient Then
                AddHandler c.ControlAdded, AddressOf MdiClientControlAddRemove
                AddHandler c.ControlRemoved, AddressOf MdiClientControlAddRemove
            End If
        Next

        AddHandler ribbonBar2.ItemClick, AddressOf ribbonBar1_ItemClick
        AddHandler ribbonBar3.ItemClick, AddressOf ribbonBar1_ItemClick
        AddHandler ribbonBar4.ItemClick, AddressOf ribbonBar1_ItemClick
    End Sub

    Private Sub MdiClientControlAddRemove(ByVal sender As Object, ByVal e As ControlEventArgs)
        If Me.MdiChildren.Length > 0 Then
            If Not ribbonTabContext.Visible Then
                ribbonTabContext.Visible = True
                ribbonControl1.RecalcLayout()
            End If
        Else
            If ribbonTabContext.Visible Then
                If ribbonTabContext.Checked Then
                    ribbonTabItem1.Checked = True
                End If
                ribbonTabContext.Visible = False
                ribbonControl1.RecalcLayout()
            End If
        End If
    End Sub

    Private Sub EnableThemes(ByVal item As ButtonItem)
        Dim bEnable As Boolean = Not item.Checked
        item.Checked = bEnable
        If bEnable Then
            item.Text = "Disable Themes"
        Else
            item.Text = "Enable Themes"
        End If

        dotNetBarManager1.ThemeAware = bEnable
        Me.Refresh()
    End Sub

    Private Sub ChangeDotNetBarStyle(ByVal style As eDotNetBarStyle)
        dotNetBarManager1.Style = style

        ' Status bar style
        barStatus.Style = style

        ' Ribbon
        ribbonControl1.Style = style
        If style = eDotNetBarStyle.Office2007 Then
            RibbonPredefinedColorSchemes.ApplyGrayColorScheme(ribbonControl1)
            ribbonControl1.Height = 108
        Else
            RibbonPredefinedColorSchemes.ApplyOffice2003ColorScheme(ribbonControl1)
            ribbonControl1.Height = 100
        End If
        ribbonControl1.Refresh()
        ribbonBar1.Style = style
        ribbonBar2.Style = style
        ribbonBar3.Style = style
        ribbonBar4.Style = style
        ribbonBar5.Style = style
        ribbonBar6.Style = style

        If style = eDotNetBarStyle.Office2003 Then
            tabStrip1.ColorScheme.ResetChangedFlag()
            tabStrip1.Style = eTabStripStyle.OneNote
        ElseIf style = eDotNetBarStyle.VS2005 Or style = eDotNetBarStyle.Office2007 Then
            tabStrip1.Style = eTabStripStyle.OneNote
            SetOffice12TabStripColorScheme()
        Else
            tabStrip1.Style = eTabStripStyle.Flat
        End If
    End Sub

    Private Sub SetOffice12TabStripColorScheme()
        Dim scheme As ColorScheme = ribbonBar1.ColorScheme
        tabStrip1.ColorScheme.TabBackground = scheme.BarBackground
        tabStrip1.ColorScheme.TabBackground2 = scheme.BarBackground2
        tabStrip1.ColorScheme.TabItemSelectedBackground = scheme.BarBackground2
        tabStrip1.ColorScheme.TabItemSelectedBackground2 = scheme.BarBackground
        tabStrip1.ColorScheme.TabItemBackground = scheme.BarBackground
        tabStrip1.ColorScheme.TabItemBackground2 = scheme.BarBackground2
        tabStrip1.ColorScheme.TabItemHotBackground = scheme.ItemHotBackground
        tabStrip1.ColorScheme.TabItemHotBackground2 = scheme.ItemHotBackground2
        tabStrip1.Refresh()
    End Sub

    Private Sub frmMain_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Move
        CloseSearch()
    End Sub

    Private Sub CloseSearch()
        If (Not m_Search Is Nothing) Then
            m_Search.Close()
            m_Search.Dispose()
            m_Search = Nothing
        End If
    End Sub

    Public Sub SearchActiveDocument(ByVal text As String)
        Dim activedocument As frmDocument = CType(Me.ActiveMdiChild, frmDocument)
        If Not activedocument Is Nothing Then
            'activedocument.ExecuteCommand("bFind", text)
        End If
    End Sub

    Private Sub buttonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonFind.Click
        If m_Search Is Nothing Or m_Search.IsDisposed Then
            m_Search = New BalloonSearch()
            m_Search.Owner = Me
            m_Search.Show(CType(sender, BaseItem), True)
        End If
    End Sub

    Private Sub buttonFileOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonFileOpen.Click
        OpenDocument()
    End Sub


    Private Sub buttonFileSaveAs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonFileSaveAs.Click
        SaveDocumentAs()
    End Sub


    Private Sub buttonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonSave.Click
        SaveDocument()
    End Sub


    Private Sub buttonExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonExit.Click
        Me.Close()
    End Sub


    Private Sub buttonNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNew.Click
        CreateNewDocument()
    End Sub


    Private Sub ribbonBar1_ItemClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ribbonBar1.ItemClick
        Dim item As BaseItem = CType(sender, BaseItem)
        Dim activeDocument As frmDocument = CType(Me.ActiveMdiChild, frmDocument)
        If activeDocument Is Nothing Then Exit Sub
        'activeDocument.ExecuteCommand(item.Name, sender)
    End Sub

#Region "IDocumentUI Implementation"
    Property CutEnabled() As Boolean Implements IDocumentUI.CutEnabled
        Get
            Return buttonCut.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonCut.Enabled = Value
        End Set
    End Property
    Property CopyEnabled() As Boolean Implements IDocumentUI.CopyEnabled
        Get
            Return buttonCopy.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonCopy.Enabled = Value
        End Set
    End Property
    Property DeleteEnabled() As Boolean Implements IDocumentUI.DeleteEnabled
        Get

        End Get
        Set(ByVal Value As Boolean)

        End Set
    End Property

    Property PasteEnabled() As Boolean Implements IDocumentUI.PasteEnabled
        Get
            Return buttonPaste.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonPaste.Enabled = Value
        End Set
    End Property
    Property SelectAllEnabled() As Boolean Implements IDocumentUI.SelectAllEnabled
        Get

        End Get
        Set(ByVal Value As Boolean)

        End Set
    End Property
    Property FindEnabled() As Boolean Implements IDocumentUI.FindEnabled
        Get
            Return buttonFind.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonFind.Enabled = Value
        End Set
    End Property
    Property FindNextEnabled() As Boolean Implements IDocumentUI.FindNextEnabled
        Get
            Return False
        End Get
        Set(ByVal Value As Boolean)

        End Set
    End Property
    Property ReplaceEnabled() As Boolean Implements IDocumentUI.ReplaceEnabled
        Get
            Return buttonReplace.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonReplace.Enabled = Value
        End Set
    End Property

    Property TextColorEnabled() As Boolean Implements IDocumentUI.TextColorEnabled
        Get
            Return buttonTextColor.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonTextColor.Enabled = Value
        End Set
    End Property

    Property BoldChecked() As Boolean Implements IDocumentUI.BoldChecked
        Get
            Return buttonFontBold.Checked
        End Get
        Set(ByVal Value As Boolean)
            buttonFontBold.Checked = Value
        End Set
    End Property
    Property ItalicChecked() As Boolean Implements IDocumentUI.ItalicChecked
        Get
            Return buttonFontItalic.Checked
        End Get
        Set(ByVal Value As Boolean)
            buttonFontItalic.Checked = Value
        End Set
    End Property
    Property UnderlineChecked() As Boolean Implements IDocumentUI.UnderlineChecked
        Get
            Return buttonFontUnderline.Checked
        End Get
        Set(ByVal Value As Boolean)
            buttonFontUnderline.Checked = Value
        End Set
    End Property
    Property StrikethroughChecked() As Boolean Implements IDocumentUI.StrikethroughChecked
        Get
            Return buttonFontStrike.Checked
        End Get
        Set(ByVal Value As Boolean)
            buttonFontStrike.Checked = Value
        End Set
    End Property
    Property AlignLeftChecked() As Boolean Implements IDocumentUI.AlignLeftChecked
        Get
            Return buttonAlignLeft.Checked
        End Get
        Set(ByVal Value As Boolean)
            buttonAlignLeft.Checked = Value
        End Set
    End Property
    Property AlignRightChecked() As Boolean Implements IDocumentUI.AlignRightChecked
        Get
            Return buttonAlignRight.Checked
        End Get
        Set(ByVal Value As Boolean)
            buttonAlignRight.Checked = Value
        End Set
    End Property
    Property AlignCenterChecked() As Boolean Implements IDocumentUI.AlignCenterChecked
        Get
            Return buttonAlignCenter.Checked
        End Get
        Set(ByVal Value As Boolean)
            buttonAlignCenter.Checked = Value
        End Set
    End Property

    Property BoldEnabled() As Boolean Implements IDocumentUI.BoldEnabled
        Get
            Return buttonFontBold.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonFontBold.Enabled = Value
        End Set
    End Property
    Property ItalicEnabled() As Boolean Implements IDocumentUI.ItalicEnabled
        Get
            Return buttonFontItalic.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonFontItalic.Enabled = Value
        End Set
    End Property
    Property UnderlineEnabled() As Boolean Implements IDocumentUI.UnderlineEnabled
        Get
            Return buttonFontUnderline.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonFontUnderline.Enabled = Value
        End Set
    End Property
    Property StrikethroughEnabled() As Boolean Implements IDocumentUI.StrikethroughEnabled
        Get
            Return buttonFontStrike.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonFontStrike.Enabled = Value
        End Set
    End Property
    Property AlignLeftEnabled() As Boolean Implements IDocumentUI.AlignLeftEnabled
        Get
            Return buttonAlignLeft.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonAlignLeft.Enabled = Value
        End Set
    End Property
    Property AlignRightEnabled() As Boolean Implements IDocumentUI.AlignRightEnabled
        Get
            Return buttonAlignRight.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonAlignRight.Enabled = Value
        End Set
    End Property
    Property AlignCenterEnabled() As Boolean Implements IDocumentUI.AlignCenterEnabled
        Get
            Return buttonAlignCenter.Enabled
        End Get
        Set(ByVal Value As Boolean)
            buttonAlignCenter.Enabled = Value
        End Set
    End Property

    Property FontSelectionEnabled() As Boolean Implements IDocumentUI.FontSelectionEnabled
        Get
            Return comboFont.Enabled
        End Get
        Set(ByVal Value As Boolean)
            comboFont.Enabled = Value
        End Set
    End Property
    Sub SetSelectionFont(ByVal font As Font) Implements IDocumentUI.SetSelectionFont
        If font Is Nothing Then
            comboFont.SelectedIndex = -1
        ElseIf comboFont.SelectedItem Is Nothing OrElse comboFont.SelectedItem.ToString() <> font.Name Then
            comboFont.SelectedIndex = comboFont.ComboBoxEx.FindStringExact(font.Name)
        End If
    End Sub
    Property FontSizeEnabled() As Boolean Implements IDocumentUI.FontSizeEnabled
        Get
            Return comboFontSize.Enabled
        End Get
        Set(ByVal Value As Boolean)
            comboFontSize.Enabled = Value
        End Set
    End Property
    Property FontSize() As Int32 Implements IDocumentUI.FontSize
        Get
            Return Int32.Parse(comboFontSize.SelectedItem.ToString())
        End Get
        Set(ByVal Value As Int32)
            Dim s As String = Value.ToString()
            If Not comboFontSize.SelectedItem Is Nothing AndAlso comboFontSize.SelectedItem.ToString() = s Then Exit Property

            Dim index As Int32 = -1, i As Int32 = 0
            For i = 0 To comboFontSize.Items.Count - 1
                If comboFontSize.Items(i).ToString() = s Then
                    index = i
                    Exit For
                End If
            Next
            comboFontSize.SelectedIndex = index
        End Set
    End Property
#End Region

    Private Sub buttonStyleOffice2003_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonStyleOffice2003.Click
        ChangeDotNetBarStyle(eDotNetBarStyle.Office2003)
    End Sub

    Private Sub buttonStyleOffice12_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonStyleOffice12.Click
        ChangeDotNetBarStyle(eDotNetBarStyle.Office2007)
    End Sub
End Class

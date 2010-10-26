<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NuGenC1PrintPreviewDialog
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
        Me.components = New System.ComponentModel.Container
        Dim C1DocEngine1 As C1.C1PrintDocument.C1DocEngine = New C1.C1PrintDocument.C1DocEngine
        Dim Root1 As C1.C1PrintDocument.DocEngine.Render.Root = New C1.C1PrintDocument.DocEngine.Render.Root
        Dim DrawelRoot1 As C1.C1PrintDocument.DocEngine.Src.DrawelRoot = New C1.C1PrintDocument.DocEngine.Src.DrawelRoot
        Dim DrawelText1 As C1.C1PrintDocument.DocEngine.Src.DrawelText = New C1.C1PrintDocument.DocEngine.Src.DrawelText
        Dim C1DColors1 As C1.C1PrintDocument.DocEngine.Src.C1DColors = New C1.C1PrintDocument.DocEngine.Src.C1DColors
        Dim Widths1 As C1.C1PrintDocument.DocEngine.Src.Widths = New C1.C1PrintDocument.DocEngine.Src.Widths
        Dim Widths2 As C1.C1PrintDocument.DocEngine.Src.Widths = New C1.C1PrintDocument.DocEngine.Src.Widths
        Dim DrawelText2 As C1.C1PrintDocument.DocEngine.Src.DrawelText = New C1.C1PrintDocument.DocEngine.Src.DrawelText
        Dim C1DColors2 As C1.C1PrintDocument.DocEngine.Src.C1DColors = New C1.C1PrintDocument.DocEngine.Src.C1DColors
        Dim Widths3 As C1.C1PrintDocument.DocEngine.Src.Widths = New C1.C1PrintDocument.DocEngine.Src.Widths
        Dim Widths4 As C1.C1PrintDocument.DocEngine.Src.Widths = New C1.C1PrintDocument.DocEngine.Src.Widths
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NuGenC1PrintPreviewDialog))
        Dim UiStrings1 As C1.C1PrintDocument.UIStrings = New C1.C1PrintDocument.UIStrings
        Me.C1PrintPreviewControl = New C1.Win.C1PrintPreview.C1PrintPreview
        Me.c1pBtnFileOpen1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnFileSave1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnFilePrint1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnPageSetup1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnReflow1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnStop1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnDocInfo1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnSeparator1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnShowNavigationBar1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnSeparator2 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnMouseHand1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnMouseZoom1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnMouseZoomOut1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnMouseSelect1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnFindText1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnSeparator3 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnGoFirst1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnGoPrev1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnGoNext1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnGoLast1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnSeparator4 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnHistoryPrev1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnHistoryNext1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnSeparator5 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnZoomOut1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnZoomIn1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnSeparator6 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnViewActualSize1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnViewFullPage1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnViewPageWidth1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnViewTwoPages1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnViewFourPages1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnSeparator7 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.c1pBtnHelp1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        Me.imgImageList1 = New System.Windows.Forms.ImageList(Me.components)
        CType(Me.C1PrintPreviewControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'C1PrintPreviewControl
        '
        Me.C1PrintPreviewControl.BackColor = System.Drawing.SystemColors.Control
        Me.C1PrintPreviewControl.C1DPageSettings = "color:True;landscape:False;margins:100,100,100,100;papersize:850,1100,TABlAHQAdAB" & _
            "lAHIA"
        Me.C1PrintPreviewControl.Dock = System.Windows.Forms.DockStyle.Fill
        C1DocEngine1.DisplayName = ""
        C1DocEngine1.Name = ""
        C1DocEngine1.rnd = Root1
        DrawelRoot1.bc = Nothing
        DrawelRoot1.bo = Nothing
        DrawelRoot1.dct = Nothing
        DrawelRoot1.f = New System.Drawing.Font("Arial", 20.0!)
        DrawelRoot1.hh = Nothing
        DrawelRoot1.i = CType(35, Long)
        DrawelRoot1.ib = Nothing
        DrawelRoot1.iba = Nothing
        DrawelRoot1.mi = Nothing
        DrawelRoot1.n = "root"
        DrawelRoot1.oi = Nothing
        DrawelRoot1.pa = Nothing
        C1DColors1.Bottom = System.Drawing.Color.LightGreen
        C1DColors1.Left = System.Drawing.Color.LightGreen
        C1DColors1.Right = System.Drawing.Color.LightGreen
        C1DColors1.Top = System.Drawing.Color.Gray
        DrawelText1.bc = C1DColors1
        DrawelText1.bl = 1
        DrawelText1.bo = Widths1
        DrawelText1.cb = System.Drawing.Color.GhostWhite
        DrawelText1.cf = System.Drawing.Color.Blue
        DrawelText1.f = New System.Drawing.Font("Arial", 18.0!)
        DrawelText1.hh = Nothing
        DrawelText1.i = CType(37, Long)
        DrawelText1.ib = Nothing
        DrawelText1.iba = Nothing
        DrawelText1.mi = Nothing
        DrawelText1.n = "text"
        DrawelText1.oi = Nothing
        DrawelText1.pa = Widths2
        DrawelText1.s = Nothing
        DrawelText1.sp = Nothing
        DrawelText1.t = "Generated on Friday, April 14, 2006."
        DrawelText1.th = C1.C1PrintDocument.DocEngine.Src.Names.HorzTextAlignment.right
        DrawelText1.ud = Nothing
        DrawelText1.ww = Nothing
        DrawelText1.xx = Nothing
        DrawelText1.yy = Nothing
        DrawelRoot1.pf = DrawelText1
        C1DColors2.Bottom = System.Drawing.Color.Gray
        C1DColors2.Left = System.Drawing.Color.LightGreen
        C1DColors2.Right = System.Drawing.Color.LightGreen
        C1DColors2.Top = System.Drawing.Color.LightGreen
        DrawelText2.bc = C1DColors2
        DrawelText2.bl = 1
        DrawelText2.bo = Widths3
        DrawelText2.cb = System.Drawing.Color.GhostWhite
        DrawelText2.cf = System.Drawing.Color.Blue
        DrawelText2.f = New System.Drawing.Font("Arial", 18.0!)
        DrawelText2.gn = "#p"
        DrawelText2.gt = "#P"
        DrawelText2.hh = Nothing
        DrawelText2.i = CType(36, Long)
        DrawelText2.ib = Nothing
        DrawelText2.iba = Nothing
        DrawelText2.mi = Nothing
        DrawelText2.n = "text"
        DrawelText2.oi = Nothing
        DrawelText2.pa = Widths4
        DrawelText2.s = Nothing
        DrawelText2.sp = Nothing
        DrawelText2.t = "Page #p of #P"
        DrawelText2.th = C1.C1PrintDocument.DocEngine.Src.Names.HorzTextAlignment.right
        DrawelText2.ud = Nothing
        DrawelText2.ww = Nothing
        DrawelText2.xx = Nothing
        DrawelText2.yy = Nothing
        DrawelRoot1.ph = DrawelText2
        DrawelRoot1.ps = Nothing
        DrawelRoot1.rf = True
        DrawelRoot1.s = "block,ttb"
        DrawelRoot1.sp = Nothing
        DrawelRoot1.ud = CType(resources.GetObject("DrawelRoot1.ud"), Object)
        DrawelRoot1.ww = Nothing
        DrawelRoot1.xx = Nothing
        DrawelRoot1.yy = Nothing
        C1DocEngine1.src = DrawelRoot1
        UiStrings1.Content = New String(-1) {}
        C1DocEngine1.UIStrings = UiStrings1
        Me.C1PrintPreviewControl.Document = C1DocEngine1
        Me.C1PrintPreviewControl.Location = New System.Drawing.Point(0, 0)
        Me.C1PrintPreviewControl.Name = "C1PrintPreviewControl"
        Me.C1PrintPreviewControl.NavigationBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreviewControl.NavigationBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.C1PrintPreviewControl.NavigationBar.OutlineView.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreviewControl.NavigationBar.OutlineView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.C1PrintPreviewControl.NavigationBar.OutlineView.Indent = 19
        Me.C1PrintPreviewControl.NavigationBar.OutlineView.ItemHeight = 16
        Me.C1PrintPreviewControl.NavigationBar.OutlineView.TabIndex = 0
        Me.C1PrintPreviewControl.NavigationBar.OutlineView.Visible = False
        Me.C1PrintPreviewControl.NavigationBar.Padding = New System.Drawing.Point(6, 3)
        Me.C1PrintPreviewControl.NavigationBar.TabIndex = 2
        Me.C1PrintPreviewControl.NavigationBar.ThumbnailsView.AutoArrange = True
        Me.C1PrintPreviewControl.NavigationBar.ThumbnailsView.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreviewControl.NavigationBar.ThumbnailsView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.C1PrintPreviewControl.NavigationBar.ThumbnailsView.TabIndex = 0
        Me.C1PrintPreviewControl.NavigationBar.ThumbnailsView.Visible = True
        Me.C1PrintPreviewControl.NavigationBar.Width = 160
        Me.C1PrintPreviewControl.Size = New System.Drawing.Size(792, 566)
        Me.C1PrintPreviewControl.Splitter.Cursor = System.Windows.Forms.Cursors.VSplit
        Me.C1PrintPreviewControl.Splitter.Width = 3
        Me.C1PrintPreviewControl.StatusBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreviewControl.StatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.C1PrintPreviewControl.StatusBar.TabIndex = 4
        Me.C1PrintPreviewControl.TabIndex = 0
        Me.C1PrintPreviewControl.ToolBar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.c1pBtnFileOpen1, Me.c1pBtnFileSave1, Me.c1pBtnFilePrint1, Me.c1pBtnPageSetup1, Me.c1pBtnReflow1, Me.c1pBtnStop1, Me.c1pBtnDocInfo1, Me.c1pBtnSeparator1, Me.c1pBtnShowNavigationBar1, Me.c1pBtnSeparator2, Me.c1pBtnMouseHand1, Me.c1pBtnMouseZoom1, Me.c1pBtnMouseZoomOut1, Me.c1pBtnMouseSelect1, Me.c1pBtnFindText1, Me.c1pBtnSeparator3, Me.c1pBtnGoFirst1, Me.c1pBtnGoPrev1, Me.c1pBtnGoNext1, Me.c1pBtnGoLast1, Me.c1pBtnSeparator4, Me.c1pBtnHistoryPrev1, Me.c1pBtnHistoryNext1, Me.c1pBtnSeparator5, Me.c1pBtnZoomOut1, Me.c1pBtnZoomIn1, Me.c1pBtnSeparator6, Me.c1pBtnViewActualSize1, Me.c1pBtnViewFullPage1, Me.c1pBtnViewPageWidth1, Me.c1pBtnViewTwoPages1, Me.c1pBtnViewFourPages1, Me.c1pBtnSeparator7, Me.c1pBtnHelp1})
        Me.C1PrintPreviewControl.ToolBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreviewControl.ToolBar.CustomImageList = Me.imgImageList1
        Me.C1PrintPreviewControl.ToolBar.CustomImageListHi = Me.imgImageList1
        Me.C1PrintPreviewControl.ToolBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.C1PrintPreviewControl.ToolBar.ImageSet = C1.Win.C1PrintPreview.ToolBarImageSetEnum.Custom
        '
        'c1pBtnFileOpen1
        '
        Me.c1pBtnFileOpen1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.FileOpen
        Me.c1pBtnFileOpen1.ImageIndex = 0
        Me.c1pBtnFileOpen1.Name = "c1pBtnFileOpen1"
        Me.c1pBtnFileOpen1.ToolTipText = "File Open"
        '
        'c1pBtnFileSave1
        '
        Me.c1pBtnFileSave1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.FileSave
        Me.c1pBtnFileSave1.Enabled = False
        Me.c1pBtnFileSave1.ImageIndex = 142
        Me.c1pBtnFileSave1.Name = "c1pBtnFileSave1"
        Me.c1pBtnFileSave1.ToolTipText = "File Save"
        '
        'c1pBtnFilePrint1
        '
        Me.c1pBtnFilePrint1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.FilePrint
        Me.c1pBtnFilePrint1.Enabled = False
        Me.c1pBtnFilePrint1.ImageIndex = 135
        Me.c1pBtnFilePrint1.Name = "c1pBtnFilePrint1"
        Me.c1pBtnFilePrint1.ToolTipText = "Print"
        '
        'c1pBtnPageSetup1
        '
        Me.c1pBtnPageSetup1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.PageSetup
        Me.c1pBtnPageSetup1.ImageIndex = 54
        Me.c1pBtnPageSetup1.Name = "c1pBtnPageSetup1"
        Me.c1pBtnPageSetup1.ToolTipText = "Page Setup"
        '
        'c1pBtnReflow1
        '
        Me.c1pBtnReflow1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.Reflow
        Me.c1pBtnReflow1.Enabled = False
        Me.c1pBtnReflow1.ImageIndex = 138
        Me.c1pBtnReflow1.Name = "c1pBtnReflow1"
        Me.c1pBtnReflow1.ToolTipText = "Reflow"
        '
        'c1pBtnStop1
        '
        Me.c1pBtnStop1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.[Stop]
        Me.c1pBtnStop1.ImageIndex = 25
        Me.c1pBtnStop1.Name = "c1pBtnStop1"
        Me.c1pBtnStop1.ToolTipText = "Stop"
        Me.c1pBtnStop1.Visible = False
        '
        'c1pBtnDocInfo1
        '
        Me.c1pBtnDocInfo1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.DocInfo
        Me.c1pBtnDocInfo1.Enabled = False
        Me.c1pBtnDocInfo1.ImageIndex = 120
        Me.c1pBtnDocInfo1.Name = "c1pBtnDocInfo1"
        Me.c1pBtnDocInfo1.ToolTipText = "Document information"
        '
        'c1pBtnSeparator1
        '
        Me.c1pBtnSeparator1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.None
        Me.c1pBtnSeparator1.Name = "c1pBtnSeparator1"
        Me.c1pBtnSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'c1pBtnShowNavigationBar1
        '
        Me.c1pBtnShowNavigationBar1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ShowNavigationBar
        Me.c1pBtnShowNavigationBar1.ImageIndex = 157
        Me.c1pBtnShowNavigationBar1.Name = "c1pBtnShowNavigationBar1"
        Me.c1pBtnShowNavigationBar1.Pushed = True
        Me.c1pBtnShowNavigationBar1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnShowNavigationBar1.ToolTipText = "Show Navigation Bar"
        '
        'c1pBtnSeparator2
        '
        Me.c1pBtnSeparator2.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.None
        Me.c1pBtnSeparator2.Name = "c1pBtnSeparator2"
        Me.c1pBtnSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'c1pBtnMouseHand1
        '
        Me.c1pBtnMouseHand1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.MouseHand
        Me.c1pBtnMouseHand1.Enabled = False
        Me.c1pBtnMouseHand1.ImageIndex = 44
        Me.c1pBtnMouseHand1.Name = "c1pBtnMouseHand1"
        Me.c1pBtnMouseHand1.Pushed = True
        Me.c1pBtnMouseHand1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnMouseHand1.ToolTipText = "Hand Tool"
        '
        'c1pBtnMouseZoom1
        '
        Me.c1pBtnMouseZoom1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.MouseZoom
        Me.c1pBtnMouseZoom1.Enabled = False
        Me.c1pBtnMouseZoom1.ImageIndex = 159
        Me.c1pBtnMouseZoom1.Name = "c1pBtnMouseZoom1"
        Me.c1pBtnMouseZoom1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.c1pBtnMouseZoom1.ToolTipText = "Zoom In Tool"
        '
        'c1pBtnMouseZoomOut1
        '
        Me.c1pBtnMouseZoomOut1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.MouseZoomOut
        Me.c1pBtnMouseZoomOut1.Enabled = False
        Me.c1pBtnMouseZoomOut1.ImageIndex = 160
        Me.c1pBtnMouseZoomOut1.Name = "c1pBtnMouseZoomOut1"
        Me.c1pBtnMouseZoomOut1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.c1pBtnMouseZoomOut1.ToolTipText = "Zoom Out Tool"
        Me.c1pBtnMouseZoomOut1.Visible = False
        '
        'c1pBtnMouseSelect1
        '
        Me.c1pBtnMouseSelect1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.MouseSelect
        Me.c1pBtnMouseSelect1.Enabled = False
        Me.c1pBtnMouseSelect1.ImageIndex = 32
        Me.c1pBtnMouseSelect1.Name = "c1pBtnMouseSelect1"
        Me.c1pBtnMouseSelect1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnMouseSelect1.ToolTipText = "Select Text"
        '
        'c1pBtnFindText1
        '
        Me.c1pBtnFindText1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.FindText
        Me.c1pBtnFindText1.Enabled = False
        Me.c1pBtnFindText1.ImageIndex = 145
        Me.c1pBtnFindText1.Name = "c1pBtnFindText1"
        Me.c1pBtnFindText1.ToolTipText = "Find Text"
        '
        'c1pBtnSeparator3
        '
        Me.c1pBtnSeparator3.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.None
        Me.c1pBtnSeparator3.Name = "c1pBtnSeparator3"
        Me.c1pBtnSeparator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'c1pBtnGoFirst1
        '
        Me.c1pBtnGoFirst1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.GoFirst
        Me.c1pBtnGoFirst1.Enabled = False
        Me.c1pBtnGoFirst1.ImageIndex = 83
        Me.c1pBtnGoFirst1.Name = "c1pBtnGoFirst1"
        Me.c1pBtnGoFirst1.ToolTipText = "First Page"
        '
        'c1pBtnGoPrev1
        '
        Me.c1pBtnGoPrev1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.GoPrev
        Me.c1pBtnGoPrev1.Enabled = False
        Me.c1pBtnGoPrev1.ImageIndex = 121
        Me.c1pBtnGoPrev1.Name = "c1pBtnGoPrev1"
        Me.c1pBtnGoPrev1.ToolTipText = "Previous Page"
        '
        'c1pBtnGoNext1
        '
        Me.c1pBtnGoNext1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.GoNext
        Me.c1pBtnGoNext1.Enabled = False
        Me.c1pBtnGoNext1.ImageIndex = 140
        Me.c1pBtnGoNext1.Name = "c1pBtnGoNext1"
        Me.c1pBtnGoNext1.ToolTipText = "Next Page"
        '
        'c1pBtnGoLast1
        '
        Me.c1pBtnGoLast1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.GoLast
        Me.c1pBtnGoLast1.Enabled = False
        Me.c1pBtnGoLast1.ImageIndex = 111
        Me.c1pBtnGoLast1.Name = "c1pBtnGoLast1"
        Me.c1pBtnGoLast1.ToolTipText = "Last Page"
        '
        'c1pBtnSeparator4
        '
        Me.c1pBtnSeparator4.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.None
        Me.c1pBtnSeparator4.Name = "c1pBtnSeparator4"
        Me.c1pBtnSeparator4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'c1pBtnHistoryPrev1
        '
        Me.c1pBtnHistoryPrev1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.HistoryPrev
        Me.c1pBtnHistoryPrev1.Enabled = False
        Me.c1pBtnHistoryPrev1.ImageIndex = 15
        Me.c1pBtnHistoryPrev1.Name = "c1pBtnHistoryPrev1"
        Me.c1pBtnHistoryPrev1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.c1pBtnHistoryPrev1.ToolTipText = "Previous View"
        '
        'c1pBtnHistoryNext1
        '
        Me.c1pBtnHistoryNext1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.HistoryNext
        Me.c1pBtnHistoryNext1.Enabled = False
        Me.c1pBtnHistoryNext1.ImageIndex = 14
        Me.c1pBtnHistoryNext1.Name = "c1pBtnHistoryNext1"
        Me.c1pBtnHistoryNext1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.c1pBtnHistoryNext1.ToolTipText = "Next View"
        '
        'c1pBtnSeparator5
        '
        Me.c1pBtnSeparator5.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.None
        Me.c1pBtnSeparator5.Name = "c1pBtnSeparator5"
        Me.c1pBtnSeparator5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        Me.c1pBtnSeparator5.Visible = False
        '
        'c1pBtnZoomOut1
        '
        Me.c1pBtnZoomOut1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ZoomOut
        Me.c1pBtnZoomOut1.Enabled = False
        Me.c1pBtnZoomOut1.ImageIndex = 160
        Me.c1pBtnZoomOut1.Name = "c1pBtnZoomOut1"
        Me.c1pBtnZoomOut1.ToolTipText = "Zoom Out"
        Me.c1pBtnZoomOut1.Visible = False
        '
        'c1pBtnZoomIn1
        '
        Me.c1pBtnZoomIn1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ZoomIn
        Me.c1pBtnZoomIn1.Enabled = False
        Me.c1pBtnZoomIn1.ImageIndex = 159
        Me.c1pBtnZoomIn1.Name = "c1pBtnZoomIn1"
        Me.c1pBtnZoomIn1.ToolTipText = "Zoom In"
        Me.c1pBtnZoomIn1.Visible = False
        '
        'c1pBtnSeparator6
        '
        Me.c1pBtnSeparator6.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.None
        Me.c1pBtnSeparator6.Name = "c1pBtnSeparator6"
        Me.c1pBtnSeparator6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        Me.c1pBtnSeparator6.Visible = False
        '
        'c1pBtnViewActualSize1
        '
        Me.c1pBtnViewActualSize1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ViewActualSize
        Me.c1pBtnViewActualSize1.Enabled = False
        Me.c1pBtnViewActualSize1.ImageIndex = 38
        Me.c1pBtnViewActualSize1.Name = "c1pBtnViewActualSize1"
        Me.c1pBtnViewActualSize1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnViewActualSize1.ToolTipText = "Actual Size"
        '
        'c1pBtnViewFullPage1
        '
        Me.c1pBtnViewFullPage1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ViewFullPage
        Me.c1pBtnViewFullPage1.Enabled = False
        Me.c1pBtnViewFullPage1.ImageIndex = 68
        Me.c1pBtnViewFullPage1.Name = "c1pBtnViewFullPage1"
        Me.c1pBtnViewFullPage1.Pushed = True
        Me.c1pBtnViewFullPage1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnViewFullPage1.ToolTipText = "Full Page"
        '
        'c1pBtnViewPageWidth1
        '
        Me.c1pBtnViewPageWidth1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ViewPageWidth
        Me.c1pBtnViewPageWidth1.Enabled = False
        Me.c1pBtnViewPageWidth1.ImageIndex = 48
        Me.c1pBtnViewPageWidth1.Name = "c1pBtnViewPageWidth1"
        Me.c1pBtnViewPageWidth1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnViewPageWidth1.ToolTipText = "Page Width"
        '
        'c1pBtnViewTwoPages1
        '
        Me.c1pBtnViewTwoPages1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ViewTwoPages
        Me.c1pBtnViewTwoPages1.Enabled = False
        Me.c1pBtnViewTwoPages1.ImageIndex = 16
        Me.c1pBtnViewTwoPages1.Name = "c1pBtnViewTwoPages1"
        Me.c1pBtnViewTwoPages1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnViewTwoPages1.ToolTipText = "Two Pages"
        '
        'c1pBtnViewFourPages1
        '
        Me.c1pBtnViewFourPages1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ViewFourPages
        Me.c1pBtnViewFourPages1.Enabled = False
        Me.c1pBtnViewFourPages1.ImageIndex = 16
        Me.c1pBtnViewFourPages1.Name = "c1pBtnViewFourPages1"
        Me.c1pBtnViewFourPages1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.c1pBtnViewFourPages1.ToolTipText = "Four Pages"
        '
        'c1pBtnSeparator7
        '
        Me.c1pBtnSeparator7.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.None
        Me.c1pBtnSeparator7.Name = "c1pBtnSeparator7"
        Me.c1pBtnSeparator7.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        Me.c1pBtnSeparator7.Visible = False
        '
        'c1pBtnHelp1
        '
        Me.c1pBtnHelp1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.Help
        Me.c1pBtnHelp1.ImageIndex = 35
        Me.c1pBtnHelp1.Name = "c1pBtnHelp1"
        Me.c1pBtnHelp1.ToolTipText = "Help"
        Me.c1pBtnHelp1.Visible = False
        '
        'imgImageList1
        '
        Me.imgImageList1.ImageStream = CType(resources.GetObject("imgImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.imgImageList1.Images.SetKeyName(0, "Folder-(Open).gif")
        Me.imgImageList1.Images.SetKeyName(1, "Addressbook.gif")
        Me.imgImageList1.Images.SetKeyName(2, "Ball.gif")
        Me.imgImageList1.Images.SetKeyName(3, "Calculator.gif")
        Me.imgImageList1.Images.SetKeyName(4, "Calendar.gif")
        Me.imgImageList1.Images.SetKeyName(5, "CD-(Blue).gif")
        Me.imgImageList1.Images.SetKeyName(6, "CD-(Green).gif")
        Me.imgImageList1.Images.SetKeyName(7, "CD-(Red).gif")
        Me.imgImageList1.Images.SetKeyName(8, "CD-(Yellow).gif")
        Me.imgImageList1.Images.SetKeyName(9, "Chart-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(10, "Clipboard.gif")
        Me.imgImageList1.Images.SetKeyName(11, "Close.gif")
        Me.imgImageList1.Images.SetKeyName(12, "Database-(1).gif")
        Me.imgImageList1.Images.SetKeyName(13, "Database-(2).gif")
        Me.imgImageList1.Images.SetKeyName(14, "Document-(Add).gif")
        Me.imgImageList1.Images.SetKeyName(15, "Document-(Remove).gif")
        Me.imgImageList1.Images.SetKeyName(16, "Documents.gif")
        Me.imgImageList1.Images.SetKeyName(17, "Dollar.gif")
        Me.imgImageList1.Images.SetKeyName(18, "Download-From-Harddisk.gif")
        Me.imgImageList1.Images.SetKeyName(19, "Drawer-(Left).gif")
        Me.imgImageList1.Images.SetKeyName(20, "Drawer-(Right).gif")
        Me.imgImageList1.Images.SetKeyName(21, "Eject.gif")
        Me.imgImageList1.Images.SetKeyName(22, "Euro.gif")
        Me.imgImageList1.Images.SetKeyName(23, "Event-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(24, "Event.gif")
        Me.imgImageList1.Images.SetKeyName(25, "Exclamtion-Mark.gif")
        Me.imgImageList1.Images.SetKeyName(26, "Export.gif")
        Me.imgImageList1.Images.SetKeyName(27, "First-Aid.gif")
        Me.imgImageList1.Images.SetKeyName(28, "Folder-(Add).gif")
        Me.imgImageList1.Images.SetKeyName(29, "Folder-(Favorites).gif")
        Me.imgImageList1.Images.SetKeyName(30, "Folder-(Open).gif")
        Me.imgImageList1.Images.SetKeyName(31, "Folder-(Remove).gif")
        Me.imgImageList1.Images.SetKeyName(32, "Font.gif")
        Me.imgImageList1.Images.SetKeyName(33, "Games.gif")
        Me.imgImageList1.Images.SetKeyName(34, "Gear.gif")
        Me.imgImageList1.Images.SetKeyName(35, "Help-(Alt-3).gif")
        Me.imgImageList1.Images.SetKeyName(36, "Home-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(37, "Import.gif")
        Me.imgImageList1.Images.SetKeyName(38, "Increase.gif")
        Me.imgImageList1.Images.SetKeyName(39, "Junk-(Empty).gif")
        Me.imgImageList1.Images.SetKeyName(40, "Junk-(Full).gif")
        Me.imgImageList1.Images.SetKeyName(41, "Left-Down.gif")
        Me.imgImageList1.Images.SetKeyName(42, "Left-Up.gif")
        Me.imgImageList1.Images.SetKeyName(43, "Lock.gif")
        Me.imgImageList1.Images.SetKeyName(44, "Move.gif")
        Me.imgImageList1.Images.SetKeyName(45, "Movie.gif")
        Me.imgImageList1.Images.SetKeyName(46, "News.gif")
        Me.imgImageList1.Images.SetKeyName(47, "Next.gif")
        Me.imgImageList1.Images.SetKeyName(48, "Notepad.gif")
        Me.imgImageList1.Images.SetKeyName(49, "Options.gif")
        Me.imgImageList1.Images.SetKeyName(50, "Paper-Clip.gif")
        Me.imgImageList1.Images.SetKeyName(51, "Pencil-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(52, "Preferences-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(53, "Preferences.gif")
        Me.imgImageList1.Images.SetKeyName(54, "Preview.gif")
        Me.imgImageList1.Images.SetKeyName(55, "Previous.gif")
        Me.imgImageList1.Images.SetKeyName(56, "Private.gif")
        Me.imgImageList1.Images.SetKeyName(57, "Redo.gif")
        Me.imgImageList1.Images.SetKeyName(58, "Right-Down.gif")
        Me.imgImageList1.Images.SetKeyName(59, "Right-Up.gif")
        Me.imgImageList1.Images.SetKeyName(60, "Rotate-Left.gif")
        Me.imgImageList1.Images.SetKeyName(61, "Rotate-Right.gif")
        Me.imgImageList1.Images.SetKeyName(62, "Send-Mail.gif")
        Me.imgImageList1.Images.SetKeyName(63, "Shopping-Chart.gif")
        Me.imgImageList1.Images.SetKeyName(64, "Stamp.gif")
        Me.imgImageList1.Images.SetKeyName(65, "Star-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(66, "Synth.gif")
        Me.imgImageList1.Images.SetKeyName(67, "Terminal.gif")
        Me.imgImageList1.Images.SetKeyName(68, "Text.gif")
        Me.imgImageList1.Images.SetKeyName(69, "Toolbox.gif")
        Me.imgImageList1.Images.SetKeyName(70, "Traffic-Light.gif")
        Me.imgImageList1.Images.SetKeyName(71, "TV.gif")
        Me.imgImageList1.Images.SetKeyName(72, "Undo.gif")
        Me.imgImageList1.Images.SetKeyName(73, "Unlocked.gif")
        Me.imgImageList1.Images.SetKeyName(74, "Upload-From-Harddisk.gif")
        Me.imgImageList1.Images.SetKeyName(75, "vCard.gif")
        Me.imgImageList1.Images.SetKeyName(76, "Warning-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(77, "Web-Server.gif")
        Me.imgImageList1.Images.SetKeyName(78, "Website.gif")
        Me.imgImageList1.Images.SetKeyName(79, "Window.gif")
        Me.imgImageList1.Images.SetKeyName(80, "Write-Mail.gif")
        Me.imgImageList1.Images.SetKeyName(81, "Add.gif")
        Me.imgImageList1.Images.SetKeyName(82, "Applications.gif")
        Me.imgImageList1.Images.SetKeyName(83, "Backward.gif")
        Me.imgImageList1.Images.SetKeyName(84, "Ball-(Blue).gif")
        Me.imgImageList1.Images.SetKeyName(85, "Ball-(Green).gif")
        Me.imgImageList1.Images.SetKeyName(86, "Ball-(Red).gif")
        Me.imgImageList1.Images.SetKeyName(87, "Ball-(Yellow).gif")
        Me.imgImageList1.Images.SetKeyName(88, "Book.gif")
        Me.imgImageList1.Images.SetKeyName(89, "Burn.gif")
        Me.imgImageList1.Images.SetKeyName(90, "Cancel.gif")
        Me.imgImageList1.Images.SetKeyName(91, "CD.gif")
        Me.imgImageList1.Images.SetKeyName(92, "Chart.gif")
        Me.imgImageList1.Images.SetKeyName(93, "Chat-Bubble-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(94, "Chat-Bubble.gif")
        Me.imgImageList1.Images.SetKeyName(95, "Checkmark.gif")
        Me.imgImageList1.Images.SetKeyName(96, "Connect.gif")
        Me.imgImageList1.Images.SetKeyName(97, "Copy.gif")
        Me.imgImageList1.Images.SetKeyName(98, "Cut.gif")
        Me.imgImageList1.Images.SetKeyName(99, "Delete.gif")
        Me.imgImageList1.Images.SetKeyName(100, "Desktop-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(101, "Desktop.gif")
        Me.imgImageList1.Images.SetKeyName(102, "Document.gif")
        Me.imgImageList1.Images.SetKeyName(103, "Down.gif")
        Me.imgImageList1.Images.SetKeyName(104, "Edit-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(105, "Edit.gif")
        Me.imgImageList1.Images.SetKeyName(106, "Favorites-(Add).gif")
        Me.imgImageList1.Images.SetKeyName(107, "Favorites-(Remove).gif")
        Me.imgImageList1.Images.SetKeyName(108, "Favorites.gif")
        Me.imgImageList1.Images.SetKeyName(109, "Folder-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(110, "Folder.gif")
        Me.imgImageList1.Images.SetKeyName(111, "Forward.gif")
        Me.imgImageList1.Images.SetKeyName(112, "Globe-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(113, "Globe.gif")
        Me.imgImageList1.Images.SetKeyName(114, "Harddisk.gif")
        Me.imgImageList1.Images.SetKeyName(115, "Help-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(116, "Help.gif")
        Me.imgImageList1.Images.SetKeyName(117, "History.gif")
        Me.imgImageList1.Images.SetKeyName(118, "Home.gif")
        Me.imgImageList1.Images.SetKeyName(119, "Image.gif")
        Me.imgImageList1.Images.SetKeyName(120, "Information.gif")
        Me.imgImageList1.Images.SetKeyName(121, "Left.gif")
        Me.imgImageList1.Images.SetKeyName(122, "Light-Bulb-(Off).gif")
        Me.imgImageList1.Images.SetKeyName(123, "Light-Bulb-(On).gif")
        Me.imgImageList1.Images.SetKeyName(124, "Mail-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(125, "Mail.gif")
        Me.imgImageList1.Images.SetKeyName(126, "Music.gif")
        Me.imgImageList1.Images.SetKeyName(127, "Network.gif")
        Me.imgImageList1.Images.SetKeyName(128, "OK.gif")
        Me.imgImageList1.Images.SetKeyName(129, "Paint.gif")
        Me.imgImageList1.Images.SetKeyName(130, "Paste.gif")
        Me.imgImageList1.Images.SetKeyName(131, "Pause.gif")
        Me.imgImageList1.Images.SetKeyName(132, "Pencil.gif")
        Me.imgImageList1.Images.SetKeyName(133, "Phone.gif")
        Me.imgImageList1.Images.SetKeyName(134, "Play.gif")
        Me.imgImageList1.Images.SetKeyName(135, "Print.gif")
        Me.imgImageList1.Images.SetKeyName(136, "Protection.gif")
        Me.imgImageList1.Images.SetKeyName(137, "Recycle.gif")
        Me.imgImageList1.Images.SetKeyName(138, "Refresh.gif")
        Me.imgImageList1.Images.SetKeyName(139, "Remove.gif")
        Me.imgImageList1.Images.SetKeyName(140, "Right.gif")
        Me.imgImageList1.Images.SetKeyName(141, "Ruler.gif")
        Me.imgImageList1.Images.SetKeyName(142, "Save.gif")
        Me.imgImageList1.Images.SetKeyName(143, "Save-As.gif")
        Me.imgImageList1.Images.SetKeyName(144, "Seal.gif")
        Me.imgImageList1.Images.SetKeyName(145, "Search.gif")
        Me.imgImageList1.Images.SetKeyName(146, "Smiley.gif")
        Me.imgImageList1.Images.SetKeyName(147, "Star.gif")
        Me.imgImageList1.Images.SetKeyName(148, "Stop-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(149, "Stop.gif")
        Me.imgImageList1.Images.SetKeyName(150, "Tool.gif")
        Me.imgImageList1.Images.SetKeyName(151, "Trash-(Empty).gif")
        Me.imgImageList1.Images.SetKeyName(152, "Trash-(Full).gif")
        Me.imgImageList1.Images.SetKeyName(153, "Up.gif")
        Me.imgImageList1.Images.SetKeyName(154, "User-(Alt-2).gif")
        Me.imgImageList1.Images.SetKeyName(155, "User.gif")
        Me.imgImageList1.Images.SetKeyName(156, "Users.gif")
        Me.imgImageList1.Images.SetKeyName(157, "View.gif")
        Me.imgImageList1.Images.SetKeyName(158, "Warning.gif")
        Me.imgImageList1.Images.SetKeyName(159, "Zoom-In.gif")
        Me.imgImageList1.Images.SetKeyName(160, "Zoom-Out.gif")
        '
        'NuGenC1PrintPreviewDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(792, 566)
        Me.Controls.Add(Me.C1PrintPreviewControl)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "NuGenC1PrintPreviewDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Print - Preview - Export"
        CType(Me.C1PrintPreviewControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents C1PrintPreviewControl As C1.Win.C1PrintPreview.C1PrintPreview
    Friend WithEvents c1pBtnFileOpen1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnFileSave1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnFilePrint1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnPageSetup1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnReflow1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnStop1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnDocInfo1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnSeparator1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnShowNavigationBar1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnSeparator2 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnMouseHand1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnMouseZoom1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnMouseZoomOut1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnMouseSelect1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnFindText1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnSeparator3 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnGoFirst1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnGoPrev1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnGoNext1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnGoLast1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnSeparator4 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnHistoryPrev1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnHistoryNext1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnSeparator5 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnZoomOut1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnZoomIn1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnSeparator6 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnViewActualSize1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnViewFullPage1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnViewPageWidth1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnViewTwoPages1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnViewFourPages1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnSeparator7 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents c1pBtnHelp1 As C1.Win.C1PrintPreview.PreviewToolBarButton
    Friend WithEvents imgImageList1 As System.Windows.Forms.ImageList
    'Friend WithEvents RibbonTabItemGroup1 As DevComponents.DotNetBar.RibbonTabItemGroup
End Class

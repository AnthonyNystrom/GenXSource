<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim UiStrings1 As C1.C1PrintDocument.UIStrings = New C1.C1PrintDocument.UIStrings
        Me.C1PrintPreview1 = New C1.Win.C1PrintPreview.C1PrintPreview
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
        Me.prec1pBtnHelp1 = New C1.Win.C1PrintPreview.PreviewToolBarButton
        CType(Me.C1PrintPreview1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'C1PrintPreview1
        '
        Me.C1PrintPreview1.C1DPageSettings = "color:True;landscape:False;margins:100,100,100,100;papersize:850,1100,TABlAHQAdAB" & _
            "lAHIA"
        C1DocEngine1.DisplayName = ""
        C1DocEngine1.Name = ""
        C1DocEngine1.rnd = Root1
        DrawelRoot1.bc = Nothing
        DrawelRoot1.bo = Nothing
        DrawelRoot1.dct = Nothing
        DrawelRoot1.f = New System.Drawing.Font("Arial", 20.0!)
        DrawelRoot1.hh = Nothing
        DrawelRoot1.i = CType(1, Long)
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
        DrawelText1.i = CType(3, Long)
        DrawelText1.ib = Nothing
        DrawelText1.iba = Nothing
        DrawelText1.mi = Nothing
        DrawelText1.n = "text"
        DrawelText1.oi = Nothing
        DrawelText1.pa = Widths2
        DrawelText1.s = Nothing
        DrawelText1.sp = Nothing
        DrawelText1.t = "Generated on Thursday, June 01, 2006."
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
        DrawelText2.i = CType(2, Long)
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
        Me.C1PrintPreview1.Document = C1DocEngine1
        Me.C1PrintPreview1.Location = New System.Drawing.Point(224, 124)
        Me.C1PrintPreview1.Name = "C1PrintPreview1"
        Me.C1PrintPreview1.NavigationBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreview1.NavigationBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.C1PrintPreview1.NavigationBar.OutlineView.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreview1.NavigationBar.OutlineView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.C1PrintPreview1.NavigationBar.OutlineView.Indent = 19
        Me.C1PrintPreview1.NavigationBar.OutlineView.ItemHeight = 16
        Me.C1PrintPreview1.NavigationBar.OutlineView.TabIndex = 0
        Me.C1PrintPreview1.NavigationBar.OutlineView.Visible = False
        Me.C1PrintPreview1.NavigationBar.Padding = New System.Drawing.Point(6, 3)
        Me.C1PrintPreview1.NavigationBar.TabIndex = 2
        Me.C1PrintPreview1.NavigationBar.ThumbnailsView.AutoArrange = True
        Me.C1PrintPreview1.NavigationBar.ThumbnailsView.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreview1.NavigationBar.ThumbnailsView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.C1PrintPreview1.NavigationBar.ThumbnailsView.TabIndex = 0
        Me.C1PrintPreview1.NavigationBar.ThumbnailsView.Visible = True
        Me.C1PrintPreview1.NavigationBar.Width = 160
        Me.C1PrintPreview1.Size = New System.Drawing.Size(467, 483)
        Me.C1PrintPreview1.Splitter.Cursor = System.Windows.Forms.Cursors.VSplit
        Me.C1PrintPreview1.Splitter.Width = 3
        Me.C1PrintPreview1.StatusBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreview1.StatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.C1PrintPreview1.StatusBar.TabIndex = 4
        Me.C1PrintPreview1.TabIndex = 0
        Me.C1PrintPreview1.ToolBar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.c1pBtnFileOpen1, Me.c1pBtnFileSave1, Me.c1pBtnFilePrint1, Me.c1pBtnPageSetup1, Me.c1pBtnReflow1, Me.c1pBtnStop1, Me.c1pBtnDocInfo1, Me.c1pBtnSeparator1, Me.c1pBtnShowNavigationBar1, Me.c1pBtnSeparator2, Me.c1pBtnMouseHand1, Me.c1pBtnMouseZoom1, Me.c1pBtnMouseZoomOut1, Me.c1pBtnMouseSelect1, Me.c1pBtnFindText1, Me.c1pBtnSeparator3, Me.c1pBtnGoFirst1, Me.c1pBtnGoPrev1, Me.c1pBtnGoNext1, Me.c1pBtnGoLast1, Me.c1pBtnSeparator4, Me.c1pBtnHistoryPrev1, Me.c1pBtnHistoryNext1, Me.c1pBtnSeparator5, Me.c1pBtnZoomOut1, Me.c1pBtnZoomIn1, Me.c1pBtnSeparator6, Me.c1pBtnViewActualSize1, Me.c1pBtnViewFullPage1, Me.c1pBtnViewPageWidth1, Me.c1pBtnViewTwoPages1, Me.c1pBtnViewFourPages1, Me.c1pBtnSeparator7, Me.prec1pBtnHelp1})
        Me.C1PrintPreview1.ToolBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.C1PrintPreview1.ToolBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.c1pBtnFileSave1.ImageIndex = 1
        Me.c1pBtnFileSave1.Name = "c1pBtnFileSave1"
        Me.c1pBtnFileSave1.ToolTipText = "File Save"
        '
        'c1pBtnFilePrint1
        '
        Me.c1pBtnFilePrint1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.FilePrint
        Me.c1pBtnFilePrint1.ImageIndex = 2
        Me.c1pBtnFilePrint1.Name = "c1pBtnFilePrint1"
        Me.c1pBtnFilePrint1.ToolTipText = "Print"
        '
        'c1pBtnPageSetup1
        '
        Me.c1pBtnPageSetup1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.PageSetup
        Me.c1pBtnPageSetup1.ImageIndex = 3
        Me.c1pBtnPageSetup1.Name = "c1pBtnPageSetup1"
        Me.c1pBtnPageSetup1.ToolTipText = "Page Setup"
        '
        'c1pBtnReflow1
        '
        Me.c1pBtnReflow1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.Reflow
        Me.c1pBtnReflow1.ImageIndex = 4
        Me.c1pBtnReflow1.Name = "c1pBtnReflow1"
        Me.c1pBtnReflow1.ToolTipText = "Reflow"
        '
        'c1pBtnStop1
        '
        Me.c1pBtnStop1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.[Stop]
        Me.c1pBtnStop1.ImageIndex = 5
        Me.c1pBtnStop1.Name = "c1pBtnStop1"
        Me.c1pBtnStop1.ToolTipText = "Stop"
        Me.c1pBtnStop1.Visible = False
        '
        'c1pBtnDocInfo1
        '
        Me.c1pBtnDocInfo1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.DocInfo
        Me.c1pBtnDocInfo1.Enabled = False
        Me.c1pBtnDocInfo1.ImageIndex = 26
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
        Me.c1pBtnShowNavigationBar1.ImageIndex = 6
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
        Me.c1pBtnMouseHand1.ImageIndex = 7
        Me.c1pBtnMouseHand1.Name = "c1pBtnMouseHand1"
        Me.c1pBtnMouseHand1.Pushed = True
        Me.c1pBtnMouseHand1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnMouseHand1.ToolTipText = "Hand Tool"
        '
        'c1pBtnMouseZoom1
        '
        Me.c1pBtnMouseZoom1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.MouseZoom
        Me.c1pBtnMouseZoom1.ImageIndex = 8
        Me.c1pBtnMouseZoom1.Name = "c1pBtnMouseZoom1"
        Me.c1pBtnMouseZoom1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.c1pBtnMouseZoom1.ToolTipText = "Zoom In Tool"
        '
        'c1pBtnMouseZoomOut1
        '
        Me.c1pBtnMouseZoomOut1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.MouseZoomOut
        Me.c1pBtnMouseZoomOut1.ImageIndex = 25
        Me.c1pBtnMouseZoomOut1.Name = "c1pBtnMouseZoomOut1"
        Me.c1pBtnMouseZoomOut1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.c1pBtnMouseZoomOut1.ToolTipText = "Zoom Out Tool"
        Me.c1pBtnMouseZoomOut1.Visible = False
        '
        'c1pBtnMouseSelect1
        '
        Me.c1pBtnMouseSelect1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.MouseSelect
        Me.c1pBtnMouseSelect1.ImageIndex = 9
        Me.c1pBtnMouseSelect1.Name = "c1pBtnMouseSelect1"
        Me.c1pBtnMouseSelect1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnMouseSelect1.ToolTipText = "Select Text"
        '
        'c1pBtnFindText1
        '
        Me.c1pBtnFindText1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.FindText
        Me.c1pBtnFindText1.ImageIndex = 10
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
        Me.c1pBtnGoFirst1.ImageIndex = 11
        Me.c1pBtnGoFirst1.Name = "c1pBtnGoFirst1"
        Me.c1pBtnGoFirst1.ToolTipText = "First Page"
        '
        'c1pBtnGoPrev1
        '
        Me.c1pBtnGoPrev1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.GoPrev
        Me.c1pBtnGoPrev1.Enabled = False
        Me.c1pBtnGoPrev1.ImageIndex = 12
        Me.c1pBtnGoPrev1.Name = "c1pBtnGoPrev1"
        Me.c1pBtnGoPrev1.ToolTipText = "Previous Page"
        '
        'c1pBtnGoNext1
        '
        Me.c1pBtnGoNext1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.GoNext
        Me.c1pBtnGoNext1.ImageIndex = 13
        Me.c1pBtnGoNext1.Name = "c1pBtnGoNext1"
        Me.c1pBtnGoNext1.ToolTipText = "Next Page"
        '
        'c1pBtnGoLast1
        '
        Me.c1pBtnGoLast1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.GoLast
        Me.c1pBtnGoLast1.ImageIndex = 14
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
        Me.c1pBtnHistoryNext1.ImageIndex = 16
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
        Me.c1pBtnZoomOut1.ImageIndex = 17
        Me.c1pBtnZoomOut1.Name = "c1pBtnZoomOut1"
        Me.c1pBtnZoomOut1.ToolTipText = "Zoom Out"
        Me.c1pBtnZoomOut1.Visible = False
        '
        'c1pBtnZoomIn1
        '
        Me.c1pBtnZoomIn1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ZoomIn
        Me.c1pBtnZoomIn1.ImageIndex = 18
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
        Me.c1pBtnViewActualSize1.ImageIndex = 19
        Me.c1pBtnViewActualSize1.Name = "c1pBtnViewActualSize1"
        Me.c1pBtnViewActualSize1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnViewActualSize1.ToolTipText = "Actual Size"
        '
        'c1pBtnViewFullPage1
        '
        Me.c1pBtnViewFullPage1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ViewFullPage
        Me.c1pBtnViewFullPage1.ImageIndex = 20
        Me.c1pBtnViewFullPage1.Name = "c1pBtnViewFullPage1"
        Me.c1pBtnViewFullPage1.Pushed = True
        Me.c1pBtnViewFullPage1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnViewFullPage1.ToolTipText = "Full Page"
        '
        'c1pBtnViewPageWidth1
        '
        Me.c1pBtnViewPageWidth1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ViewPageWidth
        Me.c1pBtnViewPageWidth1.ImageIndex = 21
        Me.c1pBtnViewPageWidth1.Name = "c1pBtnViewPageWidth1"
        Me.c1pBtnViewPageWidth1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnViewPageWidth1.ToolTipText = "Page Width"
        '
        'c1pBtnViewTwoPages1
        '
        Me.c1pBtnViewTwoPages1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ViewTwoPages
        Me.c1pBtnViewTwoPages1.ImageIndex = 22
        Me.c1pBtnViewTwoPages1.Name = "c1pBtnViewTwoPages1"
        Me.c1pBtnViewTwoPages1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.c1pBtnViewTwoPages1.ToolTipText = "Two Pages"
        '
        'c1pBtnViewFourPages1
        '
        Me.c1pBtnViewFourPages1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.ViewFourPages
        Me.c1pBtnViewFourPages1.ImageIndex = 23
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
        'prec1pBtnHelp1
        '
        Me.prec1pBtnHelp1.Action = C1.Win.C1PrintPreview.ToolBarButtonActionEnum.Help
        Me.prec1pBtnHelp1.ImageIndex = 24
        Me.prec1pBtnHelp1.Name = "prec1pBtnHelp1"
        Me.prec1pBtnHelp1.ToolTipText = "Help"
        Me.prec1pBtnHelp1.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(571, 412)
        Me.Controls.Add(Me.C1PrintPreview1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.C1PrintPreview1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents C1PrintPreview1 As C1.Win.C1PrintPreview.C1PrintPreview
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
    Friend WithEvents prec1pBtnHelp1 As C1.Win.C1PrintPreview.PreviewToolBarButton
End Class

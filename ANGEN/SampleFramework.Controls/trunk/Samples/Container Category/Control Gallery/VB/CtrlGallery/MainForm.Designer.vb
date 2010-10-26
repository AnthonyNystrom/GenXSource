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
        Me.nuGenSwitchPage3 = New Genetibase.[Shared].Controls.NuGenSwitchPage
        Me._calendar = New Genetibase.SmoothControls.NuGenSmoothCalendar
        Me.nuGenSwitchPage4 = New Genetibase.[Shared].Controls.NuGenSwitchPage
        Me._unitsSpin = New Genetibase.SmoothControls.NuGenSmoothUnitsSpin
        Me.nuGenSmoothFontSizeBox1 = New Genetibase.SmoothControls.NuGenSmoothFontSizeBox
        Me.nuGenSmoothFontBox1 = New Genetibase.SmoothControls.NuGenSmoothFontBox
        Me.nuGenSmoothDriveCombo1 = New Genetibase.SmoothControls.NuGenSmoothDriveCombo
        Me.nuGenSmoothDirectorySelector1 = New Genetibase.SmoothControls.NuGenSmoothDirectorySelector
        Me._colorBoxPopup = New Genetibase.SmoothControls.NuGenSmoothColorBoxPopup
        Me._colorBox = New Genetibase.SmoothControls.NuGenSmoothColorBox
        Me._calcDropDown = New Genetibase.SmoothControls.NuGenSmoothCalculatorDropDown
        Me._alignDropDown = New Genetibase.SmoothControls.NuGenSmoothAlignDropDown
        Me._bkgndPanel = New Genetibase.SmoothControls.NuGenSmoothPanel
        Me._switcher = New Genetibase.SmoothControls.NuGenSmoothSwitcher
        Me.nuGenSwitchPage1 = New Genetibase.[Shared].Controls.NuGenSwitchPage
        Me._textBox = New Genetibase.SmoothControls.NuGenSmoothTextBox
        Me.nuGenSmoothSpin1 = New Genetibase.SmoothControls.NuGenSmoothSpin
        Me._radioButton = New Genetibase.SmoothControls.NuGenSmoothRadioButton
        Me._pinpointList = New Genetibase.SmoothControls.NuGenSmoothPinpointList
        Me._optionSpin = New Genetibase.SmoothControls.NuGenSmoothOptionSpin
        Me._listBox = New Genetibase.SmoothControls.NuGenSmoothListBox
        Me._comboBox = New Genetibase.SmoothControls.NuGenSmoothComboBox
        Me._checkBox = New Genetibase.SmoothControls.NuGenSmoothCheckBox
        Me._button = New Genetibase.SmoothControls.NuGenSmoothButton
        Me.nuGenSwitchPage2 = New Genetibase.[Shared].Controls.NuGenSwitchPage
        Me.nuGenSmoothHotKeySelector1 = New Genetibase.SmoothControls.NuGenSmoothHotKeySelector
        Me._menuButton = New Genetibase.SmoothControls.NuGenSmoothMenuButton
        Me._alignSelector = New Genetibase.SmoothControls.NuGenSmoothAlignSelector
        Me._tooltip = New Genetibase.SmoothControls.NuGenSmoothToolTip
        Me.nuGenSmoothCheckBox1 = New Genetibase.SmoothControls.NuGenSmoothCheckBox
        Me.nuGenSwitchPage3.SuspendLayout()
        Me.nuGenSwitchPage4.SuspendLayout()
        Me._bkgndPanel.SuspendLayout()
        Me._switcher.SuspendLayout()
        Me.nuGenSwitchPage1.SuspendLayout()
        Me.nuGenSwitchPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'nuGenSwitchPage3
        '
        Me.nuGenSwitchPage3.Controls.Add(Me._calendar)
        Me.nuGenSwitchPage3.Name = "nuGenSwitchPage3"
        Me.nuGenSwitchPage3.SwitchButtonImage = CType(resources.GetObject("nuGenSwitchPage3.SwitchButtonImage"), System.Drawing.Image)
        Me.nuGenSwitchPage3.TabIndex = 3
        Me.nuGenSwitchPage3.Text = "DateTime"
        '
        '_calendar
        '
        Me._calendar.ActiveMonth.Month = 6
        Me._calendar.ActiveMonth.Year = 2007
        Me._calendar.BorderColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._calendar.Culture = New System.Globalization.CultureInfo("en-US")
        Me._calendar.Footer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me._calendar.Header.BackColor1 = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(236, Byte), Integer))
        Me._calendar.Header.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(218, Byte), Integer))
        Me._calendar.Header.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me._calendar.Header.GradientMode = Genetibase.[Shared].Controls.CalendarInternals.NuGenGradientMode.Vertical
        Me._calendar.ImageList = Nothing
        Me._calendar.Location = New System.Drawing.Point(12, 14)
        Me._calendar.MaxDate = New Date(2017, 6, 29, 17, 16, 56, 625)
        Me._calendar.MinDate = New Date(1997, 6, 29, 17, 16, 56, 625)
        Me._calendar.Month.BackgroundImage = Nothing
        Me._calendar.Month.Colors.Focus.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(236, Byte), Integer))
        Me._calendar.Month.Colors.Focus.Border = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me._calendar.Month.Colors.Selected.BackColor = System.Drawing.Color.FromArgb(CType(CType(111, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(176, Byte), Integer))
        Me._calendar.Month.Colors.Selected.Border = System.Drawing.Color.FromArgb(CType(CType(111, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(176, Byte), Integer))
        Me._calendar.Month.DateFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me._calendar.Month.TextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me._calendar.Name = "_calendar"
        Me._calendar.Size = New System.Drawing.Size(254, 257)
        Me._calendar.TabIndex = 0
        Me._calendar.Weekdays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me._calendar.Weekdays.TextColor = System.Drawing.Color.Black
        Me._calendar.Weeknumbers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me._calendar.Weeknumbers.TextColor = System.Drawing.Color.Black
        '
        'nuGenSwitchPage4
        '
        Me.nuGenSwitchPage4.Controls.Add(Me._unitsSpin)
        Me.nuGenSwitchPage4.Controls.Add(Me.nuGenSmoothFontSizeBox1)
        Me.nuGenSwitchPage4.Controls.Add(Me.nuGenSmoothFontBox1)
        Me.nuGenSwitchPage4.Controls.Add(Me.nuGenSmoothDriveCombo1)
        Me.nuGenSwitchPage4.Controls.Add(Me.nuGenSmoothDirectorySelector1)
        Me.nuGenSwitchPage4.Name = "nuGenSwitchPage4"
        Me.nuGenSwitchPage4.SwitchButtonImage = CType(resources.GetObject("nuGenSwitchPage4.SwitchButtonImage"), System.Drawing.Image)
        Me.nuGenSwitchPage4.TabIndex = 4
        Me.nuGenSwitchPage4.Text = "Specialized Controls"
        '
        '_unitsSpin
        '
        Me._unitsSpin.Location = New System.Drawing.Point(195, 41)
        Me._unitsSpin.Maximum = 100000000
        Me._unitsSpin.MeasureUnits = New String() {"B", "KB", "MB"}
        Me._unitsSpin.Name = "_unitsSpin"
        Me._unitsSpin.Size = New System.Drawing.Size(106, 20)
        Me._unitsSpin.TabIndex = 13
        '
        'nuGenSmoothFontSizeBox1
        '
        Me.nuGenSmoothFontSizeBox1.Location = New System.Drawing.Point(195, 68)
        Me.nuGenSmoothFontSizeBox1.MaxDropDownItems = 12
        Me.nuGenSmoothFontSizeBox1.Name = "nuGenSmoothFontSizeBox1"
        Me.nuGenSmoothFontSizeBox1.Size = New System.Drawing.Size(106, 21)
        Me.nuGenSmoothFontSizeBox1.TabIndex = 3
        Me.nuGenSmoothFontSizeBox1.Value = 10
        '
        'nuGenSmoothFontBox1
        '
        Me.nuGenSmoothFontBox1.Location = New System.Drawing.Point(12, 68)
        Me.nuGenSmoothFontBox1.Name = "nuGenSmoothFontBox1"
        Me.nuGenSmoothFontBox1.Size = New System.Drawing.Size(177, 21)
        Me.nuGenSmoothFontBox1.TabIndex = 2
        '
        'nuGenSmoothDriveCombo1
        '
        Me.nuGenSmoothDriveCombo1.Location = New System.Drawing.Point(12, 41)
        Me.nuGenSmoothDriveCombo1.Name = "nuGenSmoothDriveCombo1"
        Me.nuGenSmoothDriveCombo1.Size = New System.Drawing.Size(177, 21)
        Me.nuGenSmoothDriveCombo1.TabIndex = 1
        '
        'nuGenSmoothDirectorySelector1
        '
        Me.nuGenSmoothDirectorySelector1.CanChooseDirectory = True
        Me.nuGenSmoothDirectorySelector1.Location = New System.Drawing.Point(12, 14)
        Me.nuGenSmoothDirectorySelector1.Name = "nuGenSmoothDirectorySelector1"
        Me.nuGenSmoothDirectorySelector1.Size = New System.Drawing.Size(289, 21)
        Me.nuGenSmoothDirectorySelector1.TabIndex = 0
        '
        '_colorBoxPopup
        '
        Me._colorBoxPopup.Location = New System.Drawing.Point(173, 14)
        Me._colorBoxPopup.Name = "_colorBoxPopup"
        Me._colorBoxPopup.Size = New System.Drawing.Size(185, 260)
        Me._colorBoxPopup.TabIndex = 3
        '
        '_colorBox
        '
        Me._colorBox.Location = New System.Drawing.Point(12, 41)
        Me._colorBox.Name = "_colorBox"
        Me._colorBox.Size = New System.Drawing.Size(155, 21)
        Me._colorBox.TabIndex = 2
        '
        '_calcDropDown
        '
        Me._calcDropDown.Location = New System.Drawing.Point(12, 68)
        Me._calcDropDown.Name = "_calcDropDown"
        Me._calcDropDown.Size = New System.Drawing.Size(155, 21)
        Me._calcDropDown.TabIndex = 1
        '
        '_alignDropDown
        '
        Me._alignDropDown.Location = New System.Drawing.Point(12, 14)
        Me._alignDropDown.Name = "_alignDropDown"
        Me._alignDropDown.Size = New System.Drawing.Size(155, 21)
        Me._alignDropDown.TabIndex = 0
        '
        '_bkgndPanel
        '
        Me._bkgndPanel.Controls.Add(Me._switcher)
        Me._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._bkgndPanel.DrawBorder = False
        Me._bkgndPanel.Location = New System.Drawing.Point(0, 0)
        Me._bkgndPanel.Name = "_bkgndPanel"
        Me._bkgndPanel.Size = New System.Drawing.Size(752, 356)
        '
        '_switcher
        '
        Me._switcher.Dock = System.Windows.Forms.DockStyle.Fill
        Me._switcher.Location = New System.Drawing.Point(0, 0)
        Me._switcher.Name = "_switcher"
        Me._switcher.Size = New System.Drawing.Size(752, 356)
        Me._switcher.SwitchButtonSize = New System.Drawing.Size(120, 54)
        Me._switcher.SwitchPages.Add(Me.nuGenSwitchPage1)
        Me._switcher.SwitchPages.Add(Me.nuGenSwitchPage2)
        Me._switcher.SwitchPages.Add(Me.nuGenSwitchPage3)
        Me._switcher.SwitchPages.Add(Me.nuGenSwitchPage4)
        Me._switcher.SwitchPanelSize = New System.Drawing.Size(54, 70)
        Me._switcher.TabIndex = 0
        '
        'nuGenSwitchPage1
        '
        Me.nuGenSwitchPage1.Controls.Add(Me._textBox)
        Me.nuGenSwitchPage1.Controls.Add(Me.nuGenSmoothSpin1)
        Me.nuGenSwitchPage1.Controls.Add(Me._radioButton)
        Me.nuGenSwitchPage1.Controls.Add(Me._pinpointList)
        Me.nuGenSwitchPage1.Controls.Add(Me._optionSpin)
        Me.nuGenSwitchPage1.Controls.Add(Me._listBox)
        Me.nuGenSwitchPage1.Controls.Add(Me._comboBox)
        Me.nuGenSwitchPage1.Controls.Add(Me._checkBox)
        Me.nuGenSwitchPage1.Controls.Add(Me._button)
        Me.nuGenSwitchPage1.Name = "nuGenSwitchPage1"
        Me.nuGenSwitchPage1.SwitchButtonImage = CType(resources.GetObject("nuGenSwitchPage1.SwitchButtonImage"), System.Drawing.Image)
        Me.nuGenSwitchPage1.TabIndex = 1
        Me.nuGenSwitchPage1.Text = "Common Controls"
        '
        '_textBox
        '
        Me._textBox.InvalidTextBackColor = System.Drawing.Color.LightGreen
        Me._textBox.Location = New System.Drawing.Point(489, 14)
        Me._textBox.Name = "_textBox"
        Me._textBox.Pattern = ""
        Me._textBox.PatternMode = Genetibase.[Shared].Controls.NuGenTextBoxPatternMode.Digits
        Me._textBox.PromptFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me._textBox.PromptText = "Only digits are allowed"
        Me._textBox.Size = New System.Drawing.Size(150, 21)
        Me._textBox.TabIndex = 11
        Me._textBox.UseColors = True
        '
        'nuGenSmoothSpin1
        '
        Me.nuGenSmoothSpin1.Location = New System.Drawing.Point(207, 125)
        Me.nuGenSmoothSpin1.Name = "nuGenSmoothSpin1"
        Me.nuGenSmoothSpin1.Size = New System.Drawing.Size(133, 20)
        Me.nuGenSmoothSpin1.TabIndex = 10
        '
        '_radioButton
        '
        Me._radioButton.Location = New System.Drawing.Point(207, 73)
        Me._radioButton.Name = "_radioButton"
        Me._radioButton.Size = New System.Drawing.Size(84, 17)
        Me._radioButton.TabIndex = 9
        Me._radioButton.Text = "RadioButton"
        Me._radioButton.UseVisualStyleBackColor = False
        '
        '_pinpointList
        '
        Me._pinpointList.Dock = System.Windows.Forms.DockStyle.Left
        Me._pinpointList.Items.AddRange(New Object() {"AlignDropDown", "AlignSelector", "Bevel", "Button", "BlendSelector", "CalculatorDropDown", "Calendar", "CheckBox", "CloseButton", "ColorBox", "ColorBoxPopup", "ColorHistory", "ComboBox", "CommandManager", "ContextMenuStrip", "ControlReflector", "DirectorySelector", "DriveCombo", "DropDown", "FontBox", "FontSizeBox", "GradientPanel", "GroupBox", "HotKeySelector", "HSVPlainSelector", "HSVWheelSelector", "Int32Combo", "Label", "LinkLabel", "ListBox", "MatrixLabel", "MenuButton", "MiniBar", "NavigationBar", "OpenFileSelector", "OptionSpin", "Panel", "PanelEx", "PictureBox", "PinpointList", "PinpointWindow", "PopupContainer", "PrintPreview", "ProgressBar", "PropertyGrid", "RadioButton", "RoundedPanelEx", "ScrollBar", "ScrollButton", "SegDisplay", "SplitContainer", "Spin", "Spacer", "StatusStrip", "SwitchButton", "Switcher", "TabControl", "TaskBox", "Title", "TitlePanel", "ToolStrip", "ToolStripManager", "ToolTip", "TrackBar", "TreeView", "UISnap", "UnitsSpin", "Watermark"})
        Me._pinpointList.Location = New System.Drawing.Point(0, 0)
        Me._pinpointList.MinimumSize = New System.Drawing.Size(10, 10)
        Me._pinpointList.Name = "_pinpointList"
        Me._pinpointList.Size = New System.Drawing.Size(201, 286)
        Me._pinpointList.TabIndex = 8
        '
        '_optionSpin
        '
        Me._optionSpin.Items.Add("Ambient")
        Me._optionSpin.Items.Add("Black")
        Me._optionSpin.Items.Add("House")
        Me._optionSpin.Items.Add("Metal")
        Me._optionSpin.Items.Add("Rock")
        Me._optionSpin.Location = New System.Drawing.Point(207, 151)
        Me._optionSpin.Name = "_optionSpin"
        Me._optionSpin.Size = New System.Drawing.Size(133, 20)
        Me._optionSpin.TabIndex = 7
        Me._optionSpin.Text = "Rock"
        '
        '_listBox
        '
        Me._listBox.FormattingEnabled = True
        Me._listBox.Items.AddRange(New Object() {"AlignDropDown", "AlignSelector", "Bevel", "Button", "BlendSelector", "CalculatorDropDown", "Calendar", "CheckBox", "CloseButton", "ColorBox", "ColorBoxPopup", "ColorHistory", "ComboBox", "CommandManager", "ContextMenuStrip", "ControlReflector", "DirectorySelector", "DriveCombo", "DropDown", "FontBox", "FontSizeBox", "GradientPanel", "GroupBox", "HotKeySelector", "HSVPlainSelector", "HSVWheelSelector", "Int32Combo", "Label", "LinkLabel", "ListBox", "MatrixLabel", "MenuButton", "MiniBar", "NavigationBar", "OpenFileSelector", "OptionSpin", "Panel", "PanelEx", "PictureBox", "PinpointList", "PinpointWindow", "PopupContainer", "PrintPreview", "ProgressBar", "PropertyGrid", "RadioButton", "RoundedPanelEx", "ScrollBar", "ScrollButton", "SegDisplay", "SplitContainer", "Spin", "Spacer", "StatusStrip", "SwitchButton", "Switcher", "TabControl", "TaskBox", "Title", "TitlePanel", "ToolStrip", "ToolStripManager", "ToolTip", "TrackBar", "TreeView", "UISnap", "UnitsSpin", "Watermark"})
        Me._listBox.Location = New System.Drawing.Point(346, 14)
        Me._listBox.Name = "_listBox"
        Me._listBox.Size = New System.Drawing.Size(137, 161)
        Me._listBox.TabIndex = 3
        '
        '_comboBox
        '
        Me._comboBox.FormattingEnabled = True
        Me._comboBox.Items.AddRange(New Object() {"Astarte", "Deep Purple", "Dimmu Borgir", "Kiss", "Nautilus Pompilius ;-)", "Pink Floyd", "Pantera", "ReAnima 8-)"})
        Me._comboBox.Location = New System.Drawing.Point(207, 98)
        Me._comboBox.Name = "_comboBox"
        Me._comboBox.Size = New System.Drawing.Size(133, 21)
        Me._comboBox.TabIndex = 2
        Me._comboBox.Text = "Astarte"
        '
        '_checkBox
        '
        Me._checkBox.Location = New System.Drawing.Point(207, 50)
        Me._checkBox.Name = "_checkBox"
        Me._checkBox.Size = New System.Drawing.Size(75, 17)
        Me._checkBox.TabIndex = 1
        Me._checkBox.Text = "CheckBox"
        Me._checkBox.UseVisualStyleBackColor = False
        '
        '_button
        '
        Me._button.Location = New System.Drawing.Point(207, 14)
        Me._button.Name = "_button"
        Me._button.Size = New System.Drawing.Size(133, 30)
        Me._button.TabIndex = 0
        Me._button.Text = "Wait for tooltip here..."
        Me._tooltip.SetToolTip( _
            Me._button, _
            New Genetibase.Shared.Controls.NuGenToolTipInfo("Microsoft Office 2007 like ToolTip", _
            CType(resources.GetObject("_button.ToolTip"), System.Drawing.Image), _
            "Use our user-friendly tool-tip designer with in-place preview ability.", _
            "Press F1 for more help", _
            CType(resources.GetObject("_button.ToolTip1"), System.Drawing.Image), _
            "You can also specify custom tool-tip size if you are not satisfied with auto-size logic results.", _
            New System.Drawing.Size(0, 0)) _
            )
        Me._button.UseVisualStyleBackColor = False
        '
        'nuGenSwitchPage2
        '
        Me.nuGenSwitchPage2.Controls.Add(Me.nuGenSmoothHotKeySelector1)
        Me.nuGenSwitchPage2.Controls.Add(Me._menuButton)
        Me.nuGenSwitchPage2.Controls.Add(Me._alignSelector)
        Me.nuGenSwitchPage2.Controls.Add(Me._colorBoxPopup)
        Me.nuGenSwitchPage2.Controls.Add(Me._colorBox)
        Me.nuGenSwitchPage2.Controls.Add(Me._calcDropDown)
        Me.nuGenSwitchPage2.Controls.Add(Me._alignDropDown)
        Me.nuGenSwitchPage2.Name = "nuGenSwitchPage2"
        Me.nuGenSwitchPage2.SwitchButtonImage = CType(resources.GetObject("nuGenSwitchPage2.SwitchButtonImage"), System.Drawing.Image)
        Me.nuGenSwitchPage2.TabIndex = 2
        Me.nuGenSwitchPage2.Text = "Advanced UI"
        '
        'nuGenSmoothHotKeySelector1
        '
        Me.nuGenSmoothHotKeySelector1.Location = New System.Drawing.Point(525, 14)
        Me.nuGenSmoothHotKeySelector1.Name = "nuGenSmoothHotKeySelector1"
        Me.nuGenSmoothHotKeySelector1.Size = New System.Drawing.Size(155, 21)
        Me.nuGenSmoothHotKeySelector1.TabIndex = 13
        '
        '_menuButton
        '
        Me._menuButton.Location = New System.Drawing.Point(12, 95)
        Me._menuButton.Name = "_menuButton"
        Me._menuButton.Size = New System.Drawing.Size(155, 24)
        Me._menuButton.TabIndex = 5
        Me._menuButton.Text = "&Help"
        Me._menuButton.UseVisualStyleBackColor = False
        '
        '_alignSelector
        '
        Me._alignSelector.BackColor = System.Drawing.Color.Transparent
        Me._alignSelector.Location = New System.Drawing.Point(364, 14)
        Me._alignSelector.Name = "_alignSelector"
        Me._alignSelector.Size = New System.Drawing.Size(155, 148)
        Me._alignSelector.TabIndex = 4
        '
        '_tooltip
        '
        Me._tooltip.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        '
        'nuGenSmoothCheckBox1
        '
        Me.nuGenSmoothCheckBox1.Location = New System.Drawing.Point(207, 50)
        Me.nuGenSmoothCheckBox1.Name = "nuGenSmoothCheckBox1"
        Me.nuGenSmoothCheckBox1.Size = New System.Drawing.Size(149, 17)
        Me.nuGenSmoothCheckBox1.TabIndex = 3
        Me.nuGenSmoothCheckBox1.Text = "nuGenSmoothCheckBox1"
        Me.nuGenSmoothCheckBox1.UseVisualStyleBackColor = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(752, 356)
        Me.Controls.Add(Me._bkgndPanel)
        Me.Controls.Add(Me.nuGenSmoothCheckBox1)
        Me.Name = "MainForm"
        Me.Text = "Control Gallery"
        Me.nuGenSwitchPage3.ResumeLayout(False)
        Me.nuGenSwitchPage4.ResumeLayout(False)
        Me._bkgndPanel.ResumeLayout(False)
        Me._switcher.ResumeLayout(False)
        Me.nuGenSwitchPage1.ResumeLayout(False)
        Me.nuGenSwitchPage1.PerformLayout()
        Me.nuGenSwitchPage2.ResumeLayout(False)
        Me.nuGenSwitchPage2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents nuGenSwitchPage3 As Genetibase.Shared.Controls.NuGenSwitchPage
    Private WithEvents _calendar As Genetibase.SmoothControls.NuGenSmoothCalendar
    Private WithEvents nuGenSwitchPage4 As Genetibase.Shared.Controls.NuGenSwitchPage
    Private WithEvents _unitsSpin As Genetibase.SmoothControls.NuGenSmoothUnitsSpin
    Private WithEvents nuGenSmoothFontSizeBox1 As Genetibase.SmoothControls.NuGenSmoothFontSizeBox
    Private WithEvents nuGenSmoothFontBox1 As Genetibase.SmoothControls.NuGenSmoothFontBox
    Private WithEvents nuGenSmoothDriveCombo1 As Genetibase.SmoothControls.NuGenSmoothDriveCombo
    Private WithEvents nuGenSmoothDirectorySelector1 As Genetibase.SmoothControls.NuGenSmoothDirectorySelector
    Private WithEvents _colorBoxPopup As Genetibase.SmoothControls.NuGenSmoothColorBoxPopup
    Private WithEvents _colorBox As Genetibase.SmoothControls.NuGenSmoothColorBox
    Private WithEvents _calcDropDown As Genetibase.SmoothControls.NuGenSmoothCalculatorDropDown
    Private WithEvents _alignDropDown As Genetibase.SmoothControls.NuGenSmoothAlignDropDown
    Private WithEvents _bkgndPanel As Genetibase.SmoothControls.NuGenSmoothPanel
    Private WithEvents _switcher As Genetibase.SmoothControls.NuGenSmoothSwitcher
    Private WithEvents nuGenSwitchPage1 As Genetibase.Shared.Controls.NuGenSwitchPage
    Private WithEvents _textBox As Genetibase.SmoothControls.NuGenSmoothTextBox
    Private WithEvents nuGenSmoothSpin1 As Genetibase.SmoothControls.NuGenSmoothSpin
    Private WithEvents _radioButton As Genetibase.SmoothControls.NuGenSmoothRadioButton
    Private WithEvents _pinpointList As Genetibase.SmoothControls.NuGenSmoothPinpointList
    Private WithEvents _optionSpin As Genetibase.SmoothControls.NuGenSmoothOptionSpin
    Private WithEvents _listBox As Genetibase.SmoothControls.NuGenSmoothListBox
    Private WithEvents _comboBox As Genetibase.SmoothControls.NuGenSmoothComboBox
    Private WithEvents _checkBox As Genetibase.SmoothControls.NuGenSmoothCheckBox
    Private WithEvents _button As Genetibase.SmoothControls.NuGenSmoothButton
    Private WithEvents _tooltip As Genetibase.SmoothControls.NuGenSmoothToolTip
    Private WithEvents nuGenSwitchPage2 As Genetibase.Shared.Controls.NuGenSwitchPage
    Private WithEvents nuGenSmoothHotKeySelector1 As Genetibase.SmoothControls.NuGenSmoothHotKeySelector
    Private WithEvents _menuButton As Genetibase.SmoothControls.NuGenSmoothMenuButton
    Private WithEvents _alignSelector As Genetibase.SmoothControls.NuGenSmoothAlignSelector
    Private WithEvents nuGenSmoothCheckBox1 As Genetibase.SmoothControls.NuGenSmoothCheckBox

End Class

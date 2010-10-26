namespace CtrlGallery
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this._switcher = new Genetibase.SmoothControls.NuGenSmoothSwitcher();
			this.nuGenSwitchPage1 = new Genetibase.Shared.Controls.NuGenSwitchPage();
			this._textBox = new Genetibase.SmoothControls.NuGenSmoothTextBox();
			this.nuGenSmoothSpin1 = new Genetibase.SmoothControls.NuGenSmoothSpin();
			this._radioButton = new Genetibase.SmoothControls.NuGenSmoothRadioButton();
			this._pinpointList = new Genetibase.SmoothControls.NuGenSmoothPinpointList();
			this._optionSpin = new Genetibase.SmoothControls.NuGenSmoothOptionSpin();
			this._listBox = new Genetibase.SmoothControls.NuGenSmoothListBox();
			this._comboBox = new Genetibase.SmoothControls.NuGenSmoothComboBox();
			this._checkBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._button = new Genetibase.SmoothControls.NuGenSmoothButton();
			this.nuGenSwitchPage2 = new Genetibase.Shared.Controls.NuGenSwitchPage();
			this.nuGenSmoothHotKeySelector1 = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._menuButton = new Genetibase.SmoothControls.NuGenSmoothMenuButton();
			this._alignSelector = new Genetibase.SmoothControls.NuGenSmoothAlignSelector();
			this._colorBoxPopup = new Genetibase.SmoothControls.NuGenSmoothColorBoxPopup();
			this._colorBox = new Genetibase.SmoothControls.NuGenSmoothColorBox();
			this._calcDropDown = new Genetibase.SmoothControls.NuGenSmoothCalculatorDropDown();
			this._alignDropDown = new Genetibase.SmoothControls.NuGenSmoothAlignDropDown();
			this.nuGenSwitchPage3 = new Genetibase.Shared.Controls.NuGenSwitchPage();
			this._calendar = new Genetibase.SmoothControls.NuGenSmoothCalendar();
			this.nuGenSwitchPage4 = new Genetibase.Shared.Controls.NuGenSwitchPage();
			this._unitsSpin = new Genetibase.SmoothControls.NuGenSmoothUnitsSpin();
			this.nuGenSmoothFontSizeBox1 = new Genetibase.SmoothControls.NuGenSmoothFontSizeBox();
			this.nuGenSmoothFontBox1 = new Genetibase.SmoothControls.NuGenSmoothFontBox();
			this.nuGenSmoothDriveCombo1 = new Genetibase.SmoothControls.NuGenSmoothDriveCombo();
			this.nuGenSmoothDirectorySelector1 = new Genetibase.SmoothControls.NuGenSmoothDirectorySelector();
			this._bkgndPanel = new Genetibase.SmoothControls.NuGenSmoothPanel();
			this._tooltip = new Genetibase.SmoothControls.NuGenSmoothToolTip();
			this.nuGenSmoothCheckBox1 = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._switcher.SuspendLayout();
			this.nuGenSwitchPage1.SuspendLayout();
			this.nuGenSwitchPage2.SuspendLayout();
			this.nuGenSwitchPage3.SuspendLayout();
			this.nuGenSwitchPage4.SuspendLayout();
			this._bkgndPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _switcher
			// 
			this._switcher.Dock = System.Windows.Forms.DockStyle.Fill;
			this._switcher.Location = new System.Drawing.Point(0, 0);
			this._switcher.Name = "_switcher";
			this._switcher.Size = new System.Drawing.Size(752, 356);
			this._switcher.SwitchButtonSize = new System.Drawing.Size(120, 54);
			this._switcher.SwitchPages.Add(this.nuGenSwitchPage1);
			this._switcher.SwitchPages.Add(this.nuGenSwitchPage2);
			this._switcher.SwitchPages.Add(this.nuGenSwitchPage3);
			this._switcher.SwitchPages.Add(this.nuGenSwitchPage4);
			this._switcher.SwitchPanelSize = new System.Drawing.Size(54, 70);
			this._switcher.TabIndex = 0;
			// 
			// nuGenSwitchPage1
			// 
			this.nuGenSwitchPage1.Controls.Add(this._textBox);
			this.nuGenSwitchPage1.Controls.Add(this.nuGenSmoothSpin1);
			this.nuGenSwitchPage1.Controls.Add(this._radioButton);
			this.nuGenSwitchPage1.Controls.Add(this._pinpointList);
			this.nuGenSwitchPage1.Controls.Add(this._optionSpin);
			this.nuGenSwitchPage1.Controls.Add(this._listBox);
			this.nuGenSwitchPage1.Controls.Add(this._comboBox);
			this.nuGenSwitchPage1.Controls.Add(this._checkBox);
			this.nuGenSwitchPage1.Controls.Add(this._button);
			this.nuGenSwitchPage1.Name = "nuGenSwitchPage1";
			this.nuGenSwitchPage1.SwitchButtonImage = ((System.Drawing.Image)(resources.GetObject("nuGenSwitchPage1.SwitchButtonImage")));
			this.nuGenSwitchPage1.TabIndex = 1;
			this.nuGenSwitchPage1.Text = "Common Controls";
			// 
			// _textBox
			// 
			this._textBox.InvalidTextBackColor = System.Drawing.Color.LightGreen;
			this._textBox.Location = new System.Drawing.Point(489, 14);
			this._textBox.Name = "_textBox";
			this._textBox.Pattern = "";
			this._textBox.PatternMode = Genetibase.Shared.Controls.NuGenTextBoxPatternMode.Digits;
			this._textBox.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._textBox.PromptText = "Only digits are allowed";
			this._textBox.Size = new System.Drawing.Size(150, 21);
			this._textBox.TabIndex = 11;
			this._textBox.UseColors = true;
			// 
			// nuGenSmoothSpin1
			// 
			this.nuGenSmoothSpin1.Location = new System.Drawing.Point(207, 125);
			this.nuGenSmoothSpin1.Name = "nuGenSmoothSpin1";
			this.nuGenSmoothSpin1.Size = new System.Drawing.Size(133, 20);
			this.nuGenSmoothSpin1.TabIndex = 10;
			// 
			// _radioButton
			// 
			this._radioButton.Location = new System.Drawing.Point(207, 73);
			this._radioButton.Name = "_radioButton";
			this._radioButton.Size = new System.Drawing.Size(84, 17);
			this._radioButton.TabIndex = 9;
			this._radioButton.Text = "RadioButton";
			this._radioButton.UseVisualStyleBackColor = false;
			// 
			// _pinpointList
			// 
			this._pinpointList.Dock = System.Windows.Forms.DockStyle.Left;
			this._pinpointList.Items.AddRange(new object[] {
            "AlignDropDown",
            "AlignSelector",
            "Bevel",
            "Button",
            "BlendSelector",
            "CalculatorDropDown",
            "Calendar",
            "CheckBox",
            "CloseButton",
            "ColorBox",
            "ColorBoxPopup",
            "ColorHistory",
            "ComboBox",
            "CommandManager",
            "ContextMenuStrip",
            "ControlReflector",
            "DirectorySelector",
            "DriveCombo",
            "DropDown",
            "FontBox",
            "FontSizeBox",
            "GradientPanel",
            "GroupBox",
            "HotKeySelector",
            "HSVPlainSelector",
            "HSVWheelSelector",
            "Int32Combo",
            "Label",
            "LinkLabel",
            "ListBox",
            "MatrixLabel",
            "MenuButton",
            "MiniBar",
            "NavigationBar",
            "OpenFileSelector",
            "OptionSpin",
            "Panel",
            "PanelEx",
            "PictureBox",
            "PinpointList",
            "PinpointWindow",
            "PopupContainer",
            "PrintPreview",
            "ProgressBar",
            "PropertyGrid",
            "RadioButton",
            "RoundedPanelEx",
            "ScrollBar",
            "ScrollButton",
            "SegDisplay",
            "SplitContainer",
            "Spin",
            "Spacer",
            "StatusStrip",
            "SwitchButton",
            "Switcher",
            "TabControl",
            "TaskBox",
            "Title",
            "TitlePanel",
            "ToolStrip",
            "ToolStripManager",
            "ToolTip",
            "TrackBar",
            "TreeView",
            "UISnap",
            "UnitsSpin",
            "Watermark"});
			this._pinpointList.Location = new System.Drawing.Point(0, 0);
			this._pinpointList.MinimumSize = new System.Drawing.Size(10, 10);
			this._pinpointList.Name = "_pinpointList";
			this._pinpointList.Size = new System.Drawing.Size(201, 286);
			this._pinpointList.TabIndex = 8;
			// 
			// _optionSpin
			// 
			this._optionSpin.Items.Add("Ambient");
			this._optionSpin.Items.Add("Black");
			this._optionSpin.Items.Add("House");
			this._optionSpin.Items.Add("Metal");
			this._optionSpin.Items.Add("Rock");
			this._optionSpin.Location = new System.Drawing.Point(207, 151);
			this._optionSpin.Name = "_optionSpin";
			this._optionSpin.Size = new System.Drawing.Size(133, 20);
			this._optionSpin.TabIndex = 7;
			this._optionSpin.Text = "Rock";
			// 
			// _listBox
			// 
			this._listBox.FormattingEnabled = true;
			this._listBox.Items.AddRange(new object[] {
            "AlignDropDown",
            "AlignSelector",
            "Bevel",
            "Button",
            "BlendSelector",
            "CalculatorDropDown",
            "Calendar",
            "CheckBox",
            "CloseButton",
            "ColorBox",
            "ColorBoxPopup",
            "ColorHistory",
            "ComboBox",
            "CommandManager",
            "ContextMenuStrip",
            "ControlReflector",
            "DirectorySelector",
            "DriveCombo",
            "DropDown",
            "FontBox",
            "FontSizeBox",
            "GradientPanel",
            "GroupBox",
            "HotKeySelector",
            "HSVPlainSelector",
            "HSVWheelSelector",
            "Int32Combo",
            "Label",
            "LinkLabel",
            "ListBox",
            "MatrixLabel",
            "MenuButton",
            "MiniBar",
            "NavigationBar",
            "OpenFileSelector",
            "OptionSpin",
            "Panel",
            "PanelEx",
            "PictureBox",
            "PinpointList",
            "PinpointWindow",
            "PopupContainer",
            "PrintPreview",
            "ProgressBar",
            "PropertyGrid",
            "RadioButton",
            "RoundedPanelEx",
            "ScrollBar",
            "ScrollButton",
            "SegDisplay",
            "SplitContainer",
            "Spin",
            "Spacer",
            "StatusStrip",
            "SwitchButton",
            "Switcher",
            "TabControl",
            "TaskBox",
            "Title",
            "TitlePanel",
            "ToolStrip",
            "ToolStripManager",
            "ToolTip",
            "TrackBar",
            "TreeView",
            "UISnap",
            "UnitsSpin",
            "Watermark"});
			this._listBox.Location = new System.Drawing.Point(346, 14);
			this._listBox.Name = "_listBox";
			this._listBox.Size = new System.Drawing.Size(137, 161);
			this._listBox.TabIndex = 3;
			// 
			// _comboBox
			// 
			this._comboBox.FormattingEnabled = true;
			this._comboBox.Items.AddRange(new object[] {
            "Astarte",
            "Deep Purple",
            "Dimmu Borgir",
            "Kiss",
            "Nautilus Pompilius ;-)",
            "Pink Floyd",
            "Pantera",
            "ReAnima 8-)"});
			this._comboBox.Location = new System.Drawing.Point(207, 98);
			this._comboBox.Name = "_comboBox";
			this._comboBox.Size = new System.Drawing.Size(133, 21);
			this._comboBox.TabIndex = 2;
			this._comboBox.Text = "Astarte";
			// 
			// _checkBox
			// 
			this._checkBox.Location = new System.Drawing.Point(207, 50);
			this._checkBox.Name = "_checkBox";
			this._checkBox.Size = new System.Drawing.Size(75, 17);
			this._checkBox.TabIndex = 1;
			this._checkBox.Text = "CheckBox";
			this._checkBox.UseVisualStyleBackColor = false;
			// 
			// _button
			// 
			this._button.Location = new System.Drawing.Point(207, 14);
			this._button.Name = "_button";
			this._button.Size = new System.Drawing.Size(133, 30);
			this._button.TabIndex = 0;
			this._button.Text = "Wait for tooltip here...";
			this._tooltip.SetToolTip(this._button, new Genetibase.Shared.Controls.NuGenToolTipInfo("Microsoft Office 2007 like ToolTip", ((System.Drawing.Image)(resources.GetObject("_button.ToolTip"))), "Use our user-friendly tool-tip designer with in-place preview ability.", "Press F1 for more help", ((System.Drawing.Image)(resources.GetObject("_button.ToolTip1"))), "You can also specify custom tool-tip size if you are not satisfied with auto-size" +
						" logic results.", new System.Drawing.Size(0, 0)));
			this._button.UseVisualStyleBackColor = false;
			// 
			// nuGenSwitchPage2
			// 
			this.nuGenSwitchPage2.Controls.Add(this.nuGenSmoothHotKeySelector1);
			this.nuGenSwitchPage2.Controls.Add(this._menuButton);
			this.nuGenSwitchPage2.Controls.Add(this._alignSelector);
			this.nuGenSwitchPage2.Controls.Add(this._colorBoxPopup);
			this.nuGenSwitchPage2.Controls.Add(this._colorBox);
			this.nuGenSwitchPage2.Controls.Add(this._calcDropDown);
			this.nuGenSwitchPage2.Controls.Add(this._alignDropDown);
			this.nuGenSwitchPage2.Name = "nuGenSwitchPage2";
			this.nuGenSwitchPage2.SwitchButtonImage = ((System.Drawing.Image)(resources.GetObject("nuGenSwitchPage2.SwitchButtonImage")));
			this.nuGenSwitchPage2.TabIndex = 2;
			this.nuGenSwitchPage2.Text = "Advanced UI";
			// 
			// nuGenSmoothHotKeySelector1
			// 
			this.nuGenSmoothHotKeySelector1.Location = new System.Drawing.Point(525, 14);
			this.nuGenSmoothHotKeySelector1.Name = "nuGenSmoothHotKeySelector1";
			this.nuGenSmoothHotKeySelector1.Size = new System.Drawing.Size(155, 21);
			this.nuGenSmoothHotKeySelector1.TabIndex = 13;
			// 
			// _menuButton
			// 
			this._menuButton.Location = new System.Drawing.Point(12, 95);
			this._menuButton.Name = "_menuButton";
			this._menuButton.Size = new System.Drawing.Size(155, 24);
			this._menuButton.TabIndex = 5;
			this._menuButton.Text = "&Help";
			this._menuButton.UseVisualStyleBackColor = false;
			// 
			// _alignSelector
			// 
			this._alignSelector.BackColor = System.Drawing.Color.Transparent;
			this._alignSelector.Location = new System.Drawing.Point(364, 14);
			this._alignSelector.Name = "_alignSelector";
			this._alignSelector.Size = new System.Drawing.Size(155, 148);
			this._alignSelector.TabIndex = 4;
			// 
			// _colorBoxPopup
			// 
			this._colorBoxPopup.Location = new System.Drawing.Point(173, 14);
			this._colorBoxPopup.Name = "_colorBoxPopup";
			this._colorBoxPopup.Size = new System.Drawing.Size(185, 260);
			this._colorBoxPopup.TabIndex = 3;
			// 
			// _colorBox
			// 
			this._colorBox.Location = new System.Drawing.Point(12, 41);
			this._colorBox.Name = "_colorBox";
			this._colorBox.Size = new System.Drawing.Size(155, 21);
			this._colorBox.TabIndex = 2;
			// 
			// _calcDropDown
			// 
			this._calcDropDown.Location = new System.Drawing.Point(12, 68);
			this._calcDropDown.Name = "_calcDropDown";
			this._calcDropDown.Size = new System.Drawing.Size(155, 21);
			this._calcDropDown.TabIndex = 1;
			// 
			// _alignDropDown
			// 
			this._alignDropDown.Location = new System.Drawing.Point(12, 14);
			this._alignDropDown.Name = "_alignDropDown";
			this._alignDropDown.Size = new System.Drawing.Size(155, 21);
			this._alignDropDown.TabIndex = 0;
			// 
			// nuGenSwitchPage3
			// 
			this.nuGenSwitchPage3.Controls.Add(this._calendar);
			this.nuGenSwitchPage3.Name = "nuGenSwitchPage3";
			this.nuGenSwitchPage3.SwitchButtonImage = ((System.Drawing.Image)(resources.GetObject("nuGenSwitchPage3.SwitchButtonImage")));
			this.nuGenSwitchPage3.TabIndex = 3;
			this.nuGenSwitchPage3.Text = "DateTime";
			// 
			// _calendar
			// 
			this._calendar.ActiveMonth.Month = 6;
			this._calendar.ActiveMonth.Year = 2007;
			this._calendar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this._calendar.Culture = new System.Globalization.CultureInfo("en-US");
			this._calendar.Footer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this._calendar.Header.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(236)))));
			this._calendar.Header.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(224)))), ((int)(((byte)(218)))));
			this._calendar.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this._calendar.Header.GradientMode = Genetibase.Shared.Controls.CalendarInternals.NuGenGradientMode.Vertical;
			this._calendar.ImageList = null;
			this._calendar.Location = new System.Drawing.Point(12, 14);
			this._calendar.MaxDate = new System.DateTime(2017, 6, 29, 17, 16, 56, 625);
			this._calendar.MinDate = new System.DateTime(1997, 6, 29, 17, 16, 56, 625);
			this._calendar.Month.BackgroundImage = null;
			this._calendar.Month.Colors.Focus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(236)))));
			this._calendar.Month.Colors.Focus.Border = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this._calendar.Month.Colors.Selected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(137)))), ((int)(((byte)(176)))));
			this._calendar.Month.Colors.Selected.Border = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(137)))), ((int)(((byte)(176)))));
			this._calendar.Month.DateFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this._calendar.Month.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this._calendar.Name = "_calendar";
			this._calendar.Size = new System.Drawing.Size(254, 257);
			this._calendar.TabIndex = 0;
			this._calendar.Weekdays.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this._calendar.Weekdays.TextColor = System.Drawing.Color.Black;
			this._calendar.Weeknumbers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this._calendar.Weeknumbers.TextColor = System.Drawing.Color.Black;
			// 
			// nuGenSwitchPage4
			// 
			this.nuGenSwitchPage4.Controls.Add(this._unitsSpin);
			this.nuGenSwitchPage4.Controls.Add(this.nuGenSmoothFontSizeBox1);
			this.nuGenSwitchPage4.Controls.Add(this.nuGenSmoothFontBox1);
			this.nuGenSwitchPage4.Controls.Add(this.nuGenSmoothDriveCombo1);
			this.nuGenSwitchPage4.Controls.Add(this.nuGenSmoothDirectorySelector1);
			this.nuGenSwitchPage4.Name = "nuGenSwitchPage4";
			this.nuGenSwitchPage4.SwitchButtonImage = ((System.Drawing.Image)(resources.GetObject("nuGenSwitchPage4.SwitchButtonImage")));
			this.nuGenSwitchPage4.TabIndex = 4;
			this.nuGenSwitchPage4.Text = "Specialized Controls";
			// 
			// _unitsSpin
			// 
			this._unitsSpin.Location = new System.Drawing.Point(195, 41);
			this._unitsSpin.Maximum = 100000000;
			this._unitsSpin.MeasureUnits = new string[] {
        "B",
        "KB",
        "MB"};
			this._unitsSpin.Name = "_unitsSpin";
			this._unitsSpin.Size = new System.Drawing.Size(106, 20);
			this._unitsSpin.TabIndex = 13;
			// 
			// nuGenSmoothFontSizeBox1
			// 
			this.nuGenSmoothFontSizeBox1.Location = new System.Drawing.Point(195, 68);
			this.nuGenSmoothFontSizeBox1.MaxDropDownItems = 12;
			this.nuGenSmoothFontSizeBox1.Name = "nuGenSmoothFontSizeBox1";
			this.nuGenSmoothFontSizeBox1.Size = new System.Drawing.Size(106, 21);
			this.nuGenSmoothFontSizeBox1.TabIndex = 3;
			this.nuGenSmoothFontSizeBox1.Value = 10;
			// 
			// nuGenSmoothFontBox1
			// 
			this.nuGenSmoothFontBox1.Location = new System.Drawing.Point(12, 68);
			this.nuGenSmoothFontBox1.Name = "nuGenSmoothFontBox1";
			this.nuGenSmoothFontBox1.Size = new System.Drawing.Size(177, 21);
			this.nuGenSmoothFontBox1.TabIndex = 2;
			// 
			// nuGenSmoothDriveCombo1
			// 
			this.nuGenSmoothDriveCombo1.Location = new System.Drawing.Point(12, 41);
			this.nuGenSmoothDriveCombo1.Name = "nuGenSmoothDriveCombo1";
			this.nuGenSmoothDriveCombo1.Size = new System.Drawing.Size(177, 21);
			this.nuGenSmoothDriveCombo1.TabIndex = 1;
			// 
			// nuGenSmoothDirectorySelector1
			// 
			this.nuGenSmoothDirectorySelector1.CanChooseDirectory = true;
			this.nuGenSmoothDirectorySelector1.Location = new System.Drawing.Point(12, 14);
			this.nuGenSmoothDirectorySelector1.Name = "nuGenSmoothDirectorySelector1";
			this.nuGenSmoothDirectorySelector1.Size = new System.Drawing.Size(289, 21);
			this.nuGenSmoothDirectorySelector1.TabIndex = 0;
			// 
			// _bkgndPanel
			// 
			this._bkgndPanel.Controls.Add(this._switcher);
			this._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._bkgndPanel.DrawBorder = false;
			this._bkgndPanel.Location = new System.Drawing.Point(0, 0);
			this._bkgndPanel.Name = "_bkgndPanel";
			this._bkgndPanel.Size = new System.Drawing.Size(752, 356);
			// 
			// _tooltip
			// 
			this._tooltip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			// 
			// nuGenSmoothCheckBox1
			// 
			this.nuGenSmoothCheckBox1.Location = new System.Drawing.Point(207, 50);
			this.nuGenSmoothCheckBox1.Name = "nuGenSmoothCheckBox1";
			this.nuGenSmoothCheckBox1.Size = new System.Drawing.Size(149, 17);
			this.nuGenSmoothCheckBox1.TabIndex = 1;
			this.nuGenSmoothCheckBox1.Text = "nuGenSmoothCheckBox1";
			this.nuGenSmoothCheckBox1.UseVisualStyleBackColor = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(752, 356);
			this.Controls.Add(this._bkgndPanel);
			this.Name = "MainForm";
			this.Text = "Control Gallery";
			this._switcher.ResumeLayout(false);
			this.nuGenSwitchPage1.ResumeLayout(false);
			this.nuGenSwitchPage1.PerformLayout();
			this.nuGenSwitchPage2.ResumeLayout(false);
			this.nuGenSwitchPage2.PerformLayout();
			this.nuGenSwitchPage3.ResumeLayout(false);
			this.nuGenSwitchPage4.ResumeLayout(false);
			this._bkgndPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothSwitcher _switcher;
		private Genetibase.Shared.Controls.NuGenSwitchPage nuGenSwitchPage1;
		private Genetibase.Shared.Controls.NuGenSwitchPage nuGenSwitchPage2;
		private Genetibase.Shared.Controls.NuGenSwitchPage nuGenSwitchPage3;
		private Genetibase.SmoothControls.NuGenSmoothPanel _bkgndPanel;
		private Genetibase.Shared.Controls.NuGenSwitchPage nuGenSwitchPage4;
		private Genetibase.SmoothControls.NuGenSmoothSpin nuGenSmoothSpin1;
		private Genetibase.SmoothControls.NuGenSmoothRadioButton _radioButton;
		private Genetibase.SmoothControls.NuGenSmoothPinpointList _pinpointList;
		private Genetibase.SmoothControls.NuGenSmoothOptionSpin _optionSpin;
		private Genetibase.SmoothControls.NuGenSmoothListBox _listBox;
		private Genetibase.SmoothControls.NuGenSmoothComboBox _comboBox;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox _checkBox;
		private Genetibase.SmoothControls.NuGenSmoothButton _button;
		private Genetibase.SmoothControls.NuGenSmoothMenuButton _menuButton;
		private Genetibase.SmoothControls.NuGenSmoothAlignSelector _alignSelector;
		private Genetibase.SmoothControls.NuGenSmoothColorBoxPopup _colorBoxPopup;
		private Genetibase.SmoothControls.NuGenSmoothColorBox _colorBox;
		private Genetibase.SmoothControls.NuGenSmoothCalculatorDropDown _calcDropDown;
		private Genetibase.SmoothControls.NuGenSmoothAlignDropDown _alignDropDown;
		private Genetibase.SmoothControls.NuGenSmoothCalendar _calendar;
		private Genetibase.SmoothControls.NuGenSmoothFontSizeBox nuGenSmoothFontSizeBox1;
		private Genetibase.SmoothControls.NuGenSmoothFontBox nuGenSmoothFontBox1;
		private Genetibase.SmoothControls.NuGenSmoothDriveCombo nuGenSmoothDriveCombo1;
		private Genetibase.SmoothControls.NuGenSmoothDirectorySelector nuGenSmoothDirectorySelector1;
		private Genetibase.SmoothControls.NuGenSmoothTextBox _textBox;
		private Genetibase.SmoothControls.NuGenSmoothUnitsSpin _unitsSpin;
		private Genetibase.SmoothControls.NuGenSmoothToolTip _tooltip;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox nuGenSmoothCheckBox1;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector nuGenSmoothHotKeySelector1;
	}
}


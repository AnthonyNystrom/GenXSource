using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Agilix.Ink;
using Lyquidity.UtilityLibrary.Controls; // Provides the rulers
using System.Reflection;
namespace StationeryBuilder
{
	/// <summary>
	/// Test all aspects of stationery settings.
	/// Also a handy stationery creation program
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		enum Units
		{
			HiMetric,
			CM,
			Inches
		};

		public class TrackBarAdjustment
		{
			public int HLine = 70;
			public int HLineBetween = 0;
			public int VLine = 100;
			public int VLineBetween = 0;
			public int RMargin = 120;
			public int LMargin = 70;
			public int TitleWidth = 300;
			public int TitleHeight = 80;
			public int TitleX = 150;
			public int TitleY = 300;
			public int TitleSpaceAfter = 80;
		}


		#region Colors
		Color[] color = new Color[]
		{
			Color.AliceBlue,
			Color.AntiqueWhite,
			Color.Aqua,
			Color.Aquamarine,
			Color.Azure,
			Color.Beige,
			Color.Bisque,
			Color.Black,
			Color.BlanchedAlmond,
			Color.Blue,
			Color.BlueViolet,
			Color.Brown,
			Color.BurlyWood,
			Color.CadetBlue,
			Color.Chartreuse,
			Color.Chocolate,
			Color.Coral,
			Color.Cornsilk,
			Color.Crimson,
			Color.Cyan,
			Color.DarkBlue,
			Color.DarkCyan,
			Color.DarkGoldenrod,
			Color.DarkGray,
			Color.DarkGreen,
			Color.DarkKhaki,
			Color.DarkMagenta,
			Color.DarkOliveGreen,
			Color.DarkOrange,
			Color.DarkOrchid,
			Color.DarkRed,
			Color.DarkSalmon,
			Color.DarkSeaGreen,
			Color.DarkSlateBlue,
			Color.DarkSlateGray,
			Color.DarkTurquoise,
			Color.DarkViolet,
			Color.DeepPink,
			Color.DeepSkyBlue,
			Color.DimGray,
			Color.DodgerBlue,
			Color.Firebrick,
			Color.FloralWhite,
			Color.ForestGreen,
			Color.Fuchsia,
			Color.Gainsboro,
			Color.GhostWhite,
			Color.Gold,
			Color.Goldenrod,
			Color.Gray,
			Color.Green,
			Color.GreenYellow,
			Color.Honeydew,
			Color.HotPink,
			Color.IndianRed,
			Color.Indigo,
			Color.Ivory,
			Color.Khaki,
			Color.Lavender,
			Color.LavenderBlush,
			Color.LawnGreen,
			Color.LemonChiffon,
			Color.LightBlue,
			Color.LightCoral,
			Color.LightCyan,
			Color.LightGoldenrodYellow,
			Color.LightGray,
			Color.LightGreen,
			Color.LightPink,
			Color.LightSalmon,
			Color.LightSeaGreen,
			Color.LightSkyBlue,
			Color.LightSlateGray,
			Color.LightSteelBlue,
			Color.LightYellow,
			Color.Lime,
			Color.LimeGreen,
			Color.Linen,
			Color.Magenta,
			Color.Maroon,
			Color.MediumAquamarine,
			Color.MediumBlue,
			Color.MediumOrchid,
			Color.MediumPurple,
			Color.MediumSeaGreen,
			Color.MediumSlateBlue,
			Color.MediumSpringGreen,
			Color.MediumTurquoise,
			Color.MediumVioletRed,
			Color.MidnightBlue,
			Color.MintCream,
			Color.MistyRose,
			Color.Moccasin,
			Color.NavajoWhite,
			Color.Navy,
			Color.OldLace,
			Color.Olive,
			Color.OliveDrab,
			Color.Orange,
			Color.OrangeRed,
			Color.Orchid,
			Color.PaleGoldenrod,
			Color.PaleGreen,
			Color.PaleTurquoise,
			Color.PaleVioletRed,
			Color.PapayaWhip,
			Color.PeachPuff,
			Color.Peru,
			Color.Pink,
			Color.Plum,
			Color.PowderBlue,
			Color.Purple,
			Color.Red,
			Color.RosyBrown,
			Color.RoyalBlue,
			Color.SaddleBrown,
			Color.Salmon,
			Color.SandyBrown,
			Color.SeaGreen,
			Color.SeaShell,
			Color.Sienna,
			Color.Silver,
			Color.SkyBlue,
			Color.SlateBlue,
			Color.SlateGray,
			Color.Snow,
			Color.SpringGreen,
			Color.SteelBlue,
			Color.Tan,
			Color.Teal,
			Color.Thistle,
			Color.Tomato,
			Color.Transparent,
			Color.Turquoise,
			Color.Violet,
			Color.Wheat,
			Color.White,
			Color.WhiteSmoke,
			Color.Yellow,
			Color.YellowGreen
		};
		#endregion
		
		#region Fields
		private System.Windows.Forms.TabPage linesTabPage;
		private System.Windows.Forms.TabPage headingTabPage;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage generalTabPage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Panel panel1;
		private Agilix.Ink.Scribble.Scribble scribble;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ComboBox hLineStyleComboBox;
		private System.Windows.Forms.ComboBox hLineColorComboBox;
		private System.Windows.Forms.ComboBox vLineColorComboBox;
		private System.Windows.Forms.ComboBox vLineStyleComboBox;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.ComboBox titleRectangleStyleComboBox;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.TextBox xTitleCornerTextBox;
		private System.Windows.Forms.TextBox yTitleCornerTextBox;
		private System.Windows.Forms.TextBox titleHeightTextBox;
		private System.Windows.Forms.TextBox titleWidthTextBox;
		private Rectangle fTitleRectangle = new Rectangle(100,100, 5000, 3000);
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.ComboBox backgroundColorComboBox;
		private System.Windows.Forms.ComboBox backgroundColorStyleComboBox;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Button backgroundImageButton;
		private System.Windows.Forms.ComboBox backgroundImageStyleComboBox;
		private System.Windows.Forms.TrackBar backgroundImageTransparencyTrackBar;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.Label backgroundImageTransparencyLabel;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.TextBox titleTextTextBox;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.ComboBox titleForegroundColorComboBox;
		private System.Windows.Forms.ComboBox titleBackgroundColorComboBox;
		private System.Windows.Forms.ComboBox titleDateStyleComboBox;
		private ImageComboBox backgroundImageComboBox;
		private System.Windows.Forms.ImageList backgroundImageList;
		private System.Windows.Forms.OpenFileDialog openBackgroundImageFileDialog;
		private System.Windows.Forms.TextBox titleSpaceAfterTextBox;
		private System.Windows.Forms.TrackBar hLineSpaceBetweenTrackBar;
		private System.Windows.Forms.TrackBar vLineSpaceBetweenTrackBar;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TrackBar hLineSpaceBeforeTrackBar;
		private System.Windows.Forms.TrackBar vLineSpaceBeforeTrackBar;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.TrackBar lMarginLineSpacingTrackBar;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.ComboBox lMarginLineColorComboBox;
		private System.Windows.Forms.ComboBox lMarginLineStyleComboBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TrackBar rMarginLineSpacingTrackBar;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox rMarginLineColorComboBox;
		private System.Windows.Forms.ComboBox rMarginLineStyleComboBox;
		private System.Windows.Forms.Button rightMarginLineColorChooserButton;
		private System.Windows.Forms.Button leftMarginLineColorChooserButton;
		private System.Windows.Forms.TextBox lMarginLineSpacingTextBox;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.TextBox rMarginLineSpacingTextBox;
		private System.Windows.Forms.Button hLineColorChooserButton;
		private System.Windows.Forms.Button vLineColorChooserButton;
		private System.Windows.Forms.TextBox hLineSpaceBeforeTextBox;
		private System.Windows.Forms.TextBox hLineSpaceBetweenTextBox;
		private System.Windows.Forms.TextBox vLineSpaceBeforeTextBox;
		private System.Windows.Forms.TextBox vLineSpaceBetweenTextBox;
		private System.Windows.Forms.Button titleForegroundColorChooserButton;
		private System.Windows.Forms.Button titleBackgroundColorChooserButton;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.TrackBar xTitleCornerTrackBar;
		private System.Windows.Forms.TrackBar yTitleCornerTrackBar;
		private System.Windows.Forms.TrackBar titleHeightTrackBar;
		private System.Windows.Forms.TrackBar titleWidthTrackBar;
		private System.Windows.Forms.TrackBar titleSpaceAfterTrackBar;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.Button backgroundColorChooserButton;
		private System.Windows.Forms.MenuItem openMenuItem;
		private System.Windows.Forms.MenuItem saveAsMenuItem;
		private System.Windows.Forms.MenuItem saveMenuItem;
		private System.Windows.Forms.MenuItem exitMenuItem;
		private System.Windows.Forms.MenuItem inchesMenuItem;
		private System.Windows.Forms.MenuItem centimetersMenuItem;
		private System.Windows.Forms.MenuItem himetricsMenuItem;
		private System.Windows.Forms.MenuItem zoomMenuItem;
		private System.Windows.Forms.MenuItem zoom25MenuItem;
		private System.Windows.Forms.MenuItem zoom50MenuItem;
		private System.Windows.Forms.MenuItem zoom100MenuItem;
		private System.Windows.Forms.MenuItem zoom150MenuItem;
		private System.Windows.Forms.MenuItem zoom200MenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.MenuItem menuItem2;
		private Units fUnitsSetting;
		private string fFileName = "";
		private string fTempFileName = "";
		private System.Windows.Forms.MenuItem paperSmallGridMenuItem;
		private System.Windows.Forms.MenuItem paperBlankMenuItem;
		private System.Windows.Forms.MenuItem paperNarrowMenuItem;
		private System.Windows.Forms.MenuItem paperCollegeMenuItem;
		private System.Windows.Forms.MenuItem paperStandardMenuItem;
		private System.Windows.Forms.MenuItem paperWideMenuItem;
		private System.Windows.Forms.MenuItem paperGridMenuItem;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.TextBox imageWidthTextBox;
		private System.Windows.Forms.TextBox imageHeightTextBox;
		private System.Windows.Forms.CheckBox alignImageWithMinSizeCheckBox;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.GroupBox groupBox10;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.TextBox sizeHeightOnlyTextBox;
		private System.Windows.Forms.TextBox sizeWidthOnlyTextBox;
		private StationerySettings fSettings;
		private TrackBarAdjustment fTrackBarAdjustment;
		private RulerControl hRuler;
		private System.Windows.Forms.MenuItem newMenuItem;
		private System.Windows.Forms.GroupBox groupBox11;
		private System.Windows.Forms.TextBox displayNameTextBox;
		private RulerControl vRuler;
		private string fTitleText = "NuGenStationary Builder";
		#endregion

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initial Settings
			// TODO: Save the settings to the registry and retrieve them at load
			fUnitsSetting = Units.CM;
			fSettings = new StationerySettings();
			fSettings.Blank();
			fTrackBarAdjustment = new TrackBarAdjustment();
			this.Text = fTitleText;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			// TODO: Save the application settings to the registry
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				Disposer();
			}
			base.Dispose( disposing );
		}
		private void Disposer()

		{
			Type type = this.GetType();
			if (type != null)
			{
				foreach(FieldInfo fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
				{
					object field = fi.GetValue(this);
					if (field is IDisposable)
					{
						((IDisposable)field).Dispose();
						
					}
				}
			}
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.hRuler = new Lyquidity.UtilityLibrary.Controls.RulerControl();
			this.vRuler = new Lyquidity.UtilityLibrary.Controls.RulerControl();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.linesTabPage = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.vLineSpaceBetweenTextBox = new System.Windows.Forms.TextBox();
			this.vLineSpaceBeforeTextBox = new System.Windows.Forms.TextBox();
			this.vLineSpaceBeforeTrackBar = new System.Windows.Forms.TrackBar();
			this.vLineSpaceBetweenTrackBar = new System.Windows.Forms.TrackBar();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.vLineColorComboBox = new System.Windows.Forms.ComboBox();
			this.vLineStyleComboBox = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.vLineColorChooserButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.hLineSpaceBetweenTextBox = new System.Windows.Forms.TextBox();
			this.hLineSpaceBeforeTextBox = new System.Windows.Forms.TextBox();
			this.hLineColorChooserButton = new System.Windows.Forms.Button();
			this.hLineSpaceBeforeTrackBar = new System.Windows.Forms.TrackBar();
			this.hLineSpaceBetweenTrackBar = new System.Windows.Forms.TrackBar();
			this.hLineColorComboBox = new System.Windows.Forms.ComboBox();
			this.hLineStyleComboBox = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.lMarginLineSpacingTextBox = new System.Windows.Forms.TextBox();
			this.lMarginLineSpacingTrackBar = new System.Windows.Forms.TrackBar();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.lMarginLineColorComboBox = new System.Windows.Forms.ComboBox();
			this.lMarginLineStyleComboBox = new System.Windows.Forms.ComboBox();
			this.leftMarginLineColorChooserButton = new System.Windows.Forms.Button();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rMarginLineSpacingTextBox = new System.Windows.Forms.TextBox();
			this.rightMarginLineColorChooserButton = new System.Windows.Forms.Button();
			this.rMarginLineSpacingTrackBar = new System.Windows.Forms.TrackBar();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.rMarginLineColorComboBox = new System.Windows.Forms.ComboBox();
			this.rMarginLineStyleComboBox = new System.Windows.Forms.ComboBox();
			this.headingTabPage = new System.Windows.Forms.TabPage();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.displayNameTextBox = new System.Windows.Forms.TextBox();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.titleSpaceAfterTrackBar = new System.Windows.Forms.TrackBar();
			this.titleSpaceAfterTextBox = new System.Windows.Forms.TextBox();
			this.titleDateStyleComboBox = new System.Windows.Forms.ComboBox();
			this.label29 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.titleTextTextBox = new System.Windows.Forms.TextBox();
			this.label24 = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.titleWidthTrackBar = new System.Windows.Forms.TrackBar();
			this.titleHeightTrackBar = new System.Windows.Forms.TrackBar();
			this.yTitleCornerTrackBar = new System.Windows.Forms.TrackBar();
			this.xTitleCornerTrackBar = new System.Windows.Forms.TrackBar();
			this.label32 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.titleBackgroundColorChooserButton = new System.Windows.Forms.Button();
			this.titleForegroundColorChooserButton = new System.Windows.Forms.Button();
			this.titleWidthTextBox = new System.Windows.Forms.TextBox();
			this.titleHeightTextBox = new System.Windows.Forms.TextBox();
			this.yTitleCornerTextBox = new System.Windows.Forms.TextBox();
			this.xTitleCornerTextBox = new System.Windows.Forms.TextBox();
			this.label21 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.titleRectangleStyleComboBox = new System.Windows.Forms.ComboBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.titleBackgroundColorComboBox = new System.Windows.Forms.ComboBox();
			this.titleForegroundColorComboBox = new System.Windows.Forms.ComboBox();
			this.label31 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.generalTabPage = new System.Windows.Forms.TabPage();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.sizeWidthOnlyTextBox = new System.Windows.Forms.TextBox();
			this.sizeHeightOnlyTextBox = new System.Windows.Forms.TextBox();
			this.label36 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.label34 = new System.Windows.Forms.Label();
			this.alignImageWithMinSizeCheckBox = new System.Windows.Forms.CheckBox();
			this.imageHeightTextBox = new System.Windows.Forms.TextBox();
			this.imageWidthTextBox = new System.Windows.Forms.TextBox();
			this.label33 = new System.Windows.Forms.Label();
			this.backgroundImageComboBox = new StationeryBuilder.ImageComboBox();
			this.backgroundImageTransparencyLabel = new System.Windows.Forms.Label();
			this.backgroundImageTransparencyTrackBar = new System.Windows.Forms.TrackBar();
			this.label25 = new System.Windows.Forms.Label();
			this.backgroundImageButton = new System.Windows.Forms.Button();
			this.backgroundImageStyleComboBox = new System.Windows.Forms.ComboBox();
			this.label27 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.backgroundColorChooserButton = new System.Windows.Forms.Button();
			this.backgroundColorStyleComboBox = new System.Windows.Forms.ComboBox();
			this.backgroundColorComboBox = new System.Windows.Forms.ComboBox();
			this.label23 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.scribble = new Agilix.Ink.Scribble.Scribble();
			this.backgroundImageList = new System.Windows.Forms.ImageList(this.components);
			this.openBackgroundImageFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.newMenuItem = new System.Windows.Forms.MenuItem();
			this.openMenuItem = new System.Windows.Forms.MenuItem();
			this.saveAsMenuItem = new System.Windows.Forms.MenuItem();
			this.saveMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.exitMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.paperBlankMenuItem = new System.Windows.Forms.MenuItem();
			this.paperNarrowMenuItem = new System.Windows.Forms.MenuItem();
			this.paperCollegeMenuItem = new System.Windows.Forms.MenuItem();
			this.paperStandardMenuItem = new System.Windows.Forms.MenuItem();
			this.paperWideMenuItem = new System.Windows.Forms.MenuItem();
			this.paperSmallGridMenuItem = new System.Windows.Forms.MenuItem();
			this.paperGridMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.inchesMenuItem = new System.Windows.Forms.MenuItem();
			this.centimetersMenuItem = new System.Windows.Forms.MenuItem();
			this.himetricsMenuItem = new System.Windows.Forms.MenuItem();
			this.zoomMenuItem = new System.Windows.Forms.MenuItem();
			this.zoom25MenuItem = new System.Windows.Forms.MenuItem();
			this.zoom50MenuItem = new System.Windows.Forms.MenuItem();
			this.zoom100MenuItem = new System.Windows.Forms.MenuItem();
			this.zoom150MenuItem = new System.Windows.Forms.MenuItem();
			this.zoom200MenuItem = new System.Windows.Forms.MenuItem();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.tabControl.SuspendLayout();
			this.linesTabPage.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.vLineSpaceBeforeTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.vLineSpaceBetweenTrackBar)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.hLineSpaceBeforeTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.hLineSpaceBetweenTrackBar)).BeginInit();
			this.tabPage1.SuspendLayout();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lMarginLineSpacingTrackBar)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.rMarginLineSpacingTrackBar)).BeginInit();
			this.headingTabPage.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox9.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.titleSpaceAfterTrackBar)).BeginInit();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.titleWidthTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.titleHeightTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.yTitleCornerTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.xTitleCornerTrackBar)).BeginInit();
			this.generalTabPage.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.groupBox8.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.backgroundImageTransparencyTrackBar)).BeginInit();
			this.groupBox7.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// hRuler
			// 
			this.hRuler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.hRuler.BackColor = System.Drawing.Color.Khaki;
			this.hRuler.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
			this.hRuler.DivisionMarkFactor = 5;
			this.hRuler.Divisions = 4;
			this.hRuler.ForeColor = System.Drawing.SystemColors.ControlText;
			this.hRuler.Location = new System.Drawing.Point(22, 0);
			this.hRuler.MajorInterval = 1;
			this.hRuler.MiddleMarkFactor = 3;
			this.hRuler.MouseTrackingOn = false;
			this.hRuler.Name = "hRuler";
			this.hRuler.Orientation = Lyquidity.UtilityLibrary.Controls.enumOrientation.orHorizontal;
			this.hRuler.RulerAlignment = Lyquidity.UtilityLibrary.Controls.enumRulerAlignment.raBottomOrRight;
			this.hRuler.ScaleMode = Lyquidity.UtilityLibrary.Controls.enumScaleMode.smCentimetres;
			this.hRuler.Size = new System.Drawing.Size(508, 20);
			this.hRuler.StartValue = 0;
			this.hRuler.TabIndex = 0;
			this.hRuler.TabStop = false;
			this.hRuler.VerticalNumbers = false;
			this.hRuler.ZoomFactor = 1;
			// 
			// vRuler
			// 
			this.vRuler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.vRuler.BackColor = System.Drawing.Color.Khaki;
			this.vRuler.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
			this.vRuler.DivisionMarkFactor = 5;
			this.vRuler.Divisions = 4;
			this.vRuler.ForeColor = System.Drawing.SystemColors.ControlText;
			this.vRuler.Location = new System.Drawing.Point(0, 22);
			this.vRuler.MajorInterval = 1;
			this.vRuler.MiddleMarkFactor = 3;
			this.vRuler.MouseTrackingOn = false;
			this.vRuler.Name = "vRuler";
			this.vRuler.Orientation = Lyquidity.UtilityLibrary.Controls.enumOrientation.orVertical;
			this.vRuler.RulerAlignment = Lyquidity.UtilityLibrary.Controls.enumRulerAlignment.raTopOrLeft;
			this.vRuler.ScaleMode = Lyquidity.UtilityLibrary.Controls.enumScaleMode.smCentimetres;
			this.vRuler.Size = new System.Drawing.Size(20, 682);
			this.vRuler.StartValue = 0;
			this.vRuler.TabIndex = 11;
			this.vRuler.TabStop = false;
			this.vRuler.VerticalNumbers = true;
			this.vRuler.ZoomFactor = 1;
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.linesTabPage);
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.headingTabPage);
			this.tabControl.Controls.Add(this.generalTabPage);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Right;
			this.tabControl.Location = new System.Drawing.Point(530, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(312, 703);
			this.tabControl.TabIndex = 0;
			// 
			// linesTabPage
			// 
			this.linesTabPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("linesTabPage.BackgroundImage")));
			this.linesTabPage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.linesTabPage.Controls.Add(this.groupBox2);
			this.linesTabPage.Controls.Add(this.label1);
			this.linesTabPage.Controls.Add(this.groupBox1);
			this.linesTabPage.Location = new System.Drawing.Point(4, 22);
			this.linesTabPage.Name = "linesTabPage";
			this.linesTabPage.Size = new System.Drawing.Size(304, 677);
			this.linesTabPage.TabIndex = 0;
			this.linesTabPage.Text = "Lines";
			// 
			// groupBox2
			// 
			this.groupBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox2.BackgroundImage")));
			this.groupBox2.Controls.Add(this.vLineSpaceBetweenTextBox);
			this.groupBox2.Controls.Add(this.vLineSpaceBeforeTextBox);
			this.groupBox2.Controls.Add(this.vLineSpaceBeforeTrackBar);
			this.groupBox2.Controls.Add(this.vLineSpaceBetweenTrackBar);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.vLineColorComboBox);
			this.groupBox2.Controls.Add(this.vLineStyleComboBox);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.vLineColorChooserButton);
			this.groupBox2.Location = new System.Drawing.Point(8, 264);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(288, 248);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Vertical";
			// 
			// vLineSpaceBetweenTextBox
			// 
			this.vLineSpaceBetweenTextBox.Location = new System.Drawing.Point(94, 168);
			this.vLineSpaceBetweenTextBox.Name = "vLineSpaceBetweenTextBox";
			this.vLineSpaceBetweenTextBox.TabIndex = 16;
			this.vLineSpaceBetweenTextBox.Text = "";
			this.vLineSpaceBetweenTextBox.TextChanged += new System.EventHandler(this.vLineSpaceBetweenTextBox_TextChanged);
			// 
			// vLineSpaceBeforeTextBox
			// 
			this.vLineSpaceBeforeTextBox.Location = new System.Drawing.Point(96, 80);
			this.vLineSpaceBeforeTextBox.Name = "vLineSpaceBeforeTextBox";
			this.vLineSpaceBeforeTextBox.TabIndex = 15;
			this.vLineSpaceBeforeTextBox.Text = "";
			this.vLineSpaceBeforeTextBox.TextChanged += new System.EventHandler(this.vLineSpaceBeforeTextBox_TextChanged);
			// 
			// vLineSpaceBeforeTrackBar
			// 
			this.vLineSpaceBeforeTrackBar.BackColor = System.Drawing.Color.WhiteSmoke;
			this.vLineSpaceBeforeTrackBar.Location = new System.Drawing.Point(8, 112);
			this.vLineSpaceBeforeTrackBar.Maximum = 100;
			this.vLineSpaceBeforeTrackBar.Name = "vLineSpaceBeforeTrackBar";
			this.vLineSpaceBeforeTrackBar.Size = new System.Drawing.Size(272, 45);
			this.vLineSpaceBeforeTrackBar.TabIndex = 14;
			this.vLineSpaceBeforeTrackBar.TickFrequency = 10;
			this.vLineSpaceBeforeTrackBar.Scroll += new System.EventHandler(this.vLineSpaceBeforeTrackBar_Scroll);
			// 
			// vLineSpaceBetweenTrackBar
			// 
			this.vLineSpaceBetweenTrackBar.BackColor = System.Drawing.Color.WhiteSmoke;
			this.vLineSpaceBetweenTrackBar.Location = new System.Drawing.Point(8, 200);
			this.vLineSpaceBetweenTrackBar.Maximum = 100;
			this.vLineSpaceBetweenTrackBar.Name = "vLineSpaceBetweenTrackBar";
			this.vLineSpaceBetweenTrackBar.Size = new System.Drawing.Size(272, 45);
			this.vLineSpaceBetweenTrackBar.TabIndex = 13;
			this.vLineSpaceBetweenTrackBar.TickFrequency = 10;
			this.vLineSpaceBetweenTrackBar.Scroll += new System.EventHandler(this.vLineSpaceBetweenTrackBar_Scroll);
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Location = new System.Drawing.Point(8, 176);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(96, 24);
			this.label6.TabIndex = 4;
			this.label6.Text = "Space Between:";
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.Location = new System.Drawing.Point(8, 88);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 24);
			this.label7.TabIndex = 3;
			this.label7.Text = "Space Before:";
			// 
			// label8
			// 
			this.label8.BackColor = System.Drawing.Color.Transparent;
			this.label8.Location = new System.Drawing.Point(8, 24);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 24);
			this.label8.TabIndex = 2;
			this.label8.Text = "Style:";
			// 
			// vLineColorComboBox
			// 
			this.vLineColorComboBox.Location = new System.Drawing.Point(64, 16);
			this.vLineColorComboBox.Name = "vLineColorComboBox";
			this.vLineColorComboBox.Size = new System.Drawing.Size(176, 21);
			this.vLineColorComboBox.TabIndex = 10;
			this.vLineColorComboBox.SelectedValueChanged += new System.EventHandler(this.vLineColorComboBox_SelectedValueChanged);
			// 
			// vLineStyleComboBox
			// 
			this.vLineStyleComboBox.Location = new System.Drawing.Point(64, 48);
			this.vLineStyleComboBox.Name = "vLineStyleComboBox";
			this.vLineStyleComboBox.Size = new System.Drawing.Size(208, 21);
			this.vLineStyleComboBox.TabIndex = 9;
			this.vLineStyleComboBox.SelectedValueChanged += new System.EventHandler(this.vLineStyleComboBox_SelectedValueChanged);
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Location = new System.Drawing.Point(8, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 24);
			this.label5.TabIndex = 3;
			this.label5.Text = "Color:";
			// 
			// vLineColorChooserButton
			// 
			this.vLineColorChooserButton.Location = new System.Drawing.Point(248, 16);
			this.vLineColorChooserButton.Name = "vLineColorChooserButton";
			this.vLineColorChooserButton.Size = new System.Drawing.Size(24, 23);
			this.vLineColorChooserButton.TabIndex = 13;
			this.vLineColorChooserButton.Text = "...";
			this.vLineColorChooserButton.Click += new System.EventHandler(this.vLineColorChooserButton_Click);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(16, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "Color:";
			// 
			// groupBox1
			// 
			this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
			this.groupBox1.Controls.Add(this.hLineSpaceBetweenTextBox);
			this.groupBox1.Controls.Add(this.hLineSpaceBeforeTextBox);
			this.groupBox1.Controls.Add(this.hLineColorChooserButton);
			this.groupBox1.Controls.Add(this.hLineSpaceBeforeTrackBar);
			this.groupBox1.Controls.Add(this.hLineSpaceBetweenTrackBar);
			this.groupBox1.Controls.Add(this.hLineColorComboBox);
			this.groupBox1.Controls.Add(this.hLineStyleComboBox);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 248);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Horizontal";
			// 
			// hLineSpaceBetweenTextBox
			// 
			this.hLineSpaceBetweenTextBox.Location = new System.Drawing.Point(96, 168);
			this.hLineSpaceBetweenTextBox.Name = "hLineSpaceBetweenTextBox";
			this.hLineSpaceBetweenTextBox.TabIndex = 14;
			this.hLineSpaceBetweenTextBox.Text = "";
			this.hLineSpaceBetweenTextBox.TextChanged += new System.EventHandler(this.hLineSpaceBetweenTextBox_TextChanged);
			// 
			// hLineSpaceBeforeTextBox
			// 
			this.hLineSpaceBeforeTextBox.Location = new System.Drawing.Point(96, 80);
			this.hLineSpaceBeforeTextBox.Name = "hLineSpaceBeforeTextBox";
			this.hLineSpaceBeforeTextBox.TabIndex = 13;
			this.hLineSpaceBeforeTextBox.Text = "";
			this.hLineSpaceBeforeTextBox.TextChanged += new System.EventHandler(this.hLineSpaceBeforeTextBox_TextChanged);
			// 
			// hLineColorChooserButton
			// 
			this.hLineColorChooserButton.Location = new System.Drawing.Point(248, 16);
			this.hLineColorChooserButton.Name = "hLineColorChooserButton";
			this.hLineColorChooserButton.Size = new System.Drawing.Size(24, 23);
			this.hLineColorChooserButton.TabIndex = 12;
			this.hLineColorChooserButton.Text = "...";
			this.hLineColorChooserButton.Click += new System.EventHandler(this.hLineColorChooserButton_Click);
			// 
			// hLineSpaceBeforeTrackBar
			// 
			this.hLineSpaceBeforeTrackBar.BackColor = System.Drawing.Color.WhiteSmoke;
			this.hLineSpaceBeforeTrackBar.Location = new System.Drawing.Point(8, 112);
			this.hLineSpaceBeforeTrackBar.Maximum = 100;
			this.hLineSpaceBeforeTrackBar.Name = "hLineSpaceBeforeTrackBar";
			this.hLineSpaceBeforeTrackBar.Size = new System.Drawing.Size(272, 45);
			this.hLineSpaceBeforeTrackBar.TabIndex = 11;
			this.hLineSpaceBeforeTrackBar.TickFrequency = 10;
			this.hLineSpaceBeforeTrackBar.Scroll += new System.EventHandler(this.hLineSpaceBeforeTrackBar_Scroll);
			// 
			// hLineSpaceBetweenTrackBar
			// 
			this.hLineSpaceBetweenTrackBar.BackColor = System.Drawing.Color.WhiteSmoke;
			this.hLineSpaceBetweenTrackBar.Location = new System.Drawing.Point(8, 200);
			this.hLineSpaceBetweenTrackBar.Maximum = 100;
			this.hLineSpaceBetweenTrackBar.Name = "hLineSpaceBetweenTrackBar";
			this.hLineSpaceBetweenTrackBar.Size = new System.Drawing.Size(272, 45);
			this.hLineSpaceBetweenTrackBar.TabIndex = 9;
			this.hLineSpaceBetweenTrackBar.TickFrequency = 10;
			this.hLineSpaceBetweenTrackBar.Scroll += new System.EventHandler(this.hLineSpaceBetweenTrackBar_Scroll);
			// 
			// hLineColorComboBox
			// 
			this.hLineColorComboBox.Location = new System.Drawing.Point(64, 16);
			this.hLineColorComboBox.Name = "hLineColorComboBox";
			this.hLineColorComboBox.Size = new System.Drawing.Size(176, 21);
			this.hLineColorComboBox.TabIndex = 6;
			this.hLineColorComboBox.SelectedIndexChanged += new System.EventHandler(this.hLineColorComboBox_SelectedIndexChanged_1);
			// 
			// hLineStyleComboBox
			// 
			this.hLineStyleComboBox.Location = new System.Drawing.Point(64, 48);
			this.hLineStyleComboBox.Name = "hLineStyleComboBox";
			this.hLineStyleComboBox.Size = new System.Drawing.Size(208, 21);
			this.hLineStyleComboBox.TabIndex = 5;
			this.hLineStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.hLineStyleComboBox_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(8, 176);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 24);
			this.label4.TabIndex = 4;
			this.label4.Text = "Space Between:";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(8, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 24);
			this.label3.TabIndex = 3;
			this.label3.Text = "Space Before:";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(8, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "Style:";
			// 
			// tabPage1
			// 
			this.tabPage1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPage1.BackgroundImage")));
			this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tabPage1.Controls.Add(this.groupBox5);
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(304, 677);
			this.tabPage1.TabIndex = 3;
			this.tabPage1.Text = "Margin Lines";
			// 
			// groupBox5
			// 
			this.groupBox5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox5.BackgroundImage")));
			this.groupBox5.Controls.Add(this.lMarginLineSpacingTextBox);
			this.groupBox5.Controls.Add(this.lMarginLineSpacingTrackBar);
			this.groupBox5.Controls.Add(this.label16);
			this.groupBox5.Controls.Add(this.label17);
			this.groupBox5.Controls.Add(this.lMarginLineColorComboBox);
			this.groupBox5.Controls.Add(this.lMarginLineStyleComboBox);
			this.groupBox5.Controls.Add(this.leftMarginLineColorChooserButton);
			this.groupBox5.Controls.Add(this.label15);
			this.groupBox5.Location = new System.Drawing.Point(8, 176);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(288, 160);
			this.groupBox5.TabIndex = 11;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Left Margin";
			// 
			// lMarginLineSpacingTextBox
			// 
			this.lMarginLineSpacingTextBox.Location = new System.Drawing.Point(72, 80);
			this.lMarginLineSpacingTextBox.Name = "lMarginLineSpacingTextBox";
			this.lMarginLineSpacingTextBox.TabIndex = 20;
			this.lMarginLineSpacingTextBox.Text = "";
			this.lMarginLineSpacingTextBox.TextChanged += new System.EventHandler(this.lMarginLineSpacingTextBox_TextChanged);
			// 
			// lMarginLineSpacingTrackBar
			// 
			this.lMarginLineSpacingTrackBar.BackColor = System.Drawing.Color.Beige;
			this.lMarginLineSpacingTrackBar.Location = new System.Drawing.Point(8, 112);
			this.lMarginLineSpacingTrackBar.Maximum = 100;
			this.lMarginLineSpacingTrackBar.Name = "lMarginLineSpacingTrackBar";
			this.lMarginLineSpacingTrackBar.Size = new System.Drawing.Size(272, 45);
			this.lMarginLineSpacingTrackBar.TabIndex = 16;
			this.lMarginLineSpacingTrackBar.TickFrequency = 10;
			this.lMarginLineSpacingTrackBar.Scroll += new System.EventHandler(this.lMarginLineSpacingTrackBar_Scroll);
			// 
			// label16
			// 
			this.label16.BackColor = System.Drawing.Color.Transparent;
			this.label16.Location = new System.Drawing.Point(8, 80);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(56, 24);
			this.label16.TabIndex = 3;
			this.label16.Text = "Spacing:";
			// 
			// label17
			// 
			this.label17.BackColor = System.Drawing.Color.Transparent;
			this.label17.Location = new System.Drawing.Point(8, 56);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(40, 24);
			this.label17.TabIndex = 2;
			this.label17.Text = "Style:";
			// 
			// lMarginLineColorComboBox
			// 
			this.lMarginLineColorComboBox.Location = new System.Drawing.Point(64, 16);
			this.lMarginLineColorComboBox.Name = "lMarginLineColorComboBox";
			this.lMarginLineColorComboBox.Size = new System.Drawing.Size(176, 21);
			this.lMarginLineColorComboBox.TabIndex = 14;
			this.lMarginLineColorComboBox.SelectedIndexChanged += new System.EventHandler(this.lMarginLineColorComboBox_SelectedIndexChanged);
			// 
			// lMarginLineStyleComboBox
			// 
			this.lMarginLineStyleComboBox.Location = new System.Drawing.Point(64, 48);
			this.lMarginLineStyleComboBox.Name = "lMarginLineStyleComboBox";
			this.lMarginLineStyleComboBox.Size = new System.Drawing.Size(208, 21);
			this.lMarginLineStyleComboBox.TabIndex = 13;
			this.lMarginLineStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.lMarginLineStyleComboBox_SelectedIndexChanged);
			// 
			// leftMarginLineColorChooserButton
			// 
			this.leftMarginLineColorChooserButton.Location = new System.Drawing.Point(248, 16);
			this.leftMarginLineColorChooserButton.Name = "leftMarginLineColorChooserButton";
			this.leftMarginLineColorChooserButton.Size = new System.Drawing.Size(24, 24);
			this.leftMarginLineColorChooserButton.TabIndex = 19;
			this.leftMarginLineColorChooserButton.Text = "...";
			this.leftMarginLineColorChooserButton.Click += new System.EventHandler(this.leftMarginLineColorChooserButton_Click);
			// 
			// label15
			// 
			this.label15.BackColor = System.Drawing.Color.Transparent;
			this.label15.Location = new System.Drawing.Point(8, 24);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(40, 24);
			this.label15.TabIndex = 10;
			this.label15.Text = "Color:";
			// 
			// groupBox3
			// 
			this.groupBox3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox3.BackgroundImage")));
			this.groupBox3.Controls.Add(this.rMarginLineSpacingTextBox);
			this.groupBox3.Controls.Add(this.rightMarginLineColorChooserButton);
			this.groupBox3.Controls.Add(this.rMarginLineSpacingTrackBar);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.rMarginLineColorComboBox);
			this.groupBox3.Controls.Add(this.rMarginLineStyleComboBox);
			this.groupBox3.Location = new System.Drawing.Point(8, 8);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(288, 160);
			this.groupBox3.TabIndex = 9;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Right Margin";
			// 
			// rMarginLineSpacingTextBox
			// 
			this.rMarginLineSpacingTextBox.Location = new System.Drawing.Point(64, 80);
			this.rMarginLineSpacingTextBox.Name = "rMarginLineSpacingTextBox";
			this.rMarginLineSpacingTextBox.TabIndex = 19;
			this.rMarginLineSpacingTextBox.Text = "";
			this.rMarginLineSpacingTextBox.TextChanged += new System.EventHandler(this.rMarginLineSpacingTextBox_TextChanged);
			// 
			// rightMarginLineColorChooserButton
			// 
			this.rightMarginLineColorChooserButton.Location = new System.Drawing.Point(248, 16);
			this.rightMarginLineColorChooserButton.Name = "rightMarginLineColorChooserButton";
			this.rightMarginLineColorChooserButton.Size = new System.Drawing.Size(24, 24);
			this.rightMarginLineColorChooserButton.TabIndex = 18;
			this.rightMarginLineColorChooserButton.Text = "...";
			this.rightMarginLineColorChooserButton.Click += new System.EventHandler(this.rightMarginLineColorChooserButton_Click);
			// 
			// rMarginLineSpacingTrackBar
			// 
			this.rMarginLineSpacingTrackBar.BackColor = System.Drawing.Color.Beige;
			this.rMarginLineSpacingTrackBar.Location = new System.Drawing.Point(8, 112);
			this.rMarginLineSpacingTrackBar.Maximum = 100;
			this.rMarginLineSpacingTrackBar.Name = "rMarginLineSpacingTrackBar";
			this.rMarginLineSpacingTrackBar.Size = new System.Drawing.Size(272, 45);
			this.rMarginLineSpacingTrackBar.TabIndex = 17;
			this.rMarginLineSpacingTrackBar.TickFrequency = 10;
			this.rMarginLineSpacingTrackBar.Scroll += new System.EventHandler(this.rMarginLineSpacingTrackBar_Scroll);
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.Transparent;
			this.label10.Location = new System.Drawing.Point(8, 24);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(40, 24);
			this.label10.TabIndex = 16;
			this.label10.Text = "Color:";
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Location = new System.Drawing.Point(8, 88);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(48, 24);
			this.label11.TabIndex = 3;
			this.label11.Text = "Spacing:";
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.Transparent;
			this.label12.Location = new System.Drawing.Point(8, 56);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(40, 24);
			this.label12.TabIndex = 2;
			this.label12.Text = "Style:";
			// 
			// rMarginLineColorComboBox
			// 
			this.rMarginLineColorComboBox.Location = new System.Drawing.Point(64, 16);
			this.rMarginLineColorComboBox.Name = "rMarginLineColorComboBox";
			this.rMarginLineColorComboBox.Size = new System.Drawing.Size(176, 21);
			this.rMarginLineColorComboBox.TabIndex = 14;
			this.rMarginLineColorComboBox.SelectedIndexChanged += new System.EventHandler(this.rMarginLineColorComboBox_SelectedIndexChanged);
			// 
			// rMarginLineStyleComboBox
			// 
			this.rMarginLineStyleComboBox.Location = new System.Drawing.Point(64, 48);
			this.rMarginLineStyleComboBox.Name = "rMarginLineStyleComboBox";
			this.rMarginLineStyleComboBox.Size = new System.Drawing.Size(208, 21);
			this.rMarginLineStyleComboBox.TabIndex = 13;
			this.rMarginLineStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.rMarginLineStyleComboBox_SelectedIndexChanged);
			// 
			// headingTabPage
			// 
			this.headingTabPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("headingTabPage.BackgroundImage")));
			this.headingTabPage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.headingTabPage.Controls.Add(this.groupBox11);
			this.headingTabPage.Controls.Add(this.groupBox9);
			this.headingTabPage.Controls.Add(this.groupBox6);
			this.headingTabPage.Location = new System.Drawing.Point(4, 22);
			this.headingTabPage.Name = "headingTabPage";
			this.headingTabPage.Size = new System.Drawing.Size(304, 677);
			this.headingTabPage.TabIndex = 1;
			this.headingTabPage.Text = "Title";
			// 
			// groupBox11
			// 
			this.groupBox11.BackColor = System.Drawing.Color.Transparent;
			this.groupBox11.Controls.Add(this.displayNameTextBox);
			this.groupBox11.Location = new System.Drawing.Point(8, 568);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(280, 56);
			this.groupBox11.TabIndex = 5;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "Dislpay Name";
			// 
			// displayNameTextBox
			// 
			this.displayNameTextBox.Location = new System.Drawing.Point(8, 24);
			this.displayNameTextBox.Name = "displayNameTextBox";
			this.displayNameTextBox.Size = new System.Drawing.Size(256, 20);
			this.displayNameTextBox.TabIndex = 0;
			this.displayNameTextBox.Text = "";
			this.displayNameTextBox.TextChanged += new System.EventHandler(this.displayNameTextBox_TextChanged);
			// 
			// groupBox9
			// 
			this.groupBox9.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox9.BackgroundImage")));
			this.groupBox9.Controls.Add(this.titleSpaceAfterTrackBar);
			this.groupBox9.Controls.Add(this.titleSpaceAfterTextBox);
			this.groupBox9.Controls.Add(this.titleDateStyleComboBox);
			this.groupBox9.Controls.Add(this.label29);
			this.groupBox9.Controls.Add(this.label28);
			this.groupBox9.Controls.Add(this.titleTextTextBox);
			this.groupBox9.Controls.Add(this.label24);
			this.groupBox9.Location = new System.Drawing.Point(8, 400);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(280, 160);
			this.groupBox9.TabIndex = 4;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Title";
			// 
			// titleSpaceAfterTrackBar
			// 
			this.titleSpaceAfterTrackBar.BackColor = System.Drawing.Color.LavenderBlush;
			this.titleSpaceAfterTrackBar.Location = new System.Drawing.Point(8, 112);
			this.titleSpaceAfterTrackBar.Maximum = 100;
			this.titleSpaceAfterTrackBar.Name = "titleSpaceAfterTrackBar";
			this.titleSpaceAfterTrackBar.Size = new System.Drawing.Size(264, 45);
			this.titleSpaceAfterTrackBar.TabIndex = 10;
			this.titleSpaceAfterTrackBar.TickFrequency = 10;
			this.titleSpaceAfterTrackBar.Scroll += new System.EventHandler(this.titleSpaceAfterTrackBar_Scroll);
			// 
			// titleSpaceAfterTextBox
			// 
			this.titleSpaceAfterTextBox.Location = new System.Drawing.Point(152, 80);
			this.titleSpaceAfterTextBox.Name = "titleSpaceAfterTextBox";
			this.titleSpaceAfterTextBox.Size = new System.Drawing.Size(80, 20);
			this.titleSpaceAfterTextBox.TabIndex = 9;
			this.titleSpaceAfterTextBox.Text = "";
			this.titleSpaceAfterTextBox.TextChanged += new System.EventHandler(this.titleSpaceAfterTextBox_TextChanged);
			// 
			// titleDateStyleComboBox
			// 
			this.titleDateStyleComboBox.Location = new System.Drawing.Point(88, 16);
			this.titleDateStyleComboBox.Name = "titleDateStyleComboBox";
			this.titleDateStyleComboBox.Size = new System.Drawing.Size(176, 21);
			this.titleDateStyleComboBox.TabIndex = 6;
			this.titleDateStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.titleDateStyleComboBox_SelectedIndexChanged);
			// 
			// label29
			// 
			this.label29.BackColor = System.Drawing.Color.Transparent;
			this.label29.Location = new System.Drawing.Point(8, 88);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(144, 23);
			this.label29.TabIndex = 3;
			this.label29.Text = "Space After Title Baseline:";
			// 
			// label28
			// 
			this.label28.BackColor = System.Drawing.Color.Transparent;
			this.label28.Location = new System.Drawing.Point(8, 24);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(64, 23);
			this.label28.TabIndex = 2;
			this.label28.Text = "Date Style:";
			// 
			// titleTextTextBox
			// 
			this.titleTextTextBox.Location = new System.Drawing.Point(48, 48);
			this.titleTextTextBox.Name = "titleTextTextBox";
			this.titleTextTextBox.Size = new System.Drawing.Size(216, 20);
			this.titleTextTextBox.TabIndex = 1;
			this.titleTextTextBox.Text = "";
			this.titleTextTextBox.TextChanged += new System.EventHandler(this.titleTextTextBox_TextChanged);
			// 
			// label24
			// 
			this.label24.BackColor = System.Drawing.Color.Transparent;
			this.label24.Location = new System.Drawing.Point(8, 56);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(32, 23);
			this.label24.TabIndex = 0;
			this.label24.Text = "Text:";
			// 
			// groupBox6
			// 
			this.groupBox6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox6.BackgroundImage")));
			this.groupBox6.Controls.Add(this.titleWidthTrackBar);
			this.groupBox6.Controls.Add(this.titleHeightTrackBar);
			this.groupBox6.Controls.Add(this.yTitleCornerTrackBar);
			this.groupBox6.Controls.Add(this.xTitleCornerTrackBar);
			this.groupBox6.Controls.Add(this.label32);
			this.groupBox6.Controls.Add(this.label9);
			this.groupBox6.Controls.Add(this.titleBackgroundColorChooserButton);
			this.groupBox6.Controls.Add(this.titleForegroundColorChooserButton);
			this.groupBox6.Controls.Add(this.titleWidthTextBox);
			this.groupBox6.Controls.Add(this.titleHeightTextBox);
			this.groupBox6.Controls.Add(this.yTitleCornerTextBox);
			this.groupBox6.Controls.Add(this.xTitleCornerTextBox);
			this.groupBox6.Controls.Add(this.label21);
			this.groupBox6.Controls.Add(this.label20);
			this.groupBox6.Controls.Add(this.titleRectangleStyleComboBox);
			this.groupBox6.Controls.Add(this.label19);
			this.groupBox6.Controls.Add(this.label18);
			this.groupBox6.Controls.Add(this.titleBackgroundColorComboBox);
			this.groupBox6.Controls.Add(this.titleForegroundColorComboBox);
			this.groupBox6.Controls.Add(this.label31);
			this.groupBox6.Controls.Add(this.label30);
			this.groupBox6.Location = new System.Drawing.Point(8, 16);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(280, 376);
			this.groupBox6.TabIndex = 2;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Rectangle";
			// 
			// titleWidthTrackBar
			// 
			this.titleWidthTrackBar.BackColor = System.Drawing.Color.LavenderBlush;
			this.titleWidthTrackBar.Location = new System.Drawing.Point(8, 328);
			this.titleWidthTrackBar.Maximum = 100;
			this.titleWidthTrackBar.Name = "titleWidthTrackBar";
			this.titleWidthTrackBar.Size = new System.Drawing.Size(264, 45);
			this.titleWidthTrackBar.TabIndex = 16;
			this.titleWidthTrackBar.TickFrequency = 10;
			this.titleWidthTrackBar.Scroll += new System.EventHandler(this.titleWidthTrackBar_Scroll);
			// 
			// titleHeightTrackBar
			// 
			this.titleHeightTrackBar.BackColor = System.Drawing.Color.LavenderBlush;
			this.titleHeightTrackBar.Location = new System.Drawing.Point(8, 248);
			this.titleHeightTrackBar.Maximum = 100;
			this.titleHeightTrackBar.Name = "titleHeightTrackBar";
			this.titleHeightTrackBar.Size = new System.Drawing.Size(264, 45);
			this.titleHeightTrackBar.TabIndex = 15;
			this.titleHeightTrackBar.TickFrequency = 10;
			this.titleHeightTrackBar.Scroll += new System.EventHandler(this.titleHeightTrackBar_Scroll);
			// 
			// yTitleCornerTrackBar
			// 
			this.yTitleCornerTrackBar.BackColor = System.Drawing.Color.LavenderBlush;
			this.yTitleCornerTrackBar.Location = new System.Drawing.Point(144, 168);
			this.yTitleCornerTrackBar.Maximum = 100;
			this.yTitleCornerTrackBar.Name = "yTitleCornerTrackBar";
			this.yTitleCornerTrackBar.Size = new System.Drawing.Size(120, 45);
			this.yTitleCornerTrackBar.TabIndex = 14;
			this.yTitleCornerTrackBar.TickFrequency = 10;
			this.yTitleCornerTrackBar.Scroll += new System.EventHandler(this.yTitleCornerTrackBar_Scroll);
			// 
			// xTitleCornerTrackBar
			// 
			this.xTitleCornerTrackBar.BackColor = System.Drawing.Color.LavenderBlush;
			this.xTitleCornerTrackBar.Location = new System.Drawing.Point(8, 168);
			this.xTitleCornerTrackBar.Maximum = 100;
			this.xTitleCornerTrackBar.Name = "xTitleCornerTrackBar";
			this.xTitleCornerTrackBar.Size = new System.Drawing.Size(120, 45);
			this.xTitleCornerTrackBar.TabIndex = 13;
			this.xTitleCornerTrackBar.TickFrequency = 10;
			this.xTitleCornerTrackBar.Scroll += new System.EventHandler(this.xTitleCornerTrackBar_Scroll);
			// 
			// label32
			// 
			this.label32.BackColor = System.Drawing.Color.Transparent;
			this.label32.Location = new System.Drawing.Point(152, 152);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(16, 23);
			this.label32.TabIndex = 12;
			this.label32.Text = "Y:";
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Location = new System.Drawing.Point(16, 152);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(16, 23);
			this.label9.TabIndex = 11;
			this.label9.Text = "X:";
			// 
			// titleBackgroundColorChooserButton
			// 
			this.titleBackgroundColorChooserButton.Location = new System.Drawing.Point(240, 80);
			this.titleBackgroundColorChooserButton.Name = "titleBackgroundColorChooserButton";
			this.titleBackgroundColorChooserButton.Size = new System.Drawing.Size(24, 23);
			this.titleBackgroundColorChooserButton.TabIndex = 10;
			this.titleBackgroundColorChooserButton.Text = "...";
			this.titleBackgroundColorChooserButton.Click += new System.EventHandler(this.titleBackgroundColorChooserButton_Click);
			// 
			// titleForegroundColorChooserButton
			// 
			this.titleForegroundColorChooserButton.Location = new System.Drawing.Point(240, 48);
			this.titleForegroundColorChooserButton.Name = "titleForegroundColorChooserButton";
			this.titleForegroundColorChooserButton.Size = new System.Drawing.Size(24, 23);
			this.titleForegroundColorChooserButton.TabIndex = 9;
			this.titleForegroundColorChooserButton.Text = "...";
			this.titleForegroundColorChooserButton.Click += new System.EventHandler(this.titleForegroundColorChooserButton_Click);
			// 
			// titleWidthTextBox
			// 
			this.titleWidthTextBox.Location = new System.Drawing.Point(72, 304);
			this.titleWidthTextBox.Name = "titleWidthTextBox";
			this.titleWidthTextBox.Size = new System.Drawing.Size(88, 20);
			this.titleWidthTextBox.TabIndex = 7;
			this.titleWidthTextBox.Text = "";
			this.titleWidthTextBox.TextChanged += new System.EventHandler(this.titleWidthTextBox_TextChanged);
			// 
			// titleHeightTextBox
			// 
			this.titleHeightTextBox.Location = new System.Drawing.Point(72, 224);
			this.titleHeightTextBox.Name = "titleHeightTextBox";
			this.titleHeightTextBox.Size = new System.Drawing.Size(88, 20);
			this.titleHeightTextBox.TabIndex = 6;
			this.titleHeightTextBox.Text = "";
			this.titleHeightTextBox.TextChanged += new System.EventHandler(this.titleHeightTextBox_TextChanged);
			// 
			// yTitleCornerTextBox
			// 
			this.yTitleCornerTextBox.Location = new System.Drawing.Point(176, 144);
			this.yTitleCornerTextBox.Name = "yTitleCornerTextBox";
			this.yTitleCornerTextBox.Size = new System.Drawing.Size(80, 20);
			this.yTitleCornerTextBox.TabIndex = 5;
			this.yTitleCornerTextBox.Text = "";
			this.yTitleCornerTextBox.TextChanged += new System.EventHandler(this.yTitleCornerTextBox_TextChanged);
			// 
			// xTitleCornerTextBox
			// 
			this.xTitleCornerTextBox.Location = new System.Drawing.Point(40, 144);
			this.xTitleCornerTextBox.Name = "xTitleCornerTextBox";
			this.xTitleCornerTextBox.Size = new System.Drawing.Size(80, 20);
			this.xTitleCornerTextBox.TabIndex = 4;
			this.xTitleCornerTextBox.Text = "";
			this.xTitleCornerTextBox.TextChanged += new System.EventHandler(this.xTitleCornerTextBox_TextChanged);
			// 
			// label21
			// 
			this.label21.BackColor = System.Drawing.Color.Transparent;
			this.label21.Location = new System.Drawing.Point(8, 304);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(56, 23);
			this.label21.TabIndex = 3;
			this.label21.Text = "Width:";
			// 
			// label20
			// 
			this.label20.BackColor = System.Drawing.Color.Transparent;
			this.label20.Location = new System.Drawing.Point(8, 224);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(56, 23);
			this.label20.TabIndex = 2;
			this.label20.Text = "Height:";
			// 
			// titleRectangleStyleComboBox
			// 
			this.titleRectangleStyleComboBox.Location = new System.Drawing.Point(64, 16);
			this.titleRectangleStyleComboBox.Name = "titleRectangleStyleComboBox";
			this.titleRectangleStyleComboBox.Size = new System.Drawing.Size(200, 21);
			this.titleRectangleStyleComboBox.TabIndex = 0;
			this.titleRectangleStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.titleRectangleStyleComboBox_SelectedIndexChanged);
			// 
			// label19
			// 
			this.label19.BackColor = System.Drawing.Color.Transparent;
			this.label19.Location = new System.Drawing.Point(8, 24);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(48, 23);
			this.label19.TabIndex = 1;
			this.label19.Text = "Style:";
			// 
			// label18
			// 
			this.label18.BackColor = System.Drawing.Color.Transparent;
			this.label18.Location = new System.Drawing.Point(8, 120);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(96, 23);
			this.label18.TabIndex = 0;
			this.label18.Text = "Upper left corner:";
			// 
			// titleBackgroundColorComboBox
			// 
			this.titleBackgroundColorComboBox.Location = new System.Drawing.Point(112, 80);
			this.titleBackgroundColorComboBox.Name = "titleBackgroundColorComboBox";
			this.titleBackgroundColorComboBox.Size = new System.Drawing.Size(120, 21);
			this.titleBackgroundColorComboBox.TabIndex = 8;
			this.titleBackgroundColorComboBox.TextChanged += new System.EventHandler(this.titleBackgroundColorComboBox_TextChanged);
			// 
			// titleForegroundColorComboBox
			// 
			this.titleForegroundColorComboBox.Location = new System.Drawing.Point(112, 48);
			this.titleForegroundColorComboBox.Name = "titleForegroundColorComboBox";
			this.titleForegroundColorComboBox.Size = new System.Drawing.Size(120, 21);
			this.titleForegroundColorComboBox.TabIndex = 7;
			this.titleForegroundColorComboBox.TextChanged += new System.EventHandler(this.titleForegroundColorComboBox_TextChanged);
			// 
			// label31
			// 
			this.label31.BackColor = System.Drawing.Color.Transparent;
			this.label31.Location = new System.Drawing.Point(8, 88);
			this.label31.Name = "label31";
			this.label31.TabIndex = 5;
			this.label31.Text = "Background Color:";
			// 
			// label30
			// 
			this.label30.BackColor = System.Drawing.Color.Transparent;
			this.label30.Location = new System.Drawing.Point(8, 56);
			this.label30.Name = "label30";
			this.label30.TabIndex = 4;
			this.label30.Text = "Foreground Color:";
			// 
			// generalTabPage
			// 
			this.generalTabPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("generalTabPage.BackgroundImage")));
			this.generalTabPage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.generalTabPage.Controls.Add(this.groupBox10);
			this.generalTabPage.Controls.Add(this.groupBox8);
			this.generalTabPage.Controls.Add(this.groupBox7);
			this.generalTabPage.Location = new System.Drawing.Point(4, 22);
			this.generalTabPage.Name = "generalTabPage";
			this.generalTabPage.Size = new System.Drawing.Size(304, 677);
			this.generalTabPage.TabIndex = 2;
			this.generalTabPage.Text = "Background";
			// 
			// groupBox10
			// 
			this.groupBox10.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox10.BackgroundImage")));
			this.groupBox10.Controls.Add(this.sizeWidthOnlyTextBox);
			this.groupBox10.Controls.Add(this.sizeHeightOnlyTextBox);
			this.groupBox10.Controls.Add(this.label36);
			this.groupBox10.Controls.Add(this.label35);
			this.groupBox10.Location = new System.Drawing.Point(8, 368);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(280, 72);
			this.groupBox10.TabIndex = 3;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "Minimum Page Size";
			// 
			// sizeWidthOnlyTextBox
			// 
			this.sizeWidthOnlyTextBox.Location = new System.Drawing.Point(96, 16);
			this.sizeWidthOnlyTextBox.Name = "sizeWidthOnlyTextBox";
			this.sizeWidthOnlyTextBox.Size = new System.Drawing.Size(96, 20);
			this.sizeWidthOnlyTextBox.TabIndex = 6;
			this.sizeWidthOnlyTextBox.Text = "";
			this.sizeWidthOnlyTextBox.TextChanged += new System.EventHandler(this.sizeWidthOnlyTextBox_TextChanged);
			// 
			// sizeHeightOnlyTextBox
			// 
			this.sizeHeightOnlyTextBox.Location = new System.Drawing.Point(96, 40);
			this.sizeHeightOnlyTextBox.Name = "sizeHeightOnlyTextBox";
			this.sizeHeightOnlyTextBox.Size = new System.Drawing.Size(96, 20);
			this.sizeHeightOnlyTextBox.TabIndex = 5;
			this.sizeHeightOnlyTextBox.Text = "";
			this.sizeHeightOnlyTextBox.TextChanged += new System.EventHandler(this.sizeHeightOnlyTextBox_TextChanged);
			// 
			// label36
			// 
			this.label36.BackColor = System.Drawing.Color.Transparent;
			this.label36.Location = new System.Drawing.Point(8, 24);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(72, 23);
			this.label36.TabIndex = 1;
			this.label36.Text = "Page Width:";
			// 
			// label35
			// 
			this.label35.BackColor = System.Drawing.Color.Transparent;
			this.label35.Location = new System.Drawing.Point(8, 48);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(72, 23);
			this.label35.TabIndex = 0;
			this.label35.Text = "Page Height:";
			// 
			// groupBox8
			// 
			this.groupBox8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox8.BackgroundImage")));
			this.groupBox8.Controls.Add(this.label34);
			this.groupBox8.Controls.Add(this.alignImageWithMinSizeCheckBox);
			this.groupBox8.Controls.Add(this.imageHeightTextBox);
			this.groupBox8.Controls.Add(this.imageWidthTextBox);
			this.groupBox8.Controls.Add(this.label33);
			this.groupBox8.Controls.Add(this.backgroundImageComboBox);
			this.groupBox8.Controls.Add(this.backgroundImageTransparencyLabel);
			this.groupBox8.Controls.Add(this.backgroundImageTransparencyTrackBar);
			this.groupBox8.Controls.Add(this.label25);
			this.groupBox8.Controls.Add(this.backgroundImageButton);
			this.groupBox8.Controls.Add(this.backgroundImageStyleComboBox);
			this.groupBox8.Controls.Add(this.label27);
			this.groupBox8.Controls.Add(this.label26);
			this.groupBox8.Location = new System.Drawing.Point(8, 120);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(280, 240);
			this.groupBox8.TabIndex = 2;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Image";
			// 
			// label34
			// 
			this.label34.BackColor = System.Drawing.Color.Transparent;
			this.label34.Location = new System.Drawing.Point(16, 112);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(40, 16);
			this.label34.TabIndex = 18;
			this.label34.Text = "Width:";
			// 
			// alignImageWithMinSizeCheckBox
			// 
			this.alignImageWithMinSizeCheckBox.BackColor = System.Drawing.Color.Transparent;
			this.alignImageWithMinSizeCheckBox.Location = new System.Drawing.Point(8, 136);
			this.alignImageWithMinSizeCheckBox.Name = "alignImageWithMinSizeCheckBox";
			this.alignImageWithMinSizeCheckBox.Size = new System.Drawing.Size(216, 24);
			this.alignImageWithMinSizeCheckBox.TabIndex = 17;
			this.alignImageWithMinSizeCheckBox.Text = "Align image with minimum page size";
			this.alignImageWithMinSizeCheckBox.CheckedChanged += new System.EventHandler(this.alignImageWithMinSizeCheckBox_CheckedChanged);
			// 
			// imageHeightTextBox
			// 
			this.imageHeightTextBox.Location = new System.Drawing.Point(192, 104);
			this.imageHeightTextBox.Name = "imageHeightTextBox";
			this.imageHeightTextBox.Size = new System.Drawing.Size(64, 20);
			this.imageHeightTextBox.TabIndex = 16;
			this.imageHeightTextBox.Text = "";
			this.imageHeightTextBox.TextChanged += new System.EventHandler(this.imageHeightTextBox_TextChanged);
			// 
			// imageWidthTextBox
			// 
			this.imageWidthTextBox.Location = new System.Drawing.Point(64, 104);
			this.imageWidthTextBox.Name = "imageWidthTextBox";
			this.imageWidthTextBox.Size = new System.Drawing.Size(64, 20);
			this.imageWidthTextBox.TabIndex = 15;
			this.imageWidthTextBox.Text = "";
			this.imageWidthTextBox.TextChanged += new System.EventHandler(this.imageWidthTextBox_TextChanged);
			// 
			// label33
			// 
			this.label33.BackColor = System.Drawing.Color.Transparent;
			this.label33.Location = new System.Drawing.Point(144, 112);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(40, 16);
			this.label33.TabIndex = 14;
			this.label33.Text = "Height:";
			// 
			// backgroundImageComboBox
			// 
			this.backgroundImageComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.backgroundImageComboBox.ImageList = null;
			this.backgroundImageComboBox.Location = new System.Drawing.Point(16, 56);
			this.backgroundImageComboBox.Name = "backgroundImageComboBox";
			this.backgroundImageComboBox.Size = new System.Drawing.Size(208, 21);
			this.backgroundImageComboBox.TabIndex = 13;
			this.backgroundImageComboBox.SelectedIndexChanged += new System.EventHandler(this.backgroundImageComboBox_SelectedIndexChanged);
			// 
			// backgroundImageTransparencyLabel
			// 
			this.backgroundImageTransparencyLabel.BackColor = System.Drawing.Color.Transparent;
			this.backgroundImageTransparencyLabel.Location = new System.Drawing.Point(128, 168);
			this.backgroundImageTransparencyLabel.Name = "backgroundImageTransparencyLabel";
			this.backgroundImageTransparencyLabel.Size = new System.Drawing.Size(112, 23);
			this.backgroundImageTransparencyLabel.TabIndex = 12;
			// 
			// backgroundImageTransparencyTrackBar
			// 
			this.backgroundImageTransparencyTrackBar.BackColor = System.Drawing.Color.WhiteSmoke;
			this.backgroundImageTransparencyTrackBar.Location = new System.Drawing.Point(8, 192);
			this.backgroundImageTransparencyTrackBar.Maximum = 20;
			this.backgroundImageTransparencyTrackBar.Name = "backgroundImageTransparencyTrackBar";
			this.backgroundImageTransparencyTrackBar.Size = new System.Drawing.Size(256, 45);
			this.backgroundImageTransparencyTrackBar.TabIndex = 11;
			this.backgroundImageTransparencyTrackBar.Scroll += new System.EventHandler(this.backgroundImageTransparencyTrackBar_Scroll);
			// 
			// label25
			// 
			this.label25.BackColor = System.Drawing.Color.Transparent;
			this.label25.Location = new System.Drawing.Point(8, 88);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(88, 23);
			this.label25.TabIndex = 6;
			this.label25.Text = "Image size";
			// 
			// backgroundImageButton
			// 
			this.backgroundImageButton.Location = new System.Drawing.Point(232, 56);
			this.backgroundImageButton.Name = "backgroundImageButton";
			this.backgroundImageButton.Size = new System.Drawing.Size(24, 23);
			this.backgroundImageButton.TabIndex = 9;
			this.backgroundImageButton.Text = "...";
			this.backgroundImageButton.Click += new System.EventHandler(this.backgroundImageButton_Click);
			// 
			// backgroundImageStyleComboBox
			// 
			this.backgroundImageStyleComboBox.Location = new System.Drawing.Point(80, 24);
			this.backgroundImageStyleComboBox.Name = "backgroundImageStyleComboBox";
			this.backgroundImageStyleComboBox.Size = new System.Drawing.Size(176, 21);
			this.backgroundImageStyleComboBox.TabIndex = 10;
			this.backgroundImageStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.backgroundImageStyleComboBox_SelectedIndexChanged);
			// 
			// label27
			// 
			this.label27.BackColor = System.Drawing.Color.Transparent;
			this.label27.Location = new System.Drawing.Point(8, 168);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(112, 23);
			this.label27.TabIndex = 8;
			this.label27.Text = "Image Transparency:";
			// 
			// label26
			// 
			this.label26.BackColor = System.Drawing.Color.Transparent;
			this.label26.Location = new System.Drawing.Point(8, 32);
			this.label26.Name = "label26";
			this.label26.TabIndex = 7;
			this.label26.Text = "Image Style:";
			// 
			// groupBox7
			// 
			this.groupBox7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox7.BackgroundImage")));
			this.groupBox7.Controls.Add(this.backgroundColorChooserButton);
			this.groupBox7.Controls.Add(this.backgroundColorStyleComboBox);
			this.groupBox7.Controls.Add(this.backgroundColorComboBox);
			this.groupBox7.Controls.Add(this.label23);
			this.groupBox7.Controls.Add(this.label22);
			this.groupBox7.Location = new System.Drawing.Point(8, 16);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(280, 88);
			this.groupBox7.TabIndex = 1;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Color";
			// 
			// backgroundColorChooserButton
			// 
			this.backgroundColorChooserButton.Location = new System.Drawing.Point(232, 48);
			this.backgroundColorChooserButton.Name = "backgroundColorChooserButton";
			this.backgroundColorChooserButton.Size = new System.Drawing.Size(24, 23);
			this.backgroundColorChooserButton.TabIndex = 5;
			this.backgroundColorChooserButton.Text = "...";
			this.backgroundColorChooserButton.Click += new System.EventHandler(this.backgroundColorChooserButton_Click);
			// 
			// backgroundColorStyleComboBox
			// 
			this.backgroundColorStyleComboBox.Location = new System.Drawing.Point(80, 16);
			this.backgroundColorStyleComboBox.Name = "backgroundColorStyleComboBox";
			this.backgroundColorStyleComboBox.Size = new System.Drawing.Size(176, 21);
			this.backgroundColorStyleComboBox.TabIndex = 4;
			this.backgroundColorStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.backgroundColorStyleComboBox_SelectedIndexChanged);
			// 
			// backgroundColorComboBox
			// 
			this.backgroundColorComboBox.Location = new System.Drawing.Point(80, 48);
			this.backgroundColorComboBox.Name = "backgroundColorComboBox";
			this.backgroundColorComboBox.Size = new System.Drawing.Size(144, 21);
			this.backgroundColorComboBox.TabIndex = 3;
			this.backgroundColorComboBox.SelectedIndexChanged += new System.EventHandler(this.backgroundColorComboBox_SelectedIndexChanged);
			// 
			// label23
			// 
			this.label23.BackColor = System.Drawing.Color.Transparent;
			this.label23.Location = new System.Drawing.Point(8, 24);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(64, 23);
			this.label23.TabIndex = 2;
			this.label23.Text = "Style:";
			// 
			// label22
			// 
			this.label22.BackColor = System.Drawing.Color.Transparent;
			this.label22.Location = new System.Drawing.Point(8, 56);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(56, 16);
			this.label22.TabIndex = 0;
			this.label22.Text = "Color:";
			// 
			// groupBox4
			// 
			this.groupBox4.Location = new System.Drawing.Point(0, 0);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabIndex = 0;
			this.groupBox4.TabStop = false;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 72);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(96, 24);
			this.label13.TabIndex = 3;
			this.label13.Text = "Spacing:";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 48);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(40, 24);
			this.label14.TabIndex = 2;
			this.label14.Text = "Style:";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.scribble);
			this.panel1.Location = new System.Drawing.Point(22, 22);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(506, 678);
			this.panel1.TabIndex = 1;
			// 
			// scribble
			// 
			this.scribble.AllowDrop = true;
			this.scribble.BackColor = System.Drawing.Color.White;
			this.scribble.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scribble.Location = new System.Drawing.Point(0, 0);
			this.scribble.Name = "scribble";
			this.scribble.Size = new System.Drawing.Size(506, 678);
			this.scribble.TabIndex = 0;
			this.scribble.TabStop = false;
			this.scribble.Text = "scribble";
			this.scribble.Paint += new System.Windows.Forms.PaintEventHandler(this.scribble_Paint);
			// 
			// backgroundImageList
			// 
			this.backgroundImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.backgroundImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("backgroundImageList.ImageStream")));
			this.backgroundImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// openBackgroundImageFileDialog
			// 
			this.openBackgroundImageFileDialog.Filter = "Images|*.jpg;*.gif;*.png;*.bmp|All files (*.*)|*.*";
			this.openBackgroundImageFileDialog.Title = "Open Background Image";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem2,
																					  this.menuItem15,
																					  this.zoomMenuItem});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.newMenuItem,
																					  this.openMenuItem,
																					  this.saveAsMenuItem,
																					  this.saveMenuItem,
																					  this.menuItem5,
																					  this.exitMenuItem});
			this.menuItem1.Text = "&File";
			// 
			// newMenuItem
			// 
			this.newMenuItem.Index = 0;
			this.newMenuItem.Text = "&New";
			this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
			// 
			// openMenuItem
			// 
			this.openMenuItem.Index = 1;
			this.openMenuItem.Text = "&Open...";
			this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
			// 
			// saveAsMenuItem
			// 
			this.saveAsMenuItem.Index = 2;
			this.saveAsMenuItem.Text = "Save &As...";
			this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
			// 
			// saveMenuItem
			// 
			this.saveMenuItem.Index = 3;
			this.saveMenuItem.Text = "&Save";
			this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 4;
			this.menuItem5.Text = "-";
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Index = 5;
			this.exitMenuItem.Text = "E&xit";
			this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.paperBlankMenuItem,
																					  this.paperNarrowMenuItem,
																					  this.paperCollegeMenuItem,
																					  this.paperStandardMenuItem,
																					  this.paperWideMenuItem,
																					  this.paperSmallGridMenuItem,
																					  this.paperGridMenuItem});
			this.menuItem2.Text = "&Stock Stationery";
			// 
			// paperBlankMenuItem
			// 
			this.paperBlankMenuItem.Index = 0;
			this.paperBlankMenuItem.Text = "&Blank";
			this.paperBlankMenuItem.Click += new System.EventHandler(this.paperBlankMenuItem_Click);
			// 
			// paperNarrowMenuItem
			// 
			this.paperNarrowMenuItem.Index = 1;
			this.paperNarrowMenuItem.Text = "&Narrow";
			this.paperNarrowMenuItem.Click += new System.EventHandler(this.paperNarrowMenuItem_Click);
			// 
			// paperCollegeMenuItem
			// 
			this.paperCollegeMenuItem.Index = 2;
			this.paperCollegeMenuItem.Text = "&College Ruled";
			this.paperCollegeMenuItem.Click += new System.EventHandler(this.paperCollegeMenuItem_Click);
			// 
			// paperStandardMenuItem
			// 
			this.paperStandardMenuItem.Index = 3;
			this.paperStandardMenuItem.Text = "&Standard";
			this.paperStandardMenuItem.Click += new System.EventHandler(this.paperStandardMenuItem_Click);
			// 
			// paperWideMenuItem
			// 
			this.paperWideMenuItem.Index = 4;
			this.paperWideMenuItem.Text = "&Wide";
			this.paperWideMenuItem.Click += new System.EventHandler(this.paperWideMenuItem_Click);
			// 
			// paperSmallGridMenuItem
			// 
			this.paperSmallGridMenuItem.Index = 5;
			this.paperSmallGridMenuItem.Text = "S&mall Grid";
			this.paperSmallGridMenuItem.Click += new System.EventHandler(this.paperSmallGridMenuItem_Click);
			// 
			// paperGridMenuItem
			// 
			this.paperGridMenuItem.Index = 6;
			this.paperGridMenuItem.Text = "&Grid";
			this.paperGridMenuItem.Click += new System.EventHandler(this.paperGridMenuItem_Click);
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 2;
			this.menuItem15.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.inchesMenuItem,
																					   this.centimetersMenuItem,
																					   this.himetricsMenuItem});
			this.menuItem15.Text = "&Units";
			// 
			// inchesMenuItem
			// 
			this.inchesMenuItem.Index = 0;
			this.inchesMenuItem.Text = "&Inches";
			this.inchesMenuItem.Click += new System.EventHandler(this.inchesMenuItem_Click);
			// 
			// centimetersMenuItem
			// 
			this.centimetersMenuItem.Index = 1;
			this.centimetersMenuItem.Text = "&Centimeters";
			this.centimetersMenuItem.Click += new System.EventHandler(this.centimetersMenuItem_Click);
			// 
			// himetricsMenuItem
			// 
			this.himetricsMenuItem.Index = 2;
			this.himetricsMenuItem.Text = "&Himetrics";
			this.himetricsMenuItem.Click += new System.EventHandler(this.himetricsMenuItem_Click);
			// 
			// zoomMenuItem
			// 
			this.zoomMenuItem.Index = 3;
			this.zoomMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.zoom25MenuItem,
																						 this.zoom50MenuItem,
																						 this.zoom100MenuItem,
																						 this.zoom150MenuItem,
																						 this.zoom200MenuItem});
			this.zoomMenuItem.Text = "&Zoom";
			// 
			// zoom25MenuItem
			// 
			this.zoom25MenuItem.Index = 0;
			this.zoom25MenuItem.Text = "&50%";
			this.zoom25MenuItem.Click += new System.EventHandler(this.zoom25MenuItem_Click);
			// 
			// zoom50MenuItem
			// 
			this.zoom50MenuItem.Index = 1;
			this.zoom50MenuItem.Text = "&80%";
			this.zoom50MenuItem.Click += new System.EventHandler(this.zoom50MenuItem_Click);
			// 
			// zoom100MenuItem
			// 
			this.zoom100MenuItem.Index = 2;
			this.zoom100MenuItem.Text = "&100%";
			this.zoom100MenuItem.Click += new System.EventHandler(this.zoom100MenuItem_Click);
			// 
			// zoom150MenuItem
			// 
			this.zoom150MenuItem.Index = 3;
			this.zoom150MenuItem.Text = "15&0%";
			this.zoom150MenuItem.Click += new System.EventHandler(this.zoom150MenuItem_Click);
			// 
			// zoom200MenuItem
			// 
			this.zoom200MenuItem.Index = 4;
			this.zoom200MenuItem.Text = "&200%";
			this.zoom200MenuItem.Click += new System.EventHandler(this.zoom200MenuItem_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "xml";
			this.openFileDialog.Filter = "Stationery Files (*.xml) |*.xml|All Files (*.*)|*.*";
			this.openFileDialog.Title = "Open Stationery";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "xml";
			this.saveFileDialog.Filter = "Stationery Files (*.xml)|*.xml|All Files (*.*)|*.*";
			this.saveFileDialog.Title = "Save Stationery";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Silver;
			this.ClientSize = new System.Drawing.Size(842, 703);
			this.Controls.Add(this.hRuler);
			this.Controls.Add(this.vRuler);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NuGenStationary Builder";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabControl.ResumeLayout(false);
			this.linesTabPage.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.vLineSpaceBeforeTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.vLineSpaceBetweenTrackBar)).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.hLineSpaceBeforeTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.hLineSpaceBetweenTrackBar)).EndInit();
			this.tabPage1.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.lMarginLineSpacingTrackBar)).EndInit();
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.rMarginLineSpacingTrackBar)).EndInit();
			this.headingTabPage.ResumeLayout(false);
			this.groupBox11.ResumeLayout(false);
			this.groupBox9.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.titleSpaceAfterTrackBar)).EndInit();
			this.groupBox6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.titleWidthTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.titleHeightTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.yTitleCornerTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.xTitleCornerTrackBar)).EndInit();
			this.generalTabPage.ResumeLayout(false);
			this.groupBox10.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.backgroundImageTransparencyTrackBar)).EndInit();
			this.groupBox7.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.DoEvents();
			Application.Run(new Form1());
		}


		#region Private Methods
		private void Repaint()
		{
			scribble.Invalidate();
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			scribble.Stationery = new Stationery();
			
			InitalizeGUIElements();

			LoadValues();

			zoom100MenuItem.Checked = true;
			centimetersMenuItem.Checked = true;
		}

		private void LoadValues()
		{
			HorizontalLineSetup();
			VerticalLineSetup();
			RightMarginLineSetup();
			LeftMarginLineSetup();
			TitleRectangleSetup();
			BackgroundSetup();
			TitleSetup();
			PageSizeSetup();
		}
		private void LoadStationerySettings()
		{
			// Horizontal Line
			fSettings.hLineColor = scribble.Stationery.HorizontalLineColor;
			fSettings.hLineStyle = scribble.Stationery.HorizontalLineStyle;
			fSettings.hLineSpaceBetween = scribble.Stationery.HorizontalLineSpaceBetween;
			fSettings.hLineSpaceBefore = scribble.Stationery.HorizontalLineSpaceBefore;

			// Vertical Line
			fSettings.vLineColor = scribble.Stationery.VerticalLineColor;
			fSettings.vLineStyle = scribble.Stationery.VerticalLineStyle;
			fSettings.vLineSpaceBetween = scribble.Stationery.VerticalLineSpaceBetween;
			fSettings.vLineSpaceBefore = scribble.Stationery.VerticalLineSpaceBefore;

			// Right Margin
			fSettings.rMarginColor = scribble.Stationery.RightMarginLineColor;
			fSettings.rMarginStyle = scribble.Stationery.RightMarginLineStyle;
			fSettings.rMarginSpacing = scribble.Stationery.RightMarginLineSpacing;

			// Left Margin
			fSettings.lMarginColor = scribble.Stationery.LeftMarginLineColor;
			fSettings.lMarginStyle = scribble.Stationery.LeftMarginLineStyle;
			fSettings.lMarginSpacing = scribble.Stationery.LeftMarginLineSpacing;

			// Title
			fSettings.titleDateStyle = scribble.Stationery.TitleDateStyle;
			fSettings.titleString = scribble.Stationery.TitleText;
			fSettings.titleSpaceAfter = scribble.Stationery.TitleBaselineSpaceAfter;
			fSettings.displayName = scribble.Stationery.DisplayName;

			// Title Rectangle
			fSettings.titleRectangleStyle = scribble.Stationery.TitleRectangleStyle;
			fSettings.titleForegroundColor = scribble.Stationery.TitleForegroundColor;
			fSettings.titleBackgroundColor = scribble.Stationery.TitleBackgroundColor;
			fSettings.titleRectangle = scribble.Stationery.TitleRectangle;

			// Background 
			fSettings.backgroundStyle = scribble.Stationery.BackgroundColorStyle;
			fSettings.backgroundColor = scribble.Stationery.BackgroundColor;
			fSettings.backgroundImageStyle = scribble.Stationery.BackgroundImageStyle;
			fSettings.backgroundImage = scribble.Stationery.BackgroundImage;
			if (fSettings.backgroundImageData != null)
			{
				fSettings.backgroundImageData = scribble.Stationery.BackgroundImageData;
			}
			fSettings.backgroundImageSize = scribble.Stationery.BackgroundImageSize;
			fSettings.backgroundImageTransparency = scribble.Stationery.BackgroundImageTransparency;

			// Min Size
			fSettings.minWidth = scribble.Stationery.MinSize.Width;
			fSettings.minHeight = scribble.Stationery.MinSize.Height;
			fSettings.minSize = new Size(fSettings.minWidth, fSettings.minHeight);
		}

		private int ConvertToHimetric(string textValue)
		{
			int himetricValue;
			double doubleValue;

			if (textValue.Length < 1)
			{
				return 0;
			}
			if (textValue == ".")
			{
				return 0;
			}
			try
			{
				// this takes a long time
				doubleValue = Convert.ToDouble(textValue);
			}
			catch
			{
				return 0;
			}

			switch (fUnitsSetting)
			{
				case Units.HiMetric:
					himetricValue = (int)doubleValue;
					break;
				case Units.CM:
					himetricValue = (int)(doubleValue * 1000);
					break;
				case Units.Inches:
				default:
					himetricValue = (int)(doubleValue * 2450);
					break;
			}
			return himetricValue;
		}

		private double ConvertFromHimetric(int himetricValue)
		{
			double returnValue;

			switch (fUnitsSetting)
			{
				case Units.HiMetric:
					returnValue = (double) himetricValue;
					break;
				case Units.CM:
					returnValue = himetricValue / 1000F;
					break;
				case Units.Inches:
				default:
					returnValue = himetricValue / 2540F;
					break;
			}
			return returnValue;
		}

		private string TrimColorName(string colorName)
		{
			string trimString = "Color [";
			colorName = colorName.Remove(0, trimString.Length);
			colorName = colorName.Substring(0, colorName.Length - 1);
			return colorName;
		}

		private void InitalizeGUIElements()
		{
			// Fill all the list, combo, etc
			//H Line
			foreach (string s in Enum.GetNames(typeof(StationeryLineStyle)))
			{
				hLineStyleComboBox.Items.Add(s);
			}
			foreach (Color c in color)
			{
				hLineColorComboBox.Items.Add(TrimColorName(c.ToString()));
			}

			// V Line
			foreach (string s in Enum.GetNames(typeof(StationeryLineStyle)))
			{
				vLineStyleComboBox.Items.Add(s);
			}
			foreach (Color c in color)
			{
				vLineColorComboBox.Items.Add(TrimColorName(c.ToString()));
			}
			
			// R Margin
			foreach (string s in Enum.GetNames(typeof(StationeryLineStyle)))
			{
				rMarginLineStyleComboBox.Items.Add(s);
			}
			foreach (Color c in color)
			{
				rMarginLineColorComboBox.Items.Add(TrimColorName(c.ToString()));
			}

			// L Margin
			foreach (string s in Enum.GetNames(typeof(StationeryLineStyle)))
			{
				lMarginLineStyleComboBox.Items.Add(s);
			}
			foreach (Color c in color)
			{
				lMarginLineColorComboBox.Items.Add(TrimColorName(c.ToString()));
			}

			// Title Rect
			foreach (string s in Enum.GetNames(typeof(StationeryRectangleStyle)))
			{
				titleRectangleStyleComboBox.Items.Add(s);
			}

			// Background
			foreach (Color c in color)
			{
				backgroundColorComboBox.Items.Add(TrimColorName(c.ToString()));
			}
			foreach (string s in Enum.GetNames(typeof(StationeryColorStyle)))
			{
				backgroundColorStyleComboBox.Items.Add(s);
			}
			
			foreach (Color c in color)
			{
				titleForegroundColorComboBox.Items.Add(TrimColorName(c.ToString()));
			}

			foreach (string s in Enum.GetNames(typeof(StationeryImageStyle)))
			{
				backgroundImageStyleComboBox.Items.Add(s);
			}

			foreach (Color c in color)
			{
				titleBackgroundColorComboBox.Items.Add(TrimColorName(c.ToString()));
			}

			foreach (string s in Enum.GetNames(typeof(StationeryDateStyle)))
			{
				titleDateStyleComboBox.Items.Add(s);
			}

			backgroundImageComboBox.ImageList = backgroundImageList;
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Blue Parchment", 0));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Blue Weave", 1));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Brown Parchment", 2));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Crest Parchment", 3));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Dark Red Parchment", 4));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Gold Parchment", 5));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Green Parchment", 6));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Green Weave", 7));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Natural Parchment", 8));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Parchment", 9));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Pink Parchment", 10));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Purple Parchment", 11));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Red Parchment", 12));
			backgroundImageComboBox.Items.Add(new ImageComboBoxItem("Slate Parchment", 13));
			backgroundImageComboBox.Text = "";

		}

		private void SetTrackBar(TrackBar trackBar, int position, int positionAdjustment)
		{
			int trackBarValue = position / positionAdjustment;
			if (trackBarValue < trackBar.Minimum)
			{
				trackBarValue = trackBar.Minimum;
			}
			if (trackBarValue > trackBar.Maximum)
			{
				trackBarValue = trackBar.Maximum;
			}
			trackBar.Value = trackBarValue;
		}

		#endregion

		#region Horizontal Line
		private void HorizontalLineSetup()
		{
			hLineColorComboBox.SelectedIndex = hLineColorComboBox.FindString(TrimColorName(fSettings.hLineColor.ToString()));
			if (hLineColorComboBox.SelectedIndex < 0)
			{
				hLineColorComboBox.Text = TrimColorName(fSettings.hLineColor.ToString());
			}
			hLineStyleComboBox.SelectedIndex = hLineStyleComboBox.FindString(fSettings.hLineStyle.ToString());
			scribble.Stationery.HorizontalLineSpaceBefore = fSettings.hLineSpaceBefore;
			hLineSpaceBeforeTextBox.Text = ConvertFromHimetric(fSettings.hLineSpaceBefore).ToString();
			scribble.Stationery.HorizontalLineSpaceBetween = fSettings.hLineSpaceBetween;
			hLineSpaceBetweenTextBox.Text = ConvertFromHimetric(fSettings.hLineSpaceBetween).ToString();
		}

		private void hLineStyleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.HorizontalLineStyle = (Agilix.Ink.StationeryLineStyle)hLineStyleComboBox.SelectedIndex;
			fSettings.hLineStyle = scribble.Stationery.HorizontalLineStyle;
			Repaint();
		}

		private void hLineColorChooserButton_Click(object sender, System.EventArgs e)
		{
			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				scribble.Stationery.HorizontalLineColor = colorDialog.Color;
				fSettings.hLineColor = scribble.Stationery.LeftMarginLineColor;
				string colorName = colorDialog.Color.ToString();
				string trimString = "Color [";
				colorName = colorName.Remove(0, trimString.Length);
				colorName = colorName.Substring(0, colorName.Length - 1);
				hLineColorComboBox.Text = colorName;

				// if line style is set to none, set the to solid.
				if (fSettings.hLineStyle == StationeryLineStyle.None)
				{
					MakeSolidHLine();
				}

				Repaint();
			}

		}
		private void MakeSolidHLine()
		{
				fSettings.hLineStyle = StationeryLineStyle.Solid;
				scribble.Stationery.HorizontalLineStyle = fSettings.hLineStyle;
				hLineStyleComboBox.SelectedIndex = hLineStyleComboBox.FindString(fSettings.hLineStyle.ToString());
		}

		private void hLineSpaceBeforeTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = hLineSpaceBeforeTrackBar.Value * fTrackBarAdjustment.HLine;
			hLineSpaceBeforeTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}

		private void hLineSpaceBeforeTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int lineSpacing;
			if (hLineSpaceBeforeTextBox.Text.Length > 0)
			{
				lineSpacing = ConvertToHimetric(hLineSpaceBeforeTextBox.Text);
				scribble.Stationery.HorizontalLineSpaceBefore = lineSpacing;
				fSettings.hLineSpaceBefore = scribble.Stationery.HorizontalLineSpaceBefore;
				Repaint();
				SetTrackBar(hLineSpaceBeforeTrackBar, lineSpacing, fTrackBarAdjustment.HLine);
			}
		}

		private void hLineSpaceBetweenTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int lineSpacing;
			if (hLineSpaceBetweenTextBox.Text.Length > 0)
			{
				lineSpacing = ConvertToHimetric(hLineSpaceBetweenTextBox.Text);
				scribble.Stationery.HorizontalLineSpaceBetween = lineSpacing;
				fSettings.hLineSpaceBetween = scribble.Stationery.HorizontalLineSpaceBetween;
				Repaint();
				SetTrackBar(hLineSpaceBetweenTrackBar, lineSpacing - 100, 30);
			}
		}

		private void hLineSpaceBetweenTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = (hLineSpaceBetweenTrackBar.Value * 30) + 100;
			hLineSpaceBetweenTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}

		private void hLineColorComboBox_SelectedIndexChanged_1(object sender, System.EventArgs e)
		{
			if (hLineColorComboBox.SelectedIndex < 0)
			{
			}
			else
			{
				scribble.Stationery.HorizontalLineColor = color[hLineColorComboBox.SelectedIndex];
				fSettings.hLineColor = scribble.Stationery.LeftMarginLineColor;
			}
			Repaint();
		}
		#endregion

		#region Vertical Line
		private void VerticalLineSetup()
		{
			vLineColorComboBox.SelectedIndex = vLineColorComboBox.FindString(TrimColorName(fSettings.vLineColor.ToString()));
			if (vLineColorComboBox.SelectedIndex < 0)
			{
				vLineColorComboBox.Text = TrimColorName(fSettings.vLineColor.ToString());
			}
			vLineStyleComboBox.SelectedIndex = vLineStyleComboBox.FindString(fSettings.vLineStyle.ToString());
			scribble.Stationery.VerticalLineSpaceBetween = fSettings.vLineSpaceBetween;
			vLineSpaceBeforeTextBox.Text = ConvertFromHimetric(fSettings.vLineSpaceBefore).ToString();
			vLineSpaceBetweenTextBox.Text = ConvertFromHimetric(fSettings.vLineSpaceBetween).ToString();
		}

		private void vLineColorChooserButton_Click(object sender, System.EventArgs e)
		{
			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				scribble.Stationery.VerticalLineColor = colorDialog.Color;
				fSettings.vLineColor = scribble.Stationery.VerticalLineColor;
				vLineColorComboBox.Text = TrimColorName(colorDialog.Color.ToString());
				Repaint();
			}
		}

		private void vLineSpaceBetweenTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = (vLineSpaceBetweenTrackBar.Value * 50) + 100;
			vLineSpaceBetweenTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}

		private void vLineSpaceBeforeTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = vLineSpaceBeforeTrackBar.Value * fTrackBarAdjustment.VLine;
			vLineSpaceBeforeTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}

		private void vLineSpaceBeforeTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int lineSpacing;
			if (vLineSpaceBeforeTextBox.Text.Length > 0)
			{
				lineSpacing = ConvertToHimetric(vLineSpaceBeforeTextBox.Text);
				scribble.Stationery.VerticalLineSpaceBefore = lineSpacing;
				fSettings.vLineSpaceBefore = scribble.Stationery.VerticalLineSpaceBefore;
				Repaint();
				SetTrackBar(vLineSpaceBeforeTrackBar, lineSpacing, fTrackBarAdjustment.VLine);
			}
		}

		private void vLineSpaceBetweenTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int lineSpacing;
			if (vLineSpaceBetweenTextBox.Text.Length > 0)
			{
				lineSpacing = ConvertToHimetric(vLineSpaceBetweenTextBox.Text);
				scribble.Stationery.VerticalLineSpaceBetween = lineSpacing;
				fSettings.vLineSpaceBetween = scribble.Stationery.VerticalLineSpaceBetween;
				Repaint();
				SetTrackBar(vLineSpaceBetweenTrackBar, lineSpacing - 100, 50);
			}
		}

		private void vLineStyleComboBox_SelectedValueChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.VerticalLineStyle = (Agilix.Ink.StationeryLineStyle)vLineStyleComboBox.SelectedIndex;
			fSettings.vLineStyle = scribble.Stationery.VerticalLineStyle;
			Repaint();
		}

		private void vLineColorComboBox_SelectedValueChanged(object sender, System.EventArgs e)
		{
			if (vLineColorComboBox.SelectedIndex < 0)
			{
			}
			else
			{
				scribble.Stationery.VerticalLineColor = color[vLineColorComboBox.SelectedIndex];
				fSettings.vLineColor = scribble.Stationery.VerticalLineColor;
			}
			Repaint();
		}
		#endregion

		#region Right Margin Line
		private void RightMarginLineSetup()
		{
			rMarginLineColorComboBox.SelectedIndex = rMarginLineColorComboBox.FindString(TrimColorName(fSettings.rMarginColor.ToString()));
			if (rMarginLineColorComboBox.SelectedIndex < 0)
			{
				rMarginLineColorComboBox.Text = TrimColorName(fSettings.rMarginColor.ToString());
			}
			scribble.Stationery.RightMarginLineSpacing = fSettings.rMarginSpacing;
			rMarginLineSpacingTextBox.Text = ConvertFromHimetric(fSettings.rMarginSpacing).ToString();
			rMarginLineStyleComboBox.SelectedIndex = rMarginLineStyleComboBox.FindString(fSettings.rMarginStyle.ToString());
		}

		private void rMarginLineColorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (rMarginLineColorComboBox.SelectedIndex < 0)
			{
			}
			else
			{
				scribble.Stationery.RightMarginLineColor = color[rMarginLineColorComboBox.SelectedIndex];
				fSettings.rMarginColor = scribble.Stationery.RightMarginLineColor;
			}
			Repaint();
		}

		private void rMarginLineStyleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.RightMarginLineStyle = (Agilix.Ink.StationeryLineStyle)rMarginLineStyleComboBox.SelectedIndex;
			fSettings.rMarginStyle = scribble.Stationery.RightMarginLineStyle;
			Repaint();
		}

		private void rightMarginLineColorChooserButton_Click(object sender, System.EventArgs e)
		{
			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				scribble.Stationery.RightMarginLineColor = colorDialog.Color;
				fSettings.rMarginColor = scribble.Stationery.RightMarginLineColor;
				rMarginLineColorComboBox.Text = TrimColorName(colorDialog.Color.ToString());
				Repaint();
			}
		}

		private void rMarginLineSpacingTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int lineSpacing;
			if (rMarginLineSpacingTextBox.Text.Length > 0)
			{
				lineSpacing = ConvertToHimetric(rMarginLineSpacingTextBox.Text);
				scribble.Stationery.RightMarginLineSpacing = lineSpacing;
				fSettings.rMarginSpacing = scribble.Stationery.RightMarginLineSpacing;
				Repaint();
				// TODO: base this off of the right edge of the page. 
				SetTrackBar(rMarginLineSpacingTrackBar, lineSpacing - 10000, fTrackBarAdjustment.RMargin);
			}
		}

		private void rMarginLineSpacingTrackBar_Scroll(object sender, System.EventArgs e)
		{
			// TODO: Base this off of the right side of the page.
			int himetricValue = rMarginLineSpacingTrackBar.Value * fTrackBarAdjustment.RMargin + 10000;
			rMarginLineSpacingTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}
		#endregion

		#region Left Margin Line
		private void LeftMarginLineSetup()
		{
			lMarginLineColorComboBox.SelectedIndex = lMarginLineColorComboBox.FindString(TrimColorName(fSettings.lMarginColor.ToString()));
			if (lMarginLineColorComboBox.SelectedIndex < 0)
			{
				lMarginLineColorComboBox.Text = TrimColorName(fSettings.lMarginColor.ToString());
			}
			scribble.Stationery.LeftMarginLineSpacing = fSettings.lMarginSpacing;
			lMarginLineSpacingTextBox.Text = ConvertFromHimetric(fSettings.lMarginSpacing).ToString();
			lMarginLineStyleComboBox.SelectedIndex = lMarginLineStyleComboBox.FindString(fSettings.lMarginStyle.ToString());
		}


		private void leftMarginLineColorChooserButton_Click(object sender, System.EventArgs e)
		{
			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				scribble.Stationery.LeftMarginLineColor = colorDialog.Color;
				fSettings.lMarginColor = scribble.Stationery.LeftMarginLineColor;
				lMarginLineColorComboBox.Text = TrimColorName(colorDialog.Color.ToString());
				Repaint();
			}
		}

		private void lMarginLineSpacingTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int lineSpacing;
			if (lMarginLineSpacingTextBox.Text.Length > 0)
			{
				lineSpacing = ConvertToHimetric(lMarginLineSpacingTextBox.Text);
				scribble.Stationery.LeftMarginLineSpacing = lineSpacing;
				fSettings.lMarginSpacing = scribble.Stationery.LeftMarginLineSpacing;
				SetTrackBar(lMarginLineSpacingTrackBar, lineSpacing, fTrackBarAdjustment.LMargin);
				Repaint();
			}
		}

		private void lMarginLineColorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (lMarginLineColorComboBox.SelectedIndex < 0)
			{
			}
			else
			{
				scribble.Stationery.LeftMarginLineColor = color[lMarginLineColorComboBox.SelectedIndex];
				fSettings.lMarginColor = scribble.Stationery.LeftMarginLineColor;
			}
			Repaint();
		}

		private void lMarginLineStyleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.LeftMarginLineStyle = (Agilix.Ink.StationeryLineStyle)lMarginLineStyleComboBox.SelectedIndex;
			fSettings.lMarginStyle = scribble.Stationery.LeftMarginLineStyle;
			Repaint();
		}

		private void lMarginLineSpacingTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = lMarginLineSpacingTrackBar.Value * fTrackBarAdjustment.LMargin;
			lMarginLineSpacingTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}
		#endregion

		#region Title Rectangle
		private void TitleRectangleSetup()
		{
			// Style
			titleRectangleStyleComboBox.SelectedIndex = titleRectangleStyleComboBox.FindString(fSettings.titleRectangleStyle.ToString());

			// Width
			titleWidthTextBox.Text = ConvertFromHimetric(fSettings.titleRectangle.Width).ToString();
			SetTrackBar(titleWidthTrackBar, fSettings.titleRectangle.Width, fTrackBarAdjustment.TitleWidth);
			fTitleRectangle.Width = fSettings.titleRectangle.Width;

			// Height
			titleHeightTextBox.Text = ConvertFromHimetric(fSettings.titleRectangle.Height).ToString();
			SetTrackBar(titleHeightTrackBar, fSettings.titleRectangle.Height, fTrackBarAdjustment.TitleHeight);
			fTitleRectangle.Height = fSettings.titleRectangle.Height;

			// X
			xTitleCornerTextBox.Text = ConvertFromHimetric(fSettings.titleRectangle.X).ToString();
			xTitleCornerTrackBar.Value = fSettings.titleRectangle.X / fTrackBarAdjustment.TitleX;
			fTitleRectangle.X = fSettings.titleRectangle.X;

			// Y
			yTitleCornerTextBox.Text = ConvertFromHimetric(fSettings.titleRectangle.Y).ToString();
			yTitleCornerTrackBar.Value = fSettings.titleRectangle.Y / fTrackBarAdjustment.TitleY;
			fTitleRectangle.Y = fSettings.titleRectangle.Y;

			scribble.Stationery.TitleRectangle = fTitleRectangle;
		}

		private void UpdateTitleRectangle()
		{
			scribble.Stationery.TitleRectangle = fTitleRectangle;
			fSettings.titleRectangle = scribble.Stationery.TitleRectangle;
			Repaint();
		}

		private void titleRectangleStyleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.TitleRectangleStyle = (Agilix.Ink.StationeryRectangleStyle)titleRectangleStyleComboBox.SelectedIndex;
			fSettings.titleRectangleStyle = scribble.Stationery.TitleRectangleStyle;
			Repaint();
		}

		private void titleWidthTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = titleWidthTrackBar.Value * fTrackBarAdjustment.TitleWidth;
			titleWidthTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}

		private void titleWidthTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int titleWidth;
			if (titleWidthTextBox.Text.Length > 0)
			{
				titleWidth = ConvertToHimetric(titleWidthTextBox.Text);
				fTitleRectangle.Width = titleWidth;
				UpdateTitleRectangle();
				SetTrackBar(titleWidthTrackBar, titleWidth, fTrackBarAdjustment.TitleWidth);
			}
		}

		private void titleHeightTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = titleHeightTrackBar.Value * fTrackBarAdjustment.TitleHeight;
			titleHeightTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}

		private void titleHeightTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int titleHeight;
			if (titleHeightTextBox.Text.Length > 0)
			{
				titleHeight = ConvertToHimetric(titleHeightTextBox.Text);
				fTitleRectangle.Height = titleHeight;
				UpdateTitleRectangle();
				SetTrackBar(titleHeightTrackBar, titleHeight, fTrackBarAdjustment.TitleHeight);
			}
		}

		private void xTitleCornerTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = xTitleCornerTrackBar.Value * fTrackBarAdjustment.TitleX;
			xTitleCornerTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}

		private void xTitleCornerTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int titleX;
			if (xTitleCornerTextBox.Text.Length > 0)
			{
				titleX = ConvertToHimetric(xTitleCornerTextBox.Text);
				fTitleRectangle.X = titleX;
				UpdateTitleRectangle();
				SetTrackBar(xTitleCornerTrackBar, titleX, fTrackBarAdjustment.TitleX);
			}
		}

		private void yTitleCornerTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = yTitleCornerTrackBar.Value * fTrackBarAdjustment.TitleY;
			yTitleCornerTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}

		private void yTitleCornerTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int titleY;
			if (yTitleCornerTextBox.Text.Length > 0)
			{
				titleY = ConvertToHimetric(yTitleCornerTextBox.Text);
				fTitleRectangle.Y = titleY;
				UpdateTitleRectangle();
				SetTrackBar(yTitleCornerTrackBar, titleY, fTrackBarAdjustment.TitleY);
			}
		}
		#endregion
		
		#region Background
		private void BackgroundSetup()
		{
			backgroundColorComboBox.SelectedIndex = backgroundColorComboBox.FindString(TrimColorName(fSettings.backgroundColor.ToString()));
			backgroundColorStyleComboBox.SelectedIndex = backgroundColorStyleComboBox.FindString(fSettings.backgroundStyle.ToString());
			string s = fSettings.backgroundImageStyle.ToString();
			backgroundImageStyleComboBox.SelectedIndex = backgroundImageStyleComboBox.FindString(fSettings.backgroundImageStyle.ToString());

			imageWidthTextBox.Text = ConvertFromHimetric(fSettings.backgroundImageSize.Width).ToString();
			imageHeightTextBox.Text = ConvertFromHimetric(fSettings.backgroundImageSize.Height).ToString();

			SetTrackBar(backgroundImageTransparencyTrackBar, Convert.ToInt32(fSettings.backgroundImageTransparency * 100), 5);
			backgroundImageTransparencyLabel.Text = Convert.ToString(fSettings.backgroundImageTransparency * 100) + "%";
			scribble.Stationery.BackgroundImageTransparency = fSettings.backgroundImageTransparency;
		}

		private void alignImageWithMinSizeCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if (alignImageWithMinSizeCheckBox.Checked)
			{
				fSettings.alignImageWithMinSize = true;		
			}
			else
			{
				fSettings.alignImageWithMinSize = false;		
			}
			scribble.Stationery.AlignImageWithMinSize = fSettings.alignImageWithMinSize;
			Repaint();
		}

		private void imageWidthTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int imageWidth;
			if (imageWidthTextBox.Text.Length > 0)
			{
				imageWidth = ConvertToHimetric(imageWidthTextBox.Text);
				Size s = new Size(imageWidth, fSettings.backgroundImageSize.Height);
				fSettings.backgroundImageSize = s;
				scribble.Stationery.BackgroundImageSize = s;
				Repaint();
			}
		}

		private void imageHeightTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int imageHeight;
			if (imageHeightTextBox.Text.Length > 0)
			{
				imageHeight = ConvertToHimetric(imageHeightTextBox.Text);
				Size s = new Size(fSettings.backgroundImageSize.Width, imageHeight);
				fSettings.backgroundImageSize = s;
				scribble.Stationery.BackgroundImageSize = s;
				Repaint();
			}
		}

		private void backgroundColorStyleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.BackgroundColorStyle = (Agilix.Ink.StationeryColorStyle)backgroundColorStyleComboBox.SelectedIndex;
			fSettings.backgroundStyle = scribble.Stationery.BackgroundColorStyle;
			Repaint();
		}

		private void backgroundColorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.BackgroundColor = color[backgroundColorComboBox.SelectedIndex];
			fSettings.backgroundColor = scribble.Stationery.BackgroundColor;
			Repaint();
		}

		private void backgroundImageStyleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.BackgroundImageStyle = (Agilix.Ink.StationeryImageStyle)backgroundImageStyleComboBox.SelectedIndex;
			fSettings.backgroundImageStyle = scribble.Stationery.BackgroundImageStyle;
			Repaint();
		}

		private void backgroundImageTransparencyTrackBar_Scroll(object sender, System.EventArgs e)
		{
			float imageTransparency = backgroundImageTransparencyTrackBar.Value / .20F;
			backgroundImageTransparencyLabel.Text = imageTransparency.ToString() + "%";
			scribble.Stationery.BackgroundImageTransparency = imageTransparency / 100;
			fSettings.backgroundImageTransparency = imageTransparency / 100;
			Repaint();
		}
		private void backgroundImageButton_Click(object sender, System.EventArgs e)
		{
			if (openBackgroundImageFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				FileStream fs = File.OpenRead(openBackgroundImageFileDialog.FileName);
				scribble.Stationery.BackgroundImageData = fs;

				// convert pixels to himetric
				Bitmap bitmapImage = new Bitmap(fs,false);
				fs.Close();
				Size s = bitmapImage.Size;
				s.Height = scribble.PixelToInkSpaceY(s.Height);
				s.Width = scribble.PixelToInkSpaceX(s.Width);

				scribble.Stationery.BackgroundImageSize = s;
				fSettings.backgroundImageSize = scribble.Stationery.BackgroundImageSize;

				// Update the GUI
				backgroundImageComboBox.Text = "<custom>";
				imageWidthTextBox.Text = ConvertFromHimetric(fSettings.backgroundImageSize.Width).ToString();
				imageHeightTextBox.Text = ConvertFromHimetric(fSettings.backgroundImageSize.Height).ToString();

				// if background image style is set to none, set the to TopLeft.
				if (fSettings.backgroundImageStyle == StationeryImageStyle.None)
				{
					fSettings.backgroundImageStyle = StationeryImageStyle.TopLeft;
					scribble.Stationery.BackgroundImageStyle = fSettings.backgroundImageStyle;
					backgroundImageStyleComboBox.SelectedIndex = backgroundImageStyleComboBox.FindString(fSettings.backgroundImageStyle.ToString());
				}

				Repaint();
			}
		}

		private void backgroundImageComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string sImageName;
			switch (backgroundImageComboBox.SelectedIndex)
			{
				case 0:
					sImageName = "blueparchment.jpg";
					break;
				case 1:
					sImageName = "BlueWeave.JPG";
					break;
				case 2:
					sImageName = "brownparchment.jpg";
					break;
				case 3:
					sImageName = "Crest.gif";
					break;
				case 4:
					sImageName = "dkredparchment.jpg";
					break;
				case 5:
					sImageName = "goldparchment.jpg";
					break;
				case 6:
					sImageName = "greenparchment.jpg";
					break;
				case 7:
					sImageName = "GreenWeave.jpg";
					break;
				case 8:
					sImageName = "PaperNatural.gif";
					break;
				case 9:
					sImageName = "parchment.jpg";
					break;
				case 10:
					sImageName = "pinkparchment.jpg";
					break;
				case 11:
					sImageName = "purpleparchment.jpg";
					break;
				case 12:
					sImageName = "redparchment.jpg";
					break;
				case 13:
				default:
					sImageName = "slateparchment.jpg";
					break;
			}

			// Get the bitmap from the resources
			Bitmap bitmapImage = new Bitmap(GetType(), sImageName);

			// convert pixels to himetric
			Size s = bitmapImage.Size;
			s.Height = scribble.PixelToInkSpaceY(s.Height);
			s.Width = scribble.PixelToInkSpaceX(s.Width);

			scribble.Stationery.BackgroundImage = bitmapImage;
			scribble.Stationery.BackgroundImageSize = s;
			fSettings.backgroundImageSize = scribble.Stationery.BackgroundImageSize;
			
			// Update the GUI
			imageWidthTextBox.Text = ConvertFromHimetric(fSettings.backgroundImageSize.Width).ToString();
			imageHeightTextBox.Text = ConvertFromHimetric(fSettings.backgroundImageSize.Height).ToString();

			// if background image style is set to none, set the to Tile.
			if (fSettings.backgroundImageStyle == StationeryImageStyle.None)
			{
				fSettings.backgroundImageStyle = StationeryImageStyle.Tile;
				scribble.Stationery.BackgroundImageStyle = fSettings.backgroundImageStyle;
				backgroundImageStyleComboBox.SelectedIndex = backgroundImageStyleComboBox.FindString(fSettings.backgroundImageStyle.ToString());
			}

			Repaint();
		}
		#endregion

		#region Page Size
		private void PageSizeSetup()
		{
			sizeHeightOnlyTextBox.Text = ConvertFromHimetric(fSettings.minHeight).ToString();
			sizeWidthOnlyTextBox.Text = ConvertFromHimetric(fSettings.minWidth).ToString();
		}

		private void scribble_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (fSettings.minWidth > 0)
			{
				DrawMinWidth(fSettings.minWidth, e);
			}
			if (fSettings.minHeight > 0)
			{
				DrawMinHeight(fSettings.minHeight, e);
			}
		}

		private void sizeHeightOnlyTextBox_TextChanged(object sender, System.EventArgs e)
		{
			fSettings.minHeight = ConvertToHimetric(sizeHeightOnlyTextBox.Text);
			scribble.Stationery.MinHeight = fSettings.minHeight;
			
			// Resize the scribble control so it knows there is a new minimum size and puts up the scroll bars.
			Size currentSize = new Size(scribble.Size.Width, scribble.Size.Height);
			Size newSize = new Size(currentSize.Width,currentSize.Height + 1);
			scribble.Size = newSize;
			scribble.Size = currentSize;

			Repaint();
		}

		private void DrawMinHeight(int height, PaintEventArgs e)
		{
			int shadowOffset = 4;
			int lineOffset = 20; // Amount of space to move the text above the line

			// Calculate the right end of the line
			int lineEnd;
			if (fSettings.minWidth > 0)
			{
				//Draw to the minimum width
				lineEnd = scribble.InkSpaceToPixelX(fSettings.minWidth);
			}
			else
			{
				lineEnd = scribble.Size.Width;
			}

			//Make a dotted line
			Pen pen = new Pen(Color.Black);
			pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
			pen.DashPattern = new float[]
				{
					8, 8
				};
			e.Graphics.DrawLine(pen,new Point(0,scribble.InkSpaceToPixelY(fSettings.minHeight)), new Point(lineEnd,scribble.InkSpaceToPixelY(fSettings.minHeight)));

			// Draw the string.
			e.Graphics.DrawString("Minimum Height",
					new Font("Arial", 12),
					Brushes.LightGray,
					new Rectangle(														
						(lineEnd/2) - 100 + shadowOffset, // X
						scribble.InkSpaceToPixelY(fSettings.minHeight) + shadowOffset - lineOffset, // Y
						(lineEnd / 2) + 100 + shadowOffset, // Width
						scribble.InkSpaceToPixelY(fSettings.minHeight) + shadowOffset) // Height
						); 
			e.Graphics.DrawString("Minimum Height",
				new Font("Arial", 12),
				Brushes.Black,
				new Rectangle(														
					(lineEnd/2) - 100, // X
					scribble.InkSpaceToPixelY(fSettings.minHeight)- lineOffset, // Y
					(lineEnd / 2) + 100 , // Width
					scribble.InkSpaceToPixelY(fSettings.minHeight)) // Height
				); 
		}

		private void DrawMinWidth(int width, PaintEventArgs e)
		{
			int shadowOffset = 4;
			int lineOffset = 20; // Amount of space to move the text left of the line

			// Calculate the right end of the line
			int lineEnd;
			if (fSettings.minHeight > 0)
			{
				//Draw to the minimum height
				lineEnd = scribble.InkSpaceToPixelY(fSettings.minHeight);
			}
			else
			{
				lineEnd = scribble.Size.Height;
			}

			//Make a dotted line
			Pen pen = new Pen(Color.Black);
			pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
			pen.DashPattern = new float[]
				{
					8, 8
				};
			e.Graphics.DrawLine(pen,new Point(scribble.InkSpaceToPixelX(fSettings.minWidth),0), new Point(scribble.InkSpaceToPixelX(fSettings.minWidth),lineEnd));

			// Draw the string.
			StringFormat drawFormat = new StringFormat(StringFormatFlags.DirectionVertical);
			e.Graphics.DrawString("Minimum Width",
				new Font("Arial", 12),
				Brushes.LightGray,
				new Rectangle(														
					scribble.InkSpaceToPixelX(fSettings.minWidth) + shadowOffset - lineOffset, 
					(lineEnd/2) - 70 + shadowOffset, 
					scribble.InkSpaceToPixelX(fSettings.minWidth) + shadowOffset,
					(lineEnd / 2) + 70 + shadowOffset
					),
				drawFormat
				); 
			e.Graphics.DrawString("Minimum Width",
				new Font("Arial", 12),
				Brushes.Black,
				new Rectangle(														
					scribble.InkSpaceToPixelX(fSettings.minWidth) - lineOffset, 
					(lineEnd/2) - 70 + shadowOffset, 
					scribble.InkSpaceToPixelX(fSettings.minWidth) ,
					(lineEnd / 2) + 70 + shadowOffset
					),
				drawFormat
				); 
		}

		private void sizeWidthOnlyTextBox_TextChanged(object sender, System.EventArgs e)
		{
			fSettings.minWidth = ConvertToHimetric(sizeWidthOnlyTextBox.Text);
			scribble.Stationery.MinWidth = fSettings.minWidth;

			// Resize the scribble control so it knows there is a new minimum size and puts up the scroll bars.
			Size currentSize = new Size(scribble.Size.Width, scribble.Size.Height);
			Size newSize = new Size(currentSize.Width,currentSize.Height + 1);
			scribble.Size = newSize;
			scribble.Size = currentSize;

			Repaint();
		}

		#endregion

		#region Title
		private void TitleSetup()
		{
			titleBackgroundColorComboBox.Text = TrimColorName(fSettings.titleBackgroundColor.ToString());
			titleForegroundColorComboBox.Text = TrimColorName(fSettings.titleForegroundColor.ToString());
			titleDateStyleComboBox.SelectedIndex = titleDateStyleComboBox.FindString(fSettings.titleDateStyle.ToString());
			titleTextTextBox.Text = fSettings.titleString;
			titleSpaceAfterTextBox.Text = ConvertFromHimetric(fSettings.titleSpaceAfter).ToString();
			scribble.Stationery.TitleBaselineSpaceAfter = fSettings.titleSpaceAfter;
			titleSpaceAfterTrackBar.Value = fSettings.titleSpaceAfter / fTrackBarAdjustment.TitleSpaceAfter;
			displayNameTextBox.Text = fSettings.displayName;
		}

		private void displayNameTextBox_TextChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.DisplayName = displayNameTextBox.Text;
			fSettings.displayName = displayNameTextBox.Text;
			Form1.ActiveForm.Text = fTitleText + " - " + displayNameTextBox.Text;
		}

		private void titleSpaceAfterTrackBar_Scroll(object sender, System.EventArgs e)
		{
			int himetricValue = titleSpaceAfterTrackBar.Value * fTrackBarAdjustment.TitleSpaceAfter;
			titleSpaceAfterTextBox.Text = Convert.ToString(ConvertFromHimetric(himetricValue));
		}

		private void titleSpaceAfterTextBox_TextChanged(object sender, System.EventArgs e)
		{
			int titleHeight;
			if (titleSpaceAfterTextBox.Text.Length > 0)
			{
				titleHeight = ConvertToHimetric(titleSpaceAfterTextBox.Text);
				scribble.Stationery.TitleBaselineSpaceAfter = titleHeight;
				fSettings.titleSpaceAfter = scribble.Stationery.TitleBaselineSpaceAfter;
				SetTrackBar(titleHeightTrackBar, fSettings.titleRectangle.Height, fTrackBarAdjustment.TitleSpaceAfter);
				Repaint();
			}
		}

		private void titleDateStyleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			scribble.Stationery.TitleDateStyle = (Agilix.Ink.StationeryDateStyle)titleDateStyleComboBox.SelectedIndex;
			fSettings.titleDateStyle = scribble.Stationery.TitleDateStyle;
			Repaint();
		}

		private void titleTextTextBox_TextChanged(object sender, System.EventArgs e)
		{
			if (titleTextTextBox.Text.Length > 0)
			{
				scribble.Stationery.TitleText = titleTextTextBox.Text;
				fSettings.titleString = scribble.Stationery.TitleText;
				Repaint();
			}
		}

		private void titleForegroundColorComboBox_TextChanged(object sender, System.EventArgs e)
		{
			if (titleForegroundColorComboBox.SelectedIndex < 0)
			{
			}
			else
			{
				scribble.Stationery.TitleForegroundColor = color[titleForegroundColorComboBox.SelectedIndex];
				fSettings.titleForegroundColor = scribble.Stationery.TitleForegroundColor;
				Repaint();
			}
		}

		private void backgroundColorChooserButton_Click(object sender, System.EventArgs e)
		{
			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				scribble.Stationery.BackgroundColor = colorDialog.Color;
				fSettings.backgroundColor = scribble.Stationery.BackgroundColor;
				backgroundColorComboBox.Text = TrimColorName(colorDialog.Color.ToString());
				Repaint();
			}
		}

		private void titleBackgroundColorComboBox_TextChanged(object sender, System.EventArgs e)
		{
			if (titleBackgroundColorComboBox.SelectedIndex > -1)
			{
				scribble.Stationery.TitleBackgroundColor = color[titleBackgroundColorComboBox.SelectedIndex];
				fSettings.titleBackgroundColor = scribble.Stationery.TitleBackgroundColor;
				Repaint();
			}
		}

		private void titleForegroundColorChooserButton_Click(object sender, System.EventArgs e)
		{
			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				scribble.Stationery.TitleForegroundColor = colorDialog.Color;
				fSettings.titleForegroundColor = scribble.Stationery.TitleForegroundColor;
				titleForegroundColorComboBox.Text = TrimColorName(colorDialog.Color.ToString());
				Repaint();
			}
		}

		private void titleBackgroundColorChooserButton_Click(object sender, System.EventArgs e)
		{
			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				scribble.Stationery.TitleBackgroundColor = colorDialog.Color;
				fSettings.titleBackgroundColor = scribble.Stationery.TitleBackgroundColor;
				titleBackgroundColorComboBox.Text = TrimColorName(colorDialog.Color.ToString());
				Repaint();
			}
		}
		#endregion

		#region Menu
		private void aboutMenuItem_Click(object sender, System.EventArgs e)
		{
			new Agilix.Ink.Dialogs.About().ShowDialog(this);
		}

		private void zoom200MenuItem_Click(object sender, System.EventArgs e)
		{
			scribble.Zoom = 2F;
			hRuler.ZoomFactor = 2F;
			vRuler.ZoomFactor = 2F;
			zoom200MenuItem.Checked = true;
			zoom150MenuItem.Checked = false;
			zoom100MenuItem.Checked = false;
			zoom50MenuItem.Checked = false;
			zoom25MenuItem.Checked = false;
		}

		private void zoom150MenuItem_Click(object sender, System.EventArgs e)
		{
			scribble.Zoom = 1.5F;
			hRuler.ZoomFactor = 1.5F;
			vRuler.ZoomFactor = 1.5F;
			zoom200MenuItem.Checked = false;
			zoom150MenuItem.Checked = true;
			zoom100MenuItem.Checked = false;
			zoom50MenuItem.Checked = false;
			zoom25MenuItem.Checked = false;
		}

		private void zoom100MenuItem_Click(object sender, System.EventArgs e)
		{
			scribble.Zoom = 1F;
			hRuler.ZoomFactor = 1F;
			vRuler.ZoomFactor = 1F;
			zoom200MenuItem.Checked = false;
			zoom150MenuItem.Checked = false;
			zoom100MenuItem.Checked = true;
			zoom50MenuItem.Checked = false;
			zoom25MenuItem.Checked = false;
		}

		private void zoom50MenuItem_Click(object sender, System.EventArgs e)
		{
			scribble.Zoom = .8F;
			hRuler.ZoomFactor = .8F;
			vRuler.ZoomFactor = .8F;
			zoom200MenuItem.Checked = false;
			zoom150MenuItem.Checked = false;
			zoom100MenuItem.Checked = false;
			zoom50MenuItem.Checked = true;
			zoom25MenuItem.Checked = false;
		}

		private void zoom25MenuItem_Click(object sender, System.EventArgs e)
		{
			scribble.Zoom = .5F;
			hRuler.ZoomFactor = .5F;
			vRuler.ZoomFactor = .5F;
			zoom200MenuItem.Checked = false;
			zoom150MenuItem.Checked = false;
			zoom100MenuItem.Checked = false;
			zoom50MenuItem.Checked = false;
			zoom25MenuItem.Checked = true;
		}

		private void openMenuItem_Click(object sender, System.EventArgs e)
		{
			if (scribble.Modified) // Need dirty flag for the stationery
			{
				// TODO: See if you need to save the current stationery

			}

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try 
				{
					using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
					{
						scribble.Stationery = new Stationery();
						scribble.Stationery.Load(fs);
					}
					LoadStationerySettings();
					LoadValues();
					fFileName = openFileDialog.FileName;
				}
				catch(Exception ex)
				{
					MessageBox.Show(this,ex.Message);
				}
			}
		}

		private void saveAsMenuItem_Click(object sender, System.EventArgs e)
		{
			fTempFileName = fFileName;
			fFileName = ""; // If they cancel the Save dialog there won't be a name anymore.
			saveMenuItem_Click(sender, e);
		}

		private void saveMenuItem_Click(object sender, System.EventArgs e)
		{
			if (fFileName.Length < 1)
			{
				if (saveFileDialog.ShowDialog() != DialogResult.OK)
				{
					fFileName = fTempFileName;
					return;
				}
				fFileName = saveFileDialog.FileName;
			}
			try
			{
				using (FileStream fs = new FileStream(fFileName,FileMode.Create))
				{
					scribble.Stationery.Save(fs);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this,ex.Message);
			}
		}

		private void exitMenuItem_Click(object sender, System.EventArgs e)
		{
			// If dirty, ask if they want to save.
			Dispose();		
		}

		private void inchesMenuItem_Click(object sender, System.EventArgs e)
		{
			if (fUnitsSetting != Units.Inches)
			{
				fUnitsSetting = Units.Inches;
				hRuler.ScaleMode = enumScaleMode.smInches;
				vRuler.ScaleMode = enumScaleMode.smInches;
				LoadValues();
				inchesMenuItem.Checked = true;
				centimetersMenuItem.Checked = false;
				himetricsMenuItem.Checked = false;
			}
		}

		private void centimetersMenuItem_Click(object sender, System.EventArgs e)
		{
			fUnitsSetting = Units.CM;
			hRuler.ScaleMode = enumScaleMode.smCentimetres;
			vRuler.ScaleMode = enumScaleMode.smCentimetres;
			centimetersMenuItem.Checked = true;
			himetricsMenuItem.Checked = false;
			inchesMenuItem.Checked = false;
			LoadValues();
		}

		private void himetricsMenuItem_Click(object sender, System.EventArgs e)
		{
			fUnitsSetting = Units.HiMetric;
			hRuler.ScaleMode = enumScaleMode.smHimetrics;
			vRuler.ScaleMode = enumScaleMode.smHimetrics;
			himetricsMenuItem.Checked = true;
			centimetersMenuItem.Checked = false;
			inchesMenuItem.Checked = false;
			LoadValues();
		}

		private void paperBlankMenuItem_Click(object sender, System.EventArgs e)
		{
			// Check to see if you need to save.
			scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Blank, 19050);
			LoadStationerySettings();
			LoadValues();
		}

		private void paperNarrowMenuItem_Click(object sender, System.EventArgs e)
		{
			// Check to see if you need to save.
			scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Narrow, 19050);
			LoadStationerySettings();
			LoadValues();
		}

		private void paperCollegeMenuItem_Click(object sender, System.EventArgs e)
		{
			// Check to see if you need to save.
			scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.College, 19050);
			LoadStationerySettings();
			LoadValues();
		}

		private void paperStandardMenuItem_Click(object sender, System.EventArgs e)
		{
			// Check to see if you need to save.
			scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050);
			LoadStationerySettings();
			LoadValues();
		}

		private void paperWideMenuItem_Click(object sender, System.EventArgs e)
		{
			// Check to see if you need to save.
			scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Wide, 19050);
			LoadStationerySettings();
			LoadValues();
		}

		private void paperSmallGridMenuItem_Click(object sender, System.EventArgs e)
		{
			// Check to see if you need to save.
			scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.SmallGrid, 19050);
			LoadStationerySettings();
			LoadValues();
		}

		private void paperGridMenuItem_Click(object sender, System.EventArgs e)
		{
			// Check to see if you need to save.
			scribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Grid, 19050);
			LoadStationerySettings();
			LoadValues();
		}

		private void newMenuItem_Click(object sender, System.EventArgs e)
		{
			DialogResult result;

			result = MessageBox.Show(this, "Do you want to save the current stationery?", "Save?", MessageBoxButtons.YesNo);
			if(result == DialogResult.Yes)
			{
				saveMenuItem_Click(sender, e);
			}			
			fSettings.Blank();
			LoadValues();
		}

		#endregion
	}
}

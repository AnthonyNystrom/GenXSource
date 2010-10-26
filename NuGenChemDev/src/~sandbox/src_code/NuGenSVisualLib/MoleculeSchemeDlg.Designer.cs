namespace NuGenSVisualLib
{
    partial class MoleculeSchemeDlg
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

                updateThread.Abort();
                updateThread.Join();
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
            this.components = new System.ComponentModel.Container();
            Janus.Windows.Common.JanusColorScheme janusColorScheme1 = new Janus.Windows.Common.JanusColorScheme();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem1 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem2 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem3 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem4 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem5 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem6 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem7 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem8 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem9 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup1 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup2 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup3 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup4 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup5 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup6 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup7 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup8 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem10 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem11 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem12 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem13 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem14 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem15 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem16 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem17 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup9 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.ButtonBar.ButtonBarGroup buttonBarGroup10 = new Janus.Windows.ButtonBar.ButtonBarGroup();
            Janus.Windows.GridEX.GridEXLayout gridEXLayout1 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MoleculeSchemeDlg));
            this.panel2 = new System.Windows.Forms.Panel();
            this.uiBondsGSetGroup = new Janus.Windows.EditControls.UIGroupBox();
            this.uiBondDDraw = new Janus.Windows.EditControls.UICheckBox();
            this.visualStyleManager1 = new Janus.Windows.Common.VisualStyleManager(this.components);
            this.uiBondEndTypeList = new Janus.Windows.EditControls.UIComboBox();
            this.uiBondSpacingList = new Janus.Windows.EditControls.UIComboBox();
            this.uiBondLODControl = new NuGenSVisualLib.LODcontrol();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.uiAtomsGSetGroup = new Janus.Windows.EditControls.UIGroupBox();
            this.uiAtomDDraw = new Janus.Windows.EditControls.UICheckBox();
            this.uiGroupBox6 = new Janus.Windows.EditControls.UIGroupBox();
            this.uiAtomSymbolsBlend = new Janus.Windows.EditControls.UICheckBox();
            this.uiAtomSymbolsDraw = new Janus.Windows.EditControls.UICheckBox();
            this.uiAtomFillModeList = new Janus.Windows.EditControls.UIComboBox();
            this.uiAtomLODControl = new NuGenSVisualLib.LODcontrol();
            this.label23 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.uiSchemesList = new Janus.Windows.ButtonBar.ButtonBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiGroupBox8 = new Janus.Windows.EditControls.UIGroupBox();
            this.schemePreviewControl = new NuGenSVisualLib.SchemePreviewControl();
            this.uiGroupBox7 = new Janus.Windows.EditControls.UIGroupBox();
            this.requirementGague2 = new NuGenSVisualLib.RequirementGague();
            this.requirementGague1 = new NuGenSVisualLib.RequirementGague();
            this.uiGroupBox3 = new Janus.Windows.EditControls.UIGroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.uiTabPage2 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiGroupBox14 = new Janus.Windows.EditControls.UIGroupBox();
            this.uiEfxPSReqs = new NuGenSVisualLib.RequirementGague();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.uiEfxVSReqs = new NuGenSVisualLib.RequirementGague();
            this.uiGroupBox13 = new Janus.Windows.EditControls.UIGroupBox();
            this.effectPreviewControl1 = new NuGenSVisualLib.EffectPreviewControl();
            this.label18 = new System.Windows.Forms.Label();
            this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.uiRemoveEffectBtn = new Janus.Windows.EditControls.UIButton();
            this.uiAddEffectBtn = new Janus.Windows.EditControls.UIButton();
            this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
            this.uiEffectPropGroup = new Janus.Windows.EditControls.UIGroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.uiEffectLOD = new NuGenSVisualLib.LODcontrol();
            this.label17 = new System.Windows.Forms.Label();
            this.uiEffectsReqList = new Janus.Windows.ButtonBar.ButtonBar();
            this.uiCurrentEffectsList = new Janus.Windows.ButtonBar.ButtonBar();
            this.label16 = new System.Windows.Forms.Label();
            this.uiEffectsList = new Janus.Windows.ButtonBar.ButtonBar();
            this.effectsListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiTabPage3 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiGroupBox11 = new Janus.Windows.EditControls.UIGroupBox();
            this.lightsListBox = new System.Windows.Forms.ListBox();
            this.uiLightingPropGroup = new Janus.Windows.EditControls.UIGroupBox();
            this.uiLightCastShadows = new Janus.Windows.EditControls.UICheckBox();
            this.uiLightEnabled = new Janus.Windows.EditControls.UICheckBox();
            this.uiLightingPropPanel = new System.Windows.Forms.Panel();
            this.label25 = new System.Windows.Forms.Label();
            this.uiColorButton1 = new Janus.Windows.EditControls.UIColorButton();
            this.uiGroupBox9 = new Janus.Windows.EditControls.UIGroupBox();
            this.lightPreviewControl2 = new NuGenSVisualLib.LightPreviewControl();
            this.lightingSchemeLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiLightingList = new Janus.Windows.ButtonBar.ButtonBar();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.uiColorButton2 = new Janus.Windows.EditControls.UIColorButton();
            this.uiTabPage4 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiGroupBox12 = new Janus.Windows.EditControls.UIGroupBox();
            this.uiComboBox2 = new Janus.Windows.EditControls.UIComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.uiComboBox1 = new Janus.Windows.EditControls.UIComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.uiComboBox3 = new Janus.Windows.EditControls.UIComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.uiTabPage5 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiShadingElementList = new System.Windows.Forms.ListBox();
            this.uiShadingSeriesGroup = new Janus.Windows.EditControls.UIGroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.uiShadingSeriesList = new System.Windows.Forms.ListBox();
            this.uiShadingElementsGroup = new Janus.Windows.EditControls.UIGroupBox();
            this.uiElementClrBtn = new Janus.Windows.EditControls.UIColorButton();
            this.label24 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.uiElementShadingList = new Janus.Windows.ButtonBar.ButtonBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.periodicTableControl1 = new NuGenSVisualLib.PeriodicTableControl();
            this.label26 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.multiColumnCombo1 = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.uiColorButton3 = new Janus.Windows.EditControls.UIColorButton();
            this.uiCheckBox2 = new Janus.Windows.EditControls.UICheckBox();
            this.uiCheckBox3 = new Janus.Windows.EditControls.UICheckBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBondsGSetGroup)).BeginInit();
            this.uiBondsGSetGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiAtomsGSetGroup)).BeginInit();
            this.uiAtomsGSetGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox6)).BeginInit();
            this.uiGroupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiSchemesList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.uiTab1.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox8)).BeginInit();
            this.uiGroupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox7)).BeginInit();
            this.uiGroupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).BeginInit();
            this.uiGroupBox3.SuspendLayout();
            this.uiTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox14)).BeginInit();
            this.uiGroupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox13)).BeginInit();
            this.uiGroupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiEffectPropGroup)).BeginInit();
            this.uiEffectPropGroup.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiEffectsReqList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCurrentEffectsList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiEffectsList)).BeginInit();
            this.effectsListMenuStrip.SuspendLayout();
            this.uiTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox11)).BeginInit();
            this.uiGroupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiLightingPropGroup)).BeginInit();
            this.uiLightingPropGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox9)).BeginInit();
            this.uiGroupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiLightingList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            this.uiTabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox12)).BeginInit();
            this.uiGroupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox13.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.uiTabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiShadingSeriesGroup)).BeginInit();
            this.uiShadingSeriesGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiShadingElementsGroup)).BeginInit();
            this.uiShadingElementsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiElementShadingList)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.multiColumnCombo1)).BeginInit();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.uiBondsGSetGroup);
            this.panel2.Controls.Add(this.uiAtomsGSetGroup);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(409, 329);
            this.panel2.TabIndex = 1;
            // 
            // uiBondsGSetGroup
            // 
            this.uiBondsGSetGroup.Controls.Add(this.uiBondDDraw);
            this.uiBondsGSetGroup.Controls.Add(this.uiBondEndTypeList);
            this.uiBondsGSetGroup.Controls.Add(this.uiBondSpacingList);
            this.uiBondsGSetGroup.Controls.Add(this.uiBondLODControl);
            this.uiBondsGSetGroup.Controls.Add(this.label1);
            this.uiBondsGSetGroup.Controls.Add(this.label2);
            this.uiBondsGSetGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiBondsGSetGroup.Location = new System.Drawing.Point(0, 93);
            this.uiBondsGSetGroup.Name = "uiBondsGSetGroup";
            this.uiBondsGSetGroup.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiBondsGSetGroup.Size = new System.Drawing.Size(409, 87);
            this.uiBondsGSetGroup.TabIndex = 4;
            this.uiBondsGSetGroup.Text = "Bond (General)";
            this.uiBondsGSetGroup.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiBondDDraw
            // 
            this.uiBondDDraw.Location = new System.Drawing.Point(277, 53);
            this.uiBondDDraw.Name = "uiBondDDraw";
            this.uiBondDDraw.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiBondDDraw.Size = new System.Drawing.Size(104, 23);
            this.uiBondDDraw.TabIndex = 8;
            this.uiBondDDraw.Text = "Don\'t Draw";
            this.uiBondDDraw.VisualStyleManager = this.visualStyleManager1;
            this.uiBondDDraw.CheckedChanged += new System.EventHandler(this.uiBondDDraw_CheckedChanged);
            // 
            // visualStyleManager1
            // 
            janusColorScheme1.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            janusColorScheme1.Name = "Scheme1";
            janusColorScheme1.Office2007ColorScheme = Janus.Windows.Common.Office2007ColorScheme.Black;
            janusColorScheme1.Office2007CustomColor = System.Drawing.Color.Empty;
            janusColorScheme1.VisualStyle = Janus.Windows.Common.VisualStyle.Office2007;
            this.visualStyleManager1.ColorSchemes.Add(janusColorScheme1);
            this.visualStyleManager1.DefaultColorScheme = "Scheme1";
            // 
            // uiBondEndTypeList
            // 
            this.uiBondEndTypeList.AutoComplete = false;
            this.uiBondEndTypeList.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            uiComboBoxItem1.FormatStyle.Alpha = 0;
            uiComboBoxItem1.IsSeparator = false;
            uiComboBoxItem1.Text = "Open";
            uiComboBoxItem2.FormatStyle.Alpha = 0;
            uiComboBoxItem2.IsSeparator = false;
            uiComboBoxItem2.Text = "Closed";
            uiComboBoxItem3.FormatStyle.Alpha = 0;
            uiComboBoxItem3.IsSeparator = false;
            uiComboBoxItem3.Text = "Point";
            uiComboBoxItem4.FormatStyle.Alpha = 0;
            uiComboBoxItem4.IsSeparator = false;
            uiComboBoxItem4.Text = "Rounded";
            this.uiBondEndTypeList.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem1,
            uiComboBoxItem2,
            uiComboBoxItem3,
            uiComboBoxItem4});
            this.uiBondEndTypeList.Location = new System.Drawing.Point(260, 19);
            this.uiBondEndTypeList.Name = "uiBondEndTypeList";
            this.uiBondEndTypeList.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiBondEndTypeList.Size = new System.Drawing.Size(128, 22);
            this.uiBondEndTypeList.TabIndex = 7;
            this.uiBondEndTypeList.VisualStyleManager = this.visualStyleManager1;
            this.uiBondEndTypeList.SelectedIndexChanged += new System.EventHandler(this.uiBondEndTypeList_SelectedIndexChanged);
            // 
            // uiBondSpacingList
            // 
            this.uiBondSpacingList.AutoComplete = false;
            this.uiBondSpacingList.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            uiComboBoxItem5.FormatStyle.Alpha = 0;
            uiComboBoxItem5.IsSeparator = false;
            uiComboBoxItem5.Text = "AtoB";
            uiComboBoxItem6.FormatStyle.Alpha = 0;
            uiComboBoxItem6.IsSeparator = false;
            uiComboBoxItem6.Text = "Center";
            this.uiBondSpacingList.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem5,
            uiComboBoxItem6});
            this.uiBondSpacingList.Location = new System.Drawing.Point(61, 19);
            this.uiBondSpacingList.Name = "uiBondSpacingList";
            this.uiBondSpacingList.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiBondSpacingList.Size = new System.Drawing.Size(120, 22);
            this.uiBondSpacingList.TabIndex = 6;
            this.uiBondSpacingList.VisualStyleManager = this.visualStyleManager1;
            this.uiBondSpacingList.SelectedIndexChanged += new System.EventHandler(this.uiBondSpacingList_SelectedIndexChanged_1);
            // 
            // uiBondLODControl
            // 
            this.uiBondLODControl.BackColor = System.Drawing.Color.GhostWhite;
            this.uiBondLODControl.Location = new System.Drawing.Point(9, 47);
            this.uiBondLODControl.MaximumSize = new System.Drawing.Size(0, 29);
            this.uiBondLODControl.MinimumSize = new System.Drawing.Size(246, 29);
            this.uiBondLODControl.Name = "uiBondLODControl";
            this.uiBondLODControl.Size = new System.Drawing.Size(246, 29);
            this.uiBondLODControl.TabIndex = 5;
            this.uiBondLODControl.OnLODValueChanged += new System.EventHandler(this.uiBondLODControl_OnLODValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Spacing:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(198, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "End Type:";
            // 
            // uiAtomsGSetGroup
            // 
            this.uiAtomsGSetGroup.Controls.Add(this.uiAtomDDraw);
            this.uiAtomsGSetGroup.Controls.Add(this.uiGroupBox6);
            this.uiAtomsGSetGroup.Controls.Add(this.uiAtomFillModeList);
            this.uiAtomsGSetGroup.Controls.Add(this.uiAtomLODControl);
            this.uiAtomsGSetGroup.Controls.Add(this.label23);
            this.uiAtomsGSetGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiAtomsGSetGroup.Location = new System.Drawing.Point(0, 0);
            this.uiAtomsGSetGroup.Name = "uiAtomsGSetGroup";
            this.uiAtomsGSetGroup.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiAtomsGSetGroup.Size = new System.Drawing.Size(409, 93);
            this.uiAtomsGSetGroup.TabIndex = 3;
            this.uiAtomsGSetGroup.Text = "Atom (General)";
            this.uiAtomsGSetGroup.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiAtomDDraw
            // 
            this.uiAtomDDraw.Location = new System.Drawing.Point(201, 17);
            this.uiAtomDDraw.Name = "uiAtomDDraw";
            this.uiAtomDDraw.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiAtomDDraw.Size = new System.Drawing.Size(77, 23);
            this.uiAtomDDraw.TabIndex = 12;
            this.uiAtomDDraw.Text = "Don\'t Draw";
            this.uiAtomDDraw.VisualStyleManager = this.visualStyleManager1;
            this.uiAtomDDraw.CheckedChanged += new System.EventHandler(this.uiAtomDDraw_CheckedChanged);
            // 
            // uiGroupBox6
            // 
            this.uiGroupBox6.Controls.Add(this.uiAtomSymbolsBlend);
            this.uiGroupBox6.Controls.Add(this.uiAtomSymbolsDraw);
            this.uiGroupBox6.Location = new System.Drawing.Point(303, 15);
            this.uiGroupBox6.Name = "uiGroupBox6";
            this.uiGroupBox6.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox6.Size = new System.Drawing.Size(85, 62);
            this.uiGroupBox6.TabIndex = 11;
            this.uiGroupBox6.Text = "Symbols";
            this.uiGroupBox6.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiAtomSymbolsBlend
            // 
            this.uiAtomSymbolsBlend.Location = new System.Drawing.Point(15, 39);
            this.uiAtomSymbolsBlend.Name = "uiAtomSymbolsBlend";
            this.uiAtomSymbolsBlend.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiAtomSymbolsBlend.Size = new System.Drawing.Size(54, 23);
            this.uiAtomSymbolsBlend.TabIndex = 9;
            this.uiAtomSymbolsBlend.Text = "Blend";
            this.uiAtomSymbolsBlend.VisualStyleManager = this.visualStyleManager1;
            this.uiAtomSymbolsBlend.CheckedChanged += new System.EventHandler(this.uiAtomSymbolsBlend_CheckedChanged);
            // 
            // uiAtomSymbolsDraw
            // 
            this.uiAtomSymbolsDraw.Location = new System.Drawing.Point(15, 19);
            this.uiAtomSymbolsDraw.Name = "uiAtomSymbolsDraw";
            this.uiAtomSymbolsDraw.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiAtomSymbolsDraw.Size = new System.Drawing.Size(53, 23);
            this.uiAtomSymbolsDraw.TabIndex = 8;
            this.uiAtomSymbolsDraw.Text = "Draw";
            this.uiAtomSymbolsDraw.VisualStyleManager = this.visualStyleManager1;
            this.uiAtomSymbolsDraw.CheckedChanged += new System.EventHandler(this.uiAtomSymbolsDraw_CheckedChanged);
            // 
            // uiAtomFillModeList
            // 
            this.uiAtomFillModeList.AutoComplete = false;
            this.uiAtomFillModeList.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            uiComboBoxItem7.FormatStyle.Alpha = 0;
            uiComboBoxItem7.IsSeparator = false;
            uiComboBoxItem7.Text = "Point";
            uiComboBoxItem8.FormatStyle.Alpha = 0;
            uiComboBoxItem8.IsSeparator = false;
            uiComboBoxItem8.Text = "Wireframe";
            uiComboBoxItem9.FormatStyle.Alpha = 0;
            uiComboBoxItem9.IsSeparator = false;
            uiComboBoxItem9.Text = "Solid";
            this.uiAtomFillModeList.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem7,
            uiComboBoxItem8,
            uiComboBoxItem9});
            this.uiAtomFillModeList.Location = new System.Drawing.Point(67, 19);
            this.uiAtomFillModeList.Name = "uiAtomFillModeList";
            this.uiAtomFillModeList.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiAtomFillModeList.Size = new System.Drawing.Size(114, 22);
            this.uiAtomFillModeList.TabIndex = 7;
            this.uiAtomFillModeList.VisualStyleManager = this.visualStyleManager1;
            this.uiAtomFillModeList.SelectedIndexChanged += new System.EventHandler(this.uiAtomFillModeList_SelectedIndexChanged);
            // 
            // uiAtomLODControl
            // 
            this.uiAtomLODControl.BackColor = System.Drawing.Color.GhostWhite;
            this.uiAtomLODControl.Location = new System.Drawing.Point(9, 48);
            this.uiAtomLODControl.MaximumSize = new System.Drawing.Size(0, 29);
            this.uiAtomLODControl.MinimumSize = new System.Drawing.Size(246, 29);
            this.uiAtomLODControl.Name = "uiAtomLODControl";
            this.uiAtomLODControl.Size = new System.Drawing.Size(246, 29);
            this.uiAtomLODControl.TabIndex = 6;
            this.uiAtomLODControl.OnLODValueChanged += new System.EventHandler(this.uiAtomLODControl_OnLODValueChanged);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(9, 22);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(58, 13);
            this.label23.TabIndex = 1;
            this.label23.Text = "Fill Mode:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Other:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Vertex Shader:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Pixel Shader:";
            // 
            // uiSchemesList
            // 
            this.uiSchemesList.BackColor = System.Drawing.Color.White;
            buttonBarGroup1.Key = "Group1";
            buttonBarGroup1.Text = "Schemes";
            buttonBarGroup1.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            buttonBarGroup2.Key = "Group2";
            buttonBarGroup2.Text = "Presets";
            buttonBarGroup2.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            this.uiSchemesList.Groups.AddRange(new Janus.Windows.ButtonBar.ButtonBarGroup[] {
            buttonBarGroup1,
            buttonBarGroup2});
            this.uiSchemesList.ItemAppearance = Janus.Windows.ButtonBar.ItemAppearance.Flat;
            this.uiSchemesList.Location = new System.Drawing.Point(3, 4);
            this.uiSchemesList.Name = "uiSchemesList";
            this.uiSchemesList.Office2007ColorScheme = Janus.Windows.ButtonBar.Office2007ColorScheme.Black;
            this.uiSchemesList.Orientation = Janus.Windows.ButtonBar.ButtonBarOrientation.Horizontal;
            this.uiSchemesList.SelectionArea = Janus.Windows.ButtonBar.SelectionArea.FullItem;
            this.uiSchemesList.ShadowOnHover = true;
            this.uiSchemesList.Size = new System.Drawing.Size(628, 80);
            this.uiSchemesList.TabIndex = 0;
            this.uiSchemesList.Text = "buttonBar1";
            this.uiSchemesList.ThemedAreas = ((Janus.Windows.ButtonBar.ThemedArea)((((Janus.Windows.ButtonBar.ThemedArea.Border | Janus.Windows.ButtonBar.ThemedArea.Groups)
                        | Janus.Windows.ButtonBar.ThemedArea.Items)
                        | Janus.Windows.ButtonBar.ThemedArea.ScrollButton)));
            this.uiSchemesList.VisualStyle = Janus.Windows.ButtonBar.VisualStyle.Office2007;
            this.uiSchemesList.ItemSelected += new System.EventHandler(this.buttonBar1_ItemSelected);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(575, 579);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(494, 579);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Apply";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(301, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(335, 19);
            this.label13.TabIndex = 6;
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // uiTab1
            // 
            this.uiTab1.Location = new System.Drawing.Point(12, 12);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiTab1.Size = new System.Drawing.Size(638, 492);
            this.uiTab1.TabDisplay = Janus.Windows.UI.Tab.TabDisplay.Text;
            this.uiTab1.TabIndex = 7;
            this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage2,
            this.uiTabPage3,
            this.uiTabPage4,
            this.uiTabPage5});
            this.uiTab1.TabStripAlignment = Janus.Windows.UI.Tab.TabStripAlignment.Bottom;
            this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
            this.uiTab1.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.uiGroupBox8);
            this.uiTabPage1.Controls.Add(this.uiGroupBox7);
            this.uiTabPage1.Controls.Add(this.uiGroupBox3);
            this.uiTabPage1.Controls.Add(this.label22);
            this.uiTabPage1.Controls.Add(this.label13);
            this.uiTabPage1.Controls.Add(this.uiSchemesList);
            this.uiTabPage1.Location = new System.Drawing.Point(1, 1);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(636, 470);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "Scheme";
            // 
            // uiGroupBox8
            // 
            this.uiGroupBox8.Controls.Add(this.schemePreviewControl);
            this.uiGroupBox8.Location = new System.Drawing.Point(3, 90);
            this.uiGroupBox8.Name = "uiGroupBox8";
            this.uiGroupBox8.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox8.Size = new System.Drawing.Size(209, 213);
            this.uiGroupBox8.TabIndex = 7;
            this.uiGroupBox8.Text = "Preview";
            this.uiGroupBox8.VisualStyleManager = this.visualStyleManager1;
            // 
            // schemePreviewControl
            // 
            this.schemePreviewControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.schemePreviewControl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.schemePreviewControl.Location = new System.Drawing.Point(7, 15);
            this.schemePreviewControl.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.schemePreviewControl.Name = "schemePreviewControl";
            this.schemePreviewControl.OutSettings = null;
            this.schemePreviewControl.Size = new System.Drawing.Size(195, 188);
            this.schemePreviewControl.TabIndex = 6;
            // 
            // uiGroupBox7
            // 
            this.uiGroupBox7.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox7.Controls.Add(this.requirementGague2);
            this.uiGroupBox7.Controls.Add(this.label7);
            this.uiGroupBox7.Controls.Add(this.requirementGague1);
            this.uiGroupBox7.Controls.Add(this.label3);
            this.uiGroupBox7.Controls.Add(this.label8);
            this.uiGroupBox7.Location = new System.Drawing.Point(3, 305);
            this.uiGroupBox7.Name = "uiGroupBox7";
            this.uiGroupBox7.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox7.Size = new System.Drawing.Size(209, 146);
            this.uiGroupBox7.TabIndex = 4;
            this.uiGroupBox7.Text = "Requirements";
            this.uiGroupBox7.VisualStyleManager = this.visualStyleManager1;
            // 
            // requirementGague2
            // 
            this.requirementGague2.ActualValue = 0;
            this.requirementGague2.DrawTicks = false;
            this.requirementGague2.Location = new System.Drawing.Point(89, 41);
            this.requirementGague2.MaxReqValue = 0;
            this.requirementGague2.MinimumSize = new System.Drawing.Size(0, 20);
            this.requirementGague2.MinReqValue = 0;
            this.requirementGague2.Name = "requirementGague2";
            this.requirementGague2.ShowValues = false;
            this.requirementGague2.Size = new System.Drawing.Size(114, 22);
            this.requirementGague2.TabIndex = 15;
            // 
            // requirementGague1
            // 
            this.requirementGague1.ActualValue = 0;
            this.requirementGague1.DrawTicks = false;
            this.requirementGague1.Location = new System.Drawing.Point(89, 19);
            this.requirementGague1.MaxReqValue = 0;
            this.requirementGague1.MinimumSize = new System.Drawing.Size(0, 20);
            this.requirementGague1.MinReqValue = 0;
            this.requirementGague1.Name = "requirementGague1";
            this.requirementGague1.ShowValues = false;
            this.requirementGague1.Size = new System.Drawing.Size(114, 22);
            this.requirementGague1.TabIndex = 14;
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox3.Controls.Add(this.panel2);
            this.uiGroupBox3.Location = new System.Drawing.Point(218, 101);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox3.Size = new System.Drawing.Size(415, 350);
            this.uiGroupBox3.TabIndex = 8;
            this.uiGroupBox3.Text = "Settings";
            this.uiGroupBox3.VisualStyleManager = this.visualStyleManager1;
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.ForeColor = System.Drawing.Color.DimGray;
            this.label22.Location = new System.Drawing.Point(3, 454);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(628, 16);
            this.label22.TabIndex = 7;
            this.label22.Text = "Schemes define how molecules are represented";
            // 
            // uiTabPage2
            // 
            this.uiTabPage2.Controls.Add(this.uiGroupBox14);
            this.uiTabPage2.Controls.Add(this.uiGroupBox13);
            this.uiTabPage2.Controls.Add(this.label18);
            this.uiTabPage2.Controls.Add(this.uiGroupBox2);
            this.uiTabPage2.Controls.Add(this.uiRemoveEffectBtn);
            this.uiTabPage2.Controls.Add(this.uiAddEffectBtn);
            this.uiTabPage2.Controls.Add(this.uiCheckBox1);
            this.uiTabPage2.Controls.Add(this.uiEffectPropGroup);
            this.uiTabPage2.Controls.Add(this.label17);
            this.uiTabPage2.Controls.Add(this.uiEffectsReqList);
            this.uiTabPage2.Controls.Add(this.uiCurrentEffectsList);
            this.uiTabPage2.Controls.Add(this.label16);
            this.uiTabPage2.Controls.Add(this.uiEffectsList);
            this.uiTabPage2.Location = new System.Drawing.Point(1, 1);
            this.uiTabPage2.Name = "uiTabPage2";
            this.uiTabPage2.Size = new System.Drawing.Size(636, 470);
            this.uiTabPage2.TabStop = true;
            this.uiTabPage2.Text = "Effects";
            // 
            // uiGroupBox14
            // 
            this.uiGroupBox14.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox14.Controls.Add(this.uiEfxPSReqs);
            this.uiGroupBox14.Controls.Add(this.label14);
            this.uiGroupBox14.Controls.Add(this.label15);
            this.uiGroupBox14.Controls.Add(this.label12);
            this.uiGroupBox14.Controls.Add(this.uiEfxVSReqs);
            this.uiGroupBox14.Location = new System.Drawing.Point(3, 305);
            this.uiGroupBox14.Name = "uiGroupBox14";
            this.uiGroupBox14.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox14.Size = new System.Drawing.Size(209, 146);
            this.uiGroupBox14.TabIndex = 25;
            this.uiGroupBox14.Text = "Requirements of Selected";
            this.uiGroupBox14.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiEfxPSReqs
            // 
            this.uiEfxPSReqs.ActualValue = 0;
            this.uiEfxPSReqs.DrawTicks = false;
            this.uiEfxPSReqs.Location = new System.Drawing.Point(89, 41);
            this.uiEfxPSReqs.MaxReqValue = 0;
            this.uiEfxPSReqs.MinimumSize = new System.Drawing.Size(0, 20);
            this.uiEfxPSReqs.MinReqValue = 0;
            this.uiEfxPSReqs.Name = "uiEfxPSReqs";
            this.uiEfxPSReqs.ShowValues = false;
            this.uiEfxPSReqs.Size = new System.Drawing.Size(114, 22);
            this.uiEfxPSReqs.TabIndex = 15;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 28);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(81, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "Vertex Shader:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 50);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(72, 13);
            this.label15.TabIndex = 4;
            this.label15.Text = "Pixel Shader:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 73);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Other:";
            // 
            // uiEfxVSReqs
            // 
            this.uiEfxVSReqs.ActualValue = 0;
            this.uiEfxVSReqs.DrawTicks = false;
            this.uiEfxVSReqs.Location = new System.Drawing.Point(89, 19);
            this.uiEfxVSReqs.MaxReqValue = 0;
            this.uiEfxVSReqs.MinimumSize = new System.Drawing.Size(0, 20);
            this.uiEfxVSReqs.MinReqValue = 0;
            this.uiEfxVSReqs.Name = "uiEfxVSReqs";
            this.uiEfxVSReqs.ShowValues = false;
            this.uiEfxVSReqs.Size = new System.Drawing.Size(114, 22);
            this.uiEfxVSReqs.TabIndex = 14;
            // 
            // uiGroupBox13
            // 
            this.uiGroupBox13.Controls.Add(this.effectPreviewControl1);
            this.uiGroupBox13.Location = new System.Drawing.Point(3, 90);
            this.uiGroupBox13.Name = "uiGroupBox13";
            this.uiGroupBox13.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox13.Size = new System.Drawing.Size(209, 213);
            this.uiGroupBox13.TabIndex = 24;
            this.uiGroupBox13.Text = "Preview";
            this.uiGroupBox13.VisualStyleManager = this.visualStyleManager1;
            // 
            // effectPreviewControl1
            // 
            this.effectPreviewControl1.Location = new System.Drawing.Point(7, 19);
            this.effectPreviewControl1.Name = "effectPreviewControl1";
            this.effectPreviewControl1.OutSettings = null;
            this.effectPreviewControl1.Size = new System.Drawing.Size(195, 188);
            this.effectPreviewControl1.TabIndex = 0;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(295, 177);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(335, 19);
            this.label18.TabIndex = 23;
            this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox2.Controls.Add(this.pictureBox8);
            this.uiGroupBox2.Location = new System.Drawing.Point(218, 192);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox2.Size = new System.Drawing.Size(412, 60);
            this.uiGroupBox2.TabIndex = 22;
            this.uiGroupBox2.Text = "Compatibility";
            this.uiGroupBox2.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiGroupBox2.VisualStyleManager = this.visualStyleManager1;
            // 
            // pictureBox8
            // 
            this.pictureBox8.Location = new System.Drawing.Point(6, 18);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(32, 32);
            this.pictureBox8.TabIndex = 0;
            this.pictureBox8.TabStop = false;
            // 
            // uiRemoveEffectBtn
            // 
            this.uiRemoveEffectBtn.Location = new System.Drawing.Point(517, 428);
            this.uiRemoveEffectBtn.Name = "uiRemoveEffectBtn";
            this.uiRemoveEffectBtn.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiRemoveEffectBtn.Size = new System.Drawing.Size(101, 23);
            this.uiRemoveEffectBtn.TabIndex = 21;
            this.uiRemoveEffectBtn.Text = "Remove Effect";
            this.uiRemoveEffectBtn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiRemoveEffectBtn.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiAddEffectBtn
            // 
            this.uiAddEffectBtn.Location = new System.Drawing.Point(410, 428);
            this.uiAddEffectBtn.Name = "uiAddEffectBtn";
            this.uiAddEffectBtn.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiAddEffectBtn.Size = new System.Drawing.Size(101, 23);
            this.uiAddEffectBtn.TabIndex = 20;
            this.uiAddEffectBtn.Text = "Add Effect";
            this.uiAddEffectBtn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiAddEffectBtn.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiCheckBox1
            // 
            this.uiCheckBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.uiCheckBox1.Location = new System.Drawing.Point(218, 428);
            this.uiCheckBox1.Name = "uiCheckBox1";
            this.uiCheckBox1.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiCheckBox1.Size = new System.Drawing.Size(98, 23);
            this.uiCheckBox1.TabIndex = 19;
            this.uiCheckBox1.Text = "Preview Result";
            this.uiCheckBox1.TextAlignment = Janus.Windows.EditControls.TextAlignment.Center;
            this.uiCheckBox1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiCheckBox1.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiEffectPropGroup
            // 
            this.uiEffectPropGroup.BackColor = System.Drawing.Color.Transparent;
            this.uiEffectPropGroup.Controls.Add(this.panel4);
            this.uiEffectPropGroup.Enabled = false;
            this.uiEffectPropGroup.Location = new System.Drawing.Point(218, 252);
            this.uiEffectPropGroup.Name = "uiEffectPropGroup";
            this.uiEffectPropGroup.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiEffectPropGroup.Size = new System.Drawing.Size(412, 170);
            this.uiEffectPropGroup.TabIndex = 18;
            this.uiEffectPropGroup.Text = "Properties";
            this.uiEffectPropGroup.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiEffectPropGroup.VisualStyleManager = this.visualStyleManager1;
            // 
            // panel4
            // 
            this.panel4.AutoScroll = true;
            this.panel4.Controls.Add(this.uiEffectLOD);
            this.panel4.Location = new System.Drawing.Point(6, 19);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(394, 145);
            this.panel4.TabIndex = 0;
            // 
            // uiEffectLOD
            // 
            this.uiEffectLOD.BackColor = System.Drawing.Color.GhostWhite;
            this.uiEffectLOD.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiEffectLOD.Location = new System.Drawing.Point(0, 0);
            this.uiEffectLOD.MaximumSize = new System.Drawing.Size(0, 29);
            this.uiEffectLOD.MinimumSize = new System.Drawing.Size(246, 29);
            this.uiEffectLOD.Name = "uiEffectLOD";
            this.uiEffectLOD.Size = new System.Drawing.Size(394, 29);
            this.uiEffectLOD.TabIndex = 0;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(443, 94);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(15, 80);
            this.label17.TabIndex = 16;
            this.label17.Text = "+";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiEffectsReqList
            // 
            this.uiEffectsReqList.BackColor = System.Drawing.Color.White;
            buttonBarGroup3.Key = "Group1";
            buttonBarGroup3.Text = "Required";
            buttonBarGroup3.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            this.uiEffectsReqList.Groups.AddRange(new Janus.Windows.ButtonBar.ButtonBarGroup[] {
            buttonBarGroup3});
            this.uiEffectsReqList.ItemAppearance = Janus.Windows.ButtonBar.ItemAppearance.Flat;
            this.uiEffectsReqList.Location = new System.Drawing.Point(458, 94);
            this.uiEffectsReqList.Name = "uiEffectsReqList";
            this.uiEffectsReqList.Office2007ColorScheme = Janus.Windows.ButtonBar.Office2007ColorScheme.Black;
            this.uiEffectsReqList.Orientation = Janus.Windows.ButtonBar.ButtonBarOrientation.Horizontal;
            this.uiEffectsReqList.SelectionArea = Janus.Windows.ButtonBar.SelectionArea.FullItem;
            this.uiEffectsReqList.ShadowOnHover = true;
            this.uiEffectsReqList.Size = new System.Drawing.Size(172, 80);
            this.uiEffectsReqList.TabIndex = 15;
            this.uiEffectsReqList.Text = "buttonBar5";
            this.uiEffectsReqList.ThemedAreas = ((Janus.Windows.ButtonBar.ThemedArea)((((Janus.Windows.ButtonBar.ThemedArea.Border | Janus.Windows.ButtonBar.ThemedArea.Groups)
                        | Janus.Windows.ButtonBar.ThemedArea.Items)
                        | Janus.Windows.ButtonBar.ThemedArea.ScrollButton)));
            this.uiEffectsReqList.VisualStyle = Janus.Windows.ButtonBar.VisualStyle.Office2007;
            // 
            // uiCurrentEffectsList
            // 
            this.uiCurrentEffectsList.BackColor = System.Drawing.Color.White;
            buttonBarGroup4.Key = "Group1";
            buttonBarGroup4.Text = "Current";
            buttonBarGroup4.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            this.uiCurrentEffectsList.Groups.AddRange(new Janus.Windows.ButtonBar.ButtonBarGroup[] {
            buttonBarGroup4});
            this.uiCurrentEffectsList.ItemAppearance = Janus.Windows.ButtonBar.ItemAppearance.Flat;
            this.uiCurrentEffectsList.Location = new System.Drawing.Point(218, 94);
            this.uiCurrentEffectsList.Name = "uiCurrentEffectsList";
            this.uiCurrentEffectsList.Office2007ColorScheme = Janus.Windows.ButtonBar.Office2007ColorScheme.Black;
            this.uiCurrentEffectsList.Orientation = Janus.Windows.ButtonBar.ButtonBarOrientation.Horizontal;
            this.uiCurrentEffectsList.SelectionArea = Janus.Windows.ButtonBar.SelectionArea.FullItem;
            this.uiCurrentEffectsList.ShadowOnHover = true;
            this.uiCurrentEffectsList.Size = new System.Drawing.Size(225, 80);
            this.uiCurrentEffectsList.TabIndex = 14;
            this.uiCurrentEffectsList.Text = "buttonBar4";
            this.uiCurrentEffectsList.ThemedAreas = ((Janus.Windows.ButtonBar.ThemedArea)((((Janus.Windows.ButtonBar.ThemedArea.Border | Janus.Windows.ButtonBar.ThemedArea.Groups)
                        | Janus.Windows.ButtonBar.ThemedArea.Items)
                        | Janus.Windows.ButtonBar.ThemedArea.ScrollButton)));
            this.uiCurrentEffectsList.VisualStyle = Janus.Windows.ButtonBar.VisualStyle.Office2007;
            this.uiCurrentEffectsList.ItemSelected += new System.EventHandler(this.uiCurrentEffectsList_ItemSelected);
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.ForeColor = System.Drawing.Color.DimGray;
            this.label16.Location = new System.Drawing.Point(3, 454);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(628, 16);
            this.label16.TabIndex = 12;
            this.label16.Text = "Effects define how the scene is shaded";
            // 
            // uiEffectsList
            // 
            this.uiEffectsList.BackColor = System.Drawing.Color.White;
            this.uiEffectsList.ContextMenuStrip = this.effectsListMenuStrip;
            buttonBarGroup5.Key = "Group1";
            buttonBarGroup5.Text = "Effects";
            buttonBarGroup5.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            buttonBarGroup6.Key = "Group2";
            buttonBarGroup6.Text = "Presets";
            buttonBarGroup6.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            buttonBarGroup7.Key = "Group3";
            buttonBarGroup7.Text = "Custom";
            buttonBarGroup7.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            this.uiEffectsList.Groups.AddRange(new Janus.Windows.ButtonBar.ButtonBarGroup[] {
            buttonBarGroup5,
            buttonBarGroup6,
            buttonBarGroup7});
            this.uiEffectsList.ItemAppearance = Janus.Windows.ButtonBar.ItemAppearance.Flat;
            this.uiEffectsList.Location = new System.Drawing.Point(3, 4);
            this.uiEffectsList.Name = "uiEffectsList";
            this.uiEffectsList.Office2007ColorScheme = Janus.Windows.ButtonBar.Office2007ColorScheme.Black;
            this.uiEffectsList.Orientation = Janus.Windows.ButtonBar.ButtonBarOrientation.Horizontal;
            this.uiEffectsList.SelectionArea = Janus.Windows.ButtonBar.SelectionArea.FullItem;
            this.uiEffectsList.ShadowOnHover = true;
            this.uiEffectsList.Size = new System.Drawing.Size(628, 80);
            this.uiEffectsList.TabIndex = 1;
            this.uiEffectsList.ThemedAreas = ((Janus.Windows.ButtonBar.ThemedArea)((((Janus.Windows.ButtonBar.ThemedArea.Border | Janus.Windows.ButtonBar.ThemedArea.Groups)
                        | Janus.Windows.ButtonBar.ThemedArea.Items)
                        | Janus.Windows.ButtonBar.ThemedArea.ScrollButton)));
            this.uiEffectsList.VisualStyle = Janus.Windows.ButtonBar.VisualStyle.Office2007;
            this.uiEffectsList.ItemSelected += new System.EventHandler(this.uiEffectsList_ItemSelected);
            // 
            // effectsListMenuStrip
            // 
            this.effectsListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.toolStripSeparator1,
            this.detailsToolStripMenuItem});
            this.effectsListMenuStrip.Name = "effectsListMenuStrip";
            this.effectsListMenuStrip.Size = new System.Drawing.Size(110, 54);
            this.effectsListMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.effectsListMenuStrip_Opening);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(106, 6);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.detailsToolStripMenuItem.Text = "Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            // 
            // uiTabPage3
            // 
            this.uiTabPage3.Controls.Add(this.uiGroupBox11);
            this.uiTabPage3.Controls.Add(this.uiLightingPropGroup);
            this.uiTabPage3.Controls.Add(this.uiGroupBox9);
            this.uiTabPage3.Controls.Add(this.lightingSchemeLabel);
            this.uiTabPage3.Controls.Add(this.label4);
            this.uiTabPage3.Controls.Add(this.uiButton2);
            this.uiTabPage3.Controls.Add(this.uiButton1);
            this.uiTabPage3.Controls.Add(this.uiLightingList);
            this.uiTabPage3.Controls.Add(this.uiGroupBox1);
            this.uiTabPage3.Location = new System.Drawing.Point(1, 1);
            this.uiTabPage3.Name = "uiTabPage3";
            this.uiTabPage3.Size = new System.Drawing.Size(636, 470);
            this.uiTabPage3.TabStop = true;
            this.uiTabPage3.Text = "Lighting";
            // 
            // uiGroupBox11
            // 
            this.uiGroupBox11.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox11.Controls.Add(this.lightsListBox);
            this.uiGroupBox11.Location = new System.Drawing.Point(309, 103);
            this.uiGroupBox11.Name = "uiGroupBox11";
            this.uiGroupBox11.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox11.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.uiGroupBox11.Size = new System.Drawing.Size(319, 103);
            this.uiGroupBox11.TabIndex = 22;
            this.uiGroupBox11.Text = "Lights";
            this.uiGroupBox11.VisualStyleManager = this.visualStyleManager1;
            // 
            // lightsListBox
            // 
            this.lightsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightsListBox.FormattingEnabled = true;
            this.lightsListBox.Location = new System.Drawing.Point(6, 18);
            this.lightsListBox.Name = "lightsListBox";
            this.lightsListBox.Size = new System.Drawing.Size(307, 82);
            this.lightsListBox.TabIndex = 0;
            this.lightsListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // uiLightingPropGroup
            // 
            this.uiLightingPropGroup.BackColor = System.Drawing.Color.Transparent;
            this.uiLightingPropGroup.Controls.Add(this.uiLightCastShadows);
            this.uiLightingPropGroup.Controls.Add(this.uiLightEnabled);
            this.uiLightingPropGroup.Controls.Add(this.uiLightingPropPanel);
            this.uiLightingPropGroup.Controls.Add(this.label25);
            this.uiLightingPropGroup.Controls.Add(this.uiColorButton1);
            this.uiLightingPropGroup.Location = new System.Drawing.Point(309, 212);
            this.uiLightingPropGroup.Name = "uiLightingPropGroup";
            this.uiLightingPropGroup.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiLightingPropGroup.Size = new System.Drawing.Size(318, 177);
            this.uiLightingPropGroup.TabIndex = 21;
            this.uiLightingPropGroup.Text = "Properties";
            this.uiLightingPropGroup.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiLightCastShadows
            // 
            this.uiLightCastShadows.Location = new System.Drawing.Point(21, 43);
            this.uiLightCastShadows.Name = "uiLightCastShadows";
            this.uiLightCastShadows.Size = new System.Drawing.Size(104, 23);
            this.uiLightCastShadows.TabIndex = 10;
            this.uiLightCastShadows.Text = "Cast Shadows";
            this.uiLightCastShadows.CheckedChanged += new System.EventHandler(this.uiLightCastShadows_CheckedChanged);
            // 
            // uiLightEnabled
            // 
            this.uiLightEnabled.Location = new System.Drawing.Point(21, 22);
            this.uiLightEnabled.Name = "uiLightEnabled";
            this.uiLightEnabled.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiLightEnabled.Size = new System.Drawing.Size(61, 23);
            this.uiLightEnabled.TabIndex = 9;
            this.uiLightEnabled.Text = "Enabled";
            this.uiLightEnabled.VisualStyleManager = this.visualStyleManager1;
            this.uiLightEnabled.CheckedChanged += new System.EventHandler(this.uiLightEnabled_CheckedChanged);
            // 
            // uiLightingPropPanel
            // 
            this.uiLightingPropPanel.Location = new System.Drawing.Point(6, 72);
            this.uiLightingPropPanel.Name = "uiLightingPropPanel";
            this.uiLightingPropPanel.Size = new System.Drawing.Size(306, 99);
            this.uiLightingPropPanel.TabIndex = 8;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(137, 27);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(38, 13);
            this.label25.TabIndex = 5;
            this.label25.Text = "Color:";
            // 
            // uiColorButton1
            // 
            // 
            // 
            // 
            this.uiColorButton1.ColorPicker.BorderStyle = Janus.Windows.UI.BorderStyle.None;
            this.uiColorButton1.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.uiColorButton1.ColorPicker.Name = "";
            this.uiColorButton1.ColorPicker.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiColorButton1.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.uiColorButton1.ColorPicker.TabIndex = 0;
            this.uiColorButton1.ColorPicker.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiColorButton1.ColorPicker.VisualStyleManager = this.visualStyleManager1;
            this.uiColorButton1.Location = new System.Drawing.Point(181, 22);
            this.uiColorButton1.Name = "uiColorButton1";
            this.uiColorButton1.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiColorButton1.Size = new System.Drawing.Size(107, 23);
            this.uiColorButton1.TabIndex = 6;
            this.uiColorButton1.Text = "uiColorButton1";
            this.uiColorButton1.VisualStyleManager = this.visualStyleManager1;
            this.uiColorButton1.SelectedColorChanged += new System.EventHandler(this.uiColorButton1_SelectedColorChanged);
            // 
            // uiGroupBox9
            // 
            this.uiGroupBox9.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox9.Controls.Add(this.lightPreviewControl2);
            this.uiGroupBox9.Location = new System.Drawing.Point(3, 90);
            this.uiGroupBox9.Name = "uiGroupBox9";
            this.uiGroupBox9.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox9.Size = new System.Drawing.Size(300, 315);
            this.uiGroupBox9.TabIndex = 20;
            this.uiGroupBox9.Text = "Preview";
            this.uiGroupBox9.VisualStyleManager = this.visualStyleManager1;
            // 
            // lightPreviewControl2
            // 
            this.lightPreviewControl2.BackColor = System.Drawing.SystemColors.Control;
            this.lightPreviewControl2.Lighting = null;
            this.lightPreviewControl2.Location = new System.Drawing.Point(7, 16);
            this.lightPreviewControl2.Name = "lightPreviewControl2";
            this.lightPreviewControl2.OutSettings = null;
            this.lightPreviewControl2.Size = new System.Drawing.Size(287, 290);
            this.lightPreviewControl2.TabIndex = 0;
            // 
            // lightingSchemeLabel
            // 
            this.lightingSchemeLabel.BackColor = System.Drawing.Color.Transparent;
            this.lightingSchemeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lightingSchemeLabel.Location = new System.Drawing.Point(353, 87);
            this.lightingSchemeLabel.Name = "lightingSchemeLabel";
            this.lightingSchemeLabel.Size = new System.Drawing.Size(278, 19);
            this.lightingSchemeLabel.TabIndex = 19;
            this.lightingSchemeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(0, 454);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(628, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Lighting provides scene light information for the scheme and effects to use in sh" +
                "ading.";
            // 
            // uiButton2
            // 
            this.uiButton2.Location = new System.Drawing.Point(85, 411);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiButton2.Size = new System.Drawing.Size(75, 23);
            this.uiButton2.TabIndex = 17;
            this.uiButton2.Text = "Remove";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton2.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiButton1
            // 
            this.uiButton1.Location = new System.Drawing.Point(4, 411);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiButton1.Size = new System.Drawing.Size(75, 23);
            this.uiButton1.TabIndex = 14;
            this.uiButton1.Text = "Add";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton1.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiLightingList
            // 
            this.uiLightingList.BackColor = System.Drawing.Color.White;
            buttonBarGroup8.Key = "Group1";
            buttonBarGroup8.Text = "Presets";
            buttonBarGroup8.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            this.uiLightingList.Groups.AddRange(new Janus.Windows.ButtonBar.ButtonBarGroup[] {
            buttonBarGroup8});
            this.uiLightingList.ItemAppearance = Janus.Windows.ButtonBar.ItemAppearance.Flat;
            this.uiLightingList.Location = new System.Drawing.Point(3, 4);
            this.uiLightingList.Name = "uiLightingList";
            this.uiLightingList.Office2007ColorScheme = Janus.Windows.ButtonBar.Office2007ColorScheme.Black;
            this.uiLightingList.Orientation = Janus.Windows.ButtonBar.ButtonBarOrientation.Horizontal;
            this.uiLightingList.SelectionArea = Janus.Windows.ButtonBar.SelectionArea.FullItem;
            this.uiLightingList.ShadowOnHover = true;
            this.uiLightingList.Size = new System.Drawing.Size(628, 80);
            this.uiLightingList.TabIndex = 13;
            this.uiLightingList.Text = "buttonBar3";
            this.uiLightingList.ThemedAreas = ((Janus.Windows.ButtonBar.ThemedArea)((((Janus.Windows.ButtonBar.ThemedArea.Border | Janus.Windows.ButtonBar.ThemedArea.Groups)
                        | Janus.Windows.ButtonBar.ThemedArea.Items)
                        | Janus.Windows.ButtonBar.ThemedArea.ScrollButton)));
            this.uiLightingList.VisualStyle = Janus.Windows.ButtonBar.VisualStyle.Office2007;
            this.uiLightingList.ItemSelected += new System.EventHandler(this.uiLightingList_ItemSelected);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox1.Controls.Add(this.label28);
            this.uiGroupBox1.Controls.Add(this.uiColorButton2);
            this.uiGroupBox1.Location = new System.Drawing.Point(309, 395);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox1.Size = new System.Drawing.Size(319, 50);
            this.uiGroupBox1.TabIndex = 23;
            this.uiGroupBox1.Text = "Ambient";
            this.uiGroupBox1.VisualStyleManager = this.visualStyleManager1;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(18, 21);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(45, 13);
            this.label28.TabIndex = 1;
            this.label28.Text = "Colour:";
            // 
            // uiColorButton2
            // 
            // 
            // 
            // 
            this.uiColorButton2.ColorPicker.BorderStyle = Janus.Windows.UI.BorderStyle.None;
            this.uiColorButton2.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.uiColorButton2.ColorPicker.Name = "";
            this.uiColorButton2.ColorPicker.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiColorButton2.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.uiColorButton2.ColorPicker.TabIndex = 0;
            this.uiColorButton2.ColorPicker.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiColorButton2.ColorPicker.VisualStyleManager = this.visualStyleManager1;
            this.uiColorButton2.Location = new System.Drawing.Point(80, 16);
            this.uiColorButton2.Name = "uiColorButton2";
            this.uiColorButton2.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiColorButton2.Size = new System.Drawing.Size(99, 23);
            this.uiColorButton2.TabIndex = 0;
            this.uiColorButton2.Text = "uiColorButton2";
            this.uiColorButton2.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiTabPage4
            // 
            this.uiTabPage4.Controls.Add(this.uiGroupBox12);
            this.uiTabPage4.Controls.Add(this.groupBox13);
            this.uiTabPage4.Location = new System.Drawing.Point(1, 1);
            this.uiTabPage4.Name = "uiTabPage4";
            this.uiTabPage4.Size = new System.Drawing.Size(636, 470);
            this.uiTabPage4.TabStop = true;
            this.uiTabPage4.Text = "General";
            // 
            // uiGroupBox12
            // 
            this.uiGroupBox12.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox12.Controls.Add(this.uiComboBox2);
            this.uiGroupBox12.Controls.Add(this.label5);
            this.uiGroupBox12.Controls.Add(this.numericUpDown1);
            this.uiGroupBox12.Controls.Add(this.uiComboBox1);
            this.uiGroupBox12.Controls.Add(this.label9);
            this.uiGroupBox12.Controls.Add(this.label6);
            this.uiGroupBox12.Location = new System.Drawing.Point(22, 19);
            this.uiGroupBox12.Name = "uiGroupBox12";
            this.uiGroupBox12.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiGroupBox12.Size = new System.Drawing.Size(340, 99);
            this.uiGroupBox12.TabIndex = 2;
            this.uiGroupBox12.Text = "View Settings";
            this.uiGroupBox12.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiComboBox2
            // 
            uiComboBoxItem10.FormatStyle.Alpha = 0;
            uiComboBoxItem10.IsSeparator = false;
            uiComboBoxItem10.Text = "x1";
            uiComboBoxItem10.Value = ((ushort)(10));
            uiComboBoxItem11.FormatStyle.Alpha = 0;
            uiComboBoxItem11.IsSeparator = false;
            uiComboBoxItem11.Text = "x1.5";
            uiComboBoxItem11.Value = ((ushort)(15));
            uiComboBoxItem12.FormatStyle.Alpha = 0;
            uiComboBoxItem12.IsSeparator = false;
            uiComboBoxItem12.Text = "x2";
            uiComboBoxItem12.Value = ((ushort)(20));
            uiComboBoxItem13.FormatStyle.Alpha = 0;
            uiComboBoxItem13.IsSeparator = false;
            uiComboBoxItem13.Text = "x4";
            uiComboBoxItem13.Value = ((ushort)(40));
            this.uiComboBox2.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem10,
            uiComboBoxItem11,
            uiComboBoxItem12,
            uiComboBoxItem13});
            this.uiComboBox2.Location = new System.Drawing.Point(238, 53);
            this.uiComboBox2.Name = "uiComboBox2";
            this.uiComboBox2.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiComboBox2.Size = new System.Drawing.Size(84, 22);
            this.uiComboBox2.TabIndex = 5;
            this.uiComboBox2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiComboBox2.VisualStyleManager = this.visualStyleManager1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Anti-Aliasing:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(87, 53);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(53, 22);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown1.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // uiComboBox1
            // 
            this.uiComboBox1.Location = new System.Drawing.Point(88, 21);
            this.uiComboBox1.Name = "uiComboBox1";
            this.uiComboBox1.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiComboBox1.Size = new System.Drawing.Size(164, 22);
            this.uiComboBox1.TabIndex = 1;
            this.uiComboBox1.Text = "uiComboBox1";
            this.uiComboBox1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiComboBox1.VisualStyleManager = this.visualStyleManager1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(146, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Viewport Scale:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Field-of-View:";
            // 
            // groupBox13
            // 
            this.groupBox13.BackColor = System.Drawing.Color.Transparent;
            this.groupBox13.Controls.Add(this.groupBox15);
            this.groupBox13.Controls.Add(this.radioButton2);
            this.groupBox13.Controls.Add(this.radioButton1);
            this.groupBox13.Location = new System.Drawing.Point(22, 156);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(340, 118);
            this.groupBox13.TabIndex = 0;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Shadows";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.uiComboBox3);
            this.groupBox15.Controls.Add(this.label10);
            this.groupBox15.Location = new System.Drawing.Point(19, 55);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(315, 53);
            this.groupBox15.TabIndex = 2;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Shadow Map Settings";
            // 
            // uiComboBox3
            // 
            uiComboBoxItem14.FormatStyle.Alpha = 0;
            uiComboBoxItem14.IsSeparator = false;
            uiComboBoxItem14.Text = "128x128";
            uiComboBoxItem14.Value = ((uint)(128u));
            uiComboBoxItem15.FormatStyle.Alpha = 0;
            uiComboBoxItem15.IsSeparator = false;
            uiComboBoxItem15.Text = "256x256";
            uiComboBoxItem15.Value = ((uint)(256u));
            uiComboBoxItem16.FormatStyle.Alpha = 0;
            uiComboBoxItem16.IsSeparator = false;
            uiComboBoxItem16.Text = "512x512";
            uiComboBoxItem16.Value = ((uint)(512u));
            uiComboBoxItem17.FormatStyle.Alpha = 0;
            uiComboBoxItem17.IsSeparator = false;
            uiComboBoxItem17.Text = "1024x1024";
            uiComboBoxItem17.Value = ((uint)(1024u));
            this.uiComboBox3.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem14,
            uiComboBoxItem15,
            uiComboBoxItem16,
            uiComboBoxItem17});
            this.uiComboBox3.Location = new System.Drawing.Point(68, 26);
            this.uiComboBox3.Name = "uiComboBox3";
            this.uiComboBox3.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiComboBox3.Size = new System.Drawing.Size(127, 22);
            this.uiComboBox3.TabIndex = 1;
            this.uiComboBox3.VisualStyleManager = this.visualStyleManager1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Size:";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(102, 32);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(117, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Shadow Mapping";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(19, 32);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(53, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "None";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // uiTabPage5
            // 
            this.uiTabPage5.Controls.Add(this.uiShadingElementList);
            this.uiTabPage5.Controls.Add(this.uiShadingSeriesGroup);
            this.uiTabPage5.Controls.Add(this.label27);
            this.uiTabPage5.Controls.Add(this.uiShadingSeriesList);
            this.uiTabPage5.Controls.Add(this.uiShadingElementsGroup);
            this.uiTabPage5.Controls.Add(this.label21);
            this.uiTabPage5.Controls.Add(this.uiElementShadingList);
            this.uiTabPage5.Controls.Add(this.panel1);
            this.uiTabPage5.Controls.Add(this.label26);
            this.uiTabPage5.Location = new System.Drawing.Point(1, 1);
            this.uiTabPage5.Name = "uiTabPage5";
            this.uiTabPage5.Size = new System.Drawing.Size(636, 470);
            this.uiTabPage5.TabStop = true;
            this.uiTabPage5.Text = "Elements Shading";
            // 
            // uiShadingElementList
            // 
            this.uiShadingElementList.FormattingEnabled = true;
            this.uiShadingElementList.Location = new System.Drawing.Point(140, 98);
            this.uiShadingElementList.Name = "uiShadingElementList";
            this.uiShadingElementList.Size = new System.Drawing.Size(116, 160);
            this.uiShadingElementList.TabIndex = 27;
            this.uiShadingElementList.SelectedIndexChanged += new System.EventHandler(this.uiShadingElementList_SelectedIndexChanged);
            // 
            // uiShadingSeriesGroup
            // 
            this.uiShadingSeriesGroup.BackColor = System.Drawing.Color.Transparent;
            this.uiShadingSeriesGroup.Controls.Add(this.uiCheckBox2);
            this.uiShadingSeriesGroup.Controls.Add(this.uiColorButton3);
            this.uiShadingSeriesGroup.Controls.Add(this.label29);
            this.uiShadingSeriesGroup.Enabled = false;
            this.uiShadingSeriesGroup.Location = new System.Drawing.Point(262, 94);
            this.uiShadingSeriesGroup.Name = "uiShadingSeriesGroup";
            this.uiShadingSeriesGroup.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiShadingSeriesGroup.Size = new System.Drawing.Size(163, 183);
            this.uiShadingSeriesGroup.TabIndex = 26;
            this.uiShadingSeriesGroup.Text = "Series Settings";
            this.uiShadingSeriesGroup.VisualStyleManager = this.visualStyleManager1;
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(140, 259);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(116, 13);
            this.label27.TabIndex = 25;
            this.label27.Text = "Element";
            // 
            // uiShadingSeriesList
            // 
            this.uiShadingSeriesList.Enabled = false;
            this.uiShadingSeriesList.FormattingEnabled = true;
            this.uiShadingSeriesList.Location = new System.Drawing.Point(7, 98);
            this.uiShadingSeriesList.Name = "uiShadingSeriesList";
            this.uiShadingSeriesList.Size = new System.Drawing.Size(127, 160);
            this.uiShadingSeriesList.TabIndex = 21;
            this.uiShadingSeriesList.SelectedIndexChanged += new System.EventHandler(this.uiShadingSeriesList_SelectedIndexChanged);
            // 
            // uiShadingElementsGroup
            // 
            this.uiShadingElementsGroup.BackColor = System.Drawing.Color.Transparent;
            this.uiShadingElementsGroup.Controls.Add(this.uiCheckBox3);
            this.uiShadingElementsGroup.Controls.Add(this.uiElementClrBtn);
            this.uiShadingElementsGroup.Controls.Add(this.label24);
            this.uiShadingElementsGroup.Enabled = false;
            this.uiShadingElementsGroup.Location = new System.Drawing.Point(431, 94);
            this.uiShadingElementsGroup.Name = "uiShadingElementsGroup";
            this.uiShadingElementsGroup.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiShadingElementsGroup.Size = new System.Drawing.Size(197, 183);
            this.uiShadingElementsGroup.TabIndex = 20;
            this.uiShadingElementsGroup.Text = "Element Settings";
            this.uiShadingElementsGroup.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiElementClrBtn
            // 
            // 
            // 
            // 
            this.uiElementClrBtn.ColorPicker.BorderStyle = Janus.Windows.UI.BorderStyle.None;
            this.uiElementClrBtn.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.uiElementClrBtn.ColorPicker.Name = "";
            this.uiElementClrBtn.ColorPicker.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiElementClrBtn.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.uiElementClrBtn.ColorPicker.TabIndex = 0;
            this.uiElementClrBtn.ColorPicker.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiElementClrBtn.ColorPicker.VisualStyleManager = this.visualStyleManager1;
            this.uiElementClrBtn.Location = new System.Drawing.Point(53, 16);
            this.uiElementClrBtn.Name = "uiElementClrBtn";
            this.uiElementClrBtn.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiElementClrBtn.Size = new System.Drawing.Size(127, 23);
            this.uiElementClrBtn.TabIndex = 2;
            this.uiElementClrBtn.VisualStyleManager = this.visualStyleManager1;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 21);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(41, 13);
            this.label24.TabIndex = 1;
            this.label24.Text = "Color :";
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.ForeColor = System.Drawing.Color.DimGray;
            this.label21.Location = new System.Drawing.Point(0, 454);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(628, 16);
            this.label21.TabIndex = 19;
            this.label21.Text = "Determines how each element is coloured";
            // 
            // uiElementShadingList
            // 
            this.uiElementShadingList.BackColor = System.Drawing.Color.White;
            buttonBarGroup9.Key = "Group1";
            buttonBarGroup9.Text = "Presets";
            buttonBarGroup9.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            buttonBarGroup10.Key = "Group2";
            buttonBarGroup10.Text = "Templates";
            buttonBarGroup10.View = Janus.Windows.ButtonBar.ButtonBarView.SmallIcons;
            this.uiElementShadingList.Groups.AddRange(new Janus.Windows.ButtonBar.ButtonBarGroup[] {
            buttonBarGroup9,
            buttonBarGroup10});
            this.uiElementShadingList.ItemAppearance = Janus.Windows.ButtonBar.ItemAppearance.Flat;
            this.uiElementShadingList.Location = new System.Drawing.Point(3, 3);
            this.uiElementShadingList.Name = "uiElementShadingList";
            this.uiElementShadingList.Office2007ColorScheme = Janus.Windows.ButtonBar.Office2007ColorScheme.Black;
            this.uiElementShadingList.Orientation = Janus.Windows.ButtonBar.ButtonBarOrientation.Horizontal;
            this.uiElementShadingList.SelectionArea = Janus.Windows.ButtonBar.SelectionArea.FullItem;
            this.uiElementShadingList.ShadowOnHover = true;
            this.uiElementShadingList.Size = new System.Drawing.Size(625, 80);
            this.uiElementShadingList.TabIndex = 1;
            this.uiElementShadingList.Text = "buttonBar1";
            this.uiElementShadingList.ThemedAreas = ((Janus.Windows.ButtonBar.ThemedArea)((((Janus.Windows.ButtonBar.ThemedArea.Border | Janus.Windows.ButtonBar.ThemedArea.Groups)
                        | Janus.Windows.ButtonBar.ThemedArea.Items)
                        | Janus.Windows.ButtonBar.ThemedArea.ScrollButton)));
            this.uiElementShadingList.VisualStyle = Janus.Windows.ButtonBar.VisualStyle.Office2007;
            this.uiElementShadingList.ItemSelected += new System.EventHandler(this.uiElementShadingList_ItemSelected);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.periodicTableControl1);
            this.panel1.Location = new System.Drawing.Point(7, 283);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(623, 168);
            this.panel1.TabIndex = 0;
            // 
            // periodicTableControl1
            // 
            this.periodicTableControl1.BackColor = System.Drawing.Color.White;
            this.periodicTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.periodicTableControl1.Enabled = false;
            this.periodicTableControl1.Location = new System.Drawing.Point(3, 3);
            this.periodicTableControl1.Name = "periodicTableControl1";
            this.periodicTableControl1.Size = new System.Drawing.Size(617, 162);
            this.periodicTableControl1.TabIndex = 0;
            this.periodicTableControl1.OnElementSelect += new System.EventHandler(this.periodicTableControl1_OnElementSelect);
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(7, 259);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(127, 13);
            this.label26.TabIndex = 24;
            this.label26.Text = "Series";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 579);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // multiColumnCombo1
            // 
            gridEXLayout1.LayoutString = resources.GetString("gridEXLayout1.LayoutString");
            this.multiColumnCombo1.DesignTimeLayout = gridEXLayout1;
            this.multiColumnCombo1.Location = new System.Drawing.Point(10, 19);
            this.multiColumnCombo1.Name = "multiColumnCombo1";
            this.multiColumnCombo1.SelectedIndex = -1;
            this.multiColumnCombo1.SelectedItem = null;
            this.multiColumnCombo1.Size = new System.Drawing.Size(248, 22);
            this.multiColumnCombo1.TabIndex = 12;
            this.multiColumnCombo1.VisualStyleManager = this.visualStyleManager1;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.multiColumnCombo1);
            this.groupBox10.Location = new System.Drawing.Point(13, 510);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(637, 63);
            this.groupBox10.TabIndex = 13;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Settings Scope";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NuGenSVisualLib.Properties.Resources.Arrow;
            this.pictureBox1.Location = new System.Drawing.Point(672, 107);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 62);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::NuGenSVisualLib.Properties.Resources.Arrow;
            this.pictureBox2.Location = new System.Drawing.Point(672, 265);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(23, 62);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::NuGenSVisualLib.Properties.Resources.Arrow;
            this.pictureBox3.Location = new System.Drawing.Point(672, 412);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(23, 62);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 16;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Image = global::NuGenSVisualLib.Properties.Resources.redcross_32;
            this.pictureBox4.Location = new System.Drawing.Point(668, 12);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(96, 96);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox4.TabIndex = 17;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox5.Location = new System.Drawing.Point(668, 474);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(128, 128);
            this.pictureBox5.TabIndex = 18;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox6.Image = global::NuGenSVisualLib.Properties.Resources.redcross_32;
            this.pictureBox6.Location = new System.Drawing.Point(668, 169);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(96, 96);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox6.TabIndex = 19;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox7.Image = global::NuGenSVisualLib.Properties.Resources.redcross_32;
            this.pictureBox7.Location = new System.Drawing.Point(668, 328);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(96, 96);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox7.TabIndex = 20;
            this.pictureBox7.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(718, 111);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Scheme";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.Color.DimGray;
            this.label19.Location = new System.Drawing.Point(723, 268);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 13);
            this.label19.TabIndex = 22;
            this.label19.Text = "Effects";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.DimGray;
            this.label20.Location = new System.Drawing.Point(714, 427);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(50, 13);
            this.label20.TabIndex = 23;
            this.label20.Text = "Lighting";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(6, 21);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(38, 13);
            this.label29.TabIndex = 0;
            this.label29.Text = "Color:";
            // 
            // uiColorButton3
            // 
            // 
            // 
            // 
            this.uiColorButton3.ColorPicker.BorderStyle = Janus.Windows.UI.BorderStyle.None;
            this.uiColorButton3.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.uiColorButton3.ColorPicker.Name = "";
            this.uiColorButton3.ColorPicker.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiColorButton3.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.uiColorButton3.ColorPicker.TabIndex = 0;
            this.uiColorButton3.ColorPicker.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiColorButton3.ColorPicker.VisualStyleManager = this.visualStyleManager1;
            this.uiColorButton3.Location = new System.Drawing.Point(50, 16);
            this.uiColorButton3.Name = "uiColorButton3";
            this.uiColorButton3.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiColorButton3.Size = new System.Drawing.Size(97, 23);
            this.uiColorButton3.TabIndex = 1;
            this.uiColorButton3.Text = "uiColorButton3";
            this.uiColorButton3.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiCheckBox2
            // 
            this.uiCheckBox2.Location = new System.Drawing.Point(9, 151);
            this.uiCheckBox2.Name = "uiCheckBox2";
            this.uiCheckBox2.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiCheckBox2.Size = new System.Drawing.Size(104, 23);
            this.uiCheckBox2.TabIndex = 2;
            this.uiCheckBox2.Text = "Override";
            this.uiCheckBox2.VisualStyleManager = this.visualStyleManager1;
            // 
            // uiCheckBox3
            // 
            this.uiCheckBox3.Location = new System.Drawing.Point(9, 151);
            this.uiCheckBox3.Name = "uiCheckBox3";
            this.uiCheckBox3.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
            this.uiCheckBox3.Size = new System.Drawing.Size(104, 23);
            this.uiCheckBox3.TabIndex = 3;
            this.uiCheckBox3.Text = "Override";
            this.uiCheckBox3.VisualStyleManager = this.visualStyleManager1;
            // 
            // MoleculeSchemeDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 616);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.uiTab1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox3);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MoleculeSchemeDlg";
            this.Text = "Molecule Scheme Settings";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBondsGSetGroup)).EndInit();
            this.uiBondsGSetGroup.ResumeLayout(false);
            this.uiBondsGSetGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiAtomsGSetGroup)).EndInit();
            this.uiAtomsGSetGroup.ResumeLayout(false);
            this.uiAtomsGSetGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox6)).EndInit();
            this.uiGroupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiSchemesList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.uiTab1.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox8)).EndInit();
            this.uiGroupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox7)).EndInit();
            this.uiGroupBox7.ResumeLayout(false);
            this.uiGroupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).EndInit();
            this.uiGroupBox3.ResumeLayout(false);
            this.uiTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox14)).EndInit();
            this.uiGroupBox14.ResumeLayout(false);
            this.uiGroupBox14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox13)).EndInit();
            this.uiGroupBox13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiEffectPropGroup)).EndInit();
            this.uiEffectPropGroup.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiEffectsReqList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCurrentEffectsList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiEffectsList)).EndInit();
            this.effectsListMenuStrip.ResumeLayout(false);
            this.uiTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox11)).EndInit();
            this.uiGroupBox11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiLightingPropGroup)).EndInit();
            this.uiLightingPropGroup.ResumeLayout(false);
            this.uiLightingPropGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox9)).EndInit();
            this.uiGroupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiLightingList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            this.uiTabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox12)).EndInit();
            this.uiGroupBox12.ResumeLayout(false);
            this.uiGroupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.uiTabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiShadingSeriesGroup)).EndInit();
            this.uiShadingSeriesGroup.ResumeLayout(false);
            this.uiShadingSeriesGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiShadingElementsGroup)).EndInit();
            this.uiShadingElementsGroup.ResumeLayout(false);
            this.uiShadingElementsGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiElementShadingList)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.multiColumnCombo1)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Janus.Windows.ButtonBar.ButtonBar uiSchemesList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private SchemePreviewControl schemePreviewControl;
        private System.Windows.Forms.Label label13;
        private Janus.Windows.UI.Tab.UITab uiTab1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage2;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage3;
        private Janus.Windows.ButtonBar.ButtonBar uiEffectsList;
        private System.Windows.Forms.Button button3;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.ButtonBar.ButtonBar uiLightingList;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private System.Windows.Forms.ListBox lightsListBox;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo multiColumnCombo1;
        private System.Windows.Forms.GroupBox groupBox10;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage4;
        private System.Windows.Forms.Label label22;
        private LightPreviewControl lightPreviewControl2;
        private System.Windows.Forms.Label label23;
        private RequirementGague requirementGague2;
        private RequirementGague requirementGague1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox13;
        private Janus.Windows.EditControls.UIComboBox uiComboBox2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private Janus.Windows.EditControls.UIComboBox uiComboBox1;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private Janus.Windows.EditControls.UIComboBox uiComboBox3;
        private Janus.Windows.Common.VisualStyleManager visualStyleManager1;
        private Janus.Windows.EditControls.UIColorButton uiColorButton1;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Panel uiLightingPropPanel;
        private System.Windows.Forms.Label lightingSchemeLabel;
        private System.Windows.Forms.Label label16;
        private RequirementGague uiEfxPSReqs;
        private RequirementGague uiEfxVSReqs;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private Janus.Windows.ButtonBar.ButtonBar uiCurrentEffectsList;
        private Janus.Windows.ButtonBar.ButtonBar uiEffectsReqList;
        private System.Windows.Forms.Label label17;
        private Janus.Windows.EditControls.UIGroupBox uiEffectPropGroup;
        private Janus.Windows.EditControls.UIButton uiRemoveEffectBtn;
        private Janus.Windows.EditControls.UIButton uiAddEffectBtn;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox1;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.Panel panel4;
        private LODcontrol uiAtomLODControl;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox3;
        private Janus.Windows.EditControls.UIGroupBox uiAtomsGSetGroup;
        private Janus.Windows.EditControls.UIComboBox uiAtomFillModeList;
        private Janus.Windows.EditControls.UICheckBox uiAtomSymbolsDraw;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox6;
        private Janus.Windows.EditControls.UICheckBox uiAtomSymbolsBlend;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox7;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox8;
        private Janus.Windows.EditControls.UICheckBox uiAtomDDraw;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox9;
        private Janus.Windows.EditControls.UIGroupBox uiLightingPropGroup;
        private Janus.Windows.EditControls.UICheckBox uiLightEnabled;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox11;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox12;
        private Janus.Windows.EditControls.UIGroupBox uiBondsGSetGroup;
        private Janus.Windows.EditControls.UICheckBox uiBondDDraw;
        private Janus.Windows.EditControls.UIComboBox uiBondEndTypeList;
        private Janus.Windows.EditControls.UIComboBox uiBondSpacingList;
        private LODcontrol uiBondLODControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox14;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox13;
        private System.Windows.Forms.ContextMenuStrip effectsListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage5;
        private EffectPreviewControl effectPreviewControl1;
        private LODcontrol uiEffectLOD;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.ButtonBar.ButtonBar uiElementShadingList;
        private System.Windows.Forms.Label label21;
        private Janus.Windows.EditControls.UIGroupBox uiShadingElementsGroup;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ListBox uiShadingSeriesList;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private Janus.Windows.EditControls.UIGroupBox uiShadingSeriesGroup;
        private Janus.Windows.EditControls.UIColorButton uiElementClrBtn;
        private PeriodicTableControl periodicTableControl1;
        private System.Windows.Forms.ListBox uiShadingElementList;
        private Janus.Windows.EditControls.UICheckBox uiLightCastShadows;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private System.Windows.Forms.Label label28;
        private Janus.Windows.EditControls.UIColorButton uiColorButton2;
        private Janus.Windows.EditControls.UIColorButton uiColorButton3;
        private System.Windows.Forms.Label label29;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox2;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox3;
    }
}
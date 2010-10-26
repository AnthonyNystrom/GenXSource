namespace Facade
{
    using Attrs;
    using Nodes;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    internal class StylePropertiesDialog : Form
    {
        public StylePropertiesDialog(StyleAttributes styleAttributes, Node parentNode)
        {
            this.container = null;
            this.color = Color.Black;
            this.background = Color.White;
            this.isBold = false;
            this.isItalic = false;
            this.isFraktur = false;
            this.isNormal = false;
            this.isDoubleStruck = false;
            this.isScript = false;
            this.isMonospace = false;
            this.isSans = false;
            this.isBig = false;
            this.isNormalVar = false;
            this.isSmall = false;
            this.success = false;
            this.hasScriptLevel = false;
            this.InitializeComponent();
            this.Style = styleAttributes;
            this.color = this.Style.color;
            this.background = this.Style.background;
            this.isBold = this.Style.isBold;
            this.isItalic = this.Style.isItalic;
            this.isFraktur = this.Style.isFractur;
            this.isNormal = this.Style.isNormal;
            this.isDoubleStruck = this.Style.isDoubleStruck;
            this.isMonospace = this.Style.isMonospace;
            this.isScript = this.Style.isScript;
            this.isSans = this.Style.isSans;
            this.sizeEdit.Text = this.Style.FontSize();
            if (this.Style.scale == 1.25)
            {
                this.isBig = true;
            }
            else if (this.Style.scale == 0.8)
            {
                this.isSmall = true;
            }
            else if (this.Style.scale == 1)
            {
                this.isNormalVar = true;
            }
            DisplayStyle displayStyle = DisplayStyle.AUTOMATIC;
            ScriptLevel scriptLevel = ScriptLevel.NONE;
            if ((parentNode != null) && (parentNode.style_ != null))
            {
                displayStyle = parentNode.style_.displayStyle;
                scriptLevel = parentNode.style_.scriptLevel;
            }
            if ((this.Style.displayStyle == DisplayStyle.TRUE) && (this.Style.displayStyle != displayStyle))
            {
                this.dispStyle.SelectedItem = this.dispStyle.Items[1];
            }
            else if ((this.Style.displayStyle == DisplayStyle.FALSE) && (this.Style.displayStyle != displayStyle))
            {
                this.dispStyle.SelectedItem = this.dispStyle.Items[2];
            }
            else
            {
                this.dispStyle.SelectedItem = this.dispStyle.Items[0];
            }
            try
            {
                this.scriptLevel.SelectedItem = this.scriptLevel.Items[0];
                if ((this.Style.scriptLevel != ScriptLevel.NONE) && (this.Style.scriptLevel != scriptLevel))
                {
                    switch (this.Style.scriptLevel)
                    {
                        case ScriptLevel.ZERO:
                        {
                            this.scriptLevel.SelectedItem = this.scriptLevel.Items[1];
                            break;
                        }
                        case ScriptLevel.ONE:
                        {
                            this.scriptLevel.SelectedItem = this.scriptLevel.Items[2];
                            break;
                        }
                        case ScriptLevel.TWO:
                        {
                            this.scriptLevel.SelectedItem = this.scriptLevel.Items[3];
                            break;
                        }
                        case ScriptLevel.PLUS_ONE:
                        {
                            this.scriptLevel.SelectedItem = this.scriptLevel.Items[4];
                            break;
                        }
                        case ScriptLevel.PLUS_TWO:
                        {
                            this.scriptLevel.SelectedItem = this.scriptLevel.Items[5];
                            break;
                        }
                        case ScriptLevel.MINUS_ONE:
                        {
                            this.scriptLevel.SelectedItem = this.scriptLevel.Items[6];
                            break;
                        }
                        case ScriptLevel.MINUS_TWO:
                        {
                            this.scriptLevel.SelectedItem = this.scriptLevel.Items[7];
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
        
            this.smallVar.Checked = this.isSmall;
            this.normalVar.Checked = this.isNormalVar;
            this.bigVar.Checked = this.isBig;
            this.foreColorPanel.BackColor = this.color;
            this.backColorPanel.BackColor = this.background;
            this.foreColorPanel.BackColor = this.color;
            this.backColorPanel.BackColor = this.background;
            this.bold.Checked = this.isBold;
            this.italic.Checked = this.isItalic;
            this.doubleStruck.Checked = this.isDoubleStruck;
            this.fraktur.Checked = this.isFraktur;
            this.monospace.Checked = this.isMonospace;
            this.sansSerif.Checked = this.isSans;
            this.script.Checked = this.isScript;
            this.normal.Checked = this.isNormal;
            if (((!this.isBold && !this.isItalic) && (!this.isDoubleStruck && !this.isFraktur)) && ((!this.isMonospace && !this.isSans) && (!this.isScript && !this.isNormal)))
            {
                this.notSet.Checked = true;
            }
            this.hasScriptLevel = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.container != null))
            {
                this.container.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Return)
            {
                this.Success = true;
                base.Close();
            }
            else if (keyData == Keys.Escape)
            {
                base.Close();
            }
            else
            {
                return base.ProcessDialogKey(keyData);
            }
            return true;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StylePropertiesDialog));
            this.foreColor = new System.Windows.Forms.GroupBox();
            this.seleColor = new Glass.GlassButton();
            this.foreColorPanel = new System.Windows.Forms.Panel();
            this.backColor = new System.Windows.Forms.GroupBox();
            this.backColorSelect = new Glass.GlassButton();
            this.backColorPanel = new System.Windows.Forms.Panel();
            this.cancel = new Glass.GlassButton();
            this.ok = new Glass.GlassButton();
            this.bold = new System.Windows.Forms.CheckBox();
            this.italic = new System.Windows.Forms.CheckBox();
            this.group1 = new System.Windows.Forms.GroupBox();
            this.notSet = new System.Windows.Forms.RadioButton();
            this.doubleStruck = new System.Windows.Forms.RadioButton();
            this.normal = new System.Windows.Forms.RadioButton();
            this.monospace = new System.Windows.Forms.RadioButton();
            this.fraktur = new System.Windows.Forms.RadioButton();
            this.script = new System.Windows.Forms.RadioButton();
            this.sansSerif = new System.Windows.Forms.RadioButton();
            this.group22 = new System.Windows.Forms.GroupBox();
            this.sizeEdit = new System.Windows.Forms.TextBox();
            this.bigVar = new System.Windows.Forms.RadioButton();
            this.smallVar = new System.Windows.Forms.RadioButton();
            this.normalVar = new System.Windows.Forms.RadioButton();
            this.scriptLevel = new System.Windows.Forms.ComboBox();
            this.group3 = new System.Windows.Forms.GroupBox();
            this.dispStyle = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.foreColor.SuspendLayout();
            this.backColor.SuspendLayout();
            this.group1.SuspendLayout();
            this.group22.SuspendLayout();
            this.group3.SuspendLayout();
            this.SuspendLayout();
            // 
            // foreColor
            // 
            this.foreColor.Controls.Add(this.seleColor);
            this.foreColor.Controls.Add(this.foreColorPanel);
            this.foreColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.foreColor, "foreColor");
            this.foreColor.Name = "foreColor";
            this.foreColor.TabStop = false;
            // 
            // seleColor
            // 
            resources.ApplyResources(this.seleColor, "seleColor");
            this.seleColor.Name = "seleColor";
            this.seleColor.Click += new System.EventHandler(this.OnSelectColor);
            // 
            // foreColorPanel
            // 
            resources.ApplyResources(this.foreColorPanel, "foreColorPanel");
            this.foreColorPanel.Name = "foreColorPanel";
            // 
            // backColor
            // 
            this.backColor.Controls.Add(this.backColorSelect);
            this.backColor.Controls.Add(this.backColorPanel);
            this.backColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.backColor, "backColor");
            this.backColor.Name = "backColor";
            this.backColor.TabStop = false;
            // 
            // backColorSelect
            // 
            resources.ApplyResources(this.backColorSelect, "backColorSelect");
            this.backColorSelect.Name = "backColorSelect";
            this.backColorSelect.Click += new System.EventHandler(this.OnBackColorSelect);
            // 
            // backColorPanel
            // 
            resources.ApplyResources(this.backColorPanel, "backColorPanel");
            this.backColorPanel.Name = "backColorPanel";
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.Click += new System.EventHandler(this.cancelClicked);
            // 
            // ok
            // 
            resources.ApplyResources(this.ok, "ok");
            this.ok.Name = "ok";
            this.ok.Click += new System.EventHandler(this.okClicked);
            // 
            // bold
            // 
            resources.ApplyResources(this.bold, "bold");
            this.bold.Name = "bold";
            this.bold.CheckedChanged += new System.EventHandler(this.OnBold);
            // 
            // italic
            // 
            resources.ApplyResources(this.italic, "italic");
            this.italic.Name = "italic";
            this.italic.CheckedChanged += new System.EventHandler(this.OnItalic);
            // 
            // group1
            // 
            this.group1.Controls.Add(this.notSet);
            this.group1.Controls.Add(this.doubleStruck);
            this.group1.Controls.Add(this.normal);
            this.group1.Controls.Add(this.monospace);
            this.group1.Controls.Add(this.fraktur);
            this.group1.Controls.Add(this.script);
            this.group1.Controls.Add(this.sansSerif);
            this.group1.Controls.Add(this.bold);
            this.group1.Controls.Add(this.italic);
            this.group1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group1, "group1");
            this.group1.Name = "group1";
            this.group1.TabStop = false;
            // 
            // notSet
            // 
            resources.ApplyResources(this.notSet, "notSet");
            this.notSet.Name = "notSet";
            this.notSet.CheckedChanged += new System.EventHandler(this.OnDefault);
            // 
            // doubleStruck
            // 
            resources.ApplyResources(this.doubleStruck, "doubleStruck");
            this.doubleStruck.Name = "doubleStruck";
            this.doubleStruck.CheckedChanged += new System.EventHandler(this.OnDoubleStruck);
            // 
            // normal
            // 
            resources.ApplyResources(this.normal, "normal");
            this.normal.Name = "normal";
            this.normal.CheckedChanged += new System.EventHandler(this.OnNormal);
            // 
            // monospace
            // 
            resources.ApplyResources(this.monospace, "monospace");
            this.monospace.Name = "monospace";
            this.monospace.CheckedChanged += new System.EventHandler(this.OnMonospace);
            // 
            // fraktur
            // 
            resources.ApplyResources(this.fraktur, "fraktur");
            this.fraktur.Name = "fraktur";
            this.fraktur.CheckedChanged += new System.EventHandler(this.OnFraktur);
            // 
            // script
            // 
            resources.ApplyResources(this.script, "script");
            this.script.Name = "script";
            this.script.CheckedChanged += new System.EventHandler(this.OnScript);
            // 
            // sansSerif
            // 
            resources.ApplyResources(this.sansSerif, "sansSerif");
            this.sansSerif.Name = "sansSerif";
            this.sansSerif.CheckedChanged += new System.EventHandler(this.OnSansSerif);
            // 
            // group22
            // 
            this.group22.Controls.Add(this.sizeEdit);
            this.group22.Controls.Add(this.bigVar);
            this.group22.Controls.Add(this.smallVar);
            this.group22.Controls.Add(this.normalVar);
            this.group22.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group22, "group22");
            this.group22.Name = "group22";
            this.group22.TabStop = false;
            // 
            // sizeEdit
            // 
            this.sizeEdit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.sizeEdit, "sizeEdit");
            this.sizeEdit.Name = "sizeEdit";
            this.sizeEdit.ReadOnly = true;
            // 
            // bigVar
            // 
            resources.ApplyResources(this.bigVar, "bigVar");
            this.bigVar.Name = "bigVar";
            this.bigVar.CheckedChanged += new System.EventHandler(this.OnBig);
            // 
            // smallVar
            // 
            resources.ApplyResources(this.smallVar, "smallVar");
            this.smallVar.Name = "smallVar";
            this.smallVar.CheckedChanged += new System.EventHandler(this.OnSmall);
            // 
            // normalVar
            // 
            resources.ApplyResources(this.normalVar, "normalVar");
            this.normalVar.Name = "normalVar";
            this.normalVar.CheckedChanged += new System.EventHandler(this.OnNormalSize);
            // 
            // scriptLevel
            // 
            this.scriptLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scriptLevel.Items.AddRange(new object[] {
            resources.GetString("scriptLevel.Items"),
            resources.GetString("scriptLevel.Items1"),
            resources.GetString("scriptLevel.Items2"),
            resources.GetString("scriptLevel.Items3"),
            resources.GetString("scriptLevel.Items4"),
            resources.GetString("scriptLevel.Items5"),
            resources.GetString("scriptLevel.Items6"),
            resources.GetString("scriptLevel.Items7")});
            resources.ApplyResources(this.scriptLevel, "scriptLevel");
            this.scriptLevel.Name = "scriptLevel";
            this.scriptLevel.SelectedIndexChanged += new System.EventHandler(this.OnScriptLevelChanged);
            // 
            // group3
            // 
            this.group3.Controls.Add(this.dispStyle);
            this.group3.Controls.Add(this.label1);
            this.group3.Controls.Add(this.label2);
            this.group3.Controls.Add(this.scriptLevel);
            this.group3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group3, "group3");
            this.group3.Name = "group3";
            this.group3.TabStop = false;
            // 
            // dispStyle
            // 
            this.dispStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dispStyle.Items.AddRange(new object[] {
            resources.GetString("dispStyle.Items"),
            resources.GetString("dispStyle.Items1"),
            resources.GetString("dispStyle.Items2")});
            resources.ApplyResources(this.dispStyle, "dispStyle");
            this.dispStyle.Name = "dispStyle";
            this.dispStyle.SelectedIndexChanged += new System.EventHandler(this.OnDispStyle);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // StylePropertiesDialog
            // 
            this.AcceptButton = this.ok;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancel;
            this.Controls.Add(this.group3);
            this.Controls.Add(this.group22);
            this.Controls.Add(this.group1);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.backColor);
            this.Controls.Add(this.foreColor);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StylePropertiesDialog";
            this.foreColor.ResumeLayout(false);
            this.backColor.ResumeLayout(false);
            this.group1.ResumeLayout(false);
            this.group22.ResumeLayout(false);
            this.group22.PerformLayout();
            this.group3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void OnSelectColor(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            try
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.Style.color = dialog.Color;
                    this.color = dialog.Color;
                    this.foreColorPanel.BackColor = dialog.Color;
                }
            }
            catch
            {
                return;
            }
            finally
            {
                try
                {
                    dialog.Dispose();
                }
                catch
                {
                }
            }
        }

        private void OnBold(object sender, EventArgs e)
        {
            this.isBold = this.bold.Checked;
            this.Style.isBold = this.isBold;
            if (this.Style.isBold)
            {
                this.Style.isNormal = false;
                this.normal.Checked = false;
                this.notSet.Checked = false;
            }
        }

        private void OnItalic(object sender, EventArgs e)
        {
            this.isItalic = this.italic.Checked;
            this.Style.isItalic = this.isItalic;
            if (this.Style.isItalic)
            {
                this.Style.isNormal = false;
                this.notSet.Checked = false;
                this.normal.Checked = false;
            }
        }

        private void OnSansSerif(object sender, EventArgs e)
        {
            this.isSans = this.sansSerif.Checked;
            this.Style.isSans = this.isSans;
            if (this.Style.isSans)
            {
                this.bold.Enabled = true;
                this.italic.Enabled = true;
                this.Style.isNormal = false;
                this.Style.isFractur = false;
                this.Style.isScript = false;
                this.Style.isDoubleStruck = false;
                this.Style.isMonospace = false;
            }
        }

        private void OnScript(object sender, EventArgs e)
        {
            this.isScript = this.script.Checked;
            this.Style.isScript = this.isScript;
            if (this.Style.isScript)
            {
                this.italic.Checked = false;
                this.italic.Enabled = false;
                this.bold.Enabled = true;
                this.Style.isNormal = false;
                this.Style.isFractur = false;
                this.Style.isSans = false;
                this.Style.isDoubleStruck = false;
                this.Style.isMonospace = false;
            }
        }

        private void OnFraktur(object sender, EventArgs e)
        {
            this.isFraktur = this.fraktur.Checked;
            this.Style.isFractur = this.isFraktur;
            if (this.Style.isFractur)
            {
                this.italic.Checked = false;
                this.italic.Enabled = false;
                this.bold.Enabled = true;
                this.Style.isNormal = false;
                this.Style.isScript = false;
                this.Style.isSans = false;
                this.Style.isDoubleStruck = false;
                this.Style.isMonospace = false;
            }
        }

        private void OnMonospace(object sender, EventArgs e)
        {
            this.isMonospace = this.monospace.Checked;
            this.Style.isMonospace = this.isMonospace;
            if (this.Style.isMonospace)
            {
                this.bold.Checked = false;
                this.italic.Checked = false;
                this.bold.Enabled = false;
                this.italic.Enabled = false;
                this.Style.isNormal = false;
                this.Style.isScript = false;
                this.Style.isFractur = false;
                this.Style.isSans = false;
                this.Style.isDoubleStruck = false;
            }
        }

        private void OnDoubleStruck(object sender, EventArgs e)
        {
            this.isDoubleStruck = this.doubleStruck.Checked;
            this.Style.isDoubleStruck = this.isDoubleStruck;
            if (this.Style.isDoubleStruck)
            {
                this.bold.Checked = false;
                this.italic.Checked = false;
                this.bold.Enabled = false;
                this.italic.Enabled = false;
                this.Style.isNormal = false;
                this.Style.isFractur = false;
                this.Style.isSans = false;
                this.Style.isScript = false;
                this.Style.isMonospace = false;
            }
        }

        private void OnNormal(object sender, EventArgs e)
        {
            this.isNormal = this.normalVar.Checked;
            this.Style.isNormal = this.isNormal;
            if (this.Style.isNormal)
            {
                this.bold.Checked = false;
                this.italic.Checked = false;
                this.Style.isBold = false;
                this.Style.isItalic = false;
                this.Style.isDoubleStruck = false;
                this.Style.isFractur = false;
                this.Style.isSans = false;
                this.Style.isScript = false;
                this.Style.isMonospace = false;
            }
        }

        private void OnDefault(object sender, EventArgs e)
        {
            if (this.notSet.Checked)
            {
                this.bold.Checked = false;
                this.italic.Checked = false;
                this.Style.isBold = false;
                this.Style.isItalic = false;
                this.Style.isNormal = false;
                this.Style.isDoubleStruck = false;
                this.Style.isFractur = false;
                this.Style.isSans = false;
                this.Style.isScript = false;
                this.Style.isMonospace = false;
            }
        }

        private void OnScriptLevelChanged(object sender, EventArgs e)
        {
            if (this.hasScriptLevel)
            {
                try
                {
                    switch (this.scriptLevel.SelectedIndex)
                    {
                        case 0:
                        {
                            this.Style.scriptLevel = ScriptLevel.NONE;
                            return;
                        }
                        case 1:
                        {
                            this.Style.scriptLevel = ScriptLevel.ZERO;
                            return;
                        }
                        case 2:
                        {
                            this.Style.scriptLevel = ScriptLevel.ONE;
                            return;
                        }
                        case 3:
                        {
                            this.Style.scriptLevel = ScriptLevel.TWO;
                            return;
                        }
                        case 4:
                        {
                            this.Style.scriptLevel = ScriptLevel.PLUS_ONE;
                            return;
                        }
                        case 5:
                        {
                            this.Style.scriptLevel = ScriptLevel.PLUS_TWO;
                            return;
                        }
                        case 6:
                        {
                            this.Style.scriptLevel = ScriptLevel.MINUS_ONE;
                            return;
                        }
                        case 7:
                        {
                            this.Style.scriptLevel = ScriptLevel.MINUS_TWO;
                            return;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void okClicked(object sender, EventArgs e)
        {
            this.Success = true;
            base.Close();
        }

        private void OnDispStyle(object sender, EventArgs e)
        {
            if (this.hasScriptLevel)
            {
                try
                {
                    switch (this.dispStyle.SelectedIndex)
                    {
                        case 0:
                        {
                            this.Style.displayStyle = DisplayStyle.AUTOMATIC;
                            return;
                        }
                        case 1:
                        {
                            this.Style.displayStyle = DisplayStyle.TRUE;
                            return;
                        }
                        case 2:
                        {
                            this.Style.displayStyle = DisplayStyle.FALSE;
                            return;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void OnBackColorSelect(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            try
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.Style.background = dialog.Color;
                    this.background = dialog.Color;
                    this.backColorPanel.BackColor = dialog.Color;
                }
            }
            catch
            {
                return;
            }
            finally
            {
                try
                {
                    dialog.Dispose();
                }
                catch
                {
                }
            }
        }

        private void cancelClicked(object sender, EventArgs e)
        {
            base.Close();
        }

        private void OnSmall(object sender, EventArgs e)
        {
            this.isSmall = this.smallVar.Checked;
            if (this.isSmall)
            {
                this.Style.hasSize = true;
                this.Style.size = "small";
                this.sizeEdit.Text = "small";
            }
        }

        private void OnNormalSize(object sender, EventArgs e)
        {
            this.isNormalVar = this.normalVar.Checked;
            if (this.isNormalVar)
            {
                this.Style.scale = 1;
                this.Style.size = "";
                this.Style.hasSize = false;
            }
            this.sizeEdit.Text = this.Style.FontSize();
        }

        private void OnBig(object sender, EventArgs e)
        {
            this.isBig = this.bigVar.Checked;
            if (this.isBig)
            {
                this.Style.hasSize = true;
                this.Style.size = "big";
                this.sizeEdit.Text = "big";
            }
        }


        public bool Success
        {
            get
            {
                return this.success;
            }
            set
            {
                this.success = value;
            }
        }


        private GroupBox foreColor;
        private Panel foreColorPanel;
        public Color background;
        public bool isBold;
        public bool isItalic;
        public bool isFraktur;
        public bool isNormal;
        public bool isDoubleStruck;
        public bool isScript;
        public bool isMonospace;
        public bool isSans;
        public bool isBig;
        private Glass.GlassButton seleColor;
        public bool isNormalVar;
        public bool isSmall;
        private bool success;
        private CheckBox bold;
        private CheckBox italic;
        private GroupBox group1;
        private GroupBox group22;
        private RadioButton smallVar;
        private RadioButton normalVar;
        private RadioButton bigVar;
        private GroupBox backColor;
        private RadioButton sansSerif;
        private RadioButton script;
        private RadioButton fraktur;
        private RadioButton monospace;
        private RadioButton normal;
        private RadioButton doubleStruck;
        private TextBox sizeEdit;
        private RadioButton notSet;
        private ComboBox scriptLevel;
        public StyleAttributes Style;
        private Glass.GlassButton backColorSelect;
        private GroupBox group3;
        private Label label2;
        private Label label1;
        private ComboBox dispStyle;
        private bool hasScriptLevel;
        private Panel backColorPanel;
        private Glass.GlassButton cancel;
        private Glass.GlassButton ok;
        private Container container;
        public Color color;
    }
}


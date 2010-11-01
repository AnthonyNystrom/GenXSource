namespace Facade
{
    using Attrs;
    using Nodes;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    internal class FractionsPropertiesDialog : Form
    {
        public FractionsPropertiesDialog(Node node)
        {
            this.success_ = false;
            this.container = null;
            this.InitializeComponent();
            if (node.box != null)
            {
                this.FractionAttrs = AttributeBuilder.FractionAttrsFromNode(node);
                if (this.FractionAttrs != null)
                {
                    this.isBevelled_.Checked = this.FractionAttrs.isBevelled;
                    this.lineThick.Value = (decimal) this.FractionAttrs.lineThickness;
                    if (this.FractionAttrs.numAlign == FractionAlign.LEFT)
                    {
                        this.numalignLeft_.Checked = true;
                    }
                    else if (this.FractionAttrs.numAlign == FractionAlign.CENTER)
                    {
                        this.numalignCenter_.Checked = true;
                    }
                    else if (this.FractionAttrs.numAlign == FractionAlign.RIGHT)
                    {
                        this.numalignRight_.Checked = true;
                    }
                    if (this.FractionAttrs.denomAlign == FractionAlign.LEFT)
                    {
                        this.denomalignLeft_.Checked = true;
                    }
                    else if (this.FractionAttrs.denomAlign == FractionAlign.CENTER)
                    {
                        this.denomAlignCenter_.Checked = true;
                    }
                    else if (this.FractionAttrs.denomAlign == FractionAlign.RIGHT)
                    {
                        this.denomalignRight_.Checked = true;
                    }
                }
                else
                {
                    this.FractionAttrs = new FractionAttributes();
                }
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FractionsPropertiesDialog));
            this.lineThick = new System.Windows.Forms.NumericUpDown();
            this.group1 = new System.Windows.Forms.GroupBox();
            this.numalignLeft_ = new System.Windows.Forms.RadioButton();
            this.numalignCenter_ = new System.Windows.Forms.RadioButton();
            this.numalignRight_ = new System.Windows.Forms.RadioButton();
            this.group2 = new System.Windows.Forms.GroupBox();
            this.denomalignLeft_ = new System.Windows.Forms.RadioButton();
            this.denomAlignCenter_ = new System.Windows.Forms.RadioButton();
            this.denomalignRight_ = new System.Windows.Forms.RadioButton();
            this.isBevelled_ = new System.Windows.Forms.CheckBox();
            this.group3 = new System.Windows.Forms.GroupBox();
            this.group4 = new System.Windows.Forms.GroupBox();
            this.cancelButton = new Glass.GlassButton();
            this.okButton = new Glass.GlassButton();
            ((System.ComponentModel.ISupportInitialize)(this.lineThick)).BeginInit();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            this.group4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lineThick
            // 
            this.lineThick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lineThick, "lineThick");
            this.lineThick.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.lineThick.Name = "lineThick";
            this.lineThick.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lineThick.ValueChanged += new System.EventHandler(this.OnLineThick);
            // 
            // group1
            // 
            this.group1.Controls.Add(this.numalignLeft_);
            this.group1.Controls.Add(this.numalignCenter_);
            this.group1.Controls.Add(this.numalignRight_);
            this.group1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group1, "group1");
            this.group1.Name = "group1";
            this.group1.TabStop = false;
            // 
            // numalignLeft_
            // 
            resources.ApplyResources(this.numalignLeft_, "numalignLeft_");
            this.numalignLeft_.Name = "numalignLeft_";
            this.numalignLeft_.CheckedChanged += new System.EventHandler(this.numLeftClick);
            // 
            // numalignCenter_
            // 
            this.numalignCenter_.Checked = true;
            resources.ApplyResources(this.numalignCenter_, "numalignCenter_");
            this.numalignCenter_.Name = "numalignCenter_";
            this.numalignCenter_.TabStop = true;
            this.numalignCenter_.CheckedChanged += new System.EventHandler(this.numCenterClick);
            // 
            // numalignRight_
            // 
            resources.ApplyResources(this.numalignRight_, "numalignRight_");
            this.numalignRight_.Name = "numalignRight_";
            this.numalignRight_.CheckedChanged += new System.EventHandler(this.numRightclick);
            // 
            // group2
            // 
            this.group2.Controls.Add(this.denomalignLeft_);
            this.group2.Controls.Add(this.denomAlignCenter_);
            this.group2.Controls.Add(this.denomalignRight_);
            this.group2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group2, "group2");
            this.group2.Name = "group2";
            this.group2.TabStop = false;
            // 
            // denomalignLeft_
            // 
            resources.ApplyResources(this.denomalignLeft_, "denomalignLeft_");
            this.denomalignLeft_.Name = "denomalignLeft_";
            this.denomalignLeft_.CheckedChanged += new System.EventHandler(this.denleftclick);
            // 
            // denomAlignCenter_
            // 
            this.denomAlignCenter_.Checked = true;
            resources.ApplyResources(this.denomAlignCenter_, "denomAlignCenter_");
            this.denomAlignCenter_.Name = "denomAlignCenter_";
            this.denomAlignCenter_.TabStop = true;
            this.denomAlignCenter_.CheckedChanged += new System.EventHandler(this.denCenterClick);
            // 
            // denomalignRight_
            // 
            resources.ApplyResources(this.denomalignRight_, "denomalignRight_");
            this.denomalignRight_.Name = "denomalignRight_";
            this.denomalignRight_.CheckedChanged += new System.EventHandler(this.denRightclick);
            // 
            // isBevelled_
            // 
            resources.ApplyResources(this.isBevelled_, "isBevelled_");
            this.isBevelled_.Name = "isBevelled_";
            this.isBevelled_.CheckedChanged += new System.EventHandler(this.x1);
            // 
            // group3
            // 
            this.group3.Controls.Add(this.isBevelled_);
            this.group3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group3, "group3");
            this.group3.Name = "group3";
            this.group3.TabStop = false;
            // 
            // group4
            // 
            this.group4.Controls.Add(this.lineThick);
            this.group4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group4, "group4");
            this.group4.Name = "group4";
            this.group4.TabStop = false;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.OnOk);
            // 
            // FractionsPropertiesDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.group4);
            this.Controls.Add(this.group3);
            this.Controls.Add(this.group2);
            this.Controls.Add(this.group1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FractionsPropertiesDialog";
            ((System.ComponentModel.ISupportInitialize)(this.lineThick)).EndInit();
            this.group1.ResumeLayout(false);
            this.group2.ResumeLayout(false);
            this.group3.ResumeLayout(false);
            this.group4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void x1(object sender, EventArgs e)
        {
            this.FractionAttrs.isBevelled = this.isBevelled_.Checked;
        }

        private void denRightclick(object sender, EventArgs e)
        {
            this.DenomCheck();
        }

        private void OnOk(object sender, EventArgs e)
        {
            this.Success = true;
            base.Close();
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            base.Close();
        }

        private void OnLineThick(object sender, EventArgs e)
        {
            this.FractionAttrs.lineThickness = (int) this.lineThick.Value;
        }

        private void NumCheck()
        {
            if (this.numalignLeft_.Checked)
            {
                this.FractionAttrs.numAlign = FractionAlign.LEFT;
            }
            else if (this.numalignCenter_.Checked)
            {
                this.FractionAttrs.numAlign = FractionAlign.CENTER;
            }
            else if (this.numalignRight_.Checked)
            {
                this.FractionAttrs.numAlign = FractionAlign.RIGHT;
            }
        }

        private void DenomCheck()
        {
            if (this.denomalignLeft_.Checked)
            {
                this.FractionAttrs.denomAlign = FractionAlign.LEFT;
            }
            else if (this.denomAlignCenter_.Checked)
            {
                this.FractionAttrs.denomAlign = FractionAlign.CENTER;
            }
            else if (this.denomalignRight_.Checked)
            {
                this.FractionAttrs.denomAlign = FractionAlign.RIGHT;
            }
        }

        private void numLeftClick(object sender, EventArgs e)
        {
            this.NumCheck();
        }

        private void numCenterClick(object sender, EventArgs e)
        {
            this.NumCheck();
        }

        private void numRightclick(object sender, EventArgs e)
        {
            this.NumCheck();
        }

        private void denleftclick(object sender, EventArgs e)
        {
            this.DenomCheck();
        }

        private void denCenterClick(object sender, EventArgs e)
        {
            this.DenomCheck();
        }


        public bool Success
        {
            get
            {
                return this.success_;
            }
            set
            {
                this.success_ = value;
            }
        }


        private NumericUpDown lineThick;
        private GroupBox group1;
        private GroupBox group3;
        private GroupBox group4;
        private Glass.GlassButton cancelButton;
        private Glass.GlassButton okButton;
        private bool success_;
        private Container container;
        public FractionAttributes FractionAttrs;
        private RadioButton numalignLeft_;
        private RadioButton numalignCenter_;
        private RadioButton numalignRight_;
        private GroupBox group2;
        private RadioButton denomalignLeft_;
        private RadioButton denomAlignCenter_;
        private RadioButton denomalignRight_;
        private CheckBox isBevelled_;
    }
}


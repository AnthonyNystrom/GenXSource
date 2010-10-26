namespace UI
{
    using Attrs;
    using Nodes;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    internal class MActionDialog : Form
    {
        public MActionDialog(Node node)
        {
            this.container = null;
            this.success = false;
            this.node = null;
            if (node != null)
            {
                this.node = node;
                this.attributes = AttributeBuilder.ActionAttributes(node);
                if (this.attributes == null)
                {
                    this.attributes = new ActionAttributes();
                }
            }
            this.InitializeComponent();
            if (this.attributes.actionType == ActionType.StatusLine)
            {
                this.statusline.Checked = true;
                this.spinner.Enabled = false;
                this.spinner.Maximum = new decimal(1);
                this.statline.Enabled = true;
            }
            else if (this.attributes.actionType == ActionType.ToolTip)
            {
                this.tooltip.Checked = true;
                this.spinner.Enabled = false;
                this.spinner.Maximum = new decimal(1);
                this.statline.Enabled = true;
            }
            else if (this.attributes.actionType == ActionType.Highlight)
            {
                this.highlight.Checked = true;
                this.spinner.Enabled = false;
                this.spinner.Maximum = new decimal(1);
                this.statusline.Enabled = false;
                this.tooltip.Enabled = false;
                this.statline.Enabled = false;
                this.statline.Text = "";
            }
            else if (this.attributes.actionType == ActionType.Toggle)
            {
                this.toggle.Checked = true;
                this.statusline.Enabled = false;
                this.tooltip.Enabled = false;
                this.spinner.Enabled = true;
                this.spinner.Maximum = (decimal) node.numChildren;
                this.spinner.Value = (decimal) this.attributes.selection;
                this.statline.Enabled = false;
                this.statline.Text = "";
            }
            else
            {
                this.statusline.Checked = false;
                this.tooltip.Checked = false;
                this.toggle.Checked = false;
                this.highlight.Checked = false;
                this.spinner.Enabled = false;
                this.spinner.Maximum = new decimal(1);
                this.statline.Enabled = false;
                this.statline.Text = "";
            }
            if ((this.attributes.actionType == ActionType.StatusLine) || (this.attributes.actionType == ActionType.ToolTip))
            {
                string s = "";
                try
                {
                    if (node.HasChildren())
                    {
                        Node n = node.firstChild;
                        if ((n != null) && (n.nextSibling != null))
                        {
                            n = n.nextSibling;
                            if (n.type_.type == ElementType.Mtext)
                            {
                                s = n.literalText;
                            }
                        }
                    }
                }
                catch
                {
                }
                this.statline.Text = s;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MActionDialog));
            this.group1 = new System.Windows.Forms.GroupBox();
            this.toggle = new System.Windows.Forms.RadioButton();
            this.tooltip = new System.Windows.Forms.RadioButton();
            this.statusline = new System.Windows.Forms.RadioButton();
            this.highlight = new System.Windows.Forms.RadioButton();
            this.group2 = new System.Windows.Forms.GroupBox();
            this.statline = new System.Windows.Forms.TextBox();
            this.OkButton = new Glass.GlassButton();
            this.cancelButton = new Glass.GlassButton();
            this.group3 = new System.Windows.Forms.GroupBox();
            this.spinner = new System.Windows.Forms.NumericUpDown();
            this.label = new System.Windows.Forms.Label();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinner)).BeginInit();
            this.SuspendLayout();
            // 
            // group1
            // 
            this.group1.Controls.Add(this.toggle);
            this.group1.Controls.Add(this.tooltip);
            this.group1.Controls.Add(this.statusline);
            this.group1.Controls.Add(this.highlight);
            this.group1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group1, "group1");
            this.group1.Name = "group1";
            this.group1.TabStop = false;
            // 
            // toggle
            // 
            resources.ApplyResources(this.toggle, "toggle");
            this.toggle.Name = "toggle";
            this.toggle.CheckedChanged += new System.EventHandler(this.OntoggleChecked);
            // 
            // tooltip
            // 
            resources.ApplyResources(this.tooltip, "tooltip");
            this.tooltip.Name = "tooltip";
            this.tooltip.CheckedChanged += new System.EventHandler(this.OnTooltipChecked);
            // 
            // statusline
            // 
            resources.ApplyResources(this.statusline, "statusline");
            this.statusline.Name = "statusline";
            this.statusline.CheckedChanged += new System.EventHandler(this.OnStatLineChanged);
            // 
            // highlight
            // 
            resources.ApplyResources(this.highlight, "highlight");
            this.highlight.Name = "highlight";
            this.highlight.CheckedChanged += new System.EventHandler(this.OnHighlightChecked);
            // 
            // group2
            // 
            this.group2.Controls.Add(this.statline);
            this.group2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group2, "group2");
            this.group2.Name = "group2";
            this.group2.TabStop = false;
            // 
            // statline
            // 
            resources.ApplyResources(this.statline, "statline");
            this.statline.Name = "statline";
            this.statline.TextChanged += new System.EventHandler(this.OnSlineChanged);
            // 
            // OkButton
            // 
            resources.ApplyResources(this.OkButton, "OkButton");
            this.OkButton.Name = "OkButton";
            this.OkButton.Click += new System.EventHandler(this.OnOk);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.OnCancel);
            // 
            // group3
            // 
            this.group3.Controls.Add(this.spinner);
            this.group3.Controls.Add(this.label);
            this.group3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group3, "group3");
            this.group3.Name = "group3";
            this.group3.TabStop = false;
            // 
            // spinner
            // 
            this.spinner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.spinner, "spinner");
            this.spinner.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.spinner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinner.Name = "spinner";
            this.spinner.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinner.ValueChanged += new System.EventHandler(this.SpinnerChanged);
            // 
            // label
            // 
            resources.ApplyResources(this.label, "label");
            this.label.Name = "label";
            // 
            // MActionDialog
            // 
            this.AcceptButton = this.OkButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.group3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.group2);
            this.Controls.Add(this.group1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MActionDialog";
            this.group1.ResumeLayout(false);
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.group3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinner)).EndInit();
            this.ResumeLayout(false);

        }

        private void OnStatLineChanged(object sender, EventArgs e)
        {
            this.UpdateVals();
        }

        private void SpinnerChanged(object sender, EventArgs e)
        {
            if (this.attributes != null)
            {
                this.attributes.selection = (int) this.spinner.Value;
            }
        }

        private void OnTooltipChecked(object sender, EventArgs e)
        {
            this.UpdateVals();
        }

        private void OntoggleChecked(object sender, EventArgs e)
        {
            this.UpdateVals();
        }

        private void OnHighlightChecked(object sender, EventArgs e)
        {
            this.UpdateVals();
        }

        private void OnSlineChanged(object sender, EventArgs e)
        {
        }

        private void UpdateVals()
        {
            if (this.attributes != null)
            {
                if (this.tooltip.Checked)
                {
                    this.attributes.actionType = ActionType.ToolTip;
                    this.spinner.Value = new decimal(1);
                    this.spinner.Enabled = false;
                    this.spinner.Maximum = new decimal(1);
                    this.statline.Enabled = true;
                }
                else if (this.statusline.Checked)
                {
                    this.attributes.actionType = ActionType.StatusLine;
                    this.spinner.Value = new decimal(1);
                    this.spinner.Enabled = false;
                    this.spinner.Maximum = new decimal(1);
                    this.statline.Enabled = true;
                }
                else if (this.toggle.Checked)
                {
                    this.attributes.actionType = ActionType.Toggle;
                    this.spinner.Value = (decimal) this.attributes.selection;
                    this.spinner.Enabled = true;
                    this.spinner.Maximum = (decimal) this.node.numChildren;
                    this.statline.Enabled = false;
                    this.statline.Text = "";
                }
                else if (this.highlight.Checked)
                {
                    this.attributes.actionType = ActionType.Highlight;
                    this.attributes.selection = 1;
                    this.spinner.Value = new decimal(1);
                    this.spinner.Enabled = false;
                    this.spinner.Maximum = new decimal(1);
                    this.statline.Enabled = false;
                    this.statline.Text = "";
                }
                else
                {
                    this.attributes.actionType = ActionType.Unknown;
                    this.attributes.selection = 1;
                    this.spinner.Value = new decimal(1);
                    this.spinner.Enabled = false;
                    this.spinner.Maximum = new decimal(1);
                    this.statline.Enabled = false;
                    this.statline.Text = "";
                }
            }
        }

        private void OnOk(object sender, EventArgs e)
        {
            this.Success = true;
            base.Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.Success = false;
            base.Close();
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

        public string Statusline
        {
            get
            {
                return this.statline.Text;
            }
        }


        private GroupBox group1;
        private GroupBox group2;
        private bool success;
        private GroupBox group3;
        private NumericUpDown spinner;
        private Label label;
        public ActionAttributes attributes;
        private Node node;
        private Glass.GlassButton OkButton;
        private RadioButton statusline;
        private RadioButton tooltip;
        private RadioButton toggle;
        private TextBox statline;
        private RadioButton highlight;
        private Glass.GlassButton cancelButton;
        private Container container;
    }
}


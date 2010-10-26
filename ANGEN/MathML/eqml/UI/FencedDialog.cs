namespace UI
{
    using Attrs;
    using Nodes;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    internal class FencedDialog : Form
    {
        public FencedDialog(Node node)
        {
            this.container = null;
            this.success = false;
            this.InitializeComponent();
            if (node.box != null)
            {
                this.FencedAttrs = AttributeBuilder.FencedAttrsFromNode(node);
                if (this.FencedAttrs == null)
                {
                    this.FencedAttrs = new FencedAttributes();
                }
                if (this.FencedAttrs != null)
                {
                    if (this.FencedAttrs.separators.Length > 0)
                    {
                        if (this.FencedAttrs.separators == "NONE")
                        {
                            this.separatorBox.Text = "";
                        }
                        else
                        {
                            this.separatorBox.Text = this.FencedAttrs.separators;
                        }
                    }
                    if (this.FencedAttrs.open.Length > 0)
                    {
                        if ((((this.FencedAttrs.open == "(") || (this.FencedAttrs.open == "{")) || ((this.FencedAttrs.open == "[") || (this.FencedAttrs.open == "|"))) || (((this.FencedAttrs.open == "<") || (this.FencedAttrs.open[0] == '\u2329')) || (this.FencedAttrs.open[0] == '<')))
                        {
                            if (this.FencedAttrs.open == "(")
                            {
                                this.lparen.Checked = true;
                            }
                            else if (this.FencedAttrs.open == "{")
                            {
                                this.lcurl.Checked = true;
                            }
                            else if (this.FencedAttrs.open == "[")
                            {
                                this.lbrack.Checked = true;
                            }
                            else if (this.FencedAttrs.open == "|")
                            {
                                this.lvert.Checked = true;
                            }
                            else if (((this.FencedAttrs.open == "<") || (this.FencedAttrs.open[0] == '\u2329')) || (this.FencedAttrs.open[0] == '<'))
                            {
                                this.lang.Checked = true;
                            }
                        }
                        else if (this.FencedAttrs.open == "NONE")
                        {
                            this.lnone.Checked = true;
                        }
                        else
                        {
                            this.lparen.Checked = false;
                            this.lcurl.Checked = false;
                            this.lbrack.Checked = false;
                            this.lvert.Checked = false;
                            this.lang.Checked = false;
                            this.lnone.Checked = false;
                        }
                    }
                    else
                    {
                        this.lparen.Checked = true;
                    }
                    if (this.FencedAttrs.close.Length > 0)
                    {
                        if ((((this.FencedAttrs.close == ")") || (this.FencedAttrs.close == "}")) || ((this.FencedAttrs.close == "]") || (this.FencedAttrs.close == "|"))) || (((this.FencedAttrs.close == ">") || (this.FencedAttrs.close[0] == '\u232a')) || (this.FencedAttrs.close[0] == '>')))
                        {
                            if (this.FencedAttrs.close == ")")
                            {
                                this.rparen.Checked = true;
                            }
                            else if (this.FencedAttrs.close == "}")
                            {
                                this.rcurl.Checked = true;
                            }
                            else if (this.FencedAttrs.close == "]")
                            {
                                this.rbrack.Checked = true;
                            }
                            else if (this.FencedAttrs.close == "|")
                            {
                                this.rvert.Checked = true;
                            }
                            else if (((this.FencedAttrs.close == ">") || (this.FencedAttrs.close[0] == '\u232a')) || (this.FencedAttrs.close[0] == '>'))
                            {
                                this.rangle.Checked = true;
                            }
                        }
                        else if (this.FencedAttrs.close == "NONE")
                        {
                            this.rnone.Checked = true;
                        }
                        else
                        {
                            this.rparen.Checked = false;
                            this.rcurl.Checked = false;
                            this.rbrack.Checked = false;
                            this.rvert.Checked = false;
                            this.rangle.Checked = false;
                            this.rnone.Checked = false;
                        }
                    }
                    else
                    {
                        this.rparen.Checked = true;
                    }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FencedDialog));
            this.separatorBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new Glass.GlassButton();
            this.okButton = new Glass.GlassButton();
            this.lparen = new System.Windows.Forms.RadioButton();
            this.lvert = new System.Windows.Forms.RadioButton();
            this.lcurl = new System.Windows.Forms.RadioButton();
            this.lbrack = new System.Windows.Forms.RadioButton();
            this.group1 = new System.Windows.Forms.GroupBox();
            this.lnone = new System.Windows.Forms.RadioButton();
            this.lang = new System.Windows.Forms.RadioButton();
            this.group2 = new System.Windows.Forms.GroupBox();
            this.rnone = new System.Windows.Forms.RadioButton();
            this.rcurl = new System.Windows.Forms.RadioButton();
            this.rbrack = new System.Windows.Forms.RadioButton();
            this.rparen = new System.Windows.Forms.RadioButton();
            this.rvert = new System.Windows.Forms.RadioButton();
            this.rangle = new System.Windows.Forms.RadioButton();
            this.group3 = new System.Windows.Forms.GroupBox();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // separatorBox
            // 
            resources.ApplyResources(this.separatorBox, "separatorBox");
            this.separatorBox.Name = "separatorBox";
            this.separatorBox.TextChanged += new System.EventHandler(this.OnSepChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.OnCancel);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.OnOk);
            // 
            // lparen
            // 
            resources.ApplyResources(this.lparen, "lparen");
            this.lparen.Name = "lparen";
            this.lparen.CheckedChanged += new System.EventHandler(this.OnLeft);
            // 
            // lvert
            // 
            resources.ApplyResources(this.lvert, "lvert");
            this.lvert.Name = "lvert";
            this.lvert.CheckedChanged += new System.EventHandler(this.OnLeft);
            // 
            // lcurl
            // 
            resources.ApplyResources(this.lcurl, "lcurl");
            this.lcurl.Name = "lcurl";
            this.lcurl.CheckedChanged += new System.EventHandler(this.OnLeft);
            // 
            // lbrack
            // 
            resources.ApplyResources(this.lbrack, "lbrack");
            this.lbrack.Name = "lbrack";
            this.lbrack.CheckedChanged += new System.EventHandler(this.OnLeft);
            // 
            // group1
            // 
            this.group1.Controls.Add(this.lnone);
            this.group1.Controls.Add(this.lcurl);
            this.group1.Controls.Add(this.lbrack);
            this.group1.Controls.Add(this.lparen);
            this.group1.Controls.Add(this.lvert);
            this.group1.Controls.Add(this.lang);
            this.group1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group1, "group1");
            this.group1.Name = "group1";
            this.group1.TabStop = false;
            // 
            // lnone
            // 
            resources.ApplyResources(this.lnone, "lnone");
            this.lnone.Name = "lnone";
            this.lnone.CheckedChanged += new System.EventHandler(this.OnLeft);
            // 
            // lang
            // 
            resources.ApplyResources(this.lang, "lang");
            this.lang.Name = "lang";
            this.lang.CheckedChanged += new System.EventHandler(this.OnLeft);
            // 
            // group2
            // 
            this.group2.Controls.Add(this.rnone);
            this.group2.Controls.Add(this.rcurl);
            this.group2.Controls.Add(this.rbrack);
            this.group2.Controls.Add(this.rparen);
            this.group2.Controls.Add(this.rvert);
            this.group2.Controls.Add(this.rangle);
            this.group2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group2, "group2");
            this.group2.Name = "group2";
            this.group2.TabStop = false;
            // 
            // rnone
            // 
            resources.ApplyResources(this.rnone, "rnone");
            this.rnone.Name = "rnone";
            this.rnone.CheckedChanged += new System.EventHandler(this.OnRight);
            // 
            // rcurl
            // 
            resources.ApplyResources(this.rcurl, "rcurl");
            this.rcurl.Name = "rcurl";
            this.rcurl.CheckedChanged += new System.EventHandler(this.OnRight);
            // 
            // rbrack
            // 
            resources.ApplyResources(this.rbrack, "rbrack");
            this.rbrack.Name = "rbrack";
            this.rbrack.CheckedChanged += new System.EventHandler(this.OnRight);
            // 
            // rparen
            // 
            resources.ApplyResources(this.rparen, "rparen");
            this.rparen.Name = "rparen";
            this.rparen.CheckedChanged += new System.EventHandler(this.OnRight);
            // 
            // rvert
            // 
            resources.ApplyResources(this.rvert, "rvert");
            this.rvert.Name = "rvert";
            this.rvert.CheckedChanged += new System.EventHandler(this.OnRight);
            // 
            // rangle
            // 
            resources.ApplyResources(this.rangle, "rangle");
            this.rangle.Name = "rangle";
            this.rangle.CheckedChanged += new System.EventHandler(this.OnRight);
            // 
            // group3
            // 
            this.group3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group3, "group3");
            this.group3.Name = "group3";
            this.group3.TabStop = false;
            // 
            // FencedDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.group2);
            this.Controls.Add(this.group1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.separatorBox);
            this.Controls.Add(this.group3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FencedDialog";
            this.Load += new System.EventHandler(this.OnLoad);
            this.group1.ResumeLayout(false);
            this.group2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CheckLefts()
        {
            if (this.lparen.Checked)
            {
                this.FencedAttrs.open = "(";
            }
            else if (this.lcurl.Checked)
            {
                this.FencedAttrs.open = "{";
            }
            else if (this.lbrack.Checked)
            {
                this.FencedAttrs.open = "[";
            }
            else if (this.lvert.Checked)
            {
                this.FencedAttrs.open = "|";
            }
            else if (this.lang.Checked)
            {
                this.FencedAttrs.open = "" + '\u2329';
            }
            else if (this.lnone.Checked)
            {
                this.FencedAttrs.open = "NONE";
            }
        }

        private void OnRight(object sender, EventArgs e)
        {
            this.CheckRights();
        }
        
        private void OnSepChanged(object sender, EventArgs e)
        {
            if (this.separatorBox.Text == "")
            {
                this.FencedAttrs.separators = "NONE";
            }
            else
            {
                this.FencedAttrs.separators = this.separatorBox.Text;
            }
        }

        private void CheckRights()
        {
            if (this.rparen.Checked)
            {
                this.FencedAttrs.close = ")";
            }
            else if (this.rcurl.Checked)
            {
                this.FencedAttrs.close = "}";
            }
            else if (this.rbrack.Checked)
            {
                this.FencedAttrs.close = "]";
            }
            else if (this.rvert.Checked)
            {
                this.FencedAttrs.close = "|";
            }
            else if (this.rangle.Checked)
            {
                this.FencedAttrs.close = "" + '\u232a';
            }
            else if (this.rnone.Checked)
            {
                this.FencedAttrs.close = "NONE";
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
        }

        private void OnOk(object sender, EventArgs e)
        {
            this.Success = true;
            base.Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            base.Close();
        }

        private void OnLeft(object sender, EventArgs e)
        {
            this.CheckLefts();
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

        private TextBox separatorBox;
        private Glass.GlassButton cancelButton;
        private RadioButton lang;
        private RadioButton rcurl;
        private RadioButton rbrack;
        private RadioButton rparen;
        private RadioButton rvert;
        private RadioButton rangle;
        private Container container;
        public FencedAttributes FencedAttrs;
        private RadioButton lnone;
        private RadioButton rnone;
        private Glass.GlassButton okButton;
        private bool success;
        private GroupBox group1;
        private GroupBox group2;
        private GroupBox group3;
        private RadioButton lparen;
        private RadioButton lvert;
        private RadioButton lcurl;
        private RadioButton lbrack;
    }
}


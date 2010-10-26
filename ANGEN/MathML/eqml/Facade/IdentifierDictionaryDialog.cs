namespace Facade
{
    using Fonts;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Windows.Forms;

    internal class IdentifierDictionaryDialog : Form
    {
        public IdentifierDictionaryDialog(EntityManager EntityManager, FontCollection FontCollection, bool bIdentifier)
        {
            this.container = null;
            this.curEntityIndex_ = 0;
            this.numEntities_ = 0;
            this.success_ = false;
            this.ready_ = false;
            this.fonts_ = null;
            this.InitializeComponent();

            this.fonts_ = FontCollection;
            this.entManager_ = EntityManager;

            this.entities_.DoubleClick += new EventHandler(this.OnEntitiesDoubleClick);
            this.uniCats_   .ItemCheck += new ItemCheckEventHandler(this.OnUniItemCheck);
            if (bIdentifier)
            {
                this.isitalic_.Visible = true;
                this.isbold_.Visible = true;
                this.isitalic_.Checked = true;
                this.isbold_.Checked = false;
                
                this.Text = "Insert Identifier";
                this.items_ = this.entManager_.ids_;
                this.isID_.Checked = true;
                this.mo_.Checked = false;
            }
            else
            {
                this.isitalic_.Visible = false;
                this.isbold_.Visible = false;
                
                this.Text = "Insert Operator";
                this.items_ = this.entManager_.ops_;
                this.isID_.Checked = false;
                this.mo_.Checked = true;
            }
            this.cheader.Width = this.uniCats_.Size.Width - 20;
            this.FillEntities();
            this.FillCats();
            this.ready_ = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.container != null))
            {
                this.container.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                if (this.glyph_ == null)
                {
                    return;
                }
                this.unicodeBox.Text = this.glyph_.Code;
                string s =  "" + this.glyph_.CharValue;
                int x = this.label2.Location.X;
                int y = (this.label2.Location.Y + this.label2.Size.Height) + 5;
                int width = 0x2c;
                int height = 0x2c;
                this.descrBox.Text = this.glyph_.Description;
                e.Graphics.FillRectangle(Brushes.White, x, y, width, height);
                if (this.glyph_.Code == "0222C")
                {
                    s = s + s;
                }
                else if (this.glyph_.Code == "0222D")
                {
                    s = s + s + s;
                }
                try
                {
                    e.Graphics.DrawString(s, this.GetFont(), Brushes.Black, (float) (x + 1), (float) (y + 1));
                }
                catch
                {
                }
                this.unicodeCatBox.Text = this.glyph_.Category.Name;
                if (this.glyph_.FontFamily.Length > 0)
                {
                    this.fontBox.Text = this.glyph_.FontFamily;
                }
                else
                {
                    this.fontBox.Text = "Not found in installed fonts";
                }
                
            }
            catch
            {
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                if (this.entities_.SelectedIndex > 0)
                {
                    this.entities_.SelectedIndex--;
                }
            }
            else if (keyData == Keys.Down)
            {
                if ((this.entities_.SelectedIndex + 1) < this.numEntities_)
                {
                    this.entities_.SelectedIndex++;
                }
            }
            else if (keyData == Keys.Prior)
            {
                if ((this.entities_.SelectedIndex - 20) >= 0)
                {
                    this.entities_.SelectedIndex -= 20;
                }
                else
                {
                    this.entities_.SelectedIndex = 0;
                }
            }
            else if (keyData == Keys.Next)
            {
                if ((this.entities_.SelectedIndex + 20) < this.numEntities_)
                {
                    this.entities_.SelectedIndex += 20;
                }
                else
                {
                    this.entities_.SelectedIndex = this.numEntities_ - 1;
                }
            }
            else if (keyData == Keys.End)
            {
                this.entities_.SelectedIndex = this.numEntities_ - 1;
            }
            else if (keyData == Keys.Home)
            {
                this.entities_.SelectedIndex = 0;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Return)
            {
                this.success_ = true;
                base.Close();
            }
            else if (keyData == Keys.Escape)
            {
                base.Close();
            }
            else if (keyData == Keys.Insert)
            {
                this.success_ = true;
                base.Close();
            }
            else if (keyData == (Keys.Control | Keys.I))
            {
                if (this.isID_.Checked)
                {
                    if (this.isitalic_.Checked)
                    {
                        this.isitalic_.Checked = false;
                    }
                    else
                    {
                        this.isitalic_.Checked = true;
                    }
                }
            }
            else
            {
                if (keyData != (Keys.Control | Keys.B))
                {
                    return base.ProcessDialogKey(keyData);
                }
                if (this.isID_.Checked)
                {
                    if (this.isbold_.Checked)
                    {
                        this.isbold_.Checked = false;
                    }
                    else
                    {
                        this.isbold_.Checked = true;
                    }
                }
            }
            return true;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IdentifierDictionaryDialog));
            this.searchBox = new System.Windows.Forms.TextBox();
            this.okButton = new Glass.GlassButton();
            this.cancelButton = new Glass.GlassButton();
            this.descrBox = new System.Windows.Forms.TextBox();
            this.mo_ = new System.Windows.Forms.RadioButton();
            this.isID_ = new System.Windows.Forms.RadioButton();
            this.entities_ = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.unicodeCatBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.fontBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.unicodeBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.excludeEntities = new System.Windows.Forms.CheckBox();
            this.uniCats_ = new System.Windows.Forms.ListView();
            this.cheader = new System.Windows.Forms.ColumnHeader();
            this.label7 = new System.Windows.Forms.Label();
            this.group1 = new System.Windows.Forms.GroupBox();
            this.isitalic_ = new System.Windows.Forms.CheckBox();
            this.isbold_ = new System.Windows.Forms.CheckBox();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchBox
            // 
            resources.ApplyResources(this.searchBox, "searchBox");
            this.searchBox.Name = "searchBox";
            this.searchBox.TextChanged += new System.EventHandler(this.OnSearch);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.OnOk);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.OnCancel);
            // 
            // descrBox
            // 
            this.descrBox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.descrBox, "descrBox");
            this.descrBox.Name = "descrBox";
            this.descrBox.ReadOnly = true;
            // 
            // mo_
            // 
            resources.ApplyResources(this.mo_, "mo_");
            this.mo_.Name = "mo_";
            this.mo_.CheckedChanged += new System.EventHandler(this.OnMoChecked);
            // 
            // isID_
            // 
            resources.ApplyResources(this.isID_, "isID_");
            this.isID_.Name = "isID_";
            this.isID_.CheckedChanged += new System.EventHandler(this.OnMiChecked);
            // 
            // entities_
            // 
            this.entities_.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.entities_, "entities_");
            this.entities_.Name = "entities_";
            this.entities_.SelectedIndexChanged += new System.EventHandler(this.EntitiesChanged);
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // unicodeCatBox
            // 
            this.unicodeCatBox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.unicodeCatBox, "unicodeCatBox");
            this.unicodeCatBox.Name = "unicodeCatBox";
            this.unicodeCatBox.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // fontBox
            // 
            this.fontBox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.fontBox, "fontBox");
            this.fontBox.Name = "fontBox";
            this.fontBox.ReadOnly = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // unicodeBox
            // 
            this.unicodeBox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.unicodeBox, "unicodeBox");
            this.unicodeBox.Name = "unicodeBox";
            this.unicodeBox.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // excludeEntities
            // 
            this.excludeEntities.Checked = true;
            this.excludeEntities.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.excludeEntities, "excludeEntities");
            this.excludeEntities.Name = "excludeEntities";
            this.excludeEntities.CheckedChanged += new System.EventHandler(this.OnExclude);
            // 
            // uniCats_
            // 
            this.uniCats_.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.uniCats_.CheckBoxes = true;
            this.uniCats_.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cheader});
            this.uniCats_.FullRowSelect = true;
            this.uniCats_.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            resources.ApplyResources(this.uniCats_, "uniCats_");
            this.uniCats_.MultiSelect = false;
            this.uniCats_.Name = "uniCats_";
            this.uniCats_.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.uniCats_.UseCompatibleStateImageBehavior = false;
            this.uniCats_.View = System.Windows.Forms.View.Details;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // group1
            // 
            this.group1.Controls.Add(this.uniCats_);
            this.group1.Controls.Add(this.label7);
            this.group1.Controls.Add(this.excludeEntities);
            this.group1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group1, "group1");
            this.group1.Name = "group1";
            this.group1.TabStop = false;
            // 
            // isitalic_
            // 
            resources.ApplyResources(this.isitalic_, "isitalic_");
            this.isitalic_.Name = "isitalic_";
            this.isitalic_.CheckedChanged += new System.EventHandler(this.ItalicChecked);
            // 
            // isbold_
            // 
            resources.ApplyResources(this.isbold_, "isbold_");
            this.isbold_.Name = "isbold_";
            this.isbold_.CheckedChanged += new System.EventHandler(this.BoldChanged);
            // 
            // IdentifierDictionaryDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.unicodeBox);
            this.Controls.Add(this.fontBox);
            this.Controls.Add(this.unicodeCatBox);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.descrBox);
            this.Controls.Add(this.isbold_);
            this.Controls.Add(this.isitalic_);
            this.Controls.Add(this.group1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.entities_);
            this.Controls.Add(this.isID_);
            this.Controls.Add(this.mo_);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IdentifierDictionaryDialog";
            this.group1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void FillCats()
        {
            this.uniCats_.Items.Clear();
            for (int i = 0; i < this.items_.Count; i++)
            {
                MapItem item = this.items_.Get(i);
                if ((this.excludeEntities.Checked && (item.ref2 > 1)) || (!this.excludeEntities.Checked && (item.ref1 > 1)))
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = item.Name;
                    listViewItem.Tag = item;
                    listViewItem.Checked = item.IsActive;
                    if (listViewItem.Checked)
                    {
                        listViewItem.ForeColor = Color.Black;
                    }
                    else
                    {
                        listViewItem.ForeColor = Color.Gray;
                    }
                    this.uniCats_.Items.Add(listViewItem);
                }
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            base.Close();
        }

        private void OnEntitiesDoubleClick(object sender, EventArgs e)
        {
            this.success_ = true;
            base.Close();
        }

        private void OnMiChecked(object sender, EventArgs e)
        {
            if (this.ready_)
            {
                if (this.isID_.Checked)
                {
                    this.isitalic_.Visible = true;
                    this.isbold_.Visible = true;
                    
                    this.isitalic_.Checked = true;
                    this.isbold_.Checked = false;
                }
                else
                {
                    this.isitalic_.Visible = false;
                    this.isbold_.Visible = false;
                    this.isitalic_.Checked = false;
                    this.isbold_.Checked = false;
                    
                }
            }
            base.Invalidate();
        }

        private void OnMoChecked(object sender, EventArgs e)
        {
            if (this.ready_)
            {
                if (this.isID_.Checked)
                {
                    this.isitalic_.Visible = true;
                    this.isbold_.Visible = true;
                    this.isitalic_.Checked = true;
                    this.isbold_.Checked = false;
                    
                }
                else
                {
                    this.isitalic_.Visible = false;
                    this.isbold_.Visible = false;
                    this.isitalic_.Checked = false;
                    this.isbold_.Checked = false;
                    
                }
            }
            base.Invalidate();
        }

        private void ItalicChecked(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        private void BoldChanged(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        private void FillEntities()
        {
            try
            {
                this.entities_.Items.Clear();
                this.numEntities_ = 0;
                int count = this.entManager_.Count;
                for (int i = 0; i < count; i++)
                {
                    Glyph glyph = this.entManager_.Get(i);
                    if (glyph != null)
                    {
                        if (glyph.IsVisible)
                        {
                            bool noExcelude = false;
                            if (glyph.Category != null)
                            {
                                if (this.items_.Find(glyph.Category.ID))
                                {
                                    if (!this.excludeEntities.Checked)
                                    {
                                        noExcelude = true;
                                    }
                                    else if (glyph.FontFamily.Length > 0)
                                    {
                                        noExcelude = true;
                                    }
                                }
                            }
                            else if (!this.excludeEntities.Checked)
                            {
                                noExcelude = true;
                            }
                            else if (glyph.FontFamily.Length > 0)
                            {
                                noExcelude = true;
                            }
                            if (noExcelude)
                            {
                                this.entities_.Items.Add(glyph.Name);
                                this.numEntities_++;
                            }
                        }
                    }
                }
                if (this.entities_.Items.Count > 0)
                {
                    this.entities_.SelectedIndex = 0;
                }
                this.entities_.Focus();
            }
            catch
            {
            }
        }

        public Glyph GetGlyph()
        {
            return this.glyph_;
        }

        public Font GetFont()
        {
            string s = "";
            Font font = null;
            FontFamily family = null;
            FontStyle fontStyle = FontStyle.Regular;
            if (this.glyph_ == null)
            {
                return font;
            }
            s = this.glyph_.FontFamily;
            if (s.Length <= 0)
            {
                return font;
            }
            if (s == "ESSTIXNine")
            {
                if (!this.isitalic_.Checked)
                {
                    s = "ESSTIXTen";
                }
            }
            else if (s == "ESSTIXTen")
            {
                if (this.isitalic_.Checked)
                {
                    s = "ESSTIXNine";
                }
            }
            else if (this.isitalic_.Checked)
            {
                fontStyle |= FontStyle.Italic;
            }
            if (this.isbold_.Checked)
            {
                fontStyle |= FontStyle.Bold;
            }
            if (this.fonts_ != null)
            {
                family = this.fonts_.Get(s);
            }
            if (family != null)
            {
                return new Font(family, 20f, fontStyle);
            }
            return new Font(s, 20f, fontStyle);
        }

        private void EntitiesChanged(object sender, EventArgs e)
        {
            try
            {
                this.curEntityIndex_ = this.entities_.SelectedIndex;
                string s = this.entities_.Items[this.curEntityIndex_].ToString();
                Glyph glyph = this.entManager_.ByName(s);
                if (glyph != null)
                {
                    this.glyph_ = glyph;
                    try
                    {
                        if (this.isID_.Checked)
                        {
                            if ((s.Length >= 3) && ((char.IsUpper(this.glyph_.CharValue) && (this.glyph_.Category.Name == "Greek")) || (char.IsUpper(s, 0) && (((s.Substring(s.Length - 3, 3) == "opf") || (s.Substring(s.Length - 3, 3) == "scr")) || (s.Substring(s.Length - 2, 2) == "fr")))))
                            {
                                this.isitalic_.Checked = false;
                            }
                            else if ((s == "plankv") || (s == "ell"))
                            {
                                this.isitalic_.Checked = false;
                            }
                            else
                            {
                                this.isitalic_.Checked = true;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                base.Invalidate();
            }
            catch
            {
            }
        }

        private void OnExclude(object sender, EventArgs e)
        {
            if (this.ready_)
            {
                this.ready_ = false;
                try
                {
                    this.FillEntities();
                    this.FillCats();
                }
                catch
                {
                }
                this.ready_ = true;
            }
        }

        private void OnUniItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.ready_)
            {
                bool v = false;
                
                ListViewItem item = null;
                MapItem mapItem = null;
                item = this.uniCats_.Items[e.Index];
                if (item != null)
                {
                    v = Convert.ToBoolean(e.NewValue);
                    if (v)
                    {
                        item.ForeColor = Color.Black;
                    }
                    else
                    {
                        item.ForeColor = Color.Gray;
                    }
                    mapItem = (MapItem) item.Tag;
                    if (mapItem != null)
                    {
                        mapItem.IsActive = v;
                        this.FillEntities();
                    }
                }
            }
        }

        private void OnSearch(object sender, EventArgs e)
        {
            try
            {
                int index = this.entities_.FindString(this.searchBox.Text);
                if ((index >= 0) && (index < this.numEntities_))
                {
                    this.entities_.SelectedIndex = index;
                }
            }
            catch
            {
            }
        }

        private void OnOk(object sender, EventArgs e)
        {
            this.success_ = true;
            base.Close();
        }


        public bool IsID
        {
            get
            {
                if (this.isID_.Checked)
                {
                    return true;
                }
                return false;
            }
        }

        public bool Success
        {
            get
            {
                return this.success_;
            }
        }

        public bool IsItalic
        {
            get
            {
                try
                {
                    return this.isitalic_.Checked;
                }
                catch
                {
					return false;
                }
            }
        }

        public bool IsBold
        {
            get
            {
                try
                {
                    return this.isbold_.Checked;
                }
                catch
                {
					return false;
                }
            }
        }


        private Container container;
        private int curEntityIndex_;
        private EntityManager entManager_;
        private ListBox entities_;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox unicodeCatBox;
        private Label label4;
        private TextBox fontBox;
        private Label label5;
        private TextBox unicodeBox;
        private int numEntities_;
        private Label label6;
        private Glyph glyph_;
        private ListView uniCats_;
        private ColumnHeader cheader;
        private Label label7;
        private bool ready_;
        private CheckBox excludeEntities;
        private GroupBox group1;
        private MapItems items_;
        private bool success_;
        private CheckBox isitalic_;
        private CheckBox isbold_;
        private Glass.GlassButton okButton;
        private FontCollection fonts_;
        private Glass.GlassButton cancelButton;
        private TextBox searchBox;
        private TextBox descrBox;
        private RadioButton mo_;
        private RadioButton isID_;
    }
}


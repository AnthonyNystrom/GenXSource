namespace UI
{
    using Fonts;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public delegate void OnChangeSelection(object sender, SelectionArgs e);

    internal class ControlWithMenu : CoreControl
    {
        public event MouseEventHandler Event_MouseDown;

        public ControlWithMenu(EntityManager MathMLEntityManager, FontCollection FontCollection) 
            : base(MathMLEntityManager, FontCollection)
        {
            this.designMode = false;
            this.InitializeComponent();
            this.CreateMenu();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                this.Event_MouseDown(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
            }
            catch
            {
            }
            if (((e.Button == MouseButtons.Right)) && !this.designMode)
            {
                this.PopupContextMenu(e.X, e.Y);
            }
            base.OnMouseDown(e);
        }

        private void OnRedo(object sender, EventArgs e)
        {
            base.Redo();
        }

        private void OnCut(object sender, EventArgs e)
        {
            base.Cut();
        }

        private void OnCopy(object sender, EventArgs e)
        {
            base.Copy();
        }

        private void OnPaste(object sender, EventArgs e)
        {
            base.Paste();
        }

        private void OnDelete(object sender, EventArgs e)
        {
            base.Delete();
        }

        private void OnFraction(object sender, EventArgs e)
        {
            base.InsertFraction();
        }

        private void OnSquareRoot(object sender, EventArgs e)
        {
            base.InsertSqrt();
        }

        private void OnRoot(object sender, EventArgs e)
        {
            base.InsertRoot();
        }

        private void OnMatrix(object sender, EventArgs e)
        {
            base.InsertMatrixDialog();
        }

        private void SuperscriptHandler(object sender, EventArgs e)
        {
            base.InsertSuperScript();
        }

        private void SubscriptHandler(object sender, EventArgs e)
        {
            base.InsertSubScript();
        }

        private void FencedHandler(object sender, EventArgs e)
        {
            base.InsertFenced();
        }

        private void MoHandler(object sender, EventArgs e)
        {
            base.InsertEntity_Open_IdentifierDictionary_Dialog(false);
        }

        private void MiHandler(object sender, EventArgs e)
        {
            base.InsertEntity_Open_IdentifierDictionary_Dialog(true);
        }

        private void MtextHandler(object sender, EventArgs e)
        {
            base.InsertText();
        }

        private void OnPhantom(object sender, EventArgs e)
        {
            base.InsertPhantom();
        }

        private void OnAction(object sender, EventArgs e)
        {
            base.InsertAction();
        }

        private void OnFractionProperties(object sender, EventArgs e)
        {
            base.FractionProperties();
        }

        private void OnFencedProperties(object sender, EventArgs e)
        {
            base.FencedProperties();
        }

        private void OnMatrixProperties(object sender, EventArgs e)
        {
            base.MatrixProperties();
        }

        private void OnActionProperties(object sender, EventArgs e)
        {
            base.ActionProperties();
        }

        private void OnStyleProperties(object sender, EventArgs e)
        {
            base.StyleProperties();
        }

        private void CreateMenu()
        {
            this.menu_ = new ContextMenu();
            this.undoItem = new MenuItem();
            this.redoItem = new MenuItem();
            this.sep1 = new MenuItem();
            this.cutItem = new MenuItem();
            this.copyItem = new MenuItem();
            this.pasteItem = new MenuItem();
            this.deleteItem = new MenuItem();
            this.sep2 = new MenuItem();
            this.insertItem = new MenuItem();
            this.fractionItem = new MenuItem();
            this.squareRootItem = new MenuItem();
            this.rootItem = new MenuItem();
            this.sep3 = new MenuItem();
            this.matrixItem = new MenuItem();
            this.sep4 = new MenuItem();
            this.superscriptItem = new MenuItem();
            this.subscriptItem = new MenuItem();
            this.fencedItem = new MenuItem();
            this.sep5 = new MenuItem();
            this.moItem = new MenuItem();
            this.miItem = new MenuItem();
            this.sep6 = new MenuItem();
            this.mtextItem = new MenuItem();
            this.sep7 = new MenuItem();
            this.mphantomItem = new MenuItem();
            this.sep8 = new MenuItem();
            this.mactionItem = new MenuItem();
            this.sep9 = new MenuItem();
            this.propFractionItem = new MenuItem();
            this.propFencedItem = new MenuItem();
            this.propTableItem = new MenuItem();
            this.propActionItem = new MenuItem();
            this.propItem = new MenuItem();
            this.styleItem = new MenuItem();
            this.menu_.MenuItems.AddRange(new MenuItem[] { this.undoItem, this.redoItem, this.sep1, this.cutItem, this.copyItem, this.pasteItem, this.deleteItem, this.sep2, this.insertItem, this.sep9, this.propFractionItem, this.propFencedItem, this.propTableItem, this.propActionItem, this.propItem, this.styleItem });
                        
            this.undoItem.Index = 0;
            this.undoItem.Text = "Undo";
            this.undoItem.Click += new EventHandler(this.OnUndo);

            
            this.redoItem.Index = 1;
            this.redoItem.Text = "Redo";
            this.redoItem.Click += new EventHandler(this.OnRedo);
                        
            this.sep1.Index = 2;
            this.sep1.Text = "-";
            
            this.cutItem.Index = 3;
            this.cutItem.Text = "Cut";
            this.cutItem.Click += new EventHandler(this.OnCut);

            this.copyItem.Index = 4;
            this.copyItem.Text = "Copy";
            this.copyItem.Click += new EventHandler(this.OnCopy);

            this.pasteItem.Index = 5;
            this.pasteItem.Text = "Paste";
            this.pasteItem.Click += new EventHandler(this.OnPaste);
                        
            this.deleteItem.Index = 6;
            this.deleteItem.Text = "Delete";
            this.deleteItem.Click += new EventHandler(this.OnDelete);
                        
            this.sep2.Index = 7;
            this.sep2.Text = "-";
                        
            this.insertItem.Index = 8;
            this.insertItem.MenuItems.AddRange(new MenuItem[] { 
                this.fractionItem, this.squareRootItem, this.rootItem, this.sep3, this.matrixItem, this.sep4, this.superscriptItem, this.subscriptItem, this.fencedItem, this.sep5, this.moItem, this.miItem, this.sep6, this.mtextItem, this.sep7, this.mphantomItem, 
                this.sep8, this.mactionItem
             });
            this.insertItem.Text = "Insert";
                        
            this.fractionItem.Index = 0;
            this.fractionItem.Text = "Fraction";
            this.fractionItem.Click += new EventHandler(this.OnFraction);
                        
            this.squareRootItem.Index = 1;
            this.squareRootItem.Text = "Square Root";
            this.squareRootItem.Click += new EventHandler(this.OnSquareRoot);
                        
            this.rootItem.Index = 2;
            this.rootItem.Text = "Root";
            this.rootItem.Click += new EventHandler(this.OnRoot);
                        
            this.sep3.Index = 3;
            this.sep3.Text = "-";
                        
            this.matrixItem.Index = 4;
            this.matrixItem.Text = "Matrix";
            this.matrixItem.Click += new EventHandler(this.OnMatrix);
                        
            this.sep4.Index = 5;
            this.sep4.Text = "-";
                        
            this.superscriptItem.Index = 6;
            this.superscriptItem.Text = "Superscript";
            this.superscriptItem.Click += new EventHandler(this.SuperscriptHandler);
            
            this.subscriptItem.Index = 7;
            this.subscriptItem.Text = "Subscript";
            this.subscriptItem.Click += new EventHandler(this.SubscriptHandler);
                        
            this.fencedItem.Index = 8;
            this.fencedItem.Text = "Fenced";
            this.fencedItem.Click += new EventHandler(this.FencedHandler);
                        
            this.sep5.Index = 9;
            this.sep5.Text = "-";
                        
            this.moItem.Index = 10;
            this.moItem.Text = "Mathematical Operator";
            this.moItem.Click += new EventHandler(this.MoHandler);
                        
            this.miItem.Index = 11;
            this.miItem.Text = "Mathematical Identifier";
            this.miItem.Click += new EventHandler(this.MiHandler);
                        
            this.sep6.Index = 12;
            this.sep6.Text = "-";
                        
            this.mtextItem.Index = 13;
            this.mtextItem.Text = "Text";
            this.mtextItem.Click += new EventHandler(this.MtextHandler);
                        
            this.sep7.Index = 14;
            this.sep7.Text = "-";
                        
            this.mphantomItem.Index = 15;
            this.mphantomItem.Text = "Phantom";
            this.mphantomItem.Click += new EventHandler(this.OnPhantom);
                        
            this.sep8.Index = 0x10;
            this.sep8.Text = "-";
                        
            this.mactionItem.Index = 0x11;
            this.mactionItem.Text = "Action";
            this.mactionItem.Click += new EventHandler(this.OnAction);
                        
            this.sep9.Index = 9;
            this.sep9.Text = "-";
                        
            this.propFractionItem.Index = 10;
            this.propFractionItem.Text = "Fraction Properties";
            this.propFractionItem.Click += new EventHandler(this.OnFractionProperties);

            this.propFencedItem.Index = 11;
            this.propFencedItem.Text = "Fenced Properties";
            this.propFencedItem.Click += new EventHandler(this.OnFencedProperties);

            this.propTableItem.Index = 12;
            this.propTableItem.Text = "Matrix Properties";
            this.propTableItem.Click += new EventHandler(this.OnMatrixProperties);

            this.propActionItem.Index = 13;
            this.propActionItem.Text = "Action Properties";
            this.propActionItem.Click += new EventHandler(this.OnActionProperties);
            
            this.propItem.Index = 14;
            this.propItem.Text = "-";

            
            this.styleItem.Index = 15;
            this.styleItem.Text = "Style Properties";
            this.styleItem.Click += new EventHandler(this.OnStyleProperties);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.Name = "ControlWithMenu";
        }

        private void PopupContextMenu(int x, int y)
        {
            bool copy_Active = false;
            bool bCut_Active = false;
            bool bUndo_Active = false;
            bool bRedo_Active = false;
            bool bProp_Fraction_Active = false;
            bool bProp_Table_Active = false;
            bool bProp_Fenced_Active = false;
            bool bProp_Action_Active = false;
            bool style_Active = false;

            base.SelectActiveMenuItems(ref copy_Active, ref bCut_Active, ref bUndo_Active, ref bRedo_Active, ref bProp_Fraction_Active, ref bProp_Table_Active, ref bProp_Fenced_Active, ref bProp_Action_Active, ref style_Active);
            
            this.undoItem.Enabled = false;
            this.redoItem.Enabled = false;
            this.copyItem.Enabled = false;
            this.cutItem.Enabled = false;
            this.propFractionItem.Visible = false;
            this.propFencedItem.Visible = false;
            this.propActionItem.Visible = false;
            this.propTableItem.Visible = false;
            this.propItem.Visible = false;
            this.styleItem.Enabled = false;
            if (bUndo_Active)
            {
                this.undoItem.Enabled = true;
            }
            if (bRedo_Active)
            {
                this.redoItem.Enabled = true;
            }
            if (copy_Active)
            {
                this.copyItem.Enabled = true;
            }
            if (bCut_Active)
            {
                this.cutItem.Enabled = true;
            }
            if (bProp_Fraction_Active)
            {
                this.propFractionItem.Visible = true;
                this.propItem.Visible = true;
            }
            else if (bProp_Table_Active)
            {
                this.propTableItem.Visible = true;
                this.propItem.Visible = true;
            }
            else if (bProp_Fenced_Active)
            {
                this.propFencedItem.Visible = true;
                this.propItem.Visible = true;
            }
            else if (bProp_Action_Active)
            {
                this.propActionItem.Visible = true;
                this.propItem.Visible = true;
            }
            else
            {
                this.propItem.Visible = false;
            }
            if (style_Active)
            {
                this.styleItem.Enabled = true;
            }
            this.menu_.Show(this, new Point(x, y));
        }

        private void OnUndo(object sender, EventArgs e)
        {
            base.Undo();
        }


        public bool ParentControl_DesignMode
        {
            get
            {
                return this.designMode;
            }
            set
            {
                this.designMode = value;
            }
        }

        public bool UseDefaultContextMenu
        {
            get
            {
                return this.useDefaultContextMenu;
            }
            set
            {
                this.useDefaultContextMenu = value;
            }
        }

        private IContainer components;
        private MenuItem insertItem;
        private MenuItem fractionItem;
        private MenuItem squareRootItem;
        private MenuItem rootItem;
        private MenuItem matrixItem;
        private MenuItem superscriptItem;
        private MenuItem subscriptItem;
        private MenuItem fencedItem;
        private MenuItem moItem;
        private MenuItem miItem;
        
        private MenuItem mtextItem;
        private MenuItem mphantomItem;
        private MenuItem mactionItem;
        private MenuItem propFractionItem;
        private MenuItem propFencedItem;
        private MenuItem propTableItem;
        private MenuItem propActionItem;
        
        public ContextMenu menu_;
        private bool designMode;
        private bool useDefaultContextMenu;

        private MenuItem sep1;
        private MenuItem sep2;
        private MenuItem sep3;
        private MenuItem sep4;
        private MenuItem sep5;
        private MenuItem sep6;
        private MenuItem sep7;
        private MenuItem sep8;
        private MenuItem sep9;

        private MenuItem propItem;
        private MenuItem styleItem;
        
        private MenuItem undoItem;
        private MenuItem redoItem;
        private MenuItem cutItem;
        private MenuItem copyItem;
        private MenuItem pasteItem;
        private MenuItem deleteItem;
    }
}


namespace Genetibase.MathX
{
	using Genetibase.ApplicationBlocks;
    using Nodes;
    using Fonts;
    using UI;
    using Facade;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;

    public class NuGenEQML : UserControl
    {
        public event MouseEventHandler Event_MouseDown;
        public event EventHandler Event_OnGotFocus;
        public event EventHandler Event_OnLostFocus;
        public event EventHandler Event_OnSelectionChanged;
        public event EventHandler Event_OnUndoRedoStackChanged;
        public event EventHandler Event_OnValidationError;

        static NuGenEQML()
        {
        }

        public NuGenEQML()
        {
            this.container_ = null;
            this.schemaName_ = "MathML_XMLSchema.xsd";
            this.xmlSource_ = "<?xml version='1.0' ?><math xmlns='http://www.w3.org/1998/Math/MathML' display='block'><mrow/></math>";
            this.backgroundColor_ = Color.White;
            this.fontCollection = null;
            this.entityManager = null;
            
            
            this.InitializeComponent();
            
            FontsProvider fontsProvider = new FontsProvider();
            this.fontCollection = fontsProvider.LoadAll();
            this.entityManager = new EntityManager(this.fontCollection);
            
            
            this.uiMenu = new ControlWithMenu(this.entityManager, this.fontCollection);
            this.uiMenu.AllowDrop = true;
            
            this.uiMenu.BackColor = Color.White;
            this.uiMenu.Dock = DockStyle.Fill;

            this.uiMenu.Location = new Point(0, 0);
            this.uiMenu.Name = "m_EditControl";
            this.uiMenu.OffsetX = 0;
            this.uiMenu.OffsetY = 0;
            this.uiMenu.Size = new Size(0x256, 0xe2);
            this.uiMenu.TabIndex = 0;
           
            base.Controls.Add(this.uiMenu);
            this.uiMenu.SetBounds(0, 0, base.Size.Width, base.Size.Height);
            this.uiMenu.DoResize(base.Size.Width, base.Size.Height, true, false);
            this.ResetWidth();
            
            this.uiMenu.Event_OnUndoRedoStackChanged += new EventHandler(this.OnUndoRedo);
            this.uiMenu.Event_OnValidationError += new ValidationHandler(this.OnValidationErrorHandler);
            
            this.uiMenu.Event_MouseDown += new MouseEventHandler(this.OnMouseDownHandler);
            this.uiMenu.Event_OnGotFocus += new EventHandler(this.OnGotFocusHandler);
            this.uiMenu.Event_OnLostFocus += new EventHandler(this.OnLostFocusHandler);
            this.uiMenu.KeyPress += new KeyPressEventHandler(this.KeyPressHandler);
            this.uiMenu.Event_OnSelectionChanged += new OnChangeSelection(this.SelectionChangedHandler);
            this.uiMenu.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
            }
            catch
            {
            }
            if (disposing && (this.container_ != null))
            {
                this.container_.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.ResetWidth();
        }

        public void pub_Command_SubScript()
        {
            this.uiMenu.Command_SubScript();
        }

        public void pub_Command_SubSupScript()
        {
            this.uiMenu.Command_SubSupScript();
        }

        public void pub_Command_SupScript()
        {
            this.uiMenu.Command_SupScript();
        }

        public void pub_Copy()
        {
            this.uiMenu.Copy();
        }

        public bool pub_CopyActive()
        {
            bool flag1 = false;
            try
            {
                return this.uiMenu.CopyActive();
            }
            catch
            {
				return flag1;
            }
        }

        public void pub_Cut()
        {
            this.uiMenu.Cut();
        }

        public bool pub_CutActive()
        {
            try
            {
                return this.uiMenu.CutActive();
            }
            catch
            {
				return false;
            }
        }

        public void pub_Delete()
        {
            this.uiMenu.Delete();
        }

        public Bitmap pub_Export2Image(float fontSize, int nResolution, ref int ImgBaseline)
        {
            try
            {
                return this.uiMenu.Export2Image(PixelFormat.Format24bppRgb, fontSize, nResolution, ref ImgBaseline);
            }
            catch
            {
				return null;
            }
        }

        public string pub_GetXML(bool bStrip_Namespace)
        {
            try
            {
                this.xmlSource_ = this.uiMenu.GetXML(bStrip_Namespace);
                return this.xmlSource_;
            }
            catch
            {
                return "";
            }
        }

        public void pub_Insert_MathML(string sXML, bool bValidate)
        {
            try
            {
                this.uiMenu.Insert_MathML(sXML, bValidate);
            }
            catch
            {
            }
        }

        public void pub_InsertAction()
        {
            this.uiMenu.InsertAction();
        }

        public void pub_InsertBrackets(string sEntityName1, string sEntityName2, bool bStretchy)
        {
            this.uiMenu.InsertBrackets(sEntityName1, sEntityName2, bStretchy);
        }

        public void pub_InsertEntity_Identifier(string entityName, bool bItalic, bool bBold)
        {
            this.uiMenu.InsertEntity_Identifier(entityName, bItalic, bBold);
        }

        public void pub_InsertEntity_Open_IdentifierDictionary_Dialog()
        {
            this.uiMenu.InsertEntity_Open_IdentifierDictionary_Dialog(true);
        }

        public void pub_InsertEntity_Open_OperatorDictionary_Dialog()
        {
            this.uiMenu.InsertEntity_Open_IdentifierDictionary_Dialog(false);
        }

        public void pub_InsertEntity_Operator(string entityName)
        {
            this.uiMenu.InsertEntity_Operator(entityName);
        }

        public void pub_InsertFenced()
        {
            this.uiMenu.InsertFenced();
        }

        public void pub_InsertFraction()
        {
            this.uiMenu.InsertFraction();
        }

        public void pub_InsertFraction_Bevelled()
        {
            this.uiMenu.InsertFraction_Bevelled();
        }

        public void pub_InsertMatrix(int rows, int cols)
        {
            try
            {
                this.uiMenu.InsertMatrix(rows, cols);
            }
            catch
            {
            }
        }

        public void pub_InsertMatrixDialog()
        {
            this.uiMenu.InsertMatrixDialog();
        }

        public void pub_InsertOver()
        {
            this.uiMenu.InsertOver();
        }

        public void pub_InsertOverAccent(string entityName)
        {
            this.uiMenu.InsertOverAccent(entityName);
        }

        public void pub_InsertPhantom()
        {
            this.uiMenu.InsertPhantom();
        }

        public void pub_InsertPrime(string entityName)
        {
            this.uiMenu.InsertPrime(entityName);
        }

        public void pub_InsertRoot()
        {
            this.uiMenu.InsertRoot();
        }

        public void pub_InsertSqrt()
        {
            this.uiMenu.InsertSqrt();
        }

        public void pub_InsertStretchyArrow_Over(string entityName)
        {
            this.uiMenu.InsertStretchyArrow_Over(entityName);
        }

        public void pub_InsertStretchyArrow_Under(string entityName)
        {
            this.uiMenu.InsertStretchyArrow_Under(entityName);
        }

        public void pub_InsertStretchyArrow_UnderOver(string entityName)
        {
            this.uiMenu.InsertStretchyArrow_UnderOver(entityName);
        }

        public void pub_InsertSubScript()
        {
            this.uiMenu.InsertSubScript();
        }

        public void pub_InsertSubSupScript()
        {
            this.uiMenu.InsertSubSupScript();
        }

        public void pub_InsertSuperScript()
        {
            this.uiMenu.InsertSuperScript();
        }

        public void pub_InsertText()
        {
            this.uiMenu.InsertText();
        }

        public void pub_InsertUnder()
        {
            this.uiMenu.InsertUnder();
        }

        public void pub_InsertUnderAccent(string entityName)
        {
            this.uiMenu.InsertUnderAccent(entityName);
        }

        public void pub_InsertUnderOver()
        {
            this.uiMenu.InsertUnderOver();
        }

        public bool LoadFromFile(string filename)
        {
            try
            {
            StreamReader reader = new StreamReader(filename);
            string xml = reader.ReadToEnd();
            reader.Close();

            return pub_LoadXML(xml);
            }
            catch
            {
                return false;
            }
        }

        public bool pub_LoadXML(string sXML)
        {
            try
            {
                this.xmlSource_ = sXML;
                this.uiMenu.LoadXML(sXML);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void pub_Paste()
        {
            this.uiMenu.Paste();
        }

        public void pub_Redo()
        {
            this.uiMenu.Redo();
        }

        public bool pub_RedoActive()
        {
            try
            {
                return this.uiMenu.RedoActive();
            }
            catch
            {
				return false;
            }
        }

        public bool pub_Save(string fileName)
        {
            try
            {
                this.uiMenu.Save(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool pub_SavePure(string fileName)
        {
            try
            {
                this.uiMenu.SavePure(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

		public void pub_Export()
		{
			using (NuGenImageExportForm imageExportForm = new NuGenImageExportForm())
			{
				int i = 0;
				imageExportForm.ShowDialog(this.uiMenu.Export2Image(PixelFormat.Format24bppRgb, 12, 300, ref i));
			}
		}

        public bool pub_SaveAsJPEG(string fileName, float fontSize, int ImgResolution, ref int ImgBaseline)
        {
            try
            {
                this.uiMenu.SaveAsJPEG(fileName, fontSize, ImgResolution, ref ImgBaseline);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void pub_SelectAll()
        {
            try
            {
                this.uiMenu.SelectAll();
            }
            catch
            {
            }
        }

        public void pub_SetDisplayStyle(bool bDisplayStyle)
        {
            try
            {
                Nodes.Attribute attribute;
                if (this.uiMenu.builder_ == null)
                {
                    return;
                }
                Node node = null;
                node = this.uiMenu.builder_.getRoot();
                if (node == null)
                {
                    return;
                }
                if (bDisplayStyle)
                {
                    if (node.attrs != null)
                    {
                        attribute = node.attrs.Get("display");
                        if (attribute != null)
                        {
                            attribute.val = "block";
                        }
                    }
                    else
                    {
                        node.attrs = new AttributeList();
                        node.attrs.Add(new Nodes.Attribute("display", "block", ""));
                    }
                }
                else if (node.attrs != null)
                {
                    attribute = node.attrs.Get("display");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                node.displayStyle = bDisplayStyle;
                this.uiMenu.ReRender();
            }
            catch
            {
            }
        }

        public void pub_SetSize(int x, int y, int w, int h)
        {
            try
            {
                base.SetBounds(x, y, w, h);
            }
            catch
            {
            }
         }

        public void pub_ShowPropertiesDialog()
        {
            this.uiMenu.ShowPropertiesDialog();
        }

        public void pub_ShowStyleDialog()
        {
            this.uiMenu.StyleProperties();
        }

        public void pub_Undo()
        {
            this.uiMenu.Undo();
        }

        public bool pub_UndoActive()
        {
            bool flag1 = false;
            try
            {
                return this.uiMenu.UndoActive();
            }
            catch
            {
				return flag1;
            }
        }

        private void ResetWidth()
        {
            if (this.uiMenu != null)
            {
                this.uiMenu.SetWidth(base.ClientRectangle.Width);
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.uiMenu.SetSchemas(this.schemaName_);

            this.uiMenu.SetFonts(this.fontCollection);
            try
            {
                this.uiMenu.LoadXML(this.xmlSource_, "", true);
            }
            catch
            {
            }
            this.uiMenu.Invalidate();
            this.uiMenu.Focus();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenEQML));
            this.SuspendLayout();
            // 
            // MathMLControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.DoubleBuffered = true;
            this.Name = "MathMLControl";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        private void OnUndoRedo(object sender, EventArgs e)
        {
            try
            {
                this.Event_OnUndoRedoStackChanged(this, new EventArgs());
            }
            catch
            {
            }
        }

        private void OnMouseDownHandler(object sender, MouseEventArgs e)
        {
            try
            {
                this.Event_MouseDown(this, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
            }
            catch
            {
            }
        }

        private void KeyPressHandler(object sender, KeyPressEventArgs e)
        {
            try
            {
                this.OnKeyPress(e);
            }
            catch
            {
            }
        }

        private void OnValidationErrorHandler(object sender, ValidationErrorArgs e)
        {
            try
            {
                this.Event_OnValidationError(this, new ValidationErrorEventArgs(e.Message, e.Line, e.Pos));
            }
            catch
            {
            }
        }

        private void OnGotFocusHandler(object sender, EventArgs e)
        {
            try
            {
                this.Event_OnGotFocus(this, new EventArgs());
            }
            catch
            {
            }
        }

        private void OnLostFocusHandler(object sender, EventArgs e)
        {
            try
            {
                this.Event_OnLostFocus(this, new EventArgs());
            }
            catch
            {
            }
        }

        private void SelectionChangedHandler(object sender, SelectionArgs e)
        {
            try
            {
                this.Event_OnSelectionChanged(this, new OnMathMLSelectionChanged(e.HasSelection));
            }
            catch
            {
            }
        }


        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        public Color MC_BackgroundColor
        {
            get
            {
                return this.backgroundColor_;
            }
            set
            {
                this.backgroundColor_ = value;
                this.BackColor = this.backgroundColor_;
            }
        }

        public bool MC_DisplayStyle
        {
            get
            {
                bool flag1 = false;
                try
                {
                    if (this.uiMenu.builder_ != null)
                    {
                        Node z_1 = null;
                        z_1 = this.uiMenu.builder_.getRoot();
                        if (z_1 == null)
                        {
                            return flag1;
                        }
                        flag1 = z_1.displayStyle;
                    }
                }
                catch
                {
                }
                return flag1;
            }
        }

        public bool AutoCloseBrackets
        {
            get
            {
                bool r = true;
                if (this.uiMenu != null)
                {
                    r = this.uiMenu.AutoCloseBrackets;
                }
                return r;
            }
            set
            {
                if (this.uiMenu != null)
                {
                    this.uiMenu.AutoCloseBrackets = value;
                }
            }
        }

        public bool MC_EnableStretchyBrackets
        {
            get
            {
                bool r = true;
                if ((this.uiMenu != null) && this.uiMenu.NonStretchyBrackets)
                {
                    r = false;
                }
                return r;
            }
            set
            {
                if (this.uiMenu != null)
                {
                    if (value)
                    {
                        this.uiMenu.NonStretchyBrackets = false;
                    }
                    else
                    {
                        this.uiMenu.NonStretchyBrackets = true;
                    }
                }
            }
        }

        public float MC_FontSize
        {
            get
            {
                if (this.uiMenu == null)
                {
                    return 12f;
                }
                try
                {
                    return this.uiMenu.FontSize;
                }
                catch
                {
					return 12f;
                }
            }
            set
            {
                if (this.uiMenu != null)
                {
                    try
                    {
                        this.uiMenu.FontSize = value;
                    }
                    catch
                    {
                    }
                }
            }
        }

        public bool MC_UseDefaultContextMenu
        {
            get
            {
                bool r = true;
                if (this.uiMenu != null)
                {
                    r = this.uiMenu.UseDefaultContextMenu;
                }
                return r;
            }
            set
            {
                if (this.uiMenu != null)
                {
                    this.uiMenu.UseDefaultContextMenu = value;
                }
            }
        }

        public bool ParentControl_DesignMode
        {
            get
            {
                try
                {
                    return this.uiMenu.ParentControl_DesignMode;
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    this.uiMenu.ParentControl_DesignMode = value;
                }
                catch
                {
                }
            }
        }

        [Browsable(false)]
        public override System.Windows.Forms.RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
            }
        }

        private string schemaName_;
        private string xmlSource_;
        private Color backgroundColor_;
        private FontCollection fontCollection;
        private EntityManager entityManager;

        private ControlWithMenu uiMenu;
        private Container container_;
    }
}


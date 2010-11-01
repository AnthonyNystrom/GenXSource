namespace UI
{
    using Microsoft.Win32;
    using Attrs;
    using Facade;
    using JpegComment;
    using Boxes;
    using Nodes;
    using Operators;
    using Fonts;
    using UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    internal class BBox
    {
        public BBox()
        {
            this.left = 0;
            this.top = 0;
            this.right = 0;
            this.bottom = 0;
        }


        public int Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }

        public int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }

        public int Right
        {
            get
            {
                return this.right;
            }
            set
            {
                this.right = value;
            }
        }

        public int Bottom
        {
            get
            {
                return this.bottom;
            }
            set
            {
                this.bottom = value;
            }
        }

        private int left;
        private int top;
        private int right;
        private int bottom;
    }

    internal partial class CoreControl : UserControl
    {
        public event EventHandler Event_OnGotFocus;
        public event EventHandler Event_OnLostFocus;
        public event OnChangeSelection Event_OnSelectionChanged;
        public event EventHandler Event_OnUndoRedoStackChanged;
        public event ValidationHandler Event_OnValidationError;

        static CoreControl ()
        {
            CoreControl.BITSPIXEL = 12;
            CoreControl.PLANES = 14;
        }

        public CoreControl (EntityManager MathMLEntityManager, FontCollection FontCollection)
        {
            needUpdate = false;
            needCheckIsPaletted = true;
            isPaletted = false;
            selectionInfo = null;
            fonts_ = null;
            panel_ = null;
            operators_ = null;
            autoCloseBrackets_ = true;
            isInitialized_ = false;
            types = null;
            canvasWidth = 0;
            canvasHeight = 0;
            lMargin = 0;
            rMargin = 0;
            tMargin = 0;
            bMargin = 0;
            bbox = new BBox ();
            markX = 0;
            markY = 0;
            caretHeight = 0;
            showCaret = false;
            offsetY = 0;
            offsetX = 0;
            isAntiAlias = true;
            haveScrollbars_ = false;
            try
            {
                InitializeComponent ();
                Init (MathMLEntityManager, FontCollection);
                isInitialized_ = true;
            }
            catch
            {
            }
        }

        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                try
                {
                    operators_ = null;
                    types = null;
                    fonts_ = null;
                    entityManager = null;
                }
                catch
                {
                }
                
                try
                {
                    caretThread.Abort ();
                    caretThread = null;
                }
                catch
                {
                }
            }
            base.Dispose (disposing);
        }

        [DllImport ("gdi32.dll")]
        public static extern short GetDeviceCaps ([In, MarshalAs (UnmanagedType.U4)] int hDc,
                                                  [In, MarshalAs (UnmanagedType.U2)] short funct);

        protected override void OnDoubleClick (EventArgs e)
        {
            base.OnDoubleClick (e);
        }

        protected override void OnGotFocus (EventArgs e)
        {
            ReRender ();
            base.OnGotFocus (e);
            try
            {
                Event_OnGotFocus (this, new EventArgs ());
            }
            catch
            {
            }
        }

        protected override void OnInvalidated (InvalidateEventArgs e)
        {
            rect_ = e.InvalidRect;
            base.OnInvalidated (e);
        }

        protected override void OnKeyPress (KeyPressEventArgs e)
        {
            base.OnKeyPress (e);
            if (!e.Handled)
            {
                if ((char.IsPunctuation (e.KeyChar) || char.IsSeparator (e.KeyChar)) ||
                         ((char.IsSurrogate (e.KeyChar) || char.IsSymbol (e.KeyChar)) ||
                          char.IsLetterOrDigit (e.KeyChar)))
                {
                    if (builder_.InsertChar (e.KeyChar, !NonStretchyBrackets, AutoCloseBrackets))
                    {
                        ReRender ();
                        base.Update ();
                    }
                }
            }
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
        }

        protected override void OnLostFocus (EventArgs e)
        {
            try
            {
                if ((builder_ != null) && builder_.HasSelection)
                {
                    ReRender ();
                }
                else
                {
                    InvalidateBbox ();
                    InvalidateMark ();
                }
            }
            catch
            {
            }
            base.OnLostFocus (e);
            try
            {
                Event_OnLostFocus (this, new EventArgs ());
            }
            catch
            {
            }
            try
            {
                bbox.Left = 0;
                bbox.Top = 0;
                bbox.Right = 0;
                bbox.Bottom = 0;
            }
            catch
            {
                return;
            }
        }

        protected override void OnMouseDown (MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    if (builder_ != null)
                    {
                        if (Control.ModifierKeys == Keys.Shift)
                        {
                            if (!builder_.HasSelection)
                            {
                                builder_.HasSelection = true;
                            }
                            if (
                                builder_.SelectionTo ((e.X + OffsetX) - lMargin,
                                              (e.Y + OffsetY) - tMargin))
                            {
                                ReRender ();
                            }
                        }
                        else
                        {
                            try
                            {
                                SelectAt (e.X, e.Y);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            base.OnMouseDown (e);
        }

        protected override void OnMouseMove (MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (!builder_.HasSelection)
                    {
                        builder_.HasSelection = true;
                    }
                    else if (
                        builder_.SelectionTo ((e.X + OffsetX) - lMargin,
                                      (e.Y + OffsetY) - tMargin))
                    {
                        needUpdate = true;
                        ReRender ();
                    }
                }
            }
            catch
            {
            }
            base.OnMouseMove (e);
        }

        protected override void OnMouseUp (MouseEventArgs e)
        {
            if (builder_.HasSelection)
            {
                builder_.RemoveSelection ();
            }
            base.OnMouseUp (e);
        }

        protected override void OnMouseWheel (MouseEventArgs e)
        {
            try
            {
                VertScroll (-e.Delta);
            }
            catch
            {
            }
            base.OnMouseWheel (e);
        }

        protected override void OnPaint (PaintEventArgs e)
        {
            Node selectedNode = null;
            Node multiSelectNode = null;
            int selectedNodeMark = 0;
            int multiSelectMark = 0;
            bool multiSelect = false;
            
            try
            {
                if (isInitialized_)
                {
                    try
                    {
                        if ((builder_ != null) && (builder_.GetCurrentlySelectedNode () != null))
                        {
                            DetermineIsPaletted(e.Graphics);
                            builder_.SetupPainting(e.Graphics, isAntiAlias, isPaletted);
                            builder_.getRoot();
                            
                            builder_.MeasureAll();
                            selectedNode = builder_.GetCurrentlySelectedNode();
                            try
                            {
                                multiSelectNode = builder_.MultiSelectNode();
                                selectedNodeMark = selectedNode.InternalMark;
                                multiSelectMark = builder_.CurrentCaret();
                                multiSelect = builder_.HasSelection;
                                
                            }
                            catch
                            {
                            }
                            builder_.SetOrigin(lMargin - OffsetX, tMargin - OffsetY);
                            Rectangle updateRect =
                                new Rectangle((e.ClipRectangle.X - lMargin) + OffsetX,
                                              (e.ClipRectangle.Y - tMargin) + OffsetY, e.ClipRectangle.Width,
                                              e.ClipRectangle.Height);
                            if ((((builder_.RootWidth + lMargin) + rMargin) != canvasWidth) ||
                                     (((builder_.RootHeight + tMargin) + bMargin) != canvasHeight)
                                )
                            {
                                if (((builder_.RootWidth + lMargin) + rMargin) !=
                                    canvasWidth)
                                {
                                    canvasWidth = (builder_.RootWidth + lMargin) + rMargin;
                                }
                                if (((builder_.RootHeight + tMargin) + bMargin) !=
                                    canvasHeight)
                                {
                                    canvasHeight = (builder_.RootHeight + tMargin) + bMargin;
                                }
                                ResizeScrollbars();
                            }
                            builder_.FillBackground(updateRect);
                            
                            if (builder_.HasSelection && Focused)
                            {
                                try
                                {
                                    DrawHighlightSelection(builder_.CaptureSelection(), e);
                                }
                                catch
                                {
                                }
                            }
                            builder_.FillForeground(updateRect);

                            if (selectedNode.IsAppend)
                            {
                                markX = (selectedNode.box.X + selectedNode.box.Width) + lMargin;
                                try
                                {
                                    if (selectedNode.type_.type == ElementType.Ms)
                                    {
                                        markX = (selectedNode.box.X + selectedNode.LiteralStart) + lMargin;
                                    }
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                markX = (selectedNode.box.X + selectedNode.LiteralStart) + lMargin;
                            }

                            markY = selectedNode.box.Y + tMargin;
                            caretHeight = selectedNode.box.Height;
                            if (Focused)
                            {
                                if ((selectedNode.type_ != null))
                                {
                                    bbox.Left = (selectedNode.box.X + lMargin) - OffsetX;
                                    bbox.Top = ((selectedNode.box.Y + tMargin) - OffsetY) + selectedNode.box.Height;
                                    bbox.Right = selectedNode.box.Width;
                                    bbox.Bottom = 1;
                                    e.Graphics.DrawLine(caretBluePen, bbox.Left, bbox.Top,
                                                        bbox.Left + bbox.Right, bbox.Top);
                                }
                                else
                                {
                                    bbox.Left = 0;
                                    bbox.Top = 0;
                                    bbox.Right = 0;
                                    bbox.Bottom = 0;
                                }
                            }
                            if (Focused && showCaret)
                            {
                                if ((selectedNode.type_ != null))
                                {
                                    e.Graphics.DrawLine(caretBluePen, markX - OffsetX,
                                                        markY - OffsetY, markX - OffsetX,
                                                        (markY - OffsetY) + caretHeight);
                                }
                                else
                                {
                                    e.Graphics.DrawLine(caretBlackPen, markX - OffsetX,
                                                        markY - OffsetY, markX - OffsetX,
                                                        (markY - OffsetY) + caretHeight);
                                }
                            }
                        }
                        else
                        {
                            ResizeScrollbars ();
                        }
                    }
                    catch (Exception)
                    {
                        ReRender ();
                    }
                }
            }
            catch
            {
            }
            
            try
            {
                if (selectionInfo == null)
                {
                    selectionInfo = new SelectionInfo (selectedNode, selectedNodeMark, multiSelectNode, multiSelectMark, multiSelect);
                }
                else
                {
                    SelectionInfo info = new SelectionInfo (selectedNode, selectedNodeMark, multiSelectNode, multiSelectMark, multiSelect);
                    if (!selectionInfo.Equals (info))
                    {
                        try
                        {
                            Event_OnSelectionChanged (this, new SelectionArgs (builder_.HasSelection));
                        }
                        catch
                        {
                        }
                    }
                    selectionInfo = info;
                }
            }
            catch
            {
            }
            base.OnPaint (e);
        }

        protected override void OnResize (EventArgs e)
        {
            DoResize (base.Width, base.Height, true, true);
        }

        public void InsertFraction ()
        {
            builder_.InsertFraction ();
            ReRender ();
            base.Focus ();
        }

        public void InsertFraction_Bevelled ()
        {
            builder_.InsertFraction_Bevelled ();
            ReRender ();
            base.Focus ();
        }

        public void InsertText ()
        {
            builder_.insertText ();
            ReRender ();
            base.Focus ();
        }

        public void InsertAction ()
        {
            builder_.InsertAction ();
            ReRender ();
            base.Focus ();
        }

        public void InsertFenced ()
        {
            builder_.InsertFenced ();
            ReRender ();
            base.Focus ();
        }

        public void InsertPhantom ()
        {
            builder_.InsertPhantom ();
            ReRender ();
            base.Focus ();
        }

        public void InsertMatrix (int Rows, int Cols)
        {
            builder_.InsertMatrix (Rows, Cols);
            ReRender ();
        }

        public void Command_SupScript ()
        {
            if (builder_.InsertSupScript ())
            {
                ReRender ();
            }
            base.Focus ();
        }

        public void Command_SubSupScript ()
        {
            if (builder_.InsertSubSupScript ())
            {
                ReRender ();
            }
            base.Focus ();
        }

        public void Command_SubScript ()
        {
            if (builder_.InsertSubScript ())
            {
                ReRender ();
            }
            base.Focus ();
        }

        private void OnInsert ()
        {
           needUpdate = true;
        }
        
        public void InsertUnder ()
        {
            builder_.InsertUnder ();
            ReRender ();
            base.Focus ();
        }

        public void InsertOver ()
        {
            builder_.InsertOver ();
            ReRender ();
            base.Focus ();
        }

        public void InsertUnderOver ()
        {
            builder_.InsertUnderOver ();
            ReRender ();
            base.Focus ();
        }

        public void InsertOverAccent (string sEntity)
        {
            if (sEntity.Length > 0)
            {
                builder_.InsertOverAccent (sEntity);
                ReRender ();
            }
            base.Focus ();
        }

        public void InsertUnderAccent (string sEntity)
        {
            if (sEntity.Length > 0)
            {
                builder_.InsertUnderAccent (sEntity);
                ReRender ();
            }
            base.Focus ();
        }

        public void InsertPrime (string sEntity)
        {
            if (sEntity.Length > 0)
            {
                builder_.InsertPrime (sEntity);
                ReRender ();
            }
            base.Focus ();
        }

        public void InsertSqrt ()
        {
            builder_.InsertSqrt ();
            ReRender ();
            base.Focus ();
        }

        public void InsertRoot ()
        {
            builder_.InsertRoot ();
            ReRender ();
            base.Focus ();
        }

        public void InsertFenced (string sCharL, string sCharR)
        {
            builder_.InsertFenced (sCharL, sCharR);
            ReRender ();
            base.Focus ();
        }

        public void InsertBrackets (string sEntityName_Left, string sEntityName_Right, bool bStretchy)
        {
            builder_.InsertFenced (sEntityName_Left, sEntityName_Right, bStretchy);
            ReRender ();
            base.Focus ();
        }

        public void InsertSubScript ()
        {
            builder_.InsertSubscript ();
            ReRender ();
            base.Focus ();
        }

        public void InsertSuperScript ()
        {
            builder_.InsertSuperScript ();
            ReRender ();
            base.Focus ();
        }

        public void InsertSubSupScript ()
        {
            builder_.InsertSubSup ();
            ReRender ();
            base.Focus ();
        }

        //
        public void Delete ()
        {
            try
            {
                builder_.DoDelete ();
                needUpdate = true;
                ReRender ();
            }
            catch
            {
            }
        }
        //
        public void Cut ()
        {
            try
            {
                if (builder_.HasSelection)
                {
                    DoCut ();
                    ReRender ();
                    base.Update ();
                    Delete ();
                }
            }
            catch
            {
            }
        }
        //
        public bool CopyActive ()
        {
            bool r = false;
            if (builder_ != null)
            {
                r = builder_.HasSelection;
            }
            return r;
        }
        //
        public bool CutActive ()
        {
            bool r = false;
            if (builder_ != null)
            {
                r = builder_.HasSelection;
            }
            return r;
        }
        //
        public bool UndoActive ()
        {
            bool r = false;
            if (builder_ != null)
            {
                r = builder_.HasUndo ();
            }
            return r;
        }
        //
        public bool RedoActive ()
        {
            bool redo = false;
            if (builder_ != null)
            {
                redo = builder_.HasRedo ();
            }
            return redo;
        }
        //
        public void Undo ()
        {
            try
            {
                if (builder_.Undo ())
                {
                    needUpdate = true;
                    ReRender ();
                }
            }
            catch
            {
            }
        }
        //
        public void Redo ()
        {
            try
            {
                if (builder_.Redo ())
                {
                    needUpdate = true;
                    ReRender ();
                }
            }
            catch
            {
            }
        }
        //
        public void DoCut ()
        {
            string xml = "";
            DataObject dataObject = new DataObject ();
            try
            {
                xml = builder_.CaptureForClipboard ();
                xml = xml.Trim ();
                dataObject.SetData (DataFormats.UnicodeText, true, xml);
                Clipboard.SetDataObject (dataObject, true);
            }
            catch (Exception )
            {
                
            }
        }
        //
        public void Copy ()
        {
            try
            {
                if (!builder_.HasSelection)
                {
                    return;
                }
                string xml = "";
                DataObject dataObject = new DataObject ();
                try
                {
                    xml = builder_.CaptureForClipboard ();
                    
                    xml = xml.Trim ();
                    dataObject.SetData (DataFormats.UnicodeText, true, xml);
                    Clipboard.SetDataObject (dataObject, true);
                }
                catch (Exception )
                {
                }
            }
            catch
            {
            }
        }
        //
        public string GetXML (bool bStrip_Namespace)
        {
            return builder_.SaveToXML(bStrip_Namespace);
        }
        //
        public void Insert_MathML (string xml, bool doValidation)
        {
            try
            {
                bool isValid = true;
                xml = xml.Trim ();
                if (doValidation)
                {
                    isValid = builder_.Validate (xml);
                }
                if (isValid)
                {
                    xml = builder_.ProcessEntities (xml);
                    if (xml.Length <= 0)
                    {
                        return;
                    }
                    builder_.insertMathML (xml);
                    needUpdate = true;
                    ReRender ();
                    base.Focus ();
                }
            }
            catch
            {
            }
        }
        //
        public void Paste ()
        {
            IDataObject dataObject = Clipboard.GetDataObject ();
            if (dataObject.GetDataPresent (DataFormats.Text) || dataObject.GetDataPresent (DataFormats.UnicodeText))
            {
                string xml = dataObject.GetData (DataFormats.UnicodeText).ToString ();
                xml = xml.Trim ();
                if (xml.Length == 0)
                {
                    return;
                }
                if (xml[0] != '<')
                {
                    return;
                }
                else if ((xml.Length > 5) && (xml.Substring (0, 5) == "<math"))
                {
                    int index = 0;
                    index = xml.IndexOf (">");
                    if (index != -1)
                    {
                        xml = "<math xmlns=\"http://www.w3.org/1998/Math/MathML\">" +
                                xml.Substring (index + 1, (xml.Length - index) - 1);
                    }
                }
                else if ((xml.Length > 7) && (xml.Substring (0, 7) == "<m:math"))
                {
                    int index = 0;
                    index = xml.IndexOf (">");
                    if (index != -1)
                    {
                        xml = "<math xmlns=\"http://www.w3.org/1998/Math/MathML\">" +
                                xml.Substring (index + 1, (xml.Length - index) - 1);
                        xml = xml.Replace ("<m:", "<m");
                        xml = xml.Replace ("</m:", "<m");
                    }
                }
                if (builder_.Validate (xml))
                {
                    xml = builder_.ProcessEntities (xml);
                    if (xml.Length > 0)
                    {
                        builder_.insertMathML (xml, true);
                        needUpdate = true;
                        ReRender ();
                    }
                }
            }
            base.Focus ();
        }
        //
        public void DoResize (int w, int h, bool bSizeScrollbars, bool bSetVisibleRectangle)
        {
            int height = 0;
            int width = 0;
            base.Size = new Size (w, h);
            if (builder_ != null)
            {
                if ((horScroller_ != null) && horScroller_.Visible)
                {
                    height = h - horScroller_.Height;
                }
                else
                {
                    height = h;
                }
                if ((vertScroller_ != null) && vertScroller_.Visible)
                {
                    width = w - vertScroller_.Width;
                }
                else
                {
                    width = w;
                }
                builder_.SetClientHeight (height);
                builder_.SetClientWidth (width);
            }
            ReRender ();
            if (bSizeScrollbars)
            {
                ResizeScrollbars ();
            }
            if (bSetVisibleRectangle)
            {
                SetVisibleRectangle ();
            }
        }

        private void Init (EntityManager MathMLEntityManager, FontCollection FontCollection)
        {
            try
            {
                SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler (UserPrefChanged);
            }
            catch
            {
            }
            fonts_ = FontCollection;
            selectionBrush_ = new SolidBrush (Color.Gray);
                
            operators_ = new OperatorDictionary ();
            builder_ = new NodesBuilder (base.ClientRectangle.Width - 20, operators_);
           
            builder_.setFonts (fonts_);
            builder_.SetOrigin (lMargin - OffsetX, tMargin - OffsetY);
            if (MathMLEntityManager == null)
            {
                entityManager = new EntityManager (fonts_);
            }
            else
            {
                entityManager = MathMLEntityManager;
            }
            builder_.SetEntityManager (entityManager);
            builder_.UndoRedo += new EventHandler (UndoRedoHandler);
            
            builder_.InvalidXML += new InvalidXMLFile (OnInvalidXML);
            
            builder_.InsertHappened += new InsertionHappenned (OnInsert);
            base.SetStyle (ControlStyles.UserPaint, true);
            base.SetStyle (ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle (ControlStyles.DoubleBuffer, true);
            caretBlackPen = new Pen (Color.Black);
            caretBluePen = new Pen (Color.Blue);
            caretThread = new Thread (new ThreadStart (CaretThreadProc));
            caretThread.Start ();
            canvasWidth = 0;
            canvasHeight = 0;
            ResizeScrollbars ();
            horScroller_.Visible = false;
            vertScroller_.Visible = false;
            vertScroller_.TabStop = false;
            horScroller_.TabStop = false;
            vertScroller_.GotFocus += new EventHandler (VertGotFocusHandler);
            horScroller_.GotFocus += new EventHandler (HorzGotFocusHandler);
            builder_.PropogateEntityManager ();
            types = new Types ();
            builder_.SetTypes (types);
            
            SetAntialias (true);
            
            try
            {
                UpdateMargins ();
            }
            catch
            {
            }
            DoResize (base.ClientRectangle.Width, base.ClientRectangle.Height, true, false);
            if (base.ClientRectangle.Width > 50)
            {
                SetWidth ((base.ClientRectangle.Width - vertScroller_.Width) - 2);
            }
            else
            {
                SetWidth (50);
            }
        }
        //
        public void ScrollDown ()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                ReRender ();
                base.Update ();
                base.Focus ();
            }
            catch
            {
            }
            Cursor = Cursors.Default;
        }
        //
        public void ReRender ()
        {
            FireInvalidate ();
            if (needUpdate)
            {
                needUpdate = false;
                base.Update ();
                RefreshAll ();
            }
        }
        //
        public void FireInvalidate ()
        {
            if (isInitialized_)
            {
                if (builder_ != null)
                {
                    rect_ = builder_.rectangleToUpdate ();
                    base.Invalidate (rect_);
                }
                else
                {
                    base.Invalidate ();
                }
            }
        }
        //
        public void RenderWithNewFont ()
        {
            if (isInitialized_)
            {
                Cursor = Cursors.WaitCursor;
                builder_.UpdateVertical ();
                base.Update ();
                ScrollDown();
                Cursor = Cursors.Default;
            }
        }
        //
        private void DetermineIsPaletted (Graphics g)
        {
            if ((g != null) && needCheckIsPaletted)
            {
                try
                {
                    isPaletted = false;
                    int colors = 0x20;
                    IntPtr hdc = IntPtr.Zero;
                    try
                    {
                        hdc = g.GetHdc ();
                        colors = CoreControl.GetDeviceCaps ((int) hdc, CoreControl.BITSPIXEL) *
                               CoreControl.GetDeviceCaps ((int) hdc, CoreControl.PLANES);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        try
                        {
                            if (hdc != IntPtr.Zero)
                            {
                                g.ReleaseHdc (hdc);
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (colors <= 8)
                    {
                        isPaletted = true;
                    }
                }
                catch
                {
                    return;
                }
                finally
                {
                    needCheckIsPaletted = false;
                }
            }
        }
        //
        private void DrawHighlightSelection (Selection SelectionCollection, PaintEventArgs e)
        {
            try
            {
                Node multi = builder_.MultiSelectNode ();
                Node first = SelectionCollection.First;
                Node last = SelectionCollection.Last;
                SelectionCollection.nodesList.Reset ();
                if (multi != null)
                {
                    SelectionCollection.nodesList.Reset();
                    for (Node par = SelectionCollection.nodesList.Next();
                         par != null;
                         par = SelectionCollection.nodesList.Next())
                    {
                        if ((par.IsAtom() && (par == first)) && (par == last))
                        {
                            int markX = 0;
                            int markEnd = 0;
                            markX = builder_.WidthToMark(par, SelectionCollection.caret);
                            markEnd = builder_.WidthToMark(par, SelectionCollection.literalLength);

                            if (markX > markEnd)
                            {
                                int t = markX;
                                markX = markEnd;
                                markEnd = t;
                            }
                            e.Graphics.FillRectangle(selectionBrush_,
                                                     ((par.box.X + markX) + lMargin) - OffsetX,
                                                     (par.box.Y + tMargin) - OffsetY, markEnd - markX,
                                                     par.box.Height);
                        }
                        else if (par.IsAtom() && (par == first))
                        {
                            int markX = 0;
                            markX = builder_.WidthToMark(par, SelectionCollection.caret);
                            e.Graphics.FillRectangle(selectionBrush_,
                                                     ((par.box.X + markX) + lMargin) - OffsetX,
                                                     (par.box.Y + tMargin) - OffsetY,
                                                     par.box.Width - markX, par.box.Height);
                        }
                        else if (par.IsAtom() && (par == last))
                        {
                            int markEnd = 0;
                            markEnd = builder_.WidthToMark(par, SelectionCollection.literalLength);
                            if (par == multi)
                            {
                            }
                            e.Graphics.FillRectangle(selectionBrush_,
                                                     (par.box.X + lMargin) - OffsetX,
                                                     (par.box.Y + tMargin) - OffsetY, markEnd,
                                                     par.box.Height);
                        }
                        else
                        {
                            bool draw = false;
                            if (SelectionCollection.nodesList.Count == 1)
                            {
                                if ((SelectionCollection.caret == 0) &&
                                    (SelectionCollection.literalLength == par.LiteralLength))
                                {
                                    draw = true;
                                }
                            }
                            else if (par == first)
                            {
                                if (SelectionCollection.caret == 0)
                                {
                                    draw = true;
                                }
                            }
                            else if (par == last)
                            {
                                if (SelectionCollection.literalLength == par.LiteralLength)
                                {
                                    draw = true;
                                }
                            }
                            else
                            {
                                draw = true;
                            }
                            if (draw)
                            {
                                e.Graphics.FillRectangle(selectionBrush_,
                                                         (par.box.X + lMargin) - OffsetX,
                                                         (par.box.Y + tMargin) - OffsetY,
                                                         par.box.Width, par.box.Height);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        //
        public void ResizeScrollbars ()
        {
            try
            {
                panel_.Visible = false;
            }
            catch
            {
            }
            try
            {
                bool hVisible = false;
                bool vVisible = false;
                bool finalHVisible = false;
                bool finalVVisible = false;
                try
                {
                    hVisible = horScroller_.Visible;
                    vVisible = vertScroller_.Visible;
                }
                catch
                {
                }
                try
                {
                    if (haveScrollbars_)
                    {
                        bool hasH = false;
                        bool hasV = false;
                        if (canvasWidth > base.ClientRectangle.Width)
                        {
                            hasH = true;
                        }
                        if (canvasHeight > base.ClientRectangle.Height)
                        {
                            hasV = true;
                        }
                        if ((hasH && !hasV) && (canvasHeight > (base.ClientRectangle.Height - horScroller_.Height)))
                        {
                            hasV = true;
                        }
                        if ((hasV && !hasH) && (canvasWidth > (base.ClientRectangle.Width - vertScroller_.Width)))
                        {
                            hasH = true;
                        }
                        if (hasH && hasV)
                        {
                            panel_.Visible = true;
                            panel_.Location =
                                new Point (base.ClientRectangle.Width - vertScroller_.Width,
                                           base.ClientRectangle.Height - horScroller_.Height);
                            horScroller_.SetBounds (0, base.ClientRectangle.Height - horScroller_.Height,
                                                    base.ClientRectangle.Width - vertScroller_.Width,
                                                    horScroller_.Height);
                            vertScroller_.SetBounds (base.ClientRectangle.Right - vertScroller_.Width, 0,
                                                     vertScroller_.Width,
                                                     base.ClientRectangle.Height - horScroller_.Height);
                            horScroller_.Minimum = 0;
                            vertScroller_.Minimum = 0;
                            if (base.ClientRectangle.Width - vertScroller_.Width > 0)
                            {
                                horScroller_.LargeChange = base.ClientRectangle.Width - vertScroller_.Width;
                            }
                            horScroller_.Maximum = canvasWidth;
                            horScroller_.Minimum = 0;
                            horScroller_.Visible = true;
                            if (base.ClientRectangle.Height - horScroller_.Height > 0)
                            {
                                vertScroller_.LargeChange = base.ClientRectangle.Height - horScroller_.Height;
                            }
                            vertScroller_.Maximum = canvasHeight;
                            vertScroller_.Minimum = 0;
                            vertScroller_.Visible = true;
                        }
                        else if (hasV)
                        {
                            panel_.Visible = false;
                            horScroller_.SetBounds (0, base.ClientRectangle.Height - horScroller_.Height,
                                                    base.ClientRectangle.Width - vertScroller_.Width,
                                                    horScroller_.Height);
                            vertScroller_.SetBounds (base.ClientRectangle.Right - vertScroller_.Width, 0,
                                                     vertScroller_.Width, base.ClientRectangle.Height);
                            horScroller_.Minimum = 0;
                            vertScroller_.Minimum = 0;
                            if (base.ClientRectangle.Height > 0)
                            {
                                vertScroller_.LargeChange = base.ClientRectangle.Height;
                            }
                            vertScroller_.Maximum = canvasHeight;
                            vertScroller_.Minimum = 0;
                            vertScroller_.Visible = true;
                            horScroller_.Maximum = 0;
                            horScroller_.Visible = false;
                            OffsetX = 0;
                        }
                        else if (hasH)
                        {
                            panel_.Visible = false;
                            horScroller_.SetBounds (0, base.ClientRectangle.Height - horScroller_.Height,
                                                    base.ClientRectangle.Width, horScroller_.Height);
                            vertScroller_.SetBounds (base.ClientRectangle.Right - vertScroller_.Width, 0,
                                                     vertScroller_.Width,
                                                     base.ClientRectangle.Height - horScroller_.Height);
                            horScroller_.Minimum = 0;
                            vertScroller_.Minimum = 0;
                            if (base.ClientRectangle.Width > 0)
                            {
                                horScroller_.LargeChange = base.ClientRectangle.Width;
                            }
                            horScroller_.Maximum = canvasWidth;
                            horScroller_.Minimum = 0;
                            horScroller_.Visible = true;
                            vertScroller_.Maximum = 0;
                            vertScroller_.Visible = false;
                            offsetY = 0;
                        }
                        else
                        {
                            horScroller_.Maximum = 0;
                            horScroller_.Visible = false;
                            OffsetX = 0;
                            vertScroller_.Maximum = 0;
                            vertScroller_.Visible = false;
                            offsetY = 0;
                        }
                    }
                    else
                    {
                        try
                        {
                            if (base.Controls != null)
                            {
                                base.Controls.AddRange (
                                    new Control[] {horScroller_, vertScroller_, panel_});
                            }
                            haveScrollbars_ = true;
                            ResizeScrollbars ();
                        }
                        catch (Exception)
                        {
                        }
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
                        finalHVisible = horScroller_.Visible;
                        finalVVisible = vertScroller_.Visible;
                    }
                    catch
                    {
                    }
                    if ((finalHVisible != hVisible) || (finalVVisible != vVisible))
                    {
                        DoResize (base.Width, base.Height, true, true);
                    }
                }
            }
            catch
            {
                return;
            }
        }
        //
        private void HorzScrollHandler (object sender, ScrollEventArgs e)
        {
            try
            {
                if (!isInitialized_)
                {
                    return;
                }
                if ((e.Type == ScrollEventType.LargeDecrement) || (e.Type == ScrollEventType.LargeIncrement))
                {
                    builder_.HasSelection = false;
                }
                OffsetX = horScroller_.Value;
            }
            catch
            {
            }
        }
        //
        private void VertScrollHandler (object sender, ScrollEventArgs e)
        {
            try
            {
                if (!isInitialized_)
                {
                    return;
                }
                if ((e.Type == ScrollEventType.LargeDecrement) || (e.Type == ScrollEventType.LargeIncrement))
                {
                    builder_.HasSelection = false;
                }
                OffsetY = e.NewValue;
                ScrollDown ();
            }
            catch
            {
            }
        }
        //
        public void GotoNext ()
        {
            try
            {
                if (!vertScroller_.Visible)
                {
                    return;
                }
                if ((vertScroller_.Value + vertScroller_.LargeChange) < (vertScroller_.Maximum - vertScroller_.LargeChange))
                {
                    int v = vertScroller_.Value + vertScroller_.LargeChange;
                    if (v < 0)
                    {
                        v = 0;
                    }
                    vertScroller_.Value = v;
                    VertScrollHandler (this, new ScrollEventArgs (ScrollEventType.LargeIncrement, v));
                    vertScroller_.Invalidate ();
                }
                else
                {
                    int v = (vertScroller_.Maximum - vertScroller_.LargeChange) + 1;
                    if (v < 0)
                    {
                        v = 0;
                    }
                    vertScroller_.Value = v;
                    VertScrollHandler (this, new ScrollEventArgs (ScrollEventType.LargeIncrement, v));
                    vertScroller_.Invalidate ();
                }
            }
            catch
            {
            }
        }
        //
        public void GotoPrev ()
        {
            try
            {
                if (!vertScroller_.Visible)
                {
                    return;
                }
                if ((vertScroller_.Value - vertScroller_.LargeChange) >= vertScroller_.Minimum)
                {
                    int v = vertScroller_.Value - vertScroller_.LargeChange;
                    if (v < 0)
                    {
                        v = 0;
                    }
                    vertScroller_.Value = v;
                    VertScrollHandler (this, new ScrollEventArgs (ScrollEventType.LargeDecrement, v));
                    vertScroller_.Invalidate ();
                }
                else
                {
                    int vmin = vertScroller_.Minimum;
                    if (vmin < 0)
                    {
                        vmin = 0;
                    }
                    vertScroller_.Value = vmin;
                    VertScrollHandler (this, new ScrollEventArgs (ScrollEventType.LargeDecrement, vmin));
                    vertScroller_.Invalidate ();
                }
            }
            catch
            {
            }
        }
        //
        public void VertScroll (int nDiff)
        {
            try
            {
                if (nDiff > 0)
                {
                    if ((vertScroller_.Value + nDiff) <
                        ((vertScroller_.Maximum - vertScroller_.LargeChange) + 1))
                    {
                        int scrllVal = vertScroller_.Value + nDiff;
                        if (scrllVal < 0)
                        {
                            scrllVal = 0;
                        }
                        vertScroller_.Value = scrllVal;
                        VertScrollHandler (this, new ScrollEventArgs (ScrollEventType.ThumbPosition, scrllVal));
                        vertScroller_.Invalidate ();
                    }
                    else
                    {
                        int scrollVal = (vertScroller_.Maximum - vertScroller_.LargeChange) + 1;
                        if (scrollVal < 0)
                        {
                            scrollVal = 0;
                        }
                        vertScroller_.Value = scrollVal;
                        VertScrollHandler (this, new ScrollEventArgs (ScrollEventType.ThumbPosition, scrollVal));
                        vertScroller_.Invalidate ();
                    }
                }
                else
                {
                    if (nDiff >= 0)
                    {
                        return;
                    }
                    if ((vertScroller_.Value + nDiff) >= vertScroller_.Minimum)
                    {
                        int scrollval = vertScroller_.Value + nDiff;
                        if (scrollval < 0)
                        {
                            scrollval = 0;
                        }
                        vertScroller_.Value = scrollval;
                        VertScrollHandler (this, new ScrollEventArgs (ScrollEventType.ThumbPosition, scrollval));
                        vertScroller_.Invalidate ();
                    }
                    else
                    {
                        int scrollVal = vertScroller_.Minimum;
                        if (scrollVal < 0)
                        {
                            scrollVal = 0;
                        }
                        vertScroller_.Value = scrollVal;
                        VertScrollHandler (this, new ScrollEventArgs (ScrollEventType.First, scrollVal));
                        vertScroller_.Invalidate ();
                    }
                }
            }
            catch
            {
            }
        }
        //
        public void HorzScroll (int nDiff)
        {
            try
            {
                if (nDiff > 0)
                {
                    if ((horScroller_.Value + nDiff) <
                        ((horScroller_.Maximum - horScroller_.LargeChange) + 1))
                    {
                        int scroll = horScroller_.Value + nDiff;
                        if (scroll < 0)
                        {
                            scroll = 0;
                        }
                        horScroller_.Value = scroll;
                        HorzScrollHandler (this, new ScrollEventArgs (ScrollEventType.ThumbPosition, scroll));
                        horScroller_.Invalidate ();
                    }
                    else
                    {
                        int scroll = (horScroller_.Maximum - horScroller_.LargeChange) + 1;
                        if (scroll < 0)
                        {
                            scroll = 0;
                        }
                        horScroller_.Value = scroll;
                        HorzScrollHandler (this, new ScrollEventArgs (ScrollEventType.ThumbPosition, scroll));
                        horScroller_.Invalidate ();
                    }
                }
                else
                {
                    if (nDiff >= 0)
                    {
                        return;
                    }
                    if ((horScroller_.Value + nDiff) >= horScroller_.Minimum)
                    {
                        int scroll = horScroller_.Value + nDiff;
                        if (scroll < 0)
                        {
                            scroll = 0;
                        }
                        horScroller_.Value = scroll;
                        HorzScrollHandler (this, new ScrollEventArgs (ScrollEventType.ThumbPosition, scroll));
                        horScroller_.Invalidate ();
                    }
                    else
                    {
                        int scroll = horScroller_.Minimum;
                        if (scroll < 0)
                        {
                            scroll = 0;
                        }
                        horScroller_.Value = scroll;
                        HorzScrollHandler (this, new ScrollEventArgs (ScrollEventType.First, scroll));
                        horScroller_.Invalidate ();
                    }
                }
            }
            catch
            {
            }
        }
        //
        private bool GotoLast ()
        {
            try
            {
                if (builder_ != null)
                {
                    return builder_.GotoLast ();
                }
            }
            catch
            {
            }
            return false;
        }
        //
        private bool GoHome ()
        {
            try
            {
                if (builder_ != null)
                {
                    return builder_.GoHome ();
                }
            }
            catch
            {
            }
            return false;
        }
        //
        public void SelectAt (int x, int y)
        {
            if (builder_ != null)
            {
                builder_.HasSelection = false;
                builder_.SelectNode ((int) ((x + OffsetX) - lMargin), (int) ((y + OffsetY) - tMargin));
                ReRender ();
                base.Update ();
            }
        }
        //
        public bool SelectAll ()
        {
            try
            {
                if (builder_.SelectAll ())
                {
                    needUpdate = true;
                    ReRender ();
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        //
        private void RefreshAll ()
        {
            bool hasV = false;
            bool hasH = false;
            try
            {
                int diff_X = 0;
                int diff_Y = 0;
                if (!builder_.MoveToSelected (ref diff_X, ref diff_Y))
                {
                    if (diff_Y != 0)
                    {
                        try
                        {
                            if (((diff_Y < 0) && vertScroller_.Visible) &&
                                ((vertScroller_.Value + diff_Y) <= tMargin))
                            {
                                diff_Y = -vertScroller_.Value;
                            }
                        }
                        catch
                        {
                        }
                        try
                        {
                            if (diff_Y > 0)
                            {
                                int h = base.ClientRectangle.Height;
                                if (horScroller_.Visible)
                                {
                                    h -= horScroller_.Height;
                                }
                                if (vertScroller_.Visible &&
                                    (((offsetY + h) + diff_Y) >= (canvasHeight - bMargin)))
                                {
                                    diff_Y += bMargin;
                                }
                            }
                        }
                        catch
                        {
                        }
                        VertScroll (diff_Y);
                        hasV = true;
                    }
                    if (diff_X != 0)
                    {
                        try
                        {
                            if (((diff_X < 0) && horScroller_.Visible) &&
                                ((horScroller_.Value + diff_X) <= lMargin))
                            {
                                diff_X = -horScroller_.Value;
                            }
                        }
                        catch
                        {
                        }
                        try
                        {
                            if (diff_X > 0)
                            {
                                int w = base.ClientRectangle.Width;
                                if (vertScroller_.Visible)
                                {
                                    w -= vertScroller_.Width;
                                }
                                if (horScroller_.Visible &&
                                    (((offsetX + w) + diff_X) >= (canvasWidth - rMargin)))
                                {
                                    diff_X += rMargin;
                                }
                            }
                        }
                        catch
                        {
                        }
                        HorzScroll (diff_X);
                        hasH = true;
                    }
                    if (hasH || hasV)
                    {
                        base.Update ();
                    }
                }
            }
            catch
            {
            }
        }
        //
        private void SetVisibleRectangle ()
        {
            if (builder_ != null)
            {
                int width = 0;
                int height = 0;
                if ((vertScroller_ != null) && vertScroller_.Visible)
                {
                    width = base.ClientRectangle.Width - vertScroller_.Width;
                }
                else
                {
                    width = base.ClientRectangle.Width;
                }
                if (offsetX < lMargin)
                {
                    width -= lMargin - offsetX;
                }
                if ((horScroller_ != null) && horScroller_.Visible)
                {
                    height = base.ClientRectangle.Height - horScroller_.Height;
                }
                else
                {
                    height = base.ClientRectangle.Height;
                }
                if (offsetY < tMargin)
                {
                    height -= tMargin - offsetY;
                }
                int x = 0;
                int y = 0;
                if (offsetX > lMargin)
                {
                    x = offsetX - lMargin;
                }
                if (offsetY > tMargin)
                {
                    y = offsetY - tMargin;
                }
                builder_.bounds = new Rectangle (x, y, width, height);
            }
        }
        //
        public void InsertMatrixDialog ()
        {
            MatrixDialog dialog = new MatrixDialog ();
            try
            {
                dialog.ShowDialog (this);
                if (dialog.Success)
                {
                    builder_.InsertMatrix (dialog.NumRows, dialog.NumCols);
                    ReRender ();
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
                    dialog.Dispose ();
                }
                catch
                {
                }
            }
        }
        //
        private void InsertMatrix ()
        {
            if (builder_.NotUberRootNode)
            {
                InsertMatrixDialog ();
            }
        }
        //
        private void UpdateMargins ()
        {
            try
            {
                int v = Convert.ToInt32 (FontSize * 0.27f);
                lMargin = Math.Max (2, v);
                tMargin = Math.Max (2, v);
                rMargin = Math.Max (2, v);
                bMargin = Math.Max (2, v);
            }
            catch
            {
            }
        }
        //
        private void CaretThreadProc ()
        {
            try
            {
                for (;;)
                {
                    InvalidateMark();
                    Thread.Sleep(300);
                    showCaret = !showCaret;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        //
        private void InvalidateMark ()
        {
            try
            {
                base.Invalidate (new Rectangle (markX - OffsetX, markY - OffsetY, 1, caretHeight));
            }
            catch
            {
            }
        }
        //
        private void InvalidateBbox ()
        {
            try
            {
                if ((bbox.Right > 0) && (bbox.Bottom > 0))
                {
                    base.Invalidate (new Region (new Rectangle (bbox.Left, bbox.Top, bbox.Right,
                                           bbox.Bottom)));
                }
            }
            catch
            {
            }
        }
        //
        private void VertGotFocusHandler (object sender, EventArgs e)
        {
            base.ActiveControl = null;
            base.Focus ();
        }
        //
        private void HorzGotFocusHandler (object sender, EventArgs e)
        {
            base.ActiveControl = null;
            base.Focus ();
        }
        //
        private void UserPrefChanged (object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Color)
            {
                needCheckIsPaletted = true;
            }
        }
        //
        private void InitializeComponent ()
        {
            this.horScroller_ = new System.Windows.Forms.HScrollBar();
            this.vertScroller_ = new System.Windows.Forms.VScrollBar();
            this.panel_ = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // horScroller_
            // 
            this.horScroller_.Location = new System.Drawing.Point(0, 540);
            this.horScroller_.Name = "horScroller_";
            this.horScroller_.Size = new System.Drawing.Size(100, 20);
            this.horScroller_.SmallChange = 10;
            this.horScroller_.TabIndex = 1;
            this.horScroller_.Visible = false;
            this.horScroller_.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HorzScrollHandler);
            // 
            // vertScroller_
            // 
            this.vertScroller_.Location = new System.Drawing.Point(780, 3);
            this.vertScroller_.Name = "vertScroller_";
            this.vertScroller_.Size = new System.Drawing.Size(20, 100);
            this.vertScroller_.SmallChange = 10;
            this.vertScroller_.TabIndex = 0;
            this.vertScroller_.Visible = false;
            this.vertScroller_.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VertScrollHandler);
            // 
            // panel_
            // 
            this.panel_.BackColor = System.Drawing.SystemColors.Control;
            this.panel_.Location = new System.Drawing.Point(16, 10);
            this.panel_.Name = "panel_";
            this.panel_.Size = new System.Drawing.Size(752, 527);
            this.panel_.TabIndex = 2;
            this.panel_.Visible = false;
            // 
            // CEditControl_Core
            // 
            this.AllowDrop = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.horScroller_);
            this.Controls.Add(this.vertScroller_);
            this.Controls.Add(this.panel_);
            this.Name = "CEditControl_Core";
            this.Size = new System.Drawing.Size(800, 560);
            this.ResumeLayout(false);

        }
        //
        private void UndoRedoHandler (object sender, EventArgs e)
        {
            try
            {
                Event_OnUndoRedoStackChanged (this, new EventArgs ());
            }
            catch
            {
            }
        }
        //
        public void SelectActiveMenuItems (ref bool bCopy_Active, ref bool bCut_Active, ref bool bUndo_Active, ref bool bRedo_Active,
                         ref bool bProp_Fraction_Active, ref bool bProp_Table_Active, ref bool bProp_Fenced_Active,
                         ref bool bProp_Action_Active, ref bool bStyle_Active)
        {
            bCopy_Active = false;
            bCut_Active = false;
            bUndo_Active = false;
            bRedo_Active = false;
            bProp_Fraction_Active = false;
            bProp_Table_Active = false;
            bProp_Fenced_Active = false;
            bProp_Action_Active = false;
            bStyle_Active = false;
            try
            {
                bCopy_Active = CopyActive ();
                bCut_Active = CutActive ();
                bUndo_Active = UndoActive ();
                bRedo_Active = RedoActive ();
                
                Node node = builder_.GetCurrentlySelectedNode ();
                if ((node.type_ != null) && (node.type_.type == ElementType.Mfrac))
                {
                    bProp_Fraction_Active = true;
                }
                else if ((node.type_ != null) && (node.type_.type == ElementType.Mtable))
                {
                    bProp_Table_Active = true;
                }
                else if ((node.type_ != null) && (node.type_.type == ElementType.Mfenced))
                {
                    bProp_Fenced_Active = true;
                }
                else if ((node.type_ != null) && (node.type_.type == ElementType.Maction))
                {
                    bProp_Action_Active = true;
                }
                Selection sel = null;
                try
                {
                    sel = builder_.CaptureSelection ();
                    if (((sel == null) || (sel.parent == null)))
                    {
                        return;
                    }
                    bStyle_Active = true;
                }
                catch
                {
                    return;
                }
            }
            catch
            {
            }
        }
        
        //
        public void SetSchemas (string MathMLSchema)
        {
            builder_.SetSchemas (MathMLSchema);
        }
        //
        public void SetAntialias (bool antiAlias)
        {
            isAntiAlias = antiAlias;
        }
        //
        public void SetFonts (FontCollection FontCollection)
        {
            try
            {
                fonts_ = FontCollection;
                builder_.setFonts (fonts_);
            }
            catch
            {
            }
        }
        //
        public void SetWidth (int width)
        {
            try
            {
                builder_.Width = width;
            }
            catch
            {
            }
        }

        private void OnInvalidXML (object sender, InvalidXMLArgs e)
        {
            try
            {
                if (!isInitialized_)
                {
                    return;
                }
                
                try
                {
                    Event_OnValidationError (this, new ValidationErrorArgs (e.Error, e.Line, e.Pos));
                }
                catch
                {
                    return;
                }
            }
            catch
            {
            }
        }
        //
        public void Save (string sFile)
        {
            {
                if (Path.GetExtension (sFile).ToUpper () == ".JPG")
                {
                    int imgBaseline = 0;
                    int imgResolution = 0;
                    try
                    {
                        imgResolution = ImageRes;
                        if (imgResolution == 0)
                        {
                            imgResolution = 0x60;
                        }
                    }
                    catch
                    {
                    }
                    SaveAsJPEG (sFile, FontSize, imgResolution, ref imgBaseline);
                    return;
                }
                if ((builder_ != null) && (sFile.Length > 0))
                {
                    builder_.Save (sFile);
                }
            }
            base.Focus ();
        }
        public void SavePure(string file)
        {
            if ((builder_ != null) && (file.Length > 0))
            {
            builder_.SavePure (file);
            }
            base.Focus ();
        }
        //
        public void LoadXML (string sXML)
        {
            if ((sXML != null) && (sXML.Length > 0))
            {
                if (builder_.Validate (sXML))
                {
                    OffsetX = 0;
                    OffsetY = 0;
                    
                    builder_.clear ();
                    builder_.LoadXML (sXML);
                }
            }
            builder_.CanUndo = false;
            ReRender ();
            base.Focus ();
        }
        //
        public void LoadXML (string sXML, string fileName, bool bValidated)
        {
            bool ok = false;
            if (bValidated)
            {
                ok = true;
            }
            else
            {
                ok = builder_.IsValidXML (sXML);
            }
            if (ok)
            {
                OffsetX = 0;
                OffsetY = 0;
                vertScroller_.Value = 0;
                vertScroller_.Invalidate ();
                horScroller_.Value = 0;
                horScroller_.Invalidate ();
                
                builder_.clear ();
                builder_.LoadXML (sXML);
                builder_.CanUndo = false;
                ReRender ();
                base.Focus ();
            }
        }

        //
        public Bitmap Export2Image (PixelFormat pixelFormat, float fontSize, int nResolution, ref int ImgBaseLine)
        {
            try
            {
                return builder_.Export2Image (pixelFormat, fontSize, nResolution, true, ref ImgBaseLine);
            }
            catch
            {
                return null;
            }
        }
        //
        public bool SaveAsJPEG (string fileName, float fontSize, int ImgResolution, ref int ImgBaseline)
        {
            bool ok = false;
            try
            {
                string directoryName = "";
                string withoutExtension = "";
                string extension = "";
                if (fileName.Length == 0)
                {
                    return false;
                }
                directoryName = Path.GetDirectoryName (fileName);
                if (!Directory.Exists (directoryName))
                {
                    return false;
                }
                withoutExtension = Path.GetFileNameWithoutExtension (fileName);
                if (withoutExtension.Length == 0)
                {
                    return false;
                }
                extension = Path.GetExtension (fileName);
                if ((extension.Length > 0) && (extension[0] == '.'))
                {
                    extension = extension.Substring (1, extension.Length - 1);
                }
                if (extension.ToUpper () != "JPG")
                {
                    extension = "jpg";
                }
                fileName = string.Concat (new string[] {directoryName, @"\", withoutExtension, ".", extension});
                Bitmap bitmap = builder_.Export2Image (PixelFormat.Format24bppRgb, fontSize, ImgResolution, true, ref ImgBaseline);
                if (bitmap != null)
                {
                    bitmap.Save (fileName, ImageFormat.Jpeg);
                    ok = true;
                    ImageRes = ImgResolution;
                }
                if (ok)
                {
                    CommentIO io = new CommentIO ();
                    if (io.NotEmpty (fileName))
                    {
                        string xml = "";
                        
                        try
                        {
                            xml = builder_.SaveToXML ();
                        }
                        catch
                        {
                        }
                        
                        if ((xml != null) && (xml.Length > 0))
                        {
                            io.Save (xml);

                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        //
        public void InsertEntity_Open_IdentifierDictionary_Dialog (bool bIdentifier)
        {
            IdentifierDictionaryDialog dialog = new IdentifierDictionaryDialog (entityManager, fonts_, bIdentifier);
            try
            {
                dialog.ShowDialog (this);
                if (!dialog.Success)
                {
                    return;
                }
                Glyph entity = dialog.GetGlyph ();
                if (entity == null)
                {
                    return;
                }
                if (dialog.IsID)
                {
                    InsertIdentifier (entity, dialog.IsItalic, dialog.IsBold);
                }
                else
                {
                    InsertGlyph (entity);
                }
                ReRender ();
            }
            catch
            {
                return;
            }
            finally
            {
                try
                {
                    dialog.Dispose ();
                }
                catch
                {
                }
            }
        }
        //
        public void StyleProperties ()
        {
            Node cur = builder_.GetCurrentlySelectedNode ();
            try
            {
                if (((cur == null) || (cur.type_ == null)))
                {
                    return;
                }
                bool ok = false;
                StyleAttributes style = null;
                if (builder_.HasSelection)
                {
                    style = builder_.GetSelectionStyle ();
                    ok = true;
                }
                else if ((((cur != null) && (cur.InternalMark == 0)) &&
                          ((cur.type_ != null))) &&
                         (cur.type_.type != ElementType.Math))
                {
                    style = new StyleAttributes ();
                    if (cur.style_ != null)
                    {
                        cur.style_.CopyTo (style);
                    }
                    ok = true;
                }
                if (!ok)
                {
                    return;
                }
                StylePropertiesDialog dialog = new StylePropertiesDialog (style, cur.parent_);
                try
                {
                    dialog.ShowDialog (this);
                    if (dialog.Success)
                    {
                        builder_.ApplyStyleToSelection (dialog.Style);
                        ReRender ();
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
                        dialog.Dispose ();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }
        //
        public void ShowPropertiesDialog ()
        {
            Node node = builder_.GetCurrentlySelectedNode ();
            if ((node != null) && (node.type_ != null))
            {
                if (node.type_.type == ElementType.Mfrac)
                {
                    FractionProperties ();
                }
                else if (node.type_.type == ElementType.Mtable)
                {
                    MatrixProperties ();
                }
                else if (node.type_.type == ElementType.Mfenced)
                {
                    FencedProperties ();
                }
                else if (node.type_.type == ElementType.Maction)
                {
                    ActionProperties ();
                }
                else if (node.type_.type == ElementType.Mstyle)
                {
                    StyleProperties ();
                }
            }
        }
        //
        public void MatrixProperties ()
        {
            Node cur = builder_.GetCurrentlySelectedNode ();
            if (((cur != null) && (cur.type_ != null)) && (cur.type_.type == ElementType.Mtable))
            {
                MatrixPropertiesDialog dialog = new MatrixPropertiesDialog ();
                try
                {
                    dialog.SetTarget (cur);
                    dialog.ShowDialog (this);
                    if (dialog.Success && builder_.ApplyMatrixProperties (dialog.matrix))
                    {
                        ReRender ();
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
                        dialog.Dispose ();
                    }
                    catch
                    {
                    }
                }
            }
        }
        //
        public void FencedProperties ()
        {
            Node cur = builder_.GetCurrentlySelectedNode ();
            if (((cur != null) && (cur.type_ != null)) && (cur.type_.type == ElementType.Mfenced))
            {
                FencedDialog dialog = new FencedDialog (cur);
                try
                {
                    dialog.ShowDialog (this);
                    if ((dialog.Success && (dialog.FencedAttrs != null)) && builder_.ApplyFencedAttributes (cur, dialog.FencedAttrs))
                    {
                        ReRender ();
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
                        dialog.Dispose ();
                    }
                    catch
                    {
                    }
                }
            }
        }
        //
        public void ActionProperties ()
        {
            Node cur = builder_.GetCurrentlySelectedNode ();
            if (((cur != null) && (cur.type_ != null)) && (cur.type_.type == ElementType.Maction))
            {
                MActionDialog dialog = new MActionDialog (cur);
                try
                {
                    dialog.ShowDialog (this);
                    if (dialog.Success && builder_.ApplyActionAttrs (cur, dialog.attributes, dialog.Statusline))
                    {
                        ReRender ();
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
                        dialog.Dispose ();
                    }
                    catch
                    {
                    }
                }
            }
        }
        //
        public void FractionProperties ()
        {
            Node cur = builder_.GetCurrentlySelectedNode ();
            if (((cur != null) && (cur.type_ != null)) && (cur.type_.type == ElementType.Mfrac))
            {
                FractionsPropertiesDialog dialog = new FractionsPropertiesDialog (cur);
                try
                {
                    dialog.ShowDialog (this);
                    if ((dialog.Success && (dialog.FractionAttrs != null)) && builder_.ApplyFractionAttrs (cur, dialog.FractionAttrs))
                    {
                        ReRender ();
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
                        dialog.Dispose ();
                    }
                    catch
                    {
                    }
                }
            }
        }
        //
        public void InsertGlyph (Glyph entity)
        {
            if (builder_ != null)
            {
                builder_.InsertEntityOperator (entity);
                ReRender ();
            }
            base.Focus ();
        }
        //
        public void InsertEntity_Operator (string entityName)
        {
            if (builder_ != null)
            {
                builder_.InsertEntityOperator (entityName);
                ReRender ();
            }
            base.Focus ();
        }
        //
        public void InsertStretchyArrow_Under (string entityName)
        {
            if (builder_ != null)
            {
                builder_.InsertStretchyArrow_Under (entityName);
                ReRender ();
            }
            base.Focus ();
        }
        //
        public void InsertStretchyArrow_Over (string entityName)
        {
            if (builder_ != null)
            {
                builder_.InsertStretchyArrow_Over (entityName);
                ReRender ();
            }
            base.Focus ();
        }
        //
        public void InsertStretchyArrow_UnderOver (string entityName)
        {
            if (builder_ != null)
            {
                builder_.InsertStretchyArrow_UnderOver (entityName);
                ReRender ();
            }
            base.Focus ();
        }
        //
        public void InsertIdentifier (Glyph entity, bool bItalic, bool bBold)
        {
            if (builder_ != null)
            {
                builder_.insertEntity_Identifier (entity, bItalic, bBold);
                ReRender ();
            }
            base.Focus ();
        }
        //
        public void InsertEntity_Identifier (string entityName, bool bItalic, bool bBold)
        {
            if (builder_ != null)
            {
                builder_.insertEntity_Identifier (entityName, bItalic, bBold);
                ReRender ();
            }
            base.Focus ();
        }
        //
        public int OffsetY
        {
            get { return offsetY; }
            set
            {
                offsetY = value;
                SetVisibleRectangle ();
                ReRender ();
            }
        }
        
        //
        public int ImageRes
        {
            get
            {
                int r = 0;
                try
                {
                    if (builder_ != null)
                    {
                        r = builder_.HorizontalRes;
                    }
                }
                catch
                {
                }
                return r;
            }
            set
            {
                try
                {
                    if (builder_ != null)
                    {
                        builder_.HorizontalRes = value;
                    }
                }
                catch
                {
                }
            }
        }
        //
        public float FontSize
        {
            get
            {
                if (builder_ == null)
                {
                    return 12f;
                }
                try
                {
                    return builder_.FontSize;
                }
                catch
                {
                    return 12f;
                }
            }
            set
            {
                try
                {
                    float f = 12f;
                    try
                    {
                        f = value;
                    }
                    catch
                    {
                    }
                    if (builder_ != null)
                    {
                        builder_.FontSize = f;
                    }
                    try
                    {
                        UpdateMargins ();
                    }
                    catch
                    {
                    }
                    if (isInitialized_)
                    {
                        RenderWithNewFont ();
                    }
                }
                catch
                {
                }
            }
        }
        //
        public bool AutoCloseBrackets
        {
            get { return autoCloseBrackets_; }
            set { autoCloseBrackets_ = value; }
        }
        //
        public bool NonStretchyBrackets
        {
            get
            {
                bool r = false;
                if (builder_ != null)
                {
                    r = !builder_.StretchyBrackets;
                }
                return r;
            }
            set
            {
                if (builder_ != null)
                {
                    builder_.StretchyBrackets = !value;
                }
            }
        }
        //
        public int OffsetX
        {
            get { return offsetX; }
            set
            {
                if (value != offsetX)
                {
                    offsetX = value;
                    SetVisibleRectangle ();
                    horScroller_.Invalidate ();
                    ReRender ();
                }
            }
        }

        private bool needUpdate;
        private bool needCheckIsPaletted;
        private OperatorDictionary operators_;
        private bool isPaletted;

        private SelectionInfo selectionInfo;
        private bool autoCloseBrackets_;
        private FontCollection fonts_;
        private bool isInitialized_;
        private Types types;
        public EntityManager entityManager;
        public NodesBuilder builder_;
        private int canvasWidth;
        private int canvasHeight;
        public int lMargin;
        public int rMargin;
        public int tMargin;
        public int bMargin;
        private BBox bbox;
        private int markX;
        private int markY;
        private int caretHeight;
        private bool showCaret;
        private Thread caretThread;
        private Pen caretBlackPen;
        private Pen caretBluePen;
        private VScrollBar vertScroller_;
        private HScrollBar horScroller_;
        private int offsetY;
        private int offsetX;
        private Brush selectionBrush_;
        private bool isAntiAlias;
        private Rectangle rect_;
        private bool haveScrollbars_;
        public static short BITSPIXEL;
        public static short PLANES;
        private Panel panel_;
    }
}
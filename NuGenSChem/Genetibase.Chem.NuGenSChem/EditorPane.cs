using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Printing;
namespace Genetibase.Chem.NuGenSChem
{

    /*
    Custom widget for viewing and editing a molecular structure. The widget is a rather heavy one, and features a lot of the work-horse
    functions for various tools, including the specific details of user interaction, and drawing of the structure itself, with various
    annotations for editing and selection. Can optionally be used as a lightweight widget with just view and selection.*/
    public delegate void DirtyChanged(bool isDirty);

    public class DirtyChangeEventArgs : EventArgs
    {
        public DirtyChangeEventArgs(bool isDirty)
        {
            _isDirty = isDirty;
        }

        private bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
        }

    }

    [Serializable]
    public class EditorPane : System.Windows.Forms.Control// , MouseWheelListener
    {
        static private System.Int32 state4;
        private static void keyDown(System.Object event_sender, System.Windows.Forms.KeyEventArgs e)
        {
            state4 = ((int)System.Windows.Forms.Control.MouseButtons | (int)System.Windows.Forms.Control.ModifierKeys);
        }
        private static void mouseDown(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
        {
            state4 = ((int)e.Button | (int)System.Windows.Forms.Control.ModifierKeys);
        }

        internal Molecule mol;
        internal bool editable = true, hasBorder = false;
        internal const double DEFSCALE = 20; // default # Angstroms per pixel
        internal const double IDEALBOND = 1.5; // stylised bond distance (Angstroms)
        // note: px=(atomX+offsetX)*scale; ax=px/scale-offsetX; offsetX=px/scale-ax (and same for Y)
        internal double offsetX = 0, offsetY = 0; // in Angstrom units
        internal double scale = DEFSCALE; // pixels per Angstrom
        internal bool[] selected = new bool[0], dragged = null;

        public bool[] Selected
        {
            get
            {
                return selected;
            }

            set
            {
                selected = value;
            }
        }

        internal double[] px = null, py = null, rw = null, rh = null;
        internal double[][] bfx = null, bfy = null, btx = null, bty = null;
        internal int highlightAtom = 0, highlightBond = 0;

        public const int SHOW_ELEMENTS = 0;
        public const int SHOW_ALL_ELEMENTS = 1;
        public const int SHOW_INDEXES = 2;
        public const int SHOW_RINGID = 3;
        public const int SHOW_PRIORITY = 4;
        internal int showMode = SHOW_ELEMENTS;
        internal bool showHydr = true, showSter = false;

        internal const int TOOL_CURSOR = 1;
        internal const int TOOL_ROTATOR = 2;
        internal const int TOOL_ERASOR = 3;
        internal const int TOOL_ATOM = 4;
        internal const int TOOL_BOND = 5;
        internal const int TOOL_CHARGE = 6;
        internal const int TOOL_TEMPLATE = 7;

        internal const int DRAG_SELECT = 1;
        internal const int DRAG_MOVE = 2;
        internal const int DRAG_COPY = 3;
        internal const int DRAG_SCALE = 4;
        internal const int DRAG_ROTATE = 5;

        internal int trackX = -1, trackY = -1; // last seen position of mouse

        internal bool isSelectionPane = false; // false=is for editing; true=is for viewing and selecting only
        internal int selBoxW = 0, selBoxH = 0; // size to this, for selection panes

        internal MolSelectListener selectListen = null;

        internal int tool = 0;
        internal int toolDragReason = 0;
        internal double toolDragX1, toolDragY1, toolDragX2, toolDragY2;
        internal System.String toolAtomType = null;
        internal bool toolAtomDrag, toolAtomSnap;
        internal int toolAtomEditSel = 0, toolAtomEditX, toolAtomEditY;
        internal System.Windows.Forms.TextBox toolAtomEditBox = null;

        internal int toolBondOrder = 0, toolBondType = 0, toolBondFrom = 0, toolBondHit = 0;
        internal double toolBondFromX = 0, toolBondFromY = 0, toolBondToX = 0, toolBondToY = 0;
        internal bool toolSnap, toolBondDrag = false;

        internal int toolCharge = 0;

        internal const int UNDO_LEVELS = 10;
        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'EditState' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        internal class EditState
        {
            public EditState(EditorPane enclosingInstance)
            {
                InitBlock(enclosingInstance);
            }
            private void InitBlock(EditorPane enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private EditorPane enclosingInstance;
            public EditorPane Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            internal Molecule Molecule;
            internal bool[] Selected;
        }

        internal EditState[] undo = null, redo = null;

        internal Molecule template = null, templDraw = null;
        internal int templateIdx = 0;

        internal Molecule lastCleanMol = null;
        internal bool lastDirty = false;

        private Bitmap offScreenBmp;
        private Graphics offScreenDC;

        // ------------------ public functions --------------------
        
        public void RePaint()
        {
            DrawOffScreen(offScreenDC);
            Refresh();
        }       

        // Constructor for "selection only" editor panes.
        public EditorPane(int width, int height, bool selectionPane)
        {

            MouseDown += new System.Windows.Forms.MouseEventHandler(EditorPane.mouseDown);
            MouseClick += new MouseEventHandler(mouseClicked);
            MouseEnter += new EventHandler(this.mouseEntered);
            MouseLeave += new EventHandler(this.mouseExited);
            MouseDown += new MouseEventHandler(this.mousePressed);
            MouseUp += new MouseEventHandler(this.mouseReleased);
            MouseMove += new MouseEventHandler(this.mouseMoved);
            MouseMove += new MouseEventHandler(mouseDragged);
                
            isSelectionPane = selectionPane;
            if (isSelectionPane)
            {
                selBoxW = width;
                selBoxH = height;
            }
            else
            {
                this.Size = new Size(width, height);
            }
            Init();        
        }

        internal virtual void Init()
        {
            mol = new Molecule();
            DetermineSize();

            if(selected == null)
                Selected = new bool[mol.NumAtoms()];

            // Offscreen painting technique 

            offScreenBmp = new Bitmap(this.Width, this.Height);
            offScreenDC = Graphics.FromImage(offScreenBmp);
            DoubleBuffered = true;
        }

        // obtain underlying molecule; not a copy, careful about modifying
        public virtual Molecule MolData()
        {
            return mol;
        }

        public virtual bool IsEmpty()
        {
            return mol.NumAtoms() == 0;
        }

        // unit operation equivalent to deleting all atoms
        public virtual void Clear()
        {
            CacheUndo();

            mol = new Molecule();
            ClearTemporary();
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();

            CheckDirtiness();
        }

        // override the underlying molecule
        public virtual void Replace(Molecule Mol)
        {
            Replace(Mol, false, true);
        }
        public virtual void Replace(Molecule Mol, bool ClearSelection)
        {
            Replace(Mol, ClearSelection, true);
        }
        public virtual void Replace(Molecule Mol, bool ClearSelection, bool Repaint)
        {
            if (mol.NumAtoms() != Mol.NumAtoms())
                ClearSelection = true;
            mol = Mol;
            ClearTemporary(ClearSelection);
            if (Repaint)
            {
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
        }

        // set which object, if any, gets a response when an atom is "Selected" with a mouse click
        public virtual void SetMolSelectListener(MolSelectListener listen)
        {
            selectListen = listen;
        }

        // by default the editor pane captures lots of events and allows much editor; this function can be used to turn it off
        public virtual void SetEditable(bool Editable)
        {
            editable = Editable;
        }

        // if true, will draw a border around the edge
        public virtual void SetBorder(bool HasBorder)
        {
            hasBorder = HasBorder;
        }

        // informs the editor that the current state has been synchronised with what is in a disk file, or something equivalent
        public virtual void NotifySaved()
        {
            lastCleanMol = mol.Clone();
            lastDirty = false;
            if (DirtyChanged != null)
                DirtyChanged(this, new DirtyChangeEventArgs(false));
        }

        public event EventHandler<DirtyChangeEventArgs> DirtyChanged;

        // dirty==true when there have been some changes since the last modification
        public virtual bool IsDirty()
        {
            return lastDirty;
        }

        // checks to see whether the current molecule is the same as the last saved state; notifies if different; note that this is done by
        // an actual molecule comparison, which makes tracking changes a lot simpler, and also a {do something/restore it somehow} sequence
        // is functionally equivalent to undo, which is nice
        internal virtual void CheckDirtiness()
        {
            bool nowDirty = mol.CompareTo(lastCleanMol) != 0;

            if (nowDirty != lastDirty)
            {
                if (DirtyChanged != null)
                    DirtyChanged(this, new DirtyChangeEventArgs(nowDirty));
                lastDirty = nowDirty;
            }
        }

        // affect the way the molecule is rendered
        public virtual void SetShowMode(int ShowMode)
        {
            if (showMode == ShowMode)
                return;
            showMode = ShowMode;
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }
        public virtual void SetShowHydrogens(bool ShowHydr)
        {
            if (showHydr == ShowHydr)
                return;
            showHydr = ShowHydr;
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }
        public virtual void SetShowStereolabels(bool ShowSter)
        {
            if (showSter == ShowSter)
                return;
            showSter = ShowSter;
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // notify selection of various tools
        public virtual void SetToolCursor()
        {
            tool = TOOL_CURSOR;
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }
        public virtual void SetToolRotator()
        {
            tool = TOOL_ROTATOR;
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }
        public virtual void SetToolErasor()
        {
            tool = TOOL_ERASOR;
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }
        public virtual void SetToolAtom(System.String Atom)
        {
            tool = TOOL_ATOM;
            toolAtomType = Atom;
            toolAtomDrag = false;
            toolAtomSnap = false;
            toolBondFrom = 0;
            toolBondToX = 0;
            toolBondToY = 0;
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }
        public virtual void SetToolBond(int Order)
        {
            tool = TOOL_BOND;
            toolBondFrom = 0;
            if (Order >= 0)
            {
                toolBondOrder = Order; toolBondType = Molecule.BONDTYPE_NORMAL;
            }
            else
            {
                toolBondOrder = 1;
                if (Order == -1)
                    toolBondType = Molecule.BONDTYPE_INCLINED;
                else if (Order == -2)
                    toolBondType = Molecule.BONDTYPE_DECLINED;
                else if (Order == -3)
                    toolBondType = Molecule.BONDTYPE_UNKNOWN;
            }
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }
        public virtual void SetToolCharge(int DChg)
        {
            tool = TOOL_CHARGE;
            toolCharge = DChg;
        }
        public virtual void SetToolTemplate(Molecule Templ, int Idx)
        {
            tool = TOOL_TEMPLATE;
            template = Templ;
            templateIdx = Idx;
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // whether or not there is anything in the undo/redo stacks
        public virtual bool CanUndo()
        {
            return undo != null && undo[0] != null;
        }
        public virtual bool CanRedo()
        {
            return redo != null && redo[0] != null;
        }

        // cause the actual undo/redo to happen
        public virtual void Undo()
        {
            if (!CanUndo())
                return;

            if (redo == null)
                redo = new EditState[UNDO_LEVELS];
            for (int n = UNDO_LEVELS - 1; n > 0; n--)
                redo[n] = redo[n - 1];
            redo[0] = new EditState(this);
            redo[0].Molecule = mol;
            redo[0].Selected = Selected;

            mol = undo[0].Molecule;
            Selected = undo[0].Selected;
            for (int n = 0; n < UNDO_LEVELS - 1; n++)
                undo[n] = undo[n + 1];
            ClearTemporary(false);
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();

            CheckDirtiness();
        }
        public virtual void Redo()
        {
            if (!CanRedo())
                return;

            if (undo == null)
                undo = new EditState[UNDO_LEVELS];
            for (int n = UNDO_LEVELS - 1; n > 0; n--)
                undo[n] = undo[n - 1];
            undo[0] = new EditState(this);
            undo[0].Molecule = mol;
            undo[0].Selected = Selected;

            mol = redo[0].Molecule;
            Selected = redo[0].Selected;
            for (int n = 0; n < UNDO_LEVELS - 1; n++)
                redo[n] = redo[n + 1];

            ClearTemporary(false);
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();

            CheckDirtiness();
        }

        // fits the molecule on the screen and centres everything; very pleasant thing to have at certain junctures, but not too often
        public virtual void ScaleToFit()
        {
            ScaleToFit(20);
        }
        public virtual void ScaleToFit(double MaxScale)
        {
            ClearTemporary();

            double mw = 2 + mol.RangeX(), mh = 2 + mol.RangeY();
            //UPGRADE_TODO: Method 'javax.swing.JComponent.getVisibleRect' was converted to 'System.Windows.Forms.Control.DisplayRectangle' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJComponentgetVisibleRect'"
            System.Drawing.Rectangle vis = DisplayRectangle;
            double sw = selBoxW > vis.Width ? selBoxW : vis.Width;
            double sh = selBoxH > vis.Height ? selBoxH : vis.Height;
            scale = System.Math.Min(System.Math.Min(sw / mw, sh / mh), MaxScale);
            /*offsetX=1-mol.MinX();
            offsetY=1+mol.MaxY();*/
            offsetX = 0.5 * (sw / scale - mol.RangeX()) - mol.MinX();
            offsetY = 0.5 * (sh / scale - mol.RangeY()) + mol.MaxY();
        }

        // change the magnification, and adjust scrollbars etc accordingly
        public virtual void ZoomFull()
        {
            ScaleToFit();
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }
        public virtual void ZoomIn(double Mag)
        {
            scale *= Mag;
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }
        public virtual void ZoomOut(double Mag)
        {
            scale /= Mag;
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // select all atoms
        public virtual void SelectAll()
        {
            Selected = new bool[mol.NumAtoms()];
            for (int n = 0; n < mol.NumAtoms(); n++)
                Selected[n] = true;
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // finds a nice place to put the new fragment which does not overlap existing content, then appends the atoms & bonds
        public virtual void AddArbitraryFragment(Molecule Frag)
        {
            if (Frag.NumAtoms() == 0)
                return;

            CacheUndo();
            if (mol.NumAtoms() == 0)
            {
                mol = Frag;
                ClearTemporary();
                ScaleToFit();
                DetermineSize();
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
                CheckDirtiness();
                return;
            }

            //UPGRADE_NOTE: Final was removed from the declaration of 'dirX '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            //UPGRADE_NOTE: Final was removed from the declaration of 'dirY '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            double[] dirX = new double[] { 1, 0, -1, 1, -1, 1, 0, -1 };
            double[] dirY = new double[] { 1, 1, 1, 0, 0, -1, -1, -1 };
            double[] dx = new double[8], dy = new double[8], score = new double[8];

            for (int n = 0; n < 8; n++)
            {
                double vx = dirX[n], vy = dirY[n];

                if (n == 0 || n == 3 || n == 5)
                {
                    dx[n] = mol.MinX() - Frag.MaxX();
                }
                else if (n == 2 || n == 4 || n == 7)
                {
                    dx[n] = mol.MaxX() - Frag.MinX();
                }
                else
                    dx[n] = 0.5 * (mol.MinX() + mol.MaxX() - Frag.MinX() - Frag.MaxX());

                if (n == 5 || n == 6 || n == 7)
                {
                    dy[n] = mol.MinY() - Frag.MaxY();
                }
                else if (n == 0 || n == 1 || n == 2)
                {
                    dy[n] = mol.MaxY() - Frag.MinY();
                }
                else
                    dy[n] = 0.5 * (mol.MinY() + mol.MaxY() - Frag.MinY() - Frag.MaxY());

                dx[n] -= vx;
                dy[n] -= vy;
                score[n] = FragPosScore(Frag, dx[n], dy[n]);

                vx *= 0.25;
                vy *= 0.25;
                for (int iter = 100; iter > 0; iter--)
                {
                    double iscore = FragPosScore(Frag, dx[n] + vx, dy[n] + vy);
                    if (iscore <= score[n])
                        break;
                    score[n] = iscore;
                    dx[n] += vx;
                    dy[n] += vy;
                }
                for (int iter = 100; iter > 0; iter--)
                    for (int d = 0; d < 8; d++)
                    {
                        vx = dirX[d] * 0.1;
                        vy = dirY[d] * 0.1;
                        double iscore = FragPosScore(Frag, dx[n] + vx, dy[n] + vy);
                        if (iscore <= score[n])
                            break;
                        score[n] = iscore;
                        dx[n] += vx;
                        dy[n] += vy;
                    }
            }

            int best = 0;
            for (int n = 1; n < 8; n++)
                if (score[n] > score[best])
                    best = n;
            int base_Renamed = mol.NumAtoms();
            for (int n = 1; n <= Frag.NumAtoms(); n++)
            {
                mol.AddAtom(Frag.AtomElement(n), Frag.AtomX(n) + dx[best], Frag.AtomY(n) + dy[best], Frag.AtomCharge(n), Frag.AtomUnpaired(n));
            }
            for (int n = 1; n <= Frag.NumBonds(); n++)
            {
                mol.AddBond(Frag.BondFrom(n) + base_Renamed, Frag.BondTo(n) + base_Renamed, Frag.BondOrder(n), Frag.BondType(n));
            }

            ClearTemporary();
            ScaleToFit();
            DetermineSize();

            Selected = new bool[mol.NumAtoms()];
            for (int n = 0; n < mol.NumAtoms(); n++)
                Selected[n] = n >= base_Renamed;

            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
            CheckDirtiness();
        }

        // scoring function for above: more congested is better, but any two atoms < 1A = zero; post-biased to favour square aspect ratio
        internal virtual double FragPosScore(Molecule Frag, double DX, double DY)
        {
            double score = 0;
            for (int i = 1; i <= mol.NumAtoms(); i++)
                for (int j = 1; j <= Frag.NumAtoms(); j++)
                {
                    double dx = Frag.AtomX(j) + DX - mol.AtomX(i), dy = Frag.AtomY(j) + DY - mol.AtomY(i);
                    double dist2 = dx * dx + dy * dy;
                    if (dist2 < 1)
                        return 0;
                    score += 1 / dist2;
                }
            double minX = System.Math.Min(Frag.MinX() + DX, mol.MinX()), maxX = System.Math.Max(Frag.MaxX() + DX, mol.MaxX());
            double minY = System.Math.Min(Frag.MinY() + DY, mol.MinY()), maxY = System.Math.Max(Frag.MaxY() + DY, mol.MaxY());
            double rangeX = System.Math.Max(1, maxX - minX), rangeY = System.Math.Max(1, maxY - minY);
            double ratio = System.Math.Max(rangeX / rangeY, rangeY / rangeX);
            return score / ratio;
        }

        public virtual int CountSelected()
        {
            if (Selected == null)
                return 0;
            int num = 0;
            for (int n = 0; n < mol.NumAtoms(); n++)
                if (Selected[n])
                    num++;
            return num;
        }

        // returns array of atoms which are presently Selected, or everything if none
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        public List<int> SelectedIndices()
        {
            List<int> selidx = new List<int>();
            if (Selected != null)
                for (int n = 0; n < mol.NumAtoms(); n++)
                    if (Selected[n]) selidx.Add(n + 1);

            if (selidx.Count == 0)
                for (int n = 1; n <= mol.NumAtoms(); n++)
                    selidx.Add(n);

            return selidx;
        }

        // returns a subgraph of the molecule corresponding to Selected atoms - or if none, the whole thing
        public virtual Molecule SelectedSubgraph()
        {
            if (Selected == null)
                return mol.Clone();
            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
            List<int> invidx = new List<int>();
            for (int n = 0; n < mol.NumAtoms(); n++)
                invidx.Add(0);
            int sum = 0;
            for (int n = 0; n < mol.NumAtoms(); n++)
                if (Selected[n])
                    invidx[n] = ++sum;
            if (sum == 0)
                return mol.Clone();

            Molecule frag = new Molecule();
            for (int n = 1; n <= mol.NumAtoms(); n++)
                if (invidx[n - 1] > 0)
                {
                    frag.AddAtom(mol.AtomElement(n), mol.AtomX(n), mol.AtomY(n), mol.AtomCharge(n), mol.AtomUnpaired(n));
                }
            for (int n = 1; n <= mol.NumBonds(); n++)
            {
                //         	    int from=invidx.get(mol.BondFrom(n)-1),to=invidx.get(mol.BondTo(n)-1);
                int from = invidx[mol.BondFrom(n) - 1];
                int to = invidx[mol.BondTo(n) - 1];

                if (from > 0 && to > 0)
                    frag.AddBond(from, to, mol.BondOrder(n), mol.BondType(n));
            }

            return frag;
        }

        // deletes Selected atoms, or all atoms if none Selected
        public virtual void DeleteSelected()
        {
            CacheUndo();

            bool anySelected = false;
            if (Selected != null)
                for (int n = 0; n < mol.NumAtoms(); n++)
                    if (Selected[n])
                    {
                        anySelected = true; break;
                    }
            if (!anySelected)
                return;

            for (int n = mol.NumAtoms() - 1; n >= 0; n--)
                if (Selected[n])
                    mol.DeleteAtomAndBonds(n + 1);

            ClearTemporary();
            //ScaleToFit();
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
            CheckDirtiness();
        }

        // switch between explicit/implicit modes; going to explicit marks current calculated value as absolute
        public virtual void HydrogenSetExplicit(bool IsExpl)
        {
            HydrogenSetExplicit(IsExpl, Molecule.HEXPLICIT_UNKNOWN);
        }
        public virtual void HydrogenSetExplicit(bool IsExpl, int NumExpl)
        {
            CacheUndo();

            List<int> sel = SelectedIndices();

            //UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
            for (int n = 0; n < sel.Count; n++)
            {
                int i = sel[n];
                if (IsExpl)
                    mol.SetAtomHExplicit(i, mol.AtomHydrogens(i));
                else
                    mol.SetAtomHExplicit(i, NumExpl);
            }
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();

            CheckDirtiness();
        }

        // any hydrogens which are implicit or explicit are actually created as nodes in the molecular graph; the explicit value of each
        // parent is set to unknown
        public virtual void HydrogenCreateActual()
        {
            CacheUndo();

            List<int> sel = SelectedIndices();

            int[] score = new int[360];
            for (int n = 0; n < sel.Count; n++)
            {
                int i = sel[n];
                int hy = mol.AtomHydrogens(i);
                if (hy == 0)
                    continue;

                for (int j = 0; j < 360; j++)
                    score[j] = 0;
                int[] adj = mol.AtomAdjList(i);
                for (int j = 0; j < adj.Length; j++)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    int iang = (int)(System.Math.Atan2(mol.AtomY(adj[j]) - mol.AtomY(i), mol.AtomX(adj[j]) - mol.AtomX(i)) * 180 / System.Math.PI);
                    if (iang < 0)
                        iang += 360;
                    score[iang] = -1; score[(iang + 1) % 360] = -1; score[(iang + 359) % 360] = -1;
                    int i0 = (iang + 180) % 360, i1 = (iang + 120) % 360, i2 = (iang + 240) % 360;
                    if (score[i0] >= 0)
                        score[i0] += 2;
                    if (score[i1] >= 0)
                        score[i1] += 4;
                    if (score[i2] >= 0)
                        score[i2] += 4;
                }

                while (hy > 0)
                {
                    int iang = 0;
                    for (int j = 1; j < 360; j++)
                        if (score[j] > score[iang])
                            iang = j;
                    int num = mol.AddAtom("H", mol.AtomX(i) + System.Math.Cos(iang * System.Math.PI / 180.0), mol.AtomY(i) + System.Math.Sin(iang * System.Math.PI / 180.0));
                    mol.AddBond(i, num, 1);
                    score[iang] = -1; score[(iang + 1) % 360] = -1; score[(iang + 359) % 360] = -1;
                    int i0 = (iang + 180) % 360, i1 = (iang + 120) % 360, i2 = (iang + 240) % 360;
                    if (score[i0] >= 0)
                        score[i0]++;
                    if (score[i1] >= 0)
                        score[i1] += 2;
                    if (score[i2] >= 0)
                        score[i2] += 2;
                    hy--;
                }

                mol.SetAtomHExplicit(i, Molecule.HEXPLICIT_UNKNOWN);
            }

            ClearTemporary();
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();

            CheckDirtiness();
        }

        // of all the Selected atoms and their neighbours, removes any which are element H
        public virtual void HydrogenDeleteActual()
        {
            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
            List<int> sel = SelectedIndices(), chop = new List<int>();
            for (int n = 0; n < sel.Count; n++)
            {
                int i = sel[n];
                if (String.CompareOrdinal(mol.AtomElement(i), "H") == 0)
                    chop.Add((System.Int32)i);
                int[] adj = mol.AtomAdjList(i);
                for (int j = 0; j < adj.Length; j++)
                    if (String.CompareOrdinal(mol.AtomElement(adj[j]), "H") == 0)
                        chop.Add(adj[j]);
            }

            if (chop.Count == 0)
                return;
            CacheUndo();
            // Collections.sort(chop);
            chop.Sort();

            for (int n = 0; n < chop.Count; n++)
            {
                int[] adj = mol.AtomAdjList(chop[n]);
                for (int i = 0; i < adj.Length; i++)
                    mol.SetAtomHExplicit(adj[i], Molecule.HEXPLICIT_UNKNOWN);
            }

            int decr = 0, lastVal = -1;
            for (int n = 0; n < chop.Count; n++)
            {
                int i = chop[n];
                if (i == lastVal)
                    continue;
                mol.DeleteAtomAndBonds(i - decr);
                decr++;
                lastVal = i;
            }

            ClearTemporary();
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();

            CheckDirtiness();
        }

        // scale bond lengths to reasonable values (note: affects all atoms, Selected or not)
        public virtual void NormaliseBondLengths()
        {
            double numer = 0, denom = 0;
            for (int n = 1; n <= mol.NumBonds(); n++)
            {
                double dx = mol.AtomX(mol.BondFrom(n)) - mol.AtomX(mol.BondTo(n)), dy = mol.AtomY(mol.BondFrom(n)) - mol.AtomY(mol.BondTo(n));
                double weight = mol.BondInRing(n) ? 1 : 2;
                numer += System.Math.Sqrt(dx * dx + dy * dy) * weight;
                denom += weight;
            }
            if (denom == 0)
                return;

            CacheUndo();

            double stretch = IDEALBOND * denom / numer;
            for (int n = 1; n <= mol.NumAtoms(); n++)
            {
                mol.SetAtomPos(n, mol.AtomX(n) * stretch, mol.AtomY(n) * stretch);
            }


            ClearTemporary();
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // select next/prev atoms or connected components
        public virtual void CycleSelection(bool Forward, bool Group)
        {
            if (mol.NumAtoms() <= 1)
                return;

            int high = 0;
            if (Selected != null)
                for (int n = 1; n <= mol.NumAtoms(); n++)
                    if (Selected[n - 1])
                    {
                        if (Group)
                        {
                            if (mol.AtomConnComp(n) > high)
                                high = mol.AtomConnComp(n);
                        }
                        else
                        {
                            high = n;
                        }
                    }
            int max = Group ? 0 : mol.NumAtoms();
            if (Group)
                for (int n = 1; n <= mol.NumAtoms(); n++)
                    if (mol.AtomConnComp(n) > max)
                        max = mol.AtomConnComp(n);

            int pos = Forward ? high + 1 : high - 1;
            if (pos < 1)
                pos = max;
            if (pos > max)
                pos = 1;

            Selected = new bool[mol.NumAtoms()];
            for (int n = 1; n <= mol.NumAtoms(); n++)
            {
                if (Group)
                {
                    Selected[n - 1] = mol.AtomConnComp(n) == pos;
                }
                else
                {
                    Selected[n - 1] = n == pos;
                }
            }

            ClearTemporary(false);
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // move Selected atoms by a small translation
        public virtual void NudgeSelectedAtoms(double DX, double DY)
        {
            if (Selected == null)
                return;
            CacheUndo();
            for (int n = 1; n <= mol.NumAtoms(); n++)
                if (Selected[n - 1])
                    mol.SetAtomPos(n, mol.AtomX(n) + DX, mol.AtomY(n) + DY);

            ClearTemporary(false);
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // Selected atoms are inverted about a mirror plane coincident with their centre of gravity
        public virtual void FlipSelectedAtoms(bool Vertical)
        {
            if (Selected == null)
                return;

            int count = 0;
            double cx = 0, cy = 0;
            for (int n = 1; n <= mol.NumAtoms(); n++)
                if (Selected[n - 1])
                {
                    cx += mol.AtomX(n); cy += mol.AtomY(n); count++;
                }
            if (count == 0)
                return;

            CacheUndo();

            cx /= count;
            cy /= count;
            for (int n = 1; n <= mol.NumAtoms(); n++)
                if (Selected[n - 1])
                {
                    if (Vertical)
                        mol.SetAtomPos(n, mol.AtomX(n), 2 * cy - mol.AtomY(n));
                    else
                        mol.SetAtomPos(n, 2 * cx - mol.AtomX(n), mol.AtomY(n));
                }

            ClearTemporary(false);
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // Selected atoms are rotated about their centre of gravity
        public virtual void RotateSelectedAtoms(double Degrees)
        {
            if (Selected == null)
                return;

            int count = 0;
            double cx = 0, cy = 0;
            for (int n = 1; n <= mol.NumAtoms(); n++)
                if (Selected[n - 1])
                {
                    cx += mol.AtomX(n); cy += mol.AtomY(n); count++;
                }
            if (count == 0)
                return;

            CacheUndo();

            cx /= count;
            cy /= count;
            double radians = Degrees * System.Math.PI / 180;
            for (int n = 1; n <= mol.NumAtoms(); n++)
                if (Selected[n - 1])
                {
                    double dx = mol.AtomX(n) - cx, dy = mol.AtomY(n) - cy;
                    double dist = System.Math.Sqrt(dx * dx + dy * dy), theta = System.Math.Atan2(dy, dx);
                    mol.SetAtomPos(n, cx + dist * System.Math.Cos(theta + radians), cy + dist * System.Math.Sin(theta + radians));
                }

            ClearTemporary(false);
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // changes stereochemistry; STEREO_UNKNOWN=invert, POS/NEG=set to this
        public virtual void SetStereo(int Operation)
        {
            List<int> selidx = SelectedIndices();

            //UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"

            int[][] graph = new int[mol.NumAtoms()][];
            for (int n = 0; n < mol.NumAtoms(); n++)
                graph[n] = mol.AtomAdjList(n + 1);

            // chiral centres
            for (int n = 0; n < selidx.Count; n++)
            {
                int a = selidx[n];
                int ster = mol.AtomChirality(a);
                if (Operation == Molecule.STEREO_UNKNOWN)
                {
                    if (ster != Molecule.STEREO_POS && ster != Molecule.STEREO_NEG)
                        continue;
                }
                else
                {
                    if (ster == Operation)
                        continue;
                }

                // first the easy option: the atom already has chirality, can just flip all the wedges...
                if (ster == Molecule.STEREO_POS || ster == Molecule.STEREO_NEG)
                {
                    for (int i = 1; i <= mol.NumBonds(); i++)
                        if (mol.BondFrom(i) == a)
                        {
                            if (mol.BondType(i) == Molecule.BONDTYPE_INCLINED)
                                mol.SetBondType(i, Molecule.BONDTYPE_DECLINED);
                            else if (mol.BondType(i) == Molecule.BONDTYPE_DECLINED)
                                mol.SetBondType(i, Molecule.BONDTYPE_INCLINED);
                        }
                    continue;
                }

                // not quite so easy: centre has no current chirality, and a specific enantiomer has been requested
                //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
                List<int[]> perm = WedgeFormations(a, Operation);
                if (perm != null && perm.Count > 0)
                // if anything available, use best...
                {
                    int[] adj = mol.AtomAdjList(a);
                    for (int i = 0; i < adj.Length; i++)
                    {
                        int j = mol.FindBond(a, adj[i]);
                        if (j == 0)
                            continue;
                        mol.SetBondType(j, perm[0][i] < 0 ? Molecule.BONDTYPE_DECLINED : (perm[0][i] > 0 ? Molecule.BONDTYPE_INCLINED : Molecule.BONDTYPE_NORMAL));
                        if (mol.BondFrom(j) != a)
                            mol.SetBondFromTo(j, mol.BondTo(j), mol.BondFrom(j));
                    }
                }


                //ArrayList<int[]> perm = WedgeFormations(a, Operation);
                //if (perm != null && perm.size() > 0) // if anything available, use best...
                //{
                //    int[] adj = mol.AtomAdjList(a);
                //    for (int i = 0; i < adj.length; i++)
                //    {
                //        int j = mol.FindBond(a, adj[i]); if (j == 0) continue;
                //        mol.SetBondType(j, perm.get(0)[i] < 0 ? Molecule.BONDTYPE_DECLINED
                //                        : perm.get(0)[i] > 0 ? Molecule.BONDTYPE_INCLINED : Molecule.BONDTYPE_NORMAL);
                //        if (mol.BondFrom(j) != a) mol.SetBondFromTo(j, mol.BondTo(j), mol.BondFrom(j));
                //    }
                //}
            }

            // cis/trans 
            for (int n = 1; n <= mol.NumBonds(); n++)
            {
                int bf = mol.BondFrom(n), bt = mol.BondTo(n);
                if (mol.BondOrder(n) == 2 && selidx.IndexOf(bf) < 0 && selidx.IndexOf(bt) < 0)
                    continue;
                int ster = mol.BondStereo(n);
                if ((ster != Molecule.STEREO_POS && ster != Molecule.STEREO_NEG) || ster == Operation)
                    continue;
                if (mol.AtomRingBlock(bf) != 0 && mol.AtomRingBlock(bf) != mol.AtomRingBlock(bt))
                    continue; // refuse to work with ring alkene

                // classify the sides of the X=Y bond by partitioning the component
                int sc1 = 1, sc2 = 1;
                int[] side = new int[mol.NumAtoms()];
                for (int i = 0; i < mol.NumAtoms(); i++)
                    side[i] = 0;
                side[bf - 1] = 1; side[bt - 1] = 2;
                while (true)
                {
                    bool changed = false;
                    for (int i = 0; i < mol.NumAtoms(); i++)
                        if (side[i] == 0)
                            for (int j = 0; j < graph[i].Length; j++)
                                if (side[graph[i][j] - 1] != 0)
                                {
                                    side[i] = side[graph[i][j] - 1];
                                    if (side[i] == 1)
                                        sc1++;
                                    else
                                        sc2++;
                                    changed = true;
                                }
                    if (!changed)
                        break;
                }
                int which = sc1 <= sc2 ? 1 : 2;
                double cx = mol.AtomX(which == 1 ? bf : bt), cy = mol.AtomY(which == 1 ? bf : bt);
                double axis = System.Math.Atan2(cy - mol.AtomY(which == 1 ? bt : bf), cx - mol.AtomX(which == 1 ? bt : bf));
                for (int i = 0; i < mol.NumAtoms(); i++)
                    if (side[i] == which)
                    {
                        double dx = mol.AtomX(i + 1) - cx, dy = mol.AtomY(i + 1) - cy;
                        double r = System.Math.Sqrt(dx * dx + dy * dy), th = System.Math.Atan2(dy, dx);
                        th = 2 * axis - th;
                        mol.SetAtomPos(i + 1, cx + r * System.Math.Cos(th), cy + r * System.Math.Sin(th));
                    }
                for (int i = 1; i <= mol.NumBonds(); i++)
                    if (mol.BondType(i) == Molecule.BONDTYPE_INCLINED || mol.BondType(i) == Molecule.BONDTYPE_DECLINED)
                        if (side[mol.BondFrom(i) - 1] == which && side[mol.BondTo(i) - 1] == which)
                            mol.SetBondType(i, mol.BondType(i) == Molecule.BONDTYPE_INCLINED ? Molecule.BONDTYPE_DECLINED : Molecule.BONDTYPE_INCLINED);
            }

            ClearTemporary(false);
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // Selected chiral centres lose their wedge bonds
        public virtual void RemoveChiralWedges()
        {
            List<int> selidx = SelectedIndices();

            //UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
            for (int n = 0; n < selidx.Count; n++)
                if (mol.AtomChirality(selidx[n]) != Molecule.STEREO_NONE)
                {
                    for (int i = 1; i <= mol.NumBonds(); i++)
                        if ((mol.BondFrom(i) == selidx[n] || mol.BondTo(i) == selidx[n]) && (mol.BondType(i) == Molecule.BONDTYPE_INCLINED || mol.BondType(i) == Molecule.BONDTYPE_DECLINED))
                            mol.SetBondType(i, Molecule.BONDTYPE_NORMAL);
                }
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // for any chiral centres, pick the next set of valid wedge bonds
        public virtual void CycleChiralWedges()
        {
            List<int> selidx = SelectedIndices();

            //UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
            for (int n = 0; n < selidx.Count; n++)
            {
                int a = selidx[n], chi = mol.AtomChirality(a);
                if (chi != Molecule.STEREO_POS && chi != Molecule.STEREO_NEG)
                    continue;
                //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
                List<int[]> perm = WedgeFormations(a, chi);
                if (perm.Count <= 1)
                    continue; // invalid or no point

                int[] adj = mol.AtomAdjList(a), curperm = new int[adj.Length];
                for (int i = 0; i < adj.Length; i++)
                {
                    int j = mol.FindBond(a, adj[i]);
                    curperm[i] = mol.BondType(j) == Molecule.BONDTYPE_INCLINED ? 1 : (mol.BondType(j) == Molecule.BONDTYPE_DECLINED ? -1 : 0);
                }
                int match = -1;
                for (int i = 0; i < perm.Count; i++)
                {
                    int[] thisperm = perm[i];
                    bool same = true;
                    for (int j = 0; j < curperm.Length; j++)
                        if (thisperm[j] != curperm[j])
                        {
                            same = false; break;
                        }
                    if (same)
                    {
                        match = i; break;
                    }
                }
                match = (match + 1) % perm.Count;
                curperm = perm[match];

                for (int i = 0; i < adj.Length; i++)
                {
                    int j = mol.FindBond(a, adj[i]);
                    if (mol.BondFrom(j) != a)
                        mol.SetBondFromTo(j, a, adj[i]);
                    mol.SetBondType(j, curperm[i] < 0 ? Molecule.BONDTYPE_DECLINED : (curperm[i] > 0 ? Molecule.BONDTYPE_INCLINED : Molecule.BONDTYPE_NORMAL));
                }
            }
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // ------------------ private functions --------------------

        // translation of screen & molecule coordinates    
        internal virtual double AngToX(double AX)
        {
            return (offsetX + AX) * scale;
        }
        internal virtual double AngToY(double AY)
        {
            return (offsetY - AY) * scale;
        }
        internal virtual double XToAng(double PX)
        {
            return (PX / scale) - offsetX;
        }
        internal virtual double YToAng(double PY)
        {
            return ((-PY) / scale) + offsetY;
        }

        // resizes the widget, which is assumed scrollable, to fit the current magnification of the whole molecule
        internal virtual void DetermineSize()
        {
            int w, h;
            if (!isSelectionPane)
            {
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                //w = System.Math.Max((int)AngToX(mol.MaxX() + 1), 500);
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                //h = System.Math.Max((int)AngToY(mol.MinY() - 1), 500);
                //            Size = new System.Drawing.Size(w, h);
            }
            else
            {
                w = selBoxW;
                h = selBoxH;
                Size = new System.Drawing.Size(w, h);
            }                        
        }

        // erases some of the datastructures used for caching the drawing elements
        internal virtual void ClearTemporary()
        {
            ClearTemporary(true);
        }
        internal virtual void ClearTemporary(bool AndSelected)
        {
            px = py = rw = rh = null;
            highlightAtom = highlightBond = 0;
            if (AndSelected)
                Selected = null;
            else if (Selected != null && Selected.Length != mol.NumAtoms())
            {
                bool[] newSelected = new bool[mol.NumAtoms()];
                for (int n = 0; n < Selected.Length; n++)
                    newSelected[n] = Selected[n];
                Selected = newSelected;
            }
        }

        internal virtual void ResetSelected(bool Clear)
        {
            if (Selected == null)
                Selected = new bool[mol.NumAtoms()];
            if (Clear)
                for (int n = 0; n < mol.NumAtoms(); n++)
                    Selected[n] = false;
        }

        // return the atom underneath the given position, in screen coordinates; assumes that the appropriate arrays of size and position
        // have been filled out
        internal virtual int PickAtom(int X, int Y)
        {
            if (px == null || py == null)
                return 0; //DefinePositions()...?;

            for (int n = 1; n <= mol.NumAtoms(); n++)
            {
                double dx = X - px[n - 1], dy = Y - py[n - 1];
                if (System.Math.Abs(dx) <= rw[n - 1] && System.Math.Abs(dy) <= rh[n - 1])
                    if (dx * dx / (rw[n - 1] * rw[n - 1]) + dy * dy / (rh[n - 1] * rh[n - 1]) <= 1)
                    {
                        return n;
                    }
            }
            return 0;
        }

        // returns the bond underneath the screen position
        internal virtual int PickBond(int X, int Y)
        {
            if (px == null || py == null)
                return 0;

            for (int n = 1; n <= mol.NumBonds(); n++)
            {
                double x1 = px.Length > mol.BondTo(n) - 1 ? px[mol.BondFrom(n) - 1] : 0;
                double y1 = py.Length > mol.BondTo(n) - 1 ? py[mol.BondFrom(n) - 1] : 0;
                double x2 = px.Length > mol.BondTo(n) - 1 ? px[mol.BondTo(n) - 1] : 0;
                double y2 = py.Length > mol.BondTo(n) - 1 ? py[mol.BondTo(n) - 1] : 0;

                double nx1 = x1, ny1 = y1, nx2 = x2, ny2 = y2;
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int delta = System.Math.Max(2, (int)(scale / 20));
                if (nx1 > nx2)
                {
                    nx1 = x2; nx2 = x1;
                }
                if (ny1 > ny2)
                {
                    ny1 = y2; ny2 = y1;
                }
                if (X < nx1 - 2 * delta || X > nx2 + 2 * delta || Y < ny1 - 2 * delta || Y > ny2 + 2 * delta)
                    continue;

                double dx = x2 - x1, dy = y2 - y1, d;
                if (System.Math.Abs(dx) > System.Math.Abs(dy))
                    d = Y - y1 - (X - x1) * dy / dx;
                else
                    d = X - x1 - (Y - y1) * dx / dy;
                if (System.Math.Abs(d) > (2 + mol.BondOrder(n)) * delta)
                    continue;
                return n;
            }
            return 0;
        }

        // snaps the draw-to-position to multiples of 30 degrees
        internal virtual void SnapToolBond()
        {
            double cx = toolBondFrom > 0 ? mol.AtomX(toolBondFrom) : toolBondFromX;
            double cy = toolBondFrom > 0 ? mol.AtomY(toolBondFrom) : toolBondFromY;
            double dx = toolBondToX - cx, dy = toolBondToY - cy;
            double th = System.Math.Atan2(dy, dx) * 180 / System.Math.PI, ext = System.Math.Sqrt(dx * dx + dy * dy);
            //UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
            th = ((long)System.Math.Round(th / 30) * 30) * System.Math.PI / 180;
            //UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
            ext = (long)System.Math.Round(ext / IDEALBOND) * IDEALBOND;
            toolBondToX = cx + ext * System.Math.Cos(th);
            toolBondToY = cy + ext * System.Math.Sin(th);
        }

        // should be called before any unit operation is conducted; the current molecule state is stored in the undo buffer
        internal virtual void CacheUndo()
        {
            if (undo == null)
                undo = new EditState[UNDO_LEVELS];
            redo = null;
            for (int n = UNDO_LEVELS - 1; n > 0; n--)
                undo[n] = undo[n - 1];
            undo[0] = new EditState(this);
            undo[0].Molecule = mol == null ? null : mol.Clone();
            undo[0].Selected = (bool[])(Selected == null ? null : Selected.Clone());
        }

        // called when the element editing widget has ended its lifecycle, and the change is to be applied
        internal virtual void CompleteAtomEdit()
        {
            if (toolAtomEditBox == null)
                return;
            System.String el = toolAtomEditBox.Text;
            if (el.Length > 0)
            {
                CacheUndo();

                if (el[0] >= 'a' && el[0] <= 'z')
                    el = el.Substring(0, (1) - (0)).ToUpper() + el.Substring(1);

                if (toolAtomEditSel == 0)
                {
                    mol.AddAtom(el, XToAng(toolAtomEditX), YToAng(toolAtomEditY));
                    ClearTemporary();
                    DetermineSize();
                }
                else
                    mol.SetAtomElement(toolAtomEditSel, el);
            }

            toolAtomEditBox.Visible = false;
            Controls.Remove(toolAtomEditBox);
            toolAtomEditBox = null;

            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
            CheckDirtiness();
        }

        // the currently active template is rotated according to a mapping between atoms
        internal virtual void AdjustTemplateByAtom(int Atom)
        {
            templDraw = template.Clone();

            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
            List<int> bonded = new List<int>();
            for (int n = 1; n <= mol.NumBonds(); n++)
            {
                if (mol.BondFrom(n) == Atom)
                    bonded.Add((System.Int32)mol.BondTo(n));
                if (mol.BondTo(n) == Atom)
                    bonded.Add((System.Int32)mol.BondFrom(n));
            }

            //UPGRADE_NOTE: Final was removed from the declaration of 'INCR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            int INCR = 1;
            double[] rotScores = new double[360 / INCR];
            for (int n = 1; n <= templDraw.NumAtoms(); n++)
                if (n != templateIdx)
                {
                    double x = template.AtomX(n) - template.AtomX(templateIdx), y = template.AtomY(n) - template.AtomY(templateIdx);
                    double th = System.Math.Atan2(y, x), ext = System.Math.Sqrt(x * x + y * y);
                    for (int i = 0; i < (360 / INCR); i++)
                    {
                        double rx = mol.AtomX(Atom) + ext * System.Math.Cos(th + i * INCR * System.Math.PI / 180), ry = mol.AtomY(Atom) + ext * System.Math.Sin(th + i * INCR * System.Math.PI / 180);
                        for (int j = 0; j < bonded.Count; j++)
                        {
                            int k = bonded[j];
                            double dx = mol.AtomX(k) - rx, dy = mol.AtomY(k) - ry;
                            double ext2 = dx * dx + dy * dy;
                            if (ext2 < 0.01)
                                ext2 = 0.01;
                            rotScores[i] += 1 / ext2;
                        }
                    }
                }

            int bestRot = 0;
            for (int n = 1; n < (360 / INCR); n++)
                if (rotScores[n] < rotScores[bestRot])
                    bestRot = n;

            for (int n = 1; n <= templDraw.NumAtoms(); n++)
            {
                double x = template.AtomX(n) - template.AtomX(templateIdx), y = template.AtomY(n) - template.AtomY(templateIdx);
                double th = System.Math.Atan2(y, x), ext = System.Math.Sqrt(x * x + y * y);
                templDraw.SetAtomPos(n, mol.AtomX(Atom) + ext * System.Math.Cos(th + bestRot * INCR * System.Math.PI / 180), mol.AtomY(Atom) + ext * System.Math.Sin(th + bestRot * INCR * System.Math.PI / 180));
            }
        }

        // the currently active template is rotated according to a mapping between bonds
        internal virtual bool AdjustTemplateByBond(int Bond)
        {
            Molecule[] rotMol = new Molecule[2];
            double[] rotScores = new double[2];

            for (int r = 0; r < 2; r++)
            {
                rotMol[r] = template.Clone();
                int imol1 = r == 0 ? mol.BondFrom(Bond) : mol.BondTo(Bond), imol2 = r == 0 ? mol.BondTo(Bond) : mol.BondFrom(Bond);
                int irot1 = template.BondFrom(-templateIdx), irot2 = template.BondTo(-templateIdx);
                double dtheta = System.Math.Atan2(mol.AtomY(imol2) - mol.AtomY(imol1), mol.AtomX(imol2) - mol.AtomX(imol1)) - System.Math.Atan2(template.AtomY(irot2) - template.AtomY(irot1), template.AtomX(irot2) - template.AtomX(irot1));

                for (int n = 1; n <= template.NumAtoms(); n++)
                {
                    double rx = template.AtomX(n) - template.AtomX(irot1), ry = template.AtomY(n) - template.AtomY(irot1);
                    double th = System.Math.Atan2(ry, rx), ext = System.Math.Sqrt(rx * rx + ry * ry);
                    rx = mol.AtomX(imol1) + ext * System.Math.Cos(th + dtheta);
                    ry = mol.AtomY(imol1) + ext * System.Math.Sin(th + dtheta);
                    rotMol[r].SetAtomPos(n, rx, ry);

                    for (int i = 1; i <= mol.NumAtoms(); i++)
                    {
                        double dx = mol.AtomX(i) - rx, dy = mol.AtomY(i) - ry;
                        double ext2 = dx * dx + dy * dy;
                        if (ext2 < 0.01)
                            ext2 = 0.01;
                        rotScores[r] += 1 / ext2;
                    }
                }
            }

            bool swap = rotScores[0] < rotScores[1];
            templDraw = rotMol[swap ? 0 : 1];
            return swap;
        }

        // the currently active template is merely translated, as there is no current atom or bond mapping
        internal virtual void AdjustTemplateByCoord(double X, double Y)
        {
            try
            {
                templDraw = template.Clone();

                double dx = 0, dy = 0;
                if (templateIdx > 0)
                {
                    dx = template.AtomX(templateIdx); dy = template.AtomY(templateIdx);
                }
                else if (templateIdx < 0)
                {
                    int from = template.BondFrom(-templateIdx), to = template.BondTo(-templateIdx);
                    dx = 0.5 * (template.AtomX(from) + template.AtomX(to));
                    dy = 0.5 * (template.AtomY(from) + template.AtomY(to));
                }
                for (int n = 1; n <= template.NumAtoms(); n++)
                    templDraw.SetAtomPos(n, template.AtomX(n) - dx + X, template.AtomY(n) - dy + Y);
            }
            catch (Exception e)
            {

            }
        }

        // places a template, where atoms are mapped
        internal virtual void TemplateSetByAtom(int JoinAtom)
        {
            int[] map = new int[templDraw.NumAtoms()];
            int oldNum = mol.NumAtoms();
            for (int n = 1; n <= templDraw.NumAtoms(); n++)
                if (JoinAtom == 0 || n != templateIdx)
                {
                    mol.AddAtom(templDraw.AtomElement(n), templDraw.AtomX(n), templDraw.AtomY(n), templDraw.AtomCharge(n), templDraw.AtomUnpaired(n));
                }
            for (int n = 1; n <= templDraw.NumBonds(); n++)
            {
                int from = templDraw.BondFrom(n);
                int to = templDraw.BondTo(n);
                if (JoinAtom > 0)
                {
                    if (from == templateIdx)
                        from = JoinAtom;
                    else
                    {
                        if (from > templateIdx)
                            from--;
                        from += oldNum;
                    }
                    if (to == templateIdx)
                        to = JoinAtom;
                    else
                    {
                        if (to > templateIdx)
                            to--;
                        to += oldNum;
                    }
                }
                else
                {
                    from += oldNum; to += oldNum;
                }
                mol.AddBond(from, to, templDraw.BondOrder(n), templDraw.BondType(n));
            }

            ClearTemporary();
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // places a template, where bonds are mapped
        internal virtual void TemplateSetByBond(int JoinBond, bool Swap)
        {
            int[] map = new int[templDraw.NumAtoms()];
            int joinFrom = JoinBond > 0 ? mol.BondFrom(JoinBond) : 0, joinTo = JoinBond > 0 ? mol.BondTo(JoinBond) : 0;
            int newFrom = Swap ? templDraw.BondFrom(-templateIdx) : templDraw.BondTo(-templateIdx);
            int newTo = Swap ? templDraw.BondTo(-templateIdx) : templDraw.BondFrom(-templateIdx);
            for (int n = 1; n <= templDraw.NumAtoms(); n++)
            {
                if (n == newFrom && JoinBond > 0)
                    map[n - 1] = joinFrom;
                else if (n == newTo && JoinBond > 0)
                    map[n - 1] = joinTo;
                else
                {
                    map[n - 1] = mol.AddAtom(templDraw.AtomElement(n), templDraw.AtomX(n), templDraw.AtomY(n), templDraw.AtomCharge(n), templDraw.AtomUnpaired(n));
                }
            }
            for (int n = 1; n <= template.NumBonds(); n++)
                if (n != -templateIdx || JoinBond == 0)
                {
                    mol.AddBond(map[templDraw.BondFrom(n) - 1], map[templDraw.BondTo(n) - 1], templDraw.BondOrder(n), templDraw.BondType(n));
                }

            ClearTemporary();
            DetermineSize();
            //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
            RePaint();
        }

        // returns true if there are any Selected atoms
        internal virtual bool AnySelected()
        {
            if (selected == null)
                return false;
            for (int n = 0; n < mol.NumAtoms(); n++)
                if (selected[n])
                    return true;
            return false;
        }

        internal virtual double DragExtendBy(double px, double py)
        {
            double diff = 0.2 * System.Math.Sqrt(px * px + py * py) / scale;
            if (px < 0 && py < 0)
                diff = -diff;

            if (diff >= 0)
                return 1 + diff;
            else
                return System.Math.Exp(diff);
        }

        // calculate all the wedge bond formations for a given atom for a given chirality (+/-), ranked in order, null if none
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        List<int[]> WedgeFormations(int N, int Chi)
        {
            ////UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
            //List<int> selidx = new List<int>();
            //if (Selected != null)
            //    for (int n = 0; n < mol.NumAtoms(); n++)
            //        if (Selected[n])
            //            selidx.Add(n + 1);
            //if (selidx.Count == 0)
            //    for (int n = 1; n <= mol.NumAtoms(); n++)
            //        selidx.Add(n);
            //return selidx;

            if (mol.AtomAdjCount(N) != 3 && mol.AtomAdjCount(N) != 4)
                return null;
            int[] adj = mol.AtomAdjList(N);
            for (int i = 0; i < adj.Length - 1; i++)
                for (int j = i + 1; j < adj.Length; j++)
                    if (mol.AtomPriority(adj[i]) == mol.AtomPriority(adj[j]))
                        return null;

            int[] badj = new int[adj.Length];
            for (int n = 0; n < adj.Length; n++)
                badj[n] = mol.FindBond(N, adj[n]);

            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
            List<int[]> perm = new List<int[]>();

            // generate all possible sensible wedge combinations
            if (adj.Length == 3)
            {
                for (int i = 0; i < 3; i++)
                    for (int iz = -1; iz <= 1; iz += 2)
                    {
                        int[] wedges = new int[3];
                        for (int n = 0; n < 3; n++)
                            wedges[n] = 0;
                        wedges[i] = iz;
                        perm.Add(wedges);
                    }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                    for (int iz = -1; iz <= 1; iz += 2)
                    {
                        int[] wedges = new int[4];
                        for (int n = 0; n < 4; n++)
                            wedges[n] = 0;
                        wedges[i] = iz;
                        perm.Add(wedges);

                        for (int j = i + 1; j < 4; j++)
                            for (int jz = -1; jz <= 1; jz += 2)
                            {
                                if (jz == iz)
                                    continue;
                                wedges = new int[4];
                                for (int n = 0; n < 4; n++)
                                    wedges[n] = 0;
                                wedges[i] = iz;
                                wedges[j] = jz;
                                perm.Add(wedges);
                            }
                    }
            }

            // keep only the ones which indicate the desired enantiomer
            int pos = 0;
            while (pos < perm.Count)
            {
                int[] wedges = perm[pos];
                Molecule mchi = mol.Clone();
                for (int n = 0; n < adj.Length; n++)
                {
                    mchi.SetBondType(badj[n], wedges[n] < 0 ? Molecule.BONDTYPE_DECLINED : (wedges[n] > 0 ? Molecule.BONDTYPE_INCLINED : Molecule.BONDTYPE_NORMAL));
                    if (mchi.BondFrom(badj[n]) != N)
                        mol.SetBondFromTo(badj[n], mol.BondTo(badj[n]), mol.BondFrom(badj[n]));
                }
                if (mchi.AtomChirality(N) != Chi)
                    perm.RemoveAt(pos);
                else
                    pos++;
            }

            // score each one based on crude aesthetic criteria
            double[] score = new double[perm.Count];
            for (int n = 0; n < perm.Count; n++)
            {
                score[n] = 0;
                int[] wedges = perm[n];
                int wcount = 0;
                for (int i = 0; i < adj.Length; i++)
                    if (wedges[i] != 0)
                    {
                        wcount++;
                        score[n] -= 0.5 * mol.AtomPriority(adj[i]) / mol.NumAtoms();
                        if (mol.AtomAdjCount(adj[i]) == 1)
                            score[n]++;
                        if (mol.AtomRingBlock(adj[i]) > 0)
                        {
                            score[n]--;
                            if (mol.AtomRingBlock(N) == mol.AtomRingBlock(adj[i]))
                                score[n]--;
                        }
                    }
                if (adj.Length == 4 && wcount == 2)
                    score[n]++;
            }

            // sort best-first
            pos = 0;
            while (pos < perm.Count - 1)
            {
                if (score[pos] < score[pos + 1])
                {
                    int[] w1 = perm[pos], w2 = perm[pos + 1];
                    perm[pos + 1] = w1;
                    perm[pos] = w2;
                    double s = score[pos]; score[pos] = score[pos + 1]; score[pos + 1] = s;
                    if (pos > 0)
                        pos--;
                }
                else
                    pos++;
            }



            return perm;
        }

        // ------------------ event functions --------------------

        // Paint function: this is quite monolithic, which is probably a bad design decision, as many of the interactive datastructures are
        // filled out within this function, i.e. various information about what goes where is only uptodate after a paint has occurred; this
        // works relatively well as long as all of the other functions are resistant to certain things being missing.
        // 
        // The general overview is to setup various datastructures, then draw them as soon as all the information is available. Bonds are 
        // drawn first, then atoms. Adjustments to position in order to get nice aesthetics are done. Interactive drawing modes for the various
        // mouse-based tools are included. Various atom decorations are done too, including charges and hydrogens.

        private void DrawOffScreen(Graphics g)
        {
            #region PART 1: setup and preconfiguration
            System.Drawing.Color temp_Color;
            temp_Color = Color.White; // BackColor;
            System.Drawing.Color colHighlight = System.Drawing.Color.FromArgb(System.Convert.ToInt32(temp_Color.R * 0.7f), System.Convert.ToInt32(temp_Color.G * 0.7f), System.Convert.ToInt32(temp_Color.B * 0.7f));
            //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getRed' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getGreen' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            System.Drawing.Color colSelected = System.Drawing.Color.FromArgb((int)colHighlight.R, (int)colHighlight.G, 255);
            //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getRed' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            System.Drawing.Color colDragged = System.Drawing.Color.FromArgb((int)colHighlight.R, 192, 255);

            System.String[] atomLabel = new System.String[mol.NumAtoms()];
            bool[] expl = new bool[mol.NumAtoms()];
            int[] hcount = new int[mol.NumAtoms()];

            for (int n = 1; n <= mol.NumAtoms(); n++)
            {
                if (showMode == SHOW_ELEMENTS)
                    atomLabel[n - 1] = mol.AtomExplicit(n) ? mol.AtomElement(n) : "";
                else if (showMode == SHOW_ALL_ELEMENTS)
                    atomLabel[n - 1] = mol.AtomElement(n);
                else if (showMode == SHOW_INDEXES)
                    atomLabel[n - 1] = System.Convert.ToString(n);
                else if (showMode == SHOW_RINGID)
                    atomLabel[n - 1] = System.Convert.ToString(mol.AtomRingBlock(n));
                else if (showMode == SHOW_PRIORITY)
                    atomLabel[n - 1] = System.Convert.ToString(mol.AtomPriority(n));
                else
                    atomLabel[n - 1] = "?";
                expl[n - 1] = atomLabel[n - 1].Length > 0;
                hcount[n - 1] = showHydr && (showMode == SHOW_ELEMENTS || showMode == SHOW_ALL_ELEMENTS) && expl[n - 1] ? mol.AtomHydrogens(n) : 0;
            }

            SupportClass.GraphicsManager.manager.SetPaint(g, new SolidBrush( Color.White));
            g.FillRectangle(SupportClass.GraphicsManager.manager.GetPaint(g), 0, 0, Width, Height);
            if (hasBorder)
            {
                SupportClass.GraphicsManager.manager.SetColor(g, Color.Black);
                g.DrawRectangle(SupportClass.GraphicsManager.manager.GetPen(g), 0, 0, Width - 1, Height - 1);
            }

            
            //UPGRADE_ISSUE: Method 'java.awt.Graphics2D.setRenderingHint' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGraphics2DsetRenderingHint_javaawtRenderingHintsKey_javalangObject'"
            //UPGRADE_ISSUE: Field 'java.awt.RenderingHints.KEY_ANTIALIASING' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtRenderingHintsKEY_ANTIALIASING_f'"
            //UPGRADE_ISSUE: Field 'java.awt.RenderingHints.VALUE_ANTIALIAS_ON' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtRenderingHintsVALUE_ANTIALIAS_ON_f'"
            // g.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1075'"
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            //UPGRADE_TODO: Field 'java.awt.Font.PLAIN' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFontPLAIN_f'"
            System.Drawing.Font font = new System.Drawing.Font("SansSerif", (int)(0.4 * scale), System.Drawing.FontStyle.Regular);
            SupportClass.GraphicsManager.manager.SetFont(g, font);
            System.Drawing.Font metrics = SupportClass.GraphicsManager.manager.GetFont(g);
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            int txh = (int)(SupportClass.GetAscent(metrics) * 0.85);

            //UPGRADE_NOTE: If the given Font Name does not exist, a default Font instance is created. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1075'"
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            //UPGRADE_TODO: Field 'java.awt.Font.PLAIN' was converted to 'System.Drawing.FontStyle.Regular' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtFontPLAIN_f'"
            System.Drawing.Font smallFont = new System.Drawing.Font("SansSerif", (int)(0.3 * scale), System.Drawing.FontStyle.Regular);
            SupportClass.GraphicsManager.manager.SetFont(g, smallFont);
            System.Drawing.Font smallMetrics = SupportClass.GraphicsManager.manager.GetFont(g);

            px = new double[mol.NumAtoms()];
            py = new double[mol.NumAtoms()];
            rw = new double[mol.NumAtoms()];
            rh = new double[mol.NumAtoms()];
            bfx = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                bfx[i] = new double[mol.NumBonds() + 1];
            }
            bfy = new double[3][];
            for (int i2 = 0; i2 < 3; i2++)
            {
                bfy[i2] = new double[mol.NumBonds() + 1];
            }
            btx = new double[3][];
            for (int i3 = 0; i3 < 3; i3++)
            {
                btx[i3] = new double[mol.NumBonds() + 1];
            }
            bty = new double[3][];
            for (int i4 = 0; i4 < 3; i4++)
            {
                bty[i4] = new double[mol.NumBonds() + 1];
            }

            if(selected == null)
                ResetSelected(false);

            // define positions for the atoms
            for (int n = 1; n <= mol.NumAtoms(); n++)
            {
                px[n - 1] = AngToX(mol.AtomX(n));
                py[n - 1] = AngToY(mol.AtomY(n));
                if (expl[n - 1])
                {
                    rw[n - 1] = 0.5 * g.MeasureString(atomLabel[n - 1], metrics).Width;
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getDescent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    rh[n - 1] = 0.5 * (SupportClass.GetAscent(metrics) + SupportClass.GetDescent(metrics));
                    /*Rectangle2D bounds=metrics.getStringBounds(atomLabel[n-1],g);
                    rw[n-1]=0.5*bounds.getWidth();
                    rh[n-1]=0.5*bounds.getHeight();*/
                }
                else
                {
                    rw[n - 1] = rh[n - 1] = txh * 0.4;
                }
            }

            // figure out which double bonds should go on one particular side
            int[] bondside = new int[mol.NumBonds()];
            for (int n = 0; n < mol.NumBonds(); n++)
                bondside[n] = 0;
            int[][] ring6 = mol.FindRingSize(6);
            int nring6 = ring6.Length;
            for (int n = 0; n < ring6.Length; n++)
            // convert to bond indexes
            {
                int a = ring6[n][0];
                for (int i = 0; i < 6; i++)
                    ring6[n][i] = mol.FindBond(ring6[n][i], i == 5 ? a : ring6[n][i + 1]);
            }
            int[] r6score = new int[ring6.Length];
            while (true)
            {
                int best = -1;
                for (int n = 0; n < nring6; n++)
                {
                    r6score[n] = 0;
                    for (int i = 0; i < 6; i++)
                        if (mol.BondOrder(ring6[n][i]) == 2 && bondside[ring6[n][i] - 1] == 0)
                            r6score[n]++;
                    if (r6score[n] > 0 && (best < 0 || r6score[n] > r6score[best]))
                        best = n;
                }
                if (best < 0)
                    break;
                for (int n = 0; n < 6; n++)
                {
                    int bond = ring6[best][n];
                    if (mol.BondOrder(bond) != 2 || bondside[bond - 1] != 0)
                        continue;
                    int from = mol.BondFrom(bond), to = mol.BondTo(bond);
                    int numLeft = 0, numRight = 0, numFrom = 0, numTo = 0;
                    double thbond = System.Math.Atan2(mol.AtomY(to) - mol.AtomY(from), mol.AtomX(to) - mol.AtomX(from));
                    for (int i = 0; i < 6; i++)
                        if (i != n)
                        {
                            int o = mol.BondOther(ring6[best][i], from);
                            double theta;
                            if (o > 0)
                            {
                                theta = System.Math.Atan2(mol.AtomY(o) - mol.AtomY(from), mol.AtomX(o) - mol.AtomX(from));
                                numFrom++;
                            }
                            else
                            {
                                o = mol.BondOther(ring6[best][i], to);
                                if (o > 0)
                                {
                                    theta = System.Math.Atan2(mol.AtomY(o) - mol.AtomY(to), mol.AtomX(o) - mol.AtomX(to));
                                    numTo++;
                                }
                                else
                                    continue;
                            }
                            theta = theta - thbond;
                            theta += (theta > System.Math.PI ? (-2) * System.Math.PI : (theta < -System.Math.PI ? 2 * System.Math.PI : 0));
                            if (theta < 0)
                                numLeft++;
                            if (theta > 0)
                                numRight++;
                        }
                    if (numFrom > 0 && numTo > 0)
                        bondside[bond - 1] = numLeft > numRight ? -1 : 1;
                }
                if (best < nring6 - 1)
                    ring6[best] = ring6[--nring6];
            }
            // remaining bonds, if they are in a ring or are unevenly balanced
            for (int n = 1; n <= mol.NumBonds(); n++)
                if (mol.BondOrder(n) == 2 && bondside[n - 1] == 0)
                {
                    int from = mol.BondFrom(n), to = mol.BondTo(n);
                    int[] adj1 = mol.AtomAdjList(from), adj2 = mol.AtomAdjList(to);
                    if (adj1 == null || (adj1.Length >= 3 && adj2.Length >= 3 && !mol.BondInRing(n)))
                        continue; // evenly balanced chain bond, leave in middle
                    if ((adj1.Length == 1 && adj2.Length >= 3) || (adj2.Length == 1 && adj1.Length >= 3))
                        continue; // ketone-like bond
                    int numLeft = 0, numRight = 0, numFrom = 0, numTo = 0;
                    double thbond = System.Math.Atan2(mol.AtomY(to) - mol.AtomY(from), mol.AtomX(to) - mol.AtomX(from));
                    for (int i = 0; i < adj1.Length; i++)
                        if (adj1[i] != to)
                        {
                            double theta = System.Math.Atan2(mol.AtomY(adj1[i]) - mol.AtomY(from), mol.AtomX(adj1[i]) - mol.AtomX(from)) - thbond;
                            theta += (theta > System.Math.PI ? (-2) * System.Math.PI : (theta < -System.Math.PI ? 2 * System.Math.PI : 0));
                            int v = mol.BondInRing(n) && mol.AtomRingBlock(from) == mol.AtomRingBlock(adj1[i]) ? 2 : 1;
                            if (theta < 0)
                                numLeft += v;
                            if (theta > 0)
                                numRight += v;
                        }
                    for (int i = 0; i < adj2.Length; i++)
                        if (adj2[i] != from)
                        {
                            double theta = System.Math.Atan2(mol.AtomY(adj2[i]) - mol.AtomY(to), mol.AtomX(adj2[i]) - mol.AtomX(to)) - thbond;
                            theta += (theta > System.Math.PI ? (-2) * System.Math.PI : (theta < -System.Math.PI ? 2 * System.Math.PI : 0));
                            int v = mol.BondInRing(n) && mol.AtomRingBlock(to) == mol.AtomRingBlock(adj2[i]) ? 2 : 1;
                            if (theta < 0)
                                numLeft += v;
                            if (theta > 0)
                                numRight += v;
                        }
                    if (numLeft != numRight)
                        bondside[n - 1] = numLeft > numRight ? -1 : 1;
                }

            int extraAtom = ((tool == TOOL_ATOM && toolAtomDrag) || (tool == TOOL_BOND && toolBondFrom > 0)) ? 1 : 0;

            // define positions for the bonds
            for (int n = 1; n <= mol.NumBonds() + extraAtom; n++)
            {
                int from, to, order, type, side;
                double x1 = 0, y1 = 0, x2 = 0, y2 = 0, w1 = 0, h1 = 0, w2 = 0, h2 = 0;
                bool expl1 =  false, expl2 =  false;
                if (n <= mol.NumBonds())
                {
                    from = mol.BondFrom(n); to = mol.BondTo(n); order = mol.BondOrder(n); type = mol.BondType(n); side = bondside[n - 1];
                    if (px.Length > from - 1)
                        x1 = px[from - 1];
                    if (py.Length > from - 1)
                        y1 = py[from - 1]; 

                    if (px.Length > to - 1)
                        x2 = px[to - 1];

                    if (py.Length > to - 1)
                        y2 = py[to - 1];

                    if (rw.Length > from - 1)
                        w1 = rw[from - 1];
                    if (rh.Length > from - 1)
                        h1 = rh[from - 1];
                    if (rw.Length > to - 1)
                        w2 = rw[to - 1];
                    if (rh.Length > to - 1)
                        h2 = rh[to - 1];

                    if (expl.Length > from - 1)
                     expl1 = expl[from - 1];
                    if (expl.Length > to - 1)
                        expl2 = expl[to - 1];
                }
                // bond "in progress"
                else
                {
                    from = toolBondFrom; to = 0; order = toolBondOrder; type = toolBondType; side = 0;
                    x1 = px[from - 1]; y1 = py[from - 1]; x2 = AngToX(toolBondToX); y2 = AngToY(toolBondToY);
                    w1 = rw[from - 1]; h1 = rh[from - 1]; w2 = 0; h2 = 0;
                    expl1 = expl[from - 1]; expl2 = false;
                }

                double dx = x2 - x1, dy = y2 - y1;
                if (System.Math.Abs(dx) < 0.001)
                {
                    dx = 0;
                    if (expl1)
                        y1 += h1 * (dy > 0 ? 1 : -1);
                    if (expl2)
                        y2 += h2 * (dy > 0 ? -1 : 1);
                }
                else if (System.Math.Abs(dy) < 0.001)
                {
                    dy = 0;
                    if (expl1)
                        x1 += w1 * (dx > 0 ? 1 : -1);
                    if (expl2)
                        x2 += w2 * (dx > 0 ? -1 : 1);
                }
                else
                {
                    double xy = System.Math.Abs(dx / dy), yx = System.Math.Abs(dy / dx);
                    if (expl1)
                    {
                        x1 += (w1 * h1 * xy / (xy * h1 + w1)) * (dx > 0 ? 2 : -2);
                        y1 += (w1 * h1 * yx / (yx * h1 + w1)) * (dy > 0 ? 2 : -2);
                    }
                    if (expl2)
                    {
                        x2 += (w2 * h2 * xy / (xy * h2 + w2)) * (dx > 0 ? -2 : 2);
                        y2 += (w2 * h2 * yx / (yx * h2 + w2)) * (dy > 0 ? -2 : 2);
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    bfx[i][n - 1] = x1; bfy[i][n - 1] = y1; btx[i][n - 1] = x2; bty[i][n - 1] = y2;
                }

                if (order == 2 || order == 3)
                {
                    double norm = 0.15 * scale / System.Math.Sqrt(dx * dx + dy * dy);
                    double ox = Math.Sign(dy) * System.Math.Ceiling(norm * System.Math.Abs(dy) * (order == 3 ? 1 : 0.5));
                    double oy = (-Math.Sign(dx)) * System.Math.Ceiling(norm * System.Math.Abs(dx) * (order == 3 ? 1 : 0.5));
                    if (order == 2)
                    {
                        bfx[0][n - 1] += ox * (side - 1); bfy[0][n - 1] += oy * (side - 1); btx[0][n - 1] += ox * (side - 1); bty[0][n - 1] += oy * (side - 1);
                        bfx[1][n - 1] += ox * (side + 1); bfy[1][n - 1] += oy * (side + 1); btx[1][n - 1] += ox * (side + 1); bty[1][n - 1] += oy * (side + 1);

                        if (n <= mol.NumBonds() && side != 0 && mol.AtomRingBlock(from) > 0 && mol.BondInRing(n))
                        {
                            int w = side < 0 ? 0 : 1;
                            if (!expl1)
                            {
                                bfx[w][n - 1] += norm * dx; bfy[w][n - 1] += norm * dy;
                            }
                            if (!expl2)
                            {
                                btx[w][n - 1] -= norm * dx; bty[w][n - 1] -= norm * dy;
                            }
                        }
                    }
                    else
                    {
                        bfx[1][n - 1] -= ox; bfy[1][n - 1] -= oy; btx[1][n - 1] -= ox; bty[1][n - 1] -= oy;
                        bfx[2][n - 1] += ox; bfy[2][n - 1] += oy; btx[2][n - 1] += ox; bty[2][n - 1] += oy;
                    }
                }
            }
            // special case for non-ring non-explicit double bond endpoints... neighbouring single bonds get snapped "to"
            for (int n = 1; n <= mol.NumAtoms(); n++)
                if (atomLabel[n - 1].Length == 0 && mol.AtomRingBlock(n) == 0)
                {
                    bool any = false;
                    double dpx1 = 0, dpy1 = 0, dpx2 = 0, dpy2 = 0;
                    for (int i = 1; i <= mol.NumBonds(); i++)
                        if (mol.BondOrder(i) == 2)
                        {
                            if (mol.BondFrom(i) == n)
                            {
                                dpx1 = bfx[0][i - 1]; dpy1 = bfy[0][i - 1]; dpx2 = bfx[1][i - 1]; dpy2 = bfy[1][i - 1]; any = true; break;
                            }
                            if (mol.BondTo(i) == n)
                            {
                                dpx1 = btx[0][i - 1]; dpy1 = bty[0][i - 1]; dpx2 = btx[1][i - 1]; dpy2 = bty[1][i - 1]; any = true; break;
                            }
                        }
                    if (!any)
                        continue;
                    for (int i = 1; i <= mol.NumBonds(); i++)
                        if (mol.BondOrder(i) == 1)
                        {
                            if (mol.BondFrom(i) == n)
                            {
                                double dx1 = dpx1 - btx[0][i - 1], dy1 = dpy1 - bty[0][i - 1];
                                double dx2 = dpx2 - btx[0][i - 1], dy2 = dpy2 - bty[0][i - 1];
                                if (dx1 * dx1 + dy1 * dy1 < dx2 * dx2 + dy2 * dy2)
                                {
                                    bfx[0][i - 1] = dpx1; bfy[0][i - 1] = dpy1;
                                }
                                else
                                {
                                    bfx[0][i - 1] = dpx2; bfy[0][i - 1] = dpy2;
                                }
                            }
                            else if (mol.BondTo(i) == n)
                            {
                                double dx1 = dpx1 - bfx[0][i - 1], dy1 = dpy1 - bfy[0][i - 1];
                                double dx2 = dpx2 - bfx[0][i - 1], dy2 = dpy2 - bfy[0][i - 1];
                                if (dx1 * dx1 + dy1 * dy1 < dx2 * dx2 + dy2 * dy2)
                                {
                                    btx[0][i - 1] = dpx1; bty[0][i - 1] = dpy1;
                                }
                                else
                                {
                                    btx[0][i - 1] = dpx2; bty[0][i - 1] = dpy2;
                                }
                            }
                        }
                }

            // for drawn-in hydrogens, work out which quadrant they are to be drawn on; done by working out angles to bonds, and finding the
            // lowest sum total of distance; note values: 1=east,2=north,3=west,4=south (+N*90 degrees)
            int[] hdir = new int[mol.NumAtoms()];
            for (int n = 1; n <= mol.NumAtoms(); n++)
            {
                hdir[n - 1] = 0;
                if (hcount[n - 1] == 0)
                    continue;
                int[] bonds = mol.AtomAdjList(n);
                int avoid1 = 0, avoid2 = 20, avoid3 = 10, avoid4 = 20;
                for (int i = 0; i < bonds.Length + (extraAtom == 1 && toolBondFrom == n ? 1 : 0); i++)
                {
                    double x = i < bonds.Length ? mol.AtomX(bonds[i]) : toolBondToX, y = i < bonds.Length ? mol.AtomY(bonds[i]) : toolBondToY;
                    double theta = System.Math.Atan2(y - mol.AtomY(n), x - mol.AtomX(n)) * 180 / System.Math.PI;
                    double dt1 = 0 - theta, dt2 = 90 - theta, dt3 = 180 - theta, dt4 = 270 - theta;
                    dt1 = System.Math.Abs(dt1 + (dt1 < -180 ? 360 : 0) + (dt1 > 180 ? -360 : 0));
                    dt2 = System.Math.Abs(dt2 + (dt2 < -180 ? 360 : 0) + (dt2 > 180 ? -360 : 0));
                    dt3 = System.Math.Abs(dt3 + (dt3 < -180 ? 360 : 0) + (dt3 > 180 ? -360 : 0));
                    dt4 = System.Math.Abs(dt4 + (dt4 < -180 ? 360 : 0) + (dt4 > 180 ? -360 : 0));
                    avoid1 += (dt1 < 80 ? 80 - ((int)dt1) : 0);
                    avoid2 += (dt2 < 80 ? 80 - ((int)dt2) : 0);
                    avoid3 += (dt3 < 80 ? 80 - ((int)dt3) : 0);
                    avoid4 += (dt4 < 80 ? 80 - ((int)dt4) : 0);
                }
                if (avoid1 <= avoid2 && avoid1 <= avoid3 && avoid1 <= avoid4)
                    hdir[n - 1] = 1;
                else if (avoid3 <= avoid2 && avoid3 <= avoid4)
                    hdir[n - 1] = 3;
                else if (avoid2 <= avoid4)
                    hdir[n - 1] = 2;
                else
                    hdir[n - 1] = 4;
            }

            #endregion
            #region PART 2: backlighting from selection and highlights

            for (int n = 1; n <= mol.NumAtoms(); n++)
            {
                bool drag = false;
                if (dragged != null)
                    drag = dragged[n - 1];

                if (selected[n - 1] || n == highlightAtom || drag)
                {
                    SupportClass.GraphicsManager.manager.SetColor(g, selected[n - 1] ? colSelected : (drag ? colDragged : colHighlight));
                    double ext = System.Math.Max(rw[n - 1] * 1.2, rh[n - 1] * 1.2);                    
                    GraphicsPath path = SupportClass.Ellipse2DSupport.CreateEllipsePath((float)(px[n - 1] - ext), (float)(py[n - 1] - ext), (float)(2 * ext), (float)(2 * ext));
                    Brush brush = new SolidBrush(Color.FromArgb(125, Color.Gray));                    
                    g.FillPath(brush, path);
                }
            }

            if (highlightBond != 0)
                for (int n = 0; n == 0 || n < mol.BondOrder(highlightBond) && n < 3; n++)
                {
                    int bn = highlightBond - 1;
                    double x1 = bfx[n][bn], y1 = bfy[n][bn], x2 = btx[n][bn], y2 = bty[n][bn];
                    double dx = x2 - x1, dy = y2 - y1;
                    double norm = 0.15 * scale / System.Math.Sqrt(dx * dx + dy * dy);
                    double ox = norm * dy, oy = (-norm) * dx;

                    System.Drawing.Drawing2D.GraphicsPath pgn = new System.Drawing.Drawing2D.GraphicsPath();

                    SupportClass.AddPointToGraphicsPath(pgn, (int)System.Math.Round(x1 + oy * 0.5), (int)System.Math.Round(y1 - ox * 0.5));
                    SupportClass.AddPointToGraphicsPath(pgn, (int)System.Math.Round(x1 - ox), (int)System.Math.Round(y1 - oy));
                    SupportClass.AddPointToGraphicsPath(pgn, (int)System.Math.Round(x2 - ox), (int)System.Math.Round(y2 - oy));
                    SupportClass.AddPointToGraphicsPath(pgn, (int)System.Math.Round(x2 - oy * 0.5), (int)System.Math.Round(y2 + ox * 0.5));
                    SupportClass.AddPointToGraphicsPath(pgn, (int)System.Math.Round(x2 + ox), (int)System.Math.Round(y2 + oy));
                    SupportClass.AddPointToGraphicsPath(pgn, (int)System.Math.Round(x1 + ox), (int)System.Math.Round(y1 + oy));
                    SupportClass.GraphicsManager.manager.SetColor(g, colHighlight);

                    Brush brush = new SolidBrush(Color.FromArgb(125, Color.Gray));     
                    g.FillPath(brush, pgn);
                }

            #endregion
            #region PART 3: draw the bonds

            for (int n = 1; n <= mol.NumBonds() + extraAtom; n++)
            {
                double x1 = bfx[0][n - 1], y1 = bfy[0][n - 1], x2 = btx[0][n - 1], y2 = bty[0][n - 1], dx = x2 - x1, dy = y2 - y1;
                int order = n <= mol.NumBonds() ? mol.BondOrder(n) : toolBondOrder;
                int type = n <= mol.NumBonds() ? mol.BondType(n) : toolBondType;

                SupportClass.GraphicsManager.manager.SetColor(g, Color.Black);

                if (type == Molecule.BONDTYPE_INCLINED)
                {
                    double norm = 0.15 * scale / System.Math.Sqrt(dx * dx + dy * dy);
                    double ox = norm * dy, oy = (-norm) * dx;
                    System.Drawing.Drawing2D.GraphicsPath pgn = new System.Drawing.Drawing2D.GraphicsPath();
                    //UPGRADE_TODO: Method 'java.awt.Polygon.addPoint' was converted to 'SupportClass.AddPointToGraphicsPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtPolygonaddPoint_int_int'"
                    //UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
                    SupportClass.AddPointToGraphicsPath(pgn, (int)System.Math.Round(x1), (int)System.Math.Round(y1));
                    //UPGRADE_TODO: Method 'java.awt.Polygon.addPoint' was converted to 'SupportClass.AddPointToGraphicsPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtPolygonaddPoint_int_int'"
                    //UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
                    SupportClass.AddPointToGraphicsPath(pgn, (int)System.Math.Round(x2 - ox), (int)System.Math.Round(y2 - oy));
                    //UPGRADE_TODO: Method 'java.awt.Polygon.addPoint' was converted to 'SupportClass.AddPointToGraphicsPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtPolygonaddPoint_int_int'"
                    //UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
                    SupportClass.AddPointToGraphicsPath(pgn, (int)System.Math.Round(x2 + ox), (int)System.Math.Round(y2 + oy));
                    //UPGRADE_TODO: Method 'java.awt.Graphics2D.fill' was converted to 'System.Drawing.Graphics.FillPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Dfill_javaawtShape'"
                    g.FillPath(Brushes.Black, pgn);
                }
                else if (type == Molecule.BONDTYPE_DECLINED)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    int nsteps = (int)System.Math.Ceiling(System.Math.Sqrt(dx * dx + dy * dy) * 0.15);
                    double norm = 0.15 * scale / System.Math.Sqrt(dx * dx + dy * dy);
                    double ox = norm * dy, oy = (-norm) * dx;
                    for (int i = 0; i <= nsteps + 1; i++)
                    {
                        double cx = x1 + i * dx / (nsteps + 1), cy = y1 + i * dy / (nsteps + 1);
                        double ix = ox * i / (nsteps + 1), iy = oy * i / (nsteps + 1);
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                        //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float'"
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        SupportClass.GraphicsManager.manager.SetPen(g, new System.Drawing.Pen(System.Drawing.Brushes.Black, (float)(0.05 * scale)));
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                        g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)(cx - ix), (float)(cy - iy), (float)(cx + ix), (float)(cy + iy)));
                    }
                }
                else if (type == Molecule.BONDTYPE_UNKNOWN)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    int nsteps = (int)System.Math.Ceiling(System.Math.Sqrt(dx * dx + dy * dy) * 0.2);
                    double norm = 0.2 * scale / System.Math.Sqrt(dx * dx + dy * dy);
                    double ox = norm * dy, oy = (-norm) * dx;
                    for (int i = 0; i <= nsteps; i++)
                    {
                        double ax = x1 + i * dx / (nsteps + 1), ay = y1 + i * dy / (nsteps + 1);
                        double cx = x1 + (i + 1) * dx / (nsteps + 1), cy = y1 + (i + 1) * dy / (nsteps + 1);
                        double bx = (ax + cx) / 2, by = (ay + cy) / 2;
                        int sign = i % 2 == 0 ? 1 : -1;
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                        //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float'"
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        SupportClass.GraphicsManager.manager.SetPen(g, new System.Drawing.Pen(System.Drawing.Brushes.Black, (float)(0.05 * scale)));
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                        //UPGRADE_ISSUE: Constructor 'java.awt.geom.QuadCurve2D.Double.Double' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtgeomQuadCurve2DDouble'"


                        PointF[] pts = new PointF[] { new PointF((float)ax, (float)ay), new PointF((float)(bx + sign * ox), (float)(by + sign * oy)), new PointF((float)cx, (float)cy) };
                        byte[] pttypes =  new byte[] { (byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.CloseSubpath };

                        //GraphicsPath path = new GraphicsPath(pts, pttypes);
                        //g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), path);

                        g.DrawLine(Pens.Black, pts[0], pts[1]);
                        g.DrawLine(Pens.Black, pts[1], pts[2]);
                        //g.DrawLine(Pens.Black, pts[2], pts[1]);
                    }
                }
                else if (order == 0)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    int nsteps = (int)System.Math.Ceiling(System.Math.Sqrt(dx * dx + dy * dy) * 0.10);
                    for (int i = 0; i <= nsteps + 1; i++)
                    {
                        double cx = x1 + i * dx / (nsteps + 1), cy = y1 + i * dy / (nsteps + 1);
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.fill' was converted to 'System.Drawing.Graphics.FillPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Dfill_javaawtShape'"
                        g.FillPath(Brushes.Black, SupportClass.Ellipse2DSupport.CreateEllipsePath((float)(cx - 0.05 * scale), (float)(cy - 0.05 * scale), (float)(0.1 * scale), (float)(0.1 * scale)));
                    }
                }
                else
                {
                    //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                    //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float_int_int'"
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    SupportClass.GraphicsManager.manager.SetPen(g, SupportClass.StrokeConsSupport.CreatePenInstance((float)(0.075 * scale), (int)System.Drawing.Drawing2D.LineCap.Round, (int)System.Drawing.Drawing2D.LineJoin.Round));
                    for (int i = order <= 3 ? order - 1 : 2; i >= 0; i--)
                    {
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                        g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)bfx[i][n - 1], (float)bfy[i][n - 1], (float)btx[i][n - 1], (float)bty[i][n - 1]));
                    }
                }
            }

            #endregion
            #region PART 4: draw the atoms

            for (int n = 1; n <= mol.NumAtoms(); n++)
            {
                if (atomLabel[n - 1].Length == 0)
                    continue;

                SupportClass.GraphicsManager.manager.SetPaint(g, new SolidBrush(showMode == SHOW_INDEXES ? Color.Blue : (showMode == SHOW_RINGID ? Color.Red : (showMode == SHOW_PRIORITY ? Color.DarkGreen : Color.Black))));
                SupportClass.GraphicsManager.manager.SetFont(g, font);
                //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                double hx = px[n - 1], hy = py[n - 1] - 0.1 * SupportClass.GetAscent(metrics);
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DdrawString_javalangString_float_float'"
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"                

                if (AtomIsBelowBond(n))
                    hy += 1.0 * SupportClass.GetAscent(metrics);

                g.DrawString(atomLabel[n - 1], SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetPaint(g), (float)(hx - rw[n - 1]), (float)(hy + 0.5 * SupportClass.GetAscent(metrics)) - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());                

                if (hcount[n - 1] > 0)
                {
                    //UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
                    SizeF hSize = g.MeasureString("H", metrics);

                    int bigHWid = (int)hSize.Width; //  metrics.stringWidth("H");
                    //UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"

                    int subHWid = hcount[n - 1] > 1 ? (int)g.MeasureString(System.Convert.ToString(hcount[n - 1]), smallMetrics).Width : 0;
                    if (hdir[n - 1] == 1)
                    {
                        hx += rw[n - 1];
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        hy += 0.5 * SupportClass.GetAscent(metrics);
                    }
                    else if (hdir[n - 1] == 3)
                    {
                        hx -= (rw[n - 1] + bigHWid + subHWid);
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        hy += 0.5 * SupportClass.GetAscent(metrics);
                    }
                    else if (hdir[n - 1] == 2)
                    {
                        hx -= rw[n - 1];
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        hy -= 1.2/*0.5*/ * SupportClass.GetAscent(metrics);
                    }
                    else if (hdir[n - 1] == 4)
                    {
                        hx -= rw[n - 1];
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        hy += 2.1 * SupportClass.GetAscent(metrics);
                    }
                    SupportClass.GraphicsManager.manager.SetPaint(g, new SolidBrush(Color.Black));
                    //UPGRADE_TODO: Method 'java.awt.Graphics2D.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DdrawString_javalangString_float_float'"
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"                    
                    g.DrawString("H", SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetPaint(g), (float)hx, (float)hy - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());                    
                    
                    if (hcount[n - 1] > 1)
                    {
                        SupportClass.GraphicsManager.manager.SetFont(g, smallFont);
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DdrawString_javalangString_float_float'"
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"                        
                        g.DrawString(System.Convert.ToString(hcount[n - 1]), SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetPaint(g), (float)hx + bigHWid, (float)hy - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
                    }
                }
            }

            #endregion
            #region PART 5: draw annotations, such as +, -, .

            //UPGRADE_NOTE: Final was removed from the declaration of 'ANNOT_PLUS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            //UPGRADE_NOTE: Final was removed from the declaration of 'ANNOT_MINUS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            //UPGRADE_NOTE: Final was removed from the declaration of 'ANNOT_RAD '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            int ANNOT_PLUS = 1;
            int ANNOT_MINUS = 2;
            int ANNOT_RAD = 3;
            //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
            List<int> annot = new List<int>();
            double usz = scale * 0.3;

            for (int n = 1; n <= mol.NumAtoms(); n++)
            {
                annot.Clear();
                int chg = mol.AtomCharge(n);
                while (chg < 0)
                {
                    annot.Add(ANNOT_MINUS); chg++;
                }
                while (chg > 0)
                {
                    annot.Add(ANNOT_PLUS); chg--;
                }
                int rad = mol.AtomUnpaired(n);
                while (rad > 0)
                {
                    annot.Add(ANNOT_RAD); rad--;
                }

                if (annot.Count == 0)
                    continue;

                double bw = annot.Count * usz, bh = usz;
                //UPGRADE_NOTE: Final was removed from the declaration of 'ANG_INCR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                //UPGRADE_NOTE: Final was removed from the declaration of 'CLOCK_SZ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                //UPGRADE_NOTE: Final was removed from the declaration of 'SAMP_SZ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                int ANG_INCR = 5;
                int CLOCK_SZ = 360 / ANG_INCR;
                int SAMP_SZ = 3 * CLOCK_SZ;
                //UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                double bestAng = 0, bestExt = 0, bestScore = System.Double.MaxValue;
                for (int i = 1; i <= 3; i++)
                    for (int j = 0; j < CLOCK_SZ; j++)
                    {
                        double ang = j * ANG_INCR;
                        if (hdir[n - 1] == 1 && (ang <= 45 || ang >= 315))
                            continue;
                        if (hdir[n - 1] == 4 && ang >= 45 && ang <= 135)
                            continue;
                        if (hdir[n - 1] == 3 && ang >= 135 && ang <= 225)
                            continue;
                        if (hdir[n - 1] == 2 && ang >= 225 && ang <= 315)
                            continue;

                        double ext = 0.5 * (rw[n - 1] + rw[n - 1]) + i * scale * 0.25;
                        double from45 = System.Math.Min(System.Math.Abs(315 - ang + (ang < 135 ? -360 : 0)), 90);
                        double score = 10 * ext + 0.01 * from45;
                        if (score > bestScore)
                            continue;

                        double ax = px[n - 1] + ext * System.Math.Cos(ang * System.Math.PI / 180.0);
                        double ay = py[n - 1] + ext * System.Math.Sin(ang * System.Math.PI / 180.0);

                        for (int k = 1; k <= mol.NumBonds(); k++)
                        {
                            /* DISTANCE FROM POINT TO LINE SEGMENT:
                            Vector v = S.P1 - S.P0;
                            Vector w = P - S.P0;
							
                            double c1 = dot(w,v);
                            if ( c1 <= 0 )
                            return d(P, S.P0);
							
                            double c2 = dot(v,v);
                            if ( c2 <= c1 )
                            return d(P, S.P1);
							
                            double b = c1 / c2;
                            Point Pb = S.P0 + b * v;
                            return d(P, Pb);
                            */
                            double dsq = 0;
                            double x1 = px[mol.BondFrom(k) - 1], y1 = py[mol.BondFrom(k) - 1], x2 = px[mol.BondTo(k) - 1], y2 = py[mol.BondTo(k) - 1];
                            double vx = x2 - x1, vy = y2 - y1, wx = ax - x1, wy = ay - y1;
                            double c1 = vx * wx + vy * wy;
                            if (c1 <= 0)
                                dsq = (ax - x1) * (ax - x1) + (ay - y1) * (ay - y1);
                            else
                            {
                                double c2 = vx * vx + vy * vy;
                                if (c2 <= c1)
                                    dsq = (ax - x2) * (ax - x2) + (ay - y2) * (ay - y2);
                                else
                                {
                                    double b = c1 / c2;
                                    double bx = x1 + b * vx, by = y1 + b * vy;
                                    dsq = (ax - bx) * (ax - bx) + (ay - by) * (ay - by);
                                }
                            }

                            score += 100 / System.Math.Max(dsq, 0.0001);
                        }

                        if (score < bestScore)
                        {
                            bestAng = ang;
                            bestExt = ext;
                            bestScore = score;
                        }
                    }

                double ax2 = px[n - 1] + bestExt * System.Math.Cos(bestAng * System.Math.PI / 180.0) - 0.5 * bw;
                double ay2 = py[n - 1] + bestExt * System.Math.Sin(bestAng * System.Math.PI / 180.0) - 0.5 * bh;
                SupportClass.GraphicsManager.manager.SetColor(g, Color.Black);
                for (int i = 0; i < annot.Count; i++)
                {
                    int type = annot[i];
                    double x1 = ax2 + 0.2 * usz, x2 = ax2 + 0.8 * usz, y1 = ay2 + 0.2 * usz, y2 = ay2 + 0.8 * usz;
                    if (type == ANNOT_MINUS || type == ANNOT_PLUS)
                    {
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                        g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)x1, (float)(0.5 * (y1 + y2)), (float)x2, (float)(0.5 * (y1 + y2))));
                    }
                    if (type == ANNOT_PLUS)
                    {
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                        g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)(0.5 * (x1 + x2)), (float)y1, (float)(0.5 * (x1 + x2)), (float)y2));
                    }
                    if (type == ANNOT_RAD)
                    {
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.fill' was converted to 'System.Drawing.Graphics.FillPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Dfill_javaawtShape'"
                        g.FillPath(Brushes.Black, SupportClass.Ellipse2DSupport.CreateEllipsePath((float)(ax2 + 0.2 * usz), (float)(ay2 + 0.2 * usz), (float)(0.6 * usz), (float)(0.6 * usz)));
                    }
                    ax2 += usz;
                }
            }

            #endregion
            #region PART 6: draw effects of various tools

            // special draw-over for dragging a new atom into place
            if (tool == TOOL_ATOM && toolAtomDrag && toolAtomType != null && String.CompareOrdinal(toolAtomType, "C") != 0)
            {
                SupportClass.GraphicsManager.manager.SetPaint(g, new SolidBrush(Color.Blue));
                SupportClass.GraphicsManager.manager.SetFont(g, font);
                double tx = AngToX(toolBondToX), ty = AngToY(toolBondToY);
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DdrawString_javalangString_float_float'"
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                //UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
                //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getAscent' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                g.DrawString(toolAtomType, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetPaint(g), (float)(tx - 0.5 * g.MeasureString(toolAtomType, metrics).Width), (float)(ty + 0.4 * SupportClass.GetAscent(metrics)) - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
            }
            if (tool == TOOL_BOND && toolBondFrom == 0 && toolBondDrag)
            {
                SupportClass.GraphicsManager.manager.SetColor(g, Color.Black);
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int i = PickAtom((int)AngToX(toolBondToX), (int)AngToY(toolBondToY));
                if (i == 0 && toolSnap)
                    SnapToolBond();
                double x1 = toolBondFromX, y1 = toolBondFromY, x2 = toolBondToX, y2 = toolBondToY;
                if (i > 0)
                {
                    x2 = mol.AtomX(i); y2 = mol.AtomY(i);
                }
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)AngToX(x1), (float)AngToY(y1), (float)AngToX(x2), (float)AngToY(y2)));
            }
            if (toolDragReason == DRAG_SELECT)
            {
                // TODO: What dos setXORMode do? g.setXORMode(Color.Yellow);
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int x = (int)toolDragX1, y = (int)toolDragY1, w = (int)(toolDragX2 - x), h = (int)(toolDragY2 - y);
                if (w < 0)
                {
                    w = -w; x -= w;
                }
                if (h < 0)
                {
                    h = -h; y -= h;
                }
                g.DrawRectangle(SupportClass.GraphicsManager.manager.GetPen(g), x, y, w, h);
            }
            if ((toolDragReason == DRAG_MOVE || toolDragReason == DRAG_COPY || toolDragReason == DRAG_SCALE) && (toolDragX1 != toolDragX2 || toolDragY1 != toolDragY2))
            {
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int dx = (int)(toolDragX2 - toolDragX1), dy = (int)(toolDragY2 - toolDragY1);

                double extmul = 0, cx = 0, cy = 0; // scaling only
                if (toolDragReason == DRAG_SCALE)
                {
                    extmul = DragExtendBy(toolDragX2 - toolDragX1, toolDragY2 - toolDragY1);
                    int count = 0;
                    for (int n = 1; n <= mol.NumAtoms(); n++)
                        if (Selected[n - 1])
                        {
                            cx += mol.AtomX(n); cy += mol.AtomY(n); count++;
                        }
                    cx /= count; cy /= count;
                }


                for (int n = 1; n <= mol.NumAtoms(); n++)
                    if (Selected[n - 1])
                    {
                        double sx, sy;
                        if (toolDragReason == DRAG_MOVE || toolDragReason == DRAG_COPY)
                        {
                            sx = px[n - 1] + dx; sy = py[n - 1] + dy;
                        }
                        // ==DRAG_SCALE
                        else
                        {
                            sx = AngToX((mol.AtomX(n) - cx) * extmul + cx);
                            sy = AngToY((mol.AtomY(n) - cy) * extmul + cy);
                        }

                        if (toolDragReason == DRAG_SCALE)
                        {
                            SupportClass.GraphicsManager.manager.SetColor(g, extmul < 0 ? Color.Red : Color.Blue);
                            //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                            //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float_int_int_float_float[]_float'"
                            SupportClass.GraphicsManager.manager.SetPen(g, SupportClass.StrokeConsSupport.CreatePenInstance(0.5F, (int)System.Drawing.Drawing2D.LineCap.Round, (int)System.Drawing.Drawing2D.LineJoin.Round, 1F, new float[] { 2, 2 }, 0));
                            //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                            g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)toolDragX1, (float)toolDragY1, (float)toolDragX2, (float)toolDragY2));
                        }

                        // TODO: Double check - we should be able to do all this with the right pen
                        // SupportClass.GraphicsManager.manager.SetColor(g, Color.Black);

                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                        //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float'"
                        SupportClass.GraphicsManager.manager.SetPen(g, new System.Drawing.Pen(System.Drawing.Brushes.Black, 1.1F));
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                        g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Ellipse2DSupport.CreateEllipsePath((float)(sx - scale * 0.3), (float)(sy - scale * 0.3), (float)(scale * 0.6), (float)(scale * 0.6)));
                        if (toolDragReason == DRAG_COPY)
                        {
                            //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                            g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)(sx - scale * 0.15), (float)sy, (float)(sx + scale * 0.15), (float)sy));
                            //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                            g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)sx, (float)(cy - scale * 0.15), (float)sx, (float)(sy + scale * 0.15)));
                        }
                    }
            }
            if (toolDragReason == DRAG_ROTATE && (System.Math.Abs(toolDragX2 - toolDragX1) > 5 || System.Math.Abs(toolDragY2 - toolDragY1) > 5))
            {
                double dx = toolDragX2 - toolDragX1, dy = toolDragY2 - toolDragY1;
                double th = (-System.Math.Atan2(dy, dx)) * 180 / System.Math.PI;
                if (toolSnap)
                {
                    //UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
                    th = (long)System.Math.Round(th / 15) * 15;
                }
                double thrad = th * System.Math.PI / 180;

                SupportClass.GraphicsManager.manager.SetPaint(g, new SolidBrush(Color.Red));
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float_int_int_float_float[]_float'"
                SupportClass.GraphicsManager.manager.SetPen(g, SupportClass.StrokeConsSupport.CreatePenInstance(0.5F, (int)System.Drawing.Drawing2D.LineCap.Round, (int)System.Drawing.Drawing2D.LineJoin.Round, 1F, new float[] { 2, 2 }, 0));
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)toolDragX1, (float)toolDragY1, (float)(toolDragX1 + 50), (float)toolDragY1));
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float'"
                SupportClass.GraphicsManager.manager.SetPen(g, new System.Drawing.Pen(System.Drawing.Brushes.Black, 1F));
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)toolDragX1, (float)toolDragY1, (float)(toolDragX1 + 50 * System.Math.Cos(-thrad)), (float)(toolDragY1 + 50 * System.Math.Sin(-thrad))));
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Arc2DSupport.CreateArc2D((float)(toolDragX1 - 20), (float)(toolDragY1 - 20), (float)40, (float)40, (float)0, (float)th, SupportClass.Arc2DSupport.OPEN));

                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                int ty = (int)((th > 25 || (th < 0 && th >= -25)) ? toolDragY1 - 5 : toolDragY1 + 5 + txh);
                //UPGRADE_ISSUE: Class 'java.text.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
                //UPGRADE_ISSUE: Constructor 'java.text.DecimalFormat.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
                // DecimalFormat fmt = new DecimalFormat("0");
                string fmtString = "N0";

                //UPGRADE_TODO: Method 'java.awt.Graphics2D.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DdrawString_javalangString_int_int'"
                //UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                g.DrawString((th > 0 ? "+" : "") + ((long)System.Math.Round(th)).ToString(fmtString), SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetPaint(g), (int)(toolDragX1 + 25), ty - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());

                double ax = XToAng(toolDragX1), ay = YToAng(toolDragY1);
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float'"
                SupportClass.GraphicsManager.manager.SetPen(g, new System.Drawing.Pen(System.Drawing.Brushes.Black, 1.1F));
                for (int n = 1; n <= mol.NumAtoms(); n++)
                    if (selected[n - 1])
                    {
                        double rx = mol.AtomX(n) - ax, ry = mol.AtomY(n) - ay;
                        double rth = System.Math.Atan2(ry, rx), ext = System.Math.Sqrt(rx * rx + ry * ry);
                        rx = ax + ext * System.Math.Cos(rth + thrad);
                        ry = ay + ext * System.Math.Sin(rth + thrad);
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                        g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Ellipse2DSupport.CreateEllipsePath((float)(AngToX(rx) - scale * 0.3), (float)(AngToY(ry) - scale * 0.3), (float)(scale * 0.6), (float)(scale * 0.6)));
                    }
            }

            if (tool == TOOL_TEMPLATE && trackX >= 0 && trackY >= 0)
            {
                if (highlightAtom != 0 && templateIdx > 0)
                    AdjustTemplateByAtom(highlightAtom);
                else if (highlightBond != 0 && templateIdx < 0)
                    AdjustTemplateByBond(highlightBond);
                else
                    AdjustTemplateByCoord(XToAng(trackX), YToAng(trackY));

                SupportClass.GraphicsManager.manager.SetColor(g, System.Drawing.Color.FromArgb(128, 128, 128));
                //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float'"
                SupportClass.GraphicsManager.manager.SetPen(g, new System.Drawing.Pen(System.Drawing.Brushes.Black, 1));
                for (int n = 1; templDraw != null && n <= templDraw.NumBonds(); n++)
                {
                    int from = templDraw.BondFrom(n), to = templDraw.BondTo(n);
                    //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                    g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Line2DSupport.CreateLine2DPath((float)AngToX(templDraw.AtomX(from)), (float)AngToY(templDraw.AtomY(from)), (float)AngToX(templDraw.AtomX(to)), (float)AngToY(templDraw.AtomY(to))));
                }
            }

            #endregion
            #region PART 7: draw stereochemistry labels
            if (showSter)
            {
                for (int n = 1, chi; n <= mol.NumAtoms(); n++)
                    if ((chi = mol.AtomChirality(n)) != Molecule.STEREO_NONE)
                    {
                        System.String label = chi == Molecule.STEREO_POS ? "R" : (chi == Molecule.STEREO_NEG ? "S" : "R/S");
                        SupportClass.GraphicsManager.manager.SetBrush(g, new SolidBrush(Color.Blue));
                        SupportClass.GraphicsManager.manager.SetFont(g, font);
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DdrawString_javalangString_float_float'"
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        //UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getHeight' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        g.DrawString(label, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetPaint(g), (float)(px[n - 1] - 0.5 * g.MeasureString(label, metrics).Width), (float)(py[n - 1] + (int)metrics.GetHeight()) - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
                    }
                for (int n = 1, chi; n <= mol.NumBonds(); n++)
                    if ((chi = mol.BondStereo(n)) != Molecule.STEREO_NONE)
                    {
                        System.String label = chi == Molecule.STEREO_POS ? "Z" : (chi == Molecule.STEREO_NEG ? "E" : "E/Z");
                        int i1 = mol.BondFrom(n) - 1, i2 = mol.BondTo(n) - 1;
                        SupportClass.GraphicsManager.manager.SetBrush(g, new SolidBrush(Color.Blue));
                        SupportClass.GraphicsManager.manager.SetFont(g, font);
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.drawString' was converted to 'System.Drawing.Graphics.DrawString' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DdrawString_javalangString_float_float'"
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        //UPGRADE_ISSUE: Method 'java.awt.FontMetrics.stringWidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtFontMetricsstringWidth_javalangString'"
                        //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.FontMetrics.getHeight' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        g.DrawString(label, SupportClass.GraphicsManager.manager.GetFont(g), SupportClass.GraphicsManager.manager.GetPaint(g), (float)(0.5 * (px[i1] + px[i2] - g.MeasureString(label, metrics).Width)), (float)(0.5 * (py[i1] + py[i2] + (int)metrics.GetHeight())) - SupportClass.GraphicsManager.manager.GetFont(g).GetHeight());
                    }
            }

            #endregion
            #region PART 8: corrective measures

            // see if any atoms severely overlap, and if so, draw a red circle around them
            for (int i = 1; i <= mol.NumAtoms() - 1; i++)
                for (int j = i + 1; j <= mol.NumAtoms(); j++)
                {
                    double dx = mol.AtomX(i) - mol.AtomX(j), dy = mol.AtomY(i) - mol.AtomY(j);
                    if (dx * dx + dy * dy < 0.2 * 0.2)
                    {
                        SupportClass.GraphicsManager.manager.SetColor(g, Color.Red);
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.setStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2DsetStroke_javaawtStroke'"
                        //UPGRADE_TODO: Constructor 'java.awt.BasicStroke.BasicStroke' was converted to 'System.Drawing.Pen' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtBasicStrokeBasicStroke_float'"
                        SupportClass.GraphicsManager.manager.SetPen(g, new System.Drawing.Pen(System.Drawing.Brushes.Black, 0.5F));
                        //UPGRADE_TODO: Method 'java.awt.Graphics2D.draw' was converted to 'System.Drawing.Graphics.DrawPath' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtGraphics2Ddraw_javaawtShape'"
                        g.DrawPath(SupportClass.GraphicsManager.manager.GetPen(g), SupportClass.Ellipse2DSupport.CreateEllipsePath((float)(px[i - 1] - scale * 0.25), (float)(py[i - 1] - scale * 0.25), (float)(scale * 0.5), (float)(scale * 0.5)));
                    }
                }

            #endregion
        }

        private bool AtomIsBelowBond(int n)
        {
            double x = mol.AtomX(n);
            double y = mol.AtomY(n);

            double x1 = -1;
            double x2 = -1;
            double y1 = Double.MaxValue;
            double y2 = Double.MaxValue;

            for (int i = 1; i <= mol.NumBonds(); i++)
            {
                double xTo = mol.AtomX(mol.BondTo(i));
                double yTo = mol.AtomY(mol.BondTo(i));

                double xFrom = mol.AtomX(mol.BondFrom(i));
                double yFrom = mol.AtomY(mol.BondFrom(i));

                if (xTo == x && yTo == y)
                {
                    x1 = xFrom;
                    y1 = yFrom;
                }
                else if (xFrom == x && yFrom == y)
                {
                    x2 = xTo;
                    y2 = yTo;
                }
            }

            if ((x == x1 && y1 > y) || (x == x2 && y2 > y))
                return true;

            return false;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs gr_EventArg)
        {
            // gr_EventArg.Graphics.FillRectangle(Brushes.White, 0, 0, Width, Height);

            //DrawOffScreen(gr_EventArg.Graphics); // offScreenDC
            gr_EventArg.Graphics.DrawImage(offScreenBmp, 0, 0);

            // gr_EventArg.Graphics.DrawImage(offScreenBmp, this.Left, this.Top);
        }

        protected override void OnResize(EventArgs e)
        {
            Init();

            DrawOffScreen(offScreenDC);
        }

        // Mouse events: the callbacks for Clicked, Pressed, Released, Dragged and Moved form a slightly complicated interplay of the various
        // tool events. The 'tool' variable, and its various permitted values, should make most of the behaviour reasonably self-explanatory;
        // note that many of the tools have multiple functions which may be sprinkled around the various event callbacks.

        public virtual void mouseClicked(System.Object event_sender, MouseEventArgs e)
        {
            if (tool == TOOL_CURSOR && selectListen != null)
            {

                //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
                //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
                 int i = PickAtom(e.X, e.Y);
                //UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
                if ((state4 & (int)System.Windows.Forms.Keys.Control) > 0 && i > 0 && editable)
                // select connected component
                {
                    //UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
                    if ((state4 & (int)System.Windows.Forms.Keys.Shift) == 0 && Selected != null)
                        for (int n = 0; n < mol.NumAtoms(); n++)
                            Selected[n] = false;
                    if (Selected == null)
                        Selected = new bool[mol.NumAtoms()];
                    int cc = mol.AtomConnComp(i);
                    for (int n = 1; n <= mol.NumAtoms(); n++)
                        if (mol.AtomConnComp(n) == cc)
                            Selected[n - 1] = true;
                    RePaint();
                }
                else if (i > 0)
                {
                    selectListen.MolSelected(this, i, e.Clicks > 1);
                }
                // notify of atom selection
                // notify of bond (or nothing) selection
                else
                {
                    i = PickBond(e.X, e.Y);
                    /*if (i>0)*/
                    selectListen.MolSelected(this, -i, e.Clicks > 1);
                    // (0==clicked in general area)
                }
            }
            else if (tool == TOOL_ROTATOR)
            // deselect
            {
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
            else if (tool == TOOL_ERASOR)
            // delete something, be it atom or bond
            {
                //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
                //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
                int i = PickAtom(e.X, e.Y);
                if (i > 0)
                {
                    CacheUndo();
                    mol.DeleteAtomAndBonds(i);
                }
                else
                {
                    CacheUndo();
                    //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
                    //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
                    i = PickBond(e.X, e.Y);
                    if (i > 0)
                        mol.DeleteBond(i);
                }
                if (i > 0)
                {
                    ClearTemporary();
                    DetermineSize();
                    //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                    RePaint();
                }
            }
            else if (tool == TOOL_ATOM && e.Button == MouseButtons.Left && !toolAtomDrag)
            {
                if (toolAtomEditBox != null)
                {
                    CompleteAtomEdit();
                    return;
                }

                if (toolAtomType != null)
                // add new atom, or change element label
                {
                    //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
                    //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
                    int i = PickAtom(e.X, e.Y);
                    CacheUndo();
                    if (i == 0)
                    {
                        //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
                        //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
                        i = mol.AddAtom(toolAtomType, XToAng(e.X), YToAng(e.Y));
                        //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
                        offsetX = e.X / scale - mol.AtomX(i);
                        //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
                        offsetY = e.Y / scale + mol.AtomY(i);
                    }
                    else
                        mol.SetAtomElement(i, toolAtomType);
                    ClearTemporary();
                    DetermineSize();
                    //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                    RePaint();
                }
                // setup new editing box for element
                else
                {
                    //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
                    toolAtomEditX = e.X;
                    //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
                    toolAtomEditY = e.Y;
                    toolAtomEditSel = PickAtom(toolAtomEditX, toolAtomEditY);
                    //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
                    //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
                    if (toolAtomEditSel == 0 && PickBond(e.X, e.Y) > 0)
                        return;

                    System.Windows.Forms.TextBox temp_text_box;
                    temp_text_box = new System.Windows.Forms.TextBox();
                    temp_text_box.Text = toolAtomEditSel > 0 ? mol.AtomElement(toolAtomEditSel) : "";
                    toolAtomEditBox = temp_text_box;
                    //UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
                    Controls.Add(toolAtomEditBox);
                    toolAtomEditBox.Enter += new System.EventHandler(this.focusGained);
                    toolAtomEditBox.Leave += new System.EventHandler(this.focusLost);
                    toolAtomEditBox.KeyDown += new System.Windows.Forms.KeyEventHandler(EditorPane.keyDown);
                    toolAtomEditBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyPressed);
                    toolAtomEditBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyReleased);
                    toolAtomEditBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyTyped);
                    //UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_int_int'"
                    toolAtomEditBox.Location = new System.Drawing.Point(toolAtomEditX - 10, toolAtomEditY - 10);
                    //UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetSize_int_int'"
                    toolAtomEditBox.Size = new System.Drawing.Size(20, 20);
                    toolAtomEditBox.Visible = true;
                    toolAtomEditBox.SelectionStart = 0;
                    toolAtomEditBox.SelectionLength = toolAtomEditBox.Text.Length - toolAtomEditBox.SelectionStart;
                    toolAtomEditBox.Focus();
                }
            }
            else if (tool == TOOL_TEMPLATE && e.Button == MouseButtons.Right)
            // flip the template, horizontal or vertical
            {
                bool vertical = (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift);
                for (int n = 1; n <= template.NumAtoms(); n++)
                    template.SetAtomPos(n, template.AtomX(n) * (vertical ? 1 : -1), template.AtomY(n) * (vertical ? -1 : 1));
                templDraw = template.Clone();
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }

            CheckDirtiness();
        }

        public virtual void mouseEntered(System.Object event_sender, EventArgs e)
        {
            Point mouseLocation = Control.MousePosition;

            bool redraw = false;
            //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
            //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
            if (tool == TOOL_TEMPLATE && (trackX != mouseLocation.X || trackY != mouseLocation.Y))
                redraw = true;
            //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
            trackX = mouseLocation.X;
            //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
            trackY = mouseLocation.Y;
            if (redraw)
            {
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
        }

        public virtual void mouseExited(System.Object event_sender, EventArgs e)
        {
            Point mouseLocation = Control.MousePosition;
            bool redraw = false;
            //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getX' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetX'"
            //UPGRADE_ISSUE: Method 'java.awt.event.MouseEvent.getY' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventMouseEventgetY'"
            if (tool == TOOL_TEMPLATE && (trackX != mouseLocation.X || trackY != mouseLocation.Y))
                redraw = true;
            trackX = -1; trackY = -1;
            if (redraw)
            {
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
        }

        public virtual void mousePressed(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
        {
            Focus();

            if(tool == TOOL_CURSOR && e.Button == MouseButtons.Right && CountSelected() > 0)
            {
                highlightAtom = highlightBond = 0;

                toolDragReason = DRAG_MOVE;

                toolDragX1 = toolDragX2 = e.X;
                toolDragY1 = toolDragY2 = e.Y;
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }

            if ((tool == TOOL_CURSOR || (tool == TOOL_ROTATOR && !AnySelected())) && e.Button == MouseButtons.Left && editable)
            {
                // consider initiating a drag of the select, or translate position variety
                highlightAtom = highlightBond = 0;
                //UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
                bool shift = (state4 & (int)System.Windows.Forms.Keys.Shift) > 0;
                //UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
                bool ctrl = (state4 & (int)System.Windows.Forms.Keys.Control) > 0;
                //UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
                bool alt = (state4 & (int)System.Windows.Forms.Keys.Alt) > 0;
                bool anySelected = CountSelected() > 0;
                if (tool == TOOL_ROTATOR)
                {
                    shift = false; ctrl = false; alt = false;
                } // can only select with rotator
                if (!ctrl && !alt)
                {
                    ResetSelected(!shift);
                    int atom = PickAtom(e.X, e.Y);
                    if (atom > 0)
                        Selected[atom - 1] = true;
                    else
                        toolDragReason = DRAG_SELECT;
                }
                else if (!shift && ctrl && !alt && anySelected)
                    toolDragReason = DRAG_COPY;
                else if (!shift && !ctrl && alt && anySelected)
                    toolDragReason = DRAG_MOVE;
                else if (shift && !ctrl && alt && anySelected)
                    toolDragReason = DRAG_SCALE;

                toolDragX1 = toolDragX2 = e.X;
                toolDragY1 = toolDragY2 = e.Y;
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
            else if (tool == TOOL_ERASOR && e.Button == MouseButtons.Left)
            // initiate a drag-rect-delete sequence
            {
                highlightAtom = highlightBond = 0;
                ResetSelected(true);
                toolDragReason = DRAG_SELECT;
                toolDragX1 = toolDragX2 = e.X;
                toolDragY1 = toolDragY2 = e.Y;
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
            else if (tool == TOOL_ATOM)
            // note drag or change atom
            {
                toolBondFrom = PickAtom(e.X, e.Y); // in case it gets...
                toolAtomSnap = e.Button == MouseButtons.Left; // ... dragged later
            }
            else if (tool == TOOL_BOND && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle))
            // initiate bond drag
            {
                highlightAtom = highlightBond = 0;
                toolBondDrag = false;
                toolBondFrom = PickAtom(e.X, e.Y);
                toolSnap = e.Button == MouseButtons.Left;
                if (toolBondFrom > 0)
                {
                    toolBondToX = mol.AtomX(toolBondFrom);
                    toolBondToY = mol.AtomY(toolBondFrom);
                    //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                    RePaint();
                }
                toolBondFromX = XToAng(e.X);
                toolBondFromY = YToAng(e.Y);
                toolBondHit = PickBond(e.X, e.Y);
            }
            else if (tool == TOOL_TEMPLATE && e.Button == MouseButtons.Left)
            // slap a template right down
            {
                bool swap = false;
                if (highlightAtom != 0 && templateIdx > 0)
                    AdjustTemplateByAtom(highlightAtom);
                else if (highlightBond != 0 && templateIdx < 0)
                    swap = AdjustTemplateByBond(highlightBond);
                else
                    AdjustTemplateByCoord(XToAng(trackX), YToAng(trackY));

                CacheUndo();

                if (templateIdx >= 0)
                    TemplateSetByAtom(highlightAtom);
                else
                    TemplateSetByBond(highlightBond, swap);
            }
            else if (tool == TOOL_ROTATOR && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle) && AnySelected())
            {
                // initiate a rotation-drag
                toolDragReason = DRAG_ROTATE;
                toolSnap = e.Button == MouseButtons.Left;
                if (highlightAtom > 0)
                {
                    toolDragX1 = AngToX(mol.AtomX(highlightAtom)); toolDragY1 = AngToY(mol.AtomY(highlightAtom));
                }
                else if (highlightBond > 0)
                {
                    toolDragX1 = AngToX(0.5 * (mol.AtomX(mol.BondFrom(highlightBond)) + mol.AtomX(mol.BondTo(highlightBond))));
                    toolDragY1 = AngToY(0.5 * (mol.AtomY(mol.BondFrom(highlightBond)) + mol.AtomY(mol.BondTo(highlightBond))));
                }
                else
                {
                    toolDragX1 = e.X; toolDragY1 = e.Y;
                }
                highlightAtom = highlightBond = 0;

                toolDragX2 = toolDragX1;
                toolDragY2 = toolDragY1;
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
            else if (tool == TOOL_CHARGE && highlightAtom > 0)
            // offset charge
            {
                int chg = mol.AtomCharge(highlightAtom);
                if (e.Button == MouseButtons.Left)
                    chg += toolCharge;
                else if (e.Button == MouseButtons.Middle)
                    chg -= toolCharge;
                else
                    chg = 0;
                mol.SetAtomCharge(highlightAtom, chg);
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }

            CheckDirtiness();
        }

        public virtual void mouseReleased(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((tool == TOOL_CURSOR && toolDragReason != 0) || (tool == TOOL_ROTATOR && toolDragReason == DRAG_SELECT) && editable)
            {
                // solidify a translate or select
                toolDragX2 = e.X;
                toolDragY2 = e.Y;
                double mx = toolDragX2 - toolDragX1, my = toolDragY2 - toolDragY1;

                if (toolDragReason == DRAG_SELECT && dragged != null && Selected != null)
                {
                    for (int n = 0; n < mol.NumAtoms(); n++)
                        Selected[n] = Selected[n] || dragged[n];
                }
                if (toolDragReason == DRAG_MOVE && Selected != null && mx * mx + my * my > 5 * 5)
                {
                    double dx = mx / scale, dy = (-my) / scale;
                    CacheUndo();
                    for (int n = 1; n <= mol.NumAtoms(); n++)
                        if (Selected[n - 1])
                        {
                            mol.SetAtomPos(n, mol.AtomX(n) + dx, mol.AtomY(n) + dy);
                        }
                    ClearTemporary(false);
                    DetermineSize();
                }
                if (toolDragReason == DRAG_COPY && Selected != null && mx * mx + my * my > 5 * 5)
                {
                    double dx = (toolDragX2 - toolDragX1) / scale, dy = (-(toolDragY2 - toolDragY1)) / scale;
                    int oldNumAtoms = mol.NumAtoms(), oldNumBonds = mol.NumBonds();
                    int[] newPos = new int[mol.NumAtoms()];
                    CacheUndo();
                    for (int n = 1; n <= oldNumAtoms; n++)
                        if (Selected[n - 1])
                        {
                            newPos[n - 1] = mol.AddAtom(mol.AtomElement(n), mol.AtomX(n) + dx, mol.AtomY(n) + dy, mol.AtomCharge(n), mol.AtomUnpaired(n));
                        }
                    for (int n = 1; n <= oldNumBonds; n++)
                        if (Selected[mol.BondFrom(n) - 1] && Selected[mol.BondTo(n) - 1])
                        {
                            mol.AddBond(newPos[mol.BondFrom(n) - 1], newPos[mol.BondTo(n) - 1], mol.BondOrder(n), mol.BondType(n));
                        }

                    ClearTemporary();
                    Selected = new bool[mol.NumAtoms()];
                    for (int n = 1; n <= mol.NumAtoms(); n++)
                        Selected[n - 1] = n > oldNumAtoms;
                    DetermineSize();
                }
                if (toolDragReason == DRAG_SCALE && Selected != null && mx * mx + my * my > 5 * 5)
                {
                    double extmul = DragExtendBy(mx, my);
                    double cx = 0, cy = 0;
                    int count = 0;
                    for (int n = 1; n <= mol.NumAtoms(); n++)
                        if (Selected[n - 1])
                        {
                            cx += mol.AtomX(n); cy += mol.AtomY(n); count++;
                        }
                    cx /= count; cy /= count;
                    CacheUndo();
                    for (int n = 1; n <= mol.NumAtoms(); n++)
                        if (Selected[n - 1])
                        {
                            mol.SetAtomPos(n, (mol.AtomX(n) - cx) * extmul + cx, (mol.AtomY(n) - cy) * extmul + cy);
                        }

                    ClearTemporary(false);
                    DetermineSize();
                }

                toolDragReason = 0;
                dragged = null;
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
            if (tool == TOOL_ERASOR && toolDragReason != 0)
            // erase selection
            {
                toolDragX2 = e.X;
                toolDragY2 = e.Y;
                if (toolDragReason == DRAG_SELECT && dragged != null && Selected != null)
                {
                    for (int n = 0; n < mol.NumAtoms(); n++)
                        Selected[n] = Selected[n] || dragged[n];
                    DeleteSelected();
                    ClearTemporary();
                }
                toolDragReason = 0;
                dragged = null;
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
            else if (tool == TOOL_ROTATOR && toolDragReason == DRAG_ROTATE)
            // solidify a rotation
            {
                double dx = toolDragX2 - toolDragX1, dy = toolDragY2 - toolDragY1;
                double th = (-System.Math.Atan2(dy, dx)) * 180 / System.Math.PI;
                if (toolSnap)
                {
                    //UPGRADE_TODO: Method 'java.lang.Math.round' was converted to 'System.Math.Round' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangMathround_double'"
                    th = (long)System.Math.Round(th / 15) * 15;
                }
                if (System.Math.Abs(th) > 1)
                {
                    CacheUndo();
                    th = th * System.Math.PI / 180;
                    double ax = XToAng(toolDragX1), ay = YToAng(toolDragY1);
                    for (int n = 1; n <= mol.NumAtoms(); n++)
                        if (Selected[n - 1])
                        {
                            double rx = mol.AtomX(n) - ax, ry = mol.AtomY(n) - ay;
                            double rth = System.Math.Atan2(ry, rx), ext = System.Math.Sqrt(rx * rx + ry * ry);
                            mol.SetAtomPos(n, ax + ext * System.Math.Cos(rth + th), ay + ext * System.Math.Sin(rth + th));
                        }
                    ClearTemporary(false);
                    DetermineSize();
                }

                toolDragReason = 0;
                dragged = null;
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
            else if (tool == TOOL_ATOM && toolAtomDrag && toolBondFrom > 0)
            // place a new atom-from
            {
                CacheUndo();
                mol.AddAtom(toolAtomType, toolBondToX, toolBondToY);
                mol.AddBond(toolBondFrom, mol.NumAtoms(), 1);
                ClearTemporary();
                DetermineSize();
                toolAtomDrag = false;
                toolBondFrom = 0;
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
            else if (tool == TOOL_BOND)
            // bond addition, possibly by adding new atoms, too
            {
                toolBondToX = XToAng(e.X);
                toolBondToY = YToAng(e.Y);

                int joinTo = PickAtom(e.X, e.Y);
                if (toolBondFrom > 0 && joinTo == 0 && toolSnap)
                {
                    SnapToolBond();
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    joinTo = PickAtom((int)AngToX(toolBondToX), (int)AngToY(toolBondToY));
                }

                if (e.Button == MouseButtons.Left && toolBondFrom == 0 && toolBondHit > 0)
                // change hit bond order
                {
                    int i = PickBond(e.X, e.Y);
                    if (i == toolBondHit)
                    {
                        CacheUndo();
                        if (toolBondOrder == mol.BondOrder(i) && toolBondType == mol.BondType(i))
                            mol.SetBondFromTo(i, mol.BondTo(i), mol.BondFrom(i));
                        mol.SetBondOrder(i, toolBondOrder);
                        mol.SetBondType(i, toolBondType);
                        ClearTemporary();
                    }
                }
                else if (toolBondFrom == 0)
                // create a new bond from/in the middle of nowhere, possibly connected to something
                {
                    int a1 = 0, a2 = 0;
                    double x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                    if (toolBondDrag)
                    {
                        if (toolSnap)
                            SnapToolBond();
                        x1 = toolBondFromX;
                        y1 = toolBondFromY;
                        a2 = PickAtom(e.X, e.Y);
                        if (a2 > 0)
                        {
                            x2 = mol.AtomX(a2); y2 = mol.AtomY(a2);
                        }
                        else
                        {
                            x2 = toolBondToX; y2 = toolBondToY;
                        }
                    }
                    else
                    {
                        x1 = x2 = XToAng(e.X);
                        //UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
                        if ((state4 & (int)System.Windows.Forms.Keys.Shift) > 0)
                        {
                            x1 -= 0.5 * IDEALBOND; x2 += 0.5 * IDEALBOND;
                        }
                        y1 = y2 = YToAng(e.Y);
                        //UPGRADE_NOTE: The 'java.awt.event.InputEvent.getModifiers' method simulation might not work for some controls. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1284'"
                        if ((state4 & (int)System.Windows.Forms.Keys.Shift) == 0)
                        {
                            y1 -= 0.5 * IDEALBOND; y2 += 0.5 * IDEALBOND;
                        }
                    }
                    double dx = x2 - x1, dy = y2 - y1;
                    if (dx * dx + dy * dy > 0.5 * 0.5)
                    {
                        a1 = mol.AddAtom("C", x1, y1, 0, 0);
                        if (a2 == 0)
                            a2 = mol.AddAtom("C", x2, y2, 0, 0);
                        mol.AddBond(a1, a2, toolBondOrder);
                        mol.SetBondType(mol.NumBonds(), toolBondType);
                        ClearTemporary();
                    }
                    //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                    RePaint();
                }
                else if (joinTo > 0 && joinTo != toolBondFrom)
                // link two atoms together
                {
                    CacheUndo();
                    mol.AddBond(toolBondFrom, joinTo, toolBondOrder);
                    mol.SetBondType(mol.NumBonds(), toolBondType);
                    ClearTemporary();
                }
                else if (toolBondFrom > 0)
                // draw a new bond out to some place not specified by the user, i.e. a healthy guess
                {
                    double dx = toolBondToX - mol.AtomX(toolBondFrom), dy = toolBondToY - mol.AtomY(toolBondFrom);
                    if (toolBondFrom == joinTo)
                    {
                        int[] adj = mol.AtomAdjList(toolBondFrom);
                        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
                        List<Double> poss = new List<Double>();
                        double ax = mol.AtomX(toolBondFrom), ay = mol.AtomY(toolBondFrom);
                        if (adj.Length == 0)
                            poss.Add(0.0);
                        else if (adj.Length == 1)
                        {
                            double ang = System.Math.Atan2(mol.AtomY(adj[0]) - ay, mol.AtomX(adj[0]) - ax) * 180 / System.Math.PI;
                            if (toolBondOrder != 3)
                            {
                                poss.Add(ang + 120);
                                poss.Add(ang - 120);
                            }
                            else
                                poss.Add(ang + 180);
                        }
                        else if (adj.Length == 2)
                        {
                            double ang1 = System.Math.Atan2(mol.AtomY(adj[0]) - ay, mol.AtomX(adj[0]) - ax) * 180 / System.Math.PI;
                            double ang2 = System.Math.Atan2(mol.AtomY(adj[1]) - ay, mol.AtomX(adj[1]) - ax) * 180 / System.Math.PI;
                            if (ang2 < ang1)
                                ang2 += 360;
                            if (ang2 - ang1 < 180)
                                poss.Add(0.5 * (ang1 + ang2) + 180);
                            else
                                poss.Add(0.5 * (ang1 + ang2));
                        }
                        else
                            for (int n = 0; n < adj.Length; n++)
                            {
                                double ang = System.Math.Atan2(mol.AtomY(adj[n]) - ay, mol.AtomX(adj[n]) - ax) * 180 / System.Math.PI;
                                poss.Add(ang + 180);
                            }
                        double ang3 = poss[0];
                        if (poss.Count > 1)
                        {
                            int best = -1;
                            double bestScore = 0;
                            for (int n = 0; n < poss.Count; n++)
                            {
                                double nx = ax + IDEALBOND * Math.Cos(poss[n] * System.Math.PI / 180);
                                double ny = ay + IDEALBOND * Math.Sin(poss[n] * System.Math.PI / 180);
                                double score = 0;
                                for (int i = 1; i <= mol.NumAtoms(); i++)
                                {
                                    dx = mol.AtomX(i) - nx;
                                    dy = mol.AtomY(i) - ny;
                                    score += 1 / System.Math.Min(1000, dx * dx + dy * dy);
                                }
                                if (best < 0 || score < bestScore)
                                {
                                    best = n; bestScore = score;
                                }
                            }
                            ang3 = poss[best];
                        }

                        dx = IDEALBOND * System.Math.Cos(ang3 * System.Math.PI / 180);
                        dy = IDEALBOND * System.Math.Sin(ang3 * System.Math.PI / 180);
                        toolBondToX = ax + dx;
                        toolBondToY = ay + dy;
                    }
                    if (dx * dx + dy * dy > 0.5)
                    {
                        CacheUndo();
                        mol.AddAtom("C", toolBondToX, toolBondToY);
                        mol.AddBond(toolBondFrom, mol.NumAtoms(), toolBondOrder);
                        mol.SetBondType(mol.NumBonds(), toolBondType);
                        ClearTemporary();
                        DetermineSize();
                    }
                }

                toolBondDrag = false;
                toolBondFrom = 0;
                toolBondHit = 0;
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }

            CheckDirtiness();
        }

        public virtual void mouseMoved(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
        {
            bool redraw = false;

            if (trackX != e.X || trackY != e.Y)
            {
                if (tool == TOOL_TEMPLATE)
                    redraw = true;
            }

            trackX = e.X;
            trackY = e.Y;

            if (e.Button == 0 || (e.Button == MouseButtons.Right && toolDragReason == DRAG_MOVE))
            {
                int mx = e.X, my = e.Y;
                int newAtom = 0, newBond = 0;

                newAtom = PickAtom(mx, my);
                if (newAtom == 0)
                    newBond = PickBond(mx, my);

                if (tool == TOOL_TEMPLATE && templateIdx > 0)
                    newBond = 0;
                if (tool == TOOL_TEMPLATE && templateIdx < 0)
                    newAtom = 0;

                if (newAtom != highlightAtom || newBond != highlightBond)
                {
                    highlightAtom = newAtom;
                    highlightBond = newBond;
                    redraw = true;
                }
            }

            if (redraw)
            {
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
        }

        public virtual void mouseDragged(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left || Control.MouseButtons == MouseButtons.Right)
            {
                bool redraw = false;
                if (tool == TOOL_TEMPLATE && (trackX != e.X || trackY != e.Y))
                    redraw = true;
                trackX = e.X; trackY = e.Y;

                if ((tool == TOOL_CURSOR && toolDragReason != 0) || (tool == TOOL_ERASOR && toolDragReason != 0) || (tool == TOOL_ROTATOR && toolDragReason == DRAG_SELECT))
                {
                    toolDragX2 = e.X;
                    toolDragY2 = e.Y;
                    if (toolDragReason == DRAG_SELECT)
                    {
                        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                        int x = (int)toolDragX1, y = (int)toolDragY1, w = (int)toolDragX2 - x, h = (int)toolDragY2 - y;
                        if (w < 0)
                        {
                            w = -w; x -= w;
                        }
                        if (h < 0)
                        {
                            h = -h; y -= h;
                        }
                        dragged = new bool[mol.NumAtoms()];
                        for (int n = 0; n < mol.NumAtoms(); n++)
                            dragged[n] = px[n] >= x && px[n] <= x + w && py[n] >= y && py[n] <= y + h;
                    }
                    redraw = true;
                }
                else if (tool == TOOL_ROTATOR && toolDragReason == DRAG_ROTATE)
                {
                    toolDragX2 = e.X;
                    toolDragY2 = e.Y;
                    redraw = true;
                }
                else if (tool == TOOL_ATOM && toolBondFrom != 0)
                {
                    if (!toolAtomDrag)
                    {
                        double dx = XToAng(e.X) - mol.AtomX(toolBondFrom), dy = YToAng(e.Y) - mol.AtomY(toolBondFrom);
                        if (dx * dx + dy * dy > 0.8 * 0.8)
                        {
                            toolAtomDrag = true;
                            toolBondOrder = 1;
                            toolBondType = Molecule.BONDTYPE_NORMAL;
                        }
                    }
                    if (toolAtomDrag)
                    {
                        toolBondToX = XToAng(e.X);
                        toolBondToY = YToAng(e.Y);
                        if (toolAtomSnap)
                            SnapToolBond();
                        redraw = true;
                    }
                }
                else if (tool == TOOL_BOND)
                {
                    toolBondToX = XToAng(e.X);
                    toolBondToY = YToAng(e.Y);
                    int joinTo = PickAtom(e.X, e.Y);
                    if (!toolBondDrag)
                        if (System.Math.Abs(toolBondToX - toolBondFromX) > 2 / scale || System.Math.Abs(toolBondToY - toolBondFromY) > 2 / scale)
                            toolBondDrag = true;
                    if (joinTo > 0)
                    {
                        toolBondToX = mol.AtomX(joinTo); toolBondToY = mol.AtomY(joinTo);
                    }
                    else if (toolSnap)
                        SnapToolBond();
                    redraw = true;
                }

                if (redraw)
                {
                    //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                    RePaint();
                }
                CheckDirtiness();
            }
        }

        public virtual void mouseWheelMoved(MouseEventArgs e)
        {
            if (tool == TOOL_TEMPLATE)
            {
                double cx = 0, cy = 0;
                for (int n = 1; n <= template.NumAtoms(); n++)
                {
                    cx += template.AtomX(n); cy += template.AtomY(n);
                }
                cx /= template.NumAtoms();
                cy /= template.NumAtoms();

                // the .net syntax is a little bit more cumbersome...
                bool isShiftDown = (Control.ModifierKeys & Keys.Shift) != 0;
                bool isControlDown = (Control.ModifierKeys & Keys.Control) != 0;

                double accel = isShiftDown ? 3 : 1;

                if (isControlDown)
                // scale
                {
                    double factor = 1 - 0.1 * accel * e.Delta;
                    for (int n = 1; n <= template.NumAtoms(); n++)
                        template.SetAtomPos(n, cx + (template.AtomX(n) - cx) * factor, cy + (template.AtomY(n) - cy) * factor);
                }
                // rotate
                else
                {
                    double radians = 5 * accel * System.Math.PI / 180 * e.Delta;
                    for (int n = 1; n <= template.NumAtoms(); n++)
                    {
                        double dx = template.AtomX(n) - cx, dy = template.AtomY(n) - cy;
                        double dist = System.Math.Sqrt(dx * dx + dy * dy), theta = System.Math.Atan2(dy, dx);
                        template.SetAtomPos(n, cx + dist * System.Math.Cos(theta + radians), cy + dist * System.Math.Sin(theta + radians));
                    }
                }
                templDraw = template.Clone();
                //UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
                RePaint();
            }
        }

        // Other callbacks...

        public virtual void focusGained(System.Object event_sender, System.EventArgs e)
        {
        }
        public virtual void focusLost(System.Object event_sender, System.EventArgs e)
        {
            if (event_sender == toolAtomEditBox)
                CompleteAtomEdit();
        }
        public virtual void keyPressed(System.Object event_sender, System.Windows.Forms.KeyEventArgs e)
        {
        }
        public virtual void keyReleased(System.Object event_sender, System.Windows.Forms.KeyEventArgs e)
        {
        }
        public virtual void keyTyped(System.Object event_sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (event_sender == toolAtomEditBox)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.awt.event.KeyEvent.getKeyChar' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                if (e.KeyChar == '\n')
                    CompleteAtomEdit();
            }
        }

        internal void WriteCanvasToFile(string filename)
        {            
            if (!filename.Contains("."))
                filename += ".bmp";

            RePaint();
            offScreenBmp.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
        }
    }
}
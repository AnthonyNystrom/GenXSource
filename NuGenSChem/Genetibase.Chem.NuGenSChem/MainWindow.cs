using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Reflection;
using Genetibase.UI;
using System.Drawing.Printing;

namespace Genetibase.Chem.NuGenSChem
{
    public partial class MainWindow : UserControl, ITemplSelectListener, MolSelectListener
    {
        private bool _isStreamMode;
        public bool IsStreamMode
        {
            get { return _isStreamMode; }
        }

        public Templates GetTemplates()
        {
            return templ;
        }

        public ITemplSelectListener GetTemplListener()
        {
            return this;
        }

        public PrintDocument Document()
        {
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            return doc;
        }


        private void printDoc_PrintPage(Object sender, PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(editor.Width, editor.Height);
            editor.DrawToBitmap(bmp, editor.Bounds);
            e.Graphics.DrawImage(bmp, new Point(0, 0));
        }

	

        public bool IsDirty()
        {
            return editor.IsDirty(); 
        }

        public void Clear()
        {
            editor.Clear(); 

            filename = null;
            editor.NotifySaved();
        }

        	static private System.Int32 state5;
        //private class AnonymousClassRunnable : IThreadRunnable
        //{
        //    public virtual void  Run()
        //    {
        //        createAndShowGUI();
        //    }
        //}
		private static void  keyDown(System.Object event_sender, System.Windows.Forms.KeyEventArgs e)
		{
			state5 = ((int) System.Windows.Forms.Control.MouseButtons  | (int) System.Windows.Forms.Control.ModifierKeys);
		}
		private static void  mouseDown(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
		{
			state5 = ((int) e.Button | (int) System.Windows.Forms.Control.ModifierKeys);
		}
		public const System.String LICENSE = "This program is free software; you can redistribute it and/or modify\n" + "it under the terms of the GNU General Public License as published by\n" + "the Free Software Foundation; either version 2 of the License, or\n" + "(at your option) any later version.\n\n" + "This program is distributed in the hope that it will be useful,\n" + "but WITHOUT ANY WARRANTY; without even the implied warranty of\n" + "MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the\n" + "GNU General Public License for more details.\n\n" + "You should have received a copy of the GNU General Public License\n" + "along with this program; if not, write to the Free Software\n" + "Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA\n\n" + "or see http://www.gnu.org for details.";
		
		public const System.String VERSION = "1.07";
		
		//UPGRADE_TODO: Class 'javax.swing.Image' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
		internal System.Drawing.Image mainIcon = null, mainLogo = null;
		
		internal const int TOOL_CURSOR = 0;
		internal const int TOOL_ROTATOR = 1;
		internal const int TOOL_ERASOR = 2;
		internal const int TOOL_DIALOG = 3;
		internal const int TOOL_EDIT = 4;
		internal const int TOOL_SETATOM = 5;
		internal const int TOOL_SINGLE = 6;
		internal const int TOOL_DOUBLE = 7;
		internal const int TOOL_TRIPLE = 8;
		internal const int TOOL_ZERO = 9;
		internal const int TOOL_INCLINED = 10;
		internal const int TOOL_DECLINED = 11;
		internal const int TOOL_UNKNOWN = 12;
		internal const int TOOL_CHARGE = 13;
		internal const int TOOL_UNDO = 14;
		internal const int TOOL_REDO = 15;
		internal const int TOOL_TEMPLATE = 16;
		internal const int TOOL_CUT = 17;
		internal const int TOOL_COPY = 18;
		internal const int TOOL_PASTE = 19;
		internal const int TOOL_COUNT = 20;
		
		internal static readonly System.String[] IMAGE_TOOL = new System.String[]{"Cursor", "Rotator", "Erasor", "EDialog", "AEdit", "ASelect", "BSingle", "BDouble", "BTriple", "BZero", "BInclined", "BDeclined", "BUnknown", "ACharge", "Undo", "Redo", "Template", "ECut", "ECopy", "EPaste"};
		internal static readonly bool[] ACTIVE_TOOL = new bool[]{true, true, true, false, true, true, true, true, true, true, true, true, true, true, false, false, true, false, false, false};
		internal static readonly System.String[] TOOL_TIPS = new System.String[]{"Cursor: Select or translate atoms\n" + "   Click/Drag = select only\n" + "   Shift+Click/Drag = select additional\n" + "   Ctrl+Click = select component\n" + "   Ctrl+Shift+Click = select additional component\n" + "   Ctrl+Drag = copy selected atoms\n" + "   Alt+Drag = move selected atoms\n" + "   Alt+Shift+Drag = scale selected atoms", "Rotator: Rotate selected atoms about centre\n" + "   Left Drag = rotate in 15\u00B0 increments\n" + "   Right Drag = rotate freely", "Erasor: Delete atoms or bonds\n" + "   Left Click = delete underlying atom or bond\n" + "   Left Drag = delete atoms underneath marquis", "(edit dialog)", "Edit Element: Edit element in place\n" + "   Left Click = type in new element label", "Place Element: Replace or create preselected element\n" + "   Left Click = replace or create atom\n" + "   Right Click = select from short list of elements", "Single Bond: Create or impose a single bond\n" + "   Left Drag = create bond to new atom at 30\u00B0\n" + "               increments, or connect existing atoms\n" + "   Right Drag = create bond freely\n" + "   Left Click = create new bond or set bond to single", "Double Bond: Create or impose a double bond\n" + "   Left Drag = create bond to new atom at 30\u00B0\n" + "               increments, or connect existing atoms\n" + "   Right Drag = create bond freely\n" + "   Left Click = create new bond or set bond to double", "Triple Bond: Create or impose a triple bond\n" + "   Left Drag = create bond to new atom at 30\u00B0\n" + "               increments, or connect existing atoms\n" + "   Right Drag = create bond freely\n" + "   Left Click = create new bond or set bond to triple", "Zero Bond: Create or impose a zero-order bond\n" + "   Left Drag = create bond to new atom at 30\u00B0\n" + "               increments, or connect existing atoms\n" + "   Right Drag = create bond freely\n" + "   Left Click = create new bond or set bond to zero", 
			"Inclined Bond: Create or impose an inclined single bond\n" + "   Left Drag = create bond to new atom at 30\u00B0\n" + "               increments, or connect existing atoms\n" + "   Right Drag = create bond freely\n" + "   Left Click = create new bond or set bond to inclined", "Declined Bond: Create or impose a declined single bond\n" + "   Left Drag = create bond to new atom at 30\u00B0\n" + "               increments, or connect existing atoms\n" + "   Right Drag = create bond freely\n" + "   Left Click = create new bond or set bond to declined", "Squiggly Bond: Create or impose a squiggly single bond\n" + "   Left Drag = create bond to new atom at 30\u00B0\n" + "               increments, or connect existing atoms\n" + "   Right Drag = create bond freely\n" + "   Left Click = create new bond or set bond to squiggly", "Charge: Alter charge on an atom\n" + "   Left Click = increase charge on underlying atom\n" + "   Right Click = decrease charge on underlying atom\n" + "   Middle Click = remove charge on underlying atom", "(undo)", "(redo)", "Template: Select or place a template structure\n" + "   Left Click = use most recent template as a tool\n" + "   Right Click = open template store for selection\n" + "   Middle Click = flip template horizontally\n" + "                       (+Shift to flip vertically)\n" + "   Mouse Wheel = rotate template (+Shift for faster)\n" + "   Ctrl+Mouse Wheel = scale template (+Shift for faster)\n", "(cut)", "(copy)", "(paste)"};
		
		internal System.Windows.Forms.Button[] toolButtons;
		internal System.Collections.IList toolGroup;
		internal Image[] toolIcons;
		internal EditorPane editor;
		internal Templates templ;
		internal System.Windows.Forms.MenuItem chkShowHydr, chkShowSter;
		
		internal System.String filename = null;
		internal System.String lastElement = null, typedElement = "";
		internal Molecule lastTemplate = null;
		internal int templateIdx = - 1;
		internal bool streamMode = false;
		
        public MainWindow()
        {
            templ = new Templates();
            this.menu = new NuGenMainPopupMenu(this, null);
            editor = new EditorPane(this.Width, this.Height, true);
            InitializeComponent();
            SetupButtonList();
        }


	

        public void Init(System.String LoadFN, bool StreamMode)
        {
			streamMode = StreamMode;
			
			// application
			
//			Closing += new System.ComponentModel.CancelEventHandler(this.MainWindow_Closing_DISPOSE_ON_CLOSE);
			// JFrame.setDefaultLookAndFeelDecorated(false);
			
			//UPGRADE_ISSUE: Constructor 'javax.swing.Image.Image' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingImageIconImageIcon_javanetURL'"
			//GetType();
			//UPGRADE_TODO: Method 'java.lang.Class.getResource' was converted to 'System.Uri' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassgetResource_javalangString'"
            
            mainIcon = new Bitmap(Utility.GetFullPath("images\\MainIcon.png"));
			//Icon = System.Drawing.Icon.FromHandle(((System.Drawing.Bitmap) mainIcon).GetHicon());
			//UPGRADE_ISSUE: Constructor 'javax.swing.Image.Image' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingImageIconImageIcon_javanetURL'"
			GetType();
			//UPGRADE_TODO: Method 'java.lang.Class.getResource' was converted to 'System.Uri' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassgetResource_javalangString'"
            mainLogo = new Bitmap(Utility.GetFullPath("/images/MainLogo.png"));


            templ = new Templates();
			
			// toolbar

			ImageList temp_ImageList;
			temp_ImageList = new System.Windows.Forms.ImageList();
			// temp_ToolBar = new System.Windows.Forms.ToolBar();
            toolButtons = new Button[TOOL_COUNT];
			//UPGRADE_TODO: Class 'javax.swing.Image' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
			toolIcons = new System.Drawing.Image[TOOL_COUNT];
			//UPGRADE_TODO: Class 'javax.swing.ButtonGroup' was converted to 'System.Collections.IList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			toolGroup = new ArrayList();
			for (int n = 0; n < TOOL_COUNT; n++)
			{
				//UPGRADE_TODO: Method 'java.lang.Class.getResource' was converted to 'System.Uri' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassgetResource_javalangString'"
				toolIcons[n] = new Bitmap(Utility.GetFullPath("/images/" + IMAGE_TOOL[n] + ".png"));
				if (ACTIVE_TOOL[n])
				{
                    toolButtons[n] = new Button();
                    toolButtons[n].Image = toolIcons[n]; 
					toolGroup.Add((System.Object) toolButtons[n]);
					System.Windows.Forms.ToolBarButton temp_ToolBarButton;
					//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
					// tools.Items.Add(toolButtons[n]);
                    SupportClass.CommandManager.CheckCommand(toolButtons[n]);
                    SupportClass.ToolTipSupport.setToolTipText(toolButtons[n], TOOL_TIPS[n]);
				}
				/*else {toolButtons[n]=new JButton(toolIcons[n]);
				tools.Add(toolButtons[n]);
				toolButtons[n].addActionListener(this);}*/
			}
			//UPGRADE_TODO: Method 'javax.swing.AbstractButton.getModel' was converted to 'ToolStripButtonBase' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			SupportClass.ButtonGroupSupport.SetSelected(toolGroup, toolButtons[TOOL_CURSOR], true);
			
			toolButtons[TOOL_SETATOM].MouseDown += new System.Windows.Forms.MouseEventHandler(mouseDown);
			toolButtons[TOOL_SETATOM].MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousePressed);
			toolButtons[TOOL_SETATOM].KeyDown += new System.Windows.Forms.KeyEventHandler(keyDown);
			toolButtons[TOOL_SETATOM].KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyPressed);
			toolButtons[TOOL_SETATOM].KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyTyped);
			toolButtons[TOOL_TEMPLATE].MouseDown += new System.Windows.Forms.MouseEventHandler(mouseDown);
			toolButtons[TOOL_TEMPLATE].MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousePressed);
			
			SelectElement("C");
			
			// menus
			
			System.Windows.Forms.MainMenu menubar = new System.Windows.Forms.MainMenu();
			
			System.Windows.Forms.MenuItem menufile = new System.Windows.Forms.MenuItem("&File");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// menufile.setMnemonic((int) System.Windows.Forms.Keys.F);
			menufile.MenuItems.Add(MenuItem("New", (int) System.Windows.Forms.Keys.N, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('N' | (int) System.Windows.Forms.Keys.Control))));
			menufile.MenuItems.Add(MenuItem("New Window", (int) System.Windows.Forms.Keys.W, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('N' | (int) System.Windows.Forms.Keys.Control + (int) System.Windows.Forms.Keys.Shift))));
			menufile.MenuItems.Add(MenuItem("Open", (int) System.Windows.Forms.Keys.O, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('O' | (int) System.Windows.Forms.Keys.Control))));
			if (!streamMode)
				menufile.MenuItems.Add(MenuItem("Save", (int) System.Windows.Forms.Keys.S, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('S' | (int) System.Windows.Forms.Keys.Control))));
			menufile.MenuItems.Add(MenuItem("Save As", (int) System.Windows.Forms.Keys.A));
			System.Windows.Forms.MenuItem menuexport = new System.Windows.Forms.MenuItem("&Export");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// menuexport.setMnemonic((int) System.Windows.Forms.Keys.X);
			menuexport.MenuItems.Add(MenuItem("as MDL MOL", (int) System.Windows.Forms.Keys.M, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('M' | (int) System.Windows.Forms.Keys.Control + (int) System.Windows.Forms.Keys.Shift))));
			menuexport.MenuItems.Add(MenuItem("as CML XML", (int) System.Windows.Forms.Keys.X, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('X' | (int) System.Windows.Forms.Keys.Control + (int) System.Windows.Forms.Keys.Shift))));
			menufile.MenuItems.Add(menuexport);
			menufile.MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
			if (!streamMode)
				menufile.MenuItems.Add(MenuItem("Quit", (int) System.Windows.Forms.Keys.Q, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('Q' | (int) System.Windows.Forms.Keys.Control))));
			else
				menufile.MenuItems.Add(MenuItem("Save & Quit", (int) System.Windows.Forms.Keys.Q, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('Q' | (int) System.Windows.Forms.Keys.Control))));
			
			System.Windows.Forms.MenuItem menuedit = new System.Windows.Forms.MenuItem("&Edit");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// menuedit.setMnemonic((int) System.Windows.Forms.Keys.E);
			menuedit.MenuItems.Add(MenuItem("Edit...", (int) System.Windows.Forms.Keys.E, toolIcons[TOOL_DIALOG], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) (' ' | (int) System.Windows.Forms.Keys.Control))));
			menuedit.MenuItems.Add(MenuItem("Undo", (int) System.Windows.Forms.Keys.U, toolIcons[TOOL_UNDO], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('Z' | (int) System.Windows.Forms.Keys.Control))));
			menuedit.MenuItems.Add(MenuItem("Redo", (int) System.Windows.Forms.Keys.R, toolIcons[TOOL_REDO], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('Z' | (int) System.Windows.Forms.Keys.Control + (int) System.Windows.Forms.Keys.Shift))));
			menuedit.MenuItems.Add(MenuItem("Cut", (int) System.Windows.Forms.Keys.X, toolIcons[TOOL_CUT], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('X' | (int) System.Windows.Forms.Keys.Control))));
			menuedit.MenuItems.Add(MenuItem("Copy", (int) System.Windows.Forms.Keys.C, toolIcons[TOOL_COPY], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('C' | (int) System.Windows.Forms.Keys.Control))));
			menuedit.MenuItems.Add(MenuItem("Paste", (int) System.Windows.Forms.Keys.V, toolIcons[TOOL_PASTE], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('V' | (int) System.Windows.Forms.Keys.Control))));
			menuedit.MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
			menuedit.MenuItems.Add(MenuItem("Select All", (int) System.Windows.Forms.Keys.S, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('A' | (int) System.Windows.Forms.Keys.Control))));
			menuedit.MenuItems.Add(MenuItem("Next Atom", (int) System.Windows.Forms.Keys.N, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('E' | (int) System.Windows.Forms.Keys.Control))));
			menuedit.MenuItems.Add(MenuItem("Previous Atom", (int) System.Windows.Forms.Keys.P, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('E' | (int) System.Windows.Forms.Keys.Control + (int) System.Windows.Forms.Keys.Shift))));
			menuedit.MenuItems.Add(MenuItem("Next Group", (int) System.Windows.Forms.Keys.G, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('G' | (int) System.Windows.Forms.Keys.Control))));
			menuedit.MenuItems.Add(MenuItem("Previous Group", (int) System.Windows.Forms.Keys.R, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('G' | (int) System.Windows.Forms.Keys.Control + (int) System.Windows.Forms.Keys.Shift))));
			menuedit.MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
			menuedit.MenuItems.Add(MenuItem("Flip Horizontal", (int) System.Windows.Forms.Keys.H, null, null));
			menuedit.MenuItems.Add(MenuItem("Flip Vertical", (int) System.Windows.Forms.Keys.V, null, null));
			menuedit.MenuItems.Add(MenuItem("Rotate +45°", (int) System.Windows.Forms.Keys.D4, null, null));
			menuedit.MenuItems.Add(MenuItem("Rotate -45°", (int) System.Windows.Forms.Keys.D5, null, null));
			menuedit.MenuItems.Add(MenuItem("Rotate +90°", (int) System.Windows.Forms.Keys.D9, null, null));
			menuedit.MenuItems.Add(MenuItem("Rotate -90°", (int) System.Windows.Forms.Keys.D0, null, null));
			menuedit.MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
			menuedit.MenuItems.Add(MenuItem("Add Temporary Template", (int) System.Windows.Forms.Keys.T, null, null));
			menuedit.MenuItems.Add(MenuItem("Normalise Bond Lengths", (int) System.Windows.Forms.Keys.N, null, null));
			
			System.Windows.Forms.MenuItem menuview = new System.Windows.Forms.MenuItem("&View");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// menuview.setMnemonic((int) System.Windows.Forms.Keys.V);
			menuview.MenuItems.Add(MenuItem("Zoom Full", (int) System.Windows.Forms.Keys.F, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('0' | (int) System.Windows.Forms.Keys.Control))));
			menuview.MenuItems.Add(MenuItem("Zoom In", (int) System.Windows.Forms.Keys.I, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('=' | (int) System.Windows.Forms.Keys.Control))));
			menuview.MenuItems.Add(MenuItem("Zoom Out", (int) System.Windows.Forms.Keys.O, null, new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('-' | (int) System.Windows.Forms.Keys.Control))));
			menuview.MenuItems.Add(new System.Windows.Forms.MenuItem("-"));
			//UPGRADE_TODO: Class 'javax.swing.ButtonGroup' was converted to 'System.Collections.IList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			System.Collections.IList showBG = new ArrayList();
			menuview.MenuItems.Add(RadioMenuItem("Show Elements", (int) System.Windows.Forms.Keys.E, true, showBG));
			menuview.MenuItems.Add(RadioMenuItem("Show All Elements", (int) System.Windows.Forms.Keys.A, false, showBG));
			menuview.MenuItems.Add(RadioMenuItem("Show Indices", (int) System.Windows.Forms.Keys.I, false, showBG));
			menuview.MenuItems.Add(RadioMenuItem("Show Ring ID", (int) System.Windows.Forms.Keys.R, false, showBG));
			menuview.MenuItems.Add(RadioMenuItem("Show CIP Priority", (int) System.Windows.Forms.Keys.C, false, showBG));
			
			System.Windows.Forms.MenuItem menutool = new System.Windows.Forms.MenuItem("&Tool");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// menutool.setMnemonic((int) System.Windows.Forms.Keys.T);
			menutool.MenuItems.Add(MenuItem("Cursor", (int) System.Windows.Forms.Keys.C, toolIcons[TOOL_CURSOR], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ((int) System.Windows.Forms.Keys.Escape | 0))));
			menutool.MenuItems.Add(MenuItem("Rotator", (int) System.Windows.Forms.Keys.R, toolIcons[TOOL_ROTATOR], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('R' | (int) System.Windows.Forms.Keys.Control))));
			menutool.MenuItems.Add(MenuItem("Erasor", (int) System.Windows.Forms.Keys.E, toolIcons[TOOL_ERASOR], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('D' | (int) System.Windows.Forms.Keys.Control))));
			menutool.MenuItems.Add(MenuItem("Edit Atom", (int) System.Windows.Forms.Keys.A, toolIcons[TOOL_EDIT], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) (',' | (int) System.Windows.Forms.Keys.Control))));
			//UPGRADE_ISSUE: Constructor 'javax.swing.Image.Image' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingImageIconImageIcon_javanetURL'"
			GetType();
			//UPGRADE_TODO: Method 'java.lang.Class.getResource' was converted to 'System.Uri' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassgetResource_javalangString'"
			menutool.MenuItems.Add(MenuItem("Set Atom", (int) System.Windows.Forms.Keys.S, new Bitmap(Utility.GetFullPath("/images/ASelMenu.png")), new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('.' | (int) System.Windows.Forms.Keys.Control))));
			menutool.MenuItems.Add(MenuItem("Single Bond", (int) System.Windows.Forms.Keys.D1, toolIcons[TOOL_SINGLE], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('1' | (int) System.Windows.Forms.Keys.Control))));
			menutool.MenuItems.Add(MenuItem("Double Bond", (int) System.Windows.Forms.Keys.D2, toolIcons[TOOL_DOUBLE], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('2' | (int) System.Windows.Forms.Keys.Control))));
			menutool.MenuItems.Add(MenuItem("Triple Bond", (int) System.Windows.Forms.Keys.D3, toolIcons[TOOL_TRIPLE], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('3' | (int) System.Windows.Forms.Keys.Control))));
			menutool.MenuItems.Add(MenuItem("Zero Bond", (int) System.Windows.Forms.Keys.D0, toolIcons[TOOL_ZERO], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('0' | (int) System.Windows.Forms.Keys.Control))));
			menutool.MenuItems.Add(MenuItem("Inclined Bond", (int) System.Windows.Forms.Keys.I, toolIcons[TOOL_INCLINED]));
			menutool.MenuItems.Add(MenuItem("Declined Bond", (int) System.Windows.Forms.Keys.D, toolIcons[TOOL_DECLINED]));
			menutool.MenuItems.Add(MenuItem("Unknown Bond", (int) System.Windows.Forms.Keys.U, toolIcons[TOOL_UNKNOWN]));
			menutool.MenuItems.Add(MenuItem("Charge", (int) System.Windows.Forms.Keys.H, toolIcons[TOOL_CHARGE], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('H' | (int) System.Windows.Forms.Keys.Control))));
			menutool.MenuItems.Add(MenuItem("Template Tool", (int) System.Windows.Forms.Keys.T, toolIcons[TOOL_TEMPLATE], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('T' | (int) System.Windows.Forms.Keys.Control))));
			menutool.MenuItems.Add(MenuItem("Select Template", (int) System.Windows.Forms.Keys.T, toolIcons[TOOL_TEMPLATE], new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys) ('T' | (int) System.Windows.Forms.Keys.Control + (int) System.Windows.Forms.Keys.Shift))));
			System.Windows.Forms.MenuItem menuhydr = new System.Windows.Forms.MenuItem("H&ydrogen");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// menuhydr.setMnemonic((int) System.Windows.Forms.Keys.Y);
			chkShowHydr = new System.Windows.Forms.MenuItem("Show H&ydrogen");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// chkShowHydr.setMnemonic((int) System.Windows.Forms.Keys.Y);
			chkShowHydr.Checked = true;
			chkShowHydr.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(chkShowHydr);
			menuhydr.MenuItems.Add(chkShowHydr);
			menuhydr.MenuItems.Add(MenuItem("Set Explicit", (int) System.Windows.Forms.Keys.E));
			menuhydr.MenuItems.Add(MenuItem("Clear Explicit", (int) System.Windows.Forms.Keys.X));
			menuhydr.MenuItems.Add(MenuItem("Zero Explicit", (int) System.Windows.Forms.Keys.Z));
			menuhydr.MenuItems.Add(MenuItem("Create Actual", (int) System.Windows.Forms.Keys.C));
			menuhydr.MenuItems.Add(MenuItem("Delete Actual", (int) System.Windows.Forms.Keys.D));
			
			System.Windows.Forms.MenuItem menuster = new System.Windows.Forms.MenuItem("&Stereochemistry");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			//menuster.setMnemonic((int) System.Windows.Forms.Keys.S);
			chkShowSter = new System.Windows.Forms.MenuItem("Show Stereo&labels");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// chkShowSter.setMnemonic((int) System.Windows.Forms.Keys.L);
			chkShowSter.Checked = false;
			chkShowSter.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(chkShowSter);
			menuster.MenuItems.Add(chkShowSter);
			menuster.MenuItems.Add(MenuItem("Invert Stereochemistry", (int) System.Windows.Forms.Keys.I));
			menuster.MenuItems.Add(MenuItem("Set R/Z", (int) System.Windows.Forms.Keys.R));
			menuster.MenuItems.Add(MenuItem("Set S/E", (int) System.Windows.Forms.Keys.S));
			menuster.MenuItems.Add(MenuItem("Cycle Wedges", (int) System.Windows.Forms.Keys.C));
			menuster.MenuItems.Add(MenuItem("Remove Wedges", (int) System.Windows.Forms.Keys.W));
			
			System.Windows.Forms.MenuItem menuhelp = new System.Windows.Forms.MenuItem("&Help");
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// menuhelp.setMnemonic((int) System.Windows.Forms.Keys.H);
			menuhelp.MenuItems.Add(MenuItem("About", (int) System.Windows.Forms.Keys.A));
			
			menubar.MenuItems.Add(menufile);
			menubar.MenuItems.Add(menuedit);
			menubar.MenuItems.Add(menuview);
			menubar.MenuItems.Add(menutool);
			menubar.MenuItems.Add(menuhydr);
			menubar.MenuItems.Add(menuster);
			//UPGRADE_ISSUE: Method 'javax.swing.Box.createHorizontalGlue' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBox'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
            // TODO: Figure out what horizontal glue is and find equiv
            //System.Windows.Forms.Control temp_Control;
            //temp_Control = Box.createHorizontalGlue();
            //menubar.Controls.Add(temp_Control);
			menubar.MenuItems.Add(menuhelp);
			
			// molecule
			
			editor = new EditorPane(this.Width, this.Height, false);
            editor.SetMolSelectListener((MolSelectListener)this); 
            
			//UPGRADE_TODO: Class 'javax.swing.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			//UPGRADE_TODO: Constructor 'javax.swing.JScrollPane.JScrollPane' was converted to 'System.Windows.Forms.ScrollableControl.ScrollableControl' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollPaneJScrollPane_javaawtComponent'"
			System.Windows.Forms.ScrollableControl temp_scrollablecontrol;
			temp_scrollablecontrol = new System.Windows.Forms.ScrollableControl();
			temp_scrollablecontrol.AutoScroll = true;
			temp_scrollablecontrol.Controls.Add(editor);
			System.Windows.Forms.ScrollableControl scroll = temp_scrollablecontrol;
			
			// overall layout
			
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'java.awt.BorderLayout.BorderLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtBorderLayout'"
			/*
			((System.Windows.Forms.ContainerControl) this).setLayout(new BorderLayout());*/
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			((System.Windows.Forms.ContainerControl) this).Controls.Add(scroll);
			scroll.Dock = System.Windows.Forms.DockStyle.Fill;
			scroll.BringToFront();
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			// ((System.Windows.Forms.ContainerControl) this).Controls.Add(menubar);
			// menubar.Dock = System.Windows.Forms.DockStyle.Top;
			// menubar.SendToBack();
			//UPGRADE_TODO: Method 'javax.swing.JFrame.getContentPane' was converted to 'System.Windows.Forms.Form' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJFramegetContentPane'"
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent_javalangObject'"
			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
// 			pack();
			
			editor.Focus();
			
			editor.SetToolCursor();
			
			if (LoadFN != null)
			{
				try
				{
					//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
					System.IO.FileStream istr = new System.IO.FileStream(LoadFN, System.IO.FileMode.Open, System.IO.FileAccess.Read);
					Molecule frag = MoleculeStream.ReadUnknown(istr);
					editor.AddArbitraryFragment(frag);
					istr.Close();
				}
				catch (System.IO.IOException e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Open Failed", (int) System.Windows.Forms.MessageBoxIcon.Error);
					return ;
				}
				
				SetFilename(LoadFN);
				editor.NotifySaved();
			}
			if (streamMode)
				ReadStream();
			
			KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyPressed);
			KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyTyped);
			editor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyPressed);
			editor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyTyped);
		}

		
		public virtual void  SetMolecule(Molecule Mol)
		{
			editor.Replace(Mol);
			editor.ScaleToFit();
			editor.NotifySaved();
		}
		
		internal virtual System.Windows.Forms.MenuItem MenuItem(System.String txt, int key)
		{
			return MenuItem(txt, key, null, null);
		}
		internal virtual System.Windows.Forms.MenuItem MenuItem(System.String txt, int key, System.Drawing.Image icon)
		{
			return MenuItem(txt, key, icon, null);
		}
		internal virtual System.Windows.Forms.MenuItem MenuItem(System.String txt, int key, System.Drawing.Image icon, System.Windows.Forms.KeyEventArgs accel)
		{
			System.Windows.Forms.MenuItem mi = new System.Windows.Forms.MenuItem(txt.Replace("" + (char) key, "&" + (char) key));
			mi.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(mi);
			if (icon != null)
			{
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setIcon' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetIcon_javaxswingIcon'"
				// mi.setIcon(icon);
			}
            // Handled by the designer code now. 
            //if (accel != null)
            //{
            //    //UPGRADE_WARNING: Method 'javax.swing.JMenuItem.setAccelerator' was converted to 'System.Windows.Forms.MenuItem.Shortcut' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
            //    // mi.Shortcut = (System.Windows.Forms.Shortcut.) accel.Key.KeyData;
            //}
			return mi;
		}
		//UPGRADE_TODO: Class 'javax.swing.ButtonGroup' was converted to 'System.Collections.IList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		internal virtual System.Windows.Forms.MenuItem RadioMenuItem(System.String txt, int key, bool sel, System.Collections.IList bg)
		{
			System.Windows.Forms.MenuItem temp_menuitem;
			//UPGRADE_TODO: Constructor 'javax.swing.JRadioButtonMenuItem.JRadioButtonMenuItem' was converted to 'System.Windows.Forms.MenuItem' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJRadioButtonMenuItemJRadioButtonMenuItem_javalangString_boolean'"
			temp_menuitem = new System.Windows.Forms.MenuItem();
			temp_menuitem.RadioCheck = true;
			temp_menuitem.Text = txt;
			temp_menuitem.Checked = sel;
			System.Windows.Forms.MenuItem mi = temp_menuitem;
			mi.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(mi);
			//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setMnemonic' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetMnemonic_int'"
			// mi.setMnemonic(key);
			bg.Add((System.Object) mi);
			return mi;
		}
		
		internal virtual void  SetFilename(System.String fn)
		{
			if (fn.Length == 0)
			{
				filename = null; return ;
			}
			filename = fn;
			
			if (!streamMode)
			{
				System.String chopfn = fn;
				int i = chopfn.LastIndexOf("/");
				if (i >= 0)
					chopfn = chopfn.Substring(i + 1);
				Text = chopfn + " - NuGenChem";
			}
			else
				Text = "NuGenChem";
		}
		
		internal virtual void  SaveCurrent()
		{
			try
			{
				//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
				System.IO.FileStream ostr = new System.IO.FileStream(filename, System.IO.FileMode.Create);
				MoleculeStream.WriteNative(ostr, editor.MolData());
				ostr.Close();
				editor.NotifySaved();
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Save Failed", (int) System.Windows.Forms.MessageBoxIcon.Error);
			}
		}
		
		internal virtual void  ReadStream()
		{
			try
			{
				Molecule frag = MoleculeStream.ReadUnknown(System.Console.OpenStandardInput());
				editor.AddArbitraryFragment(frag);
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "<stdin> Read Failed", (int) System.Windows.Forms.MessageBoxIcon.Error);
				return ;
			}
		}
		public virtual void  WriteStream()
		{
			Molecule mol = editor.MolData();
			try
			{
				MoleculeStream.WriteMDLMOL(System.Console.OpenStandardOutput(), mol);
				MoleculeStream.WriteNative(System.Console.OpenStandardOutput(), mol);
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "<stdout> Write Failed", (int) System.Windows.Forms.MessageBoxIcon.Error);
			}
		}
		
		internal virtual void  TestMol()
		{
			Molecule mol = new Molecule();
			
			mol.AddAtom("N", 0, 0);
			mol.AddAtom("C", 1.2, 0);
			mol.AddAtom("O", 2, 0.8);
			mol.AddAtom("H", 3, - 0.8);
			mol.AddAtom("H", 4, 0);
			mol.AddBond(1, 2, 1);
			mol.AddBond(2, 3, 2);
			mol.AddBond(3, 4, 1);
			mol.AddBond(4, 5, 0);
			
			editor.Replace(mol);
		}
		
		internal virtual void  EditCut()
		{
			Molecule frag = editor.SelectedSubgraph();
			try
			{
                MemoryStream stream = new MemoryStream();
                MoleculeStream.WriteMDLMOL(stream, frag);
                MoleculeStream.WriteNative(stream, frag);

                stream.Position = 0;

                using (StreamReader rdr = new StreamReader(stream))
                {
                    Clipboard.SetDataObject(rdr.ReadToEnd());
                    editor.DeleteSelected(); // (keep this within the exception trap)
                }
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Cut Failed", (int) System.Windows.Forms.MessageBoxIcon.Error);
			}
		}
		
		internal virtual void  EditCopy()
		{
			Molecule frag = editor.SelectedSubgraph();
			try
			{
                MemoryStream stream = new MemoryStream();

                MoleculeStream.WriteMDLMOL(stream, frag);
                MoleculeStream.WriteNative(stream, frag);

                stream.Position = 0;

                using (StreamReader rdr = new StreamReader(stream))
                {                    
                    Clipboard.SetDataObject(rdr.ReadToEnd());
                }
			}
			catch (System.IO.IOException e)
			{
				SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Copy Failed", (int) System.Windows.Forms.MessageBoxIcon.Error);
			}
		}
		
		internal virtual void  EditPaste()
		{
			try
			{
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getSystemClipboard' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
				//UPGRADE_ISSUE: Method 'java.awt.Toolkit.getDefaultToolkit' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtToolkit'"
				System.Windows.Forms.IDataObject contents = Clipboard.GetDataObject();
				if (contents != null && SupportClass.IsDataFormatSupported(contents, System.Windows.Forms.DataFormats.GetFormat(System.Windows.Forms.DataFormats.StringFormat)))
				{
					string cliptext =  contents.GetData(DataFormats.Text).ToString();
                    using (MemoryStream stream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(cliptext)))
                    {
                        Molecule frag = MoleculeStream.ReadUnknown(stream);
                        if (frag != null)
                            editor.AddArbitraryFragment(frag);
                    }
				}
			}
            catch (System.IO.IOException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Paste Failed", (int)System.Windows.Forms.MessageBoxIcon.Error);
            }
			catch (System.Exception e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Clipboard Read Failed", (int) System.Windows.Forms.MessageBoxIcon.Error);
			}
		}
		
		internal virtual void  SelectElement(System.String El)
		{
			if (lastElement != null)
			{
				if (String.CompareOrdinal(lastElement, El) == 0)
					return ;
				//UPGRADE_ISSUE: Constructor 'javax.swing.Image.Image' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingImageIconImageIcon_javanetURL'"
				//UPGRADE_TODO: Method 'java.lang.Class.getResource' was converted to 'System.Uri' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassgetResource_javalangString'"
				toolIcons[TOOL_SETATOM] = new Bitmap(Utility.GetFullPath("/images/" + IMAGE_TOOL[TOOL_SETATOM] + ".png"));
			}
			
			int w = toolIcons[TOOL_SETATOM].Width, h = toolIcons[TOOL_SETATOM].Height;
			Bitmap img = new Bitmap(w, h, (System.Drawing.Imaging.PixelFormat) System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = (Graphics)Graphics.FromImage(img))
            {
                Brush background = new SolidBrush(Color.White); 
                g.FillRectangle(background, 0, 0, w, h);
                g.DrawImage(toolIcons[TOOL_SETATOM], 0, 0);

                Font font = new Font("SansSerif", El.Length == 1 ? 12 : 10, FontStyle.Regular);
                Brush brush = new SolidBrush(System.Drawing.Color.FromArgb(0, 192, 0));
                int textWidth = (int)g.MeasureString(El, font).Width;
                float x = (w - textWidth) / 2 - 3;
                float y = (h + SupportClass.GetAscent(font)) / font.GetHeight(); 

                g.DrawString(El, font, brush, x, y);
            }
			
			lastElement = El;
		}
		
		internal virtual void  TemplateTool()
		{
		/*	if (lastTemplate == null)
			{
				TemplateSelect(); return ;
			}
		*/	editor.SetToolTemplate(lastTemplate, templateIdx);

            TemplateSelect(); return;
		}
		
		internal virtual void  TemplateSelect()
		{
			TemplateSelector sel = new TemplateSelector(this, null);
			//UPGRADE_TODO: Method 'java.awt.Component.setLocation' was converted to 'System.Windows.Forms.Control.Location' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetLocation_javaawtPoint'"
			System.Windows.Forms.Control temp_Control;
			//UPGRADE_NOTE: Exceptions thrown by the equivalent in .NET of method 'java.awt.Component.getLocationOnScreen' may be different. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1099'"
			temp_Control = toolButtons[TOOL_TEMPLATE];
			sel.Location = temp_Control.PointToScreen(temp_Control.Location);
			//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
			//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
			sel.Visible = true;
		}
		
		internal virtual void  TemplateAddTo()
		{
			templ.AddTemplate(editor.SelectedSubgraph());
		}
		
		internal virtual void  EditDialog()
		{
			Molecule newMol = (new DialogEdit(this, editor.MolData(), editor.SelectedIndices())).exec();
			if (newMol != null)
				editor.Replace(newMol, false);
		}
		
		internal virtual void  HelpAbout()
		{
			System.String msg = "SketchEl v" + VERSION + "\n" + "Molecule drawing tool\n" + "© 2005 Dr. Alex M. Clark\n" + "Released under the Gnu Public\n" + "License (GPL), see www.gnu.org\n" + "Home page and documentation:\n" + "http://sketchel.sf.net\n";
			//UPGRADE_TODO: Method 'javax.swing.JOptionPane.showMessageDialog' was converted to 'SupportClass.OptionpaneSupport.showMessageDialog' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJOptionPaneshowMessageDialog_javaawtComponent_javalangObject_javalangString_int_javaxswingIcon'"
			SupportClass.OptionPaneSupport.ShowMessageDialog(null, msg, "About SketchEl", (int) System.Windows.Forms.MessageBoxIcon.Information);
		}
		
		// ------------------ event functions --------------------

        private void normaliseButton_Click(object sender, EventArgs e)
        {
            editor.NormaliseBondLengths();
        }

		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
		{
            string cmd = "";
            if (event_sender is RibbonButton)
            {
                cmd = ((RibbonButton)event_sender).Command;
            }

			int setsel = - 1;

            if (cmd == "Cursor" || event_sender == toolButtons[TOOL_CURSOR])
			{
				editor.SetToolCursor(); setsel = TOOL_CURSOR;
			}
			else if (cmd == "Rotator" || event_sender == toolButtons[TOOL_ROTATOR])
			{
				editor.SetToolRotator(); setsel = TOOL_ROTATOR;
			}
            else if (cmd == "Eraser" || event_sender == toolButtons[TOOL_ERASOR])
			{
				editor.SetToolErasor(); setsel = TOOL_ERASOR;
			}
			else if (cmd == "Edit..." || event_sender == toolButtons[TOOL_DIALOG])
				EditDialog();
			else if (cmd == "Select All")
				editor.SelectAll();
			else if (cmd == "Next Atom")
				editor.CycleSelection(true, false);
			else if (cmd == "Previous Atom")
				editor.CycleSelection(false, false);
			else if (cmd == "Next Group")
				editor.CycleSelection(true, true);
			else if (cmd == "Previous Group")
				editor.CycleSelection(false, true);
			else if (cmd == "Edit Atom" || event_sender == toolButtons[TOOL_EDIT])
			{
				editor.SetToolAtom(null); setsel = TOOL_EDIT;
			}
			else if (cmd == "Set Atom" || event_sender == toolButtons[TOOL_SETATOM])
			{
				editor.SetToolAtom(lastElement); setsel = TOOL_SETATOM;
			}
			else if (cmd == "Single Bond" || event_sender == toolButtons[TOOL_SINGLE])
			{
				editor.SetToolBond(1); setsel = TOOL_SINGLE;
			}
			else if (cmd == "Double Bond" || event_sender == toolButtons[TOOL_DOUBLE])
			{
				editor.SetToolBond(2); setsel = TOOL_DOUBLE;
			}
			else if (cmd == "Triple Bond" || event_sender == toolButtons[TOOL_TRIPLE])
			{
				editor.SetToolBond(3); setsel = TOOL_TRIPLE;
			}
			else if (cmd == "Zero Bond" || event_sender == toolButtons[TOOL_ZERO])
			{
				editor.SetToolBond(0); setsel = TOOL_ZERO;
			}
			else if (cmd == "Inclined Bond" || event_sender == toolButtons[TOOL_INCLINED])
			{
				editor.SetToolBond(- 1); setsel = TOOL_INCLINED;
			}
			else if (cmd == "Declined Bond" || event_sender == toolButtons[TOOL_DECLINED])
			{
				editor.SetToolBond(- 2); setsel = TOOL_DECLINED;
			}
			else if (cmd == "Unknown Bond" || event_sender == toolButtons[TOOL_UNKNOWN])
			{
				editor.SetToolBond(- 3); setsel = TOOL_UNKNOWN;
			}
			else if (cmd == "Charge" || event_sender == toolButtons[TOOL_CHARGE])
			{
				editor.SetToolCharge(1); setsel = TOOL_CHARGE;
			}
			else if (cmd == "Undo" || event_sender == toolButtons[TOOL_UNDO])
				editor.Undo();
			else if (cmd == "Redo" || event_sender == toolButtons[TOOL_REDO])
				editor.Redo();
			else if (cmd == "Cut")
				EditCut();
			else if (cmd == "Copy")
				EditCopy();
			else if (cmd == "Paste")
				EditPaste();
			else if (cmd == "Flip Horizontal")
				editor.FlipSelectedAtoms(false);
			else if (cmd == "Flip Vertical")
				editor.FlipSelectedAtoms(true);
			else if (cmd == "Rotate +45°")
				editor.RotateSelectedAtoms(45);
			else if (cmd == "Rotate -45°")
				editor.RotateSelectedAtoms(- 45);
			else if (cmd == "Rotate +90°")
				editor.RotateSelectedAtoms(90);
			else if (cmd == "Rotate -90°")
				editor.RotateSelectedAtoms(- 90);
			else if (cmd == "Add Temporary Template")
				TemplateAddTo();
			else if (cmd == "Normalise Bond Lengths")
				editor.NormaliseBondLengths();
			else if (cmd == "Templates" || event_sender == toolButtons[TOOL_TEMPLATE])
			{
				TemplateTool(); setsel = TOOL_TEMPLATE;
			}
			else if (cmd == "Select Template")
			{
				TemplateSelect(); setsel = TOOL_TEMPLATE;
			}
            else if (cmd == "Show Hydrogen")
            {
                showHydButton.IsPressed = !showHydButton.IsPressed;
                editor.SetShowHydrogens(showHydButton.IsPressed);
            }
            else if (cmd == "Set Explicit")
                editor.HydrogenSetExplicit(true);
            else if (cmd == "Clear Explicit")
                editor.HydrogenSetExplicit(false);
            else if (cmd == "Zero Explicit")
                editor.HydrogenSetExplicit(false, 0);
            else if (cmd == "Create Actual")
                editor.HydrogenCreateActual();
            else if (cmd == "Delete Actual")
                editor.HydrogenDeleteActual();
            else if (cmd == "Show Stereolabels")
            {
                showStereolabelsButton.IsPressed = !showStereolabelsButton.IsPressed;
                editor.SetShowStereolabels(showStereolabelsButton.IsPressed);
            }
            else if (cmd == "Invert Stereochemistry")
                editor.SetStereo(Molecule.STEREO_UNKNOWN);
            else if (cmd == "Set R/Z")
                editor.SetStereo(Molecule.STEREO_POS);
            else if (cmd == "Set S/E")
                editor.SetStereo(Molecule.STEREO_NEG);
            else if (cmd == "Cycle Wedges")
                editor.CycleChiralWedges();
            else if (cmd == "Remove Wedges")
                editor.RemoveChiralWedges();
            else if (cmd == "About")
                HelpAbout();
            else if (cmd.Length <= 2)
            {
                SelectElement(cmd); editor.SetToolAtom(lastElement);
            }

            foreach (RibbonButton b in buttonArray)
            {
                if(b != showHydButton)
                    b.IsPressed = false;

                b.Refresh();
            }

			if (setsel != - 1)
			{
                if (event_sender is RibbonButton)
                {
                    RibbonButton sender = (RibbonButton)event_sender;
                    sender.IsPressed = true;
                }
			}
		}

        private List<RibbonButton> buttonArray = new List<RibbonButton>();

        private void SetupButtonList()
        {
            buttonArray.Add(selectButton);
            buttonArray.Add(eraseButton);
            buttonArray.Add(rotateButton);
            buttonArray.Add(templateButton);
            buttonArray.Add(chargeButton);
            buttonArray.Add(singleBondButton);
            buttonArray.Add(doubleBondButton);
            buttonArray.Add(tripleBondButton);
            buttonArray.Add(declinedBondButton);
            buttonArray.Add(inclinedBondButton);
            buttonArray.Add(zeroBondButton);
            buttonArray.Add(unknownBondButton);
            buttonArray.Add(editElementButton);
            buttonArray.Add(addElementButton);
            buttonArray.Add(copyButton);
            buttonArray.Add(cutButton);
            buttonArray.Add(editDialogButton);
            buttonArray.Add(pasteButton);
            buttonArray.Add(redoButton);
            buttonArray.Add(undoButton);
            buttonArray.Add(nextGroupButton);
            buttonArray.Add(prevAtomButton);
            buttonArray.Add(nextAtomButton);
            buttonArray.Add(selectAllButton);
            buttonArray.Add(prevGroupButton);
            buttonArray.Add(showHydButton);
            buttonArray.Add(deleteActualButton);
            buttonArray.Add(createActualButton);
            buttonArray.Add(zeroExplicitButton);
            buttonArray.Add(clearExplicitButton);
            buttonArray.Add(setExplicitButton);
            buttonArray.Add(removeWedgesButton);
            buttonArray.Add(cycleWedgesButton);
            buttonArray.Add(showStereolabelsButton);
            buttonArray.Add(setSEbutton);
            buttonArray.Add(setRZbutton);
            buttonArray.Add(invertStereochemistryButton);
        }
		
		public virtual void  mousePressed(System.Object event_sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (event_sender == toolButtons[TOOL_SETATOM] && e.Button == MouseButtons.Middle)
			{
				//UPGRADE_ISSUE: Method 'javax.swing.AbstractButton.setSelected' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingAbstractButtonsetSelected_boolean'"
				toolButtons[TOOL_SETATOM].Select(); // TODO: Same functionality? .setSelected(true);
				//UPGRADE_ISSUE: Class hierarchy differences between 'javax.swing.JPopupMenu' and 'System.Windows.Forms.ContextMenu' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
				System.Windows.Forms.ContextMenu popup = new System.Windows.Forms.ContextMenu();
				popup.MenuItems.Add(MenuItem("C", 0));
				popup.MenuItems.Add(MenuItem("N", 0));
				popup.MenuItems.Add(MenuItem("O", 0));
				popup.MenuItems.Add(MenuItem("H", 0));
				popup.MenuItems.Add(MenuItem("F", 0));
				popup.MenuItems.Add(MenuItem("Cl", 0));
				popup.MenuItems.Add(MenuItem("Br", 0));
				popup.MenuItems.Add(MenuItem("I", 0));
				popup.MenuItems.Add(MenuItem("S", 0));
				popup.MenuItems.Add(MenuItem("P", 0));
				//UPGRADE_TODO: Method 'javax.swing.JPopupMenu.show' was converted to 'System.Windows.Forms.ContextMenu.Show' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJPopupMenushow_javaawtComponent_int_int'"
				popup.Show(toolButtons[TOOL_SETATOM], new System.Drawing.Point(0, 0));
			}
			if (event_sender == toolButtons[TOOL_TEMPLATE] && e.Button == MouseButtons.Middle)
			{
				//UPGRADE_TODO: Method 'javax.swing.AbstractButton.getModel' was converted to 'ToolStripButtonBase' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				SupportClass.ButtonGroupSupport.SetSelected(toolGroup, toolButtons[TOOL_TEMPLATE], true);
				TemplateSelect();
			}
		}
		public virtual void  keyPressed(System.Object event_sender, System.Windows.Forms.KeyEventArgs e)
		{
			// keyboard arrow-nudges
			//UPGRADE_ISSUE: Method 'java.awt.event.InputEvent.isMetaDown' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawteventInputEvent'"
			if (!(System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Alt) && !(System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift) && !(System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control) ) // && !e.isMetaDown())
			{
				//UPGRADE_TODO: Method 'java.awt.event.KeyEvent.getKeyCode' was converted to 'System.Windows.Forms.KeyEventArgs.KeyValue' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawteventKeyEventgetKeyCode'"
				if (e.KeyValue == (int) System.Windows.Forms.Keys.Up)
				{
					editor.NudgeSelectedAtoms(0, 0.05); return ;
				}
				//UPGRADE_TODO: Method 'java.awt.event.KeyEvent.getKeyCode' was converted to 'System.Windows.Forms.KeyEventArgs.KeyValue' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawteventKeyEventgetKeyCode'"
				if (e.KeyValue == (int) System.Windows.Forms.Keys.Down)
				{
					editor.NudgeSelectedAtoms(0, - 0.05); return ;
				}
				//UPGRADE_TODO: Method 'java.awt.event.KeyEvent.getKeyCode' was converted to 'System.Windows.Forms.KeyEventArgs.KeyValue' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawteventKeyEventgetKeyCode'"
				if (e.KeyValue == (int) System.Windows.Forms.Keys.Left)
				{
					editor.NudgeSelectedAtoms(- 0.05, 0); return ;
				}
				//UPGRADE_TODO: Method 'java.awt.event.KeyEvent.getKeyCode' was converted to 'System.Windows.Forms.KeyEventArgs.KeyValue' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawteventKeyEventgetKeyCode'"
				if (e.KeyValue == (int) System.Windows.Forms.Keys.Right)
				{
					editor.NudgeSelectedAtoms(0.05, 0); return ;
				}
			}
		}
		public virtual void  keyTyped(System.Object event_sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// user typing in an element...
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.event.KeyEvent.getKeyChar' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			char ch = e.KeyChar;
			if (ch >= 'A' && ch <= 'Z')
				typedElement = "" + ch;
			else if (typedElement.Length == 1 && ch >= 'a' && ch <= 'z')
				typedElement = typedElement + ch;
			else
			{
				typedElement = ""; return ;
			}
			
			for (int n = 1; n < Molecule.ELEMENTS.Length; n++)
				if (String.CompareOrdinal(typedElement, Molecule.ELEMENTS[n]) == 0)
				{
					SelectElement(typedElement);
					//UPGRADE_TODO: Method 'javax.swing.AbstractButton.getModel' was converted to 'ToolStripButtonBase' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					SupportClass.ButtonGroupSupport.SetSelected(toolGroup, toolButtons[TOOL_SETATOM], true);
					editor.SetToolAtom(lastElement);
					break;
				}
		}
		
		public virtual void  TemplSelected(Molecule mol, int idx)
		{
			lastTemplate = mol;
			templateIdx = idx;
			editor.SetToolTemplate(mol, idx);
		}
		public virtual void  MolSelected(EditorPane source, int idx, bool dblclick)
		{
			if (dblclick && idx != 0)
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				List<int> selidx = new List<int>();
				if (idx > 0)
					selidx.Add(idx);
				else
				{
					selidx.Add(editor.MolData().BondFrom(- idx)); selidx.Add(editor.MolData().BondTo(- idx));
				}
				Molecule newMol = (new DialogEdit(this, editor.MolData(), selidx)).exec();
				if (newMol != null)
					editor.Replace(newMol);
			}
		}
		
		
		public virtual void  lostOwnership(System.Windows.Forms.Clipboard clipboard, System.Windows.Forms.IDataObject contents)
		{
		} // don't care
		
		public virtual void  windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
//			FileQuit();
		}
	
		
		// ------------------ init functions --------------------
		
		internal static System.String[] args;
		
		private static void  createAndShowGUI()
		{
			bool dump = false, stream = false;
			System.String loadfn = null;
			
			int i = 0;
			while (i < args.Length)
			{
				if (args[i][0] == '-')
				{
					if (String.CompareOrdinal(args[i], "-h") == 0 || String.CompareOrdinal(args[i], "--help") == 0)
					{
						dump = true; break;
					}
					if (String.CompareOrdinal(args[i], "-v") == 0 || String.CompareOrdinal(args[i], "--version") == 0)
					{
						dump = true; break;
					}
					if (String.CompareOrdinal(args[i], "-s") == 0 || String.CompareOrdinal(args[i], "--stream") == 0)
					{
						stream = true; i++;
					}
				}
				else
				{
					if (loadfn != null)
					{
						dump = true; System.Console.Out.WriteLine("Unexpected parameter: " + args[i]); break;
					}
					System.IO.FileInfo f = new System.IO.FileInfo(args[i]);
					bool tmpBool;
					if (System.IO.File.Exists(f.FullName))
						tmpBool = true;
					else
						tmpBool = System.IO.Directory.Exists(f.FullName);
					if (!tmpBool)
					{
						dump = true; System.Console.Out.WriteLine("Expected filename, does not exist: " + args[i]); break;
					}
					loadfn = args[i];
					i++;
				}
			}
			
			if (dump)
			{
				System.Console.Out.WriteLine("SketchEl: Molecular drawing tool");
				System.Console.Out.WriteLine("          Version " + VERSION + " © 2005 Dr. Alex M. Clark");
				System.Console.Out.WriteLine("          Open source, released under the Gnu Public License (GPL),");
				System.Console.Out.WriteLine("          see www.gnu.org. For home page and documentation, see");
				System.Console.Out.WriteLine("          http://sketchel.sf.net\n");
				
				System.Console.Out.WriteLine("Command line parameters:");
				System.Console.Out.WriteLine(" -h|--help|-v|--version    Show parameters and summary info");
				System.Console.Out.WriteLine(" -s|--stream               Read from <stdin> at startup, write to");
				System.Console.Out.WriteLine("                           <stdout> on quit.");
				System.Console.Out.WriteLine(" filename                  Open filename on startup.");
			}
			else
			{
				//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
				//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
				MainWindow wnd = new MainWindow();
                wnd.Init(loadfn, stream); 
                wnd.Visible = true;
			}
		}
		
        //[STAThread]
        //public static void  Main(System.String[] args)
        //{
        //    MainWindow.args = args;
        //    //UPGRADE_ISSUE: Method 'javax.swing.SwingUtilities.invokeLater' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingSwingUtilities'"
        //    javax.swing.SwingUtilities.invokeLater(new AnonymousClassRunnable());
        //}

		private void  MainWindow_Closing_DISPOSE_ON_CLOSE(System.Object sender, System.ComponentModel.CancelEventArgs  e)
		{
			e.Cancel = true;
			SupportClass.CloseOperation((System.Windows.Forms.Form) sender, 2);
		}

        //#region ITemplSelectListener Members

        //void ITemplSelectListener.TemplSelected(Molecule mol, int idx)
        //{
            
        //}

        //#endregion

        #region ITemplSelectListener Members

        void ITemplSelectListener.TemplSelected(Molecule mol, int idx)
        {
            lastTemplate = mol;
            templateIdx = idx;
            editor.SetToolTemplate(mol, idx);
        }

        #endregion

        public Molecule MolData()
        {
            return editor.MolData(); 
        }

        public void NotifySaved()
        {
            editor.NotifySaved(); 
        }

        public bool IsEmpty()
        {
            return editor.IsEmpty(); 
        }

        public void Open(string fileName)
        {
            bool fresh = editor.IsEmpty();
            // TODO: not sure what this is. 
            string newfn = fileName; 

            bool anything = editor.MolData().NumAtoms() > 0;
            try
            {
                FileStream istr = new FileStream(newfn, FileMode.Open, FileAccess.Read);
                /*if (MoleculeStream.ExamineIsDatabase(istr))
                {
                    istr.Close();
                    CatalogWindow cw = new CatalogWindow(newfn);
                    //UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
                    //UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
                    cw.Visible = true;
                }
                else
                {*/
                    string ext = Path.GetExtension(newfn);

                    if (ext == ".xml" || ext == ".cml")
                    {
                        Molecule frag = MoleculeStream.ReadXML(istr);
                        editor.AddArbitraryFragment(frag);
                        istr.Close();
                        if (fresh)
                            SetFilename(newfn);
                        if (!anything)
                            editor.NotifySaved();
                    }
                    else
                    {
                        Molecule frag = MoleculeStream.ReadUnknown(istr);
                        editor.AddArbitraryFragment(frag);
                        istr.Close();
                        if (fresh)
                            SetFilename(newfn);
                        if (!anything)
                            editor.NotifySaved();
                    }
                //}
            }
            catch (IOException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Open Failed", (int)System.Windows.Forms.MessageBoxIcon.Error);
                //e.printStackTrace();
            }
        }

        public void Save()
        {
            if (filename == null)
            {
                // TODO: check this out 
                // FileSaveAs(); return;
                SaveAs(""); 
            }
            SaveCurrent();
        }

        public void ExportBMP(string filename)
        {
            editor.WriteCanvasToFile(filename);
        }

        public void ExportMOL(string filename)
        {
            string fn = filename; //  new System.IO.FileInfo().FullName;
            if (filename.IndexOf('.') < 0)
                fn = fn + ".mol";

            try
            {
                //UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
                System.IO.FileStream ostr = new System.IO.FileStream(fn, System.IO.FileMode.Create);
                MoleculeStream.WriteMDLMOL(ostr, editor.MolData());
                ostr.Close();
            }
            catch (System.IO.IOException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Export Failed", (int)System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public void ExportCML(string fileName)
        {
            string fn = fileName; //  new System.IO.FileInfo(chooser.FileName).FullName;
            if (fileName.IndexOf('.') < 0)
                fn = fn + ".xml";

            try
            {
                //UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
                System.IO.FileStream ostr = new System.IO.FileStream(fn, System.IO.FileMode.Create);
                MoleculeStream.WriteCMLXML(ostr, editor.MolData());
                ostr.Close();
            }
            catch (System.IO.IOException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Export Failed", (int)System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public void SaveAs(string filename)
        {
            System.String fn = filename; //  new System.IO.FileInfo(chooser.FileName).FullName;
            if (filename.IndexOf('.') < 0)
                fn = fn + ".el";

            SetFilename(fn);
            SaveCurrent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {           
            MouseEventArgs mouseargs = e as MouseEventArgs;            
            if (mouseargs != null && mouseargs.Button == MouseButtons.Right && sender == addElementButton)
            {
                menuAtomTypes.Show(((Control)sender).PointToScreen(mouseargs.Location));                
            }
            else
            {
                actionPerformed(sender, e);
            }
        }

        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStrip_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void toolStrip_Click(object sender, EventArgs e)
        {

        }

        private void menuAtomTypes_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem item = sender as ToolStripDropDownItem;

            if (item != null)
            {
                SelectElement(item.Text);
                editor.SetToolAtom(lastElement);
            }

            addElementButton.Text = item.Text;
            addElementButton.Command = item.Text;
        }

    }
}

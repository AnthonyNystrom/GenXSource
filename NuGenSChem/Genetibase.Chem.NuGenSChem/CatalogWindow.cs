using System;
using System.Collections.Generic;
using System.Drawing;
namespace Genetibase.Chem.NuGenSChem
{
	
	/*
	A window class for displaying a catalog of structures, ripped out of a text-style database such as an SD-file.*/
	
	[Serializable]
	public class CatalogWindow:System.Windows.Forms.Form, MolSelectListener
	{
		internal System.String catFN;
		internal System.IO.FileStream istr = null;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'COL_SIZE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal int COL_SIZE = 5;
		internal int basepos = 0, selnum = - 1;
		
		internal System.Drawing.Color bckgr
		{
			get
			{
				return bckgr_Renamed;
			}
			
			set
			{
				bckgr_Renamed = value;
			}
			
		}
		internal System.Drawing.Color highl
		{
			get
			{
				return highl_Renamed;
			}
			
			set
			{
				highl_Renamed = value;
			}
			
		}
		internal System.Drawing.Color bckgr_Renamed, highl_Renamed;
		
		//UPGRADE_TODO: Class 'javax.swing.JScrollBar' was converted to 'System.Windows.Forms.ScrollBar' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
		internal System.Windows.Forms.ScrollBar scroll;
		internal System.Windows.Forms.Button bopen, bclose;
		internal EditorPane[] entries;
		
		internal CatalogLoader loader = null;
		
		public CatalogWindow(System.String CatFN)
		{
			catFN = CatFN;
			
			Closing += new System.ComponentModel.CancelEventHandler(this.CatalogWindow_Closing_DISPOSE_ON_CLOSE);
			// JFrame.setDefaultLookAndFeelDecorated(false);
			
			Text = "SketchEl Catalog - " + catFN;
			
			bckgr = BackColor;
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getRed' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getGreen' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.awt.Color.getBlue' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			highl = System.Drawing.Color.FromArgb(System.Math.Max((int) bckgr.R - 32, 0), System.Math.Max((int) bckgr.G - 32, 0), (int) bckgr.B);
			
			//UPGRADE_TODO: Class 'javax.swing.Image' was converted to 'System.Drawing.Image' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingImageIcon'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.Image.Image' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingImageIconImageIcon_javanetURL'"
			GetType();
			//UPGRADE_TODO: Method 'java.lang.Class.getResource' was converted to 'System.Uri' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassgetResource_javalangString'"
            System.Drawing.Image mainIcon = new Bitmap(Utility.GetFullPath("/images/MainIcon.png"));
			Icon = System.Drawing.Icon.FromHandle(((System.Drawing.Bitmap) mainIcon).GetHicon());
			
			//UPGRADE_ISSUE: Class 'java.awt.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			//UPGRADE_ISSUE: Constructor 'java.awt.GridBagLayout.GridBagLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
			// TODO: GridBagLayout gb = new GridBagLayout();
			//UPGRADE_ISSUE: Class 'java.awt.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            ////UPGRADE_ISSUE: Constructor 'java.awt.GridBagConstraints.GridBagConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //GridBagConstraints gc = new GridBagConstraints();
            ////UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.insets' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //gc.insets = new System.Int32[]{2, 2, 2, 2};
			
			// add the molecules
			System.Windows.Forms.Panel content = new System.Windows.Forms.Panel();
			//UPGRADE_ISSUE: Method 'java.awt.Container.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtContainersetLayout_javaawtLayoutManager'"
			//UPGRADE_ISSUE: Constructor 'javax.swing.BoxLayout.BoxLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			//UPGRADE_ISSUE: Field 'javax.swing.BoxLayout.X_AXIS' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingBoxLayout'"
			// TODO: See how these layout command translate, this will be a bit of trial and error
            // content.setLayout(new BoxLayout(content, BoxLayout.X_AXIS));
			entries = new EditorPane[COL_SIZE];
			for (int n = 0; n < COL_SIZE; n++)
			{
				entries[n] = new EditorPane(100, 100, true);
				entries[n].SetEditable(false);
				entries[n].SetMolSelectListener(this);
				entries[n].SetToolCursor();
				entries[n].SetBorder(true);
				entries[n].MaximumSize = new System.Drawing.Size(System.Int16.MaxValue, System.Int16.MaxValue);
				content.Controls.Add(entries[n]);
			}
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.BOTH' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			// TODO: gc.fill = GridBagConstraints.BOTH;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.REMAINDER' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			// TODO: gc.gridwidth = GridBagConstraints.REMAINDER;
            ////UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //gc.weightx = 1;
            ////UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //gc.weighty = 1;
            ////UPGRADE_ISSUE: Method 'java.awt.GridBagLayout.setConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
            //gb.setConstraints(content, gc);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			Controls.Add(content);
			
			// add the scroller and buttons
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.fill' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.HORIZONTAL' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			// TODO: gc.fill = GridBagConstraints.HORIZONTAL;
			//UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weighty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
			// gc.weighty = 0;
			
			scroll = new System.Windows.Forms.HScrollBar();
			scroll.Value = 0;
			scroll.Minimum = 0;
			scroll.Maximum = 0;
			scroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.adjustmentValueChanged);
            ////UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //gc.gridwidth = 5;
            ////UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //gc.weightx = 1;
            ////UPGRADE_ISSUE: Method 'java.awt.GridBagLayout.setConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
            //gb.setConstraints(scroll, gc);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			Controls.Add(scroll);
			
			bopen = SupportClass.ButtonSupport.CreateStandardButton("Open");
			bopen.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(bopen);
            ////UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //gc.gridwidth = 1;
            ////UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //gc.weightx = 0;
            ////UPGRADE_ISSUE: Method 'java.awt.GridBagLayout.setConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
            //gb.setConstraints(bopen, gc);
			//UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			Controls.Add(bopen);
			
			bclose = SupportClass.ButtonSupport.CreateStandardButton("Close");
			bclose.Click += new System.EventHandler(this.actionPerformed);
			SupportClass.CommandManager.CheckCommand(bclose);
            ////UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.gridwidth' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //gc.gridwidth = 1;
            ////UPGRADE_ISSUE: Field 'java.awt.GridBagConstraints.weightx' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagConstraints'"
            //gc.weightx = 0;
            ////UPGRADE_ISSUE: Method 'java.awt.GridBagLayout.setConstraints' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtGridBagLayout'"
            //gb.setConstraints(bclose, gc);
            ////UPGRADE_TODO: Method 'java.awt.Container.add' was converted to 'System.Windows.Forms.ContainerControl.Controls.Add' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtContaineradd_javaawtComponent'"
			Controls.Add(bclose);
			
			//UPGRADE_ISSUE: Method 'javax.swing.JFrame.setLayout' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaxswingJFramesetLayout_javaawtLayoutManager'"
			// TODO: Needed? setLayout(gb);

			//UPGRADE_ISSUE: Method 'java.awt.Window.pack' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtWindowpack'"
			// TODO: What does the AWT pack method do?
            // pack();
			//UPGRADE_WARNING: Extra logic should be included into componentHidden to know if the Component is hidden. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1144'"
			VisibleChanged += new System.EventHandler(this.componentHidden);
			Move += new System.EventHandler(this.componentMoved);
			Resize += new System.EventHandler(this.componentResized);
			//UPGRADE_WARNING: Extra logic should be included into componentShown to know if the Component is visible. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1145'"
			VisibleChanged += new System.EventHandler(this.componentShown);
			//UPGRADE_NOTE: Some methods of the 'java.awt.event.WindowListener' class are not used in the .NET Framework. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1308'"
			Activated += new System.EventHandler(this.windowActivated);
			Closed += new System.EventHandler(this.windowClosed);
			Closing += new System.ComponentModel.CancelEventHandler(this.windowClosing);
			Deactivate += new System.EventHandler(this.windowDeactivated);
			Load += new System.EventHandler(this.windowOpened);
			
			try
			{
				//UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
				istr = new System.IO.FileStream(catFN, System.IO.FileMode.Open, System.IO.FileAccess.Read);
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Catalog Read Failed", (int) System.Windows.Forms.MessageBoxIcon.Error);
				Dispose();
				return ;
			}
			
			loader = new CatalogLoader(this, istr);
			loader.Start();
		}
		
		
		public virtual void  Synchronize(int Sz)
		{
            if (!scroll.InvokeRequired)
            {
                scroll.Maximum = Sz;
                //UPGRADE_TODO: Method 'javax.swing.JScrollBar.setVisibleAmount' was converted to System.Windows.Forms.ScrollBar.LargeChange which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollBarsetVisibleAmount_int'"
                scroll.LargeChange = COL_SIZE;
                //UPGRADE_WARNING: Method javax.swing.JScrollbar.setUnitIncrement was converted to 'System.Windows.Forms.RadioButton.SmallChange' which may throw an exception. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1101'"
                scroll.SmallChange = 1;
                //UPGRADE_TODO: Method 'javax.swing.JScrollBar.setBlockIncrement' was converted to 'System.Windows.Forms.RadioButton.LargeChange' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaxswingJScrollBarsetBlockIncrement_int'"
                scroll.LargeChange = COL_SIZE;

                if (basepos + 5 >= Sz)
                    UpdatePosition(basepos);
            }
		}
		
		internal virtual void  UpdatePosition(int NewPos)
		{
			// NB: should speed this up sometime by re-using molecules that are already loaded
			
			basepos = NewPos;
			try
			{
				for (int n = 0; n < COL_SIZE; n++)
				{
					if (basepos + n >= loader.Count())
					{
						entries[n].BackColor = bckgr;
						entries[n].Clear();
					}
					else
					{
						Molecule mol = loader.Get(basepos + n);
						entries[n].BackColor = basepos + n == selnum?highl:bckgr;
						entries[n].Replace(mol, true, false);
						entries[n].ScaleToFit();
						//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
						entries[n].Refresh();
					}
				}
			}
			catch (System.Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'OpenMolecule'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual void  OpenMolecule(int N)
		{
			lock (this)
			{
				try
				{
					Molecule mol = loader.Get(N);
					MainWindow mw = new MainWindow();
                    mw.Init(null, false); 
					mw.SetMolecule(mol);
					//UPGRADE_TODO: Method 'java.awt.Component.setVisible' was converted to 'System.Windows.Forms.Control.Visible' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentsetVisible_boolean'"
					//UPGRADE_TODO: 'System.Windows.Forms.Application.Run' must be called to start a main form. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1135'"
					mw.Visible = true;
				}
				catch (System.Exception e)
				{
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
		}
		
		// event handling
		
		public virtual void  MolSelected(EditorPane source, int idx, bool dblclick)
		{
			int entnum = - 1;
			for (int n = 0; n < COL_SIZE; n++)
				if (source == entries[n])
				{
					entnum = n; break;
				}
			if (entnum >= 0)
			{
				selnum = basepos + entnum;
				for (int n = 0; n < COL_SIZE; n++)
				{
					entries[n].BackColor = n == entnum?highl:bckgr;
					//UPGRADE_TODO: Method 'java.awt.Component.repaint' was converted to 'System.Windows.Forms.Control.Refresh' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaawtComponentrepaint'"
					entries[n].Refresh();
				}
				
				if (idx != 0)
					OpenMolecule(selnum);
			}
		}
		public virtual void  DirtyChanged(bool isdirty)
		{
		}
		
		public virtual void  actionPerformed(System.Object event_sender, System.EventArgs e)
		{
			if (event_sender == bopen)
			{
				if (selnum >= 0)
					OpenMolecule(selnum);
			}
			if (event_sender == bclose)
			{
				if (loader != null)
					loader.Zap();
				Dispose();
				return ;
			}
		}
		
		public virtual void  componentHidden(System.Object event_sender, System.EventArgs e)
		{
		}
		public virtual void  componentMoved(System.Object event_sender, System.EventArgs e)
		{
		}
		public virtual void  componentResized(System.Object event_sender, System.EventArgs e)
		{
			for (int n = 0; n < COL_SIZE; n++)
				entries[n].ScaleToFit();
		}
		public virtual void  componentShown(System.Object event_sender, System.EventArgs e)
		{
		}
		
		public virtual void  adjustmentValueChanged(System.Object event_sender, System.Windows.Forms.ScrollEventArgs e)
		{
			if (event_sender == scroll)
			{
				int pos = e.NewValue;
				if (pos != basepos)
					UpdatePosition(pos);
			}
		}
		
		public virtual void  windowActivated(System.Object event_sender, System.EventArgs e)
		{
		}
		public virtual void  windowClosed(System.Object event_sender, System.EventArgs e)
		{
			loader.Zap();
		}
		public virtual void  windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
		}
		public virtual void  windowDeactivated(System.Object event_sender, System.EventArgs e)
		{
		}
		public virtual void  windowDeiconified(System.Object event_sender, System.EventArgs e)
		{
		}
		public virtual void  windowIconified(System.Object event_sender, System.EventArgs e)
		{
		}
		public virtual void  windowOpened(System.Object event_sender, System.EventArgs e)
		{
		}
		private void  CatalogWindow_Closing_DISPOSE_ON_CLOSE(System.Object sender, System.ComponentModel.CancelEventArgs  e)
		{
			e.Cancel = true;
			SupportClass.CloseOperation((System.Windows.Forms.Form) sender, 2);
		}
	}
	
	// background thread which loads up the entries of the database, and lets the window know when new ones have arrived
	
	class CatalogLoader:SupportClass.ThreadClass
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassRunnable' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        //private class AnonymousClassRunnable : IThreadRunnable
        //{
        //    public AnonymousClassRunnable(CatalogLoader enclosingInstance)
        //    {
        //        InitBlock(enclosingInstance);
        //    }
        //    //UPGRADE_NOTE: Delegate might have a different return value and generate a compilation error. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1309'"
        //    // public delegate ~unresolved generatedDelegate();

        //    private void  InitBlock(CatalogLoader enclosingInstance)
        //    {
        //        this.enclosingInstance = enclosingInstance;
        //    }
        //    private CatalogLoader enclosingInstance;
        //    public CatalogLoader Enclosing_Instance
        //    {
        //        get
        //        {
        //            return enclosingInstance;
        //        }
				
        //    }
        //    public virtual void  Run()
        //    {
               

        //        // Enclosing_Instance.wnd.Invoke(new generatedDelegate(Enclosing_Instance.wnd.Synch), new object[]{filepos.Count});
        //    }
        //}
        delegate void Synch(int Sz); 

		internal CatalogWindow wnd;
		internal System.IO.FileStream istr;
		internal bool zap = false;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		List<long> filepos = new List<long>();
		internal System.Object mutex = new System.Object();
		
		public CatalogLoader(CatalogWindow Wnd, System.IO.FileStream IStr)
		{
			wnd = Wnd;
			istr = IStr;
		}
		
		override public void  Run()
		{
			try
			{
				long pos = 0, nextpos;
				while (!zap)
				{
					lock (mutex)
					{
						nextpos = MoleculeStream.FindNextPosition(istr, pos);
					}
					if (nextpos < 0)
						break;
					
					filepos.Add(pos);
					pos = nextpos;
					
					// inform the main window that is has work to do!
					//UPGRADE_ISSUE: Method 'java.awt.EventQueue.invokeLater' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaawtEventQueue'"
                    //EventQueue.invokeLater(new AnonymousClassRunnable(this));
                    Synch synchDelegate = new Synch(wnd.Synchronize);
                    synchDelegate(filepos.Count); 
				}
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				SupportClass.OptionPaneSupport.ShowMessageDialog(null, e.ToString(), "Catalog Load Failed", (int) System.Windows.Forms.MessageBoxIcon.Error);
				SupportClass.WriteStackTrace(e, Console.Error);
			}
		}
		
		public virtual void  Zap()
		{
			zap = true;
		}
		public virtual int Count()
		{
			lock (mutex)
			{
                return filepos.Count; // .Count;
			}
		}
		public virtual Molecule Get(int N)
		{
			try
			{
				lock (mutex)
				{
					return MoleculeStream.FetchFromPosition(istr, filepos[N]);
				}
			}
			catch (System.IO.IOException e)
			{
				return null;
			}
		}
	}
}
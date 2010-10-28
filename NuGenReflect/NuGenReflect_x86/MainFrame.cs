/***
 * 
 *  ASMEX by RiskCare Ltd.
 * 
 * This source is copyright (C) 2002 RiskCare Ltd. All rights reserved.
 * 
 * Disclaimer:
 * This code is provided 'as is', with absolutely no warranty expressed or
 * implied.  Any use of this code is at your own risk.
 *   
 * You are hereby granted the right to redistribute this source unmodified
 * in its original archive. 
 * You are hereby granted the right to use this code, or code based on it,
 * provided that you acknowledge RiskCare Ltd somewhere in the documentation
 * of your application. 
 * You are hereby granted the right to distribute changes to this source, 
 * provided that:
 * 
 * 1 -- This copyright notice is retained unchanged 
 * 2 -- Your changes are clearly marked 
 * 
 * Enjoy!
 * 
 * --------------------------------------------------------------------
 * 
 * If you use this code or have comments on it, please mail me at 
 * support@jbrowse.com or ben.peterson@riskcare.com
 * 
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;


namespace Genetibase.Debug
{
	/// <summary>
	/// The main frame.  Contains some mdi children and a docked ObjViewer.
	/// </summary>
	public class MainFrame : System.Windows.Forms.Form
	{
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.ComponentModel.IContainer components;

		public static System.Windows.Forms.ImageList ilObjTree;
		internal Genetibase.Debug.ObjViewer.ObjViewer theViewer;
		private System.Windows.Forms.ImageList ilToolbar;

		public enum StartUpAction{Blank, Restore, Path, Common};

		private StartUpAction _startUpAction;
		private string[] _path;
		private DevComponents.DotNetBar.Bar bar1;
		private DevComponents.DotNetBar.ButtonItem item_2;
		private DevComponents.DotNetBar.ButtonItem item_3;
		private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
		private Janus.Windows.UI.Dock.UIPanel uiPanel0;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
		private DevComponents.DotNetBar.ButtonItem item_13;
        private DevComponents.DotNetBar.ButtonItem item_14;

		FindState _find;

		public StartUpAction SUA
		{
			get{return _startUpAction;}
			set{_startUpAction = value;}
		}

		public string[] Path
		{
			get{return _path;}
			set{_path = value;}
		}

		public MainFrame()
		{
			if (ilObjTree == null)
				ilObjTree = LoadImageListStream(this.GetType(), "Genetibase.Debug.treeicons.bmp",new Size(16,16), true, new Point(0,0));

			
			InitializeComponent();

			//set the icon -- if we let studio write this code it freaks
			try
			{
				//				System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutDlg));
				//				this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			}
			catch//(Exception e)
			{
				//paranoia, it's the hardest song in original DDR.
			}
			

//			ilToolbar = LoadImageListStream(this.GetType(), "Asmex.buttons.bmp", new Size(32,32), true, new Point(0,0));

//			toolBar1.ImageList = ilToolbar;

			ToolBarButton btn;

			btn = new ToolBarButton();
			btn.ToolTipText = "Open File";
			btn.ImageIndex = 0;
//			toolBar1.Buttons.Add(btn);

			btn = new ToolBarButton();
			btn.ToolTipText = "Open Assembly";
			btn.ImageIndex = 1;
//			toolBar1.Buttons.Add(btn);

			btn = new ToolBarButton();
			btn.ToolTipText = "New View";
			btn.ImageIndex = 2;
//			toolBar1.Buttons.Add(btn);

			btn = new ToolBarButton();
			btn.ToolTipText = "Find";
			btn.ImageIndex = 3;
//			toolBar1.Buttons.Add(btn);

			btn = new ToolBarButton();
			btn.ToolTipText = "Find Again";
			btn.ImageIndex = 4;
//			toolBar1.Buttons.Add(btn);

			btn = new ToolBarButton();
			btn.ToolTipText = "Preferences";
			btn.ImageIndex = 5;
//			toolBar1.Buttons.Add(btn);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrame));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ilToolbar = new System.Windows.Forms.ImageList(this.components);
            this.theViewer = new Genetibase.Debug.ObjViewer.ObjViewer();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.item_2 = new DevComponents.DotNetBar.ButtonItem();
            this.item_3 = new DevComponents.DotNetBar.ButtonItem();
            this.item_13 = new DevComponents.DotNetBar.ButtonItem();
            this.item_14 = new DevComponents.DotNetBar.ButtonItem();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // ilToolbar
            // 
            this.ilToolbar.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilToolbar.ImageSize = new System.Drawing.Size(16, 16);
            this.ilToolbar.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // theViewer
            // 
            this.theViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.theViewer.Location = new System.Drawing.Point(0, 0);
            this.theViewer.Name = "theViewer";
            this.theViewer.Size = new System.Drawing.Size(268, 679);
            this.theViewer.TabIndex = 4;
            // 
            // bar1
            // 
            this.bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.item_2,
            this.item_3,
            this.item_13,
            this.item_14});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(1016, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.bar1.TabIndex = 6;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // item_2
            // 
            this.item_2.Name = "item_2";
            this.item_2.Text = "Open Assembly";
            this.item_2.Click += new System.EventHandler(this.item_2_Click);
            // 
            // item_3
            // 
            this.item_3.Name = "item_3";
            this.item_3.Text = "Open Assembly From GAC";
            this.item_3.Click += new System.EventHandler(this.item_3_Click);
            // 
            // item_13
            // 
            this.item_13.Name = "item_13";
            this.item_13.Text = "Find";
            this.item_13.Click += new System.EventHandler(this.item_13_Click);
            // 
            // item_14
            // 
            this.item_14.Name = "item_14";
            this.item_14.Text = "Find Again";
            this.item_14.Click += new System.EventHandler(this.item_14_Click);
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanel0.Id = new System.Guid("a7968a0c-6f4c-4839-ae9f-b95098a1cd1f");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("a7968a0c-6f4c-4839-ae9f-b95098a1cd1f"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(274, 703), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("a7968a0c-6f4c-4839-ae9f-b95098a1cd1f"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.AllowPanelDrag = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.AllowPanelDrop = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.AutoHide = true;
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(740, 28);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(274, 703);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "Details";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.theViewer);
            this.uiPanel0Container.Location = new System.Drawing.Point(5, 23);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(268, 679);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // MainFrame
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1016, 734);
            this.Controls.Add(this.bar1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NuGenReflect.Net";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDrop);
            this.MdiChildActivate += new System.EventHandler(this.OnMdiChildActivate);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.Load += new System.EventHandler(this.MainFrame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main() 
		{
			Application.Run(new MainFrame());
		}

//		private void mnuAsmOpen_Click(object sender, System.EventArgs e)
//		{
//			GACPicker p = new GACPicker();
//			if (p.ShowDialog() == DialogResult.OK)
//			{
//				AsmView asm = GetView(false);
//				asm.AddRoot(BaseNode.MakeNode(p.Assembly));
//			}
//		}

//		private void mnuFileOpen_Click(object sender, System.EventArgs e)
//		{
//			openFileDialog1.CheckFileExists= true;
//			openFileDialog1.CheckPathExists = true;
//			openFileDialog1.Filter ="Exe files (*.exe)|*.exe|Dll files (*.dll)|*.dll|All files (*.*)|*.*";
//			openFileDialog1.Multiselect = false;
//			openFileDialog1.ReadOnlyChecked = true;
//			if (openFileDialog1.ShowDialog() == DialogResult.OK)
//			{
//				AsmView asm = GetView(false);
//				asm.AddRoot(BaseNode.MakeNode(openFileDialog1.FileName));
//			}
//		}

		private void mnuAsmOpenNV_Click(object sender, System.EventArgs e)
		{
			GACPicker p = new GACPicker();
			if (p.ShowDialog() == DialogResult.OK)
			{
				AsmView asm = GetView(true);
				asm.AddRoot(BaseNode.MakeNode(p.Assembly));
			}
		}

		private void mnuFileOpenNV_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.CheckFileExists= true;
			openFileDialog1.CheckPathExists = true;
			openFileDialog1.Filter ="Exe files (*.exe)|*.exe|Dll files (*.dll)|*.dll|All files (*.*)|*.*";
			openFileDialog1.Multiselect = false;
			openFileDialog1.ReadOnlyChecked = true;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				AsmView asm = GetView(true);
				asm.AddRoot(BaseNode.MakeNode(openFileDialog1.FileName));
			}
		}

		private void mnuFind_Click(object sender, System.EventArgs e)
		{
			FindDialog dlg = new FindDialog(_find);

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				_find = dlg.Find;
				_find.Node = ((AsmView)ActiveMdiChild).SelectedNode;
				((AsmView)ActiveMdiChild).Find(_find);
			}
		}

		private void mnuFindAgain_Click(object sender, System.EventArgs e)
		{
			if (_find != null)
			{
				_find.Node = ((AsmView)ActiveMdiChild).SelectedNode;
				_find.StartAtRoot = false;
				((AsmView)ActiveMdiChild).Find(_find);
			}
		}

		private void mnuNewView_Click(object sender, System.EventArgs e)
		{
			GetView(true);
		}

		private AsmView GetView(bool bNew)
		{
			AsmView v;
			if (bNew)
			{
				v = new AsmView();
			}
			else
			{
				v = (AsmView)this.ActiveMdiChild;
				if (v==null) v = new AsmView();
			}
			v.MdiParent = this;
			v.Visible = true;
			return v;
		}

		private void MainFrame_Load(object sender, System.EventArgs e)
		{
			AsmView asm;

//			LoadPrefs();

			switch(_startUpAction)
			{
				case StartUpAction.Restore:
//					LoadState();break;
				case StartUpAction.Blank:
					break;
				case StartUpAction.Common:
					asm = GetView(true);
					asm.WindowState = FormWindowState.Maximized;
					asm.AddRoot(new AsmNode(typeof(System.String).Assembly));
					asm.AddRoot(new AsmNode(typeof(System.Windows.Forms.Form).Assembly));
					asm.AddRoot(new AsmNode(typeof(System.Xml.XmlAttribute).Assembly));
					asm.AddRoot(new AsmNode(typeof(System.Web.Services.WebService).Assembly));
					asm.AddRoot(new AsmNode(typeof(System.Drawing.Brushes).Assembly));
					break;
				default:
					asm = GetView(true);
					asm.WindowState = FormWindowState.Maximized;
					IList ass = GetPathAssemblies();
					for (int i=0; i< ass.Count; ++i)
					{
						asm.AddRoot(new AsmNode((Assembly)ass[i]));
					}	
					break;
			}
			
		}


		public static ImageList LoadImageListStream(Type assemblyType, 
			string imageName, 
			Size imageSize,
			bool makeTransparent,
			Point transparentPixel)
		{
			// Create storage for bitmap strip
			ImageList images = new ImageList();

			// Define the size of images we supply
			images.ImageSize = imageSize;

			// Get the assembly that contains the bitmap resource
			Assembly myAssembly = Assembly.GetAssembly(assemblyType);

			// Get the resource stream containing the images
			Stream imageStream = myAssembly.GetManifestResourceStream(imageName);

			// Load the bitmap strip from resource
			Bitmap pics = new Bitmap(imageStream, true);

			if (makeTransparent)
			{
				Color backColor = pics.GetPixel(transparentPixel.X, transparentPixel.Y);
    
				// Make backColor transparent for Bitmap
				pics.MakeTransparent(backColor);
			}
			    
			// Load them all !
			images.Images.AddStrip(pics);

			return images;
		}

//		private void OnToolBar(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
//		{
//			if (e.Button.ToolTipText == "Open File")
//			{
//				mnuFileOpen_Click(sender, e);
//			}
//			else if (e.Button.ToolTipText == "Open Assembly")
//			{
//				mnuAsmOpen_Click(sender, e);
//			}
//			else if (e.Button.ToolTipText == "New View")
//			{
//				mnuNewView_Click(sender, e);
//			}
//			else if (e.Button.ToolTipText == "Find")
//			{
//				mnuFind_Click(sender, e);
//			}
//			else if (e.Button.ToolTipText == "Find Again")
//			{
//				mnuFindAgain_Click(sender, e);
//			}
//			else if (e.Button.ToolTipText == "Preferences")
//			{
//				menuItem4_Click(sender, e);
//			}
//		}

//		private void MainFrame_Closing(object sender, System.ComponentModel.CancelEventArgs e)
//		{
//			try
//			{
//
//				RegistryKey rk = Registry.LocalMachine.CreateSubKey("SOFTWARE\\jBrowse\\Asmex");
//				rk.DeleteSubKeyTree("");
//			}
//			catch//(Exception ee)
//			{
//			}
//
//			SaveState();
//			SavePrefs();
//		}

		private void OnMdiChildActivate(object sender, System.EventArgs e)
		{
			if (ActiveMdiChild == null) return;

			((AsmView)ActiveMdiChild).OnActivate();
		}

		private void OnDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			object o  = e.Data.GetData(DataFormats.FileDrop);
			if (o == null)
			{
				MessageBox.Show("Drag a file from Explorer to open it");
				return;
			}

			AsmView v = GetView(true);
			Array a = (Array)o;

			for(int i=0;i<a.Length;++i)
			{
				BaseNode b = BaseNode.MakeNode(a.GetValue(i));
				if (b != null)
					v.AddRoot(b);
			}

		}

		private void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			object o  = e.Data.GetData(DataFormats.FileDrop);
			if (o==null)
			{
				e.Effect = DragDropEffects.None;
			}
			else
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

//		private void menuItem4_Click(object sender, System.EventArgs e)
//		{
//			ConfigDlg dlg = new ConfigDlg(this);
//			dlg.ShowDialog();
//		}	
	

//		void SaveState()
//		{
//			try
//			{
//				RegistryKey rk = Registry.LocalMachine.CreateSubKey("SOFTWARE\\jBrowse\\Asmex");
//
//				for (int i=0; i < this.MdiChildren.Length; ++i)
//				{
//					AsmView v = (AsmView)MdiChildren[i];
//
//					RegistryKey vk = rk.CreateSubKey(i.ToString());
//
//					vk.SetValue("x", v.Left);
//					vk.SetValue("y", v.Top);
//					vk.SetValue("w", v.Width);
//					vk.SetValue("h", v.Height);
//					vk.SetValue("s", (int)v.WindowState);
//
//					for(int j=0; j < v.Roots.Length; ++j)
//					{
//						vk.SetValue("root" + j.ToString(), v.Roots[j].Desc);
//					}
//
//					vk.Close();
//				}	
//
//				rk.Close();
//			}
//			catch//(Exception e)
//			{
//				MessageBox.Show("Unable to save configuration; does this account have permission to write to HKEY_LOCAL_MACHINE\\Software?");
//			}
//		}

//		void LoadState()
//		{
//			try
//			{
//				RegistryKey rk = Registry.LocalMachine.CreateSubKey("SOFTWARE\\jBrowse\\Asmex");
//
//				string[] subkeys = rk.GetSubKeyNames();
//				for (int i=0; i < subkeys.Length; ++i)
//				{
//					AsmView v = GetView(true);
//
//					RegistryKey vk = rk.OpenSubKey(subkeys[i]);
//
//					v.WindowState = (FormWindowState)vk.GetValue("s", FormWindowState.Normal);
//					v.Left = (int)vk.GetValue("x", 100);
//					v.Top = (int)vk.GetValue("y", 100);
//					v.Width = (int)vk.GetValue("w", 600);
//					v.Height = (int)vk.GetValue("h", 400);
//					
//
//					string[] values = vk.GetValueNames();
//					for(int j=0; j < values.Length; ++j)
//					{
//						if (values[j].StartsWith("root"))
//							v.AddRoot(BaseNode.MakeNode((string)vk.GetValue(values[j])));
//					}
//
//					vk.Close();
//				}	
//
//				rk.Close();
//			}
//			catch//(Exception e)
//			{
//				MessageBox.Show("Unable to load configuration; does this account have permission to access to HKEY_LOCAL_MACHINE\\Software?");
//			}
//
//		}

		void SavePrefs()
		{
			try
			{
				RegistryKey rk = Registry.LocalMachine.CreateSubKey("SOFTWARE\\jBrowse\\Asmex");

				rk.SetValue("x", Left);
				rk.SetValue("y", Top);
				rk.SetValue("w", Width);
				rk.SetValue("h", Height);
				rk.SetValue("s", theViewer.Width);

				rk.SetValue("saa", (int)_startUpAction);
				rk.SetValue("path", _path);
			}
			catch//(Exception e)
			{
			}
		}

//		void LoadPrefs()
//		{
//			try
//			{
//				RegistryKey rk = Registry.LocalMachine.CreateSubKey("SOFTWARE\\jBrowse\\Asmex");
//				
//				Left = (int)rk.GetValue("x", 100);
//				Top = (int)rk.GetValue("y", 100);
//				Width = (int)rk.GetValue("w", 600);
//				Height = (int)rk.GetValue("h", 400);
//				theViewer.Width = (int)rk.GetValue("s", 400);
//
//				_startUpAction = (StartUpAction)(int)rk.GetValue("saa", (int)StartUpAction.Restore);
//				_path = (string[])rk.GetValue("path", new string[]{});
//			}
//			catch//(Exception e)
//			{
//			}
//		}

		public ArrayList GetPathAssemblies()
		{
			ArrayList arr = new ArrayList();

			for(int i=0; i < _path.Length; ++i)
			{
				GetDirAssemblies(_path[i], arr);
			}

			return arr;
		}

		void GetDirAssemblies(string dir, ArrayList a)
		{
			Assembly asm;
			string[] files = Directory.GetFiles(dir);

			for(int i=0;i<files.Length;++i)
			{
				asm = null;

				try
				{
					asm = Assembly.LoadFrom(files[i]);
				}
				catch{}
				
				if (asm != null)
				{
					a.Add(asm);
				}
			}

			string[] dirs = Directory.GetDirectories(dir);

			for(int i=0; i< dirs.Length;++i)
			{
				GetDirAssemblies(dirs[i], a);
			}


		}

		private void item_2_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.CheckFileExists= true;
			openFileDialog1.CheckPathExists = true;
			openFileDialog1.Filter ="Exe files (*.exe)|*.exe|Dll files (*.dll)|*.dll|All files (*.*)|*.*";
			openFileDialog1.Multiselect = false;
			openFileDialog1.ReadOnlyChecked = true;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
                AsmView asm = new AsmView(); //GetView(false);
                asm.MdiParent = this;
				asm.AddRoot(BaseNode.MakeNode(openFileDialog1.FileName));
                asm.Show();
			}
		}

		private void item_3_Click(object sender, System.EventArgs e)

			{
				GACPicker p = new GACPicker();
				if (p.ShowDialog() == DialogResult.OK)
				{
                    AsmView asm = new AsmView(); //GetView(false);
                    asm.MdiParent = this;
					asm.AddRoot(BaseNode.MakeNode(p.Assembly));
                    asm.Show();
				}
			}

		private void item_13_Click(object sender, System.EventArgs e)
		{
			FindDialog dlg = new FindDialog(_find);

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				_find = dlg.Find;
				_find.Node = ((AsmView)ActiveMdiChild).SelectedNode;
				((AsmView)ActiveMdiChild).Find(_find);
			}
		}

		private void item_14_Click(object sender, System.EventArgs e)
		{
			if (_find != null)
			{
				_find.Node = ((AsmView)ActiveMdiChild).SelectedNode;
				_find.StartAtRoot = false;
				((AsmView)ActiveMdiChild).Find(_find);
			}
		}

		//		private void mnuAbout_Click(object sender, System.EventArgs e)
		//		{
		//			new AboutDlg().Show();
		//		}

//		private void menuItem11_Click(object sender, System.EventArgs e)
//		{
//			new HintDlg().Show();
//		}


		
	}
	/// <summary>
	/// Encapsulates all the data needed for a find operation.
	/// Also contains the logic for iterating through a tree of BaseNodes and matching a node
	/// What we are looking for affects what nodes we recurse into
	/// </summary>
	internal class FindState
	{
		string _str;
		bool _findTypes;
		bool _findMethods;
		bool _findParameters;
		bool _findFields;
		bool _findResources;
		bool _findProperties;
		bool _findEvents;

		bool _startRoot;
		bool _caseful;
		bool _wholeWord;
		BaseNode _found;
		public FindState(){}

		//You can't match a constructor as it has no name.
		public bool Match(BaseNode node)
		{
			if (node is ModuleNode || node is TableNode || node is RowNode || node is ObjNode ||
				node is FileNode || node is FileRegionNode || node is FolderNode || node is AsmNode || 
				node is AsmRefNode || node is RelationshipNode || node is ConsNode)
				return false;

			if (node is TypeNode && !_findTypes)
				return false;
			if (node is NamespaceNode && !_findTypes)
				return false;
			if (node is MethodNode && !_findMethods)
				return false;
			if (node is ParamNode && !_findParameters)
				return false;
			if (node is FieldNode && !_findFields)
				return false;
			if (node is EventNode && !_findEvents)
				return false;
			if ((node is ResNode || node is ManResNode) && !_findResources)
				return false;
			if (node is PropNode && !_findProperties)
				return false;

			string a, b;
			a = _str;
			b = node.MatchingString;

			if (!_caseful)
			{
				a = a.ToLower();
				b = b.ToLower();
			}

			if (_wholeWord)
			{
				return a == b;
			}

			return (b.IndexOf(a) != -1);
		}

		public string String{get{return _str;} set{_str=value;}}
		public bool FindTypes{get{return _findTypes;} set{_findTypes=value;}}
		public bool FindParameters{get{return _findParameters;} set{_findParameters=value;}}
		public bool FindMethods{get{return _findMethods;} set{_findMethods=value;}}
		public bool FindFields{get{return _findFields;} set{_findFields=value;}}
		public bool FindResources{get{return _findResources;} set{_findResources=value;}}
		public bool FindProperties{get{return _findProperties;} set{_findProperties=value;}}
		public bool FindEvents{get{return _findEvents;} set{_findEvents=value;}}

		public bool WholeWord{get{return _wholeWord;} set{_wholeWord=value;}}
		public bool StartAtRoot{get{return _startRoot;} set{_startRoot=value;}}
		public bool Caseful{get{return _caseful;} set{_caseful=value;}}
		public BaseNode Node{get{return _found;} set{_found=value;}}

		//this actually indicates whether we need to check subnodes -- foldernodes need not have 'generatechildren'
		//called but their children are still checked.
		public bool NeedToExpandNode(BaseNode node)
		{
			if (node is AsmNode || node is NamespaceNode) return true;

			if (node is ModuleNode || node is FileRegionNode || node is TableNode || node is ErrorNode || 
				node is ObjNode || node is AsmRefNode || node is RelationshipNode) return false;


			if (node is FolderNode && node.Parent is AsmNode) return true;

			if (_findProperties || _findEvents || _findFields || _findMethods || _findParameters)
			{
				if (node is TypeNode) return true;
				if (node is FolderNode && node.Parent is TypeNode) return true;
			}

			if (_findParameters)
			{
				if (node is MethodNode || node is ConsNode) return true;
				if (node is FolderNode && (node.Parent is MethodNode || node.Parent is ConsNode) && node.Text.StartsWith("Param")) return true;
			}

			if (_findResources)
			{
				if (node is ManResNode) return true;
			}

			return false;
		}
	}


}

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for SnoopControl.
	/// </summary>
	public class NuGenSnoopControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TabControl tabEventView;
		private System.Windows.Forms.TabPage tabPage1;
		private Genetibase.Debug.HighlightTrackTreeView treeView1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.Button bSetAll;
		private System.Windows.Forms.Button bClearAll;
		private Genetibase.Debug.NuGenPropertyGrid propertyGrid1;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem contextStopTracing;
		private System.Windows.Forms.MenuItem contextClear;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Splitter splitter2;
		
		
		private Panel paneltop_;
		private System.Windows.Forms.Splitter splitter3;
		private Genetibase.Debug.NuGenTraceOutputControl nuGenOInternal1;
		private System.Windows.Forms.CheckBox traceEventCheckbox;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NuGenSnoopControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			nuGenOInternal1.StartLogging();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			nuGenOInternal1.StopLogging();

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabEventView = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.treeView1 = new Genetibase.Debug.HighlightTrackTreeView();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.contextStopTracing = new System.Windows.Forms.MenuItem();
			this.contextClear = new System.Windows.Forms.MenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.traceEventCheckbox = new System.Windows.Forms.CheckBox();
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.bSetAll = new System.Windows.Forms.Button();
			this.bClearAll = new System.Windows.Forms.Button();
			this.paneltop_ = new System.Windows.Forms.Panel();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.propertyGrid1 = new Genetibase.Debug.NuGenPropertyGrid();
			this.splitter3 = new System.Windows.Forms.Splitter();
			nuGenOInternal1 = new NuGenTraceOutputControl();
			this.tabEventView.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.paneltop_.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabEventView
			// 
			this.tabEventView.CausesValidation = false;
			this.tabEventView.Controls.Add(this.tabPage1);
			this.tabEventView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabEventView.Location = new System.Drawing.Point(391, 0);
			this.tabEventView.Name = "tabEventView";
			this.tabEventView.SelectedIndex = 0;
			this.tabEventView.ShowToolTips = true;
			this.tabEventView.Size = new System.Drawing.Size(209, 448);
			this.tabEventView.TabIndex = 12;
			this.tabEventView.SelectedIndexChanged += new System.EventHandler(this.tabEventView_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.CausesValidation = false;
			this.tabPage1.Controls.Add(this.treeView1);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(201, 419);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "All Events";
			// 
			// treeView1
			// 
			this.treeView1.CausesValidation = false;
			this.treeView1.ContextMenu = this.contextMenu;
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.ImageIndex = -1;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.Size = new System.Drawing.Size(201, 419);
			this.treeView1.TabIndex = 4;
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.contextStopTracing,
																						this.contextClear});
			// 
			// contextStopTracing
			// 
			this.contextStopTracing.Index = 0;
			this.contextStopTracing.Text = "Stop tracing this event";
			this.contextStopTracing.Click += new System.EventHandler(this.contextStopTracing_Click);
			// 
			// contextClear
			// 
			this.contextClear.Index = 1;
			this.contextClear.Text = "Clear event window";
			this.contextClear.Click += new System.EventHandler(this.contextClear_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.traceEventCheckbox);
			this.panel1.Controls.Add(this.checkedListBox1);
			this.panel1.Controls.Add(this.bSetAll);
			this.panel1.Controls.Add(this.bClearAll);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(224, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(164, 448);
			this.panel1.TabIndex = 14;
			// 
			// traceEventCheckbox
			// 
			this.traceEventCheckbox.Checked = true;
			this.traceEventCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.traceEventCheckbox.Location = new System.Drawing.Point(16, 384);
			this.traceEventCheckbox.Name = "traceEventCheckbox";
			this.traceEventCheckbox.Size = new System.Drawing.Size(136, 16);
			this.traceEventCheckbox.TabIndex = 11;
			this.traceEventCheckbox.Text = "Trace Events";
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.checkedListBox1.CausesValidation = false;
			this.checkedListBox1.CheckOnClick = true;
			this.checkedListBox1.Location = new System.Drawing.Point(0, 0);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(163, 361);
			this.checkedListBox1.TabIndex = 8;
			this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
			// 
			// bSetAll
			// 
			this.bSetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bSetAll.Location = new System.Drawing.Point(86, 413);
			this.bSetAll.Name = "bSetAll";
			this.bSetAll.Size = new System.Drawing.Size(68, 28);
			this.bSetAll.TabIndex = 10;
			this.bSetAll.Text = "Set All";
			this.bSetAll.Click += new System.EventHandler(this.bSetAll_Click);
			// 
			// bClearAll
			// 
			this.bClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bClearAll.Location = new System.Drawing.Point(10, 413);
			this.bClearAll.Name = "bClearAll";
			this.bClearAll.Size = new System.Drawing.Size(67, 28);
			this.bClearAll.TabIndex = 9;
			this.bClearAll.Text = "Clear All";
			this.bClearAll.Click += new System.EventHandler(this.bClearAll_Click);
			// 
			// paneltop_
			// 
			this.paneltop_.Controls.Add(this.tabEventView);
			this.paneltop_.Controls.Add(this.splitter2);
			this.paneltop_.Controls.Add(this.panel1);
			this.paneltop_.Controls.Add(this.splitter1);
			this.paneltop_.Controls.Add(this.propertyGrid1);
			this.paneltop_.Dock = System.Windows.Forms.DockStyle.Top;
			this.paneltop_.Location = new System.Drawing.Point(0, 0);
			this.paneltop_.Name = "paneltop_";
			this.paneltop_.Size = new System.Drawing.Size(600, 448);
			this.paneltop_.TabIndex = 0;
			// 
			// splitter2
			// 
			this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter2.CausesValidation = false;
			this.splitter2.Location = new System.Drawing.Point(388, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(3, 448);
			this.splitter2.TabIndex = 13;
			this.splitter2.TabStop = false;
			// 
			// splitter1
			// 
			this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter1.CausesValidation = false;
			this.splitter1.Location = new System.Drawing.Point(221, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 448);
			this.splitter1.TabIndex = 12;
			this.splitter1.TabStop = false;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CausesValidation = false;
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Left;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(221, 448);
			this.propertyGrid1.TabIndex = 13;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// splitter3
			// 
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter3.Location = new System.Drawing.Point(0, 448);
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(600, 3);
			this.splitter3.TabIndex = 1;
			this.splitter3.TabStop = false;
			
			nuGenOInternal1.Dock = DockStyle.Fill;
			// 
			// NuGenSnoopControl
			// 
			this.Controls.Add(this.splitter3);
			this.Controls.Add(this.paneltop_);
			this.Controls.Add(nuGenOInternal1);
			this.Name = "NuGenSnoopControl";
			this.Size = new System.Drawing.Size(600, 640);
			this.tabEventView.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.paneltop_.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void checkedListBox1_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			ControlEvent ce = checkedListBox1.SelectedItem as ControlEvent;
			if (ce == null) return;

			ce.TrackEnabled = (e.NewValue == CheckState.Checked);
		}

		private void bClearAll_Click(object sender, System.EventArgs e)
		{
			EventTrackInfo evt = tabEventView.SelectedTab.Tag as EventTrackInfo;

			if (evt == null) return;

			foreach(ControlEvent ce in evt.EventList) 
			{
				ce.TrackEnabled = false;
			}

			UpdateFocusedControl();		
		}

		private void bSetAll_Click(object sender, System.EventArgs e)
		{
			EventTrackInfo evt = tabEventView.SelectedTab.Tag as EventTrackInfo;

			if (evt == null) return;

			foreach(ControlEvent ce in evt.EventList) 
			{
				ce.TrackEnabled = true;
			}

			UpdateFocusedControl();
		}

		private void UpdateFocusedControl() 
		{
			//			TabControl tabControl = sender as TabControl;

			EventTrackInfo evt = tabEventView.SelectedTab.Tag as EventTrackInfo;
			if (evt == null) 
			{
				propertyGrid1.SelectedObject = null;
				checkedListBox1.Items.Clear();
				return;
			}
			propertyGrid1.SelectedObject = evt.SelectedObject;

			checkedListBox1.DisplayMember = "EventName";
			checkedListBox1.Items.Clear();

			// Populate event list with details
			int item = 0;
			foreach(ControlEvent ce in evt.EventList) 
			{
				checkedListBox1.Items.Add(ce, ce.TrackEnabled);
				item++;
			}
		}

		private void tabEventView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateFocusedControl();
		}

		private void contextStopTracing_Click(object sender, System.EventArgs e) 
		{
			HighlightTrackTreeView tv = contextMenu.SourceControl as HighlightTrackTreeView;
			if (tv == null) return;

			TreeNode node = tv.LastRightClickNode;
			if (node == null) return;

			ControlEvent ce = node.Tag as ControlEvent;
			if (ce == null) return;

			if (node != null) 
			{
				ce.TrackEnabled = false;
				UpdateFocusedControl();
			}
		}

		private void contextClear_Click(object sender, System.EventArgs e) 
		{
			TreeView tv = contextMenu.SourceControl as TreeView;
			if (tv == null) return;

			tv.Nodes.Clear();
		}

		public void StopTracking(string eventName) 
		{
			foreach(TabPage tp in tabEventView.Controls) 
			{
				EventTrackInfo evt = tp.Tag as EventTrackInfo;

				if (evt == null) continue;

				foreach(ControlEvent ce in evt.EventList) 
				{
					if (ce.EventName == eventName) 
					{
						ce.TrackEnabled = false;
					}
				}
			}

			UpdateFocusedControl();
		}

		private Control object_;
		public Control SelectedObject
		{
			get {return object_;}
			set 
			{
				object_ = value;

				string name = object_.Name;
				
				if (object_ is Form) 
				{
					name = (object_ as Form).Text;
				}

				if (name.Length == 0)
				{
					name = object_.GetType().Name;
				}

				recursion_counter_ = 0;
				HookEvents(object_, name);
				tabEventView.SelectedIndex = tabEventView.TabCount - 1;
			}
		}

		private bool is_recursive_ = true;
		private int recursion_depth_ = 10;
		private int recursion_counter_ = 0;

		public bool RecurseIntoSubControls
		{
			get {return is_recursive_;}

			set {is_recursive_ = value;}
		}

		public int RecursionDepth
		{
			get {return recursion_depth_;}
			set {recursion_depth_ = value;}
		}

		public void HookEvents(object o, string name) 
		{
			if (recursion_counter_ >= recursion_depth_)
				return;

			recursion_counter_ ++;

			Type t = o.GetType();

			EventTrackInfo trackInfo = AddTabPage(name, o);
			ArrayList controlEventList = new ArrayList();

			foreach(EventInfo ei in t.GetEvents()) 
			{
				// Discover type of event handler
				Type eventHandlerType = ei.EventHandlerType;

				// eventHandlerType is the type of the delegate (eg System.EventHandler)
				// what we need, is to find the type of the second parameter of the
				// delegate, eg System.EventArgs

				MethodInfo mi = eventHandlerType.GetMethod("Invoke");
				ParameterInfo[] pi = mi.GetParameters();
				
				if (pi == null || pi.Length < 2)
					continue;

				// Get a class derived from ControlEvent which has a HandleEvent method
				// taking the appropriate parameters
				ControlEvent ce = GenerateEventAssembly.Instance.GetEventConsumerType(pi[1].ParameterType);

				// Hook onto this control event to get the details of all events fired
				ce.ControlName = name;
				ce.EventName = ei.Name;
				ce.EventTrackInfo = trackInfo;
				ce.EventFired += new EventHandler(eventFired);

				controlEventList.Add(ce);

				// Wire up the event handler to our new consumer
				Delegate d = Delegate.CreateDelegate(eventHandlerType, ce, "HandleEvent");
				ei.AddEventHandler(o, d);
			}

			trackInfo.EventList = controlEventList;

			if (is_recursive_)
			{
				if (o is Control) 
				{
					Control c = (Control) o;

					if (c.Controls != null) 
					{
						foreach(Control subControl in c.Controls) 
						{
							HookEvents(subControl, name + "/" + ControlName(subControl));					
						}
					}

					if (c is Form) 
					{
						Form f = (Form) c;
						if (f.Menu != null) 
						{
							foreach(MenuItem m in f.Menu.MenuItems) 
							{
								HookEvents(m, name + "/" + ControlName(m));
							}
						}
					}

					if (c.ContextMenu != null) 
					{
						foreach(MenuItem m in c.ContextMenu.MenuItems) 
						{
							HookEvents(m, name + "/" + ControlName(m));
						}
					}

				}

				if (o is MenuItem) 
				{
					MenuItem m = (MenuItem) o;
					foreach(MenuItem mi in m.MenuItems) 
					{
						HookEvents(mi, name + "/" + ControlName(mi));
					}
				}
			}

			recursion_counter_ --;
		}

		string ControlName(object o) 
		{
			if (o is Control) 
			{
				Control c = (Control) o;
				if (c.Name.Length == 0) 
				{
					if (c.Text.Length == 0) 
					{
						return c.GetType().Name;
					}
					return c.Text;
				}
				return c.Name;
			}

			if (o is MenuItem) 
			{
				MenuItem m = (MenuItem) o;
				if (m.Text.Length == 0) 
				{
					return m.GetType().Name;
				}
				return m.Text;
			}

			return o.GetType().Name;
		}

		private bool is_tracking_enabled_ = true;
		private bool is_scrolling_enabled_ = true;

		public bool TrackingEnabled
		{
			get {return is_tracking_enabled_;}
			set {is_tracking_enabled_ = value;}
		}

		public bool ScrollingEnabled
		{
			get {return is_scrolling_enabled_;}
			set {is_scrolling_enabled_ = value;}
		}

		/// <summary>
		/// Process generic inbound events and populate them to the appropriate tab page(s)
		/// </summary>
		private void eventFired(object sender, EventArgs e) 
		{
			ControlEvent ce = sender as ControlEvent;
			if (ce == null) return;

			if (this.TrackingEnabled) 
			{
				if (ce.TrackEnabled) 
				{
					if (traceEventCheckbox.Checked)
					{
						Trace.WriteLine(String.Format("hIgHlItE{0}::{1}", ce.ControlName, ce.EventName));
					}
					
					AddEventsToTreeView(ce, treeView1, true);
					AddEventsToTreeView(ce, ce.EventTrackInfo.TreeView, false);
				}
			}
		}

		protected long startTicks = DateTime.Now.Ticks;
		void AddEventsToTreeView(ControlEvent ce, TreeView treeView, bool includeControlName) 
		{
			string eventName;
			if (includeControlName) 
			{
				eventName = String.Format("{0}::{1}", ce.ControlName, ce.EventName);
			} 
			else 
			{
				eventName = ce.EventName;
			}

			TreeNode node = new TreeNode(String.Format("{0} {1}",
				DateTime.Now.Ticks - startTicks, eventName));
			node.Tag = ce;
			treeView.Nodes.Add(node);

			TreeNode eventArgsNode = new TreeNode(ce.EventArgs.GetType().FullName);
			eventArgsNode.Tag = ce;
			node.Nodes.Add(eventArgsNode);

			// Special case: save reflection overhead if this is an EventArgs
			// as this contains no values

			if (ce.EventArgs.GetType() != typeof(EventArgs)) 
			{
			
				Type eventArgsType = ce.EventArgs.GetType();

				// Iterate through properties and add details of each to the tree view.
				foreach(PropertyInfo member in eventArgsType.GetProperties()) 
				{
					try 
					{
						TreeNode newNode = new TreeNode(
							String.Format("[prop] {0} = {1}", member.Name, 
							eventArgsType.GetProperty(member.Name).GetValue(ce.EventArgs, null).ToString()));
						newNode.Tag = ce;
						eventArgsNode.Nodes.Add(newNode);			
					}
					catch (Exception) { }
				}
				// Iterate through public types
				foreach(MemberInfo type in eventArgsType.GetMembers()) 
				{
					try 
					{
						if (type.MemberType == MemberTypes.Field) 
						{
							TreeNode newNode = new TreeNode(
								String.Format("[field] {0} = {1}", type.Name, 
								eventArgsType.GetField(type.Name).GetValue(ce.EventArgs).ToString()));
							newNode.Tag = ce;
							eventArgsNode.Nodes.Add(newNode);
						}
					}
					catch (Exception) { }
				}

			}

			if (this.ScrollingEnabled)
				treeView.SelectedNode = node;
		}

		private EventTrackInfo AddTabPage(string name, object selectedObject) 
		{
			TabPage newPage = new TabPage();

			newPage.Text = name;
			newPage.ToolTipText = selectedObject.GetType().FullName ;
			newPage.CausesValidation = false;

			EventTrackInfo evtTrackInfo = new EventTrackInfo();
			evtTrackInfo.CausesValidation = false;
			evtTrackInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			evtTrackInfo.ContextMenu = contextMenu;
			evtTrackInfo.SelectedObject = selectedObject;
			newPage.Tag = evtTrackInfo;
			newPage.Controls.Add(evtTrackInfo);

			tabEventView.Controls.Add(newPage);

			return evtTrackInfo;
		}
	}
}

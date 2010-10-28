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
using System.Reflection;
using System.IO;
using System.Resources;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace Genetibase.Debug
{

	/// <summary>
	/// A single mdichild view.  It contains a tree of BaseNodes.
	/// </summary>
	internal class AsmView : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TreeView tvAsm;
		private System.ComponentModel.IContainer components;


		internal AsmView()
		{
			InitializeComponent();

			tvAsm.ImageList = MainFrame.ilObjTree;

			this.Text = "Empty View";
		}


		internal  AsmView(BaseNode node)
		{
			InitializeComponent();

			tvAsm.ImageList = MainFrame.ilObjTree;

			AddRoot(node);

		}

		internal void AddRoot(BaseNode node)
		{
			BaseNode newnode = (BaseNode)node.Clone();

			tvAsm.Nodes.Add(newnode);

			Text = node.ToString();

		}


		internal BaseNode[] Roots
		{
			get
			{
				BaseNode[] roots = new BaseNode[tvAsm.Nodes.Count];
				tvAsm.Nodes.CopyTo(roots, 0);
				return roots;
			}

		}
		

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AsmView));
            this.tvAsm = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tvAsm
            // 
            this.tvAsm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tvAsm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvAsm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvAsm.Location = new System.Drawing.Point(0, 0);
            this.tvAsm.Name = "tvAsm";
            this.tvAsm.Size = new System.Drawing.Size(692, 466);
            this.tvAsm.TabIndex = 0;
            this.tvAsm.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.BeforeExpand);
            this.tvAsm.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelect);
            this.tvAsm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp);
            this.tvAsm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
            // 
            // AsmView
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(692, 466);
            this.Controls.Add(this.tvAsm);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(4, 4);
            this.Name = "AsmView";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AsmView_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.AsmView_DragEnter);
            this.ResumeLayout(false);

		}
		#endregion

		internal void Find(FindState  f)
		{
			BaseNode bn = null;

			if (tvAsm.Nodes[0] == null)
				return;

			//try and get successor of node given in f
			if ((f.StartAtRoot == false) && (f.Node != null) && (f.Node.TreeView == tvAsm))
			{
				bn = GetSuccessorNode(f.Node, f);
			}

			//...or else just use root
			if (bn == null)
			{
				bn = (BaseNode)tvAsm.Nodes[0];
			}

			//...iterate thru nodez.
			while(bn != null)
			{
				if(f.Match(bn))
					break;

				bn = GetSuccessorNode(bn, f);//we don't descend because RecFind descends
			}

			
			if (bn != null)
			{
				f.Node = bn;
				tvAsm.SelectedNode = bn;
				bn.EnsureVisible();
			}
			else
			{
				MessageBox.Show("Cannot Locate Find Criteria");
			}
		}

		internal BaseNode GetSuccessorNode(BaseNode bn, FindState f)
		{
			BaseNode succ = bn;

			//maybe, check to find a child 'successor'
			if (f.NeedToExpandNode(bn))
			{
				bn.GenerateChildren();
				if (bn.Nodes.Count != 0)
				{
					return (BaseNode)bn.Nodes[0];
				}
			}

			//make sure there is a sib, going up tree if necessary
			while(succ.NextNode == null)
			{
				//give up if we reach top.
				if (succ.Parent == null)
					return null;

				succ = (BaseNode)succ.Parent;
			}

			succ = (BaseNode)succ.NextNode;
			
			return succ;

		}


		internal BaseNode SelectedNode
		{
			get{return (BaseNode)tvAsm.SelectedNode;}
		}
/*
		private BaseNode RecFind(FindState f, BaseNode bn)
		{
			if (f.Match(bn))
				return bn;

			if (f.NeedToExpandNode(bn))
			{
				bn.GenerateChildren();

				for(int i=0; i<bn.Nodes.Count; ++i)
				{
					BaseNode found = RecFind(f, (BaseNode)bn.Nodes[i]);
					if (found != null)
						return found;
				}
			}


			return null;

		}
*/

		private void OnNewView(object sender, System.EventArgs e)
		{
			BaseNode an = (BaseNode)tvAsm.SelectedNode;

			AsmView newView = new AsmView(an);
			newView.MdiParent = MdiParent;
			newView.Visible = true;
		}

		private void OnNewRoot(object sender, System.EventArgs e)
		{
			BaseNode an = (BaseNode)tvAsm.SelectedNode;

			BaseNode n = (BaseNode)an.Clone();

			tvAsm.Nodes.Add(n);
		}

		private void OnRemoveRoot(object sender, System.EventArgs e)
		{
			BaseNode an = (BaseNode)tvAsm.SelectedNode;

			tvAsm.Nodes.Remove(an);
		}

		private new void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				tvAsm.SelectedNode = tvAsm.GetNodeAt(e.X, e.Y);
			}
		}

		private void BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			BaseNode an = (BaseNode)e.Node;

			if (an.Spinster)
			{
				an.GenerateChildren();
			}

		}

		private void AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			BaseNode an = (BaseNode)e.Node;

			if (an.Uninteresting)
			{
				((MainFrame)MdiParent).theViewer.ReadNode(null,null,null);
			}
			else
			{
				((MainFrame)MdiParent).theViewer.ReadNode(an.Data, an.NodeLabel, an.Desc);
			}

			Text = an.Text;
		
		}

		private new void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;
			
			BaseNode an = (BaseNode)tvAsm.GetNodeAt(e.X, e.Y);

			if (an==null) return;

			ContextMenu cmnu = new ContextMenu();

			if (an.Parent == null)
			{
				MenuItem it = new MenuItem("Remove This Root", new EventHandler(this.OnRemoveRoot));
				cmnu.MenuItems.Add(it);
			}
			else
			{
				if (an.CanBeRoot)
				{
					MenuItem it = new MenuItem("New View Rooted On This Node", new EventHandler(this.OnNewView));
					cmnu.MenuItems.Add(it);
					it = new MenuItem("Add To This View As Root", new EventHandler(this.OnNewRoot));
					cmnu.MenuItems.Add(it);
				}
			}

			MenuItem[] items = an.GetMenu();

			if (items != null)
			{
				cmnu.MenuItems.AddRange(items);
			}

			if (cmnu.MenuItems.Count > 0)
				cmnu.Show(this, new Point(e.X, e.Y));
			
		
		}

		internal void OnActivate()
		{
			BaseNode an = (BaseNode)tvAsm.SelectedNode;

			if (an==null || an.Uninteresting)
			{
				((MainFrame)MdiParent).theViewer.ReadNode(null,null,null);
			}
			else
			{
				((MainFrame)MdiParent).theViewer.ReadNode(an.Data, an.NodeLabel, an.Desc);
			}
		
		
		}


		private void AsmView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			object o  = e.Data.GetData(DataFormats.FileDrop);
			if (o == null)
			{
				MessageBox.Show("Drag a file from Explorer to open it");
				return;
			}

			Array a = (Array)o;

			for(int i=0;i<a.Length;++i)
			{
				BaseNode b = BaseNode.MakeNode(a.GetValue(i));
				if (b != null)
					AddRoot(b);
			}
		}

		private void AsmView_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
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


	}
}

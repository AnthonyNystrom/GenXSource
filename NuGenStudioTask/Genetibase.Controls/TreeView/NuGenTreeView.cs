/* -----------------------------------------------
 * NuGenTreeView.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Collections;
using Genetibase.Controls.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// Represents a mixed <see cref="T:System.Windows.Forms.TreeView"/>.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(true)]
	public partial class NuGenTreeView : TreeView
	{
		#region Declarations.Fields

		private Point _OldDragLineLeft = Point.Empty;
		private Point _OldDragLineRight = Point.Empty;

		#endregion

		#region Properties.Public

		/*
		 * Nodes
		 */

		private NuGenTreeNodeCollection _Nodes = null;

		/// <summary>
		/// Gets the collection of nodes contained within this <see cref="T:NuGenTreeView"/>.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new NuGenTreeNodeCollection Nodes
		{
			get
			{
				if (_Nodes == null)
				{
					_Nodes = new NuGenTreeNodeCollection();

					_Nodes.ClearNodesRequested += delegate(object sender, NuGenItemsClearRequestedEventArgs e)
					{
						base.Nodes.Clear();
					};

					_Nodes.ContainsNodeRequested += delegate(object sender, NuGenContainsItemRequestedEventArgs e)
					{
						e.ContainsNode = base.Nodes.Contains(e.NodeToCheck);
					};

					_Nodes.EnumeratorRequested += delegate(object sender, NuGenEnumeratorRequestedEventArgs e)
					{
						e.RequestedEnumerator = base.Nodes.GetEnumerator();
					};

					_Nodes.NodeAdded += delegate(object sender, NuGenAddTreeNodeEventArgs e)
					{
						e.TreeNodeIndex = base.Nodes.Add(e.TreeNodeToAdd);
					};

					_Nodes.NodeRangeAdded += delegate(object sender, NuGenAddTreeNodeRangeEventArgs e)
					{
						base.Nodes.AddRange(e.TreeNodeRangeToAdd);
					};

					_Nodes.NodeInserted += delegate(object sender, NuGenAddTreeNodeEventArgs e)
					{
						base.Nodes.Insert(e.TreeNodeIndex, e.TreeNodeToAdd);
					};

					_Nodes.NodeRemoved += delegate(object sender, NuGenRemoveTreeNodeEventArgs e)
					{
						base.Nodes.Remove(e.TreeNodeToRemove);
					};

					_Nodes.NodeByIndexAdjusted += delegate(object sender, NuGenIndexedTreeNodeEventArgs e)
					{
						base.Nodes[e.TreeNodeIndex] = e.TreeNode;
					};

					_Nodes.NodeByIndexRequested += delegate(object sender, NuGenIndexedTreeNodeEventArgs e)
					{
						TreeNode treeNode = base.Nodes[e.TreeNodeIndex];

						if (treeNode is NuGenTreeNode)
						{
							e.TreeNode = (NuGenTreeNode)treeNode;
						}
						else
						{
							throw new InvalidCastException(Resources.InvalidCast_NodeType);
						}
					};

					_Nodes.NodeCountRequested += delegate(object sender, NuGenItemsCountRequestedEventArgs e)
					{
						e.Count = base.Nodes.Count;
					};
				}

				return _Nodes;
			}
		}

		/*
		 * SelectedNodes
		 */

		List<NuGenTreeNode> _SelectedNodes = new List<NuGenTreeNode>();

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<NuGenTreeNode> SelectedNodes
		{
			get
			{
				_SelectedNodes.Clear();

				if (this.SelectedNode != null && this.SelectedNode is NuGenTreeNode)
				{
					_SelectedNodes.Add((NuGenTreeNode)this.SelectedNode);
				}

				return _SelectedNodes;

				/* TURNED OFF: Debug.Assert(this.SelectionService != null, "this.SelectionService != null");
				 * return this.SelectionService.SelectedNodes; */
			}
		}

		/*
		 * TopNode
		 */

		/// <summary>
		/// Gets the first fully-visible tree node in the tree view control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode"></see> that represents the first fully-visible tree node in the tree view control.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new NuGenTreeNode TopNode
		{
			get
			{
				TreeNode topNode = base.TopNode;

				if (topNode is NuGenTreeNode)
				{
					return (NuGenTreeNode)topNode;
				}
				else if (topNode != null)
				{
					throw new InvalidCastException(Resources.InvalidCast_NodeType);
				}

				return null;
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * DragDropService
		 */

		private INuGenTreeViewDragDropService _DragDropService = null;

		/// <summary>
		/// </summary>
		protected INuGenTreeViewDragDropService DragDropService
		{
			get
			{
				if (_DragDropService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_DragDropService = this.ServiceProvider.GetService<INuGenTreeViewDragDropService>();

					if (_DragDropService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTreeViewDragDropService>();
					}
				}

				return _DragDropService;
			}
		}

		/*
		 * IsDragging
		 */

		private bool _IsDragging = false;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenTreeView"/> is currently dragging
		/// node(s).
		/// </summary>
		protected bool IsDragging
		{
			get
			{
				return _IsDragging;
			}
			set
			{
				if (_IsDragging != value)
				{
					_IsDragging = value;

					_OldDragLineLeft = Point.Empty;
					_OldDragLineRight = Point.Empty;

					this.Invalidate(false);
				}
			}
		}

		/*
		 * SelectionService
		 */

		private INuGenTreeViewSelectionService _SelectionService = null;

		/// <summary>
		/// </summary>
		protected INuGenTreeViewSelectionService SelectionService
		{
			get
			{
				if (_SelectionService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_SelectionService = this.ServiceProvider.GetService<INuGenTreeViewSelectionService>();

					if (_SelectionService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTreeViewSelectionService>();
					}
				}

				return _SelectionService;
			}
		}

		/*
		 * TreeNodeSorter
		 */

		private INuGenTreeNodeSorter _TreeNodeSorter = null;

		/// <summary>
		/// </summary>
		protected INuGenTreeNodeSorter TreeNodeSorter
		{
			get
			{
				if (_TreeNodeSorter == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_TreeNodeSorter = this.ServiceProvider.GetService<INuGenTreeNodeSorter>();

					if (_TreeNodeSorter == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTreeNodeSorter>();
					}
				}

				return _TreeNodeSorter;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _ServiceProvider = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _ServiceProvider;
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		/*
		 * CreateParams
		 */

		/// <summary>
		/// Gets the modified window styles to get rid of the horizontal scroll bar.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.Style |= WinUser.WS_HSCROLL;
				return cp;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * GetNodeAt
		 */

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public new NuGenTreeNode GetNodeAt(Point pointToRetrieveFrom)
		{
			TreeNode node = base.GetNodeAt(pointToRetrieveFrom);

			if (node is NuGenTreeNode)
			{
				return (NuGenTreeNode)node;
			}
			else if (node != null)
			{
				throw new InvalidCastException(Resources.InvalidCast_NodeType);
			}

			return null;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public new NuGenTreeNode GetNodeAt(int x, int y)
		{
			return this.GetNodeAt(new Point(x, y));
		}

		/*
		 * SelectAllNodes
		 */

		/// <summary>
		/// </summary>
		public void SelectAllNodes()
		{
			Debug.Assert(this.SelectionService != null, "this.SelectionService != null");
			this.SelectionService.SelectAllNodes(this.Nodes);
		}

		/*
		 * Sort
		 */

		/// <summary>
		/// Do not use standard <see cref="M:Sort"/> method due to significant overhead.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="comparerToUse"/> is <see langword="null"/>.
		/// </exception>
		public void Sort<T>(IComparer<T> comparerToUse) where T : NuGenTreeNode
		{
			if (comparerToUse == null)
			{
				throw new ArgumentNullException("comparerToUse");
			}

			Debug.Assert(this.TreeNodeSorter != null, "this.TreeNodeSorter != null");
			this.TreeNodeSorter.Sort<T>(this.Nodes, comparerToUse);
		}

		#endregion

		#region Methods.Protected.Overriden.DragDrop

		/*
		 * OnDragDrop
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DragDrop"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			this.IsDragging = false;
		}

		/*
		 * OnDragEnter
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DragEnter"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			this.IsDragging = true;
		}

		/*
		 * OnDragLeave
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DragLeave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			this.IsDragging = false;
		}

		#endregion

		#region Methods.Protected.Overriden.Input

		/*
		 * OnMouseDown
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.SelectedNode = this.GetNodeAt(this.PointToClient(Cursor.Position));
		}

		#endregion

		#region Methods.Protected.Overriden.NodeOperations

		/*
		 * OnAfterCollapse
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.TreeView.AfterCollapse"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs"></see> that contains the event data.</param>
		protected override void OnAfterCollapse(TreeViewEventArgs e)
		{
			base.OnAfterCollapse(e);

			if (e.Node is NuGenTreeNode)
			{
				NuGenTreeNode treeNode = (NuGenTreeNode)e.Node;

				treeNode.ImageIndex = treeNode.DefaultImageIndex;
				treeNode.SelectedImageIndex = treeNode.DefaultImageIndex;
			}
		}

		/*
		 * OnAfterExpand
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.TreeView.AfterExpand"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs"></see> that contains the event data.</param>
		protected override void OnAfterExpand(TreeViewEventArgs e)
		{
			base.OnAfterExpand(e);

			if (e.Node is NuGenTreeNode)
			{
				NuGenTreeNode treeNode = (NuGenTreeNode)e.Node;

				treeNode.ImageIndex = treeNode.ExpandedImageIndex;
				treeNode.SelectedImageIndex = treeNode.ExpandedImageIndex;
			}
		}

		#endregion

		#region Methods.Protected.Overriden.WndMsgProcess

		/*
		 * WndProc
		 */

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">Specifies the message to process.</param>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WinUser.WM_USER + 7246:
				{
					this.WmUser7246(ref m);
					return;
				}
				case WinUser.WM_ERASEBKGND:
				{
					this.WmEraseBkGnd(ref m);
					return;
				}
				case WinUser.WM_NCHITTEST:
				{
					this.WmNcHitTest(ref m);
					return;
				}
				default:
				{
					base.WndProc(ref m);
					return;
				}
			}
		}

		#endregion

		#region Methods.Protected.Virtual.WinMsgProcess

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		protected virtual void WmEraseBkGnd(ref Message m)
		{
			return;
		}

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		protected virtual void WmNcHitTest(ref Message m)
		{
			if (_IsDragging)
			{
				Point clientCursorPosition = this.PointToClient(Cursor.Position);
				NuGenTreeNode targetTreeNode = this.GetNodeAt(clientCursorPosition);
				Rectangle targetBounds = Rectangle.Empty;

				NuGenDropPosition dropPosition = this.DragDropService.GetDropPosition(
					targetTreeNode,
					clientCursorPosition
				);

				if (dropPosition != NuGenDropPosition.Nowhere)
				{
					targetBounds = targetTreeNode.Bounds;
				}

				Point dragLineLeft = Point.Empty;
				Point dragLineRight = Point.Empty;

				using (Graphics g = Graphics.FromHwnd(this.Handle))
				{
					NuGenControlPaint.DrawReversibleLine(g, _OldDragLineLeft, _OldDragLineRight);

					switch (dropPosition)
					{
						case NuGenDropPosition.After:
						{
							dragLineLeft = new Point(this.Left, targetBounds.Bottom);
							dragLineRight = new Point(this.Right, targetBounds.Bottom);

							NuGenControlPaint.DrawReversibleLine(g, dragLineLeft, dragLineRight);

							break;
						}
						case NuGenDropPosition.Before:
						{
							dragLineLeft = new Point(this.Left, targetBounds.Top);
							dragLineRight = new Point(this.Right, targetBounds.Top);

							NuGenControlPaint.DrawReversibleLine(g, dragLineLeft, dragLineRight);

							break;
						}
					}
				}

				_OldDragLineLeft = dragLineLeft;
				_OldDragLineRight = dragLineRight;
			}

			base.WndProc(ref m);
		}

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		protected virtual void WmUser7246(ref Message m)
		{
			NMHDR nmHeader = (NMHDR)m.GetLParam(typeof(NMHDR));

			if (nmHeader.code == CommCtrl.NM_CUSTOMDRAW)
			{
				NMTVCUSTOMDRAW tvDraw = (NMTVCUSTOMDRAW)m.GetLParam(typeof(NMTVCUSTOMDRAW));

				if (CommCtrl.CDDS_PREPAINT == tvDraw.nmcd.dwDrawStage)
				{
					m.Result = new IntPtr(CommCtrl.CDRF_NOTIFYITEMDRAW);
				}
				else if (CommCtrl.CDDS_ITEMPREPAINT == tvDraw.nmcd.dwDrawStage)
				{
					this.TreeViewCustomDraw(ref m, tvDraw);
				}
			}

			base.WndProc(ref m);
		}

		#endregion

		#region Methods.Private

		private void TreeViewCustomDraw(ref Message msg, NMTVCUSTOMDRAW tvDraw)
		{
			try
			{
				int hTreeNode = tvDraw.nmcd.dwItemSpec;

				if (hTreeNode == 0)
				{
					msg.Result = new IntPtr(CommCtrl.CDRF_DODEFAULT);
					return;
				}

				NuGenTreeNode curNode = NuGenTreeNode.FromHandle(this, new IntPtr(hTreeNode));

				if (curNode == null)
				{
					msg.Result = new IntPtr(CommCtrl.CDRF_DODEFAULT);
					return;
				}

				TreeView tree = curNode.TreeView;

				if (tree != null)
				{
					if (!curNode.HasCheckBox)
					{
						this.UncheckNode(curNode.TreeView.Handle, curNode.Handle.ToInt32());
					}
				}
			}
			finally
			{
				msg.Result = new IntPtr(CommCtrl.CDRF_DODEFAULT);
			}
		}

		private void UncheckNode(IntPtr handle, int hItem)
		{
			if (hItem > 0)
			{
				TVITEM tvi = new TVITEM();

				tvi.mask = (uint)CommCtrl.TVIF_HANDLE | (uint)CommCtrl.TVIF_STATE;
				tvi.hItem = new IntPtr(hItem);
				tvi.stateMask = (uint)CommCtrl.TVIS_STATEIMAGEMASK;
				tvi.state = 0;

				User32.SendMessage(handle, CommCtrl.TVM_SETITEM, IntPtr.Zero, ref tvi);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeView"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Required:<para/>
		/// <see cref="INuGenTreeViewDragDropService"/><para/>
		/// <see cref="INuGenTreeViewSelectionService"/><para/>
		/// <see cref="INuGenTreeNodeSorter"/><para/>
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </exception>
		public NuGenTreeView(
			INuGenServiceProvider serviceProvider
			)
		{
			_ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeView"/> class.
		/// </summary>
		public NuGenTreeView()
			: this(new NuGenTreeViewServiceProvider())
		{
		}

		#endregion
	}
}

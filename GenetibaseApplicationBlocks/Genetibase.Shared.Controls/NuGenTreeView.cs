/* -----------------------------------------------
 * NuGenTreeView.cs
 * Copyright © 2006-2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.Collections;
using Genetibase.Shared.Controls.Design;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Controls.TreeViewInternals;
using Genetibase.Shared.Drawing;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a mixed <see cref="T:System.Windows.Forms.TreeView"/>.
	/// </summary>
	[Designer(typeof(NuGenTreeViewDesigner))]
	[ToolboxItem(true)]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenTreeView : TreeView
	{
		#region Declarations.Fields

		private Point _oldDragLineLeft = Point.Empty;
		private Point _oldDragLineRight = Point.Empty;

		#endregion

		#region Properties.Public

		/*
		 * Nodes
		 */

		/// <summary>
		/// Gets the collection of tree nodes that are assigned to the tree view control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNodeCollection"></see> that represents the tree nodes assigned to the tree view control.</returns>
		[Editor(typeof(NuGenTreeNodeCollectionEditor), typeof(UITypeEditor))]
		public new TreeNodeCollection Nodes
		{
			get
			{
				return base.Nodes;
			}
		}

		#endregion

		#region Properties.Hidden

		/*
		 * SelectedImageKey
		 */

		/// <summary>
		/// Gets or sets the key of the default image shown when a <see cref="T:System.Windows.Forms.TreeNode"></see> is in a selected state.
		/// </summary>
		/// <value></value>
		/// <returns>The key of the default image shown when a <see cref="T:System.Windows.Forms.TreeNode"></see> is in a selected state.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string SelectedImageKey
		{
			get
			{
				return base.SelectedImageKey;
			}
			set
			{
				base.SelectedImageKey = value;
			}
		}

		/*
		 * SelectedImageIndex
		 */

		/// <summary>
		/// Gets or sets the image list index value of the image that is displayed when a tree node is selected.
		/// </summary>
		/// <value></value>
		/// <returns>A zero-based index value that represents the position of an <see cref="T:System.Drawing.Image"></see> in an <see cref="T:System.Windows.Forms.ImageList"></see>.</returns>
		/// <exception cref="T:System.ArgumentException">The index assigned value is less than zero. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int SelectedImageIndex
		{
			get
			{
				return base.SelectedImageIndex;
			}
			set
			{
				base.SelectedImageIndex = value;
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * IsDragging
		 */

		private bool _isDragging = false;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenTreeView"/> is currently dragging
		/// node(s).
		/// </summary>
		protected bool IsDragging
		{
			get
			{
				return _isDragging;
			}
			set
			{
				if (_isDragging != value)
				{
					_isDragging = value;

					_oldDragLineLeft = Point.Empty;
					_oldDragLineRight = Point.Empty;

					this.Invalidate(false);
				}
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * DragDropService
		 */

		private INuGenTreeViewDragDropService _dragDropService = null;

		/// <summary>
		/// </summary>
		protected INuGenTreeViewDragDropService DragDropService
		{
			get
			{
				if (_dragDropService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_dragDropService = this.ServiceProvider.GetService<INuGenTreeViewDragDropService>();

					if (_dragDropService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTreeViewDragDropService>();
					}
				}

				return _dragDropService;
			}
		}

		

		/*
		 * TreeNodeSorter
		 */

		private INuGenTreeNodeSorter _treeNodeSorter = null;

		/// <summary>
		/// </summary>
		protected INuGenTreeNodeSorter TreeNodeSorter
		{
			get
			{
				if (_treeNodeSorter == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_treeNodeSorter = this.ServiceProvider.GetService<INuGenTreeNodeSorter>();

					if (_treeNodeSorter == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTreeNodeSorter>();
					}
				}

				return _treeNodeSorter;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

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
		 * Sort
		 */

		/// <summary>
		/// Do not use standard <see cref="M:Sort"/> method due to significant overhead.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="comparerToUse"/> is <see langword="null"/>.
		/// </exception>
		public void Sort<T>(IComparer<T> comparerToUse) where T : TreeNode
		{
			if (comparerToUse == null)
			{
				throw new ArgumentNullException("comparerToUse");
			}

			Debug.Assert(this.TreeNodeSorter != null, "this.TreeNodeSorter != null");
			this.TreeNodeSorter.Sort<T>(this.Nodes, comparerToUse);
		}

		#endregion

		#region Methods.Protected.Overridden.DragDrop

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

		#region Methods.Protected.Overridden.Input

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

		#region Methods.Protected.Overridden.NodeOperations

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

		#region Methods.Protected.Overridden.WndMsgProcess

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
			if (_isDragging)
			{
				Point clientCursorPosition = this.PointToClient(Cursor.Position);
				TreeNode targetTreeNode = this.GetNodeAt(clientCursorPosition);
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
					NuGenControlPaint.DrawReversibleLine(g, _oldDragLineLeft, _oldDragLineRight);

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

				_oldDragLineLeft = dragLineLeft;
				_oldDragLineRight = dragLineRight;
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

				NuGenTreeNode curNode = NuGenTreeNode.FromHandle(this, new IntPtr(hTreeNode)) as NuGenTreeNode;

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
		public NuGenTreeView()
			: this(NuGenServiceManager.TreeViewServiceProvider)
		{
		}

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
		public NuGenTreeView(INuGenServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		#endregion
	}
}

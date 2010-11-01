/* -----------------------------------------------
 * NuGenTreeNode.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Collections;
using Genetibase.Controls.Properties;
using Genetibase.Shared;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// Represents a node of a <see cref="T:NuGenTreeView"/>.
	/// </summary>
	public partial class NuGenTreeNode : TreeNode
	{
		#region Properties.Public

		/*
		 * DefaultImageIndex
		 */

		private int defaultImageIndex = -1;

		/// <summary>
		/// Gets or sets the index of the image that is used when the node is in its default state (collapsed).
		/// </summary>
		public int DefaultImageIndex
		{
			get
			{
				return this.defaultImageIndex;
			}
			set
			{
				if (this.defaultImageIndex != value)
				{
					this.defaultImageIndex = value;

					if (!this.IsExpanded)
					{
						this.ImageIndex = this.defaultImageIndex;
						this.SelectedImageIndex = this.defaultImageIndex;
					}
				}
			}
		}

		/*
		 * ExpandedImageIndex
		 */

		private int expandedImageIndex = -1;

		/// <summary>
		/// Gets or sets the index of the image that is used when the node is expanded.
		/// </summary>
		public int ExpandedImageIndex
		{
			get
			{
				return this.expandedImageIndex;
			}
			set
			{
				if (this.expandedImageIndex != value)
				{
					this.expandedImageIndex = value;

					if (this.IsExpanded)
					{
						this.ImageIndex = this.expandedImageIndex;
						this.SelectedImageIndex = this.expandedImageIndex;
					}
				}
			}
		}

		/*
		 * FirstNode
		 */

		/// <summary>
		/// Gets the first child tree node in the tree node collection.
		/// </summary>
		/// <value></value>
		/// <returns>The first child <see cref="T:System.Windows.Forms.TreeNode"></see> in the <see cref="P:System.Windows.Forms.TreeNode.Nodes"></see> collection.</returns>
		public new NuGenTreeNode FirstNode
		{
			get
			{
				TreeNode firstChildNode = base.FirstNode;

				if (firstChildNode is NuGenTreeNode)
				{
					return (NuGenTreeNode)firstChildNode;
				}
				else if (firstChildNode != null)
				{
					throw new InvalidCastException(Resources.InvalidCast_NodeType);
				}

				return null;
			}
		}

		/*
		 * HasCheckBox
		 */

		private bool hasCheckBox = true;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenTreeNode"/> has a check box.
		/// Default value is <see langword="true"/>.
		/// </summary>
		public bool HasCheckBox
		{
			get
			{
				return this.hasCheckBox;
			}
			set
			{
				this.hasCheckBox = value;
			}
		}

		/*
		 * LastNode
		 */

		/// <summary>
		/// Gets the last child tree node.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode"></see> that represents the last child tree node.</returns>
		public new NuGenTreeNode LastNode
		{
			get
			{
				TreeNode lastNode = base.LastNode;

				if (lastNode is NuGenTreeNode)
				{
					return (NuGenTreeNode)lastNode;
				}
				else if (lastNode != null)
				{
					throw new InvalidCastException(Resources.InvalidCast_NodeType);
				}

				return null;
			}
		}

		/*
		 * NextNode
		 */

		/// <summary>
		/// Gets the next sibling tree node.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode"></see> that represents the next sibling tree node.</returns>
		public new NuGenTreeNode NextNode
		{
			get
			{
				TreeNode nextNode = base.NextNode;

				if (nextNode is NuGenTreeNode)
				{
					return (NuGenTreeNode)nextNode;
				}
				else if (nextNode != null)
				{
					throw new InvalidCastException(Resources.InvalidCast_NodeType);
				}

				return null;
			}
		}

		/*
		 * NextVisibleNode
		 */

		/// <summary>
		/// Gets the next visible tree node.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode"></see> that represents the next visible tree node.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public new NuGenTreeNode NextVisibleNode
		{
			get
			{
				TreeNode nextVisibleNode = base.NextVisibleNode;

				if (nextVisibleNode is NuGenTreeNode)
				{
					return (NuGenTreeNode)nextVisibleNode;
				}
				else if (nextVisibleNode != null)
				{
					throw new InvalidCastException(Resources.InvalidCast_NodeType);
				}

				return null;
			}
		}

		/*
		 * Nodes
		 */

		private NuGenTreeNodeCollection nodes = null;

		/// <summary>
		/// Gets the collection of nodes contained within this <see cref="T:NuGenTreeNode"/>.
		/// </summary>
		public new NuGenTreeNodeCollection Nodes
		{
			get
			{
				if (this.nodes == null)
				{
					this.nodes = new NuGenTreeNodeCollection();

					this.nodes.ClearNodesRequested += delegate(object sender, NuGenItemsClearRequestedEventArgs e)
					{
						base.Nodes.Clear();
					};

					this.nodes.ContainsNodeRequested += delegate(object sender, NuGenContainsItemRequestedEventArgs e)
					{
						e.ContainsNode = base.Nodes.Contains(e.NodeToCheck);
					};

					this.nodes.EnumeratorRequested += delegate(object sender, NuGenEnumeratorRequestedEventArgs e)
					{
						e.RequestedEnumerator = base.Nodes.GetEnumerator();
					};

					this.nodes.NodeAdded += delegate(object sender, NuGenAddTreeNodeEventArgs e)
					{
						e.TreeNodeIndex = base.Nodes.Add(e.TreeNodeToAdd);
					};

					this.nodes.NodeRangeAdded += delegate(object sender, NuGenAddTreeNodeRangeEventArgs e)
					{
						base.Nodes.AddRange(e.TreeNodeRangeToAdd);
					};

					this.nodes.NodeInserted += delegate(object sender, NuGenAddTreeNodeEventArgs e)
					{
						base.Nodes.Insert(e.TreeNodeIndex, e.TreeNodeToAdd);
					};

					this.nodes.NodeRemoved += delegate(object sender, NuGenRemoveTreeNodeEventArgs e)
					{
						base.Nodes.Remove(e.TreeNodeToRemove);
					};

					this.nodes.NodeByIndexAdjusted += delegate(object sender, NuGenIndexedTreeNodeEventArgs e)
					{
						base.Nodes[e.TreeNodeIndex] = e.TreeNode;
					};

					this.nodes.NodeByIndexRequested += delegate(object sender, NuGenIndexedTreeNodeEventArgs e)
					{
						TreeNode treeNode = base.Nodes[e.TreeNodeIndex];

						if (treeNode is NuGenTreeNode)
						{
							e.TreeNode = (NuGenTreeNode)treeNode;
						}
						else if (treeNode != null)
						{
							throw new InvalidCastException(Resources.InvalidCast_NodeType);
						}
					};

					this.nodes.NodeCountRequested += delegate(object sender, NuGenItemsCountRequestedEventArgs e)
					{
						e.Count = base.Nodes.Count;
					};
				}

				return this.nodes;
			}
		}

		/*
		 * Parent
		 */

		/// <summary>
		/// Gets the parent tree node of the current tree node.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode"></see> that represents the parent of the current tree node.</returns>
		public new NuGenTreeNode Parent
		{
			get
			{
				TreeNode parent = base.Parent;

				if (parent is NuGenTreeNode)
				{
					return (NuGenTreeNode)parent;
				}
				else if (parent != null)
				{
					throw new InvalidCastException(Resources.InvalidCast_NodeType);
				}

				return null;
			}
		}

		/*
		 * PrevNode
		 */

		/// <summary>
		/// Gets the previous sibling tree node.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode"></see> that represents the previous sibling tree node.</returns>
		public new NuGenTreeNode PrevNode
		{
			get
			{
				TreeNode prevNode = base.PrevNode;

				if (prevNode is NuGenTreeNode)
				{
					return (NuGenTreeNode)prevNode;
				}
				else if (prevNode != null)
				{
					throw new InvalidCastException(Resources.InvalidCast_NodeType);
				}

				return null;
			}
		}

		/*
		 * PrevVisibleNode
		 */

		/// <summary>
		/// Gets the previous visible tree node.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode"></see> that represents the previous visible tree node.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public new NuGenTreeNode PrevVisibleNode
		{
			get
			{
				TreeNode prevVisibleNode = base.PrevVisibleNode;

				if (prevVisibleNode is NuGenTreeNode)
				{
					return (NuGenTreeNode)prevVisibleNode;
				}
				else if (prevVisibleNode != null)
				{
					throw new InvalidCastException(Resources.InvalidCast_NodeType);
				}

				return null;
			}
		}

		#endregion

		#region Methods.Public.Static

		/// <summary>
		/// Retrieves a <see cref="T:NuGenTreeNode"/> from the given handle.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="nodeOwnerTreView"/> is <see langword="null"/>.
		/// </exception>
		public static NuGenTreeNode FromHandle(NuGenTreeView nodeOwnerTreeView, IntPtr treeNodeHandle)
		{
			if (nodeOwnerTreeView == null)
			{
				throw new ArgumentNullException("nodeOwnerTreeView");
			}

			return (NuGenTreeNode)TreeNode.FromHandle(nodeOwnerTreeView, treeNodeHandle);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>Text = ""</c>. <c>HasCheckBox = true</c>. <c>Checked = false</c>.
		/// </summary>
		public NuGenTreeNode()
			: this("")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>HasCheckBox = true</c>. <c>Checked = false</c>.
		/// </summary>
		public NuGenTreeNode(string nodeText)
			: this(nodeText, true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>Checked = false</c>.
		/// </summary>
		public NuGenTreeNode(string nodeText, bool hasCheckBox)
			: this(nodeText, hasCheckBox, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.
		/// </summary>
		public NuGenTreeNode(string nodeText, bool hasCheckBox, bool isChecked)
			: this(nodeText, hasCheckBox, isChecked, -1, -1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>HasCheckBox = true</c>. <c>Checked = false</c>. <c>SelectedImageIndex = ImageIndex</c>.
		/// </summary>
		public NuGenTreeNode(string nodeText, int imageIndex, int expandedImageIndex)
			: this(nodeText, true, false, imageIndex, expandedImageIndex)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>SelectedImageIndex = ImageIndex</c>.
		/// </summary>
		public NuGenTreeNode(
			string nodeText,
			bool hasCheckBox,
			bool isChecked,
			int imageIndex,
			int expandedImageIndex
		)
			: base(nodeText, imageIndex, imageIndex)
		{
			this.Checked = isChecked;
			this.DefaultImageIndex = imageIndex;
			this.ExpandedImageIndex = expandedImageIndex;
			this.HasCheckBox = hasCheckBox;
		}

		#endregion
	}
}

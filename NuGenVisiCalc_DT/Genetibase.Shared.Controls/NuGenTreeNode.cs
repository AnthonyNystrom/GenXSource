/* -----------------------------------------------
 * NuGenTreeNode.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Properties;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a node of a <see cref="NuGenTreeView"/>.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(NuGenTreeNodeConverter))]
	public class NuGenTreeNode : TreeNode
	{
		#region Properties.Appearance

		/*
		 * HasCheckBox
		 */

		private bool _hasCheckBox = true;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="NuGenTreeNode"/> has a check box.
		/// Default value is <see langword="true"/>.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TreeNode_HasCheckBox")]
		public bool HasCheckBox
		{
			get
			{
				return _hasCheckBox;
			}
			set
			{
				_hasCheckBox = value;
			}
		}

		#endregion

		#region Properties.Behavior

		/*
		 * DefaultImageIndex
		 */

		private int _defaultImageIndex = -1;

		/// <summary>
		/// Gets or sets the index of the image that is used when the node is in its default state (collapsed).
		/// </summary>
		[Browsable(true)]
		[DefaultValue(-1)]
		[Editor("Genetibase.Shared.Design.NuGenImageIndexEditor", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TreeNode_DefaultImageIndex")]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.ImageList")]
		public int DefaultImageIndex
		{
			get
			{
				return _defaultImageIndex;
			}
			set
			{
				if (_defaultImageIndex != value)
				{
					_defaultImageIndex = value;

					if (!this.IsExpanded)
					{
						this.ImageIndex = _defaultImageIndex;
						this.SelectedImageIndex = _defaultImageIndex;
					}
				}
			}
		}

		/*
		 * ExpandedImageIndex
		 */

		private int _expandedImageIndex = -1;

		/// <summary>
		/// Gets or sets the index of the image that is used when the node is expanded.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(-1)]
		[Editor("Genetibase.Shared.Design.NuGenImageIndexEditor", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TreeNode_ExpandedImageIndex")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.ImageList")]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		public int ExpandedImageIndex
		{
			get
			{
				return _expandedImageIndex;
			}
			set
			{
				if (_expandedImageIndex != value)
				{
					_expandedImageIndex = value;

					if (this.IsExpanded)
					{
						this.ImageIndex = _expandedImageIndex;
						this.SelectedImageIndex = _expandedImageIndex;
					}
				}
			}
		}

		#endregion

		#region Properties.Public

		/*
		 * NodeFont
		 */

		/// <summary>
		/// Gets or sets the font used to display the text on the tree node's label.
		/// </summary>
		/// <value></value>
		/// <returns>The <see cref="T:System.Drawing.Font"></see> used to display the text on the tree node's label.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public new Font NodeFont
		{
			get
			{
				return base.NodeFont;
			}
			[SecurityPermission(SecurityAction.LinkDemand)]
			set
			{
				base.NodeFont = value;

				/*
				 * FIX: System.Windows.Forms.TreeNode bounds are not recalculated if node font style changes.
				 */

				TreeView treeView = this.TreeView;

				if (treeView != null && treeView.IsHandleCreated)
				{
					TVITEMEX tvItemEx = new TVITEMEX();
					tvItemEx.mask = CommCtrl.TVIF_HANDLE | CommCtrl.TVIF_STATE;
					tvItemEx.hItem = this.Handle;

					if (value != null && value.Bold)
					{
						tvItemEx.state = CommCtrl.TVIS_BOLD;
					}
					else
					{
						tvItemEx.state = 0;
					}

					tvItemEx.stateMask = CommCtrl.TVIS_BOLD;

					IntPtr tvItemExPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TVITEMEX)));
					Marshal.StructureToPtr(tvItemEx, tvItemExPtr, true);
					User32.SendMessage(treeView.Handle, CommCtrl.TVM_SETITEM, IntPtr.Zero, tvItemExPtr);
					Marshal.FreeHGlobal(tvItemExPtr);
				}
			}
		}

		/*
		 * Nodes
		 */

		/// <summary>
		/// Gets the collection of tree nodes that are assigned to the tree view control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNodeCollection"></see> that represents the tree nodes assigned to the tree view control.</returns>
		[Editor("Genetibase.Shared.Controls.Design.NuGenTreeNodeCollectionEditor", typeof(UITypeEditor))]
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
		 * ImageKey
		 */

		/// <summary>
		/// Gets or sets the key for the image associated with this tree node when the node is in an unselected state.
		/// </summary>
		/// <value></value>
		/// <returns>The key for the image associated with this tree node when the node is in an unselected state.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string ImageKey
		{
			get
			{
				return base.ImageKey;
			}
			set
			{
				base.ImageKey = value;
			}
		}

		/*
		 * ImageIndex
		 */

		/// <summary>
		/// Gets or sets the image list index value of the image displayed when the tree node is in the unselected state.
		/// </summary>
		/// <value></value>
		/// <returns>A zero-based index value that represents the image position in the assigned <see cref="T:System.Windows.Forms.ImageList"></see>.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int ImageIndex
		{
			get
			{
				return base.ImageIndex;
			}
			set
			{
				base.ImageIndex = value;
			}
		}

		/*
		 * Index
		 */

		/// <summary>
		/// Gets the position of the tree node in the tree node collection.
		/// </summary>
		/// <value></value>
		/// <returns>A zero-based index value that represents the position of the tree node in the <see cref="P:System.Windows.Forms.TreeNode.Nodes"></see> collection.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int Index
		{
			get
			{
				return base.Index;
			}
		}

		/*
		 * SelectedImageIndex
		 */

		/// <summary>
		/// Gets or sets the image list index value of the image that is displayed when the tree node is in the selected state.
		/// </summary>
		/// <value></value>
		/// <returns>A zero-based index value that represents the image position in an <see cref="T:System.Windows.Forms.ImageList"></see>.</returns>
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

		/*
		 * SelectedImageKey
		 */

		/// <summary>
		/// Gets or sets the key of the image displayed in the tree node when it is in a selected state.
		/// </summary>
		/// <value></value>
		/// <returns>The key of the image displayed when the tree node is in a selected state.</returns>
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
		 * StateImageIndex
		 */

		/// <summary>
		/// Gets or sets the index of the image used to indicate the state of the <see cref="T:System.Windows.Forms.TreeNode"></see> when the parent <see cref="T:System.Windows.Forms.TreeView"></see> has its <see cref="P:System.Windows.Forms.TreeView.CheckBoxes"></see> property set to false.
		/// </summary>
		/// <value></value>
		/// <returns>The index of the image used to indicate the state of the <see cref="T:System.Windows.Forms.TreeNode"></see>.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int StateImageIndex
		{
			get
			{
				return base.StateImageIndex;
			}
			set
			{
				base.StateImageIndex = value;
			}
		}

		/*
		 * StateImageKey
		 */

		/// <summary>
		/// Gets or sets the key of the image used to indicate the state of the <see cref="T:System.Windows.Forms.TreeNode"></see> when the parent <see cref="T:System.Windows.Forms.TreeView"></see> has its <see cref="P:System.Windows.Forms.TreeView.CheckBoxes"></see> property set to false.
		/// </summary>
		/// <value></value>
		/// <returns>The key of the image used to indicate the state of the <see cref="T:System.Windows.Forms.TreeNode"></see>.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string StateImageKey
		{
			get
			{
				return base.StateImageKey;
			}
			set
			{
				base.StateImageKey = value;
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/*
		 * Clone
		 */

		/// <summary>
		/// Copies the tree node and the entire subtree rooted at this tree node.
		/// </summary>
		/// <returns>
		/// The <see cref="T:System.Object"></see> that represents the cloned <see cref="T:System.Windows.Forms.TreeNode"></see>.
		/// </returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[SecurityPermission(SecurityAction.Demand)]
		public override object Clone()
		{
			NuGenTreeNode node = new NuGenTreeNode();

			node.BackColor = this.BackColor;
			node.Checked = this.Checked;
			node.ContextMenu = this.ContextMenu;
			node.ContextMenuStrip = this.ContextMenuStrip;
			node.DefaultImageIndex = this.DefaultImageIndex;
			node.ExpandedImageIndex = this.ExpandedImageIndex;
			node.ForeColor = this.ForeColor;
			node.HasCheckBox = this.HasCheckBox;
			node.Name = this.Name;
			node.NodeFont = this.NodeFont;
			node.Tag = this.Tag;
			node.Text = this.Text;
			node.ToolTipText = this.ToolTipText;

			foreach (NuGenTreeNode childNode in this.Nodes)
			{
				node.Nodes.Add((NuGenTreeNode)childNode.Clone());
			}

			return node;
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Saves the state of the <see cref="NuGenTreeNode"/> to the specified <see cref="SerializationInfo"/>.
		/// </summary>
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter), SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected override void Serialize(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(_entryBackColor, this.BackColor);
			info.AddValue(_entryChecked, this.Checked);

			if (this.ContextMenu != null)
			{
				info.AddValue(_entryContextMenu, this.ContextMenu);
			}

			if (this.ContextMenuStrip != null)
			{
				info.AddValue(_entryContextMenuStrip, this.ContextMenuStrip);
			}

			info.AddValue(_entryDefaultImageIndex, this.DefaultImageIndex);
			info.AddValue(_entryExpandedImageIndex, this.ExpandedImageIndex);
			info.AddValue(_entryForeColor, this.ForeColor);
			info.AddValue(_entryHasCheckBox, this.HasCheckBox);
			info.AddValue(_entryName, this.Name);

			if (this.NodeFont != null)
			{
				info.AddValue(_entryNodeFont, this.NodeFont);
			}

			if (this.Tag != null)
			{
				info.AddValue(_entryTag, this.Tag);
			}

			info.AddValue(_entryText, this.Text);
			info.AddValue(_entryToolTipText, this.ToolTipText);
			info.AddValue(_entryChildCount, this.Nodes.Count);

			for (int i = 0; i < this.Nodes.Count; i++)
			{
				info.AddValue(_entryChildNode + i, this.Nodes[i], typeof(NuGenTreeNode));
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>Text = ""</c>. <c>HasCheckBox = true</c>. <c>Checked = false</c>.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public NuGenTreeNode()
			: this("")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>HasCheckBox = true</c>. <c>Checked = false</c>.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public NuGenTreeNode(string nodeText)
			: this(nodeText, true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>HasCheckBox = true</c>. <c>Checked = false</c>.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="childNodes"/> is <see langword="null"/>.</para>
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public NuGenTreeNode(string nodeText, NuGenTreeNode[] childNodes)
			: this(nodeText)
		{
			if (childNodes == null)
			{
				throw new ArgumentNullException("childNodes");
			}

			this.Nodes.AddRange(childNodes);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>Checked = false</c>.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public NuGenTreeNode(string nodeText, bool hasCheckBox)
			: this(nodeText, hasCheckBox, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public NuGenTreeNode(string nodeText, bool hasCheckBox, bool isChecked)
			: this(nodeText, hasCheckBox, isChecked, -1, -1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>HasCheckBox = true</c>. <c>Checked = false</c>. <c>SelectedImageIndex = ImageIndex</c>.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public NuGenTreeNode(string nodeText, int imageIndex, int expandedImageIndex)
			: this(nodeText, true, false, imageIndex, expandedImageIndex)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="childNodes"/> is <see langword="null"/>.</para>
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public NuGenTreeNode(string nodeText, int imageIndex, int expandedImageIndex, NuGenTreeNode[] childNodes)
			: this(nodeText, imageIndex, expandedImageIndex)
		{
			if (childNodes == null)
			{
				throw new ArgumentNullException("childNodes");
			}

			this.Nodes.AddRange(childNodes);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.<para/>
		/// <c>SelectedImageIndex = ImageIndex</c>.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand)]
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

		private const string _entryBackColor = "BackColor";
		private const string _entryChecked = "Checked";
		private const string _entryContextMenu = "ContextMenu";
		private const string _entryContextMenuStrip = "ContextMenuStrip";
		private const string _entryDefaultImageIndex = "DefaultImageIndex";
		private const string _entryExpandedImageIndex = "ExpandedImageIndex";
		private const string _entryForeColor = "ForeColor";
		private const string _entryHasCheckBox = "HasCheckBox";
		private const string _entryName = "Name";
		private const string _entryNodeFont = "NodeFont";
		private const string _entryTag = "Tag";
		private const string _entryText = "Text";
		private const string _entryToolTipText = "ToolTipText";
		private const string _entryChildCount = "ChildCount";
		private const string _entryChildNode = "ChildNode";

		/// <summary>
		/// Restores the state of the <see cref="NuGenTreeNode"/> from the specified <see cref="SerializationInfo"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="info"/> is <see langword="null"/>.</para></exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		protected NuGenTreeNode(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			int childCount = 0;

			foreach (SerializationEntry entry in info)
			{
				switch (entry.Name)
				{
					case _entryBackColor:
					{
						this.BackColor = (Color)info.GetValue(entry.Name, typeof(Color));
						break;
					}
					case _entryChildNode:
					{
						this.Checked = info.GetBoolean(entry.Name);
						break;
					}
					case _entryContextMenu:
					{
						this.ContextMenu = (ContextMenu)info.GetValue(entry.Name, typeof(ContextMenu));
						break;
					}
					case _entryContextMenuStrip:
					{
						this.ContextMenuStrip = (ContextMenuStrip)info.GetValue(entry.Name, typeof(ContextMenuStrip));
						break;
					}
					case _entryDefaultImageIndex:
					{
						this.DefaultImageIndex = info.GetInt32(entry.Name);
						break;
					}
					case _entryExpandedImageIndex:
					{
						this.ExpandedImageIndex = info.GetInt32(entry.Name);
						break;
					}
					case _entryForeColor:
					{
						this.ForeColor = (Color)info.GetValue(entry.Name, typeof(Color));
						break;
					}
					case _entryHasCheckBox:
					{
						this.HasCheckBox = info.GetBoolean(entry.Name);
						break;
					}
					case _entryName:
					{
						this.Name = info.GetString(entry.Name);
						break;
					}
					case _entryNodeFont:
					{
						this.NodeFont = (Font)info.GetValue(entry.Name, typeof(Font));
						break;
					}
					case _entryTag:
					{
						this.Tag = entry.Value;
						break;
					}
					case _entryText:
					{
						this.Text = info.GetString(entry.Name);
						break;
					}
					case _entryToolTipText:
					{
						this.ToolTipText = info.GetString(entry.Name);
						break;
					}
					case _entryChildCount:
					{
						childCount = info.GetInt32(entry.Name);
						break;
					}
				}

				if (childCount > 0)
				{
					TreeNode[] nodes = new TreeNode[childCount];

					for (int i = 0; i < childCount; i++)
					{
						nodes[i] = (TreeNode)info.GetValue(
							_entryChildNode + i.ToString(CultureInfo.InvariantCulture),
							typeof(TreeNode)
						);
					}

					this.Nodes.AddRange(nodes);
				}
			}
		}
	}
}

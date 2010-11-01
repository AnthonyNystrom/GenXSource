/* -----------------------------------------------
 * NuGenTreeNode.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Design;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Design;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a node of a <see cref="NuGenTreeView"/>.
	/// </summary>
	[TypeConverter(typeof(NuGenTreeNodeConverter))]
	public class NuGenTreeNode : TreeNode
	{
		#region Properties.Public

		/*
		 * DefaultImageIndex
		 */

		private int _defaultImageIndex = -1;

		/// <summary>
		/// Gets or sets the index of the image that is used when the node is in its default state (collapsed).
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		[Editor(typeof(NuGenImageIndexEditor), typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TreeNode_ExpandedImageIndex")]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.ImageList")]
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
		public override object Clone()
		{
			NuGenTreeNode node = new NuGenTreeNode();

			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);

			foreach (PropertyDescriptor descriptor in properties)
			{
				descriptor.SetValue(node, descriptor.GetValue(this));
			}

			if (this.Nodes.Count > 0)
			{
				foreach (NuGenTreeNode childNode in this.Nodes)
				{
					node.Nodes.Add((NuGenTreeNode)childNode.Clone());
				}
			}

			return node;
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
		/// <c>HasCheckBox = true</c>. <c>Checked = false</c>.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="childNodes"/> is <see langword="null"/>.</para>
		/// </exception>
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
		public NuGenTreeNode( string nodeText, bool hasCheckBox)
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
		/// Initializes a new instance of the <see cref="NuGenTreeNode"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="childNodes"/> is <see langword="null"/>.</para>
		/// </exception>
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

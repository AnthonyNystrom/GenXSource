/* -----------------------------------------------
 * NuGenTaskTreeNodeBase.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Text;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Base class for nodes within <see cref="T:NuGenTaskTreeView"/>.
	/// </summary>
	public abstract class NuGenTaskTreeNodeBase : NuGenTreeNode
	{
		#region Properties.Public

		/*
		 * HasPriority
		 */

		/// <summary>
		/// Gets the value indicating whether a priority may be set for this task.
		/// </summary>
		public virtual bool HasPriority
		{
			get
			{
				return true;
			}
		}

		/*
		 * IsDescriptionReadonly
		 */

		/// <summary>
		/// Gets the value indicating whether the task description is read-only.
		/// </summary>
		public virtual bool IsDescriptionReadonly
		{
			get
			{
				return false;
			}
		}

		/*
		 * IsRemovable
		 */

		/// <summary>
		/// Gets the value indicating whether the task is removable from the owner task list.
		/// </summary>
		public virtual bool IsRemovable
		{
			get
			{
				return true;
			}
		}
		
		/*
		 * Text
		 */

		private string text = "";

		/// <summary>
		/// Gets or sets task text. Node displays only the first line of the task.
		/// </summary>
		public new string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (this.text != value)
				{
					this.text = value != null ? value : "";
					this.SetBaseText(); // NOTE: This works only if this.TreeView != null.
				}
			}
		}

		/*
		 * TreeView
		 */

		private TreeView treeView = null;

		/// <summary>
		/// Gets the parent tree view that the tree node is assigned to.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeView"></see> that represents the parent tree view that the tree node is assigned to, or null if the node has not been assigned to a tree view.</returns>
		public new TreeView TreeView
		{
			get
			{
				return this.treeView;
			}
			set
			{
				if (this.treeView != value)
				{
					if (this.treeView != null)
					{
						this.treeView.Resize -= this.treeView_Resize;
					}

					this.treeView = value;

					if (this.treeView != null)
					{
						this.treeView.Resize += this.treeView_Resize;
					}
				}
			}
		}

		#endregion

		#region Properties.Protected
		
		/*
		 * StringProcessor
		 */

		private INuGenStringProcessor stringProcessor = null;

		/// <summary>
		/// </summary>
		protected INuGenStringProcessor StringProcessor
		{
			get
			{
				if (this.stringProcessor == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					this.stringProcessor = this.ServiceProvider.GetService<INuGenStringProcessor>();

					if (this.stringProcessor == null)
					{
						throw new InvalidOperationException(
							string.Format(Resources.InvalidOperation_NotServiceExist, typeof(INuGenStringProcessor).ToString())
						);
					}
				}

				return this.stringProcessor;
			}
		}

		/*
		 * XmlService
		 */

		private INuGenTaskXmlService xmlService = null;

		/// <summary>
		/// </summary>
		protected INuGenTaskXmlService XmlService
		{
			get
			{
				if (this.xmlService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					this.xmlService = this.ServiceProvider.GetService<INuGenTaskXmlService>();

					if (this.xmlService == null)
					{
						throw new InvalidOperationException(
							string.Format(Resources.InvalidOperation_NotServiceExist, typeof(INuGenTaskXmlService).ToString())
						);
					}
				}

				return this.xmlService;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider serviceProvider = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return this.serviceProvider;
			}
		}

		#endregion

		#region Methods.Public.Abstract

		/*
		 * Load
		 */

		/// <summary>
		/// </summary>
		public abstract void Load(XmlNode nodeToLoadFrom);

		/*
		 * Save
		 */

		/// <summary>
		/// </summary>
		public abstract void Save(XmlNode nodeToSaveTo);

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * GetVisibleNodeTextMaxLength
		 */

		/// <summary>
		/// Retrieves the width of this <see cref="T:NuGenTaskTreeNodeBase"/> visible text area.
		/// </summary>
		/// <returns></returns>
		protected virtual int GetVisibleNodeTextMaxLength()
		{
			if (this.TreeView != null)
			{
				int offset = this.Level * this.TreeView.Indent;

				if (this.HasCheckBox)
				{
					offset += SystemInformation.IconSize.Width;
				}

				if (this.DefaultImageIndex > -1)
				{
					offset += SystemInformation.IconSize.Width;
				}

				return this.TreeView.ClientRectangle.Width - offset;
			}

			return 0;
		}

		/*
		 * SetBaseText
		 */

		/// <summary>
		/// Sets the value for the underlying <see cref="P:Text"/> property. The actual node text can differ
		/// from the text the node displays to the user within a <see cref="T:TreeView"/>. Works only if 
		/// <see cref="P:TreeView"/> != <see langword="null"/>.
		/// </summary>
		protected virtual void SetBaseText()
		{
			this.TreeView = base.TreeView;

			if (this.TreeView != null)
			{
				using (Graphics g = this.TreeView.CreateGraphics())
				{
					base.Text = this.StringProcessor.EatLine(
						this.StringProcessor.GetContentUntilCRLF(this.Text),
						this.NodeFont != null ? this.NodeFont : this.TreeView.Font,
						this.GetVisibleNodeTextMaxLength(),
						g
					);
				}
			}
		}

		#endregion

		#region EventHandlers

		private void treeView_Resize(object sender, EventArgs e)
		{
			this.SetBaseText();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskTreeNodeBase"/> class.
		/// </summary>
		/// <param name="serviceProvider">Required:<para/>
		/// <see cref="T:INuGenStringProcessor"/><para/>
		/// <see cref="T:INuGenTaskXmlService"/><para/>
		/// </param>
		/// <param name="defaultNodeText">Specifies the text that appears on the node just after creation.
		/// The value of the <see cref="P:Text"/> property will still remain an empty string. Use the
		/// <see cref="P:Text"/> property to set actual text for the node after constructor initialization.
		/// </param>
		/// <param name="imageIndex"></param>
		/// <param name="expandedImageIndex"></param>
		protected NuGenTaskTreeNodeBase(
			INuGenServiceProvider serviceProvider,
			string defaultNodeText,
			int imageIndex,
			int expandedImageIndex
			) : base(defaultNodeText, imageIndex, expandedImageIndex)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskTreeNodeBase"/> class.<para/>
		/// <paramref name="defaultNodeText"/> is an empty string.
		/// </summary>
		protected NuGenTaskTreeNodeBase(
			INuGenServiceProvider serviceProvider
			) : this(serviceProvider, "", -1, -1)
		{
		}

		#endregion
	}
}

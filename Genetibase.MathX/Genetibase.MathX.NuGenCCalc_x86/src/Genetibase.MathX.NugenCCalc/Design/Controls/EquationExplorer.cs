using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	/// <summary>
	/// Summary description for EquationExplorer.
	/// </summary>
	[ToolboxItem(false)]
	public class EquationExplorer : System.Windows.Forms.UserControl
	{
		private bool _is3DDesigner = false;

		public event ItemActionEventHandler OnItemAction;
		private FunctionParameters _currentItem = null;
		private FunctionCollection _equations = null;

		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.TreeView tvEquations;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuItem_Create;
		private System.Windows.Forms.MenuItem menuItem_Delete;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem_Use;
		private System.Windows.Forms.MenuItem menuItem_Edit;


		public EquationExplorer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EquationExplorer));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem_Create = new System.Windows.Forms.MenuItem();
			this.menuItem_Edit = new System.Windows.Forms.MenuItem();
			this.menuItem_Delete = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem_Use = new System.Windows.Forms.MenuItem();
			this.tvEquations = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.menuItem_Create,
																						this.menuItem_Edit,
																						this.menuItem_Delete,
																						this.menuItem1,
																						this.menuItem_Use});
			this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
			// 
			// menuItem_Create
			// 
			this.menuItem_Create.Index = 0;
			this.menuItem_Create.Text = "Create";
			this.menuItem_Create.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItem_Edit
			// 
			this.menuItem_Edit.Index = 1;
			this.menuItem_Edit.Text = "Edit";
			this.menuItem_Edit.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItem_Delete
			// 
			this.menuItem_Delete.Index = 2;
			this.menuItem_Delete.Text = "Delete";
			this.menuItem_Delete.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 3;
			this.menuItem1.Text = "-";
			// 
			// menuItem_Use
			// 
			this.menuItem_Use.Index = 4;
			this.menuItem_Use.Text = "Use It!";
			this.menuItem_Use.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// tvEquations
			// 
			this.tvEquations.ContextMenu = this.contextMenu;
			this.tvEquations.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvEquations.ImageList = this.imageList;
			this.tvEquations.Location = new System.Drawing.Point(0, 0);
			this.tvEquations.Name = "tvEquations";
			this.tvEquations.Size = new System.Drawing.Size(150, 272);
			this.tvEquations.TabIndex = 0;
			this.tvEquations.Click += new System.EventHandler(this.tvEquations_Click);
			this.tvEquations.DoubleClick += new System.EventHandler(this.tvEquations_DoubleClick);
			this.tvEquations.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvEquations_AfterSelect);
			// 
			// EquationExplorer
			// 
			this.Controls.Add(this.tvEquations);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Name = "EquationExplorer";
			this.Size = new System.Drawing.Size(150, 272);
			this.ResumeLayout(false);

		}
		#endregion

		public bool Is3DDesigner
		{
			set
			{
				if (_is3DDesigner == value)
					return;
				_is3DDesigner = value;
			}
		}

		/// <summary>
		/// Set EquationCollection for this tree
		/// </summary>
		public FunctionCollection Equations
		{
			set
			{
				_equations = value;
			}
		}
		
		/// <summary>
		/// Get current Equation
		/// </summary>
		public FunctionParameters Selected
		{
			get
			{
				return _currentItem;
			}
		}

		/// <summary>
		/// Create nodes
		/// </summary>
		public void Init()
		{
			try
			{
				if (this._equations == null)
					return;

				_currentItem = null;
				this.tvEquations.Nodes.Clear();
				// create root nodes
				TreeNode root = new TreeNode();
				root.Text = "Equations";
				root.ImageIndex = 0;
				root.SelectedImageIndex = 1;
				FunctionNode node = null;

				if (_is3DDesigner)
				{
					foreach(FunctionParameters equation in _equations.Functions3D)
					{
						node = new FunctionNode(equation);
						node.ImageIndex = 2;
						node.SelectedImageIndex = 2;
						root.Nodes.Add(node);
					}
				}
				else
				{
					foreach(FunctionParameters equation in _equations.Functions2D)
					{
						node = new FunctionNode(equation);
						node.ImageIndex = 2;
						node.SelectedImageIndex = 2;
						root.Nodes.Add(node);
					}
				}
				this.tvEquations.Nodes.Add(root);
				this.tvEquations.ExpandAll();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Unknown error. Error:" + ex.Message, "Equation repository view", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		private void menuItem_Click(object sender, System.EventArgs e)
		{
			MenuItem item = (MenuItem)sender;
			switch(item.Text)
			{
				case "Create":
					ItemAction(null, "Create");
					break;
				case "Edit":
					ItemAction(_currentItem, "Edit");
					break;
				case "Delete":
					ItemAction(_currentItem, "Delete");
					break;
				case "Use It!":
					ItemAction(_currentItem, "Use It!");
					break;
				default:
					MessageBox.Show(this, "Unknown menu item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
			}
		}


		private void tvEquations_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			try
			{
				FunctionNode FunctionNode = e.Node as FunctionNode;

				_currentItem = null;

				if (FunctionNode != null) 
					_currentItem = (FunctionParameters)FunctionNode.Item ;
				else
					this.tvEquations.SelectedNode = e.Node.NextVisibleNode;				
			}
			catch(Exception ex)
			{
				MessageBox.Show(this,"Can't get node tag. Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="action"></param>
		private void ItemAction (FunctionParameters item, string action)
		{
			if (OnItemAction!=null)
			{
				OnItemAction(this, new ItemEventArgs(item,action));
			}
		}


		/// <summary>
		/// Show or hide some context menu items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				FunctionParameters equation = null;
				if (this.tvEquations.SelectedNode != null)
				{
					equation = ((FunctionNode)this.tvEquations.SelectedNode).Item as FunctionParameters;
				}


				if (equation == null) 
				{
					_currentItem = null;
					this.menuItem_Delete.Visible = false;
					this.menuItem_Edit.Visible = false;
					this.menuItem_Use.Visible = false;
					this.menuItem1.Visible = false;
				}
				else
				{
					_currentItem = equation;
					this.menuItem_Delete.Visible = true;
					this.menuItem_Edit.Visible = true;
					this.menuItem_Use.Visible = true;
					this.menuItem1.Visible = true;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this,"Can't get node tag. Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void tvEquations_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				FunctionNode FunctionNode = tvEquations.GetNodeAt(tvEquations.PointToClient(Cursor.Position)) as FunctionNode;

				_currentItem = null;

				if (FunctionNode != null) 
				{
					_currentItem = (FunctionParameters)FunctionNode.Item;
					tvEquations.SelectedNode = FunctionNode;
					ItemAction(_currentItem, "Edit");
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(this,"Can't get node tag. Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void tvEquations_Click(object sender, System.EventArgs e)
		{
			FunctionNode FunctionNode = tvEquations.GetNodeAt(tvEquations.PointToClient(Cursor.Position)) as FunctionNode;
			if (FunctionNode == null)
				return;
			tvEquations.SelectedNode = FunctionNode;
		}
	}
}

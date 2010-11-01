/* -----------------------------------------------
 * NuGenTreeNodeCollectionEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenTreeNodeCollectionEditor
	{
		private sealed class EditorForm : CollectionForm
		{
			#region Declarations.Fields

			private NuGenTreeNodeCollectionEditor _editor;

			private Button _addChildButton;
			private Button _addRootButton;
			private Button _cancelButton;
			private Button _deleteButton;
			private Button _okButton;
			private Button _moveDownButton;
			private Button _moveUpButton;
			
			private Label _treeViewDescriptionLabel;
			private Label _propertyDescriptionLabel;

			private NuGenPropertyGrid _propertyGrid;

			private TableLayoutPanel _navigationButtonsTableLayoutPanel;
			private TableLayoutPanel _nodeControlPanel;
			private TableLayoutPanel _okCancelPanel;
			private TableLayoutPanel _overarchingTableLayoutPanel;
			
			private TreeNode _currentNode;
			private NodesEditorTreeView _treeView;

			private int _intialNextNode;
			private int _nextNode;

			private static object _nextNodeKey;

			#endregion

			#region Properties.Private

			/*
			 * LastNode
			 */

			private TreeNode LastNode
			{
				get
				{
					TreeNode node = _treeView.Nodes[_treeView.Nodes.Count - 1];
					
					while (node.Nodes.Count > 0)
					{
						node = node.Nodes[node.Nodes.Count - 1];
					}
					
					return node;
				}
			}

			/*
			 * NextNode
			 */

			private int NextNode
			{
				get
				{
					if ((this.TreeView != null) && (this.TreeView.Site != null))
					{
						IDictionaryService dictionaryService = (IDictionaryService)this.TreeView.Site.GetService(typeof(IDictionaryService));
						
						if (dictionaryService != null)
						{
							object nextNodeKey = dictionaryService.GetValue(_nextNodeKey);
							
							if (nextNodeKey != null)
							{
								_nextNode = (int)nextNodeKey;
							}
							else
							{
								_nextNode = 0;
								dictionaryService.SetValue(_nextNodeKey, 0);
							}
						}
					}

					return _nextNode;
				}
				set
				{
					_nextNode = value;

					if ((this.TreeView != null) && (this.TreeView.Site != null))
					{
						IDictionaryService dictionaryService = (IDictionaryService)this.TreeView.Site.GetService(typeof(IDictionaryService));
						
						if (dictionaryService != null)
						{
							dictionaryService.SetValue(_nextNodeKey, _nextNode);
						}
					}
				}
			}

			/*
			 * TreeView
			 */

			private TreeView TreeView
			{
				get
				{
					if ((base.Context != null) && (base.Context.Instance is TreeView))
					{
						return (TreeView)base.Context.Instance;
					}

					return null;
				}
			}

			#endregion

			#region Methods.Protected.Overridden

			/// <summary>
			/// Provides an opportunity to perform processing when a collection value has changed.
			/// </summary>
			protected override void OnEditValueChanged()
			{
				if (base.EditValue != null)
				{
					object[] items = base.Items;
					TreeNode[] nodes = new TreeNode[items.Length];
					
					for (int i = 0; i < items.Length; i++)
					{
						nodes[i] = (NuGenTreeNode)((NuGenTreeNode)items[i]).Clone();
					}
					
					_treeView.Nodes.Clear();
					_treeView.Nodes.AddRange(nodes);
					_currentNode = null;
					_addChildButton.Enabled = false;
					_deleteButton.Enabled = false;

					TreeView treeView = this.TreeView;

					if (treeView != null)
					{
						this.SetImageProps(treeView);
					}
					
					if ((items.Length > 0) && (nodes[0] != null))
					{
						_treeView.SelectedNode = nodes[0];
					}
				}
			}

			#endregion

			#region Methods.Private

			/*
			 * Add
			 */

			private void Add(TreeNode parent)
			{
				TreeNode node = null;

				string nodeText = Resources.Text_TreeNodeCollectionEditor_node;

				if (parent == null)
				{
					int num1;
					this.NextNode = (num1 = this.NextNode) + 1;
					int num2 = num1;
					node = new NuGenTreeNode(nodeText + num2.ToString(CultureInfo.InvariantCulture));
					_treeView.Nodes.Add(node);
					node.Name = node.Text;
				}
				else
				{
					int num3;
					this.NextNode = (num3 = this.NextNode) + 1;
					int num4 = num3;
					node = new NuGenTreeNode(nodeText + num4.ToString(CultureInfo.InvariantCulture));
					parent.Nodes.Add(node);
					node.Name = node.Text;
					parent.Expand();
				}
				if (parent != null)
				{
					_treeView.SelectedNode = parent;
				}
				else
				{
					_treeView.SelectedNode = node;
					this.SetNodeProps(node);
				}
			}

			/*
			 * CheckParent
			 */

			private bool CheckParent(TreeNode child, TreeNode parent)
			{
				while (child != null)
				{
					if (parent == child.Parent)
					{
						return true;
					}

					child = child.Parent;
				}

				return false;
			}

			/*
			 * HookEvents
			 */

			private void HookEvents()
			{
				_okButton.Click += _okButton_Click;
				_cancelButton.Click += _cancelButton_Click;
				_addChildButton.Click += _addChildButton_Click;
				_addRootButton.Click += _addRootButton_Click;
				_deleteButton.Click += _deleteButton_Click;
				_propertyGrid.PropertyValueChanged += _propertyGrid_PropertyValueChanged;
				_treeView.AfterSelect += _treeView_AfterSelect;
				_moveDownButton.Click += _moveDownButton_Click;
				_moveUpButton.Click += _moveUpButton_Click;
			}

			/*
			 * InitializeComponent
			 */

			private void InitializeComponent()
			{
				_okButton = new Button();
				_cancelButton = new Button();
				
				_addRootButton = new Button();
				_addChildButton = new Button();
				_deleteButton = new Button();
				_moveDownButton = new Button();
				_moveUpButton = new Button();

				_propertyDescriptionLabel = new Label();
				_treeViewDescriptionLabel = new Label();

				_propertyGrid = new NuGenPropertyGrid();
				_propertyGrid.Dock = DockStyle.Fill;

				_okCancelPanel = new TableLayoutPanel();
				_nodeControlPanel = new TableLayoutPanel();
				_overarchingTableLayoutPanel = new TableLayoutPanel();
				_navigationButtonsTableLayoutPanel = new TableLayoutPanel();

				_treeView = new NodesEditorTreeView();
				_treeView.Dock = DockStyle.Fill;
				
				_okCancelPanel.SuspendLayout();
				_nodeControlPanel.SuspendLayout();
				_overarchingTableLayoutPanel.SuspendLayout();
				_navigationButtonsTableLayoutPanel.SuspendLayout();
				this.SuspendLayout();

				_okCancelPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				_okCancelPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				_okCancelPanel.Controls.Add(_okButton, 0, 0);
				_okCancelPanel.Controls.Add(_cancelButton, 1, 0);
				_okCancelPanel.Dock = DockStyle.Right;
				_okCancelPanel.Margin = new Padding(3, 0, 0, 0);
				_okCancelPanel.RowStyles.Add(new RowStyle());
				
				_okButton.DialogResult = DialogResult.OK;
				_okButton.Dock = DockStyle.Fill;
				_okButton.Text = Resources.Text_TreeNodeCollectionEditor_okButton;
				
				_cancelButton.DialogResult = DialogResult.Cancel;
				_cancelButton.Dock = DockStyle.Fill;
				_cancelButton.Text = Resources.Text_TreeNodeCollectionEditor_cancelButton;
				
				_nodeControlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				_nodeControlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				_nodeControlPanel.Dock = DockStyle.Fill;
				_nodeControlPanel.Controls.Add(_addRootButton, 0, 0);
				_nodeControlPanel.Controls.Add(_addChildButton, 1, 0);
				_nodeControlPanel.Margin = new Padding(0, 3, 3, 3);
				_nodeControlPanel.RowStyles.Add(new RowStyle());

				_addRootButton.Dock = DockStyle.Fill;
				_addRootButton.Margin = new Padding(0, 0, 3, 0);
				_addRootButton.Text = Resources.Text_TreeNodeCollectionEditor_addRootButton;

				_addChildButton.Dock = DockStyle.Fill;
				_addChildButton.Margin = new Padding(3, 0, 0, 0);
				_addChildButton.Text = Resources.Text_TreeNodeCollectionEditor_addChildButton;

				_deleteButton.Dock = DockStyle.Fill;
				_deleteButton.Margin = new Padding(0, 3, 0, 0);
				_deleteButton.Image = Resources.Delete;
				_deleteButton.Size = new Size(30, 30);

				_moveDownButton.Dock = DockStyle.Fill;
				_moveDownButton.Margin = new Padding(0, 1, 0, 3);
				_moveDownButton.Image = Resources.Down;
				_moveDownButton.Size = new Size(30, 30);

				_moveUpButton.Dock = DockStyle.Fill;
				_moveUpButton.Margin = new Padding(0, 0, 0, 1);
				_moveUpButton.Image = Resources.Up;
				_moveUpButton.Size = new Size(30, 30);
				
				_propertyGrid.LineColor = SystemColors.ScrollBar;
				_overarchingTableLayoutPanel.SetRowSpan(_propertyGrid, 2);

				_propertyDescriptionLabel.Dock = DockStyle.Fill;
				_propertyDescriptionLabel.Margin = new Padding(3, 1, 0, 0);

				_treeView.AllowDrop = true;
				
				_treeView.HideSelection = false;
				_treeView.Margin = new Padding(0, 3, 3, 3);

				_treeViewDescriptionLabel.Dock = DockStyle.Fill;
				_treeViewDescriptionLabel.Margin = new Padding(0, 1, 3, 0);
				_treeViewDescriptionLabel.Text = Resources.Text_TreeNodeCollectionEditor_treeViewDescriptionLabel;

				_overarchingTableLayoutPanel.Dock = DockStyle.Fill;
				_overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250f));
				_overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40f));
				_overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
				_overarchingTableLayoutPanel.Controls.Add(_navigationButtonsTableLayoutPanel, 1, 1);
				_overarchingTableLayoutPanel.Controls.Add(_propertyDescriptionLabel, 2, 0);
				_overarchingTableLayoutPanel.Controls.Add(_propertyGrid, 2, 1);
				_overarchingTableLayoutPanel.Controls.Add(_treeView, 0, 1);
				_overarchingTableLayoutPanel.Controls.Add(_treeViewDescriptionLabel, 0, 0);
				_overarchingTableLayoutPanel.Controls.Add(_nodeControlPanel, 0, 2);
				_overarchingTableLayoutPanel.Controls.Add(_okCancelPanel, 2, 3);
				_overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
				_overarchingTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
				_overarchingTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
				_overarchingTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));

				_navigationButtonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				_navigationButtonsTableLayoutPanel.Controls.Add(_moveUpButton, 0, 0);
				_navigationButtonsTableLayoutPanel.Controls.Add(_deleteButton, 0, 2);
				_navigationButtonsTableLayoutPanel.Controls.Add(_moveDownButton, 0, 1);
				_navigationButtonsTableLayoutPanel.RowStyles.Add(new RowStyle());
				_navigationButtonsTableLayoutPanel.RowStyles.Add(new RowStyle());
				_navigationButtonsTableLayoutPanel.RowStyles.Add(new RowStyle());

				this.AcceptButton = _okButton;
				this.AutoScaleMode = AutoScaleMode.Font;
				this.CancelButton = _cancelButton;
				this.Controls.Add(_overarchingTableLayoutPanel);
				this.Padding = new Padding(10);
				
				this.MaximizeBox = false;
				this.MinimizeBox = false;
				this.ShowIcon = false;
				this.ShowInTaskbar = false;
				this.Size = new Size(580, 480);
				this.MinimumSize = this.Size;
				this.Text = Resources.Text_TreeNodeCollectionEditor_EditorForm;

				_okCancelPanel.ResumeLayout(false);
				_okCancelPanel.PerformLayout();
				_nodeControlPanel.ResumeLayout(false);
				_nodeControlPanel.PerformLayout();
				_overarchingTableLayoutPanel.ResumeLayout(false);
				_overarchingTableLayoutPanel.PerformLayout();
				_navigationButtonsTableLayoutPanel.ResumeLayout(false);

				base.ResumeLayout(false);
			}

			/*
			 * SetNodeProps
			 */

			private void SetNodeProps(TreeNode node)
			{
				if (node != null)
				{
					_propertyDescriptionLabel.Text = string.Format(Resources.Text_TreeNodeCollectionEditor_propertyDescriptionLabel, node.Name.ToString());
				}
				else
				{
					_propertyDescriptionLabel.Text = Resources.Text_TreeNodeCollectionEditor_propertyDescriptionLabelNone;
				}

				_propertyGrid.SelectedObject = node;
			}

			/*
			 * SetByuttonsState
			 */

			private void SetButtonsState()
			{
				bool flag1 = _treeView.Nodes.Count > 0;

				_addChildButton.Enabled = flag1;
				_deleteButton.Enabled = flag1;
				_moveDownButton.Enabled = (flag1 && ((_currentNode != this.LastNode) || (_currentNode.Level > 0))) && (_currentNode != _treeView.Nodes[_treeView.Nodes.Count - 1]);
				_moveUpButton.Enabled = flag1 && (_currentNode != _treeView.Nodes[0]);
			}

			/*
			 * SetImageProps
			 */

			private void SetImageProps(TreeView actualTreeView)
			{
				if (actualTreeView.ImageList != null)
				{
					_treeView.ImageList = actualTreeView.ImageList;
					_treeView.ImageIndex = actualTreeView.ImageIndex;
					_treeView.SelectedImageIndex = actualTreeView.SelectedImageIndex;
				}
				else
				{
					_treeView.ImageList = null;
					_treeView.ImageIndex = -1;
					_treeView.SelectedImageIndex = -1;
				}
				if (actualTreeView.StateImageList != null)
				{
					_treeView.StateImageList = actualTreeView.StateImageList;
				}
				else
				{
					_treeView.StateImageList = null;
				}
				_treeView.CheckBoxes = actualTreeView.CheckBoxes;
			}

			#endregion

			#region EventHandlers

			private void _addChildButton_Click(object sender, EventArgs e)
			{
				this.Add(_currentNode);
				this.SetButtonsState();
			}

			private void _addRootButton_Click(object sender, EventArgs e)
			{
				this.Add(null);
				this.SetButtonsState();
			}

			private void _cancelButton_Click(object sender, EventArgs e)
			{
				if (this.NextNode != _intialNextNode)
				{
					this.NextNode = _intialNextNode;
				}
			}

			private void _deleteButton_Click(object sender, EventArgs e)
			{
				_currentNode.Remove();

				if (_treeView.Nodes.Count == 0)
				{
					_currentNode = null;
					this.SetNodeProps(null);
				}
				
				this.SetButtonsState();
			}

			private void _okButton_Click(object sender, EventArgs e)
			{
				object[] bufferNodes = new object[_treeView.Nodes.Count];

				for (int i = 0; i < bufferNodes.Length; i++)
				{
					bufferNodes[i] = _treeView.Nodes[i].Clone();
				}

				base.Items = bufferNodes;
				
				_treeView.Dispose();
				_treeView = null;
			}

			private void _moveDownButton_Click(object sender, EventArgs e)
			{
				TreeNode node1 = _currentNode;
				TreeNode node2 = _currentNode.Parent;

				if (node2 == null)
				{
					_treeView.Nodes.RemoveAt(node1.Index);
					_treeView.Nodes[node1.Index].Nodes.Insert(0, node1);
				}
				else
				{
					node2.Nodes.RemoveAt(node1.Index);
					
					if (node1.Index < node2.Nodes.Count)
					{
						node2.Nodes[node1.Index].Nodes.Insert(0, node1);
					}
					else if (node2.Parent == null)
					{
						_treeView.Nodes.Insert(node2.Index + 1, node1);
					}
					else
					{
						node2.Parent.Nodes.Insert(node2.Index + 1, node1);
					}
				}

				_treeView.SelectedNode = node1;
				_currentNode = node1;
			}

			private void _moveUpButton_Click(object sender, EventArgs e)
			{
				TreeNode node1 = _currentNode;
				TreeNode node2 = _currentNode.Parent;

				if (node2 == null)
				{
					_treeView.Nodes.RemoveAt(node1.Index);
					_treeView.Nodes[node1.Index - 1].Nodes.Add(node1);
				}
				else
				{
					node2.Nodes.RemoveAt(node1.Index);
					
					if (node1.Index == 0)
					{
						if (node2.Parent == null)
						{
							_treeView.Nodes.Insert(node2.Index, node1);
						}
						else
						{
							node2.Parent.Nodes.Insert(node2.Index, node1);
						}
					}
					else
					{
						node2.Nodes[node1.Index - 1].Nodes.Add(node1);
					}
				}
				
				_treeView.SelectedNode = node1;
				_currentNode = node1;
			}

			private void _propertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
			{
				_propertyDescriptionLabel.Text = string.Format(Resources.Text_TreeNodeCollectionEditor_propertyDescriptionLabel, _treeView.SelectedNode.Text);
			}

			private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
			{
				_currentNode = e.Node;
				this.SetNodeProps(_currentNode);
				this.SetButtonsState();
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="EditorForm"/> class.
			/// </summary>
			public EditorForm(NuGenTreeNodeCollectionEditor editor)
				: base(editor)
			{
				_editor = editor;
				this.InitializeComponent();
				this.HookEvents();
				_intialNextNode = this.NextNode;
				this.SetButtonsState();
			}

			static EditorForm()
			{
				EditorForm._nextNodeKey = new object();
			}

			#endregion
		}
	}
}

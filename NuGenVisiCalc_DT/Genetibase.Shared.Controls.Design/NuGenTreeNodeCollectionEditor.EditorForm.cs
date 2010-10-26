/* -----------------------------------------------
 * NuGenTreeNodeCollectionEditor.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Design.Properties;
using Genetibase.Shared.Design;

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
			#region Properties.Private

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
						this.SetTreeViewProps(treeView);
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
				string nodeText = Resources.Text_TreeNodeCollectionEditor_node;
				TreeNode node = new NuGenTreeNode(nodeText + _nextNodeIndex.ToString(CultureInfo.InvariantCulture));
				node.Name = node.Text;

				if (parent == null)
				{
					_treeView.Nodes.Add(node);
				}
				else
				{
					parent.Nodes.Add(node);
					parent.Expand();
				}

				_nextNodeIndex++;

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
			 * SetNodeProps
			 */

			private void SetNodeProps(TreeNode node)
			{
				if (node != null)
				{
					_propertyBlock.SetTitle(
						string.Format(
							CultureInfo.InvariantCulture,
							Resources.Text_TreeNodeCollectionEditor_propertyTitle,
							node.Name
						)
					);
				}
				else
				{
					_propertyBlock.SetTitle(
						Resources.Text_TreeNodeCollectionEditor_propertyTitleNone
					);
				}

				_propertyBlock.SetSelectedObject(node);
			}

			/*
			 * SetButtonsState
			 */

			private void SetButtonsState()
			{
				bool enabled = _treeView.Nodes.Count > 0;

				_addChildButton.Enabled = enabled;
				_deleteButton.Enabled = enabled;
				_moveDownButton.Enabled = (enabled && ((_currentNode != _treeView.LastNode) || (_currentNode.Level > 0))) && (_currentNode != _treeView.Nodes[_treeView.Nodes.Count - 1]);
				_moveUpButton.Enabled = enabled && (_currentNode != _treeView.Nodes[0]);
			}

			/*
			 * SetTreeViewProps
			 */

			private void SetTreeViewProps(TreeView actualTreeView)
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
				TreeNode currentNode = _currentNode;
				TreeNode currentNodeParent = _currentNode.Parent;

				if (currentNodeParent == null)
				{
					_treeView.Nodes.RemoveAt(currentNode.Index);
					_treeView.Nodes[currentNode.Index].Nodes.Insert(0, currentNode);
				}
				else
				{
					currentNodeParent.Nodes.RemoveAt(currentNode.Index);
					
					if (currentNode.Index < currentNodeParent.Nodes.Count)
					{
						currentNodeParent.Nodes[currentNode.Index].Nodes.Insert(0, currentNode);
					}
					else if (currentNodeParent.Parent == null)
					{
						_treeView.Nodes.Insert(currentNodeParent.Index + 1, currentNode);
					}
					else
					{
						currentNodeParent.Parent.Nodes.Insert(currentNodeParent.Index + 1, currentNode);
					}
				}

				_treeView.SelectedNode = currentNode;
				_currentNode = currentNode;
			}

			private void _moveUpButton_Click(object sender, EventArgs e)
			{
				TreeNode currentNode = _currentNode;
				TreeNode currentNodeParent = _currentNode.Parent;

				if (currentNodeParent == null)
				{
					_treeView.Nodes.RemoveAt(currentNode.Index);
					_treeView.Nodes[currentNode.Index - 1].Nodes.Add(currentNode);
				}
				else
				{
					currentNodeParent.Nodes.RemoveAt(currentNode.Index);
					
					if (currentNode.Index == 0)
					{
						if (currentNodeParent.Parent == null)
						{
							_treeView.Nodes.Insert(currentNodeParent.Index, currentNode);
						}
						else
						{
							currentNodeParent.Parent.Nodes.Insert(currentNodeParent.Index, currentNode);
						}
					}
					else
					{
						currentNodeParent.Nodes[currentNode.Index - 1].Nodes.Add(currentNode);
					}
				}
				
				_treeView.SelectedNode = currentNode;
				_currentNode = currentNode;
			}

			private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
			{
				_currentNode = e.Node;
				this.SetNodeProps(_currentNode);
				this.SetButtonsState();
			}

			#endregion

			private NuGenCollectionEditorDialogBlock _dialogBlock;
			private NuGenCollectionEditorMainBlock _mainBlock;
			private NuGenCollectionEditorPropertyBlock _propertyBlock;

			private SplitContainer _splitContainer;

			private Button _addChildButton;
			private Button _addRootButton;
			private Button _deleteButton;
			private Button _moveDownButton;
			private Button _moveUpButton;

			private TreeNode _currentNode;
			private NodesEditorTreeView _treeView;

			private int _nextNodeIndex;

			private void InitializeComponent()
			{
				_addRootButton = new Button();
				_addChildButton = new Button();
				_deleteButton = new Button();
				_dialogBlock = new NuGenCollectionEditorDialogBlock();
				_mainBlock = new NuGenCollectionEditorMainBlock();
				_moveDownButton = _mainBlock.GetMoveDownButton();
				_moveUpButton = _mainBlock.GetMoveUpButton();
				_propertyBlock = new NuGenCollectionEditorPropertyBlock();
				_splitContainer = new SplitContainer();
				_treeView = new NodesEditorTreeView();

				this.SuspendLayout();

				/* DialogBlock */

				_dialogBlock.Dock = DockStyle.Bottom;
				_dialogBlock.Parent = this;
				_dialogBlock.TabIndex = 10;

				Button okButton = _dialogBlock.GetOkButton();
				Button cancelButton = _dialogBlock.GetCancelButton();
				okButton.Click += _okButton_Click;
				cancelButton.Click += _cancelButton_Click;

				/* SplitContainer */

				_splitContainer.Dock = DockStyle.Fill;
				_splitContainer.Panel1MinSize = 250;
				_splitContainer.Panel2MinSize = 100;
				_splitContainer.Parent = this;
				_splitContainer.BringToFront();

				/* MainBlock */

				_mainBlock.Dock = DockStyle.Fill;
				_mainBlock.Parent = _splitContainer.Panel1;
				_mainBlock.TabIndex = 20;
				_mainBlock.SetTitle(Resources.Text_TreeNodeCollectionEditor_mainTitle);

				/* Action buttons */

				_moveDownButton.Click += _moveDownButton_Click;
				_moveUpButton.Click += _moveUpButton_Click;

				_deleteButton.Dock = DockStyle.Top;
				_deleteButton.Image = Resources.Delete;
				_deleteButton.Click += _deleteButton_Click;

				Control.ControlCollection actionControls = _mainBlock.GetActionControls();
				actionControls.Add(_deleteButton);
				_deleteButton.BringToFront();

				/* Populate buttons */

				_addRootButton.Dock = DockStyle.Left;
				_addRootButton.TabIndex = 10;
				_addRootButton.Text = Resources.Text_TreeNodeCollectionEditor_addRootButton;
				_addRootButton.Click += _addRootButton_Click;

				_addChildButton.Dock = DockStyle.Left;
				_addChildButton.TabIndex = 20;
				_addChildButton.Text = Resources.Text_TreeNodeCollectionEditor_addChildButton;
				_addChildButton.Click += _addChildButton_Click;

				Control.ControlCollection populateControls = _mainBlock.GetPopulateControls();
				populateControls.Add(_addChildButton);
				populateControls.Add(_addRootButton);

				/* PropertyBlock */

				_propertyBlock.Dock = DockStyle.Fill;
				_propertyBlock.Parent = _splitContainer.Panel2;
				_propertyBlock.TabIndex = 30;
				_propertyBlock.SetTitle(Resources.Text_TreeNodeCollectionEditor_propertyTitleNone);
				_propertyBlock.SelectedObjectsChanged += _propertyBlock_SelectedObjectsChanged;

				/* TreeView */

				_treeView.Dock = DockStyle.Fill;
				_treeView.Parent = _mainBlock;
				_treeView.BringToFront();
				_treeView.AfterSelect += _treeView_AfterSelect;

				/* Form */

				NuGenCollectionEditorInitializer.InitializeEditorForm(this);
				this.AcceptButton = okButton;
				this.CancelButton = cancelButton;
				this.Size = new Size(580, 480);
				this.MinimumSize = this.Size;
				this.Text = Resources.Text_TreeNodeCollectionEditor_EditorForm;
				base.ResumeLayout(false);
			}

			void _propertyBlock_SelectedObjectsChanged(object sender, EventArgs e)
			{
				_treeView.PerformLayout();
			}

			public EditorForm(NuGenTreeNodeCollectionEditor editor)
				: base(editor)
			{
				this.InitializeComponent();
				this.SetButtonsState();
			}
		}
	}
}

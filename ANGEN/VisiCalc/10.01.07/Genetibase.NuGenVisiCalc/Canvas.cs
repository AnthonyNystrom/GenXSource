/* -----------------------------------------------
 * Canvas.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using VisiTypes = Genetibase.NuGenVisiCalc.Types;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Genetibase.NuGenVisiCalc.Programs;
using Genetibase.NuGenVisiCalc.ComponentModel;
using Genetibase.NuGenVisiCalc.Operators;
using Genetibase.NuGenVisiCalc.Properties;
using Genetibase.Shared.Drawing;
using Genetibase.NuGenVisiCalc.Params;
using Genetibase.Shared;

namespace Genetibase.NuGenVisiCalc
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal sealed class Canvas : Panel
	{
		#region Properties.Equation

		[NuGenSRCategory("Category_Equation")]
		public String Equation
		{
			get
			{
				return "";
			}
		}

		#endregion

		#region Properties.Operation

		private Boolean _removeOperations;

		[NuGenSRCategory("Category_Operation")]
		public Boolean RemoveOperations
		{
			get
			{
				return _removeOperations;
			}
			set
			{
				_removeOperations = value;
			}
		}

		private Boolean _selectOperations;

		[NuGenSRCategory("Category_Operation")]
		public Boolean SelectOperations
		{
			get
			{
				return _selectOperations;
			}
			set
			{
				_selectOperations = value;
			}
		}

		private Boolean _subOperations;

		[NuGenSRCategory("Category_Operation")]
		public Boolean SubOperations
		{
			get
			{
				return _subOperations;
			}
			set
			{
				_subOperations = value;
			}
		}

		private Boolean _testConnections;

		[NuGenSRCategory("Category_Operation")]
		public Boolean TestConnections
		{
			get
			{
				return _testConnections;
			}
			set
			{
				_testConnections = value;
			}
		}

		private Boolean _upOperations;

		[NuGenSRCategory("Category_Operation")]
		public Boolean UpOperations
		{
			get
			{
				return _upOperations;
			}
			set
			{
				_upOperations = value;
			}
		}

		#endregion

		#region Properties.Schema

		/*
		 * RoundRadius
		 */

		private Single _roundRadius;

		[NuGenSRCategory("Category_Schema")]
		public Single RoundRadius
		{
			get
			{
				return _roundRadius;
			}
			set
			{
				_roundRadius = value;
			}
		}

		/*
		 * Scale
		 */

		private Single _scale;

		[NuGenSRCategory("Category_Schema")]
		public new Single Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				_scale = value;
				_pictureBox.Invalidate();
			}
		}

		/*
		 * SchemaBackColor
		 */

		[NuGenSRCategory("Category_Schema")]
		public Color SchemaBackColor
		{
			get
			{
				return _pictureBox.BackColor;
			}
			set
			{
				_pictureBox.BackColor = value;
			}
		}

		/*
		 * SchemaSize
		 */

		[NuGenSRCategory("Category_Schema")]
		public Size SchemaSize
		{
			get
			{
				return _pictureBox.Size;
			}
			set
			{
				_pictureBox.Size = value;
			}
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * Program
		 */

		private Program _program;

		[Browsable(false)]
		public Program Program
		{
			get
			{
				return _program;
			}
			set
			{
				_program = value;
			}
		}

		/*
		 * Schema
		 */

		[Browsable(false)]
		public PictureBox Schema
		{
			get
			{
				return _pictureBox;
			}
		}

		/*
		 * SelectedNode
		 */

		private NodeBase _selectedNode;

		[Browsable(false)]
		public NodeBase SelectedNode
		{
			get
			{
				return _selectedNode;
			}
			set
			{
				_selectedNode = value;
			}
		}

		/*
		 * SelectedPort
		 */

		private NodePort _selectedPort;

		[Browsable(false)]
		public NodePort SelectedPort
		{
			get
			{
				return _selectedPort;
			}
			set
			{
				_selectedPort = value;
			}
		}

		/*
		 * SelectedProgram
		 */

		private Program _selectedProgram;

		[Browsable(false)]
		public Program SelectedProgram
		{
			get
			{
				return _selectedProgram;
			}
			set
			{
				_selectedProgram = value;
			}
		}

		#endregion

		#region Properties.Internal

		/*
		 * SchemaRectangle
		 */

		internal Rectangle SchemaRectangle
		{
			get
			{
				return new Rectangle(_pictureBox.Location, _pictureBox.Size);
			}
			set
			{
				_pictureBox.Location = value.Location;
				_pictureBox.Size = value.Size;
			}
		}

		#endregion

		#region Properties.Public.New

		public new Image BackgroundImage
		{
			get
			{
				return _pictureBox.BackgroundImage;
			}
			set
			{
				_pictureBox.BackgroundImage = value;
			}
		}

		public new ImageLayout BackgroundImageLayout
		{
			get
			{
				return _pictureBox.BackgroundImageLayout;
			}
			set
			{
				_pictureBox.BackgroundImageLayout = value;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(200, 200);

		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * AddNode
		 */

		public void AddNode(NodeBase nodeToAdd)
		{
			AddNode(nodeToAdd, new Point(0, 0));
		}

		public void AddNode(NodeBase nodeToAdd, Point position)
		{
			nodeToAdd.Update(position, CreateGraphics(), Font);
			_selectedProgram.AddNode(nodeToAdd);
			_pictureBox.Invalidate();
		}

		/*
		 * ClearNodes
		 */

		public void ClearNodes()
		{
			_selectedProgram.ClearNodes();
		}

		/*
		 * InsertSchema
		 */

		public void InsertSchema(String fileName)
		{
			_selectedNode = null;
			ProgramNode programNode = Program.Insert(fileName);
			programNode.SubProgram.ParentProgram = _selectedProgram;
			Point location = new Point(SchemaRectangle.X + 10, SchemaRectangle.Y + 10);
			programNode.Update(location, CreateGraphics(), Font);
			_selectedProgram.AddNode(programNode);
		}

		/*
		 * NewSchema
		 */

		public void NewSchema()
		{
			_program = new Program();
			_selectedProgram = _program;
			_selectedNode = null;
			_selectedPort = null;
		}

		/*
		 * OpenSchema
		 */

		public void OpenSchema(String fileName)
		{
			_program = Program.Load(fileName);
			_selectedProgram = _program;
			_selectedNode = null;
			_selectedPort = null;
		}

		/*
		 * RemoveNode
		 */

		public void RemoveNode(NodeBase nodeToRemove)
		{
			_selectedProgram.RemoveNode(nodeToRemove);
		}

		public void RemoveNode(Int32 index)
		{
			_selectedProgram.RemoveNode(index);
		}

		/*
		 * RemoveSelectedNode
		 */

		public void RemoveSelectedNode()
		{
			if (_selectedNode == null)
			{
				return;
			}

			RemoveNode(_selectedNode);
			_selectedNode = null;
		}

		/*
		 * SaveSchema
		 */

		public void SaveSchema(String fileName)
		{
			Program.Save(fileName, _program);
		}

		#endregion

		#region EventHandlers.PictureBox

		private void _pictureBox_DoubleClick(Object sender, EventArgs e)
		{
			if (_subOperations)
			{
				if (_selectedNode != null)
				{
					if (_selectedNode is ProgramNode)
					{
						_selectedProgram = ((ProgramNode)_selectedNode).SubProgram;
					}
				}
			}

			_pictureBox.Invalidate();
		}

		private void _pictureBox_DragDrop(Object sender, DragEventArgs e)
		{
			Type descriptorType = NuGenArgument.GetCompatibleDataObjectType(e.Data, typeof(IDescriptor));
			IDescriptor descriptor = (IDescriptor)e.Data.GetData(descriptorType);
			NodeBase nodeToAdd = descriptor.CreateNode(this);
			Point cp = _pictureBox.PointToClient(new Point(e.X, e.Y));
			AddNode(nodeToAdd, new Point((Int32)(cp.X / _scale), (Int32)(cp.Y / _scale)));
		}

		private void _pictureBox_DragEnter(Object sender, DragEventArgs e)
		{
			if (NuGenArgument.GetCompatibleDataObjectType(e.Data, typeof(IDescriptor)) != null)
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void _pictureBox_MouseDown(Object sender, MouseEventArgs e)
		{
			Point contextPosition = new Point(e.Location.X - HorizontalScroll.Value, e.Location.Y - VerticalScroll.Value);

			if (_selectedProgram != null)
			{
				Boolean go = true;
				_selectedNode = null;

				if (_selectedProgram.Nodes.Count > 0)
				{
					Int32 mx = Convert.ToInt32(e.X / _scale);
					Int32 my = Convert.ToInt32(e.Y / _scale);

					_px = mx;
					_px = my;

					if (e.Button == MouseButtons.Left && _selectOperations)
					{
						// Select node.
						// Loop backwards to select the one on top.
						for (Int32 i = _selectedProgram.Nodes.Count - 1; i >= 0; i--)
						{
							NodeBase node = _selectedProgram.Nodes[i];

							if (node.HeaderRectangle.IntersectsWith(new Rectangle(mx, my, 1, 1)))
							{
								_selectedNode = node;
								_px = mx - node.ClientRectangle.X;
								_py = my - node.ClientRectangle.Y;

								_selectedProgram.Nodes.Insert(_selectedProgram.Nodes.Count, _selectedNode);
								_selectedProgram.Nodes.RemoveAt(i);
								i = -1;
							}
						}

						// Connect port.
						if (_selectedPort != null)
						{
							for (Int32 i = 0; i < _selectedProgram.Nodes.Count; i++)
							{
								NodeBase node = _selectedProgram.Nodes[i];

								if (node.BodyRectangle.IntersectsWith(new Rectangle(mx, my, 1, 1)))
								{
									for (Int32 j = 0; j < node.OutputsLength; j++)
									{
										if (node.OutputRectCollection[j].IntersectsWith(new Rectangle(mx, my, 1, 1)))
										{
											if (_testConnections)
											{
												if (_selectedPort.IsCompatibleConnection(node.GetOutput(j)))
												{
													_selectedPort.Connection = node.GetOutput(j);
												}
											}
											else
											{
												_selectedPort.Connection = node.GetOutput(j);
											}
										}
									}
								}
							}
						}

						// Select port.
						_selectedPort = null;

						for (Int32 i = 0; i < _selectedProgram.Nodes.Count; i++)
						{
							NodeBase node = _selectedProgram.Nodes[i];

							if (node.BodyRectangle.IntersectsWith(new Rectangle(mx, my, 1, 1)))
							{
								for (Int32 j = 0; j < node.InputsLength; j++)
								{
									if (node.InputRectCollection[j].IntersectsWith(new Rectangle(mx, my, 1, 1)))
									{
										_selectedPort = node.GetInput(j);
									}
								}
							}
						}
					}

					if (e.Button == MouseButtons.Right && _removeOperations)
					{
						// Remove node
						for (Int32 i = 0; i < _selectedProgram.Nodes.Count; i++)
						{
							NodeBase node = _selectedProgram.Nodes[i];

							if (node.HeaderRectangle.IntersectsWith(new Rectangle(mx, my, 1, 1)))
							{
								_selectedNode = node;
							}
						}

						if (_selectedNode != null)
						{
							ContextMenu conextMenu = new ContextMenu();
							MenuItem deleteNodeMenuItem = new MenuItem(Resources.Text_DeleteNode);
							deleteNodeMenuItem.Click += delegate
							{
								RemoveNode(_selectedNode);
							};
							go = false;
							conextMenu.MenuItems.Add(deleteNodeMenuItem);
							conextMenu.Show(this, contextPosition);

						}

						// Remove connection
						_selectedPort = null;

						for (Int32 i = 0; i < _selectedProgram.Nodes.Count; i++)
						{
							NodeBase node = _selectedProgram.Nodes[i];

							if (node.BodyRectangle.IntersectsWith(new Rectangle(mx, my, 1, 1)))
							{
								for (Int32 j = 0; j < node.InputsLength; j++)
								{
									if (node.InputRectCollection[j].IntersectsWith(new Rectangle(mx, my, 1, 1)))
									{
										_selectedPort = node.GetInput(j);
									}
								}
							}
						}

						if (_selectedPort != null)
						{
							go = false;
							ContextMenu contextMenu = new ContextMenu();
							MenuItem removeConnectionMenuItem = new MenuItem(Resources.Text_RemoveConnection);
							removeConnectionMenuItem.Click += delegate
							{
								_selectedPort.Connection = null;
							};
							contextMenu.MenuItems.Add(removeConnectionMenuItem);
							contextMenu.Show(this, contextPosition);
						}
					}
				}

				if (e.Button == MouseButtons.Right && _upOperations)
				{
					// Go to parent program.
					if (go)
					{
						if (_selectedNode == null)
						{
							if (_selectedProgram.ParentProgram != null)
							{
								foreach (NodeBase node in _selectedProgram.ParentProgram.Nodes)
								{
									node.Update(new Point(node.HeaderRectangle.X, node.HeaderRectangle.Y), CreateGraphics(), Font);
								}

								_selectedProgram = _selectedProgram.ParentProgram;
							}
						}
					}
				}
			}

			_pictureBox.Invalidate();
			OnMouseDown(e);
		}

		private void _pictureBox_MouseMove(Object sender, MouseEventArgs e)
		{
			Boolean needRedraw = false;

			if (_selectedProgram != null)
			{
				if (_selectedProgram.Nodes.Count > 0)
				{
					if (e.Button == MouseButtons.Left && _selectOperations)
					{
						Int32 mx = Convert.ToInt32(e.X / _scale);
						Int32 my = Convert.ToInt32(e.Y / _scale);

						for (Int32 i = 0; i < _selectedProgram.Nodes.Count; i++)
						{
							NodeBase node = _selectedProgram.Nodes[i];

							if (_selectedNode == node)
							{
								node.Update(new Point(Math.Max(Math.Min(mx - _px, (Int32)((Single)_pictureBox.Width / _scale) - node.ClientRectangle.Width), 0), Math.Max(Math.Min(my - _py, (Int32)((Single)_pictureBox.Height / _scale) - node.ClientRectangle.Height), 0)), CreateGraphics(), Font);
								needRedraw = true;
							}
						}
					}
				}
			}

			if (needRedraw)
			{
				_pictureBox.Invalidate();
			}
		}

		private void _pictureBox_Paint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			if (_selectedProgram != null)
			{
				if (_selectedProgram.Nodes.Count > 0)
				{
					g.SmoothingMode = SmoothingMode.HighQuality;
					g.PixelOffsetMode = PixelOffsetMode.HighQuality;
					g.ScaleTransform(_scale, _scale);

					// Draw connections
					for (Int32 i = 0; i < _selectedProgram.Nodes.Count; i++)
					{
						NodeBase node = _selectedProgram.Nodes[i];
						node.DrawConnections(g, Color.White, _selectedPort);
					}

					// Draw nodes
					for (Int32 i = 0; i < _selectedProgram.Nodes.Count; i++)
					{
						NodeBase node = _selectedProgram.Nodes[i];
						node.DrawNode(g, _roundRadius, Object.ReferenceEquals(node, _selectedNode));

						// Draw _selectedPort/Input.
						for (Int32 j = 0; j < node.InputsLength; j++)
						{
							if (node.GetInput(j) == _selectedPort)
							{
								NuGenControlPaint.DrawRoundRectangle(g, Pens.White, node.InputRectCollection[j], 3);
							}
						}
					}
				}
			}
		}

		#endregion

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			UpdatePictureBoxLocation();
		}

		private void UpdatePictureBoxLocation()
		{
			if (_pictureBox.Width < Width)
			{
				_pictureBox.Left = (Width - _pictureBox.Width) / 2;
			}
			else
			{
				_pictureBox.Left = 20;
			}

			if (_pictureBox.Height < Height)
			{
				_pictureBox.Top = (Height - _pictureBox.Height) / 2;
			}
			else
			{
				_pictureBox.Top = 20;
			}

			AutoScrollMargin = Size.Empty;
			AutoScrollMinSize = new Size(_pictureBox.Width, _pictureBox.Height);
		}

		private PictureBox _pictureBox;
		private Int32 _px, _py;

		/// <summary>
		/// Initializes a new instance of the <see cref="Canvas"/> class.
		/// </summary>
		public Canvas()
		{
			_pictureBox = new PictureBox();
			_pictureBox.AllowDrop = true;
			_pictureBox.BackColor = Color.White;
			_pictureBox.BorderStyle = BorderStyle.FixedSingle;
			_pictureBox.Size = new Size(512, 512);
			_pictureBox.Parent = this;
			_pictureBox.DoubleClick += _pictureBox_DoubleClick;
			_pictureBox.DragEnter += _pictureBox_DragEnter;
			_pictureBox.DragDrop += _pictureBox_DragDrop;
			_pictureBox.MouseDown += _pictureBox_MouseDown;
			_pictureBox.MouseMove += _pictureBox_MouseMove;
			_pictureBox.Paint += _pictureBox_Paint;

			_program = new Program();
			_selectedProgram = _program;
			_scale = 1;
			_roundRadius = 5;

			_selectOperations = _removeOperations = true;
			_subOperations = _upOperations = true;
			_testConnections = true;

			AutoScroll = true;
			BackColor = SystemColors.AppWorkspace;

			UpdatePictureBoxLocation();
		}

		protected override void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				if (_pictureBox != null && !_pictureBox.IsDisposed)
				{
					_pictureBox.DoubleClick -= _pictureBox_DoubleClick;
					_pictureBox.DragEnter -= _pictureBox_DragEnter;
					_pictureBox.DragDrop -= _pictureBox_DragDrop;
					_pictureBox.MouseDown -= _pictureBox_MouseDown;
					_pictureBox.MouseMove -= _pictureBox_MouseMove;
					_pictureBox.Paint -= _pictureBox_Paint;
				}
			}

			base.Dispose(disposing);
		}
	}
}

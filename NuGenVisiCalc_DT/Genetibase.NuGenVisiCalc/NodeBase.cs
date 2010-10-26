/* -----------------------------------------------
 * NodeBase.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using Genetibase.NuGenVisiCalc.ComponentModel;
using Genetibase.NuGenVisiCalc.Programs;
using Genetibase.Shared.Drawing;

namespace Genetibase.NuGenVisiCalc
{
	[Serializable]
	internal abstract partial class NodeBase
	{
		#region Properties.Appearance

		/*
		 * BodyBackColor
		 */

		private Color _bodyBackColor;

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Color BodyBackColor
		{
			get
			{
				if (_bodyBackColor == Color.Empty)
				{
					return DefaultBodyBackColor;
				}

				return _bodyBackColor;
			}
			set
			{
				_bodyBackColor = value;
			}
		}

		private Color DefaultBodyBackColor
		{
			get
			{
				return Control.DefaultBackColor;
			}
		}

		private void ResetBodyBackColor()
		{
			BodyBackColor = DefaultBodyBackColor;
		}

		private Boolean ShouldSerializeBodyBackColor()
		{
			return BodyBackColor != DefaultBodyBackColor;
		}

		/*
		 * BodyForeColor
		 */

		private Color _bodyForeColor;

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Color BodyForeColor
		{
			get
			{
				if (_bodyForeColor == Color.Empty)
				{
					return DefaultBodyForeColor;
				}

				return _bodyForeColor;
			}
			set
			{
				_bodyForeColor = value;
			}
		}

		private Color DefaultBodyForeColor
		{
			get
			{
				return Control.DefaultForeColor;
			}
		}

		private void ResetBodyForeColor()
		{
			BodyForeColor = DefaultBodyForeColor;
		}

		private Boolean ShouldSerializeBodyForeColor()
		{
			return BodyForeColor != DefaultBodyForeColor;
		}

		/*
		 * BodyFont
		 */

		private Font _bodyFont;

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Font BodyFont
		{
			get
			{
				if (_bodyFont == null)
				{
					return DefaultBodyFont;
				}

				return _bodyFont;
			}
			set
			{
				_bodyFont = value;
			}
		}

		private Font DefaultBodyFont
		{
			get
			{
				return Control.DefaultFont;
			}
		}

		private void ResetBodyFont()
		{
			BodyFont = DefaultBodyFont;
		}

		private Boolean ShouldSerializeBodyFont()
		{
			return BodyFont != DefaultBodyFont;
		}

		/*
		 * HeaderBackColor
		 */

		private Color _headerBackColor;

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Color HeaderBackColor
		{
			get
			{
				if (_headerBackColor == Color.Empty)
				{
					return DefaultHeaderBackColor;
				}

				return _headerBackColor;
			}
			set
			{
				_headerBackColor = value;
			}
		}

		private Color DefaultHeaderBackColor
		{
			get
			{
				return Color.Blue;
			}
		}

		private void ResetHeaderBackColor()
		{
			HeaderBackColor = DefaultHeaderBackColor;
		}

		private Boolean ShouldSerializeHeaderBackColor()
		{
			return HeaderBackColor != DefaultHeaderBackColor;
		}

		/*
		 * HeaderForeColor
		 */

		private Color _headerForeColor;

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Color HeaderForeColor
		{
			get
			{
				if (_headerForeColor == Color.Empty)
				{
					return DefaultHeaderForeColor;
				}

				return _headerForeColor;
			}
			set
			{
				_headerForeColor = value;
			}
		}

		private Color DefaultHeaderForeColor
		{
			get
			{
				return Color.White;
			}
		}

		private void ResetHeaderForeColor()
		{
			HeaderForeColor = DefaultHeaderForeColor;
		}

		private Boolean ShouldSerializeHeaderForeColor()
		{
			return HeaderForeColor != DefaultHeaderForeColor;
		}

		/*
		 * HeaderFont
		 */

		private Font _headerFont;

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Font HeaderFont
		{
			get
			{
				if (_headerFont == null)
				{
					return DefaultHeaderFont;
				}

				return _headerFont;
			}
			set
			{
				_headerFont = value;
			}
		}

		private Font DefaultHeaderFont
		{
			get
			{
				return Control.DefaultFont;
			}
		}

		private void ResetHeaderFont()
		{
			HeaderFont = DefaultHeaderFont;
		}

		private Boolean ShouldSerializeHeaderFont()
		{
			return HeaderFont != DefaultHeaderFont;
		}

		/*
		 * HeaderGradientStyle
		 */

		private NodeGradientStyle _headerGradientStyle = NodeGradientStyle.TopToBottom;

		[Browsable(true)]
		[DefaultValue(NodeGradientStyle.TopToBottom)]
		[NuGenSRCategory("Category_Appearance")]
		public NodeGradientStyle HeaderGradientStyle
		{
			get
			{
				return _headerGradientStyle;
			}
			set
			{
				_headerGradientStyle = value;
			}
		}

		/*
		 * OutlineColor
		 */

		private Color _outlineColor;

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Color OutlineColor
		{
			get
			{
				if (_outlineColor == Color.Empty)
				{
					return DefaultOutlineColor;
				}

				return _outlineColor;
			}
			set
			{
				_outlineColor = value;
			}
		}

		private Color DefaultOutlineColor
		{
			get
			{
				return Color.LightBlue;
			}
		}

		private void ResetOutlineColor()
		{
			OutlineColor = DefaultOutlineColor;
		}

		private Boolean ShouldSerializeOutlineColor()
		{
			return OutlineColor != DefaultOutlineColor;
		}

		/*
		 * Size
		 */

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Size Size
		{
			get
			{
				return new Size(Width, Height);
			}
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		/*
		 * Title
		 */

		private String _header;

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public String Header
		{
			get
			{
				return _header;
			}
			set
			{
				_header = value;
			}
		}

		#endregion

		#region Properties.Format

		/*
		 * DisplayFormat
		 */

		private ValueDisplayFormat _displayFormat;

		[Browsable(true)]
		[NuGenSRCategory("Category_Format")]
		public ValueDisplayFormat DisplayFormat
		{
			get
			{
				return _displayFormat;
			}
			set
			{
				_displayFormat = value;
			}
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * BodyRectangle
		 */

		private Rectangle _bodyRectangle;

		[Browsable(false)]
		public Rectangle BodyRectangle
		{
			get
			{
				return _bodyRectangle;
			}
			protected set
			{
				_bodyRectangle = value;
			}
		}

		/*
		 * ClientRectangle
		 */

		private Rectangle _clientRectangle;

		[Browsable(false)]
		public Rectangle ClientRectangle
		{
			get
			{
				return _clientRectangle;
			}
			protected set
			{
				_clientRectangle = value;
			}
		}

		/*
		 * ContainingProgram
		 */

		private Program _containingProgram;

		[Browsable(false)]
		public Program ContainingProgram
		{
			get
			{
				return _containingProgram;
			}
			set
			{
				_containingProgram = value;
			}
		}

		/*
		 * HeaderRectangle
		 */

		private Rectangle _headerRectangle;

		[Browsable(false)]
		public Rectangle HeaderRectangle
		{
			get
			{
				return _headerRectangle;
			}
			protected set
			{
				_headerRectangle = value;
			}
		}

		/*
		 * Height
		 */

		private Int32 _height;

		[Browsable(false)]
		public Int32 Height
		{
			get
			{
				return _height;
			}
			set
			{
				_height = value;
			}
		}

		/*
		 * Inputs
		 */

		private NodePort[] _inputs;

		[Browsable(false)]
		public NodePort[] Inputs
		{
			get
			{
				return _inputs;
			}
		}

		/*
		 * InputsLength
		 */

		public Int32 InputsLength
		{
			get
			{
				if (_inputs == null)
				{
					return 0;
				}

				return _inputs.Length;
			}
		}

		/*
		 * InputRectCollection
		 */

		private Rectangle[] _inputRectCollection;

		[Browsable(false)]
		public Rectangle[] InputRectCollection
		{
			get
			{
				return _inputRectCollection;
			}
		}

		/*
		 * Name
		 */

		private String _name;

		[Browsable(false)]
		public String Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		/*
		 * Outputs
		 */

		private NodePort[] _outputs;

		[Browsable(false)]
		public NodePort[] Outputs
		{
			get
			{
				return _outputs;
			}
		}

		/*
		 * OutputsLength
		 */

		[Browsable(false)]
		public Int32 OutputsLength
		{
			get
			{
				if (_outputs == null)
				{
					return 0;
				}

				return _outputs.Length;
			}
		}

		/*
		 * OutputRectCollection
		 */

		private Rectangle[] _outputRectCollection;

		[Browsable(false)]
		public Rectangle[] OutputRectCollection
		{
			get
			{
				return _outputRectCollection;
			}
		}

		/*
		 * Width
		 */

		private Int32 _width;

		[Browsable(false)]
		public Int32 Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * CreateInputs
		 */

		public void CreateInputs(Int32 inputCount)
		{
			_inputs = new NodePort[inputCount];
		}

		/*
		 * CreateOutputs
		 */

		public void CreateOutputs(Int32 outputCount)
		{
			_outputs = new NodePort[outputCount];
		}

		/*
		 * Execute
		 */

		public virtual Boolean Execute()
		{
			return false;
		}

		public virtual Boolean Execute(params Object[] options)
		{
			return false;
		}

		/*
		 * GetData
		 */

		public virtual Object GetData()
		{
			return null;
		}

		public virtual Object GetData(params Object[] options)
		{
			return null;
		}

		public virtual Object GetData(Int32 index)
		{
			return null;
		}

		public virtual Object GetData(Int32 index, params Object[] options)
		{
			return null;
		}

		/*
		 * GetInput
		 */

		public NodePort GetInput(Int32 index)
		{
			return _inputs[index];
		}

		/*
		 * GetInputData
		 */

		public virtual Object GetInputData(Int32 index)
		{
			if (_inputs[index] == null || _inputs[index].Connection == null)
			{
				return null;
			}

			return _inputs[index].Connection.Node.GetData(_inputs[index].Connection.Index);
		}

		public virtual Object GetInputData(Int32 index, params Object[] options)
		{
			if (_inputs[index] == null || _inputs[index].Connection == null)
			{
				return null;
			}

			return _inputs[index].Connection.Node.GetData(_inputs[index].Connection.Index, options);
		}

		/*
		 * GetOutput
		 */

		public NodePort GetOutput(Int32 index)
		{
			return _outputs[index];
		}

		/*
		 * SetData
		 */

		public virtual void SetData(Object data)
		{
		}

		public virtual void SetData(Int32 index, Object data)
		{
		}

		/*
		 * SetInput
		 */

		public void SetInput(Int32 index, String name)
		{
			SetInput(index, name, null, null);
		}

		public void SetInput(Int32 index, String name, Object data)
		{
			SetInput(index, name, data, null);
		}

		public void SetInput(Int32 index, String name, NodePort connection)
		{
			SetInput(index, name, null, connection);
		}

		private void SetInput(Int32 index, String name, Object data, NodePort connection)
		{
			_inputs[index] = new NodePort(this, index);
			_inputs[index].Name = name;
			_inputs[index].Data = data;
			_inputs[index].Connection = connection;
		}

		/*
		 * SetOutput
		 */

		public void SetOutput(Int32 index, String name)
		{
			SetOutput(index, name, null);
		}

		public void SetOutput(Int32 index, String name, Object data)
		{
			_outputs[index] = new NodePort(this, index);
			_outputs[index].Name = name;
			_outputs[index].Data = data;
		}

		/*
		 * SetOutputData
		 */

		public void SetOutputData(Int32 index, Object data)
		{
			_outputs[index].Data = data;
		}

		#endregion

		#region Methods.Drawing

		/*
		 * DrawConnections
		 */

		private static readonly Pen _connectionPen = new Pen(Color.FromArgb(88, 0, 0, 0), 2);

		public virtual void DrawConnections(Graphics g, Color outlineColor, NodePort selectedPort)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			for (Int32 i = 0; i < InputsLength; i++)
			{
				if (GetInput(i).Connection != null)
				{
					Color color = Color.Black;

					if (Object.ReferenceEquals(GetInput(i), selectedPort))
					{
						color = outlineColor;
					}

					NodeBase currentNode = GetInput(i).Connection.Node;
					Int32 connectionIndex = 0;

					for (Int32 j = 0; j < currentNode.OutputsLength; j++)
					{
						if (Object.ReferenceEquals(currentNode.GetOutput(j), GetInput(i).Connection))
						{
							connectionIndex = j;
							break;
						}
					}

					/* Calculate extreme points. */

					Point output = new Point(
						currentNode.BodyRectangle.X + currentNode.BodyRectangle.Width
						, currentNode.BodyRectangle.Y + connectionIndex * currentNode.HeaderRectangle.Height + currentNode.HeaderRectangle.Height / 2 + 1
					);

					Point input = new Point(
						BodyRectangle.X
						, BodyRectangle.Y + i * HeaderRectangle.Height + HeaderRectangle.Height / 2 + 1
					);


					/* Draw connection shadow and the connection itself. */

					g.DrawBezier(
						_connectionPen
						, input
						, new Point(input.X - 26, input.Y + 4)
						, new Point(output.X + 34, output.Y + 4)
						, output
					);

					using (SolidBrush sb = new SolidBrush(color))
					using (Pen pen = new Pen(sb, 2f))
					{
						g.DrawBezier(pen, input, new Point(input.X - 30, input.Y), new Point(output.X + 30, output.Y), output);
						g.FillPolygon(sb, new Point[] { new Point(input.X - 5, input.Y - 5), input, new Point(input.X - 5, input.Y + 5) });
					}
				}
			}
		}

		/*
		 * DrawNode
		 */

		private static readonly Brush _nodeBrush = new SolidBrush(Color.FromArgb(44, 0, 0, 0));

		public virtual void DrawNode(Graphics g, Single roundRadius, Boolean selected)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			Int32 x = ClientRectangle.X;
			Int32 y = ClientRectangle.Y;
			Int32 width = ClientRectangle.Width;
			Int32 height = ClientRectangle.Height;

			Rectangle[] rectArray = new Rectangle[3]
			{
				new Rectangle(x + 1, y + 1, width, height)
				, new Rectangle(x + 2, y + 2, width, height)
				, new Rectangle(x + 3, y + 3, width, height)
			};

			Boolean flag = roundRadius > 0;

			foreach (Rectangle rect in rectArray)
			{
				if (flag)
				{
					NuGenControlPaint.FillRoundRectangle(g, _nodeBrush, rect, roundRadius);
				}
				else
				{
					g.FillRectangle(_nodeBrush, rect);
				}
			}

			/* Header */

			using (LinearGradientBrush lgb = new LinearGradientBrush(HeaderRectangle, HeaderBackColor, Color.White, LinearGradientMode.Horizontal))
			{
				if (roundRadius > 0)
				{
					NuGenControlPaint.FillRoundRectangle(g, lgb, HeaderRectangle, roundRadius, NuGenRoundRectangleStyle.TopRound);
				}
				else
				{
					g.FillRectangle(lgb, HeaderRectangle);
				}
			}

			/* Header text. */

			using (SolidBrush sb = new SolidBrush(HeaderForeColor))
			{
				g.DrawString(Header, HeaderFont, sb, HeaderRectangle.X + 2, HeaderRectangle.Y + 2);
			}

			/* Fill the body background. */

			using (LinearGradientBrush lgb = new LinearGradientBrush(BodyRectangle, BodyBackColor, Color.White, LinearGradientMode.Vertical))
			{
				if (roundRadius > 0)
				{
					NuGenControlPaint.FillRoundRectangle(g, lgb, BodyRectangle, roundRadius, NuGenRoundRectangleStyle.BottomRound);
				}
				else
				{
					g.FillRectangle(lgb, BodyRectangle);
				}
			}

			/* Draw node inputs and outputs. */

			using (SolidBrush sb = new SolidBrush(BodyForeColor))
			{
				for (Int32 i = 0; i < OutputsLength; i++)
				{
					Rectangle outputRect = OutputRectCollection[i];
					g.DrawString(String.Format(CultureInfo.CurrentUICulture, "{0}<", GetOutput(i).Name), BodyFont, sb, outputRect.X, outputRect.Y);
				}

				for (Int32 i = 0; i < InputsLength; i++)
				{
					Rectangle inputRect = InputRectCollection[i];
					g.DrawString(String.Format(CultureInfo.CurrentUICulture, ">{0}", GetInput(i).Name), BodyFont, sb, inputRect.X, inputRect.Y);
				}
			}

			/* Draw outline. */

			if (selected)
			{
				using (Pen pen = new Pen(OutlineColor, 2))
				{
					DrawNodeOutline(g, pen, ClientRectangle, roundRadius);
				}
			}
			else
			{
				using (Pen pen = new Pen(Color.FromArgb(64, OutlineColor), 1))
				{
					DrawNodeOutline(g, pen, ClientRectangle, roundRadius);
				}
			}
		}

		private void DrawNodeOutline(Graphics g, Pen pen, Rectangle nodeRect, Single roundRadius)
		{
			if (roundRadius > 0)
			{
				NuGenControlPaint.DrawRoundRectangle(g, pen, nodeRect, roundRadius);
			}
			else
			{
				g.DrawRectangle(pen, nodeRect);
			}
		}

		/*
		 * Update
		 */

		/// <summary>
		/// </summary>
		/// <param name="position"></param>
		/// <param name="g"></param>
		/// <param name="font"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="font"/> is <see langword="null"/>.</para>
		/// </exception>
		public virtual void Update(Point position, Graphics g, Font font)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (font == null)
			{
				throw new ArgumentNullException("font");
			}

			SizeF titleSize = g.MeasureString(Header, font);
			Int32 maxOutLength = 0;

			for (Int32 i = 0; i < OutputsLength; i++)
			{
				String outputName = GetOutput(i).Name;
				Single currentWidth = g.MeasureString(String.Format(CultureInfo.CurrentUICulture, "{0}<", outputName), BodyFont).Width;
				maxOutLength = Math.Max(maxOutLength, Convert.ToInt32(currentWidth));
			}

			Int32 maxInLength = 0;

			for (Int32 i = 0; i < InputsLength; i++)
			{
				String inputName = GetInput(i).Name;
				Single currentWidth = g.MeasureString(inputName, BodyFont).Width;
				maxInLength = Math.Max(maxInLength, Convert.ToInt32(currentWidth));
			}

			Int32 nodeWidth = Math.Max(Convert.ToInt32(titleSize.Width), maxInLength + maxOutLength + 8) + 8;

			if (nodeWidth > Width)
			{
				Width = nodeWidth;
			}

			nodeWidth = Math.Max(nodeWidth, Width);
			Int32 titleHeight = Convert.ToInt32(titleSize.Height);

			HeaderRectangle = new Rectangle(position, new Size(nodeWidth, titleHeight + 4));
			Int32 bodyHeight = Math.Max(Math.Max(Math.Max(OutputsLength, InputsLength) * (titleHeight + 4), 1), Height - HeaderRectangle.Height);
			BodyRectangle = new Rectangle(HeaderRectangle.X, HeaderRectangle.Y + HeaderRectangle.Height, nodeWidth, bodyHeight);
			ClientRectangle = new Rectangle(position.X, position.Y, BodyRectangle.Width, HeaderRectangle.Height + BodyRectangle.Height);

			if (ClientRectangle.Height > Height)
			{
				Height = ClientRectangle.Height;
			}

			_outputRectCollection = new Rectangle[OutputsLength];

			for (Int32 i = 0; i < OutputsLength; i++)
			{
				String outputName = GetOutput(i).Name;
				SizeF currentOutputSize = g.MeasureString(String.Format(CultureInfo.CurrentUICulture, "{0}<", outputName), BodyFont);
				Int32 x = BodyRectangle.X + BodyRectangle.Width - Convert.ToInt32(currentOutputSize.Width);
				Int32 y = BodyRectangle.Y + i * HeaderRectangle.Height + 2;
				Int32 width = Convert.ToInt32(currentOutputSize.Width);
				Int32 height = Convert.ToInt32(currentOutputSize.Height);

				_outputRectCollection[i] = new Rectangle(x, y, width, height);
			}

			_inputRectCollection = new Rectangle[InputsLength];

			for (Int32 i = 0; i < InputsLength; i++)
			{
				String inputName = GetInput(i).Name;
				SizeF currentInputSize = g.MeasureString(String.Format(CultureInfo.CurrentUICulture, ">{0}", inputName), BodyFont);
				_inputRectCollection[i] = new Rectangle(
					BodyRectangle.X + 2
					, BodyRectangle.Y + i * HeaderRectangle.Height + 2
					, Convert.ToInt32(currentInputSize.Width)
					, Convert.ToInt32(currentInputSize.Height)
				);
			}
		}

		#endregion

		public NodeBase()
			: this("Unnamed")
		{
		}

		public NodeBase(String name)
		{
			Header = Name = name;
		}
	}
}

/* -----------------------------------------------
 * NuGenFormula.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.MathX.FormulaInterpreter;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.MathX
{
	/// <summary>
	/// Encapsulates formula associated data.
	/// </summary>
	public class NuGenFormula
	{
		private NuGenPlotInterval _interval;

		/// <summary>
		/// </summary>
		public NuGenPlotInterval Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				_interval = value;
			}
		}

		private NuGenPlotInterval _paintValidate;

		/// <summary>
		/// </summary>
		public NuGenPlotInterval PaintValidate
		{
			get
			{
				return _paintValidate;
			}
			set
			{
				_paintValidate = value;
			}
		}

		private double _param;

		/// <summary>
		/// </summary>
		public double Param
		{
			get
			{
				return _param;
			}
		}

		private GraphicsPath _pth;
		private GraphicsPath _pthasympt;
		private Color _frb;
		private bool isparam, _draw;
		private string _formel;
		private NuGenFormulaCollectionBase _children;
		private NuGenFormulaElement _formulaElement;

		private NuGenFormula(string formel, NuGenPlotInterval intervall, Color frb, bool draw, NuGenFormula[] children, double param)
		{
			_formel = formel;
			_children = new NuGenFormulaCollectionBase();
			if (children != null)
				_children.AddRange(children);
			this._param = param;
			this._formulaElement = NuGenInterpreter.ParseInfixExpression(formel);
			this._interval = intervall;
			this.frb = frb;
			this._paintValidate = new NuGenPlotInterval(0.0, 0.0);
			this.draw = draw;
			_pth = new GraphicsPath();
			_pthasympt = new GraphicsPath();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFormula"/> class.
		/// </summary>
		public NuGenFormula(string formel, NuGenPlotInterval intervall, Color frb, bool draw, double param)
			: this(formel, intervall, frb, draw, null, param)
		{
			this.isparam = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFormula"/> class.
		/// </summary>
		public NuGenFormula(string formel, NuGenPlotInterval intervall, Color frb, bool draw, NuGenFormula[] children)
			: this(formel, intervall, frb, draw, children, 0.0)
		{
			this.isparam = children == null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFormula"/> class.
		/// </summary>
		public NuGenFormula(string formel, NuGenPlotInterval intervall, Color frb)
			: this(formel, intervall, frb, true, null, 0.0)
		{
			this.isparam = false;
		}

		/// <summary>
		/// </summary>
		public NuGenFormulaCollectionBase Children
		{
			get
			{
				return _children;
			}
		}

		/// <summary>
		/// </summary>
		public bool HasChildren
		{
			get
			{
				return _children.Count > 0;
			}
		}

		/// <summary>
		/// </summary>
		public string Formula
		{
			get
			{
				return _formel;
			}
			set
			{
				_formulaElement = NuGenInterpreter.ParseInfixExpression(_formel = value);
				_paintValidate = new NuGenPlotInterval(0.0, 0.0);
			}
		}

		/// <summary>
		/// </summary>
		public void SetChildrenColor(Color color)
		{
			if (_children.Count == 0)
				return;
			foreach (NuGenFormula fml in _children)
				fml.frb = color;
		}

		/// <summary>
		/// </summary>
		public bool draw
		{
			get
			{
				return _draw;
			}
			set
			{
				_draw = value;
				if (_children.Count == 0)
					return;
				foreach (NuGenFormula fml in _children)
					fml.draw = value;
			}
		}

		/// <summary>
		/// </summary>
		public Color frb
		{
			get
			{
				return _frb;
			}
			set
			{
				_frb = value;
				if (_children.Count == 0)
					return;
				foreach (NuGenFormula fml in _children)
					fml.frb = value;
			}
		}

		/// <summary>
		/// </summary>
		public NuGenFormulaElement _ve
		{
			get
			{
				return _formulaElement;
			}
		}

		/// <summary>
		/// </summary>
		public TreeNode TreeNode
		{
			get
			{
				TreeNode ret = new TreeNode("", 0, 1);
				if (isparam)
					ret.Text = "a=" + _param.ToString("0.00");
				else
					ret.Text = "f(x)=" + _formel;
				ret.ImageIndex = 0;
				ret.Checked = draw;
				if (HasChildren)
					ret.Nodes.AddRange(_children.GetTreeNodes());
				return ret;
			}
		}

		/// <summary>
		/// </summary>
		public GraphicsPath pth
		{
			get
			{
				return _pth;
			}
		}

		/// <summary>
		/// </summary>
		public GraphicsPath pthasympt
		{
			get
			{
				return _pthasympt;
			}
		}

		/// <summary>
		/// </summary>
		public bool IsParameter
		{
			get
			{
				return isparam;
			}
		}
	}
}

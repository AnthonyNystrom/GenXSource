/* -----------------------------------------------
 * NuGenGraphLine.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Meters.ComponentModel;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace Genetibase.Meters
{
	/// <summary>
	/// Defines a graph line.
	/// </summary>
	public class NuGenGraphLine
	{
		#region Properties.Appearance

		private Color _lineColor;

		/// <summary>
		/// Gets or sets the line color.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[DefaultValue(typeof(Color), "Green")]
		[NuGenSRDescription("Description_LineColor")]
		public Color LineColor
		{
			get
			{
				if (_lineColor == Color.Empty)
				{
					return Color.Green;
				}

				return _lineColor;
			}
			set
			{
				if (_lineColor != value)
					_lineColor = value;
			}
		}

		/*
		 * LineColor
		 */

		private Boolean _isBar;

		/// <summary>
		/// Gets or sets the value indicating whether to show the graph as bars.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[DefaultValue(false)]
		[NuGenSRDescription("Description_IsBar")]
		public Boolean IsBar
		{
			get
			{
				return _isBar;
			}
			set
			{
				if (_isBar != value)
					_isBar = value;
			}
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * Index
		 */

		/// <summary>
		/// Determines the Id of the graph line in the collection.
		/// </summary>
		private Int32 _index;

		/// <summary>
		/// Gets the Id of the graph line in the collection.
		/// </summary>
		[Browsable(false)]
		public Int32 Index
		{
			get
			{
				return _index;
			}
			set
			{
				_index = value;
			}
		}

		/*
		 * Values
		 */

		private List<Single> _values;

		/// <summary>
		/// Gets an array of values for this graph line.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<Single> Values
		{
			get
			{
				if (_values == null)
				{
					_values = new List<Single>();
				}

				return _values;
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGraphLine"/> class.
		/// </summary>
		public NuGenGraphLine()
			: this(0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <c>Genetibase.UI.NuGenGraphLine</c> class.
		/// </summary>
		/// <param name="id">The Id of the graph line in the collection.</param>
		public NuGenGraphLine(Int32 id)
		{
			_index = id;
		}
	}
}

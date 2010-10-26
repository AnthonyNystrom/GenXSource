/* -----------------------------------------------
 * NuGenGraphLine.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.UI.NuGenMeters.ComponentModel;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Defines a graph line.
	/// </summary>
	public class NuGenGraphLine
	{
		#region Properties.Appearance

		/// <summary>
		/// Determines the line color.
		/// </summary>
		private Color lineColor = Color.Green;

		/// <summary>
		/// Gets or sets the line color.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[DefaultValue(typeof(Color), "Green")]
		[NuGenSRDescription("LineColorDescription")]
		public Color LineColor
		{
			get { return this.lineColor; }
			set { if (this.lineColor != value) this.lineColor = value; }
		}

		/*
		 * LineColor
		 */

		/// <summary>
		/// Indicates whether to show the graph as bars.
		/// </summary>
		private bool isBar = false;

		/// <summary>
		/// Gets or sets the value indicating whether to show the graph as bars.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[DefaultValue(false)]
		[NuGenSRDescription("IsBarDescription")]
		public bool IsBar
		{
			get { return this.isBar; }
			set { if (this.isBar != value) this.isBar = value; }
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * Index
		 */

		/// <summary>
		/// Determines the Id of the graph line in the collection.
		/// </summary>
		private int index = 0;

		/// <summary>
		/// Gets the Id of the graph line in the collection.
		/// </summary>
		[Browsable(false)]
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		/*
		 * Values
		 */

		/// <summary>
		/// Defines an array of values for this graph line.
		/// </summary>
		private List<float> values = new List<float>();

		/// <summary>
		/// Gets an array of values for this graph line.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<float> Values
		{
			get { return this.values; }
		}

		#endregion
		
		#region Constructors

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
		public NuGenGraphLine(int id)
		{
			this.index = id;
		}

		#endregion
	}
}

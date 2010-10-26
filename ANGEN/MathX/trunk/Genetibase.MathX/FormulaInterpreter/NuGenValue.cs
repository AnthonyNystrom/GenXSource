/* -----------------------------------------------
 * NuGenValue.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.MathX.FormulaInterpreter
{
	/// <summary>
	/// </summary>
	public class NuGenValue : NuGenFormulaElement
	{
		private double _value;

		/// <summary>
		/// </summary>
		public override double Value
		{
			get
			{
				return _value;
			}
		}

		/// <summary>
		/// Measures the element and its subitems with the speicifed <see cref="NuGenMeasureElementArgs"/>.
		/// </summary>
		/// <param name="e"></param>
		public override void MeasureElement(NuGenMeasureElementArgs e)
		{
			this.RenderBounds = new NuGenCenteredSize(
				Size.Round(e.Graphics.MeasureString(this.ToString(), e.Font))
			);
		}

		/// <summary>
		/// Measures the element and its subitems with the specified <see cref="NuGenRenderElementArgs"/>
		/// on the specified location.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="location"></param>
		public override void RenderElement(NuGenRenderElementArgs e, Point location)
		{
			e.Graphics.DrawString(this.ToString(), e.Font, e.Brush, location);
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return _value.ToString("0.###;(-0.###)");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenValue"/> class.
		/// </summary>
		public NuGenValue(double value)
		{
			_value = value;
		}
	}
}

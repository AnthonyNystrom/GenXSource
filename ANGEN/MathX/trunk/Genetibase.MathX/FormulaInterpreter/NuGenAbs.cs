/* -----------------------------------------------
 * NuGenAbs.cs
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
	/// abs()
	/// </summary>
	public class NuGenAbs : NuGenFunction
	{
		/// <summary>
		/// Gets the value of the element, including all calculations required.
		/// </summary>
		/// <value></value>
		public override double Value
		{
			get
			{
				return Math.Abs(this.Element.Value);
			}
		}

		/// <summary>
		/// Measures the element and its subitems with the speicifed <see cref="NuGenMeasureElementArgs"/>.
		/// </summary>
		/// <param name="e"></param>
		public override void MeasureElement(NuGenMeasureElementArgs e)
		{
			this.Element.MeasureElement(e);
			this.RenderBounds = new NuGenCenteredSize(
				this.Element.RenderBounds.Width + 4, this.Element.RenderBounds.Height + 6);
		}

		/// <summary>
		/// Measures the element and its subitems with the specified <see cref="NuGenRenderElementArgs"/>
		/// on the specified location.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="location"></param>
		public override void RenderElement(NuGenRenderElementArgs e, Point location)
		{
			this.Element.RenderElement(e, new Point(location.X + 2, location.Y + 3));
			//draw abs lines
			e.Graphics.DrawLine(e.Pen,
				location.X + 1, location.Y + 3,
				location.X + 1, location.Y + this.RenderBounds.Height - 3);
			e.Graphics.DrawLine(e.Pen,
				location.X + this.RenderBounds.Width, location.Y + 3,
				location.X + this.RenderBounds.Width, location.Y + this.RenderBounds.Height - 3);
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return "abs(" + this.Element.ToString() + ")";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenAbs"/> class.
		/// </summary>
		/// <param name="elem">The elem.</param>
		public NuGenAbs(NuGenFormulaElement elem)
			: base(elem)
		{
		}
	}
}

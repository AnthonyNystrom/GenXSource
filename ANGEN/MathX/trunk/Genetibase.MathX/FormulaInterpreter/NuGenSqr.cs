/* -----------------------------------------------
 * NuGenSqr.cs
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
	/// sqr()
	/// </summary>
	public class NuGenSqr : NuGenFunction
	{
		/// <summary>
		/// Gets the value of the element, including all calculations required.
		/// </summary>
		/// <value></value>
		public override double Value
		{
			get
			{
				return Math.Sqrt(this.Element.Value);
			}
		}

		/// <summary>
		/// Measures the element and its subitems with the speicifed <see cref="NuGenMeasureElementArgs"/>.
		/// </summary>
		/// <param name="e"></param>
		public override void MeasureElement(NuGenMeasureElementArgs e)
		{
			//measure child element
			this.Element.MeasureElement(e);
			//set own size
			this.RenderBounds = new NuGenCenteredSize(
				this.Element.RenderBounds.Width + 8, this.Element.RenderBounds.Height + 2,
				this.Element.RenderBounds.LeftPart + 3, this.Element.RenderBounds.TopPart + 2);
		}

		/// <summary>
		/// Measures the element and its subitems with the specified <see cref="NuGenRenderElementArgs"/>
		/// on the specified location.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="location"></param>
		public override void RenderElement(NuGenRenderElementArgs e, Point location)
		{
			//render child element
			this.Element.RenderElement(e, new Point(
				location.X + 7,
				location.Y + 2));
			//render root
			e.Graphics.DrawLines(e.Pen, new Point[]
				{
					new Point(location.X+1,location.Y+3*this.RenderBounds.Height/4),
					new Point(location.X+3,location.Y+3*this.RenderBounds.Height/4-1),
					new Point(location.X+5,location.Y+this.RenderBounds.Height-2),
					new Point(location.X+6,location.Y+2),
					new Point(location.X+this.RenderBounds.Width,location.Y+2),
					new Point(location.X+this.RenderBounds.Width,location.Y+4)
				});
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return "sqr(" + this.Element.ToString() + ")";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSqrt"/> class.
		/// </summary>
		/// <param name="element"></param>
		/// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
		public NuGenSqr(NuGenFormulaElement element)
			: base(element)
		{
		}
	}
}

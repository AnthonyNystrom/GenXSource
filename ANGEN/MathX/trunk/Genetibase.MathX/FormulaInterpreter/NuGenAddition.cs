/* -----------------------------------------------
 * NuGenAddition.cs
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
	/// Operator +.
	/// </summary>
	public class NuGenAddition : NuGenOperator
	{
		/// <summary>
		/// Returns the importancy in a complex calculation, e.g. ^ is more important than +.
		/// </summary>
		/// <value></value>
		public override int Preference
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Measures the element and its subitems with the speicifed <see cref="NuGenMeasureElementArgs"/>.
		/// </summary>
		/// <param name="e"></param>
		public override void MeasureElement(NuGenMeasureElementArgs e)
		{
			// Operator + is 3x3 pixels.
			// Measure child elements.
			this.Left.MeasureElement(e);
			this.Right.MeasureElement(e);
			
			// Calculate maximal upper half and downer part
			// (important for fraction calculations).
			int topPart = Math.Max(
				3,
				Math.Max(this.Left.RenderBounds.TopPart, this.Right.RenderBounds.TopPart)
			);
			
			int bottomPart = Math.Max(
				3,
				Math.Max(this.Left.RenderBounds.BottomPart, this.Right.RenderBounds.BottomPart)
			);
			
			// Set own size.
			this.RenderBounds = new NuGenCenteredSize(
				// Width is sum of all widths.
				this.Left.RenderBounds.Width + 8 + this.Right.RenderBounds.Width,
				// Height is sum of the maximal parts.
				topPart + bottomPart,
				// Mid is position of operator.
				this.Left.RenderBounds.Width + 4, topPart
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
			// Render first sub-element.
			this.Left.RenderElement(e, new Point(
				location.X,
				location.Y + this.RenderBounds.TopPart - this.Left.RenderBounds.TopPart));
			// Render operator.
			e.Graphics.DrawLine(e.Pen,
				location.X + this.RenderBounds.LeftPart, location.Y + this.RenderBounds.TopPart - 3,
				location.X + this.RenderBounds.LeftPart, location.Y + this.RenderBounds.TopPart + 3);
			e.Graphics.DrawLine(e.Pen,
				location.X + this.RenderBounds.LeftPart - 3, location.Y + this.RenderBounds.TopPart,
				location.X + this.RenderBounds.LeftPart + 3, location.Y + this.RenderBounds.TopPart);
			// Render second sub-element.
			this.Right.RenderElement(e, new Point(
				location.X + this.RenderBounds.Width - this.Right.RenderBounds.Width,
				location.Y + this.RenderBounds.TopPart - this.Right.RenderBounds.TopPart));
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0} + {1}", this.Left.ToString(this), this.Right.ToString(this));
		}

		/// <summary>
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		public override string ToString(NuGenFormulaElement parent)
		{
			NuGenOperator parentop = parent as NuGenOperator;
			
			if (parentop != null && parentop.Preference > 0)
				return string.Format("({0})", this.ToString());
			else
				return this.ToString();
		}

		/// <summary>
		/// Gets the value of the element, including all calculations required.
		/// </summary>
		/// <value></value>
		public override double Value
		{
			get
			{
				return this.Left.Value + this.Right.Value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenAddition"/> class.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <exception cref="ArgumentNullException">
		/// 	<para><paramref name="left"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="right"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenAddition(NuGenFormulaElement left, NuGenFormulaElement right)
			: base(left, right)
		{
		}
	}
}

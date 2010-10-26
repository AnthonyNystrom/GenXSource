/* -----------------------------------------------
 * NuGenMultiplication.cs
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
	/// Operator *.
	/// </summary>
	public class NuGenMultiplication : NuGenOperator
	{
		/// <summary>
		/// Returns the importancy in a complex calculation, e.g. ^ is more important than +.
		/// </summary>
		/// <value></value>
		public override int Preference
		{
			get
			{
				return 2;
			}
		}

		/// <summary>
		/// Gets the value of the element, including all calculations required.
		/// </summary>
		/// <value></value>
		public override double Value
		{
			get
			{
				return this.Left.Value * this.Right.Value;
			}
		}

		/// <summary>
		/// Measures the element and its subitems with the speicifed <see cref="NuGenMeasureElementArgs"/>.
		/// </summary>
		/// <param name="e"></param>
		public override void MeasureElement(NuGenMeasureElementArgs e)
		{
			//operator * is 1x1 pixels
			//measure child elements
			this.Left.MeasureElement(e);
			this.Right.MeasureElement(e);
			//calculate maximal upper half and downer half
			//(important for fraction calculations)
			int tophalf =
				Math.Max(this.Left.RenderBounds.TopPart, this.Right.RenderBounds.TopPart),
				bottomhalf =
				Math.Max(this.Left.RenderBounds.BottomPart, this.Right.RenderBounds.BottomPart);
			//set own size
			this.RenderBounds = new NuGenCenteredSize(
				//width is sum of all widths
				this.Left.RenderBounds.Width + 4 + this.Right.RenderBounds.Width,
				//height is sum of the maximal halfs
				tophalf + bottomhalf,
				//mid is position of operator
				this.Left.RenderBounds.Width + 2, tophalf);
		}

		/// <summary>
		/// Measures the element and its subitems with the specified <see cref="NuGenRenderElementArgs"/>
		/// on the specified location.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="location"></param>
		public override void RenderElement(NuGenRenderElementArgs e, Point location)
		{
			//render first subelement
			this.Left.RenderElement(e, new Point(
				location.X,
				location.Y + this.RenderBounds.TopPart - this.Left.RenderBounds.TopPart));
			//render operator
			e.Graphics.FillEllipse(e.Brush,
				location.X + this.RenderBounds.LeftPart - 1,
				location.Y + this.RenderBounds.TopPart - 1, 2, 2);
			//render second subelement
			this.Right.RenderElement(e, new Point(
				location.X + this.RenderBounds.Width - this.Right.RenderBounds.Width,
				location.Y + this.RenderBounds.TopPart - this.Right.RenderBounds.TopPart));
		}

		/// <summary>
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		public override string ToString(NuGenFormulaElement parent)
		{
			NuGenOperator parentop = parent as NuGenOperator;
			if (parentop != null && parentop.Preference > 2)
				return string.Format("({0})", this.ToString());
			else
				return this.ToString();
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0} * {1}", this.Left.ToString(this), this.Right.ToString(this));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMultiplication"/> class.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <exception cref="ArgumentNullException">
		/// 	<para><paramref name="left"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="right"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenMultiplication(NuGenFormulaElement left, NuGenFormulaElement right)
			: base(left, right)
		{
		}
	}
}

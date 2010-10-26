/* -----------------------------------------------
 * NuGenDivision.cs
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
	/// Operator /.
	/// </summary>
	public class NuGenDivision : NuGenOperator
	{
		/// <summary>
		/// Returns the importancy in a complex calculation, e.g. ^ is more important than +.
		/// </summary>
		/// <value></value>
		public override int Preference
		{
			get
			{
				return 3;
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
				return this.Left.Value / this.Right.Value;
			}
		}

		/// <summary>
		/// Measures the element and its subitems with the speicifed <see cref="NuGenMeasureElementArgs"/>.
		/// </summary>
		/// <param name="e"></param>
		public override void MeasureElement(NuGenMeasureElementArgs e)
		{
			//operator is a fraction line
			//measure child elements
			this.Left.MeasureElement(e);
			this.Right.MeasureElement(e);
			//width is max of all widths +4
			int width = Math.Max(this.Left.RenderBounds.Width, this.Right.RenderBounds.Width) + 4;
			//set own size
			this.RenderBounds = new NuGenCenteredSize(
				width,
				//height is sum of all heights
				this.Left.RenderBounds.Height + this.Right.RenderBounds.Height,
				//mid is position of operator
				width / 2, this.Left.RenderBounds.Height);
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
				location.X + this.RenderBounds.LeftPart - this.Left.RenderBounds.Width / 2,
				location.Y + this.RenderBounds.TopPart - this.Left.RenderBounds.Height + 1));
			//render operator
			e.Graphics.DrawLine(e.Pen,
				location.X + 1, location.Y + this.RenderBounds.TopPart,
				location.X + this.RenderBounds.Width - 1, location.Y + this.RenderBounds.TopPart);
			//render second subelement
			this.Right.RenderElement(e, new Point(
				location.X + this.RenderBounds.LeftPart - this.Right.RenderBounds.Width / 2,
				location.Y + this.RenderBounds.TopPart));
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
			return string.Format("{0} / {1}", this.Left.ToString(this), this.Right.ToString(this));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDivision"/> class.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <exception cref="ArgumentNullException">
		/// 	<para><paramref name="left"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="right"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenDivision(NuGenFormulaElement left, NuGenFormulaElement right)
			: base(left, right)
		{
		}
	}
}

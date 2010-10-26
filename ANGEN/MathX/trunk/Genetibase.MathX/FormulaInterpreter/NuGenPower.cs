/* -----------------------------------------------
 * NuGenPower.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.MathX.FormulaInterpreter
{
	/// <summary>
	/// Operator ^.
	/// </summary>
	public class NuGenPower : NuGenOperator
	{
		/// <summary>
		/// Returns the importancy in a complex calculation, e.g. ^ is more important than +.
		/// </summary>
		/// <value></value>
		public override int Preference
		{
			get
			{
				return 5;
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
				return Math.Pow(this.Left.Value, this.Right.Value);
			}
		}

		/// <summary>
		/// Measures the element and its subitems with the speicifed <see cref="NuGenMeasureElementArgs"/>.
		/// </summary>
		/// <param name="e"></param>
		public override void MeasureElement(NuGenMeasureElementArgs e)
		{
			//measure child elements
			this.Left.MeasureElement(e);
			this.Right.MeasureElement(e);
			//calculate height
			int height = Math.Max(this.Right.RenderBounds.Height / 2, this.Right.RenderBounds.Height / 4 + this.Left.RenderBounds.TopPart) + 1;
			//set own size
			this.RenderBounds = new NuGenCenteredSize(
				//width is sum of all widths
				this.Left.RenderBounds.Width + this.Right.RenderBounds.Width / 2,
				//height is sum of all heights
				this.Left.RenderBounds.BottomPart + height,
				//mid is position of operator
				this.Left.RenderBounds.Width, height);
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
			//prepare graphics
			Matrix transform = e.Graphics.Transform;
			e.Graphics.TranslateTransform(
				location.X + this.RenderBounds.Width - this.Right.RenderBounds.Width / 2,
				location.Y + 1);
			e.Graphics.ScaleTransform(0.5f, 0.5f);
			//render subelement
			this.Right.RenderElement(e, Point.Empty);
			//reset graphics
			e.Graphics.Transform = transform;
		}

		/// <summary>
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		public override string ToString(NuGenFormulaElement parent)
		{
			NuGenOperator parentop = parent as NuGenOperator;
			if (parentop != null && parentop.Preference > 5)
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
			return string.Format("{0} ^ {1}", this.Left.ToString(this), this.Right.ToString(this));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPower"/> class.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <exception cref="ArgumentNullException">
		/// 	<para><paramref name="left"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="right"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPower(NuGenFormulaElement left, NuGenFormulaElement right)
			: base(left, right)
		{
		}
	}
}

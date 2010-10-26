/* -----------------------------------------------
 * NuGenFormulaElement.cs
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
	public abstract class NuGenFormulaElement
	{
		private NuGenCenteredSize _renderBounds;

		/// <summary>
		/// </summary>
		public NuGenCenteredSize RenderBounds
		{
			get
			{
				return _renderBounds;
			}
			protected set
			{
				_renderBounds = value;
			}
		}

		/// <summary>
		/// Gets the value of the element, including all calculations required.
		/// </summary>
		public abstract double Value
		{
			get;
		}

		/// <summary>
		/// Measures the element and its subitems with the speicifed <see cref="NuGenMeasureElementArgs"/>.
		/// </summary>
		public abstract void MeasureElement(NuGenMeasureElementArgs e);
		
		/// <summary>
		/// Measures the element and its subitems with the specified <see cref="NuGenRenderElementArgs"/>
		/// on the specified location.
		/// </summary>
		public abstract void RenderElement(NuGenRenderElementArgs e, Point location);

		/// <summary>
		/// </summary>
		public virtual string ToString(NuGenFormulaElement parent)
		{
			return this.ToString();
		}
	}
}

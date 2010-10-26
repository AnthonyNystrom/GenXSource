/* -----------------------------------------------
 * TextContentLayer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Genetibase.Windows.Controls.Editor
{
	internal class TextContentLayer : FrameworkElement
	{
		private VisualCollection _children;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextContentLayer"/> class.
		/// </summary>
		public TextContentLayer()
		{
			_children = new VisualCollection(this);
		}

		/// <summary>
		/// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement"/> derived class.
		/// </summary>
		/// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		/// <returns>The actual size used.</returns>
		protected override Size ArrangeOverride(Size finalSize)
		{
			return base.ArrangeOverride(finalSize);
		}

		/// <summary>
		/// Gets the visual child.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="index"/> is less than zero or is greater than the amount of children.
		/// </exception>
		protected override Visual GetVisualChild(Int32 index)
		{
			if ((index < 0) || (index > _children.Count))
			{
				throw new ArgumentOutOfRangeException("index");
			}

			return _children[index];
		}

		/// <summary>
		/// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
		/// </summary>
		/// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
		protected override void OnRender(DrawingContext drawingContext)
		{
			foreach (TextLineVisual visual2 in this.Children)
			{
				if (visual2 != null)
				{
					visual2.RenderText();
				}
			}
		}

		public VisualCollection Children
		{
			get
			{
				return _children;
			}
		}

		/// <summary>
		/// Gets the number of visual child elements within this element.
		/// </summary>
		/// <value></value>
		/// <returns>The number of visual child elements for this element.</returns>
		protected override Int32 VisualChildrenCount
		{
			get
			{
				return _children.Count;
			}
		}
	}
}

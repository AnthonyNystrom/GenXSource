/* -----------------------------------------------
 * TextFormattingParagraphProperties.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.TextFormatting;
using System.Windows;

namespace Genetibase.Windows.Controls.Editor
{
	/// <summary>
	/// </summary>
	public class TextFormattingParagraphProperties : TextParagraphProperties
	{
		/// <summary>
		/// Sets the size of the tab.
		/// </summary>
		public void SetTabSize(Double tabSize)
		{
			_tabSize = new Double?(tabSize);
		}

		/// <summary>
		/// Gets the default incremental tab distance.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Double"/> value that represents the default incremental tab distance.</returns>
		public override Double DefaultIncrementalTab
		{
			get
			{
				if (!_tabSize.HasValue)
				{
					return base.DefaultIncrementalTab;
				}
				return _tabSize.Value;
			}
		}

		/// <summary>
		/// Gets the default text run properties, such as typeface or foreground brush.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Media.TextFormatting.TextRunProperties"/> value.</returns>
		public override TextRunProperties DefaultTextRunProperties
		{
			get
			{
				return TextFormattingRunProperties.DefaultProperties;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the text run is the first line of the paragraph.
		/// </summary>
		/// <value></value>
		/// <returns>true, if the text run is the first line of the paragraph; otherwise, false.</returns>
		public override Boolean FirstLineInParagraph
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a value that specifies whether the primary text advance direction shall be left-to-right, or right-to-left.
		/// </summary>
		/// <value></value>
		/// <returns>An enumerated value of <see cref="T:System.Windows.FlowDirection"/>.</returns>
		public override FlowDirection FlowDirection
		{
			get
			{
				return FlowDirection.LeftToRight;
			}
		}

		/// <summary>
		/// Gets the amount of line indentation.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Double"/> that represents the amount of line indentation.</returns>
		public override Double Indent
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Gets the height of a line of text.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Double"/> that represents the height of a line of text.</returns>
		public override Double LineHeight
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Gets a value that describes how an inline content of a block is aligned.
		/// </summary>
		/// <value></value>
		/// <returns>An enumerated value of <see cref="T:System.Windows.TextAlignment"/>.</returns>
		public override TextAlignment TextAlignment
		{
			get
			{
				return TextAlignment.Left;
			}
		}

		/// <summary>
		/// Gets a value that specifies marker characteristics of the first line in the paragraph.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Media.TextFormatting.TextMarkerProperties"/> value.</returns>
		public override TextMarkerProperties TextMarkerProperties
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Gets a value that controls whether text wraps when it reaches the flow edge of its containing block box.
		/// </summary>
		/// <value></value>
		/// <returns>An enumerated value of <see cref="T:System.Windows.TextWrapping"/>.</returns>
		public override TextWrapping TextWrapping
		{
			get
			{
				return TextWrapping.Wrap;
			}
		}

		private Double? _tabSize = null;
	}
}

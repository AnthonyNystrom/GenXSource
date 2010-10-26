/* -----------------------------------------------
 * TextEmbeddedSpace.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.TextFormatting;
using System.Windows;
using System.Windows.Media;

namespace Genetibase.Windows.Controls.Editor
{
	internal class TextEmbeddedSpace : TextEmbeddedObject
	{
		private readonly TextRunProperties _defaultTextRunProperties = new TextFormattingRunProperties(new Typeface(SystemFonts.CaptionFontFamily, SystemFonts.CaptionFontStyle, SystemFonts.CaptionFontWeight, FontStretches.Normal), SystemFonts.CaptionFontSize, SystemColors.WindowColor);
		private Size _size;

		public TextEmbeddedSpace(Size size)
		{
			_size = size;
		}

		public override Rect ComputeBoundingBox(Boolean rightToLeft, Boolean sideways)
		{
			return new Rect(0, 0, _size.Width, _size.Height);
		}

		public override void Draw(DrawingContext drawingContext, Point origin, Boolean rightToLeft, Boolean sideways)
		{
		}

		public override TextEmbeddedObjectMetrics Format(Double remainingParagraphWidth)
		{
			return new TextEmbeddedObjectMetrics(_size.Width, _size.Height, _size.Height);
		}

		public override LineBreakCondition BreakAfter
		{
			get
			{
				return LineBreakCondition.BreakPossible;
			}
		}

		public override LineBreakCondition BreakBefore
		{
			get
			{
				return LineBreakCondition.BreakPossible;
			}
		}

		public override CharacterBufferReference CharacterBufferReference
		{
			get
			{
				return new CharacterBufferReference(" ", 1);
			}
		}

		public override Boolean HasFixedSize
		{
			get
			{
				return true;
			}
		}

		public override Int32 Length
		{
			get
			{
				return 1;
			}
		}

		public override TextRunProperties Properties
		{
			get
			{
				return _defaultTextRunProperties;
			}
		}
	}
}

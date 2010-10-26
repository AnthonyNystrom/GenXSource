/* -----------------------------------------------
 * TextViewHelper.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public class TextViewHelper
	{
		#region Properties.Public

		/// <summary>
		/// </summary>
		public Int32 FirstRenderedCharacter
		{
			get
			{
				ITextLine firstRenderedLine = this.FirstRenderedLine;
				if (firstRenderedLine == null)
				{
					return -1;
				}
				return firstRenderedLine.LineSpan.Start;
			}
		}

		/// <summary>
		/// </summary>
		public ITextLine FirstRenderedLine
		{
			get
			{
				if (_textView.TextLines.Count == 0)
				{
					return null;
				}
				return _textView.TextLines[0];
			}
		}

		/// <summary>
		/// </summary>
		public Int32 FirstRenderedLineNumber
		{
			get
			{
				Int32 firstRenderedCharacter = this.FirstRenderedCharacter;
				if (firstRenderedCharacter == -1)
				{
					return -1;
				}
				return _textView.TextBuffer.GetLineNumberFromPosition(firstRenderedCharacter);
			}
		}

		/// <summary>
		/// </summary>
		public Int32 FirstVisibleCharacter
		{
			get
			{
				ITextLine firstVisibleLine = this.FirstVisibleLine;
				if (firstVisibleLine == null)
				{
					return -1;
				}
				return firstVisibleLine.LineSpan.Start;
			}
		}

		/// <summary>
		/// </summary>
		public ITextLine FirstVisibleLine
		{
			get
			{
				Int32 count = _textView.TextLines.Count;
				for (Int32 i = 0; i < count; i++)
				{
					ITextLine line = _textView.TextLines[i];
					if (line.VerticalOffset >= _textView.ViewportHeight)
					{
						break;
					}
					if ((line.VerticalOffset + line.Height) > 0)
					{
						return line;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// </summary>
		public Int32 FirstVisibleLineNumber
		{
			get
			{
				Int32 firstVisibleCharacter = this.FirstVisibleCharacter;
				if (firstVisibleCharacter == -1)
				{
					return -1;
				}
				return _textView.TextBuffer.GetLineNumberFromPosition(firstVisibleCharacter);
			}
		}

		/// <summary>
		/// </summary>
		public Int32 LastRenderedCharacter
		{
			get
			{
				ITextLine lastRenderedLine = this.LastRenderedLine;
				if (lastRenderedLine == null)
				{
					return -1;
				}
				return lastRenderedLine.LineSpan.End;
			}
		}

		/// <summary>
		/// </summary>
		public ITextLine LastRenderedLine
		{
			get
			{
				if (_textView.TextLines.Count == 0)
				{
					return null;
				}
				return _textView.TextLines[_textView.TextLines.Count - 1];
			}
		}

		/// <summary>
		/// </summary>
		public Int32 LastRenderedLineNumber
		{
			get
			{
				Int32 lastRenderedCharacter = this.LastRenderedCharacter;
				if (lastRenderedCharacter == -1)
				{
					return -1;
				}
				return _textView.TextBuffer.GetLineNumberFromPosition(lastRenderedCharacter);
			}
		}

		/// <summary>
		/// </summary>
		public Int32 LastVisibleCharacter
		{
			get
			{
				ITextLine lastVisibleLine = this.LastVisibleLine;
				if (lastVisibleLine == null)
				{
					return -1;
				}
				if (!lastVisibleLine.LineSpan.IsEmpty)
				{
					return (lastVisibleLine.LineSpan.End - 1);
				}
				return lastVisibleLine.LineSpan.End;
			}
		}

		/// <summary>
		/// </summary>
		public ITextLine LastVisibleLine
		{
			get
			{
				for (Int32 i = _textView.TextLines.Count - 1; i >= 0; i--)
				{
					ITextLine line = _textView.TextLines[i];
					if ((line.VerticalOffset + line.Height) < 0)
					{
						break;
					}
					if (line.VerticalOffset < _textView.ViewportHeight)
					{
						return line;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// </summary>
		public Int32 LastVisibleLineNumber
		{
			get
			{
				Int32 lastVisibleCharacter = this.LastVisibleCharacter;
				if (lastVisibleCharacter == -1)
				{
					return -1;
				}
				return _textView.TextBuffer.GetLineNumberFromPosition(lastVisibleCharacter);
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		/// <param name="span"></param>
		/// <param name="horizontalPadding"></param>
		/// <param name="verticalPadding"></param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="span"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para>
		///		<paramref name="verticalPadding"/> is not a number of is less than zero.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="horizontalPadding"/> is not a number or is less than zero.
		/// </para>
		/// -or-
		/// <para>
		///		span.End > textView.TextBuffer.Length
		/// </para>
		/// </exception>
		public Boolean EnsureSpanVisible(Span span, Double horizontalPadding, Double verticalPadding)
		{
			if (Double.IsNaN(horizontalPadding) || (horizontalPadding < 0))
			{
				throw new ArgumentOutOfRangeException("horizontalPadding");
			}
			if (Double.IsNaN(verticalPadding) || (verticalPadding < 0))
			{
				throw new ArgumentOutOfRangeException("verticalPadding");
			}
			if (span == null)
			{
				throw new ArgumentNullException("span");
			}
			if (span.End > _textView.TextBuffer.Length)
			{
				throw new ArgumentOutOfRangeException("span");
			}
			IList<ITextLine> textLines = _textView.TextLines;
			if (textLines.Count == 0)
			{
				return false;
			}
			Boolean flag = span.Start < textLines[0].LineSpan.Start;
			ITextLine line = textLines[textLines.Count - 1];
			Boolean flag2 = (span.End > line.LineSpan.End) || ((span.End == line.LineSpan.End) && ((span.End != _textView.TextBuffer.Length) || (line.NewlineLength != 0)));
			if (flag)
			{
				if (flag2)
				{
					return false;
				}
				_textView.DisplayLine(_textView.TextBuffer.GetLineNumberFromPosition(span.Start), verticalPadding, ViewRelativePosition.Top);
			}
			else if (flag2)
			{
				_textView.DisplayLine(_textView.TextBuffer.GetLineNumberFromPosition(span.End), verticalPadding, ViewRelativePosition.Bottom);
			}
			else if (this.GetTextLineContaining(span.Start).VerticalOffset < verticalPadding)
			{
				_textView.DisplayLine(_textView.TextBuffer.GetLineNumberFromPosition(span.Start), verticalPadding, ViewRelativePosition.Top);
			}
			else
			{
				ITextLine line3 = this.GetTextLineContaining(span.End);
				if (((line3.VerticalOffset + line3.Height) + verticalPadding) > _textView.ViewportHeight)
				{
					_textView.DisplayLine(_textView.TextBuffer.GetLineNumberFromPosition(span.End), verticalPadding, ViewRelativePosition.Bottom);
				}
			}
			Boolean flag3 = false;
			ITextLine textLineContaining = this.GetTextLineContaining(span.Start);
			if (textLineContaining == null)
			{
				return flag3;
			}
			ITextLine line5 = this.GetTextLineContaining(span.End);
			if (line5 == null)
			{
				return flag3;
			}
			flag3 = (textLineContaining.VerticalOffset >= verticalPadding) && (((line5.VerticalOffset + line5.Height) + verticalPadding) <= _textView.ViewportHeight);
			Double maxValue = Double.MaxValue;
			Double minValue = Double.MinValue;
			if (textLineContaining == line5)
			{
				this.ExpandBounds(textLineContaining, span, ref maxValue, ref minValue);
			}
			else
			{
				this.ExpandBounds(textLineContaining, new Span(span.Start, textLineContaining.LineSpan.End - span.Start), ref maxValue, ref minValue);
				this.ExpandBounds(line5, new Span(line5.LineSpan.Start, span.End - line5.LineSpan.Start), ref maxValue, ref minValue);
				Int32 indexOfTextLine = this.GetIndexOfTextLine(textLineContaining);
				Int32 num4 = this.GetIndexOfTextLine(line5);
				for (Int32 i = indexOfTextLine + 1; i < num4; i++)
				{
					ITextLine line6 = textLines[i];
					this.ExpandBounds(line6, new Span(line6.LineSpan.Start, line6.LineSpan.Length), ref maxValue, ref minValue);
				}
			}
			if (maxValue == Double.MaxValue)
			{
				return (flag3 && (horizontalPadding == 0));
			}
			Double viewportHorizontalOffset = _textView.ViewportHorizontalOffset;
			Double num7 = viewportHorizontalOffset - (maxValue - horizontalPadding);
			Double num8 = (minValue + horizontalPadding) - (viewportHorizontalOffset + _textView.ViewportWidth);
			if (num7 > 0)
			{
				if (num8 <= 0)
				{
					_textView.ScrollViewportHorizontally((viewportHorizontalOffset - num7) - _textView.ViewportHorizontalOffset);
				}
			}
			else if (num8 > 0)
			{
				_textView.ScrollViewportHorizontally((viewportHorizontalOffset + num8) - _textView.ViewportHorizontalOffset);
			}
			Double num9 = _textView.ViewportHorizontalOffset - viewportHorizontalOffset;
			maxValue += num9;
			minValue += num9;
			return ((flag3 && (maxValue >= horizontalPadding)) && ((_textView.ViewportWidth - minValue) >= horizontalPadding));
		}

		/// <summary>
		/// </summary>
		public Boolean FindTextLine(Int32 characterPosition, out Int32 index)
		{
			index = 0;
			IList<ITextLine> textLines = _textView.TextLines;
			Int32 count = textLines.Count;
			if (count != 0)
			{
				if (characterPosition == _textView.TextBuffer.Length)
				{
					ITextLine line = textLines[count - 1];
					if ((line.NewlineLength == 0) && (characterPosition == line.LineSpan.End))
					{
						index = count - 1;
						return true;
					}
				}
				if (characterPosition < textLines[0].LineSpan.Start)
				{
					return false;
				}
				if (characterPosition >= textLines[count - 1].LineSpan.End)
				{
					index = count;
					return false;
				}
				Int32 num2 = 0;
				Int32 num3 = count - 1;
				while (num3 >= num2)
				{
					Int32 num4 = (num2 + num3) / 2;
					ITextLine line2 = textLines[num4];
					if (characterPosition < line2.LineSpan.Start)
					{
						num3 = num4 - 1;
					}
					else
					{
						if (characterPosition >= line2.LineSpan.End)
						{
							num2 = num4 + 1;
							continue;
						}
						index = num4;
						return true;
					}
				}
				index = num2;
			}
			return false;
		}

		/// <summary>
		/// </summary>
		public ITextLine GetTextLineAt(Double yOffset)
		{
			if (Double.IsNaN(yOffset))
			{
				throw new ArgumentOutOfRangeException("yOffset");
			}
			IList<ITextLine> textLines = _textView.TextLines;
			Int32 count = textLines.Count;
			if (count == 0)
			{
				return null;
			}
			for (Int32 i = 0; i < count; i++)
			{
				ITextLine line = textLines[i];
				if (line.VerticalOffset <= yOffset)
				{
					if (((line.VerticalOffset + line.Height) + 0.01) >= yOffset)
					{
						return line;
					}
				}
				else
				{
					return null;
				}
			}
			return textLines[count - 1];
		}

		/// <summary>
		/// </summary>
		public ITextLine GetTextLineClosestTo(Int32 characterPosition)
		{
			if ((characterPosition < 0) || (characterPosition > _textView.TextBuffer.Length))
			{
				throw new ArgumentOutOfRangeException("characterPosition");
			}
			Int32 index = 0;
			this.FindTextLine(characterPosition, out index);
			IList<ITextLine> textLines = _textView.TextLines;
			if (index >= textLines.Count)
			{
				index = textLines.Count - 1;
			}
			return textLines[index];
		}

		/// <summary>
		/// </summary>
		public ITextLine GetTextLineContaining(Int32 characterPosition)
		{
			if ((characterPosition < 0) || (characterPosition > _textView.TextBuffer.Length))
			{
				throw new ArgumentOutOfRangeException("characterPosition");
			}
			Int32 index = 0;
			if (this.FindTextLine(characterPosition, out index))
			{
				return _textView.TextLines[index];
			}
			return null;
		}

		#endregion

		#region Methods.Private

		private void ExpandBounds(ITextLine line, Span span, ref Double leftEdge, ref Double rightEdge)
		{
			foreach (TextBounds bounds in line.GetTextBounds(span))
			{
				if (bounds.Left < leftEdge)
				{
					leftEdge = bounds.Left;
				}
				if (bounds.Right > rightEdge)
				{
					rightEdge = bounds.Right;
				}
			}
		}

		private Int32 GetIndexOfTextLine(ITextLine textLine)
		{
			Int32 num;
			if (textLine == null)
			{
				throw new ArgumentNullException("textLine");
			}
			IList<ITextLine> textLines = _textView.TextLines;
			if (!this.FindTextLine(textLine.LineSpan.Start, out num))
			{
				throw new ArgumentException("textLine not found within textlines", "textLine");
			}
			return num;
		}

		#endregion

		private ITextView _textView;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextViewHelper"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="textView"/> is <see langword="null"/>.
		/// </exception>
		public TextViewHelper(ITextView textView)
		{
			if (textView == null)
			{
				throw new ArgumentNullException("textView");
			}

			_textView = textView;
		}
	}
}

/* -----------------------------------------------
 * TextLineVisual.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Genetibase.Windows.Controls.Data.Text;
using Genetibase.Windows.Controls.Editor.Text.View;
using textView = Genetibase.Windows.Controls.Editor.Text.View;
using textFormatting = System.Windows.Media.TextFormatting;

namespace Genetibase.Windows.Controls.Editor
{
	internal class TextLineVisual : DrawingVisual, ITextLine, IDisposable
	{
		private Double _baseline;
		private Double _extent;
		private Double _height;
		private Double _horizontalOffset;
		private Int32 _length;
		private Boolean? _lineContainsBidi;
		private const Double _lineHeightPadding = 2;
		private Int32 _newLineLength;
		private const Double _newLineWidth = 7;
		private Double _overhangAfter;
		private TextPoint _startPoint;
		private List<Double> _textLineDistances;
		private IList<TextLine> _textLines;
		private List<Int32> _textLineStartIndices;
		private ITextView _textView;
		private TranslateTransform _transform;
		private Double _verticalOffset;
		private List<Int32> _virtualCharactersList;
		private Double _width;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextLineVisual"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="textView"/> is <see langword="null"/>.</para>
		/// </exception>
		public TextLineVisual(ITextView textView, IList<TextLine> textLines, TextSpan lineSpan, Int32 newLineLength, IList<Int32> virtualCharacters)
		{
			if (textView == null)
			{
				throw new ArgumentNullException("textView");
			}
			_textView = textView;
			_textLines = textLines;
			_newLineLength = newLineLength;
			_horizontalOffset = 0;
			_verticalOffset = 0;
			_startPoint = new TextPoint(lineSpan.TextBuffer, lineSpan.Start);
			_length = lineSpan.Length;
			_transform = new TranslateTransform(this.HorizontalOffset, this.VerticalOffset);
			base.Transform = _transform;
			if (virtualCharacters != null)
			{
				this.CreateVirtualCharacterLookup(virtualCharacters);
			}
			this.ComputeLineProperties();
			this.RenderText();
		}

		private void ComputeLineProperties()
		{
			_height = _width = 0;
			_baseline = _extent = _overhangAfter = 0;
			Double item = 0;
			Int32 num2 = 0;
			_textLineDistances = new List<Double>();
			_textLineStartIndices = new List<Int32>();
			foreach (TextLine line in _textLines)
			{
				if ((line.Height + 2) > _height)
				{
					_height = line.Height + 2;
				}
				if (line.Baseline > _baseline)
				{
					_baseline = line.Baseline;
				}
				if (line.Extent > _extent)
				{
					_extent = line.Extent;
				}
				if (line.OverhangAfter > _overhangAfter)
				{
					_overhangAfter = line.OverhangAfter;
				}
				_width += line.WidthIncludingTrailingWhitespace;
				_textLineDistances.Add(item);
				item += line.WidthIncludingTrailingWhitespace;
				_textLineStartIndices.Add(num2);
				num2 += line.Length;
			}
			if (_newLineLength > 0)
			{
				_width += 7;
			}
		}

		internal Boolean ContainsPosition(Int32 textBufferIndex)
		{
			if (textBufferIndex < this.LineSpan.Start)
			{
				return false;
			}
			Int32 end = this.LineSpan.End;
			if (textBufferIndex > end)
			{
				return false;
			}
			return ((textBufferIndex != end) || ((end == _textView.TextBuffer.Length) && (this.NewlineLength == 0)));
		}

		private void CreateVirtualCharacterLookup(IList<Int32> virtualCharacters)
		{
			_virtualCharactersList = new List<Int32>(virtualCharacters);
			_virtualCharactersList.Sort();
		}

		public void Dispose()
		{
			for (Int32 i = 0; i < _textLines.Count; i++)
			{
				_textLines[i].Dispose();
			}
		}

		internal Span GetAvalonTextElementSpan(Int32 bufferPosition)
		{
			if (!this.ContainsPosition(bufferPosition))
			{
				throw new ArgumentOutOfRangeException("bufferPosition");
			}
			if ((bufferPosition < this.LineSpan.Start) || (bufferPosition > this.LineSpan.End))
			{
				throw new ArgumentOutOfRangeException("bufferPosition");
			}
			if (bufferPosition > (this.LineSpan.End - this.NewlineLength))
			{
				return new Span(this.LineSpan.End, 0);
			}
			if (bufferPosition == (this.LineSpan.End - this.NewlineLength))
			{
				return new Span(this.LineSpan.End - this.NewlineLength, this.NewlineLength);
			}
			Int32 lineRelativePosition = this.GetLineRelativePosition(bufferPosition);
			if (lineRelativePosition < 0)
			{
				lineRelativePosition = 0;
			}
			Int32 indexOfLineContaining = this.GetIndexOfLineContaining(lineRelativePosition);
			TextLine line = _textLines[indexOfLineContaining];
			CharacterHit nextCaretCharacterHit = line.GetNextCaretCharacterHit(new CharacterHit(lineRelativePosition, 0));
			Int32 bufferRelativePosition = this.GetBufferRelativePosition(nextCaretCharacterHit.FirstCharacterIndex + nextCaretCharacterHit.TrailingLength);
			nextCaretCharacterHit = line.GetPreviousCaretCharacterHit(new CharacterHit(this.GetLineRelativePosition(bufferRelativePosition), 0));
			Int32 start = this.GetBufferRelativePosition(nextCaretCharacterHit.FirstCharacterIndex);
			return new Span(start, bufferRelativePosition - start);
		}

		private Int32 GetBufferRelativePosition(Int32 lineRelativePosition)
		{
			Int32 num = lineRelativePosition;
			foreach (Int32 num2 in _virtualCharactersList)
			{
				if (num2 >= num)
				{
					break;
				}
				num--;
			}
			return (num + this.LineSpan.Start);
		}

		public textView.TextBounds GetCharacterBounds(Int32 characterIndex)
		{
			Double distanceFromCharacterHit;
			Double num6;
			Int32 lineRelativePosition = this.GetLineRelativePosition(characterIndex);
			if (lineRelativePosition < 0)
			{
				return new textView.TextBounds(this.HorizontalOffset, this.VerticalOffset, 0, this.Height);
			}
			Int32 indexOfLineContaining = this.GetIndexOfLineContaining(lineRelativePosition);
			TextLine line = _textLines[indexOfLineContaining];
			Int32 num3 = _textLineStartIndices[indexOfLineContaining];
			Double num4 = _textLineDistances[indexOfLineContaining];
			if ((lineRelativePosition > 0) && (lineRelativePosition == ((num3 + line.Length) - line.NewlineLength)))
			{
				distanceFromCharacterHit = num6 = line.GetDistanceFromCharacterHit(new CharacterHit(lineRelativePosition - 1, 1));
			}
			else
			{
				distanceFromCharacterHit = line.GetDistanceFromCharacterHit(new CharacterHit(lineRelativePosition, 0));
				num6 = line.GetDistanceFromCharacterHit(new CharacterHit(lineRelativePosition, 1));
			}
			return new textView.TextBounds((this.HorizontalOffset + num4) + distanceFromCharacterHit, this.VerticalOffset, num6 - distanceFromCharacterHit, this.Height);
		}

		private Int32 GetIndexOfLineContaining(Int32 lineRelativePosition)
		{
			for (Int32 i = _textLineStartIndices.Count - 1; i >= 0; i--)
			{
				if (_textLineStartIndices[i] <= lineRelativePosition)
				{
					return i;
				}
			}
			return 0;
		}

		private Int32 GetIndexOfTextLineAtDistance(Double offset)
		{
			for (Int32 i = _textLineDistances.Count - 1; i >= 0; i--)
			{
				if (_textLineDistances[i] <= offset)
				{
					return i;
				}
			}
			return 0;
		}

		private Int32 GetLineRelativePosition(Int32 bufferRelativePosition)
		{
			Int32 num = bufferRelativePosition - this.LineSpan.Start;
			Int32 num2 = num;
			foreach (Int32 num3 in _virtualCharactersList)
			{
				if (num3 > num2)
				{
					return num;
				}
				num++;
			}
			return num;
		}

		private Int32 GetLineRelativePositionOfVirtualCharacter(Int32 bufferRelativePosition)
		{
			Int32 num = bufferRelativePosition - this.LineSpan.Start;
			Int32 num2 = num;
			foreach (Int32 num3 in _virtualCharactersList)
			{
				if (num3 >= num2)
				{
					return num;
				}
				num++;
			}
			return num;
		}

		public ReadOnlyCollection<textView.TextBounds> GetTextBounds(Span span)
		{
			var list = new List<textView.TextBounds>();
			Span span2 = span;
			if (!this.LineSpan.Contains(span))
			{
				span2 = this.LineSpan.Intersection(span);
			}
			if (span2 != null)
			{
				Int32 lineRelativePosition = this.GetLineRelativePosition(span2.Start);
				Int32 num2 = this.GetLineRelativePosition(span2.End);
				for (Int32 i = this.GetIndexOfLineContaining(lineRelativePosition); i < _textLines.Count; i++)
				{
					TextLine textLine = _textLines[i];
					Int32 num5 = _textLineStartIndices[i];
					Double horizontalOffset = _textLineDistances[i];
					Int32 startIndex = lineRelativePosition;
					if (startIndex < num5)
					{
						startIndex = num5;
					}
					Int32 endIndex = num2;
					if (endIndex > (num5 + textLine.Length))
					{
						endIndex = num5 + textLine.Length;
					}
					list.AddRange(this.GetTextBoundsOnLine(textLine, horizontalOffset, startIndex, endIndex));
					if (num2 <= (num5 + textLine.Length))
					{
						break;
					}
				}
				if ((span.End >= this.LineSpan.End) && (this.NewlineLength > 0))
				{
					list.Add(new textView.TextBounds((this.HorizontalOffset + this.Width) - 7, this.VerticalOffset, 7, this.Height));
				}
			}
			return list.AsReadOnly();
		}

		private List<textView.TextBounds> GetTextBoundsOnLine(TextLine textLine, Double horizontalOffset, Int32 startIndex, Int32 endIndex)
		{
			var list = new List<textView.TextBounds>();
			if (startIndex == endIndex)
			{
				Double distanceFromCharacterHit = textLine.GetDistanceFromCharacterHit(new CharacterHit(startIndex, 0));
				list.Add(new textView.TextBounds((distanceFromCharacterHit + horizontalOffset) + this.HorizontalOffset, this.VerticalOffset, 0, this.Height));
				return list;
			}
			foreach (textFormatting.TextBounds bounds in textLine.GetTextBounds(startIndex, endIndex - startIndex))
			{
				list.Add(new textView.TextBounds(bounds.Rectangle.Left + horizontalOffset, bounds.Rectangle.Top + this.VerticalOffset, bounds.Rectangle.Width, this.Height));
			}
			return list;
		}

		public ICaretPosition MoveCaretToLocation(Double horizontalDistance)
		{
			Double offset = horizontalDistance - this.HorizontalOffset;
			if (offset < 0)
			{
				return _textView.Caret.MoveTo(this.LineSpan.Start, CaretPlacement.LeftOfCharacter);
			}
			if (this.NewlineLength == 0)
			{
				if (offset >= this.Width)
				{
					return _textView.Caret.MoveTo(this.LineSpan.End, CaretPlacement.LeftOfCharacter);
				}
			}
			else if (offset >= (this.Width - 7))
			{
				return _textView.Caret.MoveTo(this.LineSpan.End - this.NewlineLength, CaretPlacement.LeftOfCharacter);
			}
			Int32 indexOfTextLineAtDistance = this.GetIndexOfTextLineAtDistance(offset);
			TextLine line = _textLines[indexOfTextLineAtDistance];
			Double num3 = _textLineDistances[indexOfTextLineAtDistance];
			CharacterHit characterHitFromDistance = line.GetCharacterHitFromDistance(offset - num3);
			Int32 bufferRelativePosition = this.GetBufferRelativePosition(characterHitFromDistance.FirstCharacterIndex);
			if (bufferRelativePosition > (this.LineSpan.End - this.NewlineLength))
			{
				return _textView.Caret.MoveTo(this.LineSpan.End - this.NewlineLength, CaretPlacement.LeftOfCharacter);
			}
			return _textView.Caret.MoveTo(bufferRelativePosition, (characterHitFromDistance.TrailingLength == 0) ? CaretPlacement.LeftOfCharacter : CaretPlacement.RightOfCharacter);
		}

		internal void RenderText()
		{
			DrawingContext drawingContext = base.RenderOpen();
			Double x = 0;
			foreach (TextLine line in _textLines)
			{
				line.Draw(drawingContext, new Point(x, 0), InvertAxes.None);
				x += line.WidthIncludingTrailingWhitespace;
			}
			drawingContext.Close();
		}

		internal void RenderText(DrawingContext drawingContext)
		{
			Double horizontalOffset = this.HorizontalOffset;
			foreach (TextLine line in _textLines)
			{
				Double y = ((this.VerticalOffset + this.Baseline) - line.Baseline) + 1;
				line.Draw(drawingContext, new Point(horizontalOffset, y), InvertAxes.None);
				horizontalOffset += line.WidthIncludingTrailingWhitespace;
			}
		}

		public Double Baseline
		{
			get
			{
				return _baseline;
			}
		}

		public Double Extent
		{
			get
			{
				return _extent;
			}
		}

		public Double Height
		{
			get
			{
				return _height;
			}
		}

		public Double HorizontalOffset
		{
			get
			{
				return _horizontalOffset;
			}
			set
			{
				_horizontalOffset = value;
				_transform.X = value;
			}
		}

		private Boolean LineContainsBidi
		{
			get
			{
				if (!_lineContainsBidi.HasValue)
				{
					_lineContainsBidi = false;
					foreach (TextLine line in _textLines)
					{
						foreach (IndexedGlyphRun run in line.GetIndexedGlyphRuns())
						{
							if ((run.GlyphRun.BidiLevel & 1) != 0)
							{
								_lineContainsBidi = true;
								break;
							}
						}
					}
				}
				return _lineContainsBidi.Value;
			}
		}

		public Span LineSpan
		{
			get
			{
				return new Span(_startPoint.Position, _length);
			}
		}

		public Int32 NewlineLength
		{
			get
			{
				return _newLineLength;
			}
		}

		public Double OverhangAfter
		{
			get
			{
				return _overhangAfter;
			}
		}

		public Double OverhangLeading
		{
			get
			{
				return _textLines[0].OverhangLeading;
			}
		}

		public Double OverhangTrailing
		{
			get
			{
				return _textLines[_textLines.Count - 1].OverhangTrailing;
			}
		}

		public Double VerticalOffset
		{
			get
			{
				return _verticalOffset;
			}
			set
			{
				_verticalOffset = value;
				_transform.Y = value;
			}
		}

		public Double Width
		{
			get
			{
				return _width;
			}
		}
	}
}

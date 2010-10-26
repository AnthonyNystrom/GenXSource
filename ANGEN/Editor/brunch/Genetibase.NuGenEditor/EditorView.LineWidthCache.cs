/* -----------------------------------------------
 * EditorView.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Editor;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls
{
	partial class EditorView
	{
		private sealed class LineWidthCache
		{
			private class LineWidth
			{
				/// <summary>
				/// </summary>
				public TextPoint Start
				{
					get
					{
						return _start;
					}
				}

				/// <summary>
				/// </summary>
				public Double Width
				{
					get
					{
						return _width;
					}
					set
					{
						_width = value;
					}
				}

				private TextPoint _start;
				private Double _width;

				/// <summary>
				/// </summary>
				public LineWidth(TextPoint start, Double width)
				{
					_start = start;
					_width = width;
				}
			}

			#region Properties.Public

			public Double MaxWidth
			{
				get
				{
					if (_maxWidth == Double.MaxValue)
					{
						if (_cachedLines.Count == 0)
						{
							return 0;
						}
						_maxWidth = -1;
						foreach (LineWidth width in _cachedLines)
						{
							if (width.Width > _maxWidth)
							{
								_maxWidth = width.Width;
							}
						}
					}
					return _maxWidth;
				}
			}

			#endregion

			#region Methods.Public

			public void AddLine(TextPoint start, Double width)
			{
				if (width > _maxWidth)
				{
					_maxWidth = width;
				}
				foreach (LineWidth width2 in _cachedLines)
				{
					if (width2.Start != start)
					{
						continue;
					}
					if ((width2.Width == _maxWidth) && (width < width2.Width))
					{
						_maxWidth = Double.MaxValue;
					}
					width2.Width = width;
					return;
				}
				if (_cachedLines.Count < 50)
				{
					_cachedLines.Add(new LineWidth(start, width));
				}
				else
				{
					Int32 num = 0;
					for (var i = 1; i < _cachedLines.Count; i++)
					{
						if (_cachedLines[i].Width < _cachedLines[num].Width)
						{
							num = i;
						}
					}
					if (width > _cachedLines[num].Width)
					{
						_cachedLines[num] = new LineWidth(start, width);
					}
				}
			}

			public void InvalidateSpan(Int32 changeStart, Int32 changeEnd)
			{
				Int32 startOfLineFromPosition = _textBuffer.GetStartOfLineFromPosition(changeStart);
				Int32 endOfLineFromPosition = _textBuffer.GetEndOfLineFromPosition(changeEnd);
				Int32 num3 = 0;
				while (num3 < _cachedLines.Count)
				{
					LineWidth width = _cachedLines[num3];
					if ((width.Start >= startOfLineFromPosition) && (width.Start <= endOfLineFromPosition))
					{
						if (width.Width == _maxWidth)
						{
							_maxWidth = Double.MaxValue;
						}
						_cachedLines[num3] = _cachedLines[_cachedLines.Count - 1];
						_cachedLines.RemoveAt(_cachedLines.Count - 1);
					}
					else
					{
						num3++;
					}
				}
			}

			#endregion

			private List<LineWidth> _cachedLines = new List<LineWidth>(50);
			private const Int32 _cacheSize = 50;
			private Double _maxWidth = Double.MaxValue;
			private TextBuffer _textBuffer;

			/// <summary>
			/// Initializes a new instance of the <see cref="LineWidthCache"/> class.
			/// </summary>
			public LineWidthCache(TextBuffer textBuffer)
			{
				_textBuffer = textBuffer;
			}
		}
	}
}

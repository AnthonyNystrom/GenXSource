/* -----------------------------------------------
 * NormalizedSpanManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Logic.Text.Classification;
using Genetibase.Windows.Controls.Editor.Text.Classification;
using System.Windows.Media.TextFormatting;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Editor
{
	internal class NormalizedSpanManager
	{
		private IClassificationFormatMap _classificationFormatMap;
		private IList<ClassificationSpan> _classificationSpanList;
		private Boolean _containsBiDi;
		private const String _gapClass = "_gap_";
		private static TextEndOfLine _lineBreak = new TextEndOfLine(1);
		private NormalizedSpan _predictedNode;
		private IList<SpaceNegotiation> _spaceNegotiationList;
		private Int32 _startCharacterIndex;
		private NormalizedSpan _startNode;
		private String _text;
		private List<Int32> _virtualCharacterPositions;

		public NormalizedSpanManager(String text, Int32 startCharacterIndex, IList<ClassificationSpan> classificationSpans, IList<SpaceNegotiation> spaceNegotiations, IClassificationFormatMap classificationFormatMap)
		{
			_text = text;
			_startCharacterIndex = startCharacterIndex;
			_classificationSpanList = classificationSpans;
			_spaceNegotiationList = spaceNegotiations;
			_classificationFormatMap = classificationFormatMap;
			_virtualCharacterPositions = new List<Int32>();
			this.CreateNormalizedSpans();
			this.InsertSpaceNegotiationSpans();
			this.AddTextModifiers();
			this.MergeSpans();
			NormalizedSpan next = _startNode;
			Int32 num = 0;
			while (next != null)
			{
				next.StartCharacterIndex = num;
				num += next.Length;
				next = next.Next;
			}
			_predictedNode = _startNode;
		}

		private void AddTextModifiers()
		{
			for (NormalizedSpan next = _startNode; next != null; next = next.Next)
			{
				if (next.AddTextModifiers())
				{
					_containsBiDi = true;
					_virtualCharacterPositions.Add(next.StartCharacterIndex - _startCharacterIndex);
					_virtualCharacterPositions.Add((next.StartCharacterIndex + next.Length) - _startCharacterIndex);
					next = next.Next;
				}
			}
			if (_startNode.Previous != null)
			{
				_startNode = _startNode.Previous;
			}
		}

		private void CreateNormalizedSpans()
		{
			Int32 startCharacterIndex = _startCharacterIndex;
			Int32 length = _text.Length;
			NormalizedSpan span = new NormalizedSpan("", "_gap_", _startCharacterIndex, TextFormattingRunProperties.DefaultProperties);
			_startNode = span;
			for (Int32 i = 0; i < _classificationSpanList.Count; i++)
			{
				ClassificationSpan span2 = _classificationSpanList[i];
				Span span3 = span2.Span();
				if (span3.Start >= (_startCharacterIndex + length))
				{
					break;
				}
				Int32 num4 = span3.Start - startCharacterIndex;
				if (num4 > 0)
				{
					NormalizedSpan span4 = new NormalizedSpan(_text.Substring(startCharacterIndex - _startCharacterIndex, span3.Start - startCharacterIndex), "_gap_", startCharacterIndex, TextFormattingRunProperties.DefaultProperties);
					span = span.AddNode(span4);
					startCharacterIndex += num4;
				}
				else if (num4 < 0)
				{
					Int32 num5 = span3.Length + num4;
					if (num5 <= 0)
					{
						continue;
					}
					span3 = new Span(startCharacterIndex, num5);
				}
				if (span3.Length > 0)
				{
					Int32 num6 = startCharacterIndex - _startCharacterIndex;
					TextFormattingRunProperties textProperties = _classificationFormatMap.GetTextProperties(span2.Classification);
					if ((span3.Length + num6) > length)
					{
						Int32 num7 = length - num6;
						NormalizedSpan span5 = new NormalizedSpan(_text.Substring(span3.Start - _startCharacterIndex, num7), span2.Classification, span3.Start, textProperties);
						span = span.AddNode(span5);
						startCharacterIndex = _startCharacterIndex + length;
						break;
					}
					NormalizedSpan span6 = new NormalizedSpan(_text.Substring(span3.Start - _startCharacterIndex, span3.Length), span2.Classification, span3.Start, textProperties);
					span = span.AddNode(span6);
					startCharacterIndex += span3.Length;
				}
			}
			if (startCharacterIndex < (_startCharacterIndex + length))
			{
				Int32 num8 = length - (startCharacterIndex - _startCharacterIndex);
				NormalizedSpan span7 = new NormalizedSpan(_text.Substring(startCharacterIndex - _startCharacterIndex, num8), "_gap_", startCharacterIndex, TextFormattingRunProperties.DefaultProperties);
				span = span.AddNode(span7);
			}
			if (_startNode.Next != null)
			{
				_startNode = _startNode.Next;
				_startNode.Previous = null;
			}
		}

		public TextRun GetTextRun(Int32 characterIndex)
		{
			NormalizedSpan next = _predictedNode;
			do
			{
				if (characterIndex >= (next.StartCharacterIndex + next.Length))
				{
					next = next.Next;
				}
				else
				{
					if (characterIndex >= next.StartCharacterIndex)
					{
						TextRun textRun = next.GetTextRun(characterIndex);
						if (next.Next != null)
						{
							_predictedNode = next.Next;
						}
						return textRun;
					}
					next = next.Previous;
				}
			}
			while (next != null);
			return _lineBreak;
		}

		private void InsertSpaceNegotiationSpans()
		{
			if (_spaceNegotiationList != null)
			{
				NormalizedSpan span = _startNode;
				foreach (SpaceNegotiation negotiation in _spaceNegotiationList)
				{
					TextRun textRun = new TextEmbeddedSpace(negotiation.Size);
					span = span.AddNode(new NormalizedSpan(textRun, (Int32)negotiation.TextPosition));
					_virtualCharacterPositions.Add(((Int32)negotiation.TextPosition) - _startCharacterIndex);
				}
			}
		}

		private void MergeSpans()
		{
			for (NormalizedSpan span = _startNode; span != null; span = span.TryMergeNextSpan())
			{
			}
		}

		public Boolean ContainsBiDiCharacters
		{
			get
			{
				return _containsBiDi;
			}
		}

		public List<Int32> VirtualCharacterPositions
		{
			get
			{
				return _virtualCharacterPositions;
			}
		}
	}
}

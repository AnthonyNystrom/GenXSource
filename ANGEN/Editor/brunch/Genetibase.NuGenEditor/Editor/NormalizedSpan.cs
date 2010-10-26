/* -----------------------------------------------
 * NormalizedSpan.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.TextFormatting;

namespace Genetibase.Windows.Controls.Editor
{
	internal class NormalizedSpan
	{
		private Boolean _canSplitOrMerge;
		private String _classification;
		private Int32 _length;
		private NormalizedSpan _next;
		private NormalizedSpan _previous;
		private TextFormattingRunProperties _properties;
		private Int32 _startCharacterIndex;
		private String _text;
		private TextRun _textRun;

		public NormalizedSpan(TextRun textRun, Int32 startCharacterIndex)
		{
			_textRun = textRun;
			_length = _textRun.Length;
			_startCharacterIndex = startCharacterIndex;
			_canSplitOrMerge = false;
		}

		public NormalizedSpan(String text, String classification, Int32 startCharacterIndex, TextFormattingRunProperties properties)
		{
			_text = text;
			_length = text.Length;
			_classification = classification;
			_startCharacterIndex = startCharacterIndex;
			_properties = properties;
			_next = (NormalizedSpan)(_previous = null);
			_canSplitOrMerge = true;
		}

		public NormalizedSpan AddNode(NormalizedSpan span)
		{
			if (span.StartCharacterIndex > (this.StartCharacterIndex + this.Length))
			{
				return this.Next.AddNode(span);
			}
			if (_canSplitOrMerge && (span.StartCharacterIndex < (this.StartCharacterIndex + this.Length)))
			{
				Int32 startCharacterIndex = span.StartCharacterIndex;
				NormalizedSpan span2 = new NormalizedSpan(_text.Substring(startCharacterIndex - this.StartCharacterIndex), _classification, startCharacterIndex, _properties);
				_length = startCharacterIndex - this.StartCharacterIndex;
				_text = _text.Substring(0, _length);
				this.AddNode(span);
				return span.AddNode(span2);
			}
			span.Previous = this;
			if (this.Next != null)
			{
				span.Next = this.Next;
				this.Next.Previous = span;
			}
			this.Next = span;
			return span;
		}

		public Boolean AddTextModifiers()
		{
			if (!_canSplitOrMerge || !TextFormattingSource.ContainsBiDiCharacters(_text))
			{
				return false;
			}
			NormalizedSpan span = new NormalizedSpan(new TextFormattingModifier(_properties), this.StartCharacterIndex);
			span.Next = this;
			span.Previous = this.Previous;
			if (this.Previous != null)
			{
				this.Previous.Next = span;
			}
			this.Previous = span;
			NormalizedSpan span2 = new NormalizedSpan(new TextEndOfSegment(1), this.StartCharacterIndex + this.Length);
			span2.Next = this.Next;
			span2.Previous = this;
			if (this.Next != null)
			{
				this.Next.Previous = span2;
			}
			this.Next = span2;
			return true;
		}

		public TextRun GetTextRun(Int32 startCharacterIndex)
		{
			if (_textRun != null)
			{
				return _textRun;
			}
			String characterString = _text;
			if (startCharacterIndex > _startCharacterIndex)
			{
				characterString = _text.Substring(startCharacterIndex - _startCharacterIndex);
			}
			return new TextCharacters(characterString, _properties);
		}

		public NormalizedSpan TryMergeNextSpan()
		{
			if ((!_canSplitOrMerge || (this.Next == null)) || !this.Next._canSplitOrMerge)
			{
				return this.Next;
			}
			Boolean flag = false;
			if ((_classification == this.Next._classification) || (_properties == this.Next._properties))
			{
				flag = true;
			}
			else if (_properties.SameSize(this.Next._properties))
			{
				if (_classification == "whitespace")
				{
					flag = true;
					_classification = this.Next._classification;
					_properties = this.Next._properties;
				}
				else if (this.Next._classification == "whitespace")
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return this.Next;
			}
			_text = _text + this.Next._text;
			_length += this.Next._length;
			this.Next = this.Next.Next;
			if (this.Next != null)
			{
				this.Next.Previous = this;
			}
			return this;
		}

		public Int32 Length
		{
			get
			{
				return _length;
			}
		}

		public NormalizedSpan Next
		{
			get
			{
				return _next;
			}
			set
			{
				_next = value;
			}
		}

		public NormalizedSpan Previous
		{
			get
			{
				return _previous;
			}
			set
			{
				_previous = value;
			}
		}

		public Int32 StartCharacterIndex
		{
			get
			{
				return _startCharacterIndex;
			}
			internal set
			{
				_startCharacterIndex = value;
			}
		}
	}
}

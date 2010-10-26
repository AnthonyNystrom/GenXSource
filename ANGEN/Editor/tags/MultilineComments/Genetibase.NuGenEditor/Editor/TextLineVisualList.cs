/* -----------------------------------------------
 * TextLineVisualList.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Editor.Text.View;

namespace Genetibase.Windows.Controls.Editor
{
	internal class TextLineVisualList : IList<TextLineVisual>, ICollection<TextLineVisual>, IEnumerable<TextLineVisual>, IEnumerable
	{
		private List<ITextLine> _textLineList = new List<ITextLine>();
		private List<TextLineVisual> _textLineVisualList = new List<TextLineVisual>();

		internal TextLineVisualList()
		{
		}

		public void Add(TextLineVisual value)
		{
			_textLineList.Add(value);
			_textLineVisualList.Add(value);
		}

		public void Clear()
		{
			_textLineList.Clear();
			_textLineVisualList.Clear();
		}

		public Boolean Contains(TextLineVisual value)
		{
			return _textLineVisualList.Contains(value);
		}

		public void CopyTo(TextLineVisual[] array, Int32 arrayIndex)
		{
			_textLineVisualList.CopyTo(array, arrayIndex);
		}

		public IEnumerator<TextLineVisual> GetEnumerator()
		{
			return _textLineVisualList.GetEnumerator();
		}

		public Int32 IndexOf(TextLineVisual value)
		{
			return _textLineVisualList.IndexOf(value);
		}

		public void Insert(Int32 index, TextLineVisual value)
		{
			_textLineList.Insert(index, value);
			_textLineVisualList.Insert(index, value);
		}

		public Boolean Remove(TextLineVisual value)
		{
			_textLineList.Remove(value);
			return _textLineVisualList.Remove(value);
		}

		public void RemoveAt(Int32 index)
		{
			_textLineList.RemoveAt(index);
			_textLineVisualList.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _textLineVisualList.GetEnumerator();
		}

		public Int32 Count
		{
			get
			{
				return _textLineVisualList.Count;
			}
		}

		public Boolean IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public TextLineVisual this[Int32 index]
		{
			get
			{
				return _textLineVisualList[index];
			}
			set
			{
				_textLineList[index] = value;
				_textLineVisualList[index] = value;
			}
		}

		public IList<ITextLine> TextLineList
		{
			get
			{
				return _textLineList.AsReadOnly();
			}
		}
	}
}

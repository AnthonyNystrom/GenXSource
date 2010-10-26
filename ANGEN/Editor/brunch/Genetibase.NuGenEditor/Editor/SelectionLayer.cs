/* -----------------------------------------------
 * SelectionLayer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genetibase.Windows.Controls.Data.Text;
using Genetibase.Windows.Controls.Editor.Text.View;

namespace Genetibase.Windows.Controls.Editor
{
	internal class SelectionLayer : Canvas, ITextSelection, IEnumerable<TextSpan>, IEnumerable
	{
		private EditorView _editorView;
		private Dictionary<TextSpan, Geometry> _geometryLookup;
		private Boolean _isActiveSpanReversed;
		private Brush _selectionBrush;
		private Pen _selectionPen;
		private List<TextSpan> _textSpanList;
		private SourceEditorViewHelper _editorViewHelper;

		public event EventHandler SelectionChanged;

		public SelectionLayer(EditorView editorView)
		{
			_editorView = editorView;
			_textSpanList = new List<TextSpan>();
			_editorViewHelper = new SourceEditorViewHelper(editorView);
			_geometryLookup = new Dictionary<TextSpan, Geometry>();
			Color highlightColor = SystemColors.HighlightColor;
			Color startColor = Color.FromArgb(0x60, highlightColor.R, highlightColor.G, highlightColor.B);
			Color color = Color.FromArgb(0x30, highlightColor.R, highlightColor.G, highlightColor.B);
			LinearGradientBrush brush = new LinearGradientBrush(startColor, startColor, 90);
			brush.GradientStops.Add(new GradientStop(color, 0.5));
			_selectionBrush = brush;
			_selectionPen = new Pen(SystemColors.HighlightBrush, 1);
			_selectionPen.LineJoin = PenLineJoin.Round;
			if (_selectionBrush.CanFreeze)
			{
				_selectionBrush.Freeze();
			}
			if (_selectionPen.CanFreeze)
			{
				_selectionPen.Freeze();
			}
			_editorView.LayoutChanged += new EventHandler<TextViewLayoutChangedEventArgs>(this.TextView_LayoutChanged);
			this.SelectionChanged += this.SelectionLayer_SelectionChanged;
		}

		public void Clear()
		{
			_textSpanList.Clear();
			this.SelectionChanged(this, new EventArgs());
		}

		private void EnsureTextSpanSpansAcrossTextElement(ref TextSpan span)
		{
			if (!span.IsEmpty)
			{
				Span textElementSpan = _editorView.GetTextElementSpan(span.Start);
				Span span3 = _editorView.GetTextElementSpan(span.End - 1);
				if ((span.Start != textElementSpan.Start) || (span.End != span3.End))
				{
					span = new TextSpan(_editorView.TextBuffer, textElementSpan.Start, span3.End - textElementSpan.Start);
				}
			}
		}

		public IEnumerator<TextSpan> GetEnumerator()
		{
			return _textSpanList.GetEnumerator();
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			foreach (TextSpan span in _textSpanList)
			{
				Geometry geometry = _geometryLookup[span];
				if (geometry != null)
				{
					drawingContext.DrawGeometry(_selectionBrush, _selectionPen, geometry);
				}
			}
		}

		public void Select(TextSpan span)
		{
			this.EnsureTextSpanSpansAcrossTextElement(ref span);
			_textSpanList.Add(span);
			this.SelectionChanged(this, new EventArgs());
		}

		private void SelectionLayer_SelectionChanged(Object sender, EventArgs e)
		{
			this.UpdateGeometries();
			base.InvalidateVisual();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _textSpanList.GetEnumerator();
		}

		private void TextView_LayoutChanged(Object sender, EventArgs e)
		{
			this.UpdateGeometries();
			base.InvalidateVisual();
		}

		private void UpdateGeometries()
		{
			_geometryLookup.Clear();
			foreach (TextSpan span in _textSpanList)
			{
				Geometry markerGeometry = _editorViewHelper.GetMarkerGeometry(span);
				_geometryLookup[span] = markerGeometry;
			}
		}

		public TextSpan ActiveSpan
		{
			get
			{
				if (this.IsEmpty)
				{
					return new TextSpan(_editorView.TextBuffer, 0, 0);
				}
				return _textSpanList[_textSpanList.Count - 1];
			}
			set
			{
				this.EnsureTextSpanSpansAcrossTextElement(ref value);
				if (this.IsEmpty)
				{
					_textSpanList.Add(value);
				}
				else
				{
					_textSpanList[_textSpanList.Count - 1] = value;
				}
				this.SelectionChanged(this, new EventArgs());
			}
		}

		public Int32 Count
		{
			get
			{
				return _textSpanList.Count;
			}
		}

		public Boolean IsActiveSpanReversed
		{
			get
			{
				return _isActiveSpanReversed;
			}
			set
			{
				_isActiveSpanReversed = value;
			}
		}

		public Boolean IsEmpty
		{
			get
			{
				foreach (TextSpan span in _textSpanList)
				{
					if (!span.IsEmpty)
					{
						return false;
					}
				}
				return true;
			}
		}

		public TextSpan this[Int32 index]
		{
			get
			{
				return _textSpanList[index];
			}
		}
	}
}

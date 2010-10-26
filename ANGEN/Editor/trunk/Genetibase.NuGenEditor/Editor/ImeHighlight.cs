/* -----------------------------------------------
 * ImeHighlight.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using Genetibase.WinApi;
using Genetibase.Windows.Controls.Data.Text;
using Genetibase.Windows.Controls.Editor.Text.View;

namespace Genetibase.Windows.Controls.Editor
{
	internal sealed class ImeHighlight : FrameworkElement
	{
		private SourceEditorViewHelper _editorHelper;
		private EditorView _editorView;
		private Brush _baseBrush;
		private Brush _blinkBrush;
		private Timer _blinkTimer;
		private Geometry _highlightGeometry;
		private TextSpan _provisionalSpan;
		private Boolean _useBlinkBrush = true;

		public ImeHighlight(EditorView editorView)
		{
			_editorView = editorView;
			_editorHelper = new SourceEditorViewHelper(_editorView);
			Color highlightColor = SystemColors.HighlightColor;
			Color color = Color.FromArgb(0x60, highlightColor.R, highlightColor.G, highlightColor.B);
			Color color3 = Color.FromArgb(180, (Byte)(highlightColor.R / 3), (Byte)(highlightColor.G / 3), (Byte)(highlightColor.B / 3));
			_baseBrush = new SolidColorBrush(color);
			_blinkBrush = new SolidColorBrush(color3);
			_provisionalSpan = null;
			Int32 caretBlinkTime = User32.GetCaretBlinkTime();
			if (caretBlinkTime > 0)
			{
				_blinkTimer = new Timer((Double)caretBlinkTime);
				_blinkTimer.AutoReset = true;
			}
			base.Visibility = Visibility.Hidden;
			base.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnThisVisibilityChanged);
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			if ((base.Visibility == Visibility.Visible) && (_highlightGeometry != null))
			{
				drawingContext.DrawGeometry(_useBlinkBrush ? _blinkBrush : _baseBrush, null, _highlightGeometry);
			}
		}

		private void OnTextBufferChanged(Object sender, TextChangedEventArgs e)
		{
			this.RecomputeGeometry();
		}

		private void OnThisVisibilityChanged(Object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((Boolean)e.NewValue)
			{
				_editorView.TextBuffer.Changed += new EventHandler<TextChangedEventArgs>(this.OnTextBufferChanged);
				if (_blinkTimer != null)
				{
					_blinkTimer.Enabled = true;
					_blinkTimer.Elapsed += new ElapsedEventHandler(this.OnTimerElapsed);
					_useBlinkBrush = true;
				}
			}
			else
			{
				_editorView.TextBuffer.Changed -= new EventHandler<TextChangedEventArgs>(this.OnTextBufferChanged);
				if (_blinkTimer != null)
				{
					_blinkTimer.Enabled = false;
					_blinkTimer.Elapsed -= new ElapsedEventHandler(this.OnTimerElapsed);
				}
			}
		}

		private void OnTimerElapsed(Object sender, ElapsedEventArgs e)
		{
			base.InvalidateVisual();
			_useBlinkBrush = !_useBlinkBrush;
		}

		private void RecomputeGeometry()
		{
			if (_provisionalSpan == null)
			{
				_highlightGeometry = null;
			}
			else
			{
				_highlightGeometry = _editorHelper.GetMarkerGeometry(_provisionalSpan);
			}
			base.InvalidateVisual();
		}

		public Span ProvisionalSpan
		{
			get
			{
				return _provisionalSpan;
			}
			set
			{
				if (value == null)
				{
					if (_provisionalSpan != null)
					{
						base.Visibility = Visibility.Hidden;
					}
					_provisionalSpan = null;
				}
				else
				{
					if (_provisionalSpan == null)
					{
						base.Visibility = Visibility.Visible;
					}
					_provisionalSpan = new TextSpan(_editorView.TextBuffer, value);
				}
				this.RecomputeGeometry();
			}
		}
	}
}

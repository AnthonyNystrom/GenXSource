/* -----------------------------------------------
 * CaretElement.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.TextFormatting;
using Genetibase.WinApi;
using Genetibase.Windows.Controls.Data.Text;
using Genetibase.Windows.Controls.Editor.Text;
using Genetibase.Windows.Controls.Editor.Text.View;

namespace Genetibase.Windows.Controls.Editor
{
	internal class CaretElement : FrameworkElement, ITextCaret
	{
		// Fields
		private EditorView _editorView;
		private const Double _bidiCaretIndicatorWidth = 2;
		private const Double _bidiIndicatorHeightRatio = 10;
		private AnimationClock _blinkAnimationClock;
		private Brush _caretBrush;
		private Geometry _caretGeometry;
		private CaretPlacement _caretPlacement;
		private Double _defaultOverwriteCaretWidth;
		private Boolean _ensureVisiblePending;
		private TextPoint _insertionPoint;
		private Brush _overwriteCaretBrush;
		private Boolean _overwriteMode;
		private Double _preferredHorizontalPosition;
		private Double _preferredVerticalPosition;
		private TextViewHelper _textViewHelper;
		/// <summary>
		/// </summary>
		public const Double CaretHorizontalPadding = 2;
		/// <summary>
		/// </summary>
		public const Double HorizontalScrollbarPadding = 10;

		/// <summary>
		/// </summary>
		public event EventHandler<CaretPositionChangedEventArgs> PositionChanged;

		/// <summary>
		/// </summary>
		public CaretElement(EditorView editorView)
		{
			SizeChangedEventHandler handler = null;
			_editorView = editorView;
			_caretPlacement = CaretPlacement.LeftOfCharacter;
			_insertionPoint = new TextPoint(editorView.TextBuffer, 0);
			_preferredVerticalPosition = _preferredHorizontalPosition = 0;
			_textViewHelper = new TextViewHelper(_editorView);
			this.ConstructCaretGeometry();
			TextLine line = TextFormatter.Create().FormatLine(new DefaultLineGutterTextSource("W", TextFormattingRunProperties.DefaultProperties), 0, 10, new TextFormattingParagraphProperties(), null);
			_defaultOverwriteCaretWidth = line.Width;
			_overwriteCaretBrush = new SolidColorBrush(Colors.Gray);
			_overwriteCaretBrush.Opacity = 0.5;
			DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames();
			frames.BeginTime = new TimeSpan((long)0);
			frames.RepeatBehavior = RepeatBehavior.Forever;
			frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromPercent(0)));
			Int32 caretBlinkTime = User32.GetCaretBlinkTime();
			if (caretBlinkTime > 0)
			{
				frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromPercent(0.5)));
			}
			else
			{
				caretBlinkTime = 500;
			}
			frames.Duration = new Duration(new TimeSpan(0, 0, 0, 0, caretBlinkTime * 2));
			_blinkAnimationClock = frames.CreateClock();
			base.ApplyAnimationClock(UIElement.OpacityProperty, _blinkAnimationClock);
			this.PositionChanged = (EventHandler<CaretPositionChangedEventArgs>)Delegate.Combine(this.PositionChanged, new EventHandler<CaretPositionChangedEventArgs>(this.CaretElement_PositionChanged));
			if (handler == null)
			{
				handler = delegate
				{
					this.ConstructCaretGeometry();
				};
			}
			base.SizeChanged += handler;
			base.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.UIElement_VisibleChanged);
			this.OverwriteMode = false;
		}

		/// <summary>
		/// </summary>
		public void CaptureHorizontalPosition()
		{
			_preferredHorizontalPosition = this.HorizontalOffset;
		}

		/// <summary>
		/// </summary>
		public void CaptureVerticalPosition()
		{
			_preferredVerticalPosition = Math.Min(_editorView.ViewportHeight, Math.Max((Double)0, (Double)(this.VerticalOffset + (base.Height * 0.5))));
		}

		private void CaretElement_PositionChanged(Object sender, CaretPositionChangedEventArgs e)
		{
			_ensureVisiblePending = false;
			this.UpdateCaret();
		}

		private void ConstructCaretGeometry()
		{
			PathGeometry geometry = new PathGeometry();
			geometry.AddGeometry(new RectangleGeometry(new Rect(0, 0, base.Width, base.Height)));
			if (InputLanguageManager.Current.CurrentInputLanguage.TextInfo.IsRightToLeft)
			{
				PathFigure figure = new PathFigure();
				figure.StartPoint = new Point(0, 0);
				figure.Segments.Add(new LineSegment(new Point(-2, 0), true));
				figure.Segments.Add(new LineSegment(new Point(0, base.Height / 10), true));
				figure.IsClosed = true;
				geometry.Figures.Add(figure);
			}
			_caretGeometry = geometry;
			if (_caretGeometry.CanFreeze)
			{
				_caretGeometry.Freeze();
			}
			base.InvalidateVisual();
		}

		private ICaretPosition CreateCaretPosition(Int32 characterIndex, CaretPlacement caretPlacement)
		{
			Span textElementSpan = _editorView.GetTextElementSpan(characterIndex);
			return new CaretPosition(textElementSpan.Start, (caretPlacement == CaretPlacement.LeftOfCharacter) ? textElementSpan.Start : textElementSpan.End, (textElementSpan.Length == 0) ? CaretPlacement.LeftOfCharacter : caretPlacement);
		}

		/// <summary>
		/// </summary>
		public void EnsureVisible()
		{
			this.EnsureVisible(0, ViewRelativePosition.Top);
		}

		/// <summary>
		/// </summary>
		public void EnsureVisible(Double verticalPadding, ViewRelativePosition relativeTo)
		{
			if (_editorView.TextLines.Count == 0)
			{
				_ensureVisiblePending = true;
			}
			else
			{
				Span span = new Span(this.Position.TextInsertionIndex, 0);
				if (!_textViewHelper.EnsureSpanVisible(span, 2 + base.Width, verticalPadding))
				{
					_textViewHelper.EnsureSpanVisible(span, 0, 0);
				}
			}
		}

		private void InputLanguage_InputLanguageChanged(Object sender, InputLanguageEventArgs e)
		{
			this.ConstructCaretGeometry();
		}

		/// <summary>
		/// </summary>
		public ICaretPosition MoveTo(Int32 characterIndex)
		{
			return this.MoveTo(characterIndex, CaretPlacement.LeftOfCharacter);
		}

		/// <summary>
		/// </summary>
		public ICaretPosition MoveTo(Int32 characterIndex, CaretPlacement caretPlacement)
		{
			if (_overwriteMode)
			{
				caretPlacement = CaretPlacement.LeftOfCharacter;
			}
			ICaretPosition oldPosition = this.Position;
			Span textElementSpan = _editorView.GetTextElementSpan(characterIndex);
			if (textElementSpan == null)
			{
				return oldPosition;
			}
			_caretPlacement = caretPlacement;
			if ((_caretPlacement == CaretPlacement.RightOfCharacter) && (characterIndex < _editorView.TextBuffer.Length))
			{
				_insertionPoint = new TextPoint(_editorView.TextBuffer, textElementSpan.End);
			}
			else
			{
				_insertionPoint = new TextPoint(_editorView.TextBuffer, textElementSpan.Start);
			}
			ICaretPosition position = this.Position;
			this.PositionChanged(this, new CaretPositionChangedEventArgs(oldPosition, position));
			return position;
		}

		/// <summary>
		/// </summary>
		public ICaretPosition MoveToNextCaretPosition()
		{
			ICaretPosition position = this.Position;
			if (position.TextInsertionIndex >= _editorView.TextBuffer.Length)
			{
				return position;
			}
			Int32 endOfLineFromPosition = _editorView.TextBuffer.GetEndOfLineFromPosition(position.TextInsertionIndex);
			if (position.TextInsertionIndex >= endOfLineFromPosition)
			{
				return this.MoveTo(_editorView.TextBuffer.GetStartOfNextLineFromPosition(endOfLineFromPosition), CaretPlacement.LeftOfCharacter);
			}
			Span textElementSpan = _editorView.GetTextElementSpan(position.TextInsertionIndex);
			if (!_overwriteMode)
			{
				return this.MoveTo(textElementSpan.Start, CaretPlacement.RightOfCharacter);
			}
			return this.MoveTo(textElementSpan.End, CaretPlacement.LeftOfCharacter);
		}

		/// <summary>
		/// </summary>
		public ICaretPosition MoveToPreviousCaretPosition()
		{
			ICaretPosition position = this.Position;
			Int32 textInsertionIndex = position.TextInsertionIndex;
			if (textInsertionIndex == 0)
			{
				return position;
			}
			if (textInsertionIndex == _editorView.TextBuffer.GetStartOfLineFromPosition(textInsertionIndex))
			{
				Int32 startOfPreviousLineFromPosition = _editorView.TextBuffer.GetStartOfPreviousLineFromPosition(textInsertionIndex);
				Int32 endOfLineFromPosition = _editorView.TextBuffer.GetEndOfLineFromPosition(startOfPreviousLineFromPosition);
				if (textInsertionIndex > endOfLineFromPosition)
				{
					return this.MoveTo(endOfLineFromPosition, CaretPlacement.LeftOfCharacter);
				}
				return this.MoveTo(textInsertionIndex, CaretPlacement.RightOfCharacter);
			}
			Span textElementSpan = _editorView.GetTextElementSpan((position.CharacterIndex > 0) ? (position.CharacterIndex - 1) : 0);
			if (!_overwriteMode)
			{
				return this.MoveTo((position.Placement == CaretPlacement.RightOfCharacter) ? ((position.CharacterIndex == 0) ? textElementSpan.Start : textElementSpan.End) : textElementSpan.Start, CaretPlacement.LeftOfCharacter);
			}
			return this.MoveTo(textElementSpan.Start, CaretPlacement.LeftOfCharacter);
		}

		#region Methods.Protected.Overridden

		/// <summary>
		/// </summary>
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			if ((base.Visibility == Visibility.Visible) && (base.Height != 0))
			{
				drawingContext.DrawGeometry(_caretBrush, null, _caretGeometry);
				_blinkAnimationClock.Controller.Begin();
			}
			else
			{
				_blinkAnimationClock.Controller.Stop();
			}
		}

		#endregion

		#region EventHandlers.TextView

		private void TextView_LayoutChanged(Object sender, EventArgs e)
		{
			if (_ensureVisiblePending)
			{
				this.EnsureVisible();
			}
			this.UpdateCaret();
		}

		#endregion

		#region EventHandlers.UIElement

		private void UIElement_VisibleChanged(Object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((Boolean)e.NewValue)
			{
				_editorView.LayoutChanged += new EventHandler<TextViewLayoutChangedEventArgs>(this.TextView_LayoutChanged);
				InputLanguageManager.Current.InputLanguageChanged += new InputLanguageEventHandler(this.InputLanguage_InputLanguageChanged);
				this.UpdateCaret();
				this.ConstructCaretGeometry();
			}
			else
			{
				_editorView.LayoutChanged -= new EventHandler<TextViewLayoutChangedEventArgs>(this.TextView_LayoutChanged);
				InputLanguageManager.Current.InputLanguageChanged -= new InputLanguageEventHandler(this.InputLanguage_InputLanguageChanged);
			}
		}

		#endregion

		#region Methods.Private

		private void UpdateCaret()
		{
			Int32 textInsertionIndex = this.Position.TextInsertionIndex;
			ITextLine textLineContaining = _textViewHelper.GetTextLineContaining(textInsertionIndex);
			if (textLineContaining == null)
			{
				base.Height = 0;
				if ((_editorView.TextLines.Count == 0) || (textInsertionIndex < _editorView.TextLines[0].LineSpan.Start))
				{
					Canvas.SetTop(this, 0);
				}
				else
				{
					Canvas.SetTop(this, _editorView.ViewportHeight);
				}
				Canvas.SetLeft(this, 0);
			}
			else
			{
				Canvas.SetTop(this, textLineContaining.VerticalOffset);
				base.Height = textLineContaining.Height;
				Genetibase.Windows.Controls.Editor.Text.View.TextBounds characterBounds = textLineContaining.GetCharacterBounds(this.Position.CharacterIndex);
				if (!_overwriteMode)
				{
					if (this.Placement == CaretPlacement.LeftOfCharacter)
					{
						Canvas.SetLeft(this, characterBounds.Left);
					}
					else
					{
						Canvas.SetLeft(this, characterBounds.Right);
					}
				}
				else if (characterBounds.Right > characterBounds.Left)
				{
					base.Width = characterBounds.Right - characterBounds.Left;
					Canvas.SetLeft(this, characterBounds.Left);
				}
				else if (characterBounds.Left > characterBounds.Right)
				{
					base.Width = characterBounds.Left - characterBounds.Right;
					Canvas.SetLeft(this, characterBounds.Right);
				}
				else
				{
					base.Width = _defaultOverwriteCaretWidth;
					Canvas.SetLeft(this, characterBounds.Left);
				}
				_blinkAnimationClock.Controller.Begin();
			}
			base.InvalidateVisual();
		}

		#endregion

		#region Properties.Public

		/// <summary>
		/// </summary>
		public Double HorizontalOffset
		{
			get
			{
				return Canvas.GetLeft(this);
			}
		}

		/// <summary>
		/// </summary>
		public Boolean OverwriteMode
		{
			get
			{
				return _overwriteMode;
			}
			set
			{
				_overwriteMode = value;
				if (!_overwriteMode)
				{
					_caretBrush = new SolidColorBrush(Colors.Black);
					_caretBrush.Opacity = 1;
					base.Width = SystemParameters.CaretWidth;
				}
				else
				{
					_caretBrush = _overwriteCaretBrush;
				}
				if (_caretBrush.CanFreeze)
				{
					_caretBrush.Freeze();
				}
				this.UpdateCaret();
			}
		}

		/// <summary>
		/// </summary>
		public Brush OverwriteModeBrush
		{
			get
			{
				return _overwriteCaretBrush;
			}
			set
			{
				_overwriteCaretBrush = value;
				this.UpdateCaret();
			}
		}

		/// <summary>
		/// </summary>
		public CaretPlacement Placement
		{
			get
			{
				return _caretPlacement;
			}
		}

		/// <summary>
		/// </summary>
		public ICaretPosition Position
		{
			get
			{
				if ((_caretPlacement == CaretPlacement.RightOfCharacter) && (_insertionPoint.Position > 0))
				{
					return this.CreateCaretPosition(_insertionPoint.Position - 1, CaretPlacement.RightOfCharacter);
				}
				return this.CreateCaretPosition(_insertionPoint.Position, CaretPlacement.LeftOfCharacter);
			}
		}

		/// <summary>
		/// </summary>
		public Double PreferredHorizontalPosition
		{
			get
			{
				return _preferredHorizontalPosition;
			}
			set
			{
				_preferredHorizontalPosition = value;
			}
		}

		/// <summary>
		/// </summary>
		public Double PreferredVerticalPosition
		{
			get
			{
				return _preferredVerticalPosition;
			}
		}

		/// <summary>
		/// </summary>
		public Double VerticalOffset
		{
			get
			{
				return Canvas.GetTop(this);
			}
		}

		#endregion
	}
}

/* -----------------------------------------------
 * EditorArea.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Genetibase.Windows.Controls.Editor;
using Genetibase.Windows.Controls.Editor.Adornment;
using Genetibase.Windows.Controls.Editor.AdornmentSurface;
using Genetibase.Windows.Controls.Editor.AdornmentSurfaceManager;
using Genetibase.Windows.Controls.Editor.Classification;
using Genetibase.Windows.Controls.Editor.View;

namespace Genetibase.Windows.Controls
{
    /// <summary>
    /// </summary>
    public class EditorArea : ContentControl, IEditorArea, ITextArea, IPropertyOwner, IAdornmentSurfaceHost, IValueProvider
    {
        private List<IAdornment> _adornmentList;
        private IAdornmentProvider _adornmentProvider;
        private IAdornmentSurfaceManager _adornmentSurfaceManager;
        private Canvas _baseLayer;
        private CaretElement _caretElement;
        private const Double _caretPadding = 100;
        private IClassifier _classifier;
        private TextContentLayer _contentLayer;
        private Canvas _controlHostLayer;
        private const Double _defaultViewportMargin = 2;
        private IEditorCommands _editorCommands;
        private Boolean _imeEnabled;
        private ImeHighlight _imeHighlight;
        private HwndSourceHook _immHook;
        private Boolean _immHookSet;
        private List<TextLineVisual> _invalidatedLines;
        private ITextVersion _lastUpdatedTextVersion;
        private Int32 _layoutChangeEnd;
        private Int32 _layoutChangeStart = 0x7fffffff;
        private LineWidthCache _lineWidthCache;
        private Dictionary<Object, Object> _properties;
        private SelectionLayer _selectionLayer;
        private Int32 _startLine;
        private ViewRelativePosition _startLinePosition;
        private Double _startLinePositionOffset;
        private TextLineVisualList _textLineVisuals;
        private EditorViewHelper _textViewHelper;
        private Double _viewportHorizontalOffset;
        private Double _viewportMargin = 2;
        private Double _viewportVerticalOffset;
        private VisualsFactory _visualsFactory;
        private TextBuffer _textBuffer;

        /// <summary>
        /// </summary>
        public event EventHandler<TextAreaLayoutChangedEventArgs> LayoutChanged;

        /// <summary>
        /// </summary>
        public event EventHandler<TextAreaInputEventArgs> TextAreaInputEvent;

        /// <summary>
        /// </summary>
        public event EventHandler ViewportHeightChanged;

        /// <summary>
        /// </summary>
        public event EventHandler ViewportHorizontalOffsetChanged;

        /// <summary>
        /// </summary>
        public event EventHandler ViewportVerticalOffsetChanged;

        /// <summary>
        /// </summary>
        public event EventHandler ViewportWidthChanged;

        /// <summary>
        /// </summary>
        public EditorArea(TextBuffer textBuffer)
        {
            InputMethod.SetIsInputMethodSuspended(this, true);
            _textBuffer = textBuffer;
            _lineWidthCache = new LineWidthCache(textBuffer);
            _lastUpdatedTextVersion = _textBuffer.Version;
            _textViewHelper = new EditorViewHelper(this);

            _adornmentSurfaceManager = EditorConnector.GetAdornmentSurfaceManager(this);

            _immHook = new HwndSourceHook(this.WndProc);
            _textLineVisuals = new TextLineVisualList();
            _invalidatedLines = new List<TextLineVisual>();

            _classifier = EditorConnector.GetClassifierAggregator(_textBuffer);
            _adornmentProvider = EditorConnector.GetAdornmentAggregator(this);
            
            _classifier.ClassificationChanged += new EventHandler<ClassificationChangedEventArgs>(this.OnClassificationChanged);
            _adornmentProvider.AdornmentsChanged += new EventHandler<AdornmentsChangedEventArgs>(this.OnAdornmentsChanged);
            _visualsFactory = new VisualsFactory(this, _classifier, 0);
            _visualsFactory.TabSize = 4;
            this.InitializeLayers();
            _startLine = 0;
            _startLinePosition = ViewRelativePosition.Top;
            _startLinePositionOffset = 0;
            _viewportHorizontalOffset = 0;
            _viewportVerticalOffset = 0;
            _adornmentList = new List<IAdornment>();
            _caretElement = new CaretElement(this);
            _baseLayer.Children.Add(_caretElement);
            _caretElement.Visibility = Visibility.Hidden;
            this.PerformLayout();
            _textBuffer.Changed += new EventHandler<Genetibase.Windows.Controls.Editor.TextChangedEventArgs>(this.OnTextBufferChanged);
            base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
            base.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnGotFocus);
            base.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnLostFocus);

            _editorCommands = EditorConnector.CreateEditorCommands(this);

            base.TextInput += new TextCompositionEventHandler(this.OnTextInput);
        }

        /// <summary>
        /// </summary>
        public void AddAdornmentSurface(IAdornmentSurface adornmentSurface)
        {
            if (adornmentSurface == null)
            {
                throw new ArgumentNullException("adornmentSurface");
            }
            switch (adornmentSurface.SurfacePosition)
            {
                case SurfacePosition.Bottommost:
                    _baseLayer.Children.Insert(0, adornmentSurface.SurfacePanel);
                    break;

                case SurfacePosition.BelowSelection:
                    _baseLayer.Children.Insert(_baseLayer.Children.IndexOf(_imeHighlight), adornmentSurface.SurfacePanel);
                    break;

                case SurfacePosition.AboveSelection:
                    _baseLayer.Children.Insert(_baseLayer.Children.IndexOf(_selectionLayer) + 1, adornmentSurface.SurfacePanel);
                    break;

                case SurfacePosition.BelowText:
                    _baseLayer.Children.Insert(_baseLayer.Children.IndexOf(_contentLayer), adornmentSurface.SurfacePanel);
                    break;

                case SurfacePosition.AboveText:
                    _baseLayer.Children.Insert(_baseLayer.Children.IndexOf(_contentLayer) + 1, adornmentSurface.SurfacePanel);
                    break;

                default:
                    _baseLayer.Children.Add(adornmentSurface.SurfacePanel);
                    break;
            }
            adornmentSurface.SurfacePanel.Width = _baseLayer.ActualWidth;
            adornmentSurface.SurfacePanel.Height = _baseLayer.ActualHeight;
            _baseLayer.SizeChanged += delegate {
                adornmentSurface.SurfacePanel.Width = _baseLayer.ActualWidth;
                adornmentSurface.SurfacePanel.Height = _baseLayer.ActualHeight;
            };
        }

        /// <summary>
        /// </summary>
        public void DisplayLine(Int32 lineNumber, Double lineOffset, ViewRelativePosition relativeTo)
        {
            _startLine = lineNumber;
            _startLinePosition = relativeTo;
            _startLinePositionOffset = lineOffset;
            this.PerformLayout();
        }

        private void FireLayoutChangedEvent()
        {
            if (_layoutChangeStart <= _layoutChangeEnd)
            {
                Span changeSpan = new Span(_layoutChangeStart, (_layoutChangeEnd - _layoutChangeStart) + 1);
                if (this.LayoutChanged != null)
                {
                    this.LayoutChanged(this, new TextAreaLayoutChangedEventArgs(changeSpan));
                }
                _layoutChangeStart = 0x7fffffff;
                _layoutChangeEnd = 0;
            }
        }

        private TextLineVisual GetLineVisual(Int32 lineNumber, Boolean lineWillBeRendered)
        {
            TextLineVisual visual;
            Int32 startOfLineFromLineNumber = _textBuffer.GetStartOfLineFromLineNumber(lineNumber);
            Int32 index = 0;
            if (_textViewHelper.FindTextLine(startOfLineFromLineNumber, out index))
            {
                visual = _textLineVisuals[index];
                _lineWidthCache.AddLine(new TextPoint(_textBuffer, visual.LineSpan.Start), visual.HorizontalOffset + visual.Width);
                return visual;
            }
            visual = _visualsFactory.CreateLineVisual(lineNumber)[0];
            _lineWidthCache.AddLine(new TextPoint(_textBuffer, visual.LineSpan.Start), visual.HorizontalOffset + visual.Width);
            if (lineWillBeRendered)
            {
                _textLineVisuals.Insert(index, visual);
                _contentLayer.Children.Insert(index, visual);
            }
            foreach (IAdornment adornment in _adornmentProvider.GetAdornments(new TextSpan(_textBuffer, startOfLineFromLineNumber, visual.LineSpan.Length)))
            {
                if (!_adornmentList.Contains(adornment))
                {
                    _adornmentList.Add(adornment);
                    _adornmentSurfaceManager.AddAdornment(adornment);
                }
            }
            IList<SpaceNegotiation> spaceNegotiations = _adornmentSurfaceManager.GetSpaceNegotiations(visual);
            if (spaceNegotiations.Count > 0)
            {
                if (lineWillBeRendered)
                {
                    _textLineVisuals.Remove(visual);
                    _contentLayer.Children.Remove(visual);
                }
                visual = _visualsFactory.CreateLineVisual(lineNumber, spaceNegotiations)[0];
                if (lineWillBeRendered)
                {
                    _textLineVisuals.Insert(index, visual);
                    _contentLayer.Children.Insert(index, visual);
                }
            }
            if (lineWillBeRendered)
            {
                if (visual.LineSpan.Start < _layoutChangeStart)
                {
                    _layoutChangeStart = visual.LineSpan.Start;
                }
                if (visual.LineSpan.End > _layoutChangeEnd)
                {
                    _layoutChangeEnd = visual.LineSpan.End;
                }
            }
            return visual;
        }

        /// <summary>
        /// </summary>
        public Span GetTextElementSpan(Int32 position)
        {
            if ((position < 0) || (position > _textBuffer.Length))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            TextLineVisual textLineContainingPosition = this.GetTextLineContainingPosition(position);
            if (textLineContainingPosition == null)
            {
                textLineContainingPosition = this.GetLineVisual(_textBuffer.GetLineNumberFromPosition(position), false);
            }
            if (textLineContainingPosition == null)
            {
                return new Span(position, 0);
            }
            return textLineContainingPosition.GetAvalonTextElementSpan(position);
        }

        private TextLineVisual GetTextLineContainingPosition(Int32 position)
        {
            if ((position < 0) || (position > _textBuffer.Length))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            Int32 num = _textLineVisuals.Count - 1;
            if ((num >= 0) && (position >= _textLineVisuals[0].LineSpan.Start))
            {
                if (position >= _textLineVisuals[num].LineSpan.Start)
                {
                    if (_textLineVisuals[num].ContainsPosition(position))
                    {
                        return _textLineVisuals[num];
                    }
                    return null;
                }
                num--;
                Int32 num2 = 0;
                do
                {
                    Int32 num3 = (num2 + num) / 2;
                    ITextLine line = _textLineVisuals[num3];
                    if (position < line.LineSpan.Start)
                    {
                        num = --num3;
                    }
                    else if (position >= line.LineSpan.End)
                    {
                        num2 = num3 + 1;
                    }
                    else
                    {
                        return _textLineVisuals[num3];
                    }
                }
                while (num2 <= num);
            }
            return null;
        }

        private void InitializeLayers()
        {
            base.ClipToBounds = true;
            base.Focusable = true;
            base.FocusVisualStyle = null;
            base.Cursor = Cursors.IBeam;
            _baseLayer = new Canvas();
            _baseLayer.Margin = new Thickness(this.ViewportMargin);
            _baseLayer.ClipToBounds = false;
            _imeHighlight = new ImeHighlight(this);
            _selectionLayer = new SelectionLayer(this);
            _contentLayer = new TextContentLayer();
            _baseLayer.Children.Add(_imeHighlight);
            _baseLayer.Children.Add(_selectionLayer);
            _baseLayer.Children.Add(_contentLayer);
            _controlHostLayer = new Canvas();
            _controlHostLayer.Background = SystemColors.WindowBrush;
            base.Content = _controlHostLayer;
            _controlHostLayer.Children.Add(_baseLayer);
            _controlHostLayer.SizeChanged += delegate {
                _baseLayer.Width = _selectionLayer.Width = _contentLayer.Width = _controlHostLayer.Width;
                _baseLayer.Height = _selectionLayer.Height = _contentLayer.Height = _controlHostLayer.Height;
            };
        }

        /// <summary>
        /// </summary>
        public void Invalidate()
        {
            Double viewportVerticalOffset = this.ViewportVerticalOffset;
            this.InvalidateLines(_textViewHelper.FirstRenderedCharacter, _textViewHelper.LastRenderedCharacter);
            this.PerformLayout();
            this.ViewportVerticalOffset = viewportVerticalOffset;
        }

        private void InvalidateAdornments(Span span)
        {
            for (Int32 i = _adornmentList.Count - 1; i >= 0; i--)
            {
                IAdornment adornment = _adornmentList[i];
                if (adornment.Span.Intersects(span) || span.Contains(adornment.Span))
                {
                    _adornmentSurfaceManager.RemoveAdornment(adornment);
                    _adornmentList.RemoveAt(i);
                }
            }
        }

        private void InvalidateLines(VersionedTextSpan versionedSpan)
        {
            Span span = versionedSpan.Span(_textBuffer.Version);
            if (span.Length > 0)
            {
                this.InvalidateLines(span.Start, span.End);
            }
        }

        internal void InvalidateLines(Int32 startPosition, Int32 endPosition)
        {
            _lineWidthCache.InvalidateSpan(startPosition, endPosition);
            Int32 num = 0;
            num = _textLineVisuals.Count - 1;
            while (num >= 0)
            {
                TextLineVisual item = _textLineVisuals[num];
                if (item.LineSpan.Start <= endPosition)
                {
                    _invalidatedLines.Add(item);
                    num--;
                    break;
                }
                num--;
            }
            if (num >= 0)
            {
                while (num >= 0)
                {
                    TextLineVisual visual2 = _textLineVisuals[num];
                    if (visual2.LineSpan.End <= startPosition)
                    {
                        return;
                    }
                    _invalidatedLines.Add(visual2);
                    num--;
                }
            }
        }

        private void InvalidateView()
        {
            DispatcherOperationCallback method = null;
            if (base.Dispatcher.CheckAccess())
            {
                this.PerformLayout();
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.PerformLayout();
                        return null;
                    };
                }
                base.Dispatcher.Invoke(DispatcherPriority.Normal, method, null);
            }
        }

        private static Boolean IsHangul(Char ch)
        {
            Boolean flag = false;
            if (ch >= 'ᄀ')
            {
                if (ch <= 'ᇿ')
                {
                    return true;
                }
                if (ch < '㄰')
                {
                    return flag;
                }
                if (ch <= '㆏')
                {
                    return true;
                }
                if (ch < 0xac00)
                {
                    return flag;
                }
                if (ch <= 0xd7a3)
                {
                    return true;
                }
                if ((ch >= 0xffa0) && (ch <= 0xffdf))
                {
                    flag = true;
                }
            }
            return flag;
        }

        private static Boolean IsKorean()
        {
            return (InputLanguageManager.Current.CurrentInputLanguage.LCID == 0x412);
        }

        private Int32 LayoutLinesDownward(Int32 lineNumber, ref TextLineVisual lastTextLine)
        {
            if (lineNumber >= (this.TextBuffer.LineCount - 1))
            {
                return lineNumber;
            }
            Double num = 0;
            Boolean flag = true;
            while (lineNumber < _textBuffer.LineCount)
            {
                lastTextLine = this.GetLineVisual(lineNumber, true);
                if (flag)
                {
                    flag = false;
                    if (_startLinePosition == ViewRelativePosition.Top)
                    {
                        num = _startLinePositionOffset;
                    }
                    else
                    {
                        num = (this.ViewportHeight - _startLinePositionOffset) - lastTextLine.Height;
                    }
                }
                if (lastTextLine.VerticalOffset != num)
                {
                    lastTextLine.VerticalOffset = num;
                    if (lastTextLine.LineSpan.Start < _layoutChangeStart)
                    {
                        _layoutChangeStart = lastTextLine.LineSpan.Start;
                    }
                    if (lastTextLine.LineSpan.End > _layoutChangeEnd)
                    {
                        _layoutChangeEnd = lastTextLine.LineSpan.End;
                    }
                }
                lineNumber++;
                num += lastTextLine.Height;
                if (num > (this.ViewportHeight + 1))
                {
                    break;
                }
            }
            return (lineNumber - 1);
        }

        private Int32 LayoutLinesUpward(Int32 lineNumber, out TextLineVisual firstTextLine, out TextLineVisual lastTextLine)
        {
            Double num = 0;
            Boolean flag = true;
            firstTextLine = null;
            lastTextLine = null;
            while (lineNumber >= 0)
            {
                firstTextLine = this.GetLineVisual(lineNumber, true);
                if (flag)
                {
                    lastTextLine = firstTextLine;
                    flag = false;
                    if (_startLinePosition == ViewRelativePosition.Top)
                    {
                        num = _startLinePositionOffset + firstTextLine.Height;
                    }
                    else
                    {
                        num = this.ViewportHeight - _startLinePositionOffset;
                    }
                }
                num -= firstTextLine.Height;
                if (firstTextLine.VerticalOffset != num)
                {
                    firstTextLine.VerticalOffset = num;
                    if (firstTextLine.LineSpan.Start < _layoutChangeStart)
                    {
                        _layoutChangeStart = firstTextLine.LineSpan.Start;
                    }
                    if (firstTextLine.LineSpan.End > _layoutChangeEnd)
                    {
                        _layoutChangeEnd = firstTextLine.LineSpan.End;
                    }
                }
                if (num < 0)
                {
                    break;
                }
                lineNumber--;
            }
            if (lineNumber < 0)
            {
                lineNumber++;
            }
            return lineNumber;
        }

        /// <summary>
        /// </summary>
        protected override Size MeasureOverride(Size availableSize)
        {
            if ((availableSize.Width != Double.PositiveInfinity) && (availableSize.Height != Double.PositiveInfinity))
            {
                return availableSize;
            }
            return new Size(this.TotalContentWidth, this.TotalContentHeight);
        }

        void IPropertyOwner.AddProperty(Object key, Object property)
        {
            if (_properties == null)
            {
                _properties = new Dictionary<Object, Object>();
            }
            _properties.Add(key, property);
        }

        Boolean IPropertyOwner.RemoveProperty(Object key)
        {
            if (_properties == null)
            {
                return false;
            }
            return _properties.Remove(key);
        }

        /// <summary>
        /// </summary>
        public Boolean TryGetProperty<TProperty>(Object key, out TProperty property)
        {
            if (_properties != null)
            {
                Object obj2;
                Boolean flag = _properties.TryGetValue(key, out obj2);
                property = flag ? ((TProperty) obj2) : default(TProperty);
                return flag;
            }
            property = default(TProperty);
            return false;
        }

        private void OnAdornmentsChanged(Object sender, AdornmentsChangedEventArgs e)
        {
            base.Dispatcher.Invoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate
            {
                this.InvalidateAdornments(e.ChangeSpan);
                return null;
            }), null);
            this.InvalidateLines(e.ChangeSpan.Start, e.ChangeSpan.End);
            this.InvalidateView();
        }

        private void OnClassificationChanged(Object sender, ClassificationChangedEventArgs e)
        {
            this.InvalidateLines(e.ChangeSpan);
            if (e.ChangeSpan.Version == _lastUpdatedTextVersion)
            {
                this.InvalidateView();
            }
        }

        /// <summary>
        /// </summary>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new TextEditorAutomationPeer(this);
        }

        private void OnGotFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            _caretElement.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// </summary>
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (!_immHookSet)
            {
                HwndSource hwndSource = EditorHelper.GetHwndSource(this);
                if (hwndSource != null)
                {
                    hwndSource.AddHook(_immHook);
                }
                _immHookSet = true;
            }
            EditorHelper.EnableImmComposition(this);
            base.OnGotKeyboardFocus(e);
        }

        private void OnLostFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            _caretElement.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// </summary>
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            IntPtr immContext = EditorHelper.GetImmContext(this);
            if (immContext != IntPtr.Zero)
            {
                EditorHelper.ImmNotifyIME(immContext, 0x15, 1, 0);
                EditorHelper.ImmNotifyIME(immContext, 0x11, 0, 0);
                EditorHelper.ReleaseImmContext(this, immContext);
            }
            EditorHelper.DisableImmComposition(this);
            if (_immHookSet)
            {
                HwndSource hwndSource = EditorHelper.GetHwndSource(this);
                if (hwndSource != null)
                {
                    hwndSource.RemoveHook(_immHook);
                }
                _immHookSet = false;
            }
            base.OnLostKeyboardFocus(e);
        }

        /// <summary>
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!base.IsKeyboardFocusWithin)
            {
                base.Focus();
            }
        }

        private void OnSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            if (_controlHostLayer.Width != base.ActualWidth)
            {
                _controlHostLayer.Width = base.ActualWidth;
                if (this.ViewportWidthChanged != null)
                {
                    this.ViewportWidthChanged(this, new EventArgs());
                }
            }
            if (_controlHostLayer.Height != base.ActualHeight)
            {
                _controlHostLayer.Height = base.ActualHeight;
                if (this.ViewportHeightChanged != null)
                {
                    this.ViewportHeightChanged(this, new EventArgs());
                }
            }
            this.InvalidateView();
        }

        private void OnTextBufferChanged(Object sender, Genetibase.Windows.Controls.Editor.TextChangedEventArgs e)
        {
            Int32 length = _textBuffer.Length;
            Int32 endPosition = 0;
            for (ITextVersion version = e.PriorVersion; version.Change != null; version = version.Next)
            {
                TextPoint point = new TextPoint(_textBuffer, version.Next, version.Change.Position, TrackingMode.Positive);
                TextPoint point2 = new TextPoint(_textBuffer, version.Next, version.Change.NewEnd, TrackingMode.Positive);
                if (point < length)
                {
                    length = point.Position;
                }
                if (point2 > endPosition)
                {
                    endPosition = point2.Position;
                }
            }
            if ((length == endPosition) && (length > 0))
            {
                length--;
            }
            this.InvalidateLines(length, endPosition);
            this.InvalidateView();
        }

        private void OnTextInput(Object sender, TextCompositionEventArgs e)
        {
            this.RaiseTextInputEvent(sender, new TextAreaInputEventArgs(TextInputState.Final, e.Text));
        }

        private void PerformLayout()
        {
            TextLineVisual visual2;
            TextLineVisual visual3;
            if (_startLine < 0)
            {
                _startLine = 0;
            }
            else if (_startLine > (_textBuffer.LineCount - 1))
            {
                _startLine = _textBuffer.LineCount - 1;
            }
            foreach (TextLineVisual visual in _invalidatedLines)
            {
                this.RemoveTextLine(visual);
            }
            _invalidatedLines.Clear();
            if (this.TotalContentHeight < this.ViewportHeight)
            {
                _startLine = 0;
                _startLinePosition = ViewRelativePosition.Top;
                _startLinePositionOffset = 0;
            }
            Int32 startVisibleLine = this.LayoutLinesUpward(_startLine, out visual2, out visual3);
            Int32 endVisibleLine = this.LayoutLinesDownward(_startLine, ref visual3);
            if (visual2.VerticalOffset > 0)
            {
                this.DisplayLine(0, 0, ViewRelativePosition.Top);
            }
            else if (visual3.VerticalOffset < 0)
            {
                this.DisplayLine(_textBuffer.LineCount - 1, 0, ViewRelativePosition.Top);
            }
            else
            {
                this.TrimLineVisuals(startVisibleLine, endVisibleLine);
                this.UpdateViewportCoords();
                _startLine = startVisibleLine;
                _startLinePosition = ViewRelativePosition.Top;
                _startLinePositionOffset = _textLineVisuals[0].VerticalOffset;
                _lastUpdatedTextVersion = _textBuffer.Version;
                if (_imeEnabled)
                {
                    this.PositionImmCompositionWindow();
                }
                this.FireLayoutChangedEvent();
            }
        }

        private void PositionImmCompositionWindow()
        {
            if (IsKorean())
            {
                _imeHighlight.ProvisionalSpan = _editorCommands.ProvisionalCompositionSpan;
            }
            else
            {
                ITextLine textLineContaining = _textViewHelper.GetTextLineContaining(_caretElement.Position.TextInsertionIndex);
                if (textLineContaining != null)
                {
                    IntPtr immContext = EditorHelper.GetImmContext(this);
                    if (immContext != IntPtr.Zero)
                    {
                        TextBounds characterBounds = textLineContaining.GetCharacterBounds(_caretElement.Position.CharacterIndex);
                        Double left = characterBounds.Left;
                        if (_caretElement.Position.Placement == CaretPlacement.RightOfCharacter)
                        {
                            left = characterBounds.Right;
                        }
                        left -= _viewportHorizontalOffset;
                        Double height = textLineContaining.Height;
                        Visual rootVisual = EditorHelper.GetRootVisual(this);
                        if (rootVisual != null)
                        {
                            GeneralTransform transform = base.TransformToAncestor(rootVisual);
                            Point point = transform.Transform(new Point(left, characterBounds.Top));
                            height = transform.Transform(new Point(left, characterBounds.Bottom)).Y - point.Y;
                        }
                        EditorHelper.SetImmFontHeight(immContext, (Int32) height);
                        EditorHelper.SetCompositionWindowPosition(immContext, new Point(left, characterBounds.Top), this);
                        EditorHelper.ReleaseImmContext(this, immContext);
                    }
                }
            }
        }

        private void ProcessImmCompositionString(Int32 index, String result)
        {
            if (!String.IsNullOrEmpty(result))
            {
                this.RaiseTextInputEvent(this, new TextAreaInputEventArgs(((index & 0x800) == 0) ? TextInputState.Provisional : TextInputState.Final, result));
            }
        }

        private void RaiseTextInputEvent(Object sender, TextAreaInputEventArgs e)
        {
            EventHandler<TextAreaInputEventArgs> textViewInputEvent = this.TextAreaInputEvent;
            if (textViewInputEvent != null)
            {
                textViewInputEvent(sender, e);
                _imeHighlight.ProvisionalSpan = _editorCommands.ProvisionalCompositionSpan;
            }
        }

        private void RemoveTextLine(TextLineVisual lineVisual)
        {
            if (lineVisual.LineSpan.Start < _layoutChangeStart)
            {
                _layoutChangeStart = lineVisual.LineSpan.Start;
            }
            if (lineVisual.LineSpan.End > _layoutChangeEnd)
            {
                _layoutChangeEnd = lineVisual.LineSpan.End;
            }
            _textLineVisuals.Remove(lineVisual);
            _contentLayer.Children.Remove(lineVisual);
            lineVisual.Dispose();
        }

        /// <summary>
        /// </summary>
        public Boolean ScrollViewportHorizontally(Double pixelsToScroll)
        {
            if ((pixelsToScroll < 0) && (this.ViewportHorizontalOffset == 0))
            {
                return false;
            }
            if ((pixelsToScroll > 0) && (this.ViewportHorizontalOffset >= this.TotalContentWidth))
            {
                return false;
            }
            this.ViewportHorizontalOffset += pixelsToScroll;
            if (_imeEnabled)
            {
                this.PositionImmCompositionWindow();
            }
            return true;
        }

        /// <summary>
        /// </summary>
        public Boolean ScrollViewportVertically(Double pixelsToScroll)
        {
            TextLineVisual visual = _textLineVisuals[0];
            Int32 lineNumberFromPosition = _textBuffer.GetLineNumberFromPosition(visual.LineSpan.Start);
            this.DisplayLine(lineNumberFromPosition, visual.VerticalOffset - pixelsToScroll, ViewRelativePosition.Top);
            visual = _textLineVisuals[0];
            if (visual.VerticalOffset > 0)
            {
                lineNumberFromPosition = _textBuffer.GetLineNumberFromPosition(visual.LineSpan.Start);
                this.DisplayLine(lineNumberFromPosition, 0, ViewRelativePosition.Top);
            }
            return false;
        }

        /// <summary>
        /// </summary>
        public Boolean ScrollViewportVertically(Int32 visualLinesToScroll)
        {
            if (Math.Abs(visualLinesToScroll) >= _textLineVisuals.Count)
            {
                throw new ArgumentOutOfRangeException("visualLinesToScroll");
            }
            for (Int32 i = 0; i < _textLineVisuals.Count; i++)
            {
                TextLineVisual visual = _textLineVisuals[i];
                if (visual.VerticalOffset >= 0)
                {
                    Int32 lineNumberFromPosition = _textBuffer.GetLineNumberFromPosition(visual.LineSpan.Start);
                    Int32 lineNumber = Math.Min(Math.Max(0, lineNumberFromPosition + visualLinesToScroll), _textBuffer.LineCount);
                    if ((lineNumberFromPosition == lineNumber) && (visual.VerticalOffset == 0))
                    {
                        return false;
                    }
                    this.DisplayLine(lineNumber, 0, ViewRelativePosition.Top);
                    return true;
                }
            }
            return false;
        }

        void IValueProvider.SetValue(String val)
        {
            _textBuffer.Replace(0, _textBuffer.Length, val);
        }

        private void TrimAdornments(Span visibleRegion)
        {
            for (Int32 i = _adornmentList.Count - 1; i >= 0; i--)
            {
                IAdornment adornment = _adornmentList[i];
                if (!adornment.Span.Intersects(visibleRegion) && !visibleRegion.Contains(adornment.Span))
                {
                    _adornmentSurfaceManager.RemoveAdornment(adornment);
                    _adornmentList.RemoveAt(i);
                }
            }
        }

        private void TrimLineVisuals(Int32 startVisibleLine, Int32 endVisibleLine)
        {
            Int32 startOfLineFromLineNumber = _textBuffer.GetStartOfLineFromLineNumber(startVisibleLine);
            Int32 position = _textBuffer.GetStartOfLineFromLineNumber(endVisibleLine);
            Int32 startOfNextLineFromPosition = _textBuffer.GetStartOfNextLineFromPosition(position);
            if (position == startOfNextLineFromPosition)
            {
                startOfNextLineFromPosition = _textBuffer.GetEndOfLineFromPosition(position);
            }
            else if (startOfNextLineFromPosition > position)
            {
                startOfNextLineFromPosition--;
            }
            for (Int32 i = _textLineVisuals.Count - 1; i >= 0; i--)
            {
                TextLineVisual lineVisual = _textLineVisuals[i];
                Int32 start = lineVisual.LineSpan.Start;
                if ((start < startOfLineFromLineNumber) || (start > startOfNextLineFromPosition))
                {
                    this.RemoveTextLine(lineVisual);
                }
                else if ((lineVisual.VerticalOffset + lineVisual.Height) < 0)
                {
                    this.RemoveTextLine(lineVisual);
                    startOfLineFromLineNumber = start;
                }
                else if (lineVisual.VerticalOffset > (this.ViewportHeight + 1))
                {
                    this.RemoveTextLine(lineVisual);
                    startOfNextLineFromPosition = start;
                }
            }
            this.TrimAdornments(new Span(startOfLineFromLineNumber, startOfNextLineFromPosition - startOfLineFromLineNumber));
        }

        private void UpdateViewportCoords()
        {
            if (_textLineVisuals.Count != 0)
            {
                TextLineVisual visual = _textLineVisuals[0];
                Int32 lineNumberFromPosition = _textBuffer.GetLineNumberFromPosition(visual.LineSpan.Start);
                Double verticalOffset = visual.VerticalOffset;
                Double num3 = this.TotalContentHeight / ((Double) _textBuffer.LineCount);
                Double num4 = (lineNumberFromPosition * num3) - verticalOffset;
                if (num4 != _viewportVerticalOffset)
                {
                    _viewportVerticalOffset = num4;
                    if (this.ViewportVerticalOffsetChanged != null)
                    {
                        this.ViewportVerticalOffsetChanged(this, new EventArgs());
                    }
                }
                if (_viewportVerticalOffset < 0)
                {
                    _viewportVerticalOffset = 0;
                }
            }
        }

        private IntPtr WndProc(IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            if (msg == 0x100)
            {
                if (((((Int32) wParam) == 0x19) && IsKorean()) && !_selectionLayer.IsEmpty)
                {
                    Char ch = _textBuffer[_selectionLayer.ActiveSpan.Start];
                    if (IsHangul(ch) && EditorHelper.HanjaConversion(this, EditorHelper.GetKeyboardLayout(), _textBuffer[_selectionLayer.ActiveSpan.Start]))
                    {
                        _selectionLayer.ActiveSpan = new TextSpan(_textBuffer, _selectionLayer.ActiveSpan.Start, 1);
                        _selectionLayer.IsActiveSpanReversed = false;
                        handled = true;
                    }
                }
            }
            else if (msg == 0x10f)
            {
                if (!IsKorean())
                {
                    this.PositionImmCompositionWindow();
                }
                else
                {
                    Int32 dwIndex = ((Int32) lParam) & 0x808;
                    if (dwIndex != 0)
                    {
                        IntPtr immContext = EditorHelper.GetImmContext(this);
                        if (immContext != IntPtr.Zero)
                        {
                            String immCompositionString = EditorHelper.GetImmCompositionString(immContext, dwIndex);
                            this.ProcessImmCompositionString(dwIndex, immCompositionString);
                            EditorHelper.ReleaseImmContext(this, immContext);
                        }
                    }
                    handled = true;
                }
            }
            else if (msg == 0x10d)
            {
                _imeEnabled = true;
                if (IsKorean())
                {
                    handled = true;
                }
                _caretElement.Visibility = Visibility.Hidden;
                _caretElement.EnsureVisible();
                this.PositionImmCompositionWindow();
            }
            else if (msg == 270)
            {
                _imeEnabled = false;
                if (IsKorean())
                {
                    this.RaiseTextInputEvent(this, new TextAreaInputEventArgs(TextInputState.Final, ""));
                    handled = true;
                }
                _caretElement.Visibility = Visibility.Visible;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// </summary>
        public new Brush Background
        {
            get
            {
                return _controlHostLayer.Background;
            }
            set
            {
                _controlHostLayer.Background = value;
            }
        }

        /// <summary>
        /// </summary>
        public ITextCaret Caret
        {
            get
            {
                return _caretElement;
            }
        }

        IEnumerable<KeyValuePair<Object, Object>> IPropertyOwner.Properties
        {
            get
            {
                return _properties;
            }
        }

        /// <summary>
        /// </summary>
        public ITextSelection Selection
        {
            get
            {
                return _selectionLayer;
            }
        }

        Boolean IValueProvider.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        String IValueProvider.Value
        {
            get
            {
                return _textBuffer.GetText();
            }
        }

        /// <summary>
        /// </summary>
        public Int32 TabSize
        {
            get
            {
                return _visualsFactory.TabSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _visualsFactory.TabSize = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// </summary>
        public TextBuffer TextBuffer
        {
            get
            {
                return _textBuffer;
            }
        }

        /// <summary>
        /// </summary>
        public IList<ITextLine> TextLines
        {
            get
            {
                return _textLineVisuals.TextLineList;
            }
        }

        /// <summary>
        /// </summary>
        public IEditorArea TextView
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// </summary>
        public Double TotalContentHeight
        {
            get
            {
                if (_textLineVisuals.Count == 0)
                {
                    return 0;
                }
                Double num = 0;
                foreach (TextLineVisual visual in _textLineVisuals)
                {
                    num += visual.Height;
                }
                Double num2 = num / ((Double) _textLineVisuals.Count);
                return (num2 * _textBuffer.LineCount);
            }
        }

        /// <summary>
        /// </summary>
        public Double TotalContentWidth
        {
            get
            {
                return _lineWidthCache.MaxWidth;
            }
        }

        /// <summary>
        /// </summary>
        public Double ViewportHeight
        {
            get
            {
                if (!Double.IsNaN(base.ActualHeight))
                {
                    return base.ActualHeight;
                }
                return 120;
            }
        }

        /// <summary>
        /// </summary>
        public Double ViewportHorizontalOffset
        {
            get
            {
                return _viewportHorizontalOffset;
            }
            set
            {
                Double num = Math.Max(0, Math.Min(value, (this.TotalContentWidth + 10) - this.ViewportWidth));
                if (num != _viewportHorizontalOffset)
                {
                    _viewportHorizontalOffset = num;
                    Canvas.SetLeft(_baseLayer, -_viewportHorizontalOffset);
                    if (this.ViewportHorizontalOffsetChanged != null)
                    {
                        this.ViewportHorizontalOffsetChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public Double ViewportMargin
        {
            get
            {
                return _viewportMargin;
            }
            set
            {
                if (Double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _viewportMargin = value;
                _baseLayer.Margin = new Thickness(value);
            }
        }

        /// <summary>
        /// </summary>
        public Double ViewportVerticalOffset
        {
            get
            {
                if ((this.TotalContentHeight < this.ViewportHeight) && (_viewportVerticalOffset != 0))
                {
                    this.ViewportVerticalOffset = 0;
                }
                return _viewportVerticalOffset;
            }
            set
            {
                if (_viewportVerticalOffset != value)
                {
                    _viewportVerticalOffset = value;
                    if (_viewportVerticalOffset < 0)
                    {
                        _viewportVerticalOffset = 0;
                    }
                    else if ((this.TotalContentHeight > this.ViewportHeight) && (_viewportVerticalOffset > (this.TotalContentHeight - this.ViewportHeight)))
                    {
                        _viewportVerticalOffset = this.TotalContentHeight - this.ViewportHeight;
                    }
                    Double num = this.TotalContentHeight / ((Double) _textBuffer.LineCount);
                    Int32 num2 = (Int32) (_viewportVerticalOffset / num);
                    Double num3 = 0;
                    if ((num2 * num) != _viewportVerticalOffset)
                    {
                        num2++;
                        num3 = (num2 * num) - _viewportVerticalOffset;
                    }
                    if (num2 >= _textBuffer.LineCount)
                    {
                        num2 = _textBuffer.LineCount - 1;
                    }
                    _startLine = num2;
                    _startLinePosition = ViewRelativePosition.Top;
                    _startLinePositionOffset = num3;
                    this.InvalidateView();
                    if (this.ViewportVerticalOffsetChanged != null)
                    {
                        this.ViewportVerticalOffsetChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public Double ViewportWidth
        {
            get
            {
                if (!Double.IsNaN(base.ActualWidth))
                {
                    return base.ActualWidth;
                }
                return 240;
            }
        }

        /// <summary>
        /// </summary>
        public FrameworkElement VisualElement
        {
            get
            {
                return this;
            }
        }

        private class LineWidthCache
        {
            private List<LineWidth> _cachedLines = new List<LineWidth>(50);
            private const Int32 _cacheSize = 50;
            private Double _maxWidth = Double.MaxValue;
            private TextBuffer _textBuffer;

            public LineWidthCache(TextBuffer textBuffer)
            {
                _textBuffer = textBuffer;
            }

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
                    for (Int32 i = 1; i < _cachedLines.Count; i++)
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

            private class LineWidth
            {
                private TextPoint _start;
                private Double _width;

                public LineWidth(TextPoint start, Double width)
                {
                    _start = start;
                    _width = width;
                }

                public TextPoint Start
                {
                    get
                    {
                        return _start;
                    }
                }

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
            }
        }
    }
}

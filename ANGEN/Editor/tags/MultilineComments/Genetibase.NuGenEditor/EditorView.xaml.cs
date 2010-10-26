/* -----------------------------------------------
 * EditorView.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Windows.Controls.Editor;
using Genetibase.Windows.Controls.Editor.InputBinding;
using Genetibase.Windows.Controls.Editor.View;

namespace Genetibase.Windows.Controls
{
    partial class EditorView : ContentControl, ICommandSource, IEditorView
    {
        #region Properties.Public

        /*
         * CanSplit
         */

        /// <summary>
        /// Identifies the <see cref="CanSplit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanSplitProperty;

        /// <summary>
        /// Gets or sets the value indicating whether this element can split its container.
        /// </summary>
        public Boolean CanSplit
        {
            get
            {
                return (Boolean)GetValue(CanSplitProperty);
            }
            set
            {
                SetValue(CanSplitProperty, value.Boxed());
            }
        }

        private static void OnCanSplitChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            EditorView editorView = (EditorView)target;
            editorView.Initiator.InvokeDependencyPropertyChanged(_canSplitChanged, e);
            editorView.OnCanSplitChanged(e);
        }

        private static readonly Object _canSplitChanged = new Object();

        /// <summary>
        /// Occurs when the value of the <see cref="CanSplit"/> property changes.
        /// </summary>
        public event DependencyPropertyChangedEventHandler CanSplitChanged
        {
            add
            {
                Events.AddHandler(_canSplitChanged, value);
            }
            remove
            {
                Events.RemoveHandler(_canSplitChanged, value);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="Genetibase.Windows.Controls.EditorView.CanSplitChanged"/> event is raised
        /// on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnCanSplitChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        /*
         * Command
         */

        /// <summary>
        /// Identifies the <see cref="Command"/> property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty;

        /// <summary>
        /// Gets the command that will be executed when the command source is invoked.
        /// </summary>
        /// <value></value>
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        /*
         * CommandParameter
         */

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty;

        /// <summary>
        /// Represents a user defined data value that can be passed to the command when it is executed.
        /// </summary>
        /// <value></value>
        /// <returns>The command specific data.</returns>
        public Object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        /*
         * CommandTarget
         */

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty;

        /// <summary>
        /// The Object that the command is being executed on.
        /// </summary>
        /// <value></value>
        public IInputElement CommandTarget
        {
            get
            {
                return (IInputElement)GetValue(CommandTargetProperty);
            }
            set
            {
                SetValue(CommandTargetProperty, value);
            }
        }

        /*
         * EditorCommandPrimitives
         */

        /// <summary>
        /// </summary>
        /// <value></value>
        public IEditorCommands EditorCommandPrimitives
        {
            get
            {
                return _editorCommands;
            }
        }

        /*
         * HorizontalScrollBarMargin
         */

        /// <summary>
        /// </summary>
        public Thickness HorizontalScrollBarMargin
        {
            get
            {
                return HorizontalScrollBar.Margin;
            }
            set
            {
                HorizontalScrollBar.Margin = value;
            }
        }

        /*
         * HostControl
         */

        /// <summary>
        /// </summary>
        /// <value></value>
        public Control HostControl
        {
            get { return this; }
        }

        /*
         * IsLineNumberGutterVisible
         */

        /// <summary>
        /// </summary>
        /// <value></value>
        public Boolean IsLineNumberGutterVisible
        {
            get
            {
                return OuterGrid.Children.Contains(_lineNumberProvider);
            }
            set
            {
                if (value != this.IsLineNumberGutterVisible)
                {
                    if (value)
                    {
                        if (_lineNumberProvider == null)
                        {
                            _lineNumberProvider = new DefaultLineNumberProvider(_editorArea, EditorConnector.CreateEditorCommands(_editorArea));
                            _lineNumberProvider.UpdateLineNumbers();
                        }

                        if (CanInsertGutter)
                        {
                            Grid.SetRow(_lineNumberProvider, ContentRowIndex);
                            Grid.SetColumn(_lineNumberProvider, GutterColumnIndex);

                            OuterGrid.Children.Add(_lineNumberProvider);
                        }
                    }
                    else if (this.IsLineNumberGutterVisible)
                    {
                        OuterGrid.Children.Remove(_lineNumberProvider);
                    }
                }
            }
        }

        /*
         * IsReadOnly
         */

        /// <summary>
        /// </summary>
        public Boolean IsReadOnly
        {
            get
            {
                return _editorKeyBinding.IsReadOnly;
            }
            set
            {
                _editorKeyBinding.IsReadOnly = value;
            }
        }

        /*
         * LineNumberGutterBackgroundBrush
         */

        /// <summary>
        /// </summary>
        public Brush LineNumberGutterBackgroundBrush
        {
            get
            {
                return _lineNumberProvider.BackgroundBrush;
            }
            set
            {
                _lineNumberProvider.BackgroundBrush = value;
            }
        }

        /*
         * LineNumberGutterFontSize
         */

        /// <summary>
        /// </summary>
        public Double LineNumberGutterFontSize
        {
            get
            {
                return _lineNumberProvider.FontSize;
            }
            set
            {
                _lineNumberProvider.FontSize = value;
            }
        }

        /*
         * LineNumberGutterForegroundColor
         */

        /// <summary>
        /// </summary>
        public Color LineNumberGutterForegroundColor
        {
            get
            {
                return _lineNumberProvider.ForegroundColor;
            }
            set
            {
                _lineNumberProvider.ForegroundColor = value;
            }
        }

        /*
         * LineNumberGutterTypeface
         */

        /// <summary>
        /// </summary>
        public Typeface LineNumberGutterTypeface
        {
            get
            {
                return _lineNumberProvider.Typeface;
            }
            set
            {
                _lineNumberProvider.Typeface = value;
            }
        }

        /*
         * OverwriteCaretBrush
         */

        /// <summary>
        /// </summary>
        public Brush OverwriteCaretBrush
        {
            get
            {
                CaretElement caret = _editorArea.Caret as CaretElement;
                
                if (caret != null)
                {
                    return caret.OverwriteModeBrush;
                }
                
                return null;
            }
            set
            {
                CaretElement caret = _editorArea.Caret as CaretElement;
                
                if (caret != null)
                {
                    caret.OverwriteModeBrush = value;
                }
            }
        }

        /*
         * TextView
         */

        /// <summary>
        /// </summary>
        /// <value></value>
        public IEditorArea TextView
        {
            get { return _editorArea; }
        }

        /*
         * VerticalScrollBarMargin
         */

        /// <summary>
        /// </summary>
        public Thickness VerticalScrollBarMargin
        {
            get
            {
                return VerticalScrollBar.Margin;
            }
            set
            {
                VerticalScrollBar.Margin = value;
            }
        }

        #endregion

        #region Properties.Protected

        /// <summary>
        /// </summary>
        protected EventHandlerList Events
        {
            get
            {
                return HandlerList.Events;
            }
        }

        #endregion

        #region Properties.Services

        private INuGenEventHandlerListProvider _handlerList;

        /// <summary>
        /// </summary>
        protected virtual INuGenEventHandlerListProvider HandlerList
        {
            get
            {
                if (_handlerList == null)
                {
                    _handlerList = new NuGenEventHandlerListProvider();
                }

                return _handlerList;
            }
        }

        private INuGenEventInitiatorService _initiator;

        /// <summary>
        /// </summary>
        protected virtual INuGenEventInitiatorService Initiator
        {
            get
            {
                if (_initiator == null)
                {
                    _initiator = new NuGenEventInitiatorService(this, Events);
                }

                return _initiator;
            }
        }

        #endregion

        #region Properties.Elements

        /*
         * CanInsertGutter
         */

        private Boolean _canInsertGutter;

        private Boolean CanInsertGutter
        {
            get
            {
                return _canInsertGutter;
            }
        }

        /*
         * CanInsertEditorArea
         */

        private Boolean _canInsertEditorArea;

        private Boolean CanInsertEditorArea
        {
            get
            {
                return _canInsertEditorArea;
            }
        }

        /*
         * ContentColumnIndex
         */

        private Int32 _contentColumnIndex;

        private Int32 ContentColumnIndex
        {
            get
            {
                return _contentColumnIndex;
            }
        }

        /*
         * ContentRowIndex
         */

        private Int32 _contentRowIndex;

        private Int32 ContentRowIndex
        {
            get
            {
                return _contentRowIndex;
            }
        }

        /*
         * GutterColumnIndex
         */

        private Int32 _gutterColumnIndex;

        private Int32 GutterColumnIndex
        {
            get
            {
                return _gutterColumnIndex;
            }
        }

        /*
         * HorizontalScrollBar
         */

        private ScrollBar _horizontalScrollBar;

        private ScrollBar HorizontalScrollBar
        {
            get
            {
                if (_horizontalScrollBar == null)
                {
                    /* Use this new instance as a null object. */
                    _horizontalScrollBar = new ScrollBar();
                }

                return _horizontalScrollBar;
            }
        }

        /*
         * OuterGrid
         */

        private Grid _outerGrid;

        private Grid OuterGrid
        {
            get
            {
                Debug.Assert(_outerGrid != null, "_outerGrid != null");

                if (_outerGrid == null)
                {
                    /* Use this new instance as a null object. */
                    _outerGrid = new Grid();
                }

                return _outerGrid;
            }
        }

        /*
         * SplitButton
         */

        private IInputElement _splitButton;

        private IInputElement SplitButton
        {
            get
            {
                Debug.Assert(_splitButton != null, "_splitButton != null");

                if (_splitButton == null)
                {
                    /* Use this new instance as a null object. */
                    _splitButton = new Thumb();
                }

                return _splitButton;
            }
        }

        /*
         * VerticalScrollBar
         */

        private EditorScrollBar _verticalScrollBar;

        private EditorScrollBar VerticalScrollBar
        {
            get
            {
                Debug.Assert(_verticalScrollBar != null, "_verticalScrollBar != null");

                if (_verticalScrollBar == null)
                {
                    /* Use this new instance as a null object. */
                    _verticalScrollBar = new EditorScrollBar();
                }

                return _verticalScrollBar;
            }
        }

        #endregion

        #region Methods.Public.Overridden

        /// <summary>
        /// Called when an internal process or application calls <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>, which is used to build the current template's visual tree.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _editorArea.LayoutChanged += new EventHandler<TextAreaLayoutChangedEventArgs>(EditorLayoutChanged);
            _editorArea.ViewportHorizontalOffsetChanged += new EventHandler(EditorLayoutChanged);
            _editorArea.ViewportWidthChanged += new EventHandler(EditorLayoutChanged);

            _horizontalScrollBar = Template.FindName("PART_HorizontalScrollBar", this) as ScrollBar;
            _outerGrid = GetTemplateChild("PART_OuterGrid") as Grid;
            _splitButton = GetTemplateChild("PART_SplitButton") as IInputElement;
            _verticalScrollBar = GetTemplateChild("PART_VerticalScrollBar") as EditorScrollBar;

            ColumnDefinition gutterColumn = GetTemplateChild("_gutterColumn") as ColumnDefinition;
            ColumnDefinition contentColumn = GetTemplateChild("_contentColumn") as ColumnDefinition;
            RowDefinition contentRow = GetTemplateChild("_contentRow") as RowDefinition;

            _gutterColumnIndex = OuterGrid.ColumnDefinitions.IndexOf(gutterColumn);
            _contentColumnIndex = OuterGrid.ColumnDefinitions.IndexOf(contentColumn);
            _contentRowIndex = OuterGrid.RowDefinitions.IndexOf(contentRow);

            _canInsertGutter = _gutterColumnIndex > -1 && _contentRowIndex > -1;
            _canInsertEditorArea = _contentColumnIndex > -1 && _contentRowIndex > -1;

            if (_canInsertEditorArea)
            {
                _editorArea.VisualElement.SetValue(Grid.RowProperty, ContentRowIndex);
                _editorArea.VisualElement.SetValue(Grid.ColumnProperty, ContentColumnIndex);

                OuterGrid.Children.Add(_editorArea.VisualElement);
            }

            VerticalScrollBar.LeftShiftClick += new EventHandler<RoutedPropertyChangedEventArgs<Double>>(this.OnVerticalLeftShiftClick);
            VerticalScrollBar.Scroll += new ScrollEventHandler(this.VerticalScrollBarScrolled);
            HorizontalScrollBar.Minimum = VerticalScrollBar.Minimum = 0;
            HorizontalScrollBar.ValueChanged += new RoutedPropertyChangedEventHandler<Double>(this.OnHorizontalValueChanged);

            IsLineNumberGutterVisible = true;

            Dispatcher.BeginInvoke(
                DispatcherPriority.Render
                , new DispatcherOperationCallback(
                    delegate
                    {
                        _editorArea.VisualElement.Focus();
                        return null;
                    }
                 )
                 , null
            );

            SizeChanged += new SizeChangedEventHandler(OnSizeChanged);
        }

        #endregion

        #region Methods.Protected.Overridden

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown"/> GTMT#routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (SplitButton.IsMouseOver)
            {
                CanSplit = false;
                ExecuteCommand(this);
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        #endregion

        #region Methods.Private

        private static void ExecuteCommand(ICommandSource commandSource)
        {
            Debug.Assert(commandSource != null, "commandSource != null");
            ICommand command = commandSource.Command;

            if (command != null)
            {
                Object commandParameter = commandSource.CommandParameter;
                IInputElement commandTarget = commandSource.CommandTarget;
                RoutedCommand routedCommand = command as RoutedCommand;

                if (routedCommand != null)
                {
                    if (commandTarget == null)
                    {
                        commandTarget = commandSource as IInputElement;
                    }

                    if (routedCommand.CanExecute(commandParameter, commandTarget))
                    {
                        routedCommand.Execute(commandParameter, commandTarget);
                    }
                }
                else if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
        }

        private void UpdateEditorLayout()
        {
            IList<ITextLine> textLines = _editorArea.TextLines;
            Int32 num = -1;

            while ((++num < textLines.Count) && (textLines[num].VerticalOffset < 0))
            {
            }

            Int32 count = textLines.Count;
            Double viewportHeight = _editorArea.ViewportHeight;

            while ((--count >= num) && ((textLines[count].VerticalOffset + textLines[count].Height) > viewportHeight))
            {
            }

            Int32 num4 = (1 + count) - num;

            HorizontalScrollBar.Maximum = Math.Max(
                (_editorArea.TotalContentWidth + 10) - _editorArea.ViewportWidth
                , _editorArea.ViewportHorizontalOffset
            );
            VerticalScrollBar.Maximum = _editorArea.TextBuffer.LineCount - num4;
            HorizontalScrollBar.ViewportSize = _editorArea.ViewportWidth;
            VerticalScrollBar.ViewportSize = num4;
            VerticalScrollBar.SetValue(RangeBase.LargeChangeProperty, (Double)num4);
            HorizontalScrollBar.Value = _editorArea.ViewportHorizontalOffset;
            VerticalScrollBar.Value = _editorArea.TextBuffer.GetLineNumberFromPosition(textLines[num].LineSpan.Start);
        }

        #endregion

        #region EventHandlers.EditorView

        private void OnSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            UpdateEditorLayout();
        }

        #endregion

        #region EventHandlers.EditorArea

        private void EditorLayoutChanged(Object sender, EventArgs e)
        {
            UpdateEditorLayout();   
        }

        #endregion

        #region EventHandlers.HorizontalScrollBar

        private void OnHorizontalValueChanged(Object sender, RoutedPropertyChangedEventArgs<Double> e)
        {
            if (!_inHorizontalValueChanged)
            {
                try
                {
                    _inHorizontalValueChanged = true;
                    _editorArea.ScrollViewportHorizontally(e.NewValue - _editorArea.ViewportHorizontalOffset);
                    HorizontalScrollBar.Value = _editorArea.ViewportHorizontalOffset;
                }
                finally
                {
                    _inHorizontalValueChanged = false;
                }
            }
        }

        #endregion

        #region EventHandlers.VerticalScrollBar

        private void OnVerticalLeftShiftClick(Object sender, RoutedPropertyChangedEventArgs<Double> e)
        {
            Int32 lineNumber = Math.Min((Int32)e.NewValue, _editorArea.TextBuffer.LineCount - 1);
            _editorArea.DisplayLine(lineNumber, 0, ViewRelativePosition.Top);
            _editorArea.Caret.CaptureVerticalPosition();
        }

        private void VerticalScrollBarScrolled(Object sender, ScrollEventArgs e)
        {
            switch (e.ScrollEventType)
            {
                case ScrollEventType.LargeDecrement:
                    _editorArea.ScrollViewportVertically(-_editorArea.ViewportHeight);
                    break;
                case ScrollEventType.LargeIncrement:
                    _editorArea.ScrollViewportVertically(_editorArea.ViewportHeight);
                    break;
                case ScrollEventType.SmallDecrement:
                    _editorArea.ScrollViewportVertically(-1);
                    break;
                case ScrollEventType.SmallIncrement:
                    _editorArea.ScrollViewportVertically(1);
                    break;
                default:
                    Int32 lineNumber = Math.Min((Int32)e.NewValue, _editorArea.TextBuffer.LineCount - 1);
                    _editorArea.DisplayLine(lineNumber, 0, ViewRelativePosition.Top);
                    break;
            }

            _editorArea.Caret.CaptureVerticalPosition();
        }

        #endregion

        private IEditorCommands _editorCommands;
        private EditorKeyBinding _editorKeyBinding;
        private EditorMouseBinding _editorMouseBinding;
        private Boolean _inHorizontalValueChanged;
        private DefaultLineNumberProvider _lineNumberProvider;
        private IEditorArea _editorArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorView"/> class.
        /// </summary>
        public EditorView(IEditorArea editorArea)
        {
            if (editorArea == null)
            {
                throw new ArgumentNullException("editorArea");
            }

            _editorArea = editorArea;

            _editorCommands = EditorConnector.CreateEditorCommands(_editorArea);
            _editorKeyBinding = new EditorKeyBinding(this);
            _editorMouseBinding = new EditorMouseBinding(this);

            InitializeComponent();
        }

        static EditorView()
        {
            CanSplitProperty = DependencyProperty.Register(
                "CanSplit"
                , typeof(Boolean)
                , typeof(EditorView)
                , new FrameworkPropertyMetadata(
                    false.Boxed()
                    , FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    , new PropertyChangedCallback(OnCanSplitChanged)
                )
            );

            CommandProperty = DependencyProperty.Register(
                "Command"
                , typeof(ICommand)
                , typeof(EditorView)
                , new FrameworkPropertyMetadata(null)
            );

            CommandParameterProperty = DependencyProperty.Register(
                "CommandParameter"
                , typeof(Object)
                , typeof(EditorView)
                , new FrameworkPropertyMetadata(null)
            );

            CommandTargetProperty = DependencyProperty.Register(
                "CommandTarget"
                , typeof(IInputElement)
                , typeof(EditorView)
                , new FrameworkPropertyMetadata(null)
            );
        }
    }
}

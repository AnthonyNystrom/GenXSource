/* -----------------------------------------------
 * NuGenEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Genetibase.Windows.Controls.Editor;

namespace Genetibase.Windows.Controls
{
    /// <summary>
    /// Source code editor with syntax hightlighting.
    /// </summary>	
    public partial class NuGenEditor : UserControl
    {
        #region Properties.Public

        /*
         * IsSplitted
         */

        /// <summary>
        /// Identifies the <see cref="Genetibase.Windows.Controls.NuGenEditor.IsSplitted"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSplittedProperty;

        private static void OnIsSplittedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NuGenEditor editor = sender as NuGenEditor;
            Debug.Assert(editor != null, "editor != null");
            editor.Initiator.InvokeDependencyPropertyChanged(_isSplittedChanged, e);
            editor.OnIsSplittedChanged(e);

            if (editor.IsSplitted)
            {
                editor.TopEditorRow.MaxHeight = Double.PositiveInfinity;
                editor.TopEditorRow.MinHeight = editor.SplitterHeight;
                editor.Splitter.IsEnabled = true;
                editor.Splitter.CaptureMouse();
            }
            else
            {
                if (editor.TopEditorRow.ActualHeight > editor.SplitterHeight)
                {
                    var maxHeightAnimation = new DoubleAnimation()
                    {
                        AccelerationRatio = 0.9,
                        Duration = new Duration(TimeSpan.FromMilliseconds(editor.TopEditorRow.ActualHeight)),
                        FillBehavior = FillBehavior.Stop,
                        From = editor.TopEditorRow.ActualHeight,
                        To = editor.TopEditorRow.MinHeight
                    };

                    editor.TopEditorRow.BeginAnimation(RowDefinition.MaxHeightProperty, maxHeightAnimation);
                }

                editor.TopEditorRow.MinHeight = 0;
                editor.TopEditorRow.MaxHeight = 0;
                editor.Splitter.IsEnabled = false;
                editor._bottomEditorView.CanSplit = true;
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the control window is splitted.
        /// This is a dependency property.
        /// </summary>
        public Boolean IsSplitted
        {
            get
            {
                return (Boolean)GetValue(IsSplittedProperty);
            }
            set
            {
                SetValue(IsSplittedProperty, value.Boxed());
            }
        }

        private static readonly Object _isSplittedChanged = new Object();

        /// <summary>
        /// Occurs when the value of the <see cref="Genetibase.Windows.Controls.NuGenEditor.IsSplitted"/> property changes.
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsSplittedChanged
        {
            add
            {
                Events.AddHandler(_isSplittedChanged, value);
            }
            remove
            {
                Events.RemoveHandler(_isSplittedChanged, value);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="Genetibase.Windows.Controls.NuGenEditor.IsSplittedChanged"/> event is raised
        /// on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnIsSplittedChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region Properties.Protected

        /// <summary>
        /// Gets the list of handlers for the events defined.
        /// </summary>
        /// <value></value>
        protected EventHandlerList Events
        {
            get
            {
                return this.HandlerListProvider.Events;
            }
        }

        private INuGenEventHandlerListProvider _handlerListProvider;

        /// <summary>
        /// </summary>
        protected virtual INuGenEventHandlerListProvider HandlerListProvider
        {
            get
            {
                if (_handlerListProvider == null)
                {
                    _handlerListProvider = new NuGenEventHandlerListProvider();
                }

                return _handlerListProvider;
            }
        }

        #endregion

        #region Properties.Elements

        /*
         * BottomEditorRow
         */

        private RowDefinition _bottomEditorRow;

        private RowDefinition BottomEditorRow
        {
            get
            {
                Debug.Assert(_bottomEditorRow != null, "_bottomEditorRow != null");

                if (_bottomEditorRow == null)
                {
                    /* Use this new instance as a null object. */
                    _bottomEditorRow = new RowDefinition();
                }

                return _bottomEditorRow;
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
         * Splitter
         */

        private GridSplitter _splitter;

        /// <summary>
        /// Represents the splitter between two <see cref="EditorView"/> panes.
        /// </summary>
        private GridSplitter Splitter
        {
            get
            {
                if (_splitter == null)
                {
                    Debug.Assert(_splitter != null, "_splitter != null");

                    if (_splitter == null)
                    {
                        /* Use this new instance as a null object. */
                        _splitter = new GridSplitter();
                    }
                }

                return _splitter;
            }
        }

        /*
         * SplitterHeight
         */

        private Double? _splitterHeight;

        private Double SplitterHeight
        {
            get
            {
                if (_splitterHeight == null)
                {
                    _splitterHeight = (Double)FindResource("EditorStyle_SplitterHeight");
                }

                return _splitterHeight ?? 5;
            }
        }

        /*
         * TopEditorRow
         */

        private RowDefinition _topEditorRow;

        /// <summary>
        /// This is row is invisible by default.
        /// </summary>
        private RowDefinition TopEditorRow
        {
            get
            {
                if (_topEditorRow == null)
                {
                    Debug.Assert(_topEditorRow != null, "_topEditorRow != null");

                    if (_topEditorRow == null)
                    {
                        /* Use this new instance as null object. */
                        _topEditorRow = new RowDefinition();
                    }
                }

                return _topEditorRow;
            }
        }

        #endregion

        #region Properties.Services

        private INuGenEventInitiatorService _initiator;

        /// <summary>
        /// </summary>
        protected virtual INuGenEventInitiatorService Initiator
        {
            get
            {
                if (_initiator == null)
                {
                    _initiator = new NuGenEventInitiatorService(this, this.Events);
                }

                return _initiator;
            }
        }

        #endregion

        #region Methods.Public.Overridden

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Debug.Assert(_bottomEditorView != null, "_bottomEditorView != null");

            _bottomEditorRow = GetTemplateChild("_bottomEditorRow") as RowDefinition;
            _outerGrid = GetTemplateChild("PART_OuterGrid") as Grid;
            _topEditorRow = GetTemplateChild("_topEditorRow") as RowDefinition;
            _splitter = GetTemplateChild("_splitter") as GridSplitter;

            RowDefinition topEditorRow = GetTemplateChild("_topEditorRow") as RowDefinition;
            RowDefinition bottomEditorRow = GetTemplateChild("_bottomEditorRow") as RowDefinition;

            Int32 topEditorRowIndex = OuterGrid.RowDefinitions.IndexOf(topEditorRow);
            Int32 bottomEditorRowIndex = OuterGrid.RowDefinitions.IndexOf(bottomEditorRow);

            if (topEditorRowIndex > -1)
            {
                _topEditorView.SetValue(Grid.RowProperty, topEditorRowIndex);
                OuterGrid.Children.Add(_topEditorView);
            }

            if (bottomEditorRowIndex > -1)
            {
                _bottomEditorView.SetValue(Grid.RowProperty, bottomEditorRowIndex);
                OuterGrid.Children.Add(_bottomEditorView);
            }

            this.Splitter.PreviewMouseLeftButtonUp += _Splitter_PreviewMouseLeftButtonUp;
            this.Splitter.MouseDoubleClick += _Splitter_MouseDoubleClick;

            Style editorViewStyle = FindResource("EditorViewStyle") as Style;

            _bottomEditorView.Style = editorViewStyle;
            _topEditorView.Style = editorViewStyle;

            _topEditorView.Margin = new Thickness(0, 0, 0, SplitterHeight);
        }

        #endregion

        #region CommandHandlers

        private void SplitContainerOnExecute(Object sender, ExecutedRoutedEventArgs e)
        {
            IsSplitted = true;
        }

        private void SplitContainerCanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsSplitted;
        }

        #endregion

        #region EventHandlers.Splitter

        private void _Splitter_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            IsSplitted = false;
        }

        private void _Splitter_PreviewMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            if (this.TopEditorRow.ActualHeight == (Double)FindResource("EditorStyle_SplitterHeight"))
            {
                IsSplitted = false;
            }
        }

        #endregion

        private EditorView _bottomEditorView;
        private EditorView _topEditorView;
        private TextBuffer _textBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenEditor"/> class.
        /// </summary>
        public NuGenEditor()
        {
            InitializeComponent();

            _textBuffer = new LinkBuffer("text.cs");
            _topEditorView = new EditorView(new EditorArea(_textBuffer));
            _bottomEditorView = new EditorView(new EditorArea(_textBuffer));
            _bottomEditorView.CanSplit = true;
            _bottomEditorView.Command = NuGenEditorCommands.SplitContainer;

            CommandBindings.Add(
                new CommandBinding(
                    NuGenEditorCommands.SplitContainer
                    , new ExecutedRoutedEventHandler(SplitContainerOnExecute)
                    , new CanExecuteRoutedEventHandler(SplitContainerCanExecute)
                )
            );
        }

        static NuGenEditor()
        {
            IsSplittedProperty = DependencyProperty.Register(
                "IsSplitted"
                , typeof(Boolean)
                , typeof(NuGenEditor)
                , new FrameworkPropertyMetadata(
                    false.Boxed()
                    , FrameworkPropertyMetadataOptions.AffectsArrange
                    , new PropertyChangedCallback(OnIsSplittedChanged)
                )
            );
        }
    }
}

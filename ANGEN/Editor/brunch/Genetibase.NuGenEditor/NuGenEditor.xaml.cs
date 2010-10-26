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
                editor.TopEditorRow.MinHeight = (Double)editor.FindResource("EditorStyle_SplitterHeight");
                editor.Splitter.IsEnabled = true;
                editor.Splitter.CaptureMouse();
            }
            else
            {
                
                var maxHeightAnimation = new DoubleAnimation()
                {
                    // Duration = new Duration(TimeSpan.FromSeconds(1))
                    FillBehavior = FillBehavior.Stop
                    , By = 5
                    , From = editor.TopEditorRow.ActualHeight
                    , To = editor.TopEditorRow.MinHeight
                    , AccelerationRatio = 0.9
                };

                editor.TopEditorRow.BeginAnimation(RowDefinition.MaxHeightProperty, maxHeightAnimation);

                editor.TopEditorRow.MinHeight = 0;
                editor.TopEditorRow.MaxHeight = 0;
                editor.Splitter.IsEnabled = false;
                editor.BottomEditorView.CanSplit = true;
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

        private EditorView _bottomEditorView;

        private EditorView BottomEditorView
        {
            get
            {
                if (_bottomEditorView == null)
                {
                    _bottomEditorView = Template.FindName("PART_BottomEditor", this) as EditorView;
                    Debug.Assert(_bottomEditorView != null, "_bottomEditorView != null");

                    if (_bottomEditorView == null)
                    {
                        _bottomEditorView = new EditorView();
                    }
                }

                return _bottomEditorView;
            }
        }

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
                    _splitter = Template.FindName("_splitter", this) as GridSplitter;
                    Debug.Assert(_splitter != null, "_splitter != null");

                    if (_splitter == null)
                    {
                        _splitter = new GridSplitter();
                    }
                }

                return _splitter;
            }
        }

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
                    _topEditorRow = Template.FindName("_topEditorRow", this) as RowDefinition;
                    Debug.Assert(_topEditorRow != null, "_topEditorRow != null");

                    if (_topEditorRow == null)
                    {
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

            EditorView editorView = Template.FindName("PART_BottomEditor", this) as EditorView;

            if (editorView != null)
            {
                this.Splitter.PreviewMouseLeftButtonUp += _Splitter_PreviewMouseLeftButtonUp;
                this.Splitter.MouseDoubleClick += _Splitter_MouseDoubleClick;
            }
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

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenEditor"/> class.
        /// </summary>
        public NuGenEditor()
        {
            InitializeComponent();
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

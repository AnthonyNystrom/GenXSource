/* -----------------------------------------------
 * EditorView.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

namespace Genetibase.Windows.Controls
{
    partial class EditorView : ScrollViewer, ICommandSource
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
        /// The object that the command is being executed on.
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

        private IInputElement _splitButton;

        private IInputElement SplitButton
        {
            get
            {
                if (_splitButton == null)
                {
                    /* Use this new instance as a null object. */
                    _splitButton = new Thumb();
                }

                return _splitButton;
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
            _splitButton = base.GetTemplateChild("PART_SplitButton") as IInputElement;
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

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorView"/> class.
        /// </summary>
        public EditorView()
        {
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

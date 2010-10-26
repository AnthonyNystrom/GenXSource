/* -----------------------------------------------
 * NuGenApplicationCommand.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Represents a command in a <see cref="NuGenCommandManager"/>.
    /// </summary>
    [ToolboxItem(false)]
    [DefaultProperty("ApplicationCommandName")]
    [DesignTimeVisible(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenApplicationCommand : Component
    {
        private string applicationCommandName;
        private string description;
        private bool notifyItems;
        private bool enabled = true;
        private bool visible = true;
        private bool checkState;
        private NuGenUIItemCollection uiItems;
        private NuGenCommandManagerBase switchboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenApplicationCommand"/> class.
        /// </summary>
        public NuGenApplicationCommand()
        {
            uiItems = new NuGenUIItemCollection(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenApplicationCommand"/> class.
        /// </summary>
        /// <param name="container">Associated container.</param>
        public NuGenApplicationCommand(IContainer container)
            : this()
        {
            if (container != null)
            {
                container.Add(this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenApplicationCommand"/> class with the specified name.
        /// </summary>
        /// <param name="applicationCommandName">The name of the command.</param>
        /// <exception cref="ArgumentNullException">A parameter is null.</exception>
        /// <exception cref="ArgumentException">A parameter is empty.</exception>
        public NuGenApplicationCommand(string applicationCommandName)
            : this()
        {
            if (string.IsNullOrEmpty(applicationCommandName))
            {
                throw new ArgumentNullException("applicationCommandName", "parameter applicationCommandName cannot be null");
            }

            this.applicationCommandName = applicationCommandName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenApplicationCommand"/> class with the specified name and execute handler.
        /// </summary>
        /// <param name="applicationCommandName">The name of the command.</param>
        /// <param name="executeHandler">The execute event handler.</param>
        /// <exception cref="ArgumentNullException">A parameter is null.</exception>
        /// <exception cref="ArgumentException">A parameter is empty.</exception>
        public NuGenApplicationCommand(string applicationCommandName, EventHandler<NuGenApplicationCommandEventArgs> executeHandler)
            : this()
        {
            if (string.IsNullOrEmpty(applicationCommandName))
            {
                throw new ArgumentNullException("applicationCommandName", "parameter name cannot be null");
            }
            if (executeHandler == null)
            {
                throw new ArgumentNullException("executeHandler", "parameter executeHandler cannot be null");
            }

            this.applicationCommandName = applicationCommandName;
            this.ApplicationCommandExecute += executeHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenApplicationCommand"/> class with the specified name, execute and update handler.
        /// </summary>
        /// <param name="applicationCommandName">The name of the command.</param>
        /// <param name="executeHandler">The execute event handler.</param>
        /// <param name="updateHandler">The update event handler.</param>
        /// <exception cref="ArgumentNullException">A parameter is null.</exception>
        /// <exception cref="ArgumentException">A parameter is empty.</exception>
        public NuGenApplicationCommand(string applicationCommandName, EventHandler<NuGenApplicationCommandEventArgs> executeHandler, EventHandler<NuGenApplicationCommandEventArgs> updateHandler)
            : this()
        {
            if (string.IsNullOrEmpty(applicationCommandName))
            {
                throw new ArgumentNullException("applicationCommandName", "parameter applicationCommandName cannot be null");
            }
            if (executeHandler == null)
            {
                throw new ArgumentNullException("executeHandler", "parameter executeHandler cannot be null");
            }
            if (updateHandler == null)
            {
                throw new ArgumentNullException("updateHandler", "parameter updateHandler cannot be null");
            }

            this.applicationCommandName = applicationCommandName;
            this.ApplicationCommandExecute += executeHandler;
            this.ApplicationCommandUpdate += updateHandler;
        }

        /// <summary>
        /// Gets or sets the command name.
        /// </summary>
        /// <value>The name of the application command.</value>
        [Browsable(true)]
        [Description("The name of the ApplicationCommand.")]
        public string ApplicationCommandName
        {
            get
            {
                return applicationCommandName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("ApplicationCommandName", "ApplicationCommandName cannot be null or empty");
                }
                applicationCommandName = value;
            }
        }

        /// <summary>
        /// Gets or sets the command description.
        /// </summary>
        /// <value>The description of the application command.</value>
        [Browsable(true)]
        [Category("Design")]
        [Description("ApplicationCommand description")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Gets or sets if items should be notified in an update.
        /// </summary>
        /// <value>True if items should be notified in an update; otherwise, false. The default is false.</value>
        [Browsable(true)]
        [DefaultValue(false)]
        [Description("should update be called for items")]
        public bool NotifyItems
        {
            get
            {
                return notifyItems;
            }
            set
            {
                notifyItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the command enabled state.
        /// </summary>
        /// <value>True if the command is in the enabled state; otherwise, false. The default is true.</value>
        [DefaultValue(true)]
        [Browsable(true)]
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                foreach (object item in uiItems)
                {
                    switchboard.UIItemAdapter.Enable(item, enabled);
                }
            }
        }

        /// <summary>
        /// Gets or sets the command visible state.
        /// </summary>
        /// <value>True if the command is in the visible state; otherwise, false. The default is true.</value>
        [DefaultValue(true)]
        [Browsable(true)]
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                foreach (object item in uiItems)
                {
                    switchboard.UIItemAdapter.Visible(item, visible);
                }
            }
        }

        /// <summary>
        /// Gets or sets the command checked state.
        /// </summary>
        /// <value>True if the command is in the checked state; otherwise, false. The default is false.</value>
        [DefaultValue(false)]
        [Browsable(true)]
        public bool Checked
        {
            get
            {
                return checkState;
            }
            set
            {
                checkState = value;
                foreach (object item in uiItems)
                {
                    switchboard.UIItemAdapter.Check(item, checkState);
                }
            }
        }

        /// <summary>
        /// Gets a collection containing all items in the command.
        /// </summary>
        /// <value>A collection of UIItem objects.</value>
        [Browsable(true)]
        [Category("Collections")]
        [Description("Collection of items")]
        [Editor("Genetibase.Shared.Controls.Design.NuGenUIItemsEditor", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public NuGenUIItemCollection UIItems
        {
            get
            {
                return uiItems;
            }
            set
            {
                uiItems = value;
            }
        }

        /// <summary>
        /// Executes the command with default values.
        /// </summary>
        /// <param name="sender">The source of the command.</param>
        public void Execute(object sender)
        {
            Execute(sender, null);
        }

        /// <summary>
        /// Execute command helper.
        /// </summary>
        /// <param name="sender">The source of the command.</param>
        /// <param name="item">The item causing the command to be executed.</param>
        public void Execute(object sender, object item)
        {
            NuGenApplicationCommandEventArgs e = new NuGenApplicationCommandEventArgs(this, item);
            if (ApplicationCommandExecute != null)
            {
                ApplicationCommandExecute(sender, e);
            }
            else
            {
                switchboard.ExecuteUICommand(sender, this, item);
            }
        }

        /// <summary>
        /// Updates the command state.
        /// </summary>
        /// <param name="sender">The source of the command update.</param>
        public void Update(object sender)
        {
            if (NotifyItems)
            {
                foreach (object uiItem in UIItems)
                {
                    NuGenApplicationCommandEventArgs e = new NuGenApplicationCommandEventArgs(this, uiItem);
                    if (ApplicationCommandUpdate != null)
                    {
                        ApplicationCommandUpdate(sender, e);
                    }
                    else
                    {
                        switchboard.UpdateApplicationCommand(sender, this, uiItem);
                    }
                }
            }
            else
            {
                NuGenApplicationCommandEventArgs e = new NuGenApplicationCommandEventArgs(this, null);
                if (ApplicationCommandUpdate != null)
                {
                    ApplicationCommandUpdate(sender, e);
                }
                else
                {
                    switchboard.UpdateApplicationCommand(sender, this, null);
                }
            }
        }

        /// <summary>
        /// Update item state.
        /// </summary>
        /// <param name="sender">The source of the command.</param>
        /// <param name="item">The item causing the command to be updated.</param>
        public void Update(object sender, object item)
        {
            NuGenApplicationCommandEventArgs e = new NuGenApplicationCommandEventArgs(this, item);
            if (ApplicationCommandUpdate != null)
            {
                ApplicationCommandUpdate(sender, e);
            }
            else
            {
                switchboard.UpdateApplicationCommand(sender, this, item);
            }
        }

        /// <summary>
        /// Occurs when a command shall be executed.
        /// </summary>
        public event EventHandler<NuGenApplicationCommandEventArgs> ApplicationCommandExecute;

        /// <summary>
        /// Occurs when a command shall be updated.
        /// </summary>
        public event EventHandler<NuGenApplicationCommandEventArgs> ApplicationCommandUpdate;

        /// <summary>
        /// Gets or sets the command manager associated with this command.
        /// </summary>
        [Browsable(true)]
        public NuGenCommandManagerBase CommandManager
        {
            set
            {
                switchboard = value;
            }
            get
            {
                return switchboard;
            }
        }
    }
}

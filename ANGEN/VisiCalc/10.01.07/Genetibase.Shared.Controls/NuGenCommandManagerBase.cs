/* -----------------------------------------------
 * NuGenCommandManagerBase.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Handles commands associated with menus, toolbars, status bars and context menus.
    /// </summary>
    /// <example>This sample shows how to use the <see cref="NuGenCommandManager2"/> with support for 
    /// <see cref="ToolStrip"/> based menus and toolbars.
	/// The <see cref="NuGenCommandManager2"/> is added as a Component named "commandManager".
    /// <code>
    /// private void InitializeCommandManager()
    /// {
    ///     // Create and hook up command switchboard with toolstrip support
    ///     NuGenBasicCommandManager2 commandManager = new NuGenBasicCommandManager2();
    ///     commandManager.ApplicationCommandExecute += new EventHandler&lt;NuGenApplicationCommandEventArgs&gt;(commandManager_CommandExecute);
    ///     commandManager.ApplicationCommandUpdate += new EventHandler&lt;NuGenApplicationCommandEventArgs&gt;(commandManager_CommandUpdate);
    ///
    ///     // Create commands
    ///     commandManager.ApplicationCommands.Add(new NuGenApplicationCommand("New"));
    ///     commandManager.ApplicationCommands.Add(new NuGenApplicationCommand("Save"));
    ///     commandManager.ApplicationCommands.Add(new NuGenApplicationCommand("Exit", Command_Exit_CommandExecute));
    ///
    ///     // Connect toolstrip items to commands
    ///     commandManager.ApplicationCommands["New"].UIItems.AddRange(new object[] { newToolStripMenuItem, newToolStripButton });
    ///     commandManager.ApplicationCommands["Save"].UIItems.AddRange(new object[] { saveToolStripMenuItem, saveToolStripButton });
    ///     commandManager.ApplicationCommands["Exit"].UIItems.Add(exitToolStripMenuItem);
    ///
    ///     // Register context menus
    ///     commandManager.ContextMenus.Add(contextMenuStrip1);
    /// }
    /// 
    /// void Command_Exit_CommandExecute(object sender, ApplicationCommandEventArgs e)
    /// {
    ///     Application.Exit();
    /// }
    ///
    /// void commandManager_CommandUpdate(object sender, ApplicationCommandEventArgs e)
    /// {
    ///     Control owner = sender as Control;
    ///     NuGenApplicationCommand command = e.ApplicationCommand;
    ///     ToolStripItem item = e.Item as ToolStripItem;
    ///     // TODO: Update command
    /// }
    /// 
    /// void commandManager_CommandExecute(object sender, ApplicationCommandEventArgs e)
    /// {
    ///     Control owner = sender as Control;
    ///     NuGenApplicationCommand command = e.ApplicationCommand;
    ///     ToolStripItem item = e.Item as ToolStripItem;
    ///     // TODOD: Execute command
    /// }
    /// </code>
    /// </example>
    [ToolboxItem(false)]
	[Designer("Genetibase.Shared.Controls.Design.NuGenCommandManagerDesigner")]
    [DefaultProperty("ApplicationCommands")]
    [ProvideProperty("ApplicationCommand", typeof(object))]
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract partial class NuGenCommandManagerBase : Component, IExtenderProvider
    {
		#region IExtenderProvider Members

		/// <summary>
		/// Specifies whether this object can provide its extender properties to the specified object. 
		/// </summary>
		/// <param name="extendee">The object to receive the extender properties.</param>
		/// <returns>true if this object can provide extender properties to the specified object; otherwise, false.</returns>
		public bool CanExtend(object extendee)
		{
			if (uiItemAdapter.IsUIItemSupported(extendee))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// </summary>
		/// <param name="item">The item to be associated with the command.</param>
		/// <param name="applicationCommand">The command to be associated with the item.</param>
		public void SetApplicationCommand(object item, NuGenApplicationCommand applicationCommand)
		{
			if (applicationCommand != null)
			{
				applicationCommand.CommandManager = this;
				applicationCommand.UIItems.Add(item);
			}
			else
			{
				NuGenApplicationCommand oldUICommand = GetApplicationCommandByItem(item);
				if (oldUICommand != null)
				{
					oldUICommand.UIItems.Remove(item);
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="item">The item to get the associated command.</param>
		/// <returns>The associated command</returns>
		public NuGenApplicationCommand GetApplicationCommand(object item)
		{
			return GetApplicationCommandByItem(item);
		}

		#endregion

		/// <summary>
		/// Occurs when a command shall be executed.
		/// </summary>
		public event EventHandler<NuGenApplicationCommandEventArgs> ApplicationCommandExecute;

		/// <summary>
		/// Occurs when a command shall be updated.
		/// </summary>
		public event EventHandler<NuGenApplicationCommandEventArgs> ApplicationCommandUpdate;

		/// <summary>
		/// Gets or sets the command adapter.
		/// </summary>
		/// <value>The command adapter used.</value>
		[Browsable(false)]
		public INuGenUIItemAdapter UIItemAdapter
		{
			get
			{
				return uiItemAdapter;
			}
			set
			{
				uiItemAdapter = value;
			}
		}

		/// <summary>
		/// Gets a collection containing all commands in the switchboard.
		/// </summary>
		/// <value>A strongly typed collection of ApplicationCommand objects.</value>
		[Browsable(true)]
		[Category("Collections")]
		[Description("Collection of commands")]
		[Editor("Genetibase.Shared.Controls.Design.NuGenApplicationCommandCollectionEditor", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public NuGenApplicationCommandCollection ApplicationCommands
		{
			get
			{
				return applicationCommands;
			}
			set
			{
				applicationCommands = value;
			}
		}

		/// <summary>
		/// Gets a collection containing all context menus in the switchboard.
		/// </summary>
		/// <value>A collection of context menu objects.</value>
		[Browsable(true)]
		[Category("Collections")]
		[Description("Collection of context menus")]
		[Editor("Genetibase.Shared.Controls.Design.NuGenContextMenusEditor", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public NuGenContextMenuCollection ContextMenus
		{
			get
			{
				return contextMenus;
			}
			set
			{
				contextMenus = value;
			}
		}

        private NuGenApplicationCommandCollection applicationCommands;
        private INuGenUIItemAdapter uiItemAdapter;
        private NuGenContextMenuCollection contextMenus;
        private Dictionary<object, NuGenApplicationCommand> applicationCommandByItemList = new Dictionary<object, NuGenApplicationCommand>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenCommandManagerBase"/> class. 
        /// </summary>
        public NuGenCommandManagerBase()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenCommandManagerBase"/> class. 
        /// </summary>
        /// <param name="container">Component container</param>
        public NuGenCommandManagerBase(IContainer container)
        {
            if (container != null)
            {
                container.Add(this);
            }

            applicationCommands = new NuGenApplicationCommandCollection(this);
            contextMenus = new NuGenContextMenuCollection(this);

            // Add handler for idle status update of ApplicationCommands
            Application.Idle += new EventHandler(Application_Idle);
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					Application.Idle -= new EventHandler(Application_Idle);
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

        /// <summary>
        /// Application idle loop for updating command state.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains no event data.</param>
        void Application_Idle(object sender, EventArgs e)
        {
            foreach (NuGenApplicationCommand command in ApplicationCommands)
            {
                command.Update(null);
            }
        }

        /// <summary>
        /// Get the command associated with the specified item.
        /// </summary>
        /// <param name="item">Item to get associated command for.</param>
        /// <returns>The associated command.</returns>
		public NuGenApplicationCommand GetApplicationCommandByItem(object item)
		{
			foreach (NuGenApplicationCommand applicationCommand in ApplicationCommands)
			{
				if (applicationCommand.UIItems.Contains(item))
				{
					return applicationCommand;
				}
			}

			return null;
		}

		/// <summary>
		/// Execute the specified command.
		/// </summary>
		/// <param name="sender">The source of the execute.</param>
		/// <param name="applicationCommand">The command to execute</param>
		/// <param name="item">The item that caused the command to be executed (can be null).</param>
		internal void ExecuteUICommand(object sender, NuGenApplicationCommand applicationCommand, object item)
		{
			NuGenApplicationCommandEventArgs e = new NuGenApplicationCommandEventArgs(applicationCommand, item);
			if (ApplicationCommandExecute != null)
			{
				ApplicationCommandExecute(sender, e);
			}
		}

		/// <summary>
		/// Updates the specified command.
		/// </summary>
		/// <param name="sender">The source of the update.</param>
		/// <param name="applicationCommand">The command to update</param>
		/// <param name="item">The item that should be updated (can be null).</param>
		internal void UpdateApplicationCommand(object sender, NuGenApplicationCommand applicationCommand, object item)
		{
			NuGenApplicationCommandEventArgs e = new NuGenApplicationCommandEventArgs(applicationCommand, item);
			if (ApplicationCommandUpdate != null)
			{
				ApplicationCommandUpdate(sender, e);
			}
		}
    }
}

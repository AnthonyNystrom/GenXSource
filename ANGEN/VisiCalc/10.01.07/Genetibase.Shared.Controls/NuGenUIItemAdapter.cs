/* -----------------------------------------------
 * NuGenUIItemAdapter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Provides support for Menu based menus, ToolBar based toolbars and ContextMenu based context menus.
    /// </summary>
    /// <remarks>
    /// The following items is supported:
    /// <list type="">
    /// <item>MenuItem</item>
    /// <item>ToolBarButton</item>
    /// <item>ContextMenu</item>
    /// </list>
    /// </remarks>
    public class NuGenUIItemAdapter : INuGenUIItemAdapter
    {
        private NuGenCommandManagerBase _commandManager;
        private Control _owner;

        /// <summary>
		/// Initializes a new instance of the <see ref="NuGenUIItemAdapter"/> class.
        /// Hidden to prevent construction without a command manager.
        /// </summary>
        private NuGenUIItemAdapter()
        {
        }

        /// <summary>
		/// Initializes a new instance of the <see ref="NuGenUIItemAdapter"/> class with the associated command switchboard.
        /// </summary>
        /// <param name="commandManager">The associated command manager.</param>
        public NuGenUIItemAdapter(NuGenCommandManagerBase commandManager)
        {
            _commandManager = commandManager;
        }

        /// <summary>
        /// Gets the command switchboard parent for this adapter.
        /// </summary>
        public NuGenCommandManagerBase CommandManager
        {
            get
            {
                return _commandManager;
            }
        }

        /// <summary>
        /// Called when an item is added to the <see cref="NuGenUIItemCollection"/> collection.
        /// </summary>
        /// <remarks>
        /// The adapter should normally add an event handler to the Click event of this item.
        /// </remarks>
        /// <param name="applicationCommand">The command that has an item added.</param>
        /// <param name="item">The item that has been added.</param>
        public virtual void UIItemAdded(NuGenApplicationCommand applicationCommand, object item)
        {
            MenuItem menuItem = item as MenuItem;
            if (menuItem != null)
            {
                menuItem.Click += new EventHandler(menuItem_Click);
                return;
            }

            ToolBarButton toolBarButtonItem = item as ToolBarButton;
            if (toolBarButtonItem != null)
            {
                toolBarButtonItem.Parent.ButtonClick -= new ToolBarButtonClickEventHandler(Parent_ButtonClick);
                toolBarButtonItem.Parent.ButtonClick += new ToolBarButtonClickEventHandler(Parent_ButtonClick);
                return;
            }
        }

        /// <summary>
        /// Called when an item is removed from the <see cref="NuGenUIItemCollection"/> collection.
        /// </summary>
        /// <param name="item">The item that has been removed.</param>
        public virtual void UIItemRemoved(object item)
        {
            MenuItem menuItem = item as MenuItem;
            if (menuItem != null)
            {
                menuItem.Click -= new EventHandler(menuItem_Click);
                return;
            }

            ToolBarButton toolBarButtonItem = item as ToolBarButton;
            if (toolBarButtonItem != null)
            {
                toolBarButtonItem.Parent.ButtonClick += new ToolBarButtonClickEventHandler(Parent_ButtonClick);
                return;
            }
        }

        /// <summary>
        /// Called when an item is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="EventArgs"/> that contains no data.</param>
        void menuItem_Click(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            NuGenApplicationCommand applicationCommand = _commandManager.GetApplicationCommandByItem(item);
            if (applicationCommand != null)
            {
                Control ownerControl = GetOwnerControl(sender);
                _owner = null;
                applicationCommand.Execute(ownerControl, item);
            }
        }

        /// <summary>
        /// Called when an item is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="ToolBarButtonClickEventArgs"/> that contains event data.</param>
        void Parent_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            ToolBarButton item = e.Button;
            NuGenApplicationCommand applicationCommand = _commandManager.GetApplicationCommandByItem(item);
            if (applicationCommand != null)
            {
                Control ownerControl = GetOwnerControl(sender);
                _owner = null;
                applicationCommand.Execute(ownerControl, item);
            }
        }

        /// <summary>
        /// Called when a context menu is added to the <see cref="NuGenContextMenuCollection"/> collection.
        /// </summary>
        /// <param name="contextMenu">The context menu that has been added.</param>
        public virtual void ContextMenuAdded(object contextMenu)
        {
            ContextMenu contextMenuObject = contextMenu as ContextMenu;
            contextMenuObject.Popup += new EventHandler(contextMenuObject_Popup);
        }

        /// <summary>
        /// Called when a context menu is removed from the <see cref="NuGenContextMenuCollection"/> collection.
        /// </summary>
        /// <param name="contextMenu">The contyext menu that has been removed.</param>
        public virtual void ContextMenuRemoved(object contextMenu)
        {
            ContextMenu contextMenuObject = contextMenu as ContextMenu;
            contextMenuObject.Popup -= new EventHandler(contextMenuObject_Popup);
        }

        /// <summary>
        /// Called when a context menu is opening.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="EventArgs"/> that contains no data.</param>
        void contextMenuObject_Popup(object sender, EventArgs e)
        {
            ContextMenu contextMenu = sender as ContextMenu;
            _owner = contextMenu.SourceControl;
            foreach (MenuItem item in contextMenu.MenuItems)
            {
                UpdateItem(item);
            }
        }

        /// <summary>
        /// Updates the state of a tool strip item.
        /// </summary>
        /// <param name="item">The item to update.</param>
        void UpdateItem(MenuItem item)
        {
            NuGenApplicationCommand applicationCommand = _commandManager.GetApplicationCommandByItem(item);
            if (applicationCommand != null)
            {
                Control ownerControl = GetOwnerControl(item);
                applicationCommand.Update(ownerControl, item);

                foreach (MenuItem childItem in item.MenuItems)
                {
                    UpdateItem(childItem);
                }
            }
        }

        /// <summary>
        /// Get the owner control of the item.
        /// </summary>
        /// <param name="item">The item to get the owner control of.</param>
        /// <returns>The owner control of the item.</returns>
        public virtual Control GetOwnerControl(object item)
        {
            MenuItem menuItem = item as MenuItem;
            if (menuItem != null)
            {
                return _owner;
            }
            return null;
        }

        /// <summary>
        /// Enable or disable a item.
        /// </summary>
        /// <param name="item">The item to enable or disable</param>
        /// <param name="value">True if item should be enabled, else false.</param>
        public virtual void Enable(object item, bool value)
        {
            MenuItem menuItem = item as MenuItem;
            if (menuItem != null)
            {
                menuItem.Enabled = value;
                return;
            }

            ToolBarButton toolBarButtonItem = item as ToolBarButton;
            if (toolBarButtonItem != null)
            {
                toolBarButtonItem.Enabled = value;
                return;
            }
        }

        /// <summary>
        /// Show or hide a item.
        /// </summary>
        /// <param name="item">The item to show or hide</param>
        /// <param name="value">True if item should be visible, else false.</param>
        public virtual void Visible(object item, bool value)
        {
            MenuItem menuItem = item as MenuItem;
            if (menuItem != null)
            {
                menuItem.Visible = value;
                return;
            }

            ToolBarButton toolBarButtonItem = item as ToolBarButton;
            if (toolBarButtonItem != null)
            {
                toolBarButtonItem.Visible = value;
                return;
            }
        }

        /// <summary>
        /// Check or uncheck a item.
        /// </summary>
        /// <param name="item">The item to check or uncheck.</param>
        /// <param name="value">True if item should be checked, else false.</param>
        public virtual void Check(object item, bool value)
        {
            MenuItem menuItem = item as MenuItem;
            if (menuItem != null)
            {
                menuItem.Checked = value;
            }
        }

        /// <summary>
        /// Check if the adapter supports the specified item.
        /// </summary>
        /// <param name="item">The item to check if is supported.</param>
        /// <returns>true if the item is supported, else false.</returns>
        public virtual bool IsUIItemSupported(object item)
        {
            // Inheritance hierarchy of Menu and Toolbar items
            // - MenuItem
            // - ToolBarButton
            if (item != null)
            {
                // Check for known items
                string typeName = item.GetType().ToString();
                switch (typeName)
                {
                    case "System.Windows.Forms.MenuItem": return true;
                    case "System.Windows.Forms.ToolBarButton": return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get supported items in the container.
        /// </summary>
        /// <returns>The collection of supported items.</returns>
        public virtual object[] GetAvailableUIItems()
        {
            List<object> availableItems = new List<object>();
            foreach (Component component in _commandManager.Container.Components)
            {
                if (IsUIItemSupported(component))
                {
                    availableItems.Add(component);
                }
            }
            object[] itemsArray = new object[availableItems.Count];
            availableItems.CopyTo(itemsArray);
            return itemsArray;
        }

        /// <summary>
        /// Check if the adapter supports the specified context menu.
        /// </summary>
        /// <param name="contextMenu">The context menu to check if is supported.</param>
        /// <returns>true if the context menu is supported, else false.</returns>
        public virtual bool IsContextMenuSupported(object contextMenu)
        {
			return contextMenu is ContextMenu;
        }

        /// <summary>
        /// Get supported context menus in the container.
        /// </summary>
        /// <returns>The collection of supported context menus.</returns>
        public virtual object[] GetAvailableContextMenus()
        {
            List<object> availableItems = new List<object>();
            foreach (Component component in _commandManager.Container.Components)
            {
                if (IsContextMenuSupported(component))
                {
                    availableItems.Add(component);
                }
            }
            object[] itemsArray = new object[availableItems.Count];
            availableItems.CopyTo(itemsArray);
            return itemsArray;
        }

        /// <summary>
        /// Get the name of the item.
        /// </summary>
        public virtual string GetUIItemName(object item)
        {
            if (item != null)
            {
				MenuItem menuItem = item as MenuItem;

				if (menuItem != null)
				{
					return menuItem.Name;
				}

				ToolBarButton toolbarButton = item as ToolBarButton;

				if (toolbarButton != null)
				{
					return toolbarButton.Name;
				}
            }

            return null;
        }

        /// <summary>
        /// Get the name of the context menu.
        /// </summary>
        public virtual string GetContextMenuName(object contextMenu)
        {
            ContextMenu item = contextMenu as ContextMenu;

            if (item != null)
            {
                return item.Name;
            }

            return null;
        }
    }
}

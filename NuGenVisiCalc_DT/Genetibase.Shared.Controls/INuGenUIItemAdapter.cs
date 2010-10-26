/* -----------------------------------------------
 * INuGenUIItemAdapter.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Supports specific menu, toolbar and context menu support for <see cref="NuGenCommandManager"/>.
    /// </summary>
    public interface INuGenUIItemAdapter
    {
        /// <summary>
        /// Called when an item is added to the <see cref="NuGenUIItemCollection"/> collection.
        /// </summary>
        /// <remarks>
        /// The adapter should normally add an event handler to the Click event of this item.
        /// </remarks>
        /// <param name="applicationCommand">The command that has an item added.</param>
        /// <param name="item">The item that has been added.</param>
        void UIItemAdded(NuGenApplicationCommand applicationCommand, object item);

        /// <summary>
        /// Called when an item is removed from the <see cref="NuGenUIItemCollection"/> collection.
        /// </summary>
        /// <param name="item">The item that has been removed.</param>
        void UIItemRemoved(object item);

        /// <summary>
        /// Called when a context menu is added to the <see cref="NuGenContextMenuCollection"/> collection.
        /// </summary>
        /// <param name="contextMenu">The context menu that has been added.</param>
        void ContextMenuAdded(object contextMenu);

        /// <summary>
        /// Called when a context menu is removed from the <see cref="NuGenContextMenuCollection"/> collection.
        /// </summary>
        /// <param name="contextMenu">The contyext menu that has been removed.</param>
        void ContextMenuRemoved(object contextMenu);

        /// <summary>
        /// Enable or disable a item.
        /// </summary>
        /// <param name="item">The item to enable or disable</param>
        /// <param name="value">True if item should be enabled, else false.</param>
        void Enable(object item, bool value);

        /// <summary>
        /// Show or hide a item.
        /// </summary>
        /// <param name="item">The item to show or hide</param>
        /// <param name="value">True if item should be visible, else false.</param>
        void Visible(object item, bool value);

        /// <summary>
        /// Check or uncheck a item.
        /// </summary>
        /// <param name="item">The item to check or uncheck.</param>
        /// <param name="value">True if item should be checked, else false.</param>
        void Check(object item, bool value);

        /// <summary>
        /// Get the owner control of the item.
        /// </summary>
        /// <param name="item">The item to get the owner control of.</param>
        /// <returns>The owner control of the item.</returns>
        Control GetOwnerControl(object item);

        /// <summary>
        /// Check if the adapter supports the specified item.
        /// </summary>
        /// <param name="item">The item to check if is supported.</param>
        /// <returns>true if the item is supported, else false.</returns>
        bool IsUIItemSupported(object item);

        /// <summary>
        /// Get supported items in the container.
        /// </summary>
        /// <returns>The collection of supported items.</returns>
        object[] GetAvailableUIItems();

        /// <summary>
        /// Check if the adapter supports the specified context menu.
        /// </summary>
        /// <param name="contextMenu">The context menu to check if is supported.</param>
        /// <returns>true if the context menu is supported, else false.</returns>
        bool IsContextMenuSupported(object contextMenu);

        /// <summary>
        /// Get supported context menus in the container.
        /// </summary>
        /// <returns>The collection of supported context menus.</returns>
        object[] GetAvailableContextMenus();

        /// <summary>
        /// Get the name of the item.
        /// </summary>
        /// <param name="item">Item to get name from</param>
        /// <returns>The item name.</returns>
        string GetUIItemName(object item);

        /// <summary>
        /// Get the name of the context menu.
        /// </summary>
        /// <param name="contextMenu">Context menu to get name from</param>
        /// <returns>The context menu name.</returns>
        string GetContextMenuName(object contextMenu);
    }
}

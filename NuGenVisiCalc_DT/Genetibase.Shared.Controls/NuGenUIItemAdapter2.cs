/* -----------------------------------------------
 * NuGenUIItemAdapter2.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides support for ToolStrip based menus, toolbars and context menus.
	/// </summary>
	/// <remarks>
	/// The following ToolStrip items is supported:
	/// <list type="">
	/// <item>ToolStripItem</item>
	/// <item>ToolStripButton</item>
	/// <item>ToolStripLabel</item>
	/// <item>ToolStripComboBox</item>
	/// <item>ToolStripTextBox</item>
	/// <item>ToolStripProgressBar</item>
	/// <item>ToolStripDropDownItem</item>
	/// <item>ToolStripMenuItem</item>
	/// <item>ToolStripSplitButton</item>
	/// <item>ToolStripDropDownButton</item>
	/// <item>ToolStripStatusLabel</item>
	/// <item>ContextMenuStrip</item>
	/// </list>
	/// User defined types inheriting from ToolStripControlHost must
	/// be supported by the user through extension of this adapter.
	/// </remarks>
	public class NuGenUIItemAdapter2 : INuGenUIItemAdapter
	{
		private NuGenCommandManagerBase _commandManager;
		private Control cachedOwnerControl;

		private bool inEnabledChanged;
		private bool inVisibleChanged;
		private bool inCheckedChanged;

		/// <summary>
		/// Initializes a new instance of the <see ref="NuGenUIItemAdapter"/> class.
		/// Hidden to prevent construction without a command manager.
		/// </summary>
		private NuGenUIItemAdapter2()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenUIItemAdapter"/> class with the associated command switchboard.
		/// </summary>
		/// <param name="commandManager">The associated command switchboard.</param>
		public NuGenUIItemAdapter2(NuGenCommandManagerBase commandManager)
		{
			_commandManager = commandManager;
		}

		/// <summary>
		/// Gets the <see cref="NuGenCommandManager"/> parent for this adapter.
		/// </summary>
		/// <value>The element's switchboard owner.</value>
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
		/// <param name="applicationCommand">The <see cref="NuGenApplicationCommand"/> that has an item added.</param>
		/// <param name="item">The item that has been added.</param>
		public virtual void UIItemAdded(NuGenApplicationCommand applicationCommand, object item)
		{
			// Add common handlers
			ToolStripItem toolStripItem = item as ToolStripItem;
			if (toolStripItem != null)
			{
				toolStripItem.EnabledChanged += new EventHandler(toolStripItem_EnabledChanged);
				toolStripItem.VisibleChanged += new EventHandler(toolStripItem_VisibleChanged);
			}

			// Add check handler
			ToolStripMenuItem toolStripMenuItem = item as ToolStripMenuItem;
			if (toolStripMenuItem != null)
			{
				toolStripMenuItem.CheckedChanged += new EventHandler(toolStripItem_CheckedChanged);
			}
			ToolStripButton toolStripButton = item as ToolStripButton;
			if (toolStripButton != null)
			{
				toolStripButton.CheckedChanged += new EventHandler(toolStripItem_CheckedChanged);
			}

			// Add specific combobox click handler
			ToolStripComboBox toolStripCombobox = item as ToolStripComboBox;
			if (toolStripCombobox != null)
			{
				toolStripCombobox.SelectedIndexChanged += new EventHandler(toolStripItem_Click);
				return;
			}

			// Add specific textbox click handler
			ToolStripTextBox toolStripTextbox = item as ToolStripTextBox;
			if (toolStripTextbox != null)
			{
				toolStripTextbox.TextChanged += new EventHandler(toolStripItem_Click);
				return;
			}

			// Add generic click handler
			if (toolStripItem != null)
			{
				toolStripItem.Click += new EventHandler(toolStripItem_Click);
			}
		}

		/// <summary>
		/// Called when an item is removed from the <see cref="NuGenUIItemCollection"/>.
		/// </summary>
		/// <param name="item">The item that has been removed.</param>
		public virtual void UIItemRemoved(object item)
		{
			// Remove common handlers
			ToolStripItem toolStripItem = item as ToolStripItem;
			if (toolStripItem != null)
			{
				toolStripItem.EnabledChanged -= new EventHandler(toolStripItem_EnabledChanged);
				toolStripItem.VisibleChanged -= new EventHandler(toolStripItem_VisibleChanged);
			}

			// Remove check handler
			ToolStripMenuItem toolStripMenuItem = item as ToolStripMenuItem;
			if (toolStripMenuItem != null)
			{
				toolStripMenuItem.CheckedChanged -= new EventHandler(toolStripItem_CheckedChanged);
			}
			ToolStripButton toolStripButton = item as ToolStripButton;
			if (toolStripButton != null)
			{
				toolStripButton.CheckedChanged -= new EventHandler(toolStripItem_CheckedChanged);
			}

			// Remove specific combobox click handler
			ToolStripComboBox toolStripCombobox = item as ToolStripComboBox;
			if (toolStripCombobox != null)
			{
				toolStripCombobox.SelectedIndexChanged -= new EventHandler(toolStripItem_Click);
				return;
			}

			// Remove specific textbox click handler
			ToolStripTextBox toolStripTextbox = item as ToolStripTextBox;
			if (toolStripTextbox != null)
			{
				toolStripTextbox.TextChanged -= new EventHandler(toolStripItem_Click);
				return;
			}

			// Remove generic click handler
			if (toolStripItem != null)
			{
				toolStripItem.Click -= new EventHandler(toolStripItem_Click);
			}
		}

		/// <summary>
		/// Called when an item's enabled state has changed.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="EventArgs"/> that contains no event data.</param>
		void toolStripItem_EnabledChanged(object sender, EventArgs e)
		{
			if (inEnabledChanged)
			{
				return;
			}
			try
			{
				inEnabledChanged = true;
				NuGenApplicationCommand applicationCommand = _commandManager.GetApplicationCommandByItem(sender);
				if (applicationCommand != null)
				{
					ToolStripItem item = sender as ToolStripItem;
					applicationCommand.Enabled = item.Enabled;
				}
			}
			finally
			{
				inEnabledChanged = false;
			}
		}

		/// <summary>
		/// Called when an item's visible state has changed.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="EventArgs"/> that contains no event data.</param>
		void toolStripItem_VisibleChanged(object sender, EventArgs e)
		{
			if (inVisibleChanged)
			{
				return;
			}
			try
			{
				inVisibleChanged = true;
				NuGenApplicationCommand applicationCommand = _commandManager.GetApplicationCommandByItem(sender);
				if (applicationCommand != null)
				{
					ToolStripItem item = sender as ToolStripItem;
					applicationCommand.Visible = item.Visible;
				}
			}
			finally
			{
				inVisibleChanged = false;
			}
		}

		/// <summary>
		/// Called when an item's checked state has changed.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="EventArgs"/> that contains no event data.</param>
		void toolStripItem_CheckedChanged(object sender, EventArgs e)
		{
			if (inCheckedChanged)
			{
				return;
			}
			try
			{
				inCheckedChanged = true;

				bool value = false;
				ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
				if (toolStripMenuItem != null)
				{
					value = toolStripMenuItem.Checked;
				}
				ToolStripButton toolStripButton = sender as ToolStripButton;
				if (toolStripButton != null)
				{
					value = toolStripButton.Checked;
				}

				NuGenApplicationCommand applicationCommand = _commandManager.GetApplicationCommandByItem(sender);
				if (applicationCommand != null)
				{
					applicationCommand.Visible = value;
				}
			}
			finally
			{
				inCheckedChanged = false;
			}
		}

		/// <summary>
		/// Called when an item is clicked.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A <see cref="EventArgs"/> that contains no data.</param>
		void toolStripItem_Click(object sender, EventArgs e)
		{
			ToolStripItem item = sender as ToolStripItem;
			NuGenApplicationCommand applicationCommand = _commandManager.GetApplicationCommandByItem(item);
			if (applicationCommand != null)
			{
				Control ownerControl = GetOwnerControl(sender);
				if ((ownerControl == null) && (cachedOwnerControl != null))
				{
					ownerControl = cachedOwnerControl;
				}
				cachedOwnerControl = null;

				applicationCommand.Execute(ownerControl, item);
			}
		}

		/// <summary>
		/// Called when a context menu is added to the <see cref="NuGenContextMenuCollection"/>.
		/// </summary>
		/// <param name="contextMenu">The context menu that has been added.</param>
		public virtual void ContextMenuAdded(object contextMenu)
		{
			ContextMenuStrip contextMenuStrip = contextMenu as ContextMenuStrip;
			contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(contextMenuStrip_Opening);
		}

		/// <summary>
		/// Called when a context menu is removed from the <see cref="NuGenContextMenuCollection"/>.
		/// </summary>
		/// <param name="contextMenu">The contyext menu that has been removed.</param>
		public virtual void ContextMenuRemoved(object contextMenu)
		{
			ContextMenuStrip contextMenuStrip = contextMenu as ContextMenuStrip;
			contextMenuStrip.Opening -= new System.ComponentModel.CancelEventHandler(contextMenuStrip_Opening);
		}

		/// <summary>
		/// Called when a context menu is opening.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A <see cref="System.ComponentModel.CancelEventArgs"/> that contains event data.</param>
		void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ContextMenuStrip contextMenuStrip = sender as ContextMenuStrip;
			cachedOwnerControl = contextMenuStrip.SourceControl;
			foreach (ToolStripItem item in contextMenuStrip.Items)
			{
				UpdateItem(item);
			}
			cachedOwnerControl = null;
		}

		/// <summary>
		/// Updates the state of a tool strip item.
		/// </summary>
		/// <param name="item">The item to update.</param>
		void UpdateItem(ToolStripItem item)
		{
			NuGenApplicationCommand applicationCommand = _commandManager.GetApplicationCommandByItem(item);
			if (applicationCommand != null)
			{
				Control ownerControl = GetOwnerControl(item);
				applicationCommand.Update(ownerControl, item);

				ToolStripDropDownItem dropDownItem = item as ToolStripDropDownItem;
				if (dropDownItem != null)
				{
					foreach (ToolStripItem childItem in dropDownItem.DropDownItems)
					{
						UpdateItem(childItem);
					}
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
			ToolStripItem toolStripItem = item as ToolStripItem;

			// Check if got a ToolBar item
			if (toolStripItem == null)
			{
				return null;
			}

			// Get top level item
			while (toolStripItem.OwnerItem != null)
			{
				toolStripItem = toolStripItem.OwnerItem as ToolStripItem;
			}

			// Check for context menu item
			ContextMenuStrip contextMenuStrip = toolStripItem.Owner as ContextMenuStrip;
			if (contextMenuStrip != null)
			{
				Control ownerControl = contextMenuStrip.SourceControl;
				return ownerControl;
			}
			else
			{
				// This is a form menubar, toolbar or status bar item
				return null;
			}
		}

		/// <summary>
		/// Enable or disable a item.
		/// </summary>
		/// <param name="item">The item to enable or disable</param>
		/// <param name="value">True if item should be enabled, else false.</param>
		public virtual void Enable(object item, bool value)
		{
			// Only need base class because Enabled is defined there
			ToolStripItem toolStripItem = item as ToolStripItem;
			if (toolStripItem != null)
			{
				toolStripItem.Enabled = value;
			}
		}

		/// <summary>
		/// Show or hide a item.
		/// </summary>
		/// <param name="item">The item to show or hide</param>
		/// <param name="value">True if item should be visible, else false.</param>
		public virtual void Visible(object item, bool value)
		{
			// Only need base class because Visible is defined there
			ToolStripItem toolStripItem = item as ToolStripItem;
			if (toolStripItem != null)
			{
				// Only change if different
				if (toolStripItem.Visible != value)
				{
					toolStripItem.Visible = value;

					// Need to refresh toolstrip
					if (toolStripItem.Owner != null)
					{
						toolStripItem.Owner.PerformLayout();
					}
				}
			}
		}

		/// <summary>
		/// Check or uncheck a item.
		/// </summary>
		/// <param name="item">The item to check or uncheck.</param>
		/// <param name="value">True if item should be checked, else false.</param>
		public virtual void Check(object item, bool value)
		{
			// Se if it is a menu item
			ToolStripMenuItem toolStripMenuItem = item as ToolStripMenuItem;
			if (toolStripMenuItem != null)
			{
				toolStripMenuItem.Checked = value;
				return;
			}

			// See if it is a button
			ToolStripButton toolStripButton = item as ToolStripButton;
			if (toolStripButton != null)
			{
				toolStripButton.Checked = value;
				return;
			}
		}

		/// <summary>
		/// Check if the adapter supports the specified item.
		/// </summary>
		/// <param name="item">The item to check if is supported.</param>
		/// <returns>true if the item is supported, else false.</returns>
		public virtual bool IsUIItemSupported(object item)
		{
			// Inheritance hierarchy of ToolStrip items
			// - ToolStripItem
			//   |- ToolStripButton
			//   |- ToolStripLabel
			//   |- ToolStripControlHost
			//   |  |- ToolStripComboBox
			//   |  |- ToolStripTextBox
			//   |  |- ToolStripProgressBar
			//   |- ToolStripDropDownItem
			//   |  |- ToolStripMenuItem
			//   |  |- ToolStripSplitButton
			//   |  |- ToolStripDropDownButton
			//   |- ToolStripStatusLabel

			if (item != null)
			{
				if (
					item is ToolStripButton
					|| item is ToolStripLabel
					|| item is ToolStripComboBox
					|| item is ToolStripTextBox
					|| item is ToolStripProgressBar
					|| item is ToolStripDropDownItem
					|| item is ToolStripMenuItem
					|| item is ToolStripSplitButton
					|| item is ToolStripDropDownButton
					|| item is ToolStripStatusLabel
					)
				{
					return true;
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
			return contextMenu is ContextMenuStrip;
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
		/// <param name="item">Item to get name from</param>
		/// <returns>The item name.</returns>
		public virtual string GetUIItemName(object item)
		{
			ToolStripItem baseItem = item as ToolStripItem;
			if (baseItem != null)
			{
				return baseItem.Name;
			}
			return null;
		}

		/// <summary>
		/// Get the name of the context menu.
		/// </summary>
		/// <param name="contextMenu">Context menu to get name from</param>
		/// <returns>The context menu name.</returns>
		public virtual string GetContextMenuName(object contextMenu)
		{
			ContextMenuStrip item = contextMenu as ContextMenuStrip;
			if (item != null)
			{
				return item.Name;
			}
			return null;
		}
	}
}

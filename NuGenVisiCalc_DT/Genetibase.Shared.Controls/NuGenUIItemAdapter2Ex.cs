/* -----------------------------------------------
 * NuGenUIItemAdpter2Ex.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Provides support for <see cref="NuGenToolStripTrackBar"/> item in addition to the items supported by
	/// <see cref="NuGenUIItemAdapter2"/>.
    /// </summary>
    public class NuGenUIItemAdapter2Ex : NuGenUIItemAdapter2
    {
        private bool inEnabledChanged;
        private bool inVisibleChanged;

        /// <summary>
		/// Initializes a new instance of the <see cref="NuGenUIItemAdapter2Ex"/> class.
        /// </summary>
        public NuGenUIItemAdapter2Ex(NuGenCommandManagerBase commandManager)
            : base(commandManager)
        {
        }

        /// <summary>
        /// Called when an item is added to the <see cref="NuGenUIItemCollection"/> collection.
        /// </summary>
        /// <remarks>
        /// The adapter should normally add an event handler to the Click event of this item.
        /// </remarks>
        /// <param name="command">The command that has an item added.</param>
        /// <param name="item">The item that has been added.</param>
        public override void UIItemAdded(NuGenApplicationCommand command, object item)
        {
            NuGenToolStripTrackBar toolStripTrackBarItem = item as NuGenToolStripTrackBar;
            if (toolStripTrackBarItem != null)
            {
                toolStripTrackBarItem.EnabledChanged += new EventHandler(toolStripItem_EnabledChanged);
                toolStripTrackBarItem.VisibleChanged += new EventHandler(toolStripItem_VisibleChanged);
                toolStripTrackBarItem.ValueChanged += new EventHandler(toolStripTrackBarItem_ValueChanged);
            }
            else
            {
                base.UIItemAdded(command, item);
            }
        }

        /// <summary>
        /// Called when an item is removed from the <see cref="NuGenUIItemCollection"/> collection.
        /// </summary>
        /// <param name="item">The item that has been removed.</param>
        public override void UIItemRemoved(object item)
        {
            NuGenToolStripTrackBar toolStripTrackBarItem = item as NuGenToolStripTrackBar;
            if (toolStripTrackBarItem != null)
            {
                toolStripTrackBarItem.EnabledChanged -= new EventHandler(toolStripItem_EnabledChanged);
                toolStripTrackBarItem.VisibleChanged -= new EventHandler(toolStripItem_VisibleChanged);
                toolStripTrackBarItem.ValueChanged -= new EventHandler(toolStripTrackBarItem_ValueChanged);
            }
            else
            {
                base.UIItemRemoved(item);
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
                NuGenApplicationCommand applicationCommand = CommandManager.GetApplicationCommandByItem(sender);
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
                NuGenApplicationCommand applicationCommand = CommandManager.GetApplicationCommandByItem(sender);
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
        /// Called when the trackbar value changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="EventArgs"/> that contains no data.</param>
        void toolStripTrackBarItem_ValueChanged(object sender, EventArgs e)
        {
            NuGenToolStripTrackBar item = sender as NuGenToolStripTrackBar;
            NuGenApplicationCommand applicationCommand = CommandManager.GetApplicationCommandByItem(item);
            if (applicationCommand != null)
            {
                Control ownerControl = GetOwnerControl(sender);
                applicationCommand.Execute(ownerControl, item);
            }
        }

        /// <summary>
        /// Check if the adapter supports the specified item.
        /// </summary>
        /// <param name="item">The item to check if is supported.</param>
        /// <returns>true if the item is supported, else false.</returns>
        public override bool IsUIItemSupported(object item)
        {
            if (item != null)
            {
				if (item is NuGenToolStripTrackBar)
				{
					return true;
				}
            }
            return base.IsUIItemSupported(item);
        }
    }
}

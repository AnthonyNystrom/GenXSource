/* -----------------------------------------------
 * NuGenContextMenuCollection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Represents the collection of context menus in a <see cref="NuGenCommandManager"/>.
    /// </summary>
    public sealed class NuGenContextMenuCollection : Collection<object>
    {
        private NuGenCommandManagerBase _commandManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenContextMenuCollection"/> class. 
        /// </summary>
        /// <param name="commandManager">The <see cref="NuGenCommandManager"/> that owns the collection.</param>
        public NuGenContextMenuCollection(NuGenCommandManagerBase commandManager)
        {
            _commandManager = commandManager;
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="NuGenContextMenuCollection"/>.  
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="NuGenContextMenuCollection"/>.</param>
        public new void Add(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Parameter item cannot be null");
            }

            if ((_commandManager.UIItemAdapter != null) &&
                _commandManager.UIItemAdapter.IsContextMenuSupported(item))
            {
                base.Add(item);
                _commandManager.UIItemAdapter.ContextMenuAdded(item);
            }
            else
            {
                ArgumentException e = new ArgumentException("The UI Item Adapter doesn't support the item type '" + item.GetType().ToString() + "'", "item");
                throw e;
            }
        }

        /// <summary>
        /// Removes all elements from the <see cref="NuGenContextMenuCollection"/>.
        /// </summary>
        public new void Clear()
        {
            foreach (object item in this)
            {
                _commandManager.UIItemAdapter.ContextMenuRemoved(item);
            }
        }

        /// <summary>
        /// Inserts an element into the <see cref="NuGenContextMenuCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        public new void Insert(int index, object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Parameter item cannot be null");
            }

            if ((_commandManager.UIItemAdapter != null) &&
                _commandManager.UIItemAdapter.IsContextMenuSupported(item))
            {
                base.Insert(index, item);
                _commandManager.UIItemAdapter.ContextMenuAdded(item);
            }
            else
            {
                ArgumentException e = new ArgumentException("The UI Item Adapter doesn't support the item type '" + item.GetType().ToString() + "'", "item");
                throw e;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="NuGenContextMenuCollection"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="NuGenContextMenuCollection"/>.</param>
        /// <returns>true if item is successfully removed; otherwise, false.
        /// This method also returns false if item was not found in the original <see cref="NuGenContextMenuCollection"/>.</returns>
        public new bool Remove(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Parameter item cannot be null");
            }

            bool result = base.Remove(item);
            if (result)
            {
                _commandManager.UIItemAdapter.ContextMenuRemoved(item);
            }
            return result;
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="NuGenContextMenuCollection"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public new void RemoveAt(int index)
        {
            object item = this[index];
            base.RemoveAt(index);
            _commandManager.UIItemAdapter.ContextMenuRemoved(item);
        }

        /// <summary>
        /// Gets the command switchboard containing the <see cref="NuGenContextMenuCollection"/>.
        /// </summary>
        /// <value>The element's switchboard owner.</value>
        public NuGenCommandManagerBase UISwitchboard
        {
            get
            {
                return _commandManager;
            }
        }
    }
}

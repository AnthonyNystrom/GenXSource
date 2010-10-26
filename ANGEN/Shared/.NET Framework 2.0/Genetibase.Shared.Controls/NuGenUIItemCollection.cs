/* -----------------------------------------------
 * NuGenUIItemCollection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Represents the collection of items in a <see cref="NuGenApplicationCommand"/>.
    /// </summary>
    public sealed class NuGenUIItemCollection : Collection<object>
    {
        private NuGenApplicationCommand applicationCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenUIItemCollection"/> class.
        /// </summary>
        /// <param name="applicationCommand">The <see cref="ApplicationCommand"/> that owns the collection.</param>
        internal NuGenUIItemCollection(NuGenApplicationCommand applicationCommand)
        {
            this.applicationCommand = applicationCommand;
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="NuGenUIItemCollection"/>. 
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="NuGenUIItemCollection"/>.</param>
        public new void Add(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "The item parameter cannot be null");
            }

            // Todo: check why this happend in design mode
            if (applicationCommand.CommandManager == null)
            {
                return;
            }
            if ((applicationCommand.CommandManager.UIItemAdapter != null) && 
                applicationCommand.CommandManager.UIItemAdapter.IsUIItemSupported(item))
            {
                // Handle dulicate additions by ignoring them, designer can add the item more than once.
                if (!Contains(item))
                {
                    base.Add(item);
                    applicationCommand.CommandManager.UIItemAdapter.UIItemAdded(applicationCommand, item);
                }
            }
            else
            {
                throw new ArgumentException("The UI Item Adapter doesn't support the item type '" + item.GetType().ToString() + "'", "item");
            }
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="NuGenUIItemCollection"/>. 
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the <see cref="NuGenUIItemCollection"/>.</param>
        public void AddRange(IEnumerable<object> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection", "The collection parameter cannot be null");
            }

            foreach (object item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Removes all elements from the <see cref="NuGenUIItemCollection"/>.
        /// </summary>
        public new void Clear()
        {
            foreach (object item in this)
            {
                applicationCommand.CommandManager.UIItemAdapter.UIItemRemoved(item);
            }
            base.Clear();
        }

        /// <summary>
        /// Inserts an element into the <see cref="NuGenUIItemCollection"/> at the specified index. 
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        public new void Insert(int index, object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "The item parameter cannot be null");
            }

            if ((applicationCommand.CommandManager.UIItemAdapter != null) &&
                applicationCommand.CommandManager.UIItemAdapter.IsUIItemSupported(item))
            {
                // Handle dulicate additions by ignoring them, designer can add the item more than once.
                if (!Contains(item))
                {
                    base.Insert(index, item);
                    applicationCommand.CommandManager.UIItemAdapter.UIItemAdded(applicationCommand, item);
                }
            }
            else
            {
                throw new ArgumentException("The UI Item Adapter doesn't support the item type '" + item.GetType().ToString() + "'", "item");
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="NuGenUIItemCollection"/>. 
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="NuGenUIItemCollection"/>.</param>
        /// <returns>true if item is successfully removed; otherwise, false.
        /// This method also returns false if item was not found in the original <see cref="NuGenUIItemCollection"/>.</returns>
        public new bool Remove(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "The item parameter cannot be null");
            }

            bool result = base.Remove(item);
            if (result)
            {
                applicationCommand.CommandManager.UIItemAdapter.UIItemRemoved(item);
            }
            return result;
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="NuGenUIItemCollection"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public new void RemoveAt(int index)
        {
            object item = this[index];
            base.RemoveAt(index);
            applicationCommand.CommandManager.UIItemAdapter.UIItemRemoved(item);
        }

        /// <summary>
        /// Removes a range of elements from the List.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public void RemoveRange(int index, int count)
        {
            for (int counter = 0; counter < count; counter++)
            {
                RemoveAt(index);
            }
        }

        /// <summary>
        /// Gets the command containing the collection.
        /// </summary>
        /// <value>The collection's command owner.</value>
        public NuGenApplicationCommand ApplicationCommand
        {
            get
            {
                return applicationCommand;
            }
        }
    }
}

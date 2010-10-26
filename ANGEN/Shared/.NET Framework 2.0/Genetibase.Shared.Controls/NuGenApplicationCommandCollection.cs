/* -----------------------------------------------
 * NuGenApplicationCommandCollection.cs
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
    /// Represents the collection of commands in a <see cref="NuGenCommandManager"/>.
    /// </summary>
    public sealed class NuGenApplicationCommandCollection : KeyedCollection<string, NuGenApplicationCommand>
    {
        private NuGenCommandManagerBase _commandManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenApplicationCommandCollection"/> class.
        /// </summary>
        /// <param name="commandManager">The <see cref="NuGenCommandManager"/> that owns the collection.</param>
        internal NuGenApplicationCommandCollection(NuGenCommandManagerBase commandManager)
        {
           _commandManager = commandManager;
        }

        /// <summary>
        /// Adds an object to the end of the Collection.
        /// </summary>
        /// <param name="item">The object to be added to the end of the Collection.</param>
        public new void Add(NuGenApplicationCommand item)
        {
            if (item != null)
            {
                if (!Contains(item))
                {
                    item.CommandManager = _commandManager;
                    base.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds the objects of the specified collection to the end of the Collection.
        /// </summary>
        /// <param name="items">The collection whose objects should be added to the end of the Collection.</param>
        public void AddRange(NuGenApplicationCommand[] items)
        {
            if (items != null)
            {
                foreach (NuGenApplicationCommand item in items)
                {
                    if (item != null)
                    {
                        if (!Contains(item))
                        {
                            item.CommandManager = _commandManager;
                            base.Add(item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Inserts an element into the Collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        public new void Insert(int index, NuGenApplicationCommand item)
        {
            if (item != null)
            {
                if (!Contains(item))
                {
                    item.CommandManager = _commandManager;
                    base.Insert(index, item);
                }
            }
        }

        /// <summary>
        /// Extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element with the key.</param>
        /// <returns>The key for the specified element</returns>
        protected override string GetKeyForItem(NuGenApplicationCommand item)
        {
            return item.ApplicationCommandName;
        }

        /// <summary>
        /// Gets the command switchboard containing the collection.
        /// </summary>
        public NuGenCommandManagerBase CommandManager
        {
            get
            {
                return _commandManager;
            }
        }
    }
}

/* ------------------------------------------------
 * ListEventArgs.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Next2Friends.CrossPoster.Client.Logic
{
    sealed class ListEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Creates a new instance of the <code>ListEventArgs</code> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>item</code> is <code>null</code>.
        /// </exception>
        public ListEventArgs(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            Item = item;
        }

        public T Item { get; private set; }
    }
}

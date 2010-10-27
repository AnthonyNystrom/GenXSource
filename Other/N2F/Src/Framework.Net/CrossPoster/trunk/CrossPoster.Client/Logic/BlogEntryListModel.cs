/* ------------------------------------------------
 * BlogEntryListModel.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Next2Friends.CrossPoster.Client.Logic
{
    sealed class BlogEntryListModel : ListModel<BlogEntryDescriptor>
    {
        /// <summary>
        /// Creates a new instance of the <code>BlogEntryListModel</code> class.
        /// </summary>
        public BlogEntryListModel()
        {
        }
    }
}

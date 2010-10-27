/* ------------------------------------------------
 * BlogEntryDescriptor.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Next2Friends.CrossPoster.Client.Logic
{
    sealed class BlogEntryDescriptor
    {
        /// <summary>
        /// Creates a new instance of the <code>BlogEntryDescriptor</code> class.
        /// </summary>
        public BlogEntryDescriptor()
        {
        }

        public String Content { get; set; }
        public String Sender { get; set; }
        public String Subject { get; set; }
        public DateTime Date { get; set; }
    }
}

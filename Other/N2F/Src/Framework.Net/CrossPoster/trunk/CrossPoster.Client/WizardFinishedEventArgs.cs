/* ------------------------------------------------
 * WizardFinishedEventArgs.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using Next2Friends.CrossPoster.Client.Logic;

namespace Next2Friends.CrossPoster.Client
{
    sealed class WizardFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of the <code>WizardFinishedEventArgs</code> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>blogDescriptor</code> is <code>null</code>.
        /// </exception>
        public WizardFinishedEventArgs(Boolean cancelled, BlogDescriptor blogDescriptor)
        {
            if (blogDescriptor == null)
                throw new ArgumentNullException("blogDescriptor");
            Cancelled = cancelled;
            BlogDescriptor = blogDescriptor;
        }

        public Boolean Cancelled { get; private set; }
        public BlogDescriptor BlogDescriptor { get; private set; }
    }
}

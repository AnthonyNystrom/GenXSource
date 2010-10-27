/* ------------------------------------------------
 * IBlogEngine.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using Next2Friends.CrossPoster.Client.Logic;

namespace Next2Friends.CrossPoster.Client.Engines
{
    interface IBlogEngine
    {
        IList<BlogEntryDescriptor> GetBlogEntries(BlogDescriptor blogDescriptor);
        void PublishNewEntry(BlogDescriptor blogDescriptor, String title, String content);
    }
}

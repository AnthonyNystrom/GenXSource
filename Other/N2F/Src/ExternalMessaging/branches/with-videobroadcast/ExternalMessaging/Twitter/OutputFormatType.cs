/* ------------------------------------------------
 * OutputFormatType.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalMessaging.Twitter
{
    /// <summary>
    /// The output formats supported by Twitter. Not all of them can be used with all of the functions.
    /// For more information about the output formats and the supported functions Check the 
    /// Twitter documentation at: http://groups.google.com/group/twitter-development-talk/web/api-documentation.
    /// </summary>
    internal enum OutputFormatType
    {
        JSON,
        XML,
        RSS,
        Atom
    }
}

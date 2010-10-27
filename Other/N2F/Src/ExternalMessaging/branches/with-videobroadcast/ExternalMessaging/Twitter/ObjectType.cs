/* ------------------------------------------------
 * ObjectType.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalMessaging.Twitter
{
    /// <summary>
    /// The various object types supported at Twitter.
    /// </summary>
    internal enum ObjectType
    {
        Statuses,
        Account,
        Users
    }
}

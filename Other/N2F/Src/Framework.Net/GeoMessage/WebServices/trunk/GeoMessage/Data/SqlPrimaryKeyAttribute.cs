/* ------------------------------------------------
 * SqlPrimaryKeyAttribute.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;

namespace Next2Friends.GeoMessage.Data
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class SqlPrimaryKeyAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the <c>SqlPrimaryKeyAttribute</c> class.
        /// </summary>
        public SqlPrimaryKeyAttribute()
        {
        }
    }
}

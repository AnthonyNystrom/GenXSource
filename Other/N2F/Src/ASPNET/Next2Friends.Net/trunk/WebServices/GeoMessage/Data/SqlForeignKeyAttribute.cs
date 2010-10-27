/* ------------------------------------------------
 * SqlForeignKeyAttribute.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;

namespace Next2Friends.WebServices.GeoMessage.Data
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class SqlForeignKeyAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the <c>SqlForeignKeyAttribute</c> class.
        /// </summary>
        public SqlForeignKeyAttribute()
        {
        }
    }
}

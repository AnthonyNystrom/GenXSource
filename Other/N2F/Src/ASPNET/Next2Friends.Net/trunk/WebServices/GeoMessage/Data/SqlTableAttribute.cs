/* ------------------------------------------------
 * SqlTableAttribute.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data;

namespace Next2Friends.WebServices.GeoMessage.Data
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class SqlTableAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the <c>SqlTableAttribute</c> class.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException">If the specified <c>name</c> is <c>null</c>.</exception>
        public SqlTableAttribute(String name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            Name = name;
        }

        public String Name { get; private set; }
    }
}

/* ------------------------------------------------
 * SqlParameterAttribute.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data;

namespace Next2Friends.WebServices.GeoMessage.Data
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class SqlParameterAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the <c>SqlParameterAttribute</c> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException">If the specified <c>name</c> is <c>null</c>.</exception>
        public SqlParameterAttribute(String name, SqlDbType type)
            : this(name, type, 0)
        {
        }

        /// <summary>
        /// Creates a new instance of the <c>SqlParameterAttribute</c> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="size"></param>
        /// <exception cref="ArgumentNullException">If the specified <c>name</c> is <c>null</c>.</exception>
        public SqlParameterAttribute(String name, SqlDbType type, Int32 size)
            : this(name, type, size, false)
        {
        }

        /// <summary>
        /// Creates a new instance of the <c>SqlParameterAttribute</c> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="isNullable"></param>
        /// <exception cref="ArgumentNullException">If the specified <c>name</c> is <c>null</c>.</exception>
        public SqlParameterAttribute(String name, SqlDbType type, Boolean isNullable)
            : this(name, type, 0, isNullable)
        {
        }

        /// <summary>
        /// Creates a new instance of the <c>SqlParameterAttribute</c> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="size">Default value is <c>0</c>.</param>
        /// <param name="isNullable">Default value is <c>false</c>.</param>
        /// <exception cref="ArgumentNullException">If the specified <c>name</c> is <c>null</c>.</exception>
        public SqlParameterAttribute(String name, SqlDbType type, Int32 size, Boolean isNullable)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            
            Name = name;
            Type = type;
            Size = size;
            IsNullable = isNullable;
        }

        public String Name { get; private set; }
        public SqlDbType Type { get; private set; }
        public Int32 Size { get; private set; }
        public Boolean IsNullable { get; private set; }
    }
}

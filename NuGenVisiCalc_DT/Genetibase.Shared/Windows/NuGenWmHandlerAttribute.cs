/* -----------------------------------------------
 * NuGenWmHandlerAttribute.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Indicates that this method matches <see cref="NuGenWmHandler"/> delegate. Since
	/// AllowMultiple=true you can map a single <see cref="NuGenWmHandler"/> to several Windows messages.
	/// Inherited=false to prevent the situation when the inheritor will have to handle different messages
	/// with the overriden method.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
	public sealed class NuGenWmHandlerAttribute : Attribute
	{
		private int _wmId = 0;

		/// <summary>
		/// Read-only.
		/// </summary>
		public int WmId
		{
			get
			{
				return _wmId;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWmHandlerAttribute"/> class.
		/// </summary>
		public NuGenWmHandlerAttribute(int wmId)
		{
			_wmId = wmId;
		}
	}
}

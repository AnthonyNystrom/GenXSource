/* -----------------------------------------------
 * NuGenHotKeyOperation.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Encapsulates hot key operation associated data.
	/// </summary>
	public class NuGenHotKeyOperation
	{
		private NuGenHotKeyOperationHandler _handler;

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="value"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenHotKeyOperationHandler Handler
		{
			get
			{
				return _handler;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				_handler = value;
			}
		}

		private Keys _hotKeys;

		/// <summary>
		/// </summary>
		public Keys HotKeys
		{
			get
			{
				return _hotKeys;
			}
			set
			{
				_hotKeys = value;
			}
		}

		private string _name;

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="value"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="value"/> is an empty string.
		/// </para>
		/// </exception>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}

				_name = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHotKeyOperation"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="name"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="name"/> is an empty string.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="handler"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenHotKeyOperation(
			string name
			, NuGenHotKeyOperationHandler handler
			, Keys hotKeys
			)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}

			_name = name;
			_handler = handler;
			_hotKeys = hotKeys;
		}
	}
}

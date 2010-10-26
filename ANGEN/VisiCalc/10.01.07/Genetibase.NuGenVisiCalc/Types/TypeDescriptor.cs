/* -----------------------------------------------
 * TypeDescriptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Shared.Reflection;

namespace Genetibase.NuGenVisiCalc.Types
{
	internal sealed class TypeDescriptor : IDescriptor
	{
		private String _typeName;

		public String TypeName
		{
			get
			{
				return _typeName;
			}
		}

		private System.Type _type;

		public System.Type Type
		{
			get
			{
				return _type;
			}
		}

		public NodeBase CreateNode(Canvas canvas)
		{
			return (NodeBase)NuGenActivator.CreateObject(Type);
		}

		public override String ToString()
		{
			return TypeName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeDescriptor"/> class.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="typeName"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="type"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="typeName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="typeName"/> is an empty string.</para>
		/// </exception>
		public TypeDescriptor(System.Type type, String typeName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}

			_type = type;
			_typeName = typeName;
		}
	}
}

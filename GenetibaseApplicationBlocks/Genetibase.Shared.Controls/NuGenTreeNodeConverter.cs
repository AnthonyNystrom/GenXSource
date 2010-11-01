/* -----------------------------------------------
 * NuGenTreeNodeConverter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public sealed class NuGenTreeNodeConverter : TypeConverter
	{
		#region Methods.Public.Overridden

		/*
		 * CanConvertTo
		 */

		/// <summary>
		/// Returns whether this converter can convert the object to the specified type, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="destinationType">A <see cref="T:System.Type"></see> that represents the type you want to convert to.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
		/// </returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}

			return base.CanConvertTo(context, destinationType);
		}

		/*
		 * ConvertTo
		 */

		/// <summary>
		/// Converts the given value object to the specified type, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.</param>
		/// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
		/// <param name="destinationType">The <see cref="T:System.Type"></see> to convert the value parameter to.</param>
		/// <returns>
		/// An <see cref="T:System.Object"></see> that represents the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		/// <exception cref="T:System.ArgumentNullException">The destinationType parameter is null. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}

			if (destinationType == typeof(InstanceDescriptor) && value is NuGenTreeNode)
			{
				NuGenTreeNode node = (NuGenTreeNode)value;
				MemberInfo info = null;
				object[] parameters = null;

				if (node.ImageIndex == -1 || node.ExpandedImageIndex == -1)
				{
					if (node.Nodes.Count == 0)
					{
						info = typeof(NuGenTreeNode).GetConstructor(new Type[] { typeof(string) });
						parameters = new object[] { node.Text };
					}
					else
					{
						info = typeof(NuGenTreeNode).GetConstructor(new Type[] { typeof(string), typeof(NuGenTreeNode[]) });
						NuGenTreeNode[] nodes = new NuGenTreeNode[node.Nodes.Count];
						node.Nodes.CopyTo(nodes, 0);
						parameters = new object[] { node.Text, nodes };
					}
				}
				else if (node.Nodes.Count == 0)
				{
					info = typeof(NuGenTreeNode).GetConstructor(new Type[] { typeof(string), typeof(int), typeof(int) });
					parameters = new object[] { node.Text, node.ImageIndex, node.ExpandedImageIndex };
				}
				else
				{
					info = typeof(NuGenTreeNode).GetConstructor(new Type[] { typeof(string), typeof(int), typeof(int), typeof(NuGenTreeNode[]) });
					NuGenTreeNode[] nodes = new NuGenTreeNode[node.Nodes.Count];
					node.Nodes.CopyTo(nodes, 0);
					parameters = new object[] { node.Text, node.ImageIndex, node.ExpandedImageIndex, nodes };
				}

				if (info != null)
				{
					return new InstanceDescriptor(info, parameters, false);
				}
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeNodeConverter"/> class.
		/// </summary>
		public NuGenTreeNodeConverter()
		{

		}

		#endregion
	}
}

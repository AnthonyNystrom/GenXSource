/* -----------------------------------------------
 * NuGenFrameworkElementFactoryExtensions.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Provides extension methods for <see cref="FrameworkElementFactory"/> instances.
	/// </summary>
	public static class NuGenFrameworkElementFactoryExtensions
	{
		/// <summary>
		/// Appends a collection of <see cref="FrameworkElementFactory"/> instances.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="children"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="parent"/> is <see langword="null"/>.
		/// </para>
		/// <para>
		///		<paramref name="children"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public static void AppendMany(this FrameworkElementFactory parent, FrameworkElementFactory[] children)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}

			if (children == null)
			{
				throw new ArgumentNullException("children");
			}

			foreach (var child in children)
			{
				parent.AppendChild(child);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="target"></param>
		/// <param name="source"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="parent"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void SetTemplateBinding(this FrameworkElementFactory parent, DependencyProperty target, DependencyProperty source)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}

			parent.SetValue(target, new TemplateBindingExtension(source));
		}
	}
}

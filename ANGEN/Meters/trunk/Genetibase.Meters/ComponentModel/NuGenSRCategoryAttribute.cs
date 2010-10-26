/* -----------------------------------------------
 * NuGenSRCategoryAttribute.cs
 * Copyright � 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Genetibase.Meters.ComponentModel
{
	/// <summary>
	/// Provides resource strings support for <see cref="T:System.ComponentModel.CategoryAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Property | AttributeTargets.Method)]
	internal sealed class NuGenSRCategoryAttribute : CategoryAttribute
	{
		/// <summary>
		/// Looks up the localized name of the specified category.
		/// </summary>
		/// <param name="value">The identifer for the category to look up.</param>
		/// <returns>
		/// The localized name of the category, or null if a localized name does not exist.
		/// </returns>
		protected override string GetLocalizedString(string value)
		{
			return Properties.Resources.ResourceManager.GetString(value);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SRCategoryAttribute"/> class.
		/// </summary>
		/// <param name="category">Specifies the category name.</param>
		public NuGenSRCategoryAttribute(string category) : base(category)
		{
		}
	}
}

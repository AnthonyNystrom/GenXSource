/* -----------------------------------------------
 * NuGenSRCategoryAttribute.cs
 * Copyright © 2006-2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Genetibase.ApplicationBlocks.ComponentModel
{
	/// <summary>
	/// Provides resource strings support for <see cref="T:System.ComponentModel.CategoryAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Property)]
	internal sealed class NuGenSRCategoryAttribute : CategoryAttribute
	{
		protected override string GetLocalizedString(string value)
		{
			return Properties.Resources.ResourceManager.GetString(value);
		}

		public NuGenSRCategoryAttribute(string category) : base(category)
		{
		}
	}
}

/* -----------------------------------------------
 * SRDescriptionAttribute.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Genetibase.Shared.Controls.ComponentModel
{
	/// <summary>
	/// Provides resource strings support for <see cref="T:System.ComponentModel.DescriptionAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class NuGenSRDescriptionAttribute : DescriptionAttribute
	{
		public override string Description
		{
			get
			{
				if (!_replaced)
				{
					_replaced = true;
					base.DescriptionValue = Properties.Resources.ResourceManager.GetString(base.Description);
				}

				return base.Description;
			}
		}

		private bool _replaced;

		/// <summary>
		/// Initializes a new instance of the <see cref="SRDescriptionAttribute"/> class.
		/// </summary>
		/// <param name="description">The description text.</param>
		public NuGenSRDescriptionAttribute(string description)
			: base(description)
		{
		}
	}
}

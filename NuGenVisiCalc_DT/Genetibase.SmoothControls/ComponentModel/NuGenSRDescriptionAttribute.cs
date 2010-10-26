/* -----------------------------------------------
 * SRDescriptionAttribute.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Genetibase.SmoothControls.ComponentModel
{
	/// <summary>
	/// Provides resource strings support for <see cref="T:System.ComponentModel.DescriptionAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class NuGenSRDescriptionAttribute : DescriptionAttribute
	{
		/// <summary>
		/// Gets the description stored in this attribute.
		/// </summary>
		/// <value></value>
		/// <returns>The description stored in this attribute.</returns>
		public override string Description
		{
			get
			{
				if (!this.replaced) {
					this.replaced = true;
					base.DescriptionValue = Properties.Resources.ResourceManager.GetString(base.Description);
				}

				return base.Description;
			}
		}

		/// <summary>
		/// Indicates whether the value of the <see cref="P:SRDescriptionAttribute.Description"/> property
		/// was replaced with a string from the resources.
		/// </summary>
		private bool replaced;

		/// <summary>
		/// Initializes a new instance of the <see cref="SRDescriptionAttribute"/> class.
		/// </summary>
		/// <param name="description">The description text.</param>
		public NuGenSRDescriptionAttribute(string description) : base(description)
		{
		}
	}
}

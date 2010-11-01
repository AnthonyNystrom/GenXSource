/* -----------------------------------------------
 * SRDescriptionAttribute.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
		#region Declarations.Fields

		private bool replaced = false;

		#endregion

		#region Properties.Overridden

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

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SRDescriptionAttribute"/> class.
		/// </summary>
		/// <param name="description">The description text.</param>
		public NuGenSRDescriptionAttribute(string description) : base(description)
		{
		}

		#endregion
	}
}

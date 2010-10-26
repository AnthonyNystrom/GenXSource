/* -----------------------------------------------
 * NuGenSRDescriptionAttribute.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Genetibase.UI.NuGenMeters.ComponentModel
{
	/// <summary>
	/// Provides resource strings support for <see cref="T:System.ComponentModel.DescriptionAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class NuGenSRDescriptionAttribute : DescriptionAttribute
	{
		#region Declarations.Fields

		/// <summary>
		/// Indicates whether the value of the <see cref="P:SRDescriptionAttribute.Description"/> property
		/// was replaced with a string from the resources.
		/// </summary>
		private bool replaced = false;

		#endregion

		#region Properties.Overriden

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

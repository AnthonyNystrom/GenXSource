/* -----------------------------------------------
 * NuGenSRCategoryAttribute.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.UI.NuGenInterface.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Resources;
using System.Text;

namespace Genetibase.UI.NuGenInterface.ComponentModel
{
	/// <summary>
	/// Provides resource strings support for <see cref="CategoryAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Property)]
	internal sealed class NuGenSRCategoryAttribute : CategoryAttribute
	{
		#region Declarations.Trace

		private static readonly TraceSwitch _TraceSwitch = new TraceSwitch(typeof(NuGenSRCategoryAttribute).Name, typeof(NuGenSRCategoryAttribute).FullName);
		private static readonly bool _ErrorEnabled = _TraceSwitch.TraceError;

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Looks up the localized name of the specified category.
		/// </summary>
		/// <param name="value">The identifer for the category to look up.</param>
		/// <returns>
		/// The localized name of the category, or null if a localized name does not exist.
		/// </returns>
		protected override string GetLocalizedString(string value)
		{
			string localizedString = "";

			try
			{
				localizedString = Resources.ResourceManager.GetString(value);
			}
			catch (ArgumentNullException e)
			{
				if (_ErrorEnabled)
				{
					Trace.TraceError(e.Message);
				}
			}
			catch (InvalidOperationException e)
			{
				if (_ErrorEnabled)
				{
					Trace.TraceError(e.Message);
				}
			}
			catch (MissingManifestResourceException e)
			{
				if (_ErrorEnabled)
				{
					Trace.TraceError(e.Message);
				}
			}

			return localizedString;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SRCategoryAttribute"/> class.
		/// </summary>
		/// <param name="category">Specifies the category name.</param>
		public NuGenSRCategoryAttribute(string category) : base(category)
		{
		}

		#endregion
	}
}

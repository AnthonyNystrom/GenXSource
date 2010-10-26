/* -----------------------------------------------
 * NuGenSRDescriptionAttribute.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

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
	/// Provides resource strings support for <see cref="DescriptionAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class NuGenSRDescriptionAttribute : DescriptionAttribute
	{
		#region Declarations.Fields

		/// <summary>
		/// Indicates whether the value of the <see cref="Description"/> property
		/// was replaced with a string from the resources.
		/// </summary>
		private bool _Replaced = false;

		#endregion

		#region Declarations.Trace

		private static readonly TraceSwitch _TraceSwitch = new TraceSwitch(typeof(NuGenSRDescriptionAttribute).Name, typeof(NuGenSRDescriptionAttribute).FullName);
		private static readonly bool _ErrorEnabled = _TraceSwitch.TraceError;

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
				if (!_Replaced) {
					_Replaced = true;
					string descriptionValue = "";

					try
					{
						descriptionValue = Properties.Resources.ResourceManager.GetString(base.Description);
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

					base.DescriptionValue = descriptionValue;
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

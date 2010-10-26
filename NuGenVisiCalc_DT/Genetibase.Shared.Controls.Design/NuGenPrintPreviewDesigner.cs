/* -----------------------------------------------
 * NuGenPrintPreviewDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenPrintPreview"/>.
	/// </summary>
	public class NuGenPrintPreviewDesigner : ComponentDesigner
	{
		/// <summary>
		/// Allows a designer to change or remove items from the set of properties that it exposes through a <see cref="T:System.ComponentModel.TypeDescriptor"></see>.
		/// </summary>
		/// <param name="properties">The properties for the class of the component.</param>
		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("Location");
			base.PostFilterProperties(properties);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPrintPreviewDesigner"/> class.
		/// </summary>
		public NuGenPrintPreviewDesigner()
		{
		}
	}
}

/* -----------------------------------------------
 * NuGenPinpointListEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.Shared.Controls.Design.Properties.Resources;

using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides functionality to edit <see cref="NuGenPinpointWindow.ObjectCollection"/> during design-time.
	/// </summary>
	public class NuGenPinpointListEditor : NuGenStringCollectionEditor
	{
		/// <summary>
		/// Edits the value of the specified object using the specified service provider and context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
		/// <param name="provider">A service provider object through which editing services can be obtained.</param>
		/// <param name="value">The object to edit the value of.</param>
		/// <returns>
		/// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
		/// </returns>
		/// <exception cref="T:System.ComponentModel.Design.CheckoutException">An attempt to check out a file that is checked into a source code management program did not succeed.</exception>
		public override object EditValue(
			ITypeDescriptorContext context
			, IServiceProvider provider
			, object value
			)
		{
			NuGenPinpointList control = context.Instance as NuGenPinpointList;
			
			if ((control != null) && (control.DataSource != null))
			{
				throw new ArgumentException(res.Argument_DataSourceLocksItems);
			}

			return base.EditValue(context, provider, value);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPinpointListEditor"/> class.
		/// </summary>
		/// <param name="targetCollectionType"></param>
		public NuGenPinpointListEditor(Type targetCollectionType)
			: base(targetCollectionType)
		{
		}
	}
}

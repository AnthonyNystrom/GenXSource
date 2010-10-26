/* -----------------------------------------------
 * NuGenCollectionEditorInitializer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Initializes collection editor form.
	/// </summary>
	public static class NuGenCollectionEditorInitializer
	{
		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="formToInitialize"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void InitializeEditorForm(Form formToInitialize)
		{
			if (formToInitialize == null)
			{
				throw new ArgumentNullException("formToInitialize");
			}

			formToInitialize.AutoScaleMode = AutoScaleMode.Font;
			formToInitialize.MaximizeBox = false;
			formToInitialize.MinimizeBox = false;
			formToInitialize.Padding = new Padding(10);
			formToInitialize.ShowIcon = false;
			formToInitialize.ShowInTaskbar = false;
			formToInitialize.ShowInTaskbar = false;
			formToInitialize.StartPosition = FormStartPosition.CenterParent;
		}
	}
}

/* -----------------------------------------------
 * NuGenBaseCommonDialogEditor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;
using System.Drawing.Design;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides base functionality for a common dialog editor.
	/// </summary>
	public abstract class NuGenCommonDialogEditorBase : UITypeEditor
	{
		/*
		 * DefaultExtension
		 */

		private string _defaultExtension = "";

		/// <summary>
		/// Gets or sets the default extension for the dialog.
		/// </summary>
		public virtual string DefaultExtension
		{
			[DebuggerStepThrough]
			get
			{
				return _defaultExtension;
			}
			[DebuggerStepThrough]
			set
			{
				_defaultExtension = value;
			}
		}

		/*
		 * Filter
		 */

		private string _filter = "";

		/// <summary>
		/// Gets or sets the filter for the dialog.
		/// </summary>
		public virtual string Filter
		{
			[DebuggerStepThrough]
			get
			{
				return _filter;
			}
			[DebuggerStepThrough]
			set
			{
				_filter = value;
			}
		}
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommonDialogEditorBase"/> class.
		/// </summary>
		/// <param name="defaultExtension">Specifies the default extension for the dialog.</param>
		/// <param name="filter">Specifies the filter for the dialog.</param>
		protected NuGenCommonDialogEditorBase(string defaultExtension, string filter)
		{
			this.DefaultExtension = defaultExtension;
			this.Filter = filter;
		}		
	}
}

/* -----------------------------------------------
 * IConfigurationObject.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;

namespace Genetibase.Windows.Controls.Framework.Configuration
{
	/// <summary>
	/// </summary>
	public interface IConfigurationObject : INotifyPropertyChanged
	{
		/// <summary>
		/// </summary>
		void Clear();
		/// <summary>
		/// </summary>
		void ClearProperty(String name);
		/// <summary>
		/// </summary>
		IConfigurationObject CreateConfigurationObject();
		/// <summary>
		/// </summary>
		IConfigurationObjectCollection CreateConfigurationObjectCollection();
		/// <summary>
		/// </summary>
		Object GetProperty(String name);
		/// <summary>
		/// </summary>
		Object GetProperty(String name, Object defaultValue);
		/// <summary>
		/// </summary>
		Boolean HasProperty(String name);
		/// <summary>
		/// </summary>
		void SetProperty(String name, Object value);
		/// <summary>
		/// </summary>
		void SetProperty(String name, Object value, Object defaultValue);
	}
}

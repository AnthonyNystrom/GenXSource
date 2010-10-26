/* -----------------------------------------------
 * CodeOptionsModel.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Genetibase.Windows.Controls.Framework.Configuration;

namespace Genetibase.Windows.Controls.Code.UserInterface
{
	/// <summary>
	/// </summary>
	public class CodeOptionsModel : INotifyPropertyChanged
	{
		private Boolean convertTabsToSpace;
		private Int32 tabSize;

		/// <summary>
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// </summary>
		public void Load(IConfigurationObject value)
		{
			this.ConvertTabsToSpace = (Boolean)value.GetProperty("ConvertTabsToSpace", false);
			this.TabSize = (Int32)value.GetProperty("TabSize", 4);
		}

		private void OnPropertyChanged(String propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// </summary>
		public void Save(IConfigurationObject value)
		{
			value.SetProperty("ConvertTabsToSpace", this.ConvertTabsToSpace, false);
			value.SetProperty("TabSize", this.TabSize, 4);
		}

		/// <summary>
		/// </summary>
		public Boolean ConvertTabsToSpace
		{
			get
			{
				return this.convertTabsToSpace;
			}
			set
			{
				this.convertTabsToSpace = value;
				this.OnPropertyChanged("ConvertTabsToSpaces");
			}
		}

		/// <summary>
		/// </summary>
		public Int32 TabSize
		{
			get
			{
				return this.tabSize;
			}
			set
			{
				this.tabSize = value;
				this.OnPropertyChanged("TabSize");
			}
		}
	}
}

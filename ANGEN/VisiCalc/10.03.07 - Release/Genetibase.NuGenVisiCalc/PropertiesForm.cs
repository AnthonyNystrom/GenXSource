/* -----------------------------------------------
 * PropertiesForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;
using Genetibase.NuGenVisiCalc.Properties;
using Genetibase.WinApi;

namespace Genetibase.NuGenVisiCalc
{
	[System.ComponentModel.DesignerCategory("Form")]
	internal sealed partial class PropertiesForm : FloatingToolForm
	{
		private INuGenWindowStateTracker _windowStateTracker;

		private INuGenWindowStateTracker WindowStateTracker
		{
			get
			{
				if (_windowStateTracker == null)
				{
					Debug.Assert(ServiceProvider != null, "ServiceProvider != null");
					_windowStateTracker = ServiceProvider.GetService<INuGenWindowStateTracker>();

					if (_windowStateTracker == null)
					{
						throw new NuGenServiceNotFoundException<INuGenWindowStateTracker>();
					}
				}

				return _windowStateTracker;
			}
		}

		private INuGenServiceProvider _serviceProvider;

		private INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		public void SelectObject(Object objectToSelect)
		{
			_propertyGrid.SelectedObject = objectToSelect;
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			Settings.Default.PropertiesForm_Location = Location;
			Settings.Default.PropertiesForm_Size = Size;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Location = Settings.Default.PropertiesForm_Location;
			Size = Settings.Default.PropertiesForm_Size;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			WindowStateTracker.SetWindowState(this);
		}

		public PropertiesForm()
		{
			InitializeComponent();
		}

		public PropertiesForm(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
			InitializeComponent();
			SetStyle(ControlStyles.Opaque, true);
		}
	}
}
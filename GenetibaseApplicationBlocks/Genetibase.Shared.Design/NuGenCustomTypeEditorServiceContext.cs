/* -----------------------------------------------
 * NuGenCustomTypeEditorServiceContext.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides functionality to invoke an editor for a custom type.
	/// </summary>
	public sealed class NuGenCustomTypeEditorServiceContext : ITypeDescriptorContext, IWindowsFormsEditorService, IServiceProvider
	{
		#region Declarations.Fields

		private IContainer _container = null;
		private object _instance = null;
		private IServiceProvider _serviceProvider = null;
		private PropertyDescriptor _propertyDescriptor = null;

		#endregion

		#region ITypeDescriptorContext Members

		public IContainer Container
		{
			get
			{
				return _container;
			}
		}

		public object Instance
		{
			get
			{
				return _instance;
			}
		}

		public void OnComponentChanged()
		{
			IComponentChangeService cc = _serviceProvider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (cc != null)
				cc.OnComponentChanged(_instance, _propertyDescriptor, null, null);
		}

		public bool OnComponentChanging()
		{
			return true;
		}

		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return _propertyDescriptor;
			}
		}

		#endregion

		#region IServiceProvider Members

		public object GetService(Type serviceType)
		{
			return _serviceProvider.GetService(serviceType);
		}

		#endregion

		#region IWindowsFormsEditorService Members

		void IWindowsFormsEditorService.CloseDropDown()
		{
		}

		void IWindowsFormsEditorService.DropDownControl(Control control)
		{
		}

		DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
		{
			IUIService service = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));

			if (service != null)
			{
				return service.ShowDialog(dialog);
			}

			return dialog.ShowDialog();
		}

		#endregion

		#region Methods.Public

		public void SetInstance(object instance, PropertyDescriptor desc)
		{
			_instance = instance;
			_propertyDescriptor = desc;
		}

		#endregion

		#region Constructors

		public NuGenCustomTypeEditorServiceContext(IContainer container, IServiceProvider provider)
        {
            _container = container;
            _serviceProvider = provider;
        }

		#endregion
	}
}

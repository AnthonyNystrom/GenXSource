/* -----------------------------------------------
 * NuGenCustomTypeEditorServiceContext.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		#region ITypeDescriptorContext Members

		/// <summary>
		/// Gets the container representing this <see cref="T:System.ComponentModel.TypeDescriptor"></see> request.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.ComponentModel.IContainer"></see> with the set of objects for this <see cref="T:System.ComponentModel.TypeDescriptor"></see>; otherwise, null if there is no container or if the <see cref="T:System.ComponentModel.TypeDescriptor"></see> does not use outside objects.</returns>
		public IContainer Container
		{
			get
			{
				return _container;
			}
		}

		/// <summary>
		/// Gets the object that is connected with this type descriptor request.
		/// </summary>
		/// <value></value>
		/// <returns>The object that invokes the method on the <see cref="T:System.ComponentModel.TypeDescriptor"></see>; otherwise, null if there is no object responsible for the call.</returns>
		public object Instance
		{
			get
			{
				return _instance;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentChanged"></see> event.
		/// </summary>
		public void OnComponentChanged()
		{
			IComponentChangeService cc = _serviceProvider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			
			if (cc != null)
			{
				cc.OnComponentChanged(_instance, _propertyDescriptor, null, null);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.ComponentModel.Design.IComponentChangeService.ComponentChanging"></see> event.
		/// </summary>
		/// <returns>
		/// true if this object can be changed; otherwise, false.
		/// </returns>
		public bool OnComponentChanging()
		{
			return true;
		}

		/// <summary>
		/// Gets the <see cref="T:System.ComponentModel.PropertyDescriptor"></see> that is associated with the given context item.
		/// </summary>
		/// <value></value>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor"></see> that describes the given context item; otherwise, null if there is no <see cref="T:System.ComponentModel.PropertyDescriptor"></see> responsible for the call.</returns>
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return _propertyDescriptor;
			}
		}

		#endregion

		#region IServiceProvider Members

		/// <summary>
		/// Gets the service object of the specified type.
		/// </summary>
		/// <param name="serviceType">An object that specifies the type of service object to get.</param>
		/// <returns>
		/// A service object of type serviceType.-or- null if there is no service object of type serviceType.
		/// </returns>
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

		/// <summary>
		/// </summary>
		public void SetInstance(object instance, PropertyDescriptor propertyDescriptor)
		{
			_instance = instance;
			_propertyDescriptor = propertyDescriptor;
		}

		#endregion

		private IContainer _container;
		private object _instance;
		private IServiceProvider _serviceProvider;
		private PropertyDescriptor _propertyDescriptor;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCustomTypeEditorServiceContext"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="container"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="provider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenCustomTypeEditorServiceContext(IContainer container, IServiceProvider provider)
        {
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}

            _container = container;
            _serviceProvider = provider;
        }
	}
}

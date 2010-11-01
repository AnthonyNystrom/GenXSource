/* -----------------------------------------------
 * NuGenCollectionEditorServiceContext.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides functionality to invoke a collection editor on a <see cref="ICollection"/> type.
	/// </summary>
	public sealed class NuGenCollectionEditorServiceContext : IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
	{
		#region Declarations.Fields

		private IComponentChangeService _componentChangeSvc;
		private ComponentDesigner _designer;
		private PropertyDescriptor _targetProperty;

		#endregion

		#region IServiceProvider Members

		object IServiceProvider.GetService(Type serviceType)
		{
			if ((serviceType == typeof(ITypeDescriptorContext)) || (serviceType == typeof(IWindowsFormsEditorService)))
			{
				return this;
			}
			if (_designer.Component.Site != null)
			{
				return _designer.Component.Site.GetService(serviceType);
			}
			return null;
		}

		#endregion

		#region ITypeDescrpitorContext Members

		IContainer ITypeDescriptorContext.Container
		{
			get
			{
				if (_designer.Component.Site != null)
				{
					return _designer.Component.Site.Container;
				}
				return null;
			}
		}

		object ITypeDescriptorContext.Instance
		{
			get
			{
				return _designer.Component;
			}
		}

		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return _targetProperty;
			}
		}

		void ITypeDescriptorContext.OnComponentChanged()
		{
			this.ChangeService.OnComponentChanged(_designer.Component, _targetProperty, null, null);
		}

		bool ITypeDescriptorContext.OnComponentChanging()
		{
			try
			{
				this.ChangeService.OnComponentChanging(_designer.Component, _targetProperty);
			}
			catch (CheckoutException e)
			{
				if (e != CheckoutException.Canceled)
				{
					throw;
				}

				return false;
			}

			return true;
		}

		#endregion

		#region IWindowsFormsEditorService

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
			
			return dialog.ShowDialog(_designer.Component as IWin32Window);
		}

		#endregion

		#region Properties.Private

		private IComponentChangeService ChangeService
		{
			get
			{
				if (_componentChangeSvc == null)
				{
					_componentChangeSvc = (IComponentChangeService)((IServiceProvider)this).GetService(typeof(IComponentChangeService));
				}
				return _componentChangeSvc;
			}
		}

		#endregion

		#region Methods.Public.Static

		/// <summary>
		/// </summary>
		/// <param name="designer"></param>
		/// <param name="objectToChange"></param>
		/// <param name="propName"></param>
		/// <returns></returns>
		public static object EditValue(ComponentDesigner designer, object objectToChange, string propName)
		{
			PropertyDescriptor descriptor = TypeDescriptor.GetProperties(objectToChange)[propName];
			NuGenCollectionEditorServiceContext context = new NuGenCollectionEditorServiceContext(designer, descriptor);
			UITypeEditor editor = descriptor.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
			object oldValue = descriptor.GetValue(objectToChange);
			object newValue = editor.EditValue(context, context, oldValue);
			
			if (newValue != oldValue)
			{
				try
				{
					descriptor.SetValue(objectToChange, newValue);
				}
				catch (CheckoutException)
				{
				}
			}

			return newValue;
		}

		#endregion

		#region Methods.Private

		private void OnEditItems(object sender, EventArgs e)
		{
			object component = _targetProperty.GetValue(_designer.Component);
			
			if (component != null)
			{
				CollectionEditor editor = TypeDescriptor.GetEditor(component, typeof(UITypeEditor)) as CollectionEditor;
				
				if (editor != null)
				{
					editor.EditValue(this, this, component);
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEditorServiceContext"/> class.
		/// </summary>
		public NuGenCollectionEditorServiceContext(ComponentDesigner designer)
		{
			_designer = designer;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEditorServiceContext"/> class.
		/// </summary>
		public NuGenCollectionEditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
		{
			_designer = designer;
			_targetProperty = prop;

			if (prop == null)
			{
				prop = TypeDescriptor.GetDefaultProperty(designer.Component);

				if ((prop != null) && typeof(ICollection).IsAssignableFrom(prop.PropertyType))
				{
					_targetProperty = prop;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEditorServiceContext"/> class.
		/// </summary>
		public NuGenCollectionEditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop, string newVerbText)
			: this(designer, prop)
		{
			_designer.Verbs.Add(new DesignerVerb(newVerbText, new EventHandler(this.OnEditItems)));
		}

		#endregion
	}
}

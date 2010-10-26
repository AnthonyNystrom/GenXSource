using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design.Behavior;
using System.Drawing;
using System.Drawing.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
    /// <summary>
    /// Provides an implementation of IWindowsFormsEditorService and ITypeDescriptorContext.
    /// Also provides a static method to invoke a UITypeEditor given a designer, an object 
    /// and a property name.
    /// </summary>
    internal class NuGenEditorServiceContext : IWindowsFormsEditorService, ITypeDescriptorContext
    {
        private ComponentDesigner _designer;
        private IComponentChangeService _componentChangeSvc;
        private PropertyDescriptor _targetProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorServiceContext"/> class.
        /// </summary>
        /// <param name="designer">The designer.</param>
        internal NuGenEditorServiceContext(ComponentDesigner designer)
        {
            _designer = designer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorServiceContext"/> class.
        /// </summary>
        /// <param name="designer">The designer.</param>
        /// <param name="prop">A property descriptor.</param>
        internal NuGenEditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
        {
            _designer = designer;
            _targetProperty = prop;

            if (prop == null)
            {
                prop = TypeDescriptor.GetDefaultProperty(designer.Component);
                if (prop != null && typeof(ICollection).IsAssignableFrom(prop.PropertyType))
                {
                    _targetProperty = prop;
                }
            }

            Debug.Assert(_targetProperty != null, "Need PropertyDescriptor for ICollection property to associate collectoin edtior with.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorServiceContext"/> class.
        /// </summary>
        /// <param name="designer">The designer.</param>
        /// <param name="prop">A property descriptor.</param>
        /// <param name="newVerbText">A new design verb.</param>
        internal NuGenEditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop, string newVerbText)
            : this(designer, prop)
        {
            Debug.Assert(!string.IsNullOrEmpty(newVerbText), "newVerbText cannot be null or empty");
            _designer.Verbs.Add(new DesignerVerb(newVerbText, new EventHandler(this.OnEditItems)));
        }

        /// <summary>
        /// Edits the specified object's value.
        /// </summary>
        /// <param name="designer"></param>
        /// <param name="objectToChange"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object EditValue(ComponentDesigner designer, object objectToChange, string propName)
        {
            // Get PropertyDescriptor
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(objectToChange)[propName];

            // Create a Context
            NuGenEditorServiceContext context = new NuGenEditorServiceContext(designer, descriptor);

            // Get Editor
            UITypeEditor editor = descriptor.GetEditor(typeof(UITypeEditor)) as UITypeEditor;

            // Get value to edit
            object value = descriptor.GetValue(objectToChange);

            // Edit value
            object newValue = editor.EditValue(context, context, value);

            if (newValue != value)
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

        /// <summary>
        /// Our caching property for the IComponentChangeService
        /// </summary>
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

        /// <summary>
        /// Gets the container.
        /// </summary>
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

        /// <summary>
        /// Called when the component ahs been changed.
        /// </summary>
        void ITypeDescriptorContext.OnComponentChanged()
        {
            ChangeService.OnComponentChanged(_designer.Component, _targetProperty, null, null);
        }

        /// <summary>
        /// Called when the component is to be changed.
        /// </summary>
        /// <returns></returns>
        bool ITypeDescriptorContext.OnComponentChanging()
        {
            try
            {
                ChangeService.OnComponentChanging(_designer.Component, _targetProperty);
            }
            catch (CheckoutException checkoutException)
            {
                if (checkoutException == CheckoutException.Canceled)
                {
                    return false;
                }
                throw;
            }
            return true;
        }

        /// <summary>
        /// Get the component.
        /// </summary>
        object ITypeDescriptorContext.Instance
        {
            get
            {
                return _designer.Component;
            }
        }

        /// <summary>
        /// Get the property descriptor.
        /// </summary>
        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
        {
            get
            {
                return _targetProperty;
            }
        }

        /// <summary>
        /// Get the requested service.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>The service.</returns>
        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(ITypeDescriptorContext) ||
                serviceType == typeof(IWindowsFormsEditorService))
            {
                return this;
            }

            if (_designer.Component.Site != null)
            {
                return _designer.Component.Site.GetService(serviceType);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        void IWindowsFormsEditorService.CloseDropDown()
        {
            // we'll never be called to do this.
            //
            Debug.Fail("NOTIMPL");
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        void IWindowsFormsEditorService.DropDownControl(Control control)
        {
            // nope, sorry
            //
            Debug.Fail("NOTIMPL");
            return;
        }

        /// <summary>
        /// Shows the ëditor as a modal dialog.
        /// </summary>
        /// <param name="dialog">The form to show.</param>
        /// <returns>A DialogResult</returns>
        System.Windows.Forms.DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
        {
            IUIService uiSvc = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));
            if (uiSvc != null)
            {
                return uiSvc.ShowDialog(dialog);
            }
            else
            {
                return dialog.ShowDialog(_designer.Component as IWin32Window);
            }
        }

        /// <summary>
        /// When the verb is invoked, use all the stuff above to show the dialog, etc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditItems(object sender, EventArgs e)
        {
            object propertyValue = _targetProperty.GetValue(_designer.Component);
            if (propertyValue == null)
            {
                return;
            }
            CollectionEditor itemsEditor = TypeDescriptor.GetEditor(propertyValue, typeof(UITypeEditor)) as CollectionEditor;

            Debug.Assert(itemsEditor != null, "Didn't get a collection editor for type '" + _targetProperty.PropertyType.FullName + "'");
            if (itemsEditor != null)
            {
                itemsEditor.EditValue(this, this, propertyValue);
            }
        }
    }
}

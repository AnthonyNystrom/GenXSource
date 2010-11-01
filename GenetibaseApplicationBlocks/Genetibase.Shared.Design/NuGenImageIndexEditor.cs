/* -----------------------------------------------
 * NuGenImageIndexEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// </summary>
	public sealed class NuGenImageIndexEditor : UITypeEditor
	{
		#region Declarations.Fields

		private ImageList _currentImageList;
		private PropertyDescriptor _currentImageListProp;
		private object _currentInstance;
		private UITypeEditor _imageEditor;
		private string _parentImageListProperty;

		#endregion

		#region Methods.Public

		/*
		 * GetImageListProperty
		 */

		/// <summary>
		/// </summary>
		/// <param name="currentComponent"></param>
		/// <param name="instance"></param>
		/// <returns></returns>
		public PropertyDescriptor GetImageListProperty(PropertyDescriptor currentComponent, ref object instance)
		{
			if (instance is object[])
			{
				return null;
			}

			PropertyDescriptor descriptor = null;
			object bufferInstance = instance;
			RelatedImageListAttribute attribute = currentComponent.Attributes[typeof(RelatedImageListAttribute)] as RelatedImageListAttribute;
			
			if (attribute != null)
			{
				string[] namespaceItems = attribute.RelatedImageList.Split(new char[] { '.' });
				
				for (int i = 0; i < namespaceItems.Length; i++)
				{
					if (bufferInstance == null)
					{
						return descriptor;
					}
					
					PropertyDescriptor descriptorToCheck = TypeDescriptor.GetProperties(bufferInstance)[namespaceItems[i]];
					
					if (descriptorToCheck == null)
					{
						return descriptor;
					}
					
					if (i == (namespaceItems.Length - 1))
					{
						if (typeof(ImageList).IsAssignableFrom(descriptorToCheck.PropertyType))
						{
							instance = bufferInstance;
							return descriptorToCheck;
						}
					}
					else
					{
						bufferInstance = descriptorToCheck.GetValue(bufferInstance);
					}
				}
			}

			return descriptor;
		}

		#endregion

		#region Methods.Public.Overridden

		/*
		 * GetPaintValueSupported
		 */

		/// <summary>
		/// Indicates whether the specified context supports painting a representation of an object's value within the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
		/// <returns>
		/// true if <see cref="M:System.Drawing.Design.UITypeEditor.PaintValue(System.Object,System.Drawing.Graphics,System.Drawing.Rectangle)"></see> is implemented; otherwise, false.
		/// </returns>
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			if (_imageEditor != null)
			{
				return _imageEditor.GetPaintValueSupported(context);
			}

			return false;
		}

		/*
		 * PaintValue
		 */

		/// <summary>
		/// Paints a representation of the value of an object using the specified <see cref="T:System.Drawing.Design.PaintValueEventArgs"></see>.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Drawing.Design.PaintValueEventArgs"></see> that indicates what to paint and where to paint it.</param>
		public override void PaintValue(PaintValueEventArgs e)
		{
			if (_imageEditor != null)
			{
				Image image = null;

				if (e.Value is int)
				{
					image = this.GetImage(e.Context, (int)e.Value, null, true);
				}
				else if (e.Value is string)
				{
					image = this.GetImage(e.Context, -1, (string)e.Value, false);
				}
				if (image != null)
				{
					_imageEditor.PaintValue(new PaintValueEventArgs(e.Context, image, e.Graphics, e.Bounds));
				}
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * GetImage
		 */

		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="index"></param>
		/// <param name="key"></param>
		/// <param name="useIntIndex"></param>
		/// <returns></returns>
		private Image GetImage(ITypeDescriptorContext context, int index, string key, bool useIntIndex)
		{
			Image image = null;
			object bufferInstance = context.Instance;
			
			if (!(bufferInstance is object[]))
			{
				if ((index < 0) && (key == null))
				{
					return image;
				}
				
				if (((_currentImageList == null) || (bufferInstance != _currentInstance)) || ((_currentImageListProp != null) && (((ImageList)_currentImageListProp.GetValue(_currentInstance)) != _currentImageList)))
				{
					_currentInstance = bufferInstance;
					PropertyDescriptor descriptor = this.GetImageListProperty(context.PropertyDescriptor, ref bufferInstance);
					
					while ((bufferInstance != null) && (descriptor == null))
					{
						PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties(bufferInstance);

						foreach (PropertyDescriptor bufferDescriptor in descriptors)
						{
							if (typeof(ImageList).IsAssignableFrom(bufferDescriptor.PropertyType))
							{
								descriptor = bufferDescriptor;
								break;
							}
						}
						
						if (descriptor == null)
						{
							PropertyDescriptor descriptorToCheck = descriptors[_parentImageListProperty];
							
							if (descriptorToCheck != null)
							{
								bufferInstance = descriptorToCheck.GetValue(bufferInstance);
								continue;
							}

							bufferInstance = null;
						}
					}
					
					if (descriptor != null)
					{
						_currentImageList = (ImageList)descriptor.GetValue(bufferInstance);
						_currentImageListProp = descriptor;
						_currentInstance = bufferInstance;
					}
				}
				
				if (_currentImageList != null)
				{
					if (useIntIndex)
					{
						if ((_currentImageList != null) && (index < _currentImageList.Images.Count))
						{
							index = (index > 0) ? index : 0;
							image = _currentImageList.Images[index];
						}
						
						return image;
					}
					
					return _currentImageList.Images[key];
				}
			}
			
			return null;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImageIndexEditor"/> class.
		/// </summary>
		public NuGenImageIndexEditor()
		{
			_parentImageListProperty = "Parent";
			_imageEditor = (UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
		}

		#endregion
	}
}

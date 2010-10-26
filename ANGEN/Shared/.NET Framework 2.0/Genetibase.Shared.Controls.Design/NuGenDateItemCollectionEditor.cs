/* -----------------------------------------------
 * DateItemCollectionEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections; 
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.ComponentModel;   
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// A custom collection editor for <see cref="NuGenDateItemCollectionEditor"/>.
	/// </summary>
	internal sealed class NuGenDateItemCollectionEditor : CollectionEditor
	{
		private NuGenCalendar m_calendar;
		private ITypeDescriptorContext m_context;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDateItemCollectionEditor"/> class.
		/// </summary>
		/// <param name="type">The type of the collection for this editor to edit.</param>
		public NuGenDateItemCollectionEditor(Type type) : base(type)
		{	
		}

		/// <summary>
		/// Edits the value of the specified object using the specified service provider and context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
		/// <param name="provider">A service provider object through which editing services can be obtained.</param>
		/// <param name="value">The object to edit the value of.</param>
		/// <returns>
		/// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
		/// </returns>
		/// <exception cref="T:System.ComponentModel.Design.CheckoutException">An attempt to check out a file that is checked into a source code management program did not succeed.</exception>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			m_context = context;

			object returnObject = base.EditValue(context, provider, value);
			
			NuGenDateItemCollection collection = returnObject as NuGenDateItemCollection; 
			
			if (collection !=null)
			{
				collection.ModifiedEvent();
			}
			
			return returnObject;
		}

		/// <summary>
		/// </summary>
		protected override object CreateInstance(Type itemType)
		{
			object dateItem = base.CreateInstance(itemType);
			
			NuGenCalendar originalControl = (NuGenCalendar) m_context.Instance;
			m_calendar = originalControl;	
			
			((NuGenDateItem) dateItem).Date = DateTime.Today;
			((NuGenDateItem) dateItem).Calendar = m_calendar;
			return dateItem;
		}

		/// <summary>
		/// </summary>
		protected override void DestroyInstance(object instance)
		{
			base.DestroyInstance (instance);
		}
	}
}

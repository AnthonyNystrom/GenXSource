/* -----------------------------------------------
 * NuGenOrientationControlActionList.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides an action list for controls that support horizontal and vertical layout.
	/// </summary>
	public class NuGenOrientationControlActionList : DesignerActionList
	{
		#region Declarations.Fields

		private NuGenOrientationControlBase _orientationControl;

		#endregion

		#region Properties.Public

		/*
		 * Orientation
		 */

		/// <summary>
		/// </summary>
		public NuGenOrientationStyle Orientation
		{
			get
			{
				return _orientationControl.Orientation;
			}
			set
			{
				this.OrientationControlProperties["Orientation"].SetValue(value);
			}
		}

		#endregion

		#region Properties.Private

		/*
		 * OrientationControlProperties
		 */

		private NuGenPropertyDescriptorCollection _orientationControlProperties;

		private NuGenPropertyDescriptorCollection OrientationControlProperties
		{
			get
			{
				if (_orientationControlProperties == null)
				{
					Debug.Assert(_orientationControl != null, "_orientationControl != null");
					_orientationControlProperties = NuGenTypeDescriptor.Instance(_orientationControl).Properties;
				}

				return _orientationControlProperties;
			}
		}


		#endregion

		#region Methods.Public.Overridden

		/*
		 * GetSortedActionItems
		 */

		/// <summary>
		/// Returns the collection of <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> objects contained in the list.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.ComponentModel.Design.DesignerActionItem"></see> array that contains the items in this list.
		/// </returns>
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection items = new DesignerActionItemCollection();

			items.Add(new DesignerActionPropertyItem(
				"Orientation",
				Resources.ActionList_Orientation,
				Resources.Category_Appearance,
				Resources.Description_Orientation)
			);

			return items;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenOrientationControlActionList"/> class.
		/// </summary>
		/// <param name="orientationControl"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="orientationControl"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenOrientationControlActionList(NuGenOrientationControlBase orientationControl)
	        : base(orientationControl)
	    {
			if (orientationControl == null)
			{
				throw new ArgumentNullException("orientationControl");
			}

			_orientationControl = orientationControl;
	    }

	    #endregion
	}
}

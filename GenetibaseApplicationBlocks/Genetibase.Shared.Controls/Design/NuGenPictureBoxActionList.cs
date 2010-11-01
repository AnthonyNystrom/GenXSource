/* -----------------------------------------------
 * NuGenPictureBoxActionList.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	public class NuGenPictureBoxActionList : DesignerActionList
	{
		#region Properties.Public

		/*
		 * DisplayMode
		 */

		/// <summary>
		/// </summary>
		public NuGenDisplayMode DisplayMode
		{
			get
			{
				Debug.Assert(base.Component is NuGenPictureBox, "base.Component is NuGenPictureBox");
				return ((NuGenPictureBox)base.Component).DisplayMode;
			}
			set
			{
				PropertyDescriptor displayMode = TypeDescriptor.GetProperties(base.Component)["DisplayMode"];
				Debug.Assert(displayMode != null, "displayMode != null");
				displayMode.SetValue(base.Component, value);
			}
		}

		/*
		 * Image
		 */

		/// <summary>
		/// </summary>
		public Image Image
		{
			get
			{
				Debug.Assert(base.Component is NuGenPictureBox, "base.Component is NuGenPictureBox");
				return ((NuGenPictureBox)base.Component).Image;
			}
			set
			{
				PropertyDescriptor image = TypeDescriptor.GetProperties(base.Component)["Image"];
				Debug.Assert(image != null, "image != null");
				image.SetValue(base.Component, value);
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

			items.Add(
				new DesignerActionPropertyItem(
					"Image",
					Resources.ActionList_PictureBox_Image,
					Resources.Category_Properties,
					Resources.Description_PictureBox_Image
				)
			);

			items.Add(
				new DesignerActionPropertyItem(
					"DisplayMode",
					Resources.ActionList_PictureBox_DisplayMode,
					Resources.Category_Properties,
					Resources.Description_PictureBox_DisplayMode
				)
			);

			return items;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPictureBoxActionList"/> class.
		/// </summary>
		public NuGenPictureBoxActionList(NuGenPictureBoxDesigner designer)
			: base(designer.Component)
		{
		}

		#endregion
	}
}

/* -----------------------------------------------
 * NuGenSmoothPictureBoxActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using designRes = Genetibase.SmoothControls.Design.Properties.Resources;
using ctrlRes = Genetibase.SmoothControls.Properties.Resources;

using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls.Design
{
	internal sealed class NuGenSmoothPictureBoxActionList : DesignerActionList
	{
		/*
		 * DisplayMode
		 */

		/// <summary>
		/// </summary>
		public NuGenDisplayMode DisplayMode
		{
			get
			{
				Debug.Assert(base.Component is NuGenSmoothPictureBox, "base.Component is NuGenSmoothPictureBox");
				return ((NuGenSmoothPictureBox)base.Component).DisplayMode;
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
				Debug.Assert(base.Component is NuGenSmoothPictureBox, "base.Component is NuGenSmoothPictureBox");
				return ((NuGenSmoothPictureBox)base.Component).Image;
			}
			set
			{
				PropertyDescriptor image = TypeDescriptor.GetProperties(base.Component)["Image"];
				Debug.Assert(image != null, "image != null");
				image.SetValue(base.Component, value);
			}
		}

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
					designRes.ActionList_PictureBox_Image,
					ctrlRes.Category_Properties,
					ctrlRes.Description_PictureBox_Image
				)
			);

			items.Add(
				new DesignerActionPropertyItem(
					"DisplayMode",
					designRes.ActionList_PictureBox_DisplayMode,
					ctrlRes.Category_Properties,
					ctrlRes.Description_PictureBox_DisplayMode
				)
			);

			return items;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPictureBoxActionList"/> class.
		/// </summary>
		public NuGenSmoothPictureBoxActionList(NuGenSmoothPictureBox pictureBox)
			: base(pictureBox)
		{
		}
	}
}

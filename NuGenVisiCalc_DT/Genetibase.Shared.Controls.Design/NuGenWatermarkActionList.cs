/* -----------------------------------------------
 * NuGenWatermarkActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using ctrlRes = Genetibase.Shared.Controls.Properties.Resources;
using designRes = Genetibase.Shared.Controls.Design.Properties.Resources;

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	public class NuGenWatermarkActionList : DesignerActionList
	{
		/// <summary>
		/// </summary>
		public Image Watermark
		{
			get
			{
				return this.WatermarkProperties["Watermark"].GetValue<Image>();
			}
			set
			{
				this.WatermarkProperties["Watermark"].SetValue(value);
			}
		}

		/// <summary>
		/// </summary>
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public int WatermarkTransparency
		{
			get
			{
				return this.WatermarkProperties["WatermarkTransparency"].GetValue<int>();
			}
			set
			{
				this.WatermarkProperties["WatermarkTransparency"].SetValue(value);
			}
		}

		private NuGenPropertyDescriptorCollection _watermarkProperties;

		private NuGenPropertyDescriptorCollection WatermarkProperties
		{
			get
			{
				if (_watermarkProperties == null)
				{
					_watermarkProperties = NuGenTypeDescriptor.Instance(_watermark).Properties;
				}

				return _watermarkProperties;
			}
		}

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
					"Watermark"
					, designRes.ActionList_Watermark_Watermark
					, ctrlRes.Category_Appearance
					, ctrlRes.Description_Watermark_Watermark
				)
			);
			
			items.Add(
				new DesignerActionPropertyItem(
					"WatermarkTransparency"
					, designRes.ActionList_Watermark_WatermarkTransparency
					, ctrlRes.Category_Appearance
					, ctrlRes.Description_Watermark_WatermarkTransparency
				)
			);

			return items;
		}

		private NuGenWatermark _watermark;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWatermarkActionList"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="watermark"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenWatermarkActionList(NuGenWatermark watermark)
			: base(watermark)
		{
			if (watermark == null)
			{
				throw new ArgumentNullException("watermark");
			}

			_watermark = watermark;
		}
	}
}

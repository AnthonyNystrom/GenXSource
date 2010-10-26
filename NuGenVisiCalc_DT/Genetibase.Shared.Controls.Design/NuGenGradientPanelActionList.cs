/* -----------------------------------------------
 * NuGenGradientPanelActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using ctrlRes = Genetibase.Shared.Controls.Properties.Resources;
using designRes = Genetibase.Shared.Controls.Design.Properties.Resources;

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	public class NuGenGradientPanelActionList : DesignerActionList
	{
		/// <summary>
		/// </summary>
		public ContentAlignment WatermarkAlign
		{
			get
			{
				return this.GradientPanelProperties["WatermarkAlign"].GetValue<ContentAlignment>();
			}
			set
			{
				this.GradientPanelProperties["WatermarkAlign"].SetValue(value);
			}
		}

		/// <summary>
		/// </summary>
		public int WatermarkHeight
		{
			get
			{
				return this.GetWatermarkSize().Height;
			}
			set
			{
				this.GradientPanelProperties["WatermarkSize"].SetValue(
					new Size(this.GetWatermarkSize().Width, value)
				);
			}
		}


		/// <summary>
		/// </summary>
		public int WatermarkWidth
		{
			get
			{
				return this.GetWatermarkSize().Width;
			}
			set
			{
				this.GradientPanelProperties["WatermarkSize"].SetValue(
					new Size(value, this.GetWatermarkSize().Height)
				);
			}
		}

		private NuGenPropertyDescriptorCollection _gradientPanelProperties;

		private NuGenPropertyDescriptorCollection GradientPanelProperties
		{
			get
			{
				if (_gradientPanelProperties == null)
				{
					_gradientPanelProperties = NuGenTypeDescriptor.Instance(_gradientPanel).Properties;
				}

				return _gradientPanelProperties;
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
			DesignerActionItemCollection actionItems = new DesignerActionItemCollection();

			actionItems.Add(
				new DesignerActionPropertyItem("WatermarkAlign"
					, designRes.ActionList_Watermark_WatermarkAlign
					, ctrlRes.Category_Appearance
					, ctrlRes.Description_GradientPanel_WatermarkAlign
				)
			);
			
			actionItems.Add(
				new DesignerActionPropertyItem(
					"WatermarkWidth"
					, designRes.ActionList_Watermark_WatermarkWidth
					, ctrlRes.Category_Appearance
					, designRes.Description_GradientPanel_WatermarkWidth
				)
			);
			
			actionItems.Add(
				new DesignerActionPropertyItem(
					"WatermarkHeight"
					, designRes.ActionList_Watermark_WatermarkHeight
					, ctrlRes.Category_Appearance
					, designRes.Description_GradientPanel_WatermarkHeight
				)
			);
			
			return actionItems;
		}

		private Size GetWatermarkSize()
		{
			return this.GradientPanelProperties["WatermarkSize"].GetValue<Size>();
		}

		private NuGenGradientPanel _gradientPanel;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGradientPanelActionList"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="gradientPanel"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenGradientPanelActionList(NuGenGradientPanel gradientPanel)
			: base(gradientPanel)
		{
			if (gradientPanel == null)
			{
				throw new ArgumentNullException("gradientPanel");
			}

			_gradientPanel = gradientPanel;
		}
	}
}

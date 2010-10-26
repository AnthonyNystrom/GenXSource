/* -----------------------------------------------
 * NuGenCalendarActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using ctrlRes = Genetibase.Shared.Controls.Properties.Resources;
using dsgnRes = Genetibase.Shared.Controls.Design.Properties.Resources;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	public class NuGenCalendarActionList : DesignerActionList
	{
		private NuGenCalendar calendar;
		private DesignerActionUIService designerActionUISvc;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCalendarActionList"/> class.
		/// </summary>
		/// <param name="component">A component related to the <see cref="T:System.ComponentModel.Design.DesignerActionList"></see>.</param>
		public NuGenCalendarActionList(IComponent component)
			: base(component)
		{
			this.calendar = component as NuGenCalendar;

			// Cache a reference to DesignerActionUIService, so the
			// DesigneractionList can be refreshed.
			this.designerActionUISvc =
				GetService(typeof(DesignerActionUIService))
				as DesignerActionUIService;
		}

		// Helper method to retrieve control properties. Use of 
		// GetProperties enables undo and menu updates to work properly.
		private PropertyDescriptor GetPropertyByName(String propName)
		{
			return TypeDescriptor.GetProperties(calendar)[propName];
		}

		/// <summary>
		/// </summary>
		public NuGenDateItemCollection Dates
		{
			get
			{
				return calendar.Dates;
			}
		}

		/// <summary>
		/// </summary>
		public bool ShowHeader
		{
			get
			{
				return calendar.ShowHeader;
			}
			set
			{
				GetPropertyByName("ShowHeader").SetValue(calendar, value);
			}
		}

		/// <summary>
		/// </summary>
		public bool ShowFooter
		{
			get
			{
				return calendar.ShowFooter;
			}
			set
			{
				GetPropertyByName("ShowFooter").SetValue(calendar, value);
			}
		}

		/// <summary>
		/// </summary>
		public bool ShowWeekdays
		{
			get
			{
				return calendar.ShowWeekdays;
			}
			set
			{
				GetPropertyByName("ShowWeekdays").SetValue(calendar, value);
			}
		}

		/// <summary>
		/// </summary>
		public bool ShowWeeknumbers
		{
			get
			{
				return calendar.ShowWeeknumbers;
			}
			set
			{
				GetPropertyByName("ShowWeeknumbers").SetValue(calendar, value);
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
					"Dates"
					, dsgnRes.ActionList_Calendar_Dates
					, ctrlRes.Category_Appearance
					, dsgnRes.Description_Calendar_Dates
				)
			);

			items.Add(
				new DesignerActionPropertyItem(
					"ShowHeader"
					, dsgnRes.ActionList_Calendar_ShowHeader
					, ctrlRes.Category_Appearance
					, dsgnRes.Description_Calendar_ShowHeader
				)
			);

			items.Add(
				new DesignerActionPropertyItem(
					"ShowWeekdays"
					, dsgnRes.ActionList_Calendar_ShowWeekdays
					, ctrlRes.Category_Appearance
					, dsgnRes.Description_Calendar_ShowWeekdays
				)
			);

			items.Add(
				new DesignerActionPropertyItem(
					"ShowWeeknumbers"
					, dsgnRes.ActionList_Calendar_ShowWeeknumbers
					, ctrlRes.Category_Appearance
					, dsgnRes.Description_Calendar_ShowWeeknumbers
				)
			);

			items.Add(
				new DesignerActionPropertyItem(
					"ShowFooter"
					, dsgnRes.ActionList_Calendar_ShowFooter
					, ctrlRes.Category_Appearance
					, ""
				)
			);

			return items;
		}
	}
}

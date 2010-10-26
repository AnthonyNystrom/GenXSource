/* -----------------------------------------------
 * NuGenTrackBarActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using designProps = Genetibase.Shared.Controls.Design.Properties;
using ctrlProps = Genetibase.Shared.Controls.Properties;

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	public class NuGenTrackBarActionList : NuGenOrientationControlActionList
	{
		#region Declarations.Fields

		private NuGenTrackBar _trackBar;

		#endregion

		#region Properties.Public

		/*
		 * TickStyle
		 */

		/// <summary>
		/// </summary>
		public TickStyle TickStyle
		{
			get
			{
				return _trackBar.TickStyle;
			}
			set
			{
				this.TrackBarProperties["TickStyle"].SetValue(value);				
			}
		}

		#endregion

		#region Properties.Private

		/*
		 * TrackBarProperties
		 */

		private NuGenPropertyDescriptorCollection _trackBarProperties;

		private NuGenPropertyDescriptorCollection TrackBarProperties
		{
			get
			{
				if (_trackBarProperties == null)
				{
					Debug.Assert(_trackBar != null, "_trackBar != null");
					_trackBarProperties = NuGenTypeDescriptor.Instance(_trackBar).Properties;
				}

				return _trackBarProperties;
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
			DesignerActionItemCollection items = base.GetSortedActionItems();

			items.Add(new DesignerActionPropertyItem(
				"TickStyle",
				designProps.Resources.ActionList_TrackBar_TickStyle,
				ctrlProps.Resources.Category_Appearance,
				ctrlProps.Resources.Description_TrackBar_TickStyle)
			);

			return items;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTrackBarActionList"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="trackBar"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenTrackBarActionList(NuGenTrackBar trackBar)
			: base(trackBar)
		{
			if (trackBar == null)
			{
				throw new ArgumentNullException("trackBar");
			}

			_trackBar = trackBar;
		}

		#endregion
	}
}

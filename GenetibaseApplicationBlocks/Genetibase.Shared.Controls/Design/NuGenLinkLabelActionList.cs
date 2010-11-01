/* -----------------------------------------------
 * NuGenLinkLabelActionList.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// </summary>
	public class NuGenLinkLabelActionList : DesignerActionList
	{
		#region Declarations

		private NuGenLinkLabel _linkLabel;

		#endregion

		#region Properties.Public

		/*
		 * ActiveLinkColor
		 */

		/// <summary>
		/// </summary>
		public Color ActiveLinkColor
		{
			get
			{
				return _linkLabel.ActiveLinkColor;
			}
			set
			{
				TypeDescriptor.GetProperties(_linkLabel)["ActiveLinkColor"].SetValue(_linkLabel, value);
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
				return _linkLabel.Image;
			}
			set
			{
				TypeDescriptor.GetProperties(_linkLabel)["Image"].SetValue(_linkLabel, value);
			}
		}

		/*
		 * LinkColor
		 */

		/// <summary>
		/// </summary>
		public Color LinkColor
		{
			get
			{
				return _linkLabel.LinkColor;
			}
			set
			{
				TypeDescriptor.GetProperties(_linkLabel)["LinkColor"].SetValue(_linkLabel, value);
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

			items.Add(new DesignerActionHeaderItem(Resources.Category_Appearance));
			items.Add(new DesignerActionPropertyItem(
				"Image",
				Resources.ActionList_LinkLabel_Image,
				Resources.Category_Appearance,
				Resources.Description_LinkLabel_Image)
			);
			items.Add(new DesignerActionPropertyItem(
				"LinkColor",
				Resources.ActionList_LinkLabel_LinkColor,
				Resources.Category_Appearance,
				Resources.Description_LinkLabel_LinkColor)
			);
			items.Add(new DesignerActionPropertyItem(
				"ActiveLinkColor",
				Resources.ActionList_LinkLabel_ActiveLinkColor,
				Resources.Category_Appearance,
				Resources.Description_LinkLabel_ActiveLinkColor)
			);

			return items;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinkLabelActionList"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="linkLabel"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenLinkLabelActionList(NuGenLinkLabel linkLabel)
			: base(linkLabel)
		{
			if (linkLabel == null)
			{
				throw new ArgumentNullException("linkLabel");
			}

			_linkLabel = linkLabel;
		}

		#endregion
	}
}

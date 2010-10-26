/* -----------------------------------------------
 * NuGenControlReflectorActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using designProps = Genetibase.Shared.Controls.Design.Properties;
using ctrlProps = Genetibase.Shared.Controls.Properties;

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides an action list for the <see cref="NuGenControlReflector"/>.
	/// </summary>
	public class NuGenControlReflectorActionList : DesignerActionList
	{
		/*
		 * ControlToReflect
		 */

		/// <summary>
		/// </summary>
		public Control ControlToReflect
		{
			[SecurityPermission(SecurityAction.LinkDemand)]
			get
			{
				return _ctrlReflector.ControlToReflect;
			}
			set
			{
				try
				{
					this.ControlReflectorProperties["ControlToReflect"].SetValue(value);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "ControlReflector", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		/*
		 * ControlReflectorProperties
		 */

		private NuGenPropertyDescriptorCollection _controlReflectorProperties;

		private NuGenPropertyDescriptorCollection ControlReflectorProperties
		{
			get
			{
				if (_controlReflectorProperties == null)
				{
					_controlReflectorProperties = NuGenTypeDescriptor.Instance(_ctrlReflector).Properties;
				}

				return _controlReflectorProperties;
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

			items.Add(new DesignerActionPropertyItem(
				"ControlToReflect",
				designProps.Resources.ActionList_ControlReflector_ControlToReflect,
				ctrlProps.Resources.Category_Appearance,
				ctrlProps.Resources.Description_ControlReflector_ControlToReflect)
			);

			return items;
		}

		private NuGenControlReflector _ctrlReflector;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlReflectorActionList"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="ctrlReflector"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenControlReflectorActionList(NuGenControlReflector ctrlReflector)
			: base(ctrlReflector)
		{
			if (ctrlReflector == null)
			{
				throw new ArgumentNullException("ctrlReflector");
			}

			_ctrlReflector = ctrlReflector;
		}
	}
}

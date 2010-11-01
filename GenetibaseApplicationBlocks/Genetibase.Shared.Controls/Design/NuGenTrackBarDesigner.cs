/* -----------------------------------------------
 * NuGenTrackBarDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenTrackBar"/>.
	/// </summary>
	public class NuGenTrackBarDesigner : ControlDesigner
	{
		#region Declarations.Fields

		private NuGenTrackBar _trackBar;

		#endregion

		#region Properties.Public.Overriden

		/*
		 * ActionLists
		 */

		private DesignerActionListCollection _actionLists;

		/// <summary>
		/// Gets the design-time action lists supported by the component associated with the designer.
		/// </summary>
		/// <value></value>
		/// <returns>The design-time action lists supported by the component associated with the designer.</returns>
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (_actionLists == null)
				{
					_actionLists = new DesignerActionListCollection();
					_actionLists.Add(new NuGenTrackBarActionList((NuGenTrackBar)this.Component));
				}

				return _actionLists;
			}
		}

		/*
		 * SelectionRules
		 */

		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules"></see> values.</returns>
		public override SelectionRules SelectionRules
		{
			get
			{
				if (_trackBar.AutoSize)
				{
					SelectionRules excludeSelectionRules = SelectionRules.None;

					if (_trackBar.Orientation == NuGenOrientationStyle.Horizontal)
					{
						excludeSelectionRules = SelectionRules.TopSizeable | SelectionRules.BottomSizeable;
					}
					else
					{
						excludeSelectionRules = SelectionRules.LeftSizeable | SelectionRules.RightSizeable;
					}

					return base.SelectionRules & ~excludeSelectionRules;
				}

				return base.SelectionRules;
			}
		}

		#endregion

		#region Methids.Public.Overridden

		/*
		 * Initialize
		 */

		/// <summary>
		/// Initializes the designer with the specified component.
		/// </summary>
		/// <param name="component">The <see cref="T:System.ComponentModel.IComponent"></see> to associate the designer with. This component must always be an instance of, or derive from, <see cref="T:System.Windows.Forms.Control"></see>.</param>
		public override void Initialize(IComponent component)
		{
			_trackBar = (NuGenTrackBar)component;
			base.Initialize(component);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTrackBarDesigner"/> class.
		/// </summary>
		public NuGenTrackBarDesigner()
		{

		}

		#endregion
	}
}

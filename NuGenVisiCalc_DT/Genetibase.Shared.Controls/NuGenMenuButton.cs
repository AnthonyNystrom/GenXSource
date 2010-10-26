/* -----------------------------------------------
 * NuGenMenuButton.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SplitButtonInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	public class NuGenMenuButton : NuGenSplitButtonBase
	{
		/// <summary>
		/// Gets or sets the <see cref="NuGenContextMenuStrip"/> that provides the drop down items
		/// to display.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_MenuButton_DropDownMenuStrip")]
		public NuGenContextMenuStrip DropDownMenuStrip
		{
			get
			{
				return _dropDownMenuStrip;
			}
			set
			{
				_dropDownMenuStrip = value;
			}
		}

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseDown(System.Windows.Forms.MouseEventArgs)"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (_dropDownMenuStrip != null)
			{
				_dropDownMenuStrip.Show(
					this.PointToScreen(NuGenControlPaint.RectBLCorner(this.ClientRectangle))
				);
			}
		}

		private NuGenContextMenuStrip _dropDownMenuStrip;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMenuButton"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenSplitButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenSplitButtonRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenMenuButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}

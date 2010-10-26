/* -----------------------------------------------
 * NuGenSmoothColorTable.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Provides colors used for smooth controls rendering.
	/// </summary>
	public class NuGenSmoothColorTable : ProfessionalColorTable
	{
		#region Colors.Button

		/// <summary>
		/// Gets the starting color of the gradient used when the button is checked.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when the button is checked.</returns>
		public override Color ButtonCheckedGradientBegin
		{
			get
			{
				return Color.FromArgb(143, 175, 212);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used when the button is checked.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when the button is checked.</returns>
		public override Color ButtonCheckedGradientEnd
		{
			get
			{
				return Color.FromArgb(163, 190, 220);
			}
		}

		/// <summary>
		/// Gets the middle color of the gradient used when the button is checked.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used when the button is checked.</returns>
		public override Color ButtonCheckedGradientMiddle
		{
			get
			{
				return Color.FromArgb(153, 182, 216);
			}
		}

		/// <summary>
		/// Gets the solid color used when the button is checked.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color used when the button is checked.</returns>
		public override Color ButtonCheckedHighlight
		{
			get
			{
				return base.ButtonCheckedHighlight;
			}
		}

		/// <summary>
		/// Gets the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonCheckedHighlight"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonCheckedHighlight"></see>.</returns>
		public override Color ButtonCheckedHighlightBorder
		{
			get
			{
				return Color.FromArgb(111, 137, 176);
			}
		}

		/// <summary>
		/// Gets the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin"></see>, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle"></see>, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd"></see> colors.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin"></see>, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle"></see>, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd"></see> colors.</returns>
		public override Color ButtonPressedBorder
		{
			get
			{
				return Color.FromArgb(111, 137, 176);
			}
		}

		/// <summary>
		/// Gets the starting color of the gradient used when the button is pressed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when the button is pressed.</returns>
		public override Color ButtonPressedGradientBegin
		{
			get
			{
				return Color.FromArgb(143, 175, 212);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used when the button is pressed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when the button is pressed.</returns>
		public override Color ButtonPressedGradientEnd
		{
			get
			{
				return Color.FromArgb(163, 190, 220);
			}
		}

		/// <summary>
		/// Gets the middle color of the gradient used when the button is pressed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used when the button is pressed.</returns>
		public override Color ButtonPressedGradientMiddle
		{
			get
			{
				return Color.FromArgb(153, 182, 216);
			}
		}

		/// <summary>
		/// Gets the solid color used when the button is pressed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color used when the button is pressed.</returns>
		public override Color ButtonPressedHighlight
		{
			get
			{
				return base.ButtonPressedHighlight;
			}
		}

		/// <summary>
		/// Gets the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedHighlight"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedHighlight"></see>.</returns>
		public override Color ButtonPressedHighlightBorder
		{
			get
			{
				return base.ButtonPressedHighlightBorder;
			}
		}

		/// <summary>
		/// Gets the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin"></see>, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle"></see>, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd"></see> colors.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin"></see>, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle"></see>, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd"></see> colors.</returns>
		public override Color ButtonSelectedBorder
		{
			get
			{
				return Color.FromArgb(111, 137, 176);
			}
		}

		/// <summary>
		/// Gets the starting color of the gradient used when the button is selected.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when the button is selected.</returns>
		public override Color ButtonSelectedGradientBegin
		{
			get
			{
				return Color.FromArgb(177, 206, 239);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used when the button is selected.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when the button is selected.</returns>
		public override Color ButtonSelectedGradientEnd
		{
			get
			{
				return Color.FromArgb(155, 188, 229);
			}
		}

		/// <summary>
		/// Gets the middle color of the gradient used when the button is selected.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used when the button is selected.</returns>
		public override Color ButtonSelectedGradientMiddle
		{
			get
			{
				return Color.FromArgb(166, 197, 234);
			}
		}

		/// <summary>
		/// Gets the solid color used when the button is selected.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color used when the button is selected.</returns>
		public override Color ButtonSelectedHighlight
		{
			get
			{
				return base.ButtonSelectedHighlight;
			}
		}

		/// <summary>
		/// Gets the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedHighlight"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedHighlight"></see>.</returns>
		public override Color ButtonSelectedHighlightBorder
		{
			get
			{
				return base.ButtonSelectedHighlightBorder;
			}
		}

		#endregion

		#region Colors.Check

		/// <summary>
		/// Gets the solid color to use when the button is checked and gradients are being used.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color to use when the button is checked and gradients are being used.</returns>
		public override Color CheckBackground
		{
			get
			{
				return Color.FromArgb(177, 206, 239);
			}
		}

		/// <summary>
		/// Gets the solid color to use when the button is checked and selected and gradients are being used.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color to use when the button is checked and selected and gradients are being used.</returns>
		public override Color CheckPressedBackground
		{
			get
			{
				return Color.FromArgb(177, 206, 239);
			}
		}

		/// <summary>
		/// Gets the solid color to use when the button is checked and selected and gradients are being used.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color to use when the button is checked and selected and gradients are being used.</returns>
		public override Color CheckSelectedBackground
		{
			get
			{
				return Color.FromArgb(143, 175, 212);
			}
		}

		#endregion

		#region Colors.Grip

		/// <summary>
		/// Gets the color to use for shadow effects on the grip (move handle).
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the color to use for shadow effects on the grip (move handle).</returns>
		public override Color GripDark
		{
			get
			{
				return base.GripDark;
			}
		}

		/// <summary>
		/// Gets the color to use for highlight effects on the grip (move handle).
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the color to use for highlight effects on the grip (move handle).</returns>
		public override Color GripLight
		{
			get
			{
				return base.GripLight;
			}
		}

		#endregion

		#region Colors.ImageMargin

		/// <summary>
		/// Gets the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.</returns>
		public override Color ImageMarginGradientBegin
		{
			get
			{
				return Color.FromArgb(240, 242, 236);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.</returns>
		public override Color ImageMarginGradientEnd
		{
			get
			{
				return Color.FromArgb(222, 224, 218);
			}
		}

		/// <summary>
		/// Gets the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.</returns>
		public override Color ImageMarginGradientMiddle
		{
			get
			{
				return Color.FromArgb(231, 233, 227);
			}
		}

		/// <summary>
		/// Gets the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.</returns>
		public override Color ImageMarginRevealedGradientBegin
		{
			get
			{
				return base.ImageMarginRevealedGradientBegin;
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.</returns>
		public override Color ImageMarginRevealedGradientEnd
		{
			get
			{
				return base.ImageMarginRevealedGradientEnd;
			}
		}

		/// <summary>
		/// Gets the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.</returns>
		public override Color ImageMarginRevealedGradientMiddle
		{
			get
			{
				return base.ImageMarginRevealedGradientMiddle;
			}
		}

		#endregion

		#region Colors.Menu

		/// <summary>
		/// Gets the color that is the border color to use on a <see cref="T:System.Windows.Forms.MenuStrip"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use on a <see cref="T:System.Windows.Forms.MenuStrip"></see>.</returns>
		public override Color MenuBorder
		{
			get
			{
				return Color.FromArgb(128, 128, 128);
			}
		}

		/// <summary>
		/// Gets the border color to use with a <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use with a <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see>.</returns>
		public override Color MenuItemBorder
		{
			get
			{
				return Color.FromArgb(128, 128, 128);
			}
		}

		/// <summary>
		/// Gets the starting color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.</returns>
		public override Color MenuItemPressedGradientBegin
		{
			get
			{
				return Color.FromArgb(143, 175, 212);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.</returns>
		public override Color MenuItemPressedGradientEnd
		{
			get
			{
				return Color.FromArgb(163, 190, 220);
			}
		}

		/// <summary>
		/// Gets the middle color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.</returns>
		public override Color MenuItemPressedGradientMiddle
		{
			get
			{
				return Color.FromArgb(153, 182, 216);
			}
		}

		/// <summary>
		/// Gets the solid color to use when a <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> other than the top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color to use when a <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> other than the top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.</returns>
		public override Color MenuItemSelected
		{
			get
			{
				return Color.FromArgb(177, 206, 239);
			}
		}

		/// <summary>
		/// Gets the starting color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.</returns>
		public override Color MenuItemSelectedGradientBegin
		{
			get
			{
				return Color.FromArgb(177, 206, 239);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.</returns>
		public override Color MenuItemSelectedGradientEnd
		{
			get
			{
				return Color.FromArgb(155, 188, 229);
			}
		}

		/// <summary>
		/// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip"></see>.</returns>
		public override Color MenuStripGradientBegin
		{
			get
			{
				return Color.FromArgb(222, 224, 218);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip"></see>.</returns>
		public override Color MenuStripGradientEnd
		{
			get
			{
				return Color.FromArgb(240, 242, 236);
			}
		}

		#endregion

		#region Colors.OverflowButton

		/// <summary>
		/// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.</returns>
		public override Color OverflowButtonGradientBegin
		{
			get
			{
				return Color.FromArgb(128, 128, 128);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.</returns>
		public override Color OverflowButtonGradientEnd
		{
			get
			{
				return Color.FromArgb(128, 128, 128);
			}
		}

		/// <summary>
		/// Gets the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.</returns>
		public override Color OverflowButtonGradientMiddle
		{
			get
			{
				return Color.FromArgb(128, 128, 128);
			}
		}

		#endregion

		#region Colors.RaftingContainer

		/// <summary>
		/// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer"></see>.</returns>
		public override Color RaftingContainerGradientBegin
		{
			get
			{
				return base.RaftingContainerGradientBegin;
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer"></see>.</returns>
		public override Color RaftingContainerGradientEnd
		{
			get
			{
				return base.RaftingContainerGradientEnd;
			}
		}

		#endregion

		#region Colors.Separator

		/// <summary>
		/// Gets the color to use to for shadow effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the color to use to for shadow effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator"></see>.</returns>
		public override Color SeparatorDark
		{
			get
			{
				return Color.FromArgb(128, 128, 128);
			}
		}

		/// <summary>
		/// Gets the color to use to for highlight effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the color to use to for highlight effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator"></see>.</returns>
		public override Color SeparatorLight
		{
			get
			{
				return base.SeparatorLight;
			}
		}

		#endregion

		#region Colors.StatusStrip

		/// <summary>
		/// Gets the starting color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip"></see>.</returns>
		public override Color StatusStripGradientBegin
		{
			get
			{
				return Color.FromArgb(222, 224, 218);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip"></see>.</returns>
		public override Color StatusStripGradientEnd
		{
			get
			{
				return Color.FromArgb(240, 242, 236);
			}
		}

		#endregion

		#region Colors.ToolStrip

		/// <summary>
		/// Gets the border color to use on the bottom edge of the <see cref="T:System.Windows.Forms.ToolStrip"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use on the bottom edge of the <see cref="T:System.Windows.Forms.ToolStrip"></see>.</returns>
		public override Color ToolStripBorder
		{
			get
			{
				return Color.FromArgb(128, 128, 128);
			}
		}

		/// <summary>
		/// Gets the solid background color of the <see cref="T:System.Windows.Forms.ToolStripDropDown"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid background color of the <see cref="T:System.Windows.Forms.ToolStripDropDown"></see>.</returns>
		public override Color ToolStripDropDownBackground
		{
			get
			{
				return Color.FromArgb(240, 242, 236);
			}
		}

		/// <summary>
		/// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.</returns>
		public override Color ToolStripGradientBegin
		{
			get
			{
				return Color.FromArgb(240, 242, 236);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.</returns>
		public override Color ToolStripGradientEnd
		{
			get
			{
				return Color.FromArgb(222, 224, 218);
			}
		}

		/// <summary>
		/// Gets the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.</returns>
		public override Color ToolStripGradientMiddle
		{
			get
			{
				return Color.FromArgb(231, 233, 227);
			}
		}

		#endregion

		#region Colors.ToolStripContentPanel

		/// <summary>
		/// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel"></see>.</returns>
		public override Color ToolStripContentPanelGradientBegin
		{
			get
			{
				return Color.FromArgb(240, 242, 236);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel"></see>.</returns>
		public override Color ToolStripContentPanelGradientEnd
		{
			get
			{
				return Color.FromArgb(222, 224, 218);
			}
		}

		#endregion

		#region Colors.ToolStripPanel

		/// <summary>
		/// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel"></see>.</returns>
		public override Color ToolStripPanelGradientBegin
		{
			get
			{
				return Color.FromArgb(222, 224, 218);
			}
		}

		/// <summary>
		/// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel"></see>.</returns>
		public override Color ToolStripPanelGradientEnd
		{
			get
			{
				return Color.FromArgb(240, 242, 236);
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorTable"/> class.
		/// </summary>
		public NuGenSmoothColorTable()
		{

		}
	}
}

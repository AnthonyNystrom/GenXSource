/* -----------------------------------------------
 * NuGenPrintPreviewToolStripManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.PrintPreviewInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenPrintPreviewToolStripManager : INuGenPrintPreviewToolStripManager
	{
		#region INuGenPrintPreviewToolStripManager Members

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public ToolStripButton GetCloseToolStripButton()
		{
			ToolStripButton toolStripButton = new ToolStripButton(Resources.Text_CloseButton);
			toolStripButton.ToolTipText = Resources.ToolTip_CloseButton;
			return toolStripButton;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public ToolStripButton GetFourPagesToolStripButton()
		{
			ToolStripButton toolStripButton = new ToolStripButton(Resources.Image_FourPages);
			toolStripButton.ToolTipText = Resources.ToolTip_FourPagesButton;
			return toolStripButton;
		}

		/// <summary>
		/// </summary>
		public ToolStripLabel GetPageToolStripLabel()
		{
			return new ToolStripLabel(Resources.Text_Page);
		}
		
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public ToolStripButton GetPrintToolStripButton()
		{
			ToolStripButton toolStripButton = new ToolStripButton(Resources.Image_Print);
			toolStripButton.ToolTipText = Resources.ToolTip_PrintButton;
			return toolStripButton;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public ToolStripButton GetSinglePageToolStripButton()
		{
			ToolStripButton toolStripButton = new ToolStripButton(Resources.Image_SinglePage);
			toolStripButton.ToolTipText = Resources.ToolTip_SinglePageButton;
			return toolStripButton;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public ToolStripButton GetTwoPagesToolStripButton()
		{
			ToolStripButton toolStripButton = new ToolStripButton(Resources.Image_TwoPages);
			toolStripButton.ToolTipText = Resources.ToolTip_TwoPagesButton;
			return toolStripButton;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public ToolStripDropDownButton GetZoomToolStripDropDownButton()
		{
			ToolStripDropDownButton toolStripButton = new ToolStripDropDownButton(Resources.Image_Zoom);
			toolStripButton.ToolTipText = Resources.ToolTip_ZoomButton;
			return toolStripButton;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPrintPreviewToolStripManager"/> class.
		/// </summary>
		public NuGenPrintPreviewToolStripManager()
		{
		}
	}
}

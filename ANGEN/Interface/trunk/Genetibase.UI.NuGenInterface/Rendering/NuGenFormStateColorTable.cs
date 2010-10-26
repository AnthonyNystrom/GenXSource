/* -----------------------------------------------
 * NuGenFormStateColorTable.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Genetibase.UI.NuGenInterface.Rendering
{
	/// <summary>
	/// Encapsulates form caption colors.
	/// </summary>
	public class NuGenFormStateColorTable
	{
		#region Properties.Public

		/*
		 * CaptionBottomBackground
		 */

		private NuGenLinearGradientColorTable _CaptionBottomBackground = null;

		/// <summary>
		/// Gets or sets the <see cref="NuGenLinearGradientColorTable"/> that descripts the bottom part
		/// of the caption background.
		/// </summary>
		public NuGenLinearGradientColorTable CaptionBottomBackground
		{
			get
			{
				return _CaptionBottomBackground;
			}
			set
			{
				_CaptionBottomBackground = value;
			}
		}

		/*
		 * CaptionText
		 */

		private Color _CaptionText = Color.Empty;

		/// <summary>
		/// Gets or sets the caption text color.
		/// </summary>
		public Color CaptionText
		{
			get
			{
				return _CaptionText;
			}
			set
			{
				_CaptionText = value;
			}
		}
		
		/*
		 * CaptionTopBackground
		 */

		private NuGenLinearGradientColorTable _CaptionTopBackground = null;

		/// <summary>
		/// Gets or sets the <see cref="NuGenLinearGradientColorTable"/> that descripts the top part
		/// of the caption background.
		/// </summary>
		public NuGenLinearGradientColorTable CaptionTopBackground
		{
			get
			{
				return _CaptionTopBackground;
			}
			set
			{
				_CaptionTopBackground = value;
			}
		}

		/*
		 * InnerBorder
		 */

		private Color _InnerBorder = Color.Empty;

		/// <summary>
		/// Gets or sets the inner border color.
		/// </summary>
		public Color InnerBorder
		{
			get
			{
				return _InnerBorder;
			}
			set
			{
				_InnerBorder = value;
			}
		}

		/*
		 * OuterBorder
		 */

		private Color _OuterBorder = Color.Empty;

		/// <summary>
		/// Gets or sets the outer border color.
		/// </summary>
		public Color OuterBorder
		{
			get
			{
				return _OuterBorder;
			}
			set
			{
				_OuterBorder = value;
			}
		}

		#endregion
	}
}

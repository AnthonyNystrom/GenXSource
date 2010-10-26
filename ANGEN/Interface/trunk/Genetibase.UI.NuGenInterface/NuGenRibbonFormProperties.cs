/* -----------------------------------------------
 * NuGenRibbonFormProperties.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.UI.NuGenInterface
{
	/// <summary>
	/// Provides Ribbon form parameters.
	/// </summary>
	public class NuGenRibbonFormProperties : INuGenRibbonFormProperties
	{
		#region INuGenRibbonFormProperties Members

		/*
		 * BottomLeftCornerSize
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		public int BottomLeftCornerSize
		{
			get
			{
				return 6;
			}
		}

		/*
		 * BottomRightCornerSize
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		public int BottomRightCornerSize
		{
			get
			{
				return 6;
			}
		}

		/*
		 * DisplayRectangleReductionBottom
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		public int DisplayRectangleReductionBottom
		{
			get
			{
				return 2;
			}
		}

		/*
		 * DisplayRectangleReductionHorizontal
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		public int DisplayRectangleReductionHorizontal
		{
			get
			{
				return 2;
			}
		}

		/*
		 * DisplayRectangleReductionTop
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		public int DisplayRectangleReductionTop
		{
			get
			{
				return 1;
			}
		}

		/*
		 * TopLeftCornerSize
		 */

		/// <summary>
		/// </summary>
		public int TopLeftCornerSize
		{
			get
			{
				return 24;
			}
		}

		/*
		 * TopRightCornerSize
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		public int TopRightCornerSize
		{
			get
			{
				return 6;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonFormProperties"/> class.
		/// </summary>
		public NuGenRibbonFormProperties()
		{

		}

		#endregion
	}
}

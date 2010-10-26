/* -----------------------------------------------
 * NuGenColorTableInitializer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.UI.NuGenInterface.Rendering
{
	/// <summary>
	/// Provides methods to initialize a <see cref="NuGenColorTable"/> according to the specified
	/// <see cref="NuGenColorScheme"/>.
	/// </summary>
	static class NuGenColorTableInitializer
	{
		#region Methods.Public.Static

		/// <summary>
		/// Intializes the specified <see cref="NuGenColorTable"/> according to the specified
		/// <see cref="NuGenColorScheme"/>.
		/// </summary>
		/// <param name="colorTable">Specifies the <see cref="NuGenColorTable"/> to initialize.</param>
		/// <param name="colorScheme">Specifies the <see cref="NuGenColorScheme"/> to use for intialization.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="colorTable"/> is <see langword="null"/>.
		/// </exception>
		public static void InitializeColorTable(NuGenColorTable colorTable, NuGenColorScheme colorScheme)
		{
			if (colorTable == null)
			{
				throw new ArgumentNullException("colorTable");
			}
			
			switch (colorScheme)
			{
				case NuGenColorScheme.Blue:
				{
					BuildBlueColorTable(colorTable);
					break;
				}
				case NuGenColorScheme.Gray:
				{
					BuildGrayColorTable(colorTable);
					break;
				}
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * BuildBlueColorTable
		 */

		private static void BuildBlueColorTable(NuGenColorTable colorTable)
		{
			Debug.Assert(colorTable != null, "colorTable != null");

			if (colorTable != null)
			{
				colorTable.Form.Active.CaptionBottomBackground = new NuGenLinearGradientColorTable(
					"#CADEF7",
					"#E4EFFD"
					);
				colorTable.Form.Active.CaptionText = ColorTranslator.FromHtml("#3E6AAA");
				colorTable.Form.Active.CaptionTopBackground = new NuGenLinearGradientColorTable(
					"#E3EBF6",
					"#D9E7F9"
					);
				colorTable.Form.Active.OuterBorder = ColorTranslator.FromHtml("#3B5A82");
				colorTable.Form.Active.InnerBorder = ColorTranslator.FromHtml("#B1C6E1");

				colorTable.Form.BackColor = ColorTranslator.FromHtml("#AFC9EB");

				colorTable.Form.Inactive.CaptionBottomBackground = new NuGenLinearGradientColorTable(
					"#D8E1EC",
					"#E3E8EF"
					);
				colorTable.Form.Inactive.CaptionText = ColorTranslator.FromHtml("#3E6AAA");
				colorTable.Form.Inactive.CaptionTopBackground = new NuGenLinearGradientColorTable(
					"#E3E7EC",
					"#DEE5ED"
					);
				colorTable.Form.Inactive.OuterBorder = ColorTranslator.FromHtml("#97A5B7");
				colorTable.Form.Inactive.InnerBorder = ColorTranslator.FromHtml("#E6E5E5");
			}
		}

		/*
		 * BuildGrayColorTable
		 */

		private static void BuildGrayColorTable(NuGenColorTable colorTable)
		{
			Debug.Assert(colorTable != null, "colorTable != null");

			if (colorTable != null)
			{
				colorTable.Form.Active.CaptionBottomBackground = new NuGenLinearGradientColorTable(
					"#2F3030",
					"#3E3E3E"
					);
				colorTable.Form.Active.CaptionText = ColorTranslator.FromHtml("#AED1FF");
				colorTable.Form.Active.CaptionTopBackground = new NuGenLinearGradientColorTable(
					"#434752",
					"#3A3D45"
					);
				colorTable.Form.Active.OuterBorder = ColorTranslator.FromHtml("#2F2F2F");
				colorTable.Form.Active.InnerBorder = ColorTranslator.FromHtml("#4D4D4D");

				colorTable.Form.BackColor = ColorTranslator.FromHtml("#7D7D7D");

				colorTable.Form.Inactive.CaptionBottomBackground = new NuGenLinearGradientColorTable(
					"#929292",
					"#9E9E9E"
					);
				colorTable.Form.Inactive.CaptionText = ColorTranslator.FromHtml("#AED1FF");
				colorTable.Form.Inactive.CaptionTopBackground = new NuGenLinearGradientColorTable(
					"#9B9DA2",
					"#97989C"
					);
				colorTable.Form.Inactive.OuterBorder = ColorTranslator.FromHtml("#929292");
				colorTable.Form.Inactive.InnerBorder = ColorTranslator.FromHtml("#9F9F9F");
			}
		}

		#endregion
	}
}

/* -----------------------------------------------
 * NuGenColorsProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.ColorBoxInternals
{
	/// <summary>
	/// Provides a range of available colors for a color box.
	/// </summary>
	public sealed class NuGenColorsProvider : INuGenColorsProvider
	{
		#region INuGenColorsProvider Members

		private static readonly Rectangle _sampleImageBounds = new Rectangle(0, 0, 28, 11);

		/*
		 * FillWithColorSamples
		 */

		/// <summary>
		/// </summary>
		/// <param name="colors"></param>
		/// <param name="imageListToFill"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="colors"/> is <see langword="null"/>.</para>
		/// </exception>
		public void FillWithColorSamples(List<Color> colors, out ImageList imageListToFill)
		{
			if (colors == null)
			{
				throw new ArgumentNullException("colors");
			}

			imageListToFill = new ImageList();
			imageListToFill.ImageSize = _sampleImageBounds.Size;

			foreach (Color color in colors)
			{
				Image sampleImage = new Bitmap(_sampleImageBounds.Width, _sampleImageBounds.Height);

				using (Graphics g = Graphics.FromImage(sampleImage))
				{
					using (SolidBrush sb = new SolidBrush(color))
					using (Pen pen = new Pen(Color.Black))
					{
						g.FillRectangle(sb, _sampleImageBounds);
						g.DrawRectangle(pen, NuGenControlPaint.BorderRectangle(_sampleImageBounds));
					}
				}

				imageListToFill.Images.Add(sampleImage);
			}
		}

		private static readonly List<Color> _customColors;

		/*
		 * FillWithCustomColors
		 */

		/// <summary>
		/// </summary>
		/// <param name="customColors"></param>
		public void FillWithCustomColors(out List<Color> customColors)
		{
			customColors = _customColors;
		}

		/*
		 * FillWithStandardColors
		 */

		private static readonly List<Color> _standardColors;

		/// <summary>
		/// </summary>
		/// <param name="standardColors"></param>
		public void FillWithStandardColors(out List<Color> standardColors)
		{
			standardColors = _standardColors;
		}

		/*
		 * FillWithWebColors
		 */

		private static readonly List<Color> _webColors;

		/// <summary>
		/// </summary>
		/// <param name="webColors"></param>
		public void FillWithWebColors(out List<Color> webColors)
		{
			webColors = _webColors;
		}

		#endregion

		#region Methods.Private.Static

		/*
		 * GetCustomColors
		 */

		private static List<Color> GetCustomColors()
		{
			List<Color> colors = new List<Color>();

			colors.AddRange(new Color[] {
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 192, 192),
				Color.FromArgb(255, 224, 192),
				Color.FromArgb(255, 255, 192),
				Color.FromArgb(192, 255, 192),
				Color.FromArgb(192, 255, 255),
				Color.FromArgb(192, 192, 255),
				Color.FromArgb(255, 192, 255),
				Color.FromArgb(224, 224, 224),
				Color.FromArgb(255, 128, 128),
				Color.FromArgb(255, 192, 128),
				Color.FromArgb(255, 255, 128),
				Color.FromArgb(128, 255, 128),
				Color.FromArgb(128, 255, 255),
				Color.FromArgb(128, 128, 255),
				Color.FromArgb(255, 128, 255),
				Color.FromArgb(192, 192, 192),
				Color.FromArgb(255, 0, 0),
				Color.FromArgb(255, 128, 0),
				Color.FromArgb(255, 255, 0),
				Color.FromArgb(0, 255, 0),
				Color.FromArgb(0, 255, 255),
				Color.FromArgb(0, 0, 255),
				Color.FromArgb(255, 0, 255),
				Color.FromArgb(128, 128, 128),
				Color.FromArgb(192, 0, 0),
				Color.FromArgb(192, 64, 0),
				Color.FromArgb(192, 192, 0),
				Color.FromArgb(0, 192, 0),
				Color.FromArgb(0, 192, 192),
				Color.FromArgb(0, 0, 192),
				Color.FromArgb(192, 0, 192),
				Color.FromArgb(64, 64, 64),
				Color.FromArgb(128, 0, 0),
				Color.FromArgb(128, 64, 0),
				Color.FromArgb(128, 128, 0),
				Color.FromArgb(0, 128, 0),
				Color.FromArgb(0, 128, 128),
				Color.FromArgb(0, 0, 128),
				Color.FromArgb(128, 0, 128),
				Color.FromArgb(0, 0, 0),
				Color.FromArgb(64, 0, 0),
				Color.FromArgb(128, 64, 64),
				Color.FromArgb(64, 64, 0),
				Color.FromArgb(0, 64, 0),
				Color.FromArgb(0, 64, 64),
				Color.FromArgb(0, 0, 64),
				Color.FromArgb(64, 0, 64),

				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255),
				Color.FromArgb(255, 255, 255)}
			);

			return colors;
		}

		/*
		 * GetStandardColors
		 */

		private static List<Color> GetStandardColors()
		{
			List<Color> colors = new List<Color>();

			colors.AddRange(new Color[] {
				SystemColors.ActiveBorder,
				SystemColors.ActiveCaption,
				SystemColors.ActiveCaptionText,
				SystemColors.AppWorkspace,
				SystemColors.Control,
				SystemColors.ControlDark,
				SystemColors.ControlDarkDark,
				SystemColors.ControlLight,
				SystemColors.ControlLightLight,
				SystemColors.ControlText,
				SystemColors.Desktop,
				SystemColors.GrayText,
				SystemColors.Highlight,
				SystemColors.HighlightText,
				SystemColors.HotTrack,
				SystemColors.InactiveBorder,
				SystemColors.InactiveCaption,
				SystemColors.InactiveCaptionText,
				SystemColors.Info,
				SystemColors.InfoText,
				SystemColors.Menu,
				SystemColors.MenuText,
				SystemColors.ScrollBar,
				SystemColors.Window,
				SystemColors.WindowFrame,
				SystemColors.WindowText}
			);

			return colors;
		}

		/*
		 * GetWebColors
		 */

		private static List<Color> GetWebColors()
		{
			List<Color> colors = new List<Color>();

			colors.AddRange(new Color[] {
				Color.Transparent,
				Color.Black,
				Color.DimGray,
				Color.Gray,
				Color.DarkGray,
				Color.Silver,
				Color.LightGray,
				Color.Gainsboro,
				Color.WhiteSmoke,
				Color.White,
				Color.RosyBrown,
				Color.IndianRed,
				Color.Brown,
				Color.Firebrick,
				Color.LightCoral,
				Color.Maroon,
				Color.DarkRed,
				Color.Red,
				Color.Snow,
				Color.MistyRose,
				Color.Salmon,
				Color.Tomato,
				Color.DarkSalmon,
				Color.Coral,
				Color.OrangeRed,
				Color.LightSalmon,
				Color.Sienna,
				Color.SeaShell,
				Color.Chocolate,
				Color.SaddleBrown,
				Color.SandyBrown,
				Color.PeachPuff,
				Color.Peru,
				Color.Linen,
				Color.Bisque,
				Color.DarkOrange,
				Color.BurlyWood,
				Color.Tan,
				Color.AntiqueWhite,
				Color.NavajoWhite,
				Color.BlanchedAlmond,
				Color.PapayaWhip,
				Color.Moccasin,
				Color.Orange,
				Color.Wheat,
				Color.OldLace,
				Color.FloralWhite,
				Color.DarkGoldenrod,
				Color.Goldenrod,
				Color.Cornsilk,
				Color.Gold,
				Color.Khaki,
				Color.LemonChiffon,
				Color.PaleGoldenrod,
				Color.DarkKhaki,
				Color.Beige,
				Color.LightGoldenrodYellow,
				Color.Olive,
				Color.Yellow,
				Color.LightYellow,
				Color.Ivory,
				Color.OliveDrab,
				Color.YellowGreen,
				Color.DarkOliveGreen,
				Color.GreenYellow,
				Color.Chartreuse,
				Color.LawnGreen,
				Color.DarkSeaGreen,
				Color.ForestGreen,
				Color.LimeGreen,
				Color.LightGreen,
				Color.PaleGreen,
				Color.DarkGreen,
				Color.Green,
				Color.Lime,
				Color.Honeydew,
				Color.SeaGreen,
				Color.MediumSeaGreen,
				Color.SpringGreen,
				Color.MintCream,
				Color.MediumSpringGreen,
				Color.MediumAquamarine,
				Color.Aquamarine,
				Color.Turquoise,
				Color.LightSeaGreen,
				Color.MediumTurquoise,
				Color.DarkSlateGray,
				Color.PaleTurquoise,
				Color.Teal,
				Color.DarkCyan,
				Color.Aqua,
				Color.Cyan,
				Color.LightCyan,
				Color.Azure,
				Color.DarkTurquoise,
				Color.CadetBlue,
				Color.PowderBlue,
				Color.LightBlue,
				Color.DeepSkyBlue,
				Color.SkyBlue,
				Color.LightSkyBlue,
				Color.SteelBlue,
				Color.AliceBlue,
				Color.DodgerBlue,
				Color.SlateGray,
				Color.LightSlateGray,
				Color.LightSteelBlue,
				Color.CornflowerBlue,
				Color.RoyalBlue,
				Color.MidnightBlue,
				Color.Lavender,
				Color.Navy,
				Color.DarkBlue,
				Color.MediumBlue,
				Color.Blue,
				Color.GhostWhite,
				Color.SlateBlue,
				Color.DarkSlateBlue,
				Color.MediumSlateBlue,
				Color.MediumPurple,
				Color.BlueViolet,
				Color.Indigo,
				Color.DarkOrchid,
				Color.DarkViolet,
				Color.MediumOrchid,
				Color.Thistle,
				Color.Plum,
				Color.Violet,
				Color.Purple,
				Color.DarkMagenta,
				Color.Magenta,
				Color.Fuchsia,
				Color.Orchid,
				Color.MediumVioletRed,
				Color.DeepPink,
				Color.HotPink,
				Color.LavenderBlush,
				Color.PaleVioletRed,
				Color.Crimson,
				Color.Pink,
				Color.LightPink}
			);

			return colors;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenColorsProvider"/> class.
		/// </summary>
		public NuGenColorsProvider()
		{

		}
		
		static NuGenColorsProvider()
		{
			_customColors = GetCustomColors();
			_standardColors = GetStandardColors();
			_webColors = GetWebColors();
		}

		#endregion
	}
}

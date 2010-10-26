using System;
using System.Drawing;
using Agilix.Ink;

namespace Genetibase.UI
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class StationerySettings
	{
		// Horizontal Line
		public Color hLineColor;
		public StationeryLineStyle hLineStyle;
		public int hLineSpaceBetween;
		public int hLineSpaceBefore;

		// Vertical Line
		public Color vLineColor;
		public StationeryLineStyle vLineStyle;
		public int vLineSpaceBetween;
		public int vLineSpaceBefore;

		// Right Margin
		public Color rMarginColor;
		public StationeryLineStyle rMarginStyle;
		public int rMarginSpacing;

		// Left Margin
		public Color lMarginColor;
		public StationeryLineStyle lMarginStyle;
		public int lMarginSpacing;

		// Title
		public StationeryDateStyle titleDateStyle;
		public string titleString;
		public int titleSpaceAfter;
		public string displayName;

		// Title Rectangle
		public StationeryRectangleStyle titleRectangleStyle;
		public Color titleForegroundColor;
		public Color titleBackgroundColor;
		public Rectangle titleRectangle;

		// Background 
		public StationeryColorStyle backgroundStyle;
		public bool alignImageWithMinSize;
		public Color backgroundColor;
		public StationeryImageStyle backgroundImageStyle;
		public System.Drawing.Image backgroundImage;
		public System.IO.Stream backgroundImageData;
		public System.Drawing.Size backgroundImageSize;
		public float backgroundImageTransparency;

		// Minimum Size
		public int minHeight;
		public int minWidth;
		public Size minSize;

		public StationerySettings()
		{

		}

		public void Blank()
		{
			// Horizontal Line
			hLineColor = Color.DeepSkyBlue;
			hLineStyle = StationeryLineStyle.None;
			hLineSpaceBetween = 600;
			hLineSpaceBefore = 1200;

			// Vertical Line
			vLineColor = Color.DeepSkyBlue;
			vLineStyle = StationeryLineStyle.None;
			vLineSpaceBetween = 600;
			vLineSpaceBefore = 0;

			// Right Margin
			rMarginColor = Color.IndianRed;
			rMarginStyle = StationeryLineStyle.None;
			rMarginSpacing = 13000;

			// Left Margin
			lMarginColor = Color.IndianRed;
			lMarginStyle = StationeryLineStyle.None;
			lMarginSpacing = 1600;

			// Title
			titleDateStyle = StationeryDateStyle.None;
			titleString = "Note Title";
			titleSpaceAfter = 600;
			displayName = "";

			// Title Rectangle
			titleRectangleStyle = StationeryRectangleStyle.None;
			titleForegroundColor = Color.Black;
			titleBackgroundColor = Color.White;
			titleRectangle = new Rectangle(1000, 500, 12500, 2000);

			// Background 
			backgroundStyle = StationeryColorStyle.None;
			backgroundColor = Color.White;
			backgroundImageStyle = StationeryImageStyle.None;
			backgroundImage = null;
			backgroundImageData = null;
			backgroundImageSize = new Size(9000, 9000);
			backgroundImageTransparency = 1;
			alignImageWithMinSize = false;
		}
	}
}

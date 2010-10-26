using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Genetibase.Common.Interfaces;
using System.Collections.Specialized;
namespace Genetibase.BasicFilters
{

  /// <summary>
  /// Box filter class. Creates an isometric box and renders the image on that specific view . gives a 3d view.
  /// </summary>
  public class BoxFilter : BasicFilter
  {
    #region TODOs
    //TODO: Add the option to paint images instead
    //of colors on the side of the box
    #endregion TODOs

    #region Private Fields
    private int _boxDepth = 30; //Defualt
    private Color _boxStartColor = Color.DarkBlue; //Defualt
    private Color _boxEndColor =  Color.LightBlue; //Defualt
    private int angle = 0;
    private bool drawImageOnSides = false;

    #endregion Private Fields

    #region Filter Properties


      public bool DrawImageOnSides
      {
          get { return drawImageOnSides; }
          set { drawImageOnSides = value; }
      }

    /// <summary>
    /// Defines the "3d" depth of the box
    /// </summary>
    public int BoxDepth
    {
      get
      {
        return _boxDepth;
      }
      set
      {
        _boxDepth = value;
      }
    }
    /// <summary>
    /// Sets the starting color of the box gradient brush
    /// </summary>
    public Color BoxStartColor
    {
      get
      {
        return _boxStartColor;
      }
      set
      {
        _boxStartColor = value;
      }
    }
    /// <summary>
    /// Sets the ending color of the box gradient brush
    /// </summary>
    public Color BoxEndColor
    {
      get
      {
        return _boxEndColor;
      }
      set
      {
        _boxEndColor = value;
      }
    }

      public int Angle
      {
          get { return angle; }
          set { angle = value; }
      }
    #endregion Filter Properties

    #region Public Filter Methods
    /// <summary>
    /// Executes this filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>transformes image into a boxed layout</returns>
    /// <example>
    /// <code>
    /// Image transformed;
    /// BoxFilter box = new BoxFilter();
    /// box.BoxDepth = 20;
    /// box.BoxStartColor = Color.Tan;
    /// box.BoxEndColor = Color.Wheat;
    /// transformed = box.ExecuteFilter(myImg);
    /// </code>
    /// </example>
    public override Image ExecuteFilter(Image inputImage)
    {

      Bitmap raw = (Bitmap)inputImage;      
      double tempAngle = (double)180 / (double)angle;
      double alpha = Math.PI / tempAngle; //30deg
      //Setting up the box 3d depth values.
      int sideWidth = _boxDepth;
      int sideHeight = raw.Height;
      int topWidth = raw.Width;
      int topHeight = sideWidth;
      int totalWidth = (int)(sideWidth * Math.Cos(alpha) + raw.Width * Math.Cos(alpha));
      int totalHeight = (int)(raw.Height + raw.Width * Math.Sin(alpha) + sideWidth * Math.Sin(alpha));

      //Set up the new canvas
      Bitmap result = new Bitmap(totalWidth,totalHeight);
      Graphics g = Graphics.FromImage(result);
      g.InterpolationMode = InterpolationMode.HighQualityBicubic;
      //Set background
      g.FillRectangle(new SolidBrush(BackGroundColor), 0, 0, result.Width, result.Height);
      
      //FrontSide
      //Point rightBottom = new Point((int)(raw.Width * Math.Cos(alpha)) , raw.Height - (int)(raw.Width * Math.Sin(alpha)) + yAlign);
      Point leftTop = new Point((int)(sideWidth*Math.Cos(alpha)),(int)((sideWidth+raw.Width)*Math.Sin(alpha)));
      Point leftBottom = new Point((int)(sideWidth * Math.Cos(alpha)), raw.Height + (int)((sideWidth + raw.Width) * Math.Sin(alpha)));
      Point rightTop = new Point((int)((raw.Width+sideWidth) * Math.Cos(alpha)) , (int)(sideWidth*Math.Sin(alpha)));
      g.DrawImage(raw, new Point[] { leftTop, rightTop, leftBottom });

      
      //TopSide
      Point topUpperRight = new Point(rightTop.X - (int)(topHeight * Math.Cos(alpha)), rightTop.Y - (int)(topHeight * Math.Sin(alpha)));
      Point topLowerRight = new Point(rightTop.X, rightTop.Y);
      Point topLowerLeft = new Point(leftTop.X, leftTop.Y);
      Point topUpperLeft = new Point(leftTop.X - (int)(sideWidth * Math.Cos(alpha)), leftTop.Y - (int)(sideWidth * Math.Sin(alpha)));
      Point[] top = new Point[4];
      top[0] = topUpperLeft;
      top[1] = topUpperRight;
      top[2] = topLowerRight;
      top[3] = topLowerLeft;

      Point[] newTop = new Point[3];
      newTop[0] = top[0];
      newTop[1] = top[1];
      newTop[2] = top[3];


      if (drawImageOnSides)
      {
          Image img = inputImage.GetThumbnailImage(inputImage.Width, this.BoxDepth, null, IntPtr.Zero);
          //Image img = inputImage.GetThumbnailImage(rightTop.X - leftTop.X, this.BoxDepth, null, IntPtr.Zero);
          //img.Save("c:\\temp.png");

          //Image newerImage = new Bitmap(img.Width, img.Height + top[1].Y);
          //Graphics g2 = Graphics.FromImage(newerImage);
          //g2.Clear(Color.Red);
          //g2.DrawImage(img, 0, top[0].Y, img.Width, img.Height);
          //g2.Dispose();

          //TextureBrush b = new TextureBrush(inputImage);
          //g.DrawImage(img, newTop);
          ////g.FillPolygon(b, top);
          //b.Dispose();
          //img.Dispose();
          g.DrawImage(img, newTop);
      }
      else
      {
          LinearGradientBrush topBrush = new LinearGradientBrush(topLowerRight, topUpperLeft, _boxStartColor, _boxEndColor);
          g.FillPolygon(topBrush, top);
          topBrush.Dispose();
      }

      //LeftSide
      Point sideUpperRight = new Point(leftTop.X, leftTop.Y);
      Point sideLowerRight = new Point(leftBottom.X, leftBottom.Y);
      Point sideLowerLeft = new Point(leftBottom.X - (int)(sideWidth * Math.Cos(alpha)), leftBottom.Y - (int)(sideWidth * Math.Sin(alpha)));
      Point sideUpperLeft = new Point(leftTop.X - (int)(sideWidth * Math.Cos(alpha)), leftTop.Y - (int)(sideWidth * Math.Sin(alpha)));
      Point[] side = new Point[4];
      side[0] = sideUpperLeft;
      side[1] = sideUpperRight;
      side[2] = sideLowerRight;
      side[3] = sideLowerLeft;

      newTop = new Point[3];
      newTop[0] = side[0];
      newTop[1] = side[1];
      newTop[2] = side[3];

      if (drawImageOnSides)
      {
          Image img = inputImage.GetThumbnailImage(this.BoxDepth, inputImage.Height, null, IntPtr.Zero);
         // Image img = inputImage.GetThumbnailImage(this.BoxDepth, sideLowerRight.Y - sideUpperLeft.Y + (sideLowerRight.Y - sideLowerLeft.Y), null, IntPtr.Zero);

         // Image newerImage = new Bitmap(img.Width, img.Height + side[1].Y);
         // Graphics g2 = Graphics.FromImage(newerImage);
         // g2.Clear(Color.Red);
         // g2.DrawImage(img, 0, side[0].Y, img.Width, img.Height);
         // g2.Dispose();

         //TextureBrush b = new TextureBrush(newerImage);
         // g.FillPolygon(b, side);          
         // b.Dispose();

         // newerImage.Dispose();
         // img.Dispose();
          g.DrawImage(img, newTop);          
      }
      else
      {
          LinearGradientBrush sideBrush = new LinearGradientBrush(sideUpperLeft, sideLowerRight, _boxStartColor, _boxEndColor);
          g.FillPolygon(sideBrush, side);
          sideBrush.Dispose();
      }
      return result;
        
    }
    #endregion Public Filter Methods
  }
}

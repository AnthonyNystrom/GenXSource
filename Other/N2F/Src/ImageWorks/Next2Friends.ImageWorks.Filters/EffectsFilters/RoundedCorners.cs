using System;
using NuGenImageWorksXBAP.Common.Interfaces;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NuGenImageWorksXBAP.EffectsFilters
{
  /// <summary>
  /// Rounded Corners filter class .  Turns right 90 degrees corners to round corners
  /// </summary>
  public class RoundedCorners : EffectFilter
  {
    #region Public Properties Tokens
    public static string ROTATE_DEGREES_TOKEN = "radius";
    #endregion Public Properties Tokens

    #region Private Fields
    private float _cornerRadius = 50; //Default
    #endregion Private Fields

    
    #region Filter Properties
    /// <summary>
    /// Determins the corner's radius. in pixels
    /// </summary>
    public float CornerRadius
    {
      get
      {
        return _cornerRadius;
      }
      set
      {
        if (value > 0)
          _cornerRadius = value;
        else
          _cornerRadius = 0;
      }
    }
    #endregion Filter Properties

    #region Public Filter Methods
    /// <summary>
    /// Executes this curved corners 
    /// filter on the input image and returns the result
    /// Make sure you set the BackGroundColor property before running this filter.
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>Curved Corner Image</returns>
    /// <example>
    /// <code>
    /// Image transformed;
    /// RoundedCorners rounded = new RoundedCorners();
    /// rounded.BackGroundColor = Color.FromArgb(255, 255, 255, 255);
    /// rounded.CornerRadius = 15;
    /// transformed = rounded.ExecuteFilter(myImg);
    /// </code>
    /// </example>
    public override BitmapSource ExecuteFilter(BitmapSource inputImage)
    {
        //double radius = 10.0;

        //Rect imageWidth = new Rect(0, radius, image1.ActualWidth, image1.ActualHeight - radius * 2);
        //Rect imageHeight = new Rect(radius, 0, image1.ActualWidth - radius * 2, image1.ActualHeight);

        //RectangleGeometry rectangle1 = new RectangleGeometry(imageWidth);
        //RectangleGeometry rectangle2 = new RectangleGeometry(imageHeight);

        //EllipseGeometry ellipse1 = new EllipseGeometry(new Point(radius, radius), radius, radius);
        //EllipseGeometry ellipse2 = new EllipseGeometry(new Point(image1.ActualWidth - radius, radius), radius, radius);
        //EllipseGeometry ellipse3 = new EllipseGeometry(new Point(radius, image1.ActualHeight - radius), radius, radius);
        //EllipseGeometry ellipse4 = new EllipseGeometry(new Point(image1.ActualWidth - radius, image1.ActualHeight - radius), radius, radius);

        //GeometryGroup geometry = new GeometryGroup();
        //geometry.FillRule = FillRule.Nonzero;
        //geometry.Children.Add(rectangle1);
        //geometry.Children.Add(rectangle2);
        //geometry.Children.Add(ellipse1);
        //geometry.Children.Add(ellipse2);
        //geometry.Children.Add(ellipse3);
        //geometry.Children.Add(ellipse4);

        //Drawing drawing = new GeometryDrawing(Brushes.Black, null, geometry);
        //DrawingBrush brush = new DrawingBrush(drawing);

        //image1.OpacityMask = brush;

        return null;
    }
    #endregion Public Filter Methods
  }
}

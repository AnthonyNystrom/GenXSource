using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Genetibase.Common.Interfaces;
using System.Collections.Specialized;

namespace Genetibase.BasicFilters
{
  /// <summary>
  /// Rounded Corners filter class .  Turns right 90 degrees corners to round corners
  /// </summary>
  public class RoundedCorners : BasicFilter
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
    public override Image ExecuteFilter(Image inputImage)
    {
      Bitmap bm = new Bitmap(inputImage.Width, inputImage.Height);

      Graphics g = Graphics.FromImage(bm);
      g.DrawImage(inputImage, 0, 0, bm.Width, bm.Height);
      Brush backGroundBrush = new SolidBrush(BackGroundColor);
      
      //Top left
      GraphicsPath gp = new GraphicsPath();
      float radius = _cornerRadius;
      gp.AddLine(0, 0, radius / 2, 0);
      gp.AddLine(0, 0, 0, radius / 2);
      gp.AddArc(0, 0, radius, radius, 180, 90);
      g.FillPath(backGroundBrush, gp);

      //Top Right
      gp = new GraphicsPath();
      gp.AddLine(inputImage.Width - radius / 2, 0, inputImage.Width, 0);
      gp.AddLine(inputImage.Width, 0, inputImage.Width, radius / 2);
      gp.AddArc(inputImage.Width - radius, 0, radius, radius, 270, 90);
      g.FillPath(backGroundBrush, gp);

      //Bottom Left
      gp = new GraphicsPath();
      gp.AddLine(0, inputImage.Height - radius / 2, 0, inputImage.Height);
      gp.AddLine(0, inputImage.Height, radius / 2, inputImage.Height);
      gp.AddArc(0, inputImage.Height - radius, radius, radius, 90, 90);
      g.FillPath(backGroundBrush, gp);

      //Bottom Right
      gp = new GraphicsPath();
      gp.AddLine(inputImage.Width - radius / 2, inputImage.Height, inputImage.Width, inputImage.Height);
      gp.AddLine(inputImage.Width, inputImage.Height - radius / 2, inputImage.Width, inputImage.Height);
      gp.AddArc(inputImage.Width - radius, inputImage.Height - radius, radius, radius, 0, 90);
      g.FillPath(backGroundBrush, gp);

      return bm;
    }
    public override Image ExecuteFilterDemo(Image inputImage)
    {
      this.BackGroundColor = Color.FromArgb(255, 255, 255, 255);
      return this.ExecuteFilter(inputImage);
    }
    #endregion Public Filter Methods
  }
}

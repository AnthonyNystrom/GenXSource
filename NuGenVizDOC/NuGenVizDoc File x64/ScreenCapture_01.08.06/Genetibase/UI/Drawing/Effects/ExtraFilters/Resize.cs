using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Genetibase.Common.Interfaces;
using System.Collections.Specialized;
using System.Collections;

namespace Genetibase.BasicFilters
{
  /// <summary>
  /// Resize filter class. A simple functionality to resize images
  /// </summary>
  public class ResizeFilter : BasicFilter
  {

    

    #region Private Fields
    //Defualts
    private int _width = 50;
    private int _height = 50;
    InterpolationMode _interpolationType = InterpolationMode.Bicubic;
    #endregion Private Fields
    
    #region Public Properties Tokens
    public static string WIDTH_PARAM_TOKEN = "width";
    public static string HEIGHT_PARAM_TOKEN = "height";
    public static string INTERPOLATION_TYPE_TOKEN = "interpolationType";
    #endregion Public Properties Tokens

    #region Filter Properties
    /// <summary>
    /// Image width in pixels
    /// </summary>
    public int Width
    {
      set
      {
        _width = value;
      }
      get
      {
        return _width;
      }

    }
    /// <summary>
    /// Image height in pixels
    /// </summary>
    public int Height
    {
      set
      {
        _height = value;
      }
      get
      {
        return _height;
      }

    }
    /// <summary>
    /// Scaling interpolation mode . can be HighQualityBicubic, Bilinear, etc.
    /// </summary>
    public InterpolationMode InterpolationType
    {
      get
      {
        return _interpolationType;
      }

      set
      {
        _interpolationType = value;
      }
    }
    #endregion Region Filter Properties

    #region Public Filter Methods
    /// <summary>
    /// Executes this filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>transformed image</returns>
    /// <example>
    /// <code>
    /// Image transformed;
    /// ResizeFilter resize = new ResizeFilter();
    /// resize.Width = 100;
    /// resize.Height = 70;
    /// transformed = resize.ExecuteFilter(myImg);
    /// </code>
    /// </example>
    public override Image ExecuteFilter(Image inputImage)
    {
      Bitmap result = new Bitmap(_width, _height);
      Graphics g = Graphics.FromImage(result);
      g.InterpolationMode = _interpolationType;
      g.DrawImage(inputImage, 0, 0, _width, _height);
      return result;
    }

    /// <summary>
    /// Demonostration Function. Calls the filter with default properties
    /// To be used for presentation purposes.
    /// </summary>
    /// <param name="inputImage"></param>
    /// <returns>Transformed image</returns>
    /// <summary>
    public Image ExecuteFilterDemo(Image inputImage, NameValueCollection filterProperties)
    {
      return this.ExecuteFilter(inputImage);
    }
    #endregion Public Filter Methods

  }
}

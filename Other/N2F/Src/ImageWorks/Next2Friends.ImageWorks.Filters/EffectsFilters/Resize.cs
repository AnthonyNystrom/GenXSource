using System;
using NuGenImageWorksXBAP.Common.Interfaces;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Media.Imaging;

namespace NuGenImageWorksXBAP.EffectsFilters
{
  /// <summary>
  /// Resize filter class. A simple functionality to resize images
  /// </summary>
  public class ResizeFilter : EffectFilter
  {

    

    #region Private Fields
    //Defualts
    private int _width = 50;
    private int _height = 50;    
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
    public override BitmapSource ExecuteFilter(BitmapSource inputImage)
    {
        return null;
        /*
      Bitmap result = new Bitmap(_width, _height);
      Graphics g = Graphics.FromImage(result);
      g.InterpolationMode = _interpolationType;
      g.DrawImage(inputImage, 0, 0, _width, _height);
      return result;
         */
    }

    #endregion Public Filter Methods

  }
}

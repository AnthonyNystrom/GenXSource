using System;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Media.Imaging;
using Next2Friends.ImageWorks.Filters;


namespace Next2Friends.ImageWorks.EffectsFilters
{
	/// <summary>
	/// Summary description for BasicFilter.
	/// </summary>
	public abstract class EffectFilter :  IFilter
  {

    /// <summary>
    /// Executes this filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>transformed image</returns>
    public abstract BitmapSource ExecuteFilter(BitmapSource inputImage);

    public static EffectFilter Default { get { throw new NotImplementedException("This default has not been set"); } }

	}
}

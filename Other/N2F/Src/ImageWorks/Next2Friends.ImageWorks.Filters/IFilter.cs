using System;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;

namespace Next2Friends.ImageWorks.Filters
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public interface IFilter
	{
	  BitmapSource ExecuteFilter(BitmapSource inputImage);
	}
}

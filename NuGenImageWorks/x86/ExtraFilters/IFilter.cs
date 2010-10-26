using System;
using System.Drawing;
using System.Collections.Specialized;

namespace Genetibase.Common.Interfaces
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public interface IFilter
	{
	  Image ExecuteFilter(Image inputImage);
	}
}

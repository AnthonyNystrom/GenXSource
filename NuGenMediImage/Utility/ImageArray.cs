using System;
using System.Collections;
using System.Drawing;

namespace Genetibase.NuGenMediImage.Utility
{
	/// <summary>
	/// Summary description for ImageArrayList.
	/// </summary>
	public class ImageArray : ArrayList
	{
		public new Image this[int index]
		{
			get
			{
				return (Image)this.GetRange( index , 1 )[0];				
			}			
		}
	}
}

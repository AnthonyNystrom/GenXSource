using System;

namespace Genetibase.NuGenMediImage
{
	/// <summary>
	/// Summary description for BrightnessChangedEventArgs.
	/// </summary>
	public class ContrastChangedEventArgs:EventArgs
	{

		internal float contrast = 0;

		public float Contrast
		{
			get
			{
				return contrast;
			}
		}
	}
}

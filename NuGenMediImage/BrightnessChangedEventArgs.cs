using System;

namespace Genetibase.NuGenMediImage
{
	/// <summary>
	/// Summary description for BrightnessChangedEventArgs.
	/// </summary>
	public class BrightnessChangedEventArgs:EventArgs
	{

		internal float brightness = 0;

		public float Brightness
		{
			get
			{
				return brightness;
			}
		}
	}
}

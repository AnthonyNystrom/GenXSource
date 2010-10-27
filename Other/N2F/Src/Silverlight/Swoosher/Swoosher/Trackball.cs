using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Next2Friends.Swoosher.Media3D;

namespace Next2Friends.Swoosher
{
	public class Trackball
	{
		public static Vector3D ProjectToTrackball( double width, double height, Point point )
		{
			double x = point.X / ( width / 2 );    // Scale so bounds map to [0,0] - [2,2]
			double y = point.Y / ( height / 2 );

			x = x - 1;                           // Translate 0,0 to the center
			y = 1 - y;                           // Flip so +Y is up instead of down

			double z2 = 1 - x * x - y * y;       // z^2 = 1 - x^2 - y^2
			double z = z2 > 0 ? Math.Sqrt( z2 ) : 0;

			return new Vector3D( x, y, z );
		}
	}
}
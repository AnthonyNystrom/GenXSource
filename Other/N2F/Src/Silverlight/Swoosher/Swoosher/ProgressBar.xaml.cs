using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Next2Friends.Swoosher
{
	public class ProgressBar : Control
	{
		private Rectangle box;
		private Rectangle progress;

		public ProgressBar()
		{
			System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream( "Next2Friends.Swoosher.ProgressBar.xaml" );
			var root = this.InitializeFromXaml( new System.IO.StreamReader( s ).ReadToEnd() );
			box = (Rectangle)root.FindName( "box" );
			progress = (Rectangle)root.FindName( "progress" );
		}

		public double Progress
		{
			get { return progress.Width / ( box.Width - ( box.StrokeThickness * 2 ) ); }
			set
			{
				progress.Width = ( box.Width - ( box.StrokeThickness * 2 ) )
					* Math.Max( 0, Math.Min( 1, value ) );
			}
		}
	}
}
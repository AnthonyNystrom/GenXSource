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
	public class Loading : Control
	{
		Canvas root;

		public Loading()
		{
			System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream( "Next2Friends.Swoosher.Loading.xaml" );
			root = (Canvas)this.InitializeFromXaml( new System.IO.StreamReader( s ).ReadToEnd() );
		}

		public void Stop()
		{
			( (Storyboard)root.FindName( "stop" ) ).Begin();
		}

		private void stop_Completed( object sender, EventArgs e )
		{
			( (Storyboard)root.FindName( "rotation" ) ).Stop();
		}
	}
}
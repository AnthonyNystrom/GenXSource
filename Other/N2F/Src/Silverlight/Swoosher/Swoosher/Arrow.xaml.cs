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
	public class Arrow : Control
	{
		public event EventHandler Click;

		Canvas root;
		RotateTransform angle;

		public Arrow()
		{
			System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream( "Next2Friends.Swoosher.Arrow.xaml" );
			root = (Canvas)this.InitializeFromXaml( new System.IO.StreamReader( s ).ReadToEnd() );
			angle = (RotateTransform)root.FindName( "angle" );
		}

		public double Angle
		{
			get { return angle.Angle; }
			set { angle.Angle = value; }
		}

		private void BeginStoryboard( string name )
		{
			if ( Parent != null ) ( (Storyboard)root.Resources.FindName( name ) ).Begin();
		}

		private void StopStoryboard( string name )
		{
			if ( Parent != null ) ( (Storyboard)root.Resources.FindName( name ) ).Stop();
		}

		private void mouse_Enter( object sender, MouseEventArgs e )
		{
			BeginStoryboard( "highlight" );
		}

		private void mouse_Leave( object sender, EventArgs e )
		{
			BeginStoryboard( "dim" );
		}

		private void mouse_Click( object sender, MouseEventArgs e )
		{
			if ( Click != null ) Click( this, e );
		}
	}
}
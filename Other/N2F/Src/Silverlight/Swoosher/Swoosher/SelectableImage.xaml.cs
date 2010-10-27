using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Next2Friends.Swoosher.Media3D;

namespace Next2Friends.Swoosher
{
	public class SelectableImage : Control
	{
		public event EventHandler ImageLoaded;
		public event EventHandler Failed;
		public event EventHandler Click;
		public event EventHandler WebClick;

		private Downloader downloader;
		private FrameworkElement root;
		private ProgressBar progress;
		private Canvas content;
		private Rectangle outline;
		private Rectangle border;
		private Rectangle selection;
		private Image image;
		//private Image webImage;

		private bool pressed = false;
		private Point last;
		private double viewDistance = 1;
		private bool dimmed = false;

		/// <summary>
		/// Creates a new SelectableImage
		/// </summary>
		public SelectableImage()
		{
			System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream( "Next2Friends.Swoosher.SelectableImage.xaml" );
			root = this.InitializeFromXaml( new System.IO.StreamReader( s ).ReadToEnd() );
			progress = (ProgressBar)root.FindName( "progress" );
			content = (Canvas)root.FindName( "content" );
			outline = (Rectangle)root.FindName( "outline" );
			border = (Rectangle)root.FindName( "border" );
			selection = (Rectangle)root.FindName( "selection" );
			image = (Image)root.FindName( "image" );
			//webImage = (Image)root.FindName( "webImage" );

			root.Loaded += OnLoaded;

			downloader = new Downloader();
			downloader.DownloadProgressChanged += image_DownloadProgressChanged;
			downloader.DownloadFailed += image_Failed;
			downloader.Completed += image_Completed;
		}

		public SelectableImage( Uri source )
			: this()
		{
			Source = source;
		}

		/// <summary>
		/// Gets or sets the image source
		/// </summary>
		public Uri Source
		{
			get { return downloader.Uri; }
			set { downloader.Open( "GET", value ); downloader.Send(); }
		}

		private void BeginStoryboard( string name )
		{
			if ( Parent != null ) ( (Storyboard)root.Resources.FindName( name ) ).Begin();
		}

		private void StopStoryboard( string name )
		{
			if ( Parent != null ) ( (Storyboard)root.Resources.FindName( name ) ).Stop();
		}

		private void OnLoaded( object sender, EventArgs e )
		{
			if ( progress.Progress >= 1 )
			{
				BeginStoryboard( "showContent" );
				BeginStoryboard( "leave" );
				//BeginStoryboard( "webLeave" );
				if ( dimmed ) BeginStoryboard( "dimContent" );
				else BeginStoryboard( "highlightContent" );
			}
		}

		public double ViewDistance { get { return viewDistance; } }

		protected void image_DownloadProgressChanged( object sender, EventArgs e )
		{
			if ( progress.Progress == 0 ) BeginStoryboard( "showProgress" );
			progress.Progress = downloader.DownloadProgress;
		}

		protected void image_Completed( object sender, EventArgs e )
		{
			image.SetSource( downloader, "" );
			BeginStoryboard( "getSize" );
		}

		protected void image_Failed( object sender, EventArgs e )
		{
			if ( Failed != null ) Failed( this, e );
		}

		protected void getSize_Completed( object sender, EventArgs e )
		{
			if ( image.Width > 0 && image.Height > 0 )
			{
				SetSize( image.Width, image.Height );
				if ( ImageLoaded != null ) ImageLoaded( this, EventArgs.Empty );
				BeginStoryboard( "showContent" );
			}
			else
			{
				// Try again...
				BeginStoryboard( "getSize" );
			}
		}

		/// <summary>
		/// Set the image size
		/// </summary>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		public void SetSize( double width, double height )
		{
			var aspectRatio = width / height;

			image.SetValue( Image.WidthProperty, 1 );
			image.SetValue( Image.HeightProperty, 1 );

			var actualWidth = ( width * System.Math.Min( 1, aspectRatio ) ) / width;
			var actualHeight = ( height / System.Math.Max( 1, aspectRatio ) ) / height;
			var left = ( 1 - actualWidth ) / 2;
			var top = ( 1 - actualHeight ) / 2;

			viewDistance = ( actualWidth > actualHeight ) ? 1.5 : 1.4;

			SetSize( outline, left - 0.06, top - 0.06, actualWidth + 0.12, actualHeight + 0.12 );
			SetSize( border, left - 0.04, top - 0.04, actualWidth + 0.08, actualHeight + 0.08 );
			SetSize( selection, left - 0.08, top - 0.08, actualWidth + 0.16, actualHeight + 0.16 );

			//webImage.SetValue( Canvas.TopProperty, top + 0.01 );
			//webImage.SetValue( Canvas.LeftProperty, left + 0.01 );
		}

		private void SetSize( Visual visual, double left, double top, double width, double height )
		{
			visual.SetValue( Canvas.LeftProperty, left );
			visual.SetValue( Canvas.TopProperty, top );
			visual.SetValue( Canvas.WidthProperty, width );
			visual.SetValue( Canvas.HeightProperty, height );
		}

		/// <summary>
		/// Mouse enter
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		void image_MouseEnter( object sender, MouseEventArgs e )
		{
			BeginStoryboard( "enter" );
			BeginStoryboard( "highlightContent" );
		}

		/// <summary>
		/// Mouse leave
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		void image_MouseLeave( object sender, EventArgs e )
		{
			pressed = false;
			BeginStoryboard( "leave" );
			if ( dimmed ) BeginStoryboard( "dimContent" );
		}

		/// <summary>
		/// Mouse left button down
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		void image_MouseLeftButtonDown( object sender, MouseEventArgs e )
		{
			pressed = true;
			last = e.GetPosition( image );
		}

		/// <summary>
		/// Mouse left button up
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		void image_MouseLeftButtonUp( object sender, MouseEventArgs e )
		{
			if ( pressed && Click != null ) Click( this, e );
			pressed = false;
		}
/*
		/// <summary>
		/// Web link mouse enter
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		void webImage_MouseEnter( object sender, MouseEventArgs e )
		{
			BeginStoryboard( "webEnter" );
		}

		/// <summary>
		/// Web link mouse leave
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		void webImage_MouseLeave( object sender, EventArgs e )
		{
			BeginStoryboard( "webLeave" );
		}
*/
		/// <summary>
		/// Web link mouse left button up
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		void webImage_MouseLeftButtonUp( object sender, MouseEventArgs e )
		{
			if ( WebClick != null ) WebClick( this, e );
		}

		public bool Dimmed
		{
			get { return dimmed; }
			set
			{
				if ( dimmed != value )
				{
					dimmed = value;
					if ( dimmed ) { StopStoryboard( "highlightContent" ); BeginStoryboard( "dimContent" ); }
					else { StopStoryboard( "dimContent" ); BeginStoryboard( "highlightContent" ); }
				}
			}
		}

		public Size Size
		{
			get { return new Size( border.Width, border.Height ); }
		}
	}
}
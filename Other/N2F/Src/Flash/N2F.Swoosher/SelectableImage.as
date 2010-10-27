package
{
	import flash.display.*;
	import flash.events.*;
	import flash.geom.Matrix;
	import flash.net.URLRequest;
	import flash.utils.setTimeout;

	import fl.transitions.Tween;
	import fl.transitions.TweenEvent;
	import fl.transitions.easing.*;

	public class SelectableImage extends Sprite
	{
		private var loader : TaggedLoader;
		private var viewDistance : Number = 0.5;

		private var progressBar : ProgressBar;
		private var border : Sprite;
		private var shadow : Sprite;
		private var selection : Sprite;
		public var url = null;
		public var selectable : Sprite = null;
		public var downloader = null;
		public var pressed : Boolean = false;
		public var baseAlpha: Number = 1;
		public var last = null;
		public var click : Function = null;
		public var loaded : Function = null;
		public var failed : Function = null;
		public var progress = null;

		public function SelectableImage( url, onLoad )
		{
			this.url = url;

			progressBar = new ProgressBar();
			addChild( progressBar );
			progressBar.y = 45;
			progressBar.alpha = 0;

			shadow = new Sprite();
			addChild( shadow );
			shadow.mouseEnabled = false;
			shadow.mouseChildren = false;

			border = new Sprite();
			addChild( border );
			border.alpha = 0.01;

			border.buttonMode = true;
			border.mouseChildren = false;
			border.addEventListener( MouseEvent.MOUSE_OVER, onMouseOver );
			border.addEventListener( MouseEvent.MOUSE_OUT, onMouseOut );

			selection = new Sprite();
			addChild( selection );
			selection.alpha = 0.01;
			selection.mouseEnabled = false;
			selection.mouseChildren = false;

			var request : URLRequest = new URLRequest( url );

			loader = new TaggedLoader();
			border.addChild( loader );

//			loader.content.buttonMode = true;
			//loader.content.mouseChildren = false;

			loader.contentLoaderInfo.addEventListener( ProgressEvent.PROGRESS, onProgress );
			loader.contentLoaderInfo.addEventListener( Event.COMPLETE, onLoaded );

			loader.AssignTweens( [
				CreateTween( progressBar, "alpha", progressBar.alpha, 0.5, 0.5 ) ] );

			loader.load( request );
		}

		private function onProgress( e : ProgressEvent )
		{
			progressBar.Progress = e.target.bytesLoaded / e.target.bytesTotal;
			progressBar.Refresh();
		}

		private function onLoaded( e : Event )
		{
			var size : Number = 100;
			var borderSize : Number = 1.5;
			var doubleBorder : Number = borderSize * 2;
			var aspect : Number = loader.width / loader.height;

			loader.width = ( ( aspect >= 0 ) ? size : size * aspect ) - doubleBorder;
			loader.height = ( ( aspect > 0 ) ? size / aspect : size ) - doubleBorder;
			loader.x = ( size - loader.width ) / 2;
			loader.y = ( size - loader.height ) / 2;

			var selectBorder : Number = doubleBorder * 1.5;

			var g = selection.graphics;
			g.lineStyle( selectBorder, 0xA0A0FF );
			g.drawRect(
				loader.x - selectBorder,
				loader.y - selectBorder,
				loader.width + ( selectBorder * 2 ),
				loader.height + ( selectBorder * 2 ) );

			g = shadow.graphics;
			g.lineStyle( selectBorder, 0x000000, 0.3 );
			g.drawRect(
				loader.x - selectBorder,
				loader.y - selectBorder,
				loader.width + ( selectBorder * 2 ),
				loader.height + ( selectBorder * 2 ) );

			g = border.graphics;
			g.lineStyle( doubleBorder, 0xFFFFFF );
			g.drawRect(
				loader.x - borderSize,
				loader.y - borderSize,
				loader.width + doubleBorder,
				loader.height + doubleBorder );

			loader.StopTweens();
			loader.AssignTweens( [
				CreateTween( border, "alpha", border.alpha, 1, 0.5 ),
				CreateTween( progressBar, "alpha", progressBar.alpha, 0, 0.5 ) ] );
		}

		public function dispose()
		{
		}

		public function handleLoad( plugIn, userContext, rootElement )
		{
			this.selectable = rootElement.findName( "selectableContent" );

			var progressContainer = rootElement.FindName( "progressContainer" );
/*
			this.progress = new ProgressBar( plugIn, Silverlight.createDelegate( this, function( sender, args )
			{
				progressContainer.Children.Add( args.xaml );
			} ) );
*//*
			this.selectable.addEventListener( "Loaded", Silverlight.createDelegate( this, this.OnLoaded ) );

			this.selectable.addEventListener( "MouseEnter", Silverlight.createDelegate( this, this.image_MouseEnter ) );
			this.selectable.addEventListener( "MouseLeave", Silverlight.createDelegate( this, this.image_MouseLeave ) );
			this.selectable.addEventListener( "MouseLeftButtonDown", Silverlight.createDelegate( this, this.image_MouseLeftButtonDown ) );
			this.selectable.addEventListener( "MouseLeftButtonUp", Silverlight.createDelegate( this, this.image_MouseLeftButtonUp ) );

			this.downloader = plugIn.CreateObject( "Downloader" );
			this.downloader.addEventListener( "DownloadProgressChanged", Silverlight.createDelegate( this, this.image_DownloadProgressChanged ) );
			this.downloader.addEventListener( "DownloadFailed", Silverlight.createDelegate( this, this.image_Failed ) );
			this.downloader.addEventListener( "Completed", Silverlight.createDelegate( this, this.image_Completed ) );

			var getSize = rootElement.Resources.FindName( "getSize" );
			getSize.addEventListener( "Completed", Silverlight.createDelegate( this, this.getSize_Completed ) );*/
		}

		public function BeginStoryboard( name )
		{
			//this.selectable.Resources.FindName( name ).Begin();
		}

		public function StopStoryboard( name )
		{
			//this.selectable.Resources.FindName( name ).Stop();
		}
/*
		public function OnLoaded( sender, e )
		{
			if ( this.url != null )
			{
				this.downloader.Open( "GET", this.url );
				this.downloader.Send();
			}

			if ( this.progress.getProgress() >= 1 )
			{
				this.BeginStoryboard( "showContent" );
				this.BeginStoryboard( "leave" );
				//BeginStoryboard( "webLeave" );
				if ( this.dimmed ) this.BeginStoryboard( "dimContent" );
				else this.BeginStoryboard( "highlightContent" );
			}
		}
*/
		public function get AspectRatio() { return loader.width / loader.height; }

		public function image_DownloadProgressChanged( sender, e )
		{
			if ( this.progress.getProgress() == 0 ) this.BeginStoryboard( "showProgress" );
			this.progress.setProgress( this.downloader.DownloadProgress );
		}

		public function image_Completed( sender, e )
		{
			//this.selectable.FindName( "image" ).SetSource( sender, "" );
			this.BeginStoryboard( "getSize" );
		}

		public function image_Failed( sender, e )
		{
			if ( this.failed != null ) this.failed( this, e );
		}

		public function getSize_Completed( sender, e )
		{
/*			var image = this.selectable.FindName( "image" );

			if ( image.Width > 0 && image.Height > 0 )
			{
				this.SetSize( image, image.Width, image.Height );
				if ( this.loaded != null ) this.loaded( this, e );
				this.BeginStoryboard( "showContent" );
			}
			else
			{
				// Try again...
				this.BeginStoryboard( "getSize" );
			}*/
		}

		public function SetSize( image, width, height )
		{
			var aspectRatio = width / height;

			image.Width = 1;
			image.Height = 1;

			var actualWidth = ( width * Math.min( 1, aspectRatio ) ) / width;
			var actualHeight = ( height / Math.max( 1, aspectRatio ) ) / height;
			var left = ( 1 - actualWidth ) / 2;
			var top = ( 1 - actualHeight ) / 2;

			viewDistance = ( actualWidth > actualHeight ) ? 1.0 : 1.4;

			this.SetVisualSize( "outline", left - 0.06, top - 0.06, actualWidth + 0.12, actualHeight + 0.12 );
			this.SetVisualSize( "border", left - 0.04, top - 0.04, actualWidth + 0.08, actualHeight + 0.08 );
			this.SetVisualSize( "selection", left - 0.08, top - 0.08, actualWidth + 0.16, actualHeight + 0.16 );

			//webImage.SetValue( Canvas.TopProperty, top + 0.01 );
			//webImage.SetValue( Canvas.LeftProperty, left + 0.01 );
		}

		public function SetVisualSize( name, left, top, width, height )
		{/*
			var visual = this.selectable.FindName( name );

			visual.SetValue( "Canvas.Left", left );
			visual.SetValue( "Canvas.Top", top );
			visual.Width = width;
			visual.Height = height;*/
		}

		/// <summary>
		/// Mouse enter
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		public function onMouseOver( e : MouseEvent )
		{
			trace( "onMouseOver: " + e.target.name );

			loader.StopTweens();
			loader.AssignTweens( [
				CreateTween( selection, "alpha", selection.alpha, 1, 0.1 ),
				CreateTween( border, "alpha", border.alpha, 1, 0.5 ) ] );
/*
			setTimeout( Silverlight.createDelegate( this, function()
			{
				this.BeginStoryboard( "enter" );
				this.BeginStoryboard( "highlightContent" );
			} ), 0 );*/
		}

		/// <summary>
		/// Mouse leave
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		public function onMouseOut( e : MouseEvent )
		{
			trace( "onMouseOut: " + e.target.name );

			this.pressed = false;

			loader.StopTweens();
			loader.AssignTweens( [
				CreateTween( selection, "alpha", selection.alpha, 0.01, 1 ),
				CreateTween( border, "alpha", border.alpha, baseAlpha, 0.5 ) ] );
		/*
			setTimeout( Silverlight.createDelegate( this, function()
			{
				this.BeginStoryboard( "leave" );
				if ( this.dimmed ) this.BeginStoryboard( "dimContent" );
			} ), 0 );*/
		}

		/// <summary>
		/// Mouse left button down
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		public function image_MouseLeftButtonDown( sender, e )
		{
			this.pressed = true;
			this.last = e.GetPosition( sender );
		}

		/// <summary>
		/// Mouse left button up
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		public function image_MouseLeftButtonUp( sender, e )
		{
			if ( this.pressed && this.click != null ) this.click( this, e );
			this.pressed = false;
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
		public function webImage_MouseLeftButtonUp( sender, e )
		{
//		if ( WebClick != null ) WebClick( this, e );
		}

//		public function GetDimmed() { return dimmed; }

		public function SetBaseAlpha( value : Number )
		{
			if ( baseAlpha != value )
			{
				baseAlpha = value;

				loader.StopTweens();
				loader.AssignTweens( [
					CreateTween( border, "alpha", border.alpha, baseAlpha, 1 ) ] );
			}
		}

		private function CreateTween( obj : Object, prop : String, begin : Number, finish : Number, duration : Number ):Tween
		{
			if ( !obj ) return null;
		
			return new Tween( obj, prop, Strong.easeOut, begin, finish, duration, true );
		}
	}
}
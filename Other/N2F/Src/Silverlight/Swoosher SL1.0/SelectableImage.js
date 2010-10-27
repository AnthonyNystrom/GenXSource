if (!window.Swoosher_SL_1_0)
	window.Swoosher_SL_1_0 = {};

Swoosher_SL_1_0.SelectableImage = function( plugIn, url, onLoad )
{
	/// <param name="plugIn" type="PlugIn" />
	/// <param name="onLoad" type="Function" />
	/// <param name="createNameScope" type="Boolean" />

	this.url = url;

	UserControl.load( plugIn, "SelectableImage.xaml", true, Silverlight.createDelegate( this, function( sender, args )
	{
		if ( onLoad ) onLoad( sender, args );
		this.handleLoad( plugIn, null, args.xaml );
	} ) );
}

Swoosher_SL_1_0.SelectableImage.prototype =
{
	url: null,
	selectable: null,
	downloader: null,
	pressed: false,
	dimmed: false,
	last: null,
	click: null,
	loaded: null,
	failed: null,
	progress: null,

	dispose: function()
	{
	},

	handleLoad: function( plugIn, userContext, rootElement )
	{
		this.selectable = rootElement.findName( "selectableContent" );

		var progressContainer = rootElement.FindName( "progressContainer" );

		this.progress = new Swoosher_SL_1_0.ProgressBar( plugIn, Silverlight.createDelegate( this, function( sender, args )
		{
			progressContainer.Children.Add( args.xaml );
		} ) );

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
		getSize.addEventListener( "Completed", Silverlight.createDelegate( this, this.getSize_Completed ) );
	},
	
	BeginStoryboard: function( name )
	{
		this.selectable.Resources.FindName( name ).Begin();
	},

	StopStoryboard: function( name )
	{
		this.selectable.Resources.FindName( name ).Stop();
	},

	OnLoaded: function( sender, e )
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
	},

	ViewDistance: function() { return viewDistance; },

	image_DownloadProgressChanged: function( sender, e )
	{
		if ( this.progress.getProgress() == 0 ) this.BeginStoryboard( "showProgress" );
		this.progress.setProgress( this.downloader.DownloadProgress );
	},

	image_Completed: function( sender, e )
	{
		this.selectable.FindName( "image" ).SetSource( sender, "" );
		this.BeginStoryboard( "getSize" );
	},

	image_Failed: function( sender, e )
	{
		if ( this.failed != null ) this.failed( this, e );
	},

	getSize_Completed: function( sender, e )
	{
		var image = this.selectable.FindName( "image" );

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
		}
	},

	SetSize: function( image, width, height )
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
	},

	SetVisualSize: function( name, left, top, width, height )
	{
		var visual = this.selectable.FindName( name );

		visual.SetValue( "Canvas.Left", left );
		visual.SetValue( "Canvas.Top", top );
		visual.Width = width;
		visual.Height = height;
	},

	/// <summary>
	/// Mouse enter
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	image_MouseEnter: function( sender, e )
	{
		setTimeout( Silverlight.createDelegate( this, function()
		{
			this.BeginStoryboard( "enter" );
			this.BeginStoryboard( "highlightContent" );
		} ), 0 );
	},

	/// <summary>
	/// Mouse leave
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	image_MouseLeave: function( sender, e )
	{
		this.pressed = false;

		setTimeout( Silverlight.createDelegate( this, function()
		{
			this.BeginStoryboard( "leave" );
			if ( this.dimmed ) this.BeginStoryboard( "dimContent" );
		} ), 0 );
	},

	/// <summary>
	/// Mouse left button down
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	image_MouseLeftButtonDown: function( sender, e )
	{
		this.pressed = true;
		this.last = e.GetPosition( sender );
	},

	/// <summary>
	/// Mouse left button up
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	image_MouseLeftButtonUp: function( sender, e )
	{
		if ( this.pressed && this.click != null ) this.click( this, e );
		this.pressed = false;
	},
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
	webImage_MouseLeftButtonUp: function( sender, e )
	{
//		if ( WebClick != null ) WebClick( this, e );
	},

	GetDimmed: function() { return dimmed; },
	SetDimmed: function( value )
	{
		if ( this.dimmed != value )
		{
			this.dimmed = value;
			if ( this.dimmed ) { this.StopStoryboard( "highlightContent" ); this.BeginStoryboard( "dimContent" ); }
			else { this.StopStoryboard( "dimContent" ); this.BeginStoryboard( "highlightContent" ); }
		}
	}
}
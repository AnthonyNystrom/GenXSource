/// <reference path="intellisense.js" />
/// <reference path="Sylvester/sylvester.js" />
/// <reference path="Quaternion.js" />
/// <reference path="Camera.js" />
/// <reference path="BSPImage.js" />

if (!window.Swoosher_SL_1_0)
	window.Swoosher_SL_1_0 = {};

Swoosher_SL_1_0.Swoosher = function( plugIn, photos, onLoad )
{
	/// <param name="plugIn" type="PlugIn" />
	/// <param name="onLoad" type="Function" />
	/// <param name="createNameScope" type="Boolean" />

	UserControl.load( plugIn, "Swoosher.xaml", false, Silverlight.createDelegate( this, function( sender, args )
	{
		if ( onLoad ) onLoad( sender, args );
		this.handleLoad( plugIn, photos, args.xaml );
	} ) );
}

Swoosher_SL_1_0.Swoosher.prototype =
{
	pending: 0,
	mouseDown: false,
	mouseCaptured: false,
	anchorPosition: null,
	resized: true,
	plugIn: null,
	root: null,
	tree: null,
	camera: null,
	slerp: 0,
	fromZ: 0,
	toZ: 0,
	jumpAngle: 0,
	timer: null,
	photoCanvas: null,
	refresh: true,
	resized: true,
	backButton: null,
	forwardButton: null,
	fullscreenButton: null,
	windowedButton: null,
	loading: null,
	firefoxTime: 0,
	history: [],
	selectedIndex: 0,

	dispose: function()
	{
		root = null;
	},

	handleLoad: function( plugIn, photos, rootElement )
	{
		this.plugIn = plugIn;

		this.root = rootElement;
		this.photoCanvas = rootElement.findName( "photoCanvas" );

		var loadingContainer = rootElement.FindName( "loadingContainer" );

		this.loading = new Swoosher_SL_1_0.Loading( plugIn, Silverlight.createDelegate( this, function( sender, args )
		{
			loadingContainer.Children.Add( args.xaml );
		} ) );

		var buttons = rootElement.FindName( "buttonContainer" );
		this.addArrow( buttons, "backButton", 0, 180, Silverlight.createDelegate( this, this.back_Click ) );
		this.addArrow( buttons, "forwardButton", 1, 0, Silverlight.createDelegate( this, this.forward_Click ) );
		this.addArrow( buttons, "fullscreenButton", 2, 270, Silverlight.createDelegate( this, this.fullscreen_Click ) );
		this.addArrow( buttons, "windowedButton", 2, 90, Silverlight.createDelegate( this, this.window_Click ) );

		this.root.addEventListener( "MouseLeftButtonDown", Silverlight.createDelegate( this, this.canvas_MouseButtonDown ) );
		this.root.addEventListener( "MouseLeftButtonUp", Silverlight.createDelegate( this, this.canvas_MouseButtonUp ) );
		this.root.addEventListener( "MouseMove", Silverlight.createDelegate( this, this.canvas_MouseMove ) );
		this.root.addEventListener( "MouseLeave", Silverlight.createDelegate( this, this.canvas_MouseLeave ) );

		this.camera = new Camera();
		this.camera.position = $V( [ 0, 0, -10 ] );
		this.camera.lookDirection = $V( [ 0, 0, 1 ] );
		this.camera.near = 1;
		this.camera.far = 1000;
		this.camera.fov = 60;

		this.timer = this.root.Resources.FindName( "timer" );
		this.timer.addEventListener( "Completed", Silverlight.createDelegate( this, this.timer_Tick ) );

		this.tree = new BSPTree( null, this.preparePhotos( photos ), null );

		this.timer.begin();
	},

	addArrow: function( parent, name, x, angle, onClick )
	{
		var arrow = new Swoosher_SL_1_0.Arrow( this.plugIn, x, angle, Silverlight.createDelegate( this, function( sender, args )
		{
			arrow.click = onClick;
			this[ name ] = args.xaml;
			this.root.FindName( "buttonContainer" ).Children.Add( args.xaml );
		} ) );
	},

	preparePhotos: function( photos )
	{
		var items = [];
		var count = photos.length;
		var size = Math.ceil( Math.pow( count, 1.0 / 3 ) );
		// TODO: Properly split BSP items in tree if they cross planes; then don't need to force even-valued sizez
		if ( ( size & 1 ) == 1 ) ++size;
		var offset = -( ( size - 1 ) / 2 );
		var origin = $V( [ offset, offset, offset ] );

		this.pending = 0;
		var positions = [];

		for ( var index in photos )
		{
			var photo = photos[ index ];
			var point;
			var key;

			do
			{
				point = $V( [
					Math.floor( Math.random() * size ),
					Math.floor( Math.random() * size ),
					Math.floor( Math.random() * size ) ] );

				key = point.inspect();
			}
			while ( positions[ key ] );

			positions[ key ] = photo;

			var image = new BSPImage( this.plugIn, point.add( origin ).x( 1.5 ), photo.url );
			image.itemClick = Silverlight.createDelegate( this, this.itemClick );
			image.itemLoaded = Silverlight.createDelegate( this, function( sender, args )
				{ if ( -- this.pending < 1 ) this.loading.stop(); } );
			image.itemFailed = Silverlight.createDelegate( this, function( sender, args )
				{ if ( -- this.pending < 1 ) this.loading.stop(); } );
//			image.addEventListener( "WebClick", Silverlight.createDelegate( this, ItemWebClick ) );

			items.push( image );
		}

		this.pending = items.length;

		return items;
	},

	timer_Tick: function( sender, e )
	{
		var content = this.plugIn.content;

		if ( this.tree != null && content.actualWidth > 10 && content.actualHeight > 10 )
		{
			var animated = this.ApplyAnimation();

			var state = { reordered: false };
			var images = this.Reorder( state );

			if ( animated || state.reordered || this.resized ) this.Refresh( images );
		}

		this.timer.begin();
	},

	ApplyAnimation: function()
	{
		if ( this.slerp >= 0.005 )
		{
			this.camera.rotation = Quaternion.Slerp( this.from, this.to, 1 - this.slerp, true );
			this.camera.upDirection = Camera.Transform( this.camera.rotationMatrix(), $V( [ 0, 1, 0 ] ) );
			this.slerp *= 0.8;

			var z = this.fromZ + ( ( this.toZ - this.fromZ ) * ( 1 - this.slerp ) )
				- ( Math.sin( ( 1 - this.slerp ) * Math.PI ) * this.jumpAngle / 22 );
			this.camera.position = $V( [ 0, 0, z ] );

			return true;
		}

		return false;
	},

	Reorder: function( state )
	{
		var viewpoint = Camera.Transform( this.camera.rotationMatrix(), this.camera.position );

		// CJC - 2008/01/07 - Changed to modify the ZIndex property instead.
		// (previously read it didn't work; turns out it does, but with a couple issues)

		var items = this.tree.select( viewpoint );

		for ( var index = 0; index < items.length; ++ index )
		{
			var item = items[ index ];
			var visual = item.visual;

			if ( visual != null )
			{
				if ( !item.rendered )
				{
					this.photoCanvas.Children.Add( visual );
					item.rendered = true;
				}
				else
				{
					if ( visual.GetValue( "Canvas.ZIndex" ) != index )
					{
						visual.SetValue( "Canvas.ZIndex", index );
						state.reordered = true;
					}
				}
			}
		}

		return items;
	},

	Refresh: function( items )
	{
		this.refresh = false;

		var content = this.plugIn.content;
		var width = content.actualWidth;
		var height = content.actualHeight;

		if ( this.resized && width > 0 && height > 0 )
		{
			this.resized = false;
			this.resize();

			this.camera.aspectRatio = width / height;
		}

		var display = this.camera.displayMatrix( width, height );
		var viewpoint = Camera.Transform( this.camera.rotationMatrix(), this.camera.position );
		var maxDistance = 0.5;
		var frustum = this.camera.frustum();

		for ( var index in items )
		{
			var item = items[ index ];
			var distance = frustum.distance( item.position );
			var visual = item.visual;

			if ( visual != null )
			{
				if ( distance <= maxDistance && visual != null )
				{
					visual.Opacity = 1 - ( distance / maxDistance );

					var perp = item.position.cross( viewpoint ).toUnitVector();
					var vector = item.position;
					var center = Camera.Transform( display, vector );
					var edge = Camera.Transform( display, vector.add( perp ) );

					var halfWidth = edge.subtract( center ).modulus() / 2;

					var scale = this.plugIn.Content.CreateFromXaml( "<ScaleTransform/>" );
					scale.ScaleX = halfWidth * 2;
					scale.ScaleY = halfWidth * 2;

					var translate = this.plugIn.Content.CreateFromXaml( "<TranslateTransform/>" );
					translate.X = center.elements[0] - halfWidth;
					translate.Y = center.elements[1] - halfWidth;

					var transform = this.plugIn.Content.CreateFromXaml( "<TransformGroup/>" );
					transform.Children.Add( scale );
					transform.Children.Add( translate );

					visual.RenderTransform = transform;
				}
				else
				{
					visual.Opacity = 0;
				}

				visual.IsHitTestVisible = ( visual.Opacity > 0.3 );
			}
		}
	},

	canvas_MouseButtonDown: function( sender, e )
	{
		var content = this.plugIn.content;
		this.mouseDown = true;
		this.anchorPosition = this.ProjectToTrackball( content.actualWidth, content.actualHeight, e.GetPosition( this.root ) );
	},

	canvas_MouseMove: function( sender, e )
	{
		var now = new Date().valueOf();

		if ( now > this.firefoxTime )
		{
			// To avoid Firefox's "busy script" we need to handle some events in a setTimeout
			// Nasty hack here uses a timeout every 2 seconds
			setTimeout( Silverlight.createDelegate( this, function() { this.onMouseMove( sender, e ); } ), 0 );
			this.firefoxTime = now + 2000;
		}
		else this.onMouseMove( sender, e );
	},

	onMouseMove: function( sender, e )
	{
		if ( this.mouseDown )
		{
			if ( !this.mouseCaptured )
			{
				this.root.CaptureMouse();
				this.mouseCaptured = true;
			}

			var content = this.plugIn.content;
			var position = this.ProjectToTrackball( content.actualWidth, content.actualHeight, e.GetPosition( this.root ) );

			var axis = this.anchorPosition.cross( position );
			var angle = Vector.AngleBetween( this.anchorPosition, position );

			if ( axis.elements[0] != 0 || axis.elements[1] != 0 || axis.elements[2] != 0 )
			{
				axis.elements[1] *= -1;
				var delta = $Q( axis, angle );

				this.camera.rotation = this.camera.rotation.multiply( delta );
				this.camera.upDirection = Camera.Transform( this.camera.rotationMatrix(), $V( [ 0, 1, 0 ] ) );
			}

			this.anchorPosition = position.dup();
			this.resized = true;
		}
	},

	canvas_MouseButtonUp: function( sender, e )
	{
		this.ReleaseMouse();
		this.mouseDown = false;
	},

	canvas_MouseLeave: function( sender, e )
	{
		this.ReleaseMouse();
		this.mouseDown = false;
	},

	itemClick: function( sender, e )
	{
		if ( !this.mouseCaptured )
		{
			if ( this.selected != sender && ( this.history.length < 1 || this.history[ this.selectedIndex ] != sender ) )
			{
				// Clear history from current point on
				this.history.splice( this.selectedIndex + 1, this.history.length - this.selectedIndex );

				// Append to history
				this.history.push( sender );
				if ( this.history.length > 100 ) this.history.splice( 0, 1 );
				this.selectedIndex = this.history.length - 1;
				this.refreshButtons();
			}

			// Select item
			this.selectItem( sender );
		}
	},

	selectItem: function( image )
	{
		this.fromZ = this.camera.position.elements[2];
		this.from = this.camera.rotation.normalize();

		if ( this.selected == image )
		{
			image.selected = false;
			image.refreshState();
			this.selected = null;
			this.to = this.from;
			this.toZ = -10;
			this.slerp = 1;
			this.jumpAngle = 0;
		}
		else
		{
			if ( this.selected != null )
			{
				var oldImage = this.selected;
				oldImage.selected = false;
				oldImage.refreshState();
			}

			image.selected = true;
			image.visited = true;
			image.refreshState();
			this.selected = image;

			var center = image.position;
			var dir = center.toUnitVector();

			var viewpoint = center.add( dir.multiply( image.viewDistance ) );

			var axis = this.camera.position.cross( viewpoint ).toUnitVector();
			var angle = Vector.AngleBetween( this.camera.position, viewpoint );

			this.to = ( axis.elements[0] != 0 || axis.elements[1] != 0 || axis.elements[2] != 0 )
				? $Q( axis, angle )
				: $Q( $V( [ 0, 1, 0 ] ), angle );

			this.to = this.to.normalize();

			this.toZ = -viewpoint.modulus();

			this.slerp = 1;

			this.jumpAngle = Vector.AngleBetween(
				Camera.Transform( this.camera.rotationMatrix(), this.camera.position ),
				viewpoint );
		}
	},

	resize: function()
	{
		var content = this.plugIn.content;
		var width = content.actualWidth;
		var height = content.actualHeight;

		if ( width > 0 && height > 0 )
		{
			var shortest = Math.min( width, height );

			this.root.Width = width;
			this.root.Height = height;

			this.refreshButtons();

			if ( this.loading.visual && this.loading.visual.Visibility == "Visible" )
			{
				// Size and center Loading animation
				var loadingSize = shortest * 0.25;
				this.loading.visual.RenderTransform = this.makeTransform(
					loadingSize, ( width - loadingSize ) / 2, ( height - loadingSize ) / 2 );
			}

			var buttonSize = shortest * 0.05;
			var container = this.root.FindName( "buttonContainer" );
			container.RenderTransform = this.makeScale( buttonSize );
		}
	},

	refreshButtons: function()
	{
		var content = this.plugIn.content;

		if ( this.backButton ) this.backButton.Visibility = ( this.selectedIndex < 1 ) ? "Collapsed" : "Visible";
		if ( this.forwardButton ) this.forwardButton.Visibility = ( this.selectedIndex >= this.history.length - 1 ) ? "Collapsed" : "Visible";
		if ( this.fullscreenButton ) this.fullscreenButton.Visibility = content.FullScreen ? "Collapsed" : "Visible";
		if ( this.windowedButton ) this.windowedButton.Visibility = content.FullScreen ? "Visible" : "Collapsed";
	},

	makeScale: function( size )
	{
		var scale = this.plugIn.Content.CreateFromXaml( "<ScaleTransform/>" );
		scale.ScaleX = size;
		scale.ScaleY = size;

		return scale;
	},

	makeTransform: function( size, x, y )
	{
		var scale = this.makeScale( size );

		var translate = this.plugIn.Content.CreateFromXaml( "<TranslateTransform/>" );
		translate.X = x;
		translate.Y = y;

		var transform = this.plugIn.Content.CreateFromXaml( "<TransformGroup/>" );
		transform.Children.Add( scale );
		transform.Children.Add( translate );

		return transform;
	},

	back_Click: function( sender, e )
	{
		if ( this.selectedIndex > 0 )
		{
			this.selectItem( this.history[ -- this.selectedIndex ] );
			this.refreshButtons();
		}
	},

	forward_Click: function( sender, e )
	{
		if ( this.selectedIndex < this.history.length - 1 )
		{
			this.selectItem( this.history[ ++ this.selectedIndex ] );
			this.refreshButtons();
		}
	},

	fullscreen_Click: function( sender, e )
	{
		this.plugIn.content.Fullscreen = true;
	},

	window_Click: function( sender, e )
	{
		this.plugIn.content.Fullscreen = false;
	},

	ReleaseMouse: function()
	{
		if ( this.mouseCaptured )
		{
			this.root.ReleaseMouseCapture();
			this.mouseCaptured = false;
		}
	},

	ProjectToTrackball: function( width, height, point )
	{
		var x = point.X / ( width / 2 );    // Scale so bounds map to [0,0] - [2,2]
		var y = point.Y / ( height / 2 );

		x = x - 1;                           // Translate 0,0 to the center
		y = 1 - y;                           // Flip so +Y is up instead of down

		var z2 = 1 - x * x - y * y;       // z^2 = 1 - x^2 - y^2
		var z = z2 > 0 ? Math.sqrt( z2 ) : 0;

		return new $V( [ x, y, z ] );
	}
}
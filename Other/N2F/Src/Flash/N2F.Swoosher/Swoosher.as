package
{
	import flash.display.*;
	import flash.events.*;
	import flash.geom.Matrix;
	import flash.geom.Point;

	public class Swoosher extends Sprite
	{
		private var manager : SceneManager;

		public function Swoosher( manager : SceneManager )
		{
			this.manager = manager;
			manager.itemClick = this.itemClick;
//			var items = preparePhotos( photos );
//			tree = new BSPTree( null, items, 10, null );

			this.camera = new Camera();
			this.camera.position = Vector.create( [ 0, 0, -20 ] );
			this.camera.lookDirection = Vector.create( [ 0, 0, 1 ] );
			this.camera.near = 1;
			this.camera.far = 1000;
			this.camera.fov = 60;

			var me : Swoosher = this;

			addEventListener( Event.ENTER_FRAME, onEnterFrame );
		}

		private function onEnterFrame( event : Event )
		{
			if ( this.manager != null && stage.stageWidth > 10 && stage.stageHeight > 10 )
			{
				var animated = this.ApplyAnimation();

				var state = { reordered: false };
				var images = this.Reorder( state );

				if ( animated || state.reordered || this.resized ) this.Refresh( images );
			}
/*
			var ordered : Array = [];
			tree.SelectItems( Vector.create( [ 0, 0, -5 ] ), ordered );

			for ( var index in ordered )
			{
				var item : BSPImage = ordered[ index ];
				var m : flash.geom.Matrix = new flash.geom.Matrix();
				m.translate( -50, -50 );
				m.scale( 2, 2 );
				m.translate(
					( stage.stageWidth / 2 ) + ( 100 * item.position.elements[ 0 ] ),
					( stage.stageHeight / 2 ) + ( 100 * item.position.elements[ 1 ] ) );
				item.image.transform.matrix = m;
			}*/
		}

		public var pending : Number = 0;
		public var mouseDown : Boolean = false;
		public var mouseCaptured : Boolean = false;
		public var anchorPosition = null;
		public var resized : Boolean = true;
		public var camera : Camera = null;
		public var slerp : Number = 0;
		public var from;
		public var to;
		public var fromZ : Number = 0;
		public var toZ : Number = 0;
		public var jumpAngle : Number = 0;
		public var timer = null;
		public var photoCanvas = null;
		public var refresh = true;
		public var backButton = null;
		public var forwardButton = null;
		public var fullscreenButton = null;
		public var windowedButton = null;
		public var loading = null;
		public var firefoxTime = 0;
		public var history : Array = [];
		public var selectedIndex = 0;
		public var selected = null;
		public var dragged : Boolean = false;
/*
		public function dispose()
		{
			root = null;
		}

		public function handleLoad( photos, rootElement )
		{
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
		}

		public function addArrow( parent, name, x, angle, onClick )
		{
			var arrow = new Swoosher_SL_1_0.Arrow( this.plugIn, x, angle, Silverlight.createDelegate( this, function( sender, args )
			{
				arrow.click = onClick;
				this[ name ] = args.xaml;
				this.root.FindName( "buttonContainer" ).Children.Add( args.xaml );
			} ) );
		}

		public function preparePhotos( photos )
		{
			var items = [];
			var count = photos.length;
			var size = Math.ceil( Math.pow( count, 1.0 / 3 ) );
			// TODO: Properly split BSP items in tree if they cross planes; then don't need to force even-valued sizez
			if ( ( size & 1 ) == 1 ) ++size;
			var offset = -( ( size - 1 ) / 2 );
			var origin = Vector.create( [ offset, offset, offset ] );

			this.pending = 0;
			var positions = [];

			for ( var index in photos )
			{
				var photo = photos[ index ];
				var point;
				var key;

				do
				{
					point = Vector.create( [
						Math.floor( Math.random() * size ),
						Math.floor( Math.random() * size ),
						Math.floor( Math.random() * size ) ] );

					key = point.inspect();
				}
				while ( positions[ key ] );

				positions[ key ] = photo;

				var me : Swoosher = this;
				var image = new BSPImage( point.add( origin ).x( 1.5 ), photo.url );
				image.itemClick = function( sender, e : Event ) { me.itemClick( sender, e ); };
				image.itemLoaded = function() { if ( -- me.pending < 1 ) me.loading.stop(); };
				image.itemFailed = function() { if ( -- me.pending < 1 ) me.loading.stop(); };
	//			image.addEventListener( "WebClick", Silverlight.createDelegate( this, ItemWebClick ) );

				items.push( image );
			}

			this.pending = items.length;

			return items;
		}
/*
		public function timer_Tick( sender, e )
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
		}
*/
		public function ApplyAnimation()
		{
			if ( this.slerp >= 0.005 )
			{
				this.camera.rotation = Quaternion.Slerp( this.from, this.to, 1 - this.slerp, true );
				this.camera.upDirection = Camera.Transform( this.camera.rotationMatrix(), Vector.create( [ 0, 1, 0 ] ) );
				this.slerp *= 0.8;

				var z = this.fromZ + ( ( this.toZ - this.fromZ ) * ( 1 - this.slerp ) )
					- ( Math.sin( ( 1 - this.slerp ) * Math.PI ) * this.jumpAngle / 22 );
				this.camera.position = Vector.create( [ 0, 0, z ] );

				return true;
			}

			return false;
		}

		public function Reorder( state )
		{
			var viewpoint = Camera.Transform( this.camera.rotationMatrix(), this.camera.position );

			// CJC - 2008/01/07 - Changed to modify the ZIndex property instead.
			// (previously read it didn't work; turns out it does, but with a couple issues)

			var items = this.manager.tree.SelectItems( viewpoint );
			var visualIndex : int = 0;

			for ( var index = 0; index < items.length; ++ index )
			{
				var item : BSPImage = items[ index ];
				var visual : SelectableImage = item.image;

				if ( visual != null )
				{
					if ( item.depth == -1 )
					{
						addChild( visual );
					}
					else
					{
						if ( item.depth != visualIndex )
						{
							setChildIndex( visual, visualIndex );
							state.reordered = true;
						}
					}

					item.depth = visualIndex;
					++ visualIndex;
				}
			}

			return items;
		}

		public function Refresh( items )
		{
			this.refresh = false;

			var width = stage.stageWidth;
			var height = stage.stageHeight;

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
				var visual = item.image;

				if ( visual != null )
				{
					if ( distance <= maxDistance && visual != null )
					{
						visual.alpha = 1 - ( distance / maxDistance );

						var perp = item.position.cross( viewpoint ).toUnitVector();
						var vector = item.position;
						var center = Camera.Transform( display, vector );
						var edge = Camera.Transform( display, vector.add( perp ) );

						var halfWidth = edge.subtract( center ).modulus() / 2;

						var m : flash.geom.Matrix = new flash.geom.Matrix();
//						m.translate( -50, -50 );
						m.scale( halfWidth * 2 / 100, halfWidth * 2 / 100 );
						m.translate(
							center.elements[ 0 ] - halfWidth,
							center.elements[ 1 ] - halfWidth );

						item.image.transform.matrix = m;
					}
					else
					{
						visual.alpha = 0;
					}

					visual.mouseEnabled = visual.mouseChildren = ( visual.alpha > 0.3 );
				}
			}
		}

		private function GlobalMouse( e : MouseEvent ) : Point
		{
			return e.target.localToGlobal( new Point( e.localX, e.localY ) );
		}

		public function canvas_MouseButtonDown( sender, e )
		{
			this.mouseDown = true;
			this.dragged = false;
			this.anchorPosition = this.ProjectToTrackball( stage.stageWidth, stage.stageHeight, GlobalMouse( e ) );
		}

		public function canvas_MouseMove( sender, e )
		{
			if ( this.mouseDown )
			{/*
				if ( !this.mouseCaptured )
				{
					this.root.CaptureMouse();
					this.mouseCaptured = true;
				}*/

				var position = this.ProjectToTrackball( stage.stageWidth, stage.stageHeight, GlobalMouse( e ) );

				var axis = this.anchorPosition.cross( position );
				var angle = Vector.AngleBetween( this.anchorPosition, position );

				if ( axis.elements[0] != 0 || axis.elements[1] != 0 || axis.elements[2] != 0 )
				{
					this.dragged = true;
					axis.elements[1] *= -1;
					var delta = Quaternion.create( axis, angle );

					this.camera.rotation = this.camera.rotation.multiply( delta );
					this.camera.upDirection = Camera.Transform( this.camera.rotationMatrix(), Vector.create( [ 0, 1, 0 ] ) );
				}

				this.anchorPosition = position.dup();
				this.resized = true;
			}
		}

		public function canvas_MouseButtonUp( sender, e )
		{

//			this.ReleaseMouse();
			this.mouseDown = false;
//			if ( !this.dragged ) e.target.dispatchEvent( new MouseEvent( MouseEvent.CLICK, e ) );
		}

		public function canvas_MouseLeave( sender, e )
		{
//			this.ReleaseMouse();
			this.mouseDown = false;
		}

		public function itemClick( sender, e )
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
		}

		public function selectItem( image )
		{
			this.fromZ = this.camera.position.elements[2];
			this.from = this.camera.rotation.normalize();

			if ( this.selected == image )
			{
				image.selected = false;
				image.refreshState();
				this.selected = null;
				this.to = this.from;
				this.toZ = -20;
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

				var axis = this.camera.position.cross( center ).toUnitVector();
				var angle = Vector.AngleBetween( this.camera.position, center );

				this.to = ( axis.elements[0] != 0 || axis.elements[1] != 0 || axis.elements[2] != 0 )
					? Quaternion.create( axis, angle )
					: Quaternion.create( Vector.create( [ 0, 1, 0 ] ), angle );

				this.to = this.to.normalize();

				var tc : Camera = new Camera();
				tc.position = Vector.create( [ 0, 0, -2 ] );
				tc.rotation = to;

				var corner = GetImageCorner( image, tc );
				var display = tc.displayMatrix( stage.stageWidth, stage.stageHeight );
				var frustum = tc.frustum();
				this.toZ = tc.position.elements[ 2 ] - 0.2 - frustum.distance( corner );

//				this.toZ = tc.position.elements[ 2 ]; // -viewpoint.modulus();

				this.slerp = 1;

				this.jumpAngle = Vector.AngleBetween(
					Camera.Transform( this.camera.rotationMatrix(), this.camera.position ),
					tc.position /*viewpoint*/ );
			}
		}

		private function GetImageCorner( item : BSPImage, tc : Camera ) : Vector
		{
			var vm = tc.viewMatrix();
			var aspect : Number = item.image.AspectRatio;
			var iWidth = ( aspect >= 0 ) ? 1 : aspect;
			var iHeight = ( aspect > 0 ) ? 1 / aspect : 1;
			var offset : Vector = Vector.create( [ -iWidth / 2, -iHeight / 2, 0 ] );
			var origin : Vector = Camera.Transform( vm, item.position ).add( offset );

			vm = vm.inverse();
			return Camera.Transform( vm, origin );
		}

		public function resize()
		{
/*			var width = stage.stageWidth;
			var height = stage.stageHeight;

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
			}*/
		}

		public function refreshButtons()
		{
			/*
			var content = this.plugIn.content;

			if ( this.backButton ) this.backButton.Visibility = ( this.selectedIndex < 1 ) ? "Collapsed" : "Visible";
			if ( this.forwardButton ) this.forwardButton.Visibility = ( this.selectedIndex >= this.history.length - 1 ) ? "Collapsed" : "Visible";
			if ( this.fullscreenButton ) this.fullscreenButton.Visibility = content.FullScreen ? "Collapsed" : "Visible";
			if ( this.windowedButton ) this.windowedButton.Visibility = content.FullScreen ? "Visible" : "Collapsed";
			*/
		}
/*
		public function makeScale( size )
		{
			var scale = this.plugIn.Content.CreateFromXaml( "<ScaleTransform/>" );
			scale.ScaleX = size;
			scale.ScaleY = size;

			return scale;
		}

		public function makeTransform( size, x, y )
		{
			var scale = this.makeScale( size );

			var translate = this.plugIn.Content.CreateFromXaml( "<TranslateTransform/>" );
			translate.X = x;
			translate.Y = y;

			var transform = this.plugIn.Content.CreateFromXaml( "<TransformGroup/>" );
			transform.Children.Add( scale );
			transform.Children.Add( translate );

			return transform;
		}

		public function back_Click( sender, e )
		{
			if ( this.selectedIndex > 0 )
			{
				this.selectItem( this.history[ -- this.selectedIndex ] );
				this.refreshButtons();
			}
		}

		public function forward_Click( sender, e )
		{
			if ( this.selectedIndex < this.history.length - 1 )
			{
				this.selectItem( this.history[ ++ this.selectedIndex ] );
				this.refreshButtons();
			}
		}

		public function fullscreen_Click( sender, e )
		{
			this.plugIn.content.Fullscreen = true;
		}

		public function window_Click( sender, e )
		{
			this.plugIn.content.Fullscreen = false;
		}

		public function ReleaseMouse()
		{
			if ( this.mouseCaptured )
			{
				this.root.ReleaseMouseCapture();
				this.mouseCaptured = false;
			}
		}
*/
		public function ProjectToTrackball( width, height, point )
		{
			var x = point.x / ( width / 2 );    // Scale so bounds map to [0,0] - [2,2]
			var y = point.y / ( height / 2 );

			x = x - 1;                           // Translate 0,0 to the center
			y = 1 - y;                           // Flip so +Y is up instead of down

			var z2 = 1 - x * x - y * y;       // z^2 = 1 - x^2 - y^2
			var z = z2 > 0 ? Math.sqrt( z2 ) : 0;

			return Vector.create( [ x, y, z ] );
		}
	}
}
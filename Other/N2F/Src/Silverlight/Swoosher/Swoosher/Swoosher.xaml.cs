using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Next2Friends.Swoosher.Media3D;
using System.Threading;
using System.Windows.Interop;

namespace Next2Friends.Swoosher
{
	public class Swoosher : Control
	{
		public event Func<string, bool> Expand;
		public event EventHandler Fullscreen;
		public event EventHandler Windowed;

		private Canvas root;
		private Storyboard timer;
		private Canvas photoCanvas;
		private Loading loading;
		private Arrow backButton;
		private Arrow forwardButton;
		private Arrow fullscreenButton;
		private Arrow windowedButton;
		private Arrow physicsButton;

		private BSPTree<BSPImage> tree;
		private IEnumerable<string> items;
		private bool needsRefresh = true;
		private IBSPItem selected = null;
		private List<BSPImage> previousOrder = new List<BSPImage>();
		private bool mouseDown = false;
		private bool mouseCaptured = false;
		private Vector3D anchorPosition;

		private List<BSPImage> browseOrder;
		private int selectedIndex = -1;

		private Physics physics = null;
		private Quaternion from;
		private Quaternion to;
		private double fromZ;
		private double toZ;
		private double jumpAngle;
		private double slerp = 0;
		private bool refresh = true;
		private int pending = 0;

		private Camera camera = new Camera()
		{
			Position = new Point3D( 0, 0, -10 ),
			LookDirection = new Vector3D( 0, 0, 1 ),
			Near = 1,
			Far = 1000,
			Fov = 60
		};

		public Swoosher()
		{
			System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream( "Next2Friends.Swoosher.Swoosher.xaml" );
			root = (Canvas)this.InitializeFromXaml( new System.IO.StreamReader( s ).ReadToEnd() );

			timer = (Storyboard)root.Resources.FindName( "timer" );
			timer.Completed += OnTick;

			loading = (Loading)root.FindName( "loading" );
			photoCanvas = (Canvas)root.FindName( "photoCanvas" );

			backButton = (Arrow)root.FindName( "backButton" );
			forwardButton = (Arrow)root.FindName( "forwardButton" );
			fullscreenButton = (Arrow)root.FindName( "fullscreenButton" );
			windowedButton = (Arrow)root.FindName( "windowedButton" );
			physicsButton = (Arrow)root.FindName( "physicsButton" );

			Loaded += OnLoaded;
		}

		public IEnumerable<string> Items
		{
			get { return items; }
			set
			{
				items = value.ToList();
				tree = new BSPTree<BSPImage>( PreparePhotos( items ) );
				browseOrder = new List<BSPImage>( tree.Select() );
			}
		}

		public void OnLoaded( object sender, EventArgs e )
		{
			timer.Begin();

			if ( Expand != null ) Expand( "MGM0MjQ5MzY2Y2EyNDI4Yz" );
		}

		private ICollection<BSPImage> PreparePhotos( IEnumerable<string> urls )
		{
			var items = new List<BSPImage>();
			var count = urls.Count();
			var size = (int)Math.Ceiling( Math.Pow( count, 1.0 / 3 ) );
			// TODO: Properly split BSP items in tree if they cross planes; then don't need to force even-valued sizez
			if ( ( size & 1 ) == 1 ) ++size;
			var offset = -( ( (double)size - 1 ) / 2 );
			var origin = new Vector3D( offset, offset, offset );

			pending = 0;
			var positions = new Dictionary<string, string>();
			var rand = new Random();

			foreach ( var url in urls )
			{
				Point3D point;
				string key;

				do
				{
					point = new Point3D(
						rand.Next( size ),
						rand.Next( size ),
						rand.Next( size ) );

					key = point.X + "," + point.Y + "," + point.Z;
				}
				while ( positions.ContainsKey( key ) );

				positions[ key ] = url;

				var image = new BSPImage( point + origin, new Uri( url ) );
				image.Loaded += delegate { if ( Interlocked.Decrement( ref pending ) < 1 ) loading.Stop(); };
				image.Failed += delegate { if ( Interlocked.Decrement( ref pending ) < 1 ) loading.Stop(); };
				image.Click += ItemClick;
				image.WebClick += ItemWebClick;

				items.Add( image );
			}

			pending = items.Count;

			return items;
		}

		public void OnTick( object sender, EventArgs e )
		{
			if ( tree != null )
			{
				var viewpoint = camera.GetRotationMatrix().Transform( camera.Position );
				var items = tree.Select( viewpoint ).ToArray();

				var animated = ApplyAnimation();
				var reordered = Reorder( items );

				if ( animated || reordered || needsRefresh ) Refresh( items, viewpoint );
			}

			timer.Begin();
		}

		private bool ApplyAnimation()
		{
			if ( slerp >= 0.005 )
			{
				camera.Rotation = Quaternion.Slerp( from, to, 1 - slerp );
				camera.UpDirection = camera.GetRotationMatrix().Transform( new Vector3D( 0, 1, 0 ) );
				slerp *= 0.8f;

				var z = fromZ + ( ( toZ - fromZ ) * ( 1 - slerp ) )
					- ( Math.Sin( ( 1 - slerp ) * Math.PI ) * jumpAngle / 22 );
				camera.Position = new Point3D( 0, 0, z );

				return true;
			}

			return false;
		}

		private bool Reorder( IEnumerable<BSPImage> items )
		{
			var changed = false;

			if ( physics != null )
			{
				changed |= physics.Apply( items.Cast<IPhysicsItem>(), camera );
			}

			var index = 0;

			foreach ( var item in items )
			{
				if ( physics == null && ( !item.Transform.IsIdentity || item.Angle2D != 0 ) )
				{
					item.LinearVelocity = new Vector3D();
					item.AngularVelocity2D = 0;
					item.AtRest = false;

					var m = RewindMatrix( item.Transform, 0.8 );

					item.Transform = ( Math.Abs( m.OffsetX ) + Math.Abs( m.OffsetY ) + Math.Abs( m.OffsetZ ) ) > 0.1
						? m
						: Matrix3D.Identity;

					item.Angle2D = ( Math.Abs( item.Angle2D ) > 0.01 )
						? item.Angle2D * 0.8
						: 0;

					changed = true;
				}

				if ( !item.Rendered )
				{
					photoCanvas.Children.Add( item.Visual );
					item.Rendered = true;
					changed = true;
				}

				if ( !item.Visual.GetValue( Canvas.ZIndexProperty ).Equals( index ) )
				{
					item.Visual.SetValue( Canvas.ZIndexProperty, index );
					changed = true;
				}

				++index;
			}

			return changed;
		}

		private void Refresh( IEnumerable<BSPImage> items, Point3D viewpoint )
		{
			refresh = false;

			if ( needsRefresh )
			{
				if ( Width > 0 && Height > 0 ) camera.AspectRatio = Width / Height;

				needsRefresh = false;
			}

			var display = camera.CreateDisplayMatrix( Width, Height );
			var maxDistance = 0.5;

			foreach ( var item in items )
			{
				var distance = camera.GetFrustum().Distance( item.Position );
				var visual = item.Visual;

				if ( distance <= maxDistance )
				{
					visual.Opacity = 1 - ( distance / maxDistance );

					Vector3D perp = Vector3D.CrossProduct( (Vector3D)item.Position, (Vector3D)viewpoint );
					perp.Normalize();

					var m = item.Transform;
					var translate = new Vector3D( m.OffsetX, m.OffsetY, m.OffsetZ );

					var points = new Point3D[]
					{
						(Point3D)item.Position + translate,
						(Point3D)( item.Position + translate + perp )
					};

					display.Transform( ref points );

					var halfWidth = ( points[ 1 ] - points[ 0 ] ).Length / 2;
					var transform = new TransformGroup();

					transform.Children.Add( new TranslateTransform
					{
						X = -0.5,
						Y = -0.5
					} );

					transform.Children.Add( new RotateTransform
					{
						Angle = item.Angle2D
					} );

					transform.Children.Add( new ScaleTransform
					{
						ScaleX = halfWidth * 2,
						ScaleY = halfWidth * 2
					} );

					transform.Children.Add( new TranslateTransform
					{
						X = points[ 0 ].X,
						Y = points[ 0 ].Y
					} );

					visual.RenderTransform = transform;
				}
				else
				{
					visual.Opacity = 0;
				}

				visual.IsHitTestVisible = ( visual.Opacity > 0.05 );
			}
		}

		private SelectableImage AddPhoto()
		{
			var photo = new SelectableImage();
			photoCanvas.Children.Add( photo );
			return photo;
		}

		public double Width
		{
			get { return base.Width; }
			set { base.Width = root.Width = value; Resize(); }
		}

		public double Height
		{
			get { return base.Height; }
			set { base.Height = root.Height = value; Resize(); }
		}

		private void Resize()
		{
			needsRefresh = true;

			if ( Width > 0 && Height > 0 )
			{
				var shortest = Math.Min( Width, Height );

				if ( loading.Visibility == Visibility.Visible )
				{
					// Size and center Loading animation
					var loadingSize = shortest * 0.25;
					loading.RenderTransform = MakeTransform( loadingSize, ( Width - loadingSize ) / 2, ( Height - loadingSize ) / 2 );
				}

				var buttonSize = shortest * 0.05;
				backButton.RenderTransform = MakeTransform( buttonSize, 0, 0 );
				forwardButton.RenderTransform = MakeTransform( buttonSize, buttonSize, 0 );
				fullscreenButton.RenderTransform = MakeTransform( buttonSize, buttonSize * 2, 0 );
				windowedButton.RenderTransform = MakeTransform( buttonSize, buttonSize * 2, 0 );

				physicsButton.RenderTransform = MakeTransform( buttonSize, buttonSize * 4, 0 );

				fullscreenButton.Visibility = BrowserHost.IsFullScreen ? Visibility.Collapsed : Visibility.Visible;
				windowedButton.Visibility = BrowserHost.IsFullScreen ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		private TransformGroup MakeTransform( double size, double x, double y )
		{
			var scale = new ScaleTransform { ScaleX = size, ScaleY = size };
			var translate = new TranslateTransform { X = x, Y = y };

			var transform = new TransformGroup();
			transform.Children.Add( scale );
			transform.Children.Add( translate );

			return transform;
		}

		private void ItemClick( object sender, EventArgs e )
		{
			if ( !mouseCaptured )
			{
				fromZ = camera.Position.Z;
				from = camera.Rotation;
				from.Normalize();

				SelectItem( (BSPImage)sender );
			}
		}

		private void SelectItem( BSPImage image )
		{
			if ( selected == image )
			{
				image.Selected = false;
				image.RefreshState();
				selected = null;
				to = from;
				fromZ = camera.Position.Z;
				toZ = -10;
				slerp = 1;
				jumpAngle = 0;
			}
			else
			{
				if ( selected != null )
				{
					var oldImage = (BSPImage)selected;
					oldImage.Selected = false;
					oldImage.RefreshState();
				}

				image.Selected = true;
				image.Visited = true;
				image.RefreshState();
				selected = image;

				Point3D center = image.Position;
				Vector3D dir = (Vector3D)center;
				dir.Normalize();

				Point3D viewpoint = center + ( dir * image.ViewDistance );

				var axis = Vector3D.CrossProduct( (Vector3D)camera.Position, (Vector3D)viewpoint );
				var angle = Vector3D.AngleBetween( (Vector3D)camera.Position, (Vector3D)viewpoint );

				to = ( axis != new Vector3D() )
					? new Quaternion( axis, angle )
					: new Quaternion( new Vector3D( 0, 1, 0 ), angle );

				to.Normalize();

				toZ = -( (Vector3D)viewpoint ).Length;

				slerp = 1;
				jumpAngle = Vector3D.AngleBetween(
					(Vector3D)camera.GetRotationMatrix().Transform( camera.Position ),
					(Vector3D)viewpoint );
			}
		}

		private void ItemWebClick( object sender, EventArgs e )
		{
			var image = (BSPImage)sender;
			System.Windows.Browser.HtmlPage.Navigate( image.Source.ToString(), "_blank" );
		}

		private void canvas_MouseButtonDown( object sender, MouseEventArgs e )
		{
			mouseDown = true;
			anchorPosition = Trackball.ProjectToTrackball( Width, Height, e.GetPosition( root ) );
		}

		private void canvas_MouseMove( object sender, MouseEventArgs e )
		{
			if ( mouseDown /*&& physics == null*/ )
			{
				if ( !mouseCaptured )
				{
					root.CaptureMouse();
					mouseCaptured = true;
				}

				Vector3D position = Trackball.ProjectToTrackball( Width, Height, e.GetPosition( root ) );

				Vector3D axis = Vector3D.CrossProduct( anchorPosition, position );
				double angle = Vector3D.AngleBetween( anchorPosition, position );

				if ( axis != new Vector3D() )
				{
					axis.Y *= -1;
					Quaternion delta = new Quaternion( axis, angle );

					camera.Rotation *= delta;
					camera.UpDirection = camera.GetRotationMatrix().Transform( new Vector3D( 0, 1, 0 ) );
				}

				anchorPosition = position;
				needsRefresh = true;
			}
		}

		private void canvas_MouseButtonUp( object sender, MouseEventArgs e )
		{
			ReleaseMouse();
			mouseDown = false;
		}

		private void canvas_MouseLeave( object sender, EventArgs e )
		{
			ReleaseMouse();
			mouseDown = false;
		}

		private void ReleaseMouse()
		{
			if ( mouseCaptured )
			{
				root.ReleaseMouseCapture();
				mouseCaptured = false;
			}
		}

		private void back_Click( object sender, EventArgs e )
		{
			if ( --selectedIndex < 0 ) selectedIndex = browseOrder.Count() - 1;
			SelectItem( browseOrder[ selectedIndex ] );
		}

		private void forward_Click( object sender, EventArgs e )
		{
			if ( ++selectedIndex >= browseOrder.Count() ) selectedIndex = 0;
			SelectItem( browseOrder[ selectedIndex ] );
		}

		private void fullscreen_Click( object sender, EventArgs e )
		{
			if ( Fullscreen != null ) Fullscreen( this, e );
		}

		private void window_Click( object sender, EventArgs e )
		{
			if ( Windowed != null ) Windowed( this, e );
		}

		private void physics_Click( object sender, EventArgs e )
		{
			if ( physics == null )
			{
				var rotation = camera.GetRotationMatrix();
				var direction = (Vector3D)rotation.Transform( camera.Position );
				direction.Normalize();

				physics = new Physics( direction * 0.1 );
				var rand = new Random();

				foreach ( var item in tree.Select() )
				{
					item.AngularVelocity2D = ( rand.NextDouble() * 2 ) - 1;
				}
			}
			else physics = null;
		}

		private Matrix3D RewindMatrix( Matrix3D m, double scale )
		{
			return new Matrix3D(
				( ( m.M11 - 1.0 ) * scale ) + 1.0,
				m.M12 * scale,
				m.M13 * scale,
				m.M14 * scale,
				m.M21 * scale,
				( ( m.M22 - 1.0 ) * scale ) + 1.0,
				m.M23 * scale,
				m.M24 * scale,
				m.M31 * scale,
				m.M32 * scale,
				( ( m.M33 - 1.0 ) * scale ) + 1.0,
				m.M34 * scale,
				m.OffsetX * scale,
				m.OffsetY * scale,
				m.OffsetZ * scale,
				( ( m.M44 - 1.0 ) * scale ) + 1.0 );
		}
	}
}
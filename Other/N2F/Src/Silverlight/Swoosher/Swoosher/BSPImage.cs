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
	public class BSPImage : IBSPItem, IPhysicsItem
	{
		public event EventHandler Loaded;
		public event EventHandler Failed;
		public event EventHandler Click;
		public event EventHandler WebClick;

		private Point3D position;
		private Uri source;
		private SelectableImage visual;
		private bool selected;
		private bool visited;
		private Matrix3D transform = Matrix3D.Identity;

		public BSPImage( Point3D position, Uri source )
		{
			// TODO: This hack is to ensure no point is zero.  To fix this properly, fully define
			// cubes in the BSP tree and implement 'splitting' when they cross planes.
			this.position = new Point3D(
				position.X != 0 ? position.X : 0.001,
				position.Y != 0 ? position.Y : 0.001,
				position.Z != 0 ? position.Z : 0.001 );

			this.source = source;
		}

		public Point3D Position { get { return position; } }
		public Matrix3D Transform { get { return transform; } set { transform = value; } }

		public Vector3D LinearVelocity { get; set; }
		public double Angle2D { get; set; }
		public double AngularVelocity2D { get; set; }
		public bool AtRest { get; set; }

		public Uri Source { get { return source; } }
		public double ViewDistance { get { return Visual.ViewDistance; } }
		public bool Rendered { get; set; }

		public Size Size { get { return visual.Size; } }

		public bool Selected
		{
			get { return selected; }
			set { selected = value; }
		}

		public bool Visited
		{
			get { return visited; }
			set { visited = value; }
		}

		public void RefreshState()
		{
			Visual.Dimmed = ( Visited && !Selected );
		}

		public SelectableImage Visual
		{
			get
			{
				if ( visual == null )
				{
					visual = new SelectableImage( source );
					visual.ImageLoaded += delegate( object s, EventArgs e ) { if ( Loaded != null ) Loaded( this, e ); };
					visual.Failed += delegate( object s, EventArgs e ) { if ( Failed != null ) Failed( this, e ); };
					visual.Click += delegate( object s, EventArgs e ) { if ( Click != null ) Click( this, e ); };
					visual.WebClick += delegate( object s, EventArgs e ) { if ( WebClick != null ) WebClick( this, e ); };
				}

				return visual;
			}
		}

		#region IBSPItem Members

		public PlaneIntersectionType Intersects( Plane plane, out IBSPItem frontItem, out IBSPItem backItem )
		{
			frontItem = null;
			backItem = null;

			switch ( plane.Classify( position ) )
			{
				case Halfspace.Negative: return PlaneIntersectionType.Back;
				case Halfspace.Positive: return PlaneIntersectionType.Front;
			}

			return PlaneIntersectionType.Intersecting;
		}

		public IBounds Bounds
		{
			get
			{
				Vector3D offset = new Vector3D( -0.5, -0.5, -0.5 );
				return new BoundingBox( position - offset, position + offset );
			}
		}

		#endregion
	}
}
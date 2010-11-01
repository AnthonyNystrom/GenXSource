using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Collections.Specialized;

namespace Genetibase.FactCube
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>

	public partial class FactCube : System.Windows.Controls.UserControl, INotifyPropertyChanged
	{
        public static DependencyProperty CubeBrushProperty = DependencyProperty.Register(
            "CubeBrush", typeof( Brush ), typeof( FactCube ) );

        public static DependencyProperty FlatBrushProperty = DependencyProperty.Register(
            "FlatBrush", typeof( Brush ), typeof( FactCube ) );

        public static DependencyProperty ShowCubesProperty = DependencyProperty.Register(
            "ShowCubes", typeof( bool ), typeof( FactCube ), new PropertyMetadata( new PropertyChangedCallback( Option_Changed ) ) );

		public static DependencyProperty LimitDepthProperty = DependencyProperty.Register(
			"LimitDepth", typeof( bool ), typeof( FactCube ), new PropertyMetadata( new PropertyChangedCallback( Option_Changed ) ) );

        public static DependencyProperty ClipFadeProperty = DependencyProperty.Register(
            "ClipFade", typeof( bool ), typeof( FactCube ), new PropertyMetadata( new PropertyChangedCallback( Option_Changed ) ) );

		public static DependencyProperty EmptyImagesProperty = DependencyProperty.Register(
			"EmptyImages", typeof( bool ), typeof( FactCube ), new PropertyMetadata( new PropertyChangedCallback( Option_Changed ) ) );

		public static DependencyProperty DistanceProperty = DependencyProperty.Register(
			"Distance", typeof( double ), typeof( FactCube ), new PropertyMetadata( new PropertyChangedCallback( Option_Changed ) ) );

		private BSPTree tree;
		private Trackball trackball = new Trackball();
		private bool cameraChanged = true;
        private int dimensionSize;
		private ThumbnailManager thumbnails = new ThumbnailManager( 128, 100 );

		/// <summary>
		/// Default constructor
		/// </summary>
		public FactCube()
		{
			InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler( FactCube_DataContextChanged );
		}

        /// <summary>
        /// Gets or sets cube brush
        /// </summary>
        public Material CubeBrush
        {
            get { return GetValue( CubeBrushProperty ) as Material; }
            set { SetValue( CubeBrushProperty, value ); }
        }

        /// <summary>
        /// Gets or sets flat brush
        /// </summary>
        public Brush FlatBrush
        {
            get { return GetValue( FlatBrushProperty ) as Brush; }
            set { SetValue( FlatBrushProperty, value ); }
        }

        /// <summary>
        /// Gets or sets ShowCubes value
        /// </summary>
        public bool ShowCubes
        {
            get { return (bool)GetValue( ShowCubesProperty ); }
            set { SetValue( ShowCubesProperty, value ); }
        }

		/// <summary>
		/// Gets or sets LimitDepth value
		/// </summary>
		public bool LimitDepth
		{
			get { return (bool)GetValue( LimitDepthProperty ); }
			set { SetValue( LimitDepthProperty, value ); }
		}

        /// <summary>
        /// Gets or sets whether items intersecting a clipping plane should be faded or removed
        /// </summary>
        public bool ClipFade
        {
            get { return (bool)GetValue( ClipFadeProperty ); }
            set { SetValue( ClipFadeProperty, value ); }
        }

		/// <summary>
		/// Gets or sets whether empty images should be included
		/// </summary>
		public bool EmptyImages
		{
			get { return (bool)GetValue( EmptyImagesProperty ); }
			set { SetValue( EmptyImagesProperty, value ); }
		}

		/// <summary>
		/// Gets or sets whether empty images should be included
		/// </summary>
		public double Distance
		{
			get { return (double)GetValue( DistanceProperty ); }
			set { SetValue( DistanceProperty, value ); }
		}

        /// <summary>
        /// DataContext changed handler
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        void FactCube_DataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            if ( DataContext is Facts ) BuildScene();
            cameraChanged = true;
        }

        static void Option_Changed( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            ( obj as FactCube ).cameraChanged = true;
        }

        void camera_Changed( object sender, EventArgs e )
        {
            cameraChanged = true;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
			camera.Transform = trackball.Transform;
			light.Transform = trackball.Transform;

			BuildScene();

            // Viewport3Ds only raise events when the mouse is over the rendered 3D geometry.
            // In order to capture events whenever the mouse is over the client are we use a
            // same sized transparent Border positioned on top of the Viewport3D.
			trackball.EventSource = captureBorder;

			captureBorder.Focus();

			CompositionTarget.Rendering += new EventHandler( CompositionTarget_Rendering );
			camera.Changed += new EventHandler( camera_Changed );
		}

		/// <summary>
		/// Build the scene
		/// </summary>
		private void BuildScene()
		{
            tree = null;
            Facts facts = DataContext as Facts;

            dimensionSize = ( facts != null )
                ? Math.Max( Math.Max( facts.What.Count, facts.Where.Count ), facts.When.Count )
                : 8;

            double scale = ( 1d / ( dimensionSize - 1 ) ) * 0.9;

            Matrix3D scaleMatrix = new Matrix3D();
            scaleMatrix.Scale( new Vector3D( scale, scale, scale ) );

            Random rand = new Random();
            MeshGeometry3D cubeMesh = Resources[ "cubeMesh" ] as MeshGeometry3D;

            List<IBSPItem> items = new List<IBSPItem>();

            for ( int whatIndex = 0; whatIndex < dimensionSize; ++whatIndex )
            {
                double x = ( (double)whatIndex / ( dimensionSize - 1 ) ) - 0.5d;

                for ( int whereIndex = 0; whereIndex < dimensionSize; ++whereIndex )
                {
                    double y = ( (double)whereIndex / ( dimensionSize - 1 ) ) - 0.5d;

                    for ( int whenIndex = 0; whenIndex < dimensionSize; ++whenIndex )
                    {
                        double z = ( (double)whenIndex / ( dimensionSize - 1 ) ) - 0.5d;
                        bool valid = true;
                        double materialOffset;
						Facts.FactRow fact = null;

                        if ( facts != null )
                        {
                            valid = ( whatIndex < facts.What.Count )
                                && ( whereIndex < facts.Where.Count )
                                & ( whenIndex < facts.When.Count );

							if ( valid )
							{
								fact = facts.Fact.FindByWhatWhereWhen(
									facts.What[ whatIndex ].ID,
									facts.Where[ whereIndex ].ID,
									facts.When[ whenIndex ].ID );

								valid = ( fact != null );
							}

                            materialOffset = valid ? 0.4 : 0.55;
                        }
                        else
                        {
							double alpha = ( 0.4 + ( rand.NextDouble() * 0.6 ) );

                            // Negative for red, positive for blue
                            materialOffset = ( ( rand.Next( 5 ) == 2 ) ? -alpha : alpha ) * 0.5 + 0.5;
                        }

                        Matrix3D matrix = scaleMatrix;
                        matrix.Translate( new Vector3D( x, y, z ) );

						string uri = ( fact != null ) ? fact.ThumbnailUrl : null;

                        items.Add( new BSPCube( matrix, materialOffset, cubeMesh, uri ) );
                    }
                }
            }

            tree = new BSPTree( items, new BSPTree.PartitionHandler( TreePartition ) );
		}

		/// <summary>
		/// Determine partition
		/// </summary>
		/// <param name="items">Items</param>
		/// <param name="bounds">Bounds</param>
		/// <returns>Partition</returns>
		protected double TreePartition( double length, Vector3D normal )
		{
			double cubeSize = 1d / ( dimensionSize - 1 );
			length += ( cubeSize * 0.1 );	// Add cube padding
			return Math.Round( length / cubeSize / 2, MidpointRounding.AwayFromZero ) * cubeSize / length;
		}

		private void FPSCamera( Vector3D distance )
		{
			trackball.Translation.OffsetX += distance.X;
			trackball.Translation.OffsetY += distance.Y;
			trackball.Translation.OffsetZ += distance.Z;
		}

		/// <summary>
		/// Rendering
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		void CompositionTarget_Rendering( object sender, EventArgs e )
		{
            Quaternion q = Quaternion.Identity;

            if ( Keyboard.IsKeyDown( Key.Left ) ) q *= new Quaternion( new Vector3D( 0, 3, 0 ), 5 );
            if ( Keyboard.IsKeyDown( Key.Right ) ) q *= new Quaternion( new Vector3D( 0, 3, 0 ), -5 );
            if ( Keyboard.IsKeyDown( Key.Up ) ) q *= new Quaternion( new Vector3D( 3, 0, 0 ), 5 ); ;
            if ( Keyboard.IsKeyDown( Key.Down ) ) q *= new Quaternion( new Vector3D( 3, 0, 0 ), -5 );
            /*

                        Vector3D l = camera.Transform.Transform( camera.LookDirection );
                        Matrix3D rotateLeft = new Matrix3D();
                        rotateLeft.Rotate( new Quaternion( new Vector3D( 0, 3, 0 ), 90 ) );

                        if ( Keyboard.IsKeyDown( Key.Up ) ) FPSCamera( l * 0.05 );
                        if ( Keyboard.IsKeyDown( Key.Down ) ) FPSCamera( -l * 0.05 );
                        if ( Keyboard.IsKeyDown( Key.Left ) ) FPSCamera( rotateLeft.Transform( l ) * 0.05 );
                        if ( Keyboard.IsKeyDown( Key.Right ) ) FPSCamera( -rotateLeft.Transform( l ) * 0.05 );
            */

            if ( !q.IsIdentity )
            {
                AxisAngleRotation3D rotation = trackball.Rotation;
                Quaternion newQ = new Quaternion( rotation.Axis, rotation.Angle ) * q;

                rotation.Axis = newQ.Axis;
                rotation.Angle = newQ.Angle;
            }

			if ( cameraChanged && tree != null )
			{
				cameraChanged = false;

				MatrixCamera matrixCamera = FactCubeCamera.CreateMatrixCamera( camera, ActualWidth, ActualHeight, 0.01, 3 );
				Matrix3D viewProjectionMatrix = matrixCamera.ViewMatrix * matrixCamera.ProjectionMatrix;

				Frustum frustum = new Frustum( viewProjectionMatrix, true );
                DepthPlanes depthPlanes = new DepthPlanes( camera, tree.Bounds, 1d / ( dimensionSize - 1 ) );

                Point3D cameraPos = camera.Transform.Transform( camera.Position );

                List<IBSPItem> items = new List<IBSPItem>();
                RenderTree( tree, items, cameraPos, frustum, depthPlanes );

				Matrix3D displayMatrix = viewProjectionMatrix
					* FactCubeCamera.GetHomogeneousToViewportTransform3D( ActualWidth, ActualHeight );

                //canvas.Children.Clear();
                Render3D( tree, items, cameraPos, displayMatrix, frustum, depthPlanes );
			}
		}

        /// <summary>
        /// Render 3D scene
        /// </summary>
        /// <param name="tree">BSP tree</param>
        /// <param name="camera">Camera</param>
        /// <param name="frustum">View frustum</param>
        /// <param name="depthPlanes">Depth planes</param>
        private void Render3D( BSPTree tree, IList<IBSPItem> items, Point3D cameraPos, Matrix3D displayMatrix, Frustum frustum, DepthPlanes depthPlanes )
        {
			bool clipFade = ClipFade;
			bool showCubes = ShowCubes;
			bool limitDepth = LimitDepth;

			MeshGeometry3D thumbnailMesh = new MeshGeometry3D();
			MeshGeometry3D mesh = new MeshGeometry3D();
			//MeshGeometry3D flatMesh = new MeshGeometry3D();

//			List<BSPCube> renderItems = new List<BSPCube>();

            foreach ( BSPCube cube in items )
            {
                ContainmentType type = frustum.Contains( cube.Bounds );

                if ( type != ContainmentType.Disjoint
                    && ( clipFade || type != ContainmentType.Intersects )
                    && ( !limitDepth || depthPlanes.Includes( cube.Position ) ) )
                {
					double alpha = ( type == ContainmentType.Intersects ) ? 0.2 : 1.0;

					if ( !string.IsNullOrEmpty( cube.Url ) || EmptyImages )
					{
						cube.RenderThumbnail( thumbnailMesh, cameraPos, alpha, displayMatrix, thumbnails );
					}

					if ( showCubes ) cube.Render( mesh, cameraPos, alpha );
					//cube.RenderFlat( flatMesh, cameraPos, displayMatrix, new Rect( 0, 0, 0.5, 1 ) );
				}
            }

            geometry.Geometry = mesh;
			thumbnailGeometry.Geometry = thumbnailMesh;
			//flatGeometry.Geometry = flatMesh;
		}

/*        /// <summary>
        /// Render 2D scene
        /// </summary>
        /// <param name="tree">BSP tree</param>
        /// <param name="camera">Camera</param>
        /// <param name="frustum">View frustum</param>
        /// <param name="depthPlanes">Depth planes</param>
        private void Render2D( BSPTree tree, IList<IBSPItem> items, Point3D cameraPos, Matrix3D displayMatrix, Frustum frustum, DepthPlanes depthPlanes )
        {
            int index = 0;
            bool clipFade = ClipFade;

            foreach ( BSPCube cube in items )
            {
                ContainmentType type = frustum.Contains( cube.Bounds );

                if ( type != ContainmentType.Disjoint
                    && ( clipFade || type != ContainmentType.Intersects )
                    && depthPlanes.Includes( cube.Position ) )
                {
                    Sphere sphere;

                    if ( index < canvas.Children.Count ) sphere = canvas.Children[ index ] as Sphere;
                    else canvas.Children.Add( sphere = new Sphere() );

                    sphere.Visibility = Visibility.Visible;
                    sphere.RenderTransform = new MatrixTransform( cube.SphereMatrix( cameraPos, displayMatrix ) );
                    sphere.Brush = ( cube.MaterialOffset >= 0.5 ) ? SphereBrush : SelectedSphereBrush;
                    sphere.Opacity = Math.Abs( 0.5 - cube.MaterialOffset ) * 2;

                    if ( type == ContainmentType.Intersects ) sphere.Opacity *= 0.2;

                    ++index;
                }
            }

            while ( index < canvas.Children.Count ) canvas.Children[ index++ ].Visibility = Visibility.Hidden;
        }
*/
		/// <summary>
		/// Recursively render tree
		/// </summary>
		/// <param name="tree">Tree node</param>
		private void RenderTree( BSPTree tree, IList<IBSPItem> items, Point3D cameraPos, Frustum frustum, DepthPlanes depthPlanes )
		{
			bool limitDepth = LimitDepth;

			if ( tree != null && frustum.Contains( tree.Bounds ) != ContainmentType.Disjoint
				&& ( !limitDepth || depthPlanes.Includes( tree.Bounds ) ) )
			{
				Halfspace halfspace = tree.Partition.Classify( cameraPos );

                if ( halfspace == Halfspace.Negative ) { if ( tree.Front != null ) RenderTree( tree.Front, items, cameraPos, frustum, depthPlanes ); }
                else if ( tree.Back != null ) RenderTree( tree.Back, items, cameraPos, frustum, depthPlanes );

				foreach ( IBSPItem item in tree.Items ) items.Add( item );

                if ( halfspace == Halfspace.Negative ) { if ( tree.Back != null ) RenderTree( tree.Back, items, cameraPos, frustum, depthPlanes ); }
                else if ( tree.Front != null ) RenderTree( tree.Front, items, cameraPos, frustum, depthPlanes );
			}
		}

		/// <summary>
		/// Get thumbnail images
		/// </summary>
		public ReadOnlyObservableCollection<string> Thumbnails { get { return thumbnails.Images; } }

        /// <summary>
        /// Raise property changed event
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged( string propertyName )
        {
            if ( PropertyChanged != null ) PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
package  
{
	import flash.events.ErrorEvent;
	import flash.media.Video;
	import org.papervision3d.core.data.UserData;
	import org.papervision3d.core.geom.renderables.Triangle3D;
	import org.papervision3d.core.geom.renderables.Vertex3D;
	import org.papervision3d.core.geom.TriangleMesh3D;
	import org.papervision3d.core.math.NumberUV;
	import org.papervision3d.core.proto.MaterialObject3D;
	import org.papervision3d.events.FileLoadEvent;
	import org.papervision3d.materials.BitmapMaterial;
	import org.papervision3d.materials.VideoStreamMaterial;
	
	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class VideoMesh extends TriangleMesh3D
	{
		public static var ERROR : String = "ERROR";

//		private var material : VideoStreamMaterial;
		private var vertices : Array;
		private var video : Video;
		private var stream : VideoStream;
		private var attached : Boolean;
		private var f1 : Triangle3D;
		private var f2 : Triangle3D;

		public function VideoMesh( stream : VideoStream )
		{
			this.stream = stream;

			attached = false;
			video = new Video();

			var vm : VideoStreamMaterial = new VideoStreamMaterial( video, stream.Stream );
			vm.doubleSided = true;
			vm.interactive = true;
			vm.invisible = true;
			vm.addEventListener( FileLoadEvent.LOAD_ERROR, OnError );

			vertices = [ new Vertex3D(), new Vertex3D(), new Vertex3D(), new Vertex3D() ];

			super( vm, vertices, [] );

			var v : Array = vertices;
			f1 = new Triangle3D( this, [ v[ 0 ], v[ 1 ], v[ 3 ] ], material, [ new NumberUV( 0, 1 ), new NumberUV( 1, 1 ), new NumberUV( 0, 0 ) ] );
			f2 = new Triangle3D( this, [ v[ 3 ], v[ 1 ], v[ 2 ] ], material, [ new NumberUV( 0, 0 ), new NumberUV( 1, 1 ), new NumberUV( 1, 0 ) ] );

			geometry.faces.push( f1 );
			geometry.faces.push( f2 );

			geometry.ready = true;
		}

		private function OnError( text : String ) : void
		{
			dispatchEvent( new ErrorEvent( ERROR, false, false, text ) );
		}

		public function Attach( item : WallItem ) : void
		{
			if ( !f1.userData || f1.userData.data != item )
			{
				var userData : UserData = new UserData( item );
				f1.userData = f2.userData = userData;

				Move( item.GetSelectionVertices() );
			}
		}

		public function Move( v : Array ) : void
		{
			useOwnContainer = ( filters && ( filters[ 0 ].blurX != 1 || filters[ 0 ].blurY != 1 ) );

			AssignVertex( vertices[ 0 ], v[ 0 ] );
			AssignVertex( vertices[ 1 ], v[ 1 ] );
			AssignVertex( vertices[ 2 ], v[ 2 ] );
			AssignVertex( vertices[ 3 ], v[ 3 ] );
		}

		public function set Filters( filters : Array ) : void
		{
			this.filters = filters;
		}

		private function AssignVertex( current : Vertex3D, v : Vertex3D ) : void
		{
			current.x = v.x;
			current.y = v.y;
			current.z = v.z;
		}

		public function Play( name : String ) : void
		{
			if ( !attached )
			{
				video.attachNetStream( stream.Stream );
				attached = true;
			}

			stream.Play( name );
		}

		public function Pause() : void
		{
			stream.Pause();
		}

		public function Resume() : void
		{
			stream.Resume();
		}

		public function Seek( position : Number ) : void
		{
			Trace( "Starting at: " + position );
			stream.Seek( position );
		}

		public function get Position() : Number
		{
			Trace( "Paused at: " + stream.Stream.time );
			return stream.Stream.time;
		}

		public function set Position( value : Number ) : void
		{
			trace( "Seek to " + value + " of " + stream.Duration );
			stream.Seek( value );
		}

		public function get Duration() : Number
		{
			return stream.Duration;
		}

		public function get Volume() : Number { return stream.Volume; }

		public function set Volume( value : Number ) : void { stream.Volume = value; }

		private function Trace( message : String ) : void
		{
			dispatchEvent( new TraceEvent( message ) );
		}
	}
}
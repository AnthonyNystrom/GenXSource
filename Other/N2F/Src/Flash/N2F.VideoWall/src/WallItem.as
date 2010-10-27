package
{
	import com.chriscavanagh.Silverlayout.Controls.Border;
	import com.chriscavanagh.Silverlayout.Controls.TextBlock;
	import com.chriscavanagh.Silverlayout.Size;
	import flash.display.BitmapData;
	import flash.display.Graphics;
	import flash.display.Scene;
	import flash.display.Sprite;
	import flash.events.ErrorEvent;
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.events.TimerEvent;
	import flash.external.ExternalInterface;
	import flash.filters.BlurFilter;
	import flash.geom.Rectangle;
	import flash.media.Video;
	import flash.net.NetConnection;
	import flash.net.NetStream;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.utils.Timer;
	import fr.seraf.wow.constraint.WBaseConstraint;
	import fr.seraf.wow.core.data.WVector;
	import fr.seraf.wow.core.data.WVertex;
	import org.papervision3d.core.math.Number3D;
	import org.papervision3d.materials.special.CompositeMaterial;

	import org.papervision3d.core.data.UserData;
	import org.papervision3d.core.math.NumberUV;
	import org.papervision3d.core.geom.renderables.Triangle3D;
	import org.papervision3d.core.geom.TriangleMesh3D;
	import org.papervision3d.core.geom.renderables.Vertex3D;
	import org.papervision3d.core.proto.MaterialObject3D;
	import org.papervision3d.events.FileLoadEvent;
	import org.papervision3d.materials.*;
	import org.papervision3d.scenes.Scene3D;

	import fr.seraf.wow.core.WOWEngine;
	import fr.seraf.wow.constraint.WRigidConstraint;
	import fr.seraf.wow.constraint.WSpringConstraint;
	import fr.seraf.wow.constraint.WSpringRangedConstraint;
	import fr.seraf.wow.primitive.WParticle;
	import fr.seraf.wow.primitive.WSphere;

	public class WallItem extends EventDispatcher
	{
		private static var liveTag : BitmapData;
		private static var recordedTag : BitmapData;
		private static var cornerMask : BitmapData;

		private var scene : Scene3D;

		public var tl : WParticle;
		public var tr : WParticle;
		public var br : WParticle;
		public var bl : WParticle;

		private var tlV : Vertex3D;
		private var trV : Vertex3D;
		private var brV : Vertex3D;
		private var blV : Vertex3D;

		private var mesh : TriangleMesh3D;
		public var videoMesh : TriangleMesh3D;
		private var videoMaterial : VideoStreamMaterial;

		public var origin : WVector;
		public var center : WParticle;
		private var pickup : WParticle;
		private var pickupConstraint : WBaseConstraint;
		private var userData : UserData = new UserData( this );

		private var source : ContentSource;

		public function WallItem( wow : WOWEngine, pos : WVector, w : Number, h : Number )
		{
			if ( !liveTag )
			{
				liveTag = CreateSimpleTag( 0xFFFF0000 );
				recordedTag = CreateSimpleTag( 0xFF00FF00 );

				var mask : Sprite = new Sprite();
				var g : Graphics = mask.graphics;
				g.beginFill( 0xFFFFFF );
				g.moveTo( 0, 20 );
				g.lineTo( 0, 0 );
				g.lineTo( 20, 0 );
				g.curveTo( 0, 0, 0, 20 );
				g.endFill();
				cornerMask = new BitmapData( 100, 75, true, 0 );
				cornerMask.draw( mask );
			}

			this.origin = pos;
			var mass : Number = 1;

			var halfW : Number = w / 2;
			var halfH : Number = h / 2;

			wow.addParticle( tl = new WParticle( pos.x - halfW, pos.y + halfH, pos.z, false, mass, 0, 0 ) );
			wow.addParticle( tr = new WParticle( pos.x + halfW, pos.y + halfH, pos.z, false, mass, 0, 0 ) );
			wow.addParticle( br = new WParticle( pos.x + halfW, pos.y - halfH, pos.z, false, mass, 0, 0 ) );
			wow.addParticle( bl = new WParticle( pos.x - halfW, pos.y - halfH, pos.z, false, mass, 0, 0 ) );

			wow.addParticle( center = new WParticle( pos.x, pos.y, pos.z, false ) );

			wow.addParticle( pickup = new WParticle( pos.x, pos.y, pos.z - 1 ) );

			AddConstraints( wow );
		}

		private function CreateSimpleTag( background : int ) : BitmapData
		{
			var sprite : Sprite = new Sprite();
			var g : Graphics = sprite.graphics;
			g.beginFill( background, 1 );
			g.drawCircle( 40, 40, 12 );
			g.endFill();

			var bitmapData : BitmapData = new BitmapData( 400, 300, true, 0x0 );
			bitmapData.draw( sprite );

			return bitmapData;
		}

		private function CreateTextTag( text : String, background : int, foreground : int ) : BitmapData
		{
			var border : Border = new Border( background, 0, 0, 10 );
			border.alpha = 1;
			border.HorizontalAlignment = "Left";
			border.VerticalAlignment = "Top";

			var textBlock : TextBlock = new TextBlock( text, new TextFormat( "Arial", 14, 0xFFFFFF ) );
			textBlock.alpha = 1;
			textBlock.HorizontalAlignment = "Center";
			textBlock.VerticalAlignment = "Center";
			border.addChild( textBlock );

			var bitmapData : BitmapData = new BitmapData( 100, 75, true, 0x0 );
			border.Draw( bitmapData );

			return bitmapData;
		}

		private function AddConstraints( wow : WOWEngine ) : void
		{
			wow.addConstraint( new WRigidConstraint( tl, center ) );
			wow.addConstraint( new WRigidConstraint( tr, center ) );
			wow.addConstraint( new WRigidConstraint( br, center ) );
			wow.addConstraint( new WRigidConstraint( bl, center ) );

			wow.addConstraint( new WRigidConstraint( tl, tr ) );
			wow.addConstraint( new WRigidConstraint( tr, br ) );
			wow.addConstraint( new WRigidConstraint( br, bl ) );
			wow.addConstraint( new WRigidConstraint( bl, tl ) );

			wow.addConstraint( new WSpringRangedConstraint( center, pickup, 0.5, 0, 50 ) );
		}

		public function AddToScene( scene : Scene3D, source : ContentSource ) : void
		{
			this.scene = scene;
			this.source = source;

			tlV = new Vertex3D();
			trV = new Vertex3D();
			brV = new Vertex3D();
			blV = new Vertex3D();

			var material : MaterialObject3D = new BitmapFileMaterial( source.ThumbnailUrl );
			material.addEventListener( FileLoadEvent.LOAD_COMPLETE, OnLoaded );
			material.addEventListener( FileLoadEvent.LOAD_ERROR, OnError );

			var composite : CompositeMaterial = new CompositeMaterial();
			composite.addMaterial( material );

			composite.addMaterial( new BitmapMaterial( source.IsLive ? liveTag : recordedTag ) );

			composite.doubleSided = true;
			composite.interactive = true;

			mesh = CreateRectangleMesh( composite, [ tlV, trV, brV, blV ], userData );
			scene.addChild( mesh );
		}

		private function CreateRectangleMesh( material : MaterialObject3D, v : Array, userData : UserData ) : TriangleMesh3D
		{
			var mesh : TriangleMesh3D = new TriangleMesh3D( material, v, [] );

			var f1 : Triangle3D = new Triangle3D( mesh, [ v[ 0 ], v[ 1 ], v[ 3 ] ], material, [ new NumberUV( 0, 1 ), new NumberUV( 1, 1 ), new NumberUV( 0, 0 ) ] );
			var f2 : Triangle3D = new Triangle3D( mesh, [ v[ 3 ], v[ 1 ], v[ 2 ] ], material, [ new NumberUV( 0, 0 ), new NumberUV( 1, 1 ), new NumberUV( 1, 0 ) ] );
			f1.userData = f2.userData = userData;

			mesh.geometry.faces.push( f1 );
			mesh.geometry.faces.push( f2 );

			mesh.geometry.ready = true;

			return mesh;
		}

		public function Update() : void
		{
			if ( mesh )
			{
				mesh.useOwnContainer = ( mesh.filters && ( mesh.filters[ 0 ].blurX != 1 || mesh.filters[ 0 ].blurY != 1 ) );
			}

			UpdateVertex( tl, tlV );
			UpdateVertex( tr, trV );
			UpdateVertex( br, brV );
			UpdateVertex( bl, blV );
		}

		private function UpdateVertex( p : WParticle, v : Vertex3D ) : void
		{
			if ( p && v )
			{
				var min : Number = 0.1;

				if ( Math.abs( p.px - v.x ) > min || Math.abs( p.py - v.y ) > min || Math.abs( p.pz - v.z ) > min )
				{
					v.x = p.px.valueOf();
					v.y = p.py.valueOf();
					v.z = p.pz.valueOf();
				}
			}
		}

		private function OnLoaded( event : Event ) : void
		{
			dispatchEvent( new Event( Event.COMPLETE ) );
		}

		private function OnError( e : FileLoadEvent ) : void
		{
			dispatchEvent( new ErrorEvent( ErrorEvent.ERROR, false, false, e.message ) );
		}

		public function PickUp() : void
		{
			pickup.pz = origin.z - 80;
		}

		public function Drop( wow : WOWEngine ) : void
		{
			pickup.pz = origin.z;
		}

		public function set Filters( filters : Array ) : void
		{
			mesh.filters = filters;
		}

		public function get Source() :  ContentSource { return source; }

		public function get Mesh() : TriangleMesh3D { return mesh; }

		public function GetSelectionVertices() : Array
		{
			return [ tlV, trV, brV, blV ];
		}
	}
}
/*
 * Copyright 2008 (c) Chris Cavanagh, http://chriscavanagh.wordpress.com
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

package
{
	/* Flash */
	import com.chriscavanagh.Silverlayout.Controls.Border;
	import com.chriscavanagh.Silverlayout.Controls.ContentControl;
	import com.chriscavanagh.Silverlayout.Controls.Image;
	import com.chriscavanagh.Silverlayout.FrameworkElement;
	import com.chriscavanagh.Silverlayout.Size;
	import fl.motion.Tweenables;
	import fl.transitions.easing.Elastic;
	import fl.transitions.easing.Regular;
	import fl.transitions.Fly;
	import fl.transitions.Tween;
	import fl.transitions.TweenEvent;
	import flash.display.Graphics;
	import flash.display.Sprite;
	import flash.display.StageAlign;
	import flash.display.StageQuality;
	import flash.display.StageScaleMode;
	import flash.events.*;
	import flash.external.ExternalInterface;
	import flash.filters.BlurFilter;
	import flash.media.Video;
	import flash.sampler.NewObjectSample;
	import flash.system.Security;
	import flash.utils.Timer;
	import fr.seraf.wow.core.data.WPlane;
	import fr.seraf.wow.primitive.WParticle;
	import fr.seraf.wow.structure.ParticleNode;
	import org.papervision3d.core.data.UserData;
	import org.papervision3d.core.geom.Lines3D;
	import org.papervision3d.core.geom.renderables.Line3D;
	import org.papervision3d.core.geom.renderables.Vertex3D;
	import org.papervision3d.core.geom.renderables.Vertex3DInstance;
	import org.papervision3d.core.math.Matrix3D;
	import org.papervision3d.core.proto.GeometryObject3D;
	import org.papervision3d.core.proto.MaterialObject3D;
	import org.papervision3d.core.render.data.RenderSessionData;
	import org.papervision3d.materials.special.LineMaterial;

	/* PaperVision3D */
	import org.papervision3d.cameras.Camera3D;
	import org.papervision3d.core.math.Number3D;
	import org.papervision3d.core.utils.InteractiveSceneManager;
	import org.papervision3d.events.InteractiveScene3DEvent; 
//	import org.papervision3d.cameras.light.PointLight3D;
	import org.papervision3d.materials.*;
	import org.papervision3d.objects.DisplayObject3D;
	import org.papervision3d.objects.primitives.Plane;
	import org.papervision3d.objects.primitives.Sphere;
	import org.papervision3d.render.BasicRenderEngine;
	import org.papervision3d.scenes.Scene3D;
	import org.papervision3d.view.Viewport3D;

	/* WOW */
	//wow
	import fr.seraf.wow.primitive.WSphere;
	import fr.seraf.wow.constraint.*;
	import fr.seraf.wow.core.WOWEngine;
	import fr.seraf.wow.core.data.WVector;
	import fr.seraf.wow.math.WVectorMath;
	import fr.seraf.wow.primitive.WOWPlane;
	
	public class VideoWall extends Sprite
	{
		private var items : CollectionManager;

		private var container: Sprite;
		private var viewport : Viewport3D;
		private var renderer : BasicRenderEngine;
		private var scene: Scene3D;
		private var camera: Camera3D;
		private var plane:Plane;

		private var wow:WOWEngine;
		private var sphereArray:Array;
		private var imageRows:Array = [];
		private var selectedItem : WallItem;
		private var lastItem : WallItem;
		private var playingItem : WallItem;
		private var lastPlaying : WallItem;
		private var cameraTarget : DisplayObject3D;
		private var targetViewpoint : Number3D;
		private var targetLookat : Number3D;
		private var selection : FrameworkElement;
		private var blurFilter : BlurFilter;

		private var videoMesh : VideoMesh;
		private var videoControls : VideoControls;
		private var active : Boolean = true;
		private var activityTimer : Timer;

		private var traceMethod : String;
		private var collectionManager : CollectionManager;
		private var itemsChanged : Boolean = false;
		private var changeTimer : Timer;
		private var zooming : Boolean;
		private var wasPlaying : Boolean;
		private var cameraTweens : Array = [];
		private var zoomDistance : Number;
		private var browseDistance : Number;
		private var initialized : Boolean = false;
		private var rendered : Boolean = false;

		private var layoutPanel : FrameworkElement;
		private var innerPanel : FrameworkElement;
		private var wallPanel : FrameworkElement;
		private var videoPanel : FrameworkElement;
		private var controlsPanel : Border;
		private var userPanel : Border;
		private var userDetail : UserDetail;

		private var nickname : String;
		private var liveService : LiveService;
		private var noPhysics : Boolean;

		public function VideoWall()
		{
			Security.allowDomain( "*" );
			stage.scaleMode = StageScaleMode.NO_SCALE;
			stage.align = StageAlign.TOP_LEFT;

			var params : * = root.loaderInfo.parameters;
			var initMethod : String = params[ "initMethod" ];
			var selectMethod : String = params[ "selectMethod" ];
			var photoServiceUrl : String = params[ "memberserviceurl" ] ? params[ "memberserviceurl" ] : "http://services.next2friends.com/livewidget/MemberService.asmx?wsdl";
			var server : String = params[ "server" ] ? params[ "server" ] : "rtmp://69.21.114.99/DeviceStream";
			var fToken : String = params[ "ftoken" ] ? params[ "ftoken" ] : null;
			var photoUrl : String = params[ "photo" ] ? params[ "photo" ] : null;
			var embedCode : String = params[ "embedcode" ] ? params[ "embedcode" ] : "<object></object>";
			var directLink : String = params[ "directlink" ] ? params[ "directlink" ] : "http://www.next2friends.com";

			nickname = params[ "nickname" ]; // ? params[ "nickname" ] : "lawrence";
			traceMethod = params[ "traceMethod" ];
			zoomDistance = params[ "zoomDistance" ] ? params[ "zoomDistance" ] : 100;
			browseDistance = params[ "browseDistance" ] ? params[ "browseDistance" ] : 400;
			noPhysics = params[ "noPhysics" ]; // ? params[ "noPhysics" ] : true;

			collectionManager = new CollectionManager( initMethod, selectMethod );
			collectionManager.addEventListener( CollectionManager.CHANGE, OnChanged );

			renderer = new BasicRenderEngine();
			scene = new Scene3D();

			camera = new Camera3D();
			camera.z = -browseDistance;
			//camera.zoom = 4;

			targetViewpoint = new Number3D( camera.x, camera.y, camera.z );
			targetLookat = new Number3D();
			blurFilter = new BlurFilter( 1, 1 );

			SetupWow();
			SetupLayout();
			SetupVideo();
			SetupUserDetail();
			SetupLogo();

			viewport = new Viewport3D( stage.stageWidth, stage.stageHeight, true, true, true, true );
			viewport.interactive = true;
			wallPanel.addChild( viewport );

//			var light : PointLight3D = new PointLight3D( true );

			viewport.interactiveSceneManager.addEventListener( InteractiveScene3DEvent.OBJECT_OVER, OnMouseOver ); 
			viewport.interactiveSceneManager.addEventListener( InteractiveScene3DEvent.OBJECT_OUT, OnMouseOut ); 
			viewport.interactiveSceneManager.addEventListener( InteractiveScene3DEvent.OBJECT_CLICK, OnClick );

//			stage.addEventListener( Event.MOUSE_LEAVE, DropItem );
			stage.addEventListener( Event.RESIZE, OnResize );
			stage.addEventListener( Event.ENTER_FRAME, OnEnterFrame );

			changeTimer = new Timer( 1000 );
			activityTimer = new Timer( 1000 );

			changeTimer.addEventListener( TimerEvent.TIMER, function( event : Event ) : void
			{
				itemsChanged = true;
				initialized = false;
				rendered = false;
				changeTimer.stop();
			} );

			activityTimer.addEventListener( TimerEvent.TIMER, function( event : Event ) : void
			{
				active = false;
				activityTimer.stop();
			} );

			stage.addEventListener( MouseEvent.MOUSE_MOVE, function( event : Event ) : void { StartActivityTimer(); } );

			OnResize( null );

			liveService = new LiveService( photoServiceUrl, null/*videoServiceUrl*/, null/*nickname*/, fToken, Trace );
		}

		 /**
		* initial setup for WOW-Engine.
		*/		
		public function SetupWow(): void
		{
			//this is the physics engine
			wow=new WOWEngine(0.5);
			wow.damping = 0.9;
			//wow.collisionResponseMode = wow.SIMPLE;
			wow.collisionResponseMode = wow.SELECTIVE;
			//setup the gravity
//			wow.addMasslessForce(new WVector(0,-0,0));	// 0,-3,0
		}

		/**
		*  create a ground
		*/		
		public function CreateGround(): void
		{
			//we create a ground on the physics engine
			var ground:WOWPlane = new WOWPlane(0,0,0,90);
			ground.elasticity=0.1;
			ground.friction=0.3;
			wow.addParticle(ground);		
		}

		private function SetupLayout() : void
		{
			layoutPanel = new FrameworkElement();
			addChild( layoutPanel );

			wallPanel = new FrameworkElement();
			layoutPanel.addChild( wallPanel );

			videoPanel = new FrameworkElement();
			layoutPanel.addChild( videoPanel );

			selection = new FrameworkElement();
			selection.Opacity = 0;
			selection.mouseChildren = false;
			selection.mouseEnabled = false;
			layoutPanel.addChild( selection );

			innerPanel = new FrameworkElement();
			innerPanel.Padding = 20;
			layoutPanel.addChild( innerPanel );

			controlsPanel = new Border( 0x000000, 0, 0, 40 );
			controlsPanel.HorizontalAlignment = "Center";
			controlsPanel.BackgroundAlpha = 0.3;
			controlsPanel.Padding = 5;
			controlsPanel.VerticalAlignment = "Bottom";
			controlsPanel.Opacity = 0;
			controlsPanel.Width = 370;
			controlsPanel.mouseChildren = false;
			controlsPanel.mouseEnabled = false;
			controlsPanel.buttonMode = false;
//			controlsPanel.addEventListener( MouseEvent.ROLL_OVER, function( event : Event ) : void { controlsPanel.Opacity = 1; } );
//			controlsPanel.addEventListener( MouseEvent.ROLL_OUT, function( event : Event ) : void { controlsPanel.Opacity = 0; } );
			innerPanel.addChild( controlsPanel );

			videoControls = new VideoControls();
			videoControls.VerticalAlignment = "Center";
			videoControls.addEventListener( VideoControls.PLAY, OnResume );
			videoControls.addEventListener( VideoControls.PAUSE, OnPause );
			videoControls.addEventListener( VideoControls.POSITIONCHANGEBEGIN, OnPositionChangeBegin );
			videoControls.addEventListener( VideoControls.POSITIONCHANGEEND, OnPositionChangeEnd );
			videoControls.addEventListener( VideoControls.POSITIONCHANGED, OnPositionChanged );
			videoControls.addEventListener( VideoControls.VOLUMECHANGED, OnVolumeChanged );
			videoControls.addEventListener( VideoControls.MUTECHANGED, OnMuteChanged );
			controlsPanel.addChild( videoControls );
		}

		private function SetupVideo() : void
		{
			var source : VideoSource = new VideoSource();

			var openStream : Function = function( event : Event ) : void
			{
				var stream : VideoStream = source.CreateStream();
				videoMesh = new VideoMesh( stream );
				videoMesh.filters = [ blurFilter ];
				scene.addChild( videoMesh );

				stream.addEventListener( VideoStream.METADATARECEIVED, function( event : Event ) : void
				{
					if ( playingItem )
					{
						videoMesh.material.invisible = false;
						playingItem.Mesh.material.invisible = true;
					}
				} );

				stream.addEventListener( VideoStream.FINISHED, function( event : Event ) : void
				{
					videoControls.Position = 0;
					videoMesh.Position = 0;
					if ( playingItem ) videoMesh.Play( playingItem.Source.Url );
					OnPause( event );
				} );
			};

			source.addEventListener( TraceEvent.TRACE, OnTrace );
			source.addEventListener( VideoSource.ERROR, function( error : ErrorEvent ) : void { Trace( error.text ); } );
			source.addEventListener( VideoSource.CONNECT, openStream );
			source.Connect();
		}

		private function SetupUserDetail() : void
		{
			userPanel = new Border( 0x000000, 0, 0, 20 );
			userPanel.BackgroundAlpha = 0.3;
			userPanel.VerticalAlignment = "Top";
			userPanel.HorizontalAlignment = "Left";
			userPanel.Padding = 5;
			userPanel.Opacity = 0;
			innerPanel.addChild( userPanel );

			userDetail = new UserDetail();
			userDetail.HorizontalAlignment = "Center";
			userDetail.addEventListener( Event.COMPLETE, function( event : Event ) : void { userPanel.Opacity = 1; } );
			userPanel.addChild( userDetail );
		}

		private function SetupLogo() : void
		{
			var logo : ContentControl = new ContentControl( new N2FLogo() );
			logo.HorizontalAlignment = "Right";
			logo.VerticalAlignment = "Bottom";
			logo.Width = 150;
			innerPanel.addChild( logo );
		}

		private function RefreshWall( items : Array ): void
		{
			ClearWall();

			BitmapFileMaterial.LOADING_COLOR = 0x0000FF;
			BitmapFileMaterial.ERROR_COLOR = 0xFF0000;
			var maxSize : Size = new Size( stage.stageWidth * 0.8, stage.stageHeight * ( noPhysics ? 0.9 : 0.9 ) );
			var layout : * = GetLayout( maxSize, items.length, 4 / 3 );
			var itemSize : Size = layout.Size;

			cameraTarget = new DisplayObject3D( "cameraTarget" );

			var colCount : int = layout.Columns;
			var rowCount : int = layout.Rows;
			var rowHeight : Number = itemSize.Height;
			var colWidth : Number = itemSize.Width;
			var gap : Number = 5;

			var xOffset : Number = -( ( colWidth * ( colCount - 1 ) ) / 2 )
			var yOffset : Number = ( ( rowHeight * ( rowCount - 1 ) ) / 2 );
			var index : int = 0;

			for ( var row : int = 0; row < rowCount; ++ row )
			{
				var yTop : Number = yOffset - ( rowHeight * row );
				var yBottom : Number = yTop - ( ( rowHeight + gap ) / 2 );

				var tlAnchor : WParticle = wow.addParticle( new WParticle( xOffset - 200, yTop, 0, true, 1, 0, 1 ) ).particle;
				var blAnchor : WParticle = wow.addParticle( new WParticle( xOffset - 200, yBottom, 0, true, 1, 0, 1 ) ).particle;

				var item : WallItem;

				imageRows[ row ] = [];

				for ( var col : int = 0; col < colCount; ++ col )
				{
					var source : ContentSource = items[ index ++ ];

					item = new WallItem(
						wow,
						new WVector( xOffset + ( colWidth * col ), yOffset - ( rowHeight * row ), 0 ),
						colWidth - gap,
						rowHeight - gap );

					item.addEventListener( ErrorEvent.ERROR, function( event : ErrorEvent ) : void { Trace( event.text ); } );
					item.addEventListener( Event.COMPLETE, function( event : Event ) : void { StartActivityTimer(); } );

					imageRows[ row ][ col ] = item;

					if ( source )
					{
						item.AddToScene( scene, source );

						item.Filters = [ blurFilter ];
					}

					var left : WallItem = ( col > 0 ) ? imageRows[ row ][ col - 1 ] : null;
					var top : WallItem = ( row > 0 ) ? imageRows[ row - 1 ][ col ] : null;

					if ( left ) AddConstraints( left.tr, item.tl, item.bl, left.br );
					else AddConstraints( tlAnchor, item.tl, item.bl, blAnchor );

					if ( top ) AddConstraints( top.br, item.tr, item.tl, top.bl );
/*					{
						wow.addConstraint( new WRigidConstraint( top.bl, item.tl ) );
						wow.addConstraint( new WRigidConstraint( top.br, item.tr ) );
					}*/
				}

				var trAnchor : WParticle = wow.addParticle( new WParticle( item.tr.px + 200, yTop, 0, true, 1, 0, 1 ) ).particle;
				var brAnchor : WParticle = wow.addParticle( new WParticle( item.tr.px + 200, yBottom, 0, true, 1, 0, 1 ) ).particle;

				AddConstraints( item.tr, trAnchor, brAnchor, item.br );
			}

			var topOffset : WVector = new WVector( 0, 200, 0 );
			var bottomOffset : WVector = new WVector( 0, -200, 0 );

			for ( var anchorCol : int = 0; anchorCol < colCount; ++ anchorCol )
			{
				var topItem : WallItem = imageRows[ 0 ][ anchorCol ] as WallItem;
				var bottomItem : WallItem = imageRows[ rowCount - 1 ][ anchorCol ] as WallItem;

				AddConstraints(
					CreateAnchor( topItem.tr, topOffset ), topItem.tr,
					topItem.tl, CreateAnchor( topItem.tl, topOffset ) );

				AddConstraints(
					bottomItem.br, CreateAnchor( bottomItem.br, bottomOffset ),
					CreateAnchor( bottomItem.bl, bottomOffset ), bottomItem.bl );
			}
		}

		private function ClearWall() : void
		{
			var constraints : Array = wow.getAllConstraints();
			for ( var ci : * in constraints ) wow.removeConstraint( constraints[ ci ] );

			var particles : Array = wow.getAllParticles();
			for ( var pi : * in particles ) wow.removeParticle( particles[ pi ] );

			for ( var name : * in scene.children )
			{
				var child : * = scene.getChildByName( name );
				if ( child != videoMesh ) scene.removeChildByName( name );
			}
		}

		private function GetLayout( availableSize : Size, count : int, aspectRatio : Number ) : *
		{
			var rows : int = 1;
			var columns : int = 1;

			var size : Size = GetScaledSize( availableSize, rows, columns, aspectRatio );

			while ( count > 1 )
			{
				if ( size.Height * ( rows + 1 ) <= availableSize.Height )
				{
					++ rows;
					count -= columns;
				}
				else
				{
					++ columns;
					count -= rows;
				}

				size = GetScaledSize( availableSize, rows, columns, aspectRatio );
			}

			return { Size: size, Rows: rows, Columns: columns };
		}

		private function GetScaledSize( availableSize : Size, rows : int, columns : int, aspectRatio : Number ) : Size
		{
			var maxSize : Size = new Size( availableSize.Width / columns, availableSize.Height / rows );
			var height : Number = maxSize.Width / aspectRatio;
			var scale : Number = Math.min( 1, maxSize.Height / height );

			return new Size( maxSize.Width * scale, height * scale );
		}

		private function AddConstraints( tl : WParticle, tr : WParticle, br : WParticle, bl : WParticle ) : void
		{
			wow.addConstraint( new WRigidConstraint( tl, tr ) );
			wow.addConstraint( new WRigidConstraint( bl, br ) );
//			wow.addConstraint( new WRigidConstraint( tl, br ) );
//			wow.addConstraint( new WRigidConstraint( bl, tr ) );
		}

		private function CreateAnchor( p : WParticle, offset : WVector ) : WParticle
		{
			var pos : WVector = WVectorMath.addVector( p.position, offset );
			var anchor : WSphere = new WSphere( pos.x, pos.y, pos.z, 1, true );

			wow.addParticle( anchor );

			return anchor;
		}

		private function SetCameraTarget( position : Number3D, onComplete : Function = null ) : void
		{
			if ( cameraTweens.length > 0 )
			{
				for ( var index : * in cameraTweens )
				{
					var tween : Tween = cameraTweens[ index ];
					tween.stop();
				}
			}

			var removeTween : Function = function( tween : Tween ) : void
			{
				var index : Number = cameraTweens.indexOf( tween );
				if ( index >= 0 && index < cameraTweens.length ) cameraTweens.splice( index, 1 );
				if ( cameraTweens.length < 1 && onComplete != null ) onComplete( new Event( Event.COMPLETE ) );
			}

			cameraTweens = [
				CreateTween( camera, "x", camera.x, position.x, 0.5, removeTween ),
				CreateTween( camera, "y", camera.y, position.y, 0.5, removeTween ),
				CreateTween( camera, "z", camera.z, position.z, 0.5, removeTween )
			];
		}

		private function CreateTween( obj : * , prop : String, begin : Number, finish : Number, duration : Number, listener : Function = null ) : Tween
		{
			var tween : Tween = new Tween( obj, prop, Regular.easeIn, begin, finish, duration, true );
			var onFinish : Function;

			if ( listener != null )
			{
				onFinish = function( event : Event ) : void
				{
					tween.removeEventListener( TweenEvent.MOTION_FINISH, onFinish );
					listener( tween );
				};

				tween.addEventListener( TweenEvent.MOTION_FINISH, onFinish );
			}

			return tween;
		}

		private function OnEnterFrame( event: Event ): void
		{
			if ( itemsChanged )
			{
				itemsChanged = false;
				RefreshWall( collectionManager.Items );
				lastItem = null;
				initialized = true;
			}

			if ( initialized )
			{
				// Update the physics engine
				if ( !noPhysics || !rendered )
				{
					wow.step();

					for ( var row : * in imageRows )
					{
						var columns : Array = imageRows[ row ];

						for ( var col : * in columns )
						{
							columns[ col ].Update();
						}
					}
				}

				if ( lastPlaying ) videoMesh.Attach( lastPlaying );

				if ( selectedItem )
				{
					if ( lastItem != selectedItem ) lastItem = selectedItem;
				}

				// Render the scene
				if ( active || playingItem ) renderer.renderScene( scene, camera, viewport );

				if ( active && lastItem != null )
				{
					var targetVertices : Array = lastItem.GetSelectionVertices();

					var xOffset : Number = viewport.viewportWidth / 2;
					var yOffset : Number = viewport.viewportHeight / 2;

					var g : Graphics = selection.graphics;
					g.clear();

					if ( !playingItem )
					{
						g.lineStyle( 5, 0xA0A0C0, 0.8, true );
						g.moveTo( targetVertices[ 0 ].vertex3DInstance.x + xOffset, targetVertices[ 0 ].vertex3DInstance.y + yOffset );
						g.lineTo( targetVertices[ 1 ].vertex3DInstance.x + xOffset, targetVertices[ 1 ].vertex3DInstance.y + yOffset );
						g.lineTo( targetVertices[ 2 ].vertex3DInstance.x + xOffset, targetVertices[ 2 ].vertex3DInstance.y + yOffset );
						g.lineTo( targetVertices[ 3 ].vertex3DInstance.x + xOffset, targetVertices[ 3 ].vertex3DInstance.y + yOffset );
						g.lineTo( targetVertices[ 0 ].vertex3DInstance.x + xOffset, targetVertices[ 0 ].vertex3DInstance.y + yOffset );
					}
				}

				if ( videoMesh && videoControls && playingItem && videoControls.IsPlaying )
				{
	//				Trace( "Position: " + videoMesh.Position + " of " + videoMesh.Duration + " (" + videoMesh.Position / videoMesh.Duration + ")" );
					if ( !isNaN( videoMesh.Duration ) )
					{
						videoControls.Position = videoMesh.Position / videoMesh.Duration;
						videoControls.SetText( ToTime( videoMesh.Position ) + " / " + ToTime( videoMesh.Duration ) );
					}
				}

				rendered = true;
			}
		}

		private function ToTime( seconds : Number ) : String
		{
			return !isNaN( seconds )
				? Math.floor( seconds / 60 ) + ":" + ZeroPad( Math.floor( seconds % 60 ), 2 )
				: "";
		}

		private function ZeroPad( value : Number, length : int ) : String
		{
			var padding : String = "000";
			var str : String = String( value );

			return ( str.length < length )
				? padding.substring( 0, length - str.length ) + str
				: str;
		}

		private function OnMouseOver( e : InteractiveScene3DEvent ) : void
		{
			var item : WallItem = e.face3d ? e.face3d.userData.data as WallItem : null;

			if ( playingItem == null && item != selectedItem && item != null )
			{
				if ( !noPhysics )
				{
					DropItem( e );
					item.PickUp();
				}

				selectedItem = item;

				userPanel.Opacity = 0;

				try
				{
					liveService.GetProfilePhoto( OnPhotoResponse, item.Source.User );
				}
				catch ( e : Event ) {}
			}

			if ( playingItem != null && item == playingItem )
			{
//				controlsPanel.mouseEnabled = true;
				controlsPanel.mouseChildren = true;
				controlsPanel.Opacity = 1;
			}

			selection.Opacity = 1;
		}

		private function OnMouseOut( e : InteractiveScene3DEvent ) : void
		{
			selection.Opacity = 0;

			if ( !noPhysics ) DropItem( e );

//			controlsPanel.mouseEnabled = false;
			controlsPanel.mouseChildren = false;
			controlsPanel.Opacity = 0;

			if ( playingItem == null ) userPanel.Opacity = 0;
		}

		private function DropItem( e : Event ) : void
		{
			if ( selectedItem != null )
			{
				selectedItem.Drop( wow );
				selectedItem = null;
			}
		}

		private function StartActivityTimer() : void
		{
			activityTimer.reset();
			activityTimer.start();
			active = true;
		}

		private function OnClick( e : InteractiveScene3DEvent ) : void
		{
			trace("click");
			var item : WallItem = e.face3d ? e.face3d.userData.data as WallItem : null;

			if ( playingItem != item )
			{
				item.PickUp();
				selectedItem = item;

				var onPlay : Function;

				if ( item != lastPlaying )
				{
					if ( lastPlaying )
					{
						lastPlaying.Mesh.material.invisible = false;
						videoMesh.material.invisible = true;
					}

					videoMesh.Attach( item );

					onPlay = function() : void { videoMesh.Play( item.Source.Url ); }
				}
				else onPlay = function() : void { videoMesh.Resume(); }

				var onZoomed : Function = function() : void
				{
					onPlay();
//					controlsPanel.mouseEnabled = true;
					controlsPanel.mouseChildren = true;
					controlsPanel.Opacity = 1;
					videoControls.IsPlaying = true;
				}

				SetCameraTarget( new Number3D( item.origin.x, item.origin.y, item.origin.z - zoomDistance ), onZoomed );
				OnMouseOut( e );

				playingItem = lastPlaying = item;
				StartActivityTimer();

				userPanel.Opacity = 0;
				liveService.GetProfilePhoto( OnPhotoResponse, item.Source.User );
			}
			else
			{
				videoMesh.Pause();
				videoControls.IsPlaying = false;

				playingItem = null;
				targetLookat = new Number3D();
				StartActivityTimer();

				SetCameraTarget(
					new Number3D( 0, 0, -browseDistance ),
					function( event : Event ) : void
					{
//						controlsPanel.mouseEnabled = false;
						controlsPanel.mouseChildren = false;
						controlsPanel.Opacity = 0;
						userPanel.Opacity = 0;
					} );
			}
/*			if ( playingItem == item ) videoMesh.Resume();
			else videoMesh.Play( item.Source.Url );

			if ( playingItem && playingItem != item ) playingItem.Pause();

			if ( item.TogglePlay( scene ) )
			{
				playingItem = item;
				targetViewpoint = new Number3D( item.center.px, item.center.py, item.center.pz - 50 );
				targetLookat = new Number3D( item.center.px, item.center.py, item.center.pz );
			}
			else
			{
				playingItem = null;
				targetLookat = new Number3D();
			}*/
		}

/*
		private function Interact( x : Number, y : Number ) : void
		{
			//get the ray shooting from the camera through the mouse position
			var ray : Number3D = camera.unproject( x, y );

			//make our camera position accessible
			var cameraPosition : Number3D = new Number3D( camera.x, camera.y, camera.z );
 
			//get the world position of our ray
			ray = Number3D.add( ray, cameraPosition );
 
			//create a plane3D object
			var p : Plane3D = new Plane3D();
 
			//define the plane by a normal and a point.  In this case, we want the plane to be going through (0, 0, 0) - with its normal going (0, 1, 0).  This will make the plane an XZ plane, facing upwards.
			p.setNormalAndPoint( new Number3D( 0, 1, 0 ), new Number3D( 0, 0, 0 ) );
 
			//find where the plane intersects.  this function takes 2 points to define a line - we will define it with our ray and camera.
			var intersect:Number3D = p.getIntersectionLineNumbers( cameraPosition, ray );
 
			//set object position based on results
/*			object.x = intersect.x;
			object.y = intersect.y;
			object.z = intersect.z; 
*/

		//}

		private function UpdateLayout() : void
		{
			layoutPanel.Width = stage.stageWidth;
			layoutPanel.Height = stage.stageHeight;
		}

		private function OnResize( event : Event ) : void
		{
			UpdateLayout();

			OnChanged( event );
			StartActivityTimer();
		}

		private function OnChanged( event : Event ) : void
		{
			changeTimer.start();
		}

		private function Trace( message : String ) : void
		{
			OnTrace( new TraceEvent( message ) );
		}

		private function OnTrace( event : TraceEvent ) : void
		{
			if ( traceMethod && ExternalInterface.available ) ExternalInterface.call( traceMethod, event.Message );
			trace( event.Message );
		}

		private function OnResume( event : Event ) : void
		{
			videoMesh.Resume();
			videoControls.IsPlaying = true;
		}

		private function OnPause( event : Event ) : void
		{
			videoMesh.Pause();
			videoControls.IsPlaying = false;
		}

		private function OnPositionChangeBegin( event : Event ) : void
		{
			wasPlaying = videoControls.IsPlaying;
			if ( wasPlaying ) OnPause( event );
		}

		private function OnPositionChangeEnd( event : Event ) : void
		{
			if ( wasPlaying ) OnResume( event );
		}

		private function OnPositionChanged( event : Event ) : void
		{
			videoMesh.Position = videoMesh.Duration * videoControls.Position;
		}

		private function OnVolumeChanged( event : Event ) : void
		{
			if ( !videoControls.IsMute ) videoMesh.Volume = videoControls.Volume;
		}

		private function OnMuteChanged( event : Event ) : void
		{
			videoMesh.Volume = videoControls.IsMute ? 0 : videoControls.Volume;
		}

		private function OnPhotoResponse( url : String ) : void
		{
			Trace( "Photo URL: " + url );

//			if ( playingItem )
			{
				var item : WallItem = playingItem ? playingItem : selectedItem;

				if ( item )
				{
					userDetail.SetText( item.Source.User );
					userDetail.SetNationality( item.Source.Nationality );
					userDetail.Load( url );
				}
			}
		}
/*
		private function OnVideoResponse( liveID : String ) : void
		{
			if ( ready )
			{
//				if ( liveID == null ) liveID = "DarkKnight.flv";

				if ( playingLiveID != null && liveID == null )
				{
					OnTrace( "Playback stopped." );
					player.Pause();
					playingLiveID = null;
				}
				else if ( liveID != null && liveID != playingLiveID )
				{
					OnTrace( "Playing " + liveID + "..." );
					player.Play( liveID );

					playingLiveID = liveID;
				}

				status.Text = ( liveID == null ) ? nickname + " is not broadcasting" : "";
			}
		}*/
	}
}
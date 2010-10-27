package  
{
	import flash.display.MovieClip;
	import flash.events.EventDispatcher;
	import flash.events.NetStatusEvent;
	import flash.events.TimerEvent;
	import flash.media.Sound;
	import flash.media.Video;
	import flash.net.NetConnection;
	import flash.net.NetStream;
	import flash.utils.Timer;

	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class VideoPlayer extends EventDispatcher
	{
		//net stream assets
		private var connection : NetConnection;
    	public var stream : NetStream;
    	public var video : Video;
		
		//user interaction vars
		private var duration: Number;
		private var timer: Timer;
		private var sync: Timer;
		private var currVolume:Number;
		private var control: MovieClip;
		private var so: Sound;

		public function VideoPlayer( serverUrl : String ) 
		{
			this.url = url;

        	connection = new NetConnection();
 	        connection.connect(null);
        	stream = new NetStream(connection);
			stream.checkPolicyFile = true;
			stream.addEventListener(NetStatusEvent.NET_STATUS, onNetStatus);
       		stream.bufferTime = 10;
			// set up client
			stream.client = new Object();
			stream.client.onMetaData = onMetaData;
//			stream.play( url );
//			stream.pause();
        	video = new Video(320, 240);
//        	video.attachNetStream(stream);	
		}
/*
		private function addControls()
		{
			control = new controls();
			control.x = SCREEN_WIDTH/2- control.width/2;
			control.y = 0;
			duration = 0;
			currVolume = 1
		
			control.loader.loadbar.width = 0;
			control.playvid.addEventListener(MouseEvent.MOUSE_DOWN, handlePlay);
			control.loader.scrub.addEventListener(MouseEvent.MOUSE_DOWN, handleScrubDown);
			control.soundbar.soundscrub.addEventListener(MouseEvent.MOUSE_DOWN, handleSoundDown);
			control.addEventListener(MouseEvent.MOUSE_OVER, handleControlOver);
		
			control.playvid.buttonMode = true;
			control.loader.scrub.buttonMode = true;
			control.soundbar.soundscrub.buttonMode = true;

			addChild(control);
		
			timer = new Timer(30);
			timer.addEventListener(TimerEvent.TIMER, onTimer);
			timer.start();
		}*/

		// _______________________________________________________________________
  		//                                                                 handle metadata events
	
		private function onMetaData(data:Object):void
		{
			duration = data.duration;
		}
	
		// _______________________________________________________________________
  		//                                                                 handle net status events
		
		private function onNetStatus(e:NetStatusEvent):void
		{
			switch (e.info.code) 
			{
        		case "NetConnection.Connect.Success":
            	Trace("successfully connected");
       	     	break;
                
				case "NetStream.Play.StreamNotFound":
            	Trace("Unable to locate video: ");
            	break;
			
				case "NetStream.Buffer.Full":
				Trace("buffer full");
				//control.playvid.gotoAndStop(5);
				//addEventListener(Event.ENTER_FRAME, loop3d);
				//Tweener.addTween(control, {alpha:0, time:5, transition:"easeOutSine"});
				break;
			
				case "NetStream.Buffer.Empty":
				Trace("buffer empty");
				//control.playvid.gotoAndStop(1);
				break;
			
				case "NetStream.Play.Stop":
				Trace ("video finished");
				stream.seek(0);
				stream.pause();
				//control.playvid.gotoAndStop(1);
				//Tweener.addTween(control, {alpha:1, time:.5, transition:"easeInSine"});
				break;
        	}
		}
		
		// _______________________________________________________________________
  		//                                                                 handle video/scrub bar position
		
		private function onTimer(t:TimerEvent):void
		{
			if( duration > 0 )
			{
				var per : Number = stream.bytesLoaded/stream.bytesTotal;
				control.loader.loadbar.width = per*269;
				control.loader.scrub.x = stream.time/duration*269;
			}
		}

		public function Play( url : String ) : void
		{
			stream.play( url );
        	video.attachNetStream(stream);	
		}

		public function Resume() : void
		{
			stream.resume();
		}

		public function Pause() : void
		{
			stream.pause();
		}

		private function Trace( message : String ) : void
		{
			dispatchEvent( new TraceEvent( message ) );
		}
	}	
}
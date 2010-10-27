/**
* Player that reads all media formats Flash can read.
*
* @author	Jeroen Wijering
* @version	1.9
**/


import com.jeroenwijering.players.*;
//import com.jeroenwijering.utils.BandwidthCheck;


class com.jeroenwijering.players.MediaPlayer extends AbstractPlayer {


	/** Array with all config values **/
	private var config:Object = {
		clip:undefined,
		controlbar:35,
		height:undefined,
		width:undefined,
		file:"http://www.biffcom.com/clients/n2f/v2player/video.flv",
		displayheight:undefined,
		frontcolor:0x666666,
		backcolor:0xffffff,
		lightcolor:0xffffff,
		screencolor:0x000000,
		autoscroll:"false",
		displaywidth:undefined,
		largecontrols:"false",
		live:"false",
		logo:undefined,
		logoUrl:undefined,
		mini:"false",
		showdigits:"true",
		showdownload:"false",
		showeq:"false",
		showicons:"true",
		showvolume:"true",
		thumbsinplaylist:"false",
		usefullscreen:"true",
		fsbuttonlink:undefined,
		autostart:"true",
		bufferlength:3,
		overstretch:"false",
		repeat:"false",
		rotatetime:10,
		shuffle:"true",
		smoothing:"true",
		volume:80,
		bwfile:"100k.jpg",
		bwstreams:undefined,
		callback:undefined,
		enablejs:"false",
		javascriptid:"",
		linkfromdisplay:"false",
		linktarget:undefined,
		streamscript:undefined,
		useaudio:"true",
		usecaptions:"true",
		usekeys:"true",
		version:"Biffer Media Player 0.1 (based on Jerowing)",
		
		//Added - J. Rowley
		WebMemberID:undefined,
		WebVideoID:undefined,
		Ad:undefined,
		elapsed:5,
		alternatives:true
		
		
	};


	/** Constructor **/
	public function MediaPlayer(tgt:MovieClip) {
		super(tgt);
	};


	/** Some player-specific config settings **/
	private function loadConfig(tgt:MovieClip) {
		for(var cfv in config) {
			if(_root[cfv] != undefined) {
				config[cfv] = unescape(_root[cfv]);
			}
		}
		
		
		config['largecontrols'] == "true" ? config["controlbar"] *= 2: null;
		if (config["displayheight"] == undefined) {
			config["displayheight"] = config["height"] - config['controlbar'];
		} else if(Number(config["displayheight"])>Number(config["height"])) {
			config["displayheight"] = config["height"];
		}
		if (config["displaywidth"] == undefined) {
			config["displaywidth"] = config["width"];
		}
		config["bwstreams"] == undefined ? loadFile(): null;
	};

/*
	
	private function checkStream() {
		var ref = this;
		var str = config["bwstreams"].split(",");
		var bwc = new BandwidthCheck(config["bwfile"]);
		bwc.onComplete = function(kbps) {
			trace("bandwidth: "+kbps);
			var bwc = new ContextMenuItem("Detected bandwidth: "+kbps+" kbps",
				_root.goTo);
			bwc.separatorBefore = true;
			_root.mnu.customItems.push(bwc);
			if(ref.config['enablejs'] == "true" && 
				flash.external.ExternalInterface.available) {
				flash.external.ExternalInterface.call("getBandwidth",kbps);
			}
			for (var i=1; i<str.length; i++) {
				if (kbps < Number(str[i])) {
					ref.loadFile(str[i-1]);
					return;
				}
			}
			ref.loadFile(str[str.length-1]);
		};
	};
*/

	/** Setup all necessary MCV blocks. **/
	private function setupMCV() {
		// set controller
		controller = new PlayerController(config,feeder);
		// set default views
		var dpv = new DisplayView(controller,config,feeder);
		var cbv = new ControlbarView(controller,config,feeder);
		var vws:Array = new Array(dpv,cbv);
		
		/*
		// set optional views
		if(config["displayheight"] < config["height"]-config['controlbar'] ||
			config["displaywidth"] < config["width"]) {
			var plv = new PlaylistView(controller,config,feeder);
			vws.push(plv);
		} else {
			//config["clip"].playlist._visible = 
				//config["clip"].playlistmask._visible  = false;
		}
		if(config["usekeys"] == "true") {
			var ipv = new InputView(controller,config,feeder);
			vws.push(ipv);
		}
		if(config["showeq"] == "true") {
			var eqv = new EqualizerView(controller,config,feeder);
			vws.push(eqv);
		} else {
			config["clip"].equalizer._visible = false;
		}
		if(feeder.captions == true) {
			var cpv = new CaptionsView(controller,config,feeder);
			vws.push(cpv);
		} else {
			config["clip"].captions._visible = false;
		}
		if(feeder.audio == true) {
			var adv = new AudioView(controller,config,feeder,true);
			vws.push(adv);
		}
		if(config["enablejs"] == "true") {
			var jsv = new JavascriptView(controller,config,feeder);
			vws.push(jsv);
		}
		if(config["callback"] != undefined) {
			var cav = new CallbackView(controller,config,feeder);
			vws.push(cav);
		}*/
		
		if(config['Ad']=='true') {
			var olv = new OverlayView(controller,config,feeder);
			vws.push(olv);
		} else {
			
			config["clip"].adPanel._visible = false;
			
		}
		//@change J. Rowley 
		if(config['alternatives']!=undefined) {
			var acv = new AlternativeClipView(controller,config,feeder);
			vws.push(acv);
		}
		
		
		
		
		// Set custom clip
		
		// set models
		//var mp3 = new MP3Model(vws,controller,config,feeder,config["clip"]);
			
		var flv = new FLVModel(vws,controller,config,feeder,
			config["clip"].display.video);
		var img = new ImageModel(vws,controller,config,feeder,
			config["clip"].display.image);
		var mds:Array = new Array(flv)//,img);
		//if(feeder.captions == true) { flv.capView = cpv; }
		// start mcv cycle
		controller.startMCV(mds,img);
	};


}
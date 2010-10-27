/**
* alternativeClip banner management of the mediaplayer MCV pattern.
*
* @author	Jeroen Wijering
* @version	1.0
**/


import com.jeroenwijering.players.*;
import com.jeroenwijering.utils.*;


class com.jeroenwijering.players.AlternativeClipView extends AbstractView { 


	/** link to the banner MC **/
	private var alternativeClip:MovieClip;
	/** Imageloader **/
	private var loader:ImageLoader;
	/** flag for display of the banner **/
	private var state:Number = 0;


	/** Constructor, loads caption file. **/
	function AlternativeClipView(ctr:AbstractController,cfg:Object,fed:Object) {
		super(ctr,cfg,fed);
		var ref = this;
		alternativeClip = config['clip'].alternativeClip;
		alternativeClip._alpha = 0;
		alternativeClip.icn._visible  = false;
		alternativeClip.img.mc.panel.swapDepths(4);
		
		alternativeClip.createEmptyMovieClip("img",1);
		loader = new ImageLoader(alternativeClip.img,"none");
		loader.onLoadFinished = function() { ref.setDimensions(); };
		Stage.addListener(this);
	};


	/** place and scale the alternativeClip correctly **/
	private function setDimensions() {
		
		var ref = this;
		alternativeClip.icn._x = 0;
		alternativeClip.img.mc.gotoAndPlay(1);
		if(Stage["displayState"] == "fullScreen") {
			alternativeClip._xscale = alternativeClip._yscale = 200;
			alternativeClip._x = Stage.width/2 - alternativeClip._width/2;
			alternativeClip._y = Stage.height - alternativeClip._height - 50;
			alternativeClip.icn._x = alternativeClip._width/2 - 20;
		} else {
			alternativeClip._xscale = alternativeClip._yscale = 100;
			alternativeClip._x = config['displaywidth']/2 - alternativeClip._width/2;
			alternativeClip._y = config['displayheight'] - alternativeClip._height+1// - 10;
			alternativeClip.icn._x = alternativeClip._width - 20;
		}
		

		alternativeClip.img.mc.mid = config['WebMemberID'];
		alternativeClip.img.mc.vid = config['WebVideoID'];
		alternativeClip.img.mc.loadAltData()
		
		/*
		alternativeClip.img.mc.panel.close_btn.onPress = function() {
			Animations.fadeOut(ref.alternativeClip,0);
			ref.state = 3;
		};*/
	}


	/** Check for alternativeClip **/
	private function setItem(itm:Number) {
		loader.loadImage('alternatives.swf');
		state = 1;
		/*
		if(feeder.feed[itm]['alternativeClipfile'] != undefined) {
			loader.loadImage(feeder.feed[itm]['alternativeClipfile']);
			var lnk = feeder.feed[itm]['alternativeCliplink'];
			var tgt = config["linktarget"];
			//alternativeClip.img.onPress = function() { getURL(lnk,tgt); };
			state = 1;
		} else {
			alternativeClip._visible = false;
			state = 0;
		}*/
	};

// @Biffer - changed elp (Elapsed time to 10 seconds);
	/** load or unload alternativeClip **/
	private function setTime(elp:Number,rem:Number) {
		if(elp > Number(config['elapsed']-3) && state == 1) {
			state = 2;
			//alternativeClip.img.mc.showAdvert()
			//Animations.fadeIn(alternativeClip,100);
		} else if (rem < 2 && state == 2) {
			//Animations.fadeOut(alternativeClip,0);
			state = 3;
		} else if ((rem-elp)==5) {
			alternativeClip.img.mc.loadAltData()
		}
	}


	/** reset the alternativeClip when the movie is finished **/
	private function setState(stt:Number) {
		if(stt == 3 && state == 3) {
			state = 1;
			alternativeClip.img.mc.showAlternativeScreen()
			Animations.fadeIn(alternativeClip,100);
		}
		
		
	}


	/** OnResize Handler: catches stage resizing **/
	public function onResize() { setDimensions(); };


	/** Catches fullscreen escape  **/
	public function onFullScreen(fs:Boolean) { 
		if(fs == false) { setDimensions(); }
	};


}
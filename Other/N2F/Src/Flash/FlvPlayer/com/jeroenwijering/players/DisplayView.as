﻿/**
* Display user interface management of the players MCV pattern.
*
* @author	Jeroen Wijering
* @version	1.8
**/


import com.jeroenwijering.players.*;
import com.jeroenwijering.utils.*;


class com.jeroenwijering.players.DisplayView extends AbstractView { 


	/** reference to the  imageloader object **/
	private var  imageLoader:ImageLoader;
	/** Reference to the currently active item **/
	private var currentItem;
	/** Reference to the currently active item **/
	private var itemSize:Array;
	/** Reference to the currently active item **/
	private var thumbSize:Array;
	/** Starting position of the players **/
	private var startPos:Array;


	/** Constructor **/
	function DisplayView(ctr:AbstractController,cfg:Object,fed:Object) { 
		super(ctr,cfg,fed);
		Stage.addListener(this);
		itemSize = new Array(config['displaywidth'],config['displayheight']);
		thumbSize = new Array(config['displaywidth'],config['displayheight']);
		var ref = this;
		var tgt = config["clip"];
		imageLoader = new ImageLoader(tgt.display.thumb);
		imageLoader.onLoadFinished = function() {
			ref.thumbSize = new Array(this.targetClip._width,
				this.targetClip._height);
			ref.scaleClip(tgt.display.thumb,this.targetClip._width,
				this.targetClip._height);
		}
		startPos = new Array(tgt._x,tgt._y);
		setColorsClicks();
		setDimensions();
	};


	/** Sets up colors and clicks of all display items. **/
	private function setColorsClicks() {
		var ref = this;
		// background
		var tgt = config["clip"].back;
		tgt.col = new Color(tgt);
		tgt.col.setRGB(config["backcolor"]);
		// display items
		var tgt = config["clip"].display;
		tgt.col = new Color(tgt.back);
		tgt.col.setRGB(config["screencolor"]);
		tgt.setMask(config["clip"].mask);
		if(config["showicons"] == "false") {
			tgt.playicon._visible = false;
			tgt.muteicon._visible = false;
		}
		tgt.activity._visible = false;
		tgt.back.tabEnabled = false;
		if(config["autostart"] == "muted") {
			tgt.back.onRelease = function() { 
				ref.sendEvent("volume",80);
				ref.firstClick();
			};
		} else if (config["autostart"] == "false") {
			tgt.muteicon._visible = false;
			tgt.back.onRelease = function() { 
				ref.sendEvent("playpause");
				ref.firstClick();
			};
		} else {
			ref.firstClick();
		}
		if ( config[ "logoUrl" ] != "undefined" ) {
//		if(config["logo"] != "undefined") {
			var logoMC : MovieClip = tgt.logo.attachMovie( "n2f_logo", "logo", 1000 );
//			var lll = new ImageLoader(tgt.logo,"none");
//			lll.useSmoothing = true;
//			lll.onLoadFinished = function() {
				var aspect : Number = tgt.logo._width / tgt.logo._height;
				tgt.logo.logo._width = ref.config["displaywidth"] * 0.33;
				tgt.logo.logo._height = tgt.logo._width / aspect;
				tgt.logo._x = ref.config["displaywidth"] - 
					tgt.logo.logo._width - 10;
				tgt.logo._y = ref.config["displayheight"] -
					tgt.logo.logo._height - 45;
//			};
//			lll.loadImage(config["logo"]);
			tgt.logo._alpha = 40;
			var logoUrl = config[ "logoUrl" ];
			tgt.logo.onRelease = function() { 
				tgt.getURL( logoUrl );
//				ref.sendEvent("getlink",ref.currentItem);
			};
			tgt.logo.onRollOver = function() {
				tgt.logo._alpha = 80;
			};
			tgt.logo.onRollOut = function() {
				tgt.logo._alpha = 40;
			};
		}
	};


	/** Sets up dimensions of all controlbar items. **/
	private function setDimensions() {
		var tgt = config["clip"].back;
		if(Stage["displayState"] == "fullScreen") {
			config["clip"]._x = config["clip"]._y = 0;
			tgt._width = Stage.width;
			tgt._height = Stage.height;
		} else {
			config["clip"]._x = startPos[0];
			config["clip"]._y = startPos[1];
			tgt._width = config["width"];
			tgt._height = config["height"];
			if(config["displayheight"] >= config["height"] - 
				config['controlbar'] && config["displaywidth"] == 
				config["width"]) { tgt._height--; }
		} 
		var tgt = config["clip"].display;
		scaleClip(tgt.thumb,thumbSize[0],thumbSize[1]);
		scaleClip(tgt.image,itemSize[0],itemSize[1]);
		scaleClip(tgt.video,itemSize[0],itemSize[1]);
		if(Stage["displayState"] == "fullScreen") {
			config["clip"].mask._width = 
				tgt.back._width = Stage.width;
			config["clip"].mask._height = 
				tgt.back._height = Stage.height-45;
		 } else {
			config["clip"].mask._width = 
				tgt.back._width = config["displaywidth"];
			config["clip"].mask._height = 
				tgt.back._height = config["displayheight"];
		}
		tgt.playicon._x  = tgt.muteicon._x = Math.round(tgt.back._width/2);
		tgt.activity._x = 4;
		
		tgt.playicon._y = tgt.muteicon._y = Math.round(tgt.back._height/2);
		tgt.activity._y = 4
		if(Stage["displayState"] == "fullScreen") {
			tgt.playicon._xscale = tgt.playicon._yscale = 
				tgt.muteicon._xscale = tgt.muteicon._yscale =
				tgt.activity._xscale = tgt.activity._yscale = 
				tgt.logo._xscale = tgt.logo._yscale = 200;
			tgt.logo._x = Stage.width - tgt.logo._width - 20;
			tgt.logo._y = Stage.height - tgt.logo._height - 60;
		} else {
			tgt.playicon._xscale = tgt.playicon._yscale = 
				tgt.muteicon._xscale = tgt.muteicon._yscale =
				tgt.activity._xscale = tgt.activity._yscale =
				tgt.logo._xscale = tgt.logo._yscale = 100;
			if(tgt.logo._height > 1) {
				tgt.logo._x = config["displaywidth"]- tgt.logo._width - 10;
				tgt.logo._y = config["displayheight"] -	tgt.logo._height - 10;
			}
		}
	};


	/** Show and hide the play/pause button and show activity icon **/
	private function setState(stt:Number) {
		var tgt = config["clip"].display;
		switch(stt) {
			case 0:
				if (config["linkfromdisplay"] == "false" && 
					config["showicons"] == "true" &&
					feeder.feed[currentItem]['category'] != 'preroll' &&
					feeder.feed[currentItem]['category'] != 'postroll') {
					tgt.playicon._visible = true;
				}
				tgt.activity._visible = false;
				break;
			case 1:
				tgt.playicon._visible = false;
				if (config["showicons"] == "true") {
					tgt.activity._visible = true;
				}
				break;
			case 2:
				tgt.playicon._visible = false;
				tgt.activity._visible = false;
				break;
		}
	};


	/** save size information and rescale accordingly **/
	private function setSize(wid:Number,hei:Number) {
		itemSize = new Array (wid,hei);
		var tgt = config["clip"].display;
		scaleClip(tgt.image,itemSize[0],itemSize[1]);
		scaleClip(tgt.video,itemSize[0],itemSize[1]);
	};


	/** Scale movie according to overstretch setting **/
	private function scaleClip(tgt:MovieClip,wid:Number,hei:Number):Void {
		var tcf = tgt.mc._currentframe;
		tgt.mc.gotoAndStop(1);
		if(Stage["displayState"] == "fullScreen") {
			var stw:Number = Stage.width;
			var sth:Number = Stage.height;
		} else {
			var stw = config["displaywidth"];
			var sth = config["displayheight"];
		}
		var xsr:Number = stw/wid;
		var ysr:Number = sth/hei;
		if (xsr < ysr && config["overstretch"] == "false" || 
			ysr < xsr && config["overstretch"] == "true") { 
			tgt._width = wid*xsr;
			tgt._height = hei*xsr;
		} else if(config["overstretch"] == "none") {
			tgt._width = wid;
			tgt._height = hei;
		} else if (config["overstretch"] == "fit") {
			tgt._width = stw;
			tgt._height = sth;
		} else { 
			tgt._width = wid*ysr;
			tgt._height = hei*ysr;
		}
		tgt._x = stw/2 - tgt._width/2;
		tgt._y = sth/2 - tgt._height/2;
		tgt.mc.gotoAndPlay(tcf);
	};


	/** Load Thumbnail image if available. **/
	private function setItem(idx:Number) {
		currentItem = idx;
		var tgt = config["clip"].display;
		if(feeder.feed[idx]["image"] == "undefined") { 
			tgt.thumb.clear();
			tgt.thumb._visible = false;
		} else {
			imageLoader.loadImage(feeder.feed[idx]["image"]);
			tgt.thumb._visible = true;
		}
	};


	/** OnResize Handler: catches stage resizing **/
	public function onResize() {
		if(_root.displayheight > config["height"]+10) {
			config["height"] = config["displayheight"] = Stage.height;
			config["width"] = config["displaywidth"] = Stage.width;
		}
		setDimensions(); 
	};


	/** Catches fullscreen escape  **/
	public function onFullScreen(fs:Boolean) {
		if(fs == false) { setDimensions(); }
	};


	/** Catches the first display click to reset unmute / displayclick **/
	private function firstClick() {
		var ref = this;
		var tgt = config["clip"].display;
		tgt.playicon._visible = false;
		tgt.muteicon._visible = false;
		if(config["linkfromdisplay"] == "true") {
			tgt.back.onRelease = function() { 
				ref.sendEvent("getlink",ref.currentItem); 
			};
		} else {
			tgt.back.onRelease = function() { 
				ref.sendEvent("playpause",1);
			};
		}
		
	};


}
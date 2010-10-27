// tweening prototypes 
// version 1.2.0
// Ladislav Zigo,lacoz@web.de
/*
Updates by Moses Gunesch - www.MosesSupposes.com - May 2005 [all updates marked with search string: -(MosesSupposes)- ]
	Added extensions:	invertColorTo, tintTo, sizeTo
						pauseTween, unpauseTween, isTweenPaused, ffTween, rewTween,
						(global shortcuts:) pauseAllTweens, unpauseAllTweens, stopAllTweens
	TextField support, String for relative values, comma-delimited strings for mult props,
		support for single endval : mult props, new cleanUp routine, autoStop affects all props
	<<Complete notes in $tweenManager.as>>
*/
// to avoid reseting tweenManger when loading another .swf
if(_global.$tweenManager == undefined){
	_global.$tweenManager = new zigo.tweenManager();
} else {
	// -(MosesSupposes)- Also fix for re-testing a published swf locally during tweens
	_global.$tweenManager.cleanUp();
	_global.$tweenManager.init();
	//_global.$tweenManager.playing = false; //(original LZ code - might disrupt loaded swfs)
}
// easing equations 
// from Robert Penner, www.robertpenner.com 
com.robertpenner.easing.Back;
com.robertpenner.easing.Bounce;
com.robertpenner.easing.Circ;
com.robertpenner.easing.Cubic;
com.robertpenner.easing.Elastic;
com.robertpenner.easing.Expo;
com.robertpenner.easing.Linear;
com.robertpenner.easing.Quad;
com.robertpenner.easing.Quart;
com.robertpenner.easing.Quint;
com.robertpenner.easing.Sine;
//
var Mp = MovieClip.prototype;
Mp.addListener = function() {
 if (!this._listeners) {
  AsBroadcaster.initialize(this);
 }
 this.addListener.apply(this,arguments);
};
ASSetPropFlags(Mp, "addListener", 1, 0)

// == core methods ==
Mp.tween = function(props, pEnd, seconds, animType,
				delay, callback, extra1, extra2) {
	if (_global.$tweenManager.isTweenLocked(this)){
		trace("tween not added, this movieclip is locked");
		return;
	}	
	if (arguments.length<2) {
		trace("tween not added, props & pEnd must be defined");
		return;
	}
	// parse arguments to valid type:
	// parse properties
	if (typeof (props) == "string") {
		if (props.indexOf(',')>-1) { //-(MosesSupposes)- enables comma-delimited string for mulitple props
			props = props.split(' ').join('').split(',');
		}
		else props = [props];
	}
	// parse end values
	// if pEnd is not array 
	if (!(pEnd instanceof Array)) { //-(MosesSupposes)- modified to work with string rel values
		pEnd = [pEnd];
		while (pEnd.length<props.length) {// and added this routine to allow single end val to be passed for multiple props.
			pEnd.push(pEnd[0]);
		}
	}
	// parse time properties
	if(seconds == undefined) {
		seconds = 2;
	}else if (seconds<0.01){
		seconds = 0;
	}
	if (delay<0.01 || delay == undefined) {
		delay = 0;
	}
	// parse animtype to reference to equation function 
	switch(typeof(animType)){
	case "string":
	//string
		animType = animType.toLowerCase();
		if (animType == "linear") {
			var eqf = com.robertpenner.easing.Linear.easeNone;
		} else if (animType.indexOf("easeoutin") == 0) {
			var t = animType.substr(9);
			t = t.charAt(0).toUpperCase()+t.substr(1);
			var eqf = com.robertpenner.easing[t].easeOutIn;
		} else if (animType.indexOf("easeinout") == 0) {
			var t = animType.substr(9);
			t = t.charAt(0).toUpperCase()+t.substr(1);
			var eqf = com.robertpenner.easing[t].easeInOut;
		} else if (animType.indexOf("easein") == 0) {
			var t = animType.substr(6);
			t = t.charAt(0).toUpperCase()+t.substr(1);
			var eqf = com.robertpenner.easing[t].easeIn;
		} else if (animType.indexOf("easeout") == 0) {
			var t = animType.substr(7);
			t = t.charAt(0).toUpperCase()+t.substr(1);
			var eqf = com.robertpenner.easing[t].easeOut;
		}
		if (eqf == undefined) {
			// set default tweening equation
			var eqf = com.robertpenner.easing.Expo.easeOut;
		}
		break;
	case "function":
	// function
		var eqf = animType;
		break;
	case "object":
		// object from custom easing
		if (animType.ease != undefined && animType.pts != undefined ){
			var eqf = animType.ease;
			extra1 = animType.pts;
		}else{
			var eqf = com.robertpenner.easing.Expo.easeOut;
		}
		break;
	default:
		var eqf = com.robertpenner.easing.Expo.easeOut;
	}

	// parse callback function
	switch(typeof (callback)) {
	case "function":
		callback = {func:callback, scope:this._parent};
		break;
	case "string":
		var ilp, funcp, scope, args, a;
		ilp = callback.indexOf("(");
		funcp = callback.slice(0, ilp);
		
		scope = eval(funcp.slice(0, funcp.lastIndexOf(".")));
		func = eval(funcp);
		args = callback.slice(ilp+1, callback.lastIndexOf(")")).split(",");
		for (var i = 0; i<args.length; i++) {
			a = eval(args[i]);
			if (a != undefined) {
				args[i] = a;
			}
		}
		callback = {func:func, scope:scope, args:args };
		break;
	}
	if(_global.$tweenManager.autoStop){
		// automatic removing tweens as in Zeh proto
		_global.$tweenManager.removeTween(this); // -(MosesSupposes)- changed to stop all tweening props in target. (Similar props are overwritten regardless of autoStop.)
	}
	if(delay > 0){
		_global.$tweenManager.addTweenWithDelay(delay,this, props, pEnd, seconds, eqf, callback, extra1, extra2);
	}else{
		_global.$tweenManager.addTween(this, props, pEnd, seconds, eqf, callback, extra1, extra2);
	}
};
//

// -- tween control methods --
Mp.stopTween = function(props) {
	if (typeof (props) == "string") {
		if (props.indexOf(',')>-1) {//-(MosesSupposes)- enables comma-delimited string for mulitple props
			props = props.split(' ').join('').split(',');
		}
		else props = [props];
	}
	_global.$tweenManager.removeTween(this, props);
};
//
Mp.isTweening = function(prop:String) { //-(MosesSupposes)- added prop param
	//returns boolean
	return _global.$tweenManager.isTweening(this, prop);
};
//
Mp.getTweens = function() {
	// returns count of running tweens
	return _global.$tweenManager.getTweens(this);
};
//
Mp.lockTween = function() {
	// 
	_global.$tweenManager.lockTween(this,true);
};
//
Mp.unlockTween = function() {
	// 
	_global.$tweenManager.lockTween(this,false);
};
//
Mp.isTweenLocked = function() {
	// 
	return _global.$tweenManager.isTweenLocked(this);
};
//
Mp.isTweenPaused = function(prop:String) { //-(MosesSupposes)-
	// 
	return _global.$tweenManager.isTweenPaused(this, prop);
};
//
Mp.pauseTween = function(props) { //-(MosesSupposes)-
	// 
	var propsObj;
	if (props!=undefined) {
		if (typeof (props) == "string") {
			if (props.indexOf(',')>-1) {// enables comma-delimited string for mulitple props
				props = props.split(' ').join('').split(',');
			}
			else props = [props];
		}
		propsObj = {};
		for (var i in props) propsObj[props[i]] = true;
	}
	_global.$tweenManager.pauseTween(this, propsObj);
};
//
Mp.unpauseTween = function(props) { //-(MosesSupposes)-
	// 
	var propsObj;
	if (props!=undefined) {
		if (typeof (props) == "string") {
			if (props.indexOf(',')>-1) {// enables comma-delimited string for mulitple props
				props = props.split(' ').join('').split(',');
			}
			else props = [props];
		}
		propsObj = {};
		for (var i in props) propsObj[props[i]] = true;
	}
	_global.$tweenManager.unpauseTween(this, propsObj);
};
//
Mp.pauseAllTweens = function() { //-(MosesSupposes)-
	// globally pause all tweens. (just a shortcut so you don't have to type _global.$tweenManager)
	_global.$tweenManager.pauseTween();
};
//
Mp.unpauseAllTweens = function() { //-(MosesSupposes)-
	// globally unpause all tweens (just a shortcut so you don't have to type _global.$tweenManager)
	_global.$tweenManager.unpauseTween();
};
//
Mp.stopAllTweens = function() { //-(MosesSupposes)-
	// globally stop all tweens (just a shortcut so you don't have to type _global.$tweenManager)
	_global.$tweenManager.stopAll();
};

//
Mp.ffTween = function(props) { //-(MosesSupposes)-
	// 
	var propsObj;
	if (props!=undefined) {
		if (typeof (props) == "string") {
			if (props.indexOf(',')>-1) {// enables comma-delimited string for mulitple props
				props = props.split(' ').join('').split(',');
			}
			else props = [props];
		}
		propsObj = {};
		for (var i in props) propsObj[props[i]] = true;
	}
	_global.$tweenManager.ffTween(this, propsObj);
};
Mp.rewTween = function(props) { //-(MosesSupposes)-
	// 
	var propsObj;
	if (props!=undefined) {
		if (typeof (props) == "string") {
			if (props.indexOf(',')>-1) {// enables comma-delimited string for mulitple props
				props = props.split(' ').join('').split(',');
			}
			else props = [props];
		}
		propsObj = {};
		for (var i in props) propsObj[props[i]] = true;
	}
	_global.$tweenManager.rewTween(this, propsObj);
};
//

// == shortcut methods == 
// these methods only pass parameters to tween method
Mp.alphaTo = function (destAlpha, seconds, animType, delay, callback, extra1, extra2) {
	this.tween(["_alpha"],[destAlpha],seconds,animType,delay,callback,extra1,extra2)
}
//
Mp.scaleTo = function (destScale, seconds, animType, delay, callback, extra1, extra2) {
	this.tween(["_xscale", "_yscale"],[destScale, destScale],seconds,animType,delay,callback,extra1,extra2)
}
//
Mp.sizeTo = function (destSize, seconds, animType, delay, callback, extra1, extra2) { //-(MosesSupposes)- 
	this.tween(["_width", "_height"],[destSize, destSize],seconds,animType,delay,callback,extra1,extra2)
}
//
Mp.slideTo = function (destX, destY, seconds, animType, delay, callback, extra1, extra2) {
	this.tween(["_x", "_y"],[destX, destY],seconds,animType,delay,callback,extra1,extra2)
}
//
Mp.rotateTo = function (destRotation, seconds, animType, delay, callback, extra1, extra2) {
	// note: to force counterclockwise rotation pass a neg value in string for relative positioning
	this.tween(["_rotation"],[destRotation],seconds,animType,delay,callback,extra1,extra2)
}
//
//
//
//
// getColorTransObj :: Global helper function that returns an object with transform props. -(MosesSupposes)- 
// 		example of how it could be used elsewhere: new Color(photo_mc).setTransform(_global.getColorTransObj('contrast',150));
_global.getColorTransObj = function(type:String, amt:Number, rgb:Number):Object
{
	switch(type) {
		case 'brightness': // amt:-100=black, 0=normal, 100=white
		var percent = 100 - Math.abs(amt);
		var offset = 0;
		if (amt > 0) offset = 256 * (amt / 100);
		return {ra: percent, rb:offset,
				ga: percent, gb:offset,
				ba: percent,bb:offset}
		//
		case 'brightOffset': // "burn" effect. amt:-100=black, 0=normal, 100=white
		var offset = 256*(amt/100);
		return {ra:100, rb:offset, ga:100, gb:offset, ba:100, bb:offset};
		//
		case 'contrast': // amt:0=gray, 100=normal, 200=high-contrast, higher=posterized.
		var o = {};
		o.ra = o.ga = o.ba = amt;
		o.rb = o.gb = o.bb = 128 - (128/100*amt);
		return o;
		//
		case 'invertColor': // amt:0=normal,50=gray,100=photo-negative
		var o = {};
		o.ra = o.ga = o.ba = 100 - 2 * amt;
		o.rb = o.gb = o.bb = amt * (255/100);
		return o;
		//
		case 'tint': // amt:0=none,100=solid color (>100=posterized to tint, <0=inverted posterize to tint)
		if (rgb == undefined || rgb == null) break; // rgb:0xRRGGBB number or null for reset
		var r = (rgb >> 16) ;
		var g = (rgb >> 8) & 0xFF;
		var b = rgb & 0xFF;
		var ratio = amt / 100;
		var o = {rb:r*ratio, gb:g*ratio, bb:b*ratio};
		o.ra = o.ga = o.ba = 100 - amt;
		return o;
	}
	return {rb:0, ra:100, gb:0, ga:100, bb:0, ba:100}; // resets to normal.
}
//  -- Color methods -- 
// Number datatyping added to these since relative vals are not supported. -(MosesSupposes)-
Mp.brightnessTo = function (bright:Number, seconds, animType, delay, callback, extra1, extra2) {
	this.tween(["_ct_"],[getColorTransObj('brightness',bright)],seconds,animType,delay,callback,extra1,extra2)
}
//
Mp.brightOffsetTo = function(percent:Number, seconds, animType, delay, callback, extra1, extra2) {
	this.tween(["_ct_"], [getColorTransObj('brightOffset',percent)], seconds, animType, delay, callback, extra1, extra2);
};
//
Mp.contrastTo = function(percent:Number, seconds, animType, delay, callback, extra1, extra2) {
	// from Robert Penner color toolkit
	this.tween(["_ct_"], [getColorTransObj('contrast',percent)], seconds, animType, delay, callback, extra1, extra2);
};
//
Mp.colorTo = function (rgb:Number, seconds, animType, delay, callback, extra1, extra2) {
	this.tween(["_ct_"],[getColorTransObj('tint',100,rgb)],seconds,animType,delay,callback,extra1,extra2)
}
//
Mp.colorTransformTo = function (ra:Number, rb:Number, ga:Number, gb:Number, ba:Number, bb:Number, aa:Number, ab:Number, 
								seconds, animType, delay, callback, extra1, extra2) {
	// destination color transform matrix
	var destCt = {ra: ra ,rb: rb , ga: ga, gb: gb, ba: ba, bb: bb, aa: aa, ab: ab}
	this.tween(["_ct_"],[destCt],seconds,animType,delay,callback,extra1,extra2)
}
// invertColorTo: invert colors like a photo-negative, based on a percentage -(MosesSupposes)- based on Penner / SuperColor (http://www.lalex.com)
Mp.invertColorTo = function (percent:Number, seconds, animType, delay, callback, extra1, extra2) { 
	this.tween(["_ct_"],[getColorTransObj('invertColor',percent)],seconds,animType,delay,callback,extra1,extra2)
}
// tintTo: same as colorTo but second param is percent to tint -(MosesSupposes)- based on Penner / SuperColor (http://www.lalex.com)
Mp.tintTo = function (rgb:Number, percent:Number, seconds, animType, delay, callback, extra1, extra2) {
	this.tween(["_ct_"],[getColorTransObj('tint',percent,rgb)],seconds,animType,delay,callback,extra1,extra2)
}
//

// frameTo shorcut method and _frame property - MovieClip only
Mp.getFrame = function() {
	return this._currentframe;
};
Mp.setFrame = function(fr) {
	this.gotoAndStop(Math.round(fr));
};
Mp.addProperty("_frame", Mp.getFrame, Mp.setFrame);
//
Mp.frameTo = function(endframe, duration, animType, delay, callback, extra1, extra2) {
	if (endframe == undefined) {
		endframe = this._totalframes;
	}
	this.tween("_frame", endframe, duration, animType, delay, callback, extra1, extra2);
};
//



// Copy all lmc_tween functionality to TextFields and set ASSetPropFlags for both -(MosesSupposes)-
var TFP = TextField.prototype;
if (!TFP.origAddListener) {
	TFP.origAddListener = TFP.addListener;// avoid unwanted recursion by storing ref to original method
	ASSetPropFlags(TFP, 'origAddListener', 1, 0);
	TFP.addListener = function() {
		if (!this._listeners) {
			AsBroadcaster.initialize(this);
		}
		this.origAddListener.apply(this,arguments);
	};
}
var $_$methods:Array = [
	"tween", "stopTween", "isTweening", "getTweens", "lockTween", "isTweenLocked", "unlockTween",
	"isTweenPaused", "pauseTween", "unpauseTween", "pauseAllTweens", "unpauseAllTweens", "stopAllTweens",
	"ffTween", "rewTween", "getFrame", "setFrame", "_frame", "frameTo",
	"alphaTo", "brightnessTo", "colorTo", "colorTransformTo", "invertColorTo", "tintTo",
	"scaleTo", "sizeTo", "slideTo", "rotateTo", "brightOffsetTo", "contrastTo" ];
for (var $_$i in $_$methods) {
	ASSetPropFlags(Mp, $_$methods[$_$i], 1, 0);
	if ($_$methods[$_$i].toLowerCase().indexOf('frame')==-1) { // do not copy any of the 'frameTo' stuff
		TFP[$_$methods[$_$i]] = Mp[$_$methods[$_$i]];
		ASSetPropFlags(TFP, $_$methods[$_$i], 1, 0);
	}
}

delete Mp;
delete TFP;
delete $_$methods;
delete $_$i;



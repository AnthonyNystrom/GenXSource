/**
* A couple of commonly used animation functions.
*
* @author	Jeroen Wijering
* @version	1.3
**/


class com.jeroenwijering.utils.Animations {


	/** Fadein function for MovieClip. **/
	public static function fadeIn(tgt:MovieClip,end:Number,spd:Number):Void {
		arguments.length < 3 ? spd = 20: null;
		arguments.length < 2 ? end = 100: null;
		tgt._visible = true;
		tgt.onEnterFrame = function() {
			if(this._alpha > end-spd) {
				delete this.onEnterFrame;
				this._alpha = end;
			} else {
				this._alpha += spd;
			}
		};
	};


	/** Fadeout function for MovieClip. **/
	public static function fadeOut(tgt:MovieClip,end:Number,
		spd:Number,rmv:Boolean):Void {
		arguments.length < 4 ? rmv = false: null;
		arguments.length < 3 ? spd = 20: null;
		arguments.length < 2 ? end = 0: null;
		tgt.onEnterFrame = function() {
			if(this._alpha < end+spd) {
				delete this.onEnterFrame;
				this._alpha = end;
				end == 0 ? this._visible = false: null;
				rmv == true ? this.removeMovieClip(): null;
			} else {
				this._alpha -= spd;
			}
		};
	};


	/** Crossfade a given MovieClip to/from to 0. **/
	public static function crossfade(tgt:MovieClip, alp:Number) {
		var phs = "out";
		var pct = alp/5;
		tgt.onEnterFrame = function() {
			if(phs == "out") {
				this._alpha -= pct;
				if (this._alpha < 1) { phs = "in"; }
			} else {
				this._alpha += pct;
				this._alpha >= alp ? delete this.onEnterFrame : null; 
			}
		}; 
	};


	/** Easing enterframe function for a Movieclip. **/
	public static function easeTo(tgt:MovieClip,xps:Number,yps:Number,
		spd:Number):Void {
		arguments.length < 4 ? spd = 2: null;
		tgt.onEnterFrame = function() {
			this._x = xps-(xps-this._x)/(1+1/spd);
			this._y = yps-(yps-this._y)/(1+1/spd);
			if (this._x>xps-1 && this._x<xps+1 && 
				this._y>yps-1 && this._y<yps+1) {
				this._x = Math.round(xps);
				this._y = Math.round(yps);
				delete this.onEnterFrame;
			} 
		}; 
	};


	/** Ease typewrite text into a tag after a given delay. **/
	public static function easeText(tgt:MovieClip,rnd:Number,
		txt:String,spd:Number) {
		if (arguments.length < 3) {
			tgt.str = tgt.tf.text;
			tgt.hstr = tgt.tf.htmlText;
		} else { tgt.str = tgt.hstr = txt; }
		if (arguments.length < 4) { spd = 1.4; }
		tgt.tf.text = "";
		tgt.i = 0;
		tgt.rnd = rnd;
		tgt.onEnterFrame = function() {
			if(this.i > this.rnd) { 
				this.tf.text = this.str.substr(0, this.str.length - 
					Math.floor((this.str.length - this.tf.text.length)/spd));
			}
			if(this.tf.text == this.str) {
				this.tf.htmlText = this.hstr;
				if(this.more != undefined) { this.more._visible = true; }
				delete this.onEnterFrame;
			}
			this.i++;
		};
	};


}
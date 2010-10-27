﻿class com.robertpenner.easing.Quart {
	static function easeIn (t:Number, b:Number, c:Number, d:Number):Number {
		return c*(t/=d)*t*t*t + b;
	}
	static function easeOut (t:Number, b:Number, c:Number, d:Number):Number {
		return -c * ((t=t/d-1)*t*t*t - 1) + b;
	}
	static function easeInOut (t:Number, b:Number, c:Number, d:Number):Number {
		if ((t/=d/2) < 1) return c/2*t*t*t*t + b;
		return -c/2 * ((t-=2)*t*t*t - 2) + b;
	}
	static function easeOutIn (t:Number, b:Number, c:Number, d:Number):Number {
		if ((t/=d/2)<1) return -c/2*(--t*t*t*t-1)+b;
		return c/2*(--t*t*t*t+1)+b;
	}
}

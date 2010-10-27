class com.robertpenner.easing.Sine {
	static function easeIn (t:Number, b:Number, c:Number, d:Number):Number {
		return -c * Math.cos(t/d * (Math.PI/2)) + c + b;
	}
	static function easeOut (t:Number, b:Number, c:Number, d:Number):Number {
		return c * Math.sin(t/d * (Math.PI/2)) + b;
	}
	static function easeInOut (t:Number, b:Number, c:Number, d:Number):Number {
		return -c/2 * (Math.cos(Math.PI*t/d) - 1) + b;
	}
	static function easeOutIn (t:Number, b:Number, c:Number, d:Number):Number {
		if ((t /= d/2)<1) return c/2 * (Math.sin(Math.PI*t/2) ) + b;
		return -c/2 * (Math.cos(Math.PI*--t/2)-2) + b;
	}
}

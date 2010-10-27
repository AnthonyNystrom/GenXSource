class com.robertpenner.easing.Expo {
	static function easeIn (t:Number, b:Number, c:Number, d:Number):Number {
		return (t==0) ? b : c * Math.pow(2, 10 * (t/d - 1)) + b;
	}
	static function easeOut (t:Number, b:Number, c:Number, d:Number):Number {
		return (t==d) ? b+c : c * (-Math.pow(2, -10 * t/d) + 1) + b;
	}
	static function easeInOut (t:Number, b:Number, c:Number, d:Number):Number {
		if (t==0) return b;
		if (t==d) return b+c;
		if ((t/=d/2) < 1) return c/2 * Math.pow(2, 10 * (t - 1)) + b;
		return c/2 * (-Math.pow(2, -10 * --t) + 2) + b;
	}
	static function easeOutIn (t:Number, b:Number, c:Number, d:Number):Number {
		if (t==0) return b;
		if (t==d) return b+c;
		if ((t/=d/2) < 1) return c/2 * (-Math.pow(2, -10 * t) + 1) + b;
		return c/2 * (Math.pow(2, 10 * (t-2))+1) + b;
	}
	
}

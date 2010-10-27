/**
* A couple of commonly used string operations.
*
* @author	Jeroen Wijering
* @version	1.3
**/


class com.jeroenwijering.utils.StringMagic {


	/** Strip tags and breaks from a string. **/
	static function stripTagsBreaks(str:String):String {
		if(str.length == 0 || str == undefined) { return ""; }
		var tmp:Array = str.split("\n");
		str = tmp.join("");
		var tmp:Array = str.split("\r");
		str = tmp.join("");
		var i:Number = str.indexOf("<");
		while(i != -1) {
			var j = str.indexOf(">",i+1);
			j == -1 ? j = str.length-1: null;
			str = str.substr(0,i) + str.substr(j+1,str.length);
			i = str.indexOf("<",i);
		}
		return str;
	};


	/** Chop string into a number of lines. **/
	static function chopString(str:String,cap:Number,nbr:Number):String {
		for(var i=cap; i<str.length; i+=cap) {
			if(i == cap*nbr) {
				if(str.indexOf(" ",i-5) == -1) {
					return str;
				} else {
					return str.substr(0,str.indexOf(" ",i-5));
				}
			} else  if(str.indexOf(" ",i) > 0) {
				str = str.substr(0,str.indexOf(" ",i-3)) + "\n" +
					str.substr(str.indexOf(" ",i-3)+1);
			}
		}
		return str;
	};


	/** Add a leading zero and convert number to string. **/
	static function addLeading(nbr:Number):String { 
		if(nbr < 10) { 
			return "0"+Math.floor(nbr); 
		} else { 
			return Math.floor(nbr).toString(); 
		}
	};


}
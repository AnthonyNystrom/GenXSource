/**
* Used to quickly draw a square or rounded square on stage.
*
* @author	Jeroen Wijering
* @version	1.2
**/


class com.jeroenwijering.utils.Draw {


	/** Draw a square in a given movieclip. **/
	public static function square(tgt:MovieClip,wth:Number,hei:Number,
		clr:Number,tck:Number,cls:Number):Void {
		tgt.clear();
		if(tck != undefined) { tgt.lineStyle(tck,cls,100); }
		tgt.beginFill(clr,100);
		tgt.moveTo(0,0);
		tgt.lineTo(wth,0);
		tgt.lineTo(wth,hei);
		tgt.lineTo(0,hei);
		tgt.lineTo(0,0);
		tgt.endFill();
	};


	/** Draw a rounded-corner square in a given movieclip. **/
	public static function roundedSquare(tgt:MovieClip,wth:Number,hei:Number,
		rad:Number,clr:Number,tck:Number,cls:Number,
		xof:Number,yof:Number,alp:Number):Void {
		tgt.clear();
		if(tck > 0) { tgt.lineStyle(tck,cls,100); }
		if(xof == undefined) { xof = yof = 0; }
		if(alp == undefined) { alp = 100; }
		tgt.beginFill(clr,alp);
		tgt.moveTo(rad+xof,yof);
		tgt.lineTo(wth-rad+xof,yof);
		tgt.curveTo(wth+xof,yof,wth+xof,rad+yof);
		tgt.lineTo(wth+xof,hei-rad+yof);
		tgt.curveTo(wth+xof,hei+yof,wth-rad+xof,hei+yof);
		tgt.lineTo(rad+xof,hei+yof);
		tgt.curveTo(xof,hei+yof,xof,hei-rad+yof);
		tgt.lineTo(xof,rad+yof);
		tgt.curveTo(xof,yof,rad+xof,yof);
		tgt.endFill();
	};


}
/**
* Overlay AD management of the mediaplayer MCV pattern.
*
* @author	Jeroen Wijering
* @version	1.0
**/


import com.jeroenwijering.players.*;
import com.jeroenwijering.utils.*;
import com.rapierdev.data.XMLManager;
import mx.utils.Delegate


class com.jeroenwijering.players.OverlayView extends AbstractView { 


	
	var BASE_URL		:String = 'http://12.206.33.16/N2FWebServices/VideoPopup.asmx/GetVideoPopup'
	


	private var $campaignXMLMgr:XMLManager;
	
	/** link to the banner MC **/
	private var overlay:MovieClip;
	/** Imageloader **/
	private var loader:ImageLoader;
	/** flag for display of the banner **/
	private var state:Number = 0;


	/** Constructor, loads caption file. **/
	function OverlayView(ctr:AbstractController,cfg:Object,fed:Object) {
		super(ctr,cfg,fed);
		var ref = this;
		overlay = config['clip'].adPanel;

		overlay.panel._visible = false;
		overlay.panel._alpha = 0;
		Stage.addListener(this);
		setDimensions();
		loadAdvertData();
		
	};
	
	
	public function loadAdvertData():Void {
	

	$campaignXMLMgr = new XMLManager();
	$campaignXMLMgr.addEventListener('onXMLLoad',Delegate.create(this,campaignLoadSucceed));
	$campaignXMLMgr.addEventListener('onXMLFail',campaignLoadFail);
	$campaignXMLMgr.loadObjects(BASE_URL+'?MemberID='+config['WebMemberID']+'&VideoID='+config['WebMemberID'])
	
	}
	
	// XML object sent on success

	public function campaignLoadSucceed(obj):Void  {
	
	
	var $xml:Object = obj.xmlobj.VideoAdvert[0];
	
	// Setup hyperlink
	var hyperLink = $xml.HyperLinkText[0].data;
	
	overlay.panel.bg.onRelease = function() {
		getURL(hyperLink,'_blank');
	}

	if($xml.Show[0].data!='false') {
		with(overlay.panel) {
			
			// Information Fill
			title_tf.text 			= $xml.Title[0].data;
			description_tf.text 	= $xml.Description[0].data;
			hyperlink_tf.text		= $xml.HyperLink[0].data;
			
			img.loader.loadMovie	($xml.ImageURL[0].data);
			
		// Layout colouring
			
		$xml.BGColor[0].data!=undefined ? bg.colorTo(Number('0x'+$xml.BGColor[0].data),.1): null;
		$xml.TextColor0[0].data!=undefined ? title_tf.colorTo(Number('0x'+$xml.TextColor0[0].data),.1) :null;
		$xml.TextColor1[0].data!=undefined ? description_tf.colorTo(Number('0x'+$xml.TextColor1[0].data),.1):null;
		$xml.TextColor0[0].data!=undefined ? hyperlink_tf.colorTo(Number('0x'+$xml.TextColor0[0].data),.1):null;
		$xml.OutlineColor[0].data!=undefined ? outline.colorTo(Number('0x'+$xml.OutlineColor[0].data),.1):null;
		$xml.CloseColor[0].data!=undefined ? close_btn.colorTo(Number('0x'+$xml.CloseColor[0].data),.1):null;
			}
	
		}
		
	}
	
	// Ad data fail
	public function campaignLoadFail(obj):Void {
	
		trace('Silent fail');
	}
	
	// _________________________________________________________________________Advert Animation (On-Screen);


	public function showAdvert():Void {
		overlay.panel._visible = true;
		overlay.panel.tween('_alpha',100,.3,'easeOutSine',5,{func:'adFadeOut',scope:this,args:[5]});
		/*
		panel._visible = true;
		panel._alpha = 0;
		panel._y=HIDDEN_POS
		panel.alphaTo(100,.3,'linear');
		
		
		panel.close_btn.onRollOver = function() {
			this.alphaTo(50,.2,'linear')
		}
		panel.close_btn.onRollOut = panel.close_btn.onReleaseOutside = function() {
			this.alphaTo(100,.2,'linear')
		}*/
	}
	
	
	public function adFadeOut(secs):Void {
		//overlay.panel.tween('_visible',false,.2,'easeOutSine',secs);
		overlay.panel._visible = false;
	}


	/** place and scale the overlay correctly **/
	private function setDimensions() {
		
		var ref = this;
		if(Stage["displayState"] == "fullScreen") {
			
			overlay.panel._x = 0;
			overlay.panel._y = (Stage.height - 40) - overlay.panel._height;
			overlay.panel.bg._width =  Stage.width;
			overlay.panel.outline._width =  Stage.width+4;
			overlay.panel.shine._width =  Stage.width-7;
			overlay.panel.shine._x = 3;
			overlay.panel.img._x = 6;
			overlay.panel.close_btn._x = Stage.width-overlay.panel.close_btn._width-4;
			overlay.panel.title_tf._x = overlay.panel.img._x+overlay.panel.img._width+3;
			overlay.panel.title_tf._width = Stage.width - (overlay.panel.img._x+overlay.panel.img._width+overlay.panel.close_btn._width+4);
			overlay.panel.description_tf._x = overlay.panel.hyperlink_tf._x =  overlay.panel.title_tf._x;
			overlay.panel.description_tf._width = Stage.width-overlay.panel.description_tf._x-6;	
			overlay.panel.hyperlink_tf._width = Stage.width-overlay.panel.hyperlink_tf._x-6;	
			
			
		} else {
			overlay.panel._x = 20;
			overlay.panel._y = config['displayheight']-overlay.panel._height;
			overlay.panel.bg._width =  config['displaywidth']-40
			overlay.panel.outline._width =config['displaywidth']-40
			overlay.panel.shine._width = overlay.panel.outline._width-7;
			overlay.panel.shine._x = 3;
			overlay.panel.img._x = 6;
			overlay.panel.close_btn._x = overlay.panel.outline._width-overlay.panel.close_btn._width-4;
			
			overlay.panel.title_tf._x = overlay.panel.img._x+overlay.panel.img._width+3;
			overlay.panel.title_tf._width = overlay.panel.outline._width - (overlay.panel.img._x+overlay.panel.img._width+overlay.panel.close_btn._width+4);
			overlay.panel.description_tf._x = overlay.panel.hyperlink_tf._x =  overlay.panel.title_tf._x;
			overlay.panel.description_tf._width = overlay.panel.outline._width-overlay.panel.description_tf._x-6;		
			overlay.panel.hyperlink_tf._width = overlay.panel.outline._width-overlay.panel.hyperlink_tf._x-6;
			
			
		}
		

		
		
		overlay.panel.close_btn.onPress = function() {
			Animations.fadeOut(ref.overlay.panel,0);
			ref.state = 3;
		};
		
		
	}


	/** Check for overlay **/
	private function setItem(itm:Number) {
	state = 1;
		
	};

// @Biffer - changed elp (Elapsed time to 10 seconds);
	/** load or unload overlay **/
	private function setTime(elp:Number,rem:Number) {
		
		if(config['Ad']=='true') {
		if(elp > Number(config['elapsed']) && state == 1) {
			state = 2;
			overlay.panel._alpha = 0;
			showAdvert()			
			Animations.fadeIn(overlay.panel,100);
		} else if (rem < 2 && state == 2) {
			Animations.fadeOut(overlay.panel,0);
			config['clip'].adPanel._visible = false;
			overlay.panel._visible = false;
			state = 3;
		} 
		
		}
	}


	/** reset the overlay when the movie is finished **/
	private function setState(stt:Number) {
		if(stt == 3 && state == 3) {
			state = 1;
			//showAdvert()
			//Animations.fadeIn(overlay.panel,100);
			overlay.panel._visible = false;
			config['clip'].adPanel._visible = false;
		}
	}


	/** OnResize Handler: catches stage resizing **/
	public function onResize() { setDimensions(); };


	/** Catches fullscreen escape  **/
	public function onFullScreen(fs:Boolean) { 
		if(fs == false) { setDimensions(); }
	};


}
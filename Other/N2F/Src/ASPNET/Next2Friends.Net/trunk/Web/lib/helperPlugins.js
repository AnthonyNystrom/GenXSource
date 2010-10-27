
	/* Copyright (c) 2006 Brandon Aaron (http://brandonaaron.net)
	 * Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php) 
	 * and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
	 *
	 * $LastChangedDate: 2007-07-21 18:44:59 -0500 (Sat, 21 Jul 2007) $
	 * $Rev: 2446 $
	 *
	 * Version 2.1.1
	 */

(function($){

	/**
	 * The bgiframe is chainable and applies the iframe hack to get 
	 * around zIndex issues in IE6. It will only apply itself in IE6 
	 * and adds a class to the iframe called 'bgiframe'. The iframe
	 * is appeneded as the first child of the matched element(s) 
	 * with a tabIndex and zIndex of -1.
	 * 
	 * By default the plugin will take borders, sized with pixel units,
	 * into account. If a different unit is used for the border's width,
	 * then you will need to use the top and left settings as explained below.
	 *
	 * NOTICE: This plugin has been reported to cause perfromance problems
	 * when used on elements that change properties (like width, height and
	 * opacity) a lot in IE6. Most of these problems have been caused by 
	 * the expressions used to calculate the elements width, height and 
	 * borders. Some have reported it is due to the opacity filter. All 
	 * these settings can be changed if needed as explained below.
	 *
	 * @example $('div').bgiframe();
	 * @before <div><p>Paragraph</p></div>
	 * @result <div><iframe class="bgiframe".../><p>Paragraph</p></div>
	 *
	 * @param Map settings Optional settings to configure the iframe.
	 * @option String|Number top The iframe must be offset to the top
	 * 		by the width of the top border. This should be a negative 
	 *      number representing the border-top-width. If a number is 
	 * 		is used here, pixels will be assumed. Otherwise, be sure
	 *		to specify a unit. An expression could also be used. 
	 * 		By default the value is "auto" which will use an expression 
	 * 		to get the border-top-width if it is in pixels.
	 * @option String|Number left The iframe must be offset to the left
	 * 		by the width of the left border. This should be a negative 
	 *      number representing the border-left-width. If a number is 
	 * 		is used here, pixels will be assumed. Otherwise, be sure
	 *		to specify a unit. An expression could also be used. 
	 * 		By default the value is "auto" which will use an expression 
	 * 		to get the border-left-width if it is in pixels.
	 * @option String|Number width This is the width of the iframe. If
	 *		a number is used here, pixels will be assume. Otherwise, be sure
	 * 		to specify a unit. An experssion could also be used.
	 *		By default the value is "auto" which will use an experssion
	 * 		to get the offsetWidth.
	 * @option String|Number height This is the height of the iframe. If
	 *		a number is used here, pixels will be assume. Otherwise, be sure
	 * 		to specify a unit. An experssion could also be used.
	 *		By default the value is "auto" which will use an experssion
	 * 		to get the offsetHeight.
	 * @option Boolean opacity This is a boolean representing whether or not
	 * 		to use opacity. If set to true, the opacity of 0 is applied. If
	 *		set to false, the opacity filter is not applied. Default: true.
	 * @option String src This setting is provided so that one could change 
	 *		the src of the iframe to whatever they need.
	 *		Default: "javascript:false;"
	 *
	 * @name bgiframe
	 * @type jQuery
	 * @cat Plugins/bgiframe
	 * @author Brandon Aaron (brandon.aaron@gmail.com || http://brandonaaron.net)
	 */
	$.fn.bgIframe = $.fn.bgiframe = function(s) {
		// This is only for IE6
		if ( $.browser.msie && /6.0/.test(navigator.userAgent) ) {
			s = $.extend({
				top     : 'auto', // auto == .currentStyle.borderTopWidth
				left    : 'auto', // auto == .currentStyle.borderLeftWidth
				width   : 'auto', // auto == offsetWidth
				height  : 'auto', // auto == offsetHeight
				opacity : true,
				src     : 'javascript:false;'
			}, s || {});
			var prop = function(n){return n&&n.constructor==Number?n+'px':n;},
			    html = '<iframe class="bgiframe"frameborder="0"tabindex="-1"src="'+s.src+'"'+
			               'style="display:block;position:absolute;z-index:-1;'+
				               (s.opacity !== false?'filter:Alpha(Opacity=\'0\');':'')+
						       'top:'+(s.top=='auto'?'expression(((parseInt(this.parentNode.currentStyle.borderTopWidth)||0)*-1)+\'px\')':prop(s.top))+';'+
						       'left:'+(s.left=='auto'?'expression(((parseInt(this.parentNode.currentStyle.borderLeftWidth)||0)*-1)+\'px\')':prop(s.left))+';'+
						       'width:'+(s.width=='auto'?'expression(this.parentNode.offsetWidth+\'px\')':prop(s.width))+';'+
						       'height:'+(s.height=='auto'?'expression(this.parentNode.offsetHeight+\'px\')':prop(s.height))+';'+
						'"/>';
			return this.each(function() {
				if ( $('> iframe.bgiframe', this).length == 0 )
					this.insertBefore( document.createElement(html), this.firstChild );
			});
		}
		return this;
	};

})(jQuery);

(function($){
	/* hoverIntent by Brian Cherne */
	$.fn.hoverIntent = function(f,g) {
		// default configuration options
		var cfg = {
			sensitivity: 7,
			interval: 100,
			timeout: 0
		};
		// override configuration options with user supplied object
		cfg = $.extend(cfg, g ? { over: f, out: g } : f );

		// instantiate variables
		// cX, cY = current X and Y position of mouse, updated by mousemove event
		// pX, pY = previous X and Y position of mouse, set by mouseover and polling interval
		var cX, cY, pX, pY;

		// A private function for getting mouse position
		var track = function(ev) {
			cX = ev.pageX;
			cY = ev.pageY;
		};

		// A private function for comparing current and previous mouse position
		var compare = function(ev,ob) {
			ob.hoverIntent_t = clearTimeout(ob.hoverIntent_t);
			// compare mouse positions to see if they've crossed the threshold
			if ( ( Math.abs(pX-cX) + Math.abs(pY-cY) ) < cfg.sensitivity ) {
				$(ob).unbind("mousemove",track);
				// set hoverIntent state to true (so mouseOut can be called)
				ob.hoverIntent_s = 1;
				return cfg.over.apply(ob,[ev]);
			} else {
				// set previous coordinates for next time
				pX = cX; pY = cY;
				// use self-calling timeout, guarantees intervals are spaced out properly (avoids JavaScript timer bugs)
				ob.hoverIntent_t = setTimeout( function(){compare(ev, ob);} , cfg.interval );
			}
		};

		// A private function for delaying the mouseOut function
		var delay = function(ev,ob) {
			ob.hoverIntent_t = clearTimeout(ob.hoverIntent_t);
			ob.hoverIntent_s = 0;
			return cfg.out.apply(ob,[ev]);
		};

		// A private function for handling mouse 'hovering'
		var handleHover = function(e) {
			// next three lines copied from jQuery.hover, ignore children onMouseOver/onMouseOut
			var p = (e.type == "mouseover" ? e.fromElement : e.toElement) || e.relatedTarget;
			while ( p && p != this ) { try { p = p.parentNode; } catch(e) { p = this; } }
			if ( p == this ) { return false; }

			// copy objects to be passed into t (required for event object to be passed in IE)
			var ev = jQuery.extend({},e);
			var ob = this;

			// cancel hoverIntent timer if it exists
			if (ob.hoverIntent_t) { ob.hoverIntent_t = clearTimeout(ob.hoverIntent_t); }

			// else e.type == "onmouseover"
			if (e.type == "mouseover") {
				// set "previous" X and Y position based on initial entry point
				pX = ev.pageX; pY = ev.pageY;
				// update "current" X and Y position based on mousemove
				$(ob).bind("mousemove",track);
				// start polling interval (self-calling timeout) to compare mouse coordinates over time
				if (ob.hoverIntent_s != 1) { ob.hoverIntent_t = setTimeout( function(){compare(ev,ob);} , cfg.interval );}

			// else e.type == "onmouseout"
			} else {
				// unbind expensive mousemove event
				$(ob).unbind("mousemove",track);
				// if hoverIntent state is true, then call the mouseOut function after the specified delay
				if (ob.hoverIntent_s == 1) { ob.hoverIntent_t = setTimeout( function(){delay(ev,ob);} , cfg.timeout );}
			}
		};

		// bind the function to the two event listeners
		return this.mouseover(handleHover).mouseout(handleHover);
	};
	
})(jQuery);
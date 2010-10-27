/*
 * Superfish v1.3.4 - jQuery menu widget
 *
 * Copyright (c) 2007 Joel Birch
 *
 * Dual licensed under the MIT and GPL licenses:
 * 	http://www.opensource.org/licenses/mit-license.php
 * 	http://www.gnu.org/licenses/gpl.html
 */

/* YOU SHOULD DELETE THIS CHANGELOG TO REDUCE FILE SIZE:
 * v1.2.1 altered:  2nd July 07. added hide() before animate to make work for jQuery 1.1.3. See comment in 'over' function.
 * v1.2.2 altered:  2nd August 07. changed over function .find('ul') to .find('>ul') for smoother animations
 * 				    Also deleted the iframe removal lines - not necessary it turns out
 * v1.2.3 altered:  jquery 1.1.3.1 broke keyboard access - had to change quite a few things and set display:none on the
 *				    .superfish rule in CSS instead of top:-999em
 * v1.3	: 		    Pretty much a complete overhaul to make all original features work in 1.1.3.1 and above.
 *				    .superfish rule reverted back to top:-999em (which is better).
 * v1.3.1 altered:  'li[ul]' to $('li:has(ul)') to work with jQuery 1.2
 * v1.3.2: 			added onshow callback option as requested - 'this' keyword refers to revealed ul.
		   			fixed bug whereby multiple menus on a page shared options. Now each menu can have separate options.
					fixed IE6 and IE7 bug whereby under certain circumstances => 3rd tier menus appear instantly with text missing when revisited
 * v1.3.3: 			altered event attachment selectors for performance increase on menu setup.
 * v1.3.4: 			fixed pathClass bug as current path was not being restored. Still doesn't if using keyboard nav (will work on that).
 */

(function($){
	$.fn.superfish = function(o){
		var $sf = this,
			defaults = {
			hoverClass	: 'sfHover',
			pathClass	: 'overideThisToUse',
			delay		: 800,
			animation	: {opacity:'show'},
			speed		: 'normal',
			onshow		: function(){} // in your function, 'this' is the revealed ul
		},
			over = function(){
				clearTimeout(this.sfTimer);
				$(this)
				.showSuperfishUl(o)
				.siblings()
				.hideSuperfishUl(o);
			},
			out = function(){
				var $$ = $(this);
				if ( !$$.is('.'+o.bcClass) ) {
					this.sfTimer=setTimeout(function(){
						$$.hideSuperfishUl(o);
						var sf = $$.parents('ul.superfish:first')[0];
						if (!$('.'+o.hoverClass,sf).length) {
							over.call(sf.o.$currents.hideSuperfishUl(o));
						}
					},o.delay);
				}		
			};
		$.fn.extend({
			hideSuperfishUl : function(o){
				$('li.'+o.hoverClass,this)
				.andSelf()
					.removeClass(o.hoverClass)
					.find('>ul')
						.hide()
						.css('visibility','hidden');
				return this;
			},
			showSuperfishUl : function(o){
				return this
					.addClass(o.hoverClass)
					.find('>ul:hidden')
						.css('visibility','visible')
						.animate(o.animation,o.speed,function(){
							o.onshow.call(this);
						})
					.end();
			},
			applySuperfishHovers : function(){
				return this[($.fn.hoverIntent) ? 'hoverIntent' : 'hover'](over,out);
			}
		});
		
		return this
		.addClass('superfish')
		.each(function(){
			o = $.extend({bcClass:'sfbreadcrumb'},defaults,o);
			o = $.extend(o,{$currents:$('li.'+o.pathClass,this)});
			this.o = o;
			
			if (o.$currents.length) {
				o.$currents.each(function(){
					$(this)
					.addClass(o.hoverClass+' '+o.bcClass)
					.filter(':has(ul)')
						.removeClass(o.pathClass);
				});
			}
			
			var $sfHovAr=$('li:has(ul)',this)
				.applySuperfishHovers(over,out)
				.not('.'+o.bcClass)
					.hideSuperfishUl(o)
				.end();
			
			$('a',this).each(function(){
				var $a = $(this), $li = $a.parents('li');
				$a.focus(function(){
					over.call($li);
					return false;
				}).blur(function(){
					$li.removeClass(o.hoverClass);
				});
			});
		});
	};
	$(window).unload(function(){
		$('ul.superfish').each(function(){
			$('li:has(ul)',this).unbind('mouseover').unbind('mouseout');
			this.o = this.o.$currents = null; // clean up
		});
	});
})(jQuery);
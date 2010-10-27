/*
 * imgAreaSelect jQuery plugin
 * version 0.2
 *
 * Copyright (c) 2008 Michal Wojciechowski (odyniec.net)
 *
 * Dual licensed under the MIT (MIT-LICENSE.txt) 
 * and GPL (GPL-LICENSE.txt) licenses.
 *
 * http://odyniec.net/projects/imgareaselect/
 *
 */

jQuery.imgAreaSelect = function (img, options) {
    var $overlay = jQuery('<div id="overlay"></div>'); //what is it?
    var $area = jQuery('<div id="area"></div>'); //what is it?
    var $border1 = jQuery('<div id="border1"></div>'); //what is it?
    var $border2 = jQuery('<div id="border2"></div>'); //what is it?
    var imgOfs, imgWidth, imgHeight; //what is the imgIfs
    var startX, startY, moveX, moveY; //What are they
    var resizeMargin = 10; //What is it?
    var resize = [ ]; //what is it?
    var V = 0, H = 1; //what are they
    var aspectRatio;
    var x1, x2, y1, y2;
    var selection = { x1: 0, y1: 0, x2: 0, y2: 0, width: 0, height: 0 };
  // var selection = { x1: 5, y1: 10, x2: 100, y2: 100, width: 100, height: 100 };
  //x1=10;x2=10;y1=110;y2=110;
   //what you have when it's selection on page? an area, borders,overlay in 
   //right size and right dimensions
   //but on preview can be tricky 
   
    //This function  just set current cursor and intilize 
    function areaMouseMove(event)
    {
    //pageX mouse current x position selection x1 it's a resize
        var x = event.pageX - selection.x1 - imgOfs.left;
        var y = event.pageY - selection.y1 - imgOfs.top;

        if (options.resizable) {
            resize = [ ];
        //V and H stand for horiszontal and vertical    
            if (y <= resizeMargin)
                resize[V] = 'n';
            else if (y >= selection.height - resizeMargin)
                resize[V] = 's';
            if (x <= resizeMargin)
                resize[H] = 'w';
            else if (x >= selection.width - resizeMargin)
                resize[H] = 'e';
                
            $border2.css('cursor', resize.length ? resize.join('') + '-resize' :
                options.movable ? 'move' : '');
        }
    }

    
    function areaMouseDown(event)
    {
        //if it's not generated by keybored
        if (event.which != 1) return false;
        //if resize happend in areaMouseMove overlay got cursor too
        if (resize.length > 0) {
            jQuery($overlay).css('cursor', resize.join('') + '-resize');

            x1 = (resize[H] == 'w' ? selection.x2 : selection.x1) + imgOfs.left;
            y1 = (resize[V] == 'n' ? selection.y2 : selection.y1) + imgOfs.top;
            // i think it show a and it's happend only when I am resizing
            $overlay.show();
            
            jQuery(document).mousemove(selectingMouseMove);
            $border2.unbind('mousemove', areaMouseMove);

         	jQuery(document).one('mouseup', function () {
          	    resize = [ ];

          	    $overlay.css('cursor', '').hide();

          		if (options.autoHide)
       	            $area.add($border1).add($border2).hide();

      	        options.onSelectEnd(img, selection);
      	        
     	        jQuery(document).unbind('mousemove', selectingMouseMove);
       	        $border2.mousemove(areaMouseMove);
       	    });
        }
        else if (options.movable) {
        // real cordinate selection.x1 + imgOfs.left
            moveX = selection.x1 + imgOfs.left;
            moveY = selection.y1 + imgOfs.top;
            startX = event.pageX;
            startY = event.pageY;
            
            jQuery(document)
                .mousemove(movingMouseMove)
                .one('mouseup', function () {
          	        options.onSelectEnd(img, selection);
          	        
           	        jQuery(document).unbind('mousemove', movingMouseMove);
           	    });
        }
        else
            jQuery(img).mousedown(event);

        return false;
    }

    function aspectRatioXY()
    {
        x2 = Math.max(imgOfs.left, Math.min(imgOfs.left + imgWidth,
            x1 + Math.abs(y2 - y1) * aspectRatio * (x2 > x1 ? 1 : -1)));
        y2 = Math.round(Math.max(imgOfs.top, Math.min(imgOfs.top + imgHeight,
            y1 + Math.abs(x2 - x1) / aspectRatio * (y2 > y1 ? 1 : -1))));
        x2 = Math.round(x2);
    }

    function aspectRatioYX()
    {
        y2 = Math.max(imgOfs.top, Math.min(imgOfs.top + imgHeight,
            y1 + Math.abs(x2 - x1) / aspectRatio * (y2 > y1 ? 1 : -1)));
        x2 = Math.round(Math.max(imgOfs.left, Math.min(imgOfs.left + imgWidth,
            x1 + Math.abs(y2 - y1) * aspectRatio * (x2 > x1 ? 1 : -1))));
        y2 = Math.round(y2);
    }

    function selectingMouseMove(event)
    {
        x2 = !resize.length || resize[H] || aspectRatio ? event.pageX : selection.x2 + imgOfs.left;
        y2 = !resize.length || resize[V] || aspectRatio ? event.pageY : selection.y2 + imgOfs.top;
        x2 = Math.max(imgOfs.left, Math.min(x2, imgOfs.left + imgWidth));
        y2 = Math.max(imgOfs.top, Math.min(y2, imgOfs.top + imgHeight));

        if (aspectRatio)
            if (Math.abs(x2 - x1) / aspectRatio > Math.abs(y2 - y1))
                aspectRatioYX();
            else
                aspectRatioXY();

        if (options.maxWidth && Math.abs(x2 - x1) > options.maxWidth) {
            x2 = x1 - options.maxWidth * (x2 < x1 ? 1 : -1);
            if (aspectRatio) aspectRatioYX();
        }
        
        if (options.maxHeight && Math.abs(y2 - y1) > options.maxHeight) {
            y2 = y1 - options.maxHeight * (y2 < y1 ? 1 : -1);
            if (aspectRatio) aspectRatioXY();
        }
       /// Add min functionality  
        if (options.minWidth && Math.abs(x2 - x1) < options.minWidth) {
            x2 = x1 - options.minWidth * (x2 < x1 ? 1 : -1);
            if (aspectRatio) aspectRatioYX();
        }
        
        if (options.minHeight && Math.abs(y2 - y1) < options.minHeight) {
            y2 = y1 - options.minHeight * (y2 < y1 ? 1 : -1);
            if (aspectRatio) aspectRatioXY();
        }

        selection.x1 = Math.min(x1, x2) - imgOfs.left;
        selection.x2 = Math.max(x1, x2) - imgOfs.left;
        selection.y1 = Math.min(y1, y2) - imgOfs.top;
        selection.y2 = Math.max(y1, y2) - imgOfs.top;
        selection.width = Math.abs(x2 - x1);
        selection.height = Math.abs(y2 - y1);

        $area.add($border1).add($border2).css({
            left: (selection.x1 + imgOfs.left) + 'px',
            top: (selection.y1 + imgOfs.top) + 'px',
            width: Math.max(selection.width - options.borderWidth * 2, options.minWidth) + 'px',
            height: Math.max(selection.height - options.borderWidth * 2, options.minHeight) + 'px'
        });

        options.onSelectChange(img, selection);

        return false;        
    }

    function movingMouseMove(event)
    {
        x1 = Math.max(imgOfs.left, Math.min(moveX + event.pageX - startX,
            imgOfs.left + imgWidth - selection.width));
        y1 = Math.max(imgOfs.top, Math.min(moveY + event.pageY - startY,
            imgOfs.top + imgHeight - selection.height));
        x2 = x1 + selection.width;
        y2 = y1 + selection.height;

        selection.x1 = x1 - imgOfs.left;
        selection.y1 = y1 - imgOfs.top;
        selection.x2 = x2 - imgOfs.left;
        selection.y2 = y2 - imgOfs.top;
      
      $area.add($border1).add($border2).css({
            left: x1 + 'px',
            top: y1 + 'px',
     		width: Math.max(x2 - x1 - options.borderWidth * 2, options.minWidth) + 'px',
     		height: Math.max(y2 - y1 - options.borderWidth * 2, options.minHeight) + 'px'
        });

        options.onSelectChange(img, selection);

        event.preventDefault();

        return false;
    }

    if (options.aspectRatio) {
        var dim = options.aspectRatio.split(/:/);
        aspectRatio = dim[0] / dim[1];
    }

    imgWidth = jQuery(img).width();
    imgHeight = jQuery(img).height();
    imgOfs = jQuery(img).offset();

    $overlay.css({ left: '0px', zIndex: 10, display: 'none' });

    if (jQuery.browser.msie) {
        jQuery(img).attr('unselectable', 'on');
        
        if (jQuery.browser.version == '6.0') {
            $overlay.css('position', 'absolute');
            $overlay.get(0).style.setExpression("width", "document.documentElement.clientWidth + 'px'");
            $overlay.get(0).style.setExpression("height", "document.documentElement.clientHeight + 'px'");
            $overlay.get(0).style.setExpression("top", "document.documentElement.scrollTop + " +
                "(0 + parseInt(document.body.currentStyle.paddingTop) + " +
                "parseInt(document.body.currentStyle.marginTop)) + 'px'");
        }
        else
            $overlay.css({ position: 'fixed', overflow: 'hidden', top: '0px', width: '100%', height: '100%' });
    }

    $area.add($border1).add($border2).css({
        display: 'none',
        position: 'absolute',
        lineHeight: '0px',
        fontSize: '0px',
        borderWidth: options.borderWidth + 'px'
    });    
    $area.css({
        borderStyle: 'solid',
        backgroundColor: options.selectionColor, 
        opacity: options.selectionOpacity
    });       
    $border1.css({ borderStyle: 'solid', borderColor: options.borderColor1 });
    $border2.css({ borderStyle: 'dashed', borderColor: options.borderColor2 });

    $overlay.add($area).add($border1).add($border2).appendTo(jQuery('body'));    

    if (options.resizable || options.movable)
        $border2.mousemove(areaMouseMove).mousedown(areaMouseDown);
        
     ////////////
     //   $overlay.show();
//            $area.add($border1).add($border2).css({
//            left: (10 + imgOfs.left) + 'px',
//            top: (10 + imgOfs.top) + 'px',
//            width: Math.max(selection.width - options.borderWidth * 2, 200) + 'px',
//            height: Math.max(selection.height - options.borderWidth * 2, 300) + 'px'
//        });
//        $area.add($border1).add($border2).show();
//      //  $overlay.hide();
//        ///////////
        ////////////
//          if (aspectRatio)
//          {
//                 options.width=aspectRatio*options.height;
//          }
         //  $overlay.show();
            $area.add($border1).add($border2).css({
            left: (options.left + imgOfs.left) + 'px',
            top: (options.top + imgOfs.top) + 'px',
            width: Math.max(options.width - options.borderWidth * 2, options.minWidth) + 'px',
            height: Math.max(options.height - options.borderWidth * 2, options.minHeight) + 'px'
        });
      //$area.add($border1).add($border2).show();
      $overlay.add($area).add($border1).add($border2).show();
    //$area.show();
      //  $overlay.hide();
        ///////////
        

        startX = options.left;
        startY = options.top;
        
        x1 = options.left;
        y1 = options.top;
        
        //event.x
        selection.x1=x1;
        selection.y1=y1;
        
        selection.width=options.width;
        selection.height=options.height;
        
        selection.x2=selection.x1+options.width;
        selection.y2=selection.x2+options.height;
        
         options.onSelectChange(img, selection);
         resize = [ ];
          
                                   
 // jQuery(document).unbind('mousemove', selectingMouseMove);
       //   $border2.mousemove(areaMouseMove);
       
        //  $border2.trigger("mousedown");
            
      //     $overlay.hide();      
                
        //selection.x1 = selection.x2 = event.pageX - imgOfs.left;
///selection.y1 = selection.y2 = event.pageY - imgOfs.top;
       //       $border2.unbind('mousemove', areaMouseMove);
        
        //  jQuery(document).unbind('mousemove', selectingMouseMove);
         //   $border2.mousemove(areaMouseMove);
        //  event.preventDefault();
        
        
        
         ///////////////---------------------//////////  
          ///////////////---------------------//////////  
 ///////////////---------------------//////////       
         
    jQuery(img).mousedown(function (event) {
    ///
    return false;
        if (event.which != 1) return false;

        startX = event.pageX;
        startY = event.pageY;

        x1 = event.pageX;
        y1 = event.pageY;
            
        resize = [ ];

        $overlay.show();
        $area.add($border1).add($border2).show().css({
            width: '0px',
            height: '0px',
            left: startX,
            top: startY
        });
            
        jQuery(document).mousemove(selectingMouseMove);
        $border2.unbind('mousemove', areaMouseMove);

        selection.x1 = selection.x2 = event.pageX - imgOfs.left;
        selection.y1 = selection.y2 = event.pageY - imgOfs.top;

        options.onSelectStart(img, selection);
//        ////////////
//       
//            $overlay.show();
//            $area.add($border1).add($border2).css({
//            left: (options.left + imgOfs.left) + 'px',
//            top: (options.top + imgOfs.top) + 'px',
//            width: Math.max(options.width - options.borderWidth * 2, options.minWidth) + 'px',
//            height: Math.max(options.height - options.borderWidth * 2, options.minHeight) + 'px'
//        });
//        $area.add($border1).add($border2).show();
//      //  $overlay.hide();
//        ///////////

//     ////////////
//         $overlay.show();
//            $area.add($border1).add($border2).css({
//            left: (10 + imgOfs.left) + 'px',
//            top: (10 + imgOfs.top) + 'px',
//            width: Math.max(selection.width - options.borderWidth * 2, 200) + 'px',
//            height: Math.max(selection.height - options.borderWidth * 2, 300) + 'px'
//        });
//        $area.add($border1).add($border2).show();
//        $overlay.hide();
//        ///////////
//        ////////////


   $area.add($border1).add($border2).css({
            left: (options.left + imgOfs.left) + 'px',
            top: (options.top + imgOfs.top) + 'px',
            width: Math.max(options.width - options.borderWidth * 2, options.minWidth) + 'px',
            height: Math.max(options.height - options.borderWidth * 2, options.minHeight) + 'px'
        });
      //$area.add($border1).add($border2).show();
      $overlay.add($area).add($border1).add($border2).show();

        jQuery(document).one('mouseup', function () {
            $overlay.hide();

            if (options.autoHide)
                $area.add($border1).add($border2).hide();

            options.onSelectEnd(img, selection);

            jQuery(document).unbind('mousemove', selectingMouseMove);
            $border2.mousemove(areaMouseMove);
        });

        return false;
    });
};

jQuery.fn.imgAreaSelect = function (options) {
    options = options || {};
    options.borderColor1 = options.borderColor1 || '#000';
    options.borderColor2 = options.borderColor2 || '#fff';
    options.borderWidth = options.borderWidth || 1;
    options.movable = !(options.movable == false);
    options.resizable = !(options.resizable == false);
    options.selectionColor = options.selectionColor || '#fff';
    options.selectionOpacity = options.selectionOpacity || 0.2;
    options.onSelectStart = options.onSelectStart || function () {};
    options.onSelectChange = options.onSelectChange || function () {};
    options.onSelectEnd = options.onSelectEnd || function () {};

    this.each(function () {
        new jQuery.imgAreaSelect(this, options);
    });

    return this;
};

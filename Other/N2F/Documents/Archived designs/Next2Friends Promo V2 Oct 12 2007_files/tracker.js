function w3counter(id) {
	var info;
	info = '&userAgent=' + escape(navigator.userAgent);

	if (encodeURIComponent) {
	
		info = info + '&webpageName=' + encodeURIComponent(document.title);
        info = info + '&ref=' + encodeURIComponent(document.referrer);
        
 		if (typeof _w3counter_label != 'undefined') {
 			info = info + '&label=' + encodeURIComponent(_w3counter_label);
 		}
 
	} else {
	
		info = info + '&webpageName=' + escape(document.title);
		info = info + '&ref=' + escape(document.referrer);
		
 		if (typeof _w3counter_label != 'undefined') {
 			info = info + '&label=' + escape(_w3counter_label);
 		}
 		
	}

	info = info + '&url=' + escape(window.location);
	info = info + '&width=' + screen.width;
	info = info + '&height=' + screen.height;
	info = info + '&depth=' + screen.colorDepth;

	document.write('<a href="http://www.w3counter.com"><img src="http://www.w3counter.com/tracker.php?id=' + id + info + '" style="border: 0" alt="W3Counter Web Stats" /></a>');
}

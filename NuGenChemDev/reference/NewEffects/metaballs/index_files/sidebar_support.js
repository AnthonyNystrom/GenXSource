// Sidebar JavaScript code

/*
 * Included code from some book or other.
 */

// utility function to retrieve an expiration date in proper
// format; pass three integer parameters for the number of days, hours,
// and minutes from now you want the cookie to expire (or negative
// values for a past date); all three parameters are required,
// so use zeros where appropriate
function getExpDate(days, hours, minutes) {
    var expDate = new Date( );
    if (typeof days == "number" && typeof hours == "number" && 
        typeof hours == "number") {
        expDate.setDate(expDate.getDate( ) + parseInt(days));
        expDate.setHours(expDate.getHours( ) + parseInt(hours));
        expDate.setMinutes(expDate.getMinutes( ) + parseInt(minutes));
        return expDate.toGMTString( );
    }
}
   
// utility function called by getCookie( )
function getCookieVal(offset) {
    var endstr = document.cookie.indexOf (";", offset);
    if (endstr == -1) {
        endstr = document.cookie.length;
    }
    return unescape(document.cookie.substring(offset, endstr));
}
   
// primary function to retrieve cookie by name
function getCookie(name) {
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var i = 0;
    while (i < clen) {
        var j = i + alen;
        if (document.cookie.substring(i, j) == arg) {
            return getCookieVal(j);
        }
        i = document.cookie.indexOf(" ", i) + 1;
        if (i == 0) break; 
    }
    return "";
}
   
// store cookie value with optional details as needed
function setCookie(name, value, expires, path, domain, secure) {
    document.cookie = name + "=" + escape (value) +
        ((expires) ? "; expires=" + expires : "") +
        ((path) ? "; path=" + path : "") +
        ((domain) ? "; domain=" + domain : "") +
        ((secure) ? "; secure" : "");
}
   
// remove the cookie by setting ancient expiration date
function deleteCookie(name,path,domain) {
    if (getCookie(name)) {
        document.cookie = name + "=" +
            ((path) ? "; path=" + path : "") +
            ((domain) ? "; domain=" + domain : "") +
            "; expires=Thu, 01-Jan-70 00:00:01 GMT";
    }
}

/*
 * END included book code.
 */

// stuff specific to the exaflop.org sidebar
function switchMenu(divname,bname) { 
	var el = document.getElementById(divname);
	var btn = document.getElementById(bname);

	if ( el.style.display != "none" ) { 
		el.style.display = 'none'; 
		btn.src="/i/exbut_p.gif"; 
		deleteCookie("side-doc-open", "/"); 
	} else { 
		el.style.display = ''; 
		btn.src="/i/exbut_m.gif"; 
		setCookie("side-doc-open", "yes", getExpDate(10,0,0), "/"); 
	}
}
function setupMenu() {
	// run on page load
	if (document.images)
	{
		var preload = new Image;
		preload.src="/i/exbut_m.gif";
	}
	var h = getCookie("side-doc-open");
	if (h == "yes") {
		switchMenu('document-subfolders','document-expander');
	}
}



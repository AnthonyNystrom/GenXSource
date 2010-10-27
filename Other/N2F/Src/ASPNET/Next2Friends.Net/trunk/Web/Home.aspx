<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>VideoSlider test page</title>
<script type="text/javascript" language="javascript">AC_FL_RunContent = 0;</script>
<script type="text/javascript" src="AC_RunActiveContent.js" language="javascript"></script>
<script type="text/javascript" src="N2F.VideoSlider.js" language="javascript"></script>
</head>
<body bgcolor="#C0C0FF">
<form runat="server" id="form1">
<script language="javascript">

	function initialize()
	{
		setTimeout( function()
		{
			var plugin = document.getElementById( "Main" );
			if ( !plugin ) plugin = document.all[ "Main" ];

			videoSlider.init( plugin, function( item )
			{
				alert( "You selected: '" + item.title + "'" );
			} );

			<%=LiveStreamJS %>
		}, 0 );

		return [];
	}

	if (AC_FL_RunContent == 0) {
		alert("This page requires AC_RunActiveContent.js.");
	} else {
		AC_FL_RunContent(
			'codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0',
			'width', '500',
			'height', '150',
			'src', 'N2F.VideoSlider',
			'quality', 'high',
			'pluginspage', 'http://www.macromedia.com/go/getflashplayer',
			'align', 'middle',
			'play', 'true',
			'loop', 'true',
			'scale', 'showall',
			'wmode', 'transparent',
			'devicefont', 'false',
			'id', 'Main',
			'bgcolor', '#ffffff',
			'name', 'Main',
			'menu', 'true',
			'allowFullScreen', 'false',
			'allowScriptAccess','always',
			'movie', 'N2F.VideoSlider',
			'salign', '',
			'flashvars', 'initMethod=initialize&selectMethod=selectThumbnail'
			); //end AC code
	}
</script>
<noscript>
	<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0" width="500" height="150" id="Main" align="middle">
		<param name="allowScriptAccess" value="always" />
		<param name="allowFullScreen" value="false" />
		<param name="wmode" value="transparent" />
		<param name="flashvars" value="initMethod=initialize&selectMethod=selectThumbnail" />
		<param name="movie" value="N2F.VideoSlider.swf" /><param name="quality" value="high" /><param name="bgcolor" value="#ffffff" />
		<embed src="N2F.VideoSlider.swf" quality="high" bgcolor="#ffffff" width="500" height="150" name="Main" align="middle" allowScriptAccess="always" allowFullScreen="false" wmode="transparent" flashvars="initMethod=initialize&selectMethod=selectThumbnail" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
	</object>
</noscript>

<div>
	<button onclick='videoSlider.add( "http://farm2.static.flickr.com/1035/945390013_7e3b3ab61f.jpg", "http://farm2.static.flickr.com/1035/945390013_7e3b3ab61f.jpg", "American Falls at night #1", false, false )'>Add image</button>
	<button onclick='videoSlider.insert( 0, "http://farm2.static.flickr.com/1035/945390013_7e3b3ab61f.jpg", "http://farm2.static.flickr.com/1035/945390013_7e3b3ab61f.jpg", "American Falls at night #1", false, false )'>Insert image</button>
</div>
</form>
</body>
</html>

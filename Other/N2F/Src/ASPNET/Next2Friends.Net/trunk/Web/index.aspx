<%@ Page Language="C#" MasterPageFile="main.master" Debug="true" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="IndexPage" %>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<link rel="stylesheet" type="text/css" href="/jcarousel-css/jquery.jcarousel.css" />
<link rel="stylesheet" type="text/css" href="/jcarousel-css/skin.css" />
<script type="text/javascript" src="/lib/jquery.jcarousel.pack.js"></script>
<script type="text/javascript" src="/lib/swfobject.js" language="javascript"></script>
<script type="text/javascript" src="/js/index.js?c=14"></script>
<script type="text/javascript" src="/lib/popup.js"></script>
	<script type="text/javascript" src="/SWFObject2.js" language="javascript"></script>
	<script type="text/javascript" src="/lib/N2F.VideoSlider.js" language="javascript"></script>


	<!-- middle start -->
	<div id="middle" class="no_subnav clearfix">

	<div id="Main" style="height:400px">
	<br/>
	<strong>
		To continue, you need to ensure Javascript is enabled and may need to upgrade your Flash Player.
		You can download the latest version <a href="http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash">here</a>.
	</strong>
    </div>
<script language="javascript">

	var flashvars = {
		initMethod: "initialize",
		selectMethod: "selectThumbnail",
		traceMethod: "OnDebug",
		zoomDistance: 110,
		browseDistance: 360,
		nickname: "Anthony"
	};

	var params = {
		menu: "false",
		scale: "noScale",
		wmode: "transparent",
//		quality: "high",
		allowScriptAccess: "always",
		allowFullScreen: "true"
	};

	swfobject.embedSWF( "N2F.VideoWall.swf", "Main", "100%", "400", "9.0.0", "expressInstall.swf", flashvars, params );

    function initialize()
    {
        setTimeout( function( event )
        {
            var plugin = document.getElementById( "Main" );
            if ( !plugin ) plugin = document.all[ "Main" ];

            videoSlider.init( plugin, function( item )
            {
                if(item.uniqueId.indexOf('_')>-1)
				{
					setTitle(item.title,false);
                    player(item.url,"",true,false,null);
                    setWatched(item.uniqueId);
                }else{
					setTitle(item.title,true);
                    player(item.uniqueId,"",true,true,item.uniqueId);
                }
            } );

            videoSlider.insert(0, '_ZDkxNzBkZjIwNjU0NDkxZD', 'http://www.next2friends.com/user//Olibs/video/ZDkxNzBkZjIwNjU0NDkxZD.flv', 'http://www.next2friends.com/user//Olibs/vthmb/ZDkxNzBkZjIwNjU0NDkxZD.jpg', 'olibs:GB', false, false );videoSlider.insert(0, '_ODZlOWU3OGU2OTJkNDQwMW', 'http://www.next2friends.com/user//lukicweb/video/ODZlOWU3OGU2OTJkNDQwMW.flv', 'http://www.next2friends.com/user//lukicweb/vthmb/ODZlOWU3OGU2OTJkNDQwMW.jpg', 'lukicweb:CS', false, false );videoSlider.insert(0, '_ZDA4OTYyMjg5NWYyNDMwYT', 'http://www.next2friends.com/user//t3chDzyn/video/ZDA4OTYyMjg5NWYyNDMwYT.flv', 'http://www.next2friends.com/user//t3chDzyn/vthmb/ZDA4OTYyMjg5NWYyNDMwYT.jpg', 't3chDzyn:US', false, false );videoSlider.insert(0, '_MmRjYzE4NjJjN2FhNGYzYW', 'http://www.next2friends.com/user//simmy/video/MmRjYzE4NjJjN2FhNGYzYW.flv', 'http://www.next2friends.com/user//simmy/vthmb/MmRjYzE4NjJjN2FhNGYzYW.jpg', 'simmy:US', false, false );videoSlider.insert(0, '_MmIwODhkMjQ2YjA2NDNhMz', 'http://www.next2friends.com/user//lukicweb/video/MmIwODhkMjQ2YjA2NDNhMz.flv', 'http://www.next2friends.com/user//lukicweb/vthmb/MmIwODhkMjQ2YjA2NDNhMz.jpg', 'lukicweb:CS', false, false );videoSlider.insert(0, '_MThkODZhNDAxMDJjNDU5Yz', 'http://www.next2friends.com/user//lukicweb/video/MThkODZhNDAxMDJjNDU5Yz.flv', 'http://www.next2friends.com/user//lukicweb/vthmb/MThkODZhNDAxMDJjNDU5Yz.jpg', 'lukicweb:CS', false, false );videoSlider.insert(0, '_NzE2MzFhNzU1MTg1NGI1Nj', 'http://www.next2friends.com/user//ljacob66/video/NzE2MzFhNzU1MTg1NGI1Nj.flv', 'http://www.next2friends.com/user//ljacob66/vthmb/NzE2MzFhNzU1MTg1NGI1Nj.jpg', 'ljacob66:US', false, false );videoSlider.insert(0, '_NDRjNWQzMzAwOWU1NDYwMT', 'http://www.next2friends.com/user//HCDemon/video/NDRjNWQzMzAwOWU1NDYwMT.flv', 'http://www.next2friends.com/user//HCDemon/vthmb/NDRjNWQzMzAwOWU1NDYwMT.jpg', 'HCDemon:US', false, false );videoSlider.insert(0, '_M2VmMDc1ODYxOTc4NDI3Ym', 'http://www.next2friends.com/user//ljacob66/video/M2VmMDc1ODYxOTc4NDI3Ym.flv', 'http://www.next2friends.com/user//ljacob66/vthmb/M2VmMDc1ODYxOTc4NDI3Ym.jpg', 'ljacob66:US', false, false );videoSlider.insert(0, '_NGJkMDAyZWIxM2Y2NGI4OT', 'http://www.next2friends.com/user//HCDemon/video/NGJkMDAyZWIxM2Y2NGI4OT.flv', 'http://www.next2friends.com/user//HCDemon/vthmb/NGJkMDAyZWIxM2Y2NGI4OT.jpg', 'HCDemon:US', false, false );videoSlider.insert(0, '_OWIyYzRhZDE1ZDk3NDEwNG', 'http://www.next2friends.com/user//t3chDzyn/video/OWIyYzRhZDE1ZDk3NDEwNG.flv', 'http://www.next2friends.com/user//t3chDzyn/vthmb/OWIyYzRhZDE1ZDk3NDEwNG.jpg', 't3chDzyn:US', false, false );videoSlider.insert(0, '_NTY3OGRhOGE1Yjc3NDc5ZW', 'http://www.next2friends.com/user//t3chDzyn/video/NTY3OGRhOGE1Yjc3NDc5ZW.flv', 'http://www.next2friends.com/user//t3chDzyn/vthmb/NTY3OGRhOGE1Yjc3NDc5ZW.jpg', 't3chDzyn:US', false, false );/*videoSlider.insert(0, '_Y2FkM2MyYzkyOWY1NDJiZm', 'http://www.next2friends.com/user//Anthony/video/Y2FkM2MyYzkyOWY1NDJiZm.flv', 'http://www.next2friends.com/user//Anthony/vthmb/Y2FkM2MyYzkyOWY1NDJiZm.jpg', 'Anthony:US', false, false );videoSlider.insert(0, '_ZjRiMzRjZWQxZjM5NDQ3ZD', 'http://www.next2friends.com/user//christiaan/video/ZjRiMzRjZWQxZjM5NDQ3ZD.flv', 'http://www.next2friends.com/user//christiaan/vthmb/ZjRiMzRjZWQxZjM5NDQ3ZD.jpg', 'christiaan:US', false, false );videoSlider.insert(0, '_MmY1ZGJmOWQ0YTA4NDU5MW', 'http://www.next2friends.com/user//t3chDzyn/video/MmY1ZGJmOWQ0YTA4NDU5MW.flv', 'http://www.next2friends.com/user//t3chDzyn/vthmb/MmY1ZGJmOWQ0YTA4NDU5MW.jpg', 't3chDzyn:US', false, false );videoSlider.insert(0, '_YWNiZGFlZjBhY2VhNGZmMj', 'http://www.next2friends.com/user//tonydean49/video/YWNiZGFlZjBhY2VhNGZmMj.flv', 'http://www.next2friends.com/user//tonydean49/vthmb/YWNiZGFlZjBhY2VhNGZmMj.jpg', 'tonydean49:US', false, false );videoSlider.insert(0, '_YjUyOTU4NDY4NWMwNGI4Nz', 'http://www.next2friends.com/user//tonydean49/video/YjUyOTU4NDY4NWMwNGI4Nz.flv', 'http://www.next2friends.com/user//tonydean49/vthmb/YjUyOTU4NDY4NWMwNGI4Nz.jpg', 'tonydean49:US', false, false );videoSlider.insert(0, '_OTdhZDU1YTk5ZDdjNDNkYW', 'http://www.next2friends.com/user//benzworm/video/OTdhZDU1YTk5ZDdjNDNkYW.flv', 'http://www.next2friends.com/user//benzworm/vthmb/OTdhZDU1YTk5ZDdjNDNkYW.jpg', 'benzworm:US', false, false );videoSlider.insert(0, '_ZmZiMjViMDM5MjM4NDA0Mz', 'http://www.next2friends.com/user//nts3rd/video/ZmZiMjViMDM5MjM4NDA0Mz.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/ZmZiMjViMDM5MjM4NDA0Mz.jpg', 'nts3rd:US', false, false );videoSlider.insert(0, '_OTkwOGZlNWU3NzhhNDg1NT', 'http://www.next2friends.com/user//nts3rd/video/OTkwOGZlNWU3NzhhNDg1NT.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/OTkwOGZlNWU3NzhhNDg1NT.jpg', 'nts3rd:US', false, false );videoSlider.insert(0, '_MWIxY2YxZDQ5ZWExNDFkMD', 'http://www.next2friends.com/user//nts3rd/video/MWIxY2YxZDQ5ZWExNDFkMD.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/MWIxY2YxZDQ5ZWExNDFkMD.jpg', 'nts3rd:US', false, false );videoSlider.insert(0, '_NTFlNTc3ODdlYjdkNDE3ZG', 'http://www.next2friends.com/user//nts3rd/video/NTFlNTc3ODdlYjdkNDE3ZG.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/NTFlNTc3ODdlYjdkNDE3ZG.jpg', 'nts3rd:US', false, false );videoSlider.insert(0, '_NTNkMzQwMjQwOTlmNDU0ZG', 'http://www.next2friends.com/user//dougsekus/video/NTNkMzQwMjQwOTlmNDU0ZG.flv', 'http://www.next2friends.com/user//dougsekus/vthmb/NTNkMzQwMjQwOTlmNDU0ZG.jpg', 'DougSekus:US', false, false );videoSlider.insert(0, '_OWE5ZWVhMWE0NTNlNGI0NG', 'http://www.next2friends.com/user//Genius/video/OWE5ZWVhMWE0NTNlNGI0NG.flv', 'http://www.next2friends.com/user//Genius/vthmb/OWE5ZWVhMWE0NTNlNGI0NG.jpg', 'Genius:IT', false, false );videoSlider.insert(0, '_YjFiYmFkZmRhMWQyNDllMG', 'http://www.next2friends.com/user//Genius/video/YjFiYmFkZmRhMWQyNDllMG.flv', 'http://www.next2friends.com/user//Genius/vthmb/YjFiYmFkZmRhMWQyNDllMG.jpg', 'Genius:IT', false, false );videoSlider.insert(0, '_OGJmYWNjNzcwNzg2NDM0NG', 'http://www.next2friends.com/user//t3chDzyn/video/OGJmYWNjNzcwNzg2NDM0NG.flv', 'http://www.next2friends.com/user//t3chDzyn/vthmb/OGJmYWNjNzcwNzg2NDM0NG.jpg', 't3chDzyn:US', false, false );videoSlider.insert(0, '_MTJlYjYyMzc1NzZjNDc4NW', 'http://www.next2friends.com/user//Genius/video/MTJlYjYyMzc1NzZjNDc4NW.flv', 'http://www.next2friends.com/user//Genius/vthmb/MTJlYjYyMzc1NzZjNDc4NW.jpg', 'Genius:IT', false, false );videoSlider.insert(0, '_ZTA3YTU4MTYzZDg0NDRkMD', 'http://www.next2friends.com/user//nts3rd/video/ZTA3YTU4MTYzZDg0NDRkMD.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/ZTA3YTU4MTYzZDg0NDRkMD.jpg', 'nts3rd:US', false, false );videoSlider.insert(0, '_OTllZmM3NzgzMmU5NGJiMj', 'http://www.next2friends.com/user//nts3rd/video/OTllZmM3NzgzMmU5NGJiMj.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/OTllZmM3NzgzMmU5NGJiMj.jpg', 'nts3rd:US', false, false );videoSlider.insert(0, '_NzU4YjM2MGUwZGMyNGZiYT', 'http://www.next2friends.com/user//nts3rd/video/NzU4YjM2MGUwZGMyNGZiYT.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/NzU4YjM2MGUwZGMyNGZiYT.jpg', 'nts3rd:US', false, false );videoSlider.insert(0, '_ZDQxYjMwZDViMjk4NDAzNW', 'http://www.next2friends.com/user//perkonis/video/ZDQxYjMwZDViMjk4NDAzNW.flv', 'http://www.next2friends.com/user//perkonis/vthmb/ZDQxYjMwZDViMjk4NDAzNW.jpg', 'perkonis:US', false, false );videoSlider.insert(0, '_NTUzNjA0NmE4MjBiNDYyYz', 'http://www.next2friends.com/user//perkonis/video/NTUzNjA0NmE4MjBiNDYyYz.flv', 'http://www.next2friends.com/user//perkonis/vthmb/NTUzNjA0NmE4MjBiNDYyYz.jpg', 'perkonis:US', false, false );videoSlider.insert(0, '_M2M2ODczNTMxOGM4NGQ5MW', 'http://www.next2friends.com/user//heatherdwn/video/M2M2ODczNTMxOGM4NGQ5MW.flv', 'http://www.next2friends.com/user//heatherdwn/vthmb/M2M2ODczNTMxOGM4NGQ5MW.jpg', 'HeatherDwn:US', false, false );videoSlider.insert(0, '_NzdkYWIzZTI5OWY3NGFjM2', 'http://www.next2friends.com/user//Imdrunk/video/NzdkYWIzZTI5OWY3NGFjM2.flv', 'http://www.next2friends.com/user//Imdrunk/vthmb/NzdkYWIzZTI5OWY3NGFjM2.jpg', 'imdrunk:US', false, false );videoSlider.insert(0, '_MzMzMjRjYzhlM2EzNDkwMj', 'http://www.next2friends.com/user//nene/video/MzMzMjRjYzhlM2EzNDkwMj.flv', 'http://www.next2friends.com/user//nene/vthmb/MzMzMjRjYzhlM2EzNDkwMj.jpg', 'NeNe:US', false, false );videoSlider.insert(0, '_Y2VkZDFkNzU1MDgyNGY5ZW', 'http://www.next2friends.com/user//markjnj/video/Y2VkZDFkNzU1MDgyNGY5ZW.flv', 'http://www.next2friends.com/user//markjnj/vthmb/Y2VkZDFkNzU1MDgyNGY5ZW.jpg', 'markjnj:US', false, false );videoSlider.insert(0, '_YTJlMDY0YmNkMDBmNDY2ZG', 'http://www.next2friends.com/user//markjnj/video/YTJlMDY0YmNkMDBmNDY2ZG.flv', 'http://www.next2friends.com/user//markjnj/vthmb/YTJlMDY0YmNkMDBmNDY2ZG.jpg', 'markjnj:US', false, false );videoSlider.insert(0, '_ZmViOWIwM2MyOGQ4NDI4Ym', 'http://www.next2friends.com/user//dswiese/video/ZmViOWIwM2MyOGQ4NDI4Ym.flv', 'http://www.next2friends.com/user//dswiese/vthmb/ZmViOWIwM2MyOGQ4NDI4Ym.jpg', 'dswiese:US', false, false );videoSlider.insert(0, '_MzI5MzE0ZjY0ODQ4NGE0ZW', 'http://www.next2friends.com/user//funkyjanap/video/MzI5MzE0ZjY0ODQ4NGE0ZW.flv', 'http://www.next2friends.com/user//funkyjanap/vthmb/MzI5MzE0ZjY0ODQ4NGE0ZW.jpg', 'funkyjanap:US', false, false );videoSlider.insert(0, '_MTAzMzIxM2E3ZjQ2NDIyNT', 'http://www.next2friends.com/user//dswiese/video/MTAzMzIxM2E3ZjQ2NDIyNT.flv', 'http://www.next2friends.com/user//dswiese/vthmb/MTAzMzIxM2E3ZjQ2NDIyNT.jpg', 'dswiese:US', false, false );videoSlider.insert(0, '_Yzg0YThlNzNiYWY2NDc5Yz', 'http://www.next2friends.com/user//nts3rd/video/Yzg0YThlNzNiYWY2NDc5Yz.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/Yzg0YThlNzNiYWY2NDc5Yz.jpg', 'nts3rd:US', false, false );videoSlider.insert(0, '_MDU5MzljNzU0MTE4NDIzYT', 'http://www.next2friends.com/user//dotlizard/video/MDU5MzljNzU0MTE4NDIzYT.flv', 'http://www.next2friends.com/user//dotlizard/vthmb/MDU5MzljNzU0MTE4NDIzYT.jpg', 'dotlizard:US', false, false );videoSlider.insert(0, '_ZTUwYTRlYmNhZjFhNGQ2NG', 'http://www.next2friends.com/user//dotlizard/video/ZTUwYTRlYmNhZjFhNGQ2NG.flv', 'http://www.next2friends.com/user//dotlizard/vthmb/ZTUwYTRlYmNhZjFhNGQ2NG.jpg', 'dotlizard:US', false, false );videoSlider.insert(0, '_OTFjZmJkNDBhMTdiNDU1M2', 'http://www.next2friends.com/user//hanserik/video/OTFjZmJkNDBhMTdiNDU1M2.flv', 'http://www.next2friends.com/user//hanserik/vthmb/OTFjZmJkNDBhMTdiNDU1M2.jpg', 'HansErik:US', false, false );videoSlider.insert(0, '_YmFiN2JhOGRjN2FlNDlkYW', 'http://www.next2friends.com/user//dotlizard/video/YmFiN2JhOGRjN2FlNDlkYW.flv', 'http://www.next2friends.com/user//dotlizard/vthmb/YmFiN2JhOGRjN2FlNDlkYW.jpg', 'dotlizard:US', false, false );videoSlider.insert(0, '_MmFkYjI2N2JjYmY0NDU0Ym', 'http://www.next2friends.com/user//hanserik/video/MmFkYjI2N2JjYmY0NDU0Ym.flv', 'http://www.next2friends.com/user//hanserik/vthmb/MmFkYjI2N2JjYmY0NDU0Ym.jpg', 'HansErik:US', false, false );videoSlider.insert(0, '_YmY2YjU3Mzk5ZjU1NGZjYT', 'http://www.next2friends.com/user//hanserik/video/YmY2YjU3Mzk5ZjU1NGZjYT.flv', 'http://www.next2friends.com/user//hanserik/vthmb/YmY2YjU3Mzk5ZjU1NGZjYT.jpg', 'HansErik:US', false, false );videoSlider.insert(0, '_ZThiZmIzN2Q0Y2QzNGM3Ym', 'http://www.next2friends.com/user//nts3rd/video/ZThiZmIzN2Q0Y2QzNGM3Ym.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/ZThiZmIzN2Q0Y2QzNGM3Ym.jpg', 'nts3rd:US', false, false );videoSlider.insert(0, '_ZDRiMDAxMjc1ODhjNDY4YT', 'http://www.next2friends.com/user//hanserik/video/ZDRiMDAxMjc1ODhjNDY4YT.flv', 'http://www.next2friends.com/user//hanserik/vthmb/ZDRiMDAxMjc1ODhjNDY4YT.jpg', 'HansErik:US', false, false );videoSlider.insert(0, '_NjM5NWYwOTJiNDQ5NDVkNz', 'http://www.next2friends.com/user//nts3rd/video/NjM5NWYwOTJiNDQ5NDVkNz.flv', 'http://www.next2friends.com/user//nts3rd/vthmb/NjM5NWYwOTJiNDQ5NDVkNz.jpg', 'nts3rd:US', false, false );*/
		}, 0 );

		return [];
	}

	function OnDebug( message )
	{
		function EscapeXml( xml )
		{
			return xml.replace( /\</g, "&lt;" ).replace( /\>/g, "&gt;" );
		}

		var div = document.createElement( "div" );
		div.className = "status";
		div.innerHTML = "<span class='time'>" + new Date() + "</span>" + EscapeXml( message );
		debug.insertBefore( div, debug.firstChild );

		while ( debug.childNodes.length > 10 ) debug.removeChild( debug.lastChild );
	}


</script>
<noscript>
	<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0" width="100%" height="400" id="Swoosher" align="middle">
	<param name="allowScriptAccess" value="sameDomain" />
	<param name="allowFullScreen" value="false" />
	<param name="movie" value="N2F.VideoWall.swf" /><param name="quality" value="high" /><param name="bgcolor" value="#000000" />	<embed src="N2F.VideoWall.swf" quality="high" bgcolor="#000000" width="100%" height="400" name="Swoosher" align="middle" allowScriptAccess="sameDomain" allowFullScreen="false" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
	</object>
</noscript>


		<!-- left col start -->
		<div id="left_col">

			<!-- tab start -->
			<div class="tab">
				<ul class="tab_nav">
					<li><a href="javascript:ajaxGetTopVideos(1);" class="current" id="vidTab1">Latest Videos</a></li>
					<li><a href="javascript:ajaxGetTopVideos(2);" id="vidTab2">Most Viewed</a></li>
					<li><a href="javascript:ajaxGetTopVideos(3);" id="vidTab3">Most Discussed</a></li>
					<li><a href="javascript:ajaxGetTopVideos(4);" id="vidTab4">Top Rated</a> </li>
				</ul>
				<div class="tab_content">
				<ul class="feat_videos" id="fvids">
				  <% Response.Write(TopVideoHTML);%>
                </ul>
				<script type="text/javascript">cvids[1]=$('#fvids').html();</script>	
					<p class="more_link"><a href="/video">More videos »</a></p>
				</div>
				<!--/tab_content -->

			</div>
			<!-- tab end -->

		</div>
		<!-- left col end -->


		<!-- right col start -->
		<div id="right_col">

			<div class="tab">
				<h3>News</h3>
				<div class="tab_content">
					<ul class="news">
						<li>
						    <img alt="i-stage 2008" style="float: left;width:120px;" src="/images/istage.gif" />
                            <h2>i-stage 2008</h2>
                            
                            <p>Next2Friends named top finalists for CEA’s <a href="http://www.next2friends.com/press/Press-release-CEA-Announces-Finalists-I-Stage.pdf">i-stage</a> contest.</p>
						</li>
						<li>
						    <img alt="Mobile Entertainment Awards" style="float: left;width:120px;" src="/images/mea.jpg" />
                            <h3>Mobile Entertainment Awards</h3>
                            
                            <p>Next2Friends named top finalists for 
							<a href="http://www.visiongain.com/Conference.aspx?cid=141">Mobile Entertainment Awards 2008</a></p>
						</li>
						
						
						

					</ul>
				</div>
			</div>
			<!-- news tab end -->
			
			<p></p>
				<!--mini features start -->
			<div class="minifeats">
				<h3>What is Next2Friends?</h3>

				<div class="middle">
					<ul class="minifeats-skin">
						<li class="livefeatsli">
							<p class='profile_pic' style='padding:5px 0px 1px;'>
								<a href="javascript:displayMiniVideo('MTAzZmMz','Next2Friends Live');">
								<img src="/images/livescreenshot.jpg" alt="feature image" class="right" />
								</a>
							</p>
							<p><strong>Live Broadcasting</strong><br />
							Broadcast Live Video from your Mobile Phone, watch in Real Time at Next2Friends or anywhere on the web.<br />
							<a href="javascript:displayMiniVideo('MTAzZmMz','Next2Friends Live');">Watch the video</a></p>

						</li>
						 <li class="livefeatsli">
							<p class='profile_pic' style='padding:5px 0px 1px;'>
							<a href="javascript:displayMiniVideo('OTBlYjQ0','Next2Friends Social');">
								<img src="/images/socialscreenshot.jpg" alt="feature image" class="right" />
							</a>
							</p>
							<p><strong>Social</strong><br />
							Interact and engage with your friends and the site as well submit “Asks” and “SnapUps” from your mobile phone.<br />
							<a href="javascript:displayMiniVideo('OTBlYjQ0','Next2Friends Social');">Watch the video</a></p>
						</li>

					<%--	<li class="livefeatsli">
							<img src="images/home_carousel_videomail.jpg" alt="feature image" class="right" />
							<p><strong>Video Mail</strong><br />
							Send and receive Video Mail using the Next2Friends video message centre.<br />
							<a href="#">Learn more</a>.</p>
						</li>
						
						<li class="livefeatsli">
							<img src="images/home_carousel_AAF.jpg" alt="feature image" class="right" />
							<p><strong>Ask</strong><br />
							Create a real time Opinion Poll anonymously or for your Friends. You can Ask from your desktop or mobile phone.<br />
							<a href="#">Learn more</a>.</p>
						</li>
						
						<li class="livefeatsli">
							<img src="images/home_carousel_proximity.jpg" alt="feature image" class="right" />
							<p><strong>Proximity Tagging</strong><br />
							Tag and Meet new people where you live, work and play with Next2Friends proximity tagging for your mobile.<br />
						<a href="#">Learn more</a>.</p>
						</li>
						--%>
					</ul>

				</div>

				<div class="bottom clearfix">
					<div class="controller">
	<%--					<a href="#" class="minifeat-prev"><img src="images/minifeat-prev.gif" alt="prev" /></a>
						<span class="jcarousel-control">
						  <a href="#">1</a>
						  <a href="#">2</a>
						  <a href="#">3</a>
						  <a href="#">4</a>
						  <a href="#">5</a>
						</span>
						<a href="#" class="minifeat-next"><img src="images/minifeat-next.gif" alt="next" /></a>
					</div>

					<a href="#" class="seeall">See all features</a>			--%>	
				</div>
			</div>
			<!--mini features end -->
				<p></p>
			<!-- news tab start -->
		
			<!-- feats start -->
	<%--		<div class="home_feats clearfix">
				<h3>Invite Friends</h3>
				<ul class="home_feats_list invite clearfix">
					<li>
                        
                        <asp:Literal runat="server" ID="litInvite"><p>Please feel free to invite your friends!</p></asp:Literal> 
						
						<p>
						    <label for="email1">Message</label>
						    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtInviteMessage" CssClass="form_txt" Text="Your personal message here!"></asp:TextBox>
						</p>
						<p>
							<label for="email1">Email 1</label>
							<asp:TextBox runat="server" ID="txtFriend1" CssClass="form_txt"></asp:TextBox>
						</p>
						<p>
							<label for="email2">Email 2</label>
							<asp:TextBox runat="server" ID="txtFriend2" CssClass="form_txt"></asp:TextBox>
						</p>

						<p>
							<label for="email3">Email 3</label>
							<asp:TextBox runat="server" ID="txtFriend3" CssClass="form_txt"></asp:TextBox>
						</p>
						<p>
							<label for="email4">Email 4</label>
							<asp:TextBox runat="server" ID="txtFriend4" CssClass="form_txt"></asp:TextBox>
						</p>

						<p>
							<label for="email5">Email 5</label>
							<asp:TextBox runat="server" ID="txtFriend5" CssClass="form_txt"></asp:TextBox>
						</p>
						<p class="indent">
							<input type="button" value="Invite Friends" class="form_btn" onclick="__doPostBack('<%=btnInvite.UniqueID %>', '');return false;" />
					        <asp:Button runat="server" ID="btnInvite" OnClick="btnInvite_Click" CssClass="hiddenButton" Text="Invite Friends" />
						</p>
					</li>
				</ul>
			</div>--%>

<%--			<p><a href="AskAFriend.aspx"><img src="images/banner-aaf.gif" alt="ask a friend" /></a></p>--%>

		
			<!--mini features start -->
			
<%--			<!-- feat channels start -->
		    <div class="home_feats clearfix">
				<h3>Featured Members</h3>
				<ul class="home_feats_list clearfix">
					<%=FeatureSidebarHTML%>
				</ul>
			</div>
			<!-- feats end -->
		
			<p></p>--%>

		</div>
		<!-- right col end -->

	</div>
	<!-- middle end -->
	</div>
	
	<style>
	.livefeatsli{
		margin:0;
		padding:2px 0 2px 2px;
	}
	</style>

</asp:Content>
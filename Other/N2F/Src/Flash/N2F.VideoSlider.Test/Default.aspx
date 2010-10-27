<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>VideoSlider test page</title>

	<script type="text/javascript" language="javascript">AC_FL_RunContent = 0;</script>
	<script type="text/javascript" src="AC_RunActiveContent.js" language="javascript"></script>
	<script type="text/javascript" src="N2F.VideoSlider.js" language="javascript"></script>

	<script language="javascript" type="text/javascript">

		var plugin = null;

		function AddPhotos()
		{
			$get( "addButton" ).disabled = "disabled";

			var username = $get( "username" ).value;
			var password = $get( "password" ).value;
			Photos.GetPhotos( username, password, GetPhotosSuccess, GetPhotosFailure );
		}

		function GetPhotosSuccess( result )
		{
			for ( var index in result )
			{
				var item = result[ index ];
				var isLive = ( Math.random() >= 0.8 );
				videoSlider.insert( 0, null, item.mainPhotoURLField, item.thumbnailURLField, "Title", isLive, false );
			}

			$get( "addButton" ).disabled = "";
		}

		function GetPhotosFailure( result )
		{
			$get( "addButton" ).disabled = "";
			alert( result.get_message() );
		}

	</script>

</head>
<body>
	<form id="form1" runat="server" defaultfocus="username">
	<asp:ScriptManager ID="scriptManager" runat="server">
		<Services>
			<asp:ServiceReference Path="~/Photos.svc" />
		</Services>
	</asp:ScriptManager>
	<div>

		<script language="javascript">

			function initialize()
			{
				setTimeout( function()
				{
					plugin = $get( "Slider" );
					if ( !plugin ) plugin = document[ "Slider" ];

					videoSlider.init( plugin, function( item )
					{
						alert( "You selected: '" + item.url + "'" );
					} );

					$get( "addButton" ).disabled = "";
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
					'id', 'Slider',
					'bgcolor', '#ffffff',
					'name', 'Slider',
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
			<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0" width="500" height="150" id="Slider" align="middle">
				<param name="allowScriptAccess" value="always" />
				<param name="allowFullScreen" value="false" />
				<param name="wmode" value="transparent" />
				<param name="flashvars" value="initMethod=initialize&selectMethod=selectThumbnail" />
				<param name="movie" value="N2F.VideoSlider.swf" /><param name="quality" value="high" /><param name="bgcolor" value="#ffffff" />
				<embed src="N2F.VideoSlider.swf" quality="high" bgcolor="#ffffff" width="500" height="150" name="Slider" align="middle" allowScriptAccess="always" allowFullScreen="false" wmode="transparent" flashvars="initMethod=initialize&selectMethod=selectThumbnail" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
			</object>
		</noscript>

	</div>

	<div>

		<table>
			<tr>
				<td align="right">User name</td>
				<td align="left"><input id="username" type="text" value="anthony@next2friends.com" /></td>
			</tr>
			<tr>
				<td align="right">Password</td>
				<td align="left"><input id="password" type="password" /></td>
			</tr>
			<tr>
				<td></td>
				<td><button id="addButton" runat="server" type="button" disabled="disabled" onclick="javascript:AddPhotos()">Add photos</button></td>
			</tr>
		</table>

	</div>

	</form>
</body>
</html>
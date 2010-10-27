<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Next2Friends Image Browser</title>
	<script type="text/javascript" src="Silverlight.js"></script>
	<script type="text/javascript" src="intellisense.compressed.js"></script>
	<script type="text/javascript" src="Scene.js"></script>
	<script type="text/javascript" src="UserControl.js"></script>
	<script type="text/javascript" src="Arrow.js"></script>
	<script type="text/javascript" src="Swoosher.js"></script>
	<script type="text/javascript" src="SelectableImage.js"></script>
	<script type="text/javascript" src="Sylvester/sylvester.js"></script>
	<script type="text/javascript" src="Quaternion.js"></script>
	<script type="text/javascript" src="Camera.js"></script>
	<script type="text/javascript" src="Plane.js"></script>
	<script type="text/javascript" src="Frustum.js"></script>
	<script type="text/javascript" src="BSPTree.js"></script>
	<script type="text/javascript" src="BSPImage.js"></script>
	<script type="text/javascript" src="BoundingBox.js"></script>
	<script type="text/javascript" src="Loading.js"></script>
	<script type="text/javascript" src="ProgressBar.js"></script>
	<style type="text/css">
		html, body { height: 100%; overflow: auto; }
		body { padding: 0; margin: 0; }
		#errorLocation {
			font-size: small;
			color: Gray;
		}
		#silverlightPlugInHost {
			height: 600px; /*100%;*/
			overflow: auto;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<!-- Runtime errors from Silverlight will be displayed here.
		This will contain debugging information and should be removed or hidden when debugging is completed -->
		<div id='errorLocation'></div>
<!--		<div id="debug" style="height: 100px; background-color: palegoldenrod; overflow: scroll;"></div>-->

		<div id="silverlightPlugInHost">
			<script type="text/javascript">
				if (!window.Silverlight) 
					window.Silverlight = {};

				Silverlight.createDelegate = function(instance, method) {
					return function() {
						return method.apply(instance, arguments);
					}
				}
				
				var scene = new Swoosher_SL_1_0.Scene( GetPhotos() );
				
				Silverlight.createObjectEx({
					source: 'Scene.xaml',
					parentElement: document.getElementById('silverlightPlugInHost'),
					id: 'silverlightPlugIn',
					properties: {
						width: '800',
						height: '600',
						version: '1.0'
					},
					events: {
						onLoad: Silverlight.createDelegate(scene, scene.handleLoad),
						onError: function(sender, args) {
							var errorDiv = document.getElementById("errorLocation");
							if (errorDiv != null) {
								var errorText = args.errorType + "- " + args.errorMessage;
										
								if (args.ErrorType == "ParserError") {
									errorText += "<br>File: " + args.xamlFile;
									errorText += ", line " + args.lineNumber;
									errorText += " character " + args.charPosition;
								}
								else if (args.ErrorType == "RuntimeError") {
									errorText += "<br>line " + args.lineNumber;
									errorText += " character " +  args.charPosition;
								}
								errorDiv.innerHTML = errorText;
							}	
						}
					},		
					context: null
				});

/*
				function Trace( message )
				{
					var debug = document.getElementById( "debug" );
					debug.innerHTML = ( message + "<br/>" + debug.innerHTML ).substr( 0, 1000 );
				}*/
			</script>
		</div>
	</form>
</body>
</html>

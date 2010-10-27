// JScript source code

//contains calls to silverlight.js, example below loads Page.xaml
function createSilverlight()
{
	Silverlight.createObjectEx({
		source: "Page.xaml",
		parentElement: document.getElementById("SilverlightControlHost"),
		id: "SilverlightControl",
		properties: {
			width: "100%",
			height: "100%",
			version: "1.1",
			enableHtmlAccess: "true",
			inplaceInstallPrompt: true
		},
		events: {
			onError: null,
			onLoad: OnLoad
		},
		initParams: "email=anthony@next2friends.com,password=tonyrene,maxItems=64"
		//initParams: "collectionID=1234567"
	});

	function OnLoad()
	{
		// Don't need to do anything, but leaving placeholder
	}

	// Give the keyboard focus to the Silverlight control by default
	window.onload = function()
	{
		var silverlightControl = document.getElementById('SilverlightControl');
		if ( silverlightControl ) silverlightControl.focus();
	}
}
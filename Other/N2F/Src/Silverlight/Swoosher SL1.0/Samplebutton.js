if (!window.Swoosher_SL_1_0)
	window.Swoosher_SL_1_0 = {};

Swoosher_SL_1_0.Scene = function() 
{
}

Swoosher_SL_1_0.Scene.prototype =
{
	handleLoad: function(plugIn, userContext, rootElement) 
	{
		this.plugIn = plugIn;
		
		// Sample button event hookup: Find the button and then attach event handlers
		this.button = rootElement.children.getItem(0);	
		
		this.button.addEventListener("MouseEnter", Silverlight.createDelegate(this, this.handleMouseEnter));
		this.button.addEventListener("MouseLeftButtonDown", Silverlight.createDelegate(this, this.handleMouseDown));
		this.button.addEventListener("MouseLeftButtonUp", Silverlight.createDelegate(this, this.handleMouseUp));
		this.button.addEventListener("MouseLeave", Silverlight.createDelegate(this, this.handleMouseLeave));
	},
	
	// Sample event handlers
	handleMouseEnter: function(sender, eventArgs) 
	{
		// The following code shows how to find an element by name and call a method on it.
		var mouseEnterAnimation = sender.findName("mouseEnter");
		mouseEnterAnimation.begin(); 
	},
	
	handleMouseDown: function(sender, eventArgs) 
	{
		var mouseDownAnimation = sender.findName("mouseDown");
		mouseDownAnimation.begin(); 
	},
	
	handleMouseUp: function(sender, eventArgs) 
	{
		var mouseUpAnimation = sender.findName("mouseUp");
		mouseUpAnimation.begin(); 
		
		// Put clicked logic here
		alert("clicked");
	},
	
	handleMouseLeave: function(sender, eventArgs) 
	{
		var mouseLeaveAnimation = sender.findName("mouseLeave");
		mouseLeaveAnimation.begin(); 
	}
}
package  
{
	import flash.geom.Point;
	
	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class StackPanel extends FrameworkElement
	{
		private var orientation : String;

		public function StackPanel() 
		{
			orientation = "Vertical";
		}

		public override function MeasureOverride(availableSize:Size):Size 
		{
			var largest : Size = new Size( 0, 0 );

			for ( var index : int = 0; index < numChildren; ++ index )
			{
				var child : FrameworkElement = getChildAt( index ) as FrameworkElement;

				if ( child != null )
				{
					var size : Size = child.Measure( availableSize );

					if ( orientation.toLowerCase() == "horizontal" )
					{
						largest.Width += size.Width;
						if ( size.Height > largest.Height ) largest.Height = size.Height;
					}
					else
					{
						largest.Height += size.Height;
						if ( size.Width > largest.Width ) largest.Width = size.Width;
					}
				}
			}

			return largest;
		}

		public override function ArrangeOverride(finalSize:Size):Size 
		{
			var origin : Point = new Point();

			for ( var index : int = 0; index < numChildren; ++ index )
			{
				var child : FrameworkElement = getChildAt( index ) as FrameworkElement;

				if ( child != null )
				{
					child.offset = new Point( origin.x, origin.y );

					var size : Size = child.Arrange( finalSize );

					if ( orientation.toLowerCase() == "horizontal" )
					{
						origin.x += size.Width;
						child.offset.y += GetExtent( child.VerticalAlignment, size.Height, finalSize.Height ).Offset;
					}
					else
					{
						origin.y += size.Height;
						child.offset.x += GetExtent( child.HorizontalAlignment, size.Width, finalSize.Width ).Offset;
					}
				}
			}

			return finalSize;
		}

		public override function get HorizontalAlignment() : String
		{
			return ( super.HorizontalAlignment.toLowerCase() == "stretch" && this.orientation.toLowerCase() == "horizontal" )
				? "Left"
				: super.HorizontalAlignment;
		}

		public override function get VerticalAlignment() : String
		{
			return ( super.VerticalAlignment.toLowerCase() == "stretch" && this.orientation.toLowerCase() == "vertical" )
				? "Top"
				: super.VerticalAlignment;
		}

		public function get Orientation() : String { return orientation; }
		public function set Orientation( value : String ) : void { orientation = value; Invalidate(); }
	}
}
package
{
	import flash.display.*;

	public class ProgressBar extends Sprite
	{
		private var progress : Number;

		public function ProgressBar()
		{
			progress = 0;
		}

		public function set Progress( value : Number ) : void
		{
			progress = value;
		}

		public function get Progress() : Number
		{
			return progress;
		}

		public function Refresh() : void
		{
			var g : Graphics = graphics;

			g.clear();
			g.beginFill( 0xC0C0FF );
			g.drawRect( 0, 0, progress * 100, 10 );
			g.endFill();
			g.lineStyle( 1, 0xFFFFFF );
			g.drawRect( 0, 0, 100, 10 );
		}
	}
}
package
{

	import flash.display.*;
	import flash.events.*;
	import flash.external.ExternalInterface;
	import flash.geom.Matrix;
	import flash.geom.Transform;
	import flash.system.Security;

	public class Scene extends MovieClip
	{
		public var manager : SceneManager = new SceneManager();
		public var swoosher : Swoosher;

		public function Scene()
		{
			swoosher = new Swoosher( manager.tree );
			addChild( swoosher );
		}

		public function setSize( width : Number, height : Number )
		{
			swoosher.resized = true;
		}
	}
}
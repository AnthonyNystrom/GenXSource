package
{
	import flash.display.*;
	import flash.text.TextField;
	import fl.transitions.Tween;
	import fl.transitions.TweenEvent;

	public dynamic class TaggedLoader extends Loader
	{
		public var uniqueId : *;
		public var thumbnailUrl : String;
		public var url : String;
		public var description : String;
		public var isLive : Boolean;
		public var isArchived : Boolean;
		public var tag : *;

		public var thumbnailWidth : Number;
		public var thumbnailHeight : Number;

		public var index : int;
		public var offset : Number;

		public var reflection : Bitmap;
		public var reflectionMask : Sprite;
		public var label : TextField;
		public var liveTip : Bitmap;
		public var archivedIcon : Shape;
		public var tweens : Array;
//		public var remove : Boolean;

		public function TaggedLoader() : void
		{
			reflection = null;
			tweens = [];
		}

		public function AssignTweens( newTweens : Array, onFinish : Function = null ) : void
		{
			tweens = newTweens;

			var instance : TaggedLoader = this;
			var remove : Function;

			remove = function()
				{
					if ( tweens.length > 0 )
					{
						tweens[ 0 ].removeEventListener( TweenEvent.MOTION_FINISH, remove );
					}

					tweens = [];
					if ( onFinish != null ) onFinish();
				};

			if ( tweens.length > 0 )
			{
				tweens[ 0 ].addEventListener( TweenEvent.MOTION_FINISH, remove );
			}
		}

		public function StopTweens() : void
		{
			for ( var index : * in tweens )
			{
				var tween : Tween = tweens[ index ];
				if ( tween ) tween.stop();
			}

			tweens = [];
		}

		public function SetVisible( isVisible : Boolean ) : void
		{
			if ( visible != isVisible )
			{
				visible = isVisible;
				if ( reflection ) reflection.visible = isVisible;
				if ( reflectionMask ) reflectionMask.visible = isVisible;
				if ( archivedIcon ) archivedIcon.visible = isVisible;
				if ( liveTip ) liveTip.visible = isVisible;
			}
		}
	}
}
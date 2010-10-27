package
{
	
	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class ContentSource 
	{
		private static var nextId : int = 0;

		private var id : *;
		private var thumbnailUrl : String;
		private var url : String;
		private var description : String;
		private var isLive : Boolean;
		private var isArchived : Boolean;
		private var user : String;
		private var nationality : String;
		private var tag : * ;
		private var visual : * ;

		public function ContentSource( item : * )
		{
			this.id = item.uniqueId ? item.uniqueId : nextId ++;
			this.thumbnailUrl = item.thumbnailUrl;
			this.url = item.url;
			this.description = item.title;
			this.isLive = item.isLive;
			this.isArchived = item.isArchived;
			this.tag = item.tag;

			// TODO: This should really be in 'tag' rather than 'title'
			if ( item.title )
			{
				var userInfo : Array = item.title.split( ":" );
				this.user = userInfo[ 0 ];
				this.nationality = userInfo[ 1 ];
			}

			trace( ToString() );
		}

		public function get Id() : * { return id; }
		public function get ThumbnailUrl() : String { return thumbnailUrl; }
		public function get Url() : String { return url; }
		public function get Description() : String { return description; }
		public function get IsLive() : Boolean { return isLive; }
		public function get IsArchived() : Boolean { return isArchived; }
		public function get User() : String { return user; }
		public function get Nationality() : String { return nationality; }
		public function get Tag() : * { return tag; }

		public function get Visual() : * { return visual; }
		public function set Visual( value : * ) : void { visual = value; }

		public function ToString() : String { return "{ Id: " + id + ", ThumbnailUrl: " + thumbnailUrl + ", Url: " + url + " }"; }
	}
}
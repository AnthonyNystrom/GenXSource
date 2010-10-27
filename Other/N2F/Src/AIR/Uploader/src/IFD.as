package {
	public class IFD {
		private var tag:uint = 0;
		private var type:uint = 0;
		private var values:Object = null;
		
		public function IFD( tag:uint, type:uint, values:Object ) {
			this.tag = tag;
			this.type = type;
			this.values = values;	
		}
		
		public function getType():uint {
			return type;	
		}
		
		public function getTag():uint {
			return tag;
		}
		
		public function getValues():Object {
			return values;	
		}
	}
}
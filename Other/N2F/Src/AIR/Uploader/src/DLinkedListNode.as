package
{
	public class DLinkedListNode
	{
		public function DLinkedListNode(value:* = null):void
		{
			this.value = value;
		}
		public var next:DLinkedListNode;
		public var prev:DLinkedListNode;
		public var value:*;
	}
}
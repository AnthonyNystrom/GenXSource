package
{
	public class DLinkedList
	{
		private var _head:DLinkedListNode;
		private var _tail:DLinkedListNode;
		private var _length:Number = 0;
		private var _nodeClass:Class;
		
		public function DLinkedList(nodeClass:Class = null):void
		{
			_nodeClass = (nodeClass == null)? DLinkedListNode:nodeClass;
			_head = new _nodeClass();
			_tail = new _nodeClass();
			_head.next = _tail;
			_tail.prev = _head;
		}
		public function get first():DLinkedListNode
		{
			return (_head.next == _tail)? null:_head.next;
		}
		public function get last():DLinkedListNode
		{
			return (_tail.prev == _head)? null:_head;
		}
		
		public function get tail():DLinkedListNode
		{
			return _tail;
		}
		public function get head():DLinkedListNode
		{
			return _head;
		}
		
		public function get length():Number
		{
			return _length;
		}
		
		private function makeNode(value:*):DLinkedListNode
		{
			var node:DLinkedListNode;
			if(value is DLinkedListNode)
			{
				node = value;
			}
			else
			{
				node = new _nodeClass(value);
			}
			return node;
		}

		public function insertAfter(value:*,prev:DLinkedListNode):DLinkedListNode
		{
			var node:DLinkedListNode = makeNode(value);
			node.prev = prev;
			node.next = prev.next;
			node.prev.next = node;
			node.next.prev = node;
			
			_length++;
			return node;
		}
		
		public function getNode(value:*):DLinkedListNode
		{
			if(value is DLinkedListNode)
				return value;
			else
			{
				return find(value);
			}
		}

		public function find(value:*):DLinkedListNode
		{
			var cur:DLinkedListNode = _head;
			while(cur.value != value && cur != _tail)
				cur = cur.next;
			return (cur == _tail)? null:cur;
		}
		
		public function remove(value:*):DLinkedListNode
		{
			var node:DLinkedListNode = getNode(value);
			node.prev.next = node.next;
			node.next.prev = node.prev;			
			_length--;
			return node;
		}

		public function push(value:*):DLinkedListNode
		{
			return insertAfter(value,_tail.prev);
		}

		public function pop():DLinkedListNode
		{
			return (_length == 0)? null:remove(_tail.prev);	
		}

		public function unshift(value:*):DLinkedListNode
		{
			return insertAfter(value,_head);
		}
		public function shift():DLinkedListNode
		{
			return (_length == 0)? null:remove(_head.next);
		}
	}
}
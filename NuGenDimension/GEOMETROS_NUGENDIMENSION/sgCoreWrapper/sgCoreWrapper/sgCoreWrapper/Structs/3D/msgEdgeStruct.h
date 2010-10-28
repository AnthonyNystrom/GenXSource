#pragma once
#include "sgCore/sg3D.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgEdgeStruct
		{
		public:
			msgEdgeStruct()
			{
				_edge = new SG_EDGE();
				_needDelete = true;
			}

			~msgEdgeStruct()
			{
				this->!msgEdgeStruct();
			}

			!msgEdgeStruct()
			{
				if (_needDelete)
				{
					delete _edge;
				}
			}

			property unsigned short begin_vertex_index
			{
				unsigned short get() { return _edge->begin_vertex_index; }
				void set(unsigned short value) { _edge->begin_vertex_index = value; }
			}

			property unsigned short end_vertex_index
			{
				unsigned short get() { return _edge->end_vertex_index; }
				void set(unsigned short value) { _edge->end_vertex_index = value; }
			}
			
			property unsigned short edge_type
			{
				unsigned short get() { return _edge->edge_type; }
				void set(unsigned short value) { _edge->edge_type = value; }
			}
		private:
			bool _needDelete;

		internal:
			msgEdgeStruct(SG_EDGE* edge)
			{
				_edge = edge;
				_needDelete = false;
			}

			SG_EDGE* _edge;
		};
	}
}
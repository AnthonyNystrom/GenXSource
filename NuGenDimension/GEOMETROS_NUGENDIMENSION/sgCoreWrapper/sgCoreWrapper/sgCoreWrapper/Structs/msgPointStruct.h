#pragma once
#include "sgCore/sgDefs.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgPointStruct
		{
		public:
			msgPointStruct()
			{
				CreateNewPoint();
			}

			msgPointStruct(double x, double y, double z)
			{
				CreateNewPoint();

				this->x = x;
				this->y = y;
				this->z = z;
			}

			~msgPointStruct()
			{
				this->!msgPointStruct();
			}

			!msgPointStruct()
			{
				if (_needDelete)
				{
					delete _point;
				}
			}

			property double x
			{
				double get() { return _point->x; }
				void set(double value) { _point->x = value; }
			}
			
			property double y
			{
				double get() { return _point->y; }
				void set(double value) { _point->y = value; }
			}
			property double z
			{
				double get() { return _point->z; }
				void set(double value) { _point->z = value; }
			}

		private:
			bool _needDelete;

			void CreateNewPoint()
			{
				_point = new SG_POINT();
				_needDelete = true;
			}
		
		internal:
			msgPointStruct(SG_POINT* point)
			{
				_point = point;
				_needDelete = false;
			}

			SG_POINT* _point;
		};

		public ref struct msgVectorStruct : public msgPointStruct
		{
			msgVectorStruct()
			{}

			msgVectorStruct(double x, double y, double z) : msgPointStruct(x, y, z)
			{}

		internal:
			msgVectorStruct(SG_POINT* point) : msgPointStruct(point)
			{}
		};
	}
}
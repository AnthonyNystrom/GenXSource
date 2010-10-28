#pragma once
#include "sgCore/sgDefs.h"

#include "Helpers/DrawLineCallbackHelper.h"

using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgCircleStruct
		{
		public:
			msgCircleStruct()
			{
				Init(new SG_CIRCLE(), true);
			}
			
			~msgCircleStruct()
			{
				this->!msgCircleStruct();
			}
			
			!msgCircleStruct()
			{
				if (_needDelete)
				{
					delete _sgCircle;
				}				
			}
			
			property double radius
			{
				double get() { return _sgCircle->radius; }
				void set(double value) { _sgCircle->radius = value; }
			}

			property msgVectorStruct^ normal
			{
				msgVectorStruct^ get() { return _normal; }
				void set(msgVectorStruct^ value) { _normal = value; }
			}

			property msgPointStruct^ center
			{
				msgPointStruct^ get() { return _center; }
				void set(msgPointStruct^ value) { _center = value; }
			}
			
			bool FromCenterRadiusNormal(msgPointStruct^ cen, double rad, msgVectorStruct^ nor)
			{
				return _sgCircle->FromCenterRadiusNormal(*cen->_point, rad, *nor->_point);
			}
			
			bool FromThreePoints(msgPointStruct^ p1, msgPointStruct^ p2, msgPointStruct^ p3)
			{
				return _sgCircle->FromThreePoints(*p1->_point, *p2->_point, *p3->_point);
			}

			bool Draw(DrawLineDelegate^ drawLineDelegate)
			{
				DrawLineCallbackHelper::DrawLineDel = drawLineDelegate;
				return _sgCircle->Draw((SG_DRAW_LINE_FUNC)DrawLineCallbackHelper::uDrawLineFunction);
			}
		private:
			msgVectorStruct^ _normal;
			msgPointStruct^ _center;
			bool _needDelete;

			void Init(SG_CIRCLE* circle, bool needDelete)
			{
				_sgCircle = circle;
				_normal = gcnew msgVectorStruct(&_sgCircle->normal);
				_center = gcnew msgPointStruct(&_sgCircle->center);
				_needDelete = needDelete;
			}
		internal:
			msgCircleStruct(SG_CIRCLE* circle)
			{
				Init(circle, false);
			}

			SG_CIRCLE* _sgCircle;
		};
	}
}
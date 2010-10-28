#pragma once
#include "sgCore/sgDefs.h"

#include "Helpers/DrawLineCallbackHelper.h"

using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgArcStruct
		{
		public:
			msgArcStruct()
			{
				Init(new SG_ARC(), true);
			}

			~msgArcStruct()
			{
				this->!msgArcStruct();
			}

			!msgArcStruct()
			{
				if (_needDelete)
				{
					delete _sgArc;
				}				
			}

			property double radius
			{
				double get() { return _sgArc->radius; }
				void set(double value) { _sgArc->radius = value; }
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

			property msgPointStruct^ begin
			{
				msgPointStruct^ get() { return _begin; }
				void set(msgPointStruct^ value) { _begin = value; }
			}

			property msgPointStruct^ end
			{
				msgPointStruct^ get() { return _end; }
				void set(msgPointStruct^ value) { _end = value; }
			}

			property double begin_angle
			{
				double get() { return _sgArc->begin_angle; }
				void set(double value) { _sgArc->begin_angle = value; }
			}

			property double angle
			{
				double get() { return _sgArc->angle; }
				void set(double value) { _sgArc->angle = value; }
			}

			bool FromThreePoints(msgPointStruct^ begP, msgPointStruct^ endP, msgPointStruct^ midP, 
				bool invert)
			{
				return _sgArc->FromThreePoints(*begP->_point, *endP->_point, *midP->_point, invert);
			}

			bool FromCenterBeginEnd(msgPointStruct^ cenP, msgPointStruct^ begP, msgPointStruct^ endP, 
				bool invert)
			{
				return _sgArc->FromCenterBeginEnd(*cenP->_point, *begP->_point, *endP->_point, invert);
			}

			bool FromBeginEndNormalRadius(msgPointStruct^ begP,	msgPointStruct^ endP, msgVectorStruct^ nrmlV, 
				double rad,	bool invert)
			{
				return _sgArc->FromBeginEndNormalRadius(*begP->_point, *endP->_point, *nrmlV->_point, rad, 
					invert);
			}

			bool FromCenterBeginNormalAngle(msgPointStruct^ cenP, msgPointStruct^ begP, msgVectorStruct^ nrmlV, 
				double ang)
			{
				return _sgArc->FromCenterBeginNormalAngle(*cenP->_point, *begP->_point, *nrmlV->_point, ang);
			}

			bool FromBeginEndNormalAngle(msgPointStruct^ begP, msgPointStruct^ endP, msgVectorStruct^ nrmlV, 
				double ang)
			{
				return _sgArc->FromBeginEndNormalAngle(*begP->_point, *endP->_point, *nrmlV->_point, ang);
			}

			bool Draw(DrawLineDelegate^ drawLineDelegate)
			{
				DrawLineCallbackHelper::DrawLineDel = drawLineDelegate;
				return _sgArc->Draw((SG_DRAW_LINE_FUNC)DrawLineCallbackHelper::uDrawLineFunction);
			}
		private:
			bool _needDelete;
			msgVectorStruct^ _normal;
			msgPointStruct^ _center;
			msgPointStruct^ _begin;
			msgPointStruct^ _end;

			void Init(SG_ARC* arc, bool needDelete)
			{
				_sgArc = arc;
				_normal = gcnew msgVectorStruct(&_sgArc->normal);
				_center = gcnew msgPointStruct(&_sgArc->center);
				_begin = gcnew msgPointStruct(&_sgArc->begin);
				_end = gcnew msgPointStruct(&_sgArc->end);
				_needDelete = needDelete;
			}
		internal:
			msgArcStruct(SG_ARC* arc)
			{
				Init(arc, false);
			}

			SG_ARC* _sgArc;
		};
	}
}
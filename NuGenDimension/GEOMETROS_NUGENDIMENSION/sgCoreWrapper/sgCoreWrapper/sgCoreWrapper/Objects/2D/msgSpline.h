#pragma once
#include "sgCore/sg2D.h"
#include "Objects/2D/msg2DObject.h"
#include "Structs/2D/msgSplineStruct.h"

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgSpline : msg2DObject
		{
		public:
			static msgSpline^ Create(msgSplineStruct^ arcSpline)
			{
				return gcnew msgSpline(sgCSpline::Create(*arcSpline->_sgSpline));
			}

			msgSplineStruct^ GetGeometry()
			{
				return gcnew msgSplineStruct((SG_SPLINE*)sgSpline->GetGeometry());
			}

			bool virtual ApplyTempMatrix() override
			{
				return sgSpline->ApplyTempMatrix();
			}

			bool virtual IsClosed() override
			{
				return sgSpline->IsClosed();
			}

			bool virtual IsPlane(msgVectorStruct^ vec, double value) override
			{
				return sgSpline->IsPlane(vec->_point, &value);
			}

			bool virtual IsLinear() override
			{
				return sgSpline->IsLinear();
			}

			bool virtual IsSelfIntersecting() override
			{
				return sgSpline->IsSelfIntersecting();
			}
		internal:
			msgSpline(sgCSpline* spline) : msg2DObject(spline)
			{
			}
		internal:
			property sgCSpline* sgSpline
			{
				sgCSpline* get() { return (sgCSpline*)_sgCObject; }
			}
		};
	}
}
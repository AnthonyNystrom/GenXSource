#pragma once
#include "sgCore/sg2D.h"
#include "Objects/2D/msg2DObject.h"
#include "Structs/2D/msgCircleStruct.h"
#include "Helpers/ArrayHelper.h"

using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgCircle : msg2DObject
		{
		public:
			static msgCircle^ Create(msgCircleStruct^ circleStruct)
			{
				return gcnew msgCircle(sgCCircle::Create(*circleStruct->_sgCircle));
			}
			
			msgCircleStruct^ GetGeometry()
			{
				return gcnew msgCircleStruct((SG_CIRCLE*)sgCircle->GetGeometry());
			}

			array<msgPointStruct^>^ GetPoints()
			{
				return ArrayHelper::GetManagedArray<msgPointStruct, SG_POINT>((SG_POINT*)sgCircle->GetPoints(), 
					sgCircle->GetPointsCount());
			}

			bool virtual ApplyTempMatrix() override
			{
				return sgCircle->ApplyTempMatrix();
			}

			bool virtual IsClosed() override
			{
				return sgCircle->IsClosed();
			}

			bool virtual IsPlane(msgVectorStruct^ vec, double value) override
			{
				return sgCircle->IsPlane(vec->_point, &value);
			}

			bool virtual IsLinear() override
			{
				return sgCircle->IsLinear();
			}

			bool virtual IsSelfIntersecting() override
			{
				return sgCircle->IsSelfIntersecting();
			}
		internal:
			msgCircle(sgCCircle* circle) : msg2DObject(circle)
			{
			}
		internal:
			property sgCCircle* sgCircle
			{
				sgCCircle* get() { return (sgCCircle*)_sgCObject; }
			}
		};
	}
}
#pragma once
#include "sgCore/sg2D.h"
#include "Objects/2D/msg2DObject.h"
#include "Structs/2D/msgArcStruct.h"
#include "Helpers/ArrayHelper.h"

using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgArc : msg2DObject
		{
		public:
			static msgArc^ Create(msgArcStruct^ arcStruct)
			{
				return gcnew msgArc(sgCArc::Create(*arcStruct->_sgArc));
			}

			msgArcStruct^ GetGeometry()
			{
				return gcnew msgArcStruct((SG_ARC*)sgArc->GetGeometry());
			}

			array<msgPointStruct^>^ GetPoints()
			{
				return ArrayHelper::GetManagedArray<msgPointStruct, SG_POINT>((SG_POINT*)sgArc->GetPoints(), 
					sgArc->GetPointsCount());
			}

			bool virtual ApplyTempMatrix() override
			{
				return sgArc->ApplyTempMatrix();
			}

			bool virtual IsClosed() override
			{
				return sgArc->IsClosed();
			}

			bool virtual IsPlane(msgVectorStruct^ vec, double value) override
			{
				return sgArc->IsPlane(vec->_point, &value);
			}

			bool virtual IsLinear() override
			{
				return sgArc->IsLinear();
			}

			bool virtual IsSelfIntersecting() override
			{
				return sgArc->IsSelfIntersecting();
			}
		internal:
			msgArc(sgCArc* arc) : msg2DObject(arc)
			{
			}
		internal:
			property sgCArc* sgArc
			{
				sgCArc* get() { return (sgCArc*)_sgCObject; }
			}
		};
	}
}
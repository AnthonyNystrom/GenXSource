#pragma once
#include "sgCore/sg2D.h"
#include "Interfaces/mIObjectsList.h"
#include "Objects/2D/msg2DObject.h"
#include "Structs/msgPointStruct.h"
#include "Helpers/ObjectCreateHelper.h"

using namespace sgCoreWrapper::Interfaces;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgContour : msg2DObject
		{
		public:
			static msgContour^ CreateContour(array<msgObject^>^ objcts)
			{
				sgCObject** un_objcts = new sgCObject*[objcts->Length];
				for (int i = 0; i < objcts->Length; i++)
				{
					un_objcts[i] = objcts[i]->_sgCObject;
				}
				msgContour^ contour = gcnew msgContour(sgCContour::CreateContour(un_objcts, objcts->Length));
				delete[] un_objcts;
				return contour;
			}

			mIObjectsList^ GetChildrenList()
			{
				return gcnew mIObjectsList(sgContour->GetChildrenList());
			}

			bool virtual ApplyTempMatrix() override
			{
				return sgContour->ApplyTempMatrix();
			}

			bool virtual IsClosed() override
			{
				return sgContour->IsClosed();
			}

			bool virtual IsPlane(msgVectorStruct^ vec, double value) override
			{
				return sgContour->IsPlane(vec->_point, &value);
			}

			bool virtual IsLinear() override
			{
				return sgContour->IsLinear();
			}

			bool virtual IsSelfIntersecting() override
			{
				return sgContour->IsSelfIntersecting();
			}

			bool BreakContour(array<msgObject^>^% objcts)
			{
				int count = sgContour->GetChildrenList()->GetCount();
				sgCObject** un_objcts = new sgCObject*[count];

				bool result = sgContour->BreakContour(un_objcts);
				if(result)
				{
					objcts = gcnew array<msgObject^>(count);
					for (int i = 0; i < objcts->Length; i++)
					{
						objcts[i] = ObjectCreateHelper::CreateObject(un_objcts[i]);
					}
				}
				delete[] un_objcts;
				return result;
			}
		internal:
			msgContour(sgCContour* contour) : msg2DObject(contour)
			{
			}
		internal:
			property sgCContour* sgContour
			{
				sgCContour* get() { return (sgCContour*)_sgCObject; }
			}

			static msgContour^ InternalCreate(sgCContour* contour)
			{
				return gcnew msgContour(contour);
			}
		};
	}
}
#pragma once
#include "sgCore/sgGroup.h"
#include "Objects/msgObject.h"
#include "Interfaces/mIObjectsList.h"

using namespace sgCoreWrapper::Interfaces;
using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgGroup : msgObject
		{
		public:
			static msgGroup^ CreateGroup(array<msgObject^>^ objcts)
			{
				int count = objcts->Length;
				sgCObject** uobjcts = new sgCObject*[count];
				for (int i = 0; i < count; i++)
				{
					uobjcts[i] = objcts[i]->_sgCObject;
				}
				msgGroup^ group = gcnew msgGroup(sgCGroup::CreateGroup(uobjcts, count));
				delete[] uobjcts;
				return group;
			}

			mIObjectsList^ GetChildrenList()
			{
				return gcnew mIObjectsList(sgGroup->GetChildrenList());
			}

			bool virtual ApplyTempMatrix() override
			{
				return sgGroup->ApplyTempMatrix();
			}

			bool BreakGroup(array<msgObject^>^% objcts);
			
		internal:
			msgGroup(sgCGroup* group) : msgObject(group)
			{
			}

			property sgCGroup* sgGroup
			{
				sgCGroup* get() { return (sgCGroup*)_sgCObject; }
			}
		};
	}
}
#pragma once
#include "sgCore/sgScene.h"
#include "Interfaces/mIObjectsList.h"
#include "Structs/msgPointStruct.h"
#include "Objects/msgObject.h"

using namespace sgCoreWrapper::Interfaces;
using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgScene
		{
		public:
			static msgScene^ GetScene()
			{
				return gcnew msgScene(sgCScene::GetScene());
			}

			mIObjectsList^ GetObjectsList()
			{
				return gcnew mIObjectsList(_sgScene->GetObjectsList());
			}

			mIObjectsList^ GetSelectedObjectsList()
			{
				return gcnew mIObjectsList(_sgScene->GetSelectedObjectsList());
			}

			bool AttachObject(msgObject^ obj)
			{
				return _sgScene->AttachObject(obj->_sgCObject);
			}

			void DetachObject(msgObject^ obj)
			{
				_sgScene->DetachObject(obj->_sgCObject);
			}

			bool StartUndoGroup()
			{
				return _sgScene->StartUndoGroup();
			}

			bool EndUndoGroup()
			{
				return _sgScene->EndUndoGroup();
			}

			bool IsUndoStackEmpty()
			{
				return _sgScene->IsUndoStackEmpty();
			}

			bool IsRedoStackEmpty()
			{
				return _sgScene->IsRedoStackEmpty();
			}

			bool Undo()
			{
				return _sgScene->Undo();
			}

			bool Redo()
			{
				return _sgScene->Redo();
			}

			void Clear()
			{
				_sgScene->Clear();
			}

			void GetGabarits(msgPointStruct^ p_min, msgPointStruct^ p_max)
			{
				_sgScene->GetGabarits(*p_min->_point, *p_max->_point);
			}
		internal:
			msgScene(sgCScene* scene)
			{
				_sgScene = scene;
			}

			sgCScene* _sgScene;
		};
	}
}
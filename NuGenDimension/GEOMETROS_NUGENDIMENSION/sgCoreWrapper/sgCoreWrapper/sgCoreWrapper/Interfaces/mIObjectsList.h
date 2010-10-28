#pragma once;
#include "Objects/msgObject.h"
#include "sgCore/sgObject.h"

using namespace sgCoreWrapper::Objects;

namespace sgCoreWrapper
{
	namespace Interfaces
	{
		public ref class mIObjectsList
		{
		public:
			int GetCount()
			{
				return _iObjectsList->GetCount();
			}

			msgObject^ GetHead();

			msgObject^ GetNext(msgObject^ current);

			msgObject^ GetTail();

			msgObject^ GetPrev(msgObject^ current);
		internal:
			mIObjectsList(IObjectsList* iObjectsList)
			{
				_iObjectsList = iObjectsList;
			}
		private:
			IObjectsList* _iObjectsList;
		};
	}
}
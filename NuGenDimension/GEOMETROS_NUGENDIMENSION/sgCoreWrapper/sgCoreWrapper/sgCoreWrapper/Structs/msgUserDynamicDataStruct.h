#pragma once

#include "sgCore/sgDefs.h"
#include <vcclr.h>
using namespace System;

namespace sgCoreWrapper
{
	namespace Structs
	{
		class msgUserDynamicDataStructHelper;

		public ref struct msgUserDynamicDataStruct abstract
		{
		public:
			msgUserDynamicDataStruct();

			~msgUserDynamicDataStruct()
			{
				this->!msgUserDynamicDataStruct();
			}

			!msgUserDynamicDataStruct();

			virtual void Save() = 0;

		internal:
			msgUserDynamicDataStructHelper* _msgUserDynamicDataStructHelper;
		};

		class msgUserDynamicDataStructHelper : public SG_USER_DYNAMIC_DATA
		{
		public:
			msgUserDynamicDataStructHelper(msgUserDynamicDataStruct^ msgUserDynamicData)
			{
				_msgUserDynamicData = msgUserDynamicData;
			}

			virtual void Finalize()
			{
				_msgUserDynamicData->Save();
			}

		private:
			gcroot<msgUserDynamicDataStruct^> _msgUserDynamicData;
		};
	}
}
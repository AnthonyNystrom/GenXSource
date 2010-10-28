#pragma once

namespace sgCoreWrapper 
{
	namespace Helpers
	{
		ref class ObjectCreateHelper abstract sealed
		{
		public:
			static msgObject^ CreateObject(sgCObject* sgCObject);
		};
	}
}
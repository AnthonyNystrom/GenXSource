#pragma once

#include "sgCore/sgDefs.h"
#include "sgCore/sgObject.h"
#include "Objects/msgObject.h"
#include "Structs/msgPointStruct.h"

using namespace sgCoreWrapper::Objects;
using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper 
{
	namespace Helpers
	{
		public delegate void DrawLineDelegate(msgPointStruct^ p1, msgPointStruct^ p2);

		ref class DrawLineCallbackHelper
		{
		public:
			static void uDrawLineFunction(SG_POINT* up1,SG_POINT* up2)
			{
				msgPointStruct^ p1 = gcnew msgPointStruct(up1);
				msgPointStruct^ p2 = gcnew msgPointStruct(up2);
				if (_drawLineDelegate)
				{
					_drawLineDelegate(p1, p2);
				}
			}

			static property DrawLineDelegate^ DrawLineDel
			{
				DrawLineDelegate^ get() { return _drawLineDelegate; }
				void set(DrawLineDelegate^ value) { _drawLineDelegate = value; }
			}
		private:
			static DrawLineDelegate^ _drawLineDelegate;
		};
	}
}
#pragma once
#include "Objects\msgObject.h"
#include "sgCore\sg2D.h"
#include "Structs\msgPointStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgPoint : msgObject
		{
		public:
			static msgPoint^ Create(double pX, double pY, double pZ)
			{
				return gcnew msgPoint(sgCPoint::Create(pX, pY, pZ));
			}

			msgPointStruct^ GetGeometry()
			{
				return gcnew msgPointStruct((SG_POINT*)sgPoint->GetGeometry());
			}
		internal:
			msgPoint(sgCPoint* point) : msgObject(point)
			{
			}

			property sgCPoint* sgPoint
			{
				sgCPoint* get() { return (sgCPoint*)_sgCObject; }
			}
		};
	}
}
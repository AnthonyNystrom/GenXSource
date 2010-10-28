#pragma once
#include "sgCore/sg3D.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgEllipsoidStruct
		{
		public:
			property double Radius1
			{
				double get() { return _sgEllipsoid->Radius1; }
				void set(double value) { _sgEllipsoid->Radius1 = value; }
			}

			property double Radius2
			{
				double get() { return _sgEllipsoid->Radius2; }
				void set(double value) { _sgEllipsoid->Radius2 = value; }
			}

			property double Radius3
			{
				double get() { return _sgEllipsoid->Radius3; }
				void set(double value) { _sgEllipsoid->Radius3 = value; }
			}

			property short MeridiansCount
			{
				short get() { return _sgEllipsoid->MeridiansCount; }
				void set(short value) { _sgEllipsoid->MeridiansCount = value; }
			}

			property short ParallelsCount
			{
				short get() { return _sgEllipsoid->ParallelsCount; }
				void set(short value) { _sgEllipsoid->ParallelsCount = value; }
			}
		internal:
			msgEllipsoidStruct(SG_ELLIPSOID* ellipsoid)
			{
				_sgEllipsoid = ellipsoid;
			}
			SG_ELLIPSOID* _sgEllipsoid;
		};
	}
}
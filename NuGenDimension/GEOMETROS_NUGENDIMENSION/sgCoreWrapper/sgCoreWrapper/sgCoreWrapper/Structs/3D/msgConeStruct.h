#pragma once
#include "sgCore/sg3D.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgConeStruct
		{
		public:
			property double Radius1
			{
				double get() { return _sgCone->Radius1; }
				void set(double value) { _sgCone->Radius1 = value; }
			}

			property double Radius2
			{
				double get() { return _sgCone->Radius2; }
				void set(double value) { _sgCone->Radius2 = value; }
			}

			property double Height
			{
				double get() { return _sgCone->Radius2; }
				void set(double value) { _sgCone->Radius2 = value; }
			}

			property short MeridiansCount
			{
				short get() { return _sgCone->MeridiansCount; }
				void set(short value) { _sgCone->MeridiansCount = value; }
			}
		internal:
			msgConeStruct(SG_CONE* cone)
			{
				_sgCone = cone;
			}
			SG_CONE* _sgCone;
		};
	}
}
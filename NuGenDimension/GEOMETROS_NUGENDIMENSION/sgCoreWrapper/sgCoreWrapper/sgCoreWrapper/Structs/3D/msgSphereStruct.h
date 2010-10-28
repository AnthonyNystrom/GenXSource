#pragma once
#include "sgCore/sg3D.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgSphereStruct
		{
		public:
			property double Radius
			{
				double get() { return _sgSphere->Radius; }
				void set(double value) { _sgSphere->Radius = value; }
			}

			property short MeridiansCount
			{
				short get() { return _sgSphere->MeridiansCount; }
				void set(short value) { _sgSphere->MeridiansCount = value; }
			}

			property short ParallelsCount
			{
				short get() { return _sgSphere->ParallelsCount; }
				void set(short value) { _sgSphere->ParallelsCount = value; }
			}
		internal:
			msgSphereStruct(SG_SPHERE* sphere)
			{
				_sgSphere = sphere;
			}
			SG_SPHERE* _sgSphere;
		};
	}
}
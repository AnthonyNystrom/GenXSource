#pragma once
#include "sgCore/sg3D.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgCylinderStruct
		{
		public:
			property double Radius
			{
				double get() { return _sgCylinder->Radius; }
				void set(double value) { _sgCylinder->Radius = value; }
			}

			property double Height
			{
				double get() { return _sgCylinder->Height; }
				void set(double value) { _sgCylinder->Height = value; }
			}

			property short MeridiansCount
			{
				short get() { return _sgCylinder->MeridiansCount; }
				void set(short value) { _sgCylinder->MeridiansCount = value; }
			}
		internal:
			msgCylinderStruct(SG_CYLINDER* cylinder)
			{
				_sgCylinder = cylinder;
			}
			SG_CYLINDER* _sgCylinder;
		};
	}
}
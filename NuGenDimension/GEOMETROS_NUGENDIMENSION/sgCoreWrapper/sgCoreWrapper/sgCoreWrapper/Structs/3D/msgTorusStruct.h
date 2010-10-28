#pragma once
#include "sgCore/sg3D.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgTorusStruct
		{
		public:
			property double Radius1
			{
				double get() { return _sgTorus->Radius1; }
				void set(double value) { _sgTorus->Radius1 = value; }
			}

			property double Radius2
			{
				double get() { return _sgTorus->Radius2; }
				void set(double value) { _sgTorus->Radius2 = value; }
			}

			property short MeridiansCount1
			{
				short get() { return _sgTorus->MeridiansCount1; }
				void set(short value) { _sgTorus->MeridiansCount1 = value; }
			}

			property short MeridiansCount2
			{
				short get() { return _sgTorus->MeridiansCount2; }
				void set(short value) { _sgTorus->MeridiansCount2 = value; }
			}
		internal:
			msgTorusStruct(SG_TORUS* torus)
			{
				_sgTorus = torus;
			}
			SG_TORUS* _sgTorus;
		};
	}
}
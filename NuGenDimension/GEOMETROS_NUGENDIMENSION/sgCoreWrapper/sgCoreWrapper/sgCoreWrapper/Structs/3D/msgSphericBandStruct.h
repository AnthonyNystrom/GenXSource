#pragma once
#include "sgCore/sg3D.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgSphericBandStruct
		{
		public:
			property double Radius
			{
				double get() { return _sgSphericBand->Radius; }
				void set(double value) { _sgSphericBand->Radius = value; }
			}

			property double BeginCoef
			{
				double get() { return _sgSphericBand->BeginCoef; }
				void set(double value) { _sgSphericBand->BeginCoef = value; }
			}

			property double EndCoef
			{
				double get() { return _sgSphericBand->EndCoef; }
				void set(double value) { _sgSphericBand->EndCoef = value; }
			}

			property short MeridiansCount
			{
				short get() { return _sgSphericBand->MeridiansCount; }
				void set(short value) { _sgSphericBand->MeridiansCount = value; }
			}
		internal:
			msgSphericBandStruct(SG_SPHERIC_BAND* sphericBand)
			{
				_sgSphericBand = sphericBand;
			}
			SG_SPHERIC_BAND* _sgSphericBand;
		};
	}
}
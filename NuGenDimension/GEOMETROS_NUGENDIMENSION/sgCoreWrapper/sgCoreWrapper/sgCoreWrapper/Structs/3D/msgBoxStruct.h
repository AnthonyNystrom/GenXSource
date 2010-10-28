#pragma once
#include "sgCore/sg3D.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgBoxStruct
		{
		public:
			property double SizeX
			{
				double get() { return _sgBox->SizeX; }
				void set(double value) { _sgBox->SizeX = value; }
			}

			property double SizeY
			{
				double get() { return _sgBox->SizeY; }
				void set(double value) { _sgBox->SizeY = value; }
			}

			property double SizeZ
			{
				double get() { return _sgBox->SizeZ; }
				void set(double value) { _sgBox->SizeZ = value; }
			}
		internal:
			msgBoxStruct(SG_BOX* box)
			{
				_sgBox = box;
			}
			SG_BOX* _sgBox;
		};
	}
}
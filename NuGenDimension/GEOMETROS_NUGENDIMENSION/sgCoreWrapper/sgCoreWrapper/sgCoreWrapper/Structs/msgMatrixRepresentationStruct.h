#pragma once
#include "Structs/msgDoubleStruct.h"
#include "Helpers/ArrayHelper.h"

using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgMatrixRepresentationStruct
		{
		public:
			property array<msgDoubleStruct^>^ values
			{
				array<msgDoubleStruct^>^ get() { return _doubles; }
			}
		internal:
			msgMatrixRepresentationStruct(double* data)
			{
				_doubles = ArrayHelper::GetManagedArray<msgDoubleStruct, double>(data, 16);
			}
		private:
			array<msgDoubleStruct^>^ _doubles;
		};
	}
}
#pragma once

#include "Structs/msgPointStruct.h"
#include "Structs/msgDoubleStruct.h"
#include "Structs/3D/msgEdgeStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper 
{
	namespace Helpers
	{
		class ArrayHelper
		{
		public:
			template<typename T, typename U> static array<T^>^ GetManagedArray(U* arr, int count) 
			{
				array<T^>^ returnArray = gcnew array<T^>(count);
				for (int i = 0; i < count; i++)
				{
					returnArray[i] = gcnew T(&arr[i]);
				}

				return returnArray;
			}
		};
	}
}
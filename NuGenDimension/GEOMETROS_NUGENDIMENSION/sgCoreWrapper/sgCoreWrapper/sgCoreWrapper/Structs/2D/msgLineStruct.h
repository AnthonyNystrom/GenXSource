#pragma once
#include "sgCore/sgDefs.h"
#include "Structs/msgPointStruct.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgLineStruct
		{
		public:
			msgLineStruct()
			{
				P1 = gcnew msgPointStruct();
				P2 = gcnew msgPointStruct();
			}

			msgPointStruct^ P1;

			msgPointStruct^ P2;

		internal:
			msgLineStruct(SG_LINE* sgLine)
			{
				P1 = gcnew msgPointStruct(&sgLine->p1);
				P2 = gcnew msgPointStruct(&sgLine->p2);
			}
		};
	}
}
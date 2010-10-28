#pragma once
#include "sgCore/sg3D.h"
#include "Structs/msgPointStruct.h"
#include "Structs/3D/msgEdgeStruct.h"
#include "Helpers/ArrayHelper.h"

using namespace sgCoreWrapper::Helpers;
using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgBRepPiece
		{
		public:
			void GetLocalGabarits(msgPointStruct^ p_min, msgPointStruct^ p_max)
			{
				_bRepPiece->GetLocalGabarits(*p_min->_point, *p_min->_point);
			}
			
			array<msgPointStruct^>^ GetVertexes()
			{
				return ArrayHelper::GetManagedArray<msgPointStruct, SG_POINT>((SG_POINT*)_bRepPiece->GetVertexes(), 
					_bRepPiece->GetVertexesCount());
			}

			array<msgEdgeStruct^>^ GetEdges()
			{
				return ArrayHelper::GetManagedArray<msgEdgeStruct, SG_EDGE>((SG_EDGE*)_bRepPiece->GetEdges(), 
					_bRepPiece->GetEdgesCount());
			}

			void GetTrianglesRange(int% min_numb, int% max_numb)
			{
				int min_numb_val = min_numb;
				int max_numb_val = max_numb;
				_bRepPiece->GetTrianglesRange(min_numb_val, max_numb_val);
				min_numb = min_numb_val;
				max_numb = max_numb_val;
			}
		internal:
			msgBRepPiece(sgCBRepPiece* bRepPiece)
			{
				_bRepPiece = bRepPiece;
			}
		private:
			sgCBRepPiece* _bRepPiece;
		};
	}
}
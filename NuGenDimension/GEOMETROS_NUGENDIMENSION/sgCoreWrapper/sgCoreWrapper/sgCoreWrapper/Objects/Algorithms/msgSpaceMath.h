#pragma once
#include "sgCore/sgSpaceMath.h"
#include "Structs/msgPointStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgSpaceMath abstract sealed
		{
		public:
			static bool IsPointsOnOneLine(msgPointStruct^ p1,
											msgPointStruct^ p2,
											msgPointStruct^ p3)
			{
				return sgSpaceMath::IsPointsOnOneLine(*p1->_point, *p2->_point, *p3->_point);
			}

			static double PointsDistance(msgPointStruct^ p1, msgPointStruct^ p2)
			{
				return sgSpaceMath::PointsDistance(*p1->_point, *p2->_point);
			}

			static bool NormalVector(msgPointStruct^ vect)
			{
				return sgSpaceMath::NormalVector(*vect->_point);
			}

			static msgVectorStruct^ VectorsAdd(msgVectorStruct^ v1, msgVectorStruct^ v2)
			{
				return gcnew msgVectorStruct(&sgSpaceMath::VectorsAdd(*v1->_point, *v2->_point));
			}

			static msgVectorStruct^ VectorsSub(msgVectorStruct^ v1, msgVectorStruct^ v2)
			{
				return gcnew msgVectorStruct(&sgSpaceMath::VectorsSub(*v1->_point, *v2->_point));
			}

			static double VectorsScalarMult(msgVectorStruct^ v1, msgVectorStruct^ v2)
			{
				return sgSpaceMath::VectorsScalarMult(*v1->_point, *v2->_point);
			}

			static msgVectorStruct^ VectorsVectorMult(msgVectorStruct^ v1, msgVectorStruct^ v2)
			{
				return gcnew msgVectorStruct(&sgSpaceMath::VectorsVectorMult(*v1->_point, *v2->_point));
			}

			static double ProjectPointToLineAndGetDist(msgPointStruct^ lineP, 
														    msgVectorStruct^ lineDir, 
															msgPointStruct^ pnt, 
															msgPointStruct^ resPnt)
			{
				return sgSpaceMath::ProjectPointToLineAndGetDist(*lineP->_point, *lineDir->_point, 
					*pnt->_point, *resPnt->_point);
			}
		};
	}
}
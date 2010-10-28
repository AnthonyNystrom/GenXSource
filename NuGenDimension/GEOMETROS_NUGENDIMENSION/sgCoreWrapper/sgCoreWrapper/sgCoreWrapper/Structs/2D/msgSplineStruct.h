#pragma once
#include "sgCore/sgDefs.h"

#include "Helpers/ArrayHelper.h"

using namespace sgCoreWrapper::Helpers;

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgSplineStruct
		{
		public:
			static msgSplineStruct^ Create()
			{
				return gcnew msgSplineStruct(SG_SPLINE::Create());
			}
			static void Delete(msgSplineStruct^ spline)
			{
				SG_SPLINE::Delete(spline->_sgSpline);
			}

			~msgSplineStruct()
			{
				this->~msgSplineStruct();
			}

			!msgSplineStruct()
			{
				SG_SPLINE::Delete(_sgSpline);
			}

			bool AddKnot(msgPointStruct^ pnt, int nmbr)
			{
				return _sgSpline->AddKnot(*pnt->_point, nmbr);
			}

			bool MoveKnot(int nmbr, msgPointStruct^ pnt)
			{
				return _sgSpline->MoveKnot(nmbr, *pnt->_point);
			}

			bool DeleteKnot(int nmbr)
			{
				return _sgSpline->DeleteKnot(nmbr);
			}

			bool IsClosed()
			{
				return _sgSpline->IsClosed();
			}

			bool Close()
			{
				return _sgSpline->Close();
			}

			bool UnClose(int nmbr)
			{
				return _sgSpline->UnClose(nmbr);
			}
			
			array<msgPointStruct^>^ GetPoints()
			{
				return ArrayHelper::GetManagedArray<msgPointStruct, SG_POINT>((SG_POINT*)_sgSpline->GetPoints(), 
					_sgSpline->GetPointsCount());
			}

			array<msgPointStruct^>^ GetKnots()
			{
				return ArrayHelper::GetManagedArray<msgPointStruct, SG_POINT>((SG_POINT*)_sgSpline->GetKnots(), 
					_sgSpline->GetKnotsCount());
			}
		internal:
			msgSplineStruct(SG_SPLINE* spline)
			{
				_sgSpline = spline;
			}

			SG_SPLINE* _sgSpline;
		};
	}
}
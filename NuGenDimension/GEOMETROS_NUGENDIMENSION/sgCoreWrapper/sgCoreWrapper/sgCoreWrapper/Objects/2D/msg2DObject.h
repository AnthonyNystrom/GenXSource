#pragma once
#include "sgCore\sg2D.h"
#include "Structs\msgPointStruct.h"
#include "Objects\msgObject.h"

namespace sgCoreWrapper
{
	namespace Objects
	{
		ref class msgContour;

		public enum msg2DObjectOrientEnum
		{
			OO_ERROR = 0,
			OO_CLOCKWISE,
			OO_ANTICLOCKWISE
		};

		public ref class msg2DObject abstract : msgObject
		{
		public:
			static bool IsObjectsOnOnePlane(msg2DObject^ obj1, msg2DObject^ obj2)
			{
				return sgC2DObject::IsObjectsOnOnePlane(*obj1->sg2DObject, *obj2->sg2DObject);
			}
			static bool IsObjectsIntersecting(msg2DObject^ obj1, msg2DObject^ obj2)
			{
				return sgC2DObject::IsObjectsIntersecting(*obj1->sg2DObject, *obj2->sg2DObject);
			}
			static bool IsFirstObjectInsideSecondObject(msg2DObject^ obj1, msg2DObject^ obj2)
			{
				return sgC2DObject::IsFirstObjectInsideSecondObject(*obj1->sg2DObject, *obj2->sg2DObject);
			}

			virtual bool IsClosed() abstract;
			virtual bool IsPlane(msgVectorStruct^ vec, double value) abstract;
			virtual bool IsLinear() abstract;
			virtual bool IsSelfIntersecting() abstract;

			msgContour^ GetEquidistantContour(double h1, double h2, bool toRound);
			
			msg2DObjectOrientEnum GetOrient(msgVectorStruct^ planeNormal)
			{
				return (msg2DObjectOrientEnum)sg2DObject->GetOrient(*planeNormal->_point);
			}

			bool ChangeOrient()
			{
				return sg2DObject->ChangeOrient();
			}

			msgPointStruct^ GetPointFromCoefficient(double coeff)
			{
				return gcnew msgPointStruct(&sg2DObject->GetPointFromCoefficient(coeff));
			}
		protected:
			msg2DObject(sgC2DObject* object) : msgObject(object)
			{
			}
		internal:
			property sgC2DObject* sg2DObject
			{
				sgC2DObject* get() { return (sgC2DObject*)_sgCObject; }
			}
		};
	}
}
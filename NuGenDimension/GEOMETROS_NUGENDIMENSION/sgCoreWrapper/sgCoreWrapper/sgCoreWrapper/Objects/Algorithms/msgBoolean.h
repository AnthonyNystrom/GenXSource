#pragma once
#include "sgCore/sgAlgs.h"
#include "Objects/msgGroup.h"
#include "Objects/3D/msg3DObject.h"
#include "Structs/msgPointStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgBoolean abstract sealed
		{
		public:
			static msgGroup^ Intersection(msg3DObject^ aOb, msg3DObject^ bOb)
			{
				return gcnew msgGroup(sgBoolean::Intersection(*aOb->sg3DObject, 
					*bOb->sg3DObject));
			}

			static msgGroup^ Union(msg3DObject^ aOb, msg3DObject^ bOb)
			{
				return gcnew msgGroup(sgBoolean::Union(*aOb->sg3DObject, 
					*bOb->sg3DObject));
			}

			static msgGroup^ Sub(msg3DObject^ aOb, msg3DObject^ bOb)
			{
				return gcnew msgGroup(sgBoolean::Sub(*aOb->sg3DObject, 
					*bOb->sg3DObject));
			}

			static msgGroup^ IntersectionContour(msg3DObject^ aOb, msg3DObject^ bOb)
			{
				return gcnew msgGroup(sgBoolean::IntersectionContour(*aOb->sg3DObject, 
					*bOb->sg3DObject));
			}

			static msgGroup^ Section(msg3DObject^ obj, msgVectorStruct^ planeNormal, double planeD)
			{
				return gcnew msgGroup(sgBoolean::Section(*obj->sg3DObject, 
					*planeNormal->_point, planeD));
			}
		};
	}
}
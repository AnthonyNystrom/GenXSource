#pragma once
#include "sgCore\sg3D.h"
#include "Objects\3D\msg3DObject.h"
#include "Structs\3D\msgConeStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgCone : msg3DObject
		{
		public:
			static msgCone^ Create(double rad_1,double rad_2,double heig, short merid)
			{
				return gcnew msgCone(sgCCone::Create(rad_1, rad_2, heig, merid));
			}

			void GetGeometry(msgConeStruct^ cone)
			{
				sgCone->GetGeometry(*cone->_sgCone);
			}
		protected:
			msgCone(sgCCone* cone) : msg3DObject(cone)
			{
			}
		internal:
			property sgCCone* sgCone
			{
				sgCCone* get() { return (sgCCone*)_sgCObject; }
			}
		};
	}
}
#pragma once
#include "sgCore\sg3D.h"
#include "Objects\3D\msg3DObject.h"
#include "Structs\3D\msgSphereStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgSphere : msg3DObject
		{
		public:
			static msgSphere^ Create(double rad, short merid, short parall)
			{
				return gcnew msgSphere(sgCSphere::Create(rad, merid, parall));
			}

			void GetGeometry(msgSphereStruct^ sphere)
			{
				sgSphere->GetGeometry(*sphere->_sgSphere);
			}
		protected:
			msgSphere(sgCSphere* sphere) : msg3DObject(sphere)
			{
			}
		internal:
			property sgCSphere* sgSphere
			{
				sgCSphere* get() { return (sgCSphere*)_sgCObject; }
			}
		};
	}
}
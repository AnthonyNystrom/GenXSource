#pragma once
#include "sgCore\sg3D.h"
#include "Objects\3D\msg3DObject.h"
#include "Structs\3D\msgTorusStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgTorus : msg3DObject
		{
		public:
			static msgTorus^ Create(double r1,double r2,short m1,short m2)
			{
				return gcnew msgTorus(sgCTorus::Create(r1, r2, m1, m2));
			}

			void GetGeometry(msgTorusStruct^ torus)
			{
				sgTorus->GetGeometry(*torus->_sgTorus);
			}
		protected:
			msgTorus(sgCTorus* torus) : msg3DObject(torus)
			{
			}
		internal:
			property sgCTorus* sgTorus
			{
				sgCTorus* get() { return (sgCTorus*)_sgCObject; }
			}
		};
	}
}
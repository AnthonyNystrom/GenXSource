#pragma once
#include "sgCore\sg3D.h"
#include "Objects\3D\msg3DObject.h"
#include "Structs\3D\msgCylinderStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgCylinder : msg3DObject
		{
		public:
			static msgCylinder^ Create(double rad, double heig, short merid)
			{
				return gcnew msgCylinder(sgCCylinder::Create(rad, heig, merid));
			}

			void GetGeometry(msgCylinderStruct^ cylinder)
			{
				sgCylinder->GetGeometry(*cylinder->_sgCylinder);
			}
		protected:
			msgCylinder(sgCCylinder* cylinder) : msg3DObject(cylinder)
			{
			}
		internal:
			property sgCCylinder* sgCylinder
			{
				sgCCylinder* get() { return (sgCCylinder*)_sgCObject; }
			}
		};
	}
}
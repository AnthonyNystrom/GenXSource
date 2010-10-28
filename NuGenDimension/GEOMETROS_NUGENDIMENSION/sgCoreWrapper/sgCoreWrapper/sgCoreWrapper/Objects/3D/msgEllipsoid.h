#pragma once
#include "sgCore\sg3D.h"
#include "Objects\3D\msg3DObject.h"
#include "Structs\3D\msgEllipsoidStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgEllipsoid : msg3DObject
		{
		public:
			static msgEllipsoid^ Create(double radius1, double radius2, double radius3,
				short merid_cnt, short parall_cnt)
			{
				return gcnew msgEllipsoid(sgCEllipsoid::Create(radius1, radius2, radius3, 
					merid_cnt, parall_cnt));
			}

			void GetGeometry(msgEllipsoidStruct^ ellipsoid)
			{
				sgEllipsoid->GetGeometry(*ellipsoid->_sgEllipsoid);
			}
		protected:
			msgEllipsoid(sgCEllipsoid* ellipsoid) : msg3DObject(ellipsoid)
			{
			}
		internal:
			property sgCEllipsoid* sgEllipsoid
			{
				sgCEllipsoid* get() { return (sgCEllipsoid*)_sgCObject; }
			}
		};
	}
}
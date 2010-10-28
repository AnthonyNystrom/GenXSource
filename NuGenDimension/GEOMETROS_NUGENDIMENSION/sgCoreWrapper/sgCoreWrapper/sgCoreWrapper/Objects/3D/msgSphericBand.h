#pragma once
#include "sgCore\sg3D.h"
#include "Objects\3D\msg3DObject.h"
#include "Structs\3D\msgSphericBandStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgSphericBand : msg3DObject
		{
		public:
			static msgSphericBand^ Create(double radius, double beg_koef, double end_koef,
				short merid_cnt)
			{
				return gcnew msgSphericBand(sgCSphericBand::Create(radius, beg_koef, end_koef, 
					merid_cnt));
			}

			void GetGeometry(msgSphericBandStruct^ sphericBand)
			{
				sgSphericBand->GetGeometry(*sphericBand->_sgSphericBand);
			}
		protected:
			msgSphericBand(sgCSphericBand* sphericBand) : msg3DObject(sphericBand)
			{
			}
		internal:
			property sgCSphericBand* sgSphericBand
			{
				sgCSphericBand* get() { return (sgCSphericBand*)_sgCObject; }
			}
		};
	}
}
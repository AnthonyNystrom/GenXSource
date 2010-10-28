#pragma once
#include "sgCore\sg3D.h"
#include "Objects\3D\msg3DObject.h"
#include "Structs\3D\msgBoxStruct.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgBox : msg3DObject
		{
		public:
			static msgBox^ Create(double sizeX, double sizeY, double sizeZ)
			{
				return gcnew msgBox(sgCBox::Create(sizeX, sizeY, sizeZ));				
			}

			void GetGeometry(msgBoxStruct^ box)
			{
				sgBox->GetGeometry(*box->_sgBox);
			}
		protected:
			msgBox(sgCBox* box) : msg3DObject(box)
			{
			}
		internal:
			property sgCBox* sgBox
			{
				sgCBox* get() { return (sgCBox*)_sgCObject; }
			}
		};
	}
}
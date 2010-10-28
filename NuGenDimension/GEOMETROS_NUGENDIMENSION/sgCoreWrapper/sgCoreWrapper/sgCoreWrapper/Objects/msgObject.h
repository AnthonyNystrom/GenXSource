#pragma once
#include "sgCore/sgObject.h"
#include "Structs/msgPointStruct.h"
#include "Structs/msgUserDynamicDataStruct.h"
#include "Objects/msgMatrix.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Runtime::Serialization::Formatters::Binary;
using namespace System::IO;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public enum class msgObjectTypeEnum 
		{ 
			SG_OT_BAD_OBJECT,
			SG_OT_POINT,
			SG_OT_LINE,
			SG_OT_CIRCLE,
			SG_OT_ARC,
			SG_OT_SPLINE,
			SG_OT_TEXT,
			SG_OT_CONTOUR,
			SG_OT_DIM,
			SG_OT_3D,
			SG_OT_GROUP 
		};

		public enum class msgObjectAttrEnum 
		{
			SG_OA_COLOR,
			SG_OA_LINE_TYPE,
			SG_OA_LINE_THICKNESS,
			SG_OA_LAYER,
			SG_OA_DRAW_STATE
		};

		public enum class SG_OA_DRAW_STATEValuesEnum : unsigned short
		{
			SGDS_FRAME = 512,
			SGDS_HIDE = 256,
			SGDS_GABARITE = 128,
			SGDS_FULL = 64,
		};

		//bool                SetUserDynamicData(const SG_USER_DYNAMIC_DATA* u_d_d);
		//SG_USER_DYNAMIC_DATA*  GetUserDymanicData() const;
		public ref class msgObject
		{
		public:
			static void DeleteObject(msgObject^ obj)
			{
				sgCObject::DeleteObject(obj->_sgCObject);
			}

			msgObject^ Clone()
			{
				sgCObject* clone = _sgCObject->Clone();
				return gcnew msgObject(clone);
			}

			msgObjectTypeEnum GetType()
			{
				return (msgObjectTypeEnum)_sgCObject->GetType();
			}

			String^ GetName()
			{
				return gcnew String(_sgCObject->GetName());
			}

			bool SetName(String^ object_name)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(object_name);
				const char* str = (const char*) ip.ToPointer();

				bool returnValue = _sgCObject->SetName(str);

				Marshal::FreeHGlobal(ip);
				return returnValue;
			}

			msgObject^ GetParent()
			{
				return gcnew msgObject((sgCObject*)_sgCObject->GetParent());
			}

			void GetGabarits(msgPointStruct^ p_min, msgPointStruct^ p_max)
			{
				_sgCObject->GetGabarits(*p_min->_point, *p_max->_point);
			}

			void Select(bool sel)
			{
				_sgCObject->Select(sel);
			}

			bool IsSelect()
			{
				return _sgCObject->IsSelect();
			}

			bool IsAttachedToScene()
			{
				return _sgCObject->IsAttachedToScene();
			}

			msgMatrix^ InitTempMatrix()
			{
				return gcnew msgMatrix(_sgCObject->InitTempMatrix());
			}

			bool DestroyTempMatrix()
			{
				return _sgCObject->DestroyTempMatrix();
			}

			msgMatrix^ GetTempMatrix()
			{
				sgCMatrix* matrix = _sgCObject->GetTempMatrix();
				if (matrix)
				{
					return gcnew msgMatrix(matrix);
				}
				else
				{
					return nullptr;
				}
			}

			virtual bool ApplyTempMatrix()
			{
				return _sgCObject->ApplyTempMatrix();
			}

			bool SetUserGeometry(String^ user_geometry_ID, Object^ user_geometry_data)
			{
				BinaryFormatter^ formatter = gcnew BinaryFormatter(); 
				MemoryStream^ stream = gcnew MemoryStream();
				formatter->Serialize(stream, user_geometry_data);
				array<unsigned char>^ serdata = stream->ToArray();
				pin_ptr<void> pin = &serdata[0];

				IntPtr ip = Marshal::StringToHGlobalAnsi(user_geometry_ID);
				const char* str = (const char*) ip.ToPointer();
				bool result = _sgCObject->SetUserGeometry(str, serdata->Length, pin);
				Marshal::FreeHGlobal(ip);

				return result;
			}

			String^ GetUserGeometryID()
			{
				return gcnew String(_sgCObject->GetUserGeometryID());
			}

			Object^ GetUserGeometry()
			{
				unsigned short size = 0;
				const void* data = _sgCObject->GetUserGeometry(size);
				UnmanagedMemoryStream^ stream = gcnew UnmanagedMemoryStream((unsigned char*)data, 
					size);

				BinaryFormatter^ formatter = gcnew BinaryFormatter();
				return formatter->Deserialize(stream);
			}

			bool SetUserDynamicData(msgUserDynamicDataStruct^ u_d_d)
			{
				_msgUserDynamicDataStruct = u_d_d;
				return _sgCObject->SetUserDynamicData(u_d_d->_msgUserDynamicDataStructHelper);
			}
			
			msgUserDynamicDataStruct^ GetUserDymanicData()
			{
				return _msgUserDynamicDataStruct;
			}

			unsigned short GetAttribute(msgObjectAttrEnum attributeId)
			{
				return _sgCObject->GetAttribute((SG_OBJECT_ATTR_ID)attributeId);
			}
		
			virtual bool SetAttribute(msgObjectAttrEnum attributeId, unsigned short attributeValue)
			{
				return _sgCObject->SetAttribute((SG_OBJECT_ATTR_ID)attributeId, attributeValue);
			}
		internal:
			msgObject(sgCObject* sgCObject)
			{
				_sgCObject = sgCObject;
			}

			sgCObject* _sgCObject;

		private:
			msgUserDynamicDataStruct^ _msgUserDynamicDataStruct;
		};
	}
}
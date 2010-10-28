#pragma once
#include "sgCore/sgFileManager.h"
#include "Structs/msgFileHeaderStruct.h"
#include "Structs/3D/msgAllTrianglesStruct.h"
#include "Objects/msgScene.h"
#include "Helpers/ObjectCreateHelper.h"

using namespace System::Runtime::Serialization::Formatters::Binary;
using namespace System::IO;

namespace sgCoreWrapper
{
	namespace Objects
	{
		/*sgCore_API const void*  ObjectToBitArray(const sgCObject* obj, unsigned long& arrSize);
		sgCore_API sgCObject*   BitArrayToObject(const void* bitArray, unsigned long arrSize);*/
		public ref class msgFileManager abstract sealed
		{
		public:
			static bool Save(msgScene^ scen, String^ file_name, Object^ userData)
			{
				BinaryFormatter^ formatter = gcnew BinaryFormatter(); 
				MemoryStream^ stream = gcnew MemoryStream();
				formatter->Serialize(stream, userData);
				array<unsigned char>^ serdata = stream->ToArray();
				pin_ptr<void> pin = &serdata[0];

				IntPtr ip = Marshal::StringToHGlobalAnsi(file_name);
				const char* str = (const char*) ip.ToPointer();
				bool result = sgFileManager::Save(scen->_sgScene, str, pin, serdata->Length);
				Marshal::FreeHGlobal(ip);

				return result;
			}

			static bool GetFileHeader(String^ file_name, msgFileHeaderStruct^ file_header)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(file_name);
				bool result = sgFileManager::GetFileHeader((const char*) ip.ToPointer(), 
					*file_header->_sgFileHeader);
				Marshal::FreeHGlobal(ip);
				return result;
			}

			static Object^ GetUserData(String^ file_name)
			{
				msgFileHeaderStruct^ fileHeader = gcnew msgFileHeaderStruct();
				GetFileHeader(file_name, fileHeader);
				void* data = 0;
				IntPtr ip = Marshal::StringToHGlobalAnsi(file_name);
				const char* str = (const char*) ip.ToPointer();
				if (!sgFileManager::GetUserData(str, data))
				{
					return nullptr;
				}
				Marshal::FreeHGlobal(ip);
				UnmanagedMemoryStream^ stream = gcnew UnmanagedMemoryStream((unsigned char*)data, 
					fileHeader->userBlockSize);
				BinaryFormatter^ formatter = gcnew BinaryFormatter();
				return formatter->Deserialize(stream);
			}

			static bool Open(msgScene^ scen, String^ file_name)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(file_name);
				const char* str = (const char*) ip.ToPointer();
				bool result = sgFileManager::Open(scen->_sgScene, str);
				Marshal::FreeHGlobal(ip);
				return result;
			}

			static bool ExportDXF(msgScene^ scen, String^ file_name)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(file_name);
				const char* str = (const char*) ip.ToPointer();
				bool result = sgFileManager::ExportDXF(scen->_sgScene, str);
				Marshal::FreeHGlobal(ip);
				return result;
			}

			static bool ImportDXF(msgScene^ scen, String^ file_name)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(file_name);
				const char* str = (const char*) ip.ToPointer();
				bool result = sgFileManager::ImportDXF(scen->_sgScene, str);
				Marshal::FreeHGlobal(ip);
				return result;
			}

			static bool ExportSTL(msgScene^ scen, String^ file_name)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(file_name);
				const char* str = (const char*) ip.ToPointer();
				bool result = sgFileManager::ExportDXF(scen->_sgScene, str);
				Marshal::FreeHGlobal(ip);
				return result;
			}

			static bool ImportSTL(msgScene^ scen, String^ file_name)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(file_name);
				const char* str = (const char*) ip.ToPointer();
				bool result = sgFileManager::ExportDXF(scen->_sgScene, str);
				Marshal::FreeHGlobal(ip);
				return result;
			}

			static msgObject^ ObjectFromTriangles(array<msgPointStruct^>^ pnts, array<msgIndexTriangle^>^ trngls,
													float smooth_angle_in_radians,
													bool  solids_checking)
			{
				int pnts_count = pnts->Length;
				SG_POINT* points = new SG_POINT[pnts_count];
				for (int i = 0; i < pnts_count; i++)
				{
					points[i] = *pnts[i]->_point;
				}

				int trngls_count = trngls->Length;
				SG_INDEX_TRIANGLE* ind_tr = new SG_INDEX_TRIANGLE[trngls_count];
				for (int i = 0; i < trngls_count; i++)
				{
					ind_tr[i] = *trngls[i]->_ind_triangle;
				}

				msgObject^ result = ObjectCreateHelper::CreateObject(sgFileManager::ObjectFromTriangles(points, pnts_count, ind_tr, trngls_count, smooth_angle_in_radians,solids_checking));

				delete[] ind_tr;
				delete[] points;
				return result;
			}
		};
	}
}
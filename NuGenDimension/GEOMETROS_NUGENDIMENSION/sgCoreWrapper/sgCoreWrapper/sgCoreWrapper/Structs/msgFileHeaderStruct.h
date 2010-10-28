#pragma once
#include "sgCore/sgFileManager.h"

using namespace sgFileManager;
using namespace System;
using namespace System::Runtime::InteropServices;

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgFileHeaderStruct
		{
		public:
			msgFileHeaderStruct()
			{
				_sgFileHeader = new SG_FILE_HEADER();
			}
			
			~msgFileHeaderStruct()
			{
				this->!msgFileHeaderStruct();
			}

			!msgFileHeaderStruct()
			{
				delete _sgFileHeader;
			}

			property array<wchar_t>^ signature
			{
				array<wchar_t>^ get()
				{
					array<wchar_t>^ msignature = gcnew array<System::Char>(5);
					IntPtr ptr(_sgFileHeader->signature);
					Marshal::Copy(ptr, msignature, 0, 5);
					return msignature;
				}
				void set(array<wchar_t>^ value)
				{
					IntPtr ptr(_sgFileHeader->signature);
					Marshal::Copy(value, 0, ptr, 5);
				}
			}

			property int major_ver
			{
				int get() { return _sgFileHeader->major_ver; }
				void set(int value) { _sgFileHeader->major_ver = value; }
			}

			property int minor_ver
			{
				int get() { return _sgFileHeader->minor_ver; }
				void set(int value) { _sgFileHeader->minor_ver = value; }
			}

			property UInt32 userBlockSize
			{
				UInt32 get() { return _sgFileHeader->userBlockSize; }
				void set(UInt32 value) { _sgFileHeader->userBlockSize = value; }
			}
		internal:
			SG_FILE_HEADER* _sgFileHeader;
		};
	}
}
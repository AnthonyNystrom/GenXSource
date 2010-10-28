#pragma once
#include "sgCore/sgTD.h"
#include <string.h>

using namespace System;
using namespace System::Runtime::InteropServices;

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgFontDataStruct
		{
		public:
			property String^ name
			{
				String^ get()
				{
					return gcnew String(_sgFontData->name);
				}

				void set(String^ value)
				{
					String^ valueSubstr = value->Substring(0, 14);
					IntPtr ip = Marshal::StringToHGlobalAnsi(valueSubstr);
					const char* str = static_cast<const char*>(ip.ToPointer());

					strncpy_s(_sgFontData->name, 15, str, 14);

					Marshal::FreeHGlobal( ip );
				}
			}
			
			property unsigned short table_begin
			{
				unsigned short get()
				{
					return _sgFontData->table_begin;
				}

				void set(unsigned short value)
				{
					_sgFontData->table_begin = value;
				}
			}

			property unsigned short table_end
			{
				unsigned short get()
				{
					return _sgFontData->table_end;
				}

				void set(unsigned short value)
				{
					_sgFontData->table_end = value;
				}
			}

			property unsigned short table_size
			{
				unsigned short get()
				{
					return _sgFontData->table_size;
				}

				void set(unsigned short value)
				{
					_sgFontData->table_size = value;
				}
			}

			property unsigned char posit_size
			{
				unsigned char get()
				{
					return _sgFontData->posit_size;
				}

				void set(unsigned char value)
				{
					_sgFontData->posit_size = value;
				}
			}

			property unsigned char negat_size
			{
				unsigned char get()
				{
					return _sgFontData->negat_size;
				}

				void set(unsigned char value)
				{
					_sgFontData->negat_size = value;
				}
			}

			property unsigned char state
			{
				unsigned char get()
				{
					return _sgFontData->state;
				}

				void set(unsigned char value)
				{
					_sgFontData->state = value;
				}
			}

			property double proportion
			{
				double get()
				{
					return _sgFontData->proportion;
				}

				void set(double value)
				{
					_sgFontData->proportion = value;
				}
			}

//			array<unsigned char>^ symbols_table;

		internal:
			msgFontDataStruct(SG_FONT_DATA* sgFontData)
			{
				_sgFontData = sgFontData;
			}

			SG_FONT_DATA* _sgFontData;
		};
	}
}
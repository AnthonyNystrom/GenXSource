#pragma once
#include "sgCore/sgTD.h"
#include "Structs/Text/msgFontDataStruct.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgFont
		{
		public:
			static msgFont^ LoadFont(String^ path, String^% comment)
			{
				IntPtr pathIp = Marshal::StringToHGlobalAnsi(path);
				const char* pathStr = static_cast<const char*>(pathIp.ToPointer());
				char commentArr[100];
				msgFont^ font = gcnew msgFont(sgCFont::LoadFont(pathStr, commentArr, 100));
				comment = gcnew String(commentArr);
				Marshal::FreeHGlobal(pathIp);
				return font;
			}

			static bool UnloadFont(msgFont^ font)
			{
				return sgCFont::UnloadFont(font->_sgFont);
			}

			msgFontDataStruct^ GetFontData()
			{
				return gcnew msgFontDataStruct((SG_FONT_DATA*)_sgFont->GetFontData());
			}

		internal:
			msgFont(sgCFont* sgFont)
			{
				_sgFont = sgFont;
			}

			sgCFont* _sgFont;
		};
	}
}
#pragma once

#include "sgCore/sgTD.h"
#include "Objects/Text/msgFont.h"

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgFontManager abstract sealed
		{
		public:
			static bool	AttachFont(msgFont^ font)
			{
				return sgFontManager::AttachFont(font->_sgFont);
			}
			
			static unsigned int GetFontsCount()
			{
				return sgFontManager::GetFontsCount();
			}

			static msgFont^ GetFont(unsigned int index)
			{
				return gcnew msgFont((sgCFont*)sgFontManager::GetFont(index));
			}

			static bool SetCurrentFont(unsigned int index)
			{
				return sgFontManager::SetCurrentFont(index);
			}

			static unsigned int GetCurrentFont()
			{
				return sgFontManager::GetCurrentFont();
			}
		};
	}
}
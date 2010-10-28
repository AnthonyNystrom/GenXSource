#pragma once
#include "sgCore/sgTD.h"
#include "Objects/msgObject.h"
#include "Objects/msgMatrix.h"
#include "Structs/Text/msgTextStyleStruct.h"
#include "Structs/2D/msgLineStruct.h"
#include "Helpers/DrawLineCallbackHelper.h"

using namespace sgCoreWrapper::Helpers;
using namespace sgCoreWrapper::Structs;

using namespace System;
using namespace System::Runtime::InteropServices;

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgText : msgObject
		{
		public:
			static msgText^ Create(msgFont^ font, msgTextStyleStruct^ textStyle, String^ textStr)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(textStr);
				const char* str = static_cast<const char*>(ip.ToPointer());

				msgText^ text = gcnew msgText(sgCText::Create(font->_sgFont, *textStyle->_sgTextStyle, str));

				Marshal::FreeHGlobal(ip);
				
				return text;
			}

			static bool Draw(msgFont^ font, const msgTextStyleStruct^ style, 
				msgMatrix^ matrix, String^ textStr, DrawLineDelegate^ drawLineDelegate)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(textStr);
				const char* str = static_cast<const char*>(ip.ToPointer());

				DrawLineCallbackHelper::DrawLineDel = drawLineDelegate;
				bool result = sgCText::Draw(font->_sgFont, *style->_sgTextStyle, matrix->_sgCMatrix, str,
					(SG_DRAW_LINE_FUNC)DrawLineCallbackHelper::uDrawLineFunction);

				Marshal::FreeHGlobal(ip);

				return result;
			}

			array<msgLineStruct^>^ GetLines()
			{
				int count = sgText->GetLinesCount();
				array<msgLineStruct^>^ lines = gcnew array<msgLineStruct^>(count);
				for (int i = 0; i < count; i++)
				{
					const SG_LINE* sgLines = sgText->GetLines();
					lines[i] = gcnew msgLineStruct((SG_LINE*)&sgLines[i]);
				}
				return lines;
			}

			msgTextStyleStruct^ GetStyle()
			{
				return gcnew msgTextStyleStruct(&sgText->GetStyle());
			}
			
			String^ GetText()
			{
				return gcnew String(sgText->GetText());
			}

			unsigned int GetFont()
			{
				return sgText->GetFont();
			}

			bool GetWorldMatrix(msgMatrix^% mtrx)
			{
				mtrx = gcnew msgMatrix();
				return sgText->GetWorldMatrix(*mtrx->_sgCMatrix);
			}

			virtual bool ApplyTempMatrix() override
			{
				return sgText->ApplyTempMatrix();
			}		

		internal:
			msgText(sgCText* sgText) : msgObject(sgText)
			{
			}

			property sgCText* sgText
			{
				sgCText* get() { return (sgCText*)_sgCObject; }
			}
		};
	}
}
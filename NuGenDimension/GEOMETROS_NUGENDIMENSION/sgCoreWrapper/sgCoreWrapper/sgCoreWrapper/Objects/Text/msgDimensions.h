#pragma once
#include "sgCore/sgTD.h"
#include "Structs/Text/msgDimensionStyleStruct.h"
#include "Objects/Text/msgFont.h"
#include "Objects/msgObject.h"

using namespace sgCoreWrapper::Structs;

namespace sgCoreWrapper
{
	namespace Objects
	{
		/*const SG_POINT*      GetFormedPoints();*/

		public enum class msgDimensionTypeEnum
		{
			SG_DT_LINEAR,
			SG_DT_ANGLE,
			SG_DT_RAD,
			SG_DT_DIAM
		};

		public ref class msgDimensions : msgObject
		{
		public:
			static msgDimensions^ Create(msgDimensionTypeEnum dimensionType, msgPointStruct^ point,
					msgFont^ font, msgDimensionStyleStruct^ dimensionStyle,	String^ text)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(text);
				const char* str = static_cast<const char*>(ip.ToPointer());

				msgDimensions^ dim =  gcnew msgDimensions(sgCDimensions::Create((SG_DIMENSION_TYPE)dimensionType, 
					point->_point, font->_sgFont, *dimensionStyle->_sgDimensionStyle, str));

				Marshal::FreeHGlobal(ip);

				return dim;
			}

			static bool Draw(msgDimensionTypeEnum dimensionType, msgPointStruct^ point,
						msgFont^ font, msgDimensionStyleStruct^ dimensionStyle,
						String^ text, DrawLineDelegate^ drawLineDelegate)
			{
				IntPtr ip = Marshal::StringToHGlobalAnsi(text);
				const char* str = static_cast<const char*>(ip.ToPointer());

				DrawLineCallbackHelper::DrawLineDel = drawLineDelegate;
				bool result = sgCDimensions::Draw((SG_DIMENSION_TYPE)dimensionType, point->_point,
					font->_sgFont, *dimensionStyle->_sgDimensionStyle, str, 
					(SG_DRAW_LINE_FUNC)DrawLineCallbackHelper::uDrawLineFunction);

				Marshal::FreeHGlobal(ip);

				return result;
			}

			array<msgLineStruct^>^ GetLines()
			{
				int count = sgDimensions->GetLinesCount();
				array<msgLineStruct^>^ lines = gcnew array<msgLineStruct^>(count);
				for (int i = 0; i < count; i++)
				{
					const SG_LINE* sgLines = sgDimensions->GetLines();
					lines[i] = gcnew msgLineStruct((SG_LINE*)&sgLines[i]);
				}
				return lines;
			}

			msgDimensionTypeEnum GetType()
			{
				return (msgDimensionTypeEnum)sgDimensions->GetType();
			}
			
			msgDimensionStyleStruct^ GetStyle()
			{
				return gcnew msgDimensionStyleStruct(&sgDimensions->GetStyle());
			}
			
			String^ GetText()
			{
				return gcnew String(sgDimensions->GetText());
			}
			
			unsigned int GetFont()
			{
				return sgDimensions->GetFont();
			}

			virtual bool ApplyTempMatrix() override
			{
				return sgDimensions->ApplyTempMatrix();
			}

		internal:
			msgDimensions(sgCDimensions* sgDimensions) : msgObject(sgDimensions)
			{}

			property sgCDimensions* sgDimensions
			{
				sgCDimensions* get() { return (sgCDimensions*)_sgCObject; }
			}
		};
	}
}
#pragma once

#include "sgCore/sgTD.h"
#include "Structs/Text/msgTextStyleStruct.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public enum class msgTextAlignEnum
		{
			SG_TA_CENTER,
			SG_TA_LEFT,
			SG_TA_RIGHT
		};

		public enum class msgDimensionBehaviourEnum
		{
			SG_DBT_VERTICAL,
			SG_DBT_HORIZONTAL,
			SG_DBT_PARALLEL,
			SG_DBT_SLANT,
			SG_DBT_OPTIMAL
		};

		public ref struct msgDimensionStyleStruct
		{
		public:
			msgDimensionStyleStruct()
			{
				_sgDimensionStyle = new SG_DIMENSION_STYLE();
				text_style = gcnew msgTextStyleStruct(&_sgDimensionStyle->text_style);

				_needDelete = true;
			}

			~msgDimensionStyleStruct()
			{
				this->!msgDimensionStyleStruct();
			}

			!msgDimensionStyleStruct()
			{
				if (_needDelete)
				{
					delete _sgDimensionStyle;
				}
			}

			property bool dimension_line
			{
				bool get() { return _sgDimensionStyle->dimension_line; }
				void set(bool value) { _sgDimensionStyle->dimension_line = value; }
			}

			property bool first_side_line
			{
				bool get() { return _sgDimensionStyle->first_side_line; }
				void set(bool value) { _sgDimensionStyle->first_side_line = value; }
			}
			
			property bool second_side_line
			{
				bool get() { return _sgDimensionStyle->second_side_line; }
				void set(bool value) { _sgDimensionStyle->second_side_line = value; }
			}
			
			property double lug_size
			{
				double get() { return _sgDimensionStyle->lug_size; }
				void set(double value) { _sgDimensionStyle->lug_size = value; }
			}

			property bool automatic_arrows
			{
				bool get() { return _sgDimensionStyle->automatic_arrows; }
				void set(bool value) { _sgDimensionStyle->automatic_arrows = value; }
			}

			property bool out_first_arrow
			{
				bool get() { return _sgDimensionStyle->out_first_arrow; }
				void set(bool value) { _sgDimensionStyle->out_first_arrow = value; }
			}

			property unsigned char first_arrow_style
			{
				unsigned char get() { return _sgDimensionStyle->first_arrow_style; }
				void set(unsigned char value) { _sgDimensionStyle->first_arrow_style = value; }
			}

			property bool out_second_arrow
			{
				bool get() { return _sgDimensionStyle->out_second_arrow; }
				void set(bool value) { _sgDimensionStyle->out_second_arrow = value; }
			}

			property unsigned char second_arrow_style
			{
				unsigned char get() { return _sgDimensionStyle->second_arrow_style; }
				void set(unsigned char value) { _sgDimensionStyle->second_arrow_style = value; }
			}

			property double arrows_size
			{
				double get() { return _sgDimensionStyle->arrows_size; }
				void set(double value) { _sgDimensionStyle->arrows_size = value; }
			}

			property msgTextAlignEnum text_align
			{
				msgTextAlignEnum get() { return (msgTextAlignEnum)_sgDimensionStyle->text_align; }
				void set(msgTextAlignEnum value) { _sgDimensionStyle->text_align = (SG_TEXT_ALIGN)value; }
			}

			property bool invert
			{
				bool get() { return _sgDimensionStyle->invert; }
				void set(bool value) { _sgDimensionStyle->invert = value; }
			}

			property msgDimensionBehaviourEnum behaviour_type
			{
				msgDimensionBehaviourEnum get() { return (msgDimensionBehaviourEnum)_sgDimensionStyle->behaviour_type; }
				void set(msgDimensionBehaviourEnum value) { _sgDimensionStyle->behaviour_type = (SG_DIMENSION_BEHAVIOUR)value; }
			}

			property unsigned short precision
			{
				unsigned short get() { return _sgDimensionStyle->precision; }
				void set(unsigned short value) { _sgDimensionStyle->precision = value; }
			}
			
			msgTextStyleStruct^ text_style;
		internal:
			msgDimensionStyleStruct(SG_DIMENSION_STYLE* sgDimensionStyle)
			{
				_sgDimensionStyle = sgDimensionStyle;
				text_style = gcnew msgTextStyleStruct(&_sgDimensionStyle->text_style);
				_needDelete = false;
			}

			SG_DIMENSION_STYLE* _sgDimensionStyle;

		private:
			bool _needDelete;
		};
	}
}
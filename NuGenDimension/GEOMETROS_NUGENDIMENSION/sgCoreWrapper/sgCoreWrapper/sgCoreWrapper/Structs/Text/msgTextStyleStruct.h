#pragma once

#include "sgCore/sgTD.h"

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgTextStyleStruct
		{
		public:
			msgTextStyleStruct()
			{
				_sgTextStyle = new SG_TEXT_STYLE();
				_needDelete = true;
			}

			~msgTextStyleStruct()
			{
				this->!msgTextStyleStruct();
			}

			!msgTextStyleStruct()
			{
				if (_needDelete)
				{
					delete _sgTextStyle;
				}
			}

			property unsigned char state
			{
				unsigned char get() { return _sgTextStyle->state; }
				void set (unsigned char value) { _sgTextStyle->state = value; }
			}
			
			property double height
			{
				double get() { return _sgTextStyle->height; }
				void set (double value) { _sgTextStyle->height = value; }
			}
			
			property double proportions 
			{
				double get() { return _sgTextStyle->proportions; }
				void set (double value) { _sgTextStyle->proportions = value; }
			}

			property double angle
			{
				double get() { return _sgTextStyle->angle; }
				void set (double value) { _sgTextStyle->angle = value; }
			}
			
			property double horiz_space_proportion
			{
				double get() { return _sgTextStyle->horiz_space_proportion; }
				void set (double value) { _sgTextStyle->horiz_space_proportion = value; }
			}
			
			property double vert_space_proportion
			{
				double get() { return _sgTextStyle->vert_space_proportion; }
				void set (double value) { _sgTextStyle->vert_space_proportion = value; }
			}

		internal:
			msgTextStyleStruct(SG_TEXT_STYLE* sgTextStyle)
			{
				_sgTextStyle = sgTextStyle;
				_needDelete = false;
			}

			SG_TEXT_STYLE* _sgTextStyle;
		private:
			bool _needDelete;
		};
	}
}
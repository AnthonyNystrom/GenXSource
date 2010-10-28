#pragma once

namespace sgCoreWrapper
{
	namespace Structs
	{
		public ref struct msgDoubleStruct
		{
			property double value
			{
				double get() { return *_value; }
				void set(double value) { *_value = value; }
			}
		internal:
			msgDoubleStruct(double* value)
			{
				_value = value;
			}
		private:
			double* _value;
		};
	}
}
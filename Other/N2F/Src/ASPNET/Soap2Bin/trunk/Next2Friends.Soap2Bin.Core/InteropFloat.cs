using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Soap2Bin.Core
{
    /// <summary>
    /// Imitates a fixed point data type with 2 bytes for integer and 2 bytes for fraction.
    /// </summary>
    public struct InteropFloat
    {
        private Int32 _value;

        internal Int32 Value { get { return _value; } set { _value = value; } }

        /// <summary>
        /// </summary>
        public override Boolean Equals(Object value)
        {
            if (value == null)
                return false;
            if (value is InteropFloat)
                return _value.Equals(((InteropFloat)value)._value);

            return false;
        }

        /// <summary>
        /// </summary>
        public override Int32 GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// </summary>
        public override String ToString()
        {
            return ((Single)this).ToString();
        }

        /// <summary>
        /// </summary>
        public static implicit operator InteropFloat(Int32 value)
        {
            if (value < Int16.MinValue || value > Int16.MaxValue)
                throw new OverflowException();
            return new InteropFloat() { _value = value << 16 };
        }

        /// <summary>
        /// </summary>
        public static implicit operator Int32(InteropFloat value)
        {
            return value._value >> 16;
        }

        /// <summary>
        /// </summary>
        public static implicit operator InteropFloat(Single value)
        {
            return new InteropFloat() { _value = (Int32)(value * (Single)(1 << 16) + (value < 0 ? -0.5f : 0.5f)) };
        }

        /// <summary>
        /// </summary>
        public static implicit operator Single(InteropFloat value)
        {
            return (Single)value._value / (Single)(1 << 16);
        }
    }
}

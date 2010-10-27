using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Soap2Bin.Core
{
    /// <summary>
    /// Imitates an 8-bit signed integer.
    /// </summary>
    public struct InteropByte
    {
        private Int32 _value;

        /// <summary>
        /// Represents the largets possible value of a <code>InteropByte</code>. The field is constant.
        /// </summary>
        public const Int32 MaxValue = 127;

        /// <summary>
        /// Represents the smallest possible value of a <code>InteropByte</code>. The field is constant.
        /// </summary>
        public const Int32 MinValue = -128;

        /// <summary>
        /// </summary>
        public override Boolean Equals(Object value)
        {
            if (value == null)
                return false;
            if (value is InteropByte)
                return _value == ((InteropByte)value)._value;

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
            return _value.ToString();
        }

        /// <summary>
        /// </summary>
        public static implicit operator Int32(InteropByte value)
        {
            return value._value;
        }

        /// <summary>
        /// </summary>
        public static implicit operator InteropByte(Int32 value)
        {
            if (value > MaxValue || value < MinValue)
                throw new OverflowException();
            return new InteropByte() { _value = value };
        }
    }
}

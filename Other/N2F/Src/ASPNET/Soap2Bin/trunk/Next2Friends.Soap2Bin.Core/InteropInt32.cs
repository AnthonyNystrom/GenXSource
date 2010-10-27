using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Soap2Bin.Core
{
    /// <summary>
    /// Imitates a 16-bit signed integer.
    /// </summary>
    public sealed class InteropInt32
    {
        private Int32 _value;

        /// <summary>
        /// Represents the largest possible value of <code>InteropInt32</code>. The field is constant.
        /// </summary>
        public const Int32 MaxValue = Int32.MaxValue;

        /// <summary>
        /// Represents the smallest possible value of <code>InteropInt32</code>. The field is constant.
        /// </summary>
        public const Int32 MinValue = Int32.MinValue;

        /// <summary>
        /// </summary>
        public override Boolean Equals(Object value)
        {
            if (value == null)
                return false;
            if (value is InteropInt32)
                return _value == ((InteropInt32)value)._value;

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
        public static implicit operator Int32(InteropInt32 value)
        {
            return value._value;
        }

        /// <summary>
        /// </summary>
        public static implicit operator InteropInt32(Int32 value)
        {
            return new InteropInt32() { _value = value };
        }
    }
}

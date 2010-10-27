using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Soap2Bin.Core
{
    /// <summary>
    /// Represents a 16-bit signed integer. 
    /// </summary>
    public sealed class InteropInt16
    {
        private Int32 _value;

        /// <summary>
        /// Represents the largest possible value of <code>InteropInt16</code>. This field is constant.
        /// </summary>
        public const Int32 MaxValue = Int16.MaxValue;

        /// <summary>
        /// Represents the smallest possible value of <code>InteropInt16</code>. This field is constant.
        /// </summary>
        public const Int32 MinValue = Int16.MinValue;

        /// <summary>
        /// </summary>
        public override Boolean Equals(Object value)
        {
            if (value == null)
                return false;
            if (value is InteropInt16)
                return _value == ((InteropInt16)value)._value;

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
        public static implicit operator Int32(InteropInt16 value)
        {
            return value._value;
        }

        /// <summary>
        /// </summary>
        public static implicit operator InteropInt16(Int32 value)
        {
            if (value > MaxValue || value < MinValue)
                throw new OverflowException();
            return new InteropInt16() { _value = value };
        }
    }
}

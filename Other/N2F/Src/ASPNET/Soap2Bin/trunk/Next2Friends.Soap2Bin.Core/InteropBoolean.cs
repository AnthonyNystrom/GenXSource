using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Soap2Bin.Core
{
    /// <summary>
    /// Imitates a Boolean value.
    /// </summary>
    public struct InteropBoolean
    {
        private Int32 _value;

        /// <summary>
        /// </summary>
        public override Boolean Equals(Object value)
        {
            if (value == null)
                return false;
            if (value is InteropBoolean)
                return _value == ((InteropBoolean)value)._value;

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
        public override string ToString()
        {
            return _value == 1 ? Boolean.TrueString : Boolean.FalseString;
        }

        /// <summary>
        /// </summary>
        public static implicit operator Boolean(InteropBoolean value)
        {
            return value._value == 1;
        }

        /// <summary>
        /// </summary>
        public static implicit operator InteropBoolean(Boolean value)
        {
            return new InteropBoolean() { _value = value ? 1 : 0 };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Soap2Bin.Core
{
    /// <summary>
    /// Imitates a Unicode string.
    /// </summary>
    public sealed class InteropString
    {
        private String _value;

        /// <summary>
        /// Creates a new instance of the <code>InteropString</code> class.
        /// </summary>
        private InteropString()
        {
        }

        /// <summary>
        /// Gets the number of characters in the current <code>InteropString</code> object.
        /// </summary>
        public Int32 Length
        {
            get { return _value != null ? _value.Length : 0; }
        }

        /// <summary>
        /// </summary>
        public override Boolean Equals(Object value)
        {
            if (value == null)
                return false;
            if (value is InteropString)
                return _value.Equals(((InteropString)value)._value, StringComparison.Ordinal);

            return false;
        }

        /// <summary>
        /// </summary>
        public override Int32 GetHashCode()
        {
            if (_value != null)
                return _value.GetHashCode();
            return 0;
        }

        /// <summary>
        /// </summary>
        public override String ToString()
        {
            return _value;
        }

        /// <summary>
        /// </summary>
        public static implicit operator InteropString(String value)
        {
            return new InteropString() { _value = value };
        }

        /// <summary>
        /// </summary>
        public static implicit operator String(InteropString value)
        {
            return value._value;
        }
    }
}

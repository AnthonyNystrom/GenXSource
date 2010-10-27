/* ------------------------------------------------
 * BadCredentialsException.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Data
{
    [global::System.Serializable]
    public class BadCredentialsException : Exception
    {
        public BadCredentialsException() { }
        public BadCredentialsException(String message) : base(message) { }
        public BadCredentialsException(String message, Exception inner) : base(message, inner) { }
        protected BadCredentialsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

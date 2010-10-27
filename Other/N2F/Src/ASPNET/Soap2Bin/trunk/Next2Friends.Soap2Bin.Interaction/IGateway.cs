using System;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction
{
    interface IGateway
    {
        /// <summary>
        /// Calls the associated web service method and stores the result. Call <code>Return</code> method when return value is needed.
        /// </summary>
        void Invoke(HttpSessionState session, DataInputStream input);

        /// <summary>
        /// Writes the return value retrieved when <code>Invoke</code> method was called.
        /// </summary>
        void Return(DataOutputStream output);
    }
}

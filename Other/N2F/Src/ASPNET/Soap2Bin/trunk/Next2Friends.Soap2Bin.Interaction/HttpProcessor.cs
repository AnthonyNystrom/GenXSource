using System;
using System.IO;
using System.ServiceModel;
using System.Web.SessionState;
using System.Globalization;
using Next2Friends.Soap2Bin.Core;
using Next2Friends.Soap2Bin.Interaction.Properties;
using Next2Friends.Soap2Bin.Interaction.AskService;

namespace Next2Friends.Soap2Bin.Interaction
{
    static class HttpProcessor
    {
        /// <summary>
        /// This constant indicates the command code for an invocation in the standard protocol.
        /// </summary>
        public const Int16 INVOCATION_CODE = 1;

        /// <summary>
        /// This member indicates a successful result.
        /// </summary>
        public const Int16 RESULT_SUCCESSFUL = 1;

        /// <summary>
        /// This member indicates a server side exception.
        /// </summary>
        public const Int16 RESULT_EXCEPTION = 2;

        /// <summary>
        /// The version string for the protocol. This must match the client's version.
        /// </summary>
        public const String PROTOCOL_VERSION = "???";

        public static T GetClient<T>(HttpSessionState session) where T : class, new()
        {
            var clientName = typeof(T).Name;
            T client = default(T);

            if (session != null)
                client = session[clientName] as T;
            
            if (client == null)
            {
                client = new T();

                if (session != null)
                    session.Add(clientName, client);
            }

            return client;
        }
    }
}

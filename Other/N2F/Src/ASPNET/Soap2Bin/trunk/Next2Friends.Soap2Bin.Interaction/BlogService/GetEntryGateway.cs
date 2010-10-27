/* ------------------------------------------------
 * GetEntryGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.BlogService
{
    sealed class GetEntryGateway : IGateway
    {
        private BlogEntry _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<BlogServiceSoapClient>(session).GetEntry(
                input.ReadString(),
                input.ReadString(),
                input.ReadInt32());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteGetEntryResult(_result);
        }
    }
}

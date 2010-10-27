/* ------------------------------------------------
 * RemindPasswordGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.IO;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.MemberService
{
    sealed class RemindPasswordGateway : IGateway
    {
        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            HttpProcessor.GetClient<MemberServiceSoapClient>(session).RemindPassword(
                input.ReadString());
        }

        public void Return(DataOutputStream output)
        {
            /* Void. Do nothing. */
        }
    }
}

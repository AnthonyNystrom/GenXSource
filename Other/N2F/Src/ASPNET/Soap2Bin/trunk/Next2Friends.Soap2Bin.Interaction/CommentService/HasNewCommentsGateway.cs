/* ------------------------------------------------
 * HasNewCommentsGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.CommentService
{
    sealed class HasNewCommentsGateway : IGateway
    {
        private Boolean _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<CommentServiceSoapClient>(session).HasNewComments(
                input.ReadString(),
                input.ReadString(),
                input.ReadInt32(),
                input.ReadInt32(),
                input.ReadInt32());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteBoolean(_result);
        }
    }
}

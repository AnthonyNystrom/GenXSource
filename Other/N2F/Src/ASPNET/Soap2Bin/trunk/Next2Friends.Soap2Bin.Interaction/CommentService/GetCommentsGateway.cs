/* ------------------------------------------------
 * GetCommentsGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Soap2Bin.Interaction.CommentService
{
    sealed class GetCommentsGateway : IGateway
    {
        private WebComment[] _result;

        public void Invoke(System.Web.SessionState.HttpSessionState session, Next2Friends.Soap2Bin.Core.DataInputStream input)
        {
            _result = HttpProcessor.GetClient<CommentServiceSoapClient>(session).GetComments(
                input.ReadString(),
                input.ReadString(),
                input.ReadInt32(),
                input.ReadInt32(),
                input.ReadInt32());
        }

        public void Return(Next2Friends.Soap2Bin.Core.DataOutputStream output)
        {
            output.WriteGetCommentsResult(_result);
        }
    }
}

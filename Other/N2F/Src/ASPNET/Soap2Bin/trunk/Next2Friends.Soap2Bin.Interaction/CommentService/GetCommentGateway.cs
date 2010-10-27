/* ------------------------------------------------
 * GetCommentGateway.cs
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
    sealed class GetCommentGateway : IGateway
    {
        private WebComment _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<CommentServiceSoapClient>(session).GetComment(
                input.ReadString(),
                input.ReadString(),
                input.ReadInt32());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteGetCommentResult(_result);
        }
    }
}

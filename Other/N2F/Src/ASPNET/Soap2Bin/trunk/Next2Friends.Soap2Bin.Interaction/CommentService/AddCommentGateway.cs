/* ------------------------------------------------
 * AddCommentGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.CommentService
{
    sealed class AddCommentGateway : IGateway
    {
        private Int32 _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<CommentServiceSoapClient>(session).AddComment(
                input.ReadString(),
                input.ReadString(),
                input.ReadWebComment());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteInt32(_result);
        }
    }
}

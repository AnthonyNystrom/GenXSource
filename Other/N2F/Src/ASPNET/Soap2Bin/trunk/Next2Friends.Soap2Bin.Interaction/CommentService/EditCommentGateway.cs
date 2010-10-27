/* ------------------------------------------------
 * EditCommentGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.CommentService
{
    sealed class EditCommentGateway : IGateway
    {
        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            HttpProcessor.GetClient<CommentServiceSoapClient>(session).EditComment(
                input.ReadString(),
                input.ReadString(),
                input.ReadWebComment());
        }

        public void Return(DataOutputStream output)
        {
            /* Void. Do nothing. */
        }
    }
}

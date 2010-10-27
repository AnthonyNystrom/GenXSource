using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.AskService
{
    sealed class VoteForQuestionGateway : IGateway
    {
        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            HttpProcessor.GetClient<AskServiceSoapClient>(session).VoteForQuestion(
               input.ReadString(),
               input.ReadString(),
               input.ReadInt32(),
               input.ReadInt32());
        }

        public void Return(DataOutputStream output)
        {
            /* Void. Do nothing. */
        }
    }
}

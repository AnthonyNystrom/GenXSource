using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.AskService
{
    sealed class CompleteQuestionGateway : IGateway
    {
        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            HttpProcessor.GetClient<AskServiceSoapClient>(session).CompleteQuestion(
               input.ReadString(),
               input.ReadString(),
               input.ReadString());
        }

        public void Return(DataOutputStream output)
        {
            /* Void. Do nothing. */
        }
    }
}

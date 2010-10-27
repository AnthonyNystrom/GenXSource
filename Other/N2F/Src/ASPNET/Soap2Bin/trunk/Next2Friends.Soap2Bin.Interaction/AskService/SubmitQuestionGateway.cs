using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.AskService
{
    sealed class SubmitQuestionGateway : IGateway
    {
        private AskQuestionConfirm _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<AskServiceSoapClient>(session).SubmitQuestion(
                input.ReadString(),
                input.ReadString(),
                input.ReadString(),
                input.ReadInt32(),
                input.ReadInt32(),
                input.ReadStringArray(),
                input.ReadInt32(),
                input.ReadBoolean());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteSubmitQuestionResult(_result);
        }
    }
}

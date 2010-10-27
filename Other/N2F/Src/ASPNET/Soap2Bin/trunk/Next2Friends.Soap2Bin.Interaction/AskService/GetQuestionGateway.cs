using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.AskService
{
    sealed class GetQuestionGateway : IGateway
    {
        private AskQuestion _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<AskServiceSoapClient>(session).GetQuestion(
                input.ReadString(),
                input.ReadString(),
                input.ReadInt32());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteGetQuestionResult(_result);
        }
    }
}

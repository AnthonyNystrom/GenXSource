using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.AskService
{
    sealed class GetQuestionIDsGateway : IGateway
    {
        private Int32[] _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<AskServiceSoapClient>(session).GetQuestionIDs(
                input.ReadString(),
                input.ReadString());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteInt32Array(_result);
        }
    }
}

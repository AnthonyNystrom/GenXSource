using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.AskService
{
    sealed class GetResponseGateway : IGateway
    {
        private AskResponse _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<AskServiceSoapClient>(session).GetResponse(
               input.ReadString(),
               input.ReadString(),
               input.ReadInt32());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteGetResponseResult(_result);
        }
    }
}

using System;
using System.IO;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.MemberService
{
    sealed class CheckUserExistsGateway : IGateway
    {
        private Boolean _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<MemberServiceSoapClient>(session).CheckUserExists(
                input.ReadString(),
                input.ReadString());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteBoolean(_result);
        }
    }
}

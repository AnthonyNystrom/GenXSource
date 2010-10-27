using System;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.DashboardService
{
    sealed class GetWallCommentsGateway : IGateway
    {
        private DashboardWallComment[] _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<DashboardServiceSoapClient>(session).GetWallComments(input.ReadString(), input.ReadString());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteGetWallCommentsResult(_result);
        }
    }
}

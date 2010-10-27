using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.SnapUpService
{
    sealed class DeviceUploadPhotoGateway : IGateway
    {
        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            HttpProcessor.GetClient<SnapUpServiceSoapClient>(session).DeviceUploadPhoto(
                input.ReadString(),
                input.ReadString(),
                Convert.ToBase64String(input.ReadByteArray()),
                input.ReadDateTime().Ticks.ToString());
        }

        public void Return(DataOutputStream output)
        {
            /* Void. Do nothing. */
        }
    }
}

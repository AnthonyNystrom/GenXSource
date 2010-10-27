using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.AskService
{
    sealed class AttachPhotoGateway : IGateway
    {
        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            HttpProcessor.GetClient<AskServiceSoapClient>(session).AttachPhoto(
               input.ReadString(),
               input.ReadString(),
               input.ReadString(),
               input.ReadInt32(),
               Convert.ToBase64String(input.ReadByteArray()));
        }

        public void Return(DataOutputStream output)
        {
            /* Void. Do nothing. */
        }
    }
}

/* ------------------------------------------------
 * GetThumbnailGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Next2Friends.Soap2Bin.Core;
using System.Web.SessionState;

namespace Next2Friends.Soap2Bin.Interaction.DashboardService
{
    sealed class GetThumbnailGateway : IGateway
    {
        private String _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<DashboardServiceSoapClient>(session).GetThumbnail(
                input.ReadString(),
                input.ReadString(),
                input.ReadString());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteByteArray(Convert.FromBase64String(_result));
        }
    }
}

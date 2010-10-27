/* ------------------------------------------------
 * GetPhotosGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Next2Friends.Soap2Bin.Core;

namespace Next2Friends.Soap2Bin.Interaction.DashboardService
{
    sealed class GetPhotosGateway : IGateway
    {
        private DashboardPhoto[] _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<DashboardServiceSoapClient>(session).GetPhotos(input.ReadString(), input.ReadString());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteGetPhotosResult(_result);
        }
    }
}

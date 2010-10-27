/* ------------------------------------------------
 * GetPhotosForCollectionGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Next2Friends.Soap2Bin.Core;
using System.Web.SessionState;

namespace Next2Friends.Soap2Bin.Interaction.PhotoService
{
    sealed class GetPhotosForCollectionGateway : IGateway
    {
        private WebPhoto[] _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<PhotoServiceSoapClient>(session).GetPhotosForCollection(
                input.ReadString(),
                input.ReadString(),
                input.ReadInt32());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteGetPhotosForCollectionResult(_result);
        }
    }
}

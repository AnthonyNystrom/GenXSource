using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Next2Friends.Soap2Bin.Core;
using System.Web.SessionState;

namespace Next2Friends.Soap2Bin.Interaction.TagService
{
    sealed class UploadTagsGateway : IGateway
    {
        private TagConfirmation[] _result;

        public void Invoke(HttpSessionState session, DataInputStream input)
        {
            _result = HttpProcessor.GetClient<TagServiceSoapClient>(session).UploadTags(
                input.ReadString(),
                input.ReadString(),
                input.ReadTagUpdate());
        }

        public void Return(DataOutputStream output)
        {
            output.WriteUploadTagsResult(_result);
        }
    }
}

using System;

namespace Next2Friends.WebServices.Tag
{
    public sealed class TagConfirmation
    {
        public TagConfirmation()
        {
        }
        
        public TagConfirmation(string deviceID, bool confirmedByServer)
        {
            DeviceID = deviceID;
            ConfirmedByServer = confirmedByServer;
        }

        public String DeviceID { get; set; }
        public Boolean ConfirmedByServer { get; set; }
    }
}

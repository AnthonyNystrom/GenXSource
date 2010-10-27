using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace Next2Friends.WebServices
{
    [Serializable]
    public class BTTagUpdate
    {
        public string[] TagValidationString { get; set; }
        public string[] DeviceTagID { get; set; }

        public BTTagUpdate()
        {

        }

        public BTTagUpdate(int Length)
        {
            TagValidationString = new string[Length];
            DeviceTagID = new string[Length];
        }

        public int CheckArraylength()
        {
            if (TagValidationString.Length != DeviceTagID.Length)
                return 0;

            //if (DeviceTagID.Length > 100)
             //   return 0;

            return DeviceTagID.Length;
        }
    }

    [Serializable]
    public class BTTagConfirmation
    {
        public string WebMemberID { get; set; }
        public bool ConfirmedByServer { get; set; }

        public BTTagConfirmation()
        {

        }
        
        public BTTagConfirmation(string MemberID, bool ConfirmedByServer)
        {
            this.WebMemberID = MemberID;
            this.ConfirmedByServer = ConfirmedByServer;
        }
    }
}

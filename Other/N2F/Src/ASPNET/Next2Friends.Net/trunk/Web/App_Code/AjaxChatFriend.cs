using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for AjaxChatFriend
/// </summary>
public class AjaxChatFriend
{
    public string NickName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string WebMemberID { get; set; }

    private OnlineStatus _onlineStatus = OnlineStatus.Offline;
    public OnlineStatus OnlineStatus {
        get
        {
            if( DateTime.Now.Subtract(this.LastCommDt).TotalSeconds > 60)
                return OnlineStatus.Offline;

            return _onlineStatus;
        }
        set
        {
            _onlineStatus = value;
        }
    }

    public string CustomMessage { get; set; }
    public string AvatorUrl { get; set; }
    public string OnlineStatusString { get; set; }
    public DateTime LastCommDt { get; set; }

}

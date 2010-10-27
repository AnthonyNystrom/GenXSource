using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Next2Friends.Data;

public partial class ShowFriendTags : System.Web.UI.Page
{
    public string tagstring = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        List<FriendTag> tags = FriendTag.GetAllFriendTag();

        for (int i = 0; i < tags.Count; i++)
        {
            tagstring += "tagValidationString: <strong>" + tags[i].TagValidationString + "</strong>           memberID1: <strong>" + tags[i].FirstMemberID;
            tagstring += "</strong>          memberID2: <strong>" + tags[i].SecondMemberID + "</strong>            memberID1: <strong>" + tags[i].CreatedDT.ToString() + "</strong><br />.";
        }
    }
}

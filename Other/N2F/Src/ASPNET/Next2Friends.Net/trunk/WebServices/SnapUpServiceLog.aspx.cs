using System;
using System.Collections;
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
using Next2Friends.WebServices.Utils;

namespace Next2Friends.WebServices
{
    public partial class SnapUpServiceLog : Page
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            _logLabel.Text = LogProcessor.ToHtml(SnapUpService.Identifier);
        }
    }
}

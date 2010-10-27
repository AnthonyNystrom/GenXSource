using System;
using Next2Friends.WebServices.Utils;

namespace Next2Friends.WebServices
{
    public partial class FixServiceLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _logLabel.Text = LogProcessor.ToHtml(FixService.Identifier);
        }
    }
}

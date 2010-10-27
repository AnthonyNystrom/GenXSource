using System;
using System.Web.UI;
using Next2Friends.WebServices.Utils;

namespace Next2Friends.WebServices
{
    public partial class TagServiceLog : Page
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            _logLabel.Text = LogProcessor.ToHtml(TagService.Identifier);
        }
    }
}

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

public partial class Trace : System.Web.UI.Page
{
    public string traceSTR;

    protected void Page_Load(object sender, EventArgs e)
    {
        List<Next2Friends.Data.Trace> trace = Next2Friends.Data.Trace.GetAllTrace();


        for (int i = trace.Count - 1; i > trace.Count - 20; i--)
        {
            traceSTR += trace[i].DTCreated.AddHours(6).ToLongDateString() + ":" + trace[i].DTCreated.AddHours(6).ToShortTimeString() + "<br><br>" + trace[i].Source + "<br><br>" + trace[i].Text.Replace("\r\n", "<br>") + "<br><br><hr width='100%'>"; 
        }
    }
}

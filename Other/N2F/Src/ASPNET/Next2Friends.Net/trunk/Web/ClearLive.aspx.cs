using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

public partial class ClearLive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //try
       // {
            Database db = DatabaseFactory.CreateDatabase();

            db.ExecuteNonQuery(CommandType.Text, "UPDATE LiveBroadcast SET LiveNow = 0");



        //}
        //catch { }
    }
}

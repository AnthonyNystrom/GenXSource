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

public partial class IMSPlanEdit : System.Web.UI.Page
{
    public string JSBusinessArray = "";
    public List<Business> Businesses;
    public bool LoggedIn = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadBusinesses();
    }

    /// <summary>
    /// load the Business and plans
    /// </summary>
    public void LoadBusinesses()
    {
        if (Session["LoggedIn"] != null)
        {
            LoggedIn = (bool)Session["LoggedIn"];
        }

        if (LoggedIn)
        {
            Businesses = Business.GetAllBusinessWithJoin();

            // create the js array
            JSBusinessArray = "var Companies = new Array(";
			for (int i = 0; i < Businesses.Count; i++)
			{
                string Comma = (i == Businesses.Count - 1) ? string.Empty : ",";

                JSBusinessArray += "new Array('" + Businesses[i].CompanyName.Replace("'", "&#38;") + "','" + Businesses[i].BusinessID + "')" + Comma + "";
			}

            JSBusinessArray += ");";
        }
    }


    /// <summary>
    /// user logs out
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        LoggedIn = false;
        Session["LoggedIn"] = null;

    }

    /// <summary>
    ///  user logs in
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (txtuserName.Text == "rachel" && txtPassword.Text == "password")
        {
            LoggedIn = true;
            Session["LoggedIn"] = true;
            LoadBusinesses();
        }
        else
        {
            lblLogin.Text = "<p style='color:#FF0000'>Incorrect login, please try again</p>";
        }
    }
}

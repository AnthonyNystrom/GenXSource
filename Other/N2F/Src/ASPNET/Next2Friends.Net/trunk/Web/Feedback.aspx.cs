using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Next2Friends.Data;

public partial class FeedbackPage : System.Web.UI.Page
{
    public bool FeedbackCompleted = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {

            Member member = (Member)Session["Member"];


            Feedback feedback = new Feedback();
            feedback.MemberID = (member!=null) ? member.MemberID : 0;
            feedback.Name = txtName.Text;
            feedback.EmailAddress = txtEmail.Text;
            feedback.DTCreated = DateTime.Now;
            feedback.Text = txtFeedback.Text;      
            FeedbackCompleted = true;


        }
    }
}

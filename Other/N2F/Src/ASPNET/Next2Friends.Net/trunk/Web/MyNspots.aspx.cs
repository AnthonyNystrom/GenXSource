using System;
using System.Text;
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

public partial class MyNspots : System.Web.UI.Page
{
    public string DefaultNSpotLister = string.Empty;
    public bool IsLoggedIn = false;
    public bool ShowWizard = true;
    public Member member;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            drpYear.SelectedValue = DateTime.Now.Year.ToString();
            drpMonth.SelectedValue = DateTime.Now.Month.ToString();
            drpDay.SelectedValue = DateTime.Now.Day.ToString();
            drpHour.SelectedValue = DateTime.Now.Hour.ToString();
            drpMinute.SelectedValue = DateTime.Now.Minute.ToString();
        }

        GenerateNspotLister();

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        bool go = true;
        member = new Member(3);
       

        DateTime StartDT = DateTime.Now;
        DateTime EndDT = DateTime.Now;

        if (txtNspotName.Text.Length < 5)
        {
            litNspotName.Text = "<span class='formerror_msg'>Your NSpot name must be at least 5 characters long</span>";
            txtNspotName.CssClass = "form_txt formerror";
            go = false;
        }
        else
        {
            litNspotName.Text = string.Empty;
            txtNspotName.CssClass = "form_txt";
        }

        try
        {
            int Day = Int32.Parse(drpDay.SelectedValue);
            int Month = Int32.Parse(drpMonth.SelectedValue);
            int Year = Int32.Parse(drpYear.SelectedValue);
            int Hour = Int32.Parse(drpHour.SelectedValue);
            int Minute = Int32.Parse(drpMinute.SelectedValue);

            StartDT = new DateTime(Year, Month, Day,Hour,Minute, 0);

            if (StartDT < DateTime.Now)
            {
                litBeginTime.Text = "<span class='formerror_msg'>Start Date and Time must be in the future</span>";
                go = false;
            }
            else
            {
                litBeginTime.Text = string.Empty;
            }

        }
        catch
        {
            litBeginTime.Text = "<span class='formerror_msg'>Not a valid Date and Time</span>";
        }

        try
        {
            int Hour = Int32.Parse(drpHourLast.SelectedValue);

            EndDT = StartDT.AddHours(Hour);

            litDuration.Text = string.Empty;
        }
        catch
        {
            litDuration.Text = "<span class='formerror_msg'>Not a valid Duration</span>";
            go = false;
        }

        if (txtDescription.Text.Length < 5)
        {
            litDescription.Text = "<span class='formerror_msg'>Your NSpot description must be at least 5 characters long</span>";
            txtDescription.CssClass = "form_txt formerror";
            go = false;
        }
        else
        {
            litDescription.Text = string.Empty;
            txtDescription.CssClass = "form_txt";
        }

        if (FileUpload.HasFile)
        {
            if (!Photo.IsPhotoFile(FileUpload.FileBytes))
            {
                litBrowsePhoto.Text = "<span class='formerror_msg'>please upload a Jpg or Png photo</span>";
                FileUpload.CssClass = "form_txt formerror";
                go = false;
            }
            else
            {
                litBrowsePhoto.Text = string.Empty;
                FileUpload.CssClass = "form_txt";
            }
        }
        else
        {
            litBrowsePhoto.Text = "<span class='formerror_msg'>please select your NSpot Photo</span>";
            FileUpload.CssClass = "form_txt formerror";
            go = false;
        }

        if (go)
        {
            NSpot nspot = new NSpot();
            nspot.WebNSpotID = Next2Friends.Misc.UniqueID.NewWebID();
            nspot.MemberID = member.MemberID;
            nspot.Name = txtNspotName.Text;
            nspot.Description = txtDescription.Text;
            nspot.StartDateTime = StartDT;
            nspot.EndDateTime = EndDT;
            nspot.DTCreated = DateTime.Now;
            nspot.AskBeforeJoining = ChbMakePrivate.Checked;
            nspot = Photo.ProcessNSpotPhoto(member, nspot, FileUpload.FileBytes);
            nspot.Save();

            ShowWizard = false;
        }
    }

    /// <summary>
    /// Creates a lister with members friends
    /// </summary>
    public void GenerateNspotLister()
    {
        member = new Member(3);

        List<NSpot> nspots = NSpot.GetAllNSpotByMemberID(member);

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            if (nspots.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[15];

            parameters[0] = nspots[i].WebNSpotID;
            parameters[1] = nspots[i].PhotoResourceFile.FullyQualifiedURL;
            parameters[2] = nspots[i].Name;
            parameters[3] = nspots[i].Description;
            parameters[4] = nspots[i].StartDateTime.ToString();
            parameters[5] = nspots[i].EndDateTime.ToString("hh:mm dd MMM yyyy");
            parameters[6] = nspots[i].EndDateTime.ToString("hh:mm dd MMM yyyy");
            parameters[7] = nspots[i].NumberOfMembers.ToString();
            parameters[8] = nspots[i].NumberOfViews.ToString();
            parameters[9] = nspots[i].NumberOfPhotos.ToString();
            parameters[10] = nspots[i].NumberOfComments.ToString();

            string HTMLItem = @" <div class='nspot_list clearfix'>

                <div class='profile_pic'>
					<a href='nspot.aspx?n={0}'><img src='{1}' alt='pic' /></a>
				</div>
				<div class='nspot_data'>
                    <p class='nspot_name'><a href='nspot.aspx?n={0}'>{2}</a></p>
					<div class='nscol1'>
                    
					<strong>Started:</strong> {5}<br />
					<strong>Ended:</strong> {6}<br />
                    </div>
					<div class='nscol2'>
						<strong>Number of members:</strong> {7}<br />
                        <strong>Number of views:</strong> {8}<br />
                        <strong>Number of photos:</strong> {9}<br />
                        <strong>Number of comments:</strong> {10}<br />

					</div>			
				</div>

				<ul class='nspot_actions'>
			
				</ul></div>";

                    //<li><a href='inbox.aspx?s=Mzk5OWEwN2Y5ZDg5NDg3Mz' class='send_message'>Send Message</a></li>
                    //<li><a href='#' class='send_instant'>Send Instant Message</a></li>
                    //<li><a href='inbox.aspx?f=Mzk5OWEwN2Y5ZDg5NDg3Mz' class='forward'>Forward to a nspot</a></li>
                    //<li><a href='javascript:blocknspot('Mzk5OWEwN2Y5ZDg5NDg3Mz');' class='block'>Block this user</a></li>

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        DefaultNSpotLister = sbHTMLList.ToString();
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        Master.SkinID = "Community";
        base.OnPreInit(e);
    }

}

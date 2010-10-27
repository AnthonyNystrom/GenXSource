using System;
using System.Drawing;
using System.Drawing.Imaging;
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
using Next2Friends.Data;

public partial class AAFUpload : System.Web.UI.Page
{
    public Member member;
    public string divCustomShowHide = "style='display:none;'";
    public string divMultiShowHide = "style='display:none;'";
    public string spanPhotoNoValue = string.Empty;
    public bool IsLoggedIn = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (member != null)
        {
            IsLoggedIn = true;
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Submits the AAF question
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        member = (Member)Session["Member"];
        bool Go = true;

        System.Drawing.Image image1 = null;
        System.Drawing.Image image2 = null;
        System.Drawing.Image image3 = null;
        int NumberOfPhotos = 0;
        AskResponseType responseType = AskResponseType.None;

        #region Show Hide options
        if (rbCustom.Checked)
        {
            divCustomShowHide = "style='display:block;'";
            divMultiShowHide = "style='display:none;'";
            responseType = AskResponseType.AB;
        }
        else if (rbImageSelect.Checked)
        {
            divCustomShowHide = "style='display:none;'";
            divMultiShowHide = "style='display:block;'";
            responseType = AskResponseType.MultipleSelect;
        }
        else if (rbRate110.Checked)
        {
            divCustomShowHide = "style='display:none;'";
            divMultiShowHide = "style='display:none;'";
            responseType = AskResponseType.RateTo10;
        }
        else if (rbYesNo.Checked)
        {
            divCustomShowHide = "style='display:none;'";
            divMultiShowHide = "style='display:none;'";
            responseType = AskResponseType.YesNo;
        }

        #endregion

        #region Form Validation
        if (txtQuestion.Text == string.Empty)
        {
            litErrQuestion.Text = "<span class='formerror_msg'>Please enter your question</span>";
            txtQuestion.CssClass = "form_txt formerror";
            Go = false;
        }
        else
        {
            litErrQuestion.Text = string.Empty;
            txtQuestion.CssClass = "form_txt";
        }

        if (!rbCustom.Checked && !rbImageSelect.Checked && !rbRate110.Checked && !rbYesNo.Checked)
        {
            litErrResponse.Text = "<span class='formerror_msg'>Please select a response</span>";
            Go = false;
        }
        else
        {
            litErrResponse.Text = string.Empty;
        }

        if (rbCustom.Checked && txtCustomA.Text == string.Empty)
        {
            txtCustomA.CssClass = "form_txt formerror";
            libCustomA.Text = "<span class='formerror_msg'>Please enter custom response A</span>";
            Go = false;
        }
        else
        {
            txtCustomA.CssClass = "form_txt";
            libCustomA.Text = string.Empty;
        }

        if (rbCustom.Checked && txtCustomB.Text == string.Empty)
        {
            txtCustomB.CssClass = "form_txt formerror";
            libCustomB.Text = "<span class='formerror_msg'>Please enter custom response B</span>";
            Go = false;
        }
        else
        {
            txtCustomB.CssClass = "form_txt";
            libCustomB.Text = string.Empty;
        }

        bool ValidFile1 = true;

        try
        {
            image1 = new Bitmap(FileUpload1.FileContent);
        }
        catch
        {
            ValidFile1 = false;
        }

        if (!FileUpload1.HasFile)
        {
            FileUpload1.CssClass = "form_txt formerror";
            litFileUpload1.Text = "<span class='formerror_msg'>Please select your photo to upload</span>";
            Go = false;
        }
        else
        {
            FileUpload1.CssClass = "form_txt";
            litFileUpload1.Text = string.Empty;
        }

        bool ValidFile2 = true;

        try
        {
            image2 = new Bitmap(FileUpload2.FileContent);
        }
        catch
        {
            ValidFile2 = false;
        }

        if (rbImageSelect.Checked && !ValidFile2)
        {
            FileUpload2.CssClass = "form_txt formerror";
            litFileUpload2.Text = "<span class='formerror_msg'>Multi select requres at least 2 photos be uplaoded</span>";
            Go = false;
        }
        else
        {
            FileUpload2.CssClass = "form_txt";
            litFileUpload2.Text = string.Empty;
        }

        bool ValidFile3 = true;

        try
        {
            image3 = new Bitmap(FileUpload3.FileContent);
        }
        catch
        {
            ValidFile3 = false;
        }

        if (rbImageSelect.Checked)
        {
            spanPhotoNoValue = "1";
        }
        #endregion

        if (ValidFile1) NumberOfPhotos++;
        if (ValidFile2) NumberOfPhotos++;
        if (ValidFile3) NumberOfPhotos++;

        AskAFriend AAF = new AskAFriend();

        AAF.WebAskAFriendID = Next2Friends.Misc.UniqueID.NewWebID();
        AAF.MemberID = member.MemberID;
        AAF.RejectScore = 10;
        AAF.Question = txtQuestion.Text;
        AAF.NumberOfPhotos = NumberOfPhotos;
        AAF.ResponseType = (int)responseType;
        AAF.Active = false;

        if (responseType==AskResponseType.AB)
        {
            AAF.ResponseA = txtCustomA.Text;
            AAF.ResponseB = txtCustomB.Text;
        }

        AAF.Duration = 0;
        AAF.IsPrivate = chbPrivate.Checked;
        AAF.SubmittedIP = HttpContext.Current.Request.UserHostAddress;

        if (Go)
        {
            AAF.Save();

            int IndexOrder = 1;

            if (ValidFile1)
            {
                Photo.ProcessAAFPhoto(member, AAF, image1, IndexOrder++);
            }

            if (ValidFile2)
            {
                Photo.ProcessAAFPhoto(member, AAF, image2, IndexOrder++);
            }

            if (ValidFile3)
            {
                Photo.ProcessAAFPhoto(member, AAF, image3, IndexOrder++);
            }

            AAF.WentLiveDT = DateTime.Now;
            AAF.SubmittedDT = DateTime.Now;
            AAF.Active = true;

            AAF.Save();

            // show the success message
            panelUpload.Visible = false;
            litSuccessful.Visible = true;
        }
        
    }
}

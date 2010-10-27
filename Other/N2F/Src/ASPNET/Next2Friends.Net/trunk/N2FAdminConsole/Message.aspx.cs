using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Next2Friends.Data;

namespace N2FAdminConsole
{
    public partial class Message : System.Web.UI.Page
    {
        Member member = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            member = (Member)Session["Member"];
        }

        protected void btnTestSend_Click(object sender, EventArgs e)
        {
            if (member == null)
                return;

            Member targetMember = Member.GetMemberViaNickname(txtNick.Text);

            if (targetMember == null)
                return;

            Next2Friends.Data.Message message = new Next2Friends.Data.Message();

            message.FromNickName = "Next2Friends Team";
            message.WebMessageID = Next2Friends.Misc.UniqueID.NewWebID();
            message.IsDeleted = false;
            message.IsFetched = false;
            message.IsTrash = false;
            message.MemberIDTo = targetMember.MemberID;
            message.MemberIDFrom = member.MemberID;
            message.Body = SafeHTML(txtMessage.Text);
            message.DTCreated = DateTime.Now;

            message.Save();

            message.InReplyToID = message.MessageID;

            message.Save();

        }


        private List<Member> GetAllMembers()
        {
           return Next2Friends.Data.Member.GetAllMember();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (member == null)
                return;


            Next2Friends.Data.Message message = new Next2Friends.Data.Message();

            message.FromNickName = "Next2Friends Team";
            message.IsDeleted = false;
            message.IsFetched = false;
            message.IsTrash = false;
            message.MemberIDTo = 0;
            message.MemberIDFrom = member.MemberID;
            message.Body = SafeHTML(txtMessage.Text);
            message.DTCreated = DateTime.Now;

            List<Member> allMembers = GetAllMembers();

            foreach (Member currMember in allMembers)
            {
                message.MessageID = 0;
                message.WebMessageID = Next2Friends.Misc.UniqueID.NewWebID();
                message.MemberIDTo = currMember.MemberID;
                message.Save();

                message.InReplyToID = message.MessageID;

                message.Save();
            }   
        }

        private string SafeHTML(string input)
        {
            input = input.Replace("\r\n", "<br />");

            return input;
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = CheckBox1.Enabled;
        }
    }
}

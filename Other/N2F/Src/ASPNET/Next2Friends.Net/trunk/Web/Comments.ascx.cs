using System;
using System.Text;
using System.Data;
using System.Configuration;
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
using Next2Friends.Misc;


public partial class Comments : System.Web.UI.UserControl
{
    public string PageComments = string.Empty;
    public int NumberOfComments = 0;
    public bool IsLoggedIn = false;
    public string LoginUrl;
    public string DefaultNewCommentParams;
    public string defaultCommentFunction;    
    public CommentType CommentType { set; get; }    
    public string ObjectWebId { get; set; }
    public int ObjectId { get; set; }

    public bool Collapsed { get; set; }
    
    /// <summary>
    /// Used when its required to redirect the page to some other Url other then
    /// the one displayed in the browser address bar.
    /// As an example see usage in AskAFriend
    /// </summary>
    public string RedirectUrl { get; set; }

    private Member member = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Comments));
        LoginUrl = @"/signup.aspx?u=" + Server.UrlEncode(Request.Url.AbsoluteUri);

        if (RedirectUrl != null)
            LoginUrl = @"/signup.aspx?u=" + Server.UrlEncode(RedirectUrl);

        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        PageComments = GeneratePageComments();
       
        defaultCommentFunction = "ajaxPostComment(" + DefaultNewCommentParams + ");";

    }


    /// <summary>
    /// Returns HTML for commentss
    /// </summary>
    /// <param name="pObjectWebID"></param>
    /// <param name="pCommentType"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod]
    public AjaxComment GetComments(string WebID, string type)
    {   
        this.CommentType = (CommentType)Enum.Parse(typeof(CommentType),type);
        this.ObjectWebId = WebID;

        this.ObjectId = GetObjectID(this.CommentType, this.ObjectWebId);

        if (this.CommentType == CommentType.AskAFriend)
        {
            this.RedirectUrl = ASP.global_asax.WebServerRoot + "AskAFriend.aspx?q=" + WebID;
        }

        AjaxComment comm = new AjaxComment();
        comm.CommentHTML = GeneratePageComments();
        comm.TotalNumberOfComments = this.NumberOfComments.ToString();

        return comm;
    }

    private string GeneratePageComments()
    {
        member = (Member)HttpContext.Current.Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        AjaxComment[] comments = new AjaxComment[0];

        comments = AjaxComment.GetCommentByObjectIDWithJoin(ObjectId, (int)CommentType);
        DefaultNewCommentParams = "'" + CommentType.ToString() + "','" + ObjectWebId + "'";


        NumberOfComments = comments.Length;

        for (int i = 0; i < comments.Length; i++)
        {
            PageComments += comments[i].HTML;
        }

        if (NumberOfComments <= 0)
        {
            return "<p id='pBeFirst'>Be the first to post a comment</p>";            
        }
        
        StringBuilder sbCommentHTML = new StringBuilder();
        StringBuilder sbPartialHTML = null;

        object[] parameters = new object[12];

        string HTMLItem = (@"<li class='clearfix' id='li{5}' depth='{7}'><a href='javascript:dmp(""{8}"");'><img src='{0}' alt='' width='50' height='50' class='commenter_avatar' /></a>
							<div class='comment_entry' id='{5}'>
								<p class='reply'>");


        string HTMLItem2 = (@"</p>
								<p class='commenter'> <cite><a href='/users/{1}'>{1}</a></cite><br />
								<small>{2}</small></p>
								<p id='commentBody{5}'>{4}.{11}</p>
                                <p id='commentBodyHidden{5}' style='visibility:hidden;display:none'>{9}.</p>
							</div>
						</li>");

        int lastReplytoID = -1;        

        for(int i=0; i < comments.Length; i++ )        
        {
            AjaxComment comment = comments[i];

            int CurrentCommentDepth = comment.Depth;
            int PrevCommentDepth = 0;
            int NextCommentDepth = 0;

            try
            {
                PrevCommentDepth = comments[i - 1].Depth;
            }
            catch { }

            try
            {
                NextCommentDepth = comments[i + 1].Depth;
            }
            catch { }

            sbPartialHTML = new StringBuilder();
            sbPartialHTML.Append(HTMLItem);

            if (member != null && comment.WebMemberID == member.WebMemberID && !comment.IsDeleted)
            {
                sbPartialHTML.Append(@"<small><a id=""edit{5}"" href=""javascript:editComment('{6}','{5}','{10}');void(0);"">Edit</a>");
                sbPartialHTML.Append(@"<a id=""delete{5}"" href=""javascript:deleteComment('{6}','{5}','{10}');"">Delete</a></small>");
            }

            if (!comment.IsDeleted)
            {
                if (IsLoggedIn)
                {
                    sbPartialHTML.Append(@"<a id=""reply{5}"" href=""javascript:replyToComment('{6}','{5}','{10}');void(0);"">Reply</a>");
                }
                else
                {
                    sbPartialHTML.Append(@"<a href=""" + LoginUrl + @""">Reply</a>");
                }
            }

            sbPartialHTML.Append(HTMLItem2);

            parameters[0] = comment.PhotoUrl;
            parameters[1] = comment.NickName;
            parameters[2] = TimeDistance.TimeAgo(comment.DTCreated);
            parameters[11] = "";

            if (!comment.IsDeleted)
            {
                parameters[4] = HTMLUtility.AutoLink(comment.Text);
                parameters[9] = HTMLUtility.FormatForText(comment.Text);

                if (comment.SentFromMobile)
                {
                    parameters[11] = "<p><i>Sent from my mobile</i></p>";
                }
            }
            else
            {
                parameters[4] = @"<i style=""font-weight:lighter;color:gray"">Comment Deleted</i>";
            }

            parameters[5] = comment.WebCommentID;
            parameters[6] = (int)CommentType;
            parameters[7] = comment.Depth;
            parameters[8] = comment.WebMemberID;
            parameters[10] = ObjectWebId;


            if (PrevCommentDepth <=1 && comment.Depth > 1)
            {
                sbCommentHTML.Append("<ol class=\"jqCmtOl\" id=\"jqCmtOl\">");
            }

            sbCommentHTML.AppendFormat(sbPartialHTML.ToString(), parameters);

            if (NextCommentDepth <= 1 && comment.Depth > 1)
            {
                sbCommentHTML.Append("</ol>");
            }

            lastReplytoID = comment.InReplyToCommentID;
        }

        //string FinalComments = "<ul id='ulCommentList' class='profile_commentlist'>" + sbCommentHTML.ToString() + "</ul>";

        return sbCommentHTML.ToString();
    }

    private void UpdateCommentCount(Object CommentObject, CommentType type)
    {
        if (CommentObject == null)
            return;

        if (type == CommentType.Wall)
        {
            //Member m = (Member)CommentObject;            
            //CommentObject = m;            
        }
        else if (type == CommentType.Video)
        {
            Video v = (Video)CommentObject;
            v.NumberOfComments++;
            v.Save();            
        }
        else if (type == CommentType.AskAFriend)
        {
            //AskAFriend aaf = AskAFriend.GetAskAFriendByWebAskAFriendID(WebID);
            //CommentObject = aaf;
            //return aaf.AskAFriendID;
        }
        else if (type == CommentType.Blog)
        {
            //BlogEntry blog = BlogEntry.GetBlogEntryByWebBlogEntryID(WebID);
            //CommentObject = blog;
            //return blog.BlogEntryID;
        }

    }

    private void IncrementCommentCount(CommentType type, int ObjID)
    {
        if (type == CommentType.Wall)
        {
            //Member m = Member.GetMemberByMemberIDWithJoin(ObjID);
            
        }
        else if (type == CommentType.Video)
        {
            Video v = Video.GetVideoByVideoIDWithJoin(ObjID);
            v.NumberOfComments++;
            v.Save();
        }
        else if (type == CommentType.AskAFriend)
        {            
        }
        else if (type == CommentType.Blog)
        {
        }
        else if (type == CommentType.Photo)
        {
            Photo p = Photo.GetPhotoByPhotoIDWithJoin(ObjID);
            p.NumberOfComments++;
            p.Save();
        }
    }

    private int GetObjectID(CommentType type, string WebID)
    {   
        if (type == CommentType.Wall)
        {
            Member m = Member.GetMemberViaWebMemberID(WebID);     
            return m.MemberID;
        }
        else if (type == CommentType.Video)
        {
            Video v = Video.GetVideoByWebVideoIDWithJoin(WebID);            
            return v.VideoID;
        }
        else if (type == CommentType.AskAFriend)
        {
            AskAFriend aaf = AskAFriend.GetAskAFriendByWebAskAFriendID(WebID);            
            return aaf.AskAFriendID;
        }
        else if (type == CommentType.Blog)
        {
            BlogEntry blog = BlogEntry.GetBlogEntryByWebBlogEntryID(WebID);            
            return blog.BlogEntryID;
        }
        else if (type == CommentType.Photo)
        {
            Photo photo = Photo.GetPhotoByWebPhotoIDWithJoin(WebID);
            return photo.PhotoID;
        }

        return -1;
    }

    /// <summary>
    /// Converts the object into HTML for rendering in browser
    /// </summary>
    /// <returns></returns>
    public string ToHTML(AjaxComment cmt)
    {
        StringBuilder sbHTML = new StringBuilder();

        object[] parameters = new object[9];

        parameters[0] = cmt.PhotoUrl;
        parameters[1] = cmt.NickName;
        parameters[2] = cmt.DateTimePosted;
        parameters[3] = Next2Friends.Misc.SafeHTML.AutoLink(Next2Friends.Misc.SafeHTML.FormatForHTML(cmt.Text));
        parameters[4] = cmt.WebMemberID;
        parameters[5] = cmt.WebCommentID;
        parameters[6] = cmt.Type;
        parameters[7] = Next2Friends.Misc.SafeHTML.FormatForText(cmt.Text);

        if (cmt.SentFromMobile)
        {
            parameters[8] = "<p><i>Sent from my mobile</i></p>";
        }
        else
        {
            parameters[8] = "";
        }



        if (cmt.Type == null || cmt.Type == "")
        {
            sbHTML.AppendFormat(@"<li class='clearfix' id='li{5}'><a href='/users/{1}'><img src='{0}' alt='' width='50' height='50' class='commenter_avatar' /></a>
							<div class='comment_entry' id='{5}'>								
								<p class='commenter'> <cite><a href='/users/{1}'>{1}</a></cite><br />
								<small>{2}</small></p>
								<p id='commentBody{5}'>{3}{8}</p>                                
							</div>

						</li>", parameters);
        }

        else
        {
            sbHTML.AppendFormat(@"<li class=""clearfix"" id=""li{5}""><a href='/users/{1}'><img src=""{0}"" alt="""" width=""50"" height=""50"" class=""commenter_avatar"" /></a>
							<div class=""comment_entry"" id=""{5}"">
								<p class=""reply""><small><a id=""edit{5}"" href=""javascript:editComment('{6}','{5}');void(0);"">Edit</a><a  id=""delete{5}"" href=""javascript:deleteComment('{6}','{5}');"">Delete</a></small><a id=""reply{5}"" href=""javascript:replyToComment('{6}','{5}');void(0);"">Reply</a></p>
								<p class=""commenter""> <cite><a href=""/users/{1}"">{1}</a></cite><br />
								<small>{2}</small></p>
								<p id=""commentBody{5}"">{3}{8}</p>
                                <p id=""commentBodyHidden{5}"" style=""visibility:hidden;display:none"">{7}.</p>
							</div>

						</li>", parameters);
        }


        return sbHTML.ToString();
    }
    
    /// <summary>
    /// Post a new comment to the members page
    /// </summary>
    /// <param name="WebMemberID">The WebMemberID who owns the page</param>
    /// <param name="CommentText">The text body of the comment</param>
    /// <returns>An AjaxComment object populated with all the comments properties</returns>
    [AjaxPro.AjaxMethod]
    public AjaxComment PostComment(string type, string WebID, string CommentText, int Attempt)
    {
        AjaxComment ajaxComment = new AjaxComment();
        Member member = (Member)HttpContext.Current.Session["Member"];

        CommentType = (CommentType)Enum.Parse(typeof(CommentType), type);
        

        Comment comment = new Comment();

        comment.ObjectID = GetObjectID(CommentType, WebID);
        comment.MemberIDFrom = member.MemberID;
        comment.Text = CommentText;
        comment.DTCreated = DateTime.Now;
        comment.IsDeleted = false;
        comment.CommentType = (int)CommentType;
        comment.WebCommentID = Next2Friends.Misc.UniqueID.NewWebID();
        comment.Path = "/";
        comment.SentFromMobile = 0;
        comment.Save();

        // ++ the comment count for the object if applicable        
        IncrementCommentCount(CommentType, comment.ObjectID);

        comment.ThreadNo = comment.CommentID;
        comment.InReplyToCommentID = comment.CommentID;
        comment.Save();

        ajaxComment = AjaxComment.GetAjaxCommentByCommentIDWithJoin(comment.CommentID);
        ajaxComment.Type = CommentType.ToString();
        ajaxComment.WebObjectID = WebID;
        ajaxComment.TotalNumberOfComments = AjaxComment.GetNumberOfCommentByObjectID(comment.ObjectID,(int)CommentType).ToString();

        return ajaxComment;
    }

    [AjaxPro.AjaxMethod]
    public string DeleteComment(string type, string WebID)
    {
        CommentType = (CommentType)Enum.Parse(typeof(CommentType), type);
        Comment deleteComment = Comment.GetCommentByWebCommentID(WebID);
        Comment.DeleteComment(WebID);
        
        int TotalNumberOfComments = AjaxComment.GetNumberOfCommentByObjectID(deleteComment.ObjectID, (int)CommentType);

        return TotalNumberOfComments.ToString();
    }

    [AjaxPro.AjaxMethod]
    public string UpdateComment(string type, string WebID, string Body,string ObjWebID)
    {
        Comment comment = Comment.GetCommentByWebCommentID(WebID);
        comment.Text = Body;
        comment.Save();
        
        return HTMLUtility.AutoLink(HTMLUtility.FormatForHTML(Body));
    }

    [AjaxPro.AjaxMethod]
    public AjaxComment PostReply(string type, string WebID, string Body, string ObjWebID)
    {
        Member mem = (Member)HttpContext.Current.Session["Member"];
        AjaxComment ajaxComment = new AjaxComment();

        CommentType = (CommentType)Enum.Parse(typeof(CommentType), type);
        

        Comment inReplyToComment = null;

        if (WebID != string.Empty)
            inReplyToComment = Comment.GetCommentByWebCommentID(WebID);

        Comment comment = new Comment();
        comment.MemberIDFrom = mem.MemberID;
        comment.ObjectID = inReplyToComment.ObjectID;
        comment.DTCreated = DateTime.Now;
        comment.IsDeleted = false;
        comment.WebCommentID = Next2Friends.Misc.UniqueID.NewWebID();
        comment.Text = Body;
        comment.CommentType = (int)CommentType;
        comment.SentFromMobile = 0;

        if (inReplyToComment != null)
        {
            comment.InReplyToCommentID = inReplyToComment.CommentID;
            comment.ThreadNo = inReplyToComment.ThreadNo;
        }

        comment.Save();

        // ++ the comment count for the object if applicable        
        IncrementCommentCount(CommentType, comment.ObjectID);


        if (inReplyToComment == null)
        {
            comment.InReplyToCommentID = comment.CommentID;
            comment.ThreadNo = comment.CommentID;
        }


        if (inReplyToComment.Path != "/")
        {
            comment.Path = inReplyToComment.Path;
        }
        else
        {
            comment.Path = inReplyToComment.Path + inReplyToComment.CommentID + "/";
        }

        comment.Save();

        ajaxComment = AjaxComment.GetAjaxCommentByCommentIDWithJoin(comment.CommentID);
        ajaxComment.WebObjectID = ObjWebID;
        ajaxComment.Type = CommentType.ToString();

        ajaxComment.TotalNumberOfComments = AjaxComment.GetNumberOfCommentByObjectID(comment.ObjectID, (int)CommentType).ToString();

        return ajaxComment;
    }
}

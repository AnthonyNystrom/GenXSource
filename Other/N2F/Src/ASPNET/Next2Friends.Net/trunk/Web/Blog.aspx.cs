using System;
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
using System.Text; 
using System.Collections.Generic;

using Next2Friends.CrossPoster.Client.Engines;
using Next2Friends.CrossPoster.Client.Logic;
using Google.GData.Client;

public partial class Blog : System.Web.UI.Page
{
    private Member member;
    public Member ViewingMember;
    public bool IsLoggedIn = false;

    public string BlogTitle;
    public string BlogCreationDt;
    public string BlogBody;
    public string BlogEditableBody;
    public string strBlogID = string.Empty;
    public bool IsMyPage = false;
    public bool HasContent = false;
    public string WebBlogID = string.Empty;

    public string DefaultHTMLPager = string.Empty;

    private BlogEntry blog;
    private List<BlogEntry> blogs;

    int PageTo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {   
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Blog));
         
        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        ViewingMember = ExtractPageParams.GetMember(this.Page, this.Context);

        string strPager = Request.Params["p"];
        strBlogID = Request.Params["b"];

        try
        {
            string []blogID = strBlogID.Split(new char[]{','});

            if (blogID.Length != 0)
            {
                strBlogID = blogID[blogID.Length - 1];
            }
        }
        catch{}



        Int32.TryParse(strPager, out PageTo);
        PageTo = (PageTo == 0) ? 1 : PageTo;

        if (member != null)
        {
            if (ViewingMember.WebMemberID == member.WebMemberID)
            {
                IsMyPage = true;
            }
        }

        if (ViewingMember != null)
        {
            blogs = BlogEntry.GetBlogEntryByMemberID(ViewingMember.WebMemberID);
            blogs = SortBlogsByDate(blogs);
        }

        if (blogs.Count == 0)
        {            
            return;
        }

        HasContent = true;

        int i = 1;

        if (strBlogID != null)
        {
            foreach (BlogEntry b in blogs)
            {
                if (b.WebBlogEntryID == strBlogID)
                {
                    blog = b;
                    PageTo = i;
                    break;
                }
                i++;
            }
        }
        else
        {
            if (PageTo > blogs.Count)
                PageTo = blogs.Count;

            blog = blogs[PageTo - 1];
        }

        if (blog != null)
        {

            strBlogID = blog.WebBlogEntryID;
            WebBlogID = blog.WebBlogEntryID;
            Comments1.ObjectId = blog.BlogEntryID;
            Comments1.ObjectWebId = blog.WebBlogEntryID;
            Comments1.CommentType = CommentType.Blog;        

            PopulateVariables();
            BlogPager pager = new BlogPager("/users/" + ViewingMember.NickName + "/blog/", ViewingMember.NickName + "/", PageTo, blogs.Count);
            pager.PageSize = 1;
            DefaultHTMLPager = (blogs.Count > 0) ? "<span>" + pager.ToString() + "</span>" : string.Empty;            
        }

    }

    private void PopulateVariables()
    {
        BlogTitle = blog.Title;
        BlogCreationDt = blog.DTCreated.ToString("MMM dd, yyyy");
        BlogBody = HTMLUtility.AutoLink(HTMLUtility.FormatForHTML(blog.Body));
        BlogEditableBody = blog.Body.Replace("<br />","\r\n");
    }

    /// <summary>
    /// Updates the blog
    /// </summary>
    /// <param name="WebBlogId">The WebBlogEntryID of the blog</param>
    /// <param name="Title">The new title</param>
    /// <param name="Body">The new body</param>
    /// <returns>New body formatted as HTML</returns>
    [AjaxPro.AjaxMethod]
    public string UpdateBlog(string WebBlogId, string Title, string Body)
    {
        member = (Member)Session["Member"];
        blog = BlogEntry.GetBlogEntryByWebBlogEntryID(WebBlogId);

        if (blog.MemberID != member.MemberID)
            return null;

        blog.Title = Title;
        blog.Body = HTMLUtility.FormatForHTML(Body);

        blog.Save();

        return HTMLUtility.AutoLink(blog.Body);
    }

    /// <summary>
    /// Posts new Blog
    /// </summary>    
    /// <param name="Title">The new title</param>
    /// <param name="Body">The new body</param>
    /// <returns>The new WebBlogId</returns>
    [AjaxPro.AjaxMethod]
    public string NewBlog( string Title, string Body )
    {
        member = (Member)Session["Member"];

        blog = new BlogEntry();

        blog.MemberID = member.MemberID;
        blog.Title = Title;
        blog.Body = HTMLUtility.FormatForHTML(Body);
        blog.DTCreated = DateTime.Now;
        blog.WebBlogEntryID = Next2Friends.Misc.UniqueID.NewWebID();

        blog.Save();

        return blog.WebBlogEntryID;
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        //Master.Master.HTMLTitle = PageTitle.GetMBlogTitle();
        //Master.Master.MetaDescription = "Live mobile video broadcasting networking";
        //Master.Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";
    }

    private List<BlogEntry> SortBlogsByDate(List<BlogEntry> blogsList)
    {
        IOrderedEnumerable<BlogEntry> sortedBlogs = null;

        sortedBlogs = from B in blogsList orderby B.DTCreated descending select B;

        blogsList = sortedBlogs.ToList();

        return blogsList;
    }

    [AjaxPro.AjaxMethod]
    public string GetCrossPostOptions()
    {
        object[] BloggerValues = GetCrossPostValues("blogger");

        BloggerValues[3] = (true) ? "CHECKED" : string.Empty;

        StringBuilder sbHTML = new StringBuilder();

        sbHTML.AppendFormat(@"<div class='formLoginMessage' id='frmMsg'>
				<p id='divMsg' style='margin-left:5px;'>
					Enter login details to post your blog
				</p>
			</div>
			<div class='popupRowLine'>
				<label>blog select</label>
				<select class='form_txt2' id='drpSelect' onchange='blogselect();'>
					<option value='blogger' id='drpSelect'>Blogger</option>
                    <option value='wordpress' id='drpSelect'>Word Press</option>
                    <option value='livejournal' id='drpSelect'>Live Journal</option>   
				</select>
			</div>
			<div class='popupRowLine'>
				<label>Username</label>
				<input type='text' class='form_txt2' value='{0}' id='txtUserName'/>
			</div>
			<div class='popupRowLine'>
				<label>Password</label>
				<input type='password' class='form_txt2' value='{1}' id='txtPassword'/>
			</div>
			<div class='popupRowLine'>
				<label id='lblWp' style='color:#BBBBBB'>Wordpress url</label>
				<input type='text' class='form_txt2' value='{2}' id='txtAddress' disabled='true'/>
			</div>
			<div class='popupRowLine'>
                <p style='float:right'>
				    <input type='submit' value='Save with test submission' class='form_btn' id='btnCp' onclick='saveLogin();'/>
			    </p>
                <div style='float:left'>
				    <label>Active</label>
				    <input style='margin-top:5px' type='checkbox' value='' {3} id='chbAuto'>
                </div>
			</div>
			<div class='clear'/>
           
			", BloggerValues);

        return sbHTML.ToString();
    }

    [AjaxPro.AjaxMethod]
    public object[] GetCrossPostValues(string BlogService)
    {
        if (member == null)
        {
            member = (Member)Session["Member"];
        }

        MemberBlogSettings memberBlogSettings = MemberBlogSettings.GetMemberBlogSettingsByMemberID(member.MemberID);

        object[] Values = new object[] { string.Empty, string.Empty, string.Empty, false };

        switch (BlogService)
        {
            case "blogger":
                Values[0] = memberBlogSettings.BloggerUserName;
                Values[1] = memberBlogSettings.BloggerPassword;
                Values[3] = memberBlogSettings.BloggerAutoSubmit;
                break;
            case "wordpress":
                Values[0] = memberBlogSettings.WordPressUserName;
                Values[1] = memberBlogSettings.WordPressPassword;
                Values[2] = memberBlogSettings.WordPressURL;
                Values[3] = memberBlogSettings.WordPressAutoSubmit;
                break;
            case "livejournal":
                Values[0] = memberBlogSettings.LiveJournalUserName;
                Values[1] = memberBlogSettings.LiveJournalPassword;
                Values[3] = memberBlogSettings.LiveJournalAutoSubmit ;
                break;
            default:
                break;
        }

        return Values;

    }


    [AjaxPro.AjaxMethod]
    public int CrossPost(string BlogService, string Username, string Password, string WPAddress, bool AutoSubmit)
    {
        member = (Member)Session["Member"];
        int Result = 0;

        BlogDescriptor bDescr = new BlogDescriptor();
        bDescr.Username = Username;
        bDescr.Password = Password;

        string Title = "Next2Friends test post";
        string Body = "If you are reading this post then your username and password have been entered correctly";

        bool LoginOK = false;

        try
        {
            switch (BlogService)
            {
                case "blogger":
                    bDescr.BlogType = BlogType.Blogger;
                    bDescr.Address = "http://www.blogger.com";
                    Service service = new Service("blogger", "");
                    service.Credentials = new GDataCredentials(Username, Password);
                    Uri blogPostUri = SelectUserBlog(service);
                    AtomEntry createdEntry = PostNewEntry(service, blogPostUri, Title, Body);
                    LoginOK = true;
                    break;

                case "wordpress":
                    bDescr.BlogType = BlogType.WordPress;
                    bDescr.Address = WPAddress;

                    WordPressEngine wpe = new WordPressEngine();
                    wpe.PublishNewEntry(bDescr, Title, Body);
                    LoginOK = true;
                    break;

                case "livejournal":
                    bDescr.BlogType = BlogType.LiveJournal;
                    bDescr.Address = "http://www.livejournal.com";

                    LiveJournalEngine lje = new LiveJournalEngine();
                    lje.PublishNewEntry(bDescr, Title, Body);
                    LoginOK = true;
                    break;

                default:
                    break;
            }

            Result = 0;

        }
        catch (Exception ex)
        {
            Result = 1;
        }

        //if (LoginOK)
        if (true) // always save
        {
            MemberBlogSettings memberBlogSettings = MemberBlogSettings.GetMemberBlogSettingsByMemberID(member.MemberID);

            switch (BlogService)
            {
                case "blogger":
                    memberBlogSettings.BloggerUserName = Username;
                    memberBlogSettings.BloggerPassword = Password;
                    memberBlogSettings.BloggerAutoSubmit = AutoSubmit;
                    break;

                case "wordpress":
                    memberBlogSettings.WordPressUserName = Username;
                    memberBlogSettings.WordPressPassword = Password;
                    memberBlogSettings.WordPressURL = WPAddress;
                    memberBlogSettings.WordPressAutoSubmit = AutoSubmit;
                    break;

                case "livejournal":
                    memberBlogSettings.LiveJournalUserName = Username;
                    memberBlogSettings.LiveJournalPassword = Password;
                    memberBlogSettings.LiveJournalAutoSubmit = AutoSubmit;
                    break;

                default:
                    break;
            }

            memberBlogSettings.Save();
        }

        return Result;
    }

    /// <summary>
    /// Lists the user's blogs and returns the URI for posting new entries to the blog which the user selected.
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>

    private Uri SelectUserBlog(Service service)
    {
        FeedQuery query = new FeedQuery();
        // Retrieving a list of blogs
        query.Uri = new Uri("http://www.blogger.com/feeds/default/blogs");
        AtomFeed feed = service.Query(query);

        // Publishing a blog post
        Uri blogPostUri = null;
        if (feed != null)
        {
            foreach (AtomEntry entry in feed.Entries)
            {
                // find the href in the link with a rel pointing to the blog's feed
                for (int i = 0; i < entry.Links.Count; i++)
                {
                    if (entry.Links[i].Rel.Equals("http://schemas.google.com/g/2005#post"))
                    {
                        blogPostUri = new Uri(entry.Links[i].HRef.ToString());
                    }
                }
                return blogPostUri;
            }
        }
        return blogPostUri;
    }

    /// <summary>
    /// Creates a new blog entry and sends it to the specified Uri
    /// </summary>
    /// <param name="service"></param>
    /// <param name="blogPostUri"></param>
    /// <returns></returns>

    private AtomEntry PostNewEntry(Service service, Uri blogPostUri, string Title, string Body)
    {
        AtomEntry createdEntry = null;
        if (blogPostUri != null)
        {
            // construct the new entry
            AtomEntry newPost = new AtomEntry();
            newPost.Title.Text = Title;
            newPost.Content = new AtomContent();
            newPost.Content.Content = Body;
            newPost.Content.Type = "xhtml";
            newPost.Authors.Add(new AtomPerson());
            newPost.Authors[0].Name = string.Empty;
            newPost.Authors[0].Email = string.Empty;

            createdEntry = service.Insert(blogPostUri, newPost);
        }
        return createdEntry;
    }
}

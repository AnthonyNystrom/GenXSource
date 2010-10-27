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

public partial class DisplayBlog : System.Web.UI.UserControl
{
    private Member member { get; set; }
    public Member ViewingMember { get; set; }
    public bool IsLoggedIn { get; set; }
    public bool IsMyPage { get; set; }
    public string WebBlogID { get; set; }

    public string BlogTitle;
    public string BlogCreationDt;
    public string BlogBody;
    public string BlogEditableBody;

    public BlogEntry Blog { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        AjaxPro.Utility.RegisterTypeForAjax(typeof(DisplayBlog));

        if (Blog != null)
        {
            Comments1.ObjectId = Blog.BlogEntryID;
            Comments1.ObjectWebId = Blog.WebBlogEntryID;
            Comments1.CommentType = CommentType.Blog;
            Comments1.Collapsed = true;

            PopulateVariables();                        
        }

    }


    private void PopulateVariables()
    {
        BlogTitle = Blog.Title;
        BlogCreationDt = Blog.DTCreated.ToString("MMM dd, yyyy");
        BlogBody = HTMLUtility.AutoLink(HTMLUtility.FormatForHTML(Blog.Body));
        BlogEditableBody = Blog.Body.Replace("<br />", "\r\n");
        WebBlogID = Blog.WebBlogEntryID;
    }

    [AjaxPro.AjaxMethod]
    public string GetCrossPostOptions()
    {
        string HTML = @"<div><div style='float:right;border-color:#cccccc;' id='divMsg'></div>
        Blog select:
            <select id='drpSelect'><option value='blogger' id='drpSelect'>Blogger</option>
            <option value='wordpress' id='drpSelect'>Word Press</option>
            <option value='livejournal' id='drpSelect'>Live Journal</option>            
            </select>     
        </div>
        <div>
            Username:<input type='text' id='txtUserName'>
        </div>
        <div>
            Password:<input type='text' id='txtPassword'>
        </div>
        <div>
            Optional Wordpress address:<input type='text' id='txtAddress'>
        </div>
        <div>
        </div>
        <div>
            <input type='button' value='Submit' id='btnCp' onclick='CrossPost();' />
        </div>";

        return HTML;
    }


    [AjaxPro.AjaxMethod]
    public string CrossPost(string WebBlogID, string BlogService,string Username, string Password, string WPAddress)
    {
        string ErrorMessage=string.Empty;

        BlogDescriptor bDescr = new BlogDescriptor();
        bDescr.Username = Username;
        bDescr.Password = Password;

        BlogEntry blog = BlogEntry.GetBlogEntryByWebBlogEntryID(WebBlogID);

        if (blog == null)
            throw new Exception("invalid blog entry");

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
                    AtomEntry createdEntry = PostNewEntry(service, blogPostUri, blog.Title, blog.Body);
                    break;

                case "wordpress":
                    bDescr.BlogType = BlogType.WordPress;
                    bDescr.Address = WPAddress;

                    WordPressEngine wpe = new WordPressEngine();
                    wpe.PublishNewEntry(bDescr, blog.Title, blog.Body);
                    break;

                case "livejournal":
                    bDescr.BlogType = BlogType.LiveJournal;
                    bDescr.Address = "http://www.livejournal.com";

                    LiveJournalEngine lje = new LiveJournalEngine();
                    lje.PublishNewEntry(bDescr, blog.Title, blog.Body);
                    break;

                default:
                    break;
            }

        }
        catch (Exception ex)
        {
            //ErrorMessage = ex.Message;
            ErrorMessage = "Could not login, please check your credentials";
        }

        return ErrorMessage;
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

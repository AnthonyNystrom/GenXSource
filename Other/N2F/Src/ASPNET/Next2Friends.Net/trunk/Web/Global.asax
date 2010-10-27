<%@ Application Language="C#" %>
<%@ Import Namespace="Next2Friends.Misc" %>
<%@ Import Namespace="Next2Friends.Data" %>
<%@ Import Namespace="System.Web.Routing" %>


<script runat="server">

    public static string DiskUserRoot { get; set; }
    public static string WebRoot { get; set; }
    public static string WebServerRoot = @"http://www.next2friends.com/";
    public static int NewMessageCount = 0;

    void Application_Start(object sender, EventArgs e) 
    {
        URLRewritingRoutes.RegisterRoutes(RouteTable.Routes);
        
        try
        {
            DiskUserRoot = Next2Friends.Misc.OSRegistry.GetDiskUserDirectory();
            WebRoot = Next2Friends.Misc.OSRegistry.GetWebRootDirectory();
        }
        catch { }
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        Utility.RememberMeLogin();

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        string QueryString = Request.Url.PathAndQuery;

        bool SetCacheHeader = false;

        if (QueryString.Contains(".js") || QueryString.Contains(".css") || QueryString.Contains(".swf"))
        {
            SetCacheHeader = true;
        }
        else if (QueryString.Contains("/images/"))
        {
            SetCacheHeader = true;
        }
        else if (QueryString.Contains("prototype.ashx") || QueryString.Contains("core.ashx") || QueryString.Contains("ms.ashx") || QueryString.Contains("converter.ashx"))
        {
            SetCacheHeader = true;
        }

        if (SetCacheHeader)
        {
            Response.Cache.AppendCacheExtension("post-check=100000,pre-check=600000");
            Response.Cache.SetMaxAge(new TimeSpan(7,0,0,0 ));
            //Response.Cache.SetETagFromFileDependencies();
            //Response.Cache.SetLastModifiedFromFileDependencies();
        }
    }

  

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        try
        {
            Member member = (Member)Session["Member"];

            if (member != null)
            {
                //Next2Friends.ChatClient.ChatInbox inbox = Next2Friends.ChatClient.ChatLogic.GetInbox(member.WebMemberID);
               // ChatInboxList.Remove(inbox);


                try
                {
                    LoggedIn.DeleteLoggedInByMemberID(member.MemberID);                   
                }
                catch { }

            }
        }
        catch { }

    }
       
</script>

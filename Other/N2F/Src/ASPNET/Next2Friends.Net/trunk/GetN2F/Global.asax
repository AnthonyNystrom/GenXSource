<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>


<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        RegisterRoutes(RouteTable.Routes);
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

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        string URL = Context.Request.Url.AbsolutePath.Replace("/","");

        if (URL == "1")
        {
            Server.Transfer("j2medownload.aspx");
        }
        else if (URL == "2")
        {
            Server.Transfer("symbiandownload.aspx");
        }
        else if (URL == "3")
        {
            Context.Response.Redirect("/apps/n2f.sisx");
        }
        else if (URL == "4")
        {
             Context.Response.Redirect("/apps/AAF.jar");
        }
        else if (URL == "5")
        {
            Context.Response.Redirect("/apps/AAF.jad");
        }
        else if (URL == "6")
        {
            Context.Response.Redirect("/apps/Tag.jar");
        }
        else if (URL == "7")
        {
            Context.Response.Redirect("/apps/Tag.jad");
        }
        else if (URL == "8")
        {
            Context.Response.Redirect("/apps/Sup.jar");
        }
        else if (URL == "9")
        {
            Context.Response.Redirect("/apps/Sup.jad");
        } 
    }

    public static void RegisterRoutes(RouteCollection routes)
    {
        
        //var Download1RouteHandler = new WebFormRouteHandler<Page>("~/apps/n2f.sisx");
        //routes.Add(new Route("getn2f/1/}", Download1RouteHandler));

        //var Download2RouteHandler = new WebFormRouteHandler<Page>("~/minidownload.aspx");
        //routes.Add(new Route("getn2f/2/}", Download2RouteHandler));

        //var Download3RouteHandler = new WebFormRouteHandler<Page>("~/apps/AAF.jar");
        //routes.Add(new Route("getn2f/3/}", Download3RouteHandler));

        //var Download4RouteHandler = new WebFormRouteHandler<Page>("~/apps/Tag.jar");
        //routes.Add(new Route("getn2f/4/}", Download4RouteHandler));

        //var Download5RouteHandler = new WebFormRouteHandler<Page>("~/apps/Sup.jar");
        //routes.Add(new Route("getn2f/5/}", Download5RouteHandler));
    }

    void Session_End(object sender, EventArgs e) 
    {

    }
       
</script>

using System.Web;
using System.Web.Configuration;
using System;
using Next2Friends.Misc;

public class HttpErrorModule : IHttpModule
{
    private void Context_Error(object sender, EventArgs e)
    {
        HttpContext context = ((HttpApplication)sender).Context;

        // determine if it was an HttpException
        if ((object.ReferenceEquals(context.Error.GetType(), typeof(HttpException))))
        {
            // Get the Web application configuration.
            System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~/web.config");
            // Get the section.
            CustomErrorsSection customErrorsSection = (CustomErrorsSection)configuration.GetSection("system.web/customErrors");
            // Get the collection
            CustomErrorCollection customErrorsCollection = customErrorsSection.Errors;
            int statusCode = ((HttpException)context.Error).GetHttpCode();

            //Clears existing response headers and sets the desired ones.
            context.Response.ClearHeaders();
            context.Response.StatusCode = statusCode;

            if (statusCode == 404)
            {
                string Path = context.Request.Path;

                if (Path.Contains("playlist.xml"))
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 101;
                    context.Response.Redirect("/mp3/playlist.xml");
                }
                else if(!Path.EndsWith(".js") && !Path.EndsWith(".gif") && !Path.EndsWith(".jpg") && !Path.EndsWith(".png"))
                {
                    context.Response.Clear();
                    context.Response.Write("404 file not found");
                    context.Response.Redirect("/404.aspx");
                }
            }
            else
            {
                
            }

            //context.Response.Flush();
        }
        else
        {
            // log the error here in the tracer

            //Clears existing response headers and sets the desired ones.
            context.Response.ClearHeaders();
            context.Response.StatusCode = 500;
            //context.Server.Transfer("/500.aspx");
            if (!context.Request.IsLocal)
            {
                context.Response.Redirect("/500.aspx");
            }
            //
        }
    }

    #region IHttpModule Members

    public void Dispose()
    {
        //Do nothing here
    }

    public void Init(HttpApplication context)
    {
        context.Error += new EventHandler(Context_Error);
    }

    #endregion
}
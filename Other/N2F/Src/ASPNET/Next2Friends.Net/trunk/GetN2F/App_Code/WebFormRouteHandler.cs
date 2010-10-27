using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Routing;
using System.Web.Compilation;

public class WebFormRouteHandler<T> : IRouteHandler where T : IHttpHandler, new()
{
    public string VirtualPath { get; set; }

    public WebFormRouteHandler(string virtualPath)
    {
        this.VirtualPath = virtualPath;
    }

    #region IRouteHandler Members

    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
        foreach (var value in requestContext.RouteData.Values)
        {
            requestContext.HttpContext.Items[value.Key] = value.Value;
        }

        return (VirtualPath != null)
            ? (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(VirtualPath, typeof(T))
            : new T();
    }

    #endregion
}
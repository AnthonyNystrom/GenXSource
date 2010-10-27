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

/// <summary>
/// Summary description for AjaxChatMessage
/// </summary>
public class AjaxChatMessage
{
    /// <summary>
    /// wether the message was succsessfully delivered
    /// </summary>
    public bool Delivered
    {
        get;
        set;
    }

    public bool Retrieved { get; set; }

    public string WebMemberIDFrom
    {
        get;
        set;
    }

    public string WebMemberIDTo { get; set; }

    public int MemberIDTo { get; set; }

    /// <summary>
    /// The chat id that can safely go to web browser
    /// </summary>
    public string ChatMessageWebID
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public int MemberIDFrom
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Message
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public DateTime DTCreated
    {
        get;
        set;
    }

    public int Outbound
    {
        get;
        set;
    }

}

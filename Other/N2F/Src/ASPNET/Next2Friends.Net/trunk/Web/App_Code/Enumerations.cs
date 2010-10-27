using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public enum DefaultPageType { None, Video, Photo, Member, NSpot, LiveBroadcast, Blog }
public enum CommentType { None = 0, Video = 1, Photo = 2, Wall = 3, NSpot = 4, LiveBroadcast = 5, Blog = 6, AskAFriend = 7, PhotoGallery = 21, Member = 22 }


using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using Next2Friends.Misc;
using Next2Friends.Data;


public partial class MP3Upload : System.Web.UI.Page
{
    public Member member;
    public string Mp3Lister = string.Empty;
    //private string UserMP3Dir = @"\\www\user\";
    private string UserMP3Dir = @"C:\Documents and Settings\Admin\My Documents\Visual Studio 2008\Projects\Next2Friends.root\Next2Friends\Web\mp3\";
    private string XMLPlayListLocation = string.Empty;
    public string RestartPlayer = "false";
    public string DisplayNoMP3Message = "none";

    protected void Page_Load(object sender, EventArgs e)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Utility.RememberMeLogin();
        }

        AjaxPro.Utility.RegisterTypeForAjax(typeof(MP3Upload));

        UserMP3Dir = UserMP3Dir + member.NickName + @"\mp3\";

        XMLPlayListLocation = UserMP3Dir + @"\playlist.xml";
        BuildMp3Lister();
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {

        if (mp3Upload.HasFile)
        {
            string Extention = Path.GetExtension(mp3Upload.FileName).ToLower();

            if (Extention != ".mp3")
            {
                //not an mp3 file
            }
            else
            {
                string FileName = Path.GetFileNameWithoutExtension(mp3Upload.FileName).ToLower();
                string FinalTitle = FileName.Replace("<", "&#60;").Replace(">", "&#62;").Replace(@"""", "'");
                string Title = FinalTitle.ToLower();

                // make sure the directory exists
                if (!Directory.Exists(UserMP3Dir))
                {
                    try
                    {
                        Directory.CreateDirectory(UserMP3Dir);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(UserMP3Dir+":::"+ex.ToString());
                    }
                    StreamWriter SWPlayList = File.CreateText(XMLPlayListLocation);
                    SWPlayList.Write(@"<?xml version='1.0' encoding='UTF-8'?><playlist version='1' xmlns = 'http://xspf.org/ns/0/'><trackList></trackList></playlist>");
                    SWPlayList.Flush();
                    SWPlayList.Close();
                }

                //try
                //{
                    string WebMp3ID = Next2Friends.Misc.UniqueID.NewWebID();
                    string MP3FileName = WebMp3ID + ".mp3";
                    String SavePath = UserMP3Dir + MP3FileName;
                    mp3Upload.SaveAs(SavePath);
                    AddToPlayList(WebMp3ID,Title);
                    RestartPlayer = "true";

                    Next2Friends.Data.MP3Upload mP3Upload = new Next2Friends.Data.MP3Upload();
                    mP3Upload.WebMP3UploadID = WebMp3ID;
                    mP3Upload.MemberID = member.MemberID;
                    mP3Upload.Title = Title;
                    mP3Upload.Path = MP3FileName;
                    mP3Upload.Save();

                //}
                //catch (Exception ex)
                //{
                    //throw ex;
                //}
            }

        }

        BuildMp3Lister();

    }



    public void BuildMp3Lister()
    {
        if (!File.Exists(XMLPlayListLocation))
        {
            DisplayNoMP3Message = "block";
            return;
        }

        DisplayNoMP3Message = "none";

        Mp3Lister = string.Empty;

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(XMLPlayListLocation);

        XmlNodeList Nodes = xmldoc.GetElementsByTagName("track");

        if (Nodes.Count > 0)
        {
            Mp3Lister = "<ul id='mp3Lister'>";
        }
        else
        {
            DisplayNoMP3Message = "block";
        }

        for (int i = 0; i < Nodes.Count; i++)
        {
            string WebMP3ID = Nodes[i]["webmp3id"].InnerText;
            string Title = ClipString(Nodes[i]["title"].InnerText,70);


            string Play = string.Empty;// "javascript:EP_loadMP3('ep_player', '<location>/user/" + member.NickName + "/mp3/" + WebMP3ID + ".mp3</location><creator></creator><title>" + Title + "</title>');";

            //Mp3Lister += "<li id='mp3" + WebMP3ID + "'><a class='playPlaylistItem' href=\"" + Play + "\">play</a><a href=\"javascript:deleter('" + WebMP3ID + "')\" class='deletePlaylistItem'><img src='./images/unfriend-bg.gif' width='12' height='11' alt='Remove item' /></a>";
            Mp3Lister += "<li id='mp3" + WebMP3ID + "'><a href=\"javascript:deleter('" + WebMP3ID + "')\" class='deletePlaylistItem'><img src='./images/unfriend-bg.gif' width='12' height='11' alt='Remove item' /></a>";
			Mp3Lister += Title+"</li>";
            //string FileName = Path.GetFileName(Mp3FileName[i]).Replace("-", " ");
            //Mp3Lister += @"<div class='mp3row' id='mp3" + WebMP3ID + "'><a style='float:right;' href=\"javascript:deleter('" + WebMP3ID + "')\">delete</a><div>" + Title + "</div></div>";
             
        }

        if (Nodes.Count > 0)
        {
            Mp3Lister += "</ul>";
        }
    }

    public string ClipString(string Value,int ClipTo)
    {
        if (Value.Length > ClipTo)
        {
            Value = Value.Substring(0, ClipTo) + "..";
        }

        return Value;
    }

    public void AddToPlayList(string WebMP3ID, string Mp3Title)
    {
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(XMLPlayListLocation);

        XmlElement ItemElement = xmldoc.CreateElement("track");

        XmlNode FileNameElement = (XmlNode)xmldoc.CreateElement("location");
        FileNameElement.InnerText = @"http://www.next2friends.com/user/"+member.NickName+"/mp3/" + WebMP3ID + ".mp3";
        //FileNameElement.InnerText = @"/mp3/" + member.NickName + "/mp3/" + WebMP3ID + ".mp3";
        ItemElement.AppendChild(FileNameElement);

        XmlNode WebMp3IDElement = (XmlNode)xmldoc.CreateElement("webmp3id");
        WebMp3IDElement.InnerText = WebMP3ID;
        ItemElement.AppendChild(WebMp3IDElement);

        XmlNode TitleElement = (XmlNode)xmldoc.CreateElement("title");
        TitleElement.InnerText = Mp3Title;
        ItemElement.AppendChild(TitleElement);

        xmldoc.GetElementsByTagName("trackList")[0].AppendChild(ItemElement);

        xmldoc.Save(XMLPlayListLocation);

        //<track>
        //    <location>mp3/demo.mp3</location>
        //    <title>MP3 Player!</title>
        //    <creator>E-Phonic</creator>
        //    <image>mp3/demo.jpg</image>
        //</track>
    }

    [AjaxPro.AjaxMethod]
    public string DeleteMp3(string WebMP3ID)
    {
        member = (Member)Session["Member"];
        UserMP3Dir = UserMP3Dir + member.NickName + @"\mp3\";
        XMLPlayListLocation = UserMP3Dir + @"\playlist.xml";

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(XMLPlayListLocation);

        XmlNodeList Nodes = xmldoc.GetElementsByTagName("track");

        for (int i = 0; i < Nodes.Count; i++)
		{
            string ThisWebMP3ID = Nodes[i]["webmp3id"].InnerText;

            if (ThisWebMP3ID == WebMP3ID)
            {
               xmldoc.GetElementsByTagName("trackList")[0].RemoveChild(Nodes[i]);
            }
		}

        xmldoc.Save(XMLPlayListLocation);

        string MP3FileName = UserMP3Dir + WebMP3ID + ".mp3";

        if (File.Exists(MP3FileName))
        {
            try
            {
                File.Delete(MP3FileName);
            }
            catch { }
        }

        Next2Friends.Data.MP3Upload.DeleteMP3UploadByWebMP3UploadID(WebMP3ID);

        return WebMP3ID;
    }

}

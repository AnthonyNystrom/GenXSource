using System;
using System.Collections.Generic;
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
using Next2Friends.Data;

public partial class MyVideoGallery : System.Web.UI.Page
{ 
    public string ListerHTML;
    public List<Video> videos;
    public int IIndexer = 0;
    public string Indexer = string.Empty;
    public string NoCacheID = new Random().Next(9999999).ToString();
    public Member member;
    public string NoVideosMessage = string.Empty;
    public bool NoVideos = true;
    public string CategoryTagsArrays = string.Empty;
    public string FuncDec = string.Empty;
    public string FuncShowTags = string.Empty;
    public string FuncCategorySelect = string.Empty;
    public string PreviewHTML = string.Empty;
    public string PagerHTML = string.Empty;
    public int CurrentPageIndex = 1;    

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(MyVideoGallery));

        // create the JS Arrays for autopopulating Tags from cats
        CategoryTagsArrays = GenerateCategoryTagsArrays();

        string strPage = Request.Params["p"];
        CurrentPageIndex = Pager.TryGetPageIndex(strPage);

        if (!IsPostBack)
        {
            member = (Member)Session["Member"];

            Bind(CurrentPageIndex);
        }
    }

    public string GenerateCategoryTagsArrays()
    {
        List<Category> Categories = Category.GetAllCategory();

        string FinalJS = "var tagArray = new Array();\r\n";

        for (int i = 0; i < Categories.Count; i++)
        {
            FinalJS += "tagArray[" + i + "] = new Array(" + Categories[i].CategoryID + ", new Array(";

            string[] Tags = Category.GetTagsFromCategory(Categories[i].CategoryID);

            for (int j = 0; j < Tags.Length; j++)
            {
                FinalJS += "'" + Tags[j] + "'";
                FinalJS += (j < Tags.Length - 1) ? "," : string.Empty;
            }

            FinalJS += "));\r\n";
        }

        return FinalJS;
    }


    protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        int IndexerPlus1 = e.Item.ItemIndex + 1;

        Indexer = (IndexerPlus1 < 10) ? "0" + IndexerPlus1.ToString() : IndexerPlus1.ToString();
        FuncDec = "enableSave('" + Indexer + "')";
        FuncCategorySelect = "populateTags('" + Indexer + "');" + FuncDec;
        FuncShowTags = "populateTags('" + Indexer + "')";


        Repeater repeater = (Repeater)sender;
        List<Video> videos = (List<Video>)repeater.DataSource;

        if (videos != null)
        {
            if (e.Item.ItemIndex < videos.Count && e.Item.ItemIndex != -1)
            {
                Video video = videos[e.Item.ItemIndex];

                bool IsEncoding = (video.Status == (int)VideoStatus.Active) ? false : true;

                if (IsEncoding)
                {
                    PreviewHTML = "<img src='/images/video-encoded.gif' />";
                }
                else
                {
                    string ThumbnailURL = "http://www.next2friends.com/" + video.ThumbnailResourceFile.FullyQualifiedURL;

                    PreviewHTML = @"<div id='divVideoPlayer" + Indexer + "'></div>";
                    
                    PreviewHTML += @"<script type='text/javascript'>";
                    PreviewHTML += @"var s1 = new SWFObject('/flvplayermini.swf','n2fplayer','124','122','7');";
                    PreviewHTML += @"s1.addParam('allowfullscreen','true');";
                    PreviewHTML += @"s1.addParam('bgcolor','#FFFFFF');";
                    PreviewHTML += @"s1.addParam('wmode','opaque');";
                    PreviewHTML += @"s1.addVariable('Ad','false');";
                    PreviewHTML += @"s1.addVariable('file','http://www.next2friends.com/" + video.VideoResourceFile.FullyQualifiedURL+"');";
                    PreviewHTML += @"s1.addVariable('image', '" + ThumbnailURL + "');";
                    PreviewHTML += @"s1.addVariable('width','124');";
                    PreviewHTML += @"s1.addVariable('height','122');";
                    PreviewHTML += @"s1.addVariable('autostart','false');";
                    PreviewHTML += @"s1.addVariable('mini', 'true');";
                    PreviewHTML += @"s1.write('divVideoPlayer" + Indexer + "');";
                    PreviewHTML += @"</script>";

                    //PreviewHTML += @"<script type='text/javascript'>";
                    //PreviewHTML += @"var F1 = { movie:'flvplayer.swf',wmode:'transparent',mini:'true',image:'" + ThumbnailURL + "', width:'124', height:'122', majorversion:'8',quality:'high', build:'40', allowscriptaccess:'sameDomain',flashvars:'file=http://www.next2friends.com/" + video.VideoResourceFile.FullyQualifiedURL + "&amp;Ad=false&width=124&height=122&mini=true&image=" + ThumbnailURL + "' };";
                    //PreviewHTML += @"UFO.create(F1, 'divVideoPlayer" + Indexer + "');";
                    //PreviewHTML += @"</script>";
                }

                if (video.PrivacyFlag == (int)PrivacyType.Network)
                {

                    CheckBox chbPrivacy = (CheckBox)e.Item.FindControl("chbPrivacy");
                    chbPrivacy.Checked = true;
                }
            }
        }
    }

    public void Bind(int Page)
    {
        int NumberOfVideos = Video.GetVideosCountByMemberID(member.MemberID);

        videos = Video.GetVideosByMemberIDWithJoinPager(member.MemberID, Page);

        if (videos.Count == 0)
        {
            NoVideosMessage = "<p>You have no videos in your account. To upload please use the big blue button to the right!";
            NoVideos = true;
        }
        else
        {
            NoVideos = false;
        }

        VideoRepeater.DataSource = videos;
        VideoRepeater.DataBind();

        Pager pager = new Pager("/myvideos/", Page, NumberOfVideos);

        pager.PageSize = 10;

        PagerHTML = pager.ToString();
    }

    public string GetNextIndex()
    {
        string Value = (++IIndexer < 10) ? "0" + IIndexer.ToString() : IIndexer.ToString();
        return Value;
    }

    public Video GetLocalVideoByWebVideoID(string WebVideoID)
    {
        if (videos == null)
            videos = Video.GetVideosByMemberIDWithJoin(member.MemberID,PrivacyType.Network);

        for (int i = 0; i < videos.Count; i++)
        {
            if (videos[i].WebVideoID == WebVideoID)
                return videos[i];
        }

        return null;
    }

    [AjaxPro.AjaxMethod]
    public VideoGalleryItem SaveSingle(string WebVideoID, int CatgoryID, string txtTags, string txtTitle, string txtCaption, bool DoDelete, string strIndex,bool MakePrivate)
    {
        VideoGalleryItem galleryItem = new VideoGalleryItem();
        galleryItem.Index = strIndex;

        member = (Member)Session["Member"];
        bool Update = false;

        if (member != null)
        {
                Video video = Video.GetVideoByWebVideoIDWithJoin(WebVideoID);

                if (video != null)
                {
                    
                

                if (video.MemberID == member.MemberID)
                {

                    if (DoDelete)
                    {
                        video.Delete();
                        galleryItem.IsRemoved = true;
                    }
                    else
                    {

                        if (video.Category != CatgoryID)
                        {
                            video.Category = CatgoryID;
                            Update = true;
                        }

                        if (video.Tags != txtTags)
                        {
                            video.Tags = txtTags;
                            Update = true;

                            video.UpdateTags();

                            galleryItem.Tags = video.Tags;
                        }

                        if (video.Title != txtTitle)
                        {
                            video.Title = txtTitle;
                            Update = true;
                        }

                        if (video.Description != txtCaption)
                        {
                            video.Description = txtCaption;
                            Update = true;
                        }

                        PrivacyType pt = PrivacyType.Public;
                        if (MakePrivate)
                            pt = PrivacyType.Network;

                        if (video.PrivacyFlag != (int)pt)
                        {
                            video.PrivacyFlag = (int)pt;
                            Update = true;
                        }

                        if (Update)
                        {
                            video.Save();
                        }
                    }
                }
                }
        }

        return galleryItem;
   }
        


    protected void btnSave_Click(object sender, EventArgs e)
    {
        member = (Member)Session["Member"];
        bool Update = false;

        if (member != null)
        {
            for (int i = 0; i < VideoRepeater.Items.Count; i++)
            {
                HiddenField HiddenFieldWebVideoID = (HiddenField)VideoRepeater.Items[i].FindControl("WebVideoID");
                string WebVideoID = HiddenFieldWebVideoID.Value;

                Video video = GetLocalVideoByWebVideoID(WebVideoID);

                if (video != null)
                {

                    if (video.MemberID == member.MemberID)
                    {
                        CheckBox chbPrivacy = (CheckBox)VideoRepeater.Items[i].FindControl("chbPrivacy");
                        CheckBox chbDelete = (CheckBox)VideoRepeater.Items[i].FindControl("chbDelete");
                        bool DoDelete = chbDelete.Checked;

                        if (DoDelete)
                        {
                            video.Delete();
                        }
                        else
                        {
                            TextBox txtTitle = (TextBox)VideoRepeater.Items[i].FindControl("txtTitle");
                            TextBox txtCaption = (TextBox)VideoRepeater.Items[i].FindControl("txtCaption");
                            TextBox txtTags = (TextBox)VideoRepeater.Items[i].FindControl("txtTags");
                            DropDownList drpCategories = (DropDownList)VideoRepeater.Items[i].FindControl("drpCategories");

                            if (video.Category.ToString() != drpCategories.SelectedValue)
                            {
                                video.Category = Int32.Parse(drpCategories.SelectedValue);
                                Update = true;
                            }


                            if (video.Tags != txtTags.Text)
                            {
                                video.Tags = txtTags.Text;
                                Update = true;

                                video.UpdateTags();

                                txtTags.Text = video.Tags;
                            }

                            if (video.Title != txtTitle.Text)
                            {
                                video.Title = txtTitle.Text;
                                Update = true;
                            }

                            if (video.Description != txtCaption.Text)
                            {
                                video.Description = txtCaption.Text;
                                Update = true;
                            }

                            PrivacyType pt = PrivacyType.Public;
                            if (chbPrivacy.Checked)
                                pt = PrivacyType.Network;

                            if (video.PrivacyFlag != (int)pt)
                            {
                                video.PrivacyFlag = (int)pt;
                                Update = true;
                            }

                            if (Update)
                            {
                                video.Save();
                            }
                        }
                    }
                }
            }
        }

        Bind(CurrentPageIndex);

    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Response.Redirect("signup.aspx");
        }

        Master.SkinID = "none";
        base.OnPreInit(e);
    }

    public class VideoGalleryItem
    {
        public string WebVideoCollectionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Index { get; set; }
        public string Tags { get; set; }
        public bool IsRemoved { get; set; }

    }
}

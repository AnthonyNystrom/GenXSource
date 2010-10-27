using System;
using System.Text;
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

public partial class MyPhotoGallery : System.Web.UI.Page
{
    public string ListerHTML;
    public List<Photo> photos;
    public int IIndexer = 0;
    public string Indexer = string.Empty;
    public string FuncDec = string.Empty;
    public string FuncCategorySelect = string.Empty;
    public string NoCacheID = new Random().Next(9999999).ToString();
    public Member member;
    public List<PhotoCollection> photoCollections;
    //public string Pager = string.Empty;
    public string MyGalleriesHTML = string.Empty;
    public int DefaultPhotoCollectionID = 0;
    public string GalleryName = string.Empty;
    public string CategoryTagsArrays = string.Empty;
    public int CurrentPageIndex = 1;
    public string PagerHTML = string.Empty;
    public string DefaultWebPhotoCollectionID = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(MyPhotoGallery));

        // create the JS Arrays for autopopulating Tags from cats
        CategoryTagsArrays = GenerateCategoryTagsArrays();

        string strPage = Request.Params["p"];
        CurrentPageIndex = Pager.TryGetPageIndex(strPage);

        member = (Member)Session["Member"];

        string strPhotoCollectionDeleteID = Request.Params["d"];
        string strReload= Request.Params["reload"];
        
        if (strPhotoCollectionDeleteID != null)
        {
            PhotoCollection photoCollection = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(strPhotoCollectionDeleteID);

            if (photoCollection != null)
            {
                if (photoCollection.MemberID == member.MemberID)
                {
                    photoCollection.Delete();
                }
            }
        }

        DefaultWebPhotoCollectionID = Request.Params["pc"];

        // hack to reload photocollection
        member.PhotoCollection = null;

        // if a photocollectionid has been specified then load the new collection
        if (DefaultWebPhotoCollectionID != null && !IsPostBack)
        {
            PhotoCollection photoCollection = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(DefaultWebPhotoCollectionID);
            if (photoCollection.MemberID == member.MemberID)
            {

                DefaultPhotoCollectionID = photoCollection.PhotoCollectionID;
                Session["DefaultPhotoCollectionID"] = DefaultPhotoCollectionID;
                Bind(CurrentPageIndex);
            }
        }
        else if (!IsPostBack && DefaultWebPhotoCollectionID == null)
        {
            // get the default photo collectionid on initial load
            DefaultPhotoCollectionID = member.PhotoCollection[0].PhotoCollectionID;
            DefaultWebPhotoCollectionID = member.PhotoCollection[0].WebPhotoCollectionID;
            Session["DefaultPhotoCollectionID"] = DefaultPhotoCollectionID;
            Bind(CurrentPageIndex);
        }

        DefaultPhotoCollectionID = (int)Session["DefaultPhotoCollectionID"];

        GenerateGalleries();
    }

    public string GenerateCategoryTagsArrays()
    {
        List<Category> Categories = Category.GetAllCategory();

        string FinalJS = "var tagArray = new Array();\r\n";

        for (int i = 0; i < Categories.Count; i++)
        {
            FinalJS += "tagArray[" + i + "] = new Array("+Categories[i].CategoryID+", new Array(";

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

    public string GenerateGalleries()
    {
        StringBuilder sbHTMLList = new StringBuilder();

        photoCollections = PhotoCollection.GetAllPhotoCollectionWithEmptyByMemberID(member.MemberID);

        for (int i = 0; i < photoCollections.Count; i++)
        {
            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[6];

            parameters[0] = photoCollections[i].WebPhotoCollectionID;
            parameters[1] = photoCollections[i].ShortName.Replace("'", "&#39;");
            parameters[2] = photoCollections[i].ShortDescription.Replace("'", "&#39;");
            parameters[3] = photoCollections[i].NumberOfPhotos;
            parameters[4] = photoCollections[i].Description.Replace("'", "&#39;");
            parameters[5] = i;

            string HTMLItem = @"<div class='category_item clearfix'>
					        <p class='cat_name'>
						        <a href='/MyPhotoGallery.aspx?pc={0}' id='lblName{0}'><strong>{1}</strong></a> ({3})
					        </p>
					        <p class='description' id='lblDescription{0}'>
						        {2}
					        </p>
					        <p class='actions'>
						        <a href='#TB_inline?height=180&width=350&inlineId=divConfirmDelete{5}&modal=true' class='thickbox'>Delete</a>
                                <a href='#' class='edit'>Edit</a>
					        </p>
					        <div class='edit_cat_item clearfix' style='display: none;' id='edit{0}'>
						        <p class='cat_name'>
							        <label>Gallery Name</label><br />
							        <input name='' maxlength='40' id='txtName{0}' type='text' class='form_txt' value='{1}' />
						        </p>
						        <p class='description'>
							        <label>Description</label><br />
							        <textarea name='' id='txtDescription{0}' class='form_txt'>{4}</textarea>
						        </p>
						        <p class='actions'>
							        <input name='' type='button' value='Save' class='form_btn' onclick='ajaxEditGallery(""{0}"",txtName{0},txtDescription{0})' />
						        </p>
					        </div>
				        </div><div id='divConfirmDelete{5}' style='display:none;'><div style='padding-bottom:15px;background-color:#FFFFFF;padding-left: 15px;padding-right: 5px;'><p></p><p>Are you sure you want to delete <strong>{1}</strong>?</p><p>Deleting this Gallery will also delete all your photos belonging to it</p><p></p><p style='text-align:right;padding-right: 25px'><input type='button' value='No thanks' class='form_btn2' onclick='javascript:tb_remove();'> <input type='button' class='form_btn2' value='Yes delete' onclick='window.location.href=""/MyPhotoGallery.aspx?d={0}""'></p></div></div>";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        //</div><div id='divConfirmDelete{5}'><div style='display:none;padding-left: 35px;padding-right: 35px;'><p></p><p>Are you sure you want to delete <strong>{1}</strong>?</p><p>Deleting this Gallery will also delete all your photos belonging to it</p><p></p><p style='text-align:right;padding-right: 25px'><input type='button' value='No thanks' class='form_btn2' onclick='javascript:tb_remove();'> <input type='button' class='form_btn2' value='Yes delete' onclick='window.location.href=""/MyPhotoGallery.aspx?d={0}""'></p></div></div>";
        MyGalleriesHTML = sbHTMLList.ToString();

        return sbHTMLList.ToString();

    }

    public void Bind(int Page)
    {
        member = (Member)Session["Member"];

        int NumberOfPhotos = Photo.GetPhotoCountPhotoCollectionID(DefaultPhotoCollectionID);

        photos = Photo.GetPhotoByPhotoCollectionIDWithJoinPager(DefaultPhotoCollectionID, Page, 10);

        PhotoCollection LocalGallery = new PhotoCollection(DefaultPhotoCollectionID);

        GalleryName = LocalGallery.Name + " (" + NumberOfPhotos + ")";
        
        // bit if a hack to get the webphotocollectionID
        photoCollections = member.PhotoCollection;
        for (int i = 0; i < photos.Count; i++)
        {
            PhotoCollection photoCol = GetLocalGalleryByPhotoColletionID(photos[i].PhotoCollectionID);
            photos[i].WebPhotoCollectionID = photoCol.WebPhotoCollectionID;
        }

        
        Pager pager = new Pager("/myphotos/",Page, NumberOfPhotos);
        pager.PageSize = 10;
        pager.MiscParameterString = (DefaultWebPhotoCollectionID != null) ? "pc=" + DefaultWebPhotoCollectionID : string.Empty;
        PagerHTML = pager.ToString();

        PhotoRepeater.DataSource = photos;
        PhotoRepeater.DataBind();
    }

    protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        int IndexerPlus1 = e.Item.ItemIndex + 1;

        Indexer = (IndexerPlus1 < 10) ? "0" + IndexerPlus1.ToString() : IndexerPlus1.ToString();
        FuncDec = "enableSave('" + Indexer + "')";
        FuncCategorySelect = "populateTags('" + Indexer + "');" + FuncDec;
    }

    public void btnNewGallery_Click(object sender, EventArgs e)
    {
        member = (Member)Session["Member"];

        PhotoCollection DefaultGallery = new PhotoCollection();
        DefaultGallery.WebPhotoCollectionID = Next2Friends.Misc.UniqueID.NewWebID();
        DefaultGallery.MemberID = member.MemberID;
        DefaultGallery.DTCreated = DateTime.Now;
        DefaultGallery.Name = txtGalleryName.Text;
        DefaultGallery.Description = txtGalleryDescription.Text;
        DefaultGallery.Save();

        GalleryItem galleryitem = new GalleryItem();
        galleryitem.WebPhotoCollectionID = DefaultGallery.WebPhotoCollectionID;
        galleryitem.Name = DefaultGallery.Name;
        galleryitem.Description = (DefaultGallery.Description.Length > 55) ? DefaultGallery.Description.Substring(0, 53) + ".." : DefaultGallery.Description;

        //DefaultPhotoCollectionID = DefaultGallery.PhotoCollectionID;

        // hack to reload photocollection
        member.PhotoCollection = null;
        GenerateGalleries();


        Bind(1);
    }


    [AjaxPro.AjaxMethod]
    public GalleryItem EditPhotoGallery(string WebPhotoCollectionID, string Name, string Description)
    {
        member = (Member)Session["Member"];

        PhotoCollection photoCollection = GetLocalGalleryByWebPhotoColletionID(WebPhotoCollectionID);
        photoCollection.Name = Name;
        photoCollection.Description = Description;
        photoCollection.Save();

        GalleryItem galleryitem = new GalleryItem();
        galleryitem.WebPhotoCollectionID = WebPhotoCollectionID;
        galleryitem.Name = Name;
        galleryitem.Description = (Description.Length > 75) ? Description = Description.Substring(0, 75) + ".." : Description;

        return galleryitem;
    }


    [AjaxPro.AjaxMethod]
    public void Delete(string WebPhotoID)
    {
        Photo photo = Photo.GetPhotoByWebPhotoIDWithJoin(WebPhotoID);
        photo.Delete();
    }

    public Photo GetLocalPhotoByWebPhotoID(string WebPhotoID)
    {
        if (photos == null)
            photos = Photo.GetPhotoByMemberIDWithJoinNoPager(member.MemberID);

        for (int i = 0; i < photos.Count; i++)
        {
            if (photos[i].WebPhotoID == WebPhotoID)
                return photos[i];
        }

        return null;
    }

    /// <summary>
    /// creates a new photoCollection
    /// </summary>
    public void GetLocalGalleryByWebPhotoColletionID(string Name, string Description)
    {
        member = (Member)Session["Member"];

        PhotoCollection DefaultGallery = new PhotoCollection();
        DefaultGallery.WebPhotoCollectionID = Next2Friends.Misc.UniqueID.NewWebID();
        DefaultGallery.MemberID = member.MemberID;
        DefaultGallery.DTCreated = DateTime.Now;
        DefaultGallery.Name = Name;
        DefaultGallery.Description = Description;
        DefaultGallery.Save();
    }

    public PhotoCollection GetLocalGalleryByWebPhotoColletionID(string WebPhotoCollectionID)
    {
        if (photoCollections == null)
            photoCollections = member.PhotoCollection;

        for (int i = 0; i < photoCollections.Count; i++)
        {
            if (photoCollections[i].WebPhotoCollectionID == WebPhotoCollectionID)
                return photoCollections[i];
        }

        return null;
    }

    public PhotoCollection GetLocalGalleryByPhotoColletionID(int PhotoCollectionID)
    {
        if (photoCollections == null)
            photoCollections = member.PhotoCollection;

        for (int i = 0; i < photoCollections.Count; i++)
        {
            if (photoCollections[i].PhotoCollectionID == PhotoCollectionID)
                return photoCollections[i];
        }

        return null;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        member = (Member)Session["Member"];
        bool Update = false;

        if (member != null)
        {
            for (int i = 0; i < PhotoRepeater.Items.Count; i++)
            {
                HiddenField HiddenFieldWebPhotoID = (HiddenField)PhotoRepeater.Items[i].FindControl("WebPhotoID");
                string WebPhotoID = HiddenFieldWebPhotoID.Value;

                Photo photo = GetLocalPhotoByWebPhotoID(WebPhotoID);

                if (photo != null)
                {
                    if (photo.MemberID == member.MemberID)
                    {

                        CheckBox chbDelete = (CheckBox)PhotoRepeater.Items[i].FindControl("chbDelete");
                        bool DoDelete = chbDelete.Checked;

                        DropDownList drpGallery = (DropDownList)PhotoRepeater.Items[i].FindControl("drpGallery");
                        string NewWebPhotoCollectionID = drpGallery.SelectedValue;
                        PhotoCollection NewCollectionObject = GetLocalGalleryByWebPhotoColletionID(NewWebPhotoCollectionID);
                        
                        if (DoDelete)
                        {
                            photo.Delete();
                        }
                        else
                        {
                            // determine if a gallery move has been requested
                            if (photo.PhotoCollectionID != NewCollectionObject.PhotoCollectionID)
                            {
                                photo.PhotoCollectionID = NewCollectionObject.PhotoCollectionID;
                                Update = true;
                            }

                            HiddenField HiddenFieldRotation = (HiddenField)PhotoRepeater.Items[i].FindControl("Rotation");
                            string Rotation = HiddenFieldRotation.Value;
                            int RotationValue = Int32.Parse(Rotation);
                            HiddenFieldRotation.Value = "0";

                            if (RotationValue != 0)
                            {
                                Photo.RotateGalleryImage(member, WebPhotoID, ASP.global_asax.DiskUserRoot, RotationValue);
                            }


                            TextBox txtCaption = (TextBox)PhotoRepeater.Items[i].FindControl("txtCaption");
                            //DropDownList drpCategories = (DropDownList)PhotoRepeater.Items[i].FindControl("drpCategories");
                            //TextBox txtTags = (TextBox)PhotoRepeater.Items[i].FindControl("txtTags");



                            ////if (photo.CategoryID.ToString() != drpCategories.SelectedValue)
                            //{
                            //    photo.CategoryID = Int32.Parse(drpCategories.SelectedValue);
                            //    Update = true;
                            //}


                            if (photo.Caption != txtCaption.Text)
                            {
                                photo.Caption = txtCaption.Text;
                                Update = true;
                            }

                            if (Update)
                            {
                                photo.Save();
                            }
                        }
                    }
                }
            }
        }

        string strPhotoCollectionID = Request.Params["pc"];

        // if a photocollectionid has been specified then load the new collection
        if (strPhotoCollectionID != null)
        {
            PhotoCollection photoCollection = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(strPhotoCollectionID);
            DefaultPhotoCollectionID = photoCollection.PhotoCollectionID;
        }

        Bind(CurrentPageIndex);
    }

    [AjaxPro.AjaxMethod]
    public GalleryItem SaveSingle(string WebPhotoID, string Caption, bool DoDelete, string NewWebPhotoCollectionID, int RotationValue, string strIndex)
    {
        member = (Member)Session["Member"];

        GalleryItem galleryItem = new GalleryItem();
        galleryItem.Index = strIndex;

        bool Update = false;

        if (member != null)
        {
            Photo photo = Photo.GetPhotoByWebPhotoIDWithJoin(WebPhotoID);

            if (photo != null)
            {
                if (photo.MemberID == member.MemberID)
                {
                    PhotoCollection NewCollectionObject = GetLocalGalleryByWebPhotoColletionID(NewWebPhotoCollectionID);

                    if (DoDelete)
                    {
                        photo.Delete();
                        galleryItem.IsRemoved = true;
                    }
                    else
                    {
                        // determine if a gallery move has been requested
                        if (photo.PhotoCollectionID != NewCollectionObject.PhotoCollectionID)
                        {
                            photo.PhotoCollectionID = NewCollectionObject.PhotoCollectionID;
                            galleryItem.IsRemoved = true;
                            Update = true;
                        }

                        //if (photo.Tags != Tags)
                        //{
                        //    photo.Tags = Tags;
                        //    Update = true;

                        //    // update the tags here
                        //    photo.UpdateTags();

                        //    galleryItem.Tags = photo.Tags;
                        //}

                        if (RotationValue != 0)
                        {
                            Photo.RotateGalleryImage(member, WebPhotoID, ASP.global_asax.DiskUserRoot, RotationValue);
                        }

                        //if (photo.CategoryID != CategoryID)
                        //{
                        //    photo.CategoryID = CategoryID;
                        //    Update = true;
                        //}

                        if (photo.Caption != Caption)
                        {
                            photo.Caption = Caption;
                            Update = true;
                        }

                        if (Update)
                        {
                            photo.Save();
                        }
                    }
                    
                }
            }
        }

        return galleryItem;
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
            Response.Redirect("/signup");
        }

        Master.SkinID = "photo";
        base.OnPreInit(e);
    }

    public class GalleryItem
    {
        public string WebPhotoCollectionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Index { get; set; }
        public string Tags { get; set; }
        public bool IsRemoved { get; set; }

    }
}

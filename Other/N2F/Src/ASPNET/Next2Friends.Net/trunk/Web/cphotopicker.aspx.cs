using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using AjaxPro;
using Next2Friends.Data;
using Next2Friends.Misc;


public partial class AjaxTest : System.Web.UI.Page
{
    public static Size defaultPicureSize = new Size(390, 410);
    string strWebMemberID;

    protected void Page_Load(object sender, EventArgs e)
    {

        AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxTest));

        AjaxPro.Utility.RegisterEnumForAjax(typeof(RotateFlipType));

        display.Attributes.CssStyle.Add("width", (Math.Max(defaultPicureSize.Width, defaultPicureSize.Height)+10).ToString()+"px");

        
        if(Page.Request.QueryString["m"]!=null)
            Session.Add("strWebMemberID",Page.Request.QueryString["m"].ToString());
        

          
    }

    [AjaxPro.AjaxMethod]
    public Bitmap GetPhoto()
    {
        
        if (Session["ImageStore"] != null && Session["Original"] != null)

                return (Bitmap)Session["ImageStore"];

        if (Session["strWebMemberID"] != null)
            strWebMemberID = (string)(Session["strWebMemberID"]);


        string PhotoURL="1", LargePhotoURL;

        try
        {



            if (strWebMemberID != null)
            {
                Member ViewingMember = Member.GetMembersViaWebMemberIDWithFullJoin(strWebMemberID);


                ResourceFile PhotoRes = new ResourceFile(ViewingMember.ProfilePhotoResourceFileID);

                
                PhotoURL = OSRegistry.GetDiskUserDirectory() + PhotoRes.SavePath.Replace("pthmb", "pmed");//PhotoRes.Server + "|" + PhotoRes.Path+"|"+PhotoRes.StorageLocation + PhotoRes.FileName;
                Bitmap photoLarge = (Bitmap)Image.FromFile(PhotoURL);
                Session["Original"] = photoLarge;
                Session["ImageStore"] = photoLarge;
                if (Session["Original"] != null)
                    return photoLarge;


            }
        }
        catch (Exception ex)
        {
            PhotoURL = ex.Message;
        }

     
            Bitmap pic = new Bitmap(defaultPicureSize.Width,defaultPicureSize.Height);

            Graphics g = Graphics.FromImage(pic);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, defaultPicureSize.Width, defaultPicureSize.Height);

            g.DrawRectangle(new Pen(new SolidBrush(Color.Black)),

                0, 0, defaultPicureSize.Width - 1, defaultPicureSize.Height - 1);

           // g.DrawString(PhotoURL, new Font("Courier New", 12), new SolidBrush(Color.Blue), 10, 40);
        
            return pic;
       
       }

    [AjaxPro.AjaxMethod]
    public Bitmap ReloadPhoto()
    {

        if (Session["Original"] == null)

            return GetPhoto();

       Bitmap pic =(Bitmap) Session["Original"];

       Bitmap c = ResizeImage(pic, defaultPicureSize);

       Session.Add("ImageStore", c);

       return c;
  

    }

    [AjaxPro.AjaxMethod]
    public Bitmap RotateFlipPhoto(RotateFlipType rotateFlipType)
    {
        if (Session["ImageStore"] == null)

            return GetPhoto();

        Bitmap pic = (Bitmap)Session["ImageStore"];

        try
        {

            pic.RotateFlip(rotateFlipType);

            Bitmap c = ResizeImage(pic, new Size(335, 370));

            Session["ImageStore"] = c;

        }
        catch (Exception ex)
        {
            return null;
            
        }

        return pic;
    }

    [AjaxPro.AjaxMethod]
    public Bitmap CropPhoto(int[] selection)
    {

        if (HttpContext.Current.Session["ImageStore"] != null &&

            HttpContext.Current.Session["Original"] != null)
        {
            Bitmap cropped;

            try
            {
                Rectangle rect = new Rectangle(selection[0], selection[1], selection[2], selection[3]);

                Bitmap original = (Bitmap)Session["ImageStore"];

                cropped = CropImage(original, rect);

                Session["ImageStore"] = cropped;

                //Photo.ProcessProfilePhotoWithCrop(member, Original, Cropped);
            }
            catch
            {
                return null;
            }

            return cropped;
        }

        return GetPhoto();

    }

    [AjaxPro.AjaxMethod]
    public bool Complete(int[] selection)
    {
        
        if (HttpContext.Current.Session["ImageStore"] != null &&

            HttpContext.Current.Session["Original"] != null)
        {
            Bitmap cropped;

            try
            {
                Rectangle rect = new Rectangle(selection[0], selection[1], selection[2], selection[3]);

                Bitmap original = (Bitmap)Session["ImageStore"];

                cropped = CropImage(original, rect);

                cropped = (Bitmap)Photo.ResizeTo102x102(cropped);

                //Session["ImageStore"] = cropped;

                Member member = (Member)Session["Member"];

                Photo.ProcessProfilePhotoWithCrop(member, original, cropped);
            }
            catch
            {
                return false;
            }

        }

        return true;

    }

    [AjaxPro.AjaxMethod]
    public Bitmap AdjustBrightnessPhoto(int value)
    {
        if (Session["ImageStore"] == null)

            return GetPhoto();

        Bitmap pic = (Bitmap)Session["ImageStore"];

        try
        {

            AdjustBrightnessMatrix(pic, value);

            Session["ImageStore"] = pic;

        }
        catch (Exception ex)
        {
            return null;

        }

        return pic;
    }

    [AjaxPro.AjaxMethod]
    public Bitmap AdjustContrastPhoto(int value)
    {
        if (Session["ImageStore"] == null)

            return GetPhoto();

        Bitmap pic = (Bitmap)Session["ImageStore"];

        try
        {

            AdjustContrastMatrix(pic, value);

            Session["ImageStore"] = pic;

        }
        catch (Exception ex)
        {
            return null;

        }

        return pic;
    }

    [AjaxPro.AjaxMethod]
    public Bitmap ApplyFilter()
    {
        if (Session["ImageStore"] == null)

            return GetPhoto();

        Bitmap pic = (Bitmap)Session["ImageStore"];

        return pic;
    }

    private static float[][] Multiply(float[][] f1, float[][] f2)
    {
        float[][] X = new float[5][];
        for (int d = 0; d < 5; d++)
            X[d] = new float[5];

        int size = 5;
        float[] column = new float[5];
        for (int j = 0; j < 5; j++)
        {
            for (int k = 0; k < 5; k++)
            {
                column[k] = f1[k][j];
            }
            for (int i = 0; i < 5; i++)
            {
                float[] row = f2[i];
                float s = 0;
                for (int k = 0; k < size; k++)
                {
                    s += row[k] * column[k];
                }
                X[i][j] = s;
            }
        }

        return X;
    }

    [AjaxPro.AjaxMethod]
    public Bitmap AdjustPhoto(float[] values)
    {
        if (Session["ImageStore"] == null)

            return GetPhoto();
      
        Bitmap pic = (Bitmap)Session["ImageStore"];

        try
        {

            AdjustMatrixs(pic, values);

            Session["ImageStore"] = pic;

        }
        catch (Exception ex)
        {
            return null;

        }

        return pic;
    }

    public static void AdjustMatrixs(Bitmap img, float[] value)
    {

        if (value.Length == 0) 

            return;


        float sb = (float)value[0] / 255F;

        float[][] brMatrix =

                  { 

                        new float[] {1,  0,  0,  0, 0},

                        new float[] {0,  1,  0,  0, 0},

                        new float[] {0,  0,  1,  0, 0},

                        new float[] {0,  0,  0,  1, 0},

                        new float[] {sb, sb, sb, 1, 1}

                  };
        float co = 1F - (float)value[1] / 255F;

        float[][] coMatrix =

                  { 

                        new float[] {co,  0,  0,  0, 0},

                        new float[] {0,  co,  0,  0, 0},

                        new float[] {0,  0,  co,  0, 0},

                        new float[] {0,  0,  0, 1, 0},

                        new float[] {0,  0,  0,  0, 1}

                  };

        float[][] colorMatrixElements=Multiply(brMatrix,coMatrix);



        ColorMatrix cm = new ColorMatrix(colorMatrixElements);

        ImageAttributes imgattr = new ImageAttributes();

        Rectangle rc = new Rectangle(0, 0, img.Width, img.Height);

        Graphics g = Graphics.FromImage(img);

        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        imgattr.SetColorMatrix(cm);

        g.DrawImage(img, rc, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgattr);



        //Clean everything up

        imgattr.Dispose();

        g.Dispose();

    }

    public static void AdjustBrightnessMatrix(Bitmap img, float value)

        {

            if (value == 0) // No change, so just return

                return;

 

            float sb = (float)value / 255F;

            float[][] colorMatrixElements =

                  { 

                        new float[] {1,  0,  0,  0, 0},

                        new float[] {0,  1,  0,  0, 0},

                        new float[] {0,  0,  1,  0, 0},

                        new float[] {0,  0,  0,  1, 0},

                        new float[] {sb, sb, sb, 1, 1}

                  };

 

            ColorMatrix cm = new ColorMatrix(colorMatrixElements);

            ImageAttributes imgattr = new ImageAttributes();

            Rectangle rc = new Rectangle(0, 0, img.Width, img.Height);

            Graphics g = Graphics.FromImage(img);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            imgattr.SetColorMatrix(cm);

            g.DrawImage(img, rc, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgattr);

            

            //Clean everything up

            imgattr.Dispose();

            g.Dispose();

        }

    public static void AdjustContrastMatrix(Bitmap img, float value)
    {

        if (value == 0) // No change, so just return

            return;



        float co = 1F-(float)value / 255F;

        float[][] colorMatrixElements =

                  { 

                        new float[] {co,  0,  0,  0, 0},

                        new float[] {0,  co,  0,  0, 0},

                        new float[] {0,  0,  co,  0, 0},

                        new float[] {0,  0,  0, 1, 0},

                        new float[] {0,  0,  0,  0, 1}

                  };



        ColorMatrix cm = new ColorMatrix(colorMatrixElements);

        ImageAttributes imgattr = new ImageAttributes();

        Rectangle rc = new Rectangle(0, 0, img.Width, img.Height);

        Graphics g = Graphics.FromImage(img);

        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        imgattr.SetColorMatrix(cm);

        g.DrawImage(img, rc, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgattr);

        imgattr.Dispose();

        g.Dispose();

    }

    public static Bitmap ResizeImage(Bitmap imgSource, Size sSize)
    {
        int sourceWidth = imgSource.Width;
        int sourceHeight = imgSource.Height;
        int sourceX = 0;
        int sourceY = 0, destX = 0, destY = 0;
        double dPercent = 0, dPercentW = 0, dPercentH = 0;

        dPercentW = (double)sSize.Width / (double)sourceWidth;
        dPercentH = (double)sSize.Height / (double)sourceHeight;
        if (dPercentH < dPercentW)
        {
            dPercent = dPercentH;
            destX = Convert.ToInt32((sSize.Width - (sourceWidth * dPercent)) / 2);
        }
        else
        {
            dPercent = dPercentW;
            destY = Convert.ToInt32((sSize.Height - (sourceHeight * dPercent)) / 2);
        }

        int destWidth = (int)Math.Round(sourceWidth * dPercent);
        int destHeight = (int)Math.Round((int)sourceHeight * dPercent);

        Bitmap bmPhoto = new Bitmap(destWidth, destHeight);//, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);
        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        grPhoto.Clear(Color.Red);
        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
        grPhoto.DrawImage(imgSource, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
        grPhoto.Dispose();
        return bmPhoto;

    }

    private static Bitmap CropImage(Bitmap imgSource, Rectangle area)
    {
        Bitmap bmpCrop;

        try
        {

            bmpCrop = imgSource.Clone(area, imgSource.PixelFormat);

        }
        catch
        {

            throw new Exception("Crop Exception");

        }

        return bmpCrop;
    }

    protected void Upload_Click(object sender, EventArgs e)
    {

        if (FileUpload1.HasFile)
        {
            try
            {

                Bitmap pic =new Bitmap(Image.FromStream(FileUpload1.FileContent));

                Session["Original"] = pic;

                Bitmap c = ResizeImage(pic, defaultPicureSize);

                HttpContext.Current.Session.Add("ImageStore", c);

            }
            catch
            {
                HttpContext.Current.Session["ImageStore"] = null;

                HttpContext.Current.Session["Original"] = null;
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        AdjustPhoto(new float[]{123, 143});
    }
}

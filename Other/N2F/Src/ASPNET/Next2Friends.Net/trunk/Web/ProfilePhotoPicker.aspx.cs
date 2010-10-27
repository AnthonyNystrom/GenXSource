using System;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Next2Friends.Data;

public partial class ProfilePhotoPicker : System.Web.UI.Page
{
    public string ImageURL = string.Empty;
    public string FinalImageURL = string.Empty;
    public int ImageWidth = 640;
    public int ImageHeight = 480;
    public string Root = @"\\www\user\tmp\";
    public ImageStore imageStore;
    public bool ShowDone = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CreateStore();
        }
    }

    public void CreateStore()
    {
        imageStore = new ImageStore();
        imageStore.PreName = Guid.NewGuid().ToString();
        imageStore.PostName = Guid.NewGuid().ToString();
        imageStore.PrePath = @"tmp\" + imageStore.PreName + ".jpg";
        imageStore.PostPath = Root + imageStore.PostName + ".jpg";

        Session["ImageStore"] = imageStore;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        CreateStore();

        if (FileUpload1.HasFile)
        {
            Image img = new Bitmap(FileUpload1.FileContent);
            img = Photo.ResizeTo640x480(img);
            Photo.SaveToDisk(img,imageStore.PrePath);
            ImageURL = "user/tmp/" + imageStore.PreName + ".jpg";
            ImageWidth = img.Width;
            ImageHeight = img.Height;
        }
    }
    
    protected void btnDone_Click(object sender, EventArgs e)
    {
        imageStore = (ImageStore)Session["ImageStore"];

        if (imageStore != null)
        {
            int Width = Int32.Parse(width.Value);
            int Height = Int32.Parse(height.Value);
            int X = Int32.Parse(x.Value);
            int Y = Int32.Parse(y.Value);

            Image Original = new Bitmap(@"\\www\user\"+imageStore.PrePath);
            Rectangle rect = new Rectangle(X, Y, Width, Height);
            Image Cropped = cropImage(Original, rect);
            Cropped = resizeImage(Cropped, new Size(102, 102));
            Cropped.Save(imageStore.PostPath);
            FinalImageURL = "user/tmp/" + imageStore.PreName + ".jpg";

            width.Value = "0";
            height.Value = "0";
            x.Value = "0";
            y.Value = "0";

            ShowDone = true;
            Member member = new Member(1);

            Photo.ProcessProfilePhotoWithCrop(member, Original, Cropped);
        }
        else
        {
            throw new Exception("Timed out");
        }
    }

    private static Image cropImage(Image img, Rectangle cropArea)
    {
        Bitmap bmpImage = new Bitmap(img);
        Bitmap bmpCrop = bmpImage.Clone(cropArea,
        bmpImage.PixelFormat);
        return (Image)(bmpCrop);
    }

    private static Image resizeImage(Image imgToResize, Size size)
    {
        int sourceWidth = imgToResize.Width;
        int sourceHeight = imgToResize.Height;

        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;

        nPercentW = ((float)size.Width / (float)sourceWidth);
        nPercentH = ((float)size.Height / (float)sourceHeight);

        if (nPercentH < nPercentW)
            nPercent = nPercentH;
        else
            nPercent = nPercentW;

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap b = new Bitmap(destWidth, destHeight);
        Graphics g = Graphics.FromImage((Image)b);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        g.Dispose();

        return (Image)b;
    }
}



public class ImageStore
{
    public string PostName { get; set; }
    public string PreName { get; set; }
    public string PostPath { get; set; }
    public string PrePath { get; set; }
}

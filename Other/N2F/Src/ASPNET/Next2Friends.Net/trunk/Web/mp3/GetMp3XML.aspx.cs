using System;
using System.IO;
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

public partial class mp3_GetMp3XML : System.Web.UI.Page
{
    public string XML = "<playlist><playlist>";

    protected void Page_Load(object sender, EventArgs e)
    {
        string Nickname = Request.Params["n"];

        string FilePath = @"\\www\user\"+Nickname+@"\mp3\playlist.xml";

        XML = FilePath;

        if (File.Exists(FilePath))
        {
            StreamReader sr = File.OpenText(FilePath);
            XML = sr.ReadToEnd();
        }
    }
}

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

public enum InstructionsType {BlackBerryLive, BlackBerrySocial,BlackBerryBold, Symbian, None }

public partial class Download : System.Web.UI.Page
{
    public string PhoneList;
    public bool IsDownload = false;
    public string Disabled = "disabled='disabled'";
    public string DownloadList = "";
    public string QRImageURL = "";
    public string MobileDownloadPage;
    public MobilePhone SelectedMobilephone;
    public string LiveText = string.Empty;
    public string SocialText = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string SetHelperText(InstructionsType iType)
    {
        if (iType == InstructionsType.BlackBerrySocial)
        {
            return @"<strong>Next2Friends Social for BlackBerry Pearl, Curve and Bold Supports OS: 4.5 (Excluding Bold) and above</strong><br/><br/><p>To find out which OS you are running, go to Options, then About. You will see the version starting with a 'v'.<br/><br/>If your BlackBerry doesn’t have the required OS installed, click <a href='http://na.blackberry.com/eng/support/downloads/download_sites.jsp'>here</a> to locate an upgrade.<br/><br/>Data Plan Support: GPRS, EDGE and WIFI.<br/><br/> <strong>If you plan to use your carrier data plan, you must set your Carrier APN to the correct values (CDMA Networks such as Verizon/Sprint do not require an APN value). You can find these values on your carrier's website. You can find the APN settings for your BlackBerry in Options, Advanced Options, TCP.</strong> <br/><br/> When your download begins, select 'Set Application Permissions' and set 'Allow' for each category. <br/><br/>If you choose to install using the BlackBerry Desktop Manager, you will need to go to 'Application Permissions' and set there after installation.<p>";
        }
        else if (iType == InstructionsType.BlackBerryLive)
        {
            return "<strong>Next2Friends Live for BlackBerry Pearl and Curve Supports OS: 4.5 and above</strong><br/><br/>To find out which OS you are running, go to Options, then About. You will see the version starting with a 'v'.<br/><br/>If your BlackBerry doesn’t have the required OS installed, click <a href='http://na.blackberry.com/eng/support/downloads/download_sites.jsp'>here</a> to locate an upgrade.<br/><br/>Data Plan Support: EDGE, 3G and WIFI.<br /><br />You must ensure that your video recording application (Next2Friends uses this location for buffer) is set to:<br /><br />media card/blackberry/videos<br /><br />or also known as:<br /><br />sdcard/blackberry/videos<br /><br />It should be set to the above by default, please ensure it is on your BlackBerry.<br /><br />If you plan to use your carrier data plan, you must set your Carrier APN to the correct values (CDMA Networks such as Verizon/Sprint do not require an APN value).You can find these values on your carrier's website. You can find the APN settings for your BlackBerry in Options, Advanced Options, TCP.<br /><br />When your download begins, select 'Set Application Permissions' and set 'Allow' for each category.<br /><br />If you choose to install using the BlackBerry Desktop Manager, you will need to go to 'Application Permissions' and set after installation. (Found in Options)<br /><br />Usage: After opening the Next2Friends Live Application from your BlackBerry, go to settings, enter your nickname and password. If you would like to use your WIFI connection, then make sure WIFI is selected. If you choose not to use your WIFI connection, then you must uncheck the WIFI checkbox. Now save and exit from settings.<br /><br />Select Broadcast, the Preview will open and the application will start broadcasting. Because Next2Friends Live for BlackBerry uses a buffer, It can take up to 5+ seconds before the stream appears on the Next2Friends website. To end your stream select the back button. You may now see a screen saying it is completing, this is the buffer clearing the data by pushing the rest of the broadcast to th site. You have just streamed with Next2Friends Live.<br /><br />On some Phones you will be presented with a screen to accept a change to the security timers, please accept with 'Yes'.<br /><br />Important note: Because the BlackBerry video system relies on a media card and is checking for active space so that the device can locally save the video upon stream complete. The length of your stream will be determined by how much space is available on your media card. By default, the BlackBerry devices will save all video locally. You can delete them manually from your media card to gain more space for streaming. The data is always streamed to Next2Friends and saved, but the device itself must know it has enough space to save any video as it is functioning.";
        }
        else if (iType == InstructionsType.Symbian)
        {
            return "<strong>Next2Friends Social for S40 and S60</strong><br/><br/> After installation, go to the App. Mgr, scroll to Next2Friends Social, select and choose 'Suite Settings' Set each item to 'Always Allow'";
        }
		else if(iType == InstructionsType.BlackBerryBold)
		{
			return @"<strong>Next2Friends Live for BlackBerry BOLD</strong><br /><br />Data Plan Support: EDGE, 3G and WIFI.<br /><br />You must ensure that your video recording application (Next2Friends uses this location for buffer) is set to:<br /><br />media card/blackberry/videos<br /><br />or also known as:<br /><br />sdcard/blackberry/videos<br /><br />It should be set to the above by default, please ensure it is on your BlackBerry.<br /><br />If you plan to use your carrier data plan, you must set your Carrier APN to the correct values (CDMA Networks such as Verizon/Sprint do not require an APN value).You can find these values on your carrier's website. You can find the APN settings for your BlackBerry in Options, Advanced Options, TCP.<br /><br />When your download begins, select 'Set Application Permissions' and set 'Allow' for each category.<br /><br />If you choose to install using the BlackBerry Desktop Manager, you will need to go to 'Application Permissions' and set after installation. (Found in Options)<br /><br />Usage: After opening the Next2Friends Live Application from your BlackBerry, go to settings, enter your nickname and password. If you would like to use your WIFI connection, then make sure WIFI is selected. If you choose not to use your WIFI connection, then you must uncheck the WIFI checkbox. Now save and exit from settings.<br /><br />Select Broadcast, the Preview will open and the application will start broadcasting. Because Next2Friends Live for BlackBerry uses a buffer, It can take up to 5+ seconds before the stream appears on the Next2Friends website. To end your stream select the back button. You may now see a screen saying it is completing, this is the buffer clearing the data by pushing the rest of the broadcast to th site. You have just streamed with Next2Friends Live.<br /><br />On some Phones you will be presented with a screen to accept a change to the security timers, please accept with 'Yes'.<br /><br />Important note: Because the BlackBerry video system relies on a media card and is checking for active space so that the device can locally save the video upon stream complete. The length of your stream will be determined by how much space is available on your media card. By default, the BlackBerry devices will save all video locally. You can delete them manually from your media card to gain more space for streaming. The data is always streamed to Next2Friends and saved, but the device itself must know it has enough space to save any video as it is functioning.";				
		}

        return string.Empty;
    }

    protected void SetManu(object sender, EventArgs e)
    {
        MobilePhone SelectedMobilephone = new MobilePhone();

        drpModel.Enabled = true;

        List<MobilePhone> Filtered = SelectedMobilephone.FilterManufacturer(drpManu.SelectedValue);

        drpModel.Items.Clear();
        drpModel.Items.Add(new ListItem("select"));

        for (int i = 0; i < Filtered.Count; i++)
        {
            drpModel.Items.Add(new ListItem(Filtered[i].Model,Filtered[i].ID.ToString()));
        }

        if (drpManu.Items[0].Text == "select")
        {
            drpManu.Items.RemoveAt(0);
        }
    }

    protected void SetModel(object sender, EventArgs e)
    {
        if (drpModel.Items[0].Text == "select")
        {
            drpModel.Items.RemoveAt(0);
        }

        IsDownload = true;

        // get the model
        SelectedMobilephone = MobilePhone.GetMobilePhoneByID(Int32.Parse(drpModel.SelectedValue));
        // determine the OS runtime
        //


        string JavaDownloads = @"<span class='downloadlink'>Download Social<a style='font-size:smaller' href='http://www.getn2f.com/apps/J2MESOCIAL/Next2FriendsSocial.zip'> here </a><small>Social v1.0 (245k)</small></span>
                                <br/><a onclick='socialHelpPopup();return false;' style='cursor:pointer'>Important Instructions</a><br/><br/>";
	

        if (SelectedMobilephone.Runtime == RunTimeOS.Symbian)
        {
            SocialText = SetHelperText(InstructionsType.Symbian);

            MobileDownloadPage = "http://www.getn2f.com/4";

            DownloadList = @"<span class='downloadlink'>Download Live <a style='font-size:smaller' href='http://www.getn2f.com/5'>here </a><small>Live v1.0 (455k)</small></span><br />
                              ";
            DownloadList += JavaDownloads;
            QRImageURL = "/images/qrcode/4.gif";
        }
        else if (SelectedMobilephone.Runtime == RunTimeOS.J2ME)
        {
            SocialText = SetHelperText(InstructionsType.Symbian);

            MobileDownloadPage = "http://www.getn2f.com/1";

            DownloadList = @"<span class='downloadlink' style='color:#cccccc;background: url(images/link-arrow-grey.gif) left 3px no-repeat'>Download Live <small>Currently unsupported</small></span><br />";
            
            DownloadList += JavaDownloads;
            QRImageURL = "/images/qrcode/1.gif";
        }
        else if (SelectedMobilephone.Runtime == RunTimeOS.BlackJackII)
        {
            MobileDownloadPage = "http://www.getn2f.com/9";

            DownloadList = @"<span class='downloadlink'>Download Live <a style='font-size:smaller' href='http://www.getn2f.com/10'>here </a><small>Live v1.0 (2.3mb)</small></span><br />
                            <span class='downloadlink'>Download Social <a style='font-size:smaller' href='http://www.getn2f.com/11'>here </a><small>Social v1.0 (2.3mb)</small></span><br />";

            QRImageURL = "/images/qrcode/9.gif";
        }
        else if (SelectedMobilephone.Runtime == RunTimeOS.MotoQ)
        {
            MobileDownloadPage = "http://www.getn2f.com/6";

            DownloadList = @"<span class='downloadlink'>Download Live <a style='font-size:smaller' href='http://www.getn2f.com/7'>here </a><small>Live v1.0 (2.3mb)</small></span><br />
                            <span class='downloadlink'>Download Social <a style='font-size:smaller' href='http://www.getn2f.com/8'>here </a><small>Social v1.0 (2.3mb)</small></span><br />";

            QRImageURL = "/images/qrcode/6.gif";
        }
        else if (SelectedMobilephone.Runtime == RunTimeOS.BlackBerryCurve)
        {
            LiveText = SetHelperText(InstructionsType.BlackBerryLive);
            SocialText = SetHelperText(InstructionsType.BlackBerrySocial);

            MobileDownloadPage = "http://www.getn2f.com/12";

            DownloadList = @"<span class='downloadlink'>Download Live <a style='font-size:smaller' href='http://www.getn2f.com/apps/BBCurveLIVE/N2FLive.zip'>here </a><small>Supports OS:<br/>4.5 and above</small></span><br />
                             <p style='text-align:default;'><a onclick='liveHelpPopup();return false;' style='cursor:pointer'>Important Instructions</a></p>
                            <span class='downloadlink'>Download Social <a style='font-size:smaller' href='http://www.getn2f.com/apps/BBCurveSOCIAL/Next2FriendsSocial.zip'>here </a><small>Supports OS:<br/>4.5 and above</small></span><br />
                            <p style='text-align:default;'><a onclick='socialHelpPopup();return false;' style='cursor:pointer'>Important Instructions</a></p>";

            QRImageURL = "/images/qrcode/12.gif";
        }
        else if (SelectedMobilephone.Runtime == RunTimeOS.BlackBerryPearl)
        {
            LiveText = SetHelperText(InstructionsType.BlackBerryLive);
            SocialText = SetHelperText(InstructionsType.BlackBerrySocial);

            MobileDownloadPage = "http://www.getn2f.com/15";

            DownloadList = @"<span class='downloadlink'>Download Live <a style='font-size:smaller' href='http://www.getn2f.com/apps/BBPearlLIVE/N2FLive.zip'>here </a><small>Supports OS:<br/>4.5 and above</small></span><br />
                            <p style='text-align:default;'><a onclick='liveHelpPopup();return false;' style='cursor:pointer'>Important Instructions</a></p>
                            <span class='downloadlink'>Download Social <a style='font-size:smaller' href='http://www.getn2f.com/apps/BBPearlSOCIAL/Next2FriendsSocial.zip'>here </a><small>Supports OS:<br/>4.5 and above</small></span><br />
                            <p style='text-align:default;'><a onclick='socialHelpPopup();return false;' style='cursor:pointer'>Important Instructions</a></p>";

            QRImageURL = "/images/qrcode/15.gif";
        }
        else if (SelectedMobilephone.Runtime == RunTimeOS.BlackBerryCurveNoWifi)
        {
            LiveText = SetHelperText(InstructionsType.BlackBerryLive);
            SocialText = SetHelperText(InstructionsType.BlackBerrySocial);

            MobileDownloadPage = "http://www.getn2f.com/18";

            DownloadList = @"<span class='downloadlink'>Download Live <a style='font-size:smaller' href='http://www.getn2f.com/apps/BBCurveLIVE/N2FLive.zip'>here </a><small>Supports OS:<br/>4.5 and above</small></span><br />
                             <p style='text-align:default;'><a onclick='liveHelpPopup();return false;' style='cursor:pointer'>Important Instructions</a></p>
                            <span class='downloadlink'>Download Social <a style='font-size:smaller' href='http://www.getn2f.com/apps/8320nWIFIDESK/Next2FriendsSocial.zip'>here </a><small>Supports OS:<br/>4.5 and above</small></span><br />
                            <p style='text-align:default;'><a onclick='socialHelpPopup();return false;' style='cursor:pointer'>Important Instructions</a></p>";

            QRImageURL = "/images/qrcode/18.gif";
        }
        else if (SelectedMobilephone.Runtime == RunTimeOS.BlackBerryPearlNoWifi)
        {
            LiveText = SetHelperText(InstructionsType.BlackBerryLive);
            SocialText = SetHelperText(InstructionsType.BlackBerrySocial);

            MobileDownloadPage = "http://www.getn2f.com/19";

            DownloadList = @"<span class='downloadlink'>Download Live <a style='font-size:smaller' href='http://www.getn2f.com/apps/BBPearlLIVE/N2FLive.zip'>here </a><small>Supports OS:<br/>4.5 and above</small></span><br />
                            <p style='text-align:default;'><a onclick='liveHelpPopup();return false;' style='cursor:pointer'>Important Instructions</a></p>
                            <span class='downloadlink'>Download Social <a style='font-size:smaller' href='http://www.getn2f.com/apps/8120nWIFIDESK/Next2FriendsSocial.zip'>here </a><small>Supports OS:<br/>4.5 and above</small></span><br />
                            <p style='text-align:default;'><a onclick='socialHelpPopup();return false;' style='cursor:pointer'>Important Instructions</a></p>";

            QRImageURL = "/images/qrcode/19.gif";
        }
		else if (SelectedMobilephone.Runtime == RunTimeOS.BlackBerryBold)
        {
            LiveText = SetHelperText(InstructionsType.BlackBerryBold);
            SocialText = SetHelperText(InstructionsType.BlackBerrySocial);

            MobileDownloadPage = "http://www.getn2f.com/20";

            DownloadList = @"<span class='downloadlink'>Download Live <a style='font-size:smaller' href='http://www.getn2f.com/apps/9000desk/N2FLive.zip'>here </a></span><br />
                            <p style='text-align:default;'><a onclick='liveHelpPopup();return false;' style='cursor:pointer'><br/>Important Instructions</a></p>
                            <span class='downloadlink'>Download Social <a style='font-size:smaller' href='http://www.getn2f.com/apps/9000desk/Next2FriendsSocial.zip'>here </a></span><br />
                            <p style='text-align:default;'><a onclick='socialHelpPopup();return false;' style='cursor:pointer'><br/>Important Instructions</a></p>";

            QRImageURL = "/images/qrcode/20.gif";
        }
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        Member member = (Member)Session["Member"];

        if (member == null)
        {
            Response.Redirect("/signup");
        }

        if (Page.Request.Url.AbsolutePath.ToLower().EndsWith(".aspx"))
        {
            HTTPResponse.PermamentlyMoved301(Context, "/download");
        }

        Master.SkinID = "domore";
        base.OnPreInit(e);
    }
}

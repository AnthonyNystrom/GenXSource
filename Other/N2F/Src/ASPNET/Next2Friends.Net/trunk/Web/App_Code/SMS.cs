using System;
using System.Net;
using System.IO;
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
using Next2Friends.Data;

/// <summary>
/// Summary description for SMS
/// </summary>
public class SMS
{
    public SMS()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void SignupDownloadLink(Member Member)
    {


        //string SMSMessage = "Here is the download page for your " + phone.Manufacturer + " " + phone.Model;

        //if (phone.Runtime == RunTimeOS.J2ME)
        //{
        //    SMSMessage += "http://getn2f.com/2";
        //}
        //else if (phone.Runtime == RunTimeOS.Symbian)
        //{
        //    SMSMessage += "http://getn2f.com/1";
        //}

        //SendSMS_txtLocal(false, "Next2Friends", SMSMessage, PhoneNumber, string.Empty);
    }

    public static string SendSMS_txtLocal(bool Test, string From, string Message, string SendTo, string URL)
    {
        // Send a message using the txtLocal transport                                                                                                                                                                                                                                                                                                                                                                                       
        const string TransportURL = "http://www.txtlocal.com/sendsmspost.php";
        const string TransportUserName = "lawrence.botley@gmail.com";
        const string TransportPassword = "saphire1";
        const bool TransportVerbose = true;
        string strPost;

        // Build POST String                                                                                                                                                                                                                                                                                                                                                                                                                 
        strPost = "uname=" + TransportUserName + "&pword=" + TransportPassword + "&message=" + Message + "&from=" + From + "&selectednums=" + SendTo;

        if (URL != "")
        {
            strPost += "&url=" + URL;
        }

        if (Test == true)
        {
            strPost += "&test=1";
        }

        if (TransportVerbose == true)
        {
            strPost += "&info=1";
        }

        // Create POST                                                                                                                                                                                                                                                                                                                                                                                                                       
        WebRequest request = WebRequest.Create(TransportURL);
        request.Method = "POST";

        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(strPost);
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = byteArray.Length;

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        // Get the response.                                                                                                                                                                                                                                                                                                                                                                                                                 
        WebResponse response = request.GetResponse();
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();

        // Clean up the streams.                                                                                                                                                                                                                                                                                                                                                                                                             
        reader.Close();
        dataStream.Close();
        response.Close();

        // Return result to calling function                                                                                                                                                                                                                                                                                                                                                                                                 
        if (responseFromServer.Length > 0)
        {
            return responseFromServer;
        }
        else
        {
            return ((HttpWebResponse)response).StatusDescription;
        }
    }

}

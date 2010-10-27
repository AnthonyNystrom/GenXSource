using System;
using System.IO;
using System.Web;
using Next2Friends.Data;

public class FLVStreaming : IHttpHandler
{

    // FLV header
    private static readonly byte[] _flvheader = HexToByte("464C5601010000000900000009");

    public FLVStreaming()
    {
    }

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            int pos;
            int length;

            int StartIndex = context.Request.Url.AbsoluteUri.IndexOf("?");



            string WebVideoID = context.Request.Url.AbsoluteUri.Substring(StartIndex + 1);

            Video video = Video.GetVideoByWebVideoIDWithJoin(WebVideoID);

            if (video != null)
            {
                // Check start parameter if present
               // string filename = @"\\www\user\" + video.VideoResourceFile.SavePath;
                string filename = @"C:\Documents and Settings\Admin\My Documents\Visual Studio 2008\Projects\Next2Friends.root\Next2Friends\Web\v.flv";

                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

                string qs = context.Request.Params["start"];

                qs = "200000";

                if (string.IsNullOrEmpty(qs))
                {
                    pos = 0;
                    length = Convert.ToInt32(fs.Length);
                }
                else
                {
                    pos = Convert.ToInt32(qs);
                    length = Convert.ToInt32(fs.Length - pos) + _flvheader.Length;
                    //length = Convert.ToInt32(fs.Length - pos);
                }

                // Add HTTP header stuff: cache, content type and length        
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetLastModified(DateTime.Now);

                context.Response.AppendHeader("Content-Type", "video/x-flv");
                context.Response.AppendHeader("Content-Length", length.ToString());

                // Append FLV header when sending partial file
                if (pos > 0)
                {
                    context.Response.OutputStream.Write(_flvheader, 0, _flvheader.Length);
                    fs.Position = pos;
                }

                // Read buffer and write stream to the response stream
                const int buffersize = 16384;
                byte[] buffer = new byte[buffersize];

                int count = fs.Read(buffer, 0, buffersize);
                while (count > 0)
                {
                    if (context.Response.IsClientConnected)
                    {
                        context.Response.OutputStream.Write(buffer, 0, count);
                        count = fs.Read(buffer, 0, buffersize);
                    }
                    else
                    {
                        count = -1;
                    }
                }
            }
            else
            {
                // invalid video file
            }
            
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }

    public bool IsReusable
    {
        get { return true; }
    }

    private static byte[] HexToByte(string hexString)
    {
        byte[] returnBytes = new byte[hexString.Length / 2];
        for (int i = 0; i < returnBytes.Length; i++)
            returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        return returnBytes;
    }

}
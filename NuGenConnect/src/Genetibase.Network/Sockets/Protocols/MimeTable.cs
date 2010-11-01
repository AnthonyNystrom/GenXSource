using System;
using System.Collections.Generic;
using System.IO;

using Genetibase.Network.Sockets;

namespace Genetibase.Network.Sockets.Protocols
{
	public class MimeTable: IDisposable
	{		
	  private EventHandler _OnBuildCache;
    // format:
    //   Key: extension
    //   Value: Content type
    private SortedList<string, string> mBackend = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
	  protected virtual void BuildDefaultCache()
	  {
      FillMimeTable(mBackend);
	  }
	  
	  void IDisposable.Dispose()
	  {
      mBackend.Clear();
	  }

    private static void FillMimeTable(SortedList<string, string> AMIMEList)
	  {
	    if (AMIMEList == null)
	    {
	      return;
	    }
	    if (AMIMEList.Count > 0)
	    {
	      return;
	    }
      // Audio
      AMIMEList.Add(".aiff", "audio/x-aiff");
      AMIMEList.Add(".au", "audio/basic");
      AMIMEList.Add(".mid", "midi/mid");
      AMIMEList.Add(".mp3", "audio/x-mpg");
      AMIMEList.Add(".m3u", "audio/mpegurl");
      AMIMEList.Add(".qcp", "audio/vnd.qcelp");
      AMIMEList.Add(".ra", "audio/x-realaudio");
      AMIMEList.Add(".wav", "audio/x-wav");
      AMIMEList.Add(".gsm", "audio/x-gsm");
      AMIMEList.Add(".wax", "audio/x-ms-wax");
      AMIMEList.Add(".wma", "audio/x-ms-wma");
      AMIMEList.Add(".ram", "audio/x-pn-realaudio");
      AMIMEList.Add(".mjf", "audio/x-vnd.AudioExplosion.MjuiceMediaFile");
      // Image
      AMIMEList.Add(".bmp", "image/bmp");
      AMIMEList.Add(".gif", "image/gif");
      AMIMEList.Add(".jpg", "image/jpeg");
      AMIMEList.Add(".jpeg", "image/jpeg");
      AMIMEList.Add(".jpe", "image/jpeg");
      AMIMEList.Add(".pict", "image/x-pict");
      AMIMEList.Add(".png", "image/x-png");
      AMIMEList.Add(".svg", "image/svg-xml");
      AMIMEList.Add(".tif", "image/x-tiff");
      AMIMEList.Add(".tiff", "image/x-tiff");
      AMIMEList.Add(".rf", "image/vnd.rn-realflash");
      AMIMEList.Add(".rp", "image/vnd.rn-realpix");
      AMIMEList.Add(".ico", "image/x-icon");
      AMIMEList.Add(".art", "image/x-jg");
      AMIMEList.Add(".pntg", "image/x-macpaint");
      AMIMEList.Add(".qtif", "image/x-quicktime");
      AMIMEList.Add(".sgi", "image/x-sgi");
      AMIMEList.Add(".targa", "image/x-targa");
      AMIMEList.Add(".psd", "image/x-psd");
      AMIMEList.Add(".pnm", "image/x-portable-anymap");
      AMIMEList.Add(".pbm", "image/x-portable-bitmap");
      AMIMEList.Add(".pgm", "image/x-portable-graymap");
      AMIMEList.Add(".ppm", "image/x-portable-pixmap");
      AMIMEList.Add(".rgb", "image/x-rgb");
      AMIMEList.Add(".xbm", "image/x-xbitmap");
      AMIMEList.Add(".xpm", "image/x-xpixmap");
      AMIMEList.Add(".xwd", "image/x-xwindowdump");
      // Text
      AMIMEList.Add(".323", "text/h323");
      AMIMEList.Add(".xml", "text/xml");
      AMIMEList.Add(".uls", "text/iuls");
      AMIMEList.Add(".txt", "text/plain");
      AMIMEList.Add(".rtx", "text/richtext");
      AMIMEList.Add(".wsc", "text/scriptlet");
      AMIMEList.Add(".rt", "text/vnd.rn-realtext");
      AMIMEList.Add(".htt", "text/webviewhtml");
      AMIMEList.Add(".htc", "text/x-component");
      AMIMEList.Add(".vcf", "text/x-vcard");
      // Video
      AMIMEList.Add(".avi", "video/x-msvideo");
      AMIMEList.Add(".flc", "video/flc");
      AMIMEList.Add(".mpeg", "video/x-mpeg2a");
      AMIMEList.Add(".mpg", "video/x-mpeg2a");
      AMIMEList.Add(".mov", "video/quicktime");
      AMIMEList.Add(".rv", "video/vnd.rn-realvideo");
      AMIMEList.Add(".ivf", "video/x-ivf");
      AMIMEList.Add(".wm", "video/x-ms-wm");
      AMIMEList.Add(".wmp", "video/x-ms-wmp");
      AMIMEList.Add(".wmv", "video/x-ms-wmv");
      AMIMEList.Add(".wmx", "video/x-ms/wmx");
      AMIMEList.Add(".wvx", "video/x-ms-wvx");
      AMIMEList.Add(".rms", "video/vnd.rn-realvideo-secure");
      AMIMEList.Add(".asx", "video/x-ms-asf-plugin");
      AMIMEList.Add(".movie", "video/x-sgi-movie");
      // Application
      AMIMEList.Add(".wmd", "application/x-ms-wmd");
      AMIMEList.Add(".wms", "application/x-ms-wms");
      AMIMEList.Add(".wmz", "application/x-ms-wmz");
      AMIMEList.Add(".p12", "application/x-pkcs12");
      AMIMEList.Add(".p7b", "application/x-pkcs7-certificates");
      AMIMEList.Add(".p7r", "application/x-pkcs7-certreqresp");
      AMIMEList.Add(".qtl", "application/x-quicktimeplayer");
      AMIMEList.Add(".rtsp", "application/x-rtsp");
      AMIMEList.Add(".swf", "application/x-shockwave-flash");
      AMIMEList.Add(".sit", "application/x-stuffit");
      AMIMEList.Add(".tar", "application/x-tar");
      AMIMEList.Add(".man", "application/x-troff-man");
      AMIMEList.Add(".urls", "application/x-url-list");
      AMIMEList.Add(".zip", "application/x-zip-compressed");
      AMIMEList.Add(".cdf", "application/x-cdf");
      AMIMEList.Add(".fml", "application/x-file-mirror-list");
      AMIMEList.Add(".fif", "application/fractals");
      AMIMEList.Add(".spl", "application/futuresplash");
      AMIMEList.Add(".hta", "application/hta");
      AMIMEList.Add(".hqx", "application/max-binhex40");
      AMIMEList.Add(".doc", "application/msword");
      AMIMEList.Add(".pdf", "application/pdf");
      AMIMEList.Add(".p10", "application/pkcs10");
      AMIMEList.Add(".p7m", "application/pkcs7-mime");
      AMIMEList.Add(".p7s", "application/pkcs7-signature");
      AMIMEList.Add(".cer", "application/x-x509-ca-cert");
      AMIMEList.Add(".crl", "application/pkix-crl");
      AMIMEList.Add(".ps", "application/postscript");
      AMIMEList.Add(".sdp", "application/x-sdp");
      AMIMEList.Add(".setpay", "application/set-payment-initiation");
      AMIMEList.Add(".setreg", "application/set-registration-initiation");
      AMIMEList.Add(".siml", "application/smil");
      AMIMEList.Add(".ssm", "application/streamingmedia");
      AMIMEList.Add(".xfdf", "application/vnd.adobe.xfdf");
      AMIMEList.Add(".fdf", "application/vnd.fdf");
      AMIMEList.Add(".xls", "application/x-msexcel");
      AMIMEList.Add(".sst", "application/vnd.ms-pki.certstore");
      AMIMEList.Add(".pko", "application.vnd.ms-pki.pko");
      AMIMEList.Add(".cat", "application/vnd.ms-pki.seccat");
      AMIMEList.Add(".stl", "application/vnd.ms-pki.stl");
      AMIMEList.Add(".rmf", "application/vnd.rmf");
      AMIMEList.Add(".rm", "application/rnd.rn-realmedia");
      AMIMEList.Add(".rnx", "application/vnd.rn-realplayer");
      AMIMEList.Add(".rjs", "application/vnd.rn-realsystem-rjs");
      AMIMEList.Add(".rmx", "application/vnd.rn-realsystem-rmx");
      AMIMEList.Add(".rmp", "application/vnd.rn-rn_music_package");
      AMIMEList.Add(".rsml", "application/vnd.rn-rsml");
      AMIMEList.Add(".vsl", "x-cnet-vsl");
      AMIMEList.Add(".z", "application/x-compress");
      AMIMEList.Add(".tgz", "application/x-compressed");
      AMIMEList.Add(".dir", "application/x-director");
      AMIMEList.Add(".gz", "application/x-gzip");
      AMIMEList.Add(".uin", "application/z-icq");
      AMIMEList.Add(".hpf", "application/x-icq-hpf");
      AMIMEList.Add(".pnq", "application/x-icq-png");
      AMIMEList.Add(".scm", "application/x-icq-scm");
      AMIMEList.Add(".ins", "application/x-internet-signup");
      AMIMEList.Add(".iii", "application/x-iphone");
      AMIMEList.Add(".latex", "application/x-latex");
      AMIMEList.Add(".nix", "application/x-mix-transfer");
      // WAP
      AMIMEList.Add(".wbmp", "/image/vnd.wap.wbmp");
      AMIMEList.Add(".wml", "text/vnd.wap.wml");
      AMIMEList.Add(".wmlc", "application/vnd.wap.wmlc");
      AMIMEList.Add(".wmls", "text/vnd.wap.wmlscript");
      AMIMEList.Add(".wmlsc", "application/vnd.wap.wmlscriptc");
      // WEB
      AMIMEList.Add(".css", "text/css");
      AMIMEList.Add(".htm", "text/html");
      AMIMEList.Add(".html", "text/html");
      AMIMEList.Add(".shtml", "server-parsed-html");
      AMIMEList.Add(".sgm", "text/sgml");
      AMIMEList.Add(".sgml", "text/sgml");
	  }
	  
	  public MimeTable():this(true)
	  {
	  }
	  
	  public MimeTable(bool AutoFill)
	  {
      if (AutoFill)
      {
        BuildCache();
	    }
	  }
	  
	  public virtual void BuildCache()
	  {
	    if (_OnBuildCache != null)
	    {
	      _OnBuildCache(this, EventArgs.Empty);
	    }
	    if (mBackend.Count == 0)
	    {
	      BuildDefaultCache();
	    }
	  }
	  
	  public void AddMimeType(string Ext, string MIMEType)
	  {
      string LExt = Ext.ToLowerInvariant();
	    string LMIMEType;
	    if (LExt.Length == 0)
	    {
	      throw new IndyException(ResourceStrings.MIMEExtensionEmpty);
	    }
	    else
	    {
	      if (LExt[0] != '.')
	      {
	        LExt = "." + LExt;
	      }
	    }
	    LMIMEType = MIMEType.ToLowerInvariant();
	    if (LMIMEType.Length == 0)
	    {
        throw new IndyException(ResourceStrings.MIMEMIMETypeEmpty);
	    }
	    
	    if (!mBackend.ContainsKey(LExt))
	    {
        mBackend.Add(LExt, LMIMEType);
	    }
	    else
	    {
        throw new IndyException(ResourceStrings.MIMEExtAlreadyExists);
	    }
	  }
	  
	  public string GetFileMIMEType(string AFileName)
	  {
      string LExt = Path.GetExtension(AFileName).ToLowerInvariant();
	    if (mBackend.ContainsKey(LExt))
	    {
        return mBackend[LExt];
	    }
	    else
	    {
	      BuildCache();
	      if (!mBackend.ContainsKey(LExt))
	      {
	        return "application/octet-stream";
	      }
	      else
	      {
          return mBackend[LExt];
	      }
	    }
	  }
	  
	  public string GetDefaultFileExt(string MIMEType)
	  {
	    int Index;
      string LMimeType = MIMEType.ToLowerInvariant();
	    if (mBackend.ContainsValue(LMimeType))
	    {
        return mBackend.Keys[mBackend.IndexOfValue(LMimeType)];
	    }
	    else
	    {
	      BuildCache();
        Index = mBackend.IndexOfValue(LMimeType);
	      if (Index != -1)
	      {
          return mBackend.Keys[Index];
	      }	      
	    }
	    return "";
	  }

    public void Load(List<string> AStrings)
	  {
	    Load(AStrings, '=');
	  }

    public void Load(List<string> AStrings, char MimeSeparator)
	  {
	    int i = 0;
	    string Ext;
      mBackend.Clear();
	    while (i < AStrings.Count)
	    {
        Ext = AStrings[i].Substring(0, AStrings[i].IndexOf(MimeSeparator) - 1).ToLowerInvariant();
	      if (Ext.Length > 0)
	      {
	        if (!mBackend.ContainsKey(Ext))
	        {
	          AddMimeType(Ext, AStrings[i].Substring(AStrings[i].IndexOf(MimeSeparator) + 1));
	        }
	      }
	      i++;
	    }
	  }

    public void Save(List<string> AStrings)
	  {
	    Save(AStrings, '=');
	  }

    public void Save(List<string> AStrings, char MimeSeparator)
	  {
	    AStrings.Clear();
	    int i = 0;
	    while (i < mBackend.Count)
	    {
	      AStrings.Add(mBackend.Keys[i] + MimeSeparator + mBackend.Values[i]);
	      i++;
	    }
	  }

    public event EventHandler OnBuildCache
    {
      add
      {
        _OnBuildCache += value;
      }
      remove
      {
        _OnBuildCache -= value;
      }
    }
	}
}

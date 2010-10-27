using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Next2Friends.ImageWorks
{
    public class ImageCollection
    {
        public String Name;
        public String ID;

        public ImageCollection(String name, String id)
        {
            Name = name;
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class WebService
    {
        private const String uri = @"http://services.next2friends.com/N2FWebservices/PhotoOrganise.asmx/";
        private String sessionId;

        public WebService(String username, String password)
        {
            sessionId = Login(username, password);
        }

        private String Login(string username, string password)
        {
            String response = HttpPost("Login", "Email=" + username + "&Password=" + password);
            XElement element = XElement.Parse(response);

            return element.Value as String;
        }

        public List<ImageCollection> GetCollections()
        {
            String response = HttpPost("GetCollections", "");

            List<ImageCollection> collections = new List<ImageCollection>();

            XElement element = XElement.Parse(response);

            var collectionsXml = from collection in element.Descendants()
                                 where collection.Name.LocalName == "PhotoCollectionItem"
                                 select new
                                 {
                                     id = ((XElement)collection.FirstNode).Value,
                                     name = ((XElement)collection.LastNode).Value
                                 };

            foreach (var collection in collectionsXml)
            {
                collections.Add(new ImageCollection(collection.name, collection.id));
            }

            return collections;
        }

        public List<BitmapSource> GetImages(ImageCollection collection)
        {
            String response = HttpPost("GetPhotosByCollection", "WebPhotoCollectionID=" + collection.ID);

            List<BitmapSource> images = new List<BitmapSource>();

            XElement element = XElement.Parse(response);

            var imagesXml = from image in element.Descendants()
                            where image.Name.LocalName == "MainPhotoURL"
                         select image.Value;


            foreach (String image in imagesXml)
            {
                BitmapImage img = new BitmapImage(new Uri(image));
                images.Add(img);
            }

            return images;
        }

        string HttpPost (string uriSubstring, string parameters)
        { 
           // parameters: name1=value1&name2=value2	
           WebRequest webRequest = WebRequest.Create (uri + uriSubstring);

           if (sessionId != null)
           {
               webRequest.Headers.Add(HttpRequestHeader.Cookie, "ASP.NET_SessionId=" + sessionId);
           }

           webRequest.ContentType = "application/x-www-form-urlencoded";
           webRequest.Method = "POST";
           byte[] bytes = Encoding.ASCII.GetBytes (parameters);
           Stream os = null;
           try
           { // send the Post
              webRequest.ContentLength = bytes.Length;   //Count bytes to send
              os = webRequest.GetRequestStream();
              os.Write (bytes, 0, bytes.Length);         //Send it
           }
           catch (WebException ex)
           {
               System.Console.WriteLine(ex.Message);
           }
           finally
           {
              if (os != null)
              {
                 os.Close();
              }
           }
         
           try
           { // get the response
              WebResponse webResponse = webRequest.GetResponse();
              if (webResponse == null) 
                 { return null; }
              StreamReader sr = new StreamReader (webResponse.GetResponseStream());
              return sr.ReadToEnd ().Trim ();
           }
           catch (WebException ex)
           {
               System.Console.WriteLine(ex.Message);
           }
           return null;
        } // end HttpPost 
    }
}

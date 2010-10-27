using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Next2Friends.CrossPoster.Client.Engines;
using Next2Friends.CrossPoster.Client.Logic;
using Google.GData.Client;


namespace Next2Friends.Data
{
    public partial class Blog
    {
        public static bool PostToBlog(BlogType blogType)
        {
            bool LoginOK = false;

            //switch (blogType)
            //{
            //    case BlogType.Blogger:
            //        bDescr.BlogType = BlogType.Blogger;
            //        bDescr.Address = "http://www.blogger.com";
            //        Service service = new Service("blogger", "");
            //        service.Credentials = new GDataCredentials(Username, Password);
            //        Uri blogPostUri = SelectUserBlog(service);
            //        AtomEntry createdEntry = PostNewEntry(service, blogPostUri, Title, Body);
            //        LoginOK = true;
            //        break;

            //    case BlogType.WordPress:
            //        bDescr.BlogType = BlogType.WordPress;
            //        bDescr.Address = WPAddress;

            //        WordPressEngine wpe = new WordPressEngine();
            //        wpe.PublishNewEntry(bDescr, Title, Body);
            //        LoginOK = true;
            //        break;

            //    case BlogType.LiveJournal:
            //        bDescr.BlogType = BlogType.LiveJournal;
            //        bDescr.Address = "http://www.livejournal.com";

            //        LiveJournalEngine lje = new LiveJournalEngine();
            //        lje.PublishNewEntry(bDescr, Title, Body);
            //        LoginOK = true;
            //        break;

            //    default:
            //        break;
            //}

            return LoginOK;
        }
    }
}

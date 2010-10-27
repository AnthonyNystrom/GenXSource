using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Next2Friends.Data;

namespace Next2Friends.Misc
{
    public class SEOSiteMap
    {
        private StringBuilder sbMainSiteMap = new StringBuilder();
        private StringBuilder sbSiteMap = new StringBuilder();
        public int NumberOfSiteMaps = 0;
        public string SiteMapFileLocation = @"c:\sitemap.xml";


        public SEOSiteMap(string SiteMapFileLocation)
        {
            NewSiteMap();

            AddPrimaryPages();
            AddVideosToSiteMap();
            //AddPhotosToSiteMap();
            AddBlogsToSiteMap();
            AddProfilesToSiteMap();
            
            //sbMainSiteMap.AppendLine(@"<sitemapindex xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>");


//            for (int i = 0; i < NumberOfSiteMaps+1; i++)
//            {

//                sbMainSiteMap.AppendFormat(@"<sitemap>
//                                              <loc>http://www.next2friends.com/sitemap{0}.xml</loc>
//                                              <lastmod>2004-10-01T18:23:17+00:00</lastmod>
//                                           </sitemap>", i);
//            }

            SiteMapFileLocation = this.SiteMapFileLocation;

            //sbMainSiteMap.AppendLine(@"</sitemapindex>");

            ArchiveSiteMap();
        }

        private void ArchiveSiteMap()
        {
            sbSiteMap.AppendLine(@"</urlset>");

            ToFile(SiteMapFileLocation);


            NewSiteMap();

        }

        private void NewSiteMap()
        {
            NumberOfSiteMaps++;

            sbSiteMap = new StringBuilder();

            sbSiteMap.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sbSiteMap.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

        }

        private void AddVideosToSiteMap()
        {
            List<Video> videos = Video.GetAllVideo();

            object[] parameters = new object[2];

            for (int i = 0; i < videos.Count; i++)
            {
                parameters[0] = "http://www.next2friends.com/video/" + RegexPatterns.FormatStringForURL(videos[i].Title)  + "/" + videos[i].WebVideoID + "/";
                //parameters[1] = videos[i].DTCreated.ToString("yyyy-mm-dd");
                parameters[1] = DateTime.Now.ToString("yyyy-MM-d");

                sbSiteMap.AppendFormat(@"<url>
                  <loc>{0}</loc>
                  <lastmod>{1}</lastmod>
                  <changefreq>weekly</changefreq>
                  <priority>0.9</priority>
               </url>", parameters);
            }
        }

        private void AddBlogsToSiteMap()
        {
            List<BlogEntry> blogEntrys = BlogEntry.GetAllBlogEntry();

            object[] parameters = new object[2];

            for (int i = 0; i < blogEntrys.Count; i++)
            {
                parameters[0] = "http://www.next2friends.com/blog/" + blogEntrys[i].WebBlogEntryID + "/" + RegexPatterns.FormatStringForURL(blogEntrys[i].Title) + "/";
                parameters[1] = blogEntrys[i].DTCreated.ToString("yyyy-MM-d");

                sbSiteMap.AppendFormat(@"<url>
                  <loc>{0}</loc>
                  <lastmod>{1}</lastmod>
                  <changefreq>weekly</changefreq>
                  <priority>0.8</priority>
               </url>", parameters);
            }
        }

        private void AddPrimaryPages()
        {
            string DTNow = DateTime.Now.ToString("yyyy-MM-d");

            List<string> Pages = new List<string>();
            Pages.Add("http://www.next2friends.com/");
            Pages.Add("http://www.next2friends.com/video");
            Pages.Add("http://www.next2friends.com/ask");
            Pages.Add("http://www.next2friends.com/community");
            Pages.Add("http://www.next2friends.com/signup");

            object[] parameters = new object[2];

            for (int i = 0; i < Pages.Count; i++)
            {
                parameters[0] = Pages[i];
                parameters[1] = DTNow;

                sbSiteMap.AppendFormat(@"<url>
                                          <loc>{0}</loc>
                                          <lastmod>{1}</lastmod>
                                          <changefreq>weekly</changefreq>
                                          <priority>1</priority>
                                       </url>", parameters);
            }

        }

        private void AddProfilesToSiteMap()
        {
            List<Member> members = Member.GetAllMember();

            object[] parameters = new object[2];

            for (int i = 0; i < members.Count; i++)
            {
                parameters[0] = "http://www.next2friends.com/users/" + members[i].NickName + "/";
                //parameters[1] = members[i].CreatedDT.ToString("yyyy-mm-dd");
                parameters[1] = DateTime.Now.ToString("yyyy-MM-d");

                sbSiteMap.AppendFormat(@"<url>
                  <loc>{0}</loc>
                  <lastmod>{1}</lastmod>
                  <changefreq>weekly</changefreq>
                  <priority>0.8</priority>
               </url>", parameters);
            }
        }

        private void AddPhotosToSiteMap()
        {

            List<Photo> photos = Photo.GetAllPhoto();

            object[] parameters = new object[2];

            for (int i = 0; i < photos.Count; i++)
            {
                if (sbSiteMap.Length > 9000000)
                {
                    ArchiveSiteMap();
                }

                string Title = (photos[i].Caption != string.Empty) ? RegexPatterns.FormatStringForURL(photos[i].Caption) : RegexPatterns.FormatStringForURL(photos[i].PhotoCollectionDescription)+"/";
                parameters[0] = "http://www.next2friends.com/video/" + photos[i].WebPhotoID + "/" + Title + "/";
                parameters[1] = photos[i].CreatedDT.ToString("yyyy-MM-d");

                sbSiteMap.AppendFormat(@"<url>
                  <loc>{0}</loc>
                  <lastmod>{1}</lastmod>
                  <changefreq>weekly</changefreq>
                  <priority>0.8</priority>
               </url>", parameters);
            }
        }

        #region Output

        public override string ToString()
        {
            return sbSiteMap.ToString();
        }

        public void ToFile(string Path)
        {
            FileInfo t = new FileInfo(Path);
            StreamWriter Tex = t.CreateText();
            Tex.WriteLine(sbSiteMap.ToString());
            Tex.Write(Tex.NewLine);
            Tex.Close();
        }

        #endregion
    }
}

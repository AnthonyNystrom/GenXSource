/* ------------------------------------------------
 * BloggerEngine.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using Next2Friends.CrossPoster.Client.Logic;
using gClient = Google.GData.Client;
using Next2Friends.CrossPoster.Client.Google;
using System.Windows.Forms;

namespace Next2Friends.CrossPoster.Client.Engines
{
    sealed class BloggerEngine : IBlogEngine
    {
        private IDictionary<BlogDescriptor, String> _descriptor2FeedUriMap;

        /// <summary>
        /// Creates a new instance of the <code>BloggerEngine</code> class.
        /// </summary>
        public BloggerEngine()
        {
            _descriptor2FeedUriMap = new Dictionary<BlogDescriptor, String>();
        }

        public IList<BlogEntryDescriptor> GetBlogEntries(BlogDescriptor blogDescriptor)
        {
            if (blogDescriptor == null)
                throw new ArgumentNullException("blogDescriptor");

            try
            {
                return GetBlogEntries(blogDescriptor, GetFeedUri(blogDescriptor));
            }
            catch (gClient.GDataRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
                return new List<BlogEntryDescriptor>();
            }
        }

        public void PublishNewEntry(BlogDescriptor blogDescriptor, String title, String content)
        {
            if (blogDescriptor == null)
                throw new ArgumentNullException("blogDescriptor");
            if (String.IsNullOrEmpty(blogDescriptor.Username))
                throw new ArgumentException("blogDescriptor.Username cannot be null or an empty string.");

            var entry = new gClient.AtomEntry();
            entry.Content.Content = content;
            entry.Content.Type = "html";
            entry.Title.Text = title;

            var service = new gClient.Service("blogger", GoogleSucks.GetApplicationName());
            service.Credentials = new gClient.GDataCredentials(blogDescriptor.Username, blogDescriptor.Password);
            service.Insert(new Uri(GetFeedUri(blogDescriptor)), entry);
        }

        private String GetFeedUri(BlogDescriptor blogDescriptor)
        {
            String feedUri = null;
            var blogDescriptorExists = _descriptor2FeedUriMap.Keys.Contains(blogDescriptor);

            if (blogDescriptorExists)
                feedUri = _descriptor2FeedUriMap[blogDescriptor];

            if (String.IsNullOrEmpty(feedUri))
            {
                feedUri = GetFeedUriFromQuery(blogDescriptor);

                if (blogDescriptorExists)
                    _descriptor2FeedUriMap[blogDescriptor] = feedUri;
                else
                    _descriptor2FeedUriMap.Add(blogDescriptor, feedUri);
            }

            return feedUri;
        }

        private static IList<BlogEntryDescriptor> GetBlogEntries(BlogDescriptor blogDescriptor, String feedUri)
        {
            IList<BlogEntryDescriptor> result = new List<BlogEntryDescriptor>();

            if (feedUri != null)
            {
                var query = new gClient.FeedQuery(feedUri);
                var service = new gClient.Service("blogger", GoogleSucks.GetApplicationName())
                {
                    Credentials = new gClient.GDataCredentials(blogDescriptor.Username, blogDescriptor.Password)
                };

                var bloggerFeed = service.Query(query);

                while (bloggerFeed != null && bloggerFeed.Entries.Count > 0)
                {
                    foreach (gClient.AtomEntry entry in bloggerFeed.Entries)
                    {
                        result.Add(
                            new BlogEntryDescriptor()
                            {
                                Content = entry.Content.Content,
                                Subject = entry.Title.Text,
                                Sender = entry.Authors[0].Name,
                                Date = entry.Published
                            });
                    }

                    if (bloggerFeed.NextChunk != null)
                    {
                        query.Uri = new Uri(bloggerFeed.NextChunk);
                        bloggerFeed = service.Query(query);
                    }
                    else
                    {
                        bloggerFeed = null;
                    }
                }
            }

            return result;
        }

        private static String GetFeedUriFromQuery(BlogDescriptor blogDescriptor)
        {
            var query = new gClient.FeedQuery(blogDescriptor.Address);
            var service = new gClient.Service("blogger", GoogleSucks.GetApplicationName())
            {
                Credentials = new gClient.GDataCredentials(blogDescriptor.Username, blogDescriptor.Password)
            };

            var bloggerFeed = service.Query(query);
            var feeds = new List<gClient.AtomEntry>();

            while (bloggerFeed != null && bloggerFeed.Entries.Count > 0)
            {
                foreach (gClient.AtomEntry entry in bloggerFeed.Entries)
                    feeds.Add(entry);

                if (bloggerFeed.NextChunk != null)
                {
                    query.Uri = new Uri(bloggerFeed.NextChunk);
                    bloggerFeed = service.Query(query);
                }
                else
                {
                    bloggerFeed = null;
                }
            }

            foreach (gClient.AtomEntry entry in feeds)
                if (entry != null)
                    foreach (gClient.AtomLink link in entry.Links)
                        if (link.Rel == gClient.BaseNameTable.ServiceFeed)
                            return link.HRef.ToString();

            return null;
        }
    }
}

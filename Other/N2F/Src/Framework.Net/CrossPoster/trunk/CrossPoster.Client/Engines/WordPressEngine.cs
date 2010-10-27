using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Next2Friends.CrossPoster.Client.Logic;
using Next2Friends.CrossPoster.Client.MetaWeblog;
using System.Net;

namespace Next2Friends.CrossPoster.Client.Engines
{
    sealed class WordPressEngine : IBlogEngine
    {
        public WordPressEngine()
        {
        }

        public IList<BlogEntryDescriptor> GetBlogEntries(BlogDescriptor blogDescriptor)
        {
            var gateway = GetGateway(blogDescriptor);
            var result = new List<BlogEntryDescriptor>();
            var blogId = GetBlogId(gateway, blogDescriptor);

            if (!String.IsNullOrEmpty(blogId))
            {
                var posts = gateway.getRecentPosts(
                    blogId,
                    blogDescriptor.Username,
                    blogDescriptor.Password,
                    20);

                foreach (var post in posts)
                {
                    result.Add(
                        new BlogEntryDescriptor()
                        {
                            Content = post.description,
                            Date = post.dateCreated,
                            Sender = blogDescriptor.Username,
                            Subject = post.title 
                        });
                }
            }
            
            return result;
        }

        public void PublishNewEntry(BlogDescriptor blogDescriptor, String title, String content)
        {
            var gateway = GetGateway(blogDescriptor);
            var blogId = GetBlogId(gateway, blogDescriptor);
            if (!String.IsNullOrEmpty(blogId))
            {
                gateway.newPost(
                    blogId,
                    blogDescriptor.Username,
                    blogDescriptor.Password,
                    new Post()
                    {
                        categories = new[] { "Uncategorized" },
                        dateCreated = DateTime.Now,
                        description = content,
                        title = title
                    },
                    true);
            }
        }

        private static String GetBlogId(MetaWeblogGateway gateway, BlogDescriptor blogDescriptor)
        {
            var blogs = gateway.getUsersBlogs("", blogDescriptor.Username, blogDescriptor.Password);
            if (blogs != null && blogs.Length > 0)
                return blogs[0].blogid;
            return null;
        }

        private static MetaWeblogGateway GetGateway(BlogDescriptor blogDescriptor)
        {
            return new MetaWeblogGateway()
            {
                Credentials = new NetworkCredential(blogDescriptor.Username, blogDescriptor.Password),
                KeepAlive = false,
                Url = blogDescriptor.Address
            };
        }
    }
}

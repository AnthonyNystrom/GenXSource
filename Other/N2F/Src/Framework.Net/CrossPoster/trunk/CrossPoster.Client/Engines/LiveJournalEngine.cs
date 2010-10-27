using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Next2Friends.CrossPoster.Client.Logic;
using Next2Friends.CrossPoster.Client.LiveJournal;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.Engines
{
    sealed class LiveJournalEngine : IBlogEngine
    {
        private ILiveJournalClient _gateway;

        private ILiveJournalClient GetGateway()
        {
            if (_gateway == null)
                _gateway = XmlRpcProxyGen.Create<ILiveJournalClient>();
            return _gateway;
        }

        public IList<BlogEntryDescriptor> GetBlogEntries(BlogDescriptor blogDescriptor)
        {
            var result = new List<BlogEntryDescriptor>();

            foreach (var item in GetGateway().GetEvents(
                new GetEventsRequest()
                {
                    Username = blogDescriptor.Username,
                    Password = blogDescriptor.Password,
                    HowMany = 20,
                    LineEndings = GetLineEndings(),
                    SelectType = "lastn",
                    NoProps = true
                }).Events)
            {
                result.Add(
                    new BlogEntryDescriptor()
                    {
                        Content = item.Content,
                        Sender = String.IsNullOrEmpty(item.Poster) ? blogDescriptor.Username : item.Poster,
                        Subject = item.Subject,
                        Date = DateTime.Parse(item.EventTime)
                    });
            }

            return result;
        }

        public void PublishNewEntry(BlogDescriptor blogDescriptor, String title, String content)
        {
            var now = DateTime.Now;

            GetGateway().PostEvent(
                new PostRequest()
                {
                    Username = blogDescriptor.Username,
                    Password = blogDescriptor.Password,
                    LineEndings = GetLineEndings(),
                    Subject = title,
                    Content = content,
                    Year = now.Year,
                    Month = now.Month,
                    Day = now.Day,
                    Hour = now.Hour,
                    Minute = now.Minute
                });
        }

        private static String GetLineEndings()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix ? "unix" : "pc";
        }
    }
}

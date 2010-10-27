using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Next2Friends.Data;
using Next2Friends.Misc;

namespace LawTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Banner banner = Banner.GetNextBanner(BannerType.PageBanner, "http://www.next2friends.com/index.aspx");
        }
    }
}

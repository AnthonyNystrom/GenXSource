using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;


namespace Next2Friends.Data
{
    /// <summary>
    /// Summary description for ParalellServer
    /// </summary>
    public class ParallelServer
    {
        private static string[] Servers = new string[] { @"http://www.n2fstaticx1.com/",
                                                        @"http://www.n2fstaticx2.com/",
                                                        @"http://www.n2fstaticx3.com/"};

        //private static string[] Servers = new string[] { @"http://www.next2friends.com/" };                                      

        public ParallelServer()
        {

        }

        /// <summary>
        /// Alternates between servers
        /// </summary>
        /// <returns></returns>
        public static string Get()
        {

            return Get("999");

       }

        /// <summary>
        /// Alternates between servers
        /// </summary>
        /// <returns></returns>
        public static string Get(string AnyUniqueID)
        {
            int Seed = 0;
            int index = 0;

            if (AnyUniqueID.Length > 0)
            {
                byte[] bytes = ASCIIEncoding.Default.GetBytes(AnyUniqueID);

                for (int i = 0; i < bytes.Length; i++)
                {
                    Seed += bytes[i];
                }

                Random r = new Random(Seed);
                index = r.Next(0, 2);
            }

            return Servers[index];
        }
    }
}
